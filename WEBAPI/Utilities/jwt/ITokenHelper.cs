using System.Security.Claims;

namespace WEBAPI.Utilities.jwt
{
    public interface ITokenHelper
    {
        string CreateToken(IEnumerable<Claim> claims);
        ClaimsPrincipal ValidateToken(string token);
    }
}
