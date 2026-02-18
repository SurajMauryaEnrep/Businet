using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISStockReservation
{
    public interface MISStockReservation_IService
    {
        DataTable GetStockReservationReport(string compId, string brId, string itemId, string itemGroupId, string warehouseId);

    }
}
