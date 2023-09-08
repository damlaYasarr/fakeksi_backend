namespace WEBAPI.Models.DTO_s
{
    public class GetMsgThumbnail
    {

        public string Sendername { get; set; }
        public string Msgdate { get; set; }
        public string Lastmsg { get; set; }
        public bool IsOpen { get; set; }
        public string Receivername { get; set; }
    }
}
