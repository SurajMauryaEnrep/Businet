using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.OrderDetail
{
   public class OrderDetail_Model
    {
        public string Title { get; set; }
        public string ItemID { get; set; }
        public string Sales_by { get; set; }
        public string cust_id { get; set; }
        public string RegionName { get; set; }
        public string sales_type { get; set; }
        public string Sales_per { get; set; }
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string Inv_no { get; set; }
        public string Inv_dt { get; set; }
        public string Group { get; set; }
        public List<RegionList> regionLists { get; set; }
        public string Region { get; set; }
        public List<CustCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public List<CustPortFolioList> portFolioLists { get; set; }
        public List<CurrencyList> Currencylist { get; set; }
        public List<ItemList> ItemsList { get; set; }
        public List<SoNumberList> SoNumberList { get; set; }
        public string portFolio { get; set; }
        public string soNo { get; set; }
        public string soDt { get; set; }
        public string ODFilter { get; set; }
        public string ItemDetCurr_Id_CSV { get; set; }
        public string OrdDetCust_Id_CSV { get; set; }
        public string GetPW_Product_Id_CSV { get; set; }
        public string GetPGW_Product_Id_CSV { get; set; }
        public string GetPGW_curr_id_CSV { get; set; }
        public string SalePersonCust_Id_CSV { get; set; }
        public List<CustZoneList> custzoneList { get; set; }
        public string custzone { get; set; }
        public List<CustGroupList> custgroupList { get; set; }
        public string custgroup { get; set; }
        public List<CityList> CityLists { get; set; }
        public List<StateList> StateLists { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string Hidstate { get; set; }
        public string Hidcity { get; set; }
        public string SearchCity { get; set; }
        public string SearchState { get; set; }
        public string state_id { get; set; }

    }
    public class ItemList
    {
        public string itemId { get; set; }
        public string itemName { get; set; }
    }
    public class RegionList
    {
        public string region_id { get; set; }
        public string region_val { get; set; }

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
    public class CurrencyList
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }
    }
    public class SoNumberList
    {
        public string so_no { get; set; }
        public string so_dt { get; set; }
    }
    public class CustZoneList
    {
        public string custzone_id { get; set; }
        public string custzone_val { get; set; }
    }
    public class CustGroupList
    {
        public string CustGrp_id { get; set; }
        public string CustGrp_val { get; set; }
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
}
