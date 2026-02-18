using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.OrderDetail
{
   public interface OrderDetail_IService
    {
        DataSet GetOrder_Detail(string CompID, string BrID, string userid, string cust_id, string reg_name, string sale_type, string curr_id, string productGrp
         , string Product_Id, string productPort, string custCat, string CustPort, string inv_no,
         string inv_dt, string sale_per, string From_dt, string To_dt, string Flag, string custzone, string custgroup, string custstate, string custcity);
        DataTable GetCurrencyList(string compId, string currencyId);
        DataTable GetSONumberList(string compId, string branchId);
    }
}
