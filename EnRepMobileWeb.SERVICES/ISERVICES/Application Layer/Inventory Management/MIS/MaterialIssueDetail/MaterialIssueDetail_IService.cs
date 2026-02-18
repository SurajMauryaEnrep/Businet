using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MaterialIssueDetail
{
    public interface MaterialIssueDetail_IService
    {
        DataTable GetMaterialIssueReport(string action, string compId, string brId, string itemId, string reqArea,
            string fromDate, string toDate, string transferType, string destinationBranch,
            string destinationWarehouse, string issueTo);
        DataSet IssueToList(string CompID, string Entity, string BrchID);
        DataSet GetSubItemDetails(string action,string compId, string brId, string issueNo, string issueDate, string itemId);
        
    }
}
