using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BudgetAnalysis
{
    public interface BudgerAnalysis_IService
    {
        DataTable GetFinancialYearsList(string compId, string brId);
        DataTable GetFyMonthOrQuarterList(string action, string compId, string brId, string budgetType, string finYear, string qtrName);
        DataSet GetBudgerAnalysisReport(string action,string compId, string brId, string glAccId, string finYear,
            string quarter, string month);
        DataSet GetBudgetAllocationReport(string action, string compId, string brId, string glAccId, string finYear, string qtr, string mnth, int rev_no);
        DataTable GetfyLedgerDetails(string compId, string brId, string accId, string fromDate, string toDate);
        DataTable GetfyLedgerDetailsCostCentr(string compId, string brId, string accId, string fromDate, string toDate, int CCtypeId, int CCNameId);
    }
}
