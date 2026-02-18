using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.SalesVoucher
{
    public class SalesVoucherController : Controller
    {
        string CompID, language, FromDate = String.Empty;
        string Comp_ID, Br_ID, Language, title, UserID = String.Empty;
        string DocumentMenuId = "105104115145";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SalesVoucher_IService _SalesVoucher_IService;
        DataTable dt;
        List<VouList> _SalesVoucherList;
        public SalesVoucherController(Common_IServices _Common_IServices, SalesVoucher_IService _SalesVoucher_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._SalesVoucher_IService = _SalesVoucher_IService;
        }
        // GET: ApplicationLayer/SalesVoucher
        public ActionResult SalesVoucher(SalesVoucherList_Model _SalesVoucherList_Model)
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
           // SalesVoucherList_Model _SalesVoucherList_Model = new SalesVoucherList_Model();
            GetStatusList(_SalesVoucherList_Model);
            GetAutoCompleteCustDetail(_SalesVoucherList_Model);
            List<Custcurr> _currList = new List<Custcurr>();
            dt = Getcurr();
            foreach (DataRow dr in dt.Rows)
            {
                Custcurr _curr = new Custcurr();
                _curr.curr_id = dr["curr_id"].ToString();
                _curr.curr_name = dr["curr_name"].ToString();
                _currList.Add(_curr);

            }
            _currList.Insert(0, new Custcurr() { curr_id = "0", curr_name = "---Select---" });
            _SalesVoucherList_Model.currList = _currList;
            DateTime dtnow = DateTime.Now;
            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //string endDate = dtnow.ToString("yyyy-MM-dd");
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _SalesVoucherList_Model.curr = Convert.ToInt32(a[1].Trim());
                _SalesVoucherList_Model.cust_id = a[0].Trim();
                _SalesVoucherList_Model.VouFromDate = a[2].Trim();
                _SalesVoucherList_Model.VouToDate = a[3].Trim();
                _SalesVoucherList_Model.Status = a[4].Trim();
                if (_SalesVoucherList_Model.Status == "0")
                {
                    _SalesVoucherList_Model.Status = null;
                }
                _SalesVoucherList_Model.ListFilterData = TempData["ListFilterData"].ToString();
            }
            _SalesVoucherList_Model.VoucherList = GetSalesVoucherListAll(_SalesVoucherList_Model);
            if (_SalesVoucherList_Model.VouFromDate != null)
            {
                _SalesVoucherList_Model.FromDate = _SalesVoucherList_Model.VouFromDate;
            }
            //else
            //{
            //    _SalesVoucherList_Model.FromDate = startDate;
            //}
            //_SalesVoucherList_Model.FromDate = startDate;
            
            ViewBag.VBRoleList = GetRoleList();
            ViewBag.MenuPageName = getDocumentName();
            //Session["VouSearch"] = "0";
            _SalesVoucherList_Model.VouSearch = "0";

            ViewBag.MenuPageName = getDocumentName();
            _SalesVoucherList_Model.Title = title;
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/SalesVoucher/SalesVoucherList.cshtml", _SalesVoucherList_Model);
        }
        public ActionResult EditVou(string VouNo, string Voudt, string ListFilterData,string WF_Status)
        {/*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
            SalesVoucher_Model dblclick = new SalesVoucher_Model();
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
            //Session["SalesVoucherNo"] = VouNo;
            //Session["SalesVoucherDate"] = Voudt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            //SalesVoucher_Model dblclick = new SalesVoucher_Model();
            UrlModel _url = new UrlModel();
            dblclick.SalesVoucherNo = VouNo;
            dblclick.SalesVoucherDate = Voudt;
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
            _url.SV_No = VouNo;
            _url.SV_DT = Voudt;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("SalesVoucherDetail", "SalesVoucher", _url);
        }
        public ActionResult AddSalesVoucherDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            SalesVoucher_Model AddNewModel = new SalesVoucher_Model();
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
            //    return RedirectToAction("SalesVoucher", AddNewModel);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("SalesVoucher", AddNewModel);
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
            return RedirectToAction("SalesVoucherDetail", "SalesVoucher", AddNew_Model);
        }
        public ActionResult SalesVoucherDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/

                SalesVoucher_Model _SalesVoucher_Model = new SalesVoucher_Model();
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
                var _SalesVoucher_Model1 = TempData["ModelData"] as SalesVoucher_Model;
                if (_SalesVoucher_Model1 != null)
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    GetAutoCompleteGLDetail(_SalesVoucher_Model1);

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
                    _SalesVoucher_Model1.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SalesVoucher_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_SalesVoucher_Model1.TransType == "Update" || _SalesVoucher_Model1.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["SalesVoucherNo"].ToString();
                        //string VouDt = Session["SalesVoucherDate"].ToString();
                        string VouType = "SV";
                        string VouNo = _SalesVoucher_Model1.SalesVoucherNo;
                        string VouDt = _SalesVoucher_Model1.SalesVoucherDate;
                        DataSet ds = _SalesVoucher_IService.GetSalesVoucherDetail(VouNo, VouDt, VouType, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _SalesVoucher_Model1.cust_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _SalesVoucher_Model1.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _SalesVoucher_Model1.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _SalesVoucher_Model1.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _SalesVoucher_Model1.Bill_Date = ds.Tables[0].Rows[0]["bill_dt"].ToString()==""?"":Convert.ToDateTime(ds.Tables[0].Rows[0]["bill_dt"].ToString()).ToString("yyyy-MM-dd");
                        _SalesVoucher_Model1.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _SalesVoucher_Model1.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _SalesVoucher_Model1.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _SalesVoucher_Model1.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        _SalesVoucher_Model1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _SalesVoucher_Model1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _SalesVoucher_Model1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _SalesVoucher_Model1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _SalesVoucher_Model1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _SalesVoucher_Model1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _SalesVoucher_Model1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _SalesVoucher_Model1.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _SalesVoucher_Model1.SalePerson = ds.Tables[0].Rows[0]["sls_per"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _SalesVoucher_Model1.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _SalesVoucher_Model1.DocumentStatus = Statuscode;
                        if (_SalesVoucher_Model1.Status == "C")
                        {
                            _SalesVoucher_Model1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _SalesVoucher_Model1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _SalesVoucher_Model1.BtnName = "Refresh";
                        }
                        else
                        {
                            _SalesVoucher_Model1.CancelFlag = false;
                        }

                        _SalesVoucher_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _SalesVoucher_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _SalesVoucher_Model1.Command != "Edit")
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
                                    _SalesVoucher_Model1.BtnName = "Refresh";
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
                                        _SalesVoucher_Model1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        // Session["BtnName"] = "BtnToDetailPage";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _SalesVoucher_Model1.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _SalesVoucher_Model1.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _SalesVoucher_Model1.BtnName = "BtnToDetailPage";
                                        }
                                        //_SalesVoucher_Model1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    // Session["BtnName"] = "BtnToDetailPage";
                                    _SalesVoucher_Model1.BtnName = "BtnToDetailPage";
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
                                        _SalesVoucher_Model1.BtnName = "BtnToDetailPage";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    _SalesVoucher_Model1.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";
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
                                    _SalesVoucher_Model1.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _SalesVoucher_Model1.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";

                                }
                                else
                                {
                                    _SalesVoucher_Model1.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "Refresh";
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
                        _SalesVoucher_Model1.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.CostCenterData = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        DataTable dt = getSalesPersonList();
                        List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                        foreach (DataRow data in dt.Rows)
                        {
                            SalePersonList _SlPrsnDetail = new SalePersonList();
                            _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                            _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                            _SlPrsnList.Add(_SlPrsnDetail);
                        }
                        _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                        _SalesVoucher_Model1.SalePersonList = _SlPrsnList;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/SalesVoucher/SalesVoucherDetail.cshtml", _SalesVoucher_Model1);
                    }

                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _SalesVoucher_Model1.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        DataTable dt = getSalesPersonList();
                        List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                        foreach (DataRow data in dt.Rows)
                        {
                            SalePersonList _SlPrsnDetail = new SalePersonList();
                            _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                            _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                            _SlPrsnList.Add(_SlPrsnDetail);
                        }
                        _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                        _SalesVoucher_Model1.SalePersonList = _SlPrsnList;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/SalesVoucher/SalesVoucherDetail.cshtml", _SalesVoucher_Model1);
                    }
                }
                else
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _SalesVoucher_Model.BtnName = _urlModel.bt;
                        }
                        _SalesVoucher_Model.SalesVoucherNo = _urlModel.SV_No;
                        _SalesVoucher_Model.SalesVoucherDate = _urlModel.SV_DT;
                        _SalesVoucher_Model.Command = _urlModel.Cmd;
                        _SalesVoucher_Model.TransType = _urlModel.tp;
                        _SalesVoucher_Model.WF_Status1 = _urlModel.wf;
                        _SalesVoucher_Model.DocumentStatus = _urlModel.DMS;
                    }
                    /* Add by Hina on 23-02-2024 to Refresh Page*/
                    if (_SalesVoucher_Model.TransType == null)
                    {
                        _SalesVoucher_Model.BtnName = "Refresh";
                        _SalesVoucher_Model.Command = "Refresh";
                        _SalesVoucher_Model.TransType = "Refresh";
                    }
                    /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var VouDate = _SalesVoucher_Model.SalesVoucherDate;
                    //var VouDate = "";

                    //if (_SalesVoucher_Model.SalesVoucherDate != null)
                    //{
                    //    VouDate = _SalesVoucher_Model.SalesVoucherDate;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    _SalesVoucher_Model.SalesVoucherDate = CurrentDate;
                    //    _SalesVoucher_Model.Vou_Date = CurrentDate;
                    //    VouDate = _SalesVoucher_Model.SalesVoucherDate;
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
                    GetAutoCompleteGLDetail(_SalesVoucher_Model);

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
                    _SalesVoucher_Model.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _SalesVoucher_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_SalesVoucher_Model.TransType == "Update" || _SalesVoucher_Model.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["SalesVoucherNo"].ToString();
                        //string VouDt = Session["SalesVoucherDate"].ToString();
                        string VouType = "SV";
                        string VouNo = _SalesVoucher_Model.SalesVoucherNo;
                        string VouDt = _SalesVoucher_Model.SalesVoucherDate;
                        DataSet ds = _SalesVoucher_IService.GetSalesVoucherDetail(VouNo, VouDt, VouType, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _SalesVoucher_Model.cust_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _SalesVoucher_Model.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _SalesVoucher_Model.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _SalesVoucher_Model.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _SalesVoucher_Model.Bill_Date = ds.Tables[0].Rows[0]["bill_dt"].ToString()==""?"":Convert.ToDateTime(ds.Tables[0].Rows[0]["bill_dt"].ToString()).ToString("yyyy-MM-dd");
                        _SalesVoucher_Model.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _SalesVoucher_Model.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _SalesVoucher_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _SalesVoucher_Model.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        _SalesVoucher_Model.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _SalesVoucher_Model.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _SalesVoucher_Model.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _SalesVoucher_Model.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _SalesVoucher_Model.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _SalesVoucher_Model.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _SalesVoucher_Model.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _SalesVoucher_Model.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _SalesVoucher_Model.SalePerson = ds.Tables[0].Rows[0]["sls_per"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        _SalesVoucher_Model.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _SalesVoucher_Model.DocumentStatus = Statuscode;
                        if (_SalesVoucher_Model.Status == "C")
                        {
                            _SalesVoucher_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _SalesVoucher_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _SalesVoucher_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _SalesVoucher_Model.CancelFlag = false;
                        }

                        _SalesVoucher_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _SalesVoucher_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _SalesVoucher_Model.Command != "Edit")
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
                                    _SalesVoucher_Model.BtnName = "Refresh";
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
                                        _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _SalesVoucher_Model.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _SalesVoucher_Model.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                                        }
                                        // Session["BtnName"] = "BtnToDetailPage";
                                        // _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    // Session["BtnName"] = "BtnToDetailPage";
                                    _SalesVoucher_Model.BtnName = "BtnToDetailPage";
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
                                        _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";
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
                                    _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "BtnToDetailPage";

                                }
                                else
                                {
                                    _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                                    //Session["BtnName"] = "Refresh";
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
                        _SalesVoucher_Model.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.CostCenterData = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        DataTable dt = getSalesPersonList();
                        List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                        foreach (DataRow data in dt.Rows)
                        {
                            SalePersonList _SlPrsnDetail = new SalePersonList();
                            _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                            _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                            _SlPrsnList.Add(_SlPrsnDetail);
                        }
                        _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                        _SalesVoucher_Model.SalePersonList = _SlPrsnList;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/SalesVoucher/SalesVoucherDetail.cshtml", _SalesVoucher_Model);
                    }

                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _SalesVoucher_Model.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.MenuPageName = getDocumentName();
                        DataTable dt = getSalesPersonList();
                        List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                        foreach (DataRow data in dt.Rows)
                        {
                            SalePersonList _SlPrsnDetail = new SalePersonList();
                            _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                            _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                            _SlPrsnList.Add(_SlPrsnDetail);
                        }
                        _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                        _SalesVoucher_Model.SalePersonList = _SlPrsnList;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/SalesVoucher/SalesVoucherDetail.cshtml", _SalesVoucher_Model);
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
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
        public ActionResult GetSalesVoucherList(string docid, string status)
        {
            // Session["WF_status"] = status;
            SalesVoucherList_Model Dashbord = new SalesVoucherList_Model();
            Dashbord.WF_Status = status;
            return RedirectToAction("SalesVoucher", Dashbord);
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
        public ActionResult GetAutoCompleteGLDetail(SalesVoucher_Model _SalesVoucher_Model)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> CustAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_SalesVoucher_Model.CustName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _SalesVoucher_Model.CustName;
                }
                Br_ID = Session["BranchId"].ToString();
                CustAccList = _SalesVoucher_IService.AutoGetCustAccList(Comp_ID, Acc_Name, Br_ID);

                List<CustAccName> _CustAccNameList = new List<CustAccName>();
                foreach (var dr in CustAccList)
                {
                    CustAccName _CustAccName = new CustAccName();
                    _CustAccName.cust_acc_id = dr.Key;
                    _CustAccName.cust_acc_name = dr.Value;
                    _CustAccNameList.Add(_CustAccName);
                }
                _SalesVoucher_Model.CustAccNameList = _CustAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(CustAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetAutoCompleteCustDetail(SalesVoucherList_Model _SalesVoucherList_Model)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> CustAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_SalesVoucherList_Model.cust_name))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _SalesVoucherList_Model.cust_name;
                }
                Br_ID = Session["BranchId"].ToString();
                CustAccList = _SalesVoucher_IService.AutoGetCustAccList(Comp_ID, Acc_Name, Br_ID);
                List<CustAccList> _CustAccNameList = new List<CustAccList>();
                foreach (var dr in CustAccList)
                {
                    CustAccList _CustAccName = new CustAccList();
                    _CustAccName.cust_acc_id = dr.Key;
                    _CustAccName.cust_acc_name = dr.Value;
                    _CustAccNameList.Add(_CustAccName);
                }
                _SalesVoucherList_Model.CustAccNameList = _CustAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(CustAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


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
                DataTable dt = _SalesVoucher_IService.GetCurrList(CompID);
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
                DataSet result = _SalesVoucher_IService.GetAccCurrOnChange(acc_id, CompID, Br_ID, date);
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
        public ActionResult SalesVoucherSave(SalesVoucher_Model _SalesVoucher_Model, string Vou_No, string command)
        {
            try
            {
                /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                if (_SalesVoucher_Model.DeleteCommand == "Delete")
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
                        SalesVoucher_Model adddnew = new SalesVoucher_Model();
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
                        //    return RedirectToAction("SalesVoucherDetail", "SalesVoucher", adddnew);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("SalesVoucherDetail", "SalesVoucher", adddnew);
                        //}
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("SalesVoucherDetail", "SalesVoucher", NewModel);

                    case "Edit":

                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt = _SalesVoucher_Model.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_SalesVoucher_Model.Status == "A" || _SalesVoucher_Model.Status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("EditVou", new { VouNo = _SalesVoucher_Model.Vou_No, Voudt = _SalesVoucher_Model.Vou_Date, ListFilterData = _SalesVoucher_Model.ListFilterData1, WF_Status = _SalesVoucher_Model.WFStatus });
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        string FinalStatus = CheckSVForCancellation(_SalesVoucher_Model,_SalesVoucher_Model.Vou_No, _SalesVoucher_Model.Vou_Date.ToString());
                        if (FinalStatus == "Used" || FinalStatus == "Used1")
                        {
                            //Session["Message"] = FinalStatus;
                            _SalesVoucher_Model.Message= FinalStatus;
                            _SalesVoucher_Model.Message1= FinalStatus;
                            _SalesVoucher_Model.Command= "Refresh";
                            
                            TempData["ModelData"] = _SalesVoucher_Model;
                        }
                        else
                        {
                            //Session["TransType"] = "Update";
                            //Session["Command"] = command;
                            //Session["Message"] = null;
                            //Session["BtnName"] = "BtnEdit";
                            //Session["SalesVoucherNo"] = _SalesVoucher_Model.Vou_No;
                            //Session["SalesVoucherDate"] = _SalesVoucher_Model.Vou_Date.ToString();
                            _SalesVoucher_Model.TransType = "Update";
                            _SalesVoucher_Model.Command = command;
                            _SalesVoucher_Model.BtnName = "BtnEdit";
                            _SalesVoucher_Model.SalesVoucherNo = _SalesVoucher_Model.Vou_No;
                            _SalesVoucher_Model.SalesVoucherDate = _SalesVoucher_Model.Vou_Date;
                            TempData["ModelData"] = _SalesVoucher_Model;
                            UrlModel EditModel = new UrlModel();
                            EditModel.tp = "Update";
                            EditModel.Cmd = command;
                            EditModel.bt = "BtnEdit";
                            EditModel.SV_No = _SalesVoucher_Model.Vou_No;
                            EditModel.SV_DT = _SalesVoucher_Model.Vou_Date;
                            TempData["ListFilterData"] = _SalesVoucher_Model.ListFilterData1;
                            return RedirectToAction("SalesVoucherDetail", EditModel);
                        }
                        UrlModel Model = new UrlModel();
                        Model.bt = "D";
                        Model.SV_No = _SalesVoucher_Model.SalesVoucherNo;
                        Model.SV_DT = _SalesVoucher_Model.SalesVoucherDate;
                        Model.tp = "Update";
                        TempData["ListFilterData"] = _SalesVoucher_Model.ListFilterData1;
                        return RedirectToAction("SalesVoucherDetail", Model);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        Vou_No = _SalesVoucher_Model.Vou_No;
                        SalesVoucherDelete(_SalesVoucher_Model, command);
                        SalesVoucher_Model DeleteModel = new SalesVoucher_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete = new UrlModel();
                        Delete.Cmd = DeleteModel.Command;
                        Delete.tp = "Refresh";
                        Delete.bt = "BtnDelete";                      
                        TempData["ListFilterData"] = _SalesVoucher_Model.ListFilterData1;
                        return RedirectToAction("SalesVoucherDetail", Delete);


                    case "Save":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _SalesVoucher_Model.Vou_Date;
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
                            if (_SalesVoucher_Model.Vou_No == null)
                            {
                                _SalesVoucher_Model.Command = "Refresh";
                                _SalesVoucher_Model.TransType = "Refresh";
                                _SalesVoucher_Model.BtnName = "Refresh";
                                _SalesVoucher_Model.DocumentStatus = null;
                                TempData["ModelData"] = _SalesVoucher_Model;
                                return RedirectToAction("SalesVoucherDetail", "SalesVoucher", _SalesVoucher_Model);
                            }
                            else
                            {
                                return RedirectToAction("EditVou", new { VouNo = _SalesVoucher_Model.Vou_No, Voudt = _SalesVoucher_Model.Vou_Date, ListFilterData = _SalesVoucher_Model.ListFilterData1, WF_Status = _SalesVoucher_Model.WFStatus });

                            }
                        }
                        /*End to chk Financial year exist or not*/

                        // Session["Command"] = command;
                        SaveSalesVoucher(_SalesVoucher_Model);
                        if (_SalesVoucher_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_SalesVoucher_Model.Message == "N")
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
                            _SalesVoucher_Model.currList = _currList;
                            ViewBag.MenuPageName = getDocumentName();
                            GetAutoCompleteGLDetail(_SalesVoucher_Model);
                            ViewBag.VouDetails = ViewData["VouDetails"];
                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                            DataTable dtVouDetails = ViewData["VouDetails"] as DataTable;

                            decimal TotalDr = 0;
                            decimal TotalCr = 0;
                            for (int i = 0; i < dtVouDetails.Rows.Count; i++)
                            {/*Commented and modify by HIna on 30-10-2023 to changes for Base Amount instead of specific amount*/
                                //TotalDr = TotalDr + Convert.ToDecimal(dtVouDetails.Rows[i]["dr_amt_sp"]);
                                TotalDr = TotalDr + Convert.ToDecimal(dtVouDetails.Rows[i]["dr_amt_bs"]);
                            }
                            for (int i = 0; i < dtVouDetails.Rows.Count; i++)
                            {/*Commented and modify by HIna on 30-10-2023 to changes for Base Amount instead of specific amount*/
                                //TotalCr = TotalCr + Convert.ToDecimal(dtVouDetails.Rows[i]["cr_amt_sp"]);
                                TotalCr = TotalCr + Convert.ToDecimal(dtVouDetails.Rows[i]["cr_amt_bs"]);
                            }
                            ViewBag.TotalVouAmt = TotalDr;
                            ViewBag.DiffAmt = TotalDr - TotalCr;
                            ViewBag.CostCenterData = ViewData["CostCenter"];
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                            _SalesVoucher_Model.BtnName = "BtnAddNew";
                            _SalesVoucher_Model.Command = "Add";
                            _SalesVoucher_Model.Message = "N";
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.VBRoleList = GetRoleList();
                            DataTable dt1 = getSalesPersonList();
                            List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                            foreach (DataRow data in dt1.Rows)
                            {
                                SalePersonList _SlPrsnDetail = new SalePersonList();
                                _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                                _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                                _SlPrsnList.Add(_SlPrsnDetail);
                            }
                            _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                            _SalesVoucher_Model.SalePersonList = _SlPrsnList;
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/SalesVoucher/SalesVoucherDetail.cshtml", _SalesVoucher_Model);
                        }
                        else
                        {
                            //Session["SalesVoucherNo"] = Session["SalesVoucherNo"].ToString();
                            //Session["SalesVoucherDate"] = Session["SalesVoucherDate"].ToString();
                            TempData["ModelData"] = _SalesVoucher_Model;
                            UrlModel SaveModel = new UrlModel();
                            SaveModel.bt = _SalesVoucher_Model.BtnName;
                            SaveModel.SV_No = _SalesVoucher_Model.SalesVoucherNo;
                            SaveModel.SV_DT = _SalesVoucher_Model.SalesVoucherDate;
                            SaveModel.tp = _SalesVoucher_Model.TransType;
                            TempData["ListFilterData"] = _SalesVoucher_Model.ListFilterData1;
                            return RedirectToAction("SalesVoucherDetail", SaveModel);
                        }

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _SalesVoucher_Model.Vou_Date;

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
                            return RedirectToAction("EditVou", new { VouNo = _SalesVoucher_Model.Vou_No, Voudt = _SalesVoucher_Model.Vou_Date, ListFilterData = _SalesVoucher_Model.ListFilterData1, WF_Status = _SalesVoucher_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/


                        // Session["Command"] = command;
                        Vou_No = _SalesVoucher_Model.Vou_No;
                        //Session["SalesVoucherNo"] = Vou_No;
                        //Session["SalesVoucherDate"] = _SalesVoucher_Model.Vou_Date;
                        SalesVoucherApprove(_SalesVoucher_Model,_SalesVoucher_Model.Vou_No, _SalesVoucher_Model.Vou_Date,"","", "","","");
                        TempData["ModelData"] = _SalesVoucher_Model;
                        UrlModel Approve = new UrlModel();
                        Approve.tp = "Update";
                        Approve.SV_No = _SalesVoucher_Model.SalesVoucherNo;
                        Approve.SV_DT = _SalesVoucher_Model.SalesVoucherDate;
                        Approve.bt = "BtnEdit";
                        TempData["ListFilterData"] = _SalesVoucher_Model.ListFilterData1;
                        return RedirectToAction("SalesVoucherDetail", Approve);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        // Session["DocumentStatus"] = 'D';
                        SalesVoucher_Model RefreshModel = new SalesVoucher_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _SalesVoucher_Model.ListFilterData1;
                        return RedirectToAction("SalesVoucherDetail", refesh);

                    case "Print":
                        return GenratePdfFile(_SalesVoucher_Model);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        SalesVoucherList_Model _Backtolist = new SalesVoucherList_Model();
                        _Backtolist.WF_Status = _SalesVoucher_Model.WF_Status1;
                        TempData["ListFilterData"] = _SalesVoucher_Model.ListFilterData1;
                        return RedirectToAction("SalesVoucher", "SalesVoucher", _Backtolist);

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
        public ActionResult SaveSalesVoucher(SalesVoucher_Model _SalesVoucher_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (_SalesVoucher_Model.CancelFlag == false)
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
                    DataTable SalesVoucherHeader = new DataTable();
                    DataTable SalesVoucherGLDetails = new DataTable();
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
                    if (_SalesVoucher_Model.Vou_No != null)
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
                    dtrowHeader["vou_type"] = "SV";
                    dtrowHeader["vou_no"] = _SalesVoucher_Model.Vou_No;
                    dtrowHeader["vou_dt"] = _SalesVoucher_Model.Vou_Date;
                    dtrowHeader["src_doc"] = "D";
                    dtrowHeader["src_doc_no"] = _SalesVoucher_Model.Bill_No;
                    dtrowHeader["src_doc_dt"] = _SalesVoucher_Model.Bill_Date;
                    dtrowHeader["vou_amt"] = _SalesVoucher_Model.Vou_amount;
                    dtrowHeader["remarks"] = _SalesVoucher_Model.Remarks;
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
                    SalesVoucherHeader = dtheader;

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

                    JArray jObject = JArray.Parse(_SalesVoucher_Model.GlAccountDetails);

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
                    SalesVoucherGLDetails = dtAccount;
                    ViewData["VouDetails"] = DtVouDetails(jObject);

                    /*--------------Cost Center Section Start-----------------------*/

                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));

                    JArray JAObj = JArray.Parse(_SalesVoucher_Model.CC_DetailList);
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
                    DataTable SVAttachments = new DataTable();
                    DataTable svdtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as Sale_Vocher_Model;
                    TempData["IMGDATA"] = null;
                    if (_SalesVoucher_Model.attatchmentdetail != null)
                    {

                        //if (Session["AttachMentDetailItmStp"] != null)
                        //{
                        //    svdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                        //}
                        if (attachData != null)
                        {
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                svdtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                svdtAttachment.Columns.Add("id", typeof(string));
                                svdtAttachment.Columns.Add("file_name", typeof(string));
                                svdtAttachment.Columns.Add("file_path", typeof(string));
                                svdtAttachment.Columns.Add("file_def", typeof(char));
                                svdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_SalesVoucher_Model.AttachMentDetailItmStp != null)
                            {
                                svdtAttachment = _SalesVoucher_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                svdtAttachment.Columns.Add("id", typeof(string));
                                svdtAttachment.Columns.Add("file_name", typeof(string));
                                svdtAttachment.Columns.Add("file_path", typeof(string));
                                svdtAttachment.Columns.Add("file_def", typeof(char));
                                svdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_SalesVoucher_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in svdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = svdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_SalesVoucher_Model.Vou_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = _SalesVoucher_Model.Vou_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                svdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }

                        //if (Session["TransType"].ToString() == "Update")
                        if (_SalesVoucher_Model.TransType == "Update")
                        {
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string SV_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_SalesVoucher_Model.Vou_No).ToString()))
                                {
                                    SV_CODE = (_SalesVoucher_Model.Vou_No).ToString();

                                }
                                else
                                {
                                    SV_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + Br_ID + SV_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in svdtAttachment.Rows)
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
                        SVAttachments = svdtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/

                    SaveMessage = _SalesVoucher_IService.InsertSalesVoucherDetail(SalesVoucherHeader, SalesVoucherGLDetails, SVAttachments, CRCostCenterDetails, _SalesVoucher_Model.SalePerson);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 25-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //_BankPayment_Model.Message = "Financial Year not Exist";
                        _SalesVoucher_Model.BtnName = "Refresh";
                        _SalesVoucher_Model.Command = "Refresh";
                        _SalesVoucher_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;
                        return RedirectToAction("SalesVoucherDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //_BankPayment_Model.Message = "Financial Book Closing";
                        _SalesVoucher_Model.BtnName = "Refresh";
                        _SalesVoucher_Model.Command = "Refresh";
                        _SalesVoucher_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;

                        return RedirectToAction("SalesVoucherDetail");
                    }
                    else
                    {

                        if (SaveMessage == "N")
                        {
                            _SalesVoucher_Model.Message = "N";
                            return RedirectToAction("SalesVoucherDetail");
                        }
                        else
                        {
                            string SalesVoucherNo = SaveMessage.Split(',')[1].Trim();
                            string SV_Number = SalesVoucherNo.Replace("/", "");
                            string Message = SaveMessage.Split(',')[0].Trim();
                            string SalesVoucherDate = SaveMessage.Split(',')[2].Trim();
                            if (Message == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message.Replace("_", " ") + " " + SalesVoucherNo + " in " + PageName;//SalesVoucherNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _SalesVoucher_Model.Message = Message.Split(',')[0].Replace("_", "");
                                return RedirectToAction("SalesVoucherDetail");
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
                                comCont.ResetImageLocation(CompID, Br_ID, guid, PageName, SV_Number, _SalesVoucher_Model.TransType, SVAttachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{
                                //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Br_ID + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in SVAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = CompID + Br_ID + SV_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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
                                //Session["SalesVoucherNo"] = SalesVoucherNo;
                                //Session["SalesVoucherDate"] = SalesVoucherDate;
                                //Session["TransType"] = "Update";
                                //Session["AppStatus"] = 'D';
                                //Session["BtnName"] = "BtnToDetailPage";
                                _SalesVoucher_Model.Message = "Save";
                            _SalesVoucher_Model.SalesVoucherNo = SalesVoucherNo;
                            _SalesVoucher_Model.SalesVoucherDate = SalesVoucherDate;
                            _SalesVoucher_Model.TransType = "Update";
                            _SalesVoucher_Model.BtnName = "BtnToDetailPage";
                            return RedirectToAction("SalesVoucherDetail");
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
                    string FinalStatus = CheckSVForCancellation(_SalesVoucher_Model, _SalesVoucher_Model.Vou_No, _SalesVoucher_Model.Vou_Date.ToString());
                    if (FinalStatus == "Used" || FinalStatus == "Used1")
                    {
                        _SalesVoucher_Model.Message = FinalStatus;
                        _SalesVoucher_Model.Message1 = FinalStatus;
                        TempData["ModelData"] = _SalesVoucher_Model;
                    }
                    else
                    {
                        _SalesVoucher_Model.Create_by = UserID;
                        string br_id = Session["BranchId"].ToString();
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        DataSet SaveMessage1 = _SalesVoucher_IService.SalesVoucherCancel(_SalesVoucher_Model, CompID, br_id, mac_id);
                        _SalesVoucher_Model.Message = "Cancelled";
                        _SalesVoucher_Model.SalesVoucherNo = _SalesVoucher_Model.Vou_No;
                        _SalesVoucher_Model.SalesVoucherDate = _SalesVoucher_Model.Vou_Date;
                        _SalesVoucher_Model.TransType = "Update";
                        _SalesVoucher_Model.BtnName = "Refresh";
                    }
                    
                    return RedirectToAction("SalesVoucherDetail", _SalesVoucher_Model);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_SalesVoucher_Model.TransType == "Save")
                    {
                        string Guid = "";
                        if (_SalesVoucher_Model.Guid != null)
                        {
                            Guid = _SalesVoucher_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + Br_ID, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                throw ex;
            }

        }
        public DataTable DtVouDetails(JArray jObject)
        {
            DataTable dtAccount = new DataTable();

            dtAccount.Columns.Add("acc_id", typeof(string));
            dtAccount.Columns.Add("cust_acc_id", typeof(string));
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
                dtrowLines["cust_acc_id"] = jObject[i]["cust_acc_id"].ToString();
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
        public ActionResult GetCustAccIDDetail(string CustAccID,string VouDate)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _SalesVoucher_IService.GetCustAccIDDetail(CompID, BrchID, CustAccID, VouDate);

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
        public void GetStatusList(SalesVoucherList_Model _SalesVoucherList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _SalesVoucherList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<VouList> GetSalesVoucherListAll(SalesVoucherList_Model _SalesVoucherList_Model)
        {
            try
            {
                _SalesVoucherList = new List<VouList>();
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
                if(_SalesVoucherList_Model.WF_Status!= null)
                {
                    wfstatus = _SalesVoucherList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string VouType = "SV";
                DataSet Dtdata = new DataSet();
                Dtdata = _SalesVoucher_IService.GetSalesVoucherListAll(Convert.ToInt32(_SalesVoucherList_Model.curr), _SalesVoucherList_Model.cust_id, _SalesVoucherList_Model.VouFromDate, _SalesVoucherList_Model.VouToDate, _SalesVoucherList_Model.Status, CompID, Br_ID, VouType, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    _SalesVoucherList_Model.FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (Dtdata.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in Dtdata.Tables[0].Rows)
                    {
                        VouList _VouList = new VouList();
                        _VouList.cust_name = dr["cust_name"].ToString();
                        _VouList.VouNumber = dr["vou_no"].ToString();
                        _VouList.VouDate = dr["vou_dt"].ToString();
                        _VouList.hdVouDate = dr["vou_date"].ToString();
                        _VouList.BillNo = dr["bill_no"].ToString();
                        _VouList.BillDt = dr["bill_dt"].ToString();
                        _VouList.curr_logo = dr["curr_logo"].ToString();
                        _VouList.AmountinSp = dr["vou_dr_amt_sp"].ToString();
                        _VouList.AmountinBs = dr["vou_dr_amt_bs"].ToString();
                        _VouList.VouStatus = dr["vou_status"].ToString();
                        _VouList.CreatedON = dr["created_on"].ToString();
                        _VouList.ApprovedOn = dr["app_dt"].ToString();
                        _VouList.ModifiedOn = dr["mod_on"].ToString();
                        _VouList.create_by = dr["create_by"].ToString();
                        _VouList.mod_by = dr["mod_by"].ToString();
                        _VouList.app_by = dr["app_by"].ToString();

                        _SalesVoucherList.Add(_VouList);
                    }
                }
                return _SalesVoucherList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchSalesVoucherDetail(int curr, string cust_id, string Fromdate, string Todate, string Status, string CompID, string Br_ID)
        {
            _SalesVoucherList = new List<VouList>();
            SalesVoucherList_Model _SalesVoucherList_Model = new SalesVoucherList_Model();
            //Session["WF_status"] = "";
            _SalesVoucherList_Model.WF_Status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            string VouType = "SV";
            DataSet dt = new DataSet();
            dt = _SalesVoucher_IService.GetSalesVoucherListAll(curr, cust_id, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
            //Session["VouSearch"] = "Vou_Search";
            _SalesVoucherList_Model.VouSearch = "Vou_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    VouList _VouList = new VouList();
                    _VouList.cust_name = dr["cust_name"].ToString();
                    _VouList.VouNumber = dr["vou_no"].ToString();
                    _VouList.VouDate = dr["vou_dt"].ToString();
                    _VouList.hdVouDate = dr["vou_date"].ToString();
                    _VouList.BillNo = dr["bill_no"].ToString();
                    _VouList.BillDt = dr["bill_dt"].ToString();
                    _VouList.curr_logo = dr["curr_logo"].ToString();
                    _VouList.AmountinSp = dr["vou_dr_amt_sp"].ToString();
                    _VouList.AmountinBs = dr["vou_dr_amt_bs"].ToString();
                    _VouList.VouStatus = dr["vou_status"].ToString();
                    _VouList.CreatedON = dr["created_on"].ToString();
                    _VouList.ApprovedOn = dr["app_dt"].ToString();
                    _VouList.ModifiedOn = dr["mod_on"].ToString();
                    _VouList.create_by = dr["create_by"].ToString();
                    _VouList.mod_by = dr["mod_by"].ToString();
                    _VouList.app_by = dr["app_by"].ToString();

                    _SalesVoucherList.Add(_VouList);
                }
            }
            _SalesVoucherList_Model.VoucherList = _SalesVoucherList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSalesVoucherList.cshtml", _SalesVoucherList_Model);
        }

        private ActionResult SalesVoucherDelete(SalesVoucher_Model _SalesVoucher_Model, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string SVNo = _SalesVoucher_Model.Vou_No;
                string SaleVoucNumber = SVNo.Replace("/", "");

                string Message = _SalesVoucher_IService.SVDelete(_SalesVoucher_Model, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(SaleVoucNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, SaleVoucNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["SalesVoucherNo"] = "";
                //Session["SalesVoucherDate"] = "";
                //_SalesVoucher_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("SalesVoucherDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult SalesVoucherApprove(SalesVoucher_Model _SalesVoucher_Model,string VouNo, string VouDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1,string WF_Status1)
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
                string Message = _SalesVoucher_IService.SalesVoucherApprove(VouNo, VouDate, UserID, A_Status, A_Level, A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string SalesVoucherNo = Message.Split(',')[0].Trim();
                string SalesVoucherDate = Message.Split(',')[1].Trim();
                //Session["SalesVoucherNo"] = SalesVoucherNo;
                //Session["SalesVoucherDate"] = SalesVoucherDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel ApproveModel = new UrlModel();
                _SalesVoucher_Model.TransType = "Update";
                _SalesVoucher_Model.SalesVoucherNo = SalesVoucherNo;
                _SalesVoucher_Model.SalesVoucherDate = SalesVoucherDate;
                _SalesVoucher_Model.Message = "Approved";
                _SalesVoucher_Model.BtnName = "BtnEdit";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _SalesVoucher_Model.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _SalesVoucher_Model;

                ApproveModel.tp = "Update";
                ApproveModel.SV_No = SalesVoucherNo;
                ApproveModel.SV_DT = SalesVoucherDate;
                ApproveModel.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("SalesVoucherDetail", "SalesVoucher", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1,string ModelData)
        {
            // Session["Message"] = "";
            SalesVoucher_Model ToRefreshByJS = new SalesVoucher_Model();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.SalesVoucherNo = a[0].Trim();
            ToRefreshByJS.SalesVoucherDate = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnToDetailPage";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wf = a[2].Trim();
            }
            Model.bt = "D";
            Model.SV_No= ToRefreshByJS.SalesVoucherNo;
            Model.SV_DT = ToRefreshByJS.SalesVoucherDate;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("SalesVoucherDetail", Model);
        }
        public string CheckSVForCancellation(SalesVoucher_Model _SalesVoucher_Model,string DocNo, string DocDate)
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
                DataSet Deatils = _SalesVoucher_IService.CheckSVDetail(Comp_ID, Br_ID, DocNo, DocDate);
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
                    _SalesVoucher_Model.SalesVoucherNo = DocNo;
                    _SalesVoucher_Model.SalesVoucherDate = DocDate;
                    _SalesVoucher_Model.TransType = "Update";
                    _SalesVoucher_Model.BtnName = "BtnToDetailPage";                   
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
                Sale_Vocher_Model _attachmentModel = new Sale_Vocher_Model();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                //string TransType = "";
                //string SVCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["SalesVoucherNo"] != null)
                //{
                //    SVCode = Session["SalesVoucherNo"].ToString();
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
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");
                DataTable dt = other.Upload(PageName, TransType, CompID + Br_ID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
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

        public FileResult GenratePdfFile(SalesVoucher_Model _SalesVoucher_Model)
        {
            return File(GetPdfData(_SalesVoucher_Model.Vou_No, _SalesVoucher_Model.Vou_Date), "application/pdf", "SalesVoucher.pdf");
        }
        public byte[] GetPdfData(string SVNo, string SVDate)
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
                DataSet Deatils = _SalesVoucher_IService.GetGLVoucherPrintDeatils(CompID, Br_ID, SVNo, SVDate, "SV");
                ViewBag.PageName = "SV";
                ViewBag.Title = "Sales Voucher";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                ViewBag.DocumentMenuId = DocumentMenuId;

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
                    if (ViewBag.DocStatus == "C")/*Add by Nitesh on 10-09-2025*/
                    {
                         draftImage = Server.MapPath("~/Content/Images/cancelled.png");/*Add by Nitesh on 10-09-2025*/
                    }
                    else
                    {
                         draftImage = Server.MapPath("~/Content/Images/draft.png");/*Add by Hina sharma on 16-10-2024*/
                    }

                       
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);/*Add by Hina sharma on 16-10-2024*/
                                draftimg.SetAbsolutePosition(100, 0);
                                draftimg.ScaleAbsolute(650f, 650f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F" || ViewBag.DocStatus == "C")/*Add by Hina sharma on 16-10-2024*/
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
        public FileResult SalesVoucherExporttoExcelDt(int curr, string CustName, string Fromdate, string Todate, string Status)
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
                string VouType = "SV";
                DataTable dt = new DataTable();
                DataSet dt1 = _SalesVoucher_IService.GetSalesVoucherListAll(curr, CustName, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("GL Voucher Number", typeof(string));
                dt.Columns.Add("GL Voucher Date", typeof(string));
                dt.Columns.Add("Customer Name", typeof(string));
                dt.Columns.Add("Currency", typeof(string));
                dt.Columns.Add("Amount (In Specific)", typeof(decimal));
                dt.Columns.Add("Amount (In Base)", typeof(decimal));
                dt.Columns.Add("Source Document Number", typeof(string));
                dt.Columns.Add("Source Document Date", typeof(string));
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
                        dtrowLines["Customer Name"] = dr["cust_name"].ToString();
                        dtrowLines["Currency"] = dr["curr_logo"].ToString();
                        dtrowLines["Amount (In Specific)"] = dr["vou_dr_amt_sp"].ToString();
                        dtrowLines["Amount (In Base)"] = dr["vou_dr_amt_bs"].ToString();
                        dtrowLines["Source Document Number"] = dr["bill_no"].ToString();
                        dtrowLines["Source Document Date"] = dr["bill_dt"].ToString();
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
                return commonController.ExportDatatableToExcel("SalesVoucher", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public DataTable getSalesPersonList()
        {
            if (Session["compid"] != null)
                CompID = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DataTable dt = _SalesVoucher_IService.getSalesPersonList(CompID, Br_ID, UserID);
            return dt;
        }
    }
}