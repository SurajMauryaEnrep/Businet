using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ScrapSaleInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ScrapSaleInvoice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.ScrapSaleInvoice
{
    public class ScrapSI_SERVICE: ScrapSI_ISERVICE
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
        public DataSet GetScrapVerifcationList(string Supp_id, string CompId, string BrID)
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
        public DataSet GetScrapVerifcationDetail(string VerNo, string VerDate, string CompId, string BrID)
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
        public string InsertscrpSSI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DTTaxDetail
            , DataTable DtblOCTaxDetail, DataTable DtblIOCDetail, DataTable DtblAttchDetail, DataTable DtblVouDetail
            , DataTable CRCostCenterDetails,DataTable SubitemINVQty/*, DataTable DtblTdsDetail, string Tds_amt*/
            ,DataTable ItemBatchDetails,DataTable DtblOCTdsDetail,string Narr,string CN_Narr,string slprsn_id
            ,DataTable DtblTcsDetail,DataTable ItemSerialDetails, string pay_term, string del_term, string decl_1
            , string decl_2, string inv_heading, string nontaxable,string ShipTo,DataTable DTPaymentSchedule,string corp_addr,string pvt_mark)
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
                                                        objprovider.CreateInitializedParameterTableType("@slprsn_id",SqlDbType.Int,slprsn_id ),
                                                        objprovider.CreateInitializedParameterTableType("@TcsDetails",SqlDbType.Structured, DtblTcsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemSerialDetails",SqlDbType.Structured, ItemSerialDetails),
                                                        objprovider.CreateInitializedParameterTableType("@pay_term",SqlDbType.NVarChar, pay_term),
                                                        objprovider.CreateInitializedParameterTableType("@del_term",SqlDbType.NVarChar, del_term),
                                                        objprovider.CreateInitializedParameterTableType("@decl_1",SqlDbType.NVarChar, decl_1),
                                                        objprovider.CreateInitializedParameterTableType("@decl_2",SqlDbType.NVarChar, decl_2),
                                                        objprovider.CreateInitializedParameterTableType("@inv_heading",SqlDbType.NVarChar, inv_heading),
                                                        objprovider.CreateInitializedParameterTableType("@nontaxable",SqlDbType.NVarChar, nontaxable),
                                                        objprovider.CreateInitializedParameterTableType("@shipTo",SqlDbType.NVarChar, ShipTo),
                                                        objprovider.CreateInitializedParameterTableType("@DTPaymentSchedule",SqlDbType.Structured, DTPaymentSchedule),
                                                        objprovider.CreateInitializedParameterTableType("@corp_addr",SqlDbType.NVarChar, corp_addr),
                                                        objprovider.CreateInitializedParameterTableType("@pvtmark",SqlDbType.NVarChar, pvt_mark),

                                                    };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sls$srp$sinv$detail_Insert$scrpSSI_Details]", prmcontentaddupdate).ToString();
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

        public string ApproveSSI(string Inv_No, string Inv_Date, string MenuDocId, string Branch, string CompID, string UserID
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
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[scrap$sale$invoice$Approve]", prmContentInsert);

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
                DataSet GetVerificationList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetScrapSaleInvoiceList", prmContentGetDetails);
                return GetVerificationList;
            }
            catch (SqlException ex)
            {
                throw ex;
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
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$Service$Scrap$Sales$Invoice$List", prmContentGetDetails);

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
        public DataSet GetScrapSalesInvoiceDetail(string CompId, string BrID, string Voutype, string InvNo, string InvDate, string UserID, string DocID)
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetScrapSaleInvoiceDetail", prmContentGetDetails);
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
        public DataSet getsubitem(string ItemId, string CompId, string BranchId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {
            objProvider.CreateInitializedParameter("@ItemId",DbType.String, ItemId),
            objProvider.CreateInitializedParameter("@CompId",DbType.Int32,  CompId),
            objProvider.CreateInitializedParameter("@BranchId",DbType.String,  BranchId),
            };
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sls$scrp$sinv$subitm]", prmContentGetDetails);
            return DS;
        }
        public DataSet GetWarehouseList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$warehouse_GetWarehouseList", prmContentGetDetails);
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
        public DataTable Scrap_GetSubItemDetails(string CompId, string BrID,string itmid, string docno, string Sinv_dt,string flag)
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
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ScrapSaleInvoice_GetSubItemDetails", prmContentGetDetails).Tables[0];
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
        public DataTable Scrap_GetSubItemDetails_INV_QTY(string CompId, string BrID,string itmid,string wh_id,string UOMID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@itmid",DbType.String, itmid),                  
                    objProvider.CreateInitializedParameter("@wh_id",DbType.String, wh_id),                  
                    objProvider.CreateInitializedParameter("@UOMID",DbType.String, UOMID),                  
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ScrapSaleInvoice_GetSubItemDetails$inv", prmContentGetDetails).Tables[0];
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

        public string ScrapSIDelete(ScrapSIModel _ScrapPIModel, string CompID, string br_id, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, _ScrapPIModel.Sinv_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.Date,  _ScrapPIModel.Sinv_dt),
                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                     };
                prmContentGetDetails[4].Size = 100;
                prmContentGetDetails[4].Direction = ParameterDirection.Output;

                string ActionDeatils = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[sp_Delete_Srcp_ServiceSI_Detail]", prmContentGetDetails).ToString();
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
          , string Doc_dt, string ItemID, string UomId = null )
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
            DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$srp$item$bt$detail_Get_SCRP$Item_StockBatchwise", prmContentGetDetails);
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
                DataSet DS = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$srp$item$sr$detail_GetStockSerialwise", prmContentGetDetails);
                return DS;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetScrapSaleInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date)
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
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetScrapSalesInvoiceDeatils_ForPrint]", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetScrapSlsInvGstDtlForPrint(string compId, string brchId, string siNo, string siDate)
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
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetScrapSlsInvGSTDtl_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
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
                DataSet GetDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_SCRPSI_CheckAgainstCancellation", prmContentGetDetails);
                return GetDt;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public DataTable GetTransporterList(string CompID, string BranchID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //             objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
        //             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
        //        };
        //        DataTable GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP_Get$stp$trans$detail]", prmContentGetDetails).Tables[0];
        //        return GetOCList;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
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
                    objProvider.CreateInitializedParameter("@user_id",DbType.Int32, UserID),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, Sinv_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.Date, Sinv_dt),
                    objProvider.CreateInitializedParameter("@DocType",DbType.String, "DSI"),
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

        public DataSet BindDispatchDetail(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Data$Dispatch$Detail", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetDistrictOnStateDDL(string ddlStateID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@StateID",DbType.String, ddlStateID),
                                                      };
                DataTable Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getDistrict", prmContentGetDetails).Tables[0];
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCityOnDistrictDDL(string ddlDistrictID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@DistrictID",DbType.String, ddlDistrictID),
                                                      };
                DataTable Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getCity", prmContentGetDetails).Tables[0];
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
