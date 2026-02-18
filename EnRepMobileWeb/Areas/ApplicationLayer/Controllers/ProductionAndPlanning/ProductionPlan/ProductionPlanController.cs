using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionPlan;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionPlan;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using System.Linq;
using EnRepMobileWeb.MODELS.Common;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.ProductionPlan
{
    public class ProductionPlanController : Controller
    {
        string CompID, language, BrchID, userid, title = string.Empty;
        string DocumentMenuId = "105105116";
        string FromDate;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ProductionPlan_ISERVICES _PP_ISERVICES;
        public ProductionPlanController(Common_IServices _Common_IServices, ProductionPlan_ISERVICES _PP_ISERVICES)
        {
            this._PP_ISERVICES = _PP_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/ProductionPlan
        //ProductionPlan_Model _PP_Model = new ProductionPlan_Model();
        public ActionResult ProductionPlan(string WF_Status)//List page load
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
                ProductionPlan_Model _ProductionPlan_Model = new ProductionPlan_Model();
                _ProductionPlan_Model.WF_Status = WF_Status;
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                //string endDate = dtnow.ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                GetStatusList(_ProductionPlan_Model);

                List<sourcetype> fflist = new List<sourcetype>();
                fflist.Add(new sourcetype() { id = "0", name = "ALL" });
                fflist.Add(new sourcetype() { id = "D", name = "Direct (Periodic)" });
                fflist.Add(new sourcetype() { id = "A", name = "Direct (Ad-Hoc)" });
                fflist.Add(new sourcetype() { id = "F", name = "Sales Forecast" });
                fflist.Add(new sourcetype() { id = "O", name = "Sales Order" });
                _ProductionPlan_Model.ddl_src_typeList = fflist;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var Source = a[0].Trim();
                    var Fromdate = a[1].Trim();
                    var Todate = a[2].Trim();
                    var Status = a[3].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    _ProductionPlan_Model.txtFromDate = _ProductionPlan_Model.txtFromDate;
                    _ProductionPlan_Model.ListFilterData = TempData["ListFilterData"].ToString();
                    List<PP_List> _PP_List = new List<PP_List>();
                    DataTable dt = new DataTable();
                    dt = _PP_ISERVICES.GetPPList(Source, Fromdate, Todate, Status, CompID, BrchID, "", "0", "").Tables[0];
                    _ProductionPlan_Model.PPSearch = "PP_Search";
                    _ProductionPlan_Model.ddl_src_type = Source;
                    _ProductionPlan_Model.txtFromDate = Fromdate;
                    _ProductionPlan_Model.txtToDate = Todate;
                    _ProductionPlan_Model.Status = Status;

                    foreach (DataRow dr in dt.Rows)
                    {
                        PP_List _PPList = new PP_List();
                        _PPList.PP_no = dr["pp_no"].ToString();
                        _PPList.PP_date = dr["pp_dt"].ToString();
                        _PPList.source = dr["src_type"].ToString();
                        _PPList.fy = dr["fy"].ToString();
                        _PPList.period = dr["f_periodval"].ToString();
                        _PPList.daterange = dr["daterange"].ToString();
                        _PPList.status = dr["status_name"].ToString();
                        _PPList.createon = dr["create_dt"].ToString();
                        _PPList.approvedon = dr["app_dt"].ToString();
                        _PPList.amendedon = dr["mod_dt"].ToString();
                        _PPList.create_by = dr["create_by"].ToString();
                        _PPList.app_by = dr["app_by"].ToString();
                        _PPList.mod_by = dr["mod_by"].ToString();
                        _PP_List.Add(_PPList);
                    }
                    _ProductionPlan_Model.PP_ListDetail = _PP_List;
                }
                else
                {
                    //_ProductionPlan_Model.txtToDate = endDate;
                    //if (FromDate != null)
                    //{
                    //    _ProductionPlan_Model.txtFromDate = FromDate;
                    //}
                    _ProductionPlan_Model.txtFromDate = startDate;
                    _ProductionPlan_Model.txtToDate = CurrentDate;
                    _ProductionPlan_Model.PP_ListDetail = getPPlistDetails(_ProductionPlan_Model);
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                _ProductionPlan_Model.PPSearch = "0";
                ViewBag.MenuPageName = getDocumentName();
                ViewBag.VBRoleList = GetRoleList();
                _ProductionPlan_Model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanList.cshtml", _ProductionPlan_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AddProductionPlanDetail()
        {
            try
            {
                ProductionPlan_Model AddNewModel = new ProductionPlan_Model();
                AddNewModel.Commandfc = "Add";
                AddNewModel.TransTypefc = "Save";
                AddNewModel.BtnNamefc = "BtnAddNew";
                //AddNewModel.DocumentStatus = "D";
                TempData["ModelData"] = AddNewModel;
                UrlModel AddNew_Model = new UrlModel();
                AddNew_Model.bt = "BtnAddNew";
                AddNew_Model.Cmd = "Add";
                AddNew_Model.tp = "Save";
                ViewBag.MenuPageName = getDocumentName();
                TempData["ListFilterData"] = null;
                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                {
                    TempData["Message"] = "Financial Year not Exist";
                    return RedirectToAction("ProductionPlan");
                }
                /*End to chk Financial year exist or not*/
                return RedirectToAction("ProductionPlanDetail", "ProductionPlan", AddNew_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ProductionPlanDetail(UrlModel _urlModel)//Add button page load
        {
            try
            {
                ProductionPlan_Model _ProductionPlan_Model = new ProductionPlan_Model();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                string UserID = "";
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                /*Add by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.PPL_Date) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _PP_Model = TempData["ModelData"] as ProductionPlan_Model;
                if (_PP_Model != null)
                {
                    BindDDLOnPageLoad(_PP_Model);
                    ViewBag.MenuPageName = getDocumentName();
                    _PP_Model.Title = title;
                    ViewBag.VBRoleList = GetRoleList();

                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    /*-------------------------- For Production Plan Against Sales order-------------------*/
                    List<CustName> _CustList = new List<CustName>();
                    _CustList.Insert(0, new CustName() { Cust_id = "0", Cust_name = "---Select---" });
                    _PP_Model.CustNameList = _CustList;

                    List<OrderList> _OrderList = new List<OrderList>();
                    _OrderList.Insert(0, new OrderList() { Order_id = "0", Order_val = "---Select---" });
                    _PP_Model.OrderNumberList = _OrderList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _PP_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    DataTable dt = new DataTable();
                    List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                    dt = GetRequirmentreaList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        RequirementAreaList _RAList = new RequirementAreaList();
                        _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                        _RAList.req_val = dr["setup_val"].ToString();
                        requirementAreaLists.Add(_RAList);
                    }
                    requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = " ---Select---" });
                    _PP_Model._requirementAreaLists = requirementAreaLists;

                    //_PP_Model._requirementAreaLists =  GetRequirmentreaList(); //Commented By Suraj on 25-01-2023
                    /*-------------------------- For Production Plan Against Sales order End-------------------*/
                    if (_PP_Model.TransTypefc == "Update")
                    {
                        string PP_no = _PP_Model.PPNumberfc;
                        DataSet ds = _PP_ISERVICES.GetPPDetailByNo(CompID, PP_no, BrchID, UserID, DocumentMenuId);
                        _PP_Model.ddl_src_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        _PP_Model.PPNumber = ds.Tables[0].Rows[0]["pp_no"].ToString();
                        _PP_Model.PPDate = ds.Tables[0].Rows[0]["pp_dt"].ToString();
                        if (_PP_Model.ddl_src_type == "A")
                        {
                            _PP_Model.ADHocFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                            _PP_Model.ADHocToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        }
                        else
                        {
                            _PP_Model.txtFromDate = ds.Tables[0].Rows[0]["fromdate"].ToString();
                            _PP_Model.txtToDate = ds.Tables[0].Rows[0]["todate"].ToString();
                        }

                        _PP_Model.hfFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                        _PP_Model.hfToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        _PP_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _PP_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _PP_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _PP_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _PP_Model.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _PP_Model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _PP_Model.Status = ds.Tables[0].Rows[0]["status_name"].ToString();
                        _PP_Model.ForAmmendendBtn = ds.Tables[8].Rows[0]["flag"].ToString();
                        _PP_Model.Amendment = ds.Tables[9].Rows[0]["Amendment"].ToString();
                        _PP_Model.ForDeleteBtn = ds.Tables[9].Rows[0]["ForDeleteBtn"].ToString();
                        _PP_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _PP_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        string StatusCode = ds.Tables[0].Rows[0]["pp_status"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString().Trim();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString().Trim();
                        //_PP_Model.req_area = Convert.ToInt32(ds.Tables[0].Rows[0]["req_area"].ToString());
                        _PP_Model.req_area = (ds.Tables[0].Rows[0]["req_area"].ToString());
                        if (_PP_Model.ddl_src_type == "O")
                        {
                            _PP_Model.HdnOrderNumber = ds.Tables[5].Rows[0]["so_no"].ToString();
                        }

                        ViewBag.SoItemDetails = ds.Tables[5];
                        //ViewBag.AdvDetails = ds.Tables[6];
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            if (ds.Tables[6].Rows[0]["jc_no"].ToString() == "Amnd" && ds.Tables[6].Rows[0]["jc_no"].ToString() != "")
                            {
                                ViewBag.ProdOrderDetails = null;
                            }
                            else
                            {
                                ViewBag.ProdOrderDetails = ds.Tables[6];
                            }
                        }
                        else
                        {
                            ViewBag.ProdOrderDetails = null;
                        }
                        ViewBag.SubItemDetails = ds.Tables[7];
                        ViewBag.SubItemDetails_Percure = ds.Tables[10];
                        ViewBag.PRDetails = ds.Tables[11];
                        _PP_Model.StatusCode = StatusCode;
                        _PP_Model.create_id = create_id;
                        // _PP_Model.StatusCode = StatusCode;
                        if (StatusCode == "C")
                        {
                            _PP_Model.CancelFlag = true;
                        }
                        else
                        {
                            _PP_Model.CancelFlag = false;
                        }
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Commandfc"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _PP_Model.Commandfc != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _PP_Model.BtnNamefc = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _PP_Model.BtnNamefc = "BtnEdit";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _PP_Model.BtnNamefc = "BtnEdit";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _PP_Model.BtnNamefc = "BtnEdit";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _PP_Model.BtnNamefc = "BtnEdit";
                                    }


                                }
                            }
                            if (StatusCode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _PP_Model.BtnNamefc = "BtnEdit";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _PP_Model.BtnNamefc = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {

                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _PP_Model.BtnNamefc = "BtnEdit";

                                }
                                else
                                {
                                    _PP_Model.BtnNamefc = "BtnRefresh";
                                }
                            }
                            if (_PP_Model.Status == "Forwarded")
                            {
                                if (UserID == sent_to)
                                {
                                    _PP_Model.BtnNamefc = "BtnEdit";
                                }
                                else
                                {
                                    _PP_Model.BtnNamefc = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        List<financial_year> fyList = new List<financial_year>();
                        financial_year fyObj1 = new financial_year();
                        fyObj1.id = ds.Tables[0].Rows[0]["f_fy"].ToString();
                        fyObj1.name = ds.Tables[0].Rows[0]["f_fyval"].ToString();
                        fyList.Add(fyObj1);
                        _PP_Model.ddl_financial_yearList = fyList;

                        List<period> plist = new List<period>();
                        period pObj = new period();
                        pObj.id = ds.Tables[0].Rows[0]["f_period"].ToString();
                        pObj.name = ds.Tables[0].Rows[0]["f_periodval"].ToString();
                        plist.Add(pObj);
                        _PP_Model.ddl_periodList = plist;
                        _PP_Model.UserId = UserID;
                        ViewBag.ProductDetails = ds.Tables[1];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["pp_status"].ToString().Trim();
                        _PP_Model.DocumentStatusfc = ds.Tables[0].Rows[0]["status_name"].ToString();

                        if (_PP_Model.Amend != null)
                        {

                            if (_PP_Model.Amend == "Amend")
                            {
                                ViewBag.DocumentCode = "D";
                                _PP_Model.StatusCode = "D";
                                _PP_Model.Amend = "Amend";
                            }
                        }
                        if (_PP_Model.Amendment != "Amendment" && _PP_Model.Amendment != "" && _PP_Model.Amendment != null)
                        {

                            _PP_Model.BtnNamefc = "BtnRefresh";
                            _PP_Model.wfDisableAmnd = "wfDisableAmnd";
                        }
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanDetail.cshtml", _PP_Model);
                    }
                    else
                    {
                        _PP_Model.UserId = UserID;
                        _PP_Model.PPDate = DateTime.Now.ToString("yyyy-MM-dd");

                        _PP_Model.DocumentStatusfc = "";

                        if (_PP_Model.Amend != null)
                        {
                            if (_PP_Model.Amend == "Amend")
                            {
                                ViewBag.DocumentCode = "D";
                                _PP_Model.StatusCode = "D";
                                _PP_Model.Amend = "Amend";
                            }
                        }
                        if (_PP_Model.Amendment != "Amendment" && _PP_Model.Amendment != "" && _PP_Model.Amendment != null)
                        {
                            _PP_Model.BtnNamefc = "BtnRefresh";
                            _PP_Model.wfDisableAmnd = "wfDisableAmnd";
                        }
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanDetail.cshtml", _PP_Model);
                    }
                }
                else
                {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    if (_urlModel != null)
                    {
                        _ProductionPlan_Model.BtnNamefc = _urlModel.bt;
                        _ProductionPlan_Model.PPNumberfc = _urlModel.MRP_No;
                        _ProductionPlan_Model.PPDatefc = _urlModel.PPL_Date;
                        _ProductionPlan_Model.Commandfc = _urlModel.Cmd;
                        _ProductionPlan_Model.TransTypefc = _urlModel.tp;
                        _ProductionPlan_Model.WF_Status1 = _urlModel.wf;
                        _ProductionPlan_Model.Amend = _urlModel.Amend;
                        //_ProductionPlan_Model.DocumentStatus = _urlModel.DMS;
                    }
                    BindDDLOnPageLoad(_ProductionPlan_Model);
                    ViewBag.MenuPageName = getDocumentName();
                    _ProductionPlan_Model.Title = title;
                    ViewBag.VBRoleList = GetRoleList();

                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    /*-------------------------- For Production Plan Against Sales order-------------------*/
                    List<CustName> _CustList = new List<CustName>();
                    _CustList.Insert(0, new CustName() { Cust_id = "0", Cust_name = "---Select---" });
                    _ProductionPlan_Model.CustNameList = _CustList;

                    List<OrderList> _OrderList = new List<OrderList>();
                    _OrderList.Insert(0, new OrderList() { Order_id = "0", Order_val = "---Select---" });
                    _ProductionPlan_Model.OrderNumberList = _OrderList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ProductionPlan_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    DataTable dt = new DataTable();
                    List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                    dt = GetRequirmentreaList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        RequirementAreaList _RAList = new RequirementAreaList();
                        _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                        _RAList.req_val = dr["setup_val"].ToString();
                        requirementAreaLists.Add(_RAList);
                    }
                    requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = " ---Select---" });
                    _ProductionPlan_Model._requirementAreaLists = requirementAreaLists;

                    //_ProductionPlan_Model._requirementAreaLists =  GetRequirmentreaList(); //Commented By Suraj on 25-01-2023
                    /*-------------------------- For Production Plan Against Sales order End-------------------*/
                    if (_ProductionPlan_Model.TransTypefc == "Update")
                    {
                        string PP_no = _ProductionPlan_Model.PPNumberfc;
                        DataSet ds = _PP_ISERVICES.GetPPDetailByNo(CompID, PP_no, BrchID, UserID, DocumentMenuId);
                        _ProductionPlan_Model.ddl_src_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        _ProductionPlan_Model.PPNumber = ds.Tables[0].Rows[0]["pp_no"].ToString();
                        _ProductionPlan_Model.PPDate = ds.Tables[0].Rows[0]["pp_dt"].ToString();
                        if (_ProductionPlan_Model.ddl_src_type == "A")
                        {
                            _ProductionPlan_Model.ADHocFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                            _ProductionPlan_Model.ADHocToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        }
                        else
                        {
                            _ProductionPlan_Model.txtFromDate = ds.Tables[0].Rows[0]["fromdate"].ToString();
                            _ProductionPlan_Model.txtToDate = ds.Tables[0].Rows[0]["todate"].ToString();
                        }

                        _ProductionPlan_Model.hfFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                        _ProductionPlan_Model.hfToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        _ProductionPlan_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _ProductionPlan_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _ProductionPlan_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _ProductionPlan_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _ProductionPlan_Model.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _ProductionPlan_Model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _ProductionPlan_Model.Status = ds.Tables[0].Rows[0]["status_name"].ToString();
                        _ProductionPlan_Model.ForAmmendendBtn = ds.Tables[8].Rows[0]["flag"].ToString();
                        _ProductionPlan_Model.Amendment = ds.Tables[9].Rows[0]["Amendment"].ToString();
                        _ProductionPlan_Model.ForDeleteBtn = ds.Tables[9].Rows[0]["ForDeleteBtn"].ToString();
                        _ProductionPlan_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _ProductionPlan_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        string StatusCode = ds.Tables[0].Rows[0]["pp_status"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString().Trim();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString().Trim();
                        //_ProductionPlan_Model.req_area = Convert.ToInt32(ds.Tables[0].Rows[0]["req_area"].ToString());
                        _ProductionPlan_Model.req_area = ds.Tables[0].Rows[0]["req_area"].ToString();
                        if (_ProductionPlan_Model.ddl_src_type == "O")
                        {
                            _ProductionPlan_Model.HdnOrderNumber = ds.Tables[5].Rows[0]["so_no"].ToString();
                        }

                        ViewBag.SoItemDetails = ds.Tables[5];
                        //ViewBag.AdvDetails = ds.Tables[6];
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            if (ds.Tables[6].Rows[0]["jc_no"].ToString() == "Amnd" && ds.Tables[6].Rows[0]["jc_no"].ToString() != "")
                            {
                                ViewBag.ProdOrderDetails = null;
                            }
                            else
                            {
                                ViewBag.ProdOrderDetails = ds.Tables[6];
                            }
                        }
                        else
                        {
                            ViewBag.ProdOrderDetails = null;
                        }
                        ViewBag.SubItemDetails = ds.Tables[7];
                        ViewBag.SubItemDetails_Percure = ds.Tables[10];
                        ViewBag.PRDetails = ds.Tables[11];
                        _ProductionPlan_Model.StatusCode = StatusCode;
                        _ProductionPlan_Model.create_id = create_id;
                        // _ProductionPlan_Model.StatusCode = StatusCode;
                        if (StatusCode == "C")
                        {
                            _ProductionPlan_Model.CancelFlag = true;
                        }
                        else
                        {
                            _ProductionPlan_Model.CancelFlag = false;
                        }
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        if (ViewBag.AppLevel != null && _ProductionPlan_Model.Commandfc != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {

                                    _ProductionPlan_Model.BtnNamefc = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }

                                        _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/

                                        _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                                    }


                                }
                            }
                            if (StatusCode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }

                                    _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {

                                if (create_id == UserID || approval_id == UserID)
                                {

                                    _ProductionPlan_Model.BtnNamefc = "BtnEdit";

                                }
                                else
                                {

                                    _ProductionPlan_Model.BtnNamefc = "BtnRefresh";
                                }
                            }
                            if (_ProductionPlan_Model.Status == "Forwarded")
                            {
                                if (UserID == sent_to)
                                {

                                    _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                                }
                                else
                                {

                                    _ProductionPlan_Model.BtnNamefc = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        List<financial_year> fyList = new List<financial_year>();
                        financial_year fyObj1 = new financial_year();
                        fyObj1.id = ds.Tables[0].Rows[0]["f_fy"].ToString();
                        fyObj1.name = ds.Tables[0].Rows[0]["f_fyval"].ToString();
                        fyList.Add(fyObj1);
                        _ProductionPlan_Model.ddl_financial_yearList = fyList;

                        List<period> plist = new List<period>();
                        period pObj = new period();
                        pObj.id = ds.Tables[0].Rows[0]["f_period"].ToString();
                        pObj.name = ds.Tables[0].Rows[0]["f_periodval"].ToString();
                        plist.Add(pObj);
                        _ProductionPlan_Model.ddl_periodList = plist;
                        _ProductionPlan_Model.UserId = UserID;
                        ViewBag.ProductDetails = ds.Tables[1];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["pp_status"].ToString().Trim();

                        _ProductionPlan_Model.DocumentStatusfc = ds.Tables[0].Rows[0]["status_name"].ToString();

                        if (_ProductionPlan_Model.Amend != null)
                        {

                            if (_ProductionPlan_Model.Amend == "Amend")
                            {
                                ViewBag.DocumentCode = "D";
                                _ProductionPlan_Model.StatusCode = "D";
                                _ProductionPlan_Model.Amend = "Amend";
                            }
                        }
                        if (_ProductionPlan_Model.Amendment != "Amendment" && _ProductionPlan_Model.Amendment != "" && _ProductionPlan_Model.Amendment != null)
                        {

                            _ProductionPlan_Model.BtnNamefc = "BtnRefresh";
                            _ProductionPlan_Model.wfDisableAmnd = "wfDisableAmnd";
                        }
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanDetail.cshtml", _ProductionPlan_Model);
                    }
                    else
                    {
                        _ProductionPlan_Model.UserId = UserID;
                        _ProductionPlan_Model.PPDate = DateTime.Now.ToString("yyyy-MM-dd");

                        _ProductionPlan_Model.DocumentStatusfc = "";

                        if (_ProductionPlan_Model.Amend != null)
                        {
                            if (_ProductionPlan_Model.Amend == "Amend")
                            {
                                ViewBag.DocumentCode = "D";
                                _ProductionPlan_Model.StatusCode = "D";
                                _ProductionPlan_Model.Amend = "Amend";
                            }
                        }
                        if (_ProductionPlan_Model.Amendment != "Amendment" && _ProductionPlan_Model.Amendment != "" && _ProductionPlan_Model.Amendment != null)
                        {

                            _ProductionPlan_Model.BtnNamefc = "BtnRefresh";
                            _ProductionPlan_Model.wfDisableAmnd = "wfDisableAmnd";
                        }
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanDetail.cshtml", _ProductionPlan_Model);
                    }
                }

                //}
                //else
                //{
                //    _ProductionPlan_Model.UserId = UserID;
                //    _ProductionPlan_Model.PPDate = DateTime.Now.ToString("yyyy-MM-dd");
                //    Session["DocumentStatusfc"] = "";
                //    if (Session["Amend"] != null)
                //    {
                //        if (Session["Amend"].ToString() == "Amend")
                //        {
                //            ViewBag.DocumentCode = "D";
                //            _ProductionPlan_Model.StatusCode = "D";
                //            _ProductionPlan_Model.Amend = "Amend";
                //        }
                //    }
                //    if (_ProductionPlan_Model.Amendment != "Amendment" && _ProductionPlan_Model.Amendment != "" && _ProductionPlan_Model.Amendment != null)
                //    {
                //        Session["BtnNamefc"] = "BtnRefresh";
                //        _ProductionPlan_Model.wfDisableAmnd = "wfDisableAmnd";
                //    }
                //    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanDetail.cshtml", _ProductionPlan_Model);
                //}
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        //Commented By Suraj on 25-01-2023
        //private List<RequirementAreaList> GetRequirmentreaList()
        //{
        //    try
        //    {
        //        string CompID = string.Empty;
        //        string BrchID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            BrchID = Session["BranchId"].ToString();
        //        }
        //        DataTable dt = _PP_ISERVICES.GetRequirmentreaList(CompID, BrchID); 
        //        List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            RequirementAreaList _RAList = new RequirementAreaList();
        //            _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
        //            _RAList.req_val = dr["setup_val"].ToString();
        //            requirementAreaLists.Add(_RAList);
        //        }
        //        requirementAreaLists.Insert(0,new RequirementAreaList { req_id = 0, req_val = "---Select---" });
        //        return requirementAreaLists;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        throw Ex;
        //    }
        //}
        [NonAction]
        private DataTable GetRequirmentreaList()
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
                DataTable dt = _PP_ISERVICES.GetRequirmentreaList(CompID, BrchID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData,string Mailerror)
        {

            ProductionPlan_Model ToRefreshByJS = new ProductionPlan_Model();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.PPNumberfc = a[0].Trim();
            ToRefreshByJS.TransTypefc = "Update";
            ToRefreshByJS.BtnNamefc = "BtnEdit";
            if (a[1].Trim() != null && a[1].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[1].Trim();
                Model.wf = a[1].Trim();
            }
            ToRefreshByJS.Messagefc = Mailerror;
            Model.bt = "BtnEdit";
            Model.MRP_No = ToRefreshByJS.PPNumberfc;
            Model.PPL_Date = ToRefreshByJS.PPDatefc;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ProductionPlanDetail", Model);
        }

        public ActionResult GetProductionPlanList(string docid, string status)
        {


            var WF_Status = status;
            return RedirectToAction("ProductionPlan", new { WF_Status });
        }

        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status1)
        {
            try
            {
                ProductionPlan_Model _ProductionPlan_Model = new ProductionPlan_Model();
                if (AppDtList != null)
                {
                    JArray jObjectBatch = JArray.Parse(AppDtList);
                    for (int i = 0; i < jObjectBatch.Count; i++)
                    {
                        _ProductionPlan_Model.PPNumber = jObjectBatch[i]["PPNo"].ToString();
                        _ProductionPlan_Model.PPDate = jObjectBatch[i]["PPDate"].ToString();
                        _ProductionPlan_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                        _ProductionPlan_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                        _ProductionPlan_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                        _ProductionPlan_Model.AutoGen_Remarks = jObjectBatch[i]["AutoGen_Remarks"].ToString();
                    }
                }
                if (_ProductionPlan_Model.A_Status != "Approve")
                {
                    _ProductionPlan_Model.A_Status = "Approve";
                }
                string command = "";
                Approve_PPDetails(_ProductionPlan_Model, command, ListFilterData1);
                UrlModel ApproveModel = new UrlModel();
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    ApproveModel.wf = WF_Status1;
                    _ProductionPlan_Model.WF_Status1 = WF_Status1;
                }
                TempData["ModelData"] = _ProductionPlan_Model;
                ApproveModel.tp = "Update";
                ApproveModel.MRP_No = _ProductionPlan_Model.PPNumberfc;
                ApproveModel.PPL_Date = _ProductionPlan_Model.PPDatefc;
                ApproveModel.bt = "BtnEdit";
                return RedirectToAction("ProductionPlanDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }


        public void BindDDLOnPageLoad(ProductionPlan_Model _PP_Model)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    string comp_id = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    List<sourcetype> fflist = new List<sourcetype>();

                    //sourcetype src_Obj1 = new sourcetype();
                    //src_Obj1.id = "D";
                    //src_Obj1.name = "Direct (Periodic)";
                    //fflist.Add(src_Obj1);
                    //sourcetype src_Obj2 = new sourcetype();
                    //src_Obj2.id = "A";
                    //src_Obj2.name = "Direct (Ad-Hoc)";
                    //fflist.Add(src_Obj2);
                    //sourcetype src_Obj = new sourcetype();
                    //src_Obj.id = "F";
                    //src_Obj.name = "Sales Forecast";
                    //fflist.Add(src_Obj);
                    fflist.Add(new sourcetype() { id = "D", name = "Direct (Periodic)" });
                    fflist.Add(new sourcetype() { id = "A", name = "Direct (Ad-Hoc)" });
                    fflist.Add(new sourcetype() { id = "F", name = "Sales Forecast" });
                    fflist.Add(new sourcetype() { id = "O", name = "Sales Order" });
                    _PP_Model.ddl_src_typeList = fflist;
                    string StartDate = "";
                    DataSet ds = _PP_ISERVICES.BindFinancialYear(Convert.ToInt32(comp_id), Convert.ToInt32(br_id), StartDate, "", "D");
                    List<financial_year> fyList = new List<financial_year>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //financial_year fyObj1 = new financial_year();/*commented by Hina on 13-09-2024 to add below out of table*/
                        //fyObj1.id = "0";
                        //fyObj1.name = "---Select---";
                        //fyList.Add(fyObj1);
                        foreach (DataRow data in ds.Tables[0].Rows)
                        {
                            financial_year fyObj = new financial_year();
                            fyObj.id = data["id"].ToString();
                            fyObj.name = data["name"].ToString();
                            fyList.Add(fyObj);
                        }
                    }
                    fyList.Insert(0, new financial_year() { id = "0", name = "---Select---" });/*Add by Hina on 13-09-2024*/

                    _PP_Model.ddl_financial_yearList = fyList;

                    List<period> plist = new List<period>();
                    period pObj = new period();
                    pObj.id = "0";
                    pObj.name = "---Select---";
                    plist.Add(pObj);
                    _PP_Model.ddl_periodList = plist;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        public void GetStatusList(ProductionPlan_Model _PP_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _PP_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<PP_List> getPPlistDetails(ProductionPlan_Model PP_Model)
        {
            try
            {
                List<PP_List> _PP_List = new List<PP_List>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                string UserID = "";
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string wfstatus = "";

                if (PP_Model.WF_Status != null)
                {
                    wfstatus = PP_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                BrchID = Session["BranchId"].ToString();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                //ds = _PP_ISERVICES.GetPPList("A", PP_Model.txtFromDate, PP_Model.txtToDate, PP_Model.Status, CompID, BrchID, wfstatus, UserID,DocumentMenuId);
                ds = _PP_ISERVICES.GetPPList("0", PP_Model.txtFromDate, PP_Model.txtToDate, PP_Model.Status, CompID, BrchID, wfstatus, UserID, DocumentMenuId);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    FromDate = ds.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        PP_List _PPList = new PP_List();
                        _PPList.PP_no = dr["pp_no"].ToString();
                        _PPList.PP_date = dr["pp_dt"].ToString();
                        _PPList.source = dr["src_type"].ToString();
                        _PPList.fy = dr["fy"].ToString();
                        _PPList.period = dr["f_periodval"].ToString();
                        _PPList.daterange = dr["daterange"].ToString();
                        _PPList.status = dr["status_name"].ToString();
                        _PPList.createon = dr["create_dt"].ToString();
                        _PPList.approvedon = dr["app_dt"].ToString();
                        _PPList.amendedon = dr["mod_dt"].ToString();
                        _PPList.create_by = dr["create_by"].ToString();
                        _PPList.app_by = dr["app_by"].ToString();
                        _PPList.mod_by = dr["mod_by"].ToString();
                        _PP_List.Add(_PPList);
                    }
                }
                return _PP_List;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        [HttpPost]
        public ActionResult BindPeriod(string financial_year, string Flag)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    string[] splitFY = financial_year.Split(',');
                    DataSet ds = _PP_ISERVICES.BindFinancialYear(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), splitFY[0], "", Flag);
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public ActionResult BindProductList()
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    //string[] splitFY = financial_year.Split(',');
                    DataSet ds = _PP_ISERVICES.BindProductList(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID));
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public ActionResult GetPlannedMaterialDetails(string ProductID)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    DataSet ds = _PP_ISERVICES.GetPlannedMaterialDetails(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), ProductID);
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        public ActionResult BindDateRange(string financial_year, string period)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string start_year = "";
                    string end_year = "";
                    int months = 0;
                    string fy_datefrom = "";
                    string fy_dateto = "";
                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    string[] splitPeriod = period.Split(',');
                    if (splitPeriod.Length > 1)
                    {
                        int start_year1 = Convert.ToDateTime(splitPeriod[0]).Year;
                        start_year = Convert.ToString(start_year1);
                        int end_year1 = Convert.ToDateTime(splitPeriod[1]).Year;
                        end_year = Convert.ToString(end_year1);
                        fy_datefrom = splitPeriod[0];
                        fy_dateto = splitPeriod[1];
                    }
                    else
                    {
                        string[] split_fy_year = financial_year.Split(',');
                        int start_year1 = Convert.ToDateTime(split_fy_year[0]).Year;
                        start_year = Convert.ToString(start_year1);
                        int end_year1 = Convert.ToDateTime(split_fy_year[1]).Year;
                        end_year = Convert.ToString(end_year1);
                        months = Convert.ToInt32(period);
                    }
                    DataSet ds = _PP_ISERVICES.BindDateRangeCal(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), start_year, end_year, months);
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/

                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        public ActionResult BindProductionDetails(string ProductionID, string hdn_FromDate, string hdn_ToDate, string PP_no, string PP_dt)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }

                    DataSet ds = _PP_ISERVICES.GetProductionDetailsData(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID)
                        , ProductionID, hdn_FromDate, hdn_ToDate, PP_no, PP_dt);
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        private DataTable GetRoleList()
        {
            try
            {
                string UserID = string.Empty;
                string CompID = string.Empty;
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
        [HttpPost]
        public ActionResult ProductionPlanSave(ProductionPlan_Model _ProductionPlan_Model, string command)
        {
            try
            {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ProductionPlan_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":

                        ProductionPlan_Model adddnew = new ProductionPlan_Model();
                        adddnew.Commandfc = "Add";
                        adddnew.TransTypefc = "Save";
                        adddnew.BtnNamefc = "BtnAddNew";
                        //adddnew.DocumentStatus = "D";
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "Add";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";
                        NewModel.DMS = "D";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ProductionPlan_Model.PPNumber))
                                return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                            else
                                adddnew.Commandfc = "Refresh";
                            adddnew.TransTypefc = "Refresh";
                            adddnew.BtnNamefc = "BtnRefresh";
                            adddnew.DocumentStatusfc = null;
                            TempData["ModelData"] = adddnew;
                            return RedirectToAction("ProductionPlanDetail", adddnew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ProductionPlanDetail", NewModel);

                    case "Edit":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string PPLDate = _ProductionPlan_Model.PPDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PPLDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        string CheckDocMsg = CheckDocAgainstPP(_ProductionPlan_Model.PPNumber, _ProductionPlan_Model.PPDate);
                        string CheckDocMsgPR = CheckDocAgainstPR(_ProductionPlan_Model.PPNumber, _ProductionPlan_Model.PPDate);
                        ProductionPlan_Model Used_Model = new ProductionPlan_Model();
                        UrlModel UsedModel = new UrlModel();
                        if (CheckDocMsg == "JCInProcess")
                        {
                            Used_Model.Messagefc = "JCInProcess";
                            Used_Model.TransTypefc = "Update";
                            Used_Model.Commandfc = "Refresh";
                            Used_Model.BtnNamefc = "BtnEdit";
                            Used_Model.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            // UrlModel UsedModel = new UrlModel();
                            UsedModel.Cmd = "Refresh";
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = Used_Model.PPNumberfc;
                            UsedModel.PPL_Date = Used_Model.PPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = Used_Model;
                        }
                        else if (CheckDocMsgPR == "MRPInProcess")
                        {
                            Used_Model.Messagefc = "MRPInProcess";
                            Used_Model.TransTypefc = "Update";
                            Used_Model.Commandfc = "Refresh";
                            Used_Model.BtnNamefc = "BtnEdit";
                            Used_Model.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            // UrlModel UsedModel = new UrlModel();
                            UsedModel.Cmd = "Refresh";
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = Used_Model.PPNumberfc;
                            UsedModel.PPL_Date = Used_Model.PPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = Used_Model;
                        }
                        else if (CheckDocMsgPR == "PRInProcess")
                        {
                            Used_Model.Messagefc = "PRInProcess";
                            Used_Model.TransTypefc = "Update";
                            Used_Model.Commandfc = "Refresh";
                            Used_Model.BtnNamefc = "BtnEdit";
                            Used_Model.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            Used_Model.PPDatefc = _ProductionPlan_Model.PPDate;
                            // UrlModel UsedModel = new UrlModel();
                            UsedModel.Cmd = "Refresh";
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = Used_Model.PPNumberfc;
                            UsedModel.PPL_Date = Used_Model.PPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = Used_Model;
                        }
                        else if (CheckDocMsg == "MRPInProcess")
                        {
                            //Session["Messagefc"] = "MRPInProcess";
                            Used_Model.Messagefc = "MRPInProcess";
                            Used_Model.TransTypefc = "Update";
                            Used_Model.Commandfc = "Refresh";
                            Used_Model.BtnNamefc = "BtnEdit";
                            Used_Model.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            Used_Model.PPDatefc = _ProductionPlan_Model.PPDate;
                            //UrlModel UsedModel = new UrlModel();
                            UsedModel.Cmd = "Refresh";
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = Used_Model.PPNumberfc;
                            UsedModel.PPL_Date = Used_Model.PPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = Used_Model;
                        }
                        else
                        {

                            _ProductionPlan_Model.Commandfc = command;
                            _ProductionPlan_Model.Amend = null;
                            _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                            _ProductionPlan_Model.TransTypefc = "Update";
                            _ProductionPlan_Model.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            _ProductionPlan_Model.PPDatefc = _ProductionPlan_Model.PPDate;
                            //UrlModel EditModel = new UrlModel();
                            UsedModel.Cmd = command;
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = _ProductionPlan_Model.PPNumberfc;
                            UsedModel.PPL_Date = _ProductionPlan_Model.PPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = _ProductionPlan_Model;
                            TempData["ListFilterData"] = _ProductionPlan_Model.ListFilterData1;
                            return RedirectToAction("ProductionPlanDetail", UsedModel);
                        }
                        TempData["ListFilterData"] = _ProductionPlan_Model.ListFilterData1;
                        return RedirectToAction("ProductionPlanDetail", UsedModel);
                    case "Amendment":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string PPLDate1 = _ProductionPlan_Model.PPDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PPLDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                        }
                        UrlModel Amendment_Model = new UrlModel();
                        ProductionPlan_Model Used_Model1 = new ProductionPlan_Model();
                        string CheckDocMsgPR1 = CheckDocAgainstPR(_ProductionPlan_Model.PPNumber, _ProductionPlan_Model.PPDate);
                        string CheckDocMsg1 = CheckDocAgainstPP(_ProductionPlan_Model.PPNumber, _ProductionPlan_Model.PPDate);
                        if (CheckDocMsgPR1 == "PRInProcess")
                        {
                            Used_Model1.Messagefc = "PRInProcess";
                            Used_Model1.TransTypefc = "Update";
                            Used_Model1.Commandfc = "Refresh";
                            Used_Model1.BtnNamefc = "BtnEdit";
                            Used_Model1.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            // UrlModel UsedModel = new UrlModel();
                            Amendment_Model.Cmd = "Refresh";
                            Amendment_Model.tp = "Update";
                            Amendment_Model.bt = "BtnEdit";
                            Amendment_Model.MRP_No = Used_Model1.PPNumberfc;
                            Amendment_Model.PPL_Date = Used_Model1.PPDatefc;
                            Amendment_Model.DMS = "D";
                            TempData["ModelData"] = Used_Model1;
                        }
                        else if (CheckDocMsg1 == "JCInProcess")
                        {
                            Used_Model1.Messagefc = "JCInProcess";
                            Used_Model1.TransTypefc = "Update";
                            Used_Model1.Commandfc = "Refresh";
                            Used_Model1.BtnNamefc = "BtnEdit";
                            Used_Model1.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            Used_Model1.PPDatefc = _ProductionPlan_Model.PPDate;
                            // UrlModel UsedModel = new UrlModel();
                            Amendment_Model.Cmd = "Refresh";
                            Amendment_Model.tp = "Update";
                            Amendment_Model.bt = "BtnEdit";
                            Amendment_Model.MRP_No = Used_Model1.PPNumberfc;
                            Amendment_Model.PPL_Date = Used_Model1.PPDatefc;
                            Amendment_Model.DMS = "D";
                            TempData["ModelData"] = Used_Model1;
                        }
                        else if (CheckDocMsgPR1 == "MRPInProcess")
                        {
                            Used_Model1.Messagefc = "MRPInProcess";
                            Used_Model1.TransTypefc = "Update";
                            Used_Model1.Commandfc = "Refresh";
                            Used_Model1.BtnNamefc = "BtnEdit";
                            Used_Model1.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            Used_Model1.PPDatefc = _ProductionPlan_Model.PPDate;
                            // UrlModel UsedModel = new UrlModel();
                            Amendment_Model.Cmd = "Refresh";
                            Amendment_Model.tp = "Update";
                            Amendment_Model.bt = "BtnEdit";
                            Amendment_Model.MRP_No = Used_Model1.PPNumberfc;
                            Amendment_Model.PPL_Date = Used_Model1.PPDatefc;
                            Amendment_Model.DMS = "D";
                            TempData["ModelData"] = Used_Model1;
                        }
                        else
                        {
                            /*End to chk Financial year exist or not*/
                            _ProductionPlan_Model.Commandfc = "Edit";
                            _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                            _ProductionPlan_Model.TransTypefc = "Update";
                            _ProductionPlan_Model.Amend = "Amend";
                            _ProductionPlan_Model.PPNumberfc = _ProductionPlan_Model.PPNumber;
                            _ProductionPlan_Model.PPDatefc = _ProductionPlan_Model.PPDate;
                            Amendment_Model.Cmd = "Edit";
                            Amendment_Model.tp = "Update";
                            Amendment_Model.bt = "BtnEdit";
                            Amendment_Model.Amend = "Amend";
                            Amendment_Model.MRP_No = _ProductionPlan_Model.PPNumberfc;
                            Amendment_Model.PPL_Date = Used_Model1.PPDatefc;
                            // Amendment_Model.DMS = "D";//DMS is DocumentStatus
                            TempData["ModelData"] = _ProductionPlan_Model;
                        }

                        TempData["ListFilterData"] = _ProductionPlan_Model.ListFilterData1;
                        return RedirectToAction("ProductionPlanDetail", Amendment_Model);
                    case "Delete":

                        Delete_PPDetails(_ProductionPlan_Model, command);
                        ProductionPlan_Model DeleteModel = new ProductionPlan_Model();
                        DeleteModel.Messagefc = "Deleted";
                        DeleteModel.Commandfc = "Refresh";
                        DeleteModel.TransTypefc = "Refresh";
                        DeleteModel.BtnNamefc = "BtnRefresh";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Commandfc;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnRefresh";
                        TempData["ListFilterData"] = _ProductionPlan_Model.ListFilterData1;
                        return RedirectToAction("ProductionPlanDetail", Delete_Model);

                    case "Save":

                        if (_ProductionPlan_Model.Amend != null && _ProductionPlan_Model.Amend != "" && _ProductionPlan_Model.Amendment == null)
                        {

                            _ProductionPlan_Model.TransTypefc = "Amendment";
                        }
                        SaveUpdatePP_Details(_ProductionPlan_Model);
                        if (_ProductionPlan_Model.Messagefc == "DocModify")
                        {
                            /* Add Condition By Nitesh 12-09-2023 
                             * add New Data cheack  src_docNo status when cancel and force close when Data Not Save and save procedure
                              * return NotModified and show Massage */

                            if (Session["compid"] != null)
                            {
                                CompID = Session["compid"].ToString();
                            }
                            if (Session["userid"] != null)
                            {
                                userid = Session["userid"].ToString();
                            }

                            ViewBag.MenuPageName = getDocumentName();
                            _ProductionPlan_Model.Title = title;
                            ViewBag.VBRoleList = GetRoleList();
                            List<sourcetype> fflist = new List<sourcetype>();
                            fflist.Add(new sourcetype() { id = _ProductionPlan_Model.ddlsrctype, name = _ProductionPlan_Model.ddlsrctype_id });
                            _ProductionPlan_Model.ddl_src_typeList = fflist;
                            BindDDLOnPageLoad(_ProductionPlan_Model);
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            /*-------------------------- For Production Plan Against Sales order-------------------*/
                            //if (_ProductionPlan_Model.ddlsrctype != "A" && _ProductionPlan_Model.ddlsrctype != "O")
                            //{
                            _ProductionPlan_Model.ddl_financial_year = _ProductionPlan_Model.ddlfinancialyear;
                            _ProductionPlan_Model.ddl_period = _ProductionPlan_Model.hdn_period;

                            List<period> plist = new List<period>();
                            period pObj = new period();
                            pObj.id = _ProductionPlan_Model.ddlperiod;
                            pObj.name = _ProductionPlan_Model.hdn_period;
                            plist.Add(pObj);
                            _ProductionPlan_Model.ddl_periodList = plist;
                            // }
                            //else if(_ProductionPlan_Model.ddlsrctype == "O") {

                            _ProductionPlan_Model.ddl_src_type = _ProductionPlan_Model.ddlsrctype;

                            GetCustomerNameList(_ProductionPlan_Model);
                            List<CustName> _CustList = new List<CustName>();
                            _CustList.Insert(0, new CustName() { Cust_id = "0", Cust_name = "---Select---" });
                            _ProductionPlan_Model.CustNameList = _CustList;

                            List<OrderList> _OrderList = new List<OrderList>();
                            _OrderList.Insert(0, new OrderList() { Order_id = "0", Order_val = "---Select---" });
                            _ProductionPlan_Model.OrderNumberList = _OrderList;
                            //}
                            ViewBag.SoItemDetails = ViewData["dtsub_itm"];//dtsub_itm
                            ViewBag.SubItemDetails = ViewData["subitem"];
                            ViewBag.ProductDetails = ViewData["ItemData"];
                            ViewBag.DocumentCode = "D";
                            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanDetail.cshtml", _ProductionPlan_Model);
                        }
                        if (_ProductionPlan_Model.Messagefc == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = _ProductionPlan_Model;
                        UrlModel SaveModel = new UrlModel();
                        SaveModel.bt = _ProductionPlan_Model.BtnNamefc;
                        SaveModel.MRP_No = _ProductionPlan_Model.PPNumberfc;
                        SaveModel.PPL_Date = _ProductionPlan_Model.PPDatefc;
                        SaveModel.tp = _ProductionPlan_Model.TransTypefc;
                        SaveModel.Cmd = _ProductionPlan_Model.Commandfc;

                        TempData["ListFilterData"] = _ProductionPlan_Model.ListFilterData1;
                        return RedirectToAction("ProductionPlanDetail", SaveModel);

                    case "Forward":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string PPLDate2 = _ProductionPlan_Model.PPDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PPLDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string PPLDate3 = _ProductionPlan_Model.PPDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PPLDate3) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticPPDetails", new { PPNumber = _ProductionPlan_Model.PPNumber, PPDate = _ProductionPlan_Model.PPDate, ListFilterData = _ProductionPlan_Model.ListFilterData1, WF_Status = _ProductionPlan_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        Approve_PPDetails(_ProductionPlan_Model, command, "");
                        TempData["ModelData"] = _ProductionPlan_Model;
                        UrlModel ApproveModel = new UrlModel();
                        ApproveModel.tp = "Update";
                        ApproveModel.MRP_No = _ProductionPlan_Model.PPNumberfc;
                        ApproveModel.PPL_Date = _ProductionPlan_Model.PPDatefc;
                        ApproveModel.bt = "BtnEdit";
                        TempData["ListFilterData"] = _ProductionPlan_Model.ListFilterData1;
                        return RedirectToAction("ProductionPlanDetail", ApproveModel);

                    case "Refresh":

                        ProductionPlan_Model RefreshModel = new ProductionPlan_Model();
                        RefreshModel.Commandfc = command;
                        RefreshModel.BtnNamefc = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatusfc = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh_Model = new UrlModel();
                        refesh_Model.tp = "Save";
                        refesh_Model.bt = "BtnRefresh";
                        refesh_Model.Cmd = command;
                        TempData["ListFilterData"] = _ProductionPlan_Model.ListFilterData1;
                        return RedirectToAction("ProductionPlanDetail", refesh_Model);

                    case "Print":
                        return GenratePdfFile(_ProductionPlan_Model, command);
                    case "intimation":
                        return GenratePdfFile1(_ProductionPlan_Model);
                    case "BacktoList":

                        var WF_Status = _ProductionPlan_Model.WF_Status1;
                        TempData["ListFilterData"] = _ProductionPlan_Model.ListFilterData1;
                        return RedirectToAction("ProductionPlan", new { WF_Status });

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
        public ActionResult SaveUpdatePP_Details(ProductionPlan_Model _ProductionPlan_Model)
        {
            try
            {
                if (_ProductionPlan_Model.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }

                    DataTable PPHeader = new DataTable();
                    DataTable PPItemDetails = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(string));
                    dtheader.Columns.Add("br_id", typeof(string));
                    dtheader.Columns.Add("pp_no", typeof(string));
                    dtheader.Columns.Add("pp_dt", typeof(string));

                    dtheader.Columns.Add("fy", typeof(string));
                    dtheader.Columns.Add("period", typeof(string));
                    dtheader.Columns.Add("from_date", typeof(string));
                    dtheader.Columns.Add("to_date", typeof(string));

                    dtheader.Columns.Add("pr_no", typeof(string));

                    dtheader.Columns.Add("src_type", typeof(string));
                    dtheader.Columns.Add("src_doc_no", typeof(string));
                    dtheader.Columns.Add("src_doc_date", typeof(string));
                    dtheader.Columns.Add("pp_status", typeof(string));
                    dtheader.Columns.Add("user_id", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));

                    DataRow dtHeaderrow = dtheader.NewRow();
                    dtHeaderrow["MenuDocumentId"] = DocumentMenuId;
                    //dtHeaderrow["TransType"] = Session["TransTypefc"].ToString();
                    if (_ProductionPlan_Model.Amend != null && _ProductionPlan_Model.Amend != "" && _ProductionPlan_Model.Amendment == null)
                    {

                        dtHeaderrow["TransType"] = "Amendment";
                    }
                    else if (_ProductionPlan_Model.PPNumber != null && _ProductionPlan_Model.Amend == null)
                    {
                        dtHeaderrow["TransType"] = "Update";
                    }
                    else
                    {
                        dtHeaderrow["TransType"] = "Save";
                    }
                    dtHeaderrow["comp_id"] = Session["CompId"].ToString();
                    dtHeaderrow["br_id"] = Session["BranchId"].ToString();
                    dtHeaderrow["pp_no"] = _ProductionPlan_Model.PPNumber;
                    dtHeaderrow["pp_dt"] = _ProductionPlan_Model.PPDate;
                    dtHeaderrow["fy"] = _ProductionPlan_Model.ddlfinancialyear;
                    dtHeaderrow["period"] = _ProductionPlan_Model.ddlperiod;
                    dtHeaderrow["from_date"] = _ProductionPlan_Model.ddlsrctype == "A" ? _ProductionPlan_Model.ADHocFromDate : _ProductionPlan_Model.hfFromDate;
                    dtHeaderrow["to_date"] = _ProductionPlan_Model.ddlsrctype == "A" ? _ProductionPlan_Model.ADHocToDate : _ProductionPlan_Model.hfToDate;

                    dtHeaderrow["pr_no"] = "";
                    dtHeaderrow["src_type"] = _ProductionPlan_Model.ddlsrctype;
                    dtHeaderrow["src_doc_no"] = "";
                    dtHeaderrow["src_doc_date"] = "";
                    dtHeaderrow["pp_status"] = "D";
                    dtHeaderrow["user_id"] = Session["UserId"].ToString();
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    dtHeaderrow["mac_id"] = SystemDetail;
                    dtheader.Rows.Add(dtHeaderrow);
                    PPHeader = dtheader;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(string));
                    dtItem.Columns.Add("forecast_qty", typeof(string));
                    dtItem.Columns.Add("Pending_ord_qty", typeof(string));
                    dtItem.Columns.Add("plan_qty", typeof(string));
                    dtItem.Columns.Add("ord_to_procure_qty", typeof(string));
                    dtItem.Columns.Add("procure_comp_date", typeof(string));
                    dtItem.Columns.Add("ord_to_produce_qty", typeof(string));
                    dtItem.Columns.Add("produce_comp_date", typeof(string));
                    dtItem.Columns.Add("remarks", typeof(string));
                    dtItem.Columns.Add("AvlStk", typeof(string));
                    dtItem.Columns.Add("WipStk", typeof(string));
                    dtItem.Columns.Add("ShflStk", typeof(string));

                    JArray jObject = JArray.Parse(_ProductionPlan_Model.ProductDetail);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        //decimal planned_qty;
                        //if (jObject[i]["PlannedQuantity"].ToString() == "")
                        //{
                        //    planned_qty = 0;
                        //}
                        //else
                        //{
                        //    planned_qty = Convert.ToDecimal(jObject[i]["PlannedQuantity"].ToString());
                        //}

                        DataRow dtrowItemdetails = dtItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject[i]["ProductId"].ToString();
                        dtrowItemdetails["uom_id"] = jObject[i]["UOMId"].ToString();
                        dtrowItemdetails["forecast_qty"] = IsNull(jObject[i]["ForecastQuantity"].ToString(), "0");
                        dtrowItemdetails["Pending_ord_qty"] = IsNull(jObject[i]["Pending_ord_qty"].ToString(), "0");
                        dtrowItemdetails["plan_qty"] = IsNull(jObject[i]["PlannedQuantity"].ToString(), "0");
                        dtrowItemdetails["ord_to_procure_qty"] = IsNull(jObject[i]["ord_to_procure_qty"].ToString(), "0");
                        dtrowItemdetails["procure_comp_date"] = jObject[i]["procure_comp_date"].ToString();
                        dtrowItemdetails["ord_to_produce_qty"] = IsNull(jObject[i]["ord_to_produce_qty"].ToString(), "0");
                        dtrowItemdetails["produce_comp_date"] = jObject[i]["produce_comp_date"].ToString();
                        dtrowItemdetails["remarks"] = jObject[i]["remarks"].ToString();
                        dtrowItemdetails["AvlStk"] = jObject[i]["AvlStk"].ToString();
                        dtrowItemdetails["WipStk"] = jObject[i]["WipStk"].ToString();
                        dtrowItemdetails["ShflStk"] = jObject[i]["ShflStk"].ToString();
                        dtItem.Rows.Add(dtrowItemdetails);
                    }
                    PPItemDetails = dtItem;
                    //ViewData["ItemData"] = dtitemdetail(jObject, _ProductionPlan_Model);

                    DataTable dtSOItem = new DataTable();
                    dtSOItem.Columns.Add("item_id", typeof(string));
                    dtSOItem.Columns.Add("uom_id", typeof(string));
                    dtSOItem.Columns.Add("so_no", typeof(string));
                    dtSOItem.Columns.Add("so_dt", typeof(string));
                    dtSOItem.Columns.Add("cust_id", typeof(string));
                    dtSOItem.Columns.Add("Pending_ord_qty", typeof(string));

                    JArray jObject1 = JArray.Parse(_ProductionPlan_Model.SOItemDetail);
                    for (int i = 0; i < jObject1.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSOItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject1[i]["item_id"].ToString();
                        dtrowItemdetails["uom_id"] = jObject1[i]["uom_id"].ToString();
                        dtrowItemdetails["so_no"] = jObject1[i]["so_no"].ToString();
                        dtrowItemdetails["so_dt"] = jObject1[i]["so_date"].ToString();
                        dtrowItemdetails["cust_id"] = jObject1[i]["cust_id"].ToString();
                        dtrowItemdetails["Pending_ord_qty"] = jObject1[i]["Pending_ord_qty"].ToString();
                        dtSOItem.Rows.Add(dtrowItemdetails);
                    }

                    /*------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    //if (_ProductionPlan_Model.ddlsrctype == "F")
                    //{
                    dtSubItem.Columns.Add("SrcDocQty", typeof(string));
                    dtSubItem.Columns.Add("pending_ord_qty", typeof(string));
                    //}

                    if (_ProductionPlan_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_ProductionPlan_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            if (_ProductionPlan_Model.ddlsrctype == "F" || _ProductionPlan_Model.ddlsrctype == "O")
                            {
                                dtrowItemdetails["SrcDocQty"] = jObject2[i]["SrcDocQuantity"].ToString();
                                dtrowItemdetails["pending_ord_qty"] = jObject2[i]["SubItemPPLPendingQty"].ToString();
                            }
                            else
                            {
                                dtrowItemdetails["SrcDocQty"] = "0";
                                dtrowItemdetails["pending_ord_qty"] = "0";
                            }
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                    }
                    DataTable dtSubItemProc = new DataTable();
                    dtSubItemProc.Columns.Add("item_id", typeof(string));
                    dtSubItemProc.Columns.Add("sub_item_id", typeof(string));
                    dtSubItemProc.Columns.Add("qty", typeof(string));
                    if (_ProductionPlan_Model.SubItemDetailsDt_prcure != null)
                    {
                        JArray jObject2 = JArray.Parse(_ProductionPlan_Model.SubItemDetailsDt_prcure);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails1 = dtSubItemProc.NewRow();
                            dtrowItemdetails1["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails1["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails1["qty"] = jObject2[i]["qty"].ToString();
                            dtSubItemProc.Rows.Add(dtrowItemdetails1);
                        }
                    }
                    /*------------------Sub Item end----------------------*/
                    //var req_area = _ProductionPlan_Model.req_area;
                    string SaveMessage = _PP_ISERVICES.InsertUpdatePPDetails(PPHeader, PPItemDetails, dtSOItem, dtSubItem, dtSubItemProc, _ProductionPlan_Model.req_area);
                    if (SaveMessage == "NotModified-")
                    {
                        /* Add Condition By Nitesh 12-09-2023 
                       * add New Data cheack  src_docNo status when cancel and force close when Data Not Save and save procedure
                        * return NotModified*/
                        _ProductionPlan_Model.Messagefc = "DocModify";
                        _ProductionPlan_Model.BtnNamefc = "BtnRefresh";
                        _ProductionPlan_Model.Commandfc = "Refresh";
                        _ProductionPlan_Model.TransTypefc = "Refresh";
                        TempData["ModelData"] = _ProductionPlan_Model;
                        ViewData["ItemData"] = dtitemdetail(_ProductionPlan_Model);
                        ViewData["dtsub_itm"] = dtsub_itm(_ProductionPlan_Model);
                        ViewData["subitem"] = subitem(_ProductionPlan_Model);
                        return RedirectToAction("ProductionPlanDetail");
                    }
                    string PPNumber = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Data_Not_Found")
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _ProductionPlan_Model.Title = title;
                        var a = PPNumber.Split('-');
                        var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + _ProductionPlan_Model.Title;//ContraNo is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _ProductionPlan_Model.Messagefc = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("ProductionPlanDetail");
                    }

                    if (Message == "Update" || Message == "Save")
                        _ProductionPlan_Model.Messagefc = "Save";
                    _ProductionPlan_Model.PPNumberfc = PPNumber;
                    _ProductionPlan_Model.TransTypefc = "Update";
                    _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                    return RedirectToAction("ProductionPlanDetail");
                }
                else
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }
                    string PPno = _ProductionPlan_Model.PPNumber;
                    string PPdate = _ProductionPlan_Model.PPDate;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    string SaveMessage = _PP_ISERVICES.Cancelled_PPDetail(CompID, br_id, PPno, PPdate, userid, mac_id, DocumentMenuId);
                    if (SaveMessage == "MRPInProcess")
                    {
                        _ProductionPlan_Model.Messagefc = "MRPInProcess";
                    }
                    else if (SaveMessage == "JCInProcess")
                    {
                        _ProductionPlan_Model.Messagefc = "JCInProcess";
                    }
                    else
                    {
                        try
                        {
                           // string fileName = "PP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "ProductionPlan_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_ProductionPlan_Model.PPNumber, _ProductionPlan_Model.PPDate, fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _ProductionPlan_Model.PPNumber, "C", userid, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _ProductionPlan_Model.Messagefc = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _ProductionPlan_Model.Messagefc = _ProductionPlan_Model.Messagefc == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";

                        //_ProductionPlan_Model.Messagefc = "Cancelled";
                    }
                    _ProductionPlan_Model.PPNumberfc = _ProductionPlan_Model.PPNumber;
                    _ProductionPlan_Model.TransTypefc = "Update";
                    _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                    _ProductionPlan_Model.Commandfc = "Update";
                    return RedirectToAction("ProductionPlanDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                // return View("~/Views/Shared/Error.cshtml");
            }

        }
        public DataTable subitem(ProductionPlan_Model _ProductionPlan_Model)
        {
            /* Add Condition By Nitesh 12-09-2023 
                          * add New Data cheack  src_docNo status when cancel and force close when Data Not Save and save procedure
                           * return NotModified and Show Data Sumitem Table */
            DataTable dtSubItems = new DataTable();
            dtSubItems.Columns.Add("item_id", typeof(string));
            dtSubItems.Columns.Add("sub_item_id", typeof(string));
            dtSubItems.Columns.Add("qty", typeof(string));
            dtSubItems.Columns.Add("SrcDocQty", typeof(string));
            dtSubItems.Columns.Add("pending_ord_qty", typeof(string));
            JArray jObject2 = JArray.Parse(_ProductionPlan_Model.SubItemDetailsDt);
            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItems.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                if (_ProductionPlan_Model.ddlsrctype == "F" || _ProductionPlan_Model.ddlsrctype == "O")
                {
                    dtrowItemdetails["SrcDocQty"] = jObject2[i]["SrcDocQuantity"].ToString();
                    dtrowItemdetails["pending_ord_qty"] = jObject2[i]["SubItemPPLPendingQty"].ToString();
                }
                else
                {
                    dtrowItemdetails["SrcDocQty"] = "0";
                    dtrowItemdetails["pending_ord_qty"] = "0";
                }
                dtSubItems.Rows.Add(dtrowItemdetails);

            }
            return dtSubItems;
        }
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        public DataTable dtitemdetail(ProductionPlan_Model _ProductionPlan_Model)
        {
            /* Add Condition By Nitesh 12-09-2023 
                            * add New Data cheack  src_docNo status when cancel and force close when Data Not Save and save procedure
                             * return NotModified and Show Data itemTable */
            DataTable dtItemes = new DataTable();
            dtItemes.Columns.Add("item_id", typeof(string));
            dtItemes.Columns.Add("uom_id", typeof(string));
            dtItemes.Columns.Add("forecast_qty", typeof(string));
            dtItemes.Columns.Add("pending_ord_qty", typeof(string));
            dtItemes.Columns.Add("plan_qty", typeof(string));
            dtItemes.Columns.Add("ord_to_procure_qty", typeof(string));
            dtItemes.Columns.Add("procure_comp_date", typeof(string));
            dtItemes.Columns.Add("ord_to_produce_qty", typeof(string));
            dtItemes.Columns.Add("produce_comp_date", typeof(string));
            dtItemes.Columns.Add("remarks", typeof(string));
            dtItemes.Columns.Add("AvlStock", typeof(string));
            dtItemes.Columns.Add("wip_stock", typeof(string));
            dtItemes.Columns.Add("item_name", typeof(string));
            dtItemes.Columns.Add("uom_name", typeof(string));
            dtItemes.Columns.Add("shfl_avl_stk_bs", typeof(string));
            dtItemes.Columns.Add("BOM_Avl", typeof(string));
            dtItemes.Columns.Add("sub_item", typeof(string));
            dtItemes.Columns.Add("prod_qty", typeof(string));
            dtItemes.Columns.Add("so_no", typeof(string));
            dtItemes.Columns.Add("so_dt", typeof(string));
            dtItemes.Columns.Add("cust_id", typeof(string));
            dtItemes.Columns.Add("so_date", typeof(string));
            dtItemes.Columns.Add("cust_name", typeof(string));
            JArray jObject4 = JArray.Parse(_ProductionPlan_Model.SOItemDetail);
            JArray jObject5 = JArray.Parse(_ProductionPlan_Model.ProductDetail);
            for (int i = 0; i < jObject5.Count; i++)
            {
                DataRow dtrowItemdetail = dtItemes.NewRow();
                dtrowItemdetail["item_id"] = jObject5[i]["ProductId"].ToString();
                dtrowItemdetail["uom_id"] = jObject5[i]["UOMId"].ToString();
                dtrowItemdetail["forecast_qty"] = IsNull(jObject5[i]["ForecastQuantity"].ToString(), "0");
                dtrowItemdetail["pending_ord_qty"] = IsNull(jObject5[i]["Pending_ord_qty"].ToString(), "0");
                dtrowItemdetail["plan_qty"] = IsNull(jObject5[i]["PlannedQuantity"].ToString(), "0");
                dtrowItemdetail["ord_to_procure_qty"] = IsNull(jObject5[i]["ord_to_procure_qty"].ToString(), "0");
                dtrowItemdetail["procure_comp_date"] = jObject5[i]["procure_comp_date"].ToString();
                dtrowItemdetail["ord_to_produce_qty"] = IsNull(jObject5[i]["ord_to_produce_qty"].ToString(), "0");
                dtrowItemdetail["produce_comp_date"] = jObject5[i]["produce_comp_date"].ToString();
                dtrowItemdetail["remarks"] = jObject5[i]["remarks"].ToString();
                dtrowItemdetail["AvlStock"] = jObject5[i]["AvlStk"].ToString();
                dtrowItemdetail["wip_stock"] = jObject5[i]["WipStk"].ToString();
                dtrowItemdetail["shfl_avl_stk_bs"] = jObject5[i]["ShflStk"].ToString();
                dtrowItemdetail["item_name"] = jObject5[i]["item_name"].ToString();
                dtrowItemdetail["uom_name"] = jObject5[i]["UOMName"].ToString();
                dtrowItemdetail["BOM_Avl"] = jObject5[i]["BOM_Avl"].ToString();
                dtrowItemdetail["sub_item"] = jObject5[i]["subitem"].ToString();
                dtrowItemdetail["prod_qty"] = IsNull(jObject5[i]["prod_qty"].ToString(), "0");

                for (int j = 0; j < jObject4.Count; j++)
                {
                    if (jObject5[i]["ProductId"].ToString() == jObject4[i]["item_id"].ToString())
                    {
                        dtrowItemdetail["so_no"] = jObject4[i]["so_no"].ToString();
                        dtrowItemdetail["so_dt"] = jObject4[i]["so_date"].ToString();
                        dtrowItemdetail["so_date"] = jObject4[i]["so_date"].ToString();
                        dtrowItemdetail["cust_id"] = jObject4[i]["cust_id"].ToString();
                        dtrowItemdetail["cust_name"] = jObject4[i]["cust_name"].ToString();
                    }
                }
                dtItemes.Rows.Add(dtrowItemdetail);
            }
            // DataTable dtSOItem = new DataTable();




            return dtItemes;

        }
        public DataTable dtsub_itm(ProductionPlan_Model _ProductionPlan_Model)
        {
            /* Add Condition By Nitesh 12-09-2023 
                           * add New Data cheack  src_docNo status when cancel and force close when Data Not Save and save procedure
                            * return NotModified and Show Data Subitem Table */
            /*This Method is used in case save*/
            DataTable dtSOItem = new DataTable();
            dtSOItem.Columns.Add("item_id", typeof(string));
            dtSOItem.Columns.Add("uom_id", typeof(string));
            dtSOItem.Columns.Add("so_no", typeof(string));
            dtSOItem.Columns.Add("so_dt", typeof(string));
            dtSOItem.Columns.Add("cust_id", typeof(string));
            dtSOItem.Columns.Add("so_date", typeof(string));
            dtSOItem.Columns.Add("cust_name", typeof(string));
            dtSOItem.Columns.Add("Pending_ord_qty", typeof(string));

            JArray jObject4 = JArray.Parse(_ProductionPlan_Model.SOItemDetail);
            for (int i = 0; i < jObject4.Count; i++)
            {
                DataRow dtrowItemdetail = dtSOItem.NewRow();
                dtrowItemdetail["item_id"] = jObject4[i]["item_id"].ToString();
                dtrowItemdetail["uom_id"] = jObject4[i]["uom_id"].ToString();
                dtrowItemdetail["so_no"] = jObject4[i]["so_no"].ToString();
                dtrowItemdetail["so_dt"] = jObject4[i]["so_date"].ToString();
                dtrowItemdetail["so_date"] = jObject4[i]["so_date"].ToString();
                dtrowItemdetail["cust_id"] = jObject4[i]["cust_id"].ToString();
                dtrowItemdetail["cust_name"] = jObject4[i]["cust_name"].ToString();
                dtrowItemdetail["Pending_ord_qty"] = jObject4[i]["Pending_ord_qty"].ToString();
                dtSOItem.Rows.Add(dtrowItemdetail);
            }

            return dtSOItem;
        }
        private string IsNull(string Str, string Str2)
        {
            if (!string.IsNullOrEmpty(Str))
            {
            }
            else
                Str = Str2;
            return Str;
        }
        public string CheckDocAgainstPP(string DocNo, string DocDate)
        {
            string str = "";
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet Deatils = _PP_ISERVICES.CheckDocAgainstPP(Comp_ID, Br_ID, DocNo, DocDate);

                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(Deatils.Tables[0].Rows[0]["DocCount"]) > 0)
                    {
                        str = Deatils.Tables[0].Rows[0]["DocStatus"].ToString();
                    }
                }
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public string CheckDocAgainstPR(string DocNo, string DocDate)
        {
            string str = "";
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet Deatils = _PP_ISERVICES.CheckDocAgainstPR(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "PRInProcess";
                }
                else if (Deatils.Tables[1].Rows[0]["result"].ToString() == "MRPInProcess")
                {
                    str = "MRPInProcess";
                }
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public ActionResult PPListSearch(string Source, string Fromdate, string Todate, string Status)
        {
            try
            {
                ProductionPlan_Model _ProductionPlan_Model = new ProductionPlan_Model();
                _ProductionPlan_Model.WF_Status = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();

                List<PP_List> _PP_List = new List<PP_List>();
                DataTable dt = new DataTable();
                dt = _PP_ISERVICES.GetPPList(Source, Fromdate, Todate, Status, CompID, BrchID, "", "0", "").Tables[0];
                _ProductionPlan_Model.PPSearch = "PP_Search";
                foreach (DataRow dr in dt.Rows)
                {
                    PP_List _PPList = new PP_List();
                    _PPList.PP_no = dr["pp_no"].ToString();
                    _PPList.PP_date = dr["pp_dt"].ToString();
                    _PPList.source = dr["src_type"].ToString();
                    _PPList.fy = dr["fy"].ToString();
                    _PPList.period = dr["f_periodval"].ToString();
                    _PPList.daterange = dr["daterange"].ToString();
                    _PPList.status = dr["status_name"].ToString();
                    _PPList.createon = dr["create_dt"].ToString();
                    _PPList.approvedon = dr["app_dt"].ToString();
                    _PPList.amendedon = dr["mod_dt"].ToString();
                    _PPList.create_by = dr["create_by"].ToString();
                    _PPList.app_by = dr["app_by"].ToString();
                    _PPList.mod_by = dr["mod_by"].ToString();
                    _PP_List.Add(_PPList);
                }
                _ProductionPlan_Model.PP_ListDetail = _PP_List;
                ViewBag.VBRoleList = GetRoleList();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialProductionPlanList.cshtml", _ProductionPlan_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult EditDomesticPPDetails(string PPNumber, string PPDate, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            ProductionPlan_Model dblclick = new ProductionPlan_Model();
            UrlModel _url_model = new UrlModel();
            dblclick.PPNumberfc = PPNumber;
            dblclick.PPDatefc = PPDate;
            dblclick.TransTypefc = "Update";
            dblclick.BtnNamefc = "BtnEdit";
            if (WF_Status != null && WF_Status != "")
            {
                _url_model.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            //_url_model.Cmd = "Update";
            _url_model.tp = "Update";
            _url_model.bt = "BtnEdit";
            _url_model.MRP_No = PPNumber;
            _url_model.PPL_Date = PPDate;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("ProductionPlanDetail", _url_model);
        }
        private ActionResult Delete_PPDetails(ProductionPlan_Model _ProductionPlan_Model, string command)
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

                DataSet Message = _PP_ISERVICES.Delete_PPDetails(_ProductionPlan_Model, CompID, BrchID);
                return RedirectToAction("ProductionPlanDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult Approve_PPDetails(ProductionPlan_Model _ProductionPlan_Model, string command, string ListFilterData1)
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
                string UserID = Session["UserId"].ToString();
                string PPNo = _ProductionPlan_Model.PPNumber;
                string PPDate = _ProductionPlan_Model.PPDate;
                string A_Status = _ProductionPlan_Model.A_Status;
                string A_Level = _ProductionPlan_Model.A_Level;
                string A_Remarks = _ProductionPlan_Model.A_Remarks;
                string AutoGen_Remarks = _ProductionPlan_Model.AutoGen_Remarks;

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Message = _PP_ISERVICES.Approved_PPDetails(CompID, BrchID, UserID, PPNo, PPDate, DocumentMenuId, mac_id, A_Status, A_Level, A_Remarks, AutoGen_Remarks);

                if (Message == "Approved")
                {
                    try
                    {
                        //string fileName = "PP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "ProductionPlan_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(PPNo, PPDate, fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, PPNo, "AP", UserID, "", filePath);
                    }

                    catch (Exception exMail)
                    {
                        _ProductionPlan_Model.Messagefc = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _ProductionPlan_Model.Messagefc = _ProductionPlan_Model.Messagefc == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    //_ProductionPlan_Model.Messagefc = "Approved";
                }
                _ProductionPlan_Model.TransTypefc = "Update";
                _ProductionPlan_Model.PPNumberfc = PPNo;
                _ProductionPlan_Model.Messagefc = "Approved";
                _ProductionPlan_Model.BtnNamefc = "BtnEdit";
                TempData["ModelData"] = _ProductionPlan_Model;
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("ProductionPlanDetail", _ProductionPlan_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public JsonResult AddForeCastItemDetail(string F_Fy, string F_Period, string FromDate, string ToDate)
        {
            JsonResult dataRow = null;
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet Deatils = _PP_ISERVICES.AddForeCastItemDetail(Comp_ID, Br_ID, F_Fy, F_Period, FromDate, ToDate);
                dataRow = Json(JsonConvert.SerializeObject(Deatils));
                return dataRow;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public JsonResult AddSOItemDetail(string CustID, string OrderNo, string OrderDate)
        {
            JsonResult DataRow = new JsonResult();
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet Deatils = _PP_ISERVICES.AddSOItemDetail(Comp_ID, Br_ID, CustID, OrderNo, OrderDate);
                DataRow = Json(JsonConvert.SerializeObject(Deatils));
                return DataRow;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult GetCustomerNameList(ProductionPlan_Model PPDetailModel)
        {
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string CustType = string.Empty;
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
                if (string.IsNullOrEmpty(PPDetailModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = PPDetailModel.CustName;
                }
                CustType = "";/*CustType '' Means Both Customer Export And Domestic*/
                CustList = _PP_ISERVICES.GetCustomerList(CompID, CustomerName, BrchID, CustType);
                return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult GetOrderNumberList(ProductionPlan_Model PPDetailModel)
        {
            string OrderNumber = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string CustID = string.Empty;
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
                if (string.IsNullOrEmpty(PPDetailModel.OrderNumber))
                {
                    OrderNumber = "0";
                }
                else
                {
                    OrderNumber = PPDetailModel.OrderNumber;
                }

                CustID = PPDetailModel.CustID;
                CustList = _PP_ISERVICES.GetOrderNumberList(CompID, BrchID, CustID, OrderNumber);
                return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult GetPendingStockItemWise(string ItemId, string ItemName, string UOM, string StockType, string UomId = null)
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
                ViewBag.PendingStockItemWise = _PP_ISERVICES.PendingStockItemWise(CompID, BrchID, ItemId, UomId, StockType).Tables[0];

                ViewBag.ItemName = ItemName;
                ViewBag.UOM = UOM;
                ViewBag.StockType = StockType;

                return View("~/Areas/Common/Views/PartialWIPItemStock.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetBOMDetailsItemWise(string ItemId)
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
                ViewBag.BOMDetailsItemWise = _PP_ISERVICES.GetBOMDetailsItemWise(CompID, BrchID, ItemId);
                ViewBag.BomProduct_id = ItemId;
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialBillOfMaterial.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
            , string Flag, string Status, string Doc_no, string Doc_dt, string SrcType, string Fy, Int32 F_period, string hdn_SOOrderNO, string pp_no)
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
                DataTable dt = new DataTable();
                if (Flag == "Quantity" || Flag == "OrdrToPrecurQuantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        if (SrcType == "F")
                        {
                            dt = _PP_ISERVICES.GetSubItemDetailsFromForecast(CompID, BrchID, Item_id, Fy, F_period).Tables[0];
                            if (Flag != "OrdrToPrecurQuantity")
                            {
                                Flag = "ForeCastandSOQtyforPPL1";
                            }
                        }
                        else
                        {
                            if (SrcType == "O")
                            {
                                dt = _PP_ISERVICES.GetSubItemDetailsFromSO(CompID, BrchID, Item_id, hdn_SOOrderNO, Status, pp_no).Tables[0];
                                if (Flag != "OrdrToPrecurQuantity")
                                {
                                    Flag = "ForeCastandSOQtyforPPL";
                                }
                            }
                            else
                            {
                                dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                            }
                        }

                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = item.GetValue("qty");
                                    //dt.Rows[i]["PendingQty"] =item.GetValue("PendingQty");
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _PP_ISERVICES.PP_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                        if (SrcType == "F")
                        {
                            if (Flag != "OrdrToPrecurQuantity")
                            {
                                Flag = "ForeCastandSOQtyforPPL";
                            }
                        }
                    }
                }
                else if (Flag == "PendingQty")
                {
                    dt = _PP_ISERVICES.PP_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                }
                else if (Flag == "OrderedQty")
                {
                    dt = _PP_ISERVICES.PP_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "OrderedQty").Tables[0];
                }
                else if (Flag == "ProduceQty")
                {
                    dt = _PP_ISERVICES.PP_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "ProduceQty").Tables[0];
                    Flag = "QCAccQty";
                    ViewBag._subitemPageName = "PP";
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,

                    // _subitemPageName = "CNF"
                };

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag;
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /*--------Print---------*/

        public FileResult GenratePdfFile(ProductionPlan_Model _Model, string command)
        {
            if (command == "Print")
            {
                var data = GetPdfData(_Model.PPNumber, _Model.PPDate, command);
                if (data != null)
                    return File(data, "application/pdf", "ProductionPlan.pdf");
                else
                    return File("ErrorPage", "application/pdf");
            }
            else
            {
                var DocNo = _Model.PPNumber.Replace("/", "");
                var data = GetPdfData(_Model.PPNumber, _Model.PPDate, command);
                if (data != null)
                    return File(data, "application/pdf", DocNo + ".pdf");
                else
                    return File("ErrorPage", "application/pdf");
            }
        }
        public FileResult GenratePdfFile1(ProductionPlan_Model _model)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
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
                var ppNo = _model.PPNumber;
                var ppDate = _model.PPDate;
                var DocNo = _model.PPNumber.Replace("/", "");
                DataTable Details = new DataTable();
                Details = _PP_ISERVICES.GetintimationPrintDeatils(CompID, BrchID, ppNo, ppDate);
                DataView data = new DataView();
                data = Details.DefaultView;
                data.Sort = "shfl_name";
                Details = data.ToTable();
                DataTable dt = new DataTable();
                dt = data.ToTable(true, "shfl_name");
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string htmlcontent = "";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    data = Details.DefaultView;
                    DataTable PrintGL = new DataTable();
                    pdfDoc.Open();
                    string Start = "Y";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Start == "N")
                        {
                            pdfDoc.NewPage();
                        }
                        else
                        {
                            Start = "N";
                        }

                        //data = Details.DefaultView;
                        data.RowFilter = "shfl_name='" + dr["shfl_name"].ToString() + "'";
                        PrintGL = data.ToTable();
                        ViewBag.Details = PrintGL;
                        //ViewBag.GLAccountName = PrintGL.Rows[0]["op_bal"].ToString();
                        //ViewBag.GLAccountName = PrintGL.Rows[PrintGL.Rows.Count - 1]["closing_bal"].ToString();
                        htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanIntimation.cshtml"));
                        reader = new StringReader(htmlcontent);
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    }
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    //Font font = new Font(bf, 9, Font.BOLDITALIC, BaseColor.BLACK);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return File(bytes.ToArray(), "application/pdf", DocNo + ".pdf");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
            finally
            {

            }
        }
        public byte[] GetPdfData(string ppNo, string ppDate, string command)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string GLVoucherHtml = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (command == "Print")
                {
                    DataSet Deatils = _PP_ISERVICES.GetProductionPlanPrintDeatils(CompID, BrchID, ppNo, ppDate);
                    ViewBag.PageName = "PP";
                    ViewBag.Title = "Production Plan";
                    ViewBag.Details = Deatils;
                    ViewBag.CompLogoDtl = Deatils.Tables[0];
                    string path1 = Server.MapPath("~") + "..\\Attachment\\";
                    string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                    ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                    ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                    GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/ProductionPlanPrint.cshtml"));
                }
                //else
                //{
                //DataSet Deatils = _PP_ISERVICES.GetintimationPrintDeatils(CompID, BrchID, ppNo, ppDate);
                //ViewBag.Details = Deatils;
                //GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionPlan/intimation.cshtml"));
                //}
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(GLVoucherHtml);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    string draftImage = Server.MapPath("~/Content/Images/draft.png");
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(0, 160);
                                draftimg.ScaleAbsolute(580f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }

        /*--------Print End--------*/
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Doc_dt, "Print");
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return "ErrorPage";
            }
            return null;
        }

        public ActionResult GetPpTrackingDetails(string ppNo, string ppDate)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                ViewBag.PPTrackingDetails = _PP_ISERVICES.GetPPTrackingDetails(CompID, BrchID, ppNo, ppDate);
                return View("~/Areas/ApplicationLayer/Views/Shared/_ProductionPlanTracking.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetProductionPlanQC_DetailsOnClick(string Item_id, string flag, string Item_Name, string UomAlias, string qty, string Plan_no, string Plan_dt)
        {
            try
            {
                ProductionPlan_Model objModel = new ProductionPlan_Model();
                //objModel.opId = opName;
                //objModel.shflId = shflName;
                ViewBag.ItemName = Item_Name;
                ViewBag.UOM = UomAlias;
                ViewBag.Qty = qty;
                ViewBag.InsightType = flag;
                ViewBag.ProductionAnalysisDetails = GetProductionPlan_DetailsOnClickInfo(Item_id, Plan_no, Plan_dt, flag);               
                //return PartialView("~/Areas/Common/Views/_ProductionPlanTrackingQC.cshtml", objModel);
                return PartialView("~/Areas/Common/Views/Cmn_ProductionPlanTrackingQC.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }

        }
        public DataTable GetProductionPlan_DetailsOnClickInfo(string Item_id, string Plan_no, string Plan_dt, string flag)
        {
            try
            {
                string BrID = string.Empty;
                string CompID = string.Empty;
                DataTable dt = new DataTable();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                dt = _PP_ISERVICES.GetProductionPlan_DetailsInfo(CompID, BrID, Item_id, Plan_no, Plan_dt, flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetItemQCParamDetail(string ItemID, string qc_no, string qc_dt)
        {
            try
            {
                string BrID = string.Empty;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    BrID = Session["BranchId"].ToString();
                }
                DataSet ds = _PP_ISERVICES.GetItemQCParamDetail(CompID, BrID, ItemID, qc_no, qc_dt);
                ViewBag.ItemDetailsQc = ds.Tables[0];
                ViewBag.ItemDetailsQcList = ds.Tables[1];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_OnlyShowQCParameterEvalutionDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
    }
}