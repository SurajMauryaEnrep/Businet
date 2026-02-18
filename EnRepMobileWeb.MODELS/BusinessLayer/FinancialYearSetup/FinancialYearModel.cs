using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.FinancialYearSetup
{
    public class FinancialYearModel
    {
        public string transtype { get; set; }
        public string command { get; set; }
        public string title { get; set; }
        public string pfy { get; set; }
        public string pfy_startdt { get; set; }
        public string pfy_enddt { get; set; }
        public string nfy { get; set; }
        public string nfy_startdt { get; set; }
        public string nfy_enddt { get; set; }
        public Boolean closebook { get; set; }
        public List<FinancialYearList> fy_list { get; set; }
        public List<Br_list> br_list { get; set; }
        public string br_id { get; set; }
    }
    public class FinancialYearList
    {
        public string sno { get; set; }
        public string fy_year { get; set; }
        public string pfy_sdt { get; set; }
        public string pfy_edt { get; set; }
        public string nfy_sdt { get; set; }
        public string nfy_edt { get; set; }
        public string fy_status { get; set; }
        public string book_status { get; set; }
        public string bk_close { get; set; }
    }
    public class Br_list
    {
        public string br_id { get; set; }
        public string br_name { get; set; }
    }
}
