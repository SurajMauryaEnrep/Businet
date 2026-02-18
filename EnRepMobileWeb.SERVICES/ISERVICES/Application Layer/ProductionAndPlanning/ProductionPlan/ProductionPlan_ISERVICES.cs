using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionPlan
{
    public interface ProductionPlan_ISERVICES
    {
        string insertJCDetail(DataTable bomproductdetail, DataTable bomitemdetails);//Insert
        DataSet BindFinancialYear(int CompID, int BrID, string StartDate, string Period, string Flag);
        DataSet BindDateRangeCal(int CompID, int BrID, string start_year, string end_year, Int32 months);
        DataSet BindProductList(int CompID, int BrID);
        DataSet GetPlannedMaterialDetails(int CompID, int BrID, string ProductID);
        string InsertUpdatePPDetails(DataTable PPHeader, DataTable PPItemDetails, DataTable SOItemDetails, DataTable dtSubItem,DataTable dtSubItemProc,string req_area);
        DataSet GetPPDetailByNo(string CompID, string PP_no, string BrchID, string UserID, string DocumentMenuId);
        DataSet GetPPList(string Source, string Fromdate, string Todate, string Status, string CompID, string BrchID,string wfstatus,
            string UserID, string DocumentMenuId);
        DataSet Delete_PPDetails(ProductionPlan_Model _ProductionPlan_Model, string comp_id, string br_id);
        string Approved_PPDetails(string comp_id, string br_id, string user_id, string PP_no,
            string PP_date, string DocumentMenuID, string mac_id, string A_Status, string A_Level, string A_Remarks
            , string AutoGen_Remarks);
        string Cancelled_PPDetail(string comp_id, string br_id, string pp_no, string pp_date, string userid, string mac_id, string MenuDocid);
        DataSet GetProductionDetailsData(int CompID, int BrID, string ProductionID, string hdn_FromDate, string hdn_ToDate, string PP_no, string PP_dt);
        DataSet AddForeCastItemDetail(string CompID, string Br_ID, string F_Fy, string F_Period, string FromDate,string ToDate);

        DataSet AddSOItemDetail(string Comp_ID, string Br_ID, string CustID, string OrderNo, string OrderDate);
        Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType);
        Dictionary<string, string> GetOrderNumberList(string CompID,string BrchID,string CustID,string OrderNumber);
        DataSet PendingStockItemWise(string CompID, string BrID, string ItemId,string UomId, string StockType);
        DataSet GetBOMDetailsItemWise(string CompID, string BrID, string ItemId);
        DataSet PP_GetSubItemDetails(string CompID, string Br_id, string ItemId, string pp_no, string pp_dt,string Flag);
        DataTable GetRequirmentreaList(string CompId, string br_id);
        DataSet CheckDocAgainstPP(string Comp_ID, string Br_ID,string DocNo,string DocDate);
        DataSet CheckDocAgainstPR(string Comp_ID, string Br_ID, string DocNo, string DocDate);
        DataSet GetSubItemDetailsFromForecast(string CompID, string BrchID, string Item_id,string Fy,Int32 F_period);
        DataSet GetSubItemDetailsFromSO(string CompID, string BrchID, string Item_id,string hdn_SOOrderNO,string Status,string pp_no);
        DataSet GetProductionPlanPrintDeatils(string CompID, string BrchID, string pp_No,string PP_Date);
        DataTable GetPPTrackingDetails(string compId, string brId, string ppNo, string ppDate);
        DataTable GetProductionPlan_DetailsInfo(string CompID, string BrID, string Item_id, string Plan_no, string Plan_dt, string flag);
        DataTable GetintimationPrintDeatils(string CompID, string BrchID, string pp_No, string PP_Date);
        DataSet GetItemQCParamDetail(string CompID, string br_id, string ItemID, string qc_no, string qc_dt);
    }
}
