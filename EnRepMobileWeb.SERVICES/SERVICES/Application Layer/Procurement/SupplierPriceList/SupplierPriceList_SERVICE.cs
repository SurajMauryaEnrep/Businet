using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.SupplierPriceList;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.SupplierPriceList
{
     public class SupplierPriceList_SERVICE: SupplierPriceList_ISERVICE
    {
        public DataSet GetAllData(string CompID, string BranchID,string UserID, string catalog, string portfolio, string ActStatus, string ValidUpto, string Docid, string wfStatus)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                       objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                       objProvider.CreateInitializedParameter("@UserID",DbType.Int32, UserID),
                    objProvider.CreateInitializedParameter("@catalog",DbType.String, catalog),
                      objProvider.CreateInitializedParameter("@portfolio",DbType.String, portfolio),
                     objProvider.CreateInitializedParameter("@ActStatus",DbType.String, ActStatus),
                   objProvider.CreateInitializedParameter("@ValidUpto",DbType.Date, ValidUpto),
                   objProvider.CreateInitializedParameter("@Docid",DbType.String, Docid),
                   objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfStatus),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$SPL$List", prmContentGetDetails);
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
        public DataSet GetDPIDetailDAL(string CompId, string BrID, string supp_id,string user_id, string DocID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
   
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@supp_id",DbType.String, supp_id),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int32, user_id),
                                                        objProvider.CreateInitializedParameter("@DocID",DbType.String, DocID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetSPL_Details", prmContentGetDetails);
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
        public DataSet GetSupplierAndItemList(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, "0"),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@Doc_id",DbType.String, "105101101"),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetSuppList", prmContentGetDetails);
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
        public string InsertDPI_Details(DataTable DTHeaderDetail, DataTable DTItemDetail, DataTable DtblRevisionDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                    objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                    objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                    objprovider.CreateInitializedParameterTableType("@RevItemDetail",SqlDbType.Structured, DtblRevisionDetail),
                    objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                    };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertSPL_Details", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value)
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
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
        public string DeleteSPLDetails(string CompID, string BrchID, string supp_id)
        {
            SqlDataProvider sqlDataProvider = new SqlDataProvider();
            SqlParameter[] sqlParameters =
            {
                 sqlDataProvider.CreateInitializedParameter("@CompID",DbType.String, CompID ),
                 sqlDataProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID ),
                 sqlDataProvider.CreateInitializedParameter("@supp_id",DbType.String, supp_id ),
                sqlDataProvider.CreateInitializedParameter("@DocNo",DbType.String,""),

            };
            sqlParameters[3].Direction = ParameterDirection.Output;
            sqlParameters[3].Size = 100;
            string dataSet = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteSPL_Details", sqlParameters).ToString();
            string DocNo = string.Empty;
            if (sqlParameters[3].Value != DBNull.Value) // status
            {
                DocNo = sqlParameters[3].Value.ToString();
            }
            return DocNo;
        }
        public string ApproveSPLDetail(string supp_id, string MenuDocId, string Branch, string CompID
           , string UserID, string mac_id, string wf_status, string wf_level, string wf_remarks)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@supp_id",DbType.Int32, supp_id),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String, MenuDocId),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String,Branch),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                         objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, wf_status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, wf_level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, wf_remarks),
                };
                prmContentInsert[5].Size = 100;
                prmContentInsert[5].Direction = ParameterDirection.Output;
                DataSet GrnDetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ApprovedSPLDetails", prmContentInsert);

                string DocNo = string.Empty;
                if (prmContentInsert[5].Value != DBNull.Value)
                {
                    DocNo = prmContentInsert[5].Value.ToString();
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
        public DataSet getReplicateWith(string CompID, string br_id, string SarchValue) 
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@SarchValue", DbType.String, SarchValue)
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$spl$detail$replicate$item", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetReplicateWithItemdata(string CompID, string br_id, string supp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, br_id),
                     objProvider.CreateInitializedParameter("@supp_id", DbType.Int32, supp_id)
                };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$spl$detail_ReplicateItemdetail", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCustPriceHistryDtl(string Comp_ID, string Br_ID, string supp_id, string Item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@supp_id",DbType.Int32, supp_id),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, Item_id),
                                                      };

                DataTable GetCustPriceHisDtl = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetSupplierPriceHistoryDetail", prmContentGetDetails).Tables[0];
                return GetCustPriceHisDtl;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
