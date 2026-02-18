using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.OperationSetup
{
   public interface OperationSetup_ISERVICES
    {
        DataSet BindShopFloore(string CompID, string BrID);
        DataSet GetWorkStationDAL(string comp_id, string br_id, int shfl_id);
        DataTable GetOperationDetailsDAL(Int32 comp_id);
        string insertOperationDetail(int comp_id, int op_id, string op_name, string op_type, string op_remarks, int create_id,string action
            , string shfl_id, string wrkstn_id, string supervisor);
    }
}
