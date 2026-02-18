using System;
using System.Collections.Generic;
using System.Data;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.SupplementaryPurchaseInvoice
{
    public class SuppPurchaseInvoice_Model
    {
		public string SuppName { get; set; }
		public string ItemName { get; set; }
		public string SuppPage { get; set; }
		public string BrchID { get; set; }
		public string Message { get; set; }

		public List<Supplier_Name> SupplierName_List { get; set; }
		public List<Purchase_Invoice> PurchaseInvoice_List { get; set; }
	}
	public class DocumentNumber
	{
		public string grn_no { get; set; }
		public string grn_dt { get; set; }
	}
	public class CurrancyList
	{
		public string curr_id { get; set; }
		public string curr_name { get; set; }
	}
	public class Supplier_Name
	{
		public string supp_id { get; set; }
		public string supp_name { get; set; }
	}	public class SupplierName
	{
		public string supp_id { get; set; }
		public string supp_name { get; set; }
	}
	public class PurchaseInvoice
	{
		public string InvNo { get; set; }
		public string Invdt { get; set; }
	}	
	public class Purchase_Invoice
	{
		public string InvNo { get; set; }
		public string Invdt { get; set; }
	}
	public class HeaderSuppPI_Detail
	{

		public string TransType { get; set; }
		public string MenuID { get; set; }
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
		public string conv_rate { get; set; }
		public string user_id { get; set; }
		public string inv_status { get; set; }
		public string mac_id { get; set; }
		public string gr_val { get; set; }
		public string tax_amt_nrecov { get; set; }
		public string oc_amt { get; set; }
		public string net_val { get; set; }
		public string net_val_bs { get; set; }
		public string Narration { get; set; }


	}
	public class SuppPI_ItemDetail
	{
		public string mr_no { get; set; }
		public string mr_date { get; set; }
		public string item_id { get; set; }
		public string uom_id { get; set; }
		public string mr_qty { get; set; }
		public string item_rate { get; set; }
		public string item_gr_val { get; set; }
		public string item_tax_amt { get; set; }
		public string item_oc_amt { get; set; }
		public string item_net_val_spec { get; set; }
		public string item_net_val_bs { get; set; }
		public string gl_vou_no { get; set; }
		public string gl_vou_dt { get; set; }
	}
	public class SuppPI_TaxDetail
	{
		public string mr_no { get; set; }
		public string mr_date { get; set; }
		public string item_id { get; set; }
		public string tax_id { get; set; }
		public string tax_rate { get; set; }
		public string tax_val { get; set; }
		public string tax_level { get; set; }
		public string tax_apply_on { get; set; }
	}
	public class SuppPI_OCDetail
	{
		public string oc_id { get; set; }
		public string oc_val { get; set; }

	}
	public class SuppPI_GLDetail
	{
		public string sr_no { get; set; }
		public string acc_id { get; set; }
		public string dr_amt { get; set; }
		public string cr_amt { get; set; }
	}

	public class GL_Detail
	{
		public string comp_id { get; set; }
		public string id { get; set; }
		public string type { get; set; }
		public string doctype { get; set; }
		public string Value { get; set; }
		public string ValueInBase { get; set; }
		public string DrAmt { get; set; }
		public string CrAmt { get; set; }
		public string TransType { get; set; }
		public string gl_type { get; set; }
		public string parent { get; set; }
		public string DrAmtInBase { get; set; } = "0";
		public string CrAmtInBase { get; set; } = "0";
		public string curr_id { get; set; }
		public string conv_rate { get; set; }
		public string bill_no { get; set; }
		public string bill_date { get; set; }

	}
	public class SuppPI_AttchmentDetail
	{
		public string[] FileDetail { get; set; }
	}
	public class SuppPI_ListModel
	{
		public string Hdnbilldt { get; set; }
		public string Hdnbillno { get; set; }
		public string HdnsaveApprovebtn { get; set; }
		public string EInvoive { get; set; }
		public string EWBNNumber { get; set; }
		public string FilterData1 { get; set; }
		public string FilterData { get; set; }
		public string DocumentMenuId { get; set; }
		public string Title { get; set; }
		public string SuppPI_SuppName { get; set; }
		public string SuppPI_SuppID { get; set; }
		public string Supp_Acc_Id { get; set; }
		public string Inv_Nos { get; set; }
		public string SuppPI_wfdocid { get; set; }
		public string SuppPI_wfstatus { get; set; }
		public string SuppPI_inv_dt { get; set; }
		public string SuppPI_DeleteCommand { get; set; }
		public string SuppPI_FromDate { get; set; }
		public string SuppPI_ToDate { get; set; }
		public string OrderType { get; set; }
		public string Status { get; set; }
		public string Create_id { get; set; }
		public string FromDate { get; set; }
		public string WFBarStatus { get; set; }
		public string WFStatus { get; set; }
		public string attatchmentdetail { get; set; }
		public DateTime ToDate { get; set; }
		public string CreatedBy { get; set; }
		public string Createdon { get; set; }
		public string ApprovedBy { get; set; }
		public string ApprovedOn { get; set; }
		public string AmendedBy { get; set; }
		public string AmendedOn { get; set; }
		public string Status_name { get; set; }
		public string doc_status { get; set; }
		public Boolean CancelFlag { get; set; }
		//public string supp_id { get; set; }
		public string bill_no { get; set; }
		public string bill_date { get; set; }
		public string GRNNumber { get; set; }
		public string GRNDate { get; set; }
		public string SuppCurrency { get; set; }
		public string ExRate { get; set; }
		public string NetAmountInBase { get; set; }
		public string NetAmountInBasePrev { get; set; }
		public string NetOrderValueSpePrev { get; set; }
		public string NetAmount { get; set; }
		public string OtherCharges { get; set; }
		public string OtherChargesPrev { get; set; }
		public string DocSuppOtherCharges { get; set; }//Other charge for Document Supplier.
		public string DocSuppOtherCharges_self { get; set; }//Other charge for Document Supplier.
		public string DocSuppOtherChargesTP { get; set; }//Other charge for Document Supplier.
		public string DocSuppOtherChargesPrev { get; set; }//Other charge for Document Supplier.
		public string DocSuppOtherChargesPrevTP { get; set; }//Other charge for Document Supplier.
		public string TaxAmountRecoverable { get; set; }
		public string TaxAmountNonRecoverable { get; set; }
		public string TaxAmountRecoverablePrev { get; set; }
		public string TaxAmountNonRecoverablePrev { get; set; }
		public string GrossValue { get; set; }
		public string PurchaseInv_bill_no { get; set; }
		public string PurchaseInv_bill_dt { get; set; }
		public string GrossValuePrev { get; set; }
		public string GrossValueDiff { get; set; }
		public string GrossValueInBase { get; set; }
		public string GrossValueInBasePrev { get; set; }
		public string GrossValueInBaseDiff { get; set; }
		public string PriceBasis { get; set; }
		public string FreightType { get; set; }
		public string ModeOfTransport { get; set; }
		public string Destination { get; set; }
		public string ItemDetails { get; set; }
		public string Narration { get; set; }
		public string TaxDetail { get; set; }
		public string OC_TaxDetail { get; set; }
		public string OCDetail { get; set; }
		public string vouDetail { get; set; }
		public string GLVoucherType { get; set; }
		public string GLVoucherNo { get; set; }
		public string GLVoucherDt { get; set; }
		public string Address { get; set; }
		public string bill_add_id { get; set; }
		public string Ship_Gst_number { get; set; }
		public string Ship_StateCode { get; set; }
		public string Hd_GstType { get; set; }
		public string Hd_GstCat { get; set; }
		public string TransType { get; set; }
		public string DocumentStatus { get; set; }
		public string Command { get; set; }
		public string Message { get; set; }
		public string BtnName { get; set; }
		public string MenuDocumentID2 { get; set; }
		public string LPISearch { get; set; }
		public string Inv_No { get; set; }
		public string Inv_Dt1 { get; set; }
		public string AppStatus { get; set; }
		public DataTable AttachMentDetailItmStp { get; set; }
		public string Guid { get; set; }
		public string WF_Status1 { get; set; }
		public string WF_Status { get; set; }
		public string CC_DetailList { get; set; }
		public string remarks { get; set; }
		public List<CurrancyList> currancyLists { get; set; }
		public List<DocumentNumber> GRNNumberList { get; set; }
		public List<SupplierName> SupplierNameList { get; set; }
		public List<PurchaseInvoice> PurchaseInvoiceList { get; set; }
		public List<Status> StatusList { get; set; }
		public List<SuppPurchaseInvoiceList> PIList { get; set; }
		public string ItemDetailsDt { get; set; }//For sub-item
		public string dt_SubItemDetails { get; set; }//For sub-item
		public Boolean RoundOffFlag { get; set; }
		//public string PMflagval { get; set; }
		public string Nurration { get; set; }
		public string BP_Nurration { get; set; }
		public string DN_Nurration { get; set; }//for Debit Note nurration
		public string CN_Nurration { get; set; }//for Debit Note nurration
		public string DN_VarNurration { get; set; }//for Debit Note nurration
		public string CN_VarNurration { get; set; }//for Debit Note nurration
		public string Tds_details { get; set; }//Added by Suraj on 01-07-2024 for TDS to Main Supplier
		public string Oc_tds_details { get; set; }//for TDS to Third Party Supplier
		public string TDS_Amount { get; set; }//for TDS to main Supplier
		public string TDS_AmountPrev { get; set; }//for TDS to main Supplier
		public Boolean RCMApplicable { get; set; }
		public string ValDigit { get; set; }
		public string QtyDigit { get; set; }
		public string RateDigit { get; set; }
		public string ExchDigit { get; set; }
		public string Purchaseinv_no { get; set; }
		public string ListFilterData { get; set; }
		public string TxtDiff_Amount { get; set; }
		public string TxtDiff_TotalAmountInSpec { get; set; }
		public string TxtDiff_TotalAmountInBase { get; set; }
		public string TxtDiff_TotalTaxAmountRec { get; set; }
		public string TxtDiff_TotalTaxAmountNonRec { get; set; }
		public string TxtDiff_TotalOCAmount { get; set; }
		public string TxtDiff_TotalOCTPAmount { get; set; }
		public string TxtDiff_TotalTDSAmount { get; set; }
		public string OCFor { get; set; }
		public string pmflagval { get; set; }
		public string curr_id { get; set; }
		public string conv_rate { get; set; }
		public string BatchCommand { get; set; }
		public string SubItemDetails { get; set; }
		public string SuppTdsApplicable { get; set; }
		public string UserID { get; set; }

	}
	public class SuppPurchaseInvoiceattch
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
		//public string AttachMentDetailItmStp { get; set; }
	}

	public class UrlData
	{
		public string DocumentStatus { get; set; }
		public string Command { get; set; }
		public string TransType { get; set; }
		public string BtnName { get; set; }
		public string Message { get; set; }
		public string Inv_no { get; set; }
		public string Inv_dt { get; set; }
		public string ListFilterData { get; set; }
		public string WF_Status { get; set; }
	}
	public class Status
	{
		public string status_id { get; set; }
		public string status_name { get; set; }
	}
	public class SuppPurchaseInvoiceList
	{
		public string InvoiceNo { get; set; }
		public string InvoiceDate { get; set; }
		public string InvDate { get; set; }
		public string Mr_No { get; set; }
		public string Mr_Date { get; set; }
		public string InvoiceType { get; set; }
		public string SuppName { get; set; }
		public string SuppCurrency { get; set; }
		public string InvoiceValue { get; set; }
		public string Stauts { get; set; }
		public string CreateDate { get; set; }
		public string ApproveDate { get; set; }
		public string ModifyDate { get; set; }
		public string create_by { get; set; }
		public string app_by { get; set; }
		public string mod_by { get; set; }
		public string BillNumber { get; set; }
		public string BillDate { get; set; }
	}
}
