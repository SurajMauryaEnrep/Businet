using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISDeliveryNoteDetail
{
   public class MISDeliveryNoteDetail_Model
    {
        public string Title { get; set; }
        public string ShowAs { get; set; }
        public string Fromdate { get; set; }
        public string ToDate { get; set; }
        public string SuppId { get; set; }
        public string ItemId { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
        public List<SupplierModel> SuppliersList { get; set; }
        public string SearchStatus { get; set; }
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
}
