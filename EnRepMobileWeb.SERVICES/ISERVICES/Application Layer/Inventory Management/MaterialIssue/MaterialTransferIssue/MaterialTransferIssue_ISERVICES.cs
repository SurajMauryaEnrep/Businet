using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue.MaterialTransferIssue;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue.MaterialTransferIssue
{
    public interface MaterialTransferIssue_ISERVICES
    {
        DataTable GetWhList(string CompId, string BrchID);
        DataTable GetToBranchList(string CompId);
        DataSet MTI_GetAllDDLListAndListPageData(string CompId, string BrchID, string flag, string startDate, string CurrentDate);
        DataTable GetToWhList(string CompId, string BrchID);
        DataSet GetMTODetailByNo(string CompID, string BrchID, string MTI_no, string MTI_date);
        DataSet getMTRNOList(string CompID, string BrchID, string MTRNo, string SourceBR, string SourceWH, string TransferType, string TOWH, string TOBR);
        DataSet GetMaterialTransferItemDetail(string CompID, string BrchID, string TRFDate, string TRFNo, string TRFType, string SessionBrchID);
        string InsertUpdateMaterialTransferIssue(DataTable MaterialTransferIssuetHeader, DataTable MaterialTransferIssueItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable dtSubItem);
        DataSet MaterialTransferIssueCancel(MaterialTransferIssueModel _MTIModel, string CompID, string br_id, string mac_id, string MTI_date, string MTI_Type);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID);
        DataSet CheckTransferRecieve(string CompID,string br_id,string txtMaterialIssueNo,string txtMaterialIssueDate);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string IssueType, string IssueNo, string IssueDate, string ItemID);
        DataSet MTI_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag);
        DataSet GetMaterialTransferIssuePrint(string CompID, string BrchID, string Doc_No, string Doc_dt);
        DataSet MTI_getItemstockWarehouseWise(string ItemId, string UomId, string CompId, string BranchId);


    }
    public interface MTIList_ISERVICES
    {
        DataTable GetMTIDetailList(string CompId, string BrchID, int To_WH, int To_BR, string Fromdate, string Todate, string TRF_Type, string Status);
        DataSet GetStatusList(string MenuID);
    }
}
