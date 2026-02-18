using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.CostCenterSetup
{
    public class CostCenterSetup_Model
    {
        public DataTable CostCenterValue { get; set; }
        public DataTable CostCenterModule { get; set; }
        public DataTable CostCenterBranch { get; set; }
        public DataTable CostCenterno { get; set; }
        public string hdnSavebtn { get; set; }
        public string TransTypeCCSetup { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public int comp_id { get; set; }
        public string cc_id { get; set; }
        public string DDLcc_id { get; set; }
        public string cc_name { get; set; }
        public string BtnName { get; set; }
        public string ShowCC { get; set; }
        public string ShowVal { get; set; }
        public string Showbranch { get; set; }
        public string ShowModule { get; set; }
        public string module_id { get; set; }
        public string Modulecc_id { get; set; }
        public string user_id { get; set; }
        public string cc_val_id { get; set; }
        public string cc_val_name { get; set; }
        public bool act_status { get; set; }
        public string BranchCC_id { get; set; }
        public string Branch_id { get; set; }
        public List<CostCenter> costCenter { get; set; }
        public List<Branch_list> _branchList { get; set; }
        public List<ModuleNoList> _moduleno { get; set; }

    }
    public class Branch_list
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
    }
    public class CostCenter
    {
        public string cc_id { get; set; }
        public string cc_name { get; set; }
    }
    public class ModuleNoList
    {
        public string module_id { get; set; }
        public string module_name { get; set; }
    }
}
