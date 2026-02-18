using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.JobInvoice
{
    public class JobInvoiceModel
    {
		//----Header Detail Start-------//

		public string hdnsaveApprovebtn { get; set; }
		public string WF_Status1 { get; set; }
		public string DocumentMenuId { get; set; }
		public string DocumentStatus { get; set; }
		public string Title { get; set; }
		public string BtnName { get; set; }
		public string Command { get; set; }
		public string AppStatus { get; set; }
		public string TransType { get; set; }
		public string Message { get; set; }
		public string StatusName { get; set; }
		public string StatusCode { get; set; }
		public Boolean Cancelled { get; set; }
		public string DeleteCommand { get; set; }
		public string CreatedBy { get; set; }
		public string CreatedOn { get; set; }
		public string ApprovedBy { get; set; }
		public string ApprovedOn { get; set; }
		public string AmendedBy { get; set; }
		public string AmendedOn { get; set; }
		public string Status { get; set; }
		public string doc_status { get; set; }
		public string WFBarStatus { get; set; }
		public string WFStatus { get; set; }
		public string Create_id { get; set; }
		public string GRNStatus { get; set; }
		public string A_Status { get; set; }
		public string A_Level { get; set; }
		public string A_Remarks { get; set; }
		public string ValDigit { get; set; }
		public string QtyDigit { get; set; }
		public string RateDigit { get; set; }
		public string TranstypAttach { get; set; }
		public string ListFilterData1 { get; set; }
		public string CC_DetailList { get; set; }
		public string DocNoAttach { get; set; }
		public string attatchmentdetail { get; set; }
		public DataTable AttachMentDetailItmStp { get; set; }
		public string Guid { get; set; }
		public string CompId { get; set; }
		public string JInv_No { get; set; }
        public string JInv_Dt { get; set; }
		public string Address { get; set; }
		public int bill_add_id { get; set; }
		public string SuppID { get; set; }
		public string DocumentNo { get; set; }
		public string SuppName { get; set; }
		public string GRNNumber { get; set; }
		public string GRN_Number { get; set; }
		public string GRNDate { get; set; }
		public string BrchID { get; set; }
		public List<SupplierName> SupplierNameList { get; set; }
		public List<DocumentNumber> GRNNumberList { get; set; }
		public List<CurrancyList> currancyLists { get; set; }
		public string Bill_No { get; set; }
        public string Bill_Dt { get; set; }
        public string Curr_Id { get; set; }
        public string Conv_Rate { get; set; }
		public string Currency { get; set; }
		public string ExRate { get; set; }
		public string User_Id { get; set; }
        public string Inv_Status { get; set; }
		public string NetAmount { get; set; }
		public string TDS_Amount { get; set; }
		public string tds_details { get; set; }
		public string oc_tds_details { get; set; }
		public string OtherCharges { get; set; }
		public string TaxAmount { get; set; }
		public string GrossValue { get; set; }
		public string vouDetail { get; set; }
		public string GLVoucherType { get; set; }
		public string GLVoucherNo { get; set; }
		public string GLVoucherDt { get; set; }
		public string Narration { get; set; }

        //----Item Detail Start-------//
		public string item_id { get; set; }
		public string ItemDetails { get; set; }
		public string TaxDetail { get; set; }
		public string OC_TaxDetail { get; set; }
		public string OCDetail { get; set; }
		public string Ship_Gst_number { get; set; }
		public string Ship_StateCode { get; set; }
		public string Hd_GstType { get; set; }
		public string Hd_GstCat { get; set; }
		
		public List<Status> StatusList { get; set; }
		
		public string mr_qty { get; set; }
        public string item_rate { get; set; }
        public string item_gr_val { get; set; }
        public string item_tax_amt { get; set; }
        public string item_oc_amt { get; set; }
        public string item_net_val_spec { get; set; }
        public string item_net_val_bs { get; set; }
        public string gl_vou_no { get; set; }
        public string gl_vou_dt { get; set; }
        public string Nurration { get; set; }
        public string BP_Nurration { get; set; }
        public string DN_Nurration { get; set; }
        public string supp_acc_id { get; set; }
        public string DocSuppOtherCharges { get; set; }

    }
   
	public class DocumentNumber
	{
		public string GrnNoId { get; set; }
		public string GrnnoVal { get; set; }

	}
	public class CurrancyList
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
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
	public class JI_ListModel
	{
		public string WF_Status { get; set; }
		public string ListFilterData { get; set; }
		public string Title { get; set; }
		public string SuppName { get; set; }
		public string SuppID { get; set; }
		public string Status { get; set; }
		public string FromDate { get; set; }
		public string ToDate { get; set; }
		public DateTime FinStDt { get; set; }
		public string JINVSearch { get; set; }
		public List<SupplierName> SupplierNameList { get; set; }
		public List<Status> StatusList { get; set; }
		public List<JobInvoiceList> JIList { get; set; }
	}
	public class Status
	{
		public string status_id { get; set; }
		public string status_name { get; set; }
	}
	public class JobInvoiceList
	{
		public string InvoiceNo { get; set; }
		public string InvoiceDate { get; set; }
		public string InvDate { get; set; }
		public string SuppName { get; set; }
		public string GRNNumber { get; set; }
		public string GRNDate { get; set; }
		public string GRNDt { get; set; }
		public string InvoiceValue { get; set; }
		public string Stauts { get; set; }
		public string CreateDate { get; set; }
		public string ApproveDate { get; set; }
		public string ModifyDate { get; set; }
		public string Create_By { get; set; }
		public string App_By { get; set; }
		public string Mod_By { get; set; }
	}
	public class JIDetailsattch
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
		
	}
}
