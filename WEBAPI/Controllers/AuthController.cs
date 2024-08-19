
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Services;

using WEBAPI.Utilities.Validator;

namespace WEBAPI.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authservice;

     
       public AuthController(IAuthService authservice)
        {
            _authservice = authservice;
          
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userToLogin = await _authservice.Login(userForLoginDto);

            if (userToLogin == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(userToLogin);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // Check if user already exists
            var userExists = await _authservice.UserExists(userForRegisterDto.email);
            if (userExists)
            {
                return BadRequest("Kullanıcı zaten var.");
            }

            // Validate email format
            bool emailValid = EmailValidator.IsValidEmail(userForRegisterDto.email);
            if (!emailValid)
            {
                return BadRequest("Email is not valid.");
            }

            // Register the user
            var registerResult = await _authservice.Register(userForRegisterDto, userForRegisterDto.password);
            if (registerResult == null)
            {
                return StatusCode(500, "Registration failed.");
            }

            // Try to send activation email
            try
            {
                     IDictionary<string, object> options = new Dictionary<string, object>
                 {
                { "Arguments", new[] { @"C:\Program Files (x86)\IronPython 2.7\Lib", "bar" } }
                    };
                    var name = @"C:\Users\damla\source\repos\fakeksi_backend\WEBAPI\Utilities\emailconfig\emailconfig.py";
                    var ipy = IronPython.Hosting.Python.CreateRuntime(options);
                    dynamic Python_File = ipy.UseFile(name);
                    Python_File.main(userForRegisterDto.email.ToString());
                    // Activate the user after successful email send
                    await _authservice.RegisterActivate(userForRegisterDto.email);
                    return Ok(registerResult);
            }
            catch (Exception ex)
            {
                // Log the error and return a meaningful error message
                Console.WriteLine("An error occurred: " + ex.Message);
                return StatusCode(500, "An error occurred while sending the activation email.");
            }
        }
        [HttpPost("forgotpassword")]
        public async Task<ActionResult> Register(string email, string password)
        {
            var userExists = _authservice.UserExists(email);
            if (userExists == null)
            {
                return BadRequest("null");
            }


            var result = _authservice.ForgotPassword(email, password);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(null);
        }


    }

}