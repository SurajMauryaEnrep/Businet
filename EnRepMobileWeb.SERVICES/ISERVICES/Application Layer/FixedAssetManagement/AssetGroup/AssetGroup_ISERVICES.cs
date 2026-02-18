using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.MODELS.ApplicationLayer.FixedAssetManagement.AssetGroup;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetGroup;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FixedAssetManagement.AssetGroup
{
    public interface AssetGroup_ISERVICES
    {
        DataSet GetAssetDetail(string ItemGrpId, string CompId);
        DataTable GetAssetGroupSetup(string CompId);
        Dictionary<string, string> GetLocalPurchaseAccount(string AccName, string CompID);
        Dictionary<string, string> GetAssetCategory(string CompID);
        Dictionary<string, string> GetStockAccount(string AccName, string CompID);
        string InsertAssetGroupDetail(AssetGroupModel ObjAddItemGroupSetupBOL);
        string DeleteItemGroup(int item_grp_id, string comp_id);
        string ChkPGroupDependency(int item_grp_id, string comp_id);
        string ChkChildGroupDependency(int item_grp_id, string comp_id);
        DataSet GetSelectedParentDetail(string item_grp_struc, string CompId);
         JObject GetAllItemGrpBl(ItemMenuSearchModel_AG ObjItemMenuSearchModel);
    }
}
