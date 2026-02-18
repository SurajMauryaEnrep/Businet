using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.GeneralLedger;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.GeneralLedger
{
    public interface GeneralLedger_ISERVICE
    {
        Dictionary<string, string> GLSetupGroupDAL(string GroupName, string CompID);
        Dictionary<string, string> AccGrpListGroupDAL(string GroupName, string CompID);
        DataTable GetCurrList(string CompID, string Supptype);
        DataSet Get_FYList(string Compid, string Brid);
        DataSet GetGernalLedgerDetails(string comp_id, string br_id, string acc_id, string acc_group, string acc_type, string curr, string Fromdate, string Todate,string Rpt_As, string brlist);
        DataSet GetGernalLedgerDetails(Search_Parmeters model1, int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir);
    }
}
