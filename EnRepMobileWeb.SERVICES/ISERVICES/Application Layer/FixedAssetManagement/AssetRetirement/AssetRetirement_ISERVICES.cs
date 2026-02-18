using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetRetirement;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetRetirement;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetRetirement
{
    public interface AssetRetirement_ISERVICES
    {
        Dictionary<string, string> GetAssetGroup(string CompID, string GroupId);
        Dictionary<string, string> GetSerialNo(string CompID, string BrId, string ItemId, string ShowFor, string grp_id);
        DataSet GetSerialNoJs(string CompId, string BrId, string ItemId, string ShowFor, string Grp_id);
        DataSet GetRetirmentData(string CompId, string BrId, string ItemId, string SerialNo);
        DataSet GetAssetDescDetails(string CompId, string BrId, string AssetGroupId);
        DataSet GetAssetRegGroupDetail(string CompId, string BrID, string AssetGroup, string fin_yr, string Period);
        string InsertARDetail(AssetRetirement_Model ObjAddItemGroupSetupBOL, DataTable HeaderDetail, DataTable DtblVouGLDetail, DataTable CRCostCenterDetails);
        DataSet GetDepreciationProcessDetail(string CompId, string BrId, string DocNo, string DocDate, string UserID, string DocID);
        DataSet GetAllData(string CompID, string BranchID, string GroupId, string Status,string UserID);
        string DeleteAssRetDetail(string CompID, string BrchID, string DocNo, string DocDate);
        string ApproveDPDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID, string UserID, string mac_id, string VoucherNarr);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetTaxRecivableAcc(string comp_id, string assetId);
        Dictionary<string, string> GetAssetItem(string CompID, string BrId, string ShowFor);
        DataSet GetAssetProcurmentDetail(string CompID, string BrID, string RegId);
        DataSet GetRegistrationHistory(string CompID, string BrID, string RegId);
    }
}
