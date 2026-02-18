using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesReturn;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesReturn
{
   public interface SalesReturn_ISERVICES
    {
        Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID,string CustType, string src_type);
        DataSet GetAllData(string CompID, string Cust_Name, string BrchID,string CustType, string CustId, string Fromdate, string Todate, string Status, string wfstatus, string UserID, string DocumentMenuId);
        DataSet GetSalesInvoiceNo(string CompID, string BrchID, string CustomerId, string DocumentNumber,string Src_Type);
        DataSet GetSIItemDetail(string CompID, string BrchID, string SourDocumentNo,string src_type);
        DataSet GetShipmentItemDetail(string CompID, string BrchID, string ItemID, string ShipNumber, string SrcDocNumber, string RT_Status,string src_type);
        DataSet GetSalesInvoiceItemDetail(string CompID, string BrchID, string ItemID, string InvoiceNo, string ShipNumber,string src_type);
        DataSet GetTaxAmountDetail(string CompID, string BrchID, string ItmCode, string InvoiceNo, string ShipNumber,string ReturnQuantity,string src_type);

        
        String InsertSalesReturnDetail(DataTable SalesReturnHeader, DataTable SalesReturnItemDetails, DataTable SalesReturnLotBatchSerial, DataTable SalesReturnVoudetail, DataTable dtSubItem,DataTable CRCostCenterDetails, DataTable DtblTaxDetail, string Src_Type,string InvBillNumber,string InvBillDate, string Payment_term, string Delivery_term,DataTable DtblVouGLDetail,DataTable DtblOCDetail,DataTable DtblOCTaxDetail,string oc_amt);
        DataSet GetSalesReturnDetail(string Srt_no, string Srt_dt, string CompID, string BrchID, string UserID, string DocumentMenuId);
        DataSet GetStatusList(string MenuID);
        DataSet GetSalesReturnListAll(string CustId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string wfstatus, string UserID, string DocumentMenuId);
        DataSet SalesReturnCancel(SalesReturn_Model _SalesReturn_Model, string CompID, string br_id, string mac_id,string Src_Type);
        DataSet SalesReturnDelete(SalesReturn_Model _SalesReturn_Model, string CompID, string br_id);
        string SalesReturnApprove(string SRTNo, string SRTDate, string userid, string wf_status, string wf_level, string wf_remarks, string CnNarr, string comp_id, string br_id, string mac_id, string DocID,string JVNurr);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetRoundOffGLDetails(string comp_id, string br_id);
        DataSet SR_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag,string src_doc_no,string src_type);
        DataSet Shipment_GetSubItemDetails(string CompID, string Br_id, string ShipNo, string ItemId, string doc_no, string doc_dt, string Flag,string src_type);
        DataSet GetSubItemWhAvlstockDetails(string compID, string brchID, string wh_id, string item_id, string flag, string shNo, string shDt);
        DataSet GetSubItemDetailsFromSinv(string compID, string brchID, string pinvNo, string invNo, string invDt, string item_id,string src_type);
        DataSet SRT_GetSubItemDetailsAfterApprov(string compID, string brchID, string item_id, string doc_no, string doc_dt, string flag, string shNo, string shDt, string wh_id);
        //DataSet ShipmntSaleRtrn_GetSubItemDetails(string CompID, string Br_id, string ShipNo, string ItemId, string doc_no, string doc_dt/*, string Flag*/);

        DataSet GetSalesOrderDeatilsForPrint(string CompID, string BrchID, string SR_no, string SR_Date,string Src_Type);
        DataTable GetTransportDetails(string compId, string transId, string transType, string transMode);
    }
}
