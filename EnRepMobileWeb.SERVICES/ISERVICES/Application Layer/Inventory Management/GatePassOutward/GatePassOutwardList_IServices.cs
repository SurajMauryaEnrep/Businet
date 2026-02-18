using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.GatePassOutward
{
    public interface GatePassOutwardList_IServices
    {
        DataSet GetAllDropDownList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId,
            string FromDate, string ToDate);
        DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType);
        DataTable SearchDataFilter(string Entity_type, string Entity_id , string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId);
    }
}
