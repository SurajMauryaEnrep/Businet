using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.FinancialYearSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.FinancialYearSetup
{
    public class FinancialYear_Service : FinancialYear_IService
    {
        public DataTable GetFY_List(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataTable GetUOM = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$fy_list", prmContentGetDetails).Tables[0];
                return GetUOM;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public DataTable GetPN_FYdetail(string CompID,string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, Flag),
                                                     };
                DataTable GetUOM = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_getpn_fy", prmContentGetDetails).Tables[0];
                return GetUOM;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public DataSet Insert_FyClosingDetail(string CompID, string br_id, string pfy_sdt, string pfy_edt, string nfy_sdt, string nfy_edt, bool bk_close, string transtype)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentsave = {

                 objprovider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID ),
                 objprovider.CreateInitializedParameter("@br_id",DbType.Int32, br_id ),
                 objprovider.CreateInitializedParameter("@pfy_sdt",DbType.String, pfy_sdt ),
                 objprovider.CreateInitializedParameter("@pfy_edt",DbType.String, pfy_edt ),
                 objprovider.CreateInitializedParameter("@nfy_sdt",DbType.String, nfy_sdt ),
                 objprovider.CreateInitializedParameter("@nfy_edt",DbType.String, nfy_edt ),
                 objprovider.CreateInitializedParameter("@bkclose",DbType.Boolean, bk_close ),
                 objprovider.CreateInitializedParameter("@trantype",DbType.String, transtype ),
                 //objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                };
                //prmcontentsave[2].Size = 100;
                //prmcontentsave[2].Direction = ParameterDirection.Output;

                //string fy_closing= SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertUpdate_fyclosing_Details", prmcontentsave).ToString();
                DataSet fy_closing = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "InsertUpdate_fyclosing_Details", prmcontentsave);
                //string DocNo = string.Empty;
                //if (prmcontentsave[2].Value != DBNull.Value) // status
                //{
                //    DocNo = prmcontentaddupdate[2].Value.ToString();
                //}
                return fy_closing;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
    }
}
