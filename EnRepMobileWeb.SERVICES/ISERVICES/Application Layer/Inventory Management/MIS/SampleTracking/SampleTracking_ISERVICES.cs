using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.SampleTracking
{
    public interface SampleTracking_ISERVICES
    {
        DataSet GetSTDetailsList(string CompId, string BrchID,string ReportType, string ItemName, 
            string EntityName, string Fromdate, string Todate,string EntityType, string Issuedby,string ShowAs);
        DataSet GetSampleTrackingItmList(string CompID, string BrID, string ItmName);
        Dictionary<string, string> IssueToList(string CompID, string Entity, string BrchID);
        DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType);
        DataSet GetItemIssued_ReceivedList(string CompId, string BranchID, string EntityID,
            string EntityTypeCode, string ItemID, string Type, string issue_date, 
            string receive_date, string sr_type, string other_dtl, string ST_UOM, string fromdate,
            string todate,string Issuedby);

        DataSet GetIssuedByData(string CompID, string BrchID);
    }
}
