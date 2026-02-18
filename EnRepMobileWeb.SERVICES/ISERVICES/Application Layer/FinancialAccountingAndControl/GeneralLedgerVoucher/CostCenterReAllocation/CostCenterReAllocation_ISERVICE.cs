using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.CostCenterReAllocation
{
   public interface CostCenterReAllocation_ISERVICE
    {
        DataSet GetAllDDLData(string Comp_ID, string Br_ID, string GLAccName/*, string GroupName, string PortfolioName*/);
        DataSet GetCostCenterValueListByCostCenterType(string Comp_ID, string Br_ID, string CCTypeIDS);
        DataSet OnSrchGetCCRAReport(string Comp_ID, string Br_ID, string GlAcc_id, string CCTyp_id, string CC_Val_id, string AllocationTyp_id, string From_Dt, string To_Dt);
        DataSet GetCostCenterData(string Comp_ID, string Br_ID,/*string Int_Br_Id ,*/ string Vou_No, string Vou_Dt, string GLAcc_id);
        string InsertUpdateCCDetails(string Comp_ID, string Br_ID,string CC_int_br_id, string Vou_No, string Vou_Dt, string Vou_type, string GLAcc_id,DataTable CostCenterDetails);
    }
    
}
