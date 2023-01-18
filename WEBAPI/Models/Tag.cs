using System.Diagnostics;

namespace WEBAPI.Models
{
    public class Tag
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string definition { get; set; }
        public DateTime tags_date { get; set; }
    }
}
