using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.BankPayment
{
    public class BankReceiptController : Controller
    {
        string CompID, language, FromDate = String.Empty;
        string Comp_ID, Br_ID, Language, title, UserID = String.Empty;
        string DocumentMenuId = "105104115110";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        BankPayment_IService _BankPayment_IService;
        DataTable dt;
        List<VouList> _VoucherList;
        public BankReceiptController(Common_IServices _Common_IServices, BankPayment_IService _BankPayment_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._BankPayment_IService = _BankPayment_IService;
        }
        // GET: ApplicationLayer/BankReceipt
        public ActionResult BankReceipt(BankPaymentList_Model _BankReceiptList_Model)
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
            DateTime dtnow = DateTime.Now;
            string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //string endDate = dtnow.ToString("yyyy-MM-dd");
            var other = new CommonController(_Common_IServices);
            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
            ViewBag.DocumentMenuId = DocumentMenuId;
            // BankPaymentList_Model _BankReceiptList_Model = new BankPaymentList_Model();
            GetStatusList(_BankReceiptList_Model);
            GetAutoCompleteBankDetail(_BankReceiptList_Model);
            List<Currlist> _currList = new List<Currlist>();
            dt = Getcurr();
            foreach (DataRow dr in dt.Rows)
            {
                Currlist _curr = new Currlist();
                _curr.curr_id = dr["curr_id"].ToString();
                _curr.curr_name = dr["curr_name"].ToString();
                _currList.Add(_curr);

            }
            _currList.Insert(0, new Currlist() { curr_id = "0", curr_name = "All" });
            _BankReceiptList_Model.curr_list = _currList;
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _BankReceiptList_Model.Src_Type = a[0].Trim();
                _BankReceiptList_Model.bank_id = a[1].Trim();
                _BankReceiptList_Model.VouFromDate = a[2].Trim();
                _BankReceiptList_Model.VouToDate = a[3].Trim();
                _BankReceiptList_Model.Status = a[4].Trim();
                _BankReceiptList_Model.Curr_nm = a[5].Trim();
                _BankReceiptList_Model.Instr_type = a[6].Trim();
                _BankReceiptList_Model.Reco_Status = a[7].Trim();
                if (_BankReceiptList_Model.Status == "0")
                {
                    _BankReceiptList_Model.Status = null;
                }
                _BankReceiptList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                /* Commented by Suraj Maurya on 25-07-2025 due to change in list bind method */
                //SearchBankPaymentDetail(_BankReceiptList_Model,_BankReceiptList_Model.Src_Type, _BankReceiptList_Model.bank_id, _BankReceiptList_Model.VouFromDate, _BankReceiptList_Model.VouToDate, _BankReceiptList_Model.Status, CompID, Br_ID, _BankReceiptList_Model.Curr_nm, _BankReceiptList_Model.Instr_type, _BankReceiptList_Model.Reco_Status);
            }
            else
            {
                _BankReceiptList_Model.VoucherList = GetBankPaymentListAll(_BankReceiptList_Model);
            }
            if (_BankReceiptList_Model.VouFromDate != null)
            {
                _BankReceiptList_Model.FromDate = _BankReceiptList_Model.VouFromDate;
            }
            ViewBag.VBRoleList = GetRoleList();
            ViewBag.MenuPageName = getDocumentName();
            _BankReceiptList_Model.Title = title;
            _BankReceiptList_Model.VouSearch = "0";
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/BankPayment/BankPaymentList.cshtml", _BankReceiptList_Model);
        }
        public ActionResult AddBankReceiptDetail()
        {
            BankPayment_Model AddNewModel = new BankPayment_Model();
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
            //    return RedirectToAction("BankReceipt", AddNewModel);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("BankReceipt", AddNewModel);
            //}
            ///*End to chk Financial year exist or not*/
            AddNewModel.Command = "Add";
            AddNewModel.TransType = "Save";
            AddNewModel.BtnName = "BtnAddNew";
            AddNewModel.DocumentStatus = "D";
            TempData["ModelData"] = AddNewModel;
            UrlModel _urlModel = new UrlModel();
            _urlModel.bt = "BtnAddNew";
            _urlModel.Cmd = "Add";
            _urlModel.tp = "Save";
            ViewBag.MenuPageName = getDocumentName();
            TempData["ListFilterData"] = null;
            return RedirectToAction("BankReceiptDetail", "BankReceipt", _urlModel);
        }
        public ActionResult BankReceiptDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
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
                var _BankReceipt_Model1 = TempData["ModelData"] as BankPayment_Model;
                if (_BankReceipt_Model1 != null)
                {
                    // BankPayment_Model _BankReceipt_Model1 = new BankPayment_Model();                   
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    GetAutoCompleteGLDetail(_BankReceipt_Model1);

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
                    _BankReceipt_Model1.currList = _currList;
                    _BankReceipt_Model1.UserID = UserID;

                    /*---code Add by Hina sharma on 08-08-2024 for pdc  to chng Voucher Date-----*/
                    dt = GetGLVoucherDtForPDC();
                    _BankReceipt_Model1.VouMin_Date = dt.Rows[0]["MinDate"].ToString();
                    _BankReceipt_Model1.VouMax_Date = dt.Rows[0]["MaxDate"].ToString();
                    _BankReceipt_Model1.ibt_acc = dt.Rows[0]["ibt_acc"].ToString();
                    /*---code End by Hina sharma on 09-08-2024 for pdc  to chng Voucher Date-----*/

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _BankReceipt_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_BankReceipt_Model1.TransType == "Update" || _BankReceipt_Model1.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["BankReceiptNo"].ToString();
                        string VouNo = _BankReceipt_Model1.BankReceiptNo;
                        string VouType = "BR";
                        //string VouDt = Session["BankReceiptDate"].ToString();
                        string VouDt = _BankReceipt_Model1.BankReceiptDate;
                        DataSet ds = _BankPayment_IService.GetBankPaymentDetail(VouType, VouNo, VouDt, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _BankReceipt_Model1.bank_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _BankReceipt_Model1.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _BankReceipt_Model1.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _BankReceipt_Model1.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        _BankReceipt_Model1.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _BankReceipt_Model1.src_doc_date = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _BankReceipt_Model1.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _BankReceipt_Model1.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _BankReceipt_Model1.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _BankReceipt_Model1.RecoStatus = ds.Tables[0].Rows[0]["reco_status"].ToString();
                        _BankReceipt_Model1.ReasonReturn = ds.Tables[0].Rows[0]["res_return"].ToString();/*Add By Hina on 29-08-2025 */
                        _BankReceipt_Model1.HdnPDCFlag = ds.Tables[0].Rows[0]["pdc"].ToString();/*Add By Hina on 09-08-2024 for PDC and Interbranch*/
                        _BankReceipt_Model1.HdnIntBrFlag = ds.Tables[0].Rows[0]["int_br"].ToString();
                        if (_BankReceipt_Model1.HdnPDCFlag == "Y")/*Add by HIna on 09-08-2024*/
                        {
                            _BankReceipt_Model1.PDCFlag = true;
                        }
                        else
                        {
                            _BankReceipt_Model1.PDCFlag = false;
                        }
                        if (_BankReceipt_Model1.HdnIntBrFlag == "Y")
                        {
                            _BankReceipt_Model1.IntrBrnchFlag = true;
                        }
                        else
                        {
                            _BankReceipt_Model1.IntrBrnchFlag = false;
                        }

                        //_BankReceipt_Model1.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);

                        if (_BankReceipt_Model1.RecoStatus.ToString() == "C ")
                        {
                            _BankReceipt_Model1.RecoStatus = "Cleared";
                        }
                        else if (_BankReceipt_Model1.RecoStatus.ToString() == "R ")
                        {
                            _BankReceipt_Model1.RecoStatus = "Returned";
                        }
                        else if (_BankReceipt_Model1.RecoStatus.ToString() == "U ")
                        {
                            _BankReceipt_Model1.RecoStatus = "Un-Cleared";
                        }
                        else
                        {
                            _BankReceipt_Model1.RecoStatus = "";
                        }

                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            _BankReceipt_Model1.Vou_amount = ds.Tables[7].Rows[0]["ClearBal"].ToString();
                        }
                        else
                        {
                            _BankReceipt_Model1.Vou_amount = Convert.ToDecimal(0).ToString(RateDigit);
                        }
                        //_BankReceipt_Model1.Vou_amount = ds.Tables[7].Rows[0]["ClosBL"].ToString();
                        _BankReceipt_Model1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _BankReceipt_Model1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _BankReceipt_Model1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _BankReceipt_Model1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _BankReceipt_Model1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _BankReceipt_Model1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _BankReceipt_Model1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _BankReceipt_Model1.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        _BankReceipt_Model1.ins_type = ds.Tables[0].Rows[0]["ins_type"].ToString();
                        _BankReceipt_Model1.ins_no = ds.Tables[0].Rows[0]["ins_no"].ToString();
                        if (ds.Tables[0].Rows[0]["ins_dt"].ToString() != "")
                        {
                            _BankReceipt_Model1.ins_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["ins_dt"].ToString()).ToString("yyyy-MM-dd");
                        }
                        _BankReceipt_Model1.ins_name = ds.Tables[0].Rows[0]["ins_name"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _BankReceipt_Model1.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _BankReceipt_Model1.DocumentStatus = Statuscode;
                        _BankReceipt_Model1.BillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (_BankReceipt_Model1.Status == "C")
                        {
                            _BankReceipt_Model1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _BankReceipt_Model1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _BankReceipt_Model1.BtnName = "Refresh";
                        }
                        else
                        {
                            _BankReceipt_Model1.CancelFlag = false;
                        }

                        _BankReceipt_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _BankReceipt_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _BankReceipt_Model1.Command != "Edit")
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
                                    _BankReceipt_Model1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                                        _BankReceipt_Model1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _BankReceipt_Model1.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _BankReceipt_Model1.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _BankReceipt_Model1.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        //_BankReceipt_Model1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _BankReceipt_Model1.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                                        _BankReceipt_Model1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _BankReceipt_Model1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                                    _BankReceipt_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _BankReceipt_Model1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _BankReceipt_Model1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                            /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                        _BankReceipt_Model1.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[6];
                        ViewBag.CostCenterData = ds.Tables[8];
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocumentStatus = _BankReceipt_Model1.DocumentStatus;
                        ViewBag.TransType = _BankReceipt_Model1.TransType;
                        ViewBag.Command = _BankReceipt_Model1.Command;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/BankPayment/BankPaymentDetail.cshtml", _BankReceipt_Model1);
                    }
                    else
                    {
                        _BankReceipt_Model1.DocumentStatus = "D";
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        _BankReceipt_Model1.Title = title;
                        ViewBag.TransType = _BankReceipt_Model1.TransType;
                        ViewBag.Command = _BankReceipt_Model1.Command;
                        ViewBag.DocumentStatus = _BankReceipt_Model1.DocumentStatus;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/BankPayment/BankPaymentDetail.cshtml", _BankReceipt_Model1);
                    }
                }
                else
                {
                    BankPayment_Model _BankReceipt_Model = new BankPayment_Model();
                    _BankReceipt_Model.UserID = UserID;

                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _BankReceipt_Model.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _BankReceipt_Model.BtnName = _urlModel.bt;
                        }
                        _BankReceipt_Model.BankReceiptNo = _urlModel.VN;
                        _BankReceipt_Model.BankReceiptDate = _urlModel.VDT;
                        _BankReceipt_Model.Command = _urlModel.Cmd;
                        _BankReceipt_Model.TransType = _urlModel.tp;
                        _BankReceipt_Model.WF_Status1 = _urlModel.wf;
                    }
                    /* Add by Hina on 27-02-2024 to Refresh Page*/
                    if (_BankReceipt_Model.TransType == null)
                    {
                        _BankReceipt_Model.BtnName = "Refresh";
                        _BankReceipt_Model.Command = "Refresh";
                        _BankReceipt_Model.TransType = "Refresh";

                    }
                    ///*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var VouDate = _BankReceipt_Model.BankReceiptDate;
                    //var VouDate = "";

                    //if (_BankReceipt_Model.BankReceiptDate != null)
                    //{
                    //    VouDate = _BankReceipt_Model.BankReceiptDate;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    _BankReceipt_Model.BankReceiptDate = CurrentDate;
                    //    _BankReceipt_Model.Vou_Date = CurrentDate;
                    //    VouDate = _BankReceipt_Model.BankReceiptDate;
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
                    GetAutoCompleteGLDetail(_BankReceipt_Model);

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
                    _BankReceipt_Model.currList = _currList;

                    /*---code Add by Hina sharma on 08-08-2024 for pdc  to chng Voucher Date-----*/
                    dt = GetGLVoucherDtForPDC();
                    _BankReceipt_Model.VouMin_Date = dt.Rows[0]["MinDate"].ToString();
                    _BankReceipt_Model.VouMax_Date = dt.Rows[0]["MaxDate"].ToString();
                    _BankReceipt_Model.ibt_acc = dt.Rows[0]["ibt_acc"].ToString();
                    /*---code End by Hina sharma on 09-08-2024 for pdc  to chng Voucher Date-----*/

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _BankReceipt_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_BankReceipt_Model.TransType == "Update" || _BankReceipt_Model.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["BankReceiptNo"].ToString();
                        string VouNo = _BankReceipt_Model.BankReceiptNo;
                        string VouType = "BR";
                        //string VouDt = Session["BankReceiptDate"].ToString();
                        string VouDt = _BankReceipt_Model.BankReceiptDate;
                        DataSet ds = _BankPayment_IService.GetBankPaymentDetail(VouType, VouNo, VouDt, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _BankReceipt_Model.bank_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _BankReceipt_Model.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _BankReceipt_Model.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _BankReceipt_Model.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        _BankReceipt_Model.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _BankReceipt_Model.src_doc_date = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _BankReceipt_Model.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _BankReceipt_Model.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _BankReceipt_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();

                        _BankReceipt_Model.RecoStatus = ds.Tables[0].Rows[0]["reco_status"].ToString();
                        _BankReceipt_Model.ReasonReturn = ds.Tables[0].Rows[0]["res_return"].ToString();/*Add By Hina on 29-08-2025 */
                        _BankReceipt_Model.HdnPDCFlag = ds.Tables[0].Rows[0]["pdc"].ToString();/*Add By Hina on 09-08-2024 for PDC and Interbranch*/
                        _BankReceipt_Model.HdnIntBrFlag = ds.Tables[0].Rows[0]["int_br"].ToString();
                        if (_BankReceipt_Model.HdnPDCFlag == "Y")/*Add by HIna on 09-08-2024*/
                        {
                            _BankReceipt_Model.PDCFlag = true;
                        }
                        else
                        {
                            _BankReceipt_Model.PDCFlag = false;
                        }
                        if (_BankReceipt_Model.HdnIntBrFlag == "Y")
                        {
                            _BankReceipt_Model.IntrBrnchFlag = true;
                        }
                        else
                        {
                            _BankReceipt_Model.IntrBrnchFlag = false;
                        }

                        if (_BankReceipt_Model.RecoStatus.ToString() == "C ")
                        {
                            _BankReceipt_Model.RecoStatus = "Cleared";
                        }
                        else if (_BankReceipt_Model.RecoStatus.ToString() == "R ")
                        {
                            _BankReceipt_Model.RecoStatus = "Returned";
                        }
                        else if (_BankReceipt_Model.RecoStatus.ToString() == "U ")
                        {
                            _BankReceipt_Model.RecoStatus = "Un-Cleared";
                        }
                        else
                        {
                            _BankReceipt_Model.RecoStatus = "";
                        }
                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            _BankReceipt_Model.Vou_amount = ds.Tables[7].Rows[0]["ClearBal"].ToString();
                        }
                        else
                        {
                            _BankReceipt_Model.Vou_amount = Convert.ToDecimal(0).ToString(RateDigit);
                        }
                        //_BankReceipt_Model.Vou_amount = ds.Tables[7].Rows[0]["ClosBL"].ToString();
                        _BankReceipt_Model.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _BankReceipt_Model.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _BankReceipt_Model.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _BankReceipt_Model.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _BankReceipt_Model.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _BankReceipt_Model.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _BankReceipt_Model.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _BankReceipt_Model.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        _BankReceipt_Model.ins_type = ds.Tables[0].Rows[0]["ins_type"].ToString();
                        _BankReceipt_Model.ins_no = ds.Tables[0].Rows[0]["ins_no"].ToString();
                        if (ds.Tables[0].Rows[0]["ins_dt"].ToString() != "")
                        {
                            _BankReceipt_Model.ins_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["ins_dt"].ToString()).ToString("yyyy-MM-dd");
                        }
                        _BankReceipt_Model.ins_name = ds.Tables[0].Rows[0]["ins_name"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _BankReceipt_Model.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _BankReceipt_Model.DocumentStatus = Statuscode;
                        _BankReceipt_Model.BillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (_BankReceipt_Model.Status == "C")
                        {
                            _BankReceipt_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _BankReceipt_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _BankReceipt_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _BankReceipt_Model.CancelFlag = false;
                        }

                        _BankReceipt_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _BankReceipt_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _BankReceipt_Model.Command != "Edit")
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
                                    _BankReceipt_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                                        _BankReceipt_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _BankReceipt_Model.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _BankReceipt_Model.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _BankReceipt_Model.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        // _BankReceipt_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _BankReceipt_Model.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                                        _BankReceipt_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _BankReceipt_Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                                    _BankReceipt_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _BankReceipt_Model.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _BankReceipt_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                            /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                        _BankReceipt_Model.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[6];
                        ViewBag.CostCenterData = ds.Tables[8];
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.TransType = _BankReceipt_Model.TransType;
                        ViewBag.DocumentStatus = _BankReceipt_Model.DocumentStatus;
                        ViewBag.Command = _BankReceipt_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/BankPayment/BankPaymentDetail.cshtml", _BankReceipt_Model);
                    }
                    else
                    {
                        _BankReceipt_Model.DocumentStatus = "D";
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        _BankReceipt_Model.Title = title;
                        ViewBag.TransType = _BankReceipt_Model.TransType;
                        ViewBag.Command = _BankReceipt_Model.Command;
                        ViewBag.DocumentStatus = _BankReceipt_Model.DocumentStatus;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/BankPayment/BankPaymentDetail.cshtml", _BankReceipt_Model);
                    }
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
                //throw ex;
            }

        }
        public ActionResult EditVou(string VouNo, string Voudt, string ListFilterData, string WF_Status)
        { 
            /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
            BankPayment_Model dblclick = new BankPayment_Model();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
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
            //Session["BankReceiptNo"] = VouNo;
            //Session["BankReceiptDate"] = Voudt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            //BankPayment_Model dblclick = new BankPayment_Model();
            UrlModel _url = new UrlModel();
            dblclick.Command = "Update";
            dblclick.BankReceiptNo = VouNo;
            dblclick.BankReceiptDate = Voudt;
            dblclick.TransType = "Update";
            dblclick.UserID = UserID;
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
            _url.VN = VouNo;
            _url.VDT = Voudt;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("BankReceiptDetail", "BankReceipt", _url);
        }
        public ActionResult GetBankReceiptList(string docid, string status)
        {

            //Session["WF_status"] = status;
            BankPaymentList_Model Dashbord = new BankPaymentList_Model();
            Dashbord.WF_Status = status;
            return RedirectToAction("BankReceipt", Dashbord);
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        public ActionResult GetAutoCompleteGLDetail(BankPayment_Model _BankReceipt_Model)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> BankAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_BankReceipt_Model.BankName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _BankReceipt_Model.BankName;
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                // Br_ID = Session["BranchId"].ToString();
                BankAccList = _BankPayment_IService.AutoGetBankAccList(Comp_ID, Acc_Name, Br_ID);

                List<BankAccName> _BankAccNameList = new List<BankAccName>();
                foreach (var dr in BankAccList)
                {
                    BankAccName _BankAccName = new BankAccName();
                    _BankAccName.bank_acc_id = dr.Key;
                    _BankAccName.bank_acc_name = dr.Value;
                    _BankAccNameList.Add(_BankAccName);
                }
                _BankReceipt_Model.BankAccNameList = _BankAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(BankAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetAutoCompleteBankDetail(BankPaymentList_Model _BankReceiptList_Model)
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
                if (string.IsNullOrEmpty(_BankReceiptList_Model.bank_name))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _BankReceiptList_Model.bank_name;
                }
                Br_ID = Session["BranchId"].ToString();
                BankAccList = _BankPayment_IService.AutoGetBankAccList(Comp_ID, Acc_Name, Br_ID);

                List<BankAccList> _BankAccNameList = new List<BankAccList>();
                foreach (var dr in BankAccList)
                {
                    BankAccList _BankAccName = new BankAccList();
                    _BankAccName.bank_acc_id = dr.Key;
                    _BankAccName.bank_acc_name = dr.Value;
                    _BankAccNameList.Add(_BankAccName);
                }
                _BankReceiptList_Model.BankAccNameList = _BankAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(BankAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


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
                DataTable dt = _BankPayment_IService.GetCurrList(CompID);
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
                Br_ID = Session["BranchId"].ToString();
                DataSet result = _BankPayment_IService.GetAccCurrOnChange(acc_id, CompID, Br_ID, Date);
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
        public ActionResult BankReceiptSave(BankPayment_Model _BankReceipt_Model, string Vou_No, string command)
        {
            try
            {
                /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                if (_BankReceipt_Model.DeleteCommand == "Delete")
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
                        BankPayment_Model adddnew = new BankPayment_Model();
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
                        //    return RedirectToAction("BankReceiptDetail", "BankReceipt", adddnew);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("BankReceiptDetail", "BankReceipt", adddnew);
                        //}
                        ///*End to chk Financial year exist or not*/
                        return RedirectToAction("BankReceiptDetail", "BankReceipt", NewModel);

                    case "Edit":
                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt = _BankReceipt_Model.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_BankReceipt_Model.Status == "A" || _BankReceipt_Model.Status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("EditVou", new { VouNo = _BankReceipt_Model.Vou_No, Voudt = _BankReceipt_Model.Vou_Date, ListFilterData = _BankReceipt_Model.ListFilterData1, WF_Status = _BankReceipt_Model.WFStatus });
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        if (CheckAdvancePayment(_BankReceipt_Model, _BankReceipt_Model.Vou_No, _BankReceipt_Model.Vou_Date) == "Used")
                        {
                            // Session["Message"] = "Used";
                            _BankReceipt_Model.Message = "Used";
                            _BankReceipt_Model.Command = "Refresh";
                            TempData["ModelData"] = _BankReceipt_Model;
                        }
                        else
                        {
                            _BankReceipt_Model.TransType = "Update";
                            _BankReceipt_Model.Command = command;
                            _BankReceipt_Model.BtnName = "BtnEdit";
                            _BankReceipt_Model.BankReceiptNo = _BankReceipt_Model.Vou_No;
                            _BankReceipt_Model.BankReceiptDate = _BankReceipt_Model.Vou_Date;
                            TempData["ModelData"] = _BankReceipt_Model;
                            UrlModel EditModel = new UrlModel();
                            EditModel.tp = "Update";
                            EditModel.Cmd = command;
                            EditModel.bt = "BtnEdit";
                            EditModel.VN = _BankReceipt_Model.Vou_No;
                            EditModel.VDT = _BankReceipt_Model.Vou_Date;
                            TempData["ListFilterData"] = _BankReceipt_Model.ListFilterData1;
                            return RedirectToAction("BankReceiptDetail", EditModel);
                        }
                        UrlModel Edit_Model = new UrlModel();
                        Edit_Model.tp = "Update";
                        Edit_Model.bt = "D";
                        Edit_Model.VN = _BankReceipt_Model.Vou_No;
                        Edit_Model.VDT = _BankReceipt_Model.Vou_Date;
                        TempData["ListFilterData"] = _BankReceipt_Model.ListFilterData1;
                        return RedirectToAction("BankReceiptDetail", Edit_Model);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        _BankReceipt_Model.Command = command;
                        Vou_No = _BankReceipt_Model.Vou_No;
                        BankPaymentDelete(_BankReceipt_Model, command);
                        BankPayment_Model DeleteModel = new BankPayment_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete = new UrlModel();
                        Delete.Cmd = DeleteModel.Command;
                        Delete.tp = "Refresh";
                        Delete.bt = "BtnDelete";
                        TempData["ListFilterData"] = _BankReceipt_Model.ListFilterData1;
                        return RedirectToAction("BankReceiptDetail", Delete);


                    case "Save":
                        /*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _BankReceipt_Model.Vou_Date;
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
                            if (_BankReceipt_Model.Vou_No == null)
                            {
                                _BankReceipt_Model.Command = "Refresh";
                                _BankReceipt_Model.TransType = "Refresh";
                                _BankReceipt_Model.BtnName = "Refresh";
                                _BankReceipt_Model.DocumentStatus = null;
                                TempData["ModelData"] = _BankReceipt_Model;
                                return RedirectToAction("BankReceiptDetail", "BankReceipt", _BankReceipt_Model);
                            }
                            else
                            {
                                return RedirectToAction("EditVou", new { VouNo = _BankReceipt_Model.Vou_No, Voudt = _BankReceipt_Model.Vou_Date, ListFilterData = _BankReceipt_Model.ListFilterData1, WF_Status = _BankReceipt_Model.WFStatus });
                            }

                        }
                        /*End to chk Financial year exist or not*/
                        // Session["Command"] = command;
                        _BankReceipt_Model.Command = command;
                        UrlModel _urlModel = new UrlModel();
                        SaveBankReceipt(_BankReceipt_Model);
                        if (_BankReceipt_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_BankReceipt_Model.Message == "N")
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
                            _BankReceipt_Model.currList = _currList;
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
                            GetAutoCompleteGLDetail(_BankReceipt_Model);
                            ViewBag.CostCenterData = ViewData["CostCenter"];
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                            _BankReceipt_Model.BtnName = "BtnAddNew";
                            _BankReceipt_Model.Command = "Add";
                            _BankReceipt_Model.Message = "N";
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/BankPayment/BankPaymentDetail.cshtml", _BankReceipt_Model);
                        }
                        else
                        {
                            TempData["ModelData"] = _BankReceipt_Model;
                            _urlModel.bt = _BankReceipt_Model.BtnName;
                            _urlModel.Cmd = _BankReceipt_Model.Command;
                            _urlModel.VN = _BankReceipt_Model.BankReceiptNo;
                            _urlModel.VDT = _BankReceipt_Model.BankReceiptDate;
                            _urlModel.tp = _BankReceipt_Model.TransType;
                            TempData["ListFilterData"] = _BankReceipt_Model.ListFilterData1;
                            return RedirectToAction("BankReceiptDetail", _urlModel);
                        }

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _BankReceipt_Model.Vou_Date;

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
                            return RedirectToAction("EditVou", new { VouNo = _BankReceipt_Model.Vou_No, Voudt = _BankReceipt_Model.Vou_Date, ListFilterData = _BankReceipt_Model.ListFilterData1, WF_Status = _BankReceipt_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        // Session["Command"] = command;
                        Vou_No = _BankReceipt_Model.Vou_No;
                        //Session["BankReceiptNo"] = Vou_No;
                        //Session["BankReceiptDate"] = _BankReceipt_Model.Vou_Date;
                        BankReceiptApprove(_BankReceipt_Model, _BankReceipt_Model.Vou_No, _BankReceipt_Model.Vou_Date, "", "", "", "", "", _BankReceipt_Model.HdnIntBrNurr_BR);
                        TempData["ModelData"] = _BankReceipt_Model;
                        UrlModel urlref = new UrlModel();
                        urlref.tp = "Update";
                        urlref.VN = _BankReceipt_Model.BankReceiptNo;
                        urlref.VDT = _BankReceipt_Model.BankReceiptDate;
                        urlref.bt = "BtnEdit";
                        if (_BankReceipt_Model.WF_Status1 != null)
                        {
                            urlref.wf = _BankReceipt_Model.WF_Status1;
                        }
                        TempData["ListFilterData"] = _BankReceipt_Model.ListFilterData1;
                        return RedirectToAction("BankReceiptDetail", urlref);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        //Session["DocumentStatus"] = 'D';
                        BankPayment_Model RefreshModel = new BankPayment_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _BankReceipt_Model.ListFilterData1;
                        return RedirectToAction("BankReceiptDetail", refesh);

                    case "Print":
                        return GenratePdfFile(_BankReceipt_Model);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        BankPaymentList_Model _BankReceiptList_Model = new BankPaymentList_Model();
                        _BankReceiptList_Model.WF_Status = _BankReceipt_Model.WF_Status1;
                        TempData["ListFilterData"] = _BankReceipt_Model.ListFilterData1;
                        return RedirectToAction("BankReceipt", "BankReceipt", _BankReceiptList_Model);

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
        public ActionResult SaveBankReceipt(BankPayment_Model _BankReceipt_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                string PDC = "N";/*Add By Hina on 09-08-2024 for PDC and Interbranch*/
                string InterBrch = "N";
                if (_BankReceipt_Model.CancelFlag == false)
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
                    if (_BankReceipt_Model.HdnPDCFlag == null)/*Add By Hina on 09-08-2024 for PDC and Interbranch*/
                    {
                        PDC = "N";
                    }
                    else
                    {
                        PDC = _BankReceipt_Model.HdnPDCFlag;
                    }
                    if (_BankReceipt_Model.HdnIntBrFlag == null)
                    {
                        InterBrch = "N";
                    }
                    else
                    {
                        InterBrch = _BankReceipt_Model.HdnIntBrFlag;
                    }
                    DataTable BankReceiptHeader = new DataTable();
                    DataTable BankReceiptItemDetails = new DataTable();
                    DataTable BankReceiptBillAdjDetail = new DataTable();
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
                    if (_BankReceipt_Model.Vou_No != null)
                    {
                        dtrowHeader["TransType"] = "Update";
                    }
                    else
                    {
                        dtrowHeader["TransType"] = "Save";
                    }
                    //dtrowHeader["TransType"] = Session["TransType"].ToString();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    dtrowHeader["comp_id"] = Session["CompId"].ToString();
                    dtrowHeader["br_id"] = Session["BranchId"].ToString();
                    dtrowHeader["user_id"] = Session["UserId"].ToString();
                    dtrowHeader["vou_type"] = "BR";
                    dtrowHeader["vou_no"] = _BankReceipt_Model.Vou_No;
                    dtrowHeader["vou_dt"] = _BankReceipt_Model.Vou_Date;
                    dtrowHeader["src_doc"] = "D";
                    dtrowHeader["src_doc_no"] = null;
                    dtrowHeader["src_doc_dt"] = null;
                    dtrowHeader["vou_amt"] = "0";
                    dtrowHeader["remarks"] = _BankReceipt_Model.Remarks;
                    dtrowHeader["vou_status"] = "D";
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrowHeader["mac_id"] = mac_id;

                    dtrowHeader["ins_type"] = _BankReceipt_Model.ins_type;
                    dtrowHeader["ins_no"] = _BankReceipt_Model.ins_no;
                    dtrowHeader["ins_dt"] = _BankReceipt_Model.ins_dt;
                    dtrowHeader["ins_name"] = _BankReceipt_Model.ins_name;


                    dtheader.Rows.Add(dtrowHeader);
                    BankReceiptHeader = dtheader;

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
                    dtAccount.Columns.Add("int_br_id", typeof(int));
                    JArray jObject = JArray.Parse(_BankReceipt_Model.GlAccountDetails);

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
                        dtrowLines["int_br_id"] = IsNull(jObject[i]["GlBrID"].ToString(), "0")=="0" ? Session["BranchId"].ToString() : jObject[i]["GlBrID"].ToString();
                        dtAccount.Rows.Add(dtrowLines);
                    }
                    BankReceiptItemDetails = dtAccount;
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
                    dtBillAdjDetail.Columns.Add("int_br_id", typeof(int));

                    JArray BObject = JArray.Parse(_BankReceipt_Model.BillAdjdetails);

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
                        dtrowLines["int_br_id"] = IsNull(BObject[i]["GlBrID"].ToString(), "0") == "0" ? Session["BranchId"].ToString() : BObject[i]["GlBrID"].ToString();
                        dtBillAdjDetail.Rows.Add(dtrowLines);
                    }
                    BankReceiptBillAdjDetail = dtBillAdjDetail;
                    ViewData["BillAdj"] = dtBillAdj(BObject);

                    /**----------------Cost Center Section--------------------*/
                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));
                    CC_Details.Columns.Add("int_br_id", typeof(int));



                    JArray JAObj = JArray.Parse(_BankReceipt_Model.CC_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = CC_Details.NewRow();

                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                        dtrowLines["int_br_id"] = IsNull(JAObj[i]["int_br_id"].ToString(), "0") == "0" ? Session["BranchId"].ToString() : JAObj[i]["int_br_id"].ToString();

                        CC_Details.Rows.Add(dtrowLines);
                    }
                    CRCostCenterDetails = CC_Details;
                    ViewData["CostCenter"] = dtCostCenter(JAObj);


                    /**----------------Cost Center Section END--------------------*/

                    /*-----------------Attachment Section Start------------------------*/
                    DataTable BRAttachments = new DataTable();
                    DataTable BRdtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as BankPaymentModel;
                    TempData["IMGDATA"] = null;
                    if (_BankReceipt_Model.attatchmentdetail != null)
                    {
                        if (attachData != null)
                        {
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                BRdtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                BRdtAttachment.Columns.Add("id", typeof(string));
                                BRdtAttachment.Columns.Add("file_name", typeof(string));
                                BRdtAttachment.Columns.Add("file_path", typeof(string));
                                BRdtAttachment.Columns.Add("file_def", typeof(char));
                                BRdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_BankReceipt_Model.AttachMentDetailItmStp != null)
                            {
                                BRdtAttachment = _BankReceipt_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                BRdtAttachment.Columns.Add("id", typeof(string));
                                BRdtAttachment.Columns.Add("file_name", typeof(string));
                                BRdtAttachment.Columns.Add("file_path", typeof(string));
                                BRdtAttachment.Columns.Add("file_def", typeof(char));
                                BRdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }


                        JArray jObject1 = JArray.Parse(_BankReceipt_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in BRdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = BRdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_BankReceipt_Model.Vou_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = _BankReceipt_Model.Vou_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                BRdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        // if (Session["TransType"].ToString() == "Update")
                        if (_BankReceipt_Model.TransType == "Update")
                        {
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string BR_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_BankReceipt_Model.Vou_No).ToString()))
                                {
                                    BR_CODE = (_BankReceipt_Model.Vou_No).ToString();

                                }
                                else
                                {
                                    BR_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + Br_ID + BR_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in BRdtAttachment.Rows)
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
                        BRAttachments = BRdtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/

                    SaveMessage = _BankPayment_IService.InsertBankPaymentDetail(BankReceiptHeader, BankReceiptItemDetails, BankReceiptBillAdjDetail, BRAttachments, CRCostCenterDetails, PDC, InterBrch);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 25-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //_BankPayment_Model.Message = "Financial Year not Exist";
                        _BankReceipt_Model.BtnName = "Refresh";
                        _BankReceipt_Model.Command = "Refresh";
                        _BankReceipt_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;
                        return RedirectToAction("BankReceiptDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //_BankPayment_Model.Message = "Financial Book Closing";
                        _BankReceipt_Model.BtnName = "Refresh";
                        _BankReceipt_Model.Command = "Refresh";
                        _BankReceipt_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;

                        return RedirectToAction("BankReceiptDetail");
                    }
                    else
                    {

                        if (SaveMessage == "N")
                        {
                            _BankReceipt_Model.Message = "N";
                            //_BankReceipt_Model.BtnName = "Refresh";
                            //_BankReceipt_Model.Command = "Add";
                            //TempData["ModelData"] = _BankReceipt_Model;
                            return RedirectToAction("BankReceiptDetail");
                        }
                        else
                        {
                            string BankReceiptNo = SaveMessage.Split(',')[1].Trim();
                            string BR_Number = BankReceiptNo.Replace("/", "");
                            string Message = SaveMessage.Split(',')[0].Trim();
                            string BankReceiptDate = SaveMessage.Split(',')[2].Trim();
                            if (Message == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message.Replace("_", " ") + " " + BankReceiptNo;//BankReceiptNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _BankReceipt_Model.Message = Message.Split(',')[0].Replace("_", "");
                                return RedirectToAction("BankReceiptDetail");
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
                                comCont.ResetImageLocation(CompID, Br_ID, guid, PageName, BR_Number, _BankReceipt_Model.TransType, BRAttachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{
                                //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Br_ID + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in BRAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = CompID + Br_ID + BR_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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
                                //Session["BankReceiptNo"] = BankReceiptNo;
                                //Session["BankReceiptDate"] = BankReceiptDate;
                                //Session["TransType"] = "Update";
                                //Session["AppStatus"] = 'D';
                                //Session["BtnName"] = "BtnToDetailPage";
                                _BankReceipt_Model.Message = "Save";
                            _BankReceipt_Model.Command = "Update";
                            _BankReceipt_Model.BankReceiptNo = BankReceiptNo;
                            _BankReceipt_Model.BankReceiptDate = BankReceiptDate;
                            _BankReceipt_Model.TransType = "Update";
                            _BankReceipt_Model.BtnName = "BtnToDetailPage";
                            return RedirectToAction("BankReceiptDetail");
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
                    if (CheckAdvancePayment(_BankReceipt_Model, _BankReceipt_Model.Vou_No, _BankReceipt_Model.Vou_Date) == "Used")
                    {
                        // Session["Message"] = "Used";
                        _BankReceipt_Model.Message = "Used";
                        _BankReceipt_Model.Command = "Refresh";
                        TempData["ModelData"] = _BankReceipt_Model;
                    }
                    else
                    {
                        _BankReceipt_Model.Create_by = UserID;
                        string br_id = Session["BranchId"].ToString();
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        DataSet SaveMessage1 = _BankPayment_IService.BankPaymentCancel(_BankReceipt_Model, CompID, br_id, mac_id);
                        try
                        {
                            // string fileName = "BR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "BankReceipt_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_BankReceipt_Model.Vou_No, _BankReceipt_Model.Vou_Date, fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _BankReceipt_Model.Vou_No, "C", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _BankReceipt_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _BankReceipt_Model.Message = _BankReceipt_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        //_BankReceipt_Model.Message = "Cancelled";
                        _BankReceipt_Model.Command = "Update";
                        _BankReceipt_Model.BankReceiptNo = _BankReceipt_Model.Vou_No;
                        _BankReceipt_Model.BankReceiptDate = _BankReceipt_Model.Vou_Date;
                        _BankReceipt_Model.TransType = "Update";
                        _BankReceipt_Model.BtnName = "Refresh";
                    }
                    
                    return RedirectToAction("BankReceiptDetail");
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
                    if (_BankReceipt_Model.TransType == "Save")
                    {
                        string Guid = "";
                        if (_BankReceipt_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _BankReceipt_Model.Guid;
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
        //public DataTable DtHeaderDetail()
        //{
        //    BankPayment_Model _BankReceipt_Model = new BankPayment_Model();
        //    DataTable dtheader = new DataTable();
        //    dtheader.Columns.Add("vou_dt", typeof(string));
        //    dtheader.Columns.Add("ins_type", typeof(string));
        //    dtheader.Columns.Add("ins_no", typeof(string));
        //    dtheader.Columns.Add("ins_dt", typeof(string));
        //    dtheader.Columns.Add("ins_name", typeof(string));
        //    DataRow dtrowHeader = dtheader.NewRow();



        //}
        public DataTable DtVouDetails(JArray jObject)
        {
            DataTable dtAccount = new DataTable();

            dtAccount.Columns.Add("acc_id", typeof(string));
            dtAccount.Columns.Add("acc_name", typeof(string));
            dtAccount.Columns.Add("acc_group_name", typeof(string));
            dtAccount.Columns.Add("acc_type", typeof(int));
            dtAccount.Columns.Add("curr_id", typeof(int));
            dtAccount.Columns.Add("conv_rate", typeof(string));
            dtAccount.Columns.Add("dr_amt_bs", typeof(decimal));
            dtAccount.Columns.Add("cr_amt_bs", typeof(decimal));
            dtAccount.Columns.Add("dr_amt_sp", typeof(string));
            dtAccount.Columns.Add("cr_amt_sp", typeof(string));
            dtAccount.Columns.Add("narr", typeof(string));

            //JArray jObject = JArray.Parse(RFQ_Model.Itemdetails);


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
        private string IsNull (string inValue,string outValue)
        {
            return string.IsNullOrEmpty(inValue) ? outValue : inValue;
        }
        public ActionResult GetBankAccIDDetail(string BankAccID)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _BankPayment_IService.GetBankAccIDDetail(CompID, BrchID, BankAccID);

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
        public void GetStatusList(BankPaymentList_Model _BankReceiptList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _BankReceiptList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<VouList> GetBankPaymentListAll(BankPaymentList_Model _BankReceiptList_Model)
        {
            try
            {
                _VoucherList = new List<VouList>();
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
                if (_BankReceiptList_Model.WF_Status != null)
                {
                    wfstatus = _BankReceiptList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string VouType = "BR";

                DataSet Dtdata = new DataSet();
                Dtdata = _BankPayment_IService.GetBankPaymentListAll(_BankReceiptList_Model.Src_Type, _BankReceiptList_Model.bank_id
                    , _BankReceiptList_Model.VouFromDate, _BankReceiptList_Model.VouToDate, _BankReceiptList_Model.Status
                    , CompID, Br_ID, VouType, wfstatus, UserID, DocumentMenuId, "", "", ""
                    , "0", "25", "", "SrNo", "ASC", "OnlyDateRange");
                if (Dtdata.Tables[0].Rows.Count > 0)
                {
                    _BankReceiptList_Model.FromDate = Dtdata.Tables[0].Rows[0]["finstrdate"].ToString();
                    _BankReceiptList_Model.FinalToDate = Dtdata.Tables[0].Rows[0]["finenddate"].ToString();/*add By hina on 09-08-2024 for show all transaction in future date also*/

                }
                //if (Dtdata.Tables[0].Rows.Count > 0)
                //{

                //    foreach (DataRow dr in Dtdata.Tables[0].Rows)
                //    {
                //        VouList _VouList = new VouList();
                //        _VouList.bank_name = dr["bank_name"].ToString();
                //        _VouList.VouNumber = dr["vou_no"].ToString();
                //        _VouList.VouDate = dr["vou_dt"].ToString();
                //        _VouList.hdVouDate = dr["vou_date"].ToString();
                //        _VouList.PDC = dr["pdc"].ToString();/*Add By Hina on 09-08-2024 for pdc and inter branch*/
                //        if (_VouList.PDC == "Y")
                //        {
                //            _VouList.HDNListPDC = true;
                //        }
                //        else
                //        {
                //            _VouList.HDNListPDC = false;
                //        }
                //        _VouList.InterBrch = dr["int_br"].ToString();
                //        if (_VouList.InterBrch == "Y")
                //        {
                //            _VouList.HDNListInterBrch = true;
                //        }
                //        else
                //        {
                //            _VouList.HDNListInterBrch = false;
                //        }
                //        _VouList.SrcType = dr["SrcType"].ToString();
                //        _VouList.ReqNo = dr["src_doc_no"].ToString();
                //        _VouList.ReqDt = dr["src_doc_dt"].ToString();
                //        _VouList.Ins_num = dr["ins_no"].ToString();
                //        if (_VouList.Ins_num != null || _VouList.Ins_num != "")
                //        {
                //            _VouList.Ins_num = dr["ins_no"].ToString();
                //        }
                //        else
                //        {
                //            _VouList.Ins_num = "";
                //        }
                //        _VouList.Ins_type = dr["ins_type"].ToString();
                //        if (_VouList.Ins_type.ToString() != null || _VouList.Ins_type.ToString() != "")
                //        {
                //            if (_VouList.Ins_type.ToString() == "C")
                //            {
                //                _VouList.Ins_type = "Cheque";
                //            }
                //            else if (_VouList.Ins_type.ToString() == "P")
                //            {
                //                _VouList.Ins_type = "Digital Payment";
                //            }
                //            else if (_VouList.Ins_type.ToString() == "D")
                //            {
                //                _VouList.Ins_type = "Demand Draft";
                //            }
                //            else if (_VouList.Ins_type.ToString() == "O")
                //            {
                //                _VouList.Ins_type = "Online Transfer";
                //            }
                //            else
                //            {
                //                _VouList.Ins_type = "";
                //            }
                //        }
                //        else
                //        {
                //            _VouList.Ins_type = "";
                //        }
                //        _VouList.curr_logo = dr["curr_logo"].ToString();
                //        _VouList.Amount = dr["vou_amt"].ToString();
                //        _VouList.VouStatus = dr["vou_status"].ToString();
                //        _VouList.CreatedON = dr["created_on"].ToString();
                //        _VouList.ApprovedOn = dr["app_dt"].ToString();
                //        _VouList.ModifiedOn = dr["mod_on"].ToString();
                //        _VouList.create_by = dr["create_by"].ToString();
                //        _VouList.app_by = dr["app_by"].ToString();
                //        _VouList.mod_by = dr["mod_by"].ToString();
                //        _VouList.Reco_Status = dr["reco_status"].ToString();

                //        if (_VouList.Reco_Status.ToString() == "C ")
                //        {
                //            _VouList.Reco_Status = "Cleared";
                //        }
                //        else if (_VouList.Reco_Status.ToString() == "R ")
                //        {
                //            _VouList.Reco_Status = "Returned";
                //        }
                //        else if (_VouList.Reco_Status.ToString() == "U ")
                //        {
                //            _VouList.Reco_Status = "Un-Cleared";
                //        }
                //        else
                //        {
                //            _VouList.Reco_Status = "";
                //        }

                //        _VoucherList.Add(_VouList);
                //    }
                //}
                return _VoucherList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchBankPaymentDetail(BankPaymentList_Model _BankReceiptList_Model,string Src_Type, string bank_id, string Fromdate, string Todate, string Status, string CompID, string Br_ID, string Currency, string InsType, string RecoStatus)
        {
            _VoucherList = new List<VouList>();
            //BankPaymentList_Model _BankReceiptList_Model = new BankPaymentList_Model();
            // Session["WF_status"] = "";
            _BankReceiptList_Model.WF_Status = null;
            /* Commented by Suraj Maurya on 25-07-2025 due list bind methode is changed*/
            //if (Session["CompId"] != null)
            //{
            //    CompID = Session["CompId"].ToString();
            //}

            //if (Session["BranchId"] != null)
            //{
            //    Br_ID = Session["BranchId"].ToString();
            //}
            //string VouType = "BR";
            //DataSet dt = new DataSet();
            //dt = _BankPayment_IService.GetBankPaymentListAll(Src_Type, bank_id, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "", Currency, InsType, RecoStatus);
            ////Session["VouSearch"] = "Vou_Search";
            //_BankReceiptList_Model.VouSearch = "Vou_Search";
            //if (dt.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dt.Tables[0].Rows)
            //    {
            //        VouList _VouList = new VouList();
            //        _VouList.bank_name = dr["bank_name"].ToString();
            //        _VouList.VouNumber = dr["vou_no"].ToString();
            //        _VouList.VouDate = dr["vou_dt"].ToString();
            //        _VouList.hdVouDate = dr["vou_date"].ToString();
            //        _VouList.PDC = dr["pdc"].ToString();/*Add By Hina on 09-08-2024 for pdc and inter branch*/
            //        if (_VouList.PDC == "Y")
            //        {
            //            _VouList.HDNListPDC = true;
            //        }
            //        else
            //        {
            //            _VouList.HDNListPDC = false;
            //        }
            //        _VouList.InterBrch = dr["int_br"].ToString();
            //        if (_VouList.InterBrch == "Y")
            //        {
            //            _VouList.HDNListInterBrch = true;
            //        }
            //        else
            //        {
            //            _VouList.HDNListInterBrch = false;
            //        }
            //        _VouList.SrcType = dr["SrcType"].ToString();
            //        _VouList.ReqNo = dr["src_doc_no"].ToString();
            //        _VouList.ReqDt = dr["src_doc_dt"].ToString();
            //        _VouList.Amount = dr["vou_amt"].ToString();
            //        _VouList.curr_logo = dr["curr_logo"].ToString();
            //        _VouList.VouStatus = dr["vou_status"].ToString();
            //        _VouList.CreatedON = dr["created_on"].ToString();
            //        _VouList.ApprovedOn = dr["app_dt"].ToString();
            //        _VouList.ModifiedOn = dr["mod_on"].ToString();
            //        _VouList.create_by = dr["create_by"].ToString();
            //        _VouList.app_by = dr["app_by"].ToString();
            //        _VouList.mod_by = dr["mod_by"].ToString();
            //        if (_VouList.Ins_num != null || _VouList.Ins_num != "")
            //        {
            //            _VouList.Ins_num = dr["ins_no"].ToString();
            //        }
            //        else
            //        {
            //            _VouList.Ins_num = "";
            //        }
            //        _VouList.Ins_type = dr["ins_type"].ToString();
            //        if (_VouList.Ins_type.ToString() != null || _VouList.Ins_type.ToString() != "")
            //        {
            //            if (_VouList.Ins_type.ToString() == "C")
            //            {
            //                _VouList.Ins_type = "Cheque";
            //            }
            //            else if (_VouList.Ins_type.ToString() == "P")
            //            {
            //                _VouList.Ins_type = "Digital Payment";
            //            }
            //            else if (_VouList.Ins_type.ToString() == "D")
            //            {
            //                _VouList.Ins_type = "Demand Draft";
            //            }
            //            else if (_VouList.Ins_type.ToString() == "O")
            //            {
            //                _VouList.Ins_type = "Online Transfer";
            //            }
            //            else
            //            {
            //                _VouList.Ins_type = "";
            //            }
            //        }
            //        else
            //        {
            //            _VouList.Ins_type = "";
            //        }
            //        _VouList.Reco_Status = dr["reco_status"].ToString();

            //        if (_VouList.Reco_Status.ToString() == "C ")
            //        {
            //            _VouList.Reco_Status = "Cleared";
            //        }
            //        else if (_VouList.Reco_Status.ToString() == "R ")
            //        {
            //            _VouList.Reco_Status = "Returned";
            //        }
            //        else if (_VouList.Reco_Status.ToString() == "U ")
            //        {
            //            _VouList.Reco_Status = "Un-Cleared";
            //        }
            //        else
            //        {
            //            _VouList.Reco_Status = "";
            //        }

            //        _VoucherList.Add(_VouList);
            //    }
            //}
            _BankReceiptList_Model.VoucherList = _VoucherList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBankPaymentList.cshtml", _BankReceiptList_Model);
        }

        private ActionResult BankPaymentDelete(BankPayment_Model _BankReceipt_Model, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string BRNo = _BankReceipt_Model.Vou_No;
                string BankReceiptNumber = BRNo.Replace("/", "");

                string Message = _BankPayment_IService.BPDelete(_BankReceipt_Model, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(BankReceiptNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, BankReceiptNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["BankReceiptNo"] = "";
                //Session["BankReceiptDate"] = "";
                //_BankReceipt_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("BankReceiptDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult BankReceiptApprove(BankPayment_Model _BankReceipt_Model, string VouNo, string VouDate
            , string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1,string int_br_nurr)
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
                /*start Add by Hina on 27-02-2024 to chk Financial year exist or not*/
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
                string Message =  _BankPayment_IService.BankPaymentApprove(VouNo, VouDate, UserID, A_Status, A_Level
                    , A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId, int_br_nurr);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string BankReceiptNo = Message.Split(',')[0].Trim();
                string BankReceiptDate = Message.Split(',')[1].Trim();
                //Session["BankReceiptNo"] = BankReceiptNo;
                //Session["BankReceiptDate"] = BankReceiptDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel urlref = new UrlModel();
                _BankReceipt_Model.TransType = "Update";
                _BankReceipt_Model.BankReceiptNo = BankReceiptNo;
                _BankReceipt_Model.BankReceiptDate = BankReceiptDate;
                //_BankReceipt_Model.Message = "Approved";
                _BankReceipt_Model.BtnName = "BtnEdit";
                try
                {
                    //string fileName = "BR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "BankReceipt_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(VouNo, VouDate, fileName, DocumentMenuId,"AP");
                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, VouNo, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _BankReceipt_Model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                _BankReceipt_Model.Message = _BankReceipt_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _BankReceipt_Model.WF_Status1 = WF_Status1;
                    urlref.wf = WF_Status1;
                }
                TempData["ModelData"] = _BankReceipt_Model;

                urlref.tp = "Update";
                urlref.VN = BankReceiptNo;
                urlref.VDT = BankReceiptDate;
                urlref.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("BankReceiptDetail", "BankReceipt", urlref);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData, string Mailerror)
        {
            // Session["Message"] = "";
            BankPayment_Model _BankReceipt_Model = new BankPayment_Model();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split(',');
            _BankReceipt_Model.BankReceiptNo = a[0].Trim();
            _BankReceipt_Model.BankReceiptDate = a[1].Trim();
            _BankReceipt_Model.TransType = "Update";
            _BankReceipt_Model.BtnName = "BtnToDetailPage";
            _BankReceipt_Model.Message = Mailerror;
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                _BankReceipt_Model.WF_Status1 = a[2].Trim();
                urlModel.wf = a[2].Trim();
            }
            urlModel.bt = "D";
            urlModel.VN = _BankReceipt_Model.BankReceiptNo;
            urlModel.VDT = _BankReceipt_Model.BankReceiptDate;
            urlModel.Cmd = "Update";
            urlModel.tp = "Update";
            TempData["ModelData"] = _BankReceipt_Model;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("BankReceiptDetail", urlModel);
        }

        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                BankPaymentModel _BankPayment = new BankPaymentModel();
                //string TransType = "";
                //string BrecCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["BankReceiptNo"] != null)
                //{
                //    BrecCode = Session["BankReceiptNo"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _BankPayment.Guid = DocNo;
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
                    //Session["AttachMentDetailItmStp"] = dt;
                    _BankPayment.AttachMentDetailItmStp = dt;
                }
                else
                {
                    // Session["AttachMentDetailItmStp"] = null;
                    _BankPayment.AttachMentDetailItmStp = dt;
                }
                TempData["IMGDATA"] = _BankPayment;
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
        public string CheckAdvancePayment(BankPayment_Model _BankReceipt_Model, string DocNo, string DocDate)
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
                DataSet Deatils = _BankPayment_IService.CheckAdvancePayment(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
                if (Deatils.Tables[1].Rows.Count > 0)
                {
                    str = "Used";
                }
                if (Deatils.Tables[3].Rows.Count > 0)/*Added by Suraj Maurya on 27-09-2024 to check bill paid to customers*/
                {
                    str = "Used";
                }
                if (Deatils.Tables[4].Rows.Count > 0)//checking in ar, ar$adv and BR,CR,InvAdj,'CN' 
                {
                    str = "Used";
                }
                if (str != "" && str != null)
                {
                    _BankReceipt_Model.BankReceiptNo = DocNo;
                    _BankReceipt_Model.BankReceiptDate = DocDate;
                    _BankReceipt_Model.TransType = "Update";
                    _BankReceipt_Model.BtnName = "BtnToDetailPage";
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

        /*------------- Cost Center Section-----------------*/

        public ActionResult GetCstCntrtype(string Flag, string Disableflag, string CC_rowdata,string TotalAmt=null,string Doc_ID=null)//add by sm 04-12-2024 (TotalAmt,Doc_ID)
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
                ViewBag.CCTotalAmt = TotalAmt;//add by sm 04-12-2024
                ViewBag.DocId = Doc_ID;//add by sm 04-12-2024
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

        public FileResult GenratePdfFile(BankPayment_Model _Model)
        {
            return File(GetPdfData(_Model.Vou_No, _Model.Vou_Date), "application/pdf", "BankReceipt.pdf");
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
                DataSet Deatils = _Common_IServices.Cmn_GetGLVoucherPrintDeatils(CompID, Br_ID, vNo, vDate, "BR");
                if (Deatils.Tables[0].Rows[0]["int_br"].ToString().Trim() == "Y")
                {
                    //ViewBag.Title = "Bank Receipt (Inter Branch)";/*Commented and change by Hina on 17-10-2024*/
                    ViewBag.Title = "Receipt Voucher (Inter Branch)";
                }
                else
                {
                    //ViewBag.Title = "Bank Receipt";/*Commented and change by Hina on 17-10-2024*/
                    ViewBag.Title = "Receipt Voucher";
                }
                ViewBag.PageName = "BP";
                
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                ViewBag.DocumentMenuId = DocumentMenuId;

                /* ------------------Added by Suraj Maurya on 17-02-2025 to add logo on pdf -------------------------*/
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                /* ------------------Added by Suraj Maurya on 17-02-2025  to add logo on pdf ------------------------*/

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
                        draftImage = Server.MapPath("~/Content/Images/cancelled.png");/*Add by NItesh  on 08-09-2025 */
                    }
                    else
                    {
                        draftImage = Server.MapPath("~/Content/Images/draft.png");/*Add by Hina sharma on 16-10-2024 */
                    }
                    //string draftImage = Server.MapPath("~/Content/Images/draft.png");
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
        /*-----Add By Hina on 09-08-2024 for PDC-------------------*/
        [NonAction]
        private DataTable GetGLVoucherDtForPDC()
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataTable ds = _BankPayment_IService.GetGLVoucherDtForPDC(CompID, Br_ID);
                return ds;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        /* Commented by Suraj Maurya on 25-07-2025 due to method is changed */
        //public FileResult BankReceiptExporttoExcelDt(string SrcType, string BankName, string Fromdate, string Todate, string Status, string Currency, string InsType, string RecoStatus)
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        if (Session["UserId"] != null)
        //        {
        //            UserID = Session["UserId"].ToString();
        //        }
        //        string VouType = "BR";
        //        DataTable dt = new DataTable();
        //        DataSet dt1 = _BankPayment_IService.GetBankPaymentListAll(SrcType, BankName, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "", Currency, InsType, RecoStatus);
        //        dt.Columns.Add("Sr.No", typeof(string));
        //        dt.Columns.Add("Bank Account", typeof(string));
        //        dt.Columns.Add("GL Voucher Number", typeof(string));
        //        dt.Columns.Add("GL Voucher Date", typeof(string));
        //        dt.Columns.Add("PDC", typeof(string));
        //        dt.Columns.Add("Inter Branch", typeof(string));
        //        dt.Columns.Add("Source Type", typeof(string));
        //        dt.Columns.Add("Requisition Number", typeof(string));
        //        dt.Columns.Add("Requisition Date", typeof(string));
        //        dt.Columns.Add("Instrument Type", typeof(string));
        //        dt.Columns.Add("Instrument Number", typeof(string));
        //        dt.Columns.Add("Currency", typeof(string));
        //        dt.Columns.Add("Amount", typeof(decimal));
        //        dt.Columns.Add("Status", typeof(string));
        //        dt.Columns.Add("Reconciliation Status", typeof(string));
        //        dt.Columns.Add("Created By", typeof(string));
        //        dt.Columns.Add("Created On", typeof(string));
        //        dt.Columns.Add("Approved By", typeof(string));
        //        dt.Columns.Add("Approved On", typeof(string));
        //        dt.Columns.Add("Amended By", typeof(string));
        //        dt.Columns.Add("Amended On", typeof(string));

        //        if (dt1.Tables[0].Rows.Count > 0)
        //        {
        //            int rowno = 0;
        //            foreach (DataRow dr in dt1.Tables[0].Rows)
        //            {
        //                var Ins_Type = "";
        //                var insType = dr["ins_type"].ToString();
        //                if (insType == "C")
        //                {
        //                    Ins_Type = "Cheque";
        //                }
        //                else if (insType == "P")
        //                {
        //                    Ins_Type = "Digital Payment";
        //                }
        //                else if (insType == "D")
        //                {
        //                    Ins_Type = "Demand Draft";
        //                }
        //                else if (insType == "O")
        //                {
        //                    Ins_Type = "Online Transfer";
        //                }
        //                else
        //                {
        //                    Ins_Type = "";
        //                }
        //                var PDC_Data = "";
        //                var PDC = dr["pdc"].ToString();/*Add By Hina on 09-08-2024 for pdc and inter branch*/
        //                if (PDC == "Y")
        //                {
        //                    PDC_Data = "Yes";
        //                }
        //                else
        //                {
        //                    PDC_Data = "No";
        //                }
        //                var InterBranch = "";
        //                var int_br = dr["int_br"].ToString();
        //                if (int_br == "Y")
        //                {
        //                    InterBranch = "Yes";
        //                }
        //                else
        //                {
        //                    InterBranch = "No";
        //                }
        //                var RecoSt = "";
        //                var Reco_Status = dr["reco_status"].ToString();
        //                if (Reco_Status == "C ")
        //                {
        //                    RecoSt = "Cleared";
        //                }
        //                else if (Reco_Status == "R ")
        //                {
        //                    RecoSt = "Returned";
        //                }
        //                else if (Reco_Status == "U ")
        //                {
        //                    RecoSt = "Un-Cleared";
        //                }
        //                else
        //                {
        //                    RecoSt = "";
        //                }
        //                DataRow dtrowLines = dt.NewRow();
        //                dtrowLines["Sr.No"] = rowno + 1;
        //                dtrowLines["Bank Account"] = dr["bank_name"].ToString();
        //                dtrowLines["GL Voucher Number"] = dr["vou_no"].ToString();
        //                dtrowLines["GL Voucher Date"] = dr["vou_dt"].ToString();
        //                dtrowLines["PDC"] = PDC_Data;
        //                dtrowLines["Inter Branch"] = InterBranch;
        //                dtrowLines["Source Type"] = dr["SrcType"].ToString();
        //                dtrowLines["Requisition Number"] = dr["src_doc_no"].ToString();
        //                dtrowLines["Requisition Date"] = dr["src_doc_dt"].ToString();
        //                dtrowLines["Instrument Type"] = Ins_Type;
        //                dtrowLines["Instrument Number"] = dr["ins_no"].ToString();
        //                dtrowLines["Currency"] = dr["curr_logo"].ToString();
        //                dtrowLines["Amount"] = dr["vou_amt"].ToString();
        //                dtrowLines["Status"] = dr["vou_status"].ToString();
        //                dtrowLines["Reconciliation Status"] = RecoSt;
        //                dtrowLines["Created By"] = dr["create_by"].ToString();
        //                dtrowLines["Created On"] = dr["created_on"].ToString();
        //                dtrowLines["Approved By"] = dr["app_by"].ToString();
        //                dtrowLines["Approved On"] = dr["app_dt"].ToString();
        //                dtrowLines["Amended By"] = dr["mod_by"].ToString();
        //                dtrowLines["Amended On"] = dr["mod_on"].ToString();
        //                dt.Rows.Add(dtrowLines);
        //                rowno = rowno + 1;
        //            }
        //        }
        //        var commonController = new CommonController(_Common_IServices);
        //        return commonController.ExportDatatableToExcel("BankReceipt", dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}

        public JsonResult LoadBankRecieptDetailsData(string Src_Type, string bank_id, string Fromdate, string Todate, string Status
            , string Currency, string InsType, string RecoStatus,string WF_Status)
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

                List<BankPaymentListDataModel> _ItemListModel = new List<BankPaymentListDataModel>();

                //Get Data Filtered,with Paging,Shorted
                (_ItemListModel, recordsTotal) = getDtList(Src_Type, bank_id, Fromdate, Todate, Status, Currency, InsType, RecoStatus
                            , skip, pageSize, searchValue, sortColumn, sortColumnDir, WF_Status);

                // Getting all Customer data    
                var ItemListData = (from tempitem in _ItemListModel select tempitem);

                //Paging     
                var data = ItemListData.ToList();//.Skip(skip).Take(pageSize).ToList();

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
        private (List<BankPaymentListDataModel>, int) getDtList(string Src_Type, string bank_id, string Fromdate, string Todate, string Status
            , string Currency, string InsType, string RecoStatus, int skip, int pageSize, string searchValue
            , string sortColumn, string sortColumnDir,string WF_Status, string Flag = "")
        {
            List<BankPaymentListDataModel> _ItemListModel = new List<BankPaymentListDataModel>();
            int Total_Records = 0;
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string BrId = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }

                DataSet DSet = new DataSet();
                DSet = _BankPayment_IService.GetBankPaymentListAll(Src_Type, bank_id, Fromdate, Todate, Status, CompID
                    , BrId, "BR", WF_Status, User_ID, "105104115110", Currency, InsType, RecoStatus
                    //, BrId, "BR", "", "", "", Currency, InsType, RecoStatus
                    , skip.ToString(), pageSize.ToString(), searchValue, sortColumn, sortColumnDir, Flag);


                if (DSet.Tables.Count >= 2)
                {
                    if (DSet.Tables[0].Rows.Count > 0)
                    {
                        if (Flag == "CSV")
                        {
                            _ItemListModel = DSet.Tables[0].AsEnumerable()
                .Select((row, index) => new BankPaymentListDataModel
                {
                    SrNo = row.Field<int>("SrNo"),
                    bank_name = row.Field<string>("bank_name"),
                    acc_id = row.Field<string>("acc_id"),
                    vou_no = row.Field<string>("vou_no"),
                    vou_dt = row.Field<string>("vou_dt"),
                    vou_date = row.Field<string>("vou_date"),
                    vou_status = row.Field<string>("vou_status"),
                    created_on = row.Field<string>("created_on"),
                    app_dt = row.Field<string>("app_dt"),
                    create_by = row.Field<string>("create_by"),
                    mod_by = row.Field<string>("mod_by"),
                    app_by = row.Field<string>("app_by"),
                    mod_on = row.Field<string>("mod_on"),
                    SrcType = row.Field<string>("SrcType"),
                    pdc = row.Field<string>("pdc"),
                    int_br = row.Field<string>("int_br"),
                    src_doc_no = row.Field<string>("src_doc_no"),
                    src_doc_dt = row.Field<string>("src_doc_dt"),
                    vou_amt = row.Field<string>("vou_amt").Replace(",",""),
                    curr_logo = row.Field<string>("curr_logo"),
                    reco_status = GetRecoStatusDisplay(row.Field<string>("reco_status")),
                    ins_type = GetInstrumentTypeDisplay(row.Field<string>("ins_type")),
                    ins_no = row.Field<string>("ins_no"),
                }).ToList();
                        }
                        else
                        {
                            _ItemListModel = DSet.Tables[0].AsEnumerable()
            .Select((row, index) => new BankPaymentListDataModel
            {
                SrNo = row.Field<int>("SrNo"),
                bank_name = row.Field<string>("bank_name"),
                acc_id = row.Field<string>("acc_id"),
                vou_no = row.Field<string>("vou_no"),
                vou_dt = row.Field<string>("vou_dt"),
                vou_date = row.Field<string>("vou_date"),
                vou_status = row.Field<string>("vou_status"),
                created_on = row.Field<string>("created_on"),
                app_dt = row.Field<string>("app_dt"),
                create_by = row.Field<string>("create_by"),
                mod_by = row.Field<string>("mod_by"),
                app_by = row.Field<string>("app_by"),
                mod_on = row.Field<string>("mod_on"),
                SrcType = row.Field<string>("SrcType"),
                pdc = row.Field<string>("pdc"),
                int_br = row.Field<string>("int_br"),
                src_doc_no = row.Field<string>("src_doc_no"),
                src_doc_dt = row.Field<string>("src_doc_dt"),
                vou_amt = row.Field<string>("vou_amt"),
                curr_logo = row.Field<string>("curr_logo"),
                reco_status = GetRecoStatusDisplay(row.Field<string>("reco_status")),
                ins_type = GetInstrumentTypeDisplay(row.Field<string>("ins_type")),
                ins_no = row.Field<string>("ins_no"),
            }).ToList();
                        }
                        
                    }
                    if (DSet.Tables[1].Rows.Count > 0)
                    {
                        Total_Records = Convert.ToInt32(DSet.Tables[1].Rows[0]["total_rows"]);
                    }
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
            return (_ItemListModel, Total_Records);
        }
        public ActionResult BankReceiptListActionCommands(BankPaymentList_Model _Model, string Command)
        {
            try
            {

                switch (Command)
                {
                    case "CSV":
                        return GenrateCsvFile(_Model);
                }
                return RedirectToAction("BankPayment");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult GenrateCsvFile(BankPaymentList_Model _Model)
        {
            try
            {
                string User_ID = string.Empty;
                string CompID = string.Empty;
                string BrId = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrId = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                var SearchValue = IsNull(_Model.searchValue, "");
                var Direction = IsNull(_Model.sortColumnDir, "asc");
                var sortColumnNm = IsNull(_Model.sortColumn, "SrNo");
                DataSet DSet = new DataSet();
                DSet = _BankPayment_IService.GetBankPaymentListAll(_Model.Src_Type, _Model.bank_id, _Model.FromDate
                    , _Model.ToDate.ToString("yyyy-MM-dd"), _Model.Status, CompID
                    , BrId, "BR", "", "", "", _Model.Curr_nm, _Model.Instr_type, _Model.Reco_Status
                     , "0", "0", SearchValue, sortColumnNm, Direction, "CSV");
                //, "0", "0", "", "SrNo", "asc", "CSV");//COMMENTED by shubham maurya on 31-07-2025 for add Model.searchValue


                DataTable newTable = new DataTable();
                newTable.Columns.Add("Sr.No.", typeof(string));
                newTable.Columns.Add("Bank Account", typeof(string));
                newTable.Columns.Add("GL Voucher Number", typeof(string));
                newTable.Columns.Add("GL Voucher Date", typeof(string));
                newTable.Columns.Add("PDC", typeof(string));
                newTable.Columns.Add("Inter Branch", typeof(string));
                newTable.Columns.Add("Source Type", typeof(string));
                newTable.Columns.Add("Requisition Number", typeof(string));
                newTable.Columns.Add("Requisition Date", typeof(string));
                newTable.Columns.Add("Instrument Type", typeof(string));
                newTable.Columns.Add("Instrument Number", typeof(string));
                newTable.Columns.Add("Currency", typeof(string));
                newTable.Columns.Add("Amount", typeof(decimal));
                newTable.Columns.Add("Status", typeof(string));
                newTable.Columns.Add("Reconciliation Status", typeof(string));
                newTable.Columns.Add("Created By", typeof(string));
                newTable.Columns.Add("Created On", typeof(string));
                newTable.Columns.Add("Approved By", typeof(string));
                newTable.Columns.Add("Approved On", typeof(string));
                newTable.Columns.Add("Amended By", typeof(string));
                newTable.Columns.Add("Amended On", typeof(string));

                // Copy relevant data from the original table
                foreach (DataRow row in DSet.Tables[0].Rows)
                {
                    newTable.Rows.Add(row["SrNo"], row["bank_name"], row["vou_no"], row["vou_dt"], row["pdc"], row["int_br"]
                        , row["SrcType"], row["src_doc_no"], row["src_doc_dt"], GetInstrumentTypeDisplay(row["ins_type"].ToString())
                        , row["ins_no"], row["curr_logo"], row["vou_amt"], row["vou_status"], GetRecoStatusDisplay(row["reco_status"].ToString())
                        , row["create_by"], row["created_on"], row["app_by"]
                        , row["app_dt"], row["mod_by"], row["mod_on"]);
                    // Selecting only needed columns

                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("Bank Receipt", newTable);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public static string GetInstrumentTypeDisplay(string insType)/* Created by Suraj Maurya on 25-07-2025 to get Intrument Type Name */
        {
            if (string.IsNullOrWhiteSpace(insType))
                return "";

            switch (insType.Trim())
            {
                case "C":
                    return "Cheque";
                case "P":
                    return "Digital Payment";
                case "D":
                    return "Demand Draft";
                case "O":
                    return "Online Transfer";
                default:
                    return "";
            }
        }

        public static string GetRecoStatusDisplay(string recoStatus)/* Created by Suraj Maurya on 25-07-2025 to get Reco Status Name */
        {
            if (string.IsNullOrWhiteSpace(recoStatus)) return "";

            switch (recoStatus.Trim())
            {
                case "C":
                    return "Cleared";
                case "R":
                    return "Returned";
                case "U":
                    return "Un-Cleared";
                default:
                    return "";
            }
        }

        [HttpPost]
        public ActionResult ExportCsv([System.Web.Http.FromBody] DataTableRequest request, string Src_Type
            , string bank_id, string Fromdate, string Todate, string Status
            , string Currency, string InsType, string RecoStatus, string WF_Status)
        {
            string keyword = "";
            // Apply search filter
            if (!string.IsNullOrEmpty(request.search?.value))
            {
                keyword = request.search.value;//.ToLower();
            }
            int recordsTotal = 0;
            string sortColumn = "SrNo";
            string sortColumnDir = "asc";
            if (request.order != null && request.order.Any())
            {
                var colIndex = request.order[0].column;
                sortColumnDir = request.order[0].dir;
                sortColumn = request.columns[colIndex].data;

            }
            List<BankPaymentListDataModel> _ItemListModelList = new List<BankPaymentListDataModel>();
            // 🔹 Fetch data same as LoadData but ignore paging
            (_ItemListModelList, recordsTotal) = getDtList(Src_Type, bank_id, Fromdate, Todate, Status, Currency, InsType, RecoStatus
                            , 0, request.length, keyword, sortColumn, sortColumnDir, WF_Status, "CSV");


            var data = _ItemListModelList.ToList(); // All filtered & sorted rows
            var commonController = new CommonController(_Common_IServices);
            return commonController.Cmn_GetDataToCsv(request, data);
        }

        public ActionResult ClickAdjustedAmountDetail(string InVNo, string InvDate, string Accid, string VouNo,int AccTyp)/*Add by Hina sharma on 10-12-2024*/
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                //if (Session["userid"] != null)
                //    UserID = Session["userid"].ToString();
                //DataTable dt = _AccountReceivable_ISERVICE.SearchAdvanceAmountDetail(CompID, Br_ID, accId);
                DataSet dt = _BankPayment_IService.SearchAdjustedAmountDetail(CompID, Br_ID, InVNo, InvDate, Accid, VouNo, AccTyp);
                ViewBag.AdjustedAmountDetail = dt.Tables[0];
                ViewBag.AdjustedAmountDetailTotal = dt.Tables[1];
                //ViewBag.AdvanceAmountDetail = dt;
                return PartialView("~/Areas/Common/Views/Cmn_PartialAdjustedAmountDetails.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
    }
}



















