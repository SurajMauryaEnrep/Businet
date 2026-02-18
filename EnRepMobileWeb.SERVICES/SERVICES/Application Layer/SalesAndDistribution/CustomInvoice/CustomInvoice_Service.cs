using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.CustomInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.CustomInvoice;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.CustomInvoice
{
    public class CustomInvoice_Service: CustomInvoice_ISERVICE
    {
        public Dictionary<string, string> GetCustomerList(string CompID, string Cust_Name, string BrchID, string CustType)
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
                             objProvider.CreateInitializedParameter("@DocId",DbType.String, "105103145127"),
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
        public DataSet GetAllData(string CompID, string Cust_Name, string BrchID, string CustType,
            string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus)
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
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GEtAllData$Custom$Invoice$List", prmContentGetDetails);
             
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
        public DataTable Getcurr_details(string CompId, string BrID, string ship_no, string ship_dt)
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
                DataTable curr_details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$inv$detail_Getcurrdetails", prmContentGetDetails).Tables[0];
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
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetShipmentList", prmContentGetDetails);
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
        public DataSet GetShipmentDetail(string ShipmentNo, string ShipmentDate, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@ShipmentNo",DbType.String, ShipmentNo),
                    objProvider.CreateInitializedParameter("@ShipmentDate",DbType.String, ShipmentDate),
                    objProvider.CreateInitializedParameter("@inv_type",DbType.String, "E"),

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
        public string InsertCI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblVouGLDetail
            , DataTable DTTaxDetail, DataTable DtblOCDetail, DataTable DtblAttchDetail,string Narration)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@VoucherDetail",SqlDbType.Structured,DtblVouGLDetail),                                                       
                                                        objprovider.CreateInitializedParameterTableType("@TaxDetail",SqlDbType.Structured,DTTaxDetail),
                                                        objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        objprovider.CreateInitializedParameterTableType("@DtblOCDetail",SqlDbType.Structured,DtblOCDetail), 
                                                        objprovider.CreateInitializedParameterTableType("@Narration",SqlDbType.NVarChar,Narration), 

                                                    };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sls$cstm$inv$detail_InsertSI_Details", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[5].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[5].Value.ToString();
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
        public string Delete_CI_Detail(CustomInvoice_Model _CustomInvoice_Model, string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                        objProvider.CreateInitializedParameter("@Inv_No",DbType.String,_CustomInvoice_Model.inv_no),
                                                        objProvider.CreateInitializedParameter("@Inv_Date",DbType.String,_CustomInvoice_Model.inv_dt),
                };
                string GRNId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sls$cstm$inv$detail_DeleteCI_Details", prmContentInsert).ToString();
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
        
        public string Approve_CI(string CompID, string BrchID, string CI_No, string CI_Date,string UserID,string MenuID,string mac_id, string A_Status, string A_Level, string A_Remarks,string SaleVouMsg)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                    objProvider.CreateInitializedParameter("@invno",DbType.String,CI_No),
                                                    objProvider.CreateInitializedParameter("@invdate",DbType.String, CI_Date),                                                   
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
                };
                prmContentInsert[6].Size = 100;
                prmContentInsert[6].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$cstm$inv$detail_Approved_CI_Details", prmContentInsert);

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
        public DataSet Get_CI_DetailList(string CompId, string BrchID, string CustId, string Fromdate, string Todate, string Status, string UserID, string DocumentMenuId, string wfstatus)
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
                                                               
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$$cstm$inv$detail_GetCI_DeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet Edit_CIDetail(string CompID, string BrchID, string CINo, string CIDate, string UserID, string DocumentMenuId)
        {
           
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int32, BrchID),
                                                        objProvider.CreateInitializedParameter("@CINo",DbType.String, CINo),
                                                        objProvider.CreateInitializedParameter("@CIDate",DbType.String, CIDate),                                                       
                                                         objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.Int64, DocumentMenuId),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$cstm$inv$detail_GetCI_Details", prmContentGetDetails);
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
        public DataSet GetCIAttatchDetailEdit(string CompID, string BrchID, string CI_No, string CI_Date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@CI_No",DbType.String, CI_No),
                                                        objProvider.CreateInitializedParameter("@CI_Date",DbType.String, CI_Date),
                                                      };
                DataSet Get_CI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$CIInv$getAttatchmentDetail", prmContentGetDetails);
                return Get_CI_List;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomInvoiceDeatilsForPrint(string CompID, string BrchID, string CI_No, string CI_Date,string flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@inv_no",DbType.String, CI_No),
                                                        objProvider.CreateInitializedParameter("@inv_date",DbType.String, CI_Date),
                                                        objProvider.CreateInitializedParameter("@flag",DbType.String, flag),
                                                      };
                DataSet Get_CI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetCustomInvoiceDeatils_ForPrint", prmContentGetDetails);
                /*For Alaska exports custom print*/
                //DataSet Get_CI_List = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "AE_GetCustomInvoiceDeatils_ForPrint", prmContentGetDetails);
                return Get_CI_List;
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
        public DataSet GetTaxRecivableAcc(string comp_id, string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     };
                DataSet GetGlDt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetTaxRecivableAcc", prmContentGetDetails);
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

        public DataTable GetCustomItemsToExportExcel(string compId, string brId, string invNo, string invDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompId",DbType.String,compId),
                     objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                     objProvider.CreateInitializedParameter("@InvNo",DbType.String, invNo),
                     objProvider.CreateInitializedParameter("@InvDate",DbType.String, invDate),
                };
                DataSet GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetCstmSalesInvoiceItemsToExportExcel", prmContentGetDetails);
                return GetOCList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable PortOfLoadingList()
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            //SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 

            //};
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPortOfLoadingList").Tables[0];
            return dt;
        }
        public DataTable PlOfReceiptByPreCarrierList()
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            //SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/ 

            //};
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPlOfReceiptByPreCarrierList").Tables[0];
            return dt;
        }
       

    }
}
