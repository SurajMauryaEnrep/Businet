using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.StockValuation
{
    public interface StockValuation_ISERVICES
    {
        DataTable BindGetGroupList(string GroupName, string CompID);
        DataSet BindFinancialYearMonths(int CompID, int BrID, string fin_sfy);
        DataTable GetFinYearList(string CompID, string brId);
        DataSet GetStkValDetailsMIS(string CompID, string BrID, string ItmGrpID,string ReportType, string costbase
            , string inc_shfl, string inc_zero,string acc_id, string from_dt, string to_dt, string ftype,string sp_uom_id,string priceList);
        DataSet Get_FYList(string Compid, string Brid);
        DataSet Get_rcptIssueDetail(string CompID, string BrID, string id, string flag, string rpt_type, string shfl_flag, string from_dt, string to_dt, string cost_type,string priceList);
        DataSet Get_priceList(string comp_ID, string br_ID, string searchValue, string fromDt, string toDt);
    }
}
