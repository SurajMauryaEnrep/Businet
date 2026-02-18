using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
     public interface CustomerSetup_ISERVICES
    {
         DataSet GetCustListDAL(string cust_id, string CompID);
        DataSet GetStatusList(string MenuID);
        DataSet GetCustomerListFilterDAL(string CompID, string CustID, string Custtype, string Custcat, string Custport, string CustAct, string CustStatus, string Glrtp_id);

         DataSet GetCustportDAL(string CompID);
         DataTable GetCustport(string CompID);

        DataSet GetcategoryDAL(string CompID);
        DataTable Getcategory(string CompID);
        DataTable GetGlReportingGrp(string Comp_ID, string Br_id, string gl_repoting);
        Dictionary<string, string> CustNameListDAL(string GroupName, string CompID);
        DataTable Bind_custList1(string GroupName, string CompID);
        DataSet GetDataAllDropDown(string GroupName, string CompID,string cust_id);
        DataSet GetVerifiedDataOfExcel(string compId, DataTable CustomerDetail, DataTable CustomerBranch, DataTable CustomerAddress);
        DataTable ShowExcelErrorDetail(string compId, DataTable CustomerDetail, DataTable CustomerBranch, DataTable CustomerAddress);
        string BulkImportCustomerDetail(string compId, string UserID,string BranchName,DataTable CustomerDetail, DataTable CustomerBranch, DataTable CustomerAddress);
        DataSet GetMasterDropDownList(string Comp_id, string Br_ID);
        DataSet GetCustLedgerDtl(string CompID, string Br_ID, string Cust_id,string CustAcc_id, string Curr_id);
    }
        
}
