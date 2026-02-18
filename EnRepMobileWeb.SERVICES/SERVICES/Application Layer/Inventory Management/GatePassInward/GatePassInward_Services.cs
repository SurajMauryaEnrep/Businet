using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.GatePassInward;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.GatePassInward
{
    public class GatePassInward_Services : GatePassInward_IServices
    {
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
        public DataTable SearchDataFilter(string Source_type,string Entity_type, string Entity_id, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@Source_type",DbType.String, Source_type),
                objProvider.CreateInitializedParameter("@Entity_type",DbType.String, Entity_type),
                objProvider.CreateInitializedParameter("@Entity_id",DbType.String, Entity_id),
                objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
                                                     };
                DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$Gpass$detail$in_Filter", prmContentGetDetails).Tables[0];
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSourceDocList(string Comp_ID, string Br_ID,string SuppID, string entity_type)
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GatePass_InwordsrcdocnoList", prmContentGetDetails);
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

        public DataSet GetItemDeatilData(string CompID, string BrchID, string entity_Name, string entity_type, string Doc_no, string Doc_dt,string GatePassNumber)
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
                                                        objProvider.CreateInitializedParameter("@GatePassNumber",DbType.String, GatePassNumber),
                };
                DataSet GetData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItem$gpass$TableData", prmContentGetDeatils);
                return GetData;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string InsertUpdateData(DataTable FGRHeader, DataTable ItemDetails, DataTable Attachments,DataTable srcItemDetails)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, FGRHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetails",SqlDbType.Structured, ItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, Attachments ),
                 objprovider.CreateInitializedParameterTableType("@srcItemDetails",SqlDbType.Structured, srcItemDetails ),

                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),

                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$gate$pass$in$detail_InsertUpdate", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet DeleteData(string comp_id, string br_id, string gpass_no, string gpass_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_id),
                    objProvider.CreateInitializedParameter("@gpass_no",DbType.String, gpass_no),
                    objProvider.CreateInitializedParameter("@gpass_dt",DbType.String,gpass_dt),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$GPassInward$detail_Delete]", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetFGRDeatilData(string CompID, string BrchID, string gpass_no, string gpass_dt, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@gpass_no",DbType.String, gpass_no),
                    objProvider.CreateInitializedParameter("@gpass_dt",DbType.String, gpass_dt),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$Gpass$deatil$in$data", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllDropDownList(string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId,
            string FromDate, string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                       
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                           objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                         objProvider.CreateInitializedParameter("@FromDate",DbType.String, FromDate),
                         objProvider.CreateInitializedParameter("@ToDate",DbType.String, ToDate),
                                                     };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$gpass$in$list$bind", prmContentGetDetails);
                return dt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approve_details(string comp_id, string br_id, string DocumentMenuID, string gpass_no, string gpass_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks)
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
                    objProvider.CreateInitializedParameter("@gpass_no",DbType.String,gpass_no),
                    objProvider.CreateInitializedParameter("@gpass_dt",DbType.Date,  gpass_dt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, userid),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@stkstatus",DbType.String,""),
                };
                prmContentGetDetails[10].Size = 100;
                prmContentGetDetails[10].Direction = ParameterDirection.Output;

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$gpassInward$detail_Approve", prmContentGetDetails).ToString();

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
        public DataSet CancelAndReturnData(string CompID, string br_id, string gpass_no, string gpass_dt, string UserID, string DocumentMenuId, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@MenuDocId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@gpass_no",DbType.String, gpass_no),
                    objProvider.CreateInitializedParameter("@gpass_dt",DbType.Date, gpass_dt),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),

               };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$Gpassinward$detail_Cancel", prmContentGetDetails);

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
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetPrint$Data$GatePassInward]", miobject);
                return dt;

            }
            catch (SqlException EX)
            {
                throw EX;
            }


            return null;
        } 
        public DataSet GetCustandSuppAddrDetailDL(string Entity_Name, string CompID, string BrchID, string Entity_Type)
        {
            try
            {
                SqlDataProvider dataobject = new SqlDataProvider();
                SqlParameter[] miobject ={
        dataobject.CreateInitializedParameter("@comid",DbType.String,CompID),
        dataobject.CreateInitializedParameter("@brid",DbType.String,BrchID),
        dataobject.CreateInitializedParameter("@Entity_Name",DbType.String,Entity_Name),
        dataobject.CreateInitializedParameter("@Entity_Type",DbType.String,Entity_Type),

                };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$EntityWise$Address]", miobject);
                return dt;

            }
            catch (SqlException EX)
            {
                throw EX;
            }


            return null;
        }
    }
}
