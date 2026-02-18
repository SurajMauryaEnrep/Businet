using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup
{
    public class SearchSupp
    {

        public string ddlGroup { get; set; }
        public IEnumerable<SelectListItem> Group { get; set; }
        public string ddlSupp { get; set; }
        public string ddlGrp
        {
            get; set;
        }
    }
}
