using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.DebitNote;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.DebitNote;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.DebitNote
{
    public class DebitNoteController : Controller
    {
        string CompID, language, FromDate = String.Empty;
        string Comp_ID, Br_ID, Language, title, UserID = String.Empty;
        string DocumentMenuId = "105104115130";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        DebitNote_IService _DebitNote_IService;
        DataTable dt;
        string Entitytype = "1";
        string flag = "";
        List<VouList> _EntityList;
        public DebitNoteController(Common_IServices _Common_IServices, DebitNote_IService _DebitNote_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._DebitNote_IService = _DebitNote_IService;
        }
        // GET: ApplicationLayer/DebitNote
        public ActionResult DebitNote(DebitNoteList_Model _DebitNoteList_Model)
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
            //DebitNoteList_Model _DebitNoteList_Model = new DebitNoteList_Model();
            GetStatusList(_DebitNoteList_Model);
            //GetAutoCompleteEntityDetail(_DebitNoteList_Model,Entitytype," All");

            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                DebitNote_Model _DebitNote_Model = new DebitNote_Model();
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _DebitNoteList_Model.entity_type = a[0].Trim();
                _DebitNoteList_Model.entity_id = a[1].Trim();
                _DebitNoteList_Model.VouFromDate = a[2].Trim();
                _DebitNoteList_Model.VouToDate = a[3].Trim();
                _DebitNoteList_Model.Status = a[4].Trim();
                if (_DebitNoteList_Model.Status == "0")
                {
                    _DebitNoteList_Model.Status = null;
                }
                _DebitNoteList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                var Entitytype = _DebitNoteList_Model.entity_type;
                if (Entitytype != "0")
                {
                    List<EntityAccList> _EntityAccNameList = new List<EntityAccList>();
                    dt = GetEntity1(_DebitNoteList_Model, Entitytype, " All");
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityAccList EntityName = new EntityAccList();
                        EntityName.entity_acc_id = dr["entity_acc_id"].ToString();
                        EntityName.entity_acc_name = dr["entity_acc_name"].ToString();
                        _EntityAccNameList.Add(EntityName);

                    }
                    _EntityAccNameList.Insert(0, new EntityAccList() { entity_acc_id = "0", entity_acc_name = "---Select---" });
                    _DebitNoteList_Model.EntityAccNameList = _EntityAccNameList;
                }
                else
                {
                    List<EntityAccList> _EntityAccNameList = new List<EntityAccList>();
                    _EntityAccNameList.Insert(0, new EntityAccList() { entity_acc_id = "0", entity_acc_name = "---Select---" });
                    _DebitNoteList_Model.EntityAccNameList = _EntityAccNameList;
                }
                _DebitNoteList_Model.VoucherList = GetDebitNoteListAll(_DebitNoteList_Model);
            }

            if (_DebitNoteList_Model.VouFromDate != null)
            {
                _DebitNoteList_Model.FromDate = _DebitNoteList_Model.VouFromDate;
            }
            else
            {
                List<EntityAccList> _EntityAccNameList = new List<EntityAccList>();
                _EntityAccNameList.Insert(0, new EntityAccList() { entity_acc_id = "0", entity_acc_name = "---Select---" });

                _DebitNoteList_Model.EntityAccNameList = _EntityAccNameList;
                //_DebitNoteList_Model.FromDate = startDate;
                _DebitNoteList_Model.VoucherList = GetDebitNoteListAll(_DebitNoteList_Model);
            }
            //_DebitNoteList_Model.FromDate = startDate;

            ViewBag.VBRoleList = GetRoleList();
            ViewBag.MenuPageName = getDocumentName();
            _DebitNoteList_Model.Title = title;
            //Session["VouSearch"] = "0";
            _DebitNoteList_Model.VouSearch = "0";         
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/DebitNote/DebitNoteList.cshtml", _DebitNoteList_Model);
        }
        public DataTable GetEntity1(DebitNoteList_Model _DebitNotetList_Model, string Entitytype, string flag)
        {
            string Acc_Name = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_DebitNotetList_Model.EntityName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _DebitNotetList_Model.EntityName;
                }
                Br_ID = Session["BranchId"].ToString();

                DataTable dt = _DebitNote_IService.AutoGetEntityList1(Comp_ID, Acc_Name, Br_ID, Entitytype, flag);
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
        public ActionResult AddDebitNoteDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            DebitNote_Model AddNewModel = new DebitNote_Model();
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
            //    return RedirectToAction("DebitNote", AddNewModel);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("DebitNote", AddNewModel);
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
            return RedirectToAction("DebitNoteDetail", "DebitNote", _urlModel);
        }
        public ActionResult DebitNoteDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/

                DebitNote_Model _DebitNote_Model = new DebitNote_Model();
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
                //Session["BtnName"] = "BtnAddNew";
                //Session["TransType"] = "Save";
                var _DebitNote_Model1 = TempData["ModelData"] as DebitNote_Model;
                if (_DebitNote_Model1 != null)
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    GetAutoCompleteGLDetail(_DebitNote_Model1, Entitytype, " ----Select----");
                    _DebitNote_Model1.entity_type = "2";
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
                    _DebitNote_Model1.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DebitNote_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    // if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_DebitNote_Model1.TransType == "Update" || _DebitNote_Model1.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["DebitNoteNo"].ToString();
                        //string VouDt = Session["DebitNoteDate"].ToString();
                        string VouType = "DN";
                        string VouNo = _DebitNote_Model1.DebitNoteNo;
                        string VouDt = _DebitNote_Model1.DebitNoteDate;
                        DataSet ds = _DebitNote_IService.GetDebitNoteDetail(VouNo, VouDt, VouType, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _DebitNote_Model1.HdnBillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        _DebitNote_Model1.entity_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _DebitNote_Model1.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _DebitNote_Model1.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _DebitNote_Model1.entity_type = ds.Tables[0].Rows[0]["acc_type"].ToString();
                        _DebitNote_Model1.entity_acc_Name = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _DebitNote_Model1.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        _DebitNote_Model1.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _DebitNote_Model1.Bill_Date = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        _DebitNote_Model1.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _DebitNote_Model1.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _DebitNote_Model1.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _DebitNote_Model1.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        _DebitNote_Model1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _DebitNote_Model1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _DebitNote_Model1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _DebitNote_Model1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _DebitNote_Model1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _DebitNote_Model1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _DebitNote_Model1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _DebitNote_Model1.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DebitNote_Model1.SrcDocNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _DebitNote_Model1.SrcDocDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _DebitNote_Model1.SalePerson = ds.Tables[0].Rows[0]["sls_per"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _DebitNote_Model1.Status = Statuscode;
                        _DebitNote_Model1.DocumentStatus = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        GetAutoCompleteGLDetail(_DebitNote_Model1, _DebitNote_Model1.entity_type, " ----Select----");
                        if (_DebitNote_Model1.Status == "C")
                        {
                            _DebitNote_Model1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _DebitNote_Model1.CancelFlag = true;
                            _DebitNote_Model1.BtnName = "Refresh";
                            //Session["BtnName"] = "Refresh";
                        }
                        else
                        {
                            _DebitNote_Model1.CancelFlag = false;
                        }

                        _DebitNote_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _DebitNote_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _DebitNote_Model1.Command != "Edit")
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
                                    _DebitNote_Model1.BtnName = "Refresh";
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
                                        _DebitNote_Model1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _DebitNote_Model1.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _DebitNote_Model1.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _DebitNote_Model1.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                       // _DebitNote_Model1.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DebitNote_Model1.BtnName = "BtnToDetailPage";
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
                                        _DebitNote_Model1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    // Session["BtnName"] = "BtnToDetailPage";
                                    _DebitNote_Model1.BtnName = "BtnToDetailPage";
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
                                    _DebitNote_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DebitNote_Model1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DebitNote_Model1.BtnName = "Refresh";
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
                        _DebitNote_Model1.Title = title;
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
                        _DebitNote_Model1.SalePersonList = _SlPrsnList;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/DebitNote/DebitNoteDetail.cshtml", _DebitNote_Model1);
                    }
                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _DebitNote_Model1.Title = title;
                        //ViewBag.MenuPageName = getDocumentName();
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
                        _DebitNote_Model1.SalePersonList = _SlPrsnList;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/DebitNote/DebitNoteDetail.cshtml", _DebitNote_Model1);
                    }
                }
                else
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _DebitNote_Model.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _DebitNote_Model.BtnName = _urlModel.bt;
                        }
                        _DebitNote_Model.DebitNoteNo = _urlModel.DNO;
                        _DebitNote_Model.DebitNoteDate = _urlModel.DDT;
                        _DebitNote_Model.Command = _urlModel.Cmd;
                        _DebitNote_Model.TransType = _urlModel.tp;
                        _DebitNote_Model.WF_Status1 = _urlModel.wf;
                        _DebitNote_Model.DocumentStatus = _urlModel.DMS;
                    }
                    /* Add by Hina on 21-02-2024 to Refresh Page*/
                    if (_DebitNote_Model.TransType == null)
                    {
                        _DebitNote_Model.BtnName = "Refresh";
                        _DebitNote_Model.Command = "Refresh";
                        _DebitNote_Model.TransType = "Refresh";

                    }
                    /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var VouDate = _DebitNote_Model.DebitNoteDate;
                    //var VouDate = "";

                    //if (_DebitNote_Model.DebitNoteDate != null)
                    //{
                    //    VouDate = _DebitNote_Model.DebitNoteDate;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    _DebitNote_Model.DebitNoteDate = CurrentDate;
                    //    _DebitNote_Model.Vou_Date = CurrentDate;
                    //    VouDate = _DebitNote_Model.DebitNoteDate;
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
                    GetAutoCompleteGLDetail(_DebitNote_Model, Entitytype, " ----Select----");
                    _DebitNote_Model.entity_type = "2";
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
                    _DebitNote_Model.currList = _currList;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DebitNote_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_DebitNote_Model.TransType == "Update" || _DebitNote_Model.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["DebitNoteNo"].ToString();
                        //string VouDt = Session["DebitNoteDate"].ToString();
                        string VouType = "DN";
                        string VouNo = _DebitNote_Model.DebitNoteNo;
                        string VouDt = _DebitNote_Model.DebitNoteDate;
                        DataSet ds = _DebitNote_IService.GetDebitNoteDetail(VouNo, VouDt, VouType, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _DebitNote_Model.HdnBillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        _DebitNote_Model.entity_acc_id = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _DebitNote_Model.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _DebitNote_Model.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _DebitNote_Model.entity_type = ds.Tables[0].Rows[0]["acc_type"].ToString();
                        _DebitNote_Model.entity_acc_Name = ds.Tables[0].Rows[0]["acc_id"].ToString();
                        _DebitNote_Model.Src_Type = ds.Tables[0].Rows[0]["src_doc"].ToString();
                        _DebitNote_Model.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _DebitNote_Model.Bill_Date = ds.Tables[0].Rows[0]["bill_dt"].ToString();
                        _DebitNote_Model.curr = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                        _DebitNote_Model.bs_curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bs_curr_id"].ToString());
                        _DebitNote_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _DebitNote_Model.Vou_amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["vou_amt"]).ToString(ValDigit);
                        _DebitNote_Model.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _DebitNote_Model.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _DebitNote_Model.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _DebitNote_Model.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _DebitNote_Model.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _DebitNote_Model.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _DebitNote_Model.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _DebitNote_Model.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        _DebitNote_Model.SrcDocNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        _DebitNote_Model.SrcDocDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        _DebitNote_Model.SalePerson = ds.Tables[0].Rows[0]["sls_per"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _DebitNote_Model.Status = Statuscode;
                        _DebitNote_Model.DocumentStatus = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        GetAutoCompleteGLDetail(_DebitNote_Model, _DebitNote_Model.entity_type, " ----Select----");
                        if (_DebitNote_Model.Status == "C")
                        {
                            _DebitNote_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _DebitNote_Model.CancelFlag = true;
                            _DebitNote_Model.BtnName = "Refresh";
                            //Session["BtnName"] = "Refresh";
                        }
                        else
                        {
                            _DebitNote_Model.CancelFlag = false;
                        }

                        _DebitNote_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        _DebitNote_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _DebitNote_Model.Command != "Edit")
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
                                    _DebitNote_Model.BtnName = "Refresh";
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
                                        _DebitNote_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _DebitNote_Model.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _DebitNote_Model.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _DebitNote_Model.BtnName = "BtnToDetailPage";
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        //_DebitNote_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DebitNote_Model.BtnName = "BtnToDetailPage";
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
                                        _DebitNote_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    // Session["BtnName"] = "BtnToDetailPage";
                                    _DebitNote_Model.BtnName = "BtnToDetailPage";
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
                                    _DebitNote_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DebitNote_Model.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DebitNote_Model.BtnName = "Refresh";
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
                        _DebitNote_Model.Title = title;
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
                        _DebitNote_Model.SalePersonList = _SlPrsnList;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/DebitNote/DebitNoteDetail.cshtml", _DebitNote_Model);
                    }
                    else
                    {
                        ViewBag.MenuPageName = getDocumentName();
                        _DebitNote_Model.Title = title;
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
                        _DebitNote_Model.SalePersonList = _SlPrsnList;
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/DebitNote/DebitNoteDetail.cshtml", _DebitNote_Model);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult GetDebitNoteList(string docid, string status)
        {
            DebitNoteList_Model Dashbord = new DebitNoteList_Model();
            //Session["WF_status"] = status;
            Dashbord.WF_Status = status;
            //Session["WF_status"] = status;
            return RedirectToAction("DebitNote", Dashbord);
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
                DataTable dt = _DebitNote_IService.GetCurrList(CompID);
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
                DataSet result = _DebitNote_IService.GetAccCurrOnChange(acc_id, CompID, Br_ID, date);
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

        public ActionResult GetAutoCompleteGLDetail(DebitNote_Model _DebitNote_Model, string Entitytype, string flag)
        {
            string Acc_Name = string.Empty;
            Dictionary<string, string> EntityAccList = new Dictionary<string, string>();
            string Br_ID = string.Empty;
            string Comp_ID = string.Empty;

            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (string.IsNullOrEmpty(_DebitNote_Model.EntityName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _DebitNote_Model.EntityName;
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                // Br_ID = Session["BranchId"].ToString();
                EntityAccList = _DebitNote_IService.AutoGetEntityList(Comp_ID, Acc_Name, Br_ID, Entitytype, flag);

                List<EntityAccName> _EntityAccNameList = new List<EntityAccName>();
                foreach (var dr in EntityAccList)
                {
                    EntityAccName _EntityAccName = new EntityAccName();
                    _EntityAccName.entity_acc_id = dr.Key;
                    _EntityAccName.entity_acc_name = dr.Value;
                    _EntityAccNameList.Add(_EntityAccName);
                }
                _DebitNote_Model.EntityAccNameList = _EntityAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(EntityAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAutoCompleteEntityDetail(DebitNoteList_Model _DebitNotetList_Model, string Entitytype, string flag)
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
                if (string.IsNullOrEmpty(_DebitNotetList_Model.EntityName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _DebitNotetList_Model.EntityName;
                }
                Br_ID = Session["BranchId"].ToString();

                EntityAccList = _DebitNote_IService.AutoGetEntityList(Comp_ID, Acc_Name, Br_ID, Entitytype, flag);

                List<EntityAccList> _EntityAccNameList = new List<EntityAccList>();
                foreach (var dr in EntityAccList)
                {
                    EntityAccList _EntityAccName = new EntityAccList();
                    _EntityAccName.entity_acc_id = dr.Key;
                    _EntityAccName.entity_acc_name = dr.Value;
                    _EntityAccNameList.Add(_EntityAccName);
                }
                _DebitNotetList_Model.EntityAccNameList = _EntityAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(EntityAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
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
                DataSet ds = _DebitNote_IService.GetEntityIDDetail(CompID, BrchID, EntityID, VouDate);

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
        public ActionResult DebitNoteCommands(DebitNote_Model _DebitNote_Model, string Vou_No, string command)
        {
            try
            {
                /*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                if (_DebitNote_Model.DeleteCommand == "Delete")
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
                        DebitNote_Model adddnew = new DebitNote_Model();
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
                        //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month,dtnow.Day).ToString("yyyy-MM-dd");
                        //string MsgNew = string.Empty;
                        //MsgNew = commCont.Fin_CheckFinancialYear(CompID, Br_ID, CurrentDate);
                        //if (MsgNew == "FY Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("DebitNoteDetail", "DebitNote", adddnew);
                        //}
                        //if (MsgNew == "FB Close")
                        //{
                        //    TempData["FBMessage"] = "Financial Book Closing";
                        //    adddnew.BtnName = "Refresh";
                        //    adddnew.Command = "Refresh";
                        //    adddnew.Vou_Date = CurrentDate;
                        //    return RedirectToAction("DebitNoteDetail", "DebitNote", adddnew);
                        //}
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("DebitNoteDetail", "DebitNote", NewModel);

                    case "Edit":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt = _DebitNote_Model.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_DebitNote_Model.Status == "A"|| _DebitNote_Model.Status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("EditVou", new { VouNo = _DebitNote_Model.Vou_No, Voudt = _DebitNote_Model.Vou_Date, ListFilterData = _DebitNote_Model.ListFilterData1, WF_Status = _DebitNote_Model.WFStatus });
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        string FinalStatus = CheckPaymentAgainstDebitNote(_DebitNote_Model, _DebitNote_Model.Vou_No, _DebitNote_Model.Vou_Date.ToString());
                        if (FinalStatus == "Used" || FinalStatus == "Used1")
                        {
                            _DebitNote_Model.Message = FinalStatus;
                            _DebitNote_Model.Message1 = FinalStatus;
                            if(FinalStatus == "Used1")
                            {
                                _DebitNote_Model.Command = "Approve";
                            }
                            else
                            {
                                _DebitNote_Model.Command = "Refresh";
                            }
                            
                            TempData["ModelData"] = _DebitNote_Model;
                        }
                        else
                        {
                            _DebitNote_Model.TransType = "Update";
                            _DebitNote_Model.Command = command;
                            _DebitNote_Model.BtnName = "BtnEdit";
                            _DebitNote_Model.DebitNoteNo = _DebitNote_Model.Vou_No;
                            _DebitNote_Model.DebitNoteDate = _DebitNote_Model.Vou_Date;
                            TempData["ModelData"] = _DebitNote_Model;
                            UrlModel EditModel = new UrlModel();
                            EditModel.tp = "Update";
                            EditModel.Cmd = command;
                            EditModel.bt = "BtnEdit";
                            EditModel.DNO = _DebitNote_Model.Vou_No;
                            EditModel.DDT = _DebitNote_Model.Vou_Date;
                            TempData["ListFilterData"] = _DebitNote_Model.ListFilterData1;
                            return RedirectToAction("DebitNoteDetail", EditModel);
                        }
                        UrlModel Edit_Model = new UrlModel();
                        Edit_Model.tp = "Update";
                        Edit_Model.bt = "D";
                        Edit_Model.DNO = _DebitNote_Model.DebitNoteNo;
                        Edit_Model.DDT = _DebitNote_Model.DebitNoteDate;
                        TempData["ListFilterData"] = _DebitNote_Model.ListFilterData1;
                        return RedirectToAction("DebitNoteDetail", Edit_Model);

                    case "Delete":
                        _DebitNote_Model.Command = command;
                        Vou_No = _DebitNote_Model.Vou_No;
                        DebitNoteDelete(_DebitNote_Model, command);
                        DebitNote_Model DeleteModel = new DebitNote_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete = new UrlModel();
                        Delete.Cmd = DeleteModel.Command;
                        Delete.tp = "Refresh";
                        Delete.bt = "BtnDelete";
                        TempData["ListFilterData"] = _DebitNote_Model.ListFilterData1;
                        return RedirectToAction("DebitNoteDetail", Delete);


                    case "Save":
                        /*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _DebitNote_Model.Vou_Date;
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
                            if (_DebitNote_Model.Vou_No == null)
                            {
                                _DebitNote_Model.Command = "Refresh";
                                _DebitNote_Model.TransType = "Refresh";
                                _DebitNote_Model.BtnName = "Refresh";
                                _DebitNote_Model.DocumentStatus = null;
                                TempData["ModelData"] = _DebitNote_Model;
                                return RedirectToAction("DebitNoteDetail", "DebitNote", _DebitNote_Model);
                            }
                            else
                            {
                                return RedirectToAction("EditVou", new { VouNo = _DebitNote_Model.Vou_No, Voudt = _DebitNote_Model.Vou_Date, ListFilterData = _DebitNote_Model.ListFilterData1, WF_Status = _DebitNote_Model.WFStatus });

                            }

                        }
                        /*End to chk Financial year exist or not*/
                        _DebitNote_Model.Command = command;
                        SaveDebitNote(_DebitNote_Model);
                        if (_DebitNote_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_DebitNote_Model.Message == "N")
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
                            _DebitNote_Model.currList = _currList;
                            ViewBag.MenuPageName = getDocumentName();
                            ViewBag.VouDetails = ViewData["VouDetails"];
                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                            DataTable dtVouDetails = ViewData["VouDetails"] as DataTable;

                            decimal TotalDr = 0;
                            decimal TotalCr = 0;
                            for (int i = 0; i < dtVouDetails.Rows.Count; i++)
                            {  /*Commented and modify by HIna on 30-10-2023 to changes for Base Amount instead of specific amount*/
                                //TotalDr = TotalDr + Convert.ToDecimal(dtVouDetails.Rows[i]["dr_amt_sp"]);
                                TotalDr = TotalDr + Convert.ToDecimal(dtVouDetails.Rows[i]["dr_amt_bs"]);
                            }
                            for (int i = 0; i < dtVouDetails.Rows.Count; i++)
                            {  /*Commented and modify by HIna on 30-10-2023 to changes for Base Amount instead of specific amount*/
                                //TotalCr = TotalCr + Convert.ToDecimal(dtVouDetails.Rows[i]["cr_amt_sp"]);
                                TotalCr = TotalCr + Convert.ToDecimal(dtVouDetails.Rows[i]["cr_amt_bs"]);
                            }
                            ViewBag.TotalVouAmt = TotalDr.ToString(ValDigit);
                            ViewBag.DiffAmt = TotalDr - TotalCr;

                            GetAutoCompleteGLDetail(_DebitNote_Model, _DebitNote_Model.entity_type, " ----Select----");


                            ViewBag.CostCenterData = ViewData["CostCenter"];
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                            _DebitNote_Model.BtnName = "BtnAddNew";
                            _DebitNote_Model.Command = "Add";
                            _DebitNote_Model.Message = "N";
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
                            _DebitNote_Model.SalePersonList = _SlPrsnList;
                            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/DebitNote/DebitNoteDetail.cshtml", _DebitNote_Model);
                        }
                        else
                        {
                            //Session["DebitNoteNo"] = Session["DebitNoteNo"].ToString();
                            //Session["DebitNoteDate"] = Session["DebitNoteDate"].ToString();
                            TempData["ModelData"] = _DebitNote_Model;
                            UrlModel _urlModel = new UrlModel();
                            _urlModel.bt = _DebitNote_Model.BtnName;
                            //_urlModel.Cmd = _DebitNote_Model.Command;
                            _urlModel.DNO = _DebitNote_Model.DebitNoteNo;
                            _urlModel.DDT = _DebitNote_Model.DebitNoteDate;
                            _urlModel.tp = _DebitNote_Model.TransType;
                            TempData["ListFilterData"] = _DebitNote_Model.ListFilterData1;
                            return RedirectToAction("DebitNoteDetail", _urlModel);
                        }

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _DebitNote_Model.Vou_Date;

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
                            return RedirectToAction("EditVou", new { VouNo = _DebitNote_Model.Vou_No, Voudt = _DebitNote_Model.Vou_Date, ListFilterData = _DebitNote_Model.ListFilterData1, WF_Status = _DebitNote_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        _DebitNote_Model.Command = command;
                        Vou_No = _DebitNote_Model.Vou_No;
                        //Session["DebitNoteNo"] = Vou_No;
                        //Session["DebitNoteDate"] = _DebitNote_Model.Vou_Date;
                        DebitNoteApprove(_DebitNote_Model, _DebitNote_Model.Vou_No, _DebitNote_Model.Vou_Date, "", "", "", "", "");
                        TempData["ModelData"] = _DebitNote_Model;
                        UrlModel urlref = new UrlModel();
                        urlref.tp = "Update";
                        urlref.DNO = _DebitNote_Model.DebitNoteNo;
                        urlref.DDT = _DebitNote_Model.DebitNoteDate;
                        urlref.bt = "BtnEdit";
                        TempData["ListFilterData"] = _DebitNote_Model.ListFilterData1;
                        return RedirectToAction("DebitNoteDetail", urlref);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        //Session["DocumentStatus"] = 'D';
                        DebitNote_Model RefreshModel = new DebitNote_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _DebitNote_Model.ListFilterData1;
                        return RedirectToAction("DebitNoteDetail", refesh);

                    case "Print":
                        return GenratePdfFile(_DebitNote_Model);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        DebitNoteList_Model _Backtolist = new DebitNoteList_Model();
                        _Backtolist.WF_Status = _DebitNote_Model.WF_Status1;
                        TempData["ListFilterData"] = _DebitNote_Model.ListFilterData1;
                        return RedirectToAction("DebitNote", "DebitNote", _Backtolist);

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

        public ActionResult SaveDebitNote(DebitNote_Model _DebitNote_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            try
            {
                if (_DebitNote_Model.CancelFlag == false)
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
                    DataTable DebitNoteHeader = new DataTable();
                    DataTable DebitNoteGLDetails = new DataTable();
                    DataTable DebitNoteBillAdj = new DataTable();//Added by Suraj on 07-05-2024
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
                    if (_DebitNote_Model.Vou_No != null)
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
                    dtrowHeader["vou_type"] = "DN";
                    dtrowHeader["vou_no"] = _DebitNote_Model.Vou_No;
                    dtrowHeader["vou_dt"] = _DebitNote_Model.Vou_Date;
                    dtrowHeader["src_doc"] = "D";
                    dtrowHeader["src_doc_no"] = _DebitNote_Model.Bill_No;
                    dtrowHeader["src_doc_dt"] = _DebitNote_Model.Bill_Date;
                    dtrowHeader["vou_amt"] = "0";
                    dtrowHeader["remarks"] = _DebitNote_Model.Remarks;
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
                    DebitNoteHeader = dtheader;

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
                    JArray jObject = JArray.Parse(_DebitNote_Model.GlAccountDetails);

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
                    DebitNoteGLDetails = dtAccount;
                    ViewData["VouDetails"] = DtVouDetails(jObject);

                    /**----------------Bill Adjustment Section--------------------*/
                    DebitNoteBillAdj = dtBillAdjdetails(_DebitNote_Model.HdnBillAdjdetails);
                    /**----------------Bill Adjustment Section End--------------------*/

                    /**----------------Cost Center Section--------------------*/

                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));

                    JArray JAObj = JArray.Parse(_DebitNote_Model.CC_DetailList);
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
                    DataTable DbNoAttachments = new DataTable();
                    DataTable DbNodtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as DebitModel;
                    TempData["IMGDATA"] = null;
                    if (_DebitNote_Model.attatchmentdetail != null)
                    {
                        if (attachData != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)
                            //{
                            //    DbNodtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            //}
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                DbNodtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                DbNodtAttachment.Columns.Add("id", typeof(string));
                                DbNodtAttachment.Columns.Add("file_name", typeof(string));
                                DbNodtAttachment.Columns.Add("file_path", typeof(string));
                                DbNodtAttachment.Columns.Add("file_def", typeof(char));
                                DbNodtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_DebitNote_Model.AttachMentDetailItmStp != null)
                            {
                                //CRdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                DbNodtAttachment = _DebitNote_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                DbNodtAttachment.Columns.Add("id", typeof(string));
                                DbNodtAttachment.Columns.Add("file_name", typeof(string));
                                DbNodtAttachment.Columns.Add("file_path", typeof(string));
                                DbNodtAttachment.Columns.Add("file_def", typeof(char));
                                DbNodtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_DebitNote_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in DbNodtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = DbNodtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_DebitNote_Model.Vou_No).ToString()))
                                {
                                    dtrowAttachment1["id"] = _DebitNote_Model.Vou_No;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                DbNodtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_DebitNote_Model.TransType == "Update")
                        {
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string DBNo_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_DebitNote_Model.Vou_No).ToString()))
                                {
                                    DBNo_CODE = (_DebitNote_Model.Vou_No).ToString();

                                }
                                else
                                {
                                    DBNo_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + Br_ID + DBNo_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in DbNodtAttachment.Rows)
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
                        DbNoAttachments = DbNodtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/

                    SaveMessage = _DebitNote_IService.InsertDebitNoteDetail(DebitNoteHeader, DebitNoteGLDetails, DbNoAttachments, CRCostCenterDetails
                        , DebitNoteBillAdj, _DebitNote_Model.conv_rate, _DebitNote_Model.SalePerson);
                    if (SaveMessage == "FY Not Exist")/*Add by Hina on on 25-03-2025 for check financial year exits or not along with book opening*/
                    {
                        TempData["Message"] = "Financial Year not Exist";
                        //_BankPayment_Model.Message = "Financial Year not Exist";
                        _DebitNote_Model.BtnName = "Refresh";
                        _DebitNote_Model.Command = "Refresh";
                        _DebitNote_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;
                        return RedirectToAction("DebitNoteDetail");

                    }
                    else if (SaveMessage == "FB Close")
                    {
                        TempData["FBMessage"] = "Financial Book Closing";
                        //_BankPayment_Model.Message = "Financial Book Closing";
                        _DebitNote_Model.BtnName = "Refresh";
                        _DebitNote_Model.Command = "Refresh";
                        _DebitNote_Model.TransType = "Refresh";
                        //TempData["ModelData"] = _BankPayment_Model;

                        return RedirectToAction("DebitNoteDetail");
                    }
                    else
                    {
                        if (SaveMessage == "N")
                        {
                            _DebitNote_Model.Message = "N";
                            return RedirectToAction("DebitNoteDetail");
                        }
                        else
                        {
                            string DebitNoteNo = SaveMessage.Split(',')[1].Trim();
                            string DebitNote_Number = DebitNoteNo.Replace("/", "");
                            string Message = SaveMessage.Split(',')[0].Trim();
                            string DebitNoteDate = SaveMessage.Split(',')[2].Trim();
                            if (Message == "Data_Not_Found")
                            {
                                //var a = SaveMessage.Split(',');
                                var msg = Message.Replace("_", " ") + " " + DebitNoteNo + " in " + PageName;//DebitNoteNo is use for table type
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _DebitNote_Model.Message = Message.Split(',')[0].Replace("_", "");
                                return RedirectToAction("DebitNoteDetail");
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
                                comCont.ResetImageLocation(CompID, Br_ID, guid, PageName, DebitNote_Number, _DebitNote_Model.TransType, DbNoAttachments);

                                //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                //if (Directory.Exists(sourcePath))
                                //{

                                //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + Br_ID + Guid + "_" + "*");
                                //    foreach (string file in filePaths)
                                //    {
                                //        string[] items = file.Split('\\');
                                //        string ItemName = items[items.Length - 1];
                                //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                //        foreach (DataRow dr in DbNoAttachments.Rows)
                                //        {
                                //            string DrItmNm = dr["file_name"].ToString();
                                //            if (ItemName == DrItmNm)
                                //            {
                                //                string img_nm = CompID + Br_ID + DebitNote_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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
                                //Session["DebitNoteNo"] = DebitNoteNo;
                                //Session["DebitNoteDate"] = DebitNoteDate;
                                //Session["TransType"] = "Update";
                                //Session["AppStatus"] = 'D';
                                //Session["BtnName"] = "BtnToDetailPage";
                                _DebitNote_Model.Message = "Save";
                            //_DebitNote_Model.Command = "Update";
                            _DebitNote_Model.DebitNoteNo = DebitNoteNo;
                            _DebitNote_Model.DebitNoteDate = DebitNoteDate;
                            _DebitNote_Model.TransType = "Update";
                            _DebitNote_Model.BtnName = "BtnToDetailPage";
                            return RedirectToAction("DebitNoteDetail");
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
                    string FinalStatus = CheckPaymentAgainstDebitNote(_DebitNote_Model, _DebitNote_Model.Vou_No, _DebitNote_Model.Vou_Date.ToString());
                    if (FinalStatus == "Used" || FinalStatus == "Used1")
                    {
                        _DebitNote_Model.Message = FinalStatus;
                        _DebitNote_Model.Message1 = FinalStatus;
                        _DebitNote_Model.Command = "Refresh";
                        TempData["ModelData"] = _DebitNote_Model;
                    }
                    else
                    {
                        _DebitNote_Model.Create_by = UserID;
                        string br_id = Session["BranchId"].ToString();
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        DataSet SaveMessage1 = _DebitNote_IService.DebitNoteCancel(_DebitNote_Model, CompID, br_id, mac_id);
                        try
                        {
                            // string fileName = "DN_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "DebitNote_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_DebitNote_Model.Vou_No, _DebitNote_Model.Vou_Date, fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _DebitNote_Model.Vou_No, "C", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _DebitNote_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _DebitNote_Model.Message = _DebitNote_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                       // _DebitNote_Model.Message = "Cancelled";
                        _DebitNote_Model.DebitNoteNo = _DebitNote_Model.Vou_No;
                        _DebitNote_Model.DebitNoteDate = _DebitNote_Model.Vou_Date;
                        _DebitNote_Model.TransType = "Update";
                        _DebitNote_Model.BtnName = "Refresh";
                    }
                    return RedirectToAction("DebitNoteDetail");
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
                    if (_DebitNote_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_DebitNote_Model.Guid != null)
                        {
                            Guid = _DebitNote_Model.Guid;
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

        public void GetStatusList(DebitNoteList_Model _DebitNoteList_Model)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _DebitNoteList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }

        private ActionResult DebitNoteDelete(DebitNote_Model _DebitNote_Model, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string DebitNo = _DebitNote_Model.Vou_No;
                string DebitNoteNumber = DebitNo.Replace("/", "");

                string Message = _DebitNote_IService.DNDelete(_DebitNote_Model, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(DebitNoteNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, DebitNoteNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["DebitNoteNo"] = "";
                //Session["DebitNoteDate"] = "";
                //_DebitNote_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("DebitNoteDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
               // return View("~/Views/Shared/Error.cshtml");
            }

        }
        private List<VouList> GetDebitNoteListAll(DebitNoteList_Model _DebitNoteList_Model)
        {
            try
            {
                _EntityList = new List<VouList>();
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
                if (_DebitNoteList_Model.WF_Status != null)
                {
                    wfstatus = _DebitNoteList_Model.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string VouType = "DN";
                DataSet Dtdata = new DataSet();
                Dtdata = _DebitNote_IService.GetDebitNoteListAll(_DebitNoteList_Model.entity_type, _DebitNoteList_Model.entity_id, _DebitNoteList_Model.VouFromDate, _DebitNoteList_Model.VouToDate, _DebitNoteList_Model.Status, CompID, Br_ID, VouType, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    _DebitNoteList_Model.FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
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
                        _VouList.Amountsp = dr["vou_dr_amt_sp"].ToString();
                        _VouList.Amountbs = dr["vou_dr_amt_bs"].ToString();
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
        public ActionResult SearchDebitNoteDetail(string Entity_Type, string entity_id, string Fromdate, string Todate, string Status, string CompID, string Br_ID)
        {
            _EntityList = new List<VouList>();
            DebitNoteList_Model _DebitNoteList_Model = new DebitNoteList_Model();
            //Session["WF_status"] = "";
            _DebitNoteList_Model.WF_Status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            string VouType = "DN";
            DataSet dt = new DataSet();
            dt = _DebitNote_IService.GetDebitNoteListAll(Entity_Type, entity_id, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
            // Session["VouSearch"] = "Vou_Search";
            _DebitNoteList_Model.VouSearch = "Vou_Search";
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
                    _VouList.Amountsp = dr["vou_dr_amt_sp"].ToString();
                    _VouList.Amountbs = dr["vou_dr_amt_bs"].ToString();
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
            _DebitNoteList_Model.VoucherList = _EntityList;
            ViewBag.VBRoleList = GetRoleList();
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDebitNoteList.cshtml", _DebitNoteList_Model);
        }

        public ActionResult EditVou(string VouNo, string Voudt, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 22-02-2024 to chk Financial year exist or not*/
            DebitNote_Model dblclick = new DebitNote_Model();
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
            //Session["DebitNoteNo"] = VouNo;
            //Session["DebitNoteDate"] = Voudt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            //DebitNote_Model dblclick = new DebitNote_Model();
            UrlModel _url = new UrlModel();
            dblclick.DebitNoteNo = VouNo;
            dblclick.DebitNoteDate = Voudt;
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
            _url.DNO = VouNo;
            _url.DDT = Voudt;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("DebitNoteDetail", "DebitNote", _url);
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
        public ActionResult DebitNoteApprove(DebitNote_Model _DebitNote_Model, string VouNo, string VouDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1)
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
                // }
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
                string Message = _DebitNote_IService.DebitNoteApprove(VouNo, VouDate, UserID, A_Status, A_Level, A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string DebitNoteNo = Message.Split(',')[0].Trim();
                string DebitNoteDate = Message.Split(',')[1].Trim();
                //Session["DebitNoteNo"] = DebitNoteNo;
                //Session["DebitNoteDate"] = DebitNoteDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel urlref = new UrlModel();
                _DebitNote_Model.TransType = "Update";
                _DebitNote_Model.DebitNoteNo = DebitNoteNo;
                _DebitNote_Model.DebitNoteDate = DebitNoteDate;
                //_DebitNote_Model.Message = "Approved";
                _DebitNote_Model.BtnName = "BtnEdit";
                try
                {
                    //string fileName = "DN_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "DebitNote_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(VouNo, VouDate, fileName, DocumentMenuId,"AP");
                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, VouNo, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _DebitNote_Model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                _DebitNote_Model.Message = _DebitNote_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";

                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _DebitNote_Model.WF_Status1 = WF_Status1;
                    urlref.wf = WF_Status1;
                }
                TempData["ModelData"] = _DebitNote_Model;

                urlref.tp = "Update";
                urlref.DNO = DebitNoteNo;
                urlref.DDT = DebitNoteDate;
                urlref.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("DebitNoteDetail", "DebitNote", urlref);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public string CheckPaymentAgainstDebitNote(DebitNote_Model _DebitNote_Model, string DocNo, string DocDate)
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
                DataSet Deatils = _DebitNote_IService.CheckPaymentAgainstDebitNote(Comp_ID, Br_ID, DocNo, DocDate);
                //DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
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
                    _DebitNote_Model.DebitNoteNo = DocNo;
                    _DebitNote_Model.DebitNoteDate = DocDate;
                    _DebitNote_Model.TransType = "Update";
                    _DebitNote_Model.BtnName = "BtnToDetailPage";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
               // return null;
            }
            return str;

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData,string Mailerror)
        {
            //Session["Message"] = "";
            DebitNote_Model _DebitNote_Model = new DebitNote_Model();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split(',');
            _DebitNote_Model.DebitNoteNo = a[0].Trim();
            _DebitNote_Model.DebitNoteDate = a[1].Trim();
            _DebitNote_Model.TransType = "Update";
            _DebitNote_Model.BtnName = "BtnToDetailPage";
            _DebitNote_Model.Message =  Mailerror;
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                _DebitNote_Model.WF_Status1 = a[2].Trim();
                urlModel.wf = a[2].Trim();
            }
            urlModel.bt = "D";
            urlModel.DNO = _DebitNote_Model.DebitNoteNo;
            urlModel.DDT = _DebitNote_Model.DebitNoteDate;
            urlModel.tp = "Update";
            TempData["ModelData"] = _DebitNote_Model;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("DebitNoteDetail", urlModel);
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
                DebitModel _attachmentModel = new DebitModel();
                //string TransType = "";
                //string DebitNoteCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["DebitNoteNo"] != null)
                //{
                //    DebitNoteCode = Session["DebitNoteNo"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _attachmentModel.Guid = DocNo;
                // Session["Guid"] = DoDNO;
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

        /*--------Print---------*/

        public FileResult GenratePdfFile(DebitNote_Model _Model)
        {
            return File(GetPdfData(_Model.Vou_No, _Model.Vou_Date), "application/pdf", "DebitNote.pdf");
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
                DataSet Deatils = _Common_IServices.Cmn_GetGLVoucherPrintDeatils(CompID, Br_ID, vNo, vDate, "DN");
                ViewBag.PageName = "DN";
                ViewBag.Title = "Debit Note";
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
                    if (ViewBag.DocStatus == "C" )
                    {
                         draftImage = Server.MapPath("~/Content/Images/cancelled.png");/*Add by NItesh Tewatia on 09-09-2025*/
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
        public FileResult DebitNoteExporttoExcelDt(string EtityName, string EtityType, string Fromdate, string Todate, string Status)
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
                string VouType = "DN";
                DataTable dt = new DataTable();
                DataSet dt1 = _DebitNote_IService.GetDebitNoteListAll(EtityType, EtityName, Fromdate, Todate, Status, CompID, Br_ID, VouType, "", "", "");
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
                        dtrowLines["Amount (In Specific)"] = dr["vou_dr_amt_sp"].ToString();
                        dtrowLines["Amount (In Base)"] = dr["vou_dr_amt_bs"].ToString();
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
                return commonController.ExportDatatableToExcel("DebitNote", dt);
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
            DataTable dt = _DebitNote_IService.getSalesPersonList(CompID, Br_ID, UserID);
            return dt;
        }
    }
}