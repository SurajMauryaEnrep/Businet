using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockDetailShopfloor
{
	public class StockDetailShopfloor_Model
	{
		public string Title { get; set; }
		public string ItemName { get; set; }
		public string HsnCode { get; set; }
		public string ItemID { get; set; }
		public string GroupName { get; set; }
		public string GroupID { get; set; }
		public string PortfolioName { get; set; }		
		public string PrfID { get; set; }
		public string BranchName { get; set; }
		public string ShopfloorName { get; set; }
		public string UptoDate { get; set; }
		public string StockBy { get; set; }
		public string StockByFilter { get; set; }
		public List<ItemName> ItemNameList { get; set; }
		public List<ItemGroupName> ItemGroupNameList { get; set; }

		public List<ItemPortfolio> ItemPortfolioList { get; set; }
		public List<Branch> BranchList { get; set; }
		public List<Shopfloor> ShopfloorList { get; set; }
		public List<StockListDetailShopfloor> StockDetailShopfloorList { get; set; }
		public List<HsnCodeModel> HsnCodeList { get; set; }
		public string Filters { get; set; }

	}
	public class ItemName
	{
		public string Item_Id { get; set; }
		public string Item_Name { get; set; }
	}
	public class ItemGroupName
	{
		public string Group_Id { get; set; }
		public string Group_Name { get; set; }
	}

	public class ItemPortfolio
	{
		public string Prf_Id { get; set; }
		public string Prf_Name { get; set; }
	}
	public class Branch
	{
		public string Br_Id { get; set; }
		public string Br_Name { get; set; }
	}
	public class Shopfloor
	{
		public string shfl_Id { get; set; }
		public string shfl_Name { get; set; }
	}
	public class HsnCodeModel
	{
		public string hsn_code { get; set; }
		public string hsn_desc { get; set; }
	}
	public class StockListDetailShopfloor
	{
		public int SrNo { get; set; }
		public string ItemName { get; set; }
		public string HsnCode { get; set; }
		public string subItem { get; set; }
		public string ItemID { get; set; }
		public string UOM { get; set; }
		public string Branch { get; set; }
		public string BranchID { get; set; }
		public string Shopfloor { get; set; }
		public string ShopfloorID { get; set; }
		public string Lot { get; set; }
		public string Batch { get; set; }
		public string Serial { get; set; }
		public string OpeningStock { get; set; }
		public string Receipts { get; set; }
		public string Issued { get; set; }
		public string ReservedStock { get; set; }
		public string RejectedStock { get; set; }
		public string ReworkableStock { get; set; }
		public string WIPStock { get; set; }
		public string TotalStock { get; set; }
		public string TotalStockVal { get; set; }
		public string AvailableStock { get; set; }
		public string StockValue { get; set; }
		public string sub_item_id { get; set; }
		public string sub_item_name { get; set; }
		
	}
}
