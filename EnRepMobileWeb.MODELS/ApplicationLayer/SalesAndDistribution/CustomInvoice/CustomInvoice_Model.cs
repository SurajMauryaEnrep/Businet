using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.CustomInvoice
{
   public class CustomInvoice_Model
    {
		
		public string hdnsaveApprovebtn { get; set; }
		public string Guid { get; set; }
		public string SI_Date { get; set; }
		public string WF_status1 { get; set; }
		public string SI_Number { get; set; }
		public string Message { get; set; }
		public string BtnName { get; set; }
		public string DocumentStatus { get; set; }
		public string Command { get; set; }
		public string AppStatus { get; set; }
		public string ListFilterData1 { get; set; }
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
		public List<SalesPersonName> SalesPersonNameList { get; set; }
		public string TransType { get; set; }
		public string MenuID { get; set; }
		public Boolean CancelFlag { get; set; }
		public string Cancelled { get; set; }
		public string comp_id { get; set; }
		public string br_id { get; set; }
		public string inv_type { get; set; }
		public string inv_no { get; set; }
		public string custom_inv_no { get; set; }
		public string CustomInvDate { get; set; }
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
		public string NarrationOnC { get; set; }
		public string SI_ItemDetail { get; set; }
		public string ItemTaxdetails { get; set; }
		public string ItemOCTaxdetails { get; set; }
		public string ItemOCdetails { get; set; }
		public string TblItemTaxdetails { get; set; }
		public string TblItemOCdetails { get; set; }
		public string VouGlDetails { get; set; }
		public string GrVal { get; set; }
		public string AssVal { get; set; }
		public string RoundOffSpec { get; set; }
		public string AssValSpec { get; set; }
		public string DiscAmt { get; set; }
		public string TaxAmt { get; set; }
		public string OcAmt { get; set; }
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
		public DataTable AttachMentDetailItmStp { get; set; }
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
		public string OcCalci_OtherCharge { get; set; }
		public string OcCalci_OCCurrency { get; set; }
		public string OcCalci_OCconv_rate { get; set; }
		public string OcCalci_OCAmount { get; set; }
		public string OcCalci_OCAmtInBs { get; set; }
		public List<OcCalciOtherCharge> OcCalciOtherChargeList { get; set; }
		// Added by Suraj on 20-10-2023 for Other Details
		public string pre_carr_by { get; set; }
		public string trade_term { get; set; }
		public List<trade_termList> TradeTermsList { get; set; }
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
		public string ConsigneeName { get; set; }
		public Boolean nontaxable { get; set; }
		public List<PortOfLoadingListModel> PortOfLoadingList { get; set; }
		public List<pi_rcpt_carrListModel> pi_rcpt_carrList { get; set; }
	}
	public class pi_rcpt_carrListModel
	{
		public string Pi_id { get; set; }
		public string Pi_Name { get; set; }
	}
	public class PortOfLoadingListModel
	{
		public string POL_id { get; set; }
		public string POL_Name { get; set; }
		//public string pin_number { get; set; }
		public string State_Name { get; set; }
	}
	public class trade_termList
	{
		public string TrdTrms_id { get; set; }
		public string TrdTrms_val { get; set; }
	}
	public class Cust_InvoiceModelattch
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
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
	public class UrlModel
	{
		public string BtnName { get; set; }
		public string Command { get; set; }	
		public string SI_Number { get; set; }
		public string SI_Date { get; set; }
		public string TransType { get; set; }
		public string WF_status1 { get; set; }
		public string AppStatus { get; set; }
		public string DocumentMenuId { get; set; }
		public string DocumentStatus { get; set; }
	}
	public class SI_ListModel
	{
         public string WF_status { get; set; } 
         public string LSISearch { get; set; } 
		public string ListFilterData { get; set; }
		public string Title { get; set; }
		public string CustName { get; set; }
		public string CustID { get; set; }
		public string SI_no { get; set; }
		public string SI_dt { get; set; }
		public string SI_FromDate { get; set; }
		public string SI_ToDate { get; set; }
		public string OrderType { get; set; }
		public string Status { get; set; }
		public string FromDate { get; set; }

		public DateTime ToDate { get; set; }
		public List<CustomerName> CustomerNameList { get; set; }
		public List<Status> StatusList { get; set; }
		public List<SalesInvoiceList> SIList { get; set; }
		
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
		public string custom_inv_dt { get; set; }
		public string custom_inv_no { get; set; }
		public string InvDate { get; set; }
		public string InvoiceType { get; set; }
		public string ship_no { get; set; }
		public string Ship_dt { get; set; }
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
		public string ass_val { get; set; }

	}
	public class GL_Detail
	{
		public string comp_id { get; set; }
		public string id { get; set; }
		public string type { get; set; }
		public string doctype { get; set; }
		public string Value { get; set; }
		public string DrAmt { get; set; }
		public string CrAmt { get; set; }
		public string Transtype { get; set; }
		public string gl_type { get; set; }

	}
}
