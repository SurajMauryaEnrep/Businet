using System.Collections.Generic;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ServicePurchaseOrder
{
    public interface SPO_ISERVICE
    {
        Dictionary<string, string> GetSupplierListDAL(string CompID, string SuppName, string BranchID, string SuppType);
        Dictionary<string, string> GetPOItemListDAL(string CompID, string BrID, string ItmName);
        DataSet GetPOItmListDAL(string CompID, string BrID, string ItmName);
        DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId);
        DataSet CheckDeliveryNoteDPO(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetPOItemDetailDAL(string ItemID, string CompId);
        //DataSet GetPODetailDAL(string CompId, string BrID, string PONo, string PODate);
        DataSet GetSPODetailDAL(string CompId, string BrID, string PONo, string PODate, string UserID, string DocID);
     
        DataSet GetPOItemUOMDAL(string Item_id, string CompId);
        DataSet GetsuppcurrDAL(string CompId);
       
        string SPO_Delete(string CompID, string BrID, string SPONo, string SPODate);
        string InsertSPOApproveDetail(string PONo, string PODate, string Branch, string MenuID, string CompID, string ApproveID, string mac_id, string wf_status, string wf_level, string wf_remarks);
        string InsertSPODetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail, DataTable DTOCDetail, DataTable DTDeliSchDetail, DataTable DTTermsDetail, DataTable DTAttachmentDetail);
        string InsertForwardDetails(string compid, string brid, string docid, string docno, string docdate, string status, string forwarededto, string forwardedby, string level, string remarks);
        DataTable GetSuppAddressdetail(string SuppID, string CompId);
        DataSet GetItemSupplierInfo(string ItemID, string SuppID, string CompId);
        DataSet GetSourceDocList(string Comp_ID, string Br_ID, string Flag, string SuppID);
        DataSet GetDetailsAgainstQuotationOrPR(string Comp_ID, string Br_ID, string Doc_no, string Doc_date);
        DataSet GetPurchaseOrderDeatils(string Comp_ID, string Br_ID, string OrderNo, string OrderDate);
        DataSet CheckLPOQty_ForceClosed(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet GetPOAttatchDetailEdit(string CompID, string BrchID, string PO_No, string PO_Date);
        DataSet GetSuppAddrDetail(string Supp_id, string CompId);
        DataSet GetStatusList(string MenuID);
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet GetAllData(string CompID, string SuppName, string BranchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetSPODetailList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus);
        DataSet GetPO_Detail(string CompId, string BrID, string PONo, string PODate);
        DataSet GetPOTrackingDetail(string CompId, string BrID, string PONo, string PODate);
        DataSet GetWFLevel_Detail(string CompId, string BrID, string PONo, string PODate, string DocID);
        DataSet GetSPOTrackingDetail(string CompId, string BrID, string SPONo, string SPODate);
        DataSet GetSourceDocList(string Comp_ID, string Br_ID);
    }    
}
