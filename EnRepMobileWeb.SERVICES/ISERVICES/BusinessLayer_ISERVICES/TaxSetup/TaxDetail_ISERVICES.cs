using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnRepMobileWeb.MODELS.BusinessLayer.TaxDetail;
using System.Data;


namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
   public interface TaxDetail_ISERVICES
    {
        DataTable GetAccountGroupList(string CompID, string acc_id);
        DataTable GetAccountGroupDAL(string CompID, string type);
        DataSet GetviewTaxdetailDAL(string Taxcode, string CompID);
        DataTable GetBrListDAL(string CompID);
        DataSet GetBrList(string CompID);
        DataSet GetAllData(string CompID);
        DataTable GetTaxAuthrityCoaDAL(string CompID);
        DataTable deletetaxdata(string CompID, string BrchID, int tax_id);
        DataTable GetTaxcoaDAL(string CompID);
        string insertTaxDetail(DataTable TaxDetail, DataTable TaxBranch);
        //DataSet InsertBrDetailDAL(string Comp_ID, string Taxcode, string BrID, string Flag, string TransType);
        //string InsertTaxDetailDAL(TaxDetailModel _TaxDetailModel, string CompID, string UserID);

    }
}
