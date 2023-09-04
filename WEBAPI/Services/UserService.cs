
using Microsoft.EntityFrameworkCore;
using WEBAPI.Data;
using WEBAPI.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.AccessControl;
using Newtonsoft.Json;
using System.Text;
using WEBAPI.Utilities.Security.JWT;
using WEBAPI.Constants;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Utilities.Security.Hashing;

namespace WEBAPI.Services
{
    public class UserService : IUserService
    {
      
        private readonly DataContext _context;
       
     
        public UserService(DataContext context)
        {
            _context = context;
          
        }
        public async Task<Users?> Addfollower(int benim, int otheruser)

        {
            var getuser = await GetSingleUsrsById(otheruser);
            //Console.WriteLine(getuser.user_id);
            var result = _context.Followers.SingleOrDefault(e => e.followed_id == getuser.user_id);
            if (result == null)
            {
                Followers followers = new Followers()
                {
                    //benim takip ettiğime ekledim. bense onun follower'i olmalıım.
                    followed_id = otheruser, //id takip ettiğim kişi
                    follower_id = benim,
                };

                _context.Followers.Add(followers);
                await _context.SaveChangesAsync();
            }
            return getuser;



        }

        public async Task<Users?> DeleteFollower(int benim, int otheruser)
        {
            //deneyelim
            var getuser = await GetSingleUsrsById(otheruser);
            
            var result = _context.Followers.SingleOrDefault(e => e.followed_id == getuser.user_id);
         
             var xx = from r in _context.Followers
                         where r.follower_id == benim && r.followed_id == otheruser
                         select r;
            int k = 0;
               foreach (var t in xx)
            {
                 k = t.id;
                
            }
            

             _context.Followers.Remove(Finduser(k));
             _context.SaveChanges();
            return getuser;

        

        }
        private  Followers Finduser(int id)
        {   
            var result= _context.Followers.SingleOrDefault(e => e.id == id);
            if (result == null){
                return null;
            }
            return result;
        }

  

        public Task<List<Users>?> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Users>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<Users?> GetSingleUsrsById(int id)
        {
            var person=await _context.Users.FindAsync(id);
            if (person == null)
            {
                return null;
            }
            return person;
        }



        public async Task<Users?> Logout(int id)
        {     
            var result= _context.Users.SingleOrDefault(e => e.user_id == id);
            result.isActive = false;
            await _context.SaveChangesAsync();
            return result;
         
        }


        public Task<Users> UpdateUsrs(int id, Users request)
        {
            var result = _context.Users.SingleOrDefault(e => e.user_id == id); 
            if(result != null) {
               _context.Users.Update(request);
            }
            return null;
        }

       

        public  Task<List<string>> GetAllFollower(int id)
        {
           //there is a mistake but it works correctly.
            var result = from r in _context.Users
                         join x in _context.Followers on id equals x.follower_id
                         where x.followed_id==r.user_id
                         select r.name;
            if (result == null)
            {
                return null;
            }

            List<string> usr = new();
            foreach(string x in result)
            {
                usr.Add(x);
            }
            return Task.FromResult(usr);
            


        }
      
        public Task<List<string>> GetAllFollowed(int id)
        {
            var result = from r in _context.Users
                         join x in _context.Followers on id equals x.followed_id
                         where x.follower_id == r.user_id
                         select r.name;
            if (result == null)
            {
                return null;
            }

            List<string> usr = new();
            foreach (string x in result)
            {
                usr.Add(x);
            }
            return Task.FromResult(usr);

        }
      /*  public async Task SendMessage(string user, string message)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { User = user, Message = message }), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://your-api-url/chat", content);

                if (response.IsSuccessStatusCode)
                {
                    // Message was successfully sent
                }
                else
                {
                    // Handle error
                }
            }
        }
      */
     

