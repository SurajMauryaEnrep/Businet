using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services
{
   public class OCList_SERVICES : OCList_ISERVICES 
    {
        //public Dictionary<string, string> OCSetupGroupDAL(string GroupName, string CompID)
        //{
        //    Dictionary<string, string> ddlOCNameDictionary = new Dictionary<string, string>();
        //    string firstItem = string.Empty;

        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
        //            objProvider.CreateInitializedParameter("@OCName",DbType.String, GroupName),
        //            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
        //                                             };

        //        DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$OCName]", prmContentGetDetails);

        //        if (PARQusData.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
        //            {
        //                ddlOCNameDictionary.Add(PARQusData.Tables[0].Rows[i]["oc_id"].ToString(), PARQusData.Tables[0].Rows[i]["oc_name"].ToString());
        //            }
        //        }
        //        return ddlOCNameDictionary;

        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetOTClist(string GroupName, string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@OCName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataSet GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$OCName]", prmContentGetDetails);
                return GetOCList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } 
        public DataTable GetOCListDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataTable GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$OCList$detail", prmContentGetDetails).Tables[0];
                return GetOCList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetOCListFilterDAL(string CompID, string OCID, string ActStatus, string OCtype, string Hsn_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@OCID",DbType.String,OCID),
                     objProvider.CreateInitializedParameter("@ActStatus",DbType.String,ActStatus),
                      objProvider.CreateInitializedParameter("@OCtype",DbType.String,OCtype),
                      objProvider.CreateInitializedParameter("@Hsn_ID",DbType.String,Hsn_ID),
              };
                DataSet GetOCList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetOCFilterList", prmContentGetDetails);
                return GetOCList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
