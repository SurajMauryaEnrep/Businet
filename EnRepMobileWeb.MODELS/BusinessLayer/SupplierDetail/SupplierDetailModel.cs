using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.MODELS.BusinessLayer.SupplierDetail
{
    public class SupplierDetail
    {/*Start By using Commonmodel to Common Address Information*/
        public CommonAddress_Detail _CommonAddress_Detail { get; set; }
        public string PurchaseHistorySearch { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string GlRepoting_Group_Name { get; set; }
        public string GlRepoting_Group_ID { get; set; }
        public string GlRepoting_Group { get; set; }
        public string suppname { get; set; }
        public string PageLevelDisable { get; set; }
        public string SupplierDependcy { get; set; }
        public string contpers { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public List<CmnCountryList> countryList { get; set; }
        public List<CmnDistrictList> DistrictList { get; set; }
        public List<CmnCityList> cityLists { get; set; }
        public List<CmnStateList> StateList { get; set; }
        /*End By using Commonmodel to Common Address Information*/


        public string curr_depncy { get; set; }
        public string Saveapprovebtn { get; set; }
        public string LincenceDetail { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public DataSet ProspectDetail { get; set; }
        public string brid { get; set; }
        public string Prospect_id { get; set; }
        public string ListFilterData1 { get; set; }
        public string SupplierFromProspect { get; set; }
        public string Guid { get; set; }
        public DataTable CustomerBranchList { get; set; }
        public string SuppCode { get; set; }
        public string AppStatus { get; set; }
        public string SSearch { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string Title { get; set; }
        public static string ValueRequired { get; set; }
        public Boolean ApprovedFlag { get; set; }
        public string TransType { get; set; }
        public string supp_id { get; set; }
        public string supp_name { get; set; }
        public string supp_alias { get; set; }
        public string supp_type { get; set; }
        public string SupplierAddressDetail { get; set; }
        public int supp_catg { get; set; }
        public string ddl_supp_catg { get; set; }
        public string SupplName { get; set; }
        public string typ { get; set; }
        public int supp_port { get; set; } 
        public string ddl_supp_port { get; set; }
        public string suppcatg { get; set; }
        public string suppport { get; set; }
        public int supp_coa { get; set; }
        public string ddl_supp_coa { get; set; }
        public int? paym_term { get; set; }
        public int? PaymentAlert { get; set; }
        public string supp_rmarks { get; set; }
        public Boolean act_status { get; set; }
        public string inact_reason { get; set; }
        public Boolean on_hold { get; set; }
        public string onhold_reason { get; set; }
        public string create_id { get; set; }
        public string app_id { get; set; }
        public string mod_id { get; set; }
        public string app_status { get; set; }
        public int? curr { get; set; }
        public string supp_regn_no { get; set; }
        public string supp_gst_no { get; set; }
        public string supp_tan_no { get; set; }
        public string CompCountryID { get; set; }
        public string CompCountry { get; set; }
        public string supp_pan_no { get; set; }
        public string cont_pers { get; set; }
        public string cont_email { get; set; }
        public string cont_num1 { get; set; }
        public string cont_num2 { get; set; }       
        public string mac_id { get; set; }
        public string supp_city { get; set; }
        

        public string bank_name { get; set; }
        public string bank_branch { get; set; }
        public string bank_add { get; set; }
        public string bank_acc_no { get; set; }
        public string ifsc_code { get; set; }
        public string swift_code { get; set; }
        public string D_InActive { get; set; }
      

        //stp$item$org$detail table
        public int comp_id { get; set; }
        public int br_id { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CompId { get; set; }

        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string SupplierAddressDetails { get; set; }
        public string UserIP { get; set; }
        public string creat_dt { get; set; }
        public string mod_dt { get; set; }
        public string app_dt { get; set; }
        public string GLAccountName { get; set; }


        public string DeleteCommand { get; set; }
        public string attatchmentdetail { get; set; }
        public List<SuppCoa> SuppCoaNameList { get; set; }
        public List<SuppCategory> CategoryList { get; set; }
        public List<SuppPortFolio> PortFolioList { get; set; }
        public List<Supplier> SupplierList { get; set; }
        public List<curr> currList { get; set; }
        //public List<SuppCity> SuppCityList { get; set; }
        //public List<SupplierBranch> CustomerBranchList { get; set; }
        public string SupplierBranchDetails { get; set; }
        public HttpPostedFile ImageFile { get; set; }
        public string Status { get; set; }
        public string item_ActStatus { get; set; }
        public string ListFilterData { get; set; }
        public List<Status> StatusList { get; set; }
        public string HdnInterBranch { get; set; }
        public string Gst_Cat { get; set; }
        public int acc_grp_id { get; set; }
        public List<AccountGroup> AccountGroupList { get; set; }
        public string DocumentMenuId { get; set; }
        public bool InterBranch { get; set; }
        public bool TDSApplicable { get; set; }
        public string TDSApplicableOn { get; set; }
        public List<GlReportingGroup> GlReportingGroupList { get; set; }

    }
    public class GlReportingGroup
    {
        public string Gl_rpt_id { get; set; }
        public string Gl_rpt_Name { get; set; }

    }
    public class CmnCountryList
    {
        public string country_id { get; set; }
        public string country_name { get; set; }

    }
    public class CmnStateList
    {
        public string state_id { get; set; }
        public string state_name { get; set; }

    }
    public class CmnDistrictList
    {
        public string district_id { get; set; }
        public string district_name { get; set; }
    }
    public class CmnCityList
    {
        public string City_Id { get; set; }
        public string City_Name { get; set; }
    }
    
    public class AccountGroup
    {
        public string acc_grp_id { get; set; }
        public string AccGroupChildNood { get; set; }

    } 
    public class Supplier
    {
        public string SuppID { get; set; }
        public string SuppName{ get; set; }

    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }

    public class SuppCoa
    {
        public string acc_name { get; set; }

        public string acc_id { get; set; }
    }
    public class SuppCategory
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }

    }

    public class SuppPortFolio
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }

    }

    //public class SuppCity
    //{
    //    public string city_id { get; set; }
    //    public string city_name { get; set; }

    //}

    public class curr
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }

    }
    public class SupplierDetailattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    //public class SupplierBranch
    //{

    //    public string comp_id { get; set; }
    //    public string comp_nm { get; set; }

    //}
}
