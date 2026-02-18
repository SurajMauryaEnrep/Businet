using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.GatePassOutward;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.GatePassOutward
{
   public class GatePassOutwardDetail_Services : GatePassOutwardDetail_IServices
    {
        public string InsertUpdateData(DataTable FGRHeader, DataTable ItemDetails,DataTable Attachments)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, FGRHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetails",SqlDbType.Structured, ItemDetails ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured, Attachments ),
               
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$gate$pass$out$detail_InsertUpdate", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
                }
                return DocNo;
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
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$Gpass$deatil$data", prmContentGetDetails);
                return ds;
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
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$GPassOutward$detail_Delete]", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Approve_FinishedGoodsReceipt(string comp_id, string br_id, string DocumentMenuID, string gpass_no, string gpass_dt, string userid, string mac_id, string A_Status, string A_Level, string A_Remarks)
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

                string app_msg = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$gpassOutward$detail_Approve", prmContentGetDetails).ToString();

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
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$GpassOutward$detail_Cancel", prmContentGetDetails);

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
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetPrint$Data$GatePassOutward]", miobject);
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
