using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.OverheadParameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.OverheadParameters
{
   public interface OverheadParameters_ISERVICES
    {
        DataTable GetOverheadExpParamDetailsDAL(Int32 comp_id);
        string InsertUpdateOverheadParam (int comp_id, int ohd_exp_id, string ohd_exp_name, int uom_id, string ohd_exp_remarks, int create_id,string transtype);
        DataTable GetUOMDAL(string CompID);
        DataSet getOHDdetailsEdit(string CompId, string ohd_exp_id);
        string OHDDelete(OverheadParametersModel OhdParamModelDEL, string CompID);
    }
}
