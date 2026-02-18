using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ProductCatalouge;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ProductCatalouge;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.ProductCatalouge
{
    public class ProductCatalouge_SERVICES : ProductCatalouge_ISERVICES
    {
        public string InsertUpdateProdCatalogDetail(int CompID, int BrchID, string catal_No, string catal_Date, string cust_Id, string remark, string create_id, string Transtype, string DocumentMenuId, string mac_id, DataTable CatalogItemDetail,string Cust_type)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                       objprovider.CreateInitializedParameterTableType("@comp_id",SqlDbType.NVarChar, CompID),
                                                       objprovider.CreateInitializedParameterTableType("@br_id",SqlDbType.NVarChar,BrchID),
                                                       objprovider.CreateInitializedParameterTableType("@cat_no",SqlDbType.NVarChar, catal_No),
                                                       objprovider.CreateInitializedParameterTableType("@cat_dt",SqlDbType.NVarChar,catal_Date),
                                                       objprovider.CreateInitializedParameterTableType("@cust_id",SqlDbType.NVarChar,cust_Id),
                                                       objprovider.CreateInitializedParameterTableType("@cat_rem",SqlDbType.NVarChar,remark ),
                                                       objprovider.CreateInitializedParameterTableType("@user_id",SqlDbType.NVarChar,create_id),
                                                       objprovider.CreateInitializedParameterTableType("@TransType",SqlDbType.NVarChar,Transtype),
                                                       objprovider.CreateInitializedParameterTableType("@DocumentMenuId",SqlDbType.NVarChar,DocumentMenuId),
                                                       objprovider.CreateInitializedParameterTableType("@mac_id",SqlDbType.NVarChar,mac_id),
                                                       objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, CatalogItemDetail),
                                                       objprovider.CreateInitializedParameterTableType("@Cust_type",SqlDbType.NVarChar, Cust_type),
                                                       objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                    };
                prmcontentaddupdate[12].Size = 100;
                prmcontentaddupdate[12].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sls$prod$catalog$detail_InsertUpdateProdCatalogDetail", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[12].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[12].Value.ToString();
                }

                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        
        public DataSet BindItemName(string CompID, string BrID, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_BindItemProductCatalouge", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, string> GetCustomerListProdCata(string CompID, string BranchID,string CustomerName,string CustPros_type)
        {
            Dictionary<string, string> ddlCustListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing parameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                   objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                   objProvider.CreateInitializedParameter("@CustomerName",DbType.String, CustomerName),
                   objProvider.CreateInitializedParameter("@CustPros_type",DbType.String, CustPros_type),
            };
                DataSet ProdCataData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustListPrdCata", prmContentGetDetails);
                DataRow dr;
                dr = ProdCataData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                ProdCataData.Tables[0].Rows.InsertAt(dr, 0);

                if (ProdCataData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ProdCataData.Tables[0].Rows.Count; i++)
                    {
                        ddlCustListDic.Add(ProdCataData.Tables[0].Rows[i]["cust_id"].ToString(), ProdCataData.Tables[0].Rows[i]["cust_name"].ToString());
                    }
                }
                return ddlCustListDic;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        
        public Dictionary<string, string> GetGroupList(string CompID, string GroupName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ProdCata$item$grp_GetAllItemGroup", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["item_grp_id"].ToString(), PARQusData.Tables[0].Rows[i]["ItemGroupChildNood"].ToString());
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
            return null;
        }

        public DataSet GetPortFolioList(string CompID, string PortfName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing parameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@PortfName",DbType.String, PortfName),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ProdCata$portf$detail_GetPortFolioList", prmContentGetDetails);
                return PARQusData;
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

        public DataSet GetVehicleList(string CompID, string VehicleName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing parameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@VehicleName",DbType.String, VehicleName),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ProdCata$veh$detail_GetVehicleList", prmContentGetDetails);
                return PARQusData;
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

        public DataSet GetVehOEMNoDetail(string CompID, string VehOEM_No)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing parameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Veh_OEM_No",DbType.String, VehOEM_No),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ProdCata$Veh_OEM_No$detail", prmContentGetDetails);
                return PARQusData;
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
        public DataSet GetRefNoDetail(string CompID, string RefNo)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing parameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@ReferenceNo",DbType.String, RefNo),
                                                     };
                DataSet refnum = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ProdCata$RefNo$detail", prmContentGetDetails);
                return refnum;
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

        public DataSet GetTechSpecDetail(string CompID, string Techspec)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing parameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@TechSpecific",DbType.String, Techspec),
                                                     };
                DataSet refnum = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ProdCata$TechSpecific$detail", prmContentGetDetails);
                return refnum;
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

        public DataSet GetFilterItem(string CompID, string fltrvalue, string fltrtype)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to store procedure*/   
                    objProvider.CreateInitializedParameter("@FilterId",DbType.String, fltrvalue),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@FilterType",DbType.String, fltrtype),
                                                     };
                DataSet GetFilterData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ProdCata$FilterItemData", prmContentGetDetails);
                return GetFilterData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetListOfProdCatalogDetails(string comp_id, string br_id, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, comp_id),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, br_id),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ProdCatalogList$detail_GetProdCatalogListDetail", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetSearchListOfProdCatalogDetails(string CompID, string BrchID, string CustID, string Fromdate, string Todate, string Status, string UserID, string wfstatus, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                                                        objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ProdCatalogList$detail_GetProdCatalogListDetail", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCustNameList(string CompId, string br_id, string CustomerName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetCustList]", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllData(string CompId, string br_id, string CustomerName, string UserID, string wfstatus, string DocumentMenuId,
            string CustID, string Fromdate, string Todate, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.Int64, br_id),
                    objProvider.CreateInitializedParameter("@CustName",DbType.String, CustomerName),
                    objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                   objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                   objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                      objProvider.CreateInitializedParameter("@CustID",DbType.String, CustID),
                   objProvider.CreateInitializedParameter("@Fromdate",DbType.String, Fromdate),
                   objProvider.CreateInitializedParameter("@Todate",DbType.String, Todate),
                   objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[GetAllData$product$cataloge]", prmContentGetDetails);
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
        public DataSet GetProdCatalogueDetails( string CompID, string BrchID, string Doc_no, string Doc_date, string UserID, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, Doc_no),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, Doc_date),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        //objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sls$ProdCatalog$detail_GetProdCatalogDetail", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustomerListtoEdit(string CompID, string BranchID,string CustomerName,string CustPros_type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                     objProvider.CreateInitializedParameter("@CustomerName",DbType.String, CustomerName),
                      objProvider.CreateInitializedParameter("@CustPros_type",DbType.String, CustPros_type),
                                                    };
                DataTable GetCustListEdit = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$cust$detail_GetCustListPrdCata", prmContentGetDetails).Tables[0];
                return GetCustListEdit;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string DeleteProdCatlogDetails(string CompID, string BrchID, string doc_no, string doc_date)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrchID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, doc_no),
                                                        objProvider.CreateInitializedParameter("@DocDate",DbType.String, doc_date),
                                                        objProvider.CreateInitializedParameter("@Result",DbType.String,""),
                                                    };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sls$ProdCataDel$detail_DeleteDetails", prmcontentaddupdate).ToString();
                string Result = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    Result = prmcontentaddupdate[4].Value.ToString();
                }

                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string InsertProdCatalogApproveDetails(string CTLNo, string CTLDate, string CompID, string BrchID, string DocumentMenuId, string UserID, string mac_id, string A_Status, string A_Level, string A_Remarks, string Flag)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentInsert = {
                                                        objProvider.CreateInitializedParameter("@docno",DbType.String, CTLNo),
                                                        objProvider.CreateInitializedParameter("@docdate",DbType.String, CTLDate),
                                                        objProvider.CreateInitializedParameter("@compid",DbType.String, CompID ),
                                                        objProvider.CreateInitializedParameter("@brid",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@menuid",DbType.String,DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String,UserID),
                                                        objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                                                        objProvider.CreateInitializedParameter("@wf_status",DbType.String, A_Status),
                                                        objProvider.CreateInitializedParameter("@wf_level",DbType.String, A_Level),
                                                        objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, A_Remarks),
                };
                string PCId = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sls$ProdCatApprov$Details_ApproveDetails", prmContentInsert).ToString();
                return PCId;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            
            finally
            {
            }
        }
        public DataSet GetCatalogueDeatils(string CompID, string BrchID, string CTLNo, string CTLDate)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@CTLNo",DbType.String, CTLNo),
                     objProvider.CreateInitializedParameter("@CTLDate",DbType.String, CTLDate),
                     
                                                     };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP_SLS_GetCatalogueDetail]", prmContentGetDetails);
            return ds;
        }

    }

}

