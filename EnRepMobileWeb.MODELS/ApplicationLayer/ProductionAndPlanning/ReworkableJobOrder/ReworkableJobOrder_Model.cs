using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ReworkableJobOrder
{
   public class ReworkableJobOrder_Model
    {
        public string ShopFloorAvl_Qty { get; set; }
        public string src_type_WarehouseName { get; set; }
        public string Material_Type { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentMenuId { get; set; }
        public string DeleteCommand { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
       
        public string BrchID { get; set; }
        public string MAppLevel { get; set; }
        public string ReworkJO_No { get; set; }
        public string ReworkJO_Date { get; set; }
        public string Item_Id { get; set; }
        public string Item_Name { get; set; }
        public string Item_ibatch { get; set; }
        public string Item_iserial { get; set; }
        public string ItemName { get; set; }
        public string ItemName1 { get; set; }
        public List<ItemName> ItemNamelist { get; set; }
        public string uom_id { get; set; }
        public string uom_name { get; set; }
        public string WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string Warehouse { get; set; }
        public List<wh_namelist> wh_Namelist { get; set; }
        public string Available_Qty { get; set; }
        public string Rework_Qty { get; set; }
        public string Newbatch_No { get; set; }
        public string NewExpiryDate_Flag { get; set; }
        public string NewExpiryDate { get; set; }
        
        public string Shopfloor { get; set; }
        public string Shopfloor_Id { get; set; }
        public string Shopfloor_Name { get; set; }
        public List<Shopfloorlist> ShopfloorNamelist { get; set; }
        public string ddl_Workstation { get; set; }
        public string WorkstationID { get; set; }
        public string WorkstationName { get; set; }
        public List<WorkstationName> WorkstationNameList { get; set; }
        public string ddl_shift { get; set; }
        public string ddl_shiftName { get; set; }
        public List<shift> shiftList { get; set; }
        public string supervisor_name { get; set; }
        public string sub_item { get; set; }
        public string subitemHFlag { get; set; }
        public string SubItemDetailsDt { get; set; }
        

        /*---Material Section DEtail Start*/
        public string MaterialID { get; set; }
        public string MaterialName { get; set; }
        public string MaterialType { get; set; }
        public string MaterialTypID { get; set; }
        
        public string MaterialUOM_ID { get; set; }
        public string MaterialUOM_Name { get; set; }
        public string MaterialUOMname { get; set; }
        public string RequiredQty { get; set; }
        public string sub_itemReqMtrl { get; set; }
        public string subitemReqMtrlFlag { get; set; }
        public string subitemConsMtrlFlag { get; set; }
        
        /*---Material Section DEtail End*/
        public string Create_id { get; set; }
        public string Created_by { get; set; }
        public string Created_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string StatusName { get; set; }
        public string Message { get; set; }
        public string TransType { get; set; }
        public Boolean Cancelled { get; set; }
        public Boolean JobCompletion { get; set; }
        public string hdnJobCompletion { get; set; }
        public string Command { get; set; }
        public string AppStatus { get; set; }
        public string BtnName { get; set; }
        public string Status_Code { get; set; }
        public string ItemReworkQtyDetail { get; set; }
        public string MaterialRequireddetails { get; set; }
        public string ConsumeMaterialdetails { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string WF_Status1 { get; set; }
        
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string attatchmentdetail { get; set; }
       
        public string TranstypAttach { get; set; }
        public string DocNoAttach { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string ListFilterData1 { get; set; }
        public string CMItemBatchWiseDetail { get; set; }
        public string CMItemSerialWiseDetail { get; set; }
        public string ItemID { get; set; }
        public List<ItemName1> ItemNameList1 { get; set; }
    }
    public class ItemName
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class ItemName1
    {
        public string Item_Id { get; set; }
        public string Item_Name { get; set; }
    }
    public class SearchItem
    {
        public string SearchName { get; set; }
    }
    public class wh_namelist
    {
        public string WareH_id { get; set; }
        public string wareH_name { get; set; }
    }
    public class Shopfloorlist
    {
        public string shflr_id { get; set; }
        public string shflr_name { get; set; }
    }
    public class shift
    {
       public string id { get; set; }
        public string name { get; set; }
    }
    public class WorkstationName
    {
        public string ws_id { get; set; }
        public string ws_name { get; set; }
    }
    public class RJODetailsattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    public class RJOListModel
    {
        public string RJOSearch { get; set; }
        public string WF_Status { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
       public string Item_Id { get; set; }
        public string Item_Name { get; set; }
        public string ItemName { get; set; }
        public List<ItemNameList> ItemNameLlist { get; set; }
        
        public string ItemID { get; set; }
        public string FromDate { get; set; }
        public string ListFilterData { get; set; }
        public string ToDate { get; set; }
        
        public List<RJOList> ReworkJobOrdrList { get; set; }
        public List<Status> StatusList { get; set; }
        //public String Spp_Name { get; set; }

    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }

    public class ItemNameList
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }


    public class RJOList
    {
        public string FinStDt { get; set; }
        public string RJONumber { get; set; }
        public string RJODate { get; set; }
        public string RJO_Dt { get; set; }
        public string ItemName { get; set; }
        public string ItemID { get; set; }
        public string Uom { get; set; }
        public string RewrkQty { get; set; }
        public string RJO_Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedON { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }

    }

}

