
using WEBAPI.Models;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Utilities.Security.JWT;

namespace WEBAPI.Services
{
    public interface IUserService
    {
        Task<List<Users>> GetAllUsers();
        Task<Users?> GetSingleUsrsById(int id);
       // Task<List<Users>> AddUser(Users hero);
        //Task<Users?> Login(string email, string password);
        Task<Users?> Logout(int id);
        Task<Users?> Addfollower(int benim, int otheruser);
        Task<Users?> DeleteFollower(int benim, int otheruser);
        Task<List<Users>?> UpdateUsrs(int id, Users request);
        Task<List<Users>?> DeleteUser(int id);
        Task<List<string>> GetAllFollower(int id);
        Task<List<string>> GetAllFollowed(int id);

        Task SendMessage(string user, string message);
        Task ReceiveMessage(string user, string message);
       
        
        bool IsAdmin(string email);



        List<OperationClaim> GetOperationClaims(Users user);
        void Add(Users user);
        Users GetByMail(string email);
    }
}
