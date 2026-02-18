using System;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GSTAPI
{
    public interface GSTAPI_IService
    {
        DataSet GetGSTAPIPostingDetails(string compId, string brId, string fromDate, string toDate, string dataType, string docStatus, string GSTR_DateOption,string GstCat);
        DataTable GetSalesItemDetails(string compId, string brId, string dataType, string invNo, string invDt);
        DataSet GetSalesInvoiceData(string compId, string brId, string invNo, string invDt, string invType);
        DataSet GetServiceSalesInvoiceData(string compId, string brId, string invNo, string invDt);
        int SaveApiRequestLogs(string reqBody, string apiResponse, DateTime reqTimeStamp, DateTime respTimeStamp, string status, string compId, string brId,
            string docno, string docDate, string errorMsg);
        int UpdateGstInvApiDetails(string action, string compId, string brId, string gstInvno, string invDt, string ackNo, string ackDt, string irnNo, string status, string invType, string signedQrCode);
        int UpdateGstInvAttachmentDetails(string invTbl, string compId, string brId, string gstInvno, string invDt, string docName, string docPath, string docType);
        string CheckGstInvAttachmentDetails(string invTbl, string compId, string brId, string gstInvno, string invDt, string docName, string docPath, string docType);
        DataTable GetApiErrorDetails(string compId, string brId, string invNo);
        DataTable GetApiDocsDetails(string InvTbl, string compId, string brId, string invNo, string invDt, string docName);
        DataTable GetApiClientDetails(string compId, string brId);
        DataTable GetSaleRegisterDetails(string action, string compId, string brId, string fromDate, string toDate,string GSTR_DateOption);
        DataTable GetFinMonthYearForGSTR(string compId, string brId);
        DataTable GetExcelFileNameForEMPReconsile(string gstr1Or2string, string compId, string brId, string fromDate);
    }
}
