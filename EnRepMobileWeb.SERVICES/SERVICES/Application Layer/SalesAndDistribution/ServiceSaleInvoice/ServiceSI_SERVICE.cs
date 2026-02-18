using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ServiceSaleInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ServiceSaleInvoice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.ServiceSaleInvoice
{
    public class ServiceSI_SERVICE: ServiceSI_ISERVICE
    {
        public Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, ""),
                                                     };
                //DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustList", prmContentGetDetails);
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Cmn_GetCustList", prmContentGetDetails);
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
        public DataSet GetAllData(string CompID, string SuppName, string BranchID, string CustType,
             string UserID, string CustID, string Fromdate, string Todate, string Status, string Docid, string wfStatus,string SalesPerson)
        {
        
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                     objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                    objProvider.CreateInitializedParameter("@sls_per",DbType.String, SalesPerson),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$Service$Sales$Invoice$List", prmContentGetDetails);
             
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
        public DataSet GetCustAddrDetailDL(string Cust_id, string CompId,string br_id,string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String, Cust_id),
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String, DocumentMenuId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustAddrDetails", prmContentGetDetails);
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
        public DataSet GetServiceVerifcationList(string Supp_id, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@SuppID",DbType.String, Supp_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetServiceVerficationNo", prmContentGetDetails);
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
        public DataSet GetServiceVerifcationDetail(string VerNo, string VerDate, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@VerNo",DbType.String, VerNo),
                    objProvider.CreateInitializedParameter("@VerDate",DbType.String, VerDate),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetServiceVerificationDetail", prmContentGetDetails);
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
        public DataSet CheckSSIDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckReturnAgainst_SSI", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertSSI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail,DataTable DtblOCTdsDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            , DataTable CRCostCenterDetails,string Narr,string CN_Narr,string slprsn_id, string cust_ref, string pay_term, string del_term, string decl_1
            , string decl_2, string inv_heading, string nontaxable,string Ship_Add_Id,string PlcOfSupply,string roundof, string pm_flag,string ShipTo,DataTable DTPaymentSchedule)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DtblIOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouDetail),
                                                        objprovider.CreateInitializedParameterTableType("@Nurr",SqlDbType.NVarChar,Narr ),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DtblOCTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OC_TP_Tds_Details",SqlDbType.Structured, DtblOCTdsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                                                        objprovider.CreateInitializedParameterTableType("@CN_Nurr",SqlDbType.NVarChar, CN_Narr),
                                                        objprovider.CreateInitializedParameterTableType("@slprsn_id",SqlDbType.Int, slprsn_id),
                                                        objprovider.CreateInitializedParameterTableType("@cust_ref",SqlDbType.NVarChar, cust_ref),
                                                        objprovider.CreateInitializedParameterTableType("@pay_term",SqlDbType.NVarChar, pay_term),
                                                        objprovider.CreateInitializedParameterTableType("@del_term",SqlDbType.NVarChar, del_term),
                                                        objprovider.CreateInitializedParameterTableType("@decl_1",SqlDbType.NVarChar, decl_1),
                                                        objprovider.CreateInitializedParameterTableType("@decl_2",SqlDbType.NVarChar, decl_2),
                                                        objprovider.CreateInitializedParameterTableType("@inv_heading",SqlDbType.NVarChar, inv_heading),
                                                        objprovider.CreateInitializedParameterTableType("@nontaxable",SqlDbType.NVarChar, nontaxable),
                                                        objprovider.CreateInitializedParameterTableType("@Ship_Add_Id",SqlDbType.NVarChar, Ship_Add_Id),
                                                        objprovider.CreateInitializedParameterTableType("@PlcOfSupply",SqlDbType.NVarChar, PlcOfSupply),
                                                        objprovider.CreateInitializedParameterTableType("@roundof",SqlDbType.NVarChar, roundof),
                                                        objprovider.CreateInitializedParameterTableType("@pm_flag",SqlDbType.NVarChar, pm_flag),
                                                        objprovider.CreateInitializedParameterTableType("@shipTo",SqlDbType.NVarChar, ShipTo),
                                                        objprovider.CreateInitializedParameterTableType("@DTPaymentSchedule",SqlDbType.Structured, DTPaymentSchedule),
                                                    };
                prmcontentaddupdate[7].Size = 100;
                prmcontentaddupdate[7].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sls$sinv$detail_InsertSSI_Details]", prmcontentaddupdate).ToString();
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

            finally
            {
            }
        }

        public string ApproveSSI(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID, string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string SaleVouMsg, string PV_VoucherNarr, string BP_VoucherNarr, string DN_VoucherNarr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@invno",DbType.String, Inv_No),
                                                        objProvider.CreateInitializedParameter("@invdate",DbType.String, Inv_Date),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String, MenuDocId),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                         objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                                                         objProvider.CreateInitializedParameter("@VoucherNarr",DbType.String, SaleVouMsg),
                                                         objProvider.CreateInitializedParameter("@Pv_Narr",DbType.String, PV_VoucherNarr),
                                                         objProvider.CreateInitializedParameter("@Bp_Narr",DbType.String, BP_VoucherNarr),
                                                         objProvider.CreateInitializedParameter("@Dn_Narr",DbType.String, DN_VoucherNarr),
                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_ApproveSSI_Details]", prmContentInsert);

                string DocNo = string.Empty;
                if (prmContentInsert[6].Value != DBNull.Value)
                {
                    DocNo = prmContentInsert[6].Value.ToString();
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
        public DataSet EditSSIDetail(string CompId, string BrID, string PINo, string PIDate, string Type, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrID),
                                                        objProvider.CreateInitializedParameter("@PINo",DbType.String, PINo),
                                                        objProvider.CreateInitializedParameter("@PIDate",DbType.String, PIDate),
                                                        objProvider.CreateInitializedParameter("@PIType",DbType.String, Type),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pinv$detail_GetPI_Details", prmContentGetDetails);
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
        public DataSet GetSSIAttatchDetailEdit(string CompID, string BrchID, string PI_No, string PI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@PI_No",DbType.String, PI_No),
                                                        objProvider.CreateInitializedParameter("@PI_Date",DbType.String, PI_Date),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$pi$getAttatchmentDetail", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllGLDetails(DataTable GLDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@GLDetail",SqlDbType.Structured,GLDetail),
                                                    };

                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetGLDetail", prmcontentaddupdate);
                return GetGlDt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
        }
        public DataSet GetSSIList(string CompId, string BrchID, string UserID, string CustID, string Fromdate, string Todate, string Status, string Docid, string wfStatus,string sales_person)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                                                        objProvider.CreateInitializedParameter("@sls_per",DbType.Int32, sales_person),

                                                      };
                DataSet GetVerificationList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetServiceSaleInvoiceList", prmContentGetDetails);
                return GetVerificationList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetServiceSalesInvoiceDetail(string CompId, string BrID, string Voutype, string InvNo, string InvDate, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@InvNo",DbType.String, InvNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, InvDate),
                                                        objProvider.CreateInitializedParameter("@Voutype",DbType.String, Voutype),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetServiceSaleInvoiceDetail", prmContentGetDetails);
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
        public string ServiceSIDelete(ServiceSIModel _ServicePIModel, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, _ServicePIModel.Sinv_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.Date,  _ServicePIModel.Sinv_dt),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                     };
                prmContentGetDetails[4].Size = 100;
                prmContentGetDetails[4].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_Delete_ServiceSI_Detail]", prmContentGetDetails).ToString();
                //return ActionDeatils;
                string DocNo = string.Empty;
                if (prmContentGetDetails[4].Value != DBNull.Value) // status
                {
                    DocNo = prmContentGetDetails[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSlsInvGstDtlForPrint(string compId, string brchId, string siNo, string siDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brchId),
                                                        objProvider.CreateInitializedParameter("@InvNo",DbType.String, siNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, siDate),
                                                       
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSlsSerInvGSTDtl_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID,string UserID,string Sinv_no,string Sinv_dt)
        {
            Dictionary<string, string> ddlSalesPersonListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SPersonName",DbType.String, SPersonName),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@user_id",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, Sinv_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, Sinv_dt),
                    objProvider.CreateInitializedParameter("@DocType",DbType.String, "SSI"),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSalesPersonDocWise", prmContentGetDetails);
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

        public DataSet GetSrcDocNumberList(string Cust_id, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@Custid",DbType.String, Cust_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$Service$Sinv$srcDocNo$List", prmContentGetDetails);
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
        public DataSet GetItemDetailData(string Cust_id, string Comp_ID, string Br_ID, string SourceDocNo, string SourceDocdt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, Comp_ID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, Br_ID),
                    objProvider.CreateInitializedParameter("@Custid",DbType.String, Cust_id),
                    objProvider.CreateInitializedParameter("@SourceDocNo",DbType.String, SourceDocNo),
                    objProvider.CreateInitializedParameter("@SourceDocdt",DbType.String, SourceDocdt),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$Service$getitemdetail$data ", prmContentGetDetails);
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
