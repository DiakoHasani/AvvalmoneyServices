using AS.Model.General;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(AS.WithdrawApi.Startup))]

namespace AS.WithdrawApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = ServiceKeys.WithdrawIssuer,
                    ValidAudience = ServiceKeys.WithdrawIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ServiceKeys.WithdrawJwtSecretKey))
                }
            });
        }
    }
}
