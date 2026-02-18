using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionConfirmation
{
    public interface ProductionConfirmation_IService
    {
        DataTable Bind_ProductList1(string CompID, string BrID, string SrcType, string ffy, string Period, string SearchName);
        DataSet GetAllData(int CompID, int br_id, string UserID, string wfstatus, string DocumentMenuId, string SrcType, string ffy, string Period, string SearchName,
            string txtFromdate, string txtToDate);
        DataSet GetOrderList(string Product_id, string CompId, string BrID);
        DataSet GetOrderDetails(string CompId, string BrID, string OrderNo, string OrderDate, string Flag, string Shflid);
        DataSet getItemStockBatchWise(string CompId, string BranchId, string ItemId, string ShflID,string uom_id);
        DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string PCNo, string PCDate, string ItemID);
        DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string ShflId);
        DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string PCNo, string PCDate, string ItemID);
        string InsertUpdate_ProductionConfirmation(DataTable Cnf_HeaderDetails, DataTable Cnf_CnfItemDetails, DataTable Cnf_OutputItemDetails, DataTable ItemBatchDetails, DataTable ItemSerialDetails, DataTable CnfAttachments,DataTable dtSubItem);
        DataSet Delete_ProductionConfirmation(string comp_id, string br_id, string Cnf_No, string Cnf_Date);
        DataSet Cancel_ProductionConfirmation(string CompID, string br_id, string Cnf_No, string Cnf_Date, string UserID, string DocumentMenuId, string mac_id);
        DataSet CheckQCAgainstCnf(string CompID, string br_id, string Cnf_No, string Cnf_Date);
        string Approve_ProductionConfirmation(string comp_id, string br_id, string DocumentMenuID, string cnf_no, string cnf_date, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks);
        DataSet GetProductionConfirmationDetailByNo(string CompID, string BrchID, string Cnf_No, string Cnf_Date, string UserID, string DocumentMenuId);
        DataSet GetProductionConfirmationList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId);
        DataTable GetProductionConfirmationFilter(string ProductionID, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId, string OPID, string ShflID, string WSID, string ShftID);
        DataSet PC_GetSubItemDetails(string CompID, string br_id, string Item_id,string Uom_id, string cnf_no, string cnf_dt,string Flag, string Shfl_id);
        DataSet GetProductionConfirmationPrintDeatils(string CompID, string br_id, string cnf_no, string cnf_dt);
        DataSet GetConsumeSubItemShflAvlstockDetails(string CompID, string BrID, string Item_id, string Uom_id, string shfl_id);
        DataSet GetConsumeSubItemDetails(string CompID, string BrID, string Item_id, string Shfl_id, string Doc_no, string Doc_dt,string Uom_id);
        DataSet GetQcDetail(string CompId, string BrID, string ItemId,string UOMId, string cnf_no, string cnf_dt);
    }
}
