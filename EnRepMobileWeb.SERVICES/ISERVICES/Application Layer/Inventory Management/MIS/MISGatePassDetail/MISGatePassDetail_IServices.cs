using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISGatePassDetail
{
   public interface MISGatePassDetail_IServices
    {
        DataTable SearchDataFilter(string Source_type, string Entity_type, string Entity_id, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId);
        DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType);
    }
}
