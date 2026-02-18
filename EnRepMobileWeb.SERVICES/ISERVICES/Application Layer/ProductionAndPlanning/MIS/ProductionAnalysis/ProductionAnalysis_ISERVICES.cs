using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ProductionAnalysis
{
    public interface ProductionAnalysis_ISERVICES
    {
        DataSet GetProductionMIS_Details(string CompID, string BrID, string ProductID, string From_dt, string To_dt, string ShowAs, string shflId, string opId);
        DataSet GetProductionMIS_DetailsInfo(string CompID, string BrID, string ProductID, string From_dt, string To_dt, string ShowAs, string shflId, string opId,string flag);
        //DataSet BindProductNameInDDL(string CompID, string BrID);
        DataSet GetProductionMIS_EstimateAndActualValueDetails(string CompID, string BrID, string ProductID,string ProduceQty,
            string JcNo, string JcDt, string Cnf_no, string Cnf_dt, string From_dt, string To_dt, string Flag, string shflId, string opId);
        DataSet GetItemQCParamDetail(string CompID, string br_id, string ItemID, string qc_no, string qc_dt);
    }
}
