using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSaleInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSaleInvoice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.DomesticSaleInvoice
{
    public class DomesticSaleInvoice_Service: DomesticSaleInvoice_ISERVICE
    {
        public Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID, string CustType,string DocId)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                             objProvider.CreateInitializedParameter("@DocId",DbType.String, DocId),
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
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["cust_id"].ToString(), PARQusData.Tables[0].Rows[i]["cust_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }

        }
        public DataSet GetAllData(string CompID, string Cust_Name, string BrchID, string CustType, string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus,string SalesPerson)
        {
            
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                              objProvider.CreateInitializedParameter("@CustId",DbType.String, CustId),
                            objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                            objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                            objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                            objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                            objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                            objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                            objProvider.CreateInitializedParameter("@sls_per",DbType.String, SalesPerson),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$sales$Invoice$list$Data", prmContentGetDetails);
                
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
        public Dictionary<string, string> GetSalesPersonList(string CompID, string SPersonName, string BranchID,string UserID,string SI_Number,string SI_Date)
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
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String, SI_Number),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.Date, SI_Date),
                    objProvider.CreateInitializedParameter("@DocType",DbType.String, "LSI"),
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
        public DataTable GetOCList(string CompID, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                };
                DataTable GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$oc$setup_GetOtherChargeList", prmContentGetDetails).Tables[0];
                return GetOCList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public DataSet GetShipmentList(string Cust_id, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@CustID",DbType.String, Cust_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ship$detail_GetShipmentList", prmContentGetDetails);
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
        public DataTable Getcurr_details(string CompId, string BrID,string ship_no,string ship_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@ship_no",DbType.String, ship_no),
                    objProvider.CreateInitializedParameter("@ship_dt",DbType.String, ship_dt),
                   

                };
                DataTable curr_details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ship$detail_Getcurrdetails", prmContentGetDetails).Tables[0];
                return curr_details;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetSalePerson_details(string CompId, string BrID, string ship_no, string ship_dt)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@ship_no",DbType.String, ship_no),
                    objProvider.CreateInitializedParameter("@ship_dt",DbType.String, ship_dt),
                  

                };
                DataTable curr_details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get$sale$person", prmContentGetDetails).Tables[0];
                return curr_details;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet GetShipmentDetail(string ShipmentNo, string ShipmentDate, string CompId, string BrID,string Flag,string Inv_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@ShipmentNo",DbType.String, ShipmentNo),
                    objProvider.CreateInitializedParameter("@ShipmentDate",DbType.String, ShipmentDate),
                    objProvider.CreateInitializedParameter("@flag",DbType.String, Flag),
                    objProvider.CreateInitializedParameter("@inv_type",DbType.String, Inv_type),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ship$detail_GetShipmentDetail", prmContentGetDetails);
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
        public string InsertSI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblVouGLDetail
            , DataTable DTTaxDetail,DataTable DTOCTaxDetail, DataTable DTOCDetail,DataTable DtblOCTdsDetail
            , DataTable dtSubItem, DataTable DtblAttchDetail,DataTable CRCostCenterDetails, DataTable DtblTcsDetail
            , string Nurr,string CN_Nurr, string inv_disc_amt, string inv_disc_perc, string decl_1
            , string decl_2, string inv_heading, string nontaxable,string ShipTo, DataTable DTPaymentSchedule,string corp_addr,string remarks)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouGLDetail),
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured, DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@OCDetail",SqlDbType.Structured,DTOCDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@OCTaxDetail",SqlDbType.Structured, DTOCTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@SubItemDetail",SqlDbType.Structured,dtSubItem ),
                                                        objprovider.CreateInitializedParameterTableType("@CostCenterDetail",SqlDbType.Structured,CRCostCenterDetails ),
                                                        objprovider.CreateInitializedParameterTableType("@Nurr",SqlDbType.NVarChar,Nurr ),
                                                        objprovider.CreateInitializedParameterTableType("@CN_Nurr",SqlDbType.NVarChar,CN_Nurr ),
                                                        objprovider.CreateInitializedParameterTableType("@OC_TP_Tds_Details",SqlDbType.Structured,DtblOCTdsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@SI_TcsDetails",SqlDbType.Structured,DtblTcsDetail),
                                                        objprovider.CreateInitializedParameterTableType("@InvDiscAmt",SqlDbType.NVarChar,inv_disc_amt ),
                                                        objprovider.CreateInitializedParameterTableType("@InvDiscPerc",SqlDbType.NVarChar,inv_disc_perc ),
                                                        objprovider.CreateInitializedParameterTableType("@decl_1",SqlDbType.NVarChar, decl_1),
                                                        objprovider.CreateInitializedParameterTableType("@decl_2",SqlDbType.NVarChar, decl_2),
                                                        objprovider.CreateInitializedParameterTableType("@inv_heading",SqlDbType.NVarChar, inv_heading),
                                                        objprovider.CreateInitializedParameterTableType("@nontaxable",SqlDbType.NVarChar, nontaxable),                                            
                                                        objprovider.CreateInitializedParameterTableType("@ShipTo",SqlDbType.NVarChar, ShipTo),                                            
                                                        objprovider.CreateInitializedParameterTableType("@DTPaymentSchedule",SqlDbType.Structured, DTPaymentSchedule),
                                                        objprovider.CreateInitializedParameterTableType("@corp_addr",SqlDbType.NVarChar, corp_addr),
                                                        objprovider.CreateInitializedParameterTableType("@remarks",SqlDbType.NVarChar, remarks),
            };
                prmcontentaddupdate[6].Size = 100;
                prmcontentaddupdate[6].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sls$inv$detail_InsertSI_Details", prmcontentaddupdate).ToString();
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
        public string Delete_SI_Detail(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@Inv_No",DbType.String,_DomesticSaleInvoice_Model.inv_no),
                                                        objProvider.CreateInitializedParameter("@Inv_Date",DbType.String,_DomesticSaleInvoice_Model.inv_dt),
                                                        objProvider.CreateInitializedParameter("@Inv_Type",DbType.String,_DomesticSaleInvoice_Model.OrderType),
                };
                string GRNId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sls$inv$detail_DeleteSI_Details", prmContentInsert).ToString();
                return GRNId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        
        public string Approve_SI(string CompID, string BrchID, string SI_No, string SI_Date, string InvType,string UserID
            ,string MenuID,string mac_id, string A_Status, string A_Level, string A_Remarks,string SaleVouMsg
            ,string PV_VoucherNarr,string BP_VoucherNarr,string DN_VoucherNarr,string DN_Nurr_Tcs)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                    objProvider.CreateInitializedParameter("@invno",DbType.String,SI_No),
                                                    objProvider.CreateInitializedParameter("@invdate",DbType.String, SI_Date),
                                                    objProvider.CreateInitializedParameter("@invtype",DbType.String, InvType ),
                                                    objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                                                    objProvider.CreateInitializedParameter("@brid",DbType.String,BrchID),
                                                    objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                    objProvider.CreateInitializedParameter("@menuid",DbType.String,MenuID),
                                                    objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                    objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                    objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                    objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                    objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                                                    objProvider.CreateInitializedParameter("@SaleVouMsg",DbType.String, SaleVouMsg),
                                                    objProvider.CreateInitializedParameter("@Pv_Narr",DbType.String, PV_VoucherNarr),
                                                    objProvider.CreateInitializedParameter("@Bp_Narr",DbType.String, BP_VoucherNarr),
                                                    objProvider.CreateInitializedParameter("@Dn_Narr",DbType.String, DN_VoucherNarr),
                                                    objProvider.CreateInitializedParameter("@Dn_Narr_Tcs",DbType.String, DN_Nurr_Tcs),
                };
                prmContentInsert[7].Size = 100;
                prmContentInsert[7].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$inv$detail_Approved_SI_Details", prmContentInsert);

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
        public DataSet CheckSaleReturnAgainstSaleInvoice(string CompId, string BrchID, string DocNo, string DocDate)
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
                DataSet Get_OC_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$inv$detail_CheckSaleReturnAgainstSaleInvoice", prmContentGetDetails);
                return Get_OC_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
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
        public DataSet GetSI_DetailList(string CompId, string BrchID, string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus,string Order_type,string sales_person)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@CustId",DbType.String, CustId),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@Order_type",DbType.String, Order_type),
                                                        objProvider.CreateInitializedParameter("@sls_per",DbType.String, sales_person),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$inv$detail_GetSI_DeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Edit_SIDetail(string CompID, string BrchID,string VouType, string SINo, string SIDate, string Type, string UserID, string DocumentMenuId)
        {
           
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrchID),
                                                        objProvider.CreateInitializedParameter("@VouType",DbType.String, VouType),
                                                        objProvider.CreateInitializedParameter("@SINo",DbType.String, SINo),
                                                        objProvider.CreateInitializedParameter("@SIDate",DbType.String, SIDate),
                                                        objProvider.CreateInitializedParameter("@SIType",DbType.String, Type),
                                                         objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.Int64, DocumentMenuId),
                                                        
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$inv$detail_GetSI_Details", prmContentGetDetails);
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
        public DataSet GetSIAttatchDetailEdit(string CompID, string BrchID, string SI_No, string SI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SI_No",DbType.String, SI_No),
                                                        objProvider.CreateInitializedParameter("@SI_Date",DbType.String, SI_Date),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$SIInv$getAttatchmentDetail", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSalesInvoiceDeatilsForPrint(string CompID, string BrchID, string SI_No, string SI_Date,string inv_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, SI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, SI_Date),
                                                        objProvider.CreateInitializedParameter("@flag",DbType.String, inv_type),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSalesInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_SI_List;
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
        public DataSet GetRoundOffGLDetails(string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     };
                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetRoundOffGLDetail", prmContentGetDetails);
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

        public DataSet SI_GetSubItemDetails(string CompID, string Br_id, string ItemId, string SrcDoc_no, string SrcDoc_dt, string doc_no, string doc_dt, string Flag, string DocumentMenuId)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, Br_id),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, ItemId),
                    objProvider.CreateInitializedParameter("@Ship_no",DbType.String,SrcDoc_no),
                    objProvider.CreateInitializedParameter("@Ship_dt",DbType.String, SrcDoc_dt),
                    objProvider.CreateInitializedParameter("@doc_no",DbType.String,doc_no),
                    objProvider.CreateInitializedParameter("@doc_dt",DbType.String, doc_dt),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SI_GetSubItemDetails", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet SI_GetSubItemDetailsafterapprove(string CompID, string Br_id, string ItemId, string doc_no, string doc_dt,string DocumentMenuId
            ,string Flag)
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
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                    objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                   
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SI_GetSubItemDetailsAfterApprove", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        //public DataSet GetOCGLDetails(string OCID, string CompId)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //            objProvider.CreateInitializedParameter("@OCID",DbType.String, OCID),

        //        };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetOtherChargeGLDetail", prmContentGetDetails);
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

        //public DataSet GetTaxGLDetails(string TaxID, string CompId)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //            objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //            objProvider.CreateInitializedParameter("@TaxID",DbType.String, TaxID),

        //        };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetTaxGLDetail", prmContentGetDetails);
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

        public DataTable GetBankdetail(string CompID, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                };
                DataTable GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Bind$Dropdown$bank$Name]", prmContentGetDetails).Tables[0];
                return GetOCList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }  
        public DataSet GetAllData(string CompID, string BranchID, string Cust_Name, string CustType,string SalesPersonName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                         objProvider.CreateInitializedParameter("@CustName",DbType.String, Cust_Name),
                             objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                             objProvider.CreateInitializedParameter("@SalesPersonName",DbType.String, SalesPersonName),
                };
                DataSet GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetAllData$sales$invoice$]", prmContentGetDetails);
                return GetOCList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } 
        public DataSet getbankdeatils(string bankName, string CompID, string BranchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                     objProvider.CreateInitializedParameter("@bankName",DbType.String, bankName),
                };
                DataSet GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[getdetaildataaccName]", prmContentGetDetails);
                return GetOCList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetSaleInvoiceItemstoExportExcel(string compId, string brId, string invNo, string invDate, string docId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                     objProvider.CreateInitializedParameter("@InvNo",DbType.String, invNo),
                     objProvider.CreateInitializedParameter("@InvDate",DbType.String, invDate),
                     objProvider.CreateInitializedParameter("@DocId",DbType.String, docId),
                };
                DataSet GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSalesInvoiceItemsToExportExcel", prmContentGetDetails);
                return GetOCList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSlsInvGstDtlForPrint(string compId, string brchId, string siNo, string siDate, string invType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brchId),
                                                        objProvider.CreateInitializedParameter("@InvNo",DbType.String, siNo),
                                                        objProvider.CreateInitializedParameter("@InvDate",DbType.String, siDate),
                                                        objProvider.CreateInitializedParameter("@Flag",DbType.String, invType),
                                                      };
                DataSet Get_SI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSlsInvGSTDtl_ForPrint", prmContentGetDetails);
                return Get_SI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
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
        public DataTable  GetCityOnDistrictDDL(string ddlDistrictID)
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
