using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment
{
   public class BankPayment_Model
    {
		public string CancelledRemarks { get; set; }		
		public string hdnsaveApprovebtn { get; set; }		
		public string BankPaymentDate { get; set; }		
		public string BankPaymentNo { get; set; }		
		public string WF_Status1 { get; set; }		
		public string BankReceiptDate { get; set; }		
		public string BankReceiptNo { get; set; }		
		public string DocumentStatus { get; set; }		
		public string Command { get; set; }		
		public string Message { get; set; }
		public string Message1 { get; set; }
		public string Title { get; set; }		
		public string BtnName { get; set; }		
		public string TransType { get; set; }	
		public string Vou_No { get; set; }
		
		public string RecoStatus { get; set; }
		public string ReasonReturn { get; set; }
		public string RecoStatusCode { get; set; }
        public string Vou_Date { get; set; }
		public string Src_Type { get; set; }
		public string src_doc_no { get; set; }
		public string src_doc_date { get; set; }
		public string Vou_amount { get; set; }		
		public string Remarks { get; set; }
		public string acc_id { get; set; }
		public string acc_Name { get; set; }
		public string bank_acc_id { get; set; }
		public string bank_acc_Name { get; set; }
		public string BankName { get; set; }
		public int? curr { get; set; }
		public int? bs_curr_id { get; set; }
		public string conv_rate { get; set; }		
		public string DeleteCommand { get; set; }
		public string GlAccountDetails { get; set; }
		public string BillAdjdetails { get; set; }

		public string ins_type { get; set; }
		public string ins_no { get; set; }
		public string ins_dt { get; set; }
		public string ins_name { get; set; }

		public string hd_od_allow { get; set; }
		public string hd_od_limit { get; set; }
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
		public List<DocumentNumber> DocumentNumberList { get; set; }
		public List<BankAccName> BankAccNameList { get; set; }
		public List<curr> currList { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
		public DataTable AttachMentDetailItmStp { get; set; }
		public string CC_DetailList { get; set; }
		/*----Work for PDC add by Hina on 08-08-2024-------------*/
		public bool PDCFlag { get; set; }
		public string HdnPDCFlag { get; set; }

		public bool IntrBrnchFlag { get; set; }
		public string HdnIntBrFlag { get; set; }
		public string HdnIntBrNurr_BP { get; set; }
		public string HdnIntBrNurr_BR { get; set; }

		public string VouMin_Date { get; set; }/*Add by Hina on*/
		public string VouMax_Date { get; set; }
		public string ibt_acc { get; set; } = "N";/*Add by Suraj Maurya on 30-08-2024*/
		public string UserID { get; set; }
	}
	public class BankPaymentModel
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
		public string VN { get; set; }
		public string VDT { get; set; }

	}
	public class DocumentNumber
	{
		public string doc_no { get; set; }
		public string doc_dt { get; set; }
	}

	public class BankAccName
	{
		public string bank_acc_id { get; set; }
		public string bank_acc_name { get; set; }
	}
	public class curr
	{
		public string curr_id { get; set; }
		public string curr_name { get; set; }

	}
	
	public class BankPaymentListDataModel
	{
		public int SrNo { get; set; }
		public string bank_name { get; set; }
		public string acc_id { get; set; }
		public string vou_no { get; set; }
		public string vou_dt { get; set; }
		public string vou_date { get; set; }
		public string vou_status { get; set; }
		public string created_on { get; set; }
		public string app_dt { get; set; }
		public string create_by { get; set; }
		public string mod_by { get; set; }
		public string app_by { get; set; }
		public string mod_on { get; set; }
		public string SrcType { get; set; }
		public string pdc { get; set; }
		public string int_br { get; set; }
		public string src_doc_no { get; set; }
		public string src_doc_dt { get; set; }
		public string vou_amt { get; set; }
		public string curr_logo { get; set; }
		public string reco_status { get; set; }
		public string ins_type { get; set; }
		public string ins_no { get; set; }

	}

}
