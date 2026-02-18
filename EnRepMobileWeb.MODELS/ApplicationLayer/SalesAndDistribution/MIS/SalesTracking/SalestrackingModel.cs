using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.SalesTracking
{
    public class SalestrackingModel
    {
        public string OldOrderType1 { get; set; }
        public string OrderType1 { get; set; }
        public string Title { get; set; }
        public string OrderNo { get; set; }
        public string CustId { get; set; }
        public string SlsPersId { get; set; }
        public string CurrId { get; set; }
        public string OrderType { get; set; }
        public string ItemId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchStatus { get; set; }
        public string TrackingOrderType { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }
        public string SearchValue { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
        public List<CustomerModel> CustomersList { get; set; }
        public List<CurrencyList> Currencylist { get; set; }
        public List<OrderNumberListModel> PoNumberList { get; set; }
        public List<SalesPersList> SalesPersons { get; set; }
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
        public List<CustCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public List<CustPortFolioList> portFolioLists { get; set; }
        public string portFolio { get; set; }
        public string hdnbr_ids { get; set; }
    }
    public class ItemsModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
    }
    public class CustomerModel
    {
        public string CustId { get; set; }
        public string CustName { get; set; }
    }
    public class CurrencyList
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }
    }
    public class OrderNumberListModel
    {
        public string OrderNumber { get; set; }
        public string po_Value { get; set; }
    }
    public class SalesPersList
    {
        public string sls_pers_id { get; set; }
        public string sls_pers_name { get; set; }
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
    public class SOTrackingDataModel
    {
        public Int64 SrNo { get; set; }
        public string app_so_no { get; set; }
        public string so_dt { get; set; }
        public string cust_name { get; set; }
        public string OrderType { get; set; }
        public string curr_logo { get; set; }
        public string sls_pers_name { get; set; }
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string uom_alias { get; set; }
        public string ord_qty_base { get; set; }
        public string Pending_qty { get; set; }
        public string ForceClosed { get; set; }
        public string sch_date { get; set; }
        public int overduedays { get; set; }
        public string ForceClose { get; set; }
        public string pack_no { get; set; }
        public string pack_dt { get; set; }
        public string pack_qty { get; set; }
        public string ship_no { get; set; }
        public string ship_dt { get; set; }
        public string ship_qty { get; set; }
        public string app_inv_no { get; set; }
        public string inv_dt { get; set; }
        public string invQty { get; set; }
        public string srt_no { get; set; }
        public string srt_dt { get; set; }
        public string srt_qty { get; set; }
        public string ref_doc_no { get; set; }
        public string cust_catg { get; set; }
        public string cust_port { get; set; }
        public string cust_group { get; set; }
        public string cust_zone { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public int br_id { get; set; }
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
