using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.NCR;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.NCR;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.NCR
{
    public class NCR_SERVICES: NCR_ISERVICES
    {
        public DataSet GetNcrDetails(string CompID, string BrId, string FromDt, string ToDt, string SrcDocNo, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,BrId),
                 objProvider.CreateInitializedParameter("@FromDt",DbType.String,FromDt),
                 objProvider.CreateInitializedParameter("@ToDt",DbType.String,ToDt),
                 objProvider.CreateInitializedParameter("@SrcDoc",DbType.String,SrcDocNo),
                 objProvider.CreateInitializedParameter("@Status",DbType.String,Status),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_GetNCR_Deatils", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string SaveNcrAckDetails(string compID, string brchID, AckListDataModel ackListData)
        {
            try
            {

                
                    SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,compID),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,brchID),
                 objProvider.CreateInitializedParameter("@item_id",DbType.String,ackListData.item_id),
                 objProvider.CreateInitializedParameter("@uom_id",DbType.String,ackListData.uom_id),
                 objProvider.CreateInitializedParameter("@src_type",DbType.String,ackListData.src_type),
                 objProvider.CreateInitializedParameter("@doc_no",DbType.String,ackListData.doc_no),
                 objProvider.CreateInitializedParameter("@doc_dt",DbType.String,ackListData.doc_dt),
                 objProvider.CreateInitializedParameter("@entity_id",DbType.String,ackListData.entity_id),
                 objProvider.CreateInitializedParameter("@ack_by",DbType.String,ackListData.ack_by),
                 objProvider.CreateInitializedParameter("@ack_dt",DbType.String,ackListData.ack_dt),
                 objProvider.CreateInitializedParameter("@ack_taken",DbType.String,ackListData.ack_taken),
                 objProvider.CreateInitializedParameter("@remarks",DbType.String,ackListData.remarks),
                 objProvider.CreateInitializedParameter("@Result",DbType.String,""),
                 };
                prmContentGetDetails[12].Size = 100;
                prmContentGetDetails[12].Direction = ParameterDirection.Output;

                string result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[usp_SaveNcrAcknoledgeDetails]", prmContentGetDetails).ToString();

                string FinResult = string.Empty;
                if (prmContentGetDetails[12].Value != DBNull.Value) // status
                {
                    FinResult = prmContentGetDetails[12].Value.ToString();
                }
                return FinResult;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetNcrDetailonAcknowledge(string compID, string brchID, string item_id, string uom_id, string src_type
            , string doc_no, string doc_dt, string entity_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,compID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String,brchID),
                     objProvider.CreateInitializedParameter("@item_id",DbType.String,item_id),
                     objProvider.CreateInitializedParameter("@uom_id",DbType.String,uom_id),
                     objProvider.CreateInitializedParameter("@src_type",DbType.String,src_type),
                     objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                     objProvider.CreateInitializedParameter("@doc_dt",DbType.String,doc_dt),
                     objProvider.CreateInitializedParameter("@entity_id",DbType.String,entity_id),
                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "usp_GetNCR_DeatilsForEdit", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
    }
}
