using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.MIS.ProcurementDetail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.ProcurementDetail
{
    public interface ProcurementDetail_ISERVICES
    {
        DataSet GetAllDDLData(string Comp_ID, string Br_ID, string SupplierName, string GroupName, string PortfolioName);
        DataTable GetcategoryDAL(string CompID);
        DataTable GetsuppportDAL(string CompID);
        Dictionary<string, string> ItemGroupList(string GroupName, string CompID);
        Dictionary<string, string> ItemPortfolioList(string GroupName, string CompID);
        DataSet GetPrcFilteredReport(Search_Parmeters model);
        DataSet GetPrcFilteredReport(Search_Parmeters model, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir);
        DataSet GetPrcDynamicTaxColumns(Search_Parmeters model);
        Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID);

        Dictionary<string, string> ItemSetupHSNDAL(string CompID, string HSNName);

    }
}
