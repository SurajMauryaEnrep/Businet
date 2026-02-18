using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ItemSearch;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.BusinessLayer_Services.ItemSearch
{
    public class ItemSearch_SERVICES : ItemSearch_ISERVICES
    {
      
        public DataSet GetItemListDetail(string CompID, string BranchId, string ItemID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BranchId",DbType.String,BranchId),
                     objProvider.CreateInitializedParameter("@ItemID",DbType.String,ItemID),
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$Item$Deatil$Data$Item$Search", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable BindItemList(string  ItemName, string CompID, string BranchId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@ItemName",DbType.String,ItemName),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BranchId",DbType.String,BranchId),
                   
                    
                                                     };
                DataTable GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Bind$ItemLIst$DropDown$List", prmContentGetDetails).Tables[0];
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPrintItemSearchDeatils(string CompID, string BranchId, string itemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@itemId",DbType.String,itemId),
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                     objProvider.CreateInitializedParameter("@BranchId",DbType.String,BranchId),
                   
                    
                                                     };
                DataSet GetBrList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$item$search$print", prmContentGetDetails);
                return GetBrList;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
