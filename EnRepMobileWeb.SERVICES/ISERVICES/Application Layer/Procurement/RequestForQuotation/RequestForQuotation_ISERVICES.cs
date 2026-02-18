using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.RequestForQuotation;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.RequestForQuotation
{
    public interface RequestForQuotation_ISERVICES
    {
        DataSet GetSuppList(string Comp_ID,string branchID,string SearchName, string SuppPros_type);
        DataSet GetSuppDetails(string Comp_ID,string  branchID,string  Supp_id, string SuppPros_type);
        DataSet getdetailsRFQ(string CompId, string BranchId, string rfq_no,string UserID, string DocumentMenuId);
        string RFQApprove(RequestForQuotation_Model _RFQModel, string CompID, string br_id, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocID);
        String RFQCancel(RequestForQuotation_Model _RFQModel, string CompID, string br_id, string mac_id);
        DataSet getDetailBySourceDocumentNo(string CompID, string BrchID, string SourDocumentNo);
        DataSet getPRDocumentNo(string CompID, string BrchID, string DocumentNumber);
        string RFQDelete(RequestForQuotation_Model _RFQModel, string CompID, string br_id,string RFQ_NO);
        DataSet CheckPQAgainstRFQ(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetStatusList(string MenuID);
        DataSet GetRequestForQuotationDeatils(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetRFQDetailList(string rfq_no, string CompId, string BrchID, string Fromdate, string Todate, string Status,string UserID,string wfstatus,string DocumentMenuId);
        string InsertUpdateRFQ(DataTable RFQHeader, DataTable RFQItemDetails, DataTable RFQSuppDetails ,DataTable RFQDeleveryShedDetail,DataTable RFQTermAndConDetail,DataTable dtSubItem, DataTable Attachments);
      DataSet GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet GetRFQTrackingDetail(string CompId, string BrID, string RFQ_no, string RFQ_dt);
    }
}
