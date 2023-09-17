using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Mono.Unix.Native;
using WEBAPI.Constants;
using WEBAPI.Data;
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



        public async Task<Users> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck =  _userService.GetByMail(userForLoginDto.email);

            if (userToCheck == null)
            {
                // Kullanıcı bulunamadı
                return BadRequest("kullanıcı yok");
            }
       
           
            if (Hashinghelper.VerifyPasswordHash(userForLoginDto.password, userToCheck.passwordhash, userToCheck.passwordsalt))
            {
                // Şifre uyumsuz
                return BadRequest("şifre yok");
            }

            // Token oluşturma ve döndürme
           
            return userToCheck;
        }

        private Users BadRequest(string v)
        {
            throw new NotImplementedException();
        }

        public Users Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            Hashinghelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            DateTime dt = DateTime.Now;
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");
            var user = new Users
            {
                email = userForRegisterDto.email,
                name = userForRegisterDto.name,
                register_date = s,
                type = "user",
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




        public async Task<AccessToken> CreateAccessToken(Users user)
        {
            var claims =  _userService.GetOperationClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return accessToken;
        }

        public Users ForgotPassword(string email, string password)
        {
            var result = _userService.GetByMail(email);
            byte[] passwordHash, passwordSalt;
            Hashinghelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            if (result != null)
            {
                result.passwordhash=passwordHash;
                result.passwordsalt=passwordSalt;
                _userService.UpdateUsrs(result.user_id,result);
            }
            return result;
            
        }

      
    }
    }
