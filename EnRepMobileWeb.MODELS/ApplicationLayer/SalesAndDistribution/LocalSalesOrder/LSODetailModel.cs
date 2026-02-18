using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesAndDistributionModels
{
    public class LSODetailModelattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class UrlModel
    {
        public string BtnName { get; set; }
        public string FC { get; set; }
        public string Command { get; set; }
        public string SO_SalePerson { get; set; }
        public string SO_Number { get; set; }
        public string SO_Date { get; set; }
        public string TransType { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
        public string DocumentMenuId { get; set; }
        public string CustType { get; set; }
        public string DocumentStatus { get; set; }
        public string Amend { get; set; }
    }
    public class RequirementAreaList
    {
        public int req_id { get; set; }
        public string req_val { get; set; }
    }
    public class LSODetailModel
    {
        public List<RequirementAreaList> _requirementAreaLists { get; set; }
        public string req_area { get; set; }
        public string hdnAutoPR { get; set; }
        public Boolean AutoPR { get; set; }
        public string CancelledRemarks { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string CustType { get; set; }
        public string Guid { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
        public string SO_Date { get; set; }
        public string FinStDt { get; set; }
        public string SO_Number { get; set; }
        public string ILSearch { get; set; }
        public string MenuDocumentId { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string PR_NO { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string ListFilterData { get; set; }
        public string ListFilterData1 { get; set; }
        public string Title { get; set; }
        //public string cust_id { get; set; }
        //public static string ValueRequired { get; set; }
        public string DocumentMenuId { get; set; }
        public string Delete { get; set; }
        public string FC { get; set; }
        public string TransType { get; set; }
        public string SO_no { get; set; }
        public string SO_dt { get; set; }
        public string SO_OrderType { get; set; }
        public string ApplyTax { get; set; }
        public string SO_OrderNo { get; set; }

        //[Required(ErrorMessage = "Value Required")]
        public string SO_OrderDate { get; set; }
        public string SO_SourceType { get; set; }
        public string SO_SourceTypeID { get; set; }
        //[Required(ErrorMessage = "Value Required")]
        public string SO_SourceDocNo { get; set; }

        //[Required(ErrorMessage = "Value Required")]
        public string SO_SourceDocDate { get; set; }

        //[Required(ErrorMessage = "Value Required")]
        public string SO_CustName { get; set; }
        public string cust_alias { get; set; }
        public string SO_CustID { get; set; }
        public string SpanCustPricePolicy { get; set; }
        public string SpanCustPriceGroup { get; set; }
        //[Required(ErrorMessage = "Value Required")]
        public string SO_BillingAddress { get; set; }
        public string SO_Bill_Add_Id { get; set; }
        public string SO_ShippingAddress { get; set; }
        public string SO_Shipp_Add_Id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }

        //[Required(ErrorMessage = "Value Required")]
        public string SO_Currency { get; set; }
        public int SO_CurrencyID { get; set; }

        //[Required(ErrorMessage = "Value Required")]
        public String SO_ExRate { get; set; }

        public String SO_AvlCreditLimit { get; set; }

        //[Required(ErrorMessage = "Value Required")]
        public string SO_SalePerson { get; set; }
        public string SO_SalePerson1 { get; set; }
        public string SalesprsnName { get; set; }
        public String SO_Country { get; set; }
        public string SO_RefDocNo { get; set; }
        public string SO_PortOfDest { get; set; }
        public string SO_ExportFileNo { get; set; }
        public string SO_Remarks { get; set; }
        public string trade_term { get; set; }
        public String SO_GrossValue { get; set; }
        public String OrderDiscountInPercentage { get; set; }
        public String OrderDiscountInAmount { get; set; }
        public String SO_AssessValue { get; set; }
        public String SO_TaxValue { get; set; }
        public String SO_DiscountValue { get; set; }
        public String SO_OtherCharge { get; set; }
        public String SO_NetOrderValue_InSep { get; set; }
        public String SO_NetOrderValue_InBase { get; set; }

        public string SOOrderStatus { get; set; }

        public string attatchmentdetail { get; set; }
        public Boolean SOApprovedFlag { get; set; }
        public Boolean SOForceClosed { get; set; }
        public Boolean SOCancelled { get; set; }
        public string SOTransType { get; set; }

        public int SO_Created_ID { get; set; }
        public int SO_App_ID { get; set; }
        public int SO_Mod_ID { get; set; }
        public string SO_MAC_ID { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CompId { get; set; }
        public string MAC_Address { get; set; }
        public string SystemDetail { get; set; }
        public int BrchID { get; set; }
        public string SO_ItemName { get; set; }

        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string StatusName { get; set; }
        public string WFBarStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string WFStatus { get; set; }
        public string Itemdetails { get; set; }
        public string ItemTaxdetails { get; set; }
        public string ItemOCTaxdetails { get; set; }
        public string ItemOCdetails { get; set; }
        public string ItemDelSchdetails { get; set; }
        public string ItemTermsdetails { get; set; }
        //public string UserID { get; set; }
        public string ForAmmendendBtn { get; set; }
        public string Amendment { get; set; }
        public string Amend { get; set; }
        public string wfDisableAmnd { get; set; }
        /// <summary>
        /// All Dropdown Lists
        public List<CustName> CustNameList { get; set; }
        public List<trade_termList> TradeTermsList { get; set; }
        public List<SO_CountryList> _CountryLists { get; set; }
        public List<Currency> CurrencyList { get; set; }
        public List<SourceDocNo> SourceDocList { get; set; }
        public List<SalePerson> SalePersonList { get; set; }
        public List<QuotationList> quotationsList { get; set; }
        /// </summary>

        /// <summary>
        /// For Delivery Schedule Section
        public string SO_ItemDetails { get; set; }
        /// </summary>
         
        /// <summary>
        /// For Delivery Schedule Section
        public string SO_DeliverySchDetails { get; set; }
        /// </summary>

        /// <summary>
        /// For Tax Calcultion 
        public string SO_TaxDetails { get; set; }
        /// </summary>

        /// <summary>
        /// For Other Charge Section 
        public string SO_OtherChargeDetails { get; set; }
        /// </summary>

        /// <summary>
        /// For Terms And Condition Sction
        public string SO_TermsDetails { get; set; }
        /// </summary>

        /// <summary>
        /// for Attachment Files
        public HttpPostedFile ImageFile { get; set; }
        /// </summary>

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string Status { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReplicateWith { get; set; }
        public string item { get; set; }
        public string bcurrflag { get; set; }
        /*-------------For PopUp Print-------------------- */
        
        public string ShowTotalQty { get; set; } = "N";
        public string PrintItemImage { get; set; } = "N";
        public string ShowProdDesc { get; set; } = "Y";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string CustomerAliasName { get; set; } = "N"; 
        public string ItemAliasName { get; set; } = "N"; 
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string ShowCustomerAliasName { get; set; } = "N";
        public string ShowDeliverySchedule { get; set; } = "N"; 
        public string ShowCustProductCode { get; set; } = "N";

    }
    public class CurrentDetail
    {
        public string CurrentUser { get; set; }
        public string CurrentDT { get; set; }
    }
    public class trade_termList
    {
        public string TrdTrms_id { get; set; }
        public string TrdTrms_val { get; set; }
    }
    public class SO_CountryList
    {
        public string Cntry_id { get; set; }
        public string Cntry_val { get; set; }
    }
    public class CustName
    {
        public string Cust_name { get; set; }

        public string Cust_id { get; set; }
    }
    public class Currency
    {
        public string curr_id { get; set; }
        public string curr_val { get; set; }

    }
    public class SourceDocNo
    {
        public string Doc_id { get; set; }
        public string Dic_val { get; set; }

    }
    public class SalePerson
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

    }
    public class QuotationList
    {
        public string SrcDocNo { get; set; }
        public string SrcDocNoVal { get; set; }

    }


    public class HeaderSODetail
    {
        public string TransType { get; set; }
        public string CompID { get; set; }
        public string BranchID { get; set; }
        public string order_type { get; set; }
        public string so_no { get; set; }
        public string so_date { get; set; }
        public string src_type { get; set; }
        //public string qt_number { get; set; }
        //public string qt_date { get; set; }
        public string CustName { get; set; }
        public string Currency { get; set; }
        public string conv_rate { get; set; }
        public string SalePerson { get; set; }
        public string RefDocNo { get; set; }
        public string remarks { get; set; }
        public string GrVal { get; set; }
        //public string AssVal { get; set; }
        public string DiscAmt { get; set; }
        public string TaxAmt { get; set; }
        public string OcAmt { get; set; }
        public string NetValBs { get; set; }
        public string NetValSpec { get; set; }
        public string ForceClosed { get; set; }
        public string Cancelled { get; set; }
        public string UserID { get; set; }
        public string OrdStatus { get; set; }
        public string MacID { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string ExFileNo { get; set; }
        public string DestCountry { get; set; }
        public string TradeTerms { get; set; }
        public string DestPort { get; set; }

    }
    public class SOItemDetail
    {
     
        public string SONo { get; set; }
        public string SODate { get; set; }
        public string QuotationNumber { get; set; }
        public string QuotationDate { get; set; }
        public string ItemID { get; set; }
        public string UOMID { get; set; }
        public string OrderQty { get; set; }
        public string OrderBQty { get; set; }
        public string InvQty { get; set; }
        public string ItmRate { get; set; }
        public string ItmDisPer { get; set; }
        public string ItmDisAmt { get; set; }
        public string DisVal { get; set; }
        public string GrossVal { get; set; }
        public string AssVal { get; set; }
        public string TaxAmt { get; set; }
        public string OCAmt { get; set; }
        public string NetValSpec { get; set; }
        public string NetValBase { get; set; }
        public string FClosed { get; set; }
        public string Remarks { get; set; }
        public string OrderType { get; set; }
        
    }
    public class SOTaxDetail
    {
        //public string CompID { get; set; }
        //public string Branch { get; set; }
        public string OrderType { get; set; }
        public string SONo { get; set; }
        public string SODate { get; set; }
        public string QuotationNumber { get; set; }
        public string QuotationDate { get; set; }
        public string TaxItmCode { get; set; }
        public string TaxNameID { get; set; }
        public string TaxPercentage { get; set; }
        public string TaxAmount { get; set; }
        public string TaxLevel { get; set; }
        public string TaxApplyOnID { get; set; }
    }
    public class SOOCDetail
    {
        //public string CompID { get; set; }
        //public string Branch { get; set; }
        public string SONo { get; set; }
        public string SODate { get; set; }
        public string OCID { get; set; }
        public string OCValue { get; set; }
        public string OrderType { get; set; }
    }
    public class SODeliveryDetail
    {
        //public string CompID { get; set; }
        //public string Branch { get; set; }
        public string OrderType { get; set; }
        public string SONo { get; set; }
        public string SODate { get; set; }
        public string ItemID { get; set; }
        public string DeliverySchDate { get; set; }
        public string DeliverySchQty { get; set; }
        public string DeliverySchWhouse { get; set; }
        
    }
    public class SOTermsDeatil
    {
        //public string CompID { get; set; }
        //public string Branch { get; set; }
        public string SONo { get; set; }
        public string SODate { get; set; }
        public string TermsDescription { get; set; }
        public string OrderType { get; set; }
    }
}
