using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment
{
    public class BankPaymentList_Model
    {
        public string WF_Status { get; set; }
        public string VouSearch { get; set; }
        public string searchValue { get; set; }
        public string Title { get; set; }
        public string bank_id { get; set; }
        public string bank_name { get; set; }
        public string Src_Type { get; set; }
        public string Status { get; set; }
        public string Reco_Status { get; set; }
        public string VouFromDate { get; set; }
        public string VouToDate { get; set; }
        public string FromDate { get; set; }
        public string ListFilterData { get; set; }
        public string Instr_type { get; set; }
        public DateTime ToDate { get; set; }
        public string FinalToDate { get; set; }/*add By hina on 12-08-2024*/
        public List<VouList> VoucherList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<BankAccList> BankAccNameList { get; set; }
        public List<Currlist> curr_list { get; set; }
        public string Curr_nm { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }
        //public string SearchValue { get; set; }
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
        public String PDC { get; set; }
        public bool HDNListPDC { get; set; }
        public string InterBrch { get; set; }
        public bool HDNListInterBrch { get; set; }
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
        public string Reco_Status { get; set; }
        public string Ins_type { get; set; }
        public string Ins_num { get; set; }
        

    }
    public class BankAccList
    {
        public string bank_acc_id { get; set; }
        public string bank_acc_name { get; set; }
    }
    public class Currlist
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }

    }
}
