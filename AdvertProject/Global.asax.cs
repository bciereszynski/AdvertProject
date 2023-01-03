using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AdvertProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context == null || context.Session == null)
            {
                return;
            }
            var cookie = Request.Cookies["lang"];
            CultureInfo language = cookie != null ? new CultureInfo(cookie.Value) : null;
            if (language == null)
            {
                cookie = new HttpCookie("lang");
                cookie.Value = ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
                language = new CultureInfo(cookie.Value);

            }
            cookie.Expires = DateTime.UtcNow.AddDays(30);
            Response.Cookies.Add(cookie);

            Thread.CurrentThread.CurrentUICulture = language;
            Thread.CurrentThread.CurrentCulture = language;
        }
    }

    
}
