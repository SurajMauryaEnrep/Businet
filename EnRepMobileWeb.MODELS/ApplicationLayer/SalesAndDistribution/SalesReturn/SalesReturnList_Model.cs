using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesReturn
{
   public class SalesReturnList_Model
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public string SourceType { get; set; }
        public string cust_id { get; set; }
        public string SRFromDate { get; set; }
        public string SRToDate { get; set; }
        public string FromDate { get; set; }
        public string SRSearch { get; set; }
        public string WF_status { get; set; }
        public DateTime ToDate { get; set; }
        public List<CustomerNameList> CustomerNameList { get; set; }
        public List<SRList> SalesReturnList { get; set; }
        public List<Status> StatusList { get; set; }
        public String Customer_Name { get; set; }
        public string ListFilterData { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class CustomerNameList
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }
    public class SRList
    {
        public string SRTNumber { get; set; }
        public string SRTDate { get; set; }
        public string hdSRTDate { get; set; }
        public string inv_no { get; set; }    
        public string CustomerName { get; set; }
        public string SRT_value { get; set; }
        public string SRTStatus { get; set; }
        public string CreatedON { get; set; }
        public string ModifiedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string create_by { get; set; }
        public string mod_by { get; set; }
        public string app_by { get; set; }
        public string list_src_type { get; set; }
    }
}
