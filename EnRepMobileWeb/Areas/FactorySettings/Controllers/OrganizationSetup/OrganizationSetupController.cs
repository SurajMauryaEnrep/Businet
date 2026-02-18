using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.ISERVICES.FactorySettings_ISERVICE.OrganizationSetup_ISERVICE;
using EnRepMobileWeb.MODELS.Factory_Settings.Organization_Setup;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.BusinessLayer.CustomerSetup;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.MODELS.Common;
using System.Configuration;

namespace EnRepMobileWeb.Areas.FactorySettings.Controllers.OrganizationSetup
{
    public class OrganizationSetupController : Controller
    {
        //[OSComDetail]
        string CompID, BrchID, title, language, userid = String.Empty;
        string DocumentMenuId = "102110";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        OrganizationSetup_ISERVICE _OrganizationSetup_ISERVICE;
        public OrganizationSetupController(Common_IServices _Common_IServices, OrganizationSetup_ISERVICE _OrganizationSetup_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._OrganizationSetup_ISERVICE = _OrganizationSetup_ISERVICE;
        }
        // GET: FactorySettings/OrganizationSetup
        public ActionResult OrganizationSetup()
        {
            try
            {
                //OrganizationSetupModel _OrganizationSetupModel = new OrganizationSetupModel();
                var OSCompDetail = TempData["ModelData"] as OrganizationSetupModel;
                if (OSCompDetail != null)
                {

                    DataSet HOList = _OrganizationSetup_ISERVICE.BindHeadOffice();
                    List<HO_List> _HOList = new List<HO_List>();
                    foreach (DataRow dr in HOList.Tables[0].Rows)
                    {
                        HO_List hO_List = new HO_List();
                        hO_List.HO_ID = dr["Comp_Id"].ToString();
                        hO_List.HO_Name = dr["comp_nm"].ToString();
                        _HOList.Add(hO_List);
                    }
                    _HOList.Insert(0, new HO_List() { HO_ID = "0", HO_Name = "---Select---" });
                    OSCompDetail.hO_Lists = _HOList;


                    DataSet lang = _OrganizationSetup_ISERVICE.BindLAng();
                    List<Lang_List> _LangList = new List<Lang_List>();
                    foreach (DataRow dr in lang.Tables[0].Rows)
                    {
                        Lang_List lang_List = new Lang_List();
                        lang_List.Lang_ID = dr["lang_id"].ToString();
                        lang_List.Lang_Name = dr["lang_name"].ToString();
                        _LangList.Add(lang_List);
                    }
                    _LangList.Insert(0, new Lang_List() { Lang_ID = "0", Lang_Name = "---Select---" });
                    OSCompDetail.lang_Lists = _LangList;

                    DataTable dt = new DataTable();
                    List<CurrencyNameLIst> currencyNameLIst = new List<CurrencyNameLIst>();
                    dt = GetCurrencySetup();
                    foreach (DataRow dr in dt.Rows)
                    {
                        CurrencyNameLIst _CLList = new CurrencyNameLIst();
                        _CLList.curr_id = Convert.ToInt32(dr["curr_id"]);
                        _CLList.curr_name = dr["curr_name"].ToString();
                        currencyNameLIst.Add(_CLList);
                    }
                    currencyNameLIst.Insert(0, new CurrencyNameLIst() { curr_id = 0, curr_name = "---Select---" });
                    OSCompDetail._currencyNameList = currencyNameLIst;
                    /*-------------------------------------Code start of Country,state,district,city--------------------------*/
                    List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    List<Country> _ContryList2 = new List<Country>();
                    CommonAddress_Detail _Model = new CommonAddress_Detail();

                    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });



                    DataTable dtcntry = GetCountryList();

                    foreach (DataRow dr in dtcntry.Rows)
                    {
                        CmnCountryList _Custcurr = new CmnCountryList();
                        _Custcurr.country_id = dr["country_id"].ToString();
                        _Custcurr.country_name = dr["country_name"].ToString();
                        _ContryList.Add(_Custcurr);
                        _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _Model.countryList = _ContryList2;
                    OSCompDetail.countryList = _ContryList;

                    List<CmnStateList> state = new List<CmnStateList>();
                    state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountry = "";
                    if (!string.IsNullOrEmpty(OSCompDetail.Country))
                        transCountry = OSCompDetail.Country;
                    else
                        transCountry = dtcntry.Rows[0]["country_id"].ToString();

                    DataTable dtStates = _OrganizationSetup_ISERVICE.GetstateOnCountryDDL(transCountry);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    OSCompDetail.StateList = state;

                    string transState = "0";
                    List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                    DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(OSCompDetail.State))
                        transState = OSCompDetail.State;
                    else
                        transState = "0";
                    DataTable dtDist = _OrganizationSetup_ISERVICE.GetDistrictOnStateDDL(transState);
                    if (dtDist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDist.Rows)
                        {
                            DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    OSCompDetail.DistrictList = DistList;

                    string transDist = "0";
                    if (!string.IsNullOrEmpty(OSCompDetail.District))
                        transDist = OSCompDetail.District;
                    else
                        transDist = "0";
                    DataTable dtCities = _OrganizationSetup_ISERVICE.GetCityOnDistrictDDL(transDist);

                    List<CmnCityList> cities = new List<CmnCityList>();
                    cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dtCities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCities.Rows)
                        {
                            cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    OSCompDetail.cityLists = cities;

                    OSCompDetail._CommonAddress_Detail = _Model;
                    /*----------------------------------Code End of Country,state,district,city--------------------------*/

                    /*Commented by Hina on 12-01-2024 to change Bind city on behalf of district*/
                    //Dictionary<string, string> CityLis = new Dictionary<string, string>();
                    //CityLis = _OrganizationSetup_ISERVICE.OSCityList("");
                    //List<CityList> city = new List<CityList>();
                    //if (CityLis.Count > 0)
                    //{
                    //    foreach (var dr in CityLis)
                    //    {
                    //        CityList CityList = new CityList();
                    //        CityList.CityId = dr.Key;
                    //        CityList.CityName = dr.Value;
                    //        city.Add(CityList);
                    //    }
                    //}
                    //OSCompDetail.cityLists = city;


                    /******START  DOCUMENT sETUP dATA BIND*******/
                    DataTable dt2 = new DataTable();
                    List<DocList> docLists = new List<DocList>();
                    dt2 = Getdoclist();
                    foreach (DataRow dr in dt2.Rows)
                    {
                        DocList _CLList = new DocList();
                        _CLList.Doc_id = dr["doc_id"].ToString();
                        _CLList.DOC_Name = dr["doc_name_eng"].ToString();
                        docLists.Add(_CLList);
                    }
                    docLists.Insert(0, new DocList() { Doc_id = "0", DOC_Name = "---Select---" });
                    OSCompDetail.DocumentList = docLists;
                    //if(OSCompDetail.TransType == "Update")
                    //{
                    //    OSCompDetail.Message = "Save";
                    //}
                    if (OSCompDetail.TransType == "Update" || OSCompDetail.Command == "Edit")
                    {
                        string EntityName = OSCompDetail.EntityName;
                        var com_ID = OSCompDetail.comp_id;
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }


                        DataSet ds = _OrganizationSetup_ISERVICE.GetviewCOM(com_ID);
                        if (ds.Tables[0].Rows[0]["HO_comp_id"].ToString() == "0")
                        {
                            OSCompDetail.HeadBranch = "H";
                            OSCompDetail.Quantity = ds.Tables[1].Rows[0]["qty_digit"].ToString();
                            OSCompDetail.Quantity_Value = ds.Tables[1].Rows[0]["val_digit"].ToString();
                            OSCompDetail.Rate = ds.Tables[1].Rows[0]["rate_digit"].ToString();
                            OSCompDetail.Weight = ds.Tables[1].Rows[0]["wgt_digit"].ToString();
                            OSCompDetail.Exchange = ds.Tables[1].Rows[0]["exch_digit"].ToString();
                        }
                        else
                        {
                            OSCompDetail.HeadBranch = "B";
                        }
                        ViewBag.CompAddressDetail = ds.Tables[2];/*Add by Hina sharma on 06-08-2025*/
                        //OSCompDetail.Supp_Type = ds.Tables[0].Rows[0]["HO_comp_id"].ToString();
                        OSCompDetail.comp_id = Convert.ToInt32(ds.Tables[0].Rows[0]["Comp_Id"]);
                        OSCompDetail.EntityName = ds.Tables[0].Rows[0]["comp_nm"].ToString();
                        OSCompDetail.EntityPrefix = ds.Tables[0].Rows[0]["comp_cd"].ToString();
                        OSCompDetail.HeadOffice = ds.Tables[0].Rows[0]["HO_comp_id"].ToString();
                        OSCompDetail.FY_StartDate = ds.Tables[0].Rows[0]["fin_st_dt"].ToString();
                        OSCompDetail.FY_EndDate = ds.Tables[0].Rows[0]["fin_end_dt"].ToString();
                        OSCompDetail.def_lang = ds.Tables[0].Rows[0]["def_lang"].ToString();
                        //OSCompDetail.Currency_name = ds.Tables[0].Rows[0]["bs_curr_nm"].ToString();
                        OSCompDetail.Currency_name = ds.Tables[0].Rows[0]["bs_curr_id"].ToString();
                        OSCompDetail.cont_pers = ds.Tables[0].Rows[0]["cont_per1"].ToString();
                        OSCompDetail.cont_num1 = ds.Tables[0].Rows[0]["cont_num1"].ToString();
                        OSCompDetail.cont_email = ds.Tables[0].Rows[0]["email_id1"].ToString();
                        OSCompDetail.cont_pers1 = ds.Tables[0].Rows[0]["cont_per2"].ToString();
                        OSCompDetail.cont_num2 = ds.Tables[0].Rows[0]["cont_num2"].ToString();
                        OSCompDetail.cont_email1 = ds.Tables[0].Rows[0]["email_id2"].ToString();
                        /*----------commented start by Hina on 02-08-2025-----------------*/
                        //OSCompDetail.Address = ds.Tables[0].Rows[0]["Comp_Add"].ToString();
                        //OSCompDetail.Pin = ds.Tables[0].Rows[0]["pin"].ToString();
                        //OSCompDetail.District = ds.Tables[0].Rows[0]["district"].ToString();
                        //OSCompDetail.State = ds.Tables[0].Rows[0]["state"].ToString();
                        //OSCompDetail.City = ds.Tables[0].Rows[0]["city"].ToString();
                        //OSCompDetail.Country = ds.Tables[0].Rows[0]["country"].ToString();
                        /*----------commented end by Hina on 02-08-2025-----------------*/

                        OSCompDetail.PANNumber = ds.Tables[0].Rows[0]["pan_no"].ToString();
                        OSCompDetail.bank_benef = ds.Tables[0].Rows[0]["benif_name"].ToString();
                        OSCompDetail.bank_name = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        OSCompDetail.bank_add = ds.Tables[0].Rows[0]["bank_add"].ToString();
                        OSCompDetail.bank_acc_no = ds.Tables[0].Rows[0]["acc_no"].ToString();
                        OSCompDetail.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                        OSCompDetail.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                        OSCompDetail.MSME_Number = ds.Tables[0].Rows[0]["msme_no"].ToString();
                        OSCompDetail.WebSite = ds.Tables[0].Rows[0]["comp_website"].ToString();

                        OSCompDetail.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        OSCompDetail.create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        OSCompDetail.img_six = ds.Tables[0].Rows[0]["logo"].ToString();
                        OSCompDetail.Currency_Formet = ds.Tables[0].Rows[0]["curr_format"].ToString();
                        OSCompDetail.Currency_Formet_id = ds.Tables[0].Rows[0]["curr_format"].ToString();
                        OSCompDetail.IEC_Code = ds.Tables[0].Rows[0]["iec_no"].ToString();
                        OSCompDetail.LandlineNumber = ds.Tables[0].Rows[0]["Landline_no"].ToString();
                        if (ds.Tables[0].Rows[0]["logo"].ToString() == "" || ds.Tables[0].Rows[0]["logo"].ToString() == null)
                        {
                            OSCompDetail.img_six = ds.Tables[0].Rows[0]["logo"].ToString();
                            OSCompDetail.attatchmentdetail = null;
                        }
                        else
                        {
                            //  string server_Url = "";
                            string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                            string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                            if (Request.Url.Host == localIp)
                                serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                            else
                                serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                            OSCompDetail.attatchmentdetail = serverUrl + ds.Tables[0].Rows[0]["logo"].ToString();
                            OSCompDetail.img_digi_sign = serverUrl + ds.Tables[0].Rows[0]["digi_sign"].ToString();
                            OSCompDetail.hdnAttachment = ds.Tables[0].Rows[0]["logo"].ToString();
                            OSCompDetail.hdnAttachment1 = ds.Tables[0].Rows[0]["digi_sign"].ToString();

                        }
                        OSCompDetail.GSTNumber = ds.Tables[0].Rows[0]["gst_no"].ToString();
                        if (OSCompDetail.GSTNumber != "")
                        {
                            string GSTNumber = OSCompDetail.gst_num;
                            OSCompDetail.gst_num = OSCompDetail.GSTNumber.Substring(0, 2).Trim();

                            OSCompDetail.GSTMidPrt = OSCompDetail.GSTNumber.Substring(2, 10).Trim();
                            OSCompDetail.GSTLastPrt = OSCompDetail.GSTNumber.Substring(12, 3).Trim();
                        }
                        OSCompDetail.mod_by = ds.Tables[0].Rows[0]["mod_by"].ToString();
                        OSCompDetail.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();

                        ViewBag.licenseDetails = ds.Tables[3];

                        /*---------------------------------Code start of Country,state,district,city--------------------------*/
                        List<CmnCountryList> _TransList2 = new List<CmnCountryList>();
                        //CommonAddress_Detail _Model = new CommonAddress_Detail();
                        _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                        dt = GetCountryList();

                        //foreach (DataRow dr in dt.Rows)
                        //{
                        //    CmnCountryList _Custcurr = new CmnCountryList();
                        //    _Custcurr.country_id = dr["country_id"].ToString();
                        //    _Custcurr.country_name = dr["country_name"].ToString();
                        //    _TransList2.Add(_Custcurr);
                        //    _TransList2.Add(new CmnCountryList { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                        //}
                        //OSCompDetail.countryList = _TransList2;


                        List<CmnStateList> state1 = new List<CmnStateList>();
                        //state1.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                        string transCountry1 = "";
                        if (!string.IsNullOrEmpty(OSCompDetail.Country))
                            transCountry1 = OSCompDetail.Country;
                        else
                            transCountry1 = dt.Rows[0]["country_id"].ToString();

                        dt = _OrganizationSetup_ISERVICE.GetstateOnCountryDDL(transCountry1);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                state1.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                            }
                        }
                        OSCompDetail.StateList = state1;

