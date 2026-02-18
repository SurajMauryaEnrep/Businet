using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSalesQuotation;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.DomesticSalesQuotation
{
    public class DomesticSalesQuotationList_SERVICES : DomesticSalesQuotationList_ISERVICES
    {
        public Dictionary<string, string> GetCustomerList(string CompID, string SuppName, string BranchID,string CustType)
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
        public DataSet GetAllData(string CompID, string SuppName, string BranchID,string CustType, DomesticSalesQuotationListModel _DomesticSalesQuotationListModel, string UserID, string wfstatus, string DocumentMenuId, string QtType,string SalesPersonName,string sls_per)
        {
          
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@CustType",DbType.String, CustType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                     objProvider.CreateInitializedParameter("@CustId",DbType.String, _DomesticSalesQuotationListModel.QT_CustID),
                    objProvider.CreateInitializedParameter("@QTType",DbType.String, QtType),
                    objProvider.CreateInitializedParameter("@Fromdate",DbType.String, _DomesticSalesQuotationListModel.QT_FromDate),
                    objProvider.CreateInitializedParameter("@Todate",DbType.String, _DomesticSalesQuotationListModel.QT_ToDate),
                    objProvider.CreateInitializedParameter("@Status",DbType.String, _DomesticSalesQuotationListModel.QT_Status),
                         objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                         objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                         objProvider.CreateInitializedParameter("@SalesPersonName",DbType.String, SalesPersonName),
                         objProvider.CreateInitializedParameter("@sls_per",DbType.String, sls_per),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$sls$Quo$List", prmContentGetDetails);

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
        public DataSet GetQTDetailListDAL(string CompId, string BrchID, DomesticSalesQuotationListModel _DomesticSalesQuotationListModel, string UserID, string wfstatus, string DocumentMenuId, string QtType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@CustId",DbType.String, _DomesticSalesQuotationListModel.QT_CustID),
                                                        objProvider.CreateInitializedParameter("@QTType",DbType.String, QtType),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, _DomesticSalesQuotationListModel.QT_FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, _DomesticSalesQuotationListModel.QT_ToDate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, _DomesticSalesQuotationListModel.QT_Status),
                                                        
                                                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String,wfstatus),
                                                             objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                             objProvider.CreateInitializedParameter("@sls_per",DbType.String, _DomesticSalesQuotationListModel.SQ_SalePerson),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetQuotationDeatilList", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetQT_TrackingDetail(string CompId, string BrID, string QTNo, string QTDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@QTNo",DbType.String, QTNo),
                                                        objProvider.CreateInitializedParameter("@QTDt",DbType.String, QTDate),
                                                      };
                DataSet SOTrackingData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetQT_TrackingView", prmContentGetDetails);
                return SOTrackingData;
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
