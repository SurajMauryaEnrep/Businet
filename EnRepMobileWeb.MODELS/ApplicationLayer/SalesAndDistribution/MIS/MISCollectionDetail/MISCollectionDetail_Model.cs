using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.MISCollectionDetail
{
    public class MISCollectionDetail_Model
    {
        public string Title { get; set; }
        public string hdnbr_ids { get; set; }
        public string To_dt { get; set; }
        public string cust_id { get; set; }
        public string HiddenCustId { get; set; }
        public string ReceivableType { get; set; }
        public string ReportType { get; set; }
        public string Hidcategory { get; set; }
        public string HidportFolioLists { get; set; }
        public string HidRegionName { get; set; }
        public string HidcustomerZone { get; set; }
        public string HidCustomerGroup { get; set; }
        public string Hidcity { get; set; }
        public string Hidstate { get; set; }
        public List<CustCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public List<CustPortFolioList> portFolioLists { get; set; }
        public string portFolio { get; set; }
        public string RegionName { get; set; }
        public List<RegionList> regionLists { get; set; }
        public List<customerZoneList> customerZoneLists { get; set; }
        public List<CustomerGroupList> CustomerGroupLists { get; set; }
        public List<CityList> CityLists { get; set; }
        public List<StateList> StateLists { get; set; }
        public List<SalesPersList> SalesPersons { get; set; }
        public string searchValue { get; set; }
        public string hdnCSVPrint { get; set; }
        public string Message { get; set; }
        public string customerZone { get; set; }
        public string CustomerGroup { get; set; }
        public string city { get; set; }
        public string SlsPersId { get; set; }
        public string state { get; set; }
        public string AccRcvablPDFData { get; set; }
        public string hdnPDFPrint { get; set; }
        public string SearchCity { get; set; }
        public string state_id { get; set; }
        public string SearchState { get; set; }
        public string ColAging { get; set; }
    }
    public class SalesPersList
    {
        public string sls_pers_id { get; set; }
        public string sls_pers_name { get; set; }
    }
    public class StateList
    {
        public string state_id { get; set; }
        public string state_name { get; set; }

    }
    public class CityList
    {
        public string city_id { get; set; }
        public string city_name { get; set; }

    }
    public class CustomerGroupList
    {
        public string cust_grp_id { get; set; }
        public string cust_grp_name { get; set; }

    }
    public class customerZoneList
    {
        public string cust_zone_id { get; set; }
        public string cust_zone_name { get; set; }

    }
    public class CustCategoryList
    {
        public string Cat_id { get; set; }
        public string Cat_val { get; set; }

    }
    public class CustPortFolioList
    {
        public string CatPort_id { get; set; }
        public string CatPort_val { get; set; }

    }
    public class RegionList
    {
        public string region_id { get; set; }
        public string region_val { get; set; }

    }
}
