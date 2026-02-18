using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.AccountGroup
{
   public class AccountGroupModel
    {
        public string Delete_Dependcy { get; set; }
        public string hdnSavebtn { get; set; }
        public string AppStatus { get; set; }
        public string GroupID { get; set; }
        public string ChkPgroup { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string Title { get; set; }
        public string TransType { get; set; }
        public int comp_id { get; set; }
        public int? acc_grp_id { get; set; }
        public string acc_group_name { get; set; }
        public string acc_grp_struc { get; set; }
        public string parent_acc_grp_id { get; set; }
        public string alt_grp_id { get; set; }
        public string grp_head { get; set; }
        public string grp_seq_no { get; set; }
        public string grp_type { get; set; }
        public string last_level { get; set; }
        public int? create_id { get; set; }
        public int? mod_id { get; set; }
        public string FormMode { get; set; }
        public string DeleteCommand { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public List<AccountGroup> ParentItemGroup { get; set; }
    }
    public class AccountGroup
    {
        public string acc_grp_struc { get; set; }
        public string acc_group_name { get; set; }

    }

    public class MenuSearchModel
    {
        public string search_menu { get; set; }
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
    }
}
