using EnRepMobileWeb.SERVICES.ISERVICES.FactorySettings.ResetData;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.FactorySettings.ResetData
{
    public class ResetData_Service: ResetData_IService
    {
        public DataSet BindHeadOffice()
        {
            SqlParameter[] prmContentGetDetails = {
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$Comp$detail_GetHoListAll", prmContentGetDetails);
            return ds;
        }
        public DataSet BindBranchList(string comp_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContent= {
                                            objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                          };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc_GetBranchList", prmContent);
            return ds;
        }
        public string FactoryReset_data(string comp_id, string br_id, string flag)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContent = {
                                            objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comp_id),
                                             objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                                              objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                                              objProvider.CreateInitializedParameter("@out_result",DbType.String,""),
                                          };
            prmContent[3].Size = 100;
            prmContent[3].Direction = ParameterDirection.Output;
            string result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "prc_factoryresetdata", prmContent).ToString();

            string output_ = string.Empty;
            if (prmContent[3].Value != DBNull.Value) // status
            {
                output_ = prmContent[3].Value.ToString();
            }
            return output_;
        }
    }
}
