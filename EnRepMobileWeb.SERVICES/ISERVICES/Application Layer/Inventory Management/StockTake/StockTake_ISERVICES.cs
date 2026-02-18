using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockTake;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.StockTake
{
   public interface StockTake_ISERVICES
    {
        DataSet GetItemList(string CompId, string BranchId, string ItmName,string GroupID);
        //DataSet GetItemListForAddNewPopup(string CompId, string BranchId, /*string ItmName,*/int Wh_Id);
        DataSet GetItemListForAddNewPopup(string CompID, string BrID, string ItmName, string PageName, int Wh_Id);

        Dictionary<string, string> ItemGroupList(string GroupName, string CompID);
        //Dictionary<string, string> GetWarehouseList(string WarehouseName, string CompID, string BrID);
        DataSet GetWarehouseListPopUp(string CompID, string BrID,string WareHouseName);
        Dictionary<string, string> GetWarehouseList(string WarehouseName, string CompID, string BrID);
        DataSet GetStockItemDetail(string CompID, string BrchID, string WHID, string GRPID, string ItemID, string ListItems);
        DataSet GetStockItemLotBatchSerialDetail(string CompID, string BrchID, string ItemID, string WHID, string SrcDocNumber, string RT_Status, string flag,string hdFlagForAddNewStk);
        String InsertStockTakeDetail(DataTable StockTakeHeader, DataTable StockTakeItemDetails
            , DataTable StockTakeLotBatchSerial, DataTable dtSubItem);
        DataSet GetStockTakeDetail(string Stk_no, string Stk_dt, string CompID, string BrchID, string UserID, string DocumentMenuId);
        DataSet GetStatusList(string MenuID);
        DataSet GetStockTakeListAll(string Fromdate, string Todate, string Status, string CompID, string BrchID, string wfstatus, string UserID, string DocumentMenuId);
        DataSet StockTakeDelete(StockTake_Model _StockTake_Model, string CompID, string br_id);
        string StockTakeApprove(string STKNo, string STKDate, string userid, string wf_status, string wf_level, string wf_remarks, string comp_id, string br_id, string mac_id, string DocID);
        DataSet StockTake_GetSubItemDetailsAfterApprove(string compID, string brchID, string item_id, string doc_no, string doc_dt, string flag);
        
    }
}
