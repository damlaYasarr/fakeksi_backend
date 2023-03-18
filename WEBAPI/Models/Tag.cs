using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace WEBAPI.Models
{
    public class Tag
    {
        [Key]
        public int id { get; set; }
        public int user_id { get; set; }
        public string definition { get; set; }
        public string datetime { get; set; }
       
    }
}
