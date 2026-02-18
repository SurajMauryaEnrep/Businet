using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.FactorySettings
{
    public class FactorySettingsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FactorySettings";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FactorySettings_default",
                "FactorySettings/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}