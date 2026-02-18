using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.DailyProductionPlan
{
    public interface DailyProductionPlan_ISERVICES
    {
        DataSet GetDailyProductPlanDetails(string compID, string brId, string fromDate, string toDate, string productId,string ddlOpId);
        DataSet GetDailyProductPlanPlannedQtyDetails(string compID, string brId, string planDt, string productId);
        DataSet GetDailyProductPlanProducedQtyDetails(string compID, string brId, string producedDt, string productId);
        DataTable GetOperationNameList(int CompID);//Bind data in opetation name in ddl
    }
}
