using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.SupplementaryPurchaseInvoiceIService;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.SupplementaryPurchaseInvoice
{
    public class SupplementaryPurchaseInvoice_SERVICE : SupplementaryPurchaseInvoice_ISERVICE
    {
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID, string SuppType)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                     objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
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
        }

        public Dictionary<string, string> GetPurchaseInvoicesList1(string CompID, string SuppId, string BranchID, string InvoiceType)
        {
            Dictionary<string, string> ddlPIListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@Suppid",DbType.String, SuppId),
                     objProvider.CreateInitializedParameter("@InvType",DbType.String, InvoiceType),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$spinv$detail_GetPInvList", prmContentGetDetails);
                //DataRow dr;
                //dr = PARQusData.Tables[0].NewRow();
                //dr[0] = "0";
                //dr[1] = "---Select---";
                //PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlPIListDic.Add(PARQusData.Tables[0].Rows[i]["inv_no"].ToString(), PARQusData.Tables[0].Rows[i]["Invdt"].ToString());
                    }
                }
                return ddlPIListDic;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetPurchaseInvoicesList(string CompID, string SuppId, string BranchID, string InvoiceType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                         objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@Suppid",DbType.String, SuppId),
                     objProvider.CreateInitializedParameter("@InvType",DbType.String, InvoiceType),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$spinv$detail_GetPInvList", prmContentGetDetails);
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

        public DataSet GetPurchaseInvoicesDetailsList(string CompID, string InvNo, string BranchID, string InvoiceType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                         objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@InvNo",DbType.String, InvNo),
                     objProvider.CreateInitializedParameter("@InvType",DbType.String, InvoiceType),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$spinv$detail_GetPInvDetails", prmContentGetDetails);
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

        public DataSet GetPurchaseInvoicesItemDetailsList(string CompID, string InvNo, string BranchID, string InvoiceType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                         objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@InvNo",DbType.String, InvNo),
                     objProvider.CreateInitializedParameter("@InvType",DbType.String, InvoiceType),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$spinv$detail_GetPInvItemDetails", prmContentGetDetails);
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
        public DataSet GetAllData(string CompID, string SuppName, string BranchID, string User_ID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                       objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, User_ID),
                      objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                     objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                   objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                   objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                   objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$SuppPI$List", prmContentGetDetails);
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
        public DataSet DeleteDPIDetails(string CompId, string BrID, string Inv_no, string Inv_dt, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, Inv_no),
                                                        objProvider.CreateInitializedParameter("@inv_dt",DbType.String, Inv_dt),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "DeleteSuppPI_Details", prmContentGetDetails);
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
        public DataSet DPI_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, compID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, brchID),
                                                        objProvider.CreateInitializedParameter("@doc_no",DbType.String, doc_no),
                                                        objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                                                        objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                                                      };
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$DPI$getSubitemDetailAfterApprove", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertDPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblSubItem, DataTable DTTaxDetail
    , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblTdsDetail, DataTable DtblOcTdsDetail, DataTable DtblVouDetail, DataTable DtblAttchDetail
    , DataTable CRCostCenterDetails, string Nurr, string TDS_Amount)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                    objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                    objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                    objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured, DtblSubItem),
                    objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                    objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DtblOCTaxDetail),
                    objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DtblIOCDetail),
                    objprovider.CreateInitializedParameterTableType("@DtblTdsDetail",SqlDbType.Structured, DtblTdsDetail),
                    objprovider.CreateInitializedParameterTableType("@DtblOcTdsDetail",SqlDbType.Structured, DtblOcTdsDetail),
                    objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouDetail),
                    objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                    objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                    objprovider.CreateInitializedParameterTableType("@Nurr",SqlDbType.NVarChar, Nurr),
                    objprovider.CreateInitializedParameterTableType("@TDS_Amount",SqlDbType.NVarChar, TDS_Amount),
                    objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                    };
                prmcontentaddupdate[13].Size = 100;
                prmcontentaddupdate[13].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertSuppPI_Details", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[13].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[13].Value.ToString();
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
        public string DeleteDetails(string CompID, string BrchID, string Inv_no, string Inv_dt)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                 sqlDataProvider.CreateInitializedParameter("@CompID",DbType.String, CompID ),
                 sqlDataProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID ),
                 sqlDataProvider.CreateInitializedParameter("@Inv_no",DbType.String, Inv_no ),
                sqlDataProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                sqlDataProvider.CreateInitializedParameter("@Inv_dt",DbType.String, Inv_dt ),

            };
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Size = 100;
            string dataSet = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteSuppPI_Details", sqlParameters).ToString();
            string DocNo = string.Empty;
            if (sqlParameters[3].Value != DBNull.Value) // status
            {
                DocNo = sqlParameters[3].Value.ToString();
            }
            return DocNo;
        }
        public string ApproveDPIDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID
            , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr, string Bp_Nurr
            , string Dn_Nurr, string Cn_Nurr)
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
                                                         objProvider.CreateInitializedParameter("@Cn_Narr",DbType.String, Cn_Nurr),

                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ApprovedSuppPIDetails", prmContentInsert);

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
        public DataSet GetDirectPurchaseInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, SI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, SI_Date),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDirectPurchasInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetWarehouseList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetDestWarehouseList", prmContentGetDetails);
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
        public DataSet CheckSuppPIDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckAgainstSuppPI", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetVarienceDetails(string CompId, string BrchID, string GRNNo, string GRNDate, string ItmCode)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@GRNNo",DbType.String, GRNNo),
                                                        objProvider.CreateInitializedParameter("@GRNDate",DbType.String, GRNDate),
                                                        objProvider.CreateInitializedParameter("@ItmCode",DbType.String, ItmCode),
                                                      };
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$Pinv$GetVarienceDetail", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet PI_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, compID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, brchID),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, flag),
                                                      };
                DataSet GetTaxList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "PI_GetSubItemDetails", prmContentGetDetails);
                return GetTaxList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetDPIDetailDAL(string CompId, string BrID, string Inv_no, string Inv_dt, string UserID, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@InvNo",DbType.String, Inv_no),
                                                        objProvider.CreateInitializedParameter("@inv_dt",DbType.String, Inv_dt),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSuppPI_Details", prmContentGetDetails);
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

        public string Approve_SuppPI(string Inv_No, string Inv_Date, string Inv_Type, string MenuDocId, string Branch, string CompID
            , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr, string Bp_Nurr
            , string Dn_Nurr)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@invno",DbType.String, Inv_No),
                                                        objProvider.CreateInitializedParameter("@invdate",DbType.String, Inv_Date),
                                                        objProvider.CreateInitializedParameter("@invtype",DbType.String, Inv_Type ),
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
                prmContentInsert[7].Size = 100;
                prmContentInsert[7].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$spinv$detail_Approved_SuppPI_Details", prmContentInsert);

                string DocNo = string.Empty;
                if (prmContentInsert[7].Value != DBNull.Value)
                {
                    DocNo = prmContentInsert[7].Value.ToString();
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
        public DataSet CheckPIDetail(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_CheckCancelAgainstSPI" + "", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }

    public class SupplementaryPurchaseInvoiceList_SERVICE : SupplementaryPurchaseInvoiceList_ISERVICE
    {
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
        public DataSet GetSPI_DetailList(string CompId, string BrchID, string UserID, string SuppId, string Fromdate, string Todate, string Status, string Docid, string wfStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                          objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$spinv$detail_GetSuppPIDeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        
    }
}
