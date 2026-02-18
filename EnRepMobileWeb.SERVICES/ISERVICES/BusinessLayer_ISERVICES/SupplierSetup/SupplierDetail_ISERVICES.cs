using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.BusinessLayer.SupplierDetail;
using System.Data;


namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
    public interface SupplierDetail_ISERVICES
    {
        DataTable GetAccountGroupDAL(string CompID, string type);
        DataSet GetAllDropDown(string CompID, string type,string Br_ID);
        DataTable GetGlReportingGrp(string Comp_ID, string Br_ID, string gl_repoting);
        DataSet GetBrList(string CompID);
        DataTable GetSuppcoaDAL(string CompID,string type, string SuppId);
        DataTable GetBrListDAL(string CompID);
        DataTable GetcategoryDAL(string CompID);
        DataTable GetsuppportDAL(string CompID);
        DataSet checkDependencyAddr(string Comp_ID, string custId, string addr_id);
        //DataTable GetsuppcurrDAL(string Comp_ID);
        DataTable GetCurronSuppTypeDAL(string CompID, string Supptype);
        DataTable CheckDependcyGstno(string Comp_ID, string Supp_Id, string SupplierGst);
        Dictionary<string, string> SuppCityDAL(string GroupName);
       string insertSupplierDetails(DataTable SupplierDetail, DataTable SupplierBranch, DataTable SupplierAttachments, DataTable SupplierAddress,int PaymentAlert,DataTable LicenceDetail);       
        DataSet GetviewSuppdetailDAL(string Suppcode, string CompID,string Br_Id);
        DataSet SupplierDetailDelete(SupplierDetail _SupplierDetail, string comp_id, string supp_id);
        string SupplierApprove(SupplierDetail _SupplierDetail, string comp_id,string app_id,string supp_id, string mac_id);
        /*----------------------Code Start of Country,state,district,city--------------------------*/
        DataTable GetCountryListDDL(string CompID, string SuppType);
        DataTable GetstateOnCountryDDL(string ddlCountryID);
        DataTable GetDistrictOnStateDDL(string ddlStateID);
        DataTable GetCityOnDistrictDDL(string ddlDistrictID);
        DataSet GetStateCode(string stateId);
        /*----------------------Code End of Country,state,district,city--------------------------*/

      DataSet  GetSupplierPurchaseDetail(string Comp_ID, string Br_Id, string Supp_Id, string FromDate, string ToDate);
    }
}
