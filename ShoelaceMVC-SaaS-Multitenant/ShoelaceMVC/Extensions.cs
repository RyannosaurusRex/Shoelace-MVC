using MvcDomainRouting.Code;
using ShoelaceMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ShoelaceMVC
{
    public static class Extensions
    {
        public static string GetSubdomain(this RouteData routeData)
        {
            DomainRoute dr = (DomainRoute)routeData.Route;
            return dr.Subdomain;
        }

        public static string GetDomain(this RouteData routeData)
        {
            DomainRoute dr = (DomainRoute)routeData.Route;
            return dr.Domain;
        }

        /// <summary>
        /// Gets the tenantId based on the domain.
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns>The Tennant Id, which is the Account.Id associated with the domain in the route information.</returns>
        public static int GetTenantId(this RouteData routeData)
        {
            DomainRoute dr = (DomainRoute)routeData.Route;
            ShoelaceDbContext db = new ShoelaceDbContext();
            int tenantId = -1;
            var acc = db.Accounts.FirstOrDefault(x => x.Subdomain == dr.Subdomain);
            if(null != acc)
            {
                tenantId = acc.Id;
            }
            else
            {
                acc = db.Accounts.FirstOrDefault(x => x.VanityDomain == dr.Domain);
                if (acc != null)
                {
                    tenantId = acc.Id;
                }
            }
            return tenantId;
        }
    }
}