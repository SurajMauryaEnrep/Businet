using System.Collections.Generic;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MRS;
namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialRequisitionSlip
{
    public interface  MaterialRequisitionSlip_ISERVICE
    {
        Dictionary<string, string> EntityList(string CompID, string Entity, string BrchID, string sr_type);
        DataSet GetAvlbStockForItem(string Item_id, string CompId,string BrchID);
        DataTable GetRequirmentreaList(string CompId, string br_id, string flag);
        DataSet MRS_GetAllDDLListAndListPageData(string CompID, string BrchID, string Entity, string WF_status, string UserID, string DocumentMenuId
            , string startDate, string CurrentDate); 
        DataSet GetItemList(string CompId, string BranchId, string ItmName);
        DataSet GetMRStmListDAL(string CompID, string ItmName);
        DataSet GetIssueToList(string CompId, string IssueTo, string BranchId);

        string InsertUpdateMRS(DataTable MRSHeader, DataTable MRSItemDetails, DataTable MRSAttachments, DataTable dtSubItem);
        string MRSApprove(MRSModel _MRSModel, string CompID, string br_id, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocumentMenuId);
        DataSet MRSDelete(MRSModel _MRSModel, string CompID, string br_id,string MRS);

        string MRSCancel(MRSModel _MRSModel, string CompID, string br_id, string mac_id);
        string MRSForceClose(MRSModel _MRSModel, string CompID, string br_id,string mac_id);
        DataSet CheckInternalIssue(string CompID, string br_id, string MRS_no, string MRS_date);
        string getNextDocumentNumber(string CompID, string BrchID, string MenuDocumentId, string Prefix);
        DataSet GetMRSDetail(string CompID, string mrs_no, string BrchID, string UserID, string DocumentMenuId,string language);
        DataSet GetMRSDeatilsForPrint(string CompID, string BrchID, string mrs_no, string mrs_dt);
        DataSet MRS_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet GetSourceDocList(string Comp_ID, string Br_ID, string RequiredArea, string Req_type);
        DataSet GetoutputList(string Comp_ID, string Br_ID, string docno, string docdt );
        DataSet getProductionOrderdata(string CompID, string BrchID,  string srcdocno, string srcdocdt,string RequiredArea);
        DataSet GetSubItemDetailsFromPrductnOrd(string CompID, string BrchID, string srcdocno, string srcdocdt, string RequiredArea, string ItemId);
        DataSet GetSOTrackingDetail(string CompId, string BrID, string MRS_No, string MRS_Date);
        DataSet getReplicateWith(string comp_id, string br_id, string mrs_type,string req_area, string SarchValue);
        DataSet GetReplicateWithItemdata(string comp_id, string br_id, string mrs_no, string mrs_dt);
    }
    public interface MRSList_ISERVICES
    {
        DataSet GetMRSDetailList(string CompId, string BrchID,int reqArea,string Issueto,string EntityType,  
            string Fromdate, string Todate, string MRS_Type, string SRC_Type, string Status,string UserID, string wfstatus, string DocumentMenuId,string language, string ItemID);
        DataSet GetStatusList(string MenuID);
    }

}
