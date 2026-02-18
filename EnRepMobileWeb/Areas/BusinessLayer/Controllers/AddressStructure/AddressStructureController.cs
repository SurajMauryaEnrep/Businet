using EnRepMobileWeb.MODELS.BusinessLayer.AddressStructure;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AddressStructure;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 //***All Session RemoveBy Shubham Maurya On 01-01-2023 ***//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers
{
    public class AddressStructureController : Controller
    {
        
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string CompID, branchID, user_id, language = String.Empty;
        string DocumentMenuId = "103190",title;
        AddressStructureModel _AddressStructureModel;
        AddressStructure_ISERVICES _addressStructure_ISERVICES;
        Common_IServices _Common_IServices;

        public AddressStructureController(Common_IServices _Common_IServices, AddressStructure_ISERVICES _addressStructure_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._addressStructure_ISERVICES = _addressStructure_ISERVICES;
        }
        // GET: BusinessLayer/AddressStructure 
        public ActionResult AddressStructure(AddressStructureModel _AddressStructureModel1)
        {
            try
            {
                CommonPageDetails();
                DataSet ds = _addressStructure_ISERVICES.getAddressStructureLists();
                ViewBag.StateList = ds.Tables[0];
                ViewBag.DistrictList = ds.Tables[1];
              //  ViewBag.CityAndPinList = ds.Tables[2];
                var _AddressStructureModel = TempData["ModelData"] as AddressStructureModel;
                if (_AddressStructureModel != null)
                {
                    AddressStructureModel _AddressStructureM = new AddressStructureModel();
                    //ViewBag.MenuPageName = getDocumentName();
                    _AddressStructureM.Title = title;
                    if(_AddressStructureModel.Message== "exist")
                    {
                        _AddressStructureM.hdnAction = _AddressStructureModel.hdnAction;
                    }
                    else
                    {
                        _AddressStructureM.hdnAction = "SaveState";
                    }
                   if(_AddressStructureModel.Message== "existAAddr")
                    {
                        _AddressStructureM.DhdnAction = _AddressStructureModel.DhdnAction;
                    }
                    else
                    {
                        _AddressStructureM.DhdnAction = "SaveDistrict";
                    }       
                    _AddressStructureM.ChdnAction = "SavecityAndpin";
                    string str = "";
                    //if (Session["Message"] != null)
                    if (_AddressStructureModel.Message != null)
                    {
                        //str = Session["Message"].ToString();
                        str = _AddressStructureModel.Message;
                    }

                    if (str == "New")
                    {
                        ViewBag.Message = "New";
                    }
                    //Session["Message"] = "New";
                    _AddressStructureM.Message = "New";
                    if (str == "SaveAddress")
                    {
                        //ViewBag.Message = "Save";
                        _AddressStructureM.Message = "Save";
                    }

                   else if (str == "DeletedAddr")
                    {
                        //ViewBag.Message = "Deleted";
                        _AddressStructureM.Message = "Deleted";
                        _AddressStructureM.Ccountry_id = null;
                        _AddressStructureM.Ccountry_name = null;
                        _AddressStructureM.Cstate_id = null;
                        _AddressStructureM.Cstate_name = null;
                        _AddressStructureM.City_id = null;

                    }
                   else if (str == "existAddr")
                    {
                        //ViewBag.Message = "exist";
                        _AddressStructureM.Message = "exist";
                        _AddressStructureModel.country_name = null;
                        _AddressStructureModel.Dstate_id = null;

                    }
                  else  if (str == "exist")
                    {
                        _AddressStructureM.Message = "DependencyExist";
                        _AddressStructureM.state_id = Convert.ToInt32(_AddressStructureModel.state_id);
                        //_AddressStructureM.country_name = _AddressStructureModel.country;
                        _AddressStructureM.country_name = _AddressStructureModel.country_name;
                        //_AddressStructureM.state_name = _AddressStructureModel.state;
                        _AddressStructureM.state_name = _AddressStructureModel.state_name;
                        _AddressStructureM.state_code = _AddressStructureModel.state_code;
                    }
                   else if (str == "existAAddr")
                    {
                        _AddressStructureM.Message = "DependencyExist";
                        _AddressStructureM.Dstate_name = _AddressStructureModel.state_id.ToString();
                            _AddressStructureM.district_name = _AddressStructureModel.district_name;
                            _AddressStructureM.district_id = _AddressStructureModel.district_id;
                        
                    }
                   else if (str == "DuplicateStateCode")
                    {
                        //ViewBag.Message = "DuplicateStateCode";
                        _AddressStructureM.Message = "DuplicateStateCode";
                        //_AddressStructureModel.state_id = Convert.ToInt32(Session["state_id"].ToString());
                        _AddressStructureM.state_id = Convert.ToInt32(_AddressStructureModel.state_id);
                        //Session["state_id"] = null;
                        //_AddressStructureModel.country_name = Session["country"].ToString();
                        _AddressStructureM.country_name = _AddressStructureModel.country;
                        //Session["country"] = null;
                        //_AddressStructureModel.state_name = Session["state"].ToString();
                        _AddressStructureM.state_name = _AddressStructureModel.state;
                        //Session["state"] = null;
                        //_AddressStructureModel.state_code = Session["state_code"].ToString();
                        _AddressStructureM.state_code = _AddressStructureModel.state_code;
                        //Session["state_code"] = null;
                    }
                   else if (str == "DuplicateState")
                    {
                        _AddressStructureM.Message = "Duplicate";
                        ViewBag.Message = "Duplicate";
                        //ViewBag.Message = "DuplicateStateCode";
                        //_AddressStructureModel.state_id = Convert.ToInt32(Session["state_id"].ToString());
                        _AddressStructureM.state_id = Convert.ToInt32(_AddressStructureModel.state_id);
                        //Session["state_id"] = null;
                        //_AddressStructureModel.country_name = Session["country"].ToString();
                        _AddressStructureM.country_name = _AddressStructureModel.country;
                        //Session["country"] = null;
                        //_AddressStructureModel.state_name = Session["state"].ToString();
                        _AddressStructureM.state_name = _AddressStructureModel.state;
                        //Session["state"] = null;
                        //_AddressStructureModel.state_code = Session["state_code"].ToString();
                        _AddressStructureM.state_code = _AddressStructureModel.state_code;
                        //Session["state_code"] = null;
                    }
                  else  if (str == "DuplicateDistrict")
                    {
                        //ViewBag.Message = "DuplicateDistrict";
                        _AddressStructureM.Message = "DuplicateDistrict";
                        //_AddressStructureModel.DhdnAction = Session["TransType"].ToString();
                        _AddressStructureM.DhdnAction = _AddressStructureModel.TransType;
                        //Session["TransType"] = null;
                        //_AddressStructureModel.Dstate_name = Session["state_id"].ToString();
                        _AddressStructureM.Dstate_name = _AddressStructureModel.state_id.ToString();
                        //Session["state_id"] = null;
                        //_AddressStructureModel.district_name = Session["district_name"].ToString();
                        _AddressStructureM.district_name = _AddressStructureModel.district_name;
                        //Session["district_name"] = null;
                        //_AddressStructureModel.district_id = Session["district_id"].ToString();
                        _AddressStructureM.district_id = _AddressStructureModel.district_id;
                        //Session["district_id"] = null;
                    }
                    else if (str == "Duplicate_C")
                    {
                        //ViewBag.Message = "Duplicate_C";
                        _AddressStructureM.Message = "Duplicate_C";
                        //_AddressStructureModel.ChdnAction = Session["TransType"].ToString();
                        _AddressStructureM.ChdnAction = _AddressStructureModel.TransType;
                        //Session["TransType"] = null;
                        //_AddressStructureModel.City_name = Session["city_name"].ToString();
                        _AddressStructureM.City_name = _AddressStructureModel.City_name;
                        //Session["city_name"] = null;
                        // _AddressStructureModel.pin = Session["pin"].ToString();
                        // Session["pin"] = null;
                        //_AddressStructureModel.City_id = Session["city_id"].ToString();
                        _AddressStructureM.City_id = _AddressStructureModel.City_id;
                        //Session["city_id"] = null;
                        //_AddressStructureModel.Cdistrict_name = Session["district_id"].ToString();
                        _AddressStructureM.Cdistrict_name = _AddressStructureModel.district_id;
                        //Session["district_id"] = null;
                        _AddressStructureM.Ccountry_id = _AddressStructureModel.Ccountry_id;
                        _AddressStructureM.Cstate_id = _AddressStructureModel.Cstate_id;
                        _AddressStructureM.Cdistrict_name = _AddressStructureModel.Cdistrict_name;
                    }

                    //countrylist(_AddressStructureModel); /*Commented By Suraj on 12-12-2022 for Reduce Page load time*/
                    List<country_list> _Lists = new List<country_list>();
                    _Lists.Insert(0, new country_list() { country_id = 0, country_name = "---Select---" });
                    _AddressStructureM.country_List = _Lists;
                    //ddlstatelist(_AddressStructureModel); /*Commented By Suraj on 12-12-2022 for Reduce Page load time*/
                    List<state_list> _Lists1 = new List<state_list>();
                    _Lists1.Insert(0, new state_list() { dist_state_id = 0, dist_state_name = "---Select---" });
                    _AddressStructureM.state_Lists = _Lists1;
                    List<district_list> _Lists2 = new List<district_list>();
                    _Lists2.Insert(0, new district_list() { dist_id = 0, dist_name = "---Select---" });
                    _AddressStructureM.district_Lists = _Lists2;
                    _AddressStructureM.Collapse = _AddressStructureModel.Collapse;

                    //ddldistrictlist(_AddressStructureModel); /*Commented By Suraj on 12-12-2022 for Reduce Page load time*/
                   
                    //stateList(); /*Commented By Suraj on 12-12-2022 for Reduce Page load time*/
                    //districtList();
                    //CityAndPintList();
                    if (_AddressStructureModel.country_name != null && _AddressStructureModel.country_name != "0")
                    {
                        _Lists.Insert(1, new country_list() { country_id = Convert.ToInt32(_AddressStructureModel.country_name), country_name = _AddressStructureModel.country_id });
                        _AddressStructureM.country_List = _Lists;
                        _AddressStructureM.country_id = _AddressStructureModel.country_id;
                        _AddressStructureM.country_name = _AddressStructureModel.country_name;
                    }
                    if (_AddressStructureModel.Dstate_name != null)
                    {
                        _Lists1.Insert(0, new state_list() { dist_state_id = Convert.ToInt32(_AddressStructureModel.Dstate_name), dist_state_name = _AddressStructureModel.Dstate_id });
                        _AddressStructureM.state_Lists = _Lists1;
                        _AddressStructureM.Dstate_id = _AddressStructureModel.Dstate_id;
                        _AddressStructureM.Dstate_name = _AddressStructureModel.Dstate_name;
                        _AddressStructureM.Dcountry_name = _AddressStructureModel.Dcountry_name;
                        _AddressStructureM.country_name = null;
                    }
                    if (_AddressStructureModel.City_name != null)
                    {
                        _Lists2.Insert(0, new district_list() { dist_id = Convert.ToInt32(_AddressStructureModel.Cdistrict_name), dist_name = _AddressStructureModel.Cdistrict_id });
                        _AddressStructureM.district_Lists = _Lists2;
                        _AddressStructureM.Cdistrict_name = _AddressStructureModel.Cdistrict_name;
                        _AddressStructureM.Cdistrict_id = _AddressStructureModel.Cdistrict_id;
                        _AddressStructureM.Ccountry_name = _AddressStructureModel.Ccountry_name;
                        _AddressStructureM.Cstate_name = _AddressStructureModel.Cstate_name;
                    }
                    return View("~/Areas/BusinessLayer/Views/AddressStructure/AddressStructure.cshtml", _AddressStructureM);
                }
                else
                {
                    //ViewBag.MenuPageName = getDocumentName();
                    _AddressStructureModel1.Title = title;
                    _AddressStructureModel1.hdnAction = "SaveState";
                    _AddressStructureModel1.DhdnAction = "SaveDistrict";
                    _AddressStructureModel1.ChdnAction = "SavecityAndpin";
                    string str = "";
                    //if (Session["Message"] != null)
                    if (_AddressStructureModel1.Message != null)
                    {
                        //str = Session["Message"].ToString();
                        str = _AddressStructureModel1.Message;
                    }

                    if (str == "New")
                    {
                        ViewBag.Message = "New";
                    }
                    //Session["Message"] = "New";
                    _AddressStructureModel1.Message = "New";
                    if (str == "SaveAddress")
                    {
                        //ViewBag.Message = "Save";
                        _AddressStructureModel1.Message = "Save";
                    }

                   else if (str == "DeletedAddr")
                    {
                        //ViewBag.Message = "Deleted";
                        _AddressStructureModel1.Message = "Deleted";
                    }
                   else if (str == "existAddr")
                    {
                        //ViewBag.Message = "exist";
                        _AddressStructureModel1.Message = "exist";

                    }
                  else  if (str == "DuplicateStateCode")
                    {
                        ViewBag.Message = "DuplicateStateCode";
                        //_AddressStructureModel1.state_id = Convert.ToInt32(Session["state_id"].ToString());
                        _AddressStructureModel1.state_id = Convert.ToInt32(_AddressStructureModel1.state_id);
                        //Session["state_id"] = null;
                        //_AddressStructureModel1.country_name = Session["country"].ToString();
                        _AddressStructureModel1.country_name = _AddressStructureModel1.country;
                        //Session["country"] = null;
                        //_AddressStructureModel1.state_name = Session["state"].ToString();
                        _AddressStructureModel1.state_name = _AddressStructureModel1.state;
                        //Session["state"] = null;
                        //_AddressStructureModel1.state_code = Session["state_code"].ToString();
                        _AddressStructureModel1.state_code = _AddressStructureModel1.state_code;
                        //Session["state_code"] = null;
                    }
                   else if (str == "DuplicateState")
                    {
                        _AddressStructureModel1.Message = "Duplicate";
                        ViewBag.Message = "Duplicate";
                        //ViewBag.Message = "DuplicateStateCode";
                        //_AddressStructureModel1.state_id = Convert.ToInt32(Session["state_id"].ToString());
                        _AddressStructureModel1.state_id = Convert.ToInt32(_AddressStructureModel1.state_id);
                        //Session["state_id"] = null;
                        //_AddressStructureModel1.country_name = Session["country"].ToString();
                        _AddressStructureModel1.country_name = _AddressStructureModel1.country;
                        //Session["country"] = null;
                        //_AddressStructureModel1.state_name = Session["state"].ToString();
                        _AddressStructureModel1.state_name = _AddressStructureModel1.state;
                        //Session["state"] = null;
                        //_AddressStructureModel1.state_code = Session["state_code"].ToString();
                        _AddressStructureModel1.state_code = _AddressStructureModel1.state_code;
                        //Session["state_code"] = null;
                    }
                   else if (str == "DuplicateDistrict")
                    {
                        //ViewBag.Message = "DuplicateDistrict";
                        _AddressStructureModel1.Message = "DuplicateDistrict";
                        //_AddressStructureModel1.DhdnAction = Session["TransType"].ToString();
                        _AddressStructureModel1.DhdnAction = _AddressStructureModel1.TransType;
                        //Session["TransType"] = null;
                        //_AddressStructureModel1.Dstate_name = Session["state_id"].ToString();
                        _AddressStructureModel1.Dstate_name = _AddressStructureModel1.state_id.ToString();
                        //Session["state_id"] = null;
                        //_AddressStructureModel1.district_name = Session["district_name"].ToString();
                        _AddressStructureModel1.district_name = _AddressStructureModel1.district_name;
                        //Session["district_name"] = null;
                        //_AddressStructureModel1.district_id = Session["district_id"].ToString();
                        _AddressStructureModel1.district_id = _AddressStructureModel1.district_id;
                        //Session["district_id"] = null;
                    }
                   else if (str == "Duplicate_C")
                    {
                        //ViewBag.Message = "Duplicate_C";
                        _AddressStructureModel1.Message = "Duplicate_C";
                        //_AddressStructureModel1.ChdnAction = Session["TransType"].ToString();
                        _AddressStructureModel1.ChdnAction = _AddressStructureModel1.TransType;
                        //Session["TransType"] = null;
                        //_AddressStructureModel1.City_name = Session["city_name"].ToString();
                        _AddressStructureModel1.City_name = _AddressStructureModel1.City_name;
                        //Session["city_name"] = null;
                        // _AddressStructureModel1.pin = Session["pin"].ToString();
                        // Session["pin"] = null;
                        //_AddressStructureModel1.City_id = Session["city_id"].ToString();
                        _AddressStructureModel1.City_id = _AddressStructureModel1.City_id;
                        //Session["city_id"] = null;
                        //_AddressStructureModel1.Cdistrict_name = Session["district_id"].ToString();
                        _AddressStructureModel1.Cdistrict_name = _AddressStructureModel1.district_id;
                        //Session["district_id"] = null;
                    }

                    //countrylist(_AddressStructureModel1); /*Commented By Suraj on 12-12-2022 for Reduce Page load time*/
                    List<country_list> _Lists = new List<country_list>();
                    _Lists.Insert(0, new country_list() { country_id = 0, country_name = "---Select---" });
                    _AddressStructureModel1.country_List = _Lists;
                    //ddlstatelist(_AddressStructureModel1); /*Commented By Suraj on 12-12-2022 for Reduce Page load time*/
                    List<state_list> _Lists1 = new List<state_list>();
                    _Lists1.Insert(0, new state_list() { dist_state_id = 0, dist_state_name = "---Select---" });
                    _AddressStructureModel1.state_Lists = _Lists1;
                    List<district_list> _Lists2 = new List<district_list>();
                    _Lists2.Insert(0, new district_list() { dist_id = 0, dist_name = "---Select---" });
                    _AddressStructureModel1.district_Lists = _Lists2;
                    //ddldistrictlist(_AddressStructureModel1); /*Commented By Suraj on 12-12-2022 for Reduce Page load time*/
                 

                    //stateList(); /*Commented By Suraj on 12-12-2022 for Reduce Page load time*/
                    //districtList();
                    //CityAndPintList();
                    return View("~/Areas/BusinessLayer/Views/AddressStructure/AddressStructure.cshtml", _AddressStructureModel1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
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
                    branchID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    user_id = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, branchID, user_id, DocumentMenuId, language);
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
        public void stateList()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = _addressStructure_ISERVICES.getAddressStructureLists().Tables[0];
                ViewBag.StateList = dt;
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                
            }
           
        }
        public void districtList()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = _addressStructure_ISERVICES.getAddressStructureLists().Tables[1];
                ViewBag.DistrictList = dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

            }
        }
        public void CityAndPintList()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = _addressStructure_ISERVICES.getAddressStructureLists().Tables[2];
                ViewBag.CityAndPinList = dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

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
        public ActionResult AddressAction(AddressStructureModel _AddressStructureModel, string Command)
        {

            try
            {
                string str = _AddressStructureModel.hdnAction;
                if (str == "DeleteState")
                {
                    _AddressStructureModel.hdnSavebtn = null;
                    string delete_state_id = _AddressStructureModel.state_id.ToString();
                    string massage = _addressStructure_ISERVICES.deleteaddressstructure(delete_state_id, str);
                    //Session["Message"] = massage+"Addr";
                    _AddressStructureModel.Message = massage + "Addr";
                    //Session["Collapse"] = "1";
                    _AddressStructureModel.Collapse = "1";
                    _AddressStructureModel.country_name = null;
                    TempData["ModelData"] = _AddressStructureModel;
                    return RedirectToAction("AddressStructure");
                }
                string str1 = _AddressStructureModel.DhdnAction;
                if (str1 == "DeleteDistrict")
                {
                    _AddressStructureModel.hdnSavebtn = null;
                    string delete_district_id = _AddressStructureModel.district_id;
                    string massage = _addressStructure_ISERVICES.deleteaddressstructure(delete_district_id, str1);
                    //Session["Message"] = massage + "Addr";
                    _AddressStructureModel.Message = massage + "Addr";
                    //Session["Collapse"] = "2";
                    _AddressStructureModel.Collapse = "2";
                    _AddressStructureModel.Dstate_name = null;
                    TempData["ModelData"] = _AddressStructureModel;
                    return RedirectToAction("AddressStructure");
                }
                string str2 = _AddressStructureModel.ChdnAction;
                if (str2 == "DeleteCityandPin")
                {
                    _AddressStructureModel.hdnSavebtn = null;
                    string delete_CityandPin_id = _AddressStructureModel.City_id;
                    string massage = _addressStructure_ISERVICES.deleteaddressstructure(delete_CityandPin_id, str2);
                    //Session["Message"] = massage + "Addr";
                    _AddressStructureModel.Message = massage + "Addr";
                    //Session["Collapse"] = "3";
                    _AddressStructureModel.Collapse = "3";
                    _AddressStructureModel.City_id = null;
                    _AddressStructureModel.City_name = null;
                    TempData["ModelData"] = _AddressStructureModel;
                    return RedirectToAction("AddressStructure");
                }
                switch (Command)
                {
                    case "SaveState":

                        string TransType = _AddressStructureModel.hdnAction;
                        string country = _AddressStructureModel.country_name;
                        string state = _AddressStructureModel.state_name;
                        string state_code = _AddressStructureModel.state_code;
                        string state_id;
                        if (!string.IsNullOrEmpty(_AddressStructureModel.state_id.ToString()))
                        {

                            state_id = _AddressStructureModel.state_id.ToString();
                        }
                        else
                        {
                            state_id = "0";
                        }
                        String SaveMessage = _addressStructure_ISERVICES.insertstate(state, state_code, country, state_id, TransType);
                        string TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                        if (Message == "SaveState" || Message == "UpdateState")
                        {
                            //Session["Message"] = "SaveAddress";
                            _AddressStructureModel.Message = "SaveAddress";
                            //Session["Collapse"] = "1";
                            _AddressStructureModel.Collapse = "1";
                            _AddressStructureModel.country_name = null;
                        }
                        if (Message == "Duplicate")
                        {
                            _AddressStructureModel.hdnSavebtn = null;
                            //Session["Message"] = "DuplicateState";
                            _AddressStructureModel.Message = "DuplicateState";
                            //Session["state_id"] = state_id;
                            _AddressStructureModel.state_id = Convert.ToInt32(state_id);
                            //Session["country"] = country;
                            _AddressStructureModel.country = country;
                            //Session["state"] = state;
                            _AddressStructureModel.state = state;
                            //Session["state_code"] = state_code;
                            _AddressStructureModel.state_code = state_code;
                        }
                        if (Message == "DuplicateStateCode")
                        {
                            //Session["Message"] = "DuplicateStateCode";
                            //Session["state_id"] = state_id;
                            //Session["country"] = country;
                            //Session["state"] = state;
                            //Session["state_code"] = state_code;
                            _AddressStructureModel.hdnSavebtn = null;
                            _AddressStructureModel.Message = "DuplicateStateCode";
                            _AddressStructureModel.state_id = Convert.ToInt32(state_id);
                            _AddressStructureModel.country = country;
                            _AddressStructureModel.state = state;
                            _AddressStructureModel.state_code = state_code;
                        }
                        if (Message == "exist")
                        {
                            _AddressStructureModel.hdnSavebtn = null;
                            _AddressStructureModel.state_id = Convert.ToInt32(state_id);
                            _AddressStructureModel.Message = Message;
                            //Session["Collapse"] = "1";
                            _AddressStructureModel.Collapse = "1";
                        }
                            TempData["ModelData"] = _AddressStructureModel;
                        return RedirectToAction("AddressStructure");
                    case "SaveDistrict":

                        TransType = _AddressStructureModel.DhdnAction;
                        if (_AddressStructureModel.Dcountry_id == null)
                        {
                            country = _AddressStructureModel.country_id;
                        }
                        else
                        {
                            country = _AddressStructureModel.Dcountry_id;
                        }
                        state_id = _AddressStructureModel.Dstate_name;
                        string district_name = _AddressStructureModel.district_name;
                        string district_id;
                        if (!string.IsNullOrEmpty(_AddressStructureModel.district_id))
                        {

                            district_id = _AddressStructureModel.district_id;
                        }
                        else
                        {
                            district_id = "0";
                        }
                        SaveMessage = _addressStructure_ISERVICES.insertdistrict(district_id, district_name, state_id, country, TransType);
                        TaxCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                        if (Message == "SaveDistrict" || Message == "UpdateDistrict")
                        {
                            //Session["Message"] = "SaveAddress";
                            //Session["Collapse"] = "2";
                            _AddressStructureModel.Message = "SaveAddress";
                            _AddressStructureModel.Collapse = "2";
                            _AddressStructureModel.Dstate_name = null;
                        }
                        if (Message == "Duplicate")
                        {
                            _AddressStructureModel.hdnSavebtn = null;
                            //Session["Message"] = "DuplicateDistrict";
                            //Session["district_id"] = district_id;
                            //Session["district_name"] = district_name;
                            //Session["state_id"] = state_id;
                            //Session["country_id"] = country;
                            //Session["country_nm"] = _AddressStructureModel.Dcountry_name;
                            //Session["TransType"] = TransType;

                            _AddressStructureModel.Message = "DuplicateDistrict";
                            _AddressStructureModel.district_id = district_id;
                            _AddressStructureModel.district_name = district_name;
                            _AddressStructureModel.state_id = Convert.ToInt32(state_id);
                            _AddressStructureModel.country_id = country;
                            _AddressStructureModel.country_nm = _AddressStructureModel.Dcountry_name;
                            _AddressStructureModel.TransType = TransType;
                        }
                        if (Message == "exist")
                        {
                            _AddressStructureModel.hdnSavebtn = null;
                            _AddressStructureModel.Message = Message + "AAddr";
                            //Session["Collapse"] = "1";
                            _AddressStructureModel.Collapse = "2";
                        }
                        TempData["ModelData"] = _AddressStructureModel;
                        return RedirectToAction("AddressStructure");
                    case "SavecityAndpin":
                        TransType = _AddressStructureModel.ChdnAction;
                        country = _AddressStructureModel.Ccountry_id;
                        state_id = _AddressStructureModel.Cstate_id;
                        district_id = _AddressStructureModel.Cdistrict_name;
                        string city = _AddressStructureModel.City_name;
                        // string pin = _AddressStructureModel.pin;
                        string city_id;
                        if (!string.IsNullOrEmpty(_AddressStructureModel.City_id))
                        {

                            city_id = _AddressStructureModel.City_id;
                        }
                        else
                        {
                            city_id = "0";
                        }

                        SaveMessage = _addressStructure_ISERVICES.insertcityandpin(city_id, city, district_id, state_id, country, TransType);
                        string code = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        Message = SaveMessage.Substring(0, SaveMessage.IndexOf('-'));
                        if (Message == "SavecityAndpin" || Message == "UpdatecityAndpin")
                        {
                            //Session["Message"] = "SaveAddress";
                            //Session["Collapse"] = "3";

                            _AddressStructureModel.Message = "SaveAddress";
                            _AddressStructureModel.Collapse = "3";
                            _AddressStructureModel.Cdistrict_name= null;
                            _AddressStructureModel.Cdistrict_id= null;
                            _AddressStructureModel.Ccountry_name= null;
                            _AddressStructureModel.Cstate_name=null;
                        }
                        if (Message == "Duplicate")
                        {
                            // Session["Message"] = "Duplicate_C";
                            // Session["district_id"] = district_id;
                            // Session["city_name"] = city;
                            //// Session["pin"] = pin;
                            // Session["city_id"] = city_id;
                            // Session["TransType"] = TransType;
                            _AddressStructureModel.hdnSavebtn = null;
                            _AddressStructureModel.Message = "Duplicate_C";
                            _AddressStructureModel.district_id = district_id;
                            _AddressStructureModel.City_name = city;
                            _AddressStructureModel.City_id = city_id;
                            _AddressStructureModel.TransType = TransType;
                        }
                        TempData["ModelData"] = _AddressStructureModel;
                        return RedirectToAction("AddressStructure");
                    default:
                        return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        public ActionResult countrylist(AddressStructureModel _AddressStructureModel)
        {
           
                string GroupName = string.Empty;
            Dictionary<string, string> Countrylist = new Dictionary<string, string>();
            try
                {
                    if (string.IsNullOrEmpty(Convert.ToString(_AddressStructureModel.Name)))
                    {
                        GroupName = "";
                    }
                    else
                    {
                        GroupName = Convert.ToString(_AddressStructureModel.Name);
                    }

                Countrylist = _addressStructure_ISERVICES.countrylist(GroupName);
                List<country_list> _countrylist = new List<country_list>();
                foreach (var dr in Countrylist)
                {
                    country_list _country_List = new country_list();
                    _country_List.country_id = Convert.ToInt32(dr.Key);
                    _country_List.country_name = dr.Value;
                    _countrylist.Add(_country_List);
                }
                _AddressStructureModel.country_List = _countrylist;
            }
            catch (Exception ex)
                {
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, ex);
                    return Json("ErrorPage");
                }

                return Json(Countrylist.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ddlstatelist(AddressStructureModel _AddressStructureModel)
        {
          
            string GroupName = string.Empty;
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(_AddressStructureModel.Name)))
                {
                    GroupName = "";
                }
                else
                {
                    GroupName = Convert.ToString(_AddressStructureModel.Name);
                }

                list = _addressStructure_ISERVICES.statelist(GroupName);
                List<state_list> _statelist = new List<state_list>();
                foreach (var dr in list)
                {
                    state_list _state_List = new state_list();
                    _state_List.dist_state_id = Convert.ToInt32(dr.Key);
                    _state_List.dist_state_name = dr.Value;
                    _statelist.Add(_state_List);
                }

                _AddressStructureModel.state_Lists = _statelist;

                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return Json(list.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);

         

        }
        public ActionResult ddldistrictlist(AddressStructureModel _AddressStructureModel)
        {

            string GroupName = string.Empty;
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(_AddressStructureModel.Name)))
                {
                    GroupName = "";
                }
                else
                {
                    GroupName = Convert.ToString(_AddressStructureModel.Name);
                }
                list = _addressStructure_ISERVICES.districtlist(GroupName);
                List<district_list> _districtlist = new List<district_list>();
                foreach (var dr in list)
                {
                    district_list _district_List = new district_list();
                    _district_List.dist_id = Convert.ToInt32(dr.Key);
                    _district_List.dist_name = dr.Value;
                    _districtlist.Add(_district_List);
                }
                _AddressStructureModel.district_Lists = _districtlist;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return Json(list.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);



        }
        [HttpPost]
        public JsonResult Getcountryonchangecity(string state_id)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable HoCompData = _addressStructure_ISERVICES.getcountrylist(state_id).Tables[0];
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
        public JsonResult Getcountryonchangedistrict(string district_id)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable HoCompData = _addressStructure_ISERVICES.getcountrylist(district_id).Tables[1];
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
        public JsonResult GetStateCodeOnchange(string Country_id)
        {
            JsonResult DataRows = null;
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataTable HoCompData = _addressStructure_ISERVICES.GetStateCodeOnchange(CompID).Tables[0];
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
        public ActionResult LoadData()
        {
            var draw = Request.Form["draw"];
            var start = Convert.ToInt32(Request.Form["start"]);
            var length = Convert.ToInt32(Request.Form["length"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumnName = Request.Form.GetValues("columns[" + Request.Form["order[0][column]"] + "][name]").FirstOrDefault();
            var sortDirection = Request.Form["order[0][dir]"];

            // Fetch dataset from service or DB
            DataSet ds = _addressStructure_ISERVICES.getAddressStructureLists();
            var dt = ds.Tables[2];
            if (dt == null)
            {
                return Json(new { draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() }, JsonRequestBehavior.AllowGet);
            }

            var rows = dt.AsEnumerable();

            // Filtering
            if (!string.IsNullOrEmpty(searchValue))
            {
                rows = rows.Where(row =>
                    row.Field<string>("city_name").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    row.Field<string>("district_name").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    row.Field<string>("state_name").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    row.Field<string>("country_name").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            var recordsFiltered = rows.Count();
            var recordsTotal = dt.Rows.Count;

            // Sorting
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                rows = sortDirection == "asc"
                    ? rows.OrderBy(r => r[sortColumnName])
                    : rows.OrderByDescending(r => r[sortColumnName]);
            }

            // Paging and projection
            var data = rows.Skip(start).Take(length).Select((row, index) => new
            {
                srno = start + index + 1,
                city_id = row.Field<int>("city_id"),
                city_name = row.Field<string>("city_name"),
                district_id = row.Field<int>("district_id"),
                district_name = row.Field<string>("district_name"),
                state_id = row.Field<int>("state_id"),
                state_name = row.Field<string>("state_name"),
                country_id = row.Field<int>("country_id"),
                country_name = row.Field<string>("country_name"),
                dependcy = row.Field<string>("dependcy")
            }).ToList();

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data
            }, JsonRequestBehavior.AllowGet);
        }



    }
}
