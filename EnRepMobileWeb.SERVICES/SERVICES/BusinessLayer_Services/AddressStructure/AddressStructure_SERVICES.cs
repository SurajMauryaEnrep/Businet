using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AddressStructure;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Modifed by Shubham Maurya on 16-12-2022 add state_code//
namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.AddressStructure
{ 
    public  class AddressStructure_SERVICES: AddressStructure_ISERVICES
    {
        public DataSet GetStateCodeOnchange(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "getcountry", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getcountrylist(string countryName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@flag",DbType.String,"DGetCountryList"),
                     objProvider.CreateInitializedParameter("@groupname",DbType.String,countryName),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$add$getCountry", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> countrylist(string GroupName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@flag",DbType.String,"GetCountryList"),
                    objProvider.CreateInitializedParameter("@groupname",DbType.String, GroupName),
                                  };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$add$getCountry", prmContentGetDetails);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = PARQusData.Tables[0].NewRow();
                    dr["country_id"] = "0";
                    dr["country_name"] = "---select---";
                    PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["country_id"].ToString(), PARQusData.Tables[0].Rows[i]["country_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> statelist(string GroupName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@flag",DbType.String,"GetstateList"),
                    objProvider.CreateInitializedParameter("@groupname",DbType.String, GroupName),
                                  };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$add$getCountry", prmContentGetDetails);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = PARQusData.Tables[0].NewRow();
                    dr["state_id"] = "0";
                    dr["state_name"] = "---select---";
                    PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["state_id"].ToString(), PARQusData.Tables[0].Rows[i]["state_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> districtlist(string GroupName)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmcontentdetails =
                {
                    objProvider.CreateInitializedParameter("@flag",DbType.String,"GetdistrictList"),
                    objProvider.CreateInitializedParameter("@groupname",DbType.String,GroupName),
                };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$add$getCountry", prmcontentdetails);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["district_id"] = "0";
                    dr["district_name"] = "---select---";
                    ds.Tables[0].Rows.InsertAt(dr, 0);

                    for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(ds.Tables[0].Rows[i]["district_id"].ToString(), ds.Tables[0].Rows[i]["district_name"].ToString());
                    }
                }
            }
            catch
            {

            }
            return ddlItemNameDictionary;
        }
        public DataSet getAddressStructureLists()
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@flag",DbType.String,"getaddressstructure"),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$add$getStateDistCity", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet getAddressStructuredetails(string state_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@flag",DbType.String,"getaddressstructuredetails"),
                     objProvider.CreateInitializedParameter("@id",DbType.String,state_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "get$add$getStateDistCitydetails", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string insertstate(string state,string state_code, string country, string state_id, string TransType)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                   // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@state_name",DbType.String,state),
                    objprovider.CreateInitializedParameter("@country_id",DbType.String,country),
                    objprovider.CreateInitializedParameter("@id",DbType.String,state_id),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                     objprovider.CreateInitializedParameter("@state_code",DbType.String,state_code)

                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "insertAddressStructure", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[4].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string insertdistrict(string district_id, string district_name, string state_id, string country, string TransType)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                   // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@state_id",DbType.String,state_id),
                    objprovider.CreateInitializedParameter("@country_id",DbType.String,country),
                    objprovider.CreateInitializedParameter("@id",DbType.String,district_id),
                    objprovider.CreateInitializedParameter("@district_name",DbType.String,district_name),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[5].Size = 100;
                prmContent[5].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "insertAddressStructure", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[5].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string insertcityandpin(string city_id, string city, string district_id, string state_id, string country, string TransType)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontent =
                {
                   objprovider.CreateInitializedParameter("@id",DbType.String,city_id),
                   objprovider.CreateInitializedParameter("@state_id",DbType.String,state_id),
                    objprovider.CreateInitializedParameter("@country_id",DbType.String,country),
                    objprovider.CreateInitializedParameter("@city_name",DbType.String,city),
                   // objprovider.CreateInitializedParameter("@pincode",DbType.String,pin),
                    objprovider.CreateInitializedParameter("@district_id",DbType.String,district_id),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")
                };
                prmcontent[6].Size = 100;
                prmcontent[6].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "insertAddressStructure", prmcontent).ToString();
                string DocNo = string.Empty;
                if (prmcontent[6].Value != DBNull.Value)
                {
                    DocNo = prmcontent[6].Value.ToString();
                }
                return DocNo;
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }
        public string deleteaddressstructure(string _ID, string flag)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                   // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@id",DbType.String,_ID),
                    objprovider.CreateInitializedParameter("@flag",DbType.String,flag),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[2].Size = 20;
                prmContent[2].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "proc_DeleteAddressStructure", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[2].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[2].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
