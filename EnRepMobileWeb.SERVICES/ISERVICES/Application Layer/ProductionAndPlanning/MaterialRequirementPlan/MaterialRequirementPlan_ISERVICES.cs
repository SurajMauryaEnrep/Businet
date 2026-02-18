using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MaterialRequirementPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MaterialRequirementPlan
{
    public interface MaterialRequirementPlan_ISERVICES
    {
        string insertJCDetail(DataTable bomproductdetail, DataTable bomitemdetails);//Insert
        DataSet BindFinancialYear(int CompID, int BrID, string StartDate, string Period, string Flag);
        DataSet BindDateRangeCal(int CompID, int BrID, string start_year, string end_year, Int32 months);
        DataSet BindProductList(int CompID, int BrID);
        DataSet GetPlannedMaterialDetails(int CompID, int BrID, string ProductID, string plannedqty);
        string InsertUpdateMRPDetails(DataTable MRPHeader, DataTable MRPItemDetails, DataTable MRPInputMaterialDetails
            , DataTable SFMaterialDetails, DataTable HdnSFmaterial, DataTable HdnRMdetails,DataTable dtSubItem,DataTable dtSubItem_precure);
        DataSet GetMRPDetailByNo(string CompID, string mrp_no, string BrchID, string UserID, string DocumentMenuId);
        DataTable GetMRPList(string Source, string Fromdate, string Todate, string Status, string CompID, string BrchID,string wfstatus, string UserID, string DocumentMenuId,string Req_area);
        DataSet Delete_MRPDetails(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model, string comp_id, string br_id);
        //string Approved_MRPDetails(string comp_id, string br_id, string user_id, string mrp_no, string mrp_date, string DocumentMenuID, string mac_id,string A_Status,string A_Level,string A_Remarks);
        string Approved_MRPDetails(DataTable MRPHeader, DataTable MRPItemDetails, DataTable MRPInputMaterialDetails
            , DataTable SFMaterialDetails,DataTable dtSubItem, string A_Status,string A_Level,string A_Remarks,DataTable dtSubItem_precu);
        string Cancelled_MRPDetail(string comp_id, string br_id, string mrp_no, string mrp_date, string userid, string mac_id, string MenuDocid);

        DataSet AddPRoductionPlanItemDetail(string Comp_ID, string Br_ID, string F_Fy, string F_Period
            , string FromDate, string ToDate, string P_Number, string P_Date);

        DataTable GetRequirmentreaList(string CompId, string br_id);
        DataSet CheckPRAgainstMRP(string CompId, string BrchID, string DocNo, string DocDate);
        Dictionary<string, string> GetPPNumberList(string CompID, string BrchID, string PPNumber, string RequisitionArea);
        DataSet GetPPNumberDetail(string CompID, string BrchID, string PP_Number, string PP_Date);
        DataSet GetSFBOMDetailsItemWise(string CompID, string BrID, string ItemId, string SFItemId);
        DataSet GetProducedQuantityDetail(string Comp_ID, string Br_ID, string mrp_no, string mrp_dt, string product_Id, string Flag);
        DataSet GetProcuredQuantityDetail(string Comp_ID, string Br_ID, string mrp_no, string mrp_dt, string product_Id, string Flag, string UomId);
        DataSet MRP_GetSubItemDetails(string CompID,string br_id, string Item_id, string mrp_no, string mrp_dt, string Flag);
        DataSet GetMRPPrintDeatils(string CompID,string br_id, string mrp_no, string mrp_dt);
        DataSet GetRMPendingOrderQuantityDetails(int CompID, int BrID, string ProductID,string UomId);
        DataSet ProductMaterialDetails(string comp_id, string br_id, string product_id, string qty);
        DataSet GetSfAndRmDataByProductList(string comp_ID, string br_ID, DataTable productListDt);
    }
}
