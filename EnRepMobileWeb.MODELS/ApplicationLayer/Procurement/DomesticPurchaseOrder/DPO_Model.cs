//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.DomesticPurchaseOrder
//{
//    public class DPODetail_Model
//    {
//        public string SuppName { get; set; }
//        public string POItemName { get; set; }
//        public string SuppPage { get; set; }
//        public string BrchID { get; set; }
		
//	}

//	public class HeaderPODetail
//	{
//		public string TransType { get; set; }
//		public string OrderType { get; set; }
//		public string FClosed { get; set; }
//		public string Cancelled { get; set; }
//		public string PO_No { get; set; }
//		public string PO_Date { get; set; }
//		public string Src_Type { get; set; }
//		public string SrcDocNo { get; set; }
//		public string SrcDocDate { get; set; }
//		public string SuppName { get; set; }
//		public string Currency { get; set; }
//		public string Conv_Rate { get; set; }
//		public string ValidUpto { get; set; }
//		public string ImpFileNo { get; set; }
//		public string CntryOrigin { get; set; }
//		public string Remarks { get; set; }
//		public string CompID { get; set; }
//		public string BranchID { get; set; }
//		public string UserID { get; set; }
//		public string GrVal { get; set; }
//		public string DiscAmt { get; set; }
//		public string TaxAmt { get; set; }
//		public string OcAmt { get; set; }
//		public string NetValBs { get; set; }
//		public string NetValSpec { get; set; }
//		public string OrdStatus { get; set; }
//		public int bill_add_id { get; set; }
//		public string SystemDetail { get; set; }
//	}
//	public class POItemDetail
//	{
//		public string ItemID { get; set; }
//		public string UOMID { get; set; }
//		public string OrderQty { get; set; }
//		public string OrderBQty { get; set; }
//		public string GRNQty { get; set; }
//		public string InvQty { get; set; }
//		public string ItmRate { get; set; }
//		public string ItmDisPer { get; set; }
//		public string ItmDisAmt { get; set; }
//		public string DisVal { get; set; }
//		public string GrossVal { get; set; }
//		public string AssVal { get; set; }
//		public string TaxAmt { get; set; }
//		public string OCAmt { get; set; }
//		public string NetValSpec { get; set; }
//		public string NetValBase { get; set; }
//		public string SimpleIssue { get; set; }
//		public string MRSNo { get; set; }
//		public string FClosed { get; set; }
//		public string Remarks { get; set; }
//	}
//	public class POTaxDetail
//	{
//		public string ItemID { get; set; }
//		public string TaxID { get; set; }
//		public string TaxRate { get; set; }
//		public string TaxValue { get; set; }
//		public string TaxLevel { get; set; }
//		public string TaxApplyOn { get; set; }
//	}
//	public class POOCDetail
//	{
//		public string OC_ID { get; set; }
//		public string OCValue { get; set; }
//	}
//	public class PODeliveryDetail
//	{
//		public string ItemID { get; set; }
//		public string SchDate { get; set; }
//		public string DeliveryQty { get; set; }
//	}
//	public class POTermsDeatil
//	{
//		public string TermsDesc { get; set; }
//	}

//	//public class POAttchmentDetail
// //   {
//	//	public string item_id { get; set; }
//	//	public string file_name { get; set; }
//	//	public string file_path { get; set; }
//	//	public string file_def { get; set; }
//	//	public Int32 comp_id { get; set; }
//	//}

//	public class CurrentDetail
//	{
//		public string CurrentUser { get; set; }
//		public string CurrentDT { get; set; }
//	}

//	public class DPOListModel
//    {
//		public string Title { get; set; }
//		public string SuppName { get; set; }
//        public string SuppID { get; set; }
//        public string PO_FromDate { get; set; }
//        public string PO_ToDate { get; set; }
//        public string OrderType { get; set; }
//        public string Status { get; set; }
//        public string FromDate { get; set; }
//        public DateTime ToDate { get; set; }
//		public string wfdocid { get; set; }
//		public string wfstatus { get; set; }
//		public string OrderNo { get; set; }
//		public string OrderDate { get; set; }
//		public string attatchmentdetail { get; set; }
//		public List<SupplierName> SupplierNameList { get; set; }
//		public List<Status> StatusList { get; set; }
//		public List<DomesticPurchaseOrderList> DPOList { get; set; }
//    }
//	public class Status
//	{
//		public string status_id { get; set; }
//		public string status_name { get; set; }
//	}
//	public class SupplierName
//    {
//        public string supp_id { get; set; }
//        public string supp_name { get; set; }
//    }
//    public class DomesticPurchaseOrderList
//    {
//        public string OrderNo { get; set; }
//        public string OrderDate { get; set; }
//		public string OrderDt { get; set; }
//		public string OrderType { get; set; }
//        public string SourceType { get; set; }
//        public string SourceDocNo { get; set; }
//        public string SuppName { get; set; }
//        public string Currency { get; set; }
//        public string OrderValue { get; set; }
//        public string OrderStauts { get; set; }
//        public string CreateDate { get; set; }
//        public string ApproveDate { get; set; }
//        public string ModifyDate { get; set; }
//    }
//}
