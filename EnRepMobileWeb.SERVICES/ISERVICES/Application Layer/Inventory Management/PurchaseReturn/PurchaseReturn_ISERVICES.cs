using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.PurchaseReturn;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.PurchaseReturn
{
   public interface PurchaseReturn_ISERVICES
    {
        Dictionary<string, string> AutoGetSupplierListALl(string CompID, string SuppName, string BrchID);
       DataSet GetAllData(string CompID, string SuppName, string BrchID,
           string SuppId, string Fromdate, string Todate, string Status, string wfstatus, string UserID, string DocumentMenuId);
        DataSet GetPurchaseInvoiceNo(string CompID, string BrchID, string SupplierId, string DocumentNumber,string Src_Type);
        DataSet GetPIItemDetail(string CompID, string BrchID, string SourDocumentNo,string src_type);
        DataSet GetGRNItemDetail(string CompID, string BrchID, string ItemID, string GRNNumber,string SrcDocNumber, string RT_Status,string src_type);
        DataSet GetInvoiceItemDetail(string CompID, string BrchID, string ItemID, string InvoiceNo, string GRNNumber, string src_type);
        String InsertPurchaseReturnDetail(DataTable PurchaseReturnHeader, DataTable PurchaseReturnItemDetails
            , DataTable PurchaseReturnLotBatchSerial, DataTable PurchaseReturnVoudetail, DataTable dtSubItem
            , DataTable PRCostCenterDetails,DataTable DtblTaxDetail, DataTable DtblOCDetail, DataTable DtblOCTaxDetail, string src_type,string AdHocBill_no,string AdHocBill_dt, string oc_amt);
        DataSet GetPurchaseReturnDetail(string Prt_no, string Prt_dt, string CompID, string BrchID, string UserID, string DocumentMenuId);
        DataSet GetStatusList(string MenuID);
        DataSet GetPurchaseReturnListAll(string SuppId, string Fromdate, string Todate, string Status, string CompID, string BrchID, string wfstatus, string UserID, string DocumentMenuId);
        DataSet PurchaseReturnCancel(PurchaseReturn_Model _PurchaseReturn_Model, string CompID, string br_id, string mac_id);
        DataSet PurchaseReturnDelete(PurchaseReturn_Model _PurchaseReturn_Model, string CompID, string br_id);
        string PurchaseReturnApprove(string PRTNo, string PRTDate, string userid, string wf_status, string wf_level, string wf_remarks, string DnNarr, string comp_id, string br_id, string mac_id, string DocID,string src_type);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetRoundOffGLDetails(string comp_id, string br_id);
        DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string flag, string GRNNo, string GRNDt,string src_type,int UOMID);
        DataSet PRT_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string flag, string GRNNo, string GRNDt, string wh_id,string src_type);
        DataSet GetSubItemDetailsFromPinv(string CompID, string BrchID, string PinvNo, string GRNNo, string GRNDt, string Item_id,string src_type);
        DataSet GetPurchaseReturnDeatils(string Comp_ID, string Br_ID, string prt_no, string prt_dt,string src_type);
        DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId,string Doclist);
        DataSet getItemStockBatchWiseAfterInsert(string CompID, string BrID, string PL_No, string PL_Date, string ItemId);
        DataSet GetTaxAmountDetail(string CompID, string BrchID, string ItmCode, string InvoiceNo, string ShipNumber, string ReturnQuantity, string src_type,DataTable GlTaxDetailInsight);
    }

}
