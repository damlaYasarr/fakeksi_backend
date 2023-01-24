using WEBAPI.Models;
using WEBAPI.Models.DTO_s;

namespace WEBAPI.Services
{
    public interface ITagEntryService
    {

        Task<ShareTagwithEntry?> ShareTag(ShareTagwithEntry entyshare);
        Task<Entry> addentry(int user_id, int tag_id, string def);
    }
}
