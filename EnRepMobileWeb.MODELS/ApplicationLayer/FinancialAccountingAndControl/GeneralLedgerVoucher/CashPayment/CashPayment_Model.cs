using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.CashPayment
{
   public class CashPayment_Model
    {
		public string CancelledRemarks { get; set; }
		public string hdnsaveApprovebtn { get; set; }
		public string WF_Status1 { get; set; }
		public string VoucherDate { get; set; }
		public string VoucherNo { get; set; }
		public string Message { get; set; }
		public string DocumentStatus { get; set; }
		public string Command { get; set; }
		public string BtnName { get; set; }
		public string Title { get; set; }
		public string TransType { get; set; }
		public string Vou_No { get; set; }
		public string Vou_Date { get; set; }
		public string Src_Type { get; set; }
		public string src_doc_no { get; set; }
		public string src_doc_date { get; set; }
		public string Vou_amount { get; set; }
		public string Remarks { get; set; }
		public string acc_id { get; set; }
		public string acc_Name { get; set; }
		public string cash_acc_id { get; set; }
		public string cash_acc_Name { get; set; }
		public string CashName { get; set; }
		public int? curr { get; set; }
		public int? bs_curr_id { get; set; }
		public string conv_rate { get; set; }
		public string DeleteCommand { get; set; }
		public string GlAccountDetails { get; set; }
		public string Create_id { get; set; }
		public string Create_by { get; set; }
		public string Create_on { get; set; }
		public string Amended_by { get; set; }
		public string Amended_on { get; set; }
		public string Approved_by { get; set; }
		public string Approved_on { get; set; }
		public string Status { get; set; }
		public string VouStatus { get; set; }
		public bool CancelFlag { get; set; }
		public bool Cancelled { get; set; }
		public string WFBarStatus { get; set; }
		public string WFStatus { get; set; }
		public string Guid { get; set; }
		public string BillAdjdetails { get; set; }
		public string ListFilterData1 { get; set; }
		public DataTable AttachMentDetailItmStp { get; set; }
		public List<DocumentNumber> DocumentNumberList { get; set; }
		public List<CashAccName> CashAccNameList { get; set; }
		public List<curr> currList { get; set; }
		public string attatchmentdetail { get; set; }

        public string CC_DetailList { get; set; }
		public string UserID { get; set; }
	}
    public class UrlModel {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string VN { get; set; }
        public string VDT { get; set; }

    }
	public class CashRecipt
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
	}
	public class DocumentNumber
	{
		public string doc_no { get; set; }
		public string doc_dt { get; set; }
	}

	public class CashAccName
	{
		public string cash_acc_id { get; set; }
		public string cash_acc_name { get; set; }
	}
	public class curr
	{
		public string curr_id { get; set; }
		public string curr_name { get; set; }

	}

    
}

