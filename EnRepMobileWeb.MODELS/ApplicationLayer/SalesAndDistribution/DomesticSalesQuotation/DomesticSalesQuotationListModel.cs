using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation
{
   public class DomesticSalesQuotationListModel
    {
        public string Title { get; set; }
        public string QT_CustName { get; set; }
        public string QT_CustID { get; set; }
        public string QT_FromDate { get; set; }
        public string QT_ToDate { get; set; }
        public string QTType { get; set; }
        public string QT_Status { get; set; }
        public string FromDate { get; set; }
        public string ListFilterData { get; set; }
        public string SQ_SalePerson { get; set; }
        public string sls_per { get; set; }

        public DateTime ToDate { get; set; }
        public List<CustomerName> CustomerNameList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<SalePersonList> SalePersonList { get; set; }
        public List<DomesticSalesQuotationList> DomesticQuotList { get; set; }
        public string DocumentMenuId { get; set; }
        public string CustType { get; set; }
        public string DSQSearch { get; set; }
        public string WF_status { get; set; }
    }

  
public class Status
{
    public string status_id { get; set; }
    public string status_name { get; set; }
}
    public class SalePersonList
    {
        public string salep_id { get; set; }
        public string salep_name { get; set; }

    }
    public class CustomerName
    {
        public string cust_id { get; set; }
        public string cust_name { get; set; }
    }

    public class DomesticSalesQuotationList
    {
        public string rev_no { get; set; }
        public string hdnQuotationNo { get; set; }
        public string QuotationNo { get; set; }
        public string QuotationDate { get; set; }
        public string SalesPerson { get; set; }
        public string Cust_type { get; set; }
        public string QuotationType { get; set; }       
        public string cust_name { get; set; }
        public string curr_logo { get; set; }
        public string net_val_bs { get; set; }
        public string QTStauts { get; set; }
        public string CreateDate { get; set; }
        public string ApproveDate { get; set; }
        public string ModifyDate { get; set; }
        public string create_by { get; set; }
        public string app_by { get; set; }
        public string Amendment { get; set; }
        public string mod_by { get; set; }
    }
}
