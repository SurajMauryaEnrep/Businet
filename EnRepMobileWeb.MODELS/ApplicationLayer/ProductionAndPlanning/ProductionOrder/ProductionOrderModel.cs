using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionOrder
{
    public class ProductionOrderModel
    {
        public Boolean AutoMRS { get; set; }
        public string HdnMrsDeatilData { get; set; }
        public string MRSDate { get; set; }
        public string MRSNumber { get; set; }
        public string hdnautomrs { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string PrducOrderType { get; set; }
        public Boolean ForceClose { get; set; }
        public Boolean CancelFlag { get; set; }
        public string jc_no { get; set; }
        public string jc_dt { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string op_output_itemid { get; set; }
        public string op_output_item_name { get; set; }
        public string op_output_uom_id { get; set; }
        public string op_output_uom_Name { get; set; }
        public string op_output_UomName { get; set; }
        public string hdn_rev_no { get; set; }
        public string uom_id { get; set; }
        public string uom_Name { get; set; }
        public string UOMName { get; set; }

        public string adviceno { get; set; }
        public string hdnadviceno { get; set; }
        public string advicedt { get; set; }
        public string hdnadvicedt { get; set; }
        public int jc_qty { get; set; }
        public string sub_item { get; set; }
        // public string rev_no { get; set; }
        //public string op_id { get; set; }
        //public string shfl_id { get; set; }
        //public string ws_id { get; set; }
        public string batch_no { get; set; }
        public string supervisor_name { get; set; }
        public string jc_st_date { get; set; }
        public string jc_en_date { get; set; }
        //public string shift_id { get; set; }

        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string create_id { get; set; }
        public string create_dt { get; set; }
        public string app_id { get; set; }
        public string app_dt { get; set; }
        public string mod_id { get; set; }
        public string mod_dt { get; set; }
        public string jc_status { get; set; }
        public string mac_id { get; set; }
        public string DeleteCommand { get; set; }
        public string TransType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string Status { get; set; }
        public List<statusLists> statusLists { get; set; }
        public string Title { get; set; }
        public string SO_ItemName { get; set; }
        public string ddl_ShopfloorName { get; set; }
        public string ListFilterData { get; set; }
        public List<ShopFloor> ShopFloorList { get; set; } = new List<ShopFloor> { new ShopFloor { shfl_id = "0", shfl_name = "---Select---" } };
        //public string ddl_WorkstationName { get; set; }
        //public List<WorkStation> WorkstationList { get; set; }
        public string JCItemdetails { get; set; }
        public string JCItemdetails1 { get; set; }
        public List<JC_Item_details> JC_Item_Details_List { get; set; }
        public string ddl_RevisionNumber { get; set; }
        public List<RevisionNumber> RevisionNumberList { get; set; } = new List<RevisionNumber> { new RevisionNumber { rev_no = "0", rev_text = "0" } };
        public string ddl_OperationName { get; set; }
        public string OP_Name { get; set; }
        public string ddl_op_id { get; set; }
        public List<OperationName> OperationNameList { get; set; } = new List<OperationName> { new OperationName { op_id = "0", op_name = "---Select---" } };
        public string ddl_WorkstationName { get; set; }
        public string ddl_WorkstationText { get; set; }
        public List<WorkstationName> WorkstationNameList { get; set; } = new List<WorkstationName> { new WorkstationName { ws_id = "0", ws_name = "---Select---" } };
        public string ddl_shift { get; set; }
        public string ddl_shiftName { get; set; }
        
        public List<shift> shiftList { get; set; }
        public string SourceType { get; set; }
        public string ListFilterData1 { get; set; }
        public string txtFromdate { get; set; }
        public string txtToDate { get; set; }
        public List<jc_search> jc_searchList { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string ReceiptNumber { get; set; }
        public string ReceiptDate { get; set; }
        public string OrderedQuantity { get; set; }
        public string ActualStartDateAndTime { get; set; }
        public string ActualEndDateAndTime { get; set; }
        public string ProductionHours { get; set; }
        public List<AdviceNumber> AdviceNumberList { get; set; } = new List<AdviceNumber> { new AdviceNumber { advice_dt = "0", advice_no = "---Select---" } };
        public string HdnProductionSchDetail { get; set; }
        public string attatchmentdetail { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string DocumentStatus { get; set; }
        public string BtnName { get; set; }
        public string WF_Status { get; set; }
        public string WF_Status1 { get; set; }
        public string PO_No { get; set; }
        public string PO_dt { get; set; }
        public string SaveUpd { get; set; }
        public string Guid { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
    }
    public class AttachMentModel
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class UrlModel
    {
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string PO_No { get; set; }
        public string PO_dt { get; set; }
        public string DMS { get; set; }//DMS DocumentMenuStatus
        public string Amend { get; set; }

    }
    public class statusLists
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }   
    public class shift
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class AdviceNumber
    {
        public string advice_no { get; set; }
        public string advice_dt { get; set; }
    }
    public class WorkstationName
    {
        public string ws_id { get; set; }
        public string ws_name { get; set; }
    }
    public class OperationName
    {
        public string op_id { get; set; }
        public string op_name { get; set; }
    }
    public class RevisionNumber
    {
        public string rev_no { get; set; }
        public string rev_text { get; set; }
    }
    public class ShopFloor
    {
        public string shfl_id { get; set; }
        public string shfl_name { get; set; }
    }
    
    public class JC_Item_details
    {
        //public string jc_no { get; set; }
        //public string jc_dt { get; set; }
        //public string item_id { get; set; }
        //public string uom_id { get; set; }
        //public string Item_type_id { get; set; }
        //public string req_qty { get; set; }
        //public string seq_no { get; set; }

       
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string uom_id { get; set; }
        public string uom_name { get; set; }
        public string sub_item { get; set; }
        public string Item_type_id { get; set; }
        public string Item_type_name { get; set; }
        public string req_qty { get; set; }
        public string avl_stock_shfl { get; set; }
        public string avl_stock_warehouse { get; set; }
        public string seq_no { get; set; }
        public string alt_fill { get; set; }

    }
    public class jc_search
    {
        public int count { get; set; }
        public string jc_no { get; set; }
        public string jc_dt { get; set; }
        public string item_name { get; set; }
        public string uom_alias { get; set; }
        public string jc_qty { get; set; }
        public string op_name { get; set; }
        public string shfl_name { get; set; }
        public string shfl_name1 { get; set; }
        public string supervisor_name { get; set; }
        public string status_name { get; set; }
        public string create_dt { get; set; }
        public string app_dt { get; set; }
        public string mod_dt { get; set; }
        public string jc_noHdn { get; set; }
    }
}
