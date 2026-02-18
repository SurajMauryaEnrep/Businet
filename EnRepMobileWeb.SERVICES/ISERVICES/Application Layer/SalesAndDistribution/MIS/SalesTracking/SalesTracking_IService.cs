using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.SalesTracking
{
    public interface SalesTracking_IService
    {
        DataTable GetSalesTrackingList(string compId, string brId,string userid, string orderno, string custId, string slsPersId, string currId,string orderType, string itemId,
            string fromDate, string toDate,string NotFillterOrderType);
        DataSet GetSalesTrackingList(string compId, string brId, string userid, string orderno, string custId, string slsPersId, string currId, string orderType, string itemId,
            string fromDate, string toDate, string NotFillterOrderType,
            string custCat, string CustPort, string custzone, string custgroup, string custstate, string custcity,string brlist, string skip= "0", string pageSize= "25", string searchValue="",string sortColumn= "SrNo", string sortColumnDir="ASC",string Flag="");
        DataTable GetPONumberList(string compId, string brId, string orderType, string suppId, string currId);
        DataTable GetSalesPersonList(string compId, string brId,string userid);
    }
}
