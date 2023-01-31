﻿using Microsoft.AspNetCore.Http.HttpResults;
using System;
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
            DateTime aDate = DateTime.Now;

            //yazılan tag'in id'sini almak gerekir mi?
            Tag shareentry = new Tag()
            {
                user_id = tt.user_id,
                definition = tt.definition,
                datetime = aDate
            };

            _context.Tags.Add(shareentry);
                await _context.SaveChangesAsync();
                return shareentry;


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

        public Task<List<GetContents>> GetAllTagwithEntries(int userid)
        {//mantık hatası var incele.
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
                             entries = r.definition
                         };

          

            return Task.FromResult(result.ToList());
        }

       
    }
}
