using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.BillofMaterial
{
   public class BillofMaterialModel
    {
        public List<ShopFloor> ShopFloorList { get; set; }// added By Nitesh 26-10-2023 for shopfloore dropdown
        public List<ReplicateWithList> ReplicateWithLists { get; set; }// added By Nitesh 26-10-2023 for replicate with  dropdown
        public string hdnsaveApprovebtn { get; set; }   // added By Nitesh 12-01-2024 for Disable Save And Approve Btn
        public string ReplicateWith { get; set; }   // added By Nitesh 26-10-2023 for replicate with  dropdown
        public string  ddl_ShopfloorName { get; set; }   // added By Nitesh 26-10-2023 for shopfloore dropdown
        public int comp_id { get; set; }
        public int br_id { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public int uom_id { get; set; }//
        public string uom_Name { get; set; }
        public int uomItemName_id { get; set; }
        public string qty { get; set; }
        public int rev_no { get; set; }
        public string bom_remarks { get; set; }
        public Boolean act_status { get; set; }
        public Boolean def_status { get; set; }
        public int create_id { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
       
        public string create_name { get; set; }
        public string create_dt { get; set; }
        public int app_id { get; set; }
        public string app_name { get; set; }
        public string app_dt { get; set; }
        public int mod_id { get; set; }
        public string mod_name { get; set; }
        public string mod_dt { get; set; }
        public string bom_status { get; set; }
        public string bom_status_Name { get; set; }
        public string mac_id { get; set; }
        public string DeleteCommand { get; set; }
        public string Title { get; set; }
        public string TransType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string Status { get; set; }
        public string SO_ItemName { get; set; }
        public string ItemNameDDl { get; set; }
        public string OperationNameStatus { get; set; }
        public List<OperationName> OperationNameList { get; set; }
        public int id_ItemName { get; set; }
        
        public string bomitemattr { get; set; }      
        public int op_product_id { get; set; }
        public int op_rev_no { get; set; }
        public int op_op_id { get; set; }
        public int op_Item_type { get; set; }
        public int op_item_id { get; set; }
        public int op_uom_id { get; set; }
        public int op_qty { get; set; }
        public int op_item_cost { get; set; }
        public int op_item_value { get; set; }
        public List<BOM_Item_Detail> BOM_Item_Detail_List { get; set; }
        public List<ProductName> productNameList { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string DocDate { get; set; }
        public string TransTypeBOM { get; set; }
        public string CommandBOM { get; set; }
        public string MessageBOM { get; set; }
        public string UsedInProducts { get; set; }
        public string ListFilterData1 { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        public string BOMSearch { get; set; }
        public string SaveUpd { get; set; }
        public string dbclick { get; set; }
        public string AppStatus { get; set; }
        public string WF_status1 { get; set; }
        public string ListFilterData { get; set; }
        public string WF_status { get; set; }
        public string hdnBomAltItemDetail { get; set; }
        public string statuslist { get; set; }
        public string Active { get; set; }
        public List<Status> statusLists { get; set; }
        public string GetBOMList { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class UrlModelData
    {
        public string TransTypeBOM { get; set; }
        public string CommandBOM { get; set; }
        public string BtnName { get; set; }
        public string product_id { get; set; }
        public int rev_no { get; set; }
        public string dbclick { get; set; }
    }
    //added ShopFloor class by Nitesh 26-10-2023 10:52 for bind shopfloore dropdown
    public class ShopFloor 
    {
        public string shfl_id { get; set; }
        public string shfl_name { get; set; }
    }
    //added ReplicateWithList by  Nitesh 26-10-2023 10:52 for bind replicate with dropdown
    public class ReplicateWithList
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string uom { get; set; }
    }
    public class OperationName
    {
        public string op_id { get; set; }
        public string op_name { get; set; }
    }
    public class ProductName
    {
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
    }
    public class BOM_Item_Detail
    {
        public string op_product_id { get; set; }
        public string op_product_name { get; set; }
        public int op_rev_no { get; set; }
        public int op_op_id { get; set; }
        public string op_op_name { get; set; }
        public string op_Item_type { get; set; }
        public string op_Item_type_name { get; set; }
        public string op_item_id { get; set; }
        public string op_item_name { get; set; }
        public int op_uom_id { get; set; }
        public string op_uom_name { get; set; }
        public string op_qty { get; set; }
        public string op_item_cost { get; set; }
        public string op_item_value { get; set; }
        public string alt_fill { get; set; }
        public int op_contain_row { get; set; }
        public int seq_no { get; set; }
    }
}
