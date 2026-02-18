using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.ShipmentDetail
{
    public interface ShipmentDetail_IService
    {
        DataTable GetShipmentMISDetailsReport(string compId, string brId, string fromDate, string toDate, string shipmentType,
            string customerId, string ItemId, string showAs);
        DataTable GetItemsList(string compId);
    }
}
