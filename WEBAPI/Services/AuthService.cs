using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.Unix.Native;
using System.Security.Claims;
using System.Text;
using RabbitMQ.Client;
using WEBAPI.Constants;
using WEBAPI.Data;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Utilities.encription;
using WEBAPI.Utilities.jwt;


namespace WEBAPI.Services
{
    public class AuthService : IAuthService
    {
        
        private readonly ITokenHelper _tokenService;
        private readonly IUserService _userService;

        public AuthService( ITokenHelper tokenService, IUserService userService)
        {
           
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<UserForLoginDto> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = await _userService.GetByMail(userForLoginDto.Email);

            if (userToCheck == null)
            {
                throw new Exception("Kullanıcı yok"); // User not found
            }

            if (!userToCheck.isActive)
            {
                throw new Exception("Kullanıcı aktivasyon e-postasını açmadı"); // User has not activated their account
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.passwordhash, userToCheck.passwordsalt))
            {
                throw new Exception("Yanlış şifre"); // Incorrect password
            }

            // Optionally, mark the user as active after successful login
             _userService.UserActive(userToCheck.email);

            // Create token after successful login
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userToCheck.user_id.ToString()),
        new Claim(ClaimTypes.Name, userToCheck.email)
        
    };

            var token = _tokenService.CreateToken(claims);

            return new UserForLoginDto
            {
                Email = userToCheck.email,
                Token = token
            };
        }

        public async Task<UserForRegisterDto> Register(UserForRegisterDto userForRegisterDto, string password)
        {

            // Generate password hash and salt
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // Create the user entity
            DateTime dt = DateTime.Now;
            string registerDate = dt.ToString("yyyy/MM/dd HH:mm:ss");
            var user = new Users
            {
                email = userForRegisterDto.email,
                name = userForRegisterDto.name,
                register_date = registerDate,
                type = "user",
                passwordhash = passwordHash,
                passwordsalt = passwordSalt
            };

            // Add user to the database
             _userService.Add(user);
            
            return new UserForRegisterDto
            {
                email = user.email,
                name = user.name,
       
            };
        }
      
        public async Task<bool> UserExists(string email)
{
    var result = await _userService.GetByMail(email); // Assuming GetByMail is asynchronous
    return result != null;
}





        public async Task<Users> ForgotPassword(string email, string newPassword)
        {
            // Get the user by their email
            var user = await _userService.GetByMail(email);

            if (user == null)
            {
                return null; // User not found, handle this as needed
            }

            // Create a new password hash and salt
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

            // Update the user's password hash and salt
            user.passwordhash = passwordHash;
            user.passwordsalt = passwordSalt;

            // Update the user in the database
            await _userService.UpdateUsrs(user.user_id, user);

            // Return the updated user
            return user;
        }

       

        public Task<Users?> Logout(int id)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterActivate(string email)
        {
            await _userService.UserActive(email);
        }

    }
    }
