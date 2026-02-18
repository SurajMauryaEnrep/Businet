using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.Contra
{
    public class ContraController : Controller
    {


        string CompID, Br_ID, Language, title, UserID, FromDate = String.Empty;
        string DocumentMenuId = "105104115125";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ContraVoucher_IService _ContraVoucher_IService;
        DataTable dt;
        List<VouList> _ContraVoucherList;

        public ContraController(Common_IServices _Common_IServices, ContraVoucher_IService _ContraVoucher_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._ContraVoucher_IService = _ContraVoucher_IService;
        }
        // GET: ApplicationLayer/Contra
        public ActionResult Contra(ContraList_Model _ContraList_Model)
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
                Br_ID = Session["BranchId"].ToString();
            }
            var other = new CommonController(_Common_IServices);
            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
            ViewBag.DocumentMenuId = DocumentMenuId;
            DateTime dtnow = DateTime.Now;
            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //ContraList_Model _ContraList_Model = new ContraList_Model();
            GetStatusList(_ContraList_Model);
            GetAutoCompleteBankDetail(_ContraList_Model);
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                //_ContraList_Model.Src_Type = a[0].Trim();
                //_ContraList_Model.bank_id = a[1].Trim();
                _ContraList_Model.VouFromDate = a[0].Trim();
                _ContraList_Model.VouToDate = a[1].Trim();
                _ContraList_Model.Status = a[2].Trim();
                if (_ContraList_Model.Status == "0")
                {
                    _ContraList_Model.Status = null;
                }
                _ContraList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                _ContraList_Model.VoucherList = GetContraVoucherListAll(_ContraList_Model);
            }
            _ContraList_Model.VoucherList = GetContraVoucherListAll(_ContraList_Model);
            if (_ContraList_Model.VouFromDate != null)
            {
                _ContraList_Model.FromDate = _ContraList_Model.VouFromDate;
            }

            //else
            //{
            //    _ContraList_Model.FromDate = startDate;

            //}
            //_ContraList_Model.FromDate = FromDate;
            ViewBag.VBRoleList = GetRoleList();
            ViewBag.MenuPageName = getDocumentName();
            _ContraList_Model.Title = title;
            _ContraList_Model.VouSearch = null;
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/Contra/ContraList.cshtml", _ContraList_Model);
        }
        public ActionResult AddContraDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            ContraVoucherModel _ContraVoucherModel = new ContraVoucherModel();
            ///*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
            //if (Session["CompId"] != null)
            //    CompID = Session["CompId"].ToString();
            //if (Session["BranchId"] != null)
            //    Br_ID = Session["BranchId"].ToString();
            //DateTime dtnow = DateTime.Now;
            //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
            //var commCont = new CommonController(_Common_IServices);

            //string MsgNew = string.Empty;
            //MsgNew = commCont.Fin_CheckFinancialYear(CompID, Br_ID, CurrentDate);
            //if (MsgNew == "FY Not Exist")
            //{
            //    TempData["Message"] = "Financial Year not Exist";
            //    return RedirectToAction("Contra", _ContraVoucherModel);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("Contra", _ContraVoucherModel);
            //}
            ///*End to chk Financial year exist or not*/
            _ContraVoucherModel.Command = "Add";
            _ContraVoucherModel.TransType = "Save";
            _ContraVoucherModel.BtnName = "BtnAddNew";
            _ContraVoucherModel.DocumentStatus = "D";
            TempData["ModelData"] = _ContraVoucherModel;
            UrlModel _urlModel = new UrlModel();
            _urlModel.bt = "BtnAddNew";
            _urlModel.Cmd = "Add";
            _urlModel.tp = "Save";
            ViewBag.MenuPageName = getDocumentName();
            TempData["ListFilterData"] = null;
            return RedirectToAction("ContraDetail", "Contra", _urlModel);
        }
        public ActionResult ContraDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
                ContraVoucherModel _ContraVoucherModel = new ContraVoucherModel();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
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
                var _ContraVoucherModel1 = TempData["ModelData"] as ContraVoucherModel;
                if (_ContraVoucherModel1 != null)
                {
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ContraVoucherModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, Br_ID, DocumentMenuId).Tables[0];

                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_ContraVoucherModel1.TransType == "Update" || _ContraVoucherModel1.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["ContraNo"].ToString();
                        //string VouDt = Session["ContraDate"].ToString();
                        string VouNo = _ContraVoucherModel1.ContraNo;
                        string VouDt = _ContraVoucherModel1.ContraDate;
                        DataSet ds = _ContraVoucher_IService.GetContraVoucherDetail(VouNo, VouDt, CompID, Br_ID, UserID, DocumentMenuId);
                        //_ContraVoucherModel1.bank_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _ContraVoucherModel1.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _ContraVoucherModel1.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        //_ContraVoucherModel1.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        //_ContraVoucherModel1.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        //_ContraVoucherModel1.src_doc_date = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        // _ContraVoucherModel1.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        //_ContraVoucherModel1.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        //_ContraVoucherModel1.conv_rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(RateDigit);
                        //_ContraVoucherModel1.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);


                        _ContraVoucherModel1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _ContraVoucherModel1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _ContraVoucherModel1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _ContraVoucherModel1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _ContraVoucherModel1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _ContraVoucherModel1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _ContraVoucherModel1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _ContraVoucherModel1.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _ContraVoucherModel1.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _ContraVoucherModel1.DocumentStatus = Statuscode;
                        if (_ContraVoucherModel1.Status == "C")
                        {
                            _ContraVoucherModel1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _ContraVoucherModel1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _ContraVoucherModel1.BtnName = "Refresh";
                        }
                        else
                        {
                            _ContraVoucherModel1.CancelFlag = false;
                        }

                        _ContraVoucherModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _ContraVoucherModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }


                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _ContraVoucherModel1.Command != "Edit")
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

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _ContraVoucherModel1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message"] != null)
                                            {
                                                ViewBag.Message = TempData["Message"];
                                            }
                                            if (TempData["FBMessage"] != null)
                                            {
                                                ViewBag.MessageFB = TempData["FBMessage"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ContraVoucherModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _ContraVoucherModel1.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _ContraVoucherModel1.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _ContraVoucherModel1.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        // _ContraVoucherModel1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ContraVoucherModel1.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                        }
                                        if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ContraVoucherModel1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ContraVoucherModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                        }
                                        if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    // Session["BtnName"] = "BtnToDetailPage";
                                    _ContraVoucherModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ContraVoucherModel1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _ContraVoucherModel1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                            /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                            if (TempData["Message"] != null)
                            {
                                ViewBag.Message = TempData["Message"];
                            }
                            if (TempData["FBMessage"] != null)
                            {
                                ViewBag.MessageFB = TempData["FBMessage"];
                            }
                            /*End to chk Financial year exist or not*/
                        }

                        ViewBag.DiffAmt = Convert.ToDecimal(0).ToString(ValDigit);
                        //ViewBag.TotalVouAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        ViewBag.TotalVouAmt = ds.Tables[0].Rows[0]["vou_amt"];
                        ViewBag.MenuPageName = getDocumentName();
                        _ContraVoucherModel1.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/Contra/ContraDetail.cshtml", _ContraVoucherModel1);
                    }
                    else
                    {

                        ViewBag.MenuPageName = getDocumentName();
                        ViewBag.VBRoleList = GetRoleList();
                        _ContraVoucherModel1.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/Contra/ContraDetail.cshtml", _ContraVoucherModel1);
                    }
                }
                else
                {
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ContraVoucherModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, Br_ID, DocumentMenuId).Tables[0];
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _ContraVoucherModel.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _ContraVoucherModel.BtnName = _urlModel.bt;
                        }
                        _ContraVoucherModel.ContraNo = _urlModel.CNO;
                        _ContraVoucherModel.ContraDate = _urlModel.CDT;
                        _ContraVoucherModel.Command = _urlModel.Cmd;
                        _ContraVoucherModel.TransType = _urlModel.tp;
                        _ContraVoucherModel.WF_Status1 = _urlModel.wf;
                        _ContraVoucherModel.DocumentStatus = _urlModel.DMS;
                    }
                    /* Add by Hina on 23-02-2024 to Refresh Page*/
                    if (_ContraVoucherModel.TransType == null)
                    {
                        _ContraVoucherModel.BtnName = "Refresh";
                        _ContraVoucherModel.Command = "Refresh";
                        _ContraVoucherModel.TransType = "Refresh";
                    }
                    /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var VouDate = _ContraVoucherModel.ContraDate;
                    //var VouDate = "";
                    
                    //if (_ContraVoucherModel.ContraDate != null)
                    //{
                    //    VouDate = _ContraVoucherModel.ContraDate;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    _ContraVoucherModel.ContraDate = CurrentDate;
                    //    _ContraVoucherModel.Vou_Date = CurrentDate;
                    //    VouDate = _ContraVoucherModel.ContraDate;
                    //}
                    //if (commCont.Fin_CheckFinancialYear(CompID, Br_ID, VouDate) == "FY Not Exist")
                    //{
                    //    TempData["Message"] = "Financial Year not Exist";
                    //}
                    //if (commCont.Fin_CheckFinancialYear(CompID, Br_ID, VouDate) == "FB Close")
                    //{
                    //    TempData["FBMessage"] = "Financial Book Closing";
                    //}
                    ///*End to chk Financial year exist or not*/
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_ContraVoucherModel.TransType == "Update" || _ContraVoucherModel.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["ContraNo"].ToString();
                        //string VouDt = Session["ContraDate"].ToString();
                        string VouNo = _ContraVoucherModel.ContraNo;
                        string VouDt = _ContraVoucherModel.ContraDate;
                        DataSet ds = _ContraVoucher_IService.GetContraVoucherDetail(VouNo, VouDt, CompID, Br_ID, UserID, DocumentMenuId);
                        //_ContraVoucherModel.bank_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _ContraVoucherModel.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _ContraVoucherModel.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        //_ContraVoucherModel.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        //_ContraVoucherModel.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        //_ContraVoucherModel.src_doc_date = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        // _ContraVoucherModel.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        //_ContraVoucherModel.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        //_ContraVoucherModel.conv_rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(RateDigit);
                        //_ContraVoucherModel.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);


                        _ContraVoucherModel.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _ContraVoucherModel.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _ContraVoucherModel.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _ContraVoucherModel.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _ContraVoucherModel.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _ContraVoucherModel.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _ContraVoucherModel.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _ContraVoucherModel.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _ContraVoucherModel.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _ContraVoucherModel.DocumentStatus = Statuscode;
                        if (_ContraVoucherModel.Status == "C")
                        {
                            _ContraVoucherModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _ContraVoucherModel.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _ContraVoucherModel.BtnName = "Refresh";
                        }
                        else
                        {
                            _ContraVoucherModel.CancelFlag = false;
                        }

                        _ContraVoucherModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _ContraVoucherModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }


                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _ContraVoucherModel.Command != "Edit")
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

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _ContraVoucherModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message"] != null)
                                            {
                                                ViewBag.Message = TempData["Message"];
                                            }
                                            if (TempData["FBMessage"] != null)
                                            {
                                                ViewBag.MessageFB = TempData["FBMessage"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ContraVoucherModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _ContraVoucherModel.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _ContraVoucherModel.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _ContraVoucherModel.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        // _ContraVoucherModel.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ContraVoucherModel.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                        }
                                        if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ContraVoucherModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ContraVoucherModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                        }
                                        if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    // Session["BtnName"] = "BtnToDetailPage";
                                    _ContraVoucherModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ContraVoucherModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _ContraVoucherModel.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                            /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                            if (TempData["Message"] != null)
                            {
                                ViewBag.Message = TempData["Message"];
                            }
                            if (TempData["FBMessage"] != null)
                            {
                                ViewBag.MessageFB = TempData["FBMessage"];
                            }
                            /*End to chk Financial year exist or not*/
                        }

                        ViewBag.DiffAmt = Convert.ToDecimal(0).ToString(ValDigit);
                        //ViewBag.TotalVouAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        ViewBag.TotalVouAmt = ds.Tables[0].Rows[0]["vou_amt"];
                        ViewBag.MenuPageName = getDocumentName();
                        _ContraVoucherModel.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/Contra/ContraDetail.cshtml", _ContraVoucherModel);
                    }
                    else
                    {

                        ViewBag.MenuPageName = getDocumentName();
                        ViewBag.VBRoleList = GetRoleList();
                        _ContraVoucherModel.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/Contra/ContraDetail.cshtml", _ContraVoucherModel);
                    }
                }


            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
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
                    Language = Session["Language"].ToString();
                }
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, Language);
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
        public ActionResult GetContraList(string docid, string status)
        {

            //Session["WF_status"] = status;
            ContraList_Model Dashbord = new ContraList_Model();
            //Session["WF_status"] = status;
            Dashbord.WF_Status = status;
            return RedirectToAction("Contra", Dashbord);
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
        public ActionResult EditVou(string VouNo, string Voudt, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
            ContraVoucherModel dblclick = new ContraVoucherModel();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            string DblClkMsg = string.Empty;
            DblClkMsg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
            if (DblClkMsg == "FY Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                dblclick.BtnName = "Refresh";
            }
            else if (DblClkMsg == "FB Close")
            {
                TempData["FBMessage"] = "Financial Book Closing";
                dblclick.BtnName = "Refresh";
            }
            else
            {
                dblclick.BtnName = "BtnToDetailPage";
            }

            /*End to chk Financial year exist or not*/
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["ContraNo"] = VouNo;
            //Session["ContraDate"] = Voudt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            //ContraVoucherModel dblclick = new ContraVoucherModel();
            UrlModel _url = new UrlModel();
            dblclick.Command = "Update";
            dblclick.ContraNo = VouNo;
            dblclick.ContraDate = Voudt;
            dblclick.TransType = "Update";
            //dblclick.BtnName = "BtnToDetailPage";
            if (WF_Status != null && WF_Status != "")
            {
                _url.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            _url.Cmd = "Update";
            _url.tp = "Update";
            _url.bt = "D";
            _url.CNO = VouNo;
            _url.CDT = Voudt;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("ContraDetail", "Contra", _url);
        }
        public void GetStatusList(ContraList_Model _ContraList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _ContraList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<VouList> GetContraVoucherListAll(ContraList_Model _ContraList_Model)
        {
            try
            {
                _ContraVoucherList = new List<VouList>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                Br_ID = Session["BranchId"].ToString();
                string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if (_ContraList_Model.WF_Status != null)
                {
                    wfstatus = _ContraList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string VouType = "CT";
                DataSet Dtdata = new DataSet();
                Dtdata = _ContraVoucher_IService.GetContraVoucherListAll(_ContraList_Model.VouFromDate, _ContraList_Model.VouToDate, _ContraList_Model.Status, CompID, Br_ID, VouType, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    _ContraList_Model.FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (Dtdata.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in Dtdata.Tables[0].Rows)
                    {
                        VouList _VouList = new VouList();

                        _VouList.VouNumber = dr["vou_no"].ToString();
                        _VouList.VouDate = dr["vou_dt"].ToString();
                        _VouList.hdVouDate = dr["vou_date"].ToString();
                        _VouList.Amount = dr["vou_amt"].ToString();
                        _VouList.VouStatus = dr["vou_status"].ToString();
                        _VouList.CreatedON = dr["created_on"].ToString();
                        _VouList.ApprovedOn = dr["app_dt"].ToString();
                        _VouList.ModifiedOn = dr["mod_on"].ToString();
                        _VouList.create_by = dr["create_by"].ToString();
                        _VouList.app_by = dr["app_by"].ToString();
                        _VouList.mod_by = dr["mod_by"].ToString();

                        _ContraVoucherList.Add(_VouList);
                    }
                }
                return _ContraVoucherList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult SearchContraVoucherDetail(string Fromdate, string Todate, string Status, string CompID, string Br_ID)
        {
            _ContraVoucherList = new List<VouList>();
            ContraList_Model _ContraList_Model = new ContraList_Model();
            // Session["WF_status"] = "";
            _ContraList_Model.WF_Status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            string VouType = "CT";
            DataSet dt = new DataSet();
            dt = _ContraVoucher_IService.GetContraVoucherListAll(Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
            //Session["VouSearch"] = "Vou_Search";
            _ContraList_Model.VouSearch = "Vou_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    VouList _VouList = new VouList();
                    _VouList.VouNumber = dr["vou_no"].ToString();
                    _VouList.VouDate = dr["vou_dt"].ToString();
                    _VouList.hdVouDate = dr["vou_date"].ToString();
                    _VouList.Amount = dr["vou_amt"].ToString();
                    _VouList.VouStatus = dr["vou_status"].ToString();
                    _VouList.CreatedON = dr["created_on"].ToString();
                    _VouList.ApprovedOn = dr["app_dt"].ToString();
                    _VouList.ModifiedOn = dr["mod_on"].ToString();
                    _VouList.create_by = dr["create_by"].ToString();
                    _VouList.app_by = dr["app_by"].ToString();
                    _VouList.mod_by = dr["mod_by"].ToString();

                    _ContraVoucherList.Add(_VouList);
                }
            }
            _ContraList_Model.VoucherList = _ContraVoucherList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialContraList.cshtml", _ContraList_Model);
        }

        public ActionResult GetAutoCompleteBankDetail(ContraList_Model _ContraList_Model)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> BankAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_ContraList_Model.bank_name))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _ContraList_Model.bank_name;
                }
                Br_ID = Session["BranchId"].ToString();
                BankAccList = _ContraVoucher_IService.AutoGetBankAccList(Comp_ID, Acc_Name, Br_ID);

                List<BankAccList> _BankAccNameList = new List<BankAccList>();
                foreach (var dr in BankAccList)
                {
                    BankAccList _BankAccName = new BankAccList();
                    _BankAccName.bank_acc_id = dr.Key;
                    _BankAccName.bank_acc_name = dr.Value;
                    _BankAccNameList.Add(_BankAccName);
                }
                _ContraList_Model.BankAccNameList = _BankAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(BankAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


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
        //public ActionResult GetAutoCompleteGLDetail(ContraVoucherModel _ContraVoucherModel)
        //{
        //    string Acc_Name = string.Empty;
        //    Dictionary<string, string> BankAccList = new Dictionary<string, string>();
        //    string Comp_ID = string.Empty;

        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (string.IsNullOrEmpty(_ContraVoucherModel.BankName))
        //        {
        //            Acc_Name = "0";
        //        }
        //        else
        //        {
        //            Acc_Name = _ContraVoucherModel.BankName;
        //        }
        //        Br_ID = Session["BranchId"].ToString();
        //        BankAccList = _ContraVoucher_IService.AutoGetBankAccList(Comp_ID, Acc_Name, Br_ID);

        //        List<BankAccName> _BankAccNameList = new List<BankAccName>();
        //        foreach (var dr in BankAccList)
        //        {
        //            BankAccName _BankAccName = new BankAccName();
        //            _BankAccName.bank_acc_id = dr.Key;
        //            _BankAccName.bank_acc_name = dr.Value;
        //            _BankAccNameList.Add(_BankAccName);
        //        }
        //        _ContraVoucherModel.BankAccNameList = _BankAccNameList;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Json(BankAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        //}

        [HttpPost]
        public JsonResult GetAccCLbal(string acc_id, string Date)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                DataSet result = _ContraVoucher_IService.GetAccCLBal(acc_id, CompID, Br_ID, Date);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw Ex;
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ContraSave(ContraVoucherModel _ContraVoucherModel, string Vou_No, string command)
        {
            try
            {
                /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                if (_ContraVoucherModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        //Session["Message"] = "New";
                        //Session["Command"] = "Add";
                        //Session["DocumentStatus"] = 'D';
                        //Session["TransType"] = "Save";
                        //Session["BtnName"] = "BtnAddNew";
                        ContraVoucherModel adddnew = new ContraVoucherModel();
                        adddnew.Command = "Add";
                        adddnew.TransType = "Save";
                        adddnew.BtnName = "BtnAddNew";
                        adddnew.DocumentStatus = "D";
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "Add";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";
                        NewModel.DMS = "D";
                        TempData["ModelData"] = adddnew;
                        TempData["ListFilterData"] = null;
                        ///*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
                        //if (Session["CompId"] != null)
                        //    CompID = Session["CompId"].ToString();
                        //if (Session["BranchId"] != null)
                        //    Br_ID = Session["BranchId"].ToString();
                        //DateTime dtnow = DateTime.Now;
                        //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month,dtnow.Day).ToString("yyyy-MM-dd");
                        //string MsgNew = string.Empty;
                        //MsgNew = commCont.Fin_CheckFinancialYear(CompID, Br_ID, CurrentDate);
                        //if (MsgNew == "FY Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("ContraDetail", "Contra", adddnew);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("ContraDetail", "Contra", adddnew);
                        //}
                        ///*End to chk Financial year exist or not*/
                        return RedirectToAction("ContraDetail", "Contra", NewModel);

                    case "Edit":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt = _ContraVoucherModel.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_ContraVoucherModel.Status == "A"|| _ContraVoucherModel.Status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("EditVou", new { VouNo = _ContraVoucherModel.Vou_No, Voudt = _ContraVoucherModel.Vou_Date, ListFilterData = _ContraVoucherModel.ListFilterData1, WF_Status = _ContraVoucherModel.WFStatus });
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        _ContraVoucherModel.TransType = "Update";
                        _ContraVoucherModel.Command = command;
                        _ContraVoucherModel.BtnName = "BtnEdit";
                        _ContraVoucherModel.ContraNo = _ContraVoucherModel.Vou_No;
                        _ContraVoucherModel.ContraDate = _ContraVoucherModel.Vou_Date;
                        TempData["ModelData"] = _ContraVoucherModel;
                        UrlModel EditModel = new UrlModel();
                        EditModel.tp = "Update";
                        EditModel.Cmd = command;
                        EditModel.bt = "BtnEdit";
                        EditModel.CNO = _ContraVoucherModel.Vou_No;
                        EditModel.CDT = _ContraVoucherModel.Vou_Date;
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["BtnName"] = "BtnEdit";
                        //Session["Message"] = "New";
                        //Session["AppStatus"] = 'D';
                        //Session["ContraNo"] = _ContraVoucherModel.Vou_No;
                        //Session["ContraDate"] = _ContraVoucherModel.Vou_Date.ToString();                        
                        TempData["ListFilterData"] = _ContraVoucherModel.ListFilterData1;
                        return RedirectToAction("ContraDetail", EditModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        _ContraVoucherModel.Command = command;
                        Vou_No = _ContraVoucherModel.Vou_No;
                        ContraDelete(_ContraVoucherModel, command);
                        ContraVoucherModel DeleteModel = new ContraVoucherModel();

                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete = new UrlModel();
                        Delete.Cmd = DeleteModel.Command;
                        Delete.tp = "Refresh";
                        Delete.bt = "BtnDelete";
                        TempData["ListFilterData"] = _ContraVoucherModel.ListFilterData1;
                        return RedirectToAction("ContraDetail", Delete);


                    case "Save":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _ContraVoucherModel.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt1);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (Msg == "FY Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                            }
                            else
                            {
                                TempData["FBMessage"] = "Financial Book Closing";
                            }
                           if( _ContraVoucherModel.Vou_No==null)
                            {
                                _ContraVoucherModel.Command = "Refresh";
                                _ContraVoucherModel.TransType = "Refresh";
                                _ContraVoucherModel.BtnName = "Refresh";
                                _ContraVoucherModel.DocumentStatus = null;
                                TempData["ModelData"] = _ContraVoucherModel;
                                return RedirectToAction("ContraDetail", "Contra", _ContraVoucherModel);
                            }
                            else
                            {
                                return RedirectToAction("EditVou", new { VouNo = _ContraVoucherModel.Vou_No, Voudt = _ContraVoucherModel.Vou_Date, ListFilterData = _ContraVoucherModel.ListFilterData1, WF_Status = _ContraVoucherModel.WFStatus });

                            }
                        }
                        /*End to chk Financial year exist or not*/

                        //Session["Command"] = command;
                        _ContraVoucherModel.Command = command;
                        SaveContra(_ContraVoucherModel);
                        if (_ContraVoucherModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_ContraVoucherModel.Message == "N")
                        {
                            //List<curr> _currList = new List<curr>();
                            //dt = Getcurr();
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    curr _curr = new curr();
                            //    _curr.curr_id = dr["curr_id"].ToString();
                            //    _curr.curr_name = dr["curr_name"].ToString();
                            //    _currList.Add(_curr);

                            //}
                            //_currList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                            //_BankPayment_Model.currList = _currList;
                            ViewBag.MenuPageName = getDocumentName();
                            ViewBag.VouDetails = ViewData["VouDetails"];
                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                            DataTable dtVouDetails = ViewData["VouDetails"] as DataTable;

                            decimal TotalDr = 0;
                            decimal TotalCr = 0;
                            for (int i = 0; i < dtVouDetails.Rows.Count; i++)
                            {
                                TotalDr = TotalDr + Convert.ToDecimal(dtVouDetails.Rows[i]["dr_amt_bs"]);
                            }
                            for (int i = 0; i < dtVouDetails.Rows.Count; i++)
                            {
                                TotalCr = TotalCr + Convert.ToDecimal(dtVouDetails.Rows[i]["cr_amt_bs"]);
                            }
                            ViewBag.TotalVouAmt = TotalDr;
                            ViewBag.DiffAmt = TotalDr - TotalCr;
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                            _ContraVoucherModel.BtnName = "BtnAddNew";
                            _ContraVoucherModel.Command = "Add";
                            _ContraVoucherModel.Message = "N";
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/Contra/ContraDetail.cshtml", _ContraVoucherModel);

                        }
                        else
                        {
                            //Session["ContraNo"] = Session["ContraNo"].ToString();
                            //Session["ContraDate"] = Session["ContraDate"].ToString();
                            TempData["ModelData"] = _ContraVoucherModel;
                            UrlModel _urlModel = new UrlModel();
                            _urlModel.bt = _ContraVoucherModel.BtnName;
                            _urlModel.Cmd = _ContraVoucherModel.Command;
                            _urlModel.CNO = _ContraVoucherModel.ContraNo;
                            _urlModel.CDT = _ContraVoucherModel.ContraDate;
                            _urlModel.tp = _ContraVoucherModel.TransType;
                            TempData["ListFilterData"] = _ContraVoucherModel.ListFilterData1;
                            return RedirectToAction("ContraDetail", _urlModel);
                        }

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _ContraVoucherModel.Vou_Date;

                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt3);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (Msg == "FY Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                            }
                            else
                            {
                                TempData["FBMessage"] = "Financial Book Closing";
                            }
                            return RedirectToAction("EditVou", new { VouNo = _ContraVoucherModel.Vou_No, Voudt = _ContraVoucherModel.Vou_Date, ListFilterData = _ContraVoucherModel.ListFilterData1, WF_Status = _ContraVoucherModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/

                        //Session["Command"] = command;
                        Vou_No = _ContraVoucherModel.Vou_No;
                        //Session["ContraNo"] = Vou_No;
                        //Session["ContraDate"] = _ContraVoucherModel.Vou_Date;
                        ContraApprove(_ContraVoucherModel, _ContraVoucherModel.Vou_No, _ContraVoucherModel.Vou_Date, "", "", "", "", "");
                        TempData["ModelData"] = _ContraVoucherModel;
                        UrlModel urlref = new UrlModel();
                        urlref.tp = "Update";
                        urlref.CNO = _ContraVoucherModel.ContraNo;
                        urlref.CDT = _ContraVoucherModel.ContraDate;
                        urlref.bt = "BtnEdit";
                        TempData["ListFilterData"] = _ContraVoucherModel.ListFilterData1;
                        return RedirectToAction("ContraDetail", urlref);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        //Session["DocumentStatus"] = 'D';
                        ContraVoucherModel RefreshModel = new ContraVoucherModel();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _ContraVoucherModel.ListFilterData1;
                        return RedirectToAction("ContraDetail", refesh);

                    case "Print":
                        return GenratePdfFile(_ContraVoucherModel);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        ContraList_Model _Backtolist = new ContraList_Model();
                        _Backtolist.WF_Status = _ContraVoucherModel.WF_Status1;
                        TempData["ListFilterData"] = _ContraVoucherModel.ListFilterData1;
                        return RedirectToAction("Contra", "Contra", _Backtolist);

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
        public ActionResult SaveContra(ContraVoucherModel _ContraVoucherModel)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (_ContraVoucherModel.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }
                    Br_ID = Session["BranchId"].ToString();
                    DataTable ContraHeader = new DataTable();
                    DataTable ContraGLDetails = new DataTable();

                    DataTable dtheader = new DataTable();

                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("user_id", typeof(int));
                    dtheader.Columns.Add("vou_type", typeof(string));
                    dtheader.Columns.Add("vou_no", typeof(string));
                    dtheader.Columns.Add("vou_dt", typeof(string));
                    dtheader.Columns.Add("src_doc", typeof(string));
                    dtheader.Columns.Add("src_doc_no", typeof(string));
                    dtheader.Columns.Add("src_doc_dt", typeof(string));
                    dtheader.Columns.Add("vou_amt", typeof(string));
                    dtheader.Columns.Add("remarks", typeof(string));
                    dtheader.Columns.Add("vou_status", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));

                    dtheader.Columns.Add("ins_type", typeof(string));
                    dtheader.Columns.Add("ins_no", typeof(string));
                    dtheader.Columns.Add("ins_dt", typeof(string));
                    dtheader.Columns.Add("ins_name", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();
                    //dtrowHeader["TransType"] = Session["TransType"].ToString();
                    if (_ContraVoucherModel.Vou_No != null)
                    {
                        dtrowHeader["TransType"] = "Update";
                    }
                    else
                    {
                        dtrowHeader["TransType"] = "Save";
                    }

                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    dtrowHeader["comp_id"] = Session["CompId"].ToString();
                    dtrowHeader["br_id"] = Session["BranchId"].ToString();
                    dtrowHeader["user_id"] = Session["UserId"].ToString();
                    dtrowHeader["vou_type"] = "CT";
                    dtrowHeader["vou_no"] = _ContraVoucherModel.Vou_No;
                    dtrowHeader["vou_dt"] = _ContraVoucherModel.Vou_Date;
                    dtrowHeader["src_doc"] = null;
                    dtrowHeader["src_doc_no"] = null;
                    dtrowHeader["src_doc_dt"] = null;
                    dtrowHeader["vou_amt"] = "0";
                    dtrowHeader["remarks"] = null;
                    dtrowHeader["vou_status"] = "D";
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrowHeader["mac_id"] = mac_id;

                    dtrowHeader["ins_type"] = null;
                    dtrowHeader["ins_no"] = null;
                    dtrowHeader["ins_dt"] = null;
                    dtrowHeader["ins_name"] = null;


                    dtheader.Rows.Add(dtrowHeader);
                    ContraHeader = dtheader;

                    DataTable dtAccount = new DataTable();

                    dtAccount.Columns.Add("acc_id", typeof(string));
                    dtAccount.Columns.Add("acc_type", typeof(int));
                    dtAccount.Columns.Add("curr_id", typeof(int));
                    dtAccount.Columns.Add("conv_rate", typeof(string));
                    dtAccount.Columns.Add("dr_amt_bs", typeof(string));
                    dtAccount.Columns.Add("cr_amt_bs", typeof(string));
                    dtAccount.Columns.Add("dr_amt_sp", typeof(string));
                    dtAccount.Columns.Add("cr_amt_sp", typeof(string));
                    dtAccount.Columns.Add("narr", typeof(string));
                    dtAccount.Columns.Add("seq_no", typeof(int));
                    JArray jObject = JArray.Parse(_ContraVoucherModel.GlAccountDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtAccount.NewRow();

                        dtrowLines["acc_id"] = jObject[i]["acc_id"].ToString();
                        dtrowLines["acc_type"] = jObject[i]["acc_type"].ToString();
                        dtrowLines["curr_id"] = jObject[i]["curr_id"].ToString();
                        dtrowLines["conv_rate"] = jObject[i]["conv_rate"].ToString();
                        if (jObject[i]["dr_amt_sp"].ToString() == "")
                        {
                            dtrowLines["dr_amt_bs"] = 0;
                            dtrowLines["dr_amt_sp"] = 0;
                        }
                        else
                        {
                            dtrowLines["dr_amt_bs"] = string.IsNullOrEmpty(jObject[i]["dr_amt_bs"].ToString()) == true ? "0" : jObject[i]["dr_amt_bs"].ToString();
                            dtrowLines["dr_amt_sp"] = string.IsNullOrEmpty(jObject[i]["dr_amt_sp"].ToString()) == true ? "0" : jObject[i]["dr_amt_sp"].ToString();
                        }
                        if (jObject[i]["cr_amt_sp"].ToString() == "")
                        {
                            dtrowLines["cr_amt_bs"] = 0;
                            dtrowLines["cr_amt_sp"] = 0;
                        }
                        else
                        {
                            dtrowLines["cr_amt_bs"] = string.IsNullOrEmpty(jObject[i]["cr_amt_bs"].ToString()) == true ? "0" : jObject[i]["cr_amt_bs"].ToString();
                            dtrowLines["cr_amt_sp"] = string.IsNullOrEmpty(jObject[i]["cr_amt_sp"].ToString()) == true ? "0" : jObject[i]["cr_amt_sp"].ToString();
                        }
                        dtrowLines["narr"] = jObject[i]["narr"].ToString();
                        dtrowLines["seq_no"] = jObject[i]["seq_no"].ToString();
                        dtAccount.Rows.Add(dtrowLines);
                    }
                    ContraGLDetails = dtAccount;
                    ViewData["VouDetails"] = DtVouDetails(jObject);

                    /*-----------------Attachment Section Start------------------------*/
                    DataTable ContraAttachments = new DataTable();
                    DataTable ContradtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as ContraModel;
                    TempData["IMGDATA"] = null;
                    if (_ContraVoucherModel.attatchmentdetail != null)
                    {
                        if (attachData != null)
                        {

                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                ContradtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                ContradtAttachment.Columns.Add("id", typeof(string));
                                ContradtAttachment.Columns.Add("file_name", typeof(string));
                                ContradtAttachment.Columns.Add("file_path", typeof(string));
                                ContradtAttachment.Columns.Add("file_def", typeof(char));
                                ContradtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_ContraVoucherModel.AttachMentDetailItmStp != null)
                            {
                                //CRdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                ContradtAttachment = _ContraVoucherModel.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                ContradtAttachment.Columns.Add("id", typeof(string));
                                ContradtAttachment.Columns.Add("file_name", typeof(string));
                                ContradtAttachment.Columns.Add("file_path", typeof(string));
                                ContradtAttachment.Columns.Add("file_def", typeof(char));
                                ContradtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_ContraVoucherModel.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in ContradtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = ContradtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_ContraVoucherModel.Vou_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = _ContraVoucherModel.Vou_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                ContradtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_ContraVoucherModel.TransType == "Update")
                        {
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string BP_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_ContraVoucherModel.Vou_No).ToString()))
                                {
                                    BP_CODE = (_ContraVoucherModel.Vou_No).ToString();

                                }
                                else
                                {
                                    BP_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + Br_ID + BP_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in ContradtAttachment.Rows)
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
                        ContraAttachments = ContradtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/



                    SaveMessage = _ContraVoucher_IService.InsertContraDetail(ContraHeader, ContraGLDetails, ContraAttachments);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 25-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //_BankPayment_Model.Message = "Financial Year not Exist";
                        _ContraVoucherModel.BtnName = "Refresh";
                        _ContraVoucherModel.Command = "Refresh";
                        _ContraVoucherModel.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;
                        return RedirectToAction("ContraDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //_BankPayment_Model.Message = "Financial Book Closing";
                        _ContraVoucherModel.BtnName = "Refresh";
                        _ContraVoucherModel.Command = "Refresh";
                        _ContraVoucherModel.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;

                        return RedirectToAction("ContraDetail");
                    }
                    else
                    {
                        if (SaveMessage == "N")
                        {
                            _ContraVoucherModel.Message = "N";
                            return RedirectToAction("ContraDetail");
                        }
                        else
                        {
                            string ContraNo = SaveMessage.Split(',')[1].Trim();
                            string Contra_Number = ContraNo.Replace("/", "");
                            string Message = SaveMessage.Split(',')[0].Trim();
                            string ContraDate = SaveMessage.Split(',')[2].Trim();
                            if (Message == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message.Replace("_", " ") + " " + ContraNo + " in " + PageName;//ContraNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _ContraVoucherModel.Message = Message.Split(',')[0].Replace("_", "");
                                return RedirectToAction("ContraDetail");
                            }
                            /*-----------------Attachment Section Start------------------------*/
                            if (Message == "Save")

                            {
                                string Guid = "";
                                //if (Session["Guid"] != null)
                                if (attachData != null)
                                {
                                    if (attachData.Guid != null)
                                    {
                                        Guid = attachData.Guid;
                                    }
                                }
                                string guid = Guid;
                                var comCont = new CommonController(_Common_IServices);
                                comCont.ResetImageLocation(CompID, Br_ID, guid, PageName, Contra_Number, _ContraVoucherModel.TransType, ContraAttachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{
                                //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Br_ID + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in ContraAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = CompID + Br_ID + Contra_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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
                            /*-----------------Attachment Section End------------------------*/

                            if (Message == "Update" || Message == "Save")
                                //    Session["Message"] = "Save";
                                //Session["Command"] = "Update";
                                //Session["ContraNo"] = ContraNo;
                                //Session["ContraDate"] = ContraDate;
                                //Session["TransType"] = "Update";
                                //Session["AppStatus"] = 'D';
                                //Session["BtnName"] = "BtnToDetailPage";
                                _ContraVoucherModel.Message = "Save";
                            _ContraVoucherModel.Command = "Update";
                            _ContraVoucherModel.ContraNo = ContraNo;
                            _ContraVoucherModel.ContraDate = ContraDate;
                            _ContraVoucherModel.TransType = "Update";
                            _ContraVoucherModel.BtnName = "BtnToDetailPage";
                            return RedirectToAction("ContraDetail");
                        }
                    }
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
                    _ContraVoucherModel.Create_by = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet SaveMessage1 = _ContraVoucher_IService.ContraVoucherCancel(_ContraVoucherModel, CompID, br_id, mac_id);

                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["ContraNo"] = _ContraVoucherModel.Vou_No;
                    //Session["ContraDate"] = _ContraVoucherModel.Vou_Date;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    try {
                        //string fileName = "CT_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "ContraVoucher_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_ContraVoucherModel.Vou_No, _ContraVoucherModel.Vou_Date, fileName, DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _ContraVoucherModel.Vou_No, "C", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _ContraVoucherModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    // _ContraVoucherModel.Message = "Cancelled";
                    _ContraVoucherModel.Message = _ContraVoucherModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    _ContraVoucherModel.Command = "Update";
                    _ContraVoucherModel.ContraNo = _ContraVoucherModel.Vou_No;
                    _ContraVoucherModel.ContraDate = _ContraVoucherModel.Vou_Date;
                    _ContraVoucherModel.TransType = "Update";
                    _ContraVoucherModel.BtnName = "Refresh";
                    return RedirectToAction("ContraDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ContraVoucherModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ContraVoucherModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ContraVoucherModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + Br_ID, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                throw ex;
                //return View("~/Views/Shared/Error.cshtml");
            }

        }
        public DataTable DtVouDetails(JArray jObject)
        {
            DataTable dtAccount = new DataTable();

            dtAccount.Columns.Add("acc_id", typeof(string));
            dtAccount.Columns.Add("acc_name", typeof(string));
            dtAccount.Columns.Add("acc_group_name", typeof(string));
            dtAccount.Columns.Add("od_allow", typeof(string));
            dtAccount.Columns.Add("od_limit", typeof(string));
            //dtAccount.Columns.Add("acc_grp_id", typeof(string));
            dtAccount.Columns.Add("acc_type", typeof(int));
            dtAccount.Columns.Add("curr_id", typeof(int));
            dtAccount.Columns.Add("curr_name", typeof(string));
            dtAccount.Columns.Add("foreign_currency", typeof(string));
            dtAccount.Columns.Add("ClosBL", typeof(string));
            dtAccount.Columns.Add("conv_rate", typeof(string));
            dtAccount.Columns.Add("dr_amt_bs", typeof(string));
            dtAccount.Columns.Add("cr_amt_bs", typeof(string));
            dtAccount.Columns.Add("dr_amt_sp", typeof(string));
            dtAccount.Columns.Add("cr_amt_sp", typeof(string));
            dtAccount.Columns.Add("narr", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtAccount.NewRow();

                dtrowLines["acc_id"] = jObject[i]["acc_id"].ToString();
                dtrowLines["acc_name"] = jObject[i]["acc_name"].ToString();
                dtrowLines["acc_group_name"] = jObject[i]["acc_group_name"].ToString();
                dtrowLines["od_allow"] = jObject[i]["od_allow"].ToString();
                dtrowLines["od_limit"] = jObject[i]["od_limit"].ToString();
                //dtrowLines["acc_grp_id"] = jObject[i]["acc_grp_id"].ToString();
                dtrowLines["acc_type"] = jObject[i]["acc_type"].ToString();
                dtrowLines["curr_id"] = jObject[i]["curr_id"].ToString();
                dtrowLines["curr_name"] = jObject[i]["curr_name"].ToString();
                dtrowLines["foreign_currency"] = jObject[i]["foreign_currency"].ToString();
                dtrowLines["ClosBL"] = jObject[i]["ClosBL"].ToString();
                dtrowLines["conv_rate"] = jObject[i]["conv_rate"].ToString();
                if (jObject[i]["dr_amt_sp"].ToString() == "")
                {
                    dtrowLines["dr_amt_bs"] = 0;
                    dtrowLines["dr_amt_sp"] = 0;
                }
                else
                {
                    dtrowLines["dr_amt_bs"] = string.IsNullOrEmpty(jObject[i]["dr_amt_bs"].ToString()) == true ? "0" : jObject[i]["dr_amt_bs"].ToString();
                    dtrowLines["dr_amt_sp"] = string.IsNullOrEmpty(jObject[i]["dr_amt_sp"].ToString()) == true ? "0" : jObject[i]["dr_amt_sp"].ToString();
                }
                if (jObject[i]["cr_amt_sp"].ToString() == "")
                {
                    dtrowLines["cr_amt_bs"] = 0;
                    dtrowLines["cr_amt_sp"] = 0;
                }
                else
                {
                    dtrowLines["cr_amt_bs"] = string.IsNullOrEmpty(jObject[i]["cr_amt_bs"].ToString()) == true ? "0" : jObject[i]["cr_amt_bs"].ToString();
                    dtrowLines["cr_amt_sp"] = string.IsNullOrEmpty(jObject[i]["cr_amt_sp"].ToString()) == true ? "0" : jObject[i]["cr_amt_sp"].ToString();
                }
                dtrowLines["narr"] = jObject[i]["narr"].ToString();
                dtAccount.Rows.Add(dtrowLines);
            }

            return dtAccount;
        }
        private ActionResult ContraDelete(ContraVoucherModel _ContraVoucherModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();

                string ContraNo = _ContraVoucherModel.Vou_No;
                string ContraNumber = ContraNo.Replace("/", "");

                string Message = _ContraVoucher_IService.ContraVoucherDelete(_ContraVoucherModel, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(ContraNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, ContraNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["ContraNo"] = "";
                //Session["ContraDate"] = "";
                //_ContraVoucherModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("ContraDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ContraApprove(ContraVoucherModel _ContraVoucherModel, string VouNo, string VouDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1)
        {
            try
            {

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
                //{
                MenuDocId = DocumentMenuId;
                // }
                /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                string Msg = string.Empty;
                var commCont = new CommonController(_Common_IServices);
                Msg = commCont.Fin_CheckFinancialYear(Comp_ID, BranchID, VouDate);
                if (Msg == "FY Not Exist" || Msg == "FB Close")
                {
                    if (Msg == "FY Not Exist")
                    {
                        TempData["Message"] = "Financial Year not Exist";
                    }
                    else
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                    }
                    return RedirectToAction("EditVou", new { VouNo = VouNo, Voudt = VouDate, ListFilterData = ListFilterData1, WF_Status = WF_Status1 });
                }
                /*End to chk Financial year exist or not*/
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _ContraVoucher_IService.ContraVoucherApprove(VouNo, VouDate, UserID, A_Status, A_Level, A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string ContraNo = Message.Split(',')[0].Trim();
                string ContraDate = Message.Split(',')[1].Trim();
                //Session["ContraNo"] = ContraNo;
                //Session["ContraDate"] = ContraDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel urlref = new UrlModel();
                _ContraVoucherModel.TransType = "Update";
                _ContraVoucherModel.ContraNo = ContraNo;
                _ContraVoucherModel.ContraDate = ContraDate;
               // _ContraVoucherModel.Message = "Approved";
                _ContraVoucherModel.BtnName = "BtnEdit";
                try
                {
                   // string fileName = "CT_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "ContraVoucher_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(VouNo, VouDate, fileName, DocumentMenuId,"AP");
                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, VouNo, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
            {
                    _ContraVoucherModel.Message = "ErrorInMail";
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exMail);
            }
                _ContraVoucherModel.Message = _ContraVoucherModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _ContraVoucherModel.WF_Status1 = WF_Status1;
                    urlref.wf = WF_Status1;
                }
                TempData["ModelData"] = _ContraVoucherModel;

                urlref.tp = "Update";
                urlref.CNO = ContraNo;
                urlref.CDT = ContraDate;
                urlref.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("ContraDetail", "Contra", urlref);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                ContraModel _attachmentModel = new ContraModel();
                //  string TransType = "";
                //string BPayCode = "";
                Guid gid = new Guid();
                //gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["ContraNo"] != null)
                //{
                //    BPayCode = Session["ContraNo"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _attachmentModel.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                // getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + Br_ID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    // Session["AttachMentDetailItmStp"] = dt;
                    _attachmentModel.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _attachmentModel.AttachMentDetailItmStp = null;
                }
                TempData["IMGDATA"] = _attachmentModel;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        /*-----------------Attachment Section End------------------------*/
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData)
        {
            // Session["Message"] = "";
            ContraVoucherModel _CashPayment_Model = new ContraVoucherModel();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split(',');
            _CashPayment_Model.ContraNo = a[0].Trim();
            _CashPayment_Model.ContraDate = a[1].Trim();
            _CashPayment_Model.TransType = "Update";
            _CashPayment_Model.BtnName = "BtnToDetailPage";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                _CashPayment_Model.WF_Status1 = a[2].Trim();
                urlModel.wf = a[2].Trim();
            }
            urlModel.bt = "D";
            urlModel.CNO = _CashPayment_Model.ContraNo;
            urlModel.CDT = _CashPayment_Model.ContraDate;
            urlModel.Cmd = "Update";
            urlModel.tp = "Update";
            TempData["ModelData"] = _CashPayment_Model;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ContraDetail", urlModel);
        }
        /*--------Print---------*/

        public FileResult GenratePdfFile(ContraVoucherModel _Model)
        {
            return File(GetPdfData(_Model.Vou_No, _Model.Vou_Date), "application/pdf", "ContraVoucher.pdf");
        }
        public byte[] GetPdfData(string vNo, string vDate)
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
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet Deatils = _Common_IServices.Cmn_GetGLVoucherPrintDeatils(CompID, Br_ID, vNo, vDate, "CT");
                ViewBag.PageName = "CT";
                ViewBag.Title = "Contra Voucher";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();

                /* ------------------Added by Suraj Maurya on 17-02-2025 to add logo on pdf -------------------------*/
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                /* ------------------Added by Suraj Maurya on 17-02-2025 to add logo on pdf ------------------------*/

                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_GLVoucher_Print.cshtml"));
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
                    string draftImage = string.Empty;
                    if (ViewBag.DocStatus == "C")
                    {
                         draftImage = Server.MapPath("~/Content/Images/cancelled.png");/*add by NItesh Tewatia on 09-09-2025*/
                    }
                    else
                    {
                         draftImage = Server.MapPath("~/Content/Images/draft.png");/*add by hina sharma on 16-10-2024*/
                    }

                       
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);/*add by hina sharma on 16-10-2024*/
                                draftimg.SetAbsolutePosition(0, 160);
                                draftimg.ScaleAbsolute(580f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F" || ViewBag.DocStatus == "C")/*add by hina sharma on 16-10-2024*/
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
                Br_ID = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, Br_ID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Doc_dt);
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
        public FileResult ContraExporttoExcelDt(string Fromdate, string Todate, string Status)
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
                DataTable dt = new DataTable();
                string VouType = "CT";
                DataSet dt1 = new DataSet();
                dt1 = _ContraVoucher_IService.GetContraVoucherListAll(Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("GL Voucher Number", typeof(string));
                dt.Columns.Add("GL Voucher Date", typeof(string));
                dt.Columns.Add("Amount", typeof(decimal));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Created By", typeof(string));
                dt.Columns.Add("Created On", typeof(string));
                dt.Columns.Add("Approved By", typeof(string));
                dt.Columns.Add("Approved On", typeof(string));
                dt.Columns.Add("Amended By", typeof(string));
                dt.Columns.Add("Amended On", typeof(string));

                if (dt1.Tables[0].Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in dt1.Tables[0].Rows)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["Sr.No"] = rowno + 1;
                        dtrowLines["GL Voucher Number"] = dr["vou_no"].ToString();
                        dtrowLines["GL Voucher Date"] = dr["vou_dt"].ToString();
                        dtrowLines["Amount"] = dr["vou_amt"].ToString();
                        dtrowLines["Status"] = dr["vou_status"].ToString();
                        dtrowLines["Created By"] = dr["create_by"].ToString();
                        dtrowLines["Created On"] = dr["created_on"].ToString();
                        dtrowLines["Approved By"] = dr["app_by"].ToString();
                        dtrowLines["Approved On"] = dr["app_dt"].ToString();
                        dtrowLines["Amended By"] = dr["mod_by"].ToString();
                        dtrowLines["Amended On"] = dr["mod_on"].ToString();
                        dt.Rows.Add(dtrowLines);
                        rowno = rowno + 1;
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Contra", dt);
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
