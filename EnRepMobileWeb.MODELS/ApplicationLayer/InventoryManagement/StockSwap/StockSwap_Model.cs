using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.Stock_Swap
{
    public class StockSwap_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedId { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string SwapDate { get; set; }
        public string SwapNumber { get; set; }
        public string ProductName { get; set; }
        public int Warehouse { get; set; }
        public string DeleteCommand { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string btncommand { get; set; }
        public string Message { get; set; }
        public string DocumentStatus { get; set; }
        public string CompId { get; set; }
        public string BrchID { get; set; }
        public string Src_Uom { get; set; }
        public string Src_UomID { get; set; }
        public string Dest_Uom { get; set; }
        public string Dest_UomID { get; set; }
        public string SwapQuantity { get; set; }
        public string DestSwapQuantity { get; set; }
        public string ProductName1 { get; set; }
        public int Warehouse1 { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string i_batch { get; set; }
        public string i_serial { get; set; }
        public string src_avl_stk { get; set; }
        public string Dest_avl_stk { get; set; }
        public string WFStatus { get; set; }
        public string WFBarStatus { get; set; }
        public string ListFilterData1 { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Swp_ToDate { get; set; }
        public string ListStatus { get; set; }
        public string WF_status { get; set; }
        public string SrcSubItm { get; set; }
        public string DestSubItm { get; set; }
        public string AvailableQuantitySrc { get; set; }
        public string AvailableQuantityDest { get; set; }
        public string swp_type { get; set; }
        public string SwapType { get; set; }
        public List<Status> StatusList { get; set; }
        public List<ProductNameList> _ProductNameList { get; set; }
        public List<WarehouseList> _WarehouseList { get; set; }
        public List<WarehouseListDest> _WarehouseListDest { get; set; }
    }
    public class UrlData
    {
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string ListFilterData1 { get; set; }
    }
    public class ProductNameList
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
    public class WarehouseList
    {
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }
    public class WarehouseListDest
    {
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
}
