
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialRequisitionSlip;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MRS;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MaterialRequisitionSlip
{
    public class MaterialRequisitionSlip_SERVICE : MaterialRequisitionSlip_ISERVICE
    {

        public DataSet MRS_GetAllDDLListAndListPageData(string CompId, string BranchId, string Entity, string WF_status, string UserID, string DocumentMenuId
            , string startDate, string CurrentDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BranchId),
                     objProvider.CreateInitializedParameter("@Entity",DbType.String, Entity),
                     objProvider.CreateInitializedParameter("@wfstatus",DbType.String, WF_status),
                     objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                     objProvider.CreateInitializedParameter("@From_Date",DbType.String, startDate),
                     objProvider.CreateInitializedParameter("@To_date",DbType.String, CurrentDate),
                     //objProvider.CreateInitializedParameter("@flag",DbType.String, flag),

                };
                DataSet dsdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP$MRS$GetRequirementAreaAndEntityList", prmContentGetDetails);
                return dsdetail;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public Dictionary<string, string> EntityList(string CompID, string Entity, string BrchID, string sr_type)
        {
            Dictionary<string, string> ddlcountryDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Entity",DbType.String, Entity),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@sr_type",DbType.String, sr_type),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$EntityList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlcountryDictionary.Add(PARQusData.Tables[0].Rows[i]["id"].ToString(), PARQusData.Tables[0].Rows[i]["val"].ToString());
                    }
                }
                return ddlcountryDictionary;

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
        public DataSet GetAvlbStockForItem(string Item_id, string CompId,string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, BrchID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetItemAvlableStock", prmContentGetDetails);
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
        public DataTable GetRequirmentreaList(string CompId,string br_id,string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@flag",DbType.String,flag)

                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mrs$GetRequirmentreaList", prmContentGetDetails).Tables[0];
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

        public DataSet GetItemList(string CompId, string BranchId, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@Branch",DbType.Int64, BranchId),
                     objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mrs$detail_ItemList", prmContentGetDetails);
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
        public DataSet GetMRStmListDAL(string CompID, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetItemNameList", prmContentGetDetails);

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
        public DataSet GetIssueToList(string CompId, string IssueTo, string BranchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@IssueTo",DbType.String, IssueTo),
                    objProvider.CreateInitializedParameter("@branchId",DbType.String, BranchId),                         };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ivt$mrs$detail_CustomerSupplerList", prmContentGetDetails);
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string InsertUpdateMRS(DataTable MRSHeader, DataTable MRSItemDetails, DataTable MRSAttachments,DataTable dtSubItem)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, MRSHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, MRSItemDetails ),
                  objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,MRSAttachments),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string ship_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "inv$mrs$InsertUpdateMRS", prmcontentaddupdate).ToString();

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
        public string MRSApprove(MRSModel _MRSModel, string CompID, string br_id, string wf_status, string wf_level, string wf_remarks, string mac_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@mrs_no",DbType.String, _MRSModel.mrs_no),
                    objProvider.CreateInitializedParameter("@mrs_dt",DbType.String, _MRSModel.mrs_dt),
                    objProvider.CreateInitializedParameter("@mrs_type",DbType.String,  _MRSModel.mrs_type),
                     objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MRSModel.CreatedBy ),
                    //objProvider.CreateInitializedParameter("@CreateBy",DbType.String, app_id ),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                    objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                    objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                    objProvider.CreateInitializedParameter("@DocID",DbType.String, DocumentMenuId),
                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$MRSApprove]", prmContentGetDetails);
                string DocuMRSNo = string.Empty;
                DocuMRSNo = ImageDeatils.Tables[0].Rows[0]["mrsd_detail"].ToString();
                return DocuMRSNo; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet MRSDelete(MRSModel _MRSModel, string CompID, string br_id,string MRS)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@mrs_no",DbType.String, _MRSModel.mrs_no),
                    objProvider.CreateInitializedParameter("@mrs_dt",DbType.Date,  _MRSModel.mrs_dt),
                    objProvider.CreateInitializedParameter("@mrs_type",DbType.String,  _MRSModel.mrs_type),
                                                     };
                DataSet ImageDeatils = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$MRSDelete]", prmContentGetDetails);
                return ImageDeatils;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String MRSCancel(MRSModel _MRSModel, string CompID, string br_id,string mac_id)
        {
            try 
            { 
                  SqlDataProvider objProvider = new SqlDataProvider();
                    SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@mrs_no",DbType.String, _MRSModel.mrs_no),
                    objProvider.CreateInitializedParameter("@mrs_dt",DbType.Date,  _MRSModel.mrs_dt),
                    objProvider.CreateInitializedParameter("@mrs_type",DbType.String,  _MRSModel.mrs_type),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MRSModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),

                    };

                       string mrs_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Inv$MRS$Cancel", prmContentGetDetails).ToString();
                    return mrs_no;
             }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public String MRSForceClose(MRSModel _MRSModel, string CompID, string br_id,string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 
                 objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@mrs_no",DbType.String, _MRSModel.mrs_no),
                    objProvider.CreateInitializedParameter("@mrs_dt",DbType.Date,  _MRSModel.mrs_dt),
                    objProvider.CreateInitializedParameter("@mrs_type",DbType.String,  _MRSModel.mrs_type),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _MRSModel.CreatedBy ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),

                    };

                string mrs_no = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Inv$MRS$ForceClose", prmContentGetDetails).ToString();
                return mrs_no;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet CheckInternalIssue(string CompID, string br_id, string MRS_no, string MRS_date)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, br_id),
            objProvider.CreateInitializedParameter("@MRS_no",DbType.String, MRS_no),
            objProvider.CreateInitializedParameter("@MRS_date",DbType.String, MRS_date),

            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckInternalIssue_AgainstMRS", prmContentGetDetails);
            return DS;
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
        public DataSet GetMRSDetail(string CompID, string mrs_no, string BrchID, string UserID, string DocumentMenuId, string language)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@mrs_no",DbType.String, mrs_no),
                     objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@language",DbType.String, language),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_MRS$DetailView", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataSet GetMRSDeatilsForPrint(string CompID, string BrchID, string mrs_no, string mrs_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@mrs_no",DbType.String, mrs_no),
                     objProvider.CreateInitializedParameter("@mrs_dt",DbType.String, mrs_dt),
            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMRSDeatils_ForPrint", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        //For Sub-Item Data fetch
        public DataSet MRS_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                    
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MRS_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSourceDocList(string Comp_ID, string Br_ID, string RequiredArea, string Req_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@RequiredArea",DbType.String, RequiredArea),
                                                        objProvider.CreateInitializedParameter("@Req_type",DbType.String, Req_type),                                                    

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMRSsrcdocnoList", prmContentGetDetails);
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
        public DataSet getProductionOrderdata(string CompID, string BrchID, string srcdocno, string srcdocdt, string RequiredArea)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),                 
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,srcdocno),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, srcdocdt),
                    objProvider.CreateInitializedParameter("@RequiredArea",DbType.String, RequiredArea),            
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$get$pro_order$inputitem$detaildata", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

       public DataSet GetSubItemDetailsFromPrductnOrd(string CompID, string BrchID, string srcdocno, string srcdocdt, string RequiredArea, string ItemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrchID),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,srcdocno),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, srcdocdt),
                    objProvider.CreateInitializedParameter("@RequiredArea",DbType.String, RequiredArea),
                    objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mrs$GetSubItemDetailsFromPrductnOrd", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataSet GetoutputList(string Comp_ID, string Br_ID, string docno, string docdt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@docno",DbType.String, docno),
                                                        objProvider.CreateInitializedParameter("@docdt",DbType.String, docdt),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMRS$out$item", prmContentGetDetails);
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
        public DataSet GetSOTrackingDetail(string CompId, string BrID, string MRS_No, string MRS_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@Docno",DbType.String, MRS_No),
                                                        objProvider.CreateInitializedParameter("@Docdt",DbType.String, MRS_Date),
                                                      };
                DataSet SOTrackingData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Mrs$tracking$Data", prmContentGetDetails);
                return SOTrackingData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet getReplicateWith(string CompID, string br_id, string mrs_type,string req_area, string SarchValue) // Added By Nitesh 26-10-2023 11:02 for Bind Shopfloore data
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@mrs_type", DbType.String, mrs_type),
                     objProvider.CreateInitializedParameter("@req_area", DbType.String, req_area),
                     objProvider.CreateInitializedParameter("@SarchValue", DbType.String, SarchValue)
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mrs$detail$replicate$item", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetReplicateWithItemdata(string CompID, string br_id, string mrs_no, string mrs_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@mrs_no", DbType.String, mrs_no),
                     objProvider.CreateInitializedParameter("@mrs_dt", DbType.String, mrs_dt)
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mrs$detail_ReplicateItemdetail", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
    public class MRSList_SERVICES : MRSList_ISERVICES
    {
        public DataSet GetMRSDetailList(string CompId, string BrchID, int reqArea, string Issueto,string EntityType, string Fromdate, string Todate, 
            string MRS_Type, string SRC_Type, string Status, string UserID, string wfstatus, string DocumentMenuId, string language, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@reqArea",DbType.String,reqArea),
                                                        objProvider.CreateInitializedParameter("@Issueto",DbType.String, Issueto),
                                                         objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                         objProvider.CreateInitializedParameter("@MRSType",DbType.String, MRS_Type),
                                                         objProvider.CreateInitializedParameter("@SRCType",DbType.String, SRC_Type),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@language",DbType.String, language),
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID)
                                                      };
                DataSet GetMRSList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMRSList", prmContentGetDetails);
                return GetMRSList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetStatusList(string MenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@menu_id",DbType.String, MenuID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$DocStatus", prmContentGetDetails);
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
      
    }



}
