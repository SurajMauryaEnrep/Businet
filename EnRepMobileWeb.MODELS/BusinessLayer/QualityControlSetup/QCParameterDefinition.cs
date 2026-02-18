using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup
{
   public class QCParameterDefinition
    {
        public string hdnSavebtn { get; set; }
        public string BtnPrmDef { get; set; }
        public string MessageHsn { get; set; }
        public string Ptype { get; set; }
        public string Pname { get; set; }
        public string MessagePrmDef { get; set; }
        public string Title { get; set; }
        public string paramId { get; set; }
        public int comp_id { get; set; }
        public int param_Id { get; set; }     
        public string param_name { get; set; }    
        public string param_type { get; set; }
        public int create_id { get; set; }
        public string create_dt { get; set; }
        public int mod_id { get; set; }
        public string mod_dt { get; set; }
        public string mac_id { get; set; }
       public List<ParameterList> ParamDefinitionList { get; set; }
        public List<UomList> ParamUomList { get; set; }
        public string BtnName { get; set; }

    }

  public  class ParameterList
    {
        public int param_Id { get; set; }
        public string param_name { get; set; }
        public string param_type { get; set; }
        public string param_type_val { get; set; }
        public string create_id { get; set; }
        public string create_dt { get; set; }     
        public string mod_dt { get; set; }
        public string mod_id { get; set; }
        public string mod_name { get; set; }
    }

    public class UomList
    {
        public int uom_id { get; set; }
        public string uom_name { get; set; }
    }
}
