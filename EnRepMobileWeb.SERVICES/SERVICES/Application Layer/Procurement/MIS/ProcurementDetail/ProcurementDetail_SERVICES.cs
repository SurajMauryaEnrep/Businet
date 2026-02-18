using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.MIS.ProcurementDetail;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.MIS.ProcurementDetail;
using EnRepMobileWeb.UTILITIES;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnRepMobileWeb.SERVICES.SERVICES.Application_Layer.Procurement.MIS.ProcurementDetail
{
    public class ProcurementDetail_SERVICES: ProcurementDetail_ISERVICES
    {
        public DataSet GetAllDDLData(string Comp_ID, string Br_ID, string SupplierName, string GroupName, string PortfolioName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@CompID",DbType.String,Comp_ID),
                 objProvider.CreateInitializedParameter("@BrId",DbType.String,Br_ID),
                 objProvider.CreateInitializedParameter("@SuppName",DbType.String,SupplierName),
                 objProvider.CreateInitializedParameter("@GroupName",DbType.String,GroupName),
                 objProvider.CreateInitializedParameter("@PortfName",DbType.String,PortfolioName)
                 

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_prc$MIS_GetAllDropdownListsData", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> ItemSetupHSNDAL(string CompID, string HSNName)
        {
            Dictionary<string, string> ddlcountryDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@HSN_code",DbType.String, HSNName),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "stp$item$hsn_GetAllItemhsn_new", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                //dr[0] = "0";
                //dr[1] = "---Select---";
                //PARQusData.Tables[0].Rows.InsertAt(dr, 0);

                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlcountryDictionary.Add(PARQusData.Tables[0].Rows[i]["setup_id"].ToString(), PARQusData.Tables[0].Rows[i]["setup_val"].ToString());
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
            //return null;
        }
        public DataSet GetPrcFilteredReport(Search_Parmeters model)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,model.CompId),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,model.BrId),
                 objProvider.CreateInitializedParameter("@supp_id",DbType.String,model.Supp_id),
                 objProvider.CreateInitializedParameter("@inv_type",DbType.String,model.Inv_type),
                 objProvider.CreateInitializedParameter("@ItemGrp",DbType.String,model.Group),
                 objProvider.CreateInitializedParameter("@ItemPort",DbType.String,model.Item_PortFolio),
                 objProvider.CreateInitializedParameter("@Item_Id",DbType.String,model.ItemID),
                 objProvider.CreateInitializedParameter("@suppCat",DbType.String,model.category),
                 objProvider.CreateInitializedParameter("@suppPort",DbType.String,model.portFolio),
                 objProvider.CreateInitializedParameter("@inv_no",DbType.String,model.Inv_no),
                 objProvider.CreateInitializedParameter("@inv_dt",DbType.String,model.Inv_dt),
                 objProvider.CreateInitializedParameter("@from_dt",DbType.String,model.From_dt),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,model.To_dt),
                 objProvider.CreateInitializedParameter("@Flag",DbType.String,model.flag),
                 objProvider.CreateInitializedParameter("@HSN_code",DbType.String,model.HSN_code),
                 objProvider.CreateInitializedParameter("@forCsv",DbType.String,"CSV"),
                 objProvider.CreateInitializedParameter("@RCM",DbType.String,model.RCMApp),/*add by Hina sharma on 30-06-2025*/
                 objProvider.CreateInitializedParameter("@brid_list",DbType.String,model.brid_list),
                 objProvider.CreateInitializedParameter("@currency",DbType.String,model.currency)

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$MIS_Get$purchaseDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetPrcFilteredReport(Search_Parmeters model, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,model.CompId),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,model.BrId),
                 objProvider.CreateInitializedParameter("@supp_id",DbType.String,model.Supp_id),
                 objProvider.CreateInitializedParameter("@inv_type",DbType.String,model.Inv_type),
                 objProvider.CreateInitializedParameter("@ItemGrp",DbType.String,model.Group),
                 objProvider.CreateInitializedParameter("@ItemPort",DbType.String,model.Item_PortFolio),
                 objProvider.CreateInitializedParameter("@Item_Id",DbType.String,model.ItemID),
                 objProvider.CreateInitializedParameter("@suppCat",DbType.String,model.category),
                 objProvider.CreateInitializedParameter("@suppPort",DbType.String,model.portFolio),
                 objProvider.CreateInitializedParameter("@inv_no",DbType.String,model.Inv_no),
                 objProvider.CreateInitializedParameter("@inv_dt",DbType.String,model.Inv_dt),
                 objProvider.CreateInitializedParameter("@from_dt",DbType.String,model.From_dt),
                 objProvider.CreateInitializedParameter("@to_dt",DbType.String,model.To_dt),
                 objProvider.CreateInitializedParameter("@Flag",DbType.String,model.flag),
                 objProvider.CreateInitializedParameter("@HSN_code",DbType.String,model.HSN_code),
                 objProvider.CreateInitializedParameter("@Skip",DbType.String,skip),
                 objProvider.CreateInitializedParameter("@PageSize",DbType.String,pageSize),
                 objProvider.CreateInitializedParameter("@Search",DbType.String,searchValue),
                 objProvider.CreateInitializedParameter("@sortColumn",DbType.String,sortColumn),
                 objProvider.CreateInitializedParameter("@sortColumnDir",DbType.String,sortColumnDir),
                 objProvider.CreateInitializedParameter("@RCM",DbType.String,model.RCMApp),/*add by Hina sharma on 30-06-2025*/
                 objProvider.CreateInitializedParameter("@forCsv",DbType.String,model.csvFlag),/*add by Suraj Maurya on 10-10-2025*/
                 objProvider.CreateInitializedParameter("@brid_list",DbType.String,model.brid_list),
                 objProvider.CreateInitializedParameter("@currency",DbType.String,model.currency)

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$MIS_Get$purchaseDetails", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet GetPrcDynamicTaxColumns(Search_Parmeters model)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails =
                 {
                    /*Passing perameter to sotore procedure*/                                                                                                      
                 objProvider.CreateInitializedParameter("@comp_id",DbType.String,model.CompId),
                 objProvider.CreateInitializedParameter("@br_id",DbType.String,model.BrId),
                 objProvider.CreateInitializedParameter("@Flag",DbType.String,model.flag)

                 };
                DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "prc$MIS_Get$DynamicTaxColumns_PrcDtl", prmContentGetDetails);
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetcategoryDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getcategory = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$category_Getcategory", prmContentGetDetails).Tables[0];
                return Getcategory;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public Dictionary<string, string> GetSupplierList(string CompID, string SuppName, string BranchID)
        {
            Dictionary<string, string> ddlSuppListDic = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                    objProvider.CreateInitializedParameter("@SuppName",DbType.String, SuppName),
                    //objProvider.CreateInitializedParameter("@SuppType",DbType.String, SuppType),
                    objProvider.CreateInitializedParameter("@BrchID",DbType.String, BranchID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetSuppList", prmContentGetDetails);
                DataRow dr;/*Commented by Hina on 15-11-2024 to modify multiselect dropdown*/
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
        public DataTable GetsuppportDAL(string CompID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@comp_id",DbType.Int32, CompID),
                                                    };
                DataTable Getsuppport = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp$portfolio_Getportfolio", prmContentGetDetails).Tables[0];
                return Getsuppport;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
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
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);

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
        public Dictionary<string, string> ItemPortfolioList(string GroupName, string CompID)
        {
            Dictionary<string, string> ddlItemNameDictionary = new Dictionary<string, string>();
            string firstItem = string.Empty;

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    objProvider.CreateInitializedParameter("@PortfName",DbType.String, GroupName),
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompID),
                                                     };
                DataSet PARQusData = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_GetItemPortfolioList]", prmContentGetDetails);
                DataRow dr;
                dr = PARQusData.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Tables[0].Rows.InsertAt(dr, 0);
                if (PARQusData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Tables[0].Rows.Count; i++)
                    {
                        ddlItemNameDictionary.Add(PARQusData.Tables[0].Rows[i]["setup_id"].ToString(), PARQusData.Tables[0].Rows[i]["setup_val"].ToString());
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
    }
    
}
