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
   public interface UserList_ISERVICES
    {
        DataTable GetUserListDAL();
        DataTable GettopUser(string Comp_ID);
    }
}
