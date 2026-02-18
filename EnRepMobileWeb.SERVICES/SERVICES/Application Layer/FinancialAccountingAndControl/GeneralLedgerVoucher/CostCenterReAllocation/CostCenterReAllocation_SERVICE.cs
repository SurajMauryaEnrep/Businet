using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.CostCenterReAllocation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.CostCenterReAllocation
{
   public class CostCenterReAllocation_SERVICE : CostCenterReAllocation_ISERVICE
    {
        public DataSet GetAllDDLData(string Comp_ID, string Br_ID, string GLAccName/*, string GroupName, string PortfolioName*/)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,Br_ID),
                 objProvider.CreateInitializedParameter("@AccName",DbType.String,GLAccName),
                 //objProvider.CreateInitializedParameter("@GroupName",DbType.String,GroupName),
                 //objProvider.CreateInitializedParameter("@PortfName",DbType.String,PortfolioName)


                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Fin$CCRA_GetAllDropdownListsData", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCostCenterValueListByCostCenterType(string Comp_ID, string Br_ID, string CCTypeIDS)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                     objProvider.CreateInitializedParameter("@CCTypeIDS",DbType.String,CCTypeIDS),
                };
                DataSet Ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$CCRA_CostCenterValueListByCostCenter", prmContentGetDetails);
                return Ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet OnSrchGetCCRAReport(string Comp_ID, string Br_ID,string GlAcc_id, string CCTyp_id, string CC_Val_id, string AllocationTyp_id, string From_Dt, string To_Dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/     
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                 objProvider.CreateInitializedParameter("@GlAcc_id",DbType.String,GlAcc_id),
                 objProvider.CreateInitializedParameter("@CCTyp_id",DbType.String,CCTyp_id),
                 objProvider.CreateInitializedParameter("@CC_Val_id",DbType.String,CC_Val_id),
                 objProvider.CreateInitializedParameter("@AllocationTyp_id",DbType.String,AllocationTyp_id),
                 objProvider.CreateInitializedParameter("@From_Dt",DbType.String,From_Dt),
                 objProvider.CreateInitializedParameter("@To_Dt",DbType.String,To_Dt),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Fin$OnSrchGetCCRADetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCostCenterData(string Comp_ID, string Br_ID, /*string Int_Br_Id,*/ string Vou_No, string Vou_Dt, string GLAcc_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/     
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                     //objProvider.CreateInitializedParameter("@Int_Br_Id",DbType.String,Int_Br_Id),
                 objProvider.CreateInitializedParameter("@Vou_No",DbType.String,Vou_No),
                 objProvider.CreateInitializedParameter("@Vou_Dt",DbType.String,Vou_Dt),
                 objProvider.CreateInitializedParameter("@GLAcc_id",DbType.String,GLAcc_id),
               };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Fin$GetCostCenterDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertUpdateCCDetails(string Comp_ID, string Br_ID, string CC_int_br_id, string Vou_No, string Vou_Dt, string Vou_type, string GLAcc_id, DataTable CostCenterDetails)
        {
            
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                objprovider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                     objprovider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                  objprovider.CreateInitializedParameter("@Vou_No",DbType.String,Vou_No),
                 objprovider.CreateInitializedParameter("@Vou_Dt",DbType.String,Vou_Dt),
                  objprovider.CreateInitializedParameter("@Vou_type",DbType.String,Vou_type),
                 objprovider.CreateInitializedParameter("@GLAcc_id",DbType.String,GLAcc_id),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured,CostCenterDetails),
                 objprovider.CreateInitializedParameter("@CC_int_br_id",DbType.String,CC_int_br_id),

                };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_fin$gl$CCRA$InsertUpdateCCRA]", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[6].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[6].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }


    }
}
