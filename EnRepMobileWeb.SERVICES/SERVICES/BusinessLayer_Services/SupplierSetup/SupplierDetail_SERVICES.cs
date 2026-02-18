using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.MODELS.BusinessLayer.SupplierDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
    public class SupplierDetail_SERVICES : SupplierDetail_ISERVICES
    {
        public DataTable GetAccountGroupDAL(string CompID, string type)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@type",DbType.String, type),
                                                    };
                DataTable GetAccountGroup = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetHierAccGroupList", prmContentGetDetails).Tables[0];
                return GetAccountGroup;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllDropDown(string CompID, string type,string Br_Id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@type",DbType.String, type),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, Br_Id),                                };
                DataSet GetAccountGroup = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$supplier$Deatil$All$Dropdown", prmContentGetDetails);
                return GetAccountGroup;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetGlReportingGrp(string Comp_ID, string Br_id, string gl_repoting)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                    objProvider.CreateInitializedParameter("@Br_id",DbType.String,Br_id),
                     objProvider.CreateInitializedParameter("@gl_repoting",DbType.String,gl_repoting),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Data$Drp$Dwn$GlReporting", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetBrList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$BrList$detail", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetSuppcoaDAL(string CompID,string type,string SuppId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                         objProvider.CreateInitializedParameter("@type",DbType.String, type),
                             objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                    };
                DataTable GetSuppcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Coa", prmContentGetDetails).Tables[0];
                return GetSuppcoa;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetBrListDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataTable GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$BrList$detail", prmContentGetDetails).Tables[0];
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetcategoryDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$category_Getcategory", prmContentGetDetails).Tables[0];
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetsuppportDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp$portfolio_Getportfolio", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet checkDependencyAddr(string Comp_ID, string custId, string addr_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                    objProvider.CreateInitializedParameter("@cust_SuppId",DbType.String,custId),
                     objProvider.CreateInitializedParameter("@addr_id",DbType.String,addr_id),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$SuppLier$AddrDependency", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //public DataTable GetsuppcurrDAL(string Comp_ID)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //             objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
        //        };
        //        DataTable Getsuppcurr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$curr_Getcurr", prmContentGetDetails).Tables[0];
        //        return Getsuppcurr;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataTable GetCurronSuppTypeDAL(string CompID, string Supptype)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@Supptype",DbType.String,Supptype),
                };
                DataTable GetCurronSuppType = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp_CurronType", prmContentGetDetails).Tables[0];
                return GetCurronSuppType;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable CheckDependcyGstno(string Comp_ID, string Supp_Id,string SupplierGst)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
                     objProvider.CreateInitializedParameter("@Supp_Id",DbType.String,Supp_Id),
                     objProvider.CreateInitializedParameter("@SupplierGst",DbType.String,SupplierGst),
                };
                DataTable GetCurronSuppType = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Check$dependcy$GStNo", prmContentGetDetails).Tables[0];
                return GetCurronSuppType;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> SuppCityDAL(string GroupName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CityName",DbType.String, GroupName),
                                  };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$SuppCity$detail_GetSuppCityList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["city_id"].ToString(), PARQusData.Tables[0].Rows[i]["city_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public String insertSupplierDetails(DataTable SupplierDetail, DataTable SupplierBranch, DataTable SupplierAttachments,DataTable SupplierAddress,int PaymentAlert,DataTable LicenceDetail)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@SupplierDetail",SqlDbType.Structured, SupplierDetail ),
                 objprovider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured, SupplierBranch ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,SupplierAttachments ),
                   objprovider.CreateInitializedParameterTableType("@SuppAddressDetail",SqlDbType.Structured,SupplierAddress ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@PaymentAlert",SqlDbType.Int, PaymentAlert),
                 objprovider.CreateInitializedParameterTableType("@LicenceDetail",SqlDbType.Structured, LicenceDetail),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertSupplier_Details", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetviewSuppdetailDAL(string Suppcode, string CompID,string Br_Id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@supp_id",DbType.String, Suppcode),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@Br_Id",DbType.String, Br_Id),
                                                     };
                DataSet GetviewSuppdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[stp$Supp$detail_GetSuppdetail]", prmContentGetDetails);
                return GetviewSuppdetail;

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
        public DataSet SupplierDetailDelete(SupplierDetail _SupplierDetail, string comp_id, string supp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int16, comp_id),                    
                    objProvider.CreateInitializedParameter("@SuppID",DbType.String, supp_id),
                                                     };
                DataSet SuppDetails = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp$detail_DeleteSuppDetails", prmContentGetDetails);
                return SuppDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SupplierApprove(SupplierDetail _SupplierDetail, string comp_id, string app_id, string supp_id, string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, comp_id),
                    objProvider.CreateInitializedParameter("@SuppID",DbType.String, supp_id),
                    objProvider.CreateInitializedParameter("@app_id",DbType.String, app_id ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id ),
                     objProvider.CreateInitializedParameterTableType("@DocNo1",SqlDbType.NVarChar,""),
                     };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_stp$ApproveSupp$detail", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[4].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        /*----------------------Code Start of Country,state,district,city--------------------------*/
        public DataTable GetCountryListDDL(string CompID, string SuppType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objProvider.CreateInitializedParameter("@TransModetype",DbType.String,SuppType),
                };
                DataTable GetCurronSuppType = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Country_onTransModeType", prmContentGetDetails).Tables[0];
                return GetCurronSuppType;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetstateOnCountryDDL(string ddlCountryID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CountryID",DbType.String,ddlCountryID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getState", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
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
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@StateID",DbType.String,ddlStateID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getDistrict", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
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
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@DistrictID",DbType.String,ddlDistrictID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getCity", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetStateCode(string stateId)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@StateId",DbType.String,stateId),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetStateCode", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        /*----------------------Code End of Country,state,district,city--------------------------*/


        public DataSet GetSupplierPurchaseDetail(string Comp_ID, string Br_Id, string Supp_Id, string FromDate, string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                     objProvider.CreateInitializedParameter("@Br_Id",DbType.String,Br_Id),
                     objProvider.CreateInitializedParameter("@Supp_Id",DbType.String,Supp_Id),
                     objProvider.CreateInitializedParameter("@FromDate",DbType.String,FromDate),
                     objProvider.CreateInitializedParameter("@ToDate",DbType.String,ToDate),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Supplier$Purchase$Detail", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
