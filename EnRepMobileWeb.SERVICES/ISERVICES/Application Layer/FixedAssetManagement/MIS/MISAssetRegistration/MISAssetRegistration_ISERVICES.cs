using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.MIS.MISAssetRegistration;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.MIS.MISAssetRegistration;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.MIS.MISAssetRegistration
{
    public interface MISAssetRegistration_ISERVICES
    {
        Dictionary<string, string> GetAssetGroupListPage(string CompID, string GroupId);
        Dictionary<string, string> GetAssetCategory(string CompID, string GroupId);
        Dictionary<string, string> GetAssetCategory(string CompID);
        DataSet GetAssignedRequirementArea(string CompId, string BrId, string MIS);
        DataSet GetAllData(string CompID, string BranchID, string GroupId, string CategoryId, string ReqAreaId, string WorkingStatus, string Status);
        DataSet GetRegistrationHistory(string CompID, string BrID, string RegId);
        DataSet GetAssetProcurmentDetail(string CompID, string BrID, string RegId);
        DataTable GetCurrList(string CompID);
    }
}
