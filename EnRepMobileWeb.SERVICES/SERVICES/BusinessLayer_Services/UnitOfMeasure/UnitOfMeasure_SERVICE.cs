using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.UnitOfMeasure;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.UnitOfMeasure
{
    public class UnitOfMeasure_SERVICE: UnitOfMeasure_ISERVICE
    {
        public DataSet GetUOMTable(string comp_id,string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@brid",DbType.String,br_id),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get_uom_Table", prmContentGetDetails);
                return GetBrList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public string SaveUOMData(string TransType, string comp_id, string user_id, string mac_id,string uom_id, string uom_name, string uom_alias, string item_id, string Conv_uom_id, string conv_rate,string ShowStock)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@user_id",DbType.String,user_id),
                    objprovider.CreateInitializedParameter("@mac_id",DbType.String,mac_id),
                    objprovider.CreateInitializedParameter("@uom_id",DbType.String,uom_id),
                    objprovider.CreateInitializedParameter("@uom_name",DbType.String,uom_name),
                    objprovider.CreateInitializedParameter("@uom_alias",DbType.String,uom_alias),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameter("@item_id",DbType.String,item_id),
                    objprovider.CreateInitializedParameter("@conv_uom_id",DbType.String,Conv_uom_id),
                    objprovider.CreateInitializedParameter("@conv_rate",DbType.String,conv_rate),
                    objprovider.CreateInitializedParameter("@ShowStock",DbType.String,ShowStock),
                };
                prmContent[7].Size = 100;
                prmContent[7].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertAndUpdateUOM_Data", prmContent).ToString();
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
        public string DeleteUOM(string TransType, string uom_id, string comp_id,string Conv_item_id,string conv_uom_id)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmContent =
                {
                    objprovider.CreateInitializedParameter("@uom_id",DbType.String,uom_id),
                    objprovider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                    objprovider.CreateInitializedParameter("@TransType",DbType.String,TransType),
                    objprovider.CreateInitializedParameter("@DocNo",DbType.String,""),
                    objprovider.CreateInitializedParameter("@Conv_item_id",DbType.String,Conv_item_id),
                    objprovider.CreateInitializedParameter("@conv_uom_id",DbType.String,conv_uom_id),
                };
                prmContent[3].Size = 100;
                prmContent[3].Direction = ParameterDirection.Output;
                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "deleteUOM", prmContent).ToString();
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
        public DataTable GetItemNameLists(string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetItem_List]", prmContentGetDetails).Tables[0];
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
        public DataSet GetAllDataDropDown(string CompId,string br_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, br_id),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get$All$DropDown$UnitOF$Measure]", prmContentGetDetails);
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
        public DataTable GetUomNameLists(string CompId, string br_id,string searchValue= null)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@brid",DbType.Int64,br_id),
                    objProvider.CreateInitializedParameter("@searchValue",DbType.String,searchValue),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Get_uom_Table]", prmContentGetDetails).Tables[0];
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
    }
}
