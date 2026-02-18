using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.AccountReceivable
{
   public interface AccountReceivable_ISERVICE
    {
        DataTable GetRegionDAL(string CompID);
        DataTable GetcategoryDAL(string CompID);
        DataTable GetCustportDAL(string CompID);
        DataSet GetUserRangeDetail(string CompID, string UserID);
        String InsertUserRangeDetail(string CompID, string user_id, string range1, string range2, string range3, string range4, string range5);
        DataSet GetAgingDetailList(string CompId, string BranchID, string UserID, string Cust_id, string Cat_id, string Prf_id, string Reg_id, string Basis, string AsDate, int Curr_Id, string Flag, int Acc_Id, string ReceivableType, string ReportType, string brlist,string sales_per,string customerZone,string CustomerGroup,string state_id,string city_id);
        DataTable GetInvoiceDetailList(string CompId, string BranchID, string Cust_id, string lrange, string urange, string Basis, string AsDate, int CurrId, string ReceivableType, string ReportType, string inv_no, string inv_dt, string brlist,string sls_per,string user_id);
        DataSet SearchAdvanceAmountDetail(string compId, string brId, string accId, int CurrId, string AsDate, string Basis, string ReceivableType, string brlist);
        DataSet SearchPaidAmountDetail(string compId, string brId, string InVNo, string InvDate,string cust_id);
        DataSet GetInvoiceDeatilsForPrint(string CompID, string BrchID, string invNo, string invDate, string dataType);
        DataSet GetSalesInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date, string inv_type);
        DataSet GetSlsInvGstDtlForPrint(string CompID, string BrchID, string SI_No, string SI_Date, string inv_type);
        DataSet GetGLAccountReceivablePrintData(string compId, string brId, string accId, string currId, string asOnDate, string userId, string brlist);
        DataTable GetSalesPersonList(string compId, string brId, string userid);
        DataSet GetCustomerDropdowns(string compId,string StateName, string cityName);
        DataSet BindStateListData(string comp_id, string br_id, string SarchValue);
        DataSet BindCityListdata(string comp_id, string br_id, string SarchValue,string state_id);
    }
}
