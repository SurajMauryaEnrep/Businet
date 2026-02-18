using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SchemeSetup
{
    public class SchemeSetupList_Model
    {
        public string Title { get; set; }
        public string status { get; set; }
        public string act_status { get; set; }
        public string prod_grp { get; set; }
        public string cust_price_grp { get; set; }
        public string WF_status { get; set; }
        public List<ProdGroupList> prodGroupLists { get; set; } = new List<ProdGroupList> { new ProdGroupList { grp_id = "0", grp_name = "---Select---" } };
        public List<CustPriceGroupList> custPriceGroupLists { get; set; } = new List<CustPriceGroupList> { new CustPriceGroupList { grp_id = "0", grp_name = "---Select---" } };
        public List<Status> statusList { get; set; }
        public string ListFilterData { get; set; }
    }
    
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class SchemeSetup_Model
    {
        public string Title { get; set; }
        public string DeleteCommand { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string PreCommand { get; set; }
        public string AppStatus { get; set; }
        public string TransType { get; set; }
        public string BtnName { get; set; }
        public string DocumentMenuId { get; set; }
        public string DocumentStatus { get; set; }
        public string DocStatusName { get; set; }
        public string UserId { get; set; }
        public string create_id { get; set; }
        public string create_by { get; set; }
        public string create_on { get; set; }
        public string app_by { get; set; }
        public string app_on { get; set; }
        public string mod_by { get; set; }
        public string mod_on { get; set; }
        public string WFStatus { get; set; }
        public string WF_status1 { get; set; }
        public string ListFilterData1 { get; set; }
        public string scheme_id { get; set; }
        public string scheme_name { get; set; }
        public string valid_from { get; set; }
        public string valid_upto { get; set; }
        public string scheme_type { get; set; }
        public string rev_no { get; set; }
        public Boolean is_active { get; set; }
        public string scheme_remarks { get; set; }
        public string prod_grp_id { get; set; }
        public string cust_prc_grp_id { get; set; }
        public string scheme_slab_detail { get; set; }
        public string scheme_product_grp_detail { get; set; }
        public string scheme_cust_price_grp_detail { get; set; }
        public DataTable SchemeSlabDetail { get; set; }
        public DataTable SchemeProductGrpDetail { get; set; }
        public DataTable SchemeCustPriceGrpDetail { get; set; }
        public List<ProdGroupList> prodGroupLists { get; set; } = new List<ProdGroupList> { new ProdGroupList { grp_id = "0", grp_name = "---Select---" } };
        public List<CustPriceGroupList> custPriceGroupLists { get; set; } = new List<CustPriceGroupList> { new CustPriceGroupList { grp_id = "0", grp_name = "---Select---" } };
    }
    public class ProdGroupList
    {
        public string grp_id { get; set; }
        public string grp_name { get; set; }
    }
    public class CustPriceGroupList
    {
        public string grp_id { get; set; }
        public string grp_name { get; set; }
    }
    public class UrlModel
    {
        public string BtnName { get; set; }
        public string cmd { get; set; }
        public string InvType { get; set; }
        public string SchId { get; set; }
        public string Trp { get; set; }
        public string WFS1 { get; set; }
        public string APS { get; set; }
        public string Docid { get; set; }
        public string DMS { get; set; }
    }
}
