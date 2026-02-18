using EnRepMobileWeb.MODELS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.ProspectSetup
{
    public class ProspectSetupMODEL
    {
        /*Start By using Commonmodel to Common Address Information*/
        public CommonAddress_Detail _CommonAddress_Detail { get; set; }
        public string ProspectFromProd { get; set; }
        public string ScrDocumentMenuID { get; set; }
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
        public string CustCode { get; set; }
        public string SuppCode { get; set; }
        public string ILSearch { get; set; }
        public string PQSearch { get; set; }
        public string DocumentStatus { get; set; }
        public string S_ProsList { get; set; }
        public string Gst_Cat { get; set; }
        public string AppStatus { get; set; }
        public DataSet ProspectDetail { get; set; }
        public string EntityType { get; set; }
        public string ProsCode { get; set; }
        public string ProspectFromPQ { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string ProspectFromQuot { get; set; }
        public string ProspectFromRFQ { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string Title { get; set; }
        public string ProspectName { get; set; }
        public string pros_id { get; set; }
        public string curr_id { get; set; }
        public string pros_type { get; set; }
        public string Entity_type { get; set; }
        public string hdnDeleteCommand { get; set; }
        public string Currency { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        
        //public string CityID { get; set; }
       
        //public string DistrictID { get; set; }
       
        //public string StateID { get; set; }
       
        //public string CountryID { get; set; }
        public string GSTNumber { get; set; }
        public string gst_num { get; set; }
        public string GSTMidPrt { get; set; }
        public string GSTLastPrt { get; set; }
        public string proc_status { get; set; }
        public string ddlGroup { get; set; }
        public string DeleteCommand { get; set; }
        public string create_id { get; set; }
        public string creat_dt { get; set; }
        public string mod_id { get; set; }
        public string mod_dt { get; set; }
        public int br { get; set; }
        public string remarks { get; set; }
        public string attatchmentdetail { get; set; }
        //public List<ProspectCity> ProspectCities { get; set; }
        public List<curr> currList { get; set; }
        public List<Branch> BranchList { get; set; }
        public string DocumentMenuId { get; set; }
        public string QuotationDocumentMenuId { get; set; }
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
    
    //public class ProspectCity
    //{
    //    public string city_id { get; set; }
    //    public string city_name { get; set; }

    //}
    public class curr
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }

    }
    public class Branch
    {
        public int br_id { get; set; }
        public string br_val { get; set; }
    }
    public class ProspectSetupMODELAttch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
}
