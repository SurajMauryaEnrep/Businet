//using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.MIS.CashBook;
//using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.MIS.CashBook;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.GeneralLedgerVoucher.InvoiceAdjustment
{
    public class InvoiceAdjustmentController : Controller
    {

        string Comp_ID, Br_ID, Language, title, UserID, FromDate = String.Empty;
        string CompID, language = String.Empty;
        string DocumentMenuId = "105104115150";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DataTable dt;
        Common_IServices _Common_IServices;
        InvoiceAdjustment_ISERVICE InvoiceAdjustment_ISERVICE;
        InvoiceAdjustmentModel _InvoiceAdjustmentModel;
        List<InvoiceAdjustmentList> _InvoiceAdjtList;
        public InvoiceAdjustmentController(Common_IServices _Common_IServices, InvoiceAdjustment_ISERVICE InvoiceAdjustment_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this.InvoiceAdjustment_ISERVICE = InvoiceAdjustment_ISERVICE;
        }
        // GET: ApplicationLayer/InvoiceAdjustment
        public ActionResult InvoiceAdjustmentList(InvoiceAdjustmentListModel _InvAdjListModel)
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
            //InvoiceAdjustmentListModel _InvAdjListModel = new InvoiceAdjustmentListModel();
            //List<EntityName> _ListEntity = new List<EntityName>();
            //_ListEntity.Insert(0, new EntityName() { id = "0", name = "---Select---" });
            //_InvAdjListModel.EntityNameList = _ListEntity;
            GetStatusList(_InvAdjListModel);
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _InvAdjListModel.EntyType = a[0].Trim();
                _InvAdjListModel.EntyId = a[1].Trim();
                _InvAdjListModel.VouFromDate = a[2].Trim();
                _InvAdjListModel.VouToDate = a[3].Trim();
                _InvAdjListModel.Status = a[4].Trim();
                if (_InvAdjListModel.Status == "0")
                {
                    _InvAdjListModel.Status = null;
                }
                _InvAdjListModel.entity_type = _InvAdjListModel.EntyType;
                _InvAdjListModel.ListFilterData = TempData["ListFilterData"].ToString();
                if (_InvAdjListModel.EntyType == "0")
                {
                    List<EntityName> _ListEntity = new List<EntityName>();
                    _ListEntity.Insert(0, new EntityName() { id = "0", name = "---Select---" });
                    _InvAdjListModel.EntityNameList = _ListEntity;
                }
                else
                {
                    List<EntityName> _EntityList = new List<EntityName>();
                    dt = GetEntity1(_InvAdjListModel.EntyType);
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityName EntityName = new EntityName();
                        EntityName.id = dr["id"].ToString();
                        EntityName.name = dr["name"].ToString();
                        _EntityList.Add(EntityName);

                    }
                    _EntityList.Insert(0, new EntityName() { id = "0", name = "---Select---" });
                    _InvAdjListModel.EntityNameList = _EntityList;
                }
            }
            _InvAdjListModel.InvAdjList = GetInvoiceAdjListAll(_InvAdjListModel);
            if (_InvAdjListModel.VouFromDate != null)
            {
                _InvAdjListModel.FromDate = _InvAdjListModel.VouFromDate;
            }
            else
            {
                //_InvAdjListModel.FromDate = FromDate;
                List<EntityName> _ListEntity = new List<EntityName>();
                _ListEntity.Insert(0, new EntityName() { id = "0", name = "---Select---" });
                _InvAdjListModel.EntityNameList = _ListEntity;
            }
            //_InvAdjListModel.FromDate = FromDate;

            ViewBag.VBRoleList = GetRoleList();
            ViewBag.MenuPageName = getDocumentName();
            _InvAdjListModel.Title = title;
            //Session["VouSearch"] = "";
            _InvAdjListModel.VouSearch = "";
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/InvoiceAdjustment/InvoiceAdjustmentList.cshtml", _InvAdjListModel);
        }
        public ActionResult InvoiceAdjustmentDetail(UrlModel _urlModel)
        {
            try
            {
                InvoiceAdjustmentModel _InvoiceAdjustmentModel = new InvoiceAdjustmentModel();
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
                var _InvoiceAdjustmentModel1 = TempData["ModelData"] as InvoiceAdjustmentModel;
                if (_InvoiceAdjustmentModel1 != null)
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];

                    List<EntityName> _ListEntity = new List<EntityName>();
                    _ListEntity.Insert(0, new EntityName() { id = "0", name = "---Select---" });
                    _InvoiceAdjustmentModel1.EntityNameList = _ListEntity;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _InvoiceAdjustmentModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_InvoiceAdjustmentModel1.TransType == "Update" || _InvoiceAdjustmentModel1.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["InvoiceAdjustmentNo"].ToString();
                        //string VouDt = Session["InvoiceAdjustmentDate"].ToString();
                        string VouNo = _InvoiceAdjustmentModel1.InvoiceAdjustmentNo;
                        string VouDt = _InvoiceAdjustmentModel1.InvoiceAdjustmentDate;
                        DataSet ds = InvoiceAdjustment_ISERVICE.GetInvoiceAdjustmentDetail(VouNo, VouDt, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _InvoiceAdjustmentModel1.Entity_id = ds.Tables[0].Rows[0]["entity_name"].ToString();
                        _InvoiceAdjustmentModel1.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _InvoiceAdjustmentModel1.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _InvoiceAdjustmentModel1.EntityType = ds.Tables[0].Rows[0]["entity_type"].ToString();
                        _InvoiceAdjustmentModel1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _InvoiceAdjustmentModel1.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _InvoiceAdjustmentModel1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _InvoiceAdjustmentModel1.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _InvoiceAdjustmentModel1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _InvoiceAdjustmentModel1.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _InvoiceAdjustmentModel1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _InvoiceAdjustmentModel1.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _InvoiceAdjustmentModel1.AdvanceAdjDetails = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                        _InvoiceAdjustmentModel1.BillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        _InvoiceAdjustmentModel1.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _InvoiceAdjustmentModel1.DocumentStatus = Statuscode;

                        List<EntityName> _EntityList = new List<EntityName>();
                        dt = GetEntity1(_InvoiceAdjustmentModel1.EntityType);
                        foreach (DataRow dr in dt.Rows)
                        {
                            EntityName EntityName = new EntityName();
                            EntityName.id = dr["id"].ToString();
                            EntityName.name = dr["name"].ToString();
                            _EntityList.Add(EntityName);

                        }
                        _EntityList.Insert(0, new EntityName() { id = "0", name = "---Select---" });
                        _InvoiceAdjustmentModel1.EntityNameList = _EntityList;

                        List<AdvanceList> _AdvanceDetail_List = new List<AdvanceList>();
                        List<BillsList> _BillsDetail_List = new List<BillsList>();

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[1].Rows)
                            {
                                AdvanceList adv_list = new AdvanceList();
                                adv_list.Vou_No = row["adv_vou_no"].ToString();
                                adv_list.Vou_Dt = row["vou_date"].ToString();
                                adv_list.Adv_Curr = row["curr"].ToString();
                                adv_list.AdvCurrId = row["currid"].ToString();
                                adv_list.Adv_Amt_Bs = row["amt_bs"].ToString();
                                adv_list.Adv_Amt_Sp = row["amt_sp"].ToString();
                                adv_list.Adv_Un_Adj_Amt = row["un_adj_amt"].ToString();
                                if (row["adj_amt"].ToString() == "0")
                                {
                                    adv_list.Adv_Adj_Amt = "";
                                }
                                else
                                {
                                    adv_list.Adv_Adj_Amt = row["adj_amt"].ToString();
                                }                                    
                                adv_list.Adv_Rem_Bal = row["pend_amt"].ToString();
                                _AdvanceDetail_List.Add(adv_list);
                            }
                            _InvoiceAdjustmentModel1.Advance_List = _AdvanceDetail_List;
                        }

                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[2].Rows)
                            {
                                BillsList Bills_list = new BillsList();
                                Bills_list.Inv_No = row["inv_no"].ToString();
                                Bills_list.Inv_Dt = row["inv_date"].ToString();
                                Bills_list.Bill_No = row["bill_no"].ToString();
                                Bills_list.Bill_Dt = row["bill_dt"].ToString();
                                Bills_list.Inv_Curr = row["curr"].ToString();
                                Bills_list.InvCurrId = row["currid"].ToString();
                                Bills_list.Inv_Amt_Bs = row["inv_amt_bs"].ToString();
                                Bills_list.Inv_Amt_Sp = row["inv_amt_sp"].ToString();
                                Bills_list.Inv_Un_Adj_Amt = row["un_adj_amt"].ToString();
                                if (row["adj_amt"].ToString() == "0")
                                {
                                    Bills_list.Inv_Adj_Amt = "";
                                }
                                else
                                {
                                    Bills_list.Inv_Adj_Amt = row["adj_amt"].ToString();
                                }                               
                                Bills_list.Inv_Rem_Bal = row["pend_amt"].ToString();
                                _BillsDetail_List.Add(Bills_list);
                            }
                            _InvoiceAdjustmentModel1.Bills_List = _BillsDetail_List;
                        }


                        if (_InvoiceAdjustmentModel1.Status == "C")
                        {
                            _InvoiceAdjustmentModel1.CancelFlag = true;
                            _InvoiceAdjustmentModel1.BtnName = "Refresh";
                        }
                        else
                        {
                            _InvoiceAdjustmentModel1.CancelFlag = false;
                        }
                        _InvoiceAdjustmentModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        _InvoiceAdjustmentModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }
                        if (ViewBag.AppLevel != null && _InvoiceAdjustmentModel1.Command != "Edit")
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
                                    _InvoiceAdjustmentModel1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message"] != null)
                                            {
                                                ViewBag.Message = TempData["Message"];
                                                _InvoiceAdjustmentModel1.BtnName = "Refresh";
                                            }
                                            else if (TempData["FBMessage"] != null)
                                            {
                                                ViewBag.MessageFB = TempData["FBMessage"];
                                                _InvoiceAdjustmentModel1.BtnName = "Refresh";
                                            }
                                            else
                                            {
                                                _InvoiceAdjustmentModel1.BtnName = "BtnToDetailPage";
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //_InvoiceAdjustmentModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                        }
                                        if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _InvoiceAdjustmentModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message"] != null)
                                    {
                                        ViewBag.Message = TempData["Message"];
                                    }
                                    if (TempData["FBMessage"] != null)
                                    {
                                        ViewBag.MessageFB = TempData["FBMessage"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _InvoiceAdjustmentModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                        }
                                        if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _InvoiceAdjustmentModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message"] != null)
                                    {
                                        ViewBag.Message = TempData["Message"];
                                    }
                                    if (TempData["FBMessage"] != null)
                                    {
                                        ViewBag.MessageFB = TempData["FBMessage"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _InvoiceAdjustmentModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
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
                                    _InvoiceAdjustmentModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _InvoiceAdjustmentModel1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _InvoiceAdjustmentModel1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                            /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
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
                        ViewBag.TotalVouAmt = ds.Tables[1].Rows[0]["TotAdjAmt"].ToString();
                        //ViewBag.TotalVouAmt = Convert.ToDecimal(ds.Tables[1].Rows[0]["TotAdjAmt"]).ToString(ValDigit);
                        ViewBag.MenuPageName = getDocumentName();
                        _InvoiceAdjustmentModel1.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/InvoiceAdjustment/InvoiceAdjustment.cshtml", _InvoiceAdjustmentModel1);
                    }
                    else
                    {
                        _InvoiceAdjustmentModel1.Vou_Date = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                        ViewBag.MenuPageName = getDocumentName();
                        _InvoiceAdjustmentModel1.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/InvoiceAdjustment/InvoiceAdjustment.cshtml", _InvoiceAdjustmentModel1);
                    }
                }
                else
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(Comp_ID, Br_ID, DocumentMenuId).Tables[0];
                    if (_urlModel != null)
                    {
                        if (_urlModel.bt == "D")
                        {
                            _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _InvoiceAdjustmentModel.BtnName = _urlModel.bt;
                        }
                        _InvoiceAdjustmentModel.InvoiceAdjustmentNo = _urlModel.In_No;
                        _InvoiceAdjustmentModel.InvoiceAdjustmentDate = _urlModel.In_DT;
                        _InvoiceAdjustmentModel.Command = _urlModel.Cmd;
                        _InvoiceAdjustmentModel.TransType = _urlModel.tp;
                        _InvoiceAdjustmentModel.WF_Status1 = _urlModel.wf;
                        _InvoiceAdjustmentModel.DocumentStatus = _urlModel.DMS;
                    }
                    /* Add by Hina on 28-02-2024 to Refresh Page*/
                    if (_InvoiceAdjustmentModel.TransType == null)
                    {
                        _InvoiceAdjustmentModel.BtnName = "Refresh";
                        _InvoiceAdjustmentModel.Command = "Refresh";
                        _InvoiceAdjustmentModel.TransType = "Refresh";

                    }
                    /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                    //if (Session["CompId"] != null)
                    //    CompID = Session["CompId"].ToString();
                    //if (Session["BranchId"] != null)
                    //    Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    ////var VouDate = _InvoiceAdjustmentModel.Vou_Date;
                    //var VouDate = "";

                    //if (_InvoiceAdjustmentModel.Vou_Date != null)
                    //{
                    //    VouDate = _InvoiceAdjustmentModel.Vou_Date;

                    //}
                    //else
                    //{
                    //    DateTime dtnow = DateTime.Now;
                    //    string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                    //    _InvoiceAdjustmentModel.Vou_Date = CurrentDate;
                    //    _InvoiceAdjustmentModel.Vou_Date = CurrentDate;
                    //    VouDate = _InvoiceAdjustmentModel.Vou_Date;
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
                    List<EntityName> _ListEntity = new List<EntityName>();
                    _ListEntity.Insert(0, new EntityName() { id = "0", name = "---Select---" });
                    _InvoiceAdjustmentModel.EntityNameList = _ListEntity;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _InvoiceAdjustmentModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_InvoiceAdjustmentModel.TransType == "Update" || _InvoiceAdjustmentModel.TransType == "Edit")
                    {

                        string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        //string VouNo = Session["InvoiceAdjustmentNo"].ToString();
                        //string VouDt = Session["InvoiceAdjustmentDate"].ToString();
                        string VouNo = _InvoiceAdjustmentModel.InvoiceAdjustmentNo;
                        string VouDt = _InvoiceAdjustmentModel.InvoiceAdjustmentDate;
                        DataSet ds = InvoiceAdjustment_ISERVICE.GetInvoiceAdjustmentDetail(VouNo, VouDt, Comp_ID, Br_ID, UserID, DocumentMenuId);
                        _InvoiceAdjustmentModel.Entity_id = ds.Tables[0].Rows[0]["entity_name"].ToString();
                        _InvoiceAdjustmentModel.Vou_No = ds.Tables[0].Rows[0]["vou_no"].ToString();
                        _InvoiceAdjustmentModel.Vou_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["vou_dt"].ToString()).ToString("yyyy-MM-dd");
                        _InvoiceAdjustmentModel.EntityType = ds.Tables[0].Rows[0]["entity_type"].ToString();
                        _InvoiceAdjustmentModel.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _InvoiceAdjustmentModel.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _InvoiceAdjustmentModel.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _InvoiceAdjustmentModel.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _InvoiceAdjustmentModel.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _InvoiceAdjustmentModel.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _InvoiceAdjustmentModel.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _InvoiceAdjustmentModel.VouStatus = ds.Tables[0].Rows[0]["app_status"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        _InvoiceAdjustmentModel.AdvanceAdjDetails = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                        _InvoiceAdjustmentModel.BillAdjdetails = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        _InvoiceAdjustmentModel.Status = Statuscode;
                        //Session["DocumentStatus"] = Statuscode;
                        _InvoiceAdjustmentModel.DocumentStatus = Statuscode;

                        List<EntityName> _EntityList = new List<EntityName>();
                        dt = GetEntity1(_InvoiceAdjustmentModel.EntityType);
                        foreach (DataRow dr in dt.Rows)
                        {
                            EntityName EntityName = new EntityName();
                            EntityName.id = dr["id"].ToString();
                            EntityName.name = dr["name"].ToString();
                            _EntityList.Add(EntityName);

                        }
                        _EntityList.Insert(0, new EntityName() { id = "0", name = "---Select---" });
                        _InvoiceAdjustmentModel.EntityNameList = _EntityList;

                        List<AdvanceList> _AdvanceDetail_List = new List<AdvanceList>();
                        List<BillsList> _BillsDetail_List = new List<BillsList>();

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[1].Rows)
                            {
                                AdvanceList adv_list = new AdvanceList();
                                adv_list.Vou_No = row["adv_vou_no"].ToString();
                                adv_list.Vou_Dt = row["vou_date"].ToString();
                                adv_list.Adv_Curr = row["curr"].ToString();
                                adv_list.AdvCurrId = row["currid"].ToString();
                                adv_list.Adv_Amt_Bs = row["amt_bs"].ToString();
                                adv_list.Adv_Amt_Sp = row["amt_sp"].ToString();
                                adv_list.Adv_Un_Adj_Amt = row["un_adj_amt"].ToString();
                                if (row["adj_amt"].ToString() == "0")
                                {
                                    adv_list.Adv_Adj_Amt = "";
                                }
                                else
                                {
                                    adv_list.Adv_Adj_Amt = row["adj_amt"].ToString();
                                }                                   
                                adv_list.Adv_Rem_Bal = row["pend_amt"].ToString();
                                _AdvanceDetail_List.Add(adv_list);
                            }
                            _InvoiceAdjustmentModel.Advance_List = _AdvanceDetail_List;
                        }

                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[2].Rows)
                            {
                                BillsList Bills_list = new BillsList();
                                Bills_list.Inv_No = row["inv_no"].ToString();
                                Bills_list.Inv_Dt = row["inv_date"].ToString();
                                Bills_list.Bill_No = row["bill_no"].ToString();
                                Bills_list.Bill_Dt = row["bill_dt"].ToString();
                                Bills_list.Inv_Curr = row["curr"].ToString();
                                Bills_list.InvCurrId = row["currid"].ToString();
                                Bills_list.Inv_Amt_Bs = row["inv_amt_bs"].ToString();
                                Bills_list.Inv_Amt_Sp = row["inv_amt_sp"].ToString();
                                Bills_list.Inv_Un_Adj_Amt = row["un_adj_amt"].ToString();
                                if (row["adj_amt"].ToString() == "0")
                                {
                                    Bills_list.Inv_Adj_Amt ="";
                                }
                                else
                                {
                                    Bills_list.Inv_Adj_Amt = row["adj_amt"].ToString();
                                }
                                   
                                Bills_list.Inv_Rem_Bal = row["pend_amt"].ToString();
                                _BillsDetail_List.Add(Bills_list);
                            }
                            _InvoiceAdjustmentModel.Bills_List = _BillsDetail_List;
                        }
                        if (_InvoiceAdjustmentModel.Status == "C")
                        {
                            _InvoiceAdjustmentModel.CancelFlag = true;
                            _InvoiceAdjustmentModel.BtnName = "Refresh";
                        }
                        else
                        {
                            _InvoiceAdjustmentModel.CancelFlag = false;
                        }
                        _InvoiceAdjustmentModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        _InvoiceAdjustmentModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }
                        if (ViewBag.AppLevel != null && _InvoiceAdjustmentModel.Command != "Edit")
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
                                    _InvoiceAdjustmentModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
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
                                        _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                            _InvoiceAdjustmentModel.BtnName = "Refresh";
                                        }
                                        else if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                            _InvoiceAdjustmentModel.BtnName = "Refresh";
                                        }
                                        else
                                        {
                                            _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                                        }
                                        /*End to chk Financial year exist or not*/
                                       // _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message"] != null)
                                    {
                                        ViewBag.Message = TempData["Message"];
                                    }
                                    if (TempData["FBMessage"] != null)
                                    {
                                        ViewBag.MessageFB = TempData["FBMessage"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message"] != null)
                                        {
                                            ViewBag.Message = TempData["Message"];
                                        }
                                        if (TempData["FBMessage"] != null)
                                        {
                                            ViewBag.MessageFB = TempData["FBMessage"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message"] != null)
                                    {
                                        ViewBag.Message = TempData["Message"];
                                    }
                                    if (TempData["FBMessage"] != null)
                                    {
                                        ViewBag.MessageFB = TempData["FBMessage"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
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
                                    _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _InvoiceAdjustmentModel.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                            /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
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
                        ViewBag.TotalVouAmt = ds.Tables[1].Rows[0]["TotAdjAmt"].ToString();
                        //ViewBag.TotalVouAmt = Convert.ToDecimal(ds.Tables[1].Rows[0]["TotAdjAmt"]).ToString(ValDigit);
                        ViewBag.MenuPageName = getDocumentName();
                        _InvoiceAdjustmentModel.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/InvoiceAdjustment/InvoiceAdjustment.cshtml", _InvoiceAdjustmentModel);
                    }
                    else
                    {
                        _InvoiceAdjustmentModel.Vou_Date = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                        ViewBag.MenuPageName = getDocumentName();
                        _InvoiceAdjustmentModel.Title = title;
                        ViewBag.VBRoleList = GetRoleList();
                        ViewBag.MenuPageName = getDocumentName();

                        return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/GeneralLedgerVoucher/InvoiceAdjustment/InvoiceAdjustment.cshtml", _InvoiceAdjustmentModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult GetInvoiceAdjustmentList(string docid, string status)
        {

            //Session["WF_status"] = status;
            InvoiceAdjustmentListModel DashBord = new InvoiceAdjustmentListModel();
            DashBord.WF_Status = status;
            return RedirectToAction("InvoiceAdjustmentList", DashBord);
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
        public ActionResult AddInvoiceAdjustmentDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            InvoiceAdjustmentModel AddNewModel = new InvoiceAdjustmentModel();
            /*start Add by Hina on 01-04-2025 to chk Financial year exist or not*/
            //if (Session["CompId"] != null)
            //    CompID = Session["CompId"].ToString();
            //if (Session["BranchId"] != null)
            //    Br_ID = Session["BranchId"].ToString();
            //DateTime dtnow = DateTime.Now;
            //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month,dtnow.Day).ToString("yyyy-MM-dd");
            //var commCont = new CommonController(_Common_IServices);

            //string MsgNew = string.Empty;
            //MsgNew = commCont.Fin_CheckFinancialYear(CompID, Br_ID, CurrentDate);
            //if (MsgNew == "FY Not Exist")
            //{
            //    TempData["Message"] = "Financial Year not Exist";
            //    return RedirectToAction("InvoiceAdjustmentList", AddNewModel);
            //}
            //if (MsgNew == "FB Close")
            //{
            //    TempData["FBMessage"] = "Financial Book Closing";
            //    return RedirectToAction("InvoiceAdjustmentList", AddNewModel);
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
            AddNew_Model.DMS = "D";
            TempData["ListFilterData"] = null;
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("InvoiceAdjustmentDetail", "InvoiceAdjustment", AddNew_Model);
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
        {/*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
            InvoiceAdjustmentModel dblclick = new InvoiceAdjustmentModel();
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
            //Session["InvoiceAdjustmentNo"] = VouNo;
            //Session["InvoiceAdjustmentDate"] = Voudt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            //InvoiceAdjustmentModel dblclick = new InvoiceAdjustmentModel();
            UrlModel _url = new UrlModel();
            dblclick.InvoiceAdjustmentNo = VouNo;
            dblclick.InvoiceAdjustmentDate = Voudt;
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
            _url.In_No = VouNo;
            _url.In_DT = Voudt;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("InvoiceAdjustmentDetail", "InvoiceAdjustment", _url);
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
        public ActionResult GetEntity(string EntityType)
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
                //var Transtype = Session["TransType"].ToString();
                //EntityType = _InvoiceAdjustmentModel.EntityType;                
                DataTable dt = InvoiceAdjustment_ISERVICE.GetEntity(CompID, BrchID, EntityType);
                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public DataTable GetEntity1(string EntityType)
        {
            try
            {
                //JsonResult DataRows = null;
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
                //var Transtype = Session["TransType"].ToString();
                //EntityType = _InvoiceAdjustmentModel.EntityType;                
                DataTable dt = InvoiceAdjustment_ISERVICE.GetEntity(CompID, BrchID, EntityType);
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
        public ActionResult SearchBillsDetail(string entity_id, string entity_type)
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
                InvoiceAdjustmentModel _InvoiceAdjustmentModel = new InvoiceAdjustmentModel();
                _InvoiceAdjustmentModel.DocumentStatus = "D";
                _InvoiceAdjustmentModel.Command = "Edit";
                _InvoiceAdjustmentModel.TransType = "Update";
                List<AdvanceList> _AdvanceDetail_List = new List<AdvanceList>();
                List<BillsList> _BillsDetail_List = new List<BillsList>();
                DataSet dtdata = InvoiceAdjustment_ISERVICE.GetAdv_Inv_Details(CompID, Br_ID, entity_id, entity_type);

                if (dtdata.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dtdata.Tables[0].Rows)
                    {
                        AdvanceList adv_list = new AdvanceList();
                        adv_list.Vou_No = row["Vou_no"].ToString();
                        adv_list.Vou_Dt = row["vou_date"].ToString();
                        adv_list.Adv_Curr = row["curr"].ToString();
                        adv_list.AdvCurrId = row["currid"].ToString();
                        adv_list.Adv_Amt_Bs = row["amt_bs"].ToString();
                        adv_list.Adv_Amt_Sp = row["amt_sp"].ToString();
                        adv_list.Adv_Un_Adj_Amt = row["un_adj_amt"].ToString();
                        #region Commented By Nitesh 11-01-2024 for Not Convert in intger
                        #endregion
                        //if (Convert.ToInt32(row["adj_amt"]) == 0)
                        //{
                        //    adv_list.Adv_Adj_Amt = "";
                        //}
                        //else
                        //{
                        adv_list.Adv_Adj_Amt = row["adj_amt"].ToString();
                       // }
                        adv_list.Adv_Rem_Bal = row["pend_amt"].ToString();
                        _AdvanceDetail_List.Add(adv_list);
                    }
                    _InvoiceAdjustmentModel.Advance_List = _AdvanceDetail_List;
                }

                if (dtdata.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in dtdata.Tables[1].Rows)
                    {
                        BillsList Bills_list = new BillsList();
                        Bills_list.Inv_No = row["inv_no"].ToString();
                        Bills_list.Inv_Dt = row["inv_date"].ToString();
                        if (entity_type == "S")
                        {
                            Bills_list.Bill_No = row["bill_no"].ToString();
                            Bills_list.Bill_Dt = row["bill_dt"].ToString();
                        }
                        Bills_list.Inv_Curr = row["curr"].ToString();
                        Bills_list.InvCurrId = row["currid"].ToString();
                        Bills_list.Inv_Amt_Bs = row["inv_amt_bs"].ToString();
                        Bills_list.Inv_Amt_Sp = row["inv_amt_sp"].ToString();
                        Bills_list.Inv_Un_Adj_Amt = row["un_adj_amt"].ToString();
                        //if (Convert.ToInt32(row["adj_amt"]) == 0)
                        //{
                        //    Bills_list.Inv_Adj_Amt = "";
                        //}
                        //else
                        //{
                            Bills_list.Inv_Adj_Amt = row["adj_amt"].ToString();
                       // }                            
                        Bills_list.Inv_Rem_Bal = row["pend_amt"].ToString();
                        _BillsDetail_List.Add(Bills_list);
                    }
                    _InvoiceAdjustmentModel.Bills_List = _BillsDetail_List;
                }
                _InvoiceAdjustmentModel.EntityType = entity_type;
                
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialInvoiceAdjustment.cshtml", _InvoiceAdjustmentModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult InvoiceAdjustmentSaveCommand(InvoiceAdjustmentModel _InvoiceAdjustmentModel, string Vou_No, string command)
        {
            try
            {
                /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                string Msg = string.Empty;
                /*End to chk Financial year exist or not*/
                if (_InvoiceAdjustmentModel.DeleteCommand == "Delete")
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
                        InvoiceAdjustmentModel adddnew = new InvoiceAdjustmentModel();
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
                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                        //if (Session["CompId"] != null)
                        //    CompID = Session["CompId"].ToString();
                        //if (Session["BranchId"] != null)
                        //    Br_ID = Session["BranchId"].ToString();
                        ////string Voudt = _InvoiceAdjustmentModel.Vou_Date;
                        //DateTime dtnow = DateTime.Now;
                        //string CurrentDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");

                        //Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, CurrentDate);
                        
                        //if (Msg == "FY Not Exist" || Msg == "FB Close")
                        //{
                            //if (Msg == "FY Not Exist")
                            //{
                            //    TempData["Message"] = "Financial Year not Exist";
                            //    adddnew.BtnName = "Refresh";
                            //    adddnew.Command = "Refresh";
                            //    adddnew.Vou_Date = CurrentDate;
                            //    return RedirectToAction("InvoiceAdjustmentDetail", "InvoiceAdjustment", adddnew);
                            //}
                            //else
                            //{
                            //    TempData["FBMessage"] = "Financial Book Closing";
                            //    adddnew.BtnName = "Refresh";
                            //    adddnew.Command = "Refresh";
                            //    adddnew.Vou_Date = CurrentDate;
                            //    //return RedirectToAction("InvoiceAdjustmentDetail", "InvoiceAdjustment", adddnew);
                            //}
                            //if (!string.IsNullOrEmpty(_InvoiceAdjustmentModel.Vou_No))
                            //    return RedirectToAction("EditVou", new { VouNo = _InvoiceAdjustmentModel.Vou_No, Voudt = _InvoiceAdjustmentModel.Vou_Date, ListFilterData = _InvoiceAdjustmentModel.ListFilterData1, WF_Status = _InvoiceAdjustmentModel.WFStatus });
                            //else
                            //    _InvoiceAdjustmentModel.Command = "Refresh";
                            //_InvoiceAdjustmentModel.TransType = "Refresh";
                            //_InvoiceAdjustmentModel.BtnName = "Refresh";
                            //_InvoiceAdjustmentModel.DocumentStatus = null;
                            //TempData["ModelData"] = _InvoiceAdjustmentModel;
                            //return RedirectToAction("InvoiceAdjustmentDetail", "InvoiceAdjustment", _InvoiceAdjustmentModel);

                        //}
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("InvoiceAdjustmentDetail", "InvoiceAdjustment", NewModel);

                    case "Edit":
                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt1 = _InvoiceAdjustmentModel.Vou_Date;
                        Msg = commCont.Fin_CheckFinancialYear(CompID, Br_ID, Voudt1);
                        if (Msg == "FY Not Exist" || Msg == "FB Close")
                        {
                            if (_InvoiceAdjustmentModel.Status == "A" || _InvoiceAdjustmentModel.Status == "D")
                            {
                                if (Msg == "FY Not Exist")
                                {
                                    TempData["Message"] = "Financial Year not Exist";
                                }
                                else
                                {
                                    TempData["FBMessage"] = "Financial Book Closing";
                                }
                                return RedirectToAction("EditVou", new { VouNo = _InvoiceAdjustmentModel.Vou_No, Voudt = _InvoiceAdjustmentModel.Vou_Date, ListFilterData = _InvoiceAdjustmentModel.ListFilterData1, WF_Status = _InvoiceAdjustmentModel.WFStatus });
                            }
                        }
                        /*End to chk Financial year exist or not*/
                        //if (CheckAdvancePayment(_InvoiceAdjustmentModel.Vou_No, _InvoiceAdjustmentModel.Vou_Date) == "Used")
                        //{
                        //    Session["Message"] = "Used";
                        //}
                        //else
                        //{
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["BtnName"] = "BtnEdit";
                        //Session["Message"] = "New";
                        //Session["AppStatus"] = 'D';
                        //Session["InvoiceAdjustmentNo"] = _InvoiceAdjustmentModel.Vou_No;
                        //Session["InvoiceAdjustmentDate"] = _InvoiceAdjustmentModel.Vou_Date.ToString();
                        _InvoiceAdjustmentModel.TransType = "Update";
                        _InvoiceAdjustmentModel.Command = command;
                        _InvoiceAdjustmentModel.BtnName = "BtnEdit";
                        _InvoiceAdjustmentModel.InvoiceAdjustmentNo = _InvoiceAdjustmentModel.Vou_No;
                        _InvoiceAdjustmentModel.InvoiceAdjustmentDate = _InvoiceAdjustmentModel.Vou_Date;
                        TempData["ModelData"] = _InvoiceAdjustmentModel;
                        UrlModel EditModel = new UrlModel();
                        EditModel.tp = "Update";
                        EditModel.Cmd = command;
                        EditModel.bt = "BtnEdit";
                        EditModel.In_No = _InvoiceAdjustmentModel.Vou_No;
                        EditModel.In_DT = _InvoiceAdjustmentModel.Vou_Date;
                        //}
                        TempData["ListFilterData"] = _InvoiceAdjustmentModel.ListFilterData1;
                        return RedirectToAction("InvoiceAdjustmentDetail", EditModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        Vou_No = _InvoiceAdjustmentModel.Vou_No;
                        InvoiceAdjustmentDelete(_InvoiceAdjustmentModel, command);
                        InvoiceAdjustmentModel DeleteModel = new InvoiceAdjustmentModel();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete = new UrlModel();
                        Delete.Cmd = DeleteModel.Command;
                        Delete.tp = "Refresh";
                        Delete.bt = "BtnDelete";
                        TempData["ListFilterData"] = _InvoiceAdjustmentModel.ListFilterData1;
                        return RedirectToAction("InvoiceAdjustmentDetail", Delete);


                    case "Save":
                        // Session["Command"] = command;
                        SaveInvoiceAdj(_InvoiceAdjustmentModel);
                        if (_InvoiceAdjustmentModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        //Session["InvoiceAdjustmentNo"] = Session["InvoiceAdjustmentNo"].ToString();
                        //Session["InvoiceAdjustmentDate"] = Session["InvoiceAdjustmentDate"].ToString();
                        TempData["ModelData"] = _InvoiceAdjustmentModel;
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.bt = _InvoiceAdjustmentModel.BtnName;
                        _urlModel.In_No = _InvoiceAdjustmentModel.InvoiceAdjustmentNo;
                        _urlModel.In_DT = _InvoiceAdjustmentModel.InvoiceAdjustmentDate;
                        _urlModel.tp = _InvoiceAdjustmentModel.TransType;
                        TempData["ListFilterData"] = _InvoiceAdjustmentModel.ListFilterData1;
                        return RedirectToAction("InvoiceAdjustmentDetail", _urlModel);

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        string Voudt3 = _InvoiceAdjustmentModel.Vou_Date;

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
                            return RedirectToAction("EditVou", new { VouNo = _InvoiceAdjustmentModel.Vou_No, Voudt = _InvoiceAdjustmentModel.Vou_Date, ListFilterData = _InvoiceAdjustmentModel.ListFilterData1, WF_Status = _InvoiceAdjustmentModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //  Session["Command"] = command;
                        _InvoiceAdjustmentModel.Command = command;
                        Vou_No = _InvoiceAdjustmentModel.Vou_No;
                        //Session["InvoiceAdjustmentNo"] = Vou_No;
                        //Session["InvoiceAdjustmentDate"] = _InvoiceAdjustmentModel.Vou_Date;
                        InvoiceAdjustmentApprove(_InvoiceAdjustmentModel, _InvoiceAdjustmentModel.Vou_No, _InvoiceAdjustmentModel.Vou_Date, "", "", "", "", "");
                        TempData["ModelData"] = _InvoiceAdjustmentModel;
                        UrlModel urlref = new UrlModel();
                        urlref.tp = "Update";
                        urlref.In_No = _InvoiceAdjustmentModel.InvoiceAdjustmentNo;
                        urlref.In_DT = _InvoiceAdjustmentModel.InvoiceAdjustmentDate;
                        urlref.bt = "BtnEdit";
                        TempData["ListFilterData"] = _InvoiceAdjustmentModel.ListFilterData1;
                        return RedirectToAction("InvoiceAdjustmentDetail", urlref);

                    case "Refresh":
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = "Refresh";
                        //Session["DocumentStatus"] = 'D';
                        InvoiceAdjustmentModel RefreshModel = new InvoiceAdjustmentModel();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "Refresh";
                        RefreshModel.TransType = "Save";
                        RefreshModel.DocumentStatus = "D";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh = new UrlModel();
                        refesh.tp = "Save";
                        refesh.bt = "Refresh";
                        refesh.Cmd = command;
                        TempData["ListFilterData"] = _InvoiceAdjustmentModel.ListFilterData1;
                        return RedirectToAction("InvoiceAdjustmentDetail", refesh);

                    case "Print":
                        return GenratePdfFile(_InvoiceAdjustmentModel);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        InvoiceAdjustmentListModel _Backtolist = new InvoiceAdjustmentListModel();
                        _Backtolist.WF_Status = _InvoiceAdjustmentModel.WF_Status1;
                        TempData["ListFilterData"] = _InvoiceAdjustmentModel.ListFilterData1;
                        return RedirectToAction("InvoiceAdjustmentList", "InvoiceAdjustment", _Backtolist);

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
        public ActionResult SaveInvoiceAdj(InvoiceAdjustmentModel _InvoiceAdjustmentModel)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (_InvoiceAdjustmentModel.CancelFlag == false)
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
                    DataTable InvoiceAdjustmentHeader = new DataTable();
                    DataTable InvoiceAdjustmentAdvDetails = new DataTable();
                    DataTable InvoiceAdjustmentBillsDetail = new DataTable();

                    DataTable dtheader = new DataTable();

                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("user_id", typeof(int));
                    dtheader.Columns.Add("vou_no", typeof(string));
                    dtheader.Columns.Add("vou_dt", typeof(string));
                    dtheader.Columns.Add("entity_type", typeof(string));
                    dtheader.Columns.Add("entity_name", typeof(string));
                    dtheader.Columns.Add("vou_status", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));


                    DataRow dtrowHeader = dtheader.NewRow();
                    // dtrowHeader["TransType"] = Session["TransType"].ToString();
                    if (_InvoiceAdjustmentModel.Vou_No != null)
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
                    dtrowHeader["vou_no"] = _InvoiceAdjustmentModel.Vou_No;
                    dtrowHeader["vou_dt"] = _InvoiceAdjustmentModel.Vou_Date;
                    dtrowHeader["entity_type"] = _InvoiceAdjustmentModel.EntityType;
                    dtrowHeader["entity_name"] = _InvoiceAdjustmentModel.Entity_id;
                    dtrowHeader["vou_status"] = "D";
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrowHeader["mac_id"] = mac_id;


                    dtheader.Rows.Add(dtrowHeader);
                    InvoiceAdjustmentHeader = dtheader;

                    DataTable dtAccount = new DataTable();

                    dtAccount.Columns.Add("vou_no", typeof(string));
                    dtAccount.Columns.Add("vou_dt", typeof(string));
                    dtAccount.Columns.Add("curr", typeof(int));
                    dtAccount.Columns.Add("adv_amt_bs", typeof(string));
                    dtAccount.Columns.Add("adv_amt_sp", typeof(string));
                    dtAccount.Columns.Add("un_adj_amt", typeof(string));
                    dtAccount.Columns.Add("adj_amt", typeof(string));
                    dtAccount.Columns.Add("rem_bal", typeof(string));

                    JArray jObject = JArray.Parse(_InvoiceAdjustmentModel.AdvanceAdjDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtAccount.NewRow();

                        dtrowLines["vou_no"] = jObject[i]["Vou_no"].ToString();
                        dtrowLines["vou_dt"] = jObject[i]["Vou_dt"].ToString();
                        dtrowLines["curr"] = jObject[i]["Curr"].ToString();
                        if (jObject[i]["AdvInvAmtBs"].ToString() == "")
                        {
                            dtrowLines["adv_amt_bs"] = 0;
                        }
                        else
                        {
                            dtrowLines["adv_amt_bs"] = jObject[i]["AdvInvAmtBs"].ToString();
                        }
                        if (jObject[i]["AdvInvAmtSp"].ToString() == "")
                        {
                            dtrowLines["adv_amt_sp"] = 0;
                        }
                        else
                        {
                            dtrowLines["adv_amt_sp"] = jObject[i]["AdvInvAmtSp"].ToString();
                        }
                        if (jObject[i]["Adv_Un_Adj_Amt"].ToString() == "")
                        {
                            dtrowLines["un_adj_amt"] = 0;
                        }
                        else
                        {
                            dtrowLines["un_adj_amt"] = jObject[i]["Adv_Un_Adj_Amt"].ToString();
                        }

                        if (jObject[i]["Adv_Adj_Amt"].ToString() == "")
                        {
                            dtrowLines["adj_amt"] = 0;
                        }
                        else
                        {
                            dtrowLines["adj_amt"] = jObject[i]["Adv_Adj_Amt"].ToString();
                        }
                        if (jObject[i]["AdvRemBal"].ToString() == "")
                        {
                            dtrowLines["rem_bal"] = 0;
                        }
                        else
                        {
                            dtrowLines["rem_bal"] = jObject[i]["AdvRemBal"].ToString();
                        }
                        dtAccount.Rows.Add(dtrowLines);
                    }
                    InvoiceAdjustmentAdvDetails = dtAccount;

                    DataTable dtBillAdjDetail = new DataTable();

                    dtBillAdjDetail.Columns.Add("inv_no", typeof(string));
                    dtBillAdjDetail.Columns.Add("inv_dt", typeof(string));
                    dtBillAdjDetail.Columns.Add("bill_no", typeof(string));//Added by Suraj on 03-05-2024
                    dtBillAdjDetail.Columns.Add("bill_dt", typeof(string));//Added by Suraj on 03-05-2024
                    dtBillAdjDetail.Columns.Add("curr", typeof(int));
                    dtBillAdjDetail.Columns.Add("inv_amt_bs", typeof(string));
                    dtBillAdjDetail.Columns.Add("inv_amt_sp", typeof(string));
                    dtBillAdjDetail.Columns.Add("un_adj_amt", typeof(string));
                    dtBillAdjDetail.Columns.Add("adj_amt", typeof(string));
                    dtBillAdjDetail.Columns.Add("rem_bal", typeof(string));

                    JArray BObject = JArray.Parse(_InvoiceAdjustmentModel.BillAdjdetails);

                    for (int i = 0; i < BObject.Count; i++)
                    {
                        DataRow dtrowLines = dtBillAdjDetail.NewRow();

                        dtrowLines["inv_no"] = BObject[i]["Inv_no"].ToString();
                        dtrowLines["inv_dt"] = BObject[i]["Inv_dt"].ToString();
                        if (_InvoiceAdjustmentModel.EntityType == "S")
                        {
                            dtrowLines["bill_no"] = BObject[i]["Bill_no"].ToString();
                            dtrowLines["bill_dt"] = BObject[i]["Bill_dt"].ToString();
                        }
                        else
                        {
                            dtrowLines["bill_no"] = null;
                            dtrowLines["bill_dt"] = null;
                        }
              
                        dtrowLines["curr"] = BObject[i]["Curr"].ToString();
                        if (BObject[i]["InvAmtBs"].ToString() == "")
                        {
                            dtrowLines["inv_amt_bs"] = 0;
                        }
                        else
                        {
                            dtrowLines["inv_amt_bs"] = BObject[i]["InvAmtBs"].ToString();

                        }
                        if (BObject[i]["InvAmtSp"].ToString() == "")
                        {
                            dtrowLines["inv_amt_sp"] = 0;
                        }
                        else
                        {
                            dtrowLines["inv_amt_sp"] = BObject[i]["InvAmtSp"].ToString();
                        }
                        if (BObject[i]["InvUnAdjAmt"].ToString() == "")
                        {
                            dtrowLines["un_adj_amt"] = 0;
                        }
                        else
                        {
                            dtrowLines["un_adj_amt"] = BObject[i]["InvUnAdjAmt"].ToString();
                        }
                        if (BObject[i]["Inv_Paid_Amt"].ToString() == "")
                        {
                            dtrowLines["adj_amt"] = 0;
                        }
                        else
                        {
                            dtrowLines["adj_amt"] = BObject[i]["Inv_Paid_Amt"].ToString();
                        }

                        if (BObject[i]["InvRemBal"].ToString() == "")
                        {
                            dtrowLines["rem_bal"] = 0;
                        }
                        else
                        {
                            dtrowLines["rem_bal"] = BObject[i]["InvRemBal"].ToString();
                        }
                        dtBillAdjDetail.Rows.Add(dtrowLines);
                    }
                    InvoiceAdjustmentBillsDetail = dtBillAdjDetail;


                    SaveMessage = InvoiceAdjustment_ISERVICE.InsertInvoiceAdjustmentDetail(InvoiceAdjustmentHeader, InvoiceAdjustmentAdvDetails, InvoiceAdjustmentBillsDetail);

                    string InvoiceAdjustmentNo = SaveMessage.Split(',')[1].Trim();
                    string BP_Number = InvoiceAdjustmentNo.Replace("/", "");
                    string Message = SaveMessage.Split(',')[0].Trim();
                    string InvoiceAdjustmentDate = SaveMessage.Split(',')[2].Trim();
                    if (Message == "Data_Not_Found")
                    {
                        //var a = SaveMessage.Split(',');
                        var msg = Message.Replace("_", " ") + " " + InvoiceAdjustmentNo+" in "+PageName;//InvoiceAdjustmentNo is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _InvoiceAdjustmentModel.Message = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("InvoiceAdjustmentDetail");
                    }

                    if (Message == "Update" || Message == "Save")
                        //    Session["Message"] = "Save";
                        //Session["Command"] = "Update";
                        //Session["InvoiceAdjustmentNo"] = InvoiceAdjustmentNo;
                        //Session["InvoiceAdjustmentDate"] = InvoiceAdjustmentDate;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnToDetailPage";
                        _InvoiceAdjustmentModel.Message = "Save";
                    _InvoiceAdjustmentModel.InvoiceAdjustmentNo = InvoiceAdjustmentNo;
                    _InvoiceAdjustmentModel.InvoiceAdjustmentDate = InvoiceAdjustmentDate;
                    _InvoiceAdjustmentModel.TransType = "Update";
                    _InvoiceAdjustmentModel.BtnName = "BtnToDetailPage";
                    return RedirectToAction("InvoiceAdjustmentDetail");
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
                    _InvoiceAdjustmentModel.Create_by = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet SaveMessage1 = InvoiceAdjustment_ISERVICE.InvoiceAdjustmentCancel(_InvoiceAdjustmentModel, CompID, br_id, mac_id);

                    string fileName = "IA_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(_InvoiceAdjustmentModel.Vou_No, _InvoiceAdjustmentModel.Vou_Date, fileName);
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _InvoiceAdjustmentModel.Vou_No, "C", UserID, "", filePath);

                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["InvoiceAdjustmentNo"] = _InvoiceAdjustmentModel.Vou_No;
                    //Session["InvoiceAdjustmentDate"] = _InvoiceAdjustmentModel.Vou_Date;
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    _InvoiceAdjustmentModel.Message = "Cancelled";

                    _InvoiceAdjustmentModel.InvoiceAdjustmentNo = _InvoiceAdjustmentModel.Vou_No;
                    _InvoiceAdjustmentModel.InvoiceAdjustmentDate = _InvoiceAdjustmentModel.Vou_Date;
                    _InvoiceAdjustmentModel.TransType = "Update";
                    _InvoiceAdjustmentModel.BtnName = "Refresh";
                    return RedirectToAction("InvoiceAdjustmentDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
               // return View("~/Views/Shared/Error.cshtml");
            }

        }
        public void GetStatusList(InvoiceAdjustmentListModel _InvAdjListModel)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _InvAdjListModel.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<InvoiceAdjustmentList> GetInvoiceAdjListAll(InvoiceAdjustmentListModel _InvAdjListModel)
        {
            try
            {
                _InvoiceAdjtList = new List<InvoiceAdjustmentList>();
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
                if (_InvAdjListModel.WF_Status != null)
                {
                    wfstatus = _InvAdjListModel.WF_Status;
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
                Dtdata = InvoiceAdjustment_ISERVICE.GetInvoiceAdjListAll(_InvAdjListModel.EntyType, _InvAdjListModel.EntyId, _InvAdjListModel.VouFromDate, _InvAdjListModel.VouToDate, _InvAdjListModel.Status, CompID, Br_ID, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    _InvAdjListModel.FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (Dtdata.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in Dtdata.Tables[0].Rows)
                    {
                        InvoiceAdjustmentList _InvAdjList = new InvoiceAdjustmentList();
                        _InvAdjList.VouNumber = dr["vou_no"].ToString();
                        _InvAdjList.VouDate = dr["vou_dt"].ToString();
                        _InvAdjList.hdVouDate = dr["vou_date"].ToString();
                        _InvAdjList.EntyType = dr["entity_type"].ToString();
                        _InvAdjList.EntyName = dr["acc_name"].ToString();
                        _InvAdjList.EntyId = dr["entity_name"].ToString();
                        _InvAdjList.VouStatus = dr["vou_status"].ToString();
                        _InvAdjList.CreatedON = dr["created_on"].ToString();
                        _InvAdjList.ApprovedOn = dr["app_dt"].ToString();
                        _InvAdjList.ModifiedOn = dr["mod_on"].ToString();
                        _InvAdjList.create_by = dr["create_by"].ToString();
                        _InvAdjList.mod_by = dr["mod_by"].ToString();
                        _InvAdjList.app_by = dr["app_by"].ToString();

                        _InvoiceAdjtList.Add(_InvAdjList);
                    }
                }
                return _InvoiceAdjtList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchInvoiceAdjDetail(string entity_type, string entity_id, string Fromdate, string Todate, string Status, string CompID, string Br_ID)
        {
            _InvoiceAdjtList = new List<InvoiceAdjustmentList>();
            InvoiceAdjustmentListModel _InvAdjListModel = new InvoiceAdjustmentListModel();
            //Session["WF_status"] = "";
            _InvAdjListModel.WF_Status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            DataSet dt = new DataSet();
            dt = InvoiceAdjustment_ISERVICE.GetInvoiceAdjListAll(entity_type, entity_id, Fromdate, Todate, Status, CompID, Br_ID, "", "", "");
            // Session["VouSearch"] = "Vou_Search";
            _InvAdjListModel.VouSearch = "Vou_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    InvoiceAdjustmentList _InvAdjList = new InvoiceAdjustmentList();
                    _InvAdjList.VouNumber = dr["vou_no"].ToString();
                    _InvAdjList.VouDate = dr["vou_dt"].ToString();
                    _InvAdjList.hdVouDate = dr["vou_date"].ToString();
                    _InvAdjList.EntyType = dr["entity_type"].ToString();
                    _InvAdjList.EntyName = dr["acc_name"].ToString();
                    _InvAdjList.EntyId = dr["entity_name"].ToString();
                    _InvAdjList.VouStatus = dr["vou_status"].ToString();
                    _InvAdjList.CreatedON = dr["created_on"].ToString();
                    _InvAdjList.ApprovedOn = dr["app_dt"].ToString();
                    _InvAdjList.ModifiedOn = dr["mod_on"].ToString();
                    _InvAdjList.create_by = dr["create_by"].ToString();
                    _InvAdjList.mod_by = dr["mod_by"].ToString();
                    _InvAdjList.app_by = dr["app_by"].ToString();

                    _InvoiceAdjtList.Add(_InvAdjList);
                }
            }
            _InvAdjListModel.InvAdjList = _InvoiceAdjtList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialInvoiceAdjustmentList.cshtml", _InvAdjListModel);
        }

        private ActionResult InvoiceAdjustmentDelete(InvoiceAdjustmentModel _InvAdjModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string InvAdjNo = _InvAdjModel.Vou_No;
                //string AdjNumber = InvAdjNo.Replace("/", "");

                string Message = InvoiceAdjustment_ISERVICE.InvAdjDelete(_InvAdjModel, CompID, br_id, DocumentMenuId);

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["InvoiceAdjustmentNo"] = "";
                //Session["InvoiceAdjustmentDate"] = "";
                //_InvAdjModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("InvoiceAdjustmentDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData)
        {
            // Session["Message"] = "";
            InvoiceAdjustmentModel ToRefreshByJS = new InvoiceAdjustmentModel();
            UrlModel urlModel = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.InvoiceAdjustmentNo = a[0].Trim();
            ToRefreshByJS.InvoiceAdjustmentDate = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnToDetailPage";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                urlModel.wf = a[2].Trim();
            }
            urlModel.bt = "D";
            urlModel.In_No = ToRefreshByJS.InvoiceAdjustmentNo;
            urlModel.In_DT = ToRefreshByJS.InvoiceAdjustmentDate;
            urlModel.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("InvoiceAdjustmentDetail", urlModel);
        }
        public ActionResult InvoiceAdjustmentApprove(InvoiceAdjustmentModel _InvoiceAdjustmentModel, string VouNo, string VouDate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_Status1)
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
                /*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
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
                string Message = InvoiceAdjustment_ISERVICE.InvoiceAdjustmentApprove(VouNo, VouDate, UserID, A_Status, A_Level, A_Remarks, Comp_ID, BranchID, mac_id, DocumentMenuId);
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                string InvoiceAdjustmentNo = Message.Split(',')[0].Trim();
                string InvoiceAdjustmentDate = Message.Split(',')[1].Trim();
                //Session["InvoiceAdjustmentNo"] = InvoiceAdjustmentNo;
                //Session["InvoiceAdjustmentDate"] = InvoiceAdjustmentDate;
                //Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel ApproveModel = new UrlModel();
                _InvoiceAdjustmentModel.TransType = "Update";
                _InvoiceAdjustmentModel.InvoiceAdjustmentNo = InvoiceAdjustmentNo;
                _InvoiceAdjustmentModel.InvoiceAdjustmentDate = InvoiceAdjustmentDate;
                _InvoiceAdjustmentModel.Message = "Approved";
                _InvoiceAdjustmentModel.BtnName = "BtnEdit";
                string fileName = "IA_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                var filePath = SavePdfDocToSendOnEmailAlert(VouNo, VouDate, fileName);
                _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, VouNo, "AP", UserID, "", filePath);

                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _InvoiceAdjustmentModel.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _InvoiceAdjustmentModel;

                ApproveModel.tp = "Update";
                ApproveModel.In_No = InvoiceAdjustmentNo;
                ApproveModel.In_DT = InvoiceAdjustmentDate;
                ApproveModel.bt = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("InvoiceAdjustmentDetail", "InvoiceAdjustment", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        /*--------Print---------*/

        public FileResult GenratePdfFile(InvoiceAdjustmentModel _Model)
        {
            return File(GetPdfData(_Model.Vou_No, _Model.Vou_Date), "application/pdf", "CashReceipt.pdf");
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
                DataSet Deatils = _Common_IServices.Cmn_GetGLVoucherPrintDeatils(CompID, Br_ID, vNo, vDate, "IA");
                ViewBag.PageName = "CR";
                ViewBag.Title = "Invoice Adjustment";
                ViewBag.Details = Deatils;
                if (Deatils.Tables[0].Rows.Count > 0)
                {
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
                        //string draftImage = Server.MapPath("~/Content/Images/draft.png");
                        using (var reader1 = new PdfReader(bytes))
                        {
                            using (var ms = new MemoryStream())
                            {
                                using (var stamper = new PdfStamper(reader1, ms))
                                {
                                    //var draftimg = Image.GetInstance(draftImage);
                                    //draftimg.SetAbsolutePosition(100, 0);
                                    //draftimg.ScaleAbsolute(650f, 650f);

                                    int PageCount = reader1.NumberOfPages;
                                    for (int i = 1; i <= PageCount; i++)
                                    {
                                        var content = stamper.GetUnderContent(i);
                                        //if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
                                        //{
                                        //    content.AddImage(draftimg);
                                        //}
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
                else
                {
                    return null;
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
    }

}

















