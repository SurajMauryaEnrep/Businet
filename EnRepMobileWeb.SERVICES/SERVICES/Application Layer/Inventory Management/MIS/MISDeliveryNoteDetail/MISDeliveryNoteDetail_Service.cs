using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISDeliveryNoteDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.MISDeliveryNoteDetail
{
   public class MISDeliveryNoteDetail_Service : MISDeliveryNoteDetail_IService
    {
        public DataSet Get_FYList(string Compid, string Brid)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CompID",DbType.String,Compid),
                     objProvider.CreateInitializedParameter("@BrID",DbType.String,Brid),
                };
                DataSet Getfy_list = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$fy_GetList", prmContentGetDetails);
                return Getfy_list;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSuppliersAndItemList(string CompID, string BrchID, string Supp_Name, string Item_Name)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, Supp_Name),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, Item_Name),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetSupplierAndItemNameList", prmContentGetDetails);
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
        public DataTable GetDeliveryNoteMISReport(string compId, string brId, string showAs, string fromdate, string toDate, string suppId, string itemId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                                                        objProvider.CreateInitializedParameter("@BrId",DbType.String, brId),
                                                        objProvider.CreateInitializedParameter("@ShowAs",DbType.String, showAs),
                                                        objProvider.CreateInitializedParameter("@FromDate",DbType.String, fromdate),
                                                        objProvider.CreateInitializedParameter("@ToDate",DbType.String, toDate),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,suppId),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, itemId),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetDeliveryNoteMIS_Report", prmContentGetDetails);
                return GetPODetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetSubItemDetails(string comp_ID, string br_ID,  string DnNo, string DnDate, string item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, comp_ID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int32, br_ID),
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String, DnNo),
                    objProvider.CreateInitializedParameter("@dn_dt",DbType.String, DnDate),
                    objProvider.CreateInitializedParameter("@item_id",DbType.String, item_id),
                    
                   };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_MISDN_GetSubItemDetails", prmContentGetDetails).Tables[0];
                return Getsuppport; 
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
