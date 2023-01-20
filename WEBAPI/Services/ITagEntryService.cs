using WEBAPI.Models;
using WEBAPI.Models.DTO_s;

namespace WEBAPI.Services
{
    public interface ITagEntryService
    {

        Task<ShareTagwithEntry?> ShareTag(ShareTagwithEntry entyshare);
    }
}
