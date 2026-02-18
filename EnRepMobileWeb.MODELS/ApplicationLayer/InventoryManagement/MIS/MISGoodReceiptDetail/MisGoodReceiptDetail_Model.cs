using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISGoodReceiptDetail
{
    public class MisGoodReceiptDetail_Model
    {
        public string EntityID { get; set; }
        public string EntityName { get; set; }
        public List<EntityNameList1> EntityNameList { get; set; }
        public string entity_type { get; set; }
        public string EntityType { get; set; }
        public string ReceiptType { get; set; }
        public string MultiselectStatusHdn { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string ShowAs { get; set; }
        public string Fromdate { get; set; }
        public string ToDate { get; set; }
        public string SuppId { get; set; }
        public string ItemId { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
        public List<SupplierModel> SuppliersList { get; set; }
        public string SearchStatus { get; set; }
        public List<Status> StatusList { get; set; }
    }
    public class EntityNameList1
    {
        public string entity_id { get; set; }
        public string entity_name { get; set; }
    }
    public class ItemsModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
    }
    public class SupplierModel
    {
        public string SuppId { get; set; }
        public string SuppName { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
}
