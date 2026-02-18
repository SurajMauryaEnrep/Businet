using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.MODELS.BusinessLayer.SupplierDetail;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
//***Modifyed by Shubham Maurya on 06-01-2023 for Remove Session ***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class SupplierDetailController : Controller
    { 
        string comp_id, userid, SuppId = String.Empty;
        string CompID, BrchID, language, Comp_ID = String.Empty;
        
        string DocumentMenuId = "103135",title;
        
        Common_IServices _Common_IServices;
        SupplierDetail _SupplierDetail;
        SearchSupp queryParameters;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        //SupplierDetail _SupplierDetailModel;
        SupplierDetail_ISERVICES _SupplierDetail_ISERVICES;
        // GET: BusinessLayer/SupplierDetail
        public SupplierDetailController(Common_IServices _Common_IServices,SupplierDetail_ISERVICES _SupplierDetail_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._SupplierDetail_ISERVICES = _SupplierDetail_ISERVICES;
        }
        public ActionResult SupplierDetail(SupplierDetail _SupplierDetail1, string SuppCodeURL,string TransType,string BtnName,string command)
        {
            try
            {
                var _SupplierDetail = TempData["ModelData"] as SupplierDetail;
                if (_SupplierDetail != null)
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _SupplierDetail.AttachMentDetailItmStp = null;
                    //Session["Guid"] = null;
                    _SupplierDetail.Guid = null;
                    //ViewBag.MenuPageName = getDocumentName();
                    CommonPageDetails();
                    //_SupplierDetail = new SupplierDetail();
                    _SupplierDetail.Title = title;
                    //GetAutoCompleteSuppcity(_SupplierDetail);
                    string Comp_ID = string.Empty;
                    _SupplierDetail.supp_id = null;
                    var suppCode = _SupplierDetail.SuppCode;
                  

                    //dt = GetAccountGroup("supp");
                    //List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                    //foreach (DataRow dt in dt.Rows)
                    //{
                    //    AccountGroup _AccountGroup = new AccountGroup();
                    //    _AccountGroup.acc_grp_id = dt["acc_grp_id"].ToString();
                    //    _AccountGroup.AccGroupChildNood = dt["AccGroupChildNood"].ToString();
                    //    _AccountGroupList.Add(_AccountGroup);
                    //}
                    //_AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                    //_SupplierDetail.AccountGroupList = _AccountGroupList;

                    //List<SuppCategory> _CategoryList = new List<SuppCategory>();
                    //dt = Getcategory();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    SuppCategory _Category = new SuppCategory();
                    //    _Category.setup_id = dr["setup_id"].ToString();
                    //    _Category.setup_val = dr["setup_val"].ToString();
                    //    _CategoryList.Add(_Category);
                    //}
                    //_CategoryList.Insert(0, new SuppCategory() { setup_id = "0", setup_val = "---Select---" });
                    //_SupplierDetail.CategoryList = _CategoryList;
                    //List<SuppPortFolio> _PortFolioList = new List<SuppPortFolio>();
                    //dt = GetSuppport();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    SuppPortFolio _PortFolio = new SuppPortFolio();
                    //    _PortFolio.setup_id = dr["setup_id"].ToString();
                    //    _PortFolio.setup_val = dr["setup_val"].ToString();
                    //    _PortFolioList.Add(_PortFolio);
                    //}
                    //_PortFolioList.Insert(0, new SuppPortFolio() { setup_id = "0", setup_val = "---Select---" });
                    //_SupplierDetail.PortFolioList = _PortFolioList;

                    //List<curr> _SuppcurrList = new List<curr>();
                    //dt = Getcurr("D");
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    curr _Suppcurr = new curr();
                    //    _Suppcurr.curr_id = dr["curr_id"].ToString();
                    //    _Suppcurr.curr_name = dr["curr_name"].ToString();
                    //    _SuppcurrList.Add(_Suppcurr);

                    //}
                    ////_SuppcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });

                    //_SupplierDetail.currList = _SuppcurrList;

                    GetAllDropDownList(_SupplierDetail);

                    //dt = GetBrList();
                    //List<SupplierBranch> _CustomerBranchList = new List<SupplierBranch>();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    SupplierBranch _SupplierBranch = new SupplierBranch();
                    //    _SupplierBranch.comp_id = dr["comp_id"].ToString();
                    //    _SupplierBranch.comp_nm = dr["comp_nm"].ToString();
                    //    _CustomerBranchList.Add(_SupplierBranch);
                    //}

                    /*Commented bY Hina on 05-01-2024 14:04 to change country,state,district,city in dropdown instead of textbox */

                    //List<SuppCity> suppCities = new List<SuppCity>();
                    //suppCities.Insert(0, new SuppCity() { city_id = "0", city_name = "---Select---" });
                    //_SupplierDetail.SuppCityList = suppCities;

                    /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                    List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    string SupplierType = "";
                    if (_SupplierDetail.supp_type == null)
                    {
                        SupplierType = "D";
                    }
                    else
                    {
                        SupplierType = _SupplierDetail.supp_type;
                        _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                        
                    }
                    //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    List<Country> _ContryList2 = new List<Country>();
                    CommonAddress_Detail _Model = new CommonAddress_Detail();
                    //string SupplierType = "D";
                    //if (SupplierType != "D")
                    //{
                    //    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                    //}
                    //if (!string.IsNullOrEmpty(_SupplierDetail.supp_type))
                    //    SupplierType = _SupplierDetail.supp_type;
                    

                    DataTable dtcntry = GetCountryList(SupplierType);

                    foreach (DataRow dr in dtcntry.Rows)
                    {
                        CmnCountryList _Contry = new CmnCountryList();
                        _Contry.country_id = dr["country_id"].ToString();
                        _Contry.country_name = dr["country_name"].ToString();
                        _ContryList.Add(_Contry);
                        _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _Model.countryList = _ContryList2;
                    _SupplierDetail.countryList = _ContryList;

                    List<CmnStateList> state = new List<CmnStateList>();
                    state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountry = "";
                    if (!string.IsNullOrEmpty(_SupplierDetail.Country))
                        transCountry = _SupplierDetail.Country;
                    else
                        transCountry = dtcntry.Rows[0]["country_id"].ToString();

                    DataTable dtStates = _SupplierDetail_ISERVICES.GetstateOnCountryDDL(transCountry);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    _SupplierDetail.StateList = state;

                    string transState = "0";
                    List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                    DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(_SupplierDetail.State))
                        transState = _SupplierDetail.State;
                    else
                        transState = "0";
                    DataTable dtDist = _SupplierDetail_ISERVICES.GetDistrictOnStateDDL(transState);
                    if (dtDist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDist.Rows)
                        {
                            DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    _SupplierDetail.DistrictList = DistList;
                   // _SupplierDetail.District = "0";

                    string transDist = "0";
                    if (!string.IsNullOrEmpty(_SupplierDetail.District))
                        transDist = _SupplierDetail.District;
                    else
                        transDist = "0";
                    DataTable dtCities = _SupplierDetail_ISERVICES.GetCityOnDistrictDDL(transDist);

                    List<CmnCityList> cities = new List<CmnCityList>();
                    cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dtCities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCities.Rows)
                        {
                            cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    _SupplierDetail.cityLists = cities;
                    //_SupplierDetail.City = "0";
                    _SupplierDetail._CommonAddress_Detail = _Model;
      /*------------------------------------------Code End of Country,state,district,city--------------------------*/
                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }
                    //Session["SupplierFromProspect"] = "N";
                    _SupplierDetail.SupplierFromProspect = "N";
                    //Session["Prospect_id"] = "0";
                    _SupplierDetail.Prospect_id = "0";
                    Session["brid"] = "0";
                    //_SupplierDetail.brid = "0";

                    //if (Session["ProspectDetail"] != null)
                    if (TempData["ProspectDetail"] != null)
                    {
                        DataSet prospectds = new DataSet();
                        //prospectds = (DataSet)_SupplierDetail.ProspectDetail;
                        prospectds = (DataSet)TempData["ProspectDetail"];
                        _SupplierDetail.supp_name = prospectds.Tables[0].Rows[0]["pros_name"].ToString();
                        _SupplierDetail.supp_type = prospectds.Tables[0].Rows[0]["pros_type"].ToString();

       /*------------------------------------------Code Start of Country,state,district,city--------------------------*/

                        List<CmnCountryList> _ContryListP = new List<CmnCountryList>();
                        //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                        List<Country> _ContryList2P = new List<Country>();
                        CommonAddress_Detail _ModelP = new CommonAddress_Detail();
                        string SupplierTypeP = "D";
                        if (SupplierTypeP != "D")
                        {
                            _ContryListP.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                        }
                        if (!string.IsNullOrEmpty(_SupplierDetail.supp_type))
                            SupplierTypeP = _SupplierDetail.supp_type;

                        dt = GetCountryList(SupplierTypeP);

                        foreach (DataRow dr in dt.Rows)
                        {
                            CmnCountryList _ContryP = new CmnCountryList();
                            _ContryP.country_id = dr["country_id"].ToString();
                            _ContryP.country_name = dr["country_name"].ToString();
                            _ContryListP.Add(_ContryP);
                            _ContryList2P.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                        }
                        //_Model.countryList = _ContryList2P;
                        _SupplierDetail.countryList = _ContryListP;

                        List<CmnStateList> stateP = new List<CmnStateList>();
                        //stateP.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                        string transCountryP = "";
                        if (!string.IsNullOrEmpty(_SupplierDetail.Country))
                            transCountryP = _SupplierDetail.Country;
                        else
                            transCountryP = dtcntry.Rows[0]["country_id"].ToString();

                        dt = _SupplierDetail_ISERVICES.GetstateOnCountryDDL(transCountryP);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                stateP.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                            }
                        }
                        _SupplierDetail.StateList = stateP;

                        string transStateP = "0";
                        List<CmnDistrictList> DistListP = new List<CmnDistrictList>();
                        //DistListP.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                        if (!string.IsNullOrEmpty(_SupplierDetail.State))
                            transStateP = _SupplierDetail.State;
                        else
                            transStateP = "0";
                         dt = _SupplierDetail_ISERVICES.GetDistrictOnStateDDL(transStateP);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                DistListP.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                            }
                        }
                        _SupplierDetail.DistrictList = DistListP;

                        string transDistP = "0";
                        if (!string.IsNullOrEmpty(_SupplierDetail.District))
                            transDistP = _SupplierDetail.District;
                        else
                            transDistP = "0";
                         dt = _SupplierDetail_ISERVICES.GetCityOnDistrictDDL(transDistP);

                        List<CmnCityList> citiesP = new List<CmnCityList>();
                        //citiesP.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                citiesP.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                            }
                        }
                        _SupplierDetail.cityLists = citiesP;

                        _SupplierDetail._CommonAddress_Detail = _ModelP;
      /*------------------------------------------Code End of Country,state,district,city--------------------------*/

                        /*Commented bY Hina on 08-01-2024 10:37 to change country,state,district,city in dropdown instead of textbox */

                        //foreach (DataRow dr in prospectds.Tables[2].Rows)
                        //{
                        //    SuppCity suppCity = new SuppCity();
                        //    suppCity.city_id = dr["city_id"].ToString();
                        //    suppCity.city_name = dr["city_name"].ToString();
                        //    suppCities.Add(suppCity);
                        //}
                        //_SupplierDetail.SuppCityList = suppCities;


                        dt = Getcurr(prospectds.Tables[0].Rows[0]["pros_type"].ToString());
                        DataView dv = new DataView(dt);
                        List<curr> _SuppcurrList1 = new List<curr>();
                        foreach (DataRowView dr in dv)
                        {
                            curr _Suppcurr = new curr();
                            _Suppcurr.curr_id = dr["curr_id"].ToString();
                            _Suppcurr.curr_name = dr["curr_name"].ToString();
                            _SuppcurrList1.Add(_Suppcurr);

                        }

                        _SupplierDetail.currList = _SuppcurrList1;
                        _SupplierDetail.curr = int.Parse(prospectds.Tables[0].Rows[0]["curr_id"].ToString());
                        _SupplierDetail.cont_pers = prospectds.Tables[0].Rows[0]["cont_person"].ToString();
                        _SupplierDetail.cont_email = prospectds.Tables[0].Rows[0]["cont_email"].ToString();
                        _SupplierDetail.cont_num1 = prospectds.Tables[0].Rows[0]["cont_num"].ToString();


                        ViewBag.ProsSupplierAddressDetail = prospectds.Tables[0];
                        //ViewBag.CustomerBranchList = prospectds.Tables[3];
                        _SupplierDetail.CustomerBranchList = prospectds.Tables[3];

                        //Session["SupplierFromProspect"] = "Y";
                        _SupplierDetail.SupplierFromProspect = "Y";
                        //Session["Prospect_id"] = prospectds.Tables[0].Rows[0]["pros_id"].ToString();
                        _SupplierDetail.Prospect_id = prospectds.Tables[0].Rows[0]["pros_id"].ToString();
                        //Session["brid"] = prospectds.Tables[0].Rows[0]["br_id"].ToString();
                        //_SupplierDetail.brid = prospectds.Tables[0].Rows[0]["br_id"].ToString();
                        Session["brid"] = prospectds.Tables[0].Rows[0]["br_id"].ToString();

                        //Session["ProspectDetail"] = null;
                        TempData["ProspectDetail"] = null;
                    }
                    if (TempData["ListFilterData"] != null)
                    {
                        _SupplierDetail.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    _SupplierDetail.TDSApplicableOn = "B";
                    //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                    if (_SupplierDetail.TransType == "Update" || _SupplierDetail.Command == "Edit")
                    {
                        string Br_Id = string.Empty;
                        //string SuppCode = Session["SuppCode"].ToString();
                        string SuppCode = _SupplierDetail.SuppCode;
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = Session["CompId"].ToString();
                        }
                        if (Session["BranchId"] != null)
                        {
                            Br_Id = Session["BranchId"].ToString();
                        }
                        Boolean act_status, tds_posting, on_hold = true;

                        DataSet ds = _SupplierDetail_ISERVICES.GetviewSuppdetailDAL(SuppCode, Comp_ID, Br_Id);
                        ViewBag.AttechmentDetails = ds.Tables[6];
                        if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                            act_status = true;
                        else
                            act_status = false;

                        if (ds.Tables[0].Rows[0]["on_hold"].ToString() == "Y")
                            on_hold = true;
                        else
                            on_hold = false;
                        if (ds.Tables[0].Rows[0]["tds_posting"].ToString() == "Y")
                            tds_posting = true;
                        else
                            tds_posting = false;
                        ViewBag.PurchaseHistory = ds.Tables[10];
                        ViewBag.licenseDetails = ds.Tables[11];
                        _SupplierDetail.SupplierDependcy = ds.Tables[9].Rows[0]["SupplierDependcy"].ToString();
                        _SupplierDetail.supp_id = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        _SupplierDetail.suppname = ds.Tables[0].Rows[0]["supp_name"].ToString();
                        _SupplierDetail.supp_name = ds.Tables[0].Rows[0]["supp_name"].ToString();
                        _SupplierDetail.supp_alias = ds.Tables[0].Rows[0]["supp_alias"].ToString();
                        _SupplierDetail.supp_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                        _SupplierDetail.TDSApplicableOn = ds.Tables[0].Rows[0]["tds_app_on"].ToString();
                        string Sup_coa_name = ds.Tables[0].Rows[0]["acc_name"].ToString();
                        _SupplierDetail.supp_type = ds.Tables[0].Rows[0]["supptype"].ToString();
                        _SupplierDetail.PaymentAlert = Convert.ToInt32(ds.Tables[0].Rows[0]["paym_alrt"].ToString());
                        if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                        {

                        }
                        else
                        {
                            _SupplierDetail.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        }
                        _SupplierDetail.GlRepoting_Group_ID = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                        _SupplierDetail.GlRepoting_Group_Name = ds.Tables[0].Rows[0]["gl_rpt_des"].ToString();
                        _SupplierDetail.GlRepoting_Group = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();

                        if (_SupplierDetail.GlRepoting_Group_ID != "0")
                        {
                            List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                            GlReportingGroup Glrpt = new GlReportingGroup();
                            Glrpt.Gl_rpt_id = _SupplierDetail.GlRepoting_Group_ID;
                            Glrpt.Gl_rpt_Name = _SupplierDetail.GlRepoting_Group_Name;
                            glrpt.Add(Glrpt);

                            _SupplierDetail.GlReportingGroupList = glrpt;
                        }
                        dt = Getcurr(ds.Tables[0].Rows[0]["supptype"].ToString());
                        DataView dv = new DataView(dt);
                        List<curr> _SuppcurrList1 = new List<curr>();
                        foreach (DataRowView dr in dv)
                        {
                            curr _Suppcurr = new curr();
                            _Suppcurr.curr_id = dr["curr_id"].ToString();
                            _Suppcurr.curr_name = dr["curr_name"].ToString();
                            _SuppcurrList1.Add(_Suppcurr);

                        }
                        _SupplierDetail.currList = _SuppcurrList1;

                        //List<SuppCoa> _ListSuppCoa1 = new List<SuppCoa>();
                        //suppCode = _SupplierDetail.SuppCode;
                        //dt = GetSuppCoa(suppCode, "O");
                        //foreach (DataRow dr in dt.Rows)
                        //{
                        //    SuppCoa _SuppCoa = new SuppCoa();
                        //    _SuppCoa.acc_id = dr["acc_id"].ToString();
                        //    _SuppCoa.acc_name = dr["acc_name"].ToString();
                        //    _ListSuppCoa1.Add(_SuppCoa);
                        //}
                        //_ListSuppCoa1.Insert(0, new SuppCoa() { acc_id = "0", acc_name = "---Select---" });
                        ////_ListSuppCoa1.Add(new SuppCoa { acc_id = _SupplierDetail.supp_coa.ToString(), acc_name = Sup_coa_name });
                        //_SupplierDetail.SuppCoaNameList = _ListSuppCoa1;
                        //_SupplierDetail.supp_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                        _SupplierDetail.curr = int.Parse(ds.Tables[0].Rows[0]["CurrID"].ToString());
                        _SupplierDetail.supp_catg = int.Parse(ds.Tables[0].Rows[0]["CatID"].ToString());
                        _SupplierDetail.supp_port = int.Parse(ds.Tables[0].Rows[0]["PortID"].ToString());

                        _SupplierDetail.supp_regn_no = ds.Tables[0].Rows[0]["supp_regn_no"].ToString();
                        _SupplierDetail.supp_gst_no = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                        _SupplierDetail.supp_tan_no = ds.Tables[0].Rows[0]["supp_tan_no"].ToString();
                        _SupplierDetail.supp_pan_no = ds.Tables[0].Rows[0]["supp_pan_no"].ToString();
                        _SupplierDetail.cont_pers = ds.Tables[0].Rows[0]["cont_pers"].ToString();
                        _SupplierDetail.cont_email = ds.Tables[0].Rows[0]["cont_email"].ToString();
                        _SupplierDetail.cont_num1 = ds.Tables[0].Rows[0]["cont_num1"].ToString();
                        _SupplierDetail.cont_num2 = ds.Tables[0].Rows[0]["cont_num2"].ToString();
                        _SupplierDetail.supp_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                        _SupplierDetail.act_status = act_status;
                        _SupplierDetail.on_hold = on_hold;
                        _SupplierDetail.TDSApplicable = tds_posting;
                        _SupplierDetail.inact_reason = ds.Tables[0].Rows[0]["inact_reason"].ToString();
                        _SupplierDetail.onhold_reason = ds.Tables[0].Rows[0]["onhold_reason"].ToString();
                        _SupplierDetail.supp_rmarks = ds.Tables[0].Rows[0]["supp_remarks"].ToString();
                        _SupplierDetail.paym_term = int.Parse(ds.Tables[0].Rows[0]["paym_term"].ToString());
                        _SupplierDetail.create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _SupplierDetail.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _SupplierDetail.app_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _SupplierDetail.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _SupplierDetail.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _SupplierDetail.app_dt = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _SupplierDetail.app_status = ds.Tables[0].Rows[0]["appstatus"].ToString();
                        _SupplierDetail.bank_name = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        _SupplierDetail.bank_branch = ds.Tables[0].Rows[0]["bank_branch"].ToString();
                        _SupplierDetail.bank_add = ds.Tables[0].Rows[0]["bank_add"].ToString();
                        _SupplierDetail.bank_acc_no = ds.Tables[0].Rows[0]["bank_acc_no"].ToString();
                        _SupplierDetail.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                        _SupplierDetail.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                        _SupplierDetail.Gst_Cat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                        _SupplierDetail.GLAccountName = ds.Tables[0].Rows[0]["GLAcc_Name"].ToString();
                        _SupplierDetail.HdnInterBranch = ds.Tables[0].Rows[0]["inter_br"].ToString();
                        if(_SupplierDetail.HdnInterBranch =="Y")
                        {
                            _SupplierDetail.InterBranch = true;
                        }
                        else
                        {
                            _SupplierDetail.InterBranch = false;
                        }
                        //ViewBag.CustomerBranchList = ds.Tables[1];
                        _SupplierDetail.CustomerBranchList = ds.Tables[1];
                        ViewBag.SupplierDocumentDetail = ds.Tables[2];
                        ViewBag.SupplierAddressDetail = ds.Tables[3];
                        ViewBag.GLCount = ds.Tables[5].Rows[0]["GlCount"].ToString();
                        _SupplierDetail.D_InActive = ds.Tables[7].Rows[0]["disableInActive"].ToString();
                        if (_SupplierDetail.app_status == "Approved")
                        {
                            if (ds.Tables[8].Rows[0]["disenbcurr"].ToString() != null && ds.Tables[8].Rows[0]["disenbcurr"].ToString() != "")
                            {
                                _SupplierDetail.curr_depncy = ds.Tables[8].Rows[0]["disenbcurr"].ToString();
                            }
                            else
                            {
                                _SupplierDetail.curr_depncy = null;
                            }
                        }
                        /*Commented bY Hina on 08-01-2024 10:37 to change country,state,district,city in dropdown instead of textbox */

                        //foreach (DataRow dr in ds.Tables[4].Rows)
                        //{
                        //    SuppCity suppCity = new SuppCity();
                        //    suppCity.city_id = dr["city_id"].ToString();
                        //    suppCity.city_name = dr["city_name"].ToString();
                        //    suppCities.Add(suppCity);
                        //}
                        //_SupplierDetail.SuppCityList = suppCities;

                        if (_SupplierDetail.app_status == "Approved")
                        {
                            //Session["AppStatus"] = 'A';
                            _SupplierDetail.AppStatus = "A";
                        }
                        else
                        {
                            //Session["AppStatus"] = 'D';
                            _SupplierDetail.AppStatus = "D";
                        }
                    }
                    else
                    {
                        _SupplierDetail.act_status = true;
                    }
    /*------------------------------------------Code Start of Country--------------------------*/

                    List<CmnCountryList> _ContryListU = new List<CmnCountryList>();
                    string SupplierTypeU = "";
                    if (_SupplierDetail.supp_type == null|| _SupplierDetail.supp_type =="D")
                    {   SupplierTypeU = "D";
                    }
                    else
                    {
                        SupplierTypeU = _SupplierDetail.supp_type;
                        _ContryListU.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    }
                    //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    List<Country> _ContryList2U = new List<Country>();
                    CommonAddress_Detail _ModelU = new CommonAddress_Detail();
                    

                    dt = GetCountryList(SupplierTypeU);

                    foreach (DataRow dr in dt.Rows)
                    {
                        CmnCountryList _Contry = new CmnCountryList();
                        _Contry.country_id = dr["country_id"].ToString();
                        _Contry.country_name = dr["country_name"].ToString();
                        _ContryListU.Add(_Contry);
                        _ContryList2U.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _ModelU.countryList = _ContryList2U;
                    _SupplierDetail.countryList = _ContryListU;
      /*------------------------------------------Code End of Country--------------------------*/

                    //_SupplierDetail.TransType = Session["TransType"].ToString();
                    _SupplierDetail.TransType = _SupplierDetail.TransType;
                    _SupplierDetail.DocumentMenuId = DocumentMenuId;
                    //ViewBag.VBRoleList = GetRoleList();
                  
                    return View("~/Areas/BusinessLayer/Views/SupplierSetup/SupplierDetail.cshtml", _SupplierDetail);
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _SupplierDetail1.AttachMentDetailItmStp = null;
                    //Session["Guid"] = null;
                    _SupplierDetail1.Guid = null;
                    //ViewBag.MenuPageName = getDocumentName();
                    CommonPageDetails();
                    //_SupplierDetail = new SupplierDetail();
                    _SupplierDetail1.Title = title;
                    //GetAutoCompleteSuppcity(_SupplierDetail);
                    string Comp_ID = string.Empty;
                    _SupplierDetail1.supp_id = null;

                    //dt = GetSupplierBranchList();
                    //_SupplierDetail1.CustomerBranchList = dt;
                    var suppCode = "";
                    if (SuppCodeURL != null)
                    {
                         suppCode = SuppCodeURL;
                    }
                    else
                    {
                          suppCode = _SupplierDetail1.SuppCode;
                    }
                    //List<SuppCoa> _ListSuppCoa = new List<SuppCoa>();
                    //dt = GetSuppCoa(suppCode, "N");
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    SuppCoa _SuppCoa = new SuppCoa();
                    //    _SuppCoa.acc_id = dr["acc_id"].ToString();
                    //    _SuppCoa.acc_name = dr["acc_name"].ToString();
                    //    _ListSuppCoa.Add(_SuppCoa);
                    //}
                    //_ListSuppCoa.Insert(0, new SuppCoa() { acc_id = "0", acc_name = "---Select---" });
                    //_SupplierDetail1.SuppCoaNameList = _ListSuppCoa;



                    GetAllDropDownList(_SupplierDetail1);




                    //dt = GetBrList();
                    //List<SupplierBranch> _CustomerBranchList = new List<SupplierBranch>();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    SupplierBranch _SupplierBranch = new SupplierBranch();
                    //    _SupplierBranch.comp_id = dr["comp_id"].ToString();
                    //    _SupplierBranch.comp_nm = dr["comp_nm"].ToString();
                    //    _CustomerBranchList.Add(_SupplierBranch);
                    //}

                    /*Commented bY Hina on 08-01-2024 10:37 to change country,state,district,city in dropdown instead of textbox */

                    //List<SuppCity> suppCities = new List<SuppCity>();
                    //suppCities.Insert(0, new SuppCity() { city_id = "0", city_name = "---Select---" });
                    //_SupplierDetail1.SuppCityList = suppCities;


                    string Language = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["Language"] != null)
                    {
                        Language = Session["Language"].ToString();
                    }


                    //Session["SupplierFromProspect"] = "N";
                    _SupplierDetail1.SupplierFromProspect = "N";
                    //Session["Prospect_id"] = "0";
                    Session["brid"] = "0";
                    _SupplierDetail1.Prospect_id = "0";
                    //_SupplierDetail1.brid = "0";

                    //if (Session["ProspectDetail"] != null)
                    if (TempData["ProspectDetail"] != null)
                    {
                        DataSet prospectds = new DataSet();
                        //prospectds = (DataSet)_SupplierDetail1.ProspectDetail;
                        prospectds = (DataSet)TempData["ProspectDetail"];
                        _SupplierDetail1.supp_name = prospectds.Tables[0].Rows[0]["pros_name"].ToString();
                        if (prospectds.Tables[0].Rows[0]["pros_type"].ToString() == "E")
                        {
                            _SupplierDetail1.supp_type = "I";
                        }
                        else
                        {
                            _SupplierDetail1.supp_type = prospectds.Tables[0].Rows[0]["pros_type"].ToString();
                        }
                   /*Commented bY Hina on 08-01-2024 10:37 to change country,state,district,city in dropdown instead of textbox */

                        //foreach (DataRow dr in prospectds.Tables[2].Rows)
                        //{
                        //    SuppCity suppCity = new SuppCity();
                        //    suppCity.city_id = dr["city_id"].ToString();
                        //    suppCity.city_name = dr["city_name"].ToString();
                        //    suppCities.Add(suppCity);
                        //}
                        //_SupplierDetail1.SuppCityList = suppCities;

                        /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                        List<CmnCountryList> _ContryListP = new List<CmnCountryList>();
                        //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                        List<Country> _ContryList2P = new List<Country>();
                        CommonAddress_Detail _ModelP = new CommonAddress_Detail();
                        string SupplierTypeP = "D";
                        if (SupplierTypeP != "D")
                        {
                            _ContryListP.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                        }
                        if (!string.IsNullOrEmpty(_SupplierDetail1.supp_type))
                            SupplierTypeP = _SupplierDetail1.supp_type;

                         dt = GetCountryList(SupplierTypeP);

                        foreach (DataRow dr in dt.Rows)
                        {
                            CmnCountryList _Contry = new CmnCountryList();
                            _Contry.country_id = dr["country_id"].ToString();
                            _Contry.country_name = dr["country_name"].ToString();
                            _ContryListP.Add(_Contry);
                            _ContryList2P.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                        }
                        //_Model.countryList = _ContryList2P;
                        _SupplierDetail1.countryList = _ContryListP;

                        List<CmnStateList> stateP = new List<CmnStateList>();
                        //stateP.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                        string transCountryP = "";
                        if (!string.IsNullOrEmpty(_SupplierDetail1.Country))
                            transCountryP = _SupplierDetail1.Country;
                        else
                            transCountryP = dt.Rows[0]["country_id"].ToString();

                         dt = _SupplierDetail_ISERVICES.GetstateOnCountryDDL(transCountryP);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                stateP.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                            }
                        }
                        _SupplierDetail1.StateList = stateP;

                        string transStateP = "0";
                        List<CmnDistrictList> DistListP = new List<CmnDistrictList>();
                        //DistListP.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                        if (!string.IsNullOrEmpty(_SupplierDetail1.State))
                            transStateP = _SupplierDetail1.State;
                        else
                            transStateP = "0";
                        dt = _SupplierDetail_ISERVICES.GetDistrictOnStateDDL(transStateP);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                DistListP.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                            }
                        }
                        _SupplierDetail1.DistrictList = DistListP;
                        //_SupplierDetail1.District = "0";
                        string transDistP = "0";
                        if (!string.IsNullOrEmpty(_SupplierDetail1.District))
                            transDistP = _SupplierDetail1.District;
                        else
                            transDistP = "0";
                         dt = _SupplierDetail_ISERVICES.GetCityOnDistrictDDL(transDistP);

                        List<CmnCityList> citiesP = new List<CmnCityList>();
                        //citiesP.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                citiesP.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                            }
                        }
                        _SupplierDetail1.cityLists = citiesP;
                        //_SupplierDetail1.City = "0";
                        _SupplierDetail1._CommonAddress_Detail = _ModelP;
                        /*------------------------------------------Code End of Country,state,district,city--------------------------*/

                        dt = Getcurr(prospectds.Tables[0].Rows[0]["pros_type"].ToString());
                        DataView dv = new DataView(dt);
                        List<curr> _SuppcurrList1 = new List<curr>();
                        foreach (DataRowView dr in dv)
                        {
                            curr _Suppcurr = new curr();
                            _Suppcurr.curr_id = dr["curr_id"].ToString();
                            _Suppcurr.curr_name = dr["curr_name"].ToString();
                            _SuppcurrList1.Add(_Suppcurr);
                        }
                        _SupplierDetail1.currList = _SuppcurrList1;
                        
                        _SupplierDetail1.curr = int.Parse(prospectds.Tables[0].Rows[0]["curr_id"].ToString());
                        _SupplierDetail1.cont_pers = prospectds.Tables[0].Rows[0]["cont_person"].ToString();
                        _SupplierDetail1.cont_email = prospectds.Tables[0].Rows[0]["cont_email"].ToString();
                        _SupplierDetail1.cont_num1 = prospectds.Tables[0].Rows[0]["cont_num"].ToString();
                        _SupplierDetail1.Gst_Cat = prospectds.Tables[0].Rows[0]["gst_cat"].ToString();

                        ViewBag.ProsSupplierAddressDetail = prospectds.Tables[0];
                        //ViewBag.CustomerBranchList = prospectds.Tables[3];
                        _SupplierDetail1.CustomerBranchList = prospectds.Tables[3];

                        //Session["SupplierFromProspect"] = "Y";
                        _SupplierDetail1.SupplierFromProspect = "Y";
                        //Session["Prospect_id"] = prospectds.Tables[0].Rows[0]["pros_id"].ToString();
                        //Session["brid"] = prospectds.Tables[0].Rows[0]["br_id"].ToString();
                        _SupplierDetail1.Prospect_id = prospectds.Tables[0].Rows[0]["pros_id"].ToString();
                        //_SupplierDetail1.brid = prospectds.Tables[0].Rows[0]["br_id"].ToString();
                        Session["brid"] = prospectds.Tables[0].Rows[0]["br_id"].ToString();
                        //Session["ProspectDetail"] = null;
                        TempData["ProspectDetail"] = null;
                    }
                    if (TransType != null)
                    {
                        _SupplierDetail1.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _SupplierDetail1.Command = command;
                    }
   /*------------------------------------------Code Start of Country,state,district,city--------------------------*/

                    List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    string SupplierType = "";
                    if (_SupplierDetail1.supp_type == null)
                    {
                        //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                        SupplierType = "D";
                    }
                    else
                    {
                        SupplierType = _SupplierDetail1.supp_type;
                        _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                    }
                    List<Country> _ContryList2 = new List<Country>();
                    CommonAddress_Detail _Model = new CommonAddress_Detail();
                     //SupplierType = _SupplierDetail1.supp_type;

                    
                    DataTable dtcntry = GetCountryList(SupplierType);

                    foreach (DataRow dr in dtcntry.Rows)
                    {
                        CmnCountryList _Contry = new CmnCountryList();
                        _Contry.country_id = dr["country_id"].ToString();
                        _Contry.country_name = dr["country_name"].ToString();
                        _ContryList.Add(_Contry);
                        _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _Model.countryList = _ContryList2;
                    _SupplierDetail1.countryList = _ContryList;

                    List<CmnStateList> state = new List<CmnStateList>();
                    state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountry = "";
                    if (!string.IsNullOrEmpty(_SupplierDetail1.Country))
                        transCountry = _SupplierDetail1.Country;
                    else
                        transCountry = dtcntry.Rows[0]["country_id"].ToString();

                    DataTable dtStates = _SupplierDetail_ISERVICES.GetstateOnCountryDDL(transCountry);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    _SupplierDetail1.StateList = state;

                    string transState = "0";
                    List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                    DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(_SupplierDetail1.State))
                        transState = _SupplierDetail1.State;
                    else
                        transState = "0";
                    DataTable dtDist = _SupplierDetail_ISERVICES.GetDistrictOnStateDDL(transState);
                    if (dtDist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDist.Rows)
                        {
                            DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    _SupplierDetail1.DistrictList = DistList;

                    string transDist = "0";
                    if (!string.IsNullOrEmpty(_SupplierDetail1.District))
                        transDist = _SupplierDetail1.District;
                    else
                        transDist = "0";
                    DataTable dtCities = _SupplierDetail_ISERVICES.GetCityOnDistrictDDL(transDist);

                    List<CmnCityList> cities = new List<CmnCityList>();
                    cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dtCities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCities.Rows)
                        {
                            cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    _SupplierDetail1.cityLists = cities;

                    _SupplierDetail1._CommonAddress_Detail = _Model;
                    /*------------------------------------------Code End of Country,state,district,city--------------------------*/
                    _SupplierDetail1.TDSApplicableOn = "B";
                    //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                    if (_SupplierDetail1.TransType == "Update" || _SupplierDetail1.Command == "Edit")
                    {
                        //string SuppCode = Session["SuppCode"].ToString();
                        string SuppCode = "";
                        string Br_Id = "";
                        if (SuppCodeURL != null)
                        {
                            SuppCode = SuppCodeURL;
                        }
                        else
                        {
                            SuppCode = _SupplierDetail1.SuppCode;
                        }                       
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = Session["CompId"].ToString();
                        }
                        if (Session["BranchId"] != null)
                        {
                            Br_Id = Session["BranchId"].ToString();
                        }
                        Boolean act_status, tds_posting, on_hold = true;
                        DataSet ds = _SupplierDetail_ISERVICES.GetviewSuppdetailDAL(SuppCode, Comp_ID, Br_Id);
                        ViewBag.AttechmentDetails = ds.Tables[6];
                        if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                            act_status = true;
                        else
                            act_status = false;
                        if (ds.Tables[0].Rows[0]["on_hold"].ToString() == "Y")
                            on_hold = true;
                        else
                            on_hold = false;
                        if (ds.Tables[0].Rows[0]["tds_posting"].ToString() == "Y")
                            tds_posting = true;
                        else
                            tds_posting = false;
                        _SupplierDetail1.supp_id = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        _SupplierDetail1.suppname = ds.Tables[0].Rows[0]["supp_name"].ToString();
                        _SupplierDetail1.supp_name = ds.Tables[0].Rows[0]["supp_name"].ToString();
                        _SupplierDetail1.supp_alias = ds.Tables[0].Rows[0]["supp_alias"].ToString();
                        _SupplierDetail1.supp_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                        _SupplierDetail1.TDSApplicableOn = ds.Tables[0].Rows[0]["tds_app_on"].ToString();
                        string Sup_coa_name = ds.Tables[0].Rows[0]["acc_name"].ToString();
                        _SupplierDetail1.supp_type = ds.Tables[0].Rows[0]["supptype"].ToString();
                        _SupplierDetail1.PaymentAlert = Convert.ToInt32(ds.Tables[0].Rows[0]["paym_alrt"].ToString());
                        if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                        {

                        }
                        else
                        {
                            _SupplierDetail1.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                        }
                        _SupplierDetail1.GlRepoting_Group_ID = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                        _SupplierDetail1.GlRepoting_Group_Name = ds.Tables[0].Rows[0]["gl_rpt_des"].ToString();
                        _SupplierDetail1.GlRepoting_Group = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();

                        if (_SupplierDetail1.GlRepoting_Group_ID != "0")
                        {
                            List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                            GlReportingGroup Glrpt = new GlReportingGroup();
                            Glrpt.Gl_rpt_id = _SupplierDetail1.GlRepoting_Group_ID;
                            Glrpt.Gl_rpt_Name = _SupplierDetail1.GlRepoting_Group_Name;
                            glrpt.Add(Glrpt);

                            _SupplierDetail1.GlReportingGroupList = glrpt;
                        }
                        dt = Getcurr(ds.Tables[0].Rows[0]["supptype"].ToString());
                        DataView dv = new DataView(dt);
                        List<curr> _SuppcurrList1 = new List<curr>();
                        foreach (DataRowView dr in dv)
                        {
                            curr _Suppcurr = new curr();
                            _Suppcurr.curr_id = dr["curr_id"].ToString();
                            _Suppcurr.curr_name = dr["curr_name"].ToString();
                            _SuppcurrList1.Add(_Suppcurr);
                        }
                        _SupplierDetail1.currList = _SuppcurrList1;
                        //List<SuppCoa> _ListSuppCoa1 = new List<SuppCoa>();
                        // suppCode=_SupplierDetail1.SuppCode;
                        //dt = GetSuppCoa(suppCode, "O");
                        //foreach (DataRow dr in dt.Rows)
                        //{
                        //    SuppCoa _SuppCoa = new SuppCoa();
                        //    _SuppCoa.acc_id = dr["acc_id"].ToString();
                        //    _SuppCoa.acc_name = dr["acc_name"].ToString();
                        //    _ListSuppCoa1.Add(_SuppCoa);
                        //}
                        //_ListSuppCoa1.Insert(0, new SuppCoa() { acc_id = "0", acc_name = "---Select---" });
                        ////_ListSuppCoa1.Add(new SuppCoa { acc_id = _SupplierDetail1.supp_coa.ToString(), acc_name = Sup_coa_name });
                        //_SupplierDetail1.SuppCoaNameList = _ListSuppCoa1;
                        _SupplierDetail1.SupplierDependcy = ds.Tables[9].Rows[0]["SupplierDependcy"].ToString();
                        _SupplierDetail1.curr = int.Parse(ds.Tables[0].Rows[0]["CurrID"].ToString());
                        _SupplierDetail1.supp_catg = int.Parse(ds.Tables[0].Rows[0]["CatID"].ToString());
                        _SupplierDetail1.supp_port = int.Parse(ds.Tables[0].Rows[0]["PortID"].ToString());
                        _SupplierDetail1.supp_regn_no = ds.Tables[0].Rows[0]["supp_regn_no"].ToString();
                        _SupplierDetail1.supp_gst_no = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                        _SupplierDetail1.supp_tan_no = ds.Tables[0].Rows[0]["supp_tan_no"].ToString();
                        _SupplierDetail1.supp_pan_no = ds.Tables[0].Rows[0]["supp_pan_no"].ToString();
                        _SupplierDetail1.supp_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                        _SupplierDetail1.cont_pers = ds.Tables[0].Rows[0]["cont_pers"].ToString();
                        _SupplierDetail1.cont_email = ds.Tables[0].Rows[0]["cont_email"].ToString();
                        _SupplierDetail1.cont_num1 = ds.Tables[0].Rows[0]["cont_num1"].ToString();
                        _SupplierDetail1.cont_num2 = ds.Tables[0].Rows[0]["cont_num2"].ToString();
                        _SupplierDetail1.act_status = act_status;
                        _SupplierDetail1.on_hold = on_hold;
                        _SupplierDetail1.TDSApplicable = tds_posting;
                        _SupplierDetail1.inact_reason = ds.Tables[0].Rows[0]["inact_reason"].ToString();
                        _SupplierDetail1.onhold_reason = ds.Tables[0].Rows[0]["onhold_reason"].ToString();
                        _SupplierDetail1.supp_rmarks = ds.Tables[0].Rows[0]["supp_remarks"].ToString();
                        _SupplierDetail1.paym_term = int.Parse(ds.Tables[0].Rows[0]["paym_term"].ToString());
                        _SupplierDetail1.create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _SupplierDetail1.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _SupplierDetail1.app_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _SupplierDetail1.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _SupplierDetail1.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _SupplierDetail1.app_dt = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _SupplierDetail1.app_status = ds.Tables[0].Rows[0]["appstatus"].ToString();
                        _SupplierDetail1.bank_name = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        _SupplierDetail1.bank_branch = ds.Tables[0].Rows[0]["bank_branch"].ToString();
                        _SupplierDetail1.bank_add = ds.Tables[0].Rows[0]["bank_add"].ToString();
                        _SupplierDetail1.bank_acc_no = ds.Tables[0].Rows[0]["bank_acc_no"].ToString();
                        _SupplierDetail1.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                        _SupplierDetail1.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                        _SupplierDetail1.Gst_Cat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                        _SupplierDetail1.GLAccountName = ds.Tables[0].Rows[0]["GLAcc_Name"].ToString();
                        _SupplierDetail1.HdnInterBranch = ds.Tables[0].Rows[0]["inter_br"].ToString();
                        if (_SupplierDetail1.HdnInterBranch == "Y")
                        {
                            _SupplierDetail1.InterBranch = true;
                        }
                        else
                        {
                            _SupplierDetail1.InterBranch = false;
                        }
                        //ViewBag.CustomerBranchList = ds.Tables[1];
                        _SupplierDetail1.CustomerBranchList = ds.Tables[1];
                        ViewBag.SupplierDocumentDetail = ds.Tables[2];
                        ViewBag.SupplierAddressDetail = ds.Tables[3];
                        ViewBag.PurchaseHistory = ds.Tables[10];
                        ViewBag.licenseDetails = ds.Tables[11];
                        ViewBag.GLCount = ds.Tables[5].Rows[0]["GlCount"].ToString();
                        _SupplierDetail1.D_InActive = ds.Tables[7].Rows[0]["disableInActive"].ToString();
                        if (_SupplierDetail1.app_status == "Approved")
                        {
                            if (ds.Tables[8].Rows[0]["disenbcurr"].ToString() != null && ds.Tables[8].Rows[0]["disenbcurr"].ToString() != "")
                            {
                                _SupplierDetail1.curr_depncy = ds.Tables[8].Rows[0]["disenbcurr"].ToString();
                            }
                            else
                            {
                                _SupplierDetail1.curr_depncy = null;
                            }
                        }
                        /*Commented bY Hina on 05-01-2024 14:04 to change country,state,district,city in dropdown instead of textbox */

                        //foreach (DataRow dr in ds.Tables[4].Rows)
                        //{
                        //    SuppCity suppCity = new SuppCity();
                        //    suppCity.city_id = dr["city_id"].ToString();
                        //    suppCity.city_name = dr["city_name"].ToString();
                        //    suppCities.Add(suppCity);
                        //}
                        //_SupplierDetail1.SuppCityList = suppCities;
                        /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                        List<CmnCountryList> _ContryListU = new List<CmnCountryList>();
                        //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                        List<Country> _ContryList2U = new List<Country>();
                        CommonAddress_Detail _ModelU = new CommonAddress_Detail();
                        string SupplierTypeU = "D";
                        if (SupplierTypeU != "D")
                        {
                            _ContryListU.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                        }
                        if (!string.IsNullOrEmpty(_SupplierDetail1.supp_type))
                            SupplierTypeU = _SupplierDetail1.supp_type;

                         dt = GetCountryList(SupplierTypeU);

                        foreach (DataRow dr in dt.Rows)
                        {
                            CmnCountryList _Contry = new CmnCountryList();
                            _Contry.country_id = dr["country_id"].ToString();
                            _Contry.country_name = dr["country_name"].ToString();
                            _ContryListU.Add(_Contry);
                            _ContryList2U.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                        }
                        _Model.countryList = _ContryList2U;
                        _SupplierDetail1.countryList = _ContryListU;

                        List<CmnStateList> stateU = new List<CmnStateList>();
                        state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                        string transCountryU = "";
                        if (!string.IsNullOrEmpty(_SupplierDetail1.Country))
                            transCountryU = _SupplierDetail1.Country;
                        else
                            transCountryU = dt.Rows[0]["country_id"].ToString();

                         dt = _SupplierDetail_ISERVICES.GetstateOnCountryDDL(transCountryU);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                stateU.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                            }
                        }
                        _SupplierDetail1.StateList = stateU;

                        string transStateU = "0";
                        List<CmnDistrictList> DistListU = new List<CmnDistrictList>();
                        DistListU.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                        if (!string.IsNullOrEmpty(_SupplierDetail1.State))
                            transStateU = _SupplierDetail1.State;
                        else
                            transStateU = "0";
                         dt = _SupplierDetail_ISERVICES.GetDistrictOnStateDDL(transState);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                DistListU.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                            }
                        }
                        _SupplierDetail1.DistrictList = DistListU;

                        string transDistU = "0";
                        if (!string.IsNullOrEmpty(_SupplierDetail1.District))
                            transDistU = _SupplierDetail1.District;
                        else
                            transDistU = "0";
                         dt = _SupplierDetail_ISERVICES.GetCityOnDistrictDDL(transDistU);

                        List<CmnCityList> citiesU = new List<CmnCityList>();
                        citiesU.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                citiesU.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                            }
                        }
                        _SupplierDetail1.cityLists = citiesU;

                        _SupplierDetail1._CommonAddress_Detail = _Model;
                        /*------------------------------------------Code End of Country,state,district,city--------------------------*/
                        if (_SupplierDetail1.app_status == "Approved")
                        {
                            //Session["AppStatus"] = 'A';
                            _SupplierDetail1.AppStatus = "A";
                        }
                        else
                        {
                            //Session["AppStatus"] = 'D';
                            _SupplierDetail1.AppStatus = "D";
                        }
                    }
                    else
                    {
                        _SupplierDetail1.act_status = true;
                    }                    
                    if (_SupplierDetail1.BtnName == null)
                    {
                        _SupplierDetail1.BtnName= "BtnRefresh";
                    }
                    if (_SupplierDetail1.TransType == "Update")
                    {
                        _SupplierDetail1.BtnName = "BtnEdit";
                    }
                    if (BtnName != null)
                    {
                        _SupplierDetail1.BtnName = BtnName;
                    }
                    //_SupplierDetail1.TransType = Session["TransType"].ToString();
                    _SupplierDetail1.TransType = _SupplierDetail1.TransType;
                    _SupplierDetail1.DocumentMenuId = DocumentMenuId;
                    //ViewBag.VBRoleList = GetRoleList();
                   
                    return View("~/Areas/BusinessLayer/Views/SupplierSetup/SupplierDetail.cshtml", _SupplierDetail1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllDropDownList(SupplierDetail _SupplierDetail1) /**Added BY Nitesh 18-03-2024  For All Dropdown In One Method**/
        {
            string Comp_ID = string.Empty;
            string Br_Id = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_Id = Session["BranchId"].ToString();
            }
            // dt = GetAccountGroup("supp");
            DataSet dt = _SupplierDetail_ISERVICES.GetAllDropDown(Comp_ID, "Supp", Br_Id);
            List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
            foreach (DataRow Data in dt.Tables[0].Rows)
            {
                AccountGroup _AccountGroup = new AccountGroup();
                _AccountGroup.acc_grp_id = Data["acc_grp_id"].ToString();
                _AccountGroup.AccGroupChildNood = Data["AccGroupChildNood"].ToString();
                _AccountGroupList.Add(_AccountGroup);
            }
            _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
            _SupplierDetail1.AccountGroupList = _AccountGroupList;

            List<SuppCategory> _CategoryList = new List<SuppCategory>();
          //  dt = Getcategory();
            foreach (DataRow dr in dt.Tables[1].Rows)
            {
                SuppCategory _Category = new SuppCategory();
                _Category.setup_id = dr["setup_id"].ToString();
                _Category.setup_val = dr["setup_val"].ToString();
                _CategoryList.Add(_Category);
            }
            _CategoryList.Insert(0, new SuppCategory() { setup_id = "0", setup_val = "---Select---" });
            _SupplierDetail1.CategoryList = _CategoryList;


            List<SuppPortFolio> _PortFolioList = new List<SuppPortFolio>();
          //  dt = GetSuppport();
            foreach (DataRow dr in dt.Tables[2].Rows)
            {
                SuppPortFolio _PortFolio = new SuppPortFolio();
                _PortFolio.setup_id = dr["setup_id"].ToString();
                _PortFolio.setup_val = dr["setup_val"].ToString();
                _PortFolioList.Add(_PortFolio);
            }
            _PortFolioList.Insert(0, new SuppPortFolio() { setup_id = "0", setup_val = "---Select---" });
            _SupplierDetail1.PortFolioList = _PortFolioList;

            List<curr> _SuppcurrList = new List<curr>();
           // dt = Getcurr("D");
            foreach (DataRow dr in dt.Tables[3].Rows)
            {
                curr _Suppcurr = new curr();
                _Suppcurr.curr_id = dr["curr_id"].ToString();
                _Suppcurr.curr_name = dr["curr_name"].ToString();
                _SuppcurrList.Add(_Suppcurr);

            }
            //_SuppcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
            _SupplierDetail1.currList = _SuppcurrList;

            // dt = GetSupplierBranchList();
            //ViewBag.CustomerBranchList = dt;
            _SupplierDetail1.CustomerBranchList = dt.Tables[4];

            List<GlReportingGroup> _ItemList = new List<GlReportingGroup>();

            GlReportingGroup Glrpt = new GlReportingGroup();
            Glrpt.Gl_rpt_id = "0";
            Glrpt.Gl_rpt_Name = "---Select---";
            _ItemList.Add(Glrpt);

            _SupplierDetail1.GlReportingGroupList = _ItemList;

            _SupplierDetail1.FromDate = dt.Tables[7].Rows[0]["fin_st_date"].ToString();
            _SupplierDetail1.CompCountry = dt.Tables[8].Rows[0]["country_name"].ToString();
            _SupplierDetail1.CompCountryID = dt.Tables[8].Rows[0]["country_id"].ToString();

        }

        [HttpPost]
        public JsonResult GetGlReportingGrp(SupplierDetail _SupplierDetail1)
        {
            JsonResult DataRows = null;
            try
            {

                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string gl_repoting = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_SupplierDetail1.GlRepoting_Group))
                {
                    gl_repoting = "0";
                }
                else
                {
                    gl_repoting = _SupplierDetail1.GlRepoting_Group;
                }
                DataTable GlReporting = _SupplierDetail_ISERVICES.GetGlReportingGrp(Comp_ID, Br_ID, gl_repoting);
                //List<GlReportingGroup> _ItemList = new List<GlReportingGroup>();
                //foreach (DataRow data in GlReporting.Rows)
                //{
                //    GlReportingGroup Glrpt = new GlReportingGroup();
                //    Glrpt.Gl_rpt_id = data["ID"].ToString();
                //    Glrpt.Gl_rpt_Name = data["Val"].ToString();
                //    _ItemList.Add(Glrpt);
                //}
                //_CustomerDetails.GlReportingGroupList = _ItemList;
                return Json(JsonConvert.SerializeObject(GlReporting), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;//Json("ErrorPage");
            }

        }
        [NonAction]
        private DataTable GetAccountGroup(string type)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _SupplierDetail_ISERVICES.GetAccountGroupDAL(Comp_ID, type);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetRoleList()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, userid, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private string getDocumentName()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
                string[] Docpart = DocumentName.Split('>');
                int len = Docpart.Length;
                if (len > 1)
                {
                    title = Docpart[len - 1].Trim();
                }
                return DocumentName;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
           
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [NoDirectAccess]
        public ActionResult SupplierSave(SupplierDetail _SupplierDetail, string command,string supp_id)
        {
            try
            {
                if (_SupplierDetail.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Edit":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["SuppCode"] = _SupplierDetail.supp_id;
                        //Session["TransType"] = "Update";
                        //Session["BtnName"] = "BtnEdit";

                        _SupplierDetail.Message = "";
                       _SupplierDetail.Command = command;
                        _SupplierDetail.SuppCode = _SupplierDetail.supp_id;
                        _SupplierDetail.supp_type = _SupplierDetail.supp_type;
                        _SupplierDetail.TransType = "Update";
                       _SupplierDetail.BtnName = "BtnEdit";
                        var SuppCodeURL = _SupplierDetail.SuppCode;
                        var TransType = _SupplierDetail.TransType;
                        var BtnName = _SupplierDetail.BtnName;
                        TempData["ModelData"] = _SupplierDetail;
                        TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                        return( RedirectToAction("SupplierDetail", new { SuppCodeURL = SuppCodeURL, TransType, BtnName, command }));
                    case "Add":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["SuppCode"] = "";
                        //Session["AppStatus"] = "D";
                        //_SupplierDetail = null;
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        SupplierDetail _SupplierDetailAdd = new SupplierDetail();
                        //_SupplierDetail.Message = "";
                        _SupplierDetailAdd.Command = command;
                        //_SupplierDetail.SuppCode = "";
                        _SupplierDetailAdd.AppStatus = "D";
                        _SupplierDetailAdd.TransType = "Save";
                        _SupplierDetailAdd.BtnName = "BtnAddNew";
                        TempData["ListFilterData"] = null;
                        TempData["ModelData"] = _SupplierDetailAdd;
                        return RedirectToAction("SupplierDetail", "SupplierDetail");
                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Delete";

                        _SupplierDetail.Command = command;
                        _SupplierDetail.BtnName = "Delete";
                        supp_id = _SupplierDetail.supp_id;
                        SupplierDetailDelete(_SupplierDetail, command);
                        TempData["ModelData"] = _SupplierDetail;
                        TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                        return RedirectToAction("SupplierDetail");
                    case "Save":
                        //Session["Command"] = command;
                        _SupplierDetail.Command = command;
                        if (ModelState.IsValid)
                        {
                            if (_SupplierDetail.AppStatus != "A")
                            {
                                _SupplierDetail.AppStatus = "D";
                            }
                            InsertSupplierDetail(_SupplierDetail);
                            if(_SupplierDetail.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            //Session["SuppCode"] = Session["SuppCode"].ToString();
                            _SupplierDetail.SuppCode = _SupplierDetail.SuppCode;

                            //if (Session["Message"].ToString() == "Duplicate")
                            if (_SupplierDetail.Message == "Duplicate"|| _SupplierDetail.Message== "DuplicateGL")
                            {
                                List<SuppCategory> _CategoryList = new List<SuppCategory>();
                                dt = Getcategory();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    SuppCategory _Category = new SuppCategory();
                                    _Category.setup_id = dr["setup_id"].ToString();
                                    _Category.setup_val = dr["setup_val"].ToString();
                                    _CategoryList.Add(_Category);
                                }
                                _SupplierDetail.CategoryList = _CategoryList;
                                //GetAutoCompleteSuppcity(_SupplierDetail);
                                dt = GetSupplierBranchList();
                                //ViewBag.CustomerBranchList = dt;
                                _SupplierDetail.CustomerBranchList = dt;
                                var suppCode = _SupplierDetail.SuppCode;
                                dt = GetAccountGroup("supp");
                                List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AccountGroup _AccountGroup = new AccountGroup();
                                    _AccountGroup.acc_grp_id = dt["acc_grp_id"].ToString();
                                    _AccountGroup.AccGroupChildNood = dt["AccGroupChildNood"].ToString();
                                    _AccountGroupList.Add(_AccountGroup);
                                }
                                _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                                _SupplierDetail.AccountGroupList = _AccountGroupList;

                                List<SuppPortFolio> _PortFolioList = new List<SuppPortFolio>();
                                dt = GetSuppport();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    SuppPortFolio _PortFolio = new SuppPortFolio();
                                    _PortFolio.setup_id = dr["setup_id"].ToString();
                                    _PortFolio.setup_val = dr["setup_val"].ToString();
                                    _PortFolioList.Add(_PortFolio);
                                }
                                _SupplierDetail.PortFolioList = _PortFolioList;

                                List<curr> _SuppcurrList = new List<curr>();
                                dt = Getcurr("D");
                                foreach (DataRow dr in dt.Rows)
                                {
                                    curr _Suppcurr = new curr();
                                    _Suppcurr.curr_id = dr["curr_id"].ToString();
                                    _Suppcurr.curr_name = dr["curr_name"].ToString();
                                    _SuppcurrList.Add(_Suppcurr);

                                }
                                //_SuppcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });

                                _SupplierDetail.currList = _SuppcurrList;
                                CommonPageDetails();
                                //dt = GetBrList();
                                //List<SupplierBranch> _CustomerBranchList = new List<SupplierBranch>();
                                //foreach (DataRow dr in dt.Rows)
                                //{
                                //    SupplierBranch _SupplierBranch = new SupplierBranch();
                                //    _SupplierBranch.comp_id = dr["comp_id"].ToString();
                                //    _SupplierBranch.comp_nm = dr["comp_nm"].ToString();
                                //    _CustomerBranchList.Add(_SupplierBranch);
                                //}
                                //string Comp_ID = String.Empty;
                                //if (Session["CompId"] != null)
                                //{
                                //    Comp_ID = Session["CompId"].ToString();

                                //}
                                //string SuppCode = Session["SuppCode"].ToString();
                                //List<SuppCity> suppCities = new List<SuppCity>();
                                //suppCities.Insert(0, new SuppCity() { city_id = "0", city_name = "---Select---" });
                                //_SupplierDetail.SuppCityList = suppCities;
                                //DataSet ds = _SupplierDetail_ISERVICES.GetviewSuppdetailDAL(SuppCode, Comp_ID);
                                //ViewBag.CustomerBranchList = ds.Tables[1];
                                //foreach (DataRow dr in ds.Tables[4].Rows)
                                //{
                                //    SuppCity suppCity = new SuppCity();
                                //    suppCity.city_id = dr["city_id"].ToString();
                                //    suppCity.city_name = dr["city_name"].ToString();
                                //    suppCities.Add(suppCity);
                                //}
                                //_SupplierDetail.SuppCityList = suppCities;
                   /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                                List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                                string SupplierType = "";
                                if (_SupplierDetail.supp_type == null)
                                {
                                    SupplierType = "D";
                                }
                                else
                                {
                                    SupplierType = _SupplierDetail.supp_type;
                                    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                                }
                                //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                                //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                                List<Country> _ContryList2 = new List<Country>();
                                CommonAddress_Detail _Model = new CommonAddress_Detail();
                                //string SupplierType = "D";
                                //if (SupplierType != "D")
                                //{
                                //    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                                //}
                                //if (!string.IsNullOrEmpty(_SupplierDetail.supp_type))
                                //    SupplierType = _SupplierDetail.supp_type;


                                DataTable dtcntry = GetCountryList(SupplierType);

                                foreach (DataRow dr in dtcntry.Rows)
                                {
                                    CmnCountryList _Contry = new CmnCountryList();
                                    _Contry.country_id = dr["country_id"].ToString();
                                    _Contry.country_name = dr["country_name"].ToString();
                                    _ContryList.Add(_Contry);
                                    _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                                }
                                _Model.countryList = _ContryList2;
                                _SupplierDetail.countryList = _ContryList;

                                List<CmnStateList> state = new List<CmnStateList>();
                                state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                                string transCountry = "";
                                if (!string.IsNullOrEmpty(_SupplierDetail.Country))
                                    transCountry = _SupplierDetail.Country;
                                else
                                    transCountry = dtcntry.Rows[0]["country_id"].ToString();

                                DataTable dtStates = _SupplierDetail_ISERVICES.GetstateOnCountryDDL(transCountry);
                                if (dtStates.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtStates.Rows)
                                    {
                                        state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                                    }
                                }
                                _SupplierDetail.StateList = state;

                                string transState = "0";
                                List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                                DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                                if (!string.IsNullOrEmpty(_SupplierDetail.State))
                                    transState = _SupplierDetail.State;
                                else
                                    transState = "0";
                                DataTable dtDist = _SupplierDetail_ISERVICES.GetDistrictOnStateDDL(transState);
                                if (dtDist.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtDist.Rows)
                                    {
                                        DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                                    }
                                }
                                _SupplierDetail.DistrictList = DistList;
                                // _SupplierDetail.District = "0";

                                string transDist = "0";
                                if (!string.IsNullOrEmpty(_SupplierDetail.District))
                                    transDist = _SupplierDetail.District;
                                else
                                    transDist = "0";
                                DataTable dtCities = _SupplierDetail_ISERVICES.GetCityOnDistrictDDL(transDist);

                                List<CmnCityList> cities = new List<CmnCityList>();
                                cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                                if (dtCities.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtCities.Rows)
                                    {
                                        cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                                    }
                                }
                                _SupplierDetail.cityLists = cities;
                                //_SupplierDetail.City = "0";
                                _SupplierDetail._CommonAddress_Detail = _Model;
            /*------------------------------------------Code End of Country,state,district,city--------------------------*/
                                /*Commented By Hina on 10-01-2024 to bind all data on chnge country*/

                                //DataSet dset = GetsuppCities(_SupplierDetail);
                                //List<SuppCity> suppCities = new List<SuppCity>();
                                //foreach (DataRow dr in dset.Tables[4].Rows)
                                //{
                                //    SuppCity suppCity = new SuppCity();
                                //    suppCity.city_id = dr["city_id"].ToString();
                                //    suppCity.city_name = dr["city_name"].ToString();
                                //    suppCities.Add(suppCity);
                                //}
                                //suppCities.Insert(0, new SuppCity() { city_id = "0", city_name = "---Select---" });
                                //_SupplierDetail.SuppCityList = suppCities;
                                if (_SupplierDetail.SuppCode != "0")
                                {
                                    string Comp_ID = String.Empty;
                                    string Br_Id = String.Empty;
                                    if (Session["CompId"] != null)
                                    {
                                        Comp_ID = Session["CompId"].ToString();
                                    }
                                    if (Session["BranchId"] != null)
                                    {
                                        Br_Id = Session["BranchId"].ToString();
                                    }
                                    string SuppCode = _SupplierDetail.SuppCode;
                                    DataSet ds = _SupplierDetail_ISERVICES.GetviewSuppdetailDAL(SuppCode, Comp_ID, Br_Id);
                                    ViewBag.AttechmentDetails = ds.Tables[6];
                                    ViewBag.PurchaseHistory = ds.Tables[10];
                                    ViewBag.licenseDetails = ds.Tables[11];
                                    ViewBag.SupplierDocumentDetail = ds.Tables[2];
                                    ViewBag.SupplierAddressDetail = ds.Tables[3];
                                    ViewBag.GLCount = ds.Tables[5].Rows[0]["GlCount"].ToString();
                                    _SupplierDetail.CustomerBranchList = ds.Tables[1];
                                    _SupplierDetail.create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                    _SupplierDetail.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                    _SupplierDetail.app_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                                    _SupplierDetail.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                    _SupplierDetail.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                    _SupplierDetail.app_dt = ds.Tables[0].Rows[0]["app_dt"].ToString();
                                    _SupplierDetail.app_status = ds.Tables[0].Rows[0]["appstatus"].ToString();
                                    _SupplierDetail.HdnInterBranch = ds.Tables[0].Rows[0]["inter_br"].ToString();
                                    if (_SupplierDetail.HdnInterBranch == "Y")
                                    {
                                        _SupplierDetail.InterBranch = true;
                                    }
                                    else
                                    {
                                        _SupplierDetail.InterBranch = false;
                                    }
                                    //ViewBag.SupplierAddressDetail = ds.Tables[3];
                                    //_SupplierDetail.GLAccountName = ds.Tables[0].Rows[0]["GLAcc_Name"].ToString();
                                    if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                                    {

                                    }
                                    else
                                    {
                                        _SupplierDetail.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                                    }
                                    _SupplierDetail.GlRepoting_Group_ID = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                                    _SupplierDetail.GlRepoting_Group_Name = ds.Tables[0].Rows[0]["gl_rpt_des"].ToString();
                                    _SupplierDetail.GlRepoting_Group = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();

                                    if (_SupplierDetail.GlRepoting_Group_ID != "0" && _SupplierDetail.GlRepoting_Group_ID != null )
                                    {
                                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                                        GlReportingGroup Glrpt = new GlReportingGroup();
                                        Glrpt.Gl_rpt_id = _SupplierDetail.GlRepoting_Group_ID;
                                        Glrpt.Gl_rpt_Name = _SupplierDetail.GlRepoting_Group_Name;
                                        glrpt.Add(Glrpt);

                                        _SupplierDetail.GlReportingGroupList = glrpt;
                                    }
                                    else
                                    {
                                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();
                                        GlReportingGroup Glrpt = new GlReportingGroup();
                                        Glrpt.Gl_rpt_id = "0";
                                        Glrpt.Gl_rpt_Name = "---Select---";
                                        glrpt.Add(Glrpt);

                                        _SupplierDetail.GlReportingGroupList = glrpt;
                                    }
                                }
                                else
                                {
                                    if (_SupplierDetail.GlRepoting_Group_ID != "0" &&  _SupplierDetail.GlRepoting_Group_ID != null)
                                    {
                                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                                        GlReportingGroup Glrpt = new GlReportingGroup();
                                        Glrpt.Gl_rpt_id = _SupplierDetail.GlRepoting_Group_ID;
                                        Glrpt.Gl_rpt_Name = _SupplierDetail.GlRepoting_Group_Name;
                                        glrpt.Add(Glrpt);

                                        _SupplierDetail.GlReportingGroupList = glrpt;
                                    }
                                    else
                                    {
                                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();
                                        GlReportingGroup Glrpt = new GlReportingGroup();
                                        Glrpt.Gl_rpt_id = "0";
                                        Glrpt.Gl_rpt_Name = "---Select---";
                                        glrpt.Add(Glrpt);

                                        _SupplierDetail.GlReportingGroupList = glrpt;
                                    }
                                }
                                //Session["SuppCode"] = "";
                                //Session["AppStatus"] = "D";
                                _SupplierDetail.SuppCode = "";
                                _SupplierDetail.AppStatus = "D";
                                //Commented by Hina on 14-12-2022 to save data after duplicate 
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                _SupplierDetail.BtnName = "BtnAddNew";
                                //ViewBag.Message = Session["Message"].ToString();
                                _SupplierDetail.Message = _SupplierDetail.Message;
                                //ViewBag.VBRoleList = GetRoleList();
                                _SupplierDetail.act_status = true;
                                if(_SupplierDetail.Message== "DuplicateGL")
                                {
                                    _SupplierDetail.Command = "Edit";
                                }
                                _SupplierDetail.Saveapprovebtn = null;
                                TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                                _SupplierDetail.DocumentMenuId = DocumentMenuId;
                                return View("~/Areas/BusinessLayer/Views/SupplierSetup/SupplierDetail.cshtml", _SupplierDetail);
                            }
                            else
                            {
                                SuppCodeURL = _SupplierDetail.SuppCode;
                                _SupplierDetail.supp_type = _SupplierDetail.supp_type;
                                TransType = _SupplierDetail.TransType;
                                BtnName = _SupplierDetail.BtnName;
                                TempData["ModelData"] = _SupplierDetail;
                                TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                                _SupplierDetail.DocumentMenuId = DocumentMenuId;
                                return RedirectToAction("SupplierDetail", new { SuppCodeURL = SuppCodeURL, TransType, BtnName, command });
                            }                  
                        }
                        else
                        {
                            TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                            _SupplierDetail = null;
                            return View("~/Areas/BusinessLayer/Views/SupplierSetup/SupplierDetail.cshtml", _SupplierDetail);
                        }
                    case "Forward":
                        return new EmptyResult();
                    case "Approve":
                        //Session["Command"] = command;
                        _SupplierDetail.Command = command;
                        supp_id = _SupplierDetail.supp_id;
                        //Session["SuppCode"] = supp_id;
                        _SupplierDetail.SuppCode = supp_id;
                        SupplierApprove(_SupplierDetail, command,"");
                        if (_SupplierDetail.Message == "DuplicateGL")
                        {
                            List<SuppCategory> _CategoryList = new List<SuppCategory>();
                            dt = Getcategory();
                            foreach (DataRow dr in dt.Rows)
                            {
                                SuppCategory _Category = new SuppCategory();
                                _Category.setup_id = dr["setup_id"].ToString();
                                _Category.setup_val = dr["setup_val"].ToString();
                                _CategoryList.Add(_Category);
                            }
                            _SupplierDetail.CategoryList = _CategoryList;
                            dt = GetSupplierBranchList();
                            _SupplierDetail.CustomerBranchList = dt;
                            var suppCode = _SupplierDetail.SuppCode;
                            dt = GetAccountGroup("supp");
                            List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                            foreach (DataRow dt in dt.Rows)
                            {
                                AccountGroup _AccountGroup = new AccountGroup();
                                _AccountGroup.acc_grp_id = dt["acc_grp_id"].ToString();
                                _AccountGroup.AccGroupChildNood = dt["AccGroupChildNood"].ToString();
                                _AccountGroupList.Add(_AccountGroup);
                            }
                            _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                            _SupplierDetail.AccountGroupList = _AccountGroupList;

                            List<SuppPortFolio> _PortFolioList = new List<SuppPortFolio>();
                            dt = GetSuppport();
                            foreach (DataRow dr in dt.Rows)
                            {
                                SuppPortFolio _PortFolio = new SuppPortFolio();
                                _PortFolio.setup_id = dr["setup_id"].ToString();
                                _PortFolio.setup_val = dr["setup_val"].ToString();
                                _PortFolioList.Add(_PortFolio);
                            }
                            _SupplierDetail.PortFolioList = _PortFolioList;

                            List<curr> _SuppcurrList = new List<curr>();
                            dt = Getcurr("D");
                            foreach (DataRow dr in dt.Rows)
                            {
                                curr _Suppcurr = new curr();
                                _Suppcurr.curr_id = dr["curr_id"].ToString();
                                _Suppcurr.curr_name = dr["curr_name"].ToString();
                                _SuppcurrList.Add(_Suppcurr);

                            }
                            _SupplierDetail.currList = _SuppcurrList;
                            CommonPageDetails();
                            DataSet dset = GetsuppCities(_SupplierDetail);
                            /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                            List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                            string SupplierType = "";
                            if (_SupplierDetail.supp_type == null)
                            {
                                SupplierType = "D";
                            }
                            else
                            {
                                SupplierType = _SupplierDetail.supp_type;
                                _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                            }
                            //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                            //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                            List<Country> _ContryList2 = new List<Country>();
                            CommonAddress_Detail _Model = new CommonAddress_Detail();
                            //string SupplierType = "D";
                            //if (SupplierType != "D")
                            //{
                            //    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                            //}
                            //if (!string.IsNullOrEmpty(_SupplierDetail.supp_type))
                            //    SupplierType = _SupplierDetail.supp_type;


                            DataTable dtcntry = GetCountryList(SupplierType);

                            foreach (DataRow dr in dtcntry.Rows)
                            {
                                CmnCountryList _Contry = new CmnCountryList();
                                _Contry.country_id = dr["country_id"].ToString();
                                _Contry.country_name = dr["country_name"].ToString();
                                _ContryList.Add(_Contry);
                                _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                            }
                            _Model.countryList = _ContryList2;
                            _SupplierDetail.countryList = _ContryList;

                            List<CmnStateList> state = new List<CmnStateList>();
                            state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                            string transCountry = "";
                            if (!string.IsNullOrEmpty(_SupplierDetail.Country))
                                transCountry = _SupplierDetail.Country;
                            else
                                transCountry = dtcntry.Rows[0]["country_id"].ToString();

                            DataTable dtStates = _SupplierDetail_ISERVICES.GetstateOnCountryDDL(transCountry);
                            if (dtStates.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtStates.Rows)
                                {
                                    state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                                }
                            }
                            _SupplierDetail.StateList = state;

                            string transState = "0";
                            List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                            DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                            if (!string.IsNullOrEmpty(_SupplierDetail.State))
                                transState = _SupplierDetail.State;
                            else
                                transState = "0";
                            DataTable dtDist = _SupplierDetail_ISERVICES.GetDistrictOnStateDDL(transState);
                            if (dtDist.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtDist.Rows)
                                {
                                    DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                                }
                            }
                            _SupplierDetail.DistrictList = DistList;
                            // _SupplierDetail.District = "0";

                            string transDist = "0";
                            if (!string.IsNullOrEmpty(_SupplierDetail.District))
                                transDist = _SupplierDetail.District;
                            else
                                transDist = "0";
                            DataTable dtCities = _SupplierDetail_ISERVICES.GetCityOnDistrictDDL(transDist);

                            List<CmnCityList> cities = new List<CmnCityList>();
                            cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                            if (dtCities.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtCities.Rows)
                                {
                                    cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                                }
                            }
                            _SupplierDetail.cityLists = cities;
                            //_SupplierDetail.City = "0";
                            _SupplierDetail._CommonAddress_Detail = _Model;
                            /*------------------------------------------Code End of Country,state,district,city--------------------------*/

                            /*Commented By Hina on 10-01-2024 to bind all data on chnge country*/
                            //List<SuppCity> suppCities = new List<SuppCity>();
                            //foreach (DataRow dr in dset.Tables[4].Rows)
                            //{
                            //    SuppCity suppCity = new SuppCity();
                            //    suppCity.city_id = dr["city_id"].ToString();
                            //    suppCity.city_name = dr["city_name"].ToString();
                            //    suppCities.Add(suppCity);
                            //}
                            //suppCities.Insert(0, new SuppCity() { city_id = "0", city_name = "---Select---" });
                            //_SupplierDetail.SuppCityList = suppCities;
                            if (_SupplierDetail.SuppCode != "0")
                            {
                                string Comp_ID = String.Empty;
                                string Br_Id = String.Empty;
                                if (Session["CompId"] != null)
                                {
                                    Comp_ID = Session["CompId"].ToString();
                                }
                                if (Session["BranchId"] != null)
                                {
                                    Br_Id = Session["BranchId"].ToString();
                                }
                                string SuppCode = _SupplierDetail.SuppCode;

                                Boolean act_status, tds_posting, on_hold = true;
                                DataSet ds = _SupplierDetail_ISERVICES.GetviewSuppdetailDAL(SuppCode, Comp_ID, Br_Id);
                                ViewBag.AttechmentDetails = ds.Tables[6];
                                ViewBag.PurchaseHistory = ds.Tables[10];
                                ViewBag.SupplierDocumentDetail = ds.Tables[2];
                                ViewBag.SupplierAddressDetail = ds.Tables[3];
                                ViewBag.GLCount = ds.Tables[5].Rows[0]["GlCount"].ToString();
                                _SupplierDetail.CustomerBranchList = ds.Tables[1];
                                if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                                    act_status = true;
                                else
                                    act_status = false;

                                if (ds.Tables[0].Rows[0]["on_hold"].ToString() == "Y")
                                    on_hold = true;
                                else
                                    on_hold = false;
                                if (ds.Tables[0].Rows[0]["tds_posting"].ToString() == "Y")
                                    tds_posting = true;
                                else
                                    tds_posting = false;
                                _SupplierDetail.act_status = act_status;
                                _SupplierDetail.on_hold = on_hold;
                                _SupplierDetail.TDSApplicable = tds_posting;
                                _SupplierDetail.inact_reason = ds.Tables[0].Rows[0]["inact_reason"].ToString();
                                _SupplierDetail.onhold_reason = ds.Tables[0].Rows[0]["onhold_reason"].ToString();
                                _SupplierDetail.create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                                _SupplierDetail.mod_id = ds.Tables[0].Rows[0]["mod_id"].ToString();
                                _SupplierDetail.app_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                                _SupplierDetail.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                                _SupplierDetail.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                                _SupplierDetail.app_dt = ds.Tables[0].Rows[0]["app_dt"].ToString();
                                _SupplierDetail.app_status = ds.Tables[0].Rows[0]["appstatus"].ToString();
                                _SupplierDetail.supp_name = ds.Tables[0].Rows[0]["supp_name"].ToString();
                                _SupplierDetail.GLAccountName = ds.Tables[0].Rows[0]["GLAcc_Name"].ToString();
                                _SupplierDetail.HdnInterBranch = ds.Tables[0].Rows[0]["inter_br"].ToString();
                                if (_SupplierDetail.HdnInterBranch == "Y")
                                {
                                    _SupplierDetail.InterBranch = true;
                                }
                                else
                                {
                                    _SupplierDetail.InterBranch = false;
                                }
                                ViewBag.SupplierAddressDetail = ds.Tables[3];
                                if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                                {

                                }
                                else
                                {
                                    _SupplierDetail.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                                }
                                _SupplierDetail.GlRepoting_Group_ID = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                                _SupplierDetail.GlRepoting_Group_Name = ds.Tables[0].Rows[0]["gl_rpt_des"].ToString();
                                _SupplierDetail.GlRepoting_Group = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();

                                if (_SupplierDetail.GlRepoting_Group_ID != "0" && _SupplierDetail.GlRepoting_Group_ID != null)
                                {
                                    List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                                    GlReportingGroup Glrpt = new GlReportingGroup();
                                    Glrpt.Gl_rpt_id = _SupplierDetail.GlRepoting_Group_ID;
                                    Glrpt.Gl_rpt_Name = _SupplierDetail.GlRepoting_Group_Name;
                                    glrpt.Add(Glrpt);

                                    _SupplierDetail.GlReportingGroupList = glrpt;
                                }
                                else
                                {
                                    List<GlReportingGroup> glrpt = new List<GlReportingGroup>();
                                    GlReportingGroup Glrpt = new GlReportingGroup();
                                    Glrpt.Gl_rpt_id = "0";
                                    Glrpt.Gl_rpt_Name = "---Select---";
                                    glrpt.Add(Glrpt);

                                    _SupplierDetail.GlReportingGroupList = glrpt;
                                }
                            }
                            else
                            {
                                if (_SupplierDetail.GlRepoting_Group_ID != "0" && _SupplierDetail.GlRepoting_Group_ID != null)
                                {
                                    List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                                    GlReportingGroup Glrpt = new GlReportingGroup();
                                    Glrpt.Gl_rpt_id = _SupplierDetail.GlRepoting_Group_ID;
                                    Glrpt.Gl_rpt_Name = _SupplierDetail.GlRepoting_Group_Name;
                                    glrpt.Add(Glrpt);

                                    _SupplierDetail.GlReportingGroupList = glrpt;
                                }
                                else
                                {
                                    List<GlReportingGroup> glrpt = new List<GlReportingGroup>();
                                    GlReportingGroup Glrpt = new GlReportingGroup();
                                    Glrpt.Gl_rpt_id = "0";
                                    Glrpt.Gl_rpt_Name = "---Select---";
                                    glrpt.Add(Glrpt);

                                    _SupplierDetail.GlReportingGroupList = glrpt;
                                }
                            }
                            _SupplierDetail.SuppCode = "";
                            _SupplierDetail.AppStatus = "D";
                            _SupplierDetail.BtnName = "BtnToDetailPage";
                            _SupplierDetail.Message = _SupplierDetail.Message;
                            _SupplierDetail.act_status = true;
                            TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                            _SupplierDetail.DocumentMenuId = DocumentMenuId;
                            _SupplierDetail.Command = "Add";
                            _SupplierDetail.TransType = "Update";
                            return View("~/Areas/BusinessLayer/Views/SupplierSetup/SupplierDetail.cshtml", _SupplierDetail);
                        }
                        SuppCodeURL = _SupplierDetail.supp_id;
                        TransType = _SupplierDetail.TransType;
                        BtnName = _SupplierDetail.BtnName;
                            TempData["ModelData"] = _SupplierDetail;
                        TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                        return RedirectToAction("SupplierDetail", new { SuppCodeURL = SuppCodeURL, TransType, BtnName, command });
                    case "Refresh":
                        SupplierDetail _SupplierDetailRefresh=new SupplierDetail();
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Refresh";
                        //Session["Message"] = "";
                        //Session["AppStatus"] = "";
                        //_SupplierDetail = null;
                        _SupplierDetailRefresh.BtnName= "BtnRefresh";
                        _SupplierDetailRefresh.Command = command;
                        _SupplierDetailRefresh.TransType = "Refresh";
                        //_SupplierDetail.BtnName = "BtnRefresh";
                       //_SupplierDetail.Command = command;
                       //_SupplierDetail.TransType = "Refresh";
                       // _SupplierDetail.Message = "";
                       // _SupplierDetail.AppStatus = "";
                       // _SupplierDetail.supp_name = null;
                       // _SupplierDetail.supp_catg = 0;
                       // _SupplierDetail.supp_port = 0;
                       // _SupplierDetail.paym_term = null;
                       // _SupplierDetail.supp_coa = 0;
                       // _SupplierDetail.curr = null;
                       // _SupplierDetail.supp_regn_no = null;
                       // _SupplierDetail.supp_tan_no = null;
                       // _SupplierDetail.supp_pan_no = null;
                       // _SupplierDetail.cont_pers = null;
                       // _SupplierDetail.supp_name = null;
                       // _SupplierDetail.cont_email = null;
                       // _SupplierDetail.cont_num1 = null;
                       // _SupplierDetail.cont_num2 = null;
                       // _SupplierDetail.on_hold = false;
                       // _SupplierDetail.act_status = true;
                       // _SupplierDetail.inact_reason = null;
                       // _SupplierDetail.onhold_reason= null;
                       // _SupplierDetail.SupplierAddressDetails = null;
                       // _SupplierDetail.SupplierAddressDetail = null;
                       // _SupplierDetail.create_id = null;
                       // _SupplierDetail.creat_dt = null;
                       // _SupplierDetail.mod_dt = null;
                       // _SupplierDetail.mod_id = null;
                       // _SupplierDetail.app_status = null;
                       // _SupplierDetail.app_id = null;
                       // _SupplierDetail.app_dt = null;
                       // _SupplierDetail.acc_grp_id = 0;
                        TempData["ModelData"] = _SupplierDetailRefresh;
                        TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                        return RedirectToAction("SupplierDetail");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        TempData["ListFilterData"] = _SupplierDetail.ListFilterData1;
                        return RedirectToAction("SupplierList", "SupplierList");

                    default:
                        return new EmptyResult();

                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private DataSet GetsuppCities(SupplierDetail _SupplierDetail)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_Id = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_Id = Session["BranchId"].ToString();
                }
                string SuppCode = _SupplierDetail.SuppCode;
                DataSet dset = _SupplierDetail_ISERVICES.GetviewSuppdetailDAL(SuppCode, Comp_ID, Br_Id);
                return dset;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult InsertSupplierDetail(SupplierDetail _SupplierDetail)
        {
            
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            try
            {
                    if (Session["compid"] != null)
                    {
                        comp_id = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }

                    DataTable SupplierDetail = new DataTable();
                    DataTable SupplierBranch = new DataTable();
                    DataTable SupplierAttachments = new DataTable();
                DataTable SupplierAddress = new DataTable();
                DataTable LicenceDetail = new DataTable();

                DataTable dt = new DataTable();
                    dt.Columns.Add("TransType", typeof(string));
                dt.Columns.Add("SupplierFromProspect", typeof(string));
                dt.Columns.Add("Prospect_id", typeof(string));
                dt.Columns.Add("brid", typeof(string));
                dt.Columns.Add("supp_id", typeof(string));
                dt.Columns.Add("supp_name", typeof(string));
                dt.Columns.Add("supp_type", typeof(string));
                     dt.Columns.Add("supp_catg", typeof(int));
                    dt.Columns.Add("supp_port", typeof(int));                   
                    dt.Columns.Add("supp_coa", typeof(int));
                    dt.Columns.Add("supp_remarks", typeof(string));
                dt.Columns.Add("app_status", typeof(string));
                dt.Columns.Add("def_curr_id", typeof(int));
                dt.Columns.Add("supp_gst_no", typeof(string));
                dt.Columns.Add("supp_regn_no", typeof(string));
                dt.Columns.Add("supp_tan_no", typeof(string));
                dt.Columns.Add("on_hold", typeof(string));
                dt.Columns.Add("onhold_reason", typeof(string));
                dt.Columns.Add("act_status", typeof(string));
                dt.Columns.Add("inact_reason", typeof(string));
                dt.Columns.Add("supp_pan_no", typeof(string));
                dt.Columns.Add("cont_pers", typeof(string));
                dt.Columns.Add("create_id", typeof(string));
                dt.Columns.Add("mod_id", typeof(string));
                dt.Columns.Add("comp_id", typeof(int));
                dt.Columns.Add("UserMacaddress", typeof(string));
                    dt.Columns.Add("UserSystemName", typeof(string));
                    dt.Columns.Add("UserIP", typeof(string));
                    dt.Columns.Add("cont_email", typeof(string));
                    dt.Columns.Add("cont_num1", typeof(string));
                    dt.Columns.Add("cont_num2", typeof(string));
                    dt.Columns.Add("paym_term", typeof(int));
                dt.Columns.Add("bank_name", typeof(string));
                dt.Columns.Add("bank_branch", typeof(string));
                dt.Columns.Add("bank_add", typeof(string));
                dt.Columns.Add("bank_acc_no", typeof(string));
                dt.Columns.Add("ifsc_code", typeof(string));
                dt.Columns.Add("swift_code", typeof(string));
                dt.Columns.Add("gst_cat", typeof(string));
                dt.Columns.Add("acc_grp_id", typeof(string));
                dt.Columns.Add("acc_name", typeof(string));
                dt.Columns.Add("tds_posting", typeof(string));
                dt.Columns.Add("tds_app_on", typeof(string));
                dt.Columns.Add("gl_rpt_id", typeof(int));
                dt.Columns.Add("supp_alias", typeof(string));
                dt.Columns.Add("inter_br", typeof(char));
                DataRow dtrow = dt.NewRow();
                //var transtype = Session["TransType"].ToString();
                //dtrow["TransType"] = Session["TransType"].ToString();
                dtrow["TransType"] = _SupplierDetail.TransType;
                //dtrow["Prospect_id"] = Session["Prospect_id"].ToString();
                //dtrow["brid"] = Session["brid"].ToString();
                //dtrow["SupplierFromProspect"] = Session["SupplierFromProspect"].ToString();
                if (_SupplierDetail.SupplierFromProspect != null)
                {
                    dtrow["SupplierFromProspect"] = _SupplierDetail.SupplierFromProspect;
                    dtrow["Prospect_id"] = _SupplierDetail.Prospect_id;
                    dtrow["brid"] = Session["brid"].ToString();
                }
                else
                {
                    dtrow["SupplierFromProspect"] = "";//_SupplierDetail.SupplierFromProspect;
                    dtrow["Prospect_id"] = "";//_SupplierDetail.Prospect_id;
                    dtrow["brid"] = "";//Session["brid"].ToString();
                }
                if (!string.IsNullOrEmpty(_SupplierDetail.supp_id))
                    {
                        dtrow["supp_id"] = _SupplierDetail.supp_id;
                    }
                    else
                    {
                        dtrow["supp_id"] = "0";
                    }
                if(_SupplierDetail.supp_name ==null || _SupplierDetail.supp_name=="")
                {
                    dtrow["supp_name"] = _SupplierDetail.suppname;
                }
                else
                {
                    dtrow["supp_name"] = _SupplierDetail.supp_name;
                }
                
                dtrow["supp_type"] = _SupplierDetail.supp_type;
                if (_SupplierDetail.supp_catg != 0)
                {
                    dtrow["supp_catg"] = _SupplierDetail.supp_catg;
                }
                else
                {
                    dtrow["supp_catg"] = _SupplierDetail.ddl_supp_catg;
                }
                if (_SupplierDetail.supp_port != 0)
                {
                    dtrow["supp_port"] = _SupplierDetail.supp_port;
                }
                else
                {
                    dtrow["supp_port"] = _SupplierDetail.ddl_supp_port;
                }
                //if (_SupplierDetail.supp_coa != 0)
                //{
                    dtrow["supp_coa"] = _SupplierDetail.supp_coa;
                //}
                //else
                //{
                    //dtrow["supp_coa"] = _SupplierDetail.ddl_supp_coa;
                //}
                dtrow["supp_remarks"] = _SupplierDetail.supp_rmarks;
                //dtrow["app_status"] = Session["AppStatus"].ToString();
                dtrow["app_status"] = _SupplierDetail.AppStatus;
                dtrow["def_curr_id"] = _SupplierDetail.curr;
                dtrow["supp_gst_no"] = _SupplierDetail.supp_gst_no;
                dtrow["supp_regn_no"] = _SupplierDetail.supp_regn_no;
                dtrow["supp_tan_no"] = _SupplierDetail.supp_tan_no;
                if (_SupplierDetail.on_hold)
                {
                    dtrow["on_hold"] = "Y";
                }
                else
                {
                    dtrow["on_hold"] = "N";
                }
                dtrow["onhold_reason"] = _SupplierDetail.onhold_reason;
                //if (Session["TransType"].ToString() == "Save")
                if (_SupplierDetail.TransType == "Save")
                {
                    dtrow["act_status"] = "Y";
                }
                else
                {
                    if (_SupplierDetail.act_status)
                    {
                        dtrow["act_status"] = "Y";
                    }
                    else
                    {
                        dtrow["act_status"] = "N";
                    }
                }               
                dtrow["inact_reason"] = _SupplierDetail.inact_reason;
                dtrow["supp_pan_no"] = _SupplierDetail.supp_pan_no;
                dtrow["cont_pers"] = _SupplierDetail.cont_pers;
                dtrow["create_id"] = Session["UserId"].ToString();
                dtrow["mod_id"] = Session["UserId"].ToString();
                dtrow["comp_id"] = comp_id;
                dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                dtrow["UserIP"] = Session["UserIP"].ToString();
                dtrow["cont_email"] = _SupplierDetail.cont_email;
                dtrow["cont_num1"] = _SupplierDetail.cont_num1;
                dtrow["cont_num2"] = _SupplierDetail.cont_num2;
                if (!string.IsNullOrEmpty(Convert.ToString(_SupplierDetail.paym_term)))
                {
                    dtrow["paym_term"] = _SupplierDetail.paym_term;
                }
                else
                {
                    dtrow["paym_term"] = "0";
                }
                dtrow["bank_name"] = _SupplierDetail.bank_name;
                dtrow["bank_branch"] = _SupplierDetail.bank_branch;
                dtrow["bank_add"] = _SupplierDetail.bank_add;
                dtrow["bank_acc_no"] = _SupplierDetail.bank_acc_no;
                dtrow["ifsc_code"] = _SupplierDetail.ifsc_code;
                dtrow["swift_code"] = _SupplierDetail.swift_code;
                dtrow["gst_cat"] = _SupplierDetail.Gst_Cat;
                dtrow["acc_grp_id"] = _SupplierDetail.acc_grp_id;
                dtrow["acc_name"] = _SupplierDetail.GLAccountName;
                if (_SupplierDetail.TDSApplicable)
                {
                    dtrow["tds_posting"] = "Y";
                }
                else
                {
                    dtrow["tds_posting"] = "N";
                }
               
                dtrow["tds_app_on"] = _SupplierDetail.TDSApplicableOn==null?"B": _SupplierDetail.TDSApplicableOn;//Added by Suraj on 02-07-2024
                dtrow["gl_rpt_id"] = Convert.ToInt32(_SupplierDetail.GlRepoting_Group_ID);
                dtrow["supp_alias"] = _SupplierDetail.supp_alias;
                dtrow["inter_br"] = _SupplierDetail.HdnInterBranch;
                dt.Rows.Add(dtrow);

                SupplierDetail = dt;

                    DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("supp_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));
           
                JArray jObject = JArray.Parse(_SupplierDetail.SupplierBranchDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    if (!string.IsNullOrEmpty(_SupplierDetail.supp_id))
                    {
                        dtrowBrdetails["supp_id"] = _SupplierDetail.supp_id;
                    }
                    else
                    {
                        dtrowBrdetails["supp_id"] = "0";
                    }
                   
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString(); 
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();
                   
                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                SupplierBranch = dtBranch;

                DataTable dtAddress = new DataTable();
                dtAddress.Columns.Add("comp_id", typeof(int));
                dtAddress.Columns.Add("supp_id", typeof(int));
                dtAddress.Columns.Add("supp_pin", typeof(string));
                dtAddress.Columns.Add("address_id", typeof(int));
                dtAddress.Columns.Add("supp_add", typeof(string));
                dtAddress.Columns.Add("supp_city", typeof(int));
                dtAddress.Columns.Add("supp_dist", typeof(int));
                dtAddress.Columns.Add("supp_state", typeof(int));
                dtAddress.Columns.Add("supp_cntry", typeof(int));
                dtAddress.Columns.Add("supp_gst_no", typeof(string));
                dtAddress.Columns.Add("def_bill_add", typeof(string));
                dtAddress.Columns.Add("Flag", typeof(string));

                JArray AddObject = JArray.Parse(_SupplierDetail.SupplierAddressDetails);
                for (int i = 0; i < AddObject.Count; i++)
                {
                    DataRow dtrowAddress = dtAddress.NewRow();
                    dtrowAddress["comp_id"] = Session["CompId"].ToString();
                    if (!string.IsNullOrEmpty(_SupplierDetail.supp_id))
                    {
                        dtrowAddress["supp_id"] = _SupplierDetail.supp_id;
                    }
                    else
                    {
                        dtrowAddress["supp_id"] = "0";
                    }
                    dtrowAddress["supp_pin"] = AddObject[i]["pin"].ToString();
                    dtrowAddress["address_id"] = AddObject[i]["address_id"].ToString();
                    dtrowAddress["supp_add"] = AddObject[i]["address"].ToString();
                    dtrowAddress["supp_city"] = AddObject[i]["City"].ToString();
                    dtrowAddress["supp_dist"] = AddObject[i]["District"].ToString();
                    dtrowAddress["supp_state"] = AddObject[i]["State"].ToString();
                    dtrowAddress["supp_cntry"] = AddObject[i]["Country"].ToString();
                    dtrowAddress["supp_gst_no"] = AddObject[i]["GSTNo"].ToString();
                    dtrowAddress["def_bill_add"] = AddObject[i]["BillingAddress"].ToString();
                    dtrowAddress["Flag"] = AddObject[i]["Flag"].ToString();

                    dtAddress.Rows.Add(dtrowAddress);
                }
                SupplierAddress = dtAddress;
                // DataTable dtAttachment = new DataTable();
                // dtAttachment.Columns.Add("supp_id", typeof(Int32));
                // dtAttachment.Columns.Add("comp_id", typeof(Int32));
                // dtAttachment.Columns.Add("doc_name", typeof(char));
                // dtAttachment.Columns.Add("doc_attach", typeof(char));

                //if (SupplierDoc.Length > 0)
                //{


                //    string AttachmentFilePath = Server.MapPath("~/Attachment/SupplierDocument/");
                //    if (Directory.Exists(AttachmentFilePath))
                //    {
                //        string[] filePaths = Directory.GetFiles(AttachmentFilePath, comp_id + _SupplierDetail.supp_id.Replace("/", "") + "-*");
                //        foreach (var fielpath in filePaths)
                //        {
                //            System.IO.File.Delete(fielpath);
                //        }
                //    }

                //    foreach (HttpPostedFileBase file in SupplierDoc)
                //    {
                //        if (file != null)
                //        {
                //            string str = _SupplierDetail.supp_id.Replace("/", "");
                //            string img_nm = comp_id + str + "-" + Path.GetFileName(file.FileName).ToString();
                //            string doc_path = Path.Combine(Server.MapPath("~/Attachment/SupplierDocument/"), img_nm);
                //            string DocumentPath = Server.MapPath("~/Attachment/SupplierDocument/");
                //            int br_id = _SupplierDetail.br_id;
                //        string Supp_id = _SupplierDetail.supp_id;
                //            if (!Directory.Exists(DocumentPath))
                //            {
                //                DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                //            }
                //            file.SaveAs(doc_path);

                //            DataRow dtrowAttachment = dtAttachment.NewRow();
                //        //dtrowAttachment["supp_id"] = 0;
                //        if (!string.IsNullOrEmpty(_SupplierDetail.supp_id))
                //        {
                //            dtrowAttachment["supp_id"] = _SupplierDetail.supp_id;
                //        }
                //        else
                //        {
                //            dtrowAttachment["supp_id"] = "0";
                //        }
                //        dtrowAttachment["comp_id"] = Session["CompId"].ToString();
                //        dtrowAttachment["doc_name"] = img_nm;
                //        dtrowAttachment["doc_attach"] = doc_path;

                //            dtAttachment.Rows.Add(dtrowAttachment);
                //        }

                //    }
                //SupplierAttachments = dtAttachment;
                //}
                var _SupplierDetailAttch = TempData["ModelDataattch"] as SupplierDetailattch;
                TempData["ModelDataattch"] = null;
                DataTable dtAttachment = new DataTable();
                if (_SupplierDetail.attatchmentdetail != null)
                {

                    //if (Session["AttachMentDetailItmStp"] != null)
                    //{
                    //    dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                    //}
                    if (_SupplierDetailAttch != null)
                    {
                        if (_SupplierDetailAttch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _SupplierDetailAttch.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }
                    else
                    {
                        if (_SupplierDetail.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _SupplierDetail.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }                   
                    JArray jObject1 = JArray.Parse(_SupplierDetail.attatchmentdetail);
                    for (int i = 0; i < jObject1.Count; i++)
                    {
                        string flag = "Y";
                        foreach (DataRow dr in dtAttachment.Rows)
                        {
                            string drImg = dr["file_name"].ToString();
                            string ObjImg = jObject1[i]["file_name"].ToString();
                            if (drImg == ObjImg)
                            {
                                flag = "N";
                            }
                        }
                        if (flag == "Y")
                        {

                            DataRow dtrowAttachment1 = dtAttachment.NewRow();
                            if (!string.IsNullOrEmpty(_SupplierDetail.supp_id))
                            {
                                dtrowAttachment1["id"] = _SupplierDetail.supp_id;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_SupplierDetail.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_SupplierDetail.supp_id))
                            {
                                ItmCode = _SupplierDetail.supp_id;
                            }
                            else
                            {
                                ItmCode = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + ItmCode.Replace("/", "") + "*");

                            foreach (var fielpath in filePaths)
                            {
                                string flag = "Y";
                                foreach (DataRow dr in dtAttachment.Rows)
                                {
                                    string drImgPath = dr["file_path"].ToString();
                                    if (drImgPath == fielpath.Replace("/",@"\"))
                                    {
                                        flag = "N";
                                    }
                                }
                                if (flag == "Y")
                                {
                                    System.IO.File.Delete(fielpath);
                                }
                            }
                        }
                    }
                    SupplierAttachments = dtAttachment;
                }
                /***---------------------------------------------------------------***/
                DataTable dtLicence = new DataTable();

                dtLicence.Columns.Add("lic_id", typeof(int));
                dtLicence.Columns.Add("lic_name", typeof(string));
                dtLicence.Columns.Add("lic_number", typeof(string));
                dtLicence.Columns.Add("exp_dt", typeof(string));
                dtLicence.Columns.Add("alert_days", typeof(int));

                if (_SupplierDetail.LincenceDetail != null)
                {
                    JArray AddObj = JArray.Parse(_SupplierDetail.LincenceDetail);
                    for (int i = 0; i < AddObj.Count; i++)
                    {
                        DataRow dtrowAddress = dtLicence.NewRow();
                        dtrowAddress["lic_id"] = AddObj[i]["l_id"].ToString();
                        dtrowAddress["lic_name"] = AddObj[i]["LicenseNm"].ToString();
                        dtrowAddress["lic_number"] = AddObj[i]["LicenseNum"].ToString();
                        dtrowAddress["exp_dt"] = AddObj[i]["ExpDate"].ToString();
                        dtrowAddress["alert_days"] = Convert.ToInt32(AddObj[i]["ExpiryAlrtDays"]);
                        dtLicence.Rows.Add(dtrowAddress);
                    }
                    LicenceDetail = dtLicence;
                }

                /***---------------------------------------------------------------***/
                SaveMessage = _SupplierDetail_ISERVICES.insertSupplierDetails(SupplierDetail, SupplierBranch, SupplierAttachments, SupplierAddress,Convert.ToInt32(_SupplierDetail.PaymentAlert), LicenceDetail);
               
                    string SuppCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
             
               
                if (Message == "Save")
                {
                    string Guid = "";
                    if (_SupplierDetailAttch != null)
                    {
                        //if (Session["Guid"] != null)
                        if (_SupplierDetailAttch.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _SupplierDetailAttch.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    comCont.ResetImageLocation(CompID, "00", guid, PageName, SuppCode, _SupplierDetail.TransType, SupplierAttachments);

                    //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                    //if (Directory.Exists(sourcePath))
                    //{
                    //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Guid + "_" + "*");
                    //    foreach (string file in filePaths)
                    //    {
                    //        string[] items = file.Split('\\');
                    //        string ItemName = items[items.Length - 1];
                    //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                    //        foreach (DataRow dr in SupplierAttachments.Rows)
                    //        {
                    //            string DrItmNm = dr["file_name"].ToString();
                    //            if (ItemName == DrItmNm)
                    //            {
                    //                string img_nm = CompID + SuppCode + "_" + Path.GetFileName(DrItmNm).ToString();
                    //                string doc_path = Path.Combine(Server.MapPath("~/Attachment/" + PageName + "/"), img_nm);
                    //                string DocumentPath = Server.MapPath("~/Attachment/" + PageName + "/");
                    //                if (!Directory.Exists(DocumentPath))
                    //                {
                    //                    DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                    //                }
                    //                System.IO.File.Move(file, doc_path);
                    //            }
                    //        }
                    //    }
                    //}
                }
                if ( Message == "DataNotFound")
                {                                 
                    var a = SuppCode.Split('-');
                    var msg = "Data Not Found"+" "+ a[0].Trim()+" in "+PageName;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg,"","");                  
                    _SupplierDetail.Message = Message;
                    return RedirectToAction("SupplierDetail");
                }
                if (Message == "Update" || Message == "Save")
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _SupplierDetail.AttachMentDetailItmStp = null;
                    //Session["Guid"] = null;
                    //Session["Message"] = "Save";
                    //Session["SuppCode"] = SuppCode;
                    //Session["TransType"] = "Update";

                    _SupplierDetail.Guid = null;
                    _SupplierDetail.Message = "Save";
                    _SupplierDetail.SuppCode = SuppCode;
                    _SupplierDetail.TransType = "Update";
                }                
                    if (Message == "Duplicate")
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _SupplierDetail.AttachMentDetailItmStp = null;
                    //Session["Guid"] = null;
                    _SupplierDetail.Guid = null;
                    //Commented by Hina on 14-12-2022 to save data after duplicate 
                    //Session["TransType"] = "Duplicate";
                    //Session["Message"] = "Duplicate";
                    //Session["Command"] = "New";
                    //Session["SuppCode"] = SuppCode;

                    _SupplierDetail.Message = "Duplicate";
                    _SupplierDetail.Command = "New";
                    _SupplierDetail.SuppCode = SuppCode;
                }
                if (Message == "DuplicateGL")
                {
                    _SupplierDetail.Message = "DuplicateGL";
                    _SupplierDetail.Command = "New";
                    _SupplierDetail.SuppCode = SuppCode;
                }
                    if (_SupplierDetail.app_status == "A")
                {
                    //Session["AppStatus"] = 'A';
                    _SupplierDetail.AppStatus = "A";
                }
                else
                {
                    //Session["AppStatus"] = 'D';
                    _SupplierDetail.AppStatus = "D";
                }
                //Session["BtnName"] = "BtnSave";
                _SupplierDetail.BtnName = "BtnSave";
                TempData["ModelData"] = _SupplierDetail;
                return RedirectToAction("SupplierDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_SupplierDetail.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_SupplierDetail.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _SupplierDetail.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID, PageName, Guid, Server);
                    }
                }
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
                //return View("~/Views/Shared/Error.cshtml");
            }

        }
        public JsonResult Upload(string title,string DocNo, string TransType)
        {

            try
            {
                SupplierDetailattch _SupplierDetail = new SupplierDetailattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string SuppCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //if (_SupplierDetail.TransType != null)
                //{
                //    //TransType = Session["TransType"].ToString();
                //    TransType = _SupplierDetail.TransType;
                //}
                //if (Session["SuppCode"] != null)
                //if (_SupplierDetail.SuppCode != null)
                //{
                //    //SuppCode = Session["SuppCode"].ToString();
                //    SuppCode = DocNo;
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                //SuppCode = SuppCode.Replace("/", "");
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = SuppCode;
                _SupplierDetail.Guid = DocNo;

                if (Session["compid"] != null)
                {
                    comp_id = Session["compid"].ToString();
                }
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, comp_id, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _SupplierDetail.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _SupplierDetail.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _SupplierDetail;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        private ActionResult SupplierDetailDelete(SupplierDetail _SupplierDetail, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();

                }
                string supp_id = _SupplierDetail.supp_id;
                DataSet Message = _SupplierDetail_ISERVICES.SupplierDetailDelete(_SupplierDetail, comp_id, supp_id);
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Delete";
                //Session["SuppCode"] = "";
                //_SupplierDetail = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "Delete";

                _SupplierDetail.Message = "Deleted";
                _SupplierDetail.Command = "Delete";
               _SupplierDetail.SuppCode = "";
               _SupplierDetail.TransType = "Refresh";
                _SupplierDetail.AppStatus = "DL";
               _SupplierDetail.BtnName = "Delete";
                _SupplierDetail.creat_dt = null;
                _SupplierDetail.create_id = null;
                _SupplierDetail.app_status = null;
                _SupplierDetail.mod_id = null;
                _SupplierDetail.mod_dt = null;
                TempData["ModelData"] = _SupplierDetail;
                return RedirectToAction("SupplierDetail", "SupplierDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private ActionResult SupplierApprove(SupplierDetail _SupplierDetail, string command,string ListFilterData1)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                 }
                string supp_id = _SupplierDetail.supp_id;
                string app_id = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _SupplierDetail_ISERVICES.SupplierApprove(_SupplierDetail, comp_id, app_id, supp_id, mac_id);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                //Session["SuppCode"] = _SupplierDetail.supp_id;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'A';
                //Session["BtnName"] = "BtnApprove";
                if(Message== "DuplicateGL")
                {
                    _SupplierDetail.Message = "DuplicateGL";
                    _SupplierDetail.Command = "New";
                    _SupplierDetail.SuppCode = supp_id;
                }
                else
                {
                    _SupplierDetail.TransType = "Update";
                    _SupplierDetail.Command = command;
                    _SupplierDetail.SuppCode = _SupplierDetail.supp_id;
                    _SupplierDetail.Message = "Approved";
                    _SupplierDetail.AppStatus = "A";
                    _SupplierDetail.BtnName = "BtnApprove";
                }
                TempData["ModelData"] = _SupplierDetail;
                TempData["ListFilterData1"] = ListFilterData1;
                return RedirectToAction("SupplierDetail", "SupplierDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private void CommonPageDetails()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, userid, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
                ViewBag.VBRoleList = ds.Tables[3];
                ViewBag.StatusList = ds.Tables[4];

                string[] Docpart = DocumentName.Split('>');
                int len = Docpart.Length;
                if (len > 1)
                {
                    title = Docpart[len - 1].Trim();
                }
                ViewBag.MenuPageName = DocumentName;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable GetSupplierBranchList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _SupplierDetail_ISERVICES.GetBrList(Comp_ID).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetSuppCoa(string SuppCode, string type)
        {
            try
            {
                string Comp_ID = string.Empty;
                string BrchID = string.Empty;
                //string SuppCode = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                //if (Session["SuppCode"] != null)
                //if (SuppCode != null)
                //{
                //    //SuppCode = Session["SuppCode"].ToString();
                //    SuppCode = _SupplierDetail.SuppCode;
                //}
                DataTable dt = _SupplierDetail_ISERVICES.GetSuppcoaDAL(Comp_ID, type, SuppCode);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
          
        }
        [NonAction]
        private DataTable GetBrList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _SupplierDetail_ISERVICES.GetBrListDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        [NonAction]
        private DataTable Getcategory()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _SupplierDetail_ISERVICES.GetcategoryDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        [NonAction]
        private DataTable GetSuppport()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }

                DataTable dt = _SupplierDetail_ISERVICES.GetsuppportDAL(Comp_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
           
        }

        [HttpPost]
        public JsonResult GetCurronSuppType(string Supptype)
        {
            JsonResult DataRows = null;
            try
            {
                List<curr> _SuppcurrList = new List<curr>();
                _SupplierDetail = new SupplierDetail();
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _SupplierDetail_ISERVICES.GetCurronSuppTypeDAL(Comp_ID, Supptype);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
                //foreach (DataRow dr in dt.Rows)
                //{
                //    curr _Suppcurr = new curr();
                //    _Suppcurr.curr_id = dr["curr_id"].ToString();
                //    _Suppcurr.curr_name = dr["curr_name"].ToString();
                //    _SuppcurrList.Add(_Suppcurr);

                //}
                //if (Supptype == "I")
                //{
                //    _SuppcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                //    _SupplierDetail.currList = _SuppcurrList;
                //}
                //else
                //{
                //    _SupplierDetail.currList = _SuppcurrList;
                //}
                //return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialCurrencyDropDown.cshtml", _SupplierDetail);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        [HttpPost]
        public JsonResult CheckDependcyGstno(string Supp_Id, string SupplierGst)
        {
            JsonResult DataRows = null;
            try
            {
              
                _SupplierDetail = new SupplierDetail();
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _SupplierDetail_ISERVICES.CheckDependcyGstno(Comp_ID, Supp_Id, SupplierGst);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/              
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        private DataTable Getcurr(string Supptype)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                Supptype = Supptype == "I" ? "A" : Supptype;//Added by Suraj Maurya on 06-03-2025
                DataTable dt = _SupplierDetail_ISERVICES.GetCurronSuppTypeDAL(Comp_ID, Supptype);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult GetAutoCompleteSuppcity(SearchSupp queryParameters)
        {
            try
            { 
            string GroupName = string.Empty;
            Dictionary<string, string> SuppCity = new Dictionary<string, string>();

            try
            {
                    if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                    {
                        GroupName = "0";
                    }
                    else
                    {
                        GroupName = queryParameters.ddlGroup;
                    }
                    SuppCity = _SupplierDetail_ISERVICES.SuppCityDAL(GroupName);

                //List<SuppCity> _SuppCityList = new List<SuppCity>();
                //foreach (var dr in SuppCity)
                //{
                //    SuppCity _SuppCityName = new SuppCity();
                //    _SuppCityName.city_id = dr.Key;
                //    _SuppCityName.city_name = dr.Value;
                //    _SuppCityList.Add(_SuppCityName);
                //}
                //_SupplierDetail.SuppCityList = _SuppCityList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(SuppCity.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public JsonResult ChakeDependencyAddr(string custId, string addr_id)
        {
            JsonResult DataRows = null;
            try
            {

                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet CDA = _SupplierDetail_ISERVICES.checkDependencyAddr(Comp_ID, custId, addr_id);
                if (CDA.Tables[0].Rows.Count > 0)
                {
                    DataRows = Json("Detail Exists");
                }
                else
                {
                    DataRows = Json("");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;//Json("ErrorPage");
            }
            return DataRows;
        }
        /*----------------------Code start of Country,state,district,city--------------------------*/
        [NonAction]
        private DataTable GetCountryList(string CustType)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _SupplierDetail_ISERVICES.GetCountryListDDL(Comp_ID, CustType);

                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetCountryonChngSuppTyp(string SuppType)
        {
            JsonResult DataRows = null;
            try
            {
                List<CmnCountryList> _TransList = new List<CmnCountryList>();
                _SupplierDetail = new SupplierDetail();
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _SupplierDetail_ISERVICES.GetCountryListDDL(Comp_ID, SuppType);
                if (SuppType == "I")
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    dr[0] = "0";
                    dr[1] = "---Select---";
                    dt.Rows.InsertAt(dr, 0);
                }
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        [HttpPost]
        public JsonResult GetstateOnCountry(string ddlCountryID)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable dt = _SupplierDetail_ISERVICES.GetstateOnCountryDDL(ddlCountryID);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetDistrictOnState(string ddlStateID)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable dt = _SupplierDetail_ISERVICES.GetDistrictOnStateDDL(ddlStateID);
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                dt.Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetCityOnDistrict(string ddlDistrictID)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable dt = _SupplierDetail_ISERVICES.GetCityOnDistrictDDL(ddlDistrictID);
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                dt.Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public JsonResult GetStateCode(string stateId)
        {
            JsonResult DataRows = null;
            try
            {
                DataSet ds = _SupplierDetail_ISERVICES.GetStateCode(stateId);
                DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        /*----------------------Code End of Country,state,district,city--------------------------*/

        [HttpPost]
        [OutputCache(Duration = 0)]
        public ActionResult GetSupplierPurchaseDetail(string FromDate,string ToDate,string Supp_Id)
        {
            SupplierDetail _SupplierDetail = new SupplierDetail();
            ViewBag.VBSupplierList = null;
            string Comp_ID = string.Empty;
            string Br_Id = string.Empty;

            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_Id = Session["BranchId"].ToString();
            }
            try
            {

                DataTable HoCompData = _SupplierDetail_ISERVICES.GetSupplierPurchaseDetail(Comp_ID, Br_Id, Supp_Id, FromDate, ToDate).Tables[0];
                ViewBag.PurchaseHistory = HoCompData;
                //Session["SSearch"] = "S_Search";
                _SupplierDetail.PurchaseHistorySearch = "PurchaseSearch";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialSupplierPurchaseDetail.cshtml", _SupplierDetail);
        }
    }
}