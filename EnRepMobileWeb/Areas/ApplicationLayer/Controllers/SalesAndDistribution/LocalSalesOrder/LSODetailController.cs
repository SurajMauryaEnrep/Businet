
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.SalesAndDistributionModels;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.SalesAndDistributionIServices;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution
{
    public class LSODetailController : Controller
    {
        //string comp_id, userid, CustId = String.Empty;
        LSODetailModel _LSODetailModel;
        string CompID, UserID, BrchID, language, title = String.Empty, crm = "Y", rpt_id, UserName;
        string Br_ID = string.Empty;
        string DocumentMenuId = "";

        Common_IServices _Common_IServices;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        LSODetail_ISERVICE _LSODetail_ISERVICE;

        public LSODetailController(Common_IServices _Common_IServices, LSODetail_ISERVICE _LSODetail_ISERVICE)
        {
            this._LSODetail_ISERVICE = _LSODetail_ISERVICE;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/LSO
        public ActionResult LSODetail(UrlModel _urlModel)
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
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYearAndPreviousYear(CompID, Br_ID, _urlModel.SO_Date) == "TransNotAllow")
            //{
            //    //TempData["Message2"] = "TransNotAllow";
            //    ViewBag.Message = "TransNotAllow";
            //}
            try
            {
                var _LSODetailModel = TempData["ModelData"] as LSODetailModel;
                if (_LSODetailModel != null)
                {
                    _LSODetailModel.ILSearch = null;
                   
                    if (_urlModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = _urlModel.DocumentMenuId;
                        _LSODetailModel.MenuDocumentId = _urlModel.DocumentMenuId;
                    }
                    else
                    {
                        DocumentMenuId = _LSODetailModel.DocumentMenuId;
                        _LSODetailModel.MenuDocumentId = _LSODetailModel.DocumentMenuId;
                    }
                    if (_urlModel.CustType != null)
                    {
                        var CustType = _urlModel.CustType;
                        _LSODetailModel.CustType = _urlModel.CustType;
                    }
                    else
                    {
                        var CustType = _LSODetailModel.CustType;
                    }
                    _LSODetailModel.FC = _urlModel.FC;
                    // BindReplicateWithlist(_LSODetailModel);
                    ViewBag.DocID = DocumentMenuId;
                    string Comp_ID = string.Empty;
                    CommonPageDetails();

                    List<CustName> _CustList = new List<CustName>();

                    _CustList.Insert(0, new CustName() { Cust_id = "0", Cust_name = "---Select---" });
                    _LSODetailModel.CustNameList = _CustList;

                    //List<Currency> Currency = new List<Currency>();
                    //Currency.Insert(0, new Currency() { curr_id = "0", curr_val = "---Select---" });
                    //_LSODetailModel.CurrencyList = Currency;

                    GetAllData(_LSODetailModel);
                    //DataTable dtcurr = GetCurrList();
                    //List<Currency> Currency = new List<Currency>();
                    //foreach (DataRow dr in dtcurr.Rows)
                    //{
                    //    Currency _curr = new Currency();
                    //    _curr.curr_id = dr["curr_id"].ToString();
                    //    _curr.curr_val = dr["curr_name"].ToString();
                    //    Currency.Add(_curr);
                    //}
                    //Currency.Insert(0, new Currency() { curr_id = "0", curr_val = "---Select---" });
                    //_LSODetailModel.CurrencyList = Currency;


                    List<QuotationList> quotations = new List<QuotationList>();
                    quotations.Insert(0, new QuotationList() { SrcDocNo = "0", SrcDocNoVal = "---Select---" });
                    _LSODetailModel.quotationsList = quotations;

                    List<SourceDocNo> sourceDocs = new List<SourceDocNo>();
                    sourceDocs.Insert(0, new SourceDocNo() { Doc_id = "0", Dic_val = "---Select---" });
                    _LSODetailModel.SourceDocList = sourceDocs;

                    List<SO_CountryList> _CountryLists = new List<SO_CountryList>();
                    _CountryLists.Insert(0, new SO_CountryList() { Cntry_id = "0", Cntry_val = "---Select---" });
                    _LSODetailModel._CountryLists = _CountryLists;

                    List<trade_termList> _TermLists = new List<trade_termList>();
                    //_TermLists.Insert(0, new trade_termList() { TrdTrms_id = "0", TrdTrms_val = "---Select---" });//commented by Suraj on 23-10-2023
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });//Added by Suraj on 23-10-2023
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });

                    _LSODetailModel.TradeTermsList = _TermLists;
                    string ValDigit = "";
                    string QtyDigit = "";
                    string RateDigit = "";
                    if (_LSODetailModel.MenuDocumentId== "105103145110")
                    {
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"].ToString()));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"].ToString()));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpRateDigit"].ToString()));
                    }
                    else
                    {
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    }                    
                    _LSODetailModel.ValDigit = ValDigit;
                    _LSODetailModel.QtyDigit = QtyDigit;
                    _LSODetailModel.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    _LSODetailModel.ApplyTax = "E";

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _LSODetailModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["SO_No"] != null && Session["SO_Date"] != null)
                    if (_LSODetailModel.SO_Number != null && _LSODetailModel.SO_Date != null)
                    {
                        //string Doc_no = Session["SO_No"].ToString();
                        string Doc_no = _LSODetailModel.SO_Number;
                        string Doc_date = _LSODetailModel.SO_Date;
                         DataSet ds = GetSODetailEdit(Doc_no, Doc_date);
                      
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_Model.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            _LSODetailModel.SO_OrderType = ds.Tables[0].Rows[0]["order_type"].ToString();
                            _LSODetailModel.ApplyTax = ds.Tables[0].Rows[0]["apply_tax"].ToString();
                            _LSODetailModel.SO_SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();
                            _LSODetailModel.SO_no = Doc_no;// ds.Tables[0].Rows[0]["po_no"].ToString();
                            _LSODetailModel.SO_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["so_dt"].ToString()).ToString("yyyy-MM-dd");
                            _LSODetailModel.SO_Remarks = ds.Tables[0].Rows[0]["so_rem"].ToString();
                            _LSODetailModel.SO_AvlCreditLimit = ds.Tables[0].Rows[0]["cre_limit"].ToString();
                            _LSODetailModel.SO_RefDocNo = ds.Tables[0].Rows[0]["ref_doc_no"].ToString();
                            //_LSODetailModel.SO_SourceDocDate = ds.Tables[0].Rows[0]["src_doc_date"].ToString();
                            _LSODetailModel.SO_CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();
                            _LSODetailModel.SO_CustID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                            _LSODetailModel.cust_alias = ds.Tables[0].Rows[0]["cust_alias"].ToString();
                            _LSODetailModel.SpanCustPricePolicy = ds.Tables[0].Rows[0]["cust_pr_pol"].ToString();
                            _LSODetailModel.SpanCustPriceGroup = ds.Tables[0].Rows[0]["cust_pr_grp"].ToString();
                            _LSODetailModel.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                            _LSODetailModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                           
                            _CustList.Add(new CustName { Cust_id = _LSODetailModel.SO_CustID, Cust_name = _LSODetailModel.SO_CustName });
                            _LSODetailModel.CustNameList = _CustList;
                            /*start Code add by Hina on 12-06-2025 for single quotation*/
                            _LSODetailModel.SO_SourceDocNo = ds.Tables[1].Rows[0]["src_doc_number"].ToString();
                            _LSODetailModel.SO_SourceDocDate = ds.Tables[1].Rows[0]["src_doc_date"].ToString();
                            List<QuotationList> quotations1 = new List<QuotationList>();
                            quotations1.Add(new QuotationList { SrcDocNo = _LSODetailModel.SO_SourceDocNo, SrcDocNoVal = _LSODetailModel.SO_SourceDocNo});
                            _LSODetailModel.quotationsList = quotations1;

                            /*end Code add by Hina on 12-06-2025 for single quotation*/


                            //string sls_pers_name = ds.Tables[0].Rows[0]["sls_pers_name"].ToString();
                            // _LSODetailModel.SO_SalePerson = ds.Tables[0].Rows[0]["sls_pers_name"].ToString();
                            _LSODetailModel.SO_SalePerson = ds.Tables[0].Rows[0]["emp_id"].ToString();
                            //SalePerson.Add(new SalePerson { salep_id = _LSODetailModel.SO_SalePerson, salep_name = sls_pers_name });
                            //_LSODetailModel.SalePersonList = SalePerson;
                            string CntryOfDest = ds.Tables[0].Rows[0]["country_name"].ToString();
                            if (CntryOfDest == "") { CntryOfDest = "---Select---"; }
                            _LSODetailModel.SO_Country = ds.Tables[0].Rows[0]["cntry_dest"].ToString();
                            _CountryLists.Add(new SO_CountryList { Cntry_id = _LSODetailModel.SO_Country, Cntry_val = CntryOfDest });
                            _LSODetailModel._CountryLists = _CountryLists;
                            _LSODetailModel.SO_PortOfDest = ds.Tables[0].Rows[0]["port_dest"].ToString();
                            _LSODetailModel.SO_ExportFileNo = ds.Tables[0].Rows[0]["exp_file_no"].ToString();
                            _LSODetailModel.SO_RefDocNo = ds.Tables[0].Rows[0]["ref_doc_no"].ToString();
                            _LSODetailModel.trade_term = ds.Tables[0].Rows[0]["trade_terms"].ToString();
                            _LSODetailModel.SO_BillingAddress = ds.Tables[0].Rows[0]["bill_address"].ToString();
                            _LSODetailModel.SO_ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                            //_LSODetailModel.SuppName = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _LSODetailModel.SO_Bill_Add_Id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            _LSODetailModel.SO_Shipp_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                            string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _LSODetailModel.SO_CurrencyID = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"]);
                            List<Currency> Currency1 = new List<Currency>();
                            Currency1.Add(new Currency { curr_id = _LSODetailModel.SO_CurrencyID.ToString(), curr_val = Curr_name });
                            _LSODetailModel.CurrencyList = Currency1;
                            _LSODetailModel.SO_ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _LSODetailModel.SO_GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _LSODetailModel.SO_AssessValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _LSODetailModel.SO_DiscountValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["disc_amt"]).ToString(ValDigit);
                            _LSODetailModel.SO_TaxValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _LSODetailModel.SO_OtherCharge = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _LSODetailModel.SO_NetOrderValue_InSep = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_spec"]).ToString(ValDigit);
                            _LSODetailModel.SO_NetOrderValue_InBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _LSODetailModel.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _LSODetailModel.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _LSODetailModel.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _LSODetailModel.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                            _LSODetailModel.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _LSODetailModel.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                            _LSODetailModel.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _LSODetailModel.StatusName = ds.Tables[0].Rows[0]["OrderStauts"].ToString();
                            _LSODetailModel.SOOrderStatus = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                            _LSODetailModel.ForAmmendendBtn = ds.Tables[13].Rows[0]["flag"].ToString();
                            _LSODetailModel.Amendment = ds.Tables[14].Rows[0]["Amendment"].ToString();
                            _LSODetailModel.bcurrflag = ds.Tables[0].Rows[0]["bcurrflag"].ToString();
                            _LSODetailModel.req_area = ds.Tables[0].Rows[0]["req_area"].ToString();
                            _LSODetailModel.OrderDiscountInPercentage = Convert.ToDecimal(ds.Tables[0].Rows[0]["ord_disc_perc"]).ToString("0.00");
                            _LSODetailModel.OrderDiscountInAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["ord_disc_amt"]).ToString(ValDigit);
                            _LSODetailModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _LSODetailModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                            //ViewBag.ItemDelSchdetails = ds.Tables[4];
                            if (ds.Tables[4].Rows.Count > 0)/*add by Hina sharma on 16-06-2025 for auot generate sales order by Sales quotation*/
                            {
                                ViewBag.ItemDelSchdetails = ds.Tables[4];
                            }
                            else
                            {
                                ViewBag.ItemDelSchdetails = null;
                            }
                            ViewBag.ItemTermsdetails = ds.Tables[5];
                            // = ds.Tables[0].Rows[0]["createid"].ToString();   //

                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.ItemTaxDetailsList = ds.Tables[9];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.AttechmentDetails = ds.Tables[10];
                            ViewBag.OCTaxDetails = ds.Tables[11];
                            ViewBag.SubItemDetails = ds.Tables[12];
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                        ViewBag.Approve_id = approval_id;
                        if (Statuscode == "C")
                        {
                            _LSODetailModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _LSODetailModel.SOCancelled = true;
                        }
                        else
                        {
                            _LSODetailModel.SOCancelled = false;
                        }
                        string ForceClose = ds.Tables[0].Rows[0]["force_close"].ToString().Trim();
                        if (ForceClose == "Y")
                        {
                            _LSODetailModel.SOForceClosed = true;
                        }
                        else
                        {
                            _LSODetailModel.SOForceClosed = false;
                        }
                        _LSODetailModel.hdnAutoPR = ds.Tables[0].Rows[0]["raise_pr"].ToString().Trim();
                        if (_LSODetailModel.hdnAutoPR == "Y")
                        {
                            _LSODetailModel.AutoPR = true;
                        }
                        else
                        {
                            _LSODetailModel.AutoPR = false;
                        }
                        //Session["DocumentStatus"] = Statuscode;
                        _LSODetailModel.DocumentStatus = Statuscode;
                        ViewBag.DocumentStatus = Statuscode;

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _LSODetailModel.Command != "Edit")
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
                                if (_LSODetailModel.Create_id != UserID)
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _LSODetailModel.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (_LSODetailModel.Create_id == UserID)
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
                                        _LSODetailModel.BtnName = "BtnToDetailPage";
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
                                        _LSODetailModel.BtnName = "BtnToDetailPage";
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
                                    _LSODetailModel.BtnName = "BtnToDetailPage";
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
                                        _LSODetailModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (Statuscode == "F")
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
                                    _LSODetailModel.BtnName = "BtnToDetailPage";
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
                                    _LSODetailModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (_LSODetailModel.Create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _LSODetailModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _LSODetailModel.BtnName = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel != null)
                        {
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                        }
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _LSODetailModel.Title = title;
                    _LSODetailModel.DocumentMenuId = DocumentMenuId;
                    //ViewBag.VBRoleList = GetRoleList();

                    ViewBag.DocumentMenuId = DocumentMenuId;
                    //ViewBag.FinstDt = Session["FinStDt"].ToString();
                    ViewBag.FinstDt = _LSODetailModel.FinStDt;
                    ViewBag.DocumentStatus = _LSODetailModel.DocumentStatus;
                    _LSODetailModel.UserId = Convert.ToInt32(UserID);
                    if (_LSODetailModel.Amend == "Amend")
                    {
                        _LSODetailModel.SOOrderStatus = "D";
                        _LSODetailModel.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                    }
                    if (_LSODetailModel.Amendment != "Amendment" && _LSODetailModel.Amendment != "" && _LSODetailModel.Amendment != null)
                    {
                        _LSODetailModel.BtnName = "BtnRefresh";
                        _LSODetailModel.wfDisableAmnd = "wfDisableAmnd";
                    }
                 
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/LSODetail.cshtml", _LSODetailModel);

                }
                else
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
                    LSODetailModel _LSODetailModel1 = new LSODetailModel();
                    if (_urlModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = _urlModel.DocumentMenuId;
                        _LSODetailModel1.MenuDocumentId = _urlModel.DocumentMenuId;
                        _LSODetailModel1.DocumentMenuId = _urlModel.DocumentMenuId;
                    }
                    else
                    {
                        DocumentMenuId = _LSODetailModel1.DocumentMenuId;
                        _LSODetailModel1.MenuDocumentId = _urlModel.DocumentMenuId;
                        _LSODetailModel1.DocumentMenuId = _urlModel.DocumentMenuId;
                    }
                    if (_urlModel.CustType != null)
                    {
                        var CustType = _urlModel.CustType;
                        _LSODetailModel1.CustType = _urlModel.CustType;
                    }
                    else
                    {
                        var CustType = _LSODetailModel1.CustType;
                        _LSODetailModel1.CustType = _LSODetailModel1.CustType;
                    }
                    if (_urlModel.BtnName != null)
                    {
                        _LSODetailModel1.BtnName = _urlModel.BtnName;
                    }
                    if (_urlModel.TransType != null)
                    {
                        _LSODetailModel1.TransType = _urlModel.TransType;
                    }
                    if (_urlModel.Command != null)
                    {
                        _LSODetailModel1.Command = _urlModel.Command;
                    }
                    if (_urlModel.DocumentStatus != null)
                    {
                        _LSODetailModel1.DocumentStatus = _urlModel.DocumentStatus;
                    }
                    _LSODetailModel1.ILSearch = null;
                    _LSODetailModel1.FC = _urlModel.FC;
                  //  BindReplicateWithlist(_LSODetailModel1);
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

                    ViewBag.DocID = DocumentMenuId;
                    string Comp_ID = string.Empty;
                    CommonPageDetails();
                    List<CustName> _CustList = new List<CustName>();

                    _CustList.Insert(0, new CustName() { Cust_id = "0", Cust_name = "---Select---" });
                    _LSODetailModel1.CustNameList = _CustList;

                    //List<Currency> Currency = new List<Currency>();
                    //Currency.Insert(0, new Currency() { curr_id = "0", curr_val = "---Select---" });
                    //_LSODetailModel1.CurrencyList = Currency;

                    //DataTable dtcurr = GetCurrList();
                    //List<Currency> Currency = new List<Currency>();
                    //foreach (DataRow dr in dtcurr.Rows)
                    //{
                    //    Currency _curr = new Currency();
                    //    _curr.curr_id = dr["curr_id"].ToString();
                    //    _curr.curr_val = dr["curr_name"].ToString();
                    //    Currency.Add(_curr);
                    //}
                    //Currency.Insert(0, new Currency() { curr_id = "0", curr_val = "---Select---" });
                    //_LSODetailModel1.CurrencyList = Currency;
                    GetAllData(_LSODetailModel1);

                    List<QuotationList> quotations = new List<QuotationList>();
                    quotations.Insert(0, new QuotationList() { SrcDocNo = "0", SrcDocNoVal = "---Select---" });
                    _LSODetailModel1.quotationsList = quotations;

                    List<SourceDocNo> sourceDocs = new List<SourceDocNo>();
                    sourceDocs.Insert(0, new SourceDocNo() { Doc_id = "0", Dic_val = "---Select---" });
                    _LSODetailModel1.SourceDocList = sourceDocs;

                    List<SO_CountryList> _CountryLists = new List<SO_CountryList>();
                    _CountryLists.Insert(0, new SO_CountryList() { Cntry_id = "0", Cntry_val = "---Select---" });
                    _LSODetailModel1._CountryLists = _CountryLists;

                    List<trade_termList> _TermLists = new List<trade_termList>();
                    //_TermLists.Insert(0, new trade_termList() { TrdTrms_id = "0", TrdTrms_val = "---Select---" });//commented by Suraj on 23-10-2023
                    _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });//Added by Suraj on 23-10-2023
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                    _LSODetailModel1.TradeTermsList = _TermLists;

                    //GetSalesPersonList(_LSODetailModel);
                    //DataTable dtr = GetSalesperson(_LSODetailModel1);
                    //List<SalePerson> SalePerson = new List<SalePerson>();
                    //foreach (DataRow dr in dtr.Rows)
                    //{
                    //    SalePerson _SalePerson = new SalePerson();
                    //    //_SalePerson.salep_id = dr["setup_id"].ToString();
                    //    //_SalePerson.salep_name = dr["setup_val"].ToString();
                    //    _SalePerson.salep_id = dr["sls_pers_id"].ToString();
                    //    _SalePerson.salep_name = dr["sls_pers_name"].ToString();
                    //    SalePerson.Add(_SalePerson);
                    //}
                    //SalePerson.Insert(0, new SalePerson() { salep_id = "0", salep_name = "---Select---" });
                    //_LSODetailModel1.SalePersonList = SalePerson;
                    //List<SalePerson> SalePerson = new List<SalePerson>();
                    //SalePerson.Insert(0, new SalePerson() { salep_id = "0", salep_name = "---Select---" });
                    //SalePerson.Insert(1, new SalePerson() { salep_id = "1", salep_name = "Sale Person 1" });
                    //_LSODetailModel1.SalePersonList = SalePerson;

                    //DataTable Gstdt = _Common_IServices.GetGSTApplicable(CompID, Br_ID, "101").Tables[0];
                    //ViewBag.GstApplicable = Gstdt.Rows.Count > 0 ? Gstdt.Rows[0]["param_stat"].ToString() : ""; /*this is used in MasterLayout*/

                    string ValDigit = "";
                    string QtyDigit = "";
                    string RateDigit = "";
                    if (_LSODetailModel1.MenuDocumentId == "105103145110")
                    {
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"].ToString()));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"].ToString()));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpRateDigit"].ToString()));
                    }
                    else
                    {
                        ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                        QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                        RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    }
                    _LSODetailModel1.ValDigit = ValDigit;
                    _LSODetailModel1.QtyDigit = QtyDigit;
                    _LSODetailModel1.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    _LSODetailModel1.ApplyTax = "E";

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _LSODetailModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    _LSODetailModel1.SO_Number = _urlModel.SO_Number;
                    _LSODetailModel1.SO_Date = _urlModel.SO_Date;
                    //if (Session["SO_No"] != null && Session["SO_Date"] != null)
                    if (_LSODetailModel1.SO_Number != null && _LSODetailModel1.SO_Date != null)
                    {
                        //string Doc_no = Session["SO_No"].ToString();                     

                        string Doc_no = _LSODetailModel1.SO_Number;
                        string Doc_date = _LSODetailModel1.SO_Date;
                        DataSet ds = GetSODetailEdit(Doc_no, Doc_date);
                      

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //_Model.hdnfromDt = ds.Tables[10].Rows[0]["findate"].ToString();
                            _LSODetailModel1.SO_OrderType = ds.Tables[0].Rows[0]["order_type"].ToString();
                            _LSODetailModel1.ApplyTax = ds.Tables[0].Rows[0]["apply_tax"].ToString();
                            _LSODetailModel1.SO_SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();
                            _LSODetailModel1.SO_no = Doc_no;// ds.Tables[0].Rows[0]["po_no"].ToString();
                            _LSODetailModel1.SO_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["so_dt"].ToString()).ToString("yyyy-MM-dd");
                            _LSODetailModel1.SO_Remarks = ds.Tables[0].Rows[0]["so_rem"].ToString();
                            _LSODetailModel1.SO_AvlCreditLimit = ds.Tables[0].Rows[0]["cre_limit"].ToString();
                            _LSODetailModel1.SO_RefDocNo = ds.Tables[0].Rows[0]["ref_doc_no"].ToString();
                            //_LSODetailModel1.SO_SourceDocDate = ds.Tables[0].Rows[0]["src_doc_date"].ToString();
                            _LSODetailModel1.SO_CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();
                            _LSODetailModel1.SO_CustID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                            _LSODetailModel1.cust_alias = ds.Tables[0].Rows[0]["cust_alias"].ToString();
                            _LSODetailModel1.SpanCustPricePolicy = ds.Tables[0].Rows[0]["cust_pr_pol"].ToString();
                            _LSODetailModel1.SpanCustPriceGroup = ds.Tables[0].Rows[0]["cust_pr_grp"].ToString();
                            _LSODetailModel1.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                            _LSODetailModel1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _CustList.Add(new CustName { Cust_id = _LSODetailModel1.SO_CustID, Cust_name = _LSODetailModel1.SO_CustName });
                            _LSODetailModel1.CustNameList = _CustList;

                            /*start Code add by Hina on 12-06-2025 for single quotation*/
                            _LSODetailModel1.SO_SourceDocNo = ds.Tables[1].Rows[0]["src_doc_number"].ToString();
                            _LSODetailModel1.SO_SourceDocDate = ds.Tables[1].Rows[0]["src_doc_date"].ToString();

                            List<QuotationList> quotations1 = new List<QuotationList>();
                            quotations1.Add(new QuotationList { SrcDocNo = _LSODetailModel1.SO_SourceDocNo, SrcDocNoVal = _LSODetailModel1.SO_SourceDocNo });
                            _LSODetailModel1.quotationsList = quotations1;

                            /*end Code add by Hina on 12-06-2025 for single quotation*/
                            //string sls_pers_name = ds.Tables[0].Rows[0]["sls_pers_name"].ToString();
                            _LSODetailModel1.SO_SalePerson = ds.Tables[0].Rows[0]["emp_id"].ToString();
                            //SalePerson.Add(new SalePerson { salep_id = _LSODetailModel1.SO_SalePerson, salep_name = sls_pers_name });
                            //_LSODetailModel1.SalePersonList = SalePerson;
                            string CntryOfDest = ds.Tables[0].Rows[0]["country_name"].ToString();
                            if (CntryOfDest == "") { CntryOfDest = "---Select---"; }
                            _LSODetailModel1.SO_Country = ds.Tables[0].Rows[0]["cntry_dest"].ToString();
                            _CountryLists.Add(new SO_CountryList { Cntry_id = _LSODetailModel1.SO_Country, Cntry_val = CntryOfDest });
                            _LSODetailModel1._CountryLists = _CountryLists;
                            _LSODetailModel1.SO_PortOfDest = ds.Tables[0].Rows[0]["port_dest"].ToString();
                            _LSODetailModel1.SO_ExportFileNo = ds.Tables[0].Rows[0]["exp_file_no"].ToString();
                            _LSODetailModel1.SO_RefDocNo = ds.Tables[0].Rows[0]["ref_doc_no"].ToString();
                            _LSODetailModel1.trade_term = ds.Tables[0].Rows[0]["trade_terms"].ToString();
                            _LSODetailModel1.SO_BillingAddress = ds.Tables[0].Rows[0]["bill_address"].ToString();
                            _LSODetailModel1.SO_ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                            //_LSODetailModel1.SuppName = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            _LSODetailModel1.SO_Bill_Add_Id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            _LSODetailModel1.SO_Shipp_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                            string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                            _LSODetailModel1.SO_CurrencyID = Convert.ToInt32(ds.Tables[0].Rows[0]["curr_id"]);
                            List<Currency> Currency1 = new List<Currency>();
                            Currency1.Add(new Currency { curr_id = _LSODetailModel1.SO_CurrencyID.ToString(), curr_val = Curr_name });
                            _LSODetailModel1.CurrencyList = Currency1;
                            _LSODetailModel1.SO_ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _LSODetailModel1.SO_GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _LSODetailModel1.SO_AssessValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _LSODetailModel1.SO_DiscountValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["disc_amt"]).ToString(ValDigit);
                            _LSODetailModel1.SO_TaxValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _LSODetailModel1.SO_OtherCharge = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _LSODetailModel1.SO_NetOrderValue_InSep = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_spec"]).ToString(ValDigit);
                            _LSODetailModel1.SO_NetOrderValue_InBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _LSODetailModel1.Create_by = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _LSODetailModel1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _LSODetailModel1.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _LSODetailModel1.Amended_by = ds.Tables[0].Rows[0]["mod_id"].ToString();
                            _LSODetailModel1.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _LSODetailModel1.Approved_by = ds.Tables[0].Rows[0]["app_id"].ToString();
                            _LSODetailModel1.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _LSODetailModel1.StatusName = ds.Tables[0].Rows[0]["OrderStauts"].ToString();
                            _LSODetailModel1.SOOrderStatus = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                            _LSODetailModel1.ForAmmendendBtn = ds.Tables[13].Rows[0]["flag"].ToString();
                            _LSODetailModel1.Amendment = ds.Tables[14].Rows[0]["Amendment"].ToString();
                            _LSODetailModel1.bcurrflag = ds.Tables[0].Rows[0]["bcurrflag"].ToString();
                            _LSODetailModel1.OrderDiscountInPercentage = Convert.ToDecimal(ds.Tables[0].Rows[0]["ord_disc_perc"]).ToString("0.00");
                            _LSODetailModel1.OrderDiscountInAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["ord_disc_amt"]).ToString(ValDigit);
                            _LSODetailModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                            _LSODetailModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                            _LSODetailModel1.req_area = ds.Tables[0].Rows[0]["req_area"].ToString();
                            //ViewBag.ItemDelSchdetails = ds.Tables[4];
                            ViewBag.ItemTermsdetails = ds.Tables[5];
                            // = ds.Tables[0].Rows[0]["createid"].ToString();   //
                            if (ds.Tables[4].Rows.Count > 0)/*add by Hina sharma on 16-06-2025 for auot generate sales order by Sales quotation*/
                            {
                                ViewBag.ItemDelSchdetails = ds.Tables[4];
                            }
                            else
                            {
                                ViewBag.ItemDelSchdetails = null;
                            }
                            ViewBag.ItemDetailsList = ds.Tables[1];
                            ViewBag.ItemTaxDetails = ds.Tables[2];
                            ViewBag.ItemTaxDetailsList = ds.Tables[9];
                            ViewBag.OtherChargeDetails = ds.Tables[3];
                            ViewBag.AttechmentDetails = ds.Tables[10];
                            ViewBag.OCTaxDetails = ds.Tables[11];
                            ViewBag.SubItemDetails = ds.Tables[12];
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        ViewBag.Approve_id = approval_id;
                        string Statuscode = ds.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _LSODetailModel1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                            _LSODetailModel1.SOCancelled = true;
                        }
                        else
                        {
                            _LSODetailModel1.SOCancelled = false;
                        }
                        string ForceClose = ds.Tables[0].Rows[0]["force_close"].ToString().Trim();
                        if (ForceClose == "Y")
                        {
                            _LSODetailModel1.SOForceClosed = true;
                        }
                        else
                        {
                            _LSODetailModel1.SOForceClosed = false;
                        }
                        _LSODetailModel1.hdnAutoPR = ds.Tables[0].Rows[0]["raise_pr"].ToString().Trim();
                        if (_LSODetailModel1.hdnAutoPR == "Y")
                        {
                            _LSODetailModel1.AutoPR = true;
                        }
                        else
                        {
                            _LSODetailModel1.AutoPR = false;
                        }
                        //Session["DocumentStatus"] = Statuscode;
                        _LSODetailModel1.DocumentStatus = Statuscode;
                        ViewBag.DocumentStatus = Statuscode;

                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[8];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _LSODetailModel1.Command != "Edit")
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
                                if (_LSODetailModel1.Create_id != UserID)
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _LSODetailModel1.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (_LSODetailModel1.Create_id == UserID)
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
                                        _LSODetailModel1.BtnName = "BtnToDetailPage";
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
                                        _LSODetailModel1.BtnName = "BtnToDetailPage";
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
                                    _LSODetailModel1.BtnName = "BtnToDetailPage";
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
                                        _LSODetailModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (Statuscode == "F")
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
                                    _LSODetailModel1.BtnName = "BtnToDetailPage";
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
                                    _LSODetailModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (_LSODetailModel1.Create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _LSODetailModel1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _LSODetailModel1.BtnName = "BtnRefresh";
                                }
                            }
                            if (Statuscode == "PN" || Statuscode == "IN")
                            {
                                _LSODetailModel1.BtnName = "BtnToDetailPage";
                            }
                        }
                        if (ViewBag.AppLevel != null)
                        {
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                        }
                    }
                    //ViewBag.MenuPageName = getDocumentName();
                    _LSODetailModel1.Title = title;
                    _LSODetailModel1.DocumentMenuId = DocumentMenuId;
                    //ViewBag.VBRoleList = GetRoleList();
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    //ViewBag.FinstDt = Session["FinStDt"].ToString();
                    ViewBag.FinstDt = _LSODetailModel1.FinStDt;
                    _LSODetailModel1.ILSearch = null;
                    ViewBag.DocumentStatus = _LSODetailModel1.DocumentStatus;
                    _LSODetailModel1.UserId = Convert.ToInt32(UserID);
                    if (_urlModel.Amend == "Amend")
                    {
                        _LSODetailModel1.Amend = _urlModel.Amend;
                        _LSODetailModel1.SOOrderStatus = "D";
                        _LSODetailModel1.DocumentStatus = "D";
                        _LSODetailModel1.DocumentStatus = "D";
                    }
                    if (_LSODetailModel1.Amendment != "Amendment" && _LSODetailModel1.Amendment != "" && _LSODetailModel1.Amendment != null)
                    {
                        _LSODetailModel1.BtnName = "BtnRefresh";
                        _LSODetailModel1.wfDisableAmnd = "wfDisableAmnd";
                    }
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/LSODetail.cshtml", _LSODetailModel1);
                }
            }
            //Session["ILSearch"] = null;
            catch (Exception ex)
            {
                //throw ex;
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private void GetAllData(LSODetailModel _LSODetailModel1)
        {
            #region Added By Nitesh on 06-04-2024 For All DropDown and Detail Data is One procedure
            #endregion
            string SalesPersonName = string.Empty;
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
            if (_LSODetailModel1.SO_SalePerson == null)
            {
                SalesPersonName = "0";
            }
            else
            {
                SalesPersonName = _LSODetailModel1.SO_SalePerson;
            }
            var orderTYpe = "D";
            if (_LSODetailModel1.DocumentMenuId == "105103145110")
            {
                orderTYpe = "E";
            }
            DataSet GetAllData = new DataSet();
            if (orderTYpe == "E")
            {
                crm = "Y";
                GetAllData = _LSODetail_ISERVICE.GetAllData(CompID, SalesPersonName, Br_ID, "1001", _LSODetailModel1.SO_Number, _LSODetailModel1.SO_Date);
            }
            else
            {
                GetAllData = _LSODetail_ISERVICE.GetAllData(CompID, SalesPersonName, Br_ID, UserID, _LSODetailModel1.SO_Number, _LSODetailModel1.SO_Date);
            }

          
            List<Currency> Currency = new List<Currency>();
            foreach (DataRow dr in GetAllData.Tables[0].Rows)
            {
                Currency _curr = new Currency();
                _curr.curr_id = dr["curr_id"].ToString();
                _curr.curr_val = dr["curr_name"].ToString();
                Currency.Add(_curr);
            }
            Currency.Insert(0, new Currency() { curr_id = "0", curr_val = "---Select---" });
            _LSODetailModel1.CurrencyList = Currency;
            
            List<SalePerson> SalePerson = new List<SalePerson>();
            if ((rpt_id == "0" || _LSODetailModel1.TransType=="Update"|| orderTYpe == "E"|| UserID=="1001") && crm=="Y")
            {
                foreach (DataRow dr in GetAllData.Tables[1].Rows)
                {
                    SalePerson _SalePerson = new SalePerson();
                    _SalePerson.salep_id = dr["sls_pers_id"].ToString();
                    _SalePerson.salep_name = dr["sls_pers_name"].ToString();
                    SalePerson.Add(_SalePerson);
                }
                SalePerson.Insert(0, new SalePerson() { salep_id = "0", salep_name = "---Select---" });
            }
            else
            {
                SalePerson.Insert(0, new SalePerson() { salep_id = UserID, salep_name = UserName });
            }

            _LSODetailModel1.SalePersonList = SalePerson;

            List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();

            foreach (DataRow dr in GetAllData.Tables[2].Rows)
            {
                RequirementAreaList _RAList = new RequirementAreaList();
                _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                _RAList.req_val = dr["setup_val"].ToString();
                requirementAreaLists.Add(_RAList);
            }
            requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "---Select---" });
            _LSODetailModel1._requirementAreaLists = requirementAreaLists;
            _LSODetailModel1.hdnAutoPR = "N"; ;
        }
        public ActionResult BindReplicateWithlist(LSODetailModel _LSODetailModel)
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
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (_LSODetailModel.item == null)
                    {
                        SarchValue = "0";
                    }
                    else
                    {
                        SarchValue = _LSODetailModel.item;
                    }
                    DataSet ProductList = _LSODetail_ISERVICE.getReplicateWith(CompID, Br_ID, _LSODetailModel.SO_OrderType, SarchValue);
                    if (ProductList.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ProductList.Tables[0].Rows.Count; i++)
                        {
                            string so_no = ProductList.Tables[0].Rows[i]["so_no"].ToString();
                            string so_dt = ProductList.Tables[0].Rows[i]["so_dt"].ToString();
                            string cust_name = ProductList.Tables[0].Rows[i]["cust_name"].ToString();
                            ItemList.Add(so_no + "," + so_dt, cust_name);
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
        public JsonResult GetReplicateWithSoNumber(string so_no, string so_dt,string ReplicateType, string cust_id, string order_dt)
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
                DataSet result = _LSODetail_ISERVICE.GetReplicateWithItemdata(CompID, Br_ID, so_no, so_dt, ReplicateType, cust_id, order_dt);
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
        private DataTable GetSalesperson(LSODetailModel _LSODetailModel)
        {
            try
            {
                string SalesPersonName = "";
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (_LSODetailModel.SO_SalePerson == null)
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = _LSODetailModel.SO_SalePerson;
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                //SalesPersonName = _LSODetailModel.SO_SalePerson;
                DataTable dt = _LSODetail_ISERVICE.GetSalesPersonList(Comp_ID, SalesPersonName, Br_ID, UserID);
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        private DataTable GetCurrList()
        {
            try
            {
                DataTable dt = _LSODetail_ISERVICE.GetCurrList();
                return dt;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult DblClickLSODetail(string So_no, string So_dt, string ListFilterData, string DocumentMenuId, string WF_status, string CustType)
        {
            try
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
                //Session["Message"] = "New";
                //Session["Command"] = "Refresh";
                //Session["AppStatus"] = 'D';
                //Session["TransType"] = "Save";
                //Session["SO_No"] = So_no;
                //Session["SO_Date"] = So_dt;
                //Session["BtnName"] = "BtnToDetailPage";
                LSODetailModel _Doubleclick = new LSODetailModel();
                UrlModel _urlModel = new UrlModel();
                _Doubleclick.Message = "New";
                _Doubleclick.Command = "Add";
                _Doubleclick.AppStatus = "D";
                _Doubleclick.TransType = "Update";
                _Doubleclick.BtnName = "BtnToDetailPage";
                _Doubleclick.SO_Number = So_no;
                _Doubleclick.SO_Date = So_dt;
                _Doubleclick.DocumentMenuId = DocumentMenuId;
                _Doubleclick.CustType = CustType;
                if (WF_status != null && WF_status != "")
                {
                    _Doubleclick.WF_status1 = WF_status;
                    _urlModel.WF_status1 = WF_status;
                }
                TempData["ModelData"] = _Doubleclick;
                _urlModel.TransType = "Update";
                _urlModel.BtnName = "BtnToDetailPage";
                _urlModel.SO_Number = So_no;
                _urlModel.SO_Date = So_dt;
                _urlModel.DocumentMenuId = DocumentMenuId;
                _urlModel.CustType = CustType;
                TempData["ListFilterData"] = ListFilterData;
                return RedirectToAction("LSODetail", "LSODetail", _urlModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult SODetailsActions(LSODetailModel _Model, string Command)
        {

            try
            { /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                //PODetailsModel _Model = new PODetailsModel();
                if (_Model.Delete == "Delete")
                {
                    Command = "Delete";
                    // DeletePQDetails(_Model);
                }
                string msgtype = string.Empty;
                switch (Command)
                {
                    case "AddNew":
                        UrlModel URLModel = new UrlModel();
                        LSODetailModel _LSODetailModeladd = new LSODetailModel();
                        _LSODetailModeladd.Message = "New";
                        _LSODetailModeladd.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                        //ViewBag.DocumentStatus = "D";
                        _LSODetailModeladd.BtnName = "BtnAddNew";
                        _LSODetailModeladd.TransType = "Save";
                        _LSODetailModeladd.Command = "New";
                        _LSODetailModeladd.DocumentMenuId = _Model.DocumentMenuId;
                        _LSODetailModeladd.CustType = _Model.CustType;
                        TempData["ModelData"] = _LSODetailModeladd;
                        URLModel.DocumentMenuId = _Model.DocumentMenuId;
                        URLModel.TransType = "Save";
                        URLModel.BtnName = "BtnAddNew";
                        URLModel.Command = "New";
                        URLModel.CustType = _Model.CustType;
                        URLModel.DocumentStatus = _LSODetailModeladd.DocumentStatus;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_Model.SO_no))
                                return RedirectToAction("DblClickLSODetail", new { So_no = _Model.SO_no, So_dt=_Model.SO_dt, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus, CustType = _Model.CustType });
                            else
                                _LSODetailModeladd.Command = "Refresh";
                            _LSODetailModeladd.TransType = "Refresh";
                            _LSODetailModeladd.BtnName = "BtnRefresh";
                            _LSODetailModeladd.DocumentStatus = null;
                            TempData["ModelData"] = _LSODetailModeladd;
                            return RedirectToAction("LSODetail");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("LSODetail", URLModel);
                    case "Save":
                        _Model.TransType = Command;
                        msgtype = InsertSODetails(_Model);
                        if (_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (msgtype == "ErrorPage")
                        {
                            return RedirectToAction("ErrorPage");
                        }
                        if (_Model.Message == "DocModify")
                        {
                            DocumentMenuId = _Model.DocumentMenuId;
                            _Model.DocumentMenuId = DocumentMenuId;
                            CommonPageDetails();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";
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

                            List<CustName> _CustList = new List<CustName>();

                            _CustList.Insert(0, new CustName() { Cust_id = _Model.SO_CustID, Cust_name = _Model.SO_CustName });
                            _Model.CustNameList = _CustList;

                            List<Currency> Currency = new List<Currency>();
                            Currency.Insert(0, new Currency() { curr_id = "0", curr_val = _Model.SO_Currency });
                            _Model.CurrencyList = Currency;

                            List<QuotationList> quotations = new List<QuotationList>();
                            quotations.Insert(0, new QuotationList() { SrcDocNo = "0", SrcDocNoVal = "---Select---" });
                            _Model.quotationsList = quotations;

                            List<SourceDocNo> sourceDocs = new List<SourceDocNo>();
                            sourceDocs.Insert(0, new SourceDocNo() { Doc_id = "0", Dic_val = "---Select---" });
                            _Model.SourceDocList = sourceDocs;

                            List<SO_CountryList> _CountryLists = new List<SO_CountryList>();
                            _CountryLists.Insert(0, new SO_CountryList() { Cntry_id = "0", Cntry_val = "---Select---" });
                            _Model._CountryLists = _CountryLists;

                            List<trade_termList> _TermLists = new List<trade_termList>();
                            _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                            _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });

                            _Model.TradeTermsList = _TermLists;

                            GetAllData(_Model);
                            //DataTable dtr = GetSalesperson(_Model);
                            //List<SalePerson> SalePerson = new List<SalePerson>();
                            //foreach (DataRow dr in dtr.Rows)
                            //{
                            //    SalePerson _SalePerson = new SalePerson();
                            //    _SalePerson.salep_id = dr["sls_pers_id"].ToString();
                            //    _SalePerson.salep_name = dr["sls_pers_name"].ToString();
                            //    SalePerson.Add(_SalePerson);
                            //}
                            //SalePerson.Insert(0, new SalePerson() { salep_id = "0", salep_name = _Model.SalesprsnName });
                            //_Model.SalePersonList = SalePerson;

                            _Model.SO_Date = DateTime.Now.ToString();
                            _Model.SO_SourceType = _Model.SO_SourceType;
                            _Model.SO_Currency = _Model.SO_Currency;
                            _Model.SalesprsnName = _Model.SalesprsnName;

                            //_Model.bill_date = _Model.bill_date;
                            _Model.SO_BillingAddress = _Model.SO_BillingAddress;
                            _Model.SO_ShippingAddress = _Model.SO_ShippingAddress;

                            ViewBag.ItemDetailsList = ViewData["ItemDetails"];
                            ViewBag.ItemTaxDetails = ViewData["Tax_Details"];
                            ViewBag.ItemTaxDetailsList = ViewData["Tax_Details"];
                            ViewBag.OCTaxDetails = ViewData["OCTaxDetails"];
                            //ViewBag.ItemTaxDetailsList = ViewData["TaxDetails"];
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemDelSchdetails = ViewData["DelvScheDetails"];
                            ViewBag.ItemTermsdetails = ViewData["TrmAndConDetails"];

                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _Model.BtnName = "BtnRefresh";
                            _Model.Command = "Refresh";
                            _Model.DocumentStatus = "D";
                            //_Model.SOOrderStatus = "D";


                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            _Model.QtyDigit = QtyDigit;
                            _Model.ValDigit = ValDigit;
                            _Model.RateDigit = RateDigit;
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/LSODetail.cshtml", _Model);

                        }
                        else
                        {
                            _Model.TransType = "Update";
                            _Model.Command = Command;
                            _Model.BtnName = "BtnSave";
                            _Model.SO_Number = _Model.SO_Number;
                            _Model.SO_Date = _Model.SO_Date;
                            _Model.DocumentMenuId = _Model.DocumentMenuId;
                            _Model.CustType = _Model.CustType;
                            _Model.PR_NO = _Model.PR_NO;
                            _Model.SO_SalePerson = null;
                            TempData["ModelData"] = _Model;
                            UrlModel URLSaveModel = new UrlModel();
                            URLSaveModel.TransType = _Model.TransType;
                            URLSaveModel.Command = Command;
                            URLSaveModel.BtnName = "BtnSave";
                            // _Model.SO_SalePerson = null;
                            URLSaveModel.SO_Number = _Model.SO_Number;
                            URLSaveModel.SO_Date = _Model.SO_Date;
                            URLSaveModel.TransType = "Update";
                            URLSaveModel.DocumentMenuId = _Model.DocumentMenuId;
                            URLSaveModel.CustType = _Model.CustType;
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            return RedirectToAction("LSODetail", URLSaveModel);
                        }
                    case "Update":
                        _Model.TransType = Command;
                        if (_Model.Amend != null && _Model.Amend != "" && _Model.Amendment == null)
                        {
                            _Model.TransType = "Amendment";
                        }
                        msgtype = InsertSODetails(_Model);
                        if (msgtype == "ErrorPage")
                        {
                            return RedirectToAction("ErrorPage");
                        }
                        else
                        {
                            _Model.TransType = "Update";
                            _Model.Command = Command;
                            _Model.BtnName = "BtnSave";
                            _Model.SO_Number = _Model.SO_Number;
                            _Model.DocumentMenuId = _Model.DocumentMenuId;
                            _Model.CustType = _Model.CustType;
                            _Model.SO_Date = _Model.SO_Date;
                            _Model.SO_SalePerson = null;
                            UrlModel URLupdateModel = new UrlModel();
                            URLupdateModel.TransType = _Model.TransType;
                            URLupdateModel.Command = Command;
                            URLupdateModel.SO_SalePerson = null;
                            URLupdateModel.BtnName = "BtnSave";
                            URLupdateModel.SO_Number = _Model.SO_Number;
                            URLupdateModel.SO_Date = _Model.SO_Date;
                            URLupdateModel.DocumentMenuId = _Model.DocumentMenuId;
                            //    Session["TransType"] = "Update";
                            //Session["Command"] = Command;
                            //Session["BtnName"] = "BtnSave";
                            if (_Model.SOOrderStatus == "PDL" || _Model.SOOrderStatus == "PR" || _Model.SOOrderStatus == "PN")
                            {
                                //Session["BtnName"] = "BtnToDetailPage";
                                URLupdateModel.BtnName = "BtnToDetailPage";
                                _Model.BtnName = "BtnToDetailPage";
                                _Model.DocumentMenuId = _Model.DocumentMenuId;
                                URLupdateModel.DocumentMenuId = _Model.DocumentMenuId;
                            }
                            TempData["ModelData"] = _Model;
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            return RedirectToAction("LSODetail", URLupdateModel);
                        }

                    case "Approve":
                        /*start Add by Hina on 08-05-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DblClickLSODetail", new { So_no = _Model.SO_no, So_dt = _Model.SO_dt, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus, CustType = _Model.CustType });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        //string SODate = _Model.SO_dt;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SODate) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DblClickLSODetail", new { So_no = _Model.SO_no, So_dt = _Model.SO_dt, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus, CustType = _Model.CustType });
                        //}
                        /*End to chk Financial year exist or not*/
                        //_Model.TransType = Command;
                        InsertLSOApproveDetails(_Model, _Model.SO_no, _Model.SO_dt, "", "", "", "", "", "", "");
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "BtnApprove";      

                        UrlModel URLModelApprove = new UrlModel();
                        URLModelApprove.Command = _Model.Command;
                        URLModelApprove.TransType = _Model.TransType;
                        URLModelApprove.SO_Number = _Model.SO_Number;
                        URLModelApprove.SO_Date = _Model.SO_Date;
                        URLModelApprove.DocumentMenuId = _Model.DocumentMenuId;
                        URLModelApprove.CustType = _Model.CustType;
                        if (_Model.WF_status1 != null && _Model.WF_status1 != "")
                        {
                            URLModelApprove.WF_status1 = _Model.WF_status1;
                        }
                        URLModelApprove.BtnName = "BtnApprove";
                        TempData["ModelData"] = _Model;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("LSODetail", URLModelApprove);
                    case "Edit":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DblClickLSODetail", new { So_no = _Model.SO_no, So_dt = _Model.SO_dt, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus, CustType = _Model.CustType });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        //string SODate1 = _Model.SO_dt;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SODate1) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DblClickLSODetail", new { So_no = _Model.SO_no, So_dt = _Model.SO_dt, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus, CustType = _Model.CustType });
                        //}
                        /*End to chk Financial year exist or not*/
                        UrlModel URLeditModel = new UrlModel();
                        if (_Model.SOOrderStatus == "A")
                        {
                            if (CheckPackingListAgainstLSO(_Model, _Model.SO_no, _Model.SO_dt) == "Y")
                            {
                                if (_Model.Message == "UsedInPP")
                                {
                                    _Model.FC = "UsedInPP";
                                    _Model.TransType = "Update";
                                    _Model.BtnName = "BtnEdit";
                                    _Model.DocumentMenuId = _Model.DocumentMenuId;
                                    _Model.CustType = _Model.CustType;
                                    _Model.SO_Number = _Model.SO_no;
                                    _Model.SO_Date = _Model.SO_dt;
                                    _Model.Message = _Model.Message;
                                    TempData["ModelData"] = _Model;

                                    URLeditModel.TransType = _Model.TransType;
                                    URLeditModel.Command = Command;
                                    URLeditModel.FC = _Model.FC;
                                    URLeditModel.BtnName = "BtnEdit";
                                    URLeditModel.DocumentMenuId = _Model.DocumentMenuId;
                                    URLeditModel.CustType = _Model.CustType;
                                    URLeditModel.SO_Number = _Model.SO_no;
                                    URLeditModel.SO_Date = _Model.SO_dt;
                                }
                                else if(_Model.Message == "PRUnderProcess")
                                {
                                    _Model.FC = "PRUnderProcess";
                                    _Model.BtnName = "BtnToDetailPage";
                                    _Model.BtnName = "BtnToDetailPage";
                                    _Model.Command = "Refresh";
                                    _Model.TransType = "Update";
                                    _Model.SO_Number = _Model.SO_no;
                                    _Model.SO_Date = _Model.SO_dt;
                                    _Model.DocumentMenuId = _Model.DocumentMenuId;
                                    _Model.CustType = _Model.CustType;
                                    _Model.Message = _Model.Message;
                                    TempData["ModelData"] = _Model;
                                    URLeditModel.TransType = _Model.TransType;
                                    URLeditModel.Command = _Model.Command;
                                    URLeditModel.BtnName = _Model.BtnName;
                                    URLeditModel.FC = _Model.FC;
                                    URLeditModel.SO_Number = _Model.SO_Number;
                                    URLeditModel.DocumentMenuId = _Model.DocumentMenuId;
                                    URLeditModel.CustType = _Model.CustType;
                                    URLeditModel.SO_Date = _Model.SO_Date;
                                    TempData["ListFilterData"] = _Model.ListFilterData1;
                                }
                                else
                                {
                                    _Model.FC = "N";
                                    _Model.BtnName = "BtnToDetailPage";
                                    _Model.Command = "Refresh";
                                    _Model.TransType = "Update";
                                    _Model.SO_Number = _Model.SO_no;
                                    _Model.SO_Date = _Model.SO_dt;
                                    _Model.DocumentMenuId = _Model.DocumentMenuId;
                                    _Model.CustType = _Model.CustType;
                                    _Model.Message = _Model.Message;
                                    TempData["ModelData"] = _Model;
                                    URLeditModel.TransType = _Model.TransType;
                                    URLeditModel.Command = _Model.Command;
                                    URLeditModel.BtnName = _Model.BtnName;
                                    URLeditModel.FC = _Model.FC;
                                    URLeditModel.SO_Number = _Model.SO_Number;
                                    URLeditModel.DocumentMenuId = _Model.DocumentMenuId;
                                    URLeditModel.CustType = _Model.CustType;
                                    URLeditModel.SO_Date = _Model.SO_Date;
                                    TempData["ListFilterData"] = _Model.ListFilterData1;
                                }
                               
                            }
                            else
                            {
                                _Model.Message = null;
                                _Model.Command = Command;
                                _Model.TransType = "Update";
                                _Model.BtnName = "BtnEdit";
                                _Model.DocumentMenuId = _Model.DocumentMenuId;
                                _Model.CustType = _Model.CustType;
                                _Model.SO_Number = _Model.SO_no;
                                _Model.SO_Date = _Model.SO_dt;
                                TempData["ModelData"] = _Model;

                                URLeditModel.TransType = _Model.TransType;
                                URLeditModel.Command = Command;
                                URLeditModel.BtnName = "BtnEdit";
                                URLeditModel.DocumentMenuId = _Model.DocumentMenuId;
                                URLeditModel.CustType = _Model.CustType;
                                URLeditModel.SO_Number = _Model.SO_no;
                                URLeditModel.SO_Date = _Model.SO_dt;
                                TempData["ListFilterData"] = _Model.ListFilterData1;
                            }
                        }
                        else if (_Model.SOOrderStatus == "PDL" || _Model.SOOrderStatus == "PR" || _Model.SOOrderStatus == "PN")
                        {
                            if (CheckSalesOrderQtyforForceclosed(_Model, _Model.SO_no, _Model.SO_dt) == "N")
                            {
                                _Model.Message = _Model.Message;
                                _Model.Command = "Refresh";
                                _Model.TransType = "Update";
                                _Model.DocumentMenuId = _Model.DocumentMenuId;
                                _Model.CustType = _Model.CustType;
                                // _Model.BtnName = "BtnEdit";
                                _Model.BtnName = "BtnRefresh";
                                _Model.SO_Number = _Model.SO_no;
                                _Model.SO_Date = _Model.SO_dt;
                                TempData["ModelData"] = _Model;

                                URLeditModel.TransType = _Model.TransType;
                                // URLeditModel.Command = Command;
                                URLeditModel.Command = "Refresh";
                                // URLeditModel.BtnName = "BtnEdit";
                                URLeditModel.BtnName = "BtnRefresh";
                                URLeditModel.DocumentMenuId = _Model.DocumentMenuId;
                                URLeditModel.CustType = _Model.CustType;
                                URLeditModel.SO_Number = _Model.SO_no;
                                URLeditModel.SO_Date = _Model.SO_dt;
                                TempData["ListFilterData"] = _Model.ListFilterData1;
                            }
                            else
                            {
                                _Model.Message = null;
                                //_Model.Command = Command;
                                _Model.Command = Command;
                                _Model.TransType = "Update";
                                _Model.DocumentMenuId = _Model.DocumentMenuId;
                                _Model.CustType = _Model.CustType;
                                _Model.BtnName = "BtnEdit";
                                _Model.SO_Number = _Model.SO_no;
                                _Model.SO_Date = _Model.SO_dt;
                                TempData["ModelData"] = _Model;

                                URLeditModel.TransType = _Model.TransType;
                                URLeditModel.Command = Command;
                                URLeditModel.BtnName = "BtnEdit";
                                URLeditModel.DocumentMenuId = _Model.DocumentMenuId;
                                URLeditModel.CustType = _Model.CustType;
                                URLeditModel.SO_Number = _Model.SO_no;
                                URLeditModel.SO_Date = _Model.SO_dt;
                                TempData["ListFilterData"] = _Model.ListFilterData1;
                            }

                        }
                        else
                        {
                            _Model.Message = null;
                            _Model.Command = Command;
                            _Model.TransType = "Update";
                            _Model.BtnName = "BtnEdit";
                            _Model.DocumentMenuId = _Model.DocumentMenuId;
                            _Model.CustType = _Model.CustType;
                            _Model.SO_Number = _Model.SO_no;
                            _Model.SO_Date = _Model.SO_dt;
                            TempData["ModelData"] = _Model;

                            URLeditModel.TransType = _Model.TransType;
                            URLeditModel.Command = Command;
                            URLeditModel.BtnName = "BtnEdit";
                            URLeditModel.DocumentMenuId = _Model.DocumentMenuId;
                            URLeditModel.CustType = _Model.CustType;
                            URLeditModel.SO_Number = _Model.SO_Number;
                            URLeditModel.SO_Date = _Model.SO_Date;
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                        }
                        return RedirectToAction("LSODetail", URLeditModel);
                    case "Amendment":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DblClickLSODetail", new { So_no = _Model.SO_no, So_dt = _Model.SO_dt, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus, CustType = _Model.CustType });
                        //}
                        /*Above Commented and modify by Hina sharma on 08-05-2025 to check Existing with previous year transaction*/
                        //string SODate2 = _Model.SO_dt;
                        //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SODate2) == "TransNotAllow")
                        //{
                        //    TempData["Message1"] = "TransNotAllow";
                        //    return RedirectToAction("DblClickLSODetail", new { So_no = _Model.SO_no, So_dt = _Model.SO_dt, ListFilterData = _Model.ListFilterData1, DocumentMenuId = _Model.DocumentMenuId, WF_status = _Model.WFStatus, CustType = _Model.CustType });
                        //}
                        /*End to chk Financial year exist or not*/
                        UrlModel URLModelAmendment = new UrlModel();
                        if (_Model.SOOrderStatus == "A")
                        {
                            if (CheckPackingListAgainstLSO(_Model, _Model.SO_no, _Model.SO_dt) == "Y")
                            {
                               
                                 if (_Model.Message == "PRUnderProcess")
                                {
                                    _Model.FC = "PRUnderProcess";
                                    _Model.BtnName = "BtnToDetailPage";
                                    _Model.BtnName = "BtnToDetailPage";
                                    _Model.Command = "Refresh";
                                    _Model.TransType = "Update";
                                    _Model.SO_Number = _Model.SO_no;
                                    _Model.SO_Date = _Model.SO_dt;
                                    _Model.DocumentMenuId = _Model.DocumentMenuId;
                                    _Model.CustType = _Model.CustType;
                                    _Model.Message = "PRUnderProcessAmmend";
                                    TempData["ModelData"] = _Model;
                                    URLModelAmendment.TransType = _Model.TransType;
                                    URLModelAmendment.Command = _Model.Command;
                                    URLModelAmendment.BtnName = _Model.BtnName;
                                    URLModelAmendment.FC = _Model.FC;
                                    URLModelAmendment.SO_Number = _Model.SO_Number;
                                    URLModelAmendment.DocumentMenuId = _Model.DocumentMenuId;
                                    URLModelAmendment.CustType = _Model.CustType;
                                    URLModelAmendment.SO_Date = _Model.SO_Date;
                                    TempData["ListFilterData"] = _Model.ListFilterData1;
                                  
                                }
                                else
                                {
                                    _Model.Command = "Edit";
                                    _Model.TransType = "Update";
                                    _Model.BtnName = "BtnEdit";
                                    _Model.Amend = "Amend";
                                    _Model.SO_Number = _Model.SO_no;
                                    _Model.SO_Date = _Model.SO_dt;
                                    TempData["ModelData"] = _Model;
                                    URLModelAmendment.Command = "Edit";
                                    URLModelAmendment.TransType = "Update";
                                    URLModelAmendment.BtnName = "BtnEdit";
                                    URLModelAmendment.DocumentMenuId = _Model.DocumentMenuId;
                                    URLModelAmendment.SO_Date = _Model.SO_dt;
                                    URLModelAmendment.SO_Number = _Model.SO_no;
                                    URLModelAmendment.Amend = "Amend";
                                    URLModelAmendment.CustType = _Model.CustType;
                                }
                            }
                            else
                            {
                                _Model.Command = "Edit";
                                _Model.TransType = "Update";
                                _Model.BtnName = "BtnEdit";
                                _Model.Amend = "Amend";
                                _Model.SO_Number = _Model.SO_no;
                                _Model.SO_Date = _Model.SO_dt;
                                TempData["ModelData"] = _Model;
                                URLModelAmendment.Command = "Edit";
                                URLModelAmendment.TransType = "Update";
                                URLModelAmendment.BtnName = "BtnEdit";
                                URLModelAmendment.DocumentMenuId = _Model.DocumentMenuId;
                                URLModelAmendment.SO_Date = _Model.SO_dt;
                                URLModelAmendment.SO_Number = _Model.SO_no;
                                URLModelAmendment.Amend = "Amend";
                                URLModelAmendment.CustType = _Model.CustType;
                            }
                            
                        }
                        else
                        {
                            _Model.Command = "Edit";
                            _Model.TransType = "Update";
                            _Model.BtnName = "BtnEdit";
                            _Model.Amend = "Amend";
                            _Model.SO_Number = _Model.SO_no;
                            _Model.SO_Date = _Model.SO_dt;
                            TempData["ModelData"] = _Model;
                            URLModelAmendment.Command = "Edit";
                            URLModelAmendment.TransType = "Update";
                            URLModelAmendment.BtnName = "BtnEdit";
                            URLModelAmendment.DocumentMenuId = _Model.DocumentMenuId;
                            URLModelAmendment.SO_Date = _Model.SO_dt;
                            URLModelAmendment.SO_Number = _Model.SO_no;
                            URLModelAmendment.Amend = "Amend";
                            URLModelAmendment.CustType = _Model.CustType;
                        }

                     
                        
                        return RedirectToAction("LSODetail", URLModelAmendment);
                    case "Delete":
                        DeleteSODetails(_Model.SO_no, _Model.SO_dt, _Model.Title);
                        LSODetailModel _Modeldelete = new LSODetailModel();
                        _Modeldelete.Command = "Refresh";
                        _Modeldelete.Message = "Deleted";
                        _Modeldelete.TransType = "New";
                        _Modeldelete.BtnName = "BtnRefresh";
                        _Modeldelete.DocumentMenuId = _Model.DocumentMenuId;
                        _Modeldelete.CustType = _Model.CustType;
                        UrlModel URLModeldelete = new UrlModel();
                        URLModeldelete.Command = _Modeldelete.Command;
                        URLModeldelete.TransType = _Modeldelete.TransType;
                        URLModeldelete.DocumentMenuId = _Modeldelete.DocumentMenuId;
                        URLModeldelete.CustType = _Modeldelete.CustType;
                        URLModeldelete.BtnName = _Modeldelete.BtnName;
                        TempData["ModelData"] = _Modeldelete;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("LSODetail", URLModeldelete);
                    case "Print":
                        return GenratePdfFile(_Model, Command);
                    case "intimation":
                        return GenratePdfFile(_Model, Command);
                    case "Refresh":
                        LSODetailModel _ModelRefresh = new LSODetailModel();
                        _ModelRefresh.Command = Command;
                        _ModelRefresh.TransType = "New";
                        _ModelRefresh.BtnName = "BtnRefresh";
                        _ModelRefresh.DocumentMenuId = _Model.DocumentMenuId;
                        _ModelRefresh.CustType = _Model.CustType;
                        TempData["ModelData"] = _ModelRefresh;
                        UrlModel URLModelrefesh = new UrlModel();
                        URLModelrefesh.Command = _ModelRefresh.Command;
                        URLModelrefesh.TransType = _ModelRefresh.TransType;
                        URLModelrefesh.DocumentMenuId = _ModelRefresh.DocumentMenuId;
                        URLModelrefesh.CustType = _ModelRefresh.CustType;
                        URLModelrefesh.BtnName = _ModelRefresh.BtnName;
                        TempData["ListFilterData"] = _Model.ListFilterData1;
                        return RedirectToAction("LSODetail", URLModelrefesh);
                    case "BacktoList":
                        LSOListModel _LSOList_Model = new LSOListModel();
                        _LSOList_Model.WF_status = _Model.WF_status1;
                        if (_Model.DocumentMenuId == "105103145110")
                        {

                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            return RedirectToAction("ESOList", "LSOList", _LSOList_Model);
                        }
                        else
                        {
                            TempData["ListFilterData"] = _Model.ListFilterData1;
                            return RedirectToAction("LSOList", "LSOList", _LSOList_Model);
                        }
                    default: return new EmptyResult();

                }

                //return RedirectToAction("LSODetail", _Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                ViewBag.VBRoleList = ds.Tables[3];
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
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
        public ActionResult GetAutoCompleteSearchCustList(LSODetailModel _LSODetailModel)
        {
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string CustType = string.Empty;
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
                if (string.IsNullOrEmpty(_LSODetailModel.SO_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _LSODetailModel.SO_CustName;
                }
                if (_LSODetailModel.DocumentMenuId != null)
                {
                    if (_LSODetailModel.DocumentMenuId == "105103125")
                    {
                        CustType = "D";
                    }
                    if (_LSODetailModel.DocumentMenuId == "105103145110")
                    {
                        CustType = "E";
                    }
                    if (_LSODetailModel.DocumentMenuId == "105103135")//This is docid Shipment(D) customer list
                    {
                        CustType = "D";
                    }
                    else if (_LSODetailModel.DocumentMenuId == "105103145120")//docid Shipment(E)
                    {
                        CustType = "E";
                    }
                }

                CustList = _LSODetail_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType, _LSODetailModel.DocumentMenuId);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSalesPersonList(LSODetailModel _LSODetailModel)
        {
            string SalesPersonName = string.Empty;
            Dictionary<string, string> SPersonList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(_LSODetailModel.SO_SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = _LSODetailModel.SO_SalePerson;

                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataTable PARQusData = _LSODetail_ISERVICE.GetSalesPersonList(Comp_ID, SalesPersonName, Br_ID, UserID);
                DataRow dr = PARQusData.NewRow();
                dr[0] = "0";
                dr[1] = "---Select---";
                PARQusData.Rows.InsertAt(dr, 0);

                if (PARQusData.Rows.Count > 0)
                {
                    for (int i = 0; i < PARQusData.Rows.Count; i++)
                    {
                        SPersonList.Add(PARQusData.Rows[i]["sls_pers_id"].ToString(), PARQusData.Rows[i]["sls_pers_name"].ToString());
                    }
                }

                List<SalePerson> SalePerson = new List<SalePerson>();
                foreach (DataRow dtr in PARQusData.Rows)
                {
                    SalePerson people = new SalePerson();
                    people.salep_id = dtr["sls_pers_id"].ToString();
                    people.salep_name = dtr["sls_pers_name"].ToString();
                    SalePerson.Add(people);
                }
                _LSODetailModel.SalePersonList = SalePerson;
                return Json(SPersonList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult GetCountryList(LSODetailModel _LSODetailModel)
        {
            string CountryName = string.Empty;
            Dictionary<string, string> SPersonList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(_LSODetailModel.SO_Country))
                {
                    CountryName = "0";
                }
                else
                {
                    CountryName = _LSODetailModel.SO_Country;
                }
                SPersonList = _LSODetail_ISERVICE.GetCountryList(CountryName);
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
        public ActionResult GetSOItemList(LSODetailModel _LSODetailModel)
        {
            JsonResult DataRows = null;
            string SOItmName = string.Empty;
            //Dictionary<string, string> SuppList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(_LSODetailModel.SO_ItemName))
                {
                    SOItmName = "0";
                }
                else
                {
                    SOItmName = _LSODetailModel.SO_ItemName;
                }
                DataSet SOItmList = _LSODetail_ISERVICE.GetSOItmListDL(Comp_ID, Br_ID, SOItmName);
                DataRows = Json(JsonConvert.SerializeObject(SOItmList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetSOItemDetail(string ItemID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _LSODetail_ISERVICE.GetSOItemDetailDL(ItemID, Comp_ID);
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
        public JsonResult GetSOItemUOM(string Itm_ID)
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
                DataSet result = _LSODetail_ISERVICE.GetSOItemUOMDL(Itm_ID, Comp_ID);
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
        [HttpPost]
        public JsonResult GetSOItemAvlStock(string Itm_ID)
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
                DataSet result = _LSODetail_ISERVICE.GetSOItemAvlStock(Comp_ID, Br_ID, Itm_ID);
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
        [HttpPost]
        public JsonResult GetPriceListRate(string Itm_ID, string PPolicy, string PGroup, string Cust_id,string OrdDate=null)
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
                DataSet result = _LSODetail_ISERVICE.GetPriceListRate(Comp_ID, Br_ID, Itm_ID, PPolicy, PGroup, Cust_id, OrdDate);
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
        [HttpPost]
        public JsonResult getremarks(string Itm_ID, string Cust_id)/*Modified By Nitesh 15-12-2023 for onchange item name get remarks from item setup comstomer info */
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
                DataSet result = _LSODetail_ISERVICE.getremarks(Comp_ID, Itm_ID, Cust_id);
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
        [HttpPost]
        public JsonResult GetCustAddrDetail(string Cust_id)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _LSODetail_ISERVICE.GetCustAddrDetailDL(Cust_id, Comp_ID);
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
        [HttpPost]
        public JsonResult GetConrateDetail(string Curr_id)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _LSODetail_ISERVICE.GetConvRateDetail(Curr_id, Comp_ID);
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
        [HttpPost]
        public JsonResult GetSOTaxTypeList(string type = null)
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

                DataSet SOTaxList = _LSODetail_ISERVICE.GetSOTaxListDAL(Comp_ID, Br_ID, type);
                DataRows = Json(JsonConvert.SerializeObject(SOTaxList));/*Result convert into Json Format for javasript*/
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
        public JsonResult GetOtherChargeList()
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
                DataSet SO_OC = _LSODetail_ISERVICE.GetOtherChargeDAL(Comp_ID, Br_ID);
                DataRows = Json(JsonConvert.SerializeObject(SO_OC));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        //[HttpPost]
        public string CheckPackingListAgainstLSO(LSODetailModel _LSODetailModelS, string DocNo, string DocDate)
        {
            string DataRows = "N";
            try
            {
                //LSODetailModel _LSODetailModelS = new LSODetailModel();
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
                DataSet Deatils = _LSODetail_ISERVICE.CheckPakingListLSO(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[4].Rows[0]["DependcyPR"].ToString() == "Used")
                {
                    DataRows = "Y";
                    _LSODetailModelS.Message = "PRUnderProcess";
                }
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    DataRows = "Y";
                    _LSODetailModelS.Message = "packinglistunderprocess";
                }
                if (Convert.ToDecimal(Deatils.Tables[1].Rows[0]["reserve_qty"]) > 0)
                {
                    DataRows = "Y";
                    _LSODetailModelS.Message = "ReservedQuantityChanged";
                }
                if (Deatils.Tables[2].Rows[0]["CheckReserStock"].ToString() != "0")
                {
                    DataRows = "Y";
                    _LSODetailModelS.Message = "SomeItemsIsReserved";
                }
                if (Deatils.Tables[3].Rows.Count > 0)
                {
                    DataRows = "Y";
                    _LSODetailModelS.Message = "UsedInPP";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;// Json("ErrorPage");
            }
            return DataRows;
        }
        //[HttpPost]
        public string CheckSalesOrderQtyforForceclosed(LSODetailModel _LSODetailModelS, string DocNo, string DocDate)
        {
            string DataRows = null;
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
                DataSet Deatils = _LSODetail_ISERVICE.CheckLSOQty_ForceClosed(Comp_ID, Br_ID, DocNo, DocDate);
                if (Convert.ToDecimal(Deatils.Tables[0].Rows.Count) > 0)
                {
                    DataRows = "Y";
                    //Session["Message"] = "StockOfSomeItemsIsReservedAgainstThisOrderOrderCannotBeCancelledOrForcedClosed";
                    _LSODetailModelS.Message = "ReservedQuantityChanged";
                }
                else
                {
                    DataRows = "N";
                    //_LSODetailModelS.Message = "ReservedQuantityChanged";
                }
                //DataRows = Deatils.Tables[0].Rows[0][""].ToString();// Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;// Json("ErrorPage");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetSOTaxPercentList(string TaxID)
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
                DataSet SOTaxList = _LSODetail_ISERVICE.GetSOTaxPercentageDAL(Comp_ID, Br_ID, TaxID);
                DataRows = Json(JsonConvert.SerializeObject(SOTaxList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public string InsertSODetails(LSODetailModel _Model)
        {
            //JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            /*--------------------------For Attachment Start--------------------------*/
            //getDocumentName(); /* To set Title*/
            string PageName = _Model.Title.Replace(" ", "");
            /*--------------------------For Attachment End--------------------------*/


            string Comp_ID = string.Empty;

            string BranchID = string.Empty;

            try
            {
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
                if (_Model.DocumentMenuId != null)
                {
                    if (_Model.DocumentMenuId == "105103125")
                    {
                        DocumentMenuId = "105103125";
                    }
                    if (_Model.DocumentMenuId == "105103145110")
                    {
                        DocumentMenuId = "105103145110";
                    }
                }
                if (ModelState.IsValid)/*this Is check Model Is Validate Or Not*/
                {
                    if (_Model.Itemdetails != null)
                    {
                        DataTable DtblHDetail = new DataTable();
                        DataTable DtblItemDetail = new DataTable();
                        DataTable DtblTaxDetail = new DataTable();
                        DataTable DtblOCTaxDetail = new DataTable();
                        DataTable DtblOCDetail = new DataTable();
                        DataTable DtblDeliSchDetail = new DataTable();
                        DataTable DtblTermsDetail = new DataTable();
                        DataTable DtblAttchDetail = new DataTable();
                        DtblHDetail = ToDtblHDetail(_Model);
                        DtblItemDetail = ToDtblItemDetail(_Model.Itemdetails);

                        DtblTaxDetail = ToDtblTaxDetail(_Model.ItemTaxdetails);
                        ViewData["Tax_Details"] = ViewData["TaxDetails"];
                        DtblOCTaxDetail = ToDtblTaxDetail(_Model.ItemOCTaxdetails);
                        ViewData["OCTaxDetails"] = ViewData["TaxDetails"];
                        DtblOCDetail = ToDtblOCDetail(_Model.ItemOCdetails);
                        DtblDeliSchDetail = ToDtblDelSchDetail(_Model.ItemDelSchdetails);
                        DtblTermsDetail = ToDtblTermsDetail(_Model.ItemTermsdetails);
                        /*----------------------Sub Item ----------------------*/
                        DataTable dtSubItem = new DataTable();
                        dtSubItem.Columns.Add("src_doc_number", typeof(string));
                        dtSubItem.Columns.Add("src_doc_date", typeof(string));
                        dtSubItem.Columns.Add("item_id", typeof(string));
                        dtSubItem.Columns.Add("sub_item_id", typeof(string));
                        dtSubItem.Columns.Add("qty", typeof(string));
                        dtSubItem.Columns.Add("foc_qty", typeof(string));

                        if (_Model.SubItemDetailsDt != null)
                        {
                            JArray jObject2 = JArray.Parse(_Model.SubItemDetailsDt);
                            for (int i = 0; i < jObject2.Count; i++)
                            {
                                DataRow dtrowItemdetails = dtSubItem.NewRow();
                                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                                dtrowItemdetails["src_doc_number"] = jObject2[i]["QuotationNumber"].ToString();
                                dtrowItemdetails["src_doc_date"] = jObject2[i]["QuotationDate"].ToString();
                                dtrowItemdetails["foc_qty"] = jObject2[i]["foc_qty"].ToString();
                                dtSubItem.Rows.Add(dtrowItemdetails);
                            }
                            ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                        }

                        /*------------------Sub Item end----------------------*/
                        /*--------------------------For Attachment Start--------------------------*/
                        DtblAttchDetail = BindAttachData(DtblHDetail, _Model.attatchmentdetail);
                        /*--------------------------For Attatchment End--------------------------*/
                        string FSOId = _LSODetail_ISERVICE.FinalInsertLSO_Details(DtblHDetail, DtblItemDetail, DtblTaxDetail, DtblOCTaxDetail, DtblOCDetail, DtblDeliSchDetail, DtblTermsDetail, DtblAttchDetail, dtSubItem);
                        /*--------------------------For Attatchment Start--------------------------*/
                        if (FSOId == "DocModify")
                        {
                            _Model.Message = "DocModify";
                            _Model.BtnName = "BtnRefresh";
                            _Model.Command = "Refresh";
                            TempData["ModelData"] = _Model;
                        }
                        else
                        {
                            string transtype = DtblHDetail.Rows[0]["TransType"].ToString();
                            string FSO_Id = FSOId.Split(',')[0];
                            if (FSOId.Split(',')[0] == "Data_Not_Found")
                            {
                                var a = FSO_Id.Split('-');
                                var msg = FSO_Id.Replace("_", " ") + " " + FSOId.Split(',')[1] + " in " + PageName;
                                string path = Server.MapPath("~");
                                Errorlog.LogError_customsg(path, msg, "", "");
                                _Model.Message = FSOId.Split(',')[0].Replace("_", "");
                                return ("LSODetail");
                            }
                            string message = FSOId.Split(',')[2];
                            //Session["SO_No"] = FSO_Id;
                            //Session["SO_Date"] = DtblHDetail.Rows[0]["so_dt"].ToString();
                            _Model.SO_Number = FSO_Id;
                            _Model.SO_Date = DtblHDetail.Rows[0]["so_dt"].ToString();

                            //if(transtype=="Save"|| transtype == "Update")
                            //{
                            //    Session["Message"] = "Save";
                            //}
                            if (message == "Drafted")
                            {
                                //Session["Message"] = "Save";
                                _Model.Message = "Save";
                            } 
                            if (message == "Force Closed")
                            {
                                //Session["Message"] = "Save";
                                _Model.Message = "ForceClosed";
                            }
                            if (message == "Cancelled")
                            {
                                try
                                {
                                    //string fileName = "LSO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                    string fileName = "SalesOrder_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                    var filePath = SavePdfDocToSendOnEmailAlert(_Model, _Model.SO_no, _Model.SO_Date, fileName, DocumentMenuId, "C");
                                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, _Model.SO_no, "C", UserID, "", filePath);
                                }
                                catch (Exception exMail)
                                {
                                    _Model.Message = "ErrorInMail";
                                    string path = Server.MapPath("~");
                                    Errorlog.LogError(path, exMail);
                                }
                                _Model.Message = _Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                                //Session["Message"] = "Cancelled";
                                //_Model.Message = "Cancelled";
                            }

                            string guid = "";
                            var _LSODetailModelattch = TempData["ModelDataattch"] as LSODetailModelattch;
                            if (_LSODetailModelattch != null)
                                 guid = _LSODetailModelattch.Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, Br_ID, guid, PageName, FSO_Id, transtype, DtblAttchDetail);

                            /*--------------------------For Attatchment End--------------------------*/
                        }
                    }
                    //Validate = Json(FSOId);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*-------------Attatchment Start-------------------*/
                //getDocumentName(); /* To set Title*/
                PageName = _Model.Title.Replace(" ", "");
                string Guid = "";
                //if (Session["Guid"] != null)
                if (_Model.Guid != null)
                {
                    //Guid = Session["Guid"].ToString();
                    Guid = _Model.Guid;
                }
                var other = new CommonController(_Common_IServices);
                other.DeleteTempFile(Comp_ID + BranchID, PageName, Guid, Server);
                /*-------------Attatchment End-------------------*/
                return ("ErrorPage");
            }
            return ("");
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
        private DataTable ToDtblHDetail(LSODetailModel _Model)
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
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("order_type", typeof(string));
                dtheader.Columns.Add("so_no", typeof(string));
                dtheader.Columns.Add("so_dt", typeof(string));
                dtheader.Columns.Add("src_type", typeof(string));
                dtheader.Columns.Add("apply_tax", typeof(string));
                dtheader.Columns.Add("cust_id", typeof(int));
                dtheader.Columns.Add("curr_id", typeof(int));
                dtheader.Columns.Add("conv_rate", typeof(string));
                dtheader.Columns.Add("emp_id", typeof(int));
                dtheader.Columns.Add("ref_doc_no", typeof(string));
                dtheader.Columns.Add("so_rem", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("disc_amt", typeof(string));
                dtheader.Columns.Add("tax_amt", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("net_val_spec", typeof(string));
                dtheader.Columns.Add("force_close", typeof(string));
                dtheader.Columns.Add("ord_canc", typeof(string));
                dtheader.Columns.Add("create_id", typeof(int));
                dtheader.Columns.Add("ord_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("bill_address", typeof(string));//
                dtheader.Columns.Add("ship_address", typeof(string));
                dtheader.Columns.Add("ExFileNo", typeof(string));
                dtheader.Columns.Add("DestCountry", typeof(int));
                dtheader.Columns.Add("TradeTerms", typeof(string));
                dtheader.Columns.Add("DestPort", typeof(string));
                dtheader.Columns.Add("ord_disc_perc", typeof(string));
                dtheader.Columns.Add("ord_disc_amt", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));
                dtheader.Columns.Add("AutoPR", typeof(string));
                dtheader.Columns.Add("req_area", typeof(string));
                //dtheader.Columns.Add("src_doc_no", typeof(string));
                //dtheader.Columns.Add("src_doc_dt", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _Model.TransType;
                dtrowHeader["comp_id"] = CompID;
                dtrowHeader["br_id"] = BrchID;
                dtrowHeader["order_type"] = _Model.SO_OrderType;
                dtrowHeader["so_no"] = IsNull(_Model.SO_no, "");
                dtrowHeader["so_dt"] = _Model.SO_dt;
                dtrowHeader["src_type"] = _Model.SO_SourceType;
                dtrowHeader["apply_tax"] = _Model.ApplyTax;
                dtrowHeader["cust_id"] = _Model.SO_CustID;
                dtrowHeader["curr_id"] = _Model.SO_CurrencyID;
                dtrowHeader["conv_rate"] = _Model.SO_ExRate;
                dtrowHeader["emp_id"] = _Model.SO_SalePerson1;
                dtrowHeader["ref_doc_no"] = _Model.SO_RefDocNo;
                dtrowHeader["so_rem"] = _Model.SO_Remarks;
                dtrowHeader["gr_val"] = _Model.SO_GrossValue;
                dtrowHeader["disc_amt"] = _Model.SO_DiscountValue;
                dtrowHeader["tax_amt"] = _Model.SO_TaxValue;
                dtrowHeader["oc_amt"] = _Model.SO_OtherCharge;
                dtrowHeader["net_val_bs"] = _Model.SO_NetOrderValue_InBase;
                dtrowHeader["net_val_spec"] = _Model.SO_NetOrderValue_InSep;
                dtrowHeader["force_close"] = ConvertBoolToStrint(_Model.SOForceClosed);
                dtrowHeader["ord_canc"] = ConvertBoolToStrint(_Model.SOCancelled);
                dtrowHeader["create_id"] = UserID;
                dtrowHeader["ord_status"] = IsNull(_Model.SOOrderStatus, "D");
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["bill_address"] = _Model.SO_Bill_Add_Id;
                dtrowHeader["ship_address"] = _Model.SO_Shipp_Add_Id;
                dtrowHeader["ExFileNo"] = _Model.SO_ExportFileNo;
                dtrowHeader["DestCountry"] = IsNull(_Model.SO_Country, "0");
                dtrowHeader["TradeTerms"] = IsNull(_Model.trade_term, "0");
                dtrowHeader["DestPort"] = _Model.SO_PortOfDest;
                dtrowHeader["ord_disc_perc"] = IsNull(_Model.OrderDiscountInPercentage,"0");
                dtrowHeader["ord_disc_amt"] = IsNull(_Model.OrderDiscountInAmount,"0");
                dtrowHeader["cancel_remarks"] = _Model.CancelledRemarks;
                dtrowHeader["AutoPR"] = _Model.hdnAutoPR;
                dtrowHeader["req_area"] = _Model.req_area;
                //dtrowHeader["src_doc_no"] = _Model.SO_SourceDocNo;
                //dtrowHeader["src_doc_dt"] = _Model.SO_SourceDocDate;
                
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
        private DataTable ToDtblItemDetail(string pQItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("SONo", typeof(string));
                dtItem.Columns.Add("SODate", typeof(string));
                dtItem.Columns.Add("src_doc_number", typeof(string));
                dtItem.Columns.Add("src_doc_date", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(int));
                dtItem.Columns.Add("OrderQty", typeof(string));
                dtItem.Columns.Add("OrderBQty", typeof(string));
                dtItem.Columns.Add("InvQty", typeof(string));
                dtItem.Columns.Add("mrp", typeof(string));
                dtItem.Columns.Add("mrp_disc", typeof(string));
                dtItem.Columns.Add("ItmRate", typeof(string));
                dtItem.Columns.Add("ItmDisPer", typeof(string));
                dtItem.Columns.Add("ItmDisAmt", typeof(string));
                dtItem.Columns.Add("DisVal", typeof(string));
                dtItem.Columns.Add("GrossVal", typeof(string));
                dtItem.Columns.Add("AssVal", typeof(string));
                dtItem.Columns.Add("TaxAmt", typeof(string));
                dtItem.Columns.Add("OCAmt", typeof(string));
                dtItem.Columns.Add("NetValSpec", typeof(string));
                dtItem.Columns.Add("NetValBase", typeof(string));
                dtItem.Columns.Add("FClosed", typeof(string));
                dtItem.Columns.Add("Remarks", typeof(string));
                dtItem.Columns.Add("OrderType", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("sr_no", typeof(int));
                dtItem.Columns.Add("FOC", typeof(string));
                dtItem.Columns.Add("price_list_no", typeof(string));
                dtItem.Columns.Add("foc_qty", typeof(string));

                if (pQItemDetail != null)
                {
                    JArray jObject = JArray.Parse(pQItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["SONo"] = jObject[i]["SONo"].ToString();
                        dtrowLines["SODate"] = jObject[i]["SODate"].ToString();
                        dtrowLines["src_doc_number"] = jObject[i]["QuotationNumber"].ToString();
                        dtrowLines["src_doc_date"] = jObject[i]["QuotationDate"].ToString();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        if(jObject[i]["UOMID"].ToString()=="" || jObject[i]["UOMID"].ToString() == null)
                        {
                            dtrowLines["UOMID"] = 0;
                        }
                        else
                        {
                            dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        }                       
                        dtrowLines["OrderQty"] = jObject[i]["OrderQty"].ToString();
                        dtrowLines["OrderBQty"] = jObject[i]["OrderQty"].ToString();
                        dtrowLines["InvQty"] = jObject[i]["InvQty"].ToString();
                        dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                        dtrowLines["mrp_disc"] = jObject[i]["mrp_disc"].ToString();
                        dtrowLines["ItmRate"] = jObject[i]["ItmRate"].ToString();
                        dtrowLines["ItmDisPer"] = jObject[i]["ItmDisPer"].ToString();
                        dtrowLines["ItmDisAmt"] = jObject[i]["ItmDisAmt"].ToString();
                        dtrowLines["DisVal"] = jObject[i]["DisVal"].ToString();
                        dtrowLines["GrossVal"] = jObject[i]["GrossVal"].ToString();
                        dtrowLines["AssVal"] = jObject[i]["AssVal"].ToString();
                        dtrowLines["TaxAmt"] = jObject[i]["TaxAmt"].ToString();
                        dtrowLines["OCAmt"] = jObject[i]["OCAmt"].ToString();
                        dtrowLines["NetValSpec"] = jObject[i]["NetValSpec"].ToString();
                        dtrowLines["NetValBase"] = jObject[i]["NetValBase"].ToString();
                        dtrowLines["FClosed"] = jObject[i]["FClosed"].ToString();
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();
                        dtrowLines["OrderType"] = jObject[i]["OrderType"].ToString();
                        dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                        dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                        dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                        dtrowLines["sr_no"] = jObject[i]["sr_no"].ToString();
                        dtrowLines["FOC"] = jObject[i]["FOC"].ToString();
                        dtrowLines["price_list_no"] = jObject[i]["price_list_no"].ToString();
                        dtrowLines["foc_qty"] = jObject[i]["FOCQty"].ToString();
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
        private DataTable ToDtblTaxDetail(string TaxDetails)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("OrderType", typeof(string));
                dtItem.Columns.Add("SONo", typeof(string));
                dtItem.Columns.Add("SODate", typeof(string));
                dtItem.Columns.Add("src_doc_number", typeof(string));
                dtItem.Columns.Add("src_doc_date", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("TaxID", typeof(int));
                dtItem.Columns.Add("TaxRate", typeof(string));
                dtItem.Columns.Add("TaxValue", typeof(string));
                dtItem.Columns.Add("TaxLevel", typeof(int));
                dtItem.Columns.Add("TaxApplyOn", typeof(string));


                if (TaxDetails != null)
                {
                    JArray jObject = JArray.Parse(TaxDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["OrderType"] = jObject[i]["OrderType"].ToString();
                        dtrowLines["SONo"] = jObject[i]["SONo"].ToString();
                        dtrowLines["SODate"] = jObject[i]["SODate"].ToString();
                        dtrowLines["src_doc_number"] = jObject[i]["QuotationNumber"].ToString();
                        dtrowLines["src_doc_date"] = jObject[i]["QuotationDate"].ToString();
                        dtrowLines["ItemID"] = jObject[i]["TaxItmCode"].ToString();
                        dtrowLines["TaxID"] = jObject[i]["TaxNameID"].ToString();
                        dtrowLines["TaxRate"] = jObject[i]["TaxPercentage"].ToString();
                        dtrowLines["TaxValue"] = jObject[i]["TaxAmount"].ToString();
                        dtrowLines["TaxLevel"] = jObject[i]["TaxLevel"].ToString();
                        dtrowLines["TaxApplyOn"] = jObject[i]["TaxApplyOnID"].ToString();

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
                dtItem.Columns.Add("SONo", typeof(string));
                dtItem.Columns.Add("SODate", typeof(string));
                dtItem.Columns.Add("OC_ID", typeof(int));
                dtItem.Columns.Add("OCValue", typeof(string));
                dtItem.Columns.Add("OrderType", typeof(string));
                dtItem.Columns.Add("OCTaxAmt", typeof(string));
                dtItem.Columns.Add("OCTotalTaxAmt", typeof(string));

                if (OCDetails != null)
                {
                    JArray jObject = JArray.Parse(OCDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["SONo"] = jObject[i]["SONo"].ToString();
                        dtrowLines["SODate"] = jObject[i]["SODate"].ToString();
                        dtrowLines["OC_ID"] = jObject[i]["OCID"].ToString();
                        dtrowLines["OCValue"] = jObject[i]["OCValue"].ToString();
                        dtrowLines["OrderType"] = jObject[i]["OrderType"].ToString();
                        dtrowLines["OCTaxAmt"] = jObject[i]["OCTaxAmt"].ToString();
                        dtrowLines["OCTotalTaxAmt"] = jObject[i]["OCTotalTaxAmt"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    DtblItemOCDetail = dtItem;
                    ViewData["OCDetails"] = dtOCdetail(jObject);
                }

                return DtblItemOCDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private DataTable ToDtblDelSchDetail(string DelSchDetails)
        {
            try
            {
                DataTable DtblItemDelSchDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("OrderType", typeof(string));
                dtItem.Columns.Add("SONo", typeof(string));
                dtItem.Columns.Add("SODate", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("SchDate", typeof(string));
                dtItem.Columns.Add("DeliveryQty", typeof(string));
                dtItem.Columns.Add("WhouseID", typeof(string));

                if (DelSchDetails != null)
                {
                    JArray jObject = JArray.Parse(DelSchDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["OrderType"] = jObject[i]["OrderType"].ToString();
                        dtrowLines["SONo"] = jObject[i]["SONo"].ToString();
                        dtrowLines["SODate"] = jObject[i]["SODate"].ToString();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["SchDate"] = jObject[i]["DeliverySchDate"].ToString();
                        dtrowLines["DeliveryQty"] = jObject[i]["DeliverySchQty"].ToString();
                        dtrowLines["WhouseID"] = jObject[i]["DeliverySchWhouse"].ToString();
                        dtItem.Rows.Add(dtrowLines);


                    }
                    DtblItemDelSchDetail = dtItem;
                    ViewData["DelvScheDetails"] = dtdeldetail(jObject);
                }

                return DtblItemDelSchDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblTermsDetail(string TermsDetails)
        {
            try
            {
                DataTable DtblItemTermsDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("SONo", typeof(string));
                dtItem.Columns.Add("SODate", typeof(string));
                dtItem.Columns.Add("TermsDesc", typeof(string));
                dtItem.Columns.Add("OrderType", typeof(string));
                dtItem.Columns.Add("sno", typeof(int));

                if (TermsDetails != null)
                {
                    JArray jObject = JArray.Parse(TermsDetails);
                    int sno = 1;
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["SONo"] = jObject[i]["SONo"].ToString();
                        dtrowLines["SODate"] = jObject[i]["SODate"].ToString();
                        dtrowLines["TermsDesc"] = jObject[i]["TermsDescription"].ToString();
                        dtrowLines["OrderType"] = jObject[i]["OrderType"].ToString();
                        dtrowLines["sno"] = sno;
                        dtItem.Rows.Add(dtrowLines);
                        sno += 1;
                    }
                    DtblItemTermsDetail = dtItem;
                    ViewData["TrmAndConDetails"] = dttermAndCondetail(jObject);
                }

                return DtblItemTermsDetail;
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

            dtItem.Columns.Add("src_doc_number", typeof(string));
            dtItem.Columns.Add("src_doc_date", typeof(string));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("Avlstock", typeof(string));
            dtItem.Columns.Add("ord_qty_spec", typeof(string));
            dtItem.Columns.Add("OrderBQty", typeof(string));
            dtItem.Columns.Add("InvQty", typeof(string));
            dtItem.Columns.Add("mrp", typeof(string));
            dtItem.Columns.Add("mrp_disc", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_disc_perc", typeof(string));
            dtItem.Columns.Add("item_disc_amt", typeof(string));
            dtItem.Columns.Add("item_disc_val", typeof(string));
            dtItem.Columns.Add("item_gr_val", typeof(string));
            dtItem.Columns.Add("item_ass_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_spec", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("force_close", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("OrderType", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("plan_qty", typeof(string));
            dtItem.Columns.Add("prod_acc_qty", typeof(string));
            dtItem.Columns.Add("pack_qty", typeof(string));
            dtItem.Columns.Add("ship_qty", typeof(string));
            dtItem.Columns.Add("inv_qty", typeof(string));
            dtItem.Columns.Add("tmplt_id", typeof(string)); 
            dtItem.Columns.Add("SrvcType", typeof(string));
            dtItem.Columns.Add("br_res_stk_bs", typeof(string));
            dtItem.Columns.Add("br_rej_stk_bs", typeof(string));
            dtItem.Columns.Add("br_rwk_stk_bs", typeof(string));
            dtItem.Columns.Add("total_stk", typeof(string));
            dtItem.Columns.Add("FOC", typeof(string));
            dtItem.Columns.Add("price_list_no", typeof(string));


            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["src_doc_number"] = jObject[i]["QuotationNumber"].ToString();
                dtrowLines["src_doc_date"] = jObject[i]["QuotationDate"].ToString();
                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                if (jObject[i]["UOMID"].ToString() == "" || jObject[i]["UOMID"].ToString() == null)
                {
                    dtrowLines["uom_id"] = 0;
                }
                else
                {
                    dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                }
                //dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["uom_name"] = jObject[i]["UOMName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["Avlstock"] = jObject[i]["AvlStock"].ToString();
                dtrowLines["ord_qty_spec"] = jObject[i]["OrderQty"].ToString();
                dtrowLines["OrderBQty"] = jObject[i]["OrderQty"].ToString();
                dtrowLines["InvQty"] = jObject[i]["InvQty"].ToString();
                dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                dtrowLines["mrp_disc"] = jObject[i]["mrp_disc"].ToString();
                dtrowLines["item_rate"] = jObject[i]["ItmRate"].ToString();
                dtrowLines["item_disc_perc"] = jObject[i]["ItmDisPer"].ToString();
                dtrowLines["item_disc_amt"] = jObject[i]["ItmDisAmt"].ToString();
                dtrowLines["item_disc_val"] = jObject[i]["DisVal"].ToString();
                dtrowLines["item_gr_val"] = jObject[i]["GrossVal"].ToString();
                dtrowLines["item_ass_val"] = jObject[i]["AssVal"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["TaxAmt"].ToString();
                dtrowLines["item_oc_amt"] = jObject[i]["OCAmt"].ToString();
                dtrowLines["item_net_val_spec"] = jObject[i]["NetValSpec"].ToString();
                dtrowLines["item_net_val_bs"] = jObject[i]["NetValBase"].ToString();
                dtrowLines["force_close"] = jObject[i]["FClosed"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["Remarks"].ToString();
                dtrowLines["OrderType"] = jObject[i]["OrderType"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                dtrowLines["plan_qty"] = "0";
                dtrowLines["prod_acc_qty"] = "0";
                dtrowLines["pack_qty"] = "0";
                dtrowLines["ship_qty"] = "0";
                dtrowLines["inv_qty"] = "0";

                dtrowLines["tmplt_id"] = "0";
                dtrowLines["SrvcType"] = "0";
                dtrowLines["br_res_stk_bs"] = "0";
                dtrowLines["br_rej_stk_bs"] = "0";
                dtrowLines["br_rwk_stk_bs"] = "0";
                dtrowLines["total_stk"] = "0";
                dtrowLines["FOC"] = jObject[i]["FOC"].ToString();
                dtrowLines["price_list_no"] = jObject[i]["price_list_no"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtTaxdetail(JArray jObject)
        {
            DataTable DtblItemTaxDetail = new DataTable();
            DtblItemTaxDetail.Columns.Add("src_doc_number", typeof(string));
            DtblItemTaxDetail.Columns.Add("src_doc_date", typeof(string));
            DtblItemTaxDetail.Columns.Add("item_id", typeof(string));
            DtblItemTaxDetail.Columns.Add("tax_id", typeof(int));
            DtblItemTaxDetail.Columns.Add("tax_name", typeof(string));
            DtblItemTaxDetail.Columns.Add("tax_rate", typeof(string));
            DtblItemTaxDetail.Columns.Add("tax_val", typeof(string));
            DtblItemTaxDetail.Columns.Add("tax_level", typeof(int));
            DtblItemTaxDetail.Columns.Add("tax_apply_on", typeof(string));
            DtblItemTaxDetail.Columns.Add("tax_apply_Name", typeof(string));
            DtblItemTaxDetail.Columns.Add("item_tax_amt", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = DtblItemTaxDetail.NewRow();

                dtrowLines["src_doc_number"] = jObject[i]["QuotationNumber"].ToString();
                dtrowLines["src_doc_date"] = jObject[i]["QuotationDate"].ToString();
                dtrowLines["item_id"] = jObject[i]["TaxItmCode"].ToString();
                dtrowLines["tax_id"] = jObject[i]["TaxNameID"].ToString();
                dtrowLines["tax_name"] = jObject[i]["TaxName"].ToString();
                //dtrowTaxDetailsLines["tax_rate"] = jObjectTax[i]["tax_rate"].ToString();
                string tax_rate = jObject[i]["TaxPercentage"].ToString();
                tax_rate = tax_rate.Replace("%", "");
                dtrowLines["tax_rate"] = tax_rate;
                dtrowLines["tax_val"] = jObject[i]["TaxAmount"].ToString();
                dtrowLines["tax_level"] = jObject[i]["TaxLevel"].ToString();
                dtrowLines["tax_apply_on"] = jObject[i]["TaxApplyOnID"].ToString();
                dtrowLines["tax_apply_Name"] = jObject[i]["taxapplyname"].ToString();
                dtrowLines["item_tax_amt"] = jObject[i]["TotalTaxAmount"].ToString();


                DtblItemTaxDetail.Rows.Add(dtrowLines);
            }

            return DtblItemTaxDetail;
        }
        public DataTable dtOCdetail(JArray jObject)
        {
            DataTable OC_detail = new DataTable();

            OC_detail.Columns.Add("oc_id", typeof(int));
            OC_detail.Columns.Add("oc_name", typeof(string));
            OC_detail.Columns.Add("curr_name", typeof(string));
            OC_detail.Columns.Add("conv_rate", typeof(float));
            OC_detail.Columns.Add("oc_val", typeof(string));
            OC_detail.Columns.Add("OCValBs", typeof(string));
            OC_detail.Columns.Add("tax_amt", typeof(string));
            OC_detail.Columns.Add("total_amt", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowOCDetailsLines = OC_detail.NewRow();

                dtrowOCDetailsLines["oc_id"] = jObject[i]["OCID"].ToString();
                dtrowOCDetailsLines["oc_name"] = jObject[i]["OCName"].ToString();
                dtrowOCDetailsLines["curr_name"] = jObject[i]["OC_Curr"].ToString();
                dtrowOCDetailsLines["conv_rate"] = jObject[i]["OC_Conv"].ToString();
                dtrowOCDetailsLines["oc_val"] = jObject[i]["OCValue"].ToString();
                dtrowOCDetailsLines["OCValBs"] = jObject[i]["OC_AmtBs"].ToString();
                dtrowOCDetailsLines["tax_amt"] = jObject[i]["OCTaxAmt"].ToString();
                dtrowOCDetailsLines["total_amt"] = jObject[i]["OCTotalTaxAmt"].ToString();

                OC_detail.Rows.Add(dtrowOCDetailsLines);
            }

            return OC_detail;
        }
        public DataTable dtdeldetail(JArray jObject)
        {
            DataTable dtDelShed = new DataTable();

            dtDelShed.Columns.Add("item_id", typeof(string));
            dtDelShed.Columns.Add("item_name", typeof(string));
            dtDelShed.Columns.Add("sch_date", typeof(string));
            dtDelShed.Columns.Add("delv_qty", typeof(float));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtDelShed.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sch_date"] = jObject[i]["DeliverySchDate"].ToString();
                dtrowLines["delv_qty"] = jObject[i]["DeliverySchQty"].ToString();
                dtDelShed.Rows.Add(dtrowLines);
            }
            return dtDelShed;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();

            dtSubItem.Columns.Add("src_doc_number", typeof(string));
            dtSubItem.Columns.Add("src_doc_date", typeof(string));
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["src_doc_number"] = jObject2[i]["QuotationNumber"].ToString();
                dtrowItemdetails["src_doc_date"] = jObject2[i]["QuotationDate"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        public DataTable dttermAndCondetail(JArray jObject)
        {
            DataTable dtterm = new DataTable();

            dtterm.Columns.Add("term_desc", typeof(string));
            dtterm.Columns.Add("sno", typeof(int));

            int sno = 1;
            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtterm.NewRow();

                dtrowLines["term_desc"] = jObject[i]["TermsDescription"].ToString();
                dtrowLines["sno"] = sno;

                dtterm.Rows.Add(dtrowLines);
                sno += 1;
            }
            return dtterm;
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
        [HttpPost]
        public JsonResult GetCurrentDT()
        {
            CurrentDetail CurrentDT = new CurrentDetail();
            try
            {
                CurrentDT.CurrentUser = Session["UserName"].ToString();
                CurrentDT.CurrentDT = DateTime.Now.ToString("dd-MM-yyyy HH:mm");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

            return Json(CurrentDT);
        }
        //  [HttpPost]
        public ActionResult InsertLSOApproveDetails(LSODetailModel _LSODetailModels, string SONo, string SODate, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_status1, string docid, string CustType)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
            try
            {
                // LSODetailModel _LSODetailModels = new LSODetailModel();
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
                //    if (Session["MenuDocumentId"].ToString() == "105103125")
                //    {
                //        MenuDocId = "105103125";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145110")
                //    {
                //        MenuDocId = "105103145110";
                //    }
                //}
                if (docid != null && docid != "")
                {
                    MenuDocId = docid;
                    _LSODetailModels.DocumentMenuId = docid;
                }
                else
                {
                    MenuDocId = _LSODetailModels.DocumentMenuId;
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string SOId = _LSODetail_ISERVICE.InsertLSOApproveDetails(SONo, SODate, BranchID, MenuDocId, Comp_ID, UserID, mac_id, A_Status, A_Level, A_Remarks);
                string So_no = SOId.Split(',')[0];
                string Raise_PR = SOId.Split(',')[3];
                string PR_no = SOId.Split(',')[4];
                try
                {
                    //string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "SalesOrder_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(_LSODetailModels,SONo, SODate, fileName, MenuDocId,"AP");
                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, MenuDocId, So_no, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _LSODetailModels.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                //Session["Message"] = "Approved";
                //Session["SO_No"] = So_no;
                UrlModel URLModelApprove = new UrlModel();
                //_LSODetailModels.Message = "Approved";
                if(Raise_PR =="Y")
                {
                    _LSODetailModels.Message = _LSODetailModels.Message == "ErrorInMail" ? "PRApproved_ErrorInMail" : "PR_Approved";
                    _LSODetailModels.PR_NO = PR_no;
                }
                else
                {
                    _LSODetailModels.Message = _LSODetailModels.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
               
                _LSODetailModels.SO_Number = So_no;
                _LSODetailModels.SO_Date = SODate;
                if (WF_status1 != null && WF_status1 != "")
                {
                    _LSODetailModels.WF_status1 = WF_status1;/*WF_status is use when come from Dashbord bdlclick direct*/
                    URLModelApprove.WF_status1 = _LSODetailModels.WF_status1;
                }
                _LSODetailModels.Command = "Refresh";
                _LSODetailModels.TransType = "update";
                _LSODetailModels.BtnName = "BtnEdit";
                _LSODetailModels.CustType = CustType;
                TempData["ModelData"] = _LSODetailModels;
                URLModelApprove.Command = _LSODetailModels.Command;
                URLModelApprove.BtnName = _LSODetailModels.BtnName;
                URLModelApprove.TransType = _LSODetailModels.TransType;
                URLModelApprove.SO_Number = _LSODetailModels.SO_Number;
                URLModelApprove.SO_Date = _LSODetailModels.SO_Date;
                URLModelApprove.DocumentMenuId = _LSODetailModels.DocumentMenuId;
                URLModelApprove.CustType = _LSODetailModels.CustType;

                //URLModelApprove.DocumentMenuId = _LSODetailModel.DocumentMenuId;
                TempData["ListFilterData"] = ListFilterData1;
                Validate = Json(SOId);
                return RedirectToAction("LSODetail", URLModelApprove);
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                //string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType)
        {
            //Session["Message"] = "";
            LSODetailModel _LSOtorefresh = new LSODetailModel();
            UrlModel URLModel = new UrlModel();
            _LSOtorefresh.Message = "";
            var a = TrancType.Split(',');
            _LSOtorefresh.SO_Number = a[0].Trim();
            _LSOtorefresh.SO_Date = a[1].Trim();
            _LSOtorefresh.DocumentMenuId = a[2].Trim();
            var WF_status1 = a[3].Trim();
            if (WF_status1 != null && WF_status1 != "")
            {
                _LSOtorefresh.WF_status1 = a[3].Trim();
                URLModel.WF_status1 = a[3].Trim();
            }
            _LSOtorefresh.CustType = a[4].Trim();
            _LSOtorefresh.TransType = "Update";
            _LSOtorefresh.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _LSOtorefresh;
            URLModel.SO_Number = a[0].Trim();
            URLModel.SO_Date = a[1].Trim();
            URLModel.TransType = "Update";
            URLModel.BtnName = _LSOtorefresh.BtnName;
            URLModel.DocumentMenuId = a[2].Trim();
            URLModel.CustType = a[4].Trim();
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("LSODetail", URLModel);
        }
        //[HttpPost]
        public JsonResult DeleteSODetails(string SONo, string SODate, string PageName)
        {
            JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
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
                string SODelete = _LSODetail_ISERVICE.SO_InsertRollback(Comp_ID, BranchID, SONo, SODate);
                //Session["Message"] = "Deleted";
                //_LSODetailModel.Message = "Deleted";
                /*--------------------------For Attachment Start--------------------------*/
                if (!string.IsNullOrEmpty(SONo))
                {
                    //getDocumentName(); /* To set Title*/
                    PageName = PageName.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string SONo1 = SONo.Replace("/", "");
                    other.DeleteTempFile(Comp_ID + BranchID, PageName, SONo1, Server);
                }
                /*--------------------------For Attachment End--------------------------*/
                Validate = Json(SODelete);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return Validate;
        }
        public DataSet GetSODetailEdit(string LSO_No, string LSO_Date)
        {
            try
            {
                DataSet DataRows = null;
                string Comp_ID = string.Empty;
                string BranchID = string.Empty;
                string User_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    User_ID = Session["UserId"].ToString();
                }
                DataSet result = _LSODetail_ISERVICE.GetSODetailDL(Comp_ID, BranchID, LSO_No, LSO_Date, User_ID, DocumentMenuId);
                DataRows = result;//Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetQuotationNumberList(string Cust_id)
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
                DataSet result = _LSODetail_ISERVICE.GetQuotationNumberList(Cust_id, Comp_ID, Br_ID);

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
        public JsonResult GetQuotDetail(string QuotNo)
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
                DataSet result = _LSODetail_ISERVICE.GetQuotDetail(QuotNo, Comp_ID, Br_ID);
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
        public JsonResult GetQTDetail(string QTNo, string QTDate, string so_no,string DocumentMenuId)
        {
            try
            {
                JsonResult DataRows = null;
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
                DataSet result = _LSODetail_ISERVICE.GetQTDetail(Comp_ID, BranchID, QTNo, QTDate, so_no, DocumentMenuId);
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
        /*--------------------------For PDF Print Start--------------------------*/
        [HttpPost]
        public FileResult GenratePdfFile(LSODetailModel _model, string Command)
        {
            var SoType = "";
            //if (_model.DocumentMenuId == "105103125")/*commented by Hina on 13-11-2024 to show all option on Export print also*/
            //{
            //SoType = "D";
            DataTable dt = new DataTable();
                dt.Columns.Add("PrintFormat", typeof(string));
                dt.Columns.Add("ShowProdDesc", typeof(string));
                dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
                dt.Columns.Add("ShowProdTechDesc", typeof(string));
                dt.Columns.Add("ShowSubItem", typeof(string));
                dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ItemAliasName", typeof(string));/*Add by Hina on 13-11-2024*/
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("PrintItemImage", typeof(string));
            dt.Columns.Add("DelivrySchdl", typeof(string));
            dt.Columns.Add("CustProductCode", typeof(string));
            DataRow dtr = dt.NewRow();
                //dtr["PrintFormat"] = _model.PrintFormat;
                dtr["ShowProdDesc"] = _model.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
                dtr["ShowSubItem"] = _model.ShowSubItem;
                dtr["CustAliasName"] = _model.CustomerAliasName;
            dtr["ItemAliasName"] = _model.ItemAliasName;
            dtr["ShowTotalQty"] = _model.ShowTotalQty;
            dtr["PrintItemImage"] = _model.PrintItemImage;
            dtr["DelivrySchdl"] = _model.ShowDeliverySchedule;
            dtr["CustProductCode"] = _model.ShowCustProductCode;
            dt.Rows.Add(dtr);
                ViewBag.PrintOption = dt;
            if (_model.DocumentMenuId == "105103125")/*code start add by Hina on 13-11-2024 to show all option on Export print also*/
            {
                SoType = "D";/*code start add by Hina on 13-11-2024 to show all option on Export print also*/
            }
            else
            {
                SoType = "E";
            }
            ViewBag.DocumentMenuId = _model.DocumentMenuId;
            //return File(GetPdfData(_model.DocumentMenuId, _model.inv_no, _model.inv_dt, ProntOption), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
            if (Command == "Print")
            {
                return File(GetPdfData(_model.SO_no, _model.SO_dt, _model.DocumentMenuId, Command, SoType, _model.ShowDeliverySchedule), "application/pdf", "SalesOrder.pdf");
            }
            else
            {             
                var DocNo = _model.SO_no.Replace("/", "");
                return File(GetPdfData(_model.SO_no, _model.SO_dt, _model.DocumentMenuId, Command, SoType,""), "application/pdf", DocNo + ".pdf");
            }

        }
        public byte[] GetPdfData(string soNo, string soDate,string DocumentMenuId, string Command,string SoType,string ShowDeliverySchedule)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            Document pdfDoc1 = null;
            PdfWriter writer = null;

            try
            {
                string htmlcontent = "";
                string DelSchedule = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                Byte[] bytes = null;
                DataSet Details = new DataSet();
                string LogoImage = string.Empty;
                if (Command == "Print")
                {
                     Details = _LSODetail_ISERVICE.GetSalesOrderDeatilsForPrint(CompID, Br_ID, soNo, soDate);
                    ViewBag.PageName = "SO";
                    
                    ViewBag.Title = "Sales Order";
                    ViewBag.Details = Details;
                    ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by Hina Sharma on 04-04-2025*/
                    string path1 = Server.MapPath("~") + "..\\Attachment\\";
                    string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                    ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                    ViewBag.InvoiceTo = "";
                    ViewBag.ApprovedBy = "Arvind Gupta";
                    ViewBag.DocStatus = Details.Tables[0].Rows[0]["ord_status"].ToString().Trim();
                    LogoImage= LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                    ViewBag.DocId = DocumentMenuId;
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/SalesOrderPrint.cshtml"));
                    if(ShowDeliverySchedule=="Y")
                    {
                        DelSchedule = ConvertPartialViewToString(PartialView("~/Areas/Common/Views/Cmn_PrintReportDeliverySchedule.cshtml"));
                    }
                    
                }
                else
                {
                    ViewBag.PageName = "SO";
                    ViewBag.Title = "Sales Order";
                    
                    DataSet ds = _LSODetail_ISERVICE.GetIntimationDetail(CompID, Br_ID, soNo, soDate, SoType);
                    ViewBag.Details = ds;
                    string path1 = Server.MapPath("~") + "..\\Attachment\\";
                    string LogoPath = path1 + ds.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                    ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/LocalSalesOrder/intimation.cshtml"));
                }
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    //pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    if (Command == "Print")
                    {
                        pdfDoc = new Document(PageSize.A4, 0f, 0f, 155f, 20f);
                        //pdfDoc1 = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                       
                    }
                    else
                    {
                        pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                        //pdfDoc1 = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    }
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    if (Command == "Print")
                    {
                        reader = new StringReader(DelSchedule);
                        pdfDoc.NewPage();
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    }
                    //PdfPage page=pdfDoc.AddProducer
                    pdfDoc.Close();
                    bytes = stream.ToArray();

                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    string draftImage = "";
                    if ( ViewBag.DocStatus == "C")
                    {
                         draftImage = Server.MapPath("~/Content/Images/cancelled.png");
                    }
                    else 
                    {
                         draftImage = Server.MapPath("~/Content/Images/draft.png");
                    }



                        /*-------------------Start Code Add By Hina Sharma on 17-04-2025 to Make Common header-----------------------*/
                        BaseFont bf1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font fontnormal = new Font(bf1, 9, Font.NORMAL);
                    Font fontbold = new Font(bf1, 9, Font.BOLD);
                    //fontbold.SetStyle("bold");
                    Font fonttitle = new Font(bf1, 15, Font.BOLD);
                    //fonttitle.SetStyle("underline");
                    /*-------------------Start Code Add By Hina Sharma on 17-04-2025 to Make Common header-----------------------*/
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
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 10, 0);

                                    /*-------------------Start Code Add By Hina Sharma on 17-04-2025 to Make Common header-----------------------*/
                                   
                                        if (Command == "Print")
                                        {

                                            if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["logo"].ToString()))
                                        {
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(LogoImage))
                                                {
                                                    var Logo = Image.GetInstance(LogoImage);
                                                    Logo.SetAbsolutePosition(21, 790);
                                                    //Logo.SetAbsolutePosition(475, 720);
                                                    //Logo.ScaleAbsolute(100f, 95f);
                                                    Logo.ScaleAbsolute(110f, 35f);


                                                    if (i == 1)
                                                        content.AddImage(Logo);
                                                }
                                            }
                                            catch
                                            {

                                            }

                                        }
                                        string comp_nm, Comp_Add, Comp_Add1, cont_num, email_id, comp_website, State, gst_no, br_pan_no, ship_to = string.Empty;

                                            comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();
                                            Comp_Add = Details.Tables[0].Rows[0]["Comp_Add"].ToString().Trim();
                                            Comp_Add1 = Details.Tables[0].Rows[0]["Comp_Add1"].ToString().Trim();

                                            cont_num = Details.Tables[0].Rows[0]["cont_num1"].ToString().Trim();
                                            email_id = Details.Tables[0].Rows[0]["email_id1"].ToString().Trim();
                                            email_id = Details.Tables[0].Rows[0]["email_id1"].ToString().Trim();
                                            if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["comp_website"].ToString().Trim()))
                                            {
                                                comp_website = Details.Tables[0].Rows[0]["comp_website"].ToString().Trim();
                                            }
                                            else
                                            {
                                                comp_website = "";
                                            }
                                            State = Details.Tables[0].Rows[0]["State"].ToString().Trim();
                                            gst_no = Details.Tables[0].Rows[0]["gst_no"].ToString().Trim();
                                            br_pan_no = Details.Tables[0].Rows[0]["br_pan_no"].ToString().Trim();
                                            
                                            Phrase ptitle = new Phrase(String.Format("Sales Order", i, PageCount), fonttitle);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 795, 0);

                                            Phrase compname = new Phrase(String.Format("{0}", comp_nm, i, PageCount), fontbold);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, compname, 21, 770, 0);

                                            Phrase address = new Phrase(String.Format("Address : {0}", Comp_Add, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, address, 21,760,0 );
                                        Int32 H = 0;
                                        if(Comp_Add1!=null && Comp_Add1 != "")
                                        {
                                            H = 10;
                                            Phrase address1 = new Phrase(String.Format("{0}", Comp_Add1, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, address1, 62, 750, 0);
                                        }
                                        
                                      
                                      

                                        Phrase phone = new Phrase(String.Format("Ph.No.  : {0}", cont_num, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, phone, 21, 750 - H, 0);

                                            Phrase email = new Phrase(String.Format("Email : {0}", email_id, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, email, 21, 740 - H, 0);

                                            Phrase website = new Phrase(String.Format("Website : {0}", comp_website, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, website, 21, 730 - H, 0);

                                            Phrase state = new Phrase(String.Format("State Code/State : {0}", State, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, state, 21, 720 - H, 0);

                                            Phrase gstno = new Phrase(String.Format("GST No. : {0}", gst_no, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, gstno, 21, 710 - H, 0);

                                            Phrase panno = new Phrase(String.Format("PAN No. : {0}", br_pan_no, i, PageCount), fontnormal);
                                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, panno, 21, 700 - H, 0);
                                            

                                        }
                                   
                                    /*-------------------End Code Add By Hina Sharma on 17-04-2025 to Make Common header-----------------------*/
                                }
                            }
                            bytes = ms.ToArray();
                        }
                    }
                }
                return bytes.ToArray();
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
        /*--------------------------For PDF Print End--------------------------*/

        /*--------------------------For Attachment Start--------------------------*/
        protected DataTable BindAttachData(DataTable DtblHDetail, string SOAttachDeatil)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            DataTable dtAttachment = new DataTable();
            dtAttachment.Columns.Add("id", typeof(string));
            dtAttachment.Columns.Add("file_name", typeof(string));
            dtAttachment.Columns.Add("file_path", typeof(string));
            dtAttachment.Columns.Add("file_def", typeof(char));
            dtAttachment.Columns.Add("comp_id", typeof(Int32));
            if (SOAttachDeatil != null)
            {
                JArray jObject = JArray.Parse(SOAttachDeatil);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtAttachment.NewRow();

                    dtrowLines["id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["file_name"] = jObject[i]["file_name"].ToString();
                    dtrowLines["file_path"] = jObject[i]["file_path"].ToString();
                    dtrowLines["file_def"] = jObject[i]["file_def"].ToString();
                    dtrowLines["comp_id"] = CompID;
                    dtAttachment.Rows.Add(dtrowLines);
                }
            }

            DataTable dtAttach = new DataTable();
            var _LSODetailModelattch = TempData["ModelDataattch"] as LSODetailModelattch;
            //TempData["ModelDataattch"] = null;
            string Transtype = DtblHDetail.Rows[0]["TransType"].ToString();
            string SO_No = DtblHDetail.Rows[0]["so_no"].ToString();
            //if (Session["AttachMentDetailItmStp"] != null)
            if (SOAttachDeatil != null)
            {
                if (_LSODetailModelattch != null)
                {
                    if (_LSODetailModelattch.AttachMentDetailItmStp != null)
                    {
                        //dtAttach = Session["AttachMentDetailItmStp"] as DataTable;
                        dtAttach = _LSODetailModelattch.AttachMentDetailItmStp as DataTable;
                        // Session["AttachMentDetailItmStp"] = null;
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
            }
            if (dtAttach.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAttach.Rows)
                {
                    string flag = "Y";
                    foreach (DataRow dr1 in dtAttachment.Rows)
                    {
                        string drImg = dr1["file_name"].ToString();
                        string ObjImg = dr["file_name"].ToString();
                        if (drImg == ObjImg)
                        {
                            flag = "N";
                        }
                    }
                    if (flag == "Y")
                    {
                        DataRow dtrowAttachment1 = dtAttachment.NewRow();
                        if (!string.IsNullOrEmpty(SO_No))
                        {
                            dtrowAttachment1["id"] = SO_No;
                        }
                        else
                        {
                            dtrowAttachment1["id"] = "0";
                        }
                        string path = dr["file_path"].ToString();
                        string file_name = dr["file_name"].ToString();
                        dtrowAttachment1["file_path"] = path;
                        dtrowAttachment1["file_name"] = file_name;
                        dtrowAttachment1["file_def"] = "Y";
                        dtrowAttachment1["comp_id"] = CompID;
                        dtAttachment.Rows.Add(dtrowAttachment1);
                    }

                    //DataRow dtrowAttachment1 = dtAttachment.NewRow();
                    //if (!string.IsNullOrEmpty(SO_No))
                    //{
                    //    dtrowAttachment1["id"] = SO_No;
                    //}
                    //else
                    //{
                    //    dtrowAttachment1["id"] = "0";
                    //}
                    //string path = dr["file_path"].ToString();
                    //string file_name = dr["file_name"].ToString();
                    ////if (Transtype == "Update")
                    ////{
                    ////    file_name = CompID + Br_ID + SO_No.Replace("/", "") + "_" + file_name;
                    ////    path = dr["file_path"].ToString() + file_name;
                    ////}
                    //dtrowAttachment1["file_path"] = path;
                    //dtrowAttachment1["file_name"] = file_name;
                    //dtrowAttachment1["file_def"] = "Y";
                    //dtrowAttachment1["comp_id"] = CompID;
                    //dtAttachment.Rows.Add(dtrowAttachment1);
                }

            }
            return dtAttachment;
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                LSODetailModelattch _LSODetailModelattch = new LSODetailModelattch();
                // string TransType = "Save";
                // string SO_No = "";
                string branchID = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();

                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _LSODetailModelattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _LSODetailModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _LSODetailModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _LSODetailModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        public ActionResult GetLSOAttatchDetailEdit(string LSO_No, string LSO_Date)
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
                DataSet result = _LSODetail_ISERVICE.GetLSOAttatchDetailEdit(CompID, Br_ID, LSO_No, LSO_Date);
                ViewBag.AttechmentDetails = result.Tables[0];
                ViewBag.Disable = true;
                return PartialView("~/Areas/Common/Views/cmn_imagebind.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        /*--------------------------For Attachment End--------------------------*/
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
        , string Flag, string Status, string QtNo, string Doc_no, string Doc_dt,string DocumentMenuId,string src_type)
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
                int QtyDigit = 0;
                if (DocumentMenuId == "105103145110")
                {
                    QtyDigit = Convert.ToInt32(Session["ExpImpQtyDigit"]);
                }
                else
                {
                    QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.status = Status;
                if (Flag == "Quantity" || Flag == "FocQuantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {   
                        if(src_type == "D")
                        {
                            dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                        }
                        else
                        {
                            //if (Doc_no == "")
                            //{
                                dt = _LSODetail_ISERVICE.GetSubItemDetailsBySQ(CompID, BrchID, QtNo, Item_id).Tables[0];
                            //}
                            //else
                            //{
                            //    dt = _LSODetail_ISERVICE.SO_GetSubItemDetails(CompID, BrchID, Item_id, QtNo, Doc_no, Doc_dt, Flag).Tables[0];
                            //}
                        }
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (src_type == "D")
                                {
                                    if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                    {
                                        //dt.Rows[i]["Qty"] = item.GetValue("qty");
                                        if(Flag == "FocQuantity")
                                        {
                                            dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("foc_qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                        }
                                        else
                                        {
                                            dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                        }
                                        

                                    }
                                }
                                else 
                                {
                                    if (item.GetValue("src_doc_number").ToString() == dt.Rows[i]["qt_no"].ToString() && item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                    {
                                        //dt.Rows[i]["Qty"] = item.GetValue("qty");
                                        if (Flag == "FocQuantity")
                                        {
                                            dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("foc_qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                        }
                                        else
                                        {
                                            dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                        }

                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _LSODetail_ISERVICE.SO_GetSubItemDetails(CompID, BrchID, Item_id, QtNo, Doc_no, Doc_dt, Flag).Tables[0];
                    }
                }
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
        public string SavePdfDocToSendOnEmailAlert(LSODetailModel _model,string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            string SoType = "";
            DataTable dt = new DataTable();
            var commonCont = new CommonController(_Common_IServices);
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ItemAliasName", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("PrintItemImage", typeof(string));
            dt.Columns.Add("DelivrySchdl", typeof(string));
            dt.Columns.Add("CustProductCode", typeof(string));
            DataRow dtr = dt.NewRow();
            //dtr["PrintFormat"] = _model.PrintFormat;
            dtr["ShowProdDesc"] = _model.ShowProdDesc;
            dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
            dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
            dtr["ShowSubItem"] = _model.ShowSubItem;
            dtr["CustAliasName"] = _model.CustomerAliasName;
            dtr["ItemAliasName"] = _model.ItemAliasName;
            dtr["ShowTotalQty"] = _model.ShowTotalQty;
            dtr["PrintItemImage"] = _model.PrintItemImage;
            dtr["DelivrySchdl"] = _model.ShowDeliverySchedule;
            dtr["CustProductCode"] = _model.ShowCustProductCode;


            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            if (docid == "105103125")
            {
                SoType = "D";
            }
            else
            {
                SoType = "E";
            }
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Doc_dt, docid, "Print", SoType, "");
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
        public FileResult ExportItemsToExcel(string soNo, string soDate)
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
            DataTable DtItems = _LSODetail_ISERVICE.GetItemsToExportExcel(CompID, BrchID, soNo, soDate);
            var commonController = new CommonController(_Common_IServices);
            return commonController.ExportDatatableToExcel("DomesticSaleInv_Items", DtItems);
        }
        public ActionResult ReplicateWithSelectPrintTypePopup()
        {
            try
            {
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSaleOrderForReplicate.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public string SavePdfDocToSendOnEmailAlert_Ext(string Doc_no, string Doc_dt, string docid, string srcType, string fileName, string PrintFormat)
        {
            var printOptionsList = JsonConvert.DeserializeObject<List<LSODetailModel>>(PrintFormat);
            LSODetailModel _model = new LSODetailModel();
            DataTable dt = new DataTable();
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ItemAliasName", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("PrintItemImage", typeof(string));
            dt.Columns.Add("DelivrySchdl", typeof(string));
            dt.Columns.Add("CustProductCode", typeof(string));
            DataRow dtr = dt.NewRow();
            if (PrintFormat == "")
            {
                dtr["ShowProdDesc"] = _model.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
                dtr["ShowSubItem"] = _model.ShowSubItem;
                dtr["CustAliasName"] = _model.CustomerAliasName;
                dtr["ItemAliasName"] = _model.ItemAliasName;
                dtr["ShowTotalQty"] = _model.ShowTotalQty;
                dtr["PrintItemImage"] = _model.PrintItemImage;
                dtr["DelivrySchdl"] = _model.ShowDeliverySchedule;
                dtr["CustProductCode"] = _model.ShowCustProductCode;
            }
            else
            {
                dtr["ShowProdDesc"] = printOptionsList[0].ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = printOptionsList[0].ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = printOptionsList[0].ShowProdTechDesc;
                dtr["ShowSubItem"] = printOptionsList[0].ShowSubItem;
                dtr["CustAliasName"] = printOptionsList[0].CustomerAliasName;
                dtr["ItemAliasName"] = printOptionsList[0].ItemAliasName;
                dtr["ShowTotalQty"] = printOptionsList[0].ShowTotalQty;
                dtr["PrintItemImage"] = printOptionsList[0].PrintItemImage;
                dtr["DelivrySchdl"] = printOptionsList[0].ShowDeliverySchedule;
                dtr["CustProductCode"] = _model.ShowCustProductCode;
            }

            dt.Rows.Add(dtr);
            ViewBag.PrintOption = dt;
            var data = GetPdfData(Doc_no, Doc_dt, docid, "Print", srcType, "");
            var commonCont = new CommonController(_Common_IServices);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
        //Added by Nidhi on 04-08-2025
        public ActionResult SendEmailAlert(LSODetailModel _model, string mail_id, string status, string docid, string Doc_no, string Doc_dt, string statusAM, string filepath, string srcType)
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
                if (statusAM == "Amendment")
                {
                    status = "AM";
                }
                var commonCont = new CommonController(_Common_IServices);
                string message = "";
                string mail_cont = "";
                string file_path = "";
                if (status == "A")
                {
                    try
                    {
                        string fileName = "SO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert_Ext(Doc_no, Doc_dt, docid, srcType, fileName, "");
                        string keyword = @"\ExternalEmailAlertPDFs\";
                        int index = filePath.IndexOf(keyword);
                        file_path = (index >= 0) ? filePath.Substring(index) : filePath;
                        message = _Common_IServices.SendAlertEmailExternal(CompID, BrchID, UserID, docid, Doc_no, "A", mail_id, filePath);
                        if (message.Contains(","))
                        {
                            var a = message.Split(',');
                            message = a[0];
                            mail_cont = a[1];
                        }
                        if (message == "success")
                        {
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
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
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);

                        }

                    }
                    catch (Exception exMail)
                    {
                        message = "ErrorInMail";
                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
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
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'Y', mail_id, mail_cont, file_path);
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
                            _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
                        }
                    }
                    catch (Exception exMail)
                    {
                        string keyword = @"\ExternalEmailAlertPDFs\";
                        int index = filepath.IndexOf(keyword);
                        file_path = (index >= 0) ? filepath.Substring(index) : filepath;
                        message = "ErrorInMail";

                        if (message == "ErrorInMail")
                        {
                            mail_cont = "Invalid sender email configuration";
                        }
                        _Common_IServices.SendAlertlog(CompID, BrchID, "Email", Doc_no, Doc_dt, docid, status, DateTime.Now.ToString(), 'N', mail_id, mail_cont, file_path);
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
        public ActionResult GetPriceListDetails(string cust_id, string item_id,string disable,string OrdDate)
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
                DataTable dt = _LSODetail_ISERVICE.GetPriceListDetails(CompID, BrchID, cust_id, item_id, OrdDate).Tables[0];
                ViewBag.PriceList = dt;
                ViewBag.PriceListdisable = disable;
                return View("~/Areas/ApplicationLayer/Views/Shared/PartialPriceListDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}