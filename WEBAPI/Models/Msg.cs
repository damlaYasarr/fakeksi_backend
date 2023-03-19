using System.ComponentModel.DataAnnotations;

namespace WEBAPI.Models
{
    public class Msg
    {
        [Key]
        public int msg_id { get; set; }
        public string msg_detail { get; set; }
        public int msg_sender_id { get; set; }
        public int msg_receiver_id { get; set; }
        public string msg_date { get; set; }
        public bool isOpened { get; set;}
    }
}
