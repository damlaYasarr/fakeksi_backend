namespace WEBAPI.Models
{
    public class Msg
    {
        public int msg_id { get; set; }
        public string msg_detail { get; set; }
        public int msg_sender_id { get; set; }
        public int msg_receiver_id { get; set; }
        public string msg_date { get; set; }
    }
}
