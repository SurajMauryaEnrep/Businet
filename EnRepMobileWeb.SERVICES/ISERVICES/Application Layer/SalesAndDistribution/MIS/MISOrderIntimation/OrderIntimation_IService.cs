using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.MIS.MISOrderIntimation
{
    public interface OrderIntimation_IService
    {
        DataSet GetOrderIntimationDetail(string comp_id, string br_id,string userid, string cust_id, string From_dt, string To_dt, string OrderType, string OrderNumber,string SalesPerson,string ItemId);
        DataSet GetIntimationDetail(string CompID, string br_id, DataTable SodataTable);
        DataSet GetoOrderIntimationSONoList(string CompID, string BrchID, string Cust_id, string curr_Id,string doc_id, string From_dt, string To_dt, string OrderType);
        DataTable GetSalesPersonList(string compId, string brId, string userid);
        DataTable BindGetItemList(string GroupName, string CompID, string BrchID);
    }
}
