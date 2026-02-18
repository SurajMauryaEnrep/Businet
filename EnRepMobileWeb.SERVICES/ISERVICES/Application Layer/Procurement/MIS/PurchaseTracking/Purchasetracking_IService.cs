using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.PurchaseTracking
{
    public interface Purchasetracking_IService
    {
        DataSet GetPoTrackingDetailsMIS(string compId, string brId, string poNo, string suppId, string orderType, string itemId, string currId,
            string fromDate, string toDate,string ItemType, string NotFillterOrderType, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir, string SuppCat, string SuppPort, string flag="",string bridlist="");
        DataTable GetOrderNumberList(string compId, string brId, string orderType, string suppId, string currId, string SuppCat, string SuppPort, string SearchName="");
    }
}
