using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.MISOrderDetail
{
    public interface MISOrderDetail_IService
    {
        DataSet GetMISOrderDetail(string compId, string brId, string fromDate, string toDate, string showAs, string suppId, 
            string itemId, string currId, string srctype, string ordertype, string Status, string PoNo, string PoDate, string SuppCat, string SuppPort,string brid_list);
        DataSet GetAllDDLData(string Comp_ID, string Br_ID, string SupplierName);
        DataSet GetcategoryPortfolioDAL(string CompID);
    }
}
