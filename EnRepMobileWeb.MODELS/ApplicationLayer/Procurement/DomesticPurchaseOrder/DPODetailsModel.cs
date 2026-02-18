using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.DomesticPurchaseOrder
{
	public class PODetailsModel
	{
        public string item { get; set; }
        public string Pending_Src_DocNo { get; set; }
        public string Pending_Src_Docdt { get; set; }
        public string Pending_Supp_id { get; set; }
        public string Pending_SourceDocument { get; set; }
        public string ContectNumber { get; set; }
        public string ContectPersone { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string hdn_item_typ { get; set; }
        public string Item_type { get; set; }
        public string DocumentMenuId { get; set; }
        public string Title { get; set; }
		//public string documentStatus { get; set; }
		public string SuppName { get; set; }
		public string SuppID { get; set; }
		public List<SupplierName> SupplierNameList { get; set; }
		public List<CurrancyList> currancyLists { get; set; }
		public List<CountryOfOrigin> countryOfOrigins { get; set; }
		public List<SrcDocNoList> docNoLists { get; set; }

        public string trade_term { get; set; }
        public string Delete { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string ExchDigit { get; set; }
        public string POItemName { get; set; }
        public string SuppPage { get; set; }
        public string BrchID { get; set; }
        public string TransType { get; set; }
        public string POOrderType { get; set; }
        public Boolean FClosed { get; set; }
        public Boolean Cancelled { get; set; }
        public string PO_No { get; set; }
        public string PO_Date { get; set; }
        public string ReplicateWith { get; set; }
        public string Src_Type { get; set; }
        public string SrcDocNo { get; set; }
        public string SrcDocDate { get; set; }
        public string Currency { get; set; }
        public string bs_curr_id { get; set; }
        public string Conv_Rate { get; set; }
        public string ValidUpto { get; set; }
        public string Del_dstn { get; set; }
        public string ShipTo { get; set; }
        public string Pymnt_trms { get; set; }
        public string ImpFileNo { get; set; }
        public string CntryOrigin { get; set; }
        public string CountryName { get; set; }
        public string CountryId { get; set; }
        
        public string PortOrigin { get; set; }
        public string CancelledRemarks { get; set; }
        public string Remarks { get; set; }
        public string CompID { get; set; }
        public string BranchID { get; set; }
        public string UserID { get; set; }
        public string GrVal { get; set; }
        public string AssessableVal { get; set; }
        public string DiscAmt { get; set; }
        public string TaxAmt { get; set; }
        public string OcAmt { get; set; }
        public string NetValBs { get; set; }
        public string NetValSpec { get; set; }
        public string OrdStatus { get; set; }
        public string Address { get; set; }
        public int bill_add_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string PriceBasis { get; set; }
        public string FreightType { get; set; }
        public string ModeOfTransport { get; set; }
        public string Destination { get; set; }
        public string SystemDetail { get; set; }
        //public string wfdocid { get; set; }
        //public string wfstatus { get; set; }
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string StatusName { get; set; }
       // public string Status_Code { get; set; }
        public string Itemdetails { get; set; }
        public string ItemTaxdetails { get; set; }
        public string ItemOCdetails { get; set; }
        public string ItemOCTaxdetails { get; set; }
        
        public string ItemDelSchdetails { get; set; }
        public string ItemTermsdetails { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string attatchmentdetail { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string ListFilterData1 { get; set; }
        public List<trade_termList> TradeTermsList { get; set; }
        public string ILSearch { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string WF_status1 { get; set; }
        public string SrcType { get; set; }
        public string ForAmmendendBtn { get; set; }
        public string Amendment { get; set; }
        public string Amend { get; set; }
        public string wfDisableAmnd { get; set; }
        public string SubItemDetailsDt { get; set; }//For Sub-item
        public string GstApplicable { get; set; }
        public string ShowProdDesc { get; set; } = "Y"; 
        public string ShowDeliverySchedule { get; set; } = "Y"; 
        public string ShowHSNNumber { get; set; } = "Y"; 
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowRemarksBlwItm { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string ItemAliasName { get; set; } = "N"; 
        public string SupplierAliasName { get; set; } = "N"; 
        public string ShowPayTerms { get; set; } = "N"; 
        public string ShowTotalQty { get; set; } = "N";
        public string ShowMRP { get; set; } = "N";
        public string ShowPackSize { get; set; } = "N";
        public string PrintFormat { get; set; } = "F1";
        public string FCFlag { get; set; } = "N";
        public string req_area { get; set; }
        public List<RequirementAreaList> _requirementAreaLists { get; set; }
    }
    public class RequirementAreaList
    {
        public int req_id { get; set; }
        public string req_val { get; set; }
    }
    public class trade_termList
    {
        public string TrdTrms_id { get; set; }
        public string TrdTrms_val { get; set; }
    }
    //public class CountryList
    //{
    //    public string Cntry_id { get; set; }
    //    public string Cntry_val { get; set; }
    //}
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
	public class CountryOfOrigin
	{
		public string cntry_id { get; set; }
		public string cntry_name { get; set; }
	}
	public class SrcDocNoList
	{
		public string SrcDocnoId { get; set; }
		public string SrcDocnoVal { get; set; }
	}
    public class DPOListModel
    {
        public string Itemtype { get; set; }
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string DocumentMenuId { get; set; }
        public string Title { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public string PO_FromDate { get; set; }
        public string PO_ToDate { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string attatchmentdetail { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<DomesticPurchaseOrderList> DPOList { get; set; }
        public List<PendingDocumentList> Pending_DocumentList { get; set; }
        public string DPOSearch { get; set; }
        public string PendingDPOSearch { get; set; }
        public string WF_status { get; set; }
        public string SrcType { get; set; }
        public string Message { get; set; }
        public string GstApplicable { get; set; }
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
    public class DomesticPurchaseOrderList
    {
        public string item_type { get; set; }
        public string supp_id { get; set; }

        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string OrderDt { get; set; }
        public string OrderType { get; set; }
        public string req_area { get; set; }
        public string SourceType { get; set; }
        public string SourceDocNo { get; set; }
        public string SuppName { get; set; }
        public string Currency { get; set; }
        public string OrderValue { get; set; }
        public string OrderStauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
        public string imp_file_no { get; set; }
        public string country_origin { get; set; }
    }
    public class PendingDocumentList
    {
        public string flag { get; set; }
        public string Pending_Docdt { get; set; }
        public string Pending_Req_area_id { get; set; }
        public string Pending_supplier_ID { get; set; }
        public string Pending_Req_area { get; set; }
        public string Pending_Document_no { get; set; }
        public string Pending_Document_dt { get; set; }
        public string Pending_OrderDt { get; set; }     
        public string Pending_SourceDocAlias { get; set; }
        public string Pending_SourceDocName { get; set; }
        public string Pending_Supplier_Name { get; set; }
        public string Pending_Doc_Status { get; set; }
        public string Pending_CreateDate { get; set; }
        public string Pending_ApproveDate { get; set; }     
        public string Pending_create_by { get; set; }
        public string Pending_app_by { get; set; }     
       
    }
    public class URLDetailModel
    {
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string DocumentMenuId { get; set; }
        public string Amend { get; set; }
    }
    public class PurchaseOrderattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
}
