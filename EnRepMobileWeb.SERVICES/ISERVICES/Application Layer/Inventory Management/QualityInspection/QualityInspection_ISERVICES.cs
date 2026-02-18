using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.QualityInspection;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.QualityInspection
{
   public interface QualityInspection_ISERVICES
    {
        DataSet getQCInsSourceDocumentNo(string CompID, string BrchID, string DocumentNumber,string Src_type,string itemId,string supp_id);
        DataSet GetItemDetailBySourceDocumentNo(string CompID, string BrchID, string SourDocumentNo, string Src_Type);
        DataSet GetItemQCParamDetail(string CompID,string br_id, string ItemID,string qc_no,string qc_dt,string status);
        String InsertQualityInspectionDetail(DataTable QualityInspectionHeader, DataTable QualityInspectionItemDetail,
            DataTable QualityInspectionItemParamDetail, DataTable Attachments, DataTable QualityInspectionLotBatchDetails, DataTable dtSubItem);
        DataSet GetQcInspectionDetail(string CompID, string qc_no, string BrchID, string userid, string DocumentMenuId);
        DataTable GetRejectionReason(string Comp_ID, string Br_ID, string search);
        DataSet QCInspectionDelete(QualityInspectionModel _QualityInspectionModel, string comp_id, string br_id,string DocumentMenuId,string qc_no);

        DataSet QCInspectionApprove(QualityInspectionModel _QualityInspectionModel, string comp_id, string br_id, string mac_id, string DocumentMenuId,string Location_type);
        DataSet AfterApproveItemStockDetailBatchLotWise(QualityInspectionModel _QualityInspectionModel, string comp_id, string br_id);
        String QCInspectionCancel(QualityInspectionModel _QualityInspectionModel, string comp_id, string userid, string br_id, string mac_id);
        DataSet CheckGRNAgainstQC(string CompId, string BrchID, string DocNo, string DocDate);
        DataTable GetRejectWHList(string CompId, string BrchID);
        //DataTable GetSourceAndAcceptWHList(string CompId, string BrchID);
        DataSet GetSourceAndAcceptWHList(string CompId, string BrchID);
        DataTable GetReworkWHList(string CompId, string BrchID);
        DataSet GetBatchNo(string CompId, string BrID, string DocumentNumber);
        DataSet QC_GetSubItemDetails(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag);
        DataSet QCRWK_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag);
        DataSet QC_GetSubItemDetailsAftrApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag);
        DataSet GetItmListDL(string CompID, string BrID, string ItmName, string PageName,string SrcWh_Id,string LocationTyp,string suppid);

        DataSet GetQualityInspectionPrintDetails(string compId, string brId, string qcNo, string qcDate, string qcType);
        DataSet getItemStockBatchWiseofShopFloor(string CompId, string BranchId, string ItemId, string ShflID, string UOMId);
        DataSet AfterApproveItemStockDetailBatchLotWiseforShopfloor(QualityInspectionModel _QualityInspectionModel, string comp_id, string br_id);

    }
    public interface QualityInspectionList_ISERVICES
    {
        DataSet GetQCDetailList(string CompId, string BrchID, string Fromdate, string Todate, string QC_Type, string Status
            , string userid, string wfstatus, string DocumentMenuId, string ItemID);
        DataSet GetStatusList(string MenuID);
      

    }
}
