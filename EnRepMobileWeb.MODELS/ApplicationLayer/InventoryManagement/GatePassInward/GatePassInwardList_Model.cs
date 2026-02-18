using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.GatePassInward
{
    public class GatePassInwardList_Model
    {
        public string GpassSearch { get; set; }
        public string Source_Type { get; set; }
        public string SourceType { get; set; }
        public string EntityType { get; set; }
        public string entity_type { get; set; }
        public string EntityID { get; set; }
        public string EntityName { get; set; }
        public string ListFilterData { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Title { get; set; }
        public string WF_Status { get; set; }
        public string Status { get; set; }
        public List<Status> StatusList { get; set; }
        public List<EntityNameList> EntityNameList { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class EntityNameList
    {
        public string entity_id { get; set; }
        public string entity_name { get; set; }
    }
}
