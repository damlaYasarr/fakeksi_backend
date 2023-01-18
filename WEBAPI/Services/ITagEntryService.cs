using WEBAPI.Models;

namespace WEBAPI.Services
{
    public interface ITagEntryService
    {
        Task<Tag?> ShareTag(Tag tag);
    }
}
