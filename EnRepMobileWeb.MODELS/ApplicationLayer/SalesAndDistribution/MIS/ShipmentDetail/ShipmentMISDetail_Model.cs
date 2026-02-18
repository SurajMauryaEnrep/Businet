using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.MIS.ShipmentDetail
{
    public class ShipmentMISDetail_Model
    {
        public string Title { get; set; }
        public string ItemId { get; set; }
        public string CustId { get; set; }
        public string ShipType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ShowAs { get; set; }
        public List<Customers> CustomersList { get; set; }
        public List<Items> itemsList { get; set; }
        public string SearchStatus { get; set; }
    }
    public class Customers
    {
        public string CustId { get; set; }
        public string CustName { get; set; }
    }
    public class Items
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
