namespace WEBAPI.Utilities.jwt
{
    public class TokenOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public int AccessTokenExpiration { get; set; } // Duration in minutes
    }
}
