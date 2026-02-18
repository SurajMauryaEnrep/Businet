using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher
{
    public class SalesVoucherList_Model
    {
        public string WF_Status { get; set; }
        public string VouSearch { get; set; }
        public string Title { get; set; }
        public string cust_id { get; set; }
        public string cust_name { get; set; }      
        public string Status { get; set; }
        public string VouFromDate { get; set; }
        public string VouToDate { get; set; }
        public string FromDate { get; set; }
        public string ListFilterData { get; set; }
        public DateTime ToDate { get; set; }
        public int? curr { get; set; }
        public List<VouList> VoucherList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<CustAccList> CustAccNameList { get; set; }
        public List<Custcurr> currList { get; set; }
    }    
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class Custcurr
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }

    }
    public class VouList
    {
        public string cust_name { get; set; }
        public string VouNumber { get; set; }
        public string VouDate { get; set; }
        public string hdVouDate { get; set; }         
        public string BillNo { get; set; }
        public string BillDt { get; set; }
        public string curr_logo { get; set; }
        public string AmountinSp { get; set; }
        public string AmountinBs { get; set; }
        public string VouStatus { get; set; }
        public string CreatedON { get; set; }
        public string ModifiedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }

    }
    public class CustAccList
    {
        public string cust_acc_id { get; set; }
        public string cust_acc_name { get; set; }
    }
}
