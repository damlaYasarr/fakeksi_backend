using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s;
using WEBAPI.Services;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagEntryController : Controller
    {

        private readonly ITagEntryService _tagentryservice;

        public TagEntryController(ITagEntryService tagentryservice)
        {
            _tagentryservice = tagentryservice;
        }
        [HttpPost("tagekle")]
        public async Task<ActionResult<ShareTagwithEntry>> Addtag(ShareTagwithEntry entyshare)
        {
            await _tagentryservice.ShareTag(entyshare);
            return Ok("tag eklendi"); 
        }

    }
}
