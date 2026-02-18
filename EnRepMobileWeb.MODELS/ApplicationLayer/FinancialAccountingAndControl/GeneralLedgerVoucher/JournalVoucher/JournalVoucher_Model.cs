using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher
{
   public class JournalVoucher_Model
    {
		// For Header Detail
		public string CancelledRemarks { get; set; }
		public string hdnsaveApprovebtn { get; set; }
		public string JV_num{ get; set; }
		public string JV_dt{ get; set; }
		public string WF_Status1 { get; set; }
		public string WF_Status { get; set; }
		public string JVSearch { get; set; }
		public string DocumentStatus { get; set; }
		public string Command { get; set; }
		public string Message { get; set; }
		public string Message1 { get; set; }
		public string BtnName { get; set; }
		public string Title { get; set; }
		public string Create_id { get; set; }
		public string Create_by { get; set; }
		public string Create_on { get; set; }
		public string Amended_by { get; set; }
		public string Amended_on { get; set; }
		public string Approved_by { get; set; }
		public string Approved_on { get; set; }
		public string StatusName { get; set; }
		public string TransType { get; set; }
		public bool CancelFlag { get; set; }
		public bool Cancelled { get; set; }
		public string JV_No { get; set; }
		public string JV_Date { get; set; }
		public string JVT_amount { get; set; }
		public string Narrat { get; set; }
		public string Remarks { get; set; }
		// For Account Detail
		public string acc_id { get; set; }
		public string acc_Name { get; set; }
		public int grp_id { get; set; }
		public string grp_Name { get; set; }
		public string DeleteCommand { get; set; }
		public string GlAccountDetails { get; set; }
		public string WFBarStatus { get; set; }
		public string WFStatus { get; set; }
		public string attatchmentdetail { get; set; }
		public string searchValue { get; set; }

		// For List Page
		public string doc_status { get; set; }
		public string SourceType { get; set; }
		public string JV_FromDate { get; set; }
		public string JV_ToDate { get; set; }
		public string JV_status { get; set; }
		public DataTable AttachMentDetailItmStp { get; set; }		
		public string Guid { get; set; }
		public string ListFilterData { get; set; }
		public string ListFilterData1 { get; set; }
		public List<StatusList> _StatusLists { get; set; }
		public class StatusList
		{
			public string status_id { get; set; }
			public string status_name { get; set; }
		}

        /* -------for Cost Center------*/
        public string CC_DetailList { get; set; }
        public string ReplicateWith { get; set; }
		public string item { get; set; }
	}
	public class UrlModel
	{
		public string bt { get; set; }
		public string Cmd { get; set; }
		public string tp { get; set; }
		public string wf { get; set; }
		public string JV_num { get; set; }
		public string JV_dt { get; set; }

	}
	public class JournalVoucher_model
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
	}
}
