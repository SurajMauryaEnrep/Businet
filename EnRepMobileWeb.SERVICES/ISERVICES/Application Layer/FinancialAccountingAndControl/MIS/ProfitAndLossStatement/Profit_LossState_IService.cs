
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.ProfitAndLossStatement
{
    public interface Profit_LossState_IService
    {
        DataSet Get_PL_Statement(string comp_id, string br_id, string br_list, string Fromdate, string Todate, string rpt_type);
        DataSet Get_FYList(string Compid, string Brid);
    }
}
