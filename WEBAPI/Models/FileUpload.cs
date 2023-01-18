using System.ComponentModel.DataAnnotations.Schema;

namespace WEBAPI.Models
{
    public class FileUpload
    {
        public int id { get; set; }
        [NotMapped]
        public IFormFile image { get; set; }
    }
}
