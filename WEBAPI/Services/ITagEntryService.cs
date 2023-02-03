using WEBAPI.Models;
using WEBAPI.Models.DTO_s;

namespace WEBAPI.Services
{
    public interface ITagEntryService
    {

        Task<Tag> ShareTag(Tag tt);
        Task<Entry> Addentry(int user_id, int tag_id, string def);
        Task<string> GetTagContentwithId(int id);
        Task<List<string>> GetTagAllTag();
        Task<List<string>> GetTagwithEntries(int tag_id);
        Task<List<GetContents>> GetAllTagwithEntries(int userid);
        Task<List<string>> ListTagsByDate();
        Task<List<Tag>> ListTagsByContent();
        Task<List<GetContents>> ListTagsandOneEntryByLikeCount();
        Task<string> AddLike(int user_id, int entry_id);
        Task<int> GetLikeCountLike(int user_id, int entry_id);

    }
}
