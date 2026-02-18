using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.DomesticSalesQuotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.DomesticSalesQuotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Text;
using EnRepMobileWeb.MODELS.Common;
using ZXing;
using System.Configuration;
using System.Collections.Generic;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.DomesticSalesQuotation
{
    public class DomesticSalesQuotationController : Controller
    {
        string title, crm="Y";
        string DocumentMenuId = "";
        Common_IServices _Common_IServices;
        string CompID, language, BrchID, UserID = string.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DomesticSalesQuotation_ISERVICES _DomesticSalesQuotation_ISERVICES;
        public DomesticSalesQuotationController(Common_IServices _Common_IServices, DomesticSalesQuotation_ISERVICES _DomesticSalesQuotation_ISERVICES)
        {
            this._DomesticSalesQuotation_ISERVICES = _DomesticSalesQuotation_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/DomesticSalesQuotation

        private void GetCompDetail()
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
        }
        public ActionResult DomesticSalesQuotationDetail(URLDetailModel URLModel,string Flag)
        {
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
            try
            {
                GetCompDetail();
                
                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.DocDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                DocumentMenuId = Flag;
                var SQModel = TempData["ModelData"] as DomesticSalesQuotationModel;
                if (SQModel != null)
                {
                    SQModel.UserID = UserID;
                    if (URLModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = URLModel.DocumentMenuId;
                    }
                    else
                    {
                        DocumentMenuId = SQModel.DocumentMenuId;
                    }
                  
                    if (URLModel.CustType != null)
                    {
                        var CustType = URLModel.CustType;
                    }
                    else
                    {
                        var CustType = SQModel.CustType;
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    SQModel.DocumentMenuId = DocumentMenuId;
                    SQModel.GstApplicable = ViewBag.GstApplicable;
                    //Session["ProspectFromQuot"] = "N";
                    //Session["ProspectFromRFQ"] = "N";
                    SQModel.ProspectFromRFQ = "N";
                    //   GetAutoCompleteSearchCustList(SQModel, "C", "D");

                    List<CustName> _CustList = new List<CustName>();
                    CustName _CustName = new CustName();
                    _CustName.Cust_id = "0";
                    _CustName.Cust_name = "---Select---";
                    _CustList.Add(_CustName);
                    SQModel.CustNameList = _CustList;
                    GetAllData(SQModel, "C", "D");
                    //GetSalesPersonList(SQModel);

                    List<TaxCalciTaxName> _TaxName = new List<TaxCalciTaxName>();
                    TaxCalciTaxName _TaxNameList = new TaxCalciTaxName();
                    _TaxNameList.tax_name = "---Select---";
                    _TaxNameList.tax_id = "0";
                    _TaxName.Add(_TaxNameList);

                    List<OcCalciOtherCharge> _OCName = new List<OcCalciOtherCharge>();
                    OcCalciOtherCharge _OCNameList = new OcCalciOtherCharge();
                    _OCNameList.oc_name = "---Select---";
                    _OCNameList.oc_id = "0";
                    _OCName.Add(_OCNameList);


                    //ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                    //DataTable dt = _Common_IServices.GetGSTApplicable(CompID, BrchID, "101").Tables[0];
                    //ViewBag.GstApplicable = dt.Rows.Count > 0 ? dt.Rows[0]["param_stat"].ToString() : ""; /*this is used in MasterLayout*/ 
                    string ValDigit = "";
                    string QtyDigit = "";
                    string RateDigit = "";
                    if (SQModel.DocumentMenuId == "105103145105")
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
                    SQModel.ValDigit = ValDigit;
                    SQModel.QtyDigit = QtyDigit;
                    SQModel.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (SQModel.TransType == "Update" || SQModel.TransType == "Edit")
                    {
                        AfterSaveDataShow(SQModel, RateDigit, QtyDigit, ValDigit);
                       
                    }
                    else
                    {
                       // SQModel.DocumentStatus = "D";
                        ViewBag.DocumentStatus ="D";
                        //ViewBag.MenuPageName = getDocumentName();
                        //Session["DocumentStatus"] = "D";
                        SQModel.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        //ViewBag.FinstDt = Session["FinStDt"].ToString();
                        
                    }
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/DomesticSalesQuotationDetail.cshtml", SQModel);
                }
                else
                {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/                                    
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    DomesticSalesQuotationModel SQModel1 = new DomesticSalesQuotationModel();
                    SQModel1.UserID = UserID;
                    if (URLModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = URLModel.DocumentMenuId;
                    }
                    else
                    {
                        if (DocumentMenuId != null)
                        {
                            SQModel1.DocumentMenuId = DocumentMenuId;

                        }
                        else
                        {
                            DocumentMenuId = SQModel1.DocumentMenuId;

                        }
                    }
                    if (Flag != null)
                    {

                        SQModel1.Message = "New";
                        SQModel1.DocumentStatus = "D";
                        SQModel1.BtnName = "BtnAddNew";
                        SQModel1.TransType = "Save";
                        SQModel1.Command = "New";
                        SQModel1.ProspectFromQuot = "Y";
                    }
                    if (URLModel.CustType != null)
                    {
                        var CustType = URLModel.CustType;
                    }
                    else
                    {
                        var CustType = SQModel1.CustType;
                    }
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    SQModel1.DocumentMenuId = DocumentMenuId;
                    SQModel1.GstApplicable = ViewBag.GstApplicable;
                    //Session["ProspectFromQuot"] = "N";
                    //Session["ProspectFromRFQ"] = "N";
                    SQModel1.ProspectFromRFQ = "N";
                    //    GetAutoCompleteSearchCustList(SQModel1, "C", "D");

                    List<CustName> _CustList = new List<CustName>();
                    CustName _CustName = new CustName();
                    _CustName.Cust_id = "0";
                    _CustName.Cust_name = "---Select---";
                    _CustList.Add(_CustName);
                    SQModel1.CustNameList = _CustList;

                    GetAllData(SQModel1, "C", "D");
                    //   GetSalesPersonList(SQModel1);

                    List<TaxCalciTaxName> _TaxName = new List<TaxCalciTaxName>();
                    TaxCalciTaxName _TaxNameList = new TaxCalciTaxName();
                    _TaxNameList.tax_name = "---Select---";
                    _TaxNameList.tax_id = "0";
                    _TaxName.Add(_TaxNameList);

                    List<OcCalciOtherCharge> _OCName = new List<OcCalciOtherCharge>();
                    OcCalciOtherCharge _OCNameList = new OcCalciOtherCharge();
                    _OCNameList.oc_name = "---Select---";
                    _OCNameList.oc_id = "0";
                    _OCName.Add(_OCNameList);                   
                    if (URLModel.DocNo != null || URLModel.DocDate != null)
                    {
                        SQModel1.SQ_no = URLModel.DocNo;
                        SQModel1.SQ_dt = URLModel.DocDate;
                    }
                    if (URLModel.TransType != null)
                    {
                        SQModel1.TransType = URLModel.TransType;
                    }
                    if (URLModel.BtnName != null)
                    {
                        SQModel1.BtnName = URLModel.BtnName;
                    }
                    if (URLModel.Command != null)
                    {
                        SQModel1.Command = URLModel.Command;
                    }
                    if (URLModel.Amend != null)
                    {
                        SQModel1.Amend = URLModel.Amend;
                    }
                  
                    if (URLModel.AmendmentFlag != null)
                    {
                        SQModel1.AmendmentFlag = URLModel.AmendmentFlag;
                    }
                    if (URLModel.rev_no != null)
                    {
                        SQModel1.rev_no = URLModel.rev_no;
                    }
                    //ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, BrchID, DocumentMenuId).Tables[0];
                    //DataTable dt = _Common_IServices.GetGSTApplicable(CompID, BrchID, "101").Tables[0];
                    //ViewBag.GstApplicable = dt.Rows.Count > 0 ? dt.Rows[0]["param_stat"].ToString() : ""; /*this is used in MasterLayout*/ 
                    string ValDigit = "";
                    string QtyDigit = "";
                    string RateDigit = "";
                    if (SQModel1.DocumentMenuId == "105103145105")
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
                    SQModel1.ValDigit = ValDigit;
                    SQModel1.QtyDigit = QtyDigit;
                    SQModel1.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (SQModel1.TransType == "Update" || SQModel1.TransType == "Edit")
                    {
                        AfterSaveDataShow(SQModel1, RateDigit, QtyDigit, ValDigit);                       
                    }
                    else
                    {
                        ViewBag.DocumentStatus = "D";
                        SQModel1.Title = title;                                            
                    }
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/DomesticSalesQuotationDetail.cshtml", SQModel1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void AfterSaveDataShow(DomesticSalesQuotationModel SQModel,string RateDigit, string QtyDigit, string ValDigit)
        {
            try
            {
                GetCompDetail();
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    SQModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                }
                if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                {
                    SQModel.WF_status1 = TempData["WF_status1"].ToString();
                }


                DataSet ds = new DataSet();

                string SQNo = SQModel.SQ_no;
                if (SQModel.AmendmentFlag == "Amendment")
                {
                    ds = _DomesticSalesQuotation_ISERVICES.Edit_SQDetail(SQNo, CompID, BrchID, DocumentMenuId, UserID, "AmendDocument", SQModel.rev_no);
                    SQModel.ForAmmendendBtn = "N";
                }
                else
                {
                    ds = _DomesticSalesQuotation_ISERVICES.Edit_SQDetail(SQNo, CompID, BrchID, DocumentMenuId, UserID, "", "");
                    SQModel.ForAmmendendBtn = ds.Tables[12].Rows[0]["SOAllReadyCreated"].ToString();
                }


                SQModel.SQ_no = ds.Tables[0].Rows[0]["qt_no"].ToString();
                SQModel.SQ_dt = ds.Tables[0].Rows[0]["qt_dt"].ToString();
                SQModel.CustPros_type = ds.Tables[0].Rows[0]["cust_type"].ToString();
                SQModel.Cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                SQModel.SQ_CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();

                List<CustName> _CustList = new List<CustName>();
                _CustList.Add(new CustName { Cust_id = SQModel.Cust_id, Cust_name = SQModel.SQ_CustName });
                SQModel.CustNameList = _CustList;
                SQModel.BillingAddres = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                SQModel.ShippingAddres = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                if (SQModel.CustPros_type == "C")
                {
                    SQModel.Billing_id = Convert.ToInt32(ds.Tables[0].Rows[0]["_bill_addr_ID"].ToString());
                    SQModel.Shipping_id = Convert.ToInt32(ds.Tables[0].Rows[0]["_ship_addr_ID"].ToString());
                }
                SQModel.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                SQModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                SQModel.Currenc = ds.Tables[0].Rows[0]["curr_name"].ToString();
                SQModel.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                SQModel.bs_curr_id = ds.Tables[0].Rows[0]["bs_curr_id"].ToString();
                SQModel.SpanCustPricePolicy = ds.Tables[0].Rows[0]["cust_pr_pol"].ToString();
                SQModel.SpanCustPriceGroup = ds.Tables[0].Rows[0]["cust_pr_grp"].ToString();

                SQModel.convrate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                SQModel.SQ_SalePerson = ds.Tables[0].Rows[0]["emp_id"].ToString();
                SQModel.SQ_SalePersonID = ds.Tables[0].Rows[0]["emp_id"].ToString();
                SQModel.SQ_SalePersonName = ds.Tables[0].Rows[0]["sls_pers_name"].ToString();
                //if(SQModel.SQ_SalePersonName=="")
                //{
                //    SQModel.SQ_SalePersonName = "---Select---";
                //}
                //GetSalesPersonList(SQModel);
                //List<SalePerson> _SpList = new List<SalePerson>();
               // _SpList.Add(new SalePerson { salep_id = SQModel.SQ_SalePerson, salep_name = SQModel.SQ_SalePersonName });
                //SQModel.SalePersonList = _SpList;
                //SQModel.SQ_SalePerson = ds.Tables[0].Rows[0]["emp_id"].ToString();
                //SQModel.SQ_SalePersonID = ds.Tables[0].Rows[0]["emp_id"].ToString();
                SQModel.DtRemarks = ds.Tables[0].Rows[0]["qt_rem"].ToString();
                SQModel.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                SQModel.AssessableVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                SQModel.DiscAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["disc_amt"]).ToString(ValDigit);
                SQModel.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                SQModel.OcAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                SQModel.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                SQModel.NetValSpec = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_spec"]).ToString(ValDigit);
                SQModel.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                SQModel.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                SQModel.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                SQModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                SQModel.AmmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                SQModel.AmmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                SQModel.Createid = ds.Tables[0].Rows[0]["Creator_id"].ToString();
                SQModel.QTStatus = ds.Tables[0].Rows[0]["app_status"].ToString();
                var RaiseOrder = ds.Tables[0].Rows[0]["raise_ord"].ToString();
                SQModel.FClosedFlag = ds.Tables[14].Rows[0]["ForceCloseFlag"].ToString();
                //if(RaiseOrder=="Y")
                //{
                //    SQModel.RaiseOrder = true;
                //    SQModel.BtnName = "Refresh";
                //}
                //else
                //{
                //    SQModel.RaiseOrder = false;
                //}
                SQModel.SQ_ItemDetail = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                SQModel.ItemTaxdetails = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                SQModel.ItemOCdetails = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                SQModel.ItemTermsdetails = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                SQModel.ItemOCTaxdetails = DataTableToJSONWithStringBuilder(ds.Tables[13]);
               
                string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                string create_id = ds.Tables[0].Rows[0]["Creator_id"].ToString();
                string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                ViewBag.Approve_id = approval_id;
                SQModel.Status = doc_status;
                //Session["DocumentStatus"] = doc_status;
                SQModel.DocumentStatus = doc_status;

                if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                {
                    SQModel.Cancelled = true;
                    //Session["BtnName"] = "Refresh";
                    SQModel.BtnName = "Refresh";
                }
                else
                {
                    SQModel.Cancelled = false;
                }

                if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "FC")
                {
                    SQModel.FClosed = true;
                    //Session["BtnName"] = "Refresh";
                    SQModel.BtnName = "Refresh";
                }
                else
                {
                    SQModel.FClosed = false;
                }

                SQModel.WF_BarStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                SQModel.WF_Status = DataTableToJSONWithStringBuilder(ds.Tables[6]);

                if (doc_status != "D" && doc_status != "F")
                {
                    ViewBag.AppLevel = ds.Tables[7];
                }
                //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                if (ViewBag.AppLevel != null && SQModel.Command != "Edit")
                {

                    var sent_to = "";
                    var nextLevel = "";
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        sent_to = ds.Tables[5].Rows[0]["sent_to"].ToString();
                    }

                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        nextLevel = ds.Tables[6].Rows[0]["nextlevel"].ToString().Trim();
                    }

                    if (doc_status == "D")
                    {
                        if (create_id != UserID)
                        {
                            //Session["BtnName"] = "Refresh";
                            SQModel.BtnName = "Refresh";
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
                                SQModel.BtnName = "BtnToDetailPage";
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
                                SQModel.BtnName = "BtnToDetailPage";
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
                            SQModel.BtnName = "BtnToDetailPage";
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
                                SQModel.BtnName = "BtnToDetailPage";
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
                            SQModel.BtnName = "BtnToDetailPage";
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
                            SQModel.BtnName = "BtnToDetailPage";
                        }
                    }
                    if (doc_status == "A")
                    {
                        if (create_id == UserID || approval_id == UserID)
                        {
                            if (SQModel.Command == "Amendment")
                            {

                                // SQModel.DocumentStatus = "D";
                                SQModel.BtnName = "BtnEdit";
                            }
                            else
                            {
                                if (SQModel.AmendmentFlag == "Amendment")
                                {
                                    SQModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    SQModel.BtnName = "BtnToDetailPage";
                                }

                            }
                            //Session["BtnName"] = "BtnToDetailPage";

                        }
                        else
                        {
                            //Session["BtnName"] = "Refresh";
                            SQModel.BtnName = "Refresh";
                        }
                    }
                    if (doc_status == "QP")
                    {
                        //Session["BtnName"] = "Refresh";
                        SQModel.BtnName = "Refresh";
                    }
                }
                if (ViewBag.AppLevel.Rows.Count == 0)
                {
                    ViewBag.Approve = "Y";
                }
                //ViewBag.MenuPageName = getDocumentName();
                SQModel.Title = title;
                ViewBag.ItemDetailsList = ds.Tables[1];
                ViewBag.ItemTaxDetails = ds.Tables[2];
                ViewBag.ItemTaxDetailsList = ds.Tables[10];
                ViewBag.OtherChargeDetails = ds.Tables[3];
                ViewBag.ItemTermsdetails = ds.Tables[4];
                ViewBag.AttechmentDetails = ds.Tables[8];
                ViewBag.SubItemDetails = ds.Tables[11];
                ViewBag.OCTaxDetails = ds.Tables[13];
                //ViewBag.VBRoleList = GetRoleList();
                //ViewBag.FinstDt = Session["FinStDt"].ToString();
                //ViewBag.FinstDt = Session["FinStDt"].ToString();
                if (SQModel.Command == "Amendment")
                {
                    ViewBag.DocumentStatus = "D";
                }
                else
                {
                    ViewBag.DocumentStatus = SQModel.DocumentStatus;
                }
                if (RaiseOrder == "Y")
                {
                    SQModel.RaiseOrder = true;
                    SQModel.BtnName = "Refresh";
                }
                else
                {
                    SQModel.RaiseOrder = false;
                }
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        
               
        }
        private void GetAllData(DomesticSalesQuotationModel SQModel, string CustPros_type, string Cust_type)
        {
            //DomesticSalesQuotationModel SQModel = new DomesticSalesQuotationModel();
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            //string Comp_ID = string.Empty;
            //string Br_ID = string.Empty;
            string UserName = string.Empty;
            string rpt_id = "0";
            try
            {
                GetCompDetail();
                if (string.IsNullOrEmpty(SQModel.SQ_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = SQModel.SQ_CustName;
                }
                SQModel.CustPros_type = CustPros_type;

                string SalesPersonName = string.Empty;
                if (string.IsNullOrEmpty(SQModel.SQ_SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = SQModel.SQ_SalePerson;
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
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //  SPersonList = _DomesticSalesQuotation_ISERVICES.GetSalesPersonList(Comp_ID, SalesPersonName, Br_ID);
                DataSet AllData = new DataSet();
                if (DocumentMenuId == "105103145105")
                {
                    crm = "Y";
                    AllData = _DomesticSalesQuotation_ISERVICES.GetAllData(CompID, CustomerName, BrchID, "1001", CustPros_type, Cust_type, SalesPersonName, SQModel.SQ_no, SQModel.SQ_dt);
                }
                else
                {
                    AllData = _DomesticSalesQuotation_ISERVICES.GetAllData(CompID, CustomerName, BrchID, UserID, CustPros_type, Cust_type, SalesPersonName, SQModel.SQ_no, SQModel.SQ_dt);
                }

                List<CustomerName> _CustList = new List<CustomerName>();                
                    foreach (DataRow data in AllData.Tables[0].Rows)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.cust_id = data["cust_id"].ToString();
                    _CustDetail.cust_name = data["cust_name"].ToString();
                    _CustList.Add(_CustDetail);
                }
                _CustList.Insert(0, new CustomerName() { cust_id = "0", cust_name = "---Select---" });
                SQModel.CustomerNameList = _CustList;

                List<SalePerson> _SlPrsnList = new List<SalePerson>();
                if ((rpt_id == "0" || SQModel.TransType == null || UserID == "1001"|| DocumentMenuId == "105103145105") && crm == "Y")
                {
                    foreach (DataRow data in AllData.Tables[1].Rows)
                    {
                        SalePerson _SlPrsnDetail = new SalePerson();
                        _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                        _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                        _SlPrsnList.Add(_SlPrsnDetail);
                    }
                    _SlPrsnList.Insert(0, new SalePerson() { salep_id = "0", salep_name = "---Select---" });
                }
                else
                {                    
                    _SlPrsnList.Insert(0, new SalePerson() { salep_id = UserID, salep_name = UserName });                    
                }
                
                SQModel.SalePersonList = _SlPrsnList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void CommonPageDetails()
        {
            try
            {             
                GetCompDetail();
              

                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
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
        //public ActionResult AddNewDomesticSalesQuotation()
        //{
        //    Session["Message"] = "New";
        //    Session["Command"] = "Add";
        //    Session["AppStatus"] = 'D';
        //    Session["TransType"] = "Save";
        //    Session["BtnName"] = "BtnAddNew";
        //    Session.Remove("ProspectFromQuot");

        //    return RedirectToAction("DomesticSalesQuotationDetail", "DomesticSalesQuotation");
        //}
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
        //private DataTable GetRoleList()
        //{
        //    try
        //    {
        //        string UserID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            UserID = Session["userid"].ToString();
        //        }
        //        if (Session["MenuDocumentId"] != null)
        //        {
        //            if (Session["MenuDocumentId"].ToString() == "105103120")
        //            {
        //                DocumentMenuId = "105103120";
        //            }
        //            if (Session["MenuDocumentId"].ToString() == "105103145105")
        //            {
        //                DocumentMenuId = "105103145105";
        //            }
        //        }
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
        //private string getDocumentName()
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["Language"] != null)
        //        {
        //            language = Session["Language"].ToString();
        //        }
        //        if (Session["MenuDocumentId"] != null)
        //        {
        //            if (Session["MenuDocumentId"].ToString() == "105103120")
        //            {
        //                DocumentMenuId = "105103120";
        //            }
        //            if (Session["MenuDocumentId"].ToString() == "105103145105")
        //            {
        //                DocumentMenuId = "105103145105";
        //            }
        //        }
        //        string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
        //        string[] Docpart = DocumentName.Split('>');
        //        int len = Docpart.Length;
        //        if (len > 1)
        //        {
        //            title = Docpart[len - 1].Trim();
        //        }
        //        return DocumentName;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaleQuotationBtnCommand(DomesticSalesQuotationModel SQModel, string command)
        {
            try
            {
                GetCompDetail();
                /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (SQModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        URLDetailModel URLModel = new URLDetailModel();
                        DomesticSalesQuotationModel SQModelAddNew = new DomesticSalesQuotationModel();
                        SQModelAddNew.Message = "New";
                        SQModelAddNew.DocumentStatus = "D";
                        SQModelAddNew.BtnName = "BtnAddNew";
                        SQModelAddNew.TransType = "Save";
                        SQModelAddNew.Command = "New";
                        SQModelAddNew.DocumentMenuId = SQModel.DocumentMenuId;
                        SQModelAddNew.CustType = SQModel.CustType;
                        TempData["ModelData"] = SQModelAddNew;
                        URLModel.DocumentMenuId = SQModel.DocumentMenuId;
                        URLModel.TransType = "Save";
                        URLModel.BtnName = "BtnAddNew";
                        URLModel.Command = "New";
                        URLModel.CustType = SQModel.CustType;
                        //Session["Message"] = "New";
                        //Session["DocumentStatus"] = "D";
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                       
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(SQModel.SQ_no))
                                return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no, SQDate= SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                            else
                                SQModelAddNew.Command = "Refresh";
                            SQModelAddNew.TransType = "Refresh";
                            SQModelAddNew.BtnName = "Refresh";
                            SQModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = SQModelAddNew;
                            return RedirectToAction("DomesticSalesQuotationDetail", "DomesticSalesQuotation");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("DomesticSalesQuotationDetail", "DomesticSalesQuotation", URLModel);

                    case "Edit":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no,SQDate= SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string SQDt = SQModel.SQ_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SQDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no, SQDate = SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                        }
                        /*End to chk Financial year exist or not*/
                        URLDetailModel URLModelEdit = new URLDetailModel();
                        if (CheckSOAgainstQuo(SQModel.SQ_no, SQModel.SQ_dt) == "Used")
                        {
                            //Session["Message"] = "Used";
                            SQModel.Message = "Used";
                            SQModel.BtnName = "Refresh";
                            //SQModel.BtnName = "BtnToDetailPage";
                            TempData["ListFilterData"] = SQModel.ListFilterData1;
                            URLModelEdit.Command = SQModel.Command;
                            URLModelEdit.TransType = SQModel.TransType;
                            URLModelEdit.BtnName = "Refresh";
                            URLModelEdit.DocumentMenuId = SQModel.DocumentMenuId;
                            URLModelEdit.DocDate = SQModel.SQ_dt;
                            URLModelEdit.DocNo = SQModel.SQ_no;
                            TempData["ModelData"] = SQModel;
                        }
                        else
                        {
                            SQModel.TransType = "Update";
                            SQModel.Command = command;
                            SQModel.BtnName = "BtnEdit";
                            SQModel.Message = "New";
                            SQModel.AppStatus = "D";
                            TempData["ModelData"] = SQModel;
                            URLModelEdit.Command = command;
                            URLModelEdit.TransType = "Update";
                            URLModelEdit.BtnName = "BtnEdit";
                            URLModelEdit.DocumentMenuId = SQModel.DocumentMenuId;
                            URLModelEdit.DocDate = SQModel.SQ_dt;
                            URLModelEdit.DocNo = SQModel.SQ_no;
                            //Session["TransType"] = "Update";
                            //Session["Command"] = command;
                            //Session["BtnName"] = "BtnEdit";
                            //Session["Message"] = "New";
                            //Session["AppStatus"] = 'D';
                            //Session["SQ_No"] = SQModel.SQ_no;
                            TempData["ListFilterData"] = SQModel.ListFilterData1;
                        }
                        return RedirectToAction("DomesticSalesQuotationDetail", URLModelEdit);

                    case "Delete":
                        URLDetailModel URLModelDelete = new URLDetailModel();
                        DomesticSalesQuotationModel SQModelDelete = new DomesticSalesQuotationModel();
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        SQDelete(SQModel, command);
                        SQModelDelete.Message = "Deleted";
                        SQModelDelete.Command = "Refresh";
                        SQModelDelete.SQ_no = "";
                        SQModelDelete.TransType = "Refresh";
                        SQModelDelete.AppStatus = "DL";
                        SQModelDelete.BtnName = "BtnDelete";
                        SQModelDelete.DocumentMenuId = SQModel.DocumentMenuId;
                        SQModelDelete.CustType = SQModel.CustType;
                        TempData["ModelData"] = SQModelDelete;
                        URLModelDelete.DocumentMenuId = SQModel.DocumentMenuId;
                        URLModelDelete.CustType = SQModel.CustType;
                        URLModelDelete.TransType = "Refresh";
                        URLModelDelete.BtnName = "BtnDelete";
                        URLModelDelete.Command = "Refresh";
                        TempData["ListFilterData"] = SQModel.ListFilterData1;
                        return RedirectToAction("DomesticSalesQuotationDetail", URLModelDelete);

                    case "Save":
                        //Session["Command"] = command;
                        SQModel.Command = command;

                        if (SQModel.TransType == null)
                        {
                            SQModel.TransType = command;
                        }
                        if (SQModel.Amend != null && 
                            SQModel.Amend != "" && SQModel.Amend == "Amend" && SQModel.Status == "A")
                        {
                            SQModel.TransType = "Amendment";

                        }
                        SaveSQDetail(SQModel);
                        if (SQModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        //SQModel.SQ_no = Session["SQ_No"].ToString();
                        SQModel.ProspectFromQuot = null;
                        URLDetailModel URLModelSave = new URLDetailModel();
                        URLModelSave.DocumentMenuId = SQModel.DocumentMenuId;
                        URLModelSave.CustType = SQModel.CustType;
                        URLModelSave.TransType = "Update";
                        URLModelSave.BtnName = "BtnSave";
                        URLModelSave.Command = command;
                        URLModelSave.DocDate = SQModel.SQ_dt;
                        URLModelSave.DocNo = SQModel.SQ_no;
                        TempData["ModelData"] = SQModel;
                        TempData["ListFilterData"] = SQModel.ListFilterData1;
                        //Session["SQ_Date"] = Session["SQ_Date"].ToString();
                        return RedirectToAction("DomesticSalesQuotationDetail", URLModelSave);

                    case "Forward":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no,SQDate= SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string SQDt1 = SQModel.SQ_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SQDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no, SQDate = SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no,SQDate= SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string SQDt2 = SQModel.SQ_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SQDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no, SQDate = SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        URLDetailModel URLModelApprove = new URLDetailModel();
                        SQModel.Command = command;
                        SQListApprove(SQModel);
                        TempData["ModelData"] = SQModel;
                        URLModelApprove.Command = SQModel.Command;
                        URLModelApprove.TransType = SQModel.TransType;
                        URLModelApprove.DocNo = SQModel.SQ_no;
                        URLModelApprove.DocDate = SQModel.SQ_dt;
                        URLModelApprove.DocumentMenuId = SQModel.DocumentMenuId;
                        URLModelApprove.CustType = SQModel.CustType;
                        TempData["ListFilterData"] = SQModel.ListFilterData1;
                        return RedirectToAction("DomesticSalesQuotationDetail", URLModelApprove);

                    case "Refresh":
                        URLDetailModel URLModelRefresh = new URLDetailModel();
                        DomesticSalesQuotationModel SQModelRefresh = new DomesticSalesQuotationModel();
                        SQModelRefresh.BtnName = "Refresh";
                        SQModelRefresh.Command = command;
                        SQModelRefresh.TransType = "Save";
                        SQModelRefresh.DocumentMenuId = SQModel.DocumentMenuId;
                        SQModelRefresh.CustType = SQModel.CustType;
                        TempData["ModelData"] = SQModelRefresh;
                        //URLModelRefresh.Command = SQModel.Command;
                        //URLModelRefresh.TransType = SQModel.TransType;
                        //URLModelRefresh.DocNo = SQModel.SQ_no;
                        //URLModelRefresh.DocDate = SQModel.SQ_dt;
                        URLModelRefresh.DocumentMenuId = SQModel.DocumentMenuId;
                        URLModelRefresh.CustType = SQModel.CustType;
                        URLModelRefresh.BtnName = "Refresh";
                        SQModelRefresh.Command = command;
                        SQModelRefresh.TransType = "Save";
                        TempData["ListFilterData"] = SQModel.ListFilterData1;
                        return RedirectToAction("DomesticSalesQuotationDetail", URLModelRefresh);
                    case "Amendment":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no,SQDate= SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string SQDt3 = SQModel.SQ_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SQDt3) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSQ", new { SQId = SQModel.SQ_no, SQDate = SQModel.SQ_dt, ListFilterData = SQModel.ListFilterData1, DocumentMenuId = SQModel.DocumentMenuId, WF_status = SQModel.WF_Status, CustType = SQModel.CustType });
                        }
                        /*End to chk Financial year exist or not*/
                        URLDetailModel URLModelAmend = new URLDetailModel();
                        SQModel.TransType = "Update";
                            SQModel.Command = command;
                            SQModel.BtnName = "BtnEdit";
                            SQModel.Message = "New";
                            SQModel.AppStatus = "D";
                        SQModel.Amend = "Amend";
                      
                        TempData["ModelData"] = SQModel;
                            URLModelAmend.Command = command;
                            URLModelAmend.TransType = "Update";
                            URLModelAmend.BtnName = "BtnEdit";
                        URLModelAmend.Amend = "Amend";
                     
                        URLModelAmend.DocumentMenuId = SQModel.DocumentMenuId;
                            URLModelAmend.DocDate = SQModel.SQ_dt;
                            URLModelAmend.DocNo = SQModel.SQ_no;                            
                            TempData["ListFilterData"] = SQModel.ListFilterData1;
                         
                        return RedirectToAction("DomesticSalesQuotationDetail", URLModelAmend);
                    case "Print":
                        return GenratePdfFile(SQModel);
                    case "BacktoList":
                        if (SQModel.DocumentMenuId == "105103120")
                        {
                            TempData["WF_status"] = SQModel.WF_status1;
                            TempData["ListFilterData"] = SQModel.ListFilterData1;
                            return RedirectToAction("DomesticSalesQuotationDList", "DomesticSalesQuotationList");
                        }
                        else
                        {
                            TempData["WF_status"] = SQModel.WF_status1;
                            TempData["ListFilterData"] = SQModel.ListFilterData1;
                            return RedirectToAction("DomesticSalesQuotationEList", "DomesticSalesQuotationList");
                        }
                    //Session.Remove("Message");
                    //Session.Remove("TransType");
                    //Session.Remove("Command");
                    //Session.Remove("BtnName");
                    //Session.Remove("DocumentStatus");
                    //Session.Remove("ProspectFromQuot");
                    //    TempData["ListFilterData"] = SQModel.ListFilterData1;
                    //return RedirectToAction("DomesticSalesQuotationList", "DomesticSalesQuotationList");
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
        public ActionResult SaveSQDetail(DomesticSalesQuotationModel SQModel)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = SQModel.Title.Replace(" ", "");

            try
            {
               
                GetCompDetail();
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCDetail = new DataTable();
                DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblTermsDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable dtheader = new DataTable();

                DtblHDetail = ToDtblHDetail(SQModel);
                DtblItemDetail = ToDtblItemDetail(SQModel.SQ_ItemDetail);
                DtblTaxDetail = ToDtblTaxDetail(SQModel.ItemTaxdetails);
                DtblOCDetail = ToDtblOCDetail(SQModel.ItemOCdetails);
                DtblOCTaxDetail = ToDtblTaxDetail(SQModel.ItemOCTaxdetails);/*add by Hina on 22-05-2025*/
                DtblTermsDetail = ToDtblTermsDetail(SQModel.ItemTermsdetails);

                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));
                if (SQModel.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(SQModel.SubItemDetailsDt);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                }

                /*------------------Sub Item end----------------------*/

                DataTable dtAttachment = new DataTable();
                var _SalesQuotationModelattch = TempData["ModelDataattch"] as SalesQuotationModelattch;
                TempData["ModelDataattch"] = null;
                if (SQModel.attatchmentdetail != null)
                {
                    if (_SalesQuotationModelattch != null)
                    {
                        if (_SalesQuotationModelattch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _SalesQuotationModelattch.AttachMentDetailItmStp as DataTable;
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
                        if (SQModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = SQModel.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(SQModel.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(SQModel.SQ_no))
                            {
                                dtrowAttachment1["id"] = SQModel.SQ_no;
                            }
                            else
                            {
                                dtrowAttachment1["id"] = "0";
                            }
                            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                            dtrowAttachment1["file_def"] = "Y";
                            dtrowAttachment1["comp_id"] = CompID;
                            dtAttachment.Rows.Add(dtrowAttachment1);
                        }
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (SQModel.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string SQTCode = string.Empty;
                            if (!string.IsNullOrEmpty(SQModel.SQ_no))
                            {
                                SQTCode = SQModel.SQ_no;
                            }
                            else
                            {
                                SQTCode = "0";
                            }
                            string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrchID + SQTCode.Replace("/", "") + "*");

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
                SaveMessage = _DomesticSalesQuotation_ISERVICES.InsertSQ_Details(DtblHDetail, DtblItemDetail, DtblTaxDetail, DtblOCDetail, DtblOCTaxDetail, DtblTermsDetail, dtSubItem, DtblAttchDetail);
                string[] Data = SaveMessage.Split(',');
                string SQNo = Data[0];
                string SQ_No = SQNo.Replace("/", "");
                string Message = Data[2];
                string SQDate = Data[1];
                string Message1 = Data[4];
                string StatusCode = Data[3];
                if (Message == "Data_Not_Found")
                {

                    var a = StatusCode.Split('-');// statuscode is Table Name
                    var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    SQModel.Message = Message.Replace("_", "");
                    return RedirectToAction("DomesticSalesQuotationDetail");
                }
                /*-----------------Attachment Section Start------------------------*/
                if (Message == "Save")
                {
                    string Guid = "";
                    if (_SalesQuotationModelattch != null)
                    {
                        //if (Session["Guid"] != null)
                        if (_SalesQuotationModelattch.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _SalesQuotationModelattch.Guid;
                        }
                    }
                    string guid = Guid;
                    var comCont = new CommonController(_Common_IServices);
                    comCont.ResetImageLocation(CompID, BrchID, guid, PageName, SQ_No, SQModel.TransType, DtblAttchDetail);

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
                    //                string SQ_No1 = SQ_No.Replace("/", "");
                    //                string img_nm = CompID + BrchID + SQ_No1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                    
                    try
                    {
                        ViewBag.PrintFormat = PrintFormatDataTable(SQModel, null);
                        //string fileName = "DSQ_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "SalesQuotation_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(SQModel.SQ_no, SQModel.SQ_dt, fileName, null, SQModel.DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, SQModel.DocumentMenuId, SQModel.SQ_no, "C", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        SQModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    SQModel.Message = SQModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    // SQModel.Message = "Cancelled";
                    SQModel.Command = "Update";
                    SQModel.TransType = "Update";
                    SQModel.AppStatus = "D";
                    SQModel.BtnName = "Refresh";
                    //TempData["SQ_No"] = SQModel.SQ_no;
                    //TempData["SQ_Date"] = SQModel.SQ_dt;
                    return RedirectToAction("DomesticSalesQuotationDetail");
                }

                if (Message == "Update" || Message == "Save")
                {
                    SQModel.Message = "Save";
                    SQModel.Command = "Update";
                    SQModel.SQ_no = SQNo;
                    SQModel.SQ_dt = SQDate;
                    SQModel.TransType = "Update";
                    //SQModel.InvType = SQModel.QtType;
                    SQModel.AppStatus = "D";
                    SQModel.BtnName = "BtnSave";
                    //Session["Message"] = "Save";
                    //Session["Command"] = "Update";
                    //Session["SQ_No"] = SQNo;
                    //Session["SQ_Date"] = SQDate;
                    //Session["TransType"] = "Update";
                    //Session["InvType"] = SQModel.QtType;
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "BtnSave";
                }
                return RedirectToAction("DomesticSalesQuotationDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (SQModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (SQModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = SQModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private DataTable PrintFormatDataTable(DomesticSalesQuotationModel _Model, string PrintFormat)
        {
            DataTable dt = new DataTable();
            //var commonCont = new CommonController(_Common_IServices);

            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));
            //dt.Columns.Add("ShowTotalQty", typeof(string));
            //dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            //dt.Columns.Add("showDeclare1", typeof(string));
            //dt.Columns.Add("showDeclare2", typeof(string));
            //dt.Columns.Add("showInvHeading", typeof(string));
            //dt.Columns.Add("NumberOfCopy", typeof(string));
            //dt.Columns.Add("PrintShipFromAddress", typeof(string));

            if (_Model != null)
            {
                DataRow dtr = dt.NewRow();
                dtr["PrintFormat"] = _Model.PrintFormat;
                dtr["ShowProdDesc"] = _Model.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = _Model.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = _Model.ShowProdTechDesc;
                dtr["ShowSubItem"] = _Model.ShowSubItem;
                dtr["CustAliasName"] = _Model.CustAliasName;
                dtr["PrintRemarks"] = _Model.PrintRemarks;
                //dtr["ShowTotalQty"] = _Model.ShowTotalQty;
                //dtr["ShowWithoutSybbol"] = _Model.ShowWithoutSybbol;
                //dtr["showDeclare1"] = _Model.showDeclare1;
                //dtr["showDeclare2"] = _Model.showDeclare2;
                //dtr["showInvHeading"] = _Model.showInvHeading;
                //dtr["NumberOfCopy"] = _Model.NumberofCopy;
                //dtr["PrintShipFromAddress"] = _Model.PrintShipFromAddress;
                dt.Rows.Add(dtr);
            }
            else
            {
                DataRow dtr = dt.NewRow();
                if (!string.IsNullOrEmpty(PrintFormat))
                {
                    JArray jObject = JArray.Parse(PrintFormat);
                    dtr["PrintFormat"] = jObject[0]["PrintFormat"].ToString(); ;
                    dtr["ShowProdDesc"] = jObject[0]["ShowProdDesc"];
                    dtr["ShowCustSpecProdDesc"] = jObject[0]["ShowCustSpecProdDesc"];
                    dtr["ShowProdTechDesc"] = jObject[0]["ShowProdTechDesc"];
                    dtr["ShowSubItem"] = jObject[0]["ShowSubItem"];
                    dtr["CustAliasName"] = jObject[0]["CustAliasName"];
                    dtr["PrintRemarks"] = jObject[0]["PrintRemarks"];
                    dt.Rows.Add(dtr);
                }
            }
            return dt;
        }
        private DataTable ToDtblHDetail(DomesticSalesQuotationModel SQModel)
        {
            try
            {
                GetCompDetail();
                DataTable DtblHDetail = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("QT_type", typeof(string));
                dtheader.Columns.Add("SQ_no", typeof(string));
                dtheader.Columns.Add("SQ_dt", typeof(string));
                dtheader.Columns.Add("Cust_Type", typeof(string));
                dtheader.Columns.Add("cust_id", typeof(int));
                dtheader.Columns.Add("curr_id", typeof(int));
                dtheader.Columns.Add("conv_rate", typeof(string));
                dtheader.Columns.Add("emp_id", typeof(int));
                dtheader.Columns.Add("Remarks", typeof(string));
                dtheader.Columns.Add("GrVal", typeof(string));
                //dtheader.Columns.Add("AssVal", typeof(string));
                dtheader.Columns.Add("DiscAmt", typeof(string));
                dtheader.Columns.Add("TaxAmt", typeof(string));
                dtheader.Columns.Add("OcAmt", typeof(string));
                dtheader.Columns.Add("NetValBs", typeof(string));
                dtheader.Columns.Add("NetValSpec", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("user_id", typeof(int));
                dtheader.Columns.Add("SQT_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(int));
                dtheader.Columns.Add("ship_add_id", typeof(int));
                dtheader.Columns.Add("RaiseOrder", typeof(string));
                dtheader.Columns.Add("ForceClosed", typeof(char));
                
                DataRow dtrowHeader = dtheader.NewRow();
                //dtrowHeader["TransType"] = Session["TransType"].ToString();
                dtrowHeader["TransType"] = SQModel.TransType;
                dtrowHeader["comp_id"] = CompID;
                dtrowHeader["br_id"] = BrchID;
                dtrowHeader["QT_type"] = SQModel.QtType;
                dtrowHeader["SQ_no"] = SQModel.SQ_no;
                dtrowHeader["SQ_dt"] = SQModel.SQ_dt;
                dtrowHeader["Cust_Type"] = SQModel.CustPros_type;
                dtrowHeader["cust_id"] = SQModel.Cust_id;
                dtrowHeader["curr_id"] = SQModel.curr_id;
                dtrowHeader["conv_rate"] = SQModel.convrate;
                //dtrowHeader["emp_id"] = SQModel.SQ_SalePerson;/*commented and modify by Hina sharma on 30-01-2025 for null value on cancel document*/
                dtrowHeader["emp_id"] = IsNull(SQModel.SQ_SalePerson,"0");
                dtrowHeader["emp_id"] = IsNull(SQModel.SQ_SalePersonID, "0");
                dtrowHeader["Remarks"] = SQModel.DtRemarks;
                dtrowHeader["GrVal"] = SQModel.GrVal;
                //dtrowHeader["AssVal"] = SQModel.AssessableVal;
                dtrowHeader["DiscAmt"] = IsNull(SQModel.DiscAmt, "0");
                dtrowHeader["TaxAmt"] = IsNull(SQModel.TaxAmt, "0");
                dtrowHeader["OcAmt"] = IsNull(SQModel.OcAmt, "0");
                dtrowHeader["NetValSpec"] = SQModel.NetValSpec;
                dtrowHeader["NetValBs"] = SQModel.NetValBs;
                string cancelflag = SQModel.Cancelled.ToString();
                if (cancelflag == "False")
                {
                    dtrowHeader["Cancelled"] = "N";
                }
                else
                {
                    dtrowHeader["Cancelled"] = "Y";
                }
               
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                dtrowHeader["SQT_status"] = "D";
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                if (SQModel.CustPros_type == "C")
                {
                    dtrowHeader["bill_add_id"] = SQModel.Billing_id;
                    dtrowHeader["ship_add_id"] = SQModel.Shipping_id;
                }
                string RaiseOrderflag = SQModel.RaiseOrder.ToString();
                if (RaiseOrderflag == "False")
                {
                    dtrowHeader["RaiseOrder"] = "N";
                }
                else
                {
                    dtrowHeader["RaiseOrder"] = "Y";
                }
                string ForceClosed = SQModel.FClosed.ToString();
                if (ForceClosed == "False")
                {
                    dtrowHeader["ForceClosed"] = "N";
                }
                else
                {
                    dtrowHeader["ForceClosed"] = "Y";
                }
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
        private DataTable ToDtblItemDetail(string QTItemList)
        {
            try
            {
                GetCompDetail();
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
             
                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("br_id", typeof(int));
                dtItem.Columns.Add("QTNo", typeof(string));
                dtItem.Columns.Add("QTDate", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(int));
                dtItem.Columns.Add("QuotQty", typeof(string));
                dtItem.Columns.Add("OrderBQty", typeof(string));
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
                dtItem.Columns.Add("Remarks", typeof(string));
                dtItem.Columns.Add("QtType", typeof(string));
                dtItem.Columns.Add("cust_type", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("LandedPrice", typeof(string));
                dtItem.Columns.Add("Landed_Remarks", typeof(string));
                dtItem.Columns.Add("sr_no", typeof(int));
                dtItem.Columns.Add("FOC", typeof(string));

                if (QTItemList != null)
                {
                    JArray jObject = JArray.Parse(QTItemList);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["comp_id"] = CompID;
                        dtrowLines["br_id"] = BrchID;
                        dtrowLines["QTNo"] = jObject[i]["QTNo"].ToString();
                        dtrowLines["QTDate"] = jObject[i]["QTDate"].ToString();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        if (jObject[i]["UOMID"].ToString() == "" || jObject[i]["UOMID"].ToString() == null)
                        {
                            dtrowLines["UOMID"] = 0;
                        }
                        else
                        {
                            dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        }

                        dtrowLines["QuotQty"] = jObject[i]["QuotQty"].ToString();
                        dtrowLines["OrderBQty"] = jObject[i]["OrderBQty"].ToString();
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
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();
                        dtrowLines["QtType"] = jObject[i]["QtType"].ToString();
                        dtrowLines["cust_type"] = jObject[i]["cust_type"].ToString();
                        dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                        dtrowLines["hsn_code"] = jObject[i]["ItemHsnCode"].ToString();
                        dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                        dtrowLines["LandedPrice"] = jObject[i]["LandedCost"].ToString();
                        dtrowLines["Landed_Remarks"] = jObject[i]["Landed_Remarks"].ToString();
                        dtrowLines["sr_no"] =Convert.ToInt32(jObject[i]["sr_no"].ToString());
                        dtrowLines["FOC"] = jObject[i]["FOC"].ToString();

                        dtItem.Rows.Add(dtrowLines);
                    }
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
                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("br_id", typeof(int));
                dtItem.Columns.Add("QtType", typeof(string));
                dtItem.Columns.Add("QTNo", typeof(string));
                dtItem.Columns.Add("QTDate", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("TaxID", typeof(int));
                dtItem.Columns.Add("TaxRate", typeof(double));
                dtItem.Columns.Add("TaxValue", typeof(double));
                dtItem.Columns.Add("TaxLevel", typeof(int));
                dtItem.Columns.Add("TaxApplyOn", typeof(string));
                if (TaxDetails != null)

                {
                    JArray jObject = JArray.Parse(TaxDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["comp_id"] = CompID;
                        dtrowLines["br_id"] =BrchID;
                        dtrowLines["QtType"] = jObject[i]["QtType"].ToString();
                        dtrowLines["QTNo"] = jObject[i]["QTNo"].ToString();
                        dtrowLines["QTDate"] = jObject[i]["QTDate"].ToString();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["TaxID"] = jObject[i]["TaxID"].ToString();
                        dtrowLines["TaxRate"] = jObject[i]["TaxRate"].ToString();
                        dtrowLines["TaxValue"] = jObject[i]["TaxValue"].ToString();
                        dtrowLines["TaxLevel"] = jObject[i]["TaxLevel"].ToString();
                        dtrowLines["TaxApplyOn"] = jObject[i]["TaxApplyOn"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
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
        private DataTable ToDtblOCDetail(string SQ_OCList)
        {
            try
            {

                DataTable DtblItemOCDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("br_id", typeof(int));
                dtItem.Columns.Add("QTNo", typeof(string));
                dtItem.Columns.Add("QTDate", typeof(string));
                dtItem.Columns.Add("OC_ID", typeof(int));
                dtItem.Columns.Add("OCValue", typeof(string));
                dtItem.Columns.Add("QtType", typeof(string));
                /*Add by Hina on 22-05-2025*/
                dtItem.Columns.Add("OCTaxAmt", typeof(string));
                dtItem.Columns.Add("OCTotalTaxAmt", typeof(string));
                if (SQ_OCList != null)
                {

                    JArray jObject = JArray.Parse(SQ_OCList);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["comp_id"] =CompID;
                        dtrowLines["br_id"] = BrchID;
                        dtrowLines["QTNo"] = jObject[i]["QTNo"].ToString();
                        dtrowLines["QTDate"] = jObject[i]["QTDate"].ToString();
                        dtrowLines["OC_ID"] = jObject[i]["OCID"].ToString();
                        dtrowLines["OCValue"] = jObject[i]["OCValue"].ToString();
                        dtrowLines["QtType"] = jObject[i]["QtType"].ToString();
                        dtrowLines["OCTaxAmt"] = jObject[i]["OCTaxAmt"].ToString(); /*Add by Hina on 22-05-2025*/
                        dtrowLines["OCTotalTaxAmt"] = jObject[i]["OCTotalTaxAmt"].ToString(); /*Add by Hina on 22-05-2025*/
                        dtItem.Rows.Add(dtrowLines);
                    }
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
        private DataTable ToDtblTermsDetail(string SQTermsList)
        {
            try
            {
                DataTable DtblItemTermsDetail = new DataTable();
                DataTable dtItem = new DataTable();
               
                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("br_id", typeof(int));
                dtItem.Columns.Add("QTNo", typeof(string));
                dtItem.Columns.Add("QTDate", typeof(string));
                dtItem.Columns.Add("TermsDesc", typeof(string));
                dtItem.Columns.Add("QtType", typeof(string));
                dtItem.Columns.Add("sno", typeof(int));
                if (SQTermsList != null)
                {
                    JArray jObject = JArray.Parse(SQTermsList);
                    int sno = 1;
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["comp_id"] = CompID;
                        dtrowLines["br_id"] = BrchID;
                        dtrowLines["QTNo"] = jObject[i]["QTNo"].ToString();
                        dtrowLines["QTDate"] = jObject[i]["QTDate"].ToString();
                        dtrowLines["TermsDesc"] = jObject[i]["TermsDesc"].ToString();
                        dtrowLines["QtType"] = jObject[i]["QtType"].ToString();
                        dtrowLines["sno"] = sno;
                        dtItem.Rows.Add(dtrowLines);
                        sno += 1;
                    }
                    DtblItemTermsDetail = dtItem;
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
        public ActionResult GetSalesPersonList(DomesticSalesQuotationModel SQModel)
        {
            string SalesPersonName = string.Empty;
            Dictionary<string, string> SPersonList = new Dictionary<string, string>();
            try
            {
                GetCompDetail();
                if (string.IsNullOrEmpty(SQModel.SQ_SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = SQModel.SQ_SalePerson;
                }
                SPersonList = _DomesticSalesQuotation_ISERVICES.GetSalesPersonList(CompID, SalesPersonName, BrchID);
                List<SalePerson> _SlPrsnList = new List<SalePerson>();
                foreach (var data in SPersonList)
                {
                    SalePerson _SlPrsnDetail = new SalePerson();
                    _SlPrsnDetail.salep_id = data.Key;
                    _SlPrsnDetail.salep_name = data.Value;
                    _SlPrsnList.Add(_SlPrsnDetail);
                }

                SQModel.SalePersonList = _SlPrsnList;
                return Json(SPersonList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            //return Json(SPersonList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
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
        private string IsNull(string Str, string Str2)
        {
            if (!string.IsNullOrEmpty(Str))
            {
            }
            else
                Str = Str2;
            return Str;
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
        public ActionResult EditSQ(string SQId,string SQDate, string ListFilterData, string DocumentMenuId, 
            string WF_status, string CustType,string Amendment,string rev_no)
        {/*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
            GetCompDetail();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            URLDetailModel URLModel = new URLDetailModel();
            DomesticSalesQuotationModel _Model = new DomesticSalesQuotationModel();
            _Model.Message = "New";
            _Model.Command = "Add";
            _Model.SQ_no = SQId;
            _Model.SQ_dt = SQDate;
            _Model.TransType = "Update";
            _Model.AppStatus = "D";
            _Model.BtnName = "BtnToDetailPage";
            _Model.AmendmentFlag = Amendment;
            _Model.rev_no = rev_no;
            _Model.WF_status1 = WF_status;
            TempData["ModelData"] = _Model;
            URLModel.DocNo = SQId;
            //URLModel.DocDate = "";
            URLModel.DocDate = SQDate;
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.Command = "Add";
            URLModel.DocumentMenuId = DocumentMenuId;
            URLModel.CustType = CustType;
            URLModel.AmendmentFlag = Amendment;
            URLModel.rev_no = rev_no;
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["SQ_No"] = SQId;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("DomesticSalesQuotationDetail", URLModel);
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
        public ActionResult SQListApprove(DomesticSalesQuotationModel SQModel)
        {
            try
            {
               
                string MenuID = string.Empty;
                GetCompDetail();
                MenuID = SQModel.DocumentMenuId;
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103120")
                //    {
                //        MenuID = "105103120";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105103145105")
                //    {
                //        MenuID = "105103145105";
                //    }
                //}
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string QTNo = SQModel.SQ_no;
                string QTDate = SQModel.SQ_dt;
                string A_Status = SQModel.A_Status;
                string A_Level = SQModel.A_Level;
                string A_Remarks = SQModel.A_Remarks;

                string Message = _DomesticSalesQuotation_ISERVICES.QTApproveDetails(CompID, BrchID, QTNo, QTDate, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks);
                string ApMessage = Message.Split(',')[1].Trim();
                string SQ_No = Message.Split(',')[0].Trim();
                if (ApMessage == "A")
                {
                    string PrintFormat = SQModel.PrintFormat;
                    try
                    {
                        ViewBag.PrintFormat = PrintFormatDataTable(SQModel, null);
                        //string fileName = "DSQ_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "SalesQuotation_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(QTNo, QTDate, fileName, null, MenuID,"AP");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, MenuID, QTNo, "AP", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        SQModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    ViewBag.PrintFormat = PrintFormatDataTable(SQModel);
                    //Session["Message"] = "Approved";
                    //SQModel.Message = "Approved";
                    SQModel.Message = SQModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                SQModel.TransType = "Update";
                SQModel.Command = "Approve";
                SQModel.AppStatus = "D";
                SQModel.BtnName = "BtnEdit";
                SQModel.Amend = null;
               
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";

                return RedirectToAction("DomesticSalesQuotationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1, string docid, string CustType)
        {
            DomesticSalesQuotationModel SQModel = new DomesticSalesQuotationModel();
            URLDetailModel URLModel = new URLDetailModel();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    SQModel.SQ_no = jObjectBatch[i]["SQNo"].ToString();
                    SQModel.SQ_dt = jObjectBatch[i]["SQDate"].ToString();

                    SQModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    SQModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    SQModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();

                }
            }
            if (SQModel.A_Status != "Approve")
            {
                SQModel.A_Status = "Approve";
            }
            SQModel.DocumentMenuId = docid;
            SQListApprove(SQModel);
            TempData["ModelData"] = SQModel;
            TempData["WF_status1"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            URLModel.DocNo = SQModel.SQ_no;
            URLModel.DocDate = SQModel.SQ_dt;
            URLModel.TransType = SQModel.TransType;
            URLModel.BtnName = SQModel.BtnName;
            URLModel.Command = SQModel.Command;
            URLModel.DocumentMenuId = docid;
            URLModel.CustType = CustType;
            return RedirectToAction("DomesticSalesQuotationDetail", URLModel);
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            //Session["Message"] = "";
            DomesticSalesQuotationModel _Model = new DomesticSalesQuotationModel();
            var a = TrancType.Split(',');
            _Model.SQ_no = a[0].Trim();
            _Model.SQ_dt = a[1].Trim();
            _Model.DocumentMenuId = a[2].Trim();
            var WF_status1 = a[3].Trim();
            _Model.CustType = a[4].Trim();
            _Model.TransType = "Update";
            _Model.WF_status1 = WF_status1;
            _Model.BtnName = "BtnToDetailPage";
            _Model.Message = Mailerror;
            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1; ;
            TempData["ListFilterData"] = ListFilterData1;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.DocNo = a[0].Trim();
            URLModel.DocDate = a[1].Trim();
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.DocumentMenuId = a[2].Trim();
            URLModel.CustType = a[4].Trim();
            return RedirectToAction("DomesticSalesQuotationDetail", URLModel);
        }
        //[HttpPost]
        // public JsonResult CheckSOAgainstQuo(string DocNo, string DocDate)
        // {
        //     JsonResult DataRows = null;
        //     try
        //     {
        //         string Comp_ID = string.Empty;
        //         string Br_ID = string.Empty;
        //         if (Session["CompId"] != null)
        //         {
        //             Comp_ID = Session["CompId"].ToString();
        //         }
        //         if (Session["BranchId"] != null)
        //         {
        //             Br_ID = Session["BranchId"].ToString();
        //         }
        //         DataSet Deatils = _DomesticSalesQuotation_ISERVICES.CheckSODetail(Comp_ID, Br_ID, DocNo, DocDate);
        //         DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
        //     }
        //     catch (Exception ex)
        //     {
        //         string path = Server.MapPath("~");
        //         Errorlog.LogError(path, ex);
        //         return Json("ErrorPage");
        //     }
        //     return DataRows;
        // }
        public string CheckSOAgainstQuo(string DocNo, string DocDate)
        {
            string str = "";
            try
            {
                
                GetCompDetail();
                DataSet Deatils = _DomesticSalesQuotation_ISERVICES.CheckSODetail(CompID, BrchID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
                if (Deatils.Tables[1].Rows.Count > 0)
                {
                   if(Deatils.Tables[1].Rows[0]["ForceCloseFlag"].ToString()=="Y")
                    {
                        str = "UNUsed";
                    }
                   
                   
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
        private ActionResult SQDelete(DomesticSalesQuotationModel SQModel, string command)
        {
            try
            {

                GetCompDetail();
              
                string doc_no = SQModel.SQ_no;
                string Message = _DomesticSalesQuotation_ISERVICES.QTdetailDelete(SQModel, CompID, BrchID);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = SQModel.Title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, doc_no1, Server);
                }
                SQModel.Message = "Deleted";
                SQModel.Command = "Refresh";
                SQModel.SQ_no = "";
                SQModel.TransType = "Refresh";
                SQModel.AppStatus = "DL";
                SQModel.BtnName = "BtnDelete";
                TempData["ModelData"] = SQModel;
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["SQ_No"] = "";
                //SQModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("DomesticSalesQuotationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult GetCustAddressdetail(string CustID, string CustPros_type)
        {
            try
            {
                JsonResult DataRows = null;
             
                GetCompDetail();
                DataTable result = _DomesticSalesQuotation_ISERVICES.GetCustAddressdetail(CustID, CompID, CustPros_type, BrchID);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.VBAddresslist = result;
                Session["ILSearch"] = "IL_Search";

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/_AddressInfomation.cshtml");
        }
        public ActionResult GetAutoCompleteSearchCustList(DomesticSalesQuotationModel SQModel, string CustPros_type, string Cust_type)
        {
            //DomesticSalesQuotationModel SQModel = new DomesticSalesQuotationModel();
            string CustomerName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            
            try
            {
                GetCompDetail();
                if (string.IsNullOrEmpty(SQModel.SQ_CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = SQModel.SQ_CustName;
                }
                SQModel.CustPros_type = CustPros_type;

                CustList = _DomesticSalesQuotation_ISERVICES.GetCustomerList(CompID, CustomerName, BrchID, CustPros_type, Cust_type);

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetCustAddrDetail(string Cust_id, string CustPros_type)
        {
            try
            {
                JsonResult DataRows = null;
 
                GetCompDetail();
                DataSet result = _DomesticSalesQuotation_ISERVICES.GetCustAddrDetailDL(Cust_id, CompID, BrchID, CustPros_type);
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
        public JsonResult GetItemCustomerInfo(string ItemID, string CustID)
        {
            try
            {
                JsonResult DataRows = null;
                GetCompDetail();
                DataSet result = _DomesticSalesQuotation_ISERVICES.GetItemCustomerInfo(ItemID, CustID, CompID);
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
        public JsonResult GetPriceListRate(string Itm_ID, string PPolicy, string PGroup, string Cust_id)
        {
            try
            {
                JsonResult DataRows = null;
                GetCompDetail();
                DataSet result = _DomesticSalesQuotation_ISERVICES.GetPriceListRate(CompID, BrchID, Itm_ID, PPolicy, PGroup, Cust_id);
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
        /*--------------------------For PDF Print Start--------------------------*/
        public FileResult GenratePdfFile(DomesticSalesQuotationModel _model)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("PrintFormat", typeof(string));
            //dt.Columns.Add("ShowProdDesc", typeof(string));
            //dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            //dt.Columns.Add("ShowProdTechDesc", typeof(string));
            //dt.Columns.Add("ShowSubItem", typeof(string));
            //dt.Columns.Add("CustAliasName", typeof(string));
            //if(_model.DocumentMenuId== "105103120")
            //{
            //    dt.Columns.Add("PrintRemarks", typeof(string));/*Add by Hina on 27-09-2024*/
            //}

            //DataRow dtr = dt.NewRow();
            //dtr["PrintFormat"] = _model.PrintFormat;
            //dtr["ShowProdDesc"] = _model.ShowProdDesc;
            //dtr["ShowCustSpecProdDesc"] = _model.ShowCustSpecProdDesc;
            //dtr["ShowProdTechDesc"] = _model.ShowProdTechDesc;
            //dtr["ShowSubItem"] = _model.ShowSubItem;
            //dtr["CustAliasName"] = _model.CustomerAliasName;
            //if (_model.DocumentMenuId == "105103120")
            //{
            //    dtr["PrintRemarks"] = _model.PrintRemarks;/*Add by Hina on 27-09-2024*/
            //}
            //dt.Rows.Add(dtr);
            dt = PrintFormatDataTable(_model);
            ViewBag.PrintOption = dt;

            if (_model.GstApplicable == "Y" && _model.DocumentMenuId != "105103145105")
                return File(GetPdfDataOfGstInv(dt, _model.DocumentMenuId, _model.SQ_no, _model.SQ_dt), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");
            else
                return File(GetPdfData(dt, _model.SQ_no, _model.SQ_dt), "application/pdf", "SalesQuotation.pdf");

            //return File(GetPdfData(_model.SQ_no, _model.SQ_dt), "application/pdf", "SalesQuotation.pdf");
        }
        private DataTable PrintFormatDataTable(DomesticSalesQuotationModel _Model)
        {
            DataTable dt = new DataTable();
            //dt = PrintOptionsDt(PrintFormat);
            var commonCont = new CommonController(_Common_IServices);
            Cmn_PrintOptions cmn_PrintOptions = new Cmn_PrintOptions//Added by Suraj on 08-10-2024
            {
                PrintFormat = _Model.PrintFormat,
                ShowProdDesc = _Model.ShowProdDesc,
                ItemAliasName = _Model.ShowItemAliasName,
                ShowCustSpecProdDesc = _Model.ShowCustSpecProdDesc,
                ShowProdTechDesc = _Model.ShowProdTechDesc,
                ReferenceNo =_Model.ShowReferenceNo,
                CatalogueNo = _Model.ShowCatalogueNo,
                OEMNo = _Model.ShowOEMNo,
                CustAliasName = _Model.CustAliasName,
                ShowHsnNumber = _Model.ShowHsnCode,
                Discount = _Model.ShowDiscount,
                ShipTo = _Model.ShowShipTo,
                BillTo = _Model.ShowBillTo,
                BankDetail = _Model.ShowBankDetail,
                PrintImage = _Model.ShowPrintImage,
                ShowSubItem = _Model.ShowSubItem,
                PrintRemarks = _Model.PrintRemarks,
                CompAddress = _Model.ShowCompAddress,
                //ShowProdTechDesc = _Model.ShowProdTechDesc,

            };
            dt = commonCont.PrintOptionsDt(cmn_PrintOptions);
            return dt;
        }
        public byte[] GetPdfData(DataTable dt,string sqNo, string sqDate)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string Qt_type = "D";
                GetCompDetail();
                //DataSet Details = _DomesticSalesQuotation_ISERVICES.GetSalesQuotationDeatils(CompID, BrchID, sqNo, sqDate);
                DataSet Details = _DomesticSalesQuotation_ISERVICES.GetSlsQtGstDtlForPrint(CompID, BrchID, sqNo, sqDate, Qt_type);
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();//.Replace("/", "\\'");
                ViewBag.PageName = "SQ";
                ViewBag.Title = "Sales Quotation";
                ViewBag.Details = Details;
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();//.Replace("/", "\\'");
                ViewBag.InvoiceTo = "";
                ViewBag.ApprovedBy = "Arvind Gupta";
                string htmlcontent = "";
                var docstatus = Details.Tables[0].Rows[0]["qt_status"].ToString().Trim();
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["qt_status"].ToString().Trim();
                if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/SalesQuotationWithGSTPrintF2.cshtml"));
                }
                else
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/SalesQuotationWithGSTPrint.cshtml"));

                }
                //string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/SalesQuotationPrint.cshtml"));

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    string draftImage = Server.MapPath("~/Content/Images/draft.png");
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                //draftimg.SetAbsolutePosition(20, 40);
                                //draftimg.ScaleAbsolute(650f, 600f);
                                draftimg.SetAbsolutePosition(0, 160);
                                draftimg.ScaleAbsolute(580f, 580f);


                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
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
                //reader.Dispose();
                //pdfDoc.Dispose();
                //writer.Dispose();
            }
        }
        public byte[] GetPdfDataOfGstInv(DataTable dt, string docId, string SQtNo, string SQtDt)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                GetCompDetail();
                string Qt_type = "D";
                string ReportType = "common";

                //switch (docId)
                //{
                //    case "105103140":
                //        Qt_type = "D";
                //        break;
                //    //case "105103145105":
                //    //    Qt_type = "E";
                //    //    break;
                //    //default:
                //    //    Qt_type = "";
                //    //    break;
                //}
                DataSet Details = _DomesticSalesQuotation_ISERVICES.GetSlsQtGstDtlForPrint(CompID, BrchID, SQtNo, SQtDt, Qt_type);

                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp || Request.Url.Host == "localhost")
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[0].Rows[0]["digi_sign"].ToString();//.Replace("/", "\\'");
                ViewBag.PageName = "SQ";
                string QTType = Details.Tables[0].Rows[0]["qt_type"].ToString().Trim();
                ViewBag.Details = Details;
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();//.Replace("/", "\\'");

                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["qt_status"].ToString().Trim();
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                //ViewBag.Title = "Proforma Invoice";/*commented and change by Hina on 27-09-2024 to change name*/
                ViewBag.Title = "Sales Quotation";
                if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/SalesQuotationWithGSTPrintF2.cshtml"));
                }
                else
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/DomesticSalesQuotation/SalesQuotationWithGSTPrint.cshtml"));
                }
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (ReportType == "common")
                    {
                        if (Qt_type == "D")
                        {
                            pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 60f);
                        }
                        else
                        {
                            pdfDoc = new Document(PageSize.A4, 10f, 10f, 70f, 90f);
                        }
                    }
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = GSTHeaderFooterPagination(bytes, Details, ReportType, QTType);
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
        private Byte[] GSTHeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType, string QTType)
        {
            var docstatus = Details.Tables[0].Rows[0]["qt_status"].ToString().Trim();
            var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 15, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            //string logo = Server.MapPath("~/") + Details.Tables[0].Rows[0]["logo"].ToString();
            //string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());
            string draftImage = Server.MapPath("~/Content/Images/draft.png");
            string bnetImage = Server.MapPath("~/images/businet.png");

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        if (ReportType == "common")
                        {
                            if (QTType == "D")
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                //draftimg.SetAbsolutePosition(20, 40);
                                //draftimg.ScaleAbsolute(650f, 600f);
                                draftimg.SetAbsolutePosition(0, 160);
                                draftimg.ScaleAbsolute(580f, 580f);

                                //var qrCode = Image.GetInstance(QR);
                                //qrCode.SetAbsolutePosition(640, 490);
                                //qrCode.ScaleAbsolute(100f, 95f);
                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F")
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
                                    //    if (i == 1)
                                    //        content.AddImage(qrCode);
                                    //}
                                    Phrase ptitle = new Phrase(String.Format("Proforma Invoice", i, PageCount), fonttitle);
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    if (i == PageCount)
                                    {
                                        //try
                                        //{
                                        //    var bnetlogo = Image.GetInstance(bnetImage);
                                        //    bnetlogo.SetAbsolutePosition(502, 35);
                                        //    bnetlogo.ScaleAbsolute(70f, 16f);
                                        //    content.AddImage(bnetlogo);
                                        //}
                                        //catch { }
                                        //Phrase ftr = new Phrase(String.Format("This Document is generated by ", i, PageCount), font);
                                        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, ftr, 500, 40, 0);
                                    }
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 800, 15, 0);
                                    //if (i == 1)
                                    //    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 400, 550, 0);
                                }
                            }
                            //else
                            //{
                            //    var image = Image.GetInstance(logo);
                            //    image.SetAbsolutePosition(31, 794);
                            //    image.ScaleAbsolute(68f, 15f);

                            //    var draftimg = Image.GetInstance(draftImage);
                            //    draftimg.SetAbsolutePosition(20, 220);
                            //    draftimg.ScaleAbsolute(550f, 600f);

                            //    int PageCount = reader1.NumberOfPages;
                            //    for (int i = 1; i <= PageCount; i++)
                            //    {
                            //        var content = stamper.GetUnderContent(i);
                            //        if (docstatus == "D" || docstatus == "F")
                            //        {
                            //            content.AddImage(draftimg);
                            //        }
                            //        content.AddImage(image);
                            //        content.Rectangle(34.5, 28, 526, 60);


                            //        BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                            //        content.BeginText();
                            //        content.SetColorFill(CMYKColor.BLACK);
                            //        content.SetFontAndSize(baseFont1, 9);
                            //        content.CreateTemplate(20, 10);
                            //        content.SetLineWidth(25);

                            //        var txt = Details.Tables[0].Rows[0]["declar"].ToString();
                            //        string[] stringSeparators = new string[] { "\r\n" };
                            //        string text = txt;
                            //        string[] lines = text.Split(stringSeparators, StringSplitOptions.None);

                            //        var y = 65;
                            //        for (var j = 0; j < lines.Length; j++)
                            //        {
                            //            content.ShowTextAlignedKerned(PdfContentByte.ALIGN_LEFT, lines[j], 40, y, 0);
                            //            y = y - 10;
                            //        }

                            //        txt = Details.Tables[0].Rows[0]["inv_head"].ToString();
                            //        text = txt;
                            //        string[] lines1 = text.Split(stringSeparators, StringSplitOptions.None);

                            //        content.SetFontAndSize(baseFont1, 8);
                            //        y = 771;
                            //        for (var j = 0; j < lines1.Length; j++)
                            //        {
                            //            content.ShowTextAlignedKerned(PdfContentByte.ALIGN_CENTER, lines1[j], 300, y, 0);
                            //            y = y - 10;
                            //        }
                            //        content.SetFontAndSize(baseFont1, 9);
                            //        content.EndText();
                            //        //content.Rectangle(450, 25, 120, 35);
                            //        string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
                            //        Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);
                            //        Phrase ptitle = new Phrase(String.Format("Commercial Invoice", i, PageCount), fonttitle);
                            //        Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                            //        Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
                            //        //Phrase p7 = new Phrase(String.Format("Signature & date", i, PageCount), fontb);
                            //        Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
                            //        Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
                            //        //Phrase p1 = new Phrase(Details.Tables[0].Rows[0]["declar"].ToString(), font1);
                            //        //Phrase p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
                            //        //Phrase p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
                            //        //Phrase p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
                            //        /*------------------Header ---------------------------*/
                            //        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
                            //        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

                            //        /*------------------Header end---------------------------*/

                            //        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
                            //        //ColumnText.AddText(p1);
                            //        //content.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Details.Tables[0].Rows[0]["declar"].ToString(), 300, 400, 45);
                            //        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
                            //        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
                            //        //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
                            //        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                            //        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
                            //        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
                            //    }
                            //}
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
            Random rand = new Random();
            string path = Server.MapPath("~");
            string fileName = "QR_" + rand.Next(111111, 999999).ToString();
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

        /*--------------------------For PDF Print End--------------------------*/
        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                SalesQuotationModelattch _SalesQuotationModelattch = new SalesQuotationModelattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string SQ_No = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["SQ_No"] != null)
                //{
                //    SQ_No = Session["SQ_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _SalesQuotationModelattch.Guid = DocNo;
                GetCompDetail();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _SalesQuotationModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _SalesQuotationModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _SalesQuotationModelattch;
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
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
                    , string Flag, string Status, string Doc_no, string Doc_dt,string DocumentMenuId,string AmendmentFlag,string ddlrev_no)
        {
            try
            {
                GetCompDetail();
                DataTable dt = new DataTable();
                int QtyDigit = 0;
                if (DocumentMenuId== "105103145105")
                {
                    QtyDigit = Convert.ToInt32(Session["ExpImpQtyDigit"]);
                }
                else
                {
                    QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                if (Flag == "Quantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                            }
                        }
                    }
                    else
                    {
                        if(AmendmentFlag== "Amendment")
                        {
                            Flag = "";
                            Flag = AmendmentFlag;
                            dt = _DomesticSalesQuotation_ISERVICES.SQ_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                            Flag = "Quantity";
                        }
                        else
                        {
                            dt = _DomesticSalesQuotation_ISERVICES.SQ_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                        }
                       
                    }
                }
                else if (Flag == "SOrderQty")
                {
                    dt = _DomesticSalesQuotation_ISERVICES.SQ_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
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
        public string SavePdfDocToSendOnEmailAlert(string soNo, string soDate, string fileName, string PrintFormat,string docid, string docstatus)
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
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                if (ViewBag.PrintFormat == null)
                {
                    if (PrintFormat != null)
                    {
                        dt = PrintFormatDataTable(null, PrintFormat);
                    }
                }
                else
                {
                    dt = ViewBag.PrintFormat;
                }
                ViewBag.PrintOption = dt;
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(dt, soNo, soDate);
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
        public ActionResult SendEmailAlert(DomesticSalesQuotationModel _Model, string mail_id, string status, string docid, string Doc_no, string Doc_dt, string statusAM, string filepath)
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
                            dt = PrintFormatDataTable(_Model);
                            ViewBag.PrintOption = dt;
                            var data = GetPdfData(dt, Doc_no, Doc_dt);
                            string fileName = "SQ_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = commonCont.SaveAlertDocument_MailExt(data, fileName);
                           
                        }
                        message = _Common_IServices.SendAlertEmailExternal(CompID, BrchID, UserID, docid, Doc_no, "A", mail_id, filepath);
                        string keyword = @"\ExternalEmailAlertPDFs\";
                        int index = filepath.IndexOf(keyword);
                        file_path = (index >= 0) ? filepath.Substring(index) : filepath;
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
        //Added by Nidhi on 06-08-2025
        public string SavePdfDocToSendOnEmailAlert_Ext(string SQNo, string SQDate, string fileName, string PrintFormat)
        {
            DataTable dt = new DataTable();
            var commonCont = new CommonController(_Common_IServices);
            if (ViewBag.PrintFormat == null)
            {
                if (PrintFormat != null)
                {
                    dt = commonCont.PrintOptionsDt(PrintFormat);
                }
            }
            else
            {
                dt = ViewBag.PrintFormat;
            }
            ViewBag.PrintOption = dt;
            var data = GetPdfData(dt, SQNo, SQDate);
            return commonCont.SaveAlertDocument_MailExt(data, fileName);
        }
    }
}