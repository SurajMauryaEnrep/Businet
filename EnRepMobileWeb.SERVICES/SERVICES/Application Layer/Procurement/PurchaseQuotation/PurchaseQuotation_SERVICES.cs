using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.Purchase_Quotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.Purchase_Quotation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.Purchase_Quotation
{
    public class PurchaseQuotation_SERVICES: PurchaseQuotation_ISERVICES
    {
        public string InsertPQTransactionDetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail, DataTable DTOCDetail,DataTable DtblDeliSchDetail,DataTable DtblTermsDetail,DataTable dtSubItem, DataTable Attachments)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DTOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DeliSchDetail",SqlDbType.Structured, DtblDeliSchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TermsDetail",SqlDbType.Structured,DtblTermsDetail),
                                                        //objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DTAttachmentDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,Attachments),
                                                          objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem),

                                                    };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "prc$pq$detail_InsertPQTransactionDetails", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[6].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[6].Value.ToString();
                }

                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }            
        }
        public DataSet GetSuppAddrDetailDAL(string Supp_id, string CompId,string Br_ID,string SuppPros_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.Int64, Br_ID),
                                                         objProvider.CreateInitializedParameter("@SuppPros_type",DbType.String, SuppPros_type),
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
        public DataSet GetSuppRfqList(string Supp_id, string CompId, string Br_ID, string SuppPros_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.Int64, Br_ID),
                                                         objProvider.CreateInitializedParameter("@SuppPros_type",DbType.String, SuppPros_type),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_RFQListForPQ", prmContentGetDetails);
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
        public DataSet GetPRList( string CompId, string Br_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                       // objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.Int64, Br_ID),
                                                      //  objProvider.CreateInitializedParameter("@SuppPros_type",DbType.String, SuppPros_type),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_PRListForPQ", prmContentGetDetails);
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
        public DataSet AddRfqOrPRItemDetailForQtsn(string CompID, string BrchID, string doc_no, string Doc_date,string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.Int64, BrchID),
                                                         objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
                                                         objProvider.CreateInitializedParameter("@Doc_date",DbType.String, Doc_date),
                                                         objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$Item$detail_RfqItemDtlForQtsn", prmContentGetDetails);
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
        public string InsertPQApproveDetails(string PQNo, string PQDate, string CompID, string BrchID, string DocumentMenuId, string UserID, string mac_id, string A_Status, string A_Level, string A_Remarks, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@docno",DbType.String, PQNo),
                                                        objProvider.CreateInitializedParameter("@docdate",DbType.String, PQDate),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String,DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                        objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),                                                        
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "prc$qt$detail_UpdatePQArroveDetails", prmContentInsert).ToString();
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
        public string DeletePQDetails(string CompID, string BrchID, string doc_no, string doc_date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, doc_no),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, doc_date),
                                                        objProvider.CreateInitializedParameter("@Result",DbType.String,""),
                                                    };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "prc$pq$detail_DeleteDetails", prmcontentaddupdate).ToString();
                string Result = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    Result = prmcontentaddupdate[4].Value.ToString();
                }

                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPurchaseQuotationDetails(string Doc_no,string Doc_date, string CompID, string BrchID,string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        //objProvider.CreateInitializedParameter("@qt_no",DbType.Int64, qt_no),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, Doc_no),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, Doc_date),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$qt$detail_GetPurchaseQuotation_RelDetails", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetListOfPQDetails(string comp_id, string br_id,string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        //objProvider.CreateInitializedParameter("@qt_no",DbType.Int64, qt_no),
                                                           objProvider.CreateInitializedParameter("@CompID",DbType.Int64, comp_id),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, br_id),
                                                        objProvider.CreateInitializedParameter("@SupplierID",DbType.String, ""),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, ""),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, ""),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, ""),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String,DocumentMenuId),
                                                      };
                /*Commented By Nitesh 30-03-2024 For Get list data Differnt Procedure*/
                //   DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$qt$detail_GetPurchaseQuotation_RelDetails", prmContentGetDetails);
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Pur$Qua$List$Data", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }         
        }
        public DataTable GetSuppNameList(string CompId, string br_id, string SupplierName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SupplierName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetSuppList]", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllData(string CompId, string br_id, string SupplierName,string UserID, string wfstatus, string DocumentMenuId,
             string SuppID, string PQ_FromDate,string PQ_ToDate, string PQ_status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SupplierName),
                    objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@SupplierID",DbType.String, SuppID),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String, PQ_FromDate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, PQ_ToDate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, PQ_status),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetAllData$Pur$Qua$List]", prmContentGetDetails);
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
        public DataTable GetProsSuppNameList(string CompId, string br_id, string SupplierName,string SuppPros_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                      objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@SearchName",DbType.String, SupplierName),
                     objProvider.CreateInitializedParameter("@SuppPros_type",DbType.String,SuppPros_type),
                     objProvider.CreateInitializedParameter("@Flag",DbType.String, "SuppList"),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[prc$rfq$supp$list]", prmContentGetDetails).Tables[0];
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
        public DataSet GetSearchListOfPQDetails(string CompID, string BrchID, string SupplierID, string Fromdate, string Todate, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@SupplierID",DbType.String, SupplierID),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, ""),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, ""),

                                                      };
                /*Commented By Nitesh 30-03-2024 For Get list data Differnt Procedure*/
                //DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$qt$detail_GetPurchaseQuotation_RelDetails", prmContentGetDetails);
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Pur$Qua$List$Data", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckPOAgainstPQ(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$po$detail_CheckPOAgainstPQ", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubItemDetailsAfterApprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PQ_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPurchaseQuotationDeatils(string Comp_ID, string Br_ID, string PQ_No, string PQ_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@pq_no",DbType.String, PQ_No),
                                                        objProvider.CreateInitializedParameter("@pq_date",DbType.String, PQ_Date),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPurchaseQuotationDeatils_ForPrint", prmContentGetDetails);
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
