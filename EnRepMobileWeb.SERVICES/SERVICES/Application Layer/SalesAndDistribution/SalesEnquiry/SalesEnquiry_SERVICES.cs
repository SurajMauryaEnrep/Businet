using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesEnquiry;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesEnquiry;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.SalesEnquiry
{
  public  class SalesEnquiry_SERVICES : SalesEnquiry_ISERVICES
    {
        public Dictionary<string, string> GetCustomerList(string CompID, string CustomerName, string BranchID, string CustProstype, string Enquiry_type)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, Enquiry_type),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                      objProvider.CreateInitializedParameter("@CustPros_type",DbType.String, CustProstype),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustListOnQuotation", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_name"].ToString());
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

        }

        public DataSet GetAllData(string CompID, string CustomerName, string BranchID, string CustProstype, string Enquiry_type, string SPersonName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, Enquiry_type),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                      objProvider.CreateInitializedParameter("@CustPros_type",DbType.String, CustProstype),
                      objProvider.CreateInitializedParameter("@SPersonName",DbType.String, SPersonName),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_SlsEnqry$GetAllDDlAndListData", prmContentGetDetails);

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
        public string InsertSE_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblCommunicationDetail, DataTable dtSubItem, DataTable DtblAttchDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@CommunicationDetail",SqlDbType.Structured,DtblCommunicationDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                         objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SLS$SlsEnqry$InsertSE_Details", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[2].Value.ToString();
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
        public DataSet Edit_SEDetail(string CompId, string BrID, string DocumentMenuID, string UserID, string SENo )
        {
            try
            {
                DataSet searchmenu = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();

                
                    SqlParameter[] prmContentGetDetails = {
                                                        
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        objProvider.CreateInitializedParameter("@SENo",DbType.String, SENo),
                                                   };
                    searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_SLS$SlsEnqry$GetViewDetails", prmContentGetDetails);
                

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
        public DataSet GetSlsEnqryListandSrchDetail(string CompId, string BrchID, SEListModel _SEListModel, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@Enqtype",DbType.String,_SEListModel.EnqryTyp),
                                                        objProvider.CreateInitializedParameter("@Custtype",DbType.String,_SEListModel.CustTyp),
                                                        objProvider.CreateInitializedParameter("@EnqSrc",DbType.String,_SEListModel.EnqSrc),
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String,_SEListModel.CustName),
                                                        objProvider.CreateInitializedParameter("@CustRegn",DbType.String,_SEListModel.Region),
                                                        objProvider.CreateInitializedParameter("@Custport",DbType.String,_SEListModel.Portfolio),
                                                        objProvider.CreateInitializedParameter("@Custcat",DbType.String,_SEListModel.Catgry),
                                                        objProvider.CreateInitializedParameter("@SlsPrsn",DbType.String,_SEListModel.SlsPrsn),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_SEListModel.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_SEListModel.ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _SEListModel.Status),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_SLS$SlsEnqy$GetListandSrchDetail", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SEdetailDelete(SalesEnquiryModel SEModel, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@ENQNo",DbType.String, SEModel.EnquiryNo ),
                                                        objProvider.CreateInitializedParameter("@ENQDate",DbType.String,SEModel.EnquiryDt),
                };
                string SOId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "SP_SLS$SlsEnqry$DeleteDetails", prmContentInsert).ToString();
                return SOId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetPrintDeatils(string CompID, string BrchID, string EnquiryNo, string EnquiryDt)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),

                      objProvider.CreateInitializedParameter("@EnqNo",DbType.String, EnquiryNo),
                      objProvider.CreateInitializedParameter("@EnqDate",DbType.String, EnquiryDt),
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP_SLS$SlsEnqry$PrintDetails]", prmContentGetDetails);
            return ds;
        }
        public DataSet SE_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt/*, string Flag*/)
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
                    //objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_SLS$SlsEnqry$GetSubItemDetailsAfterStatuschng", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
