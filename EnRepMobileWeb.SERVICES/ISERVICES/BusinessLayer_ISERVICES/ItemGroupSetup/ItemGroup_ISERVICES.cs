using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.BusinessLayer.ItemGroupSetup;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemGroupSetup
{
    public interface ItemGroup_ISERVICES
    {
        DataSet GetAllItemGrp(ItemMenuSearchModel ObjItemMenuSearchModel);

        DataSet GetItemDetail(string ItemGrpId, int CompId);
        DataSet GetItemGroupDetail(string ItemGrpId, int CompId);
        DataSet GetDefaultItemDetail(int CompId,string GroupID);
        DataTable GetItemGroupSetup(int CompId);

        Dictionary<string, string> GetLocalSaleAccount(string AccName, string CompID);

        Dictionary<string, string> GetLocalPurchaseAccount(string AccName, string CompID);

        Dictionary<string, string> GetStockAccount(string AccName, string CompID);

        Dictionary<string, string> GetProvisionalPayableAccount(string AccName, string CompID);

        string InsertitemGroupDetail(ItemGroupModel ObjAddItemGroupSetupBOL);

        string DeleteItemGroup(int item_grp_id, int comp_id);
        string ChkPGroupDependency(int item_grp_id, int comp_id);

        DataSet GetSelectedParentDetail(string item_grp_struc, int CompId);
        JObject GetAllItemGrpBl(ItemMenuSearchModel ObjItemMenuSearchModel);


    }
}
