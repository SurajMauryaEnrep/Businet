using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSaleInvoice
{
   public class DomesticSaleInvoice_Model
    {
		public string PvtMark { get; set; }
		public string CancelledRemarks { get; set; }
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
		public string hdnsaveApprovebtn { get; set; }
		public string Title { get; set; }
		public string DocumentMenuId { get; set; }
		
		public string SI_no { get; set; }
		public string SI_dt { get; set; }
		public string Customer_type { get; set; }
		public string CustID { get; set; }
		public string cust_id { get; set; }
		public string cust_name { get; set; }
		public string slprsn_id { get; set; }
		public string slprsn_name { get; set; }
		public string ship_no { get; set; }
		public string ShipNum { get; set; }
		public string ship_dt { get; set; }
		public string SI_BillingAddress { get; set; }
		public string SI_Bill_Add_Id { get; set; }
		public string SI_ShippingAddress { get; set; }
		public string SI_Shipp_Add_Id { get; set; }
		public string Ship_Gst_number { get; set; }
		public string Ship_StateCode { get; set; }
		public string CustName { get; set; }
        public string CustType { get; set; }
		public string SalePerson { get; set; }
		public string ItemName { get; set; }
        public string CustPage { get; set; }
        public string BrchID { get; set; }
		public string OrderType { get; set; }
		public List<curr> currList { get; set; }
		public List<CustomerName> CustomerNameList { get; set; }
		public List<BankName> BankNameList { get; set; }
		public List<SalesPersonName> SalesPersonNameList { get; set; }
		public string TransType { get; set; }
		public string MenuID { get; set; }
		public Boolean CancelFlag { get; set; }
		public string Cancelled { get; set; }
		public string comp_id { get; set; }
		public string br_id { get; set; }
		public string inv_type { get; set; }
		public string inv_no { get; set; }
		public string inv_dt { get; set; }
		public string supp_id { get; set; }
		public string bill_no { get; set; }
		public string bill_dt { get; set; }
		public string curr_id { get; set; }
		public string bs_curr_id { get; set; }
		public string curr { get; set; }
		public string conv_rate { get; set; }
		public string sale_per { get; set; }
		public string user_id { get; set; }
		public string inv_status { get; set; }
		public string mac_id { get; set; }
		public string gr_val { get; set; }
		public string tax_amt { get; set; }
		public string oc_amt { get; set; }
		public string net_val { get; set; }
		public string net_val_bs { get; set; }
		public string Narration { get; set; }
		public string tcs_details { get; set; }
		public string tcs_amt { get; set; }
		public string oc_tds_details { get; set; }//Added by Hina sharma on 09-07-2024 for Third party TDS
		public string SI_ItemDetail { get; set; }
		public string ItemTaxdetails { get; set; }
		public string ItemOCTaxdetails { get; set; }
		public string ItemOCdetails { get; set; }
		public string TblItemTaxdetails { get; set; }
		public string TblItemOCdetails { get; set; }
		public string VouGlDetails { get; set; }
		public string GrVal { get; set; }
		public string OrderDiscountInPercentage { get; set; }
		public string OrderDiscountInAmount { get; set; }
		public string DiscAmt { get; set; }
		public string TaxAmt { get; set; }
		public string OcAmt { get; set; }
		public string DocSuppOtherCharges { get; set; }
		public Boolean RoundOffFlag { get; set; }
		
		public string pmflagval { get; set; }
		public string FRoundOffAmt { get; set; }
		public string NetValBs { get; set; }
		public string NetValSpec { get; set; }
		public string AllGlDetails { get; set; }
		public string GLVoucherType { get; set; }
		public string GLVoucherNo { get; set; }
		public string GLVoucherDt { get; set; }
		public string WFBarStatus { get; set; }
		public string SubItemDetailsDt { get; set; }
		public string WFStatus { get; set; }
		public string Create_id { get; set; }
		public string Status { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedOn { get; set; }
		public string ApprovedBy { get; set; }
		public string ApprovedOn { get; set; }
		public string AmmendedBy { get; set; }
		public string AmmendedOn { get; set; }
		public string DeleteCommand { get; set; }
		public string A_Status { get; set; }
		public string A_Level { get; set; }
		public string A_Remarks { get; set; }
		public string SaleVouMsg { get; set; }
		public string attatchmentdetail { get; set; }
		public string TaxCalci_ItemName { get; set; }
		public string TaxCalci_ItemID { get; set; }
		
		public string TaxCalci_AssessableValue { get; set; }
		public string TaxCalci_Tax_Template { get; set; }
		public string TaxCalci_Tax_Type { get; set; }
		public string TaxCalci_TaxName { get; set; }
		public string TaxCalci_TaxNameID { get; set; }
		public string TaxCalci_Tax_Percentage { get; set; }
		public string TaxCalci_Level { get; set; }
		
		public string TaxCalci_ApplyOn { get; set; }
		public string TaxCalci_Tax_Amount { get; set; }
		public List<TaxCalciTaxName> TaxCalciTaxNameList { get; set; }
		public string HdnPaymentSchedule { get; set; }
		public string OcCalci_OtherCharge { get; set; }
		public string OcCalci_OCCurrency { get; set; }
		public string OcCalci_OCconv_rate { get; set; }
		public string OcCalci_OCAmount { get; set; }
		public string OcCalci_OCAmtInBs { get; set; }
		public string ListFilterData1 { get; set; }
		public string Message { get; set; }
		public string Command { get; set; }
		public string DocumentStatus { get; set; }
		public string BtnName { get; set; }
		public string WF_status { get; set; }
        public string SI_Number { get; set; }
        public string SI_Date { get; set; }
        public string InvType { get; set; }
        public string MenuDocumentId { get; set; }
        public string AppStatus { get; set; }
        public string Guid { get; set; }
        public string WF_status1 { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public List<OcCalciOtherCharge> OcCalciOtherChargeList { get; set; }
		public List<ShipNumberList> shipNumbers { get; set; }
        public string CC_DetailList { get; set; }
        public string benif_name { get; set; }
        public string bank_name { get; set; }
        public string bank_address { get; set; }
        public string bank_add { get; set; }
        public string acc_num { get; set; }
        public string acc_no { get; set; }
        public string shift_cd { get; set; }
        public string swift_code { get; set; }
        public string ifsc_cd { get; set; }
        public string ifsc_code { get; set; }
        public string usd_corr_bank { get; set; }
        public string pre_carr_by { get; set; }
        public string trade_term { get; set; }
		public List<trade_termList> TradeTermsList { get; set; }// Added by Suraj on 20-10-2023 for trade term add in SI
		public string pi_rcpt_carr { get; set; }
        public string ves_fli_no { get; set; }
        public string loading_port { get; set; }
        public string discharge_port { get; set; }
        public string fin_disti { get; set; }
        public string container_no { get; set; }
        public string other_ref { get; set; }
        public string term_del_pay { get; set; }
        public string des_good { get; set; }
        public string prof_detail { get; set; }
        public string declar { get; set; }
        public string BuyerIfOtherThenConsignee { get; set; }
        public string CountryOfOriginOfGoods { get; set; }
        public string CountryOfFinalDestination { get; set; }
        public string ExportersReference { get; set; }
        public string ConsigneeAddress { get; set; }
        public string InvoiceHeading { get; set; }
        public string BuyersOrderNumberAndDate { get; set; }
        public string ExporterAddress { get; set; }
        public string IRNNumber { get; set; }
        public string custom_inv_no { get; set; }
        public string CustomInvDate { get; set; }
		public string HdnPrintOptons { get; set; }
		public string PrtOpt_catlog_number { get; set; }
		public string PrtOpt_item_code { get; set; }
		public string PrtOpt_item_desc { get; set; }
		public string GstApplicable { get; set; }
		public string bcurrflag { get; set; }
		public string ConsigneeName { get; set; }
		public List<PortOfLoadingListModel> PortOfLoadingList { get; set; }
		public List<pi_rcpt_carrListModel> pi_rcpt_carrList { get; set; }
        public string Custome_Reference { get; set; }
		public string ShipTo { get; set; }
		public string ShipFromAddress { get; set; }
		public string Payment_term { get; set; }
        public string Delivery_term { get; set; }
		public string ShowProdDesc { get; set; } = "N";
		public string ShowProdTechDesc { get; set; } = "N";
		public string ShowSubItem { get; set; } = "N"; 
		public string PrintShipFromAddress { get; set; } = "N";
		public string CustomerAliasName { get; set; } = "N";
		public string ItemAliasName { get; set; } = "N";
		public string showInvHeading { get; set; } = "N";
		public string PrintCorpAddr { get; set; } = "Y";
		public string PrintRemarks { get; set; } = "Y";
		public string showDeclare2 { get; set; } = "N";
		public string ShowWithoutSybbol { get; set; } = "Y";
		public string showDeclare1 { get; set; } = "N";
		public string ShowCustSpecProdDesc { get; set; } = "N";
		public string PrintFormat { get; set; } = "F1"; 
		public int NumberofCopy { get; set; } 
		public bool rev_charge { get; set; }
        public string cust_acc_id { get; set; }
		public string PV_Nurration { get; set; }
		public string BP_Nurration { get; set; }
		public string CN_Nurration { get; set; }
		public string DN_Narration { get; set; }
		public string DN_Narration_Tcs { get; set; }		
		public string Hd_GstType { get; set; }
		public string Hd_GstCat { get; set; }
		public string Invoice_Heading { get; set; }
		public string Invoice_remarks { get; set; }
		public string Corporate_Address { get; set; }
		public string Declaration_1 { get; set; }
		public string Declaration_2 { get; set; }
		public bool nontaxable { get; set; }
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
	public class pi_rcpt_carrListModel
	{
		public string Pi_id { get; set; }
		public string Pi_Name { get; set; }
	}
	public class PrintOptionsList
	{
		public bool PrtOpt_catlog_number { get; set; }
		public bool PrtOpt_item_code { get; set; }
		public bool PrtOpt_item_desc { get; set; }
	}
	public class trade_termList
	{
		public string TrdTrms_id { get; set; }
		public string TrdTrms_val { get; set; }
	}
	public class SaleInvoiceModelattch
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
	}
	public class PortOfLoadingListModel
	{
		public string POL_id { get; set; }
		public string POL_Name { get; set; }
		//public string pin_number { get; set; }
		public string State_Name { get; set; }
	}
	public class UrlModel
	{
		public string BtnName { get; set; }
		public string Command { get; set; }
		public string InvType { get; set; }
		public string SI_Number { get; set; }
		public string SI_Date { get; set; }
		public string TransType { get; set; }
		public string FRoundOffAmt { get; set; }
		
		public string WF_status1 { get; set; }
		public string AppStatus { get; set; }
		public string DocumentMenuId { get; set; }
		public string CustType { get; set; }
		public string DocumentStatus { get; set; }
	}
	public class ShipNumberList
	{
		public string Ship_number { get; set; }
		//public string Ship_date { get; set; }
	}
	public class TaxCalciTaxName
	{
		public string tax_id { get; set; }
		public string tax_name { get; set; }
	}
	public class OcCalciOtherCharge
	{
		public string oc_id { get; set; }
		public string oc_name { get; set; }
	}
	public class CustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }
	public class BankName
	{
        public string acc_id { get; set; }
        public string Acc_Name { get; set; }
    }
	public class SalesPersonName
	{
		public string slprsn_id { get; set; }
		public string slprsn_name { get; set; }
	}
	public class curr
	{
		public string curr_id { get; set; }
		public string curr_name { get; set; }

	}
	public class SI_ListModel
	{
		public string ListFilterData { get; set; }
		public string Title { get; set; }
		public string CustName { get; set; }
		public string CustomerId { get; set; }
		public string CustID { get; set; }
		public string SI_no { get; set; }
		public string SI_dt { get; set; }
		public string SI_FromDate { get; set; }
		public string SI_ToDate { get; set; }
		public string OrderType { get; set; }
		public string Status { get; set; }
		public string FromDate { get; set; }
		public string LSISearch { get; set; }
		public string DocumentMenuId { get; set; }
		public string CustType { get; set; }
		public string WF_status { get; set; }

		public DateTime ToDate { get; set; }
		public string SQ_SalePerson { get; set; }
		public List<SalePersonList> SalePersonList { get; set; }
		public List<CustomerName> CustomerNameList { get; set; }
		public List<Status> StatusList { get; set; }
		public List<SalesInvoiceList> SIList { get; set; }
		
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
	public class SalesInvoiceList
	{
		public string InvoiceNo { get; set; }
		public string InvoiceDate { get; set; }
		public string SalesPerson { get; set; }
		public string InvDate { get; set; }
		public string InvoiceType { get; set; }
		public string ship_no { get; set; }
		public string ship_dt { get; set; }
		public string custom_inv_dt { get; set; }
		public string custom_inv_no { get; set; }
		public string CustName { get; set; }
		public string Currency { get; set; }
		public string InvoiceValue { get; set; }
		public string Stauts { get; set; }
		public string CreateDate { get; set; }
		public string ApproveDate { get; set; }
		public string ModifyDate { get; set; }
		public string create_by { get; set; }
		public string app_by { get; set; }
		public string mod_by { get; set; }
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
		public string Transtype { get; set; }
		public string gl_type { get; set; }

	}
}