                        string transState1 = "0";
                        List<CmnDistrictList> DistList1 = new List<CmnDistrictList>();
                        //DistList1.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                        if (!string.IsNullOrEmpty(OSCompDetail.State))
                            transState1 = OSCompDetail.State;
                        else
                            transState1 = "0";
                        dt = _OrganizationSetup_ISERVICE.GetDistrictOnStateDDL(transState1);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                DistList1.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                            }
                        }
                        OSCompDetail.DistrictList = DistList1;

                        string transDist1 = "0";
                        if (!string.IsNullOrEmpty(OSCompDetail.District))
                            transDist1 = OSCompDetail.District;
                        else
                            transDist1 = "0";
                        dt = _OrganizationSetup_ISERVICE.GetCityOnDistrictDDL(transDist1);

                        List<CmnCityList> cities1 = new List<CmnCityList>();
                        //cities1.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cities1.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                            }
                        }
                        OSCompDetail.cityLists = cities1;

                        OSCompDetail._CommonAddress_Detail = _Model;
                        /*-----------------------------------------Code End of Country,state,district,city--------------------------*/


                    }
                    if (OSCompDetail.BtnName == null)
                    {
                        OSCompDetail.BtnName = "BtnAddNew";

                        OSCompDetail.TransType = OSCompDetail.TransType;
                        OSCompDetail.Command = OSCompDetail.Command;
                        OSCompDetail.TransType = OSCompDetail.TransType;
                        OSCompDetail.Command = "Add";
                        OSCompDetail.BtnName = "BtnSave";
                    }
                    if (OSCompDetail.TransType == "Save")
                    {
                        ViewBag.Message = "New";

                    }
                    if (OSCompDetail.TransType == "Update")
                    {

                    }
                    if (OSCompDetail.Command == "Save")
                    {
                        OSCompDetail.Message = "Save";
                    }

                    if (OSCompDetail.BtnName == "BtnEdit")
                    {
                        OSCompDetail.Command = "Edit";
                        OSCompDetail.TransType = "Update";
                    }
                    if (OSCompDetail.Message == "Duplicate" || OSCompDetail.Message == "DuplicateUpdate")
                    {

                     
                      
                        OSCompDetail.Command = "Add";
                        if(OSCompDetail.Message == "Duplicate")
                        {
                            OSCompDetail.TransType = "Duplicate";
                            OSCompDetail.comp_id = 0;
                        }
                        else if(OSCompDetail.Message == "DuplicateUpdate")
                        {
                            OSCompDetail.TransType = "DuplicateUpdate";
                            OSCompDetail.comp_id = OSCompDetail.comp_id;
                        }
                     
                        OSCompDetail.BtnName = "BtnAddNew";
                        OSCompDetail.Message = "Duplicate";
                    }
                    if (OSCompDetail.Quantity == null)
                    {
                        OSCompDetail.Quantity = "2";
                    }
                    if (OSCompDetail.Quantity_Value == null)
                    {
                        OSCompDetail.Quantity_Value = "2";
                    }
                    if (OSCompDetail.Rate == null)
                    {
                        OSCompDetail.Rate = "2";
                    }
                    if (OSCompDetail.Weight == null)
                    {
                        OSCompDetail.Weight = "2";
                    }
                    if (OSCompDetail.Exchange == null)
                    {
                        OSCompDetail.Exchange = "2";
                    }

                    OSCompDetail.TransType = OSCompDetail.TransType;
                    ViewBag.VBRoleList = GetRoleList();
                    ViewBag.MenuPageName = getDocumentName();
                    OSCompDetail.Title = title;
                    return View("~/Areas/FactorySettings/Views/OrganizationSetup/OrganizationSetup.cshtml", OSCompDetail);
                }
                else
                {
                    OrganizationSetupModel _OrganizationSetupModel = new OrganizationSetupModel();
                    DataSet HOList = _OrganizationSetup_ISERVICE.BindHeadOffice();
                    List<HO_List> _HOList = new List<HO_List>();
                    foreach (DataRow dr in HOList.Tables[0].Rows)
                    {
                        HO_List hO_List = new HO_List();
                        hO_List.HO_ID = dr["Comp_Id"].ToString();
                        hO_List.HO_Name = dr["comp_nm"].ToString();
                        _HOList.Add(hO_List);
                    }
                    _HOList.Insert(0, new HO_List() { HO_ID = "0", HO_Name = "---Select---" });
                    _OrganizationSetupModel.hO_Lists = _HOList;


                    DataSet lang = _OrganizationSetup_ISERVICE.BindLAng();
                    List<Lang_List> _LangList = new List<Lang_List>();
                    foreach (DataRow dr in lang.Tables[0].Rows)
                    {
                        Lang_List lang_List = new Lang_List();
                        lang_List.Lang_ID = dr["lang_id"].ToString();
                        lang_List.Lang_Name = dr["lang_name"].ToString();
                        _LangList.Add(lang_List);
                    }
                    _LangList.Insert(0, new Lang_List() { Lang_ID = "0", Lang_Name = "---Select---" });
                    _OrganizationSetupModel.lang_Lists = _LangList;

                    DataTable dt = new DataTable();
                    List<CurrencyNameLIst> currencyNameLIst = new List<CurrencyNameLIst>();
                    dt = GetCurrencySetup();
                    foreach (DataRow dr in dt.Rows)
                    {
                        CurrencyNameLIst _CLList = new CurrencyNameLIst();
                        _CLList.curr_id = Convert.ToInt32(dr["curr_id"]);
                        _CLList.curr_name = dr["curr_name"].ToString();
                        currencyNameLIst.Add(_CLList);
                    }
                    currencyNameLIst.Insert(0, new CurrencyNameLIst() { curr_id = 0, curr_name = "---Select---" });
                    _OrganizationSetupModel._currencyNameList = currencyNameLIst;

                    /*------------------------------------------Code Start of Country,state,district,city--------------------------*/

                    List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    List<Country> _ContryList2 = new List<Country>();
                    CommonAddress_Detail _Model = new CommonAddress_Detail();

                    DataTable dtcntry = GetCountryList();

                    foreach (DataRow dr in dtcntry.Rows)
                    {
                        CmnCountryList _Contry = new CmnCountryList();
                        _Contry.country_id = dr["country_id"].ToString();
                        _Contry.country_name = dr["country_name"].ToString();
                        _ContryList.Add(_Contry);
                        _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _Model.countryList = _ContryList2;
                    _OrganizationSetupModel.countryList = _ContryList;

                    List<CmnStateList> state = new List<CmnStateList>();
                    state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountry = "";
                    if (!string.IsNullOrEmpty(_OrganizationSetupModel.Country))
                        transCountry = _OrganizationSetupModel.Country;
                    else
                        transCountry = dtcntry.Rows[0]["country_id"].ToString();

                    DataTable dtStates = _OrganizationSetup_ISERVICE.GetstateOnCountryDDL(transCountry);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    _OrganizationSetupModel.StateList = state;

                    string transState = "0";
                    List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                    DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(_OrganizationSetupModel.State))
                        transState = _OrganizationSetupModel.State;
                    else
                        transState = "0";
                    DataTable dtDist = _OrganizationSetup_ISERVICE.GetDistrictOnStateDDL(transState);
                    if (dtDist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDist.Rows)
                        {
                            DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    _OrganizationSetupModel.DistrictList = DistList;

                    string transDist = "0";
                    if (!string.IsNullOrEmpty(_OrganizationSetupModel.District))
                        transDist = _OrganizationSetupModel.District;
                    else
                        transDist = "0";
                    DataTable dtCities = _OrganizationSetup_ISERVICE.GetCityOnDistrictDDL(transDist);

                    List<CmnCityList> cities = new List<CmnCityList>();
                    cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dtCities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCities.Rows)
                        {
                            cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    _OrganizationSetupModel.cityLists = cities;

                    _OrganizationSetupModel._CommonAddress_Detail = _Model;
                    /*------------------------------------------Code End of Country,state,district,city--------------------------*/


                    /*Commented by Hina on 03-01-2024 to change Bind city on behalf of district*/

                    //Dictionary<string, string> CityLis = new Dictionary<string, string>();
                    //CityLis = _OrganizationSetup_ISERVICE.OSCityList("");
                    //List<CityList> city = new List<CityList>();
                    //if (CityLis.Count > 0)
                    //{
                    //    foreach (var dr in CityLis)
                    //    {
                    //        CityList CityList = new CityList();
                    //        CityList.CityId = dr.Key;
                    //        CityList.CityName = dr.Value;
                    //        city.Add(CityList);
                    //    }
                    //}
                    //_OrganizationSetupModel.cityLists = city;
                    //List<CityList> _city = new List<CityList>();
                    //DataTable dt4 = new DataTable();
                    //dt4 = GetAutoCompleteSuppcity(_OrganizationSetupModel);
                    //foreach (DataRow dr in dt4.Rows)
                    //{
                    //    CityList suppCity = new CityList();
                    //    suppCity.CityId = dr["city_id"].ToString();
                    //    suppCity.CityName = dr["city_name"].ToString();
                    //    _city.Add(suppCity);
                    //}

                    //_OrganizationSetupModel.cityLists = _city;
                    //_city.Insert(0, new CityList() { CityId = "0", CityName = "---Select---" });
                    //_OrganizationSetupModel.cityLists = _city;

                    /******START  DOCUMENT sETUP dATA BIND*******/
                    DataTable dt2 = new DataTable();
                    List<DocList> docLists = new List<DocList>();
                    dt2 = Getdoclist();
                    foreach (DataRow dr in dt2.Rows)
                    {
                        DocList _CLList = new DocList();
                        _CLList.Doc_id = dr["doc_id"].ToString();
                        _CLList.DOC_Name = dr["doc_name_eng"].ToString();
                        docLists.Add(_CLList);
                    }
                    docLists.Insert(0, new DocList() { Doc_id = "0", DOC_Name = "---Select---" });
                    _OrganizationSetupModel.DocumentList = docLists;
                    if (TempData["BtnName"] == null)
                    {
                        _OrganizationSetupModel.BtnName = "BtnRefresh";
                        //_OrganizationSetupModel.TransType = "Refresh";
                        _OrganizationSetupModel.TransType = "Update";

                    }
                    if (_OrganizationSetupModel.TransType == "Update" || _OrganizationSetupModel.Command == "Edit")
                    {
                        string EntityName = _OrganizationSetupModel.EntityName;
                        var com_ID = _OrganizationSetupModel.comp_id;
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                        DataSet ds = _OrganizationSetup_ISERVICE.GetviewCOM(com_ID);
                        if (ds.Tables[0].Rows[0]["HO_comp_id"].ToString() == "0")
                        {
                            _OrganizationSetupModel.HeadBranch = "H";
                            _OrganizationSetupModel.Quantity = ds.Tables[1].Rows[0]["qty_digit"].ToString();
                            _OrganizationSetupModel.Quantity_Value = ds.Tables[1].Rows[0]["val_digit"].ToString();
                            _OrganizationSetupModel.Rate = ds.Tables[1].Rows[0]["rate_digit"].ToString();
                            _OrganizationSetupModel.Weight = ds.Tables[1].Rows[0]["wgt_digit"].ToString();
                            _OrganizationSetupModel.Exchange = ds.Tables[1].Rows[0]["exch_digit"].ToString();
                        }
                        else
                        {
                            _OrganizationSetupModel.HeadBranch = "B";
                        }
                        ViewBag.CompAddressDetail = ds.Tables[2];/*Add by Hina sharma on 06-08-2025*/
                        //OSCompDetail.Supp_Type = ds.Tables[0].Rows[0]["HO_comp_id"].ToString();
                        _OrganizationSetupModel.comp_id = Convert.ToInt32(ds.Tables[0].Rows[0]["Comp_Id"]);
                        _OrganizationSetupModel.EntityName = ds.Tables[0].Rows[0]["comp_nm"].ToString();
                        _OrganizationSetupModel.EntityPrefix = ds.Tables[0].Rows[0]["comp_cd"].ToString();
                        _OrganizationSetupModel.HeadOffice = ds.Tables[0].Rows[0]["HO_comp_id"].ToString();
                        _OrganizationSetupModel.FY_StartDate = ds.Tables[0].Rows[0]["fin_st_dt"].ToString();
                        _OrganizationSetupModel.FY_EndDate = ds.Tables[0].Rows[0]["fin_end_dt"].ToString();
                        _OrganizationSetupModel.def_lang = ds.Tables[0].Rows[0]["def_lang"].ToString();
                        //_OrganizationSetupModel.Currency_name = ds.Tables[0].Rows[0]["bs_curr_nm"].ToString();
                        _OrganizationSetupModel.Currency_name = ds.Tables[0].Rows[0]["bs_curr_id"].ToString();
                        _OrganizationSetupModel.cont_pers = ds.Tables[0].Rows[0]["cont_per1"].ToString();
                        _OrganizationSetupModel.cont_num1 = ds.Tables[0].Rows[0]["cont_num1"].ToString();
                        _OrganizationSetupModel.cont_email = ds.Tables[0].Rows[0]["email_id1"].ToString();
                        _OrganizationSetupModel.cont_pers1 = ds.Tables[0].Rows[0]["cont_per2"].ToString();
                        _OrganizationSetupModel.cont_num2 = ds.Tables[0].Rows[0]["cont_num2"].ToString();
                        _OrganizationSetupModel.cont_email1 = ds.Tables[0].Rows[0]["email_id2"].ToString();
                        /*----------commented start by Hina on 02-08-2025-----------------*/
                        //_OrganizationSetupModel.Address = ds.Tables[0].Rows[0]["Comp_Add"].ToString();
                        //_OrganizationSetupModel.Pin = ds.Tables[0].Rows[0]["pin"].ToString();
                        //_OrganizationSetupModel.cust_city = ds.Tables[0].Rows[0]["city"].ToString();
                        //_OrganizationSetupModel.District = ds.Tables[0].Rows[0]["district_name"].ToString();
                        //_OrganizationSetupModel.District = ds.Tables[0].Rows[0]["district"].ToString();
                        //_OrganizationSetupModel.State = ds.Tables[0].Rows[0]["state_name"].ToString();
                        //_OrganizationSetupModel.Cus_State = ds.Tables[0].Rows[0]["state"].ToString();
                        //_OrganizationSetupModel.Country = ds.Tables[0].Rows[0]["country_name"].ToString();
                        //_OrganizationSetupModel.cust_Country = ds.Tables[0].Rows[0]["country"].ToString();
                        //_OrganizationSetupModel.State = ds.Tables[0].Rows[0]["state"].ToString();
                        //_OrganizationSetupModel.Cus_State = ds.Tables[0].Rows[0]["state_name"].ToString();
                        //_OrganizationSetupModel.Country = ds.Tables[0].Rows[0]["country"].ToString();
                        //_OrganizationSetupModel.cust_Country = ds.Tables[0].Rows[0]["country_name"].ToString();
                        /*----------commented End by Hina on 02-08-2025-----------------*/
                        _OrganizationSetupModel.PANNumber = ds.Tables[0].Rows[0]["pan_no"].ToString();
                        _OrganizationSetupModel.bank_benef = ds.Tables[0].Rows[0]["benif_name"].ToString();
                        _OrganizationSetupModel.bank_name = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        _OrganizationSetupModel.bank_add = ds.Tables[0].Rows[0]["bank_add"].ToString();
                        _OrganizationSetupModel.bank_acc_no = ds.Tables[0].Rows[0]["acc_no"].ToString();
                        _OrganizationSetupModel.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                        _OrganizationSetupModel.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();

                        _OrganizationSetupModel.MSME_Number = ds.Tables[0].Rows[0]["msme_no"].ToString();
                        _OrganizationSetupModel.WebSite = ds.Tables[0].Rows[0]["comp_website"].ToString();

                        _OrganizationSetupModel.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _OrganizationSetupModel.create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _OrganizationSetupModel.img_six = ds.Tables[0].Rows[0]["logo"].ToString();
                        _OrganizationSetupModel.Currency_Formet = ds.Tables[0].Rows[0]["curr_format"].ToString();
                        _OrganizationSetupModel.Currency_Formet_id = ds.Tables[0].Rows[0]["curr_format"].ToString();
                        _OrganizationSetupModel.IEC_Code = ds.Tables[0].Rows[0]["iec_no"].ToString();
                        if (ds.Tables[0].Rows[0]["logo"].ToString() == "" || ds.Tables[0].Rows[0]["logo"].ToString() == null)
                        {
                            _OrganizationSetupModel.img_six = ds.Tables[0].Rows[0]["logo"].ToString();
                            _OrganizationSetupModel.attatchmentdetail = null;
                        }
                        else
                        {
                            // string server_Url = "";
                            string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                            string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                            if (Request.Url.Host == localIp)
                                serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                            else
                                serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                            _OrganizationSetupModel.attatchmentdetail = serverUrl + ds.Tables[0].Rows[0]["logo"].ToString();
                            _OrganizationSetupModel.hdnAttachment = ds.Tables[0].Rows[0]["logo"].ToString();
                            _OrganizationSetupModel.hdnAttachment1 = ds.Tables[0].Rows[0]["digi_sign"].ToString();
                        }
                        _OrganizationSetupModel.GSTNumber = ds.Tables[0].Rows[0]["gst_no"].ToString();
                        if (_OrganizationSetupModel.GSTNumber != "")
                        {

                            //string GSTNumber = _OrganizationSetupModel.gst_num;
                            _OrganizationSetupModel.gst_num = _OrganizationSetupModel.GSTNumber.Substring(0, 2).Trim();
                            _OrganizationSetupModel.GSTMidPrt = _OrganizationSetupModel.GSTNumber.Substring(2, 10).Trim();
                            _OrganizationSetupModel.GSTLastPrt = _OrganizationSetupModel.GSTNumber.Substring(12, 3).Trim();
                        }
                        _OrganizationSetupModel.mod_by = ds.Tables[0].Rows[0]["mod_by"].ToString();
                        _OrganizationSetupModel.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();

                        ViewBag.licenseDetails = ds.Tables[3];

                        /*------------------------------------------Code Start of Country,state,district,city--------------------------*/

                        List<CmnCountryList> _ContryListR = new List<CmnCountryList>();
                        //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                        List<Country> _ContryList2R = new List<Country>();
                        CommonAddress_Detail _ModelR = new CommonAddress_Detail();

                        dt = GetCountryList();

                        foreach (DataRow dr in dt.Rows)
                        {
                            CmnCountryList _Contry = new CmnCountryList();
                            _Contry.country_id = dr["country_id"].ToString();
                            _Contry.country_name = dr["country_name"].ToString();
                            _ContryListR.Add(_Contry);
                            _ContryList2R.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                        }
                        _ModelR.countryList = _ContryList2R;
                        _OrganizationSetupModel.countryList = _ContryListR;

                        List<CmnStateList> stateR = new List<CmnStateList>();
                        state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                        string transCountryR = "";
                        if (!string.IsNullOrEmpty(_OrganizationSetupModel.Country))
                            transCountryR = _OrganizationSetupModel.Country;
                        else
                            transCountryR = dtcntry.Rows[0]["country_id"].ToString();

                        dt = _OrganizationSetup_ISERVICE.GetstateOnCountryDDL(transCountryR);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                stateR.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                            }
                        }
                        _OrganizationSetupModel.StateList = stateR;

                        string transStateR = "0";
                        List<CmnDistrictList> DistListR = new List<CmnDistrictList>();
                        DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                        if (!string.IsNullOrEmpty(_OrganizationSetupModel.State))
                            transStateR = _OrganizationSetupModel.State;
                        else
                            transStateR = "0";
                        dt = _OrganizationSetup_ISERVICE.GetDistrictOnStateDDL(transStateR);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                DistListR.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                            }
                        }
                        _OrganizationSetupModel.DistrictList = DistListR;

                        string transDistR = "0";
                        if (!string.IsNullOrEmpty(_OrganizationSetupModel.District))
                            transDistR = _OrganizationSetupModel.District;
                        else
                            transDistR = "0";
                        dt = _OrganizationSetup_ISERVICE.GetCityOnDistrictDDL(transDistR);

                        List<CmnCityList> citiesR = new List<CmnCityList>();
                        cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                citiesR.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                            }
                        }
                        _OrganizationSetupModel.cityLists = citiesR;

                        _OrganizationSetupModel._CommonAddress_Detail = _ModelR;
                        /*------------------------------------------Code End of Country,state,district,city--------------------------*/

                    }

                    else
                    {
                        _OrganizationSetupModel.Command = "Add";
                        _OrganizationSetupModel.TransType = "Save";
                        _OrganizationSetupModel.BtnName = "BtnAddNew";
                    }
                    if (_OrganizationSetupModel.Message == "Duplicate" || _OrganizationSetupModel.Message == "DuplicateUpdate")
                    {
                       
                            
                       
                        _OrganizationSetupModel.Command = "Add";
                        if (_OrganizationSetupModel.Message == "Duplicate")
                        {
                            _OrganizationSetupModel.TransType = "Duplicate";
                            _OrganizationSetupModel.comp_id = 0;
                        }
                        else if (_OrganizationSetupModel.Message == "DuplicateUpdate")
                        {
                            _OrganizationSetupModel.TransType = "DuplicateUpdate";
                            _OrganizationSetupModel.comp_id = _OrganizationSetupModel.comp_id;
                        }

                        _OrganizationSetupModel.BtnName = "BtnAddNew";
                        _OrganizationSetupModel.Message = "Duplicate";
                    }
                 
                    if (_OrganizationSetupModel.Quantity == null)
                    {
                        _OrganizationSetupModel.Quantity = "2";
                    }
                    if (_OrganizationSetupModel.Quantity_Value == null)
                    {
                        _OrganizationSetupModel.Quantity_Value = "2";
                    }
                    if (_OrganizationSetupModel.Rate == null)
                    {
                        _OrganizationSetupModel.Rate = "2";
                    }
                    if (_OrganizationSetupModel.Weight == null)
                    {
                        _OrganizationSetupModel.Weight = "2";
                    }
                    if (_OrganizationSetupModel.Exchange == null)
                    {
                        _OrganizationSetupModel.Exchange = "2";
                    }
                    ViewBag.VBRoleList = GetRoleList();
                    ViewBag.MenuPageName = getDocumentName();
                    _OrganizationSetupModel.Title = title;
                    return View("~/Areas/FactorySettings/Views/OrganizationSetup/OrganizationSetup.cshtml", _OrganizationSetupModel);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult OSTreeView(string Comp_Id)
        {
            try
            {
                OrganizationSetupModel _OrganizationSetupModel = new OrganizationSetupModel();
                _OrganizationSetupModel.comp_id = Convert.ToInt32(Comp_Id);
                _OrganizationSetupModel.TransType = "Update";
                _OrganizationSetupModel.Command = "Test";
                _OrganizationSetupModel.BtnName = "BtnSave";
                TempData["ModelData"] = _OrganizationSetupModel;
                return RedirectToAction("OrganizationSetup");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public JsonResult GetAllHoBranchGrp(OrganizationSetupModel _OrganizationSetupModel)
        {
            try
            {
                string Comp_ID = string.Empty;
                //if (Session["CompId"] != null)
                //{
                //    _OrganizationSetupModel.Comp_ID = Session["CompId"].ToString();
                //}
                JsonResult DataRows = null;
                string FinalData = string.Empty;
                Newtonsoft.Json.Linq.JObject FData = new Newtonsoft.Json.Linq.JObject();
                FData = _OrganizationSetup_ISERVICE.GetAllHoBranchGrp(_OrganizationSetupModel);
                DataRows = Json(JsonConvert.SerializeObject(FData), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult OrganizationSetupSave(OrganizationSetupModel _OrganizationSetupModel, string command, HttpPostedFileBase img_six, HttpPostedFileBase img_digi_sign)
        {
            try
            {

                //if (_OrganizationSetupModel.DeleteCommand == "Delete")
                //{
                //    command = "Delete";
                //}
                switch (command)
                {

                    //case "Delete":
                    //    _OrganizationSetupModel.Command = command;
                    //    _OrganizationSetupModel.BtnName = "Delete";
                    //    //item_grp_id = Convert.ToInt32(_ItemGroupModel.item_grp_id).ToString();
                    //    DeleteItemGroup(_OrganizationSetupModel);
                    //    OrganizationSetupModel _organizationSetupModelDelete = new OrganizationSetupModel();
                    //    _organizationSetupModelDelete.Message = "Deleted";
                    //    _organizationSetupModelDelete.Command = "Delete";
                    //    _organizationSetupModelDelete.TransType = "Refresh";
                    //    _organizationSetupModelDelete.BtnName = "Delete";
                    //    TempData["ModelData"] = _organizationSetupModelDelete;
                    //    return RedirectToAction("OrganizationSetup");
                    case "Save":
                        _OrganizationSetupModel.Command = command;
                        DateTime now = DateTime.Now;                       
                        string timeWithMilliseconds = now.ToString("HHmmssfff");
                        string randomFiveDigit = timeWithMilliseconds;
                        if (ModelState.IsValid)
                        {
                          
                            if (img_six != null)
                            {
                                Guid gid = new Guid();
                                gid = Guid.NewGuid();

                                // string imgpath = Server.MapPath("~/Attachment/OrganizationSetup");
                                // string imgpath = Server.MapPath("~/Content/images");
                                string imgpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + "CompanyLogo");
                                string imgname = Path.GetFileName(img_six.FileName);
                                var Abc = imgname.Split('.');
                                string Abc1 = Abc[0].ToString();
                                string Abc2 = Abc[1].ToString();
                                string newname = imgname.Replace(Abc1, "Logo");
                                string newname1 = _OrganizationSetupModel.EntityName.Replace(" ", "")
                                    + '_' + randomFiveDigit + '_' + newname;
                             
                                if (!Directory.Exists(imgpath))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(imgpath);
                                }
                                string fPath = "/" + "CompanyLogo" + "/" + newname1;
                                _OrganizationSetupModel.attatchmentdetail = fPath;

                            }
                            if (img_six == null)
                            {
                                if (_OrganizationSetupModel.hdnAttachment != null)
                                {
                                    _OrganizationSetupModel.attatchmentdetail = _OrganizationSetupModel.hdnAttachment;
                                }
                            }
                            
                            if (img_digi_sign != null)
                            {
                                Guid gid = new Guid();
                                gid = Guid.NewGuid();

                                // string imgpath = Server.MapPath("~/Attachment/OrganizationSetup");
                                // string imgpath = Server.MapPath("~/Content/images");
                                string imgpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + "CompanyLogo");
                                string imgname = Path.GetFileName(img_digi_sign.FileName);
                                var Abc = imgname.Split('.');
                                string Abc1 = Abc[0].ToString();
                                string Abc2 = Abc[1].ToString();
                                string newname = imgname.Replace(Abc1, "DigiSign");
                                string newname1 = _OrganizationSetupModel.EntityName.Replace(" ", "")
                                      + '_' + randomFiveDigit + '_' + newname;
                                if (!Directory.Exists(imgpath))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(imgpath);
                                }
                                string fPath = "/" + "CompanyLogo" + "/" + newname1;
                                _OrganizationSetupModel.img_digi_sign = fPath;

                            }
                            if (img_digi_sign == null)
                            {
                                if (_OrganizationSetupModel.hdnAttachment1 != null)
                                {
                                    _OrganizationSetupModel.img_digi_sign = _OrganizationSetupModel.hdnAttachment1;
                                }
                            }
                            InsertOrganizationSetup(_OrganizationSetupModel, img_six);
                        }

                        if (_OrganizationSetupModel.Message == "Duplicate" || _OrganizationSetupModel.Message == "DuplicateUpdate" || _OrganizationSetupModel.Message== "DuplicatePerfix")
                        {

                           

                           

                            // _OrganizationSetupModel.attatchmentdetail = null;
                            _OrganizationSetupModel.TransType = "Save";
                            _OrganizationSetupModel.Message = _OrganizationSetupModel.Message;
                            _OrganizationSetupModel.BtnName = "BtnAddNew";
                            _OrganizationSetupModel.Command = "Add";
                            TempData["ModelData"] = _OrganizationSetupModel;
                            return RedirectToAction("OrganizationSetup");

                      

                        }
                        if (_OrganizationSetupModel.Message == "Save")
                        {
                            string imgpath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + "CompanyLogo");
                            if (img_six != null)
                            {
                                
                                string imgname = Path.GetFileName(img_six.FileName);
                                var Abc = imgname.Split('.');
                                string Abc1 = Abc[0].ToString();
                                string Abc2 = Abc[1].ToString();
                                string newname = imgname.Replace(Abc1, "Logo");
                                string newname1 = _OrganizationSetupModel.EntityName.Replace(" ", "")
                                   + '_' + randomFiveDigit + '_' + newname;
                                string imagesave1 = Path.Combine(imgpath + "\\" + newname1);
                                img_six.SaveAs(imagesave1);
                            }
                            
                            if (img_digi_sign != null)
                            {
                                string imgname = Path.GetFileName(img_digi_sign.FileName);
                                var Abc = imgname.Split('.');
                                string Abc1 = Abc[0].ToString();
                                string Abc2 = Abc[1].ToString();
                                string newname = imgname.Replace(Abc1, "DigiSign");
                                string newname1 = _OrganizationSetupModel.EntityName.Replace(" ", "")
                                     + '_' + randomFiveDigit + '_' + newname;
                                string imagesave1 = Path.Combine(imgpath + "\\" + newname1);
                                img_digi_sign.SaveAs(imagesave1);
                            }
                            _OrganizationSetupModel.TransType = "Update";
                            _OrganizationSetupModel.BtnName = "BtnSave";
                            _OrganizationSetupModel.Command = command;
                            _OrganizationSetupModel.Message = "Save";
                            TempData["ModelData"] = _OrganizationSetupModel;
                            return RedirectToAction("OrganizationSetup");
                        }

                        return RedirectToAction("OrganizationSetup");


                    case "Edit":
                        //_OrganizationSetupModel.Message = "";
                        _OrganizationSetupModel.Command = command;
                        _OrganizationSetupModel.comp_id = _OrganizationSetupModel.comp_id;
                        _OrganizationSetupModel.TransType = "Update";
                        _OrganizationSetupModel.BtnName = "BtnEdit";
                        _OrganizationSetupModel.commEdit = command;
                        TempData["ModelData"] = _OrganizationSetupModel;
                        return RedirectToAction("OrganizationSetup");
                    case "Add":
                        _OrganizationSetupModel.Message = "";
                        _OrganizationSetupModel.Command = command;
                        _OrganizationSetupModel.TransType = "Save";
                        _OrganizationSetupModel.BtnName = "BtnAddNew";
                        TempData["BtnName"] = _OrganizationSetupModel.BtnName;
                        _OrganizationSetupModel.comp_id = 0;
                        TempData["ModelData"] = null;
                        return RedirectToAction("OrganizationSetup");
                    case "Refresh":
                        OrganizationSetupModel _OrganizationSetupModelRefresh = new OrganizationSetupModel();
                        _OrganizationSetupModelRefresh.BtnName = "BtnRefresh";
                        _OrganizationSetupModelRefresh.Command = command;
                        _OrganizationSetupModelRefresh.TransType = "Refresh";
                        _OrganizationSetupModelRefresh.Message = "";
                        TempData["ModelData"] = _OrganizationSetupModelRefresh;

                        return RedirectToAction("OrganizationSetup");
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        return RedirectToAction("OrganizationSetup");
                }
                return RedirectToAction("OrganizationSetup");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        //public ActionResult DeleteItemGroup(OrganizationSetupModel _OrganizationSetupModel)
        //{
        //    try
        //    {
        //        JsonResult Result = Json("");
        //        //ViewBag.PageHeader = PageHeader;

        //        //if (Session["CompId"] != null)
        //        //{
        //        //    compid = int.Parse(Session["CompId"].ToString());
        //        //}
        //        int comid = _OrganizationSetupModel.comp_id;
        //        string status = _OrganizationSetup_ISERVICE.DeleteOSComDetail(comid);
        //        _OrganizationSetupModel.Message = "Deleted";
        //        _OrganizationSetupModel.Command = "Delete";
        //        _OrganizationSetupModel.comp_id = 0;
        //        _OrganizationSetupModel.TransType = "Refresh";
        //        _OrganizationSetupModel.BtnName = "Delete";
        //        return RedirectToAction("OrganizationSetupSave", _OrganizationSetupModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //}
        public ActionResult InsertOrganizationSetup(OrganizationSetupModel _OrganizationSetupModel, HttpPostedFileBase img_six)
        {
            try
            {
                string SaveMessage = "";
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }

                if (_OrganizationSetupModel.TransType == "Duplicate" || _OrganizationSetupModel.TransType == "DuplicateUpdate")
                {
                    if (_OrganizationSetupModel.comp_id == 0)
                    {
                        //Session["TransType"] = "Save";
                        _OrganizationSetupModel.TransType = "Save";
                    }
                    else
                    {
                        //Session["TransType"] = "Update";
                        _OrganizationSetupModel.TransType = "Update";
                    }
                }
                if (_OrganizationSetupModel.TransType == null)
                {
                    if (_OrganizationSetupModel.comp_id != 0)
                    {
                        _OrganizationSetupModel.TransType = "Update";
                    }
                    else
                    {
                        _OrganizationSetupModel.TransType = "Save";
                    }
                }
                DataTable ODDetail = new DataTable();
                DataTable ODQuantity = new DataTable();
                DataTable ORGAddressDetail = new DataTable();
                DataTable LicenceDetail = new DataTable();
                DataTable dt = new DataTable();

                dt.Columns.Add("TransType", typeof(string));
                //dt.Columns.Add("Comp_Id", typeof(string));
                dt.Columns.Add("comp_cd", typeof(string));
                dt.Columns.Add("comp_nm", typeof(string));
                dt.Columns.Add("gst_no", typeof(string));
                dt.Columns.Add("pan_no", typeof(string));
                dt.Columns.Add("comp_alias", typeof(string));
                dt.Columns.Add("HO_comp_id", typeof(int));
                dt.Columns.Add("HO_comp_nm", typeof(string));
                dt.Columns.Add("fin_st_dt", typeof(string));
                dt.Columns.Add("fin_end_dt", typeof(string));
                dt.Columns.Add("def_lang", typeof(string));
                dt.Columns.Add("bs_curr_nm", typeof(string));
                dt.Columns.Add("bs_curr_id", typeof(string));
                dt.Columns.Add("cont_per1", typeof(string));
                dt.Columns.Add("cont_num1", typeof(string));
                dt.Columns.Add("email_id1", typeof(string));
                dt.Columns.Add("cont_per2", typeof(string));
                dt.Columns.Add("cont_num2", typeof(string));
                dt.Columns.Add("email_id2", typeof(string));
                dt.Columns.Add("Comp_Add", typeof(string));
                dt.Columns.Add("create_by", typeof(string));
                dt.Columns.Add("create_id", typeof(int));
                dt.Columns.Add("create_dt", typeof(string));
                dt.Columns.Add("comp_prefix", typeof(string));
                dt.Columns.Add("print_name", typeof(string));
                dt.Columns.Add("pin", typeof(string));
                dt.Columns.Add("city", typeof(int));
                dt.Columns.Add("district", typeof(int));
                dt.Columns.Add("state", typeof(int));
                dt.Columns.Add("country", typeof(int));
                dt.Columns.Add("logo", typeof(string));
                dt.Columns.Add("benif_name", typeof(string));
                dt.Columns.Add("bank_name", typeof(string));
                dt.Columns.Add("bank_add", typeof(string));
                dt.Columns.Add("acc_no", typeof(string));
                dt.Columns.Add("swift_code", typeof(string));
                dt.Columns.Add("ifsc_code", typeof(string));
                dt.Columns.Add("mod_id", typeof(int));
                dt.Columns.Add("mod_by", typeof(string));
                dt.Columns.Add("msme_no", typeof(string));
                dt.Columns.Add("comp_website", typeof(string));
                dt.Columns.Add("digi_sign", typeof(string));
                dt.Columns.Add("curr_format", typeof(string));
                dt.Columns.Add("IEC_Code", typeof(string));
                //DataRow dtrow = dt.NewRow();

                DataRow dtrowHeader = dt.NewRow();

                dtrowHeader["TransType"] = _OrganizationSetupModel.TransType;
                //dtrowHeader["comp_id"] = _OrganizationSetupModel.comp_id;

                dtrowHeader["HO_comp_nm"] = _OrganizationSetupModel.ho_com_name;
                dtrowHeader["create_dt"] = "";


                //dtrowHeader["comp_alias"] = _OrganizationSetupModel.comp_alias; 
                dtrowHeader["comp_nm"] = _OrganizationSetupModel.EntityName;
                dtrowHeader["comp_cd"] = _OrganizationSetupModel.EntityPrefix;
                dtrowHeader["print_name"] = _OrganizationSetupModel.EntityName;
                dtrowHeader["comp_prefix"] = _OrganizationSetupModel.EntityPrefix;
                dtrowHeader["fin_st_dt"] = _OrganizationSetupModel.FY_StartDate;
                dtrowHeader["fin_end_dt"] = _OrganizationSetupModel.FY_EndDate;
                dtrowHeader["def_lang"] = _OrganizationSetupModel.def_lang;
                dtrowHeader["bs_curr_nm"] = _OrganizationSetupModel.Curren_name;
                dtrowHeader["bs_curr_id"] = _OrganizationSetupModel.Currency_id;
                dtrowHeader["cont_per1"] = _OrganizationSetupModel.cont_pers;
                dtrowHeader["cont_num1"] = _OrganizationSetupModel.cont_num1;
                dtrowHeader["email_id1"] = _OrganizationSetupModel.cont_email;
                dtrowHeader["cont_per2"] = _OrganizationSetupModel.cont_pers1;
                dtrowHeader["cont_num2"] = _OrganizationSetupModel.cont_num2;
                dtrowHeader["email_id2"] = _OrganizationSetupModel.cont_email1;
                dtrowHeader["gst_no"] = _OrganizationSetupModel.GSTNumber;
                if(_OrganizationSetupModel.PANNumber==null)
                {
                    dtrowHeader["pan_no"] = "";
                }
                else
                {
                    dtrowHeader["pan_no"] = _OrganizationSetupModel.PANNumber;
                }
                
                dtrowHeader["Comp_Add"] = _OrganizationSetupModel.Address;
                dtrowHeader["logo"] = _OrganizationSetupModel.attatchmentdetail;
                dtrowHeader["pin"] = _OrganizationSetupModel.Pin;
                //dtrowHeader["city"] =Convert.ToInt32( _OrganizationSetupModel.cust_city);
                //dtrowHeader["district"] =Convert.ToInt32( _OrganizationSetupModel.District);
                //dtrowHeader["state"] =Convert.ToInt32( _OrganizationSetupModel.Cus_State);
                //dtrowHeader["country"] =Convert.ToInt32( _OrganizationSetupModel.cust_Country);
                dtrowHeader["city"] = Convert.ToInt32(_OrganizationSetupModel.City);
                dtrowHeader["district"] = Convert.ToInt32(_OrganizationSetupModel.District);
                dtrowHeader["state"] = Convert.ToInt32(_OrganizationSetupModel.State);
                dtrowHeader["country"] = Convert.ToInt32(_OrganizationSetupModel.Country);
                dtrowHeader["benif_name"] = _OrganizationSetupModel.bank_benef;
                dtrowHeader["bank_name"] = _OrganizationSetupModel.bank_name;
                dtrowHeader["bank_add"] = _OrganizationSetupModel.bank_add;
                dtrowHeader["acc_no"] = _OrganizationSetupModel.bank_acc_no;
                dtrowHeader["swift_code"] = _OrganizationSetupModel.swift_code;
                dtrowHeader["ifsc_code"] = _OrganizationSetupModel.ifsc_code;
                dtrowHeader["comp_alias"] = _OrganizationSetupModel.comp_alias;
                dtrowHeader["msme_no"] = _OrganizationSetupModel.MSME_Number;
                dtrowHeader["comp_website"] = _OrganizationSetupModel.WebSite;
                dtrowHeader["digi_sign"] = _OrganizationSetupModel.img_digi_sign;
                if (_OrganizationSetupModel.HeadBranch == "H")
                {
                    dtrowHeader["HO_comp_id"] = 0;
                    dtrowHeader["curr_format"] = "0";
                }
                else
                {
                    //dtrowHeader["HO_comp_nm"] = _OrganizationSetupModel.HeadOffice;
                    dtrowHeader["HO_comp_id"] = Convert.ToInt32(_OrganizationSetupModel.HeadOffice);
                    dtrowHeader["curr_format"] = _OrganizationSetupModel.Currency_Formet_id;
                  
                }
                dtrowHeader["IEC_Code"] = _OrganizationSetupModel.IEC_Code;
                dtrowHeader["create_id"] = Session["UserId"].ToString();

                dtrowHeader["create_by"] = Session["UserId"].ToString();
                dtrowHeader["mod_id"] = Session["UserId"].ToString();
                dtrowHeader["mod_by"] = Session["UserId"].ToString();

                dt.Rows.Add(dtrowHeader);
                ODDetail = dt;

                /*-------CODE START ADD BY HINA SHARMA ON 05-08-2025 TO ADD MULTIPLE ADDRESS-----------*/
                DataTable dtAddress = new DataTable();
                
                dtAddress.Columns.Add("comp_add_id", typeof(int));
                dtAddress.Columns.Add("org_add", typeof(string));
                dtAddress.Columns.Add("org_cntry", typeof(int));
                dtAddress.Columns.Add("org_state", typeof(int));
                dtAddress.Columns.Add("org_dist", typeof(int));
                dtAddress.Columns.Add("org_city", typeof(int));
                dtAddress.Columns.Add("org_pin", typeof(string));
                dtAddress.Columns.Add("def_add", typeof(string));
                //dtAddress.Columns.Add("Flag", typeof(string));

                JArray AddObject = JArray.Parse(_OrganizationSetupModel.OrgAddressDetails);
                for (int i = 0; i < AddObject.Count; i++)
                {
                    DataRow dtrowAddress = dtAddress.NewRow();
                    if (!string.IsNullOrEmpty(AddObject[i]["comp_add_id"].ToString()))
                    {
                        dtrowAddress["comp_add_id"] = AddObject[i]["comp_add_id"].ToString();
                    }
                    else
                    {
                        dtrowAddress["comp_add_id"] = "0";
                    }
                    //dtrowAddress["comp_add_id"] = AddObject[i]["comp_add_id"].ToString();
                    dtrowAddress["org_add"] = AddObject[i]["address"].ToString();
                    dtrowAddress["org_cntry"] = AddObject[i]["Country"].ToString();
                    dtrowAddress["org_state"] = AddObject[i]["State"].ToString();
                    dtrowAddress["org_dist"] = AddObject[i]["District"].ToString();
                    dtrowAddress["org_city"] = AddObject[i]["City"].ToString();
                    dtrowAddress["org_pin"] = AddObject[i]["pin"].ToString();
                    dtrowAddress["def_add"] = AddObject[i]["DefAddress"].ToString();
                    //dtrowAddress["Flag"] = AddObject[i]["Flag"].ToString();

                    dtAddress.Rows.Add(dtrowAddress);
                }
                ORGAddressDetail = dtAddress;
                /*-------CODE END ADD BY HINA SHARMA ON 05-08-2025 TO ADD MULTIPLE ADDRESS-----------*/
                /***---------------------------------------------------------------***/
                DataTable dtLicence = new DataTable();

                dtLicence.Columns.Add("lic_id", typeof(int));
                dtLicence.Columns.Add("lic_name", typeof(string));
                dtLicence.Columns.Add("lic_number", typeof(string));
                dtLicence.Columns.Add("exp_dt", typeof(string));
                dtLicence.Columns.Add("alert_days", typeof(int));

                if (_OrganizationSetupModel.hdnLincenceDetail != null)
                {
                    JArray AddObj = JArray.Parse(_OrganizationSetupModel.hdnLincenceDetail);
                    for (int i = 0; i < AddObj.Count; i++)
                    {
                        DataRow dtrowAddress = dtLicence.NewRow();
                        dtrowAddress["lic_id"] = AddObj[i]["l_id"].ToString();
                        dtrowAddress["lic_name"] = AddObj[i]["LicenseNm"].ToString();
                        dtrowAddress["lic_number"] = AddObj[i]["LicenseNum"].ToString();
                        dtrowAddress["exp_dt"] = AddObj[i]["ExpDate"].ToString();
                        dtrowAddress["alert_days"] =Convert.ToInt32(AddObj[i]["ExpiryAlrtDays"]);
                        dtLicence.Rows.Add(dtrowAddress);
                    }
                    LicenceDetail = dtLicence;
                }
                
                /***---------------------------------------------------------------***/


                DataTable dt1 = new DataTable();
                dt1.Columns.Add("TransType", typeof(string));
                if (_OrganizationSetupModel.TransType == "Update")
                {
                    dt1.Columns.Add("Comp_id", typeof(int));
                }
                else
                {
                    dt1.Columns.Add("comp_id", typeof(string));
                }
                dt1.Columns.Add("qty_digit", typeof(string));
                dt1.Columns.Add("val_digit", typeof(string));
                dt1.Columns.Add("rate_digit", typeof(string));
                dt1.Columns.Add("wgt_digit", typeof(string));
                dt1.Columns.Add("exch_digit", typeof(string));


                DataRow dtrowHeader1 = dt1.NewRow();
                dtrowHeader1["TransType"] = _OrganizationSetupModel.TransType;
                if (_OrganizationSetupModel.TransType == "Update")
                {
                    dtrowHeader1["comp_id"] = _OrganizationSetupModel.comp_id;
                }
                else
                {
                    dtrowHeader1["comp_id"] = CompID;
                }
                dtrowHeader1["qty_digit"] = Convert.ToInt32(_OrganizationSetupModel.Quantity);
                dtrowHeader1["val_digit"] = Convert.ToInt32(_OrganizationSetupModel.Quantity_Value);
                dtrowHeader1["rate_digit"] = Convert.ToInt32(_OrganizationSetupModel.Rate);
                dtrowHeader1["wgt_digit"] = Convert.ToInt32(_OrganizationSetupModel.Weight);
                dtrowHeader1["exch_digit"] = Convert.ToInt32(_OrganizationSetupModel.Exchange);


                dt1.Rows.Add(dtrowHeader1);
                ODQuantity = dt1;
                SaveMessage = _OrganizationSetup_ISERVICE.InsertOS_Data(ODDetail, ODQuantity, _OrganizationSetupModel.LandlineNumber, ORGAddressDetail, LicenceDetail);


                var data = SaveMessage.Split('-');
                var Message = data[0].Trim();
                _OrganizationSetupModel.comp_id = Convert.ToInt32(data[1].Trim());
                if (Message == "Update" || Message == "Save")
                {
                    if (Message == "Update")
                    {
                        if (img_six != null)
                        {

                            if (_OrganizationSetupModel.hdnAttachment != null)
                            {
                                var ImgName = (_OrganizationSetupModel.hdnAttachment).Replace("/", "\\");
                                string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment" + ImgName;
                                if (System.IO.File.Exists(AttachmentFilePath))
                                {
                                    System.IO.File.Delete(AttachmentFilePath);
                                }
                            }

                        }

                        if (_OrganizationSetupModel.img_digi_sign != null)
                        {


                            var ImgName = (_OrganizationSetupModel.img_digi_sign).Replace("/", "\\");
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment" + ImgName;
                            if (System.IO.File.Exists(AttachmentFilePath))
                            {
                                System.IO.File.Delete(AttachmentFilePath);
                            }

                        }
                    }
                    _OrganizationSetupModel.Message = "Save";
                    _OrganizationSetupModel.TransType = "Update";
                }
                if (Message == "Duplicate" || Message=="DuplicateUpdate" || Message == "DuplicatePerfix")
                {

                    _OrganizationSetupModel.TransType = "Save";
                    _OrganizationSetupModel.Message = Message;
                    _OrganizationSetupModel.comp_id = _OrganizationSetupModel.comp_id;
                }
                TempData["ModelData"] = _OrganizationSetupModel;
                return RedirectToAction("OrganizationSetupSave", _OrganizationSetupModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable GetRoleList()
        {
            try
            {
                string UserID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataTable GetAutoCompleteSuppcity(OrganizationSetupModel _OrganizationSetupModel)
        {
            try
            {
                string CompID = string.Empty;
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                string GroupName = string.Empty;

                if (string.IsNullOrEmpty(_OrganizationSetupModel.cust_city))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = _OrganizationSetupModel.cust_city;
                }
                DataTable dt = _OrganizationSetup_ISERVICE.SuppCityDAL(GroupName);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetAutoCompleteCity(SearchSupp queryParameters)
        {
            Dictionary<string, string> SuppCity = new Dictionary<string, string>();
            try
            {
                string GroupName = string.Empty;

                if (string.IsNullOrEmpty(queryParameters.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.ddlGroup;
                }

                SuppCity = _OrganizationSetup_ISERVICE.OSCityList(GroupName);
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
                DataSet HoCompData = _OrganizationSetup_ISERVICE.GetsuppDSCntrDAL(SuppCity);
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
        private string getDocumentName()
        {
            OrganizationSetupModel _OrganizationSetupModel = new OrganizationSetupModel();
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
                _OrganizationSetupModel.Title = title;
                return DocumentName;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable GetCurrencySetup()
        {
            try
            {

                DataTable dt = _OrganizationSetup_ISERVICE.GetCurrencyList();
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable Getdoclist()
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                DataTable dt = _OrganizationSetup_ISERVICE.GetdOCNAME_ENG(CompID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetFinst_dt_End_dt_bs_curr(string Headoffice_id) //added by Nitesh 21-10-2023 1413 for headoffice accoding fin_stdt and enddt,curr
        {
            JsonResult DataRows = null;
            try
            {
                OrganizationSetupModel _OrganizationSetupModel = new OrganizationSetupModel();
                DataSet ho_stdt_enddt = _OrganizationSetup_ISERVICE.getst_dt_end_dt(Headoffice_id);
                DataRows = Json(JsonConvert.SerializeObject(ho_stdt_enddt));/*Result convert into Json Format for javasript*/
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
        public JsonResult CheckDependcy(string Entityprefix,string flag,string comp_id, string RoleHoName, string Branchchk) 
        {
            JsonResult DataRows = null;
            try
            {
                OrganizationSetupModel _OrganizationSetupModel = new OrganizationSetupModel();
                DataSet entity_prefix = _OrganizationSetup_ISERVICE.GetDataCheckDepency(Entityprefix, flag, comp_id, RoleHoName, Branchchk);
                DataRows = Json(JsonConvert.SerializeObject(entity_prefix));/*Result convert into Json Format for javasript*/
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
        public JsonResult CheckDependcyEntityName(string Entityname, string flag,string comp_id,string RoleHoName,string Branchchk)
        {
            JsonResult DataRows = null;
            try
            {
                OrganizationSetupModel _OrganizationSetupModel = new OrganizationSetupModel();
                DataSet entity_name = _OrganizationSetup_ISERVICE.GetDataCheckDepency(Entityname, flag, comp_id, RoleHoName, Branchchk);
                DataRows = Json(JsonConvert.SerializeObject(entity_name));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        /*----------------------Code start of Country,state,district,city--------------------------*/
        [NonAction]
        private DataTable GetCountryList()
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _OrganizationSetup_ISERVICE.GetCountryListDDL();
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                dt.Rows.InsertAt(dr, 0);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        //[HttpPost]
        //public JsonResult GetCountryonChngPros(string ProspectType)
        //{
        //    JsonResult DataRows = null;
        //    try
        //    {
        //        List<CmnCountryList> _TransList = new List<CmnCountryList>();
        //        //_OSCompDetail = new OSCompDetail();
        //        string Comp_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        DataTable dt = _OrganizationSetup_ISERVICE.GetCountryListDDL(Comp_ID, ProspectType);
        //        if (ProspectType == "E")
        //        {
        //            DataRow dr;
        //            dr = dt.NewRow();
        //            dr[0] = "0";
        //            dr[1] = "---Select---";
        //            dt.Rows.InsertAt(dr, 0);
        //        }
        //        DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/

        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return DataRows;
        //}

        [HttpPost]
        public JsonResult GetstateOnCountry(string ddlCountryID)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable dt = _OrganizationSetup_ISERVICE.GetstateOnCountryDDL(ddlCountryID);
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
                DataTable dt = _OrganizationSetup_ISERVICE.GetDistrictOnStateDDL(ddlStateID);
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
                DataTable dt = _OrganizationSetup_ISERVICE.GetCityOnDistrictDDL(ddlDistrictID);
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
                DataSet ds = _OrganizationSetup_ISERVICE.GetStateCode(stateId);
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

    }

}