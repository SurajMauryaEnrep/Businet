using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.Assembly_Kit
{
    public interface AssemblyKit_ISERVICES
    {
       DataSet BindItemList(string ItemName,string CompID,string BranchId);
        DataSet GetItemStockBatchWise(string CompID, string BranchId, string ItemId, string Wh_ID, string uom_id);
        DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string Wh_ID);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string Doc_no, string Doc_dt, string ItemID);
        string InsertUpdate(DataTable FGRHeader, DataTable ItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable InputdtSubItem,DataTable Attachments);
        DataTable SearchDataFilter(string AssemblyProduct, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId,string Userid,string wf_status);
        DataSet GetDeatilData(string CompID, string BrchID, string Doc_no, string Doc_dt, string UserID, string DocumentMenuId);
        DataSet DeleteData(string comp_id, string br_id, string gpass_no, string gpass_dt);
        string Approve_Detail(string comp_id, string br_id, string DocumentMenuID, string Doc_no, string Doc_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet GetDeatilSubitm(string CompID, string BrID, string Item_id, string Uom_id, string WhID, string Doc_no, string Doc_dt, string Flag);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string doc_no, string doc_dt, string ItemID);
        DataSet Cancel_Document(string CompID, string br_id, string Doc_no, string Doc_dt, string UserID, string DocumentMenuId, string mac_id);
        DataSet GetPurchaseAKDeatilsPDF(string Comp_ID, string Br_ID, string DocumentNumber, string DocumentDate);

      
    }
}
