using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.FinishedGoodsReceipt
{
   public interface FinishedGoodsReceipt_ISERVICE
    {
        DataSet ItemList(string GroupName, string CompID, string BrchID);
        DataSet OperationData(string GroupName, string CompID, string BrchID);
        DataSet GetBomDeatilData(string itemID, string operation_ID, string shop_floor, string CompID, string brnchID,string  flag);


        DataSet GetAllDropDownList(string comid, string brid);
        DataSet GetConsumeSubItemShflAvlstockDetails(string CompID, string BrID, string Item_id, string Uom_id, string shfl_id);
        DataSet GetOutputDeatilSubitm(string CompID, string BrID, string Item_id, string Uom_id, string shfl_id, string rcpt_no, string rcpt_dt,string Flag);
        DataSet getItemStockBatchWise(string CompId, string BranchId, string ItemId, string ShflID, string uom_id);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string FGR_No, string FGR_dt, string ItemID);
        DataSet GetAvlStock(string CompID, string brnchID, string itemid, string shopflore);
        string InsertUpdate_FinishGoodRecipt(DataTable FGRHeader, DataTable InputFGRItemDetails, DataTable OutputItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable OutputdtSubItem, DataTable InputdtSubItem);
        DataSet GetFGRDeatilData(string CompID, string BrchID,  string rcpt_no, string rcpt_dt, string UserID, string DocumentMenuId);
        DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string ShflId);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string rcpt_no, string rcpt_dt, string ItemID);
        DataSet GetFGRList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId, string FromDate, string ToDate);
        DataSet Delete_FinishGoodsRecipt(string comp_id, string br_id, string rcpt_no, string rcpt_dt);
        string Approve_FinishedGoodsReceipt(string comp_id, string br_id, string DocumentMenuID, string rcpt_no, string rcpt_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataTable SearchDataFilter(string shopfloreid, string opertionid, string Fromdate,
            string Todate, string Status, string CompID, string BrchID, string DocumentMenuId, string Source_type, string Item_id);
        DataSet Cancel_FinishGoodsReceipt(string CompID, string br_id, string rcpt_No, string rcpt_Date, string UserID, string DocumentMenuId, string mac_id);
        DataSet GetProductionConfirmationPrintDeatils(string CompID, string br_id, string cnf_no, string cnf_dt);
    };
}
