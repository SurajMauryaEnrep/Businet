using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.TaxSlab
{
    public class TaxSlab_Model
    {
        public string hdnSavebtn { get; set; }
        public string Title { get; set; }

        public string HSN_Number { get; set; }
        //public string tax_per { get; set; }
        public float? tax_per { get; set; }
        public bool goods { get; set; }
        public string Message { get; set; }
        public bool services { get; set; }
        public string TransType { get; set; }
        public string HSNDetail { get; set; }
        public string create_id { get; set; }
        public string DeleteCommand { get; set; }
        public string creat_dt { get; set; }
        public string mod_id { get; set; }
        public string mod_dt { get; set; }
        public string listTaxPer { get; set; }
        public string hdnAction { get; set; }
        public string hdnlistTaxPer { get; set; }
        public string hdnlistTaxHsn { get; set; }
        //public int hdntexper { get; set; }//commentd by sm on 07-12-2024
        public string hdntexper { get; set; }
        public string HSN_code { get; set; }
        public List<HSNno> HSNList { get; set; }
        public List<HSNNumbarList> hSNNumbars { get; set; }
        public List<taxPerlist> TaxPerlist { get; set; }

    }
    public class taxPerlist
    {
        public string taxPer_id { get; set; }
        public string taxPer { get; set; }
    }
    public class HSNno
    {
        public string setup_val { get; set; }
        public string setup_id { get; set; }
    }
    public class HSNNumbarList
    {
        public string HSNNumberId { get; set; }
        public string HSNNumber { get; set; }
    }
}
