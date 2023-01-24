using WEBAPI.Models;
using WEBAPI.Models.DTO_s;

namespace WEBAPI.Services
{
    public interface ITagEntryService
    {

        Task<ShareTagwithEntry?> ShareTag(ShareTagwithEntry entyshare);
        Task<Entry> Addentry(int user_id, int tag_id, string def);
        Task<string> GetTagContentwithId(int id);
        Task<List<string>> GetTagAllTag();
        Task<List<string>> GetTagwithEntries(int tag_id);
    }
}
