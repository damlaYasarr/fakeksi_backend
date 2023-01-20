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
        //başlık kısmı mutlaka entry ile paylaşılmalı
       
        public async Task<Tag?> ShareTag(Tag tag)
        {//deneme kod cvvvc
            //kjbkkh
            /*
              var result = from r in context.basvuru
                                 join x in context.users on user_id equals x.Id
                                 join u in context.status_table on r.status equals u.id into ux
                                 from u in ux.DefaultIfEmpty()
                                 where r.Id == user_id
                                 select new ApplicationSchemaDto
                                 {
                                     id = r.Id,
                                     user_id = r.User_Id,
                                     Created = r.Zaman_Damgası,
                                     status = u.id,
                                     baslik = r.Baslik
                                 };
             */
            var result = from r in _context.Tags
                         join p in _context.Users on user_id equals r.
                         join u in _context.Tags on r.tag_id equals u.id into ux
                         from u in ux.DefaultIfEmpty()
                         where r.Id == user_id
                         select new ShareTagwithEntry
                         {
                             
                         };


            Tag tg = new Tag()
            {
                definition=tag.definition,
                id=tag.id,
                user_id=tag.user_id,

            };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;


        }
    }
}
