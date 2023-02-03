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
        public async Task<ActionResult<Tag>> Addtag(Tag tt)
        {
            await _tagentryservice.ShareTag(tt);
            return Ok("tag eklendi"); 
        }
        [HttpPost("entryekle")]
        public async Task<ActionResult<Entry>> AddEntry(int user_id, int tag_id,string def)
        {
            await _tagentryservice.Addentry(user_id,tag_id, def);
            return Ok("tag eklendi");
        }
        [HttpGet("gettag")]
        public async Task<ActionResult<string>> GetTag(int id)
        {
           var result= await _tagentryservice.GetTagContentwithId(id);
            return Ok(result);
        }
        [HttpGet("getAlltag")]
        public async Task<ActionResult<List<string>>> GetAllTag()
        {
            var result = await _tagentryservice.GetTagAllTag();
            return Ok(result);
        }
        [HttpGet("getonestagallEntries")]
        public async Task<ActionResult<List<string>>> GetAllentrieswithonetag(int id)
        {
            var result = await _tagentryservice.GetTagwithEntries(id);
            return Ok(result);
        }
        [HttpGet("getalltagandentrieswithUSER")]
        public async Task<ActionResult<List<GetContents>>> GetAllTagwithEntries(int id)
        {
        
            var result=  await _tagentryservice.GetAllTagwithEntries(id);
            return Ok(result);
        }
        [HttpGet("listAllTagByDate")]
        public async Task<ActionResult<List<string>>> ListTagsByDate()
        {

            var result = await _tagentryservice.ListTagsByDate();
            return Ok(result);
        }
        [HttpPost("addLike")]
        public async Task<ActionResult<int>> AddLike(int userid, int entryid)
        {

            var result = await _tagentryservice.AddLike(userid, entryid);
            return Ok(result);
        }
        [HttpGet("getlikecount")]
        public async Task<ActionResult<string>> GetLikeCount( int entryid)
        {

            var result = await _tagentryservice.GetLikeCountLike( entryid);
            return Ok(result);
        }
        [HttpGet("getlikesusersname")]
        public async Task<ActionResult<string>> Getlikeusername(int entryid)
        {

            var result = await _tagentryservice.GetListLikesUserName(entryid);
            return Ok(result);
        }
    }
}
