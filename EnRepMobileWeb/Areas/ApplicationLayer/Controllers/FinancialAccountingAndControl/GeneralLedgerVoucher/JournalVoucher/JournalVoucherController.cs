using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher.JournalVoucher_Model;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using System.IO;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Configuration;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.JournalVoucher
{
    public class JournalVoucherController : Controller
    {
        string CompID, BrchID, UserID, language, title = String.Empty;
        string DocumentMenuId = "105104115101";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        JournalVoucher_ISERVICES _journalVoucher_ISERVICES;
        public JournalVoucherController(Common_IServices _Common_IServices, JournalVoucher_ISERVICES _journalVoucher_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._journalVoucher_ISERVICES = _journalVoucher_ISERVICES;
        }
        // GET: ApplicationLayer/JournalVoucher
        public ActionResult JournalVoucher(JournalVoucher_Model ApproveModel)
        {
            try
            {
                //JournalVoucher_Model ApproveModel = new JournalVoucher_Model();
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
                    UserID = Session["userid"].ToString();
                }
                // ApproveModel.WF_Status = WF_Status;
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<StatusList> statusLists = new List<StatusList>();
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new StatusList { status_id = x.status_id, status_name = x.status_name });
                ApproveModel._StatusLists = listOfStatus;

                DateTime dtnow = DateTime.Now;
                string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                //string endDate = dtnow.ToString("yyyy-MM-dd");
                ViewBag.MenuPageName = getDocumentName();
                ViewBag.VBRoleList = GetRoleList();
                ApproveModel.Title = title;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    ApproveModel.JV_FromDate = a[0].Trim();
                    ApproveModel.JV_ToDate = a[1].Trim();
                    ApproveModel.JV_status = a[2].Trim();
                    if (ApproveModel.JV_status == "0")
                    {
                        ApproveModel.JV_status = null;
                    }
                    ApproveModel.JV_FromDate = ApproveModel.JV_FromDate;
                    ApproveModel.ListFilterData = TempData["ListFilterData"].ToString();
                }

                DataSet dt1 = _journalVoucher_ISERVICES.GetJVDetailList(CompID, BrchID, ApproveModel.JV_FromDate, ApproveModel.JV_ToDate, ApproveModel.JV_status, UserID, ApproveModel.WF_Status, DocumentMenuId, "0");
                ViewBag.JVDetailsList = dt1.Tables[0];
                //---Comented by Satya Veer To change From Date to Financial Year start Date-----//
                //if (ApproveModel.JV_FromDate == null)
                //{
                //    ApproveModel.JV_FromDate = startDate;
                //}
                if (ApproveModel.JV_FromDate == null)
                {
                    ApproveModel.JV_FromDate = dt1.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                //dt1.Tables[1].Rows[0]["finstrdate"].ToString();
                //Session["JVSearch"] = "0";
                ApproveModel.JVSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/JournalVoucher/JournalVoucherList.cshtml", ApproveModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //----It's used in Dashboard Js----//
        public ActionResult GetJournalVoucherList(string docid, string status)
        {
            //Session["WF_status"] = status;                 
            JournalVoucher_Model ApproveModels = new JournalVoucher_Model();
            ApproveModels.WF_Status = status;
            return RedirectToAction("JournalVoucher", ApproveModels);
        }
        public ActionResult AddJournalVoucherDetail()
        {
            try
            {
                JournalVoucher_Model AddNewModel = new JournalVoucher_Model();
                /*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
                //if (Session["CompId"] != null)
                //    CompID = Session["CompId"].ToString();
                //if (Session["BranchId"] != null)
                //    BrchID = Session["BranchId"].ToString();
                //DateTime dtnow = DateTime.Now;
                //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                //var commCont = new CommonController(_Common_IServices);
                ////if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                ////{
                ////    TempData["Message"] = "Financial Year not Exist";
                ////    AddNewModel.Message = "Financial Year not Exist";
                ////    return RedirectToAction("JournalVoucher", AddNewModel);
                ////}
                ////if (commCont.CheckFinancialYear(CompID, BrchID)  == "FB Close")
                ////{
                ////    TempData["FBMessage"] = "Financial Book Closing";
                ////    return RedirectToAction("JournalVoucher", AddNewModel);
                ////}
                //string MsgNew = string.Empty;
                //MsgNew = commCont.Fin_CheckFinancialYear(CompID, BrchID, CurrentDate);
                //if (MsgNew == "FY Not Exist")
                //{
                //    TempData["Message"] = "Financial Year not Exist";
                //    return RedirectToAction("JournalVoucher", AddNewModel);
                //}
                //if (MsgNew == "FB Close")
                //{
                //    TempData["FBMessage"] = "Financial Book Closing";
                //    return RedirectToAction("JournalVoucher", AddNewModel);
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
                return RedirectToAction("JournalVoucherDetail", "JournalVoucher", AddNew_Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Dblclick(string JVId, string JVDate, string ListFilterData, string WF_Status)
        {

            JournalVoucher_Model dblclick = new JournalVoucher_Model();
            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            string DblClkMsg = string.Empty;
            DblClkMsg = commCont.Fin_CheckFinancialYear(CompID, BrchID, JVDate);
            if (DblClkMsg == "FY Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                dblclick.BtnName = "BtnRefresh";
            }
            else if (DblClkMsg == "FB Close")
            {
                TempData["FBMessage"] = "Financial Book Closing";
                dblclick.BtnName = "BtnRefresh";
            }
            else
            {
                dblclick.BtnName = "BtnToDetailPage";
            }
            /*End to chk Financial year exist or not*/
            UrlModel dblclick_Model = new UrlModel();
            dblclick.JV_num = JVId;
            dblclick.JV_dt = JVDate;
            dblclick.TransType = "Update";
            dblclick.Command = "Refresh";
            dblclick.Message = "New";
            //dblclick.BtnName = "BtnToDetailPage";
            if (WF_Status != null && WF_Status != "")
            {
                dblclick_Model.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            //_url.Cmd = "Update";
            dblclick_Model.tp = "Update";
            dblclick_Model.bt = "D";
            dblclick_Model.JV_num = dblclick.JV_num;
            dblclick_Model.JV_dt = dblclick.JV_dt;
            TempData["ListFilterData"] = ListFilterData;

            return RedirectToAction("JournalVoucherDetail", dblclick_Model);
        }
        //public ActionResult JournalVoucherDetail(string JVId, string JVDate,string ListFilterData)
        public ActionResult JournalVoucherDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
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
                    UserID = Session["userid"].ToString();
                }
                ViewBag.DocumentMenuId = DocumentMenuId;

                var ApproveModel1 = TempData["ModelData"] as JournalVoucher_Model;
                if (ApproveModel1 != null)
                {
                    //JournalVoucher_Model ApproveModel = new JournalVoucher_Model();
                    BindReplicateWithlist(ApproveModel1);
                    ViewBag.MenuPageName = getDocumentName();
                    ApproveModel1.Title = title;
                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    var create_id = "";
                    //TempData["ListFilterData"] = ListFilterData;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        ApproveModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (ApproveModel1.TransType == "Update" || ApproveModel1.TransType == "Edit")
                    {
                        //string Doc_no = Session["JV_No"].ToString();
                        string Doc_no = ApproveModel1.JV_num;
                        //string Doc_date = Session["JV_Date"].ToString();
                        string Doc_date = ApproveModel1.JV_dt;
                        DataSet ds = _journalVoucher_ISERVICES.getdetailsJV(CompID, BrchID, Doc_no, Doc_date, UserID, DocumentMenuId);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ApproveModel1.JV_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                            ApproveModel1.JV_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                            ApproveModel1.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            ApproveModel1.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            ApproveModel1.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            ApproveModel1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            ApproveModel1.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            ApproveModel1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                            ApproveModel1.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            ApproveModel1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            ApproveModel1.JV_status = ds.Tables[0].Rows[0]["vou_status"].ToString();
                            ApproveModel1.Narrat = ds.Tables[1].Rows[0]["narr"].ToString();
                            create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                            {
                                ApproveModel1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                ApproveModel1.CancelFlag = true;
                                //Session["BtnName"] = "BtnRefresh";
                                ApproveModel1.BtnName = "BtnRefresh";
                            }
                            else
                            {
                                ApproveModel1.CancelFlag = false;
                            }
                            ViewBag.GlAccountDetails = ds.Tables[1];

                            ViewBag.SpanDrbtAmnt = ds.Tables[1].Rows[0]["Total_dr_amt_bs"].ToString();
                            ViewBag.SpanCrdtAmnt = ds.Tables[1].Rows[0]["Total_cr_amt_bs"].ToString();

                            ApproveModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                            ApproveModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);//Cancelled
                        }
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.CostCenterData = ds.Tables[6];
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                        ApproveModel1.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        ApproveModel1.DocumentStatus = doc_status;
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && ApproveModel1.Command != "Edit")
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

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    ApproveModel1.BtnName = "BtnRefresh";
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
                                        ApproveModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            ApproveModel1.BtnName = "BtnRefresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            ApproveModel1.BtnName = "BtnRefresh";
                                        }
                                        else
                                        {
                                            ApproveModel1.BtnName = "BtnToDetailPage";
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        //ApproveModel1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ApproveModel1.BtnName = "BtnToDetailPage";
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
                                        ApproveModel1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ApproveModel1.BtnName = "BtnToDetailPage";
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
                                    ApproveModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ApproveModel1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    ApproveModel1.BtnName = "BtnRefresh";
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

                    }
                    else
                    {
                        //Session["DocumentStatus"] = "D";
                        ApproveModel1.DocumentStatus = "D";
                        //ViewBag.GlAccountDetails = ApproveModel1.GlAccountDetails;
                    }

                    //}
                    ViewBag.VBRoleList = GetRoleList();

                    return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/JournalVoucher/JournalVoucherDetail.cshtml", ApproveModel1);
                }
                else
                {
                    JournalVoucher_Model ApproveModel = new JournalVoucher_Model();
                    BindReplicateWithlist(ApproveModel);
                    ViewBag.MenuPageName = getDocumentName();
                    ApproveModel.Title = title;
                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            ApproveModel.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            //ApproveModel.BtnName = _urlModel.bt;
                            ApproveModel.BtnName = "BtnAddNew";
                        }
                        ApproveModel.JV_num = _urlModel.JV_num;
                        ApproveModel.JV_dt = _urlModel.JV_dt;
                        ApproveModel.Command = _urlModel.Cmd;
                        ApproveModel.TransType = _urlModel.tp;
                        ApproveModel.WF_Status1 = _urlModel.wf;
                    }
                    if (ApproveModel.TransType == null)
                    {
                        ApproveModel.BtnName = "BtnRefresh";
                        ApproveModel.Command = "Refresh";
                        ApproveModel.TransType = "Refresh";

                    }
                    /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    BrchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var JVDate = ApproveModel.JV_dt;
                    //var JVDate = "";

                    //if (ApproveModel.JV_dt != null)
                    //{
                    //    JVDate = ApproveModel.JV_dt;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    ApproveModel.JV_dt = CurrentDate;
                    //    ApproveModel.JV_Date = CurrentDate;
                    //    JVDate = ApproveModel.JV_dt;
                    //}
                    //if (commCont.Fin_CheckFinancialYear(CompID, BrchID, JVDate) == "FY Not Exist")
                    //{
                    //    TempData["Message"] = "Financial Year not Exist";
                    //}
                    //if (commCont.Fin_CheckFinancialYear(CompID, BrchID, JVDate) == "FB Close")
                    //{
                    //    TempData["FBMessage"] = "Financial Book Closing";
                    //}
                    /*End to chk Financial year exist or not*/
                    var create_id = "";
                    // TempData["ListFilterData"] = ListFilterData;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        ApproveModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (ApproveModel.TransType == "Update" || ApproveModel.TransType == "Edit")
                    {
                        //string Doc_no = Session["JV_No"].ToString();
                        string Doc_no = ApproveModel.JV_num;
                        //string Doc_date = Session["JV_Date"].ToString();
                        string Doc_date = ApproveModel.JV_dt;
                        DataSet ds = _journalVoucher_ISERVICES.getdetailsJV(CompID, BrchID, Doc_no, Doc_date, UserID, DocumentMenuId);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ApproveModel.JV_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                            ApproveModel.JV_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                            ApproveModel.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            ApproveModel.Create_by = ds.Tables[0].Rows[0]["create_nm"].ToString();
                            ApproveModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            ApproveModel.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            ApproveModel.Approved_by = ds.Tables[0].Rows[0]["app_nm"].ToString();
                            ApproveModel.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                            ApproveModel.Amended_by = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                            ApproveModel.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            ApproveModel.JV_status = ds.Tables[0].Rows[0]["vou_status"].ToString();
                            ApproveModel.Narrat = ds.Tables[1].Rows[0]["narr"].ToString();
                            create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                            {
                                ApproveModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                ApproveModel.CancelFlag = true;
                                //Session["BtnName"] = "BtnRefresh";
                                ApproveModel.BtnName = "BtnRefresh";
                            }
                            else
                            {
                                ApproveModel.CancelFlag = false;
                            }
                            ViewBag.GlAccountDetails = ds.Tables[1];

                            ViewBag.SpanDrbtAmnt = ds.Tables[1].Rows[0]["Total_dr_amt_bs"].ToString();
                            ViewBag.SpanCrdtAmnt = ds.Tables[1].Rows[0]["Total_cr_amt_bs"].ToString();

                            ApproveModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                            ApproveModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);//Cancelled
                        }
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.CostCenterData = ds.Tables[6];
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                        ApproveModel.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        ApproveModel.DocumentStatus = doc_status;
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && ApproveModel.Command != "Edit")
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

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    ApproveModel.BtnName = "BtnRefresh";
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
                                        ApproveModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            ApproveModel.BtnName = "BtnRefresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            ApproveModel.BtnName = "BtnRefresh";
                                        }
                                        else
                                        {
                                            ApproveModel.BtnName = "BtnToDetailPage";
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        //ApproveModel.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ApproveModel.BtnName = "BtnToDetailPage";
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
                                        ApproveModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ApproveModel.BtnName = "BtnToDetailPage";
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
                                    ApproveModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    ApproveModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    ApproveModel.BtnName = "BtnRefresh";
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

                    }
                    else
                    {
                        //Session["DocumentStatus"] = "D";
                        ApproveModel.DocumentStatus = "D";
                    }

                    //}
                    ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/JournalVoucher/JournalVoucherDetail.cshtml", ApproveModel);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JournalVoucCommands(JournalVoucher_Model ApproveModel, string command)
        {
            try
            {/*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                string JV_No = string.Empty;
                string JVDate = string.Empty;

                if (ApproveModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        JournalVoucher_Model adddnew = new JournalVoucher_Model();
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
                        /*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
                        //if (Session["CompId"] != null)
                        //    CompID = Session["CompId"].ToString();
                        //if (Session["BranchId"] != null)
                        //    BrchID = Session["BranchId"].ToString();
                        //DateTime dtnow = DateTime.Now;
                        //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                        ////var commCont1 = new CommonController(_Common_IServices);
                        //string MsgNew = string.Empty;
                        //MsgNew = commCont.Fin_CheckFinancialYear(CompID, BrchID, CurrentDate);
                        //if (MsgNew == "FY Not Exist")
                        //{ 
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    adddnew.BtnName = "BtnRefresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.JV_Date = CurrentDate;
                        //    return RedirectToAction("JournalVoucherDetail", "JournalVoucher", adddnew);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    adddnew.BtnName = "BtnRefresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.JV_Date = CurrentDate;
                        //    return RedirectToAction("JournalVoucherDetail", "JournalVoucher", adddnew);
                        //}
                        /*End to chk Financial year exist or not*/

                        return RedirectToAction("JournalVoucherDetail", "JournalVoucher", NewModel);
                    case "Edit":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        JVDate = ApproveModel.JV_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, BrchID, JVDate);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (ApproveModel.doc_status == "A" || ApproveModel.doc_status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("Dblclick", new { JVId = ApproveModel.JV_No, JVDate = ApproveModel.JV_Date, ListFilterData = ApproveModel.ListFilterData1, WF_Status = ApproveModel.WFStatus });

                            }
                        }
                        /*End to chk Financial year exist or not*/
                        string FinalStatus = CheckSVForCancellation(ApproveModel, ApproveModel.JV_No, ApproveModel.JV_Date.ToString());
                        if (FinalStatus == "Used" || FinalStatus == "Used1")
                        {
                            ApproveModel.Message = FinalStatus;
                            ApproveModel.Message1 = FinalStatus;
                            ApproveModel.JV_num = ApproveModel.JV_No;
                            ApproveModel.JV_dt = ApproveModel.JV_Date;
                            TempData["ModelData"] = ApproveModel;
                        }
                        else if (FinalStatus == "AutoGen")
                        {
                            ApproveModel.Message = FinalStatus;
                            ApproveModel.Message1 = FinalStatus;
                            ApproveModel.JV_num = ApproveModel.JV_No;
                            ApproveModel.JV_dt = ApproveModel.JV_Date;
                            TempData["ModelData"] = ApproveModel;
                        }
                        else
                        {
                            ApproveModel.TransType = "Update";
                            ApproveModel.Command = command;
                            ApproveModel.BtnName = "BtnEdit";
                            ApproveModel.JV_num = ApproveModel.JV_No;
                            ApproveModel.JV_dt = ApproveModel.JV_Date;
                            TempData["ModelData"] = ApproveModel;
                            UrlModel EditModel = new UrlModel();
                            EditModel.tp = "Update";
                            EditModel.Cmd = command;
                            EditModel.bt = "BtnEdit";
                            EditModel.JV_num = ApproveModel.JV_No;
                            EditModel.JV_dt = ApproveModel.JV_Date;
                            TempData["ListFilterData"] = ApproveModel.ListFilterData1;
                            return RedirectToAction("JournalVoucherDetail", EditModel);
                        }
                        UrlModel Model = new UrlModel();
                        Model.bt = "D";
                        Model.JV_num = ApproveModel.JV_No;
                        Model.JV_dt = ApproveModel.JV_Date;
                        Model.tp = "Update";
                        TempData["ListFilterData"] = ApproveModel.ListFilterData1;
                        return RedirectToAction("JournalVoucherDetail", Model);

                    case "Delete":
                        JVDelete(ApproveModel, command);
                        JournalVoucher_Model DeleteModel = new JournalVoucher_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnDelete";
                        TempData["ListFilterData"] = ApproveModel.ListFilterData1;
                        return RedirectToAction("JournalVoucherDetail", Delete_Model);
                    case "Save":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        JVDate = ApproveModel.JV_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, BrchID, JVDate);
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
                            if (ApproveModel.JV_No == null)
                            {
                                ApproveModel.Command = "Refresh";
                                //ApproveModel.Command = "Add";
                                ApproveModel.TransType = "Refresh";
                                ApproveModel.BtnName = "BtnRefresh";
                                //ApproveModel.BtnName = "BtnAddNew";

                                ApproveModel.DocumentStatus = null;
                                TempData["ModelData"] = ApproveModel;
                                return RedirectToAction("JournalVoucherDetail", "JournalVoucher", ApproveModel);
                            }
                            else
                            {
                                return RedirectToAction("Dblclick", new { JVId = ApproveModel.JV_No, JVDate = ApproveModel.JV_Date, ListFilterData = ApproveModel.ListFilterData1, WF_Status = ApproveModel.WFStatus });

                            }

                        }
                        /*End to chk Financial year exist or not*/
                        SaveJVoucDetail(ApproveModel);
                        if (ApproveModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (ApproveModel.Message == "N")
                        {
                            ViewBag.MenuPageName = getDocumentName();
                            ViewBag.GlAccountDetails = ViewData["VouDetails"];
                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                            DataTable dtVouDetails = ViewData["VouDetails"] as DataTable;

                            decimal TotalDr = 0;
                            decimal TotalCr = 0;
                            for (int i = 0; i < dtVouDetails.Rows.Count; i++)
                            {
                                TotalDr = TotalDr + Convert.ToDecimal(dtVouDetails.Rows[i]["dr_amt_sp"]);
                            }
                            for (int i = 0; i < dtVouDetails.Rows.Count; i++)
                            {
                                TotalCr = TotalCr + Convert.ToDecimal(dtVouDetails.Rows[i]["cr_amt_sp"]);
                            }
                            ViewBag.SpanDrbtAmnt = TotalDr;
                            ViewBag.SpanCrdtAmnt = TotalCr;
                            ViewBag.DiffAmt = TotalDr - TotalCr;
                            ViewBag.CostCenterData = ViewData["CostCenter"];
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                            ApproveModel.BtnName = "BtnAddNew";
                            ApproveModel.Command = "Add";
                            ApproveModel.Message = "N";
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/JournalVoucher/JournalVoucherDetail.cshtml", ApproveModel);
                        }
                        else
                        {
                            TempData["ModelData"] = ApproveModel;
                            UrlModel SaveModel = new UrlModel();
                            SaveModel.bt = ApproveModel.BtnName;
                            SaveModel.JV_num = ApproveModel.JV_num;
                            SaveModel.JV_dt = ApproveModel.JV_dt;
                            SaveModel.tp = ApproveModel.TransType;
                            TempData["ListFilterData"] = ApproveModel.ListFilterData1;
                            return RedirectToAction("JournalVoucherDetail", SaveModel);
                        }
                    case "Approve":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        JVDate = ApproveModel.JV_Date;

                        Msg = commCont.Fin_CheckFinancialYear(CompID, BrchID, JVDate);
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
                            return RedirectToAction("Dblclick", new { JVId = ApproveModel.JV_No, JVDate = ApproveModel.JV_Date, ListFilterData = ApproveModel.ListFilterData1, WF_Status = ApproveModel.WFStatus });

                        }
                        /*End to chk Financial year exist or not*/
                        ApproveJournalVoucherDetails(ApproveModel, ApproveModel.JV_No, ApproveModel.JV_Date, "Direct Approve", "", "", "", "");
                        TempData["ModelData"] = ApproveModel;
                        UrlModel Approve_Model = new UrlModel();
                        Approve_Model.tp = "Update";
                        Approve_Model.JV_num = ApproveModel.JV_num;
                        Approve_Model.JV_dt = ApproveModel.JV_dt;
                        Approve_Model.bt = "BtnToDetailPage";
                        TempData["ListFilterData"] = ApproveModel.ListFilterData1;
                        return RedirectToAction("JournalVoucherDetail", Approve_Model);
                    case "Update":
                        SaveJVoucDetail(ApproveModel);
                        if (ApproveModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = ApproveModel;
                        UrlModel Save_Model = new UrlModel();
                        Save_Model.bt = ApproveModel.BtnName;
                        Save_Model.JV_num = ApproveModel.JV_No;
                        Save_Model.JV_dt = ApproveModel.JV_Date;
                        Save_Model.tp = ApproveModel.TransType;
                        TempData["ListFilterData"] = ApproveModel.ListFilterData1;
                        return RedirectToAction("JournalVoucherDetail", Save_Model);
                    case "Refresh":
                        JournalVoucher_Model RefreshModel = new JournalVoucher_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh_Model = new UrlModel();
                        refesh_Model.tp = "Save";
                        refesh_Model.bt = "BtnRefresh";
                        refesh_Model.Cmd = command;
                        TempData["ListFilterData"] = ApproveModel.ListFilterData1;
                        return RedirectToAction("JournalVoucherDetail", refesh_Model);
                    case "Print":
                        return GenratePdfFile(ApproveModel);
                    case "BacktoList":
                        JournalVoucher_Model ApproveModels = new JournalVoucher_Model();
                        ApproveModels.WF_Status = ApproveModel.WF_Status1;
                        TempData["ListFilterData"] = ApproveModel.ListFilterData1;
                        return RedirectToAction("JournalVoucher", "JournalVoucher", ApproveModels);
                }
                return RedirectToAction("JournalVoucherDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult BindReplicateWithlist(JournalVoucher_Model JournalVoucher)
        {
            DataSet dt = new DataSet();
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                string SarchValue = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    if (JournalVoucher.item == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = JournalVoucher.item;
                    }
                    DataSet ProductList = _journalVoucher_ISERVICES.getReplicateWith(CompID, BrchID, "JV", SarchValue);
                    if (ProductList.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[0].Rows.Count; i++)
                        {
                            string vou_no = ProductList.Tables[0].Rows[i]["vou_no"].ToString();
                            string vou_dt = ProductList.Tables[0].Rows[i]["vou_dt"].ToString();
                            ItemList.Add(vou_no + ',' + vou_dt, vou_dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReplicateWithVouNumber(string Vou_no, string vou_dt)
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
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _journalVoucher_ISERVICES.GetReplicateWithItemdata(CompID, BrchID, Vou_no, vou_dt, "JV");
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult SaveJVoucDetail(JournalVoucher_Model ApproveModel)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            try
            {
                if (ApproveModel.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }
                    DataTable JVHeader = new DataTable();
                    DataTable JVAccountDetails = new DataTable();
                    DataTable dtheader = new DataTable();
                    DataTable JVCostCenterDetails = new DataTable();

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
                    //dtheader.Columns.Add("vou_amt", typeof(string));
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
                    dtrowHeader["TransType"] = ApproveModel.TransType;
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    dtrowHeader["comp_id"] = Session["CompId"].ToString();
                    dtrowHeader["br_id"] = Session["BranchId"].ToString();
                    dtrowHeader["user_id"] = Session["UserId"].ToString();
                    dtrowHeader["vou_type"] = "JV";
                    dtrowHeader["vou_no"] = ApproveModel.JV_No;
                    dtrowHeader["vou_dt"] = ApproveModel.JV_Date;
                    dtrowHeader["src_doc"] = "D";
                    dtrowHeader["src_doc_no"] = null;
                    dtrowHeader["src_doc_dt"] = null;
                    dtrowHeader["vou_amt"] = ApproveModel.JVT_amount;
                    dtrowHeader["remarks"] = "";
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
                    JVHeader = dtheader;

                    DataTable dtAccount = new DataTable();

                    dtAccount.Columns.Add("acc_id", typeof(string));
                    dtAccount.Columns.Add("acc_type", typeof(int));
                    dtAccount.Columns.Add("curr_id", typeof(int));
                    //dtAccount.Columns.Add("conv_rate", typeof(string));
                    //dtAccount.Columns.Add("dr_amt_bs", typeof(string));
                    //dtAccount.Columns.Add("cr_amt_bs", typeof(string));
                    //dtAccount.Columns.Add("dr_amt_sp", typeof(string));
                    //dtAccount.Columns.Add("cr_amt_sp", typeof(string));
                    dtAccount.Columns.Add("conv_rate", typeof(string));
                    dtAccount.Columns.Add("dr_amt_bs", typeof(string));
                    dtAccount.Columns.Add("cr_amt_bs", typeof(string));
                    dtAccount.Columns.Add("dr_amt_sp", typeof(string));
                    dtAccount.Columns.Add("cr_amt_sp", typeof(string));
                    dtAccount.Columns.Add("narr", typeof(string));
                    dtAccount.Columns.Add("seq_no", typeof(int));
                    JArray jObject = JArray.Parse(ApproveModel.GlAccountDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtAccount.NewRow();

                        dtrowLines["acc_id"] = jObject[i]["acc_id"].ToString();
                        dtrowLines["acc_type"] = jObject[i]["acc_type"].ToString();
                        dtrowLines["curr_id"] = jObject[i]["curr_id"].ToString();
                        var a = jObject[i]["conv_rate"].ToString();
                        dtrowLines["conv_rate"] = jObject[i]["conv_rate"].ToString();
                        if (jObject[i]["dr_amt_bs"].ToString() == "")
                        {
                            dtrowLines["dr_amt_bs"] = 0;
                            dtrowLines["dr_amt_sp"] = 0;
                        }
                        else
                        {
                            dtrowLines["dr_amt_bs"] = jObject[i]["dr_amt_bs"].ToString();
                            dtrowLines["dr_amt_sp"] = jObject[i]["dr_amt_sp"].ToString();
                        }
                        if (jObject[i]["cr_amt_bs"].ToString() == "")
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
                    JVAccountDetails = dtAccount;
                    ViewData["VouDetails"] = DtVouDetails(jObject);

                    DataTable CC_Details = new DataTable();
                    /**----------------Cost Center Section--------------------*/

                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    //CC_Details.Columns.Add("cc_amt", typeof(string));
                    CC_Details.Columns.Add("cc_amt", typeof(string));


                    JArray JAObj = JArray.Parse(ApproveModel.CC_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = CC_Details.NewRow();

                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                        CC_Details.Rows.Add(dtrowLines);
                    }
                    JVCostCenterDetails = CC_Details;
                    ViewData["CostCenter"] = dtCostCenter(JAObj);

                    /**----------------Cost Center Section End--------------------*/

                    /*-----------------Attachment Section Start------------------------*/
                    DataTable JVAttachments = new DataTable();
                    DataTable jvdtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as JournalVoucher_model;
                    TempData["IMGDATA"] = null;
                    if (ApproveModel.attatchmentdetail != null)
                    {
                        if (attachData != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                //jvdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                jvdtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                jvdtAttachment.Columns.Add("id", typeof(string));
                                jvdtAttachment.Columns.Add("file_name", typeof(string));
                                jvdtAttachment.Columns.Add("file_path", typeof(string));
                                jvdtAttachment.Columns.Add("file_def", typeof(char));
                                jvdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (ApproveModel.AttachMentDetailItmStp != null)
                            {
                                //jvdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                jvdtAttachment = ApproveModel.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                jvdtAttachment.Columns.Add("id", typeof(string));
                                jvdtAttachment.Columns.Add("file_name", typeof(string));
                                jvdtAttachment.Columns.Add("file_path", typeof(string));
                                jvdtAttachment.Columns.Add("file_def", typeof(char));
                                jvdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(ApproveModel.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in jvdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = jvdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((ApproveModel.JV_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = ApproveModel.JV_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                jvdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (ApproveModel.TransType == "Update")
                        {
                            string AttachmentFilePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string JV_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((ApproveModel.JV_No).ToString()))
                                {
                                    JV_CODE = (ApproveModel.JV_No).ToString();

                                }
                                else
                                {
                                    JV_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + JV_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in jvdtAttachment.Rows)
                                    {
                                        string drImgPath = dr["file_path"].ToString();
                                        if (drImgPath == fielpath)
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
                        JVAttachments = jvdtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/

                    SaveMessage = _journalVoucher_ISERVICES.InsertUpdateJV(JVHeader, JVAccountDetails, JVAttachments, JVCostCenterDetails);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 24-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //ApproveModel.Message = "Financial Year not Exist";
                        ApproveModel.BtnName = "BtnRefresh";
                        //TempData["ModelData"] = ApproveModel;
                        return RedirectToAction("JournalVoucherDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //ApproveModel.Message = "Financial Book Closing";
                        ApproveModel.BtnName = "BtnRefresh";
                        //TempData["ModelData"] = ApproveModel;
                        return RedirectToAction("JournalVoucherDetail");
                    }
                    else
                    {
                        if (SaveMessage == "N")
                        {
                            ApproveModel.Message = "N";
                            return RedirectToAction("JournalVoucherDetail");
                        }
                        else
                        {
                            string JVNo = SaveMessage.Split(',')[1].Trim();
                            string JV_Number = JVNo.Replace("/", "");
                            string Message = SaveMessage.Split(',')[0].Trim();
                            string JVDate = SaveMessage.Split(',')[2].Trim();
                            if (Message == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message.Replace("_", " ") + " " + JVNo + " in " + PageName;
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                ApproveModel.Message = Message.Split(',')[0].Replace("_", "");
                                return RedirectToAction("ProductCatalougeDetail");
                            }
                            /*-----------------Attachment Section Start------------------------*/
                            if (Message == "Save")

                            {
                                string Guid = "";
                                if (attachData != null)
                                {
                                    if (attachData.Guid != null)
                                    {
                                        Guid = attachData.Guid;
                                    }
                                }
                                string guid = Guid;
                                var comCont = new CommonController(_Common_IServices);
                                comCont.ResetImageLocation(CompID, BrchID, guid, PageName, JV_Number, ApproveModel.TransType, JVAttachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{
                                //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in JVAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = CompID + BrchID + JV_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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

                                //}

                            }
                            /*-----------------Attachment Section End------------------------*/

                            if (Message == "Update" || Message == "Save")
                            {
                                //Session["Message"] = "Save";
                                //Session["Command"] = "Update";
                                //Session["JV_No"] = JVNo;
                                //Session["JV_Date"] = JVDate;
                                //Session["TransType"] = "Update";
                                //Session["AppStatus"] = 'D';
                                //Session["BtnName"] = "BtnSave";
                                ApproveModel.Message = "Save";
                                //ApproveModel.Command = "Update";
                                ApproveModel.JV_num = JVNo;
                                ApproveModel.JV_dt = JVDate;
                                ApproveModel.TransType = "Update";
                                ApproveModel.BtnName = "BtnSave";
                            }
                            return RedirectToAction("JournalVoucherDetail");

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
                    string FinalStatus = CheckSVForCancellation(ApproveModel, ApproveModel.JV_No, ApproveModel.JV_Date.ToString());
                    if (FinalStatus == "Used" || FinalStatus == "Used1")
                    {
                        ApproveModel.Message = FinalStatus;
                        ApproveModel.Message1 = FinalStatus;
                        ApproveModel.JV_num = ApproveModel.JV_No;
                        ApproveModel.JV_dt = ApproveModel.JV_Date;
                        TempData["ModelData"] = ApproveModel;
                    }
                    else if (FinalStatus == "AutoGen")
                    {
                        ApproveModel.Message = FinalStatus;
                        ApproveModel.Message1 = FinalStatus;
                        ApproveModel.JV_num = ApproveModel.JV_No;
                        ApproveModel.JV_dt = ApproveModel.JV_Date;
                        TempData["ModelData"] = ApproveModel;
                    }
                    else
                    {
                        string br_id = Session["BranchId"].ToString();
                        ApproveModel.Create_by = UserID;
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        String SaveMessage1 = _journalVoucher_ISERVICES.JVCancel(ApproveModel, CompID, br_id, mac_id);
                        string MRSNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        ApproveModel.Message = "Cancelled";
                        // ApproveModel.Command = "Update";
                        ApproveModel.JV_num = ApproveModel.JV_No;
                        ApproveModel.JV_dt = ApproveModel.JV_Date;
                        ApproveModel.TransType = "Update";
                        ApproveModel.BtnName = "BtnRefresh";
                    }

                    return RedirectToAction("JournalVoucherDetail");
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
                    if (ApproveModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (ApproveModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = ApproveModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                throw ex;
                // return View("~/Views/Shared/Error.cshtml");  //Commented by Nitesh 23-10-2023 17:20 for error page
            }
        }
        public DataTable DtVouDetails(JArray jObject)
        {
            DataTable dtAccount = new DataTable();

            dtAccount.Columns.Add("acc_id", typeof(string));
            dtAccount.Columns.Add("acc_name", typeof(string));
            dtAccount.Columns.Add("acc_group_name", typeof(string));
            dtAccount.Columns.Add("acc_grp_id", typeof(string));
            dtAccount.Columns.Add("acc_type", typeof(int));
            dtAccount.Columns.Add("curr_id", typeof(int));
            dtAccount.Columns.Add("conv_rate", typeof(string));
            dtAccount.Columns.Add("dr_amt_bs", typeof(decimal));
            dtAccount.Columns.Add("cr_amt_bs", typeof(decimal));
            dtAccount.Columns.Add("dr_amt_sp", typeof(string));
            dtAccount.Columns.Add("cr_amt_sp", typeof(string));
            dtAccount.Columns.Add("narr", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtAccount.NewRow();

                dtrowLines["acc_id"] = jObject[i]["acc_id"].ToString();
                dtrowLines["acc_name"] = jObject[i]["acc_name"].ToString();
                dtrowLines["acc_group_name"] = jObject[i]["acc_group_name"].ToString();
                dtrowLines["acc_grp_id"] = jObject[i]["acc_grp_id"].ToString();
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
            //CC_Details.Columns.Add("cc_amt", typeof(string));
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

        //-----For List Page------//
        public ActionResult SearchJVDetail(string Fromdate, string Todate, string Status)
        {
            try
            {
                JournalVoucher_Model ApproveModel = new JournalVoucher_Model();
                //Session.Remove("WF_status");
                ApproveModel.WF_Status = null;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataTable dt1 = _journalVoucher_ISERVICES.GetJVDetailList(CompID, BrchID, Fromdate, Todate, Status, UserID, "", "", "0").Tables[0];

                ViewBag.JVDetailsList = dt1;
                // Session["JVSearch"] = "JV_Search";
                ApproveModel.JVSearch = "JV_Search";
                ViewBag.VBRoleList = GetRoleList();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialJournalVoucherList.cshtml", ApproveModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        //------ListPage End----//
        public FileResult JournalVoucherExporttoExcelDt(string Fromdate, string Todate, string Status,string searchValue)
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataTable dt = new DataTable();
                if (searchValue == null)
                    searchValue = "0";
                DataTable dt1 = _journalVoucher_ISERVICES.GetJVDetailList(CompID, BrchID, Fromdate, Todate, Status, UserID, "", "", searchValue).Tables[0];
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

                if (dt1.Rows.Count > 0)
                {
                    int rowno = 0;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        DataRow dtrowLines = dt.NewRow();
                        dtrowLines["Sr.No"] = rowno + 1;
                        dtrowLines["GL Voucher Number"] = dr["vou_no"].ToString();
                        dtrowLines["GL Voucher Date"] = dr["JVDate"].ToString();
                        dtrowLines["Amount"] = dr["vou_amt"].ToString();
                        dtrowLines["Status"] = dr["Status"].ToString();
                        dtrowLines["Created By"] = dr["create_by"].ToString();
                        dtrowLines["Created On"] = dr["CreateDate"].ToString();
                        dtrowLines["Approved By"] = dr["app_by"].ToString();
                        dtrowLines["Approved On"] = dr["ApproveDate"].ToString();
                        dtrowLines["Amended By"] = dr["mod_by"].ToString();
                        dtrowLines["Amended On"] = dr["ModifyDate"].ToString();
                        dt.Rows.Add(dtrowLines);
                        rowno = rowno + 1;
                    }
                }
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("JournalVoucher", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private ActionResult JVDelete(JournalVoucher_Model ApproveModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string JVNO = ApproveModel.JV_No;
                string JV_NO = JVNO.Replace("/", "");

                string Message = _journalVoucher_ISERVICES.JVDelete(ApproveModel, CompID, br_id);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(JV_NO))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, JV_NO, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["JV_No"] = "";
                //ApproveModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("JournalVoucherDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public string CheckSVForCancellation(JournalVoucher_Model _JV_Model, string DocNo, string DocDate)
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
                DataSet Deatils = _journalVoucher_ISERVICES.CheckJVDetail(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
                if (Deatils.Tables[1].Rows.Count > 0)
                {
                    str = "AutoGen";
                }
                if (str != "" && str != null)
                {
                    _JV_Model.JV_No = DocNo;
                    _JV_Model.JV_Date = DocDate;
                    _JV_Model.TransType = "Update";
                    _JV_Model.BtnName = "BtnToDetailPage";
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
        //----Used for Forward and Workflow-----//
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
        //----Used For only Forward button,used in JS----//
        public ActionResult ApproveJOVDetails(string JVNo, string JVDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
            string Msg = string.Empty;
            var commCont = new CommonController(_Common_IServices);
            Msg = commCont.Fin_CheckFinancialYear(CompID, BrchID, JVDate);
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
                return RedirectToAction("Dblclick", new { JVId = JVNo, JVDate = JVDate, ListFilterData = ListFilterData1, WF_Status = WF_Status1 });

            }
            /*End to chk Financial year exist or not*/
            JournalVoucher_Model ApproveModel = new JournalVoucher_Model();
            ApproveJournalVoucherDetails(ApproveModel, JVNo, JVDate, A_Status, A_Level, A_Remarks, "", ListFilterData1);
            UrlModel urlref = new UrlModel();
            ApproveModel.TransType = "Update";
            ApproveModel.JV_num = ApproveModel.JV_num;
            ApproveModel.JV_dt = ApproveModel.JV_dt;
            ApproveModel.Message = "Approved";
            ApproveModel.BtnName = "BtnToDetailPage";
            ApproveModel.Command = "Refresh";

            try
            {
                //string fileName = "SHP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                string fileName = "JournalVoucher_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                var filePath = SavePdfDocToSendOnEmailAlert(JVNo, JVDate, fileName, DocumentMenuId,"AP");
                _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, JVNo, "AP", Session["UserId"].ToString(), "", filePath);
            }
            catch (Exception exMail)
            {
                ApproveModel.Message = "ErrorInMail";
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exMail);
            }
            ApproveModel.Message = ApproveModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
            if (WF_Status1 != null && WF_Status1 != "")
            {
                ApproveModel.WF_Status1 = WF_Status1;
                urlref.wf = WF_Status1;
            }
            TempData["ModelData"] = ApproveModel;

            urlref.tp = "Update";
            urlref.JV_num = ApproveModel.JV_num;
            urlref.JV_dt = ApproveModel.JV_dt;
            urlref.bt = ApproveModel.BtnName;
            urlref.Cmd = ApproveModel.Command;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("JournalVoucherDetail", urlref);
        }
        public ActionResult ApproveJournalVoucherDetails(JournalVoucher_Model ApproveModel, string JVNo, string JVDate, string A_Status, string A_Level, string A_Remarks, string Flag, string ListFilterData1)
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
                    UserID = Session["userid"].ToString();
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string SaveMessage = _journalVoucher_ISERVICES.InsertJornlVoucApproveDetails(JVNo, JVDate, CompID, BrchID, DocumentMenuId, UserID, mac_id, A_Status, A_Level, A_Remarks, Flag);
                JVNo = SaveMessage.Split(',')[1].Trim();
                string Message = SaveMessage.Split(',')[0].Trim();
                JVDate = SaveMessage.Split(',')[2].Trim();
                if (!string.IsNullOrEmpty(JVNo))
                {
                    //Session["JV_No"] = JVNo;
                    ApproveModel.JV_num = JVNo;
                }
                if (Message == "Approved")
                {
                    // Session["Message"] = "Approved";
                    ApproveModel.Message = "Approved";
                }
                if (Message == "Cancelled")
                {
                    try
                    {
                        //string fileName = "SHP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "JournalVoucher_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(JVNo, JVDate, fileName, DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, JVNo, "C", Session["UserId"].ToString(), "", filePath);
                    }
                    // Session["Message"] = "Cancelled";
                    catch (Exception exMail)
                    {
                        ApproveModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    //ApproveModel.Message = "Cancelled";
                    ApproveModel.Message = ApproveModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                }
                if (!string.IsNullOrEmpty(JVDate))
                {
                    //Session["JV_Date"] = JVDate;
                    ApproveModel.JV_dt = JVDate;
                }
                ApproveModel.TransType = "Update";
                ApproveModel.BtnName = "BtnToDetailPage";
                ApproveModel.Command = "Refresh";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("JournalVoucherDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData,string Mailerror)
        {
            // Session["Message"] = "";
            JournalVoucher_Model _DebitNote_Model = new JournalVoucher_Model();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split(',');
            _DebitNote_Model.JV_num = a[0].Trim();
            _DebitNote_Model.JV_dt = a[1].Trim();
            _DebitNote_Model.TransType = "Update";
            _DebitNote_Model.BtnName = "BtnToDetailPage";
            _DebitNote_Model.Message = Mailerror;
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                _DebitNote_Model.WF_Status1 = a[2].Trim();
                urlModel.wf = a[2].Trim();
            }
            urlModel.bt = "D";
            urlModel.JV_num = _DebitNote_Model.JV_num;
            urlModel.JV_dt = _DebitNote_Model.JV_dt;
            urlModel.tp = "Update";
            TempData["ModelData"] = _DebitNote_Model;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("JournalVoucherDetail", urlModel);
        }

        //----Workflow End----//
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


        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string Title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                JournalVoucher_model attachment_Model = new JournalVoucher_model();
                //string TransType = "";
                //string JVCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["JV_No"] != null)
                //{
                //    JVCode = Session["JV_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    //JVCode = gid.ToString();
                    DocNo = gid.ToString();
                }
                //JVCode = JVCode.Replace("/", "");
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = JVCode;
                attachment_Model.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    attachment_Model.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    attachment_Model.AttachMentDetailItmStp = null;
                }
                TempData["IMGDATA"] = attachment_Model;
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

        public ActionResult GetCstCntrtype(string Flag, string Disableflag, string CC_rowdata, string TotalAmt = null, string Doc_ID = null)
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
                    BrchID = Session["BranchId"].ToString();
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


                DataSet ds = _Common_IServices.GetCstCntrData(CompID, BrchID, "0", Flag);

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
                ViewBag.CCTotalAmt = TotalAmt;//add by sm 06-12-2024
                ViewBag.DocId = Doc_ID;//add by sm 06-12-2024
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
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds = _Common_IServices.GetCstCntrData(CompID, BrchID, CCtypeid, "ccname");
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

        public FileResult GenratePdfFile(JournalVoucher_Model _Model)
        {
            return File(GetPdfData(_Model.JV_No, _Model.JV_Date), "application/pdf", "JournalVoucher.pdf");
        }
        public byte[] GetPdfData(string jvNo, string jvDate)
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
                DataSet Deatils = _journalVoucher_ISERVICES.GetGLVoucherPrintDeatils(CompID, BrchID, jvNo, jvDate, "JV");
                ViewBag.PageName = "JV";
                ViewBag.Title = "Journal Voucher";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                ViewBag.DocumentMenuId = DocumentMenuId;
                //string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                //string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                //if (Request.Url.Host == localIp)
                //    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                //else
                //    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                //ViewBag.FLogoPath = serverUrl + Deatils.Tables[0].Rows[0]["logo"].ToString().Trim();

                /* Added by Suraj Maurya on 17-02-2025 to add logo*/
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                /* Added by Suraj Maurya on 17-02-2025 to add logo End */

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
                      
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);/*Add by Hina sharma on 16-10-2024 */
                                draftimg.SetAbsolutePosition(0, 160);
                                draftimg.ScaleAbsolute(580f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F" || ViewBag.DocStatus == "C")/*Add by Hina sharma on 16-10-2024 */
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
