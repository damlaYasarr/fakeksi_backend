using Microsoft.EntityFrameworkCore;
using WEBAPI.Constants;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Utilities.Security.Hashing;
using WEBAPI.Utilities.Security.JWT;

namespace WEBAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;

       public AuthService(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

   

        public Users Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck == null)
            {
                return null;
            }
            if (!Hashinghelper.VerifyPasswordHash(userForLoginDto.password, userToCheck.passwordhash, userToCheck.passwordsalt))
            {
                return null;
            }
            return userToCheck;
        }

        public Users Register(UserForRegisterDto userForRegisterDto, string password)
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
            return user;
            // throw new NotImplementedException();
        }

        public Task UserExists(string email)
        {
            var result = _userService.GetByMail(email);
            if (result != null)
            {
                return Task.FromResult(Messages.UserAlreadyExists
                    );
            }
            return Task.FromResult("user var");
        }




        public Task<AccessToken> CreateAccessToken(Users user)
        {
            var claims = _userService.GetOperationClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return Task.FromResult(accessToken);
        }

    }
    }
