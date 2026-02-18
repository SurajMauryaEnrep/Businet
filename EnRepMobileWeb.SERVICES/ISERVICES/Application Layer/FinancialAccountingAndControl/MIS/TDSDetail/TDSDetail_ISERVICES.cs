using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.TDSDetail
{
   public interface TDSDetail_ISERVICES
    {
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string tax_type);
        DataSet Get_FYList(string Compid, string Brid);
        DataSet GetTDSDetailsMIS(string CompID, string BrID, string TDSId, string suppId, string fromDate, string toDate,string tax_type,string sec_code);
        DataSet GetTDSNameList(string compID, string br_ID, string tax_type);
    }
}
