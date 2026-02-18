using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.GatePassInward
{
    public interface GatePassInward_IServices
    {
        DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType);
        DataTable SearchDataFilter(string Source_type,string Entity_type, string Entity_id, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId);
        DataSet GetSourceDocList(string Comp_ID, string Br_ID,string SuppID, string entity_type);

        DataSet GetItemDeatilData(string CompID, string BrchID, string entity_Name, string entity_type, string Doc_no, string Doc_dt,string GatePassNumber);

        string InsertUpdateData(DataTable FGRHeader, DataTable ItemDetails, DataTable Attachments,DataTable srcItemDetails);
        DataSet GetFGRDeatilData(string CompID, string BrchID, string gpass_no, string gpass_dt, string UserID, string DocumentMenuId);
        DataSet GetAllDropDownList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId, string FromDate, string ToDate);
        DataSet DeleteData(string comp_id, string br_id, string gpass_no, string gpass_dt);
        string Approve_details(string comp_id, string br_id, string DocumentMenuID, string gpass_no, string gpass_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet CancelAndReturnData(string CompID, string br_id, string rcpt_No, string rcpt_Date, string UserID, string DocumentMenuId, string mac_id);
        DataSet GetDataForPrint(string CompID, string BrchID, string Doc_No, string Doc_dt);
        DataSet GetCustandSuppAddrDetailDL(string Entity_Name, string CompID, string BrchID, string Entity_Type);
    }
}
