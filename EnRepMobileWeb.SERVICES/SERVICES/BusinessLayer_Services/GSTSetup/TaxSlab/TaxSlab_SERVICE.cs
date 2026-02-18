using EnRepMobileWeb.MODELS.BusinessLayer.TaxSlab;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.GSTSetup.TaxSlab;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.GSTSetup.TaxSlab
{
    public class TaxSlab_SERVICE : TaxSlab_ISERVICE
    {
        public DataSet GetTaxSlabDetail(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/  
                   
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                     };
                DataSet GetviewGLdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$TaxSlab$ViewDetail]", prmContentGetDetails);
                return GetviewGLdetail;

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            {
            }
            //return null;
        }
        public string InsertTaxSlabDetail(string CompID, string taxper, string goods, string services, string transtype)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                 // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@tex_perc",DbType.String, taxper),
                    objprovider.CreateInitializedParameter("@goods",DbType.String,goods),
                    objprovider.CreateInitializedParameter("@service",DbType.String,services),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,transtype),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[5].Size = 100;
                prmContent[5].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[insertTexSlab]", prmContent).ToString();
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
        public string InsertHsnDetails(string CompID, string listTaxPer, string HSN_Number, string transtype)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                 // objprovider.CreateInitializedParameter("@flag",DbType.String,"Insertstate"),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@tax_perc",DbType.String, listTaxPer),
                    objprovider.CreateInitializedParameter("@hsn_no",DbType.String,HSN_Number),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,transtype),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")

                };
                prmContent[4].Size = 100;
                prmContent[4].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[insertHsnDetails]", prmContent).ToString();
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
        public DataSet getAttrName(string Comp_ID)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@flag",DbType.String,"getTaxPer"),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "proc$GetTaxPer", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string DeleteTaxSlab(string CompID, string tex, string texHsn, string transType)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {

                    objprovider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    objprovider.CreateInitializedParameter("@tex_perc",DbType.String,tex),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,transType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameter("@hsn_no",DbType.String,texHsn),

                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "DeleteTaxSlab", prmContent).ToString();
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
        public DataSet GetModulAndHsnDetails(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                        //objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, Templateid),

                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetHSNListTaxSlab", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } 
        public DataSet GetDataDropDownList(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                                                          objProvider.CreateInitializedParameter("@flag",DbType.String,"getTaxPer"),
                                                        //objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, Templateid),

                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$all$DropDown", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }

}
