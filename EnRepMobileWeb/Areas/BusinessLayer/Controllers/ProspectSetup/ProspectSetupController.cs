using EnRepMobileWeb.MODELS.BusinessLayer.ProspectSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.ProspectSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ProductCatalouge;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesEnquiry;
//***Modifyed by Shubham Maurya on 06-01-2023 for Remove Session ***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.ProspectSetup
{
    public class ProspectSetupController : Controller
    {
        DomesticSalesQuotationModel _DomesticSalesQuotationModel;
        string CompID,UserID, BranchName, Br_ID, language = String.Empty;
        string DocumentMenuId = "103123", title, pros_id;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ProspectSetupMODEL _ProspectSetupMODEL;
        ProspectSetup_ISERVICES prospectSetup_ISERVICES;
        DataTable dt;
        public ProspectSetupController(Common_IServices _Common_IServices, ProspectSetup_ISERVICES prospectSetup_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this.prospectSetup_ISERVICES = prospectSetup_ISERVICES;
        }
        // GET: BusinessLayer/ProspectSetup
        public ActionResult ProspectSetup()
        {
            try
            {
                string branch_id = string.Empty;
                ProspectSetupMODEL prospectSetupMODEL = new ProspectSetupMODEL();
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                //Session["Message"] = "New";
                //Session["Command"] = "Add";
                //Session["TransType"] = "Save";
                //Session["BtnName"] = "BtnAddNew";
                //Session["ProspectFromQuot"] = "N";
                //Session["ProspectFromRFQ"] = "N";

                prospectSetupMODEL.AttachMentDetailItmStp = null;
                prospectSetupMODEL.Guid = null;
                prospectSetupMODEL.Message = "New";
                prospectSetupMODEL.Command = "Add";
                prospectSetupMODEL.TransType = "Save";
                prospectSetupMODEL.BtnName = "BtnAddNew";
                prospectSetupMODEL.ProspectFromQuot = "N";
                prospectSetupMODEL.ProspectFromRFQ = "N";
                
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                //prospectSetupMODEL.br = Br_ID;
                DataSet ds = prospectSetup_ISERVICES.GetProspectDetailsList(pros_id, CompID, Br_ID);
                ViewBag.Pros_List = ds.Tables[0];
                //dt= prospectSetup_ISERVICES.GetProspectDetailsList(pros_id, CompID, Br_ID).Tables[1];
                ViewBag.AttechmentDetails = ds.Tables[1];
                List<Branch> _BranchList = new List<Branch>();
                dt = GetBranchList();

                DataRow DRow = dt.NewRow();
                DRow["Comp_id"] = "0";
                DRow["comp_nm"] = "---All---";
                dt.Rows.InsertAt(DRow, 0);

                foreach (DataRow dr in dt.Rows)
                {
                    Branch _Branch = new Branch();
                    _Branch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    _Branch.br_val = dr["comp_nm"].ToString();
                    _BranchList.Add(_Branch);
                }
                prospectSetupMODEL.BranchList = _BranchList;
                prospectSetupMODEL.br = Int16.Parse(Br_ID);
                ViewBag.MenuPageName = getDocumentName();
                prospectSetupMODEL.Title = title;
                prospectSetupMODEL.DocumentMenuId = DocumentMenuId;
                CommonPageDetails();
                return View("~/Areas/BusinessLayer/Views/ProspectSetup/ProspectSetupList.cshtml", prospectSetupMODEL);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult ProspectListFilter(string brId)
        {
            try
            {
                ProspectSetupMODEL prospectSetupMODEL = new ProspectSetupMODEL();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                //TempData["S_ProsList"] = "PS";
                prospectSetupMODEL.S_ProsList = "PS";
                dt = prospectSetup_ISERVICES.GetProspectDetailsList(pros_id, CompID, brId).Tables[0];
                ViewBag.Pros_List = dt;
                prospectSetupMODEL.DocumentMenuId = DocumentMenuId;
                return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialProspectSetupList.cshtml", prospectSetupMODEL);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult dblckicktoDetail(ProspectSetupMODEL prospectSetupMODEL, string Pros_id,string BranchId)
        {
            try
            {

                //    Session["ProsCode"] = Pros_id;
                //    Session["TransType"] = "Update";
                //    Session["BtnName"] = "BtnSave";
                //Session["ProspectFromQuot"] = "N";
                //Session["ProspectFromRFQ"] = "N";
                //Session["ProspectFromPQ"] = "N";
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                prospectSetupMODEL.ProsCode = Pros_id;
                prospectSetupMODEL.TransType= "Update";
                prospectSetupMODEL.BtnName = "BtnToDetailPage";
                prospectSetupMODEL.ProspectFromQuot = "N";
                prospectSetupMODEL.ProspectFromRFQ = "N";
                prospectSetupMODEL.ProspectFromPQ = "N";
                
                prospectSetupMODEL.AttachMentDetailItmStp = null;
                prospectSetupMODEL.Guid = null;

                /*Commented by Hina on 03-01-2024 to change Bind city on behalf of district*/

                //Dictionary<string, string> CityListD = new Dictionary<string, string>();
                //CityListD = prospectSetup_ISERVICES.GetCityList("");
                //List<ProspectCity> prospectCities = new List<ProspectCity>();
                //if (CityListD.Count > 0)
                //{
                //    foreach (var dr in CityListD)
                //    {
                //        ProspectCity CityList = new ProspectCity();
                //        CityList.city_id = dr.Key;
                //        CityList.city_name = dr.Value;
                //        prospectCities.Add(CityList);
                //    }
                //}

                //prospectCities.Insert(0, new ProspectCity() { city_id = "0", city_name = "---Select---" });
                //prospectSetupMODEL.ProspectCities = prospectCities;
                CommonPageDetails();
                List<curr> _CustcurrList = new List<curr>();
                string curr = prospectSetupMODEL.pros_type;

                if (curr != null)
                {
                    dt = GetProspectCurrency(curr);
                }
                else
                {
                    dt = GetProspectCurrency("D");
                }

                foreach (DataRow dr in dt.Rows)
                {
                    curr _Custcurr = new curr();
                    _Custcurr.curr_id = dr["curr_id"].ToString();
                    _Custcurr.curr_name = dr["curr_name"].ToString();
                    _CustcurrList.Add(_Custcurr);

                }
                prospectSetupMODEL.currList = _CustcurrList;

                List<Branch> _BranchList = new List<Branch>();
                dt = GetBranchList();
                foreach (DataRow dr in dt.Rows)
                {
                    Branch _Branch = new Branch();
                    _Branch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    _Branch.br_val = dr["comp_nm"].ToString();
                    _BranchList.Add(_Branch);
                }
                prospectSetupMODEL.BranchList = _BranchList;

                //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                if (prospectSetupMODEL.TransType == "Update" || prospectSetupMODEL.Command == "Edit")
                {
                    //string ProsCode = Session["ProsCode"].ToString();
                    string ProsCode = prospectSetupMODEL.ProsCode;
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (BranchId != null && BranchId!="")
                    {
                        if (Br_ID != BranchId)
                        {
                            //Session["BtnName"] = "BtnRefresh";
                        }
                    }
                    else
                    {
                        BranchId = Br_ID;
                    }

                    DataSet ds = prospectSetup_ISERVICES.Getviewprospectdetail(ProsCode, CompID, BranchId);
                    ViewBag.AttechmentDetails = ds.Tables[4];
                    prospectSetupMODEL.Entity_type = ds.Tables[0].Rows[0]["entity_type"].ToString();
                    prospectSetupMODEL.pros_type = ds.Tables[0].Rows[0]["pros_type"].ToString();
                    prospectSetupMODEL.pros_id = ds.Tables[0].Rows[0]["pros_id"].ToString();
                    prospectSetupMODEL.ProspectName = ds.Tables[0].Rows[0]["pros_name"].ToString();
                    prospectSetupMODEL.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                    prospectSetupMODEL.br =Convert.ToInt32(ds.Tables[0].Rows[0]["br_id"].ToString());
                    prospectSetupMODEL.ContactNumber = ds.Tables[0].Rows[0]["cont_num"].ToString();
                    prospectSetupMODEL.Email = ds.Tables[0].Rows[0]["cont_email"].ToString();
                    prospectSetupMODEL.ContactPerson = ds.Tables[0].Rows[0]["cont_person"].ToString();
                    prospectSetupMODEL.Address = ds.Tables[0].Rows[0]["address"].ToString();
                    prospectSetupMODEL.Pin = ds.Tables[0].Rows[0]["pin"].ToString();
                    prospectSetupMODEL.Country = ds.Tables[0].Rows[0]["cntry"].ToString();
                    prospectSetupMODEL.State = ds.Tables[0].Rows[0]["state"].ToString();
                    prospectSetupMODEL.District = ds.Tables[0].Rows[0]["dist"].ToString();
                    prospectSetupMODEL.City = ds.Tables[0].Rows[0]["cust_city"].ToString();
                    prospectSetupMODEL.GSTNumber = ds.Tables[0].Rows[0]["gst_no"].ToString();
                    prospectSetupMODEL.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                    prospectSetupMODEL.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                    prospectSetupMODEL.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                    prospectSetupMODEL.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                    prospectSetupMODEL.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                    prospectSetupMODEL.Gst_Cat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                    //Session["EntityType"] = ds.Tables[0].Rows[0]["entity_type"].ToString();
                    //Session["ProspectDetail"] = ds;
                    TempData["ProspectDetail"] = ds;
                    prospectSetupMODEL.EntityType = ds.Tables[0].Rows[0]["entity_type"].ToString();
                    prospectSetupMODEL.ProspectDetail = ds;
                    if (prospectSetupMODEL.GSTNumber != "")
                    {
                        prospectSetupMODEL.gst_num = prospectSetupMODEL.GSTNumber.Substring(0, 2);
                        prospectSetupMODEL.GSTMidPrt = prospectSetupMODEL.GSTNumber.Substring(2, 10);
                        prospectSetupMODEL.GSTLastPrt = prospectSetupMODEL.GSTNumber.Substring(12, 3);
                    }

                    curr = prospectSetupMODEL.pros_type;
                     dt = GetProspectCurrency(curr);
                    _CustcurrList = new List<curr>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        curr _Custcurr = new curr();
                        _Custcurr.curr_id = dr["curr_id"].ToString();
                        _Custcurr.curr_name = dr["curr_name"].ToString();
                        _CustcurrList.Add(_Custcurr);
                    }
                    prospectSetupMODEL.currList = _CustcurrList;
  /*------------------------------------------Code start of Country,state,district,city--------------------------*/
                    List<CmnCountryList> _TransList = new List<CmnCountryList>();
                    _TransList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                    List<Country> _TransList2 = new List<Country>();
                    CommonAddress_Detail _Model = new CommonAddress_Detail();
                    string transportType = "D";
                    if (!string.IsNullOrEmpty(prospectSetupMODEL.pros_type))
                        transportType = prospectSetupMODEL.pros_type;
                    DataTable dtcntry = GetCountryList(transportType);

                    foreach (DataRow dr in dtcntry.Rows)
                    {
                        CmnCountryList _Custcurr = new CmnCountryList();
                        _Custcurr.country_id = dr["country_id"].ToString();
                        _Custcurr.country_name = dr["country_name"].ToString();
                        _TransList.Add(_Custcurr);
                        _TransList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _Model.countryList = _TransList2;

                    prospectSetupMODEL.countryList = _TransList;
                    List<CmnStateList> state = new List<CmnStateList>();
                    state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountry = "";
                    if (!string.IsNullOrEmpty(prospectSetupMODEL.Country))
                        transCountry = prospectSetupMODEL.Country;
                    else
                        transCountry = dtcntry.Rows[0]["country_id"].ToString();

                    DataTable dtStates = prospectSetup_ISERVICES.GetstateOnCountryDDL(transCountry);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL.StateList = state;

                    string transState = "0";
                    List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                    DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(prospectSetupMODEL.State))
                        transState = prospectSetupMODEL.State;
                    else
                        transState = "0";
                    DataTable dtDist = prospectSetup_ISERVICES.GetDistrictOnStateDDL(transState);
                    if (dtDist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDist.Rows)
                        {
                            DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL.DistrictList = DistList;

                    string transDist = "0";
                    if (!string.IsNullOrEmpty(prospectSetupMODEL.District))
                        transDist = prospectSetupMODEL.District;
                    else
                        transDist = "0";
                    DataTable dtCities = prospectSetup_ISERVICES.GetCityOnDistrictDDL(transDist);

                    List<CmnCityList> cities = new List<CmnCityList>();
                    cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dtCities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCities.Rows)
                        {
                            cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL.cityLists = cities;

                    prospectSetupMODEL._CommonAddress_Detail = _Model;
                    /*----------------------Code End of Country,state,district,city--------------------------*/
                }

                ViewBag.MenuPageName = getDocumentName();
                prospectSetupMODEL.Title = title;
                prospectSetupMODEL.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/BusinessLayer/Views/ProspectSetup/ProspectSetupDetail.cshtml", prospectSetupMODEL);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return null;
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult AddProspectSetup()
        {
            ProspectSetupMODEL prospectSetupMODEL = new ProspectSetupMODEL();
            prospectSetupMODEL.Message = "New";
            prospectSetupMODEL.Command = "Add";
            prospectSetupMODEL.AppStatus = "D";
            prospectSetupMODEL.TransType = "Save";
            prospectSetupMODEL.BtnName = "BtnAddNew";
            TempData["ModelData"] = prospectSetupMODEL;
            TempData["ListFilterData"] = null;
            return RedirectToAction("AddProspectSetupDetail");
        }
        public ActionResult CreateReferencePageProspectSetup(string ProspectFromProd,string SrcDocumentMenuID) /**Added By Nitesh 20-02-2024 for Product Cataloge**/
        {
            ProspectSetupMODEL prospectSetupMODEL1 = new ProspectSetupMODEL();
            prospectSetupMODEL1.ScrDocumentMenuID = SrcDocumentMenuID;
            prospectSetupMODEL1.ProspectFromProd = ProspectFromProd;
            return RedirectToAction("AddProspectSetupDetail", "ProspectSetup",prospectSetupMODEL1);
        }
        public ActionResult AddProspectSetupDetail(ProspectSetupMODEL prospectSetupMODEL1, string ProspectFromQuot,string ProspectFromPQ,string ProspectFromRFQ,string ProsCodeURL, string TransType, string BtnName, string command,string Srcdocument=null,string QuotationDocumentMenuId=null)
        {
            try
            {
                //TempData["QuotationDocumentMenuId"] = QuotationDocumentMenuId;
                if (TempData["QuotationDocumentMenuId"] != null)
                {
                    QuotationDocumentMenuId = TempData["QuotationDocumentMenuId"].ToString();
                }
                var prospectSetupMODEL = TempData["ModelData"] as ProspectSetupMODEL;
                if (prospectSetupMODEL != null)
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    CommonPageDetails();
                    prospectSetupMODEL.AttachMentDetailItmStp = null;
                    prospectSetupMODEL.Guid = null;
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (Session["BranchName"] != null)
                    {
                        BranchName = Session["BranchName"].ToString();
                    }

                    //if (Session["ProspectFromQuot"] == null)
                    if (prospectSetupMODEL.ProspectFromQuot == null)
                    {
                        //Session["ProspectFromQuot"] = ProspectFromQuot;
                        prospectSetupMODEL.ProspectFromQuot = ProspectFromQuot;
                    }
                    var value = "";
                    //if (Session["ProspectFromQuot"] == null)
                    if (prospectSetupMODEL.ProspectFromQuot == null)
                    {
                        //value = Session["ProspectFromQuot"].ToString();
                        value = prospectSetupMODEL.ProspectFromQuot;
                    }
                    if(prospectSetupMODEL.ProspectFromProd == "Y")
                    {
                        ProspectFromQuot = prospectSetupMODEL.ProspectFromProd;
                        prospectSetupMODEL.ProspectFromQuot= prospectSetupMODEL.ProspectFromProd;

                    }
                    if (ProspectFromQuot == "Y")
                    {
                        //Session["Command"] = "Add";
                        prospectSetupMODEL.Command = "Add";
                    }
                    prospectSetupMODEL.Entity_type = ProspectFromQuot;
                    if(Srcdocument != null)
                    {
                        prospectSetupMODEL.ScrDocumentMenuID = Srcdocument;
                    }
                    if (QuotationDocumentMenuId != null)
                    {
                        prospectSetupMODEL.QuotationDocumentMenuId = QuotationDocumentMenuId;
                    }
                    /*-------------------------------------Code start of Country,state,district,city--------------------------*/
                    List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                   
                    List<Country> _ContryList2 = new List<Country>();
                    CommonAddress_Detail _Model = new CommonAddress_Detail();
                    string ProspectType = "";
                    if (prospectSetupMODEL.pros_type == null|| prospectSetupMODEL.pros_type == "D")
                    {
                        ProspectType = "D";
                    }
                    else
                    {
                        ProspectType = prospectSetupMODEL.pros_type;
                        _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                    }
                    //string ProspectType = "D";
                    //if(ProspectType != "D")
                    //{
                    //    _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                    //}
                    //if (!string.IsNullOrEmpty(prospectSetupMODEL.pros_type))
                    // ProspectType = prospectSetupMODEL.pros_type;

                    DataTable dtcntry = GetCountryList(ProspectType);

                    foreach (DataRow dr in dtcntry.Rows)
                    {
                        CmnCountryList _Custcurr = new CmnCountryList();
                        _Custcurr.country_id = dr["country_id"].ToString();
                        _Custcurr.country_name = dr["country_name"].ToString();
                        _ContryList.Add(_Custcurr);
                        _ContryList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _Model.countryList = _ContryList2;
                    prospectSetupMODEL.countryList = _ContryList;

                    List<CmnStateList> state = new List<CmnStateList>();
                    state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountry = "";
                    if (!string.IsNullOrEmpty(prospectSetupMODEL.Country))
                        transCountry = prospectSetupMODEL.Country;
                    else
                        transCountry = dtcntry.Rows[0]["country_id"].ToString();

                    DataTable dtStates = prospectSetup_ISERVICES.GetstateOnCountryDDL(transCountry);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL.StateList = state;

                    string transState = "0";
                    List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                    DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(prospectSetupMODEL.State))
                        transState = prospectSetupMODEL.State;
                    else
                        transState = "0";
                    DataTable dtDist = prospectSetup_ISERVICES.GetDistrictOnStateDDL(transState);
                    if (dtDist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDist.Rows)
                        {
                            DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL.DistrictList = DistList;

                    string transDist = "0";
                    if (!string.IsNullOrEmpty(prospectSetupMODEL.District))
                        transDist = prospectSetupMODEL.District;
                    else
                        transDist = "0";
                    DataTable dtCities = prospectSetup_ISERVICES.GetCityOnDistrictDDL(transDist);

                    List<CmnCityList> cities = new List<CmnCityList>();
                    cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dtCities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCities.Rows)
                        {
                            cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL.cityLists = cities;

                    prospectSetupMODEL._CommonAddress_Detail = _Model;
   /*----------------------------------Code End of Country,state,district,city--------------------------*/
                    
                    /*Commented by Hina on 03-01-2024 to change Bind city on behalf of district*/

                    //Dictionary<string, string> CityListD = new Dictionary<string, string>();
                    //CityListD = prospectSetup_ISERVICES.GetCityList("");
                    //List<ProspectCity> prospectCities = new List<ProspectCity>();
                    //if (CityListD.Count > 0)
                    //{
                    //    foreach (var dr in CityListD)
                    //    {
                    //        ProspectCity CityList = new ProspectCity();
                    //        CityList.city_id = dr.Key;
                    //        CityList.city_name = dr.Value;
                    //        prospectCities.Add(CityList);
                    //    }
                    //}
                    //prospectCities.Insert(0, new ProspectCity() { city_id = "0", city_name = "---Select---" });
                    //prospectSetupMODEL.ProspectCities = prospectCities;

                    List<curr> _CustcurrList = new List<curr>();
                    string curr = prospectSetupMODEL.pros_type;

                    if (curr != null)
                    {
                        dt = GetProspectCurrency(curr);
                    }
                    else
                    {
                        dt = GetProspectCurrency("D");
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        curr _Custcurr = new curr();
                        _Custcurr.curr_id = dr["curr_id"].ToString();
                        _Custcurr.curr_name = dr["curr_name"].ToString();
                        _CustcurrList.Add(_Custcurr);
                    }
                    prospectSetupMODEL.currList = _CustcurrList;
                    List<Branch> _BranchList = new List<Branch>();
                    dt = GetBranchList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Branch _Branch = new Branch();
                        _Branch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                        _Branch.br_val = dr["comp_nm"].ToString();
                        _BranchList.Add(_Branch);
                    }
                    prospectSetupMODEL.BranchList = _BranchList;
                    prospectSetupMODEL.br = Convert.ToInt32(Br_ID);

                    //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                    if (prospectSetupMODEL.TransType == "Update" || prospectSetupMODEL.Command == "Edit")
                    {
                        //string ProsCode = Session["ProsCode"].ToString();
                        string ProsCode = prospectSetupMODEL.ProsCode;
                        if (prospectSetupMODEL.ProsCode == null)
                        {
                            ProsCode = prospectSetupMODEL.pros_id;
                        }
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                        if (Session["br_id"] != null)
                        {
                            if (Session["br_id"].ToString() != "0")
                            {
                                Br_ID = Session["Br_id"].ToString();
                            }
                            else
                            {
                                Br_ID = Session["BranchId"].ToString();
                            }
                        }
                        else
                        {
                            Br_ID = Session["BranchId"].ToString();
                        }
                        prospectSetupMODEL.br = Convert.ToInt32(Br_ID);
                        DataSet ds = prospectSetup_ISERVICES.Getviewprospectdetail(ProsCode, CompID, Br_ID);
                        if (ProsCode != null)
                        {
                            ViewBag.AttechmentDetails = ds.Tables[4];
                            prospectSetupMODEL.Entity_type = ds.Tables[0].Rows[0]["entity_type"].ToString();
                            prospectSetupMODEL.pros_type = ds.Tables[0].Rows[0]["pros_type"].ToString();
                            prospectSetupMODEL.pros_id = ds.Tables[0].Rows[0]["pros_id"].ToString();
                            prospectSetupMODEL.ProspectName = ds.Tables[0].Rows[0]["pros_name"].ToString();
                            prospectSetupMODEL.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            prospectSetupMODEL.ContactNumber = ds.Tables[0].Rows[0]["cont_num"].ToString();
                            prospectSetupMODEL.Email = ds.Tables[0].Rows[0]["cont_email"].ToString();
                            prospectSetupMODEL.ContactPerson = ds.Tables[0].Rows[0]["cont_person"].ToString();
                            prospectSetupMODEL.Address = ds.Tables[0].Rows[0]["address"].ToString();
                            prospectSetupMODEL.Pin = ds.Tables[0].Rows[0]["pin"].ToString();
                            prospectSetupMODEL.Country = ds.Tables[0].Rows[0]["cntry"].ToString();
                            prospectSetupMODEL.State = ds.Tables[0].Rows[0]["state"].ToString();
                            prospectSetupMODEL.District = ds.Tables[0].Rows[0]["dist"].ToString();
                            prospectSetupMODEL.City = ds.Tables[0].Rows[0]["cust_city"].ToString();
                            prospectSetupMODEL.GSTNumber = ds.Tables[0].Rows[0]["gst_no"].ToString();
                            prospectSetupMODEL.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            prospectSetupMODEL.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            prospectSetupMODEL.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            prospectSetupMODEL.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            prospectSetupMODEL.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            prospectSetupMODEL.Gst_Cat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            //Session["EntityType"] = ds.Tables[0].Rows[0]["entity_type"].ToString();
                            //Session["ProspectDetail"] = ds;
                            prospectSetupMODEL.EntityType = ds.Tables[0].Rows[0]["entity_type"].ToString();
                            prospectSetupMODEL.ProspectDetail = ds;
                            TempData["ProspectDetail"] = ds;
                            curr = prospectSetupMODEL.pros_type;
                            if (prospectSetupMODEL.GSTNumber != "")
                            {
                                prospectSetupMODEL.gst_num = prospectSetupMODEL.GSTNumber.Substring(0, 2);
                                prospectSetupMODEL.GSTMidPrt = prospectSetupMODEL.GSTNumber.Substring(2, 10);
                                prospectSetupMODEL.GSTLastPrt = prospectSetupMODEL.GSTNumber.Substring(12, 3);
                            }
                            dt = GetProspectCurrency(curr);

                            _CustcurrList = new List<curr>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                curr _Custcurr = new curr();
                                _Custcurr.curr_id = dr["curr_id"].ToString();
                                _Custcurr.curr_name = dr["curr_name"].ToString();
                                _CustcurrList.Add(_Custcurr);
                            }
                            prospectSetupMODEL.currList = _CustcurrList;

                            /*Commented by Hina on 03-01-2024 to change Bind city on behalf of district*/
                            //foreach (DataRow dr in ds.Tables[2].Rows)
                            //{
                            //    ProspectCity CityList = new ProspectCity();
                            //    CityList.city_id = dr["city_id"].ToString();
                            //    CityList.city_name = dr["city_name"].ToString();
                            //    //prospectCities.Add(CityList);
                            //}
                            //prospectSetupMODEL.ProspectCities = prospectCities;

                            //List<CmnCountryList> _TransList = new List<CmnCountryList>();
                            //_TransList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                            /*---------------------------------Code start of Country,state,district,city--------------------------*/
                            List<CmnCountryList> _TransList2 = new List<CmnCountryList>();
                            //CommonAddress_Detail _Model = new CommonAddress_Detail();
                            //string transportType = "D";
                            //if (!string.IsNullOrEmpty(prospectSetupMODEL.pros_type))
                            //    transportType = prospectSetupMODEL.pros_type;
                            string ProspectTypeEdt = "";
                            if (prospectSetupMODEL.pros_type == null || prospectSetupMODEL.pros_type == "D")
                            {
                                ProspectTypeEdt = "D";
                            }
                            else
                            {
                                ProspectTypeEdt = prospectSetupMODEL.pros_type;
                                _ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                            }
                            dt = GetCountryList(ProspectTypeEdt);

                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    CmnCountryList _Custcurr = new CmnCountryList();
                            //    _Custcurr.country_id = dr["country_id"].ToString();
                            //    _Custcurr.country_name = dr["country_name"].ToString();
                            //    _TransList2.Add(_Custcurr);
                            //    _TransList2.Add(new CmnCountryList { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                            //}
                            //prospectSetupMODEL.countryList = _TransList2;


                            List<CmnStateList> state1 = new List<CmnStateList>();
                            //state1.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                            string transCountry1 = "";
                            if (!string.IsNullOrEmpty(prospectSetupMODEL.Country))
                                transCountry1 = prospectSetupMODEL.Country;
                            else
                                transCountry1 = dt.Rows[0]["country_id"].ToString();

                            dt = prospectSetup_ISERVICES.GetstateOnCountryDDL(transCountry1);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    state1.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                                }
                            }
                            prospectSetupMODEL.StateList = state1;

                            string transState1 = "0";
                            List<CmnDistrictList> DistList1 = new List<CmnDistrictList>();
                            //DistList1.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                            if (!string.IsNullOrEmpty(prospectSetupMODEL.State))
                                transState1 = prospectSetupMODEL.State;
                            else
                                transState1 = "0";
                            dt = prospectSetup_ISERVICES.GetDistrictOnStateDDL(transState1);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    DistList1.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                                }
                            }
                            prospectSetupMODEL.DistrictList = DistList1;

                            string transDist1 = "0";
                            if (!string.IsNullOrEmpty(prospectSetupMODEL.District))
                                transDist1 = prospectSetupMODEL.District;
                            else
                                transDist1 = "0";
                            dt = prospectSetup_ISERVICES.GetCityOnDistrictDDL(transDist1);

                            List<CmnCityList> cities1 = new List<CmnCityList>();
                            //cities1.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    cities1.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                                }
                            }
                            prospectSetupMODEL.cityLists = cities1;

                            prospectSetupMODEL._CommonAddress_Detail = _Model;
                            /*-----------------------------------------Code End of Country,state,district,city--------------------------*/
                        }
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    prospectSetupMODEL.Title = title;
                    prospectSetupMODEL.hdnDeleteCommand = null;
                    prospectSetupMODEL.DocumentMenuId = DocumentMenuId;
                    return View("~/Areas/BusinessLayer/Views/ProspectSetup/ProspectSetupDetail.cshtml", prospectSetupMODEL);
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    CommonPageDetails();
                    prospectSetupMODEL1.AttachMentDetailItmStp = null;
                    prospectSetupMODEL1.Guid = null;
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (Session["BranchName"] != null)
                    {
                        BranchName = Session["BranchName"].ToString();
                    }

                    //if (Session["ProspectFromQuot"] == null)
                    if (prospectSetupMODEL1.ProspectFromQuot == null)
                    {
                        //Session["ProspectFromQuot"] = ProspectFromQuot;
                        prospectSetupMODEL1.ProspectFromQuot = ProspectFromQuot;
                    }
                    var value = "";
                    //if (Session["ProspectFromQuot"] == null)
                    if (prospectSetupMODEL1.ProspectFromQuot == null)
                    {
                        //value = Session["ProspectFromQuot"].ToString();
                        value = prospectSetupMODEL1.ProspectFromQuot;
                    }
                    if (prospectSetupMODEL1.ProspectFromProd == "Y")
                    {
                        ProspectFromQuot = prospectSetupMODEL1.ProspectFromProd;
                        prospectSetupMODEL1.ProspectFromQuot = prospectSetupMODEL1.ProspectFromProd;

                    }
                    if (ProspectFromQuot == "Y")
                    {
                        //Session["Command"] = "Add";
                        prospectSetupMODEL1.Command = "Add";
                    }
                    if (QuotationDocumentMenuId != null)
                    {
                        prospectSetupMODEL1.QuotationDocumentMenuId = QuotationDocumentMenuId.ToString();
                    }
                    prospectSetupMODEL1.Entity_type = ProspectFromQuot;
                    /*------------------------------------------Code Start of Country,state,district,city--------------------------*/

                    List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                    //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                    string CustomerType = "";
                    if (prospectSetupMODEL1.pros_type == null)
                    {
                        //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                        CustomerType = "D";
                    }
                    else
                    {
                        CustomerType = prospectSetupMODEL1.pros_type;
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
                    prospectSetupMODEL1.countryList = _ContryList;

                    List<CmnStateList> state = new List<CmnStateList>();
                    state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                    string transCountry = "";
                    if (!string.IsNullOrEmpty(prospectSetupMODEL1.Country))
                        transCountry = prospectSetupMODEL1.Country;
                    else
                        transCountry = dtcntry.Rows[0]["country_id"].ToString();

                    DataTable dtStates = prospectSetup_ISERVICES.GetstateOnCountryDDL(transCountry);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL1.StateList = state;

                    string transState = "0";
                    List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                    DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(prospectSetupMODEL1.State))
                        transState = prospectSetupMODEL1.State;
                    else
                        transState = "0";
                    DataTable dtDist = prospectSetup_ISERVICES.GetDistrictOnStateDDL(transState);
                    if (dtDist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDist.Rows)
                        {
                            DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL1.DistrictList = DistList;

                    string transDist = "0";
                    if (!string.IsNullOrEmpty(prospectSetupMODEL1.District))
                        transDist = prospectSetupMODEL1.District;
                    else
                        transDist = "0";
                    DataTable dtCities = prospectSetup_ISERVICES.GetCityOnDistrictDDL(transDist);

                    List<CmnCityList> cities = new List<CmnCityList>();
                    cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                    if (dtCities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCities.Rows)
                        {
                            cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    prospectSetupMODEL1.cityLists = cities;

                    prospectSetupMODEL1._CommonAddress_Detail = _Model;
                    /*------------------------------------------Code End of Country,state,district,city--------------------------*/


                    /*Commented by Hina on 03-01-2024 to change Bind city on behalf of district*/

                    //Dictionary<string, string> CityListD = new Dictionary<string, string>();
                    //CityListD = prospectSetup_ISERVICES.GetCityList("");
                    //List<ProspectCity> prospectCities = new List<ProspectCity>();
                    //if (CityListD.Count > 0)
                    //{
                    //    foreach (var dr in CityListD)
                    //    {
                    //        ProspectCity CityList = new ProspectCity();
                    //        CityList.city_id = dr.Key;
                    //        CityList.city_name = dr.Value;
                    //        prospectCities.Add(CityList);
                    //    }
                    //}
                    //prospectCities.Insert(0, new ProspectCity() { city_id = "0", city_name = "---Select---" });
                    //prospectSetupMODEL1.ProspectCities = prospectCities;

                    List<curr> _CustcurrList = new List<curr>();
                    string curr = prospectSetupMODEL1.pros_type;

                    if (curr != null)
                    {
                        dt = GetProspectCurrency(curr);
                    }
                    else
                    {
                        dt = GetProspectCurrency("D");
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        curr _Custcurr = new curr();
                        _Custcurr.curr_id = dr["curr_id"].ToString();
                        _Custcurr.curr_name = dr["curr_name"].ToString();
                        _CustcurrList.Add(_Custcurr);
                    }
                    prospectSetupMODEL1.currList = _CustcurrList;
                    List<Branch> _BranchList = new List<Branch>();
                    dt = GetBranchList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Branch _Branch = new Branch();
                        _Branch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                        _Branch.br_val = dr["comp_nm"].ToString();
                        _BranchList.Add(_Branch);
                    }
                    prospectSetupMODEL1.BranchList = _BranchList;
                    prospectSetupMODEL1.br = Convert.ToInt32(Br_ID);
                    if (TransType != null)
                    {
                        prospectSetupMODEL1.TransType = TransType;
                    }
                    if (command != null)
                    {
                        prospectSetupMODEL1.Command = command;
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["Command"].ToString() == "Edit")
                    if (prospectSetupMODEL1.TransType == "Update" || prospectSetupMODEL1.Command == "Edit")
                    {
                        var ProsCode = "";
                        if (ProsCodeURL != null)
                        {
                            prospectSetupMODEL1.ProsCode = ProsCodeURL;
                        }
                        else
                        {
                            ProsCode = prospectSetupMODEL1.ProsCode;
                        }
                        //string ProsCode = Session["ProsCode"].ToString();
                        //string ProsCode = prospectSetupMODEL1.ProsCode;
                        if (prospectSetupMODEL1.ProsCode == null)
                        {
                            ProsCode = prospectSetupMODEL1.pros_id;
                        }
                        if (prospectSetupMODEL1.ProsCode != null)
                        {
                            ProsCode = prospectSetupMODEL1.ProsCode;
                        }
                            if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                        if (Session["br_id"] != null)
                        {
                            if (Session["br_id"].ToString() != "0")
                            {
                                Br_ID = Session["Br_id"].ToString();
                            }
                            else
                            {
                                Br_ID = Session["BranchId"].ToString();
                            }
                        }
                        else
                        {
                            Br_ID = Session["BranchId"].ToString();
                        }
                        prospectSetupMODEL1.br = Convert.ToInt32(Br_ID);
                        DataSet ds = prospectSetup_ISERVICES.Getviewprospectdetail(ProsCode, CompID, Br_ID);
                        if (ProsCode != null)
                        {
                            ViewBag.AttechmentDetails = ds.Tables[4];
                            prospectSetupMODEL1.Entity_type = ds.Tables[0].Rows[0]["entity_type"].ToString();
                            prospectSetupMODEL1.pros_type = ds.Tables[0].Rows[0]["pros_type"].ToString();
                            prospectSetupMODEL1.pros_id = ds.Tables[0].Rows[0]["pros_id"].ToString();
                            prospectSetupMODEL1.ProspectName = ds.Tables[0].Rows[0]["pros_name"].ToString();
                            prospectSetupMODEL1.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            prospectSetupMODEL1.ContactNumber = ds.Tables[0].Rows[0]["cont_num"].ToString();
                            prospectSetupMODEL1.Email = ds.Tables[0].Rows[0]["cont_email"].ToString();
                            prospectSetupMODEL1.ContactPerson = ds.Tables[0].Rows[0]["cont_person"].ToString();
                            prospectSetupMODEL1.Address = ds.Tables[0].Rows[0]["address"].ToString();
                            prospectSetupMODEL1.Pin = ds.Tables[0].Rows[0]["pin"].ToString();
                            prospectSetupMODEL1.Country = ds.Tables[0].Rows[0]["cntry"].ToString();
                            prospectSetupMODEL1.State = ds.Tables[0].Rows[0]["state"].ToString();
                            prospectSetupMODEL1.District = ds.Tables[0].Rows[0]["dist"].ToString();
                            prospectSetupMODEL1.City = ds.Tables[0].Rows[0]["cust_city"].ToString();
                            prospectSetupMODEL1.GSTNumber = ds.Tables[0].Rows[0]["gst_no"].ToString();
                            prospectSetupMODEL1.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            prospectSetupMODEL1.create_id = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            prospectSetupMODEL1.creat_dt = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            prospectSetupMODEL1.mod_id = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            prospectSetupMODEL1.mod_dt = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            prospectSetupMODEL1.Gst_Cat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            //Session["EntityType"] = ds.Tables[0].Rows[0]["entity_type"].ToString();
                            //Session["ProspectDetail"] = ds;
                            TempData["ProspectDetail"] = ds;
                            prospectSetupMODEL1.EntityType = ds.Tables[0].Rows[0]["entity_type"].ToString();
                            prospectSetupMODEL1.ProspectDetail = ds;
                            curr = prospectSetupMODEL1.pros_type;
                            if (prospectSetupMODEL1.GSTNumber != "")
                            {
                                prospectSetupMODEL1.gst_num = prospectSetupMODEL1.GSTNumber.Substring(0, 2);
                                prospectSetupMODEL1.GSTMidPrt = prospectSetupMODEL1.GSTNumber.Substring(2, 10);
                                prospectSetupMODEL1.GSTLastPrt = prospectSetupMODEL1.GSTNumber.Substring(12, 3);
                            }
                            dt = GetProspectCurrency(curr);

                            _CustcurrList = new List<curr>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                curr _Custcurr = new curr();
                                _Custcurr.curr_id = dr["curr_id"].ToString();
                                _Custcurr.curr_name = dr["curr_name"].ToString();
                                _CustcurrList.Add(_Custcurr);
                            }
                            prospectSetupMODEL1.currList = _CustcurrList;

                            /*Commented by Hina on 03-01-2024 to change Bind city on behalf of district*/

                            //foreach (DataRow dr in ds.Tables[2].Rows)
                            //{
                            //    ProspectCity CityList = new ProspectCity();
                            //    CityList.city_id = dr["city_id"].ToString();
                            //    CityList.city_name = dr["city_name"].ToString();
                            //    prospectCities.Add(CityList);
                            //}
                            //prospectSetupMODEL1.ProspectCities = prospectCities;
                            /*------------------------------------------Code Start of Country,state,district,city--------------------------*/

                            List<CmnCountryList> _ContryListR = new List<CmnCountryList>();
                            //_ContryList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });

                            string CustomerTypeR = "";
                            if (prospectSetupMODEL1.pros_type == null|| prospectSetupMODEL1.pros_type == "D")
                            {
                                //List<CmnCountryList> _ContryList = new List<CmnCountryList>();
                                CustomerTypeR = "D";
                            }
                            else
                            {
                                CustomerTypeR = prospectSetupMODEL1.pros_type;
                                _ContryListR.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                            }
                            List<Country> _ContryList2R = new List<Country>();
                            CommonAddress_Detail _ModelR = new CommonAddress_Detail();

                             dt = GetCountryList(CustomerTypeR);

                            foreach (DataRow dr in dt.Rows)
                            {
                                CmnCountryList _Contry = new CmnCountryList();
                                _Contry.country_id = dr["country_id"].ToString();
                                _Contry.country_name = dr["country_name"].ToString();
                                _ContryListR.Add(_Contry);
                                _ContryList2R.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                            }
                            _ModelR.countryList = _ContryList2R;
                            prospectSetupMODEL1.countryList = _ContryListR;

                            List<CmnStateList> stateR = new List<CmnStateList>();
                            state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                            string transCountryR = "";
                            if (!string.IsNullOrEmpty(prospectSetupMODEL1.Country))
                                transCountryR = prospectSetupMODEL1.Country;
                            else
                                transCountryR = dtcntry.Rows[0]["country_id"].ToString();

                            dt = prospectSetup_ISERVICES.GetstateOnCountryDDL(transCountryR);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    stateR.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                                }
                            }
                            prospectSetupMODEL1.StateList = stateR;

                            string transStateR = "0";
                            List<CmnDistrictList> DistListR = new List<CmnDistrictList>();
                            DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                            if (!string.IsNullOrEmpty(prospectSetupMODEL1.State))
                                transStateR = prospectSetupMODEL1.State;
                            else
                                transStateR = "0";
                            dt = prospectSetup_ISERVICES.GetDistrictOnStateDDL(transStateR);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    DistListR.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                                }
                            }
                            prospectSetupMODEL1.DistrictList = DistListR;

                            string transDistR = "0";
                            if (!string.IsNullOrEmpty(prospectSetupMODEL1.District))
                                transDistR = prospectSetupMODEL1.District;
                            else
                                transDistR = "0";
                             dt = prospectSetup_ISERVICES.GetCityOnDistrictDDL(transDistR);

                            List<CmnCityList> citiesR = new List<CmnCityList>();
                            cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    citiesR.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                                }
                            }
                            prospectSetupMODEL1.cityLists = citiesR;

                            prospectSetupMODEL1._CommonAddress_Detail = _ModelR;
                            /*------------------------------------------Code End of Country,state,district,city--------------------------*/

                        }
                    }
                    if (prospectSetupMODEL1.BtnName == null)
                    {
                        prospectSetupMODEL1.BtnName = "BtnRefresh";
                    }
                    if (prospectSetupMODEL1.TransType == "Update")
                    {
                        prospectSetupMODEL1.BtnName = "BtnEdit";
                    }
                    if (BtnName != null)
                    {
                        prospectSetupMODEL1.BtnName = BtnName;
                    }
                    if (ProspectFromQuot !=null)
                    {
                        prospectSetupMODEL1.BtnName= "BtnAddNew";
                        prospectSetupMODEL1.Command= "Add";
                        prospectSetupMODEL1.TransType= "Save";
                        if (ProspectFromQuot == "Y")
                        {
                            prospectSetupMODEL1.ProspectFromQuot = ProspectFromQuot;
                        }
                        if (ProspectFromPQ != null)
                        {
                            prospectSetupMODEL1.ProspectFromPQ = ProspectFromPQ;
                        }
                        if (ProspectFromRFQ != null)
                        {
                            prospectSetupMODEL1.ProspectFromRFQ = ProspectFromPQ;
                        }
                        
                    }
                    //if (ProspectFromRFQ != null)
                    //{
                    //    prospectSetupMODEL1.BtnName = "BtnAddNew";
                    //    prospectSetupMODEL1.Command = "Add";
                    //    prospectSetupMODEL1.TransType = "Save";
                    //    prospectSetupMODEL1.ProspectFromRFQ = ProspectFromPQ;
                    //}
                    ViewBag.MenuPageName = getDocumentName();
                    prospectSetupMODEL1.Title = title;
                    prospectSetupMODEL1.hdnDeleteCommand = null;
                    prospectSetupMODEL1.DocumentMenuId = DocumentMenuId;
                    return View("~/Areas/BusinessLayer/Views/ProspectSetup/ProspectSetupDetail.cshtml", prospectSetupMODEL1);
                }                
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return null;
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
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
        public ActionResult ProspectSetupDetailSave(ProspectSetupMODEL prospectSetupMODEL,string command)
        {
            try
            {
                if (prospectSetupMODEL.hdnDeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "Edit":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["ProsCode"] = prospectSetupMODEL.pros_id;
                        //Session["br_id"] = prospectSetupMODEL.br;
                        //Session["TransType"] = "Update";
                        //Session["BtnName"] = "BtnEdit";
                        prospectSetupMODEL.Message = "";
                        prospectSetupMODEL.Command = command;
                        prospectSetupMODEL.ProsCode = prospectSetupMODEL.pros_id;
                        Session["br_id"] = prospectSetupMODEL.br;
                        prospectSetupMODEL.TransType = "Update";
                        prospectSetupMODEL.BtnName = "BtnEdit";
                        TempData["QuotationDocumentMenuId"] = prospectSetupMODEL.QuotationDocumentMenuId;
                        var Srcdocument = prospectSetupMODEL.ScrDocumentMenuID;
                        var ProsCodeURL = prospectSetupMODEL.ProsCode;
                        var TransType = prospectSetupMODEL.TransType;
                        var BtnName = prospectSetupMODEL.BtnName;
                        TempData["ModelData"] = prospectSetupMODEL;
                        return RedirectToAction("AddProspectSetupDetail", new { ProsCodeURL = ProsCodeURL, TransType, BtnName, command, Srcdocument });
                    case "Add":
                        //Session["Message"] = "";
                        //Session["Command"] = command;
                        //Session["ProsCode"] = "";
                        //Session["AppStatus"] = "D";
                        //  //prospectSetupMODEL = null;
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        prospectSetupMODEL.Message = "";
                        prospectSetupMODEL.Command = command;
                        prospectSetupMODEL.ProsCode = null;
                        prospectSetupMODEL.pros_id = null;
                        prospectSetupMODEL.AppStatus = "D";
                        //prospectSetupMODEL = null;
                        prospectSetupMODEL.TransType = "Save";
                        prospectSetupMODEL.BtnName = "BtnAddNew";
                        prospectSetupMODEL.Address = null;
                        prospectSetupMODEL.remarks = null;
                        prospectSetupMODEL.Gst_Cat = null;
                        prospectSetupMODEL.GSTNumber = null;
                        prospectSetupMODEL.gst_num = null;
                        prospectSetupMODEL.GSTMidPrt = null;
                        prospectSetupMODEL.GSTLastPrt = null;
                        TempData["ModelData"] = prospectSetupMODEL;
                        //Session["ProspectFromQuot"] = "N";
                        //Session["ProspectFromRFQ"] = "N";
                        return RedirectToAction("AddProspectSetupDetail");

                    case "Convert":
                        //Session["Message"] = "";
                        //Session["Command"] = command;                       
                        //Session["AppStatus"] = "D";                      
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        prospectSetupMODEL.Message = "";
                        prospectSetupMODEL.Command = command;
                        prospectSetupMODEL.AppStatus = "D";
                        prospectSetupMODEL.TransType = "Save";
                        prospectSetupMODEL.BtnName = "BtnAddNew";
                        //TempData["QuotationDocumentMenuId"] = prospectSetupMODEL.QuotationDocumentMenuId;
                        //if (Session["EntityType"] != null)
                        if (prospectSetupMODEL.EntityType != null)
                        {
                            //if (Session["EntityType"].ToString() == "C")
                            if (prospectSetupMODEL.EntityType == "C")
                            {
                                //Session["Message"] = "";
                                //Session["Command"] = "Add";
                                //Session["CustCode"] = "";
                                //Session["AppStatus"] = "D";
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                prospectSetupMODEL.Message = "";
                                prospectSetupMODEL.Command = "Add";
                                prospectSetupMODEL.CustCode = "";
                                prospectSetupMODEL.AppStatus = "D";
                                prospectSetupMODEL.TransType = "Save";
                                prospectSetupMODEL.BtnName = "BtnAddNew";
                                var custCodeURL = prospectSetupMODEL.CustCode;
                                TransType = prospectSetupMODEL.TransType;
                                BtnName = prospectSetupMODEL.BtnName;
                                command = "Add";
                                 Srcdocument = prospectSetupMODEL.ScrDocumentMenuID;
                                //TempData["ModelDataPRODPACT"] = prospectSetupMODEL;
                                return RedirectToAction("CustomerDetails", "CustomerDetails", new { custCodeURL = custCodeURL, TransType, BtnName, command, Srcdocument });
                            }
                            //if (Session["EntityType"].ToString() == "S")
                            if (prospectSetupMODEL.EntityType == "S")
                            {
                                //Session["Message"] = "";
                                //Session["Command"] = "Add";
                                //Session["SuppCode"] = "";
                                //Session["AppStatus"] = "D";
                                ////prospectSetupMODEL = null;
                                //Session["TransType"] = "Save";
                                //Session["BtnName"] = "BtnAddNew";
                                prospectSetupMODEL.Message = "";
                                prospectSetupMODEL.Command = "Add";
                                prospectSetupMODEL.SuppCode = "";
                                prospectSetupMODEL.AppStatus = "D";
                                prospectSetupMODEL.TransType = "Save";
                                prospectSetupMODEL.BtnName = "BtnAddNew";
                                var SuppCodeURL = prospectSetupMODEL.SuppCode;
                                TransType = prospectSetupMODEL.TransType;
                                BtnName = prospectSetupMODEL.BtnName;
                                command = "Add";
                                //TempData["ModelData"] = prospectSetupMODEL;
                                return RedirectToAction("SupplierDetail", "SupplierDetail", new { SuppCodeURL = SuppCodeURL, TransType, BtnName, command });
                            }
                        }
                    return RedirectToAction("AddProspectSetupDetail");


                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Delete";
                        prospectSetupMODEL.Command = command;
                        prospectSetupMODEL.BtnName = "Delete";
                        prospectSetupMODEL.Address = null;
                        prospectSetupMODEL.gst_num = null;
                        prospectSetupMODEL.GSTMidPrt = null;
                        prospectSetupMODEL.GSTLastPrt = null;
                        prospectSetupMODEL.GSTNumber = null;
                        prospectSetupMODEL.remarks = null;
                        //pros_id = prospectSetupMODEL.pros_id;
                        ProspectDetailDelete(prospectSetupMODEL, command);
                        TempData["ModelData"] = prospectSetupMODEL;
                        TempData["ProspectDetail"] = null;
                        return RedirectToAction("AddProspectSetupDetail");

                    case "Save":
                        //Session["Command"] = command;
                        prospectSetupMODEL.Command = command;
                        InsertProspectDetail(prospectSetupMODEL);
                        //if(Session["Message"].ToString() == "Duplicate")
                        if (prospectSetupMODEL.Message == "Duplicate" || prospectSetupMODEL.Message == "DuplicateGst_No")
                        {
                            TempData["ModelData"] = prospectSetupMODEL;
                            return RedirectToAction("ProspectDetailInDuplicateCase");
                        }
                        //Session["ProsCode"] = prospectSetupMODEL.pros_id;
                        //Session["br_id"] = prospectSetupMODEL.br;
                        ProsCodeURL = prospectSetupMODEL.ProsCode;
                        TransType = prospectSetupMODEL.TransType;
                        BtnName = prospectSetupMODEL.BtnName;
                         Srcdocument = prospectSetupMODEL.ScrDocumentMenuID;
                        TempData["QuotationDocumentMenuId"] = prospectSetupMODEL.QuotationDocumentMenuId;
                        TempData["ModelData"] = prospectSetupMODEL;
                        return RedirectToAction("AddProspectSetupDetail", new { ProsCodeURL = ProsCodeURL, TransType, BtnName, command, Srcdocument });
                    case "Refresh":
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Refresh";
                        //Session["Message"] = "";
                        //Session["AppStatus"] = "";
                        // prospectSetupMODEL = null;

                        prospectSetupMODEL.BtnName = "BtnRefresh";
                        prospectSetupMODEL.Command = command;
                        prospectSetupMODEL.TransType = "Refresh";
                        prospectSetupMODEL.Message = "";
                       prospectSetupMODEL.AppStatus = "";
                        prospectSetupMODEL.Address = null;
                        prospectSetupMODEL.remarks = null;
                        prospectSetupMODEL.Gst_Cat = null;
                        prospectSetupMODEL.ProspectName = null;
                        prospectSetupMODEL.ContactNumber = null;
                        prospectSetupMODEL.Email = null;
                        prospectSetupMODEL.ContactPerson = null;
                        prospectSetupMODEL.Country = null;
                        prospectSetupMODEL.State = null;
                        prospectSetupMODEL.District = null;
                        prospectSetupMODEL.City = null;
                        prospectSetupMODEL.Pin = null;
                        prospectSetupMODEL.GSTLastPrt = null;
                        prospectSetupMODEL.GSTMidPrt = null;
                        prospectSetupMODEL.gst_num = null;
                        TempData["QuotationDocumentMenuId"] = prospectSetupMODEL.QuotationDocumentMenuId;
                        TempData["ModelData"] = prospectSetupMODEL;
                        TempData["ProspectDetail"] = null;
                        return RedirectToAction("AddProspectSetupDetail");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        Session.Remove("br_id");
                        //if (Session["ProspectFromPQ"] == null)
                        if (prospectSetupMODEL.ProspectFromPQ == null)
                        {
                            //Session["ProspectFromPQ"] = "N";
                            prospectSetupMODEL.ProspectFromPQ = "N";
                        }
                        //if (Session["ProspectFromQuot"] == null)
                        if (prospectSetupMODEL.ProspectFromQuot == null)
                        {
                            //Session["ProspectFromQuot"] = "N";
                            prospectSetupMODEL.ProspectFromQuot = "N";
                        }
                        //if (Session["ProspectFromRFQ"] == null)
                        if (prospectSetupMODEL.ProspectFromRFQ == null)
                        {
                            //Session["ProspectFromRFQ"] = "N";
                            prospectSetupMODEL.ProspectFromRFQ = "N";
                        }
                        
                        if (prospectSetupMODEL.ProspectFromQuot == "Y" && prospectSetupMODEL.ScrDocumentMenuID== "105103185") /**Added By Nitesh 20-02-2024 for Product Cataloge**/
                        {
                            ProductCatalouge_Model _ProdCataModel = new ProductCatalouge_Model();
                            ViewBag.MenuPageName = getDocumentName();
                            _ProdCataModel.Title = title;
                            prospectSetupMODEL.ILSearch = "0";
                            prospectSetupMODEL.Message = "New";
                            prospectSetupMODEL.DocumentStatus = "D";
                            prospectSetupMODEL.BtnName = "BtnAddNew";
                            prospectSetupMODEL.TransType = "Save";
                            prospectSetupMODEL.Command = "New";
                            prospectSetupMODEL.ProspectFromQuot = null;
                            return RedirectToAction("AddProductCatalougeDetail", "ProductCatalouge", new { Area = "ApplicationLayer",Flag="Y" });
                        }
                        //if (Session["ProspectFromQuot"].ToString() == "Y")
                        if ((prospectSetupMODEL.ProspectFromQuot == "Y" && prospectSetupMODEL.QuotationDocumentMenuId == "105103120")|| (prospectSetupMODEL.ProspectFromQuot == "Y" && prospectSetupMODEL.QuotationDocumentMenuId == "105103145105"))
                        {
                                //Session.Remove("ProspectFromQuot");
                            _DomesticSalesQuotationModel = new DomesticSalesQuotationModel();
                            ViewBag.MenuPageName = getDocumentName();
                            _DomesticSalesQuotationModel.Title = title;
                            //Session["ILSearch"] = "0";
                            //Session["Message"] = "New";
                            //Session["DocumentStatus"] = "D";
                            //Session["BtnName"] = "BtnAddNew";
                            //Session["TransType"] = "Save";
                            //Session["Command"] = "New";
                            
                            prospectSetupMODEL.ILSearch = "0";
                            prospectSetupMODEL.Message = "New";
                            prospectSetupMODEL.DocumentStatus = "D";
                            prospectSetupMODEL.BtnName = "BtnAddNew";
                            prospectSetupMODEL.TransType = "Save";
                            prospectSetupMODEL.Command = "New";
                            prospectSetupMODEL.ProspectFromQuot = null;
                            var Flag = prospectSetupMODEL.QuotationDocumentMenuId;                        
                            return RedirectToAction("DomesticSalesQuotationDetail", "DomesticSalesQuotation", new { Area = "ApplicationLayer", Flag = Flag });
                            }
                        //if (Session["ProspectFromRFQ"].ToString() == "Y")
                        if (prospectSetupMODEL.ProspectFromRFQ == "Y")
                        {
                            //Session.Remove("ProspectFromRFQ");
                            ViewBag.MenuPageName = getDocumentName();                          
                            //Session.Remove("Message");
                            //Session.Remove("Command");
                            //Session["TransType"] = "Save";
                            //Session["ILSearch"] = "0";
                            prospectSetupMODEL.TransType = "Save";
                            prospectSetupMODEL.ILSearch = "0";
                            prospectSetupMODEL.ProspectFromRFQ = null;
                            //Session.Remove("DocumentStatus");
                            return RedirectToAction("AddRequestForQuotationDetail", "RequestForQuotation", new { Area = "ApplicationLayer" });
                        }
                        if (prospectSetupMODEL.ProspectFromPQ == "Y")
                        {
                            //Session.Remove("ProspectFromPQ");
                            ViewBag.MenuPageName = getDocumentName();
                            //Session["PQSearch"] = "0";
                            prospectSetupMODEL.PQSearch = "0";
                            //Session.Remove("Message");
                            //Session.Remove("Command");
                            //Session["TransType"] = "Save";
                            var ProspectFromPQ = prospectSetupMODEL.ProspectFromPQ;
                            prospectSetupMODEL.ProspectFromPQ = null;
                            prospectSetupMODEL.TransType = "Save";
                            //Session.Remove("DocumentStatus");
                            return RedirectToAction("AddPurchaseQuotationDetail", "PurchaseQuotation", new { Area = "ApplicationLayer", ProspectFromPQ });
                        }
                        //if (Session["ProspectFromPQ"].ToString() == "Y")
                        if (prospectSetupMODEL.ProspectFromQuot == "Y" && prospectSetupMODEL.QuotationDocumentMenuId == "105103117") /**Add By HINA SHARMA 17-01-2025 for SALES ENQUIRY**/
                        {
                            SalesEnquiryModel _SEModel = new SalesEnquiryModel();
                            ViewBag.MenuPageName = getDocumentName();
                            _SEModel.Title = "SalesEnquiry";
                            prospectSetupMODEL.ILSearch = "0";
                            prospectSetupMODEL.Message = "New";
                            prospectSetupMODEL.DocumentStatus = "D";
                            prospectSetupMODEL.BtnName = "BtnAddNew";
                            prospectSetupMODEL.TransType = "Save";
                            prospectSetupMODEL.Command = "New";
                            prospectSetupMODEL.ProspectFromQuot = null;
                            var ProsFlag = "Y";
                            var ProsEnq = prospectSetupMODEL.pros_type;
                            return RedirectToAction("SalesEnquiryDetail", "SalesEnquiry", new { Area = "ApplicationLayer", ProsFlag, ProsEnq });
                        }

                        else
                        {
                            //Session.Remove("Message");
                            //Session.Remove("TransType");
                            //Session.Remove("Command");
                            //Session.Remove("BtnName");
                            //Session.Remove("DocumentStatus");
                            TempData["ProspectDetail"] = null;
                            return RedirectToAction("ProspectSetup");
                        }

                    default:
                        return new EmptyResult();

                }
             
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult InsertProspectDetail(ProspectSetupMODEL prospectSetupMODEL )
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            try
            {

                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable ProspectDetail = new DataTable();
                DataTable dt = new DataTable();
                DataTable Attachments = new DataTable();

                dt.Columns.Add("TransType", typeof(string));
                dt.Columns.Add("comp_id", typeof(Int32));
                dt.Columns.Add("br_id", typeof(Int32));
                dt.Columns.Add("pros_id", typeof(string));
                dt.Columns.Add("entity_type", typeof(string));
                dt.Columns.Add("pros_type", typeof(string));
                dt.Columns.Add("pros_name", typeof(string));
                dt.Columns.Add("curr_id", typeof(string));
                dt.Columns.Add("cont_num", typeof(string));
                dt.Columns.Add("cont_email", typeof(string));
                dt.Columns.Add("cont_person", typeof(string));
                dt.Columns.Add("address", typeof(string));
                dt.Columns.Add("pin", typeof(string));
                dt.Columns.Add("city", typeof(string));
                dt.Columns.Add("dist", typeof(string));
                dt.Columns.Add("state", typeof(string));
                dt.Columns.Add("cntry", typeof(string));
                dt.Columns.Add("gst_no", typeof(string));
                dt.Columns.Add("pros_status", typeof(string));
                dt.Columns.Add("user_id", typeof(string));
                dt.Columns.Add("mac_id", typeof(string));
                dt.Columns.Add("remarks", typeof(string));
                dt.Columns.Add("gst_cat", typeof(string));


                DataRow dtrow = dt.NewRow();
                //if (Session["TransType"].ToString() == "Duplicate")
                if (prospectSetupMODEL.TransType == "Duplicate")
                {
                    if (prospectSetupMODEL.pros_id == null)
                    {
                        //Session["TransType"] = "Save";
                        prospectSetupMODEL.TransType = "Save";
                    }
                    else
                    {
                        //Session["TransType"] = "Update";
                        prospectSetupMODEL.TransType = "Update";
                    }
                }
                if (prospectSetupMODEL.TransType != null)
                {
                    if (prospectSetupMODEL.pros_id != null)
                    {
                        prospectSetupMODEL.TransType = "Update";
                    }
                }
                //dtrow["TransType"] = Session["TransType"].ToString();
                dtrow["TransType"] = prospectSetupMODEL.TransType;
                dtrow["comp_id"] = CompID;
                dtrow["br_id"] = prospectSetupMODEL.br; 
                dtrow["pros_id"] = prospectSetupMODEL.pros_id;
                dtrow["entity_type"] = prospectSetupMODEL.Entity_type;
                dtrow["pros_type"] = prospectSetupMODEL.pros_type;
                dtrow["pros_name"] = prospectSetupMODEL.ProspectName;
                dtrow["curr_id"] = prospectSetupMODEL.Currency;
                dtrow["cont_num"] = prospectSetupMODEL.ContactNumber;
                dtrow["cont_email"] = prospectSetupMODEL.Email;
                dtrow["cont_person"] = prospectSetupMODEL.ContactPerson;
                dtrow["address"] = prospectSetupMODEL.Address;
                dtrow["pin"] = prospectSetupMODEL.Pin;
                dtrow["city"] = prospectSetupMODEL.City;
                dtrow["dist"] = prospectSetupMODEL.District;
                dtrow["state"] = prospectSetupMODEL.State;
                dtrow["cntry"] = prospectSetupMODEL.Country;
                dtrow["gst_no"] = prospectSetupMODEL.GSTNumber;
                dtrow["pros_status"] = "N";//prospectSetupMODEL.proc_status;
                dtrow["user_id"] = UserID;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrow["mac_id"] = mac_id;
                dtrow["remarks"] = prospectSetupMODEL.remarks;
                dtrow["gst_cat"] = prospectSetupMODEL.Gst_Cat;

                dt.Rows.Add(dtrow);
                ProspectDetail = dt;
                var ProspectSetupMODELAttc = TempData["ModelDataattch"] as ProspectSetupMODELAttch;

                //if (ProspectSetupMODELAttc.Guid == prospectSetupMODEL.pros_id)
                //{
                //    if (prospectSetupMODEL.TransType == "Update")
                //    {
                //        ProspectSetupMODELAttc = null;
                //    }
                //}
                DataTable dtAttachment = new DataTable();
                if (prospectSetupMODEL.attatchmentdetail != null)
                {
                   
                    if (ProspectSetupMODELAttc != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (ProspectSetupMODELAttc.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = ProspectSetupMODELAttc.AttachMentDetailItmStp as DataTable;
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
                        if (prospectSetupMODEL.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = prospectSetupMODEL.AttachMentDetailItmStp as DataTable;
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
                       
                    JArray jObject1 = JArray.Parse(prospectSetupMODEL.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(prospectSetupMODEL.pros_id))
                            {
                                dtrowAttachment1["id"] = prospectSetupMODEL.pros_id;
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
                    if (prospectSetupMODEL.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(prospectSetupMODEL.pros_id))
                            {
                                ItmCode = prospectSetupMODEL.pros_id;
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
                    Attachments = dtAttachment;
                }
                TempData["ModelDataattch"] = null;
                SaveMessage = prospectSetup_ISERVICES.insertProspectDetails(ProspectDetail, Attachments);
                string[] Data = SaveMessage.Split('-');
                string ProsCode = Data[1];
                string Message = Data[0];
                //Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "Save")
                {
                    string Guid = "";
                    if (ProspectSetupMODELAttc != null)
                    {
                        //if (Session["Guid"] != null)
                        if (ProspectSetupMODELAttc.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = ProspectSetupMODELAttc.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    comCont.ResetImageLocation(CompID, "00", guid, PageName, ProsCode, prospectSetupMODEL.TransType, Attachments);

                    //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                    //if (Directory.Exists(sourcePath))
                    //{
                    //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Guid + "_" + "*");
                    //    foreach (string file in filePaths)
                    //    {
                    //        string[] items = file.Split('\\');
                    //        string ItemName = items[items.Length - 1];
                    //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                    //        foreach (DataRow dr in Attachments.Rows)
                    //        {
                    //            string DrItmNm = dr["file_name"].ToString();
                    //            if (ItemName == DrItmNm)
                    //            {
                    //                string img_nm = CompID + ProsCode + "_" + Path.GetFileName(DrItmNm).ToString();
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
                string br_id = Data[2];

                if (Message == "Update" || Message == "Save")
                {
                    //Session["Message"] = "Save";
                    //Session["ProsCode"] = ProsCode;
                    //Session["TransType"] = "Update";
                    //Session["BtnName"] = "BtnSave";
                    //Session["Br_id"] = br_id;
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;

                    prospectSetupMODEL.Message = "Save";
                    prospectSetupMODEL.ProsCode = ProsCode;
                    prospectSetupMODEL.TransType = "Update";
                    prospectSetupMODEL.BtnName = "BtnSave";
                    Session["Br_id"] = br_id;
                    prospectSetupMODEL.AttachMentDetailItmStp = null;
                    prospectSetupMODEL.Guid = null;

                }
                if (Message == "Duplicate")
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    //Session["TransType"] = "Duplicate";
                    //Session["Message"] = "Duplicate";
                    //Session["ProsCode"] = ProsCode;
                    ////Session["BtnName"] = "BtnAddNew";
                    //Session["Command"] = "Edit";

                    prospectSetupMODEL.AttachMentDetailItmStp = null;
                    prospectSetupMODEL.Guid = null;
                    prospectSetupMODEL.TransType = "Duplicate";
                    prospectSetupMODEL.Message = "Duplicate";
                    prospectSetupMODEL.ProsCode = ProsCode;
                    //prospectSetupMODEL.BtnName"] = "BtnAddNew";
                    prospectSetupMODEL.Command = "Edit";
                    TempData["ModelData"] = prospectSetupMODEL;
                    return RedirectToAction("ProspectDetailInDuplicateCase");
                }
                if (Message == "DuplicateGst_No")
                {
                    prospectSetupMODEL.AttachMentDetailItmStp = null;
                    prospectSetupMODEL.Guid = null;
                    prospectSetupMODEL.TransType = "Duplicate";
                    prospectSetupMODEL.Message = "DuplicateGst_No";
                    prospectSetupMODEL.ProsCode = ProsCode;
                    //prospectSetupMODEL.BtnName"] = "BtnAddNew";
                    prospectSetupMODEL.Command = "Edit";
                    TempData["ModelData"] = prospectSetupMODEL;
                    return RedirectToAction("ProspectDetailInDuplicateCase");
                }
                TempData["ModelData"] = prospectSetupMODEL;
                return RedirectToAction("AddProspectSetupDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (prospectSetupMODEL.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (prospectSetupMODEL.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = prospectSetupMODEL.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID, PageName, Guid, Server);
                    }
                }
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        //public ActionResult ProspectDetailInDuplicateCase(ProspectSetupMODEL prospectSetupMODEL)
        public ActionResult ProspectDetailInDuplicateCase(ProspectSetupMODEL prospectSetupMODEL1)
        {
            try
            {
                var prospectSetupMODEL = TempData["ModelData"] as ProspectSetupMODEL;
                if (prospectSetupMODEL != null)
                {
                    if (Session["compid"] != null)
                    { /*Commented by Hina on 03-01-2024 to change Bind city on behalf of district*/
                        //Dictionary<string, string> CityListD = new Dictionary<string, string>();
                        //CityListD = prospectSetup_ISERVICES.GetCityList("");
                        //List<ProspectCity> prospectCities = new List<ProspectCity>();
                        //if (CityListD.Count > 0)
                        //{
                        //    foreach (var dr in CityListD)
                        //    {
                        //        ProspectCity CityList = new ProspectCity();
                        //        CityList.city_id = dr.Key;
                        //        CityList.city_name = dr.Value;
                        //        prospectCities.Add(CityList);
                        //    }
                        //}

                        //prospectCities.Insert(0, new ProspectCity() { city_id = "0", city_name = "---Select---" });
                        //prospectSetupMODEL.ProspectCities = prospectCities;
                        /*----------------------Code start of Country,state,district,city--------------------------*/
                        List<CmnCountryList> _TransList = new List<CmnCountryList>();
                        _TransList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                        List<Country> _TransList2 = new List<Country>();
                        CommonAddress_Detail _Model = new CommonAddress_Detail();
                        string transportType = "D";
                        if (!string.IsNullOrEmpty(prospectSetupMODEL.pros_type))
                            transportType = prospectSetupMODEL.pros_type;
                        DataTable dtcntry = GetCountryList(transportType);

                        foreach (DataRow dr in dtcntry.Rows)
                        {
                            CmnCountryList _Custcurr = new CmnCountryList();
                            _Custcurr.country_id = dr["country_id"].ToString();
                            _Custcurr.country_name = dr["country_name"].ToString();
                            _TransList.Add(_Custcurr);
                            _TransList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                        }
                        _Model.countryList = _TransList2;

                        prospectSetupMODEL.countryList = _TransList;
                        List<CmnStateList> state = new List<CmnStateList>();
                        state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                        string transCountry = "";
                        if (!string.IsNullOrEmpty(prospectSetupMODEL.Country))
                            transCountry = prospectSetupMODEL.Country;
                        else
                            transCountry = dtcntry.Rows[0]["country_id"].ToString();

                        DataTable dtStates = prospectSetup_ISERVICES.GetstateOnCountryDDL(transCountry);
                        if (dtStates.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtStates.Rows)
                            {
                                state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                            }
                        }
                        prospectSetupMODEL.StateList = state;

                        string transState = "0";
                        List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                        DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                        if (!string.IsNullOrEmpty(prospectSetupMODEL.State))
                            transState = prospectSetupMODEL.State;
                        else
                            transState = "0";
                        DataTable dtDist = prospectSetup_ISERVICES.GetDistrictOnStateDDL(transState);
                        if (dtDist.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtDist.Rows)
                            {
                                DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                            }
                        }
                        prospectSetupMODEL.DistrictList = DistList;

                        string transDist = "0";
                        if (!string.IsNullOrEmpty(prospectSetupMODEL.District))
                            transDist = prospectSetupMODEL.District;
                        else
                            transDist = "0";
                        DataTable dtCities = prospectSetup_ISERVICES.GetCityOnDistrictDDL(transDist);

                        List<CmnCityList> cities = new List<CmnCityList>();
                        cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                        if (dtCities.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtCities.Rows)
                            {
                                cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                            }
                        }
                        prospectSetupMODEL.cityLists = cities;

                        prospectSetupMODEL._CommonAddress_Detail = _Model;
                        /*----------------------Code End of Country,state,district,city--------------------------*/
                        List<curr> _CustcurrList = new List<curr>();
                        string curr = prospectSetupMODEL.pros_type;

                        if (curr != null)
                        {
                            dt = GetProspectCurrency(curr);
                        }
                        else
                        {
                            dt = GetProspectCurrency("D");
                        }
                        CommonPageDetails();
                        foreach (DataRow dr in dt.Rows)
                        {
                            curr _Custcurr = new curr();
                            _Custcurr.curr_id = dr["curr_id"].ToString();
                            _Custcurr.curr_name = dr["curr_name"].ToString();
                            _CustcurrList.Add(_Custcurr);

                        }
                        prospectSetupMODEL.currList = _CustcurrList;

                        List<Branch> _BranchList = new List<Branch>();
                        dt = GetBranchList();
                        foreach (DataRow dr in dt.Rows)
                        {
                            Branch _Branch = new Branch();
                            _Branch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                            _Branch.br_val = dr["comp_nm"].ToString();
                            _BranchList.Add(_Branch);
                        }
                        prospectSetupMODEL.BranchList = _BranchList;


                        ViewBag.MenuPageName = getDocumentName();
                        prospectSetupMODEL.Title = title;
                        //if (Session["BtnName"] == null)
                        if (prospectSetupMODEL.BtnName == null)
                        {
                            //if (Session["BtnName"].ToString() == "")
                            //if (prospectSetupMODEL.BtnName == "")
                            //{
                            //Session["TransType"] = "Refresh";
                            //Session["BtnName"] = "BtnAddNew";
                            //Session["Command"] = "Add";

                            prospectSetupMODEL.TransType = "Save";
                            prospectSetupMODEL.BtnName = "BtnAddNew";
                            prospectSetupMODEL.Command = "Add";
                            //}
                        }
                        Int32 comp_id = Convert.ToInt32(Session["compid"].ToString());
                        Int32 br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        Int32 pros_id = 0;

                        //if (Session["ProsCode"] != null)
                        if (prospectSetupMODEL.ProsCode != null)
                        {
                            //pros_id = Convert.ToInt32(Session["ProsCode"].ToString());
                            pros_id = Convert.ToInt32(prospectSetupMODEL.ProsCode);
                        }
                    }
                    prospectSetupMODEL.DocumentMenuId = DocumentMenuId;
                    return View("~/Areas/BusinessLayer/Views/ProspectSetup/ProspectSetupDetail.cshtml", prospectSetupMODEL);
                }
                else
                {
                    if (Session["compid"] != null)
                    {
                        //Dictionary<string, string> CityListD = new Dictionary<string, string>();
                        //CityListD = prospectSetup_ISERVICES.GetCityList("");
                        //List<ProspectCity> prospectCities = new List<ProspectCity>();
                        //if (CityListD.Count > 0)
                        //{
                        //    foreach (var dr in CityListD)
                        //    {
                        //        ProspectCity CityList = new ProspectCity();
                        //        CityList.city_id = dr.Key;
                        //        CityList.city_name = dr.Value;
                        //        prospectCities.Add(CityList);
                        //    }
                        //}

                        //prospectCities.Insert(0, new ProspectCity() { city_id = "0", city_name = "---Select---" });
                        //prospectSetupMODEL1.ProspectCities = prospectCities;
                        /*----------------------Code start of Country,state,district,city--------------------------*/
                        List<CmnCountryList> _TransList = new List<CmnCountryList>();
                        _TransList.Add(new CmnCountryList { country_id = "0", country_name = "---Select---" });
                        List<Country> _TransList2 = new List<Country>();
                        CommonAddress_Detail _Model = new CommonAddress_Detail();
                        string transportType = "D";
                        if (!string.IsNullOrEmpty(prospectSetupMODEL1.pros_type))
                            transportType = prospectSetupMODEL1.pros_type;
                        DataTable dtcntry = GetCountryList(transportType);

                        foreach (DataRow dr in dtcntry.Rows)
                        {
                            CmnCountryList _Custcurr = new CmnCountryList();
                            _Custcurr.country_id = dr["country_id"].ToString();
                            _Custcurr.country_name = dr["country_name"].ToString();
                            _TransList.Add(_Custcurr);
                            _TransList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                        }
                        _Model.countryList = _TransList2;

                        prospectSetupMODEL1.countryList = _TransList;
                        List<CmnStateList> state = new List<CmnStateList>();
                        state.Add(new CmnStateList { state_id = "0", state_name = "---Select---" });
                        string transCountry = "";
                        if (!string.IsNullOrEmpty(prospectSetupMODEL1.Country))
                            transCountry = prospectSetupMODEL1.Country;
                        else
                            transCountry = dtcntry.Rows[0]["country_id"].ToString();

                        DataTable dtStates = prospectSetup_ISERVICES.GetstateOnCountryDDL(transCountry);
                        if (dtStates.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtStates.Rows)
                            {
                                state.Add(new CmnStateList { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                            }
                        }
                        prospectSetupMODEL1.StateList = state;

                        string transState = "0";
                        List<CmnDistrictList> DistList = new List<CmnDistrictList>();
                        DistList.Add(new CmnDistrictList { district_id = "0", district_name = "---Select---" });
                        if (!string.IsNullOrEmpty(prospectSetupMODEL1.State))
                            transState = prospectSetupMODEL1.State;
                        else
                            transState = "0";
                        DataTable dtDist = prospectSetup_ISERVICES.GetDistrictOnStateDDL(transState);
                        if (dtDist.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtDist.Rows)
                            {
                                DistList.Add(new CmnDistrictList { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                            }
                        }
                        prospectSetupMODEL1.DistrictList = DistList;

                        string transDist = "0";
                        if (!string.IsNullOrEmpty(prospectSetupMODEL1.District))
                            transDist = prospectSetupMODEL1.District;
                        else
                            transDist = "0";
                        DataTable dtCities = prospectSetup_ISERVICES.GetCityOnDistrictDDL(transDist);

                        List<CmnCityList> cities = new List<CmnCityList>();
                        cities.Add(new CmnCityList { City_Id = "0", City_Name = "---Select---" });
                        if (dtCities.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtCities.Rows)
                            {
                                cities.Add(new CmnCityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                            }
                        }
                        prospectSetupMODEL1.cityLists = cities;

                        prospectSetupMODEL1._CommonAddress_Detail = _Model;
                       /*----------------------Code End of Country,state,district,city--------------------------*/
                        List<curr> _CustcurrList = new List<curr>();
                        string curr = prospectSetupMODEL1.pros_type;

                        if (curr != null)
                        {
                            dt = GetProspectCurrency(curr);
                        }
                        else
                        {
                            dt = GetProspectCurrency("D");
                        }
                        CommonPageDetails();
                        foreach (DataRow dr in dt.Rows)
                        {
                            curr _Custcurr = new curr();
                            _Custcurr.curr_id = dr["curr_id"].ToString();
                            _Custcurr.curr_name = dr["curr_name"].ToString();
                            _CustcurrList.Add(_Custcurr);

                        }
                        prospectSetupMODEL1.currList = _CustcurrList;

                        List<Branch> _BranchList = new List<Branch>();
                        dt = GetBranchList();
                        foreach (DataRow dr in dt.Rows)
                        {
                            Branch _Branch = new Branch();
                            _Branch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                            _Branch.br_val = dr["comp_nm"].ToString();
                            _BranchList.Add(_Branch);
                        }
                        prospectSetupMODEL1.BranchList = _BranchList;


                        ViewBag.MenuPageName = getDocumentName();
                        prospectSetupMODEL1.Title = title;
                        //if (Session["BtnName"] == null)
                        if (prospectSetupMODEL1.BtnName == null)
                        {
                            //if (Session["BtnName"].ToString() == "")
                            //if (prospectSetupMODEL1.BtnName == "")
                            //{
                            //Session["TransType"] = "Refresh";
                            //Session["BtnName"] = "BtnAddNew";
                            //Session["Command"] = "Add";

                            prospectSetupMODEL1.TransType = "Save";
                            prospectSetupMODEL1.BtnName = "BtnAddNew";
                            prospectSetupMODEL1.Command = "Add";
                            //}
                        }
                        Int32 comp_id = Convert.ToInt32(Session["compid"].ToString());
                        Int32 br_id = Convert.ToInt32(Session["BranchId"].ToString());
                        Int32 pros_id = 0;

                        //if (Session["ProsCode"] != null)
                        if (prospectSetupMODEL1.ProsCode != null)
                        {
                            //pros_id = Convert.ToInt32(Session["ProsCode"].ToString());
                            pros_id = Convert.ToInt32(prospectSetupMODEL1.ProsCode);
                        }
                    }
                    prospectSetupMODEL1.DocumentMenuId = DocumentMenuId;
                    return View("~/Areas/BusinessLayer/Views/ProspectSetup/ProspectSetupDetail.cshtml", prospectSetupMODEL1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ProspectDetailDelete(ProspectSetupMODEL prospectSetupMODEL,string command)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                string dt = prospectSetup_ISERVICES.DeleteProsId(prospectSetupMODEL, command,Comp_ID, Br_ID);


                //Session["Message"] = dt;
                prospectSetupMODEL.Message = dt;
                if (dt == "Deleted")
                {
                    if (!string.IsNullOrEmpty(prospectSetupMODEL.pros_id))
                    {
                        getDocumentName(); /* To set Title*/
                        string PageName = title.Replace(" ", "");
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(Comp_ID, PageName, prospectSetupMODEL.pros_id, Server);
                    }

                    //Session["Command"] = "Delete";
                    //Session["ProsCode"] = "";
                    //Session["TransType"] = "Refresh";
                    //Session["BtnName"] = "Delete";
                    //prospectSetupMODEL = null;
                    prospectSetupMODEL.Command = "Delete";
                    prospectSetupMODEL.ProsCode = "";
                    prospectSetupMODEL.TransType = "Refresh";
                    prospectSetupMODEL.BtnName = "Delete";
                }
                else
                {
                    //Session["TransType"] = "Update";
                    //Session["BtnName"] = "BtnSave";

                    prospectSetupMODEL.TransType = "Update";
                    prospectSetupMODEL.BtnName = "BtnSave";
                }
                TempData["ModelData"] = prospectSetupMODEL;
                return RedirectToAction("AddProspectSetupDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        /*----------------------Code start of Country,state,district,city--------------------------*/
        [NonAction]
        private DataTable GetCountryList(string ProspectType)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = prospectSetup_ISERVICES.GetCountryListDDL(Comp_ID,ProspectType);
                
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
        public JsonResult GetCountryonChngPros(string ProspectType)
        {
            JsonResult DataRows = null;
            try
            {
                List<CmnCountryList> _TransList = new List<CmnCountryList>();
                _ProspectSetupMODEL = new ProspectSetupMODEL();
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = prospectSetup_ISERVICES.GetCountryListDDL(Comp_ID, ProspectType);
                if (ProspectType == "E")
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
                DataTable dt = prospectSetup_ISERVICES.GetstateOnCountryDDL(ddlCountryID);
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
                DataTable dt = prospectSetup_ISERVICES.GetDistrictOnStateDDL(ddlStateID);
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
                DataTable dt = prospectSetup_ISERVICES.GetCityOnDistrictDDL(ddlDistrictID);
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
                DataSet ds = prospectSetup_ISERVICES.GetStateCode(stateId);
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
        private DataTable GetProspectCurrency(string prosType)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = prospectSetup_ISERVICES.GetCurronProspectTypeDAL(Comp_ID, prosType);
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
        public JsonResult GetCurronProspType(string prosType)
        {
            JsonResult DataRows = null;
            try
            {
                List<curr> _CustcurrList = new List<curr>();
                ProspectSetupMODEL prospectSetupMODEL = new ProspectSetupMODEL();
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = prospectSetup_ISERVICES.GetCurronProspectTypeDAL(Comp_ID, prosType);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/

                //foreach (DataRow dr in dt.Rows)
                //{
                //    curr _Custcurr = new curr();
                //    _Custcurr.curr_id = dr["curr_id"].ToString();
                //    _Custcurr.curr_name = dr["curr_name"].ToString();
                //    _CustcurrList.Add(_Custcurr);

                //}
                //if (prosType == "O")
                //{
                //    _CustcurrList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                //    prospectSetupMODEL.currList = _CustcurrList;
                //}
                //else
                //{
                //    prospectSetupMODEL.currList = _CustcurrList;
                //}


                //return PartialView("~/Areas/BusinessLayer/Views/Shared/PartialCurrencyProspect.cshtml", prospectSetupMODEL);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult GetAutoCompleteCity(ProspectSetupMODEL prospectSetupMODEL)
        {
            string GroupName = string.Empty;
           
            Dictionary<string, string> CityList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(prospectSetupMODEL.ddlGroup))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = prospectSetupMODEL.ddlGroup;
                }
                CityList = prospectSetup_ISERVICES.GetCityList(GroupName);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Json(CityList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetBranchList()
        {
            string CompID = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            DataTable dt = prospectSetup_ISERVICES.BranchList(CompID);
            return dt;
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                ProspectSetupMODELAttch ProspectSetupMODELAttc = new ProspectSetupMODELAttch();
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
                ProspectSetupMODELAttc.Guid = DocNo;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    ProspectSetupMODELAttc.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    ProspectSetupMODELAttc.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = ProspectSetupMODELAttc;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        //public JsonResult Upload()
        //{

        //    try
        //    {
        //        var other = new CommonController(_Common_IServices);
        //        HttpFileCollectionBase Files = Request.Files;

        //        string TransType = "";
        //        string ProsCode = "";
        //        Guid gid = new Guid();
        //        gid = Guid.NewGuid();
        //        if (Session["TransType"] != null)
        //        {
        //            TransType = Session["TransType"].ToString();
        //        }
        //        if (Session["ProsCode"] != null)
        //        {
        //            ProsCode = Session["ProsCode"].ToString();
        //        }
        //        if (TransType == "Save")
        //        {
        //            ProsCode = gid.ToString();
        //        }
        //        ProsCode = ProsCode.Replace("/", "");
        //        Session["Guid"] = ProsCode;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        getDocumentName(); /* To set Title*/
        //        string PageName = title.Replace(" ", "");

        //        DataTable dt = other.Upload(PageName, TransType, CompID, ProsCode, Files, Server);
        //        if (dt.Rows.Count > 0)
        //        {
        //            Session["AttachMentDetailItmStp"] = dt;
        //        }
        //        else
        //        {
        //            Session["AttachMentDetailItmStp"] = null;
        //        }

        //        return Json("Uploaded " + Request.Files.Count + " files");
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("Error");
        //    }

        //}
    }

}