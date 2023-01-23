namespace WEBAPI.Models
{
    public class Followers
    {
        public int id { get; set; }
        public int follower_id { get; set; }
        public int followed_id { get; set; }
    }
}
