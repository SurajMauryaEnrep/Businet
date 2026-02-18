using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialTransferOrder
{
   public class MTOModel
    {
        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string CustomerAliasName { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string ShowCustomerAliasName { get; set; } = "N";
        public string hdnsaveApprovebtn { get; set; }
        public string ListFilterData1 { get; set; }
        public string Title { get; set; }
        public string DocumentNo { get; set; }
        public string MenuDocumentId { get; set; }
        public string ddlissueto { get; set; }
        public string trf_no { get; set; }
        public DateTime trf_dt { get; set; }
        public string trf_type { get; set; }
        public int from_br { get; set; }
        //public string from_brid { get; set; }
        public string to_br { get; set; }
        public string to_brid { get; set; }
        public int from_wh { get; set; }
        public int to_wh { get; set; }
        public string trf_rem { get; set; }
        public string trf_status { get; set; }        
        public string item_id { get; set; }
        public string uom_id { get; set; }
        public float trf_qty { get; set; }
        public float rec_qty { get; set; }
        public string it_remarks { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string mac_id { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string create_id { get; set; }
        public string create_dt { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_dt { get; set; }
        public string mod_id { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string MRSStatus { get; set; }
        public string Itemdetails { get; set; }
        public string DeleteCommand { get; set; }
        public Boolean CancelFlag { get; set; }
        public Boolean ForceClose { get; set; }
        public string MTO_ItemName { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string Createid { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string StatusCode { get; set; }
        public string Command { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string MTOSearch { get; set; }
        public string TRFNo { get; set; }
        public string AppStatus { get; set; }
        public string TRFDate { get; set; }
        public string WF_status1 { get; set; }
        public string docid { get; set; }
        public string RateDigit { get; set; }
        public string QtyDigit { get; set; }
        public string ValDigit { get; set; }

        public List<FromWharehouse> FromWharehouseList { get; set; }
        public List<ToWharehouse> ToWharehouseList { get; set; }
        public List<FromBranch> FromBranchList { get; set; }


    }
    public class UrlModel
    {
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string TRFNo { get; set; }
        public string TRFDate { get; set; }
        public DateTime MRSDate { get; set; }
        public string TransType { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
    }
    public class FromWharehouse
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }
    public class ToWharehouse
    {
        public int wh_id { get; set; }
        public string wh_val { get; set; }
    }
    public class FromBranch
    {
        public int br_id { get; set; }
        public string br_val { get; set; }
    }
}
