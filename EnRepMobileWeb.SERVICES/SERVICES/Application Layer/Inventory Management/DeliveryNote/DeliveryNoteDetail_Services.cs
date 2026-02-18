using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnRepMobileWeb.UTILITIES;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management;
using EnRepMobileWeb.MODELS.ApplicationLayer.Inventory_Management;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management
{
    public class DeliveryNoteDetail_Services : DeliveryNoteDetail_IServices
    {
        public DataTable GetSupplierListALl(string CompID, string SuppName, string BrchID)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                     };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$supp$detail_GetSuppListAll]", prmContentGetDetails).Tables[0];
            return dt;
        }
        public DataSet getDetailBySourceDocumentNo(string CompID, string BrchID, string SourDocumentNo,string SourDocumentDate,string Item_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SourDocumentNo",DbType.String, SourDocumentNo),
                     objProvider.CreateInitializedParameter("@srcdate",DbType.String, SourDocumentDate),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$dn$GetDetailBySourceDocumentNo]", prmContentGetDetails);
            return ds;
        }
        public String insertDeliveryNoteDetails(DataTable DeliveryNoteHeader, DataTable DeliveryNoteItemDetails, DataTable Attachments,DataTable dtSubItem)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, DeliveryNoteHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DeliveryNoteItemDetails ),
                  objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,Attachments),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetals",SqlDbType.Structured,dtSubItem),
                };
                 prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$dn$detail_InsertDeliveryNote_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
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
        public DataSet checkDependency(string CompID, string BrchID, string dn_no, string dn_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String, dn_no),
                     objProvider.CreateInitializedParameter("@dn_dt",DbType.Date,  dn_dt),
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_DeliveryNoteCheckDependency", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDeliveryNoteDetailByNo(string CompID, string dn_no, string BrchID, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String, dn_no),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_dn$detail$DeliveryNote$DetailById", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet GetDeliveryNoteDeatilsForPrint(string CompID, string BrchID, string dn_no, string dn_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String, dn_no),
                     objProvider.CreateInitializedParameter("@dn_dt",DbType.Date,  dn_dt),
                };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDeliveryNoteDeatils_ForPrint", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet DeliveryNoteDelete(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, string comp_id, string br_id,string DeliveryNoteNo)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String, _DeliveryNoteDetail_MODELS.dn_no),
                     objProvider.CreateInitializedParameter("@dn_dt",DbType.Date,  _DeliveryNoteDetail_MODELS.dn_dt),
                };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$DeliveryNoteDelete", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeliveryNoteApprove(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, string comp_id, string br_id, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String,  _DeliveryNoteDetail_MODELS.dn_no),
                    objProvider.CreateInitializedParameter("@dn_dt",DbType.Date,  _DeliveryNoteDetail_MODELS.dn_dt),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _DeliveryNoteDetail_MODELS.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                     objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                          objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                      objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocumentMenuId),
                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$DeliveryNoteApprove", prmContentGetDetails);
                string DocuNo = string.Empty;
                DocuNo = ImageDeatils.Tables[0].Rows[0]["dn_detail"].ToString();
                return DocuNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public Dictionary<string, string> AutoGetSupplierListALl(string CompID, string SuppName, string BrchID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppListAll", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public DataSet getDeliveryNoteSourceDocumentNo(string CompID, string BrchID, string SupplierId, string DocumentNumber,string Item_id,string Dn_type)
        {
            Dictionary<string, string> ddlDocumentNumbereDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SupplierId",DbType.String, SupplierId),
                      objProvider.CreateInitializedParameter("@DocumentNumber",DbType.String, DocumentNumber),
                      objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@Dn_type",DbType.String, Dn_type),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$dn$GetSourceDocument]", prmContentGetDetails);
                
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public String DeliveryNoteCancel(DeliveryNoteDetail_MODELS _DeliveryNoteDetail_MODELS, string comp_id, string userid, string br_id,string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String,  _DeliveryNoteDetail_MODELS.dn_no),
                    objProvider.CreateInitializedParameter("@dn_dt",DbType.Date,  _DeliveryNoteDetail_MODELS.dn_dt),             
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, userid),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                           objProvider.CreateInitializedParameter("@cancelRemarks",DbType.Date,  _DeliveryNoteDetail_MODELS.CancelledRemarks),
                     };
                string dn_no = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_stp$DeliveryNoteCancel", prmContentGetDetails).ToString();
                return dn_no; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public string getNextDocumentNumber(string CompID, string BrchID, string MenuDocumentId, string Prefix)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                objProvider.CreateInitializedParameter("@MenuDocumentId",DbType.String,MenuDocumentId),
                objProvider.CreateInitializedParameter("@Prefix",DbType.String,Prefix),
               };

            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$GetDocumentNextNUmber]", prmContentGetDetails).Tables[0];
            string documentNo = dt.Rows[0]["Column1"].ToString();
            return documentNo;
        }
        public DataSet GetSubItemDetailsFromPO(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag,string Srcdoc_no)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@dn_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@dn_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      objProvider.CreateInitializedParameter("@SrcDocNo",DbType.String, Srcdoc_no)
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetSubItemDetailsFromPO]", prmContentGetDetails);
            return ds;
        }
        public DataSet DN_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag,string Srcdoc_no)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@dn_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@dn_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      objProvider.CreateInitializedParameter("@SrcDocNo",DbType.String, Srcdoc_no)
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[DNGetSubItemDetailsAftrInsert]", prmContentGetDetails);
            return ds;
        }

    }
}