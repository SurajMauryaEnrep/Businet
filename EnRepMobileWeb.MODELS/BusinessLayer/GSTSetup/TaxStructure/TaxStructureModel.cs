using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.GSTSetup.TaxStructure
{
   public class TaxStructureModel
    {
        public string Flag { get; set; }
        public string hdnSavebtn { get; set; }
        //public int TaxCode { get; set; }
        public string TaxCode { get; set; }
        public string AppStatus { get; set; }
        public string TaxSearch { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string TransType { get; set; }
        public string Title { get; set; }
        public static string ValueRequired { get; set; }       
        public string DeleteCommand { get; set; }
        public string Command { get; set; }
        public string igst_tax_id { get; set; }      
        public string rcm_igst_tax_id { get; set; }      
        public string cgst_tax_id { get; set; }      
        public string rcm_cgst_tax_id { get; set; }      
        public string sgst_tax_id { get; set; }        
        public string rcm_sgst_tax_id { get; set; }           
        public float? igst_tax_perc { get; set; }
        public float? cgst_tax_perc { get; set; }
        public float? sgst_tax_perc { get; set; }

        public List<TaxPerc> TaxPercList { get; set; }
        public List<Tax>TaxList { get; set; }  
        public string EditData { get; set; }
    }
    public class TaxPerc
    {
        public string tax_id { get; set; }
        public string tax_perc { get; set; }

    }
    public class Tax
    {
        public string tax_id { get; set; }
        public string tax_name { get; set; }

    }   

}
