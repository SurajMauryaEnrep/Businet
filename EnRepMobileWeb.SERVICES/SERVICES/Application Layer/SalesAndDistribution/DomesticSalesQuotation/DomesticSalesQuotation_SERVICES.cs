using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSalesQuotation;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.DomesticSalesQuotation
{
   public class DomesticSalesQuotation_SERVICES : DomesticSalesQuotation_ISERVICES
    {
        public string InsertSQ_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail, DataTable DTOCDetail, DataTable DtblOCTaxDetail , DataTable DTTermsDetail, DataTable dtSubItem,  DataTable DtblAttchDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DTOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TermsDetail",SqlDbType.Structured,DTTermsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, dtSubItem ),
                                                        objprovider.CreateInitializedParameterTableType("@OC_TaxDetail",SqlDbType.Structured, DtblOCTaxDetail ),
                };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_sls$qt$detail_InsertQt_Details", prmcontentaddupdate).ToString();
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

            finally
            {
            }
        }
        public DataTable GetTaxTypeList(string CompID, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                };
                DataTable GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$tax$setup_GetTaxTypes", prmContentGetDetails).Tables[0];
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID)
        {
            Dictionary<string, string> ddlSalesPersonListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SPersonName",DbType.String, SPersonName),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$sls$person$comp_GetSalesPersonList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSalesPersonListDic.Add(PARQusData.Tables[0].Rows[i]["sls_pers_id"].ToString(), PARQusData.Tables[0].Rows[i]["sls_pers_name"].ToString());
                    }
                }
                return ddlSalesPersonListDic;

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
        public DataSet Edit_SQDetail(string SQNo, string CompId, string BrID, string DocumentMenuID, string UserID,string Flag,string rev_no)
        {
            try
            {
                DataSet searchmenu = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();
                
                if (Flag== "AmendDocument")
                {
                    SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@QTNo",DbType.String, SQNo),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        objProvider.CreateInitializedParameter("@rev_no",DbType.Int64, rev_no),

                                                   };
                    searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetQTViewDetails$AmendDocument", prmContentGetDetails);
                }
                else
                {
                    SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@QTNo",DbType.String, SQNo),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),

                                                   };
                    searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetQTViewDetails", prmContentGetDetails);
                }
               
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
        public string QTApproveDetails(string CompID, string BrchID, string QTNo, string QTDate, string UserID, string MenuID, string mac_id,  string A_Status, string A_Level, string A_Remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                              objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                              objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                              objProvider.CreateInitializedParameter("@docno",DbType.String, QTNo),
                                                              objProvider.CreateInitializedParameter("@docdate",DbType.String, QTDate),
                                                              objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                              objProvider.CreateInitializedParameter("@DocMenuId",DbType.String, MenuID),
                                                              objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                              objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                              objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                              objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                };
                string POId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_QTArroveDetails", prmContentInsert).ToString();
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
        public DataSet CheckSODetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ChecSOAgainstQuotation", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
       
        public string QTdetailDelete(DomesticSalesQuotationModel _DomesticSalesQuotationModel, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@QTNo",DbType.String, _DomesticSalesQuotationModel.SQ_no ),
                                                        objProvider.CreateInitializedParameter("@QTDate",DbType.String,_DomesticSalesQuotationModel.SQ_dt),
                };
                string SOId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_DeleteQuotaionDetails", prmContentInsert).ToString();
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
        public DataTable GetCustAddressdetail(string CustID, string CompId, string CustPros_type, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                          objProvider.CreateInitializedParameter("@CustPros_type",DbType.String, CustPros_type),
                                                           objProvider.CreateInitializedParameter("@BranchID",DbType.String, BranchID),
                                                      };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetCustomerQuotationAddressDetail", prmContentGetDetails).Tables[0];
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
        public Dictionary<string, string> GetCustomerList(string CompID, string CustomerName, string BranchID, string CustProstype, string Cust_type)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, Cust_type),
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
        public DataSet GetAllData(string CompID, string CustomerName, string BranchID,string UserID, string CustProstype, string Cust_type, string SPersonName,string SQ_no,string SQ_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, Cust_type),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@userid",DbType.Int32, UserID),
                      objProvider.CreateInitializedParameter("@CustPros_type",DbType.String, CustProstype),
                      objProvider.CreateInitializedParameter("@SPersonName",DbType.String, SPersonName),
                      objProvider.CreateInitializedParameter("@SQ_no",DbType.String, SQ_no),
                      objProvider.CreateInitializedParameter("@SQ_dt",DbType.Date, SQ_dt),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$sales$Quot$Detail", prmContentGetDetails);
               
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
        public DataSet GetCustAddrDetailDL(string Cust_id, string CompId, string BranchID, string CustPros_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String, Cust_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                       objProvider.CreateInitializedParameter("@CustPros_type",DbType.String, CustPros_type),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustAddrDetailsforQuotation", prmContentGetDetails);
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
        public DataSet GetItemCustomerInfo(string ItemID, string CustID, string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                                                         objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$detail_GetCustInfoDetails", prmContentGetDetails);
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
        public DataSet GetPriceListRate(string CompId, string BrID, string Item_id, string PPolicy, string PGroup, string Cust_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {objProvider.CreateInitializedParameter("@CompId",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BranchId",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, Item_id),
                                                        objProvider.CreateInitializedParameter("@PPolicy",DbType.String, PPolicy),
                                                        objProvider.CreateInitializedParameter("@PGroup",DbType.String, PGroup),
                                                        objProvider.CreateInitializedParameter("@Cust_id",DbType.String, Cust_id),

                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetItemWisePriceList]", prmContentGetDetails);
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
        public DataSet GetQTAttatchDetailEdit(string CompID, string BrchID, string QT_No)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@QT_No",DbType.String, QT_No),
                                                        //objProvider.CreateInitializedParameter("@PI_Date",DbType.String, PI_Date),
                                                      };
                DataSet Get_QT_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$QT$getAttatchmentDetail", prmContentGetDetails);
                return Get_QT_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesQuotationDeatils(string CompID, string BrchID, string QT_No, string QT_Dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SQ_no",DbType.String, QT_No),
                                                        objProvider.CreateInitializedParameter("@SQ_date",DbType.String, QT_Dt),
                                                      };
                DataSet result = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSalesQuotationDeatils_ForPrint", prmContentGetDetails);
                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet SQ_GetSubItemDetails(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt, string Flag)
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
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SQ_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSlsQtGstDtlForPrint(string compId, string brchId, string SQtNo, string SQtDt, string Qt_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brchId),
                                                        objProvider.CreateInitializedParameter("@QtNo",DbType.String, SQtNo),
                                                        objProvider.CreateInitializedParameter("@QtDate",DbType.String, SQtDt),
                                                        objProvider.CreateInitializedParameter("@Flag",DbType.String, Qt_type),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSlsQTGSTDtl_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
