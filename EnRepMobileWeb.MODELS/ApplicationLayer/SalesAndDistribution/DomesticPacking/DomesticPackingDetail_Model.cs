using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticPacking
{
    public class DomesticPackingDetail_Model
    {
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string checkpreameter { get; set; }
        public string WgtDigit { get; set; }
        public string DocumentMenuId { get; set; }
        public string Docid { get; set; }
        public string WF_Status1 { get; set; }
        public string Packing_No { get; set; }
        public string AppStatus { get; set; }
        public string DocumentStatus { get; set; }
        public string MenuDocumentId { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string StockMessage { get; set; }
        public string BtnName { get; set; }
        public string ListFilterData1 { get; set; }
        public string Title { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public bool Disable { get; set; }
        public DataTable PL_SerialiizationDT { get; set; }
        public string DnItemdetails { get; set; }
        public string FilterOrderNumber { get; set; }
        public string filterCustomerName { get; set; }
        public string Customer_type { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string cust_name { get; set; }
        public string pack_type { get; set; }
        public string pack_no { get; set; }
        public DateTime pack_dt { get; set; }
        public string cust_id { get ; set; }
        public string pack_remarks { get; set; }
        public string so_no { get; set; }
        public string so_dt { get; set; }
        public string exp_file_no { get; set; }
        public string curr_id { get; set; }
        public string Ex_rate { get; set; }
        public string pack_status { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<OrderNumber> OrderNumberList { get; set; }
        public List<CurrList> currLists { get; set; }
        public List<ItemDetails> ItemDetailsList { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string mac_id { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string Qty_TransType { get; set; }
        public string Qty_pari_Command { get; set; }
        public string TransType { get; set; }
        public string create_id { get; set; }
        public string create_dt { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_dt { get; set; }
        public string mod_id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string hdnItemorientation { get; set; }
        public string DeleteCommand { get; set; }
        public string Itemorientation { get; set; }
        public Boolean CancelFlag { get; set; }
        public string SO_DATE { get; set; }
        public string  SO_Number { get; set; }
        public string ItemOrderQtyDetail { get; set; }
        public string PackingSrlznDetail { get; set; }
        public string WFBarStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string SubItemResDetailsDt { get; set; }
        public string SubItemPackResDetailsDt { get; set; }
        public string WFStatus { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string OrderReservedItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string ItemPackagingDetail { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public string TotalGrossWgt { get; set; }
        public string TotalNetWgt { get; set; }
        public string TotalCBM { get; set; }
        public string UserID { get; set; }
        public string ForAmmendendBtn { get; set; }
        public string PrintFormat { get; set; } = "F1";
        public string Amend { get; set; }
        public string CmdType { get; set; }
    }
    public class UrlModel
    {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string PAC_No { get; set; }
        public string PAC_Dt { get; set; }
        public string DMS { get; set; }
        public string Docid { get; set; }
        //public string DocumentMenuId { get; set; }
        
        public string Amend { get; set; }
        public string CmdType { get; set; }

    }
    public class CustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }

    public class Dt_FieldsModel
    {
        public List<Dt_Fields> _Dt_Fields { get; set; }
    }
    public class Dt_Fields
    {
        public string Field_name { get; set; }
        public string Field_dataType { get; set; }
    }
    public class OrderNumber
    {
        public string so_no    { get; set; }
        public string so_dt  { get; set; }
    }
    public class CurrList
    {
        public string curr_id    { get; set; }
        public string curr_nm  { get; set; }
    }
    public class ItemDetails
    {
        public string  comp_id { get; set; }
        public string  br_id { get; set; }
        public string  pack_type { get; set; }
        public string  pack_no { get; set; }
        public string  pack_dt { get; set; }
        public string  item_id { get; set; }
        public string  uom_id { get; set; }
        public string  ord_qty { get; set; }
        public string  bal_qty { get; set; }
        public string  pack_nos { get; set; }
        public string  tot_wght { get; set; }
        public string  pack_qty { get; set; }
        public string  ship_qty { get; set; }
        public string  it_remarks { get; set; }

    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
}

