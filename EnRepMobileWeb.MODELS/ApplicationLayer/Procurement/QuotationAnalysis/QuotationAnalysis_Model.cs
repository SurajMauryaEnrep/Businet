using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.QuotationAnalysis
{
	public class QAList_Model
	{
		
		public string ListFilterData1 { get; set; }
		public string ListFilterData { get; set; }
		public string title { get; set; }
		public string PQA_FromDate { get; set; }
		public string PQA_ToDate { get; set; }
		public string PQA_status { get; set; }
		public string WF_status { get; set; }
		public string Command { get; set; }
		public string TransType { get; set; }
		public string BtnName { get; set; }
		public string PQASearch { get; set; }
		public string FinStDt { get; set; }
		public string Message { get; set; }
		public string DocumentMenuId { get; set; }
		public List<statusLists> statusLists { get; set; }
	}
	public class QuotationAnalysis_Model
    {
		public string DeleteCommand { get; set; }
		public string hdnsaveApprovebtn { get; set; }
		public string SubItemDetailsDt { get; set; }
        public string ListFilterData1 { get; set; }
        public string title { get; set; }
		public string DocumentMenuId { get; set; }
		public string WFBarStatus { get; set; }
		public string WFStatus { get; set; }
		public string Create_id { get; set; }
		public string Create_by { get; set; }
		public string Create_on { get; set; }
		public string Amended_by { get; set; }
		public string Amended_on { get; set; }
		public string Approved_by { get; set; }
		public string Approved_on { get; set; }
		public string StatusName { get; set; }
		public string TransType { get; set; }
		public string Status { get; set; }
		public string SourceType { get; set; }
		public string hdnfromDt { get; set; }
		public bool Cancelled { get; set; }
		public string QA_No { get; set; }
		public string QA_Date { get; set; }
		public string Supp_Type { get; set; }
		public string SuppID { get; set; }
		public string SuppName { get; set; }
		public string attatchmentdetail { get; set; }
		public string Itemdetails { get; set; }
		public string Delete { get; set; }
		public List<RFQList> rfqLists { get; set; }
		public string rfqID { get; set; }
		public string rfqdt { get; set; }
		public string rfqVal { get; set; }
		public string Command { get; set; }
		public string DocumentStatus { get; set; }
		public string Message { get; set; }
		public string BtnName { get; set; }
		public string WF_status1 { get; set; }
		public string AppStatus { get; set; }
		public DataTable AttachMentDetailItmStp { get; set; }
		public string Guid { get; set; }
        public string CancelledRemarks { get; set; }
    }
	public class QuotationAnalysisattch
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
	}
	public class RFQList
	{
		public string RFQ_id { get; set; }
		public string RFQ_value { get; set; }
    }
	public class statusLists
	{
		public string status_id { get; set; }
		public string status_name { get; set; }
	}
	public class PQItemDetail
	{
		public string ItemID { get; set; }
		public string ItemName { get; set; }
		public string ItemType { get; set; }
		public string UOMID { get; set; }
		public string UOMName{ get; set; }
		public string QuotedQty { get; set; }
		public string QuotedPrice { get; set; }
		public string ItmDisPer { get; set; }
		public string ItmDisAmt { get; set; }
		public string ItmRate { get; set; }
		public string TaxAmt { get; set; }
		public string OCAmt { get; set; }
		public string NetValBase { get; set; }
		public string Remarks { get; set; }
		public string SupplierName { get; set; }
		public string SupplierId { get; set; }
		public string SupplierType { get; set; }
		public string SupplierRating { get; set; }
		public string QuotationDetails { get; set; }
		public string QuotationInv { get; set; }
		public string QuotationDt { get; set; }
		public string SuppPaymentTerms { get; set; }
		public string DeliveryDate { get; set; }
		public string OrderFor { get; set; }
	}
	public class AttchmentDetail
	{
		public string[] FileDetail { get; set; }
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
		public string ListFilterData1 { get; set; }
		public string DocumentMenuId { get; set; }
	}
	public class DirectPurchaseInvoiceattch
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
		//public string AttachMentDetailItmStp { get; set; }
	}
}
