using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.MISStockReservation
{
    public class MISStockReservation_Model
    {
        public string title { get; set; }
        public string ItemId { get; set; }
        public string ItemGroupId { get; set; }
        public string WarehouseId { get; set; }
        public string SearchStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string DocumentMenuId { get; set; }
        public List<ItemsModel> ItemsList { get; set; }
        public List<ItemGroupModel> ItemsGroupList { get; set; }
        public List<WarehouseModel> WarehouseList { get; set; }
    }
	public class ItemsModel
	{
		public string Item_Id { get; set; }
		public string Item_Name { get; set; }
	}
	public class ItemGroupModel
	{
		public string Group_Id { get; set; }
		public string Group_Name { get; set; }
	}
	public class WarehouseModel
	{
		public string Wh_Id { get; set; }
		public string Wh_Name { get; set; }
	}
}
