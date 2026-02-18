using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES
{
    public interface LOGIN_ISERVICES
    {
        DataSet GetValidUser(string userName, string Comp_id,string BranchId);
        DataSet ValidateUserToForgetPassword(string userName);
        int ChangePassword(string userName, string newPassword);
    }
}
