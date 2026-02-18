using EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.MessageSetup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.MessageSetup
{
    public interface AlertSetup_IService        
    {
        int AddUpdateAlertSetup(AlertSetupModel asModel);
        //int AddUpdateEmailAlertSetup(AlertSetupModel asModel);
        int DeleteAlertSetup(string compId, string brId, string docId, string events, string alertType);
        DataTable GetAlertMsg(string compId, string brId,string alertType, string docId, string events, string langFlag);
        DataSet GetAllData(string compId,string brId, string langFlag, string docId, string ddltype, string alertType,string events);  
        DataTable GetDocumentList(string compId,string brId, string langFlag,string alert_type);  
        DataTable GetDocumentEvents( string docId, string ddltype, string alertType, string compId, string brId);  
        DataTable GetDocumentFieldName(string compId, string docId);  

    }
}
