using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockDetailConsolidated
{
    public class StockDetailConsolidated_Model
	{
		public string Title { get; set; }
        public string ItemId { get; set; }
        public string ItemGroupId { get; set; }
        public string AsOnDate { get; set; }
		public List<ItemListModel> ItemsList { get; set; }
		public List<ItemGroupListModel> ItemsGroupList { get; set; }
        public string SearchStatus { get; set; }
        public string Filters { get; set; }
    }
	public class ItemListModel
	{
		public string Item_Id { get; set; }
		public string Item_Name { get; set; }
	}
	public class ItemGroupListModel
	{
		public string Group_Id { get; set; }
		public string Group_Name { get; set; }
	}
	public class StockDetailsConsolidatedList
	{
		public int SrNo { get; set; }
		public string compid { get; set; }
		public string brid { get; set; }
		public string itemid { get; set; }
		public string itemname { get; set; }
		public string sub_item { get; set; }
		public string hsncode { get; set; }
		public string uom { get; set; }
		public string uomid { get; set; }
		public string itemgrpid { get; set; }
		public string itemgroup { get; set; }
		public string WhAvlStk { get; set; }
		public string WhReservedStk { get; set; }
		public string WhRejectStk { get; set; }
		public string WhReworkStk { get; set; }
		public string WhTotalStk { get; set; }
		public string WhTotalStkVal { get; set; }
		public string ShflAvlStk { get; set; }
		public string ShflRejectStk { get; set; }
		public string ShflReworkStk { get; set; }
		public string WIPStock { get; set; }
		public string ShflTotalStk { get; set; }
		public string ShflTotalStkVal { get; set; }
		public string TotalAvlStk { get; set; }
		public string TotalReservedStk { get; set; }
		public string TotalRejectStk { get; set; }
		public string TotalReworkStk { get; set; }
		public string TotalStk { get; set; }
		public string TotalStkVal { get; set; }
	}

}
