using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesAndDistributionModels
{
   public class LSOListModel
    {
        public string InsightType { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public string Qty { get; set; }
        public string FinStDt { get; set; }
        public string CustType { get; set; }
        public string MenuDocumentId { get; set; }
        public string WF_status { get; set; }
        public string LSOSearch { get; set; }
        public string ListFilterData { get; set; }
        public string Title { get; set; }
        public string SO_CustName { get; set; }
        public string SO_CustID { get; set; }
        public string SO_FromDate { get; set; }
        public string SO_ToDate { get; set; }
        public string SO_OrderType { get; set; }
        public string SO_Status { get; set; }
        public string FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SQ_SalePerson { get; set; }

        public List<SalePersonList> SalePersonList { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public string DocumentMenuId { get; set; }

        public List<Status> StatusList { get; set; }

        public List<LocalSalesOrder> LocalSalesOrderList { get; set; }

    }
    public class SalePersonList
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

    }

    public class Status
{
    public string status_id { get; set; }
    public string status_name { get; set; }
}

public class CustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }

    public class LocalSalesOrder
    {
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string SalesPerson { get; set; }
        public string OrderDt { get; set; }
        public string OrderType { get; set; }
        public string SourceType { get; set; }
        public string src_doc_number { get; set; }
        public string cust_alias { get; set; }
        public string cust_name { get; set; }
        public string curr_logo { get; set; }
        public string net_val_bs { get; set; }
        public string net_val_spec { get; set; }
        public string OrderStauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string src_doc_date { get; set; }
        public string create_by { get; set; }
        public string mod_by { get; set; }
        public string app_by { get; set; }
        public string ref_doc_no { get; set; }
    }

}
