using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISGoodReceiptDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.MISGoodReceiptDetail
{
    public class MISGoodReceiptDetail_Service : MISGoodReceiptDetail_IService
    {
        public DataTable GetGoodReceiptNoteMISReport(string compId, string brId, string showAs, string fromdate, string toDate, 
            string suppId, string itemId,string MultiselectStatusHdn,string ReceiptType,string EntityType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@ShowAs",DbType.String, showAs),
                                                        objProvider.CreateInitializedParameter("@FromDate",DbType.String, fromdate),
                                                        objProvider.CreateInitializedParameter("@ToDate",DbType.String, toDate),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,suppId),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                                                        objProvider.CreateInitializedParameter("@MultiselectStatusHdn",DbType.String, MultiselectStatusHdn),
                                                        objProvider.CreateInitializedParameter("@ReceiptType",DbType.String, ReceiptType),
                                                        objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Get_GRN_MIS_Report", prmContentGetDetails);
                return GetPODetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();


                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@EntityName",DbType.String, EntityName),
                      objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GatePass_Supp_CustList", prmContentGetDetails);
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            //return null;
        }
        public  DataTable GetBatchDeatilMIS(string compId, string BrID, string recept_no, string recept_dt, string Item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();


                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, compId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrID),
                     objProvider.CreateInitializedParameter("@recept_no",DbType.String, recept_no),
                      objProvider.CreateInitializedParameter("@recept_dt",DbType.String, recept_dt),
                      objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                     };
                DataTable PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MIS$External$Receipt$Batch$Deatil", prmContentGetDetails).Tables[0];
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            //return null;
        }
        public  DataTable GetMISSerialDetailData(string compId, string BrID, string recept_no, string recept_dt, string Item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();


                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, compId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrID),
                     objProvider.CreateInitializedParameter("@recept_no",DbType.String, recept_no),
                      objProvider.CreateInitializedParameter("@recept_dt",DbType.String, recept_dt),
                      objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                     };
                DataTable PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MIS$External$Receipt$Serial$Deatil", prmContentGetDetails).Tables[0];
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            //return null;
        }
    }
}
