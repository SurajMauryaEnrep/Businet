using EnRepMobileWeb.SERVICES.ISERVICES.Dashboard_ISERVICE;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace EnRepMobileWeb.SERVICES.SERVICES.Dashboard_SERVICE
{
    public class Dashboard_SERVICE: Dashboard_ISERVICE
    {
        public DataSet GetDashboardData(string CompId, string BrID, string Dateflag, string Fromdt, string Todt, string Top, string Charttype, string UserID, string Language, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@compid",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@Dateflag",DbType.String, Dateflag),
                                                        objProvider.CreateInitializedParameter("@fromdt",DbType.String, Fromdt),
                                                        objProvider.CreateInitializedParameter("@todt",DbType.String, Todt),
                                                        objProvider.CreateInitializedParameter("@Top",DbType.Int64, Top),
                                                        objProvider.CreateInitializedParameter("@Charttype",DbType.String, Charttype),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@lang",DbType.String, Language),
                                                        objProvider.CreateInitializedParameter("@Flag",DbType.String, Flag),
                                                      };
                DataSet dashboard_data = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_GetAllDashboard_data", prmContentGetDetails);
                return dashboard_data;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet Updateavlvalue(string UserID, string value)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {

                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@value",DbType.String, value),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, ""),
                                                      };
                DataSet dashboard_data = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[update$user$toggle]", prmContentGetDetails);
                return dashboard_data;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        //public DataSet Getfromdtae(string CompId, string BrID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
        //                                              };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_GetFromDate", prmContentGetDetails);
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
        //public DataSet GetPendingDocumentList(string CompId, string BrID, string UserID,string Language)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
        //                                                objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
        //                                                objProvider.CreateInitializedParameter("@lang",DbType.String, Language),
        //                                              };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$app$wf$detail_GetPendingDocList", prmContentGetDetails);
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
        //public DataSet GetChartsDataList(string CompId, string BrID, string DateFlag, int top,string ChartType, string Fromdt, string Todt)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
        //                                                objProvider.CreateInitializedParameter("@Dateflag",DbType.String, DateFlag),
        //                                                 objProvider.CreateInitializedParameter("@Top",DbType.Int64, top),
        //                                                 objProvider.CreateInitializedParameter("@Charttype",DbType.String, ChartType),
        //                                                 objProvider.CreateInitializedParameter("@fromdt",DbType.String, Fromdt),
        //                                                objProvider.CreateInitializedParameter("@todt",DbType.String, Todt),
        //                                              };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_GetChartsdataList", prmContentGetDetails);
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
        //public DataSet GetTickersDataList(string CompId, string BrID, string DateFlag,string Fromdt,string Todt)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
        //                                                objProvider.CreateInitializedParameter("@Dateflag",DbType.String, DateFlag),
        //                                                objProvider.CreateInitializedParameter("@fromdt",DbType.String, Fromdt),
        //                                                objProvider.CreateInitializedParameter("@todt",DbType.String, Todt),

        //                                              };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_GetTickerdata", prmContentGetDetails);
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

        //public DataSet GetAlertsDataList(string CompId, string BrID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {
        //                                                objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
        //                                                objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
        //                                              };
        //        DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_GetAlertsdata", prmContentGetDetails);
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
        public DataSet GetPendingDocument(string CompId, string BrID, string UserID, string Language)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@compid",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.Int64, BrID),                                                       
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@lang",DbType.String, Language),
                                                      
                                                      };
                DataSet dashboard_data = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$Pending$Document$Dashboard]", prmContentGetDetails);
                return dashboard_data;
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
