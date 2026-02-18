using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.BankRecancellation
{
    public class BankRecancellation_Model
    {
        
        public string Account_Bal { get; set; }
        public string Command { get; set; }
        public string Bnk_List { get; set; }
        public string Fy_OpDate { get; set; }
        public string Fy_ClDate { get; set; }
        public string VouSearch { get; set; }
        public string FyMinDate { get; set; }
        public string SaveVouList { get; set; }
        public string ToMinDate { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string ClReceipt { get; set; }
        public string ClPayment { get; set; }
        public string ReReceipt { get; set; }
        public string RePayment { get; set; }
        public string UnReceipt { get; set; }
        public string UnPayment { get; set; }
       
        public List<BankList> _bankLists { get; set; }

        public List<SerchedList> _VoucherList { get; set; }
        public string BankRe_VouList { get; set; }
        public string HdnCsvPrint { get; set; }
        public string searchValue { get; set; }
        public string Status { get; set; }
        public string TransactionType { get; set; }
        public string ToDate { get; set; }
        public string Acc_id { get; set; }
        public class BankList
        {
            public string bank_id { get; set; }
            public string bank_name { get; set; }
            public string Curr_Id { get; set; }
        }
        
    }
   
    public class Fy_Todate
    {
        public string FyToDate { get; set; }
        public string ParMinDate { get; set; }
      
    }

    public class SerchedList 
    {
        public string Bank_Name { get; set; }
        public string Vou_No { get; set; }
        public string Vou_dt { get; set; }
        public string VouDt { get; set; }
        public string Vou_type { get; set; }
        public string Ins_Num { get; set; }
        public string Ins_dt { get; set; }
        public string InsDt { get; set; }
        public string Amt_bs { get; set; }
        public string Amt_sp { get; set; }
        public string Status { get; set; }
        public string Cl_dt { get; set; }
        public string ClearDate { get; set; }
        public string Rsn_Ret { get; set; }
        public string InstrDate { get; set; }
    }

    public class CommadsModel
    {
        public string Cmd { get; set; }
    }
}
