using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue.MaterialTransferIssue
{
    public class MaterialTransferIssueModel
    {
        public string ShowProdDesc { get; set; } = "N";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string CustomerAliasName { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string ShowCustomerAliasName { get; set; } = "N";
        public List<TransListModel> TransList { get; set; }
        public string GR_No { get; set; }
        public string GR_Dt { get; set; }
        public string HdnGRDate { get; set; }
        public string Transpt_NameID { get; set; }
        public string No_Of_Packages { get; set; }
        public string HdnTrnasportName { get; set; }
        public string Veh_Number { get; set; }
        public string Driver_Name { get; set; }
        public string Mob_No { get; set; }
        public string Tot_Tonnage { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        public string ListFilterData1 { get; set; }
        public string DocumentNo { get; set; }
        public string MenuDocumentId { get; set; }
        public DateTime MaterialIssueDate { get; set; }
        public string MaterialIssueNo { get; set; }
        public string CompId { get; set; }
        public string BrchID { get; set; }
        public string trf_type { get; set; }
        public string hdtrf_type { get; set; }
        public string from_br { get; set; }
        public string from_brid { get; set; }
        public int to_br { get; set; }
        public int hdto_brid { get; set; }
        public int from_wh { get; set; }
        public int hdfrom_whid { get; set; }
        public int to_wh { get; set; }
        public int hdto_whid { get; set; }
        public string hdto_WhName { get; set; }
        public string issue_rem { get; set; }
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
        public string CMN_Command { get; set; }
        public string create_id { get; set; }
        public string create_dt { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_dt { get; set; }
        public string mod_id { get; set; }
        public string FilterSourceWH { get; set; }
        public string FilterMTRNo { get; set; }
        public string FilterToWH { get; set; }
        public string FilterToBR { get; set; }
        public string FilterTransferType { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string MTIStatus { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string Itemdetails { get; set; }
        public string DeleteCommand { get; set; }
        public Boolean CancelFlag { get; set; }     
        public List<Req_NO> Req_NO_List { get; set; }
        public string Req_No { get; set; }
        public DateTime? Req_Date { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string MaterialIssueItemDetails { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMsg1 { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string AppStatus { get; set; }
        public string MTI_Number { get; set; }
        public string MTI_Date { get; set; }
        public string MTI_Type { get; set; }
        public string hdfrom_whName { get; set; }
        public string hdReq_Number { get; set; }
        public List<FromWharehouse> FromWharehouseList { get; set; }
        public List<ToWharehouse> ToWharehouseList { get; set; }
        public List<ToBranch> ToBranchList { get; set; }


    }

    public class URLDetailModel
    {
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string MTI_Number { get; set; }
        public string MTI_Date { get; set; }
        public string MTI_Type { get; set; }
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
    public class ToBranch
    {
        public int br_id { get; set; }
        public string br_val { get; set; }
    }
    public class Req_NO
    {
        public string RequirementNo { get; set; }
        public string RequirementDate { get; set; }
    }
}