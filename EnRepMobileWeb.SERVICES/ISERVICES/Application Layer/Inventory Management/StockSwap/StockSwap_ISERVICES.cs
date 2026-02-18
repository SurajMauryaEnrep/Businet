using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.Stock_Swap;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.Stock_Swap
{
    public interface StockSwap_ISERVICES
    {
        DataTable GetProductNameLists(string CompId, string br_id,string itemId);
        DataTable GetWarehouseNameLists(string CompId, string br_id,string wh_id);
        DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string SwapNumber, string SwapDate, string ItemID);
        DataSet getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string SwapNumber, string SwapDate, string ItemID);
        string InsertUpdateMaterialIssue(StockSwap_Model _StockSwap_Model,string CompID,string BrchID,string userid,string mac_id,string DocumentMenuId, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable dtSubItem);
        DataSet Edit_SwapStockDetail(string CompId, string BrID, string SwpNumber, string SwpDate, string UserID, string DocID);
        string ApproveSwapStockDetails(string compID, string brId, string swp_no,string swp_dt, string a_Status, string a_Level, string a_Remarks, string userID, string mac_id, string documentMenuId,string SwapType);
        string DeleteSwapStockDetails(string compID, string brId, string swp_no, string swp_dt);
        DataSet GetSwapStockList(string compID, string brId, string src_Prod_id, string dest_prod_id, string status, string DmenuId, string wfStatus, string Userid,string FromDate,string ToDate);
        DataSet GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag,string Type);
        DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string item_id, string flag,string swp_no,string swp_dt);

        DataSet GetDataitemList(string CompID, string BrchID, string ItmName, string Itemid);
    }
}
