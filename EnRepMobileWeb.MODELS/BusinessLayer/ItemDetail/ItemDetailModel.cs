using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EnRepMobileWeb.MODELS.BusinessLayer.ItemDetail
{
    public class ItemDetailModel
    {
		public DataTable CustomerBranchList { get; set; }
		public DataTable AttachMentDetailItmStp { get; set; }
		public string ItemListFilter { get; set; }
		//public string ddl_sal_ret_coa { get; set; }
		
		public string dependcy_i_capg { get; set; }
		public string AutoGen_catalogNoParameter { get; set; }
		public string AutoGen_Ref_noParameter { get; set; }
		public string ItemNameDependcy { get; set; }
		public string itemname1 { get; set; }
		public string Savebtn { get; set; }
		public string Guid { get; set; }
		public string AppStatus { get; set; }
		public string ItemCode { get; set; }
		public string IL_SSearch { get; set; }
		public string ILSearch { get; set; }
		public string BtnName { get; set; }
		public string i_packdetail { get; set; }
		public string Message { get; set; }
		public string Command { get; set; }
		public string Title { get; set; }
		public string DocumentMenuId { get; set; }
		public static string ValueRequired { get; set; }
		public string ddlgroup_name { get; set; }
		public string ddlhsncode { get; set; }
		 public string ddlcoa_name { get; set; }
		public Boolean ApprovedFlag { get; set; }
		public string TransType { get; set; }
		public string item_id { get; set; }
		public string item_name { get; set; }
		public string item_oem_no { get; set; }
		public string item_sam_cd { get; set; }
		public string item_sam_des { get; set; }

		public string TechSpec { get; set; }
		public string CatlNo { get; set; }
		public string CatlNoPrefix { get; set; }
		
		public string SalesAlias { get; set; }
		public string PurchaseAlias { get; set; }
		public string item_leg_cd { get; set; }
		public string item_leg_des { get; set; }
		public string Item_group_name { get; set; }
		public string item_grp_id { get; set; }
		public string base_uom_id { get; set; }
		public string pur_uom_id { get; set; }
		public string sl_uom_id { get; set; }
		public string cost_price { get; set; }
		public string sale_price { get; set; }
		public int item_prf_id { get; set; }
		public string issue_method { get; set; }
		public string cost_method { get; set; }
		public string min_stk_lvl { get; set; }
		public string min_pr_stk { get; set; }
		public string re_ord_lvl { get; set; }
		public Boolean stkout_warn { get; set; }
		public string item_remarks { get; set; }
		public Boolean act_status { get; set; }
		public string inact_reason { get; set; }
		public int? wh_id { get; set; }
		public int? bin_id { get; set; }
		public string HSN_code { get; set; }
		public string Hsn_id { get; set; }
        public Boolean i_sls { get; set; }
        public Boolean i_pur { get; set; }
        public Boolean i_wip { get; set; }
        public Boolean i_capg { get; set; }
        public Boolean i_stk { get; set; }
        public Boolean i_qc { get; set; }
        public Boolean i_srvc { get; set; }
        public string i_Sample_dependcy { get; set; }
        public string i_batch_dependcy { get; set; }
        public string i_Ser_dependcy { get; set; }
        public string i_cons_dependcy { get; set; }
        public Boolean i_cons { get; set; }
        public Boolean i_serial { get; set; }
        public Boolean i_Sam { get; set; }
        public Boolean i_batch { get; set; }
        public Boolean i_exp { get; set; }
		public Boolean i_pack { get; set; }
		public Boolean i_catalog { get; set; }
		public Boolean i_ws { get; set; }
		public Boolean i_exempted { get; set; }
		public Boolean SubItem { get; set; }
		public int? create_id { get; set; }
		public string create_dt { get; set; }
		public string ListFilterData1 { get; set; }
		public int? app_id { get; set; }
		public string app_dt { get; set; }
		public string app_status { get; set; }
		public int loc_sls_coa { get; set; }
		public int exp_sls_coa { get; set; }
		public int loc_pur_coa { get; set; }
		public int imp_pur_coa { get; set; }
		public int stk_coa { get; set; }
		public int sal_ret_coa { get; set; }
		public int Disc_coa { get; set; }
		public int pur_ret_coa { get; set; }
		public int prov_pay_coa { get; set; }
		public int cogs_coa { get; set; }
		public int stk_adj_coa { get; set; }
		public int dep_coa { get; set; }
		public int InterBranch_pur_coa { get; set; }
		public int InterBranch_sls_coa { get; set; }
		public int asset_coa { get; set; }
		public string wght_kg { get; set; }
		public string wght_ltr { get; set; }
		public string gr_wght { get; set; }
		public string nt_wght { get; set; }
		public string item_hgt { get; set; }
		public string item_wdh { get; set; }
		public string item_len { get; set; }
		public string item_pack_sz { get; set; }

		public int pack_uom { get; set; }
		public string pack_length { get; set; }
		public string pack_width { get; set; }
		public string pack_height { get; set; }
		public string pack_cbm { get; set; }
		

		//stp$item$org$detail table
		public string comp_id { get; set; }

		//stp$item$image
		public string item_img_name { get; set; }
		public string item_img_path { get; set; }

		//stp$item$attr$detail
		public string item_att_id { get; set; }
		public string item_att_val { get; set; }
		public string D_InActive { get; set; }
		public string SubItemName { get; set; }
		public string HdnSubItemName { get; set; }
		public string HdnEdit { get; set; }

		//stp$item$alt$detail
		public string alt_item_id { get; set; }

		//------------------------------
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string CompId { get; set; }

		public string UserMacaddress { get; set; }
		public string UserSystemName { get; set; }

		public string UserIP { get; set; }
		public string creat_dt { get; set; }
		public string mod_dt { get; set; }


		public string CreatedBy { get; set; }
		public string CreatedOn { get; set; }
		public string ApprovedBy { get; set; }

		public string ApprovedOn { get; set; }
		public string AmmendedBy { get; set; }
		public string AmmendedOn { get; set; }
		public string Status { get; set; }
		public string DeleteCommand { get; set; }
		public string ItemAttrDetail { get; set; }
		public string ItemCustomerDetail { get; set; }
		public string ItemSupplierDetail { get; set; }
		public string ItemPortfolioDetail { get; set; }
		public string ItemVehicleDetail { get; set; }
		public string ItemSubItemDetail { get; set; }
		public string CustName { get; set; }
		public string CustID { get; set; }
		public string SuppName { get; set; }
		public string SuppID { get; set; }
		public string PortfName { get; set; }
		public string PrfID { get; set; }

		public string VehicleName { get; set; }

		//public List<ItemBranch> CustomerBranchList { get; set; }
		public string ItemBranchDetail { get; set; }

		public List<UOM> UOMList { get; set; }
		public List<warehouse> warehouseList { get; set; }
		public List<Bin> BinList { get; set; }
		public List<Portfolio> PortfolioList { get; set; }
		public List<Vehicle> VehicleList { get; set; }
		public List<GroupName> GroupList { get; set; }
		public List<AttrName> AttrList { get; set; }
		public List<AttrValName> AttrValList { get; set; }
		public List<HSNno> HSNList { get; set; }
		public List<AssetsCOA> AssetsCOAList { get; set; }
		public List<IncomeCOA> IncomeCOAList { get; set; }
		public List<LiabilityCOA> LiabilityCOAList { get; set; }
		public List<ExpenseCOA> ExpenseCOAList { get; set; }
		public List<Status> StatusList { get; set; }
		public string attatchmentdetail { get; set; }
		public string attatchmentDefaultdetail { get; set; }
		public string Reference_noflag { get; set; }
		public string ddl_ItemName { get; set; }
		public string ddl_ItemGroup { get; set; }
		public string ddlGroup { get; set; }
		public string ddlAttributeValue { get; set; }
		public string ddlItemPortfolio { get; set; }
		public string ImageFilter { get; set; }
		public string hdnitemActStatus { get; set; }
		public string item_ActStatus { get; set; }
		public string ddlAttributeName { get; set; }
		public string ListFilterData { get; set; }
		public List<ItemName> ItemNameList { get; set; }
		public List<GroupList> ItemGroupList { get; set; }
		public List<ItemPortfolio> ItemPortfolioList { get; set; }
		public List<AttributeName> AttributeNameList { get; set; }
		public List<AttributeValue> AttributeValueList { get; set; }
		public string ItemUsedInTrans { get; set; }
		public string ToDisableBaseUnit { get; set; }
		public int? ExpiryAlertDays { get; set; }
	}
	public class ItemName
	{
		public string Item_ID { get; set; }
		public string Item_Name { get; set; }
	}
	public class GroupList
	{
		public string ID { get; set; }
		public string Name { get; set; }
	}
	public class ItemPortfolio
	{
		public string ID { get; set; }
		public string Name { get; set; }
	}
	public class AttributeName
	{
		public string ID { get; set; }
		public string Name { get; set; }
	}
	public class AttributeValue
	{
		public string ID { get; set; }
		public string Name { get; set; }
	}
	public class ItemDetailsattch
	{
		public DataTable AttachMentDetailItmStp { get; set; }
		public string attatchmentdetail { get; set; }
		public string Guid { get; set; }
		//public string AttachMentDetailItmStp { get; set; }
	}
	public class Status
	{
		public string status_id { get; set; }
		public string status_name { get; set; }
	}

	//public class ItemBranch
	//{
	//	public string comp_id { get; set; }
	//	public string comp_nm { get; set; }
	//}
	public class UOM
	{
		public string uom_id { get; set; }
		public string uom_name { get; set; }
	}
	public class warehouse
	{
		public string wh_id { get; set; }
		public string wh_name { get; set; }
	}
	public class Bin
	{
		public string setup_id { get; set; }
		public string setup_val { get; set; }
	}
	public class Portfolio
	{
		public string setup_id { get; set; }
		public string setup_val { get; set; }
	}
	public class Vehicle
	{
		public string setup_id { get; set; }
		public string setup_val { get; set; }
	}
	public class GroupName
	{
		public string item_grp_id { get; set; }
		public string ItemGroupChildNood { get; set; }		
	}
	public class AttrName
	{
		public string attr_id { get; set; }
		public string attr_name { get; set; }
	}
	public class AttrValName
	{
		public string attr_val_id { get; set; }
		public string attr_val_name { get; set; }
	}
	public class HSNno
	{
		public string setup_val { get; set; }
		public string setup_id { get; set; }
	}
	public class AssetsCOA
	{
		public string coa_id { get; set; }
		public string coa_name { get; set; }
	}
	public class IncomeCOA
	{
		public string coa_id { get; set; }
		public string coa_name { get; set; }
	}
	public class LiabilityCOA
	{
		public string coa_id { get; set; }
		public string coa_name { get; set; }
	}
	public class ExpenseCOA
	{
		public string coa_id { get; set; }
		public string coa_name { get; set; }
	}
}
