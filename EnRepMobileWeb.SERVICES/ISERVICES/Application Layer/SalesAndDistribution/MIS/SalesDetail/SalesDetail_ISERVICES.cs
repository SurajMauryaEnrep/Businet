using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.SalesDetail
{
   
    public interface SalesDetail_ISERVICES
    {
        DataSet GetSales_Detail(string CompID, string BrID, string userid, string cust_id,string reg_name, string sale_type, string curr_id, string productGrp
            ,string Product_Id,string productPort,string custCat,string CustPort, string inv_no,
            string inv_dt, string sale_per,string From_dt, string To_dt, string Flag,string HSN_code, string custzone, string custgroup, string custstate, string custcity,string brlist,string uom_id);
        DataSet GetCustomerList(string CompID, string SuppName, string BranchID, string CustType);
        DataTable GetRegionDAL(string CompID);
        DataTable GetcategoryDAL(string CompID);
        DataTable GetCustportDAL(string CompID);
        DataSet BindProductNameInDDL(string CompID, string BrID);
        DataSet PaidAmountDetail(string compId, string brId, string InVNo, string InvDate, string Curr_id , string Fromdate, string Todate, string Flag);
        Dictionary<string, string> ItemSetupHSNDAL(string CompID, string HSNName);
        DataSet GetCustCommonDropdownDAL(string CompID,string SearchVal, string Stateid);
    }
}
