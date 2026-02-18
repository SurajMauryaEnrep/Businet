using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using EnRepMobileWeb.MODELS.Common;

namespace EnRepMobileWeb.MODELS.BusinessLayer.CustomerDetails
{
    public class CustomerDetails
    {
        /*Start By using Commonmodel to Common Address Information*/
        public CommonAddress_Detail _CommonAddress_Detail { get; set; }
        public string Addrcont_no { get; set; }
        public string Addrcont_pers { get; set; }
        public string cust_zone { get; set; }
        public string cust_group { get; set; }
        public string PageLevelDisable { get; set; }
        public string GstFlagValid { get; set; }
        public string DefaultTransporter_ID { get; set; }
        public string DefaultTransporter { get; set; }
        public string SalesHistorySearch { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string GlRepoting_Group_Name { get; set; }
        public string GlRepoting_Group_ID { get; set; }
        public string GlRepoting_Group { get; set; }
        public string DocumentMenuId { get; set; }
        public string CustomerDependcy { get; set; }
        public string custname { get; set; }
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
        public int acc_grp_id { get; set; }
        public DataTable CustomerBranchList { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string SaveAndApproveBtnDisatble { get; set; }
        public string Title { get; set; }
        public string ListFilterData1 { get; set; }
        public string LincenceDetail { get; set; }
        public string CustomerFromProspect { get; set; }
        public string Prospect_id { get; set; }
        public DataSet ProspectDetail { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string Guid { get; set; }
        //public string Message { get; set; }
        public string Command { get; set; }
        public string AppStatus { get; set; }
        public string BtnName { get; set; }
        public string CustCode { get; set; }
        public string FinStDt { get; set; }
        public string CSSearch { get; set; }
        public static string ValueRequired { get; set; }
        public Boolean ApprovedFlag { get; set; }
        //public string TransType { get; set; }
        public string cust_id { get; set; }
        public string cust_name { get; set; }
        public string cust_alias { get; set; }
        public string Cust_type { get; set; }
        public int cust_catg { get; set; }
        public int cust_port { get; set; }
        public string cust_ports { get; set; }
        //public string cust_ports { get; set; }
        public int cust_pr_grp { get; set; }
        public string cust_pr_pol { get; set; }
        public string customer_billing_address { get; set; }
        //public int cust_city { get; set; }
        //public int? district_id { get; set; }
        //public string Custdist { get; set; }
        //public int state_id { get; set; }     
        //public string CustState { get; set; }
        //public int country_id { get; set; }
        //public string CustCountry { get; set; }
        public int cust_coa { get; set; }
        public string ddl_cust_coa { get; set; }
        public int cust_region { get; set; }
        public string cust_ship_add1 { get; set; }
        public Boolean act_stats { get; set; }
        public string inact_reason { get; set; }
        public string cust_rmarks { get; set; }
        public int? PaymentAlert { get; set; }
        public string D_InActive { get; set; }
        public Boolean cust_hold { get; set; }
        public string hold_reason { get; set; }
        public int create_id { get; set; }
        public int app_id { get; set; }
        public int mod_id { get; set; }
        public string app_status { get; set; }
        public int? curr { get; set; }
        public string Regn_num { get; set; }
        public string gst_num { get; set; }
        public string tan_num { get; set; }
        public string CompCountryID { get; set; }
        public string CompCountry { get; set; }
        public string pan_num { get; set; }
        public string cont_pers { get; set; }
        public string cont_email { get; set; }      
        public string cont_num1 { get; set; }      
        public string cont_num2 { get; set; }
        //public string supp_bill_pincode { get; set; }
        public string mac_id { get; set; }

        //stp$item$org$detail table
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public float credit_limit { get; set; }
        public int credit_days { get; set; }
        public string apply_on { get; set; }

        public string  CustCoa { get; set; }

        public string bank_name { get; set; }
        public string bank_branch { get; set; }
        public string bank_add { get; set; }
        public string bank_acc_no { get; set; }
        public string ifsc_code { get; set; }
        public string swift_code { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CompId { get; set; }

        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }

        public string UserIP { get; set; }
        public string cust { get; set; }
        public string DeleteCommand { get; set; }

        public int SalesPersID { get; set; }
        public string attatchmentdetail { get; set; }
        public string GLAccountNm { get; set; }
        public List<CustCoa> CustCoaNameList { get; set; }
        public List<Category> CategoryList { get; set; }

        public List<PortFolio> PortFolioList { get; set; }
        public List<custzone> custzoneList { get; set; }
        public List<custgroup> cust_groupList { get; set; }

        public List<curr> currList { get; set; }
        
        public List<cust> custList { get; set; }

        public List<PriceGroup> PriceGroupList { get; set; }
        public List<Region> RegionList { get; set; }
        public List<ListDefaultTransporter> DefaultTransporterList { get; set; }

        //public List<CustomerBranch> CustomerBranchList { get; set; }


        public string CustomerBranchDetails { get; set; }

        public string CustomerAddressDetails { get; set; }

        public HttpPostedFile ImageFile { get; set; }


        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }

        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string Status { get; set; }
        public string item_ActStatus { get; set; }
        public string _item_cust_type { get; set; }
        public string cust_categ { get; set; }
        public string ListFilterData { get; set; }
        public List<Status> StatusList { get; set; }
        //public List<CityList> cityLists { get; set; }
        public string curr_depncy { get; set; }
        public string HdnInterBranch { get; set; }
        public string Gst_Cat { get; set; }
        public List<AccountGroup> AccountGroupList { get; set; }
        public List<GlReportingGroup> GlReportingGroupList { get; set; }
        public bool InterBranch { get; set; }
        public bool TCSApplicable { get; set; }

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
    public class GlReportingGroup
    {
        public string Gl_rpt_id { get; set; }
        public string Gl_rpt_Name { get; set; }

    }
    public class CustomerDetailsattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    //public class CityList
    //{
    //    public string CityId { get; set; }
    //    public string CityName{ get; set; }
    //}

    public class Status
{
    public string status_id { get; set; }
    public string status_name { get; set; }
}

public class CustCoa
    {
        public string acc_name { get; set; }

        public string acc_id { get; set; }
    }
    public class Category
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }
        
    }
    public class PortFolio
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }

    }
    public class custgroup
    {
        public string CustGrp_id { get; set; }
        public string CustGrp_val { get; set; }

    }
    public class custzone
    {
        public string custzone_id { get; set; }
        public string custzone_val { get; set; }

    }
    public class curr
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }
        
    } 
    public class cust
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
        
    }
    public class Region
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }

    }    
    public class ListDefaultTransporter
    {
        public string Transporter_id { get; set; }
        public string Transporter_val { get; set; }

    }
    public class PriceGroup
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }

    }
    //public class  CustomerBranch
    //{
    //    public string comp_id { get; set; }
    //    public string comp_nm { get; set; }     
    //}
}
