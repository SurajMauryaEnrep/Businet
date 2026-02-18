using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.CustomerPriceList;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.CustomerPriceList;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SalesAndDistribution.CustomerPriceList
{
   public class CustomerPriceList_SERVICES : CustomerPriceList_ISERVICES
    {
        public DataTable GetCustPriceGrpDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable GetCustPriceGrp = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Cust$pricegroup", prmContentGetDetails).Tables[0];
                return GetCustPriceGrp;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public String InsertPriceListDetail(DataTable PriceListHeader, DataTable PriceListItemDetail, DataTable PriceListPriceGroup)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured, PriceListHeader ),
                 objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, PriceListItemDetail ),
                   objprovider.CreateInitializedParameterTableType("@PriceGroupDetail",SqlDbType.Structured, PriceListPriceGroup ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                };
                prmcontentaddupdate[3].Size = 100;
                prmcontentaddupdate[3].Direction = ParameterDirection.Output;

                string insp_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertPriceList_Details", prmcontentaddupdate).ToString();

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
        public DataSet GetviewPriceListdetail(string ListNo, string BrchID, string Comp_ID, string userid, string DocumentMenuId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, Comp_ID),
                      objProvider.CreateInitializedParameter("@br_id",DbType.String, BrchID),
                        objProvider.CreateInitializedParameter("@list_no",DbType.String, ListNo),
                     objProvider.CreateInitializedParameter("@UserID",DbType.String, userid),
                    objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),

            };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_PriceListDetailView", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DataTable GetPriceList(string CompId, string BrchID, string userid, string wfstatus, string DocumentMenuId, string Act_Status)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@userid",DbType.String, userid),
                                                        objProvider.CreateInitializedParameter("@wfstatus",DbType.String, wfstatus),
                                                        objProvider.CreateInitializedParameter("@DocumentMenuId",DbType.String, DocumentMenuId),
                                                        objProvider.CreateInitializedParameter("@Act_Status",DbType.String, Act_Status),
                                                      };
                DataSet GetPriceList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "GetPriceList", prmContentGetDetails);
                return GetPriceList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet PriceListDetailDelete(PriceListDetailModel _PriceListModel, string comp_id, string BrchID, string DocumentMenuID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int16, comp_id),
                    objProvider.CreateInitializedParameter("@list_no",DbType.String,_PriceListModel.list_no),
                    //objProvider.CreateInitializedParameter("@create_date",DbType.Date,_PriceListModel.create_dt),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BrchID),
                      objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.Date,DocumentMenuID),
                                                     };
                DataSet PriceList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Delete$PriceList", prmContentGetDetails);
                return PriceList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet PriceListApprove(PriceListDetailModel _PriceListModel, string comp_id, string BrchID, /*string app_id, int list_no,*/string mac_id, string DocumentMenuId,string app_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int16, comp_id),
                     objProvider.CreateInitializedParameter("@BrchID",DbType.Int16, BrchID),
                      objProvider.CreateInitializedParameter("@list_no",DbType.Int32,  _PriceListModel.list_no),
                    objProvider.CreateInitializedParameter("@create_dt1",DbType.Date,  _PriceListModel.create_dt),
                    objProvider.CreateInitializedParameter("@CreateBy",DbType.String, _PriceListModel.CreatedBy ),
                    //objProvider.CreateInitializedParameter("@list_no",DbType.String, list_no),
                    objProvider.CreateInitializedParameter("@app_id",DbType.Int16, app_id ),
                     objProvider.CreateInitializedParameter("@mac_id",DbType.String, mac_id),
                         objProvider.CreateInitializedParameter("@wf_status",DbType.String, _PriceListModel.A_Status ),
                         objProvider.CreateInitializedParameter("@wf_level",DbType.String, _PriceListModel.A_Level ),
                         objProvider.CreateInitializedParameter("@wf_remarks",DbType.String, _PriceListModel.A_Remarks ),
                         objProvider.CreateInitializedParameter("@menuid",DbType.String,DocumentMenuId ),
                          //objProvider.CreateInitializedParameter("@app_id",DbType.String,app_id ),
                     };
                DataSet PriceList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$ApprovePriceList", prmContentGetDetails);
                return PriceList; ;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }
        public DataTable GetCustPriceHistryDtl(string Comp_ID, string Br_ID, string Doc_no, string Doc_dt, string Item_id/*, string hd_Status*/)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, Comp_ID),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, Br_ID),
                                                        objProvider.CreateInitializedParameter("@DocNo",DbType.String, Doc_no),
                                                        objProvider.CreateInitializedParameter("@DocDt",DbType.String, Doc_dt),
                                                        objProvider.CreateInitializedParameter("@ItemId",DbType.String, Item_id),
                                                        //objProvider.CreateInitializedParameter("@Status",DbType.String, hd_Status),
                                                      };

                DataTable GetCustPriceHisDtl = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_GetCustPriceHistoryDetail", prmContentGetDetails).Tables[0];
                return GetCustPriceHisDtl;
                
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetMasterDropDownList(string CompID, string Br_Id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, Br_Id),
                };
                DataSet GetList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_Get$CPL$Master$Dropdown", prmContentGetDetails);
                return GetList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetPriceListName(string CompID, string Br_Id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, Br_Id),
                };
                DataTable GetList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$plist$name", prmContentGetDetails).Tables[0];
                return GetList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetPriceListItemDetail(string CompID, string Br_Id,string list_no)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@Br_Id",DbType.String, Br_Id),
                    objProvider.CreateInitializedParameter("@list_no",DbType.String, list_no),
                };
                DataTable GetList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Get$plist$item$detail", prmContentGetDetails).Tables[0];
                return GetList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
