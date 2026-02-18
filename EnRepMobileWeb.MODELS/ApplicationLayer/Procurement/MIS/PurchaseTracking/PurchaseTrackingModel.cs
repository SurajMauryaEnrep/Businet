using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.MIS.PurchaseTracking
{
    public class PurchaseTrackingModel
    {
        public string OldOrderType1 { get; set; }
        public string OrderType1 { get; set; }
        public string Title { get; set; }
        public string OrderNo { get; set; }
        public string SuppId { get; set; }
        public string CurrId { get; set; }
        public string OrderType { get; set; }
        public string ItemId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchStatus { get; set; }
        public string SearchValue { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
        public List<SupplierModel> SuppliersList { get; set; }
        public List<CurrencyList> Currencylist { get; set; }
        public List<OrderNumberListModel> PoNumberList { get; set; }
        public string Filters { get; set; }
        public List<SuppCategoryList> categoryLists { get; set; }
        public string category { get; set; }
        public List<SuppPortFolioList> portFolioLists { get; set; }
        public string portFolio { get; set; }
        public string hdbr_id { get; set; }
    }
    public class SuppCategoryList
    {
        public string Cat_id { get; set; }
        public string Cat_val { get; set; }

    }
    public class SuppPortFolioList
    {
        public string Port_id { get; set; }
        public string Port_val { get; set; }

    }
    public class ItemsModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
    }
    public class SupplierModel
    {
        public string SuppId { get; set; }
        public string SuppName { get; set; }
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
    public class POTrackingDataModel
    {
        public Int64 SrNo { get; set; }
        public string supp_name { get; set; }
        public string city_name { get; set; }
        public string app_po_no { get; set; }
        public string po_dt { get; set; }
        public string bill_no { get; set; }
        public string bill_dt { get; set; }
        public string curr_logo { get; set; }
        public string item_id { get; set; }
        public string order_type { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
        public string item_type { get; set; }
        public string ForceClose { get; set; }
        public string ord_qty_base { get; set; }
        public string pending_qty { get; set; }
        public string dn_no { get; set; }
        public string dn_dt { get; set; }
        public string dnQty { get; set; }
        public string qc_no { get; set; }
        public string qc_dt { get; set; }
        public string accept_qty { get; set; }
        public string reject_qty { get; set; }
        public string rework_qty { get; set; }
        public string short_qty { get; set; }
        public string sample_qty { get; set; }
        public string mr_no { get; set; }
        public string mr_dt { get; set; }
        public string mr_qty { get; set; }
        public string inv_no { get; set; }
        public string inv_dt { get; set; }
        public string inv_qty { get; set; }
        public string sch_date { get; set; }
        public int overduedays { get; set; }
        public string supp_cat_name { get; set; }
        public string supp_port_name { get; set; }
        public int br_id { get; set; }
        public string prt_no { get; set; }
        public string prt_dt { get; set; }
        public string prt_qty { get; set; }

    }

}
