using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.QuotationAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.QuotationAnalysis
{
    public interface QuotationAnalysis_ISERVICES
    {
        string InsertQTATransactionDetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable Attachments);
        Dictionary<string, string> GetRFQList(string CompID, string BranchID, string status);
        DataSet GetRFQListJS(string CompID, string BranchID, string status);
        DataSet GetRFQDetail(string CompId, string BrID, string invno);
        DataSet GetPQADetailDAL(string CompId, string BrID, string Inv_no, string Inv_dt, string UserID, string DocID);
        DataSet GetAllData(string CompID, string BranchID, string User_ID, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        string InsertPQApproveDetails(string PQNo, string PQDate, string CompID, string BrchID, string DocumentMenuId, string UserID, string mac_id, string A_Status, string A_Level, string A_Remarks, string Flag, string doc_no);
        string DeletePQDetails(string CompID, string BrchID, string doc_no, string doc_date);
        DataSet CheckPQADetail(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetQuotationAnalysisDetailForPrint(string CompID, string BrchID, string SI_No, string SI_Date);
        DataSet GetPOItemDetailDAL(string CompId, string BrID, string PONO, string POdt);
        DataSet GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
    }
}
