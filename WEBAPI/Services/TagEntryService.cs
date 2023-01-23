using Microsoft.AspNetCore.Http.HttpResults;
using WEBAPI.Data;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s;

namespace WEBAPI.Services
{
    public class TagEntryService : ITagEntryService
    {
        private readonly DataContext _context;

        public TagEntryService(DataContext context)
        {
            _context = context;
        }
       
       
        public async Task<ShareTagwithEntry?> ShareTag(ShareTagwithEntry entyshare)
        {
            var person=_context.Users.SingleOrDefault(b=>b.user_id == entyshare.user_id);
            
                var result = from r in _context.Users
                             join p in _context.Tags on entyshare.tag_id equals p.id //user id gerekli
                             join u in _context.Entry on r.user_id equals u.user_id into ux
                             from u in ux.DefaultIfEmpty()
                             where p.user_id == entyshare.user_id
                             select new ShareTagwithEntry
                             {
                                 tag_id = p.id,
                                 user_id = r.user_id,
                                 entry_id = u.id,
                                 tag = p.definition,
                                 entry = u.definition
                             };


                await _context.SaveChangesAsync();
                return entyshare;
            




        }
    }
}
