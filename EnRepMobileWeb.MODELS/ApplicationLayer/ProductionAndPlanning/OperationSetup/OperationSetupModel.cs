using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.OperationSetup
{
   public class OperationSetupModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string Supervisor { get; set; }
        public string Workstation { get; set; }
        public string Shopfloor { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string Title { get; set; }
        public int comp_id { get; set; }
        public int op_id { get; set; }
        public string op_name { get; set; }
        public string op_type { get; set; }
        public List<OpType> OpTypeList { get; set; }
        public string op_remarks { get; set; }
        public int create_id { get; set; }
        public string TransType { get; set; }
        public string DeleteCommand { get; set; }
        public List<ShopFloor> ShopFloorList { get; set; }
        public List<Workstation> WorkstationList { get; set; }
        public List<Supervisor> SupervisorList { get; set; }

    }
    public class Supervisor
    {
        public string super_id { get; set; }
        public string super_name { get; set; }
    }public class Workstation
    {
        public string wrk_id { get; set; }
        public string wrk_name { get; set; }
    }
    public class ShopFloor
    {
        public string shfl_id { get; set; }
        public string shfl_name { get; set; }
    }
    public class OpType
    {
        public string op_id { get; set; }
        public string op_val { get; set; }
    }
}
