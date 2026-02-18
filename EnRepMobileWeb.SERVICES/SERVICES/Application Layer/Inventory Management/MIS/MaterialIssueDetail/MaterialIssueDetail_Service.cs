using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MaterialIssueDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.MaterialIssueDetail
{
    public class MaterialIssueDetail_Service : MaterialIssueDetail_IService
    {
        public DataTable GetMaterialIssueReport(string action, string compId, string brId, string itemId,
            string reqArea, string fromDate, string toDate, string transferType, string destinationBranch,
            string destinationWarehouse, string issueTo)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Action",DbType.String, action),
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                                                        objProvider.CreateInitializedParameter("@ReqArea",DbType.String, reqArea),
                                                        objProvider.CreateInitializedParameter("@FromDate",DbType.String, fromDate),
                                                        objProvider.CreateInitializedParameter("@ToDate",DbType.String, toDate),
                                                        objProvider.CreateInitializedParameter("@TransferType",DbType.String,transferType),
                                                        objProvider.CreateInitializedParameter("@DestinationBranch",DbType.String, destinationBranch),
                                                        objProvider.CreateInitializedParameter("@DestinationWarehouse",DbType.String, destinationWarehouse),
                                                        objProvider.CreateInitializedParameter("@IssueTo",DbType.String, issueTo),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_MIS_GetMaterialIssueDetail", prmContentGetDetails);
                return GetPODetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetSubItemDetails(string action, string compId, string brId, string issueNo, string issueDate, string itemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Action",DbType.String, action),
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                     objProvider.CreateInitializedParameter("@IssueNo",DbType.String, issueNo),
                     objProvider.CreateInitializedParameter("@IssueDate",DbType.String, issueDate),
                     objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_MIS_Get_SubItemDetails", prmContentGetDetails);
                return PARQusData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet IssueToList(string CompID, string Entity, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Entity",DbType.String, Entity),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$EntityList", prmContentGetDetails);
                return PARQusData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
