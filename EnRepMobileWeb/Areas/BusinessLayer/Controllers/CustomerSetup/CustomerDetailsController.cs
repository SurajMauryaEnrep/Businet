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
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerDetails;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
//***All Session Remove By Shubham Maurya on 03-01-2023 Using Model to Pass Data***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class CustomerDetailsController : Controller
    {
        string userid, title, CustId = String.Empty;
        string CompID, Br_ID, language = String.Empty;
        string DocumentMenuId = "103125";
        string Comp_ID = String.Empty;
        Common_IServices _Common_IServices;
        CustomerDetails _CustomerDetails;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        CustomerDetails_ISERVICES _CustomerDetails_ISERVICES;
        // GET: BusinessLayer/CustomerDetails
        public CustomerDetailsController(Common_IServices _Common_IServices, CustomerDetails_ISERVICES _CustomerDetails_ISERVICES)
        {
            this._CustomerDetails_ISERVICES = _CustomerDetails_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        private void GetCompIDAndBRID()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();

                Comp_ID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }

        }
        public ActionResult CustomerDetails(CustomerDetails _CustomerDetails1, string custCodeURL, string TransType, string BtnName, string command)
        {
            try
            {
                GetCompIDAndBRID();
                var _CustomerDetails = TempData["ModelData"] as CustomerDetails;
                if (_CustomerDetails != null)
                {
                    SetAllDataTemData(_CustomerDetails);
                    return View("~/Areas/BusinessLayer/Views/CustomerSetup/CustomerDetail.cshtml", _CustomerDetails);
                }
                else
                {
                    SetUrlData(_CustomerDetails1, custCodeURL, TransType, BtnName, command);
                    SetAllDataelse(_CustomerDetails1);                   
                    return View("~/Areas/BusinessLayer/Views/CustomerSetup/CustomerDetail.cshtml", _CustomerDetails1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void SetUrlData(CustomerDetails _CustomerDetails1, string custCodeURL, string TransType, string BtnName, string command)
        {
            try
            {
                var CustCode = "";
                if (custCodeURL != null)
                {
                    CustCode = custCodeURL;
                    _CustomerDetails1.CustCode = custCodeURL;
                }
                else
                {
                    CustCode = _CustomerDetails1.CustCode;
                }
                if (TransType != null)
                {
                    _CustomerDetails1.TransType = TransType;
                }
                if (command != null)
                {
                    _CustomerDetails1.Command = command;
                }
                if (BtnName != null)
                {
                    _CustomerDetails1.BtnName = BtnName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void SetAllDataelse(CustomerDetails _CustomerDetails1)
        {
            try
            {
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                _CustomerDetails1.AttachMentDetailItmStp = null;
                _CustomerDetails1.Guid = null;
                //ViewBag.MenuPageName = getDocumentName();
                //_CustomerDetails1 = new CustomerDetails();


                //_CustomerDetails1.cust_id = null;
                CommonPageDetails();
                _CustomerDetails1.Title = title;


                //CustCode = _CustomerDetails1.CustCode;
                List<CustCoa> _ListCustCoa = new List<CustCoa>();
                ////dt = GetCustcoa(CustCode, "N");
                ////foreach (DataRow dr in dt.Rows)
                ////{
                ////    CustCoa _CustCoa = new CustCoa();
                ////    _CustCoa.acc_id = dr["acc_id"].ToString();
                ////    _CustCoa.acc_name = dr["acc_name"].ToString();
                ////    _ListCustCoa.Add(_CustCoa);
                ////}
                ////_ListCustCoa.Insert(0, new CustCoa() { acc_id = "0", acc_name = "---Select---" });
                ////_CustomerDetails1.CustCoaNameList = _ListCustCoa;
                ///

                GetAllDropdownlist(_CustomerDetails1);
                //dt = GetBrList();
                //List<CustomerBranch> _CustomerBranchList = new List<CustomerBranch>();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    CustomerBranch _CustomerBranch = new CustomerBranch();
                //    _CustomerBranch.comp_id = dr["comp_id"].ToString();
                //    _CustomerBranch.comp_nm = dr["comp_nm"].ToString();
                //    _CustomerBranchList.Add(_CustomerBranch);
                //}                             

                /*Commented start bY Hina on 10-01-2024 10:37 to change country,state,district,city in dropdown instead of textbox */

                //List<CityList> cityLists = new List<CityList>();
                //cityLists.Insert(0, new CityList() { CityId = "0", CityName = "---Select---" });
                //_CustomerDetails1.cityLists = cityLists;

                /*----------Commented End bY Hina on 10-01-2024 10:37 ----*/

                //Session["CustomerFromProspect"] = "N";
                //Session["Prospect_id"] = "0";
                //Session["brid"] = "0";
                _CustomerDetails1.CustomerFromProspect = "N";
                _CustomerDetails1.Prospect_id = "0";
                Session["brid"] = "0";


                GetprospectData(_CustomerDetails1);

                /*------------------------------------------Code Start of Country,state,district,city--------------------------*/

                List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                string CustomerType = "";
                if (_CustomerDetails1.Cust_type == null)
                {
                    //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    CustomerType = "D";
                }
                else
                {
                    CustomerType = _CustomerDetails1.Cust_type;
                    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                }
                List<Country> _ContryList2 = new List<Country>();
                CommonAddress_Detail _Model = new CommonAddress_Detail();

                DataTable dtcntry = GetCountryList(CustomerType);

                foreach (DataRow dr in dtcntry.Rows)
                {
                    CmnCountryList _Contry = new CmnCountryList();
                    _Contry.country_id = dr["country_id"].ToString();
                    _Contry.country_name = dr["country_name"].ToString();
                    _ContryList.Add(_Contry);
                    _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                }
                _Model.countryList = _ContryList2;
                _CustomerDetails1.countryList = _ContryList;

                List<CmnStateList> state = new List<CmnStateList>();
                state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                string transCountry = "";
                if (!string.IsNullOrEmpty(_CustomerDetails1.Country))
                    transCountry = _CustomerDetails1.Country;
                else
                    transCountry = dtcntry.Rows[0]["country_id"].ToString();

                DataTable dtStates = _CustomerDetails_ISERVICES.GetstateOnCountryDDL(transCountry);
                if (dtStates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStates.Rows)
                    {
                        state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                    }
                }
                _CustomerDetails1.StateList = state;

                string transState = "0";
                List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                if (!string.IsNullOrEmpty(_CustomerDetails1.State))
                    transState = _CustomerDetails1.State;
                else
                    transState = "0";
                DataTable dtDist = _CustomerDetails_ISERVICES.GetDistrictOnStateDDL(transState);
                if (dtDist.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDist.Rows)
                    {
                        DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                    }
                }
                _CustomerDetails1.DistrictList = DistList;

                string transDist = "0";
                if (!string.IsNullOrEmpty(_CustomerDetails1.District))
                    transDist = _CustomerDetails1.District;
                else
                    transDist = "0";
                DataTable dtCities = _CustomerDetails_ISERVICES.GetCityOnDistrictDDL(transDist);

                List<CmnCityList> cities = new List<CmnCityList>();
                cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                if (dtCities.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCities.Rows)
                    {
                        cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                    }
                }
                _CustomerDetails1.cityLists = cities;

                _CustomerDetails1._CommonAddress_Detail = _Model;
                /*------------------------------------------Code End of Country,state,district,city--------------------------*/

                //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                if (_CustomerDetails1.TransType == "Update" || _CustomerDetails1.Command == "Edit")
                {
                    //string Custcode = Session["CustCode"].ToString();
                    string Custcode = _CustomerDetails1.CustCode;
                    string Br_Id = string.Empty;
                    if (Session["CompId"] != null)
                    {
                        Comp_ID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        Br_Id = Session["BranchId"].ToString();
                    }
                    Boolean act_stats, tcs_posting, cust_hold = true;

                    DataSet ds = _CustomerDetails_ISERVICES.GetviewCustdetail(Custcode, Comp_ID, Br_Id);
                    ViewBag.AttechmentDetails = ds.Tables[6];

                    if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                        act_stats = true;
                    else
                        act_stats = false;

                    if (ds.Tables[0].Rows[0]["on_hold"].ToString() == "Y")
                        cust_hold = true;
                    else
                        cust_hold = false;
                    if (ds.Tables[0].Rows[0]["tcs_posting"].ToString() == "Y")
                        tcs_posting = true;
                    else
                        tcs_posting = false;
                    ViewBag.VBCustomerSaleshistorylist = ds.Tables[11];
                    ViewBag.licenseDetails = ds.Tables[12];
                    _CustomerDetails1.CustomerDependcy = ds.Tables[10].Rows[0]["customerDependcy"].ToString();
                    _CustomerDetails1.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                    _CustomerDetails1.cust_name = ds.Tables[0].Rows[0]["cust_name"].ToString();
                    _CustomerDetails1.custname = ds.Tables[0].Rows[0]["cust_name"].ToString();
                    _CustomerDetails1.cust_alias = ds.Tables[0].Rows[0]["cust_alias"].ToString();
                    _CustomerDetails1.cust_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                    _CustomerDetails1.Cust_type = ds.Tables[0].Rows[0]["custtype"].ToString();
                    _CustomerDetails1.PaymentAlert = Convert.ToInt32(ds.Tables[0].Rows[0]["paym_alrt"].ToString());
                    if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                    {

                    }
                    else
                    {
                        _CustomerDetails1.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                    }
                    //_CustomerDetails1.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());

                    dt = GetCustcurr(ds.Tables[0].Rows[0]["custtype"].ToString());
                    DataView dv = new DataView(dt);
                    //dv.RowFilter = "curr_id =" + _CustomerDetails1.curr;
                    List<curr> _CustcurrList1 = new List<curr>();
                    foreach (DataRowView dr in dv)
                    {
                        curr _Custcurr = new curr();
                        _Custcurr.curr_id = dr["curr_id"].ToString();
                        _Custcurr.curr_name = dr["curr_name"].ToString();
                        _CustcurrList1.Add(_Custcurr);

                    }
                    _CustomerDetails1.currList = _CustcurrList1;

                    List<CustCoa> _ListCustCoa1 = new List<CustCoa>();
                    ////dt = GetCustcoa(CustCode, "O");
                    ////foreach (DataRow dr in dt.Rows)
                    ////{
                    ////    CustCoa _CustCoa = new CustCoa();
                    ////    _CustCoa.acc_id = dr["acc_id"].ToString();
                    ////    _CustCoa.acc_name = dr["acc_name"].ToString();
                    ////    _ListCustCoa1.Add(_CustCoa);
                    ////}
                    ////_ListCustCoa1.Insert(0, new CustCoa() { acc_id = "0", acc_name = "---Select---" });
                    ////_CustomerDetails1.CustCoaNameList = _ListCustCoa1;

                    _CustomerDetails1.curr = int.Parse(ds.Tables[0].Rows[0]["CurrID"].ToString());
                    _CustomerDetails1.cust_catg = int.Parse(ds.Tables[0].Rows[0]["CatID"].ToString());
                    _CustomerDetails1.cust_port = int.Parse(ds.Tables[0].Rows[0]["PortID"].ToString());
                    _CustomerDetails1.cust_pr_grp = int.Parse(ds.Tables[0].Rows[0]["PriceGrpID"].ToString());
                    _CustomerDetails1.cust_region = int.Parse(ds.Tables[0].Rows[0]["RegionID"].ToString());
                    _CustomerDetails1.cust_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                    _CustomerDetails1.Regn_num = ds.Tables[0].Rows[0]["cust_regn_no"].ToString();
                    _CustomerDetails1.tan_num = ds.Tables[0].Rows[0]["cust_tan_no"].ToString();
                    _CustomerDetails1.pan_num = ds.Tables[0].Rows[0]["cust_pan_no"].ToString();
                    _CustomerDetails1.cont_pers = ds.Tables[0].Rows[0]["cont_pers"].ToString();
                    _CustomerDetails1.cont_email = ds.Tables[0].Rows[0]["cont_email"].ToString();
                    _CustomerDetails1.cont_num1 = ds.Tables[0].Rows[0]["cont_num1"].ToString();
                    //_CustomerDetails1.cont_num2 = ds.Tables[0].Rows[0]["cont_num2"].ToString();
                    _CustomerDetails1.act_stats = act_stats;
                    _CustomerDetails1.app_status = ds.Tables[0].Rows[0]["appstatus"].ToString();
                    _CustomerDetails1.cust_hold = cust_hold;
                    _CustomerDetails1.TCSApplicable = tcs_posting;
                    _CustomerDetails1.inact_reason = ds.Tables[0].Rows[0]["inact_reason"].ToString();
                    _CustomerDetails1.hold_reason = ds.Tables[0].Rows[0]["onhold_reason"].ToString();
                    _CustomerDetails1.cust_rmarks = ds.Tables[0].Rows[0]["cust_remarks"].ToString();
                    _CustomerDetails1.D_InActive = ds.Tables[8].Rows[0]["disableInActive"].ToString();
                    _CustomerDetails1.GLAccountNm = ds.Tables[0].Rows[0]["GLAcc_Name"].ToString();
                    _CustomerDetails1.GlRepoting_Group_ID = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                    _CustomerDetails1.GlRepoting_Group_Name = ds.Tables[0].Rows[0]["gl_rpt_des"].ToString();
                    _CustomerDetails1.GlRepoting_Group = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                    _CustomerDetails1.DefaultTransporter = ds.Tables[0].Rows[0]["def_trns_id"].ToString();
                    _CustomerDetails1.DefaultTransporter_ID = ds.Tables[0].Rows[0]["def_trns_id"].ToString();
                    _CustomerDetails1.cust_zone = ds.Tables[0].Rows[0]["cust_zone"].ToString();
                    _CustomerDetails1.cust_group = ds.Tables[0].Rows[0]["cust_group"].ToString();
                    if (_CustomerDetails1.GlRepoting_Group_ID != "0")
                    {

                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                        GlReportingGroup Glrpt = new GlReportingGroup();
                        Glrpt.Gl_rpt_id = _CustomerDetails1.GlRepoting_Group_ID;
                        Glrpt.Gl_rpt_Name = _CustomerDetails1.GlRepoting_Group_Name;
                        glrpt.Add(Glrpt);

                        _CustomerDetails1.GlReportingGroupList = glrpt;
                    }

                    if (ds.Tables[0].Rows[0]["cre_limit"].ToString() == "")
                    {
                        _CustomerDetails1.credit_limit = 0;
                    }
                    else
                    {
                        _CustomerDetails1.credit_limit = int.Parse(ds.Tables[0].Rows[0]["cre_limit"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["cre_days"].ToString() == "")
                    {
                        _CustomerDetails1.credit_days = 0;
                    }
                    else
                    {
                        _CustomerDetails1.credit_days = int.Parse(ds.Tables[0].Rows[0]["cre_days"].ToString());
                    }
                    _CustomerDetails1.cust_pr_pol = ds.Tables[0].Rows[0]["cust_pr_pol"].ToString();
                    _CustomerDetails1.apply_on = ds.Tables[0].Rows[0]["app_on"].ToString();
                    _CustomerDetails1.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                    _CustomerDetails1.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                    _CustomerDetails1.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                    _CustomerDetails1.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                    _CustomerDetails1.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                    _CustomerDetails1.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                    _CustomerDetails1.Status = ds.Tables[0].Rows[0]["appstatus"].ToString();
                    _CustomerDetails1.bank_name = ds.Tables[0].Rows[0]["bank_name"].ToString();
                    _CustomerDetails1.bank_branch = ds.Tables[0].Rows[0]["bank_branch"].ToString();
                    _CustomerDetails1.bank_add = ds.Tables[0].Rows[0]["bank_add"].ToString();
                    _CustomerDetails1.bank_acc_no = ds.Tables[0].Rows[0]["bank_acc_no"].ToString();
                    _CustomerDetails1.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                    _CustomerDetails1.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                    _CustomerDetails1.Gst_Cat = ds.Tables[0].Rows[0]["gst_cat"].ToString();

                    _CustomerDetails1.HdnInterBranch = ds.Tables[0].Rows[0]["inter_br"].ToString();
                    if (_CustomerDetails1.HdnInterBranch == "Y")
                    {
                        _CustomerDetails1.InterBranch = true;
                    }
                    else
                    {
                        _CustomerDetails1.InterBranch = false;
                    }

                    //ViewBag.CustomerBranchList = ds.Tables[1];
                    _CustomerDetails1.CustomerBranchList = ds.Tables[1];
                    ViewBag.CustomerAddressDetail = ds.Tables[2];
                    ViewBag.CustomerDocumentDetail = ds.Tables[3];
                    //  ViewBag.VBCustomerSaleshistorylist = ds.Tables[7];
                    ViewBag.GLCount = ds.Tables[5].Rows[0]["GlCount"].ToString();
                    if (_CustomerDetails1.app_status == "Approved")
                    {
                        if (ds.Tables[9].Rows[0]["disenbcurr"].ToString() != null && ds.Tables[9].Rows[0]["disenbcurr"].ToString() != "")
                        {
                            _CustomerDetails1.curr_depncy = ds.Tables[9].Rows[0]["disenbcurr"].ToString();
                        }
                        else
                        {
                            _CustomerDetails1.curr_depncy = null;
                        }
                    }
                    /*Commented bY Hina on 10-01-2024 15:38 to change country,state,district,city in dropdown instead of textbox */

                    //foreach (DataRow dr in ds.Tables[4].Rows)
                    //{
                    //    CityList _City_List = new CityList();
                    //    _City_List.CityId = dr["city_id"].ToString();
                    //    _City_List.CityName = dr["city_name"].ToString();
                    //    cityLists.Add(_City_List);
                    //}
                    ////cityLists.Insert(0, new CityList() { CityId = "0", CityName = "---Select---" });
                    //_CustomerDetails1.cityLists = cityLists;

                    /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                    List<CmnCountryList> _ContryListU = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    List<Country> _ContryList2U = new List<Country>();
                    CommonAddress_Detail _ModelU = new CommonAddress_Detail();
                    string CustomerTypeU = "D";
                    if (CustomerTypeU != "D")
                    {
                        _ContryListU.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                    }
                    if (!string.IsNullOrEmpty(_CustomerDetails1.Cust_type))
                        CustomerTypeU = _CustomerDetails1.Cust_type;

                    dt = GetCountryList(CustomerTypeU);

                    foreach (DataRow dr in dt.Rows)
                    {
                        CmnCountryList _Contry = new CmnCountryList();
                        _Contry.country_id = dr["country_id"].ToString();
                        _Contry.country_name = dr["country_name"].ToString();
                        _ContryListU.Add(_Contry);
                        _ContryList2U.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _Model.countryList = _ContryList2U;
                    _CustomerDetails1.countryList = _ContryListU;

                    List<CmnStateList> stateU = new List<CmnStateList>();
                    state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountryU = "";
                    if (!string.IsNullOrEmpty(_CustomerDetails1.Country))
                        transCountryU = _CustomerDetails1.Country;
                    else
                        transCountryU = dt.Rows[0]["country_id"].ToString();

                    dt = _CustomerDetails_ISERVICES.GetstateOnCountryDDL(transCountryU);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            stateU.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    _CustomerDetails1.StateList = stateU;

                    string transStateU = "0";
                    List<CmnDistrictList> DistListU = new List<CmnDistrictList>();
                    DistListU.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(_CustomerDetails1.State))
                        transStateU = _CustomerDetails1.State;
                    else
                        transStateU = "0";
                    dt = _CustomerDetails_ISERVICES.GetDistrictOnStateDDL(transState);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DistListU.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    _CustomerDetails1.DistrictList = DistListU;

                    string transDistU = "0";
                    if (!string.IsNullOrEmpty(_CustomerDetails1.District))
                        transDistU = _CustomerDetails1.District;
                    else
                        transDistU = "0";
                    dt = _CustomerDetails_ISERVICES.GetCityOnDistrictDDL(transDistU);

                    List<CmnCityList> citiesU = new List<CmnCityList>();
                    citiesU.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            citiesU.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    _CustomerDetails1.cityLists = citiesU;

                    _CustomerDetails1._CommonAddress_Detail = _Model;
                    /*------------------------------------------Code End of Country,state,district,city--------------------------*/


                    if (_CustomerDetails1.app_status == "Approved")
                    {
                        //Session["AppStatus"] = 'A';
                        _CustomerDetails1.AppStatus = "A";
                    }
                    else
                    {
                        //Session["AppStatus"] = 'D';
                        _CustomerDetails1.AppStatus = "D";
                    }
                }
                else
                {
                    _CustomerDetails1.act_stats = true;
                }
                if (_CustomerDetails1.BtnName == null)
                {
                    _CustomerDetails1.BtnName = "BtnRefresh";
                }
                if (_CustomerDetails1.TransType == "Update" && _CustomerDetails1.Command == "Edit")
                {
                    _CustomerDetails1.BtnName = "BtnEdit";
                }

                //NOTE--Comment by Hina on 01-12-2022 10:10
                //Comment Session and do it by viewBag bywhich it will be work for prospect and this page
                //ViewBag.FinstDt = Session["FinStDt"].ToString();
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet dsresult = _CustomerDetails_ISERVICES.GetFromDateCust(Comp_ID, Br_ID);
                if (dsresult.Tables[0].Rows.Count > 0)
                {
                    ViewBag.FinstDt = dsresult.Tables[0].Rows[0]["FromDt"].ToString();
                }
                else
                {
                    ViewBag.FinstDt = DateTime.Now.ToString("yyyy-MM-dd");
                }
                //ViewBag.FinstDt = dsresult.Tables[0].Rows[0]["FromDt"].ToString();
                //_CustomerDetails1.TransType = Session["TransType"].ToString();
                _CustomerDetails1.TransType = _CustomerDetails1.TransType;
                _CustomerDetails1.DeleteCommand = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GetprospectData(CustomerDetails _CustomerDetails1)
        {
            try
            {
                if (TempData["ProspectDetail"] != null)
                {
                    DataSet prospectds = new DataSet();
                    //prospectds =(DataSet)Session["ProspectDetail"];
                    prospectds = (DataSet)TempData["ProspectDetail"];
                    _CustomerDetails1.cust_name = prospectds.Tables[0].Rows[0]["pros_name"].ToString();
                    _CustomerDetails1.custname = prospectds.Tables[0].Rows[0]["pros_name"].ToString();
                    _CustomerDetails1.Cust_type = prospectds.Tables[0].Rows[0]["pros_type"].ToString();
                    /*Commented start bY Hina on 10-01-2024 10:37 to change country,state,district,city in dropdown instead of textbox */

                    //foreach (DataRow dr in prospectds.Tables[2].Rows)
                    //{
                    //    CityList _City_List = new CityList();
                    //    _City_List.CityId = dr["city_id"].ToString();
                    //    _City_List.CityName = dr["city_name"].ToString();
                    //    cityLists.Add(_City_List);
                    //}
                    //_CustomerDetails1.cityLists = cityLists;

                    /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                    List<CmnCountryList> _ContryListP = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    List<Country> _ContryList2P = new List<Country>();
                    CommonAddress_Detail _ModelP = new CommonAddress_Detail();
                    string CustomerTypeP = "D";
                    if (CustomerTypeP != "D")
                    {
                        _ContryListP.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                    }
                    if (!string.IsNullOrEmpty(_CustomerDetails1.Cust_type))
                        CustomerTypeP = _CustomerDetails1.Cust_type;

                    dt = GetCountryList(CustomerTypeP);

                    foreach (DataRow dr in dt.Rows)
                    {
                        CmnCountryList _Contry = new CmnCountryList();
                        _Contry.country_id = dr["country_id"].ToString();
                        _Contry.country_name = dr["country_name"].ToString();
                        _ContryListP.Add(_Contry);
                        _ContryList2P.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    //_Model.countryList = _ContryList2P;
                    _CustomerDetails1.countryList = _ContryListP;

                    List<CmnStateList> stateP = new List<CmnStateList>();
                    //stateP.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountryP = "";
                    if (!string.IsNullOrEmpty(_CustomerDetails1.Country))
                        transCountryP = _CustomerDetails1.Country;
                    else
                        transCountryP = dt.Rows[0]["country_id"].ToString();

                    dt = _CustomerDetails_ISERVICES.GetstateOnCountryDDL(transCountryP);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            stateP.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    _CustomerDetails1.StateList = stateP;

                    string transStateP = "0";
                    List<CmnDistrictList> DistListP = new List<CmnDistrictList>();
                    //DistListP.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(_CustomerDetails1.State))
                        transStateP = _CustomerDetails1.State;
                    else
                        transStateP = "0";
                    dt = _CustomerDetails_ISERVICES.GetDistrictOnStateDDL(transStateP);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DistListP.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    _CustomerDetails1.DistrictList = DistListP;
                    //_CustomerDetails1.District = "0";
                    string transDistP = "0";
                    if (!string.IsNullOrEmpty(_CustomerDetails1.District))
                        transDistP = _CustomerDetails1.District;
                    else
                        transDistP = "0";
                    dt = _CustomerDetails_ISERVICES.GetCityOnDistrictDDL(transDistP);

                    List<CmnCityList> citiesP = new List<CmnCityList>();
                    //citiesP.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            citiesP.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    _CustomerDetails1.cityLists = citiesP;
                    //_CustomerDetails1.City = "0";
                    _CustomerDetails1._CommonAddress_Detail = _ModelP;
                    /*------------------------------------------Code End of Country,state,district,city--------------------------*/




                    dt = GetCustcurr(prospectds.Tables[0].Rows[0]["pros_type"].ToString());
                    DataView dv = new DataView(dt);
                    //dv.RowFilter = "curr_id =" + _CustomerDetails1.curr;
                    List<curr> _CustcurrList1 = new List<curr>();
                    foreach (DataRowView dr in dv)
                    {
                        curr _Custcurr = new curr();
                        _Custcurr.curr_id = dr["curr_id"].ToString();
                        _Custcurr.curr_name = dr["curr_name"].ToString();
                        _CustcurrList1.Add(_Custcurr);

                    }
                    _CustomerDetails1.currList = _CustcurrList1;

                    _CustomerDetails1.curr = int.Parse(prospectds.Tables[0].Rows[0]["curr_id"].ToString());
                    _CustomerDetails1.cont_pers = prospectds.Tables[0].Rows[0]["cont_person"].ToString();
                    _CustomerDetails1.cont_email = prospectds.Tables[0].Rows[0]["cont_email"].ToString();
                    _CustomerDetails1.cont_num1 = prospectds.Tables[0].Rows[0]["cont_num"].ToString();
                    _CustomerDetails1.Gst_Cat = prospectds.Tables[0].Rows[0]["gst_cat"].ToString();
                    _CustomerDetails1.cust_rmarks = prospectds.Tables[0].Rows[0]["remarks"].ToString();

                    ViewBag.ProsCustomerAddressDetail = prospectds.Tables[0];
                    //ViewBag.CustomerBranchList = prospectds.Tables[3];
                    _CustomerDetails1.CustomerBranchList = prospectds.Tables[3];

                    //Session["CustomerFromProspect"] = "Y";
                    //Session["Prospect_id"] = prospectds.Tables[0].Rows[0]["pros_id"].ToString();
                    _CustomerDetails1.CustomerFromProspect = "Y";
                    _CustomerDetails1.Prospect_id = prospectds.Tables[0].Rows[0]["pros_id"].ToString();
                    Session["brid"] = prospectds.Tables[0].Rows[0]["br_id"].ToString();
                    TempData["ProspectDetail"] = null;
                    //Session["ProspectDetail"] = null;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private void SetAllDataTemData(CustomerDetails _CustomerDetails)
        {
            try
            {
                _CustomerDetails.AttachMentDetailItmStp = null;
                _CustomerDetails.Guid = null;
                _CustomerDetails.cust_id = null;
                CommonPageDetails();
                _CustomerDetails.Title = title;

                //ViewBag.CustomerBranchList = dt;
                //  _CustomerDetails.CustomerBranchList = dt;
                var CustCode = _CustomerDetails.CustCode;

                GetAllDropdownlist(_CustomerDetails);

                string Language = string.Empty;

                /*Commented bY Hina on 10-01-2024 14:04 to change country,state,district,city in dropdown instead of textbox */

                //List<CityList> cityLists = new List<CityList>();
                //cityLists.Insert(0, new CityList() { CityId = "0", CityName = "---Select---" });
                //_CustomerDetails.cityLists = cityLists;

                /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                string CustomerType = "";
                if (_CustomerDetails.Cust_type == null)
                {
                    CustomerType = "D";
                }
                else
                {
                    CustomerType = _CustomerDetails.Cust_type;
                    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                }
                //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                List<Country> _ContryList2 = new List<Country>();
                CommonAddress_Detail _Model = new CommonAddress_Detail();
                //string CustomerType = "D";
                //if (CustomerType != "D")
                //{
                //    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                //}
                //if (!string.IsNullOrEmpty(_CustomerDetails.Cust_type))
                //    CustomerType = _CustomerDetails.Cust_type;


                DataTable dtcntry = GetCountryList(CustomerType);

                foreach (DataRow dr in dtcntry.Rows)
                {
                    CmnCountryList _Contry = new CmnCountryList();
                    _Contry.country_id = dr["country_id"].ToString();
                    _Contry.country_name = dr["country_name"].ToString();
                    _ContryList.Add(_Contry);
                    _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                }
                _Model.countryList = _ContryList2;
                _CustomerDetails.countryList = _ContryList;


                List<CmnStateList> state = new List<CmnStateList>();
                state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                string transCountry = "";
                if (!string.IsNullOrEmpty(_CustomerDetails.Country))
                    transCountry = _CustomerDetails.Country;
                else
                    transCountry = dtcntry.Rows[0]["country_id"].ToString();

                DataTable dtStates = _CustomerDetails_ISERVICES.GetstateOnCountryDDL(transCountry);
                if (dtStates.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtStates.Rows)
                    {
                        state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                    }
                }
                _CustomerDetails.StateList = state;

                string transState = "0";
                List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                if (!string.IsNullOrEmpty(_CustomerDetails.State))
                    transState = _CustomerDetails.State;
                else
                    transState = "0";
                DataTable dtDist = _CustomerDetails_ISERVICES.GetDistrictOnStateDDL(transState);
                if (dtDist.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDist.Rows)
                    {
                        DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                    }
                }
                _CustomerDetails.DistrictList = DistList;


                string transDist = "0";
                if (!string.IsNullOrEmpty(_CustomerDetails.District))
                    transDist = _CustomerDetails.District;
                else
                    transDist = "0";
                DataTable dtCities = _CustomerDetails_ISERVICES.GetCityOnDistrictDDL(transDist);

                List<CmnCityList> cities = new List<CmnCityList>();
                cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                if (dtCities.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCities.Rows)
                    {
                        cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                    }
                }
                _CustomerDetails.cityLists = cities;
                //_CustomerDetails.City = "0";
                _CustomerDetails._CommonAddress_Detail = _Model;
                /*------------------------------------------Code End of Country,state,district,city--------------------------*/
                _CustomerDetails.CustomerFromProspect = "N";
                _CustomerDetails.Prospect_id = "0";
                Session["brid"] = "0";
                GetprospectData(_CustomerDetails);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    _CustomerDetails.ListFilterData1 = TempData["ListFilterData"].ToString();
                }

                //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                if (_CustomerDetails.TransType == "Update" || _CustomerDetails.Command == "Edit")
                {
                    //string Custcode = Session["CustCode"].ToString();
                    string Custcode = _CustomerDetails.CustCode;
                    Boolean act_stats, tcs_posting, cust_hold = true;

                    DataSet ds = _CustomerDetails_ISERVICES.GetviewCustdetail(Custcode, CompID, Br_ID);
                    ViewBag.AttechmentDetails = ds.Tables[6];

                    if (ds.Tables[0].Rows[0]["act_status"].ToString() == "Y")
                        act_stats = true;
                    else
                        act_stats = false;

                    if (ds.Tables[0].Rows[0]["on_hold"].ToString() == "Y")
                        cust_hold = true;
                    else
                        cust_hold = false;
                    if (ds.Tables[0].Rows[0]["tcs_posting"].ToString() == "Y")
                        tcs_posting = true;
                    else
                        tcs_posting = false;
                    ViewBag.VBCustomerSaleshistorylist = ds.Tables[11];
                    ViewBag.licenseDetails = ds.Tables[12];
                    _CustomerDetails.CustomerDependcy = ds.Tables[10].Rows[0]["customerDependcy"].ToString();
                    _CustomerDetails.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                    _CustomerDetails.cust_name = ds.Tables[0].Rows[0]["cust_name"].ToString();
                    _CustomerDetails.custname = ds.Tables[0].Rows[0]["cust_name"].ToString();
                    _CustomerDetails.cust_alias = ds.Tables[0].Rows[0]["cust_alias"].ToString();
                    _CustomerDetails.cust_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                    _CustomerDetails.Cust_type = ds.Tables[0].Rows[0]["custtype"].ToString();
                    _CustomerDetails.PaymentAlert = Convert.ToInt32(ds.Tables[0].Rows[0]["paym_alrt"].ToString());
                    if (ds.Tables[0].Rows[0]["acc_grp_id"].ToString() == "")
                    {

                    }
                    else
                    {
                        _CustomerDetails.acc_grp_id = int.Parse(ds.Tables[0].Rows[0]["acc_grp_id"].ToString());
                    }
                    _CustomerDetails.GlRepoting_Group_ID = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                    _CustomerDetails.GlRepoting_Group_Name = ds.Tables[0].Rows[0]["gl_rpt_des"].ToString();
                    _CustomerDetails.GlRepoting_Group = ds.Tables[0].Rows[0]["gl_rpt_id"].ToString();
                    _CustomerDetails.DefaultTransporter = ds.Tables[0].Rows[0]["def_trns_id"].ToString();
                    _CustomerDetails.DefaultTransporter_ID = ds.Tables[0].Rows[0]["def_trns_id"].ToString();
                    _CustomerDetails.cust_zone = ds.Tables[0].Rows[0]["cust_zone"].ToString();
                    _CustomerDetails.cust_group = ds.Tables[0].Rows[0]["cust_group"].ToString();
                    if (_CustomerDetails.GlRepoting_Group_ID != "0")
                    {
                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                        GlReportingGroup Glrpt = new GlReportingGroup();
                        Glrpt.Gl_rpt_id = _CustomerDetails.GlRepoting_Group_ID;
                        Glrpt.Gl_rpt_Name = _CustomerDetails.GlRepoting_Group_Name;
                        glrpt.Add(Glrpt);

                        _CustomerDetails.GlReportingGroupList = glrpt;
                    }

                    dt = GetCustcurr(ds.Tables[0].Rows[0]["custtype"].ToString());
                    DataView dv = new DataView(dt);
                    //dv.RowFilter = "curr_id =" + _CustomerDetails.curr;
                    List<curr> _CustcurrList1 = new List<curr>();
                    foreach (DataRowView dr in dv)
                    {
                        curr _Custcurr = new curr();
                        _Custcurr.curr_id = dr["curr_id"].ToString();
                        _Custcurr.curr_name = dr["curr_name"].ToString();
                        _CustcurrList1.Add(_Custcurr);

                    }
                    _CustomerDetails.currList = _CustcurrList1;
                    CustCode = _CustomerDetails.CustCode;
                    List<CustCoa> _ListCustCoa1 = new List<CustCoa>();
                    _CustomerDetails.curr = int.Parse(ds.Tables[0].Rows[0]["CurrID"].ToString());
                    _CustomerDetails.cust_catg = int.Parse(ds.Tables[0].Rows[0]["CatID"].ToString());
                    _CustomerDetails.cust_port = int.Parse(ds.Tables[0].Rows[0]["PortID"].ToString());
                    _CustomerDetails.cust_pr_grp = int.Parse(ds.Tables[0].Rows[0]["PriceGrpID"].ToString());
                    _CustomerDetails.cust_region = int.Parse(ds.Tables[0].Rows[0]["RegionID"].ToString());
                    _CustomerDetails.cust_coa = int.Parse(ds.Tables[0].Rows[0]["CoaId"].ToString());
                    _CustomerDetails.Regn_num = ds.Tables[0].Rows[0]["cust_regn_no"].ToString();
                    _CustomerDetails.tan_num = ds.Tables[0].Rows[0]["cust_tan_no"].ToString();
                    _CustomerDetails.pan_num = ds.Tables[0].Rows[0]["cust_pan_no"].ToString();
                    _CustomerDetails.cont_pers = ds.Tables[0].Rows[0]["cont_pers"].ToString();
                    _CustomerDetails.cont_email = ds.Tables[0].Rows[0]["cont_email"].ToString();
                    _CustomerDetails.cont_num1 = ds.Tables[0].Rows[0]["cont_num1"].ToString();
                    //_CustomerDetails.cont_num2 = ds.Tables[0].Rows[0]["cont_num2"].ToString();
                    _CustomerDetails.act_stats = act_stats;
                    _CustomerDetails.app_status = ds.Tables[0].Rows[0]["appstatus"].ToString();
                    _CustomerDetails.cust_hold = cust_hold;
                    _CustomerDetails.TCSApplicable = tcs_posting;
                    _CustomerDetails.inact_reason = ds.Tables[0].Rows[0]["inact_reason"].ToString();
                    _CustomerDetails.hold_reason = ds.Tables[0].Rows[0]["onhold_reason"].ToString();
                    _CustomerDetails.cust_rmarks = ds.Tables[0].Rows[0]["cust_remarks"].ToString();
                    _CustomerDetails.GLAccountNm = ds.Tables[0].Rows[0]["GLAcc_Name"].ToString();
                    _CustomerDetails.D_InActive = ds.Tables[8].Rows[0]["disableInActive"].ToString();
                    if (ds.Tables[0].Rows[0]["cre_limit"].ToString() == "")
                    {
                        _CustomerDetails.credit_limit = 0;
                    }
                    else
                    {
                        _CustomerDetails.credit_limit = float.Parse(ds.Tables[0].Rows[0]["cre_limit"].ToString());
                    }
                    if (ds.Tables[0].Rows[0]["cre_days"].ToString() == "")
                    {
                        _CustomerDetails.credit_days = 0;
                    }
                    else
                    {
                        _CustomerDetails.credit_days = int.Parse(ds.Tables[0].Rows[0]["cre_days"].ToString());
                    }
                    _CustomerDetails.cust_pr_pol = ds.Tables[0].Rows[0]["cust_pr_pol"].ToString();
                    _CustomerDetails.apply_on = ds.Tables[0].Rows[0]["app_on"].ToString();
                    _CustomerDetails.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                    _CustomerDetails.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                    _CustomerDetails.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                    _CustomerDetails.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                    _CustomerDetails.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                    _CustomerDetails.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                    _CustomerDetails.Status = ds.Tables[0].Rows[0]["appstatus"].ToString();
                    _CustomerDetails.bank_name = ds.Tables[0].Rows[0]["bank_name"].ToString();
                    _CustomerDetails.bank_branch = ds.Tables[0].Rows[0]["bank_branch"].ToString();
                    _CustomerDetails.bank_add = ds.Tables[0].Rows[0]["bank_add"].ToString();
                    _CustomerDetails.bank_acc_no = ds.Tables[0].Rows[0]["bank_acc_no"].ToString();
                    _CustomerDetails.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                    _CustomerDetails.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                    _CustomerDetails.Gst_Cat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                    _CustomerDetails.HdnInterBranch = ds.Tables[0].Rows[0]["inter_br"].ToString();
                    if (_CustomerDetails.HdnInterBranch == "Y")
                    {
                        _CustomerDetails.InterBranch = true;
                    }
                    else
                    {
                        _CustomerDetails.InterBranch = false;
                    }
                    if (_CustomerDetails.app_status == "Approved")
                    {
                        if (ds.Tables[9].Rows[0]["disenbcurr"].ToString() != null && ds.Tables[9].Rows[0]["disenbcurr"].ToString() != "")
                        {
                            _CustomerDetails.curr_depncy = ds.Tables[9].Rows[0]["disenbcurr"].ToString();
                        }
                        else
                        {
                            _CustomerDetails.curr_depncy = null;
                        }
                    }
                    //ViewBag.CustomerBranchList = ds.Tables[1];
                    _CustomerDetails.CustomerBranchList = ds.Tables[1];
                    ViewBag.CustomerAddressDetail = ds.Tables[2];
                    ViewBag.CustomerDocumentDetail = ds.Tables[3];
                    // ViewBag.VBCustomerSaleshistorylist = ds.Tables[7];
                    ViewBag.GLCount = ds.Tables[5].Rows[0]["GlCount"].ToString();
                    /*Commented start bY Hina on 10-01-2024 15:37 to change country,state,district,city in dropdown instead of textbox */

                    //foreach (DataRow dr in ds.Tables[4].Rows)
                    //{
                    //    CityList _City_List = new CityList();
                    //    _City_List.CityId = dr["city_id"].ToString();
                    //    _City_List.CityName = dr["city_name"].ToString();
                    //    cityLists.Add(_City_List);
                    //}
                    //_CustomerDetails.cityLists = cityLists;
                    /*Commented End bY Hina on 10-01-2024 15:37 to change country,state,district,city in dropdown instead of textbox */

                    //if (_CustomerDetails.app_status == null)
                    //{
                    //    _CustomerDetails.app_status = _CustomerDetails.Status;
                    //}
                    if (_CustomerDetails.app_status == "Approved")
                    {
                        //Session["AppStatus"] = 'A';
                        _CustomerDetails.AppStatus = "A";
                    }
                    else
                    {
                        //Session["AppStatus"] = 'D';
                        _CustomerDetails.AppStatus = "D";
                    }
                }
                else
                {
                    _CustomerDetails.cust_pr_pol = "M";
                    _CustomerDetails.act_stats = true;
                }
                /*------------------------------------------Code Start of Country--------------------------*/

                List<CmnCountryList> _ContryListU = new List<CmnCountryList>();
                string CustomerTypeU = "";
                if (_CustomerDetails.Cust_type == null || _CustomerDetails.Cust_type == "D")
                {
                    CustomerTypeU = "D";
                }
                else
                {
                    CustomerTypeU = _CustomerDetails.Cust_type;
                    _ContryListU.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                }
                //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                List<Country> _ContryList2U = new List<Country>();
                CommonAddress_Detail _ModelU = new CommonAddress_Detail();


                dt = GetCountryList(CustomerTypeU);

                foreach (DataRow dr in dt.Rows)
                {
                    CmnCountryList _Contry = new CmnCountryList();
                    _Contry.country_id = dr["country_id"].ToString();
                    _Contry.country_name = dr["country_name"].ToString();
                    _ContryListU.Add(_Contry);
                    _ContryList2U.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                }
                _ModelU.countryList = _ContryList2U;
                _CustomerDetails.countryList = _ContryListU;
                /*------------------------------------------Code End of Country--------------------------*/

                //NOTE--Comment by Hina on 01-12-2022 10:10
                //Comment Session and do it by viewBag bywhich it will be work for prospect and this page
                //ViewBag.FinstDt = Session["FinStDt"].ToString();

                DataSet dsresult = _CustomerDetails_ISERVICES.GetFromDateCust(CompID, Br_ID);
                if (dsresult.Tables[0].Rows.Count > 0)
                {
                    ViewBag.FinstDt = dsresult.Tables[0].Rows[0]["FromDt"].ToString();
                }
                else
                {
                    ViewBag.FinstDt = DateTime.Now.ToString("yyyy-MM-dd");
                }
                //_CustomerDetails.TransType = Session["TransType"].ToString();
                _CustomerDetails.TransType = _CustomerDetails.TransType;
                _CustomerDetails.DeleteCommand = null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        private void GetAllDropdownlist(CustomerDetails _CustomerDetails)
        {
            try
            {
                GetCompIDAndBRID();
                DataSet Table = _CustomerDetails_ISERVICES.GetAllDropDownList(Comp_ID, Br_ID);
                /******************Bind Catergory DropDown**********************/
                List<Category> _CategoryList = new List<Category>();
                //  dt = Getcategory();
                foreach (DataRow dr in Table.Tables[0].Rows)
                {
                    Category _Category = new Category();
                    _Category.setup_id = dr["setup_id"].ToString();
                    _Category.setup_val = dr["setup_val"].ToString();
                    _CategoryList.Add(_Category);
                }
                _CustomerDetails.CategoryList = _CategoryList;
                /************************End*******************************/

                /********************Bind Portfolio *****************************************/
                List<PortFolio> _PortFolioList = new List<PortFolio>();
                // dt = GetCustport();
                foreach (DataRow dr in Table.Tables[1].Rows)
                {
                    PortFolio _PortFolio = new PortFolio();
                    _PortFolio.setup_id = dr["setup_id"].ToString();
                    _PortFolio.setup_val = dr["setup_val"].ToString();
                    _PortFolioList.Add(_PortFolio);
                }
                _CustomerDetails.PortFolioList = _PortFolioList;

                /************************End***************************************************/
                /************************Bind Currency data******************************************************/
                List<curr> _CustcurrList = new List<curr>();
                // dt = GetCustcurr("D");
                foreach (DataRow dr in Table.Tables[2].Rows)
                {
                    curr _Custcurr = new curr();
                    _Custcurr.curr_id = dr["curr_id"].ToString();
                    _Custcurr.curr_name = dr["curr_name"].ToString();
                    _CustcurrList.Add(_Custcurr);

                }
                if (_CustcurrList.Count == 0)//Codition Added by Suraj on 16-10-2024
                {
                    _CustcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                }
                _CustomerDetails.currList = _CustcurrList;
                /****************************End**************************************************************************/

                /***********************************Get Region Dropdownlist**************************************************************/
                //dt = GetRegion();
                List<Region> _RegionList = new List<Region>();
                foreach (DataRow dt in Table.Tables[3].Rows)
                {
                    Region _Region = new Region();
                    _Region.setup_id = dt["setup_id"].ToString();
                    _Region.setup_val = dt["setup_val"].ToString();
                    _RegionList.Add(_Region);
                }
                _CustomerDetails.RegionList = _RegionList;

                /*******************************************End***************************************************************************/

                /***************************************Price DropDownList**********************************************************************************/
                //dt = GetCustPriceGrp();
                List<PriceGroup> _PriceGroupList = new List<PriceGroup>();
                foreach (DataRow dr in Table.Tables[4].Rows)
                {
                    PriceGroup _PriceGroup = new PriceGroup();
                    _PriceGroup.setup_id = dr["setup_id"].ToString();
                    _PriceGroup.setup_val = dr["setup_val"].ToString();
                    _PriceGroupList.Add(_PriceGroup);
                }
                _CustomerDetails.PriceGroupList = _PriceGroupList;

                /************************************************End*****************************************************************************************/
                /*************************************************Get Account Group***********************************************************************************/

                //  dt = GetAccountGroup("cust");
                List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                foreach (DataRow dt in Table.Tables[5].Rows)
                {
                    AccountGroup _AccountGroup = new AccountGroup();
                    _AccountGroup.acc_grp_id = dt["acc_grp_id"].ToString();
                    _AccountGroup.AccGroupChildNood = dt["AccGroupChildNood"].ToString();
                    _AccountGroupList.Add(_AccountGroup);
                }
                _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                _CustomerDetails.AccountGroupList = _AccountGroupList;

                /*******************************************End******************************************************/

                List<GlReportingGroup> _ItemList = new List<GlReportingGroup>();

                GlReportingGroup Glrpt = new GlReportingGroup();
                Glrpt.Gl_rpt_id = "0";
                Glrpt.Gl_rpt_Name = "---Select---";
                _ItemList.Add(Glrpt);

                _CustomerDetails.GlReportingGroupList = _ItemList;
                //dt = GetCustomerBranchList();
                //ViewBag.CustomerBranchList = dt;
                _CustomerDetails.CustomerBranchList = Table.Tables[6];

                _CustomerDetails.FromDate = Table.Tables[9].Rows[0]["fin_st_date"].ToString();

                List<ListDefaultTransporter> _LisTrans = new List<ListDefaultTransporter>();
                foreach (DataRow dt in Table.Tables[10].Rows)
                {
                    ListDefaultTransporter _list = new ListDefaultTransporter();
                    _list.Transporter_id = dt["trans_id"].ToString();
                    _list.Transporter_val = dt["trans_name"].ToString();
                    _LisTrans.Add(_list);
                }
                _LisTrans.Insert(0, new ListDefaultTransporter() { Transporter_id = "0", Transporter_val = "---Select---" });
                _CustomerDetails.DefaultTransporterList = _LisTrans;


                List<custzone> _custzonelist = new List<custzone>();
                foreach (DataRow dr in Table.Tables[12].Rows)
                {
                    custzone _custzone = new custzone();
                    _custzone.custzone_id = dr["setup_id"].ToString();
                    _custzone.custzone_val = dr["setup_val"].ToString();
                    _custzonelist.Add(_custzone);
                }

                _custzonelist.Insert(0, new custzone() { custzone_id = "0", custzone_val = "---Select---" });
                _CustomerDetails.custzoneList = _custzonelist;


                List<custgroup> _custgrouplist = new List<custgroup>();
                foreach (DataRow dr in Table.Tables[13].Rows)
                {
                    custgroup _custgroup = new custgroup();
                    _custgroup.CustGrp_id = dr["setup_id"].ToString();
                    _custgroup.CustGrp_val = dr["setup_val"].ToString();
                    _custgrouplist.Add(_custgroup);
                }

                _custgrouplist.Insert(0, new custgroup() { CustGrp_id = "0", CustGrp_val = "---Select---" });
                _CustomerDetails.cust_groupList = _custgrouplist;

                _CustomerDetails.CompCountry = Table.Tables[11].Rows[0]["country_name"].ToString();
                _CustomerDetails.CompCountryID = Table.Tables[11].Rows[0]["country_id"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void CommonPageDetails()
        {
            try
            {
                GetCompIDAndBRID();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, userid, DocumentMenuId, language);
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
        [NonAction]
        private DataTable GetAccountGroup(string type)
        {
            try
            {
                GetCompIDAndBRID();
                DataTable dt = _CustomerDetails_ISERVICES.GetAccountGroupDAL(CompID, type);
                return dt;
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
                GetCompIDAndBRID();
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

        public ActionResult CustomerSave(CustomerDetails _CustomerDetails, string command, string cust_id, HttpPostedFileBase[] CustomerFiles)
        {
            try
            {
                if (_CustomerDetails.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Edit":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["CustCode"] = _CustomerDetails.cust_id;
                        //Session["TransType"] = "Update";
                        //Session["BtnName"] = "BtnEdit";
                        _CustomerDetails.Message = "";
                        _CustomerDetails.Command = command;
                        _CustomerDetails.SaveAndApproveBtnDisatble = null;
                        _CustomerDetails.CustCode = _CustomerDetails.cust_id;
                        _CustomerDetails.TransType = "Update";
                        _CustomerDetails.BtnName = "BtnEdit";

                        var TransType = "Update";
                        var BtnName = "BtnEdit";
                        var custCodeURL = _CustomerDetails.cust_id;
                        TempData["ModelData"] = _CustomerDetails;
                        TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                        return (RedirectToAction("CustomerDetails", new { custCodeURL = custCodeURL, TransType, BtnName, command }));

                    case "Add":
                        CustomerDetails _CustomerDetailAdd = new CustomerDetails();
                        //_CustomerDetails.Message = "";
                        //_CustomerDetails.CustCode = "";
                        _CustomerDetailAdd.SaveAndApproveBtnDisatble = null;
                        _CustomerDetailAdd.AppStatus = "D";
                        _CustomerDetailAdd.TransType = "Save";
                        _CustomerDetailAdd.BtnName = "BtnAddNew";
                        _CustomerDetailAdd.Command = command;
                        //_CustomerDetails.CreatedBy = null;
                        //_CustomerDetails.CreatedOn = null;
                        //_CustomerDetails.ApprovedBy = null;
                        //_CustomerDetails.ApprovedOn = null;
                        //_CustomerDetails.AmmendedBy = null;
                        //_CustomerDetails.AmmendedOn = null;
                        //_CustomerDetails.Status = null;
                        //_CustomerDetails.hold_reason = null;
                        //_CustomerDetails.cust_rmarks = null;
                        //_CustomerDetails.ListFilterData1 = null;   
                        TempData["ModelData"] = _CustomerDetailAdd;
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("CustomerDetails", "CustomerDetails");


                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Delete";
                        cust_id = _CustomerDetails.cust_id;
                        _CustomerDetails.SaveAndApproveBtnDisatble = null;
                        _CustomerDetails.Command = command;
                        _CustomerDetails.BtnName = "Delete";
                        CustomerDetailDelete(_CustomerDetails, cust_id);
                        TempData["ModelData"] = _CustomerDetails;
                        TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                        return RedirectToAction("CustomerDetails");

                    case "Save":
                        //Session["Command"] = command;
                        _CustomerDetails.Command = command;
                        if (_CustomerDetails.Status == "Approved")
                        {
                            _CustomerDetails.AppStatus = "A";
                        }
                        if (_CustomerDetails.Status != "Approved")
                        {
                            _CustomerDetails.AppStatus = "D";
                        }
                        if (ModelState.IsValid)
                        {
                            InsertCustomerDetail(_CustomerDetails);

                            _CustomerDetails.CustCode = _CustomerDetails.CustCode;
                            if (_CustomerDetails.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            if (_CustomerDetails.Message == "Duplicate" || _CustomerDetails.Message == "DuplicateGL")
                            {
                                var _CustomerDetailsattch = TempData["ModelDataattch"] as CustomerDetailsattch;
                                if (_CustomerDetailsattch != null)
                                {
                                    ViewBag.AttechmentDetails = _CustomerDetailsattch.AttachMentDetailItmStp;
                                }
                                dt = GetCustomerBranchList();
                                //ViewBag.CustomerBranchList = dt;
                                _CustomerDetails.CustomerBranchList = dt;
                                CommonPageDetails();
                                var CustCode = _CustomerDetails.CustCode;
                                List<CustCoa> _ListCustCoa = new List<CustCoa>();
                                ////dt = GetCustcoa(CustCode,"O");
                                ////foreach (DataRow dr in dt.Rows)
                                ////{
                                ////    CustCoa _CustCoa = new CustCoa();
                                ////    _CustCoa.acc_id = dr["acc_id"].ToString();
                                ////    _CustCoa.acc_name = dr["acc_name"].ToString();
                                ////    _ListCustCoa.Add(_CustCoa);
                                ////}
                                ////_CustomerDetails.CustCoaNameList = _ListCustCoa;

                                dt = GetAccountGroup("cust");
                                List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    AccountGroup _AccountGroup = new AccountGroup();
                                    _AccountGroup.acc_grp_id = dt["acc_grp_id"].ToString();
                                    _AccountGroup.AccGroupChildNood = dt["AccGroupChildNood"].ToString();
                                    _AccountGroupList.Add(_AccountGroup);
                                }
                                _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                                _CustomerDetails.AccountGroupList = _AccountGroupList;

                                List<Category> _CategoryList = new List<Category>();
                                dt = Getcategory();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    Category _Category = new Category();
                                    _Category.setup_id = dr["setup_id"].ToString();
                                    _Category.setup_val = dr["setup_val"].ToString();
                                    _CategoryList.Add(_Category);
                                }
                                _CustomerDetails.CategoryList = _CategoryList;
                                List<PortFolio> _PortFolioList = new List<PortFolio>();
                                dt = GetCustport();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    PortFolio _PortFolio = new PortFolio();
                                    _PortFolio.setup_id = dr["setup_id"].ToString();
                                    _PortFolio.setup_val = dr["setup_val"].ToString();
                                    _PortFolioList.Add(_PortFolio);
                                }
                                _CustomerDetails.PortFolioList = _PortFolioList;

                                List<curr> _CustcurrList = new List<curr>();
                                dt = GetCustcurr("D");
                                foreach (DataRow dr in dt.Rows)
                                {
                                    curr _Custcurr = new curr();
                                    _Custcurr.curr_id = dr["curr_id"].ToString();
                                    _Custcurr.curr_name = dr["curr_name"].ToString();
                                    _CustcurrList.Add(_Custcurr);

                                }
                                dt = GetCustcurr("I");
                                foreach (DataRow dr in dt.Rows)
                                {
                                    curr _Custcurr = new curr();
                                    _Custcurr.curr_id = dr["curr_id"].ToString();
                                    _Custcurr.curr_name = dr["curr_name"].ToString();
                                    _CustcurrList.Add(_Custcurr);

                                }
                                //_CustcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                                _CustomerDetails.currList = _CustcurrList;
                                dt = GetRegion();
                                List<Region> _RegionList = new List<Region>();
                                foreach (DataRow dt in dt.Rows)
                                {
                                    Region _Region = new Region();
                                    _Region.setup_id = dt["setup_id"].ToString();
                                    _Region.setup_val = dt["setup_val"].ToString();
                                    _RegionList.Add(_Region);
                                }
                                _CustomerDetails.RegionList = _RegionList;
                                dt = GetCustPriceGrp();
                                List<PriceGroup> _PriceGroupList = new List<PriceGroup>();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    PriceGroup _PriceGroup = new PriceGroup();
                                    _PriceGroup.setup_id = dr["setup_id"].ToString();
                                    _PriceGroup.setup_val = dr["setup_val"].ToString();
                                    _PriceGroupList.Add(_PriceGroup);
                                }
                                _CustomerDetails.PriceGroupList = _PriceGroupList;
                                //dt = GetBrList();
                                //List<CustomerBranch> _CustomerBranchList = new List<CustomerBranch>();
                                //foreach (DataRow dr in dt.Rows)
                                //{
                                //    CustomerBranch _CustomerBranch = new CustomerBranch();
                                //    _CustomerBranch.comp_id = dr["comp_id"].ToString();
                                //    _CustomerBranch.comp_nm = dr["comp_nm"].ToString();
                                //    _CustomerBranchList.Add(_CustomerBranch);
                                //}
                                //string Custcode = Session["CustCode"].ToString();
                                string Custcode = _CustomerDetails.CustCode;

                                if (Session["CompId"] != null)
                                {
                                    Comp_ID = Session["CompId"].ToString();
                                }
                                /*Commented By Hina on 10-01-2023 to bind on chng country data*/
                                //List<CityList> cityLists = new List<CityList>();
                                //cityLists.Insert(0, new CityList() { CityId = "0", CityName = "---Select---" });
                                //_CustomerDetails.cityLists = cityLists;
                                //DataSet ds = _CustomerDetails_ISERVICES.GetviewCustdetail(Custcode, Comp_ID);
                                //foreach (DataRow dr in ds.Tables[4].Rows)
                                //{
                                //    CityList _City_List = new CityList();
                                //    _City_List.CityId = dr["city_id"].ToString();
                                //    _City_List.CityName = dr["city_name"].ToString();
                                //    cityLists.Add(_City_List);
                                //}
                                ////cityLists.Insert(0, new CityList() { CityId = "0", CityName = "---Select---" });
                                //_CustomerDetails.cityLists = cityLists;
                                /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                                List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                                string CustomerType = "";
                                if (_CustomerDetails.Cust_type == null)
                                {
                                    CustomerType = "D";
                                }
                                else
                                {
                                    CustomerType = _CustomerDetails.Cust_type;
                                    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                                }
                                //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                                //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                                List<Country> _ContryList2 = new List<Country>();
                                CommonAddress_Detail _Model = new CommonAddress_Detail();
                                //string CustomerType = "D";
                                //if (CustomerType != "D")
                                //{
                                //    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                                //}
                                //if (!string.IsNullOrEmpty(_CustomerDetails.supp_type))
                                //    CustomerType = _CustomerDetails.supp_type;


                                DataTable dtcntry = GetCountryList(CustomerType);

                                foreach (DataRow dr in dtcntry.Rows)
                                {
                                    CmnCountryList _Contry = new CmnCountryList();
                                    _Contry.country_id = dr["country_id"].ToString();
                                    _Contry.country_name = dr["country_name"].ToString();
                                    _ContryList.Add(_Contry);
                                    _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                                }
                                _Model.countryList = _ContryList2;
                                _CustomerDetails.countryList = _ContryList;

                                List<CmnStateList> state = new List<CmnStateList>();
                                state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                                string transCountry = "";
                                if (!string.IsNullOrEmpty(_CustomerDetails.Country))
                                    transCountry = _CustomerDetails.Country;
                                else
                                    transCountry = dtcntry.Rows[0]["country_id"].ToString();

                                DataTable dtStates = _CustomerDetails_ISERVICES.GetstateOnCountryDDL(transCountry);
                                if (dtStates.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtStates.Rows)
                                    {
                                        state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                                    }
                                }
                                _CustomerDetails.StateList = state;

                                string transState = "0";
                                List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                                DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                                if (!string.IsNullOrEmpty(_CustomerDetails.State))
                                    transState = _CustomerDetails.State;
                                else
                                    transState = "0";
                                DataTable dtDist = _CustomerDetails_ISERVICES.GetDistrictOnStateDDL(transState);
                                if (dtDist.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtDist.Rows)
                                    {
                                        DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                                    }
                                }
                                _CustomerDetails.DistrictList = DistList;
                                // _CustomerDetails.District = "0";

                                string transDist = "0";
                                if (!string.IsNullOrEmpty(_CustomerDetails.District))
                                    transDist = _CustomerDetails.District;
                                else
                                    transDist = "0";
                                DataTable dtCities = _CustomerDetails_ISERVICES.GetCityOnDistrictDDL(transDist);

                                List<CmnCityList> cities = new List<CmnCityList>();
                                cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                                if (dtCities.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtCities.Rows)
                                    {
                                        cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                                    }
                                }
                                _CustomerDetails.cityLists = cities;
                                //_CustomerDetails.City = "0";
                                _CustomerDetails._CommonAddress_Detail = _Model;
                                /*------------------------------------------Code End of Country,state,district,city--------------------------*/
                                /*Commented By Hina on 10-01-2024 to bind all data on chnge country*/



                                //DataSet dset = GetsuppCities(_CustomerDetails);
                                //List<CmnCityList> suppCities = new List<CmnCityList>();
                                //foreach (DataRow dr in dset.Tables[4].Rows)
                                //{
                                //    SuppCity suppCity = new SuppCity();
                                //    suppCity.city_id = dr["city_id"].ToString();
                                //    suppCity.city_name = dr["city_name"].ToString();
                                //    suppCities.Add(suppCity);
                                //}
                                //suppCities.Insert(0, new SuppCity() { city_id = "0", city_name = "---Select---" });
                                //_CustomerDetails.SuppCityList = suppCities;
                                string Br_Id = string.Empty;
                                if (_CustomerDetails.CustCode != "0")
                                {
                                    if (Session["CompId"] != null)
                                    {
                                        Comp_ID = Session["CompId"].ToString();
                                    }
                                    if (Session["BranchId"] != null)
                                    {
                                        Br_Id = Session["BranchId"].ToString();
                                    }
                                    Boolean act_stats, cust_hold = true;

                                    DataSet dst = _CustomerDetails_ISERVICES.GetviewCustdetail(Custcode, Comp_ID, Br_Id);
                                    ViewBag.AttechmentDetails = dst.Tables[6];
                                    _CustomerDetails.CustomerBranchList = dst.Tables[1];
                                    _CustomerDetails.CreatedBy = dst.Tables[0].Rows[0]["create_id"].ToString();
                                    _CustomerDetails.CreatedOn = dst.Tables[0].Rows[0]["create_dt"].ToString();
                                    _CustomerDetails.ApprovedBy = dst.Tables[0].Rows[0]["app_id"].ToString();
                                    _CustomerDetails.ApprovedOn = dst.Tables[0].Rows[0]["app_dt"].ToString();
                                    _CustomerDetails.AmmendedBy = dst.Tables[0].Rows[0]["mod_id"].ToString();
                                    _CustomerDetails.AmmendedOn = dst.Tables[0].Rows[0]["mod_dt"].ToString();
                                    _CustomerDetails.Status = dst.Tables[0].Rows[0]["appstatus"].ToString();
                                    _CustomerDetails.acc_grp_id = int.Parse(dst.Tables[0].Rows[0]["acc_grp_id"].ToString());
                                    ViewBag.CustomerAddressDetail = dst.Tables[2];
                                    if (_CustomerDetails.GlRepoting_Group_ID != "0" && _CustomerDetails.GlRepoting_Group_ID != null)
                                    {
                                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                                        GlReportingGroup Glrpt = new GlReportingGroup();
                                        Glrpt.Gl_rpt_id = _CustomerDetails.GlRepoting_Group_ID;
                                        Glrpt.Gl_rpt_Name = _CustomerDetails.GlRepoting_Group_Name;
                                        glrpt.Add(Glrpt);

                                        _CustomerDetails.GlReportingGroupList = glrpt;
                                    }
                                    else
                                    {
                                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();
                                        GlReportingGroup Glrpt = new GlReportingGroup();
                                        Glrpt.Gl_rpt_id = "0";
                                        Glrpt.Gl_rpt_Name = "---Select---";
                                        glrpt.Add(Glrpt);

                                        _CustomerDetails.GlReportingGroupList = glrpt;
                                    }

                                }
                                else
                                {
                                    if (_CustomerDetails.GlRepoting_Group_ID != "0" && _CustomerDetails.GlRepoting_Group_ID != null)
                                    {
                                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                                        GlReportingGroup Glrpt = new GlReportingGroup();
                                        Glrpt.Gl_rpt_id = _CustomerDetails.GlRepoting_Group_ID;
                                        Glrpt.Gl_rpt_Name = _CustomerDetails.GlRepoting_Group_Name;
                                        glrpt.Add(Glrpt);

                                        _CustomerDetails.GlReportingGroupList = glrpt;
                                    }
                                    else
                                    {
                                        List<GlReportingGroup> glrpt = new List<GlReportingGroup>();
                                        GlReportingGroup Glrpt = new GlReportingGroup();
                                        Glrpt.Gl_rpt_id = "0";
                                        Glrpt.Gl_rpt_Name = "---Select---";
                                        glrpt.Add(Glrpt);

                                        _CustomerDetails.GlReportingGroupList = glrpt;
                                    }
                                }
                                GetCompIDAndBRID();
                                DataSet Table = _CustomerDetails_ISERVICES.GetAllDropDownList(Comp_ID, Br_ID);
                                List<ListDefaultTransporter> _LisTrans = new List<ListDefaultTransporter>();
                                foreach (DataRow dt in Table.Tables[10].Rows)
                                {
                                    ListDefaultTransporter _list = new ListDefaultTransporter();
                                    _list.Transporter_id = dt["trans_id"].ToString();
                                    _list.Transporter_val = dt["trans_name"].ToString();
                                    _LisTrans.Add(_list);
                                }
                                _LisTrans.Insert(0, new ListDefaultTransporter() { Transporter_id = "0", Transporter_val = "---Select---" });
                                _CustomerDetails.DefaultTransporterList = _LisTrans;
                              

                                if (_CustomerDetails.DefaultTransporter_ID != "0" && _CustomerDetails.DefaultTransporter_ID != null)
                                {
                                    _CustomerDetails.DefaultTransporter = _CustomerDetails.DefaultTransporter_ID;
                                }

                                List<custzone> _custzonelist = new List<custzone>();
                                foreach (DataRow dr in Table.Tables[12].Rows)
                                {
                                    custzone _custzone = new custzone();
                                    _custzone.custzone_id = dr["setup_id"].ToString();
                                    _custzone.custzone_val = dr["setup_val"].ToString();
                                    _custzonelist.Add(_custzone);
                                }

                                _custzonelist.Insert(0, new custzone() { custzone_id = "0", custzone_val = "---Select---" });
                                _CustomerDetails.custzoneList = _custzonelist;
                                if (_CustomerDetails.cust_zone != "0" && _CustomerDetails.cust_zone != null)
                                {
                                    _CustomerDetails.cust_zone = _CustomerDetails.cust_zone;
                                }

                                List<custgroup> _custgrouplist = new List<custgroup>();
                                foreach (DataRow dr in Table.Tables[13].Rows)
                                {
                                    custgroup _custgroup = new custgroup();
                                    _custgroup.CustGrp_id = dr["setup_id"].ToString();
                                    _custgroup.CustGrp_val = dr["setup_val"].ToString();
                                    _custgrouplist.Add(_custgroup);
                                }

                                _custgrouplist.Insert(0, new custgroup() { CustGrp_id = "0", CustGrp_val = "---Select---" });
                                _CustomerDetails.cust_groupList = _custgrouplist;

                                if (_CustomerDetails.cust_group != "0" && _CustomerDetails.cust_group != null)
                                {
                                    _CustomerDetails.cust_group = _CustomerDetails.cust_group;
                                }
                                //Session["CustCode"] = "";
                                //Session["AppStatus"] = "D";
                                _CustomerDetails.CustCode = "";
                                _CustomerDetails.AppStatus = "D";
                                //Commented by Hina on 09-12-2022 to save data after duplicate 
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                _CustomerDetails.BtnName = "BtnAddNew";
                                //ViewBag.Message = Session["Message"].ToString();
                                _CustomerDetails.Message = _CustomerDetails.Message;
                                //ViewBag.VBRoleList = GetRoleList();
                                //_CustomerDetails.act_stats = true;
                                _CustomerDetails.SaveAndApproveBtnDisatble = null;
                                TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                                return View("~/Areas/BusinessLayer/Views/CustomerSetup/CustomerDetail.cshtml", _CustomerDetails);
                            }
                            else
                            {
                                custCodeURL = _CustomerDetails.CustCode;
                                TransType = _CustomerDetails.TransType;
                                BtnName = _CustomerDetails.BtnName;
                                _CustomerDetails.SaveAndApproveBtnDisatble = null;
                                TempData["ModelData"] = _CustomerDetails;
                                TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                                return (RedirectToAction("CustomerDetails", new { custCodeURL = custCodeURL, TransType, BtnName, command }));
                            }

                        }
                        else
                        {
                            _CustomerDetails = null;
                            TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                            return View("~/Areas/BusinessLayer/Views/CustomerSetup/CustomerDetail.cshtml", _CustomerDetails);
                        }

                    case "Forward":
                        return new EmptyResult();
                    case "Approve":
                        _CustomerDetails.Command = command;
                        cust_id = _CustomerDetails.cust_id;
                        _CustomerDetails.CustCode = cust_id;
                        CustomerApprove(_CustomerDetails, command, "");
                        if (_CustomerDetails.Message == "DuplicateGL")
                        {
                            var _CustomerDetailsattch = TempData["ModelDataattch"] as CustomerDetailsattch;
                            if (_CustomerDetailsattch != null)
                            {
                                ViewBag.AttechmentDetails = _CustomerDetailsattch.AttachMentDetailItmStp;
                            }
                            dt = GetCustomerBranchList();
                            //ViewBag.CustomerBranchList = dt;
                            _CustomerDetails.CustomerBranchList = dt;
                            CommonPageDetails();
                            var CustCode = _CustomerDetails.CustCode;
                            List<CustCoa> _ListCustCoa = new List<CustCoa>();
                            dt = GetAccountGroup("cust");
                            List<AccountGroup> _AccountGroupList = new List<AccountGroup>();
                            foreach (DataRow dt in dt.Rows)
                            {
                                AccountGroup _AccountGroup = new AccountGroup();
                                _AccountGroup.acc_grp_id = dt["acc_grp_id"].ToString();
                                _AccountGroup.AccGroupChildNood = dt["AccGroupChildNood"].ToString();
                                _AccountGroupList.Add(_AccountGroup);
                            }
                            _AccountGroupList.Insert(0, new AccountGroup() { acc_grp_id = "0", AccGroupChildNood = "---Select---" });
                            _CustomerDetails.AccountGroupList = _AccountGroupList;

                            List<Category> _CategoryList = new List<Category>();
                            dt = Getcategory();
                            foreach (DataRow dr in dt.Rows)
                            {
                                Category _Category = new Category();
                                _Category.setup_id = dr["setup_id"].ToString();
                                _Category.setup_val = dr["setup_val"].ToString();
                                _CategoryList.Add(_Category);
                            }
                            _CustomerDetails.CategoryList = _CategoryList;
                            List<PortFolio> _PortFolioList = new List<PortFolio>();
                            dt = GetCustport();
                            foreach (DataRow dr in dt.Rows)
                            {
                                PortFolio _PortFolio = new PortFolio();
                                _PortFolio.setup_id = dr["setup_id"].ToString();
                                _PortFolio.setup_val = dr["setup_val"].ToString();
                                _PortFolioList.Add(_PortFolio);
                            }
                            _CustomerDetails.PortFolioList = _PortFolioList;

                            List<curr> _CustcurrList = new List<curr>();
                            dt = GetCustcurr("D");
                            foreach (DataRow dr in dt.Rows)
                            {
                                curr _Custcurr = new curr();
                                _Custcurr.curr_id = dr["curr_id"].ToString();
                                _Custcurr.curr_name = dr["curr_name"].ToString();
                                _CustcurrList.Add(_Custcurr);
                            }
                            dt = GetCustcurr("I");
                            foreach (DataRow dr in dt.Rows)
                            {
                                curr _Custcurr = new curr();
                                _Custcurr.curr_id = dr["curr_id"].ToString();
                                _Custcurr.curr_name = dr["curr_name"].ToString();
                                _CustcurrList.Add(_Custcurr);
                            }
                            _CustomerDetails.currList = _CustcurrList;
                            dt = GetRegion();
                            List<Region> _RegionList = new List<Region>();
                            foreach (DataRow dt in dt.Rows)
                            {
                                Region _Region = new Region();
                                _Region.setup_id = dt["setup_id"].ToString();
                                _Region.setup_val = dt["setup_val"].ToString();
                                _RegionList.Add(_Region);
                            }
                            _CustomerDetails.RegionList = _RegionList;
                            dt = GetCustPriceGrp();
                            List<PriceGroup> _PriceGroupList = new List<PriceGroup>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                PriceGroup _PriceGroup = new PriceGroup();
                                _PriceGroup.setup_id = dr["setup_id"].ToString();
                                _PriceGroup.setup_val = dr["setup_val"].ToString();
                                _PriceGroupList.Add(_PriceGroup);
                            }
                            _CustomerDetails.PriceGroupList = _PriceGroupList;
                            string Custcode = _CustomerDetails.CustCode;

                            if (Session["CompId"] != null)
                            {
                                Comp_ID = Session["CompId"].ToString();
                            }
                            /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                            List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                            string CustomerType = "";
                            if (_CustomerDetails.Cust_type == null)
                            {
                                CustomerType = "D";
                            }
                            else
                            {
                                CustomerType = _CustomerDetails.Cust_type;
                                _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                            }
                            //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                            //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                            List<Country> _ContryList2 = new List<Country>();
                            CommonAddress_Detail _Model = new CommonAddress_Detail();
                            //string CustomerType = "D";
                            //if (CustomerType != "D")
                            //{
                            //    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                            //}
                            //if (!string.IsNullOrEmpty(_CustomerDetails.supp_type))
                            //    CustomerType = _CustomerDetails.supp_type;


                            DataTable dtcntry = GetCountryList(CustomerType);

                            foreach (DataRow dr in dtcntry.Rows)
                            {
                                CmnCountryList _Contry = new CmnCountryList();
                                _Contry.country_id = dr["country_id"].ToString();
                                _Contry.country_name = dr["country_name"].ToString();
                                _ContryList.Add(_Contry);
                                _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                            }
                            _Model.countryList = _ContryList2;
                            _CustomerDetails.countryList = _ContryList;

                            List<CmnStateList> state = new List<CmnStateList>();
                            state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                            string transCountry = "";
                            if (!string.IsNullOrEmpty(_CustomerDetails.Country))
                                transCountry = _CustomerDetails.Country;
                            else
                                transCountry = dtcntry.Rows[0]["country_id"].ToString();

                            DataTable dtStates = _CustomerDetails_ISERVICES.GetstateOnCountryDDL(transCountry);
                            if (dtStates.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtStates.Rows)
                                {
                                    state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                                }
                            }
                            _CustomerDetails.StateList = state;

                            string transState = "0";
                            List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                            DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                            if (!string.IsNullOrEmpty(_CustomerDetails.State))
                                transState = _CustomerDetails.State;
                            else
                                transState = "0";
                            DataTable dtDist = _CustomerDetails_ISERVICES.GetDistrictOnStateDDL(transState);
                            if (dtDist.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtDist.Rows)
                                {
                                    DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                                }
                            }
                            _CustomerDetails.DistrictList = DistList;
                            // _CustomerDetails.District = "0";

                            string transDist = "0";
                            if (!string.IsNullOrEmpty(_CustomerDetails.District))
                                transDist = _CustomerDetails.District;
                            else
                                transDist = "0";
                            DataTable dtCities = _CustomerDetails_ISERVICES.GetCityOnDistrictDDL(transDist);

                            List<CmnCityList> cities = new List<CmnCityList>();
                            cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                            if (dtCities.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtCities.Rows)
                                {
                                    cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                                }
                            }
                            _CustomerDetails.cityLists = cities;
                            //_CustomerDetails.City = "0";
                            _CustomerDetails._CommonAddress_Detail = _Model;
                            /*------------------------------------------Code End of Country,state,district,city--------------------------*/
                            /*Commented By Hina on 10-01-2024 to bind all data on chnge country*/
                            //List<CityList> cityLists = new List<CityList>();
                            //cityLists.Insert(0, new CityList() { CityId = "0", CityName = "---Select---" });
                            //_CustomerDetails.cityLists = cityLists;
                            //DataSet ds = _CustomerDetails_ISERVICES.GetviewCustdetail(Custcode, Comp_ID);
                            //foreach (DataRow dr in ds.Tables[4].Rows)
                            //{
                            //    CityList _City_List = new CityList();
                            //    _City_List.CityId = dr["city_id"].ToString();
                            //    _City_List.CityName = dr["city_name"].ToString();
                            //    cityLists.Add(_City_List);
                            //}
                            //_CustomerDetails.cityLists = cityLists;
                            if (_CustomerDetails.CustCode != "0")
                            {
                                string Br_Id = string.Empty;
                                if (Session["CompId"] != null)
                                {
                                    Comp_ID = Session["CompId"].ToString();
                                }
                                if (Session["BranchId"] != null)
                                {
                                    Br_Id = Session["BranchId"].ToString();
                                }
                                Boolean act_stats, cust_hold = true;

                                DataSet dst = _CustomerDetails_ISERVICES.GetviewCustdetail(Custcode, Comp_ID, Br_Id);
                                ViewBag.AttechmentDetails = dst.Tables[6];
                                _CustomerDetails.CustomerBranchList = dst.Tables[1];
                                _CustomerDetails.CreatedBy = dst.Tables[0].Rows[0]["create_id"].ToString();
                                _CustomerDetails.CreatedOn = dst.Tables[0].Rows[0]["create_dt"].ToString();
                                _CustomerDetails.ApprovedBy = dst.Tables[0].Rows[0]["app_id"].ToString();
                                _CustomerDetails.ApprovedOn = dst.Tables[0].Rows[0]["app_dt"].ToString();
                                _CustomerDetails.AmmendedBy = dst.Tables[0].Rows[0]["mod_id"].ToString();
                                _CustomerDetails.AmmendedOn = dst.Tables[0].Rows[0]["mod_dt"].ToString();
                                _CustomerDetails.Status = dst.Tables[0].Rows[0]["appstatus"].ToString();
                                _CustomerDetails.GLAccountNm = dst.Tables[0].Rows[0]["GLAcc_Name"].ToString();
                                _CustomerDetails.cust_name = dst.Tables[0].Rows[0]["cust_name"].ToString();
                                _CustomerDetails.acc_grp_id = int.Parse(dst.Tables[0].Rows[0]["acc_grp_id"].ToString());
                                ViewBag.CustomerAddressDetail = dst.Tables[2];
                                if (_CustomerDetails.GlRepoting_Group_ID != "0" && _CustomerDetails.GlRepoting_Group_ID != null)
                                {
                                    List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                                    GlReportingGroup Glrpt = new GlReportingGroup();
                                    Glrpt.Gl_rpt_id = _CustomerDetails.GlRepoting_Group_ID;
                                    Glrpt.Gl_rpt_Name = _CustomerDetails.GlRepoting_Group_Name;
                                    glrpt.Add(Glrpt);

                                    _CustomerDetails.GlReportingGroupList = glrpt;
                                }
                                else
                                {
                                    List<GlReportingGroup> glrpt = new List<GlReportingGroup>();
                                    GlReportingGroup Glrpt = new GlReportingGroup();
                                    Glrpt.Gl_rpt_id = "0";
                                    Glrpt.Gl_rpt_Name = "---Select---";
                                    glrpt.Add(Glrpt);

                                    _CustomerDetails.GlReportingGroupList = glrpt;
                                }
                            }
                            else
                            {
                                if (_CustomerDetails.GlRepoting_Group_ID != "0" && _CustomerDetails.GlRepoting_Group_ID != null)
                                {
                                    List<GlReportingGroup> glrpt = new List<GlReportingGroup>();

                                    GlReportingGroup Glrpt = new GlReportingGroup();
                                    Glrpt.Gl_rpt_id = _CustomerDetails.GlRepoting_Group_ID;
                                    Glrpt.Gl_rpt_Name = _CustomerDetails.GlRepoting_Group_Name;
                                    glrpt.Add(Glrpt);

                                    _CustomerDetails.GlReportingGroupList = glrpt;
                                }
                                else
                                {
                                    List<GlReportingGroup> glrpt = new List<GlReportingGroup>();
                                    GlReportingGroup Glrpt = new GlReportingGroup();
                                    Glrpt.Gl_rpt_id = "0";
                                    Glrpt.Gl_rpt_Name = "---Select---";
                                    glrpt.Add(Glrpt);

                                    _CustomerDetails.GlReportingGroupList = glrpt;
                                }
                            }
                            GetCompIDAndBRID();
                            DataSet Table = _CustomerDetails_ISERVICES.GetAllDropDownList(Comp_ID, Br_ID);
                            List<ListDefaultTransporter> _LisTrans = new List<ListDefaultTransporter>();
                            foreach (DataRow dt in Table.Tables[10].Rows)
                            {
                                ListDefaultTransporter _list = new ListDefaultTransporter();
                                _list.Transporter_id = dt["trans_id"].ToString();
                                _list.Transporter_val = dt["trans_name"].ToString();
                                _LisTrans.Add(_list);
                            }
                            _LisTrans.Insert(0, new ListDefaultTransporter() { Transporter_id = "0", Transporter_val = "---Select---" });
                            _CustomerDetails.DefaultTransporterList = _LisTrans;
                          

                            if (_CustomerDetails.DefaultTransporter_ID != "0" && _CustomerDetails.DefaultTransporter_ID != null)
                            {
                                _CustomerDetails.DefaultTransporter = _CustomerDetails.DefaultTransporter_ID;
                            }
                            List<custzone> _custzonelist = new List<custzone>();
                            foreach (DataRow dr in Table.Tables[12].Rows)
                            {
                                custzone _custzone = new custzone();
                                _custzone.custzone_id = dr["setup_id"].ToString();
                                _custzone.custzone_val = dr["setup_val"].ToString();
                                _custzonelist.Add(_custzone);
                            }

                            _custzonelist.Insert(0, new custzone() { custzone_id = "0", custzone_val = "---Select---" });
                            _CustomerDetails.custzoneList = _custzonelist;
                            if (_CustomerDetails.cust_zone != "0" && _CustomerDetails.cust_zone != null)
                            {
                                _CustomerDetails.cust_zone = _CustomerDetails.cust_zone;
                            }

                            List<custgroup> _custgrouplist = new List<custgroup>();
                            foreach (DataRow dr in Table.Tables[13].Rows)
                            {
                                custgroup _custgroup = new custgroup();
                                _custgroup.CustGrp_id = dr["setup_id"].ToString();
                                _custgroup.CustGrp_val = dr["setup_val"].ToString();
                                _custgrouplist.Add(_custgroup);
                            }

                            _custgrouplist.Insert(0, new custgroup() { CustGrp_id = "0", CustGrp_val = "---Select---" });
                            _CustomerDetails.cust_groupList = _custgrouplist;

                            if (_CustomerDetails.cust_group != "0" && _CustomerDetails.cust_group != null)
                            {
                                _CustomerDetails.cust_group = _CustomerDetails.cust_group;
                            }
                            _CustomerDetails.CustCode = "";
                            _CustomerDetails.AppStatus = "D";
                            _CustomerDetails.BtnName = "BtnToDetailPage";
                            _CustomerDetails.Command = "Add";
                            _CustomerDetails.TransType = "Update";
                            _CustomerDetails.SaveAndApproveBtnDisatble = null;
                            _CustomerDetails.Message = _CustomerDetails.Message;
                            TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                            return View("~/Areas/BusinessLayer/Views/CustomerSetup/CustomerDetail.cshtml", _CustomerDetails);
                        }
                        custCodeURL = _CustomerDetails.cust_id;
                        TransType = _CustomerDetails.TransType;
                        BtnName = _CustomerDetails.BtnName;
                        _CustomerDetails.SaveAndApproveBtnDisatble = null;
                        TempData["ModelData"] = _CustomerDetails;
                        TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                        return (RedirectToAction("CustomerDetails", new { custCodeURL = custCodeURL, TransType, BtnName, command }));


                    case "Refresh":
                        CustomerDetails _CustomerDetailRefresh = new CustomerDetails();
                        _CustomerDetailRefresh.BtnName = "BtnRefresh";
                        _CustomerDetailRefresh.Command = command;
                        _CustomerDetailRefresh.SaveAndApproveBtnDisatble = null;
                        _CustomerDetailRefresh.TransType = "Refresh";
                        TempData["ModelData"] = _CustomerDetailRefresh;
                        TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                        return RedirectToAction("CustomerDetails");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        TempData["ListFilterData"] = _CustomerDetails.ListFilterData1;
                        return RedirectToAction("CustomerList", "CustomerSetup");

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
        public  ActionResult InsertCustomerDetail(CustomerDetails _CustomerDetails)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            try
            {
                GetCompIDAndBRID();
                DataTable CustomerDetail = new DataTable();
                DataTable CustomerBranch = new DataTable();
                DataTable CustomerAttachments = new DataTable();
                DataTable CustomerAddress = new DataTable();
                DataTable LicenceDetail = new DataTable();

                DataTable dt = new DataTable();
                dt.Columns.Add("TransType", typeof(string));
                dt.Columns.Add("CustomerFromProspect", typeof(string));
                dt.Columns.Add("Prospect_id", typeof(string));
                dt.Columns.Add("brid", typeof(string));
                dt.Columns.Add("cust_id", typeof(int));
                dt.Columns.Add("cust_name", typeof(string));
                dt.Columns.Add("cust_alias", typeof(string));
                dt.Columns.Add("cust_type", typeof(string));
                dt.Columns.Add("cust_catg", typeof(int));
                dt.Columns.Add("cust_port", typeof(int));
                dt.Columns.Add("cust_pr_grp", typeof(int));
                dt.Columns.Add("cust_pr_pol", typeof(string));
                dt.Columns.Add("cust_bill_add", typeof(string));
                dt.Columns.Add("cust_city", typeof(int));
                dt.Columns.Add("cust_state", typeof(int));
                dt.Columns.Add("cust_cntry", typeof(int));
                dt.Columns.Add("cust_region", typeof(string));
                dt.Columns.Add("cust_ship_add", typeof(string));
                dt.Columns.Add("cust_coa", typeof(int));
                dt.Columns.Add("cust_remarks", typeof(string));
                dt.Columns.Add("act_status", typeof(string));
                dt.Columns.Add("inact_reason", typeof(string));
                dt.Columns.Add("on_hold", typeof(string));
                dt.Columns.Add("onhold_reason", typeof(string));
                dt.Columns.Add("create_id", typeof(int));
                dt.Columns.Add("mod_id", typeof(int));
                dt.Columns.Add("app_status", typeof(string));
                dt.Columns.Add("def_curr_id", typeof(int));
                dt.Columns.Add("cust_regn_no", typeof(string));
                dt.Columns.Add("cust_gst_no", typeof(string));
                dt.Columns.Add("cust_tan_no", typeof(string));
                dt.Columns.Add("cust_pan_no", typeof(string));
                dt.Columns.Add("cont_pers", typeof(string));
                dt.Columns.Add("cont_email", typeof(string));
                dt.Columns.Add("cont_num1", typeof(string));
                dt.Columns.Add("UserMacaddress", typeof(string));
                dt.Columns.Add("UserSystemName", typeof(string));
                dt.Columns.Add("UserIP", typeof(string));
                //dt.Columns.Add("cont_num2", typeof(string));
                dt.Columns.Add("cre_limit", typeof(float));
                dt.Columns.Add("cre_days", typeof(int));
                dt.Columns.Add("app_on", typeof(int));
                dt.Columns.Add("comp_id", typeof(int));
                dt.Columns.Add("cust_dist", typeof(int));
                dt.Columns.Add("sales_rep", typeof(int));
                dt.Columns.Add("bank_name", typeof(string));
                dt.Columns.Add("bank_branch", typeof(string));
                dt.Columns.Add("bank_add", typeof(string));
                dt.Columns.Add("bank_acc_no", typeof(string));
                dt.Columns.Add("ifsc_code", typeof(string));
                dt.Columns.Add("swift_code", typeof(string));
                dt.Columns.Add("gst_cat", typeof(string));
                dt.Columns.Add("acc_grp_id", typeof(string));
                dt.Columns.Add("acc_name", typeof(string));
                dt.Columns.Add("tcs_posting", typeof(string));
                dt.Columns.Add("gl_rpt_id", typeof(int));
                dt.Columns.Add("def_trns_id", typeof(int));
                dt.Columns.Add("inter_br", typeof(char));
                dt.Columns.Add("cust_zone", typeof(int));
                dt.Columns.Add("cust_group", typeof(int));
                DataRow dtrow = dt.NewRow();
                //var transtype = Session["TransType"].ToString();
                //dtrow["TransType"] = Session["TransType"].ToString();
                var transtype = _CustomerDetails.TransType;
                dtrow["TransType"] = _CustomerDetails.TransType;
                //if (Session["CustomerFromProspect"] != null)
                //{
                //    dtrow["CustomerFromProspect"] = Session["CustomerFromProspect"].ToString();
                //    dtrow["Prospect_id"] = Session["Prospect_id"].ToString();
                //    dtrow["brid"] = Session["brid"].ToString();
                //}
                //else
                //{
                //    dtrow["CustomerFromProspect"] = "";// Session["CustomerFromProspect"].ToString();
                //    dtrow["Prospect_id"] = "";// Session["Prospect_id"].ToString();
                //    dtrow["brid"] = "";//Session["brid"].ToString();
                //}
                if (_CustomerDetails.CustomerFromProspect != null)
                {
                    //dtrow["CustomerFromProspect"] = Session["CustomerFromProspect"].ToString();
                    //dtrow["Prospect_id"] = Session["Prospect_id"].ToString();
                    dtrow["CustomerFromProspect"] = _CustomerDetails.CustomerFromProspect;
                    dtrow["Prospect_id"] = _CustomerDetails.Prospect_id;
                    dtrow["brid"] = Session["brid"].ToString();
                }
                else
                {
                    dtrow["CustomerFromProspect"] = "";// Session["CustomerFromProspect"].ToString();
                    dtrow["Prospect_id"] = "";// Session["Prospect_id"].ToString();
                    dtrow["brid"] = "";//Session["brid"].ToString();
                }



                if (!string.IsNullOrEmpty(_CustomerDetails.cust_id))
                {
                    dtrow["cust_id"] = _CustomerDetails.cust_id;
                }
                else
                {
                    dtrow["cust_id"] = "0";
                }
                if (_CustomerDetails.cust_name == null || _CustomerDetails.cust_name == "")
                {
                    dtrow["cust_name"] = _CustomerDetails.custname;
                }
                else
                {
                    dtrow["cust_name"] = _CustomerDetails.cust_name;
                }
                dtrow["cust_alias"] = _CustomerDetails.cust_alias;
                dtrow["cust_type"] = _CustomerDetails.Cust_type;
                dtrow["cust_catg"] = _CustomerDetails.cust_catg;
                dtrow["cust_port"] = _CustomerDetails.cust_port;
                dtrow["cust_pr_grp"] = _CustomerDetails.cust_pr_grp;
                dtrow["cust_pr_pol"] = _CustomerDetails.cust_pr_pol;
                dtrow["cust_bill_add"] = _CustomerDetails.customer_billing_address;
                dtrow["cust_city"] = 0/*_CustomerDetails.City*/;
                dtrow["cust_state"] = 0/*_CustomerDetails.State*/;
                dtrow["cust_cntry"] = 0/*_CustomerDetails.Country*/;
                dtrow["cust_region"] = _CustomerDetails.cust_region;
                dtrow["cust_ship_add"] = _CustomerDetails.cust_ship_add1;
                dtrow["cust_coa"] = _CustomerDetails.cust_coa;
                dtrow["cust_remarks"] = _CustomerDetails.cust_rmarks;

                //if (Session["TransType"].ToString() == "Save")
                if (_CustomerDetails.TransType == "Save")
                {
                    dtrow["act_status"] = "Y";
                }
                else
                {
                    if (_CustomerDetails.act_stats)
                    {
                        dtrow["act_status"] = "Y";
                    }
                    else
                    {
                        dtrow["act_status"] = "N";
                    }
                }
                dtrow["inact_reason"] = _CustomerDetails.inact_reason;
                if (_CustomerDetails.cust_hold)
                {
                    dtrow["on_hold"] = "Y";
                }
                else
                {
                    dtrow["on_hold"] = "N";
                }
                dtrow["onhold_reason"] = _CustomerDetails.hold_reason;
                dtrow["create_id"] = Session["UserId"].ToString();
                dtrow["mod_id"] = Session["UserId"].ToString();
                //dtrow["app_status"] = Session["AppStatus"].ToString();
                dtrow["app_status"] = _CustomerDetails.AppStatus;
                dtrow["def_curr_id"] = _CustomerDetails.curr;
                dtrow["cust_regn_no"] = _CustomerDetails.Regn_num;
                dtrow["cust_gst_no"] = _CustomerDetails.gst_num;
                dtrow["cust_tan_no"] = _CustomerDetails.tan_num;
                dtrow["cust_pan_no"] = _CustomerDetails.pan_num;
                dtrow["cont_pers"] = _CustomerDetails.cont_pers;
                dtrow["cont_email"] = _CustomerDetails.cont_email;
                dtrow["cont_num1"] = _CustomerDetails.cont_num1;
                dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                dtrow["UserIP"] = Session["UserIP"].ToString();
                //dtrow["cont_num2"] = _CustomerDetails.cont_num2;
                dtrow["cre_limit"] = _CustomerDetails.credit_limit;
                dtrow["cre_days"] = _CustomerDetails.credit_days;
                dtrow["app_on"] = _CustomerDetails.apply_on;
                dtrow["comp_id"] = CompID;
                dtrow["cust_dist"] = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(_CustomerDetails.SalesPersID)))
                {
                    dtrow["sales_rep"] = _CustomerDetails.SalesPersID;
                }
                else
                {
                    dtrow["sales_rep"] = "0";
                }
                dtrow["bank_name"] = _CustomerDetails.bank_name;
                dtrow["bank_branch"] = _CustomerDetails.bank_branch;
                dtrow["bank_add"] = _CustomerDetails.bank_add;
                dtrow["bank_acc_no"] = _CustomerDetails.bank_acc_no;
                dtrow["ifsc_code"] = _CustomerDetails.ifsc_code;
                dtrow["swift_code"] = _CustomerDetails.swift_code;
                dtrow["onhold_reason"] = _CustomerDetails.hold_reason;
                dtrow["gst_cat"] = _CustomerDetails.Gst_Cat;
                dtrow["acc_grp_id"] = _CustomerDetails.acc_grp_id;
                dtrow["acc_name"] = _CustomerDetails.GLAccountNm;
                if (_CustomerDetails.TCSApplicable)
                {
                    dtrow["tcs_posting"] = "Y";
                }
                else
                {
                    dtrow["tcs_posting"] = "N";
                }
                dtrow["gl_rpt_id"] = Convert.ToInt32(_CustomerDetails.GlRepoting_Group_ID);
                dtrow["def_trns_id"] = Convert.ToInt32(_CustomerDetails.DefaultTransporter_ID);
                dtrow["inter_br"] = _CustomerDetails.HdnInterBranch;

                dtrow["cust_zone"] = Convert.ToInt32(_CustomerDetails.cust_zone);
                dtrow["cust_group"] = Convert.ToInt32( _CustomerDetails.cust_group);
                dt.Rows.Add(dtrow);

                CustomerDetail = dt;


                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("cust_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));

                JArray jObject = JArray.Parse(_CustomerDetails.CustomerBranchDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = CompID;
                    if (!string.IsNullOrEmpty(_CustomerDetails.cust_id))
                    {
                        dtrowBrdetails["cust_id"] = _CustomerDetails.cust_id;
                    }
                    else
                    {
                        dtrowBrdetails["cust_id"] = "0";
                    }

                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();

                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                CustomerBranch = dtBranch;
                DataTable dtAddress = new DataTable();
                dtAddress.Columns.Add("comp_id", typeof(int));
                dtAddress.Columns.Add("cust_id", typeof(int));
                dtAddress.Columns.Add("cust_pin", typeof(string));
                dtAddress.Columns.Add("address_id", typeof(int));
                dtAddress.Columns.Add("cust_add", typeof(string));
                dtAddress.Columns.Add("cust_city", typeof(int));
                dtAddress.Columns.Add("cust_dist", typeof(int));
                dtAddress.Columns.Add("cust_state", typeof(int));
                dtAddress.Columns.Add("cust_cntry", typeof(int));
                dtAddress.Columns.Add("cust_gst_no", typeof(string));
                dtAddress.Columns.Add("def_ship_add", typeof(string));
                dtAddress.Columns.Add("def_bill_add", typeof(string));
                dtAddress.Columns.Add("Flag", typeof(string));
                dtAddress.Columns.Add("cont_pers", typeof(string));
                dtAddress.Columns.Add("cont_num", typeof(string));

                JArray AddObject = JArray.Parse(_CustomerDetails.CustomerAddressDetails);
                for (int i = 0; i < AddObject.Count; i++)
                {
                    DataRow dtrowAddress = dtAddress.NewRow();
                    dtrowAddress["comp_id"] = CompID;
                    if (!string.IsNullOrEmpty(_CustomerDetails.cust_id))
                    {
                        dtrowAddress["cust_id"] = _CustomerDetails.cust_id;
                    }
                    else
                    {
                        dtrowAddress["cust_id"] = "0";
                    }
                    dtrowAddress["address_id"] = AddObject[i]["address_id"].ToString();
                    dtrowAddress["cust_pin"] = AddObject[i]["cust_pin"].ToString();
                    dtrowAddress["cust_add"] = AddObject[i]["address"].ToString();
                    dtrowAddress["cust_city"] = AddObject[i]["City"].ToString();
                    dtrowAddress["cust_dist"] = AddObject[i]["District"].ToString();
                    dtrowAddress["cust_state"] = AddObject[i]["State"].ToString();
                    dtrowAddress["cust_cntry"] = AddObject[i]["Country"].ToString();
                    dtrowAddress["cust_gst_no"] = AddObject[i]["GSTNo"].ToString();
                    dtrowAddress["def_ship_add"] = AddObject[i]["ShippingAddress"].ToString();
                    dtrowAddress["def_bill_add"] = AddObject[i]["BillingAddress"].ToString();
                    dtrowAddress["Flag"] = AddObject[i]["Flag"].ToString();
                    dtrowAddress["cont_pers"] = AddObject[i]["Addrcont_per"].ToString();
                    dtrowAddress["cont_num"] = AddObject[i]["Addrcont_no"].ToString();

                    dtAddress.Rows.Add(dtrowAddress);
                }
                CustomerAddress = dtAddress;

                var _CustomerDetailsattch = TempData["ModelDataattch"] as CustomerDetailsattch;
                TempData["ModelDataattch"] = null;
                DataTable dtAttachment = new DataTable();
                if (_CustomerDetails.attatchmentdetail != null)
                {

                    if (_CustomerDetailsattch != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_CustomerDetailsattch.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _CustomerDetailsattch.AttachMentDetailItmStp as DataTable;
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
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_CustomerDetails.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _CustomerDetails.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_CustomerDetails.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_CustomerDetails.cust_id))
                            {
                                dtrowAttachment1["id"] = _CustomerDetails.cust_id;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = CompID;
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_CustomerDetails.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_CustomerDetails.cust_id))
                            {
                                ItmCode = _CustomerDetails.cust_id;
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
                                    if (drImgPath == fielpath.Replace("/", @"\"))
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
                    CustomerAttachments = dtAttachment;
                }
                /***---------------------------------------------------------------***/
                DataTable dtLicence = new DataTable();

                dtLicence.Columns.Add("lic_id", typeof(int));
                dtLicence.Columns.Add("lic_name", typeof(string));
                dtLicence.Columns.Add("lic_number", typeof(string));
                dtLicence.Columns.Add("exp_dt", typeof(string));
                dtLicence.Columns.Add("alert_days", typeof(int));

                if (_CustomerDetails.LincenceDetail != null)
                {
                    JArray AddObj = JArray.Parse(_CustomerDetails.LincenceDetail);
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
                    //LicenceDetail = dtLicence;//Commented by Suraj Maurya on 26-11-2025
                }
                LicenceDetail = dtLicence;
                SaveMessage = _CustomerDetails_ISERVICES.insertCustomerDetails(CustomerDetail, CustomerBranch, CustomerAttachments, CustomerAddress, Convert.ToInt32(_CustomerDetails.PaymentAlert), LicenceDetail);
                string CustCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "DataNotFound")
                {
                    var a = CustCode.Split('-');
                    var msg = ("Data Not found") + " " + a[0].Trim() + " in " + PageName;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _CustomerDetails.Message = Message;
                    return RedirectToAction("CustomerDetails");
                }
                if (Message == "Save")
                {
                    string Guid = "";
                    if (_CustomerDetailsattch != null)
                    {
                        //if (Session["Guid"] != null)
                        if (_CustomerDetailsattch.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _CustomerDetailsattch.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    comCont.ResetImageLocation(CompID, "00", guid, PageName, CustCode, _CustomerDetails.TransType, CustomerAttachments);

                    //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                    //if (Directory.Exists(sourcePath))
                    //{
                    //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Guid + "_" + "*");
                    //    foreach (string file in filePaths)
                    //    {
                    //        string[] items = file.Split('\\');
                    //        string ItemName = items[items.Length - 1];
                    //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                    //        foreach (DataRow dr in CustomerAttachments.Rows)
                    //        {
                    //            string DrItmNm = dr["file_name"].ToString();
                    //            if (ItemName == DrItmNm)
                    //            {
                    //                string img_nm = CompID + CustCode + "_" + Path.GetFileName(DrItmNm).ToString();
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
                if (Message == "Update" || Message == "Save")
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    //Session["Message"] = "Save";
                    //Session["CustCode"] = CustCode;
                    //Session["TransType"] = "Update";
                    //Session["CustomerFromProspect"] = null;
                    //Session["Prospect_id"] = null;
                    _CustomerDetails.AttachMentDetailItmStp = null;
                    _CustomerDetails.Guid = null;
                    _CustomerDetails.Message = "Save";
                    _CustomerDetails.CustCode = CustCode;
                    _CustomerDetails.TransType = "Update";
                    _CustomerDetails.CustomerFromProspect = null;
                    _CustomerDetails.Prospect_id = null;
                    Session["brid"] = null;
                }
                if (Message == "Duplicate")
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    _CustomerDetails.AttachMentDetailItmStp = null;
                    _CustomerDetails.Guid = null;
                    //Commented by Hina on 09-12-2022 to save data after duplicate 
                    //Session["TransType"] = "Duplicate";
                    //Session["Message"] = "Duplicate";
                    //Session["Command"] = "New";
                    //Session["CustCode"] = CustCode;
                    _CustomerDetails.Message = "Duplicate";
                    _CustomerDetails.Command = "New";
                    _CustomerDetails.CustCode = CustCode;
                    _CustomerDetails.TransType = "Duplicate";
                    string Guid = "";
                    if (_CustomerDetailsattch != null)
                    {
                        if (_CustomerDetailsattch.Guid != null)
                        {
                            Guid = _CustomerDetailsattch.Guid;
                        }
                        //var id = 1;
                        DataTable table = new DataTable();
                        table.Columns.Add("id", typeof(string));
                        table.Columns.Add("file_name", typeof(string));
                        table.Columns.Add("file_path", typeof(string));
                        table.Columns.Add("file_def", typeof(char));
                        table.Columns.Add("comp_id", typeof(Int32));
                        foreach (DataRow dr in CustomerAttachments.Rows)
                        {
                            string DrItmNm = Guid + "_" + dr["file_name"].ToString();
                            //var filepath= dr["file_path"].ToString();

                            string doc_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/", DrItmNm);


                            table.Rows.Add("0", DrItmNm, doc_path, "Y", CompID);
                        }
                        _CustomerDetailsattch.AttachMentDetailItmStp = table;

                    }
                    TempData["ModelDataattch"] = _CustomerDetailsattch;
                }
                if (Message == "DuplicateGL")
                {
                    _CustomerDetails.Message = "DuplicateGL";
                    _CustomerDetails.Command = "New";
                    _CustomerDetails.CustCode = CustCode;
                    _CustomerDetails.TransType = "Duplicate";
                }
                if (_CustomerDetails.app_status == "A")
                {
                    //Session["AppStatus"] = 'A';
                    _CustomerDetails.AppStatus = "A";
                }
                else
                {
                    //Session["AppStatus"] = 'D';
                    _CustomerDetails.AppStatus = "D";
                }
                //Session["BtnName"] = "BtnSave";
                _CustomerDetails.BtnName = "BtnSave";
                TempData["ModelData"] = _CustomerDetails;


                return RedirectToAction("CustomerDetails");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_CustomerDetails.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_CustomerDetails.Guid != null)
                        {
                            Guid = _CustomerDetails.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID, PageName, Guid, Server);
                    }
                }
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                GetCompIDAndBRID();
                CustomerDetailsattch _CustomerDetail = new CustomerDetailsattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string CustCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _CustomerDetail.Guid = DocNo;
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    ////Session["AttachMentDetailItmStp"] = dt;
                    _CustomerDetail.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _CustomerDetail.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _CustomerDetail;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        private ActionResult CustomerDetailDelete(CustomerDetails _CustomerDetails, string CustID)
        {
            try
            {
                //CustomerDetails _CustomerDetails = new CustomerDetails();
                GetCompIDAndBRID();
                string cust_id = CustID;
                string Message = _CustomerDetails_ISERVICES.CustomerDetailDelete(_CustomerDetails, CompID, cust_id);
                if (Message == "Deleted")
                {
                    if (!string.IsNullOrEmpty(cust_id))
                    {
                        getDocumentName(); /* To set Title*/
                        string PageName = title.Replace(" ", "");
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID, PageName, cust_id, Server);
                    }
                    _CustomerDetails.Message = "Deleted";
                    _CustomerDetails.Command = "Delete";
                    _CustomerDetails.CustCode = "";
                    //_CustomerDetails = null;
                    _CustomerDetails.TransType = "Refresh";
                    _CustomerDetails.AppStatus = "DL";
                    _CustomerDetails.BtnName = "Delete";
                    _CustomerDetails.hold_reason = null;
                    //_CustomerDetails.hold_reason = null;
                    _CustomerDetails.CreatedBy = null;
                    _CustomerDetails.CreatedOn = null;
                    _CustomerDetails.ApprovedBy = null;
                    _CustomerDetails.ApprovedOn = null;
                    _CustomerDetails.AmmendedBy = null;
                    _CustomerDetails.AmmendedOn = null;
                    _CustomerDetails.Status = null;
                    _CustomerDetails.cust_coa = 0;
                    TempData["ModelData"] = _CustomerDetails;
                    return RedirectToAction("CustomerDetails", "CustomerDetails");
                }
                else
                {
                    //Session["Message"] = "Dependency";
                    //Session["Command"] = "Delete";
                    //ViewBag.Message = Session["Message"].ToString();

                    _CustomerDetails.Message = "Dependency";
                    _CustomerDetails.Command = "Delete";
                    _CustomerDetails.Message = _CustomerDetails.Message;
                    return View("~/Areas/BusinessLayer/Views/CustomerSetup/CustomerDetail.cshtml", _CustomerDetails);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult CustomerApprove(CustomerDetails _CustomerDetails, string command, string ListFilterData1)
        {
            try
            {
                GetCompIDAndBRID();
                string cust_id = _CustomerDetails.cust_id;
                string app_id = userid;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _CustomerDetails_ISERVICES.CustomerApprove(_CustomerDetails, CompID, app_id, cust_id, mac_id);

                if (Message == "DuplicateGL")
                {
                    _CustomerDetails.Message = "DuplicateGL";
                    _CustomerDetails.Command = "New";
                    _CustomerDetails.CustCode = _CustomerDetails.cust_id;
                }
                else
                {
                    _CustomerDetails.TransType = "Update";
                    _CustomerDetails.Command = command;
                    _CustomerDetails.CustCode = _CustomerDetails.cust_id;
                    _CustomerDetails.Message = "Approved";
                    _CustomerDetails.AppStatus = "A";
                    _CustomerDetails.BtnName = "BtnApprove";
                }
                var custCodeURL = _CustomerDetails.cust_id;
                var TransType = _CustomerDetails.TransType;
                var BtnName = _CustomerDetails.BtnName;
                TempData["ModelData"] = _CustomerDetails;
                TempData["ListFilterData"] = ListFilterData1;
                return (RedirectToAction("CustomerDetails", "CustomerDetails", new { custCodeURL = custCodeURL, TransType, BtnName, command }));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [NonAction]
        private DataTable GetCustcoa(string CustCode, string type)
        {
            try
            {
                GetCompIDAndBRID();
                DataTable dt = _CustomerDetails_ISERVICES.GetCustcoaDAL(CompID, type, CustCode);
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
                GetCompIDAndBRID();
                DataTable dt = _CustomerDetails_ISERVICES.GetcategoryDAL(Comp_ID);
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
        private DataTable GetCustport()
        {
            try
            {
                GetCompIDAndBRID();

                DataTable dt = _CustomerDetails_ISERVICES.GetCustportDAL(Comp_ID);
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
        private DataTable GetCustcurr(string Supptype)
        {
            try
            {
                GetCompIDAndBRID();
                if (Supptype == "I" || Supptype == "E")
                {
                    Supptype = "A";
                }
                DataTable dt = _CustomerDetails_ISERVICES.GetCurronSuppTypeDAL(Comp_ID, Supptype);
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
        private DataTable GetRegion()
        {
            try
            {
                GetCompIDAndBRID();

                DataTable dt = _CustomerDetails_ISERVICES.GetRegionDAL(Comp_ID);
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
        private DataTable GetCustPriceGrp()
        {
            try
            {
                GetCompIDAndBRID();
                DataTable dt = _CustomerDetails_ISERVICES.GetCustPriceGrpDAL(Comp_ID);
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
                GetCompIDAndBRID();

                DataTable dt = _CustomerDetails_ISERVICES.GetBrListDAL(Comp_ID);
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
                List<curr> _CustcurrList = new List<curr>();
                _CustomerDetails = new CustomerDetails();
                GetCompIDAndBRID();
                if (Supptype == "I" || Supptype == "E")
                {
                    Supptype = "A";
                }
                DataTable dt = _CustomerDetails_ISERVICES.GetCurronSuppTypeDAL(Comp_ID, Supptype);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
                //foreach (DataRow dr in dt.Rows)
                //{
                //    curr _Custcurr = new curr();
                //    _Custcurr.curr_id = dr["curr_id"].ToString();
                //    _Custcurr.curr_name = dr["curr_name"].ToString();
                //    _CustcurrList.Add(_Custcurr);

                //}
                //if (Supptype == "I")
                //{
                //    _CustcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                //    _CustomerDetails.currList = _CustcurrList;
                //}
                //else
                //{
                //    _CustomerDetails.currList = _CustcurrList;
                //}
                //return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialCurrencyCustomer.cshtml", _CustomerDetails);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        private DataTable GetCustomerBranchList()
        {
            try
            {
                GetCompIDAndBRID();

                DataTable dt = _CustomerDetails_ISERVICES.GetBrList(Comp_ID).Tables[0];
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
            string GroupName = string.Empty;
            //string ErrorMessage = "success";
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
                SuppCity = _CustomerDetails_ISERVICES.SuppCityDAL(GroupName);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(SuppCity.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsuppDSCntr(string SuppCity)
        {
            JsonResult DataRows = null;
            try
            {
                DataSet HoCompData = _CustomerDetails_ISERVICES.GetsuppDSCntrDAL(SuppCity);
                DataRows = Json(JsonConvert.SerializeObject(HoCompData));/*Result convert into Json Format for javasript*/
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
        public JsonResult ChakeDependencyAddr(string custId, string addr_id)
        {
            JsonResult DataRows = null;
            try
            {

                GetCompIDAndBRID();

                DataSet CDA = _CustomerDetails_ISERVICES.checkDependencyAddr(Comp_ID, custId, addr_id);
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
        [HttpPost]
        public JsonResult GetGlReportingGrp(CustomerDetails _CustomerDetails)
        {
            JsonResult DataRows = null;
            try
            {


                string gl_repoting = string.Empty;
                GetCompIDAndBRID();
                if (string.IsNullOrEmpty(_CustomerDetails.GlRepoting_Group))
                {
                    gl_repoting = "0";
                }
                else
                {
                    gl_repoting = _CustomerDetails.GlRepoting_Group;
                }
                DataTable GlReporting = _CustomerDetails_ISERVICES.GetGlReportingGrp(Comp_ID, Br_ID, gl_repoting);
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
        /*----------------------Code start of Country,state,district,city--------------------------*/
        [NonAction]
        private DataTable GetCountryList(string SuppType)
        {
            try
            {
                GetCompIDAndBRID();
                DataTable dt = _CustomerDetails_ISERVICES.GetCountryListDDL(Comp_ID, SuppType);

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
        public JsonResult GetCountryonChngCustTyp(string CustType)
        {
            JsonResult DataRows = null;
            try
            {
                List<CmnCountryList> _TransList = new List<CmnCountryList>();
                _CustomerDetails = new CustomerDetails();
                GetCompIDAndBRID();
                DataTable dt = _CustomerDetails_ISERVICES.GetCountryListDDL(Comp_ID, CustType);
                if (CustType == "I")
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
                DataTable dt = _CustomerDetails_ISERVICES.GetstateOnCountryDDL(ddlCountryID);
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
                DataTable dt = _CustomerDetails_ISERVICES.GetDistrictOnStateDDL(ddlStateID);
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
                DataTable dt = _CustomerDetails_ISERVICES.GetCityOnDistrictDDL(ddlDistrictID);
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
                DataSet ds = _CustomerDetails_ISERVICES.GetStateCode(stateId);
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
        public ActionResult GetCustomerSalesDetail(string FromDate, string ToDate, string cust_ID)
        {
            GetCompIDAndBRID();
            CustomerDetails _CustomerDetails = new CustomerDetails();
            ViewBag.VBSupplierList = null;
            try
            {
                DataTable HoCompData = _CustomerDetails_ISERVICES.GetCustomerSalesDetail(Comp_ID, Br_ID, cust_ID, FromDate, ToDate).Tables[0];
                ViewBag.VBCustomerSaleshistorylist = HoCompData;
                _CustomerDetails.SalesHistorySearch = "SalesSearch";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialCustomerSaleHistory.cshtml", _CustomerDetails);
        }

        [HttpPost]
        public JsonResult CheckDependcyGstno(string Cust_Id, string CustGst)
        {
            JsonResult DataRows = null;
            try
            {

                CustomerDetails _CustomerDetails = new CustomerDetails();
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _CustomerDetails_ISERVICES.CheckDependcyGstno(Comp_ID, Cust_Id, CustGst);
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
    }
}