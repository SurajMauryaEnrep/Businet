using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.SampleTracking;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.SampleTracking
{
    public class SampleTracking_SERVICES:SampleTracking_ISERVICES
    {
        public DataSet getSuppCustList(string CompID, string BrchID, string EntityName, string EntityType)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                     objProvider.CreateInitializedParameter("@EntityName",DbType.String, EntityName),
                      objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_Supp_CustList", prmContentGetDetails);

                return PARQusData;

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
        public DataSet GetSTDetailsList(string CompId, string BrchID,string ReportType, string ItemName, string EntityName,
            string Fromdate, string Todate,string EntityType, string Issuedby,string ShowAs)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@ReportType",DbType.String, ReportType),
                                                        objProvider.CreateInitializedParameter("@itemname",DbType.String, ItemName),
                                                        objProvider.CreateInitializedParameter("@entityname",DbType.String,EntityName),
                                                        objProvider.CreateInitializedParameter("@fromdate",DbType.String, Fromdate),
                                                        objProvider.CreateInitializedParameter("@todate",DbType.String, Todate),
                                                         objProvider.CreateInitializedParameter("@EntityType",DbType.String, EntityType),
                                                         objProvider.CreateInitializedParameter("@Issuedby",DbType.String, Issuedby),
                                                         objProvider.CreateInitializedParameter("@ShowAs",DbType.String, ShowAs),
                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "inv$mi$detail_GetSampleTrackingDetail", prmContentGetDetails);
                return GetPODetailList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetSampleTrackingItmList(string CompID, string BrID, string ItmName)
        {
            //Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            //string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$ItemList$detail_GetSR_ItemList", prmContentGetDetails);

                //if (PARQusData.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                //    {
                //        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["item_id"].ToString(), PARQusData.Tables[0].Rows[i]["item_name"].ToString());
                //    }
                //}
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
        public Dictionary<string, string> IssueToList(string CompID, string Entity, string BrchID)
        {
            Dictionary<string, string> ddlcountryDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Entity",DbType.String, Entity),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$EntityList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlcountryDictionary.Add(PARQusData.Tables[0].Rows[i]["id"].ToString(), PARQusData.Tables[0].Rows[i]["val"].ToString());
                    }
                }
                return ddlcountryDictionary;

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
        public DataSet GetItemIssued_ReceivedList(string CompId, string BranchID, string EntityID,
            string EntityTypeCode, string ItemID,string Type, string issue_date, 
            string receive_date, string sr_type, string other_dtl, string ST_UOM, string fromdate, string todate,string Issuedby)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@ItemID",DbType.String, ItemID),
                     objProvider.CreateInitializedParameter("@EntityID",DbType.String, EntityID),
                     objProvider.CreateInitializedParameter("@EntityTypeCode",DbType.String, EntityTypeCode),
                     objProvider.CreateInitializedParameter("@Type",DbType.String, Type),
                     objProvider.CreateInitializedParameter("@issue_date",DbType.String, issue_date),
                     objProvider.CreateInitializedParameter("@receive_date",DbType.String, receive_date),
                     objProvider.CreateInitializedParameter("@sr_type",DbType.String, sr_type),
                     objProvider.CreateInitializedParameter("@other_dtl",DbType.String, other_dtl),
                     objProvider.CreateInitializedParameter("@ST_UOM",DbType.String, ST_UOM),
                     objProvider.CreateInitializedParameter("@fromdate",DbType.String, fromdate),
                     objProvider.CreateInitializedParameter("@todate",DbType.String, todate),
                     objProvider.CreateInitializedParameter("@Issuedby",DbType.String, Issuedby),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Proc_GetSampleItemIssued_ReceivedDeatil", prmContentGetDetails);
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
        public DataSet GetIssuedByData(string CompID, string BrchID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompID),
                    objProvider.CreateInitializedParameter("@br_id",DbType.Int64, BrchID)
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$emp$issuedby$data$materialIssue", prmContentGetDetails);
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
