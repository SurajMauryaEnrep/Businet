using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue
{
    public interface MaterialIssue_IServices
    {
        DataSet GetAllDDLandListPageData(string CompId, string br_id, string flag
            , string startDate, string CurrentDate);
        DataSet GetIssuedByData(string CompID, string BrchID);
        DataSet GetRequirmentreaList(string CompId,string br_id, string flag);
        DataSet getMRSNOList(string CompID, string BrchID, string MRSNo, string Area, string RequisitionType);
        DataSet GetIssueToList(string CompId, string IssueTo, string BranchId);
        DataSet GetSuppAddrDetailDAL(string CompID, string BrchID, string MRSDate, string MRSNo, string mrs_type);
        DataSet GetMaterialRequisitionIssueTo(string CompID, string BrchID, string MRSDate, string MRSNo, string mrs_type);
        DataSet GetMaterialRequisitionItemDetailByNO(string CompID, string BrchID, string MRSDate, string MRSNo, string MRSType);
        string InsertUpdateMaterialIssue(DataTable MaterialIssuetHeader, DataTable MaterialIssueItemDetails
            , DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable dtSubItem);
        string getNextDocumentNumber(string CompID, string BrchID, string MenuDocumentId, string Prefix);
        DataSet getDocumentStatus(string MenuDocumentId);
        DataSet GettMaterialIssueListAll(string CompID, string br_id);
        //DataSet GetMaterialIssueToList(string CompID, string BranchId);
        DataTable GetMaterialIssueDetailByFilter(string CompID, string br_id, string RequisitionTyp, string RequiredArea, string MaterialIssueTo, string Fromdate, string Todate, string Status,string flag);
       // DataTable GetIssueDetailByFilter(string CompID, string br_id, string  issue_to,string Fromdate,string Todates);
        DataSet GetMatrialIssueDetailByNo(string CompID, string BrchID, string MIssue_type, string MIssue_no, string MIssue_date);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet GetWarehouseList1(string CompId, string BrID,string doc_id);
        DataSet getItemStockBatchWise(string ItemId,string UomId, string WarehouseId, string CompId, string BranchId);
        DataSet getItemStockBatchWisefromRwkJO(string ItemId,string UomId, string WarehouseId, string CompId, string BranchId,string MRSNo);
        DataSet getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId);
        DataSet getItemStockSerialWisefromRwkJO(string ItemId, string WarehouseId, string CompId, string BranchId, string MRSNo);

        Dictionary<string, string> IssueToList(string CompID, string Entity, string BrchID, string sr_type);
        string MaterialIssueCancel(MaterialIssue_Model _MaterialIssue_Model, string DocumentMenuId, string CompID, string br_id, string mac_id);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID,string UomId);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID);
        DataSet MI_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);

        DataSet GetMRSDeatilsForPrint(string CompID, string BrchID, string Doc_No, string Doc_dt,string IssueType);

        DataSet GetPendingDocumentData(string CompID, string BrchID, string Docid, string language
            , string ItemID, string flag);
        DataSet GetPendingDocumentDataitemdetail(string CompID, string BrchID, string doc_no, string doc_dt);

        DataSet checkDependency(string CompID, string BrchID, string issue_no, string issue_dt);

    }
}
