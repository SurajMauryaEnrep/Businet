using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.BusinessLayer.QualityControlSetup;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES
{
     public interface QCParameterDefinition_ISERVICES
    {
        DataSet QCItemParameterSave(int Comp_ID, int userid, string param_name, string param_type, string Insert, string SystemDetail,int paramId);
        DataTable GetItemParameterList(string CompID);
        DataTable GetItemParaMList(string CompID, string Parmid);
        DataTable GetUomIdList(string CompID);
        DataSet GetVerifiedDataOfExcel(string compId, DataTable PDDetail);
        DataTable ShowExcelErrorDetail(string compId, DataTable PDDetail);
        string BulkImportPDDetail(string compId, string UserID, DataTable PDDetail);
    }
        
}
