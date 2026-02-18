using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.MODELS.ApplicationLayer.Inventory_Management;
namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management
{
    public interface DeliveryNoteList_IServices
    {
        Dictionary<string, string> GetSupplierListALl(string CompID, string SuppName, string BrchID);
       DataSet GetAllData(string CompID, string SuppName, string BrchID, string dn_no,string UserID, string wfstatus, string DocumentMenuId
           , string SuppId, string SourceType, string Fromdate, string Todate, string Status);

        DataSet GetDeliveryNoteListAll(string dn_no,string CompID,string BrchID, string UserID, string wfstatus, string DocumentMenuId);
        DataSet GetStatusList(string MenuID);

        DataTable GetDeliveryNoteSearch(string SuppId, string SourceType, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId);

     }
}
