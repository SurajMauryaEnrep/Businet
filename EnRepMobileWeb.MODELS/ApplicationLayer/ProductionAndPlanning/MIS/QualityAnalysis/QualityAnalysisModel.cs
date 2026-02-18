using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MIS.QualityAnalysis
{
    public class QualityAnalysisModel
    {
        public string Title { get; set; }
        public string QcType { get; set; }
        public string ProductName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ShowAs { get; set; }
        public string QAFilter { get; set; }
        public string DocumentID { get; set; }
        
        public List<ItemsDetailsModel> ItemsList { get; set; }
    }
    public class ItemsDetailsModel
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
    }
}
