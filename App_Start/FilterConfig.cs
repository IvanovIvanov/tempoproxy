using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace TempoProxy
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeIPAddressAttribute());
        }
    }

    public class AuthorizeIPAddressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string ipAddress = HttpContext.Current.Request.UserHostAddress;

            if (!IsIpAddressAllowed(ipAddress.Trim()))
            {
                context.Result = new HttpStatusCodeResult(403);
            }

            base.OnActionExecuting(context);
        }

        private bool IsIpAddressAllowed(string IpAddress)
        {
            if (!string.IsNullOrWhiteSpace(IpAddress))
            {
                string[] addresses = Convert.ToString(ConfigurationManager.AppSettings["AllowedIPAddresses"]).Split(',');
                return addresses.Where(a => a.Trim().Equals(IpAddress, StringComparison.InvariantCultureIgnoreCase)).Any();
            }
            return false;
        }
    }


}
