using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.WorkstationSetup
{
   public interface WorkstationSetup_ISERVICES
    {//
        DataTable GetShopFloorDetailsDAL(Int32 comp_id, Int32 br_id);
        Dictionary<string, string> GetGroupList(string CompID, string GroupName);
        DataTable GetWSDetailsDAL(Int32 comp_id, Int32 br_id, int shfl_id, string action);
        DataSet GetWSDoubleClickEdit(Int32 comp_id, Int32 br_id, Int32 ws_id);
        string insertWorkStationDetail(int comp_id, int br_id, int ws_id, string ws_name, int shfl_id, string op_st_date, string op_name, 
            string sr_no, int user_id, string TransType, string Make, string Model_no, string Grp_nm,string mac_id,DataTable WRKSAttachments,DataTable WorkstationCapacity);
    }
}
