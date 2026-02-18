using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.TrialBalance
{
    public interface TrialBalance_ISERVICE
    {
        Dictionary<string, string> GLSetupGroupDAL(string GroupName, string CompID);
        Dictionary<string, string> AccGrpListGroupDAL(string GroupName, string CompID);
        DataSet GetAllBrchList(string CompID, string User_id);
        DataSet GetBalanceDetailList(string CompId, string BranchID, string UserID, string IncludeZeroStockFlag, string BalanceBy, string AccId, string AccGrpId, string AccType, string RptType, string Branch, string UptoDate, string curr_filter);
        DataSet GetTrialBalHisList(string CompId, string BranchID, string AccId, string Type, string UptoDate, string BalType, string CurrType);


    }
}
