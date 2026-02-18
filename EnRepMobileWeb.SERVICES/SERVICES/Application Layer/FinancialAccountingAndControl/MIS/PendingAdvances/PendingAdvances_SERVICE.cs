using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.PendingAdvances;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.FinancialAccountingAndControl.MIS.PendingAdvances
{
   public class PendingAdvances_SERVICE : PendingAdvances_ISERVICE
    {
        public string InsertUserRangeDetail(string CompID, string user_id, string range1, string range2, string range3, string range4, string range5)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                               objprovider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                 objprovider.CreateInitializedParameter("@user_id",DbType.String, user_id ),
                 objprovider.CreateInitializedParameter("@range1",DbType.String, range1 ),
                 objprovider.CreateInitializedParameter("@range2",DbType.String, range2 ),
                 objprovider.CreateInitializedParameter("@range3",DbType.String, range3 ),
                 objprovider.CreateInitializedParameter("@range4",DbType.String, range4 ),
                 objprovider.CreateInitializedParameter("@range5",DbType.String, range5 ),
                };

                string rangedt = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "fin$gl$pa$UserAgingBucket$InsertUpdate", prmcontentaddupdate).ToString();
                return rangedt;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet GetUserRangeDetail(string CompID, string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Pa$UserAgingBucket$View", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetPendingAdvancesList(string CompId, string BranchID, string UserID, string EntityType, string AsDate, string ReportType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@user_id",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@entitytype",DbType.String, EntityType),
                                                        objProvider.CreateInitializedParameter("@date",DbType.String, AsDate),
                                                        objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                      };
                DataSet GetAgingDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPendingAdvanceDetails", prmContentGetDetails);
                return GetAgingDetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetPendingAdvancesAgingList(string CompId, string BranchID,string Acc_ID, string lrange, string urange, string EntityType, string AsDate, int CurrId, string ReportType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@Acc_ID",DbType.String, Acc_ID),
                                                        objProvider.CreateInitializedParameter("@lrange",DbType.String, lrange),
                                                        objProvider.CreateInitializedParameter("@urange",DbType.String, urange),
                                                        objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                                                        objProvider.CreateInitializedParameter("@date",DbType.String, AsDate),
                                                        objProvider.CreateInitializedParameter("@CurrId",DbType.Int32, CurrId),
                                                        objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                      };
                DataSet GetInvoiceDetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_GetPendingAdvAgingHistory", prmContentGetDetails);
                return GetInvoiceDetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
