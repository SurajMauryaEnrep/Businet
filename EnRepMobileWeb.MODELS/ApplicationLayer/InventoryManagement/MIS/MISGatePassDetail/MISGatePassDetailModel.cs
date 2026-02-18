using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISGatePassDetail
{
    public class MISGatePassDetailModel
    {
        public string GatePassSearch { get; set; }
        public string Title { get; set; }
        public string entity_type { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string EntityID { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public string GatePassType { get; set; }
        public string GatePass_Type { get; set; }
        public List<EntityNameList1> EntityNameList { get; set; }
    }
    public class EntityNameList1
    {
        public string entity_id { get; set; }
        public string entity_name { get; set; }
    }
}
