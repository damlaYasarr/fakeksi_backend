using WEBAPI.Models;

namespace WEBAPI.Services
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUsers();
        Task<Users?> GetSingleUsrs(int id);
        Task<List<Users>> AddUser(Users hero);
        Task<Users?> Login(int id, string email, string password);
        Task<Users?> Logout(int id);
        Task<Users?> Addfollower(int id);
        Task<Users?> DeleteFollower(int id);
        Task<List<Users>?> UpdateUsrs(int id, Users request);
        Task<List<Users>?> DeleteUser(int id);
    }
}
