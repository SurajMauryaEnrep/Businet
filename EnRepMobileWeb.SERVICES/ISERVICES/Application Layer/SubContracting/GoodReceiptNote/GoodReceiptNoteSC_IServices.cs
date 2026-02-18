using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.GoodReceiptNote;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.GoodReceiptNote
{
   public interface GoodReceiptNoteSC_IServices
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet GetDeliveryNoteList(string Supp_id, string CompId, string BrID);
        //DataSet getdetailsGRN(string CompId, string BranchId, string mr_no, string mr_dt, string UserID, string DocumentMenuId);
        DataSet GetDeliveryNoteDetail(string DnNo, string CompId, string BrID);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet GetGRNSCDetails(string DNNo, string DNDate, string CompId, string BrID);
        string InsertGRNSC_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTItemBatchDetail, 
            DataTable DTItemSerialDetail, DataTable DtblAttchDetail,DataTable DtblConsumeItemDetail, 
            DataTable DtblConsumeItemBatchDetails, DataTable DtblScrapItemDetail, DataTable DtblScrapItemBatchDetail, DataTable DtblSubItemDetail);
        string InsertGRNCosting_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable CostingDetailConItmConList, DataTable DTItemTaxDetail, DataTable DTItemOCDetail, DataTable DTItemOCTaxDetail);
        DataSet GetGRNSCListandSrchDetail(string CompId, string BrchID, GRNListModel _GRNListModel, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetGRNDetailEditUpdate(string CompId, string BrchID, string GRNSC_NO, string GRNSC_Date, string UserID, string DocID);

        string GRN_DeleteDetail(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel, string CompId, string BrID);
        string GRNApproveDetails(string CompID, string BrchID, string GRN_No, string GRN_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks);
        string GRNCancel(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel, string CompID, string br_id, string mac_id);
        DataSet getOrderResrvItemStockBatchWise(string ItemId, string CompID, string BrchID, string DN_NO);
        
       DataSet getOrderResrvItemStockBatchWiseOnDblClk(string CompID, string BrID, string GRNNo, string GRNDate, string ItemID, string DN_NO);
        DataSet GetSubItemDetailsFromDNSC(string CompID, string br_id, string Item_id, string DNNo, string doc_no, string doc_dt, string Flag,string Status);
        DataSet CheckGRNAgainstInvoic(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetGRNDeatilsForPrint(string CompID, string Branch, string GRN_No, string GRN_Date);
    }
}
