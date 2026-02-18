using System.Collections.Generic;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.GoodsReceiptNote;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.GoodsReceiptNote
{
    public interface GoodsReceiptNote_ISERVICE
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet GetAllData(string CompID, string SuppName, string BranchID, string User_ID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfstatus);
        DataSet GetDeliveryNoteList(string Supp_id, string CompId, string BrID);
        //DataSet getdetailsGRN(string CompId, string BranchId, string mr_no, string mr_dt, string UserID, string DocumentMenuId);
        DataSet GetDeliveryNoteDetail(string DnNo, string CompId, string BrID);
        DataSet GetGRNDetails(string DNNo, string DNDate, string CompId, string BrID);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet GetPIAttatchDetailEdit(string CompID, string BrchID, string GRNNo, string GRNDate);
        string InsertGRN_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTItemBatchDetail, DataTable DTItemSerialDetail, DataTable DtblAttchDetail, DataTable DtblSubItemDetail);
        string InsertGRNCosting_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTItemTaxDetail, DataTable DTItemOCDetail,DataTable DTItemOCTaxDetail);
        string Delete_GRN_Detail(GoodsReceiptNoteModel GoodsReceiptNote, string MrType, string CompId, string BrID);
        string Approve_GRN(string grn_no,string grn_dt, string Mr_Type, string MenuID, string Branch, string CompID, string ApproveID, string mac_id, string wf_status, string wf_level, string wf_remarks);
        DataSet Edit_GRNDetail(string CompId, string BrID, string GRNNo, string GRNDate, string UserID, string DocID,string Type);
        string Check_GRNItemStock(string CompID, string Branch, string MR_No, string MR_Date);
        DataSet GetGRNDeatilsForPrint(string CompID, string Branch, string GRN_No, string GRN_Date);
        DataSet CheckInvoiceAgainstGRN(string CompId, string BrchID, string DocNo, string DocDate);
        //DataSet GetAllGLDetails(DataTable GLDetail);
    
    }
    public interface GoodsReceiptNoteList_ISERVICE
    {
        DataSet GetGRNDetailList(string CompId, string BrchID,string User_ID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfstatus);
        DataSet GetStatusList(string MenuID);
    }
}
