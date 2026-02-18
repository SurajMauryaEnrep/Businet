using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.MODELS.SecurityLayer.UserSetup;

namespace EnRepMobileWeb.SERVICES.ISERVICES.SecurityLayer.UserSetup
{
    public interface UserDetail_ISERVICES
    {
        DataSet BindLAng();
        DataSet BindHeadOffice();
        DataSet BindReportingTo();
        DataSet getUserSetUpDt(string user_id);
        DataSet GetRoleName(string HO_ID);
        DataSet GetBranchName(string HO_ID);
        string InsertUpdateUserSetup(DataTable Userdetail, DataTable UserRoleDetail, DataTable UserBranchAccDetail);
        string DeleteUserSetup(string User_ID);
        DataSet GetUserTree(ItemMenuSearchModel ObjItemMenuSearchModel);

    }
}
