using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.FinancialAccountingAndControl.PaymentAdvice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.FinancialAccountingAndControl.PaymentAdvice;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.FinancialAccountingAndControl.PaymentAdvice
{
    public class PaymentAdviceController : Controller
    {
        string CompID, BrchID, language, title, UserID = String.Empty;
        string DocumentMenuId = "105104118";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        PaymentAdvice_ISERVICES _PaymentAdvice_ISERVICES;
        List<PaymentAdviceList> _PaymentAdvList;
        public PaymentAdviceController(Common_IServices _Common_IServices, PaymentAdvice_ISERVICES _PaymentAdvice_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._PaymentAdvice_ISERVICES = _PaymentAdvice_ISERVICES;
        }
        // GET: ApplicationLayer/PaymentAdvice
        public ActionResult PaymentAdvice(PaymentAdviceListModel _PayAdvListModel)
        {
            CommonPageDetails();
            ViewBag.DocumentMenuId = DocumentMenuId;
            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            _PayAdvListModel.StatusList = statusLists;
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');

                _PayAdvListModel.VouFromDate = a[0].Trim();
                _PayAdvListModel.VouToDate = a[1].Trim();
                _PayAdvListModel.Status = a[2].Trim();
                if (_PayAdvListModel.Status == "0")
                {
                    _PayAdvListModel.Status = null;
                }

                _PayAdvListModel.ListFilterData = TempData["ListFilterData"].ToString();
                
            }
            _PayAdvListModel.PAList = GetPayAdvListAll(_PayAdvListModel);
            if (_PayAdvListModel.VouFromDate != null)
            {
                _PayAdvListModel.FromDate = _PayAdvListModel.VouFromDate;
            }
            
            //_InvAdjListModel.FromDate = FromDate;

            _PayAdvListModel.Title = title;
            _PayAdvListModel.PASearch = "";

            //ViewBag.MenuPageName = getDocumentName();
            return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/PaymentAdvice/PaymentAdviceList.cshtml", _PayAdvListModel);
        }
       

        //public ActionResult AddPaymentAdviceDetail()
        //{
        //    Session["Message"] = "New";
        //    Session["Command"] = "Add";
        //    Session["AppStatus"] = 'D';
        //    Session["TransType"] = "Save";
        //    Session["BtnName"] = "BtnAddNew";
        //    ViewBag.MenuPageName = getDocumentName();
        //    return RedirectToAction("PaymentAdviceDetail", "PaymentAdvice");
        //}
        public ActionResult AddPaymentAdviceDetail(string DocNo, string DocDate, string ListFilterData)
        {
            try
            {
                UrlData urlData = new UrlData();
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (DocNo == null)
                {
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {

                        TempData["Message"] = "Financial Year not Exist";
                        SetUrlData(urlData, "", "", "", "Financial Year not Exist", DocNo, DocDate, ListFilterData);
                        return RedirectToAction("PaymentAdvice", "PaymentAdvice", urlData);
                    }
                }

                /*End to chk Financial year exist or not*/
                string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = DocNo == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, DocDate, ListFilterData);
                return RedirectToAction("PaymentAdviceDetail", "PaymentAdvice", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult PaymentAdviceDetail(UrlData urlData)
        {
            try
            {
                CommonPageDetails();
                PaymentAdviceModel _PaymentAdviceModel = new PaymentAdviceModel();
                
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                    UserID = Session["userid"].ToString();
                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.Adv_Dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                _PaymentAdviceModel.AdviceDate = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                _PaymentAdviceModel.ToDate_Dt = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");

                GetAutoCompleteBankDetail(_PaymentAdviceModel);
                DataSet dt = new DataSet();
                dt = GetFindates();
                _PaymentAdviceModel.FromDate_Dt= dt.Tables[0].Rows[0]["StartDate"].ToString();
                
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    _PaymentAdviceModel.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                }
                
                _PaymentAdviceModel.Command = urlData.Command;
                //_PaymentAdviceModel.EditCommand = urlData.Command;
                _PaymentAdviceModel.TransType = urlData.TransType;
                _PaymentAdviceModel.BtnName = urlData.BtnName;
                _PaymentAdviceModel.AdviceNo = urlData.Adv_No;
                _PaymentAdviceModel.AdviceDate = urlData.Adv_Dt;
                _PaymentAdviceModel.WF_Status1 = urlData.wf;
                _PaymentAdviceModel.ListFilterData1 = urlData.ListFilterData1;

                //List<BankAccName> bankLists = new List<BankAccName>();
                //bankLists.Add(new BankAccName { bank_acc_id = _PaymentAdviceModel.bank_acc_id, bank_acc_name = _PaymentAdviceModel.BankName });
                //_PaymentAdviceModel.BankAccNameList = bankLists;

                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;

                if (_PaymentAdviceModel.AdviceNo != null && _PaymentAdviceModel.AdviceDate != null)
                {
                    DataSet ds = GetPayAdvDtlOnView(_PaymentAdviceModel.AdviceNo, _PaymentAdviceModel.AdviceDate);
                   
                    _PaymentAdviceModel.AdviceNo = ds.Tables[0].Rows[0]["padv_no"].ToString();
                    _PaymentAdviceModel.AdviceDate = ds.Tables[0].Rows[0]["padv_dt"].ToString();
                    _PaymentAdviceModel.FromDate_Dt = ds.Tables[0].Rows[0]["from_dt"].ToString();
                    _PaymentAdviceModel.ToDate_Dt = ds.Tables[0].Rows[0]["to_dt"].ToString();
                    //_PaymentAdviceModel.InstrumentType = ds.Tables[0].Rows[0]["ins_typeName"].ToString();
                    //_PaymentAdviceModel.InsTypeID = ds.Tables[0].Rows[0]["ins_type"].ToString();
                    _PaymentAdviceModel.InstrumentType = ds.Tables[0].Rows[0]["ins_type"].ToString();
                    
                    _PaymentAdviceModel.bank_acc_id = ds.Tables[0].Rows[0]["bankacc_id"].ToString();
                    _PaymentAdviceModel.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                    _PaymentAdviceModel.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                    _PaymentAdviceModel.Create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                    _PaymentAdviceModel.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                    _PaymentAdviceModel.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                    _PaymentAdviceModel.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                    _PaymentAdviceModel.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                    _PaymentAdviceModel.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                    _PaymentAdviceModel.PayAdvStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                    _PaymentAdviceModel.Status = ds.Tables[0].Rows[0]["padv_status"].ToString();


                    string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                    string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                    string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                    
                    _PaymentAdviceModel.DocumentStatus = doc_status;
                    
                    ViewBag.DocumentCode = doc_status;
                    
                    _PaymentAdviceModel.DocumentStatus = doc_status;
                    if (doc_status == "C")
                    {
                        _PaymentAdviceModel.CancelFlag = true;
                        _PaymentAdviceModel.BtnName = "Refresh";
                    }
                    else
                    {
                        _PaymentAdviceModel.CancelFlag = false;
                    }

                    List<VouItemList> Item_List = new List<VouItemList>();
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            VouItemList Itm_list = new VouItemList();
                            Itm_list.Vou_No = row["vou_no"].ToString();
                            Itm_list.Vou_Dt = row["vou_dt"].ToString();
                            Itm_list.Vou_Date = row["vou_date"].ToString();
                            Itm_list.BankAccountId = row["bankacc_id"].ToString();
                            Itm_list.BankAccount = row["BankName"].ToString();
                            Itm_list.GLAccountId = row["glacc_id"].ToString();
                            Itm_list.GLAccount = row["GLName_CS"].ToString();
                            Itm_list.Amount = row["amount"].ToString();
                            Itm_list.Instrument_Type = row["ins_typeName"].ToString();
                            Itm_list.InsType_Id = row["ins_type"].ToString();

                            Itm_list.Instrument_No = row["ins_no"].ToString();
                            Itm_list.Instrument_Dt = row["ins_dt"].ToString();
                            Itm_list.ChkPadv = row["chk_padv"].ToString();
                            Item_List.Add(Itm_list);
                        }
                        _PaymentAdviceModel.VouItm_List = Item_List;
                    }

                    _PaymentAdviceModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                    _PaymentAdviceModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                    if (doc_status != "D" && doc_status != "F")
                    {
                        ViewBag.AppLevel = ds.Tables[4];
                    }
                    if (ViewBag.AppLevel != null && _PaymentAdviceModel.Command != "Edit")
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
                                _PaymentAdviceModel.BtnName = "Refresh";
                            }
                            else
                            {
                                if (nextLevel == "0")
                                {
                                    if (create_id == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                    }
                                    _PaymentAdviceModel.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    ViewBag.Approve = "N";
                                    ViewBag.ForwardEnbl = "Y";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    _PaymentAdviceModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                _PaymentAdviceModel.BtnName = "BtnToDetailPage";
                            }
                            if (nextLevel == "0")
                            {
                                if (sent_to == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    _PaymentAdviceModel.BtnName = "BtnToDetailPage";
                                }
                            }
                        }
                        if (doc_status == "F")
                        {
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                _PaymentAdviceModel.BtnName = "BtnToDetailPage";
                            }
                            if (nextLevel == "0")
                            {
                                if (sent_to == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                }
                                _PaymentAdviceModel.BtnName = "BtnToDetailPage";
                            }
                        }
                        if (doc_status == "A")
                        {
                            if (create_id == UserID || approval_id == UserID)
                            {
                                //_PaymentAdviceModel.BtnName = "BtnToDetailPage";
                                _PaymentAdviceModel.BtnName = "Refresh";
                            }
                            else
                            {
                                _PaymentAdviceModel.BtnName = "Refresh";
                            }
                        }
                    }
                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }

                }



                _PaymentAdviceModel.Title = title;
                _PaymentAdviceModel.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;

                return View("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/PaymentAdvice/PaymentAdviceDetail.cshtml", _PaymentAdviceModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /*----------------------------Start Header Section---------------------------------------*/
        public ActionResult GetAutoCompleteBankDetail(PaymentAdviceModel _PaymentAdviceModel)
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
                if (string.IsNullOrEmpty(_PaymentAdviceModel.BankName))
                {
                    Acc_Name = "0";
                }
                else
                {
                    Acc_Name = _PaymentAdviceModel.BankName;
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                // Br_ID = Session["BranchId"].ToString();
                BankAccList = _PaymentAdvice_ISERVICES.AutoGetBankAccList(Comp_ID, Acc_Name, Br_ID);

                List<BankAccName> _BankAccNameList = new List<BankAccName>();
                foreach (var dr in BankAccList)
                {
                    BankAccName _BankAccName = new BankAccName();
                    _BankAccName.bank_acc_id = dr.Key;
                    _BankAccName.bank_acc_name = dr.Value;
                    _BankAccNameList.Add(_BankAccName);
                }
                _PaymentAdviceModel.BankAccNameList = _BankAccNameList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(BankAccList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        public DataSet GetFindates()
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

                DataSet Result = _PaymentAdvice_ISERVICES.GetFinYearDates(CompID, BrchID);
                return Result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult SearchPAItemDetail(string FromDate, string ToDate, string InsType, string BankAcc_id)
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
                var searchValue = "0";
                PaymentAdviceModel _PaymentAdviceModel = new PaymentAdviceModel();
                _PaymentAdviceModel.DocumentStatus = "D";
                //_PaymentAdviceModel.Command = "Edit";
                //_PaymentAdviceModel.TransType = "Update";

                List<VouItemList> _VouItm_List = new List<VouItemList>();
                DataSet dtdata = _PaymentAdvice_ISERVICES.SearchPayAdvItemDetails(CompID, BrchID, FromDate, ToDate, InsType, BankAcc_id);

                _PaymentAdviceModel.VouSearch = "Vou_Search";
                if (dtdata.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dtdata.Tables[0].Rows)
                    {
                        VouItemList vou_list = new VouItemList();
                        vou_list.Vou_No = row["vou_no"].ToString();
                        vou_list.Vou_Dt = row["vou_dt"].ToString();
                        vou_list.Vou_Date = row["vou_date"].ToString();
                        vou_list.BankAccountId = row["acc_id"].ToString();
                        vou_list.BankAccount = row["acc_name"].ToString();
                        vou_list.GLAccount = row["GLName_CS"].ToString();
                        vou_list.GLAccountId = row["GLNameID_CS"].ToString();
                        vou_list.Amount = row["cr_amt_bs"].ToString();
                        vou_list.InsType_Id = row["ins_type"].ToString();
                        vou_list.Instrument_Type = row["ins_typeName"].ToString();
                        vou_list.Instrument_No = row["ins_no"].ToString();
                        vou_list.Instrument_Dt = row["ins_dt"].ToString();
                        _VouItm_List.Add(vou_list);
                    }
                    _PaymentAdviceModel.VouItm_List = _VouItm_List;
                }
                //_PaymentAdviceModel.EntityType = entity_type;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPaymentAdviceItemDetail.cshtml", _PaymentAdviceModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        /*----------------------------End Header Section---------------------------------------*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionCommandPayAdvDetail(PaymentAdviceModel _PAModel, string Command)
        {
            try
            {
                var commCont = new CommonController(_Common_IServices);
                UrlData urlData = new UrlData();
                if (_PAModel.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                
                
                switch (Command)
                {
                    case "AddNew":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_PAModel.AdviceNo))
                            {
                                SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", "Financial Year not Exist", _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
                                return RedirectToAction("PaymentAdviceDetail", _PAModel);
                            }
                            else
                            {
                                SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, null, _PAModel.ListFilterData1);
                                return RedirectToAction("PaymentAdviceDetail", _PAModel);
                            }
                        }
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, null, _PAModel.ListFilterData1);
                        return RedirectToAction("PaymentAdviceDetail", urlData);
                    
                    case "Edit":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        string invdt = _PAModel.AdviceDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
                            return RedirectToAction("PaymentAdviceDetail", urlData);
                        }
                        //if (_PAModel.SSIStatus == "A")
                        //{
                        //    string checkforCancle = CheckDependencyIBS(_PAModel.AdviceNo, _PAModel.AdviceDate);
                        //    if (checkforCancle != "")
                        //    {
                        //        SetUrlData(urlData, "Refresh", "Update", "BtnToDetailPage", checkforCancle, _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
                        //    }
                        //    else
                        //    {
                        //        SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
                        //    }
                        //}
                        else
                        {
                            SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
                        }
                        return RedirectToAction("PaymentAdviceDetail", urlData);
                    
                    case "Save":

                        SavePaymentAdvice(_PAModel);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _PAModel.Message, _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
                        return RedirectToAction("PaymentAdviceDetail", urlData);
                    
                    case "Approve":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        string Invdt1 = _PAModel.AdviceDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, Invdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", _PAModel.Message, _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
                            return RedirectToAction("PaymentAdviceDetail", urlData);
                        }
                        ApprovePADetails(_PAModel);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _PAModel.Message, _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
                        return RedirectToAction("PaymentAdviceDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _PAModel.ListFilterData1);
                        return RedirectToAction("PaymentAdviceDetail", urlData);
                    case "Delete":
                        DeletePADetail(_PAModel);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _PAModel.Message, null, null, _PAModel.ListFilterData1);
                        return RedirectToAction("PaymentAdviceDetail", urlData);
                    case "BacktoList":
                        PaymentAdviceListModel _PayAdvListModel = new PaymentAdviceListModel();
                        _PayAdvListModel.WF_Status = _PAModel.WF_Status1;
                        TempData["ListFilterData"] = _PAModel.ListFilterData1;
                        SetUrlData(urlData, "", "", "", null, null, null, _PAModel.ListFilterData1);
                        return RedirectToAction("PaymentAdvice", _PayAdvListModel);
                    case "Print":
                        return GenratePdfFile(_PAModel);
                    
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("PaymentAdviceDetail", urlData);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string AdviceNo = null, string AdviceDate = null, string ListFilterData1 = null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Adv_No = AdviceNo;
                urlData.Adv_Dt = AdviceDate;
                urlData.ListFilterData1 = ListFilterData1;
                TempData["UrlData"] = urlData;
                TempData["Message"] = Message;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        
        public ActionResult SavePaymentAdvice(PaymentAdviceModel _PaymentAdviceModel)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {
                if (_PaymentAdviceModel.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }
                    BrchID = Session["BranchId"].ToString();

                    DataTable PayAdviceHeaderDetail = new DataTable();
                    DataTable PayAdviceItemDetail = new DataTable();

                    DataTable dtheader = new DataTable();

                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("user_id", typeof(int));
                    dtheader.Columns.Add("padv_no", typeof(string));
                    dtheader.Columns.Add("padv_dt", typeof(string));
                    dtheader.Columns.Add("from_dt", typeof(string));
                    dtheader.Columns.Add("to_dt", typeof(string));
                    dtheader.Columns.Add("ins_type", typeof(string));
                    dtheader.Columns.Add("bankacc_id", typeof(string));
                    dtheader.Columns.Add("padv_status", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));


                    DataRow dtrowHeader = dtheader.NewRow();
                    // dtrowHeader["TransType"] = Session["TransType"].ToString();
                    if (_PaymentAdviceModel.AdviceNo != null)
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
                    dtrowHeader["padv_no"] = _PaymentAdviceModel.AdviceNo;
                    dtrowHeader["padv_dt"] = _PaymentAdviceModel.AdviceDate;
                    dtrowHeader["from_dt"] = _PaymentAdviceModel.FromDate_Dt;
                    dtrowHeader["to_dt"] = _PaymentAdviceModel.ToDate_Dt;
                    dtrowHeader["ins_type"] = _PaymentAdviceModel.InstrumentType;
                    dtrowHeader["bankacc_id"] = _PaymentAdviceModel.bank_acc_id;
                    dtrowHeader["padv_status"] = "D";
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrowHeader["mac_id"] = mac_id;


                    dtheader.Rows.Add(dtrowHeader);
                    PayAdviceHeaderDetail = dtheader;

                    DataTable dtAccount = new DataTable();

                    //dtAccount.Columns.Add("adv_no", typeof(string));
                    //dtAccount.Columns.Add("adv_dt", typeof(string));
                    dtAccount.Columns.Add("vou_no", typeof(string));
                    dtAccount.Columns.Add("vou_dt", typeof(string));
                    dtAccount.Columns.Add("bankacc_id", typeof(int));
                    dtAccount.Columns.Add("glacc_id", typeof(string));
                    dtAccount.Columns.Add("amount", typeof(string));
                    dtAccount.Columns.Add("ins_type", typeof(string));
                    dtAccount.Columns.Add("ins_no", typeof(string));
                    dtAccount.Columns.Add("ins_dt", typeof(string));
                    dtAccount.Columns.Add("chk_padv", typeof(string));

                    JArray jObject = JArray.Parse(_PaymentAdviceModel.PayAdvItemDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtAccount.NewRow();
                        
                        //dtrowLines["adv_no"] = jObject[i]["adv_no"].ToString();
                        //dtrowLines["adv_dt"] = jObject[i]["adv_dt"].ToString();
                        dtrowLines["vou_no"] = jObject[i]["vou_no"].ToString();
                        dtrowLines["vou_dt"] = jObject[i]["vou_dt"].ToString();
                        dtrowLines["bankacc_id"] = jObject[i]["bankacc_id"].ToString();
                        dtrowLines["glacc_id"] = jObject[i]["glacc_id"].ToString();
                        dtrowLines["amount"] = jObject[i]["amt"].ToString();
                        dtrowLines["ins_type"] = jObject[i]["instype"].ToString();
                        dtrowLines["ins_no"] = jObject[i]["ins_no"].ToString();
                        dtrowLines["ins_dt"] = jObject[i]["ins_dt"].ToString();
                        dtrowLines["chk_padv"] = jObject[i]["chkpadv"].ToString();

                        dtAccount.Rows.Add(dtrowLines);
                    }
                    PayAdviceItemDetail = dtAccount;

                    SaveMessage = _PaymentAdvice_ISERVICES.InsertPayAdvDetail(PayAdviceHeaderDetail, PayAdviceItemDetail);

                    string AdviceNo = SaveMessage.Split(',')[1].Trim();
                    string BP_Number = AdviceNo.Replace("/", "");
                    string Message = SaveMessage.Split(',')[0].Trim();
                    string AdviceDt = SaveMessage.Split(',')[2].Trim();
                    if (Message == "Data_Not_Found")
                    {
                        //var a = SaveMessage.Split(',');
                        var msg = Message.Replace("_", " ") + " " + AdviceNo + " in " + PageName;//InvoiceAdjustmentNo is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _PaymentAdviceModel.Message = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("PaymentAdviceDetail");
                    }

                    if (Message == "Update" || Message == "Save")
                        //    Session["Message"] = "Save";
                        //Session["Command"] = "Update";
                        //Session["InvoiceAdjustmentNo"] = InvoiceAdjustmentNo;
                        //Session["InvoiceAdjustmentDate"] = InvoiceAdjustmentDate;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnToDetailPage";
                        _PaymentAdviceModel.Message = "Save";
                    _PaymentAdviceModel.AdviceNo = AdviceNo;
                    _PaymentAdviceModel.AdviceDate = AdviceDt;
                    _PaymentAdviceModel.TransType = "Update";
                    _PaymentAdviceModel.BtnName = "BtnToDetailPage";
                    return RedirectToAction("PaymentAdviceDetail");
                }
                return RedirectToAction("PaymentAdviceDetail");
                //else
                //{
                //    if (Session["compid"] != null)
                //    {
                //        CompID = Session["compid"].ToString();
                //    }
                //    if (Session["userid"] != null)
                //    {
                //        UserID = Session["userid"].ToString();
                //    }
                //    _PaymentAdviceModel.Create_by = UserID;
                //    string br_id = Session["BranchId"].ToString();
                //    string mac = Session["UserMacaddress"].ToString();
                //    string system = Session["UserSystemName"].ToString();
                //    string ip = Session["UserIP"].ToString();
                //    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                //    DataSet SaveMessage1 = _PaymentAdvice_ISERVICES.InvoiceAdjustmentCancel(_PaymentAdviceModel, CompID, br_id, mac_id);

                //    string fileName = "IA_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                //    var filePath = SavePdfDocToSendOnEmailAlert(_PaymentAdviceModel.Vou_No, _PaymentAdviceModel.Vou_Date, fileName);
                //    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _PaymentAdviceModel.Vou_No, "C", UserID, "", filePath);


                //    _PaymentAdviceModel.Message = "Cancelled";

                //    _PaymentAdviceModel.InvoiceAdjustmentNo = _PaymentAdviceModel.Vou_No;
                //    _PaymentAdviceModel.InvoiceAdjustmentDate = _PaymentAdviceModel.Vou_Date;
                //    _PaymentAdviceModel.TransType = "Update";
                //    _PaymentAdviceModel.BtnName = "Refresh";
                //    return RedirectToAction("InvoiceAdjustmentDetail");
                //}
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                // return View("~/Views/Shared/Error.cshtml");
            }

        }
        public DataSet GetPayAdvDtlOnView(string PAdv_No, string PAdv_Date)
        {
            try
            {
                //JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string Br_ID = string.Empty;
                //string Voutype = "SV";
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataSet result = _PaymentAdvice_ISERVICES.GetPayAdvDetailOnView(Comp_ID, Br_ID,PAdv_No, PAdv_Date, /*FromDate,ToDate,InsType,BankAccId, */UserID, DocumentMenuId);

                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                throw Ex;
            }
        }

        /*----------------------------Start List Page Data---------------------------------------*/
        private List<PaymentAdviceList> GetPayAdvListAll(PaymentAdviceListModel _PayAdvListModel)
        {
            try
            {
                _PaymentAdvList = new List<PaymentAdviceList>();
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
                if (_PayAdvListModel.WF_Status != null)
                {
                    wfstatus = _PayAdvListModel.WF_Status;
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
                Dtdata = _PaymentAdvice_ISERVICES.GetPayAdvListDetail(CompID, BrchID, _PayAdvListModel.VouFromDate, _PayAdvListModel.VouToDate, _PayAdvListModel.Status, wfstatus, UserID, DocumentMenuId);
                if (Dtdata.Tables[1].Rows.Count > 0)
                {
                    _PayAdvListModel.FromDate = Dtdata.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (Dtdata.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in Dtdata.Tables[0].Rows)
                    {
                        PaymentAdviceList _AdvList = new PaymentAdviceList();
                        _AdvList.PANumber = dr["padv_no"].ToString();
                        _AdvList.PADate = dr["padv_dt"].ToString();
                        _AdvList.hdPADate = dr["padv_date"].ToString();
                        _AdvList.PAStatus = dr["PA_Status"].ToString();
                        _AdvList.create_by = dr["create_by"].ToString();
                        _AdvList.CreatedON = dr["created_on"].ToString();
                        _AdvList.app_by = dr["app_by"].ToString();
                        _AdvList.ApprovedOn = dr["app_dt"].ToString();


                        _PaymentAdvList.Add(_AdvList);
                    }
                }
                return _PaymentAdvList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchPayAdvListDetail(string CompID, string Br_ID, string Fromdate, string Todate, string Status)
        {
            _PaymentAdvList = new List<PaymentAdviceList>();
            PaymentAdviceListModel _ListModel = new PaymentAdviceListModel();
            //Session["WF_status"] = "";
            _ListModel.WF_Status = null;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            DataSet dt = new DataSet();
            dt = _PaymentAdvice_ISERVICES.GetPayAdvListDetail(CompID, Br_ID, Fromdate, Todate, Status, "", "", "");
            // Session["VouSearch"] = "Vou_Search";
            _ListModel.PASearch = "PA_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    PaymentAdviceList _PAdvList = new PaymentAdviceList();
                    _PAdvList.PANumber = dr["padv_no"].ToString();
                    _PAdvList.PADate = dr["padv_dt"].ToString();
                    _PAdvList.hdPADate = dr["padv_date"].ToString();
                    _PAdvList.PAStatus = dr["PA_Status"].ToString();
                    _PAdvList.create_by = dr["create_by"].ToString();
                    _PAdvList.CreatedON = dr["created_on"].ToString();
                    _PAdvList.app_by = dr["app_by"].ToString();
                    _PAdvList.ApprovedOn = dr["app_dt"].ToString();
                    _PaymentAdvList.Add(_PAdvList);
                }
            }
            _ListModel.PAList = _PaymentAdvList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPaymentAdviceList.cshtml", _ListModel);
        }
        //public ActionResult EditPA_ByList(string PAdvNo, string PAdvdt, string ListFilterData, string WF_Status)
        //{/*start Add by Hina on 28-02-2024 to chk Financial year exist or not*/
        //    PaymentAdviceModel dblclick = new PaymentAdviceModel();
        //    if (Session["CompId"] != null)
        //        CompID = Session["CompId"].ToString();
        //    if (Session["BranchId"] != null)
        //        BrchID = Session["BranchId"].ToString();
        //    var commCont = new CommonController(_Common_IServices);
        //    string DblClkMsg = string.Empty;
        //    DblClkMsg = commCont.Fin_CheckFinancialYear(CompID, BrchID, PAdvdt);
        //    if (DblClkMsg == "FY Not Exist")
        //    {
        //        TempData["Message"] = "Financial Year not Exist";
        //        dblclick.BtnName = "Refresh";
        //    }
        //    else if (DblClkMsg == "FB Close")
        //    {
        //        TempData["FBMessage"] = "Financial Book Closing";
        //        dblclick.BtnName = "Refresh";
        //    }
        //    else
        //    {
        //        dblclick.BtnName = "BtnToDetailPage";
        //    }
        //    /*End to chk Financial year exist or not*/
        //    //Session["Message"] = "New";
        //    //Session["Command"] = "Update";
        //    //Session["InvoiceAdjustmentNo"] = VouNo;
        //    //Session["InvoiceAdjustmentDate"] = Voudt;
        //    //Session["TransType"] = "Update";
        //    //Session["AppStatus"] = 'D';
        //    //Session["BtnName"] = "BtnToDetailPage";
        //    //InvoiceAdjustmentModel dblclick = new InvoiceAdjustmentModel();
        //    UrlData _url = new UrlData();
        //    dblclick.AdviceNo = PAdvNo;
        //    dblclick.AdviceDate = PAdvdt;
        //    dblclick.TransType = "Update";
        //    //dblclick.BtnName = "BtnToDetailPage";
        //    if (WF_Status != null && WF_Status != "")
        //    {
        //        _url.wf = WF_Status;
        //        dblclick.WF_Status1 = WF_Status;
        //    }
        //    TempData["ModelData"] = dblclick;
        //    //_url.Cmd = "Update";
        //    _url.TransType = "Update";
        //    //_url.BtnName = "D";
        //    _url.Adv_No = PAdvNo;
        //    _url.Adv_Dt = PAdvdt;
        //    TempData["ListFilterData"] = ListFilterData;
        //    return RedirectToAction("PaymentAdviceDetail", "PaymentAdvice", _url);
        //}
        public ActionResult EditPA_ByList(string PAdvNo, string PAdvdt, string ListFilterData,string WF_Status)
        {
            try
            {
                UrlData urlData = new UrlData();
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (PAdvNo == null)
                {
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {

                        TempData["Message"] = "Financial Year not Exist";
                        SetUrlData(urlData, "", "", "", "Financial Year not Exist", PAdvNo, PAdvdt, ListFilterData);
                        return RedirectToAction("PaymentAdvice", "PaymentAdvice", urlData);
                    }
                }
                urlData.wf = WF_Status;
                /*End to chk Financial year exist or not*/
                string BtnName = PAdvNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = PAdvNo == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, PAdvNo, PAdvdt, ListFilterData);
                return RedirectToAction("PaymentAdviceDetail", "PaymentAdvice", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        /*----------------------------End List Page Data---------------------------------------*/
        private ActionResult DeletePADetail(PaymentAdviceModel _PaymentAdviceModel)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string PAdvNo = _PaymentAdviceModel.AdviceNo;
                //string AdjNumber = InvAdjNo.Replace("/", "");

                string Message = _PaymentAdvice_ISERVICES.PADelete(_PaymentAdviceModel, CompID, br_id, DocumentMenuId);
                _PaymentAdviceModel.Message = Message.Split(',')[1];

                return RedirectToAction("PaymentAdviceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ApprovePADetails(PaymentAdviceModel _PaymentAdviceModel)
        {
            try
            {
                string MenuDocId = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["UserId"] != null)
                    UserID = Session["UserId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                if (DocumentMenuId != null)
                    MenuDocId = DocumentMenuId;
                var DocNo = _PaymentAdviceModel.AdviceNo;
                var DocDate = _PaymentAdviceModel.AdviceDate;
                var A_Status = _PaymentAdviceModel.A_Status;
                var A_Level = _PaymentAdviceModel.A_Level;
                var A_Remarks = _PaymentAdviceModel.A_Remarks;
               
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                

                string Message = _PaymentAdvice_ISERVICES.ApprovePADetail(DocNo, DocDate, MenuDocId, BrchID, CompID, UserID, mac_id
                    , A_Status, A_Level, A_Remarks);
                
                    string[] FDetail = Message.Split(',');
                    string INV_NO = string.Empty;
                    string INV_DT = string.Empty;
                    string ApMessage = string.Empty;
                
                    INV_NO = FDetail[0].ToString();
                    INV_DT = FDetail[4].ToString();
                    ApMessage = FDetail[3].ToString().Trim();

                if (ApMessage == "A")
                {
                    _PaymentAdviceModel.Message = "Approved";
                    _PaymentAdviceModel.BtnName = "BtnEdit";
                    _PaymentAdviceModel.Command = "Approve";
                }
                _PaymentAdviceModel.TransType = "Update";
                _PaymentAdviceModel.AdviceNo = INV_NO;
                _PaymentAdviceModel.AdviceDate = INV_DT;
                _PaymentAdviceModel.AppStatus = "D";
                return RedirectToAction("PaymentAdviceDetail", _PaymentAdviceModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1)
        {
            //JArray jObjectBatch = JArray.Parse(list);
            PaymentAdviceModel _PAModel = new PaymentAdviceModel();
            UrlData urlData = new UrlData();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _PAModel.AdviceNo = jObjectBatch[i]["DocNo"].ToString();
                    _PAModel.AdviceDate = jObjectBatch[i]["DocDate"].ToString();
                    _PAModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _PAModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _PAModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                    
                }
            }
            if (_PAModel.A_Status != "Approve" || _PAModel.A_Status == "" || _PAModel.A_Status == null)
            {
                _PAModel.A_Status = "Approve";
            }
            _PAModel.ListFilterData1 = ListFilterData1;

            ApprovePADetails(_PAModel);
            TempData["ModelData"] = _PAModel;
            TempData["WF_status"] = WF_status1;
            SetUrlData(urlData, _PAModel.Command, _PAModel.TransType, _PAModel.BtnName, _PAModel.Message, _PAModel.AdviceNo, _PAModel.AdviceDate, _PAModel.ListFilterData1);
            return RedirectToAction("PaymentAdviceDetail");
        }
        public ActionResult GetPAFromDashboardList(string docid, string status)
        {

            //Session["WF_status"] = status;
            PaymentAdviceListModel DashBord = new PaymentAdviceListModel();
            DashBord.WF_Status = status;
            return RedirectToAction("PaymentAdvice", DashBord);
        }
        /*--------Print---------*/

        public FileResult GenratePdfFile(PaymentAdviceModel _Model)
        {
            return File(GetPdfData(_Model.AdviceNo, _Model.AdviceDate, _Model.Status), "application/pdf", "PaymentAdvice.pdf");
        }
        public byte[] GetPdfData(string PAdvNo, string PAdvDate, string Status)
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
            
                DataSet Deatils = _PaymentAdvice_ISERVICES.GetPAPrintDeatils(CompID, BrchID, PAdvNo, PAdvDate, Status);
                ViewBag.PageName = "PA";
                ViewBag.Title = "Payment Advice";
                ViewBag.Details = Deatils;
                
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();

                /* Added by Suraj Maurya on 17-02-2025 to add logo*/
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                /* Added by Suraj Maurya on 17-02-2025 to add logo End */

                 string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/FinancialAccountingAndControl/PaymentAdvice/PaymentAdvicePrint.cshtml"));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
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
                    BrchID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
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
    }

}