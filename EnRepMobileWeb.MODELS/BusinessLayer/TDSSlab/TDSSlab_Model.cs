using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.TDSSlab
{
    public class TDSSlab_Model
    {
        public string hdnSavebtn { get; set; }
        public string Title { get; set; }
        public string comp_id { get; set; }
        public string slab_id { get; set; }
        public string value_from { get; set; }
        public string value_to { get; set; }
        public string tds_perc { get; set; }
        public string Message { get; set; }
        public string HdnCommand { get; set; }
        public string tds_acc_id { get; set; }
        public string tcs_acc_id { get; set; }
        public DataTable TDSList { get; set; }
        public List<TdsAccList> tdsAccLists { get; set; }
    }
    public class TdsAccList
    {
        public string tds_acc_id { get; set; }
        public string tds_acc_name { get; set; }
    }
}
