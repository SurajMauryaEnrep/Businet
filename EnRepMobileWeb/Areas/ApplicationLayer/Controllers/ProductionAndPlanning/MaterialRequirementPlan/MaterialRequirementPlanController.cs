using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.MaterialRequirementPlan;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.MaterialRequirementPlan;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.MaterialRequirementPlan
{
    public class MaterialRequirementPlanController : Controller
    {
        string CompID, language, BrchID, userid, title = string.Empty;
        string DocumentMenuId = "105105117";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MaterialRequirementPlan_ISERVICES _MRP_ISERVICES;
        public MaterialRequirementPlanController(Common_IServices _Common_IServices, MaterialRequirementPlan_ISERVICES _MRP_ISERVICES)
        {
            this._MRP_ISERVICES = _MRP_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/MaterialRequirementPlan
        //MaterialRequirementPlan_Model _MRP_Model = new MaterialRequirementPlan_Model();
        public ActionResult MaterialRequirementPlan(string WF_Status)//List page load
        {
            MaterialRequirementPlan_Model _MaterialRequirementPlan_Model = new MaterialRequirementPlan_Model();
            try
            {
                CommonPageDetails();
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                //string endDate = dtnow.ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                _MaterialRequirementPlan_Model.WF_Status = WF_Status;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                List<sourcetype> fflist = new List<sourcetype>();
                fflist.Add(new sourcetype() { id = "0", name = "All" });
                fflist.Add(new sourcetype() { id = "D", name = "Direct (Periodic)" });
                fflist.Add(new sourcetype() { id = "A", name = "Direct (Ad-Hoc)" });
                fflist.Add(new sourcetype() { id = "P", name = "Production Plan" });
                fflist.Add(new sourcetype() { id = "F", name = "Sales Forecast" });

                //_MaterialRequirementPlan_Model.txtToDate = endDate;
                _MaterialRequirementPlan_Model.ddl_src_typeList = fflist;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var Source = a[0].Trim();
                    var Fromdate = a[1].Trim();
                    var Todate = a[2].Trim();
                    var Status = a[3].Trim();
                    var Req_area = a[4].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    _MaterialRequirementPlan_Model.txtFromDate = Fromdate;
                    _MaterialRequirementPlan_Model.ListFilterData = TempData["ListFilterData"].ToString();
                    List<MRP_List> _MRP_List = new List<MRP_List>();
                    DataTable dt = new DataTable();
                    dt = _MRP_ISERVICES.GetMRPList(Source, Fromdate, Todate, Status, CompID, BrchID, "", "", "", Req_area);
                    //Session["MRPSearch"] = "MRP_Search";
                    _MaterialRequirementPlan_Model.MRPSearch = "MRP_Search";
                    _MaterialRequirementPlan_Model.ddl_src_type = Source;
                    _MaterialRequirementPlan_Model.txtToDate = Todate;
                    _MaterialRequirementPlan_Model.txtFromDate = Fromdate;
                    _MaterialRequirementPlan_Model.Status = Status;
                    _MaterialRequirementPlan_Model.req_area = Convert.ToInt32(Req_area);
                    //MaterialRequirementPlan_Model _MaterialRequirementPlan_Model = new MaterialRequirementPlan_Model();
                    foreach (DataRow dr in dt.Rows)
                    {
                        MRP_List _MRPList = new MRP_List();
                        _MRPList.mrp_no = dr["mrp_no"].ToString();
                        _MRPList.mrp_date = dr["mrp_dt"].ToString();
                        _MRPList.fy = dr["fy"].ToString();
                        _MRPList.period = dr["f_periodval"].ToString();
                        _MRPList.daterange = dr["daterange"].ToString();
                        _MRPList.source = dr["src_type"].ToString();
                        _MRPList.Req_area = dr["req_area"].ToString();
                        _MRPList.status = dr["status_name"].ToString();
                        _MRPList.createon = dr["create_dt"].ToString();
                        _MRPList.approvedon = dr["app_dt"].ToString();
                        _MRPList.amendedon = dr["mod_dt"].ToString();
                        _MRPList.createdby = dr["createdby"].ToString();
                        _MRPList.approvedby = dr["approvedby"].ToString();
                        _MRPList.amendby = dr["ammendby"].ToString();
                        _MRPList.src_doc_no = dr["src_doc_no"].ToString();
                        _MRPList.src_doc_date = dr["src_doc_date"].ToString();
                        _MRP_List.Add(_MRPList);

                    }
                    _MaterialRequirementPlan_Model.MRP_ListDetail = _MRP_List;
                }
                else
                {
                    _MaterialRequirementPlan_Model.txtFromDate = startDate;
                    _MaterialRequirementPlan_Model.txtToDate = CurrentDate;
                    _MaterialRequirementPlan_Model.MRP_ListDetail = getMRPlistDetails(_MaterialRequirementPlan_Model);
                }

                GetStatusList(_MaterialRequirementPlan_Model);
                List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                DataTable dt1 = GetRequirmentreaList();
                foreach (DataRow dr in dt1.Rows)
                {
                    RequirementAreaList _RAList = new RequirementAreaList();
                    _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                    _RAList.req_val = dr["setup_val"].ToString();
                    requirementAreaLists.Add(_RAList);
                }
                requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = " ---Select---" });
                _MaterialRequirementPlan_Model._requirementAreaLists = requirementAreaLists;
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                //Session["MRPSearch"] = "0";
                _MaterialRequirementPlan_Model.MRPSearch = "0";
                //ViewBag.MenuPageName = getDocumentName();
                _MaterialRequirementPlan_Model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanList.cshtml", _MaterialRequirementPlan_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AddMaterialRequirementPlanDetail()
        {
            try
            {
                //Session["Messagefc"] = "New";
                //Session["Commandfc"] = "Add";
                //Session["AppStatusfc"] = 'D';
                //Session["TransTypefc"] = "Save";
                //Session["BtnNamefc"] = "BtnAddNew";
                MaterialRequirementPlan_Model AddNewModel = new MaterialRequirementPlan_Model();
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
                    return RedirectToAction("MaterialRequirementPlan");
                }
                /*End to chk Financial year exist or not*/
                return RedirectToAction("MaterialRequirementPlanDetail", "MaterialRequirementPlan", AddNew_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult MaterialRequirementPlanDetail(UrlModel _urlModel)//Add button page load
        {
            try
            {
                MaterialRequirementPlan_Model _MaterialRequirementPlan_Model = new MaterialRequirementPlan_Model();

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
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.MRP_Date) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var MRP_model = TempData["ModelData"] as MaterialRequirementPlan_Model;
                if (MRP_model != null)
                {
                    BindDDLOnPageLoad(MRP_model);
                    ViewBag.MenuPageName = getDocumentName();
                    MRP_model.Title = title;
                    ViewBag.VBRoleList = GetRoleList();

                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;

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
                    MRP_model._requirementAreaLists = requirementAreaLists;

                    List<PP_NumberList> PP_NumberList = new List<PP_NumberList>();
                    PP_NumberList.Add(new PP_NumberList() { PP_id = "0", PP_val = "---Select---" });
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        MRP_model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    MRP_model.PP_NoLists = PP_NumberList;
                    //if (Session["TransTypefc"] != null)
                    //{
                    //if (Session["TransTypefc"].ToString() == "Update")
                    if (MRP_model.TransTypefc == "Update")
                    {
                        //string mrp_no = Session["MRPNumberfc"].ToString();
                        string mrp_no = MRP_model.MRPNumberfc;
                        DataSet ds = _MRP_ISERVICES.GetMRPDetailByNo(CompID, mrp_no, BrchID, UserID, DocumentMenuId);
                        string PPID = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        string PPVal = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        PP_NumberList.Add(new PP_NumberList() { PP_id = PPID, PP_val = PPVal });
                        MRP_model.PP_NoLists = PP_NumberList;
                        MRP_model.PP_Number = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        MRP_model.PP_Date = ds.Tables[0].Rows[0]["src_doc_date"].ToString();
                        MRP_model.ddl_src_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        MRP_model.MRPNumber = ds.Tables[0].Rows[0]["mrp_no"].ToString();
                        MRP_model.MRPDate = ds.Tables[0].Rows[0]["mrp_dt"].ToString();
                        MRP_model.src_doc_type = ds.Tables[0].Rows[0]["src_doc_type"].ToString();
                        if (MRP_model.ddl_src_type == "A" || MRP_model.src_doc_type == "A")
                        {
                            MRP_model.AdHocFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                            MRP_model.AdHocToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        }
                        else
                        {
                            MRP_model.hfFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                            MRP_model.hfToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        }
                        MRP_model.txtFromDate = ds.Tables[0].Rows[0]["fromdate"].ToString();
                        MRP_model.txtToDate = ds.Tables[0].Rows[0]["todate"].ToString();
                        //MRP_model.hfFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                        //MRP_model.hfToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        MRP_model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        MRP_model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        MRP_model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        MRP_model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        MRP_model.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        MRP_model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        MRP_model.Status = ds.Tables[0].Rows[0]["status_name"].ToString();
                        MRP_model.req_area = Convert.ToInt32(ds.Tables[0].Rows[0]["req_area"].ToString());
                        MRP_model.RequisitionNumber = ds.Tables[0].Rows[0]["RequisitionNumber"].ToString();
                        MRP_model.ForAmmendendBtn = ds.Tables[14].Rows[0]["flag"].ToString();
                        //MRP_model.ForPOAmmendendBtn = ds.Tables[15].Rows[0]["PO_flag"].ToString();
                        //MRP_model.ForQTAmmendendBtn = ds.Tables[16].Rows[0]["Qt_flag"].ToString();
                        MRP_model.Amendment = ds.Tables[15].Rows[0]["Amendment"].ToString();
                        MRP_model.ForDeleteBtn = ds.Tables[15].Rows[0]["ForDeleteBtn"].ToString();
                        MRP_model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        MRP_model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        string StatusCode = ds.Tables[0].Rows[0]["mrp_status"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["Creator_id"].ToString().Trim();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        MRP_model.create_id = create_id;
                        MRP_model.StatusCode = StatusCode;
                        //Session["StatusCode"] = StatusCode;
                        MRP_model.StatusCode = StatusCode;
                        ViewBag.SubItemDetails = ds.Tables[12];
                        ViewBag.SubItemDetails_Percure = ds.Tables[13];
                        if (StatusCode == "C")
                        {
                            MRP_model.CancelFlag = true;
                        }
                        else
                        {
                            MRP_model.CancelFlag = false;
                        }
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Commandfc"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && MRP_model.Commandfc != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[4].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnNamefc"] = "BtnRefresh";
                                    MRP_model.BtnNamefc = "BtnRefresh";
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
                                        //Session["BtnNamefc"] = "BtnEdit";
                                        MRP_model.BtnNamefc = "BtnEdit";
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
                                        //Session["BtnNamefc"] = "BtnEdit";
                                        MRP_model.BtnNamefc = "BtnEdit";
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
                                    //Session["BtnNamefc"] = "BtnEdit";
                                    MRP_model.BtnNamefc = "BtnEdit";
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
                                        //Session["BtnNamefc"] = "BtnEdit";
                                        MRP_model.BtnNamefc = "BtnEdit";
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
                                    //Session["BtnNamefc"] = "BtnEdit";
                                    MRP_model.BtnNamefc = "BtnEdit";
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
                                    //Session["BtnNamefc"] = "BtnEdit";
                                    MRP_model.BtnNamefc = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnNamefc"] = "BtnEdit";
                                    MRP_model.BtnNamefc = "BtnEdit";
                                }
                                else
                                {
                                    //Session["BtnNamefc"] = "BtnRefresh";
                                    MRP_model.BtnNamefc = "BtnRefresh";
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
                        MRP_model.ddl_financial_yearList = fyList;
                        MRP_model.ddl_financial_year = fyObj1.id;

                        List<period> plist = new List<period>();
                        period pObj = new period();
                        pObj.id = ds.Tables[0].Rows[0]["f_period"].ToString();
                        pObj.name = ds.Tables[0].Rows[0]["f_periodval"].ToString();
                        plist.Add(pObj);
                        MRP_model.ddl_periodList = plist;
                        MRP_model.UserId = UserID;
                        ViewBag.ProductDetails = ds.Tables[1];
                        ViewBag.InputMaterialDetail = ds.Tables[2];
                        ViewBag.SFMaterialDetail = ds.Tables[7];
                        MRP_model.hdnSFMaterialDetail = Json(JsonConvert.SerializeObject(ds.Tables[8])).Data.ToString(); //ds.Tables[8];
                        MRP_model.hdnInputMaterialDetail = Json(JsonConvert.SerializeObject(ds.Tables[9])).Data.ToString(); //ds.Tables[8];
                        //ViewBag.HdnSFMaterialDetail = Json(JsonConvert.SerializeObject(ds.Tables[8])).Data; //ds.Tables[8];
                        //ViewBag.HdnRowMaterialDetail = Json(JsonConvert.SerializeObject(ds.Tables[9])).Data;// ds.Tables[9]; 
                        ViewBag.ProdOrderDetails = ds.Tables[10];
                        ViewBag.PRDetails = ds.Tables[11];
                        ViewBag.ProductMaterialDetail = ds.Tables[3];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["mrp_status"].ToString().Trim();
                        //Session["DocumentStatusfc"] = ds.Tables[0].Rows[0]["status_name"].ToString();
                        MRP_model.DocumentStatusfc = ds.Tables[0].Rows[0]["status_name"].ToString();
                        if (MRP_model.Amend != null)
                        {
                            if (MRP_model.Amend == "Amend")
                            {
                                //if (ds.Tables[14].Rows[0]["flag"].ToString() != "Y")
                                //{
                                ViewBag.DocumentCode = "D";
                                MRP_model.StatusCode = "D";
                                //}                               
                            }
                        }
                        if (MRP_model.Amendment != "Amendment" && MRP_model.Amendment != "" && MRP_model.Amendment != null)
                        {
                            MRP_model.BtnNamefc = "BtnRefresh";
                            //MRP_model.wfDisableAmnd = "wfDisableAmnd";
                        }
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanDetail.cshtml", MRP_model);
                    }
                    else
                    {
                        if (MRP_model.Amend != null)
                        {
                            if (MRP_model.Amend == "Amend")
                            {
                                ViewBag.DocumentCode = "D";
                                MRP_model.StatusCode = "D";
                            }
                        }
                        MRP_model.UserId = UserID;
                        MRP_model.MRPDate = DateTime.Now.ToString("yyyy-MM-dd");
                        //Session["DocumentStatusfc"] = "";
                        MRP_model.DocumentStatusfc = "";
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanDetail.cshtml", MRP_model);
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
                        _MaterialRequirementPlan_Model.BtnNamefc = _urlModel.bt;
                        _MaterialRequirementPlan_Model.MRPNumberfc = _urlModel.MRP_No;
                        _MaterialRequirementPlan_Model.MRPDatefc = _urlModel.MRP_Date;
                        _MaterialRequirementPlan_Model.Commandfc = _urlModel.Cmd;
                        _MaterialRequirementPlan_Model.TransTypefc = _urlModel.tp;
                        _MaterialRequirementPlan_Model.WF_Status1 = _urlModel.wf;
                        //_MaterialRequirementPlan_Model.DocumentStatus = _urlModel.DMS;
                    }
                    BindDDLOnPageLoad(_MaterialRequirementPlan_Model);
                    ViewBag.MenuPageName = getDocumentName();
                    _MaterialRequirementPlan_Model.Title = title;
                    _MaterialRequirementPlan_Model.ddl_src_type = "D";
                    ViewBag.VBRoleList = GetRoleList();

                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;

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
                    _MaterialRequirementPlan_Model._requirementAreaLists = requirementAreaLists;

                    List<PP_NumberList> PP_NumberList = new List<PP_NumberList>();
                    PP_NumberList.Add(new PP_NumberList() { PP_id = "0", PP_val = "---Select---" });
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _MaterialRequirementPlan_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    _MaterialRequirementPlan_Model.PP_NoLists = PP_NumberList;
                    //if (Session["TransTypefc"] != null)
                    //{
                    //if (Session["TransTypefc"].ToString() == "Update")
                    if (_MaterialRequirementPlan_Model.TransTypefc == "Update")
                    {
                        //string mrp_no = Session["MRPNumberfc"].ToString();
                        string mrp_no = _MaterialRequirementPlan_Model.MRPNumberfc;
                        DataSet ds = _MRP_ISERVICES.GetMRPDetailByNo(CompID, mrp_no, BrchID, UserID, DocumentMenuId);
                        string PPID = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        string PPVal = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        PP_NumberList.Add(new PP_NumberList() { PP_id = PPID, PP_val = PPVal });
                        _MaterialRequirementPlan_Model.PP_NoLists = PP_NumberList;
                        _MaterialRequirementPlan_Model.PP_Number = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _MaterialRequirementPlan_Model.PP_Date = ds.Tables[0].Rows[0]["src_doc_date"].ToString();
                        _MaterialRequirementPlan_Model.ddl_src_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                        _MaterialRequirementPlan_Model.MRPNumber = ds.Tables[0].Rows[0]["mrp_no"].ToString();
                        _MaterialRequirementPlan_Model.MRPDate = ds.Tables[0].Rows[0]["mrp_dt"].ToString();
                        _MaterialRequirementPlan_Model.src_doc_type = ds.Tables[0].Rows[0]["src_doc_type"].ToString();
                        if (_MaterialRequirementPlan_Model.ddl_src_type == "A" || _MaterialRequirementPlan_Model.src_doc_type == "A")
                        {
                            _MaterialRequirementPlan_Model.AdHocFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                            _MaterialRequirementPlan_Model.AdHocToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        }
                        else
                        {
                            _MaterialRequirementPlan_Model.hfFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                            _MaterialRequirementPlan_Model.hfToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        }
                        _MaterialRequirementPlan_Model.txtFromDate = ds.Tables[0].Rows[0]["fromdate"].ToString();
                        _MaterialRequirementPlan_Model.txtToDate = ds.Tables[0].Rows[0]["todate"].ToString();
                        _MaterialRequirementPlan_Model.hfFromDate = ds.Tables[0].Rows[0]["from_date"].ToString();
                        _MaterialRequirementPlan_Model.hfToDate = ds.Tables[0].Rows[0]["to_date"].ToString();
                        _MaterialRequirementPlan_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _MaterialRequirementPlan_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _MaterialRequirementPlan_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _MaterialRequirementPlan_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _MaterialRequirementPlan_Model.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _MaterialRequirementPlan_Model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _MaterialRequirementPlan_Model.Status = ds.Tables[0].Rows[0]["status_name"].ToString();
                        _MaterialRequirementPlan_Model.req_area = Convert.ToInt32(ds.Tables[0].Rows[0]["req_area"].ToString());
                        _MaterialRequirementPlan_Model.RequisitionNumber = ds.Tables[0].Rows[0]["RequisitionNumber"].ToString();
                        _MaterialRequirementPlan_Model.ForAmmendendBtn = ds.Tables[14].Rows[0]["flag"].ToString();
                        //_MaterialRequirementPlan_Model.ForPOAmmendendBtn = ds.Tables[15].Rows[0]["PO_flag"].ToString();
                        //_MaterialRequirementPlan_Model.ForQTAmmendendBtn = ds.Tables[16].Rows[0]["Qt_flag"].ToString();
                        _MaterialRequirementPlan_Model.Amendment = ds.Tables[15].Rows[0]["Amendment"].ToString();
                        _MaterialRequirementPlan_Model.ForDeleteBtn = ds.Tables[15].Rows[0]["ForDeleteBtn"].ToString();
                        _MaterialRequirementPlan_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _MaterialRequirementPlan_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        string StatusCode = ds.Tables[0].Rows[0]["mrp_status"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["Creator_id"].ToString().Trim();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        _MaterialRequirementPlan_Model.create_id = create_id;
                        _MaterialRequirementPlan_Model.StatusCode = StatusCode;
                        //Session["StatusCode"] = StatusCode;
                        _MaterialRequirementPlan_Model.StatusCode = StatusCode;
                        ViewBag.SubItemDetails = ds.Tables[12];
                        ViewBag.SubItemDetails_Percure = ds.Tables[13];
                        if (StatusCode == "C")
                        {
                            _MaterialRequirementPlan_Model.CancelFlag = true;
                        }
                        else
                        {
                            _MaterialRequirementPlan_Model.CancelFlag = false;
                        }
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Commandfc"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _MaterialRequirementPlan_Model.Commandfc != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[4].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnNamefc"] = "BtnRefresh";
                                    _MaterialRequirementPlan_Model.BtnNamefc = "BtnRefresh";
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
                                        //Session["BtnNamefc"] = "BtnEdit";
                                        _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
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
                                        //Session["BtnNamefc"] = "BtnEdit";
                                        _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
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
                                    //Session["BtnNamefc"] = "BtnEdit";
                                    _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
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
                                        //Session["BtnNamefc"] = "BtnEdit";
                                        _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
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
                                    //Session["BtnNamefc"] = "BtnEdit";
                                    _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
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
                                    //Session["BtnNamefc"] = "BtnEdit";
                                    _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {

                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnNamefc"] = "BtnEdit";
                                    _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";


                                }
                                else
                                {
                                    //Session["BtnNamefc"] = "BtnRefresh";
                                    _MaterialRequirementPlan_Model.BtnNamefc = "BtnRefresh";
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
                        _MaterialRequirementPlan_Model.ddl_financial_yearList = fyList;
                        _MaterialRequirementPlan_Model.ddl_financial_year = fyObj1.id;

                        List<period> plist = new List<period>();
                        period pObj = new period();
                        pObj.id = ds.Tables[0].Rows[0]["f_period"].ToString();
                        pObj.name = ds.Tables[0].Rows[0]["f_periodval"].ToString();
                        plist.Add(pObj);
                        _MaterialRequirementPlan_Model.ddl_periodList = plist;
                        _MaterialRequirementPlan_Model.UserId = UserID;
                        ViewBag.ProductDetails = ds.Tables[1];
                        ViewBag.InputMaterialDetail = ds.Tables[2];
                        ViewBag.SFMaterialDetail = ds.Tables[7];
                        _MaterialRequirementPlan_Model.hdnSFMaterialDetail = Json(JsonConvert.SerializeObject(ds.Tables[8])).Data.ToString(); //ds.Tables[8];
                        _MaterialRequirementPlan_Model.hdnInputMaterialDetail = Json(JsonConvert.SerializeObject(ds.Tables[9])).Data.ToString(); //ds.Tables[8];
                        //ViewBag.HdnSFMaterialDetail = ds.Tables[8];
                        //ViewBag.HdnRowMaterialDetail = ds.Tables[9];
                        ViewBag.ProdOrderDetails = ds.Tables[10];
                        ViewBag.PRDetails = ds.Tables[11];
                        ViewBag.ProductMaterialDetail = ds.Tables[3];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["mrp_status"].ToString().Trim();
                        //Session["DocumentStatusfc"] = ds.Tables[0].Rows[0]["status_name"].ToString();
                        _MaterialRequirementPlan_Model.DocumentStatusfc = ds.Tables[0].Rows[0]["status_name"].ToString();
                        if (_urlModel.Amend != null)
                        {
                            if (_urlModel.Amend == "Amend")
                            {
                                //if (ds.Tables[14].Rows[0]["flag"].ToString() != "Y")
                                //{
                                ViewBag.DocumentCode = "D";
                                _MaterialRequirementPlan_Model.StatusCode = "D";
                                //}                                    
                            }
                        }
                        if (_MaterialRequirementPlan_Model.Amendment != "Amendment" && _MaterialRequirementPlan_Model.Amendment != "" && _MaterialRequirementPlan_Model.Amendment != null)
                        {
                            _MaterialRequirementPlan_Model.BtnNamefc = "BtnRefresh";
                            //MRP_model.wfDisableAmnd = "wfDisableAmnd";
                        }
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanDetail.cshtml", _MaterialRequirementPlan_Model);
                    }
                    else
                    {
                        _MaterialRequirementPlan_Model.UserId = UserID;
                        _MaterialRequirementPlan_Model.MRPDate = DateTime.Now.ToString("yyyy-MM-dd");
                        //Session["DocumentStatusfc"] = "";
                        _MaterialRequirementPlan_Model.DocumentStatusfc = "";
                        if (_urlModel.Amend != null)
                        {
                            if (_urlModel.Amend == "Amend")
                            {
                                ViewBag.DocumentCode = "D";
                                _MaterialRequirementPlan_Model.StatusCode = "D";
                            }
                        }
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanDetail.cshtml", _MaterialRequirementPlan_Model);
                    }
                }


                //}
                //else
                //{
                //    _MaterialRequirementPlan_Model.MRPDate = DateTime.Now.ToString("yyyy-MM-dd");
                //    //Session["DocumentStatusfc"] = "";
                //    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanDetail.cshtml", _MaterialRequirementPlan_Model);
                //}
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData,string Mailerror)
        {
            //Session["Message"] = "";
            MaterialRequirementPlan_Model ToRefreshByJS = new MaterialRequirementPlan_Model();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.MRPNumberfc = a[0].Trim();
            ToRefreshByJS.TransTypefc = "Update";
            ToRefreshByJS.BtnNamefc = "BtnEdit";
            if (a[1].Trim() != null && a[1].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[1].Trim();
                Model.wf = a[1].Trim();
            }
            ToRefreshByJS.Messagefc = Mailerror;
            Model.bt = "BtnEdit";
            Model.MRP_No = ToRefreshByJS.MRPNumberfc;
            Model.MRP_Date = ToRefreshByJS.MRPDatefc;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("MaterialRequirementPlanDetail", Model);
        }

        public ActionResult GetMaterialRequirementPlanList(string docid, string status)
        {

            //Session["WF_status"] = status;
            MaterialRequirementPlan_Model DashBordModel = new MaterialRequirementPlan_Model();
            DashBordModel.WF_Status = status;
            var WF_Status = status;
            return RedirectToAction("MaterialRequirementPlan", new { WF_Status });
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
        public void BindDDLOnPageLoad(MaterialRequirementPlan_Model _MRP_Model)
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
                    //src_Obj1.name = "Direct";
                    //fflist.Add(src_Obj1);
                    //sourcetype src_Obj = new sourcetype();
                    //src_Obj.id = "P";
                    //src_Obj.name = "Production Plan";
                    //fflist.Add(src_Obj);
                    //fflist.Add(new sourcetype() { id = "F", name = "Sales Forecast" });
                    fflist.Add(new sourcetype() { id = "D", name = "Direct (Periodic)" });
                    fflist.Add(new sourcetype() { id = "A", name = "Direct (Ad-Hoc)" });
                    fflist.Add(new sourcetype() { id = "P", name = "Production Plan" });
                    fflist.Add(new sourcetype() { id = "F", name = "Sales Forecast" });
                    _MRP_Model.ddl_src_typeList = fflist;
                    string StartDate = "";
                    DataSet ds = _MRP_ISERVICES.BindFinancialYear(Convert.ToInt32(comp_id), Convert.ToInt32(br_id), StartDate, "", "MRP_D");
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
                    _MRP_Model.ddl_financial_yearList = fyList;

                    List<period> plist = new List<period>();
                    period pObj = new period();
                    pObj.id = "0";
                    pObj.name = "---Select---";
                    plist.Add(pObj);
                    _MRP_Model.ddl_periodList = plist;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        public void GetStatusList(MaterialRequirementPlan_Model _MRP_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _MRP_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<MRP_List> getMRPlistDetails(MaterialRequirementPlan_Model MRP_Model)
        {
            try
            {
                List<MRP_List> _MRP_List = new List<MRP_List>();
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
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if (MRP_Model.WF_Status != null)
                {
                    wfstatus = MRP_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                BrchID = Session["BranchId"].ToString();
                DataTable dt = new DataTable();
                dt = _MRP_ISERVICES.GetMRPList("0", MRP_Model.txtFromDate, MRP_Model.txtToDate, MRP_Model.Status, CompID, BrchID, wfstatus, UserID, DocumentMenuId,MRP_Model.req_area.ToString());
                //if (dt.Rows.Count > 0)
                //{
                //    FromDate = dt.Rows[0]["finstrdate"].ToString();
                //}
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        MRP_List _MRPList = new MRP_List();
                        _MRPList.mrp_no = dr["mrp_no"].ToString();
                        _MRPList.mrp_date = dr["mrp_dt"].ToString();
                        _MRPList.fy = dr["fy"].ToString();
                        _MRPList.period = dr["f_periodval"].ToString();
                        _MRPList.daterange = dr["daterange"].ToString();
                        _MRPList.source = dr["src_type"].ToString();
                        _MRPList.Req_area = dr["req_area"].ToString();
                        _MRPList.status = dr["status_name"].ToString();
                        _MRPList.createon = dr["create_dt"].ToString();
                        _MRPList.approvedon = dr["app_dt"].ToString();
                        _MRPList.amendedon = dr["mod_dt"].ToString();
                        _MRPList.createdby = dr["createdby"].ToString();
                        _MRPList.approvedby = dr["approvedby"].ToString();
                        _MRPList.amendby = dr["ammendby"].ToString();
                        _MRPList.src_doc_no = dr["src_doc_no"].ToString();
                        _MRPList.src_doc_date = dr["src_doc_date"].ToString();
                        _MRP_List.Add(_MRPList);
                    }
                }
                return _MRP_List;
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
                    DataSet ds = _MRP_ISERVICES.BindFinancialYear(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), splitFY[0], "", Flag);
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
                    DataSet ds = _MRP_ISERVICES.BindProductList(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID));
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
        public ActionResult GetPlannedMaterialDetails(string ProductID, string plannedqty)
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
                    DataSet ds = _MRP_ISERVICES.GetPlannedMaterialDetails(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), ProductID, plannedqty);
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
        public ActionResult GetPlannedRowMaterialDetails(string ProductID, string SF_Item_id, string Req_Qty)
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
                    DataSet ds = new DataSet();// _MRP_ISERVICES.GetPlannedRowMaterialDetails(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), ProductID, SF_Item_id, Req_Qty);
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
                    DataSet ds = _MRP_ISERVICES.BindDateRangeCal(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), start_year, end_year, months);
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
        public ActionResult MaterialRequirementPlanSave(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model, string command)
        {
            try
            {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_MaterialRequirementPlan_Model.ActionCommand == "Delete")
                {
                    command = "Delete";
                    _MaterialRequirementPlan_Model.ActionCommand = null;
                }
                else if (_MaterialRequirementPlan_Model.ActionCommand == "Approve")
                {
                    command = "Approve";
                    _MaterialRequirementPlan_Model.ActionCommand = null;
                }
                switch (command)
                {
                    case "AddNew":
                        MaterialRequirementPlan_Model adddnew = new MaterialRequirementPlan_Model();
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
                            if (!string.IsNullOrEmpty(_MaterialRequirementPlan_Model.MRPNumber))
                                return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
                            else
                                adddnew.Commandfc = "Refresh";
                            adddnew.TransTypefc = "Refresh";
                            adddnew.BtnNamefc = "BtnRefresh";
                            adddnew.DocumentStatusfc = null;
                            TempData["ModelData"] = adddnew;
                            return RedirectToAction("MaterialRequirementPlanDetail", adddnew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("MaterialRequirementPlanDetail", NewModel);

                    case "Edit":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string MRPDate = _MaterialRequirementPlan_Model.MRPDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MRPDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        string CheckDocAgainstMRP = CheckPRAgainstMRP(_MaterialRequirementPlan_Model.MRPNumber, _MaterialRequirementPlan_Model.MRPDate.ToString());
                        if (CheckDocAgainstMRP == "Used")
                        {
                            MaterialRequirementPlan_Model Used_Model = new MaterialRequirementPlan_Model();
                            //Used_Model.Messagefc = "Used";
                            Used_Model.Messagefc = "PRInProcess";
                            Used_Model.TransTypefc = "Update";
                            Used_Model.Commandfc = "Refresh";
                            Used_Model.BtnNamefc = "BtnEdit";
                            Used_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                            Used_Model.MRPDatefc = _MaterialRequirementPlan_Model.MRPDate;
                            UrlModel UsedModel = new UrlModel();
                            UsedModel.Cmd = "Refresh";
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = Used_Model.MRPNumberfc;
                            UsedModel.MRP_Date = Used_Model.MRPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = Used_Model;
                            TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                            return RedirectToAction("MaterialRequirementPlanDetail", UsedModel);
                        }
                        else if (CheckDocAgainstMRP == "JCInProcess")
                        {
                            MaterialRequirementPlan_Model Used_Model = new MaterialRequirementPlan_Model();
                            Used_Model.Messagefc = "JCInProcess";
                            Used_Model.TransTypefc = "Update";
                            Used_Model.Commandfc = "Refresh";
                            Used_Model.BtnNamefc = "BtnEdit";
                            Used_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                            Used_Model.MRPDatefc = _MaterialRequirementPlan_Model.MRPDate;
                            UrlModel UsedModel = new UrlModel();
                            UsedModel.Cmd = "Refresh";
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = Used_Model.MRPNumberfc;
                            UsedModel.MRP_Date = Used_Model.MRPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = Used_Model;
                            TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                            return RedirectToAction("MaterialRequirementPlanDetail", UsedModel);
                        }
                        else
                        {
                            _MaterialRequirementPlan_Model.Amend = null;

                            _MaterialRequirementPlan_Model.Commandfc = command;
                            _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
                            _MaterialRequirementPlan_Model.TransTypefc = "Update";
                            _MaterialRequirementPlan_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                            _MaterialRequirementPlan_Model.MRPDatefc = _MaterialRequirementPlan_Model.MRPDate;
                            UrlModel EditModel = new UrlModel();
                            EditModel.Cmd = command;
                            EditModel.tp = "Update";
                            EditModel.bt = "BtnEdit";
                            EditModel.MRP_No = _MaterialRequirementPlan_Model.MRPNumberfc;
                            EditModel.MRP_Date = _MaterialRequirementPlan_Model.MRPDatefc;
                            EditModel.DMS = "D";
                            TempData["ModelData"] = _MaterialRequirementPlan_Model;
                            TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                            return RedirectToAction("MaterialRequirementPlanDetail", EditModel);
                        }
                    //TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                    //return RedirectToAction("MaterialRequirementPlanDetail");
                    case "Amendment":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string MRPDate1 = _MaterialRequirementPlan_Model.MRPDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MRPDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        UrlModel UrlAmendment = new UrlModel();
                        string CheckDocAgainstMRP1 = CheckPRAgainstMRP(_MaterialRequirementPlan_Model.MRPNumber, _MaterialRequirementPlan_Model.MRPDate.ToString());
                        if (CheckDocAgainstMRP1 == "Used")
                        {
                            MaterialRequirementPlan_Model Used_Model = new MaterialRequirementPlan_Model();
                            //Used_Model.Messagefc = "Used";
                            Used_Model.Messagefc = "PRInProcess";
                            Used_Model.TransTypefc = "Update";
                            Used_Model.Commandfc = "Refresh";
                            Used_Model.BtnNamefc = "BtnEdit";
                            Used_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                            Used_Model.MRPDatefc = _MaterialRequirementPlan_Model.MRPDate;
                            UrlModel UsedModel = new UrlModel();
                            UsedModel.Cmd = "Refresh";
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = Used_Model.MRPNumberfc;
                            UsedModel.MRP_Date = Used_Model.MRPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = Used_Model;
                            TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                            return RedirectToAction("MaterialRequirementPlanDetail", UsedModel);
                        }
                        else if (CheckDocAgainstMRP1 == "JCInProcess")
                        {
                            MaterialRequirementPlan_Model Used_Model = new MaterialRequirementPlan_Model();
                            Used_Model.Messagefc = "JCInProcess";
                            Used_Model.TransTypefc = "Update";
                            Used_Model.Commandfc = "Refresh";
                            Used_Model.BtnNamefc = "BtnEdit";
                            Used_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                            Used_Model.MRPDatefc = _MaterialRequirementPlan_Model.MRPDate;
                            UrlModel UsedModel = new UrlModel();
                            UsedModel.Cmd = "Refresh";
                            UsedModel.tp = "Update";
                            UsedModel.bt = "BtnEdit";
                            UsedModel.MRP_No = Used_Model.MRPNumberfc;
                            UsedModel.MRP_Date = Used_Model.MRPDatefc;
                            UsedModel.DMS = "D";
                            TempData["ModelData"] = Used_Model;
                            TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                            return RedirectToAction("MaterialRequirementPlanDetail", UsedModel);
                        }
                        else
                        {
                            _MaterialRequirementPlan_Model.Commandfc = "Edit";
                            _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
                            _MaterialRequirementPlan_Model.TransTypefc = "Update";
                            _MaterialRequirementPlan_Model.Amend = "Amend";
                            _MaterialRequirementPlan_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                            _MaterialRequirementPlan_Model.MRPDatefc = _MaterialRequirementPlan_Model.MRPDate;
                            UrlAmendment.Cmd = "Edit";//command;
                            UrlAmendment.tp = "Update";
                            UrlAmendment.bt = "BtnEdit";
                            UrlAmendment.MRP_No = _MaterialRequirementPlan_Model.MRPNumberfc;
                            UrlAmendment.MRP_Date = _MaterialRequirementPlan_Model.MRPDatefc;
                            UrlAmendment.DMS = "D";
                            UrlAmendment.Amend = "Amend";
                            TempData["ModelData"] = _MaterialRequirementPlan_Model;
                        }
                        return RedirectToAction("MaterialRequirementPlanDetail", UrlAmendment);
                    case "Delete":
                        //Session["Commandfc"] = command;
                        //Session["BtnNamefc"] = "BtnRefresh";
                        Delete_MRPDetails(_MaterialRequirementPlan_Model, command);
                        MaterialRequirementPlan_Model DeleteModel = new MaterialRequirementPlan_Model();
                        DeleteModel.Messagefc = "Deleted";
                        DeleteModel.Commandfc = "Refresh";
                        DeleteModel.TransTypefc = "Refresh";
                        DeleteModel.BtnNamefc = "BtnRefresh";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Commandfc;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnRefresh";
                        TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                        return RedirectToAction("MaterialRequirementPlanDetail", Delete_Model);

                    case "Save":
                        // Session["Commandfc"] = command;
                        _MaterialRequirementPlan_Model.Commandfc = command;
                        SaveUpdateMRP_Details(_MaterialRequirementPlan_Model);
                        if (_MaterialRequirementPlan_Model.Messagefc == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_MaterialRequirementPlan_Model.Messagefc == "DocModify")
                        {
                            /**This Massage is created by Nitesh used in show Data when message is come DocModify(NotModified) 
                             * in insert new data**/
                            BindDDLOnPageLoad(_MaterialRequirementPlan_Model);
                            ViewBag.MenuPageName = getDocumentName();
                            _MaterialRequirementPlan_Model.Title = title;
                            ViewBag.VBRoleList = GetRoleList();

                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                            ViewBag.DocumentMenuId = DocumentMenuId;

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
                            _MaterialRequirementPlan_Model._requirementAreaLists = requirementAreaLists;

                            List<PP_NumberList> PP_NumberList = new List<PP_NumberList>();
                            PP_NumberList.Add(new PP_NumberList() { PP_id = _MaterialRequirementPlan_Model.PP_Number, PP_val = _MaterialRequirementPlan_Model.PP_Number });
                            _MaterialRequirementPlan_Model.PP_NoLists = PP_NumberList;
                            List<sourcetype> fflist = new List<sourcetype>();
                            fflist.Add(new sourcetype() { id = _MaterialRequirementPlan_Model.ddlsrctype, name = _MaterialRequirementPlan_Model.ddl_srctype });
                            _MaterialRequirementPlan_Model.ddl_src_typeList = fflist;
                            _MaterialRequirementPlan_Model.ddl_src_type = _MaterialRequirementPlan_Model.ddlsrctype;
                            _MaterialRequirementPlan_Model.docmodified = "DocModify";
                            _MaterialRequirementPlan_Model.ddl_financial_year = _MaterialRequirementPlan_Model.ddlfinancialyear;
                            List<period> plist = new List<period>();
                            period pObj = new period();
                            pObj.id = _MaterialRequirementPlan_Model.ddlperiod;
                            pObj.name = _MaterialRequirementPlan_Model.hdnddl_period;
                            plist.Add(pObj);
                            _MaterialRequirementPlan_Model.ddl_periodList = plist;

                            ViewBag.DocumentCode = "D";
                            ViewBag.ProductDetails = ViewData["ItemData"];
                            ViewBag.SubItemDetails = ViewData["Subitem"];
                            ViewBag.InputMaterialDetail = ViewData["Rawmate_itmDtl"];
                            ViewBag.HdnSFMaterialDetail = ViewData["HdnSFMaterialDetail"];
                            ViewBag.HdnRowMaterialDetail = ViewData["hdn_Raw_MaterialDetail"];
                            ViewBag.SFMaterialDetail = ViewData["SFItemTable"];
                            ViewBag.SubItemDetails_Percure = ViewData["Sub_item_procure"];
                            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanDetail.cshtml", _MaterialRequirementPlan_Model);
                        }

                        //Session["MRPNumberfc"] = Session["MRPNumberfc"].ToString();
                        TempData["ModelData"] = _MaterialRequirementPlan_Model;
                        UrlModel SaveModel = new UrlModel();
                        SaveModel.bt = _MaterialRequirementPlan_Model.BtnNamefc;
                        SaveModel.MRP_No = _MaterialRequirementPlan_Model.MRPNumberfc;
                        SaveModel.MRP_Date = _MaterialRequirementPlan_Model.MRPDatefc;
                        SaveModel.tp = _MaterialRequirementPlan_Model.TransTypefc;
                        SaveModel.Cmd = _MaterialRequirementPlan_Model.Commandfc;
                        TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                        return RedirectToAction("MaterialRequirementPlanDetail", SaveModel);

                    case "Forward":
                        /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string MRPDate2 = _MaterialRequirementPlan_Model.MRPDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MRPDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
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
                        //    return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string MRPDate3 = _MaterialRequirementPlan_Model.MRPDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, MRPDate3) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditDomesticMRPDetails", new { MRPNumber = _MaterialRequirementPlan_Model.MRPNumber, MRPDate = _MaterialRequirementPlan_Model.MRPDate, ListFilterData = _MaterialRequirementPlan_Model.ListFilterData1, WF_Status = _MaterialRequirementPlan_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Commandfc"] = command;
                        //Session["MRPNumberfc"] = _MaterialRequirementPlan_Model.MRPNumber;
                        Approve_MRPDetails(_MaterialRequirementPlan_Model, command, "", "");
                        TempData["ModelData"] = _MaterialRequirementPlan_Model;
                        UrlModel ApproveModel = new UrlModel();
                        ApproveModel.tp = "Update";
                        ApproveModel.MRP_No = _MaterialRequirementPlan_Model.MRPNumberfc;
                        ApproveModel.MRP_Date = _MaterialRequirementPlan_Model.MRPDatefc;
                        ApproveModel.bt = "BtnEdit";
                        ApproveModel.Amend = "";
                        TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                        return RedirectToAction("MaterialRequirementPlanDetail", ApproveModel);

                    case "Refresh":
                        //Session["BtnNamefc"] = "BtnRefresh";
                        //Session["Commandfc"] = command;
                        //Session["TransTypefc"] = "Save";
                        //Session["Messagefc"] = null;
                        //Session["DocumentStatusfc"] = null;
                        MaterialRequirementPlan_Model RefreshModel = new MaterialRequirementPlan_Model();
                        RefreshModel.Commandfc = command;
                        RefreshModel.BtnNamefc = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatusfc = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh_Model = new UrlModel();
                        refesh_Model.tp = "Save";
                        refesh_Model.bt = "BtnRefresh";
                        refesh_Model.Cmd = command;
                        TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                        return RedirectToAction("MaterialRequirementPlanDetail", refesh_Model);

                    case "Print":
                        return GenratePdfFile(_MaterialRequirementPlan_Model);
                    case "BacktoList":
                        //Session.Remove("Messagefc");
                        //Session.Remove("TransTypefc");
                        //Session.Remove("Commandfc");
                        //Session.Remove("BtnNamefc");
                        //Session.Remove("DocumentStatusfc");
                        MaterialRequirementPlan_Model _Backtolist = new MaterialRequirementPlan_Model();
                        // _Backtolist.WF_Status = _MaterialRequirementPlan_Model.WF_Status1;
                        var WF_Status = _MaterialRequirementPlan_Model.WF_Status1;
                        TempData["ListFilterData"] = _MaterialRequirementPlan_Model.ListFilterData1;
                        return RedirectToAction("MaterialRequirementPlan", new { WF_Status });

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
        public ActionResult SaveUpdateMRP_Details(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            try
            {
                if (_MaterialRequirementPlan_Model.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }

                    DataTable MRPHeader = new DataTable();
                    DataTable MRPItemDetails = new DataTable();
                    DataTable SFMaterialDetails = new DataTable();
                    DataTable InputMaterialDetails = new DataTable();
                    DataTable HdnSFmaterial = new DataTable();
                    DataTable HdnRMdetails = new DataTable();

                    MRPHeader = TblHeaderDetails(_MaterialRequirementPlan_Model);
                    MRPItemDetails = ProductDetailTable(_MaterialRequirementPlan_Model.ProductDetail);
                    HdnSFmaterial = HdnSFMaterialTable(_MaterialRequirementPlan_Model.hdnSFMaterialDetail);
                    HdnRMdetails = HdnRMTable(_MaterialRequirementPlan_Model.hdnInputMaterialDetail);
                    SFMaterialDetails = SFMaterialTable(_MaterialRequirementPlan_Model.SFMaterialDetail);
                    InputMaterialDetails = InputMaterialDetailTable(_MaterialRequirementPlan_Model.InputMaterialDetail);

                    /*------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    DataTable dtSubItem_precure = new DataTable(); /**Added By Nitesh 16-05-2024 For Order to Precure **/
                    dtSubItem = SubItemDetailsDtTable(_MaterialRequirementPlan_Model.SubItemDetailsDt);

                    dtSubItem_precure = SubItemDetailsDtTable(_MaterialRequirementPlan_Model.SubItemDetailsDt_prcure);/**Added By Nitesh 16-05-2024 For Order to Precure **/
                    //DataTable dtSFSubItem = new DataTable();
                    //dtSFSubItem = SFSubItemDetailsDtTable(_MaterialRequirementPlan_Model.SFSubItemDetailsDt);
                    /*------------------Sub Item end----------------------*/

                    string SaveMessage = _MRP_ISERVICES.InsertUpdateMRPDetails(MRPHeader, MRPItemDetails, InputMaterialDetails, SFMaterialDetails, HdnSFmaterial, HdnRMdetails, dtSubItem/*, dtSFSubItem*/, dtSubItem_precure);
                    string MRPNumber = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                    if (Message == "Data_Not_Found")
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _MaterialRequirementPlan_Model.Title = title;

                        var a = MRPNumber.Split('-');
                        var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + _MaterialRequirementPlan_Model.Title;//ContraNo is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _MaterialRequirementPlan_Model.Messagefc = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("MaterialRequirementPlanDetail");
                    }
                    if (Message == "NotModified")
                    {
                        /****** This Massage Created By Nitesh 14-09-2023 
                         used in New Data Insert by use src_doc_no or Item_Name  When add data in table and other User Cancel or Force
                        this  (src_doc_no or Item Name) Document in refrence Page When Data is Not Insert and Give massage NotModified
                        and retun to case(Save) and Show Data User insert but Not Save **********/
                        _MaterialRequirementPlan_Model.Messagefc = "DocModify";
                        _MaterialRequirementPlan_Model.BtnNamefc = "BtnRefresh";
                        _MaterialRequirementPlan_Model.Commandfc = "Refresh";
                        _MaterialRequirementPlan_Model.TransTypefc = "Refresh";
                        ViewData["ItemData"] = NotModified_MRPProductDetailTable(_MaterialRequirementPlan_Model); /*All ViewData is Store Table data When Massage NotModified*/
                        ViewData["Subitem"] = Notmodified_SubItemDetailsDtTable(_MaterialRequirementPlan_Model);
                        ViewData["Rawmate_itmDtl"] = NotModified_Raw_MaterialDetailTable(_MaterialRequirementPlan_Model);
                        ViewData["HdnSFMaterialDetail"] = not_Modified_HdnSFMaterialTable(_MaterialRequirementPlan_Model);
                        ViewData["hdn_Raw_MaterialDetail"] = Not_Modified_HdnRMTable(_MaterialRequirementPlan_Model);
                        ViewData["SFItemTable"] = Not_Modified_SFMaterialTable(_MaterialRequirementPlan_Model);
                        ViewData["Sub_item_procure"] = Notmodified_SubItemDetailsDtTable_precure(_MaterialRequirementPlan_Model);
                        return RedirectToAction("MaterialRequirementPlanDetail");
                    }

                    if (Message == "Update" || Message == "Save")
                        _MaterialRequirementPlan_Model.Messagefc = "Save";
                    _MaterialRequirementPlan_Model.MRPNumberfc = MRPNumber;
                    _MaterialRequirementPlan_Model.TransTypefc = "Update";
                    _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
                    return RedirectToAction("MaterialRequirementPlanDetail");
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
                    string mrpno = _MaterialRequirementPlan_Model.MRPNumber;
                    string mrpdate = _MaterialRequirementPlan_Model.MRPDate;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    string SaveMessage = _MRP_ISERVICES.Cancelled_MRPDetail(CompID, br_id, mrpno, mrpdate, userid, mac_id, DocumentMenuId);
                    //string MRPNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    if (SaveMessage == "JCInProcess")
                    {
                        _MaterialRequirementPlan_Model.Messagefc = "JCInProcess";
                        _MaterialRequirementPlan_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                        _MaterialRequirementPlan_Model.TransTypefc = "Update";
                        _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
                        _MaterialRequirementPlan_Model.Commandfc = "Refresh";
                    }
                    else if (SaveMessage == "PRInProcess")
                    {
                        _MaterialRequirementPlan_Model.Messagefc = "PRInProcess";
                        _MaterialRequirementPlan_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                        _MaterialRequirementPlan_Model.TransTypefc = "Update";
                        _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
                        _MaterialRequirementPlan_Model.Commandfc = "Refresh";
                    }
                    else
                    {
                        try
                        {
                           // string fileName = "MRP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "MaterialRequirementPlan_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_MaterialRequirementPlan_Model.MRPNumber, _MaterialRequirementPlan_Model.MRPDate, fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MaterialRequirementPlan_Model.MRPNumber, "C", userid, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _MaterialRequirementPlan_Model.Messagefc = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _MaterialRequirementPlan_Model.Messagefc = _MaterialRequirementPlan_Model.Messagefc == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        // _MaterialRequirementPlan_Model.Messagefc = "Cancelled";
                        _MaterialRequirementPlan_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                        _MaterialRequirementPlan_Model.TransTypefc = "Update";
                        _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
                        _MaterialRequirementPlan_Model.Commandfc = "Update";
                    }
                    return RedirectToAction("MaterialRequirementPlanDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private DataTable TblHeaderDetails(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            DataTable dtheader = new DataTable();
            dtheader.Columns.Add("MenuDocumentId", typeof(string));
            dtheader.Columns.Add("TransType", typeof(string));
            dtheader.Columns.Add("comp_id", typeof(string));
            dtheader.Columns.Add("br_id", typeof(string));
            dtheader.Columns.Add("mrp_no", typeof(string));
            dtheader.Columns.Add("mrp_dt", typeof(string));

            dtheader.Columns.Add("fy", typeof(string));
            dtheader.Columns.Add("period", typeof(string));
            dtheader.Columns.Add("from_date", typeof(string));
            dtheader.Columns.Add("to_date", typeof(string));

            dtheader.Columns.Add("pr_no", typeof(string));

            dtheader.Columns.Add("src_type", typeof(string));
            dtheader.Columns.Add("src_doc_no", typeof(string));
            dtheader.Columns.Add("src_doc_date", typeof(string));
            dtheader.Columns.Add("mrp_status", typeof(string));
            dtheader.Columns.Add("user_id", typeof(string));
            dtheader.Columns.Add("mac_id", typeof(string));
            dtheader.Columns.Add("req_area", typeof(int));

            DataRow dtHeaderrow = dtheader.NewRow();
            dtHeaderrow["MenuDocumentId"] = DocumentMenuId;
            // dtHeaderrow["TransType"] = Session["TransTypefc"].ToString();
            if (_MaterialRequirementPlan_Model.MRPNumber != null)
            {
                dtHeaderrow["TransType"] = "Update";
            }
            else
            {
                dtHeaderrow["TransType"] = "Save";
            }
            if (_MaterialRequirementPlan_Model.Amend != null && _MaterialRequirementPlan_Model.Amend != "" && _MaterialRequirementPlan_Model.Amendment == null)
            {
                dtHeaderrow["TransType"] = "Amendment";
            }
            dtHeaderrow["comp_id"] = Session["CompId"].ToString();
            dtHeaderrow["br_id"] = Session["BranchId"].ToString();
            dtHeaderrow["mrp_no"] = _MaterialRequirementPlan_Model.MRPNumber;
            dtHeaderrow["mrp_dt"] = _MaterialRequirementPlan_Model.MRPDate;
            dtHeaderrow["fy"] = _MaterialRequirementPlan_Model.ddlfinancialyear;
            dtHeaderrow["period"] = _MaterialRequirementPlan_Model.ddlperiod;
            dtHeaderrow["from_date"] = (_MaterialRequirementPlan_Model.ddlsrctype == "A" || _MaterialRequirementPlan_Model.src_doc_type == "A") ? _MaterialRequirementPlan_Model.AdHocFromDate : _MaterialRequirementPlan_Model.hfFromDate;
            dtHeaderrow["to_date"] = (_MaterialRequirementPlan_Model.ddlsrctype == "A" || _MaterialRequirementPlan_Model.src_doc_type == "A") ? _MaterialRequirementPlan_Model.AdHocToDate : _MaterialRequirementPlan_Model.hfToDate;

            dtHeaderrow["pr_no"] = _MaterialRequirementPlan_Model.RequisitionNumber;
            dtHeaderrow["src_type"] = _MaterialRequirementPlan_Model.ddlsrctype;
            dtHeaderrow["src_doc_no"] = _MaterialRequirementPlan_Model.PP_Number;
            dtHeaderrow["src_doc_date"] = _MaterialRequirementPlan_Model.PP_Date;
            dtHeaderrow["mrp_status"] = "D";
            dtHeaderrow["user_id"] = Session["UserId"].ToString();
            string SystemDetail = string.Empty;
            SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
            dtHeaderrow["mac_id"] = SystemDetail;
            dtHeaderrow["req_area"] = _MaterialRequirementPlan_Model.reqarea;
            dtheader.Rows.Add(dtHeaderrow);
            return dtheader;
        }
        private DataTable SubItemDetailsDtTable(string SubItemDetailsDt)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));
            dtSubItem.Columns.Add("item_type", typeof(string));
            if (SubItemDetailsDt != null)
            {
                JArray jObject2 = JArray.Parse(SubItemDetailsDt);
                for (int i = 0; i < jObject2.Count; i++)
                {
                    DataRow dtrowItemdetails = dtSubItem.NewRow();
                    dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                    dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                    dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                    dtrowItemdetails["item_type"] = jObject2[i]["ItemType"].ToString();
                    dtSubItem.Rows.Add(dtrowItemdetails);
                }
            }
            return dtSubItem;
        }
        private DataTable SFSubItemDetailsDtTable(string SFSubItemDetailsDt)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));
            if (SFSubItemDetailsDt != null)
            {
                JArray jObject2 = JArray.Parse(SFSubItemDetailsDt);
                for (int i = 0; i < jObject2.Count; i++)
                {
                    DataRow dtrowItemdetails = dtSubItem.NewRow();
                    dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                    dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                    dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                    dtSubItem.Rows.Add(dtrowItemdetails);
                }
            }
            return dtSubItem;
        }
        private DataTable RMSubItemDetailsDtTable(string RMSubItemDetailsDt)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));
            if (RMSubItemDetailsDt != null)
            {
                JArray jObject2 = JArray.Parse(RMSubItemDetailsDt);
                for (int i = 0; i < jObject2.Count; i++)
                {
                    DataRow dtrowItemdetails = dtSubItem.NewRow();
                    dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                    dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                    dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                    dtSubItem.Rows.Add(dtrowItemdetails);
                }
            }
            return dtSubItem;
        }
        private DataTable Notmodified_SubItemDetailsDtTable(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            /*This Table is Used in when save data and Error Massage is NotModified (Created by Nitesh 14-09-2023)*/
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));

            JArray jObject2 = JArray.Parse(_MaterialRequirementPlan_Model.SubItemDetailsDt);
            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }

            return dtSubItem;
        }
        private DataTable Notmodified_SubItemDetailsDtTable_precure(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            /*This Table is Used in when save data and Error Massage is NotModified (Created by Nitesh 14-09-2023)*/
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));

            JArray jObject2 = JArray.Parse(_MaterialRequirementPlan_Model.SubItemDetailsDt_prcure);
            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }

            return dtSubItem;
        }
        private DataTable ProductDetailTable(string ProductDetail)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(string));
            dtItem.Columns.Add("forecast_qty", typeof(string));
            dtItem.Columns.Add("plan_qty", typeof(string));
            dtItem.Columns.Add("remarks", typeof(string));
            dtItem.Columns.Add("avl_stk", typeof(string));
            dtItem.Columns.Add("sr_no", typeof(string));

            JArray jObject = JArray.Parse(ProductDetail);
            for (int i = 0; i < jObject.Count; i++)
            {
                decimal planned_qty;
                if (jObject[i]["PlannedQuantity"].ToString() == "")
                {
                    planned_qty = 0;
                }
                else
                {
                    planned_qty = Convert.ToDecimal(jObject[i]["PlannedQuantity"].ToString());
                }

                DataRow dtrowItemdetails = dtItem.NewRow();
                dtrowItemdetails["item_id"] = jObject[i]["ProductId"].ToString();
                dtrowItemdetails["uom_id"] = jObject[i]["UOMId"].ToString();
                dtrowItemdetails["forecast_qty"] = jObject[i]["ForecastQuantity"].ToString();
                dtrowItemdetails["plan_qty"] = planned_qty;
                dtrowItemdetails["remarks"] = jObject[i]["remarks"].ToString();
                dtrowItemdetails["avl_stk"] = jObject[i]["avl_stk"].ToString();
                dtrowItemdetails["sr_no"] = jObject[i]["SrNo"].ToString();
                dtItem.Rows.Add(dtrowItemdetails);
            }
            return dtItem;
        }
        private DataTable NotModified_MRPProductDetailTable(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            /*This Table is Used in when save data and Error Massage is NotModified (Created by Nitesh 14-09-2023)*/
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(string));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("forecast_qty", typeof(string));
            dtItem.Columns.Add("plan_qty", typeof(string));
            dtItem.Columns.Add("remarks", typeof(string));
            dtItem.Columns.Add("AvlStock", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("prod_qty", typeof(string));

            JArray jObject = JArray.Parse(_MaterialRequirementPlan_Model.ProductDetail);
            for (int i = 0; i < jObject.Count; i++)
            {
                decimal planned_qty;
                if (jObject[i]["PlannedQuantity"].ToString() == "")
                {
                    planned_qty = 0;
                }
                else
                {
                    planned_qty = Convert.ToDecimal(jObject[i]["PlannedQuantity"].ToString());
                }

                DataRow dtrowItemdetails = dtItem.NewRow();
                dtrowItemdetails["item_id"] = jObject[i]["ProductId"].ToString();
                dtrowItemdetails["item_name"] = jObject[i]["item_name"].ToString();
                dtrowItemdetails["uom_id"] = jObject[i]["UOMId"].ToString();
                dtrowItemdetails["uom_name"] = jObject[i]["uom_name"].ToString();
                dtrowItemdetails["forecast_qty"] = jObject[i]["ForecastQuantity"].ToString();
                dtrowItemdetails["plan_qty"] = planned_qty;
                dtrowItemdetails["remarks"] = jObject[i]["remarks"].ToString();
                dtrowItemdetails["AvlStock"] = jObject[i]["avl_stk"].ToString();
                dtrowItemdetails["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowItemdetails["prod_qty"] = jObject[i]["ProduceQuantity"].ToString();
                dtItem.Rows.Add(dtrowItemdetails);
            }
            return dtItem;
        }
        private DataTable NotModified_Raw_MaterialDetailTable(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            /*This Table is Used in when save data and Error Massage is NotModified (Created by Nitesh 14-09-2023)*/
            DataTable inputmaterial_detail = new DataTable();
            inputmaterial_detail.Columns.Add("item_id", typeof(string));
            inputmaterial_detail.Columns.Add("item_name", typeof(string));
            inputmaterial_detail.Columns.Add("uom_id", typeof(string));
            inputmaterial_detail.Columns.Add("uom_name", typeof(string));
            inputmaterial_detail.Columns.Add("bom_qty", typeof(string));
            inputmaterial_detail.Columns.Add("minresstk", typeof(string));
            inputmaterial_detail.Columns.Add("req_qty", typeof(string));
            inputmaterial_detail.Columns.Add("pend_ord_qty", typeof(string));
            inputmaterial_detail.Columns.Add("pr_qty", typeof(string));
            inputmaterial_detail.Columns.Add("proc_dt", typeof(string));
            inputmaterial_detail.Columns.Add("AvlStock", typeof(string));
            inputmaterial_detail.Columns.Add("shfl_stk", typeof(string));
            inputmaterial_detail.Columns.Add("in_trans_stk", typeof(string));
            inputmaterial_detail.Columns.Add("procured_qty", typeof(string));

            JArray jObjectBatch = JArray.Parse(_MaterialRequirementPlan_Model.InputMaterialDetail);
            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowinputmaterialdetailLines = inputmaterial_detail.NewRow();
                dtrowinputmaterialdetailLines["item_id"] = jObjectBatch[i]["materialid"].ToString();
                dtrowinputmaterialdetailLines["item_name"] = jObjectBatch[i]["MaterialName"].ToString();
                dtrowinputmaterialdetailLines["uom_id"] = jObjectBatch[i]["uomid"].ToString();
                dtrowinputmaterialdetailLines["uom_name"] = jObjectBatch[i]["uom_name"].ToString();
                dtrowinputmaterialdetailLines["bom_qty"] = jObjectBatch[i]["bomqty"].ToString();
                dtrowinputmaterialdetailLines["minresstk"] = jObjectBatch[i]["minrevstk"].ToString();
                dtrowinputmaterialdetailLines["req_qty"] = jObjectBatch[i]["requiredqty"].ToString();
                dtrowinputmaterialdetailLines["pend_ord_qty"] = jObjectBatch[i]["pendingorederqty"].ToString();
                dtrowinputmaterialdetailLines["pr_qty"] = jObjectBatch[i]["requisitionqty"].ToString();
                dtrowinputmaterialdetailLines["proc_dt"] = jObjectBatch[i]["procurementdate"].ToString();
                dtrowinputmaterialdetailLines["AvlStock"] = jObjectBatch[i]["avl_stk"].ToString();
                dtrowinputmaterialdetailLines["shfl_stk"] = jObjectBatch[i]["ShopFloorstock"].ToString();
                dtrowinputmaterialdetailLines["in_trans_stk"] = jObjectBatch[i]["INtransit"].ToString();
                dtrowinputmaterialdetailLines["procured_qty"] = jObjectBatch[i]["ProcuredQty"].ToString();
                inputmaterial_detail.Rows.Add(dtrowinputmaterialdetailLines);
            }



            return inputmaterial_detail;
        }
        private DataTable InputMaterialDetailTable(string InputMaterialDetail)
        {
            DataTable inputmaterial_detail = new DataTable();
            inputmaterial_detail.Columns.Add("item_id", typeof(string));
            inputmaterial_detail.Columns.Add("uom_id", typeof(string));
            inputmaterial_detail.Columns.Add("bom_qty", typeof(string));
            inputmaterial_detail.Columns.Add("min_res_stk", typeof(string));
            inputmaterial_detail.Columns.Add("req_qty", typeof(string));
            inputmaterial_detail.Columns.Add("pend_ord_qty", typeof(string));
            inputmaterial_detail.Columns.Add("pr_qty", typeof(string));
            inputmaterial_detail.Columns.Add("procure_date", typeof(string));
            inputmaterial_detail.Columns.Add("avl_stk", typeof(string));
            inputmaterial_detail.Columns.Add("shfl_stk", typeof(string));
            inputmaterial_detail.Columns.Add("in_trans_stk", typeof(string));

            if (InputMaterialDetail != null)
            {
                JArray jObjectBatch = JArray.Parse(InputMaterialDetail);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    DataRow dtrowinputmaterialdetailLines = inputmaterial_detail.NewRow();
                    dtrowinputmaterialdetailLines["item_id"] = jObjectBatch[i]["materialid"].ToString();
                    dtrowinputmaterialdetailLines["uom_id"] = jObjectBatch[i]["uomid"].ToString();
                    dtrowinputmaterialdetailLines["bom_qty"] = jObjectBatch[i]["bomqty"].ToString();
                    dtrowinputmaterialdetailLines["min_res_stk"] = jObjectBatch[i]["minrevstk"].ToString();
                    dtrowinputmaterialdetailLines["req_qty"] = jObjectBatch[i]["requiredqty"].ToString();
                    dtrowinputmaterialdetailLines["pend_ord_qty"] = jObjectBatch[i]["pendingorederqty"].ToString();
                    dtrowinputmaterialdetailLines["pr_qty"] = jObjectBatch[i]["requisitionqty"].ToString();
                    dtrowinputmaterialdetailLines["procure_date"] = jObjectBatch[i]["procurementdate"].ToString();
                    dtrowinputmaterialdetailLines["avl_stk"] = jObjectBatch[i]["avl_stk"].ToString();
                    dtrowinputmaterialdetailLines["shfl_stk"] = jObjectBatch[i]["ShopFloorstock"].ToString();
                    dtrowinputmaterialdetailLines["in_trans_stk"] = jObjectBatch[i]["INtransit"].ToString();
                    inputmaterial_detail.Rows.Add(dtrowinputmaterialdetailLines);
                }
            }
            return inputmaterial_detail;
        }

        public DataTable SFMaterialTable(string SFMaterialdtl)
        {
            DataTable SFmaterial_detail = new DataTable();
            SFmaterial_detail.Columns.Add("item_id", typeof(string));
            SFmaterial_detail.Columns.Add("fg_item_id", typeof(string));
            SFmaterial_detail.Columns.Add("bom_item_id", typeof(string));
            SFmaterial_detail.Columns.Add("item_type", typeof(string));
            SFmaterial_detail.Columns.Add("uom_id", typeof(string));
            SFmaterial_detail.Columns.Add("bom_qty", typeof(string));
            SFmaterial_detail.Columns.Add("avl_stk", typeof(string));
            SFmaterial_detail.Columns.Add("wip_stk", typeof(string));
            SFmaterial_detail.Columns.Add("shfl_stk", typeof(string));
            SFmaterial_detail.Columns.Add("in_trans_stk", typeof(string));
            SFmaterial_detail.Columns.Add("req_qty", typeof(string));
            SFmaterial_detail.Columns.Add("pend_ord_qty", typeof(string));
            SFmaterial_detail.Columns.Add("ord_proc_qty", typeof(string));
            SFmaterial_detail.Columns.Add("proc_dt", typeof(string));
            SFmaterial_detail.Columns.Add("ord_prod_qty", typeof(string));
            SFmaterial_detail.Columns.Add("prod_dt", typeof(string));
            SFmaterial_detail.Columns.Add("sr_no", typeof(string));

            if (SFMaterialdtl != null)
            {
                JArray jObjectBatch = JArray.Parse(SFMaterialdtl);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    DataRow dtrowSFmaterialdetailLines = SFmaterial_detail.NewRow();
                    dtrowSFmaterialdetailLines["item_id"] = jObjectBatch[i]["materialid"].ToString();
                    dtrowSFmaterialdetailLines["fg_item_id"] = jObjectBatch[i]["fg_item_id"].ToString();
                    dtrowSFmaterialdetailLines["bom_item_id"] = jObjectBatch[i]["bom_item_id"].ToString();
                    dtrowSFmaterialdetailLines["item_type"] = jObjectBatch[i]["item_type"].ToString();
                    dtrowSFmaterialdetailLines["uom_id"] = jObjectBatch[i]["uomid"].ToString();
                    dtrowSFmaterialdetailLines["bom_qty"] = jObjectBatch[i]["bomqty"].ToString();
                    dtrowSFmaterialdetailLines["avl_stk"] = jObjectBatch[i]["AvlStock"].ToString();
                    dtrowSFmaterialdetailLines["wip_stk"] = jObjectBatch[i]["WIPstock"].ToString();
                    dtrowSFmaterialdetailLines["shfl_stk"] = jObjectBatch[i]["ShopFloorstock"].ToString();
                    dtrowSFmaterialdetailLines["in_trans_stk"] = jObjectBatch[i]["INtransit"].ToString();
                    dtrowSFmaterialdetailLines["req_qty"] = jObjectBatch[i]["RequiredQuantity"].ToString();
                    dtrowSFmaterialdetailLines["pend_ord_qty"] = jObjectBatch[i]["pend_ord_qty"].ToString();
                    dtrowSFmaterialdetailLines["ord_proc_qty"] = jObjectBatch[i]["OrderToProcureQuantity"].ToString();
                    dtrowSFmaterialdetailLines["proc_dt"] = jObjectBatch[i]["ProcurementCompletionDate"].ToString();
                    dtrowSFmaterialdetailLines["ord_prod_qty"] = jObjectBatch[i]["OrderToProduceQuantity"].ToString();
                    dtrowSFmaterialdetailLines["prod_dt"] = jObjectBatch[i]["ProductionCompletionDate"].ToString();
                    dtrowSFmaterialdetailLines["sr_no"] = jObjectBatch[i]["sr_no"].ToString();
                    SFmaterial_detail.Rows.Add(dtrowSFmaterialdetailLines);

                }
            }
            return SFmaterial_detail;
        }

        public DataTable Not_Modified_SFMaterialTable(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            /*This Table is Used in when save data and Error Massage is NotModified (Created by Nitesh 14-09-2023)*/
            DataTable SFmaterial_detail = new DataTable();
            SFmaterial_detail.Columns.Add("item_id", typeof(string));
            SFmaterial_detail.Columns.Add("item_name", typeof(string));
            SFmaterial_detail.Columns.Add("fg_item_id", typeof(string));
            SFmaterial_detail.Columns.Add("bom_item_id", typeof(string));
            SFmaterial_detail.Columns.Add("item_type", typeof(string));
            SFmaterial_detail.Columns.Add("uom_id", typeof(string));
            SFmaterial_detail.Columns.Add("uom_name", typeof(string));
            SFmaterial_detail.Columns.Add("bom_qty", typeof(string));
            SFmaterial_detail.Columns.Add("AvlStock", typeof(string));
            SFmaterial_detail.Columns.Add("wip_stk", typeof(string));
            SFmaterial_detail.Columns.Add("shfl_stk", typeof(string));
            SFmaterial_detail.Columns.Add("in_trans_stk", typeof(string));
            SFmaterial_detail.Columns.Add("req_qty", typeof(string));
            SFmaterial_detail.Columns.Add("pend_ord_qty", typeof(string));
            SFmaterial_detail.Columns.Add("ord_proc_qty", typeof(string));
            SFmaterial_detail.Columns.Add("proc_dt", typeof(string));
            SFmaterial_detail.Columns.Add("ord_prod_qty", typeof(string));
            SFmaterial_detail.Columns.Add("prod_dt", typeof(string));
            SFmaterial_detail.Columns.Add("procured_qty", typeof(string));
            SFmaterial_detail.Columns.Add("produced_qty", typeof(string));

            if (_MaterialRequirementPlan_Model.SFMaterialDetail != null)
            {
                JArray jObjectBatch = JArray.Parse(_MaterialRequirementPlan_Model.SubItemDetailsDt_prcure);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    DataRow dtrowSFmaterialdetailLines = SFmaterial_detail.NewRow();
                    dtrowSFmaterialdetailLines["item_id"] = jObjectBatch[i]["materialid"].ToString();
                    dtrowSFmaterialdetailLines["item_name"] = jObjectBatch[i]["MaterialName"].ToString();
                    dtrowSFmaterialdetailLines["fg_item_id"] = jObjectBatch[i]["fg_item_id"].ToString();
                    dtrowSFmaterialdetailLines["bom_item_id"] = jObjectBatch[i]["bom_item_id"].ToString();
                    dtrowSFmaterialdetailLines["item_type"] = jObjectBatch[i]["item_type"].ToString();
                    dtrowSFmaterialdetailLines["uom_id"] = jObjectBatch[i]["uomid"].ToString();
                    dtrowSFmaterialdetailLines["uom_name"] = jObjectBatch[i]["uom_name"].ToString();
                    dtrowSFmaterialdetailLines["bom_qty"] = jObjectBatch[i]["bomqty"].ToString();
                    dtrowSFmaterialdetailLines["AvlStock"] = jObjectBatch[i]["AvlStock"].ToString();
                    dtrowSFmaterialdetailLines["wip_stk"] = jObjectBatch[i]["WIPstock"].ToString();
                    dtrowSFmaterialdetailLines["shfl_stk"] = jObjectBatch[i]["ShopFloorstock"].ToString();
                    dtrowSFmaterialdetailLines["in_trans_stk"] = jObjectBatch[i]["INtransit"].ToString();
                    dtrowSFmaterialdetailLines["req_qty"] = jObjectBatch[i]["RequiredQuantity"].ToString();
                    dtrowSFmaterialdetailLines["pend_ord_qty"] = jObjectBatch[i]["pend_ord_qty"].ToString();
                    dtrowSFmaterialdetailLines["ord_proc_qty"] = jObjectBatch[i]["OrderToProcureQuantity"].ToString();
                    dtrowSFmaterialdetailLines["proc_dt"] = jObjectBatch[i]["ProcurementCompletionDate"].ToString();
                    dtrowSFmaterialdetailLines["ord_prod_qty"] = jObjectBatch[i]["OrderToProduceQuantity"].ToString();
                    dtrowSFmaterialdetailLines["prod_dt"] = jObjectBatch[i]["ProductionCompletionDate"].ToString();
                    dtrowSFmaterialdetailLines["procured_qty"] = jObjectBatch[i]["ProcuredQty"].ToString();
                    dtrowSFmaterialdetailLines["produced_qty"] = jObjectBatch[i]["ProducedQty"].ToString();
                    SFmaterial_detail.Rows.Add(dtrowSFmaterialdetailLines);

                }
            }
            return SFmaterial_detail;
        }
        public DataTable not_Modified_HdnSFMaterialTable(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            /*This Table is Used in when save data and Error Massage is NotModified (Created by Nitesh 14-09-2023)
             This table is hidden table in view and pass data in view when Error Massage is NotModified  */
            DataTable HdnSFmaterial_detail = new DataTable();
            HdnSFmaterial_detail.Columns.Add("row_id", typeof(string));
            HdnSFmaterial_detail.Columns.Add("fg_item_id", typeof(string));
            HdnSFmaterial_detail.Columns.Add("sf_item_id", typeof(string));
            HdnSFmaterial_detail.Columns.Add("bom_qty", typeof(string));
            HdnSFmaterial_detail.Columns.Add("req_qty", typeof(string));


            if (_MaterialRequirementPlan_Model.hdnSFMaterialDetail != null)
            {
                JArray jObjectBatch = JArray.Parse(_MaterialRequirementPlan_Model.hdnSFMaterialDetail);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    DataRow dtrowSFmaterialdetailLines = HdnSFmaterial_detail.NewRow();
                    dtrowSFmaterialdetailLines["row_id"] = jObjectBatch[i]["fg_rowno"].ToString();
                    dtrowSFmaterialdetailLines["fg_item_id"] = jObjectBatch[i]["fg_ItemId"].ToString();
                    dtrowSFmaterialdetailLines["sf_item_id"] = jObjectBatch[i]["fg_Sf_ItemID"].ToString();
                    dtrowSFmaterialdetailLines["bom_qty"] = jObjectBatch[i]["fg_BomQty"].ToString();
                    dtrowSFmaterialdetailLines["req_qty"] = jObjectBatch[i]["fg_ReqQty"].ToString();
                    HdnSFmaterial_detail.Rows.Add(dtrowSFmaterialdetailLines);

                }
            }
            return HdnSFmaterial_detail;
        }
        public DataTable Not_Modified_HdnRMTable(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model)
        {
            /*This Table is Used in when save data and Error Massage is NotModified (Created by Nitesh 14-09-2023)
              This table is hidden table in view and pass data in view when Error Massage is NotModified */
            DataTable HdnRMdtl_detail = new DataTable();
            HdnRMdtl_detail.Columns.Add("row_id", typeof(string));
            HdnRMdtl_detail.Columns.Add("fg_item_id", typeof(string));
            HdnRMdtl_detail.Columns.Add("sf_item_id", typeof(string));
            HdnRMdtl_detail.Columns.Add("rm_item_id", typeof(string));
            HdnRMdtl_detail.Columns.Add("bom_qty", typeof(string));
            HdnRMdtl_detail.Columns.Add("req_qty", typeof(string));


            if (_MaterialRequirementPlan_Model.hdnInputMaterialDetail != null)
            {
                JArray jObjectBatch = JArray.Parse(_MaterialRequirementPlan_Model.hdnInputMaterialDetail);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    DataRow dtrowHdnRMdtldetailLines = HdnRMdtl_detail.NewRow();
                    dtrowHdnRMdtldetailLines["row_id"] = jObjectBatch[i]["fg_rowno"].ToString();
                    dtrowHdnRMdtldetailLines["fg_item_id"] = jObjectBatch[i]["fg_ItemId"].ToString();
                    dtrowHdnRMdtldetailLines["sf_item_id"] = jObjectBatch[i]["Sf_ItemID"].ToString();
                    dtrowHdnRMdtldetailLines["rm_item_id"] = jObjectBatch[i]["RM_ItemID"].ToString();
                    dtrowHdnRMdtldetailLines["bom_qty"] = jObjectBatch[i]["RM_BomQty"].ToString();
                    dtrowHdnRMdtldetailLines["req_qty"] = jObjectBatch[i]["RM_ReqQty"].ToString();
                    HdnRMdtl_detail.Rows.Add(dtrowHdnRMdtldetailLines);
                }
            }
            return HdnRMdtl_detail;
        }
        public DataTable HdnSFMaterialTable(string HdnSFMaterialdtl)
        {

            DataTable HdnSFmaterial_detail = new DataTable();
            HdnSFmaterial_detail.Columns.Add("row_id", typeof(string));
            HdnSFmaterial_detail.Columns.Add("fg_item_id", typeof(string));
            HdnSFmaterial_detail.Columns.Add("sf_item_id", typeof(string));
            HdnSFmaterial_detail.Columns.Add("bom_qty", typeof(string));
            HdnSFmaterial_detail.Columns.Add("req_qty", typeof(string));
            HdnSFmaterial_detail.Columns.Add("parent_sf_item_id", typeof(string));
            HdnSFmaterial_detail.Columns.Add("fg_sf_item_type", typeof(string));


            if (HdnSFMaterialdtl != null)
            {
                JArray jObjectBatch = JArray.Parse(HdnSFMaterialdtl);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    DataRow dtrowSFmaterialdetailLines = HdnSFmaterial_detail.NewRow();
                    dtrowSFmaterialdetailLines["row_id"] = jObjectBatch[i]["fg_rowno"].ToString();
                    dtrowSFmaterialdetailLines["fg_item_id"] = jObjectBatch[i]["fg_ItemId"].ToString();
                    dtrowSFmaterialdetailLines["sf_item_id"] = jObjectBatch[i]["fg_Sf_ItemID"].ToString();
                    dtrowSFmaterialdetailLines["bom_qty"] = jObjectBatch[i]["fg_BomQty"].ToString();
                    dtrowSFmaterialdetailLines["req_qty"] = jObjectBatch[i]["fg_ReqQty"].ToString();
                    dtrowSFmaterialdetailLines["parent_sf_item_id"] = jObjectBatch[i]["parent_sf_item_id"].ToString();
                    dtrowSFmaterialdetailLines["fg_sf_item_type"] = jObjectBatch[i]["fg_sf_item_type"].ToString();
                    HdnSFmaterial_detail.Rows.Add(dtrowSFmaterialdetailLines);

                }
            }
            return HdnSFmaterial_detail;
        }
        public DataTable HdnRMTable(string HdnRMdtl)
        {
            DataTable HdnRMdtl_detail = new DataTable();
            HdnRMdtl_detail.Columns.Add("row_id", typeof(string));
            HdnRMdtl_detail.Columns.Add("fg_item_id", typeof(string));
            HdnRMdtl_detail.Columns.Add("sf_item_id", typeof(string));
            HdnRMdtl_detail.Columns.Add("rm_item_id", typeof(string));
            HdnRMdtl_detail.Columns.Add("bom_qty", typeof(string));
            HdnRMdtl_detail.Columns.Add("req_qty", typeof(string));


            if (HdnRMdtl != null)
            {
                JArray jObjectBatch = JArray.Parse(HdnRMdtl);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    DataRow dtrowHdnRMdtldetailLines = HdnRMdtl_detail.NewRow();
                    dtrowHdnRMdtldetailLines["row_id"] = jObjectBatch[i]["fg_rowno"].ToString();
                    dtrowHdnRMdtldetailLines["fg_item_id"] = jObjectBatch[i]["fg_ItemId"].ToString();
                    dtrowHdnRMdtldetailLines["sf_item_id"] = jObjectBatch[i]["Sf_ItemID"].ToString();
                    dtrowHdnRMdtldetailLines["rm_item_id"] = jObjectBatch[i]["RM_ItemID"].ToString();
                    dtrowHdnRMdtldetailLines["bom_qty"] = jObjectBatch[i]["RM_BomQty"].ToString();
                    dtrowHdnRMdtldetailLines["req_qty"] = jObjectBatch[i]["RM_ReqQty"].ToString();
                    HdnRMdtl_detail.Rows.Add(dtrowHdnRMdtldetailLines);
                }
            }
            return HdnRMdtl_detail;
        }
        public ActionResult MRPListSearch(string Source, string Fromdate, string Todate, string Status,string Req_area)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                // Session["WF_status"] = null;
                MaterialRequirementPlan_Model _MaterialRequirementPlan_Model = new MaterialRequirementPlan_Model();
                _MaterialRequirementPlan_Model.WF_Status = null;
                BrchID = Session["BranchId"].ToString();

                List<MRP_List> _MRP_List = new List<MRP_List>();
                DataTable dt = new DataTable();
                dt = _MRP_ISERVICES.GetMRPList(Source, Fromdate, Todate, Status, CompID, BrchID, "", "", "", Req_area);
                //Session["MRPSearch"] = "MRP_Search";
                _MaterialRequirementPlan_Model.MRPSearch = "MRP_Search";

                foreach (DataRow dr in dt.Rows)
                {
                    MRP_List _MRPList = new MRP_List();
                    _MRPList.mrp_no = dr["mrp_no"].ToString();
                    _MRPList.mrp_date = dr["mrp_dt"].ToString();
                    _MRPList.fy = dr["fy"].ToString();
                    _MRPList.period = dr["f_periodval"].ToString();
                    _MRPList.daterange = dr["daterange"].ToString();
                    _MRPList.source = dr["src_type"].ToString();
                    _MRPList.Req_area = dr["req_area"].ToString();
                    _MRPList.status = dr["status_name"].ToString();
                    _MRPList.createon = dr["create_dt"].ToString();
                    _MRPList.approvedon = dr["app_dt"].ToString();
                    _MRPList.amendedon = dr["mod_dt"].ToString();
                    _MRPList.createdby = dr["createdby"].ToString();
                    _MRPList.approvedby = dr["approvedby"].ToString();
                    _MRPList.amendby = dr["ammendby"].ToString();
                    _MRPList.src_doc_no = dr["src_doc_no"].ToString();
                    _MRPList.src_doc_date = dr["src_doc_date"].ToString();
                    _MRP_List.Add(_MRPList);

                }
                _MaterialRequirementPlan_Model.MRP_ListDetail = _MRP_List;
                CommonPageDetails();
                List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                DataTable dt1 = GetRequirmentreaList();
                foreach (DataRow dr in dt1.Rows)
                {
                    RequirementAreaList _RAList = new RequirementAreaList();
                    _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                    _RAList.req_val = dr["setup_val"].ToString();
                    requirementAreaLists.Add(_RAList);
                }
                requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = " ---Select---" });
                _MaterialRequirementPlan_Model._requirementAreaLists = requirementAreaLists;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialRequirementPlanList.cshtml", _MaterialRequirementPlan_Model);
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
        public ActionResult EditDomesticMRPDetails(string MRPNumber, string MRPDate, string ListFilterData, string WF_Status)
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
            //Session["Messagefc"] = "New";
            //Session["Commandfc"] = "Update";
            //Session["MRPNumberfc"] = MRPNumber;
            //Session["TransTypefc"] = "Update";
            //Session["AppStatusfc"] = 'D';
            //Session["BtnNamefc"] = "BtnEdit";
            MaterialRequirementPlan_Model dblclick = new MaterialRequirementPlan_Model();
            UrlModel _url = new UrlModel();
            dblclick.MRPNumberfc = MRPNumber;
            dblclick.MRPDatefc = MRPDate;
            dblclick.TransTypefc = "Update";
            dblclick.BtnNamefc = "BtnEdit";
            if (WF_Status != null && WF_Status != "")
            {
                _url.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            //_url.Cmd = "Update";
            _url.tp = "Update";
            _url.bt = "BtnEdit";
            _url.MRP_No = MRPNumber;
            _url.MRP_Date = MRPDate;
            TempData["ListFilterData"] = ListFilterData;

            return RedirectToAction("MaterialRequirementPlanDetail", _url);
        }
        private ActionResult Delete_MRPDetails(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model, string command)
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

                DataSet Message = _MRP_ISERVICES.Delete_MRPDetails(_MaterialRequirementPlan_Model, CompID, BrchID);

                //Session["BtnNamefc"] = "BtnRefresh";
                //Session["Commandfc"] = "Refresh";
                //Session["TransTypefc"] = "Save";
                //Session["Messagefc"] = "Deleted";
                //Session["DocumentStatusfc"] = null;

                return RedirectToAction("MaterialRequirementPlanDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status1)
        {
            //JArray jObjectBatch = JArray.Parse(list);
            MaterialRequirementPlan_Model _MaterialRequirementPlan_Model = new MaterialRequirementPlan_Model();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _MaterialRequirementPlan_Model.MRPNumber = jObjectBatch[i]["MRPNo"].ToString();
                    _MaterialRequirementPlan_Model.MRPDate = jObjectBatch[i]["MRPDate"].ToString();
                    _MaterialRequirementPlan_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _MaterialRequirementPlan_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _MaterialRequirementPlan_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_MaterialRequirementPlan_Model.A_Status != "Approve")
            {
                _MaterialRequirementPlan_Model.A_Status = "Approve";
            }
            string command = "";
            Approve_MRPDetails(_MaterialRequirementPlan_Model, command, ListFilterData1, WF_Status1);
            UrlModel ApproveModel = new UrlModel();
            //_MaterialRequirementPlan_Model.TransTypefc = "Update";
            //_MaterialRequirementPlan_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumberfc;
            //_MaterialRequirementPlan_Model.Messagefc = "Approved";
            //_MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
            if (WF_Status1 != null && WF_Status1 != "")
            {
                // _MaterialRequirementPlan_Model.WF_Status1 = WF_Status1;
                ApproveModel.wf = WF_Status1;
            }
            TempData["ModelData"] = _MaterialRequirementPlan_Model;
            ApproveModel.tp = "Update";
            ApproveModel.MRP_No = _MaterialRequirementPlan_Model.MRPNumberfc;
            ApproveModel.MRP_Date = _MaterialRequirementPlan_Model.MRPDatefc;
            ApproveModel.bt = "BtnEdit";
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("MaterialRequirementPlanDetail", ApproveModel);
        }

        public ActionResult Approve_MRPDetails(MaterialRequirementPlan_Model _MaterialRequirementPlan_Model, string command, string ListFilterData1, string WF_Status1)
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
                string MrpNo = _MaterialRequirementPlan_Model.MRPNumber;
                string MrpDate = _MaterialRequirementPlan_Model.MRPDate;
                string A_Status = _MaterialRequirementPlan_Model.A_Status;
                string A_Level = _MaterialRequirementPlan_Model.A_Level;
                string A_Remarks = _MaterialRequirementPlan_Model.A_Remarks;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                /*-----------------------------For Save Current Details On Approve------------------------------*/
                DataTable MRPHeader = new DataTable();
                DataTable MRPItemDetails = new DataTable();
                DataTable SFMaterialDetails = new DataTable();
                DataTable InputMaterialDetails = new DataTable();
                //DataTable HdnSFmaterial = new DataTable();
                //DataTable HdnRMdetails = new DataTable();

                MRPHeader = TblHeaderDetails(_MaterialRequirementPlan_Model);
                MRPItemDetails = ProductDetailTable(_MaterialRequirementPlan_Model.ProductDetail);
                //HdnSFmaterial = HdnSFMaterialTable(_MaterialRequirementPlan_Model.hdnSFMaterialDetail);
                //HdnRMdetails = HdnRMTable(_MaterialRequirementPlan_Model.hdnInputMaterialDetail);
                SFMaterialDetails = SFMaterialTable(_MaterialRequirementPlan_Model.SFMaterialDetail);
                InputMaterialDetails = InputMaterialDetailTable(_MaterialRequirementPlan_Model.InputMaterialDetail);

                /*------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                DataTable dtSubItem_precu = new DataTable();
                dtSubItem = SubItemDetailsDtTable(_MaterialRequirementPlan_Model.SubItemDetailsDt);
                dtSubItem_precu = SubItemDetailsDtTable(_MaterialRequirementPlan_Model.SubItemDetailsDt_prcure);
                /*------------------Sub Item end----------------------*/
                /*-----------------------------For Save Current Details On Approve End------------------------------*/
                string Message = _MRP_ISERVICES.Approved_MRPDetails(MRPHeader, MRPItemDetails, InputMaterialDetails, SFMaterialDetails
                    , dtSubItem, A_Status, A_Level, A_Remarks, dtSubItem_precu);

                if (Message == "Approved")
                {
                    try
                    {
                        //string fileName = "MRP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "MaterialRequirementPlan_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(MrpNo, MrpDate, fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, MrpNo, "AP", UserID, "", filePath);
                    }

                    catch (Exception exMail)
                    {
                        _MaterialRequirementPlan_Model.Messagefc = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    //Session["Messagefc"] = "Approved";
                    _MaterialRequirementPlan_Model.Messagefc = _MaterialRequirementPlan_Model.Messagefc == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    //_MaterialRequirementPlan_Model.Messagefc = "Approved";
                }
                //Session["TransTypefc"] = "Update";
                //Session["Commandfc"] = "Approve";
                //Session["MRPNumberfc"] = _MaterialRequirementPlan_Model.MRPNumber;
                //Session["AppStatusfc"] = 'D';
                //Session["BtnNamefc"] = "BtnEdit";
                UrlModel ApproveModel = new UrlModel();
                _MaterialRequirementPlan_Model.TransTypefc = "Update";
                _MaterialRequirementPlan_Model.MRPNumberfc = _MaterialRequirementPlan_Model.MRPNumber;
                _MaterialRequirementPlan_Model.Messagefc = "Approved";
                _MaterialRequirementPlan_Model.BtnNamefc = "BtnEdit";
                _MaterialRequirementPlan_Model.Amend = "";

                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _MaterialRequirementPlan_Model.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _MaterialRequirementPlan_Model;

                TempData["ListFilterData"] = ListFilterData1;

                return RedirectToAction("MaterialRequirementPlanDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }

        public JsonResult AddPRoductionPlanItemDetail(string F_Fy, string F_Period, string FromDate, string ToDate,
            string P_Number, string P_Date)
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
                DataSet Deatils = _MRP_ISERVICES.AddPRoductionPlanItemDetail(Comp_ID, Br_ID, F_Fy, F_Period, FromDate, ToDate, P_Number, P_Date);
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
        public JsonResult BatchSFAndRMItemDetail(string productList)
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
                DataTable productListDt = ProductListDtTable(productList, Comp_ID, Br_ID);
                DataSet Deatils = _MRP_ISERVICES.GetSfAndRmDataByProductList(Comp_ID, Br_ID, productListDt);
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
        public ActionResult BatchSFAndRMItemDetailPartial(string productList)
        {
            //JsonResult dataRow = null;
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
                Common_Model _Model = new Common_Model();
                DataTable productListDt = ProductListDtTable(productList, Comp_ID, Br_ID);
                DataSet Deatils = _MRP_ISERVICES.GetSfAndRmDataByProductList(Comp_ID, Br_ID, productListDt);
                _Model.cmn_data = Json(JsonConvert.SerializeObject(Deatils)).Data.ToString();
                return PartialView("~/Areas/Common/Views/Cmn_DataContainer.cshtml", _Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataTable ProductListDtTable(string productList, string comp_id, string br_id)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("sr_no", typeof(string));
            dtItem.Columns.Add("comp_id", typeof(string));
            dtItem.Columns.Add("br_id", typeof(string));
            dtItem.Columns.Add("product_id", typeof(string));
            dtItem.Columns.Add("qty", typeof(string));
            if (productList != null)
            {
                JArray jObject2 = JArray.Parse(productList);
                for (int i = 0; i < jObject2.Count; i++)
                {
                    DataRow dtrowItemdetails = dtItem.NewRow();
                    dtrowItemdetails["sr_no"] = jObject2[i]["sr_no"].ToString();
                    dtrowItemdetails["comp_id"] = comp_id;
                    dtrowItemdetails["br_id"] = br_id;
                    dtrowItemdetails["product_id"] = jObject2[i]["product_id"].ToString();
                    dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                    dtItem.Rows.Add(dtrowItemdetails);
                }
            }
            return dtItem;
        }
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
                DataTable dt = _MRP_ISERVICES.GetRequirmentreaList(CompID, BrchID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public string CheckPRAgainstMRP(string DocNo, string DocDate)
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
                DataSet Deatils = _MRP_ISERVICES.CheckPRAgainstMRP(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
                if (Deatils.Tables[1].Rows.Count > 0)
                {
                    if (Convert.ToInt32(Deatils.Tables[1].Rows[0]["JcCount"]) > 0)
                    {
                        str = "JCInProcess";
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return str;
        }
        public ActionResult GetPPNumberList(MaterialRequirementPlan_Model MRP_Model, string RequisitionArea)
        {
            string PPNumber = string.Empty;
            Dictionary<string, string> PPNumberList = new Dictionary<string, string>();

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
                if (string.IsNullOrEmpty(MRP_Model.PP_Number))
                {
                    PPNumber = "0";
                }
                else
                {
                    PPNumber = MRP_Model.PP_Number;
                }

                PPNumberList = _MRP_ISERVICES.GetPPNumberList(CompID, BrchID, PPNumber, RequisitionArea);
                return Json(PPNumberList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public JsonResult GetPPNumberDetail(string PP_Number, string PP_Date)
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
                DataSet Result = _MRP_ISERVICES.GetPPNumberDetail(CompID, BrchID, PP_Number, PP_Date);
                return Json(JsonConvert.SerializeObject(Result));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        /*--------------For Getting Semi finished Item BOM------------*/
        public ActionResult GetSFBOMDetailsItemWise(string FGItemId, string SFItemId, string Level = null)
        {
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
                ViewBag.BOMDetailsItemWise = _MRP_ISERVICES.GetSFBOMDetailsItemWise(Comp_ID, Br_ID, FGItemId, SFItemId);
                ViewBag.BomProduct_id = FGItemId;
                ViewBag.BOMLevel = Level;
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialBillOfMaterial.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetProducedQuantityDetail(string mrp_no, string mrp_dt, string product_Id, string Flag)
        {
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
                ViewBag.ProdDetail = _MRP_ISERVICES.GetProducedQuantityDetail(Comp_ID, Br_ID, mrp_no, mrp_dt, product_Id, Flag).Tables[0];

                return View("~/Areas/ApplicationLayer/Views/Shared/_ProducedQuantityDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetProcuredQuantityDetail(string mrp_no, string mrp_dt, string product_Id, string Flag, string UomId = null)
        {
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
                ViewBag.ProcDetail = _MRP_ISERVICES.GetProcuredQuantityDetail(Comp_ID, Br_ID, mrp_no, mrp_dt, product_Id, Flag, UomId).Tables[0];

                return View("~/Areas/ApplicationLayer/Views/Shared/_ProcuredQuantityDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, string Flag
            , string Status, string Doc_no, string Doc_dt)
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
                if (Flag == "Quantity" || Flag == "OrdrToPrecurQuantity" || Flag == "RMOrdrToPrecurQuantity" || Flag == "OrdrToProduceQuantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0]; // common method to get subitem details item wise
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = item.GetValue("qty");
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _MRP_ISERVICES.MRP_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0]; // After approve to get subitem details item wise
                    }
                }
                else if (Flag == "ProduceQty" || Flag == "OrderedQty")
                {
                    dt = _MRP_ISERVICES.MRP_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0]; // After approve to get subitem details item wise
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = (Flag == "RMOrdrToPrecurQuantity" || Flag == "OrdrToPrecurQuantity") ? "Y" : "N"
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

        public FileResult GenratePdfFile(MaterialRequirementPlan_Model _Model)
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
                DataSet Deatils = _MRP_ISERVICES.GetMRPPrintDeatils(CompID, BrchID, _Model.MRPNumber, _Model.MRPDate);
                ViewBag.PageName = "MRP";
                ViewBag.Title = "Material Requirment Plan";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(GLVoucherHtml);
                    pdfDoc = new Document(PageSize.A4.Rotate(), 0f, 0f, 10f, 20f);
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
                                draftimg.SetAbsolutePosition(100, 0);
                                draftimg.ScaleAbsolute(650f, 650f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                    return File(bytes.ToArray(), "application/pdf", "MaterialRequirementPlan.pdf");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }

        }
        public byte[] GetPdfData(string mrpNo, string mrpDate)
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
                DataSet Deatils = _MRP_ISERVICES.GetMRPPrintDeatils(CompID, BrchID, mrpNo, mrpDate);
                ViewBag.PageName = "MRP";
                ViewBag.Title = "Material Requirment Plan";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/MaterialRequirementPlan/MaterialRequirementPlanPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(GLVoucherHtml);
                    pdfDoc = new Document(PageSize.A4.Rotate(), 0f, 0f, 10f, 20f);
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
                                draftimg.SetAbsolutePosition(100, 0);
                                draftimg.ScaleAbsolute(650f, 650f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
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
                        var data = GetPdfData(Doc_no,Doc_dt);
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

        public ActionResult GetRMPendingOrderQuantityDetails(string ProductID, string PndOrdQty, string MaterialName
            , string UOM, string UomId = null)
        {
            DataSet ds = new DataSet();
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
                    ds = _MRP_ISERVICES.GetRMPendingOrderQuantityDetails(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID)
                        , ProductID, UomId);
                    //DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                }
                ViewBag.RM_PendOrdQtyDetails = ds;
                ViewBag.ItemName = MaterialName;
                ViewBag.UOM = UOM;
                ViewBag.PndOrdQty = PndOrdQty;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_RMPendingOrderDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json(Ex.Message);
            }

        }
        public ActionResult getProductMaterialDetails(string product_id, string qty)
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
                DataSet ds = _MRP_ISERVICES.ProductMaterialDetails(CompID, BrchID, product_id, qty);
                ViewBag.MaterialDetails = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_MaterialDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return PartialView("~/Views/Shared/Error.cshtml");
            }

        }
    }
}