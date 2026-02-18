using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.MISCollectionDetail
{
     public interface MISCollectionDetail_IService
    {
        DataTable GetRegionDAL(string CompID);
        DataTable GetcategoryDAL(string CompID);
        DataTable GetCustportDAL(string CompID);
        DataSet GetCustomerDropdowns(string compId, string StateName, string cityName);
        DataSet BindStateListData(string comp_id, string br_id, string SarchValue);
        DataSet BindCityListdata(string comp_id, string br_id, string SarchValue, string state_id);
        DataSet GetCollectionDetailList(string CompId, string BranchID, string UserID, string Cust_id, string Cat_id, string Prf_id, string Reg_id, string AsDate, int Curr_Id, string Flag, int Acc_Id, string ReceivableType, string ReportType, string brlist, string customerZone, string CustomerGroup, string state_id, string city_id,string includeZero,string sales_per);
        DataSet GetSalesAmtDetails(string CompId, string BranchID, string Cust_id, string AsDate, int CurrId, string ReceivableType, string ReportType, string inv_no, string inv_dt, string brlist, string user_id,string includeZero);
        DataSet SearchPaidAmountDetail(string CompId, string BranchID, string UserID, string Cust_id, string curr_id, string AsDate, string ReceivableType, string ReportType, string brlist, string includeZero);
        DataTable GetSalesPersonList(string compId, string brId, string userid);
    }
}
