using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.JobOrder
{
     public class JobOrderModel
    {
        public string hdnsaveApprovebtn { get; set; }
        public string Title { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentMenuId { get; set; }
        // Start for Direct Job Order
        public string SourceType { get; set; }
        public string JOOrderQty { get; set; }
        public string ItmWeight { get; set; }
        public string Fproduct_id { get; set; }
        public string Fproduct_name { get; set; }
        public string decimalAllowed { get; set; }
        
        public string sub_item { get; set; }
        public string subitemHFlag { get; set; }
        // End for Direct Job Order
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        
        public List<ProdOrdNoList> prdordNoLists { get; set; }
        public string ForAmmendendBtn { get; set; }
        public string Amendment { get; set; }
        public string Amend { get; set; }
        public string wfDisableAmnd { get; set; }
        public string DeleteCommand { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string JOItemName { get; set; }
       
        public string SuppPage { get; set; }
        public string BrchID { get; set; }
        public string MAppLevel { get; set; }

        public string JOOrderType { get; set; }
        public string FinishProduct { get; set; }
        public string FinishProductId { get; set; }
        public string FinishUom { get; set; }
        public string FinishUomId { get; set; }
        
        public string Operation_Name { get; set; }
        public string OpId { get; set; }

        public string ProducOrdNum { get; set; }
        public string HdnProducOrdNum { get; set; }
        public string ProducOrdDate { get; set; }
        public string ProducOrd_Num { get; set; }
        
        public Boolean FClosed { get; set; }
        public Boolean Cancelled { get; set; }
        public string JO_No { get; set; }
        public string JO_Date { get; set; }
        //public string Src_Type { get; set; }
        public string SrcDocNo { get; set; }
        public string SrcDocDate { get; set; }
        public string JO_ItemName { get; set; }
        
        public string Currency { get; set; }
        public string Conv_Rate { get; set; }
        public string ValidUpto { get; set; }
        public string ImpFileNo { get; set; }
        public string CntryOrigin { get; set; }
        public string Remarks { get; set; }
        public string CompID { get; set; }
        public string BranchID { get; set; }
        public string UserID { get; set; }
        public string GrVal { get; set; }
        public string AssessableVal { get; set; }
        public string DiscAmt { get; set; }
        public string TaxAmt { get; set; }
        public string OcAmt { get; set; }
        public string NetValBs { get; set; }
        public string NetValSpec { get; set; }
        public string JobOrdStatus { get; set; }
        public string Address { get; set; }
        public int bill_add_id { get; set; }
        public string Ship_Gst_number { get; set; }
        public string Ship_StateCode { get; set; }
        public string SystemDetail { get; set; }
        //public string wfdocid { get; set; }
        //public string wfstatus { get; set; }
        public string SubItemDetailsDt { get; set; }
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
        public string Command { get; set; }
        public string AppStatus { get; set; }
        public string BtnName { get; set; }

        public string TitleAttach { get; set; }
        public string TranstypAttach { get; set; }
        public string DocNoAttach { get; set; }

        // public string Status_Code { get; set; }
        public string Itemdetails { get; set; }
        public string ServcOrdQty { get; set; }
        public string OutputItemdetails { get; set; }
        public string InputItemdetails { get; set; }
        public string ItemTaxdetails { get; set; }
        public string ItemOCdetails { get; set; }
        public string ItemDelSchdetails { get; set; }
        public string ItemTermsdetails { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string attatchmentdetail { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string ListFilterData1 { get; set; }
        public string ItemType { get; set; }
        public decimal OrderQty { get; set; }
        public decimal DispatchQty { get; set; }
        public string  WF_Status1 { get; set; }
        public string ShowProdDesc { get; set; } = "Y";
        public string ShowProdTechDesc { get; set; } = "N";
        public string ShowSubItem { get; set; } = "N";
        public string ShowCustSpecProdDesc { get; set; } = "N";
        public string PrintFormat { get; set; } = "F1";

    }
    public class JobOrderDetailsattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
   
    public class ProdOrdNoList
    {
        public string ProdOrdnoId { get; set; }
        public string ProdOrdnoVal { get; set; }
    }
    public class JOListModel
    {
        public string FinStDt { get; set; }
        public string JOSearch { get; set; }
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
        public List<SupplierName> SupplierNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<JobOrderList> JobOrdList { get; set; }
    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }
    public class CurrentDetail
    {
        public string CurrentUser { get; set; }
        public string CurrentDT { get; set; }
    }
    public class JobOrderList
    {
        public string SourceType { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string OrderDt { get; set; }
        public string SuppName { get; set; }
        public string Valid_Upto { get; set; }
        public string ProductName { get; set; }
        public string FgItemName { get; set; }
        public string FgUomName { get; set; }
        public string UOM { get; set; }
        public string Quantity { get; set; }
        public string ProductionOrdNo { get; set; }
        public string ProductionOrdDate { get; set; }
        public string OprationName { get; set; }
        public string JO_Status { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
    }
}

