
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TransporterSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.TransporterSetup
{
    public class TransporterSetupController : Controller
    {
        string CompID, Br_ID, language, UserId = String.Empty;
        string DocumentMenuId = "103130", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        DataTable dt;
        TransporterSetupModel _TransporterSetupModel;
        TransporterSetup_ISERVICES _TransporterSetup_ISERVICES;
        public TransporterSetupController(Common_IServices _Common_IServices, TransporterSetup_ISERVICES _TransporterSetup_ISERVICES)
        {
            this._TransporterSetup_ISERVICES = _TransporterSetup_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: BusinessLayer/TransporterSetup
        public ActionResult TransporterSetup()
        {
            CommonPageDetails();
            GetTransporterDetailsModel objModel = new GetTransporterDetailsModel();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            objModel.TransList = GetTransporterList(CompID);
            //ViewBag.MenuPageName = getDocumentName();
            ViewBag.TransDetails = GetTransDetails(CompID, "0", "0", "0");
            objModel.Title = title;
           
            return View("~/Areas/BusinessLayer/Views/TransporterSetup/TransporterSetupList.cshtml", objModel);
        }
        public List<TransListModel> GetTransporterList(string compId)
        {

            List<TransListModel> transporterList = new List<TransListModel>();
            transporterList.Add(new TransListModel { TransId = "0", TransName = "---ALL--" });
            DataTable dt = GetTransDetails(compId, "0", "0", "0");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    transporterList.Add(new TransListModel { TransId = dr["trans_id"].ToString(), TransName = dr["trans_name"].ToString() });
                }
            }
            return transporterList;
        }
        public DataTable GetTransDetails(string compId, string transId, string transType, string transMode)
        {
            return _TransporterSetup_ISERVICES.GetTransportDetails(compId, transId, transType, transMode);
        }
        public ActionResult GetTransporterDetails(string transId, string transType, string transMode)
        {
            CommonPageDetails();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            //GetTransporterDetailsModel objModel = new GetTransporterDetailsModel();
            //ViewBag.MenuPageName = getDocumentName();
            ViewBag.TransDetails = GetTransDetails(CompID, transId, transType, transMode);
            GetTransporterDetailsModel objModel = new GetTransporterDetailsModel();
            objModel.SearchStatus = "SEARCH";
            return View("~/Areas/BusinessLayer/Views/Shared/PartialTransporterSetupList.cshtml", objModel);
        }
        public ActionResult EditTransSetup(string transId)
        {
            TransporterSetupModel asModel = new TransporterSetupModel();
            asModel.Command = "Update";
            asModel.Message = "New";
            asModel.TransType = "Update";
            asModel.TransId = transId;
            //asModel.appStatus = "D";
            asModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = asModel;
            var transType = "Update";
            var btnName = "BtnToDetailPage";
            var command = "Add";
            return (RedirectToAction("TransporterSetupDetail", new { TransType = transType, BtnName = btnName, command = command }));
        }
        public ActionResult AddTransporterSetupDetail()
        {
            TransporterSetupModel _TransporterSetupModel = new TransporterSetupModel();
            _TransporterSetupModel.Message = "New";
            _TransporterSetupModel.Command = "Add";
            _TransporterSetupModel.TransMode = _TransporterSetupModel.TransMode;
            _TransporterSetupModel.TransMode = "D";
            //ViewBag.DocumentStatus = _TransporterSetupModel.AppStatus;
            _TransporterSetupModel.TransType = "Save";
            _TransporterSetupModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _TransporterSetupModel;
            CommonPageDetails();
            TempData["ListFilterData"] = null;
            return RedirectToAction("TransporterSetupDetail", _TransporterSetupModel);
        }
        public ActionResult TransporterSetupDetail(TransporterSetupModel _TransporterSetupModel1, string TransType, string BtnName, string command)
        {
            try
            {
                CommonPageDetails();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserId = Session["UserId"].ToString();
                }
                var _TransporterSetupModel = TempData["ModelData"] as TransporterSetupModel;
                if (_TransporterSetupModel != null)
                {               
                    _TransporterSetupModel.DocumentMenuId = DocumentMenuId;                    
                    // _TranportDetails.AttachMentDetailItmStp = null;
                    //_TranportDetails.Guid = null;
                    DataSet data = new DataSet();
                    DataTable dt = new DataTable();
                    if (string.IsNullOrEmpty(_TransporterSetupModel.TransId))
                        _TransporterSetupModel.TransId = "-1";
                    if (_TransporterSetupModel.Message != "Duplicate")
                    {
                        data = _TransporterSetup_ISERVICES.GetDeatilTrasportData(CompID, _TransporterSetupModel.TransId, "0", "0");
                        dt = data.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            _TransporterSetupModel.TransName = dt.Rows[0]["trans_name"].ToString();
                            //_TransporterSetupModel.Command = "Update";
                            _TransporterSetupModel.TransportType = dt.Rows[0]["trans_type"].ToString().Trim();
                            _TransporterSetupModel.TransMode = dt.Rows[0]["trans_mode"].ToString().Trim();
                            _TransporterSetupModel.TransAdd = dt.Rows[0]["trans_add"].ToString().Trim();
                            _TransporterSetupModel.TransCity = dt.Rows[0]["trans_city"].ToString().Trim();
                            _TransporterSetupModel.TransState = dt.Rows[0]["trans_state"].ToString().Trim();
                            _TransporterSetupModel.TransDist = dt.Rows[0]["trans_dist"].ToString().Trim();
                            _TransporterSetupModel.TransCntry = dt.Rows[0]["trans_cntry"].ToString().Trim();
                            _TransporterSetupModel.TransPin = dt.Rows[0]["trans_pin"].ToString().Trim();
                            _TransporterSetupModel.transPanNo = dt.Rows[0]["trans_pan_no"].ToString().Trim();
                            _TransporterSetupModel.transGstFirstPart = dt.Rows[0]["LGST"].ToString().Trim();
                            _TransporterSetupModel.transGstMidPart = dt.Rows[0]["MGST"].ToString().Trim();
                            _TransporterSetupModel.transGstLastPart = dt.Rows[0]["RGST"].ToString().Trim();
                            _TransporterSetupModel.Remarks = dt.Rows[0]["remarks"].ToString().Trim();
                            _TransporterSetupModel.CreatedBy = dt.Rows[0]["created_by"].ToString().Trim();
                            _TransporterSetupModel.CreatedOn = dt.Rows[0]["create_dt"].ToString().Trim();
                            _TransporterSetupModel.AmendedBy = dt.Rows[0]["mod_by"].ToString().Trim();
                            _TransporterSetupModel.AmendedOn = dt.Rows[0]["mod_dt"].ToString().Trim();
                            _TransporterSetupModel.Gst_Cat = dt.Rows[0]["gst_cat"].ToString().Trim();
                            bool isOnHold = false;
                            if (dt.Rows[0]["on_hold"].ToString().Trim() == "Y")
                                isOnHold = true;
                            _TransporterSetupModel.OnHold = isOnHold;



                        }
                        if (data.Tables[1].Rows.Count > 0) /*Added By Nitesh 30042024 For Attachment*/
                        {
                            ViewBag.AttechmentDetails = data.Tables[1];
                        }
                    }
                    _TransporterSetupModel.Title = title;
                    List<TransCountry> _TransList = new List<TransCountry>();
                    _TransList.Add(new TransCountry { country_id = "0", country_name = "---Select---" });
                    List<Country> _TransList2 = new List<Country>();
                    CommonAddress_Detail _Model = new CommonAddress_Detail();
                    string transportType = "D";
                    if (!string.IsNullOrEmpty(_TransporterSetupModel.TransportType))
                        transportType = _TransporterSetupModel.TransportType;
                    DataTable dtcntry = GetTransportCountry(transportType);

                    foreach (DataRow dr in dtcntry.Rows)
                    {
                        TransCountry _Custcurr = new TransCountry();
                        _Custcurr.country_id = dr["country_id"].ToString();
                        _Custcurr.country_name = dr["country_name"].ToString();
                        _TransList.Add(_Custcurr);
                        _TransList2.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });
                    }
                    _Model.countryList = _TransList2;

                    _TransporterSetupModel.countryList = _TransList;
                    List<TransState> state = new List<TransState>();
                    state.Add(new TransState { state_id = "0", state_name = "---Select---" });
                    string transCountry = "";
                    if (!string.IsNullOrEmpty(_TransporterSetupModel.TransCntry))
                        transCountry = _TransporterSetupModel.TransCntry;
                    else
                        transCountry = dtcntry.Rows[0]["country_id"].ToString();

                    DataTable dtStates = _TransporterSetup_ISERVICES.GetstateOnCountryDDL(transCountry);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new TransState { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    _TransporterSetupModel.StateList = state;

                    string transState = "0";
                    List<DistrictModel> DistList = new List<DistrictModel>();
                    DistList.Add(new DistrictModel { district_id = "0", district_name = "---Select---" });
                    if (!string.IsNullOrEmpty(_TransporterSetupModel.TransState))
                        transState = _TransporterSetupModel.TransState;
                    else
                        transState = "0";
                    DataTable dtDist = _TransporterSetup_ISERVICES.GetDistrictOnStateDDL(transState);
                    if (dtDist.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDist.Rows)
                        {
                            DistList.Add(new DistrictModel { district_id = dr["district_id"].ToString(), district_name = dr["district_name"].ToString() });
                        }
                    }
                    _TransporterSetupModel.DistrictList = DistList;

                    string transDist = "0";
                    if (!string.IsNullOrEmpty(_TransporterSetupModel.TransDist))
                        transDist = _TransporterSetupModel.TransDist;
                    else
                        transDist = "0";
                    DataTable dtCities = _TransporterSetup_ISERVICES.GetCityOnDistrictDDL(transDist);

                    List<CityList> cities = new List<CityList>();
                    cities.Add(new CityList { City_Id = "0", City_Name = "---Select---" });
                    if (dtCities.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCities.Rows)
                        {
                            cities.Add(new CityList { City_Id = dr["city_id"].ToString(), City_Name = dr["city_name"].ToString() });
                        }
                    }
                    _TransporterSetupModel.cityLists = cities;

                    _TransporterSetupModel._CommonAddress_Detail = _Model;

                    if (dt.Rows.Count > 0)
                    {
                        if (_TransporterSetupModel.OnHold)
                            _TransporterSetupModel.TransOnHold = "Y";
                        else
                            _TransporterSetupModel.TransOnHold = "N";
                    }
                    else
                    {
                        _TransporterSetupModel.TransOnHold = "N";
                    }

                    if (_TransporterSetupModel.BtnName != null)
                    {
                        _TransporterSetupModel.BtnName = _TransporterSetupModel.BtnName;
                    }
                    _TransporterSetupModel.TransType = _TransporterSetupModel.TransType;
                    ViewBag.TransType = _TransporterSetupModel.TransType;


                    if (_TransporterSetupModel.DocumentStatus == null)
                    {
                        _TransporterSetupModel.DocumentStatus = "D";
                        ViewBag.DocumentCode = "D";
                    }
                    else
                    {
                        _TransporterSetupModel.DocumentStatus = _TransporterSetupModel.DocumentStatus;
                        ViewBag.DocumentCode = _TransporterSetupModel.DocumentStatus;
                        ViewBag.Command = _TransporterSetupModel.Command;
                    }
                    ViewBag.DocumentStatus = _TransporterSetupModel.DocumentStatus;
                    ViewBag.DocumentCode = _TransporterSetupModel.DocumentStatus;

                    return View("~/Areas/BusinessLayer/Views/TransporterSetup/TransporterSetupDetail.cshtml", _TransporterSetupModel);
                }
                else
                {
                    // _TranportDetails.AttachMentDetailItmStp = null;
                    //_TranportDetails.Guid = null;
                    //CommonPageDetails();
                    _TransporterSetupModel1.DocumentMenuId = DocumentMenuId;
                    _TransporterSetupModel1.Title = title;
                    List<TransCountry> _TransList = new List<TransCountry>();
                    List<Country> _TransList1 = new List<Country>();
                    CommonAddress_Detail _Model = new CommonAddress_Detail();
                    dt = GetTransportCountry("D");
                    string cntryId = "0";
                    foreach (DataRow dr in dt.Rows)
                    {
                        TransCountry _Custcurr = new TransCountry();
                        _Custcurr.country_id = dr["country_id"].ToString();
                        _Custcurr.country_name = dr["country_name"].ToString();
                        _TransList.Add(_Custcurr);
                        cntryId = dr["country_id"].ToString();
                        _TransList1.Add(new Country { country_id = dr["country_id"].ToString(), country_name = dr["country_name"].ToString() });

                    }
                    _Model.countryList = _TransList1;
                    _TransporterSetupModel1.countryList = _TransList;
                    List<TransState> state = new List<TransState>();
                    state.Add(new TransState { state_id = "0", state_name = "---Select---" });
                    DataTable dtStates = _TransporterSetup_ISERVICES.GetstateOnCountryDDL(cntryId);
                    if (dtStates.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStates.Rows)
                        {
                            state.Add(new TransState { state_id = dr["state_id"].ToString(), state_name = dr["state_name"].ToString() });
                        }
                    }
                    _TransporterSetupModel1.StateList = state;

                    List<DistrictModel> DistList = new List<DistrictModel>();
                    DistList.Add(new DistrictModel { district_id = "0", district_name = "---Select---" });

                    _TransporterSetupModel1.DistrictList = DistList;


                    List<CityList> cities = new List<CityList>();
                    cities.Add(new CityList { City_Id = "0", City_Name = "---Select---" });

                    _TransporterSetupModel1.cityLists = cities;
                    if (_TransporterSetupModel1.BtnName == null)
                    {
                        _TransporterSetupModel1.BtnName = "BtnAddNew";
                    }

                    _TransporterSetupModel1._CommonAddress_Detail = _Model;

                    if (TransType != null)
                    {
                        _TransporterSetupModel1.TransType = TransType;
                        ViewBag.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _TransporterSetupModel1.Command = command;
                        ViewBag.Command = command;
                    }

                    if (_TransporterSetupModel1.BtnName == null && _TransporterSetupModel1.Command == null)
                    {
                        _TransporterSetupModel1.BtnName = "AddNew";
                        _TransporterSetupModel1.Command = "Add";
                        //_TransporterSetupModel1.AppStatus = "D";
                        //ViewBag.DocumentStatus = _TransporterSetupModel1.AppStatus;
                        _TransporterSetupModel1.TransType = "Save";
                        _TransporterSetupModel1.BtnName = "BtnAddNew";

                    }

                    if (_TransporterSetupModel1.BtnName != null)
                    {
                        _TransporterSetupModel1.BtnName = _TransporterSetupModel1.BtnName;
                    }
                    _TransporterSetupModel1.TransType = _TransporterSetupModel1.TransType;
                    ViewBag.TransType = _TransporterSetupModel1.TransType;
                    if (_TransporterSetupModel1.DocumentStatus != null)
                    {
                        _TransporterSetupModel1.DocumentStatus = _TransporterSetupModel1.DocumentStatus;
                        ViewBag.DocumentStatus = _TransporterSetupModel1.DocumentStatus;
                        ViewBag.DocumentCode = _TransporterSetupModel1.DocumentStatus;
                    }
                    if (TransType != null)
                    {
                        _TransporterSetupModel1.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _TransporterSetupModel1.Command = command;
                    }
                    _TransporterSetupModel1.Title = title;

                    return View("~/Areas/BusinessLayer/Views/TransporterSetup/TransporterSetupDetail.cshtml", _TransporterSetupModel1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransporterSetupBtnCommand(TransporterSetupModel _TransporterSetupModel, string command)
        {
            try
            {

                if (_TransporterSetupModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        _TransporterSetupModel = new TransporterSetupModel();
                        _TransporterSetupModel.Message = "New";
                        _TransporterSetupModel.Command = "Add";
                        _TransporterSetupModel.SaveAndApproveBtnDisatble = "";

                        _TransporterSetupModel.TransMode = _TransporterSetupModel.TransMode;
                        _TransporterSetupModel.TransMode = "D";
                        _TransporterSetupModel.TransType = "Save";
                        _TransporterSetupModel.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _TransporterSetupModel;
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("TransporterSetupDetail", "TransporterSetup");

                    case "Edit":
                        var TransType = "Update";
                        var BtnName = "BtnEdit";
                        var TransCodeURL = _TransporterSetupModel.TransId;
                        _TransporterSetupModel.TransType = "Update";
                        _TransporterSetupModel.Command = command;
                        _TransporterSetupModel.BtnName = "BtnEdit";
                        _TransporterSetupModel.Message = "New";
                        _TransporterSetupModel.SaveAndApproveBtnDisatble = "";
                        TempData["ModelData"] = _TransporterSetupModel;


                        command = _TransporterSetupModel.Command;
                        TempData["ModelData"] = _TransporterSetupModel;
                        TempData["ListFilterData"] = _TransporterSetupModel.ListFilterData1;
                        return (RedirectToAction("TransporterSetupDetail", new { TransCodeURL = TransCodeURL, TransType, BtnName, command }));

                    case "Delete":
                        _TransporterSetupModel.Command = command;
                        _TransporterSetupModel.BtnName = "BtnDelete";
                        _TransporterSetupModel.SaveAndApproveBtnDisatble = "";
                        DeleteTransDetails(_TransporterSetupModel);
                        TempData["ModelData"] = _TransporterSetupModel;
                        TempData["ListFilterData"] = _TransporterSetupModel.ListFilterData1;
                      
                        return RedirectToAction("TransporterSetupDetail");
                    case "Save":
                        _TransporterSetupModel.Command = command;
                        if (_TransporterSetupModel.TransType == null)
                            _TransporterSetupModel.TransType = command;
                        SaveTransporterDetail(_TransporterSetupModel);
                        if (_TransporterSetupModel.Message == "DataNotFound")
                        {
                            _TransporterSetupModel.SaveAndApproveBtnDisatble = "";
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_TransporterSetupModel.Message == "Duplicate")
                        {
                            _TransporterSetupModel.Message = "Duplicate";
                            _TransporterSetupModel.Command = "Add";
                            _TransporterSetupModel.SaveAndApproveBtnDisatble = "";

                            _TransporterSetupModel.TransMode = _TransporterSetupModel.TransMode;
                            //_TransporterSetupModel.TransMode = "D";
                            _TransporterSetupModel.TransType = "Save";
                            _TransporterSetupModel.BtnName = "BtnAddNew";
                            TransType = _TransporterSetupModel.TransType;
                            BtnName = _TransporterSetupModel.BtnName;
                             _TransporterSetupModel.TransId="0";
                            TransCodeURL = _TransporterSetupModel.TransId;

                        }
                        else
                        {
                            _TransporterSetupModel.Message = "Save";
                            _TransporterSetupModel.Command = command;
                            TransCodeURL = _TransporterSetupModel.TransId;
                            TransType = _TransporterSetupModel.TransType;
                            BtnName = _TransporterSetupModel.BtnName;
                        }
                        //cust_id = _TransporterSetupModel.cust_id;
                        //_TransporterSetupModel.Transport_ID = cust_id;
                        _TransporterSetupModel.SaveAndApproveBtnDisatble = "";
                      
                        TempData["ModelData"] = _TransporterSetupModel;
                        TempData["ListFilterData"] = _TransporterSetupModel.ListFilterData1;
                        return (RedirectToAction("TransporterSetupDetail", new { TransCodeURL = TransCodeURL, TransType, BtnName, command }));


                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        _TransporterSetupModel.Command = command;
                        //Transporter_ID = _TransporterSetupModel.Transporter_ID;
                        //_TransporterSetupModel.Transporter_ID = Transporter_ID;
                        TransCodeURL = _TransporterSetupModel.TransId;
                        TransType = _TransporterSetupModel.TransType;
                        BtnName = _TransporterSetupModel.BtnName;
                        _TransporterSetupModel.SaveAndApproveBtnDisatble = "";
                        TempData["ModelData"] = _TransporterSetupModel;
                        TempData["ListFilterData"] = _TransporterSetupModel.ListFilterData1;
                        return (RedirectToAction("TransporterSetupDetail", new { TransCodeURL = TransCodeURL, TransType, BtnName, command }));



                    case "Refresh":
                        TransporterSetupModel _TransporterSetupModelRefresh = new TransporterSetupModel();
                        _TransporterSetupModel.Message = null;
                        _TransporterSetupModelRefresh.Command = command;
                        _TransporterSetupModelRefresh.TransType = "Refresh";
                        _TransporterSetupModelRefresh.BtnName = "Refresh";
                        _TransporterSetupModelRefresh.SaveAndApproveBtnDisatble = "";
                        TempData["ModelData"] = _TransporterSetupModelRefresh;
                        TempData["ListFilterData"] = _TransporterSetupModel.ListFilterData1;
                        return RedirectToAction("TransporterSetupDetail");

                    case "Print":
                    //return GenratePdfFile(_TransporterSetupModel);
                    case "BacktoList":
                        TempData["ListFilterData"] = _TransporterSetupModel.ListFilterData1;
                        return RedirectToAction("TransporterSetup", "TransporterSetup");

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
        public ActionResult SaveTransporterDetail(TransporterSetupModel _TransporterSetupModel)
        {
            string SaveMessage = "";
            /*getDocumentName();*/ /* To set Title*/
            CommonPageDetails();
            string PageName = title.Replace(" ", "");
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                if (Session["Userid"] != null)
                    UserId = Session["Userid"].ToString();
                _TransporterSetupModel.CompId = CompID;
                if (_TransporterSetupModel.OnHold)
                    _TransporterSetupModel.TransOnHold = "Y";
                else
                    _TransporterSetupModel.TransOnHold = "N";
                //dtrow["on_hold"] = _TransporterSetupModel.Transport_OnHold;
                _TransporterSetupModel.MacId = "Mac : " + Session["UserMacaddress"].ToString() + " " + "System : " + Session["UserSystemName"].ToString() + " IP : " + Session["UserIP"].ToString();
                _TransporterSetupModel.createModId = UserId;
                _TransporterSetupModel.TransStatus = "D";
                if (!string.IsNullOrEmpty(_TransporterSetupModel.transGstMidPart))
                    _TransporterSetupModel.transGstNo = _TransporterSetupModel.transGstFirstPart + _TransporterSetupModel.transGstMidPart + _TransporterSetupModel.transGstLastPart;

                DataTable ItemAttachments = new DataTable(); /**Added By Nitesh for Attachment 30042024 **/

                var Detailsattchment = TempData["ModelDataattch"] as TransportAttachment;
                TempData["ModelDataattch"] = null;
                DataTable dtAttachment = new DataTable();
                if (_TransporterSetupModel.attatchmentdetail != null)
                {
                    if (Detailsattchment != null)
                    {
                        if (Detailsattchment.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = Detailsattchment.AttachMentDetailItmStp as DataTable;
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
                        if (_TransporterSetupModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _TransporterSetupModel.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_TransporterSetupModel.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_TransporterSetupModel.TransId))
                            {
                                dtrowAttachment1["id"] = _TransporterSetupModel.TransId;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y"; /*Not Use in Table So Accouding to type y is tempery declare Not Use*/
                            dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    if (_TransporterSetupModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string TransId = string.Empty;
                            if (!string.IsNullOrEmpty(_TransporterSetupModel.TransId))
                            {
                                TransId = _TransporterSetupModel.TransId;
                            }
                            else
                            {
                                TransId = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + TransId.Replace("/", "") + "*");

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
                    ItemAttachments = dtAttachment;
                }
                //foreach (DataRow dr in ItemAttachments.Rows)
                //{
                //    string file_name = dr["file_name"].ToString();
                //    file_name = file_name.Substring(file_name.IndexOf('_') + 1);
                //    var arr = file_name.Split('.');
                //    string file_name1 = arr[0].Substring(0, arr[0].Length - 1) + "." + arr[1];
                //    if (_TransporterSetupModel.attatchmentDefaultdetail == file_name || _TransporterSetupModel.attatchmentDefaultdetail == file_name1)
                //    {
                //        dr["file_def"] = "Y";
                //    }
                //    else
                //    {
                //        dr["file_def"] = "N";
                //    }
                //}
               if( _TransporterSetupModel.Gst_Cat == null){
                    _TransporterSetupModel.Gst_Cat = "RG";
                }
                SaveMessage = _TransporterSetup_ISERVICES.InsertTransport_Details(_TransporterSetupModel, ItemAttachments);
                if (SaveMessage.Contains("DataNotFound"))
                {
                    var msg = ("Data Not found");
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    _TransporterSetupModel.Message = msg;
                    return RedirectToAction("TransporterSetupDetail");
                }
                string[] transId = SaveMessage.Split(',');
                if (SaveMessage.Contains("Save"))
                {
                    string Guid = "";
                    if (Detailsattchment != null)
                    {
                        if (Detailsattchment.Guid != null)
                        {
                            Guid = Detailsattchment.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    if (ItemAttachments.Rows.Count > 0)
                    {
                        comCont.ResetImageLocation(CompID, "00", guid, PageName, transId[1].ToString(), _TransporterSetupModel.TransType, ItemAttachments);
                    }
                }
                if (SaveMessage.Contains("Update") || SaveMessage.Contains("Save"))
                {

                    _TransporterSetupModel.Message = "Save";
                    _TransporterSetupModel.TransId = transId[1].ToString();                
                    _TransporterSetupModel.TransType = "Update";
                    Session["brid"] = null;
                }
                if (SaveMessage.Contains("Duplicate"))
                {
                    _TransporterSetupModel.Message = "Duplicate";
                    _TransporterSetupModel.Command = "New";
                  //  _TransporterSetupModel.TransId = transId[1].ToString();
                    _TransporterSetupModel.TransType = "Duplicate";

                }
                _TransporterSetupModel.BtnName = "BtnSave";
                TempData["ModelData"] = _TransporterSetupModel;
                return RedirectToAction("TransporterSetupDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_TransporterSetupModel.TransType == "Save")
                    {

                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + Br_ID, PageName, "", Server);
                    }
                }
                throw ex;
            }
        }

        [NonAction]
        private DataTable GetTransportCountry(string TransModetype)
        {
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _TransporterSetup_ISERVICES.GetCountryBehfOf_HOD_Organisation(Comp_ID, TransModetype);

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
        public JsonResult GetCountryonTransMode(string TransportMode)
        {
            JsonResult DataRows = null;
            try
            {
                List<TransCountry> _TransList = new List<TransCountry>();
                _TransporterSetupModel = new TransporterSetupModel();
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataTable dt = _TransporterSetup_ISERVICES.GetCountryBehfOf_HOD_Organisation(Comp_ID, TransportMode);
                if (TransportMode == "O")
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
                DataTable dt = _TransporterSetup_ISERVICES.GetstateOnCountryDDL(ddlCountryID);
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
                DataTable dt = _TransporterSetup_ISERVICES.GetDistrictOnStateDDL(ddlStateID);
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
                DataTable dt = _TransporterSetup_ISERVICES.GetCityOnDistrictDDL(ddlDistrictID);
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
                    UserId = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserId, DocumentMenuId, language);
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
        public JsonResult GetStateCode(string stateId)
        {
            JsonResult DataRows = null;
            try
            {
                DataSet ds = _TransporterSetup_ISERVICES.GetStateCode(stateId);
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
        private ActionResult DeleteTransDetails(TransporterSetupModel _TransporterSetupModel)
        {
            try
            {
                CommonPageDetails();
                //CustomerDetails _CustomerDetails = new CustomerDetails();
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string result = _TransporterSetup_ISERVICES.DeleteTransDetails(CompID, _TransporterSetupModel.TransId);
                if (result == "DELETED")
                {
                    _TransporterSetupModel.Message = "Deleted";
                    _TransporterSetupModel.Command = "Delete";
                    _TransporterSetupModel.TransType = "Refresh";
                    _TransporterSetupModel.BtnName = "Delete";
                    //_CustomerDetails.hold_reason = null;
               
                    TempData["ModelData"] = _TransporterSetupModel;
                    return RedirectToAction("TransporterSetupDetail", _TransporterSetupModel);
                }
                else
                {
                    _TransporterSetupModel.Message = "Dependency";
                    _TransporterSetupModel.Command = "Delete";
                    return View("~/Areas/BusinessLayer/Views/TransporterSetup/TransporterSetupDetail.cshtml", _TransporterSetupModel);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                TransportAttachment Detailsattchment = new TransportAttachment();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                Detailsattchment.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
              
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {                  
                    Detailsattchment.AttachMentDetailItmStp = dt;
                }
                else
                {                  
                    Detailsattchment.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = Detailsattchment;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }

    }

}