using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.MiscellaneousSetup
{
    public class HSNCode_SERVICES : HSNCode_ISERVICES
    {
        public string SaveHSNDetails(string TransType, string hsn_id, string dbk_code, string hsn_no, string Remarks, string hsn_des, string mac_id, string comp_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                   // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),                 
                    objprovider.CreateInitializedParameter("@hsn_id",DbType.String,hsn_id),
                    objprovider.CreateInitializedParameter("@hsn_no",DbType.String,hsn_no),
                       objprovider.CreateInitializedParameter("@remarks",DbType.String,Remarks),
                       objprovider.CreateInitializedParameter("@hsn_Des",DbType.String,hsn_des),
                    objprovider.CreateInitializedParameter("@mac_id",DbType.String,mac_id),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                   // objprovider.CreateInitializedParameter("@setup_flag",DbType.String,setup_flag),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameter("@dbk_code",DbType.String,dbk_code),

                };
                prmContent[7].Size = 100;
                prmContent[7].Direction = ParameterDirection.Output;
                 string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertAndUpdateHsnDetail", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[7].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[7].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeleteHSNDetail(string hsn_number, string comp_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {                   
                    objprovider.CreateInitializedParameter("@hsn_no",DbType.String,hsn_number),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[2].Size = 100;
                prmContent[2].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteHsnDetail", prmContent).ToString();
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
        public DataSet Get_HsnDetails(string comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     //objProvider.CreateInitializedParameter("@flag",DbType.String,"getaddressstructuredetails"),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_HsnDetails", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetTaxDetailAgainstHSN(string hsn_number, string comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                      objProvider.CreateInitializedParameter("@hsn_no",DbType.String,hsn_number),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetTaxDetailAgainstHSN", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
