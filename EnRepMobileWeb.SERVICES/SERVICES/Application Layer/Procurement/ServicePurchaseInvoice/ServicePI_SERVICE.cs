using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServicePurchaseInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ServicePurchaseInvoice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.ServicePurchaseInvoice
{
    public class ServicePI_SERVICE : ServicePI_ISERVICE
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
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, 'D'),
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
            return null;
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
        public DataSet CheckSPIDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckReturnAgainst_SPI", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertSPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            ,DataTable CRCostCenterDetails,DataTable DtblTdsDetail,DataTable DtblOcTdsDetail, string Tds_amt,string Bp_Narr,string Dn_Narr)
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
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DtblOCTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                                                        objprovider.CreateInitializedParameterTableType("@TDS_Details",SqlDbType.Structured, DtblTdsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OC_TP_Tds_Details",SqlDbType.Structured, DtblOcTdsDetail),
                                                        objprovider.CreateInitializedParameter("@TDS_Amt",DbType.String, Tds_amt),
                                                        objprovider.CreateInitializedParameter("@Bp_Narr",DbType.String, Bp_Narr),
                                                        objprovider.CreateInitializedParameter("@Dn_Narr",DbType.String, Dn_Narr),

                                                    };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[prc$sinv$detail_InsertSPI_Details]", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[6].Value != DBNull.Value)
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
        
        public string ApproveSPI(string Inv_No, string Inv_Date, string MenuDocId, string Branch
            , string CompID, string UserID, string mac_id, string wf_status, string wf_level
            , string wf_remarks,string VoucherNarr,string Bp_Nurr,string Dn_Nurr)
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
                                                         objProvider.CreateInitializedParameter("@VoucherNarr",DbType.String, VoucherNarr),
                                                         objProvider.CreateInitializedParameter("@Bp_Narr",DbType.String, Bp_Nurr),
                                                         objProvider.CreateInitializedParameter("@Dn_Narr",DbType.String, Dn_Nurr),

                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_ApproveSPI_Details", prmContentInsert);

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
        public DataSet EditSPIDetail(string CompId, string BrID, string PINo, string PIDate, string Type, string UserID, string DocID)
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
        public DataSet GetSPIAttatchDetailEdit(string CompID, string BrchID, string PI_No, string PI_Date)
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
        public DataSet GetSPIList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),

                                                      };
                DataSet GetVerificationList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetServicePurchaseInvoiceList", prmContentGetDetails);
                return GetVerificationList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetServicePurchaseInvoiceDetail(string CompId, string BrID,string Voutype, string InvNo, string InvDate, string UserID, string DocID)
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetServicePurchaseInvoiceDetail", prmContentGetDetails);
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
        public string ServicePIDelete(ServicePIModel _ServicePIModel, string CompID, string br_id, string DocumentMenuId)
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

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_Delete_ServicePI_Detail]", prmContentGetDetails).ToString();
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
        public DataSet GetServicePurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, SI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, SI_Date)
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetServicePurchasInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet CheckDuplicateBillNo(string CompId, string BrID, string supp_id, string Bill_no,string doc_id,string bill_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@supp_id",DbType.String, supp_id),
                                                        objProvider.CreateInitializedParameter("@bill_no",DbType.String, Bill_no),
                                                        objProvider.CreateInitializedParameter("@doc_id",DbType.String, doc_id),
                                                        objProvider.CreateInitializedParameter("@bill_dt",DbType.String, bill_dt)
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetCheckduplicateBillNo", prmContentGetDetails);
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

