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
                             join u in _context.Entry on entyshare.user_id equals u.user_id into ux
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

       public async Task<Entry> Addentry(int user_id, int tag_id, string def)
        {
            //create entry kod
            var result = _context.Users.SingleOrDefault(b => b.user_id == user_id);
            Entry entry = new Entry()
            {
                user_id = user_id,
                tag_id = tag_id,
                definition = def,
            };
            _context.Entry.Add(entry);
            _context.SaveChanges();
            return entry;
        }

        public async Task<string?> GetTagContentwithId(int id)
        {
            var result= _context.Tags.SingleOrDefault(t => t.id == id);
            if (result == null)
            {
                return "bu id ile tag yok";
            }
            return result.definition;
        }

        public Task<List<string>> GetTagAllTag()
        {
            var result=from t in _context.Tags
                       select t.definition;

            return Task.FromResult(result.ToList());
        }

        public Task<List<string>> GetTagwithEntries(int tag_id)
        {

            var xx = from x in _context.Entry
                     where x.tag_id == tag_id
                     select x.definition;

            return Task.FromResult(xx.ToList());

        }
    }
}
