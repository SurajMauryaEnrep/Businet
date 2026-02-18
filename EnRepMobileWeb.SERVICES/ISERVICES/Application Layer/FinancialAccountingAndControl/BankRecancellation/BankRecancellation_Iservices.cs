using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.BankRecancellation
{
    public interface  BankRecancellation_Iservices
    {
        DataSet GetBankLists(string Comp_Id);

        DataSet GetBankCurr(string CompID,string Br_Id, string BankId,string FromDate,string ToDate);

        DataSet GetFinYearDates(string CompID, string BrchID);

        DataSet GetFyToDate(string CompID, string BrchID, string ToDate,string FromDate,string Year);

        DataSet GetSearchedData(string CompID, string BrchID, string acc_id, string FromDate, string ToDate, string TransType,string Status,string searchValue);
        string InsertUpdateBankreco(DataTable BanrecoList);
        DataSet CheckBeforeInsertUpdateBankreco(DataTable BanrecoList);
        DataSet CheckAdvancePayment(string CompId, string BrchID, string DocNo, string DocDate);
    }
}
