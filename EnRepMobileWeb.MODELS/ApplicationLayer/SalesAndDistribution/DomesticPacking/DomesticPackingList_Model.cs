using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.Packing
{
     public class DomesticPackingList_Model
    {
        public string Flag { get; set; }
        public string WF_Status { get; set; }
        public string DPDSearch { get; set; }
        public string DocumentMenuId { get; set; }
        public string ListFilterData { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string SourceType { get; set; }
        public string customerID { get; set; }
        public string cust_id { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ListCustomerName> CustomerNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<PackingList> PackingListDetail { get; set; }
    }
public class Status
{
    public string status_id { get; set; }
    public string status_name { get; set; }
}

public class ListCustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }
    public class PackingList
    {
        public string ListFilterData1 { get; set; }
        public string pack_dt { get; set; }
        public string PackingListNO { get; set; }
        public string CustomerName { get; set; }
        public string SourceType { get; set; }
        public string CreatedON { get; set; }
        public string ModifiedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string PackStatus { get; set; }
        public string pack_type { get; set; }
        public string packing_date { get; set; }
        public string create_by { get; set; }
        public string mod_by { get; set; }
        public string app_by { get; set; }
    }
}
