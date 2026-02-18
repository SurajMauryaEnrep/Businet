using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
   public interface TaxList_ISERVICES
    {
        DataTable GetTaxNameList(string GroupName, string CompID);
        DataSet GetAllData(string GroupName, string CompID);
        DataSet GetTaxListFilterDAL(string CompID, string TaxID, string ActStatus, string Taxtype);
        Dictionary<string, string> TaxSetupGroupDAL(string GroupName, string CompID);
        DataTable GetTaxListDAL(string CompID);
    }
}
