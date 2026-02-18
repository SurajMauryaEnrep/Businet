using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.WarehouseSetup;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.WarehouseSetup
{
    public class WarehouseSetup_SERVICES : WarehouseSetup_ISERVICES
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

        public DataSet GetWarehouseDetails(string wh_type, string warehouse_id, string comp_id)
        {
            try
            {

                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@wh_type",DbType.String,wh_type),
                     objProvider.CreateInitializedParameter("@wh_id",DbType.String,warehouse_id),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                                                     };
                DataSet Getwh_details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ViewWarehouseDetailsList", prmContentGetDetails);
                return Getwh_details;
                //DataTable Getwh_details = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "ViewWarehouseDetailsList", prmContentGetDetails).Tables[0];
                //return Getwh_details;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string Delete_warehousedetails(string wh_id, string comp_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@flag",DbType.String,"Delete"),
                    objprovider.CreateInitializedParameter("@wh_id",DbType.String,wh_id),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,"")
                };
                prmContent[3].Size = 20;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "proc_DeleteWarehouseDetailsList", prmContent).ToString();
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
        public string insertWarehouseDetails(DataTable WarehouseDetail, DataTable WarehouseBranch)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@WarehouseDetail",SqlDbType.Structured, WarehouseDetail ),
                 objprovider.CreateInitializedParameterTableType("@BranchDetail",SqlDbType.Structured, WarehouseBranch ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertWarehouseDetail", prmcontentaddupdate).ToString();

                string DocNo = string.Empty;
                if (prmcontentaddupdate[2].Value != DBNull.Value) // status
                {
                    DocNo = prmcontentaddupdate[2].Value.ToString();
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
