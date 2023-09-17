using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Services;
using Python.Runtime;
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

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userToLogin = await _usrservice.Login(userForLoginDto);

            if (userToLogin == null)
            {
                return Unauthorized("Invalid username or password"); 
            }
            var accessToken = await _usrservice.CreateAccessToken(userToLogin);
            if (accessToken != null)
            {
                return Ok(accessToken);
            }
            return BadRequest("kullanıcı olmalı");
        }

        [HttpPost("register")]
        public async Task<ActionResult>Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _usrservice.UserExists(userForRegisterDto.email);
            if (userExists == null)
            {
                return BadRequest("null");
            }
         
         

       
            var registerResult = _usrservice.Register(userForRegisterDto, userForRegisterDto.password);
          
            if (registerResult != null)
            {


                 RunScript("__main__", registerResult.email);
                 
                return Ok(registerResult);
            }

            return BadRequest(null);
        }
        [HttpPost("forgotpassword")]
        public async Task<ActionResult> Register(string email, string password)
        {
            var userExists = _usrservice.UserExists(email);
            if (userExists == null)
            {
                return BadRequest("null");
            }

            
            var result = _usrservice.ForgotPassword(email,password);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(null);
        }


         static void RunScript(string scriptname,string email)
        {
            Runtime.PythonDLL = @"C:\Program Files\Python311\python311.dll";
            PythonEngine.Initialize();

            using (Py.GIL()) 
            {
                dynamic pythonModule = Py.Import(scriptname);
                pythonModule.InvokeMethod("invokemethod", email); 


            }
        }
}
}
