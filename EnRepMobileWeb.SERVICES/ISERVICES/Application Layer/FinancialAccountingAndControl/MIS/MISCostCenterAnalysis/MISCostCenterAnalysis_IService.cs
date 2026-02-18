using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.MISCostCenterAnalysis
{
    public interface MISCostCenterAnalysis_IService
    {
        //Added by Suraj on 08-10-2024
        //To Get Expance And Revanue Details
        DataSet GetExpanceAndRevanueDs(string comp_id, string br_id, string cc_id, string cc_val_id, string from_dt, string to_dt);

        //Added by Suraj on 09-10-2024
        //To Get Cost Center Transaction Details
        DataSet GetCostCenterTransactionDetails(string comp_id, string br_id, string cc_id, string cc_val_id, string from_dt, string to_dt,string acc_id);

        //Added by Suraj on 09-10-2024
        //To Get Page Load Details on Cost Center Analysis.
        DataSet GetCCExpRevPageLoadDetail(string comp_id, string br_id);

        //Added by Suraj on 09-10-2024
        //To Get Cost Center Value List on behalf Cost Center.
        DataSet GetCostCenterValueListByCostCenter(string comp_id, string br_id, string cc_id);
    }
}
