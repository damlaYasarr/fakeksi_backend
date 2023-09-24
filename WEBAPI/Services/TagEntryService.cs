using GW2NET.Items;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using WEBAPI.Data;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s;
using WEBAPI.Models.DTO_s.UserDTos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WEBAPI.Services
{
    public class TagEntryService : ITagEntryService
    {
        private readonly DataContext _context;
       

        public TagEntryService(DataContext context)
        {
            _context = context;
        }

        public async Task<Tag> ShareTag(int user_id, string def)
        {
            DateTime dt = DateTime.Now;
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");

            // Check if a tag with the same definition already exists
            var existingTag = await _context.Tags.FirstOrDefaultAsync(x => x.definition == def);

            if (existingTag != null)
            {
            
                return null; 
            }

            // Create a new tag
            Tag shareentry = new Tag()
            {
                user_id = user_id,
                definition = def,
                datetime = s
            };

            _context.Tags.Add(shareentry);
            await _context.SaveChangesAsync();
            return shareentry;
        }


        public async Task<Entry> AddEntry(ShareEntry content)
        {
           

            //create entry kod
            DateTime dt = DateTime.Now; // Or whatever
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");
            var result = await _context.Entries.FirstAsync(x => x.definition == content.definition);
            //block duplicate definition
            if (result != null)
            {
                return null;
            }
            Entry addlike = new Entry()
                {
                    user_id = content.user_id,
                    tag_id = content.tag_id,
                    definition = content.definition,
                    date_entry=s,
                    kod=createCode()
                };
                _context.Entries.Add(addlike);
                await _context.SaveChangesAsync();
                return await Task.FromResult(addlike);
            
            
            
            

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

            var xx = from x in _context.Entries
                     where x.tag_id == tag_id
                     select x.definition;

            return Task.FromResult(xx.ToList());

        }

        public Task<List<GetContents>> GetAllTagwithEntries(int userid)
        {
            var result = from r in _context.Entries
                         join x in _context.Users on userid equals x.user_id
                         join u in _context.Tags on r.tag_id equals u.id into ux
                         from u in ux.DefaultIfEmpty()
                         where r.user_id == userid
                         select new GetContents
                         {
                             name = x.name,
                             enrydate = r.date_entry,
                             tagname = u.definition,
                             entries = r.definition,
                             kod=r.kod
                         };

          

            return Task.FromResult(result.ToList());
        }
        public Task<List<GetContents>> GetAllTagwithEntriesByUserName(string name)
        {
            var result = from r in _context.Entries
                         join x in _context.Users on name equals x.name
                         join u in _context.Tags on r.tag_id equals u.id into ux
                         from u in ux.DefaultIfEmpty()
                         where r.user_id == x.user_id
                         select new GetContents
                         {
                             name = x.name,
                             enrydate = r.date_entry,
                             tagname = u.definition,
                             entries = r.definition,
                             kod = r.kod
                         };



            return Task.FromResult(result.ToList());
        }
        public Task<List<GetContents>> GetAllEntries(int tagid)
        {
            var result = from r in _context.Entries
                         join x in _context.Users on r.user_id equals x.user_id
                         join u in _context.Tags on r.tag_id equals u.id into ux
                         from u in ux.DefaultIfEmpty()
                         where r.tag_id==tagid
                         select new GetContents
                         {
                             name = x.name,
                             enrydate = r.date_entry,
                            
                             entries = r.definition,
                             kod = r.kod
                         };



            return Task.FromResult(result.ToList());
        }
        private string createCode()
        {
            Random r = new Random();
            var x = r.Next(0, 1000000);
            string s = x.ToString("000000");
            string result = "#" + s;
            return result;
        }

       

   

        public Task<List<GetContents>> ListTagsandOneEntryByLikeCount()
        {//pretty hard
            var query = from entry in _context.Entries
                        join popularEntry in (from like in _context.Likes
                                              group like by like.entry_id into g
                                              orderby g.Count() descending
                                              select g.Key)
                        on entry.id equals popularEntry
                        join tag in _context.Tags
                        on entry.tag_id equals tag.id
                        join user in _context.Users
                        on entry.user_id equals user.user_id
                        select new GetContents { 
                            name=user.name, 
                            tagname=tag.definition,
                            enrydate=entry.date_entry,
                            entries=entry.definition,
                            kod=entry.kod,
                            likecount=popularEntry };
            var result = query.FirstOrDefault();
           return Task.FromResult(query.ToList());
        }
        public async Task<bool> AddLike(int user_id, int entry_id)
        {
            var resul = from x in _context.Likes
                         where x.user_id == user_id && x.entry_id == entry_id
                         select x.like_id;
            if (resul.IsNullOrEmpty())
            {
                Likes addlike = new Likes()
                {
                    user_id = user_id,
                    entry_id = entry_id,
                };
                _context.Likes.Add(addlike);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
             
        }
        public async Task<string> DeleteLike(int user_id, int entry_id)
        {
            try
            {
                var likeToDelete = _context.Likes.FirstOrDefault(x => x.user_id == user_id && x.entry_id == entry_id);

                if (likeToDelete != null)
                {
                    _context.Likes.Remove(likeToDelete);
                    await _context.SaveChangesAsync();
                    return "Like removed successfully.";
                }
                else
                {
                    return "Like not found.";
                }
            }
            catch (Exception ex)
            {
        
                return "An error occurred while deleting the like.";
            }
        }
     
        public Task<int> GetLikeCountLike( int entry_id)
        {
            var result = from x in _context.Likes
                         where x.entry_id == entry_id
                         select x.user_id;
            return Task.FromResult(result.Count());
        }
        public Task<List<string>> GetListLikesUserName(int entry_id)
        {
            var result = from x in _context.Users
                         join r in _context.Likes on x.user_id equals r.user_id
                         where r.entry_id == entry_id
                         select x;
            List<string> namelist = new(); 
            foreach(var tt in result)
            {
                namelist.Add(tt.name);

            }
            return Task.FromResult(namelist);
        }

        public Task<List<string>> TopTrendwithMaxEntryNumber()
        { 
            //1 haftanın en fazla entry alan tagleri
            throw new NotImplementedException();
        }

        public Task<List<GetTagandEntryCount>> TopEntrylikescount()
        {
            
            var result1 = from t in _context.Tags
                          join e in _context.Entries on t.id equals e.tag_id
                          group t.definition by t.definition into g
                          select new GetTagandEntryCount { Tag = g.Key, entryCount = g.Count() };


            return Task.FromResult( result1.ToList());
        }

        public Task<int> GetTagIdByTagName(string name)
        {
            var result = from x in _context.Tags
                         where x.definition == name
                         select x;
            int k = 0;
            foreach(var t in result) { k = t.id; }
            return Task.FromResult(k);

        }

        public async Task<List<string>> SearchFindTagandUserName(string input)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.name.Contains(input));
            // var result2 = await _context.Tags.FirstOrDefaultAsync(e => e.definition.Contains(input));
            var result = from x in _context.Tags
                         select x.definition;
            var user = from x in _context.Users
                         select x.name;
            List<string> sentences = new();


            foreach (var item in result)
            {
                string xx = item.ToString();
                if (xx.Contains(input))
                {
                    sentences.Add(xx);
                }
                
            }
            foreach (var item in user)
            {
                string xx = item.ToString();
                if (xx.Contains(input))
                {
                    sentences.Add("@"+xx);
                }

            }

            return sentences;
        }

        public async Task<string> RemoveLike(int user_id, int entry_id)
        {
            var resul = from x in _context.Likes
                        where x.user_id == user_id && x.entry_id == entry_id
                        select x;
            var like = await resul.FirstOrDefaultAsync();
            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
                return "removed";
            }
            else
            {
                return "başarılı olamıyor";
            }
        }

        public async Task<int> GetEntryIdByName(string entryDetail)
        {
            var result = await (
                from x in _context.Entries
                where entryDetail == x.definition
                select x.id
            ).FirstOrDefaultAsync();

            return result;
        }
      

        public async Task<List<Tag>> GetTodayContent()
        {
            DateTime today = DateTime.Now.Date;

            var tagQuery = from tag in _context.Tags
                           where _context.Entries.Any(entry => entry.tag_id == tag.id && entry.date_entry == today.ToString())
                           select tag;

            List<Tag> listTag = await tagQuery.ToListAsync(); 

            return listTag;
        }

        public Task<List<OtherUserProfile>> GetOtherUserPofile(string name)
        {
            throw new NotImplementedException();


        }

        public async Task<string> TagAndEntryAdd(int userId, string tagDefinition, string entryDefinition)
        {

            DateTime today = DateTime.Now.Date;
            try
            { 
                // Create a new Tag entity and Entry entity
                var newTag = new Tag
                {
                    user_id = userId,
                    definition = tagDefinition, 
                    datetime=today.ToString(),
                    // Other properties related to Tag
                };

                var newEntry = new Entry
                {
                    user_id = userId,
                    definition = entryDefinition,
                    date_entry = today.ToString()
                   
                };

               
                _context.Tags.Add(newTag);
                _context.Entries.Add(newEntry);

             
                await _context.SaveChangesAsync();

                return "Tag and Entry added successfully.";
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return $"An error occurred: {ex.Message}";
            }
        }

        Task<List<GetContents>> ITagEntryService.GetTodayContent()
        {
            throw new NotImplementedException();
        }

       
    }
}
