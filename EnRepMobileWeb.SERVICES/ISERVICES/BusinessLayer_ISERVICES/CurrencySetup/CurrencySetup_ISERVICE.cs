using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.CurrencySetup;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.CurrencySetup
{
    public interface CurrencySetup_ISERVICE 
    {
        DataSet GetCurrancyTable(string comp_id);
        DataSet GetAllData(string comp_id,string br_id);
        DataTable GetCurrencyList(string comp_id, string br_id);
        string InsertCurr(string CompID, int currencyid, string Price, string Date, string transtype);
        string DeleteCurrDetail(string CompID, int currencyid, string Date , string transtype);
    }
}
