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
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerDetails;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
    public class CustomerDetails_SERVICES : CustomerDetails_ISERVICES
    {

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
        public DataSet GetviewCustdetail(string Custcode, string CompID,string Br_Id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@cust_id",DbType.String, Custcode),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@Br_Id",DbType.String, Br_Id),
                                                     };
                DataSet dtGetviewCustdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$CustViewdetail]", prmContentGetDetails);
                return dtGetviewCustdetail;

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
        public DataTable GetAccountGroupDAL(string CompID,string type)
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
        public DataTable GetCustcoaDAL(string CompID,string type,string Cust_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                     objProvider.CreateInitializedParameter("@type",DbType.String, type),
                      objProvider.CreateInitializedParameter("@Cust_id",DbType.String, Cust_id),
                                                    };
                DataTable GetCustcoa = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Coa$cust", prmContentGetDetails).Tables[0];
                return GetCustcoa;
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
                DataTable Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$category", prmContentGetDetails).Tables[0];
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable GetCustportDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetCustport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$portfolio", prmContentGetDetails).Tables[0];
                return GetCustport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetRegionDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetRegion = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$region", prmContentGetDetails).Tables[0];
                return GetRegion;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCustPriceGrpDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetCustPriceGrp = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$pricegroup", prmContentGetDetails).Tables[0];
                return GetCustPriceGrp;
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
        public string insertCustomerDetails(DataTable CustomerDetail, DataTable CustomerBranch, DataTable CustomerAttachments, DataTable CustomerAddress,int PaymentAlert,DataTable LicenceDetail)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@CustomerDetail",SqlDbType.Structured, CustomerDetail ),
                 objprovider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured, CustomerBranch ),
                 objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,CustomerAttachments ),
                 objprovider.CreateInitializedParameterTableType("@CustAddressDetail",SqlDbType.Structured,CustomerAddress ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameterTableType("@PaymentAlert",SqlDbType.Int,PaymentAlert),
                 objprovider.CreateInitializedParameterTableType("@LicenceDetail",SqlDbType.Structured,LicenceDetail),
                };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertCustomer_Detail", prmcontentaddupdate).ToString();
                
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

        public DataSet GetsuppDSCntrDAL(string SuppCity)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@SuppCity",DbType.String,SuppCity),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp$DisStatCntr_GetsuppDisStatCntr", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
       public DataSet checkDependencyAddr(string Comp_ID,string custId, string addr_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String,Comp_ID),
                    objProvider.CreateInitializedParameter("@cust_SuppId",DbType.String,custId),
                     objProvider.CreateInitializedParameter("@addr_id",DbType.String,addr_id),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$cust$AddrDependency", prmContentGetDetails);
                return GetsuppDSCntr;
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
        public DataSet InsertCustomerAddrress(string comp_id, string CustId, string Address, string City, string District, string State, string Country, string GSTNo, string BillingAddress, string ShippingAddress)
        {

            SqlDataProvider objprovider = new SqlDataProvider();
            SqlParameter[] prmcontentaddupdate = {
                 objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                 objprovider.CreateInitializedParameter("@CustId",DbType.String,CustId),
                 objprovider.CreateInitializedParameter("@Address",DbType.String,Address),
                 objprovider.CreateInitializedParameter("@City",DbType.String,City),
                 objprovider.CreateInitializedParameter("@District",DbType.String,District),
                 objprovider.CreateInitializedParameter("@State",DbType.String,State),
                 objprovider.CreateInitializedParameter("@Country",DbType.String,Country),
                 objprovider.CreateInitializedParameter("@GSTNo",DbType.String,GSTNo),
                 objprovider.CreateInitializedParameter("@BillingAddress",DbType.String,BillingAddress),
                 objprovider.CreateInitializedParameter("@ShippingAddress",DbType.String,ShippingAddress)
                 };
            DataSet CustomerAddressDetail= SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$insertCustomerAddressDeatils", prmcontentaddupdate);
            return CustomerAddressDetail;
        }

        public string CustomerDetailDelete(CustomerDetails _CustomerDetails, string comp_id, string cust_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@CustID",DbType.String, cust_id),
                                                     };
                //string SuppDetails = string.Empty;
                string SuppDetails = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "sp_stp$Cust$detail_DeleteCust", prmContentGetDetails));
                return SuppDetails;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string CustomerApprove(CustomerDetails _CustomerDetails, string comp_id, string app_id, string cust_id,string mac_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@CustID",DbType.String, cust_id),
                    objProvider.CreateInitializedParameter("@app_id",DbType.Int16, app_id ),
                    objProvider.CreateInitializedParameter("@mac_id",DbType.Int16, mac_id ),
                    objProvider.CreateInitializedParameterTableType("@DocNo1",SqlDbType.NVarChar,""),
                     };
                prmcontentaddupdate[4].Size = 100;
                prmcontentaddupdate[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "sp_stp$ApproveCust$detail", prmcontentaddupdate).ToString();
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
        public DataSet GetFromDateCust(string CompId, string BrID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_GetFromDate", prmContentGetDetails);
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
        /*----------------------Code Start of Country,state,district,city--------------------------*/
        public DataTable GetCountryListDDL(string CompID, string CustType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objProvider.CreateInitializedParameter("@TransModetype",DbType.String,CustType),
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
        public DataSet GetAllDropDownList(string Comp_id,string Br_ID)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Comp_id",DbType.String,Comp_id),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_ID),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Cust$All$data", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /*----------------------Code End of Country,state,district,city--------------------------*/

        public DataSet GetCustomerSalesDetail(string Comp_ID, string Br_Id, string cust_ID, string FromDate, string ToDate)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Comp_id",DbType.String,Comp_ID),
                     objProvider.CreateInitializedParameter("@Br_ID",DbType.String,Br_Id),
                     objProvider.CreateInitializedParameter("@cust_ID",DbType.String,cust_ID),
                     objProvider.CreateInitializedParameter("@FromDate",DbType.String,FromDate),
                     objProvider.CreateInitializedParameter("@ToDate",DbType.String,ToDate),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Customer$Sales$Detail", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable CheckDependcyGstno(string Comp_ID, string Cust_Id, string custGst)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
                     objProvider.CreateInitializedParameter("@cust_id",DbType.String,Cust_Id),
                     objProvider.CreateInitializedParameter("@custGst",DbType.String,custGst),
                };
                DataTable GetCurronSuppType = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Check$dependcy$GStNo$customer", prmContentGetDetails).Tables[0];
                return GetCurronSuppType;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
