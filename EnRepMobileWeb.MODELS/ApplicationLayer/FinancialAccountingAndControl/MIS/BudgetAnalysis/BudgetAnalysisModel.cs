using System;
using System.Collections.Generic;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.BudgetAnalysis
{
    public class BudgetAnalysisModel
    {
        public string Title { get; set; }
        public string glAccId { get; set; }
        public string PeriodDashBoad { get; set; }
        public string FinYear { get; set; }
        public string Period { get; set; }
        public string Quarter { get; set; }
        public string Month { get; set; }
        public string ShowAs { get; set; }
        public string Search { get; set; }
        public string IsFiltered { get; set; }
        public string searchValue { get; set; }
        public string hdnCSVPrint { get; set; }
        public string BudgetAnalysisData { get; set; }
        public List<GlAccountModel> GlAccList { get; set; }
        public List<FinYearModel> FinYearList { get; set; }
        public List<FyMonthsModel> FyMonthslist { get; set; }
        public List<FYQuarterModel> FYQuarterList { get; set; }
        public List<BudgetAnalysisListYearly> _BudgetAnalysisListYearly { get; set; }
        public List<BudgetAnalysisListQuarterly> _BudgetAnalysisListQuarterly { get; set; }
        public List<BudgetAnalysisListMonthly> _BudgetAnalysisListMonthly { get; set; }
    }
    public class BudgetAnalysisListYearly
    {
        public Int64 SrNo { get; set; }
        public string acc_name { get; set; }
        public string acc_Group_name { get; set; }
        public string Budget_amount { get; set; }
        public string Actual_amount { get; set; }
        public string Variance { get; set; }
    }
    public class BudgetAnalysisListQuarterly
    {
        public Int64 SrNo { get; set; }
        public string acc_name { get; set; }
        public string acc_Group_name { get; set; }
        public string Budget_amountQ1 { get; set; }
        public string Actual_amountQ1 { get; set; }
        public string VarianceQ1 { get; set; }
        public string Budget_amountQ2 { get; set; }
        public string Actual_amountQ2 { get; set; }
        public string VarianceQ2 { get; set; }
        public string Budget_amountQ3 { get; set; }
        public string Actual_amountQ3 { get; set; }
        public string VarianceQ3 { get; set; }
        public string Budget_amountQ4 { get; set; }
        public string Actual_amountQ4 { get; set; }
        public string VarianceQ4 { get; set; }
    }
    public class BudgetAnalysisListMonthly
    {
        public Int64 SrNo { get; set; }
        public string acc_name { get; set; }
        public string acc_Group_name { get; set; }
        public string Budget_amount_M1 { get; set; }
        public string Actual_amount_M1 { get; set; }
        public string Variance_M1 { get; set; }
        public string Budget_amount_M2 { get; set; }
        public string Actual_amount_M2 { get; set; }
        public string Variance_M2 { get; set; }
        public string Budget_amount_M3 { get; set; }
        public string Actual_amount_M3 { get; set; }
        public string Variance_M3 { get; set; }
        public string Budget_amount_M4 { get; set; }
        public string Actual_amount_M4 { get; set; }
        public string Variance_M4 { get; set; }
        public string Budget_amount_M5 { get; set; }
        public string Actual_amount_M5 { get; set; }
        public string Variance_M5 { get; set; }
        public string Budget_amount_M6 { get; set; }
        public string Actual_amount_M6 { get; set; }
        public string Variance_M6 { get; set; }
        public string Budget_amount_M7 { get; set; }
        public string Actual_amount_M7 { get; set; }
        public string Variance_M7 { get; set; }
        public string Budget_amount_M8 { get; set; }
        public string Actual_amount_M8 { get; set; }
        public string Variance_M8 { get; set; }
        public string Budget_amount_M9 { get; set; }
        public string Actual_amount_M9 { get; set; }
        public string Variance_M9 { get; set; }
        public string Budget_amount_M10 { get; set; }
        public string Actual_amount_M10 { get; set; }
        public string Variance_M10 { get; set; }
        public string Budget_amount_M11 { get; set; }
        public string Actual_amount_M11 { get; set; }
        public string Variance_M11 { get; set; }
        public string Budget_amount_M12 { get; set; }
        public string Actual_amount_M12 { get; set; }
        public string Variance_M12 { get; set; }
    }
    public class GlAccountModel
    {
        public string acc_id { get; set; }
        public string acc_name { get; set; }
    }
    public class FinYearModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class FyMonthsModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class FYQuarterModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
