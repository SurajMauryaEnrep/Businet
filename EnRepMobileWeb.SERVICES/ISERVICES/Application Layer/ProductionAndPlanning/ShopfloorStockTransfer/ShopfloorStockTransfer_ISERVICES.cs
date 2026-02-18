using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ShopfloorStockTransfer
{
    public interface ShopfloorStockTransfer_ISERVICES
    {
        DataSet Delete_ShopfloorStockTransfer(string comp_id, string br_id, string trf_No, string trf_Date);
        string Approve_ShopfloorStockTransfer(string comp_id, string br_id, string DocumentMenuID, string trf_no, string trf_date, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet GetSourceAndDestinationList(string CompId,string BrID,string TransferType,string MaterialType);
        DataSet getItemStockBatchWise(string ItemId, string ShflID, string CompId, string BranchId, string MaterialType);
        DataSet getItemStockBatchWiseAfterSavedDetail(string ItemId, string trf_no, string trf_dt, string CompId, string BranchId);
        DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string ShflId);
        DataSet getItemstockSerialWiseAfterSavedDetail(string CompId, string BranchId, string trf_no, string trf_dt);
        DataSet GetSHFLTransferList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId,
            string FromDate, string ToDate);
        DataTable GetSHFLTransferListFiltered(string transferType, string materialType, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId);
        DataSet GetShopfloorStockTransferDetailByNo(string CompID, string BrchID, string trf_No, string trf_Date, string UserID, string DocumentMenuId);
        string InsertUpdate_ShopfloorStockTransfer(DataTable trff_HeaderDetails, DataTable trf_ItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails,DataTable dtSubItem);
        DataSet TRF_GetSubItemDetails(string CompID, string Br_id, string ItemId, string trf_no, string trf_dt, string Flag);
        DataSet GetShflStkTrfDeatilsForPrint(string CompID, string BrchID, string TRFNo, string TRFDate);
    }
}
