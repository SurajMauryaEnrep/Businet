using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.DailyProductionPlan
{
    public class DailyProductionPlan_Model
    {
        public string Title { get; set; }
        public string product_id { get; set; }
        public string DR_fromDt { get; set; }
        public string DR_ToDt { get; set; }
        public string OperationNameStatus { get; set; }
        public List<OperationName> OperationNameList { get; set; }
        public string Filters { get; set; }
        public string hdnCSVPrint { get; set; }
        public string searchValue { get; set; }
        
    }
    public class OperationName
    {
        public string op_id { get; set; }
        public string op_name { get; set; }
    }
}
