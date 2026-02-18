using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesAndDistributionIServices
{
    public interface LSOList_ISERVICE
    {
        DataSet GetStatusList(string MenuID);
        DataSet GetSODetailListDAL(string CompId, string BrchID, string CustId,string Fromdate, string Todate, string Status,string UserID,string DocumentMenuId, string wfstatus, string SO_type,string sales_person);
        Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType,string DocId);
       DataSet GetAllData(string CompID, string SuppName, string BranchID, string CustType
           , string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus, string SO_type,string sls_per);
        DataSet GetSO_Detail(string CompId, string BrID, string SONo, string SODate);
        DataSet GetSOTrackingDetail(string CompId, string BrID, string SONo, string SODate);
        DataSet GetProductionTrackingDetail(string CompId, string BrID, string SONo, string SODate);
        DataTable GetProductionPlan_DetailsInfo(string CompID, string BrID, string Item_id, string Plan_no, string Plan_dt, string flag);
        DataSet GetItemQCParamDetail(string CompID, string br_id, string ItemID, string qc_no, string qc_dt);
    }
}
