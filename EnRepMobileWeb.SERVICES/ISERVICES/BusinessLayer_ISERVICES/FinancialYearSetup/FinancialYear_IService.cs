using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.FinancialYearSetup
{
    public interface FinancialYear_IService
    {
        DataTable GetFY_List(string CompID);
        DataTable GetPN_FYdetail(string CompID, string Flag);
        DataSet Insert_FyClosingDetail(string CompID, string br_id, string pfy_sdt, string pfy_edt, string nfy_sdt, string nfy_edt, bool bk_close, string transtype);


    }
}
