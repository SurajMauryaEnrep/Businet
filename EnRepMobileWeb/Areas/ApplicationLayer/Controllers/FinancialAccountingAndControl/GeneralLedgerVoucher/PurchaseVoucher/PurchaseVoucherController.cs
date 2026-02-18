using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.PurchaseVoucher;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.PurchaseVoucher;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using System.IO;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.PurchaseVoucher
{
    public class PurchaseVoucherController : Controller
    {
        string CompID, language, FromDate = String.Empty;
        string Comp_ID, Br_ID, Language, title, UserID = String.Empty;
        string DocumentMenuId = "105104115140";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        PurchaseVoucher_IService _PurchaseVoucher_IService;
        DataTable dt;
        List<VouList> _PurchaseVoucherList;
        public PurchaseVoucherController(Common_IServices _Common_IServices, PurchaseVoucher_IService _PurchaseVoucher_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._PurchaseVoucher_IService = _PurchaseVoucher_IService;
        }
        // GET: ApplicationLayer/PurchaseVoucher
        public ActionResult PurchaseVoucher(PurchaseVoucherList_Model _Pur_voc_Model)
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
            // PurchaseVoucherList_Model _Pur_voc_Model = new PurchaseVoucherList_Model();
            GetStatusList(_Pur_voc_Model);
            GetAutoCompleteSuppDetail(_Pur_voc_Model);
            List<Suppcurr> _currList = new List<Suppcurr>();
            dt = Getcurr();
            foreach (DataRow dr in dt.Rows)
            {
                Suppcurr _curr = new Suppcurr();
                _curr.curr_id = dr["curr_id"].ToString();
                _curr.curr_name = dr["curr_name"].ToString();
                _currList.Add(_curr);

            }
            _currList.Insert(0, new Suppcurr() { curr_id = "0", curr_name = "---Select---" });
            _Pur_voc_Model.currList = _currList;
            DateTime dtnow = DateTime.Now;
            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //string endDate = dtnow.ToString("yyyy-MM-dd");
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _Pur_voc_Model.curr = Convert.ToInt32(a[1].Trim());
                _Pur_voc_Model.supp_id = a[0].Trim();
                _Pur_voc_Model.VouFromDate = a[2].Trim();
                _Pur_voc_Model.VouToDate = a[3].Trim();
                _Pur_voc_Model.Status = a[4].Trim();
                if (_Pur_voc_Model.Status == "0")
                {
                    _Pur_voc_Model.Status = null;
                }
                _Pur_voc_Model.ListFilterData = TempData["ListFilterData"].ToString();
            }
            _Pur_voc_Model.VoucherList = GetPurchaseVoucherListAll(_Pur_voc_Model);
            if (_Pur_voc_Model.VouFromDate != null)
            {
                _Pur_voc_Model.FromDate = _Pur_voc_Model.VouFromDate;
            }
          
            ViewBag.VBRoleList = GetRoleList();
            ViewBag.MenuPageName = getDocumentName();
            _Pur_voc_Model.Title = title;
            //Session["VouSearch"] = "0";
            _Pur_voc_Model.VouSearch = "0";
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/PurchaseVoucher/PurchaseVoucherList.cshtml", _Pur_voc_Model);
        }
        public ActionResult EditVou(string VouNo, string Voudt, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
            PurchaseVoucher_Model dblclick = new PurchaseVoucher_Model();
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
            //Session["PurchaseVoucherNo"] = VouNo;
            //Session["PurchaseVoucherDate"] = Voudt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            //PurchaseVoucher_Model dblclick = new PurchaseVoucher_Model();
            UrlModel _url = new UrlModel();
            dblclick.PurchaseVoucherNo = VouNo;
            dblclick.PurchaseVoucherDate = Voudt;
            dblclick.TransType = "Update";
            //dblclick.BtnName = "BtnToDetailPage";
            if (WF_Status != null && WF_Status != "")
            {
                _url.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            //_url.Cmd = "Update";
            _url.tp = "Update";
            _url.bt = "D";
            _url.PNO = VouNo;
            _url.PDT = Voudt;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("PurchaseVoucherDetail", "PurchaseVoucher", _url);
        }
        public ActionResult AddPurchaseVoucherDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            PurchaseVoucher_Model AddNewModel = new PurchaseVoucher_Model();
            /*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
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
            //    return RedirectToAction("PurchaseVoucher", AddNewModel);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("PurchaseVoucher", AddNewModel);
            //}
            /*End to chk Financial year exist or not*/
            AddNewModel.Command = "Add";
            AddNewModel.TransType = "Save";
            AddNewModel.BtnName = "BtnAddNew";
            AddNewModel.DocumentStatus = "D";
            TempData["ModelData"] = AddNewModel;
            UrlModel AddNew_Model = new UrlModel();
            AddNew_Model.bt = "BtnAddNew";
            AddNew_Model.Cmd = "Add";
            AddNew_Model.tp = "Save";
            TempData["ListFilterData"] = null;
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("PurchaseVoucherDetail", "PurchaseVoucher", AddNew_Model);

        }
        public ActionResult PurchaseVoucherDetail(UrlModel _urlModel)
        {
            /*----------Attachment Section Start----------*/
            //Session["AttachMentDetailItmStp"] = null;
            //Session["Guid"] = null;
            /*----------Attachment Section End----------*/
            try
            {
                PurchaseVoucher_Model _PurchaseVoucher_Model = new PurchaseVoucher_Model();
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
                var _PurchaseVoucher_Model1 = TempData["ModelData"] as PurchaseVoucher_Model;
                if (_PurchaseVoucher_Model1 != null)
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    GetAutoCompleteGLDetail(_PurchaseVoucher_Model1);

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
                    _PurchaseVoucher_Model1.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _PurchaseVoucher_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_PurchaseVoucher_Model1.TransType == "Update" || _PurchaseVoucher_Model1.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["PurchaseVoucherNo"].ToString();
                        //string VouDt = Session["PurchaseVoucherDate"].ToString();
                        string VouType = "PV";
                        string VouNo = _PurchaseVoucher_Model1.PurchaseVoucherNo;
                        string VouDt = _PurchaseVoucher_Model1.PurchaseVoucherDate;
                        DataSet ds = _PurchaseVoucher_IService.GetPurchaseVoucherDetail(VouNo, VouDt, VouType, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _PurchaseVoucher_Model1.supp_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _PurchaseVoucher_Model1.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _PurchaseVoucher_Model1.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _PurchaseVoucher_Model1.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _PurchaseVoucher_Model1.Bill_Date = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        _PurchaseVoucher_Model1.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _PurchaseVoucher_Model1.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _PurchaseVoucher_Model1.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _PurchaseVoucher_Model1.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        _PurchaseVoucher_Model1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _PurchaseVoucher_Model1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _PurchaseVoucher_Model1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _PurchaseVoucher_Model1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _PurchaseVoucher_Model1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _PurchaseVoucher_Model1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _PurchaseVoucher_Model1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _PurchaseVoucher_Model1.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _PurchaseVoucher_Model1.SrcDocNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _PurchaseVoucher_Model1.SrcDocDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _PurchaseVoucher_Model1.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _PurchaseVoucher_Model1.DocumentStatus = Statuscode;
                        if (_PurchaseVoucher_Model1.Status == "C")
                        {
                            _PurchaseVoucher_Model1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _PurchaseVoucher_Model1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _PurchaseVoucher_Model1.BtnName = "Refresh";
                        }
                        else
                        {
                            _PurchaseVoucher_Model1.CancelFlag = false;
                        }

                        _PurchaseVoucher_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _PurchaseVoucher_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _PurchaseVoucher_Model1.Command != "Edit")
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
                                    _PurchaseVoucher_Model1.BtnName = "Refresh";
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
                                        _PurchaseVoucher_Model1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _PurchaseVoucher_Model1.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _PurchaseVoucher_Model1.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _PurchaseVoucher_Model1.BtnName = "BtnToDetailPage";
                                        }
                                        //_PurchaseVoucher_Model1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseVoucher_Model1.BtnName = "BtnToDetailPage";
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
                                        _PurchaseVoucher_Model1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseVoucher_Model1.BtnName = "BtnToDetailPage";
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
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseVoucher_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseVoucher_Model1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _PurchaseVoucher_Model1.BtnName = "Refresh";
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
                        _PurchaseVoucher_Model1.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.CostCenterData = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/PurchaseVoucher/PurchaseVoucherDetail.cshtml", _PurchaseVoucher_Model1);
                    }

                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _PurchaseVoucher_Model1.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/PurchaseVoucher/PurchaseVoucherDetail.cshtml", _PurchaseVoucher_Model1);
                    }
                }
                else
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _PurchaseVoucher_Model.BtnName = _urlModel.bt;
                        }
                        _PurchaseVoucher_Model.PurchaseVoucherNo = _urlModel.PNO;
                        _PurchaseVoucher_Model.PurchaseVoucherDate = _urlModel.PDT;
                        _PurchaseVoucher_Model.Command = _urlModel.Cmd;
                        _PurchaseVoucher_Model.TransType = _urlModel.tp;
                        _PurchaseVoucher_Model.WF_Status1 = _urlModel.wf;
                        _PurchaseVoucher_Model.DocumentStatus = _urlModel.DMS;
                    }
                    /* Add by Hina on 23-02-2024 to Refresh Page*/
                    if (_PurchaseVoucher_Model.TransType == null)
                    {
                        _PurchaseVoucher_Model.BtnName = "Refresh";
                        _PurchaseVoucher_Model.Command = "Refresh";
                        _PurchaseVoucher_Model.TransType = "Refresh";
                    }
                    /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var VouDate = _PurchaseVoucher_Model.PurchaseVoucherDate;
                    //var VouDate = "";

                    //if (_PurchaseVoucher_Model.PurchaseVoucherDate != null)
                    //{
                    //    VouDate = _PurchaseVoucher_Model.PurchaseVoucherDate;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    _PurchaseVoucher_Model.PurchaseVoucherDate = CurrentDate;
                    //    _PurchaseVoucher_Model.Vou_Date = CurrentDate;
                    //    VouDate = _PurchaseVoucher_Model.PurchaseVoucherDate;
                    //}
                    //if (commCont.Fin_CheckFinancialYear(CompID, Br_ID, VouDate) == "FY Not Exist")
                    //{
                    //    TempData["Message"] = "Financial Year not Exist";
                    //}
                    //if (commCont.Fin_CheckFinancialYear(CompID, Br_ID, VouDate) == "FB Close")
                    //{
                    //    TempData["FBMessage"] = "Financial Book Closing";
                    //}
                    /*End to chk Financial year exist or not*/
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    GetAutoCompleteGLDetail(_PurchaseVoucher_Model);

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
                    _PurchaseVoucher_Model.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _PurchaseVoucher_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_PurchaseVoucher_Model.TransType == "Update" || _PurchaseVoucher_Model.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["PurchaseVoucherNo"].ToString();
                        //string VouDt = Session["PurchaseVoucherDate"].ToString();
                        string VouType = "PV";
                        string VouNo = _PurchaseVoucher_Model.PurchaseVoucherNo;
                        string VouDt = _PurchaseVoucher_Model.PurchaseVoucherDate;
                        DataSet ds = _PurchaseVoucher_IService.GetPurchaseVoucherDetail(VouNo, VouDt, VouType, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _PurchaseVoucher_Model.supp_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _PurchaseVoucher_Model.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _PurchaseVoucher_Model.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _PurchaseVoucher_Model.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _PurchaseVoucher_Model.Bill_Date = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        _PurchaseVoucher_Model.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _PurchaseVoucher_Model.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _PurchaseVoucher_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _PurchaseVoucher_Model.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        _PurchaseVoucher_Model.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _PurchaseVoucher_Model.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _PurchaseVoucher_Model.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _PurchaseVoucher_Model.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _PurchaseVoucher_Model.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _PurchaseVoucher_Model.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _PurchaseVoucher_Model.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _PurchaseVoucher_Model.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _PurchaseVoucher_Model.SrcDocNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _PurchaseVoucher_Model.SrcDocDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _PurchaseVoucher_Model.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _PurchaseVoucher_Model.DocumentStatus = Statuscode;
                        if (_PurchaseVoucher_Model.Status == "C")
                        {
                            _PurchaseVoucher_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _PurchaseVoucher_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _PurchaseVoucher_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _PurchaseVoucher_Model.CancelFlag = false;
                        }

                        _PurchaseVoucher_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _PurchaseVoucher_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _PurchaseVoucher_Model.Command != "Edit")
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
                                    _PurchaseVoucher_Model.BtnName = "Refresh";
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
                                        _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                                        //if (TempData["Message"] != null)
                                        //{
                                        //    ViewBag.Message = TempData["Message"];
                                        //}
                                        //if (TempData["FBMessage"] != null)
                                        //{
                                        //    ViewBag.MessageFB = TempData["FBMessage"];
                                        //}
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _PurchaseVoucher_Model.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _PurchaseVoucher_Model.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                       // _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
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
                                        _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
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
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _PurchaseVoucher_Model.BtnName = "Refresh";
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
                        _PurchaseVoucher_Model.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.CostCenterData = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/PurchaseVoucher/PurchaseVoucherDetail.cshtml", _PurchaseVoucher_Model);
                    }

                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _PurchaseVoucher_Model.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/PurchaseVoucher/PurchaseVoucherDetail.cshtml", _PurchaseVoucher_Model);
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
        public ActionResult GetPurchaseVoucherList(string docid, string status)
        {
            //Session["WF_status"] = status;
            PurchaseVoucherList_Model Dashbord = new PurchaseVoucherList_Model();
            Dashbord.WF_Status = status;
            return RedirectToAction("PurchaseVoucher", Dashbord );
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
        public ActionResult GetAutoCompleteGLDetail(PurchaseVoucher_Model _PurchaseVoucher_Model)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> SuppAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_PurchaseVoucher_Model.SuppName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _PurchaseVoucher_Model.SuppName;
                }
                Br_ID = Session["BranchId"].ToString();
                SuppAccList = _PurchaseVoucher_IService.AutoGetSuppAccList(Comp_ID, Acc_Name, Br_ID);

                List<SuppAccName> _SuppAccNameList = new List<SuppAccName>();
                foreach (var dr in SuppAccList)
                {
                    SuppAccName _SuppAccName = new SuppAccName();
                    _SuppAccName.supp_acc_id = dr.Key;
                    _SuppAccName.supp_acc_name = dr.Value;
                    _SuppAccNameList.Add(_SuppAccName);
                }
                _PurchaseVoucher_Model.SuppAccNameList = _SuppAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SuppAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetAutoCompleteSuppDetail(PurchaseVoucherList_Model _PurchaseVoucherList_Model)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> SuppAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_PurchaseVoucherList_Model.supp_name))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _PurchaseVoucherList_Model.supp_name;
                }
                Br_ID = Session["BranchId"].ToString();
                SuppAccList = _PurchaseVoucher_IService.AutoGetSuppAccList(Comp_ID, Acc_Name, Br_ID);
                List<SuppAccList> _SuppAccNameList = new List<SuppAccList>();
                foreach (var dr in SuppAccList)
                {
                    SuppAccList _SuppAccName = new SuppAccList();
                    _SuppAccName.supp_acc_id = dr.Key;
                    _SuppAccName.supp_acc_name = dr.Value;
                    _SuppAccNameList.Add(_SuppAccName);
                }
                _PurchaseVoucherList_Model.SuppAccNameList = _SuppAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SuppAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


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
                DataTable dt = _PurchaseVoucher_IService.GetCurrList(CompID);
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
        public JsonResult GetAccCurr(string acc_id)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                string date = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                Br_ID = Session["BranchId"].ToString();
                date = DateTime.Now.ToString("yyyy-MM-dd");
                DataSet result = _PurchaseVoucher_IService.GetAccCurrOnChange(acc_id, CompID, Br_ID, date);
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
        public ActionResult PurchaseVoucherSave(PurchaseVoucher_Model _PurchaseVoucher_Model, string Vou_No, string command)
        {
            try
            {/*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/

                if (_PurchaseVoucher_Model.DeleteCommand == "Delete")
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
                        PurchaseVoucher_Model adddnew = new PurchaseVoucher_Model();
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
                        /*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
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
                        //    return RedirectToAction("PurchaseVoucherDetail", "PurchaseVoucher", adddnew);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("PurchaseVoucherDetail", "PurchaseVoucher", adddnew);
                        //}
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("PurchaseVoucherDetail", "PurchaseVoucher", NewModel);

                    case "Edit":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt = _PurchaseVoucher_Model.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_PurchaseVoucher_Model.Status == "A" || _PurchaseVoucher_Model.Status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("EditVou", new { VouNo = _PurchaseVoucher_Model.Vou_No, Voudt = _PurchaseVoucher_Model.Vou_Date, ListFilterData = _PurchaseVoucher_Model.ListFilterData1, WF_Status = _PurchaseVoucher_Model.WFStatus });
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        string FinalStatus = CheckPVForCancellation(_PurchaseVoucher_Model, _PurchaseVoucher_Model.Vou_No, _PurchaseVoucher_Model.Vou_Date.ToString());
                        if (FinalStatus == "Used" || FinalStatus == "Used1")
                        {
                            //Session["Message"] = FinalStatus;
                            _PurchaseVoucher_Model.Message = FinalStatus;
                            _PurchaseVoucher_Model.Message1 = FinalStatus;
                            _PurchaseVoucher_Model.Command = "Refresh";
                            TempData["ModelData"] = _PurchaseVoucher_Model;
                        }
                        else
                        {
                            //Session["TransType"] = "Update";
                            //Session["Command"] = command;
                            //Session["Message"] = null;
                            //Session["BtnName"] = "BtnEdit";
                            //Session["PurchaseVoucherNo"] = _PurchaseVoucher_Model.Vou_No;
                            //Session["PurchaseVoucherDate"] = _PurchaseVoucher_Model.Vou_Date.ToString();
                            _PurchaseVoucher_Model.TransType = "Update";
                            _PurchaseVoucher_Model.Command = command;
                            _PurchaseVoucher_Model.BtnName = "BtnEdit";
                            _PurchaseVoucher_Model.PurchaseVoucherNo = _PurchaseVoucher_Model.Vou_No;
                            _PurchaseVoucher_Model.PurchaseVoucherDate = _PurchaseVoucher_Model.Vou_Date;
                            TempData["ModelData"] = _PurchaseVoucher_Model;
                            UrlModel EditModel = new UrlModel();
                            EditModel.tp = "Update";
                            EditModel.Cmd = command;
                            EditModel.bt = "BtnEdit";
                            EditModel.PNO = _PurchaseVoucher_Model.Vou_No;
                            EditModel.PDT = _PurchaseVoucher_Model.Vou_Date;
                            TempData["ListFilterData"] = _PurchaseVoucher_Model.ListFilterData1;
                            return RedirectToAction("PurchaseVoucherDetail", EditModel);
                        }
                        UrlModel Edit_Model = new UrlModel();
                        Edit_Model.tp = "Update";
                        Edit_Model.bt = "D";
                        Edit_Model.PNO = _PurchaseVoucher_Model.Vou_No;
                        Edit_Model.PDT = _PurchaseVoucher_Model.Vou_Date;
                        TempData["ListFilterData"] = _PurchaseVoucher_Model.ListFilterData1;
                        return RedirectToAction("PurchaseVoucherDetail", Edit_Model);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        _PurchaseVoucher_Model.Command = command;
                        Vou_No = _PurchaseVoucher_Model.Vou_No;
                        PurchaseVoucherDelete(_PurchaseVoucher_Model, command);
                        PurchaseVoucher_Model DeleteModel = new PurchaseVoucher_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete = new UrlModel();
                        Delete.Cmd = DeleteModel.Command;
                        Delete.tp = "Refresh";
                        Delete.bt = "BtnDelete";
                        TempData["ListFilterData"] = _PurchaseVoucher_Model.ListFilterData1;
                        return RedirectToAction("PurchaseVoucherDetail", Delete);


                    case "Save":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _PurchaseVoucher_Model.Vou_Date;
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
                            if (_PurchaseVoucher_Model.Vou_No == null)
                            {
                                _PurchaseVoucher_Model.Command = "Refresh";
                                _PurchaseVoucher_Model.TransType = "Refresh";
                                _PurchaseVoucher_Model.BtnName = "Refresh";
                                _PurchaseVoucher_Model.DocumentStatus = null;
                                TempData["ModelData"] = _PurchaseVoucher_Model;
                                return RedirectToAction("PurchaseVoucherDetail", "PurchaseVoucher", _PurchaseVoucher_Model);
                            }
                            else
                            {
                                return RedirectToAction("EditVou", new { VouNo = _PurchaseVoucher_Model.Vou_No, Voudt = _PurchaseVoucher_Model.Vou_Date, ListFilterData = _PurchaseVoucher_Model.ListFilterData1, WF_Status = _PurchaseVoucher_Model.WFStatus });

                            }
                        }
                        /*End to chk Financial year exist or not*/

                        // Session["Command"] = command;
                        _PurchaseVoucher_Model.Command = command;
                        SavePurchaseVoucher(_PurchaseVoucher_Model);
                        if (_PurchaseVoucher_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_PurchaseVoucher_Model.Message == "N")
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
                            _PurchaseVoucher_Model.currList = _currList;
                            ViewBag.MenuPageName = getDocumentName();
                            GetAutoCompleteGLDetail(_PurchaseVoucher_Model);
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
                            ViewBag.CostCenterData = ViewData["CostCenter"];
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                            _PurchaseVoucher_Model.BtnName = "BtnAddNew";
                            _PurchaseVoucher_Model.Command = "Add";
                            _PurchaseVoucher_Model.Message = "N";
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/PurchaseVoucher/PurchaseVoucherDetail.cshtml", _PurchaseVoucher_Model);
                        }
                        else
                        {
                            //Session["PurchaseVoucherNo"] = Session["PurchaseVoucherNo"].ToString();
                            //Session["PurchaseVoucherDate"] = Session["PurchaseVoucherDate"].ToString();
                            TempData["ModelData"] = _PurchaseVoucher_Model;
                            UrlModel _urlModel = new UrlModel();
                            _urlModel.bt = _PurchaseVoucher_Model.BtnName;
                            //_urlModel.Cmd = _PurchaseVoucher_Model.Command;
                            _urlModel.PNO = _PurchaseVoucher_Model.PurchaseVoucherNo;
                            _urlModel.PDT = _PurchaseVoucher_Model.PurchaseVoucherDate;
                            _urlModel.tp = _PurchaseVoucher_Model.TransType;
                            TempData["ListFilterData"] = _PurchaseVoucher_Model.ListFilterData1;
                            return RedirectToAction("PurchaseVoucherDetail", _urlModel);
                        }

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _PurchaseVoucher_Model.Vou_Date;

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
                            return RedirectToAction("EditVou", new { VouNo = _PurchaseVoucher_Model.Vou_No, Voudt = _PurchaseVoucher_Model.Vou_Date, ListFilterData = _PurchaseVoucher_Model.ListFilterData1, WF_Status = _PurchaseVoucher_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/


                        //Session["Command"] = command;
                        Vou_No = _PurchaseVoucher_Model.Vou_No;
                        //Session["PurchaseVoucherNo"] = Vou_No;
                        //Session["PurchaseVoucherDate"] = _PurchaseVoucher_Model.Vou_Date;
                        PurchaseVoucherApprove(_PurchaseVoucher_Model, _PurchaseVoucher_Model.Vou_No, _PurchaseVoucher_Model.Vou_Date, "", "", "", "", "");
                        TempData["ModelData"] = _PurchaseVoucher_Model;
                        UrlModel urlref = new UrlModel();
                        urlref.tp = "Update";
                        urlref.PNO = _PurchaseVoucher_Model.PurchaseVoucherNo;
                        urlref.PDT = _PurchaseVoucher_Model.PurchaseVoucherDate;
                        urlref.bt = "BtnEdit";
                        TempData["ListFilterData"] = _PurchaseVoucher_Model.ListFilterData1;
                        return RedirectToAction("PurchaseVoucherDetail", urlref);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        //Session["DocumentStatus"] = 'D';
                        PurchaseVoucher_Model RefreshModel = new PurchaseVoucher_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _PurchaseVoucher_Model.ListFilterData1;
                        return RedirectToAction("PurchaseVoucherDetail", refesh);

                    case "Print":
                        return GenratePdfFile(_PurchaseVoucher_Model);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        PurchaseVoucherList_Model _Backtolist = new PurchaseVoucherList_Model();
                        _Backtolist.WF_Status = _PurchaseVoucher_Model.WF_Status1;
                        TempData["ListFilterData"] = _PurchaseVoucher_Model.ListFilterData1;
                        return RedirectToAction("PurchaseVoucher", "PurchaseVoucher", _Backtolist);

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
        public ActionResult SavePurchaseVoucher(PurchaseVoucher_Model _PurchaseVoucher_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (_PurchaseVoucher_Model.CancelFlag == false)
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
                    DataTable PurchaseVoucherHeader = new DataTable();
                    DataTable PurchaseVoucherGLDetails = new DataTable();
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
                    dtheader.Columns.Add("bill_no", typeof(string));
                    dtheader.Columns.Add("bill_dt", typeof(string));
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
                    if (_PurchaseVoucher_Model.Vou_No != null)
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
                    dtrowHeader["vou_type"] = "PV";
                    dtrowHeader["vou_no"] = _PurchaseVoucher_Model.Vou_No;
                    dtrowHeader["vou_dt"] = _PurchaseVoucher_Model.Vou_Date;
                    dtrowHeader["src_doc"] = "D";
                    dtrowHeader["bill_no"] = _PurchaseVoucher_Model.Bill_No;
                    dtrowHeader["bill_dt"] = _PurchaseVoucher_Model.Bill_Date;
                    dtrowHeader["vou_amt"] = _PurchaseVoucher_Model.Vou_amount;
                    dtrowHeader["remarks"] = _PurchaseVoucher_Model.Remarks;
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
                    PurchaseVoucherHeader = dtheader;

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

                    JArray jObject = JArray.Parse(_PurchaseVoucher_Model.GlAccountDetails);

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
                    PurchaseVoucherGLDetails = dtAccount;
                    ViewData["VouDetails"] = DtVouDetails(jObject);

                    /*--------------Cost Center Section Start-----------------------*/

                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));

                    JArray JAObj = JArray.Parse(_PurchaseVoucher_Model.CC_DetailList);
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
                    /*--------------Cost Center Section End-----------------------*/


                    /*-----------------Attachment Section Start------------------------*/
                    DataTable PVAttachments = new DataTable();
                    DataTable pvdtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as Pur_Vocher_Model;
                    TempData["IMGDATA"] = null;
                    if (_PurchaseVoucher_Model.attatchmentdetail != null)
                    {
                        if (attachData != null)
                        {

                            //if (Session["AttachMentDetailItmStp"] != null)
                            //{
                            //    pvdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            //}
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                pvdtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                pvdtAttachment.Columns.Add("id", typeof(string));
                                pvdtAttachment.Columns.Add("file_name", typeof(string));
                                pvdtAttachment.Columns.Add("file_path", typeof(string));
                                pvdtAttachment.Columns.Add("file_def", typeof(char));
                                pvdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {

                            //if (Session["AttachMentDetailItmStp"] != null)
                            //{
                            //    pvdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            //}
                            if (_PurchaseVoucher_Model.AttachMentDetailItmStp != null)
                            {
                                //CRdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                pvdtAttachment = _PurchaseVoucher_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                pvdtAttachment.Columns.Add("id", typeof(string));
                                pvdtAttachment.Columns.Add("file_name", typeof(string));
                                pvdtAttachment.Columns.Add("file_path", typeof(string));
                                pvdtAttachment.Columns.Add("file_def", typeof(char));
                                pvdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_PurchaseVoucher_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in pvdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = pvdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_PurchaseVoucher_Model.Vou_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = _PurchaseVoucher_Model.Vou_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                pvdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_PurchaseVoucher_Model.TransType == "Update")
                        {
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string PV_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_PurchaseVoucher_Model.Vou_No).ToString()))
                                {
                                    PV_CODE = (_PurchaseVoucher_Model.Vou_No).ToString();

                                }
                                else
                                {
                                    PV_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + Br_ID + PV_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in pvdtAttachment.Rows)
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
                        PVAttachments = pvdtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/


                    SaveMessage = _PurchaseVoucher_IService.InsertPurchaseVoucherDetail(PurchaseVoucherHeader, PurchaseVoucherGLDetails, PVAttachments, CRCostCenterDetails);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 25-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //_BankPayment_Model.Message = "Financial Year not Exist";
                        _PurchaseVoucher_Model.BtnName = "Refresh";
                        _PurchaseVoucher_Model.Command = "Refresh";
                        _PurchaseVoucher_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;
                        return RedirectToAction("PurchaseVoucherDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //_BankPayment_Model.Message = "Financial Book Closing";
                        _PurchaseVoucher_Model.BtnName = "Refresh";
                        _PurchaseVoucher_Model.Command = "Refresh";
                        _PurchaseVoucher_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;

                        return RedirectToAction("PurchaseVoucherDetail");
                    }
                    else
                    {

                        if (SaveMessage == "N")
                        {
                            _PurchaseVoucher_Model.Message = "N";
                            return RedirectToAction("PurchaseVoucherDetail");
                        }
                        else
                        {
                            string PurchaseVoucherNo = SaveMessage.Split(',')[1].Trim();
                            string PV_Number = PurchaseVoucherNo.Replace("/", "");
                            string Message = SaveMessage.Split(',')[0].Trim();
                            string PurchaseVoucherDate = SaveMessage.Split(',')[2].Trim();
                            if (Message == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message.Replace("_", " ") + " " + PurchaseVoucherNo + " in " + PageName; //PurchaseVoucherNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _PurchaseVoucher_Model.Message = Message.Split(',')[0].Replace("_", "");
                                return RedirectToAction("PurchaseVoucherDetail");
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
                                comCont.ResetImageLocation(CompID, Br_ID, guid, PageName, PV_Number, _PurchaseVoucher_Model.TransType, PVAttachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{
                                //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Br_ID + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in PVAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = CompID + Br_ID + PV_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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
                                //Session["Message"] = "Save";
                                // Session["Command"] = "Update";
                                // Session["PurchaseVoucherNo"] = PurchaseVoucherNo;
                                // Session["PurchaseVoucherDate"] = PurchaseVoucherDate;
                                // Session["TransType"] = "Update";
                                // Session["AppStatus"] = 'D';
                                // Session["BtnName"] = "BtnToDetailPage";
                                _PurchaseVoucher_Model.Message = "Save";
                            //_PurchaseVoucher_Model.Command = "Update";
                            _PurchaseVoucher_Model.PurchaseVoucherNo = PurchaseVoucherNo;
                            _PurchaseVoucher_Model.PurchaseVoucherDate = PurchaseVoucherDate;
                            _PurchaseVoucher_Model.TransType = "Update";
                            _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
                            return RedirectToAction("PurchaseVoucherDetail");
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
                    string FinalStatus = CheckPVForCancellation(_PurchaseVoucher_Model, _PurchaseVoucher_Model.Vou_No, _PurchaseVoucher_Model.Vou_Date.ToString());
                    if (FinalStatus == "Used" || FinalStatus == "Used1")
                    {
                        
                        _PurchaseVoucher_Model.Message = FinalStatus;
                        _PurchaseVoucher_Model.Message1 = FinalStatus;
                        _PurchaseVoucher_Model.Command = "Refresh";
                        TempData["ModelData"] = _PurchaseVoucher_Model;
                    }
                    else
                    {
                        _PurchaseVoucher_Model.Create_by = UserID;
                        string br_id = Session["BranchId"].ToString();
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        DataSet SaveMessage1 = _PurchaseVoucher_IService.PurchaseVoucherCancel(_PurchaseVoucher_Model, CompID, br_id, mac_id);

                        string fileName = "PV_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_PurchaseVoucher_Model.Vou_No, _PurchaseVoucher_Model.Vou_Date, fileName);
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _PurchaseVoucher_Model.Vou_No, "C", UserID, "", filePath);
                        _PurchaseVoucher_Model.Message = "Cancelled";
                        // _PurchaseVoucher_Model.Command = "Update";
                        _PurchaseVoucher_Model.PurchaseVoucherNo = _PurchaseVoucher_Model.Vou_No;
                        _PurchaseVoucher_Model.PurchaseVoucherDate = _PurchaseVoucher_Model.Vou_Date;
                        _PurchaseVoucher_Model.TransType = "Update";
                        _PurchaseVoucher_Model.BtnName = "Refresh";
                    }
                    
                    return RedirectToAction("PurchaseVoucherDetail");
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
                    if (_PurchaseVoucher_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_PurchaseVoucher_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _PurchaseVoucher_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + Br_ID, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                throw ex;
            //    return View("~/Views/Shared/Error.cshtml");
            }

        }
        public DataTable DtVouDetails(JArray jObject)
        {
            DataTable dtAccount = new DataTable();

            dtAccount.Columns.Add("acc_id", typeof(string));
            dtAccount.Columns.Add("supp_acc_id", typeof(string));
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
                dtrowLines["supp_acc_id"] = jObject[i]["supp_acc_id"].ToString();
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
        public ActionResult GetSuppAccIDDetail(string SuppAccID,string VouDate)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _PurchaseVoucher_IService.GetSuppAccIDDetail(CompID, BrchID, SuppAccID, VouDate);

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
        public void GetStatusList(PurchaseVoucherList_Model _PurchaseVoucherList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _PurchaseVoucherList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<VouList> GetPurchaseVoucherListAll(PurchaseVoucherList_Model _PurchaseVoucherList_Model,string Flag = "")
        {
            _PurchaseVoucherList = new List<VouList>();
            try
            {
                
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
                if (_PurchaseVoucherList_Model.WF_Status != null)
                {
                    wfstatus = _PurchaseVoucherList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string VouType = "PV";
                DataSet Dtdata = new DataSet();
                Dtdata = _PurchaseVoucher_IService.GetPurchaseVoucherListAll(Convert.ToInt32(_PurchaseVoucherList_Model.curr), _PurchaseVoucherList_Model.supp_id, _PurchaseVoucherList_Model.VouFromDate, _PurchaseVoucherList_Model.VouToDate, _PurchaseVoucherList_Model.Status, CompID, Br_ID, VouType, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    _PurchaseVoucherList_Model.FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (Dtdata.Tables[0].Rows.Count > 0)
                {
                    var i = 1;
                    if (Flag == "CSV")
                    {
                        foreach (DataRow dr in Dtdata.Tables[0].Rows)
                        {
                            VouList _VouList = new VouList();
                            _VouList.SrNo = i;
                            _VouList.supp_name = dr["supp_name"].ToString();
                            _VouList.VouNumber = dr["vou_no"].ToString();
                            _VouList.VouDate = dr["vou_dt"].ToString();
                            _VouList.hdVouDate = dr["vou_date"].ToString();
                            _VouList.BillNo = dr["bill_no"].ToString();
                            _VouList.BillDt = dr["bill_dt"].ToString();
                            _VouList.curr_logo = dr["curr_logo"].ToString();
                            _VouList.AmountinSp = Convert.ToDecimal(dr["vou_amt_sp"].ToString().Replace(",", ""));
                            _VouList.AmountinBs = Convert.ToDecimal(dr["vou_amt_bs"].ToString().Replace(",", ""));
                            _VouList.VouStatus = dr["vou_status"].ToString();
                            _VouList.CreatedON = dr["created_on"].ToString();
                            _VouList.ApprovedOn = dr["app_dt"].ToString();
                            _VouList.ModifiedOn = dr["mod_on"].ToString();
                            _VouList.create_by = dr["create_by"].ToString();
                            _VouList.mod_by = dr["mod_by"].ToString();
                            _VouList.app_by = dr["app_by"].ToString();

                            _PurchaseVoucherList.Add(_VouList);
                            i++;
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in Dtdata.Tables[0].Rows)
                        {
                            VouList _VouList = new VouList();
                            _VouList.SrNo = i;
                            _VouList.supp_name = dr["supp_name"].ToString();
                            _VouList.VouNumber = dr["vou_no"].ToString();
                            _VouList.VouDate = dr["vou_dt"].ToString();
                            _VouList.hdVouDate = dr["vou_date"].ToString();
                            _VouList.BillNo = dr["bill_no"].ToString();
                            _VouList.BillDt = dr["bill_dt"].ToString();
                            _VouList.curr_logo = dr["curr_logo"].ToString();
                            _VouList.AmountinSp = Convert.ToDecimal(dr["vou_amt_sp"].ToString().Replace(",", ""));
                            _VouList.AmountinBs = Convert.ToDecimal(dr["vou_amt_bs"].ToString().Replace(",", ""));
                            _VouList.VouStatus = dr["vou_status"].ToString();
                            _VouList.CreatedON = dr["created_on"].ToString();
                            _VouList.ApprovedOn = dr["app_dt"].ToString();
                            _VouList.ModifiedOn = dr["mod_on"].ToString();
                            _VouList.create_by = dr["create_by"].ToString();
                            _VouList.mod_by = dr["mod_by"].ToString();
                            _VouList.app_by = dr["app_by"].ToString();

                            _PurchaseVoucherList.Add(_VouList);
                            i++;
                        }
                    }
                    
                }
                
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return _PurchaseVoucherList;
            }
            return _PurchaseVoucherList;
        }
        [HttpPost]
        public ActionResult SearchPurchaseVoucherDetail(int curr, string supp_id, string Fromdate, string Todate, string Status, string CompID, string Br_ID)
        {
            _PurchaseVoucherList = new List<VouList>();
            PurchaseVoucherList_Model _PurchaseVoucherList_Model = new PurchaseVoucherList_Model();
            //Session["WF_status"] = "";
            _PurchaseVoucherList_Model.WF_Status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            string VouType = "PV";
            DataSet dt = new DataSet();
            dt = _PurchaseVoucher_IService.GetPurchaseVoucherListAll(curr, supp_id, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
            //Session["VouSearch"] = "Vou_Search";
            _PurchaseVoucherList_Model.VouSearch = "Vou_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    VouList _VouList = new VouList();
                    _VouList.supp_name = dr["supp_name"].ToString();
                    _VouList.VouNumber = dr["vou_no"].ToString();
                    _VouList.VouDate = dr["vou_dt"].ToString();
                    _VouList.hdVouDate = dr["vou_date"].ToString();
                    _VouList.BillNo = dr["bill_no"].ToString();
                    _VouList.BillDt = dr["bill_dt"].ToString();
                    _VouList.curr_logo = dr["curr_logo"].ToString();
                    _VouList.AmountinSp = Convert.ToDecimal(dr["vou_amt_sp"].ToString().Replace(",", ""));
                    _VouList.AmountinBs = Convert.ToDecimal(dr["vou_amt_bs"].ToString().Replace(",", ""));
                    _VouList.VouStatus = dr["vou_status"].ToString();
                    _VouList.CreatedON = dr["created_on"].ToString();
                    _VouList.ApprovedOn = dr["app_dt"].ToString();
                    _VouList.ModifiedOn = dr["mod_on"].ToString();
                    _VouList.create_by = dr["create_by"].ToString();
                    _VouList.mod_by = dr["mod_by"].ToString();
                    _VouList.app_by = dr["app_by"].ToString();

                    _PurchaseVoucherList.Add(_VouList);
                }
            }
            _PurchaseVoucherList_Model.VoucherList = _PurchaseVoucherList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPurchaseVoucherList.cshtml", _PurchaseVoucherList_Model);
        }

        private ActionResult PurchaseVoucherDelete(PurchaseVoucher_Model _PurchaseVoucher_Model, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string PVNo = _PurchaseVoucher_Model.Vou_No;
                string PurVouNumber = PVNo.Replace("/", "");

                string Message = _PurchaseVoucher_IService.PVDelete(_PurchaseVoucher_Model, CompID, br_id, DocumentMenuId);
                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(PurVouNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, PurVouNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["PurchaseVoucherNo"] = "";
                //Session["PurchaseVoucherDate"] = "";
                //_PurchaseVoucher_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("PurchaseVoucherDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult PurchaseVoucherApprove(PurchaseVoucher_Model _PurchaseVoucher_Model, string VouNo, string VouDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1)
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
                //}
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
                string Message = _PurchaseVoucher_IService.PurchaseVoucherApprove(VouNo, VouDate, UserID, A_Status, A_Level, A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string PurchaseVoucherNo = Message.Split(',')[0].Trim();
                string PurchaseVoucherDate = Message.Split(',')[1].Trim();
                //Session["PurchaseVoucherNo"] = PurchaseVoucherNo;
                //Session["PurchaseVoucherDate"] = PurchaseVoucherDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel ApproveModel = new UrlModel();
                _PurchaseVoucher_Model.TransType = "Update";
                _PurchaseVoucher_Model.PurchaseVoucherNo = PurchaseVoucherNo;
                _PurchaseVoucher_Model.PurchaseVoucherDate = PurchaseVoucherDate;
                _PurchaseVoucher_Model.Message = "Approved";
                _PurchaseVoucher_Model.BtnName = "BtnEdit";
                string fileName = "PV_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                var filePath = SavePdfDocToSendOnEmailAlert(VouNo, VouDate, fileName);
                _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, VouNo, "AP", UserID, "", filePath);
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _PurchaseVoucher_Model.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _PurchaseVoucher_Model;

                ApproveModel.tp = "Update";
                ApproveModel.PNO = PurchaseVoucherNo;
                ApproveModel.PDT = PurchaseVoucherDate;//(PDT = PurchaseVoucherDate && PNO = PurchaseVoucherNo)
                ApproveModel.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("PurchaseVoucherDetail", "PurchaseVoucher", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData)
        {
            // Session["Message"] = "";
            PurchaseVoucher_Model ToRefreshByJS = new PurchaseVoucher_Model();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.PurchaseVoucherNo = a[0].Trim();
            ToRefreshByJS.PurchaseVoucherDate = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnToDetailPage";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                urlModel.wf = a[2].Trim();
            }
            urlModel.bt = "D";
            urlModel.PNO = ToRefreshByJS.PurchaseVoucherNo;
            urlModel.PDT = ToRefreshByJS.PurchaseVoucherDate;
            urlModel.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("PurchaseVoucherDetail", urlModel);
        }
        public string CheckPVForCancellation(PurchaseVoucher_Model _PurchaseVoucher_Model, string DocNo, string DocDate)
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
                DataSet Deatils = _PurchaseVoucher_IService.CheckPVDetail(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
                if (Deatils.Tables[1].Rows.Count > 0)
                {
                    str = "Used1";
                }
                if (str != "" && str != null)
                {
                    _PurchaseVoucher_Model.PurchaseVoucherNo = DocNo;
                    _PurchaseVoucher_Model.PurchaseVoucherDate = DocDate;
                    _PurchaseVoucher_Model.TransType = "Update";
                    _PurchaseVoucher_Model.BtnName = "BtnToDetailPage";
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

        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Pur_Vocher_Model _attachmentModel = new Pur_Vocher_Model();
                //string TransType = "";
                //string PVCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["PurchaseVoucherNo"] != null)
                //{
                //    PVCode = Session["PurchaseVoucherNo"].ToString();
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

        /*------------- Cost Center Section-----------------*/

        public ActionResult GetCstCntrtype(string Flag, string Disableflag, string CC_rowdata)
        {
            try
            {
                CostCenterDt _CC_Model = new CostCenterDt();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                if (CC_rowdata.ToString() != null && CC_rowdata.ToString() != "" && CC_rowdata.ToString() != "[]")
                {
                    DataTable Cctype = new DataTable();
                    Cctype.Columns.Add("GlAccount", typeof(string));
                    Cctype.Columns.Add("ddl_cc_type", typeof(string));
                    Cctype.Columns.Add("dd_cc_type_id", typeof(string));
                    JArray arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow dtrowLines = Cctype.NewRow();
                        dtrowLines["GlAccount"] = arr[i]["GlAccount"].ToString();
                        dtrowLines["ddl_cc_type"] = arr[i]["ddl_CC_Type"].ToString();
                        dtrowLines["dd_cc_type_id"] = arr[i]["ddl_Type_Id"].ToString();
                        Cctype.Rows.Add(dtrowLines);
                    }
                    DataView dv = new DataView();
                    dv = Cctype.DefaultView;
                    ViewBag.CC_type = dv.ToTable(true, "GlAccount", "ddl_cc_type", "dd_cc_type_id");

                    DataTable ccitem = new DataTable();
                    ccitem.Columns.Add("dd_cc_typ_id", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name", typeof(string));
                    ccitem.Columns.Add("ddl_cc_name_id", typeof(string));
                    ccitem.Columns.Add("cc_Amount", typeof(string));

                    JArray Arr = JArray.Parse(CC_rowdata);
                    for (int i = 0; i < arr.Count; i++)
                    {
                        DataRow DtrowLines = ccitem.NewRow();
                        DtrowLines["dd_cc_typ_id"] = arr[i]["ddl_Type_Id"].ToString();
                        DtrowLines["ddl_cc_name"] = arr[i]["ddl_CC_Name"].ToString();
                        DtrowLines["ddl_cc_name_id"] = arr[i]["ddl_Name_Id"].ToString();
                        DtrowLines["cc_Amount"] = arr[i]["CC_Amount"].ToString();
                        ccitem.Rows.Add(DtrowLines);
                    }
                    ViewBag.CC_Item = ccitem;
                }


                DataSet ds = _Common_IServices.GetCstCntrData(CompID, Br_ID, "0", Flag);

                List<CostcntrType> Cctypelist = new List<CostcntrType>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CostcntrType Cc_type = new CostcntrType();
                    Cc_type.cc_id = dr["cc_id"].ToString();
                    Cc_type.cc_name = dr["cc_name"].ToString();
                    Cctypelist.Add(Cc_type);
                }
                Cctypelist.Insert(0, new CostcntrType() { cc_id = "0", cc_name = "---Select---" });
                _CC_Model.costcntrtype = Cctypelist;

                _CC_Model.disflag = Disableflag;

                return PartialView("~/Areas/Common/Views/Cmn_PartialCostCenterDetail.cshtml", _CC_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public JsonResult GetCstCntrName(string CCtypeid)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet ds = _Common_IServices.GetCstCntrData(CompID, Br_ID, CCtypeid, "ccname");
                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        /*------------- Cost Center Section END ----------------*/
        /*--------Print---------*/

        public FileResult GenratePdfFile(PurchaseVoucher_Model _Model)
        {
            return File(GetPdfData(_Model.Vou_No,_Model.Vou_Date), "application/pdf", "PurchaseVoucher.pdf");
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
                DataSet Deatils = _Common_IServices.Cmn_GetGLVoucherPrintDeatils(CompID, Br_ID, vNo, vDate, "PV");
                ViewBag.PageName = "PV";
                ViewBag.Title = "Purchase Voucher";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();

                /* Added by Suraj Maurya on 17-02-2025 to add logo*/
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                /* Added by Suraj Maurya on 17-02-2025 to add logo End */

                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_GLVoucher_Print.cshtml"));
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
                    string draftImage = string.Empty;
                    if (ViewBag.DocStatus == "C")/*Add by NItesh on 10-09-2025 */
                    {
                         draftImage = Server.MapPath("~/Content/Images/cancelled.png");/*Add by NItesh on 10-09-2025 */
                    }
                    else
                    {
                         draftImage = Server.MapPath("~/Content/Images/draft.png");/*Add by Hina on 16-10-2024 */
                    }
                        //string draftImage = Server.MapPath("~/Content/Images/draft.png");/*Add by Hina on 16-10-2024 */
                        using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);/*Add by Hina on 16-10-2024 */
                                //draftimg.SetAbsolutePosition(0, 160);/*Commented By NItesh 10092025*/
                                //draftimg.ScaleAbsolute(580f, 580f);
                                draftimg.SetAbsolutePosition(0, 10);
                                draftimg.ScaleAbsolute(750f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F" || ViewBag.DocStatus == "C")/*Add by Hina on 16-10-2024 */
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
        public string SavePdfDocToSendOnEmailAlert(string poNo, string poDate, string fileName)
        {
            var data = GetPdfData(poNo, poDate);
            var commonCont = new CommonController(_Common_IServices);
            return commonCont.SaveAlertDocument(data, fileName);
        }
        public FileResult PurchaseVoucherExporttoExcelDt(int curr, string SuppName, string Fromdate, string Todate, string Status)
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
                string VouType = "PV";
                DataTable dt = new DataTable();
                DataSet dt1 = _PurchaseVoucher_IService.GetPurchaseVoucherListAll(curr, SuppName, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("GL Voucher Number", typeof(string));
                dt.Columns.Add("GL Voucher Date", typeof(string));
                dt.Columns.Add("Supplier Name", typeof(string));
                dt.Columns.Add("Currency", typeof(string));
                dt.Columns.Add("Amount (In Specific)", typeof(decimal));
                dt.Columns.Add("Amount (In Base)", typeof(decimal));
                dt.Columns.Add("Bill Number", typeof(string));
                dt.Columns.Add("Bill Date", typeof(string));
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
                        dtrowLines["Supplier Name"] = dr["supp_name"].ToString();
                        dtrowLines["Currency"] = dr["curr_logo"].ToString();
                        dtrowLines["Amount (In Specific)"] = dr["vou_amt_sp"].ToString();
                        dtrowLines["Amount (In Base)"] = dr["vou_amt_bs"].ToString();
                        dtrowLines["Bill Number"] = dr["bill_no"].ToString();
                        dtrowLines["Bill Date"] = dr["bill_dt"].ToString();
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
                return commonController.ExportDatatableToExcel("PurchaseVoucher", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        /*-------------------------Purchase Vouchar List (Server Side)-------------------------*/
        public JsonResult LoadPVListData(string ListFilter/*, string IncludeZeroStockFlag, string StockBy, string ItemId, string GroupId, string BrachID, string WarehouseID, string UpToDate, string PortfolioId, string hsnCode*/)
        {
            try
            {


                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToUpper();

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<VouList> _VouListModel = new List<VouList>();
                PurchaseVoucherList_Model _Pur_voc_Model = new PurchaseVoucherList_Model();
                var a = ListFilter.Split(',');
                _Pur_voc_Model.supp_id = a[0].Trim();
                _Pur_voc_Model.curr = Convert.ToInt32(a[1].Trim());
                _Pur_voc_Model.VouFromDate = a[2].Trim();
                _Pur_voc_Model.VouToDate = a[3].Trim();
                _Pur_voc_Model.Status = a[4].Trim();
                _Pur_voc_Model.WF_Status = a[5].Trim();
                _VouListModel = GetPurchaseVoucherListAll(_Pur_voc_Model);

                // Getting all Voucher data    
                var ItemListData = (from tempitem in _VouListModel select tempitem);

                //Search
                ItemListData = FilteredList(ItemListData, searchValue);
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    searchValue = searchValue.ToUpper();
                //    ItemListData = ItemListData.Where(m => m.VouNumber.ToUpper().Contains(searchValue) || m.VouDate.ToUpper().Contains(searchValue)
                //    || m.supp_name.ToUpper().Contains(searchValue) || m.curr_logo.ToUpper().Contains(searchValue) 
                //    || m.AmountinSp.ToUpper().Contains(searchValue)|| m.AmountinBs.ToUpper().Contains(searchValue)
                //    || m.BillNo.ToUpper().Contains(searchValue) || m.BillDt.ToUpper().Contains(searchValue)
                //    || m.VouStatus.ToUpper().Contains(searchValue) || m.create_by.ToUpper().Contains(searchValue) 
                //    || m.CreatedON.ToUpper().Contains(searchValue)|| m.app_by.ToUpper().Contains(searchValue) 
                //    || m.ApprovedOn.ToUpper().Contains(searchValue) || m.mod_by.ToUpper().Contains(searchValue) 
                //    || m.ModifiedOn.ToUpper().Contains(searchValue)
                //    );
                //}

                //Sorting
                ItemListData = ShortVoucherList(ItemListData, sortColumn, sortColumnDir);
                //total number of rows count     
                recordsTotal = ItemListData.Count();
                //Paging     
                var data = ItemListData.Skip(skip).Take(pageSize).ToList();
                //var data = ItemListData.Take(pageSize).ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        private IEnumerable<VouList> FilteredList(IEnumerable<VouList> ItemListData, string searchValue)/* Added by Suraj Maurya on 04-10-2025 for filter */
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToUpper();
                ItemListData = ItemListData.Where(m => m.VouNumber.ToUpper().Contains(searchValue) || m.VouDate.ToUpper().Contains(searchValue)
                    || m.supp_name.ToUpper().Contains(searchValue) || m.curr_logo.ToUpper().Contains(searchValue)
                    || m.AmountinSp.ToString().ToUpper().Contains(searchValue) || m.AmountinBs.ToString().ToUpper().Contains(searchValue)
                    || m.BillNo.ToUpper().Contains(searchValue) || m.BillDt.ToUpper().Contains(searchValue)
                    || m.VouStatus.ToUpper().Contains(searchValue) || m.create_by.ToUpper().Contains(searchValue)
                    || m.CreatedON.ToUpper().Contains(searchValue) || m.app_by.ToUpper().Contains(searchValue)
                    || m.ApprovedOn.ToUpper().Contains(searchValue) || m.mod_by.ToUpper().Contains(searchValue)
                    || m.ModifiedOn.ToUpper().Contains(searchValue)
                    );
            }

            return ItemListData;
        }
        private IEnumerable<VouList> ShortVoucherList(IEnumerable<VouList> ItemListData, string sortColumn,string sortColumnDir)
        {
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                if (sortColumn == "VouDate")
                {
                    if (sortColumnDir == "asc")
                    {
                        ItemListData = ItemListData
                            .OrderBy(x => x.VouDate == null || x.VouDate == "" ? DateTime.MaxValue : Convert.ToDateTime(x.VouDate));
                    }
                    else
                    {
                        ItemListData = ItemListData
                            .OrderByDescending(x => x.VouDate == null || x.VouDate == "" ? DateTime.MinValue : Convert.ToDateTime(x.VouDate));
                    }
                }
                else if (sortColumn == "CreatedON")
                {
                    if (sortColumnDir == "asc")
                    {
                        ItemListData = ItemListData
                            .OrderBy(x => x.CreatedON == null || x.CreatedON == "" ? DateTime.MaxValue : Convert.ToDateTime(x.CreatedON));
                    }
                    else
                    {
                        ItemListData = ItemListData
                            .OrderByDescending(x => x.CreatedON == null || x.CreatedON == "" ? DateTime.MinValue : Convert.ToDateTime(x.CreatedON));
                    }
                }
                else if (sortColumn == "ModifiedOn")
                {
                    if (sortColumnDir == "asc")
                    {
                        ItemListData = ItemListData
                            .OrderBy(x => x.ModifiedOn == null || x.ModifiedOn == "" ? DateTime.MaxValue : Convert.ToDateTime(x.ModifiedOn));
                    }
                    else
                    {
                        ItemListData = ItemListData
                            .OrderByDescending(x => x.ModifiedOn == null || x.ModifiedOn == "" ? DateTime.MinValue : Convert.ToDateTime(x.ModifiedOn));
                    }
                }
                else if (sortColumn == "ApprovedOn")
                {
                    if (sortColumnDir == "asc")
                    {
                        ItemListData = ItemListData
                            .OrderBy(x => x.ApprovedOn == null || x.ApprovedOn == "" ? DateTime.MaxValue : Convert.ToDateTime(x.ApprovedOn));
                    }
                    else
                    {
                        ItemListData = ItemListData
                            .OrderByDescending(x => x.ApprovedOn == null || x.ApprovedOn == "" ? DateTime.MinValue : Convert.ToDateTime(x.ApprovedOn));
                    }
                }
                else
                {
                    ItemListData = ItemListData.OrderBy(sortColumn + " " + sortColumnDir);
                }
            }
            return ItemListData;
        }

        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request, string ListFilter)
        {
            string keyword = "";
            // Apply search filter
            if (!string.IsNullOrEmpty(request.search?.value))
            {
                keyword = request.search.value;//.ToLower();
            }
            
            // 🔹 Fetch data same as LoadData but ignore paging
            
            PurchaseVoucherList_Model _Pur_voc_Model = new PurchaseVoucherList_Model();
            var a = ListFilter.Split(',');
            _Pur_voc_Model.supp_id = a[0].Trim();
            _Pur_voc_Model.curr = Convert.ToInt32(a[1].Trim());
            _Pur_voc_Model.VouFromDate = a[2].Trim();
            _Pur_voc_Model.VouToDate = a[3].Trim();
            _Pur_voc_Model.Status = a[4].Trim();
            _Pur_voc_Model.WF_Status = a[5].Trim();
            IEnumerable<VouList> _VouListModel = FilteredList(GetPurchaseVoucherListAll(_Pur_voc_Model,"CSV"), keyword); 

            if (request.order != null && request.order.Any())
            {
                var colIndex = request.order[0].column;
                var dir = request.order[0].dir;
                var sortColumn = request.columns[colIndex].data;

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(dir)))
                {
                    _VouListModel = ShortVoucherList(_VouListModel, sortColumn, dir);
                }
            }
            
            var data = _VouListModel.ToList(); // All filtered & sorted rows
            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data);
        }

        /*-------------------------Purchase Vouchar List (Server Side) End-------------------------*/

    }
}