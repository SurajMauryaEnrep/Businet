using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.SampleReceipt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.SampleReceipt
{
   public interface SampleReceipt_ISERVICE
    {
        DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType,string source_type);
        //Dictionary<string, string> getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType);
        DataSet getSampleRcptDetail(string CompID, string BrchID, string SampleRcptNumber, string SampleRcptDate);
        DataSet GetWarehouseList(string CompId, string BrID);
        //DataSet SampleRcptDelete(SampleReceiptModel _SampleReceiptModel, string CompID, string br_id);
        //string SampleRcpt_InsertAndUpdate(DataTable SampleRcptHeader, DataTable SampleRcptItemDetails, DataTable SampleRcptAttachments, DataTable SampleRcptItemBatchDetails, DataTable SampleRcptItemSerialDetails);
        //DataSet SampleRcpt_Cancel(SampleReceiptModel _SampleReceiptModel, string CompID, string br_id, string mac_id);
        //DataSet SampleRcpt_Approve(SampleReceiptModel _SampleReceiptModel, string CompID, string br_id, string mac_id);
        DataSet GetSampleRcptItmList(string CompID, string BrID, string ItmName);
        DataSet GetItemUOMDAL(string Item_id, string CompId);
        string Delete_SR_Detail(SampleReceiptModel _SRModel, string CompId, string BrID);
        string Approve_SampleReceipt(SampleReceiptModel _SRModel, string CompID, string br_id, string SR_Date, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocID);
        //string Approve_SampleReceipt(string SR_No, string SR_Date, string MenuID, string Branch, string CompID, string ApproveID, string mac_id, string wf_status, string wf_level, string wf_remarks);
        string InsertUpdateSR_Details(DataTable DTHeaderDetail, DataTable DTItemDetail,/* DataTable dtSubItem,*/ DataTable DtblAttchDetail);
        DataSet GetStatusList(string MenuID);
        DataSet GetSRDetailList(string CompId, string BrchID, string User_ID, string EntityType, string EntityName, string Fromdate, string Todate, string Status, string Docid, string wfstatus);
        //DataSet Edit_SampleReceiptDetail(string CompId, string BrID, string SRNo, string SRDate,string UserID, string DocID);
        DataSet DblClickgetdetailsSR(string CompId, string BrchID, string SRNo, string SRDate, string Userid, string DocumentMenuId);
      
        DataSet GetSourceDocList(string Comp_ID, string Br_ID, string Itm_ID, string SuppID,string entity_type,string sr_number);
        string Check_SampleReceiptItemStock(string CompID, string Branch, string SR_No, string SR_Date);
        DataSet GetAttatchDetailEdit(string CompID, string BrchID, string SR_No, string SR_Date);
        DataSet SR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag,string src_docdate,string src_doc_no,string Status);
        DataSet getMIdata(string CompID,string BrchID, string sample_name, string srcdocno, string srcdocdt,string entity_type,string entityname,string SR_Number);
        DataSet GetItemList(string CompID, string BrID);//, string src_type, string EntityName, string EntityType);
        DataSet CheckRFQAgainstPR(string CompId, string BrchID, string DocNo, string DocDate);
    }
}
