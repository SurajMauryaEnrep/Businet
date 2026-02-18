using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.MIS.MaterialTracking;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.MIS.MaterialTracking;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.SubContracting.MIS.MaterialTracking
{
   public class MaterialTracking_SERVICE : MaterialTracking_ISERVICE
    {
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, "D"),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppListDic.Add(PARQusData.Tables[0].Rows[i]["supp_id"].ToString(), PARQusData.Tables[0].Rows[i]["supp_name"].ToString());
                    }
                }
                return ddlSuppListDic;

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
        public DataSet BindProductNameInDDL(string CompID, string BrID, string ItmName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/                  
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, BrID),
                    objProvider.CreateInitializedParameter("@ItmName",DbType.String, ItmName),
                                                     };
                DataSet Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_SC$Job$BindItmListName", prmContentGetDetails);
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetJobORDDocList(string Comp_ID, string Br_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@Comp_ID",DbType.String, Comp_ID),
                                                         objProvider.CreateInitializedParameter("@Br_ID",DbType.String, Br_ID),
                                                          
                                                      };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$SC$MIS$MTGetJobORDNoList", prmContentGetDetails);
                return ds.Tables[0];
                //return searchmenu;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataTable GetMTTrackDetailList_Onload(string CompId, string BranchID, string SuppId, string OutOPProdctID, string JobOrdNo, MaterialTracking_Model _MaterialTracking_Model)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@OutOPProdctID",DbType.String, OutOPProdctID),

                    objProvider.CreateInitializedParameter("@JobOrdNo",DbType.String, JobOrdNo),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,_MaterialTracking_Model.FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,_MaterialTracking_Model.ToDate),
                                                        //objProvider.CreateInitializedParameter("@Status",DbType.String, Status)

                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$SC$MIS$MaterialTrackOnLoad", prmContentGetDetails);
                return GetPODetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetMaterialTrackDetail(string CompId, string BranchID, string SuppId,string OutOPProdctID, string JobOrdNO, string FromDate, string ToDate)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BranchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String, SuppId),
                                                        objProvider.CreateInitializedParameter("@OutOPProdctID",DbType.String, OutOPProdctID),
                                                        objProvider.CreateInitializedParameter("@JobOrdNO",DbType.String, JobOrdNO),
                                                        objProvider.CreateInitializedParameter("@Fromdate",DbType.String,FromDate),
                                                        objProvider.CreateInitializedParameter("@Todate",DbType.String,ToDate),
                                                        //objProvider.CreateInitializedParameter("@Status",DbType.String, Status)

                                                      };
                DataSet GetPODetailList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$SC$MIS$MaterialTrackOnLoad", prmContentGetDetails);
                return GetPODetailList.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetMTAllQtyItemDetailList(string Comp_ID, string Branch, string Type, string JobOrdNo, string hdnJobOrdDt,string MaterialID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, Comp_ID),
                    objProvider.CreateInitializedParameter("@BrID",DbType.String, Branch),
                    objProvider.CreateInitializedParameter("@Type",DbType.String, Type),
                     objProvider.CreateInitializedParameter("@JobOrdNo",DbType.String, JobOrdNo),
                    objProvider.CreateInitializedParameter("@JobOrdDt",DbType.String, hdnJobOrdDt),
                    objProvider.CreateInitializedParameter("@MaterialID",DbType.String, MaterialID),
                    //objProvider.CreateInitializedParameter("@OpId",DbType.String, OpId),
                    //objProvider.CreateInitializedParameter("@OpOutItmID",DbType.String, OpOutItmID)
                    
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$SC$MIS$MaterialTrackingAllQtyDetail", prmContentGetDetails);
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
