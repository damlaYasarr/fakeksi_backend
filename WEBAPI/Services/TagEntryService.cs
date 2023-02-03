﻿using Microsoft.AspNetCore.Http.HttpResults;
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
            //tarih değeri
           
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
            //random tag seçilecek bu tag'lardaki en yüksek like count olan entry çekilecek.
            throw new NotImplementedException();
        }
        public async Task<string> AddLike(int user_id, int entry_id)
        {//if else ekle
            var resul = from x in _context.Likes
                         where x.user_id == user_id && x.entry_id == entry_id
                         select x.like_id;        
           
                Likes addlike = new Likes()
                {
                    user_id = user_id,
                    entry_id = entry_id,
                };
                _context.Likes.Add(addlike);
                await _context.SaveChangesAsync();
                return "eklendi";
        }

        public Task<int> GetLikeCountLike(int user_id, int entry_id)
        {
            //like count 
            throw new NotImplementedException();
        }
    }
}
