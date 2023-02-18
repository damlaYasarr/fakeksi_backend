using WEBAPI.Models;

namespace WEBAPI.Utilities.Security.JWT
{
    public interface ITokenHelper
    {    //json web token create with user info
        AccessToken CreateToken(Users user, List<OperationClaim> operationclaims);

    }
}
