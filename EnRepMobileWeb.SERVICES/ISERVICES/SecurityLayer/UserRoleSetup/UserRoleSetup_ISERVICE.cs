using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.SecurityLayer.UserRoleSetup
{
   public interface UserRoleSetup_ISERVICE
    {
        DataSet GetMenuList_UserRoleSetup(string CompID);
        DataTable GetHoDetail();
        string InsertUserRoleSetupDetails(DataTable UserRoleSetupDetail);
        DataSet GetUserRoleDetail(string CompID, string userrole_no);
        DataTable GetUserRoleList_Detail(string comp_id);
        string Delete_UserRoleDetail(string CompID, string userrole_no);
        //string CheckDuplicate_UserRoleName(string CompID, string role_name);
    }
}
