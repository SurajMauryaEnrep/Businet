using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup
{
    public class GetTransporterDetailsModel
    {
        public string TransId { get; set; }
        public string TransType { get; set; }
        public string TransMode { get; set; }
        public string SearchStatus { get; set; }
        public string Title { get; set; }
        public List<TransListModel> TransList { get; set; }
    }
    public  class TransListModel
    {
        public string TransId { get; set; }
        public string TransName { get; set; }
    }
}
