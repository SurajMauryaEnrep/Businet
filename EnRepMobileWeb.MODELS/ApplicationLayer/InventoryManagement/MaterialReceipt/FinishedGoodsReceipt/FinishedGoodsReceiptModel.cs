using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.FinishedGoodsReceipt
{
    public class FinishedGoodsReceiptModelDetail
    {
        public Boolean CancelFlag { get; set; }
        public string Uom_id { get; set; }
        public string UomName { get; set; }
        public string Source_typeID { get; set; }
        public string Source_type { get; set; }
        public string DocumentStatus { get; set; }
        public string batch_Command { get; set; }
        public string Subitmflag { get; set; }
        public string create_id { get; set; }
        public string WFStatus { get; set; }
        public string WFBarStatus { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string InputItemDetail { get; set; }
        public string OutputItemDetail { get; set; }
        public string shopfloor_id { get; set; }
        public string operation_id { get; set; }
        public string StatusCode { get; set; }     
        public string mod_on { get; set; }
        public string mod_by { get; set; }
        public string app_on { get; set; }
        public string app_by { get; set; }
        public string create_on { get; set; }
        public string create_by { get; set; }     
        public string ListFilterData1 { get; set; }
        public string DeleteCommand { get; set; }
        public string Docid { get; set; }
        public string WF_Status1 { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string InputSubItemDetailsDt { get; set; }
        public string OutPutSubItemDetailsDt { get; set; }
        public string DocumentMenuID { get; set; }
        public string Remarks { get; set; }
        public string SuppervisorName { get; set; }
        public string A_Remarks { get; set; }
        public string A_Level { get; set; }
        public string A_Status { get; set; }
        public string RecieptDate { get; set; }
        public string RecieptNumber { get; set; }
        public string Shopfloor { get; set; }
        public string Operation { get; set; }
        public string Status { get; set; }
        public string Massage { get; set; }
        public string StockItemWiseMessage { get; set; }
        public string Command { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string BtnName { get; set; }
        public string DetailView { get; set; }
        public string Title { get; set; }
        public string Product_id { get; set; }
        public string ProductName { get; set; }
        public List<ProductName> productNameList { get; set; }
        public List<ShopfloorListDropDown> ShopfloorList { get; set; }
        public List<OperationListDropDown> OperationList { get; set; }    
    }
    public class UrlModel
    {
        public string Msg { get; set; }
        public string bt { get; set; }
        public string Cmd { get; set; }
        public string tp { get; set; }
        public string wf { get; set; }
        public string FGR_no { get; set; }
        public string FGR_dt { get; set; }
        public string DMS { get; set; }

    }
    public class FinishedGoodsReceiptModel
    {
        public string Product_id { get; set; }
        public string Source_type { get; set; }
        public string ProductName { get; set; }
        public string WF_Status { get; set; }
        public string FGRSearch { get; set; }
        public string ListFilterData { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DocumentMenuID { get; set; }
        public string Shopfloor { get; set; }
        public string Operation { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public List<ShopfloorListDropDown> ShopfloorList { get; set; }
        public List<ProductName> productNameList { get; set; }
        public List<OperationListDropDown> OperationList { get; set; }
        public List<StatusList> Status_list { get; set; }
    }
    public class ProductName
    {
        public string Item_id { get; set; }
        public string Item_name { get; set; }
        public string Uom_name { get; set; }
    }
    public class ShopfloorListDropDown
    {
        public string shop_id { get; set; }
        public string shop_name { get; set; }
    }
    public class OperationListDropDown
    {
        public string op_id { get; set; }
        public string op_name { get; set; }
    }
    public class StatusList
    {
        public string StatusID { get; set; }
        public string Status_Name { get; set; }
    }
}
