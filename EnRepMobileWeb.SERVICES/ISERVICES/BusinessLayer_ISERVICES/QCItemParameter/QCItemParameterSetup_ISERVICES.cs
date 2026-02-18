using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
     public interface QCItemParameterSetup_ISERVICES
    {
        DataSet GetItemParameterSetupList(int CompID);
        DataTable GetITMlist(string GroupName, string CompID,string BrchID);
        DataSet GetAllData(string GroupName, string CompID,string BrchID,string item_Group);
        DataTable ItemGroupList(string GroupName, string CompID);
        DataSet getQCItemParameterSave(string TransType,int Comp_ID, int userid, string param_name, string param_Id, string UpperValue, string LowerValue,  string SystemDetail, string uomId, string itemId);
        DataTable GetItemSetupParameterList(int CompID);
        DataTable GetItemSetupList(int CompID, string BrchID, string Value);
        String insertQCDetails(DataTable QCItemDetail);
        DataTable GetItemSetupListOnVIew(int CompID, string itemID);
        DataSet GetItemSetupToDelete(string itemId, string CompID);
        DataSet GetQCItemDetailLIst(string ItemID, string CompId);
        DataSet GetItemSetupToApprove(string itemId, string CompID, string UserId, string status);
        DataSet GetItemParamListFilterDAL(string CompID, string ItmID, string ItmGrpID);
        DataTable GetUomIdList(string CompID);
        DataTable GetItemParaMList(string CompID, string Parmid);
        DataSet getReplicateWith(string comp_id, string SarchValue);
        DataSet GetReplicateWithItemdata(string comp_id, string ItemId);
        DataSet GetMasterDropDownList(string compid, string Br_Id);
        DataSet GetVerifiedDataOfExcel(string compId, string Br_Id, DataTable QCDetail);
        DataTable ShowExcelErrorDetail(string compId, string Br_Id, DataTable QCDetail);
        string BulkImportQCItemDetail(string compId, string UserID, DataTable QCDetail);
    }
        
}
