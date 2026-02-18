using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.DepreciationProcess;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.DepreciationProcess;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.DepreciationProcess
{

    public interface DepreciationProcess_ISERVICES
    {
        Dictionary<string, string> GetAssetGroup(string CompID, string GroupId);
        DataSet GetAssetCategoryDetails(string CompId, string BrId, string AssetGroupId);
        DataSet BindFinancialYear(int CompID, int BrID, string f_freq, string StartDate, string Period, string AssetGroupId);
        DataSet GetAssetRegGroupDetail(string CompId, string BrID, string AssetGroup, string fin_yr, string Period);
        string InsertDepreciationProcessDetail(DepreciationProcess_Model ObjAddItemGroupSetupBOL, DataTable HeaderDetail, DataTable AssetDetails, DataTable Attachments, DataTable DtblVouGLDetail, DataTable CRCostCenterDetails);
        DataSet GetDepreciationProcessDetail(string CompId, string BrId, string DocNo, string DocDate, string UserID, string DocID);
        DataSet GetAllData(string CompID, string BranchID, string GroupId, string Status, string wfstatus, string UserID);
        string DeleteDPetails(string CompID, string BrchID, string DocNo, string DocDate);
        string ApproveDPDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID,
     string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr);

        DataSet BindPeriod(int CompID, int BrID, string f_freq, string StartDate);
        DataSet BindDateRangeCal(int CompID, int BrID, string f_frequency, string start_year, string end_year, Int32 months);
        DataSet GetAllGLDetails(DataTable GLDetail);
        DataSet GetTaxRecivableAcc(string comp_id, string assetId);
    }
}
