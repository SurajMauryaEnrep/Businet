using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.PurchaseReturn
{
   public class PurchaseReturnList_Model
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public string SourceType { get; set; }
        public string supp_id { get; set; }
        public string PRFromDate { get; set; }        
       public string PRToDate { get; set; }
        public string FromDate { get; set; }       
        public string ListFilterData { get; set; }       
        public string LSISearch { get; set; }       
        public string DNSearch { get; set; }       
        public string WF_status { get; set; }       
        public string MTISearch { get; set; }       
        public DateTime ToDate { get; set; }
        public List<SupplierNameList> SupplierNameList { get; set; }
        public List<PRList> PurchaseReturnList { get; set; }
        public List<Status> StatusList { get; set; }
        public String Spp_Name { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class SupplierNameList
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class PRList
    {
        public string PRTNumber { get; set; }
        public string PRTDate { get; set; }
        public string hdPRTDate { get; set; }
        public string SrcType { get; set; }
        public string inv_no { get; set; }
        public string inv_date { get; set; }
        public string SupplierName { get; set; }
        public string PRT_value { get; set; }
        public string PRTStatus { get; set; }
        public string CreatedON { get; set; }
        public string ModifiedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
    }
}
