using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.ConsumptionAnalysis
{
    public class ConsumptionAnalysis_Model
    {
        public string Title { get; set; }
        public string ItemID { get; set; }
        public string From_dt { get; set; }
        public string To_dt { get; set; }
        public string Group { get; set; }
        public string CAFilter { get; set; }
        public string shflId { get; set; }
        public string opId { get; set; }
        public List<ShopFloor> ShopFloorList { get; set; }
        public List<Operation> OperationList { get; set; }
    }
    public class ShopFloor
    {
        public string shfl_id { get; set; }
        public string shfl_name { get; set; }
    }
    public class Operation
    {
        public string op_id { get; set; }
        public string op_name { get; set; }
    }
}
