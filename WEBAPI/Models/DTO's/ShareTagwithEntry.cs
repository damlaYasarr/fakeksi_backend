namespace WEBAPI.Models.DTO_s
{
    public class ShareTagwithEntry
    {
        public int tag_id { get; set; }
        public int user_id { get; set; }
        public int entry_id { get; set; }
        public string tag { get; set; }
        public string entry { get; set;}
    }
}
