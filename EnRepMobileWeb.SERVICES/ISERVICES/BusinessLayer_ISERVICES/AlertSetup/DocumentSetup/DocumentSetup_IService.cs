using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.DocumentSetup
{
    public interface DocumentSetup_IService
    {
        DataTable GetUsersList(string compId, string branchId, string docId, string receipientType, string events, string alertType);
        DataTable CheckIfDocumentTypeAlreadySet(string compid, string brId, string docId, string eventId, string alertType);
        int SaveDocSetupDetails(string compId, string branchId, string alertType, string docId, string events, string receipientType, string rcptId);
        int DeleteAlertDocSetup(string rowId);
        DataTable GetAlertDocData(string compId, string branchId, string alertType, string docId, string events, string receipientType);
        DataSet GetAlldata(string compId, string branchId, string language);
        DataSet GetDocumentList(string compId, string brId, string alertType, string lang);
        DataSet GetDocumentEvents(string compId, string brId, string alertType, string docId);
    }
}
