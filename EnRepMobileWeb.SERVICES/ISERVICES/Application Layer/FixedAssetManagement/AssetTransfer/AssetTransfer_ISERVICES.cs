using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetTransfer;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetTransfer;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetTransfer
{
    public interface AssetTransfer_ISERVICES
    {
        DataSet GetAssignedRequirementArea(string CompId, string BrId);
        DataSet GetSerialNoJs(string CompId, string BrId, string ItemId, string ShowFor);
        DataSet GetLabelJs(string CompId, string BrId, string ItemId, string SerialNo);
        Dictionary<string, string> GetAssetItem(string CompID, string BrId, string ShowFor);
        Dictionary<string, string> GetSerialNo(string CompID, string BrId, string ItemId, string ShowFor, int RegId);
        string InsertAssetRegDetail(AssetTransfer_Model ObjAddItemGroupSetupBOL, string mac_id, DataTable Attachments);
        DataSet GetAssetTransferDetail(string CompId, string BrId, string DocNo, string DocDate, string UserID, string DocID);
        DataSet GetAllData(string CompID, string BranchID, string UserID, string AssetId, string ReqAreaId, string Status, string Fromdate, string Todate, string wfstatus, string Docid);
        string DeleteATDetails(string CompID, string BrchID, string DocNo, string DocDate);
        string ApproveATDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID,
     string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks);
    }
}
