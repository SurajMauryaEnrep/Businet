using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockTake
{
    public class StockTakeList_Model
    {
        public string Title { get; set; }
        public string Status { get; set; }       
        public string STKFromDate { get; set; }
        public string STKToDate { get; set; }
        public string FromDate { get; set; }
        public string ListFilterData { get; set; }
        public string ToDate { get; set; }       
        public string STKSearch { get; set; }       
        public string WF_status { get; set; }       
        public List<STKList> StockTakeList { get; set; }
        public List<Status> StatusList { get; set; }       
    }
        public class Status
        {
            public string status_id { get; set; }
            public string status_name { get; set; }
        }      
        public class STKList
    {
            public string STKNumber { get; set; }
            public string STKDate { get; set; }
            public string hdSTKDate { get; set; }    
            public string Warehouse { get; set; }
            public string STKStatus { get; set; }
            public string CreatedON { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string ApprovedOn { get; set; }

        }
   
}
