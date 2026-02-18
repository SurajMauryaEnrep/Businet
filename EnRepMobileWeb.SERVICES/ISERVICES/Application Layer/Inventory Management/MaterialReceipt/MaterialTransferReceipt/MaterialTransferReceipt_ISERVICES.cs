using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.MaterialTransferReceipt;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.MaterialTransferReceipt
{
    public interface MaterialTransferReceipt_ISERVICES
    {
        //DataTable GetWhList(string CompId, string BrchID);
        //DataTable GetToBranchList(string CompId);
        DataSet GetAllDDLListAndListPageData(string CompId, string BrchID,string flag, string PageName, /*string From_br, string From_wh, string Fromdate, string Todate, string Status,*/ string UserID, string wfstatus, string DocumentMenuId);
        DataTable GetToWhList(string CompId, string Tobranch);
        DataSet getMTRNOList(string CompID, string BrchID, string MTRNo, string SourceBR, string SourceWH, string TransferType, string TOWH, string TOBR);
        DataSet GetMaterialTransferItemDetail(string CompID, string BrchID, string TRFDate, string TRFNo, string TRFType,  string MINo, string MIDate, string frombranch);
        DataSet getMINOList(string CompID, string BrchID, string SourceBR, string SourceWH, string MTRNo, string MTRDate);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID, string frombranch);
        string InsertUpdateMaterialTransferReceipt(DataTable MaterialTransferReceiptHeader, DataTable MaterialTransferReceiptItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable Attachments);
        DataSet MaterialTransferReceiptCancel(MaterialTransferReceiptModel _MTRModel, string CompID, string br_id, string mac_id, string MTI_date);
        DataSet GetTMRDetailByNo(string CompID, string BrchID, string TMR_no, string TMR_date, string UserID, string DocumentMenuId);
        String Approve_TMR(MaterialTransferReceiptModel _MTRModel,string Branch, string CompID,string wf_status, string wf_level, string wf_remarks, string mac_id, string DocumentMenuId);
        DataSet MTRDelete(MaterialTransferReceiptModel _MTRModel, string CompID, string br_id,string MaterialReceiptNo);
        DataSet MTRcpt_GetSubItemDetails(string CompID, string BrchID, string Item_id, string Doc_no, string Doc_dt, string Flag,string frombranch);
        DataSet GetMTRDeatilsForPrint(string CompID, string BrchID, string MTRNo, string MTRDate);
    }
    public interface TMRList_ISERVICES
    {
        DataSet GetTMRDetailList(string CompId, string BrchID, string From_br, string From_wh, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetStatusList(string MenuID);
    }
}
