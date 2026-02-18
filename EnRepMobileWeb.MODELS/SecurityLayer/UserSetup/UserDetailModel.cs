using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EnRepMobileWeb.MODELS.SecurityLayer.UserSetup
{
   public class UserDetailModel
    {
        public Boolean CRMUser { get; set; }
        public string LogInID { get; set; }
        public string Designation { get; set; }
        public string ReportingTo { get; set; }
        public List<ReportingTo_List> ReportingToLists { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public bool Available { get; set; }
        public string Title { get; set; }
        public string TransType { get; set; }
        public int user_id { get; set; }
        public string user_nm { get; set; }
        public string nick_nm { get; set; }
        public string user_pwd { get; set; }
        public string def_lang { get; set; }
        public string sender_email { get; set; }
        public string mail_pwd { get; set; }
        public string host_server { get; set; }
        public int port { get; set; }
        public string ssl_flag { get; set; }
        public string use_deflt_cred { get; set; }
        public string DOB { get; set; }
        public List<Lang_List> lang_Lists { get; set; }
        public string Role_HeadOffice { get; set; }
        public string Br_HeadOffice { get; set; }
        public List<HO_List> hO_Lists { get; set; }
        public string user_email { get; set; }
        public string user_cont { get; set; }
        public string create_dt { get; set; }
        public int create_id { get; set; }
        public string mod_dt { get; set; }
        public string digi_sign { get; set; }
        public string UserImage { get; set; }
        public string hdnAttachment_UserImg { get; set; }
      
        public string UserImage_attachmentdetails { get; set; }
        public string attachmentdetails { get; set; }
        public string hdnAttachment { get; set; }
        public int mod_id { get; set; }
        public string user_salute { get; set; }
        public bool act { get; set; }
        public string Role_Name { get; set; }
        public string Branch_Name { get; set; }
        public string gender { get; set; }
        public string BranchDetail { get; set; }
        public string RoleDetail { get; set; }
        public string CreatedBy { get; set; }
        public string Createdon { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string hdnAction { get; set; }

    }
    public class ItemMenuSearchModel
    {
        public string search_tree_menu { get; set; }
        public string Comp_ID { get; set; }
    }
    public class Lang_List
    {
        public string Lang_ID { get; set; }
        public string Lang_Name { get; set; }
    }
    public class HO_List
    {
        public string HO_ID { get; set; }
        public string HO_Name { get; set; }
    }
    public class ReportingTo_List
    {
        public string ReportingTo_ID { get; set; }
        public string ReportingTo_Name { get; set; }
    }
   

    public class Header
    {
        public List<childrenNode> TreeStr { get; set; } = new List<childrenNode>();
    }

    public class childrenNode
    {
        public string label { get; set; }
        public string value { get; set; }
        public string color { get; set; }
        public List<childrenNode> children { get; set; }
    }



}
