using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.ConsumptionAnalysis
{
    public interface ConsumptionAnalysis_ISERVICES
    {
        DataSet GetConsumption_Details(string CompID, string BrID, string ProductID, string From_dt, string To_dt, string Group, string Flag, string shflId, string opId);
        DataSet GetLotDetail(string CompID, string BrID, string cnf_no, string cnf_dt, string Product_Id, string Material_item_id);
        //DataSet BindProductNameInDDL(string CompID, string BrID);
        DataTable ItemGroupListDAL(string GroupName, string CompID);

    }
}
