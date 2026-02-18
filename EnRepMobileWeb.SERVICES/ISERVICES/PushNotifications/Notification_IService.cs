using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.PushNotifications
{
    public interface Notification_IService
    {
        DataTable GetAllUnreadNotifications(int companyId, int branchId, int userId);
        int UpdateReadStatus(string rowId);
    }
}
