using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.OpeningMaterialReceipt
{
   public class OpeningMaterialReceiptModel
    {
        public string ImportExcelFileFlag { get; set; }
        public string Importwhid { get; set; }
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string OPR_Item { get; set; }
        public string warehouse_id { get; set; }
        public string MenuDocumentId { get; set; }   
        public string Status { get; set; }
        public string StatusCode { get; set; }      
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string comp_id { get; set; }
        public string br_id { get; set; }
        public string UserMacaddress { get; set; }
        public string UserSystemName { get; set; }
        public string UserIP { get; set; }
        public string TransType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string AmmendedBy { get; set; }
        public string AmmendedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string status_name { get; set; }
        public string op_status { get; set; }
        public string doc_status { get; set; }
        public Boolean CancelFlag { get; set; }
        public string WFBarStatus { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string PR_status { get; set; }
        public string Opening_dt { get; set; }
        public string opstk_rno { get; set; }
        public string wh_Name { get; set; }
        public string OpeningValue { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public string DeleteCommand { get; set; }
        public string SampleRcptItemDetail { get; set; }
        public string OPR_ItemDetail { get; set; }
        public string OPR_Val { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string wh_id { get; set; }
        public string OpFinSTdate { get; set; }
        public string BatchDetail { get; set; }
        public string SerialDetail { get; set; }
        public int fy_count { get; set; }
        public string DocumentStatus { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string Message { get; set; }
        public string ListFilterData1 { get; set; }
        public string WF_status1 { get; set; }
        public string AppStatus { get; set; }
        public string OPR_Date { get; set; }
    }
    public class URLModelDetails
    {
        public string TransType { get; set; }
        public string Command { get; set; }
        public string OPR_Date { get; set; }
        public string BtnName { get; set; }
        public string wh_id { get; set; }
        public string id { get; set; }
    }
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class ItemDetails
    {
        public string ItemID { get; set; }
        public string UOM { get; set; }
        public string ItemCost { get; set; }
        public string RecievedQty { get; set; }
        public string Warehouse { get; set; }
        public string LotNo { get; set; }
        public string Remarks { get; set; }
    }

    public class OpeningMaterialHeaderDetail
    {

        public string TransType { get; set; }
        public string MenuID { get; set; }
        public int wh_id { get; set; }
        public int comp_id { get; set; }
        public int br_id { get; set; }    
        public string op_dt { get; set; }       
        public int user_id { get; set; }
        public string op_status { get; set; }
        public string mac_id { get; set; }
        public string op_val { get; set; }      

    }
    public class OpeningMaterialItemDetail
    {
        public string item_id { get; set; }
        public int uom_id { get; set; }
        public string op_qty { get; set; }
        public string lot_id { get; set; }        
        public string item_rate { get; set; }
        public string op_val { get; set; }
    }
    public class OpeningMaterialItemBatchDetail
    {
        public string item_id { get; set; }
        public string batch_no { get; set; }
        public string batch_qty { get; set; }
        public string exp_dt { get; set; }
    }
    public class OpeningMaterialItemSerialDetail
    {
        public string item_id { get; set; }
        public string serial_no { get; set; }
    }

    public class OpeningMaterial_ListModel
    {
        public string Title { get; set; }    
        public string Status { get; set; }
        public string FromDate { get; set; }
        public DateTime ToDate { get; set; }    
        public List<Status> StatusList { get; set; }
        public List<OpeningMaterialList> OPR_List { get; set; }
        public List<OPYear>op_yearList { get; set; }
        public int fycount { get; set; }
        public string ListFilterData { get; set; }
        public string WF_status { get; set; }
        public string OMRSearch { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class OPYear
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class OpeningMaterialList
    {
        public string OPRDate { get; set; }
        public string OPR_DT { get; set; }
        public string wh_Name { get; set; }
        public string wh_id { get; set; }
        public string id { get; set; }
        public string Stauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string op_val { get; set; }

    }
}
