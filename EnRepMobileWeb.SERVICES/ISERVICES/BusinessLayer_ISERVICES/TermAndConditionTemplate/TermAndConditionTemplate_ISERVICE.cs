using EnRepMobileWeb.MODELS.BusinessLayer.TermAndConditionTemplate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TermAndConditionTemplate
{
    public interface TermAndConditionTemplate_ISERVICE
    {
        DataTable tmpltList(string CompID);
        DataSet GetViewDetails(string CompID, string Templateid);
        string SaveAndUpdateDetails(DataTable dtHeader, DataTable dtTermsConditions, DataTable TaxBranch);
        string TamplateDetailDelete(TermAndConditionTemplate_Model termAndConditionTemplate_Model, string CompID);
    }
}
