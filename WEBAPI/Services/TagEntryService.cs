using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBAPI.Data;
using WEBAPI.Models;
using WEBAPI.Models.DTO_s;
using WEBAPI.Models.DTO_s.UserDTos;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace WEBAPI.Services
{
    public class TagEntryService : ITagEntryService
    {
        private readonly DataContext _context;
       

        public TagEntryService(DataContext context)
        {
            _context = context;
        }

        //kişinin bütün yazdığı tagleri listele
        //kişinin bütün entryleri listele

        public async Task<Tag> ShareTag(int user_id, string def)
        {
            
            DateTime dt = DateTime.Now;
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");
            //yazılan tag'in id'sini almak gerekir mi?
            Tag shareentry = new Tag()
            {
                user_id = user_id,
                definition =def,
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

        public Task<List<string>> ListTagsByDate()
        {
            //bugün atılan başlık bugün başlığı altında yayınlanacak
            DateTime dt = DateTime.Now;
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");
            var firstvalue = s.Split(" ")[0];
          
            var result = from x in _context.Tags
                         select x;
           
            List<string> listtag = new();
            foreach (var t in result)
            {
                var xx = t.datetime.Split(" ")[0];
                if (firstvalue == xx)
                {
                    listtag.Add(t.definition);
                    
                }
            }
            
            //GETİRİLEN LİSTE RANDOM YAPILACAK 
            return Task.FromResult(listtag);


        }

        public Task<List<Tag>> ListTagsByContent()
        {
            //it will be made last .
            throw new NotImplementedException();
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
        public async Task<string> AddLike(int user_id, int entry_id)
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
                return "eklendi";
            }
            else
            {
                return "kişi var";
            }
             
        }
        public async Task<string> DeleteLike(int user_id, int entry_id)
        {//çalışmıyor kontrol et
            var resul = from x in _context.Likes
                        where x.user_id == user_id && x.entry_id == entry_id
                        select x;
            if (!resul.IsNullOrEmpty())
            {
               
                _context.Likes.Remove((Likes)resul);
                await _context.SaveChangesAsync();
                return "like silindi";
            }
            else
            {
                return "like yok";
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
    }
}
