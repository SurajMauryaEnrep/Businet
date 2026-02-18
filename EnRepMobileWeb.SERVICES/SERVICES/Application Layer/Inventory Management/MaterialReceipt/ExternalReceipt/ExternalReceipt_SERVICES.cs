using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.ExternalReceipt;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialReceipt.ExternalReceipt
{
    public class ExternalReceipt_SERVICES : ExternalReceipt_ISERVICES
    {
        public DataTable GetItemStockBatchWise(string comp_id, string br_id, string item_id)
        {
            SqlDataProvider objectProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails =
            {
                objectProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
               // objectProvider.CreateInitializedParameter("@brID",DbType.String,br_id),
                objectProvider.CreateInitializedParameter("@item_id",DbType.String,item_id),
            };

            DataTable data = new DataTable();
            data = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetSubItemDetails", prmContentGetDetails).Tables[0];
            return data;
        }
        public DataSet GetWarehouseList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
                };
                //DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetWarehouseList", prmContentGetDetails);//commented By Shubham Maurya 22-07-2024
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDestWarehouseList", prmContentGetDetails);
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
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
        public string InsertUpdateData(DataTable HeaderData, DataTable DataItemTable, DataTable Attachments, 
            DataTable DataSubItemTable, DataTable BatchItemTableData, DataTable SerialItemTableData,DataTable LotBatchSerialItemTableData)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, HeaderData ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetails",SqlDbType.Structured, DataItemTable ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, Attachments ),
                 objprovider.CreateInitializedParameterTableType("@DataSubItemTable",SqlDbType.Structured, DataSubItemTable ),
                 objprovider.CreateInitializedParameterTableType("@BatchItemTableData",SqlDbType.Structured, BatchItemTableData ),
                 objprovider.CreateInitializedParameterTableType("@SerialItemTableData",SqlDbType.Structured, SerialItemTableData ),
                 objprovider.CreateInitializedParameterTableType("@LotBatchSerialItemTableData",SqlDbType.Structured, LotBatchSerialItemTableData ),

                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),

                };
                prmcontentaddupdate[7].Size = 100;
                prmcontentaddupdate[7].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "Save$Data$ExternelReceipt$Data", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[7].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[7].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet GetAllDropDownList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId
            , string FromDate, string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                         objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                         objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                         objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                                                     };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$ExternalReceipt$list$bind", prmContentGetDetails);
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDeatilData(string CompID, string BrchID, string recpt_no, string recpt_dt, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@recpt_no",DbType.String, recpt_no),
                    objProvider.CreateInitializedParameter("@recpt_dt",DbType.String, recpt_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$ExternalReceipt$deatil$in$data", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet DeleteData(string comp_id, string br_id, string recpt_no, string recpt_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@recpt_no",DbType.String, recpt_no),
                    objProvider.CreateInitializedParameter("@recpt_dt",DbType.String,recpt_dt),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$externalreceipt$detail_Delete]", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable SearchDataFilter(string Entity_type, string Entity_id, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@Entity_type",DbType.String, Entity_type),
                objProvider.CreateInitializedParameter("@Entity_id",DbType.String, Entity_id),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                                                     };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$External$Receipt_Filter", prmContentGetDetails).Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approve_details(string comp_id, string br_id, string DocumentMenuID, string recpt_no, string recpt_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),//@menuid
                    objProvider.CreateInitializedParameter("@menuid",DbType.String, DocumentMenuID),
                    objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                    objProvider.CreateInitializedParameter("@recpt_no",DbType.String,recpt_no),
                    objProvider.CreateInitializedParameter("@recpt_dt",DbType.Date,  recpt_dt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                prmContentGetDetails[10].Size = 100;
                prmContentGetDetails[10].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$external$receipt$detail_Approve", prmContentGetDetails).ToString();

                string msg = string.Empty;
                if (prmContentGetDetails[10].Value != DBNull.Value) // status
                {
                    msg = prmContentGetDetails[10].Value.ToString();
                }

                return msg;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
      public  DataSet GetSubItemDetails(string CompID, string brnchID, string recpt_no, string recpt_dt, string Item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, brnchID),
                    objProvider.CreateInitializedParameter("@recpt_no",DbType.String, recpt_no),
                    objProvider.CreateInitializedParameter("@recpt_dt",DbType.String, recpt_dt),
                    objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$inv$ext$subitem$Deatil_afterApprove", prmContentGetDetails);
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CancelData(string CompID, string br_id, string recpt_no, string recpt_dt,
            string UserID, string DocumentMenuId, string mac_id,string CancelledRemarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@MenuDocId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@recpt_no",DbType.String, recpt_no),
                    objProvider.CreateInitializedParameter("@recpt_dt",DbType.Date, recpt_dt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@CancelledRemarks",DbType.String, CancelledRemarks),

               };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$ext$receipt$detail_Cancel", prmContentGetDetails);

                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataForPrint(string CompID, string BrchID, string Doc_No, string Doc_dt)
        {
            try
            {
                SqlDataProvider dataobject = new SqlDataProvider();
                SqlParameter[] miobject ={
        dataobject.CreateInitializedParameter("@comid",DbType.String,CompID),
        dataobject.CreateInitializedParameter("@brid",DbType.String,BrchID),
        dataobject.CreateInitializedParameter("@DocNo",DbType.String,Doc_No),
        dataobject.CreateInitializedParameter("@Doc_dt",DbType.DateTime,Doc_dt),

                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetPrint$Data$ExternalReceipt]", miobject);
                return dt;

            }
            catch (SqlException EX)
            {
                throw EX;
            }


            return null;
        }
        public DataSet GetSourceDocList(string Comp_ID, string Br_ID, string SuppID, string entity_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),

                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, SuppID),
                                                        objProvider.CreateInitializedParameter("@entitytype",DbType.String, entity_type),


                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ExternalIssue$ExternalIssue$srcdocnoList", prmContentGetDetails);
                return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetItemDeatilData(string CompID, string BrchID, string entity_Name, string entity_type, string Doc_no, string Doc_dt, string rcpt_no)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDeatils =
                {
                                                      objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@entityName",DbType.String, entity_Name),
                                                        objProvider.CreateInitializedParameter("@entitytype",DbType.String, entity_type),
                                                        objProvider.CreateInitializedParameter("@Doc_no",DbType.String, Doc_no),
                                                        objProvider.CreateInitializedParameter("@Doc_dt",DbType.String, Doc_dt),
                                                        objProvider.CreateInitializedParameter("@rcpt_no",DbType.String, rcpt_no),
                };
                DataSet GetData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItem$Ext$rcpt$TableData", prmContentGetDeatils);
                return GetData;
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
