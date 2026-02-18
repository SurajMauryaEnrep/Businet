using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.Assembly_Kit
{
   public class AssemblyKit_DetailModel
    {
        public Boolean CancelFlag { get; set; }
        public string WFBarStatus { get; set; }
        public string hdnAssemblyProductID { get; set; }
        public string ItemDetail { get; set; }
        public string WFStatus { get; set; }
        public string ListFilterData1 { get; set; }
        public string WF_Status1 { get; set; }
        public string attatchmentdetail { get; set; }
        public string SubItemDeatilINput { get; set; }
        public string batch_Command { get; set; }
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        public string Create_id { get; set; }
        public string Status_Code { get; set; }
        public string doc_status { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string Amended_on { get; set; }
        public string Amended_by { get; set; }
        public string Approved_on { get; set; }
        public string Approved_by { get; set; }
        public string Created_on { get; set; }
        public string Created_by { get; set; }
        public string DeleteCommand { get; set; }
        public string DocumentStatus { get; set; }
        public string BtnName { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMessage { get; set; }
        public string Command { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string A_Remarks { get; set; }
        public string A_Level { get; set; }
        public string A_Status { get; set; }
        public string DocumentDate { get; set; }
        public string DocumentNumber { get; set; }
        public string AssemblyUOMID { get; set; }
        public string AssemblyUOM { get; set; }
        public string AssemblyQuantity { get; set; }
        public string WHIDHeader { get; set; }
        public string Warehouse { get; set; }
        public string wh_idItemTable { get; set; }
        public string remarks { get; set; }
        public string AssemblyProduct { get; set; }
        public List<ItemName_List1> ItemNameList { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
    }

    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }
    public class ItemName_List1
    {
        public string Item_ID { get; set; }
        public string Item_Name { get; set; }
    }
    public class attchmentModel
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    public class UrlModel
    {
        public string Msg { get; set; }
        public string btn { get; set; }
        public string Cmd { get; set; }
        public string trnstyp { get; set; }
        public string wrkf { get; set; }
        public string Doc_no { get; set; }
        public string Doc_dt { get; set; }
        public string DMS { get; set; }

    }
}
