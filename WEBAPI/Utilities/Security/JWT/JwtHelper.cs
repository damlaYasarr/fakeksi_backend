using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WEBAPI.Models;
using WEBAPI.Utilities.Extensions;
using WEBAPI.Utilities.Security.Encryption;

namespace WEBAPI.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper

    {
        //IConfiguration--> read appsetting 
        public IConfiguration Configuration { get;}
        private TokenOptions _tokenOptions;
        private DateTime _accessTokenExpiration; 

        public JwtHelper(IConfiguration configuration) {
        Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

       
        public AccessToken CreateToken(Users user, List<OperationClaim> operationclaims)
        {
           
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredidentialHelpers.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationclaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, Users user,
           SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(Users user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            
            claims.AddNameIdentifier(user.user_id.ToString());
            claims.AddEmail(user.email);
            claims.AddName($"{user.name}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }

    }
}
