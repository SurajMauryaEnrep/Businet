using EnRepMobileWeb.MODELS.BusinessLayer.ProspectSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ProspectSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.ProspectSetup
{
    public class ProspectSetup_SERVICES : ProspectSetup_ISERVICES
    {
        public DataSet GetProspectDetailsList(string CompID,string Br_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                     };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$ProspectViewdetailList]", prmContentGetDetails);
                return dt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetCityList(string GroupName)
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

        public DataTable GetCurronProspectTypeDAL(string CompID, string prosType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@Supptype",DbType.String,prosType),
                };
                DataTable GetCurronSuppType = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp_CurronType", prmContentGetDetails).Tables[0];
                return GetCurronSuppType;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeleteProsId(ProspectSetupMODEL prospectSetupMODEL, string command, string Comp_ID,string Br_ID)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameter("@pros_id",DbType.String, prospectSetupMODEL.pros_id ),
                 objprovider.CreateInitializedParameter("@comp_id",DbType.String,Comp_ID),
                 objprovider.CreateInitializedParameter("@br_id",DbType.String,Br_ID),
                 objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                 
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteProspect_Detail", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[3].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;

            }
        }
        public DataSet Getviewprospectdetail(string Custcode, string CompID,string Br_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Pros_id",DbType.String, Custcode),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@br_id",DbType.String, Br_ID),
                                                     };
                DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$ProspectViewdetail]", prmContentGetDetails);
                return dt;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
          
        }
        public string insertProspectDetails(DataTable ProspectDetail)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@ProspectDetail",SqlDbType.Structured, ProspectDetail ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[1].Size = 100;
                prmcontentaddupdate[1].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertProspect_Detail", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[1].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[1].Value.ToString();
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
