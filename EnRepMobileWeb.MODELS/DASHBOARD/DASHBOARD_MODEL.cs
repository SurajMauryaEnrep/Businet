using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.DASHBOARD
{
    public class DASHBOARD_MODEL
    {
        public string Branch { get; set; }

        public List<Branch> BranchList { get; set; }
    }
    public class Branch
    {
        public string Comp_Id { get; set; }
        public string comp_nm { get; set; }
    }
}
