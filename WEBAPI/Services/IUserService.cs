using WEBAPI.Models;

namespace WEBAPI.Services
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUsers();
        Task<Users?> GetSingleUsrsById(int id);
        Task<List<Users>> AddUser(Users hero);
        Task<Users?> Login(int id, string email, string password);
        Task<Users?> Logout(int id);
        Task<Users?> Addfollower(int benim, int otheruser);
        Task<Users?> DeleteFollower(int benim, int otheruser);
        Task<List<Users>?> UpdateUsrs(int id, Users request);
        Task<List<Users>?> DeleteUser(int id);
        //Task<int?> getfollowersid(int benim, int otheruser);
        Task<List<Users>?> GetAllFollower(int id);
    }
}
