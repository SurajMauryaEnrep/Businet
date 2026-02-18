using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ReworkableJobOrder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ReworkableJobOrder
{
   public interface ReworkableJobOrder_ISERVICES
    {
        DataSet AllDDLBind_OnPageLOad(string CompID, string BrID, string SearchName);
        DataSet GetWorkStationDAL(string comp_id, string br_id, int shfl_id);//Bind work station in ddl
        DataSet GetRewrkWHAvalStk(string CompID, string BrchID,string ItemID, string WarehouseID,string src_type,string accodian_type);
        DataSet GetNewBatchNo(string CompID, string BrchID);
        DataSet GetMaterialNameByMtrlTyp(string CompID, string BrchID, string ddl_MaterialTyp);
        DataSet GetItemUOM( string CompId, string br_id, string MaterialID);
        DataSet GetReworkQtyDetails(string CompID, string BrchID, string ItemID, string WHID,string src_type);
        DataSet getItemReworkQtyAfterInsert(string CompID, string BrID, string RJO_No, string RJO_Date, string ItemId, string WHID);
        DataSet GetSubItemWhAvlstockDetails(string comp_ID, string br_ID, string wh_id, string item_id, string UomId, string flag1,string flag);
        string InsertReworkJO_Details(DataTable DtblHDetail, DataTable DtblReqMatrlDetail, DataTable ReworkQtyItemDetails, DataTable DTAttachmentDetail, DataTable DtblConsumeMatrlDetail,DataTable CMItemBatchDetails, DataTable CMItemSerialDetails,string hdnJobCmplted, DataTable dtSubItem);
        DataSet GetRJOListandSrchDetail(string CompId, string BrchID, RJOListModel _RJOListModel, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetRewrkJODetailEditUpdate(string CompId, string BrchID, string JobCard_NO,string JobCard_Date,string UserID, string DocID);
        string RewrkJO_DeleteDetail(ReworkableJobOrder_Model _ReworkableJobOrder_Model, string CompId, string BrID);
        string RewrkJOApproveDetails(string CompID, string BrchID, string RJO_No, string RJO_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet RewrkJOCancel(ReworkableJobOrder_Model _ReworkableJobOrder_Mode, string CompID, string br_id, string mac_id);
        DataSet GetDetailsOfRequiredMaterialTbl(string Comp_ID, string Br_ID, string RJO_No, string RJO_Date,string ShopfloorId);
        DataSet getCMItemStockBatchSerialWise(string ItemId, string ShpfloorId, string CompId, string BranchId);
        DataSet getCMStockBatchWiseAfterInsert(string CompID, string BrID, string RJO_No, string RJO_Date, string MtrlTypId, string ItemId);
        DataSet getCMStockSerialWiseAfterInsert(string CompID, string BrID, string RJO_No, string RJO_Date, string MtrlTypId, string ItemId);
        DataSet ChkRJOagainstMRS(string CompID, string BrID, string RJONo, string RJODate);
        DataSet RJO_GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag,string Shfl_Id);
        DataSet QCAcptRejRewkQty_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet GetCMSubItemShflAvlstockDetails(string comp_ID, string br_ID, string doc_no, string doc_dt, string Shfl_Id, string item_id, string flag);
        //DataSet RJO_GetCMSubItemDetailsAfterSave(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet ItemList(string GroupName, string CompID, string BrchID, string ddl_MaterialTyp, string ddl_HedrItemId);
    }
}
