using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Models;
using WEBAPI.Utilities.Security.JWT;

namespace WEBAPI.Services
{
    public interface IAuthService
    {
        Users Register(UserForRegisterDto userForRegisterDto, string password);

        Task<Users> Login(UserForLoginDto userForLoginDto);
        Task UserExists(string email);
        Task<AccessToken> CreateAccessToken(Users user);
        Task<Users> ForgotPassword(string email, string password);
        void RegisterActivate(string email);
    }
}
