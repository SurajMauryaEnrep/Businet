using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.UTILITIES;
using EnRepMobileWeb.SERVICES.ISERVICES;

namespace EnRepMobileWeb.SERVICES.SERVICES
{
    public class MENU_SERVICES : MENU_ISERVICES
    {
        public DataSet GetAllMenuDAL( string CompID, string Language,string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        /*Passing perameter to sotore procedure*/                                                                                                      
                                                        objProvider.CreateInitializedParameter("@SearchMenu",DbType.String,null),
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                                                        objProvider.CreateInitializedParameter("@Language",DbType.String,Language),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.String,UserID),
                                                    };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$Comp$menu_GetAllMenu_new", prmContentGetDetails);
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
        public DataSet GetAllTopNavBrchList(string CompID, string User_id, string brId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                 objProvider.CreateInitializedParameter("@BrID",DbType.String,brId),
                 objProvider.CreateInitializedParameter("@User_id",DbType.String,User_id),
                 };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$comp$detail_GetBrchList", prmContentGetDetails);
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
        public DataSet GetMyFavListItems(string compId, string language, string userId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@Language",DbType.String, language),
                    objProvider.CreateInitializedParameter("@UserID",DbType.String,  userId)
                 };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetMyFavMenu", prmContentGetDetails);
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
        public DataSet GetMyFavMenuDetails(string compId, string userId, string docId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                   objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@UserId",DbType.String, userId),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String,  docId)
                 };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_CheckMyFavMenuExist", prmContentGetDetails);
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
        public int SaveUpdateMyFavouriteMenu( string action,string compId, string userId, string docId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Action",DbType.String, action),
                    objProvider.CreateInitializedParameter("@CompId",DbType.String, compId),
                    objProvider.CreateInitializedParameter("@UserId",DbType.String, userId),
                    objProvider.CreateInitializedParameter("@DocId",DbType.String,  docId)
                };
                int result = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_Stp$Fav$Details", prmContentGetDetails);
                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
    }
}
