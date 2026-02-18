using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer
{
    public class BusinessLayerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BusinessLayer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BusinessLayer_default",
                "BusinessLayer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}