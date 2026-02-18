using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.BusinessLayer.MiscellaneousSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.MiscellaneousSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//All Session Removed By Shubham Maurya On 20-12-2022//
namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.MiscellaneousSetup
{
    public class MISController : Controller
    {
        string CompID, Br_ID, UserID, language = String.Empty;
        string DocumentMenuId = "103205101", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MISDetail_ISERVICES _MIS_ISERVICES;
        private readonly CustomerDetails_ISERVICES _CustomerDetails_ISERVICES;
        public MISController(Common_IServices _Common_IServices, MISDetail_ISERVICES _MIS_ISERVICES,
            CustomerDetails_ISERVICES CustomerDetails_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._MIS_ISERVICES = _MIS_ISERVICES;
            _CustomerDetails_ISERVICES = CustomerDetails_ISERVICES;
        }
        // GET: BusinessLayer/MIS



        public ActionResult MIS(string Setup_Type_id)
        {
            try
            {
                var _MISDetail = TempData["ModelData"] as MISDetail_Model;
                if (_MISDetail != null)
                {
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    string br_id = "";
                    if (Session["BranchId"] != null)
                    {
                        br_id = Session["BranchId"].ToString();
                    }
                    if (Setup_Type_id != null)
                    {
                        //Session["setup_type_id"] = Setup_Type_id;
                        //Session["MessageMIS"] = "New";
                        _MISDetail.setup_type_id = Setup_Type_id;
                        _MISDetail.hdnsetuptypeid = Setup_Type_id;
                        if (_MISDetail.MessageMIS == null || _MISDetail.MessageMIS == "")
                        {
                            _MISDetail.MessageMIS = "New";
                        }
                    }
                    else
                    {
                        if(_MISDetail.hdnsetuptypeid != "" && _MISDetail.hdnsetuptypeid != null)
                        {
                            _MISDetail.setup_type_id = _MISDetail.hdnsetuptypeid;
                        }
                       
                    }
                    //if (Session["MessageMIS"] != null)
                    if (_MISDetail.MessageMIS != null)
                    {
                        //ViewBag.Message = Session["MessageMIS"].ToString();
                        _MISDetail.Message = _MISDetail.MessageMIS;
                        //Session["MessageMIS"] = "New";
                        _MISDetail.MessageMIS = "New";
                        //if (Session["TransTypeMIS"] != null)
                        if (_MISDetail.TransTypeMIS != null)
                        {
                            //if (Session["TransTypeMIS"].ToString() == "Duplicate")
                            if (_MISDetail.TransTypeMIS == "Duplicate")
                            {
                                //Session["TransTypeMIS"] = null;
                                //_MISDetail.TransTypeMIS = null;
                                //if (Session["setup_id"] != null && Session["setup_val"] != null)
                                if (_MISDetail.setup_id != null && _MISDetail.setup_val != null)
                                {
                                    //if (ViewBag.Message == "DuplicateBIN")
                                    if (_MISDetail.Message == "DuplicateBIN")
                                    {
                                        //_MISDetail.BIN_id = Session["setup_id"].ToString();
                                        //_MISDetail.BINNumber = Session["setup_val"].ToString();

                                        _MISDetail.BIN_id = _MISDetail.setup_id;
                                        _MISDetail.BINNumber = _MISDetail.setup_val;
                                    }
                                    //else if (ViewBag.Message == "DuplicateITEM PORTFOLIO")
                                    else if (_MISDetail.Message == "DuplicateITEM PORTFOLIO")
                                    {
                                        _MISDetail.Portfolio_id = _MISDetail.setup_id;
                                        _MISDetail.PortfolioName = _MISDetail.setup_val;
                                    }
                                    //else if (ViewBag.Message == "DuplicateCustomer Portfolio")
                                    else if (_MISDetail.Message == "DuplicateCustomer Portfolio")
                                    {
                                        _MISDetail.CustPortfolio_id = _MISDetail.setup_id;
                                        _MISDetail.CustPortfolioName = _MISDetail.setup_val;
                                    }
                                    //else if (ViewBag.Message == "DuplicateSupplier Portfolio")
                                    else if (_MISDetail.Message == "DuplicateSupplier Portfolio")
                                    {
                                        _MISDetail.SuppPortfolio_id = _MISDetail.setup_id;
                                        _MISDetail.SuppPortfolioName = _MISDetail.setup_val;
                                    }
                                    //else if (ViewBag.Message == "DuplicateCustomer Category")
                                    else if (_MISDetail.Message == "DuplicateCustomer Category")
                                    {
                                        //_MISDetail.CustCategory_id = Session["setup_id"].ToString();
                                        //_MISDetail.CustCategoryName = Session["setup_val"].ToString();

                                        _MISDetail.CustCategory_id = _MISDetail.setup_id;
                                        _MISDetail.CustCategoryName = _MISDetail.setup_val;
                                    }
                                    //else if (ViewBag.Message == "DuplicateSupplier Category")
                                    else if (_MISDetail.Message == "DuplicateSupplier Category")
                                    {
                                        _MISDetail.SuppCategory_id = _MISDetail.setup_id;
                                        _MISDetail.SuppCategoryName = _MISDetail.setup_val;
                                    }
                                    //else if (ViewBag.Message == "DuplicateSales Region")
                                    else if (_MISDetail.Message == "DuplicateSales Region")
                                    {
                                        _MISDetail.SalesRegion_id = _MISDetail.setup_id;
                                        _MISDetail.SalesRegionName = _MISDetail.setup_val;
                                    }
                                    //else if (ViewBag.Message == "DuplicateRequirement Area")
                                    else if (_MISDetail.Message == "DuplicateRequirement Area")
                                    {
                                        _MISDetail.RequirmentArea_id = _MISDetail.setup_id;
                                        _MISDetail.RequirmentAreaName = _MISDetail.setup_val;
                                        string SF = _MISDetail.setup_flag;
                                        if (SF == "Y")
                                        {
                                            _MISDetail.ShopFloor = true;
                                        }
                                        else
                                        {
                                            _MISDetail.ShopFloor = false;
                                        }

                                    }
                                    else if (_MISDetail.Message == "DuplicateVehicle Setup")
                                    {
                                        _MISDetail.VehicleName_id = _MISDetail.setup_id;
                                        _MISDetail.VehicleName = _MISDetail.setup_val;
                                    }
                                    else if (ViewBag.Message == "Duplicate Customer Price Setup")
                                    {
                                        _MISDetail.GroupName_id = _MISDetail.setup_id;
                                        _MISDetail.GroupName = _MISDetail.setup_val;
                                    }
                                    else if (_MISDetail.Message == "DuplicateAssestCatgory")
                                    {
                                        _MISDetail.AssetCategory_id = _MISDetail.setup_id;
                                        _MISDetail.AssetCategory = _MISDetail.setup_val;
                                    }
                                    _MISDetail.BtnName = _MISDetail.BtnNameMIS;
                                    _MISDetail.setup_id = null;
                                    _MISDetail.setup_val = null;
                                    _MISDetail.BtnNameMIS = null;
                                    _MISDetail.setup_flag = null;
                                }
                            }
                        }


                    }

                    BindViewBagLists(_MISDetail);
                    var other = new CommonController(_Common_IServices);
                    DataTable dt = other.GetBranchList(CompID);
                    List<BrList> brLists = new List<BrList>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        BrList brList = new BrList();
                        brList.br_id = dr["comp_id"].ToString();
                        brList.br_name = dr["comp_nm"].ToString();
                        brLists.Add(brList);
                    }
                    _MISDetail.brLists = brLists;
                    if (_MISDetail.TransTypeMIS == "Duplicate")
                    {
                        _MISDetail.TransTypeMIS = null;
                    }
                    else
                    {
                        _MISDetail.BINNumber = null;
                        _MISDetail.PortfolioName = null;
                        _MISDetail.CustPortfolioName = null;
                        _MISDetail.SuppPortfolioName = null;
                        _MISDetail.CustCategoryName = null;
                        _MISDetail.SuppCategoryName = null;
                        _MISDetail.SalesRegionName = null;
                        _MISDetail.RequirmentAreaName = null;
                        _MISDetail.VehicleName = null;
                        _MISDetail.GroupName = null;
                    }
                    _MISDetail.TransTypeMIS = null;

                    List<BrList> branchList = new List<BrList>();
                    DataTable dtBranches = _CustomerDetails_ISERVICES.GetBrListDAL(CompID);
                    _MISDetail.CustomerBranchList = dtBranches;

                 


                    getstatelist(_MISDetail);
                    _MISDetail.DocumentMenuId = _MISDetail.DocumentMenuId;
                    DocumentMenuId = _MISDetail.DocumentMenuId;
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/MiscellaneousSetup/MISDetail.cshtml", _MISDetail);
                }
                else
                {
                    MISDetail_Model _MISDetail1 = new MISDetail_Model();
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    string br_id = "";
                    if (Session["BranchId"] != null)
                    {
                        br_id = Session["BranchId"].ToString();
                    }
                    if (Setup_Type_id != null)
                    {
                        _MISDetail1.setup_type_id = Setup_Type_id;
                        _MISDetail1.hdnsetuptypeid = Setup_Type_id;
                        _MISDetail1.MessageMIS = "New";
                    }
                    else
                    {
                        if (_MISDetail1.hdnsetuptypeid != "" && _MISDetail1.hdnsetuptypeid != null)
                        {
                            _MISDetail1.setup_type_id = _MISDetail1.hdnsetuptypeid;
                        }

                    }
                    BindViewBagLists(_MISDetail1);

                    var other = new CommonController(_Common_IServices);
                    DataTable dt = other.GetBranchList(CompID);
                    List<BrList> brLists = new List<BrList>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        BrList brList = new BrList();
                        brList.br_id = dr["comp_id"].ToString();
                        brList.br_name = dr["comp_nm"].ToString();
                        brList.br_name = dr["comp_nm"].ToString();
                        brLists.Add(brList);
                    }
                    _MISDetail1.brLists = brLists;
                    DataTable dtBranches = _CustomerDetails_ISERVICES.GetBrListDAL(CompID);
                    _MISDetail1.CustomerBranchList = dtBranches;
                    /*Added By Nitesh 14-12-2023 15:19 for Bind Country List in Table 14*/
                    

                    _MISDetail1.DocumentMenuId = _MISDetail1.DocumentMenuId;
                    DocumentMenuId = _MISDetail1.DocumentMenuId;
                    getstatelist(_MISDetail1);  /*Added By Nitesh 14-12-2023 15:19 for Bind State List */
                    CommonPageDetails();
                    return View("~/Areas/BusinessLayer/Views/MiscellaneousSetup/MISDetail.cshtml", _MISDetail1);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private void SetSetupTypeDetails(MISDetail_Model model)
        {
            try
            {
                if (model?.setup_type_id == null)
                {
                    model.ShowBin = "show";
                    model.DocumentMenuId = "103205101";
                    DocumentMenuId = model.DocumentMenuId;
                    return;
                }

                // Define mapping between setup_type_id and configuration
                var setupConfig = new Dictionary<string, (string MenuId, Action<MISDetail_Model> SetShow)>
    {
        { "1",  ("103205101", m => m.ShowBin = "show") },
        { "2",  ("103205102", m => m.ShowItemPort = "show") },
        { "7",  ("103205107", m => m.ShowCustPort = "show") },
        { "6",  ("103205111", m => m.ShowSuppPort = "show") },
        { "8",  ("103205109", m => m.ShowCustCat = "show") },
        { "4",  ("103205110", m => m.ShowSuppCat = "show") },
        { "10", ("103205108", m => m.ShowSalesReg = "show") },
        { "11", ("103205112", m => m.ShowReqArea = "show") },
        { "12", ("103205113", m => m.ShowVehicleName = "show") },
        { "13", ("103205114", m => m.ShowGroupName = "show") },
        { "14", ("103205145", m => m.ShowAsset = "show") },
        { "15", ("103205115", m => m.ShowSEName = "show") },
        { "17", ("103205120", m => m.ShowPortSetup = "show") },
        { "18", ("103205125", m => m.ShowWastage = "show") },
        { "19", ("103205130", m => m.ShowRejectionReason = "show") },
        { "20", ("103205135", m => m.ShowEmployee = "show") },
        { "21", ("103205140", m => m.ShowGLReport = "show") },
        { "22", ("103205150", m => m.ShowCustomerZone = "show") },
        { "23", ("103205155", m => m.ShowCustomergroup = "show") }
    };

                if (setupConfig.TryGetValue(model.setup_type_id, out var config))
                {
                    model.DocumentMenuId = config.MenuId;
                    config.SetShow(model);
                }
                else 
                {
                    model.ShowBin = "show"; // default case
                }
                DocumentMenuId = model.DocumentMenuId;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        private void BindViewBagLists(MISDetail_Model model)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string br_id = "";
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                SetSetupTypeDetails(model);
                ViewBag.MenuPageName = getDocumentName();
                DataSet ds = _MIS_ISERVICES.Get_MISAllTables(CompID, br_id);
                ViewBag.BINNumberList = ds.Tables[0];
                ViewBag.ItemPortFolioList = ds.Tables[1];
                ViewBag.CustPortFolioList = ds.Tables[6];
                ViewBag.SuppPortFolioList = ds.Tables[5];
                ViewBag.CustCategoryList = ds.Tables[7];
                ViewBag.SuppCategoryList = ds.Tables[3];
                ViewBag.SalesRegionList = ds.Tables[9];
                ViewBag.RequirmentAreaList = ds.Tables[10];
                ViewBag.VehicleNameList = ds.Tables[11];
                ViewBag.GroupNameList = ds.Tables[12];
                ViewBag.SalesExecutiveList = ds.Tables[13];
                ViewBag.portDeatildata = ds.Tables[15];  /*Added By Nitesh 14-12-2023  for Table Data*/
                ViewBag.WastageReason = ds.Tables[16];
                ViewBag.RejectionReason = ds.Tables[17];
                ViewBag.EmployeeSetup = ds.Tables[18];
                ViewBag.GlReporting = ds.Tables[19];
                ViewBag.AssetCategoryList = ds.Tables[20];
                ViewBag.CustomerZone = ds.Tables[21];
                ViewBag.CustomerGroup = ds.Tables[22];
                model.Title = title;


                List<Countrylist> country = new List<Countrylist>();
                foreach (DataRow a in ds.Tables[14].Rows)
                {
                    Countrylist countrylist = new Countrylist();
                    countrylist.country_id = a["country_id"].ToString();
                    countrylist.country_name = a["country_name"].ToString();
                    country.Add(countrylist);
                }
                country.Insert(0, new Countrylist() { country_id = "0", country_name = "---Select---" });
                model.Country_list = country;

                List<SalesRegionlist> _SalesRegionlist = new List<SalesRegionlist>();
                foreach (DataRow a in ds.Tables[9].Rows)
                {
                    SalesRegionlist _SalesRegion = new SalesRegionlist();
                    _SalesRegion.sr_id = a["setup_id"].ToString();
                    _SalesRegion.sr_name = a["setup_val"].ToString();
                    _SalesRegionlist.Add(_SalesRegion);
                }
                _SalesRegionlist.Insert(0, new SalesRegionlist() { sr_id = "0", sr_name = "---Select---" });
                model.SalesRegionlist = _SalesRegionlist;
            }
            catch(Exception ex)
            {
                throw ex;
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                ViewBag.VBRoleList = ds.Tables[3];
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
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
        public ActionResult SaveMISData(MISDetail_Model _MISDetail, string Command)
        {
            try
            {
                string ShopFloor = "N";
                if (_MISDetail.ShopFloor == true)
                {
                    ShopFloor = "Y";
                }
                switch (Command)
                {

                    case "SaveBin":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "1", "BIN", "", _MISDetail.BINNumber, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateBin":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "1", "BIN", _MISDetail.BIN_id, _MISDetail.BINNumber, "", null, "Update");
                        // //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveItemPort":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "2", "ITEM PORTFOLIO", "", _MISDetail.PortfolioName, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateItemPort":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "2", "ITEM PORTFOLIO", _MISDetail.Portfolio_id, _MISDetail.PortfolioName, "", null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveCustPort":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "7", "Customer Portfolio", "", _MISDetail.CustPortfolioName, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateCustPort":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "7", "Customer Portfolio", _MISDetail.CustPortfolio_id, _MISDetail.CustPortfolioName, "", null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveSuppPort":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "6", "Supplier Portfolio", "", _MISDetail.SuppPortfolioName, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateSuppPort":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "6", "Supplier Portfolio", _MISDetail.SuppPortfolio_id, _MISDetail.SuppPortfolioName, "", null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveCustCat":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "8", "Customer Category", "", _MISDetail.CustCategoryName, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateCustCat":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "8", "Customer Category", _MISDetail.CustCategory_id, _MISDetail.CustCategoryName, "", null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveSuppCat":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "4", "Supplier Category", "", _MISDetail.SuppCategoryName, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateSuppCat":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "4", "Supplier Category", _MISDetail.SuppCategory_id, _MISDetail.SuppCategoryName, "", null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveSalesReg":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "10", "Sales Region", "", _MISDetail.SalesRegionName, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateSalesReg":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "10", "Sales Region", _MISDetail.SalesRegion_id, _MISDetail.SalesRegionName, "", null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveReqArea":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "11", "Requirement Area", "", _MISDetail.RequirmentAreaName, ShopFloor, null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateReqArea":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "11", "Requirement Area", _MISDetail.RequirmentArea_id, _MISDetail.RequirmentAreaName, ShopFloor, null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveVehicleName":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "12", "Vehicle Setup", "", _MISDetail.VehicleName, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateVehicleName":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "12", "Vehicle Setup", _MISDetail.VehicleName_id, _MISDetail.VehicleName, "", null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveGroupName":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "13", " Customer Price Setup", "", _MISDetail.GroupName, "", null, "Save");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateGroupName":
                        //Session["BtnNameMIS"] = Command;
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "13", " Customer Price Setup", _MISDetail.GroupName_id, _MISDetail.GroupName, "", null, "Update");
                        //_MISDetail.MessageMIS = "Save";
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");

                    case "SaveSalesExecutive":
                        _MISDetail.BtnNameMIS = Command;
                        SaveSalesExecutiveDetails(_MISDetail, "Save");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateSalesExecutive":
                        _MISDetail.BtnNameMIS = Command;
                        SaveSalesExecutiveDetails(_MISDetail, "Update");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "Saveport": /*Added By Nitesh 14-12-2023  for Save Data*/
                        _MISDetail.BtnNameMIS = Command;
                        if (ModelState.IsValid)
                        {
                            Saveport(_MISDetail, "Save");
                        }
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "updatesaveportdetail":  /*Added By Nitesh 14-12-2023  for Update Data*/
                        _MISDetail.BtnNameMIS = Command;
                        if (ModelState.IsValid)
                        {
                            Saveport(_MISDetail, "Update");
                        }
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveWastageReason":
                        _MISDetail.BtnNameMIS = Command;
                        SaveWastageReasonDetails(_MISDetail, "Save");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");

                    case "UpdateWastageReason":
                        _MISDetail.BtnNameMIS = Command;
                        SaveWastageReasonDetails(_MISDetail, "Update");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveRejectionReason":
                        _MISDetail.BtnNameMIS = Command;
                        SaveRejectionReasonDetails(_MISDetail, "Save");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateRejectionReason":
                        _MISDetail.BtnNameMIS = Command;
                        SaveRejectionReasonDetails(_MISDetail, "Update");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveEmployeeName":
                        _MISDetail.BtnNameMIS = Command;
                        SaveEmployeeSetup(_MISDetail, "Save");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateEmployeeName":
                        _MISDetail.BtnNameMIS = Command;
                        SaveEmployeeSetup(_MISDetail, "Update");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveGLReportingGroup":
                        _MISDetail.BtnNameMIS = Command;
                        SaveGLReport(_MISDetail, "Save");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateGLReportingGroup":
                        _MISDetail.BtnNameMIS = Command;
                        SaveGLReport(_MISDetail, "Update");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveAssestCatgory":
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "14", "AssestCatgory", "", _MISDetail.AssetCategory, "", null, "Save");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "UpdateAssestCatgory":
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "14", "AssestCatgory", _MISDetail.AssetCategory_id, _MISDetail.AssetCategory, "", null, "Update");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS");
                    case "SaveCustomerZone":
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "22", "CustomerZone", "", _MISDetail.CustomerZone, "", null, "Save");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS", new { Setup_Type_id = _MISDetail.setup_type_id });
                    case "UpdateCustomerZone":
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "22", "CustomerZone", _MISDetail.CustomerZone_id, _MISDetail.CustomerZone, "", null, "Update");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS", new { Setup_Type_id = _MISDetail.setup_type_id });
                    case "SaveCustomergroup":
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "23", "Customergroup", "", _MISDetail.Customergroup, "", null, "Save");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS", new { Setup_Type_id = _MISDetail.setup_type_id });
                    case "UpdateCustomergroup":
                        _MISDetail.BtnNameMIS = Command;
                        SaveMISSetupTypeWise(_MISDetail, "23", "Customergroup", _MISDetail.Customergroup_id, _MISDetail.Customergroup, "", null, "Update");
                        TempData["ModelData"] = _MISDetail;
                        return RedirectToAction("MIS", new { Setup_Type_id = _MISDetail.setup_type_id });
                }
                return RedirectToAction("MIS");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult SaveMISSetupTypeWise(MISDetail_Model _MISDetail, string setup_type_id, string setup_type_name, 
            string setup_id, string setup_val, string setup_flag, string ExtraParameter, string Command)
        {
            try
            {
                //MISDetail_Model _MISDetail = new MISDetail_Model();
                string user_id = "";
                string br_id = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    user_id = Session["userid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                string SystemDetail = string.Empty;
                SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                string mac_id = SystemDetail;
                string SaveMessage = _MIS_ISERVICES.SaveMISData(Command, setup_type_id, setup_type_name, setup_id, setup_val, mac_id, CompID, setup_flag, br_id, user_id);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                //Session["setup_id"] = setup_id;
                //Session["setup_val"] = setup_val;
                _MISDetail.setup_id = setup_id;
                _MISDetail.setup_val = setup_val;
                if (Message == "Update" || Message == "Save")
                {
                    //Session["setup_type_id"] = setup_type_id;
                    //Session["MessageMIS"] = "Save";
                    //Session["TransTypeMIS"] = null;
                    if(setup_type_id=="22")
                    {
                        _MISDetail.CustomerZone = "";
                        _MISDetail.CustomerZone_id = "";
                    }
                    else if(setup_type_id == "23")
                    {
                        _MISDetail.Customergroup = "";
                        _MISDetail.Customergroup_id = "";
                    }
                    _MISDetail.AssetCategory = "";
                    _MISDetail.AssetCategory_id = "";
                    _MISDetail.setup_type_id = setup_type_id;
                    _MISDetail.hdnsetuptypeid = setup_type_id;
                    _MISDetail.MessageMIS = "Save";
                    _MISDetail.TransTypeMIS = null;
                }
                if (Message == "Duplicate" + setup_type_name)
                {
                    //Session["TransTypeMIS"] = "Duplicate";
                    //Session["MessageMIS"] = "Duplicate" + setup_type_name;
                    //Session["setup_type_id"] = setup_type_id;
                    //Session["setup_flag"] = setup_flag;

                    _MISDetail.hdnSavebtn = null;
                    _MISDetail.TransTypeMIS = "Duplicate";
                    _MISDetail.MessageMIS = "Duplicate" + setup_type_name;
                    _MISDetail.setup_type_id = setup_type_id;
                    _MISDetail.hdnsetuptypeid = setup_type_id;
                    _MISDetail.setup_flag = setup_flag;
                }
                //TempData["ModelData"] = _MISDetail;
                return RedirectToAction("MIS", new { setup_type_id= setup_type_id });
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private ActionResult SaveRejectionReasonDetails(MISDetail_Model _MISDetail, string action)
        {
            try
            {
                string user_id = "";
                string br_id = "";
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    user_id = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("rej_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));
                if (action == "Save")
                {
                    _MISDetail.Rejection_id = "0";
                }
                JArray jObject = JArray.Parse(_MISDetail.CustomerBranchDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["rej_id"] = _MISDetail.Rejection_id;
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();
                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                string result = _MIS_ISERVICES.SaveRejectionReasonDetails(action, CompID, _MISDetail.Rejection_id, _MISDetail.RejectionReason, dtBranch);
                string ItemCode = result.Substring(result.IndexOf('-') + 1);
                string Message = result.Substring(0, result.IndexOf("-"));

                if (Message == "Updated" || Message == "Saved")
                {
                    _MISDetail.MessageMIS = "Save";
                    _MISDetail.RejectionReason = "";
                    _MISDetail.Rejection_id = "0";
                    _MISDetail.BtnName = "SaveRejectionReason";

                }
                if (Message.Contains("Duplicate"))
                {
                    _MISDetail.hdnSavebtn = null;
                    if (action == "Update")
                    {
                        _MISDetail.BtnName = "UpdateRejectionReason";
                        _MISDetail.Rejection_id = _MISDetail.Rejection_id;
                    }
                    else
                    {
                        _MISDetail.Rejection_id = "0";
                        _MISDetail.BtnName = "SaveRejectionReason";
                    }

                    _MISDetail.MessageMIS = "DuplicateRejectionReason";

                }
               _MISDetail.hdnsetuptypeid = "19";
                _MISDetail.setup_type_id = "19";




                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult SaveGLReport(MISDetail_Model _MISDetail, string action)
        {
            try
            {
                string user_id = "";
                string br_id = "";
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    user_id = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("gl_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));
                if (action == "Save")
                {
                    _MISDetail.GLReport_Id = "0";
                }
                JArray jObject = JArray.Parse(_MISDetail.CustomerBranchDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["gl_id"] = _MISDetail.GLReport_Id;
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();
                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                string result = _MIS_ISERVICES.SaveGLReport(action, CompID, _MISDetail.GLReport_Id, _MISDetail.GLReportingGroup, dtBranch);
                string ItemCode = result.Substring(result.IndexOf('-') + 1);
                string Message = result.Substring(0, result.IndexOf("-"));

                if (Message == "Updated" || Message == "Saved")
                {
                    _MISDetail.MessageMIS = "Save";
                    _MISDetail.GLReportingGroup = "";
                    _MISDetail.GLReport_Id = "0";
                    _MISDetail.BtnName = "SaveGLReportingGroup";

                }
                if (Message.Contains("Duplicate"))
                {
                    _MISDetail.hdnSavebtn = null;
                    if (action == "Update")
                    {
                        _MISDetail.BtnName = "UpdateGLReportingGroup";
                        _MISDetail.GLReport_Id = _MISDetail.GLReport_Id;
                    }
                    else
                    {
                        _MISDetail.GLReport_Id = "0";
                        _MISDetail.BtnName = "SaveGLReportingGroup";
                    }

                    _MISDetail.MessageMIS = "DuplicateGLRptGrp";

                }
                _MISDetail.hdnsetuptypeid = "21";
                _MISDetail.setup_type_id = "21";
                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult SaveWastageReasonDetails(MISDetail_Model _MISDetail, string action)
        {
            try
            {
                string user_id = "";
                string br_id = "";
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    user_id = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("wastage_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));
                if (action == "Save")
                {
                    _MISDetail.Wstg_id = "0";
                }
                JArray jObject = JArray.Parse(_MISDetail.CustomerBranchDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["wastage_id"] = _MISDetail.Wstg_id;
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();
                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                string result = _MIS_ISERVICES.SaveWastageReasonDetails(action, CompID, _MISDetail.Wstg_id, _MISDetail.WastageReason, dtBranch);
                string ItemCode = result.Substring(result.IndexOf('-') + 1);
                string Message = result.Substring(0, result.IndexOf("-"));

                if (Message == "Updated" || Message == "Saved")
                {
                    _MISDetail.MessageMIS = "Save";
                    _MISDetail.WastageReason = "";
                    _MISDetail.Wstg_id = "0";
                    _MISDetail.BtnName = "SaveWastageReason";

                }
                if (Message.Contains("Duplicate"))
                {
                    _MISDetail.hdnSavebtn = null;
                    if (action == "Update")
                    {
                        _MISDetail.BtnName = "UpdateWastageReason";
                        _MISDetail.Wstg_id = _MISDetail.Wstg_id;
                    }
                    else
                    {
                        _MISDetail.Wstg_id = "0";
                        _MISDetail.BtnName = "SaveWastageReason";
                    }

                    _MISDetail.MessageMIS = "Duplicate_wastage";

                }
                _MISDetail.hdnsetuptypeid = "18";
                _MISDetail.setup_type_id = "18";




                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult SaveSalesExecutiveDetails(MISDetail_Model _MISDetail, string action)
        {
            try
            {
                string user_id = "";
                string br_id = "";
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    user_id = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("sls_pers_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));
                if (action == "Save")
                {
                    _MISDetail.SeId = "0";
                }
                JArray jObject = JArray.Parse(_MISDetail.CustomerBranchDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["sls_pers_id"] = _MISDetail.SeId;
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();
                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                string result = _MIS_ISERVICES.SaveSalesExecutiveData(action, CompID, _MISDetail.SeId, _MISDetail.SalesExecutiveName, _MISDetail.SEContactNo, _MISDetail.SEEmailId, dtBranch, _MISDetail.SalesRegion);
                string ItemCode = result.Substring(result.IndexOf('-') + 1);
                string Message = result.Substring(0, result.IndexOf("-"));

                if (Message == "Updated" || Message == "Saved")
                {
                    _MISDetail.MessageMIS = "Save";
                    _MISDetail.SalesExecutiveName = "";
                    _MISDetail.SEEmailId = "";
                    _MISDetail.SEContactNo = "";
                    _MISDetail.SalesRegion = "0";
                }
                if (Message.Contains("Duplicate"))
                {
                    _MISDetail.hdnSavebtn = null;
                    _MISDetail.MessageMIS = "Duplicate";
                }

                _MISDetail.hdnsetuptypeid = "15";
                _MISDetail.setup_type_id = "15";
                _MISDetail.SeId = "0";
                _MISDetail.BtnName = "SaveSalesExecutive";


                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private ActionResult SaveEmployeeSetup(MISDetail_Model _MISDetail, string action)
        {
            try
            {
                string user_id = "";
                string br_id = "";
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    user_id = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                DataTable dtBranch = new DataTable();
                dtBranch.Columns.Add("comp_id", typeof(Int32));
                dtBranch.Columns.Add("br_id", typeof(Int32));
                dtBranch.Columns.Add("emp_id", typeof(Int32));
                dtBranch.Columns.Add("act_status", typeof(string));
                if (action == "Save")
                {
                    _MISDetail.EmployeeNameID = "0";
                }
                JArray jObject = JArray.Parse(_MISDetail.CustomerBranchDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowBrdetails = dtBranch.NewRow();
                    dtrowBrdetails["comp_id"] = Session["CompId"].ToString();
                    dtrowBrdetails["br_id"] = jObject[i]["Id"].ToString();
                    dtrowBrdetails["emp_id"] = Convert.ToInt32(_MISDetail.EmployeeNameID);
                    dtrowBrdetails["act_status"] = jObject[i]["BranchFlag"].ToString();
                    dtBranch.Rows.Add(dtrowBrdetails);
                }
                string result = _MIS_ISERVICES.SaveEmployeeData(action, CompID, _MISDetail.EmployeeNameID, _MISDetail.EmployeeName, _MISDetail.Emp_ContactNo, _MISDetail.Emp_EmailId, dtBranch);
                string ItemCode = result.Substring(result.IndexOf('-') + 1);
                string Message = result.Substring(0, result.IndexOf("-"));

                if (Message == "Updated" || Message == "Saved")
                {
                    _MISDetail.MessageMIS = "Save";
                    _MISDetail.EmployeeName = "";
                    _MISDetail.Emp_ContactNo = "";
                    _MISDetail.Emp_EmailId = "";
                }
                if (Message.Contains("Duplicate"))
                {
                    _MISDetail.hdnSavebtn = null;
                    if (action == "Update")
                    {
                        _MISDetail.BtnName = "UpdateEmployeeName";
                        _MISDetail.Wstg_id = _MISDetail.Wstg_id;
                    }
                    else
                    {
                        _MISDetail.Wstg_id = "0";
                        _MISDetail.BtnName = "SaveEmployeeName";
                    }

                    _MISDetail.MessageMIS = "Duplicate_Employee";

                }
                _MISDetail.hdnsetuptypeid = "20";
                _MISDetail.setup_type_id = "20";



                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private ActionResult Saveport(MISDetail_Model _MISDetail, string Command)   /*Added By Nitesh 14-12-2023  for save and Update Data*/
        {
            try
            {
                string result = _MIS_ISERVICES.Saveportdata(_MISDetail.Country, _MISDetail.portid, _MISDetail.PortDescription, _MISDetail.PinCode, _MISDetail.state, Command, _MISDetail.hdnprot_id, _MISDetail.Porttype);
                string Message = result.Substring(result.IndexOf('-') + 1);

                if (Message == "Update" || Message == "Save")
                {
                    _MISDetail.MessageMIS = "Save";
                    _MISDetail.Country = "0";
                    _MISDetail.portid = null;
                    _MISDetail.PortDescription = null;
                    _MISDetail.PinCode = null;
                    _MISDetail.state = "0";
                    _MISDetail.Porttype = "0";
                }
                if (Message.Contains("Duplicate"))
                {
                    _MISDetail.hdnSavebtn = null;
                    _MISDetail.MessageMIS = "Duplicate_PORTID";
                }
                _MISDetail.hdnsetuptypeid = "17";
                _MISDetail.setup_type_id = "17";
                _MISDetail.SeId = "0";
                _MISDetail.BtnName = "Saveport";


                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult DeleteMISDetail(string setup_type_id, string setup_id, string setup_val)
        {
            try
            {
                MISDetail_Model _MISDetail = new MISDetail_Model();
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                if (setup_type_id != "" && setup_id != "" && setup_val != "")
                {
                    string SaveMessage = _MIS_ISERVICES.DeleteMISData(setup_type_id, setup_id, setup_val, CompID, br_id);
                    string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Deleted")
                    {
                        //Session["setup_type_id"] = setup_type_id;
                        //Session["MessageMIS"] = "Deleted";
                        _MISDetail.hdnsetuptypeid = setup_type_id;
                        _MISDetail.setup_type_id = setup_type_id;
                        _MISDetail.MessageMIS = "Deleted";
                    }
                    if (Message == "Used")
                    {
                        //Session["MessageMIS"] = "Used";
                        _MISDetail.hdnsetuptypeid = setup_type_id;
                        _MISDetail.setup_type_id = setup_type_id;
                        _MISDetail.MessageMIS = "Used";
                        _MISDetail.hdnSavebtn = null;
                    }
                }
                TempData["ModelData"] = _MISDetail;
                return RedirectToAction("MIS",new { setup_type_id });
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult DeleteSeDetail(string seId)
        {
            try
            {
                MISDetail_Model _MISDetail = new MISDetail_Model();
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                string SaveMessage = _MIS_ISERVICES.DeleteSeDetail(CompID, br_id, seId);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "Deleted")
                {
                    _MISDetail.SeId = seId;
                    _MISDetail.MessageMIS = "Deleted";
                }
                if (Message == "DE")
                {
                    //Session["MessageMIS"] = "Used";
                    _MISDetail.MessageMIS = "Used";
                }
                _MISDetail.hdnsetuptypeid = "15";
                _MISDetail.setup_type_id = "15";
              //  string Setup_Type_id = "15";
                TempData["ModelData"] = _MISDetail;
                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult DeleteWastageDetail(string wastage_id)
        {
            try
            {
                MISDetail_Model _MISDetail = new MISDetail_Model();
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                string SaveMessage = _MIS_ISERVICES.DeleteWastageDetail(CompID, br_id, wastage_id);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "Deleted")
                {
                    _MISDetail.Wstg_id = wastage_id;
                    _MISDetail.MessageMIS = "Deleted";
                }

                _MISDetail.hdnsetuptypeid = "18";
                _MISDetail.setup_type_id = "18";
                // string Setup_Type_id = "15";
                TempData["ModelData"] = _MISDetail;
                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult DeleteRejectionDetail(string Rejection_id)
        {
            try
            {
                MISDetail_Model _MISDetail = new MISDetail_Model();
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                string SaveMessage = _MIS_ISERVICES.DeleteRejectionDetail(CompID, br_id, Rejection_id);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "Deleted")
                {
                    _MISDetail.Rejection_id = Rejection_id;
                    _MISDetail.MessageMIS = "Deleted";
                }
                else if (Message == "DulicateRejection")
                {

                    _MISDetail.MessageMIS = "Used";
                }
                _MISDetail.hdnsetuptypeid = "19";
                _MISDetail.setup_type_id = "19";

                TempData["ModelData"] = _MISDetail;
                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult DeleteEmployeeSetup(string Emp_id)
        {
            try
            {
                MISDetail_Model _MISDetail = new MISDetail_Model();
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                string SaveMessage = _MIS_ISERVICES.DeleteEmployeeSetup(CompID, br_id, Emp_id);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "Deleted")
                {
                    _MISDetail.EmployeeNameID = Emp_id;
                    _MISDetail.MessageMIS = "Deleted";
                }
                else if (Message == "DulicateEmployee")
                {

                    _MISDetail.MessageMIS = "Used";
                }
                _MISDetail.hdnsetuptypeid = "20";
                _MISDetail.setup_type_id = "20";

                TempData["ModelData"] = _MISDetail;
                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult DeleteGl_rptgrp(string Gl_id)
        {
            try
            {
                MISDetail_Model _MISDetail = new MISDetail_Model();
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                string SaveMessage = _MIS_ISERVICES.DeleteGLrpt_grp(CompID, br_id, Gl_id);
                string ItemCode = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                if (Message == "Deleted")
                {
                    _MISDetail.GLReport_Id = Gl_id;
                    _MISDetail.MessageMIS = "Deleted";
                }
                else if (Message == "DulicateGL")
                {

                    _MISDetail.MessageMIS = "Used";
                }
                _MISDetail.hdnsetuptypeid = "21";
                _MISDetail.setup_type_id = "21";

                TempData["ModelData"] = _MISDetail;
                return RedirectToAction("MIS", _MISDetail);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public JsonResult CheckSeBranchStatus(string branchId, string seId)
        {
            try
            {

                string compId = "0";
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                DataTable dt = _MIS_ISERVICES.checkSalesExecutiveBranchStatus(compId, branchId, seId);
                return Json(JsonConvert.SerializeObject(dt));
            }
            catch
            {
                return Json("Error");
            }
        }
        public JsonResult CheckBranchStatus(string branchId, string Doc_id, string flag)
        {
            try
            {

                string compId = "0";
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                DataTable dt = _MIS_ISERVICES.checkBranchStatus(compId, branchId, Doc_id, flag);
                return Json(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult getstatelist(MISDetail_Model _MISDetail) /*Added By Nitesh 14-12-2023  for bind Dropdown state Data*/
        {
            try
            {
                string hdnstate_id = string.Empty;
                //MISDetail_Model _MISDetail = new MISDetail_Model();
                var countryid = _MISDetail.countryid;
                if (_MISDetail.ddlstate != null)
                {
                    hdnstate_id = _MISDetail.ddlstate;
                }
                else
                {
                    hdnstate_id = "";
                }

                DataTable dt = _MIS_ISERVICES.getstatelist(countryid, hdnstate_id);

                List<statelist> statelist = new List<statelist>();
                foreach (DataRow a in dt.Rows)
                {
                    statelist state_list = new statelist();
                    state_list.state_id = a["state_id"].ToString();
                    state_list.state_name = a["state_name"].ToString();
                    statelist.Add(state_list);
                }
                statelist.Insert(0, new statelist() { state_id = "0", state_name = "---Select---" });
                _MISDetail.state_list = statelist;
                //return(_MISDetail);
                return Json(statelist.Select(c => new { ID = c.state_id, Name = c.state_name }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        /*Added this Function  By Nitesh 14-12-2023  for Delete Port Setup Data
        
         */
        public ActionResult DeletePortDetail(string Portid)
        {
            try
            {
                MISDetail_Model _MISDetail = new MISDetail_Model();
                string DeleteMassage = _MIS_ISERVICES.DeletePortDetail(Portid);

                string Message = DeleteMassage.Substring(DeleteMassage.IndexOf('-') + 1);


                if (Message == "Delete")
                {
                    _MISDetail.MessageMIS = "Deleted";
                }
                //if (Message == "DE")
                //{
                //    //Session["MessageMIS"] = "Used";
                //    _MISDetail.MessageMIS = "Used";
                //}
                _MISDetail.hdnsetuptypeid = "17";
                _MISDetail.setup_type_id = "17";
                TempData["ModelData"] = _MISDetail;
                return RedirectToAction("MIS");


            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /*Commented By Nitesh 20-12-2023 Because Check Dependcy on Page Loas Method in Controller is use In Port Setup*/
        //public JsonResult checkdependcyport(string tblport_id)
        //{
        //    try
        //    {

        //        string compId = "0";
        //        string br_id = "0";
        //        if (Session["CompId"] != null)
        //            CompID = Session["CompId"].ToString();
        //        if (Session["BranchId"] != null)
        //            br_id = Session["BranchId"].ToString();
        //        DataTable dt = _MIS_ISERVICES.checkdependcyport(CompID, br_id, tblport_id);
        //        return Json(JsonConvert.SerializeObject(dt));
        //    }
        //    catch
        //    {
        //        return Json("Error");
        //    }
        //}
    }
}