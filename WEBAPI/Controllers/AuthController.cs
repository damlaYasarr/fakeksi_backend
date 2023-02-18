using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Services;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _usrservice;

        public AuthController(IAuthService usrservice)
        {
            _usrservice = usrservice;

        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _usrservice.Login(userForLoginDto);
            if (userToLogin == null)
            {
                return BadRequest(userToLogin);
            }

            var result = _usrservice.CreateAccessToken(userToLogin);
            if (result.IsCompleted)
            {
                return Ok(result);
            }

            return BadRequest(null);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _usrservice.UserExists(userForRegisterDto.email);
            if (userExists == null)
            {
                return BadRequest("null");
            }

            var registerResult = _usrservice.Register(userForRegisterDto, userForRegisterDto.password);
            var result = _usrservice.CreateAccessToken(registerResult);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(null);
        }
    }
}
