namespace WEBAPI.Models
{
    public class FileUpload
    {
        public int id { get; set; }
        public IFormFile image { get; set; }
    }
}
