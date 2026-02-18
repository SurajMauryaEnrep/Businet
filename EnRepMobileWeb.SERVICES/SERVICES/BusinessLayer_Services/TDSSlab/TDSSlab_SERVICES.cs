using EnRepMobileWeb.MODELS.BusinessLayer.TDSSlab;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TDSSlab;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.TDSSlab
{
    public class TDSSlab_SERVICES: TDSSlab_ISERVICES
    {
        public DataSet GetTDSList(string comp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@comp_id",DbType.String, comp_id),
                                                        //objProvider.CreateInitializedParameter("@tmplt_id",DbType.String, Templateid),

                                                      };
                DataSet SOData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "TDSSlabList", prmContentGetDetails);
                return SOData;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string TDS_SlabInserUpdate(TDSSlab_Model _Model,string TransType)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameter("@comp_id",DbType.String,_Model.comp_id),
                                                        objprovider.CreateInitializedParameter("@slab_id",DbType.String,_Model.slab_id),
                                                        objprovider.CreateInitializedParameter("@value_from",DbType.String,_Model.value_from),
                                                        objprovider.CreateInitializedParameter("@value_to",DbType.String,_Model.value_to),
                                                        objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                                                        objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                                                        objprovider.CreateInitializedParameter("@tds_perc",DbType.String,_Model.tds_perc),
                                                        objprovider.CreateInitializedParameter("@tds_acc_id",DbType.String,_Model.tds_acc_id),
                                                        objprovider.CreateInitializedParameter("@tcs_acc_id",DbType.String,_Model.tcs_acc_id),

                                                    };
                prmcontentaddupdate[5].Size = 100;
                prmcontentaddupdate[5].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "TDSSlabInsertUpdate", prmcontentaddupdate).ToString();
                string DocNo = string.Empty;
                if (prmcontentaddupdate[5].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[5].Value.ToString();
                }
                return DocNo;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string TDS_SlabDelete(string comp_id,string slab_id, string TransType)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                                                        objprovider.CreateInitializedParameter("@slab_id",DbType.String,slab_id),
                                                        objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                                                        objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),

                                                    };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "TDSSlabDelete", prmcontentaddupdate).ToString();
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


    }
}
