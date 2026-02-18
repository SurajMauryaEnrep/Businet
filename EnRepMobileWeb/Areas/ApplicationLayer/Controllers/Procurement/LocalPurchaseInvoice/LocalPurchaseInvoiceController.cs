using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.DomesticPurchaseInvoice;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.Resources;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.DomesticPurchaseInvoiceIService;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.LocalPurchaseInvoice
{
    public class LocalPurchaseInvoiceController : Controller
    {
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string DocumentMenuId = "";
        string CompID, User_ID, language, title, Inv_No, BrchID = string.Empty;
        string FromDate;
        DomesticPurchaseInvoiceIService _DomesticPurchaseInvoiceIService;
        DomesticPurchaseInvoiceListIService _DomesticPurchaseInvoiceListIService;
        Common_IServices _Common_IServices;

        // GET: ApplicationLayer/LocalPurchaseInvoice
        public LocalPurchaseInvoiceController(Common_IServices _Common_IServices, DomesticPurchaseInvoiceIService _DomesticPurchaseInvoiceIService, DomesticPurchaseInvoiceListIService _DomesticPurchaseInvoiceListIService)
        {
            this._Common_IServices = _Common_IServices;
            this._DomesticPurchaseInvoiceIService = _DomesticPurchaseInvoiceIService;
            this._DomesticPurchaseInvoiceListIService = _DomesticPurchaseInvoiceListIService;
        }

        public ActionResult LocalPurchaseInvoiceList(PI_ListModel _PI_ListModel)
        {
            try
            {

                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                //PI_ListModel _PI_ListModel = new PI_ListModel();
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105101145")
                //    {
                //        DocumentMenuId = "105101145";

                //    }                   
                //}
                DocumentMenuId = "105101145";
                _PI_ListModel.DocumentMenuId = DocumentMenuId;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                
                CommonPageDetails();
                if (_PI_ListModel.DocumentMenuId != null)
                {
                    _PI_ListModel.PI_wfdocid = _PI_ListModel.DocumentMenuId;
                }
                else
                {
                    _PI_ListModel.PI_wfdocid = "0";
                }
                if (TempData["WF_Status"] != null && TempData["WF_Status"].ToString() != "")
                {
                    if (TempData["WF_Status"] != null)
                    {
                        _PI_ListModel.WF_Status = TempData["WF_Status"].ToString();
                        _PI_ListModel.PI_wfstatus = _PI_ListModel.WF_Status;
                    }
                    else
                    {
                        _PI_ListModel.PI_wfstatus = "";
                    }
                }
                else
                {
                    if (_PI_ListModel.WF_Status != null)
                    {
                        _PI_ListModel.PI_wfstatus = _PI_ListModel.WFStatus;
                    }
                    else
                    {
                        _PI_ListModel.PI_wfstatus = "";
                    }
                }
                //}if (Session["WF_status"] != null)
                //{
                //    _PI_ListModel.PI_wfstatus = Session["WF_status"].ToString();
                //}
                //else
                //{
                //    _PI_ListModel.PI_wfstatus = "";
                //}
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string todate = range.ToDate;

                GetSuppList(_PI_ListModel);
                ViewBag.DocumentMenuId = DocumentMenuId;
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _PI_ListModel.StatusList = statusLists;
                if (TempData["FilterData"] != null)
                {
                    var PRData = TempData["FilterData"].ToString();
                    var a = PRData.Split(',');
                    _PI_ListModel.PI_SuppID = a[0].Trim();
                    _PI_ListModel.PI_FromDate = a[1].Trim();
                    _PI_ListModel.PI_ToDate = a[2].Trim();
                    _PI_ListModel.Status = a[3].Trim();
                    if (_PI_ListModel.Status == "0")
                    {
                        _PI_ListModel.Status = null;
                    }
                    _PI_ListModel.FilterData = TempData["FilterData"].ToString();
                }
              
                _PI_ListModel.FromDate = _PI_ListModel.PI_FromDate;
                _PI_ListModel.ToDate =Convert.ToDateTime(_PI_ListModel.PI_ToDate);
                if (_PI_ListModel.FromDate == null)
                {
                    _PI_ListModel.FromDate = startDate;
                    _PI_ListModel.PI_FromDate = startDate;
                    _PI_ListModel.PI_ToDate = todate;
                }
                _PI_ListModel.PIList = GetGRNDetailList(_PI_ListModel);
                _PI_ListModel.Title = title;
                //Session["LPISearch"] = "0";
                _PI_ListModel.LPISearch = "0";
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/LocalPurchaseInvoiceList.cshtml", _PI_ListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult ImportPurchaseInvoiceList()
        {
            try
            {

                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                //if (Session["MenuDocumentId"] != null)
                //{                   
                //    if (Session["MenuDocumentId"].ToString() == "105101140125")
                //    {
                //        DocumentMenuId = "105101140125";
                //    }
                //}
                PI_ListModel _PI_ListModel = new PI_ListModel();
                DocumentMenuId = "105101140125";
                _PI_ListModel.DocumentMenuId = DocumentMenuId;
                
              
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                
                CommonPageDetails();
               
                //if (Session["MenuDocumentId"] != null)
                //{
                //    _PI_ListModel.PI_wfdocid = Session["MenuDocumentId"].ToString();
                //}
                //else
                //{
                //    _PI_ListModel.PI_wfdocid = "0";
                //}
                //if (Session["WF_status"] != null)
                //{
                //    _PI_ListModel.PI_wfstatus = Session["WF_status"].ToString();
                //}
                //else
                //{
                //    _PI_ListModel.PI_wfstatus = "";
                //}

                if (_PI_ListModel.DocumentMenuId != null)
                {
                    _PI_ListModel.PI_wfdocid = _PI_ListModel.DocumentMenuId;
                }
                else
                {
                    _PI_ListModel.PI_wfdocid = "0";
                }
                if (TempData["WF_Status"] != null && TempData["WF_Status"].ToString() != "")
                {
                    if (TempData["WF_Status"] != null)
                    {
                        _PI_ListModel.WF_Status = TempData["WF_Status"].ToString();
                        _PI_ListModel.PI_wfstatus = _PI_ListModel.WF_Status;
                    }
                    else
                    {
                        _PI_ListModel.PI_wfstatus = "";
                    }
                }
                else
                {
                    if (_PI_ListModel.WF_Status != null)
                    {
                        _PI_ListModel.PI_wfstatus = _PI_ListModel.WFStatus;
                    }
                    else
                    {
                        _PI_ListModel.PI_wfstatus = "";
                    }
                }
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");             

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string todate = range.ToDate;

                GetSuppList(_PI_ListModel);         
                ViewBag.DocumentMenuId = DocumentMenuId;         

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _PI_ListModel.StatusList = statusLists;
                if (TempData["FilterData"] != null)
                {
                    var PRData = TempData["FilterData"].ToString();
                    var a = PRData.Split(',');
                    _PI_ListModel.PI_SuppID = a[0].Trim();
                    _PI_ListModel.PI_FromDate = a[1].Trim();
                    _PI_ListModel.PI_ToDate = a[2].Trim();
                    _PI_ListModel.Status = a[3].Trim();
                    if (_PI_ListModel.Status == "0")
                    {
                        _PI_ListModel.Status = null;
                    }
                    _PI_ListModel.FilterData = TempData["FilterData"].ToString();
                }
            
                if (_PI_ListModel.PI_FromDate == null)
                {
                    _PI_ListModel.FromDate = startDate;
                    _PI_ListModel.PI_FromDate = startDate;
                    _PI_ListModel.PI_ToDate = todate;
                }
                else
                {
                    _PI_ListModel.FromDate = _PI_ListModel.PI_FromDate;
                }
                //_PI_ListModel.FromDate = startDate;
                _PI_ListModel.PIList = GetGRNDetailList(_PI_ListModel);
                _PI_ListModel.Title = title;
                //Session["LPISearch"] = "0";
                _PI_ListModel.LPISearch = "0";
                return View("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/LocalPurchaseInvoiceList.cshtml", _PI_ListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void CommonPageDetails()
        {
            try
            {
                //PI_ListModel _PI_ListModel = new PI_ListModel();
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105101145")
                //    {
                //        DocumentMenuId = "105101145";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105101140125")
                //    {
                //        DocumentMenuId = "105101140125";
                //    }
                //}
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
                    User_ID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, User_ID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
                ViewBag.VBRoleList = ds.Tables[3];
                ViewBag.StatusList = ds.Tables[4];
                ViewBag.DnForVarianceQty = ds.Tables[11].Rows.Count > 0 ? ds.Tables[11].Rows[0]["param_stat"].ToString() : ""; ;
                /* ---------- Added by Suraj Maurya on 08-12-2025 to add new parameter ----------- */
                DataRow[] rows = ds.Tables[1].Select("param_id = '118'");
                ViewBag.CostCenterDetailMandatory = rows.Length > 0 ? ds.Tables[1].Rows[4]["param_stat"].ToString() : "";
                /* ---------- Added by Suraj Maurya on 08-12-2025 to add new parameter end ----------- */

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

        public ActionResult ToRefreshByJS(string FilterData,string TrancType,string Mailerror)
        {
            PI_ListModel _PI_ListModel = new PI_ListModel();
            //Session["Message"] = "";
            _PI_ListModel.Message = "";
            var a = TrancType.Split(',');
            _PI_ListModel.DocumentMenuId = a[0].Trim();
            _PI_ListModel.PI_inv_no = a[1].Trim();
            _PI_ListModel.PI_inv_dt = a[2].Trim();
            _PI_ListModel.TransType ="Update";          
            var WF_status1 = a[3].Trim();
            _PI_ListModel.WF_Status1 = WF_status1;
            _PI_ListModel.Message = Mailerror;
            _PI_ListModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _PI_ListModel;
            TempData["WF_status1"] = WF_status1; 
            TempData["FilterData"] = FilterData;
            var Inv_no = _PI_ListModel.PI_inv_no;
           var  Inv_dt = _PI_ListModel.PI_inv_dt;
           var  TransType = _PI_ListModel.TransType;
           var  BtnName = _PI_ListModel.BtnName;
          var   Command = _PI_ListModel.Command;
            var menuDocumentID = _PI_ListModel.DocumentMenuId;
            return RedirectToAction("LocalPurchaseInvoiceDetail",new { Inv_no = Inv_no, Inv_dt, TransType, BtnName, Command, menuDocumentID, WF_status1 });
        }
        public ActionResult LocalPurchaseInvoiceDetail(PI_ListModel _PI_ListModel,string Inv_no,string Inv_dt,string TransType,string BtnName,string Command ,string menuDocumentID,string WF_Status1)
        {
           
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                User_ID = Session["userid"].ToString();
            }
            /*Add by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, Inv_dt) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
               
                var pi_ListModel = TempData["ModelData"] as PI_ListModel;
                if (pi_ListModel != null)
                {
                   if(menuDocumentID != null)
                    {
                        pi_ListModel.DocumentMenuId = menuDocumentID;

                        DocumentMenuId = menuDocumentID;
                    }
                    else
                    {
                        menuDocumentID = pi_ListModel.DocumentMenuId;
                    }
                        
                    DomesticPurchaseInvoice_Model DomesticPurchaseInvoice_Model = new DomesticPurchaseInvoice_Model();
                    DocumentMenuId = menuDocumentID;
                    CommonPageDetails();
                    HeaderPI_Detail HeaderPI_Detail = new HeaderPI_Detail();
                    GetAutoCompleteSearchSuppList(pi_ListModel);

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.grn_no = "---Select---";
                    _DocumentNumber.grn_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    pi_ListModel.GRNNumberList = _DocumentNumberList;

                    List<CurrancyList> currancyLists = new List<CurrancyList>();
                    currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    pi_ListModel.currancyLists = currancyLists;

                    //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    //string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    //ViewBag.ValDigit = ValDigit;
                    //ViewBag.RateDigit = RateDigit;
                    //ViewBag.QtyDigit = QtyDigit;
                    SetDecimals(pi_ListModel);
                    if (TempData["FilterData"] != null && TempData["FilterData"].ToString() != "")
                    {
                        pi_ListModel.FilterData1 = TempData["FilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        pi_ListModel.WF_Status1 = TempData["wf_status1"].ToString();
                    }
                    else
                    {
                        pi_ListModel.WF_Status1 = WF_Status1;
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (pi_ListModel.TransType == "Update" || pi_ListModel.TransType == "Edit")
                    {
                        //string PI_No = Session["Inv_No"].ToString();
                        //string PI_Date = Session["Inv_Dt"].ToString();
                        string VouType = "PV";
                        string PI_No = pi_ListModel.PI_inv_no;
                        string PI_Date =(pi_ListModel.PI_inv_dt);
                        string Type = "D";
                        DataSet ds = _DomesticPurchaseInvoiceIService.Edit_PIDetail(CompID, BrchID, VouType, PI_No, PI_Date, Type, User_ID, DocumentMenuId);
                        ViewBag.AttechmentDetails = ds.Tables[8];
                        /* Added by Suraj Maurya on 05-04-2025 */
                        pi_ListModel.var_qty_dtl = Json(JsonConvert.SerializeObject(ds.Tables[16]), JsonRequestBehavior.AllowGet).Data.ToString();
                        pi_ListModel.var_qty_tax_dtl = Json(JsonConvert.SerializeObject(ds.Tables[17]), JsonRequestBehavior.AllowGet).Data.ToString();
                        /* Added by Suraj Maurya on 05-04-2025 */
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
                        ViewBag.SubItemDetails = ds.Tables[13];
                        ViewBag.ItemTDSDetails = ds.Tables[14];
                        ViewBag.ItemOC_TDSDetails = ds.Tables[15];
                        pi_ListModel.PI_inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        pi_ListModel.PI_inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                        pi_ListModel.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        pi_ListModel.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                        pi_ListModel.hdnbillno = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        pi_ListModel.hdnbilldt = ds.Tables[0].Rows[0]["BillDate"].ToString();
                        pi_ListModel.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                        string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        pi_ListModel.SuppCurrency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        currancyLists.Add(new CurrancyList { curr_id = pi_ListModel.SuppCurrency, curr_name = Curr_name });
                        pi_ListModel.currancyLists = currancyLists;
                        //pi_ListModel.SuppCurrency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        pi_ListModel.ExRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(pi_ListModel.QtyDigit);
                        pi_ListModel.PI_SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        pi_ListModel.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                        pi_ListModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        pi_ListModel.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        pi_ListModel.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        pi_ListModel.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        pi_ListModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        pi_ListModel.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        pi_ListModel.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        pi_ListModel.Status_name = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                        pi_ListModel.PriceBasis = ds.Tables[0].Rows[0]["price_basis"].ToString();
                        pi_ListModel.FreightType = ds.Tables[0].Rows[0]["freight_type"].ToString();
                        pi_ListModel.ModeOfTransport = ds.Tables[0].Rows[0]["mode_trans"].ToString();
                        pi_ListModel.Destination = ds.Tables[0].Rows[0]["dest"].ToString();
                        pi_ListModel.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                        pi_ListModel.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                        pi_ListModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        pi_ListModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                        pi_ListModel.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        pi_ListModel.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(pi_ListModel.ValDigit);
                        pi_ListModel.GrossValueInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val_bs"]).ToString(pi_ListModel.ValDigit);
                        pi_ListModel.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(pi_ListModel.ValDigit);
                        pi_ListModel.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(pi_ListModel.ValDigit);
                        pi_ListModel.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val"]).ToString(pi_ListModel.ValDigit);
                        pi_ListModel.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(pi_ListModel.ValDigit);

                        pi_ListModel.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                        pi_ListModel.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                        pi_ListModel.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                        pi_ListModel.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();

                        //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                        var state_code = ds.Tables[0].Rows[0]["state_code"];
                        var br_state_code = ds.Tables[0].Rows[0]["br_state_code"];
                        //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                        if (state_code.ToString() == br_state_code.ToString())
                        {
                            pi_ListModel.Hd_GstType = "Both";
                        }
                        else
                        {
                            pi_ListModel.Hd_GstType = "IGST";
                        }


                        pi_ListModel.GRNNumber = "---Select---";

                            List<DocumentNumber> _DocumentNumberListForImp = new List<DocumentNumber>();
                            _DocumentNumber.grn_no = ds.Tables[0].Rows[0]["mr_no"].ToString();
                            _DocumentNumber.grn_dt = ds.Tables[0].Rows[0]["mr_date"].ToString();
                            _DocumentNumberListForImp.Add(_DocumentNumber);
                            pi_ListModel.GRNNumberList = _DocumentNumberListForImp;
                            pi_ListModel.GRNDate = ds.Tables[0].Rows[0]["mr_date"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["inv_status1"].ToString().Trim();
                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        pi_ListModel.pmflagval= ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        if (roundoff_status == "Y")
                        {
                            pi_ListModel.RoundOffFlag = true;
                        }
                        else
                        {
                            pi_ListModel.RoundOffFlag = false;
                        }
                        string RCMApplicable = ds.Tables[0].Rows[0]["rcm_app"].ToString().Trim();
                        if (RCMApplicable == "Y")
                        {
                            pi_ListModel.RCMApplicable = true;
                        }
                        else
                        {
                            pi_ListModel.RCMApplicable = false;
                        }
                        pi_ListModel.doc_status = doc_status;
                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            if (doc_status == "A" || doc_status == "C")
                            {
                                pi_ListModel.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                            }
                            pi_ListModel.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                            pi_ListModel.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                            ViewBag.GLVoucherNo = pi_ListModel.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                        }


                        //Session["DocumentStatus"] = doc_status;
                        pi_ListModel.DocumentStatus = doc_status;
                        if (doc_status == "C")
                        {
                            pi_ListModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString(); 
                            pi_ListModel.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            pi_ListModel.BtnName = "Refresh";
                        }
                        else
                        {
                            pi_ListModel.CancelFlag = false;
                        }
                        pi_ListModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        pi_ListModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && pi_ListModel.Command != "Edit")
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
                                if (create_id != User_ID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    pi_ListModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == User_ID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        pi_ListModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        pi_ListModel.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (User_ID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    pi_ListModel.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == User_ID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        pi_ListModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (User_ID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    pi_ListModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == User_ID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    pi_ListModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == User_ID || approval_id == User_ID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    pi_ListModel.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    pi_ListModel.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        //ViewBag.MenuPageName = getDocumentName();
                        pi_ListModel.Title = title;
                        pi_ListModel.DocumentMenuId = DocumentMenuId;
                        ViewBag.CostCenterData = ds.Tables[12];
                        ViewBag.ItemDetails = ds.Tables[1];
                        //ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        return View("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/LocalPurchaseInvoiceDetail.cshtml", pi_ListModel);
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        pi_ListModel.Title = title;
                        //Session["DocumentStatus"] = "";
                        pi_ListModel.DocumentStatus = "";
                        //ViewBag.VBRoleList = GetRoleList();
                        pi_ListModel.DocumentMenuId = DocumentMenuId;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        return View("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/LocalPurchaseInvoiceDetail.cshtml", pi_ListModel);
                    }
                }
                else
                {/*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
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
                    DomesticPurchaseInvoice_Model DomesticPurchaseInvoice_Model = new DomesticPurchaseInvoice_Model();
                    PI_ListModel _PI_ListModels = new PI_ListModel();
                    if (menuDocumentID != null)
                    {
                        //pi_ListModel.DocumentMenuId = menuDocumentID;

                        DocumentMenuId = menuDocumentID;
                    }
                    else
                    {
                        menuDocumentID = _PI_ListModels.DocumentMenuId;
                    }
                    
                    DocumentMenuId = menuDocumentID;
                    _PI_ListModels.DocumentMenuId = menuDocumentID;
                    _PI_ListModels.UserID = User_ID;
                    CommonPageDetails();
                    HeaderPI_Detail HeaderPI_Detail = new HeaderPI_Detail();
                    //var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    GetAutoCompleteSearchSuppList(_PI_ListModels);

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.grn_no = "---Select---";
                    _DocumentNumber.grn_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);
                    _PI_ListModels.GRNNumberList = _DocumentNumberList;

                    List<CurrancyList> currancyLists = new List<CurrancyList>();
                    currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                    _PI_ListModels.currancyLists = currancyLists;

                    //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    //string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    //ViewBag.ValDigit = ValDigit;
                    //ViewBag.RateDigit = RateDigit;
                    //ViewBag.QtyDigit = QtyDigit;
                    SetDecimals(_PI_ListModels);
                    //DataTable dt = _Common_IServices.GetBaseCurrency(CompID).Tables[0]; /*created by suraj on 02-12-2022 for test*/
                    //if (dt.Rows.Count > 0)
                    //{
                    //    string baseCurrency = dt.Rows[0]["bs_curr_id"].ToString();
                    //    ViewBag.CurrInBase = "Y";
                    //}
                    if(TempData["wf_status1"] != null && TempData["wf_status1"].ToString() != "")
                    {
                        _PI_ListModels.WF_Status1 = TempData["wf_status1"].ToString();
                    }
                    else
                    {
                        _PI_ListModels.WF_Status1 = WF_Status1;
                    }
                    if (TempData["FilterData"] != null && TempData["FilterData"].ToString() != "")
                    {
                        _PI_ListModels.FilterData1 = TempData["FilterData"].ToString();
                    }
                    _PI_ListModels.TransType = TransType;
                    _PI_ListModels.Command = Command;
                    _PI_ListModels.BtnName = BtnName;                 
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_PI_ListModels.TransType == "Update" || _PI_ListModels.TransType == "Edit")
                    {
                        //string PI_No = Session["Inv_No"].ToString();
                        //string PI_Date = Session["Inv_Dt"].ToString();
                        string VouType = "PV";
                        string PI_No = Inv_no;
                        string PI_Date = Inv_dt;
                        string Type = "D";
                        DataSet ds = _DomesticPurchaseInvoiceIService.Edit_PIDetail(CompID, BrchID, VouType, PI_No, PI_Date, Type, User_ID, DocumentMenuId);
                        ViewBag.AttechmentDetails = ds.Tables[8];

                        /* Added by Suraj Maurya on 05-04-2025 */
                        _PI_ListModels.var_qty_dtl = Json(JsonConvert.SerializeObject(ds.Tables[16]), JsonRequestBehavior.AllowGet).Data.ToString();
                        _PI_ListModels.var_qty_tax_dtl = Json(JsonConvert.SerializeObject(ds.Tables[17]), JsonRequestBehavior.AllowGet).Data.ToString();
                        /* Added by Suraj Maurya on 05-04-2025 */

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
                        ViewBag.SubItemDetails = ds.Tables[13];
                        ViewBag.ItemTDSDetails = ds.Tables[14];
                        ViewBag.ItemOC_TDSDetails = ds.Tables[15];
                        _PI_ListModels.PI_inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        _PI_ListModels.PI_inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                        _PI_ListModels.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _PI_ListModels.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                        _PI_ListModels.hdnbillno = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        _PI_ListModels.hdnbilldt = ds.Tables[0].Rows[0]["BillDate"].ToString();
                        _PI_ListModels.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                        string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _PI_ListModels.SuppCurrency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        currancyLists.Add(new CurrancyList { curr_id = _PI_ListModels.SuppCurrency, curr_name = Curr_name });
                        _PI_ListModels.currancyLists = currancyLists;
                        //_PI_ListModels.SuppCurrency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _PI_ListModels.ExRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(_PI_ListModels.QtyDigit);
                        _PI_ListModels.PI_SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        _PI_ListModels.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                        _PI_ListModels.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _PI_ListModels.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _PI_ListModels.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _PI_ListModels.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _PI_ListModels.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _PI_ListModels.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _PI_ListModels.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _PI_ListModels.Status_name = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                        _PI_ListModels.PriceBasis = ds.Tables[0].Rows[0]["price_basis"].ToString();
                        _PI_ListModels.FreightType = ds.Tables[0].Rows[0]["freight_type"].ToString();
                        _PI_ListModels.ModeOfTransport = ds.Tables[0].Rows[0]["mode_trans"].ToString();
                        _PI_ListModels.Destination = ds.Tables[0].Rows[0]["dest"].ToString();
                        _PI_ListModels.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                        _PI_ListModels.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                        _PI_ListModels.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        _PI_ListModels.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                        _PI_ListModels.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        _PI_ListModels.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(_PI_ListModels.ValDigit);
                        _PI_ListModels.GrossValueInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val_bs"]).ToString(_PI_ListModels.ValDigit);
                        _PI_ListModels.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(_PI_ListModels.ValDigit);
                        _PI_ListModels.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(_PI_ListModels.ValDigit);
                        _PI_ListModels.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val"]).ToString(_PI_ListModels.ValDigit);
                        _PI_ListModels.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(_PI_ListModels.ValDigit);
                        _PI_ListModels.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                        _PI_ListModels.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                        _PI_ListModels.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                        _PI_ListModels.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                        var state_code = ds.Tables[0].Rows[0]["state_code"];
                        var br_state_code = ds.Tables[0].Rows[0]["br_state_code"];
                        //if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                        if (state_code.ToString() == br_state_code.ToString())
                        {
                            _PI_ListModels.Hd_GstType = "Both";
                        }
                        else
                        {
                            _PI_ListModels.Hd_GstType = "IGST";
                        }


                        _PI_ListModels.GRNNumber = "---Select---";

                        //if (DocumentMenuId == "105101140125")
                        //{
                            List<DocumentNumber> _DocumentNumberListForImp = new List<DocumentNumber>();
                            _DocumentNumber.grn_no = ds.Tables[0].Rows[0]["mr_no"].ToString();
                            _DocumentNumber.grn_dt = ds.Tables[0].Rows[0]["mr_date"].ToString();
                            _DocumentNumberListForImp.Add(_DocumentNumber);
                            _PI_ListModels.GRNNumberList = _DocumentNumberListForImp;
                            _PI_ListModels.GRNDate = ds.Tables[0].Rows[0]["mr_date"].ToString();
                        //}
                        //else
                        //{
                        //    _DocumentNumber.grn_no = _PI_ListModels.GRNNumber;
                        //    _DocumentNumber.grn_dt = _PI_ListModels.GRNNumber;
                        //    _DocumentNumberList.Add(_DocumentNumber);
                        //    _PI_ListModels.GRNNumberList = _DocumentNumberList;
                        //}


                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["inv_status1"].ToString().Trim();
                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        _PI_ListModels.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        if (roundoff_status == "Y")
                        {
                            _PI_ListModels.RoundOffFlag = true;
                        }
                        else
                        {
                            _PI_ListModels.RoundOffFlag = false;
                        }
                        string RCMApplicable = ds.Tables[0].Rows[0]["rcm_app"].ToString().Trim();
                        if (RCMApplicable == "Y")
                        {
                            _PI_ListModels.RCMApplicable = true;
                        }
                        else
                        {
                            _PI_ListModels.RCMApplicable = false;
                        }
                        _PI_ListModels.doc_status = doc_status;
                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            if (doc_status == "A" || doc_status == "C")
                            {
                                _PI_ListModels.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                            }
                            _PI_ListModels.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                            _PI_ListModels.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                            ViewBag.GLVoucherNo = _PI_ListModels.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                        }


                        //Session["DocumentStatus"] = doc_status;
                        _PI_ListModels.DocumentStatus = doc_status;
                        if (doc_status == "C")
                        {
                            _PI_ListModels.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _PI_ListModels.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _PI_ListModels.BtnName = "Refresh";
                        }
                        else
                        {
                            _PI_ListModels.CancelFlag = false;
                        }
                        _PI_ListModels.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _PI_ListModels.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _PI_ListModels.Command != "Edit")
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
                                if (create_id != User_ID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _PI_ListModels.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == User_ID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _PI_ListModels.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _PI_ListModels.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (User_ID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PI_ListModels.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == User_ID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _PI_ListModels.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (User_ID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PI_ListModels.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == User_ID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PI_ListModels.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == User_ID || approval_id == User_ID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _PI_ListModels.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _PI_ListModels.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        //ViewBag.MenuPageName = getDocumentName();
                        _PI_ListModels.Title = title;
                        _PI_ListModels.DocumentMenuId = DocumentMenuId;
                        ViewBag.CostCenterData = ds.Tables[12];
                        ViewBag.ItemDetails = ds.Tables[1];
                        //ViewBag.VBRoleList = GetRoleList();
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        //return View("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/LocalPurchaseInvoiceDetail.cshtml", _PI_ListModels);
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        _PI_ListModels.Title = title;
                        //Session["DocumentStatus"] = "";
                        _PI_ListModels.DocumentStatus = "";
                        //ViewBag.VBRoleList = GetRoleList();
                        _PI_ListModels.DocumentMenuId = DocumentMenuId;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                       
                    }
                    return View("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/LocalPurchaseInvoiceDetail.cshtml", _PI_ListModels);
                }
            }
            
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetPurchaseInvoiceDashbrd(string docid, string status)
        {
            PI_ListModel _PI_ListModel =new PI_ListModel();
            //_PI_ListModel.DocumentMenuId = docid;        
            TempData["DocumentID"] = docid;
            //Session["MenuDocumentId"] = docid;
            //Session["WF_status"] = status;
            //Session["WF_Docid"] = docid;
            //_PI_ListModel.WF_Status = status;
            TempData["WF_Status"] = status;
            TempData["docid"] = docid;
            TempData["ModelData"] = _PI_ListModel;
            if (docid == "105101145")
            {
                return RedirectToAction("LocalPurchaseInvoiceList", "LocalPurchaseInvoice");
            }
            else
            {
                return RedirectToAction("ImportPurchaseInvoiceList", "LocalPurchaseInvoice");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionPIDeatils(PI_ListModel _PI_ListModel, string command)
        {
            try
            {/*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                
                /*End to chk Financial year exist or not*/
                var Inv_no = "";
                var Inv_dt = "";
                var TransType = "";
                var BtnName = "";
                var Command = "";
                var menuDocumentID = "";
                if (_PI_ListModel.PI_DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        PI_ListModel _PI_ListModelAddnew = new PI_ListModel();
                        _PI_ListModelAddnew.Message = null;
                        _PI_ListModelAddnew.AppStatus = "D";
                        _PI_ListModelAddnew.BtnName= "BtnAddNew";
                        _PI_ListModelAddnew.TransType = "Save";
                        _PI_ListModelAddnew.Command = "New";
                        _PI_ListModelAddnew.DocumentMenuId = _PI_ListModel.DocumentMenuId;
                        menuDocumentID = _PI_ListModelAddnew.DocumentMenuId;                                          
                        TransType = _PI_ListModelAddnew.TransType;
                        BtnName = _PI_ListModelAddnew.BtnName;
                        Command = _PI_ListModelAddnew.Command;
                        TempData["ModelData"] = _PI_ListModelAddnew;
                        TempData["FilterData"] = null;
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                             if (!string.IsNullOrEmpty(_PI_ListModel.PI_inv_no))
                                return RedirectToAction("EditPurchaseInvoice", new { Inv_no = _PI_ListModel.PI_inv_no, Inv_dt = _PI_ListModel.PI_inv_dt, FilterData = _PI_ListModel.FilterData1, DocumentMenuId = _PI_ListModel.DocumentMenuId, WF_status = _PI_ListModel.WFStatus });
                            else
                                _PI_ListModelAddnew.Command = "Refresh";
                            _PI_ListModelAddnew.TransType = "Refresh";
                            _PI_ListModelAddnew.BtnName = "Refresh";
                            _PI_ListModelAddnew.DocumentStatus = null;
                            TempData["ModelData"] = _PI_ListModelAddnew;
                            return RedirectToAction("LocalPurchaseInvoiceDetail", "LocalPurchaseInvoice", _PI_ListModelAddnew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("LocalPurchaseInvoiceDetail", "LocalPurchaseInvoice",new { BtnName= BtnName, Command,TransType, menuDocumentID });

                    case "Edit":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPurchaseInvoice", new { Inv_no = _PI_ListModel.PI_inv_no, Inv_dt = _PI_ListModel.PI_inv_dt, FilterData = _PI_ListModel.FilterData1, DocumentMenuId = _PI_ListModel.DocumentMenuId, WF_status = _PI_ListModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                        string PI_invdt = _PI_ListModel.PI_inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PI_invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPurchaseInvoice", new { Inv_no = _PI_ListModel.PI_inv_no, Inv_dt = _PI_ListModel.PI_inv_dt, FilterData = _PI_ListModel.FilterData1, DocumentMenuId = _PI_ListModel.DocumentMenuId, WF_status = _PI_ListModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/

                        if (_PI_ListModel.doc_status == "A")
                        {
                            string checkforCancle = CheckPIForCancellationinReturn(_PI_ListModel.PI_inv_no, _PI_ListModel.PI_inv_dt);
                            if (checkforCancle != "")
                            {
                                //Session["Message"] = checkforCancle;
                                _PI_ListModel.Message = checkforCancle;
                                _PI_ListModel.BtnName = "BtnToDetailPage";
                                TempData["ModelData"] = _PI_ListModel;
                                _PI_ListModel.Command = "Refresh";
                                TempData["FilterData"] = _PI_ListModel.FilterData1;
                            }
                            else
                            {
                                _PI_ListModel.TransType = "Update";
                                _PI_ListModel.Command = command;
                                _PI_ListModel.BtnName = "BtnEdit";
                                _PI_ListModel.Message = null;

                                TempData["ModelData"] = _PI_ListModel;
                                TempData["FilterData"] = _PI_ListModel.FilterData1;
                            }
                        }
                        else 
                        {
                            _PI_ListModel.TransType = "Update";
                            _PI_ListModel.Command = command;
                            _PI_ListModel.BtnName = "BtnEdit";
                            _PI_ListModel.Message = null;
                             Inv_no = _PI_ListModel.PI_inv_no;
                             Inv_dt = _PI_ListModel.PI_inv_dt;
                             TransType = _PI_ListModel.TransType;
                             BtnName = _PI_ListModel.BtnName;
                             Command = _PI_ListModel.Command;
                            menuDocumentID = _PI_ListModel.DocumentMenuId;
                            TempData["ModelData"] = _PI_ListModel;
                            TempData["FilterData"] = _PI_ListModel.FilterData1;
                        }
                        return RedirectToAction("LocalPurchaseInvoiceDetail",new { Inv_no = _PI_ListModel.PI_inv_no, Inv_dt=_PI_ListModel.PI_inv_dt, TransType= _PI_ListModel.TransType, BtnName= _PI_ListModel.BtnName, Command = command, menuDocumentID=_PI_ListModel.DocumentMenuId } );
                    case "Delete":
                        PI_ListModel _PI_ListModeldelete = new PI_ListModel();                     
                        _PI_ListModel.Command = command;
                        _PI_ListModel.BtnName = "Refresh";
                        Inv_No = _PI_ListModel.PI_inv_no;
                        string InvType = _PI_ListModel.OrderType;
                        DeletePI_Details(_PI_ListModel, InvType);
                        _PI_ListModeldelete.Message = "Deleted";
                        _PI_ListModeldelete.Command = "Refresh";
                        _PI_ListModeldelete.PI_inv_no = null;
                        _PI_ListModeldelete.PI_inv_dt = null;
                        _PI_ListModeldelete.TransType = "Refresh";
                        _PI_ListModeldelete.AppStatus = "D";
                        _PI_ListModeldelete.BtnName = "BtnDelete";                        
                        _PI_ListModeldelete.DocumentMenuId = _PI_ListModel.DocumentMenuId;
                        _PI_ListModeldelete.WF_Status1 = _PI_ListModel.WF_Status1;
                        TempData["ModelData"] = _PI_ListModeldelete;
                        menuDocumentID = _PI_ListModeldelete.DocumentMenuId;
                        TransType = _PI_ListModeldelete.TransType;
                        BtnName = _PI_ListModeldelete.BtnName;
                        Command = _PI_ListModeldelete.Command;
                        TempData["FilterData"] = _PI_ListModel.FilterData1;
                        return RedirectToAction("LocalPurchaseInvoiceDetail", new { Inv_no = TransType, BtnName, Command, menuDocumentID });

                    case "Save":
                        _PI_ListModel.Command = command;
                        string checkforCancle_onSave = CheckPIForCancellationinReturn(_PI_ListModel.PI_inv_no, _PI_ListModel.PI_inv_dt);
                        if (checkforCancle_onSave != "")//Added by Suraj on 21-09-2024 to check before cancel
                        {
                            _PI_ListModel.Message = checkforCancle_onSave;
                            _PI_ListModel.BtnName = "BtnToDetailPage";
                            TempData["ModelData"] = _PI_ListModel;

                            TempData["FilterData"] = _PI_ListModel.FilterData1;
                        }
                        else
                        {
                            SavePurchaseInvoice(_PI_ListModel);
                        }
                        
                        if (_PI_ListModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_PI_ListModel.Message == "DocModify")
                        {
                            DocumentMenuId = _PI_ListModel.DocumentMenuId;
                           

                            CommonPageDetails();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";

                            //var other = new CommonController(_Common_IServices);
                            //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);

                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = _PI_ListModel.PI_SuppID, supp_name = _PI_ListModel.PI_SuppName });
                            _PI_ListModel.SupplierNameList = suppLists;

                            List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                            DocumentNumber _DocumentNumber = new DocumentNumber();
                            _DocumentNumber.grn_no = "---Select---";
                            _DocumentNumber.grn_dt = "0";
                            _DocumentNumberList.Add(_DocumentNumber);
                            _PI_ListModel.GRNNumberList = _DocumentNumberList;

                            List<CurrancyList> currancyLists = new List<CurrancyList>();
                            currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                            _PI_ListModel.currancyLists = currancyLists;


                            _PI_ListModel.PI_inv_dt = DateTime.Now.ToString();
                            _PI_ListModel.bill_no = _PI_ListModel.bill_no;
                            _PI_ListModel.bill_date = _PI_ListModel.bill_date;
                            _PI_ListModel.PI_SuppName = _PI_ListModel.PI_SuppName;
                            _PI_ListModel.Address = _PI_ListModel.Address;
                            CommonPageDetails();
                            ViewBag.GstApplicable = ViewBag.GstApplicable;
                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.OCTaxDetails = ViewData["TaxDetails"];
                            ViewBag.GLAccount =ViewData["VouDetail"];
                            //ViewBag.GLTOtal = ViewData["VouDetail"];
                            ViewBag.CostCenterData = ViewData["CCdetail"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _PI_ListModel.BtnName = "Refresh";
                            _PI_ListModel.Command = "Refresh";
                            _PI_ListModel.DocumentStatus = "D";

                            //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            //string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            //ViewBag.ValDigit = ValDigit;
                            //ViewBag.QtyDigit = QtyDigit;
                            //ViewBag.RateDigit = RateDigit;
                            SetDecimals(_PI_ListModel);
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/LocalPurchaseInvoiceDetail.cshtml", _PI_ListModel);

                        }
                        else
                        {

                            TempData["FilterData"] = _PI_ListModel.FilterData1;
                            Inv_no = _PI_ListModel.PI_inv_no;
                            Inv_dt = _PI_ListModel.PI_inv_dt;
                            TransType = _PI_ListModel.TransType;
                            BtnName = _PI_ListModel.BtnName;
                            Command = _PI_ListModel.Command;
                            menuDocumentID = _PI_ListModel.DocumentMenuId;
                            return RedirectToAction("LocalPurchaseInvoiceDetail", new { Inv_no = Inv_no, Inv_dt, TransType, BtnName, Command, menuDocumentID });
                        }
                    case "Forward":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPurchaseInvoice", new { Inv_no = _PI_ListModel.PI_inv_no, Inv_dt = _PI_ListModel.PI_inv_dt, FilterData = _PI_ListModel.FilterData1, DocumentMenuId = _PI_ListModel.DocumentMenuId, WF_status = _PI_ListModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                        string PI_invdt1 = _PI_ListModel.PI_inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PI_invdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPurchaseInvoice", new { Inv_no = _PI_ListModel.PI_inv_no, Inv_dt = _PI_ListModel.PI_inv_dt, FilterData = _PI_ListModel.FilterData1, DocumentMenuId = _PI_ListModel.DocumentMenuId, WF_status = _PI_ListModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/

                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPurchaseInvoice", new { Inv_no = _PI_ListModel.PI_inv_no, Inv_dt = _PI_ListModel.PI_inv_dt, FilterData = _PI_ListModel.FilterData1, DocumentMenuId = _PI_ListModel.DocumentMenuId, WF_status = _PI_ListModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
                        string PI_invdt2 = _PI_ListModel.PI_inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PI_invdt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPurchaseInvoice", new { Inv_no = _PI_ListModel.PI_inv_no, Inv_dt = _PI_ListModel.PI_inv_dt, FilterData = _PI_ListModel.FilterData1, DocumentMenuId = _PI_ListModel.DocumentMenuId, WF_status = _PI_ListModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _PI_ListModel.Command = command;
                        Approve_PurchaseInvoice(_PI_ListModel, _PI_ListModel.PI_inv_no, _PI_ListModel.PI_inv_dt, "", "", "", _PI_ListModel.Nurration, "","","",_PI_ListModel.BP_Nurration,_PI_ListModel.DN_Nurration,_PI_ListModel.DN_VarNurration);
                         Inv_no = _PI_ListModel.PI_inv_no;
                         Inv_dt = _PI_ListModel.PI_inv_dt;
                         TransType = _PI_ListModel.TransType;
                        BtnName = _PI_ListModel.BtnName;
                         Command = _PI_ListModel.Command;
                        var WF_Status1 = _PI_ListModel.WF_Status1;
                        menuDocumentID = _PI_ListModel.DocumentMenuId;
                        TempData["ModelData"] = _PI_ListModel;
                        TempData["FilterData"] = _PI_ListModel.FilterData1;
                        return RedirectToAction("LocalPurchaseInvoiceDetail", new { Inv_no = Inv_no, Inv_dt, TransType, BtnName, Command, menuDocumentID, WF_Status1 });

                    case "Refresh":
                        PI_ListModel _PI_ListModelrefresh = new PI_ListModel();
                        _PI_ListModelrefresh.BtnName= "Refresh";
                        _PI_ListModelrefresh.Command = command;
                        _PI_ListModelrefresh.TransType = "Save";
                        _PI_ListModelrefresh.Message = null;
                        _PI_ListModelrefresh.DocumentStatus = null;
                        _PI_ListModelrefresh.DocumentMenuId = _PI_ListModel.DocumentMenuId;
                        menuDocumentID = _PI_ListModelrefresh.DocumentMenuId;
                        TransType = _PI_ListModelrefresh.TransType;
                        BtnName = _PI_ListModelrefresh.BtnName;
                        Command = _PI_ListModelrefresh.Command;
                        TempData["ModelData"] = _PI_ListModelrefresh;
                        TempData["FilterData"] = _PI_ListModel.FilterData1;
                        return RedirectToAction("LocalPurchaseInvoiceDetail", new { Inv_no = TransType, BtnName, Command, menuDocumentID });

                    case "Print":
                        return GenratePdfFile(_PI_ListModel);
                    //return new EmptyResult();
                    case "BacktoList":
                        TempData["WF_Status"] = _PI_ListModel.WF_Status1;
                        TempData["FilterData"] = _PI_ListModel.FilterData1;
                        if(_PI_ListModel.DocumentMenuId == "105101145")
                        {
                            return RedirectToAction("LocalPurchaseInvoiceList", "LocalPurchaseInvoice");
                        }
                        else
                        {
                            return RedirectToAction("ImportPurchaseInvoiceList", "LocalPurchaseInvoice");
                        }
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
        public FileResult GenratePdfFile(PI_ListModel _model)
        {
            return File(GetPdfData(_model.DocumentMenuId, _model.PI_inv_no, _model.PI_inv_dt), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
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
                string inv_type = "";
                string ReportType = "common";
                string Command = "";
                if (docId == "105101145")
                {
                    inv_type = "D";
                }
                if (docId == "105101140125")
                {
                    inv_type = "I";
                }
                DataSet Details = _DomesticPurchaseInvoiceIService.GetPurchaseInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt, inv_type);
                ViewBag.PageName = "PI";
                string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                string htmlAutoDebitNote = "";
                if (invType == "D")
                {   ViewBag.Title = "Purchase Invoice";
                    Command = "Print";
                    Double DnAmount = Convert.ToDouble(Details.Tables[3].Rows[0]["dn_amount"].ToString());
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/PurchaseInvoicePrint.cshtml"));
                    if (DnAmount != 0)
                    {
                        ViewBag.Title = "Debit Note";
                        htmlAutoDebitNote = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/AutoDebitNotePrint.cshtml"));
                    }
                }
                else
                {
                    ViewBag.Title = "Import Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/LocalPurchaseInvoice/PurchaseInvoicePrint.cshtml"));
                }
                ViewBag.Title = "Purchase Invoice";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    if (Command == "Print")
                    {
                        reader = new StringReader(htmlAutoDebitNote);
                        pdfDoc.NewPage();
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    }
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
        public ActionResult SavePurchaseInvoice(PI_ListModel _PI_ListModel)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = _PI_ListModel.Title.Replace(" ", "");
            if (_PI_ListModel.DocumentMenuId != null)
            {
                if (_PI_ListModel.DocumentMenuId == "105101145")
                {
                    DocumentMenuId = "105101145";
                }
                if (_PI_ListModel.DocumentMenuId == "105101140125")
                {
                    DocumentMenuId = "105101140125";
                }
            }
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
                User_ID = Session["userid"].ToString();
            }
            try
            {
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblIOCDetail = new DataTable();
                DataTable DtblTdsDetail = new DataTable();
                DataTable DtblOcTdsDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable DtblVouDetail = new DataTable();
                DataTable CRCostCenterDetails = new DataTable();
                DataTable DtblVarDetail = new DataTable();
                DataTable DtblVarTaxDetail = new DataTable();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("roundoff", typeof(string));
                dtheader.Columns.Add("pm_flag", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(string));
                dtheader.Columns.Add("br_id", typeof(string));
                dtheader.Columns.Add("inv_type", typeof(string));
                dtheader.Columns.Add("inv_no", typeof(string));
                dtheader.Columns.Add("inv_dt", typeof(string));
                dtheader.Columns.Add("supp_id", typeof(string));
                dtheader.Columns.Add("bill_no", typeof(string));
                dtheader.Columns.Add("bill_dt", typeof(string));
                dtheader.Columns.Add("curr_id", typeof(string));
                dtheader.Columns.Add("conv_rate", typeof(string));
                dtheader.Columns.Add("user_id", typeof(string));
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("tax_amt_nrecov", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("Narration", typeof(string));
                dtheader.Columns.Add("price_basis", typeof(string));
                dtheader.Columns.Add("freight_type", typeof(string));
                dtheader.Columns.Add("mode_trans", typeof(string));
                dtheader.Columns.Add("dest", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));
                dtheader.Columns.Add("ewb_no", typeof(string));
                dtheader.Columns.Add("einv_no", typeof(string));
                dtheader.Columns.Add("gr_val_bs", typeof(string));/*Added by Suraj on 27-03-2024 for value (in base) */
                dtheader.Columns.Add("rcm_app", typeof(string));/*Added by Suraj on 27-03-2024 for value (in base) */
                dtheader.Columns.Add("dn_amt", typeof(string));/*Added by Suraj on 31-03-2025 for debit note amount*/
                dtheader.Columns.Add("cancel_remarks", typeof(string));/*Added by Nitesh on 04-09-2025 for debit note amount*/
                DataRow dtrowHeader = dtheader.NewRow();
                //dtrowHeader["TransType"] = Session["TransType"].ToString();
               if(_PI_ListModel.PI_inv_no!= null)
                {
                    _PI_ListModel.TransType = "Update";
                    dtrowHeader["TransType"] = _PI_ListModel.TransType;
                }
                else
                {
                    _PI_ListModel.TransType = "Save";
                    dtrowHeader["TransType"] = _PI_ListModel.TransType;
                }
                //dtrowHeader["TransType"] = _PI_ListModel.TransType;
                dtrowHeader["MenuID"] = DocumentMenuId;
                string cancelflag = _PI_ListModel.CancelFlag.ToString();
                if (cancelflag == "False")
                {
                    dtrowHeader["Cancelled"] = "N";
                }
                else
                {
                    dtrowHeader["Cancelled"] = "Y";
                }
                string roundoffflag = _PI_ListModel.RoundOffFlag.ToString();
                if (roundoffflag == "False")
                {
                    dtrowHeader["roundoff"] = "N";
                }
                else
                {
                    dtrowHeader["roundoff"] = "Y";
                }
                dtrowHeader["pm_flag"] = _PI_ListModel.pmflagval;
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["inv_type"] = _PI_ListModel.OrderType;
                dtrowHeader["inv_no"] = _PI_ListModel.PI_inv_no;
                dtrowHeader["inv_dt"] = _PI_ListModel.PI_inv_dt;
                dtrowHeader["supp_id"] = _PI_ListModel.PI_SuppID;
                dtrowHeader["bill_no"] = _PI_ListModel.hdnbillno;
                dtrowHeader["bill_dt"] = _PI_ListModel.hdnbilldt;
                dtrowHeader["curr_id"] = _PI_ListModel.SuppCurrency; 
                dtrowHeader["conv_rate"] = _PI_ListModel.ExRate; 
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                //dtrowHeader["inv_status"] = Session["AppStatus"].ToString();
                if(_PI_ListModel.AppStatus==null)
                {
                    _PI_ListModel.AppStatus = "D";
                    dtrowHeader["inv_status"] = _PI_ListModel.AppStatus;
                }
                else
                {
                    dtrowHeader["inv_status"] = _PI_ListModel.AppStatus;
                }
                
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["gr_val"] = _PI_ListModel.GrossValue;
                dtrowHeader["tax_amt_nrecov"] = _PI_ListModel.TaxAmount;
                dtrowHeader["oc_amt"] = _PI_ListModel.OtherCharges;
                dtrowHeader["net_val"] = _PI_ListModel.NetAmount;
                dtrowHeader["net_val_bs"] = _PI_ListModel.NetAmountInBase;
                dtrowHeader["Narration"] = _PI_ListModel.Narration;
                dtrowHeader["price_basis"] = _PI_ListModel.PriceBasis;
                dtrowHeader["freight_type"] = _PI_ListModel.FreightType;
                dtrowHeader["mode_trans"] = _PI_ListModel.ModeOfTransport;
                dtrowHeader["dest"] = _PI_ListModel.Destination;
                dtrowHeader["bill_add_id"] = _PI_ListModel.bill_add_id==null?"0": _PI_ListModel.bill_add_id;
                dtrowHeader["remarks"] = _PI_ListModel.remarks;
                dtrowHeader["ewb_no"] = _PI_ListModel.EWBNNumber;
                dtrowHeader["einv_no"] = _PI_ListModel.EInvoive;
                dtrowHeader["gr_val_bs"] = _PI_ListModel.GrossValueInBase;
                string RCMApplicable = _PI_ListModel.RCMApplicable.ToString();
                if (RCMApplicable == "False")
                {
                    dtrowHeader["rcm_app"] = "N";
                }
                else
                {
                    dtrowHeader["rcm_app"] = "Y";
                }
                dtrowHeader["dn_amt"] = string.IsNullOrEmpty(_PI_ListModel.var_dn_amt)?"0": _PI_ListModel.var_dn_amt;
                dtrowHeader["cancel_remarks"] = _PI_ListModel.CancelledRemarks;
                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;

                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("mr_no", typeof(string));
                dtItem.Columns.Add("mr_date", typeof(string));
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("mr_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val_spec", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("gl_vou_no", typeof(string));
                dtItem.Columns.Add("gl_vou_dt", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("claim_itc", typeof(string));
                dtItem.Columns.Add("item_gr_val_bs", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));
                dtItem.Columns.Add("item_var_dn_amt", typeof(string));
                dtItem.Columns.Add("sr_no", typeof(int));
                dtItem.Columns.Add("mrp", typeof(string));
                dtItem.Columns.Add("PackSize", typeof(string));

                JArray jObject = JArray.Parse(_PI_ListModel.ItemDetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["mr_no"] = jObject[i]["mr_no"].ToString();
                    dtrowLines["mr_date"] = jObject[i]["mr_date"].ToString();
                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"].ToString());
                    dtrowLines["mr_qty"] = jObject[i]["mr_qty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                    dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                    dtrowLines["item_net_val_spec"] = jObject[i]["item_net_val_spec"].ToString();
                    dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                    dtrowLines["gl_vou_no"] = jObject[i]["gl_vou_no"].ToString();
                    dtrowLines["gl_vou_dt"] = jObject[i]["gl_vou_dt"].ToString();
                    dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                    dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtrowLines["claim_itc"] = jObject[i]["ClaimITC"].ToString();
                    dtrowLines["item_gr_val_bs"] = jObject[i]["item_gr_val_bs"].ToString();
                    dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();
                    dtrowLines["item_var_dn_amt"] = jObject[i]["item_var_dn_amt"].ToString();
                    dtrowLines["sr_no"] = Convert.ToInt32(jObject[i]["srno"].ToString());
                    if(_PI_ListModel.DocumentMenuId== "105101140125")
                    {
                        dtrowLines["mrp"] = "0";
                        dtrowLines["PackSize"] = "";
                    }
                    else
                    {
                        dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                        dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();
                    }
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDetail = dtItem;
                ViewData["ItemDetails"] = dtitemdetail(jObject);
                DtblTaxDetail = ToDtblTaxDetail(_PI_ListModel.TaxDetail,"Y");
                DtblOCTaxDetail = ToDtblTaxDetail(_PI_ListModel.OC_TaxDetail,"N");

                DtblTdsDetail = ToDtblTdsDetail(_PI_ListModel.tds_details, "");
                DtblOcTdsDetail = ToDtblTdsDetail(_PI_ListModel.oc_tds_details, "OC");
                DtblVarDetail = ToDtblVarDetail(_PI_ListModel.var_qty_dtl);
                DtblVarTaxDetail = ToDtblVarTaxDetail(_PI_ListModel.var_qty_tax_dtl);

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
                if (_PI_ListModel.OCDetail != null)
                {
                    JArray jObjectOC = JArray.Parse(_PI_ListModel.OCDetail);
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
                        dtrowOCDetailsLines["tds_amt"] = jObjectOC[i]["tds_amt"].ToString()==""?"0": jObjectOC[i]["tds_amt"].ToString();
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
    

                JArray JAObj = JArray.Parse(_PI_ListModel.CC_DetailList);
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

                DataTable dtAttachment = new DataTable();
                var _PurchaseInvoiceattch = TempData["ModelDataattch"] as PurchaseInvoiceattch;
                //TempData["ModelDataattch"] = null;
                if (_PI_ListModel.attatchmentdetail != null)
                {
                    if (_PurchaseInvoiceattch != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_PurchaseInvoiceattch.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _PurchaseInvoiceattch.AttachMentDetailItmStp as DataTable;
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
                        if (_PI_ListModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _PI_ListModel.AttachMentDetailItmStp as DataTable;
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

                    JArray jObject1 = JArray.Parse(_PI_ListModel.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_PI_ListModel.PI_inv_no))
                            {
                                dtrowAttachment1["id"] = _PI_ListModel.PI_inv_no;
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
                    if (_PI_ListModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_PI_ListModel.PI_inv_no))
                            {
                                ItmCode = _PI_ListModel.PI_inv_no;
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
                
                DtblVouDetail = dtVouDetails(_PI_ListModel.vouDetail);/* DataTable Createtion Code Shifted to Shaperate method*/
                DataTable DtblSubItemDetail = ToDtblSubItem(_PI_ListModel.SubItemDetailsDt);
                string Nurr="";
                if (_PI_ListModel.CancelFlag)
                {
                    Nurr = _PI_ListModel.Nurration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }
                //if(DtblVouDetail.Select(" curr_id=0 or conv_rate='' or conv_rate=0 or conv_rate is null").Any())
                //{
                //}
                string tds_amt = _PI_ListModel.TDS_Amount==null?"0": _PI_ListModel.TDS_Amount;
                SaveMessage = _DomesticPurchaseInvoiceIService.InsertPI_Details(DtblHDetail, DtblItemDetail, DtblTaxDetail
                    , DtblOCTaxDetail, DtblIOCDetail, DtblAttchDetail, DtblVouDetail, CRCostCenterDetails, DtblSubItemDetail
                    , Nurr,DtblTdsDetail,DtblOcTdsDetail, tds_amt, DtblVarDetail, DtblVarTaxDetail);
                if (SaveMessage == "DocModify")
                {
                    _PI_ListModel.Message = "DocModify";
                    _PI_ListModel.BtnName = "Refresh";
                    _PI_ListModel.Command = "Refresh";
                    TempData["ModelData"] = _PI_ListModel;
                    return RedirectToAction("LocalPurchaseInvoiceDetail");
                }
                else
                {
                    string[] FDetail = SaveMessage.Split(',');

                    string Message = FDetail[5].ToString();
                    string Inv_no = FDetail[0].ToString();
                    string Inv_DATE = FDetail[6].ToString();
                    string Cansal = FDetail[1].ToString();
                    if (FDetail[2].ToString() == "Cancelled")
                    {
                        try
                        {
                            //string fileName = "PI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "PurchaseInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_PI_ListModel.PI_inv_no, _PI_ListModel.PI_inv_dt, fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _PI_ListModel.PI_inv_no, "Cancel", User_ID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _PI_ListModel.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                    }
                    if (Message == "DataNotFound")
                    {
                        var msg = "Data Not found" +" "+ Inv_DATE+" in "+PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _PI_ListModel.Message = Message;
                        return RedirectToAction("LocalPurchaseInvoiceDetail");
                    }
                    if (Message == "Save")
                    {
                        //string Guid = "";//commented by shubham Maurya on 08-07-2025

                        //if (Session["Guid"] != null)
                        //{
                        //    Guid = Session["Guid"].ToString();
                        //}
                        //if (_PurchaseInvoiceattch != null)//commented by shubham Maurya on 08-07-2025
                        //{
                        //    if (_PurchaseInvoiceattch.Guid != null)
                        //    {
                        //        Guid = _PurchaseInvoiceattch.Guid;
                        //    }
                        //}
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
                        //Session["Message"] = "Cancelled";
                        //Session["Command"] = "Update";
                        //Session["Inv_No"] = Inv_no;
                        //Session["Inv_Dt"] = Inv_DATE;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "Refresh";

                        //_PI_ListModel.Message = "Cancelled";
                        _PI_ListModel.Message = _PI_ListModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        _PI_ListModel.Command = "Update";
                        _PI_ListModel.PI_inv_no = Inv_no;
                        _PI_ListModel.PI_inv_dt = Inv_DATE;
                        _PI_ListModel.AppStatus = "D";
                        _PI_ListModel.BtnName = "Refresh";
                        _PI_ListModel.TransType = "Update";
                        TempData["ModelData"] = _PI_ListModel;
                        //Session["AttachMentDetailItmStp"] = null;
                        //Session["Guid"] = null;
                    }
                    else
                    {
                        if (Message == "Update" || Message == "Save")
                        {
                            //Session["Message"] = "Save";
                            //Session["Command"] = "Update";
                            //Session["Inv_No"] = Inv_no;
                            //Session["Inv_Dt"] = Inv_DATE;
                            //Session["TransType"] = "Update";
                            //Session["AppStatus"] = 'D';
                            //Session["BtnName"] = "BtnSave";
                            _PI_ListModel.Message = "Save";
                            _PI_ListModel.Command = "Update";
                            _PI_ListModel.PI_inv_no = Inv_no;
                            _PI_ListModel.PI_inv_dt = Inv_DATE;
                            _PI_ListModel.AppStatus = "D";

                            _PI_ListModel.BtnName = "BtnSave";
                            _PI_ListModel.TransType = "Update";
                            TempData["ModelData"] = _PI_ListModel;
                            //Session["AttachMentDetailItmStp"] = null;
                            //Session["Guid"] = null;
                        }
                    }
                    var _attachModel = TempData["ModelDataattch"] as PurchaseInvoiceattch;
                    TempData["ModelDataattch"] = null;
                    string guid = "";
                    if (_attachModel != null)
                        guid = _attachModel.Guid;
                    var comCont = new CommonController(_Common_IServices);
                    comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, _PI_ListModel.TransType, DtblAttchDetail);
                    return RedirectToAction("LocalPurchaseInvoiceDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_PI_ListModel.TransType == "Save")
                    {
                        string Guid = "";
                        if (_PI_ListModel.Guid != null)
                        {
                            Guid = _PI_ListModel.Guid;
                        }
                        //if (Session["Guid"] != null)
                        //{
                        //    Guid = Session["Guid"].ToString();
                        //}
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public DataTable dtitemdetail(JArray jObject)
        {

            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("mr_no", typeof(string));
            dtItem.Columns.Add("mr_date", typeof(string));
            dtItem.Columns.Add("mr_dt", typeof(string));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_alias", typeof(string));
            dtItem.Columns.Add("mr_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_gr_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_spec", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("gl_vou_no", typeof(string));
            dtItem.Columns.Add("gl_vou_dt", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("tmplt_id", typeof(string));
            dtItem.Columns.Add("claim_itc", typeof(string));
            dtItem.Columns.Add("item_gr_val_bs", typeof(string));
            dtItem.Columns.Add("item_acc_id", typeof(string));



            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["mr_no"] = jObject[i]["mr_no"].ToString();
                dtrowLines["mr_date"] = jObject[i]["mr_date"].ToString();
                dtrowLines["mr_dt"] = jObject[i]["mr_date"].ToString();
                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"].ToString());
                dtrowLines["uom_alias"] = jObject[i]["uom_name"].ToString();
                dtrowLines["mr_qty"] = jObject[i]["mr_qty"].ToString();
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
                //dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                dtrowLines["item_net_val_spec"] = jObject[i]["item_net_val_spec"].ToString();
                dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                dtrowLines["gl_vou_no"] = jObject[i]["gl_vou_no"].ToString();
                dtrowLines["gl_vou_dt"] = jObject[i]["gl_vou_dt"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                dtrowLines["tmplt_id"] = "0";
                dtrowLines["claim_itc"] = jObject[i]["ClaimITC"].ToString();
                dtrowLines["item_gr_val_bs"] = jObject[i]["item_gr_val_bs"].ToString();
                dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();//Added by Suraj on 08-08-2024 save item gl account
                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtVouDetails(string vouDetail)
        {
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
            if (vouDetail != null)
            {
                JArray jObjectVOU = JArray.Parse(vouDetail);
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
            return vou_Details;
        }
        public DataTable dtTaxdetail(JArray jObjectTax)
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


                Tax_detail.Rows.Add(dtrowTaxDetailsLines);
            }

            return Tax_detail;
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
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                dtrowLines["acc_id"] = JAObj[i]["GlAccountId"].ToString();
                dtrowLines["cc_id"] = JAObj[i]["CstCntrTypeId"].ToString();
                dtrowLines["cc_val_id"] = JAObj[i]["CstNameId"].ToString();
                dtrowLines["cc_amt"] = JAObj[i]["CstAmt"].ToString();
                dtrowLines["cc_name"] = JAObj[i]["ddl_CC_Name"].ToString();
                dtrowLines["cc_val_name"] = JAObj[i]["ddl_CC_Type"].ToString();

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

        private DataTable ToDtblSubItem(string SubitemDetails)
        {
            try
            {
                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));
                dtSubItem.Columns.Add("src_doc_number", typeof(string));
                dtSubItem.Columns.Add("src_doc_date", typeof(string));

                if (!string.IsNullOrEmpty(SubitemDetails))
                {
                    JArray jObject2 = JArray.Parse(SubitemDetails);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["qty"] = jObject2[i]["Qty"].ToString();
                        dtrowItemdetails["src_doc_number"] = jObject2[i]["src_doc_number"].ToString();
                        dtrowItemdetails["src_doc_date"] = jObject2[i]["src_doc_date"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                    ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                }

                /*------------------Sub Item end----------------------*/
                return dtSubItem;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("Qty", typeof(string));
            dtSubItem.Columns.Add("src_doc_number", typeof(string));
            dtSubItem.Columns.Add("src_doc_date", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["Qty"] = jObject2[i]["Qty"].ToString();
                dtrowItemdetails["src_doc_number"] = jObject2[i]["src_doc_number"].ToString();
                dtrowItemdetails["src_doc_date"] = jObject2[i]["src_doc_date"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        public string CheckPIForCancellationinReturn(string DocNo, string DocDate)
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
                DataSet Deatils = _DomesticPurchaseInvoiceIService.CheckPIDetail(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = "InvoiceCannotbeModified";
                }
                if (Deatils.Tables[1].Rows.Count > 0)
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
                DataSet GlDt = _DomesticPurchaseInvoiceIService.GetAllGLDetails(DtblGLDetail);
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
        [HttpPost]
        public ActionResult GetGLDetailsPI(List<GL_Detail> GLDetail)
        {
            //JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                DataTable DtblGLDetail = new DataTable();

                if (GLDetail != null)
                {
                    DtblGLDetail = ToDataTable(GLDetail);
                }
                DataSet GlDt = _DomesticPurchaseInvoiceIService.GetAllGLDetails(DtblGLDetail);
                //Validate = Json(GlDt);
                //JsonResult DataRows = null;
                //DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);
                ViewBag.GL_Detail = GlDt;
                return PartialView("~/Areas/Common/Views/Cmn_PartialGLAccountDetail.cshtml");
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
        private DataTable ToDtblTaxDetail(string TaxDetails,string recov)
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
                Tax_detail.Columns.Add("tax_recov", typeof(string));
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
                        dtrowTaxDetailsLines["tax_recov"] = recov=="Y" ? jObjectTax[i]["tax_recov"].ToString():"";
                        Tax_detail.Rows.Add(dtrowTaxDetailsLines);
                    }
                    ViewData["TaxDetails"] = dtTaxdetail(jObjectTax);
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
                    //ViewData["TDSDetails"] = dtTDSdetail(jObjecttds);
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
        private DataTable ToDtblVarDetail(string varDetails)
        {
            try
            {
                DataTable var_detail = new DataTable();
                var_detail.Columns.Add("sr_no", typeof(string));
                var_detail.Columns.Add("dn_no", typeof(string));
                var_detail.Columns.Add("dn_date", typeof(string));
                var_detail.Columns.Add("src_doc_number", typeof(string));
                var_detail.Columns.Add("src_doc_date", typeof(string));
                var_detail.Columns.Add("item_id", typeof(string));
                var_detail.Columns.Add("var_type", typeof(string));
                var_detail.Columns.Add("var_qty", typeof(string));
                var_detail.Columns.Add("price", typeof(string));
                var_detail.Columns.Add("value", typeof(string));
                var_detail.Columns.Add("tax_amt", typeof(string));
                var_detail.Columns.Add("net_amt", typeof(string));
                var_detail.Columns.Add("dn_amt", typeof(string));
                var_detail.Columns.Add("include", typeof(string));

                if (varDetails != null)
                {
                    JArray jObjectvar = JArray.Parse(varDetails);
                    for (int i = 0; i < jObjectvar.Count; i++)
                    {
                        DataRow dtrowvarDetailsLines = var_detail.NewRow();
                        dtrowvarDetailsLines["sr_no"] = jObjectvar[i]["sr_no"].ToString();
                        dtrowvarDetailsLines["dn_no"] = jObjectvar[i]["dn_no"].ToString();
                        dtrowvarDetailsLines["dn_date"] = jObjectvar[i]["dn_date"].ToString();
                        dtrowvarDetailsLines["src_doc_number"] = jObjectvar[i]["mr_no"].ToString();
                        dtrowvarDetailsLines["src_doc_date"] = jObjectvar[i]["mr_date"].ToString();
                        dtrowvarDetailsLines["item_id"] = jObjectvar[i]["item_id"].ToString();
                        dtrowvarDetailsLines["var_type"] = jObjectvar[i]["var_type"].ToString();
                        dtrowvarDetailsLines["var_qty"] = jObjectvar[i]["qty"].ToString();
                        dtrowvarDetailsLines["price"] = jObjectvar[i]["price"].ToString();
                        dtrowvarDetailsLines["value"] = jObjectvar[i]["value"].ToString();
                        dtrowvarDetailsLines["tax_amt"] = jObjectvar[i]["tax"].ToString();
                        dtrowvarDetailsLines["net_amt"] = jObjectvar[i]["net_amt"].ToString();
                        dtrowvarDetailsLines["dn_amt"] = jObjectvar[i]["dn_amt"].ToString();
                        dtrowvarDetailsLines["include"] = jObjectvar[i]["include"].ToString();

                        var_detail.Rows.Add(dtrowvarDetailsLines);
                    }
                    //ViewData["varDetails"] = dtvardetail(jObjectvar);
                }
                
                return var_detail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblVarTaxDetail(string varTaxDetails)
        {
            try
            {
                DataTable var_tax_detail = new DataTable();
                var_tax_detail.Columns.Add("src_doc_number", typeof(string));
                var_tax_detail.Columns.Add("src_doc_date", typeof(string));
                var_tax_detail.Columns.Add("item_id", typeof(string));
                var_tax_detail.Columns.Add("var_type", typeof(string));
                var_tax_detail.Columns.Add("tax_id", typeof(string));
                var_tax_detail.Columns.Add("tax_rate", typeof(string));
                var_tax_detail.Columns.Add("tax_val", typeof(string));
                var_tax_detail.Columns.Add("tax_level", typeof(string));
                var_tax_detail.Columns.Add("tax_apply_on", typeof(string));

                if (varTaxDetails != null)
                {
                    JArray jObjectvar_tax = JArray.Parse(varTaxDetails);
                    for (int i = 0; i < jObjectvar_tax.Count; i++)
                    {
                        DataRow dtrowVarTaxDetailsLines = var_tax_detail.NewRow();
                        dtrowVarTaxDetailsLines["src_doc_number"] = jObjectvar_tax[i]["mr_no"].ToString();
                        dtrowVarTaxDetailsLines["src_doc_date"] = jObjectvar_tax[i]["mr_date"].ToString();
                        dtrowVarTaxDetailsLines["item_id"] = jObjectvar_tax[i]["item_id"].ToString();
                        dtrowVarTaxDetailsLines["var_type"] = jObjectvar_tax[i]["var_type"].ToString();
                        dtrowVarTaxDetailsLines["tax_id"] = jObjectvar_tax[i]["tax_id"].ToString();
                        dtrowVarTaxDetailsLines["tax_rate"] = jObjectvar_tax[i]["tax_rate"].ToString();
                        dtrowVarTaxDetailsLines["tax_val"] = jObjectvar_tax[i]["tax_val"].ToString();
                        dtrowVarTaxDetailsLines["tax_level"] = jObjectvar_tax[i]["tax_level"].ToString();
                        dtrowVarTaxDetailsLines["tax_apply_on"] = jObjectvar_tax[i]["tax_apply_on"].ToString();

                        var_tax_detail.Rows.Add(dtrowVarTaxDetailsLines);
                    }
                    //ViewData["varDetails"] = dtvardetail(jObjectvar);
                }

                return var_tax_detail;
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
        public ActionResult Approve_PurchaseInvoice(PI_ListModel _PI_ListModel, string Inv_No, string Inv_Date
            , string A_Status, string A_Level, string A_Remarks, string VoucherNarr,string FilterData
            ,string docid,string WF_Status1,string Bp_Nurr,string Dn_Nurration,string DN_VarNarr)
        {
            //JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            //JsonResult DataRows = null;
            try
            {
                //PI_ListModel _PI_ListModel = new PI_ListModel();              
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
                
                if (_PI_ListModel.DocumentMenuId != null)
                {
                    MenuDocId = _PI_ListModel.DocumentMenuId;
                }
                else
                {
                    MenuDocId = docid;
                }
                string Dn_Nurr = _PI_ListModel.DN_Nurration==null? Dn_Nurration: _PI_ListModel.DN_Nurration;
                string DN_VarianceNarr = _PI_ListModel.DN_VarNurration==null? DN_VarNarr : _PI_ListModel.DN_VarNurration;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string InvType = "D";
                string Message = _DomesticPurchaseInvoiceIService.Approve_PI(Inv_No, Inv_Date, InvType, MenuDocId, BranchID
                    , Comp_ID, UserID, mac_id, A_Status, A_Level, A_Remarks, VoucherNarr,Bp_Nurr, Dn_Nurr, DN_VarianceNarr);
                try
                {
                    //string fileName = Inv_No + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "PurchaseInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(Inv_No, Inv_Date,fileName, MenuDocId,"AP");
                    _Common_IServices.SendAlertEmail(CompID, BrchID, MenuDocId, Inv_No, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _PI_ListModel.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                string[] FDetail = Message.Split(',');
                string ApMessage = FDetail[6].ToString().Trim();
                string INV_NO = FDetail[0].ToString();
                string INV_DT = FDetail[7].ToString();
  
                if (ApMessage == "A")
                {
                    //_PI_ListModel.Message = "Approved";
                    _PI_ListModel.Message = _PI_ListModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                _PI_ListModel.TransType = "Update";
                _PI_ListModel.Command = "Update";
                _PI_ListModel.PI_inv_no = INV_NO;
                _PI_ListModel.PI_inv_dt = INV_DT;
                _PI_ListModel.AppStatus = "D";
                if(docid!=null && docid != "")
                {
                    _PI_ListModel.DocumentMenuId = docid;
                }
                _PI_ListModel.WF_Status1 = WF_Status1;
                _PI_ListModel.BtnName = "BtnEdit";
                var Inv_no = _PI_ListModel.PI_inv_no;
                var Inv_dt = _PI_ListModel.PI_inv_dt;
                var TransType = _PI_ListModel.TransType;
                var BtnName = _PI_ListModel.BtnName;
                var Command = _PI_ListModel.Command;
                var menuDocumentID = "";
                if (_PI_ListModel.DocumentMenuId != null)
                {
                    menuDocumentID = _PI_ListModel.DocumentMenuId;
                }
                else
                {
                    menuDocumentID = docid;
                }
                //var menuDocumentID = docid;
                TempData["WF_status1"] = WF_Status1;
                TempData["ModelData"] = _PI_ListModel;
                TempData["FilterData"] = FilterData;
                return RedirectToAction("LocalPurchaseInvoiceDetail", new { Inv_no = Inv_no, Inv_dt, TransType, BtnName, Command, menuDocumentID, WF_Status1 });
                //DataRows = Json(GrnDetail);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return DataRows;
        }
        public ActionResult EditPurchaseInvoice(string Inv_no, string Inv_dt,string FilterData,string DocumentMenuId, string WF_Status)
        {
            PI_ListModel _PI_ListModel = new PI_ListModel();
            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
            {
                User_ID = Session["userid"].ToString();
            }
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/


            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["Inv_No"] = Inv_no;
            //Session["Inv_Dt"] = Inv_dt;
            TempData["FilterData"] = FilterData;
            _PI_ListModel.DocumentMenuId = DocumentMenuId;
            TempData["DocumentID2"] = DocumentMenuId;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            _PI_ListModel.PI_inv_no = Inv_no;
            _PI_ListModel.PI_inv_dt = Inv_dt;
            _PI_ListModel.Message = "New"; 
            _PI_ListModel.Command = "Add";
            TempData["WF_status1"] = WF_Status;
            _PI_ListModel.TransType = "Update";
            var TransType = _PI_ListModel.TransType;
            _PI_ListModel.AppStatus = "D"; 
            _PI_ListModel.BtnName = "BtnToDetailPage";
            var BtnName = _PI_ListModel.BtnName;
            var Command = _PI_ListModel.Command;
            var menuDocumentID = DocumentMenuId;
            TempData["ModelData"] = _PI_ListModel;
            _PI_ListModel.UserID = User_ID; //Added by nidhi on 01-09-2025


            return RedirectToAction("LocalPurchaseInvoiceDetail", new { Inv_no = Inv_no, Inv_dt, TransType, BtnName, Command, menuDocumentID, WF_Status });
        }
        [HttpPost]
        public JsonResult GetGoodReceiptNoteLists(string Supp_id)
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
                DataSet result = _DomesticPurchaseInvoiceIService.GetGoodReceiptNoteList(Supp_id, Comp_ID, Br_ID);

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
        public ActionResult VarienceDetails(string GRNNo, string GRNDate, string ItmCode,string DocumentMenuId,string SubItem,string DnForVarncQty="N")
        {
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
                DataTable Deatils = _DomesticPurchaseInvoiceIService.GetVarienceDetails(Comp_ID, Br_ID, GRNNo, GRNDate, ItmCode).Tables[0];
                ViewBag.VarienceDetail = Deatils;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.PopSubItem = SubItem;
                ViewBag.DnForVarianceQty = DnForVarncQty;
                return PartialView("~/Areas/Common/Views/_VarianceDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult VarienceQuantityDetails(string GRNNo, string GRNDate, string ItmCode, string DocumentMenuId
            ,string VarianceDataList,string Disabled)
        {
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
                DataTable VarData = new DataTable();
                VarData.Columns.Add("dn_no", typeof(string));
                VarData.Columns.Add("dn_date", typeof(string));
                VarData.Columns.Add("mr_no", typeof(string));
                VarData.Columns.Add("mr_date", typeof(string));
                VarData.Columns.Add("item_id", typeof(string));
                VarData.Columns.Add("var_type", typeof(string));
                VarData.Columns.Add("var_type_desc", typeof(string));
                VarData.Columns.Add("qty", typeof(string));
                VarData.Columns.Add("price", typeof(string));
                VarData.Columns.Add("value", typeof(string));
                VarData.Columns.Add("tax", typeof(string));
                VarData.Columns.Add("net_amt", typeof(string));
                VarData.Columns.Add("dn_amt", typeof(string));
                VarData.Columns.Add("include", typeof(string));
                if (!string.IsNullOrEmpty(VarianceDataList))
                {
                    JArray VarDataList = JArray.Parse(VarianceDataList);
                    for (int i = 0; i < VarDataList.Count; i++)
                    {
                        DataRow dtrowTaxDetailsLines = VarData.NewRow();
                        dtrowTaxDetailsLines["dn_no"] = VarDataList[i]["dn_no"].ToString();
                        dtrowTaxDetailsLines["dn_date"] = VarDataList[i]["dn_date"].ToString();
                        dtrowTaxDetailsLines["mr_no"] = VarDataList[i]["mr_no"].ToString();
                        dtrowTaxDetailsLines["mr_date"] = VarDataList[i]["mr_date"].ToString();
                        dtrowTaxDetailsLines["item_id"] = VarDataList[i]["item_id"].ToString();
                        dtrowTaxDetailsLines["var_type"] = VarDataList[i]["var_type"].ToString();
                        dtrowTaxDetailsLines["var_type_desc"] = getVarTypeDesc(VarDataList[i]["var_type"].ToString());//VarDataList[i]["var_type_desc"].ToString();
                        dtrowTaxDetailsLines["qty"] = VarDataList[i]["qty"].ToString();
                        dtrowTaxDetailsLines["price"] = VarDataList[i]["price"].ToString();
                        dtrowTaxDetailsLines["value"] = VarDataList[i]["value"].ToString();
                        dtrowTaxDetailsLines["tax"] = VarDataList[i]["tax"].ToString();
                        dtrowTaxDetailsLines["net_amt"] = VarDataList[i]["net_amt"].ToString();
                        dtrowTaxDetailsLines["dn_amt"] = VarDataList[i]["dn_amt"].ToString();
                        dtrowTaxDetailsLines["include"] = VarDataList[i]["include"].ToString();
                        VarData.Rows.Add(dtrowTaxDetailsLines);
                    }
                }
                //DataTable Deatils = _DomesticPurchaseInvoiceIService.GetVarienceDetails(Comp_ID, Br_ID, GRNNo, GRNDate, ItmCode, "QtyDetails").Tables[0];
                ViewBag.VarienceDetail = VarData;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.Disabled = Disabled;
                
                return PartialView("~/Areas/Common/Views/Cmn_VarianceQuantityDetails.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private string getVarTypeDesc(string var_type)
        {
            string var_type_desc = "";
            if (!string.IsNullOrEmpty(var_type))
            {
                if (var_type == "SRGE")
                {
                    var_type_desc = Resource.ShortReceivedInGateEntry;
                }
                else if (var_type == "SCQC")
                {
                    var_type_desc = Resource.ShortCountedInQC;
                }
                else if (var_type == "SQTY")
                {
                    var_type_desc = Resource.SampleReserveQuantity;
                }
            }
            return var_type_desc;
        }
        //public JsonResult VarienceQuantityTaxDetails(string GRNNo, string GRNDate, string ItmCode)
        //{
        //    try
        //    {
        //        JsonResult DataRows = null;
        //        string Comp_ID = string.Empty;
        //        string Br_ID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            Comp_ID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            Br_ID = Session["BranchId"].ToString();
        //        }
        //        DataTable Deatils = _DomesticPurchaseInvoiceIService.GetVarienceDetails(Comp_ID, Br_ID, GRNNo, GRNDate, ItmCode, "TaxDetails").Tables[0];
        //        ViewBag.VarienceDetail = Deatils;
        //        DataRows = Json(JsonConvert.SerializeObject(Deatils), JsonRequestBehavior.AllowGet);

        //        return DataRows;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //}
        public ActionResult GetSuppList(PI_ListModel _PI_ListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(_PI_ListModel.PI_SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _PI_ListModel.PI_SuppName;
                }
                if(_PI_ListModel.DocumentMenuId !=null && _PI_ListModel.DocumentMenuId != "")
                {                 
                    if (_PI_ListModel.DocumentMenuId == "105101145")
                    {
                        SuppType = "D";
                    }
                    if (_PI_ListModel.DocumentMenuId == "105101140125")
                    {
                        SuppType = "I";
                    }

                }        
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105101145")
                //    {
                //        SuppType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105101140125")
                //    {
                //        SuppType = "I";
                //    }
                //}
                CustList = _DomesticPurchaseInvoiceIService.GetSupplierList(Comp_ID, SupplierName, Br_ID, SuppType);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _PI_ListModel.SupplierNameList = _SuppList;
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
        public ActionResult DeletePI_Details(PI_ListModel _PI_ListModel, string InvType)
        {
            try
            {
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string BranchID = string.Empty;
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
                string GRN_Delete = _DomesticPurchaseInvoiceIService.Delete_PI_Detail(_PI_ListModel, InvType, Comp_ID, BranchID);
                /*--------------------------For Attatchment Start--------------------------*/
                if (!string.IsNullOrEmpty(_PI_ListModel.PI_inv_no))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = _PI_ListModel.Title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string InvNo1 = _PI_ListModel.PI_inv_no.Replace("/", "");
                    other.DeleteTempFile(Comp_ID + BranchID, PageName, InvNo1, Server);
                }
                /*--------------------------For Attatchment End--------------------------*/
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["Inv_No"] = _PI_ListModel.PI_inv_no;
                //Session["Inv_Dt"] = _PI_ListModel.PI_inv_dt;
                //_PI_ListModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnDelete";
                _PI_ListModel.Message = "Deleted";
                _PI_ListModel.Command = "Refresh";
                _PI_ListModel.PI_inv_no = null;
                _PI_ListModel.PI_inv_dt = null;
                _PI_ListModel.TransType = "Refresh";
                _PI_ListModel.AppStatus = "D";
                _PI_ListModel.BtnName = "BtnDelete";
                TempData["ModelData"] = _PI_ListModel;

                return RedirectToAction("LocalPurchaseInvoiceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private List<PurchaseInvoiceList> GetGRNDetailList(PI_ListModel _PI_ListModel)
        {
            try
            {
                List<PurchaseInvoiceList> _PurchaseInvoiceList = new List<PurchaseInvoiceList>();
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string User_ID = string.Empty;
                DataSet dt = new DataSet();

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
                    User_ID = Session["UserId"].ToString();
                }
                //else
                //{
                //    _PI_ListModel.RFQ_FromDate = startDate;
                //}
                dt = _DomesticPurchaseInvoiceListIService.GetPI_DetailList(Comp_ID, Br_ID, User_ID, _PI_ListModel.PI_SuppID, _PI_ListModel.PI_FromDate, _PI_ListModel.PI_ToDate, _PI_ListModel.Status, _PI_ListModel.PI_wfdocid, _PI_ListModel.PI_wfstatus);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        PurchaseInvoiceList _PIList = new PurchaseInvoiceList();
                        _PIList.InvoiceNo = dr["inv_no"].ToString();
                        _PIList.InvoiceDate = dr["InvDate"].ToString();
                        _PIList.InvDate = dr["InvDt"].ToString();
                        _PIList.Mr_No = dr["mr_no"].ToString();
                        _PIList.Mr_Date = dr["mr_dt"].ToString();
                        _PIList.InvoiceType = dr["InvType"].ToString();
                        _PIList.SuppName = dr["supp_name"].ToString();
                        _PIList.SuppCurrency = dr["curr"].ToString();
                        _PIList.InvoiceValue = dr["net_val"].ToString();
                        _PIList.Stauts = dr["Status"].ToString();
                        _PIList.CreateDate = dr["CreateDate"].ToString();
                        _PIList.ApproveDate = dr["ApproveDate"].ToString();
                        _PIList.ModifyDate = dr["ModifyDate"].ToString();
                        _PIList.create_by = dr["create_by"].ToString();
                        _PIList.app_by = dr["app_by"].ToString();
                        _PIList.mod_by = dr["mod_by"].ToString();
                        _PIList.BillNumber = dr["bill_no"].ToString();
                        _PIList.BillDate = dr["bill_date"].ToString();
                        _PurchaseInvoiceList.Add(_PIList);
                    }
                }
                return _PurchaseInvoiceList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult AddNewLocalPurchaseInvoice(string DocumentMenuId)
        {
            PI_ListModel _PI_ListModel = new PI_ListModel();
            /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ImportPurchaseInvoiceList");
            }
           /*End to chk Financial year exist or not*/

            TempData["FilterData"] = null;
            //@Session["Message"] = "New";
            //Session["Command"] = "New";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            _PI_ListModel.Message = "New";
            _PI_ListModel.Command = "New";
            _PI_ListModel.AppStatus = "D";
            _PI_ListModel.TransType = "Save";
            _PI_ListModel.BtnName = "BtnAddNew";
            _PI_ListModel.DocumentMenuId =DocumentMenuId;
            var TransType = _PI_ListModel.TransType;
            var BtnName = _PI_ListModel.BtnName;
            var Command = _PI_ListModel.Command;
            var menuDocumentID = DocumentMenuId;
            var Message = _PI_ListModel.Message;
            TempData["ModelData"] = _PI_ListModel;
            return RedirectToAction("LocalPurchaseInvoiceDetail", "LocalPurchaseInvoice",new { TransType= TransType, BtnName, Command, menuDocumentID, Message });
        }
        [HttpPost]
        public JsonResult GetGoodReceiptNoteDetails(string GRNNo, string GRNDate,string supp_id)
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
                DataSet result = _DomesticPurchaseInvoiceIService.GetGoodReceiptNoteDetail(GRNNo, GRNDate, Comp_ID, Br_ID, supp_id);

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
        public JsonResult CheckRoundOffAccount()
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
                DataTable result = _DomesticPurchaseInvoiceIService.CheckRoundOffAcc(Comp_ID, Br_ID);

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
        public ActionResult GetAutoCompleteSearchSuppList(PI_ListModel _PI_ListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string  SuppType = string.Empty;
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
                if (string.IsNullOrEmpty(_PI_ListModel.PI_SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _PI_ListModel.PI_SuppName;
                }
                if(_PI_ListModel.DocumentMenuId != null && _PI_ListModel.DocumentMenuId !="")
                {
                    if(_PI_ListModel.DocumentMenuId == "105101145")
                    {
                        SuppType = "D";
                    }
                    if(_PI_ListModel.DocumentMenuId == "105101140125")
                    {
                        SuppType = "I";
                    }
                }
                    //if (Session["MenuDocumentId"] != null)
                    //{
                    //    if (Session["MenuDocumentId"].ToString() == "105101145")
                    //    {
                    //        SuppType = "D";
                    //    }
                    //    if (Session["MenuDocumentId"].ToString() == "105101140125")
                    //    {
                    //        SuppType = "I";
                    //    }
                    //}
                    CustList = _DomesticPurchaseInvoiceIService.GetSupplierList(Comp_ID, SupplierName, Br_ID, SuppType);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _PI_ListModel.SupplierNameList = _SuppList;
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
        public ActionResult SearchPI_Detail(string SuppId, string Fromdate, string Todate, string Status,string Docid)
        {
            PI_ListModel _PI_ListModel = new PI_ListModel();
            try
            {
                _PI_ListModel.WF_Status = null;
                _PI_ListModel.PI_wfstatus = null;

                if(Docid != null)
                {
                    _PI_ListModel.DocumentMenuId = Docid;
                    if (_PI_ListModel.DocumentMenuId == "105101145")
                    {
                        DocumentMenuId = "105101145";
                    }
                    if(_PI_ListModel.DocumentMenuId == "105101140125")
                    {
                        DocumentMenuId = "105101140125";
                    }
                }
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string User_ID = string.Empty;
                DataSet dt = new DataSet();

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
                    User_ID = Session["UserId"].ToString();
                }
                List<PurchaseInvoiceList> _PurchaseInvoiceList = new List<PurchaseInvoiceList>();
                dt = _DomesticPurchaseInvoiceListIService.GetPI_DetailList(Comp_ID, Br_ID, User_ID, SuppId
                    , Fromdate, Todate, Status, DocumentMenuId, "");
                _PI_ListModel.LPISearch = "LPI_Search";
                //Session["LPISearch"] = "LPI_Search";
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        PurchaseInvoiceList _PIList = new PurchaseInvoiceList();
                        _PIList.InvoiceNo = dr["inv_no"].ToString();
                        _PIList.InvoiceDate = dr["InvDate"].ToString();
                        _PIList.InvDate = dr["InvDate"].ToString();
                        _PIList.Mr_No = dr["mr_no"].ToString();
                        _PIList.Mr_Date = dr["mr_dt"].ToString();
                        _PIList.InvoiceType = dr["InvType"].ToString();
                        _PIList.SuppName = dr["supp_name"].ToString();
                        _PIList.SuppCurrency = dr["curr"].ToString();
                        _PIList.InvoiceValue = dr["net_val"].ToString();
                        _PIList.Stauts = dr["Status"].ToString();
                        _PIList.CreateDate = dr["CreateDate"].ToString();
                        _PIList.ApproveDate = dr["ApproveDate"].ToString();
                        _PIList.ModifyDate = dr["ModifyDate"].ToString();
                        _PIList.create_by = dr["create_by"].ToString();
                        _PIList.app_by = dr["app_by"].ToString();
                        _PIList.mod_by = dr["mod_by"].ToString();
                        _PIList.BillNumber = dr["bill_no"].ToString();
                        _PIList.BillDate = dr["bill_date"].ToString();
                        _PurchaseInvoiceList.Add(_PIList);
                    }
                }
                _PI_ListModel.PIList = _PurchaseInvoiceList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPurchageInvoiceList.cshtml", _PI_ListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        [HttpPost]
        public JsonResult GetPITaxTypeList()
        {
            JsonResult DataRows = null;
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
                DataSet PITaxList = _DomesticPurchaseInvoiceIService.GetPITaxListDAL(Comp_ID, Br_ID);
                DataRows = Json(JsonConvert.SerializeObject(PITaxList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                PurchaseInvoiceattch _PurchaseInvoiceattch = new PurchaseInvoiceattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string Inv_No = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //TransType = _PI_ListModel.TransType;
                //Inv_No = _PI_ListModel.PI_inv_no;
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["Inv_No"] != null)
                //{
                //    Inv_No = Session["Inv_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = Inv_No;
                _PurchaseInvoiceattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _PurchaseInvoiceattch.AttachMentDetailItmStp = dt;
                    //Session["AttachMentDetailItmStp"] = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _PurchaseInvoiceattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _PurchaseInvoiceattch;
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

        /*---------------------------------Sub-Item Start-------------------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
     , string Flag, string Status, string Doc_no, string Doc_dt)
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
                DataTable dt = new DataTable();
                int QtyDigit = Convert.ToInt32(Session["QtyDigit"]);

                if (Flag == "Quantity")
                {
                    dt.Columns.Add("item_id", typeof(string));
                    dt.Columns.Add("sub_item_id", typeof(string));
                    dt.Columns.Add("sub_item_name", typeof(string));
                    dt.Columns.Add("Qty", typeof(string));
                    dt.Columns.Add("src_doc_number", typeof(string));
                    dt.Columns.Add("src_doc_date", typeof(string));

                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    foreach (JObject item in arr.Children())//
                    {
                        DataRow dRow = dt.NewRow();
                        dRow["item_id"] = item.GetValue("item_id").ToString();
                        dRow["sub_item_id"] = item.GetValue("sub_item_id").ToString();
                        dRow["sub_item_name"] = item.GetValue("sub_item_name").ToString();
                        dRow["Qty"] = item.GetValue("qty").ToString();
                        dRow["src_doc_number"] = item.GetValue("src_doc_no").ToString();
                        dRow["src_doc_date"] = item.GetValue("src_doc_dt").ToString();
                        dt.Rows.Add(dRow);
                    }

                }
                else
                {
                    dt = _DomesticPurchaseInvoiceIService.PI_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    Flag = "PInvVariance";
                }
                

                
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    _subitemPageName = "PInv",
                    //ShowStock = "Y",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y"

                };

                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /*---------------------------------Sub-Item End-------------------------------------*/
        [HttpPost]
        public JsonResult getEinvoiceno_ewbNo(string grnno, string GRN_Date)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _DomesticPurchaseInvoiceIService.getEinvoiceno_ewbNo(Comp_ID, BrchID, grnno, GRN_Date);
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
        private void SetDecimals(PI_ListModel _model)//Added by Suraj Maurya on 25-10-2024
        {

            _model.ValDigit = ToFixDecimal(Convert.ToInt32((_model.DocumentMenuId == "105101140125" ? Session["ExpImpValDigit"] : Session["ValDigit"]).ToString()));
            _model.QtyDigit = ToFixDecimal(Convert.ToInt32((_model.DocumentMenuId == "105101140125" ? Session["ExpImpQtyDigit"] : Session["QtyDigit"]).ToString()));
            _model.RateDigit = ToFixDecimal(Convert.ToInt32((_model.DocumentMenuId == "105101140125" ? Session["ExpImpRateDigit"] : Session["PurCostingRateDigit"]).ToString()));
            //_model.RateDigit = ToFixDecimal(Convert.ToInt32((_model.DocumentMenuId == "105101140125" ? Session["ExpImpRateDigit"] : Session["RateDigit"]).ToString()));
            _model.ExchDigit = ToFixDecimal(Convert.ToInt32(Session["ExchDigit"].ToString()));

            ViewBag.ValDigit = _model.ValDigit;
            ViewBag.QtyDigit = _model.QtyDigit;
            ViewBag.RateDigit = _model.RateDigit;
            ViewBag.ExchDigit = _model.ExchDigit;
        }
        //Added by Nidhi on 11-06-2025
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
                        var data = GetPdfData(docid, Doc_no, Doc_dt);
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
        public string SavePdfDocToSendOnEmailAlert_Ext(string docid, string Doc_no, string Doc_dt, string fileName)
        {
            try
            {
                DataTable dt = new DataTable();
                var commonCont = new CommonController(_Common_IServices);
                var data = GetPdfData(docid, Doc_no, Doc_dt);
                return commonCont.SaveAlertDocument_MailExt(data, fileName);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
        }
        public ActionResult SendEmailAlert(string mail_id, string status, string docid, string Doc_no, string Doc_dt,string filepath)
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
                            string fileName = "PI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
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
    }
}