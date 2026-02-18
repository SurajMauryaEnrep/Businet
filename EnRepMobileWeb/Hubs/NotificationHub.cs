using Microsoft.AspNet.SignalR;

namespace EnRepMobileWeb.Hubs
{
    public class NotificationHub : Hub
    {
        public static void show()
        {
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            hubContext.Clients.All.displayCustomer();
        }
    }
}