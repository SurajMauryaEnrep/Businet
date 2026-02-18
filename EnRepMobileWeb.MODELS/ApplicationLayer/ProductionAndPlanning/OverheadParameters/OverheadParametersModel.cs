using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.OverheadParameters
{
   public class OverheadParametersModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string OHD_ID { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string Title { get; set; }
        public int ohd_exp_id { get; set; }
        public string ohd_exp_name { get; set; }
        public int uom_id { get; set; }
        public string ohd_exp_remarks { get; set; }
        public int create_id { get; set; }
        //public int TransType { get; set; }
        public string DeleteCommand { get; set; }
        public List<UOMList> _uomList { get; set; }
        public class UOMList
        {
            public string uom_id { get; set; }
            public string uom_val { get; set; }
        }
        

    }

} 
