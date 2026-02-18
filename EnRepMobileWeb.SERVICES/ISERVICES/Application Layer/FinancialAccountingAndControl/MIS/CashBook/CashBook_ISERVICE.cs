using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.CashBook;
using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.CashBook
{
    public interface CashBook_ISERVICE
    {
        Dictionary<string, string> GetCashBookAcc_List(string Comp_Id, string Br_Id, string AccName);
        DataSet GetCashBookDetails(string comp_id, string br_id, string acc_id, string curr_id, string Fromdate, string Todate);
        DataSet GetCashBookDetails(Search_Parmeters model, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir);/* Added by Suraj Maurya 19-06-2025 */
        DataTable GetCurrList(string CompID, string Supptype);
        DataSet Get_FYList(string Compid, string Brid);

    }
}
