using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Services;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet("getuserIdByEmail")]
        public async Task<ActionResult<int>> GetUserIdByEmail(string email)
        {
            var result = await _usrservice.getUserIdByEmail(email);
            return Ok(result);
        }
        [HttpGet("getuserIdByname")]
        public async Task<ActionResult<int>> GetUserIdByName(string name)
        {
            var result = await _usrservice.getUserIdByName(name);
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
        [HttpGet("getAllFollower")]
        public async Task<ActionResult<List<string>>> GetAllFollower(int id)
        {
            var result=await _usrservice.GetAllFollower(id);


            return Ok(result);
        }


        [HttpGet("getAllFolloweD")]
        public async Task<ActionResult<List<string>>> GetAllFollowed(int id)
        {
            var result = await _usrservice.GetAllFollowed(id);


            return Ok(result);
        }
        [Route("isAdminorUser")]
        [HttpPost]
        public async Task<ActionResult<bool>> GetResult(string email)
        {
            var result =  _usrservice.IsAdmin(email);
            

            return Ok(result);
        }

      
        [HttpGet("UserProfileDetail")]
        public async Task<ActionResult<List<Users>>> GetUserProfileDetail(int id)
        {
            var result = await _usrservice.GetUserProfileInfo(id);
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
