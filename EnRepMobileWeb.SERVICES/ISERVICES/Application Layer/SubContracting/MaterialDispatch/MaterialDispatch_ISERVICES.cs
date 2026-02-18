using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MaterialDispatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.MaterialDispatch
{
   public interface MaterialDispatch_ISERVICES
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);
        DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId);
        DataSet GetJobORDDocList(string Supp_IdNm, string Comp_ID, string Br_ID);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet getMaterialInputItemDetailByJONumber(string CompID, string BrchID, string JODate, string JONo, string OrdQty, string DispatchQty);
        DataSet getWarehouseWiseItemStock(string CompID, string BrID, string Wh_ID, string ItemID, string JONo, string JODate, string OrdQty, string DispatchQty, string UomId, string LotID, string BatchNo);

        DataSet GetDetailofJobOrdNoList(string CompID, string BrchID, string JobordNo, string JobOrddate, string Disp_No);

        string InsertMD_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable dtSubItem);
        DataSet GetMDListandSrchDetail(string CompId, string BrchID, MDListModel _MDListModel, string UserID, string wfstatus, string DocumentMenuId);
        DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName);//Bind product name

        DataSet GetMDDetailEditUpdate(string CompId, string BrchID, string MDSC_NO, string MDSC_Date, string UserID, string DocID);
        string MD_DeleteDetail(MaterialDispatchModel _MaterialDispatchModel, string CompId, string BrID);
        string MDApproveDetails(string CompID, string BrchID, string MDNo, string MDDate, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks);
        string MDCancel(MaterialDispatchModel _MaterialDispatchModel, string CompID, string br_id, string mac_id);
        DataSet ChkDNSCDagainstMDSC(string CompID, string BrID, string MDNo, string MDDate);
        DataSet getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        DataSet getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string IssueNo, string IssueDate, string ItemID);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string IssueNo, string IssueDate, string ItemID);
        DataSet GetMaterialDispatchDeatils(string Comp_ID, string Br_ID, string OrderNo, string OrderDate);
        DataSet MD_GetSubItemDetails(string CompID, string br_id, string Item_id, string JobOrdNo, string JobOrdDt, string Flag, 
            string OrdQty, string DispatchQty,string Wh_id, string flagwh, string DocNo, string Status);
        DataSet MD_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string DocNo, string DocDt, string JobOrdNo, string JobOrdDt, string Flag,
            string OrdQty, string DispatchQty, string Wh_id, string flagwh,string JOTyp);

    }

}
