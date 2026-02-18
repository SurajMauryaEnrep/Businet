using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SchemeSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SchemeSetup
{
    public interface SchemeSetup_ISERVICES
    {
        DataSet GetAllPageLoadData(string compID, string brId, string user_id, string scheme_id);
        string SaveSchemeData(DataTable schemeSetupHeader, DataTable schemeSetupSlabDetail, DataTable schemeSetupProductGroup, DataTable schemeSetupCustomerPriceGroup);
        DataSet SchemeDataList(string compID, string brId, string prod_grp, string cust_price_grp, string status, string act_status);
        DataSet SchemeDelete(SchemeSetup_Model schemeSetup_Model, string compID, string brId);
        DataSet SchemeApprove(SchemeSetup_Model schemeSetup_Model, string compID, string brId, string macId);
        DataSet SchemeListWithFilter(string compID, string brId, string prod_grp_id, string cust_prc_grp_id, string status, string act_status);
        DataSet ProductGrpList(string compID, string brId, string fromDt, string uptoDt, string search);
        DataSet CustPriceGrpList(string compID, string brId, string fromDt, string uptoDt, string search);
        DataSet ChkGrpsAlrdyAddedInRange(string compID, string brId, string fromDt, string uptoDt, string scheme_id, string prodGrps, string cstPrcGrps);
        DataSet GetPctGrpAndCstPrcGrpList(string compID, string brId, string fromDt, string uptoDt, string scheme_id);
    }
}
