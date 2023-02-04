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

namespace WEBAPI.Services
{
    public class TagEntryService : ITagEntryService
    {
        private readonly DataContext _context;
        private object definition;

        public TagEntryService(DataContext context)
        {
            _context = context;
        }

        //kişinin bütün yazdığı tagleri listele
        //kişinin bütün entryleri listele

        public async Task<Tag> ShareTag(Tag tt)
        {
            
            DateTime dt = DateTime.Now;
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");
            //yazılan tag'in id'sini almak gerekir mi?
            Tag shareentry = new Tag()
            {
                user_id = tt.user_id,
                definition = tt.definition,
                datetime = s
            };

            _context.Tags.Add(shareentry);
                await _context.SaveChangesAsync();
                return shareentry;


        }

       public async Task<Entry> Addentry(int user_id, int tag_id, string def)
        {
            //create entry kod
            DateTime dt = DateTime.Now; // Or whatever
            string s = dt.ToString("yyyy/MM/dd HH:mm:ss");

            var result = _context.Users.SingleOrDefault(b => b.user_id == user_id);
            Entry entry = new Entry()
            {
                user_id = user_id,
                tag_id = tag_id,
                definition = def,
                date_entry = s,
                kod = createCode(),
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

        public Task<List<GetContents>> GetAllTagwithEntries(int userid)
        {
            var result = from r in _context.Entry
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
        {//1.user 5. entry beğendi beğendi ise veritabanına id eklenmeli
            //bütün tag içinden 
            //random tag seçilecek bu tag'lardaki en yüksek like count olan entry çekilecek.
           
            throw new NotImplementedException();
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
        {
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
            //dünün en fazla enrty alan tagları ve yanında count
            var result1 = from t in _context.Tags
                          join e in _context.Entry on t.id equals e.tag_id
                          group t.definition by t.definition into g
                          select new GetTagandEntryCount { Tag = g.Key, entryCount = g.Count() };








            return Task.FromResult( result1.ToList());
        }
    }
}
