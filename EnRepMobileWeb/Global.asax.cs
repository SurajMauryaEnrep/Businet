using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace EnRepMobileWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
          //  FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //SqlDependency.Start(ConfigurationManager.ConnectionStrings["SignalrCon"].ConnectionString);
            //  GlobalFilters.Filters.Add(new HandleErrorAttribute());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            UnityConfig.RegisterComponents();
        }
        public void Application_End()
        {
            //SqlDependency.Stop(ConfigurationManager.ConnectionStrings["SignalrCon"].ConnectionString);
        }
        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            Session.Timeout = 60;
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
            if (cookie != null && cookie.Value != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            }
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.UrlReferrer == null ||
                        filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            {
                filterContext.Result = new RedirectToRouteResult(new
                               RouteValueDictionary(new { controller = "Home", action = "Index", area = "" }));
            }
        }
    }
}
