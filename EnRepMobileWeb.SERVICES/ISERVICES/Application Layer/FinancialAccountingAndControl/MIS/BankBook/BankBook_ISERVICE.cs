using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.BankBook;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BankBook
{
    public interface BankBook_ISERVICE
    {
        DataTable GetCurrList(string CompID, string Supptype);
        DataSet Get_FYList(string Compid, string Brid);
        Dictionary<string, string> BB_AccList(string CompID, string BrID,string AccName);
        DataSet GetBankBookDetails(string comp_id, string br_id, string acc_id, string curr_id, string Fromdate, string Todate);
        DataSet GetBankBookDetails(Search_Parmeters model, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir);/*Added By Suraj Maurya on 20-06-2025*/

    }
}
