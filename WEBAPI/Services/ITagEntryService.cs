using WEBAPI.Models;
using WEBAPI.Models.DTO_s;
using WEBAPI.Models.DTO_s.UserDTos;

namespace WEBAPI.Services
{
    public interface ITagEntryService
    {

        Task<Tag> ShareTag(int user_id, string def);
        Task<Entry> AddEntry(ShareEntry content);
        Task<string> GetTagContentwithId(int id);
        Task<List<string>> GetTagAllTag();
        Task<List<string>> GetTagwithEntries(int tag_id);
        Task<List<GetContents>> GetAllEntries(int tagid);
        
        Task<List<GetContents>> ListTagsandOneEntryByLikeCount();

        Task<int> GetEntryIdByName(string entryidetail);

        Task<bool> AddLike(int user_id, int entry_id);
        Task<string> RemoveLike(int user_id, int entry_id);

        Task<int> GetLikeCountLike( int entry_id);
        Task<List<string>> GetListLikesUserName(int entry_id);
        Task<List<string>> TopTrendwithMaxEntryNumber();
        Task<List<GetTagandEntryCount>> TopEntrylikescount();
        Task<string> DeleteLike(int user_id, int entry_id);
        Task<int> GetTagIdByTagName(string name);
        Task<List<GetContents>> GetAllTagwithEntries(int userid);
        Task<List<string>> SearchFindTagandUserName(string input);

        Task<List<GetContents>> GetAllTagwithEntriesByDateToday();

        //python sentiment analysis
        Task<List<GetContents>> GetAllTagwithEntriesByUserName(string name);

        Task<string> TagAndEntryAdd(int userId, string tagDefinition, string entryDefinition);
    }
}
