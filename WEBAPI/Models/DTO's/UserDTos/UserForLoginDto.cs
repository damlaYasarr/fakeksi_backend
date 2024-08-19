namespace WEBAPI.Models.DTO_s.UserDTos
{
    public class UserForLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; internal set; } // Consider if you need to expose the Token property
    }

    
}
