using System;
using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.TrialBalance
{
    public class TrialBalanceModel
    {
        public string BalanceByFilter { get; set; }
        public string BalanceBy { get; set; }
        public string ddlGroup { get; set; }
        public string Title { get; set; }
        public string BalanceType { get; set; }
        public string ReportType { get; set; }
        public string AccountName { get; set; }
        public string AccountGroup { get; set; }
        public string AccountType { get; set; }
        public string Branch { get; set; }
        public List<AccName> AccNameList { get; set; }
        public List<AccGroup> AccGroupList { get; set; }
        public List<Branch> BranchList { get; set; }
        public string UpToDate { get; set; }
        public string Currency { get; set; }
        public string AccountGroupName { get; set; }
        public string AccountTypeName { get; set; }
        public string hdnPDFPrint { get; set; }
        public string PrintPDFData { get; set; }
        public string PrintTotalPDFData { get; set; }
        
        public string PrintBrIdListPDFData { get; set; }
        
        public string hdnCSVPrint { get; set; }
        public string searchValue { get; set; }
        public string InsightButton { get; set; }
        public string Allfilters { get; set; }
        public string InsightButtonData { get; set; }
        public List<TrialBalanceList> TrialBalanceListDetails { get; set; }
    }
    public class AccName
        {
        public string acc_id { get; set; }
        public string acc_name { get; set; }
    }
    public class AccGroup
    {
        public string accgroup_id { get; set; }
        public string accgroup_name { get; set; }
    }
    public class Branch
    {
        public string br_id { get; set; }
        public string br_name { get; set; }
    }
    public class TrialBalanceList
    {
        public Int64 SrNo { get; set; }//add by shubham maurya on 01-01-2025 for export CSV
        public string AccountName { get; set; }
        public string AccountId { get; set; }
        public string AccountType { get; set; }
        public string AccountGroup { get; set; }
        public string Branch { get; set; }
        public string BranchId { get; set; }
        public string curr_name { get; set; }
        public string curr_id { get; set; }
        public string Ho_op_dr_bs { get; set; }
        public string Ho_op_cr_bs { get; set; }
        public string Ho_op_dr_sp { get; set; }
        public string Ho_op_cr_sp { get; set; }
        public string br_op_dr_bs { get; set; }
        public string br_op_cr_bs { get; set; }
        public string br_op_dr_sp { get; set; }
        public string br_op_cr_sp { get; set; }
        public string HoTotalDebit_bs { get; set; }
        public string HoTotalDebit_sp { get; set; }
        public string HoTotalCredit_bs { get; set; }
        public string HoTotalCredit_sp { get; set; }
        public string BrTotalDebit_bs { get; set; }
        public string BrTotalDebit_sp { get; set; }
        public string BrTotalCredit_bs { get; set; }
        public string BrTotalCredit_sp { get; set; }
        public string Ho_cl_dr_bs { get; set; }
        public string Ho_cl_cr_bs { get; set; }
        public string Ho_cl_dr_sp { get; set; }
        public string Ho_cl_cr_sp { get; set; }
        public string br_cl_dr_bs { get; set; }
        public string br_cl_cr_bs { get; set; }
        public string br_cl_dr_sp { get; set; }
        public string br_cl_cr_sp { get; set; }
        public string Vou_No { get; set; }
        public string Vou_dt { get; set; }
        public string cc_vou_amt_bs { get; set; }
        public string cc_vou_amt_sP { get; set; }
        public string amt_type { get; set; }
        public string curr_logo { get; set; }
        public string conv_rate { get; set; }
        public string nurr { get; set; }
    }
}
