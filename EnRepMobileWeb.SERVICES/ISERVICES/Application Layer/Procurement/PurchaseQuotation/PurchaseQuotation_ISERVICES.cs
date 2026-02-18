using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.Purchase_Quotation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.Purchase_Quotation
{
    public interface PurchaseQuotation_ISERVICES
    {
        string InsertPQTransactionDetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail, DataTable DTOCDetail,DataTable DtblDeliSchDetail,DataTable DtblTermsDetail,DataTable dtSubItem, DataTable Attachments);
        DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId, string Br_ID,string SuppPros_type);
        DataSet GetSuppRfqList(string Supp_id, string CompId, string Br_ID, string SuppPros_type);
        DataSet GetPRList(string CompId, string Br_ID);
        DataSet AddRfqOrPRItemDetailForQtsn(string CompID, string BrchID, string doc_no, string Doc_date,string Flag);
        string InsertPQApproveDetails(string PQNo, string PQDate, string CompID, string BrchID, string DocumentMenuId, string UserID, string mac_id, string A_Status, string A_Level, string A_Remarks,string Flag);
        string DeletePQDetails(string CompID,string BrchID,string doc_no,string doc_date);
        DataSet GetPurchaseQuotationDetails(string Doc_no,string Doc_date, string CompID, string BrchID,string UserID);
        DataSet GetListOfPQDetails(string comp_id ,string br_id,string UserID, string wfstatus,string DocumentMenuId);
        DataTable GetSuppNameList(string CompId, string br_id, string SupplierName);
        DataSet GetAllData(string CompId, string br_id, string SupplierName, string UserID, string wfstatus, string DocumentMenuId, string SuppID, 
            string PQ_FromDate, string PQ_ToDate, string PQ_status);
        DataTable GetProsSuppNameList(string CompId, string br_id, string SupplierName,string SuppPros_type);
        DataSet GetSearchListOfPQDetails(string CompID,string BrchID, string SupplierID, string Fromdate, string Todate, string Status);
        DataSet CheckPOAgainstPQ(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet GetPurchaseQuotationDeatils(string Comp_ID, string Br_ID, string PQ_No, string PQ_Date);
    }
}
