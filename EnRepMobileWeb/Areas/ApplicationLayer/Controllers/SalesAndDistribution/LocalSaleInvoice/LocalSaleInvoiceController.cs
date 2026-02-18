using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSaleInvoice;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.Resources;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSaleInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZXing;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.LocalSaleInvoice
{
    public class LocalSaleInvoiceController : Controller
    {
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string DocumentMenuId = "";
        string FromDate, title, crm = "Y", rpt_id;
        DataTable dt;
        string CompID, UserID, language, BrchID = string.Empty;

        DomesticSaleInvoice_ISERVICE _DomesticSaleInvoice_ISERVICE;
        Common_IServices _Common_IServices;
        //string CompID, BrchID = String.Empty;
        public LocalSaleInvoiceController(Common_IServices _Common_IServices, DomesticSaleInvoice_ISERVICE _LocalSalesInvoice_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._DomesticSaleInvoice_ISERVICE = _LocalSalesInvoice_ISERVICES;
        }
        public ActionResult CommercialInvoiceList(SI_ListModel _SI_ListModel)
        {
            try
            {
                DocumentMenuId = "105103145125";
                var CustType = "E";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                CommonPageDetails();

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                BrchID = Session["BranchId"].ToString();
                DomesticSaleInvoice_Model _DomesticComercialInvoice_Model = new DomesticSaleInvoice_Model();
                _DomesticComercialInvoice_Model.GstApplicable = ViewBag.GstApplicable;
                _SI_ListModel.DocumentMenuId = DocumentMenuId;
                _SI_ListModel.CustType = CustType;
                #region Commented By Nitesh 08-04-2024 For All List Data
                // GetCustList(_SI_ListModel, _DomesticComercialInvoice_Model, "L");
                #endregion
                GetStatusList(_SI_ListModel);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _SI_ListModel.CustID = a[0].Trim();
                    _SI_ListModel.SI_FromDate = a[1].Trim();
                    _SI_ListModel.SI_ToDate = a[2].Trim();
                    _SI_ListModel.Status = a[3].Trim();
                    _SI_ListModel.SQ_SalePerson = a[4].Trim();
                    if (_SI_ListModel.Status == "0")
                    {
                        _SI_ListModel.Status = null;
                    }
                    _SI_ListModel.ListFilterData = TempData["ListFilterData"].ToString();

                }
                #region Commented By Nitesh 08-04-2024 For All List Data
                // _SI_ListModel.SIList = GetSI_DetailList(_SI_ListModel);
                #endregion
                if (_SI_ListModel.SI_FromDate != null)
                {
                    _SI_ListModel.FromDate = _SI_ListModel.SI_FromDate;
                }
                else
                {
                    _SI_ListModel.FromDate = startDate;
                    _SI_ListModel.SI_FromDate = startDate;
                    _SI_ListModel.SI_ToDate = CurrentDate;
                }
                GetAllData(_SI_ListModel);
                _SI_ListModel.Title = title;
                _SI_ListModel.LSISearch = "0";
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/LocalSaleInvoiceList.cshtml", _SI_ListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult LocalSaleInvoiceList(SI_ListModel _SI_ListModel)
        {
            try
            {
                DocumentMenuId = "105103140";
                string CustType = "";
                CustType = "D";

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["crm"] != null)
                {
                    crm = Session["crm"].ToString();
                }
                if (UserID == "1001")
                {
                    crm = "Y";
                }
                ViewBag.crm = crm;
                CommonPageDetails();
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;


                BrchID = Session["BranchId"].ToString();
                DomesticSaleInvoice_Model _DomesticSaleInvoice_Model = new DomesticSaleInvoice_Model();
                _SI_ListModel.DocumentMenuId = DocumentMenuId;
                _SI_ListModel.CustType = CustType;
                #region Commented By Nitesh 08-04-2024 For All List Data
                // GetCustList(_SI_ListModel, _DomesticSaleInvoice_Model, "L");
                GetStatusList(_SI_ListModel);
                #endregion
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _SI_ListModel.CustID = a[0].Trim();
                    _SI_ListModel.SI_FromDate = a[1].Trim();
                    _SI_ListModel.SI_ToDate = a[2].Trim();
                    _SI_ListModel.Status = a[3].Trim();
                    _SI_ListModel.SQ_SalePerson = a[4].Trim();
                    if (_SI_ListModel.Status == "0")
                    {
                        _SI_ListModel.Status = null;
                    }
                    _SI_ListModel.ListFilterData = TempData["ListFilterData"].ToString();
                }
                #region Commented By Nitesh 08-04-2024 For All List Data

                //_SI_ListModel.SIList = GetSI_DetailList(_SI_ListModel);
                #endregion
                if (_SI_ListModel.SI_FromDate != null)
                {
                    _SI_ListModel.FromDate = _SI_ListModel.SI_FromDate;
                }
                else
                {
                    _SI_ListModel.FromDate = startDate;
                    _SI_ListModel.SI_FromDate = startDate;
                    _SI_ListModel.SI_ToDate = CurrentDate;
                }
                GetAllData(_SI_ListModel);
                _SI_ListModel.Title = title;
                _SI_ListModel.LSISearch = "0";
                _SI_ListModel.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/LocalSaleInvoiceList.cshtml", _SI_ListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(SI_ListModel _SI_ListModel)
        {
            string CustomerName = string.Empty;
            string wfstatus = string.Empty;
            string CustType = string.Empty;
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
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }

                if (string.IsNullOrEmpty(_SI_ListModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _SI_ListModel.CustName;
                }


                if (_SI_ListModel.CustType != null)
                {
                    CustType = _SI_ListModel.CustType;
                }
                if (_SI_ListModel.WF_status != null)
                {
                    wfstatus = _SI_ListModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }

                DataSet AllData = _DomesticSaleInvoice_ISERVICE.GetAllData(Comp_ID, CustomerName, Br_ID, CustType
                  , _SI_ListModel.CustID, _SI_ListModel.SI_FromDate, _SI_ListModel.SI_ToDate, _SI_ListModel.Status, UserID, DocumentMenuId, wfstatus, _SI_ListModel.SQ_SalePerson);
                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (DataRow data in AllData.Tables[0].Rows)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.cust_id = data["cust_id"].ToString();
                    _CustDetail.cust_name = data["cust_name"].ToString();
                    _CustList.Add(_CustDetail);
                }
                _CustList.Insert(0, new CustomerName() { cust_id = "0", cust_name = "All" });
                _SI_ListModel.CustomerNameList = _CustList;

                List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                foreach (DataRow data in AllData.Tables[3].Rows)
                {
                    SalePersonList _SlPrsnDetail = new SalePersonList();
                    _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                    _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                    _SlPrsnList.Add(_SlPrsnDetail);
                }
                _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                _SI_ListModel.SalePersonList = _SlPrsnList;

                if (_SI_ListModel.CustomerId != null && _SI_ListModel.CustomerId != "")
                {
                    _SI_ListModel.CustID = _SI_ListModel.CustomerId;
                }
                SetAllDatainListTable(_SI_ListModel, AllData);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void SetAllDatainListTable(SI_ListModel _SI_ListModel, DataSet dt)
        {
            List<SalesInvoiceList> _SalesInvoiceList = new List<SalesInvoiceList>();
            if (dt.Tables[2].Rows.Count > 0)
            {
                //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
            }
            if (dt.Tables[1].Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    SalesInvoiceList _SIList = new SalesInvoiceList();
                    _SIList.InvoiceNo = dr["inv_no"].ToString();
                    _SIList.InvoiceDate = dr["InvDate"].ToString();
                    _SIList.SalesPerson = dr["SalesPerson"].ToString();
                    _SIList.InvDate = dr["InvDt"].ToString();
                    _SIList.InvoiceType = dr["InvType"].ToString();
                    _SIList.ship_no = dr["sh_no"].ToString();
                    _SIList.ship_dt = dr["sh_date"].ToString();
                    _SIList.CustName = dr["cust_name"].ToString();
                    _SIList.Currency = dr["curr"].ToString();
                    _SIList.InvoiceValue = dr["net_val"].ToString();
                    _SIList.Stauts = dr["Status"].ToString();
                    _SIList.CreateDate = dr["CreateDate"].ToString();
                    _SIList.ApproveDate = dr["ApproveDate"].ToString();
                    _SIList.ModifyDate = dr["ModifyDate"].ToString();
                    _SIList.create_by = dr["create_by"].ToString();
                    _SIList.app_by = dr["app_by"].ToString();
                    _SIList.mod_by = dr["mod_by"].ToString();
                    _SIList.custom_inv_no = dr["custom_inv_no"].ToString();
                    _SIList.custom_inv_dt = dr["custom_inv_dt"].ToString();
                    _SalesInvoiceList.Add(_SIList);
                }
            }
            _SI_ListModel.SIList = _SalesInvoiceList;
        }
        public ActionResult AddNewLocalSaleInvoice(string DocumentMenuId)
        {
            DomesticSaleInvoice_Model _DomesticSaleInvoiceaddNew_Model = new DomesticSaleInvoice_Model();
            SI_ListModel _SI_ListModel = new SI_ListModel();
            _DomesticSaleInvoiceaddNew_Model.Message = "New";
            _DomesticSaleInvoiceaddNew_Model.Command = "New";
            _DomesticSaleInvoiceaddNew_Model.TransType = "Save";
            _DomesticSaleInvoiceaddNew_Model.BtnName = "BtnAddNew";
            _DomesticSaleInvoiceaddNew_Model.DocumentMenuId = DocumentMenuId;
            if (DocumentMenuId != null)
            {
                if (DocumentMenuId == "105103140")
                {
                    _DomesticSaleInvoiceaddNew_Model.InvType = "D";
                }
                else
                {
                    _DomesticSaleInvoiceaddNew_Model.InvType = "E";
                }
            }
            _DomesticSaleInvoiceaddNew_Model.CustType = _SI_ListModel.CustType;
            _DomesticSaleInvoiceaddNew_Model.DocumentStatus = "D";
            TempData["ModelData"] = _DomesticSaleInvoiceaddNew_Model;
            UrlModel _urlModel = new UrlModel();
            _urlModel.Command = "New";
            _urlModel.TransType = "Save";
            _urlModel.BtnName = "BtnAddNew";
            _urlModel.DocumentMenuId = DocumentMenuId;
            _urlModel.DocumentStatus = _DomesticSaleInvoiceaddNew_Model.DocumentStatus;
            _urlModel.InvType = _DomesticSaleInvoiceaddNew_Model.InvType;
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                if (DocumentMenuId == "105103140")
                {
                    return RedirectToAction("LocalSaleInvoiceList");
                }
                else
                {
                    return RedirectToAction("CommercialInvoiceList");
                }

            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("LocalSaleInvoiceDetail", _urlModel);
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
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
                ViewBag.VBRoleList = ds.Tables[3];
                ViewBag.StatusList = ds.Tables[4];
                ViewBag.GstApplicableForExport = ds.Tables[5].Rows.Count > 0 ? ds.Tables[5].Rows[0]["param_stat"].ToString() : "";
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                ViewBag.ExpComrcPrintOptions = ds.Tables[9].Rows.Count > 0 ? ds.Tables[9].Rows[0]["param_stat"].ToString() : "";

                #region Commented By Nitesh 24-05-2024 for Gst Applicable And Other Parameter table Name And Param_id is changed
                #endregion
                //foreach (DataRow Row in ds.Tables[1].Rows)
                //{
                //    if (Row["param_id"].ToString() == "101")
                //    {
                //        ViewBag.GstApplicable = Row["param_stat"].ToString();
                //    }
                //    else if (Row["param_id"].ToString() == "107")
                //    {
                //        ViewBag.ExpComrcPrintOptions = Row["param_stat"].ToString();
                //    }
                //}
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
        public ActionResult LocalSaleInvoiceDetail(UrlModel _urlModel)
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
                if (Session["crm"] != null)
                {
                    crm = Session["crm"].ToString();
                }
                if (UserID == "1001")
                {
                    crm = "Y";
                }
                ViewBag.crm = crm;
                /*Add by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.SI_Date) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
                var _DomesticSaleInvoice_ModelS = TempData["ModelData"] as DomesticSaleInvoice_Model;
                if (_DomesticSaleInvoice_ModelS != null)
                {
                    if (_DomesticSaleInvoice_ModelS.DocumentMenuId != null)
                    {
                        if (_DomesticSaleInvoice_ModelS.DocumentMenuId == "105103140")
                        {
                            DocumentMenuId = _DomesticSaleInvoice_ModelS.DocumentMenuId;
                            _DomesticSaleInvoice_ModelS.MenuDocumentId = DocumentMenuId;

                        }
                        if (_DomesticSaleInvoice_ModelS.DocumentMenuId == "105103145125")
                        {
                            DocumentMenuId = _DomesticSaleInvoice_ModelS.DocumentMenuId;
                            _DomesticSaleInvoice_ModelS.MenuDocumentId = DocumentMenuId;
                        }
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    BrchID = Session["BranchId"].ToString();
                    if (Session["UserId"] != null)
                    {
                        UserID = Session["UserId"].ToString();
                    }
                    _DomesticSaleInvoice_ModelS.DocumentMenuId = DocumentMenuId;
                    _DomesticSaleInvoice_ModelS.GstApplicable = ViewBag.GstApplicable;
                    _DomesticSaleInvoice_ModelS.user_id = UserID;//Added by nidhi on 27-08-2025
                    SI_ListModel _SI_ListModel = new SI_ListModel();

                    DataTable dt = BindBankDropdown();
                    List<BankName> _BankName = new List<BankName>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        BankName _bankdetail = new BankName();
                        _bankdetail.acc_id = dr["acc_id"].ToString();
                        _bankdetail.Acc_Name = dr["acc_name"].ToString();
                        _BankName.Add(_bankdetail);
                    }
                    _BankName.Insert(0, new BankName() { acc_id = "0", Acc_Name = "---Select---" });
                    _DomesticSaleInvoice_ModelS.BankNameList = _BankName;

                    List<PortOfLoadingListModel> _PortOfLoadingListModel1 = new List<PortOfLoadingListModel>();
                    PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
                    PortOfLoadingLis.POL_Name = "---Select---";
                    PortOfLoadingLis.POL_id = "0";
                    _PortOfLoadingListModel1.Add(PortOfLoadingLis);
                    _DomesticSaleInvoice_ModelS.PortOfLoadingList = _PortOfLoadingListModel1;

                    List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                    pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                    _pi_rcpt_carrLis.Pi_Name = "---Select---";
                    _pi_rcpt_carrLis.Pi_id = "0";
                    _pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                    _DomesticSaleInvoice_ModelS.pi_rcpt_carrList = _pi_rcpt_carrListModel;

                    GetCustList(_SI_ListModel, _DomesticSaleInvoice_ModelS, "D", _DomesticSaleInvoice_ModelS.DocumentMenuId);
                    GetSalesPersonList(_DomesticSaleInvoice_ModelS);
                    //GetSITaxTypeList(_DomesticSaleInvoice_ModelS);
                    List<TaxCalciTaxName> _TaxName = new List<TaxCalciTaxName>();
                    TaxCalciTaxName _TaxNameList = new TaxCalciTaxName();
                    _TaxNameList.tax_name = "---Select---";
                    _TaxNameList.tax_id = "0";
                    _TaxName.Add(_TaxNameList);
                    _DomesticSaleInvoice_ModelS.TaxCalciTaxNameList = _TaxName;

                    List<OcCalciOtherCharge> _OCName = new List<OcCalciOtherCharge>();
                    OcCalciOtherCharge _OCNameList = new OcCalciOtherCharge();
                    _OCNameList.oc_name = "---Select---";
                    _OCNameList.oc_id = "0";
                    _OCName.Add(_OCNameList);
                    _DomesticSaleInvoice_ModelS.OcCalciOtherChargeList = _OCName;
                    if (_DomesticSaleInvoice_ModelS.DocumentMenuId == "105103140")
                    {
                        if (_DomesticSaleInvoice_ModelS.SI_Number == "" ||
                 _DomesticSaleInvoice_ModelS.SI_Number == null)
                        {
                            DataSet dt1 = new DataSet();
                            BindDispatchDetail(_DomesticSaleInvoice_ModelS, "", dt1);
                        }
                    }
                    // for bind Trade term
                    List<trade_termList> _TermLists = new List<trade_termList>();
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                    _DomesticSaleInvoice_ModelS.TradeTermsList = _TermLists;

                    DataTable dtbscurr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                    if (dtbscurr.Rows.Count > 0)
                    {
                        _DomesticSaleInvoice_ModelS.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                        ViewBag.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                    }
                    string ValDigit;
                    string QtyDigit;
                    string RateDigit;
                    if (_DomesticSaleInvoice_ModelS.DocumentMenuId == "105103145125")
                    {
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"]));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"]));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpRateDigit"])); //
                    }
                    else
                    {
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"])); //
                    }

                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DomesticSaleInvoice_ModelS.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    List<ShipNumberList> shipNoList = new List<ShipNumberList>();
                    shipNoList.Add(new ShipNumberList { Ship_number = "---Select---"/*, Ship_date = "0"*/ });
                    _DomesticSaleInvoice_ModelS.shipNumbers = shipNoList;
                    if (_DomesticSaleInvoice_ModelS.TransType == "Update" || _DomesticSaleInvoice_ModelS.TransType == "Edit")
                    {
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }
                        string SINo = _DomesticSaleInvoice_ModelS.SI_Number;
                        string SIDate = _DomesticSaleInvoice_ModelS.SI_Date;
                        string VouType = "SV";
                        string Type = _DomesticSaleInvoice_ModelS.InvType;
                        DataSet ds = _DomesticSaleInvoice_ISERVICE.Edit_SIDetail(CompID, BrchID, VouType, SINo, SIDate, Type, UserID, DocumentMenuId);
                        ViewBag.ItemTCSDetails = ds.Tables[17];
                      
                        _DomesticSaleInvoice_ModelS.tcs_amt = ds.Tables[0].Rows[0]["tcs_amt"].ToString();
                        _DomesticSaleInvoice_ModelS.custom_inv_no = ds.Tables[0].Rows[0]["custom_inv_no"].ToString();
                        _DomesticSaleInvoice_ModelS.InvoiceHeading = ds.Tables[0].Rows[0]["inv_head"].ToString();
                        _DomesticSaleInvoice_ModelS.BuyersOrderNumberAndDate = ds.Tables[0].Rows[0]["buyer_ord_no_dt"].ToString();
                        _DomesticSaleInvoice_ModelS.inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        _DomesticSaleInvoice_ModelS.inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                        _DomesticSaleInvoice_ModelS.inv_type = ds.Tables[0].Rows[0]["inv_type"].ToString();
                        _DomesticSaleInvoice_ModelS.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _DomesticSaleInvoice_ModelS.curr = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _DomesticSaleInvoice_ModelS.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        shipNoList.Add(new ShipNumberList { Ship_number = ds.Tables[0].Rows[0]["sh_no"].ToString() });
                        _DomesticSaleInvoice_ModelS.shipNumbers = shipNoList;
                        _DomesticSaleInvoice_ModelS.ship_no = ds.Tables[0].Rows[0]["sh_no"].ToString();
                        _DomesticSaleInvoice_ModelS.ship_dt = ds.Tables[0].Rows[0]["sh_date"].ToString();
                        ViewBag.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _DomesticSaleInvoice_ModelS.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _DomesticSaleInvoice_ModelS.slprsn_id = ds.Tables[0].Rows[0]["emp_id"].ToString();
                        _DomesticSaleInvoice_ModelS.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                        _DomesticSaleInvoice_ModelS.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(ValDigit);
                        _DomesticSaleInvoice_ModelS.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                        _DomesticSaleInvoice_ModelS.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val"]).ToString(ValDigit);
                        _DomesticSaleInvoice_ModelS.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                        _DomesticSaleInvoice_ModelS.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _DomesticSaleInvoice_ModelS.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _DomesticSaleInvoice_ModelS.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _DomesticSaleInvoice_ModelS.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _DomesticSaleInvoice_ModelS.AmmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _DomesticSaleInvoice_ModelS.AmmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _DomesticSaleInvoice_ModelS.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        _DomesticSaleInvoice_ModelS.inv_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DomesticSaleInvoice_ModelS.TaxCalci_ItemName = ds.Tables[1].Rows[0]["item_name"].ToString();
                        _DomesticSaleInvoice_ModelS.TaxCalci_AssessableValue = ds.Tables[1].Rows[0]["item_gr_val"].ToString();
                        _DomesticSaleInvoice_ModelS.SI_BillingAddress = ds.Tables[0].Rows[0]["bill_address"].ToString();
                        _DomesticSaleInvoice_ModelS.SI_Bill_Add_Id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                        _DomesticSaleInvoice_ModelS.SI_ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                        _DomesticSaleInvoice_ModelS.SI_Shipp_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                        _DomesticSaleInvoice_ModelS.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                        _DomesticSaleInvoice_ModelS.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        _DomesticSaleInvoice_ModelS.benif_name = ds.Tables[0].Rows[0]["benif_name"].ToString();
                        _DomesticSaleInvoice_ModelS.bank_name = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        _DomesticSaleInvoice_ModelS.bank_add = ds.Tables[0].Rows[0]["bank_add"].ToString();
                        _DomesticSaleInvoice_ModelS.acc_no = ds.Tables[0].Rows[0]["acc_no"].ToString();
                        _DomesticSaleInvoice_ModelS.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                        _DomesticSaleInvoice_ModelS.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                        _DomesticSaleInvoice_ModelS.usd_corr_bank = ds.Tables[0].Rows[0]["usd_corr_bank"].ToString();
                        _DomesticSaleInvoice_ModelS.pre_carr_by = ds.Tables[0].Rows[0]["pre_carr_by"].ToString();
                        _DomesticSaleInvoice_ModelS.pi_rcpt_carr = ds.Tables[0].Rows[0]["pi_rcpt_carr"].ToString();
                        _DomesticSaleInvoice_ModelS.ves_fli_no = ds.Tables[0].Rows[0]["ves_fli_no"].ToString();
                        _DomesticSaleInvoice_ModelS.loading_port = ds.Tables[0].Rows[0]["loading_port"].ToString();
                        _DomesticSaleInvoice_ModelS.discharge_port = ds.Tables[0].Rows[0]["discharge_port"].ToString();
                        _DomesticSaleInvoice_ModelS.fin_disti = ds.Tables[0].Rows[0]["fin_disti"].ToString();
                        _DomesticSaleInvoice_ModelS.container_no = ds.Tables[0].Rows[0]["container_no"].ToString();
                        _DomesticSaleInvoice_ModelS.other_ref = ds.Tables[0].Rows[0]["other_ref"].ToString();
                        _DomesticSaleInvoice_ModelS.term_del_pay = ds.Tables[0].Rows[0]["term_del_pay"].ToString();
                        _DomesticSaleInvoice_ModelS.des_good = ds.Tables[0].Rows[0]["des_good"].ToString();
                        _DomesticSaleInvoice_ModelS.prof_detail = ds.Tables[0].Rows[0]["prof_detail"].ToString();
                        _DomesticSaleInvoice_ModelS.declar = ds.Tables[0].Rows[0]["declar"].ToString();
                        _DomesticSaleInvoice_ModelS.CountryOfOriginOfGoods = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                        _DomesticSaleInvoice_ModelS.CountryOfFinalDestination = ds.Tables[0].Rows[0]["cntry_fin_dest"].ToString();
                        _DomesticSaleInvoice_ModelS.ExportersReference = ds.Tables[0].Rows[0]["ext_ref"].ToString();
                        _DomesticSaleInvoice_ModelS.BuyerIfOtherThenConsignee = ds.Tables[0].Rows[0]["buyer_consig"].ToString();
                        _DomesticSaleInvoice_ModelS.trade_term = ds.Tables[0].Rows[0]["trade_term"].ToString();
                        _DomesticSaleInvoice_ModelS.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                        _DomesticSaleInvoice_ModelS.ExporterAddress = ds.Tables[0].Rows[0]["exp_addr"].ToString();
                        _DomesticSaleInvoice_ModelS.ConsigneeName = ds.Tables[0].Rows[0]["consig_name"].ToString();
                        _DomesticSaleInvoice_ModelS.ConsigneeAddress = ds.Tables[0].Rows[0]["consig_addr"].ToString();
                        _DomesticSaleInvoice_ModelS.CustomInvDate = ds.Tables[0].Rows[0]["custom_inv_dt"].ToString();
                        _DomesticSaleInvoice_ModelS.ShipFromAddress = ds.Tables[0].Rows[0]["ship_from_address"].ToString();
                        _DomesticSaleInvoice_ModelS.Custome_Reference = ds.Tables[0].Rows[0]["cust_ref"].ToString();
                        _DomesticSaleInvoice_ModelS.Payment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                        _DomesticSaleInvoice_ModelS.Delivery_term = ds.Tables[0].Rows[0]["deli_term"].ToString();
                        _DomesticSaleInvoice_ModelS.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                        _DomesticSaleInvoice_ModelS.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                        /*Modifyed By Suraj Maurya on 17-10-2024 to add inv_disc_amt,inv_disc_perc column*/
                        _DomesticSaleInvoice_ModelS.OrderDiscountInAmount = ds.Tables[0].Rows[0]["inv_disc_amt"].ToString();
                        _DomesticSaleInvoice_ModelS.OrderDiscountInPercentage = ds.Tables[0].Rows[0]["inv_disc_perc"].ToString();
                        _DomesticSaleInvoice_ModelS.Declaration_1 = ds.Tables[0].Rows[0]["declar_1"].ToString();
                        _DomesticSaleInvoice_ModelS.Declaration_2 = ds.Tables[0].Rows[0]["declar_2"].ToString();
                        _DomesticSaleInvoice_ModelS.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();
                        _DomesticSaleInvoice_ModelS.ShipTo = ds.Tables[0].Rows[0]["ship_to"].ToString();
                        _DomesticSaleInvoice_ModelS.PvtMark = ds.Tables[0].Rows[0]["pvt_mark"].ToString();
                        _DomesticSaleInvoice_ModelS.Corporate_Address = ds.Tables[0].Rows[0]["corp_off_addr"].ToString();
                        _DomesticSaleInvoice_ModelS.Invoice_remarks = ds.Tables[0].Rows[0]["remarks"].ToString();

                        BindDispatchDetail(_DomesticSaleInvoice_ModelS, "update", ds);

                        _DomesticSaleInvoice_ModelS.Address1 = ds.Tables[0].Rows[0]["disp_Addr1"].ToString();
                        _DomesticSaleInvoice_ModelS.Address2 = ds.Tables[0].Rows[0]["disp_Addr2"].ToString();
                        _DomesticSaleInvoice_ModelS.Country = ds.Tables[0].Rows[0]["disp_country"].ToString();
                        _DomesticSaleInvoice_ModelS.State = ds.Tables[0].Rows[0]["disp_state"].ToString();
                   
                        _DomesticSaleInvoice_ModelS.District = ds.Tables[0].Rows[0]["disp_district"].ToString();
                        _DomesticSaleInvoice_ModelS.City = ds.Tables[0].Rows[0]["disp_city"].ToString();
                        _DomesticSaleInvoice_ModelS.Pin = ds.Tables[0].Rows[0]["disp_pin"].ToString();

                        string nontaxable = ds.Tables[0].Rows[0]["non_taxable"].ToString();
                        if (nontaxable == "Y")
                        {
                            _DomesticSaleInvoice_ModelS.nontaxable = true;
                        }
                        else
                        {
                            _DomesticSaleInvoice_ModelS.nontaxable = false;
                        }
                        /*Modifyed By Suraj Maurya on 17-10-2024 to add inv_disc_amt,inv_disc_perc column*/
                        bool revcharge = false;
                        bool.TryParse(ds.Tables[0].Rows[0]["rev_charge"].ToString(), out revcharge);
                        _DomesticSaleInvoice_ModelS.rev_charge = revcharge;
                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        _DomesticSaleInvoice_ModelS.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        if (roundoff_status == "Y")
                        {
                            _DomesticSaleInvoice_ModelS.RoundOffFlag = true;
                        }
                        else
                        {
                            _DomesticSaleInvoice_ModelS.RoundOffFlag = false;
                        }


                        _DomesticSaleInvoice_ModelS.VouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (doc_status == "A" || doc_status == "C")
                        {
                            _DomesticSaleInvoice_ModelS.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                        }
                        _DomesticSaleInvoice_ModelS.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                        _DomesticSaleInvoice_ModelS.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                        ViewBag.GLVoucherNo = _DomesticSaleInvoice_ModelS.GLVoucherNo;/*add by Hina Sharma on 11-08-2025*/
                        _DomesticSaleInvoice_ModelS.Status = doc_status;
                        _DomesticSaleInvoice_ModelS.DocumentStatus = doc_status;


                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            _DomesticSaleInvoice_ModelS.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _DomesticSaleInvoice_ModelS.CancelFlag = true;
                            _DomesticSaleInvoice_ModelS.BtnName = "Refresh";
                        }
                        else
                        {
                            _DomesticSaleInvoice_ModelS.CancelFlag = false;
                        }
                        _DomesticSaleInvoice_ModelS.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _DomesticSaleInvoice_ModelS.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        if (ViewBag.AppLevel != null && _DomesticSaleInvoice_ModelS.Command != "Edit")
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
                                    _DomesticSaleInvoice_ModelS.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _DomesticSaleInvoice_ModelS.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _DomesticSaleInvoice_ModelS.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _DomesticSaleInvoice_ModelS.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _DomesticSaleInvoice_ModelS.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _DomesticSaleInvoice_ModelS.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _DomesticSaleInvoice_ModelS.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _DomesticSaleInvoice_ModelS.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _DomesticSaleInvoice_ModelS.BtnName = "Refresh";
                                }
                            }
                            if (doc_status == "QP")
                            {
                                _DomesticSaleInvoice_ModelS.BtnName = "Refresh";
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _DomesticSaleInvoice_ModelS.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.GLGroup = ds.Tables[7];
                        ViewBag.GLAccount = ds.Tables[7];
                        //ViewBag.TaxDetail = ds.Tables[9]; /*Commented by Suraj on 01-12-2022 for change to common*/
                        ViewBag.ItemTaxDetailsList = ds.Tables[9];
                        ViewBag.OCTaxDetails = ds.Tables[11];
                        //ViewBag.OtherCharge = ds.Tables[3]; /*Commented by Suraj on 01-12-2022 for change to common*/
                        ViewBag.OtherChargeDetails = ds.Tables[3];
                        //ViewBag.TaxCalculatorDetail = ds.Tables[2];
                        ViewBag.ItemTaxDetails = ds.Tables[2];
                        ViewBag.AttechmentDetails = ds.Tables[10];
                        ViewBag.SubItemDetails = ds.Tables[12];
                        ViewBag.CostCenterData = ds.Tables[13];
                        ViewBag.ItemOC_TDSDetails = ds.Tables[16];
                        _DomesticSaleInvoice_ModelS.bcurrflag = ds.Tables[14].Rows[0]["bcurrflag"].ToString();
                        ViewBag.PaymentScheduleData = ds.Tables[22];
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/LocalSaleInvoiceDetail.cshtml", _DomesticSaleInvoice_ModelS);
                    }
                    else
                    {
                        _DomesticSaleInvoice_ModelS.DocumentStatus = "D";
                        _DomesticSaleInvoice_ModelS.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/LocalSaleInvoiceDetail.cshtml", _DomesticSaleInvoice_ModelS);
                    }
                }
                else
                {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    if (Session["UserId"] != null)
                    {
                        UserID = Session["UserId"].ToString();
                    }
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    DomesticSaleInvoice_Model _DomesticSaleInvoice_Model = new DomesticSaleInvoice_Model();
                    _DomesticSaleInvoice_Model.user_id = UserID;//Added by Nidhi on 26-08-2025
                    if (_urlModel.DocumentMenuId != null)
                    {
                        if (_urlModel.DocumentMenuId == "105103140")
                        {
                            DocumentMenuId = _urlModel.DocumentMenuId;
                            _DomesticSaleInvoice_Model.MenuDocumentId = DocumentMenuId;
                        }
                        if (_urlModel.DocumentMenuId == "105103145125")
                        {
                            DocumentMenuId = _urlModel.DocumentMenuId;
                            _DomesticSaleInvoice_Model.MenuDocumentId = DocumentMenuId;
                        }
                    }
                    if (_urlModel != null)
                    {
                        _DomesticSaleInvoice_Model.Command = _urlModel.Command;
                        _DomesticSaleInvoice_Model.TransType = _urlModel.TransType;
                        _DomesticSaleInvoice_Model.BtnName = _urlModel.BtnName;
                        _DomesticSaleInvoice_Model.SI_Number = _urlModel.SI_Number;
                        _DomesticSaleInvoice_Model.SI_Date = _urlModel.SI_Date;
                        _DomesticSaleInvoice_Model.InvType = _urlModel.InvType;
                        _DomesticSaleInvoice_Model.FRoundOffAmt = _urlModel.FRoundOffAmt;
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    BrchID = Session["BranchId"].ToString();
                    if (Session["UserId"] != null)
                    {
                        UserID = Session["UserId"].ToString();
                    }
                    _DomesticSaleInvoice_Model.DocumentMenuId = DocumentMenuId;
                    _DomesticSaleInvoice_Model.GstApplicable = ViewBag.GstApplicable;
                    _DomesticSaleInvoice_Model.FRoundOffAmt = _urlModel.FRoundOffAmt;
                    SI_ListModel _SI_ListModel = new SI_ListModel();
                    DataTable dt = BindBankDropdown();
                    List<BankName> _BankName = new List<BankName>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        BankName _bankdetail = new BankName();
                        _bankdetail.acc_id = dr["acc_id"].ToString();
                        _bankdetail.Acc_Name = dr["acc_name"].ToString();
                        _BankName.Add(_bankdetail);
                    }
                    _BankName.Insert(0, new BankName() { acc_id = "0", Acc_Name = "---Select---" });
                    _DomesticSaleInvoice_Model.BankNameList = _BankName;

                    List<PortOfLoadingListModel> _PortOfLoadingListModel1 = new List<PortOfLoadingListModel>();
                    PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
                    PortOfLoadingLis.POL_Name = "---Select---";
                    PortOfLoadingLis.POL_id = "0";
                    _PortOfLoadingListModel1.Add(PortOfLoadingLis);
                    _DomesticSaleInvoice_Model.PortOfLoadingList = _PortOfLoadingListModel1;

                    List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                    pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                    _pi_rcpt_carrLis.Pi_Name = "---Select---";
                    _pi_rcpt_carrLis.Pi_id = "0";
                    _pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                    _DomesticSaleInvoice_Model.pi_rcpt_carrList = _pi_rcpt_carrListModel;

                    GetCustList(_SI_ListModel, _DomesticSaleInvoice_Model, "D", _DomesticSaleInvoice_Model.DocumentMenuId);
                    GetSalesPersonList(_DomesticSaleInvoice_Model);
                    List<TaxCalciTaxName> _TaxName = new List<TaxCalciTaxName>();
                    TaxCalciTaxName _TaxNameList = new TaxCalciTaxName();
                    _TaxNameList.tax_name = "---Select---";
                    _TaxNameList.tax_id = "0";
                    _TaxName.Add(_TaxNameList);
                    _DomesticSaleInvoice_Model.TaxCalciTaxNameList = _TaxName;

                    List<OcCalciOtherCharge> _OCName = new List<OcCalciOtherCharge>();
                    OcCalciOtherCharge _OCNameList = new OcCalciOtherCharge();
                    _OCNameList.oc_name = "---Select---";
                    _OCNameList.oc_id = "0";
                    _OCName.Add(_OCNameList);
                    _DomesticSaleInvoice_Model.OcCalciOtherChargeList = _OCName;
                    if (_DomesticSaleInvoice_Model.DocumentMenuId == "105103140")
                    {
                        if (_DomesticSaleInvoice_Model.SI_Number == "" ||
               _DomesticSaleInvoice_Model.SI_Number == null)
                        {
                            DataSet dt1 = new DataSet();
                            BindDispatchDetail(_DomesticSaleInvoice_Model, "", dt1);
                        }
                    }
                    // for bind Trade term
                    List<trade_termList> _TermLists = new List<trade_termList>();
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                    _DomesticSaleInvoice_Model.TradeTermsList = _TermLists;

                    DataTable dtbscurr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                    if (dtbscurr.Rows.Count > 0)
                    {
                        _DomesticSaleInvoice_Model.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                        ViewBag.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                    }
                    string ValDigit;
                    string QtyDigit;
                    string RateDigit;
                    if (_DomesticSaleInvoice_Model.DocumentMenuId == "105103145125")
                    {
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"]));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"]));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpRateDigit"])); //
                    }
                    else
                    {
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"])); //
                    }
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _DomesticSaleInvoice_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    List<ShipNumberList> shipNoList = new List<ShipNumberList>();
                    shipNoList.Add(new ShipNumberList { Ship_number = "---Select---"/*, Ship_date = "0"*/ });
                    _DomesticSaleInvoice_Model.shipNumbers = shipNoList;
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_DomesticSaleInvoice_Model.TransType == "Update" || _DomesticSaleInvoice_Model.TransType == "Edit")
                    {
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }

                        //string SINo = Session["SI_No"].ToString();
                        string SINo = _DomesticSaleInvoice_Model.SI_Number;
                        string SIDate = _DomesticSaleInvoice_Model.SI_Date;

                        //string Type = Session["InvType"].ToString();
                        string VouType = "SV";
                        string Type = _DomesticSaleInvoice_Model.InvType;
                        DataSet ds = _DomesticSaleInvoice_ISERVICE.Edit_SIDetail(CompID, BrchID, VouType, SINo, SIDate, Type, UserID, DocumentMenuId);
                        ViewBag.ItemTCSDetails = ds.Tables[17];
                        _DomesticSaleInvoice_Model.tcs_amt = ds.Tables[0].Rows[0]["tcs_amt"].ToString();
                        _DomesticSaleInvoice_Model.custom_inv_no = ds.Tables[0].Rows[0]["custom_inv_no"].ToString();
                        _DomesticSaleInvoice_Model.InvoiceHeading = ds.Tables[0].Rows[0]["inv_head"].ToString();
                        _DomesticSaleInvoice_Model.BuyersOrderNumberAndDate = ds.Tables[0].Rows[0]["buyer_ord_no_dt"].ToString();
                        _DomesticSaleInvoice_Model.inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        _DomesticSaleInvoice_Model.inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                        _DomesticSaleInvoice_Model.inv_type = ds.Tables[0].Rows[0]["inv_type"].ToString();
                        _DomesticSaleInvoice_Model.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _DomesticSaleInvoice_Model.curr = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _DomesticSaleInvoice_Model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        shipNoList.Add(new ShipNumberList { Ship_number = ds.Tables[0].Rows[0]["sh_no"].ToString() });
                        _DomesticSaleInvoice_Model.shipNumbers = shipNoList;
                        _DomesticSaleInvoice_Model.ship_no = ds.Tables[0].Rows[0]["sh_no"].ToString();
                        _DomesticSaleInvoice_Model.ship_dt = ds.Tables[0].Rows[0]["sh_date"].ToString();
                        ViewBag.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _DomesticSaleInvoice_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _DomesticSaleInvoice_Model.slprsn_id = ds.Tables[0].Rows[0]["emp_id"].ToString();
                        _DomesticSaleInvoice_Model.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                        _DomesticSaleInvoice_Model.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(ValDigit);
                        _DomesticSaleInvoice_Model.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                        _DomesticSaleInvoice_Model.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val"]).ToString(ValDigit);
                        _DomesticSaleInvoice_Model.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                        _DomesticSaleInvoice_Model.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _DomesticSaleInvoice_Model.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _DomesticSaleInvoice_Model.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _DomesticSaleInvoice_Model.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _DomesticSaleInvoice_Model.AmmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _DomesticSaleInvoice_Model.AmmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _DomesticSaleInvoice_Model.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        _DomesticSaleInvoice_Model.inv_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _DomesticSaleInvoice_Model.TaxCalci_ItemName = ds.Tables[1].Rows[0]["item_name"].ToString();
                        _DomesticSaleInvoice_Model.TaxCalci_AssessableValue = ds.Tables[1].Rows[0]["item_gr_val"].ToString();
                        _DomesticSaleInvoice_Model.SI_BillingAddress = ds.Tables[0].Rows[0]["bill_address"].ToString();
                        _DomesticSaleInvoice_Model.SI_Bill_Add_Id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                        _DomesticSaleInvoice_Model.SI_ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                        _DomesticSaleInvoice_Model.SI_Shipp_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                        _DomesticSaleInvoice_Model.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                        _DomesticSaleInvoice_Model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        _DomesticSaleInvoice_Model.benif_name = ds.Tables[0].Rows[0]["benif_name"].ToString();
                        _DomesticSaleInvoice_Model.bank_name = ds.Tables[0].Rows[0]["bank_name"].ToString();
                        _DomesticSaleInvoice_Model.bank_add = ds.Tables[0].Rows[0]["bank_add"].ToString();
                        _DomesticSaleInvoice_Model.acc_no = ds.Tables[0].Rows[0]["acc_no"].ToString();
                        _DomesticSaleInvoice_Model.swift_code = ds.Tables[0].Rows[0]["swift_code"].ToString();
                        _DomesticSaleInvoice_Model.ifsc_code = ds.Tables[0].Rows[0]["ifsc_code"].ToString();
                        _DomesticSaleInvoice_Model.usd_corr_bank = ds.Tables[0].Rows[0]["usd_corr_bank"].ToString();
                        _DomesticSaleInvoice_Model.pre_carr_by = ds.Tables[0].Rows[0]["pre_carr_by"].ToString();
                        _DomesticSaleInvoice_Model.pi_rcpt_carr = ds.Tables[0].Rows[0]["pi_rcpt_carr"].ToString();
                        _DomesticSaleInvoice_Model.ves_fli_no = ds.Tables[0].Rows[0]["ves_fli_no"].ToString();
                        _DomesticSaleInvoice_Model.loading_port = ds.Tables[0].Rows[0]["loading_port"].ToString();
                        _DomesticSaleInvoice_Model.discharge_port = ds.Tables[0].Rows[0]["discharge_port"].ToString();
                        _DomesticSaleInvoice_Model.fin_disti = ds.Tables[0].Rows[0]["fin_disti"].ToString();
                        _DomesticSaleInvoice_Model.container_no = ds.Tables[0].Rows[0]["container_no"].ToString();
                        _DomesticSaleInvoice_Model.other_ref = ds.Tables[0].Rows[0]["other_ref"].ToString();
                        _DomesticSaleInvoice_Model.term_del_pay = ds.Tables[0].Rows[0]["term_del_pay"].ToString();
                        _DomesticSaleInvoice_Model.des_good = ds.Tables[0].Rows[0]["des_good"].ToString();
                        _DomesticSaleInvoice_Model.prof_detail = ds.Tables[0].Rows[0]["prof_detail"].ToString();
                        _DomesticSaleInvoice_Model.declar = ds.Tables[0].Rows[0]["declar"].ToString();
                        _DomesticSaleInvoice_Model.CountryOfOriginOfGoods = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                        _DomesticSaleInvoice_Model.CountryOfFinalDestination = ds.Tables[0].Rows[0]["cntry_fin_dest"].ToString();
                        _DomesticSaleInvoice_Model.ExportersReference = ds.Tables[0].Rows[0]["ext_ref"].ToString();
                        _DomesticSaleInvoice_Model.BuyerIfOtherThenConsignee = ds.Tables[0].Rows[0]["buyer_consig"].ToString();
                        _DomesticSaleInvoice_Model.trade_term = ds.Tables[0].Rows[0]["trade_term"].ToString();
                        _DomesticSaleInvoice_Model.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                        _DomesticSaleInvoice_Model.ExporterAddress = ds.Tables[0].Rows[0]["exp_addr"].ToString();
                        _DomesticSaleInvoice_Model.ConsigneeName = ds.Tables[0].Rows[0]["consig_name"].ToString();
                        _DomesticSaleInvoice_Model.ConsigneeAddress = ds.Tables[0].Rows[0]["consig_addr"].ToString();
                        _DomesticSaleInvoice_Model.CustomInvDate = ds.Tables[0].Rows[0]["custom_inv_dt"].ToString();
                        _DomesticSaleInvoice_Model.ShipFromAddress = ds.Tables[0].Rows[0]["ship_from_address"].ToString();
                        _DomesticSaleInvoice_Model.Custome_Reference = ds.Tables[0].Rows[0]["cust_ref"].ToString();
                        _DomesticSaleInvoice_Model.Payment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                        _DomesticSaleInvoice_Model.Delivery_term = ds.Tables[0].Rows[0]["deli_term"].ToString();
                        _DomesticSaleInvoice_Model.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                        _DomesticSaleInvoice_Model.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                        /*Modifyed By Suraj Maurya on 17-10-2024 to add inv_disc_amt,inv_disc_perc column*/
                        _DomesticSaleInvoice_Model.OrderDiscountInAmount = ds.Tables[0].Rows[0]["inv_disc_amt"].ToString();
                        _DomesticSaleInvoice_Model.OrderDiscountInPercentage = ds.Tables[0].Rows[0]["inv_disc_perc"].ToString();
                        _DomesticSaleInvoice_Model.Declaration_1 = ds.Tables[0].Rows[0]["declar_1"].ToString();
                        _DomesticSaleInvoice_Model.Declaration_2 = ds.Tables[0].Rows[0]["declar_2"].ToString();
                        _DomesticSaleInvoice_Model.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();
                        _DomesticSaleInvoice_Model.ShipTo = ds.Tables[0].Rows[0]["ship_to"].ToString();
                        BindDispatchDetail(_DomesticSaleInvoice_Model, "update", ds);
                        _DomesticSaleInvoice_Model.Address1 = ds.Tables[0].Rows[0]["disp_Addr1"].ToString();
                        _DomesticSaleInvoice_Model.Address2 = ds.Tables[0].Rows[0]["disp_Addr2"].ToString();
                        _DomesticSaleInvoice_Model.Country = ds.Tables[0].Rows[0]["disp_country"].ToString();
                        _DomesticSaleInvoice_Model.State = ds.Tables[0].Rows[0]["disp_state"].ToString();
                        _DomesticSaleInvoice_Model.District = ds.Tables[0].Rows[0]["disp_district"].ToString();
                        _DomesticSaleInvoice_Model.City = ds.Tables[0].Rows[0]["disp_city"].ToString();
                        _DomesticSaleInvoice_Model.Pin = ds.Tables[0].Rows[0]["disp_pin"].ToString();
                        _DomesticSaleInvoice_Model.PvtMark = ds.Tables[0].Rows[0]["pvt_mark"].ToString();
                        _DomesticSaleInvoice_Model.Corporate_Address = ds.Tables[0].Rows[0]["corp_off_addr"].ToString();
                        _DomesticSaleInvoice_Model.Invoice_remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        string nontaxable = ds.Tables[0].Rows[0]["non_taxable"].ToString();
                        if (nontaxable == "Y")
                        {
                            _DomesticSaleInvoice_Model.nontaxable = true;
                        }
                        else
                        {
                            _DomesticSaleInvoice_Model.nontaxable = false;
                        }
                        /*Modifyed By Suraj Maurya on 17-10-2024 to add inv_disc_amt,inv_disc_perc column*/
                        bool revcharge = false;
                        bool.TryParse(ds.Tables[0].Rows[0]["rev_charge"].ToString(), out revcharge);
                        _DomesticSaleInvoice_Model.rev_charge = revcharge;

                        string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                        _DomesticSaleInvoice_Model.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                        if (roundoff_status == "Y")
                        {
                            _DomesticSaleInvoice_Model.RoundOffFlag = true;
                        }
                        else
                        {
                            _DomesticSaleInvoice_Model.RoundOffFlag = false;
                        }

                        //ViewBag.TotalOC = Convert.ToDecimal(ds.Tables[0].Rows[0]["total_oc_amt"]).ToString(ValDigit);
                        //_DomesticSaleInvoice_Model.SI_ItemDetail = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                        _DomesticSaleInvoice_Model.VouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        //_DomesticSaleInvoice_Model.ItemTaxdetails = DataTableToJSONWithStringBuilder(ds.Tables[9]);
                        //_DomesticSaleInvoice_Model.ItemOCdetails = DataTableToJSONWithStringBuilder(ds.Tables[3]);


                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (doc_status == "A" || doc_status == "C")
                        {
                            _DomesticSaleInvoice_Model.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                        }
                        _DomesticSaleInvoice_Model.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                        _DomesticSaleInvoice_Model.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                        ViewBag.GLVoucherNo = _DomesticSaleInvoice_Model.GLVoucherNo;/*add by Hina Sharma on 11-08-2025*/
                        _DomesticSaleInvoice_Model.Status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        _DomesticSaleInvoice_Model.DocumentStatus = doc_status;


                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            _DomesticSaleInvoice_Model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _DomesticSaleInvoice_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _DomesticSaleInvoice_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _DomesticSaleInvoice_Model.CancelFlag = false;
                        }


                        _DomesticSaleInvoice_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _DomesticSaleInvoice_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _DomesticSaleInvoice_Model.Command != "Edit")
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
                                    _DomesticSaleInvoice_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _DomesticSaleInvoice_Model.BtnName = "Refresh";
                                }
                            }
                            if (doc_status == "QP")
                            {
                                //Session["BtnName"] = "Refresh";
                                _DomesticSaleInvoice_Model.BtnName = "Refresh";
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        //ViewBag.MenuPageName = getDocumentName();
                        _DomesticSaleInvoice_Model.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.GLGroup = ds.Tables[7];
                        ViewBag.GLAccount = ds.Tables[7];
                        //ViewBag.TaxDetail = ds.Tables[9]; /*Commented by Suraj on 01-12-2022 for change to common*/
                        ViewBag.ItemTaxDetailsList = ds.Tables[9];
                        ViewBag.OCTaxDetails = ds.Tables[11];
                        //ViewBag.OtherCharge = ds.Tables[3]; /*Commented by Suraj on 01-12-2022 for change to common*/
                        ViewBag.OtherChargeDetails = ds.Tables[3];
                        //ViewBag.TaxCalculatorDetail = ds.Tables[2];
                        ViewBag.ItemTaxDetails = ds.Tables[2];
                        ViewBag.AttechmentDetails = ds.Tables[10];
                        ViewBag.SubItemDetails = ds.Tables[12];
                        _DomesticSaleInvoice_Model.bcurrflag = ds.Tables[14].Rows[0]["bcurrflag"].ToString();
                        ViewBag.CostCenterData = ds.Tables[13];
                        ViewBag.ItemOC_TDSDetails = ds.Tables[16];
                        ViewBag.PaymentScheduleData = ds.Tables[22];
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/LocalSaleInvoiceDetail.cshtml", _DomesticSaleInvoice_Model);
                    }

                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        //Session["DocumentStatus"] = 'D';
                        _DomesticSaleInvoice_Model.DocumentStatus = "D";
                        _DomesticSaleInvoice_Model.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/LocalSaleInvoiceDetail.cshtml", _DomesticSaleInvoice_Model);
                    }
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void BindDispatchDetail(DomesticSaleInvoice_Model _DomesticSaleInvoice_ModelS,string Flag,DataSet dt)
        {
            if(Flag=="update")
            {
                List<CmnCountryList> _CmnCountryList = new List<CmnCountryList>();
                if (dt.Tables[18].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[18].Rows)
                    {
                        CmnCountryList _CountryList = new CmnCountryList();
                        _CountryList.country_name = row["country_name"].ToString();
                        _CountryList.country_id = row["country_id"].ToString();
                        _CmnCountryList.Add(_CountryList);
                    }
                }
                _DomesticSaleInvoice_ModelS.countryList = _CmnCountryList;


                List<CmnStateList> _CmnStateList = new List<CmnStateList>();
                if (dt.Tables[19].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[19].Rows)
                    {
                        CmnStateList _StateList = new CmnStateList();
                        _StateList.state_name = row["state_name"].ToString();
                        _StateList.state_id = row["state_id"].ToString();
                        _CmnStateList.Add(_StateList);
                    }
                }
                _DomesticSaleInvoice_ModelS.StateList = _CmnStateList;


                List<CmnDistrictList> _CmnDistrictList = new List<CmnDistrictList>();
                if (dt.Tables[20].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[20].Rows)
                    {
                        CmnDistrictList _DistrictList = new CmnDistrictList();
                        _DistrictList.district_name = row["district_name"].ToString();
                        _DistrictList.district_id = row["district_id"].ToString();
                        _CmnDistrictList.Add(_DistrictList);
                    }
                }
                _DomesticSaleInvoice_ModelS.DistrictList = _CmnDistrictList;




                List<CmnCityList> _CmnCityList = new List<CmnCityList>();
                if (dt.Tables[21].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[21].Rows)
                    {
                        CmnCityList _CityList = new CmnCityList();
                        _CityList.City_Name = row["city_name"].ToString();
                        _CityList.City_Id = row["city_id"].ToString();
                        _CmnCityList.Add(_CityList);
                    }
                }
                _DomesticSaleInvoice_ModelS.cityLists = _CmnCityList;
            }
            else
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet getdata = new DataSet();
                getdata = _DomesticSaleInvoice_ISERVICE.BindDispatchDetail(CompID, BrchID);

                List<CmnCountryList> _CmnCountryList = new List<CmnCountryList>();
                if (getdata.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in getdata.Tables[1].Rows)
                    {
                        CmnCountryList _CountryList = new CmnCountryList();
                        _CountryList.country_name = row["country_name"].ToString();
                        _CountryList.country_id = row["country_id"].ToString();
                        _CmnCountryList.Add(_CountryList);
                    }
                }
                _DomesticSaleInvoice_ModelS.countryList = _CmnCountryList;


                List<CmnStateList> _CmnStateList = new List<CmnStateList>();
                if (getdata.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in getdata.Tables[2].Rows)
                    {
                        CmnStateList _StateList = new CmnStateList();
                        _StateList.state_name = row["state_name"].ToString();
                        _StateList.state_id = row["state_id"].ToString();
                        _CmnStateList.Add(_StateList);
                    }
                }
                _DomesticSaleInvoice_ModelS.StateList = _CmnStateList;


                List<CmnDistrictList> _CmnDistrictList = new List<CmnDistrictList>();
                if (getdata.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow row in getdata.Tables[3].Rows)
                    {
                        CmnDistrictList _DistrictList = new CmnDistrictList();
                        _DistrictList.district_name = row["district_name"].ToString();
                        _DistrictList.district_id = row["district_id"].ToString();
                        _CmnDistrictList.Add(_DistrictList);
                    }
                }
                _DomesticSaleInvoice_ModelS.DistrictList = _CmnDistrictList;




                List<CmnCityList> _CmnCityList = new List<CmnCityList>();
                if (getdata.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow row in getdata.Tables[4].Rows)
                    {
                        CmnCityList _CityList = new CmnCityList();
                        _CityList.City_Name = row["city_name"].ToString();
                        _CityList.City_Id = row["city_id"].ToString();
                        _CmnCityList.Add(_CityList);
                    }
                }
                _DomesticSaleInvoice_ModelS.cityLists = _CmnCityList;


                if (_DomesticSaleInvoice_ModelS.SI_Number == "" ||
                    _DomesticSaleInvoice_ModelS.SI_Number == null)
                {
                    if (getdata.Tables[0].Rows.Count > 0)
                    {
                        _DomesticSaleInvoice_ModelS.Address1 = getdata.Tables[0].Rows[0]["comp_add"].ToString();
                        _DomesticSaleInvoice_ModelS.Country = getdata.Tables[0].Rows[0]["country_id"].ToString();
                        _DomesticSaleInvoice_ModelS.State = getdata.Tables[0].Rows[0]["state_id"].ToString();
                        _DomesticSaleInvoice_ModelS.District = getdata.Tables[0].Rows[0]["district_id"].ToString();
                        _DomesticSaleInvoice_ModelS.City = getdata.Tables[0].Rows[0]["city_id"].ToString();
                        _DomesticSaleInvoice_ModelS.Pin = getdata.Tables[0].Rows[0]["pin"].ToString();
                    }
                }
            }
           
           
        }

        [HttpPost]
        public JsonResult GetDistrictOnState(string ddlStateID)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable dt = _DomesticSaleInvoice_ISERVICE.GetDistrictOnStateDDL(ddlStateID);
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                dt.Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetCityOnDistrict(string ddlDistrictID)
        {
            JsonResult DataRows = null;
            try
            {
                DataTable dt = _DomesticSaleInvoice_ISERVICE.GetCityOnDistrictDDL(ddlDistrictID);
                DataRow dr;
                dr = dt.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                dt.Rows.InsertAt(dr, 0);
                DataRows = Json(JsonConvert.SerializeObject(dt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult EditSI(string SIId, string SIDate, string InvType, string ListFilterData, string Docid, string WF_status)
        {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
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
            DomesticSaleInvoice_Model _Dblclick = new DomesticSaleInvoice_Model();
            _Dblclick.BtnName = "BtnToDetailPage";
            _Dblclick.Command = "Add";
            _Dblclick.SI_Number = SIId;
            _Dblclick.SI_Date = SIDate;
            _Dblclick.WF_status1 = WF_status;
            //_Dblclick.InvType = InvType;
            if (InvType != null)
            {
                if (InvType == "Domestic" || InvType == "D")
                {
                    _Dblclick.InvType = "D";
                }
                else
                {
                    _Dblclick.InvType = "E";
                }
            }
            _Dblclick.AppStatus = "D";
            _Dblclick.TransType = "Update";
            _Dblclick.DocumentMenuId = Docid;
            TempData["ModelData"] = _Dblclick;
            UrlModel _urlModel = new UrlModel();
            _urlModel.WF_status1 = WF_status;
            _urlModel.BtnName = "BtnToDetailPage";
            _urlModel.Command = "Add";
            _urlModel.SI_Number = SIId;
            _urlModel.SI_Date = SIDate;
            _urlModel.InvType = _Dblclick.InvType;
            _urlModel.TransType = "Update";
            _urlModel.DocumentMenuId = Docid;
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["SI_No"] = SIId;
            //Session["SI_Date"] = SIDate;
            //Session["InvType"] = InvType;
            //if (Session["InvType"].ToString() == "Domestic")
            //{
            //    Session["InvType"] = "D";
            //}
            //else
            //{
            //    Session["InvType"] = "E";
            //}
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("LocalSaleInvoiceDetail", _urlModel);
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
        public ActionResult SaleInvoiceBtnCommand(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model, string command)
        {
            try
            {
                /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_DomesticSaleInvoice_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        DomesticSaleInvoice_Model _DSIAddNewModel = new DomesticSaleInvoice_Model();
                        _DSIAddNewModel.Message = "New";
                        _DSIAddNewModel.Command = "New";
                        _DSIAddNewModel.AppStatus = "D";
                        _DSIAddNewModel.TransType = "Save";
                        _DSIAddNewModel.BtnName = "BtnAddNew";
                        _DSIAddNewModel.DocumentStatus = "D";
                        _DSIAddNewModel.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                        _DSIAddNewModel.InvType = _DomesticSaleInvoice_Model.InvType;
                        TempData["ModelData"] = _DSIAddNewModel;
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.TransType = "Save";
                        _urlModel.BtnName = "BtnAddNew";
                        _urlModel.AppStatus = "D";
                        _urlModel.Command = "New";
                        _urlModel.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                        _urlModel.InvType = _DomesticSaleInvoice_Model.InvType;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_DomesticSaleInvoice_Model.inv_no))
                                return RedirectToAction("EditSI", new { SIId = _DomesticSaleInvoice_Model.inv_no, SIDate = _DomesticSaleInvoice_Model.inv_dt, InvType = _DomesticSaleInvoice_Model.InvType, ListFilterData = _DomesticSaleInvoice_Model.ListFilterData1, Docid = _DomesticSaleInvoice_Model.DocumentMenuId, WF_status = _DomesticSaleInvoice_Model.WFStatus });
                            else
                                _DSIAddNewModel.Command = "Refresh";
                            _DSIAddNewModel.TransType = "Refresh";
                            _DSIAddNewModel.BtnName = "Refresh";
                            _DSIAddNewModel.DocumentStatus = null;
                            TempData["ModelData"] = _DSIAddNewModel;
                            return RedirectToAction("LocalSaleInvoiceDetail", "LocalSaleInvoice", _DSIAddNewModel);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("LocalSaleInvoiceDetail", "LocalSaleInvoice", _urlModel);

                    case "Edit":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSI", new { SIId = _DomesticSaleInvoice_Model.inv_no, SIDate = _DomesticSaleInvoice_Model.inv_dt, InvType = _DomesticSaleInvoice_Model.InvType, ListFilterData = _DomesticSaleInvoice_Model.ListFilterData1, Docid = _DomesticSaleInvoice_Model.DocumentMenuId, WF_status = _DomesticSaleInvoice_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string SInvDate = _DomesticSaleInvoice_Model.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SInvDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSI", new { SIId = _DomesticSaleInvoice_Model.inv_no, SIDate = _DomesticSaleInvoice_Model.inv_dt, InvType = _DomesticSaleInvoice_Model.InvType, ListFilterData = _DomesticSaleInvoice_Model.ListFilterData1, Docid = _DomesticSaleInvoice_Model.DocumentMenuId, WF_status = _DomesticSaleInvoice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        UrlModel _urlEditModel = new UrlModel();
                        if (CheckSaleReturnAgainstSaleInvoice(_DomesticSaleInvoice_Model.inv_no, _DomesticSaleInvoice_Model.inv_dt) == "Used")
                        {
                            //Session["Message"] = "Used";
                            _DomesticSaleInvoice_Model.Message = "Used";
                            _DomesticSaleInvoice_Model.TransType = "Update";
                            _DomesticSaleInvoice_Model.Command = "Used";
                            _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                        }
                        else if (CheckSaleReturnAgainstSaleInvoice(_DomesticSaleInvoice_Model.inv_no, _DomesticSaleInvoice_Model.inv_dt) == "PaymentCreated")
                        {
                            //Session["Message"] = "PaymentCreated";
                            _DomesticSaleInvoice_Model.Message = "PaymentCreated";
                            _DomesticSaleInvoice_Model.TransType = "Update";
                            _DomesticSaleInvoice_Model.Command = "Used";
                            _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                        }
                        else if (CheckSaleReturnAgainstSaleInvoice(_DomesticSaleInvoice_Model.inv_no, _DomesticSaleInvoice_Model.inv_dt) == "CustomInvoiceCreated")
                        {
                            //Session["Message"] = "CustomInvoiceCreated";
                            _DomesticSaleInvoice_Model.Message = "CustomInvoiceCreated";
                            _DomesticSaleInvoice_Model.TransType = "Update";
                            _DomesticSaleInvoice_Model.Command = "Used";
                            _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
                        }
                        else
                        {
                            _DomesticSaleInvoice_Model.TransType = "Update";
                            _DomesticSaleInvoice_Model.Command = command;
                            _DomesticSaleInvoice_Model.BtnName = "BtnEdit";
                            _DomesticSaleInvoice_Model.Message = "New";
                            _DomesticSaleInvoice_Model.SI_Number = _DomesticSaleInvoice_Model.inv_no;
                            _DomesticSaleInvoice_Model.SI_Date = _DomesticSaleInvoice_Model.inv_dt;
                            _DomesticSaleInvoice_Model.InvType = _DomesticSaleInvoice_Model.InvType;
                            _DomesticSaleInvoice_Model.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                            TempData["ModelData"] = _DomesticSaleInvoice_Model;

                            _urlEditModel.TransType = "Update";
                            _urlEditModel.BtnName = "BtnEdit";
                            _urlEditModel.Command = command;
                            _urlEditModel.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                            _urlEditModel.SI_Number = _DomesticSaleInvoice_Model.inv_no;
                            _urlEditModel.SI_Date = _DomesticSaleInvoice_Model.SI_Date;
                            _urlEditModel.InvType = _DomesticSaleInvoice_Model.InvType;
                            return RedirectToAction("LocalSaleInvoiceDetail", _urlEditModel);
                        }

                        _DomesticSaleInvoice_Model.AppStatus = "D";
                        _DomesticSaleInvoice_Model.SI_Number = _DomesticSaleInvoice_Model.inv_no;
                        _DomesticSaleInvoice_Model.SI_Date = _DomesticSaleInvoice_Model.inv_dt;
                        _DomesticSaleInvoice_Model.InvType = _DomesticSaleInvoice_Model.InvType;
                        _DomesticSaleInvoice_Model.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                        _urlEditModel.Command = _DomesticSaleInvoice_Model.Command;
                        _urlEditModel.TransType = "Update";
                        _urlEditModel.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                        _urlEditModel.SI_Number = _DomesticSaleInvoice_Model.inv_no;
                        _urlEditModel.SI_Date = _DomesticSaleInvoice_Model.SI_Date;
                        _urlEditModel.InvType = _DomesticSaleInvoice_Model.InvType;
                        _urlEditModel.BtnName = _DomesticSaleInvoice_Model.BtnName;
                        TempData["ModelData"] = _DomesticSaleInvoice_Model;
                        TempData["ListFilterData"] = _DomesticSaleInvoice_Model.ListFilterData1;
                        return RedirectToAction("LocalSaleInvoiceDetail", _urlEditModel);

                    case "Delete":
                        DomesticSaleInvoice_Model _DSIDeleteModel_Model = new DomesticSaleInvoice_Model();
                        _DomesticSaleInvoice_Model.Command = command;
                        SIDelete(_DomesticSaleInvoice_Model, command);
                        _DSIDeleteModel_Model.Message = "Deleted";
                        _DSIDeleteModel_Model.Command = "Refresh";
                        _DSIDeleteModel_Model.TransType = "Refresh";
                        _DSIDeleteModel_Model.AppStatus = "DL";
                        _DSIDeleteModel_Model.BtnName = "BtnDelete";
                        _DomesticSaleInvoice_Model.InvType = _DomesticSaleInvoice_Model.InvType;
                        _DSIDeleteModel_Model.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                        UrlModel _url_Delete_Model = new UrlModel();
                        _url_Delete_Model.TransType = "Save";
                        _url_Delete_Model.BtnName = "BtnDelete";
                        _url_Delete_Model.Command = "Add";
                        _url_Delete_Model.InvType = _DomesticSaleInvoice_Model.InvType;
                        _url_Delete_Model.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                        TempData["ModelData"] = _DSIDeleteModel_Model;
                        TempData["ListFilterData"] = _DomesticSaleInvoice_Model.ListFilterData1;
                        return RedirectToAction("LocalSaleInvoiceDetail", _url_Delete_Model);

                    case "Save":
                        //Session["Command"] = command;
                        string CheckDependecyMessage = CheckSaleReturnAgainstSaleInvoice(_DomesticSaleInvoice_Model.inv_no, _DomesticSaleInvoice_Model.inv_dt);
                        if (CheckDependecyMessage == "Used" || CheckDependecyMessage == "PaymentCreated" || CheckDependecyMessage == "CustomInvoiceCreated")
                        {
                            UrlModel _urlEditModel1 = new UrlModel();
                            //Session["Message"] = "CustomInvoiceCreated";
                            _DomesticSaleInvoice_Model.Message = CheckDependecyMessage;// "CustomInvoiceCreated";
                            _DomesticSaleInvoice_Model.TransType = "Update";
                            _DomesticSaleInvoice_Model.Command = "Used";
                            _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";

                            _DomesticSaleInvoice_Model.AppStatus = "D";
                            _DomesticSaleInvoice_Model.SI_Number = _DomesticSaleInvoice_Model.inv_no;
                            _DomesticSaleInvoice_Model.SI_Date = _DomesticSaleInvoice_Model.inv_dt;
                            _DomesticSaleInvoice_Model.InvType = _DomesticSaleInvoice_Model.InvType;
                            _DomesticSaleInvoice_Model.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                            _urlEditModel1.Command = _DomesticSaleInvoice_Model.Command;
                            _urlEditModel1.TransType = "Update";
                            _urlEditModel1.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                            _urlEditModel1.SI_Number = _DomesticSaleInvoice_Model.inv_no;
                            _urlEditModel1.SI_Date = _DomesticSaleInvoice_Model.SI_Date;
                            _urlEditModel1.InvType = _DomesticSaleInvoice_Model.InvType;
                            _urlEditModel1.BtnName = _DomesticSaleInvoice_Model.BtnName;
                            TempData["ModelData"] = _DomesticSaleInvoice_Model;
                            TempData["ListFilterData"] = _DomesticSaleInvoice_Model.ListFilterData1;
                            return RedirectToAction("LocalSaleInvoiceDetail", _urlEditModel1);
                        }
                        
                        _DomesticSaleInvoice_Model.Command = command;
                        SaveSIDetail(_DomesticSaleInvoice_Model);
                        if (_DomesticSaleInvoice_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_DomesticSaleInvoice_Model.Message == "DocModify")
                        {
                            if (_DomesticSaleInvoice_Model.DocumentMenuId != null)
                            {
                                if (_DomesticSaleInvoice_Model.DocumentMenuId == "105103140")
                                {
                                    DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                                    _DomesticSaleInvoice_Model.MenuDocumentId = DocumentMenuId;

                                }
                                if (_DomesticSaleInvoice_Model.DocumentMenuId == "105103145125")
                                {
                                    DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                                    _DomesticSaleInvoice_Model.MenuDocumentId = DocumentMenuId;
                                }
                            }
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            CommonPageDetails();
                            if (Session["CompId"] != null)
                            {
                                CompID = Session["CompId"].ToString();
                            }
                            BrchID = Session["BranchId"].ToString();
                            if (Session["UserId"] != null)
                            {
                                UserID = Session["UserId"].ToString();
                            }
                            if (Session["crm"] != null)
                            {
                                crm = Session["crm"].ToString();
                            }
                            if (UserID == "1001")
                            {
                                crm = "Y";
                            }
                            ViewBag.crm = crm;
                            _DomesticSaleInvoice_Model.DocumentMenuId = DocumentMenuId;
                            _DomesticSaleInvoice_Model.GstApplicable = ViewBag.GstApplicable;
                            List<CustomerName> _CustomerNameList = new List<CustomerName>();
                            CustomerName _CustomerName = new CustomerName();
                            _CustomerName.cust_name = _DomesticSaleInvoice_Model.CustName;
                            _CustomerName.cust_id = _DomesticSaleInvoice_Model.cust_id;
                            _CustomerNameList.Add(_CustomerName);
                            _DomesticSaleInvoice_Model.CustomerNameList = _CustomerNameList;

                            GetSalesPersonList(_DomesticSaleInvoice_Model);

                            List<ShipNumberList> shipNoList = new List<ShipNumberList>();
                            shipNoList.Add(new ShipNumberList { Ship_number = _DomesticSaleInvoice_Model.ShipNum/*, Ship_date = "0"*/ });
                            _DomesticSaleInvoice_Model.shipNumbers = shipNoList;
                            _DomesticSaleInvoice_Model.inv_no = DateTime.Now.ToString();
                            _DomesticSaleInvoice_Model.SI_BillingAddress = _DomesticSaleInvoice_Model.SI_BillingAddress;
                            _DomesticSaleInvoice_Model.SI_ShippingAddress = _DomesticSaleInvoice_Model.SI_ShippingAddress;
                            _DomesticSaleInvoice_Model.curr = _DomesticSaleInvoice_Model.curr;
                            _DomesticSaleInvoice_Model.conv_rate = _DomesticSaleInvoice_Model.conv_rate;
                            _DomesticSaleInvoice_Model.FRoundOffAmt = _DomesticSaleInvoice_Model.FRoundOffAmt;
                            if (_DomesticSaleInvoice_Model.DocumentMenuId == "105103140")
                            {
                                DataSet dt1 = new DataSet();
                                BindDispatchDetail(_DomesticSaleInvoice_Model, "", dt1);
                            }

                            ViewBag.ItemDetails = ViewData["ItemDetails"]; 
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTaxDetailsList = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.OCTaxDetails = ViewData["TaxDetails"];
                            ViewBag.GLGroup = ViewData["VouDetail"];
                            //ViewBag.GLTOtal = ViewData["VouDetail"];
                            ViewBag.CostCenterData = ViewData["CCdetail"];
                            ViewBag.SubItemDetails = ViewData["SubItemdetail"];
                            ViewBag.ItemOC_TDSDetails = ViewData["OCTDSDetails"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _DomesticSaleInvoice_Model.BtnName = "Refresh";
                            _DomesticSaleInvoice_Model.Command = "Refresh";
                            _DomesticSaleInvoice_Model.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/LocalSaleInvoiceDetail.cshtml", _DomesticSaleInvoice_Model);


                        }
                        else
                        {

                            //Session["SI_No"] = Session["SI_No"].ToString();
                            //Session["SI_Date"] = Session["SI_Date"].ToString();
                            TempData["ModelData"] = _DomesticSaleInvoice_Model;
                            UrlModel URLModelSave = new UrlModel();
                            URLModelSave.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                            URLModelSave.CustType = _DomesticSaleInvoice_Model.CustType;
                            URLModelSave.TransType = "Update";
                            URLModelSave.BtnName = "BtnSave";
                            URLModelSave.Command = command;
                            URLModelSave.SI_Number = _DomesticSaleInvoice_Model.SI_Number;
                            URLModelSave.SI_Date = _DomesticSaleInvoice_Model.SI_Date;
                            URLModelSave.InvType = _DomesticSaleInvoice_Model.InvType;
                            URLModelSave.FRoundOffAmt = _DomesticSaleInvoice_Model.FRoundOffAmt;
                            
                            TempData["ListFilterData"] = _DomesticSaleInvoice_Model.ListFilterData1;
                            return RedirectToAction("LocalSaleInvoiceDetail", URLModelSave);
                        }
                    case "Forward":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSI", new { SIId = _DomesticSaleInvoice_Model.inv_no, SIDate = _DomesticSaleInvoice_Model.inv_dt, InvType = _DomesticSaleInvoice_Model.InvType, ListFilterData = _DomesticSaleInvoice_Model.ListFilterData1, Docid = _DomesticSaleInvoice_Model.DocumentMenuId, WF_status = _DomesticSaleInvoice_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string SinvDate1 = _DomesticSaleInvoice_Model.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SinvDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSI", new { SIId = _DomesticSaleInvoice_Model.inv_no, SIDate = _DomesticSaleInvoice_Model.inv_dt, InvType = _DomesticSaleInvoice_Model.InvType, ListFilterData = _DomesticSaleInvoice_Model.ListFilterData1, Docid = _DomesticSaleInvoice_Model.DocumentMenuId, WF_status = _DomesticSaleInvoice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSI", new { SIId = _DomesticSaleInvoice_Model.inv_no, SIDate = _DomesticSaleInvoice_Model.inv_dt, InvType = _DomesticSaleInvoice_Model.InvType, ListFilterData = _DomesticSaleInvoice_Model.ListFilterData1, Docid = _DomesticSaleInvoice_Model.DocumentMenuId, WF_status = _DomesticSaleInvoice_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        string SinvDate2 = _DomesticSaleInvoice_Model.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SinvDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSI", new { SIId = _DomesticSaleInvoice_Model.inv_no, SIDate = _DomesticSaleInvoice_Model.inv_dt, InvType = _DomesticSaleInvoice_Model.InvType, ListFilterData = _DomesticSaleInvoice_Model.ListFilterData1, Docid = _DomesticSaleInvoice_Model.DocumentMenuId, WF_status = _DomesticSaleInvoice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        _DomesticSaleInvoice_Model.Command = command;
                        //PC_No = _ProductionConfirmation_Model.confirmation_no;
                        //PC_Date = _ProductionConfirmation_Model.confirmation_dt;
                        SIListApprove(_DomesticSaleInvoice_Model, "", "", "", _DomesticSaleInvoice_Model.PV_Nurration, _DomesticSaleInvoice_Model.BP_Nurration,_DomesticSaleInvoice_Model.DN_Narration);
                        TempData["ModelData"] = _DomesticSaleInvoice_Model;
                        UrlModel _urlApproveModel = new UrlModel();
                        _urlApproveModel.SI_Number = _DomesticSaleInvoice_Model.SI_Number;
                        _urlApproveModel.SI_Date = _DomesticSaleInvoice_Model.SI_Date;
                        _urlApproveModel.InvType = _DomesticSaleInvoice_Model.InvType;
                        _urlApproveModel.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                        _urlApproveModel.TransType = "Update";
                        _urlApproveModel.Command = "Approve";
                        _urlApproveModel.AppStatus = "D";
                        _urlApproveModel.BtnName = "BtnEdit";
                        _urlApproveModel.FRoundOffAmt = _DomesticSaleInvoice_Model.FRoundOffAmt;
                        TempData["ListFilterData"] = _DomesticSaleInvoice_Model.ListFilterData1;
                        return RedirectToAction("LocalSaleInvoiceDetail", _urlApproveModel);

                    case "Refresh":
                        UrlModel URLModelRefresh = new UrlModel();
                        DomesticSaleInvoice_Model __DomesticSaleInvoice_ModelRefresh = new DomesticSaleInvoice_Model();
                        __DomesticSaleInvoice_ModelRefresh.BtnName = "Refresh";
                        __DomesticSaleInvoice_ModelRefresh.Command = command;
                        __DomesticSaleInvoice_ModelRefresh.TransType = "Save";
                        __DomesticSaleInvoice_ModelRefresh.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                        __DomesticSaleInvoice_ModelRefresh.CustType = _DomesticSaleInvoice_Model.CustType;
                        TempData["ModelData"] = __DomesticSaleInvoice_ModelRefresh;
                        URLModelRefresh.DocumentMenuId = __DomesticSaleInvoice_ModelRefresh.DocumentMenuId;
                        URLModelRefresh.BtnName = __DomesticSaleInvoice_ModelRefresh.BtnName;
                        URLModelRefresh.Command = __DomesticSaleInvoice_ModelRefresh.Command;
                        URLModelRefresh.TransType = __DomesticSaleInvoice_ModelRefresh.TransType;
                        URLModelRefresh.InvType = __DomesticSaleInvoice_ModelRefresh.InvType;
                        TempData["ListFilterData"] = _DomesticSaleInvoice_Model.ListFilterData1;
                        return RedirectToAction("LocalSaleInvoiceDetail", URLModelRefresh);

                    case "Print":
                        return GenratePdfFile(_DomesticSaleInvoice_Model);
                    case "BacktoList":
                        SI_ListModel _SI_ListModel = new SI_ListModel();
                        TempData["ListFilterData"] = _DomesticSaleInvoice_Model.ListFilterData1;
                        _SI_ListModel.WF_status = _DomesticSaleInvoice_Model.WF_status1;
                        if (_DomesticSaleInvoice_Model.DocumentMenuId != null)
                        {
                            if (_DomesticSaleInvoice_Model.DocumentMenuId == "105103140")
                            {

                                return RedirectToAction("LocalSaleInvoiceList", _SI_ListModel);
                            }
                            else if (_DomesticSaleInvoice_Model.DocumentMenuId == "105103145125")
                            {
                                return RedirectToAction("CommercialInvoiceList", _SI_ListModel);
                            }
                        }
                        return RedirectToAction("LocalSaleInvoiceList", "LocalSaleInvoice");

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
        [NonAction]
        public ActionResult SaveSIDetail(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model)
        {
            string SaveMessage = "";
            string PageName = _DomesticSaleInvoice_Model.Title.Replace(" ", "");

            try
            {
                DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
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
                DataTable DtblVouGLDetail = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblOCDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable dtheader = new DataTable();
                DataTable CRCostCenterDetails = new DataTable();
                DataTable DtblOCTdsDetail = new DataTable();
                DataTable DtblTcsDetail = new DataTable();//for TCS Added by Suraj Maurya on 03-02-2025

                DataTable DTPaymentSchedule = new DataTable();

                DtblHDetail = ToDtblHDetail(_DomesticSaleInvoice_Model);
                DtblItemDetail = ToDtblItemDetail(_DomesticSaleInvoice_Model.SI_ItemDetail);
                DtblVouGLDetail = ToDtblVouGlDetail(_DomesticSaleInvoice_Model.VouGlDetails);
                DtblTaxDetail = ToDtblTaxDetail(_DomesticSaleInvoice_Model.ItemTaxdetails);
                DtblOCTaxDetail = ToDtblTaxDetail(_DomesticSaleInvoice_Model.ItemOCTaxdetails);
                DtblOCDetail = ToDtblOCDetail(_DomesticSaleInvoice_Model.ItemOCdetails);
                DtblOCTdsDetail = ToDtblOCTdsDetail(_DomesticSaleInvoice_Model.oc_tds_details); /*----------Add By Hina on 10-07-2024 for tds on OC of third party supplier-----------------------------*/
                DtblTcsDetail = ToDtblTcsDetail(_DomesticSaleInvoice_Model.tcs_details);

                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("ship_qty", typeof(string));
                dtSubItem.Columns.Add("ship_foc_qty", typeof(string));
                if (_DomesticSaleInvoice_Model.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(_DomesticSaleInvoice_Model.SubItemDetailsDt);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["ship_qty"] = jObject2[i]["qty"].ToString();
                        dtrowItemdetails["ship_foc_qty"] = jObject2[i]["foc_qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                    ViewData["SubItemdetail"] = dtSubitemdetail(jObject2);
                }

                /*------------------Sub Item end----------------------*/

                /**----------------Cost Center Section--------------------*/
                DataTable CC_Details = new DataTable();
                CC_Details.Columns.Add("vou_sr_no", typeof(string));
                CC_Details.Columns.Add("gl_sr_no", typeof(string));
                CC_Details.Columns.Add("acc_id", typeof(string));
                CC_Details.Columns.Add("cc_id", typeof(int));
                CC_Details.Columns.Add("cc_val_id", typeof(int));
                CC_Details.Columns.Add("cc_amt", typeof(string));

                JArray JAObj = JArray.Parse(_DomesticSaleInvoice_Model.CC_DetailList);
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
                DataTable PaymentSchedule = new DataTable();


                PaymentSchedule.Columns.Add("sr_no", typeof(int));
                PaymentSchedule.Columns.Add("paym_mst", typeof(string));
                PaymentSchedule.Columns.Add("precentage", typeof(double));
                PaymentSchedule.Columns.Add("amt", typeof(double));

                if (_DomesticSaleInvoice_Model.HdnPaymentSchedule != null)
                {
                    JArray JPS = JArray.Parse(_DomesticSaleInvoice_Model.HdnPaymentSchedule);

                    for (int i = 0; i < JPS.Count; i++)
                    {
                        DataRow dtrowLines = PaymentSchedule.NewRow();

                        // 2. FIX: Convert values safely instead of just .ToString()
                        // This ensures "10.5" becomes a number (10.5), not a text string
                        dtrowLines["sr_no"] = Convert.ToInt32(JPS[i]["sr_no"]);
                        dtrowLines["paym_mst"] = JPS[i]["paym_mst"].ToString();
                        dtrowLines["precentage"] = Convert.ToDouble(JPS[i]["precentage"]);
                        dtrowLines["amt"] = Convert.ToDouble(JPS[i]["amt"]);

                        PaymentSchedule.Rows.Add(dtrowLines);
                    }
                }
                // Parse JSON


                DTPaymentSchedule = PaymentSchedule;
                ViewData["PaymentSchedule"] = DTPaymentSchedule;

                DataTable dtAttachment = new DataTable();
                var _SaleInvoiceModelattch = TempData["ModelDataattch"] as SaleInvoiceModelattch;
                TempData["ModelDataattch"] = null;
                if (_DomesticSaleInvoice_Model.attatchmentdetail != null)
                {
                    if (_SaleInvoiceModelattch != null)
                    {
                        if (_SaleInvoiceModelattch.AttachMentDetailItmStp != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)

                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _SaleInvoiceModelattch.AttachMentDetailItmStp as DataTable;
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
                        if (_DomesticSaleInvoice_Model.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _DomesticSaleInvoice_Model.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_DomesticSaleInvoice_Model.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_DomesticSaleInvoice_Model.inv_no))
                            {
                                dtrowAttachment1["id"] = _DomesticSaleInvoice_Model.inv_no;
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
                    if (_DomesticSaleInvoice_Model.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string InvoiceCode = string.Empty;
                            if (!string.IsNullOrEmpty(_DomesticSaleInvoice_Model.inv_no))
                            {
                                InvoiceCode = _DomesticSaleInvoice_Model.inv_no;
                            }
                            else
                            {
                                InvoiceCode = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + InvoiceCode.Replace("/", "") + "*");

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
                string Nurr = "";
                string CN_Nurr = "";
                if (_DomesticSaleInvoice_Model.CancelFlag)
                {
                    Nurr = _DomesticSaleInvoice_Model.PV_Nurration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                    CN_Nurr = _DomesticSaleInvoice_Model.CN_Nurration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }
                string inv_disc_amt = _DomesticSaleInvoice_Model.OrderDiscountInAmount;
                string inv_disc_perc = _DomesticSaleInvoice_Model.OrderDiscountInPercentage;
                var nontax = "N";
                string nontaxable = _DomesticSaleInvoice_Model.nontaxable.ToString();
                if (nontaxable == "False")
                {
                    nontax = "N";
                }
                else
                {
                    nontax = "Y";
                }
                SaveMessage = _DomesticSaleInvoice_ISERVICE.InsertSI_Details(DtblHDetail, DtblItemDetail, DtblVouGLDetail, DtblTaxDetail
                    , DtblOCTaxDetail, DtblOCDetail, DtblOCTdsDetail, dtSubItem, DtblAttchDetail, CRCostCenterDetails, DtblTcsDetail
                    , Nurr, CN_Nurr, inv_disc_amt,inv_disc_perc, _DomesticSaleInvoice_Model.Declaration_1, _DomesticSaleInvoice_Model.Declaration_2,
                    _DomesticSaleInvoice_Model.Invoice_Heading, nontax, _DomesticSaleInvoice_Model.ShipTo,DTPaymentSchedule, _DomesticSaleInvoice_Model.Corporate_Address,
                    _DomesticSaleInvoice_Model.Invoice_remarks);
                if (SaveMessage == "DocModify")
                {
                    _DomesticSaleInvoice_Model.Message = "DocModify";
                    _DomesticSaleInvoice_Model.BtnName = "Refresh";
                    _DomesticSaleInvoice_Model.Command = "Refresh";
                    TempData["ModelData"] = _DomesticSaleInvoice_Model;
                    return RedirectToAction("ShipmentDetail");
                }
                else
                {
                    string[] Data = SaveMessage.Split(',');

                    string SINo = Data[0];
                    if (SINo == "Data_Not_Found")
                    {
                        var a = SaveMessage.Split(',');
                        var msg = SINo.Replace("_", " ") + " " + SaveMessage.Split(',')[1] + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _DomesticSaleInvoice_Model.Message = SINo.Split(',')[0].Replace("_", "");
                        return RedirectToAction("LocalSaleInvoiceDetail");
                    }
                    string SI_No = SINo.Replace("/", "");
                    string Message = Data[2];
                    string SIDate = Data[1];
                    string Message1 = Data[4];
                    string StatusCode = Data[3];
                    /*-----------------Attachment Section Start------------------------*/
                    if (Message == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        //{
                        //    Guid = Session["Guid"].ToString();
                        //}
                        if (_SaleInvoiceModelattch != null)
                        {
                            //if (Session["Guid"] != null)
                            if (_SaleInvoiceModelattch.Guid != null)
                            {
                                //Guid = Session["Guid"].ToString();
                                Guid = _SaleInvoiceModelattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, SI_No, _DomesticSaleInvoice_Model.TransType, DtblAttchDetail);

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
                        //                string SI_No1 = SI_No.Replace("/", "");
                        //                string img_nm = CompID + BrchID + SI_No1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                    if (Message1 == "Cancelled")
                    {
                        //Session["Message"] = "Cancelled";
                        //Session["Command"] = "Update";
                        //TempData["SI_No"] = _DomesticSaleInvoice_Model.inv_no;
                        //TempData["SI_Date"] = _DomesticSaleInvoice_Model.inv_dt;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "Refresh";
                        try
                        {
                            ViewBag.PrintFormat = PrintFormatDataTable(_DomesticSaleInvoice_Model, null);
                            //string fileName = "SI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "TaxInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_DomesticSaleInvoice_Model.DocumentMenuId, _DomesticSaleInvoice_Model.inv_no, _DomesticSaleInvoice_Model.inv_dt, fileName, null, _DomesticSaleInvoice_Model.GstApplicable, _DomesticSaleInvoice_Model.DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, _DomesticSaleInvoice_Model.DocumentMenuId, _DomesticSaleInvoice_Model.inv_no, "C", Session["UserId"].ToString(), "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _DomesticSaleInvoice_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _DomesticSaleInvoice_Model.Message = _DomesticSaleInvoice_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                       // _DomesticSaleInvoice_Model.Message = "Cancelled";
                        _DomesticSaleInvoice_Model.Command = "Update";
                        _DomesticSaleInvoice_Model.TransType = "Update";
                        _DomesticSaleInvoice_Model.AppStatus = "D";
                        _DomesticSaleInvoice_Model.BtnName = "Refresh";
                        _DomesticSaleInvoice_Model.SI_Number = _DomesticSaleInvoice_Model.inv_no;
                        _DomesticSaleInvoice_Model.SI_Date = _DomesticSaleInvoice_Model.inv_dt;
                        _DomesticSaleInvoice_Model.InvType = _DomesticSaleInvoice_Model.OrderType;

                        // return RedirectToAction("LocalSaleInvoiceDetail");
                    }

                    else if (Message == "Update" || Message == "Save")
                    {
                        //    Session["Message"] = "Save";
                        //    Session["Command"] = "Update";
                        //    Session["SI_No"] = SINo;
                        //    Session["SI_Date"] = SIDate;
                        //    Session["TransType"] = "Update";
                        //    Session["InvType"] = _DomesticSaleInvoice_Model.OrderType;
                        //    Session["AppStatus"] = 'D';
                        //    Session["BtnName"] = "BtnSave";

                        _DomesticSaleInvoice_Model.Message = "Save";
                        _DomesticSaleInvoice_Model.Command = "Update";
                        _DomesticSaleInvoice_Model.SI_Number = SINo;
                        _DomesticSaleInvoice_Model.SI_Date = SIDate;
                        _DomesticSaleInvoice_Model.TransType = "Update";
                        _DomesticSaleInvoice_Model.InvType = _DomesticSaleInvoice_Model.OrderType;
                        _DomesticSaleInvoice_Model.AppStatus = "D";
                        _DomesticSaleInvoice_Model.BtnName = "BtnSave";

                    }
                    //TempData["ModelData"] = _DomesticSaleInvoice_Model;
                    return RedirectToAction("LocalSaleInvoiceDetail");
                }
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_DomesticSaleInvoice_Model.TransType == "Save")
                    {
                        string Guid = "";

                        //if (Session["Guid"] != null)
                        //{
                        //    Guid = Session["Guid"].ToString();
                        //}
                        if (_DomesticSaleInvoice_Model.Guid != null)
                        {
                            Guid = _DomesticSaleInvoice_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private ActionResult SIDelete(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model, string command)
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
                string doc_no = _DomesticSaleInvoice_Model.inv_no;
                string Message = _DomesticSaleInvoice_ISERVICE.Delete_SI_Detail(_DomesticSaleInvoice_Model, CompID, BrchID);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = _DomesticSaleInvoice_Model.Title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + br_id, PageName, doc_no1, Server);
                }

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["SI_No"] = "";
                //_DomesticSaleInvoice_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";

                return RedirectToAction("LocalSaleInvoiceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //  return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private DataTable ToDtblHDetail(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model)
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
                dtheader.Columns.Add("MenuDocumentId", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("roundoff", typeof(string));
                dtheader.Columns.Add("pm_flag", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("inv_type", typeof(string));
                dtheader.Columns.Add("inv_no", typeof(string));
                dtheader.Columns.Add("inv_dt", typeof(string));
                dtheader.Columns.Add("cust_id", typeof(int));

                dtheader.Columns.Add("curr_id", typeof(int));
                dtheader.Columns.Add("conv_rate", typeof(double));
                dtheader.Columns.Add("sale_per", typeof(int));
                dtheader.Columns.Add("user_id", typeof(int));
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(double));
                dtheader.Columns.Add("tax_amt", typeof(double));
                dtheader.Columns.Add("oc_amt", typeof(double));
                dtheader.Columns.Add("net_val", typeof(double));
                dtheader.Columns.Add("net_val_bs", typeof(double));
                dtheader.Columns.Add("Narration", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(string));
                dtheader.Columns.Add("ship_add_id", typeof(string));
                dtheader.Columns.Add("benif_name", typeof(string));
                dtheader.Columns.Add("bank_name", typeof(string));
                dtheader.Columns.Add("bank_add", typeof(string));
                dtheader.Columns.Add("acc_no", typeof(string));
                dtheader.Columns.Add("swift_code", typeof(string));
                dtheader.Columns.Add("ifsc_code", typeof(string));
                dtheader.Columns.Add("usd_corr_bank", typeof(string));

                dtheader.Columns.Add("pre_carr_by", typeof(string));
                dtheader.Columns.Add("pi_rcpt_carr", typeof(string));
                dtheader.Columns.Add("ves_fli_no", typeof(string));
                dtheader.Columns.Add("loading_port", typeof(string));
                dtheader.Columns.Add("discharge_port", typeof(string));
                dtheader.Columns.Add("fin_disti", typeof(string));
                dtheader.Columns.Add("container_no", typeof(string));
                dtheader.Columns.Add("other_ref", typeof(string));
                dtheader.Columns.Add("term_del_pay", typeof(string));
                dtheader.Columns.Add("des_good", typeof(string));
                dtheader.Columns.Add("prof_detail", typeof(string));
                dtheader.Columns.Add("declar", typeof(string));
                dtheader.Columns.Add("cntry_origin", typeof(string));
                dtheader.Columns.Add("cntry_fin_dest", typeof(string));
                dtheader.Columns.Add("ext_ref", typeof(string));
                dtheader.Columns.Add("buyer_consig", typeof(string));
                dtheader.Columns.Add("trade_term", typeof(string));
                dtheader.Columns.Add("consig_addr", typeof(string));
                dtheader.Columns.Add("inv_heading", typeof(string));
                dtheader.Columns.Add("buyers_ord_and_dt", typeof(string));
                dtheader.Columns.Add("custom_inv_no", typeof(string));
                dtheader.Columns.Add("exp_addr", typeof(string));
                dtheader.Columns.Add("consig_name", typeof(string));
                dtheader.Columns.Add("custom_inv_dt", typeof(string));
                dtheader.Columns.Add("cust_ref", typeof(string));
                dtheader.Columns.Add("pay_term", typeof(string));
                dtheader.Columns.Add("deli_term", typeof(string));
                dtheader.Columns.Add("rev_charge", typeof(string));
                dtheader.Columns.Add("tcs_amt", typeof(string));
                dtheader.Columns.Add("ship_from_address", typeof(string));
                dtheader.Columns.Add("disp_Addr1", typeof(string));
                dtheader.Columns.Add("disp_Addr2", typeof(string));
                dtheader.Columns.Add("disp_country", typeof(int));
                dtheader.Columns.Add("disp_state", typeof(int));
                dtheader.Columns.Add("disp_district", typeof(int));
                dtheader.Columns.Add("disp_city", typeof(int));
                dtheader.Columns.Add("disp_pin", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));
                dtheader.Columns.Add("pvt_mark", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                //dtrowHeader["TransType"] = Session["TransType"].ToString();
                if (_DomesticSaleInvoice_Model.inv_no != null)
                {
                    dtrowHeader["TransType"] = "Update";
                }
                else
                {
                    dtrowHeader["TransType"] = "Save";
                }
                dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                string cancelflag = _DomesticSaleInvoice_Model.CancelFlag.ToString();
                if (cancelflag == "False")
                {
                    dtrowHeader["Cancelled"] = "N";
                }
                else
                {
                    dtrowHeader["Cancelled"] = "Y";
                }
                string roundoffflag = _DomesticSaleInvoice_Model.RoundOffFlag.ToString();
                if (roundoffflag == "False")
                {
                    dtrowHeader["roundoff"] = "N";
                }
                else
                {
                    dtrowHeader["roundoff"] = "Y";
                }
                dtrowHeader["pm_flag"] = _DomesticSaleInvoice_Model.pmflagval;
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["inv_type"] = _DomesticSaleInvoice_Model.OrderType;
                dtrowHeader["inv_no"] = _DomesticSaleInvoice_Model.inv_no;
                dtrowHeader["inv_dt"] = _DomesticSaleInvoice_Model.inv_dt;
                dtrowHeader["cust_id"] = _DomesticSaleInvoice_Model.cust_id;
                dtrowHeader["curr_id"] = _DomesticSaleInvoice_Model.curr_id;
                dtrowHeader["conv_rate"] = _DomesticSaleInvoice_Model.conv_rate;
                dtrowHeader["sale_per"] = _DomesticSaleInvoice_Model.slprsn_id;
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                dtrowHeader["inv_status"] = "D";
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["gr_val"] = _DomesticSaleInvoice_Model.GrVal;
                dtrowHeader["tax_amt"] = IsNull(_DomesticSaleInvoice_Model.TaxAmt, "0");
                dtrowHeader["oc_amt"] = IsNull(_DomesticSaleInvoice_Model.OcAmt, "0");
                dtrowHeader["net_val"] = _DomesticSaleInvoice_Model.NetValSpec;
                dtrowHeader["net_val_bs"] = _DomesticSaleInvoice_Model.NetValBs;
                dtrowHeader["Narration"] = _DomesticSaleInvoice_Model.Narration;
                dtrowHeader["bill_add_id"] = _DomesticSaleInvoice_Model.SI_Bill_Add_Id;
                dtrowHeader["ship_add_id"] = _DomesticSaleInvoice_Model.SI_Shipp_Add_Id;
                dtrowHeader["benif_name"] = _DomesticSaleInvoice_Model.benif_name;
                dtrowHeader["bank_name"] = _DomesticSaleInvoice_Model.bank_name;
                dtrowHeader["bank_add"] = _DomesticSaleInvoice_Model.bank_address;
                dtrowHeader["acc_no"] = _DomesticSaleInvoice_Model.acc_num;
                dtrowHeader["swift_code"] = _DomesticSaleInvoice_Model.shift_cd;
                dtrowHeader["ifsc_code"] = _DomesticSaleInvoice_Model.ifsc_cd;
                dtrowHeader["usd_corr_bank"] = _DomesticSaleInvoice_Model.usd_corr_bank;

                dtrowHeader["pre_carr_by"] = _DomesticSaleInvoice_Model.pre_carr_by;
                dtrowHeader["pi_rcpt_carr"] = _DomesticSaleInvoice_Model.pi_rcpt_carr;
                dtrowHeader["ves_fli_no"] = _DomesticSaleInvoice_Model.ves_fli_no;
                dtrowHeader["loading_port"] = _DomesticSaleInvoice_Model.loading_port;
                dtrowHeader["discharge_port"] = _DomesticSaleInvoice_Model.discharge_port;
                dtrowHeader["fin_disti"] = _DomesticSaleInvoice_Model.fin_disti;
                dtrowHeader["container_no"] = _DomesticSaleInvoice_Model.container_no;
                dtrowHeader["other_ref"] = _DomesticSaleInvoice_Model.other_ref;
                dtrowHeader["term_del_pay"] = _DomesticSaleInvoice_Model.term_del_pay;
                dtrowHeader["des_good"] = _DomesticSaleInvoice_Model.des_good;
                dtrowHeader["prof_detail"] = _DomesticSaleInvoice_Model.prof_detail;
                dtrowHeader["declar"] = _DomesticSaleInvoice_Model.declar;
                dtrowHeader["cntry_origin"] = _DomesticSaleInvoice_Model.CountryOfOriginOfGoods;
                dtrowHeader["cntry_fin_dest"] = _DomesticSaleInvoice_Model.CountryOfFinalDestination;
                dtrowHeader["ext_ref"] = _DomesticSaleInvoice_Model.ExportersReference;
                dtrowHeader["buyer_consig"] = _DomesticSaleInvoice_Model.BuyerIfOtherThenConsignee;
                dtrowHeader["trade_term"] = _DomesticSaleInvoice_Model.trade_term;
                dtrowHeader["consig_addr"] = _DomesticSaleInvoice_Model.ConsigneeAddress;
                dtrowHeader["inv_heading"] = _DomesticSaleInvoice_Model.InvoiceHeading;
                dtrowHeader["buyers_ord_and_dt"] = _DomesticSaleInvoice_Model.BuyersOrderNumberAndDate;
                dtrowHeader["custom_inv_no"] = _DomesticSaleInvoice_Model.custom_inv_no;
                dtrowHeader["exp_addr"] = _DomesticSaleInvoice_Model.ExporterAddress;
                dtrowHeader["consig_name"] = _DomesticSaleInvoice_Model.ConsigneeName;
                dtrowHeader["cust_ref"] = _DomesticSaleInvoice_Model.Custome_Reference;
                dtrowHeader["pay_term"] = _DomesticSaleInvoice_Model.Payment_term;
                dtrowHeader["deli_term"] = _DomesticSaleInvoice_Model.Delivery_term;
                dtrowHeader["rev_charge"] = _DomesticSaleInvoice_Model.rev_charge;
                dtrowHeader["tcs_amt"] = IsNull(_DomesticSaleInvoice_Model.tcs_amt,"0");
                dtrowHeader["ship_from_address"] = _DomesticSaleInvoice_Model.ShipFromAddress;
               
                if (DocumentMenuId == "105103145125")
                {
                    dtrowHeader["custom_inv_dt"] = _DomesticSaleInvoice_Model.CustomInvDate;
                }
                else
                {
                    dtrowHeader["custom_inv_dt"] = _DomesticSaleInvoice_Model.inv_dt;
                }
                if(_DomesticSaleInvoice_Model.OrderType == "D" && cancelflag == "False")
                {
                    dtrowHeader["disp_Addr1"] = _DomesticSaleInvoice_Model.Address1;
                    dtrowHeader["disp_Addr2"] = _DomesticSaleInvoice_Model.Address2;
                    dtrowHeader["disp_country"] = Convert.ToInt32(_DomesticSaleInvoice_Model.Country);
                    dtrowHeader["disp_state"] = Convert.ToInt32(_DomesticSaleInvoice_Model.State);
                    dtrowHeader["disp_district"] = Convert.ToInt32(_DomesticSaleInvoice_Model.District);
                    dtrowHeader["disp_city"] = Convert.ToInt32(_DomesticSaleInvoice_Model.City);
                    dtrowHeader["disp_pin"] = _DomesticSaleInvoice_Model.Pin;
                }
                else
                {
                    dtrowHeader["disp_Addr1"] = "";
                    dtrowHeader["disp_Addr2"] = "";
                    dtrowHeader["disp_country"] = 0;
                    dtrowHeader["disp_state"] = 0;
                    dtrowHeader["disp_district"] = 0;
                    dtrowHeader["disp_city"] = 0;
                    dtrowHeader["disp_pin"] = "";
                }
                dtrowHeader["cancel_remarks"] = _DomesticSaleInvoice_Model.CancelledRemarks;
                dtrowHeader["pvt_mark"] = _DomesticSaleInvoice_Model.PvtMark;
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
        private string IsNull(string Str, string Str2)
        {
            if (!string.IsNullOrEmpty(Str))
            {
            }
            else
                Str = Str2;
            return Str;
        }
        private DataTable ToDtblItemDetail(string SIItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ship_no", typeof(string));
                dtItem.Columns.Add("ship_date", typeof(string));
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("ship_qty", typeof(double));
                dtItem.Columns.Add("mrp", typeof(string));
                dtItem.Columns.Add("mrp_disc", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_inv_rate", typeof(string));
                dtItem.Columns.Add("item_disc_perc", typeof(string));
                dtItem.Columns.Add("item_disc_amt", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(double));
                dtItem.Columns.Add("item_tax_amt", typeof(double));
                dtItem.Columns.Add("item_oc_amt", typeof(double));
                dtItem.Columns.Add("item_net_val_spec", typeof(double));
                dtItem.Columns.Add("item_net_val_bs", typeof(double));
                dtItem.Columns.Add("gl_vou_no", typeof(string));
                dtItem.Columns.Add("gl_vou_dt", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("item_disc_val", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));//Added by Suraj on 12-08-2024 
                dtItem.Columns.Add("it_remarks", typeof(string));
                dtItem.Columns.Add("foc", typeof(string));
                dtItem.Columns.Add("PackSize", typeof(string));
                dtItem.Columns.Add("ship_foc_qty", typeof(string));
                JArray jObject = JArray.Parse(SIItemDetail);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["ship_no"] = jObject[i]["ship_no"].ToString();
                    dtrowLines["ship_date"] = jObject[i]["ship_date"].ToString();
                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["uom_id"].ToString();
                    dtrowLines["ship_qty"] = jObject[i]["ship_qty"].ToString();
                    if (DocumentMenuId == "105103145125")
                    {
                        dtrowLines["mrp"] = "0";
                        dtrowLines["mrp_disc"] = "0";
                    }
                    else
                    {
                        dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                        dtrowLines["mrp_disc"] = jObject[i]["mrp_disc"].ToString();
                    }

                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    dtrowLines["item_inv_rate"] = jObject[i]["item_inv_rate"].ToString();
                    dtrowLines["item_disc_perc"] = jObject[i]["item_disc_perc"].ToString();
                    dtrowLines["item_disc_amt"] = jObject[i]["item_disc_amt"].ToString();
                    dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();

                    if (jObject[i]["item_tax_amt"].ToString() == "")
                    {
                        dtrowLines["item_tax_amt"] = "0";
                    }
                    else
                    {
                        dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                    }
                    if (jObject[i]["item_oc_amt"].ToString() == "")
                    {
                        dtrowLines["item_oc_amt"] = "0";
                    }
                    else
                    {
                        dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                    }
                    dtrowLines["item_net_val_spec"] = jObject[i]["item_net_val_spec"].ToString();
                    dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                    dtrowLines["gl_vou_no"] = jObject[i]["gl_vou_no"].ToString();
                    dtrowLines["gl_vou_dt"] = jObject[i]["gl_vou_dt"].ToString();
                    dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                    dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtrowLines["item_disc_val"] = jObject[i]["item_disc_val"].ToString();
                    dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();//Added by Suraj on 12-08-2024 
                    dtrowLines["it_remarks"] = jObject[i]["it_remarks"].ToString();
                    dtrowLines["foc"] = jObject[i]["foc"].ToString();
                    dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();
                    dtrowLines["ship_foc_qty"] = jObject[i]["ship_foc_qty"].ToString();
                    dtItem.Rows.Add(dtrowLines); 
                }
                DtblItemDetail = dtItem;
                ViewData["ItemDetails"] = dtitemdetail(jObject);
                return DtblItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private DataTable ToDtblTaxDetail(string TaxDetails)
        {
            try
            {

                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("ship_no", typeof(string));
                dtItem.Columns.Add("ship_date", typeof(string));
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("tax_id", typeof(int));
                dtItem.Columns.Add("tax_rate", typeof(double));
                dtItem.Columns.Add("tax_val", typeof(double));
                dtItem.Columns.Add("tax_level", typeof(int));
                dtItem.Columns.Add("tax_apply_on", typeof(string));
                if (TaxDetails != null)

                {
                    JArray jObject = JArray.Parse(TaxDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["ship_no"] = jObject[i]["ship_no"].ToString();
                        dtrowLines["ship_date"] = jObject[i]["ship_date"].ToString();
                        dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                        dtrowLines["tax_id"] = jObject[i]["tax_id"].ToString();
                        dtrowLines["tax_rate"] = jObject[i]["tax_rate"].ToString();
                        dtrowLines["tax_val"] = jObject[i]["tax_val"].ToString();
                        dtrowLines["tax_level"] = jObject[i]["tax_level"].ToString();
                        dtrowLines["tax_apply_on"] = jObject[i]["tax_apply_on"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["TaxDetails"] = dtTaxdetail(jObject);
                }
                DtblItemTaxDetail = dtItem;
                return DtblItemTaxDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblOCDetail(string OCDetails)
        {
            try
            {

                DataTable DtblItemOCDetail = new DataTable();
                DataTable dtItem = new DataTable();

                //dtItem.Columns.Add("oc_id", typeof(int));
                //dtItem.Columns.Add("oc_val", typeof(double));
                //dtItem.Columns.Add("tax_amt", typeof(string));
                //dtItem.Columns.Add("total_amt", typeof(string));
                dtItem.Columns.Add("oc_id", typeof(string));
                dtItem.Columns.Add("oc_val", typeof(string));
                dtItem.Columns.Add("tax_amt", typeof(string));
                dtItem.Columns.Add("total_amt", typeof(string));
                dtItem.Columns.Add("curr_id", typeof(string));
                dtItem.Columns.Add("conv_rate", typeof(string));
                dtItem.Columns.Add("supp_id", typeof(string));
                dtItem.Columns.Add("supp_type", typeof(string));
                dtItem.Columns.Add("bill_no", typeof(string));
                dtItem.Columns.Add("bill_date", typeof(string));
                dtItem.Columns.Add("tds_amt", typeof(string)); /*Added by Hina on 10-07-2024 to chnge for tds on oc*/
                dtItem.Columns.Add("roundoff", typeof(string));//Added by Suraj Maurya on 11-12-2024
                dtItem.Columns.Add("pm_flag", typeof(string));//Added by Suraj Maurya on 11-12-2024
                if (OCDetails != null)
                {

                    JArray jObject = JArray.Parse(OCDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["oc_id"] = jObject[i]["oc_id"].ToString();
                        dtrowLines["oc_val"] = jObject[i]["oc_val"].ToString();
                        dtrowLines["tax_amt"] = jObject[i]["tax_amt"].ToString();
                        dtrowLines["total_amt"] = jObject[i]["total_amt"].ToString();
                        dtrowLines["curr_id"] = jObject[i]["curr_id"].ToString();
                        dtrowLines["conv_rate"] = jObject[i]["OC_Conv"].ToString();
                        dtrowLines["supp_id"] = jObject[i]["supp_id"].ToString();
                        dtrowLines["supp_type"] = jObject[i]["supp_type"].ToString();
                        dtrowLines["bill_no"] = jObject[i]["bill_no"].ToString();
                        dtrowLines["bill_date"] = jObject[i]["bill_date"].ToString();
                        dtrowLines["tds_amt"] = jObject[i]["tds_amt"].ToString(); /*Added by Hina on 10-07-2024 to chnge for tds on oc*/
                        dtrowLines["roundoff"] = jObject[i]["round_off"].ToString();
                        dtrowLines["pm_flag"] = jObject[i]["pm_flag"].ToString();

                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["OCDetails"] = dtOCdetail(jObject);
                }
                DtblItemOCDetail = dtItem;
                return DtblItemOCDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblVouGlDetail(string VouGlDetails)
        {
            try
            {
                DataTable DtblVouGlDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("comp_id", typeof(string));
                dtItem.Columns.Add("vou_sr_no", typeof(string));
                dtItem.Columns.Add("gl_sr_no", typeof(string));
                dtItem.Columns.Add("id", typeof(string));
                dtItem.Columns.Add("type", typeof(string));
                dtItem.Columns.Add("doctype", typeof(string));
                dtItem.Columns.Add("Value", typeof(string));
                dtItem.Columns.Add("ValueInBase", typeof(string));
                dtItem.Columns.Add("DrAmt", typeof(string));
                dtItem.Columns.Add("CrAmt", typeof(string));
                dtItem.Columns.Add("TransType", typeof(string));
                dtItem.Columns.Add("Gltype", typeof(string));
                dtItem.Columns.Add("parent", typeof(string));
                dtItem.Columns.Add("DrAmtInBase", typeof(string));
                dtItem.Columns.Add("CrAmtInBase", typeof(string));
                dtItem.Columns.Add("curr_id", typeof(string));
                dtItem.Columns.Add("conv_rate", typeof(string));
                dtItem.Columns.Add("vou_type", typeof(string));
                dtItem.Columns.Add("bill_no", typeof(string));
                dtItem.Columns.Add("bill_date", typeof(string));
                dtItem.Columns.Add("gl_narr", typeof(string));

                JArray jObject = JArray.Parse(VouGlDetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["comp_id"] = jObject[i]["comp_id"].ToString();
                    dtrowLines["vou_sr_no"] = jObject[i]["VouSrNo"].ToString();
                    dtrowLines["gl_sr_no"] = jObject[i]["GlSrNo"].ToString();
                    dtrowLines["id"] = jObject[i]["id"].ToString();
                    dtrowLines["type"] = jObject[i]["type"].ToString();
                    dtrowLines["doctype"] = jObject[i]["doctype"].ToString();
                    dtrowLines["Value"] = jObject[i]["Value"].ToString();
                    dtrowLines["ValueInBase"] = jObject[i]["ValueInBase"].ToString();
                    dtrowLines["DrAmt"] = jObject[i]["DrAmt"].ToString();
                    dtrowLines["CrAmt"] = jObject[i]["CrAmt"].ToString();
                    dtrowLines["TransType"] = jObject[i]["TransType"].ToString();
                    dtrowLines["Gltype"] = jObject[i]["Gltype"].ToString();
                    dtrowLines["parent"] = "0";// jObject[i]["Gltype"].ToString();
                    dtrowLines["DrAmtInBase"] = jObject[i]["DrAmtInBase"].ToString();
                    dtrowLines["CrAmtInBase"] = jObject[i]["CrAmtInBase"].ToString();
                    dtrowLines["curr_id"] = jObject[i]["curr_id"].ToString();
                    dtrowLines["conv_rate"] = jObject[i]["conv_rate"].ToString();
                    dtrowLines["vou_type"] = jObject[i]["vou_type"].ToString();
                    dtrowLines["bill_no"] = jObject[i]["bill_no"].ToString();
                    dtrowLines["bill_date"] = jObject[i]["bill_date"].ToString();
                    dtrowLines["gl_narr"] = jObject[i]["gl_narr"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblVouGlDetail = dtItem;
                ViewData["VouDetail"] = dtVoudetail(jObject);
                return DtblVouGlDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblOCTdsDetail(string octdsDetails)
        {
            try
            {
                DataTable DtblOCtdsDetail = new DataTable();
                DataTable OCtds_detail = new DataTable();
                OCtds_detail.Columns.Add("tds_id", typeof(string));
                OCtds_detail.Columns.Add("tds_rate", typeof(string));
                OCtds_detail.Columns.Add("tds_val", typeof(string));
                OCtds_detail.Columns.Add("tds_level", typeof(string));
                OCtds_detail.Columns.Add("tds_apply_on", typeof(string));
                OCtds_detail.Columns.Add("oc_id", typeof(string));
                OCtds_detail.Columns.Add("supp_id", typeof(string));
                OCtds_detail.Columns.Add("tds_base_amt", typeof(string));//Added by Suraj Maurya on 11-12-2024
                OCtds_detail.Columns.Add("tds_ass_apply_on", typeof(string));//Added by Suraj Maurya on 11-12-2024


                if (octdsDetails != null)
                {
                    JArray jObjecttdsoc = JArray.Parse(octdsDetails);
                    for (int i = 0; i < jObjecttdsoc.Count; i++)
                    {
                        DataRow dtrowtdsDetailsLines = OCtds_detail.NewRow();
                        dtrowtdsDetailsLines["tds_id"] = jObjecttdsoc[i]["Tds_id"].ToString();
                        string tds_rate = jObjecttdsoc[i]["Tds_rate"].ToString();
                        tds_rate = tds_rate.Replace("%", "");
                        dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                        dtrowtdsDetailsLines["tds_level"] = jObjecttdsoc[i]["Tds_level"].ToString();
                        dtrowtdsDetailsLines["tds_val"] = jObjecttdsoc[i]["Tds_val"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on"] = jObjecttdsoc[i]["Tds_apply_on"].ToString();
                        dtrowtdsDetailsLines["oc_id"] = jObjecttdsoc[i]["Tds_oc_id"].ToString();
                        dtrowtdsDetailsLines["supp_id"] = jObjecttdsoc[i]["Tds_supp_id"].ToString();
                        dtrowtdsDetailsLines["tds_base_amt"] = jObjecttdsoc[i]["Tds_totalAmnt"].ToString();
                        dtrowtdsDetailsLines["tds_ass_apply_on"] = jObjecttdsoc[i]["Tds_AssValApplyOn"].ToString();
                        OCtds_detail.Rows.Add(dtrowtdsDetailsLines);
                    }
                    ViewData["OCTDSDetails"] = dtOCTDSdetail(jObjecttdsoc);
                }
                DtblOCtdsDetail = OCtds_detail;
                return DtblOCtdsDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        private DataTable ToDtblTcsDetail(string tcsDetails)
        {
            try
            {
                DataTable DtblTcsDetail = new DataTable();
                DataTable Tcs_detail = new DataTable();
                Tcs_detail.Columns.Add("tcs_id", typeof(string));
                Tcs_detail.Columns.Add("tcs_rate", typeof(string));
                Tcs_detail.Columns.Add("tcs_val", typeof(string));
                Tcs_detail.Columns.Add("tcs_level", typeof(string));
                Tcs_detail.Columns.Add("tcs_apply_on", typeof(string));
                Tcs_detail.Columns.Add("tcs_ass_val", typeof(string));
                Tcs_detail.Columns.Add("tcs_base_amt", typeof(string));
                Tcs_detail.Columns.Add("tcs_ass_apply_on", typeof(string));


                if (tcsDetails != null)
                {
                    JArray jObjecttcs = JArray.Parse(tcsDetails);
                    for (int i = 0; i < jObjecttcs.Count; i++)
                    {
                        DataRow dtrowtcsDetailsLines = Tcs_detail.NewRow();
                        dtrowtcsDetailsLines["tcs_id"] = jObjecttcs[i]["Tcs_id"].ToString();
                        string tcs_rate = jObjecttcs[i]["Tcs_rate"].ToString();
                        tcs_rate = tcs_rate.Replace("%", "");
                        dtrowtcsDetailsLines["tcs_rate"] = tcs_rate;
                        dtrowtcsDetailsLines["tcs_level"] = jObjecttcs[i]["Tcs_level"].ToString();
                        dtrowtcsDetailsLines["tcs_val"] = jObjecttcs[i]["Tcs_val"].ToString();
                        dtrowtcsDetailsLines["tcs_apply_on"] = jObjecttcs[i]["Tcs_apply_on"].ToString();
                        dtrowtcsDetailsLines["tcs_ass_val"] = jObjecttcs[i]["Tcs_totalAmnt"].ToString();
                        dtrowtcsDetailsLines["tcs_base_amt"] = jObjecttcs[i]["Tcs_totalAmnt"].ToString();
                        dtrowtcsDetailsLines["tcs_ass_apply_on"] = jObjecttcs[i]["Tcs_AssValApplyOn"].ToString();
                        Tcs_detail.Rows.Add(dtrowtcsDetailsLines);
                    }
                    //ViewData["TcsDetails"] = dtTcsdetail(jObjecttcs);
                }
                DtblTcsDetail = Tcs_detail;
                return DtblTcsDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        //private DataTable ToDtblVouGlDetail(string VouGlDetails)
        //{
        //    try
        //    {
        //        DataTable DtblVouGlDetail = new DataTable();
        //        DataTable dtItem = new DataTable();

        //        dtItem.Columns.Add("comp_id", typeof(int));
        //        dtItem.Columns.Add("id", typeof(double));
        //        dtItem.Columns.Add("type", typeof(int));
        //        dtItem.Columns.Add("doctype", typeof(string));
        //        dtItem.Columns.Add("Value", typeof(int));
        //        dtItem.Columns.Add("DrAmt", typeof(string));
        //        dtItem.Columns.Add("CrAmt", typeof(string));
        //        dtItem.Columns.Add("TransType", typeof(string));
        //        dtItem.Columns.Add("Gltype", typeof(string));

        //        JArray jObject = JArray.Parse(VouGlDetails);

        //        for (int i = 0; i < jObject.Count; i++)
        //        {
        //            DataRow dtrowLines = dtItem.NewRow();

        //            dtrowLines["comp_id"] = jObject[i]["comp_id"].ToString();
        //            dtrowLines["id"] = jObject[i]["id"].ToString();
        //            dtrowLines["type"] = 'I';
        //            dtrowLines["doctype"] = jObject[i]["doctype"].ToString();
        //            dtrowLines["Value"] = jObject[i]["Value"].ToString();
        //            dtrowLines["DrAmt"] = jObject[i]["DrAmt"].ToString();
        //            dtrowLines["CrAmt"] = jObject[i]["CrAmt"].ToString();
        //            dtrowLines["TransType"] = jObject[i]["TransType"].ToString();
        //            dtrowLines["Gltype"] = jObject[i]["Gltype"].ToString();

        //            dtItem.Rows.Add(dtrowLines);
        //        }
        //        DtblVouGlDetail = dtItem;
        //        ViewData["VouDetail"] = dtVoudetail(jObject);
        //        return DtblVouGlDetail;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        throw ex;
        //    }

        //}
        public DataTable dtitemdetail(JArray jObject)
        {

            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("sh_no", typeof(string));
            dtItem.Columns.Add("sh_date", typeof(string));
            dtItem.Columns.Add("sh_dt", typeof(string));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_alias", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("ship_qty", typeof(double));
            dtItem.Columns.Add("mrp", typeof(string));
            dtItem.Columns.Add("mrp_disc", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(double));
            dtItem.Columns.Add("item_disc_perc", typeof(string));
            dtItem.Columns.Add("item_disc_amt", typeof(string));
            dtItem.Columns.Add("item_gr_val", typeof(double));
            dtItem.Columns.Add("item_tax_amt", typeof(double));
            dtItem.Columns.Add("item_oc_amt", typeof(double));
            dtItem.Columns.Add("item_net_val_spec", typeof(double));
            dtItem.Columns.Add("item_net_val_bs", typeof(double));
            dtItem.Columns.Add("gl_vou_no", typeof(string));
            dtItem.Columns.Add("gl_vou_dt", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string)); 
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("PackSize", typeof(string));



            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["sh_no"] = jObject[i]["ship_no"].ToString();
                dtrowLines["sh_date"] = jObject[i]["ship_date"].ToString();
                dtrowLines["sh_dt"] = jObject[i]["ship_date"].ToString();
                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();
                dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"].ToString());
                dtrowLines["uom_alias"] = jObject[i]["uom_name"].ToString();
                dtrowLines["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowLines["ship_qty"] = jObject[i]["ship_qty"].ToString();
                if (DocumentMenuId == "105103145125")
                {
                    dtrowLines["mrp"] = "0";
                    dtrowLines["mrp_disc"] = "0";
                }
                else
                {
                    dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                    dtrowLines["mrp_disc"] = jObject[i]["mrp_disc"].ToString();
                }

                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                dtrowLines["item_disc_perc"] = jObject[i]["item_disc_perc"].ToString();
                dtrowLines["item_disc_amt"] = jObject[i]["item_disc_amt"].ToString();
                dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();

                if (jObject[i]["item_tax_amt"].ToString() == "")
                {
                    dtrowLines["item_tax_amt"] = "0";
                }
                else
                {
                    dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                }
                if (jObject[i]["item_oc_amt"].ToString() == "")
                {
                    dtrowLines["item_oc_amt"] = "0";
                }
                else
                {
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                }
                dtrowLines["item_net_val_spec"] = jObject[i]["item_net_val_spec"].ToString();
                dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                dtrowLines["gl_vou_no"] = jObject[i]["gl_vou_no"].ToString();
                dtrowLines["gl_vou_dt"] = jObject[i]["gl_vou_dt"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["it_remarks"].ToString();
                dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtTaxdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("sh_no", typeof(string));
            dtItem.Columns.Add("sh_date", typeof(string));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("tax_id", typeof(int));
            dtItem.Columns.Add("tax_name", typeof(string));
            dtItem.Columns.Add("tax_rate", typeof(string));
            dtItem.Columns.Add("tax_val", typeof(string));
            dtItem.Columns.Add("tax_level", typeof(int));
            dtItem.Columns.Add("tax_apply_on", typeof(string));
            dtItem.Columns.Add("tax_apply_Name", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["sh_no"] = jObject[i]["ship_no"].ToString();
                dtrowLines["sh_date"] = jObject[i]["ship_date"].ToString();
                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["tax_id"] = jObject[i]["tax_id"].ToString();
                dtrowLines["tax_name"] = jObject[i]["tax_name"].ToString();
                string tax_rate = jObject[i]["tax_rate"].ToString();
                tax_rate = tax_rate.Replace("%", "");
                dtrowLines["tax_rate"] = tax_rate;
                dtrowLines["tax_val"] = jObject[i]["tax_val"].ToString();
                dtrowLines["tax_level"] = jObject[i]["tax_level"].ToString();
                dtrowLines["tax_apply_on"] = jObject[i]["tax_apply_on"].ToString();
                dtrowLines["tax_apply_Name"] = jObject[i]["tax_apply_name"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtOCdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("oc_id", typeof(int));
            dtItem.Columns.Add("oc_name", typeof(string));
            dtItem.Columns.Add("curr_name", typeof(string));
            dtItem.Columns.Add("conv_rate", typeof(string));
            dtItem.Columns.Add("oc_val", typeof(string));
            dtItem.Columns.Add("OCValBs", typeof(string));
            dtItem.Columns.Add("tax_amt", typeof(string));
            dtItem.Columns.Add("total_amt", typeof(string));
            dtItem.Columns.Add("curr_id", typeof(string));
            dtItem.Columns.Add("supp_id", typeof(string));
            dtItem.Columns.Add("supp_type", typeof(string));
            dtItem.Columns.Add("bill_no", typeof(string));
            dtItem.Columns.Add("bill_date", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["oc_id"] = jObject[i]["oc_id"].ToString();
                dtrowLines["oc_name"] = jObject[i]["OCName"].ToString();
                dtrowLines["curr_name"] = jObject[i]["OC_Curr"].ToString();
                dtrowLines["conv_rate"] = jObject[i]["OC_Conv"].ToString();
                dtrowLines["oc_val"] = jObject[i]["oc_val"].ToString();
                dtrowLines["OCValBs"] = jObject[i]["OC_AmtBs"].ToString();
                dtrowLines["tax_amt"] = jObject[i]["tax_amt"].ToString();
                dtrowLines["total_amt"] = jObject[i]["total_amt"].ToString();
                dtrowLines["curr_id"] = jObject[i]["curr_id"].ToString();
                dtrowLines["supp_id"] = jObject[i]["supp_id"].ToString();
                dtrowLines["supp_type"] = jObject[i]["supp_type"].ToString();
                dtrowLines["bill_no"] = jObject[i]["bill_no"].ToString();
                dtrowLines["bill_date"] = jObject[i]["bill_date"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtCCdetail(JArray JAObj)
        {
            DataTable CC_Details = new DataTable();

            CC_Details.Columns.Add("vou_sr_no", typeof(string));
            CC_Details.Columns.Add("gl_sr_no", typeof(string));
            CC_Details.Columns.Add("acc_id", typeof(string));
            CC_Details.Columns.Add("cc_id", typeof(int));
            CC_Details.Columns.Add("cc_val_id", typeof(int));
            CC_Details.Columns.Add("cc_amt", typeof(string));
            CC_Details.Columns.Add("cc_name", typeof(string));
            CC_Details.Columns.Add("cc_val_name", typeof(string));
            for (int i = 0; i < JAObj.Count; i++)
            {
                DataRow dtrowLines = CC_Details.NewRow();

                dtrowLines["vou_sr_no"] = JAObj[i]["vou_sr_no"].ToString();
                dtrowLines["gl_sr_no"] = JAObj[i]["gl_sr_no"].ToString();
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
        public DataTable dtVoudetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("comp_id", typeof(string));
            dtItem.Columns.Add("vou_sr_no", typeof(string));
            dtItem.Columns.Add("gl_sr_no", typeof(string));
            dtItem.Columns.Add("id", typeof(string));
            dtItem.Columns.Add("type", typeof(string));
            dtItem.Columns.Add("doctype", typeof(string));
            dtItem.Columns.Add("Value", typeof(string));
            dtItem.Columns.Add("ValueInBase", typeof(string));
            dtItem.Columns.Add("DrAmt", typeof(string));
            dtItem.Columns.Add("CrAmt", typeof(string));
            dtItem.Columns.Add("TransType", typeof(string));
            dtItem.Columns.Add("Gltype", typeof(string));
            dtItem.Columns.Add("parent", typeof(string));
            dtItem.Columns.Add("DrAmtInBase", typeof(string));
            dtItem.Columns.Add("CrAmtInBase", typeof(string));
            dtItem.Columns.Add("curr_id", typeof(string));
            dtItem.Columns.Add("conv_rate", typeof(string));
            dtItem.Columns.Add("vou_type", typeof(string));
            dtItem.Columns.Add("bill_no", typeof(string));
            dtItem.Columns.Add("bill_date", typeof(string));
            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["comp_id"] = jObject[i]["comp_id"].ToString();
                dtrowLines["vou_sr_no"] = jObject[i]["VouSrNo"].ToString();
                dtrowLines["gl_sr_no"] = jObject[i]["GlSrNo"].ToString();
                dtrowLines["id"] = jObject[i]["id"].ToString();
                dtrowLines["type"] = jObject[i]["type"].ToString();
                dtrowLines["doctype"] = jObject[i]["doctype"].ToString();
                dtrowLines["Value"] = jObject[i]["Value"].ToString();
                dtrowLines["ValueInBase"] = jObject[i]["ValueInBase"].ToString();
                dtrowLines["DrAmt"] = jObject[i]["DrAmt"].ToString();
                dtrowLines["CrAmt"] = jObject[i]["CrAmt"].ToString();
                dtrowLines["TransType"] = jObject[i]["TransType"].ToString();
                dtrowLines["Gltype"] = jObject[i]["Gltype"].ToString();
                dtrowLines["parent"] = "0";// jObject[i]["Gltype"].ToString();
                dtrowLines["DrAmtInBase"] = jObject[i]["DrAmtInBase"].ToString();
                dtrowLines["CrAmtInBase"] = jObject[i]["CrAmtInBase"].ToString();
                dtrowLines["curr_id"] = jObject[i]["curr_id"].ToString();
                dtrowLines["conv_rate"] = jObject[i]["conv_rate"].ToString();
                dtrowLines["vou_type"] = jObject[i]["vou_type"].ToString();
                dtrowLines["bill_no"] = jObject[i]["bill_no"].ToString();
                dtrowLines["bill_date"] = jObject[i]["bill_date"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }
            return dtItem;
        }
        public DataTable dtOCTDSdetail(JArray jObjecttdsoc)
        {
            DataTable DtblItemtdsDetail = new DataTable();
            DataTable OCtds_detail = new DataTable();
            OCtds_detail.Columns.Add("tds_id", typeof(string));
            OCtds_detail.Columns.Add("tds_rate", typeof(string));
            OCtds_detail.Columns.Add("tds_val", typeof(string));
            OCtds_detail.Columns.Add("tds_level", typeof(string));
            OCtds_detail.Columns.Add("tds_apply_on", typeof(string));
            OCtds_detail.Columns.Add("tds_name", typeof(string));
            OCtds_detail.Columns.Add("tds_apply_Name", typeof(string));
            OCtds_detail.Columns.Add("oc_id", typeof(string));
            OCtds_detail.Columns.Add("supp_id", typeof(string));
            for (int i = 0; i < jObjecttdsoc.Count; i++)
            {
                DataRow dtrowtdsDetailsLines = OCtds_detail.NewRow();
                dtrowtdsDetailsLines["tds_id"] = jObjecttdsoc[i]["Tds_id"].ToString();
                string tds_rate = jObjecttdsoc[i]["Tds_rate"].ToString();
                tds_rate = tds_rate.Replace("%", "");
                dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                dtrowtdsDetailsLines["tds_level"] = jObjecttdsoc[i]["Tds_level"].ToString();
                dtrowtdsDetailsLines["tds_val"] = jObjecttdsoc[i]["Tds_val"].ToString();
                dtrowtdsDetailsLines["tds_apply_on"] = jObjecttdsoc[i]["Tds_apply_on"].ToString();
                dtrowtdsDetailsLines["tds_name"] = jObjecttdsoc[i]["Tds_name"].ToString();
                dtrowtdsDetailsLines["tds_apply_Name"] = jObjecttdsoc[i]["Tds_applyOnName"].ToString();
                dtrowtdsDetailsLines["oc_id"] = jObjecttdsoc[i]["Tds_oc_id"].ToString();
                dtrowtdsDetailsLines["supp_id"] = jObjecttdsoc[i]["Tds_supp_id"].ToString();
                OCtds_detail.Rows.Add(dtrowtdsDetailsLines);
            }

            return OCtds_detail;
        }
        public DataTable dtSubitemdetail(JArray jObject2)
        {

            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("ship_qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["ship_qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }

            return dtSubItem;
        }

        //private DataTable GetRoleList()
        //{
        //    try
        //    {

        //        string UserID = string.Empty;
        //        string CompID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            UserID = Session["userid"].ToString();
        //        }
        //        //if (Session["MenuDocumentId"] != null)
        //        //{
        //        //    if (Session["MenuDocumentId"].ToString() == "105103140")
        //        //    {
        //        //        DocumentMenuId = "105103140";
        //        //    }
        //        //    if (Session["MenuDocumentId"].ToString() == "105103145125")
        //        //    {
        //        //        DocumentMenuId = "105103145125";
        //        //    }
        //        //}
        //        DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

        //        return RoleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
        public ActionResult GetSaleInvoiceList(string docid, string status)
        {
            //Session["MenuDocumentId"] = docid;
            //Session["WF_status"] = status;
            SI_ListModel _SI_ListModel = new SI_ListModel();
            _SI_ListModel.DocumentMenuId = docid;
            _SI_ListModel.WF_status = status;
            if (_SI_ListModel.DocumentMenuId != null)
            {
                if (_SI_ListModel.DocumentMenuId == "105103140")
                {
                    return RedirectToAction("LocalSaleInvoiceList", _SI_ListModel);
                }
                if (_SI_ListModel.DocumentMenuId == "105103145125")
                {
                    return RedirectToAction("CommercialInvoiceList", _SI_ListModel);
                }
            }
            return RedirectToAction("LocalSaleInvoiceList");
        }
        private List<SalesInvoiceList> GetSI_DetailList(SI_ListModel _SI_ListModel)
        {
            try
            {
                List<SalesInvoiceList> _SalesInvoiceList = new List<SalesInvoiceList>();
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string Order_type = string.Empty;
                DataSet dt = new DataSet();
                string wfstatus = "";
                //if (Session["WF_status"] != null)
                //{
                //    wfstatus = Session["WF_status"].ToString();
                //}
                //else
                //{
                //    wfstatus = "";
                //}
                if (_SI_ListModel.WF_status != null)
                {
                    wfstatus = _SI_ListModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103140")
                //    {
                //        DocumentMenuId = "105103140";
                //        Order_type = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145125")
                //    {
                //        DocumentMenuId = "105103145125";
                //        Order_type = "E";
                //    }
                //}
                Order_type = _SI_ListModel.CustType;
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                dt = _DomesticSaleInvoice_ISERVICE.GetSI_DetailList(Comp_ID, Br_ID, _SI_ListModel.CustID, _SI_ListModel.SI_FromDate, _SI_ListModel.SI_ToDate, _SI_ListModel.Status, UserID, DocumentMenuId, wfstatus, Order_type, _SI_ListModel.SQ_SalePerson);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        SalesInvoiceList _SIList = new SalesInvoiceList();
                        _SIList.InvoiceNo = dr["inv_no"].ToString();
                        _SIList.InvoiceDate = dr["InvDate"].ToString();
                        _SIList.SalesPerson = dr["SalesPerson"].ToString();
                        _SIList.InvDate = dr["InvDt"].ToString();
                        _SIList.InvoiceType = dr["InvType"].ToString();
                        _SIList.ship_no = dr["sh_no"].ToString();
                        _SIList.ship_dt = dr["sh_date"].ToString();
                        _SIList.CustName = dr["cust_name"].ToString();
                        _SIList.Currency = dr["curr"].ToString();
                        _SIList.InvoiceValue = dr["net_val"].ToString();
                        _SIList.Stauts = dr["Status"].ToString();
                        _SIList.CreateDate = dr["CreateDate"].ToString();
                        _SIList.ApproveDate = dr["ApproveDate"].ToString();
                        _SIList.ModifyDate = dr["ModifyDate"].ToString();
                        _SIList.create_by = dr["create_by"].ToString();
                        _SIList.app_by = dr["app_by"].ToString();
                        _SIList.mod_by = dr["mod_by"].ToString();
                        _SIList.custom_inv_no = dr["custom_inv_no"].ToString();
                        _SIList.custom_inv_dt = dr["custom_inv_dt"].ToString();
                        _SalesInvoiceList.Add(_SIList);
                    }
                }
                return _SalesInvoiceList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult SearchSI_Detail(string CustId, string Fromdate, string Todate, string Status, string DocID,string sales_person)
        {
            try
            {
                SI_ListModel _SI_ListModel = new SI_ListModel();
                List<SalesInvoiceList> _SalesInvoiceList = new List<SalesInvoiceList>();
                string Order_type = string.Empty;
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                DataSet dt = new DataSet();
                //Session.Remove("WF_status");
                _SI_ListModel.WF_status = null;
                // SI_ListModel _SI_ListModel = new SI_ListModel();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103140")
                //    {
                //        DocumentMenuId = "105103140";
                //        Order_type = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145125")
                //    {
                //        DocumentMenuId = "105103145125";
                //        Order_type = "E";
                //    }
                //}

                if (DocID != null)
                {
                    _SI_ListModel.DocumentMenuId = DocID;
                    if (_SI_ListModel.DocumentMenuId == "105103140")
                    {
                        DocumentMenuId = "105103140";
                        Order_type = "D";
                    }
                    else if (_SI_ListModel.DocumentMenuId == "105103145125")
                    {
                        DocumentMenuId = "105103145125";
                        Order_type = "E";
                    }
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                dt = _DomesticSaleInvoice_ISERVICE.GetSI_DetailList(Comp_ID, Br_ID, CustId, Fromdate, Todate, Status, UserID, _SI_ListModel.DocumentMenuId, "", Order_type, sales_person);
                _SI_ListModel.LSISearch = "LSI_Search";
                if (dt.Tables[0].Rows.Count > 0)
                {
                    // Session["LSISearch"] = "LSI_Search";

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        SalesInvoiceList _SIList = new SalesInvoiceList();
                        _SIList.InvoiceNo = dr["inv_no"].ToString();
                        _SIList.InvoiceDate = dr["InvDate"].ToString();
                        _SIList.SalesPerson = dr["SalesPerson"].ToString();
                        _SIList.InvDate = dr["InvDt"].ToString();
                        _SIList.InvoiceType = dr["InvType"].ToString();
                        _SIList.ship_no = dr["sh_no"].ToString();
                        _SIList.ship_dt = dr["sh_date"].ToString();
                        _SIList.CustName = dr["cust_name"].ToString();
                        _SIList.Currency = dr["curr"].ToString();
                        _SIList.InvoiceValue = dr["net_val"].ToString();
                        _SIList.Stauts = dr["Status"].ToString();
                        _SIList.CreateDate = dr["CreateDate"].ToString();
                        _SIList.ApproveDate = dr["ApproveDate"].ToString();
                        _SIList.ModifyDate = dr["ModifyDate"].ToString();
                        _SIList.create_by = dr["create_by"].ToString();
                        _SIList.app_by = dr["app_by"].ToString();
                        _SIList.mod_by = dr["mod_by"].ToString();
                        _SIList.custom_inv_no = dr["custom_inv_no"].ToString();
                        _SIList.custom_inv_dt = dr["custom_inv_dt"].ToString();

                        _SalesInvoiceList.Add(_SIList);
                    }
                }
                _SI_ListModel.SIList = _SalesInvoiceList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSalesInvoiceList.cshtml", _SI_ListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public void GetStatusList(SI_ListModel _SI_ListModel)
        {
            try
            {
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103140")
                //    {
                //        DocumentMenuId = "105103140";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145125")
                //    {
                //        DocumentMenuId = "105103145125";
                //    }
                //}
                DocumentMenuId = _SI_ListModel.DocumentMenuId;
                DataTable DTdata = ViewBag.StatusList;
                List<Status> statusLists1 = new List<Status>();
                if (DTdata.Rows.Count > 0)
                {
                    foreach (DataRow data in DTdata.Rows)
                    {
                        Status _Statuslist = new Status();
                        _Statuslist.status_id = data["status_code"].ToString();
                        _Statuslist.status_name = data["status_name"].ToString();
                        statusLists1.Add(_Statuslist);
                    }
                }
                _SI_ListModel.StatusList = statusLists1;
                //List<Status> statusLists = new List<Status>();
                //var other = new CommonController(_Common_IServices);
                //var statusListsC = other.GetStatusList1(DocumentMenuId);
                //var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                //_SI_ListModel.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        public ActionResult GetAutoCompleteCustomerList(DomesticSaleInvoice_Model _DSIModel)
        {
            string CustomerName = string.Empty;
            string CustomerType = string.Empty;
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
                if (string.IsNullOrEmpty(_DSIModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _DSIModel.CustName;
                }

                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103140")
                //    {
                //        CustomerType = "D";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145125")
                //    {
                //        CustomerType = "E";
                //    }
                //}
                if (_DSIModel.DocumentMenuId == "105103140")
                {
                    CustomerType = "D";
                }
                else if (_DSIModel.DocumentMenuId == "105103145125")
                {
                    CustomerType = "E";
                }
                //if (string.IsNullOrEmpty(_DSIModel.CustType))
                //{
                //    CustomerType = "";
                //}
                //else
                //{
                //    CustomerType = _DSIModel.CustType;
                //}
                CustList = _DomesticSaleInvoice_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustomerType, _DSIModel.DocumentMenuId);

                List<CustomerName> _SuppList = new List<CustomerName>();
                foreach (var data in CustList)
                {
                    CustomerName _SuppDetail = new CustomerName();
                    _SuppDetail.cust_id = data.Key;
                    _SuppDetail.cust_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                //_DSIModel.CustomerNameList = _SuppList1;
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
        public ActionResult GetCustList(SI_ListModel _SI_ListModel, DomesticSaleInvoice_Model _DomesticSaleInvoice_Model, string type,string DocumentMenuId)
        {
            string CustomerName = string.Empty;
            string CustType = string.Empty;
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
                if (type == "L")
                {
                    if (string.IsNullOrEmpty(_SI_ListModel.CustName))
                    {
                        CustomerName = "0";
                    }
                    else
                    {
                        CustomerName = _SI_ListModel.CustName;
                    }
                }
                if (type == "D")
                {
                    if (string.IsNullOrEmpty(_DomesticSaleInvoice_Model.CustName))
                    {
                        CustomerName = "0";
                    }
                    else
                    {
                        CustomerName = _DomesticSaleInvoice_Model.CustName;
                    }
                }
                if (_SI_ListModel.CustType != null)
                {
                    CustType = _SI_ListModel.CustType;
                }
                if (_DomesticSaleInvoice_Model.InvType != null)
                {
                    CustType = _DomesticSaleInvoice_Model.InvType;
                }


                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103140")
                //    {
                //        CustType = "D";

                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145125")
                //    {
                //        CustType = "E";
                //    }
                //}
                CustList = _DomesticSaleInvoice_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType, DocumentMenuId);
                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (var data in CustList)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.cust_id = data.Key;
                    _CustDetail.cust_name = data.Value;
                    _CustList.Add(_CustDetail);
                }
                if (type == "L")
                {
                    _SI_ListModel.CustomerNameList = _CustList;
                }
                if (type == "D")
                {
                    _DomesticSaleInvoice_Model.CustomerNameList = _CustList;
                }

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
        public ActionResult GetSalesPersonList(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model)
        {
            string SalesPersonName = string.Empty;
            Dictionary<string, string> SPersonList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string UserName = string.Empty;
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
                if (Session["UserName"] != null)
                {
                    UserName = Session["UserName"].ToString();
                }
                if (Session["crm"] != null)
                {
                    crm = Session["crm"].ToString();
                }
                if (Session["rpt_id"] != null)
                {
                    rpt_id = Session["rpt_id"].ToString();
                }
                if (Session["Userid"] != null)
                {
                    UserID = Session["Userid"].ToString();
                }
                if (string.IsNullOrEmpty(_DomesticSaleInvoice_Model.SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = _DomesticSaleInvoice_Model.SalePerson;
                }
                if(DocumentMenuId== "105103140")
                {
                    SPersonList = _DomesticSaleInvoice_ISERVICE.GetSalesPersonList(Comp_ID, SalesPersonName, Br_ID, UserID, _DomesticSaleInvoice_Model.SI_Number, _DomesticSaleInvoice_Model.SI_Date);
                }
                else
                {
                    SPersonList = _DomesticSaleInvoice_ISERVICE.GetSalesPersonList(Comp_ID, SalesPersonName, Br_ID, "1001", _DomesticSaleInvoice_Model.SI_Number, _DomesticSaleInvoice_Model.SI_Date);
                }
                List<SalesPersonName> _SlPrsnList = new List<SalesPersonName>();
                if ((rpt_id == "0" || _DomesticSaleInvoice_Model.TransType == "Update" || _DomesticSaleInvoice_Model.InvType == "E"|| UserID == "1001") && crm == "Y" )
                { 
                    foreach (var data in SPersonList)
                    {
                        SalesPersonName _SlPrsnDetail = new SalesPersonName();
                        _SlPrsnDetail.slprsn_id = data.Key;
                        _SlPrsnDetail.slprsn_name = data.Value;
                        _SlPrsnList.Add(_SlPrsnDetail);
                    }
                }
                else
                {
                    _SlPrsnList.Insert(0, new SalesPersonName() { slprsn_id = UserID, slprsn_name = UserName });
                }
                  

                _DomesticSaleInvoice_Model.SalesPersonNameList = _SlPrsnList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(SPersonList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetSIOCTypeList(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model)
        {
            try
            {
                List<OcCalciOtherCharge> _TaxCalciOCName = new List<OcCalciOtherCharge>();
                _DomesticSaleInvoice_Model = new DomesticSaleInvoice_Model();
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
                DataTable dt = _DomesticSaleInvoice_ISERVICE.GetOCList(Comp_ID, Br_ID);

                foreach (DataRow dr in dt.Rows)
                {
                    OcCalciOtherCharge _OCName = new OcCalciOtherCharge();
                    _OCName.oc_id = dr["oc_id"].ToString();
                    _OCName.oc_name = dr["oc_name"].ToString();
                    _TaxCalciOCName.Add(_OCName);

                }
                _DomesticSaleInvoice_Model.OcCalciOtherChargeList = _TaxCalciOCName;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_OtherChargeNew.cshtml", _DomesticSaleInvoice_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetRoundOffGLDetails()
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {

                string Comp_ID = string.Empty;
                string BranchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                DataSet GlDt = _DomesticSaleInvoice_ISERVICE.GetRoundOffGLDetails(Comp_ID, BranchID);
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
        private string getDocumentName()
        {
            try
            {
                string CompID = string.Empty;
                string language = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103140")
                //    {
                //        DocumentMenuId = "105103140";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145125")
                //    {
                //        DocumentMenuId = "105103145125";
                //    }
                //}
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
        public JsonResult GetShipmentLists(string Cust_id)
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
                DataSet result = _DomesticSaleInvoice_ISERVICE.GetShipmentList(Cust_id, Comp_ID, Br_ID);

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
        public JsonResult Getcurr_details(string ship_no, string ship_date, string flag)
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
                DataTable result = new DataTable();
                if (flag == "domestic")
                {
                    result = _DomesticSaleInvoice_ISERVICE.GetSalePerson_details(Comp_ID, Br_ID, ship_no, ship_date);
                }
                else
                {
                    result = _DomesticSaleInvoice_ISERVICE.Getcurr_details(Comp_ID, Br_ID, ship_no, ship_date);
                }
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
        public JsonResult GetShipmentDetails(string ShipmentNo, string ShipmentDate,string Inv_type)
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

                DataSet result = _DomesticSaleInvoice_ISERVICE.GetShipmentDetail(ShipmentNo, ShipmentDate, Comp_ID, Br_ID, "CommInv", Inv_type);

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
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
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
        public string CheckSaleReturnAgainstSaleInvoice(string DocNo, string DocDate)
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
                DataSet Deatils = _DomesticSaleInvoice_ISERVICE.CheckSaleReturnAgainstSaleInvoice(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
                if (Deatils.Tables[1].Rows.Count > 0)
                {
                    str = "PaymentCreated";
                }
                if (Deatils.Tables[2].Rows.Count > 0)
                {
                    str = "CustomInvoiceCreated";
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
        public ActionResult getTaxDetailItemWise(string ItemID, string SelectedItemdetail, string ItemName, string AssAmount)
        {
            try
            {
                DomesticSaleInvoice_Model _DomesticSaleInvoice_Model = new DomesticSaleInvoice_Model();
                DataTable DTableTaxDetail = new DataTable();
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    DataTable dttaxdtl = new DataTable();
                    dttaxdtl.Columns.Add("ship_no", typeof(string));
                    dttaxdtl.Columns.Add("ship_date", typeof(string));
                    dttaxdtl.Columns.Add("item_id", typeof(string));
                    dttaxdtl.Columns.Add("tax_name", typeof(string));
                    dttaxdtl.Columns.Add("tax_id", typeof(int));
                    dttaxdtl.Columns.Add("tax_rate", typeof(string));
                    dttaxdtl.Columns.Add("tax_val", typeof(double));
                    dttaxdtl.Columns.Add("tax_level", typeof(int));
                    dttaxdtl.Columns.Add("tax_apply_on", typeof(string));
                    //dttaxdtl.Columns.Add("item_tax_amt", typeof(string));
                    dttaxdtl.Columns.Add("tax_apply_Name", typeof(string));

                    JArray jObjectTax = JArray.Parse(SelectedItemdetail);

                    foreach (JObject item in jObjectTax.Children())
                    {
                        //if (item.GetValue("item_id").ToString() == ItemID.ToString())
                        //{
                        DataRow dtTaxdtlrow = dttaxdtl.NewRow();
                        dtTaxdtlrow["ship_no"] = item.GetValue("ship_no").ToString();
                        dtTaxdtlrow["ship_date"] = item.GetValue("ship_date").ToString();
                        dtTaxdtlrow["item_id"] = item.GetValue("item_id").ToString();
                        dtTaxdtlrow["tax_name"] = item.GetValue("tax_name").ToString();
                        dtTaxdtlrow["tax_id"] = item.GetValue("tax_id").ToString();
                        dtTaxdtlrow["tax_rate"] = item.GetValue("tax_rate").ToString() + "%";
                        dtTaxdtlrow["tax_apply_Name"] = item.GetValue("tax_apply_name").ToString();
                        dtTaxdtlrow["tax_val"] = item.GetValue("tax_val").ToString();
                        dtTaxdtlrow["tax_level"] = item.GetValue("tax_level").ToString();
                        dtTaxdtlrow["tax_apply_on"] = item.GetValue("tax_apply_on").ToString();
                        //dtTaxdtlrow["item_tax_amt"] = item.GetValue("item_tax_amt").ToString();



                        dttaxdtl.Rows.Add(dtTaxdtlrow);
                        //}
                    }
                    DTableTaxDetail = dttaxdtl;
                }
                if (DTableTaxDetail.Rows.Count > 0)
                {
                    ViewBag.TaxCalculatorDetail = DTableTaxDetail;
                }

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
                List<TaxCalciTaxName> _TaxCalciTaxName = new List<TaxCalciTaxName>();
                DataTable dt = _DomesticSaleInvoice_ISERVICE.GetTaxTypeList(Comp_ID, Br_ID);
                foreach (DataRow dr in dt.Rows)
                {
                    TaxCalciTaxName _TaxName = new TaxCalciTaxName();
                    _TaxName.tax_id = dr["tax_id"].ToString();
                    _TaxName.tax_name = dr["tax_name"].ToString();
                    _TaxCalciTaxName.Add(_TaxName);

                }
                _TaxCalciTaxName.Insert(0, new TaxCalciTaxName() { tax_id = "0", tax_name = "---Select---" });
                _DomesticSaleInvoice_Model.TaxCalciTaxNameList = _TaxCalciTaxName;
                _DomesticSaleInvoice_Model.TaxCalci_ItemName = ItemName;
                _DomesticSaleInvoice_Model.TaxCalci_AssessableValue = AssAmount;
                return PartialView("~/Areas/Common/Views/_TaxCalculationNew.cshtml", _DomesticSaleInvoice_Model);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType,string Mailerror)
        {
            // Session["Message"] = "";
            DomesticSaleInvoice_Model _DomesticSaleInvoice_Model = new DomesticSaleInvoice_Model();
            var a = TrancType.Split(',');
            _DomesticSaleInvoice_Model.SI_Number = a[0].Trim();
            _DomesticSaleInvoice_Model.SI_Date = a[1].Trim();
            _DomesticSaleInvoice_Model.DocumentMenuId = a[2].Trim();
            var WF_status1 = a[3].Trim();
            _DomesticSaleInvoice_Model.InvType = a[4].Trim();
            _DomesticSaleInvoice_Model.TransType = "Update";
            _DomesticSaleInvoice_Model.WF_status1 = WF_status1;
            _DomesticSaleInvoice_Model.BtnName = "BtnToDetailPage";
            _DomesticSaleInvoice_Model.Message = Mailerror;
            TempData["ModelData"] = _DomesticSaleInvoice_Model;
            TempData["ListFilterData"] = ListFilterData1;
            UrlModel URLModel = new UrlModel();
            URLModel.SI_Number = _DomesticSaleInvoice_Model.SI_Number;
            URLModel.SI_Date = _DomesticSaleInvoice_Model.SI_Date;
            URLModel.TransType = "Update"; ;
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
            URLModel.InvType = _DomesticSaleInvoice_Model.InvType;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("LocalSaleInvoiceDetail", URLModel);
        }
        public ActionResult GetSIList(string docid, string status)
        {
            //Session["WF_Docid"] = docid;
            //Session["WF_status"] = status;
            SI_ListModel _SI_ListModel = new SI_ListModel();
            _SI_ListModel.DocumentMenuId = docid;
            if (_SI_ListModel.DocumentMenuId != null)
            {
                if (_SI_ListModel.DocumentMenuId == "105103140")
                {
                    _SI_ListModel.WF_status = status;
                    return RedirectToAction("LocalSaleInvoiceList", _SI_ListModel);
                }
                if (_SI_ListModel.DocumentMenuId == "105103145125")
                {
                    _SI_ListModel.WF_status = status;
                    return RedirectToAction("CommercialInvoiceList", _SI_ListModel);
                }
            }
            return null;
        }
        public ActionResult SIListApprove(DomesticSaleInvoice_Model _DomesticSaleInvoice_Model, string ListFilterData1
            , string WF_status1, string docid, string PV_VoucherNarr, string BP_VoucherNarr,string DN_VoucherNarr)
        {
            try
            {

                string CompID = string.Empty;
                string UserID = string.Empty;
                string BrchID = string.Empty;
                string MenuID = string.Empty;
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
                if (docid != null && docid != "")
                {
                    MenuID = docid;
                    _DomesticSaleInvoice_Model.DocumentMenuId = docid;
                }
                else
                {
                    MenuID = _DomesticSaleInvoice_Model.DocumentMenuId;
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string SI_No = _DomesticSaleInvoice_Model.inv_no;
                string SI_Date = _DomesticSaleInvoice_Model.inv_dt;
                string A_Status = _DomesticSaleInvoice_Model.A_Status;
                string A_Level = _DomesticSaleInvoice_Model.A_Level;
                string A_Remarks = _DomesticSaleInvoice_Model.A_Remarks;
                string InvType = _DomesticSaleInvoice_Model.OrderType;
                string SaleVouMsg = _DomesticSaleInvoice_Model.SaleVouMsg;
                string DN_Nurr_Tcs = _DomesticSaleInvoice_Model.DN_Narration_Tcs;

                string Message = _DomesticSaleInvoice_ISERVICE.Approve_SI(CompID, BrchID, SI_No, SI_Date, InvType, UserID
                    , MenuID, mac_id, A_Status, A_Level, A_Remarks, SaleVouMsg, PV_VoucherNarr, BP_VoucherNarr, DN_VoucherNarr
                    , DN_Nurr_Tcs);
                string ApMessage = Message.Split(',')[1].Trim();
                string SINo = Message.Split(',')[0].Trim();
                string VouNo = Message.Split(',')[3].Trim();
                string VouDate = Message.Split(',')[4].Trim();
                string VouType = Message.Split(',')[5].Trim();
                if (ApMessage == "A")
                {
                    try
                    {
                        ViewBag.PrintFormat = PrintFormatDataTable(_DomesticSaleInvoice_Model,null);
                        //string fileName = "SI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "TaxInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(MenuID, SI_No, SI_Date, fileName,null, _DomesticSaleInvoice_Model.GstApplicable, MenuID,"AP");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, MenuID, SINo, "AP", Session["UserId"].ToString(), "", filePath);
                        //Session["Message"] = "Approved";
                       
                    }
                    catch (Exception exMail)
                    {
                        _DomesticSaleInvoice_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _DomesticSaleInvoice_Model.Message = _DomesticSaleInvoice_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";

                }
                
                
                    //Session["TransType"] = "Update";
                    //Session["Command"] = "Approve";
                    _DomesticSaleInvoice_Model.TransType = "Update";
                _DomesticSaleInvoice_Model.Command = "Approve";
                _DomesticSaleInvoice_Model.GLVoucherNo = VouNo;
                _DomesticSaleInvoice_Model.GLVoucherDt = VouDate;
                _DomesticSaleInvoice_Model.GLVoucherType = VouType;
                _DomesticSaleInvoice_Model.SI_Number = SINo;
                _DomesticSaleInvoice_Model.SI_Date = SI_Date;
                _DomesticSaleInvoice_Model.InvType = InvType;
                _DomesticSaleInvoice_Model.AppStatus = "D";
                _DomesticSaleInvoice_Model.BtnName = "BtnEdit";
                //_DomesticSaleInvoice_Model.FRoundOffAmt = _DomesticSaleInvoice_Model.FRoundOffAmt;
                _DomesticSaleInvoice_Model.WF_status1 = WF_status1;
                TempData["ModelData"] = _DomesticSaleInvoice_Model;
                UrlModel _urlApproveModel = new UrlModel();
                _urlApproveModel.SI_Number = _DomesticSaleInvoice_Model.SI_Number;
                _urlApproveModel.SI_Date = _DomesticSaleInvoice_Model.SI_Date;
                _urlApproveModel.InvType = InvType;
                _urlApproveModel.TransType = "Update";
                _urlApproveModel.Command = "Approve";
                _urlApproveModel.AppStatus = "D";
                _urlApproveModel.Command = "Approve";
                _urlApproveModel.DocumentMenuId = _DomesticSaleInvoice_Model.DocumentMenuId;
                _urlApproveModel.WF_status1 = _DomesticSaleInvoice_Model.WF_status1;
                //_urlApproveModel.FRoundOffAmt = _DomesticSaleInvoice_Model.FRoundOffAmt;
                //Session["SI_No"] = _DSI_Model.inv_no;
                //Session["InvType"] = _DSI_Model.OrderType;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("LocalSaleInvoiceDetail", _urlApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1, string docid
            ,string InvType,string PV_VoucherNarr,string BP_VoucherNarr, string DN_VoucherNarr)
        {
            DomesticSaleInvoice_Model _DomesticSaleInvoice_Model = new DomesticSaleInvoice_Model();

            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _DomesticSaleInvoice_Model.inv_no = jObjectBatch[i]["SINo"].ToString();
                    _DomesticSaleInvoice_Model.inv_dt = jObjectBatch[i]["SIDate"].ToString();
                    _DomesticSaleInvoice_Model.OrderType = jObjectBatch[i]["InvType"].ToString();
                    _DomesticSaleInvoice_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _DomesticSaleInvoice_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _DomesticSaleInvoice_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                    _DomesticSaleInvoice_Model.SaleVouMsg = jObjectBatch[i]["SaleVouMsg"].ToString();
                    _DomesticSaleInvoice_Model.DN_Narration_Tcs = jObjectBatch[i]["DN_Nurr_tcs"].ToString();
                    _DomesticSaleInvoice_Model.GstApplicable = jObjectBatch[i]["GstApplicable"].ToString();
                }
            }

            SIListApprove(_DomesticSaleInvoice_Model, ListFilterData1, WF_status1, docid, PV_VoucherNarr, BP_VoucherNarr,DN_VoucherNarr);
            TempData["ListFilterData"] = ListFilterData1;
            TempData["ModelData"] = _DomesticSaleInvoice_Model;
            UrlModel _urlApproveModel = new UrlModel();
            _urlApproveModel.SI_Number = _DomesticSaleInvoice_Model.SI_Number;
            _urlApproveModel.SI_Date = _DomesticSaleInvoice_Model.SI_Date;
            _urlApproveModel.InvType = _DomesticSaleInvoice_Model.InvType;
            _urlApproveModel.TransType = "Update";
            _urlApproveModel.Command = "Approve";
            _urlApproveModel.AppStatus = "D";
            _urlApproveModel.Command = "Approve";
            _urlApproveModel.DocumentMenuId = docid;
            _urlApproveModel.WF_status1 = _DomesticSaleInvoice_Model.WF_status1;
            return RedirectToAction("LocalSaleInvoiceDetail", _urlApproveModel);
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
        /*--------------------------For PDF Print Start--------------------------*/
        public FileResult GenratePdfFile(DomesticSaleInvoice_Model _model)
        {
            PrintOptionsList ProntOption = new PrintOptionsList();
            if (_model.HdnPrintOptons == "Y")
            {
                ProntOption.PrtOpt_catlog_number = _model.PrtOpt_catlog_number == "Y" ? true : false;
                ProntOption.PrtOpt_item_code = _model.PrtOpt_item_code == "Y" ? true : false;
                ProntOption.PrtOpt_item_desc = _model.PrtOpt_item_desc == "Y" ? true : false;
            }
            else
            {
                ProntOption.PrtOpt_catlog_number = true;
                ProntOption.PrtOpt_item_code = true;
                ProntOption.PrtOpt_item_desc = true;
            }
            //DataTable dt = new DataTable();
            //DataTable dtprintcopy = new DataTable();
            //dt.Columns.Add("PrintFormat", typeof(string));
            //dt.Columns.Add("ShowProdDesc", typeof(string));
            //dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            //dt.Columns.Add("ShowProdTechDesc", typeof(string));
            //dt.Columns.Add("ShowSubItem", typeof(string));
            //dt.Columns.Add("CustAliasName", typeof(string));
            //dt.Columns.Add("NumberOfCopy", typeof(int));
            //dt.Columns.Add("ItemAliasName", typeof(string));
            //dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            //dt.Columns.Add("showDeclare1", typeof(string));
            //dt.Columns.Add("showDeclare2", typeof(string));
            //dt.Columns.Add("showInvHeading", typeof(string));
            //dt.Columns.Add("PrintShipFromAddress", typeof(string));
            //DataRow dtr = dt.NewRow();
            //dtr["PrintFormat"] = _model.PrintFormat;
            //dtr["ShowProdDesc"] = _model.ShowProdDesc;
            //dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
            //dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
            //dtr["ShowSubItem"] = _model.ShowSubItem;
            //dtr["CustAliasName"] = _model.CustomerAliasName;
            //dtr["NumberOfCopy"] = _model.NumberofCopy;
            //dtr["ItemAliasName"] = _model.ItemAliasName;
            //dtr["ShowWithoutSybbol"] = _model.ShowWithoutSybbol;
            //dtr["showDeclare1"] = _model.showDeclare1;
            //dtr["showDeclare2"] = _model.showDeclare2;
            //dtr["showInvHeading"] = _model.showInvHeading;
            //dtr["PrintShipFromAddress"] = _model.PrintShipFromAddress;
            //dt.Rows.Add(dtr);

            DataTable dt = new DataTable();
            dt = PrintFormatDataTable(_model,null);
            ViewBag.PrintOption = dt;
            //dtprintcopy = dt;
            //return File(GetPdfData(_model.DocumentMenuId, _model.inv_no, _model.inv_dt, ProntOption), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
            if (_model.GstApplicable == "Y" && _model.DocumentMenuId != "105103145125")
                return File(GetPdfDataOfGstInv(dt, _model.DocumentMenuId, _model.inv_no, _model.inv_dt, ProntOption, _model.NumberofCopy), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");
            else
                return File(GetPdfData_Print(_model.DocumentMenuId, _model.inv_no, _model.inv_dt, _model.NumberofCopy, ProntOption), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");
        }
        private DataTable PrintFormatDataTable(DomesticSaleInvoice_Model _model, string PrintFormat)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("NumberOfCopy", typeof(int));
            dt.Columns.Add("ItemAliasName", typeof(string));
            dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            dt.Columns.Add("showDeclare1", typeof(string));
            dt.Columns.Add("showDeclare2", typeof(string));
            dt.Columns.Add("showInvHeading", typeof(string));
            dt.Columns.Add("PrintShipFromAddress", typeof(string));
            dt.Columns.Add("PrintCorpAddr", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));
            if (_model != null)
            {
                DataRow dtr = dt.NewRow();
                dtr["PrintFormat"] = _model.PrintFormat;
                dtr["ShowProdDesc"] = _model.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
                dtr["ShowSubItem"] = _model.ShowSubItem;
                dtr["CustAliasName"] = _model.CustomerAliasName;
                dtr["NumberOfCopy"] = IsNull(_model.NumberofCopy.ToString(), "1");
                dtr["ItemAliasName"] = _model.ItemAliasName;
                dtr["ShowWithoutSybbol"] = _model.ShowWithoutSybbol;
                dtr["showDeclare1"] = _model.showDeclare1;
                dtr["showDeclare2"] = _model.showDeclare2;
                dtr["showInvHeading"] = _model.showInvHeading;
                dtr["PrintShipFromAddress"] = _model.PrintShipFromAddress;
                dtr["PrintCorpAddr"] = _model.PrintCorpAddr;
                dtr["PrintRemarks"] = _model.PrintRemarks;
                dt.Rows.Add(dtr);
            }
            else
            {
                DataRow dtr = dt.NewRow();
                if (!string.IsNullOrEmpty(PrintFormat))
                {
                    JArray jObject = JArray.Parse(PrintFormat);
                    dtr["PrintFormat"] = jObject[0]["PrintFormat"].ToString();
                    dtr["ShowProdDesc"] = jObject[0]["ShowProdDesc"];
                    dtr["ShowCustSpecProdDesc"] = jObject[0]["ShowCustSpecProdDesc"];
                    dtr["ShowProdTechDesc"] = jObject[0]["ShowProdTechDesc"];
                    dtr["ShowSubItem"] = jObject[0]["ShowSubItem"];
                    dtr["ItemAliasName"] = jObject[0]["ItemAliasName"];
                    dtr["CustAliasName"] = jObject[0]["CustAliasName"];
                    dtr["NumberOfCopy"] = jObject[0]["NumberOfCopy"];
                    dtr["ShowWithoutSybbol"] = jObject[0]["ShowWithoutSybbol"];
                    dtr["showDeclare1"] = jObject[0]["showDeclare1"];
                    dtr["showDeclare2"] = jObject[0]["showDeclare2"];
                    dtr["showInvHeading"] = jObject[0]["showInvHeading"];
                    dtr["PrintShipFromAddress"] = jObject[0]["PrintShipFromAddress"];
                    dtr["PrintCorpAddr"] = jObject[0]["PrintCorpAddr"];
                    dtr["PrintRemarks"] = jObject[0]["PrintRemarks"];
                    dt.Rows.Add(dtr);
                }
                //dt.Rows.Add(dtr);
            }
            
            return dt;
        }
        //public static string IsNull(string In, string Out)
        //{
        //    return string.IsNullOrEmpty(In) ? Out : In;
        //}
        public byte[] GetPdfData_Print(string docId, string invNo, string invDt, int NumberofCopy, PrintOptionsList ProntOption)
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
                if (docId == "105103140")
                {
                    inv_type = "SI";
                }
                if (docId == "105103145125")
                {
                    inv_type = "CI";
                }
                DataSet Details = _DomesticSaleInvoice_ISERVICE.GetSalesInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt, inv_type);
                ViewBag.PageName = "SI";
                string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp)
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                //if (invType == "D")/*commented by Hina sharma on 01-10-2024 to show multiple print copy*/
                //{
                //    ViewBag.Title = "Sales Invoice";
                //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoicePrint.cshtml"));
                //}
                //else
                //{
                //    ViewBag.Title = "Commercial Invoice";
                //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/CommercialInvoicePrint.cshtml"));
                //}
                if (invType != "D")/*Add by Hina sharma on 01-10-2024 to show single print copy*/
                {
                    ViewBag.Title = "Commercial Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/CommercialInvoicePrint.cshtml"));
                }
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (ReportType == "common")
                    {
                        if (inv_type == "SI")
                        {
                            pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                        }
                        else
                        {
                            pdfDoc = new Document(PageSize.A4, 20f, 20f, 70f, 90f);
                        }
                    }

                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    Int32 noofprint = NumberofCopy;/*Add code by Hina sharma on 01-10-2024 to show multiple print copy*/
                    for (int i = 1; i <= noofprint; i++)
                    {

                        if (noofprint > 1)
                        {
                            ViewBag.Copyno = i;
                        }
                        else
                        {
                            ViewBag.Copyno = null;
                        }
                        if (invType == "D")
                        {
                            ViewBag.Title = "Sales Invoice";
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoicePrint.cshtml"));
                        }
                        //else
                        //{
                        //    ViewBag.Title = "Commercial Invoice";
                        //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/CommercialInvoicePrint.cshtml"));
                        //}
                        reader = new StringReader(htmlcontent);

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                        pdfDoc.NewPage();
                    }
                    //if (invType != "D")
                    //{
                    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    //}
                    if (invType != "D")
                    {
                        using (var Reader1 = new StringReader(htmlcontent))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, Reader1);
                        }
                    }
                    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);/*commented by Hina sharma on 01-10-2024 to show multiple print copy*/
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = HeaderFooterPagination(bytes, Details, ReportType, inv_type);
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
        public byte[] GetPdfData(string docId, string invNo, string invDt,PrintOptionsList ProntOption)
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
                if (docId == "105103140")
                {
                    inv_type = "SI";
                }
                if (docId == "105103145125")
                {
                    inv_type = "CI";
                }
                DataSet Details = _DomesticSaleInvoice_ISERVICE.GetSalesInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt, inv_type);
                ViewBag.PageName = "SI";
                string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp)
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                if (invType == "D")
                {
                    ViewBag.Title = "Sales Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoicePrint.cshtml"));
                }
                else
                {
                    ViewBag.Title = "Commercial Invoice";
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/CommercialInvoicePrint.cshtml"));
                }

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (ReportType == "common")
                    {
                        if (inv_type == "SI")
                        {
                            pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                        }
                        else
                        {
                            pdfDoc = new Document(PageSize.A4, 20f, 20f, 70f, 90f);
                        }
                    }

                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = HeaderFooterPagination(bytes, Details, ReportType, inv_type);
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
        public byte[] GetPdfDataOfGstInv(DataTable dt, string docId, string invNo, string invDt, PrintOptionsList ProntOption,int NumberofCopy)
        {
            StringReader reader = null;
            //StringReader reader1 = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            //PdfWriter writer1 = null;
            try
            {
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();
                string inv_type = "";
                string ReportType = "common";
                switch (docId)
                {
                    case "105103140":
                        inv_type = "SI";
                        break;
                    case "105103145125":
                        inv_type = "CI";
                        break;
                    default:
                        inv_type = "";
                        break;
                }
                DataSet Details = _DomesticSaleInvoice_ISERVICE.GetSlsInvGstDtlForPrint(CompID, Br_ID, invNo, invDt, inv_type);
                ViewBag.PageName = "SI";
                string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();

                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.ProntOption = ProntOption;
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();
                string htmlcontent = "";
                //string htmlcontentDupli = "";
                
                ViewBag.Title = "Tax Invoice";
                
                //if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                //{
                //    htmlcontentOrigin = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrintF2.cshtml"));
                //}
                //else
                //{
                //    ViewBag.Copy = "Original";
                //    htmlcontentOrigin = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrint.cshtml"));
                //    //ViewBag.Copy = "Duplicate";
                //    //htmlcontentDupli = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrint.cshtml"));
                    
                //}
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    
                    //reader1 = new StringReader(htmlcontentDupli);
                    if (ReportType == "common")
                    {
                        if (inv_type == "SI")
                        {
                            if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F3")
                            {
                                pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 60f);
                            }
                            else
                            {
                                pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 60f);
                            }
                        }
                        else
                        {
                            pdfDoc = new Document(PageSize.A4, 10f, 10f, 70f, 90f);
                        }
                    }
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    //Int32 noofprint = Convert.ToInt32(Details.Tables[0].Rows[0]["noofcopy"]);
                    //var numberofprcopy = dt.Rows[0]["NumberOfCopy"].ToString();/*Add by Hina sharma on 01-10-2024*/
                    var numberofprcopy = NumberofCopy;/*Add by Shubham Maurya on 28-05-2024*/
                    Int32 noofprint = Convert.ToInt32(numberofprcopy);
                    for (int i= 1; i<= noofprint; i++)
                    {
                        
                        if (noofprint > 1)
                        {
                            ViewBag.Copyno = i;
                        }
                        else
                        {
                            ViewBag.Copyno = null;
                        }
                        if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                        {
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrintF2.cshtml"));
                        }
                        else if(dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F3")
                        {
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrintF3.cshtml"));
                        }
                        else
                        {   
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSaleInvoice/SalesInvoiceWithGSTPrint.cshtml"));
                        }
                        reader = new StringReader(htmlcontent);

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                        pdfDoc.NewPage();
                    }

                    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader1);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = GSTHeaderFooterPagination(bytes, Details, ReportType, inv_type, Convert.ToInt32(numberofprcopy));
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
        private Byte[] HeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType, string inv_type)
        {
            var docstatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
            var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 13, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = Server.MapPath(Details.Tables[0].Rows[0]["logo"].ToString());
            //     string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string draftImage = "";
            if (docstatus == "D" || docstatus == "F")
            {
                 draftImage = Server.MapPath("~/Content/Images/draft.png");
            }
            else if(docstatus == "C")
            {
                draftImage = Server.MapPath("~/Content/Images/cancelled.png");
            }
            else
            {
                draftImage = Server.MapPath("~/Content/Images/draft.png");
            }

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        if (ReportType == "common")
                        {
                            if (inv_type == "SI")
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 220);
                                draftimg.ScaleAbsolute(550f, 600f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F" || docstatus == "C")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                   
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                }
                            }
                            else
                            {


                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 220);
                                draftimg.ScaleAbsolute(550f, 600f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F" || docstatus == "C")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    try
                                    {
                                        //var image = Image.GetInstance(logo);
                                        //image.SetAbsolutePosition(31, 794);
                                        //image.ScaleAbsolute(68f, 15f);
                                        //content.AddImage(image);
                                    }
                                    catch { }
                                    content.Rectangle(34.5, 28, 526, 60);


                                    BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                                    content.BeginText();
                                    content.SetColorFill(CMYKColor.BLACK);
                                    content.SetFontAndSize(baseFont1, 9);
                                    content.CreateTemplate(20, 10);
                                    content.SetLineWidth(25);

                                    var txt = Details.Tables[0].Rows[0]["declar"].ToString();
                                    string[] stringSeparators = new string[] { "\r\n" };
                                    string text = txt;
                                    string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

                                    var y = 65;
                                    for (var j = 0; j < lines.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_LEFT, lines[j], 40, y, 0);
                                        y = y - 10;
                                    }

                                    txt = Details.Tables[0].Rows[0]["inv_head"].ToString();
                                    text = txt;
                                    string[] lines1 = text.Split(stringSeparators, StringSplitOptions.None);

                                    content.SetFontAndSize(baseFont1, 8);
                                    y = 771;
                                    for (var j = 0; j < lines1.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_CENTER, lines1[j], 300, y, 0);
                                        y = y - 10;
                                    }
                                    content.SetFontAndSize(baseFont1, 9);

                                    content.EndText();

                                    //content.Rectangle(450, 25, 120, 35);
                                    string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
                                    Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);
                                    Phrase ptitle = new Phrase(String.Format("Commercial Invoice", i, PageCount), fonttitle);
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
                                    //Phrase p7 = new Phrase(String.Format("Signature & date", i, PageCount), fontb);
                                    Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
                                    Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
                                    //Phrase p1 = new Phrase(Details.Tables[0].Rows[0]["declar"].ToString(), font1);
                                    //Phrase p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
                                    //Phrase p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
                                    //Phrase p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
                                    /*------------------Header ---------------------------*/
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

                                    /*------------------Header end---------------------------*/

                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
                                    //ColumnText.AddText(p1);
                                    //content.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Details.Tables[0].Rows[0]["declar"].ToString(), 300, 400, 45);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
                                }
                            }
                        }


                    }
                    bytes = ms.ToArray();
                }
            }

            return bytes;
        }
        private Byte[] GSTHeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType, string inv_type,int numberofprcopy)
        {
            string Br_ID = string.Empty;
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var docstatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
            var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            Font font2 = new Font(bf, 6, Font.NORMAL);/*Add by Hina on 07-12-2024*/
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 15, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());
            string State_Name = Details.Tables[9].Rows[0]["state_name"].ToString();
            String StateName = (State_Name).ToUpper();
            ViewBag.QrCode = QR;
            string draftImage = "";
            if (docstatus == "D" || docstatus == "F")
            {
                 draftImage = Server.MapPath("~/Content/Images/draft.png");
            }
            else if(docstatus == "C")
            {
                 draftImage = Server.MapPath("~/Content/Images/cancelled.png");
            }
            else
            {
                 draftImage = Server.MapPath("~/Content/Images/draft.png");
            }
               
            string bnetImage = Server.MapPath("~/images/businet.png");

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        if (ReportType == "common")
                        {
                            if (inv_type == "SI")
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 40);
                                draftimg.ScaleAbsolute(650f, 600f);



                                var qrCode = Image.GetInstance(QR);
                                qrCode.SetAbsolutePosition(475,725 );
                                //qrCode.ScaleAbsolute(100f, 95f);
                                qrCode.ScaleAbsolute(100f, 90f);



                                int PageCount = reader1.NumberOfPages;
                                var PageCount1 = reader1.NumberOfPages / numberofprcopy;

                                int count = 0;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F" || docstatus == "C")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    //try
                                    //{
                                    //    var image = Image.GetInstance(logo);
                                    //    image.SetAbsolutePosition(30, 540);
                                    //    image.ScaleAbsolute(90f, 25f);
                                    //    if (i == 1)
                                    //        content.AddImage(image);
                                    //}
                                    //catch { }
                                    //if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["inv_qr_code"].ToString()))
                                    //{
                                    //    //if (i == 1)
                                    //    if (PageCount1 == 1)
                                    //        content.AddImage(qrCode);
                                    //}
                                    //Phrase ptitle = new Phrase(String.Format("Tax Invoice", i, PageCount), fonttitle);
                                    if (PageCount1 > count)//add by shubham maurya on 17-04-2025 
                                        count = count + 1;
                                    else
                                        count = 1;
                                    if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["inv_qr_code"].ToString()))
                                    {
                                        //if (i == 1)
                                        if (count == 1)
                                            content.AddImage(qrCode);
                                    }
                                    //Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", count, PageCount1), font);
                                    if (count == PageCount1)
                                    {
                                        //try
                                        //{
                                        //    var bnetlogo = Image.GetInstance(bnetImage);
                                        //    //bnetlogo.SetAbsolutePosition(322, 35);/**commented and modify by Hina sharma on 07-12-2024 to change small size/
                                        //    //bnetlogo.ScaleAbsolute(70f, 16f);
                                        //    bnetlogo.SetAbsolutePosition(322, 38);
                                        //    bnetlogo.ScaleAbsolute(40f, 10f);
                                        //    content.AddImage(bnetlogo);

                                        //}
                                        //catch { }
                                        //Phrase ftr = new Phrase(String.Format("This Document is generated by ", i, PageCount), font);
                                        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, ftr, 320, 40, 0);
                                        Phrase ftr1 = new Phrase(String.Format("SUBJECT TO " + StateName + " JURISDICTION ", i, PageCount), font2);
                                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ftr1, 298, 40, 0);
                                        // 500,560 is for left right alignment of text and 40,60 is for above or below text of align.

                                    }
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, p, 560, 15, 0);
                                    //if(i == 1)
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 400, 550, 0);
                                }
                            }
                            else
                            {
                                //var image = Image.GetInstance(logo);
                                //image.SetAbsolutePosition(31, 794);
                                //image.ScaleAbsolute(68f, 15f);

                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 220);
                                draftimg.ScaleAbsolute(550f, 600f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F" || docstatus == "C")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    //content.AddImage(image);
                                    content.Rectangle(34.5, 28, 526, 60);


                                    BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                                    content.BeginText();
                                    content.SetColorFill(CMYKColor.BLACK);
                                    content.SetFontAndSize(baseFont1, 9);
                                    content.CreateTemplate(20, 10);
                                    content.SetLineWidth(25);

                                    var txt = Details.Tables[0].Rows[0]["declar"].ToString();
                                    string[] stringSeparators = new string[] { "\r\n" };
                                    string text = txt;
                                    string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

                                    var y = 65;
                                    for (var j = 0; j < lines.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_LEFT, lines[j], 40, y, 0);
                                        y = y - 10;
                                    }

                                    txt = Details.Tables[0].Rows[0]["inv_head"].ToString();
                                    text = txt;
                                    string[] lines1 = text.Split(stringSeparators, StringSplitOptions.None);

                                    content.SetFontAndSize(baseFont1, 8);
                                    y = 771;
                                    for (var j = 0; j < lines1.Length; j++)
                                    {
                                        content.ShowTextAlignedKerned(PdfContentByte.ALIGN_CENTER, lines1[j], 300, y, 0);
                                        y = y - 10;
                                    }
                                    content.SetFontAndSize(baseFont1, 9);
                                    content.EndText();
                                    //content.Rectangle(450, 25, 120, 35);
                                    string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
                                    Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);
                                    Phrase ptitle = new Phrase(String.Format("Commercial Invoice", i, PageCount), fonttitle);
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
                                    //Phrase p7 = new Phrase(String.Format("Signature & date", i, PageCount), fontb);
                                    Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
                                    Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
                                    //Phrase p1 = new Phrase(Details.Tables[0].Rows[0]["declar"].ToString(), font1);
                                    //Phrase p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
                                    //Phrase p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
                                    //Phrase p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
                                    /*------------------Header ---------------------------*/
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

                                    /*------------------Header end---------------------------*/

                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
                                    //ColumnText.AddText(p1);
                                    //content.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Details.Tables[0].Rows[0]["declar"].ToString(), 300, 400, 45);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
                                    //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
                                }
                            }
                        }

                    }
                    bytes = ms.ToArray();
                }
            }
            return bytes;
        }
        private string GenerateQRCode(string qrcodeText)
        {
            if (string.IsNullOrEmpty(qrcodeText))
                qrcodeText = "N/A";
            string path = Server.MapPath("~");
            string fileName = "QR_" + Guid.NewGuid().ToString();
            string folderPath = path + ("..\\LogsFile\\EmailAlertPDFs\\");
            string imagePath = folderPath + fileName + ".jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = imagePath;
            var barcodeBitmap = new System.Drawing.Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
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
        public ActionResult GetSelectPrintTypePopup(string PrintOptions, string DocMenuId)
        {
            try
            {
                ViewBag.PrintCommand = "Y";
                ViewBag.PrintOptions = PrintOptions;
                ViewBag.DocMenuId = DocMenuId;
                return PartialView("~/Areas/Common/Views/Cmn_SelectPrintType.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }

        /*--------------------------For PDF Print End--------------------------*/
        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                SaleInvoiceModelattch _SaleInvoiceModelattch = new SaleInvoiceModelattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string SI_No = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["SI_No"] != null)
                //{
                //    SI_No = Session["SI_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _SaleInvoiceModelattch.Guid = DocNo;
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
                    //Session["AttachMentDetailItmStp"] = dt;
                    _SaleInvoiceModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _SaleInvoiceModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _SaleInvoiceModelattch;
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
                DataSet GlDt = _DomesticSaleInvoice_ISERVICE.GetAllGLDetails(DtblGLDetail);
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
            return Validate;
        }
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
   , string Flag, string Status, string SrcDoc_no, string SrcDoc_dt, string Doc_no, string Doc_dt,string DocumentMenuId)
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
                ViewBag.DocumentMenuId = DocumentMenuId;
                DataTable dt = new DataTable();
                if (Flag == "Shipped" || Flag == "ShippedFoc")
                {
                    if (Status == "" || Status == "D" || Status == "F")
                    {
                        dt = _DomesticSaleInvoice_ISERVICE.SI_GetSubItemDetails(CompID, BrchID, Item_id, SrcDoc_no, SrcDoc_dt, Doc_no, Doc_dt, Flag, DocumentMenuId).Tables[0];
                    }
                    else
                    {
                        dt = _DomesticSaleInvoice_ISERVICE.SI_GetSubItemDetailsafterapprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, DocumentMenuId, Flag).Tables[0];
                    }
                }

                //else if (Flag == "Received")
                //{
                //    dt = _MTO_ISERVICES.MTR_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                //}

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    //_subitemPageName = "MTO",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y"

                };

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag == "Quantity" ? Flag : "MTO";
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public string SavePdfDocToSendOnEmailAlert(string docId, string shpNo, string shpDt,string fileName, string PrintFormat, string GstApplicable,string docid,string docstatus)
        {
            PrintOptionsList ProntOption = new PrintOptionsList()
            {
                PrtOpt_catlog_number = true,
                PrtOpt_item_code = true,
                PrtOpt_item_desc = true
            };
            var commonCont = new CommonController(_Common_IServices);
            if (ViewBag.PrintFormat == null)
            {
                if (PrintFormat != null)
                {
                    //dt = commonCont.PrintOptionsDt(PrintFormat); //Added by Suraj on 08-10-2024
                    dt = PrintFormatDataTable(null, PrintFormat);
                }
            }
            else
            {
                dt = ViewBag.PrintFormat;
            }
            ViewBag.PrintOption = dt;


            //var data = GetPdfData(docId, shpNo, shpDt, ProntOption);
            //var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        if (GstApplicable == "Y" && docId != "105103145125")//added by shubham maurya on 05-07-2025
                        {
                            var data = GetPdfDataOfGstInv(dt, docId, shpNo, shpDt, ProntOption, 1);
                            return commonCont.SaveAlertDocument(data, fileName);
                        }
                        else
                        {
                            var data = GetPdfData_Print(docId, shpNo, shpDt, 1, ProntOption);
                            return commonCont.SaveAlertDocument(data, fileName);
                        }
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
       /*------------- Cost Center Section-----------------*/
        public ActionResult GetCstCntrtype(string Flag, string Disableflag, string CC_rowdata,string DocumentMenuId=null)
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
                ViewBag.DocumentMenuId = DocumentMenuId;
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
        [NonAction]
        public DataTable BindBankDropdown()
        {
            try
            {
                //  List<BankName> _BankName = new List<BankName>();
                //  _DomesticSaleInvoice_Model = new DomesticSaleInvoice_Model();
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
                DataTable dt = _DomesticSaleInvoice_ISERVICE.GetBankdetail(Comp_ID, Br_ID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return Json("ErrorPage");
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult getbankdetail(string bankName)
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
                DataSet GlDt = _DomesticSaleInvoice_ISERVICE.getbankdeatils(bankName, Comp_ID, Br_ID);
                DataRows = Json(JsonConvert.SerializeObject(GlDt));

                return DataRows;
            }

            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public FileResult ExportItemsToExcel(string invNo, string invDate, string docId)
        {
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            string UserID = "";
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DocumentMenuId = "105103130";
            //ITEMDETAILS
            DataTable DtItems = _DomesticSaleInvoice_ISERVICE.GetSaleInvoiceItemstoExportExcel(CompID, BrchID, invNo, invDate, docId);
            var commonController = new CommonController(_Common_IServices);
            return commonController.ExportDatatableToExcel("DomesticSaleInv_Items", DtItems);
        }
        //Added by Nidhi on 26-08-2025
        public string SavePdfDocToSendOnEmailAlert_Ext(DomesticSaleInvoice_Model _model, string Doc_no, string Doc_dt, string docid, string fileName, string PrintFormat, string GstApplicable)
        {
            var printOptionsList = JsonConvert.DeserializeObject<List<DomesticSaleInvoice_Model>>(PrintFormat);
            var commonCont = new CommonController(_Common_IServices);
            PrintOptionsList ProntOption = new PrintOptionsList()
            {
                PrtOpt_catlog_number = true,
                PrtOpt_item_code = true,
                PrtOpt_item_desc = true
            };
            if(string.IsNullOrEmpty(PrintFormat))
            {
                dt = PrintFormatDataTable(_model, PrintFormat);
            }
            else
            {
                dt = PrintFormatDataTable(null, PrintFormat);
            }
            ViewBag.PrintOption = dt;

            if (GstApplicable == "Y" && docid != "105103145125")
            {
                var data = GetPdfDataOfGstInv(dt, docid, Doc_no, Doc_dt, ProntOption, 1);
                return commonCont.SaveAlertDocument_MailExt(data, fileName);
            }
            else
            {
                var data = GetPdfData_Print(docid, Doc_no, Doc_dt, 1, ProntOption);
                return commonCont.SaveAlertDocument_MailExt(data, fileName);
            }
        }
        public ActionResult SendEmailAlert(DomesticSaleInvoice_Model _model, string mail_id, string status, string docid, string SrcType, string Doc_no, string Doc_dt, string statusAM, string filepath, string GstApplicable)
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
                //var _Model = TempData["ModelData"] as PODetailsModel;
                DataTable dt = new DataTable();
                string message = "";
                string mail_cont = "";
                string file_path = "";
                if (status == "A")
                {
                    try
                    {
                        if (filepath == "" || filepath == null)
                        {
                            string fileName = "SI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            filepath = SavePdfDocToSendOnEmailAlert_Ext(_model,Doc_no, Doc_dt, docid, fileName, "", GstApplicable);
                        }
                        string keyword = @"\ExternalEmailAlertPDFs\";
                        int index = filepath.IndexOf(keyword);
                        file_path = (index >= 0) ? filepath.Substring(index) : filepath;
                        message = _Common_IServices.SendAlertEmailExternal(CompID, BrchID, UserID, docid, Doc_no, "A", mail_id, filepath);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            if (message == "invalidemail")
                            {
                                mail_cont = "Invalid email body configuration";
                            }
                            if (message == "invalid")
                            {
                                mail_cont = "Invalid sender email configuration";
                            }
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                }
                if (status == "C" || status == "FC" || status == "AM")
                {
                    try
                    {
                        message = _Common_IServices.SendAlertEmailExternal1(CompID, BrchID, UserID, docid, Doc_no, status, mail_id);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
                        }
                        else
                        {
                            if (message == "invalidemail")
                            {
                                mail_cont = "Invalid email body configuration";
                            }
                            if (message == "invalid")
                            {
                                mail_cont = "Invalid sender email configuration";
                            }
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt.ToString(), docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
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