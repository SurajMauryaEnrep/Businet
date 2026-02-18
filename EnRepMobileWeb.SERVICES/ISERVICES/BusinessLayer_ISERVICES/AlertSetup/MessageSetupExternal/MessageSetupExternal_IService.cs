using EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.MessageSetupExternal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.MessageSetupExternal
{
    public interface MessageSetupExternal_IService
    {
        int AddUpdateAlertSetup(AlertMessageSetupExternal asModel);
        DataTable GetDocumentList(string compId, string brId, string langFlag, string alert_type);
        DataTable GetAlertMsg(string compId, string brId, string docId, string events, string langFlag);
       DataTable GetDocumentEvents(string docId, string ddltype, string alertType, string compId, string brId);
        DataTable GetDocumentFieldName(string compId, string docId);
        DataSet GetAllData(string compId, string brId, string langFlag, string docId, string events);
        int DeleteAlertSetup(string compId, string brId, string docId, string events, string alertType);
    }
}
