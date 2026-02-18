using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServicePurchaseInvoice
{
   public class ServicePIModel
    {
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string duplicateBillNo { get; set; } = "N";
        public string EInvoive { get; set; }
        public string FilterData1 { get; set; }
        public string FilterData { get; set; }
        public string DocumentMenuId { get; set; }
        public string Title { get; set; }
        //public string documentStatus { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public string supp_acc_id { get; set; }
        public string Nurration { get; set; }
        public string BP_Nurration { get; set; }
        public string DN_Nurration { get; set; }
        public string hdnbilldt { get; set; }
        public string hdnbillno { get; set; }
        public Boolean Cancelled { get; set; }
        public string VoucherNarr { get; set; }
        public string Sinv_no { get; set; }
        public string Sinv_dt { get; set; }
        public string bill_no { get; set; }
        public string bill_date { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<CurrancyList> currancyLists { get; set; }
        public int curr_id { get; set; }
        public string Currency { get; set; }
        public string ExRate { get; set; }
        public string Delete { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string TransType { get; set; }
        public string SourceType { get; set; }
        public string Remarks { get; set; }       
        public string UserID { get; set; }
        public string GrVal { get; set; }
        public string AssessableVal { get; set; }
        public string DiscAmt { get; set; }
        public string TaxAmt { get; set; }
        public string OcAmt { get; set; }
        public string NetValBs { get; set; }
        public string NetValSpec { get; set; }
        public string SPIStatus { get; set; }
        public string Address { get; set; }
        public string bill_add_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }

        public string Hd_GstType { get; set; }
        public string Hd_GstCat { get; set; }
        public string SrcDocNo { get; set; }
        public string SrcDocDate { get; set; }
        public List<SourceDoc> SourceDocList { get; set; }
        public string SystemDetail { get; set; }
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string StatusName { get; set; }

        public string NetAmountInBase { get; set; }
        public string NetAmount { get; set; }
        public string OtherCharges { get; set; }
        public string DocSuppOtherCharges { get; set; }//Other charge for Document Supplier.
        public string TDS_Amount { get; set; }
        public string tds_details { get; set; }
        public string oc_tds_details { get; set; }//Added by Suraj on 19-06-2024 for Third part TDS
        public string TaxAmount { get; set; }
        public string GrossValue { get; set; }

        public string GLVoucherType { get; set; }
        public string GLVoucherNo { get; set; }
        public string GLVoucherDt { get; set; }

        public string Itemdetails { get; set; }
        public string ItemTaxdetails { get; set; }
        public string OC_TaxDetail { get; set; }       
        public string ItemOCdetails { get; set; }
        public string vouDetail { get; set; }
        public string ItemTermsdetails { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string attatchmentdetail { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string ListFilterData1 { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
        public string SPISearch { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string CC_DetailList { get; set; }
        public string pmflagval { get; set; }
        public Boolean RoundOffFlag { get; set; }
        public Boolean RCMApplicable { get; set; }
    }
    public class GL_Detail
    {
        public string comp_id { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string doctype { get; set; }
        public float Value { get; set; }
        public string DrAmt { get; set; }
        public string CrAmt { get; set; }
        public string TransType { get; set; }
        public string gl_type { get; set; }

    }
    public class ServicePIModelattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class SourceDoc
    {
        public string doc_no { get; set; }
        public string doc_dt { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class CurrancyList
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }
    }

    public class ServicePIListModel
    {
        public string Title { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public string SPI_FromDate { get; set; }
        public string SPI_ToDate { get; set; }       
        public string Status { get; set; }
        public string FromDate { get; set; }
        //public DateTime ToDate { get; set; }
        public string ToDate { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string attatchmentdetail { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<ServicePurchaseInvoiceList> SPIList { get; set; }
        public string ListFilterData { get; set; }
        public string WF_status { get; set; }
        public string SPISearch { get; set; }
    }
    public class URLModelDetails
    {
        public string Sinv_no { get; set; }
        public string Sinv_dt { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class CurrentDetail
    {
        public string CurrentUser { get; set; }
        public string CurrentDT { get; set; }
    }
    public class ServicePurchaseInvoiceList
    {
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string InvDt { get; set; }
        public string InvType { get; set; }
        public string SourceDocNo { get; set; }
        public string SuppName { get; set; }
        public string Currency { get; set; }
        public string InvValue { get; set; }
        public string InvStauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
        public string bill_dt { get; set; }
        public string bill_no { get; set; }
        public string einvoice { get; set; }
    }
}
