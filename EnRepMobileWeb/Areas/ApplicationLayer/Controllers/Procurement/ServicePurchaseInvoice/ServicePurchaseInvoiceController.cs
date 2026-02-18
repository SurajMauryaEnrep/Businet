using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.ServicePurchaseInvoice;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.ServicePurchaseInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using EnRepMobileWeb.Resources;
//using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.DomesticPurchaseInvoice;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.ServicePurchaseInvoice
{
    public class ServicePurchaseInvoiceController : Controller
    {
        List<ServicePurchaseInvoiceList> _SPIList;
        string CompID, language, title, Inv_No, BrchID, UserID, create_id = string.Empty;
        string DocumentMenuId = "105101150";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ServicePI_ISERVICE _ServicePI_ISERVICE;

        public ServicePurchaseInvoiceController(Common_IServices _Common_IServices, ServicePI_ISERVICE _ServicePI_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._ServicePI_ISERVICE = _ServicePI_ISERVICE;
        }
        // GET: ApplicationLayer/ServicePurchaseInvoice
        public ActionResult ServicePurchaseInvoice(ServicePIListModel _SPIListModel,string WF_Status)
        {
            try
            {
                CommonPageDetails();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //ServicePIListModel _SPIListModel = new ServicePIListModel();
                //if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                //{
                //    _SPIListModel.WF_status = TempData["WF_status"].ToString();
                //}
                //else
                //{
                //    _SPIListModel.wfstatus = "";
                //}              
                if (DocumentMenuId != null)
                {
                    _SPIListModel.wfdocid = DocumentMenuId;
                }
                else
                {
                    _SPIListModel.wfdocid = "0";
                }
                _SPIListModel.WF_status = WF_Status;
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string todate = range.ToDate;

                GetAutoCompleteSearchSuppList(_SPIListModel);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();

                    var a = PRData.Split(',');
                    _SPIListModel.SuppID = a[0].Trim();
                    _SPIListModel.SPI_FromDate = a[1].Trim();
                    _SPIListModel.SPI_ToDate = a[2].Trim();
                    _SPIListModel.Status = a[3].Trim();
                    if (_SPIListModel.Status == "0")
                    {
                        _SPIListModel.Status = null;

                    }

                    _SPIListModel.FromDate = _SPIListModel.SPI_FromDate;
                    _SPIListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    _SPIListModel.SPIList = GetServicePIList(_SPIListModel);
                }
                else
                {
                    _SPIListModel.SPI_FromDate = startDate;
                    _SPIListModel.SPI_ToDate = todate;
                    _SPIListModel.FromDate = startDate;
                    _SPIListModel.SPIList = GetServicePIList(_SPIListModel);
                    
                }
                //var other = new CommonController(_Common_IServices);                
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _SPIListModel.StatusList = statusLists;

                _SPIListModel.Title = title;
                _SPIListModel.SPISearch = "0";
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseInvoice/ServicePurchaseInvoiceList.cshtml", _SPIListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AddServicePurchaseInvoiceDetail()
        {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ServicePurchaseInvoice");
            }
            /*End to chk Financial year exist or not*/
            ServicePIModel _ServicePIModel = new ServicePIModel();
            _ServicePIModel.Message = "New";
            _ServicePIModel.Command = "Add";
            _ServicePIModel.AppStatus = "D";
            _ServicePIModel.TransType = "Save";
            _ServicePIModel.BtnName = "BtnAddNew";
            _ServicePIModel.DocumentStatus = "D";
            TempData["ModelData"] = _ServicePIModel;
            TempData["ListFilterData"] = null;
            ViewBag.DocumentStatus = "D";
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("ServicePurchaseInvoiceDetail", "ServicePurchaseInvoice");
        }
        public ActionResult ServicePurchaseInvoiceDetail(URLModelDetails URLModel)
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
            /*Add by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.Sinv_dt) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                var _ServicePIModel = TempData["ModelData"] as ServicePIModel;
                if (_ServicePIModel != null)
                {
                    //ServicePIModel _ServicePIModel = new ServicePIModel();
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    //Session["SPISearch"] = null;
                    _ServicePIModel.SPISearch = null;

                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.RateDigit = RateDigit;
                    ViewBag.QtyDigit = QtyDigit;

                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _ServicePIModel.SupplierNameList = suppLists;

                    List<SourceDoc> _DocumentNumberList = new List<SourceDoc>();
                    SourceDoc _DocumentNumber = new SourceDoc();
                    _DocumentNumber.doc_no = "---Select---";
                    _DocumentNumber.doc_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _ServicePIModel.SourceDocList = _DocumentNumberList;

                    List<CurrancyList> currancyLists = new List<CurrancyList>();
                    currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    _ServicePIModel.currancyLists = currancyLists;
                    _ServicePIModel.Title = title;
                    _ServicePIModel.UserID = UserID;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ServicePIModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _ServicePIModel.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["Inv_No"] != null && Session["Inv_Date"] != null)
                    if (_ServicePIModel.Sinv_no != null && _ServicePIModel.Sinv_dt != null)
                    {
                        string Doc_no = _ServicePIModel.Sinv_no;
                        string Doc_date = _ServicePIModel.Sinv_dt;
                        //string Doc_no = Session["Inv_No"].ToString();
                        //string Doc_date = Session["Inv_Date"].ToString();

                        DataSet ds = GetServicePurchaseInvoiceEdit(Doc_no, Doc_date);


                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            ViewBag.AttechmentDetails = ds.Tables[8];
                            ViewBag.GLAccount = ds.Tables[7];
                            ViewBag.GLTOtal = ds.Tables[9];
                            //ViewBag.TaxDetail = ds.Tables[2];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            //ViewBag.OCDetail = ds.Tables[3];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.TotalDetails = ds.Tables[0];
                            //ViewBag.TaxDetail2 = ds.Tables[10];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OCTaxDetails = ds.Tables[11];
                            ViewBag.CostCenterData = ds.Tables[12];
                            ViewBag.ItemTDSDetails = ds.Tables[13];
                            ViewBag.ItemOC_TDSDetails = ds.Tables[14];
                            _ServicePIModel.Sinv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _ServicePIModel.Sinv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                            _ServicePIModel.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _ServicePIModel.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                            _ServicePIModel.Currency = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _ServicePIModel.curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                            _ServicePIModel.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _ServicePIModel.SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();
                            _ServicePIModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _ServicePIModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _ServicePIModel.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _ServicePIModel.SuppID, supp_name = _ServicePIModel.SuppName });
                            _ServicePIModel.SupplierNameList = suppLists;

                            _ServicePIModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _ServicePIModel.Create_by = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _ServicePIModel.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _ServicePIModel.Approved_by = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _ServicePIModel.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _ServicePIModel.Amended_by = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _ServicePIModel.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _ServicePIModel.StatusName = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                            _ServicePIModel.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            _ServicePIModel.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _ServicePIModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _ServicePIModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _ServicePIModel.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _ServicePIModel.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _ServicePIModel.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _ServicePIModel.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _ServicePIModel.SPIStatus = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            _ServicePIModel.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            _ServicePIModel.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            _ServicePIModel.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                            _ServicePIModel.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                            _ServicePIModel.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                            //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            var state_code = ds.Tables[0].Rows[0]["state_code"];
                            var br_state_code = ds.Tables[0].Rows[0]["br_state_code"];
                            //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            if (state_code.ToString() == br_state_code.ToString())
                            {
                                _ServicePIModel.Hd_GstType = "Both";
                            }
                            else
                            {
                                _ServicePIModel.Hd_GstType = "IGST";
                            }
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            ViewBag.Approve_id = approval_id;
                            string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                            string doc_status = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                            _ServicePIModel.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                            if (roundoff_status == "Y")
                            {
                                _ServicePIModel.RoundOffFlag = true;
                            }
                            else
                            {
                                _ServicePIModel.RoundOffFlag = false;
                            }
                            _ServicePIModel.DocumentStatus = doc_status;
                            string RCMApplicable = ds.Tables[0].Rows[0]["rcm_app"].ToString().Trim();
                            if (RCMApplicable == "Y")
                            {
                                _ServicePIModel.RCMApplicable = true;
                            }
                            else
                            {
                                _ServicePIModel.RCMApplicable = false;
                            }
                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                if (doc_status == "A" || doc_status == "C")
                                {
                                    _ServicePIModel.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                                }
                                _ServicePIModel.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                                _ServicePIModel.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                                ViewBag.GLVoucherNo = _ServicePIModel.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                            }
                            ViewBag.DocumentStatus = doc_status;
                            //Session["DocumentStatus"] = doc_status;
                            _ServicePIModel.DocumentStatus = doc_status;
                            if (doc_status == "C")
                            {
                                _ServicePIModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                _ServicePIModel.Cancelled = true;
                                //Session["BtnName"] = "Refresh";
                                _ServicePIModel.BtnName = "Refresh";
                            }
                            else
                            {
                                _ServicePIModel.Cancelled = false;
                            }
                            _ServicePIModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _ServicePIModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                            _ServicePIModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _DocumentNumberList.Add(new SourceDoc { doc_no = _ServicePIModel.SrcDocNo, doc_dt = _ServicePIModel.SrcDocNo });
                            _ServicePIModel.SourceDocList = _DocumentNumberList;
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _ServicePIModel.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            if (doc_status != "D" && doc_status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[6];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _ServicePIModel.Command != "Edit")
                            {
                                var sent_to = "";
                                var nextLevel = "";
                                if (ds.Tables[4].Rows.Count > 0)
                                {
                                    sent_to = ds.Tables[4].Rows[0]["sent_to"].ToString();
                                }
                                if (ds.Tables[5].Rows.Count > 0)
                                {
                                    nextLevel = ds.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                                }
                                if (doc_status == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "Refresh";
                                        _ServicePIModel.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServicePIModel.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServicePIModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServicePIModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServicePIModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (doc_status == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServicePIModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServicePIModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (doc_status == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServicePIModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "Refresh";
                                        _ServicePIModel.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            //ViewBag.MenuPageName = getDocumentName();
                            _ServicePIModel.Title = title;
                            _ServicePIModel.DocumentMenuId = DocumentMenuId;

                            ViewBag.ItemDetails = ds.Tables[1];
                            //ViewBag.VBRoleList = GetRoleList();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                        }
                    }
                    else
                    {
                        _ServicePIModel.SourceType = "D";
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseInvoice/ServicePurchaseInvoiceDetail.cshtml", _ServicePIModel);
                }
                else
                {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    ServicePIModel _ServicePIModel1 = new ServicePIModel();
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    //Session["SPISearch"] = null;
                    _ServicePIModel1.SPISearch = null;
                    _ServicePIModel1.UserID = UserID;
                    if (URLModel.Sinv_no != null || URLModel.Sinv_dt != null)
                    {
                        _ServicePIModel1.Sinv_no = URLModel.Sinv_no;
                        _ServicePIModel1.Sinv_dt = URLModel.Sinv_dt;
                    }
                    if (URLModel.TransType != null)
                    {
                        _ServicePIModel1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _ServicePIModel1.TransType = "New";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _ServicePIModel1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _ServicePIModel1.BtnName = "BtnRefresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _ServicePIModel1.Command = URLModel.Command;
                    }
                    else
                    {
                        _ServicePIModel1.Command = "Refresh";
                    }
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.RateDigit = RateDigit;
                    ViewBag.QtyDigit = QtyDigit;

                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _ServicePIModel1.SupplierNameList = suppLists;

                    List<SourceDoc> _DocumentNumberList = new List<SourceDoc>();
                    SourceDoc _DocumentNumber = new SourceDoc();
                    _DocumentNumber.doc_no = "---Select---";
                    _DocumentNumber.doc_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _ServicePIModel1.SourceDocList = _DocumentNumberList;

                    List<CurrancyList> currancyLists = new List<CurrancyList>();
                    currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    _ServicePIModel1.currancyLists = currancyLists;
                    _ServicePIModel1.Title = title;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ServicePIModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                    {
                        _ServicePIModel1.WF_status1 = TempData["WF_status"].ToString();
                    }
                    //if (Session["Inv_No"] != null && Session["Inv_Date"] != null)
                    if (_ServicePIModel1.Sinv_no != null && _ServicePIModel1.Sinv_dt != null)
                    {
                        string Doc_no = _ServicePIModel1.Sinv_no;
                        string Doc_date = _ServicePIModel1.Sinv_dt;
                        //string Doc_no = Session["Inv_No"].ToString();
                        //string Doc_date = Session["Inv_Date"].ToString();

                        DataSet ds = GetServicePurchaseInvoiceEdit(Doc_no, Doc_date);


                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            ViewBag.AttechmentDetails = ds.Tables[8];
                            ViewBag.GLAccount = ds.Tables[7];
                            ViewBag.GLTOtal = ds.Tables[9];

                            //ViewBag.TaxDetail = ds.Tables[2];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            //ViewBag.OCDetail = ds.Tables[3];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.TotalDetails = ds.Tables[0];
                            //ViewBag.TaxDetail2 = ds.Tables[10];/*commented by suraj on 01-12-2022 for change to common */
                            ViewBag.ItemTaxDetailsList = ds.Tables[10];
                            ViewBag.OCTaxDetails = ds.Tables[11];
                            ViewBag.CostCenterData = ds.Tables[12];
                            ViewBag.ItemTDSDetails = ds.Tables[13];
                            ViewBag.ItemOC_TDSDetails = ds.Tables[14];
                            _ServicePIModel1.Sinv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _ServicePIModel1.Sinv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                            _ServicePIModel1.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _ServicePIModel1.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                            _ServicePIModel1.Currency = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _ServicePIModel1.curr_id = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"].ToString());
                            _ServicePIModel1.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _ServicePIModel1.SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();
                            _ServicePIModel1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _ServicePIModel1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _ServicePIModel1.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _ServicePIModel1.SuppID, supp_name = _ServicePIModel1.SuppName });
                            _ServicePIModel1.SupplierNameList = suppLists;

                            _ServicePIModel1.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _ServicePIModel1.Create_by = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _ServicePIModel1.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _ServicePIModel1.Approved_by = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _ServicePIModel1.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _ServicePIModel1.Amended_by = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _ServicePIModel1.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _ServicePIModel1.StatusName = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                            _ServicePIModel1.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            _ServicePIModel1.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _ServicePIModel1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _ServicePIModel1.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _ServicePIModel1.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _ServicePIModel1.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _ServicePIModel1.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _ServicePIModel1.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _ServicePIModel1.SPIStatus = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            _ServicePIModel1.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            _ServicePIModel1.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            _ServicePIModel1.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                            _ServicePIModel1.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                            _ServicePIModel1.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                            //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            var state_code = ds.Tables[0].Rows[0]["state_code"];
                            var br_state_code = ds.Tables[0].Rows[0]["br_state_code"];
                            //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            if (state_code.ToString() == br_state_code.ToString())
                            {
                                _ServicePIModel1.Hd_GstType = "Both";
                            }
                            else
                            {
                                _ServicePIModel1.Hd_GstType = "IGST";
                            }
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            ViewBag.Approve_id = approval_id;
                            string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                            string doc_status = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                            _ServicePIModel1.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                            if (roundoff_status == "Y")
                            {
                                _ServicePIModel1.RoundOffFlag = true;
                            }
                            else
                            {
                                _ServicePIModel1.RoundOffFlag = false;
                            }
                            _ServicePIModel1.DocumentStatus = doc_status;
                            string RCMApplicable = ds.Tables[0].Rows[0]["rcm_app"].ToString().Trim();
                            if (RCMApplicable == "Y")
                            {
                                _ServicePIModel1.RCMApplicable = true;
                            }
                            else
                            {
                                _ServicePIModel1.RCMApplicable = false;
                            }
                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                if (doc_status == "A" || doc_status == "C")
                                {
                                    _ServicePIModel1.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                                }
                                _ServicePIModel1.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                                _ServicePIModel1.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                                ViewBag.GLVoucherNo = _ServicePIModel1.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                            }
                            ViewBag.DocumentStatus = doc_status;
                            //Session["DocumentStatus"] = doc_status;
                            _ServicePIModel1.DocumentStatus = doc_status;
                            if (doc_status == "C")
                            {
                                _ServicePIModel1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                _ServicePIModel1.Cancelled = true;
                                //Session["BtnName"] = "Refresh";
                                _ServicePIModel1.BtnName = "Refresh";
                            }
                            else
                            {
                                _ServicePIModel1.Cancelled = false;
                            }
                            _ServicePIModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _ServicePIModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                            _ServicePIModel1.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_number"].ToString();
                            _DocumentNumberList.Add(new SourceDoc { doc_no = _ServicePIModel1.SrcDocNo, doc_dt = _ServicePIModel1.SrcDocNo });
                            _ServicePIModel1.SourceDocList = _DocumentNumberList;
                            if (ds.Tables[0].Rows[0]["src_doc_date"] != null && ds.Tables[0].Rows[0]["src_doc_date"].ToString() != "")
                            {
                                _ServicePIModel1.SrcDocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]).ToString("yyyy-MM-dd");
                            }
                            if (doc_status != "D" && doc_status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[6];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _ServicePIModel1.Command != "Edit")
                            {
                                var sent_to = "";
                                var nextLevel = "";
                                if (ds.Tables[4].Rows.Count > 0)
                                {
                                    sent_to = ds.Tables[4].Rows[0]["sent_to"].ToString();
                                }
                                if (ds.Tables[5].Rows.Count > 0)
                                {
                                    nextLevel = ds.Tables[5].Rows[0]["nextlevel"].ToString().Trim();
                                }
                                if (doc_status == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "Refresh";
                                        _ServicePIModel1.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServicePIModel1.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServicePIModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServicePIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ServicePIModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (doc_status == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServicePIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServicePIModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (doc_status == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ServicePIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "Refresh";
                                        _ServicePIModel1.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            //ViewBag.MenuPageName = getDocumentName();
                            _ServicePIModel1.Title = title;
                            _ServicePIModel1.DocumentMenuId = DocumentMenuId;

                            ViewBag.ItemDetails = ds.Tables[1];
                            //ViewBag.VBRoleList = GetRoleList();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                        }
                    }
                    else
                    {
                        _ServicePIModel1.SourceType = "D";
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseInvoice/ServicePurchaseInvoiceDetail.cshtml", _ServicePIModel1);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetInvoiceList(string docid, string status)
        {
            ServicePIListModel _SPIListModel = new ServicePIListModel();
            _SPIListModel.WF_status = status;
            //Session["WF_Docid"] = docid;
            return RedirectToAction("ServicePurchaseInvoice", "ServicePurchaseInvoice", _SPIListModel);
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
        public ActionResult GetAutoCompleteSearchSuppList(ServicePIListModel _ServicePIListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string SuppType = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_ServicePIListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _ServicePIListModel.SuppName;
                }

                SuppList = _ServicePI_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in SuppList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _ServicePIListModel.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DoubleClickOnList(string DocNo, string DocDate, string ListFilterData, string WF_status)
        {
            URLModelDetails URLModel = new URLModelDetails();
            /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            ServicePIModel _ServicePIModel = new ServicePIModel();
            _ServicePIModel.Message = "New";
            _ServicePIModel.Command = "Update";
            _ServicePIModel.TransType = "Update";
            _ServicePIModel.BtnName = "BtnToDetailPage";
            _ServicePIModel.Sinv_no = DocNo;
            _ServicePIModel.Sinv_dt = DocDate;
            TempData["ModelData"] = _ServicePIModel;
            TempData["WF_status"] = WF_status;
            TempData["WF_status1"] = WF_status;
            TempData["ListFilterData"] = ListFilterData;
            URLModel.Sinv_no = DocNo;
            URLModel.Sinv_dt = DocDate;
            URLModel.TransType = "Update";
            URLModel.Command = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["TransType"] = "Update";
            //Session["BtnName"] = "BtnToDetailPage";
            //Session["Inv_No"] = DocNo;
            //Session["Inv_Date"] = DocDate;
            return RedirectToAction("ServicePurchaseInvoiceDetail", "ServicePurchaseInvoice", URLModel);
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            //Session["Message"] = "";
            ServicePIModel _ServicePIModel = new ServicePIModel();
            var a = TrancType.Split(',');
            _ServicePIModel.Sinv_no = a[0].Trim();
            _ServicePIModel.Sinv_dt = a[1].Trim();
            _ServicePIModel.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            _ServicePIModel.Message = Mailerror;
            //_SPODetailModel.WF_status1 = WF_status1;
            _ServicePIModel.BtnName = "BtnToDetailPage";
            URLModelDetails URLModel = new URLModelDetails();
            URLModel.Sinv_no = a[0].Trim();
            URLModel.Sinv_dt = a[01].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _ServicePIModel;
            TempData["WF_status1"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ServicePurchaseInvoiceDetail", "ServicePurchaseInvoice", URLModel);
        }
        [HttpPost]
        public JsonResult GetServiceVerifcationList(string Supp_id)
        {
            try
            {
                JsonResult DataRows = null;
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
                DataSet result = _ServicePI_ISERVICE.GetServiceVerifcationList(Supp_id, Comp_ID, Br_ID);

                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";

                result.Tables[0].Rows.InsertAt(Drow, 0);
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
        [HttpPost]
        public JsonResult GetServiceVerifcationDetails(string VerNo, string VerDate)
        {
            try
            {
                JsonResult DataRows = null;
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
                DataSet result = _ServicePI_ISERVICE.GetServiceVerifcationDetail(VerNo, VerDate, Comp_ID, Br_ID);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionSPIDeatils(ServicePIModel _ServicePIModel, string command)
        {
            try
            {/*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ServicePIModel.Delete == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        ServicePIModel _ServicePIModelAddNew = new ServicePIModel();
                        _ServicePIModelAddNew.AppStatus = "D";
                        _ServicePIModelAddNew.BtnName = "BtnAddNew";
                        _ServicePIModelAddNew.TransType = "Save";
                        _ServicePIModelAddNew.Command = "New";
                        ViewBag.DocumentStatus = "D";
                        TempData["ModelData"] = _ServicePIModelAddNew;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ServicePIModel.Sinv_no))
                                return RedirectToAction("DoubleClickOnList", new { DocNo = _ServicePIModel.Sinv_no, DocDate = _ServicePIModel.Sinv_dt, ListFilterData = _ServicePIModel.ListFilterData1, WF_status = _ServicePIModel.WFStatus });
                            else
                                _ServicePIModelAddNew.Command = "Refresh";
                            _ServicePIModelAddNew.TransType = "Refresh";
                            _ServicePIModelAddNew.BtnName = "Refresh";
                            _ServicePIModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = _ServicePIModelAddNew;
                            return RedirectToAction("ServicePurchaseInvoiceDetail", "ServicePurchaseInvoice", _ServicePIModelAddNew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ServicePurchaseInvoiceDetail", "ServicePurchaseInvoice");

                    case "Edit":
                        URLModelDetails URLModelEdit = new URLModelDetails();
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ServicePIModel.Sinv_no, DocDate = _ServicePIModel.Sinv_dt, ListFilterData = _ServicePIModel.ListFilterData1, WF_status = _ServicePIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                        string SINVDate = _ServicePIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SINVDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ServicePIModel.Sinv_no, DocDate = _ServicePIModel.Sinv_dt, ListFilterData = _ServicePIModel.ListFilterData1, WF_status = _ServicePIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/

                        if (_ServicePIModel.SPIStatus == "A")
                        {
                            string checkforCancle = CheckPIForCancellationinVoucher(_ServicePIModel.Sinv_no, _ServicePIModel.Sinv_dt);
                            if (checkforCancle != "")
                            {
                                //Session["Message"] = checkforCancle;
                                _ServicePIModel.Message = checkforCancle;
                                _ServicePIModel.BtnName = "BtnToDetailPage";
                                _ServicePIModel.Command = "Refresh";
                                URLModelEdit.Command = "Refresh";
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.Sinv_no = _ServicePIModel.Sinv_no;
                                URLModelEdit.Sinv_dt = _ServicePIModel.Sinv_dt;
                                URLModelEdit.BtnName = _ServicePIModel.BtnName;
                                
                                TempData["ModelData"] = _ServicePIModel;

                                TempData["FilterData"] = _ServicePIModel.ListFilterData1;
                            }
                            else
                            {
                                _ServicePIModel.TransType = "Update";
                                _ServicePIModel.Command = command;
                                _ServicePIModel.BtnName = "BtnEdit";
                                TempData["ModelData"] = _ServicePIModel;
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.Command = command;
                                URLModelEdit.BtnName = "BtnEdit";
                                URLModelEdit.Sinv_no = _ServicePIModel.Sinv_no;
                                URLModelEdit.Sinv_dt = _ServicePIModel.Sinv_dt;


                                TempData["ListFilterData"] = _ServicePIModel.ListFilterData1;
                            }
                        }
                        else
                        {

                            _ServicePIModel.TransType = "Update";
                            _ServicePIModel.Command = command;
                            _ServicePIModel.BtnName = "BtnEdit";
                            TempData["ModelData"] = _ServicePIModel;
                            URLModelEdit.TransType = "Update";
                            URLModelEdit.Command = command;
                            URLModelEdit.BtnName = "BtnEdit";
                            URLModelEdit.Sinv_no = _ServicePIModel.Sinv_no;
                            URLModelEdit.Sinv_dt = _ServicePIModel.Sinv_dt;
                            TempData["ListFilterData"] = _ServicePIModel.ListFilterData1;
                        }
                        return RedirectToAction("ServicePurchaseInvoiceDetail", URLModelEdit);
                    case "Delete":
                        ServicePIModel _ServicePIModelDelete = new ServicePIModel();
                        _ServicePIModel.Command = command;
                        _ServicePIModel.BtnName = "Refresh";
                        //Inv_No = _ServicePIModel.Sinv_no;
                        ServicePIDelete(_ServicePIModel, command, _ServicePIModel.Title);
                        _ServicePIModelDelete.Message = "Deleted";
                        _ServicePIModelDelete.Command = "Refresh";
                        _ServicePIModelDelete.TransType = "Refresh";
                        _ServicePIModelDelete.AppStatus = "DL";
                        _ServicePIModelDelete.BtnName = "BtnDelete";
                        _ServicePIModelDelete.SourceType = "D";
                        TempData["ModelData"] = _ServicePIModelDelete;
                        TempData["ListFilterData"] = _ServicePIModel.ListFilterData1;
                        return RedirectToAction("ServicePurchaseInvoiceDetail");

                    case "Save":
                        //Session["Command"] = command;
                        //URLModelDetails URLModelSave = new URLModelDetails();
                        _ServicePIModel.Command = command;
                        if (_ServicePIModel.TransType == null)
                        {
                            _ServicePIModel.TransType = command;
                        }
                        string checkforCancle_onSave = CheckPIForCancellationinVoucher(_ServicePIModel.Sinv_no, _ServicePIModel.Sinv_dt);
                        if (checkforCancle_onSave != "")//Added by Suraj on 21-09-2024 to check before cancel
                        {
                            _ServicePIModel.Message = checkforCancle_onSave;
                            _ServicePIModel.BtnName = "BtnToDetailPage";
                            TempData["ModelData"] = _ServicePIModel;
                            TempData["FilterData"] = _ServicePIModel.ListFilterData1;
                        }
                        else
                        {
                            SaveServicePurchaseInvoice(_ServicePIModel);
                        }
                        
                        if (_ServicePIModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_ServicePIModel.Message == "DocModify")
                        {
                            DocumentMenuId = _ServicePIModel.DocumentMenuId;
                            CommonPageDetails();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";

                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = _ServicePIModel.SuppID, supp_name = _ServicePIModel.SuppName });
                            _ServicePIModel.SupplierNameList = suppLists;

                            List<SourceDoc> _DocumentNumberList = new List<SourceDoc>();
                            SourceDoc _DocumentNumber = new SourceDoc();
                            _DocumentNumber.doc_no = _ServicePIModel.SrcDocNo;
                            _DocumentNumber.doc_dt = "0";
                            _DocumentNumberList.Add(_DocumentNumber);
                            _ServicePIModel.SourceDocList = _DocumentNumberList;

                            List<CurrancyList> currancyLists = new List<CurrancyList>();
                            currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                            _ServicePIModel.currancyLists = currancyLists;
                            _ServicePIModel.Title = title;


                            _ServicePIModel.Sinv_dt = DateTime.Now.ToString();
                            _ServicePIModel.bill_no = _ServicePIModel.bill_no;
                            _ServicePIModel.bill_date = _ServicePIModel.bill_date;
                            _ServicePIModel.SuppName = _ServicePIModel.SuppName;
                            _ServicePIModel.Address = _ServicePIModel.Address;
                            _ServicePIModel.SrcDocNo = _ServicePIModel.SrcDocNo;
                            _ServicePIModel.SrcDocDate = _ServicePIModel.SrcDocDate;

                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetailsList = ViewData["SRPITaxDetails"];
                            ViewBag.OCTaxDetails = ViewData["OCTaxDetails"];
                            ViewBag.GLAccount = ViewData["VouDetail"];
                            ViewBag.ItemTDSDetails = ViewData["TDSDetails"];
                            ViewBag.CostCenterData = ViewData["CCdetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _ServicePIModel.BtnName = "Refresh";
                            _ServicePIModel.Command = "Refresh";
                            _ServicePIModel.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseInvoice/ServicePurchaseInvoiceDetail.cshtml", _ServicePIModel);

                        }
                        else
                        {
                            TempData["ModelData"] = _ServicePIModel;
                            URLModelDetails URLModel = new URLModelDetails();
                            URLModel.BtnName = "BtnSave";
                            URLModel.Command = command;
                            URLModel.TransType = "Update";
                            URLModel.Sinv_no = _ServicePIModel.Sinv_no;
                            URLModel.Sinv_dt = _ServicePIModel.Sinv_dt;
                            TempData["ListFilterData"] = _ServicePIModel.ListFilterData1;
                            return RedirectToAction("ServicePurchaseInvoiceDetail", URLModel);
                        }
                    case "Forward":
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ServicePIModel.Sinv_no, DocDate = _ServicePIModel.Sinv_dt, ListFilterData = _ServicePIModel.ListFilterData1, WF_status = _ServicePIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                        string SINVDate1 = _ServicePIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SINVDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ServicePIModel.Sinv_no, DocDate = _ServicePIModel.Sinv_dt, ListFilterData = _ServicePIModel.ListFilterData1, WF_status = _ServicePIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/

                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ServicePIModel.Sinv_no, DocDate = _ServicePIModel.Sinv_dt, ListFilterData = _ServicePIModel.ListFilterData1, WF_status = _ServicePIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                        string SINVDate2 = _ServicePIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SINVDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ServicePIModel.Sinv_no, DocDate = _ServicePIModel.Sinv_dt, ListFilterData = _ServicePIModel.ListFilterData1, WF_status = _ServicePIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _ServicePIModel.Command = command;
                        ServicePurchaseInvoiceApprove(_ServicePIModel);
                        URLModelDetails URLModelApprove = new URLModelDetails();
                        URLModelApprove.BtnName = _ServicePIModel.BtnName;
                        URLModelApprove.Command = command;
                        URLModelApprove.TransType = _ServicePIModel.TransType;
                        URLModelApprove.Sinv_no = _ServicePIModel.Sinv_no;
                        URLModelApprove.Sinv_dt = _ServicePIModel.Sinv_dt;
                        TempData["ModelData"] = _ServicePIModel;
                        TempData["WF_status1"] = _ServicePIModel.WF_status1;
                        TempData["ListFilterData"] = _ServicePIModel.ListFilterData1;
                        return RedirectToAction("ServicePurchaseInvoiceDetail", URLModelApprove);

                    case "Refresh":
                        ServicePIModel _ServicePIModelRefresh = new ServicePIModel();
                        _ServicePIModelRefresh.BtnName = "Refresh";
                        _ServicePIModelRefresh.Command = command;
                        _ServicePIModelRefresh.TransType = "Save";
                        _ServicePIModelRefresh.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                        TempData["ModelData"] = _ServicePIModelRefresh;
                        TempData["ListFilterData"] = _ServicePIModel.ListFilterData1;
                        return RedirectToAction("ServicePurchaseInvoiceDetail");

                    case "Print":
                        return GenratePdfFile(_ServicePIModel.DocumentMenuId, _ServicePIModel.Sinv_no, _ServicePIModel.Sinv_dt);
                    case "BacktoList":
                        // ServicePIListModel _SPIListModel = new ServicePIListModel();
                        //_SPIListModel.WF_status = _ServicePIModel.WF_status1; 
                        // TempData["ListFilterData"] = _ServicePIModel.ListFilterData1;
                        var WF_Status = _ServicePIModel.WF_status1;
                        if (_ServicePIModel.ListFilterData1 == "undefined")
                        {
                            TempData["ListFilterData"] = null;
                        }
                        else
                        {
                            TempData["ListFilterData"] = _ServicePIModel.ListFilterData1;
                        }
                        return RedirectToAction("ServicePurchaseInvoice", "ServicePurchaseInvoice", new { WF_Status });

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
        public string CheckPIForCancellationinVoucher(string DocNo, string DocDate)
        {
            string Result = "";
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
                DataSet Deatils = _ServicePI_ISERVICE.CheckSPIDetail(Comp_ID, Br_ID, DocNo, DocDate);

                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = "PaymentsHasBeenAdjustedAgainstThisInvoiceItCanNotBeModified";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return Result;
        }
        public ActionResult SaveServicePurchaseInvoice(ServicePIModel _ServicePIModel)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = _ServicePIModel.Title.Replace(" ", "");

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
            try
            {
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblIOCDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable DtblVouDetail = new DataTable();
                DataTable CRCostCenterDetails = new DataTable();
                DataTable DtblTdsDetail = new DataTable();
                DataTable DtblOcTdsDetail = new DataTable();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("roundoff", typeof(string));
                dtheader.Columns.Add("pm_flag", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(string));
                dtheader.Columns.Add("br_id", typeof(string));
                dtheader.Columns.Add("inv_no", typeof(string));
                dtheader.Columns.Add("inv_dt", typeof(string));
                dtheader.Columns.Add("supp_id", typeof(string));
                dtheader.Columns.Add("bill_no", typeof(string));
                dtheader.Columns.Add("bill_dt", typeof(string));
                dtheader.Columns.Add("curr_id", typeof(string));
                dtheader.Columns.Add("conv_rate", typeof(string));
                dtheader.Columns.Add("SrcType", typeof(string));
                dtheader.Columns.Add("SrcDocNo", typeof(string));
                dtheader.Columns.Add("SrcDocDate", typeof(string));
                dtheader.Columns.Add("UserID", typeof(string));
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("tax_amt", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(string));
                dtheader.Columns.Add("Narration", typeof(string));
                dtheader.Columns.Add("einv_no", typeof(string));
                dtheader.Columns.Add("rcm_app", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));
                

                DataRow dtrowHeader = dtheader.NewRow();
                //dtrowHeader["TransType"] = Session["TransType"].ToString();
                dtrowHeader["TransType"] = _ServicePIModel.TransType;
                dtrowHeader["MenuID"] = DocumentMenuId;
                string cancelflag = _ServicePIModel.Cancelled.ToString();
                if (cancelflag == "False")
                {
                    dtrowHeader["Cancelled"] = "N";
                }
                else
                {
                    dtrowHeader["Cancelled"] = "Y";
                }
                string roundoffflag = _ServicePIModel.RoundOffFlag.ToString();
                if (roundoffflag == "False")
                {
                    dtrowHeader["roundoff"] = "N";
                }
                else
                {
                    dtrowHeader["roundoff"] = "Y";
                }
                dtrowHeader["pm_flag"] = _ServicePIModel.pmflagval;
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["inv_no"] = _ServicePIModel.Sinv_no;
                dtrowHeader["inv_dt"] = _ServicePIModel.Sinv_dt;
                dtrowHeader["supp_id"] = _ServicePIModel.SuppID;
                dtrowHeader["bill_no"] = _ServicePIModel.bill_no;
                dtrowHeader["bill_dt"] = _ServicePIModel.bill_date;
                dtrowHeader["curr_id"] = _ServicePIModel.curr_id;
                dtrowHeader["conv_rate"] = _ServicePIModel.ExRate;
                dtrowHeader["SrcType"] = _ServicePIModel.SourceType;
                if (_ServicePIModel.SrcDocNo == "---Select---" || _ServicePIModel.SrcDocNo == "0")
                {
                    dtrowHeader["SrcDocNo"] = "";
                }
                else
                {
                    dtrowHeader["SrcDocNo"] = _ServicePIModel.SrcDocNo;
                }
                dtrowHeader["SrcDocDate"] = _ServicePIModel.SrcDocDate;
                dtrowHeader["UserID"] = Session["UserId"].ToString();
                dtrowHeader["inv_status"] = IsNull(_ServicePIModel.AppStatus, "D");
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["gr_val"] = _ServicePIModel.GrossValue;
                dtrowHeader["tax_amt"] = _ServicePIModel.TaxAmount;
                dtrowHeader["oc_amt"] = _ServicePIModel.OtherCharges;
                dtrowHeader["net_val_bs"] = _ServicePIModel.NetAmountInBase;
                dtrowHeader["remarks"] = _ServicePIModel.Remarks;
                dtrowHeader["bill_add_id"] = _ServicePIModel.bill_add_id == null ? "0" : _ServicePIModel.bill_add_id;
                dtrowHeader["Narration"] = _ServicePIModel.VoucherNarr;// + " " + _ServicePIModel.Nurration+$" {Resource.On} {Resource.Cancelled}.";
                dtrowHeader["einv_no"] = _ServicePIModel.EInvoive;
                string RCMApplicable = _ServicePIModel.RCMApplicable.ToString();
                if (RCMApplicable == "False")
                {
                    dtrowHeader["rcm_app"] = "N";
                }
                else
                {
                    dtrowHeader["rcm_app"] = "Y";
                }
                dtrowHeader["cancel_remarks"] = _ServicePIModel.CancelledRemarks;
                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;

                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("inv_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("claim_itc", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));
                dtItem.Columns.Add("itm_remarks", typeof(string));/*Add by Hina sharma on 25-10-2024 to add new field ,show remarks for print*/
                dtItem.Columns.Add("Hsn_no", typeof(string));/*Add by Hina sharma on 25-10-2024 to add new field ,show remarks for print*/


                JArray jObject = JArray.Parse(_ServicePIModel.Itemdetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["inv_qty"] = jObject[i]["inv_qty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                    dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                    dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                    dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtrowLines["claim_itc"] = jObject[i]["ClaimITC"].ToString();
                    dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();
                    dtrowLines["itm_remarks"] = jObject[i]["ItmRemarks"].ToString();/*Add by Hina sharma on 25-10-2024 to add new field ,show remarks for print*/
                    dtrowLines["Hsn_no"] = jObject[i]["Hsn_no"].ToString();/*Add by Hina sharma on 25-10-2024 to add new field ,show remarks for print*/
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDetail = dtItem;
                ViewData["ItemDetails"] = dtitemdetail(jObject);

                DtblTaxDetail = ToDtblTaxDetail(_ServicePIModel.ItemTaxdetails,"Y");
                ViewData["TaxDetails"] = ViewData["SRPITaxDetails"];
                DtblOCTaxDetail = ToDtblTaxDetail(_ServicePIModel.OC_TaxDetail,"N");
                ViewData["OCTaxDetails"] = ViewData["SRPITaxDetails"];
                DtblTdsDetail = ToDtblTdsDetail(_ServicePIModel.tds_details,"");
                DtblOcTdsDetail = ToDtblTdsDetail(_ServicePIModel.oc_tds_details,"OC");

                DataTable OC_detail = new DataTable();
                OC_detail.Columns.Add("oc_id", typeof(string));
                OC_detail.Columns.Add("oc_val", typeof(string));
                OC_detail.Columns.Add("tax_amt", typeof(string));
                OC_detail.Columns.Add("total_amt", typeof(string));
                OC_detail.Columns.Add("curr_id", typeof(string));
                OC_detail.Columns.Add("conv_rate", typeof(string));
                OC_detail.Columns.Add("supp_id", typeof(string));
                OC_detail.Columns.Add("supp_type", typeof(string));
                OC_detail.Columns.Add("bill_no", typeof(string));
                OC_detail.Columns.Add("bill_date", typeof(string));
                OC_detail.Columns.Add("tds_amt", typeof(string));
                OC_detail.Columns.Add("roundoff", typeof(string));
                OC_detail.Columns.Add("pm_flag", typeof(string));
                if (_ServicePIModel.ItemOCdetails != null)
                {
                    JArray jObjectOC = JArray.Parse(_ServicePIModel.ItemOCdetails);
                    for (int i = 0; i < jObjectOC.Count; i++)
                    {
                        DataRow dtrowOCDetailsLines = OC_detail.NewRow();
                        dtrowOCDetailsLines["oc_id"] = jObjectOC[i]["oc_id"].ToString();
                        dtrowOCDetailsLines["oc_val"] = jObjectOC[i]["oc_val"].ToString();
                        dtrowOCDetailsLines["tax_amt"] = jObjectOC[i]["tax_amt"].ToString();
                        dtrowOCDetailsLines["total_amt"] = jObjectOC[i]["total_amt"].ToString();
                        dtrowOCDetailsLines["curr_id"] = jObjectOC[i]["curr_id"].ToString();
                        dtrowOCDetailsLines["conv_rate"] = jObjectOC[i]["OC_Conv"].ToString();
                        dtrowOCDetailsLines["supp_id"] = jObjectOC[i]["supp_id"].ToString();
                        dtrowOCDetailsLines["supp_type"] = jObjectOC[i]["supp_type"].ToString();
                        dtrowOCDetailsLines["bill_no"] = jObjectOC[i]["bill_no"].ToString();
                        dtrowOCDetailsLines["bill_date"] = jObjectOC[i]["bill_date"].ToString();
                        dtrowOCDetailsLines["tds_amt"] = jObjectOC[i]["tds_amt"].ToString();
                        dtrowOCDetailsLines["roundoff"] = jObjectOC[i]["round_off"].ToString();
                        dtrowOCDetailsLines["pm_flag"] = jObjectOC[i]["pm_flag"].ToString();
                        OC_detail.Rows.Add(dtrowOCDetailsLines);
                    }
                    ViewData["OCDetails"] = dtOCdetail(jObjectOC);

                }
                DtblIOCDetail = OC_detail;

                /**----------------Cost Center Section--------------------*/
                DataTable CC_Details = new DataTable();

                CC_Details.Columns.Add("vou_sr_no", typeof(string));
                CC_Details.Columns.Add("gl_sr_no", typeof(string));
                CC_Details.Columns.Add("acc_id", typeof(string));
                CC_Details.Columns.Add("cc_id", typeof(int));
                CC_Details.Columns.Add("cc_val_id", typeof(int));
                CC_Details.Columns.Add("cc_amt", typeof(string));

                if (_ServicePIModel.CC_DetailList != null)
                {
                    JArray JAObj = JArray.Parse(_ServicePIModel.CC_DetailList);
                    for (int i = 0; i < JAObj.Count; i++)
                    {
                        DataRow dtrowLines = CC_Details.NewRow();

                        dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                        dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();
                        dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                        dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                        dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                        dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                        CC_Details.Rows.Add(dtrowLines);
                    }
                    ViewData["CCdetail"] = dtCCdetail(JAObj);
                }
                
                CRCostCenterDetails = CC_Details;
                

                DataTable dtAttachment = new DataTable();
                var _ServicePIModelattch = TempData["ModelDataattch"] as ServicePIModelattch;
                TempData["ModelDataattch"] = null;
                if (_ServicePIModel.attatchmentdetail != null)
                {
                    if (_ServicePIModelattch != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_ServicePIModelattch.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _ServicePIModelattch.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }
                    else
                    {
                        if (_ServicePIModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _ServicePIModel.AttachMentDetailItmStp as DataTable;
                        }
                        else
                        {
                            dtAttachment.Columns.Add("id", typeof(string));
                            dtAttachment.Columns.Add("file_name", typeof(string));
                            dtAttachment.Columns.Add("file_path", typeof(string));
                            dtAttachment.Columns.Add("file_def", typeof(char));
                            dtAttachment.Columns.Add("comp_id", typeof(Int32));
                        }
                    }
                    JArray jObject1 = JArray.Parse(_ServicePIModel.attatchmentdetail);
                    for (int i = 0; i < jObject1.Count; i++)
                    {
                        string flag = "Y";
                        foreach (DataRow dr in dtAttachment.Rows)
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

                            DataRow dtrowAttachment1 = dtAttachment.NewRow();
                            if (!string.IsNullOrEmpty(_ServicePIModel.Sinv_no))
                            {
                                dtrowAttachment1["id"] = _ServicePIModel.Sinv_no;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_ServicePIModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_ServicePIModel.Sinv_no))
                            {
                                ItmCode = _ServicePIModel.Sinv_no;
                            }
                            else
                            {
                                ItmCode = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + ItmCode.Replace("/", "") + "*");

                            foreach (var fielpath in filePaths)
                            {
                                string flag = "Y";
                                foreach (DataRow dr in dtAttachment.Rows)
                                {
                                    string drImgPath = dr["file_path"].ToString();
                                    if (drImgPath == fielpath.Replace("/", @"\"))
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
                    DtblAttchDetail = dtAttachment;
                }


                //DataTable vou_Details = new DataTable();
                //vou_Details.Columns.Add("comp_id", typeof(string));
                //vou_Details.Columns.Add("id", typeof(string));
                //vou_Details.Columns.Add("type", typeof(string));
                //vou_Details.Columns.Add("doctype", typeof(string));
                //vou_Details.Columns.Add("Value", typeof(string));
                //vou_Details.Columns.Add("DrAmt", typeof(string));
                //vou_Details.Columns.Add("CrAmt", typeof(string));
                //vou_Details.Columns.Add("TransType", typeof(string));
                //vou_Details.Columns.Add("Gltype", typeof(string));
                //if (_ServicePIModel.vouDetail != null)
                //{

                //    JArray jObjectVOU = JArray.Parse(_ServicePIModel.vouDetail);
                //    for (int i = 0; i < jObjectVOU.Count; i++)
                //    {
                //        DataRow dtrowVouDetailsLines = vou_Details.NewRow();
                //        dtrowVouDetailsLines["comp_id"] = jObjectVOU[i]["comp_id"].ToString();
                //        dtrowVouDetailsLines["id"] = jObjectVOU[i]["id"].ToString();
                //        dtrowVouDetailsLines["type"] = jObjectVOU[i]["type"].ToString();
                //        dtrowVouDetailsLines["doctype"] = jObjectVOU[i]["doctype"].ToString();
                //        dtrowVouDetailsLines["Value"] = jObjectVOU[i]["Value"].ToString();
                //        dtrowVouDetailsLines["DrAmt"] = jObjectVOU[i]["DrAmt"].ToString();
                //        dtrowVouDetailsLines["CrAmt"] = jObjectVOU[i]["CrAmt"].ToString();
                //        dtrowVouDetailsLines["TransType"] = jObjectVOU[i]["TransType"].ToString();
                //        dtrowVouDetailsLines["Gltype"] = jObjectVOU[i]["Gltype"].ToString();
                //        vou_Details.Rows.Add(dtrowVouDetailsLines);
                //    }
                //    ViewData["VouDetail"] = dtVoudetail(jObjectVOU);
                //}
                //DtblVouDetail = vou_Details;
                DtblVouDetail = ToDtblVouDetail(_ServicePIModel.vouDetail);
                if (cancelflag == "True")
                {
                    //_ServicePIModel.DN_Nurration = _ServicePIModel.VoucherNarr + " " + _ServicePIModel.Nurration + $" {Resource.On} {Resource.Cancelled}.";
                    _ServicePIModel.DN_Nurration = _ServicePIModel.Nurration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }
                    
                SaveMessage = _ServicePI_ISERVICE.InsertSPI_Details(DtblHDetail, DtblItemDetail
                    , DtblTaxDetail, DtblOCTaxDetail, DtblIOCDetail, DtblAttchDetail, DtblVouDetail
                    , CRCostCenterDetails, DtblTdsDetail, DtblOcTdsDetail, _ServicePIModel.TDS_Amount
                    , _ServicePIModel.BP_Nurration, _ServicePIModel.DN_Nurration);

                string[] detail = SaveMessage.Split(',');
                string Status = detail[2].ToString();
                if (SaveMessage == "DocModify")
                {
                    _ServicePIModel.Message = "DocModify";
                    _ServicePIModel.BtnName = "Refresh";
                    _ServicePIModel.Command = "Refresh";
                    _ServicePIModel.DocumentMenuId = DocumentMenuId;
                    TempData["ModelData"] = _ServicePIModel;
                    return RedirectToAction("ServicePurchaseInvoiceDetail");
                }
                // Added by Nidhi on 12-05-2025 10:49
                else if (Status == "Cancelled")
                {
                    try
                    {
                        //string fileName = "SPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "ServicePurchaseInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_ServicePIModel.Sinv_no, _ServicePIModel.Sinv_dt, fileName, "105101150","C");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, "105101150", _ServicePIModel.Sinv_no, "Cancel", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _ServicePIModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _ServicePIModel.Message = _ServicePIModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    //_ServicePIModel.Message = "Cancelled";
                    _ServicePIModel.Command = "Update";
                    _ServicePIModel.AppStatus = "D";
                    _ServicePIModel.BtnName = "Refresh";
                    _ServicePIModel.TransType = "Update";
                    return RedirectToAction("ServicePurchaseInvoiceDetail");
                }
                // END 
                else
                {
                    string[] FDetail = SaveMessage.Split(',');
                    string Message = FDetail[5].ToString();
                    string Inv_no = FDetail[0].ToString();
                    string Inv_DATE = FDetail[6].ToString();
                    string Cansal = FDetail[1].ToString();
                    if (Message == "DataNotFound")
                    {
                        var msg = "Data Not found" + " " + Inv_DATE + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _ServicePIModel.Message = Message;
                        return RedirectToAction("ServicePurchaseInvoiceDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_ServicePIModelattch != null)
                        {
                            if (_ServicePIModelattch.Guid != null)
                            {
                                Guid = _ServicePIModelattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, _ServicePIModel.TransType, DtblAttchDetail);

                        //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                        //if (Directory.Exists(sourcePath))
                        //{
                        //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
                        //    foreach (string file in filePaths)
                        //    {
                        //        string[] items = file.Split('\\');
                        //        string ItemName = items[items.Length - 1];
                        //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                        //        foreach (DataRow dr in DtblAttchDetail.Rows)
                        //        {
                        //            string DrItmNm = dr["file_name"].ToString();
                        //            if (ItemName == DrItmNm)
                        //            {
                        //                string Inv_no1 = Inv_no.Replace("/", "");
                        //                string img_nm = CompID + BrchID + Inv_no1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                    if (Cansal == "C" && Message == "Update")
                    {
                        _ServicePIModel.Message = "Cancelled";
                        _ServicePIModel.Command = "Update";
                        _ServicePIModel.Sinv_no = Inv_no;
                        _ServicePIModel.Sinv_dt = Inv_DATE;
                        _ServicePIModel.TransType = "Update";
                        _ServicePIModel.AppStatus = "D";
                        _ServicePIModel.BtnName = "Refresh";
                        //Session["Message"] = "Cancelled";
                        //Session["Command"] = "Update";
                        //Session["Inv_No"] = Inv_no;
                        //Session["Inv_Date"] = Inv_DATE;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "Refresh";
                        //Session["AttachMentDetailItmStp"] = null;
                        //Session["Guid"] = null;
                    }
                    else
                    {
                        if (Message == "Update" || Message == "Save")
                        {
                            _ServicePIModel.Message = "Save";
                            _ServicePIModel.Command = "Update";
                            _ServicePIModel.Sinv_no = Inv_no;
                            _ServicePIModel.Sinv_dt = Inv_DATE;
                            _ServicePIModel.TransType = "Update";
                            _ServicePIModel.AppStatus = "D";
                            _ServicePIModel.BtnName = "BtnSave";
                            _ServicePIModel.AttachMentDetailItmStp = null;
                            _ServicePIModel.Guid = null;
                            // Session["Message"] = "Save";
                            // Session["Command"] = "Update";
                            // Session["Inv_No"] = Inv_no;
                            //Session["Inv_Date"] = Inv_DATE;
                            // Session["TransType"] = "Update";
                            // Session["AppStatus"] = 'D';
                            // Session["BtnName"] = "BtnSave";
                            // Session["AttachMentDetailItmStp"] = null;
                            // Session["Guid"] = null;
                        }
                    }
                    return RedirectToAction("ServicePurchaseInvoiceDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ServicePIModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ServicePIModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ServicePIModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetGLDetails(List<Cmn_GL_Detail> GLDetail)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                DataTable DtblGLDetail = new DataTable();

                if (GLDetail != null)
                {
                    DtblGLDetail = ToDataTable(GLDetail);
                }
                DataSet GlDt = _ServicePI_ISERVICE.GetAllGLDetails(DtblGLDetail);
                Validate = Json(GlDt);
                JsonResult DataRows = null;
                DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);

                return DataRows;
            }

            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            //return Validate;
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        public DataTable ToDataTable<T>(IList<T> data)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                object[] values = new object[props.Count];
                using (DataTable table = new DataTable())
                {
                    long _pCt = props.Count;
                    for (int i = 0; i < _pCt; ++i)
                    {
                        PropertyDescriptor prop = props[i];
                        table.Columns.Add(prop.Name, prop.PropertyType);
                    }
                    foreach (T item in data)
                    {
                        long _vCt = values.Length;
                        for (int i = 0; i < _vCt; ++i)
                        {
                            values[i] = props[i].GetValue(item);
                        }
                        table.Rows.Add(values);
                    }
                    return table;
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private DataTable ToDtblTaxDetail(string TaxDetails,string recov)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable Tax_detail = new DataTable();
                Tax_detail.Columns.Add("item_id", typeof(string));
                Tax_detail.Columns.Add("tax_id", typeof(string));
                Tax_detail.Columns.Add("tax_rate", typeof(string));
                Tax_detail.Columns.Add("tax_val", typeof(string));
                Tax_detail.Columns.Add("tax_level", typeof(string));
                Tax_detail.Columns.Add("tax_apply_on", typeof(string));
                Tax_detail.Columns.Add("tax_recov", typeof(string));
                if (TaxDetails != null)
                {
                    JArray jObjectTax = JArray.Parse(TaxDetails);
                    for (int i = 0; i < jObjectTax.Count; i++)
                    {
                        DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();
                        dtrowTaxDetailsLines["item_id"] = jObjectTax[i]["item_id"].ToString();
                        dtrowTaxDetailsLines["tax_id"] = jObjectTax[i]["tax_id"].ToString();
                        string tax_rate = jObjectTax[i]["tax_rate"].ToString();
                        tax_rate = tax_rate.Replace("%", "");
                        dtrowTaxDetailsLines["tax_rate"] = tax_rate;
                        dtrowTaxDetailsLines["tax_level"] = jObjectTax[i]["tax_level"].ToString();
                        dtrowTaxDetailsLines["tax_val"] = jObjectTax[i]["tax_val"].ToString();
                        dtrowTaxDetailsLines["tax_apply_on"] = jObjectTax[i]["tax_apply_on"].ToString();
                        dtrowTaxDetailsLines["tax_recov"] = recov=="Y"?jObjectTax[i]["tax_recov"].ToString():"";
                        Tax_detail.Rows.Add(dtrowTaxDetailsLines);
                    }
                    ViewData["SRPITaxDetails"] = dtTaxdetail(jObjectTax);

                }
                DtblItemTaxDetail = Tax_detail;

                return DtblItemTaxDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblTdsDetail(string tdsDetails,string tdsType)
        {
            try
            {
                DataTable DtblItemtdsDetail = new DataTable();
                DataTable tds_detail = new DataTable();
                tds_detail.Columns.Add("tds_id", typeof(string));
                tds_detail.Columns.Add("tds_rate", typeof(string));
                tds_detail.Columns.Add("tds_val", typeof(string));
                tds_detail.Columns.Add("tds_level", typeof(string));
                tds_detail.Columns.Add("tds_apply_on", typeof(string));
                if (tdsType == "OC")
                {
                    tds_detail.Columns.Add("oc_id", typeof(string));
                    tds_detail.Columns.Add("supp_id", typeof(string));
                }
                tds_detail.Columns.Add("tds_base_amt", typeof(string));
                tds_detail.Columns.Add("tds_ass_apply_on", typeof(string));

                if (tdsDetails != null)
                {
                    JArray jObjecttds = JArray.Parse(tdsDetails);
                    for (int i = 0; i < jObjecttds.Count; i++)
                    {
                        DataRow dtrowtdsDetailsLines = tds_detail.NewRow();
                        dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
                        string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
                        tds_rate = tds_rate.Replace("%", "");
                        dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                        dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
                        dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();
                        if (tdsType == "OC")
                        {
                            dtrowtdsDetailsLines["oc_id"] = jObjecttds[i]["Tds_oc_id"].ToString();
                            dtrowtdsDetailsLines["supp_id"] = jObjecttds[i]["Tds_supp_id"].ToString();
                        }
                        dtrowtdsDetailsLines["tds_base_amt"] = jObjecttds[i]["Tds_totalAmnt"].ToString();
                        dtrowtdsDetailsLines["tds_ass_apply_on"] = jObjecttds[i]["Tds_AssValApplyOn"].ToString();
                        tds_detail.Rows.Add(dtrowtdsDetailsLines);
                    }
                    ViewData["TDSDetails"] = dtTDSdetail(jObjecttds);
                }
                DtblItemtdsDetail = tds_detail;
                return DtblItemtdsDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblVouDetail(string VouDetails)
        {
            try
            {
                DataTable DtblVouDetail = new DataTable();
                DataTable vou_Details = new DataTable();
                vou_Details.Columns.Add("comp_id", typeof(string));
                vou_Details.Columns.Add("vou_sr_no", typeof(string));
                vou_Details.Columns.Add("gl_sr_no", typeof(string));
                vou_Details.Columns.Add("id", typeof(string));
                vou_Details.Columns.Add("type", typeof(string));
                vou_Details.Columns.Add("doctype", typeof(string));
                vou_Details.Columns.Add("Value", typeof(string));
                vou_Details.Columns.Add("ValueInBase", typeof(string));
                vou_Details.Columns.Add("DrAmt", typeof(string));
                vou_Details.Columns.Add("CrAmt", typeof(string));
                vou_Details.Columns.Add("TransType", typeof(string));
                vou_Details.Columns.Add("Gltype", typeof(string));
                vou_Details.Columns.Add("parent", typeof(string));
                vou_Details.Columns.Add("DrAmtInBase", typeof(string));
                vou_Details.Columns.Add("CrAmtInBase", typeof(string));
                vou_Details.Columns.Add("curr_id", typeof(string));
                vou_Details.Columns.Add("conv_rate", typeof(string));
                vou_Details.Columns.Add("vou_type", typeof(string));
                vou_Details.Columns.Add("bill_no", typeof(string));
                vou_Details.Columns.Add("bill_date", typeof(string));
                vou_Details.Columns.Add("gl_narr", typeof(string));
                if (VouDetails != null)
                {
                    JArray jObjectVOU = JArray.Parse(VouDetails);
                    for (int i = 0; i < jObjectVOU.Count; i++)
                    {
                        DataRow dtrowVouDetailsLines = vou_Details.NewRow();
                        dtrowVouDetailsLines["comp_id"] = jObjectVOU[i]["comp_id"].ToString();
                        dtrowVouDetailsLines["vou_sr_no"] = jObjectVOU[i]["VouSrNo"].ToString();
                        dtrowVouDetailsLines["gl_sr_no"] = jObjectVOU[i]["GlSrNo"].ToString();
                        dtrowVouDetailsLines["id"] = jObjectVOU[i]["id"].ToString();
                        dtrowVouDetailsLines["type"] = jObjectVOU[i]["type"].ToString();
                        dtrowVouDetailsLines["doctype"] = jObjectVOU[i]["doctype"].ToString();
                        dtrowVouDetailsLines["Value"] = jObjectVOU[i]["Value"].ToString();
                        dtrowVouDetailsLines["ValueInBase"] = jObjectVOU[i]["ValueInBase"].ToString();
                        dtrowVouDetailsLines["DrAmt"] = jObjectVOU[i]["DrAmt"].ToString();
                        dtrowVouDetailsLines["CrAmt"] = jObjectVOU[i]["CrAmt"].ToString();
                        dtrowVouDetailsLines["TransType"] = jObjectVOU[i]["TransType"].ToString();
                        dtrowVouDetailsLines["Gltype"] = jObjectVOU[i]["Gltype"].ToString();
                        dtrowVouDetailsLines["parent"] = "0";// jObjectVOU[i]["Gltype"].ToString();
                        dtrowVouDetailsLines["DrAmtInBase"] = jObjectVOU[i]["DrAmtInBase"].ToString();
                        dtrowVouDetailsLines["CrAmtInBase"] = jObjectVOU[i]["CrAmtInBase"].ToString();
                        dtrowVouDetailsLines["curr_id"] = jObjectVOU[i]["curr_id"].ToString();
                        dtrowVouDetailsLines["conv_rate"] = jObjectVOU[i]["conv_rate"].ToString();
                        dtrowVouDetailsLines["vou_type"] = jObjectVOU[i]["vou_type"].ToString();
                        dtrowVouDetailsLines["bill_no"] = jObjectVOU[i]["bill_no"].ToString();
                        dtrowVouDetailsLines["bill_date"] = jObjectVOU[i]["bill_date"].ToString();
                        dtrowVouDetailsLines["gl_narr"] = jObjectVOU[i]["gl_narr"].ToString();
                        vou_Details.Rows.Add(dtrowVouDetailsLines);
                    }
                    ViewData["VouDetail"] = dtVoudetail(jObjectVOU);
                }
                DtblVouDetail = vou_Details;
                return DtblVouDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        public DataTable dtitemdetail(JArray jObject)
        {

            DataTable dtItem = new DataTable();


            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("inv_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_gr_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("hsn_no", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("itm_remarks", typeof(string));/*Add by Hina sharma on 25-10-2024 to add new field ,show remarks for print*/

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();

                dtrowLines["inv_qty"] = jObject[i]["inv_qty"].ToString();
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                if (jObject[i]["item_oc_amt"].ToString() == "" || jObject[i]["item_oc_amt"].ToString() == null)
                {
                    dtrowLines["item_oc_amt"] = "0.00";
                }
                else
                {
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                }
                dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                dtrowLines["hsn_no"] = jObject[i]["Hsn_no"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                dtrowLines["itm_remarks"] = jObject[i]["ItmRemarks"].ToString();/*Add by Hina sharma on 25-10-2024 to add new field ,show remarks for print*/

                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }

        public DataTable dtTaxdetail(JArray jObjectTax)
        {
            DataTable Tax_detail = new DataTable();

            Tax_detail.Columns.Add("item_id", typeof(string));
            Tax_detail.Columns.Add("tax_id", typeof(int));
            Tax_detail.Columns.Add("tax_name", typeof(string));
            Tax_detail.Columns.Add("tax_rate", typeof(string));
            Tax_detail.Columns.Add("tax_val", typeof(string));
            Tax_detail.Columns.Add("tax_level", typeof(int));
            Tax_detail.Columns.Add("tax_apply_on", typeof(string));
            Tax_detail.Columns.Add("tax_apply_Name", typeof(string));
            Tax_detail.Columns.Add("item_tax_amt", typeof(string));

            for (int i = 0; i < jObjectTax.Count; i++)
            {
                DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();

                dtrowTaxDetailsLines["item_id"] = jObjectTax[i]["item_id"].ToString();
                dtrowTaxDetailsLines["tax_id"] = jObjectTax[i]["tax_id"].ToString();
                dtrowTaxDetailsLines["tax_name"] = jObjectTax[i]["TaxName"].ToString();
                //dtrowTaxDetailsLines["tax_rate"] = jObjectTax[i]["tax_rate"].ToString();
                string tax_rate = jObjectTax[i]["tax_rate"].ToString();
                tax_rate = tax_rate.Replace("%", "");
                dtrowTaxDetailsLines["tax_rate"] = tax_rate;
                dtrowTaxDetailsLines["tax_val"] = jObjectTax[i]["tax_val"].ToString();
                dtrowTaxDetailsLines["tax_level"] = jObjectTax[i]["tax_level"].ToString();
                dtrowTaxDetailsLines["tax_apply_on"] = jObjectTax[i]["tax_apply_on"].ToString();
                dtrowTaxDetailsLines["tax_apply_Name"] = jObjectTax[i]["tax_apply_onName"].ToString();
                dtrowTaxDetailsLines["item_tax_amt"] = jObjectTax[i]["totaltax_amt"].ToString();


                Tax_detail.Rows.Add(dtrowTaxDetailsLines);
            }

            return Tax_detail;
        }

        public DataTable dtTDSdetail(JArray jObjecttds)
        {
            DataTable DtblItemtdsDetail = new DataTable();
            DataTable tds_detail = new DataTable();
            tds_detail.Columns.Add("tds_id", typeof(string));
            tds_detail.Columns.Add("tds_rate", typeof(string));
            tds_detail.Columns.Add("tds_val", typeof(string));
            tds_detail.Columns.Add("tds_level", typeof(string));
            tds_detail.Columns.Add("tds_apply_on", typeof(string));
            tds_detail.Columns.Add("tds_name", typeof(string));
            tds_detail.Columns.Add("tds_apply_Name", typeof(string));

            for (int i = 0; i < jObjecttds.Count; i++)
            {
                DataRow dtrowtdsDetailsLines = tds_detail.NewRow();
                dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
                string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
                tds_rate = tds_rate.Replace("%", "");
                dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
                dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
                dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();
                dtrowtdsDetailsLines["tds_name"] = jObjecttds[i]["Tds_name"].ToString();
                dtrowtdsDetailsLines["tds_apply_Name"] = jObjecttds[i]["Tds_applyOnName"].ToString();
                tds_detail.Rows.Add(dtrowtdsDetailsLines);
            }

            return tds_detail;
        }
        public DataTable dtOCdetail(JArray jObjectOC)
        {
            DataTable OC_detail = new DataTable();

            OC_detail.Columns.Add("oc_id", typeof(int));
            OC_detail.Columns.Add("oc_name", typeof(string));
            OC_detail.Columns.Add("curr_name", typeof(string));
            OC_detail.Columns.Add("conv_rate", typeof(string));
            OC_detail.Columns.Add("oc_val", typeof(string));
            OC_detail.Columns.Add("OCValBs", typeof(string));
            OC_detail.Columns.Add("tax_amt", typeof(string));
            OC_detail.Columns.Add("total_amt", typeof(string));
            OC_detail.Columns.Add("supp_type", typeof(string));
            OC_detail.Columns.Add("bill_no", typeof(string));
            OC_detail.Columns.Add("bill_date", typeof(string));

            for (int i = 0; i < jObjectOC.Count; i++)
            {
                DataRow dtrowOCDetailsLines = OC_detail.NewRow();

                dtrowOCDetailsLines["oc_id"] = jObjectOC[i]["oc_id"].ToString();
                dtrowOCDetailsLines["oc_name"] = jObjectOC[i]["OCName"].ToString();
                dtrowOCDetailsLines["curr_name"] = jObjectOC[i]["OC_Curr"].ToString();
                dtrowOCDetailsLines["conv_rate"] = jObjectOC[i]["OC_Conv"].ToString();
                dtrowOCDetailsLines["oc_val"] = jObjectOC[i]["oc_val"].ToString();
                dtrowOCDetailsLines["OCValBs"] = jObjectOC[i]["OC_AmtBs"].ToString();
                dtrowOCDetailsLines["tax_amt"] = jObjectOC[i]["tax_amt"].ToString();
                dtrowOCDetailsLines["total_amt"] = jObjectOC[i]["total_amt"].ToString();
                dtrowOCDetailsLines["supp_type"] = jObjectOC[i]["supp_type"].ToString();
                dtrowOCDetailsLines["bill_no"] = jObjectOC[i]["bill_no"].ToString();
                dtrowOCDetailsLines["bill_date"] = jObjectOC[i]["bill_date"].ToString();

                OC_detail.Rows.Add(dtrowOCDetailsLines);
            }

            return OC_detail;
        }
        public DataTable dtCCdetail(JArray JAObj)
        {
            DataTable CC_Details = new DataTable();

            CC_Details.Columns.Add("acc_id", typeof(string));
            CC_Details.Columns.Add("cc_id", typeof(int));
            CC_Details.Columns.Add("cc_val_id", typeof(int));
            CC_Details.Columns.Add("cc_amt", typeof(string));
            CC_Details.Columns.Add("cc_name", typeof(string));
            CC_Details.Columns.Add("cc_val_name", typeof(string));
            CC_Details.Columns.Add("vou_sr_no", typeof(string));
            CC_Details.Columns.Add("gl_sr_no", typeof(string));
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();
                dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();

                CC_Details.Rows.Add(dtrowLines);
            }
            return CC_Details;
        }
        public DataTable dtVoudetail(JArray jObjectVOU)
        {
            DataTable vou_Details = new DataTable();
            vou_Details.Columns.Add("comp_id", typeof(string));
            vou_Details.Columns.Add("acc_id", typeof(string));
            vou_Details.Columns.Add("acc_name", typeof(string));
            vou_Details.Columns.Add("type", typeof(string));
            vou_Details.Columns.Add("doctype", typeof(string));
            vou_Details.Columns.Add("Value", typeof(string));
            vou_Details.Columns.Add("dr_amt_sp", typeof(string));
            vou_Details.Columns.Add("cr_amt_sp", typeof(string));
            vou_Details.Columns.Add("TransType", typeof(string));
            vou_Details.Columns.Add("gl_type", typeof(string));
            for (int i = 0; i < jObjectVOU.Count; i++)
            {
                DataRow dtrowVouDetailsLines = vou_Details.NewRow();
                dtrowVouDetailsLines["comp_id"] = jObjectVOU[i]["comp_id"].ToString();
                dtrowVouDetailsLines["acc_id"] = jObjectVOU[i]["id"].ToString();
                dtrowVouDetailsLines["acc_name"] = jObjectVOU[i]["id"].ToString();
                dtrowVouDetailsLines["type"] = jObjectVOU[i]["type"].ToString();
                dtrowVouDetailsLines["doctype"] = jObjectVOU[i]["doctype"].ToString();
                dtrowVouDetailsLines["Value"] = jObjectVOU[i]["Value"].ToString();
                dtrowVouDetailsLines["dr_amt_sp"] = jObjectVOU[i]["DrAmt"].ToString();
                dtrowVouDetailsLines["cr_amt_sp"] = jObjectVOU[i]["CrAmt"].ToString();
                dtrowVouDetailsLines["TransType"] = jObjectVOU[i]["TransType"].ToString();
                dtrowVouDetailsLines["gl_type"] = jObjectVOU[i]["Gltype"].ToString();
                vou_Details.Rows.Add(dtrowVouDetailsLines);
            }
            return vou_Details;
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
        private ActionResult ServicePIDelete(ServicePIModel _SPIModel, string command, string title)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();

                string BPNo = _SPIModel.Sinv_no;
                string BankPayNumber = BPNo.Replace("/", "");

                string Message = _ServicePI_ISERVICE.ServicePIDelete(_SPIModel, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(BankPayNumber))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, BankPayNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                _SPIModel.Message = "Deleted";
                _SPIModel.Command = "Refresh";
                _SPIModel.Sinv_no = null;
                _SPIModel.Sinv_dt = null;
                _SPIModel.TransType = "Refresh";
                _SPIModel.AppStatus = "DL";
                _SPIModel.BtnName = "BtnDelete";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["Inv_No"] = null;
                //Session["Inv_Date"] = null;
                //_SPIModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("ServicePurchaseInvoiceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult ServicePurchaseInvoiceApprove(ServicePIModel _SPIModel)
        {
            //JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            //JsonResult DataRows = null;
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
                if (DocumentMenuId != null)
                {
                    MenuDocId = DocumentMenuId;
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    MenuDocId = Session["MenuDocumentId"].ToString();
                //}
                var DocNo = _SPIModel.Sinv_no;
                var DocDate = _SPIModel.Sinv_dt;
                var A_Status = _SPIModel.A_Status;
                var A_Level = _SPIModel.A_Level;
                var A_Remarks = _SPIModel.A_Remarks;
                var VoucherNarr = /*_SPIModel.VoucherNarr +" "+ */_SPIModel.Nurration;
                var Bp_Nurr = _SPIModel.BP_Nurration;
                var Dn_Nurr = _SPIModel.DN_Nurration;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _ServicePI_ISERVICE.ApproveSPI(DocNo, DocDate, MenuDocId, BranchID, Comp_ID
                    , UserID, mac_id, A_Status, A_Level, A_Remarks, VoucherNarr, Bp_Nurr, Dn_Nurr);
                string[] FDetail = Message.Split(',');
                string ApMessage = FDetail[6].ToString().Trim();
                string INV_NO = FDetail[0].ToString();
                string INV_DT = FDetail[7].ToString();

                // Added by Nidhi on 12-05-2025 10:51
                try
                {
                    //string fileName = "SPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "ServicePurchaseInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(INV_NO, INV_DT, fileName, "105101150","AP");
                    _Common_IServices.SendAlertEmail(CompID, BrchID, "105101150", INV_NO, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _SPIModel.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                // END 

                if (ApMessage == "A")
                {
                    //Session["Message"] = "Approved";
                    //_SPIModel.Message = "Approved";
                    _SPIModel.Message = _SPIModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                _SPIModel.TransType = "Update";
                _SPIModel.Command = "Approve";
                _SPIModel.Sinv_no = INV_NO;
                _SPIModel.Sinv_dt = INV_DT;
                _SPIModel.AppStatus = "D";
                _SPIModel.BtnName = "BtnEdit";

                // Session["TransType"] = "Update";
                // Session["Command"] = "Approve";
                // Session["Inv_No"] = INV_NO;
                //Session["Inv_Date"] = INV_DT;
                // Session["AppStatus"] = 'D';
                // Session["BtnName"] = "BtnEdit";
                //TempData["ListFilterData"] = FilterData;             
                return RedirectToAction("ServicePurchaseInvoiceDetail");
                //DataRows = Json(GrnDetail);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            //return DataRows;
        }

        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1)
        {
            try
            {
                //JArray jObjectBatch = JArray.Parse(list);
                ServicePIModel _SPIModel = new ServicePIModel();
                URLModelDetails URLModel = new URLModelDetails();
                if (AppDtList != null)
                {
                    JArray jObjectBatch = JArray.Parse(AppDtList);
                    for (int i = 0; i < jObjectBatch.Count; i++)
                    {
                        _SPIModel.Sinv_no = jObjectBatch[i]["DocNo"].ToString();
                        _SPIModel.Sinv_dt = jObjectBatch[i]["DocDate"].ToString();
                        _SPIModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                        _SPIModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                        _SPIModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                        _SPIModel.VoucherNarr = jObjectBatch[i]["VoucherNarr"].ToString();
                        _SPIModel.Nurration = jObjectBatch[i]["Nurration"].ToString();
                        _SPIModel.BP_Nurration = jObjectBatch[i]["BP_Nurration"].ToString();
                        _SPIModel.DN_Nurration = jObjectBatch[i]["DN_Nurration"].ToString();

                    }
                }
                if (_SPIModel.A_Status != "Approve")
                {
                    _SPIModel.A_Status = "Approve";
                }
                _SPIModel.ListFilterData1 = ListFilterData1;
                _SPIModel.WF_status1 = WF_status1;
                ServicePurchaseInvoiceApprove(_SPIModel);
                TempData["ModelData"] = _SPIModel;
                TempData["WF_status1"] = WF_status1;
              
                URLModel.Sinv_no = _SPIModel.Sinv_no;
                URLModel.Sinv_dt = _SPIModel.Sinv_dt;
                URLModel.TransType = _SPIModel.TransType;
                URLModel.BtnName = _SPIModel.BtnName;
                URLModel.Command = _SPIModel.Command;
                return RedirectToAction("ServicePurchaseInvoiceDetail", URLModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            
        }

        //public ActionResult EditPurchaseInvoice(string Inv_no, string Inv_dt, string FilterData)
        //{
        //    Session["Message"] = "New";
        //    Session["Command"] = "Add";
        //    Session["Inv_No"] = Inv_no;
        //   Session["Inv_Date"] = Inv_dt;
        //    TempData["ListFilterData"] = FilterData;
        //    Session["TransType"] = "Update";
        //    Session["AppStatus"] = 'D';
        //    Session["BtnName"] = "BtnToDetailPage";
        //    return RedirectToAction("ServicePurchaseInvoiceDetail");
        //}

        private List<ServicePurchaseInvoiceList> GetServicePIList(ServicePIListModel _SPIListModel)
        {
            _SPIList = new List<ServicePurchaseInvoiceList>();
            try
            {
                string UserID = string.Empty;
                string SuppType = string.Empty;
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                var wfStatus = "";
                if (_SPIListModel.WF_status != null)
                {
                    wfStatus = _SPIListModel.WF_status;
                }
                else
                {
                    wfStatus = null;
                }

                DataSet DSet = _ServicePI_ISERVICE.GetSPIList(CompID, BrchID, UserID, _SPIListModel.SuppID, _SPIListModel.SPI_FromDate, _SPIListModel.SPI_ToDate, _SPIListModel.Status, _SPIListModel.wfdocid, wfStatus);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ServicePurchaseInvoiceList _ServicePIList = new ServicePurchaseInvoiceList();
                        _ServicePIList.InvNo = dr["inv_no"].ToString();
                        _ServicePIList.InvDate = dr["inv_dt"].ToString();
                        _ServicePIList.InvDt = dr["InvDt"].ToString();
                        _ServicePIList.SourceDocNo = dr["src_doc_number"].ToString();
                        _ServicePIList.InvType = dr["src_type"].ToString();
                        _ServicePIList.InvValue = dr["net_val_bs"].ToString();
                        _ServicePIList.SuppName = dr["supp_name"].ToString();
                        _ServicePIList.Currency = dr["curr_name"].ToString();
                        _ServicePIList.InvStauts = dr["InvStauts"].ToString();
                        _ServicePIList.CreateDate = dr["CreateDate"].ToString();
                        _ServicePIList.ApproveDate = dr["ApproveDate"].ToString();
                        _ServicePIList.ModifyDate = dr["ModifyDate"].ToString();
                        _ServicePIList.create_by = dr["create_by"].ToString();
                        _ServicePIList.app_by = dr["app_by"].ToString();
                        _ServicePIList.mod_by = dr["mod_by"].ToString();
                        _ServicePIList.bill_no = dr["bill_no"].ToString();
                        _ServicePIList.bill_dt = dr["bill_dt"].ToString();
                        _ServicePIList.einvoice = dr["einv_no"].ToString();
                        _SPIList.Add(_ServicePIList);
                    }
                }

                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                ViewBag.FinStDt = DSet.Tables[2].Rows[0]["findate"];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;

            }
            return _SPIList;
        }

        public ActionResult SearchServicePurchaseInvoiceList(string SuppId, string Fromdate, string Todate, string Status)
        {
            ServicePIListModel _SPIListModel = new ServicePIListModel();
            try
            {
                //Session.Remove("WF_Docid");
                //Session.Remove("WF_status");
                string UserID = string.Empty;
                string SuppType = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                SuppType = "L";
                _SPIListModel.WF_status = null;
                _SPIList = new List<ServicePurchaseInvoiceList>();
                DataSet DSet = _ServicePI_ISERVICE.GetSPIList(CompID, BrchID, UserID, SuppId, Fromdate, Todate, Status, DocumentMenuId, _SPIListModel.WF_status);
                //Session["SPISearch"] = "SPI_Search";
                _SPIListModel.SPISearch = "SPI_Search";

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ServicePurchaseInvoiceList _ServicePIList = new ServicePurchaseInvoiceList();
                        _ServicePIList.InvNo = dr["inv_no"].ToString();
                        _ServicePIList.InvDate = dr["inv_dt"].ToString();
                        _ServicePIList.InvDt = dr["InvDt"].ToString();
                        _ServicePIList.SourceDocNo = dr["src_doc_number"].ToString();
                        _ServicePIList.InvType = dr["src_type"].ToString();
                        _ServicePIList.InvValue = dr["net_val_bs"].ToString();
                        _ServicePIList.SuppName = dr["supp_name"].ToString();
                        _ServicePIList.Currency = dr["curr_name"].ToString();
                        _ServicePIList.InvStauts = dr["InvStauts"].ToString();
                        _ServicePIList.CreateDate = dr["CreateDate"].ToString();
                        _ServicePIList.ApproveDate = dr["ApproveDate"].ToString();
                        _ServicePIList.ModifyDate = dr["ModifyDate"].ToString();
                        _ServicePIList.create_by = dr["create_by"].ToString();
                        _ServicePIList.app_by = dr["app_by"].ToString();
                        _ServicePIList.mod_by = dr["mod_by"].ToString();
                        _ServicePIList.bill_no = dr["bill_no"].ToString();
                        _ServicePIList.bill_dt = dr["bill_dt"].ToString();
                        _ServicePIList.einvoice = dr["einv_no"].ToString();
                        _SPIList.Add(_ServicePIList);
                    }
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                ViewBag.FinStDt = DSet.Tables[2].Rows[0]["findate"];
                _SPIListModel.SPIList = _SPIList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            //return PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ServiceSaleInvoive/ServiceSaleInvoiveList.cshtml", _SPIListModel);
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialServicePurchaseInvoiceList.cshtml", _SPIListModel);
        }
        public DataSet GetServicePurchaseInvoiceEdit(string Inv_No, string Inv_Date)
        {
            try
            {
                //JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string Br_ID = string.Empty;
                string Voutype = "PV";
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
                DataSet result = _ServicePI_ISERVICE.GetServicePurchaseInvoiceDetail(Comp_ID, Br_ID, Voutype, Inv_No, Inv_Date, UserID, DocumentMenuId);
                //DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

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
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                ServicePIModelattch _ServicePIModelattch = new ServicePIModelattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                //string TransType = "Save";
                //string PONo = "";
                string branchID = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["SPO_No"] != null)
                //{
                //    PONo = Session["SPO_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _ServicePIModelattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _ServicePIModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _ServicePIModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _ServicePIModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }


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

        /*-------------Print Section--------------*/

        public FileResult GenratePdfFile(string docMenuId, string invNo, string invDate)
        {
            return File(GetPdfData(docMenuId, invNo, invDate), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        }
        public byte[] GetPdfData(string docId, string invNo, string invDt)
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
          
                DataSet Details = _ServicePI_ISERVICE.GetServicePurchaseInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt);
                ViewBag.PageName = "PI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                ViewBag.Details = Details;
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.RCMApplicable = Details.Tables[0].Rows[0]["rcm_app"].ToString().Trim();/*Add by Hina sharma on 25-10-2024 for print*/
                ViewBag.DocumentID = DocumentMenuId;/*Add by Hina sharma on 25-10-2024 for print*/
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";

                ViewBag.Title = "Service Purchase Invoice";
                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/ServicePurchaseInvoice/ServicePurchaseInvoicePrint.cshtml"));


                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
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
            finally
            {

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
        /*-----------Print Section End-----------*/
        /*------------- Added by Nidhi 12-05-2025 10:47 ----------*/
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
            try
            {
                DataTable dt = new DataTable();
                var commonCont = new CommonController(_Common_IServices);
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData("105101150", Doc_no, Doc_dt);
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
            return null;
        }
        /*-------------- END 12-05-2025 10:47 ---------------*/
        public ActionResult CheckDuplicateBillNo(string supp_id, string Bill_No, string doc_id, string bill_dt)
        {

            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet SOItmList = _ServicePI_ISERVICE.CheckDuplicateBillNo(Comp_ID, Br_ID, supp_id, Bill_No, doc_id, bill_dt);
                DataRows = Json(JsonConvert.SerializeObject(SOItmList));
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            //return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        /*------------- Added by Nidhi 01-09-2025 ----------*/
        public string SavePdfDocToSendOnEmailAlert_Ext(string docid, string Doc_no, string Doc_dt, string fileName)
        {
            var commonCont = new CommonController(_Common_IServices);
            var data = GetPdfData(docid, Doc_no, Doc_dt);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        public ActionResult SendEmailAlert(string mail_id, string status, string docid, string Doc_no, string Doc_dt, string filepath)
        {
            try
            {
                string UserID = string.Empty;
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
                var commonCont = new CommonController(_Common_IServices);
                DataTable dt = new DataTable();
                string message = "";
                try
                {
                    if (filepath == "" || filepath == null)
                    {
                        string fileName = "SPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        filepath = SavePdfDocToSendOnEmailAlert_Ext(docid, Doc_no, Doc_dt, fileName);
                    }
                    message = commonCont.SendEmailAlert(CompID, BrchID, UserID, mail_id, status, docid, Doc_no, Doc_dt, filepath);
                }
                catch (Exception exMail)
                {
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                return Json(message);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        /*-------------- END 01-09-2025 ---------------*/
    }
}