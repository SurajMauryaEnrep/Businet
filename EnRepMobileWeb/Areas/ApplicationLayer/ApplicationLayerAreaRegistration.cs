using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer
{
    public class ApplicationLayerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ApplicationLayer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ApplicationLayer_default",
                "ApplicationLayer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}