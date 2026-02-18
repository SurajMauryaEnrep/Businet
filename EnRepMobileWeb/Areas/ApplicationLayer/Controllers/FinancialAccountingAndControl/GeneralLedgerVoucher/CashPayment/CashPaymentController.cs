using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.CashPayment;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.CashPayment;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Configuration;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.CashPayment
{
    public class CashPaymentController : Controller
    {
        string CompID, language, FromDate = String.Empty;
        string Comp_ID, Br_ID, Language, title, UserID = String.Empty;
        string DocumentMenuId = "105104115115";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        CashPayment_IService _CashPayment_IService;
        DataTable dt;
        List<VouList> _CashPaymentList;
        public CashPaymentController(Common_IServices _Common_IServices, CashPayment_IService _CashPayment_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._CashPayment_IService = _CashPayment_IService;
        }
        // GET: ApplicationLayer/CashPayment
        public ActionResult CashPayment(CashPaymentList_Model _CashPaymentList_Model)
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
            //CashPaymentList_Model _CashPaymentList_Model = new CashPaymentList_Model();
            GetStatusList(_CashPaymentList_Model);
            GetAutoCompleteCashDetail(_CashPaymentList_Model);
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _CashPaymentList_Model.Src_Type = a[0].Trim();
                _CashPaymentList_Model.cash_id = a[1].Trim();
                _CashPaymentList_Model.VouFromDate = a[2].Trim();
                _CashPaymentList_Model.VouToDate = a[3].Trim();
                _CashPaymentList_Model.Status = a[4].Trim();
                if (_CashPaymentList_Model.Status == "0")
                {
                    _CashPaymentList_Model.Status = null;
                }
                _CashPaymentList_Model.ListFilterData = TempData["ListFilterData"].ToString();
            }
            _CashPaymentList_Model.VoucherList = GetVoucherListAll(_CashPaymentList_Model);
            if (_CashPaymentList_Model.VouFromDate != null)
            {
                _CashPaymentList_Model.FromDate = _CashPaymentList_Model.VouFromDate;
            }
            //else
            //{
            //    _CashPaymentList_Model.FromDate = FromDate;
            //}
            //_CashPaymentList_Model.FromDate = FromDate;

            ViewBag.VBRoleList = GetRoleList();
            ViewBag.MenuPageName = getDocumentName();
            _CashPaymentList_Model.Title = title;
            //Session["VouSearch"] = "0";
            _CashPaymentList_Model.VouSearch = "0";
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CashPayment/CashPaymentList.cshtml", _CashPaymentList_Model);
        }
        public ActionResult AddCashPaymentDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            CashPayment_Model _CashPayment_Model1 = new CashPayment_Model();
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
            //    return RedirectToAction("CashPayment", _CashPayment_Model1);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("CashPayment", _CashPayment_Model1);
            //}
            ///*End to chk Financial year exist or not*/
            _CashPayment_Model1.Command = "Add";
            _CashPayment_Model1.TransType = "Save";
            _CashPayment_Model1.BtnName = "BtnAddNew";
            _CashPayment_Model1.DocumentStatus = "D";
            TempData["ModelData"] = _CashPayment_Model1;
            UrlModel _urlModel = new UrlModel();
            _urlModel.bt = "BtnAddNew";
            _urlModel.Cmd = "Add";
            _urlModel.tp = "Save";
            ViewBag.MenuPageName = getDocumentName();
            TempData["ListFilterData"] = null;
            return RedirectToAction("CashPaymentDetail", "CashPayment", _urlModel);
        }
        public ActionResult CashPaymentDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/


                var _CashPayment_Model1 = TempData["ModelData"] as CashPayment_Model;
                if (_CashPayment_Model1 != null)
                {
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
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    GetAutoCompleteGLDetail(_CashPayment_Model1);

                    List<curr> _currList = new List<curr>();
                    dt = Getcurr();
                    foreach (DataRow dr in dt.Rows)
                    {
                        curr _curr = new curr();
                        _curr.curr_id = dr["curr_id"].ToString();
                        _curr.curr_name = dr["curr_name"].ToString();
                        _currList.Add(_curr);

                    }
                    _currList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                    _CashPayment_Model1.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _CashPayment_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_CashPayment_Model1.TransType == "Update" || _CashPayment_Model1.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["VoucherNo"].ToString();
                        //string VouDt = Session["VoucherDate"].ToString();
                        string VouType = "CP";
                        string VouNo = _CashPayment_Model1.VoucherNo;
                        string VouDt = _CashPayment_Model1.VoucherDate;
                        DataSet ds = _CashPayment_IService.GetVoucherDetail(VouType, VouNo, VouDt, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _CashPayment_Model1.cash_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _CashPayment_Model1.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _CashPayment_Model1.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _CashPayment_Model1.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        _CashPayment_Model1.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _CashPayment_Model1.src_doc_date = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _CashPayment_Model1.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _CashPayment_Model1.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        //_CashPayment_Model1.conv_rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(RateDigit);
                        _CashPayment_Model1.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        //_CashPayment_Model1.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            _CashPayment_Model1.Vou_amount = ds.Tables[7].Rows[0]["ClosBL"].ToString();
                        }
                        else
                        {
                            _CashPayment_Model1.Vou_amount = Convert.ToDecimal(0).ToString(RateDigit);
                        }
                        _CashPayment_Model1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _CashPayment_Model1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _CashPayment_Model1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _CashPayment_Model1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _CashPayment_Model1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _CashPayment_Model1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _CashPayment_Model1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _CashPayment_Model1.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _CashPayment_Model1.Status = Statuscode;
                        // Session["DocumentStatus"] = Statuscode;
                        _CashPayment_Model1.DocumentStatus = Statuscode;
                        if (_CashPayment_Model1.Status == "C")
                        {
                            _CashPayment_Model1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _CashPayment_Model1.CancelFlag = true;
                            _CashPayment_Model1.BtnName = "Refresh";
                            // Session["BtnName"] = "Refresh";
                        }
                        else
                        {
                            _CashPayment_Model1.CancelFlag = false;
                        }

                        _CashPayment_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _CashPayment_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        _CashPayment_Model1.BillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _CashPayment_Model1.Command != "Edit")
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
                                    _CashPayment_Model1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                                        _CashPayment_Model1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _CashPayment_Model1.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _CashPayment_Model1.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _CashPayment_Model1.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        //_CashPayment_Model1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CashPayment_Model1.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                                        _CashPayment_Model1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CashPayment_Model1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                                    _CashPayment_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CashPayment_Model1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _CashPayment_Model1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                        _CashPayment_Model1.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[6];
                        ViewBag.CostCenterData = ds.Tables[8];
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocumentStatus = _CashPayment_Model1.DocumentStatus;
                        ViewBag.TransType = _CashPayment_Model1.TransType;
                        ViewBag.Command = _CashPayment_Model1.Command;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CashPayment/CashPaymentDetail.cshtml", _CashPayment_Model1);
                    }

                    else
                    {

                        _CashPayment_Model1.DocumentStatus = "D";
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        _CashPayment_Model1.Title = title;
                        ViewBag.DocumentStatus = _CashPayment_Model1.DocumentStatus;
                        ViewBag.TransType = _CashPayment_Model1.TransType;
                        ViewBag.Command = _CashPayment_Model1.Command;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CashPayment/CashPaymentDetail.cshtml", _CashPayment_Model1);
                    }
                }
                else
                {
                    CashPayment_Model _CashPayment_Model = new CashPayment_Model();
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
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _CashPayment_Model.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _CashPayment_Model.BtnName = _urlModel.bt;
                        }
                        _CashPayment_Model.VoucherNo = _urlModel.VN;
                        _CashPayment_Model.VoucherDate = _urlModel.VDT;
                        _CashPayment_Model.Command = _urlModel.Cmd;
                        _CashPayment_Model.TransType = _urlModel.tp;
                        _CashPayment_Model.WF_Status1 = _urlModel.wf;
                    }
                    /* Add by Hina on 22-02-2024 to Refresh Page*/
                    if (_CashPayment_Model.TransType == null)
                    {
                        _CashPayment_Model.BtnName = "Refresh";
                        _CashPayment_Model.Command = "Refresh";
                        _CashPayment_Model.TransType = "Refresh";

                    }
                    ///*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var VouDate = _CashPayment_Model.VoucherDate;
                    //var VouDate = "";
                    
                    //if (_CashPayment_Model.VoucherDate != null)
                    //{
                    //    VouDate = _CashPayment_Model.VoucherDate;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    _CashPayment_Model.VoucherDate = CurrentDate;
                    //    _CashPayment_Model.Vou_Date = CurrentDate;
                    //    VouDate = _CashPayment_Model.VoucherDate;
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
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    GetAutoCompleteGLDetail(_CashPayment_Model);

                    List<curr> _currList = new List<curr>();
                    dt = Getcurr();
                    foreach (DataRow dr in dt.Rows)
                    {
                        curr _curr = new curr();
                        _curr.curr_id = dr["curr_id"].ToString();
                        _curr.curr_name = dr["curr_name"].ToString();
                        _currList.Add(_curr);

                    }
                    _currList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                    _CashPayment_Model.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _CashPayment_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_CashPayment_Model.TransType == "Update" || _CashPayment_Model.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["VoucherNo"].ToString();
                        //string VouDt = Session["VoucherDate"].ToString();
                        string VouType = "CP";
                        string VouNo = _CashPayment_Model.VoucherNo;
                        string VouDt = _CashPayment_Model.VoucherDate;
                        DataSet ds = _CashPayment_IService.GetVoucherDetail(VouType, VouNo, VouDt, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _CashPayment_Model.cash_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _CashPayment_Model.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _CashPayment_Model.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _CashPayment_Model.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        _CashPayment_Model.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _CashPayment_Model.src_doc_date = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _CashPayment_Model.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _CashPayment_Model.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        //_CashPayment_Model.conv_rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(RateDigit);
                        _CashPayment_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        //_CashPayment_Model.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            _CashPayment_Model.Vou_amount = ds.Tables[7].Rows[0]["ClosBL"].ToString();
                        }
                        else
                        {
                            _CashPayment_Model.Vou_amount = Convert.ToDecimal(0).ToString(RateDigit);
                        }
                        _CashPayment_Model.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _CashPayment_Model.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _CashPayment_Model.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _CashPayment_Model.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _CashPayment_Model.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _CashPayment_Model.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _CashPayment_Model.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _CashPayment_Model.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _CashPayment_Model.Status = Statuscode;
                        // Session["DocumentStatus"] = Statuscode;
                        _CashPayment_Model.DocumentStatus = Statuscode;
                        if (_CashPayment_Model.Status == "C")
                        {
                            _CashPayment_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _CashPayment_Model.CancelFlag = true;
                            _CashPayment_Model.BtnName = "Refresh";
                            // Session["BtnName"] = "Refresh";
                        }
                        else
                        {
                            _CashPayment_Model.CancelFlag = false;
                        }

                        _CashPayment_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _CashPayment_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        _CashPayment_Model.BillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _CashPayment_Model.Command != "Edit")
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
                                    _CashPayment_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                                        _CashPayment_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _CashPayment_Model.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _CashPayment_Model.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _CashPayment_Model.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        //_CashPayment_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CashPayment_Model.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                                        _CashPayment_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CashPayment_Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                                    _CashPayment_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CashPayment_Model.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _CashPayment_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                        _CashPayment_Model.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocumentStatus = _CashPayment_Model.DocumentStatus;
                        ViewBag.TransType = _CashPayment_Model.TransType;
                        ViewBag.Command = _CashPayment_Model.Command;
                        ViewBag.CostCenterData = ds.Tables[8];
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CashPayment/CashPaymentDetail.cshtml", _CashPayment_Model);
                    }

                    else
                    {

                        _CashPayment_Model.DocumentStatus = "D";
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        _CashPayment_Model.Title = title;
                        ViewBag.DocumentStatus = _CashPayment_Model.DocumentStatus;
                        ViewBag.TransType = _CashPayment_Model.TransType;
                        ViewBag.Command = _CashPayment_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CashPayment/CashPaymentDetail.cshtml", _CashPayment_Model);
                    }
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult EditVou(string VouNo, string Voudt, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
            CashPayment_Model dblclick = new CashPayment_Model();
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
            //Session["VoucherNo"] = VouNo;
            //Session["VoucherDate"] = Voudt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
           
            UrlModel _url = new UrlModel();
            dblclick.Command = "Update";
            dblclick.VoucherNo = VouNo;
            dblclick.VoucherDate = Voudt;
            dblclick.TransType = "Update";
            //dblclick.BtnName = "BtnToDetailPage";
            if (WF_Status != null && WF_Status != "")
            {
                _url.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            _url.Cmd = "Update";
            _url.tp = dblclick.TransType;
            _url.bt = "D";
            _url.VN = VouNo;
            _url.VDT = Voudt;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("CashPaymentDetail", "CashPayment", _url);
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
        public ActionResult GetCashPaymentList(string docid, string status)
        {

            //Session["WF_status"] = status;
            CashPaymentList_Model Dashbord = new CashPaymentList_Model();
            Dashbord.WF_Status = status;
            return RedirectToAction("CashPayment", Dashbord);
        }
        public ActionResult CashPaymentCommands(CashPayment_Model _CashPayment_Model, string Vou_No, string command)
        {
            try
            {
                /*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                if (_CashPayment_Model.DeleteCommand == "Delete")
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
                        CashPayment_Model adddnew = new CashPayment_Model();
                        adddnew.Command = "Add";
                        adddnew.TransType = "Save";
                        adddnew.BtnName = "BtnAddNew";
                        adddnew.DocumentStatus = "D";
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "Add";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";
                        TempData["ModelData"] = adddnew;
                        TempData["ListFilterData"] = null;
                        ///*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
                        //if (Session["CompId"] != null)
                        //    CompID = Session["CompId"].ToString();
                        //if (Session["BranchId"] != null)
                        //    Br_ID = Session["BranchId"].ToString();
                        //DateTime dtnow = DateTime.Now;
                        //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                        //string MsgNew = string.Empty;
                        //MsgNew = commCont.Fin_CheckFinancialYear(CompID, Br_ID, CurrentDate);
                        //if (MsgNew == "FY Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("CashPaymentDetail", "CashPayment", adddnew);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("CashPaymentDetail", "CashPayment", adddnew);
                        //}
                        ///*End to chk Financial year exist or not*/
                        return RedirectToAction("CashPaymentDetail", "CashPayment", NewModel);

                    case "Edit":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt = _CashPayment_Model.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_CashPayment_Model.Status == "A"|| _CashPayment_Model.Status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("EditVou", new { VouNo = _CashPayment_Model.Vou_No, Voudt = _CashPayment_Model.Vou_Date, ListFilterData = _CashPayment_Model.ListFilterData1, WF_Status = _CashPayment_Model.WFStatus });
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        if (CheckAdvancePayment(_CashPayment_Model, _CashPayment_Model.Vou_No, _CashPayment_Model.Vou_Date) == "Used")
                        {
                            //Session["Message"] = "Used";
                            _CashPayment_Model.Message = "Used";
                            _CashPayment_Model.Command = "Refresh";
                            TempData["ModelData"] = _CashPayment_Model;
                        }
                        else
                        {
                            //Session["TransType"] = "Update";
                            //Session["Command"] = command;
                            //Session["BtnName"] = "BtnEdit";
                            //Session["Message"] = "New";
                            //Session["AppStatus"] = 'D';
                            //Session["VoucherNo"] = _CashPayment_Model.Vou_No;
                            //Session["VoucherDate"] = _CashPayment_Model.Vou_Date.ToString();
                            _CashPayment_Model.TransType = "Update";
                            _CashPayment_Model.Command = command;
                            _CashPayment_Model.BtnName = "BtnEdit";
                            _CashPayment_Model.VoucherNo = _CashPayment_Model.Vou_No;
                            _CashPayment_Model.VoucherDate = _CashPayment_Model.Vou_Date;
                            TempData["ModelData"] = _CashPayment_Model;
                            UrlModel EditModel = new UrlModel();
                            EditModel.tp = "Update";
                            EditModel.Cmd = command;
                            EditModel.bt = "BtnEdit";
                            EditModel.VN = _CashPayment_Model.Vou_No;
                            EditModel.VDT = _CashPayment_Model.Vou_Date;
                            TempData["ListFilterData"] = _CashPayment_Model.ListFilterData1;
                            return RedirectToAction("CashPaymentDetail", EditModel);
                        }
                        UrlModel Edit_Model = new UrlModel();
                        Edit_Model.tp = "Update";
                        Edit_Model.bt = "D";
                        Edit_Model.VN = _CashPayment_Model.Vou_No;
                        Edit_Model.VDT = _CashPayment_Model.Vou_Date;
                        TempData["ListFilterData"] = _CashPayment_Model.ListFilterData1;
                        return RedirectToAction("CashPaymentDetail", Edit_Model);

                    case "Delete":
                        CashPayment_Model DeleteModel = new CashPayment_Model();
                        DeleteModel.Command = command;
                        Vou_No = _CashPayment_Model.Vou_No;
                        VoucherDelete(_CashPayment_Model, command);
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete = new UrlModel();
                        Delete.Cmd = DeleteModel.Command;
                        Delete.tp = "Refresh";
                        Delete.bt = "BtnDelete";
                        TempData["ListFilterData"] = _CashPayment_Model.ListFilterData1;
                        return RedirectToAction("CashPaymentDetail", Delete);


                    case "Save":
                        // Session["Command"] = command;
                        /*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _CashPayment_Model.Vou_Date;
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
                            if (_CashPayment_Model.Vou_No == null)
                            {
                                _CashPayment_Model.Command = "Refresh";
                                _CashPayment_Model.TransType = "Refresh";
                                _CashPayment_Model.BtnName = "Refresh";
                                _CashPayment_Model.DocumentStatus = null;
                                TempData["ModelData"] = _CashPayment_Model;
                                return RedirectToAction("CashPaymentDetail", "CashPayment", _CashPayment_Model);
                            }
                            else
                            {
                                return RedirectToAction("EditVou", new { VouNo = _CashPayment_Model.Vou_No, Voudt = _CashPayment_Model.Vou_Date, ListFilterData = _CashPayment_Model.ListFilterData1, WF_Status = _CashPayment_Model.WFStatus });

                            }

                        }
                        /*End to chk Financial year exist or not*/
                        _CashPayment_Model.Command = command;
                        SaveVoucher(_CashPayment_Model);
                        if (_CashPayment_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_CashPayment_Model.Message == "N")
                        {
                            List<curr> _currList = new List<curr>();
                            dt = Getcurr();
                            foreach (DataRow dr in dt.Rows)
                            {
                                curr _curr = new curr();
                                _curr.curr_id = dr["curr_id"].ToString();
                                _curr.curr_name = dr["curr_name"].ToString();
                                _currList.Add(_curr);

                            }
                            _currList.Insert(0, new curr() { curr_id = "0", curr_name = "---Select---" });
                            _CashPayment_Model.currList = _currList;
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
                            ViewBag.TotalVouAmt = TotalDr.ToString(ValDigit);
                            ViewBag.DiffAmt = TotalDr - TotalCr;

                            //ViewBag.AttechmentDetails =
                            GetAutoCompleteGLDetail(_CashPayment_Model);
                            ViewBag.CostCenterData = ViewData["CostCenter"];
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                            _CashPayment_Model.BtnName = "BtnAddNew";
                            _CashPayment_Model.Command = "Add";
                            _CashPayment_Model.Message = "N";
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CashPayment/CashPaymentDetail.cshtml", _CashPayment_Model);
                        }
                        else
                        {
                            //Session["VoucherNo"] = Session["VoucherNo"].ToString();
                            //Session["VoucherDate"] = Session["VoucherDate"].ToString();
                            TempData["ModelData"] = _CashPayment_Model;
                            UrlModel _urlModel = new UrlModel();
                            _urlModel.bt = _CashPayment_Model.BtnName;
                            _urlModel.Cmd = _CashPayment_Model.Command;
                            _urlModel.VN = _CashPayment_Model.VoucherNo;
                            _urlModel.VDT = _CashPayment_Model.VoucherDate;
                            _urlModel.tp = _CashPayment_Model.TransType;
                            TempData["ListFilterData"] = _CashPayment_Model.ListFilterData1;
                            return RedirectToAction("CashPaymentDetail", _urlModel);
                        }

                    case "Approve":
                        /*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _CashPayment_Model.Vou_Date;

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
                            return RedirectToAction("EditVou", new { VouNo = _CashPayment_Model.Vou_No, Voudt = _CashPayment_Model.Vou_Date, ListFilterData = _CashPayment_Model.ListFilterData1, WF_Status = _CashPayment_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        // Session["Command"] = command;
                        _CashPayment_Model.Command = command;
                        CashPaymentApprove(_CashPayment_Model, _CashPayment_Model.Vou_No, _CashPayment_Model.Vou_Date, "", "", "", "", "", "");
                        TempData["ModelData"] = _CashPayment_Model;
                        UrlModel urlref = new UrlModel();
                        urlref.tp = "Update";
                        urlref.VN = _CashPayment_Model.VoucherNo;
                        urlref.VDT = _CashPayment_Model.VoucherDate;
                        urlref.bt = "BtnEdit";
                        if (_CashPayment_Model.WF_Status1 != null)
                        {
                            urlref.wf = _CashPayment_Model.WF_Status1;
                        }
                        TempData["ListFilterData"] = _CashPayment_Model.ListFilterData1;
                        return RedirectToAction("CashPaymentDetail", urlref);

                    case "Forward":
                        return new EmptyResult();

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        //Session["DocumentStatus"] = 'D';
                        CashPayment_Model RefreshModel = new CashPayment_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _CashPayment_Model.ListFilterData1;
                        return RedirectToAction("CashPaymentDetail", refesh);

                    case "Print":
                        return GenratePdfFile(_CashPayment_Model);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        CashPaymentList_Model _CashPaymentDash_Model = new CashPaymentList_Model();
                        _CashPaymentDash_Model.WF_Status = _CashPayment_Model.WF_Status1;
                        TempData["ListFilterData"] = _CashPayment_Model.ListFilterData1;
                        return RedirectToAction("CashPayment", "CashPayment", _CashPaymentDash_Model);
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
        public ActionResult SaveVoucher(CashPayment_Model _CashPayment_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (_CashPayment_Model.CancelFlag == false)
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
                    DataTable VoucherHeader = new DataTable();
                    DataTable VoucherItemDetails = new DataTable();
                    DataTable VoucherBillAdjDetail = new DataTable();
                    DataTable CRCostCenterDetails = new DataTable();

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
                    if (_CashPayment_Model.Vou_No != null)
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
                    dtrowHeader["vou_type"] = "CP";
                    dtrowHeader["vou_no"] = _CashPayment_Model.Vou_No;
                    dtrowHeader["vou_dt"] = _CashPayment_Model.Vou_Date;
                    dtrowHeader["src_doc"] = _CashPayment_Model.Src_Type;
                    dtrowHeader["src_doc_no"] = null;
                    dtrowHeader["src_doc_dt"] = null;
                    dtrowHeader["vou_amt"] = "0";
                    dtrowHeader["remarks"] = _CashPayment_Model.Remarks;
                    dtrowHeader["vou_status"] = "D";
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrowHeader["mac_id"] = mac_id;
                    dtrowHeader["ins_type"] = "";
                    dtrowHeader["ins_no"] = "";
                    dtrowHeader["ins_dt"] = "";
                    dtrowHeader["ins_name"] = "";

                    dtheader.Rows.Add(dtrowHeader);
                    VoucherHeader = dtheader;

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

                    JArray jObject = JArray.Parse(_CashPayment_Model.GlAccountDetails);

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
                            dtrowLines["dr_amt_bs"] = jObject[i]["dr_amt_bs"].ToString();
                            dtrowLines["dr_amt_sp"] = jObject[i]["dr_amt_sp"].ToString();
                        }
                        if (jObject[i]["cr_amt_sp"].ToString() == "")
                        {
                            dtrowLines["cr_amt_bs"] = 0;
                            dtrowLines["cr_amt_sp"] = 0;
                        }
                        else
                        {
                            dtrowLines["cr_amt_bs"] = jObject[i]["cr_amt_bs"].ToString();
                            dtrowLines["cr_amt_sp"] = jObject[i]["cr_amt_sp"].ToString();
                        }
                        dtrowLines["narr"] = jObject[i]["narr"].ToString();
                        dtrowLines["seq_no"] = jObject[i]["seq_no"].ToString();
                        dtAccount.Rows.Add(dtrowLines);
                    }
                    VoucherItemDetails = dtAccount;
                    ViewData["VouDetails"] = DtVouDetails(jObject);

                    DataTable dtBillAdjDetail = new DataTable();

                    dtBillAdjDetail.Columns.Add("acc_id", typeof(string));
                    dtBillAdjDetail.Columns.Add("inv_no", typeof(string));
                    dtBillAdjDetail.Columns.Add("inv_dt", typeof(string));
                    dtBillAdjDetail.Columns.Add("bill_no", typeof(string));
                    dtBillAdjDetail.Columns.Add("bill_dt", typeof(string));
                    dtBillAdjDetail.Columns.Add("curr_id", typeof(int));
                    //dtBillAdjDetail.Columns.Add("conv_rate", typeof(string));
                    dtBillAdjDetail.Columns.Add("inv_amt_sp", typeof(string));
                    dtBillAdjDetail.Columns.Add("inv_amt_bs", typeof(string));
                    dtBillAdjDetail.Columns.Add("paid_amt", typeof(string));
                    dtBillAdjDetail.Columns.Add("pend_amt", typeof(string));

                    JArray BObject = JArray.Parse(_CashPayment_Model.BillAdjdetails);

                    for (int i = 0; i < BObject.Count; i++)
                    {
                        DataRow dtrowLines = dtBillAdjDetail.NewRow();

                        dtrowLines["acc_id"] = BObject[i]["AccID"].ToString();
                        dtrowLines["inv_no"] = BObject[i]["InvoiceNo"].ToString();
                        dtrowLines["inv_dt"] = BObject[i]["InvoiceDate"].ToString();
                        dtrowLines["Bill_no"] = BObject[i]["BillNo"].ToString();
                        dtrowLines["Bill_dt"] = BObject[i]["BillDate"].ToString();
                        dtrowLines["curr_id"] = 1;
                        //dtrowLines["conv_rate"] = BObject[i]["conv_rate"].ToString();
                        if (BObject[i]["InvAmtInSp"].ToString() == "")
                        {
                            dtrowLines["inv_amt_sp"] = 0;
                        }
                        else
                        {
                            dtrowLines["inv_amt_sp"] = BObject[i]["InvAmtInSp"].ToString();

                        }
                        if (BObject[i]["InvAmtInBase"].ToString() == "")
                        {
                            dtrowLines["inv_amt_bs"] = 0;
                        }
                        else
                        {
                            dtrowLines["inv_amt_bs"] = BObject[i]["InvAmtInBase"].ToString();
                        }
                        if (BObject[i]["PayAmount"].ToString() == "")
                        {
                            dtrowLines["paid_amt"] = 0;
                        }
                        else
                        {
                            dtrowLines["paid_amt"] = BObject[i]["PayAmount"].ToString();
                        }
                        if (BObject[i]["RemBal"].ToString() == "")
                        {
                            dtrowLines["pend_amt"] = 0;
                        }
                        else
                        {
                            dtrowLines["pend_amt"] = BObject[i]["RemBal"].ToString();
                        }
                        dtBillAdjDetail.Rows.Add(dtrowLines);
                    }
                    VoucherBillAdjDetail = dtBillAdjDetail;
                    ViewData["BillAdj"] = dtBillAdj(BObject);

                    /**----------------Cost Center Section--------------------*/
                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));


                    JArray JAObj = JArray.Parse(_CashPayment_Model.CC_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = CC_Details.NewRow();

                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                        CC_Details.Rows.Add(dtrowLines);
                    }
                    CRCostCenterDetails = CC_Details;
                    ViewData["CostCenter"] = dtCostCenter(JAObj);

                    /**----------------Cost Center Section END--------------------*/


                    /*-----------------Attachment Section Start------------------------*/
                    DataTable CPAttachments = new DataTable();
                    DataTable CPdtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as CashRecipt;
                    TempData["IMGDATA"] = null;
                    if (_CashPayment_Model.attatchmentdetail != null)
                    {

                        if (attachData != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                //CRdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                CPdtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                CPdtAttachment.Columns.Add("id", typeof(string));
                                CPdtAttachment.Columns.Add("file_name", typeof(string));
                                CPdtAttachment.Columns.Add("file_path", typeof(string));
                                CPdtAttachment.Columns.Add("file_def", typeof(char));
                                CPdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_CashPayment_Model.AttachMentDetailItmStp != null)
                            {
                                //CRdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                CPdtAttachment = _CashPayment_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                CPdtAttachment.Columns.Add("id", typeof(string));
                                CPdtAttachment.Columns.Add("file_name", typeof(string));
                                CPdtAttachment.Columns.Add("file_path", typeof(string));
                                CPdtAttachment.Columns.Add("file_def", typeof(char));
                                CPdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_CashPayment_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in CPdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = CPdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_CashPayment_Model.Vou_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = _CashPayment_Model.Vou_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                CPdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_CashPayment_Model.TransType == "Update")
                        {
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string CP_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_CashPayment_Model.Vou_No).ToString()))
                                {
                                    CP_CODE = (_CashPayment_Model.Vou_No).ToString();

                                }
                                else
                                {
                                    CP_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + Br_ID + CP_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in CPdtAttachment.Rows)
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
                        CPAttachments = CPdtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/

                    SaveMessage = _CashPayment_IService.InsertVoucherDetail(VoucherHeader, VoucherItemDetails, CPAttachments, VoucherBillAdjDetail, CRCostCenterDetails);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 25-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //_BankPayment_Model.Message = "Financial Year not Exist";
                        _CashPayment_Model.BtnName = "Refresh";
                        _CashPayment_Model.Command = "Refresh";
                        _CashPayment_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;
                        return RedirectToAction("CashPaymentDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //_BankPayment_Model.Message = "Financial Book Closing";
                        _CashPayment_Model.BtnName = "Refresh";
                        _CashPayment_Model.Command = "Refresh";
                        _CashPayment_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;

                        return RedirectToAction("CashPaymentDetail");
                    }
                    else
                    {
                        if (SaveMessage == "N")
                        {
                            _CashPayment_Model.Message = "N";
                            return RedirectToAction("CashPaymentDetail");
                        }
                        else
                        {
                            string VoucherNo = SaveMessage.Split(',')[1].Trim();
                            string CP_Number = VoucherNo.Replace("/", "");
                            string Message = SaveMessage.Split(',')[0].Trim();
                            string VoucherDate = SaveMessage.Split(',')[2].Trim();
                            if (Message == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message.Replace("_", " ") + " " + VoucherNo + " in " + PageName;//BankReceiptNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _CashPayment_Model.Message = Message.Split(',')[0].Replace("_", "");
                                return RedirectToAction("CashPaymentDetail");
                            }
                            /*-----------------Attachment Section Start------------------------*/
                            if (Message == "Save")

                            {
                                //if (Session["Guid"] != null)
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
                                comCont.ResetImageLocation(CompID, Br_ID, guid, PageName, CP_Number, _CashPayment_Model.TransType, CPAttachments);

                                //string Guid = Session["Guid"].ToString();
                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{

                                //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Br_ID + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in CPAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = CompID + Br_ID + CP_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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
                                //Session["VoucherNo"] = VoucherNo;
                                //Session["VoucherDate"] = VoucherDate;
                                //Session["TransType"] = "Update";
                                //Session["AppStatus"] = 'D';
                                //Session["BtnName"] = "BtnToDetailPage";
                                _CashPayment_Model.Message = "Save";
                            _CashPayment_Model.Command = "Update";
                            _CashPayment_Model.VoucherNo = VoucherNo;
                            _CashPayment_Model.VoucherDate = VoucherDate;
                            _CashPayment_Model.TransType = "Update";
                            _CashPayment_Model.BtnName = "BtnToDetailPage";
                            return RedirectToAction("CashPaymentDetail");
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
                    if (CheckAdvancePayment(_CashPayment_Model, _CashPayment_Model.Vou_No, _CashPayment_Model.Vou_Date) == "Used")
                    {
                        //Session["Message"] = "Used";
                        _CashPayment_Model.Message = "Used";
                        TempData["ModelData"] = _CashPayment_Model;
                    }
                    else
                    {
                        _CashPayment_Model.Create_by = UserID;
                        string br_id = Session["BranchId"].ToString();
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        DataSet SaveMessage1 = _CashPayment_IService.CashPaymentCancel(_CashPayment_Model, CompID, br_id, mac_id);
                        try
                        {
                            // string fileName = "CP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "CashPayment_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_CashPayment_Model.Vou_No, _CashPayment_Model.Vou_Date, fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _CashPayment_Model.Vou_No, "C", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _CashPayment_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _CashPayment_Model.Message = _CashPayment_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        //_CashPayment_Model.Message = "Cancelled";
                        _CashPayment_Model.Command = "Update";
                        _CashPayment_Model.VoucherNo = _CashPayment_Model.Vou_No;
                        _CashPayment_Model.VoucherDate = _CashPayment_Model.Vou_Date;
                        _CashPayment_Model.TransType = "Update";
                        _CashPayment_Model.BtnName = "Refresh";
                    }
                    
                    return RedirectToAction("CashPaymentDetail");
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
                    if (_CashPayment_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        //{
                        //    Guid = Session["Guid"].ToString();
                        //}
                        if (_CashPayment_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _CashPayment_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + Br_ID, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                throw ex;
               // return View("~/Views/Shared/Error.cshtml");
            }

        }

        public DataTable DtVouDetails(JArray jObject)
        {
            DataTable dtAccount = new DataTable();

            dtAccount.Columns.Add("acc_id", typeof(string));
            dtAccount.Columns.Add("acc_name", typeof(string));
            dtAccount.Columns.Add("acc_group_name", typeof(string));
            dtAccount.Columns.Add("acc_type", typeof(int));
            dtAccount.Columns.Add("curr_id", typeof(int));
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
                    dtrowLines["dr_amt_bs"] = jObject[i]["dr_amt_bs"].ToString();
                    dtrowLines["dr_amt_sp"] = jObject[i]["dr_amt_sp"].ToString();
                }
                if (jObject[i]["cr_amt_sp"].ToString() == "")
                {
                    dtrowLines["cr_amt_bs"] = 0;
                    dtrowLines["cr_amt_sp"] = 0;
                }
                else
                {
                    dtrowLines["cr_amt_bs"] = jObject[i]["cr_amt_bs"].ToString();
                    dtrowLines["cr_amt_sp"] = jObject[i]["cr_amt_sp"].ToString();
                }
                dtrowLines["narr"] = jObject[i]["narr"].ToString();
                dtAccount.Rows.Add(dtrowLines);
            }
            return dtAccount;
        }
        public DataTable dtBillAdj(JArray BObject)
        {
            DataTable dtBillAdjDetail = new DataTable();

            dtBillAdjDetail.Columns.Add("acc_id", typeof(string));
            dtBillAdjDetail.Columns.Add("inv_no", typeof(string));
            dtBillAdjDetail.Columns.Add("inv_dt", typeof(string));
            dtBillAdjDetail.Columns.Add("curr_id", typeof(int));
            //dtBillAdjDetail.Columns.Add("conv_rate", typeof(string));
            dtBillAdjDetail.Columns.Add("inv_amt_sp", typeof(string));
            dtBillAdjDetail.Columns.Add("inv_amt_bs", typeof(string));
            dtBillAdjDetail.Columns.Add("paid_amt", typeof(string));
            dtBillAdjDetail.Columns.Add("pend_amt", typeof(string));
            for (int i = 0; i < BObject.Count; i++)
            {
                DataRow dtrowLines = dtBillAdjDetail.NewRow();

                dtrowLines["acc_id"] = BObject[i]["AccID"].ToString();
                dtrowLines["inv_no"] = BObject[i]["InvoiceNo"].ToString();
                dtrowLines["inv_dt"] = BObject[i]["InvoiceDate"].ToString();
                dtrowLines["curr_id"] = 1;
                //dtrowLines["conv_rate"] = BObject[i]["conv_rate"].ToString();
                if (BObject[i]["InvAmtInSp"].ToString() == "")
                {
                    dtrowLines["inv_amt_sp"] = 0;
                }
                else
                {
                    dtrowLines["inv_amt_sp"] = BObject[i]["InvAmtInSp"].ToString();

                }
                if (BObject[i]["InvAmtInBase"].ToString() == "")
                {
                    dtrowLines["inv_amt_bs"] = 0;
                }
                else
                {
                    dtrowLines["inv_amt_bs"] = BObject[i]["InvAmtInBase"].ToString();
                }
                if (BObject[i]["PayAmount"].ToString() == "")
                {
                    dtrowLines["paid_amt"] = 0;
                }
                else
                {
                    dtrowLines["paid_amt"] = BObject[i]["PayAmount"].ToString();
                }
                if (BObject[i]["RemBal"].ToString() == "")
                {
                    dtrowLines["pend_amt"] = 0;
                }
                else
                {
                    dtrowLines["pend_amt"] = BObject[i]["RemBal"].ToString();
                }
                dtBillAdjDetail.Rows.Add(dtrowLines);
            }
            return dtBillAdjDetail;
        }
        public DataTable dtCostCenter(JArray JAObj)
        {
            DataTable CC_Details = new DataTable();

            CC_Details.Columns.Add("acc_id", typeof(string));
            CC_Details.Columns.Add("cc_id", typeof(int));
            CC_Details.Columns.Add("cc_val_id", typeof(int));
            CC_Details.Columns.Add("cc_amt", typeof(string));

            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                CC_Details.Rows.Add(dtrowLines);
            }
            return CC_Details;

        }
        public ActionResult GetAutoCompleteGLDetail(CashPayment_Model _CashPayment_Model)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> CashAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_CashPayment_Model.CashName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _CashPayment_Model.CashName;
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                // Br_ID = Session["BranchId"].ToString();
                CashAccList = _CashPayment_IService.AutoGetCashAccList(Comp_ID, Acc_Name, Br_ID);

                List<CashAccName> _CashAccNameList = new List<CashAccName>();
                foreach (var dr in CashAccList)
                {
                    CashAccName _CashAccName = new CashAccName();
                    _CashAccName.cash_acc_id = dr.Key;
                    _CashAccName.cash_acc_name = dr.Value;
                    _CashAccNameList.Add(_CashAccName);
                }
                _CashPayment_Model.CashAccNameList = _CashAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(CashAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        [NonAction]
        private DataTable Getcurr()
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                DataTable dt = _CashPayment_IService.GetCurrList(CompID);
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
        public JsonResult GetAccCurr(string acc_id, string Date)
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
                //Br_ID = Session["BranchId"].ToString();
                DataSet result = _CashPayment_IService.GetAccCurrOnChange(acc_id, CompID, Br_ID, Date);
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
        public ActionResult GetCashAccIDDetail(string CashAccID)
        {
            try
            {
                JsonResult DataRows = null;
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
                // string BrchID = Session["BranchId"].ToString();
                DataSet ds = _CashPayment_IService.GetCashAccIDDetail(CompID, BrchID, CashAccID);

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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        //------------For List page--------------//
        public ActionResult GetAutoCompleteCashDetail(CashPaymentList_Model _CashPaymentList_Model)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> CashAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_CashPaymentList_Model.cash_name))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _CashPaymentList_Model.cash_name;
                }
                Br_ID = Session["BranchId"].ToString();
                CashAccList = _CashPayment_IService.AutoGetCashAccList(Comp_ID, Acc_Name, Br_ID);

                List<CashAccList> _CashAccNameList = new List<CashAccList>();
                foreach (var dr in CashAccList)
                {
                    CashAccList _CashAccName = new CashAccList();
                    _CashAccName.cash_acc_id = dr.Key;
                    _CashAccName.cash_acc_name = dr.Value;
                    _CashAccNameList.Add(_CashAccName);
                }
                _CashPaymentList_Model.CashAccNameList = _CashAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(CashAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        private List<VouList> GetVoucherListAll(CashPaymentList_Model _CashPaymentList_Model)
        {
            try
            {
                _CashPaymentList = new List<VouList>();
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
                if (_CashPaymentList_Model.WF_Status != null)
                {
                    wfstatus = _CashPaymentList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string VouType = "CP";
                DataSet Dtdata = new DataSet();
                Dtdata = _CashPayment_IService.GetVoucherListAll(_CashPaymentList_Model.Src_Type, _CashPaymentList_Model.cash_id, _CashPaymentList_Model.VouFromDate, _CashPaymentList_Model.VouToDate, _CashPaymentList_Model.Status, CompID, Br_ID, VouType, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    _CashPaymentList_Model.FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (Dtdata.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in Dtdata.Tables[0].Rows)
                    {
                        VouList _VouList = new VouList();
                        _VouList.cash_name = dr["cash_name"].ToString();
                        _VouList.VouNumber = dr["vou_no"].ToString();
                        _VouList.VouDate = dr["vou_dt"].ToString();
                        _VouList.hdVouDate = dr["vou_date"].ToString();
                        _VouList.SrcType = dr["SrcType"].ToString();
                        _VouList.ReqNo = dr["src_doc_no"].ToString();
                        _VouList.ReqDt = dr["src_doc_dt"].ToString();
                        _VouList.curr_logo = dr["curr_logo"].ToString();
                        _VouList.Amount = dr["vou_amt"].ToString();
                        _VouList.VouStatus = dr["vou_status"].ToString();
                        _VouList.CreatedON = dr["created_on"].ToString();
                        _VouList.ApprovedOn = dr["app_dt"].ToString();
                        _VouList.ModifiedOn = dr["mod_on"].ToString();
                        _VouList.create_by = dr["create_by"].ToString();
                        _VouList.mod_by = dr["mod_by"].ToString();
                        _VouList.app_by = dr["app_by"].ToString();
                        _CashPaymentList.Add(_VouList);
                    }
                }
                return _CashPaymentList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public void GetStatusList(CashPaymentList_Model _CashPaymentList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _CashPaymentList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        public FileResult CashPaymentExporttoExcelDt(string SrcType, string CashName, string Fromdate, string Todate, string Status)
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
                string VouType = "CP";
                DataTable dt = new DataTable();
                DataSet dt1 = _CashPayment_IService.GetVoucherListAll(SrcType, CashName, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("Cash Account", typeof(string));
                dt.Columns.Add("GL Voucher Number", typeof(string));
                dt.Columns.Add("GL Voucher Date", typeof(string));
                dt.Columns.Add("Source Type", typeof(string));
                dt.Columns.Add("Requisition Number", typeof(string));
                dt.Columns.Add("Requisition Date", typeof(string));
                dt.Columns.Add("Currency", typeof(string));
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
                        dtrowLines["Cash Account"] = dr["cash_name"].ToString();
                        dtrowLines["GL Voucher Number"] = dr["vou_no"].ToString();
                        dtrowLines["GL Voucher Date"] = dr["vou_dt"].ToString();
                        dtrowLines["Source Type"] = dr["SrcType"].ToString();
                        dtrowLines["Requisition Number"] = dr["src_doc_no"].ToString();
                        dtrowLines["Requisition Date"] = dr["src_doc_dt"].ToString();
                        dtrowLines["Currency"] = dr["curr_logo"].ToString();
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
                return commonController.ExportDatatableToExcel("CashPayment", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult SearchVoucherDetail(string Src_Type, string cash_id, string Fromdate, string Todate, string Status, string CompID, string Br_ID)
        {
            _CashPaymentList = new List<VouList>();
            CashPaymentList_Model _CashPaymentList_Model = new CashPaymentList_Model();
            // Session["WF_status"] = "";
            _CashPaymentList_Model.WF_Status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            string VouType = "CP";
            DataSet dt = new DataSet();
            dt = _CashPayment_IService.GetVoucherListAll(Src_Type, cash_id, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
            //Session["VouSearch"] = "Vou_Search";
            _CashPaymentList_Model.VouSearch = "Vou_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    VouList _VouList = new VouList();
                    _VouList.cash_name = dr["cash_name"].ToString();
                    _VouList.VouNumber = dr["vou_no"].ToString();
                    _VouList.VouDate = dr["vou_dt"].ToString();
                    _VouList.hdVouDate = dr["vou_date"].ToString();
                    _VouList.SrcType = dr["SrcType"].ToString();
                    _VouList.ReqNo = dr["src_doc_no"].ToString();
                    _VouList.ReqDt = dr["src_doc_dt"].ToString();
                    _VouList.Amount = dr["vou_amt"].ToString();
                    _VouList.curr_logo = dr["curr_logo"].ToString();
                    _VouList.VouStatus = dr["vou_status"].ToString();
                    _VouList.CreatedON = dr["created_on"].ToString();
                    _VouList.ApprovedOn = dr["app_dt"].ToString();
                    _VouList.ModifiedOn = dr["mod_on"].ToString();
                    _VouList.create_by = dr["create_by"].ToString();
                    _VouList.mod_by = dr["mod_by"].ToString();
                    _VouList.app_by = dr["app_by"].ToString();
                    _CashPaymentList.Add(_VouList);
                }
            }
            _CashPaymentList_Model.VoucherList = _CashPaymentList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialCashPaymentList.cshtml", _CashPaymentList_Model);
        }
        //-----List Page End------------------
        private ActionResult VoucherDelete(CashPayment_Model _CashPayment_Model, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string CPNo = _CashPayment_Model.Vou_No;
                string CashPayNumber = CPNo.Replace("/", "");

                string Message = _CashPayment_IService.CPDelete(_CashPayment_Model, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(CashPayNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, CashPayNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["VoucherNo"] = "";
                //Session["VoucherDate"] = "";
                //_CashPayment_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("CashPaymentDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //  return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData,string Mailerror)
        {
            //Session["Message"] = "";
            CashPayment_Model _CashPayment_Model = new CashPayment_Model();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split(',');
            _CashPayment_Model.VoucherNo = a[0].Trim();
            _CashPayment_Model.VoucherDate = a[1].Trim();
            _CashPayment_Model.TransType = "Update";
            _CashPayment_Model.BtnName = "BtnToDetailPage";
            _CashPayment_Model.Message = Mailerror;
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                _CashPayment_Model.WF_Status1 = a[2].Trim();
                urlModel.wf = a[2].Trim();
            }
            urlModel.bt = "D";
            urlModel.VN = _CashPayment_Model.VoucherNo;
            urlModel.VDT = _CashPayment_Model.VoucherDate;
            urlModel.Cmd = "Update";
            urlModel.tp = "Update";
            TempData["ModelData"] = _CashPayment_Model;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("CashPaymentDetail", urlModel);
        }

        public ActionResult CashPaymentApprove(CashPayment_Model _CashPayment_Model, string VouNo, string VouDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1, string docid)
        {
            try
            {   string Comp_ID = string.Empty;
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
                //}
                /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                string Message = _CashPayment_IService.CashPaymentApprove(VouNo, VouDate, UserID, A_Status, A_Level, A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string VoucherNo = Message.Split(',')[0].Trim();
                string VoucherDate = Message.Split(',')[1].Trim();
                //Session["VoucherNo"] = VoucherNo;
                //Session["VoucherDate"] = VoucherDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel urlref = new UrlModel();
                _CashPayment_Model.TransType = "Update";
                _CashPayment_Model.VoucherNo = VoucherNo;
                _CashPayment_Model.VoucherDate = VoucherDate;
                //_CashPayment_Model.Message = "Approved";
                _CashPayment_Model.BtnName = "BtnEdit";
                try
                {
                    // string fileName = "CP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "CashPayment_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(VouNo, VouDate, fileName, DocumentMenuId,"AP");
                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, VouNo, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _CashPayment_Model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                _CashPayment_Model.Message = _CashPayment_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _CashPayment_Model.WF_Status1 = WF_Status1;
                    urlref.wf = WF_Status1;
                }
                TempData["ModelData"] = _CashPayment_Model;

                urlref.tp = "Update";
                urlref.VN = VoucherNo;
                urlref.VDT = VoucherDate;
                urlref.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("CashPaymentDetail", "CashPayment", urlref);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
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

        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string Title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                CashRecipt _Cashpayment = new CashRecipt();
                // string TransType = "";
                //string CPayCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["VoucherNo"] != null)
                //{
                //    CPayCode = Session["VoucherNo"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _Cashpayment.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //getDocumentName(); /* To set Title*/
                string PageName = Title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + Br_ID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _Cashpayment.AttachMentDetailItmStp = dt;
                    //Session["AttachMentDetailItmStp"] = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _Cashpayment.AttachMentDetailItmStp = null;
                }
                TempData["IMGDATA"] = _Cashpayment;
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
        public string CheckAdvancePayment(CashPayment_Model _CashPayment_Model, string DocNo, string DocDate)
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
                DataSet Deatils = _CashPayment_IService.CheckAdvancePayment(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)/*Added by Suraj Maurya on 24-09-2024 Advance to customer*/
                {
                    str = "Used";
                }
                if (Deatils.Tables[1].Rows.Count > 0)/*Added by Suraj Maurya on 24-09-2024 Advance to Supplier*/
                {
                    str = "Used";
                }
                if (Deatils.Tables[3].Rows.Count > 0)/*Added by Suraj Maurya on 24-09-2024 to check bill paid to customers*/
                {
                    str = "Used";
                }
                if (Deatils.Tables[4].Rows.Count > 0) /*Added by Suraj Maurya on 24-09-2024 to check bill paid to suppliers*/
                {
                    str = "Used";
                }

                if (str != "" && str != null)
                {
                    _CashPayment_Model.VoucherNo = DocNo;
                    _CashPayment_Model.VoucherDate = DocDate;
                    _CashPayment_Model.TransType = "Update";
                    _CashPayment_Model.BtnName = "BtnToDetailPage";
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
        /*--------Print---------*/

        public FileResult GenratePdfFile(CashPayment_Model _Model)
        {
            return File(GetPdfData(_Model.Vou_No, _Model.Vou_Date), "application/pdf", "CashPayment.pdf");
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
                DataSet Deatils = _Common_IServices.Cmn_GetGLVoucherPrintDeatils(CompID, Br_ID, vNo, vDate, "CP");
                ViewBag.PageName = "CP";
                //ViewBag.Title = "Cash Payment";/*Commented and change by Hina on 17-10-2024*/
                ViewBag.Title = "Payment Voucher";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                ViewBag.DocumentMenuId = DocumentMenuId;

                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp)
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                //ViewBag.FLogoPath = serverUrl+ Deatils.Tables[0].Rows[0]["logo"].ToString().Trim();
                
                /* ------------------Added by Suraj Maurya on 17-02-2025 to add logo on pdf -------------------------*/
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                /* ------------------Added by Suraj Maurya on 17-02-2025  to add logo on pdf ------------------------*/

                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_GLVoucher_Print.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(GLVoucherHtml);
                   // pdfDoc = new Document(PageSize.A4.Rotate(), 0f, 0f, 10f, 20f);
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
                        draftImage = Server.MapPath("~/Content/Images/cancelled.png");/*Add by NItesh  on 08-09-2025 */
                    }
                    else
                    {
                        draftImage = Server.MapPath("~/Content/Images/draft.png");/*Add by Hina sharma on 16-10-2024 */
                    }
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
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F" || ViewBag.DocStatus == "C")
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
    }

}