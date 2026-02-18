using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.BusinessLayer.AccountGroup;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AccountGroupSetup
{
   public interface AccountGroup_ISERVICES
    {
        DataSet GetAllAccGrp(AccMenuSearchModel ObjAccMenuSearchModel);
        DataTable GetAccGroupSetup(int CompId);
        string InsertAccGrpDetail(AccountGroupModel ObjAddAccGroupSetupBOL);
        DataSet GetDefaultAccGrp(int CompId, string GroupID);
        DataSet GetAccDetail(string AccGrpId, int CompId);
        DataSet GetAccViewDetail(string AccGrpId, int CompId);
        string DeleteAccGroup(int AccGrpID, int comp_id);
        JObject GetAllAccGrpBl(AccMenuSearchModel ObjAccMenuSearchModel);
        string ChkPGroupDependency(int acc_grp_id, int comp_id);
        string get_grptype(int comp_id, string acc_grp_id);
    }
}
