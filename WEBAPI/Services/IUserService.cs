
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
        Task<UserForProfileInfo> GetUserProfileInfo(int id);
        Task<Msg> SendMessage(int userid, int otherid, string msg);
        Task<List<string>> ReceiveMessage(int userid, int otherid);


        Task<int> getUserIdByEmail(string email);

        Task<int> getUserIdByName(string name);
        bool IsAdmin(string email);



        List<OperationClaim> GetOperationClaims(Users user);
        void Add(Users user);
        Users GetByMail(string email);
    }
}
