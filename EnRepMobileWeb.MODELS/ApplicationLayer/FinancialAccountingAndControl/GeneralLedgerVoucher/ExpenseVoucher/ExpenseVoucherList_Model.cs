using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.ExpenseVoucher
{
    public class ExpenseVoucherList_Model
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Payeeacc { get; set; }
        public string ListFilterData { get; set; }
        public string WF_Status { get; set; }
        public string EXvou_Status { get; set; }
        public string ExpVouSearch { get; set; }
        public string Title { get; set; }
        public List<ExVouList> VouList { get; set; }
        public List<PayeeAccLst> PaAccLst { get; set; }
        public List<StatusList> _StatusLists { get; set; }

        public class StatusList
        {
            public string status_id { get; set; }
            public string status_name { get; set; }
        }
        public class ExVouList
        {
            public string Vou_No { get; set; }
            public string Vou_Dt { get; set; }
            public string Vou_Date { get; set; }
            public string acc_id { get; set; }
            public string PayAmt { get; set; }
            public string ExpAmt { get; set; }
            public string Vou_Status { get; set; }
            public string CreateBy { get; set; }
            public string CreateDt { get; set; }
            public string Approve_by { get; set; }
            public string Approve_Date { get; set; }
            public string Modify_By { get; set; }
            public string Modify_Date { get; set; }

        }
        public class PayeeAccLst
        {
            public string acc_id { get; set; }
            public string acc_name { get; set; }
        }

    }
}
