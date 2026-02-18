using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticPacking;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.DomesticPackingIServices
{
    public interface DomesticPacking_IServices
    {
        Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID,string CustType, string DocId);
        DataSet getPackingListSONo(string CompID, string BrchID, string Cust_id, string curr_Id);
        DataSet checkorderqtymorethenpackingqty(string CompID, string BrchID);
        DataSet getDetailByOrderNo(string CompID, string BrchID, string OrderNumber,string PackingNumber,string PackType,string DocumentMenuId);
        DataSet GetOrderQty(string CompID, string BrchID, string custtomerID, string ItemID, string PackType, string DocumentMenuId, string packingNo);
        String InsertUpdatePackingList(DataTable PackingListHeader, DataTable PackingListItemDetails
            , DataTable PackingListSoItemDetails, DataTable PackingListItemBatchDetails
            , DataTable PackingListOrdResItemBatchDetails, DataTable PackingListItemSerialDetails,
            DataTable PL_SerializationDetails,DataTable dtSubItem, DataTable dtSubItemRes, DataTable dtSubItemPackRes);
        string PackingListCancel(DomesticPackingDetail_Model _DomesticPackingDetail_Model, string comp_id, string userid, string br_id,string mac_id, string MenuDocid,string Amendment);
        DataSet PackingListDelete(DomesticPackingDetail_Model _DeliveryNoteDetail_MODELS, string comp_id, string br_id);
        DataSet GetPackingListAll(string CompID, string BrchID,string UserID,string wfstatus,string DocumentMenuId);
        DataSet GetStatusList(string MenuID);
        DataTable GetCustNameList(string CompId, string br_id, string CustomerName,string cus_typ, string DocId);
        DataSet GetAllData(string CompId, string br_id, string CustomerName,string cus_typ, string CustID, string Fromdate, string Todate, string Status, string DocumentMenuId,string UserID, string wfstatus );
        DataTable GetPackingListFilter(string CustID, DateTime Fromdate, DateTime Todate, string Status, string CompID, string BrchID, string DocumentMenuId);
        DataSet GetPackingListDetailByNo(string CompID, string pack_no, string BrchID,string UserID,string DocumentMenuId);
        string PackingListApprove(DomesticPackingDetail_Model _DeliveryNoteDetail_MODELS, string comp_id, string DocumentMenuID, string br_id, string mac_id,string  A_Status,string  A_Level,string  A_Remarks);
        DataSet CheckShipmentAgainstPackingList(string CompId, string BrchID, string DocNo, string DocDate);
        DataSet getPackagingItemDetails(string CompID, string ItemID);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet getItemStockBatchWiseAfterInsert(string CompID, string BrID, string Type, string PL_No, string PL_Date, string ItemId,string docid);
        DataSet getItemstockSerialWiseAfterInsert(string CompID, string BrID, string Pl_Type, string Pl_No, string Pl_Date, string ItemId);
        DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId, string Doclist);
        DataSet GetOrderResItemStockBatchWise(string CompId, string BranchId, string ItemId, string WarehouseId, string LotId, string BatchId, string Doclist);
        DataSet GetOrderResItemStockForSubItemBatchWise(string CompId, string BranchId, string ItemId, string WarehouseId
            , string Doclist, string status);
        DataSet GetOrderResItemStockBatchWiseAfterInsert(string CompId, string BranchId, string packtype, string Packno, string Packdt, string ItemId, string LotId, string BatchId);
        DataSet GetPackingListDeatilsForPrint(string CompId, string BrchID, string DocNo, string DocDate,string printFormet);
        DataSet GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string SoNo, string SoDate, string Flag);
        DataSet GetSubItemDetailsBySO(string CompID, string Br_id, string doc_no, string doc_dt, string SoNo,string SoDate,string ItemId,string pack_type);
        DataSet GetOrdrPendSubItemDetailsBySO(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt
            , string SoNo, string SoDate, string wh_id, string flag);

        //DataSet GetOrdrPendSubItemDetailsBySOAfterApprov(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string SoNo, string SoDate, string pack_type);
        DataTable GetPLDetailsToExportExcel(string action, string compId, string branchId, string documentMenuId, string userId, string packNo);
        DataTable GetCurrencies(string comp_ID, string currType);

        DataSet GetVerifiedDataOfExcel( string compId,string BrchID, DataTable  ExcelLevelData, DataTable PageLevelData );
    }

}
