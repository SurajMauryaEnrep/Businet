using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TransporterSetup
{
   public interface TransporterSetup_ISERVICES
    {
        DataTable GetCountryBehfOf_HOD_Organisation(string CompID, string TransModetype);
        DataTable GetstateOnCountryDDL(string ddlCountryID);
        DataTable GetDistrictOnStateDDL(string ddlStateID);
        DataTable GetCityOnDistrictDDL(string ddlDistrictID);
        DataSet GetStateCode(string stateId);
         DataTable GetTransportDetails(string compId, string transId, string transType, string transMode);
         DataSet GetDeatilTrasportData(string compId, string transId, string transType, string transMode);
        string InsertTransport_Details(TransporterSetupModel _TransporterSetupModel, DataTable ItemAttachments);
        string DeleteTransDetails(string compId, string transId);

    }
}
