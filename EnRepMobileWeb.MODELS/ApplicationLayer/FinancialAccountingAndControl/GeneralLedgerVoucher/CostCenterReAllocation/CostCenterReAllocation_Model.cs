using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.CostCenterReAllocation
{
    public class CostCenterReAllocation_Model
    {
        public string GLAccount { get; set; }
        public string AccountType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Title { get; set; }
        public string CCType { get; set; }
        public string CCVal { get; set; }
        public string AllocationType { get; set; }
        public string CC_DetailList { get; set; }
        public string disflag { get; set; } = "N";
        
        public List<GLAccountName> GLAccNameList { get; set; }
        public string HdnMultiselectGLAccName { get; set; }
        public List<CostCenterTypeList> CostCenterTypLists { get; set; }
        public string HdnMultiselectCostCenterTyp { get; set; }
        public List<CostCenterValueList> CostCenterValueLists { get; set; }
        public string HdnMultiselectCostCenterVal { get; set; }
        public string HdnCostCenterValId { get; set; }
        public string HdnCostCenterValName { get; set; }
        
    }
    public class GLAccountName
    {
        public string acc_id { get; set; }
        public string acc_name { get; set; }

    }
    public class CostCenterTypeList
    {
        public string cctyp_id { get; set; }
        public string cctyp_name { get; set; } 
    }
    public class CostCenterValueList
    {
        public string cc_val_id { get; set; } 
        public string cc_val_name { get; set; } 
    }
}
