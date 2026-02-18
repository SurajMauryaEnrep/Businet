using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.SalesDetail
{
    public class SalesDetail_Model
    {
        public List<HSNno> HSNList { get; set; }
        public string ddlhsncode { get; set; }
        public string HSN_code { get; set; }
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
        public string SDFilter { get; set; }
        public List<RegionList> regionLists { get; set; }
        public string Region { get; set; }
        public List<CustCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public List<CustPortFolioList> portFolioLists { get; set; }
        public string portFolio { get; set; }
        public string Inv_no_Csv { get; set; }
        public string inv_dt_Csv { get; set; }
        public string curr_id_Csv { get; set; }
        public string Cust_Id_Csv { get; set; }
        public string Product_Id_Csv { get; set; }
        public string sale_type_Csv { get; set; }
        public string productgrp_Csv { get; set; }
        public string productPort_Csv { get; set; }
        public string Region_Id_Csv { get; set; }
        public string CustRegion_Csv { get; set; }
        public string InvRegion_Csv { get; set; }
        public string sale_per_id_Csv { get; set; }
        public string SE_Item_Currency_Csv { get; set; }
        public string PGWP_Product_ID_Csv { get; set; }
        public string itmInvoiceNumber_Csv { get; set; }
        public string itmInvoiceDate_Csv { get; set; }
        public string DataShowCsvWise { get; set; }
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
        public string hdnbr_ids { get; set; }
    }
    public class HSNno
    {
        public string setup_val { get; set; }
        public string setup_id { get; set; }
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
