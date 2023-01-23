using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models;
using WEBAPI.Services;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _usrservice;
       
        public UserController(IUserService usrservice)
        {
            _usrservice = usrservice;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetAllHeroes()
        {
            return await _usrservice.GetAllUsers();
        }

      
        [HttpGet("getuserbyid")]
        public async Task<ActionResult<Users>> GetSingleHero(int id)
        {
            var result = await _usrservice.GetSingleUsrsById(id);
            if (result is null)
                return NotFound("Hero not found.");

            return Ok(result);
        }
        [HttpPost("addFollower")]
        public async Task<ActionResult<Users>> AddFollower(int id, int otherid)
        {
            await _usrservice.Addfollower(id,otherid);
           

            return Ok("işlem başarılı");
        }

        //[Route("api/users/{userId}/followers/{followerId}")]
        [HttpDelete("deletefollower")]
        public async Task<ActionResult<Users>> DeleteFollower(int id, int otherid)
        {
            await _usrservice.DeleteFollower(id, otherid);


            return Ok("işlem başarılı");
        }
        [HttpPost("Register")]
        public async Task<ActionResult<List<Users>>> AddHero(Users hero)
        {
            var result = await _usrservice.AddUser(hero);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<List<Users>>> Login(int id,string email, string password)
        {
            var result = await _usrservice.Login(id, email,password);
            return Ok(result);
        }
        [HttpGet("Logout")]
        public async Task<ActionResult<List<Users>>> Logout(int id)
        {
            var result = await _usrservice.Logout(id);
            return Ok(result);
        }

   
        [HttpPut("{id}")]
        public async Task<ActionResult<List<Users>>> UpdateHero(int id, Users request)
        {
          //  var result = await _usrservice.UpdateHero(id, request);
           

            return Ok("");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Users>>> DeleteHero(int id)
        {
            var result = await _usrservice.DeleteUser(id);
            if (result is null)
                return NotFound("Hero not found.");

            return Ok(result);
        }
    }
}
