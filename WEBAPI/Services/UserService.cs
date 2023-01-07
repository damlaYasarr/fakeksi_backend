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

        public Task<List<Users>> AddUser(Users hero)
        {
            throw new NotImplementedException();
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

        public Task<List<Users>?> UpdateUsrs(int id, Users request)
        {
            throw new NotImplementedException();
        }
    }
}
