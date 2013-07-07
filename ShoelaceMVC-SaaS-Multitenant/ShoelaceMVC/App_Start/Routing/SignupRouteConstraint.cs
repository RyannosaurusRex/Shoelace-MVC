using System.Web;
using System.Web.Routing;

namespace Ekklio.Web.Routing
{
    public class SignupRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            string url = httpContext.Request.Headers["HOST"];
            int index = url.IndexOf(".", System.StringComparison.Ordinal);

            if (index < 0)
            {
                return false;
            }
            // Because the domain names will end with ".com",".net", we need to match and make sure it doesn't count the "subdomain" as our main name.
            //so probably there will be a "." in url.So check if the sub is not "yourdomainname" or "www" at runtime.
            string sub = url.Split('.')[0];
            if (sub == "register")
            {
                if (!values.ContainsKey("subdomain"))
                {
                    values.Add("subdomain", sub);
                }
                return true;
            }
            return false;
        }
    }
}