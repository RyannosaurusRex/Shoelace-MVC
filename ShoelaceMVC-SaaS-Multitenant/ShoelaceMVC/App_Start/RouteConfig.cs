using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcDomainRouting.Code;

namespace ShoelaceMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("UserSite", new DomainRoute(
                "{subdomain}.ekklio.com",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                ));

            routes.Add("LocalSite", new DomainRoute(
                "localhost",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                ));

            
        }
    }

    public class SubdomainRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            string url = httpContext.Request.Headers["HOST"];
            int index = url.IndexOf(".", System.StringComparison.Ordinal);

            if (index < 0)
            {
                values.Add("TenantId", "zero");
                return true;
            }

            //Because the domain names will end with ".com",".net",
            //there may be a "." in the url.So check if the sub is not "yourdomainname" or "www" at runtime.
            string sub = url.Split('.')[0];
            if (sub == "www" || sub == "yourdomainname" || sub == "mail")
            {
                return false;
            }

            //Add a custom parameter named "user". Anythink you like :)
            values.Add("TenantId", sub);
            return true;
        }
    }
}
