﻿
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
using WEBAPI.Constants;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Models.DTO_s;
using System.Formats.Tar;
using Castle.DynamicProxy.Generators;

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


   

        public void Add(Users user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public async Task<Users> GetByMail(string email)
        {

            return  _context.Users.SingleOrDefault(e => e.email == email);
        }

        //public Task<bool> IsAdmin(string email)
        //{
        //    var result = GetByMail(email);
        //    if (result == null) return Task.FromResult(false);
        //    if (result.type == "user")
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
           
           
        //}

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

        public async Task<UserForProfileInfo> GetUserProfileInfo(int id)
        {
           
            var user = await _context.Users.FirstOrDefaultAsync(u => u.user_id == id);

         
            if (user == null)
            {
                return null; 
            }

           
            int followersCount = await _context.Followers
                .Where(x => x.follower_id == id)
                .CountAsync();

            int followedCount = await _context.Followers
                .Where(x => x.followed_id == id)
                .CountAsync();

            int entriesCount = await _context.Entries
                .Where(e => e.user_id == id)
                .CountAsync();

            UserForProfileInfo info = new UserForProfileInfo()
            {
                followednumber = followedCount,
                username = user.name,
                followernumber = followersCount,
                totalentrynumber = entriesCount,
            };

            return info;
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

      

        public async Task<string> GetUserNameById(int id)
        {
            var result = await (from x in _context.Users
                                where x.user_id == id
                                select x.name)
                                .FirstOrDefaultAsync(); // LINQ sorgusunu çalıştır ve ilk sonucu al

            return result.ToString();
        }

     

        //işaretleyerek silme
        public async Task DeleteAllMsg(int userid, int senderid)
        {
            var result = from x in _context.Msg
                         where x.msg_sender_id == userid && x.msg_receiver_id == senderid
                         select x; 
            _context.Remove(result);
            _context.SaveChanges(); 


        }
        //bütün olarak silme --> msj thumbnailden hepsini silme
       


        public async Task<List<GetMsgThumbnail>> ReceiveThumbnailMessages(int userId)
        {
            var latestMessages = await (from msg in _context.Msg
                                        join user in _context.Users on msg.msg_sender_id equals user.user_id
                                        where msg.msg_receiver_id == userId
                                        group new { msg, user } by user.name into messageGroup
                                        select new
                                        {
                                            SenderName = messageGroup.Key,
                                            LatestMessage = messageGroup.OrderByDescending(m => m.msg.msg_date).FirstOrDefault()
                                        })
                                       .ToListAsync(); // Farklı kişilerden gelen son mesajları gruplayarak al

            var thumbnails = latestMessages.Select(async m =>
            {
                if (m.LatestMessage != null)
                {
                    var latestMsg = m.LatestMessage.msg;
                    var senderId = m.LatestMessage.user.user_id; // Eksik olan senderid burada alınacak

                    return new GetMsgThumbnail
                    {
                        Sendername = m.SenderName,
                        IsOpen = latestMsg.isOpened,
                        Lastmsg = latestMsg.msg_detail,
                        Msgdate = latestMsg.msg_date,
                        MsgCount =  GetMsgCounts(userId, senderId), // GetMsgCount işlemini asenkron olarak çağırın
                        Receivername = _context.Users.FirstOrDefault(u => u.user_id == userId)?.name
                    };
                }
                return null;
            });

            var thumbnailsList = await Task.WhenAll(thumbnails);

            return thumbnailsList.OrderByDescending(m => m.Msgdate).ToList();
        }

        private int GetMsgCounts(int userId, int senderId)
        {
            var result =  _context.Msg
                .Where(x => x.msg_receiver_id == userId && x.msg_sender_id == senderId)
                .Count();

            return result;
        }


        public async Task<List<Msg>> GetAllMessagesBetweenUsers(int userId1, int userId2)
        {
                var messagesFromUser1ToUser2 = await _context.Msg
                    .Where(x => x.msg_sender_id == userId1 && x.msg_receiver_id == userId2 && x.msg_date != null)
                    .OrderBy(x => x.msg_date)
                    .ToListAsync();

                var messagesFromUser2ToUser1 = await _context.Msg
                    .Where(x => x.msg_sender_id == userId2 && x.msg_receiver_id == userId1 && x.msg_date != null)
                    .OrderBy(x => x.msg_date)
                    .ToListAsync();

                var allMessagesBetweenUsers = messagesFromUser1ToUser2
                    .Concat(messagesFromUser2ToUser1)
                    .OrderBy(x => x.msg_date)
                    .ToList();

                return allMessagesBetweenUsers;
            }

        public async Task UserActive(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.email == email);
            if (result != null)
            {
                result.isActive = true;
                _context.Users.Update(result);
                await _context.SaveChangesAsync();
            }
        }
    }
}