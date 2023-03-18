using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBAPI.Models
{
    public class Entry
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int tag_id { get; set; }
        public string kod { get; set; }
        public string definition { get; set; } 
        public string date_entry { get; set; }
       
    }
}
