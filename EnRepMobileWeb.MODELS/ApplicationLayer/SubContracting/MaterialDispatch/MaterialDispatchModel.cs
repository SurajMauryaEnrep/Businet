using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MaterialDispatch
{
   public class MaterialDispatchModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string WF_Status1 { get; set; }
        public string Title { get; set; }        
        public string DocumentMenuId { get; set; }
        public string DocumentStatus { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string AppStatus { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string StockItemWiseMessage { get; set; }
        public string DeleteCommand { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string StatusName { get; set; }
        public string MDStatus { get; set; }
        public string StatusCode { get; set; }
        public string Create_id { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        
        public string MDIssue_No { get; set; }
        public string MDIssue_Date { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<JobOrdNoList> jobordNoLists { get; set; }
        public string Address { get; set; }
        public int bill_add_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string JobOrdNum { get; set; }
        public string JobOrdDate { get; set; }
        public string JobOrd_Num { get; set; }
        public string JobOrderType { get; set; }
        public string FinishProduct { get; set; }
        public string FinishProductId { get; set; }
        public string FinishUom { get; set; }
        public string FinishUomId { get; set; }
        public string Operation_Name { get; set; }
        public string OpId { get; set; }
        public string Product_Name { get; set; }
        public string ProductId { get; set; }
        public string UOM_Name { get; set; }
        public string UomId { get; set; }
        public string Order_Qty { get; set; }
        public string MDFlag { get; set; }
        public string Pending_Qty { get; set; }
        public string Dispatch_Qty { get; set; }
        public string DispEWBNNumber { get; set; }
        public string Remarks { get; set; }
        public string MD_ItemName { get; set; }
        public string Itemdetails { get; set; }

        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string attatchmentdetail { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string sub_item { get; set; }
        public string SubItemDetailsDt { get; set; }
        public string Guid { get; set; }
        public string SystemDetail { get; set; }
        public string CompId { get; set; }
        public string BrchID { get; set; }
        public List<Warehouse> WarehouseList { get; set; }
        public string ItemBatchWiseDetail { get; set; }
        public string ItemSerialWiseDetail { get; set; }
        public string MaterialIssueItemDetails { get; set; }
        public bool Cancelled { get; set; }
        public string ListFilterData1 { get; set; }
        /*--------------For Print------------------*/
        public string ShowProdDesc { get; set; } = "Y";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string PrintFormat { get; set; } = "F1";
        /*--------------For Transporter Detail------------------*/
        public string GRNumber { get; set; }
        public DateTime? GRDate { get; set; }
        public string FreightAmount { get; set; }
        public string TransporterName { get; set; }
        public string VehicleNumber { get; set; }
        public string veh_load { get; set; }

    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class JobOrdNoList
    {
        public string JobOrdnoId { get; set; }
        public string JobOrdnoVal { get; set; }
    }

   
   
   
    public class Warehouse
    {
        public string wh_id { get; set; }
        public string wh_name { get; set; }
    }

    public class MDListModel
    {
        public string FinStDt { get; set; }
        public string WF_Status { get; set; }
        public string Title { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public string Product_id { get; set; }
        public string FinishProdct_Id { get; set; }
        public string ListFilterData { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string wfdocid { get; set; }
        public string wfstatus { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string attatchmentdetail { get; set; }     
        public string MDSearch { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<MaterialDisList> MaterialDispatchList { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
   
    public class MaterialDisList
    {
      
      
        public string MDIssueNo { get; set; }
        public string MDIssueDate { get; set; }
        public string MDIssueDt { get; set; }
        public string SuppName { get; set; }
        public string ProductName { get; set; }
        public string UOM { get; set; }
        //public string Quantity { get; set; }
        public string JobOrdNo { get; set; }
        public string JobOrdDate { get; set; }
        public string FItemName { get; set; }
        public string FUOM { get; set; }
        public string OprationName { get; set; }
        public string MD_Status { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string FinStDt { get; set; }
    }



}
