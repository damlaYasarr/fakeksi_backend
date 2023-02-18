using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace WEBAPI.Utilities.Security.Encryption
{
    public class SecurityKeyHelper
    { //control token-appjson used
        public static SecurityKey CreateSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}
