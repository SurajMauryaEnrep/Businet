using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.FinanceBudget
{
    public class FinanceBudgetModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string BudgetAmount { get; set; }
        public string Period { get; set; }
        public string FinYr { get; set; }
        public string FinYears { get; set; }
        public string SFinYears { get; set; }
        public string EFinYears { get; set; }
        public string Glacc { get; set; }
        public string DisableFlag { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Createdon { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string DeleteCommand { get; set; }
        public string ListFilterData1 { get; set; }
        public List<Finyear> Finyrlist { get; set; }
        public List<GlAccounts> Glacclist { get; set; }
        public List<monthlist> Monthlists { get; set; }
        public List<ListAll> ListAlls { get; set; }
        public string CC_DetailList { get; set; }
        public string Fin_Gldetails { get; set; }
        public string FinBudQuatMonList { get; set; }
        public string TotalAmount { get; set; }
        public string Acc_Id { get; set; }
        public string WF_Status1 { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string DocumentStatus { get; set; }
        public string BgtStatus { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string PeriodFlag { get; set; }
        public string Revno { get; set; }
        public string FinBudDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string HdnCsvPrint { get; set; }

    }
    public class UrlModel
    {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string VN { get; set; }
        public string wf { get; set; }
        public string Fy { get; set; }
        public string Rvn { get; set; }
        public string Per { get; set; }

    }
    public class monthlist
    {
        public string month { get; set; }
        public string monPeriod { get; set; }
        public string monAmount { get; set; }
        public string montype { get; set; }
        public int Sqn { get; set; }
        public int qtr_sno { get; set; }

    }
    
    public class ListAll
    {
        public string allmonths { get; set; }
        public string  allperiod{ get; set; }
        public string allamount { get; set; }
        public string alltype { get; set; }
        public int Sqno { get; set; }
        public int qtr_sno { get; set; }

    }
    public class  Finyear
    {
        public string FinyrId { get; set; }
        public string Finyrs { get; set; }
    }
    public class GlAccounts
    {
        public string GlaccId { get; set; }
        public string Glaccname { get; set; }
    }
}
