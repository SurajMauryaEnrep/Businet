using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.PurchaseRequisition;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.PurchaseRequisition
{
    public interface PurchaseRequisition_ISERVICES
    {
        string PRDelete(PurchaseRequisition_Model _PRModel, string CompID, string br_id, string DocumentMenuId);
        DataTable GetRequirmentreaList(string CompId, string br_id);
        DataSet GetAllData(string CompId, string br_id, string pr_no, int reqArea, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetPRDetailList(string pr_no,string CompId, string BrchID, int reqArea, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetPOTrackingDetail(string CompId, string BrID, string PRNo, string PRDate);
        DataSet GetStatusList(string MenuID);
        string PRListApprove(PurchaseRequisition_Model _PRModel, string CompID, string br_id, string PR_Date,string SrcType, string wf_status, string wf_level, string wf_remarks,  string mac_id, string DocID);
        DataSet getdetailsPR(string CompId, string BranchId, string pr_no, string pr_dt, string UserID, string DocumentMenuId);
        string InsertUpdatePR(DataTable PRHeader, DataTable PRItemDetails ,DataTable dtSubItem,DataTable Attachments);
        String PRCancel(PurchaseRequisition_Model _PRModel, string CompID, string br_id, string mac_id);
        String PRForceClose(PurchaseRequisition_Model _PRModel, string CompID, string br_id, string mac_id);
        DataSet CheckRFQAgainstPR(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet PR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet GetPurchaseRequisitionDeatils(string Comp_ID, string Br_ID, string PR_No, string PR_Date);

        DataSet GetDataReorderLevel(string CompID, string BrchID);
    }
}
