using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.NCR
{
    public class NCR_Model
    {
        public string Title { get; set; }
        public string FromDt { get; set; }
        public string Todt { get; set; }
        public string SrcDoc { get; set; }
        public string Status { get; set; }
        public string hdnCommand { get; set; }
        public string MenuDocumentId { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public List<StatusList> statusList { get; set; }
        public List<SrcDocList> srcDocLists { get; set; }
    }
    public class StatusList
    {
        public string id { get; set; }
        public string text { get; set; }
    }
    public class SrcDocList
    {
        public string SrcDocId { get; set; }
        public string SrcDocVal { get; set; }
    }
    public class AckListDataModel
    {
        public string item_id { get; set; }
        public string uom_id { get; set; }
        public string src_type { get; set; }
        public string doc_no { get; set; }
        public string doc_dt { get; set; }
        public string entity_id { get; set; }
        public string ack_by { get; set; }
        public string ack_dt { get; set; }
        public string ack_taken { get; set; }
        public string remarks { get; set; }
    }
}
