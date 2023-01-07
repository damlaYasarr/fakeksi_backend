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

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetSingleHero(int id)
        {
            

            return Ok("");
        }

        [HttpPost]
        public async Task<ActionResult<List<Users>>> AddHero(Users hero)
        {
            var result = await _usrservice.AddUser(hero);
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
