using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.DocApproval
{
    public class DocApprovalModel
    {
        public string hdnSavebtn { get; set; }
        public string createdt { get; set; }
        public string moddt { get; set; }
        public string createid { get; set; }
        public string modid { get; set; }
        public DataTable DocAppList { get; set; }
        public DataTable Data { get; set; }
        public string AppStatus { get; set; }
        public string Command { get; set; }
        public string DeleteCode { get; set; }
        public string EditCode { get; set; }
        public string btnName { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string DASearch { get; set; }
        public string Title { get; set; }
        public string Branch_id { get; set; }
     
        public string UserDetailslist { get; set; }
        public List<Branch_list> _branchList { get; set; }
    
        public string Doc_id { get; set; }
        public List<Doc_list> _docList { get; set; }
     
        public string Username { get; set; }
      
        public List<User_list>  _userList { get; set; }
    
        public string hdnAction { get; set; }

    }
    public class Branch_list
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
    }
    public class Doc_list
    {
        public string DocID { get; set; }
        public string Document { get; set; }
    }
    public class User_list
    {
        public int User_ID { get; set; }
        public string User_name { get; set; }
    }

}
