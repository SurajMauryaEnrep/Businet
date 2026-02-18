using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionConfirmation
{
    public class ProductionConfirmation_Model
    {

        public string op_output_UomName { get; set; }
        public string op_output_uom_id { get; set; }
        public string op_output_uom_Name { get; set; }
        public string op_output_itemid { get; set; }
        public string op_output_item_name { get; set; }
        public string ddl_shift { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Sub_item { get; set; }
        public string TransType_Modified { get; set; }
        public string batchCommand { get; set; }
        public string NotModifiedmsg { get; set; }
        public string errorcmd { get; set; }
        public string productname { get; set; }
        public string batch_Command { get; set; }
        public string Docid { get; set; }
        public string PC_Date { get; set; }
        public string PC_Number { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string DocumentStatus { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMessage { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string confirmation_no { get; set; }
        public string confirmation_dt { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string uom_id { get; set; }
        public string uom_name { get; set; }
        public string order_no { get; set; }
        public string hdn_order_no { get; set; }
        public string order_dt { get; set; }
        public string hdn_order_dt { get; set; }
        public string order_qty { get; set; }
        public string pending_qty { get; set; }
        public string produced_qty { get; set; }
        public string qc_acceptedqty { get; set; }
        public string qc_rejectedqty { get; set; }
        public string qc_reworkableqty { get; set; }
        public string operation { get; set; }
        public string operation_id { get; set; }
        public string shopfloor { get; set; }
        public string shopfloor_id { get; set; }
        //public string workstation { get; set; }/*Commented and add by Hina on 20-09-2024 to add dropdown instead of textbox*/
        //public string workstation_id { get; set; }
        public string ddl_OperationName { get; set; }
        public string OP_Name { get; set; }
        public string ddl_op_id { get; set; }
        public List<OperationName> OperationNameList { get; set; } = new List<OperationName> { new OperationName { op_id = "0", op_name = "---Select---" } };
        public string ddl_ShopfloorName { get; set; }
        
        public List<ShopFloor> ShopFloorList { get; set; } = new List<ShopFloor> { new ShopFloor { shfl_id = "0", shfl_name = "---Select---" } };

        public string ddl_WorkstationName { get; set; }
        public string ddl_WorkstationText { get; set; }
        public List<WorkstationName> WorkstationNameList { get; set; } = new List<WorkstationName> { new WorkstationName { ws_id = "0", ws_name = "---Select---" } };

        public string supervisorName { get; set; }
        public string batchno { get; set; }
        public string jobstartdate { get; set; }
        public string jobenddate { get; set; }
        public string hours { get; set; }
        public string shift { get; set; }
        public string shift_id { get; set; }
        public string Remks { get; set; }
        public string create_by { get; set; }
        public string create_on { get; set; }
        public string app_by { get; set; }
        public string app_on { get; set; }
        public string mod_by { get; set; }
        public string mod_on { get; set; }
        public string status { get; set; }
        public string StatusCode { get; set; }
        public string mac_id { get; set; }
        public string DeleteCommand { get; set; }
        public string TransType { get; set; }
        public string Title { get; set; }
        public string Subitmflag { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string ConsumptionItemDetail { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string OutputItemDetail { get; set; }
        public string txtFromdate { get; set; }
        public string txtToDate { get; set; }
        public Boolean CancelFlag { get; set; }
        public List<OrderNumber> OrderNumberList { get; set; }

        public string CNFSearch { get; set; }
        public string FromDate { get; set; }
        public string ddl_ProductName { get; set; }



        public string ToDate { get; set; }
        public string ListStatus { get; set; }
        public List<LStatus> StatusList { get; set; }

        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string ListFilterData1 { get; set; }
        public string ListFilterData { get; set; }
        public string create_id { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        public string WF_Status1 { get; set; }
        public string WF_Status { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public List<ProductName> ProductNameList { get; set; }
        public List<Shiftlist> ShiftdropdownlistP { get; set; }
        public string op_product_name { get; set; }
        public string c_item_name { get; set; }
    }
    public class ShopFloor
    {
        public string shfl_id { get; set; }
        public string shfl_name { get; set; }
    }
    public class OperationName
    {
        public string op_id { get; set; }
        public string op_name { get; set; }
    }
    public class WorkstationName
    {
        public string ws_id { get; set; }
        public string ws_name { get; set; }
    }
    public class Shiftlist
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class UrlModel
    {
        public string Msg { get; set; }
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string PAC_No { get; set; }
        public string PAC_dt { get; set; }
        public string DMS { get; set; }

    }
    public class Pro_Model
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
    public class OrderNumber
    {
        public string porder_no { get; set; }
        public string porder_dt { get; set; }
    }
    public class LStatus
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
   

    public class ProductName
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class SearchItem
    {
        public string SearchName { get; set; }
    }
}
