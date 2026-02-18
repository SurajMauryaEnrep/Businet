using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.CurrencySetup
{
    public class CurrencySetup_Model
    {
        public int Curr_name { get; set; }
        public int Curr_id { get; set; }
        public string hdncurrsetup { get; set; }
        public string Title { get; set; }
        public string Eff_Date { get; set; }
        public string Price { get; set; }
        public string hdnConv_date { get; set; }
        public string Message { get; set; }
        public string DeleteCommand { get; set; }
        public string hdnAction { get; set; }
        public string conv_date { get; set; }
        public string Command { get; set; }
        public List<CurrencyNameLIst> _currencyNameList { get; set; }

    }
    public class CurrencyNameLIst
    {
        public int curr_id { get; set; }
        public string curr_name { get; set; }
    }
}
