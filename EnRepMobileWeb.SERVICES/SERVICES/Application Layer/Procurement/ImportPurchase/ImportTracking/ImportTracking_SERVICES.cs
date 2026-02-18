using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ImportPurchase.ImportTracking;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.ImportPurchase.ImportTracking
{
   public class ImportTracking_SERVICES : ImportTracking_ISERVICES
    {
        public Dictionary<string, string> GetSupplierList(string CompID, string BranchID ,string SuppType, string SuppName)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                    objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    
                    
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$supp$detail_GetSuppList", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
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
        public DataSet GetSrcDocNumberList(string CompId, string BrID, string Supp_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@Supp_id",DbType.String, Supp_id),

                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_prc$ImprtTrck$GetImportPONoList", prmContentGetDetails);
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
        public DataSet GetPONumberDetail(string CompId, string BrID, string PONo)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                    objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                    objProvider.CreateInitializedParameter("@PONo",DbType.String, PONo),
                };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_prc$ImprtTrck$GetPONumberDetail", prmContentGetDetails);
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
        public string InsertImportTrackingDetails(DataTable DTHeaderDetail, DataTable DTItemDetail/*, DataTable DtblAttchDetail*/)
        {
            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {
                                                        objprovider.CreateInitializedParameterTableType("@HeaderDetail",SqlDbType.Structured,DTHeaderDetail),
                                                        objprovider.CreateInitializedParameterTableType("@ItemDetail",SqlDbType.Structured, DTItemDetail),
                                                        objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                                                        //objprovider.CreateInitializedParameterTableType("@AttachmentDetail",SqlDbType.Structured,DtblAttchDetail),
                                                         
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;
                string companyid = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "SP_Prc$ImpTrack$InsertData", prmcontentaddupdate).ToString();
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

            finally
            {
            }
        }
        public DataSet Edit_ImpTrackDetail(string CompId, string BrID, string PONo)
        {
            try
            {
                DataSet searchmenu = new DataSet();
                SqlDataProvider objProvider = new SqlDataProvider();


                SqlParameter[] prmContentGetDetails = {

                                                        objProvider.CreateInitializedParameter("@CompID",DbType.Int64, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.Int64, BrID),
                                                        objProvider.CreateInitializedParameter("@PONo",DbType.String, PONo),
                                                        //objProvider.CreateInitializedParameter("@DocumentMenuID",DbType.String, DocumentMenuID),
                                                        //objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),
                                                        
                                                   };
                searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "SP_Prc$ImpTrack$GetViewDetails", prmContentGetDetails);


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
        public DataSet GetAllDocImpTrackList(string CompId, string BrchID, string suppID, string SrcDocNo, string UserID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                                                        objProvider.CreateInitializedParameter("@BrID",DbType.String, BrchID),
                                                        objProvider.CreateInitializedParameter("@SuppId",DbType.String,suppID),
                                                        objProvider.CreateInitializedParameter("@SrcDocNo",DbType.String, SrcDocNo),
                                                        objProvider.CreateInitializedParameter("@UserID",DbType.Int64, UserID),


                                                      };
                DataSet GetList = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[SP_Prc$ImpTrack$GetAllDocSearchList]", prmContentGetDetails);
                return GetList;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
