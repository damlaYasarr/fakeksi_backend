using System.ComponentModel.DataAnnotations;
using System.Security;

namespace WEBAPI.Models
{
    public class Users
    {
        [Key]
        public int user_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string register_date { get; set; }

        public string password { get; set; } = string.Empty;
        public bool isActive { get; set; } 

    }
}
 