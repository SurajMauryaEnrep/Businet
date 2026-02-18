using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Dashboard_ISERVICE
{
    public interface Dashboard_ISERVICE
    {
        DataSet GetDashboardData(string CompId, string BrID, string Dateflag, string Fromdt, string Todt, string Top, string Charttype, string UserID, string Language, string Flag);
        DataSet GetPendingDocument(string Comp_ID, string Br_ID, string User_ID, string Lang);
        DataSet Updateavlvalue(string UserID,string value);
    }
}
