using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.Shipment
{
    public class ShipmentList_MODEL
    {

        public string Flag { get; set; }
        public string CustomerId { get; set; }
        public string ListFilterData { get; set; }
        public string Title { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ListCustomerName> CustomerNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public string cust_id { get; set; }
        public string StatusCode { get; set; }
        public string SCSearch { get; set; }
        public string MenuDocumentId { get; set; }
        public string WF_Status { get; set; }
        public string DocumentMenuId { get; set; }
        public string ShipMent_type { get; set; }



        public List<ShipmentDetails> ShipmentDetailsList { get; set; }
    }

    public class ListCustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }


    public class Status
    {
        public string status_code { get; set; }
        public string status_name { get; set; }

    }

    public class ShipmentDetails
    {
        public string ship_no { get; set; }
        public string finstrdate { get; set; }
        public string ship_dt { get; set; }
        public string ship_date { get; set; }
        public string custom_inv_no { get; set; }
        public string custom_inv_dt { get; set; }
        public string ship_status { get; set; }
        public string pack_no { get; set; }
        public string pack_dt { get; set; }
        public string cust_name { get; set; }
        public string create_by { get; set; }
        public string create_dt { get; set; }
        public string app_dt { get; set; }
        public string ship_type { get; set; }
        public string mod_dt { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
        public string GRNumber { get; set; }
        public string GRDate { get; set; }
        public string TransporterName { get; set; }
    }
}
