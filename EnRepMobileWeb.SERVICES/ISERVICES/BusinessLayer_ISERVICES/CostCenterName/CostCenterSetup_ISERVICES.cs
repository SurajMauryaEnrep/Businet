using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.CostCenterSetup
{
    public interface CostCenterSetup_ISERVICES
    {
        DataSet GetCCSetupTable(string comp_id);
        string SaveCCSetupData(string TransType, string comp_id, string user_id, string cc_id,string cc_name);
        string SaveCCSetupValueData(string TransType, string comp_id, string user_id, string cc_id,string cc_val_id,string cc_val_name);
        string SaveCCSetup_branch_ueData(string TransType, string comp_id, string user_id, string cc_id, string Branch_id);
        string SaveCCSetup_Module(string TransType, string comp_id, string user_id, string cc_id, string module_id);
        string DeleteCCSetup( string TransType, string cc_id, string comp_id);
        string DeleteCC_Setup_val(string TransType, string cc_val_id, string comp_id);
        string deleteCC_branchSetup_val(string TransType, string cc_id,string br_id, string comp_id);
        string deleteCC_module(string TransType, string cc_id, string br_id, string comp_id);
        DataSet GetModulDetails(string CompID);
        DataTable GetCostCenterList(string CompId);
        DataSet GetAllDropDown(string flag, string CompID, string create_id);
        DataSet getAppDocDetails(string flag, string CompID, string create_id);
        DataSet GetBranchOnchangeCC(string ddlcc_id, string CompID, string create_id);
        DataSet GetModuleOnchangeCC(string DDLModulecc_id, string CompID, string create_id);
    }
}
