using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerDetails;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
    public interface CustomerDetails_ISERVICES
    {
        DataTable GetAccountGroupDAL(string CompID, string type);
        DataTable GetCustcoaDAL(string CompID, string type, string Cust_id);
        DataTable GetcategoryDAL(string CompID);
        DataTable GetCustportDAL(string CompID);

        //DataTable GetCustcurrDAL(string CompID,string Supptype);
        DataTable GetRegionDAL(string CompID);
        DataTable GetCustPriceGrpDAL(string CompID);
        DataTable GetBrListDAL(string CompID);
        DataSet GetviewCustdetail(string Custcode, string CompID,string Br_Id);        
        DataTable GetCurronSuppTypeDAL(string CompID, string Supptype);
        DataSet GetBrList(string CompID);    
        string insertCustomerDetails(DataTable CustomerDetail, DataTable CustomerBranch, DataTable CustomerAttachments,DataTable CustomerAddress,int PaymentAlert,DataTable LicenceDetail);
        Dictionary<string, string> SuppCityDAL(string GroupName);
        DataSet GetsuppDSCntrDAL(string SuppCity);
        DataSet checkDependencyAddr(string Comp_ID,string custId, string addr_id);
        DataTable GetGlReportingGrp(string Comp_ID, string Br_id, string gl_repoting);
        DataSet InsertCustomerAddrress(string comp_id, string CustId, string Address,string City,string District,string State, string Country, string GSTNo, string BillingAddress, string ShippingAddress);

        string CustomerDetailDelete(CustomerDetails _CustomerDetails, string comp_id, string cust_id);
        string CustomerApprove(CustomerDetails _CustomerDetails, string comp_id, string app_id, string cust_id, string mac_id);
        DataSet GetFromDateCust(string CompId, string BrID);
        /*----------------------Code Start of Country,state,district,city--------------------------*/
        DataTable GetCountryListDDL(string CompID, string CustType);
        DataTable GetstateOnCountryDDL(string ddlCountryID);
        DataTable GetDistrictOnStateDDL(string ddlStateID);
        DataTable GetCityOnDistrictDDL(string ddlDistrictID);
        DataSet GetStateCode(string stateId);
        DataSet GetAllDropDownList(string comp_id, string Br_ID);
        DataSet GetCustomerSalesDetail(string Comp_ID, string Br_Id, string cust_ID, string FromDate, string ToDate);
        /*----------------------Code End of Country,state,district,city--------------------------*/

        DataTable CheckDependcyGstno(string Comp_ID, string Cust_Id, string custGst);
    }
}
