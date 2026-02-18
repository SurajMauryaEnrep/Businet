using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.CustomerPriceList
{
   public class PriceListDetailModel
    {
        public string ListSearch { get; set; }
        public string ActStatus { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string Title { get; set; }
        public int list_no { get; set; }
        public string list_name { get; set; }
        public string valid_fr { get; set; }
        public string valid_to { get; set; }
        public Boolean act_status { get; set; }
        public string List_remarks { get; set; }
        public string list_status { get; set; }
        public string item_id { get; set; }
        public string uom_id { get; set; }
        public float sale_price { get; set; }
        public float disc_perc { get; set; }
        public float disc_unit { get; set; }
        public float effect_price { get; set; }
        public string it_remarks { get; set; }
        public string cust_pr_grp { get; set; }
        public string cust_pr_name { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string mac_id { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string create_id { get; set; }
        public string MenuDocumentId { get; set; }
        public string create_dt { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_dt { get; set; }
        public string mod_id { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string WF_status { get; set; }
        public string PriceListNo { get; set; }
        public string AppStatus { get; set; }


        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string Status { get; set; }
        public string CPL_status { get; set; }

        public string Itemdetails { get; set; }
        public DataTable Itemdetailsdata { get; set; }
        public string PriceGroupDetail { get; set; }
        public DataTable PriceGroupDetaildata { get; set; }
        public string DeleteCommand { get; set; }
        public string WF_status1 { get; set; }
        public List<PriceGroup> PriceGroupList { get; set; }
        public List<PriceListName> PriceListName { get; set; }
        public List<ListPageList> CustomerPriceList { get; set; }
    }
    public class UrlModel
    {
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string list_status { get; set; }
        public string PriceListNo { get; set; }
        public DateTime MRSDate { get; set; }
        public string TransType { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
    }
    public class PriceGroup
    {
        public string setup_id { get; set; }
        public string setup_val { get; set; }

    }
    public class PriceListName
    {
        public string list_no { get; set; }
        public string list_name { get; set; }

    }
    public class ListPageList
    {
        public string create_dt2 { get; set; }
        public string list_no { get; set; }
        public string list_name { get; set; }
        public string valid_fr { get; set; }
        public string valid_to { get; set; }
        public string Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string List_remarks { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string mod_by { get; set; }
        public string active { get; set; }
    }
}
