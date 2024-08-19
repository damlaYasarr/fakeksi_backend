using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Models;
using WEBAPI.Utilities.jwt;

namespace WEBAPI.Services
{
    public interface IAuthService
    {
        Task<UserForRegisterDto> Register(UserForRegisterDto userForRegisterDto, string password);
        Task<Users?> Logout(int id);
        Task<UserForLoginDto> Login(UserForLoginDto userForLoginDto);
        Task<bool> UserExists(string email);
        Task<Users> ForgotPassword(string email, string password);
        Task RegisterActivate(string email);
    }
}
