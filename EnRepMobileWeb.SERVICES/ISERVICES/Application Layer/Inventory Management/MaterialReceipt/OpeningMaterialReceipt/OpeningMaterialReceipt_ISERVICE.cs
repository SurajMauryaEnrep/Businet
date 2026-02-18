using System.Data;
using System.Collections.Generic;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.OpeningMaterialReceipt;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.OpeningMaterialReceipt
{
    public interface OpeningMaterialReceipt_ISERVICE
    {
        DataSet GetOpeningRcptItmList(string CompID, string BrID, string ItmName, string wh_id);       
        DataSet GetItemUOMDAL(string Item_id, string CompId);
        string InsertOP_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTItemBatchDetail, DataTable DTItemSerialDetail,DataTable SubItem,string op_rno);
        DataSet GetStatusList(string MenuID);
        DataSet GetOPRDetailList(string CompId, string BrchID, string Fromdate,string  wfstatus,string UserID,string DocumentMenuId);
        DataSet GetOpeningDate(int CompID, int BrID);
        DataSet Edit_OpeningDetail(string CompId, string BrID, string OPDate, string wh_id, string UserID, string DocID,string id);
        string Delete_OPR_Detail(OpeningMaterialReceiptModel model, string CompId, string BrID);
        string Approve_OpeningMaterialReceipt(string id,string wh_id, string OPRDate, string Branch, string CompID, string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string DocID);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet GetOpeningQtyDetauls(string comp_id, string br_id, string op_dt, string wh_id, string Item_id, string flag);
        DataSet OMR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string wh_id, string OP_dt, string Flag);
        DataSet GetMasterDataForExcelFormat(string CompID, string Br_id);
        DataSet GetVerifiedDataOfExcel(string Warehouse, string BrchID, DataTable ItemDetail, DataTable BatchDetail, DataTable SerialDetail, DataTable SubItemDetail, string CompID);
        DataTable ShowExcelErrorDetail(string Warehouse, string BrchID, DataTable ItemDetail, DataTable BatchDetail, DataTable SerialDetail, DataTable SubItemDetail, string CompID);
        string BulkImportOMRDetail(string compId, string UserID, string brId, string Warehouse, string op_dt, float op_val1, DataTable ItemDetail, DataTable BatchDetail, DataTable SerialDetail, DataTable SubItemDetail);
    }
}
