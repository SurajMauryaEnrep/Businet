using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemAttributeSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.ItemAttributeSetup
{
    public class ItemAttributeSetup_SERVICES:ItemAttributeSetup_ISERVICES
    {

        public DataSet getAttrName(string CompID)
        {
            try
            {
              
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@flag",DbType.String,"getAttrName"),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$GetAttrDetails", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeleteAttr(string TransType, string attr_id, string CompID)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                 
                    objprovider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@attr_id",DbType.String,attr_id),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteAttribute", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[3].Value != DBNull.Value) // status
                {
                    DocNo = prmContent[3].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    
        public string insertAttributeName(string CompID,string attr_id, string attrName,string TransType)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                 // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@attr_id",DbType.String,attr_id),
                    objprovider.CreateInitializedParameter("@attr_name",DbType.String,attrName),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "insertAttribute", prmContent).ToString();
                string DocNo = string.Empty;
                if (prmContent[4].Value != DBNull.Value) // status
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

        public string insertAttributeVal(string CompID, string attr_id, string attr_val_Id, string attr_val_Name, string TransType)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                 // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@attr_id",DbType.String,attr_id),
                    objprovider.CreateInitializedParameter("@attr_val_Id",DbType.String,attr_val_Id),
                    objprovider.CreateInitializedParameter("@attr_val_Name",DbType.String,attr_val_Name),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[5].Size = 100;
                prmContent[5].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "insertAttribute", prmContent).ToString();
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
    }
}
