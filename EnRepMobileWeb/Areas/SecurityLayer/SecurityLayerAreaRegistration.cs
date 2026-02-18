using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.SecurityLayer
{
    public class SecurityLayerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SecurityLayer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SecurityLayer_default",
                "SecurityLayer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}