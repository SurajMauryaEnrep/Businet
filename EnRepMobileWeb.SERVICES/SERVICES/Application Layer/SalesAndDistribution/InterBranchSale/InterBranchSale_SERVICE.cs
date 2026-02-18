using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.InterBranchSale;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.InterBranchSale
{
     public class InterBranchSale_SERVICE: InterBranchSale_ISERVICE
    {
        public DataSet GetAllData(string CompID, string CustName, string BranchID, string User_ID, string CustId, string Fromdate, string Todate, string Status, string Docid, string wfStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustName),
                       objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String, User_ID),
                      objProvider.CreateInitializedParameter("@CustId",DbType.String, CustId),
                     objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                   objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                   objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                   objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$IBS$List", prmContentGetDetails);
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
        public Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID, string CustType,string DocId)
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
                    objProvider.CreateInitializedParameter("@DocId",DbType.String, DocId),
                                                     };
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
        public DataSet GetCustAddrDetailDL(string Cust_id, string CompId, string br_id, string DocumentMenuId)
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
        public DataSet GetIBSDetail(string CompId, string BrID, string Voutype, string InvNo, string InvDate, string UserID, string DocID)
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetIBSDetail", prmContentGetDetails);
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
        public Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID,string UserID)
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
        public DataTable Scrap_GetSubItemDetails(string CompId, string BrID, string itmid, string docno, string Sinv_dt, string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@itmid",DbType.String, itmid),
                    objProvider.CreateInitializedParameter("@docno",DbType.String, docno),
                    objProvider.CreateInitializedParameter("@Sinv_dt",DbType.String, Sinv_dt),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "InterBranchSale_GetSubItemDetails", prmContentGetDetails).Tables[0];
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
        public DataSet getItemStockBatchWise(string ItemId, string UomId, string WarehouseId, string CompId, string BranchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, WarehouseId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockBatchwise]", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemStockBatchWiseAfterStockUpdate(string CompID, string BrID, string Doc_No
          , string Doc_dt, string ItemID, string UomId = null)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@DocNo",DbType.String, Doc_No),
            objProvider.CreateInitializedParameter("@Docdt",DbType.String, Doc_dt),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            //objProvider.CreateInitializedParameter("@UomId",DbType.String, UomId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ibs$item$bt$detail_Get_StockBatchwise", prmContentGetDetails);
            return DS;
        }
        public DataSet getItemstockSerialWise(string CompId, string BranchId, string ItemId, string Wh_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@WarehouseId",DbType.Int32, Wh_ID),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[inv$stk$detail_GetItemStockSerialwise]", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getItemstockSerialWiseAfterStockUpdate(string CompID, string BrID, string Doc_no, string Doc_dt, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,CompID),
            objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrID),
            objProvider.CreateInitializedParameter("@Doc_no",DbType.String, Doc_no),
            objProvider.CreateInitializedParameter("@Doc_dt",DbType.String, Doc_dt),
            objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
            };
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ibs$item$sr$detail_GetStockSerialwise", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string InsertIBSDetails(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            , DataTable CRCostCenterDetails, DataTable SubitemINVQty/*, DataTable DtblTdsDetail, string Tds_amt*/
            , DataTable ItemBatchDetails, DataTable DtblOCTdsDetail, string Narr, string CN_Narr, DataTable ItemSerialDetails)
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
                                                        objprovider.CreateInitializedParameterTableType("@SubitemINVQty",SqlDbType.Structured, SubitemINVQty),
                                                        objprovider.CreateInitializedParameterTableType("@ItemBatchDetails",SqlDbType.Structured, ItemBatchDetails),
                                                        objprovider.CreateInitializedParameterTableType("@OC_TP_Tds_Details",SqlDbType.Structured, DtblOCTdsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@Narr",SqlDbType.NVarChar,Narr ),
                                                        objprovider.CreateInitializedParameterTableType("@CN_Narr",SqlDbType.NVarChar,CN_Narr ),
                                                        objprovider.CreateInitializedParameterTableType("@ItemSerialDetails",SqlDbType.Structured, ItemSerialDetails),

                                                    };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sls$ibs$detail_Insert$IBS_Details]", prmcontentaddupdate).ToString();
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
        public string DeleteIBSDetails(string CompID, string BrchID, string Inv_no, string Inv_dt)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                 sqlDataProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID ),
                 sqlDataProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID ),
                 sqlDataProvider.CreateInitializedParameter("@doc_no",DbType.String, Inv_no ),
                sqlDataProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                sqlDataProvider.CreateInitializedParameter("@doc_dt",DbType.String, Inv_dt ),

            };
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Size = 100;
            string dataSet = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Delete_Ibs_Detail", sqlParameters).ToString();
            string DocNo = string.Empty;
            if (sqlParameters[3].Value != DBNull.Value) // status
            {
                DocNo = sqlParameters[3].Value.ToString();
            }
            return DocNo;
        }
        public string ApproveIBSDetail(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID, string UserID
            , string mac_id, string wf_status, string wf_level, string wf_remarks, string SaleVouMsg
            , string PV_VoucherNarr, string BP_VoucherNarr, string DN_VoucherNarr, string DN_Nurr_Tcs)
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
                                                         objProvider.CreateInitializedParameter("@Dn_Narr_Tcs",DbType.String, DN_Nurr_Tcs),

                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[IBSApprove]", prmContentInsert);
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
        public DataSet GetIBSDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, SI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, SI_Date),
                                                        //objProvider.CreateInitializedParameter("@flag",DbType.String, inv_type),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetIBSDeatils_ForPrint]", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetIBSGstDtlForPrint(string compId, string brchId, string siNo, string siDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brchId),
                                                        objProvider.CreateInitializedParameter("@InvNo",DbType.String, siNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, siDate),
                                                        //objProvider.CreateInitializedParameter("@Flag",DbType.String, invType),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetIBSGSTDtl_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet checkDependencyIBP(string Comp_ID, string brchId, string SI_No, string SI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                    objProvider.CreateInitializedParameter("@Br_ID",DbType.String,brchId),
                    objProvider.CreateInitializedParameter("@Inv_no",DbType.String, SI_No),
                     objProvider.CreateInitializedParameter("@Inv_dt",DbType.String, SI_Date),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_sls$IBP$InvDependency", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
