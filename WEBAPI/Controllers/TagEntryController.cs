using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s;
using WEBAPI.Models.DTO_s.UserDTos;
using WEBAPI.Services;

namespace WEBAPI.Controllers
{
    [Route("api/[controller]")]
    public class TagEntryController : Controller
    {

        private readonly ITagEntryService _tagentryservice;

        public TagEntryController(ITagEntryService tagentryservice)
        {
            _tagentryservice = tagentryservice;
        }
        [HttpPost("tagekle")]
        public async Task<ActionResult<Tag>> Addtag(int user_id, string def)
        {
          var result=  await _tagentryservice.ShareTag(user_id, def);
            return Ok(result); 
        }
    
        [HttpPost("entryekle")]
        public async Task<ActionResult<Entry>> AddEntries(ShareEntry content)
        {
            var result =await _tagentryservice.AddEntry(content);
            
            return Ok(result);
           
        }
        [Route("gettag{id}")]
        [HttpGet]
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
        [HttpGet("getalltagandentrieswithUSER{id}")]
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
        [Route("entries{id}")]
        [HttpGet]
        public async Task<ActionResult<List<GetContents>>> GetEntries(int id)
        {
            
            var result =await _tagentryservice.GetAllEntries(id);
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
        [HttpDelete("deletelike")]
        public async Task<ActionResult<string>> DeleteLike(int userid,int entryid)
        {

            var result = await _tagentryservice.DeleteLike(userid,entryid);
            return Ok(result);
        }
        [HttpGet("GetTagandcount")]
        public async Task<ActionResult<GetTagandEntryCount>> GetTrendTagandEntryCount()
        {

            var result = await _tagentryservice.TopEntrylikescount();
            return Ok(result);
        }
        [HttpGet("GetMOSTlikesEntryDetailwithContent")]
        public async Task<ActionResult<GetTagandEntryCount>> ListTagsandOneEntryByLikeCount()
        {

            var result = await _tagentryservice.ListTagsandOneEntryByLikeCount();
            return Ok(result);
        }
        [HttpGet("tagidbyname")]
        public async Task<ActionResult<GetTagandEntryCount>> GetTagIdByTagname(string name)
        {

            var result = await _tagentryservice.GetTagIdByTagName(name);
            return Ok(result);
        }
        //GetEntryIdByName
        [HttpGet("entryidbyname")]
        public async Task<ActionResult<GetTagandEntryCount>> GetEntryIdByName(string name)
        {

            var result = await _tagentryservice.GetEntryIdByName(name);
            return Ok(result);
        }
        [Route("search")]
        [HttpGet]
        public async Task<ActionResult<List<string>>> SearchFindTagandUserNames(string nn)
        {
            var result = await _tagentryservice.SearchFindTagandUserName(nn);
            return Ok(result);
        }
    }
}
