using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockDetail
{
	public class StockDetail_Model
	{
		public string Title { get; set; }
		public string ItemName { get; set; }
		public string ItemID { get; set; }
		public string GroupName { get; set; }
		public string GroupID { get; set; }
		public string PortfolioName { get; set; }		
		public string PrfID { get; set; }
		public string BranchName { get; set; }
		public string WarehouseName { get; set; }
		public string UptoDate { get; set; }
		public string StockBy { get; set; }
		public string StockByFilter { get; set; }
		public string SupplierName { get; set; }
		public string HsnCode { get; set; }
		public string ExpiredItm { get; set; }
		public string StockOutItm { get; set; }
		public string NearExpiryItm { get; set; }
		public List<ItemName> ItemNameList { get; set; }
		public List<ItemGroupName> ItemGroupNameList { get; set; }

		public List<ItemPortfolio> ItemPortfolioList { get; set; }
		public List<Branch> BranchList { get; set; }
		public List<WarehouseName> WarehouseNameList { get; set; }
		public List<StockListDetail> StockDetailList { get; set; }
        public List<HsnCodeModel> HsnCodeList { get; set; }
        public List<SuppNameList> SuppName_List { get; set; }
        public List<StockGlAccount> StockGlAccList { get; set; }
		public string stock_gl_acc_id { get; set; } = "0";
		public List<ItemAlias> ItemAliasList { get; set; } = new List<ItemAlias> { new ItemAlias { alias_id="",alias_name="---Select---"} };
		public string itemAlias { get; set; } = "";
	}
	public class HsnCodeModel
	{
		public string hsn_code { get; set; }
		public string hsn_desc { get; set; }
	}
	public class SuppNameList
	{
		public string Supp_id { get; set; }
		public string Supp_Name { get; set; }
	}
	public class StockGlAccount
	{
		public string acc_id { get; set; }
		public string acc_name { get; set; }
	}
	public class ItemAlias
	{
		public string alias_id { get; set; }
		public string alias_name { get; set; }
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
	public class WarehouseName
	{
		public string Wh_Id { get; set; }
		public string Wh_Name { get; set; }
	}
	public class StockListDetail
	{
		public string ItemName { get; set; }
		public string Hsn_Code { get; set; }
		public string SubItem { get; set; }
		public string ItemID { get; set; }
		public string UOM { get; set; }
		public string Branch { get; set; }
		public string BranchID { get; set; }
		public string Warehouse { get; set; }
		public string WarehouseID { get; set; }
		public string Lot { get; set; }
		public string Batch { get; set; }
		public string exp_dt { get; set; }
		public string Serial { get; set; }
		public string OpeningStock { get; set; }
		public string Receipts { get; set; }
		public string Issued { get; set; }
		public string ReservedStock { get; set; }
		public string RejectedStock { get; set; }
		public string ReworkableStock { get; set; }
		public string TotalStock { get; set; }
		public string TotalStockVal { get; set; }
		public string AvailableStock { get; set; }
		
		public string StockValue { get; set; }
		public string sub_item_id { get; set; }
		public string sub_item_name { get; set; }
		public string min_stk_lvl { get; set; }
		public string Saleable { get; set; }

	}
	public class StockDetailsDataModel
	{
		//public string ILSearch { get; set; }
		//public string IL_SSearch { get; set; }
		public Int64 SrNo { get; set; }
		public string compid { get; set; }
		public string brid {get;set;}
		public string brname { get; set; }
		public string itemid {get;set;}
		[DisplayName("Item Name")]
		public string itemname {get;set;}
		public string HSNCode {get;set;}
		public string sub_item {get;set;}
		public string uom {get;set;}
		public string uomname {get;set;}
		public string whid {get;set;}
		public string whname {get;set;}
		public string lotno { get; set; }
		public string batchno {get;set;}
		public string exp_dt { get;set;}
		public string serialno { get; set; } = "";
		public double opening {get;set;}
		public double receipts {get;set;}
		public double issued {get;set;}
		public double reserved {get;set;}
		public double unreserved {get;set;}
		public double rejected {get;set;}
		public double reworkabled {get;set;}
		public double totalstk { get;set;}
		public string totalstk_in_sp { get;set;}
		public double totalstkval {get;set;}
		public double avlstk {get;set;}
		public string avlstk_in_sp {get;set;}
		public double avlstkvalue {get;set;}
		public string sub_item_id { get; set; }
		public string sub_item_name { get; set; }
		public double min_stk_lvl { get; set; }
		public string Saleable { get; set; }
		public string mfg_name { get; set; } = "";
		public string mfg_mrp { get; set; } = "";
		public string mfg_date { get; set; } = "";
		public string supp_name { get; set; } = "";
	}

}