        public List<OperationClaim> GetOperationClaims(Users user)
        {
                var result = from operationClaim in _context.OperationClaim
                             join userOperationClaim in _context.UserOperationClaim
                                 on operationClaim.Id equals userOperationClaim.operationid
                             where userOperationClaim.Userid == user.user_id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();

            
        }

        public void Add(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public Users GetByMail(string email)
        {

            return _context.Users.SingleOrDefault(e => e.email == email);
        }

        public bool IsAdmin(string email)
        {
            var result = GetByMail(email);
            if (result == null) return false;
            if (result.type=="user")
            {
                return false;
            }
            else
            {
                return true;
            }
           
           
        }

        public Task<int> getUserIdByEmail(string email)
        {
            var result= from x in _context.Users
                        where x.email == email
                        select x;
            if (result == null) { return null; }
            int k = 0;
            foreach(var x in result)
            {
                k = x.user_id;
            }
            return Task.FromResult(k);
        }

        public Task<UserForProfileInfo> GetUserProfileInfo(int id)
        {
            var result = from user in _context.Users
                               where user.user_id == id
                               select user;
            string name="";
            foreach(var t in result) { name = t.name; }

            //Kişinin toplam takipçi sayısı follower tablosundan alınır
            int followersCount = (from x in _context.Followers
                                 where x.follower_id == id
                                 select x).Count();
            if(followersCount ==null) { followersCount = 0; }

            //Kişinin toplam takip edilen sayısı follower tablosundan alınır
            int followed = (from x in _context.Followers
                                  where x.followed_id == id
                                  select x).Count();
            if (followed == null) { followed = 0; }
            //Kişinin toplam entry sayısı entry tablosundan alınır
            int entriesCount = (from e in _context.Entries
                               where e.user_id == id
                                select e).Count();
            if (entriesCount == null) { entriesCount = 0; }
            UserForProfileInfo info = new UserForProfileInfo()
            {
               followednumber = followed,
               username=name,
               followernumber=followersCount,
               totalentrynumber=entriesCount,
            };


            return Task.FromResult(info);
        }

        public Task<int> getUserIdByName(string name)
        {
            var result = from x in _context.Users
                         where x.name == name
                         select x;
            if (result == null) { return null; }
            int k = 0;
            foreach (var x in result)
            {
                k = x.user_id;
            }
            return Task.FromResult(k);
        }
        public async Task<List<string>> ReceiveMessage(int userid, int otherid)
        {
            var result = from x in _context.Msg
                         where x.msg_receiver_id == userid && x.msg_sender_id == otherid
                         select x;
            List<string> msg= new List<string>();

            foreach (var x in result)
            {
                if (x.isOpened == false)
                {
                    
                    msg.Add(x.msg_detail);
                    x.isOpened = true;
                    _context.Msg.Update(x);
                }
               
            }

            await _context.SaveChangesAsync();

            return msg;
           
        }


        public async Task<Msg> SendMessage(int userid, int otherid, string msg)
        {
            if (userid == otherid)
            {
                return null;
            }
            if (msg == "")
            {
                return null;
            }
            DateTime dt = DateTime.Now; // Or whatever
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");
            var newmsg = new Msg() {
                msg_date = s,
                msg_detail = msg,
                isOpened = false,
                msg_receiver_id=otherid,
                msg_sender_id=userid
            };
            _context.Msg.Add(newmsg);
            await _context.SaveChangesAsync();

            return newmsg;

        }

        public List<Msg> getAllMssgies(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserNameById(int id)
        {
            var result = await (from x in _context.Users
                                where x.user_id == id
                                select x.name)
                                .FirstOrDefaultAsync(); // LINQ sorgusunu çalıştır ve ilk sonucu al

            return result;
        }

        public async Task<int> GetMsgCount(int userid, int senderid)
        {
            var result = await _context.Msg
        .Where(x => x.msg_receiver_id == userid && x.msg_sender_id == senderid)
        .CountAsync();
                return result;
        }
    }
}