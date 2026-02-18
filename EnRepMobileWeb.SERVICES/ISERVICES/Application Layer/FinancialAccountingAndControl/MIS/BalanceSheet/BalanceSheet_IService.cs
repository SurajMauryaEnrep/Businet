using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.BalanceSheet
{
    public interface BalanceSheet_IService
    {
        DataSet Get_BalSheet_Detail(string comp_id, string br_id, string br_list, string Fromdate, string Todate, string rpt_type);
    }
}
