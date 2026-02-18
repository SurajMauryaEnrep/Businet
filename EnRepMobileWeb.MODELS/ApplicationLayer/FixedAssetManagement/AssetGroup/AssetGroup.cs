using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetGroup
{
    public class AssetGroupModel
    {
        public string Delete_Dependcy { get; set; }
        public string AppStatus { get; set; }
        public string GroupID { get; set; }
        public string ChkPgroup { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string Title { get; set; }
        public string Ddlcoa_name { get; set; }
        public string TransType { get; set; }
        public int Comp_id { get; set; }
        public int? Asset_grp_id { get; set; }
        public string Asset_group_name { get; set; }
        public string Asset_grp_struc { get; set; }
        public string Asset_grp_par_id { get; set; }
        public string Dep_method { get; set; }
        public string Dep_freq { get; set; }
        public string Dep_per { get; set; }
        public int? Dep_coa { get; set; }
        public int? Asset_coa { get; set; }
        public int? Assetcat_coa { get; set; }
        public int? Create_id { get; set; }
        public int? Mod_id { get; set; }
        public string FormMode { get; set; }
        public string DeleteCommand { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string Onetimeclick { get; set; }
        public string AmmendedOn { get; set; }
        public string ChkChildGroup { get; set; }
        public List<AccountGroup_AG> ParentItemGroup { get; set; }
        public List<AssetsCOA_AG> AssetsCOAList { get; set; }
        public List<AssetsCategoryCOA_AG> AssetsCategoryCOAList { get; set; }
        public List<ExpenseCOA> ExpenseCOAList { get; set; }
        public List<DepMethod> DepMethodList { get; set; }
    }

    public class AccountGroup_AG
    {
        public string item_grp_struc { get; set; }
        public string par_grp_name { get; set; }
    }
    public class AssetsCOA_AG
    {
        public string coa_id { get; set; }
        public string coa_name { get; set; }
    }
    public class AssetsCategoryCOA_AG
    {
        public string coa_id { get; set; }
        public string coa_name { get; set; }
    }
    public class Header_AG
    {
        public ParentNode_AG TreeStr { get; set; }
    }
    public class ParentNode_AG
    {
        public string label { get; set; }
        public string value { get; set; }
        public List<childrenNode_AG> children { get; set; }

    }
    public class childrenNode_AG
    {
        public string label { get; set; }
        public string value { get; set; }
        public List<SubchildrenNode_AG> children { get; set; }
    }
    public class SubchildrenNode_AG
    {
        public string label { get; set; }
        public string value { get; set; }
        public List<SubSubchildrenNode_AG> children { get; set; }
    }
    public class SubSubchildrenNode_AG
    {
        public string label { get; set; }
        public string value { get; set; }
        public List<SubSubSubchildrenNode_AG> children { get; set; }
    }
    public class SubSubSubchildrenNode_AG
    {
        public string label { get; set; }
        public string value { get; set; }
    }
    public class MenuSearchModel_AG
    {
        public string search_menu { get; set; }
        public string Comp_ID { get; set; }
    }
    public class ItemMenuSearchModel_AG
    {
        public string search_tree_menu { get; set; }
        public string Comp_ID { get; set; }
    }
    public class AccMenuSearchModel_AG
    {
        public string search_tree_menu { get; set; }
        public string Comp_ID { get; set; }
    }

    public class FeatureMenuSearchModel_AG
    {
        public string search_tree_menu { get; set; }
        public string Comp_ID { get; set; }
    }
    public class SearchItemBOL_AG
    {
        public string ddlGroup { get; set; }
        public IEnumerable<SelectListItem> Group { get; set; }
        public string ddlItem { get; set; }
        public string ddlGrp { get; set; }
        public string ddlattrID { get; set; }
        public string EditItemCode { get; set; }
        public string CurrentUser { get; set; }
        public string CurrentDT { get; set; }
    }
    public class ExpenseCOA
    {
        public string coa_id { get; set; }
        public string coa_name { get; set; }
    }
    public class DepMethod
    {
        public string depmed_id { get; set; }
        public string depmed_name { get; set; }
    }
}
