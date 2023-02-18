using WEBAPI.Constants;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Utilities.Security.Hashing;
using WEBAPI.Utilities.Security.JWT;

namespace WEBAPI.Services
{
    public class AuthManager : IAuthService
    {

        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }
        public Task<AccessToken> CreateAccessToken(Users user)
        {
            var claims = _userService.GetOperationClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return Task.FromResult(accessToken);
        }

        public Task Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck == null)
            {
                return Task.FromResult(Messages.UserNotFound);
            }
            if (!Hashinghelper.VerifyPasswordHash(userForLoginDto.password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return Task.FromResult(Messages.PasswordError);
            }
            return Task.FromResult(Messages.SuccessfulLogin
                );
        }

        public Task<Users> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            Hashinghelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new Users
            {
                email = userForRegisterDto.email,
                name = userForRegisterDto.name,
            
                passwordhash = passwordHash,
                passwordsalt = passwordSalt,
                isActive = true
            };
            _userService.Add(user);
            return Task.FromResult(user);
           // throw new NotImplementedException();
        }

        public Task UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return Task.FromResult(Messages.UserAlreadyExists
                    );
            }
            return Task.FromResult("user var");
        }
    }
}
