using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.SecurityLayer.UserRoleSetup
{
    public class UserRoleSetup_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string HOName { get; set; }
        public string RoleName { get; set; }
        public string RoleID { get; set; }
        public List<HOName> HoList { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string DeleteCommand { get; set; }
        public string UserRoleSetupList { get; set; }
        public List<UserRoleSetupList> UserRoleSetup_List { get; set; }
    }
    public class HOName
    {
        public string Comp_id { get; set; }
        public string Comp_name { get; set; }
    }
    public class UserRoleSetupList
    {
        public string RoleName { get; set; }
        public string Role_ID { get; set; }
        public string HO { get; set; }
        public string HO_ID { get; set; }
        public string Createdon { get; set; }
        public string AmendedOn { get; set; }
        
    }
}

