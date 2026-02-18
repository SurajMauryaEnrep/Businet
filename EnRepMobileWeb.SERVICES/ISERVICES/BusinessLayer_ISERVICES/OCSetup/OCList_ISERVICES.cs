using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
   public interface OCList_ISERVICES
    {
        //Dictionary<string, string> OCSetupGroupDAL(string GroupName, string CompID);
        DataSet GetOTClist(string GroupName, string CompID);
        DataTable GetOCListDAL(string CompID);
        DataSet GetOCListFilterDAL(string CompID, string OCID, string ActStatus, string OCtype, string Hsn_ID);
    }
}
