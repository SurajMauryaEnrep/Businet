using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.DeliveryNote
{
   public class DeliveryNoteDetailSC_Model
    {
        public string hdnsaveApprovebtn { get; set; }
        public string WF_Status1 { get; set; }
        public string Title { get; set; }
        public string DocumentMenuId { get; set; }
        public string DocumentStatus { get; set; }
        public string DNSCSubitemFlag { get; set; }
        public string BtnName { get; set; }
        public string Command { get; set; }
        public string AppStatus { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public Boolean Cancelled { get; set; }
        public string DeleteCommand { get; set; }
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string AmendedBy { get; set; }
        public string AmendedOn { get; set; }
        public string Status { get; set; }
        public string WFBarStatus { get; set; }
        public string WFStatus { get; set; }
        public string Create_id { get; set; }
        public string A_Status { get; set; }
        public string A_Level { get; set; }
        public string A_Remarks { get; set; }
        public string Doc_status { get; set; }
        public string DNStatus { get; set; }
        public string JobOrdTyp { get; set; }
        public string DN_No { get; set; }
        public string DN_Dt { get; set; }
        public string Bill_date { get; set; }
        public string Bill_no { get; set; }
        public string Veh_no { get; set; }
        public string Veh_load { get; set; }
        public string Remarks { get; set; }
        public string SuppID { get; set; }
        public string SuppName { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public string JobOrdNum { get; set; }
        public string JobOrdDate { get; set; }
        public string JobOrd_Num { get; set; }
        public List<JobOrderNo> JobOrderNoList { get; set; }
        public string MDNo { get; set; }
        public string MDDate { get; set; }
        public string MD_Num { get; set; }
        public List<MaterialDispatchNo> MaterialDispatchNoList { get; set; }
        public string FinishProduct { get; set; }
        public string FinishProductId { get; set; }
        public string FinishUom { get; set; }
        public string FinishUomId { get; set; }
        public string DnItemdetails { get; set; }
        public string SubItemDetailsDt { get; set; }//For Sub-item
        public string SubItemByProductScrapDetailsDt { get; set; }//For ByProductScrap Sub-item
        public string ItemDispatchQtyDetail { get; set; }
        public string DnItemReturndetails { get; set; }
        public string TranstypAttach { get; set; }
        public string ListFilterData1 { get; set; }
        public string DocNoAttach { get; set; }
        public string attatchmentdetail { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string Pending_Qty { get; set; }
        public string VehicleNumber { get; set; }
        public string TransporterName { get; set; }
        public string FreightAmount { get; set; }
        public string GRNumber { get; set; }
        public DateTime? GRDate { get; set; }
        

    }
    public class BindRtrnItemList
    {
        public string SearchName { get; set; }
    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class JobOrderNo
    {
        public string JobOrdnoId { get; set; }
        public string JobOrdnoVal { get; set; }
    }
    public class MaterialDispatchNo
    {
        public string MdNoId { get; set; }
        public string MdNoVal { get; set; }
    }
    public class DocumentNumber
    {
        public string po_no { get; set; }
        public string po_dt { get; set; }

    }
    public class DNDetailsattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
        //public string AttachMentDetailItmStp { get; set; }
    }
    public class ItemDetails
    {
        public string item_id { get; set; }
        public string ord_qty { get; set; }
        public string base_uom_id { get; set; }
        public string ord_qty_base { get; set; }
        public string SourceDocumentNo { get; set; }
        public string SourceDocumentDate { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public string DispatchQty { get; set; }
        public string BilledQty { get; set; }
        public string RecievedQty { get; set; }
        public Boolean QCRequired { get; set; }
        public string AcceptedQty { get; set; }
        public string RejectedQty { get; set; }
        public string ReworkableQty { get; set; }
        public Boolean SampleRecieved { get; set; }
        public string it_remarks { get; set; }
    }


    public class DNListModel
    {
        public string DNSCSearch { get; set; }
        public string WF_Status { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
       // public string SourceType { get; set; }
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public string FromDate { get; set; }
        public string ListFilterData { get; set; }
        public string ToDate { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<DNSCList> DeliveryNoteSCList { get; set; }
        public List<Status> StatusList { get; set; }
        //public String Spp_Name { get; set; }

    }
    public class Status
    {
        public string status_id { get; set; }
        public string status_name { get; set; }
    }

    public class SupplierNameList
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }


    public class DNSCList
    {
        public string FinStDt { get; set; }
        public string DNNumber { get; set; }
        public string DNDate { get; set; }
        public string DN_Dt { get; set; }
        public string SuppName { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string JobOrdNo { get; set; }
        public string JobOrdDt { get; set; }
        public string Quantity { get; set; }
        public string DN_Status { get; set; }
        public string CreatedON { get; set; }
        public string ApprovedOn { get; set; }
        //public string Src_Doc_No { get; set; }
        //public string Src_Doc_Date { get; set; }

        //public string ModifiedOn { get; set; }
     
     
        
    }

}
