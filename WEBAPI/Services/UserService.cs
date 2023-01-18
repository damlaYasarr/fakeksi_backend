using Microsoft.EntityFrameworkCore;
using WEBAPI.Data;
using WEBAPI.Models;

namespace WEBAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> AddUser(Users hero)
        {
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

        public Task<Users?> GetSingleUsrs(int id)
        {
            throw new NotImplementedException();
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

        public async Task<Users?> Logout(Users user)
        {     
            user.isActive = false;
            await _context.SaveChangesAsync();
            return user;
         
        }


        public Task<List<Users>?> UpdateUsrs(int id, Users request)
        {
            throw new NotImplementedException();
        }
    }
}