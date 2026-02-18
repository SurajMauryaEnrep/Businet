using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.DeliveryNote;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.DeliveryNote;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SubContracting.DeliveryNote
{
   public class DeliveryNoteDetailSC_Services : DeliveryNoteDetailSC_IServices
    {
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, "D"),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
                    }
                }
                return ddlSuppListDic;

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
        public DataSet GetJobORDDocNOList(string Supp_id, string Comp_ID, string Br_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                         objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SC$DN$Get_JobORDNoList", prmContentGetDetails);
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
        public DataSet GetMDDocNOList(string Comp_ID, string Br_ID,string JONO)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                         objProvider.CreateInitializedParameter("@JONO",DbType.String, JONO),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SC$DN$Get_MaterialDispatchNoList", prmContentGetDetails);
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
        //public DataSet GetReturnItemDDLList(string Comp_ID, string Br_ID, string JONO)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
        //                                                 objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
        //                                                 objProvider.CreateInitializedParameter("@JONO",DbType.String, JONO),

        //                                              };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SC$DN$Get_ReturnItemDDLList", prmContentGetDetails);
        //        return searchmenu;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}
        public DataSet getDetailBySourceDocumentMDNo(string CompID, string BrchID, string SourDocumentNo, string SourDocumentDate,string DNNo)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@SourDocumentNo",DbType.String, SourDocumentNo),
                     objProvider.CreateInitializedParameter("@srcdate",DbType.String, SourDocumentDate),
                     objProvider.CreateInitializedParameter("@DNNO",DbType.String, DNNo),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SC$DN$GetDetailBySourceDocumentMDNo]", prmContentGetDetails);
            return ds;
        }
        public string InsertDN_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblItemReturnDetail, DataTable DispatchQtyItemDetails, DataTable DTAttachmentDetail, DataTable dtSubItem, DataTable dtByPrdctScrapSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemReturnDetail",SqlDbType.Structured, DtblItemReturnDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DispatchQtyItemDetails",SqlDbType.Structured, DispatchQtyItemDetails),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DTAttachmentDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetails",SqlDbType.Structured,dtSubItem), 
                                                        objprovider.CreateInitializedParameterTableType("@ByPrdctScrapSubItemDetails",SqlDbType.Structured,dtByPrdctScrapSubItem),

                                                    };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sc$DN$detail_InsertDNDetails", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[5].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[5].Value.ToString();
                }

                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet GetDNSCListandSrchDetail(string CompId, string BrchID, DNListModel _DNListModel, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,_DNListModel.SuppID),
                                                       objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_DNListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_DNListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _DNListModel.Status),
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$DNSC$GetListandSrchDetail", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDNDetailEditUpdate(string CompId, string BrchID, string DNSC_NO, string DNSC_Date, string UserID, string DocID)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@DNNo",DbType.String, DNSC_NO),
                                                        objProvider.CreateInitializedParameter("@DNDate",DbType.String, DNSC_Date),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$DN$detail_GetDNSCEditUpdtDetails", prmContentGetDetails);
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

        public string DN_DeleteDetail(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model, string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@DNNo",DbType.String,_DeliveryNoteDetailSC_Model.DN_No),
                                                        objProvider.CreateInitializedParameter("@DNDate",DbType.String,_DeliveryNoteDetailSC_Model.DN_Dt),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$DN$_DeleteAllSectionDetails", prmContentInsert).ToString();
                return POId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string DNApproveDetails(string CompID, string BrchID, string DN_No, string DN_Date, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                              objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                              objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                              objProvider.CreateInitializedParameter("@docno",DbType.String, DN_No),
                                                              objProvider.CreateInitializedParameter("@docdate",DbType.String, DN_Date),
                                                              objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                              objProvider.CreateInitializedParameter("@DocMenuId",DbType.String, MenuID),
                                                              objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                              objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                              objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                              objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                                                              
                };
                
                
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$DN$DNApproveDetails", prmContentGetDetails).ToString();
                return POId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet DNCancel(DeliveryNoteDetailSC_Model _DeliveryNoteDetailSC_Model, string CompID, string br_id, string mac_id)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String,  _DeliveryNoteDetailSC_Model.DN_No),
                    objProvider.CreateInitializedParameter("@dn_dt",DbType.Date,  _DeliveryNoteDetailSC_Model.DN_Dt),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _DeliveryNoteDetailSC_Model.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
               };

            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp$DNSC$DNCancel", prmContentGetDetails);

            return DS;
        }

        public DataSet GetReturnItemDDLList(string CompID, string BrID, string ItmName, string PageName, string JONO)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@PageName",DbType.String, PageName),
                    objProvider.CreateInitializedParameter("@JONO",DbType.String, JONO),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SC$DN$Get_ReturnItemDDLList", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet BindByProdctScrapItm_AgainstDircetJO(string CompID, string BrID, string ItmName, string PageName/*, string JONO*/)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@PageName",DbType.String, PageName),
                    //objProvider.CreateInitializedParameter("@JONO",DbType.String, JONO),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$DNSC$GetByPrdctScrapItm", prmContentGetDetails);
                return PARQusData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataSet ChkGRNSCDagainstDNSC(string CompID, string BrID, string DNNo, string DNDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String, DNNo),
                    objProvider.CreateInitializedParameter("@dn_dt",DbType.String, DNDate),

                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$ChckDtlGRNSCAgainstDNSC", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet CheckQCAgainstDNSC(string CompId, string BrchID, string DocNo, string DocDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, DocNo),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, DocDate),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$dnscqc$detail_CheckQCAgainstDNSC", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetailsFromMD(string CompID, string br_id, string Item_id, string doc_no, string doc_dt, string Flag, string JobOrdNo, string MDNo, string MDDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@dn_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@dn_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      objProvider.CreateInitializedParameter("@JobOrdNo",DbType.String, JobOrdNo),
                      objProvider.CreateInitializedParameter("@MDNo",DbType.String, MDNo),
                      objProvider.CreateInitializedParameter("@MDDate",DbType.String, MDDate)
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetSubItemDetailsFromMD]", prmContentGetDetails);
            return ds;
        }
        public DataSet DNSC_GetSubItemDetailsAfterApprov(string CompID, string br_id, string Item_id, string MDNo, string doc_no, string doc_dt, string Flag)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                     objProvider.CreateInitializedParameter("@MDNo",DbType.String, MDNo),
                      objProvider.CreateInitializedParameter("@dn_no",DbType.String, doc_no),
                      objProvider.CreateInitializedParameter("@dn_dt",DbType.String, doc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      //objProvider.CreateInitializedParameter("@SrcDocNo",DbType.String, Srcdoc_no)
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[DNSCGetSubItemDetailsAftrApprov]", prmContentGetDetails);
            return ds;
        }
        public DataSet GetDeliveryNoteSCDeatilsForPrint(string CompID, string BrchID, string dn_no, string dn_dt)
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
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$DN$GetDNSCDeatils_ForPrint", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
