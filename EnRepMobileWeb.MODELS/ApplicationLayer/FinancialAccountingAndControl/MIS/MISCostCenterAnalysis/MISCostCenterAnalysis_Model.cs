using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.MISCostCenterAnalysis
{
    public class MISCostCenterAnalysis_Model
    {
        public string Title { get; set; }
        public string cc_id { get; set; }
        public string cc_val_id { get; set; }
        public string from_dt { get; set; }
        public string to_dt { get; set; }
        public string searchValue { get; set; }
        public string hdnCSVPrint { get; set; }
        public string cc_TransactionDetail { get; set; }
        public List<CostCenterList> costCenterLists { get; set; } = new List<CostCenterList>() { new CostCenterList { cc_id = "0",cc_name= "---select---" } };
        public List<CostCenterValueList> costCenterValueLists { get; set; } = new List<CostCenterValueList>() { new CostCenterValueList { cc_val_id = "0", cc_val_name = "---select---" } };
        public List<cc_TransactionDetail> _cc_TransactionDetail { get; set; }
    }
    public class cc_TransactionDetail
    {
        public Int64 SrNo { get; set; }
        public string Vou_No { get; set; }
        public string Vou_dt { get; set; }
        public string cc_vou_amt_bs { get; set; }
        public string cc_vou_amt_sP { get; set; }
        public string amt_type { get; set; }
        public string curr_logo { get; set; }
        public string conv_rate { get; set; }
        public string nurr { get; set; }

    }
    public class CostCenterList
    {
        public string cc_id { get; set; } = "0";
        public string cc_name { get; set; } = "---Select---";
    }
    public class CostCenterValueList
    {
        public string cc_val_id { get; set; } = "0";
        public string cc_val_name { get; set; } = "---Select---";
    }
}
