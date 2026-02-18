using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EnRepMobileWeb.MODELS.Factory_Settings.Organization_Setup;
using EnRepMobileWeb.SERVICES.ISERVICES.FactorySettings_ISERVICE.OrganizationSetup_ISERVICE;
using EnRepMobileWeb.UTILITIES;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnRepMobileWeb.SERVICES.SERVICES.FactorySettings_SERVICE.OrganizationSetup_SERVICE
{
    public class OrganizationSetup_SERVICE : OrganizationSetup_ISERVICE
    {
        //public string DeleteOSComDetail( int comid)
        //{
        //    try
        //    {
        //        SqlDataProvider objProvider = new SqlDataProvider();
        //        SqlParameter[] prmContentAddUpdate = {
                                        
        //                                objProvider.CreateInitializedParameter("@comp_id",DbType.Int64, comid),
        //                                objProvider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
        //                              };
        //        //string Result = string.Empty;
        //        string dn_no = Convert.ToString(SqlHelper.ExecuteScalar(CommandType.StoredProcedure, "[OSComDelete]", prmContentAddUpdate));
              
        //        string DocNo = string.Empty;
        //        if (prmContentAddUpdate[1].Value != DBNull.Value) // status
        //        {
        //            DocNo = prmContentAddUpdate[1].Value.ToString();
        //        }
        //        return DocNo;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}
        public DataSet BindLAng()
        {

            SqlParameter[] prmContentGetDetails = {
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$langList]", prmContentGetDetails);
            return ds;
        }
        public DataSet BindHeadOffice()
        {
            SqlParameter[] prmContentGetDetails = {
            };
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[fct$Comp$detail_GetHoListAll]", prmContentGetDetails);
            return ds;
        }
        public DataTable GetCurrencyList()
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "CurrencyList", prmContentGetDetails).Tables[0];
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
        public DataTable GetdOCNAME_ENG(string CompId)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                    objProvider.CreateInitializedParameter("@CompID",DbType.String, CompId),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "Getdocname", prmContentGetDetails).Tables[0];
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

        public DataTable SuppCityDAL(string GroupName)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                     objProvider.CreateInitializedParameter("@CityName", DbType.String, GroupName),
                };
                DataTable searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$SuppCity$detail_GetSuppCityList]", prmContentGetDetails).Tables[0];
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
        public Dictionary<string, string> OSCityList(string GroupName)
        {
            Dictionary<string, string> ddlSuppCityDal = new Dictionary<string, string>();
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetils =
                {
                        objProvider.CreateInitializedParameter("@CityName", DbType.String, GroupName),
                    };
                DataSet OSCity = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[sp_stp$SuppCity$detail_GetSuppCityList]", prmContentGetDetils);
                DataRow dr;
                dr = OSCity.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                OSCity.Tables[0].Rows.InsertAt(dr, 0);
                if (OSCity.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < OSCity.Tables[0].Rows.Count; i++)
                    {
                        ddlSuppCityDal.Add(OSCity.Tables[0].Rows[i]["city_id"].ToString(), OSCity.Tables[0].Rows[i]["city_name"].ToString());
                    }
                }
                return ddlSuppCityDal;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetsuppDSCntrDAL(string SuppCity)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@SuppCity",DbType.String,SuppCity),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Supp$DisStatCntr_GetsuppDisStatCntr", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public string InsertOS_Data(DataTable ODDetail, DataTable ODQuantity,string LandlineNumber,DataTable ORGAddressDetail,DataTable LicenceDetail)
        {

            try
            {
                SqlDataProvider objprovider = new SqlDataProvider();
                SqlParameter[] prmcontentaddupdate = {

                 objprovider.CreateInitializedParameterTableType("@OSHeaderDetails",SqlDbType.Structured, ODDetail ),
                 objprovider.CreateInitializedParameterTableType("@OSQuantity",SqlDbType.Structured, ODQuantity ),
                 objprovider.CreateInitializedParameterTableType("@DocNo",SqlDbType.NVarChar,""),
                 objprovider.CreateInitializedParameter("@LandlineNumber",DbType.String,LandlineNumber),
                 objprovider.CreateInitializedParameterTableType("@ORGAddressDetail",SqlDbType.Structured, ORGAddressDetail ),
                 objprovider.CreateInitializedParameterTableType("@LicenceDetail",SqlDbType.Structured, LicenceDetail ),
                 
                };
                prmcontentaddupdate[2].Size = 100;
                prmcontentaddupdate[2].Direction = ParameterDirection.Output;

                string dn_no = SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "InsertOS_Data", prmcontentaddupdate).ToString();

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


        }

        public DataSet GetviewCOM(int com_ID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    //objProvider.CreateInitializedParameter("@EntityName",DbType.String, EntityName),
                     //objProvider.CreateInitializedParameter("@comp_id",DbType.String, CompID),
                               objProvider.CreateInitializedParameter("@comp",DbType.Int32, com_ID),
                                                     };
                DataSet GetviewSuppdetail = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[OSCOMP_DETAILES]", prmContentGetDetails);
                return GetviewSuppdetail;

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
        public DataSet GetAllHoBranchGroup(OrganizationSetupModel _OrganizationSetupModel)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {
                                                        //objProvider.CreateInitializedParameter("@CompID",DbType.String, _OrganizationSetupModel.Comp_ID),
                                                      };
                DataSet searchmenu = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "OSHeadBranchtree", prmContentGetDetails);
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
        public JObject GetAllHoBranchGrp(OrganizationSetupModel _OrganizationSetupModel)
        {
            JObject FinalList = new JObject();

            try
            {
                DataSet GetItemList = GetAllHoBranchGroup(_OrganizationSetupModel);

                DataTable PresentNode = new DataTable();
                DataTable ChildNode = new DataTable();

                PresentNode = GetItemList.Tables[0];
                ChildNode = GetItemList.Tables[1];

                ParentNode ParentNod = new ParentNode();
                childrenNode ChildNod = new childrenNode();

                Header Headertree = new Header();

                if (PresentNode.Rows.Count > 0)
                {
                    for (int x = 0; x < PresentNode.Rows.Count; x++)
                    {
                        JObject NewObj = new JObject();
                        var Finaldata = string.Empty;

                        List<childrenNode> NodeChild = new List<childrenNode>();
                        string PNodeName = string.Empty;
                        PNodeName = PresentNode.Rows[x]["Comp_id"].ToString();

                        DataView DV = new DataView(ChildNode);
                        DV.RowFilter = "Ho_comp_id='" + PNodeName + "'";
                        DataTable PreNode = new DataTable();
                        PreNode = DV.ToTable();
                        if (PreNode.Rows.Count > 0)
                        {
                            for (int az = 0; az < PreNode.Rows.Count; az++)
                            {
                                NodeChild.Add(new childrenNode()
                                {
                                    label = PreNode.Rows[az]["comp_nm"].ToString(),
                                    value = PreNode.Rows[az]["Comp_Id"].ToString(),
                                });
                            }
                        }
                        ParentNod.children = NodeChild;
                        ParentNod.label = PresentNode.Rows[x]["comp_nm"].ToString();
                        ParentNod.value = PresentNode.Rows[x]["Comp_Id"].ToString();
                        Headertree.TreeStr = ParentNod;

                        var Fdata = JsonConvert.SerializeObject(Headertree);
                        Finaldata = Fdata;

                        if (!string.IsNullOrEmpty(Finaldata))
                        {
                            if (x != 0)
                            {
                                Finaldata = Finaldata.Replace("TreeStr", "TreeStr" + x.ToString());
                            }
                        }
                        NewObj = JObject.Parse(Finaldata);
                        if (x == 0)
                        {
                            FinalList = NewObj;
                        }
                        else
                        {
                            FinalList.Merge(NewObj);
                        }

                    }

                    //FinalList = "";
                }
                //FinalList = JObject.Parse(Finaldata);
                return FinalList;
            }
            catch (Exception ex)
            {
                JObject Obj = new JObject();
                return Obj;
                //throw;
            }
        }
        public DataSet getst_dt_end_dt(string Headoffice_id)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Headoffice_id",DbType.String,Headoffice_id),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[getfin$strt$dt$end$dt$cuur]", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataCheckDepency(string Entityprefix,string flag, string comp_id, string RoleHoName, string Branchchk)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@Entityprefix",DbType.String,Entityprefix),
                     objProvider.CreateInitializedParameter("@flag",DbType.String,flag),
                     objProvider.CreateInitializedParameter("@comp_id",DbType.String,comp_id),
                     objProvider.CreateInitializedParameter("@RoleHoName",DbType.String,RoleHoName),
                     objProvider.CreateInitializedParameter("@Branchchk",DbType.String,Branchchk),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "[Check$dependcy$entity$perfix]", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        /*----------------------Code Start of Country,state,district,city--------------------------*/
        public DataTable GetCountryListDDL()
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                    //objProvider.CreateInitializedParameter("@CompID",DbType.String,CompID),
                    //objProvider.CreateInitializedParameter("@TransModetype",DbType.String,ProspectType),
                };
                DataTable GetCurronSuppType = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$AllCountryForOrgnisationSetup", prmContentGetDetails).Tables[0];
                return GetCurronSuppType;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetstateOnCountryDDL(string ddlCountryID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@CountryID",DbType.String,ddlCountryID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getState", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetDistrictOnStateDDL(string ddlStateID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@StateID",DbType.String,ddlStateID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getDistrict", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataTable GetCityOnDistrictDDL(string ddlDistrictID)
        {
            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@DistrictID",DbType.String,ddlDistrictID),
                };
                DataTable GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_stp$Trans$getCity", prmContentGetDetails).Tables[0];
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public DataSet GetStateCode(string stateId)
        {

            try
            {
                SqlDataProvider objProvider = new SqlDataProvider();
                SqlParameter[] prmContentGetDetails = {/*Passing perameter to sotore procedure*/   
                     objProvider.CreateInitializedParameter("@StateId",DbType.String,stateId),
                };
                DataSet GetsuppDSCntr = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_GetStateCode", prmContentGetDetails);
                return GetsuppDSCntr;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        /*----------------------Code End of Country,state,district,city--------------------------*/
    }
}
