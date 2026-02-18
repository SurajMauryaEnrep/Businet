using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra
{
   public class ContraVoucherModel
    {

		public DataTable AttachMentDetailItmStp { get; set; }
		public string CancelledRemarks { get; set; }
		public string hdnsaveApprovebtn { get; set; }
		public string WF_Status1 { get; set; }
		public string ContraDate { get; set; }
		public string ContraNo { get; set; }
		public string BtnName { get; set; }
		public string Message { get; set; }
		public string DocumentStatus { get; set; }
		public string Command { get; set; }
		public string Title { get; set; }
		public string TransType { get; set; }
		public string Vou_No { get; set; }
		public string Vou_Date { get; set; }
		public string acc_id { get; set; }
		public string acc_Name { get; set; }
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
		public string ListFilterData1 { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
	}
	public class ContraModel
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
	}
	public class UrlModel
	{
		public string bt { get; set; }
		public string Cmd { get; set; }
		public string tp { get; set; }
		public string wf { get; set; }
		public string CNO { get; set; }
		public string CDT { get; set; }
		public string DMS { get; set; }

	}
	public class ContraList_Model
	{
	 public string WF_Status { get; set; }
	 public string VouSearch { get; set; }
		public string Title { get; set; }
		public string bank_id { get; set; }
		public string bank_name { get; set; }
		public string Src_Type { get; set; }
		public string Status { get; set; }
		public string VouFromDate { get; set; }
		public string VouToDate { get; set; }
		public string FromDate { get; set; }
		public string ListFilterData { get; set; }
		public DateTime ToDate { get; set; }
		public List<VouList> VoucherList { get; set; }
		public List<Status> StatusList { get; set; }
		public List<BankAccList> BankAccNameList { get; set; }
	}
	public class Status
	{
		public string status_id { get; set; }
		public string status_name { get; set; }
	}
	public class VouList
	{
		public string bank_name { get; set; }
		public string VouNumber { get; set; }
		public string VouDate { get; set; }
		public string hdVouDate { get; set; }
		public string SrcType { get; set; }
		public string ReqNo { get; set; }
		public string ReqDt { get; set; }
		public string curr_logo { get; set; }
		public string Amount { get; set; }
		public string VouStatus { get; set; }
		public string CreatedON { get; set; }
		public string ModifiedOn { get; set; }
		public string ApprovedOn { get; set; }
		public string create_by { get; set; }
		public string app_by { get; set; }
		public string mod_by { get; set; }

	}
	public class BankAccList
	{
		public string bank_acc_id { get; set; }
		public string bank_acc_name { get; set; }
	}
}
