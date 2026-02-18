
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.CreditNote;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.CreditNote;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.CreditNote
{
    public class CreditNoteController : Controller
    {
        string CompID, language = String.Empty;
        string Comp_ID, Br_ID, FromDate ,Language, title, UserID = String.Empty;
        string DocumentMenuId = "105104115135";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        CreditNote_IService _CreditNote_IService;
        DataTable dt;
        string flag = "";
        string Entitytype = "1";
       List<VouList> _EntityList;

        public CreditNoteController(Common_IServices _Common_IServices, CreditNote_IService _CreditNote_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._CreditNote_IService = _CreditNote_IService;
        }
        // GET: ApplicationLayer/CreditNote
        public ActionResult CreditNote(CreditNoteList_Model _CreditNoteList_Model)
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
           // CreditNoteList_Model _CreditNoteList_Model = new CreditNoteList_Model();
            GetStatusList(_CreditNoteList_Model);
            //GetAutoCompleteEntityDetail(_CreditNoteList_Model, Entitytype," All");
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _CreditNoteList_Model.entity_type = a[0].Trim();
                _CreditNoteList_Model.entity_id = a[1].Trim();
                _CreditNoteList_Model.VouFromDate = a[2].Trim();
                _CreditNoteList_Model.VouToDate = a[3].Trim();
                _CreditNoteList_Model.Status = a[4].Trim();
                if (_CreditNoteList_Model.Status == "0")
                {
                    _CreditNoteList_Model.Status = null;
                }
                _CreditNoteList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                var Entitytype = _CreditNoteList_Model.entity_type;
                if (Entitytype != "0")
                {
                    List<EntityAccList> _EntityAccNameList = new List<EntityAccList>();
                    dt = GetEntity1(_CreditNoteList_Model, Entitytype, " All");
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityAccList EntityName = new EntityAccList();
                        EntityName.entity_acc_id = dr["entity_acc_id"].ToString();
                        EntityName.entity_acc_name = dr["entity_acc_name"].ToString();
                        _EntityAccNameList.Add(EntityName);

                    }
                    _EntityAccNameList.Insert(0, new EntityAccList() { entity_acc_id = "0", entity_acc_name = "---Select---" });
                    _CreditNoteList_Model.EntityAccNameList = _EntityAccNameList;
                }
                else
                {
                    List<EntityAccList> _EntityAccNameList = new List<EntityAccList>();
                    _EntityAccNameList.Insert(0, new EntityAccList() { entity_acc_id = "0", entity_acc_name = "---Select---" });
                    _CreditNoteList_Model.EntityAccNameList = _EntityAccNameList;
                }
            }
            _CreditNoteList_Model.VoucherList = GetCreditNoteListAll(_CreditNoteList_Model);
            if (_CreditNoteList_Model.VouFromDate != null)
            {
                _CreditNoteList_Model.FromDate = _CreditNoteList_Model.VouFromDate;
            }
            else
            {
                List<EntityAccList> _EntityAccNameList = new List<EntityAccList>();
                _EntityAccNameList.Insert(0, new EntityAccList() { entity_acc_id = "0", entity_acc_name = "---Select---" });
                _CreditNoteList_Model.EntityAccNameList = _EntityAccNameList;
                //_CreditNoteList_Model.FromDate = startDate;
            }
            //_CreditNoteList_Model.FromDate = startDate;
           
            ViewBag.VBRoleList = GetRoleList();
            ViewBag.MenuPageName = getDocumentName();
            _CreditNoteList_Model.Title = title;
            CommonPageDetails();
            //Session["VouSearch"] = "0";
            _CreditNoteList_Model.VouSearch = "0";
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CreditNote/CreditNoteList.cshtml", _CreditNoteList_Model);
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
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
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
        public DataTable GetEntity1(CreditNoteList_Model CreditNoteList_Model, string Entitytype, string flag)
        {
            string Acc_Name = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(CreditNoteList_Model.EntityName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = CreditNoteList_Model.EntityName;
                }
                Br_ID = Session["BranchId"].ToString();

                DataTable dt = _CreditNote_IService.AutoGetEntityList1(Comp_ID, Acc_Name, Br_ID, Entitytype, flag);
                //DataTable dt = _DebitNote_IService.GetEntity(CompID, BrchID, EntityType);
                //DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult AddCreditNoteDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            CreditNote_Model AddNewModel = new CreditNote_Model();
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
            //    return RedirectToAction("CreditNote", AddNewModel);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("CreditNote", AddNewModel);
            //}
            /*End to chk Financial year exist or not*/
            AddNewModel.Command = "Add";
            AddNewModel.TransType = "Save";
            AddNewModel.BtnName = "BtnAddNew";
            AddNewModel.DocumentStatus = "D";
            TempData["ModelData"] = AddNewModel;
            UrlModel _urlModel = new UrlModel();
            _urlModel.bt = "BtnAddNew";
            _urlModel.Cmd = "Add";
            _urlModel.tp = "Save";
            TempData["ListFilterData"] = null;
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("CreditNoteDetail", "CreditNote", _urlModel);
        }
        public ActionResult CreditNoteDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/

                CreditNote_Model _CreditNote_Model = new CreditNote_Model();
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
                var _CreditNote_Model1 = TempData["ModelData"] as CreditNote_Model;
                if(_CreditNote_Model1 != null)
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    GetAutoCompleteGLDetail(_CreditNote_Model1, Entitytype, " ----Select----");
                    _CreditNote_Model1.entity_type = "1";
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
                    _CreditNote_Model1.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _CreditNote_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_CreditNote_Model1.TransType == "Update" || _CreditNote_Model1.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["CreditNoteNo"].ToString();
                        //string VouDt = Session["CreditNoteDate"].ToString();
                        string VouType = "CN";
                        string VouNo = _CreditNote_Model1.CreditNoteNo;
                        string VouDt = _CreditNote_Model1.CreditNoteDate;
                        DataSet ds = _CreditNote_IService.GetCreditNoteDetail(VouNo, VouDt, VouType, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _CreditNote_Model1.HdnBillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        _CreditNote_Model1.entity_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _CreditNote_Model1.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _CreditNote_Model1.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _CreditNote_Model1.entity_type = ds.Tables[0].Rows[0]["acc_type"].ToString();
                        _CreditNote_Model1.entity_acc_Name = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _CreditNote_Model1.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        _CreditNote_Model1.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _CreditNote_Model1.Bill_Date = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        _CreditNote_Model1.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _CreditNote_Model1.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _CreditNote_Model1.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _CreditNote_Model1.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        _CreditNote_Model1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _CreditNote_Model1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _CreditNote_Model1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _CreditNote_Model1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _CreditNote_Model1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _CreditNote_Model1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _CreditNote_Model1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _CreditNote_Model1.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _CreditNote_Model1.SrcDocNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _CreditNote_Model1.SrcDocDate= ds.Tables[0].Rows[0]["src_doc_dt"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _CreditNote_Model1.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _CreditNote_Model1.DocumentStatus = Statuscode;
                        GetAutoCompleteGLDetail(_CreditNote_Model1, _CreditNote_Model1.entity_type, " ----Select----");
                        if (_CreditNote_Model1.Status == "C")
                        {
                            _CreditNote_Model1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _CreditNote_Model1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _CreditNote_Model1.BtnName = "Refresh";
                        }
                        else
                        {
                            _CreditNote_Model1.CancelFlag = false;
                        }

                        _CreditNote_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _CreditNote_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _CreditNote_Model1.Command != "Edit")
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
                                    _CreditNote_Model1.BtnName = "Refresh";
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
                                        _CreditNote_Model1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _CreditNote_Model1.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _CreditNote_Model1.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _CreditNote_Model1.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                       // _CreditNote_Model1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CreditNote_Model1.BtnName = "BtnToDetailPage";
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
                                        _CreditNote_Model1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CreditNote_Model1.BtnName = "BtnToDetailPage";
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
                                    _CreditNote_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CreditNote_Model1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _CreditNote_Model1.BtnName = "Refresh";
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
                        _CreditNote_Model1.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.CostCenterData = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CreditNote/CreditNoteDetail.cshtml", _CreditNote_Model1);
                    }
                    else
                    {
                        //DateTime dtnow = DateTime.Now;
                        //string startDate = new DateTime(dtnow.Year, dtnow.Month,dtnow.Day).ToString("yyyy-MM-dd");
                        //if (_CreditNote_Model1.Command == "Refresh")
                        //{
                        //    _CreditNote_Model1.Bill_Date = startDate;
                        //    _CreditNote_Model1.Vou_Date = startDate;
                        //}
                      
                        ViewBag.MenuPageName = getDocumentName();
                        _CreditNote_Model1.Title = title;
                        ViewBag.MenuPageName = getDocumentName();
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CreditNote/CreditNoteDetail.cshtml", _CreditNote_Model1);
                    }
                }
                else
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _CreditNote_Model.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _CreditNote_Model.BtnName = _urlModel.bt;
                        }
                        _CreditNote_Model.CreditNoteNo = _urlModel.CNO;
                        _CreditNote_Model.CreditNoteDate = _urlModel.CDT;
                        _CreditNote_Model.Command = _urlModel.Cmd;
                        _CreditNote_Model.TransType = _urlModel.tp;
                        _CreditNote_Model.WF_Status1 = _urlModel.wf;
                        _CreditNote_Model.DocumentStatus = _urlModel.DMS;
                    }
                    /* Add by Hina on 23-02-2024 to Refresh Page*/
                    if (_CreditNote_Model.TransType == null)
                    {
                        _CreditNote_Model.BtnName = "Refresh";
                        _CreditNote_Model.Command = "Refresh";
                        _CreditNote_Model.TransType = "Refresh";

                    }
                    /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var VouDate = _CreditNote_Model.CreditNoteDate;
                    //var VouDate = "";

                    //if (_CreditNote_Model.CreditNoteDate != null)
                    //{
                    //    VouDate = _CreditNote_Model.CreditNoteDate;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    _CreditNote_Model.CreditNoteDate = CurrentDate;
                    //    _CreditNote_Model.Vou_Date = CurrentDate;
                    //    VouDate = _CreditNote_Model.CreditNoteDate;
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
                    GetAutoCompleteGLDetail(_CreditNote_Model, Entitytype, " ----Select----");
                    _CreditNote_Model.entity_type = "1";
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
                    _CreditNote_Model.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _CreditNote_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_CreditNote_Model.TransType == "Update" || _CreditNote_Model.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["CreditNoteNo"].ToString();
                        //string VouDt = Session["CreditNoteDate"].ToString();
                        string VouType = "CN";
                        string VouNo = _CreditNote_Model.CreditNoteNo;
                        string VouDt = _CreditNote_Model.CreditNoteDate;
                        DataSet ds = _CreditNote_IService.GetCreditNoteDetail(VouNo, VouDt, VouType, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _CreditNote_Model.HdnBillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        _CreditNote_Model.entity_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _CreditNote_Model.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _CreditNote_Model.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _CreditNote_Model.entity_type = ds.Tables[0].Rows[0]["acc_type"].ToString();
                        _CreditNote_Model.entity_acc_Name = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _CreditNote_Model.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        _CreditNote_Model.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _CreditNote_Model.Bill_Date = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        _CreditNote_Model.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _CreditNote_Model.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _CreditNote_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _CreditNote_Model.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        _CreditNote_Model.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _CreditNote_Model.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _CreditNote_Model.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _CreditNote_Model.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _CreditNote_Model.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _CreditNote_Model.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _CreditNote_Model.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _CreditNote_Model.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _CreditNote_Model.SrcDocNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _CreditNote_Model.SrcDocDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _CreditNote_Model.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _CreditNote_Model.DocumentStatus = Statuscode;
                        GetAutoCompleteGLDetail(_CreditNote_Model, _CreditNote_Model.entity_type, " ----Select----");
                        if (_CreditNote_Model.Status == "C")
                        {
                            _CreditNote_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _CreditNote_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _CreditNote_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _CreditNote_Model.CancelFlag = false;
                        }

                        _CreditNote_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _CreditNote_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _CreditNote_Model.Command != "Edit")
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
                                    _CreditNote_Model.BtnName = "Refresh";
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
                                        _CreditNote_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _CreditNote_Model.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _CreditNote_Model.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _CreditNote_Model.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        //_CreditNote_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CreditNote_Model.BtnName = "BtnToDetailPage";
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
                                        _CreditNote_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CreditNote_Model.BtnName = "BtnToDetailPage";
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
                                    _CreditNote_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CreditNote_Model.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _CreditNote_Model.BtnName = "Refresh";
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
                        _CreditNote_Model.Title = title;
                        ViewBag.VouDetails = ds.Tables[1];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.CostCenterData = ds.Tables[6];
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CreditNote/CreditNoteDetail.cshtml", _CreditNote_Model);
                    }
                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _CreditNote_Model.Title = title;
                        //DateTime dtnow = DateTime.Now;
                        //string startDate = new DateTime(dtnow.Year, dtnow.Month,dtnow.Day).ToString("yyyy-MM-dd");
                        //if (_CreditNote_Model.Command == "Refresh")
                        //{
                        //    _CreditNote_Model.Bill_Date = startDate;
                        //    _CreditNote_Model.Vou_Date = startDate;
                        //}
                        ViewBag.MenuPageName = getDocumentName();
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CreditNote/CreditNoteDetail.cshtml", _CreditNote_Model);
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult CreditNoteBtnCommands(CreditNote_Model _CreditNote_Model, string Vou_No, string command)
        {
            try
            {
                /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                if (_CreditNote_Model.DeleteCommand == "Delete")
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
                        CreditNote_Model adddnew = new CreditNote_Model();
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
                        //    return RedirectToAction("CreditNoteDetail", "CreditNote", adddnew);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("CreditNoteDetail", "CreditNote", adddnew);
                        //}
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("CreditNoteDetail", "CreditNote", NewModel);

                    case "Edit":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt = _CreditNote_Model.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_CreditNote_Model.Status == "A"|| _CreditNote_Model.Status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("EditVou", new { VouNo = _CreditNote_Model.Vou_No, Voudt = _CreditNote_Model.Vou_Date, ListFilterData = _CreditNote_Model.ListFilterData1, WF_Status = _CreditNote_Model.WFStatus });
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        string FinalStatus = CheckPaymentAgainstCreditNote(_CreditNote_Model, _CreditNote_Model.Vou_No, _CreditNote_Model.Vou_Date.ToString());
                        if (FinalStatus == "Used" || FinalStatus == "Used1")
                        {
                            _CreditNote_Model.Message= FinalStatus;
                            _CreditNote_Model.Message1= FinalStatus;
                            _CreditNote_Model.Command= "Refresh";

                            TempData["ModelData"] = _CreditNote_Model;
                        }                       
                        else
                        {
                            _CreditNote_Model.TransType = "Update";
                            _CreditNote_Model.Command = command;
                            _CreditNote_Model.BtnName = "BtnEdit";
                            _CreditNote_Model.CreditNoteNo = _CreditNote_Model.Vou_No;
                            _CreditNote_Model.CreditNoteDate = _CreditNote_Model.Vou_Date;
                            TempData["ModelData"] = _CreditNote_Model;
                            UrlModel EditModel = new UrlModel();
                            EditModel.tp = "Update";
                            EditModel.Cmd = command;
                            EditModel.bt = "BtnEdit";
                            EditModel.CNO = _CreditNote_Model.Vou_No;
                            EditModel.CDT = _CreditNote_Model.Vou_Date;
                            TempData["ListFilterData"] = _CreditNote_Model.ListFilterData1;
                            return RedirectToAction("CreditNoteDetail", EditModel);
                        }
                        UrlModel Edit_Model = new UrlModel();
                        Edit_Model.tp = "Update";                      
                        Edit_Model.bt = "D";
                        Edit_Model.CNO = _CreditNote_Model.CreditNoteNo;
                        Edit_Model.CDT = _CreditNote_Model.CreditNoteDate;
                        TempData["ListFilterData"] = _CreditNote_Model.ListFilterData1;
                        return RedirectToAction("CreditNoteDetail", Edit_Model);

                    case "Delete":
                        _CreditNote_Model.Command = command;
                        Vou_No = _CreditNote_Model.Vou_No;
                        CreditNoteDelete(_CreditNote_Model, command);
                        CreditNote_Model DeleteModel = new CreditNote_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnDelete";
                        TempData["ListFilterData"] = _CreditNote_Model.ListFilterData1;
                        return RedirectToAction("CreditNoteDetail", Delete_Model);


                    case "Save":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _CreditNote_Model.Vou_Date;
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
                            if (_CreditNote_Model.Vou_No == null)
                            {
                                _CreditNote_Model.Command = "Refresh";
                                _CreditNote_Model.TransType = "Refresh";
                                _CreditNote_Model.BtnName = "Refresh";
                                _CreditNote_Model.DocumentStatus = null;
                                TempData["ModelData"] = _CreditNote_Model;
                                return RedirectToAction("CreditNoteDetail", "CreditNote", _CreditNote_Model);
                            }
                            else
                            {
                                return RedirectToAction("EditVou", new { VouNo = _CreditNote_Model.Vou_No, Voudt = _CreditNote_Model.Vou_Date, ListFilterData = _CreditNote_Model.ListFilterData1, WF_Status = _CreditNote_Model.WFStatus });

                            }
                            
                        }
                        /*End to chk Financial year exist or not*/

                        _CreditNote_Model.Command = command;
                        // Session["Command"] = command;
                        SaveCreditNote(_CreditNote_Model);
                        if (_CreditNote_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_CreditNote_Model.Message == "N")
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
                            _CreditNote_Model.currList = _currList;
                            ViewBag.MenuPageName = getDocumentName();
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
                            ViewBag.TotalVouAmt = TotalDr.ToString(ValDigit);
                            ViewBag.DiffAmt = TotalDr - TotalCr;

                            GetAutoCompleteGLDetail(_CreditNote_Model, _CreditNote_Model.entity_type, " ----Select----");


                            ViewBag.CostCenterData = ViewData["CostCenter"];
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                            _CreditNote_Model.BtnName = "BtnAddNew";
                            _CreditNote_Model.Command = "Add";
                            _CreditNote_Model.Message = "N";
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/CreditNote/CreditNoteDetail.cshtml", _CreditNote_Model);
                        }
                        else
                        {
                            TempData["ModelData"] = _CreditNote_Model;
                            UrlModel SaveModel = new UrlModel();
                            SaveModel.bt = _CreditNote_Model.BtnName;
                            SaveModel.CNO = _CreditNote_Model.CreditNoteNo;
                            SaveModel.CDT = _CreditNote_Model.CreditNoteDate;
                            SaveModel.tp = _CreditNote_Model.TransType;
                            TempData["ListFilterData"] = _CreditNote_Model.ListFilterData1;
                            return RedirectToAction("CreditNoteDetail", SaveModel);
                        }

                    //case "Forward":
                    //    return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _CreditNote_Model.Vou_Date;

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
                            return RedirectToAction("EditVou", new { VouNo = _CreditNote_Model.Vou_No, Voudt = _CreditNote_Model.Vou_Date, ListFilterData = _CreditNote_Model.ListFilterData1, WF_Status = _CreditNote_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        Vou_No = _CreditNote_Model.Vou_No;
                        CreditNoteApprove(_CreditNote_Model, _CreditNote_Model.Vou_No, _CreditNote_Model.Vou_Date,"","","","","");
                        TempData["ModelData"] = _CreditNote_Model;
                        UrlModel urlref = new UrlModel();
                        urlref.tp = "Update";
                        urlref.CNO = _CreditNote_Model.CreditNoteNo;
                        urlref.CDT = _CreditNote_Model.CreditNoteDate;
                        urlref.bt = "BtnEdit";
                        TempData["ListFilterData"] = _CreditNote_Model.ListFilterData1;
                        return RedirectToAction("CreditNoteDetail", urlref);

                    case "Refresh":
                        CreditNote_Model RefreshModel = new CreditNote_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _CreditNote_Model.ListFilterData1;
                        return RedirectToAction("CreditNoteDetail", refesh);

                    case "Print":
                        return GenratePdfFile(_CreditNote_Model);
                    case "BacktoList":
                        CreditNoteList_Model _Backtolist = new CreditNoteList_Model();
                        _Backtolist.WF_Status = _CreditNote_Model.WF_Status1;
                        TempData["ListFilterData"] = _CreditNote_Model.ListFilterData1;
                        return RedirectToAction("CreditNote", "CreditNote", _Backtolist);

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

        public ActionResult SaveCreditNote(CreditNote_Model _CreditNote_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (_CreditNote_Model.CancelFlag == false)
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
                    DataTable CreditNoteHeader = new DataTable();
                    DataTable CreditNoteGLDetails = new DataTable();
                    DataTable CRCostCenterDetails = new DataTable();
                    DataTable DebitNoteBillAdj = new DataTable();
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
                    if (_CreditNote_Model.Vou_No != null)
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
                    dtrowHeader["vou_type"] = "CN";
                    dtrowHeader["vou_no"] = _CreditNote_Model.Vou_No;
                    dtrowHeader["vou_dt"] = _CreditNote_Model.Vou_Date;
                    dtrowHeader["src_doc"] = "D";
                    dtrowHeader["src_doc_no"] = _CreditNote_Model.Bill_No;
                    dtrowHeader["src_doc_dt"] = _CreditNote_Model.Bill_Date;
                    dtrowHeader["vou_amt"] = "0";
                    dtrowHeader["remarks"] = _CreditNote_Model.Remarks;
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
                    CreditNoteHeader = dtheader;

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
                    JArray jObject = JArray.Parse(_CreditNote_Model.GlAccountDetails);

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
                    CreditNoteGLDetails = dtAccount;
                    ViewData["VouDetails"] = DtVouDetails(jObject);

                    /*--------------Cost Center Section Start-----------------------*/

                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));

                    JArray JAObj = JArray.Parse(_CreditNote_Model.CC_DetailList);
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
                    /**----------------Bill Adjustment Section--------------------*/
                    DebitNoteBillAdj = dtBillAdjdetails(_CreditNote_Model.HdnBillAdjdetails);
                    /**----------------Bill Adjustment Section End--------------------*/

                    /*-----------------Attachment Section Start------------------------*/
                    DataTable CrNoAttachments = new DataTable();
                    DataTable CrNodtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as Credit_Model;
                    TempData["IMGDATA"] = null;
                    if (_CreditNote_Model.attatchmentdetail != null)
                    {
                        if (attachData != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)
                            //{
                            //    CrNodtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            //}
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                CrNodtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                CrNodtAttachment.Columns.Add("id", typeof(string));
                                CrNodtAttachment.Columns.Add("file_name", typeof(string));
                                CrNodtAttachment.Columns.Add("file_path", typeof(string));
                                CrNodtAttachment.Columns.Add("file_def", typeof(char));
                                CrNodtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_CreditNote_Model.AttachMentDetailItmStp != null)
                            {
                                CrNodtAttachment = _CreditNote_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                CrNodtAttachment.Columns.Add("id", typeof(string));
                                CrNodtAttachment.Columns.Add("file_name", typeof(string));
                                CrNodtAttachment.Columns.Add("file_path", typeof(string));
                                CrNodtAttachment.Columns.Add("file_def", typeof(char));
                                CrNodtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_CreditNote_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in CrNodtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = CrNodtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_CreditNote_Model.Vou_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = _CreditNote_Model.Vou_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                CrNodtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_CreditNote_Model.TransType == "Update")
                        {
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string CN_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_CreditNote_Model.Vou_No).ToString()))
                                {
                                    CN_CODE = (_CreditNote_Model.Vou_No).ToString();

                                }
                                else
                                {
                                    CN_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + Br_ID + CN_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in CrNodtAttachment.Rows)
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
                        CrNoAttachments = CrNodtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/

                    SaveMessage = _CreditNote_IService.InsertCreditNoteDetail(CreditNoteHeader
                        , CreditNoteGLDetails, CrNoAttachments, CRCostCenterDetails, DebitNoteBillAdj,_CreditNote_Model.conv_rate);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 25-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //_BankPayment_Model.Message = "Financial Year not Exist";
                        _CreditNote_Model.BtnName = "Refresh";
                        _CreditNote_Model.Command = "Refresh";
                        _CreditNote_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;
                        return RedirectToAction("CreditNoteDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //_BankPayment_Model.Message = "Financial Book Closing";
                        _CreditNote_Model.BtnName = "Refresh";
                        _CreditNote_Model.Command = "Refresh";
                        _CreditNote_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;

                        return RedirectToAction("CreditNoteDetail");
                    }
                    else
                    {
                        if (SaveMessage == "N")
                        {
                            _CreditNote_Model.Message = "N";
                            return RedirectToAction("CreditNoteDetail");
                        }
                        else
                        {
                            string CreditNoteNo = SaveMessage.Split(',')[1].Trim();
                            string CreditNote_Number = CreditNoteNo.Replace("/", "");
                            string Message = SaveMessage.Split(',')[0].Trim();
                            string CreditNoteDate = SaveMessage.Split(',')[2].Trim();
                            if (Message == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message.Replace("_", " ") + " " + CreditNoteNo + " in " + PageName;//ContraNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _CreditNote_Model.Message = Message.Split(',')[0].Replace("_", "");
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
                                comCont.ResetImageLocation(CompID, Br_ID, guid, PageName, CreditNote_Number, _CreditNote_Model.TransType, CrNoAttachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{
                                //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Br_ID + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in CrNoAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = CompID + Br_ID + CreditNote_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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
                                //Session["CreditNoteNo"] = CreditNoteNo;
                                //Session["CreditNoteDate"] = CreditNoteDate;
                                //Session["TransType"] = "Update";
                                //Session["AppStatus"] = 'D';
                                //Session["BtnName"] = "BtnToDetailPage";
                                _CreditNote_Model.Message = "Save";
                            //_CreditNote_Model.Command = "Update";
                            _CreditNote_Model.CreditNoteNo = CreditNoteNo;
                            _CreditNote_Model.CreditNoteDate = CreditNoteDate;
                            _CreditNote_Model.TransType = "Update";
                            _CreditNote_Model.BtnName = "BtnToDetailPage";
                            return RedirectToAction("CreditNoteDetail");
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
                    string FinalStatus = CheckPaymentAgainstCreditNote(_CreditNote_Model, _CreditNote_Model.Vou_No, _CreditNote_Model.Vou_Date.ToString());
                    if (FinalStatus == "Used" || FinalStatus == "Used1")
                    {
                        _CreditNote_Model.Message = FinalStatus;
                        _CreditNote_Model.Message1 = FinalStatus;
                        _CreditNote_Model.Command = "Refresh";

                        TempData["ModelData"] = _CreditNote_Model;
                    }
                    else
                    {
                        _CreditNote_Model.Create_by = UserID;
                        string br_id = Session["BranchId"].ToString();
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        DataSet SaveMessage1 = _CreditNote_IService.CreditNoteCancel(_CreditNote_Model, CompID, br_id, mac_id);
                        try
                        {
                            string fileName = "CreditNote_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_CreditNote_Model.Vou_No, _CreditNote_Model.Vou_Date, fileName, DocumentMenuId, "C");
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _CreditNote_Model.Vou_No, "C", UserID, "");
                        }
                        catch (Exception exMail)
                        {
                            _CreditNote_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _CreditNote_Model.Message = _CreditNote_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                       
                        //_CreditNote_Model.Message = "Cancelled";
                        _CreditNote_Model.CreditNoteNo = _CreditNote_Model.Vou_No;
                        _CreditNote_Model.CreditNoteDate = _CreditNote_Model.Vou_Date;
                        _CreditNote_Model.TransType = "Update";
                        _CreditNote_Model.BtnName = "Refresh";
                    }
                    
                    return RedirectToAction("CreditNoteDetail");
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
                    if (_CreditNote_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_CreditNote_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _CreditNote_Model.Guid.ToString();
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
        public DataTable dtBillAdjdetails(string BillAdjdetails)
        {

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
            JArray BObject = JArray.Parse(BillAdjdetails);
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
            ViewData["BillAdj"] = dtBillAdj(BObject);
            return dtBillAdjDetail;

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
        public ActionResult GetAutoCompleteGLDetail(CreditNote_Model _CreditNote_Model, string Entitytype,string flag)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> EntityAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_CreditNote_Model.EntityName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _CreditNote_Model.EntityName;
                }
                Br_ID = Session["BranchId"].ToString();
                EntityAccList = _CreditNote_IService.AutoGetEntityList(Comp_ID, Acc_Name, Br_ID, Entitytype,flag);

                List<EntityAccName> _EntityAccNameList = new List<EntityAccName>();
                foreach (var dr in EntityAccList)
                {
                    EntityAccName _EntityAccName = new EntityAccName();
                    _EntityAccName.entity_acc_id = dr.Key;
                    _EntityAccName.entity_acc_name = dr.Value;
                    _EntityAccNameList.Add(_EntityAccName);
                }
                _CreditNote_Model.EntityAccNameList = _EntityAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(EntityAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }

        public ActionResult GetCreditNoteListDashboard(string docid, string status)
        {

            CreditNoteList_Model Dashbord = new CreditNoteList_Model();
            //Session["WF_status"] = status;
            Dashbord.WF_Status = status;
            return RedirectToAction("CreditNote", Dashbord);
        }
        public ActionResult GetEntityIDDetail(string EntityID,string VouDate)
        {
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _CreditNote_IService.GetEntityIDDetail(CompID, BrchID, EntityID, VouDate);

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
                DataTable dt = _CreditNote_IService.GetCurrList(CompID);
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
                DataSet result = _CreditNote_IService.GetAccCurrOnChange(acc_id, CompID, Br_ID, date);
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
        public ActionResult GetAutoCompleteEntityDetail(CreditNoteList_Model _CreditNotetList_Model, string Entitytype,string flag)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> EntityAccList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_CreditNotetList_Model.EntityName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _CreditNotetList_Model.EntityName;
                }
                Br_ID = Session["BranchId"].ToString();

                EntityAccList = _CreditNote_IService.AutoGetEntityList(Comp_ID, Acc_Name, Br_ID, Entitytype,flag);

                List<EntityAccList> _EntityAccNameList = new List<EntityAccList>();
                foreach (var dr in EntityAccList)
                {
                    EntityAccList _EntityAccName = new EntityAccList();
                    _EntityAccName.entity_acc_id = dr.Key;
                    _EntityAccName.entity_acc_name = dr.Value;
                    _EntityAccNameList.Add(_EntityAccName);
                }
                _CreditNotetList_Model.EntityAccNameList = _EntityAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(EntityAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        private List<VouList> GetCreditNoteListAll(CreditNoteList_Model _CreditNoteList_Model)
        {
            try
            {
                _EntityList = new List<VouList>();
                string Br_ID = string.Empty;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //Br_ID = Session["BranchId"].ToString();
                string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if(_CreditNoteList_Model.WF_Status != null)
                {
                    wfstatus = _CreditNoteList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string VouType = "CN";
                DataSet Dtdata = new DataSet();
                Dtdata = _CreditNote_IService.GetCreditNoteListAll(_CreditNoteList_Model.entity_type, _CreditNoteList_Model.entity_id, _CreditNoteList_Model.VouFromDate, _CreditNoteList_Model.VouToDate, _CreditNoteList_Model.Status, CompID, Br_ID, VouType, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    _CreditNoteList_Model.FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (Dtdata.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in Dtdata.Tables[0].Rows)
                    {
                        VouList _VouList = new VouList();
                        _VouList.VouNumber = dr["vou_no"].ToString();
                        _VouList.VouDate = dr["vou_dt"].ToString();
                        _VouList.hdVouDate = dr["vou_date"].ToString();
                        _VouList.entity_type = dr["EntityType"].ToString();
                        _VouList.entity_name = dr["entity_name"].ToString();
                        _VouList.Amountsp = dr["vou_amt_sp"].ToString();
                        _VouList.Amountbs = dr["vou_amt_bs"].ToString();
                        _VouList.curr_logo = dr["curr_logo"].ToString();
                        _VouList.VouStatus = dr["vou_status"].ToString();
                        _VouList.CreatedON = dr["created_on"].ToString();
                        _VouList.ApprovedOn = dr["app_dt"].ToString();
                        _VouList.ModifiedOn = dr["mod_on"].ToString();
                        _VouList.BillNo = dr["bill_no"].ToString();
                        _VouList.BillDt = dr["bill_dt"].ToString();
                        _VouList.create_by = dr["create_by"].ToString();
                        _VouList.app_by = dr["app_by"].ToString();
                        _VouList.mod_by = dr["mod_by"].ToString();

                        _EntityList.Add(_VouList);
                    }
                }
                return _EntityList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchCreditNoteDetail(string Entity_Type, string entity_id, string Fromdate, string Todate, string Status, string CompID, string Br_ID)
        {
            _EntityList = new List<VouList>();
            CreditNoteList_Model _CreditNoteList_Model = new CreditNoteList_Model();
            //Session["WF_status"] = "";
            _CreditNoteList_Model.WF_Status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            string VouType = "CN";
            DataSet dt = new DataSet();
            dt = _CreditNote_IService.GetCreditNoteListAll(Entity_Type, entity_id, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
            //Session["VouSearch"] = "Vou_Search";
            _CreditNoteList_Model.VouSearch = "Vou_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    VouList _VouList = new VouList();

                    _VouList.VouNumber = dr["vou_no"].ToString();
                    _VouList.VouDate = dr["vou_dt"].ToString();
                    _VouList.hdVouDate = dr["vou_date"].ToString();
                    _VouList.entity_type = dr["EntityType"].ToString();
                    _VouList.entity_name = dr["entity_name"].ToString();
                    _VouList.Amountsp = dr["vou_amt_sp"].ToString();
                    _VouList.Amountbs = dr["vou_amt_bs"].ToString();
                    _VouList.curr_logo = dr["curr_logo"].ToString();
                    _VouList.VouStatus = dr["vou_status"].ToString();
                    _VouList.CreatedON = dr["created_on"].ToString();
                    _VouList.ApprovedOn = dr["app_dt"].ToString();
                    _VouList.ModifiedOn = dr["mod_on"].ToString();
                    _VouList.BillNo = dr["bill_no"].ToString();
                    _VouList.BillDt = dr["bill_dt"].ToString();
                    _VouList.create_by = dr["create_by"].ToString();
                    _VouList.app_by = dr["app_by"].ToString();
                    _VouList.mod_by = dr["mod_by"].ToString();

                    _EntityList.Add(_VouList);
                }
            }
            _CreditNoteList_Model.VoucherList = _EntityList;
            CommonPageDetails();
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialCreditNoteList.cshtml", _CreditNoteList_Model);
        }
        public ActionResult EditVou(string VouNo, string Voudt, string ListFilterData,string WF_Status)
        {/*start Add by Hina on 23-02-2024 to chk Financial year exist or not*/
            CreditNote_Model dblclick = new CreditNote_Model();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            string DblClkMsg = string.Empty;
            DblClkMsg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
            //if (DblClkMsg == "FY Not Exist")
            //{
            //    TempData["Message"] = "Financial Year not Exist";
            //}
            //if (DblClkMsg == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //}
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
            //Session["CreditNoteNo"] = VouNo;
            //Session["CreditNoteDate"] = Voudt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            //CreditNote_Model dblclick = new CreditNote_Model();
            UrlModel _url = new UrlModel();
            dblclick.CreditNoteNo = VouNo;
            dblclick.CreditNoteDate = Voudt;
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
            _url.CNO = VouNo;
            _url.CDT = Voudt;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("CreditNoteDetail", "CreditNote", _url);
        }
        private ActionResult CreditNoteDelete(CreditNote_Model _CreditNote_Model, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string CreditNo = _CreditNote_Model.Vou_No;
                string CreditNoteNumber = CreditNo.Replace("/", "");

                string Message = _CreditNote_IService.CNDelete(_CreditNote_Model, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(CreditNoteNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, CreditNoteNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["CreditNoteNo"] = "";
                //Session["CreditNoteDate"] = "";
                //_CreditNote_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("CreditNoteDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1,string ModelData,string Mailerror)
        {
            // Session["Message"] = "";
            CreditNote_Model ToRefreshByJS = new CreditNote_Model();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.CreditNoteNo = a[0].Trim();
            ToRefreshByJS.CreditNoteDate = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnToDetailPage";
            ToRefreshByJS.Message = Mailerror;
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                urlModel.wf = a[2].Trim();
            }
            urlModel.bt = "D";
            urlModel.CNO = ToRefreshByJS.CreditNoteNo;
            urlModel.CDT = ToRefreshByJS.CreditNoteDate;
            urlModel.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("CreditNoteDetail", urlModel);
        }
        public ActionResult CreditNoteApprove(CreditNote_Model _CreditNote_Model,string VouNo, string VouDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1,string WF_Status1)
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
                string Message = _CreditNote_IService.CreditNoteApprove(VouNo, VouDate, UserID, A_Status, A_Level, A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string CreditNoteNo = Message.Split(',')[0].Trim();
                string CreditNoteDate = Message.Split(',')[1].Trim();
                try
                {
                    string fileName = "CreditNote_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(CreditNoteNo, CreditNoteDate, fileName, DocumentMenuId, "AP");
                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, CreditNoteNo, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _CreditNote_Model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                _CreditNote_Model.Message = _CreditNote_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                //Session["CreditNoteNo"] = CreditNoteNo;
                //Session["CreditNoteDate"] = CreditNoteDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel urlref = new UrlModel();
                _CreditNote_Model.TransType = "Update";
                _CreditNote_Model.CreditNoteNo = CreditNoteNo;
                _CreditNote_Model.CreditNoteDate = CreditNoteDate;
               // _CreditNote_Model.Message = "Approved";
                _CreditNote_Model.BtnName = "BtnEdit";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _CreditNote_Model.WF_Status1 = WF_Status1;
                    urlref.wf = WF_Status1;
                }
                TempData["ModelData"] = _CreditNote_Model;

                urlref.tp = "Update";
                urlref.CNO = CreditNoteNo;
                urlref.CDT = CreditNoteDate;
                urlref.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("CreditNoteDetail", "CreditNote", urlref);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public string CheckPaymentAgainstCreditNote(CreditNote_Model _CreditNote_Model, string DocNo, string DocDate)
        {
            //JsonResult DataRows = null;
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
                DataSet Deatils = _CreditNote_IService.CheckPaymentAgainstCreditNote(Comp_ID, Br_ID, DocNo, DocDate);
                //DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
                if (Deatils.Tables.Count > 1)
                {
                    if (Deatils.Tables[1].Rows.Count > 0)
                    {
                        str = "Used1";
                    }
                }
               
                if (str != "" && str != null)
                {
                    _CreditNote_Model.CreditNoteNo = DocNo;
                    _CreditNote_Model.CreditNoteDate = DocDate;
                    _CreditNote_Model.TransType = "Update";
                    _CreditNote_Model.BtnName = "BtnToDetailPage";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return null;
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        public void GetStatusList(CreditNoteList_Model _CreditNoteList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _CreditNoteList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Credit_Model _attachmentModel = new Credit_Model();
                //string TransType = "";
                //string CreditNoteCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["CreditNoteNo"] != null)
                //{
                //    CreditNoteCode = Session["CreditNoteNo"].ToString();
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
                getDocumentName(); /* To set Title*/
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

        public ActionResult GetCstCntrtype(string Flag, string Disableflag, string CC_rowdata, string TotalAmt = null, string Doc_ID = null)//add by sm 04-12-2024 (TotalAmt,Doc_ID)
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
                ViewBag.CCTotalAmt = TotalAmt;//add by sm 09-12-2024
                ViewBag.DocId = Doc_ID;//add by sm 09-12-2024
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
        public FileResult CreditNoteExporttoExcelDt(string EtityName, string EtityType, string Fromdate, string Todate, string Status)
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
                string VouType = "CN";
                DataTable dt = new DataTable();
                DataSet dt1 = new DataSet();
                dt1 = _CreditNote_IService.GetCreditNoteListAll(EtityType, EtityName, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
                dt.Columns.Add("Sr.No", typeof(string));
                dt.Columns.Add("GL Voucher Number", typeof(string));
                dt.Columns.Add("GL Voucher Date", typeof(string));
                dt.Columns.Add("Entity Type", typeof(string));
                dt.Columns.Add("Entity Name", typeof(string));
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
                        dtrowLines["Entity Type"] = dr["EntityType"].ToString();
                        dtrowLines["Entity Name"] = dr["entity_name"].ToString();
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
                return commonController.ExportDatatableToExcel("CreditNote", dt);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        /*--------Print---------*/

        public FileResult GenratePdfFile(CreditNote_Model _Model)
        {
            return File(GetPdfData(_Model.Vou_No, _Model.Vou_Date), "application/pdf", "CreditNote.pdf");
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
                DataSet Deatils = _Common_IServices.Cmn_GetGLVoucherPrintDeatils(CompID, Br_ID, vNo, vDate, "CN");
                ViewBag.PageName = "CN";
                ViewBag.Title = "Credit Note";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
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
                    if (ViewBag.DocStatus == "C")/*Add by NItesh  Tewatia on 09-09-2025*/
                    {
                         draftImage = Server.MapPath("~/Content/Images/cancelled.png");/*Add by Hina sharma on 16-10-2024*/
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
                                //draftimg.SetAbsolutePosition(0, 160);  /*Commented by nitesh 09092025*/
                                //draftimg.ScaleAbsolute(580f, 580f);
                                draftimg.SetAbsolutePosition(0, 10);
                                draftimg.ScaleAbsolute(750f, 580f);
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