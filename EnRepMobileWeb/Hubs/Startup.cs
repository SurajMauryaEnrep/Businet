using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EnRepMobileWeb.Hubs.Startup))]

namespace EnRepMobileWeb.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
