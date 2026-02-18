using System;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ServiceSaleInvoice
{
   
    public class ServiceSIModel
    {
        public List<SrcDocNo> SrcdocnoList { get; set; }
        public string HdnPaymentSchedule { get; set; }
        public string SourceDocNo { get; set; }
        public string SourceDocDate { get; set; }
        public string SO_SourceType { get; set; }
        public string CancelledRemarks { get; set; }
        public string GstApplicable { get; set; }
        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string PrintFormat { get; set; } = "F1"; 
        public string PrintRemarks { get; set; } = "N";/*Add by Hina on 25-09-2024*/
        public string ShowWithoutSybbol { get; set; } = "Y"; 
        public string showDeclare1 { get; set; } = "N";
        public string showDeclare2 { get; set; } = "N";
        public string showInvHeading { get; set; } = "N";
        public string CustomerAliasName { get; set; } = "N";
        public string ShowTotalQty { get; set; } = "N";


        public string PrtOpt_catlog_number { get; set; }
        public string PrtOpt_item_code { get; set; }
        public string PrtOpt_item_desc { get; set; }
        public string HdnPrintOptons { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string FilterData1 { get; set; }
        public string FilterData { get; set; }
        public string DocumentMenuId { get; set; }
        public string Title { get; set; }
        //public string documentStatus { get; set; }
       
        public Boolean Cancelled { get; set; }
        public Boolean nontaxable { get; set; }
        public string Sinv_no { get; set; }
        public string Sinv_dt { get; set; }
        public string bill_no { get; set; }
        public string bill_date { get; set; }
        public string CustName { get; set; }
        public string CustID { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<CurrancyList> currancyLists { get; set; }
        public string curr_id { get; set; }
        public string bs_curr_id { get; set; }
        public string Currency { get; set; }
        public string ExRate { get; set; }
        public string Delete { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string TransType { get; set; }
        public string SourceType { get; set; }
        public string Remarks { get; set; }
        public string txt_PlcOfSupply { get; set; }
        public string UserID { get; set; }
        public string GrVal { get; set; }
        public string AssessableVal { get; set; }
        public string DiscAmt { get; set; }
        public string TaxAmt { get; set; }
        public string OcAmt { get; set; }
        public string NetValBs { get; set; }
        public string NetValSpec { get; set; }
        public string SSIStatus { get; set; }
        public string Address { get; set; }
        public string bill_add_id { get; set; }
        public string Ship_Add_Id { get; set; }
        public string ShippingAddress { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string SaleVouMsg { get; set; }
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

        public string pmflagval { get; set; }
        public Boolean RoundOffFlag { get; set; }
        public string NetAmountInBase { get; set; }
        public string NetAmount { get; set; }
        public string OtherCharges { get; set; }
        public string DocSuppOtherCharges { get; set; }
        public string TDS_Amount { get; set; }
        public string tds_details { get; set; }
        public string oc_tds_details { get; set; }//Added by Hina sharma on 02-07-2024 for Third party TDS
        public string TaxAmount { get; set; }
        public string GrossValue { get; set; }
        public string Narration { get; set; }
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
        public string SSISearch { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string CC_DetailList { get; set; }
        public string IRNNumber { get; set; }
        public string RefDoc_No { get; set; }
        public string RefDoc_Dt { get; set; }
        
        public string TblItemOCdetails { get; set; }
        public string PV_Narration { get; set; }
        public string BP_Narration { get; set; }
        public string CN_Narration { get; set; }
        public string DN_Narration { get; set; }//Added by Hina sharma on 02-07-2024 for Third party TDS
        public string cust_acc_id { get; set; }
        public string slprsn_id { get; set; }
        public string SalePerson { get; set; }
        public string ddlDelivery_term { get; set; }
        public string ShipTo { get; set; }
        public string ddlPayment_term { get; set; }
        public string ddlCustome_Reference { get; set; }
        public string Declaration_1 { get; set; }
        public string Declaration_2 { get; set; }
        public string Invoice_Heading { get; set; }
        public int NumberofCopy { get; set; } = 1;
        public List<SalesPersonName> SalesPersonNameList { get; set; }
    }

    public class SrcDocNo
    {
        public string Src_DocNo { get; set; }
        public string SrcDocNoVal { get; set; }

    }
    public class SalesPersonName
    {
        public string slprsn_id { get; set; }
        public string slprsn_name { get; set; }
    }
    public class PrintOptionsList
    {
        public bool PrtOpt_catlog_number { get; set; }
        public bool PrtOpt_item_code { get; set; }
        public bool PrtOpt_item_desc { get; set; }
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
    public class ServiceSIModelattch
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
    
    public class CurrancyList
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }
    }

    public class ServiceSIListModel
    {
        public string GstApplicable { get; set; }
        public string Title { get; set; }
        
        public string SSI_FromDate { get; set; }
        public string SSI_ToDate { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        //public DateTime ToDate { get; set; }
        public string ToDate { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string attatchmentdetail { get; set; }
        public string CustName { get; set; }
        public string CustID { get; set; }
        public string SQ_SalePerson { get; set; }

        public List<SalePersonList> SalePersonList { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<ServiceSalesInvoiceList> SSIList { get; set; }
        public string ListFilterData { get; set; }
        public string WF_status { get; set; }
        public string SSISearch { get; set; }
    }
    public class URLModelDetails
    {
        public string Sinv_no { get; set; }
        public string Sinv_dt { get; set; }
        public string TransType { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
    }
    public class SalePersonList
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

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
    public class CustomerName
    {
        public string Cust_id { get; set; }
        public string Cust_name { get; set; }
    }
    public class ServiceSalesInvoiceList
    {
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string src_type { get; set; }
        public string src_doc_no { get; set; }
        public string src_doc_dt { get; set; }
        public string SalesPerson { get; set; }
        public string InvDt { get; set; }
        public string InvType { get; set; }
        public string SourceDocNo { get; set; }
        public string CustName { get; set; }
        public string Currency { get; set; }
        public string InvValue { get; set; }
        public string InvStauts { get; set; }
        public string RefDocNo { get; set; }
        public string RefDocDt { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
    }
}

