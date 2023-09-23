using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Services;
using Python.Runtime;
using IronPython.Hosting;

using Microsoft.Scripting.Hosting;
using System.Xml.Linq;
using static IronPython.Runtime.Profiler;
using WEBAPI.Models;

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
            dynamic userToLogin = await _usrservice.Login(userForLoginDto);

            if (userToLogin == null)
            {
                return Unauthorized("Invalid username or password"); 
            }
            dynamic accessToken = await _usrservice.CreateAccessToken(userToLogin);
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
            if (userExists != null)
            {
                return BadRequest("kullanıcı zaten var ");
            }
       
            var registerResult = _usrservice.Register(userForRegisterDto, userForRegisterDto.password);
            if (registerResult != null)
            {
                try
                {
                    IDictionary<string, object> options = new Dictionary<string, object>();
                    options["Arguments"] = new[] { @"C:\Program Files (x86)\IronPython 2.7\Lib", "bar" };
                    var name = @"C:\Users\damla\source\repos\fakeksi_backend\WEBAPI\Services\emailconfig\emailconfig.py";
                    var ipy = IronPython.Hosting.Python.CreateRuntime(options);
                    dynamic Python_File = ipy.UseFile(name);
                    //main is my funciton name
                    Python_File.main(userForRegisterDto.email.ToString());
                    _usrservice.RegisterActivate(userForRegisterDto.email);
                    return Ok( registerResult);

                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it or display an error message).
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

              
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


        // static void RunScript(string scriptPath, string functionname, string email)
        
        //    {
        //    Runtime.PythonDLL = @"C:\Program Files\Python311\python311.dll";
        //    PythonEngine.Initialize();

        //    using (Py.GIL())
        //    {
        //        dynamic pythonModule = Py.Import(scriptPath);
        //        dynamic pythonFunction = pythonModule.ToPython(functionname);
        //        pythonFunction(email);
               
        //    }
        //}
}
}
