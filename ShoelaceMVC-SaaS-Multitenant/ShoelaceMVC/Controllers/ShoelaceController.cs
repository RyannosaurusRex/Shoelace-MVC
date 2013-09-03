using ShoelaceMVC.Repositories;
using ShoelaceMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;

namespace ShoelaceMVC.Controllers
{
    public class ShoelaceController : Controller
    {
        protected UnitOfWork db = null;
        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            using (var ctx = new ShoelaceDbContext())
            {
                var tid = RouteData.GetTenantId();
                if (User.Identity.IsAuthenticated)
	            {
                    var ct = ctx.Users.Where(x => x.AccountId == tid && x.UserName == User.Identity.Name).Count();
                    if (ct <= 0)
                    {
                        HttpContext.SignOut();
                        filterContext.Result = RedirectToAction("Index", "Home");
                    }
                }
            }
            base.OnAuthentication(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var tid = RouteData.GetTenantId();
            if (true)
	        
            if (tid == -1)
            {
                filterContext.Result = Redirect(WebConfigurationManager.AppSettings["SignupUrl"]);
                return;
            }
            else
            {
                db = new UnitOfWork(tid);
            }
            base.OnActionExecuting(filterContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}