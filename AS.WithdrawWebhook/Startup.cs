using AS.Model.General;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(AS.WithdrawWebhook.Startup))]

namespace AS.WithdrawWebhook
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(new Microsoft.Owin.Security.Jwt.JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = ServiceKeys.WithdrawIssuer,
                    ValidAudience = ServiceKeys.WithdrawIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ServiceKeys.WithdrawJwtSecretKey)),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true
                }
            });
        }
    }
}
