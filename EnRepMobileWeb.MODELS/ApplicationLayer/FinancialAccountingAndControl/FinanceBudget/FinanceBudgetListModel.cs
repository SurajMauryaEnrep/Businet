using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.FinanceBudget
{
    public class FinanceBudgetListModel
    {
        public List<Finyr> Finyrlist { get; set; }
        public string Fin { get; set; }
        public string Status { get; set; }
        public List<Budlist> BudgetList { get; set; }
        public string BudSearch { get; set; }
        public string Title { get; set; }
        public string ListFilterData { get; set; }
        public string WF_Status { get; set; }
        public List<StatusList> statusLists { get; set; }

        
        public class Budlist
        {
            public string Finyr { get; set; }
            public string Period { get; set; }
            public string Revno { get; set; }
            public string Remarks { get; set; }
            public string Status { get; set; }
            public string CreateDate { get; set; }
            public string ApproveDate { get; set; }
            public string ModiDate { get; set; }         
        }
        public class Finyr
        {
            public string FinyrId { get; set; }
            public string Finyrs { get; set; }
        } 
    }
    public class StatusList
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
}
