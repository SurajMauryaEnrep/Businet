using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.Purchase_Quotation
{
	public class PQList_Model
	{
		public string SupplierID { get; set; }
		public string ListFilterData1 { get; set; }
		public string ListFilterData { get; set; }
		public string title { get; set; }
		public string SuppID { get; set; }
		public string PQ_FromDate { get; set; }
		public string PQ_ToDate { get; set; }
		public string PQ_status { get; set; }
		public string WF_status { get; set; }
		public string Command { get; set; }
		public string TransType { get; set; }
		public string BtnName { get; set; }
		public string PQSearch { get; set; }
		public string FinStDt { get; set; }
		public string Message { get; set; }
		
		public List< SuppList> suppLists { get; set; }
		public List<statusLists> statusLists { get; set; }
	}
	public class PurchaseQuotation_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string ListFilterData1 { get; set; }
        public string title { get; set; }
		public string Create_id { get; set; }
		public string Create_by { get; set; }
		public string Create_on { get; set; }
		public string Amended_by { get; set; }
		public string Amended_on { get; set; }
		public string Approved_by { get; set; }
		public string Approved_on { get; set; }
		public string StatusName { get; set; }
		public string TransType { get; set; }
		public string SubItemDetailsDt { get; set; }
		public string SourceType { get; set; }
		//public string FClosed { get; set; }
		public string hdnfromDt { get; set; }
		public bool Cancelled { get; set; }
		public string PQ_No { get; set; }
		public string PQ_Date { get; set; }
		public string Supp_Type { get; set; }
		public string SuppID { get; set; }
		public string SuppName { get; set; }
		public string SrcDocNo { get; set; }
		public string SrcDocDate { get; set; }
		public string AddQuotation { get; set; }
		public string Currency { get; set; }
		public string Payterm { get; set; }
		public string Conv_Rate { get; set; }
		public string ValidUpto { get; set; }
		public string PONoAndDate { get; set; }
		//public string ImpFileNo { get; set; }
		//public string CntryOrigin { get; set; }
		public string Remarks { get; set; }
		public Boolean RaiseOrder { get; set; }
		public string hdnRaiseOrder { get; set; }
		

		//public string CompID { get; set; }
		//public string BranchID { get; set; }
		//public string UserID { get; set; }
		public string GrVal { get; set; }
		public string DiscAmt { get; set; }
		public string TaxAmt { get; set; }
		public string OcAmt { get; set; }
		public string NetValBs { get; set; }
		public string NetValSpec { get; set; }
		public string Status { get; set; }
		public string Address { get; set; }
		public string Ship_Gst_number { get; set; }
		public string Ship_StateCode { get; set; }
		public int bill_add_id { get; set; }
		public string attatchmentdetail { get; set; }
		public string Itemdetails { get; set; }//
		public string rfqItemdetails { get; set; }
		public string ItemTaxdetails { get; set; }
		public string ItemOCdetails { get; set; }
		public string ItemDelSchdetails { get; set; }
		public string ItemTermsdetails { get; set; }
		public string DocumentMenuId { get; set; }
		public string WFBarStatus { get; set; }
		public string WFStatus { get; set; }
		public string Delete { get; set; }
		public List<SuppList> suppLists { get; set; }
		public List<ProsSuppList> ProsSuppLists { get; set; }
		public string Command { get; set; }
		public string DocumentStatus { get; set; }
		public string Message { get; set; }
		public string BtnName { get; set; }
		public string WF_status1 { get; set; }
		public string PQNo { get; set; }
		public string PQDate { get; set; }
		public string ProspectFromRFQ { get; set; }
		public string ProspectFromPQ { get; set; }
		public string ProspectFromQuot { get; set; }
		public string AppStatus { get; set; }
		public DataTable AttachMentDetailItmStp { get; set; }
		public string Guid { get; set; }
	}
	public class PurchaseQuotationattch
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
	}
	public class SuppList
    {
		public string Supp_id { get; set; }
		public string Supp_name { get; set; }
    }
	public class ProsSuppList
	{
		public string Supp_id { get; set; }
		public string Supp_name { get; set; }
	}
	public class statusLists
	{
		public string status_id { get; set; }
		public string status_name { get; set; }
	}
	public class PQItemDetail
	{
		public string ItemID { get; set; }
		public string UOMID { get; set; }
		public string QuotedQty { get; set; }
		public string QuotedPrice { get; set; }
		//public string GRNQty { get; set; }
		//public string InvQty { get; set; }
		public string ItmDisPer { get; set; }
		public string ItmDisAmt { get; set; }
		public string ItmRate { get; set; }
	
		//public string DisVal { get; set; }
		//public string GrossVal { get; set; }
		//public string AssVal { get; set; }
		public string TaxAmt { get; set; }
		public string OCAmt { get; set; }
		//public string NetValSpec { get; set; }
		public string NetValBase { get; set; }
		//public string SimpleIssue { get; set; }
		//public string MRSNo { get; set; }
		//public string FClosed { get; set; }
		public string Remarks { get; set; }
	}
	public class TaxDetail
	{
		public string ItemID { get; set; }
		public string TaxID { get; set; }
		public string TaxRate { get; set; }
		public string TaxValue { get; set; }
		public string TaxLevel { get; set; }
		public string TaxApplyOn { get; set; }
	}
	public class OCDetail
	{
		public string OC_ID { get; set; }
		public string OCValue { get; set; }
	}
	public class DeliveryDetail
	{
		public string ItemID { get; set; }
		public string SchDate { get; set; }
		public string DeliveryQty { get; set; }
	}
	public class TermsDeatil
	{
		public string TermsDesc { get; set; }
	}

	public class AttchmentDetail
	{
		public string[] FileDetail { get; set; }
	}
}
