using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*----------------THIS PAGE IS CREATED BY HINA SHARMA ON 13-02-2025----------------------------------*/
namespace EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ImportPurchase.ImportTracking
{
  public  class ImportTracking_Model
    {
        public string CompID { get; set; }
        public string BrchID { get; set; }
        public string UserID { get; set; }
        public string DocumentMenuId { get; set; }
        public string Title { get; set; }
        public string ImpTrckSearch { get; set; }
        //public string SuppPage { get; set; }
        /*----------Search Section Start------------------------*/
        public string SuppName { get; set; }
        public string SuppID { get; set; }
        public List<SupplierName> SupplierNameList { get; set; }
        public List<SrcDocNoList> DocNoLists { get; set; }
        public string PO_No { get; set; }
        public string PO_Date { get; set; }
        public string SrcDocNo { get; set; }
        public string SrcDocDate { get; set; }
        public string Currency { get; set; }
        public string Curr_id { get; set; }
        public string Trade_Term { get; set; }
        public string ImpFileNo { get; set; }
        public string CntryOrigin { get; set; }
        public string PortOrigin { get; set; }
        public string Remarks { get; set; }
        public string ListFilterData1 { get; set; }
       
        

        /*----------Search Section End------------------------*/
        /*------------Add New Section Start------------------------*/
        public string Date { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string Status { get; set; }
        public string NewDocumentDetail { get; set; }
        public string Combo_SrNoDate { get; set; }/*ForAttachment*/
        public List<ImpTrck_Document_Details> ImpTrck_Document_Details_List { get; set; }
        public string attatchmentdetail { get; set; }
        public DataTable AttachMentDetailItmStp { get; set; }
        public string Guid { get; set; }
        public string TblEditFlag { get; set; }
        public string EditSrNoforAttch { get; set; }
        


        /*------------Add New Section End------------------------*/
        public string Create_id { get; set; }
        public string Create_by { get; set; }
        public string Create_on { get; set; }
        public string Amended_by { get; set; }
        public string Amended_on { get; set; }
        public string Approved_by { get; set; }
        public string Approved_on { get; set; }
        public string StatusName { get; set; }
       
        public string SystemDetail { get; set; }
        public string Delete { get; set; }
        public string TransType { get; set; }
        public string Message { get; set; }
        public string Command { get; set; }
        public string BtnName { get; set; }
        public string DocumentStatus { get; set; }
        
        public string ValDigit { get; set; }
        public string QtyDigit { get; set; }
        public string RateDigit { get; set; }
        public string ExchDigit { get; set; }

    }
    public class SupplierName
    {
        public string supp_id { get; set; }
        public string supp_name { get; set; }
    }
    public class SrcDocNoList
    {
        public string SrcDocnoId { get; set; }
        public string SrcDocnoVal { get; set; }
    }
    public class URLModelDetails
    {
        public string TransType { get; set; }
        public string Command { get; set; }
        public string Src_DocDate { get; set; }
        public string BtnName { get; set; }
        public string Src_DocNo { get; set; }
    }
    public class ImpTrck_Document_Details
    {
        public string SNo { get; set; }
        public string Date { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string Status { get; set; }
        public string EntryFlag_List { get; set; }
        public string SrcDocPONo_List { get; set; }
    }
    public class ImpTrackModelattch
    {
        public DataTable AttachMentDetailItmStp { get; set; }
        public string attatchmentdetail { get; set; }
        public string Guid { get; set; }
    }
}
