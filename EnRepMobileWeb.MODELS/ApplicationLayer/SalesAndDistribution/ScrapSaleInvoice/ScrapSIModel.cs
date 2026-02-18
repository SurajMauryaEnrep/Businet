using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using System;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ScrapSaleInvoice
{
    public class ScrapSIModel
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public List<CmnCountryList> countryList { get; set; }
        public List<CmnDistrictList> DistrictList { get; set; }
        public List<CmnCityList> cityLists { get; set; }
        public List<CmnStateList> StateList { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public string cust_trnsport_id { get; set; }
        public string EditCommand { get; set; }
        public string hdnTransType { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string HdnPaymentSchedule { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string FilterData1 { get; set; }
        public string FilterData { get; set; }
        public string DocumentMenuId { get; set; }
        public string Title { get; set; }
        //public string documentStatus { get; set; }
       
        public Boolean Cancelled { get; set; }
        public string Sinv_no { get; set; }
        public string Sinv_dt { get; set; }
        public string bill_no { get; set; }
        public string bill_date { get; set; }
        public string CustName { get; set; }
        public string CustID { get; set; }
        public string cust_acc_id { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<CurrancyList> currancyLists { get; set; }
        public string curr_id { get; set; }
        
        public string bs_curr_id { get; set; }
        public string conv_rate { get; set; }
        
        public string Currency { get; set; }
        public string ExRate { get; set; }
        public string Delete { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string _mdlCommand { get; set; }
        public string TransType { get; set; }
        public string SourceType { get; set; }
        public string Remarks { get; set; }
        public string CustRefNo { get; set; }
        public string CustRefDt { get; set; }
        
        public string CancelledRemarks { get; set; }
        public string PlaceOfSupply { get; set; }
        
        public string UserID { get; set; }
        public string GrVal { get; set; }
        public string AssessableVal { get; set; }
        public string DiscAmt { get; set; }
        public string TaxAmt { get; set; }
        public string OcAmt { get; set; }
        public string DocSuppOtherCharges { get; set; }
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
        public string TDS_Amount { get; set; }
        public string tcs_amt { get; set; }
        public string tcs_details { get; set; }
        public string oc_tds_details { get; set; }
        
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
        public string TblItemOCdetails { get; set; }/*add by Hina on 20-07-2024*/
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
        public string StockItemWiseMessage { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
        public string SSISearch { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string CC_DetailList { get; set; }
        public string IRNNumber { get; set; }
        public string PV_Narration { get; set; }
        public string BP_Narration { get; set; }
        public string CN_Narration { get; set; }
        public string DN_Narration { get; set; }
        public string DN_Narration_Tcs { get; set; }

        /*----------------------Print Detail Start-------------------------------*/
        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string PrintShipFromAddress { get; set; } = "N";
        public string CustomerAliasName { get; set; } = "N";
        public string ShowTotalQty { get; set; } = "Y";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string PrintFormat { get; set; } = "F1";
        public string HdnPrintOptons { get; set; }
        public string PrtOpt_catlog_number { get; set; }
        public string PrtOpt_item_code { get; set; }
        public string PrtOpt_item_desc { get; set; }
        public string GstApplicable { get; set; }
        public int NumberofCopy { get; set; } = 1;
        public string ShowWithoutSybbol { get; set; } = "Y";
        public string PrintCorpAddr { get; set; } = "Y";
        public string PrintRemarks { get; set; } = "Y";
        public string showDeclare1 { get; set; } = "N";
        public string showDeclare2 { get; set; } = "N";
        public string showInvHeading { get; set; } = "N";
        /*------------------------Transport detail start--------------------*/
        public string GR_No { get; set; }
        public string HdnGRNumber { get; set; }
        public string GR_Dt { get; set; }
        public string HdnGRDate { get; set; }
        
        public string No_Of_Packages { get; set; }
        public string hdnNumberOfPacks { get; set; }
        //public string Transpt_Id { get; set; }
        public string Transpt_NameID { get; set; }
        public string HdnTrnasportName { get; set; }
        
        public string Veh_Number { get; set; }
        
        public string Driver_Name { get; set; }
        public string Mob_No { get; set; }
        public string Tot_Tonnage { get; set; }
        public string slprsn_id { get; set; }
        public string SalePerson { get; set; }
        public string ddlPayment_term { get; set; }
        public string ddlDelivery_term { get; set; }
        public string Invoice_Heading { get; set; }
        public string Declaration_1 { get; set; }
        public string Declaration_2 { get; set; }
        public string Corporate_Address { get; set; }
        public string pvt_mark { get; set; }
        public bool nontaxable { get; set; }
        public string ShipFromAddress { get; set; }
        public string ShipTo { get; set; }
        public List<SalesPersonName> SalesPersonNameList { get; set; }
        public List<TransListModel> TransList { get; set; }
    }
    public class CmnCountryList
    {
        public string country_id { get; set; }
        public string country_name { get; set; }

    }
    public class CmnStateList
    {
        public string state_id { get; set; }
        public string state_name { get; set; }

    }
    public class CmnDistrictList
    {
        public string district_id { get; set; }
        public string district_name { get; set; }
    }
    public class CmnCityList
    {
        public string City_Id { get; set; }
        public string City_Name { get; set; }
    }
    public class SalesPersonName
    {
        public string slprsn_id { get; set; }
        public string slprsn_name { get; set; }
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
    public class ScrapSIModelattch
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
    public class PrintOptionsList
    {
        public bool PrtOpt_catlog_number { get; set; }
        public bool PrtOpt_item_code { get; set; }
        public bool PrtOpt_item_desc { get; set; }
    }
    //public class TransListModel
    //{
    //    public string TransId { get; set; }
    //    public string TransName { get; set; }
    //}
    public class ScrapSIListModel
    {
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
        public List<ScrapSaleInvoiceList> SSIList { get; set; }
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
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class SalePersonList
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

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
    public class ScrapSaleInvoiceList
    {
        public string cust_ref_no { get; set; }
        public string InvNo { get; set; }
        public string InvDate { get; set; }
        public string SalesPerson { get; set; }
        public string InvDt { get; set; }
        public string InvType { get; set; }
        public string SourceDocNo { get; set; }
        public string CustName { get; set; }
        public string Currency { get; set; }
        public string InvValue { get; set; }
        public string InvStauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
}

