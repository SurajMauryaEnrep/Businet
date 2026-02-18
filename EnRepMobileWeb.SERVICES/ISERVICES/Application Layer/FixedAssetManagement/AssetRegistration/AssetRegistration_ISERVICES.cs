using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetRegistration;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetRegistration;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetRegistration
{
    public interface AssetRegistration_ISERVICES
    {
        Dictionary<string, string> GetAssetGroup(string CompID, string GroupId);
        Dictionary<string, string> GetAssetGroupListPage(string CompID, string GroupId);
        Dictionary<string, string> GetAssetCategory(string CompID, string GroupId);
        Dictionary<string, string> GetAssetCategory(string CompID);
        Dictionary<string, string> GetRequirmentArea(string CompId, string BrId);
        DataSet GetAssignedRequirementArea(string CompId, string BrId);
        DataSet GetSerialNoJs(string CompId, string BrId, string ItemId, string ShowFor);
        Dictionary<string, string> GetAssetItem(string CompID, string BrId, string ShowFor);
        Dictionary<string, string> GetSerialNo(string CompID, string BrId, string ItemId, string ShowFor, int RegId);
        //Dictionary<string, string> GetAssignedRequirementArea1(string CompId, string BrId);

        DataSet GetAssetCategoryDetails(string CompId, string BrId, string AssetGroupId);
        string InsertAssetRegDetail(AssetRegistration_Model ObjAddItemGroupSetupBOL, string mac_id, DataTable Attachments, DataTable ProcurmentDetail);
        DataSet GetAssetRegistrationDetail(string CompId, string BrId, int RegId);
        DataSet GetAllData(string CompID, string BranchID, string GroupId, string CategoryId, string ReqAreaId, string WorkingStatus, string Status);
        string DeleteARetails(string CompID, string BrchID, string AssetRegId);
        string ApproveAssetRegistration(string AssetRegId, string CompID, string BrchID, string doc_no, string UserID, string mac_id);
        DataSet Get_FYList(string Compid, string Brid);
        DataSet GetTransferDetail(string CompID, string BrID, string AssetDescriptionTD, string SerialNumberTD);
        DataTable GetCurrList(string CompID);
        DataSet GetMasterDropDownList(string Comp_id, string Br_ID);
        DataSet GetVerifiedDataOfExcel(string compId, string brId, DataTable CustomerDetail, DataTable CustomerBranch);
        DataTable ShowExcelErrorDetail(string compId, string brId, DataTable CustomerDetail, DataTable CustomerBranch);
        string BulkImportAssetRegistrationDetail(string compId, string UserID, string BranchName, DataTable CustomerDetail, DataTable CustomerBranch);
    }
}
