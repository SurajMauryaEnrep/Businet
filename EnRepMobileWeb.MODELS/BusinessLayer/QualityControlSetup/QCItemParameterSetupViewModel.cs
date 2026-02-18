using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnRepMobileWeb.MODELS.Enumerator;

namespace EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup
{
   public class QCItemParameterSetupViewModel: BaseViewModel
    {
        public string IPSearch { get; set; }
        public string BtnNameIPS { get; set; }
        public string Title { get; set; }
        public int comp_id { get; set; }        
        public string param_Id { get; set; }
        public string uom_id { get; set; }
        // public string param_name { get; set; }
        public string item_id { get; set; }
        public string para_type { get; set; }
        public string upper_val { get; set; }
        public string lower_val { get; set; }
        public string create_id { get; set; }
        public string create_dt { get; set; }
        public int mod_id { get; set; } 
        public string mod_dt { get; set; }
        public string mac_id { get; set; }    
        public string item_parList { get; set; }    
        public string ListFilterData { get; set; }    
        public string item_Group { get; set; }    
        public DataOperation  Operation { get; set; }
        public string remarks { get; set; }
        public List<getViewModelItemList> getViewModelItemList { get; set; }
        public List<ItemParaList> getperaItemList { get; set; }
        public List<ItemGroupList> getItemGroupList { get; set; }

    }
    public class ItemParaList
    {
        public string Itm_id { get; set; }
        public string Itm_name { get; set; }
    }
    public class ItemGroupList
    {
        public string gr_id { get; set; }
        public string gr_name { get; set; }
    }
    public class getViewModelItemList
    {
        public int sr_no { get; set; }
        public string mod_by { get; set; }
        public string app_by { get; set; }
        public string create_by { get; set; }
        public string item_id { get; set; }
        public string item_Name { get; set; }
        public string param_Id { get; set; }
        public string uom_id { get; set; }
        public string uom_name { get; set; }
        public string param_name { get; set; }    
        public string group { get; set; }
        public string UOM { get; set; }
        public string OEM_No { get; set; }
        public string Sample_code { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedOn { get; set; }
        public string comp_Id { get; set; }

    }

}
