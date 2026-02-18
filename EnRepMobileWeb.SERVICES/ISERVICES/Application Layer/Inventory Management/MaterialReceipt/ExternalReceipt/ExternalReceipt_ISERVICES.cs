using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.ExternalReceipt
{
    public interface ExternalReceipt_ISERVICES
    {
        DataTable GetItemStockBatchWise(string compid, string br_id, string item_id);
        DataSet GetWarehouseList(string CompId, string BrID);
        DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType);
        string InsertUpdateData(DataTable HeaderData, DataTable DataItemTable, DataTable Attachments,
            DataTable DataSubItemTable, DataTable BatchItemTableData, DataTable SerialItemTableData, DataTable LotBatchSerialItemTableData);
        DataSet GetAllDropDownList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId,
            string FromDate, string ToDate);
        DataSet GetDeatilData(string CompID, string BrchID, string recpt_no, string recpt_dt, string UserID, string DocumentMenuId);
        DataSet DeleteData(string comp_id, string br_id, string recpt_no, string recpt_dt);
        DataTable SearchDataFilter(string Entity_type, string Entity_id, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId);
        string Approve_details(string comp_id, string br_id, string DocumentMenuID, string recpt_no, string recpt_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet GetSubItemDetails(string CompID, string brnchID, string recpt_no, string recpt_dt, string Item_id);
        DataSet CancelData(string CompID, string br_id, string recpt_no, string recpt_dt, 
            string UserID, string DocumentMenuId, string mac_id, string CancelledRemarks);
        DataSet GetDataForPrint(string CompID, string BrchID, string Doc_No, string Doc_dt);
        DataSet GetSourceDocList(string Comp_ID, string Br_ID, string SuppID, string entity_type);
        DataSet GetItemDeatilData(string CompID, string BrchID, string entity_Name, string entity_type, string Doc_no, string Doc_dt, string rcpt_no);
    }
}
