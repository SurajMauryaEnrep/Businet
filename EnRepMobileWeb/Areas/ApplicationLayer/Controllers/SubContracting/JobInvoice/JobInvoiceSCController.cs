using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.JobInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.JobInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.Common;
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
using EnRepMobileWeb.Resources;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SubContracting.JobInvoice
{
    public class JobInvoiceSCController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105108123", title;
        List<JobInvoiceList> _JobInvoiceList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        JobInvoice_IServices _JobInvoice_IServices;
        public JobInvoiceSCController(Common_IServices _Common_IServices, JobInvoice_IServices _JobInvoice_IServices)
        {
            this._Common_IServices = _Common_IServices;
            this._JobInvoice_IServices = _JobInvoice_IServices;
        }
        // GET: ApplicationLayer/JobInvoiceSC
        public ActionResult JobInvoiceSC(string WF_Status)
        {
            try
            {
                ViewBag.DocID = DocumentMenuId;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                JI_ListModel _JI_ListModel = new JI_ListModel();
                if(WF_Status != null && WF_Status != "")
                {
                    _JI_ListModel.WF_Status = WF_Status;
                }
                DateTime dtnow = DateTime.Now;
                string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                GetAutoCompleteSearchSuppList(_JI_ListModel);
                CommonPageDetails();
                _JI_ListModel.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _JI_ListModel.StatusList = statusLists;
                if (TempData["ListFilterData"] != null)
                {
                    if (TempData["ListFilterData"].ToString() != "")
                    {
                        var PRData = TempData["ListFilterData"].ToString();
                        var a = PRData.Split(',');
                        _JI_ListModel.SuppID = a[0].Trim();
                        _JI_ListModel.FromDate = a[1].Trim();
                        _JI_ListModel.ToDate = a[2].Trim();
                        _JI_ListModel.Status = a[3].Trim();
                        if (_JI_ListModel.Status == "0")
                        {
                            _JI_ListModel.Status = null;
                        }
                        _JI_ListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    }
                }
                else
                {
                    _JI_ListModel.FromDate = startDate;
                }
                _JI_ListModel.JIList = getJINVList(_JI_ListModel);

                //Session["JINVSearch"] = "0";
                _JI_ListModel.JINVSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/SubContracting/JobInvoice/JobInvoiceList.cshtml", _JI_ListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult AddJobInvoiceSCDetail()
        {

            JobInvoiceModel _JobInvoiceModel = new JobInvoiceModel();
            _JobInvoiceModel.Message = "New";
            _JobInvoiceModel.Command = "Add";
            _JobInvoiceModel.AppStatus = "D";
            ViewBag.DocumentStatus = _JobInvoiceModel.AppStatus;
            _JobInvoiceModel.TransType = "Save";
            _JobInvoiceModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _JobInvoiceModel;
            CommonPageDetails();
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("JobInvoiceSC");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("JobInvoiceSCDetail", "JobInvoiceSC");
        }
        public ActionResult JobInvoiceSCDetail(JobInvoiceModel _JobInvoiceModel1, string JICodeURL, string JIDate, string TransType, string BtnName, string command,string WF_Status1) 
        {
            try
            {
                ViewBag.DocID = DocumentMenuId;
                CommonPageDetails();
                /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, JIDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _JobInvoiceModel = TempData["ModelData"] as JobInvoiceModel;
                if (_JobInvoiceModel != null)
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
                    ViewBag.DocumentMenuId = DocumentMenuId;
                   
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));

                    _JobInvoiceModel.Title = title;
                    _JobInvoiceModel.ValDigit = ValDigit;
                    _JobInvoiceModel.QtyDigit = QtyDigit;
                    _JobInvoiceModel.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;

                    ViewBag.DocumentMenuId = DocumentMenuId;

                    CommonPageDetails();
                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _JobInvoiceModel.SupplierNameList = suppLists;

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.GrnNoId = "---Select---";
                    _DocumentNumber.GrnnoVal = "0";
                    //_DocumentNumber.GrnNoId = "0";
                    //_DocumentNumber.GrnnoVal = "---Select---";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _JobInvoiceModel.GRNNumberList = _DocumentNumberList;

                    List<CurrancyList> currancyLists = new List<CurrancyList>();
                    currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    _JobInvoiceModel.currancyLists = currancyLists;
                    DataTable dtbscurr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                    if (dtbscurr.Rows.Count > 0)
                    {
                        _JobInvoiceModel.Curr_Id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                        ViewBag.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                    }

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _JobInvoiceModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    if (_JobInvoiceModel.TransType == "Update" || _JobInvoiceModel.Command == "Edit")

                    {
                        string JISC_NO = _JobInvoiceModel.JInv_No;
                        string JISC_Date = _JobInvoiceModel.JInv_Dt;
                        string VouType = "PV";
                        DataSet ds = _JobInvoice_IServices.GetJobInvDetailEditUpdate(CompID, BrchID, JISC_NO, JISC_Date, UserID, DocumentMenuId, VouType);

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            _JobInvoiceModel.JInv_No = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _JobInvoiceModel.DocNoAttach = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _JobInvoiceModel.JInv_Dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                            _JobInvoiceModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _JobInvoiceModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _JobInvoiceModel.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                            suppLists.Add(new SupplierName { supp_id = _JobInvoiceModel.SuppID, supp_name = _JobInvoiceModel.SuppName });
                            _JobInvoiceModel.SupplierNameList = suppLists;
                            _JobInvoiceModel.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bill_add_id"].ToString());
                            _JobInvoiceModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _JobInvoiceModel.GRNNumber = ds.Tables[0].Rows[0]["grn_no"].ToString();
                            _JobInvoiceModel.GRN_Number = ds.Tables[0].Rows[0]["grn_no"].ToString();
                            _DocumentNumberList.Add(new DocumentNumber { GrnNoId = _JobInvoiceModel.GRNNumber, GrnnoVal = _JobInvoiceModel.GRNNumber });
                            _JobInvoiceModel.GRNNumberList = _DocumentNumberList;
                            if (ds.Tables[0].Rows[0]["grn_dt"] != null && ds.Tables[0].Rows[0]["grn_dt"].ToString() != "")
                            {
                                _JobInvoiceModel.GRNDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["grn_dt"]).ToString("yyyy-MM-dd");
                            }
                            _JobInvoiceModel.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _JobInvoiceModel.Bill_Dt = ds.Tables[0].Rows[0]["BillDate"].ToString();
                            string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _JobInvoiceModel.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            currancyLists.Add(new CurrancyList { curr_id = _JobInvoiceModel.Currency, curr_name = Curr_name });
                            _JobInvoiceModel.currancyLists = currancyLists;
                          _JobInvoiceModel.ExRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(QtyDigit);
                            _JobInvoiceModel.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _JobInvoiceModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _JobInvoiceModel.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _JobInvoiceModel.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(ValDigit);
                            _JobInvoiceModel.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _JobInvoiceModel.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val"]).ToString(ValDigit);
                            _JobInvoiceModel.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                            //_JobInvoiceModel.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);

                            _JobInvoiceModel.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            {
                                _JobInvoiceModel.Hd_GstType = "Both";
                            }
                            else
                            {
                                _JobInvoiceModel.Hd_GstType = "IGST";
                            }
                            _JobInvoiceModel.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _JobInvoiceModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _JobInvoiceModel.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _JobInvoiceModel.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _JobInvoiceModel.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _JobInvoiceModel.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _JobInvoiceModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _JobInvoiceModel.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                            _JobInvoiceModel.Inv_Status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                            _JobInvoiceModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _JobInvoiceModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);//Cancelled
                            
                            ViewBag.TotalDetails = ds.Tables[0];
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.OCTaxDetails = ds.Tables[4];
                            ViewBag.ItemTaxDetailsList = ds.Tables[5];
                            ViewBag.AttechmentDetails = ds.Tables[9];
                            ViewBag.GLAccount = ds.Tables[10];
                            ViewBag.GLTOtal = ds.Tables[11];
                            ViewBag.CostCenterData = ds.Tables[12];
                            ViewBag.ItemTDSDetails = ds.Tables[13];
                            ViewBag.ItemOC_TDSDetails = ds.Tables[14];

                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (ds.Tables[10].Rows.Count > 0)
                        {
                            if (Statuscode == "A" || Statuscode == "C")
                            {
                                _JobInvoiceModel.GLVoucherType = ds.Tables[10].Rows[0]["vou_type"].ToString();
                            }
                            _JobInvoiceModel.GLVoucherNo = ds.Tables[10].Rows[0]["vou_no"].ToString();
                            _JobInvoiceModel.GLVoucherDt = ds.Tables[10].Rows[0]["vou_dt"].ToString();
                        }
                        if (Statuscode == "C")
                        {
                            _JobInvoiceModel.Cancelled = true;
                        }
                        else
                        {
                            _JobInvoiceModel.Cancelled = false;
                        }

                        _JobInvoiceModel.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }

                        if (ViewBag.AppLevel != null && _JobInvoiceModel.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[6].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[6].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[7].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _JobInvoiceModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _JobInvoiceModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _JobInvoiceModel.BtnName = "BtnToDetailPage";

                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _JobInvoiceModel.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _JobInvoiceModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _JobInvoiceModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _JobInvoiceModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _JobInvoiceModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _JobInvoiceModel.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }



                    //_JobInvoiceModel.BtnName = "BtnAddNew";
                    if (_JobInvoiceModel.BtnName != null)
                    {
                        _JobInvoiceModel.BtnName = _JobInvoiceModel.BtnName;
                    }
                    _JobInvoiceModel.TransType = _JobInvoiceModel.TransType;
                    ViewBag.TransType = _JobInvoiceModel.TransType;


                    if (_JobInvoiceModel.DocumentStatus == null)
                    {
                        _JobInvoiceModel.DocumentStatus = "D";
                        ViewBag.DocumentCode = "D";
                        ViewBag.Command = _JobInvoiceModel.Command;
                    }
                    else
                    {
                        _JobInvoiceModel.DocumentStatus = _JobInvoiceModel.DocumentStatus;
                        ViewBag.DocumentCode = _JobInvoiceModel.DocumentStatus;
                        ViewBag.Command = _JobInvoiceModel.Command;
                    }
                    ViewBag.DocumentStatus = _JobInvoiceModel.DocumentStatus;
                    ViewBag.DocumentCode = _JobInvoiceModel.DocumentStatus;



                    return View("~/Areas/ApplicationLayer/Views/SubContracting/JobInvoice/JobInvoiceDetail.cshtml", _JobInvoiceModel);
                }
                else
                {/*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
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
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _JobInvoiceModel1.Title = title;
                    string ValDigit1 = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit1 = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit1 = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _JobInvoiceModel1.ValDigit = ValDigit1;
                    _JobInvoiceModel1.QtyDigit = QtyDigit1;
                    _JobInvoiceModel1.RateDigit = RateDigit1;
                    ViewBag.ValDigit = ValDigit1;
                    ViewBag.QtyDigit = QtyDigit1;
                    ViewBag.RateDigit = RateDigit1;
                    ViewBag.DocumentStatus = "D";
                    if (WF_Status1 != null && WF_Status1 != "")
                    {
                        _JobInvoiceModel1.WF_Status1 = WF_Status1;
                    }
                    List<SupplierName> suppLists1 = new List<SupplierName>();
                    suppLists1.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _JobInvoiceModel1.SupplierNameList = suppLists1;

                    List<DocumentNumber> _DocumentNumberList1 = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber1 = new DocumentNumber();
                    _DocumentNumber1.GrnNoId = "---Select---";
                    _DocumentNumber1.GrnnoVal = "0";
                    _DocumentNumberList1.Add(_DocumentNumber1);
                    _JobInvoiceModel1.GRNNumberList = _DocumentNumberList1;

                    List<CurrancyList> currancyLists1 = new List<CurrancyList>();
                    currancyLists1.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    _JobInvoiceModel1.currancyLists = currancyLists1;
                    DataTable dtbscurr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                    if (dtbscurr.Rows.Count > 0)
                    {
                        _JobInvoiceModel1.Curr_Id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                        ViewBag.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                    }
                    if (_JobInvoiceModel1.TransType == "Update" || _JobInvoiceModel1.Command == "Edit")

                    {
                        
                        string JISC_NO = JICodeURL;
                        string JISC_Date = JIDate;
                        string VouType = "PV";
                        DataSet ds = _JobInvoice_IServices.GetJobInvDetailEditUpdate(CompID, BrchID, JISC_NO, JISC_Date, UserID, DocumentMenuId, VouType);

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            _JobInvoiceModel1.JInv_No = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _JobInvoiceModel1.DocNoAttach = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _JobInvoiceModel1.JInv_Dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                            _JobInvoiceModel1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _JobInvoiceModel1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _JobInvoiceModel1.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                            suppLists1.Add(new SupplierName { supp_id = _JobInvoiceModel1.SuppID, supp_name = _JobInvoiceModel1.SuppName });
                            _JobInvoiceModel1.SupplierNameList = suppLists1;
                            _JobInvoiceModel1.bill_add_id = Convert.ToInt32(ds.Tables[0].Rows[0]["bill_add_id"].ToString());
                            _JobInvoiceModel1.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                            _JobInvoiceModel1.GRNNumber = ds.Tables[0].Rows[0]["grn_no"].ToString();
                            _JobInvoiceModel1.GRN_Number = ds.Tables[0].Rows[0]["grn_no"].ToString();
                            _DocumentNumberList1.Add(new DocumentNumber { GrnNoId = _JobInvoiceModel1.GRNNumber, GrnnoVal = _JobInvoiceModel1.GRNNumber });
                            _JobInvoiceModel1.GRNNumberList = _DocumentNumberList1;
                            if (ds.Tables[0].Rows[0]["grn_dt"] != null && ds.Tables[0].Rows[0]["grn_dt"].ToString() != "")
                            {
                                _JobInvoiceModel1.GRNDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["grn_dt"]).ToString("yyyy-MM-dd");
                            }
                            _JobInvoiceModel1.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _JobInvoiceModel1.Bill_Dt = ds.Tables[0].Rows[0]["BillDate"].ToString();
                            string Curr_name1 = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _JobInvoiceModel1.Currency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            currancyLists1.Add(new CurrancyList { curr_id = _JobInvoiceModel1.Currency, curr_name = Curr_name1 });
                            _JobInvoiceModel1.currancyLists = currancyLists1;
                            _JobInvoiceModel1.ExRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(QtyDigit1);

                            _JobInvoiceModel1.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                            _JobInvoiceModel1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _JobInvoiceModel1.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit1);
                            _JobInvoiceModel1.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(ValDigit1);
                            _JobInvoiceModel1.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit1);
                            _JobInvoiceModel1.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val"]).ToString(ValDigit1);
                            _JobInvoiceModel1.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                            //_JobInvoiceModel.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);

                            _JobInvoiceModel1.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            {
                                _JobInvoiceModel1.Hd_GstType = "Both";
                            }
                            else
                            {
                                _JobInvoiceModel1.Hd_GstType = "IGST";
                            }
                            _JobInvoiceModel1.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _JobInvoiceModel1.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _JobInvoiceModel1.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _JobInvoiceModel1.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _JobInvoiceModel1.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _JobInvoiceModel1.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _JobInvoiceModel1.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _JobInvoiceModel1.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                            _JobInvoiceModel1.Inv_Status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                            _JobInvoiceModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _JobInvoiceModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);//Cancelled

                            ViewBag.TotalDetails = ds.Tables[0];
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.OCTaxDetails = ds.Tables[4];
                            ViewBag.ItemTaxDetailsList = ds.Tables[5];
                            ViewBag.AttechmentDetails = ds.Tables[9];
                            ViewBag.GLAccount = ds.Tables[10];
                            ViewBag.GLTOtal = ds.Tables[11];
                            ViewBag.CostCenterData = ds.Tables[12];
                            ViewBag.ItemTDSDetails = ds.Tables[13];
                            ViewBag.ItemOC_TDSDetails = ds.Tables[14];
                            ViewBag.QtyDigit1 = QtyDigit1;
                        }
                        var create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (ds.Tables[10].Rows.Count > 0)
                        {
                            if (Statuscode == "A" || Statuscode == "C")
                            {
                                _JobInvoiceModel1.GLVoucherType = ds.Tables[10].Rows[0]["vou_type"].ToString();
                            }
                            _JobInvoiceModel1.GLVoucherNo = ds.Tables[10].Rows[0]["vou_no"].ToString();
                            _JobInvoiceModel1.GLVoucherDt = ds.Tables[10].Rows[0]["vou_dt"].ToString();
                        }
                        if (Statuscode == "C")
                        {
                            _JobInvoiceModel1.Cancelled = true;
                        }
                        else
                        {
                            _JobInvoiceModel1.Cancelled = false;
                        }

                        _JobInvoiceModel1.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }

                        if (ViewBag.AppLevel != null && _JobInvoiceModel1.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[6].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[6].Rows[0]["sent_to"].ToString();
                            }

                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[7].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _JobInvoiceModel1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _JobInvoiceModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _JobInvoiceModel1.BtnName = "BtnToDetailPage";

                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _JobInvoiceModel1.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _JobInvoiceModel1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _JobInvoiceModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _JobInvoiceModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _JobInvoiceModel1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _JobInvoiceModel1.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }



                    var JICode = "";
                    if (JICodeURL != null)
                    {
                        JICode = JICodeURL;
                        _JobInvoiceModel1.GRNNumber = JICodeURL;
                    }
                    else
                    {
                        JICode = _JobInvoiceModel1.GRNNumber;
                    }
                    if (TransType != null)
                    {
                        _JobInvoiceModel1.TransType = TransType;
                        ViewBag.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _JobInvoiceModel1.Command = command;
                        ViewBag.Command = command;
                    }
                    if (TransType == "Save" && command == "Save")
                    {
                        _JobInvoiceModel1.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _JobInvoiceModel1.DocumentStatus;

                    }
                    if (_JobInvoiceModel1.BtnName == null && _JobInvoiceModel1.Command == null)
                    {
                        _JobInvoiceModel1.BtnName = "AddNew";
                        _JobInvoiceModel1.Command = "Add";
                        _JobInvoiceModel1.AppStatus = "D";
                        ViewBag.DocumentStatus = _JobInvoiceModel1.AppStatus;
                        _JobInvoiceModel1.DocumentStatus = "D";
                        _JobInvoiceModel1.TransType = "Save";
                        _JobInvoiceModel1.BtnName = "BtnAddNew";

                    }

                    if (_JobInvoiceModel1.BtnName != null)
                    {
                        _JobInvoiceModel1.BtnName = _JobInvoiceModel1.BtnName;
                    }
                    _JobInvoiceModel1.TransType = _JobInvoiceModel1.TransType;
                    ViewBag.TransType = _JobInvoiceModel1.TransType;
                    if (_JobInvoiceModel1.DocumentStatus != null)
                    {
                        _JobInvoiceModel1.DocumentStatus = _JobInvoiceModel1.DocumentStatus;
                        ViewBag.DocumentStatus = _JobInvoiceModel1.DocumentStatus;
                        ViewBag.DocumentCode = _JobInvoiceModel1.DocumentStatus;
                    }
                    return View("~/Areas/ApplicationLayer/Views/SubContracting/JobInvoice/JobInvoiceDetail.cshtml", _JobInvoiceModel1);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobInvoiceBtnCommand(JobInvoiceModel _JobInvoiceModel, string command)
        {
            try
            {
                /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_JobInvoiceModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        //_JobInvoiceModel = new JobInvoiceModel();
                        //_JobInvoiceModel.Message = "New";
                        //_JobInvoiceModel.Command = "Add";
                        //_JobInvoiceModel.AppStatus = "D";
                        //_JobInvoiceModel.DocumentStatus = "D";
                        //ViewBag.DocumentStatus = _JobInvoiceModel.DocumentStatus;
                        //_JobInvoiceModel.TransType = "Save";
                        //_JobInvoiceModel.BtnName = "BtnAddNew";
                        //TempData["ModelData"] = _JobInvoiceModel;
                        //TempData["ListFilterData"] = null;
                        JobInvoiceModel _JobInvoiceModelAdd = new JobInvoiceModel();
                        _JobInvoiceModelAdd.Message = "New";
                        _JobInvoiceModelAdd.Command = "Add";
                        _JobInvoiceModelAdd.AppStatus = "D";
                        _JobInvoiceModelAdd.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _JobInvoiceModel.DocumentStatus;
                        _JobInvoiceModelAdd.TransType = "Save";
                        _JobInvoiceModelAdd.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _JobInvoiceModelAdd;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_JobInvoiceModel.JInv_No))
                                return RedirectToAction("DoubleClickFromList", new { DocNo = _JobInvoiceModel.JInv_No, DocDate = _JobInvoiceModel.JInv_Dt, ListFilterData = _JobInvoiceModel.ListFilterData1, WF_status = _JobInvoiceModel.WFStatus });
                            else
                                _JobInvoiceModelAdd.Command = "Refresh";
                            _JobInvoiceModelAdd.TransType = "Refresh";
                            _JobInvoiceModelAdd.BtnName = "Refresh";
                            _JobInvoiceModelAdd.DocumentStatus = null;
                            TempData["ModelData"] = _JobInvoiceModelAdd;
                            return RedirectToAction("JobInvoiceSCDetail", "JobInvoiceSC");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("JobInvoiceSCDetail", "JobInvoiceSC");

                    case "Edit":
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _JobInvoiceModel.JInv_No, DocDate = _JobInvoiceModel.JInv_Dt, ListFilterData = _JobInvoiceModel.ListFilterData1, WF_status = _JobInvoiceModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string invDt = _JobInvoiceModel.JInv_Dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _JobInvoiceModel.JInv_No, DocDate = _JobInvoiceModel.JInv_Dt, ListFilterData = _JobInvoiceModel.ListFilterData1, WF_status = _JobInvoiceModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/

                        if (_JobInvoiceModel.Inv_Status == "A")/*Add BY Hina on 17-08-2024*/
                        {
                            string checkforCancle = CheckJICancellationAgainstVoucher(_JobInvoiceModel.JInv_No, _JobInvoiceModel.JInv_Dt);
                            if (checkforCancle == "PaymentCreated")
                            {
                                _JobInvoiceModel.Message = "PaymentCreated";
                                _JobInvoiceModel.TransType = "Update";
                                _JobInvoiceModel.Command = "Used";
                                _JobInvoiceModel.BtnName = "BtnToDetailPage";
                            }
                            else
                            {
                                _JobInvoiceModel.Command = command; 
                                _JobInvoiceModel.BtnName = "BtnEdit";
                                _JobInvoiceModel.Message = "New";
                            }

                        }
                        else
                        {
                            _JobInvoiceModel.TransType = "Update";
                            _JobInvoiceModel.Command = command;
                            _JobInvoiceModel.BtnName = "BtnEdit";
                            _JobInvoiceModel.Message = "New";
                            _JobInvoiceModel.AppStatus = "D";
                            _JobInvoiceModel.DocumentStatus = "D";
                            ViewBag.DocumentStatus = _JobInvoiceModel.AppStatus;

                            command = _JobInvoiceModel.Command;
                            TempData["ModelData"] = _JobInvoiceModel;
                            TempData["ListFilterData"] = _JobInvoiceModel.ListFilterData1;
                        }

                        _JobInvoiceModel.TransType = "Update";
                        _JobInvoiceModel.Command = _JobInvoiceModel.Command;
                        //_JobInvoiceModel.Command = command; /*commented by Hina on 17-08-2024*/
                        //_JobInvoiceModel.BtnName = "BtnEdit";
                        //_JobInvoiceModel.Message = "New";
                        _JobInvoiceModel.AppStatus = "D";
                        _JobInvoiceModel.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _JobInvoiceModel.AppStatus;
                        var TransType = "Update";
                        var BtnName = "BtnEdit";
                        var JICodeURL = _JobInvoiceModel.JInv_No;
                        var JIDate = _JobInvoiceModel.JInv_Dt;
                        command = _JobInvoiceModel.Command;
                        TempData["ModelData"] = _JobInvoiceModel;
                        TempData["ListFilterData"] = _JobInvoiceModel.ListFilterData1;
                        return (RedirectToAction("JobInvoiceSCDetail", new { JICodeURL = JICodeURL, JIDate, TransType, BtnName, command }));

                    case "Delete":
                        _JobInvoiceModel.Command = command;
                        _JobInvoiceModel.BtnName = "Refresh";
                        DeleteJInvDetails(_JobInvoiceModel, command);
                        TempData["ListFilterData"] = _JobInvoiceModel.ListFilterData1;
                        return RedirectToAction("JobInvoiceSCDetail");

                    case "Save":
                        _JobInvoiceModel.Command = command;
                        string checkforCancle_onSave = CheckJICancellationAgainstVoucher(_JobInvoiceModel.JInv_No, _JobInvoiceModel.JInv_Dt);
                        if (checkforCancle_onSave != "")
                        {
                            _JobInvoiceModel.Message = checkforCancle_onSave;
                            _JobInvoiceModel.TransType = "Update";
                            _JobInvoiceModel.Command = "Used";
                            _JobInvoiceModel.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            SaveJobInvoiceDetail(_JobInvoiceModel);
                        }
                        
                        if (_JobInvoiceModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_JobInvoiceModel.Message == "DocModify")
                        {
                            DocumentMenuId = _JobInvoiceModel.DocumentMenuId;
                            CommonPageDetails();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";

                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = _JobInvoiceModel.SuppID, supp_name = _JobInvoiceModel.SuppName });
                            _JobInvoiceModel.SupplierNameList = suppLists;

                            List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                            DocumentNumber _DocumentNumber = new DocumentNumber();
                            _DocumentNumber.GrnNoId = _JobInvoiceModel.GRN_Number;
                            _DocumentNumber.GrnnoVal = _JobInvoiceModel.GRN_Number;
                            _DocumentNumberList.Add(_DocumentNumber);
                            _JobInvoiceModel.GRNNumberList = _DocumentNumberList;

                            List<CurrancyList> currancyLists = new List<CurrancyList>();
                            currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                            _JobInvoiceModel.currancyLists = currancyLists;
                            _JobInvoiceModel.Title = title;

                             _JobInvoiceModel.JInv_Dt = DateTime.Now.ToString();
                            _JobInvoiceModel.Bill_No = _JobInvoiceModel.Bill_No;
                            _JobInvoiceModel.Bill_Dt = _JobInvoiceModel.Bill_Dt;
                            _JobInvoiceModel.SuppName = _JobInvoiceModel.SuppName;
                            _JobInvoiceModel.Address = _JobInvoiceModel.Address;
                            _JobInvoiceModel.GRN_Number = _JobInvoiceModel.GRN_Number;
                            _JobInvoiceModel.GRNDate = _JobInvoiceModel.GRNDate;

                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetailsList = ViewData["TaxDetails"];
                            ViewBag.OCTaxDetails = ViewData["OCTaxDetails"];
                            ViewBag.GLAccount = ViewData["VouDetail"];
                            ViewBag.ItemTDSDetails = ViewData["TDSDetails"];
                            ViewBag.CostCenterData = ViewData["CCdetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _JobInvoiceModel.BtnName = "Refresh";
                            _JobInvoiceModel.Command = "Refresh";
                            _JobInvoiceModel.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/SubContracting/JobInvoice/JobInvoiceDetail.cshtml", _JobInvoiceModel);

                        }
                        else
                        {
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            JICodeURL = _JobInvoiceModel.JInv_No;
                            JIDate = _JobInvoiceModel.JInv_Dt;
                            TransType = _JobInvoiceModel.TransType;
                            BtnName = _JobInvoiceModel.BtnName;
                            TempData["ModelData"] = _JobInvoiceModel;
                            TempData["ListFilterData"] = _JobInvoiceModel.ListFilterData1;
                            return (RedirectToAction("JobInvoiceSCDetail", new { JICodeURL = JICodeURL, JIDate, TransType, BtnName, command }));

                        }


                    case "Forward":
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _JobInvoiceModel.JInv_No, DocDate = _JobInvoiceModel.JInv_Dt, ListFilterData = _JobInvoiceModel.ListFilterData1, WF_status = _JobInvoiceModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string invDt1 = _JobInvoiceModel.JInv_Dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _JobInvoiceModel.JInv_No, DocDate = _JobInvoiceModel.JInv_Dt, ListFilterData = _JobInvoiceModel.ListFilterData1, WF_status = _JobInvoiceModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _JobInvoiceModel.JInv_No, DocDate = _JobInvoiceModel.JInv_Dt, ListFilterData = _JobInvoiceModel.ListFilterData1, WF_status = _JobInvoiceModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string invDt2 = _JobInvoiceModel.JInv_Dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _JobInvoiceModel.JInv_No, DocDate = _JobInvoiceModel.JInv_Dt, ListFilterData = _JobInvoiceModel.ListFilterData1, WF_status = _JobInvoiceModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _JobInvoiceModel.Command = command;
                        JIApprove(_JobInvoiceModel, "","");

                        JICodeURL = _JobInvoiceModel.JInv_No;
                        JIDate = _JobInvoiceModel.JInv_Dt;
                        TransType = _JobInvoiceModel.TransType;
                        BtnName = _JobInvoiceModel.BtnName;
                        TempData["ModelData"] = _JobInvoiceModel;
                        TempData["ListFilterData"] = _JobInvoiceModel.ListFilterData1;
                        return (RedirectToAction("JobInvoiceSCDetail", new { JICodeURL = JICodeURL, JIDate, TransType, BtnName, command }));



                    case "Refresh":
                        JobInvoiceModel _JobInvoiceModelRefresh = new JobInvoiceModel();
                        //_JobInvoiceModel = new DeliveryNoteDetailSC_Model();
                        _JobInvoiceModel.Message = null;
                        //_JobInvoiceModel.Command = command;
                        //_JobInvoiceModel.TransType = "Refresh";
                        //_JobInvoiceModel.BtnName = "Refresh";
                        //_JobInvoiceModel.DocumentStatus = null;
                        _JobInvoiceModelRefresh.Command = command;
                        _JobInvoiceModelRefresh.TransType = "Refresh";
                        _JobInvoiceModelRefresh.BtnName = "Refresh";
                        _JobInvoiceModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = _JobInvoiceModelRefresh;
                        TempData["ListFilterData"] = _JobInvoiceModel.ListFilterData1;
                        return RedirectToAction("JobInvoiceSCDetail");

                    case "Print":
                    return GenratePdfFile(_JobInvoiceModel);
                    case "BacktoList":
                        var WF_Status = _JobInvoiceModel.WF_Status1;
                        TempData["ListFilterData"] = _JobInvoiceModel.ListFilterData1;
                        return RedirectToAction("JobInvoiceSC", "JobInvoiceSC",new { WF_Status });

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

        public ActionResult SaveJobInvoiceDetail(JobInvoiceModel _JobInvoiceModel)
        {
            string SaveMessage = "";
            /*getDocumentName();*/ /* To set Title*/
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_JobInvoiceModel.Cancelled == false)
                {
                    _JobInvoiceModel.DocumentMenuId = _JobInvoiceModel.DocumentMenuId;


                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BrchID = Session["BranchId"].ToString();
                    }
                    if (Session["Userid"] != null)
                    {
                        UserID = Session["Userid"].ToString();
                    }

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

                    DtblHDetail = ToDtblHDetail(_JobInvoiceModel);
                    DtblItemDetail = ToDtblItemDetail(_JobInvoiceModel.ItemDetails);
                    DtblTaxDetail = ToDtblTaxDetail(_JobInvoiceModel.TaxDetail, "Y");
                    ViewData["TaxDetails"] = ViewData["JITaxDetails"];
                    DtblOCTaxDetail = ToDtblTaxDetail(_JobInvoiceModel.OC_TaxDetail,"N");
                    ViewData["OCTaxDetails"] = ViewData["JITaxDetails"];
                    DtblTdsDetail = ToDtblTdsDetail(_JobInvoiceModel.tds_details,"");
                    DtblOcTdsDetail = ToDtblTdsDetail(_JobInvoiceModel.oc_tds_details, "OC");

                    DataTable OC_detail = new DataTable();
                    //OC_detail.Columns.Add("oc_id", typeof(string));
                    //OC_detail.Columns.Add("oc_val", typeof(string));
                    //OC_detail.Columns.Add("tax_amt", typeof(string));
                    //OC_detail.Columns.Add("total_amt", typeof(string));

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

                    if (_JobInvoiceModel.OCDetail != null)
                    {
                        JArray jObjectOC = JArray.Parse(_JobInvoiceModel.OCDetail);
                        for (int i = 0; i < jObjectOC.Count; i++)
                        {
                            DataRow dtrowOCDetailsLines = OC_detail.NewRow();
                            //dtrowOCDetailsLines["oc_id"] = jObjectOC[i]["oc_id"].ToString();
                            //dtrowOCDetailsLines["oc_val"] = jObjectOC[i]["oc_val"].ToString();
                            //dtrowOCDetailsLines["tax_amt"] = jObjectOC[i]["tax_amt"].ToString();
                            //dtrowOCDetailsLines["total_amt"] = jObjectOC[i]["total_amt"].ToString();

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
                            dtrowOCDetailsLines["tds_amt"] = jObjectOC[i]["tds_amt"].ToString() == "" ? "0" : jObjectOC[i]["tds_amt"].ToString();
                            dtrowOCDetailsLines["roundoff"] = jObjectOC[i]["round_off"].ToString();
                            dtrowOCDetailsLines["pm_flag"] = jObjectOC[i]["pm_flag"].ToString();
                            OC_detail.Rows.Add(dtrowOCDetailsLines);
                        }
                        ViewData["OCDetails"] = dtOCdetail(jObjectOC);
                    }
                    DtblIOCDetail = OC_detail;

                    var _JobOrderDetailsattch = TempData["ModelDataattch"] as JIDetailsattch;
                    TempData["ModelDataattch"] = null;
                    DataTable dtAttachment = new DataTable();
                    if (_JobInvoiceModel.attatchmentdetail != null)
                    {
                        if (_JobOrderDetailsattch != null)
                        {
                            if (_JobOrderDetailsattch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _JobOrderDetailsattch.AttachMentDetailItmStp as DataTable;
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
                            if (_JobInvoiceModel.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _JobInvoiceModel.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(_JobInvoiceModel.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(_JobInvoiceModel.JInv_No))
                                {
                                    dtrowAttachment1["id"] = _JobInvoiceModel.JInv_No;
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

                        if (_JobInvoiceModel.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_JobInvoiceModel.JInv_No))
                                {
                                    ItmCode = _JobInvoiceModel.JInv_No;
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
                        DtblAttchDetail = dtAttachment;
                    }

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
                    if (_JobInvoiceModel.vouDetail != null)
                    {

                        JArray jObjectVOU = JArray.Parse(_JobInvoiceModel.vouDetail);
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

                    /**----------------Cost Center Section--------------------*/
                    DataTable CC_Details = new DataTable();

                    CC_Details.Columns.Add("vou_sr_no", typeof(string));
                    CC_Details.Columns.Add("gl_sr_no", typeof(string));
                    CC_Details.Columns.Add("acc_id", typeof(string));
                    CC_Details.Columns.Add("cc_id", typeof(int));
                    CC_Details.Columns.Add("cc_val_id", typeof(int));
                    CC_Details.Columns.Add("cc_amt", typeof(string));

                    JArray JAObj = JArray.Parse(_JobInvoiceModel.CC_DetailList);
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
                    CRCostCenterDetails = CC_Details;
                    ViewData["CCdetail"] = dtCCdetail(JAObj);
                    /**----------------Cost Center Section END--------------------*/

                    SaveMessage = _JobInvoice_IServices.InsertJI_Details(DtblHDetail, DtblItemDetail, DtblTaxDetail
                        , DtblOCTaxDetail, DtblIOCDetail, DtblAttchDetail, DtblVouDetail, CRCostCenterDetails, DtblTdsDetail, DtblOcTdsDetail
                    , _JobInvoiceModel.TDS_Amount);
                    if (SaveMessage == "DocModify")
                    {
                        _JobInvoiceModel.Message = "DocModify";
                        _JobInvoiceModel.BtnName = "Refresh";
                        _JobInvoiceModel.Command = "Refresh";
                        _JobInvoiceModel.DocumentMenuId = DocumentMenuId;
                        TempData["ModelData"] = _JobInvoiceModel;
                        return RedirectToAction("GoodReceiptNoteSCDetail");
                    }
                    else
                    {
                        string[] Data = SaveMessage.Split(',');

                        string JINo = Data[0];
                        string StatusCode = Data[1];
                        string JI_No = JINo.Replace("/", "");
                        if (JI_No == "Data_Not_Found")
                        {
                            var a = StatusCode.Split('-');
                            var msg = JI_No.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _JobInvoiceModel.Message = JI_No.Replace("_", "");
                            return RedirectToAction("DeliveryNoteSCDetail");
                        }
                        string Message = Data[5];
                        string JI_Date = Data[6];
                        string Message1 = Data[5];

                        /*-----------------Attachment Section Start------------------------*/
                        if (Message == "Save")
                        {

                            string Guid = "";
                            if (_JobOrderDetailsattch != null)
                            {
                                if (_JobOrderDetailsattch.Guid != null)
                                {
                                    Guid = _JobOrderDetailsattch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, JI_No, _JobInvoiceModel.TransType, dtAttachment);

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
                            //                string JI_No1 = JI_No.Replace("/", "");
                            //                string img_nm = CompID + BrchID + JI_No1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                        {
                            _JobInvoiceModel.Message = "Save";

                            _JobInvoiceModel.JInv_No = JINo;
                            _JobInvoiceModel.JInv_Dt = JI_Date;
                            _JobInvoiceModel.TransType = "Update";
                            _JobInvoiceModel.Command = "Update";
                            _JobInvoiceModel.AppStatus = "D";
                            _JobInvoiceModel.DocumentStatus = "D";

                            _JobInvoiceModel.BtnName = "BtnSave";
                            TempData["ModelData"] = _JobInvoiceModel;
                            return RedirectToAction("JobInvoiceSCDetail");

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
                    _JobInvoiceModel.CreatedBy = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    string Nurr = _JobInvoiceModel.Nurration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                    DataSet SaveMessage1 = _JobInvoice_IServices.JobInvCancel(_JobInvoiceModel, CompID, br_id, mac_id, Nurr);

                    _JobInvoiceModel.Message = "Cancelled";
                    _JobInvoiceModel.Command = "Update";
                    _JobInvoiceModel.JInv_No = _JobInvoiceModel.JInv_No;
                    _JobInvoiceModel.JInv_Dt = _JobInvoiceModel.JInv_Dt;
                    _JobInvoiceModel.TransType = "Update";
                    _JobInvoiceModel.AppStatus = "D";
                    _JobInvoiceModel.BtnName = "Refresh";
                    TempData["ModelData"] = _JobInvoiceModel;
                    return RedirectToAction("JobInvoiceSCDetail");
                }
                return RedirectToAction("JobInvoiceSCDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_JobInvoiceModel.TransType == "Save")
                    {
                        string Guid = "";
                        if (_JobInvoiceModel.Guid != null)
                        {
                            Guid = _JobInvoiceModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }

        private DataTable ToDtblHDetail(JobInvoiceModel _JobInvoiceModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataTable DtblHDetail = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("Inv_No", typeof(string));
                dtheader.Columns.Add("Inv_Date", typeof(string));
                dtheader.Columns.Add("Supp_ID", typeof(int));
                dtheader.Columns.Add("Bill_add_id", typeof(string));
                //dtheader.Columns.Add("Grn_No", typeof(string));
                //dtheader.Columns.Add("Grn_Date", typeof(string));
                dtheader.Columns.Add("Bill_No", typeof(string));
                dtheader.Columns.Add("Bill_Date", typeof(string));
                dtheader.Columns.Add("Curr_Id", typeof(string));
                dtheader.Columns.Add("Conv_Rate", typeof(string));
                dtheader.Columns.Add("Inv_Status", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("tax_amt_nrecov", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val", typeof(string));
                //dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("Narration", typeof(string));
                
                dtheader.Columns.Add("Comp_ID", typeof(string));
                dtheader.Columns.Add("Branch_ID", typeof(string));
                dtheader.Columns.Add("User_ID", typeof(int));
                dtheader.Columns.Add("Mac_ID", typeof(string));
               


                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _JobInvoiceModel.TransType;
                //dtrowHeader["Cancelled"] = ConvertBoolToStrint(_JobInvoiceModel.Cancelled);
                dtrowHeader["Inv_No"] = _JobInvoiceModel.JInv_No;
                dtrowHeader["Inv_Date"] = _JobInvoiceModel.JInv_Dt;
                dtrowHeader["Supp_ID"] = _JobInvoiceModel.SuppID;
                dtrowHeader["Bill_add_id"] = _JobInvoiceModel.bill_add_id;
                //dtrowHeader["GRN_No"] = _JobInvoiceModel.GRN_Number;
                //dtrowHeader["GRN_Date"] = _JobInvoiceModel.GRNDate;
                dtrowHeader["Bill_No"] = _JobInvoiceModel.Bill_No;
                dtrowHeader["Bill_Date"] = _JobInvoiceModel.Bill_Dt;
                dtrowHeader["Curr_Id"] = _JobInvoiceModel.Currency;
                dtrowHeader["Conv_Rate"] = _JobInvoiceModel.ExRate;
                dtrowHeader["Inv_Status"] = IsNull(_JobInvoiceModel.Inv_Status, "D");
                dtrowHeader["gr_val"] = _JobInvoiceModel.GrossValue;
                dtrowHeader["tax_amt_nrecov"] = _JobInvoiceModel.TaxAmount;
                dtrowHeader["oc_amt"] = _JobInvoiceModel.OtherCharges;
                dtrowHeader["net_val"] = _JobInvoiceModel.NetAmount;
                //dtrowHeader["net_val_bs"] = _JobInvoiceModel.NetAmountInBase;
                dtrowHeader["Narration"] = _JobInvoiceModel.Narration;
               

                dtrowHeader["Comp_ID"] = CompID;
                dtrowHeader["Branch_ID"] = BrchID;
                dtrowHeader["User_ID"] = UserID;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["Mac_ID"] = mac_id;
                
                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;


                return DtblHDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private DataTable ToDtblItemDetail(string InvItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("mr_no", typeof(string));
                dtItem.Columns.Add("mr_date", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("InvoiceQty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val", typeof(string));
                //dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("gl_vou_no", typeof(string));
                dtItem.Columns.Add("gl_vou_dt", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("claim_itc", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));

                if (InvItemDetail != null)
                {
                    JArray jObject = JArray.Parse(InvItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal Accept_qty;

                        if (jObject[i]["jiinv_qty"].ToString() == "")
                            Accept_qty = 0;
                        else
                            Accept_qty = Convert.ToDecimal(jObject[i]["jiinv_qty"].ToString());

                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["mr_no"] = jObject[i]["mr_no"].ToString();
                        dtrowLines["mr_date"] = jObject[i]["mr_date"].ToString();
                        dtrowLines["ItemID"] = jObject[i]["item_id"].ToString();
                        dtrowLines["InvoiceQty"] = Accept_qty;
                        dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                        dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                        dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                        dtrowLines["item_oc_amt"] = 0;
                        dtrowLines["item_net_val"] = jObject[i]["item_net_val"].ToString();
                        //dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                        dtrowLines["gl_vou_no"] = jObject[i]["gl_vou_no"].ToString();
                        dtrowLines["gl_vou_dt"] = jObject[i]["gl_vou_dt"].ToString();
                        dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                        dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                        dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                        dtrowLines["claim_itc"] = jObject[i]["ClaimITC"].ToString();
                        dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();

                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["ItemDetails"] = dtitemdetail(jObject);
                }

                DtblItemDetail = dtItem;
                return DtblItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private DataTable ToDtblTaxDetail(string TaxDetails, string recov)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable Tax_detail = new DataTable();
                Tax_detail.Columns.Add("mr_no", typeof(string));
                Tax_detail.Columns.Add("mr_date", typeof(string));
                Tax_detail.Columns.Add("item_id", typeof(string));
                Tax_detail.Columns.Add("tax_id", typeof(string));
                Tax_detail.Columns.Add("tax_rate", typeof(string));
                Tax_detail.Columns.Add("tax_val", typeof(string));
                Tax_detail.Columns.Add("tax_level", typeof(string));
                Tax_detail.Columns.Add("tax_apply_on", typeof(string));
                Tax_detail.Columns.Add("recov", typeof(string));
                if (TaxDetails != null)
                {
                    JArray jObjectTax = JArray.Parse(TaxDetails);
                    for (int i = 0; i < jObjectTax.Count; i++)
                    {
                        DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();
                        dtrowTaxDetailsLines["mr_no"] = jObjectTax[i]["mr_no"].ToString();
                        dtrowTaxDetailsLines["mr_date"] = jObjectTax[i]["mr_date"].ToString();
                        dtrowTaxDetailsLines["item_id"] = jObjectTax[i]["item_id"].ToString();
                        dtrowTaxDetailsLines["tax_id"] = jObjectTax[i]["tax_id"].ToString();
                        string tax_rate = jObjectTax[i]["tax_rate"].ToString();
                        tax_rate = tax_rate.Replace("%", "");
                        dtrowTaxDetailsLines["tax_rate"] = tax_rate;
                        dtrowTaxDetailsLines["tax_level"] = jObjectTax[i]["tax_level"].ToString();
                        dtrowTaxDetailsLines["tax_val"] = jObjectTax[i]["tax_val"].ToString();
                        dtrowTaxDetailsLines["tax_apply_on"] = jObjectTax[i]["tax_apply_on"].ToString();
                        dtrowTaxDetailsLines["recov"] = recov == "Y" ? jObjectTax[i]["tax_recov"].ToString() : "";
                        Tax_detail.Rows.Add(dtrowTaxDetailsLines);
                    }
                    ViewData["JITaxDetails"] = dtTaxdetail(jObjectTax, recov);
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
        private DataTable ToDtblTdsDetail(string tdsDetails, string tdsType)
        {
            try
            {
                DataTable DtblItemtdsDetail = new DataTable();
                DataTable tds_detail = new DataTable();
                //tds_detail.Columns.Add("tds_id", typeof(string));
                //tds_detail.Columns.Add("tds_rate", typeof(string));
                //tds_detail.Columns.Add("tds_val", typeof(string));
                //tds_detail.Columns.Add("tds_level", typeof(string));
                //tds_detail.Columns.Add("tds_apply_on", typeof(string));
                tds_detail.Columns.Add("tds_id", typeof(string));
                tds_detail.Columns.Add("tds_rate", typeof(string));
                tds_detail.Columns.Add("tds_val", typeof(string));
                tds_detail.Columns.Add("tds_level", typeof(string));
                tds_detail.Columns.Add("tds_apply_on", typeof(string));
                tds_detail.Columns.Add("tds_ass_val", typeof(string));
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
                        //dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
                        //string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
                        //tds_rate = tds_rate.Replace("%", "");
                        //dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                        //dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
                        //dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
                        //dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();

                        dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
                        string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
                        tds_rate = tds_rate.Replace("%", "");
                        dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                        dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
                        dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();
                        dtrowtdsDetailsLines["tds_ass_val"] = jObjecttds[i]["Tds_totalAmnt"].ToString();
                        if (tdsType == "OC")
                        {
                            dtrowtdsDetailsLines["oc_id"] = jObjecttds[i]["Tds_oc_id"].ToString();
                            dtrowtdsDetailsLines["supp_id"] = jObjecttds[i]["Tds_supp_id"].ToString();
                        }
                        dtrowtdsDetailsLines["tds_base_amt"] = jObjecttds[i]["Tds_totalAmnt"].ToString();
                        dtrowtdsDetailsLines["tds_ass_apply_on"] = jObjecttds[i]["Tds_AssValApplyOn"].ToString();

                        tds_detail.Rows.Add(dtrowtdsDetailsLines);
                    }
                    ViewData["TDSDetails"] = dtTDSdetail(jObjecttds, tdsType);
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
            dtItem.Columns.Add("item_net_val", typeof(string));
            //dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("gl_vou_no", typeof(string));
            dtItem.Columns.Add("gl_vou_dt", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            dtItem.Columns.Add("claim_itc", typeof(string));
            dtItem.Columns.Add("loc_pur_coa", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                decimal Accept_qty;

                if (jObject[i]["jiinv_qty"].ToString() == "")
                    Accept_qty = 0;
                else
                    Accept_qty = Convert.ToDecimal(jObject[i]["jiinv_qty"].ToString());

                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();
                dtrowLines["inv_qty"] = Accept_qty;
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                dtrowLines["item_oc_amt"] = 0;
                dtrowLines["item_net_val"] = jObject[i]["item_net_val"].ToString();
                //dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                dtrowLines["gl_vou_no"] = jObject[i]["gl_vou_no"].ToString();
                dtrowLines["gl_vou_dt"] = jObject[i]["gl_vou_dt"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                dtrowLines["claim_itc"] = jObject[i]["ClaimITC"].ToString();
                dtrowLines["loc_pur_coa"] = jObject[i]["item_acc_id"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }

        public DataTable dtTaxdetail(JArray jObjectTax, string recov)
        {
            DataTable Tax_detail = new DataTable();

            Tax_detail.Columns.Add("mr_no", typeof(string));
            Tax_detail.Columns.Add("mr_date", typeof(string));
            Tax_detail.Columns.Add("item_id", typeof(string));
            Tax_detail.Columns.Add("tax_id", typeof(int));
            Tax_detail.Columns.Add("tax_name", typeof(string));
            Tax_detail.Columns.Add("tax_rate", typeof(string));
            Tax_detail.Columns.Add("tax_val", typeof(string));
            Tax_detail.Columns.Add("tax_level", typeof(int));
            Tax_detail.Columns.Add("tax_apply_on", typeof(string));
            Tax_detail.Columns.Add("tax_apply_Name", typeof(string));
            Tax_detail.Columns.Add("item_tax_amt", typeof(string));
            Tax_detail.Columns.Add("tax_recov", typeof(string));

            for (int i = 0; i < jObjectTax.Count; i++)
            {
                DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();

                dtrowTaxDetailsLines["mr_no"] = jObjectTax[i]["mr_no"].ToString();
                dtrowTaxDetailsLines["mr_date"] = jObjectTax[i]["mr_date"].ToString();
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
                dtrowTaxDetailsLines["tax_recov"] = recov == "Y" ? jObjectTax[i]["tax_recov"].ToString() : "";

                Tax_detail.Rows.Add(dtrowTaxDetailsLines);
            }

            return Tax_detail;
        }

        public DataTable dtTDSdetail(JArray jObjecttds, string tdsType)
        {
            //DataTable DtblItemtdsDetail = new DataTable();
            //DataTable tds_detail = new DataTable();
            //tds_detail.Columns.Add("tds_id", typeof(string));
            //tds_detail.Columns.Add("tds_rate", typeof(string));
            //tds_detail.Columns.Add("tds_val", typeof(string));
            //tds_detail.Columns.Add("tds_level", typeof(string));
            //tds_detail.Columns.Add("tds_apply_on", typeof(string));
            //tds_detail.Columns.Add("tds_name", typeof(string));
            //tds_detail.Columns.Add("tds_apply_Name", typeof(string));

            DataTable DtblItemtdsDetail = new DataTable();
            DataTable tds_detail = new DataTable();
            tds_detail.Columns.Add("tds_id", typeof(string));
            tds_detail.Columns.Add("tds_rate", typeof(string));
            tds_detail.Columns.Add("tds_val", typeof(string));
            tds_detail.Columns.Add("tds_level", typeof(string));
            tds_detail.Columns.Add("tds_apply_on", typeof(string));
            tds_detail.Columns.Add("tds_ass_val", typeof(string));
            if (tdsType == "OC")
            {
                tds_detail.Columns.Add("oc_id", typeof(string));
                tds_detail.Columns.Add("supp_id", typeof(string));
            }
            if (jObjecttds != null)
            {
                for (int i = 0; i < jObjecttds.Count; i++)
                {
                    DataRow dtrowtdsDetailsLines = tds_detail.NewRow();
                    //dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
                    //string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
                    //tds_rate = tds_rate.Replace("%", "");
                    //dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                    //dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
                    //dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
                    //dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();
                    //dtrowtdsDetailsLines["tds_name"] = jObjecttds[i]["Tds_name"].ToString();
                    //dtrowtdsDetailsLines["tds_apply_Name"] = jObjecttds[i]["Tds_applyOnName"].ToString();

                    dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
                    string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
                    tds_rate = tds_rate.Replace("%", "");
                    dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                    dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
                    dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
                    dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();
                    dtrowtdsDetailsLines["tds_ass_val"] = jObjecttds[i]["Tds_totalAmnt"].ToString();
                    if (tdsType == "OC")
                    {
                        dtrowtdsDetailsLines["oc_id"] = jObjecttds[i]["Tds_oc_id"].ToString();
                        dtrowtdsDetailsLines["supp_id"] = jObjecttds[i]["Tds_supp_id"].ToString();
                    }
                    tds_detail.Rows.Add(dtrowtdsDetailsLines);
                }
            }
            return tds_detail;
        }
        public DataTable dtOCdetail(JArray jObjectOC)
        {
            DataTable OC_detail = new DataTable();

            //OC_detail.Columns.Add("oc_id", typeof(int));
            //OC_detail.Columns.Add("oc_name", typeof(string));
            //OC_detail.Columns.Add("curr_name", typeof(string));
            //OC_detail.Columns.Add("conv_rate", typeof(string));
            //OC_detail.Columns.Add("oc_val", typeof(string));
            //OC_detail.Columns.Add("OCValBs", typeof(string));
            //OC_detail.Columns.Add("tax_amt", typeof(string));
            //OC_detail.Columns.Add("total_amt", typeof(string));

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

            for (int i = 0; i < jObjectOC.Count; i++)
            {
                DataRow dtrowOCDetailsLines = OC_detail.NewRow();

                //dtrowOCDetailsLines["oc_id"] = jObjectOC[i]["oc_id"].ToString();
                //dtrowOCDetailsLines["oc_name"] = jObjectOC[i]["OCName"].ToString();
                //dtrowOCDetailsLines["curr_name"] = jObjectOC[i]["OC_Curr"].ToString();
                //dtrowOCDetailsLines["conv_rate"] = jObjectOC[i]["OC_Conv"].ToString();
                //dtrowOCDetailsLines["oc_val"] = jObjectOC[i]["oc_val"].ToString();
                //dtrowOCDetailsLines["OCValBs"] = jObjectOC[i]["OC_AmtBs"].ToString();
                //dtrowOCDetailsLines["tax_amt"] = jObjectOC[i]["tax_amt"].ToString();
                //dtrowOCDetailsLines["total_amt"] = jObjectOC[i]["total_amt"].ToString();

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
                dtrowOCDetailsLines["tds_amt"] = jObjectOC[i]["tds_amt"].ToString() == "" ? "0" : jObjectOC[i]["tds_amt"].ToString();

                OC_detail.Rows.Add(dtrowOCDetailsLines);
            }

            return OC_detail;
        }
        public DataTable dtCCdetail(JArray JAObj)
        {
            DataTable CC_Details = new DataTable();

            //CC_Details.Columns.Add("acc_id", typeof(string));
            //CC_Details.Columns.Add("cc_id", typeof(int));
            //CC_Details.Columns.Add("cc_val_id", typeof(int));
            //CC_Details.Columns.Add("cc_amt", typeof(string));
            //CC_Details.Columns.Add("cc_name", typeof(string));
            //CC_Details.Columns.Add("cc_val_name", typeof(string));
            CC_Details.Columns.Add("vou_sr_no", typeof(string));
            CC_Details.Columns.Add("gl_sr_no", typeof(string));
            CC_Details.Columns.Add("acc_id", typeof(string));
            CC_Details.Columns.Add("cc_id", typeof(int));
            CC_Details.Columns.Add("cc_val_id", typeof(int));
            CC_Details.Columns.Add("cc_amt", typeof(string));
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                //dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                //dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                //dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                //dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                //dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                //dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();
                dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();
                dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();

                CC_Details.Rows.Add(dtrowLines);
            }
            return CC_Details;
        }
        public DataTable dtVoudetail(JArray jObjectVOU)
        {
            DataTable vou_Details = new DataTable();
            //vou_Details.Columns.Add("comp_id", typeof(string));
            //vou_Details.Columns.Add("acc_id", typeof(string));
            //vou_Details.Columns.Add("acc_name", typeof(string));
            //vou_Details.Columns.Add("type", typeof(string));
            //vou_Details.Columns.Add("doctype", typeof(string));
            //vou_Details.Columns.Add("Value", typeof(string));
            //vou_Details.Columns.Add("dr_amt_sp", typeof(string));
            //vou_Details.Columns.Add("cr_amt_sp", typeof(string));
            //vou_Details.Columns.Add("TransType", typeof(string));
            //vou_Details.Columns.Add("gl_type", typeof(string));
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
            for (int i = 0; i < jObjectVOU.Count; i++)
            {
                DataRow dtrowVouDetailsLines = vou_Details.NewRow();
                //dtrowVouDetailsLines["comp_id"] = jObjectVOU[i]["comp_id"].ToString();
                //dtrowVouDetailsLines["acc_id"] = jObjectVOU[i]["id"].ToString();
                //dtrowVouDetailsLines["acc_name"] = jObjectVOU[i]["acc_name"].ToString();
                //dtrowVouDetailsLines["type"] = jObjectVOU[i]["type"].ToString();
                //dtrowVouDetailsLines["doctype"] = jObjectVOU[i]["doctype"].ToString();
                //dtrowVouDetailsLines["Value"] = jObjectVOU[i]["Value"].ToString();
                //dtrowVouDetailsLines["dr_amt_sp"] = jObjectVOU[i]["DrAmt"].ToString();
                //dtrowVouDetailsLines["cr_amt_sp"] = jObjectVOU[i]["CrAmt"].ToString();
                //dtrowVouDetailsLines["TransType"] = jObjectVOU[i]["TransType"].ToString();
                //dtrowVouDetailsLines["gl_type"] = jObjectVOU[i]["Gltype"].ToString();
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
                vou_Details.Rows.Add(dtrowVouDetailsLines);
            }
            return vou_Details;
        }
        private List<JobInvoiceList> getJINVList(JI_ListModel _JI_ListModel)
        {
            _JobInvoiceList = new List<JobInvoiceList>();
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                var docid = DocumentMenuId;
                string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                if(_JI_ListModel.WF_Status != null)
                {
                    wfstatus = _JI_ListModel.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }

                DataSet DSet = _JobInvoice_IServices.GetJoInvListandSrchDetail(CompID, BrchID, _JI_ListModel, UserID, wfstatus, DocumentMenuId);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        JobInvoiceList _JINVList = new JobInvoiceList();
                        _JINVList.InvoiceNo = dr["inv_no"].ToString();
                        _JINVList.InvoiceDate = dr["InvDate"].ToString();
                        _JINVList.InvDate = dr["InvDt"].ToString();
                        _JINVList.SuppName = dr["supp_name"].ToString();
                        _JINVList.GRNNumber = dr["grn_no"].ToString();
                        _JINVList.GRNDate = dr["GrnDate"].ToString();
                        _JINVList.GRNDt = dr["grn_dt"].ToString();
                        _JINVList.InvoiceValue = dr["net_val"].ToString();
                        _JINVList.Stauts = dr["Status"].ToString();
                        _JINVList.CreateDate = dr["CreateDate"].ToString();
                        _JINVList.ApproveDate = dr["ApproveDate"].ToString();
                        _JINVList.ModifyDate = dr["ModifyDate"].ToString();
                        _JINVList.Create_By = dr["create_by"].ToString();
                        _JINVList.Mod_By = dr["mod_by"].ToString();
                        _JINVList.App_By = dr["app_by"].ToString();

                        _JobInvoiceList.Add(_JINVList);
                    }
                }
                _JI_ListModel.FinStDt = Convert.ToDateTime(DSet.Tables[1].Rows[0]["finstrdate"]);
                //Session["FinStDt"] = DSet.Tables[1].Rows[0]["finstrdate"];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;

            }
            return _JobInvoiceList;
        }

        [HttpPost]
        public ActionResult SearchJINVListDetail(string SuppId, string Fromdate, string Todate, string Status)
        {
            JI_ListModel _JI_ListModel = new JI_ListModel();
            try
            {
                //Session.Remove("WF_Docid");
                // Session.Remove("WF_status");
                _JI_ListModel.WF_Status = null;
                string User_ID = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                var docid = DocumentMenuId;
                List<JobInvoiceList> _JobInvoiceList = new List<JobInvoiceList>();
                _JI_ListModel.SuppID = SuppId;

                _JI_ListModel.FromDate = Fromdate;
                _JI_ListModel.ToDate = Todate;
                _JI_ListModel.Status = Status;
                DataSet DSet = _JobInvoice_IServices.GetJoInvListandSrchDetail(CompID, BrchID, _JI_ListModel, "", "", "");
                //Session["JINVSearch"] = "JINV_Search";
                _JI_ListModel.JINVSearch= "JINV_Search";
                foreach (DataRow dr in DSet.Tables[0].Rows)
                {
                    JobInvoiceList _JINVList = new JobInvoiceList();
                    _JINVList.InvoiceNo = dr["inv_no"].ToString();
                    _JINVList.InvoiceDate = dr["InvDate"].ToString();
                    _JINVList.InvDate = dr["InvDt"].ToString();
                    _JINVList.SuppName = dr["supp_name"].ToString();
                    _JINVList.GRNNumber = dr["grn_no"].ToString();
                    _JINVList.GRNDate = dr["GrnDate"].ToString();
                    _JINVList.GRNDt = dr["grn_dt"].ToString();
                    _JINVList.InvoiceValue = dr["net_val"].ToString();
                    _JINVList.Stauts = dr["Status"].ToString();
                    _JINVList.CreateDate = dr["CreateDate"].ToString();
                    _JINVList.ApproveDate = dr["ApproveDate"].ToString();
                    _JINVList.ModifyDate = dr["ModifyDate"].ToString();
                    _JINVList.Create_By = dr["create_by"].ToString();
                    _JINVList.Mod_By = dr["mod_by"].ToString();
                    _JINVList.App_By = dr["app_by"].ToString();
                    _JobInvoiceList.Add(_JINVList);
                }
              
                _JI_ListModel.JIList = _JobInvoiceList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
               
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialJobInvoiceSCList.cshtml", _JI_ListModel);
        }
        public ActionResult DoubleClickFromList(string DocNo, string DocDate, JobInvoiceModel _JobInvoiceModel, string ListFilterData,string WF_Status)
        {/*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
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
            _JobInvoiceModel.Message = "New";
            _JobInvoiceModel.Command = "Update";
            _JobInvoiceModel.TransType = "Update";
            _JobInvoiceModel.BtnName = "BtnToDetailPage";
            _JobInvoiceModel.JInv_No = DocNo;
            _JobInvoiceModel.JInv_Dt = DocDate;
            if(WF_Status != null && WF_Status!= "")
            {
                _JobInvoiceModel.WF_Status1 = WF_Status;
            }
            var WF_Status1 = _JobInvoiceModel.WF_Status1;
            var JICodeURL = DocNo;
            var JIDate = DocDate;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var command = "Add";

            TempData["ModelData"] = _JobInvoiceModel;
            TempData["ListFilterData"] = ListFilterData;

            return (RedirectToAction("JobInvoiceSCDetail", "JobInvoiceSC", new { JICodeURL = JICodeURL, JIDate, TransType, BtnName, command, WF_Status1 }));


        }

        public ActionResult DeleteJInvDetails(JobInvoiceModel _JobInvoiceModel, string command)
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
                string br_id = Session["BranchId"].ToString();
                string JInvNo = _JobInvoiceModel.JInv_No;
                string JInvDelete = _JobInvoice_IServices.JInv_DeleteDetail(_JobInvoiceModel, CompID, BrchID);

                if (!string.IsNullOrEmpty(JInvNo))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string JInvNo1 = JInvNo.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, JInvNo1, Server);
                }
                _JobInvoiceModel = new JobInvoiceModel();
                _JobInvoiceModel.Message = "Deleted";
                _JobInvoiceModel.Command = "Refresh";
                _JobInvoiceModel.TransType = "Refresh";
                _JobInvoiceModel.BtnName = "BtnDelete";
                TempData["ModelData"] = _JobInvoiceModel;
                return RedirectToAction("GoodReceiptNoteSCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult JIApprove(JobInvoiceModel _JobInvoiceModel,string VoucherNarr, string ListFilterData1)
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
                string MenuID = DocumentMenuId;

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string JI_No = _JobInvoiceModel.JInv_No;
                string JI_Date = _JobInvoiceModel.JInv_Dt;
                string A_Status = _JobInvoiceModel.A_Status;
                string A_Level = _JobInvoiceModel.A_Level;
                string A_Remarks = _JobInvoiceModel.A_Remarks;
                string BP_Nurration = _JobInvoiceModel.BP_Nurration;
                string DN_Nurration = _JobInvoiceModel.DN_Nurration;
                if (VoucherNarr == "")
                {
                    VoucherNarr = _JobInvoiceModel.Nurration;
                }
                string Message = _JobInvoice_IServices.JIApproveDetails(CompID, BrchID, JI_No, JI_Date, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks,VoucherNarr, BP_Nurration, DN_Nurration);
                string ApMessage = Message.Split(',')[2].Trim();
                string MDSC_No = Message.Split(',')[0].Trim();

                if (ApMessage == "A")
                {
                    _JobInvoiceModel.Message = "Approved";
                }

                _JobInvoiceModel.TransType = "Update";
                _JobInvoiceModel.Command = "Approve";
                _JobInvoiceModel.AppStatus = "D";
                _JobInvoiceModel.BtnName = "BtnEdit";

                var JICodeURL = MDSC_No;
                var JIDate = _JobInvoiceModel.JInv_Dt;
                var TransType = _JobInvoiceModel.TransType;
                var BtnName = _JobInvoiceModel.BtnName;
                var command = _JobInvoiceModel.Command;
                TempData["ModelData"] = _JobInvoiceModel;
                TempData["ListFilterData"] = ListFilterData1;
                return (RedirectToAction("DeliveryNoteSCDetail", new { JICodeURL = JICodeURL, JIDate, TransType, BtnName, command }));


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList,string VoucherNarr, string ListFilterData1, string WF_Status)
        {
            JobInvoiceModel _JobInvoiceModel = new JobInvoiceModel();

            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _JobInvoiceModel.JInv_No = jObjectBatch[i]["JINo"].ToString();
                    _JobInvoiceModel.JInv_Dt = jObjectBatch[i]["JIDate"].ToString();

                    _JobInvoiceModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _JobInvoiceModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _JobInvoiceModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();

                }
            }
            if (_JobInvoiceModel.A_Status != "Approve")
            {
                _JobInvoiceModel.A_Status = "Approve";
            }
            JIApprove(_JobInvoiceModel, VoucherNarr, ListFilterData1);
            if (WF_Status != null && WF_Status != "")
            {
                _JobInvoiceModel.WF_Status1 = WF_Status;
            }
            var WF_Status1 = _JobInvoiceModel.WF_Status1;
            TempData["ModelData"] = _JobInvoiceModel;
            var JICodeURL = _JobInvoiceModel.JInv_No;
            var JIDate = _JobInvoiceModel.JInv_Dt;
            var TransType = _JobInvoiceModel.TransType;
            var BtnName = _JobInvoiceModel.BtnName;
            var command = _JobInvoiceModel.Command;
            return (RedirectToAction("JobInvoiceSCDetail", new { JICodeURL = JICodeURL, JIDate, TransType, BtnName, command, WF_Status1 }));


        }
        public ActionResult ToRefreshByJS(string FrwdDtList, string ListFilterData1 ,string WF_Status)
        {
            JobInvoiceModel _JobInvoiceModel = new JobInvoiceModel();

            if (FrwdDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(FrwdDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _JobInvoiceModel.JInv_No = jObjectBatch[i]["JINo"].ToString();
                    _JobInvoiceModel.JInv_Dt = jObjectBatch[i]["JIDate"].ToString();
                    _JobInvoiceModel.TransType = "Update";
                    _JobInvoiceModel.BtnName = "BtnToDetailPage";
                    if (WF_Status != null && WF_Status != "") 
                    {
                        _JobInvoiceModel.WF_Status1 = WF_Status;
                    }
                    TempData["ModelData"] = _JobInvoiceModel;
                }
            }
            var WF_Status1 = _JobInvoiceModel.WF_Status1;
            var JICodeURL = _JobInvoiceModel.JInv_No;
            var JIDate = _JobInvoiceModel.JInv_Dt;
            var TransType = _JobInvoiceModel.TransType;
            var BtnName = _JobInvoiceModel.BtnName;
            var command = "Refresh";
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("JobInvoiceSCDetail", new { JICodeURL = JICodeURL, JIDate, TransType, BtnName, command, WF_Status1 }));

        }
        public ActionResult GetJobInvoiceDashbordList(string docid, string status)
        {

            // Session["WF_status"] = status;
            var WF_Status = status;
            return RedirectToAction("JobInvoiceSC",new { WF_Status });
        }

        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                JIDetailsattch _JIDetail = new JIDetailsattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Guid gid = new Guid();
                gid = Guid.NewGuid();


                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _JIDetail.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //CommonPageDetails();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _JIDetail.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _JIDetail.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _JIDetail;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        /*--------------------------For Attatchment End--------------------------*/

        [HttpPost]
        public JsonResult GetGLDetails(List<GL_Detail> GLDetail)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                DataTable DtblGLDetail = new DataTable();

                if (GLDetail != null)
                {
                    DtblGLDetail = ToDataTable(GLDetail);
                }
                DataSet GlDt = _JobInvoice_IServices.GetAllGLDetails(DtblGLDetail);
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
        public ActionResult GetAutoCompleteSearchSuppList(JI_ListModel _JI_ListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
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
                if (string.IsNullOrEmpty(_JI_ListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _JI_ListModel.SuppName;
                }
                CustList = _JobInvoice_IServices.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _JI_ListModel.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSuppAddrDetail(string Supp_id)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _JobInvoice_IServices.GetSuppAddrDetailDAL(Supp_id, Comp_ID);
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
        public JsonResult GetGoodReceiptNoteLists(JobInvoiceModel _JobInvoiceModel/*string Supp_id*/)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string DocumentNumber = string.Empty;
                string Supp_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                Supp_id = _JobInvoiceModel.SuppID;
                DocumentNumber = _JobInvoiceModel.DocumentNo;
                DataSet result = _JobInvoice_IServices.GetGoodReceiptNoteList(Supp_id, Comp_ID, Br_ID,"");

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
        public ActionResult GetDeliveryNoteSourceDocumentNoList(JobInvoiceModel _JobInvoiceModel)
        {
            try
            {
                string DocumentNumber = string.Empty;
                DataSet DocumentNumberList = new DataSet();
                string Supp_id = string.Empty;
                string Comp_ID = string.Empty;
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                }
                Supp_id = _JobInvoiceModel.SuppID;
                DocumentNumber = _JobInvoiceModel.DocumentNo;
                string Br_ID = Session["BranchId"].ToString();
                DocumentNumberList = _JobInvoice_IServices.GetGoodReceiptNoteList(Supp_id, Comp_ID, Br_ID, DocumentNumber);
                DataRow dr1;
                dr1 = DocumentNumberList.Tables[0].NewRow();
                dr1[0] = "---Select---";
                dr1[1] = "0";
                DocumentNumberList.Tables[0].Rows.InsertAt(dr1, 0);
                foreach (DataRow dr in DocumentNumberList.Tables[0].Rows)
                {
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.GrnNoId = dr["mr_no"].ToString();
                    _DocumentNumber.GrnnoVal = dr["mr_dt"].ToString();
                    _DocumentNumberList.Add(_DocumentNumber);
                }

                return Json(_DocumentNumberList.Select(c => new { Name = c.GrnNoId, ID = c.GrnnoVal }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }


        }
        [HttpPost]
        public JsonResult GetJOandGoodReceiptNoteSCDetails(string GRNNo, string GRNDate)
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
                DataSet result = _JobInvoice_IServices.GetJOandGoodReceiptNoteSCDetails(GRNNo, GRNDate, Comp_ID, Br_ID);

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

        /*------------------- For Print Section-------------------------------*/
        public FileResult GenratePdfFile(JobInvoiceModel _JobInvoiceModel)
        {
            return File(GetPdfData(_JobInvoiceModel.DocumentMenuId, _JobInvoiceModel.JInv_No, _JobInvoiceModel.JInv_Dt), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
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
                //string inv_type = "";
                //string ReportType = "common";
                
                DataSet Details = _JobInvoice_IServices.GetJobInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt);
                ViewBag.PageName = "JI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 07-04-2025*/
                ViewBag.Title = "Job Invoice";
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 07-04-2025*/
                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SubContracting/JobInvoice/JobInvoicePrint.cshtml"));
                
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

        /*------------------- For Print Section End-------------------------------*/
        /*------------------- For Cancel Voucher-------------------------------*/
        public string CheckJICancellationAgainstVoucher(string DocNo, string DocDate)/*Add By Hina on 16-08-2024 */
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
                DataSet Deatils = _JobInvoice_IServices.CheckJIDetail(Comp_ID, Br_ID, DocNo, DocDate);

                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = "PaymentCreated";
                }
                return Result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            
        }

        /*------------------- For Cancel Voucher End-------------------------------*/
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

                //ViewBag.GstApplicable = "N";
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
        public ActionResult ErrorPage()
        {
            try
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
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
        private string ConvertBoolToStrint(Boolean _bool)
        {
            if (_bool)
                return "Y";
            else
                return "N";
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
    }

}