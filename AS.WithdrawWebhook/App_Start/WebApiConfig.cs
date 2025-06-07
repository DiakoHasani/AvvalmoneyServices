using AS.BL;
using AS.WithdrawWebhook.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using Unity.AspNet.WebApi;

namespace AS.WithdrawWebhook
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            var container = new DependencyInjection().Config();
            container.RegisterType<IRegisterJwtToken, RegisterJwtToken>();
            config.DependencyResolver = new UnityDependencyResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
