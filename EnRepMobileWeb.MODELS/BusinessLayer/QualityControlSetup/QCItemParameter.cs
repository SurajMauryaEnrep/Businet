using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup
{
   public class QCItemParameter
    {      
        public int comp_id { get; set; }
        public int param_Id { get; set; }
        [Required(ErrorMessage = "Value Required")]
        public string param_name { get; set; }
        [Required(ErrorMessage = "Value Required")]
        public string param_type { get; set; }
        public int create_id { get; set; }
        public DateTime create_dt { get; set; }
        public int mod_id { get; set; }
        public DateTime mod_dt { get; set; }
        public string mac_id { get; set; }


    }
}
