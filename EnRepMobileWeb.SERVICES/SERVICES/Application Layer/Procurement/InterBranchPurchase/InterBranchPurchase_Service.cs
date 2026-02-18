using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.InterBranchPurchase;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.InterBranchPurchase
{
    public class InterBranchPurchase_Service : InterBranchPurchase_IService
    {
        public DataSet GetAllData(string CompID, string CustName, string BranchID, string User_ID, string CustId, string Fromdate, string Todate, string Status, string Docid, string wfStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, CustName),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, User_ID),
                    objProvider.CreateInitializedParameter("@SuppId",DbType.String, CustId),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                    objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$IBP$List", prmContentGetDetails);
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

        public DataSet GetBranchList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetBrachList", prmContentGetDetails);
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

        public DataSet GetInterBranchSaleInvoiceDetail(string CompId, string BrID, string Invno, string invdt, string BrchIDCurr,string supp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@InvNo",DbType.String, Invno),
                    objProvider.CreateInitializedParameter("@InvDate",DbType.String, invdt),
                    objProvider.CreateInitializedParameter("@BrchIDCurr",DbType.Int64, BrchIDCurr),
                    objProvider.CreateInitializedParameter("@supp_id",DbType.Int64, supp_id),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetIBSaleInvoiceList", prmContentGetDetails);
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

        //public DataSet GetBillNoList(string CompId, string BrID, string InterBrchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //            objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BrID),
        //            objProvider.CreateInitializedParameter("@InterBrchID",DbType.Int64, BrID),
        //        };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetBillNoList", prmContentGetDetails);
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
        public Dictionary<string, string> GetBillNoList(string CompID, string BranchID, string InterBrchID)
        {
            Dictionary<string, string> ddlListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                   objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                   objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, BranchID),
                   objProvider.CreateInitializedParameter("@InterBrchID",DbType.Int64, InterBrchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetBillNoList", prmContentGetDetails);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlListDic.Add(PARQusData.Tables[0].Rows[i]["inv_no"].ToString(), PARQusData.Tables[0].Rows[i]["Invdt"].ToString());
                    }
                }
                return ddlListDic;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet GetIBPDetail(string CompId, string BrID, string Voutype, string InvNo, string InvDate, string UserID, string DocID)
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetIBPDetail", prmContentGetDetails);
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

        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "IBP_GetSuppList", prmContentGetDetails);
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
        public DataTable CheckRoundOffAcc(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                };
                DataTable acc_flag = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc_checkroundoffacc", prmContentGetDetails).Tables[0];
                return acc_flag;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string Insert_IBPDetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable SubitemINVQty, DataTable ItemBatchDetails, DataTable ItemSerialDetails,
            DataTable DTTaxDetail, DataTable DtblIOCDetail , DataTable DtblOCTaxDetail, DataTable DtblOCTdsDetail, DataTable DtblVouDetail, DataTable DtblAttchDetail
                    , DataTable CRCostCenterDetails , string Narr /*, DataTable DtblTdsDetail, string Tds_amt ,string CN_Narr,*/ )
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                    objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                    objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                    objprovider.CreateInitializedParameterTableType("@SubitemINVQty",SqlDbType.Structured, SubitemINVQty),
                    objprovider.CreateInitializedParameterTableType("@ItemBatchDetails",SqlDbType.Structured, ItemBatchDetails),
                    objprovider.CreateInitializedParameterTableType("@ItemSerialDetails",SqlDbType.Structured, ItemSerialDetails),
                    objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                    objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DtblIOCDetail),
                    objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DtblOCTaxDetail),
                    objprovider.CreateInitializedParameterTableType("@OC_TP_Tds_Details",SqlDbType.Structured, DtblOCTdsDetail),
                    objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouDetail),
                    objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                    objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured, CRCostCenterDetails),
                    // objprovider.CreateInitializedParameterTableType("@CN_Narr",SqlDbType.NVarChar,CN_Narr ),
                    objprovider.CreateInitializedParameterTableType("@Narr",SqlDbType.NVarChar,Narr),
                    objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                    };
                prmcontentaddupdate[13].Size = 100;
                prmcontentaddupdate[13].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "prc$ibp$detail_Insert$IBP_Details", prmcontentaddupdate).ToString();
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

        public string ApproveDPIDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID
           , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks, string VoucherNarr, string Bp_Nurr
           , string Dn_Nurr)
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
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ApprovedIBPDetails", prmContentInsert);

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

        public string DeleteIBPDetail(string CompID, string BrchID, string Inv_no, string Inv_dt)
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
            string dataSet = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteIBP_Details", sqlParameters).ToString();
            string DocNo = string.Empty;
            if (sqlParameters[3].Value != DBNull.Value) // status
            {
                DocNo = sqlParameters[3].Value.ToString();
            }
            return DocNo;
        }

        public DataSet IBP_GetSubItemDetails(string compID, string brchID, string item_id, string doc_no, string doc_dt)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$IBP$getSubitemDetailAfterApprove", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetIBPInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date)
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
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetIBPInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
