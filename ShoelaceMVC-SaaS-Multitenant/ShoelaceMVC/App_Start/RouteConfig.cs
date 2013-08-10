using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcDomainRouting.Code;
using System.Web.Http;

namespace ShoelaceMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("UserSite", new DomainRoute(
                "{subdomain}.myapp.com",
                "{controller}/{action}/{id}",
                new {subdomain = UrlParameter.Optional, controller = "Home", action = "Index", id = UrlParameter.Optional }
                ));
            
            routes.Add("LocalSite", new DomainRoute(
                "localhost",
                "{controller}/{action}/{id}",
                new { subdomain = UrlParameter.Optional, controller = "Home", action = "Index", id = UrlParameter.Optional }
                ));

            routes.IgnoreRoute("{file}.css");
        }
    }
}
