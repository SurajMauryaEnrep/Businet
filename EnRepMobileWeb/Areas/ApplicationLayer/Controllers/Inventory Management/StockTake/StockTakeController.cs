using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.StockTake;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.StockTake;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.StockTake
{
    public class StockTakeController : Controller
    {
        string CompID, language, BrchID, title, UserID, FromDate = String.Empty;
        string DocumentMenuId = "105102155";
        List<STKList> _StockTakeList;
        DataTable dt;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        StockTake_ISERVICES _StockTake_ISERVICES;
        StockTake_Model _StockTake_Model;
        public StockTakeController(StockTake_ISERVICES _StockTake_ISERVICES, Common_IServices _Common_IServices)
        {
            this._StockTake_ISERVICES = _StockTake_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/StockTake
        public ActionResult StockTake(StockTakeList_Model _StockTakeList_Model)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //StockTakeList_Model _StockTakeList_Model = new StockTakeList_Model();               
                GetStatusList(_StockTakeList_Model);
                ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //string endDate = dtnow.ToString("yyyy-MM-dd");
                //_StockTakeList_Model.STKFromDate, _StockTakeList_Model.STKToDate, _StockTakeList_Model.Status
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    _StockTakeList_Model.STKFromDate = a[0].Trim();
                    _StockTakeList_Model.STKToDate = a[1].Trim();
                    _StockTakeList_Model.Status = a[2].Trim();
                    if (_StockTakeList_Model.Status == "0")
                    {
                        _StockTakeList_Model.Status = null;
                    }
                    _StockTakeList_Model.FromDate = _StockTakeList_Model.STKFromDate;
                    _StockTakeList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                }
                else
                {
                    _StockTakeList_Model.FromDate = startDate;
                    _StockTakeList_Model.STKFromDate = startDate;
                    _StockTakeList_Model.STKToDate = CurrentDate;
                }
                _StockTakeList_Model.StockTakeList = GetStockTakeListAll(_StockTakeList_Model);
                ViewBag.MenuPageName = getDocumentName();               
                ViewBag.DocumentMenuId = DocumentMenuId;
                _StockTakeList_Model.Title = title;
                //Session["STKSearch"] = "0";
                _StockTakeList_Model.STKSearch = "0";
                ViewBag.VBRoleList = GetRoleList();
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockTake/StockTakeList.cshtml", _StockTakeList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
          
        }
        public ActionResult GetStockTakeList(string docid, string status)
        {
            StockTakeList_Model StkList_Model = new StockTakeList_Model();
            //Session["WF_status"] = status;
            StkList_Model.WF_status = status;
            return RedirectToAction("StockTake", StkList_Model);
        }
        public ActionResult AddStockTakeDetail()
        {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("StockTake");
            }
            /*End to chk Financial year exist or not*/
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";

            StockTake_Model _StockTake_Model = new StockTake_Model();
            _StockTake_Model.Message = "New";
            _StockTake_Model.Command = "Add";
            _StockTake_Model.AppStatus = "D";
            _StockTake_Model.TransType = "Save";
            _StockTake_Model.BtnName = "BtnAddNew";
            TempData["ModelData"] = _StockTake_Model;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.DocumentMenuId = DocumentMenuId;            
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnAddNew";
            URLModel.TransType = "Save";
            URLModel.Message = "New";
            ViewBag.MenuPageName = getDocumentName();
            TempData["ListFilterData"] = null;
            return RedirectToAction("StockTakeDetail", "StockTake", URLModel);           
        }
        public ActionResult StockTakeDetail(URLDetailModel URLModel)
        {
            try
            {
                ViewBag.DocumentMenuId = DocumentMenuId;
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string Language = string.Empty;
                StockTake_Model _StockTake_Model = new StockTake_Model();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    Language = Session["Language"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(Comp_ID, Br_ID, URLModel.StockTakeDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var StockTakeModelData =   TempData["ModelData"] as StockTake_Model;
                if(StockTakeModelData != null)
                {
                    ViewBag.MenuPageName = getDocumentName();
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, Br_ID, DocumentMenuId).Tables[0];
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    GetItemGrpList(StockTakeModelData);
                    GetWarehouseList(StockTakeModelData);
                    //DataTable dt = new DataTable();
                    //List<WarehouseNamePopUp> WHLists = new List<WarehouseNamePopUp>();
                    //dt = GetWarehouseListForPopUpStk1(StockTakeModelData);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    WarehouseNamePopUp WarehouseList = new WarehouseNamePopUp();
                    //    WarehouseList.Wh_Id = dr["wh_id"].ToString();
                    //    WarehouseList.Wh_Name = dr["wh_name"].ToString();
                    //    WHLists.Add(WarehouseList);
                    //}
                    //WHLists.Insert(0, new WarehouseNamePopUp() { Wh_Id = "0", Wh_Name = "---Select---" });
                    //StockTakeModelData.WarehouseNameListForPopup = WHLists;
                    //GetWarehouseListForPopup(StockTakeModelData);
                    StockTakeModelData.stktake_dt = DateTime.Now;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        StockTakeModelData.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (StockTakeModelData.TransType == "Update" || StockTakeModelData.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        BrchID = Session["BranchId"].ToString();
                        //string Stk_no = Session["StockTakeNo"].ToString();
                        string Stk_no = StockTakeModelData.StockTakeNo;
                        //string Stk_dt = Session["StockTakeDate"].ToString();
                        string Stk_dt = StockTakeModelData.StockTakeDate;
                        DataSet ds = _StockTake_ISERVICES.GetStockTakeDetail(Stk_no, Stk_dt, CompID, BrchID, UserID, DocumentMenuId);
                        StockTakeModelData.stktake_no = ds.Tables[0].Rows[0]["stk_no"].ToString();
                        StockTakeModelData.stktake_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["stk_dt"].ToString());
                        StockTakeModelData.wh = Convert.ToInt32(ds.Tables[0].Rows[0]["wh_id"].ToString());
                        StockTakeModelData.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        StockTakeModelData.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        StockTakeModelData.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        StockTakeModelData.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        StockTakeModelData.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        StockTakeModelData.stktake_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        StockTakeModelData.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        StockTakeModelData.DocumentStatus = Statuscode;

                        StockTakeModelData.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        StockTakeModelData.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);


                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && StockTakeModelData.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[3].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[4].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    StockTakeModelData.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        StockTakeModelData.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        StockTakeModelData.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    StockTakeModelData.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        StockTakeModelData.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    StockTakeModelData.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    StockTakeModelData.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    StockTakeModelData.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    StockTakeModelData.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.MenuPageName = getDocumentName();
                        StockTakeModelData.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        ViewBag.ItemLotBatchSerialDetails = ds.Tables[2];
                       
                        StockTakeModelData.StkTakeItemBatchSerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockTake/StockTakeDetail.cshtml", StockTakeModelData);
                    }
                    else
                    {
                        StockTakeModelData.Title = title;
                        //Session["DNSearch"] = "0";
                        StockTakeModelData.DNSearch = "0";
                        ViewBag.VBRoleList = GetRoleList();
                        //Session["DocumentStatus"] = "New";
                        StockTakeModelData.DocumentStatus = "New";
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockTake/StockTakeDetail.cshtml", StockTakeModelData);
                    }
                }
                else
                {       /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
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
                    if (URLModel.StockTakeDate != null)
                    {
                        _StockTake_Model.StockTakeDate = URLModel.StockTakeDate;
                    }                    
                    if (URLModel.StockTakeNo != null)
                    {
                        _StockTake_Model.StockTakeNo = URLModel.StockTakeNo;
                    }
                    if (URLModel.TransType != null)
                    {
                        _StockTake_Model.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _StockTake_Model.TransType = "Save";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _StockTake_Model.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _StockTake_Model.BtnName = "Refresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _StockTake_Model.Command = URLModel.Command;
                    }
                    else
                    {
                        _StockTake_Model.Command = "Refresh";
                    }       
                    if (URLModel.WF_status1 != null)
                    {
                        _StockTake_Model.WF_status1 = URLModel.WF_status1;
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, Br_ID, DocumentMenuId).Tables[0];
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    GetItemGrpList(_StockTake_Model);
                    GetWarehouseList(_StockTake_Model);
                    //GetWarehouseListForPopup(_StockTake_Model);
                    //DataTable dt = new DataTable();
                    //List<WarehouseNamePopUp> WHLists = new List<WarehouseNamePopUp>();
                    //dt = GetWarehouseListForPopUpStk1(_StockTake_Model);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    WarehouseNamePopUp WarehouseList = new WarehouseNamePopUp();
                    //    WarehouseList.Wh_Id = dr["wh_id"].ToString();
                    //    WarehouseList.Wh_Name = dr["wh_name"].ToString();
                    //    WHLists.Add(WarehouseList);
                    //}
                    //WHLists.Insert(0, new WarehouseNamePopUp() { Wh_Id = "0", Wh_Name = "---Select---" });
                    //_StockTake_Model.WarehouseNameListForPopup = WHLists;
                    _StockTake_Model.stktake_dt = DateTime.Now;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _StockTake_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_StockTake_Model.TransType == "Update" || _StockTake_Model.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        BrchID = Session["BranchId"].ToString();
                        //string Stk_no = Session["StockTakeNo"].ToString();
                        string Stk_no = _StockTake_Model.StockTakeNo;
                        //string Stk_dt = Session["StockTakeDate"].ToString();
                        string Stk_dt = _StockTake_Model.StockTakeDate;
                        DataSet ds = _StockTake_ISERVICES.GetStockTakeDetail(Stk_no, Stk_dt, CompID, BrchID, UserID, DocumentMenuId);
                        _StockTake_Model.stktake_no = ds.Tables[0].Rows[0]["stk_no"].ToString();
                        _StockTake_Model.stktake_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["stk_dt"].ToString());
                        _StockTake_Model.wh = Convert.ToInt32(ds.Tables[0].Rows[0]["wh_id"].ToString());
                        _StockTake_Model.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _StockTake_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _StockTake_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _StockTake_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _StockTake_Model.create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _StockTake_Model.stktake_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        _StockTake_Model.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _StockTake_Model.DocumentStatus = Statuscode;

                        _StockTake_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        _StockTake_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);


                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _StockTake_Model.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[3].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[4].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _StockTake_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _StockTake_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _StockTake_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _StockTake_Model.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _StockTake_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _StockTake_Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _StockTake_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _StockTake_Model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _StockTake_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.MenuPageName = getDocumentName();
                        _StockTake_Model.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        ViewBag.ItemLotBatchSerialDetails = ds.Tables[2];
                        _StockTake_Model.StkTakeItemBatchSerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockTake/StockTakeDetail.cshtml", _StockTake_Model);
                    }
                    else
                    {
                        _StockTake_Model.Title = title;
                        //Session["DNSearch"] = "0";
                        _StockTake_Model.DNSearch = "0";
                        ViewBag.VBRoleList = GetRoleList();
                        //Session["DocumentStatus"] = "New";
                        _StockTake_Model.DocumentStatus = "New";
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockTake/StockTakeDetail.cshtml", _StockTake_Model);
                    }
                }
               
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
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
        public ActionResult EditStockTake(string STKNo, string STK_Date,string ListFilterData,string WF_status)
        {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
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
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["StockTakeNo"] = STKNo;
            //Session["StockTakeDate"] = STK_Date;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            StockTake_Model _StockTake_Model = new StockTake_Model();
            _StockTake_Model.Message = "New";
            _StockTake_Model.Command = "Add";
            _StockTake_Model.StockTakeNo = STKNo;
            _StockTake_Model.StockTakeDate = STK_Date;
            _StockTake_Model.TransType = "Update";
            _StockTake_Model.AppStatus = "D";       
            _StockTake_Model.BtnName = "BtnToDetailPage";
            _StockTake_Model.WF_status1 = WF_status;         
            TempData["ModelData"] = _StockTake_Model;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.StockTakeNo = STKNo;
            URLModel.StockTakeDate = STK_Date;           
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.Command = "Add";
            URLModel.WF_status1 = WF_status;          
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("StockTakeDetail", "StockTake", URLModel);
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
        public ActionResult GetItemGrpList(StockTake_Model queryParameters)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            string GroupName = string.Empty;
            Dictionary<string, string> ItemGroupList = new Dictionary<string, string>();

            try
            {
                if (string.IsNullOrEmpty(queryParameters.GroupName))
                {
                    GroupName = "0";
                }
                else
                {
                    GroupName = queryParameters.GroupName;
                }
                ItemGroupList = _StockTake_ISERVICES.ItemGroupList(GroupName, Comp_ID);

                List<ItemGroupName> _ItemGroupList = new List<ItemGroupName>();
                foreach (var data in ItemGroupList)
                {
                    ItemGroupName _ItemGroupDetail = new ItemGroupName();
                    _ItemGroupDetail.Group_Id = data.Key;
                    _ItemGroupDetail.Group_Name = data.Value;
                    _ItemGroupList.Add(_ItemGroupDetail);
                }
                queryParameters.ItemGroupNameList = _ItemGroupList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemGroupList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetWarehouseList(StockTake_Model queryParameters)
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

                string WareHouseName = string.Empty;
                Dictionary<string, string> WareHouseNameList = new Dictionary<string, string>();

                try
                {
                    if (string.IsNullOrEmpty(queryParameters.WarehouseName))
                    {
                    WareHouseName = "0";
                    }
                    else
                    {
                    WareHouseName = queryParameters.WarehouseName;
                    }
                WareHouseNameList = _StockTake_ISERVICES.GetWarehouseList(WareHouseName, Comp_ID, Br_ID);

                    List<WarehouseName> _WarehouseNameList = new List<WarehouseName>();
                    foreach (var data in WareHouseNameList)
                    {
                    WarehouseName _WarehouseNameDetail = new WarehouseName();
                    _WarehouseNameDetail.Wh_Id = data.Key;
                    _WarehouseNameDetail.Wh_Name = data.Value;
                    _WarehouseNameList.Add(_WarehouseNameDetail);
                    }
                    queryParameters.WarehouseNameList = _WarehouseNameList;

                }
                catch (Exception Ex)
                {
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, Ex);
                    return Json("ErrorPage");
                }
                return Json(WareHouseNameList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);

          
        }
        //public ActionResult GetWarehouseListForPopup(StockTake_Model queryParameters)
        //{
        //    string Comp_ID = string.Empty;
        //    string Br_ID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        Comp_ID = Session["CompId"].ToString();
        //    }
        //    if (Session["BranchId"] != null)
        //    {
        //        Br_ID = Session["BranchId"].ToString();
        //    }

        //    string WareHouseName = string.Empty;
        //    Dictionary<string, string> WareHouseListPopup = new Dictionary<string, string>();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(queryParameters.WarehouseNamePopup))
        //        {
        //            WareHouseName = "0";
        //        }
        //        else
        //        {
        //            WareHouseName = queryParameters.WarehouseNamePopup;
        //        }
        //        WareHouseListPopup = _StockTake_ISERVICES.GetWarehouseList(WareHouseName, Comp_ID, Br_ID);

        //        List<WarehouseNamePopUp> _WarehouseNameList = new List<WarehouseNamePopUp>();
        //        foreach (var data in WareHouseListPopup)
        //        {
        //            WarehouseNamePopUp _WarehouseDetail = new WarehouseNamePopUp();
        //            _WarehouseDetail.Wh_Id = data.Key;
        //            _WarehouseDetail.Wh_Name = data.Value;
        //            _WarehouseNameList.Add(_WarehouseDetail);
        //        }
        //        queryParameters.WarehouseNameListForPopup = _WarehouseNameList;

        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(WareHouseListPopup.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        //}
        //public DataTable GetWarehouseListForPopUpStk1(StockTake_Model queryParameters)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        //JsonResult DataRows = null;
        //        string Comp_ID = string.Empty;
        //        string Br_ID = string.Empty;
        //        string WareHouseName = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        if (string.IsNullOrEmpty(queryParameters.WHNamePopup))
        //        {
        //            WareHouseName = "0";
        //        }
        //        else
        //        {
        //            WareHouseName = queryParameters.WHNamePopup;
        //        }
        //        DataSet result = _StockTake_ISERVICES.GetWarehouseListPopUp(Comp_ID, Br_ID, WareHouseName);
        //        dt = result.Tables[0];
                
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //        //return Json("ErrorPage");
        //    }
        //    return dt;
        //}
        //[HttpPost]
        //public JsonResult GetWarehouseListForPopUpStk(StockTake_Model queryParameters)
        //{
        //    try
        //    {
        //        JsonResult DataRows = null;
        //        string Comp_ID = string.Empty;
        //        string Br_ID = string.Empty;
        //        string WareHouseName = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        if (string.IsNullOrEmpty(queryParameters.WHNamePopup))
        //        {
        //            WareHouseName = "0";
        //        }
        //        else
        //        {
        //            WareHouseName = queryParameters.WHNamePopup;
        //        }
        //        DataSet result = _StockTake_ISERVICES.GetWarehouseListPopUp(Comp_ID, Br_ID, WareHouseName);
        //        DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
        //        return DataRows;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //}
        [HttpPost]
        public ActionResult getListOfItems(string GroupID)
        {
            JsonResult DataRows = null;
            string ItmName = string.Empty;
            //string GroupID = string.Empty;
            //Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string BranchId = string.Empty;
                if (Session["BranchId"] != null)
                {
                    BranchId = Session["BranchId"].ToString();
                }
                    ItmName = "0";
              
                DataSet SOItmList = _StockTake_ISERVICES.GetItemList(Comp_ID, BranchId, ItmName, GroupID);
                DataRow dr;
                dr = SOItmList.Tables[0].NewRow();
                dr[0] = "0";
                dr[1] = "---All---";
                dr[2] = "0";
                SOItmList.Tables[0].Rows.InsertAt(dr, 0);

                DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        //[HttpPost]
        //public ActionResult getListOfItemsForAddNewPopup(int Wh_Id)
        //{
        //    JsonResult DataRows = null;
        //    string ItmName = string.Empty;
        //    //string GroupID = string.Empty;
        //    //Dictionary<string, string> SuppList = new Dictionary<string, string>();
        //    string Comp_ID = string.Empty;
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        string BranchId = string.Empty;
        //        if (Session["BranchId"] != null)
        //        {
        //            BranchId = Session["BranchId"].ToString();
        //        }


        //        DataSet SOItmListNew = _StockTake_ISERVICES.GetItemListForAddNewPopup(Comp_ID, BranchId, /*ItmName,*/ Wh_Id);
        //        DataRow dr;
        //        dr = SOItmListNew.Tables[0].NewRow();
        //        dr[0] = "0";
        //        dr[1] = "---Select---";
        //        dr[2] = "0";
        //        SOItmListNew.Tables[0].Rows.InsertAt(dr, 0);

        //        DataRows = Json(JsonConvert.SerializeObject(SOItmListNew));/*Result convert into Json Format for javasript*/
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return View("~/Views/Shared/Error.cshtml");
        //    }
        //    return DataRows;
        //}
        public ActionResult getListOfItemsForAddNewPopup(BindItemListPopup bindItem, string PageName,int WHID)
        {

            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                bindItem.Wh_Id = WHID;
                
                DataSet RetrnItmList = _StockTake_ISERVICES.GetItemListForAddNewPopup(Comp_ID, Br_ID, bindItem.SearchName, PageName, bindItem.Wh_Id);
                if (RetrnItmList.Tables[0].Rows.Count > 0)
                {
                    //ItemList.Add("0" + "_" + "H1", "Heading");
                    for (int i = 0; i < RetrnItmList.Tables[0].Rows.Count; i++)
                    {
                        string itemId = RetrnItmList.Tables[0].Rows[i]["Item_id"].ToString();
                        string itemName = RetrnItmList.Tables[0].Rows[i]["Item_name"].ToString();
                        string Uom = RetrnItmList.Tables[0].Rows[i]["uom_name"].ToString();
                        ItemList.Add(itemId + "_" + Uom, itemName);
                    }
                }
                //DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStockTakeItemDetail(string WHID, string GRPID,string ItemID,string ListItems)
        {
            try
            {
                //JsonResult DataRows = null;
                _StockTake_Model = new StockTake_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();              
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                int Rows = ListItems == ""? 0 : ListItems.Split(',').Length;
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _StockTake_ISERVICES.GetStockItemDetail(CompID, BrchID, WHID, GRPID, ItemID, ListItems);
                ViewBag.RowCount = Rows;
                ViewBag.AddItemDetails = ds.Tables[0];
                //DataRows = Json(JsonConvert.SerializeObject(ds));
                //return DataRows;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_StockTakeDetails.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult GetStockItemLotBatchSerialDetail(string ItemID, string WHID, string SrcDocNumber, string RT_Status, string flag,string hdFlagForAddNewStk)
        {
            try
            {


                JsonResult DataRows = null;
                _StockTake_Model = new StockTake_Model();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                //List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _StockTake_ISERVICES.GetStockItemLotBatchSerialDetail(CompID, BrchID, ItemID, WHID, SrcDocNumber, RT_Status, flag, hdFlagForAddNewStk);

                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult StockTakeSave(StockTake_Model _StockTake_Model, string stktake_no, string command)
        {
            try
            {
                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_StockTake_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        //Session["Message"] = "New";
                        //Session["Command"] = "Add";
                        //Session["AppStatus"] = 'D';
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        StockTake_Model __StockTake_ModelAddNew = new StockTake_Model();
                        __StockTake_ModelAddNew.AppStatus = "D";
                        __StockTake_ModelAddNew.BtnName = "BtnAddNew";
                        __StockTake_ModelAddNew.TransType = "Save";
                        __StockTake_ModelAddNew.Command = "New";
                        __StockTake_ModelAddNew.Message = "New";
                        TempData["ModelData"] = __StockTake_ModelAddNew;
                        URLDetailModel URLModel = new URLDetailModel();
                        URLModel.Command = "New";
                        URLModel.BtnName = "BtnAddNew";
                        URLModel.TransType = "Save";
                        URLModel.Message = "New";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_StockTake_Model.stktake_no))
                                return RedirectToAction("EditStockTake", new { STKNo = _StockTake_Model.stktake_no, STK_Date= _StockTake_Model.stktake_dt, ListFilterData = _StockTake_Model.ListFilterData1, WF_status = _StockTake_Model.WFStatus });
                            else
                                __StockTake_ModelAddNew.Command = "Refresh";
                            __StockTake_ModelAddNew.TransType = "Refresh";
                            __StockTake_ModelAddNew.BtnName = "Refresh";
                            __StockTake_ModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = __StockTake_ModelAddNew;
                            return RedirectToAction("StockTakeDetail", "StockTake");
                        }
                        /*End to chk Financial year exist or not*/

                        return RedirectToAction("StockTakeDetail", "StockTake", URLModel);

                    case "Edit":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditStockTake", new { STKNo = _StockTake_Model.stktake_no, STK_Date = _StockTake_Model.stktake_dt, ListFilterData = _StockTake_Model.ListFilterData1, WF_status = _StockTake_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string stkdt = _StockTake_Model.stktake_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, stkdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditStockTake", new { STKNo = _StockTake_Model.stktake_no, STK_Date = _StockTake_Model.stktake_dt, ListFilterData = _StockTake_Model.ListFilterData1, WF_status = _StockTake_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["Message"] = null;
                        //Session["BtnName"] = "BtnEdit";
                        //Session["StockTakeNo"] = _StockTake_Model.stktake_no;
                        //Session["StockTakeDate"] = _StockTake_Model.stktake_dt.ToString("yyyy-MM-dd");                  
                        _StockTake_Model.BtnName = "BtnEdit";
                        _StockTake_Model.TransType = "Update";
                        _StockTake_Model.Command = command;
                        _StockTake_Model.StockTakeNo = _StockTake_Model.stktake_no;
                        _StockTake_Model.StockTakeDate = _StockTake_Model.stktake_dt.ToString("yyyy-MM-dd");
                        TempData["ModelData"] = _StockTake_Model;
                        URLDetailModel URLeditModel = new URLDetailModel();
                        URLeditModel.Command = _StockTake_Model.Command;
                        URLeditModel.BtnName = _StockTake_Model.BtnName;
                        URLeditModel.TransType = _StockTake_Model.TransType;
                        URLeditModel.StockTakeNo = _StockTake_Model.StockTakeNo;
                        URLeditModel.StockTakeDate = _StockTake_Model.stktake_dt.ToString("yyyy-MM-dd");

                        TempData["ListFilterData"] = _StockTake_Model.ListFilterData1;
                        return RedirectToAction("StockTakeDetail", URLeditModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        _StockTake_Model.BtnName = "Refresh";
                        _StockTake_Model.Command = command;
                        stktake_no = _StockTake_Model.stktake_no;
                        StockTakeDelete(_StockTake_Model, command);
                        StockTake_Model _StockTakeDelete_Model =  new StockTake_Model();
                        _StockTakeDelete_Model.Message = "Deleted";
                        _StockTakeDelete_Model.Command = "Refresh";
                        _StockTakeDelete_Model.TransType = command;
                        _StockTakeDelete_Model.AppStatus = "D";
                        _StockTakeDelete_Model.BtnName = "BtnDelete";
                        TempData["ModelData"] = _StockTakeDelete_Model;
                        URLDetailModel URLModelDelete = new URLDetailModel();
                        URLModelDelete.Command = "Refresh";
                        URLModelDelete.BtnName = "BtnDelete";
                        URLModelDelete.TransType = command;
                        TempData["ListFilterData"] = _StockTake_Model.ListFilterData1;
                        return RedirectToAction("StockTakeDetail", URLModelDelete);


                    case "Save":
                        //Session["Command"] = command;
                        _StockTake_Model.Command = command;
                        SaveStockTake(_StockTake_Model);
                        if (_StockTake_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = _StockTake_Model;
                        URLDetailModel URLModelSave = new URLDetailModel();
                        URLModelSave.TransType = _StockTake_Model.TransType;
                        URLModelSave.Command = _StockTake_Model.Command;
                        URLModelSave.BtnName = _StockTake_Model.BtnName;
                        URLModelSave.StockTakeNo = _StockTake_Model.StockTakeNo;
                        URLModelSave.StockTakeDate = _StockTake_Model.StockTakeDate;
                        //Session["StockTakeNo"] = Session["StockTakeNo"].ToString();
                        //Session["StockTakeDate"] = Session["StockTakeDate"].ToString();
                        TempData["ListFilterData"] = _StockTake_Model.ListFilterData1;
                        return RedirectToAction("StockTakeDetail", URLModelSave);

                    case "Forward":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditStockTake", new { STKNo = _StockTake_Model.stktake_no, STK_Date = _StockTake_Model.stktake_dt, ListFilterData = _StockTake_Model.ListFilterData1, WF_status = _StockTake_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string stkdt1 = _StockTake_Model.stktake_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, stkdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditStockTake", new { STKNo = _StockTake_Model.stktake_no, STK_Date = _StockTake_Model.stktake_dt, ListFilterData = _StockTake_Model.ListFilterData1, WF_status = _StockTake_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditStockTake", new { STKNo = _StockTake_Model.stktake_no, STK_Date = _StockTake_Model.stktake_dt, ListFilterData = _StockTake_Model.ListFilterData1, WF_status = _StockTake_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string stkdt2 = _StockTake_Model.stktake_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, stkdt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditStockTake", new { STKNo = _StockTake_Model.stktake_no, STK_Date = _StockTake_Model.stktake_dt, ListFilterData = _StockTake_Model.ListFilterData1, WF_status = _StockTake_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        _StockTake_Model.Command = command;
                        stktake_no = _StockTake_Model.stktake_no;
                        //_StockTake_Model.StockTakeNo = _StockTake_Model.stktake_no;
                        //_StockTake_Model.StockTakeDate = _StockTake_Model.stktake_dt.To;
                        //Session["StockTakeNo"] = stktake_no;
                        //Session["StockTakeDate"] = _StockTake_Model.stktake_dt;

                        StockTakeApprove(_StockTake_Model,_StockTake_Model.stktake_no, _StockTake_Model.stktake_dt.ToString("yyyy-MM-dd"),"","", "","","","");
                        //_StockTake_Model.StockTakeNo = _StockTake_Model.stktake_no;
                        //_StockTake_Model.StockTakeDate = _StockTake_Model.StockTakeDate;
                        _StockTake_Model.Message = "Approved";
                        _StockTake_Model.Command = "Approve";
                        _StockTake_Model.TransType = "Update";
                        _StockTake_Model.AppStatus = "D";
                        _StockTake_Model.BtnName = "BtnEdit";
                        // _StockTake_Model.WF_status1 = WF_Status1;
                        // TempData["WF_Status1"] = WF_Status1;
                        TempData["ModelData"] = _StockTake_Model;
                        URLDetailModel _approve_Model = new URLDetailModel();
                        _approve_Model.StockTakeNo = _StockTake_Model.StockTakeNo;
                        _approve_Model.WF_status1 = _StockTake_Model.WF_status1;
                        _approve_Model.StockTakeDate = _StockTake_Model.StockTakeDate;
                        _approve_Model.TransType = "Update";
                        _approve_Model.BtnName = "BtnEdit";
                        TempData["ListFilterData"] = _StockTake_Model.ListFilterData1;
                        return RedirectToAction("StockTakeDetail", _approve_Model);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        //Session["DocumentStatus"] = null;
                        StockTake_Model __StockTakeModelRefresh = new StockTake_Model();
                        __StockTakeModelRefresh.BtnName = "Refresh";
                        __StockTakeModelRefresh.Command = command;
                        __StockTakeModelRefresh.TransType = "Save";
                        __StockTakeModelRefresh.Message = "Refresh";
                        __StockTakeModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = __StockTakeModelRefresh;
                        TempData["ListFilterData"] = _StockTake_Model.ListFilterData1;
                        return RedirectToAction("StockTakeDetail");

                    case "Print":
                        return new EmptyResult();
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        StockTakeList_Model Stock_Model = new StockTakeList_Model();
                        Stock_Model.WF_status = _StockTake_Model.WF_status1;
                        TempData["ListFilterData"] = _StockTake_Model.ListFilterData1;
                        return RedirectToAction("StockTake", "StockTake", Stock_Model);

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
        public ActionResult SaveStockTake(StockTake_Model _StockTake_Model)
        {
            try
            {
                if (_StockTake_Model.CancelFlag == false)
                {
                    var commonContr = new CommonController();
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }

                    DataTable StockTakeHeader = new DataTable();
                    DataTable StockTakeItemDetails = new DataTable();
                    DataTable StockTakeLotBatchSerial = new DataTable();


                    DataTable dt = new DataTable();
                    dt.Columns.Add("MenuDocumentId", typeof(string));
                    dt.Columns.Add("comp_id", typeof(int));
                    dt.Columns.Add("br_id", typeof(int));
                    dt.Columns.Add("stktake_no", typeof(string));
                    dt.Columns.Add("stktake_dt", typeof(string));
                    dt.Columns.Add("stk_rem", typeof(string));
                    dt.Columns.Add("wh_id", typeof(int));
                    dt.Columns.Add("stktake_status", typeof(string));
                    dt.Columns.Add("CreateBy", typeof(string));                    
                    dt.Columns.Add("UserMacaddress", typeof(string));
                    dt.Columns.Add("UserSystemName", typeof(string));
                    dt.Columns.Add("UserIP", typeof(string));
                    dt.Columns.Add("TransType", typeof(string));

                    DataRow dtrow = dt.NewRow();

                    dtrow["MenuDocumentId"] = DocumentMenuId;
                    dtrow["comp_id"] = Session["CompId"].ToString();
                    dtrow["br_id"] = Session["BranchId"].ToString();
                    dtrow["stktake_no"] = _StockTake_Model.stktake_no;
                    dtrow["stktake_dt"] = _StockTake_Model.stktake_dt;
                    dtrow["stk_rem"] = Session["UserId"].ToString();
                    dtrow["wh_id"] = _StockTake_Model.wh;
                    //dtrow["stktake_status"] = Session["AppStatus"].ToString();
                    dtrow["stktake_status"] = "D";
                    dtrow["CreateBy"] = Session["UserId"].ToString();
                    dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrow["UserIP"] = Session["UserIP"].ToString();
                    //dtrow["TransType"] = Session["TransType"].ToString();
                    if(_StockTake_Model.stktake_no != null)
                    {
                        dtrow["TransType"] = "Update";
                    }
                    else
                    {
                        dtrow["TransType"] = "Save";
                    }
                    dt.Rows.Add(dtrow);
                    StockTakeHeader = dt;
                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(Int32));
                    dtItem.Columns.Add("br_id", typeof(Int32));
                    dtItem.Columns.Add("flag", typeof(string));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("wh_id", typeof(int));
                    dtItem.Columns.Add("avl_stk", typeof(string));
                    dtItem.Columns.Add("phy_stk", typeof(string));
                    dtItem.Columns.Add("item_cost", typeof(string));
                    dtItem.Columns.Add("short_qty", typeof(string));
                    dtItem.Columns.Add("surplus_qty", typeof(string));
                    dtItem.Columns.Add("adj_value", typeof(string));                 
                    dtItem.Columns.Add("remarks", typeof(string));

                    JArray jObject = JArray.Parse(_StockTake_Model.StkTakeItemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal avl_stk, phy_stk, short_qty, surplus_qty, adj_value;
                        if (jObject[i]["AvailableStock"].ToString() == "")
                            avl_stk = 0;
                        else
                            avl_stk = Convert.ToDecimal(jObject[i]["AvailableStock"].ToString());

                        if (jObject[i]["PhysicalStock"].ToString() == "")
                            phy_stk = 0;
                        else
                            phy_stk = Convert.ToDecimal(jObject[i]["PhysicalStock"].ToString());
                        if (jObject[i]["ShortQuantity"].ToString() == "")
                            short_qty = 0;
                        else
                            short_qty = Convert.ToDecimal(jObject[i]["ShortQuantity"].ToString());
                        if (jObject[i]["SurplusQuantity"].ToString() == "")
                            surplus_qty = 0;
                        else
                            surplus_qty = Convert.ToDecimal(jObject[i]["SurplusQuantity"].ToString());
                        if (jObject[i]["AdjustmentValue"].ToString() == "")
                            adj_value = 0;
                        else
                            adj_value = Convert.ToDecimal(jObject[i]["AdjustmentValue"].ToString());

                        DataRow dtrowItemdetails = dtItem.NewRow();
                        dtrowItemdetails["comp_id"] = Session["CompId"].ToString();
                        dtrowItemdetails["br_id"] = Session["BranchId"].ToString();
                        //dtrowItemdetails["flag"] = jObject[i]["FlagForStk"].ToString();
                        var FlgStk = jObject[i]["FlagForStk"].ToString();
                        if (FlgStk == "I" || FlgStk == "N")
                        {
                            dtrowItemdetails["flag"] = "N";
                        }
                        else
                        {
                            dtrowItemdetails["flag"] = "U";
                        }
                        dtrowItemdetails["item_id"] = jObject[i]["ItemId"].ToString();
                        string str = Convert.ToInt32(jObject[i]["UOMId"]).ToString();
                        dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMId"]);
                        dtrowItemdetails["wh_id"] = Convert.ToInt32(jObject[i]["WHId"]);
                        dtrowItemdetails["avl_stk"] = avl_stk;
                        dtrowItemdetails["phy_stk"] = phy_stk;
                        dtrowItemdetails["item_cost"] = jObject[i]["ItemCost"].ToString();
                        dtrowItemdetails["short_qty"] = short_qty;
                        dtrowItemdetails["surplus_qty"] = surplus_qty;
                        dtrowItemdetails["adj_value"] = adj_value;                        
                        dtrowItemdetails["remarks"] = jObject[i]["ItemRemarks"].ToString();

                        dtItem.Rows.Add(dtrowItemdetails);
                    }
                    StockTakeItemDetails = dtItem;

                    DataTable dtItemBatchSerial = new DataTable();
                    dtItemBatchSerial.Columns.Add("comp_id", typeof(Int32));
                    dtItemBatchSerial.Columns.Add("br_id", typeof(Int32));
                    dtItemBatchSerial.Columns.Add("item_id", typeof(string));
                    dtItemBatchSerial.Columns.Add("uom_id", typeof(int));
                    dtItemBatchSerial.Columns.Add("wh_id", typeof(int));
                    dtItemBatchSerial.Columns.Add("lot_no", typeof(string));                   
                    dtItemBatchSerial.Columns.Add("batch_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("serial_no", typeof(string));
                    dtItemBatchSerial.Columns.Add("avl_stk", typeof(string));
                    dtItemBatchSerial.Columns.Add("short_qty", typeof(string));
                    dtItemBatchSerial.Columns.Add("surplus_qty", typeof(string));
                    dtItemBatchSerial.Columns.Add("landed_rate", typeof(string));
                    dtItemBatchSerial.Columns.Add("adj_value", typeof(string));
                    dtItemBatchSerial.Columns.Add("Batchable", typeof(string));
                    dtItemBatchSerial.Columns.Add("Serialable", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_name", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_mrp", typeof(string));
                    dtItemBatchSerial.Columns.Add("mfg_date", typeof(string));
                    dtItemBatchSerial.Columns.Add("exp_date", typeof(string));
                    if (_StockTake_Model.StkTakeItemBatchSerialDetail != null)
                    {
                        JArray jObject1 = JArray.Parse(_StockTake_Model.StkTakeItemBatchSerialDetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            decimal avl_stk, short_qty, surplus_qty, landed_rate, adj_value;
                            if (jObject1[i]["AvaQty"].ToString() == "")
                                avl_stk = 0;
                            else
                                avl_stk = Convert.ToDecimal(jObject1[i]["AvaQty"].ToString());

                            if (jObject1[i]["ShortQty"].ToString() == "")
                                short_qty = 0;
                            else
                                short_qty = Convert.ToDecimal(jObject1[i]["ShortQty"].ToString());

                            if (jObject1[i]["PlusQty"].ToString() == "")
                                surplus_qty = 0;
                            else
                                surplus_qty = Convert.ToDecimal(jObject1[i]["PlusQty"].ToString());

                            if (jObject1[i]["CostPrice"].ToString() == "")
                                landed_rate = 0;
                            else
                                landed_rate = Convert.ToDecimal(jObject1[i]["CostPrice"].ToString());

                            if (jObject1[i]["ItemValue"].ToString() == "")
                                adj_value = 0;
                            else
                                adj_value = Convert.ToDecimal(jObject1[i]["ItemValue"].ToString());


                            DataRow dtrowItemBatchSerialdetails = dtItemBatchSerial.NewRow();
                            dtrowItemBatchSerialdetails["comp_id"] = Session["CompId"].ToString();
                            dtrowItemBatchSerialdetails["br_id"] = Session["BranchId"].ToString();
                            dtrowItemBatchSerialdetails["item_id"] = jObject1[i]["ItmCode"].ToString();
                            string str1 = Convert.ToInt32(jObject1[i]["ItmUomId"]).ToString();
                            dtrowItemBatchSerialdetails["uom_id"] = Convert.ToInt32(jObject1[i]["ItmUomId"]);
                            string Wh_Id = jObject1[i]["wh_id"].ToString();/*add by Hina on 25-07-2024 to fix bug for validation after edit old data then save old data only*/
                            if (Wh_Id=="")
                            {
                                dtrowItemBatchSerialdetails["wh_id"] = 0;
                            }
                            else
                            {
                                dtrowItemBatchSerialdetails["wh_id"] = jObject1[i]["wh_id"].ToString();
                            }
                            //dtrowItemBatchSerialdetails["wh_id"] = Convert.ToInt32(jObject1[i]["wh_id"]);
                            //dtrowItemBatchSerialdetails["wh_id"] = jObject1[i]["wh_id"].ToString();
                            dtrowItemBatchSerialdetails["lot_no"] = jObject1[i]["Lot"].ToString();
                            dtrowItemBatchSerialdetails["batch_no"] = jObject1[i]["Batch"].ToString();
                            dtrowItemBatchSerialdetails["serial_no"] = jObject1[i]["Serial"].ToString();
                            dtrowItemBatchSerialdetails["avl_stk"] = avl_stk;
                            dtrowItemBatchSerialdetails["short_qty"] = short_qty;
                            dtrowItemBatchSerialdetails["surplus_qty"] = surplus_qty;
                            dtrowItemBatchSerialdetails["landed_rate"] = landed_rate;
                            dtrowItemBatchSerialdetails["adj_value"] = adj_value;
                            dtrowItemBatchSerialdetails["Batchable"] = jObject1[i]["Batchable"].ToString();
                            dtrowItemBatchSerialdetails["Serialable"] = jObject1[i]["Serialable"].ToString();
                            dtrowItemBatchSerialdetails["mfg_name"] = commonContr.IsBlank(jObject1[i]["MfgName"].ToString(),null);
                            dtrowItemBatchSerialdetails["mfg_mrp"] = commonContr.IsBlank(jObject1[i]["MfgMrp"].ToString(),null);
                            dtrowItemBatchSerialdetails["mfg_date"] = commonContr.IsBlank(jObject1[i]["MfgDate"].ToString(),null);
                            dtrowItemBatchSerialdetails["exp_date"] = commonContr.IsBlank(jObject1[i]["ExpDate"].ToString(),null);


                            dtItemBatchSerial.Rows.Add(dtrowItemBatchSerialdetails);
                        }
                    }
           
                    StockTakeLotBatchSerial = dtItemBatchSerial;
                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("avl_stk", typeof(string));
                    dtSubItem.Columns.Add("phy_stk", typeof(string));
                    dtSubItem.Columns.Add("short_qty", typeof(string));
                    dtSubItem.Columns.Add("surplus_qty", typeof(string));
                    if (_StockTake_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_StockTake_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["avl_stk"] = jObject2[i]["avl_stk"].ToString();
                            dtrowItemdetails["phy_stk"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["short_qty"] = jObject2[i]["short_qty"].ToString();
                            dtrowItemdetails["surplus_qty"] = jObject2[i]["surplus_qty"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                    }

                    /*------------------Sub Item end----------------------*/
                    String SaveMessage = _StockTake_ISERVICES.InsertStockTakeDetail(StockTakeHeader, StockTakeItemDetails, StockTakeLotBatchSerial, dtSubItem);

                    ////string StockTakeNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    ////string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));

                    string StockTakeNo = SaveMessage.Split(',')[1].Trim();
                    string Message = SaveMessage.Split(',')[0].Trim();
                    string StockTakeDate = SaveMessage.Split(',')[2].Trim();
                    if (Message == "Data_Not_Found")
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _StockTake_Model.Title = title;
                        var msg = Message.Replace("_", " ") + " " + StockTakeNo+" in "+ _StockTake_Model.Title;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _StockTake_Model.Message = Message.Replace("_", "");
                        return RedirectToAction("DeliveryNoteDetail");
                    }

                    if (Message == "Update" || Message == "Save")
                        //    Session["Message"] = "Save";
                        //Session["Command"] = "Update";
                        //Session["StockTakeNo"] = StockTakeNo;
                        //Session["StockTakeDate"] = StockTakeDate;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnToDetailPage";
                        _StockTake_Model.Command = "Update";
                    _StockTake_Model.Message = "Save";
                    _StockTake_Model.StockTakeNo = StockTakeNo;
                    _StockTake_Model.StockTakeDate = StockTakeDate;
                    _StockTake_Model.TransType = "Update";
                    _StockTake_Model.AppStatus = "D";
                    _StockTake_Model.BtnName = "BtnSave";
                    return RedirectToAction("StockTakeDetail");
                }


                else
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }
                    _StockTake_Model.CreatedBy = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    //DataSet SaveMessage = _StockTake_ISERVICES.StockTakeCancel(_StockTake_Model, CompID, br_id, mac_id);
                    //string StockTakeNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["StockTakeNo"] = _StockTake_Model.stktake_no;
                    //Session["StockTakeDate"] = _StockTake_Model.stktake_dt;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _StockTake_Model.stktake_no, "C", UserID, "");
                    _StockTake_Model.Command = "Update";
                    _StockTake_Model.Message = "Cancelled";
                    _StockTake_Model.StockTakeNo = _StockTake_Model.stktake_no;
                    _StockTake_Model.StockTakeDate = _StockTake_Model.stktake_dt.ToString();
                    _StockTake_Model.TransType = "Update";
                    _StockTake_Model.AppStatus = "D";
                    _StockTake_Model.BtnName = "Refresh";
                    return RedirectToAction("StockTakeDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
              //  return View("~/Views/Shared/Error.cshtml");
            }

        }

        public void GetStatusList(StockTakeList_Model _StockTakeList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _StockTakeList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<STKList> GetStockTakeListAll(StockTakeList_Model _StockTakeList_Model)
        {
            try
            {
                _StockTakeList = new List<STKList>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if(_StockTakeList_Model.WF_status != null)
                {
                    wfstatus = _StockTakeList_Model.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataSet Dtdata = new DataSet();
                Dtdata = _StockTake_ISERVICES.GetStockTakeListAll(_StockTakeList_Model.STKFromDate, _StockTakeList_Model.STKToDate, _StockTakeList_Model.Status, CompID, BrchID, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    //FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (Dtdata.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in Dtdata.Tables[0].Rows)
                    {
                        STKList _STKList = new STKList();
                        _STKList.STKNumber = dr["stk_no"].ToString();
                        _STKList.STKDate = dr["stk_dt"].ToString();
                        _STKList.hdSTKDate = dr["stk_date"].ToString();
                        _STKList.Warehouse = dr["wh_name"].ToString();
                        _STKList.STKStatus = dr["stk_status"].ToString();
                        _STKList.CreatedON = dr["created_on"].ToString();
                        _STKList.create_by = dr["create_by"].ToString();
                        _STKList.ApprovedOn = dr["app_dt"].ToString();
                        _STKList.app_by = dr["app_by"].ToString();

                        _StockTakeList.Add(_STKList);
                    }
                }
                return _StockTakeList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult SearchStockTakeDetail(string Fromdate, string Todate, string Status)
        {
            _StockTakeList = new List<STKList>();
            StockTakeList_Model _StockTakeList_Model = new StockTakeList_Model();
            //Session.Remove("WF_status");
            _StockTakeList_Model.WF_status = null;

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            DataSet Dtdata = new DataSet();
            Dtdata = _StockTake_ISERVICES.GetStockTakeListAll(Fromdate, Todate, Status, CompID, BrchID, "", "", "");

            if (Dtdata.Tables[1].Rows.Count > 0)
            {
                //FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
            }
                if (Dtdata.Tables[0].Rows.Count > 0)
            {
                //Session["STKSearch"] = "STKSearch";
                _StockTakeList_Model.STKSearch = "STKSearch";
                foreach (DataRow dr in Dtdata.Tables[0].Rows)
                {
                    STKList _STKList = new STKList();
                    _STKList.STKNumber = dr["stk_no"].ToString();
                    _STKList.STKDate = dr["stk_dt"].ToString();
                    _STKList.hdSTKDate = dr["stk_date"].ToString();
                    _STKList.Warehouse = dr["wh_name"].ToString();
                    _STKList.STKStatus = dr["stk_status"].ToString();
                    _STKList.CreatedON = dr["created_on"].ToString();
                    _STKList.create_by = dr["create_by"].ToString();
                    _STKList.ApprovedOn = dr["app_dt"].ToString();
                    _STKList.app_by = dr["app_by"].ToString();

                    _StockTakeList.Add(_STKList);
                }
            }
            //Session["STKSearch"] = "STKSearch";
            _StockTakeList_Model.STKSearch = "STKSearch";
            _StockTakeList_Model.StockTakeList = _StockTakeList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockTakeList.cshtml", _StockTakeList_Model);
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
        private ActionResult StockTakeDelete(StockTake_Model _StockTake_Model, string command)
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
                DataSet Message = _StockTake_ISERVICES.StockTakeDelete(_StockTake_Model, CompID, BrchID);
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["TRFNo"] = "";
                //_StockTake_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                StockTake_Model _StockTakeDelete_Model = new StockTake_Model();
                _StockTakeDelete_Model.Message = "Deleted";
                _StockTakeDelete_Model.Command = "Refresh";
                _StockTakeDelete_Model.TransType = command;
                _StockTakeDelete_Model.AppStatus = "D";
                _StockTakeDelete_Model.BtnName = "BtnDelete";
                TempData["ModelData"] = _StockTakeDelete_Model;
                URLDetailModel URLModelDelete = new URLDetailModel();
                URLModelDelete.Command = "Refresh";
                URLModelDelete.BtnName = "BtnDelete";
                URLModelDelete.TransType = command;
                return RedirectToAction("StockTakeDetail", "StockTake", URLModelDelete);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult StockTakeApprove(StockTake_Model _StockTake_Model, string STKNo, string STKDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1, string docid)
        {
            try
            {
                //StockTake_Model _StockTake_Model = new StockTake_Model();
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string BranchID = string.Empty;
                string MenuDocId = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                if (docid != null)
                {
                    MenuDocId = docid;
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _StockTake_ISERVICES.StockTakeApprove(STKNo, STKDate, UserID, A_Status, A_Level, A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId);

                _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, STKNo, "AP", UserID, "");
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string StockTakeNo = Message.Split(',')[0].Trim();
                string StockTakeDate = Message.Split(',')[1].Trim();
                if (StockTakeDate == "StockNotAvail")
                {
                    _StockTake_Model.StockTakeDate = Message.Split(',')[2].Trim();
                    _StockTake_Model.StockTakeNo = StockTakeNo;
                    _StockTake_Model.Message = "StockNotAvailable";
                    _StockTake_Model.TransType = "Update";
                    _StockTake_Model.Command = "Save";
                    _StockTake_Model.AppStatus = "D";
                    _StockTake_Model.BtnName = "BtnToDetailPage";
                }
                else
                {
                    _StockTake_Model.StockTakeNo = StockTakeNo;
                    _StockTake_Model.StockTakeDate = StockTakeDate;
                    _StockTake_Model.Message = "Approved";
                    _StockTake_Model.Command = "Approve";
                    _StockTake_Model.TransType = "Update";
                    _StockTake_Model.AppStatus = "D";
                    _StockTake_Model.BtnName = "BtnEdit";
                    _StockTake_Model.WF_status1 = WF_Status1;
                }
                TempData["ModelData"] = _StockTake_Model;
                URLDetailModel _approve_Model = new URLDetailModel();
                _approve_Model.StockTakeNo = StockTakeNo;
                _approve_Model.WF_status1 = _StockTake_Model.WF_status1;
                _approve_Model.StockTakeDate = _StockTake_Model.StockTakeDate;
                _approve_Model.TransType = _StockTake_Model.TransType;
                _approve_Model.BtnName = _StockTake_Model.BtnName;
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("StockTakeDetail", "StockTake", _approve_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1,string dashbordData)
        {
            //Session["Message"] = "";
            StockTake_Model _StockTake_Model = new StockTake_Model();
            _StockTake_Model.Message= "";
            var a = dashbordData.Split(',');
            //_StockTake_Model.docid = a[0].Trim();
            _StockTake_Model.StockTakeNo = a[1].Trim();
            _StockTake_Model.StockTakeDate= a[2].Trim();
            _StockTake_Model.WF_status1 = a[3].Trim();
            _StockTake_Model.TransType = "Update";
            _StockTake_Model.BtnName = "BtnToDetailPage";
            _StockTake_Model.Message = null;
            TempData["ModelData"] = _StockTake_Model;
            URLDetailModel _UrlforModel = new URLDetailModel();
            _UrlforModel.StockTakeNo = _StockTake_Model.StockTakeNo;
            _UrlforModel.WF_status1 = _StockTake_Model.WF_status1;
            _UrlforModel.StockTakeDate = _StockTake_Model.StockTakeDate;
            _UrlforModel.TransType = "Update";
            _UrlforModel.BtnName = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("StockTakeDetail", _UrlforModel);
        }
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string Flag
, string doc_no, string doc_dt, string wh_id,string IsDisabled, string hd_Status,string stqty)
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
                if (hd_Status == "D"|| hd_Status=="F"|| hd_Status=="")
                {
                    Flag = "Quantity";
                    //dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, wh_id, Item_id, null/*UomId*/, "wh").Tables[0];
                    dt.Columns.Add("Qty", typeof(string));
                    dt.Columns.Add("short_qty", typeof(string));
                    dt.Columns.Add("surplus_qty", typeof(string));
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    DataTable NewDt = new DataTable();
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (arr.Count > 0)
                        //{

                        //}
                        //else
                        //{
                        //    if (Convert.ToDouble(stqty) > 0)
                        //    {
                        //        dt.Rows[i]["Qty"] = item.GetValue("qty");
                        //        dt.Rows[i]["short_qty"] = item.GetValue("short_qty");
                        //        dt.Rows[i]["surplus_qty"] = item.GetValue("surplus_qty");
                        //    }
                            
                        //}
                        foreach (JObject item in arr.Children())
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["Qty"] = item.GetValue("qty");
                                dt.Rows[i]["short_qty"] = item.GetValue("short_qty");
                                dt.Rows[i]["surplus_qty"] = item.GetValue("surplus_qty");
                            }
                        }
                    }
                }
                else
                {
                    dt = _StockTake_ISERVICES.StockTake_GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, doc_no, doc_dt, Flag).Tables[0];
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = "StockTake",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y",
                    stqty = stqty
                };
                if (Flag == "AvailableStock")
                {
                    ViewBag.SubitemAvlStockDetail = dt;
                    return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                }
                else
                {
                    return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
                }
                
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetSubItemDetails_StkPopUp(string Item_id, string SubItemListwithPageData, string IsDisabled
           , string Flag, string Status)
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
                int QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                //if (Flag == "PopUpStk_Qty")
                //{
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                        
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                            }
                        }
                    }
                    
                //}
                


                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag ,
                    _subitemPageName = "PR",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y"

                };

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag == "Quantity" ? Flag : "MTO";
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }

    }

}