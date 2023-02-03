using System.ComponentModel.DataAnnotations;

namespace WEBAPI.Models
{
    public class Likes
    {
        [Key]
        public int like_id { get; set; }
        public int entry_id { get; set; }
        public int user_id { get; set; }
    }
}
