using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MIS.QualityAnalysis;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.ProductionAndPlanning.MIS.QualityAnalysis
{
   public class QualityAnalysis_Service :QualityAnalysis_IService
    {
        public DataTable GetQualityAnalysisReport(string compId, string brId, string srcType, string itemId,
          string fromDate, string toDate, string showAs,string DocId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@SrcType",DbType.String,srcType),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,itemId),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                  objProvider.CreateInitializedParameter("@ShowAs",DbType.String,showAs),
                  objProvider.CreateInitializedParameter("@DocId",DbType.String,DocId),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetQualityAnalysisReport", prmContentGetDetails);
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetQADetailsByItemId(string compId, string brId, string srcType, string itemId,
              string fromDate, string toDate, string showAs, string DocId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@SrcType",DbType.String,srcType),
                 objProvider.CreateInitializedParameter("@ItemId",DbType.String,itemId),
                 objProvider.CreateInitializedParameter("@FromDate",DbType.String,fromDate),
                 objProvider.CreateInitializedParameter("@ToDate",DbType.String,toDate),
                  objProvider.CreateInitializedParameter("@ShowAs",DbType.String,showAs),
                  objProvider.CreateInitializedParameter("@DocId",DbType.String,DocId),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetQAReportByItemId", prmContentGetDetails);
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return null;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetItemsDetails(string compId, string brId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brId),
                                                     };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetItemsDetails", prmContentGetDetails);
                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return null;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
