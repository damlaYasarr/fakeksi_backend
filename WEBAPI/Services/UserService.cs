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

        /*  public async Task<List<Users>> AddUser(Users hero)
          {    
              hero.isActive = false;
             _context.Users.Add(hero);
               await _context.SaveChangesAsync();
              return await _context.Users.ToListAsync();
          }*/

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


        public Task<List<Users>?> UpdateUsrs(int id, Users request)
        {
            throw new NotImplementedException();
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
        public async Task SendMessage(string user, string message)
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


       

        public Task ReceiveMessage(string user, string message)
        {
            throw new NotImplementedException();
        }


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
    }
}