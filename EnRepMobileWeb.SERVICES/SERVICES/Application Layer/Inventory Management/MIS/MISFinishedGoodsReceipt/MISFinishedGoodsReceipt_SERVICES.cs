using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MIS.MISFinishedGoodsReceipt;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Inventory_Management.MIS.MISFinishedGoodsReceipt
{
    public class MISFinishedGoodsReceipt_SERVICES : MISFinishedGoodsReceipt_ISERVICES
    {
        public Dictionary<string, string> ItemList(string GroupName, string CompID,string BrchID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MIS$FGR$Item$TableBind", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "All";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["Item_id"].ToString(), PARQusData.Tables[0].Rows[i]["Item_name"].ToString());
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
        public Dictionary<string, string> ItemGroupList(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@GroupName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };

                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$grp_GetAllItemGroupNoodChilds", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                //dr[0] = "0";
                //dr[1] = "All";
                //PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["item_grp_id"].ToString(), PARQusData.Tables[0].Rows[i]["ItemGroupChildNood"].ToString());
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

        public DataTable GetDataTableMISFGR(string CompID, string BrchID, string itmid, string GroupID, 
            string txtFromdate, string txtTodate,string MultiselectItemHdn,string ShopFloor_id,string ddl_ShowAs)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@itmid",DbType.String, itmid),
                    objProvider.CreateInitializedParameter("@GroupID",DbType.String, GroupID),
                    objProvider.CreateInitializedParameter("@txtFromdate",DbType.String, txtFromdate),
                    objProvider.CreateInitializedParameter("@txtTodate",DbType.String, txtTodate),
                    objProvider.CreateInitializedParameter("@MultiselectItemHdn",DbType.String, MultiselectItemHdn),
                    objProvider.CreateInitializedParameter("@ShopFloor_id",DbType.String, ShopFloor_id),
                    objProvider.CreateInitializedParameter("@ddl_ShowAs",DbType.String, ddl_ShowAs),
                                                     };

                DataTable dataSerch = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$MIS$FGR", prmContentGetDetails).Tables[0];
                return dataSerch;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
          
        }
        public DataSet GetBatchDeatilData(string CompID, string BrchID, string rcpt_no, string rcpt_dt, string item_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_ID",DbType.String, BrchID),
                    objProvider.CreateInitializedParameter("@FGR_No",DbType.String, rcpt_no),
                    objProvider.CreateInitializedParameter("@FGR_dt",DbType.String, rcpt_dt),
                    objProvider.CreateInitializedParameter("@ItemID",DbType.String, item_id),
                };
                DataSet dataSerch = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "MIS$fgr$in_item$bt$detail_GetStockBatchwise", prmContentGetDetails);
                return dataSerch;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
