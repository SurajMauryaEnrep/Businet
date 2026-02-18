using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EnRepMobileWeb.MODELS.BusinessLayer.GLDetail
{
    public class GLDetailModel
    {
        public string GLReportingGroup_ID { get; set; }
        public string GLReportingGroup { get; set; }
        public string Flag { get; set; }
        public string hdnSavebtn { get; set; }
        public string ifsc_code { get; set; }
        public string swift_code { get; set; }
        public string acc_no { get; set; }
        public string bank_add { get; set; }
        public string bank_name { get; set; }
        public string curr_depncy { get; set; }
        public DataTable CustomerBranchList { get; set; }
        public string GLCode { get; set; }
        public string AppStatus { get; set; }
        public string GLSearch { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get;set;}
        public string Title { get; set; }
        public static string ValueRequired { get; set; }
        public Boolean ApprovedFlag { get; set; }
        public string TransType { get; set; }
        public int acc_id { get; set; }
        public string acc_name { get; set; }
        public int acc_type { get; set; }
        public int acc_grp_id { get; set; }

        public float op_bal { get; set; }
        public string op_bal_type { get; set; }
        public string cf_type { get; set; }
        public string mac_id { get; set; }
        public int? curr { get; set; }
        public string D_InActive { get; set; }
        public Boolean act_status_tr { get; set; }
        public Boolean roa { get; set; }
        public Boolean plr { get; set; }
        public Boolean ibt { get; set; }
        public Boolean iwt { get; set; }
        public Boolean egl { get; set; }
        public Boolean sta { get; set; }
        public Boolean tr { get; set; }
        public Boolean tp { get; set; }
        public Boolean TAp { get; set; }
        public Boolean bra { get; set; }

        public string inact_reason { get; set; }
        public string Overdraft_Limit { get; set; }
        public int create_id { get; set; }
        public string createby { get; set; }
        public int app_id { get; set; }
        public int mod_id { get; set; }
        public string modby { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CompId { get; set; }

        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }

        public string UserIP { get; set; }
        public string creat_dt { get; set; }
        public string mod_dt { get; set; }

        //public List<GLBranch> CustomerBranchList { get; set; }
        public string GLBranchDetail { get; set; }
        public List<curr> currList { get; set; }
        public List<AccountGroup> AccountGroupList { get; set; }
        public string DeleteCommand { get; set; }
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string GLActStatus { get; set; }
        public string ddlGLName { get; set; }
       
        public string ddlGLGroup { get; set; }
        public List<GLName> GLNameList { get; set; }
        public List<GLGroup> GLGroupList { get; set; }
    }
    public class GLName
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class GLGroup
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    //public class GLBranch
    //{
    //    public string comp_id { get; set; }
    //    public string comp_nm { get; set; }
    //}
    public class AccountGroup
    {
        public string acc_grp_id { get; set; }
        public string AccGroupChildNood { get; set; }

    }
    public class curr
    {
        public string curr_id { get; set; }
        public string curr_name { get; set; }

    }
}
