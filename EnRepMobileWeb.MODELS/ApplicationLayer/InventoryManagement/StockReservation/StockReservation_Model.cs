using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockReservation
{
    public class StockReservation_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public int uom_id { get; set; }
        public string uom_Name { get; set; }
        public string item_id { get; set; }
        public string item_Name { get; set; }
        public string to_br { get; set; }
        public string to_brid { get; set; }
        public string warehouse_id { get; set; }
        public string DocumentMenuId { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public string doc_no { get; set; }
        public string doc_dt { get; set; }
        public string Doc_Number { get; set; }
        public string Doc_Date { get; set; }
        public string hdn_i_serial { get; set; }
        public string hdn_i_batch { get; set; }
        public string Transaction_Type { get; set; }
        public float Tot_stk { get; set; }
        public float Aval_stk { get; set; }
        public float Res_stk { get; set; }
        public string Doc_Type { get; set; }
        public float doc_qty { get; set; }
        public float pending_qty { get; set; }
        public float unres_qty { get; set; }
        public string lot_id { get; set; }
        public string batch_id { get; set; }
        public string serial_id { get; set; }
        public float BtSrAvalStk { get; set; }
        public float ResQty { get; set; }
        public string MenuDocumentId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ItemName { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string CNFSearch { get; set; }
        public string AppStatus { get; set; }
        public string DocumentStatus { get; set; }
        public List<ItemNameList> ItemList { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<DocumentNumber> DocumentNumberList { get; set; }
        //public List<ListCustomerName> CustomerNameList { get; set; }
        public List<Lot> LotList { get; set; }
        public List<Batch> BatchList { get; set; }
        public List<Serial> SerialList { get; set; }
        public string Res_UnresStockDetails { get; set; }
        public string TotalRes_UnresQty { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string UOMName { get; set; }
        public string CustNam { get; set; }
        public string warehouse_Name { get; set; }
    }
    public class UrlModel
    {
        public string BtnName { get; set; }
        public string Docid { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
    }
    public class Warehouse
    {
        public int wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class Lot
    {
        public string Lot_id { get; set; }
        public string Lot_name { get; set; }
    }
    public class Batch
    {
        public string Batch_id { get; set; }
        public string Batch_name { get; set; }
    }
    public class Serial
    {
        public string Serial_id { get; set; }
        public string Serial_name { get; set; }
    }
    public class ItemNameList
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
    }
    public class CustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }
    public class DocumentNumber
    {
        public string doc_no { get; set; }
        public string doc_dt { get; set; }
    }
   
}
