using MvcDomainRouting.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShoelaceMVC
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // TODO: This doesn't quite work yet.  For some reason
            // the MVC and Api routes get confused and MVC puts
            // Api routes as the link for some buttons.  Lame.

            /*config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            */

            /*config.Routes.MapHttpRoute("SubdomainApiRoute", new HttpDomainRoute(
                "{subdomain}.myapp.com",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
                ));

            config.Routes.MapHttpRoute("LocalApiRoute", new HttpDomainRoute(
                "{domain}",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
                ));*/
        }
    }
}
