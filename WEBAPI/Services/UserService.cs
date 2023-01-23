﻿using Azure.Core;
using Microsoft.EntityFrameworkCore;
using WEBAPI.Data;
using WEBAPI.Models;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace WEBAPI.Services
{
    public class UserService : IUserService
    {
      
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<Users?> AddFollower(int benim, int otheruser)
        {
            var getuser = await GetSingleUsrsById(otheruser);
            //Console.WriteLine(getuser.user_id);
            var result = _context.Follower.SingleOrDefault(e => e.followed_id == getuser.user_id);
            if (result == null)
            {
                Followers followers = new Followers()
                {
                    //benim takip ettiğime ekledim. bense onun follower'i olmalıım.
                    followed_id = otheruser, //id takip ettiğim kişi
                    follower_id = benim,
                };

                _context.Follower.Add(followers);
                await _context.SaveChangesAsync();
            }
            return getuser;



        }

        public async Task<Users?> DeleteFollower(int benim, int otheruser)
        {
            var getuser = await GetSingleUsrsById(otheruser);
            //Console.WriteLine(getuser.user_id);
            var result = _context.Follower.SingleOrDefault(e => e.followed_id == getuser.user_id);
            if (result != null)
            {
                var result=from follower in _context.Follower 

   //bişey delete olacak
                _context.Follower.Add(followers);
                await _context.SaveChangesAsync();
            }
            return getuser;

            ///bişey

        }

        public async Task<List<Users>> AddUser(Users hero)
        {    
            hero.isActive = false;
           _context.Users.Add(hero);
             await _context.SaveChangesAsync();
            return await _context.Users.ToListAsync();
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

        public async Task<Users?> Login(int id, string email, string password)
        {
            var result = _context.Users.SingleOrDefault(e => e.user_id == id); 
            if (result == null)
            {
                return null;
            }
            else
            {
                if(result.email == email & result.password==password) {
                   
                        result.isActive = true;
                        await _context.SaveChangesAsync();
                    
                }
            }
                
            return result;
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

        public Task<Users?> Addfollower(int benim, int otheruser)
        {
            throw new NotImplementedException();
        }
    }
}