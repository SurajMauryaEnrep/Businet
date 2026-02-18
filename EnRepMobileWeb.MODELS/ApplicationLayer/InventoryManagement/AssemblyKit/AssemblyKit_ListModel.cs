using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.Assembly_Kit
{
    public class AssemblyKit_ListModel
    {
        public string DocumentMenuId { get; set; }
        public string Search { get; set; }
        public string WF_Status { get; set; }
        public string ListFilterData { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string AssemblyProduct { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public List<ItemName_List> ItemNameList { get; set; }
        public List<Status> StatusList { get; set; }
    }
    public class ItemName_List
    {
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
}
