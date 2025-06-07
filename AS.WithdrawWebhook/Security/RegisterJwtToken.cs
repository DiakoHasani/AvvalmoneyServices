using AS.Model.General;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace AS.WithdrawWebhook.Security
{
    public class RegisterJwtToken : IRegisterJwtToken
    {
        public string Register()
        {
            var issuer = ServiceKeys.WithdrawIssuer;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ServiceKeys.WithdrawJwtSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Name", "Bot")
            };

            var token = new JwtSecurityToken(issuer,
                issuer,
                permClaims,
                expires: ServiceKeys.WithdrawTimeLogin,
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public interface IRegisterJwtToken
    {
        string Register();
    }
}