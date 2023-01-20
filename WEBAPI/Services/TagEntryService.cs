using WEBAPI.Data;
using WEBAPI.Models;

namespace WEBAPI.Services
{
    public class TagEntryService : ITagEntryService
    {
        private readonly DataContext _context;

        public TagEntryService(DataContext context)
        {
            _context = context;
        }
        //başlık kısmı mutlaka entry ile paylaşılmalı
        //mesela 
        public async Task<Tag?> ShareTag(Tag tag)
        {
           
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;


        }
    }
}
