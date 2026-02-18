using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.Inventory_Management
{
    public class DeliveryNoteList_MODELS
    { 
        public string Title { get; set; }
        public string Status { get; set; }
        public string SourceType { get; set; }
        public string SupplierID { get; set; }
        public string supp_id { get; set; }
        public string ListFilterData { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<DeliveryNote> DeliveryNoteList { get; set; }
        public List<Status> StatusList { get; set; }
        public String Spp_Name { get; set; }
        public String DNSearch { get; set; }
        public String WF_status { get; set; }
        public String flag { get; set; }

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


    public class DeliveryNote
    {
        public string DeliverNoteNumber { get; set; }
        public string DeliveryNoteDate { get; set; }
        public string Dn_date { get; set; }
        public string SupplierName { get; set; }
        public string SourceType { get; set; }
        public string src_doc_no { get; set; }
        public string src_doc_date { get; set; }
        public string CreatedON { get; set; }
        public string ModifiedOn { get; set; }
        public string ApprovedOn { get; set; }
        public string DeliveryNoteStatus { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string curr_name { get; set; }
        public string mod_by { get; set; }
        public string BillNumber { get; set; }
        public string BillDate { get; set; }
    }   
}
