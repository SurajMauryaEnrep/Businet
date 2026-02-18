using EnRepMobileWeb.MODELS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EnRepMobileWeb.MODELS.Factory_Settings.Organization_Setup
{
   public class OrganizationSetupModel
    {/*Start By using Commonmodel to Common Address Information*/
        public CommonAddress_Detail _CommonAddress_Detail { get; set; }
        public string IEC_Code { get; set; }
        public string Currency_Formet_id { get; set; }
        public string Currency_Formet { get; set; }
        public string ValidEntity_Name { get; set; }
        public string ValidEntity_prefix { get; set; }
        public string WebSite { get; set; }
        public string MSME_Number { get; set; }
        public string hdnAttachment { get; set; }
        public string hdnAttachment1 { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string hdnLincenceDetail { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public List<CmnCountryList> countryList { get; set; }
        public List<CmnStateList> StateList { get; set; }
        public List<CmnDistrictList> DistrictList { get; set; }
        public List<CmnCityList> cityLists { get; set; }
        /*End By using Commonmodel to Common Address Information*/
        public string Title { get; set; }
        public string create_id { get; set; }
        public string BtnName { get; set; }
        public string commEdit { get; set; }
        public string creat_dt { get; set; }
        public string DeleteCommand { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_id { get; set; }
        public string mod_dt { get; set; }
        public string mod_by { get; set; }
        public string app_status { get; set; }
        public string EntityName { get; set; }
        public string HeadBranch { get; set; }
        public string HeadOffice { get; set; }
        public string ho_id { get; set; }

        public string EntityPrefix { get; set; }
        public string FY_StartDate { get; set; }
        public string FY_EndDate { get; set; }
        public string def_lang { get; set; }
        public string defa_lang { get; set; }
        public string Currency_name { get; set; }
        public string Currency_id { get; set; }
        public string Curren_name { get; set; }
     
        public string cont_pers { get; set; }
        public string cont_pers1 { get; set; }
        public string cont_num2 { get; set; }
        public string gst_num { get; set; }
        public string GSTMidPrt { get; set; }
        public string GSTLastPrt { get; set; }
        public string GSTNumber { get; set; }
        public string cont_num1 { get; set; }
        public string cont_email { get; set; }
       
        public string cont_email1 { get; set; }
        public string cust_city { get; set; }
        public string Address { get; set; }
        public string img_six { get; set; }
        public string attatchmentdetail { get; set; }
        public string img_digi_sign { get; set; }
        public string digi_sign { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string Quantity { get; set; }
        public string Quantity_Value { get; set; }
        public string Exchange { get; set; }
        public string Weight { get; set; }
        public string Rate { get; set; }
        public string Doc_name { get; set; }
        public string Prefix { get; set; }
        public string bank_benef { get; set; }
        public string bank_name { get; set; }
        public string bank_add { get; set; }
        public string bank_acc_no { get; set; }
        public string swift_code { get; set; }
        public string create_dt { get; set; }
        public string ifsc_code { get; set; }
        public string PANNumber { get; set; }
        public string TransType { get; set; }
        //public string PIN { get; set; }
        //public string DistrictAndZone { get; set; }
        //public string State { get; set; }
        //public string District { get; set; }
        //public string Country { get; set; }
        public string Cus_State { get; set; }
        public string cust_Country { get; set; }
        public int comp_id { get; set; }
        public string comp_alias { get; set; }
        public string ho_com_name { get; set; }
        public string LandlineNumber { get; set; }
        public string OrgAddressDetails { get; set; }/*ADD BY HINA SHARMA ON 04-08-2025*/
        public string comp_add_id { get; set; }/*ADD BY HINA SHARMA ON 04-08-2025*/
        public List<HO_List> hO_Lists { get; set; }
        public List<Lang_List> lang_Lists { get; set; }
        //public List<CityList> cityLists { get; set; }

        public List<CurrencyNameLIst> _currencyNameList { get; set; }
        public List<DocList> DocumentList { get; set; }
    }
    public class DocList
    {
        public string Doc_id { get; set; }
        public string DOC_Name { get; set; }
    }
    public class HO_List
    {
        public string HO_ID { get; set; }
        public string HO_Name { get; set; }
    }
    public class Lang_List
    {
        public string Lang_ID { get; set; }
        public string Lang_Name { get; set; }
    }
    public class CurrencyNameLIst
    {
        public int curr_id { get; set; }
        public string curr_name { get; set; }
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
    //public class CityList
    //{
    //    public string CityId { get; set; }
    //    public string CityName { get; set; }
    //}
    public class ItemMenuSearchModel
    {
        public string search_tree_menu { get; set; }
        public string Comp_ID { get; set; }
    }
    public class ParentNode
    {

        public string label { get; set; }
        public string value { get; set; }
        public List<childrenNode> children { get; set; }

    }
    public class childrenNode
    {

        public string label { get; set; }
        public string value { get; set; }
        //public List<SubchildrenNode> children { get; set; }
    }
    public class Header
    {
        public ParentNode TreeStr { get; set; }
    }
}
