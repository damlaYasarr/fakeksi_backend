using Microsoft.IdentityModel.Tokens;

namespace WEBAPI.Utilities.Security.Encryption
{
    public class SigningCredidentialHelpers
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey security)
        {
            return new SigningCredentials(security,SecurityAlgorithms.HmacSha512Signature );
        }
            
    }
}
