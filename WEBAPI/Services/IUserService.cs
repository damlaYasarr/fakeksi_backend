using WEBAPI.Models;

namespace WEBAPI.Services
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUsers();
        Task<Users?> GetSingleUsrs(int id);
        Task<List<Users>> AddUser(Users hero);
        Task<List<Users>?> UpdateUsrs(int id, Users request);
        Task<List<Users>?> DeleteUser(int id);
    }
}
