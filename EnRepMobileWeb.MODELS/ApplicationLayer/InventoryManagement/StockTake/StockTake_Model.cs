using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockTake
{
   public class StockTake_Model
    {
        public string ImportExcelFileFlag { get; set; }
        public string hdnsaveApprovebtn { get; set; }
       
        public string Title { get; set; }
        public string DocumentNo { get; set; }
        public string ItemName { get; set; }
        public string ItemID { get; set; }
        public string GroupName { get; set; }
        public string GroupID { get; set; }
        public string WarehouseName { get; set; }
        public int wh { get; set; }
        public string MenuDocumentId { get; set; }
        public string CreatedBy { get; set; }
        public string create_id { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string Status { get; set; }
        public string stktake_status { get; set; }
        public string ListFilterData1 { get; set; }
        public string stktake_no { get; set; }
        public DateTime stktake_dt { get; set; }  
        public string DocumentId { get; set; }
        public Boolean CancelFlag { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string STKSearch { get; set; }
        public string AppStatus { get; set; }
        public string StockTakeNo { get; set; }
        public string StockTakeDate { get; set; }
        public string DNSearch { get; set; }
        public List<ItemName> ItemNameList { get; set; }
        public List<ItemGroupName> ItemGroupNameList { get; set; }
        public List<WarehouseName> WarehouseNameList { get; set; }
        public string StkTakeItemdetails { get; set; }
        public string StkTakeItemBatchSerialDetail { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string remarks { get; set; }
        public string CreateBy { get; set; }
        public string DeleteCommand { get; set; }
        public string WF_status1 { get; set; }
        public string SubItemDetailsDt { get; set; }
        /*------------For Popup--------*/
        public string WHNamePopup { get; set; }
        public int PopUpWh_Id { get; set; }
        public int PopUpWh_Name { get; set; }
        public List<WarehouseNamePopUp> WarehouseNameListForPopup { get; set; }
    }
    public class URLDetailModel
    {
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string TransType { get; set; }
        public string StockTakeNo { get; set; }
        //public string DocDate { get; set; }
        public string DocumentMenuId { get; set; }
        public string StockTakeDate { get; set; }
        public string WF_status1 { get; set; }
    }
    public class BindItemListPopup
    {
        public string SearchName { get; set; }
        public int Wh_Id { get; set; }
    }
    public class ItemName
    {
        public string Item_Id { get; set; }
        public string Item_Name { get; set; }
    }
    public class ItemGroupName
    {
        public string Group_Id { get; set; }
        public string Group_Name { get; set; }
    }
    public class WarehouseName
    {
        public string Wh_Id { get; set; }
        public string Wh_Name { get; set; }
    }
    public class WarehouseNamePopUp
    {
        public string Wh_Id { get; set; }
        public string Wh_Name { get; set; }
    }

    public class ItemDetails
    {
        public string item_id { get; set; }
        public string ord_qty { get; set; }
        public string base_uom_id { get; set; }
        public string ord_qty_base { get; set; }
        public string SourceDocumentNo { get; set; }
        public string SourceDocumentDate { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public string OrderQty { get; set; }
        public string BilledQty { get; set; }
        public string RecievedQty { get; set; }
        public string AcceptedQty { get; set; }
        public string RejectedQty { get; set; }
        public string ReworkableQty { get; set; }
        public string it_remarks { get; set; }
    }
}
