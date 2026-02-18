using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MIS.StockAgeing;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.StockAgeing;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.StockAgeing
{
    public class StockAgeingDetail_SERVICES: StockAgeingDetail_ISERVICES
    {
        public string SaveAgeingRange(string CompId, string UserId, AgeingRanges ageingRanges)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                     objProvider.CreateInitializedParameter("@user_id",DbType.String, UserId),
                     objProvider.CreateInitializedParameter("@range_1",DbType.String, ageingRanges.range_1),
                     objProvider.CreateInitializedParameter("@range_2",DbType.String, ageingRanges.range_2),
                     objProvider.CreateInitializedParameter("@range_3",DbType.String, ageingRanges.range_3),
                     objProvider.CreateInitializedParameter("@range_4",DbType.String, ageingRanges.range_4),
                     objProvider.CreateInitializedParameter("@range_5",DbType.String, ageingRanges.range_5),
                     objProvider.CreateInitializedParameter("@ExecResult",DbType.String, ""),
                      
                };
                prmContentGetDetails[7].Size = 100;
                prmContentGetDetails[7].Direction = ParameterDirection.Output;
                string result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertUpdate_StockAgeingRange", prmContentGetDetails).ToString();
                string ExeResult = string.Empty;
                if (prmContentGetDetails[7].Value != DBNull.Value) // status
                {
                    ExeResult = prmContentGetDetails[7].Value.ToString();
                }
                return ExeResult;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetAgiengPageLoad(string CompId, string UserId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@user_id",DbType.String, UserId)
                                                      };
                DataSet Result = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_getStockAgingPageLoadData", prmContentGetDetails);
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetStockAgeingMISData(string compID, string br_id,string UserId, string ReportType, string itemGrpId
            , string itemPrtFloId, string hsnCode, string brnchId, string upToDate
            , int skip, int pageSize, string searchValue, string sortColumn, string sortColumnDir, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, compID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                                                        objProvider.CreateInitializedParameter("@UserId",DbType.String, UserId),
                                                        objProvider.CreateInitializedParameter("@report_type",DbType.String, ReportType),
                                                        objProvider.CreateInitializedParameter("@ItemGroup",DbType.String, itemGrpId),
                                                        objProvider.CreateInitializedParameter("@PortfolioId",DbType.String, itemPrtFloId),
                                                        objProvider.CreateInitializedParameter("@BrnchId",DbType.String, brnchId),
                                                        objProvider.CreateInitializedParameter("@HsnCode",DbType.String, hsnCode),
                                                        objProvider.CreateInitializedParameter("@UpTodate",DbType.String, upToDate),
                                                        objProvider.CreateInitializedParameter("@skip",DbType.String, skip),
                                                        objProvider.CreateInitializedParameter("@pageSize",DbType.String, pageSize),
                                                        objProvider.CreateInitializedParameter("@searchValue",DbType.String, searchValue),
                                                        objProvider.CreateInitializedParameter("@sortColumn",DbType.String, sortColumn),
                                                        objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String, sortColumnDir),
                                                        objProvider.CreateInitializedParameter("@flag",DbType.String, flag),

                                                      };
                DataSet Result = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetStockAgeingDetails", prmContentGetDetails);
                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
