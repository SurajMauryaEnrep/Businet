using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSalesQuotation
{
    public interface DomesticSalesQuotationList_ISERVICES
    {
        Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType);
        DataSet GetAllData(string CompID, string SuppName, string BranchID, string CustType, DomesticSalesQuotationListModel _DomesticSalesQuotationListModel, string UserID, string wfstatus, string DocumentMenuId, string QtType,string SalesPersonName,string sls_per);
        DataSet GetStatusList(string MenuID);
        DataSet GetQTDetailListDAL(string CompId, string BrchID, DomesticSalesQuotationListModel _DomesticSalesQuotationListModel,string UserID,string wfstatus,string DocumentMenuId, String QtType);
        DataSet GetQT_TrackingDetail(string CompId, string BrID, string QTNo, string QTDate);
    }
}
