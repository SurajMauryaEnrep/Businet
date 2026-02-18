using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnRepMobileWeb.UTILITIES;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management;
using EnRepMobileWeb.MODELS.ApplicationLayer.Inventory_Management;
namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management
{
    public class DeliveryNoteList_Services : DeliveryNoteList_IServices
    {
        public Dictionary<string, string> GetSupplierListALl(string CompID, string SuppName, string BrchID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppListAll", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
                    }
                }
                return ddlItemNameDictionary;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }
        public  DataSet GetAllData(string CompID, string SuppName, string BrchID, string dn_no, string UserID, string wfstatus, string DocumentMenuId
            , string SuppId, string SourceType, string Fromdate, string Todate, string Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                             objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                             objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                             objProvider.CreateInitializedParameter("@dn_no",DbType.String, dn_no),
                             objProvider.CreateInitializedParameter("@UserID",DbType.String, UserID),
                                 objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                             objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),                        
                             objProvider.CreateInitializedParameter("@supp_id",DbType.String, SuppId),
                               objProvider.CreateInitializedParameter("@SourceType",DbType.String,SourceType),
                               objProvider.CreateInitializedParameter("@FromDate",DbType.String, Fromdate),
                               objProvider.CreateInitializedParameter("@ToDate",DbType.String, Todate),
                               objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
                                                             };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetAllData$inv$del$note", prmContentGetDetails);
                return PARQusData;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
            return null;
        }

        public DataSet GetStatusList(string MenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@menu_id",DbType.String, MenuID),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "fct$DocStatus", prmContentGetDetails);
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
        public DataSet GetDeliveryNoteListAll(string dn_no, string CompID, string BrchID, string UserID, string wfstatus, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@dn_no",DbType.String, dn_no),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@userid",DbType.String, UserID),
                    objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId)
                                                     };
            DataSet dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$DeliveryNoteList$details]", prmContentGetDetails);
            return dt;
        }

        public DataTable GetDeliveryNoteSearch(string SuppId, string SourceType, string Fromdate, string Todate, string Status, string CompID, string BrchID, string DocumentMenuId)
        {
            SqlDataProvider objProvider = new SqlDataProvider();
            SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
            objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
            objProvider.CreateInitializedParameter("@BrId",DbType.String, BrchID),
            objProvider.CreateInitializedParameter("@supp_id",DbType.String, SuppId),
            objProvider.CreateInitializedParameter("@SourceType",DbType.String,SourceType),
            objProvider.CreateInitializedParameter("@FromDate",DbType.String, Fromdate),
            objProvider.CreateInitializedParameter("@ToDate",DbType.String, Todate),
            objProvider.CreateInitializedParameter("@Status",DbType.String, Status),
            objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId)
            };
            DataTable dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$DeliveryNoteList$Search]", prmContentGetDetails).Tables[0];
            return dt;
        }
    }
}
