using System.ComponentModel.DataAnnotations;

namespace WEBAPI.Models
{
    public class Users
    {
        [Key]
        public int user_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        //public string date { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public bool isActive { get; set; }
    }
}
