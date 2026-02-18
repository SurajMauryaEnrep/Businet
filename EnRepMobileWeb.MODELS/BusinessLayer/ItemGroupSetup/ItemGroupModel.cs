using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using System.Web.WebPages.Html;
namespace EnRepMobileWeb.MODELS.BusinessLayer.ItemGroupSetup
{
   
    public class ItemGroupModel
    {
        public int? InterBranch_sls_coa { get; set; }
        public int? InterBranch_pur_coa { get; set; }
        public string Delete_Dependcy { get; set; }
        public string AppStatus { get; set; }
        public string GroupID { get; set; }
        public string ChkPgroup { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string Title { get; set; }
        public string ddlcoa_name { get; set; }
        public string TransType { get; set; }
        public int comp_id { get; set; }
        public int? item_grp_id { get; set; }
        public string item_group_name { get; set; }
        public string item_grp_struc { get; set; }
        public string item_grp_par_id { get; set; }
        public string issue_method { get; set; }
        public string cost_method { get; set; }
        public string It_Remarks { get; set; }
        public Boolean i_sls { get; set; }
        public Boolean i_pur { get; set; }
        public Boolean i_wip { get; set; }
        public Boolean i_capg { get; set; }
        public Boolean i_stk { get; set; }
        public Boolean i_qc { get; set; }
        public Boolean i_srvc { get; set; }
        public Boolean i_cons { get; set; }
        public Boolean i_serial { get; set; }
        public Boolean i_sam { get; set; }
        public Boolean i_batch { get; set; }
        public Boolean i_exp { get; set; }
        public Boolean i_pack { get; set; }
        public Boolean i_catalog { get; set; }
        public string act_stat { get; set; }
        public int? loc_sls_coa { get; set; }
        public int? exp_sls_coa { get; set; }
        public int? loc_pur_coa { get; set; }
        public int? imp_pur_coa { get; set; }
        public int? stk_coa { get; set; }
        public int? sal_ret_coa { get; set; }
        public int? disc_coa { get; set; }
        public int? pur_ret_coa { get; set; }
        public int? prov_pay_coa { get; set; }
        public int? cogs_coa { get; set; }
        public int? stk_adj_coa { get; set; }
        public int? dep_coa { get; set; }
        public int? asset_coa { get; set; }
        public int? create_id { get; set; }
        public int? mod_id { get; set; }
        public string FormMode { get; set; }
        public string DeleteCommand { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string onetimeclick { get; set; }
        public string AmmendedOn { get; set; }

        public List<AccountGroup> ParentItemGroup { get; set; }
        public List<AssetsCOA> AssetsCOAList { get; set; }
        public List<IncomeCOA> IncomeCOAList { get; set; }
        public List<LiabilityCOA> LiabilityCOAList { get; set; }
        public List<ExpenseCOA> ExpenseCOAList { get; set; }
    }
    public class AccountGroup
    {
        public string item_grp_struc { get; set; }
        public string par_grp_name { get; set; }

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
    public class Header
    {
        public ParentNode TreeStr { get; set; }
    }
    public class ParentNode
    {

        public string label { get; set; }
        public string value { get; set; }
        public List<childrenNode> children { get; set; }

    }
    public class childrenNode
    {

        public string label { get; set; }
        public string value { get; set; }
        public List<SubchildrenNode> children { get; set; }
    }
    public class SubchildrenNode
    {

        public string label { get; set; }
        public string value { get; set; }
        public List<SubSubchildrenNode> children { get; set; }
    }
    public class SubSubchildrenNode
    {
        public string label { get; set; }
        public string value { get; set; }
        public List<SubSubSubchildrenNode> children { get; set; }
    }
    public class SubSubSubchildrenNode
    {
        public string label { get; set; }
        public string value { get; set; }
    }
    public class MenuSearchModel
    {
        public string search_menu { get; set; }
        public string Comp_ID { get; set; }
    }
    public class ItemMenuSearchModel
    {
        public string search_tree_menu { get; set; }
        public string Comp_ID { get; set; }
    }
    public class AccMenuSearchModel
    {
        public string search_tree_menu { get; set; }
        public string Comp_ID { get; set; }
    }

    public class FeatureMenuSearchModel
    {
        public string search_tree_menu { get; set; }
        public string Comp_ID { get; set; }
    }
    public class SearchItemBOL
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
 
    //public class AddItemGroupSetupBOL
    //{
        
    //}
}
