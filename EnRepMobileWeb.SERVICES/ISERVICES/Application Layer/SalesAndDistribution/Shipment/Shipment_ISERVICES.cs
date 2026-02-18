using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.Shipment;
namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.Shipment
{
    public interface Shipment_ISERVICES
    {
        Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID, string CustType);
        DataTable GetCustomer_List(string CompID, string Cust_Name, string BrchID, string CustType);
        DataSet GetAllData(string CompID, string Cust_Name, string BrchID, string CustType, string CustID, string Fromdate, string Todate, string Status, string DocumentMenuId, string UserID, string wfstatus,string Flag);
        DataSet getCustomerAddress(string comp_id, string CustID);
        DataSet getShipmentPackingList(string CompID, string BrchID, string Cust_id, string PackNumber);
        DataSet getDetailPckingListByPackNo(string CompID, string BrchID, string Pack_NO, string Pack_date,string DocumentMenuId);
        DataTable getcurr_Detail(string CompID, string BrchID, string Pack_NO, string Pack_date);
        DataSet ShipmentApprove(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string CompID, string br_id, string mac_id, string menuid, string A_Status, string A_Level, string A_Remarks);
        DataSet CheckSaleInvoiceAgainstShipment(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet ShipmentDelete(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string CompID, string br_id);
        string InsertUpdateShipment(DataTable ShipmentHeader, DataTable ShipmentItemDetails , DataTable ItemBatchDetails, DataTable ItemSerialDetails,DataTable dtSubItem, DataTable ShipmentAttachments,DataTable ShipmentTranspoterDetails);
        DataSet getDocumentStatus(string MenuDocumentId);
        DataSet GetShipmentListAll(string CompID, string BrchID,string UserID,string  wfstatus,string  DocumentMenuId, string FromDate, string ToDate);
        DataTable GetShipmentListByFilter(string CustID, DateTime Fromdate, DateTime Todate, string Status, string CompID, string BrchID, string DocumentMenuId);
        DataTable GetCustNameList(string CompId, string br_id, string CustomerName);
        DataSet getShipmentDetailByShipmentNo(string CompID, string BrchID ,string UserID, string ShipmentNumber, DateTime ShipmentDate,string DocumentMenuId);
        string getNextDocumentNumber(string CompID, string BrchID, string MenuDocumentId, string Prefix);
        DataSet ShipmentCancel(ShipmentDetail_MODEL _ShipmentDetail_MODEL, string CompID,string br_id, string mac_id, string DocMenuID);
        DataSet GetShipmentDeatilsForPrint(string CompId, string BrchID, string DocNo, string DocDate,string flag);
        DataSet Shipment_GetSubItemDetails(string CompID, string Br_id, string ItemId, string SrcDoc_no, string SrcDoc_dt, string doc_no, string doc_dt, string Flag,string DocumentMenuId);
        DataSet Shipment_GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt,string DocumentMenuId);
        DataTable PortOfLoadingList();
        DataTable PlOfReceiptByPreCarrierList();
        DataTable GetShipmentItemsToExportExcel(string compId, string brId, string shipmentNo, string shipmentDate);
        string GetCstmInvNo(string CompID, string BrchID);

        //DataSet GetAllGLDetails(DataTable GLDetail);
        //DataSet GetRoundOffGLDetails(string comp_id, string br_id);
        //DataSet GetWarehouseList(string CompId, string BrID);
        //string getWarehouseWiseItemStock(string CompID, string BrID, string Wh_ID, string ItemID, string LotID, string BatchNo);
        //DataSet getItemStockBatchWiseAfterInsert(string CompID, string BrID, string Sh_Type, string Sh_No, string Sh_Date, string ItemId);
        //DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        //DataSet getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        //DataSet getItemstockSerialWiseAfterInsert(string CompID, string BrID, string Sh_Type, string Sh_No, string Sh_Date, string ItemId);
        //DataSet getItemstockWarehouseWise(string ItemId, string CompId, string BranchId);
    }
}
