using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS
{
   public class BaseViewModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string AmendedBy { get; set; }
        public DateTime AmendedDate { get; set; }
        public string Status { get; set; }
    }
}
