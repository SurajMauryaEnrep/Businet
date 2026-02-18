using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.JobOrder;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.JobOrder;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SubContracting.JobOrder
{
    public class JobOrder_SERVICES: JobOrder_ISERVICES
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
                dr[1] = "---All---";
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
            return null;
        }
        public DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppAddrDetails", prmContentGetDetails);
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
        public DataSet GetAllData(string CompId, string BrchID,string SuppName, JOListModel _JOListModel, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrchID),
                     objProvider.CreateInitializedParameter("@SuppName",DbType.Int64, SuppName),
                     objProvider.CreateInitializedParameter("@SuppType",DbType.String, "D"),
                     objProvider.CreateInitializedParameter("@SuppId",DbType.String,_JOListModel.SuppID),
                     objProvider.CreateInitializedParameter("@OpOutProdctID",DbType.String,_JOListModel.Product_id),
                     objProvider.CreateInitializedParameter("@FinishProdctID",DbType.String,_JOListModel.FinishProdct_Id),
                     objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_JOListModel.FromDate),
                     objProvider.CreateInitializedParameter("@Todate",DbType.String,_JOListModel.ToDate),
                     objProvider.CreateInitializedParameter("@Status",DbType.String, _JOListModel.Status),
                     objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                     objProvider.CreateInitializedParameter("@wfstatus",DbType.String,_JOListModel.WF_Status),
                     objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_sc$job$GetAllData", prmContentGetDetails);
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

        public DataSet GetProducORDDocList(string Comp_ID, string Br_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                        
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_ProducORDNoListByOPName", prmContentGetDetails);
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
        public DataTable GetItemUOM( string CompId, string Item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                 //objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                objProvider.CreateInitializedParameter("@ItemID",DbType.String, Item_id)};
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetItemUOMDetails", prmContentGetDetails).Tables[0];
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

        public DataSet GetDetailsAgainstProducOrdNo(string Comp_ID, string Br_ID, string ProductionOrd_no, string ProductionOrd_date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.Int32, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.Int32, Br_ID),
                                                         objProvider.CreateInitializedParameter("@PrductnOrd_no",DbType.String, ProductionOrd_no),
                                                         objProvider.CreateInitializedParameter("@PrductnOrd_date",DbType.String, ProductionOrd_date),
                                                        
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_DetailsAgainstProducORDNo", prmContentGetDetails);
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
        public string InsertJO_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblOutputItemDetail, DataTable DtblInputItemDetail, DataTable DTTaxDetail, DataTable DTOCDetail, DataTable DTDeliSchDetail, DataTable DTTermsDetail, DataTable dtSubItem, DataTable DTAttachmentDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OutPutItemDetail",SqlDbType.Structured, DtblOutputItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@InputItemDetail",SqlDbType.Structured, DtblInputItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DTOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DeliSchDetail",SqlDbType.Structured, DTDeliSchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TermsDetail",SqlDbType.Structured,DTTermsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DTAttachmentDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetails",SqlDbType.Structured, dtSubItem),

                    
                                                    };
                prmcontentaddupdate[9].Size = 100;
                prmcontentaddupdate[9].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sc$jo$detail_InsertJODetails", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[9].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[9].Value.ToString();
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
        public DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_SC$Job$BindItmListName", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public DataSet GetJODetailList(string CompId, string BrchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
        //                                                //objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
        //                                                //objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
        //                                                //objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
        //                                                //objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
        //                                                //objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
        //                                                //objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
        //                                                //objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                                                          
        //                                              };
        //        DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$job$GetListDetail", prmContentGetDetails);
        //        return GetPODetailList;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataSet GetJOSrchDetail(string CompID, string BrID, string SuppId, string ProdctID, string Fromdate, string Todate, string Status)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
        //    objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
        //    objProvider.CreateInitializedParameter("@ProdctID",DbType.String,ProdctID),
            
        //    objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
        //    objProvider.CreateInitializedParameter("@ToDate",DbType.String, Todate),
        //     objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
        //    };
        //        DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$job$searchlist", prmContentGetDetails);
        //        return Getsuppport;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetJODetailEditUpdate(string CompId, string BrchID, string JOSC_NO, string JOSC_Date, string UserID, string DocID)
        {
            
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@JONo",DbType.String, JOSC_NO),
                                                        objProvider.CreateInitializedParameter("@JODate",DbType.String, JOSC_Date),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$job$detail_GetJOSCEditUpdtDetails", prmContentGetDetails);
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

        public string JO_DeleteDetail(JobOrderModel _JobOrderModel,string CompID, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@JONo",DbType.String,_JobOrderModel.JO_No),
                                                        objProvider.CreateInitializedParameter("@JODate",DbType.String,_JobOrderModel.JO_Date),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$job$_DeleteAllSectionDetails", prmContentInsert).ToString();
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

         public DataSet GetJOListandSrchDetail(string CompId, string BrchID, JOListModel _JOListModel, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,_JOListModel.SuppID),
                                                        objProvider.CreateInitializedParameter("@OpOutProdctID",DbType.String,_JOListModel.Product_id),
                                                        objProvider.CreateInitializedParameter("@FinishProdctID",DbType.String,_JOListModel.FinishProdct_Id),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_JOListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_JOListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _JOListModel.Status),
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$job$GetListandSrchDetail", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string JOApproveDetails(string CompID, string BrchID, string JONo, string JODate, string UserID, string MenuID, string mac_id, string A_Status, string A_Level, string A_Remarks)
        {
           
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                              objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                              objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                              objProvider.CreateInitializedParameter("@docno",DbType.String, JONo),
                                                              objProvider.CreateInitializedParameter("@docdate",DbType.String, JODate),
                                                              objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                              objProvider.CreateInitializedParameter("@DocMenuId",DbType.String, MenuID),
                                                              objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                              objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                              objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                              objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sc$job$JOApproveDetails", prmContentInsert).ToString();
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
        public DataSet CheckMtrialDisptchDagainstJobOrdr(string CompID, string BrID, string JONo, string JODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, BrID),
                      objProvider.CreateInitializedParameter("@jo_no",DbType.String, JONo),
                    objProvider.CreateInitializedParameter("@jo_dt",DbType.String, JODate),
                    
                   };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sc$detail_CheckMtrialDisptchAgainstJobOrdr", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet JO_GetSubItemDetails(string CompID, string br_id, string Item_id, string jc_no, string jc_dt, string Flag, string Status, string JobOrdNo, string JobOrdDt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@Item_id",DbType.String, Item_id),
                      objProvider.CreateInitializedParameter("@jc_no",DbType.String, jc_no),
                      objProvider.CreateInitializedParameter("@jc_dt",DbType.String, jc_dt),
                      objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                      objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                      objProvider.CreateInitializedParameter("@JobOrdNo",DbType.String, JobOrdNo),
                      objProvider.CreateInitializedParameter("@JobOrdDt",DbType.String, JobOrdDt),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[JO_GetSubItemDetails]", prmContentGetDetails);
            return ds;
        }

        public DataSet GetJobOrderDeatils(string Comp_ID, string Br_ID, string OrderNo, string OrderDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@jo_no",DbType.String, OrderNo),
                                                        objProvider.CreateInitializedParameter("@jo_date",DbType.String, OrderDate),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetJobOrderDeatils_ForPrint]", prmContentGetDetails);
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
        public DataSet GetJOTrackingDetail(string CompId, string BrID, string JONo, string JODate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@JoNo",DbType.String, JONo),
                                                        objProvider.CreateInitializedParameter("@Jodt",DbType.String, JODate),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetJOTrackingView", prmContentGetDetails);
                return SOData;
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
