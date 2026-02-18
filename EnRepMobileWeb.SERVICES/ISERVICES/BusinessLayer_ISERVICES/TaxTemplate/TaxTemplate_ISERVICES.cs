using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.BusinessLayer.TaxTemplate;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TaxTemplate
{
    public interface TaxTemplate_ISERVICES
    {
        DataSet GetTaxTemplateList( string CompID);
         string SaveAndUpdateDetails(DataTable dtHeader, DataTable dtTax, DataTable dtBranch, DataTable dtModule, DataTable dtHSNNumber, string Method);
        DataSet GetViewDetails(string CompID,string Templateid);
        DataSet GetModulAndHsnDetails(string CompID);
        string TamplateDetailDelete(TaxTemplate_Model template_Model, string CompID);
    }
}
