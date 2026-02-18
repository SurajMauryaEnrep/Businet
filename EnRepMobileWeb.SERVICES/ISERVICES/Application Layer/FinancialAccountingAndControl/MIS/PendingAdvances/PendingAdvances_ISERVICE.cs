using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.PendingAdvances
{
   public interface PendingAdvances_ISERVICE
    {
        String InsertUserRangeDetail(string CompID, string user_id, string range1, string range2, string range3, string range4, string range5);
        DataSet GetUserRangeDetail(string CompID, string UserID);
        DataSet GetPendingAdvancesList(string CompId, string BranchID, string UserID, string EntityType, string AsDate, string ReportType);
        DataTable GetPendingAdvancesAgingList(string CompId, string BranchID,string Acc_ID, string lrange, string urange, string EntityType, string AsDate, int CurrId, string ReportType);
    }
}
