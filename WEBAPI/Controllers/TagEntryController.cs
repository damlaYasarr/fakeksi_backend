using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models;
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
        [HttpPost("tag ekle")]
        public async Task<ActionResult<Tag>> Addtag(Tag tag)
        {
            await _tagentryservice.ShareTag(tag);
            return Ok("tag eklendi"); 
        }

    }
}
