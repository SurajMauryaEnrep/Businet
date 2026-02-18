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
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.CustomInvoice;
using EnRepMobileWeb.Resources;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.CustomInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.CustomInvoice
{
    public class CustomInvoiceController : Controller
    {
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string DocumentMenuId = "105103145127";
        string FromDate, title;
        DataTable dt;
        string CompID, UserID, language, BrchID = string.Empty;

        CustomInvoice_ISERVICE _CustomInvoice_ISERVICE;
        Common_IServices _Common_IServices;
        //string CompID, BrchID = String.Empty;


        public CustomInvoiceController(Common_IServices _Common_IServices, CustomInvoice_ISERVICE _LocalSalesInvoice_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._CustomInvoice_ISERVICE = _LocalSalesInvoice_ISERVICES;
        }
        public ActionResult CustomInvoiceList(SI_ListModel _SI_ListModel)
        {
            try
            {
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                DocumentMenuId = "105103145127";
                //    }                   
                //}
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
                CustomInvoice_Model _CustomInvoice_Model = new CustomInvoice_Model();
                //SI_ListModel _SI_ListModel = new SI_ListModel();
                //  GetCustList(_SI_ListModel, _CustomInvoice_Model, "L");
              
                GetStatusList(_SI_ListModel);
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _SI_ListModel.CustID = a[0].Trim();
                    _SI_ListModel.SI_FromDate = a[1].Trim();
                    _SI_ListModel.SI_ToDate = a[2].Trim();
                    _SI_ListModel.Status = a[3].Trim();
                    if (_SI_ListModel.Status == "0")
                    {
                        _SI_ListModel.Status = null;
                    }
                    _SI_ListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    _SI_ListModel.FromDate = _SI_ListModel.SI_FromDate;
                }
            //    _SI_ListModel.SIList = GetSI_DetailList(_SI_ListModel);
                if (_SI_ListModel.SI_FromDate == null)
                {
                    _SI_ListModel.FromDate = startDate;
                    _SI_ListModel.SI_FromDate = startDate;
                    _SI_ListModel.SI_ToDate = CurrentDate;
                }
                GetAllData(_SI_ListModel);
                _SI_ListModel.Title = title;
                //Session["LSISearch"] = "0";                
                _SI_ListModel.LSISearch = "0";
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomInvoice/CustomInvoiceList.cshtml", _SI_ListModel);
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
            string CustType = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string wfstatus = string.Empty;
            string Br_ID = string.Empty;
            try
            {

              
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(_SI_ListModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _SI_ListModel.CustName;
                }
                if (_SI_ListModel.WF_status != null)
                {
                    wfstatus = _SI_ListModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
                CustType = "E";          
                DataSet AllData = _CustomInvoice_ISERVICE.GetAllData(Comp_ID, CustomerName, Br_ID, CustType, _SI_ListModel.CustID, _SI_ListModel.SI_FromDate, _SI_ListModel.SI_ToDate, _SI_ListModel.Status, UserID, DocumentMenuId, wfstatus);
                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (DataRow data in AllData.Tables[0].Rows)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.cust_id = data["cust_id"].ToString();
                    _CustDetail.cust_name = data["cust_name"].ToString();
                    _CustList.Add(_CustDetail);
                }
                _CustList.Insert(0,new CustomerName() { cust_id="0",cust_name="All"});
                _SI_ListModel.CustomerNameList = _CustList;

                SetAllData(AllData, _SI_ListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public void SetAllData(DataSet dt, SI_ListModel _SI_ListModel)
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
                    _SIList.custom_inv_no = dr["custom_inv_no"].ToString();
                    _SIList.custom_inv_dt = dr["custom_inv_dt"].ToString();
                    _SIList.InvDate = dr["InvDt"].ToString();
                    _SIList.ship_no = dr["sh_no"].ToString();
                    _SIList.Ship_dt = dr["sh_date"].ToString();
                    _SIList.CustName = dr["cust_name"].ToString();
                    _SIList.Currency = dr["curr"].ToString();
                    _SIList.InvoiceValue = dr["val"].ToString();
                    _SIList.Stauts = dr["Status"].ToString();
                    _SIList.CreateDate = dr["CreateDate"].ToString();
                    _SIList.ApproveDate = dr["ApproveDate"].ToString();
                    _SIList.ModifyDate = dr["ModifyDate"].ToString();
                    _SIList.create_by = dr["create_by"].ToString();
                    _SIList.app_by = dr["app_by"].ToString();
                    _SIList.mod_by = dr["mod_by"].ToString();
                    _SIList.ass_val = dr["ass_val"].ToString();
                    _SalesInvoiceList.Add(_SIList);
                }
            }
            _SI_ListModel.SIList = _SalesInvoiceList;
        }
        public ActionResult AddNewCustomInvoice()
        {
            //Session["TransType"] = "Save";
            //Session["Command"] = "New";
            //Session["Message"] = "New";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            CustomInvoice_Model _CustomInvoice_Model = new CustomInvoice_Model();
            _CustomInvoice_Model.Command = "New";
            _CustomInvoice_Model.AppStatus = "D";
            _CustomInvoice_Model.TransType = "Save";
            _CustomInvoice_Model.BtnName = "BtnAddNew";
            
            UrlModel _urlModel = new UrlModel();
            _urlModel.Command = "New";
            _urlModel.TransType = "Save";
            _urlModel.BtnName = "BtnAddNew";
            CheckTaxRecoverabeAccount();
            if (ViewBag.TaxRecivableAccMsg == "N")
            {
                _CustomInvoice_Model.TransType = "Refresh";
                _CustomInvoice_Model.BtnName = "Refresh";
                _CustomInvoice_Model.Command = "Refresh";
                _urlModel.TransType = "Save";
                _urlModel.BtnName = "Refresh";
                _urlModel.Command = "New";
                
            }
            TempData["ModelData"] = _CustomInvoice_Model;
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("CustomInvoiceList");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("CustomInvoiceDetail", _urlModel);
        }
        private void CheckTaxRecoverabeAccount()
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
                DataSet GlDt = _CustomInvoice_ISERVICE.GetTaxRecivableAcc(CompID, BrchID);
                string TaxRecivableAccMsg = "Y";
                if (GlDt.Tables.Count > 0)
                {
                    if (GlDt.Tables[0].Rows.Count == 0)
                    {
                        TaxRecivableAccMsg = "N";
                    }
                   
                }
                ViewBag.TaxRecivableAccMsg = TaxRecivableAccMsg;
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
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
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                //        DocumentMenuId = "105103145127";
                //    }
                //}

                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                string DocumentName = ds.Tables[2].Rows[0]["pagename"].ToString();
                ViewBag.VBRoleList = ds.Tables[3];
                ViewBag.StatusList = ds.Tables[4];
                ViewBag.GstApplicableForExport = ds.Tables[5].Rows.Count > 0 ? ds.Tables[5].Rows[0]["param_stat"].ToString() : "";
                ViewBag.RateIncludingOC= ds.Tables[8].Rows[0]["param_stat"].ToString();

                #region Commented Code By Nitesh 24-05-2024 for Gst Applicable Param_id and table Name Changed
                #endregion
                //foreach (DataRow dr in ds.Tables[1].Rows) 
                //{
                //    //if (dr["param_id"].ToString() == "101")
                //    //{
                //    //    ViewBag.GstApplicable = dr["param_stat"].ToString();
                //    //}

                //    if(dr["param_id"].ToString()=="108")
                //    {
                //        ViewBag.RateIncludingOC = dr["param_stat"].ToString();
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
        public ActionResult CustomInvoiceDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                //        DocumentMenuId = "105103145127";
                //    }
                //}
                ViewBag.DocumentMenuId = DocumentMenuId;
                CommonPageDetails();
                CheckTaxRecoverabeAccount();
                
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                /*Add by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _urlModel.SI_Date) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _CustomInvoice_ModelS = TempData["ModelData"] as CustomInvoice_Model;
                if (_CustomInvoice_ModelS != null)
                {
                    // CustomInvoice_Model _CustomInvoice_ModelSS = new CustomInvoice_Model();
                    _CustomInvoice_ModelS.DocumentMenuId = DocumentMenuId;
                    SI_ListModel _SI_ListModel = new SI_ListModel();
                    GetCustList(_SI_ListModel, _CustomInvoice_ModelS, "D");
                    List<TaxCalciTaxName> _TaxName = new List<TaxCalciTaxName>();
                    TaxCalciTaxName _TaxNameList = new TaxCalciTaxName();
                    _TaxNameList.tax_name = "---Select---";
                    _TaxNameList.tax_id = "0";
                    _TaxName.Add(_TaxNameList);
                    _CustomInvoice_ModelS.TaxCalciTaxNameList = _TaxName;

                    //List<PortOfLoadingListModel> _PortOfLoadingListModel1 = new List<PortOfLoadingListModel>();
                    //PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
                    //PortOfLoadingLis.POL_Name = "---Select---";
                    //PortOfLoadingLis.POL_id = "0";
                    //_PortOfLoadingListModel1.Add(PortOfLoadingLis);
                    _CustomInvoice_ModelS.PortOfLoadingList = PortOfLoading();// _PortOfLoadingListModel1;

                    //List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                    //pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                    //_pi_rcpt_carrLis.Pi_Name = "---Select---";
                    //_pi_rcpt_carrLis.Pi_id = "0";
                    //_pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                    _CustomInvoice_ModelS.pi_rcpt_carrList = pi_rcpt_carr();// _pi_rcpt_carrListModel;
                    // For Trade term
                    //List<trade_termList> _TermLists = new List<trade_termList>();
                    //_TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                    //_TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    //_TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    //_TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    //_TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                    _CustomInvoice_ModelS.TradeTermsList = TradeTerm();// _TermLists;

                    DataTable dtbscurr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                    if (dtbscurr.Rows.Count > 0)
                    {
                        _CustomInvoice_ModelS.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                        ViewBag.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                    }
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"]));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"]));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpRateDigit"])); //
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _CustomInvoice_ModelS.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_CustomInvoice_ModelS.TransType == "Update" || _CustomInvoice_ModelS.TransType == "Edit")
                    {
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }

                        //string SINo = Session["SI_No"].ToString();
                        string SINo = _CustomInvoice_ModelS.SI_Number;
                        //string SIDate = Session["SI_Date"].ToString();                    
                        string SIDate = _CustomInvoice_ModelS.SI_Date;
                        DataSet ds = _CustomInvoice_ISERVICE.Edit_CIDetail(CompID, BrchID, SINo, SIDate, UserID, DocumentMenuId);

                        _CustomInvoice_ModelS.RoundOffSpec = ds.Tables[0].Rows[0]["roff_amt"].ToString();
                        _CustomInvoice_ModelS.custom_inv_no = ds.Tables[0].Rows[0]["custom_inv_no"].ToString();
                        _CustomInvoice_ModelS.InvoiceHeading = ds.Tables[0].Rows[0]["inv_head"].ToString();
                        _CustomInvoice_ModelS.BuyersOrderNumberAndDate = ds.Tables[0].Rows[0]["buyer_ord_no_dt"].ToString();
                        _CustomInvoice_ModelS.inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        _CustomInvoice_ModelS.inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                        _CustomInvoice_ModelS.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _CustomInvoice_ModelS.curr = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _CustomInvoice_ModelS.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();

                        _CustomInvoice_ModelS.ship_no = ds.Tables[0].Rows[0]["sh_no"].ToString();
                        _CustomInvoice_ModelS.ship_dt = ds.Tables[0].Rows[0]["sh_dt"].ToString();

                        _CustomInvoice_ModelS.ship_no = ds.Tables[0].Rows[0]["sh_no"].ToString();
                        //srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _Model.SrcDocNo, SrcDocnoVal = _Model.SrcDocNo });
                        //_Model.docNoLists = srcDocNoLists;
                        if (ds.Tables[0].Rows[0]["sh_dt"] != null && ds.Tables[0].Rows[0]["sh_dt"].ToString() != "")
                        {
                            _CustomInvoice_ModelS.ship_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["sh_dt"]).ToString("yyyy-MM-dd");
                        }


                        ViewBag.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _CustomInvoice_ModelS.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _CustomInvoice_ModelS.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["val"]).ToString(ValDigit);
                        _CustomInvoice_ModelS.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                        _CustomInvoice_ModelS.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["inv_amt"]).ToString(ValDigit);
                        _CustomInvoice_ModelS.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _CustomInvoice_ModelS.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _CustomInvoice_ModelS.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _CustomInvoice_ModelS.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _CustomInvoice_ModelS.AmmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _CustomInvoice_ModelS.AmmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _CustomInvoice_ModelS.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        _CustomInvoice_ModelS.inv_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _CustomInvoice_ModelS.TaxCalci_ItemName = ds.Tables[1].Rows[0]["item_name"].ToString();
                        _CustomInvoice_ModelS.TaxCalci_AssessableValue = ds.Tables[1].Rows[0]["item_gr_val_bs"].ToString();
                        _CustomInvoice_ModelS.SI_BillingAddress = ds.Tables[0].Rows[0]["bill_address"].ToString();
                        _CustomInvoice_ModelS.SI_Bill_Add_Id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                        _CustomInvoice_ModelS.SI_ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                        _CustomInvoice_ModelS.SI_Shipp_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                        _CustomInvoice_ModelS.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        _CustomInvoice_ModelS.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                        //Added for Other Detail by Suraj on 20-10-2023
                        _CustomInvoice_ModelS.pre_carr_by = ds.Tables[0].Rows[0]["pre_carr_by"].ToString();
                        _CustomInvoice_ModelS.pi_rcpt_carr = ds.Tables[0].Rows[0]["pi_rcpt_carr"].ToString();
                        _CustomInvoice_ModelS.ves_fli_no = ds.Tables[0].Rows[0]["ves_fli_no"].ToString();
                        _CustomInvoice_ModelS.loading_port = ds.Tables[0].Rows[0]["loading_port"].ToString();
                        _CustomInvoice_ModelS.discharge_port = ds.Tables[0].Rows[0]["discharge_port"].ToString();
                        _CustomInvoice_ModelS.fin_disti = ds.Tables[0].Rows[0]["fin_disti"].ToString();
                        _CustomInvoice_ModelS.container_no = ds.Tables[0].Rows[0]["container_no"].ToString();
                        _CustomInvoice_ModelS.other_ref = ds.Tables[0].Rows[0]["other_ref"].ToString();
                        _CustomInvoice_ModelS.term_del_pay = ds.Tables[0].Rows[0]["term_del_pay"].ToString();
                        _CustomInvoice_ModelS.des_good = ds.Tables[0].Rows[0]["des_good"].ToString();
                        _CustomInvoice_ModelS.prof_detail = ds.Tables[0].Rows[0]["prof_detail"].ToString();
                        _CustomInvoice_ModelS.declar = ds.Tables[0].Rows[0]["declar"].ToString();
                        _CustomInvoice_ModelS.CountryOfOriginOfGoods = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                        _CustomInvoice_ModelS.CountryOfFinalDestination = ds.Tables[0].Rows[0]["cntry_fin_dest"].ToString();
                        _CustomInvoice_ModelS.ExportersReference = ds.Tables[0].Rows[0]["ext_ref"].ToString();
                        _CustomInvoice_ModelS.BuyerIfOtherThenConsignee = ds.Tables[0].Rows[0]["buyer_consig"].ToString();
                        _CustomInvoice_ModelS.trade_term = ds.Tables[0].Rows[0]["trade_term"].ToString();
                        _CustomInvoice_ModelS.ConsigneeAddress = ds.Tables[0].Rows[0]["consig_addr"].ToString();
                        _CustomInvoice_ModelS.OcAmt = ds.Tables[0].Rows[0]["oc_amt"].ToString();
                        _CustomInvoice_ModelS.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                        _CustomInvoice_ModelS.NetValSpec = ds.Tables[0].Rows[0]["inv_amt_sp"].ToString();
                        _CustomInvoice_ModelS.AssVal = ds.Tables[0].Rows[0]["ass_val"].ToString();
                        _CustomInvoice_ModelS.ExporterAddress = ds.Tables[0].Rows[0]["exp_addr"].ToString();
                        _CustomInvoice_ModelS.ConsigneeName = ds.Tables[0].Rows[0]["consig_name"].ToString();
                        _CustomInvoice_ModelS.CustomInvDate = ds.Tables[0].Rows[0]["custom_inv_dt"].ToString();

                        _CustomInvoice_ModelS.VouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        ViewBag.OtherChargeDetails = ds.Tables[10];

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            if (doc_status == "A" || doc_status == "C")
                            {
                                _CustomInvoice_ModelS.GLVoucherType = ds.Tables[6].Rows[0]["vou_type"].ToString();
                            }
                            _CustomInvoice_ModelS.GLVoucherNo = ds.Tables[6].Rows[0]["vou_no"].ToString();
                            _CustomInvoice_ModelS.GLVoucherDt = ds.Tables[6].Rows[0]["vou_dt"].ToString();

                        }

                        _CustomInvoice_ModelS.Status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        _CustomInvoice_ModelS.DocumentStatus = doc_status;


                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            _CustomInvoice_ModelS.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _CustomInvoice_ModelS.BtnName = "Refresh";
                        }
                        else
                        {
                            _CustomInvoice_ModelS.CancelFlag = false;
                        }
                        if (ds.Tables[0].Rows[0]["non_taxable"].ToString().Trim() == "Y")
                        {
                            _CustomInvoice_ModelS.nontaxable = true;
                        }
                        else
                        {
                            _CustomInvoice_ModelS.nontaxable = false;
                        }

                        _CustomInvoice_ModelS.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        _CustomInvoice_ModelS.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _CustomInvoice_ModelS.Command != "Edit")
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

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _CustomInvoice_ModelS.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _CustomInvoice_ModelS.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _CustomInvoice_ModelS.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CustomInvoice_ModelS.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _CustomInvoice_ModelS.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CustomInvoice_ModelS.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CustomInvoice_ModelS.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CustomInvoice_ModelS.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _CustomInvoice_ModelS.BtnName = "Refresh";
                                }
                            }
                            if (doc_status == "QP")
                            {
                                //Session["BtnName"] = "Refresh";
                                _CustomInvoice_ModelS.BtnName = "Refresh";
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _CustomInvoice_ModelS.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.GLGroup = ds.Tables[6];
                        ViewBag.ItemTaxDetailsList = ds.Tables[8];
                        ViewBag.ItemTaxDetails = ds.Tables[2];

                        ViewBag.AttechmentDetails = ds.Tables[9];

                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomInvoice/CustomInvoiceDetail.cshtml", _CustomInvoice_ModelS);
                    }

                    else
                    {
                        
                        //Session["DocumentStatus"] = 'D';
                        _CustomInvoice_ModelS.DocumentStatus = "D";
                        _CustomInvoice_ModelS.Title = title;
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomInvoice/CustomInvoiceDetail.cshtml", _CustomInvoice_ModelS);
                    }
                }
                else
                {/*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
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
                    CustomInvoice_Model _CustomInvoice_Model = new CustomInvoice_Model();
                    _CustomInvoice_Model.DocumentMenuId = DocumentMenuId;
                    SI_ListModel _SI_ListModel = new SI_ListModel();
                    GetCustList(_SI_ListModel, _CustomInvoice_Model, "D");
                    List<TaxCalciTaxName> _TaxName = new List<TaxCalciTaxName>();
                    TaxCalciTaxName _TaxNameList = new TaxCalciTaxName();
                    _TaxNameList.tax_name = "---Select---";
                    _TaxNameList.tax_id = "0";
                    _TaxName.Add(_TaxNameList);
                    _CustomInvoice_Model.TaxCalciTaxNameList = _TaxName;

                    //List<PortOfLoadingListModel> _PortOfLoadingListModel1 = new List<PortOfLoadingListModel>();
                    //PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
                    //PortOfLoadingLis.POL_Name = "---Select---";
                    //PortOfLoadingLis.POL_id = "0";
                    //_PortOfLoadingListModel1.Add(PortOfLoadingLis);
                    _CustomInvoice_Model.PortOfLoadingList = PortOfLoading();// _PortOfLoadingListModel1;

                    //List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                    //pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                    //_pi_rcpt_carrLis.Pi_Name = "---Select---";
                    //_pi_rcpt_carrLis.Pi_id = "0";
                    //_pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                    _CustomInvoice_Model.pi_rcpt_carrList = pi_rcpt_carr();// _pi_rcpt_carrListModel;
                    // For Trade term
                    //List<trade_termList> _TermLists = new List<trade_termList>();
                    //_TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
                    //_TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
                    //_TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
                    //_TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
                    //_TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
                    _CustomInvoice_Model.TradeTermsList = TradeTerm();// _TermLists; //Commented by Suraj on 29-07-2024

                    DataTable dtbscurr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                    if (dtbscurr.Rows.Count > 0)
                    {
                        _CustomInvoice_Model.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                        ViewBag.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                    }
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpValDigit"]));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpQtyDigit"]));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["ExpImpRateDigit"])); //
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _CustomInvoice_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_urlModel != null)
                    {
                        _CustomInvoice_Model.Command = _urlModel.Command;
                        _CustomInvoice_Model.SI_Number = _urlModel.SI_Number;
                        _CustomInvoice_Model.SI_Date = _urlModel.SI_Date;
                        _CustomInvoice_Model.TransType = _urlModel.TransType;
                        _CustomInvoice_Model.Command = _urlModel.Command;
                        _CustomInvoice_Model.BtnName = _urlModel.BtnName;
                        _CustomInvoice_Model.WF_status1 = _urlModel.WF_status1;
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_CustomInvoice_Model.TransType == "Update" || _CustomInvoice_Model.TransType == "Edit")
                    {
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }

                        //string SINo = Session["SI_No"].ToString();
                        string SINo = _CustomInvoice_Model.SI_Number;
                        //string SIDate = Session["SI_Date"].ToString();                    
                        string SIDate = _CustomInvoice_Model.SI_Date;
                        DataSet ds = _CustomInvoice_ISERVICE.Edit_CIDetail(CompID, BrchID, SINo, SIDate, UserID, DocumentMenuId);

                        _CustomInvoice_Model.RoundOffSpec = ds.Tables[0].Rows[0]["roff_amt"].ToString();
                        _CustomInvoice_Model.custom_inv_no = ds.Tables[0].Rows[0]["custom_inv_no"].ToString();
                        _CustomInvoice_Model.InvoiceHeading = ds.Tables[0].Rows[0]["inv_head"].ToString();
                        _CustomInvoice_Model.BuyersOrderNumberAndDate = ds.Tables[0].Rows[0]["buyer_ord_no_dt"].ToString();
                        _CustomInvoice_Model.inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                        _CustomInvoice_Model.inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                        _CustomInvoice_Model.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                        _CustomInvoice_Model.curr = ds.Tables[0].Rows[0]["curr_name"].ToString();
                        _CustomInvoice_Model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();

                        _CustomInvoice_Model.ship_no = ds.Tables[0].Rows[0]["sh_no"].ToString();
                        _CustomInvoice_Model.ship_dt = ds.Tables[0].Rows[0]["sh_dt"].ToString();

                        _CustomInvoice_Model.ship_no = ds.Tables[0].Rows[0]["sh_no"].ToString();
                        //srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = _Model.SrcDocNo, SrcDocnoVal = _Model.SrcDocNo });
                        //_Model.docNoLists = srcDocNoLists;
                        if (ds.Tables[0].Rows[0]["sh_dt"] != null && ds.Tables[0].Rows[0]["sh_dt"].ToString() != "")
                        {
                            _CustomInvoice_Model.ship_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["sh_dt"]).ToString("yyyy-MM-dd");
                        }


                        ViewBag.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        _CustomInvoice_Model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        _CustomInvoice_Model.GrVal = Convert.ToDecimal(ds.Tables[0].Rows[0]["val"]).ToString(ValDigit);
                        _CustomInvoice_Model.TaxAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                        _CustomInvoice_Model.NetValBs = Convert.ToDecimal(ds.Tables[0].Rows[0]["inv_amt"]).ToString(ValDigit);
                        _CustomInvoice_Model.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        _CustomInvoice_Model.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        _CustomInvoice_Model.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        _CustomInvoice_Model.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        _CustomInvoice_Model.AmmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        _CustomInvoice_Model.AmmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        _CustomInvoice_Model.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        _CustomInvoice_Model.inv_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _CustomInvoice_Model.TaxCalci_ItemName = ds.Tables[1].Rows[0]["item_name"].ToString();
                        _CustomInvoice_Model.TaxCalci_AssessableValue = ds.Tables[1].Rows[0]["item_gr_val_bs"].ToString();
                        _CustomInvoice_Model.SI_BillingAddress = ds.Tables[0].Rows[0]["bill_address"].ToString();
                        _CustomInvoice_Model.SI_Bill_Add_Id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                        _CustomInvoice_Model.SI_ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                        _CustomInvoice_Model.SI_Shipp_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                        _CustomInvoice_Model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                        _CustomInvoice_Model.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();

                        //Added for Other Detail by Suraj on 20-10-2023
                        _CustomInvoice_Model.pre_carr_by = ds.Tables[0].Rows[0]["pre_carr_by"].ToString();
                        _CustomInvoice_Model.pi_rcpt_carr = ds.Tables[0].Rows[0]["pi_rcpt_carr"].ToString();
                        _CustomInvoice_Model.ves_fli_no = ds.Tables[0].Rows[0]["ves_fli_no"].ToString();
                        _CustomInvoice_Model.loading_port = ds.Tables[0].Rows[0]["loading_port"].ToString();
                        _CustomInvoice_Model.discharge_port = ds.Tables[0].Rows[0]["discharge_port"].ToString();
                        _CustomInvoice_Model.fin_disti = ds.Tables[0].Rows[0]["fin_disti"].ToString();
                        _CustomInvoice_Model.container_no = ds.Tables[0].Rows[0]["container_no"].ToString();
                        _CustomInvoice_Model.other_ref = ds.Tables[0].Rows[0]["other_ref"].ToString();
                        _CustomInvoice_Model.term_del_pay = ds.Tables[0].Rows[0]["term_del_pay"].ToString();
                        _CustomInvoice_Model.des_good = ds.Tables[0].Rows[0]["des_good"].ToString();
                        _CustomInvoice_Model.prof_detail = ds.Tables[0].Rows[0]["prof_detail"].ToString();
                        _CustomInvoice_Model.declar = ds.Tables[0].Rows[0]["declar"].ToString();
                        _CustomInvoice_Model.CountryOfOriginOfGoods = ds.Tables[0].Rows[0]["cntry_origin"].ToString();
                        _CustomInvoice_Model.CountryOfFinalDestination = ds.Tables[0].Rows[0]["cntry_fin_dest"].ToString();
                        _CustomInvoice_Model.ExportersReference = ds.Tables[0].Rows[0]["ext_ref"].ToString();
                        _CustomInvoice_Model.BuyerIfOtherThenConsignee = ds.Tables[0].Rows[0]["buyer_consig"].ToString();
                        _CustomInvoice_Model.trade_term = ds.Tables[0].Rows[0]["trade_term"].ToString();
                        _CustomInvoice_Model.ConsigneeAddress = ds.Tables[0].Rows[0]["consig_addr"].ToString();
                        _CustomInvoice_Model.OcAmt = ds.Tables[0].Rows[0]["oc_amt"].ToString();
                        _CustomInvoice_Model.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                        _CustomInvoice_Model.NetValSpec = ds.Tables[0].Rows[0]["inv_amt_sp"].ToString();
                        _CustomInvoice_Model.AssVal = ds.Tables[0].Rows[0]["ass_val"].ToString();
                        _CustomInvoice_Model.ExporterAddress = ds.Tables[0].Rows[0]["exp_addr"].ToString();
                        _CustomInvoice_Model.ConsigneeName = ds.Tables[0].Rows[0]["consig_name"].ToString();
                        _CustomInvoice_Model.CustomInvDate = ds.Tables[0].Rows[0]["custom_inv_dt"].ToString();

                        _CustomInvoice_Model.VouGlDetails = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        ViewBag.OtherChargeDetails = ds.Tables[10];

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            if (doc_status == "A" || doc_status == "C")
                            {
                                _CustomInvoice_Model.GLVoucherType = ds.Tables[6].Rows[0]["vou_type"].ToString();
                            }
                            _CustomInvoice_Model.GLVoucherNo = ds.Tables[6].Rows[0]["vou_no"].ToString();
                            _CustomInvoice_Model.GLVoucherDt = ds.Tables[6].Rows[0]["vou_dt"].ToString();

                        }

                        _CustomInvoice_Model.Status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        _CustomInvoice_Model.DocumentStatus = doc_status;


                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            _CustomInvoice_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _CustomInvoice_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _CustomInvoice_Model.CancelFlag = false;
                        }
                        if (ds.Tables[0].Rows[0]["non_taxable"].ToString().Trim() == "Y")
                        {
                            _CustomInvoice_Model.nontaxable = true;
                        }
                        else
                        {
                            _CustomInvoice_Model.nontaxable = false;
                        }


                        _CustomInvoice_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        _CustomInvoice_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _CustomInvoice_Model.Command != "Edit")
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

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _CustomInvoice_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _CustomInvoice_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _CustomInvoice_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CustomInvoice_Model.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _CustomInvoice_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CustomInvoice_Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CustomInvoice_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _CustomInvoice_Model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _CustomInvoice_Model.BtnName = "Refresh";
                                }
                            }
                            if (doc_status == "QP")
                            {
                                //Session["BtnName"] = "Refresh";
                                _CustomInvoice_Model.BtnName = "Refresh";
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _CustomInvoice_Model.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.GLGroup = ds.Tables[6];
                        ViewBag.ItemTaxDetailsList = ds.Tables[8];
                        ViewBag.ItemTaxDetails = ds.Tables[2];

                        ViewBag.AttechmentDetails = ds.Tables[9];
                      
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomInvoice/CustomInvoiceDetail.cshtml", _CustomInvoice_Model);
                    }

                    else
                    {
                        //Session["DocumentStatus"] = 'D';
                        _CustomInvoice_Model.DocumentStatus = "D";
                        _CustomInvoice_Model.Title = title;
                    
                        return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomInvoice/CustomInvoiceDetail.cshtml", _CustomInvoice_Model);
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
        public List<trade_termList> TradeTerm()
        {
            List<trade_termList> _TermLists = new List<trade_termList>();
            _TermLists.Insert(0, new trade_termList() { TrdTrms_id = "CFR", TrdTrms_val = "CFR" });
            _TermLists.Add(new trade_termList() { TrdTrms_id = "FOB", TrdTrms_val = "FOB" });
            _TermLists.Add(new trade_termList() { TrdTrms_id = "C&F", TrdTrms_val = "C&F" });
            _TermLists.Add(new trade_termList() { TrdTrms_id = "CIF", TrdTrms_val = "CIF" });
            _TermLists.Add(new trade_termList() { TrdTrms_id = "EXW", TrdTrms_val = "EXW" });
            return _TermLists;
        }
        public List<pi_rcpt_carrListModel> pi_rcpt_carr()
        {
            List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
            pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
            _pi_rcpt_carrLis.Pi_Name = "---Select---";
            _pi_rcpt_carrLis.Pi_id = "0";
            _pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
            return _pi_rcpt_carrListModel;
        }
        public List<PortOfLoadingListModel> PortOfLoading()
        {
            List<PortOfLoadingListModel> _PortOfLoadingListModel = new List<PortOfLoadingListModel>();
            PortOfLoadingListModel PortOfLoadingLis = new PortOfLoadingListModel();
            PortOfLoadingLis.POL_Name = "---Select---";
            PortOfLoadingLis.POL_id = "0";
            _PortOfLoadingListModel.Add(PortOfLoadingLis);
            return _PortOfLoadingListModel;
        }

        public ActionResult GetPortOfLoadingList()
        {
            try
            {
                string OrderType = string.Empty;
                JsonResult DataRows = null;
                DataTable PackListNumberDs = new DataTable();
                PackListNumberDs = _CustomInvoice_ISERVICE.PortOfLoadingList();
                DataRow Drow = PackListNumberDs.NewRow();
                Drow[1] = "---Select---";
                Drow[0] = "0";
                Drow[2] = "0";
                PackListNumberDs.Rows.InsertAt(Drow, 0);
                DataRows = Json(JsonConvert.SerializeObject(PackListNumberDs));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetPlOfReceiptByPreCarrierList()
        {
            try
            {
                string OrderType = string.Empty;
                JsonResult DataRows = null;
                DataTable PackListNumberDs = new DataTable();
                PackListNumberDs = _CustomInvoice_ISERVICE.PlOfReceiptByPreCarrierList();
                DataRow Drow = PackListNumberDs.NewRow();
                Drow[1] = "---Select---";
                Drow[0] = "0";
                Drow[2] = "0";
                PackListNumberDs.Rows.InsertAt(Drow, 0);
                DataRows = Json(JsonConvert.SerializeObject(PackListNumberDs));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult EditSI(string SIId, string SIDate, string ListFilterData, string WF_status)
        {
            /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
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
            //Session["Command"] = "Add";
            //Session["SI_No"] = SIId;
            //Session["SI_Date"] = SIDate;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            CustomInvoice_Model dblclick = new CustomInvoice_Model();
            UrlModel _urlModel = new UrlModel();
            dblclick.BtnName = "BtnToDetailPage";
            dblclick.TransType = "Update";
            dblclick.Command = "Update";
            dblclick.Message = "New"; ;
            dblclick.SI_Number = SIId;
            dblclick.SI_Date = SIDate;
            if (WF_status != null && WF_status != "")
            {
                dblclick.WF_status1 = WF_status;
                _urlModel.WF_status1 = WF_status;
            }
            dblclick.AppStatus = "D";
            TempData["ModelData"] = dblclick;
            _urlModel.Command = "Update";
            _urlModel.TransType = "Update";
            _urlModel.SI_Number = dblclick.SI_Number;
            _urlModel.SI_Date = dblclick.SI_Date;
            _urlModel.BtnName = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("CustomInvoiceDetail", _urlModel);
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
        public ActionResult SaleInvoiceBtnCommand(CustomInvoice_Model _CustomInvoice_Model, string command)
        {
            try
            {
                /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_CustomInvoice_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        CustomInvoice_Model _CustomInvoiceadd_Model = new CustomInvoice_Model();
                        _CustomInvoiceadd_Model.Message = "New";
                        _CustomInvoiceadd_Model.Command = "New";
                        //_CustomInvoiceadd_Model.AppStatus = "D";
                        _CustomInvoiceadd_Model.TransType = "Save";
                        _CustomInvoiceadd_Model.BtnName = "BtnAddNew";
                        
                        UrlModel _urlModel = new UrlModel();
                        _urlModel.Command = "New";
                        _urlModel.TransType = "Save";
                        _urlModel.BtnName = "BtnAddNew";
                        CheckTaxRecoverabeAccount();
                        if (ViewBag.TaxRecivableAccMsg == "N")
                        {
                            _urlModel.BtnName = "Refresh";
                            _urlModel.Command = "Refresh";
                            _urlModel.TransType = "Refresh";
                            _CustomInvoiceadd_Model.Command = "Refresh";
                            _CustomInvoiceadd_Model.TransType = "Refresh";
                            _CustomInvoiceadd_Model.BtnName = "Refresh";
                        }
                        TempData["ModelData"] = _CustomInvoiceadd_Model;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_CustomInvoice_Model.inv_no))
                                return RedirectToAction("EditSI", new { SIId = _CustomInvoice_Model.inv_no, SIDate = _CustomInvoice_Model.inv_dt, ListFilterData = _CustomInvoice_Model.ListFilterData1, WF_status = _CustomInvoice_Model.WFStatus });
                            else
                                _CustomInvoiceadd_Model.Command = "Refresh";
                            _CustomInvoiceadd_Model.TransType = "Refresh";
                            _CustomInvoiceadd_Model.BtnName = "Refresh";
                            _CustomInvoiceadd_Model.DocumentStatus = null;
                            TempData["ModelData"] = _CustomInvoiceadd_Model;
                            return RedirectToAction("CustomInvoiceDetail", "CustomInvoice", _CustomInvoiceadd_Model);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("CustomInvoiceDetail", "CustomInvoice", _urlModel);

                    case "Edit":
                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSI", new { SIId = _CustomInvoice_Model.inv_no, SIDate = _CustomInvoice_Model.inv_dt, ListFilterData = _CustomInvoice_Model.ListFilterData1, WF_status = _CustomInvoice_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string CInvDate = _CustomInvoice_Model.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, CInvDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSI", new { SIId = _CustomInvoice_Model.inv_no, SIDate = _CustomInvoice_Model.inv_dt, ListFilterData = _CustomInvoice_Model.ListFilterData1, WF_status = _CustomInvoice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _CustomInvoice_Model.Message = "New";
                        _CustomInvoice_Model.Command = command;
                        _CustomInvoice_Model.AppStatus = "D";
                        _CustomInvoice_Model.TransType = "Update";
                        _CustomInvoice_Model.BtnName = "BtnEdit";
                        _CustomInvoice_Model.SI_Number = _CustomInvoice_Model.inv_no;
                        _CustomInvoice_Model.SI_Date = _CustomInvoice_Model.inv_dt;
                        TempData["ModelData"] = _CustomInvoice_Model;
                        UrlModel _urlEditModel = new UrlModel();
                        _urlEditModel.Command = command;
                        _urlEditModel.TransType = "Update";
                        _urlEditModel.BtnName = "BtnEdit";
                        _urlEditModel.SI_Number = _CustomInvoice_Model.SI_Number;
                        _urlEditModel.SI_Date = _CustomInvoice_Model.SI_Date;
                        //}
                        TempData["ListFilterData"] = _CustomInvoice_Model.ListFilterData1;
                        return RedirectToAction("CustomInvoiceDetail", _urlEditModel);

                    case "Delete":
                        _CustomInvoice_Model.Command = command;
                        SIDelete(_CustomInvoice_Model, command);
                        CustomInvoice_Model _CustomInvoice_DeleteModel = new CustomInvoice_Model();
                        _CustomInvoice_DeleteModel.Message = "Deleted";
                        _CustomInvoice_DeleteModel.Command = "Refresh";
                        _CustomInvoice_DeleteModel.AppStatus = "D";
                        _CustomInvoice_DeleteModel.TransType = "Refresh";
                        _CustomInvoice_DeleteModel.BtnName = "BtnDelete";
                        TempData["ModelData"] = _CustomInvoice_DeleteModel;
                        UrlModel _urlDeleteModel = new UrlModel();
                        _urlDeleteModel.Command = "Refresh";
                        _urlDeleteModel.TransType = "Refresh";
                        _urlDeleteModel.BtnName = "BtnDelete";
                        TempData["ListFilterData"] = _CustomInvoice_Model.ListFilterData1;
                        return RedirectToAction("CustomInvoiceDetail", _urlDeleteModel);

                    case "Save":
                        _CustomInvoice_Model.Command = command;
                        SaveSIDetail(_CustomInvoice_Model);
                        if (_CustomInvoice_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_CustomInvoice_Model.Message == "DocModify")
                        {
                            _CustomInvoice_Model.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            CommonPageDetails();
                            List<CustomerName> _CustomerNameList = new List<CustomerName>();
                            CustomerName _CustomerName = new CustomerName();
                            _CustomerName.cust_name = _CustomInvoice_Model.CustName;
                            _CustomerName.cust_id = _CustomInvoice_Model.cust_id;
                            _CustomerNameList.Add(_CustomerName);
                            _CustomInvoice_Model.CustomerNameList = _CustomerNameList;

                            List<TaxCalciTaxName> _TaxName = new List<TaxCalciTaxName>();
                            TaxCalciTaxName _TaxNameList = new TaxCalciTaxName();
                            _TaxNameList.tax_name = "---Select---";
                            _TaxNameList.tax_id = "0";
                            _TaxName.Add(_TaxNameList);
                            _CustomInvoice_Model.TaxCalciTaxNameList = _TaxName;

                            //List<pi_rcpt_carrListModel> _pi_rcpt_carrListModel = new List<pi_rcpt_carrListModel>();
                            //pi_rcpt_carrListModel _pi_rcpt_carrLis = new pi_rcpt_carrListModel();
                            //_pi_rcpt_carrLis.Pi_Name = "---Select---";
                            //_pi_rcpt_carrLis.Pi_id = "0";
                            //_pi_rcpt_carrListModel.Add(_pi_rcpt_carrLis);
                            _CustomInvoice_Model.pi_rcpt_carrList = pi_rcpt_carr();// _pi_rcpt_carrListModel;
                            _CustomInvoice_Model.PortOfLoadingList = PortOfLoading();
                            _CustomInvoice_Model.TradeTermsList = TradeTerm();
                            DataTable dtbscurr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                            if (dtbscurr.Rows.Count > 0)
                            {
                                _CustomInvoice_Model.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                                ViewBag.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                            }

                             _CustomInvoice_Model.inv_dt = DateTime.Now.ToString();
                            _CustomInvoice_Model.SI_BillingAddress = _CustomInvoice_Model.SI_BillingAddress;
                            _CustomInvoice_Model.SI_ShippingAddress = _CustomInvoice_Model.SI_ShippingAddress;
                            _CustomInvoice_Model.curr = _CustomInvoice_Model.curr;
                            _CustomInvoice_Model.conv_rate = _CustomInvoice_Model.conv_rate;

                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                           
                            ViewBag.ItemTaxDetailsList = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];

                            ViewBag.GLGroup = ViewData["VouDetails"];

                            //ViewBag.CostCenterData = ViewData["CCdetail"];
                            //ViewBag.SubItemDetails = ViewData["SubItemdetail"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _CustomInvoice_Model.BtnName = "Refresh";
                            _CustomInvoice_Model.Command = "Refresh";
                            _CustomInvoice_Model.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomInvoice/CustomInvoiceDetail.cshtml", _CustomInvoice_Model);
                        }
                        else
                        {
                            TempData["ModelData"] = _CustomInvoice_Model;
                            UrlModel urlSaveModel = new UrlModel();
                            urlSaveModel.Command = "Update";
                            urlSaveModel.SI_Number = _CustomInvoice_Model.SI_Number;
                            urlSaveModel.SI_Date = _CustomInvoice_Model.SI_Date;
                            urlSaveModel.TransType = "Update";
                            urlSaveModel.BtnName = "Refresh";
                            urlSaveModel.AppStatus = "D";
                            TempData["ListFilterData"] = _CustomInvoice_Model.ListFilterData1;
                            return RedirectToAction("CustomInvoiceDetail", urlSaveModel);
                        }
                    case "Forward":
                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSI", new { SIId = _CustomInvoice_Model.inv_no, SIDate = _CustomInvoice_Model.inv_dt, ListFilterData = _CustomInvoice_Model.ListFilterData1, WF_status = _CustomInvoice_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string CInvDate1 = _CustomInvoice_Model.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, CInvDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSI", new { SIId = _CustomInvoice_Model.inv_no, SIDate = _CustomInvoice_Model.inv_dt, ListFilterData = _CustomInvoice_Model.ListFilterData1, WF_status = _CustomInvoice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 17-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditSI", new { SIId = _CustomInvoice_Model.inv_no, SIDate = _CustomInvoice_Model.inv_dt, ListFilterData = _CustomInvoice_Model.ListFilterData1, WF_status = _CustomInvoice_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string CInvDate2 = _CustomInvoice_Model.inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, CInvDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditSI", new { SIId = _CustomInvoice_Model.inv_no, SIDate = _CustomInvoice_Model.inv_dt, ListFilterData = _CustomInvoice_Model.ListFilterData1, WF_status = _CustomInvoice_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        // Session["Command"] = command;
                        _CustomInvoice_Model.Command = command;
                        SIListApprove(_CustomInvoice_Model, "", "");
                        TempData["ModelData"] = _CustomInvoice_Model;
                        UrlModel urlModel = new UrlModel();
                        urlModel.TransType = "Update";
                        urlModel.Command = "Update";
                        urlModel.SI_Number = _CustomInvoice_Model.SI_Number;
                        urlModel.SI_Date = _CustomInvoice_Model.SI_Date;
                        if (_CustomInvoice_Model.WF_status1 != null)
                        {
                            urlModel.WF_status1 = _CustomInvoice_Model.WF_status1;
                        }
                        urlModel.BtnName = "BtnEdit";
                        urlModel.AppStatus = "D";
                        TempData["ListFilterData"] = _CustomInvoice_Model.ListFilterData1;
                        return RedirectToAction("CustomInvoiceDetail", urlModel);

                    case "Refresh":
                        CustomInvoice_Model _CustomInvoice_RefreshModel = new CustomInvoice_Model();
                        _CustomInvoice_RefreshModel.BtnName = "Refresh";
                        _CustomInvoice_RefreshModel.Command = command;
                        _CustomInvoice_RefreshModel.TransType = "Save";
                        TempData["ModelData"] = _CustomInvoice_RefreshModel;
                        UrlModel urlRefresh_Model = new UrlModel();
                        urlRefresh_Model.BtnName = "Refresh";
                        urlRefresh_Model.Command = command;
                        urlRefresh_Model.TransType = "Save";
                        TempData["ListFilterData"] = _CustomInvoice_Model.ListFilterData1;
                        return RedirectToAction("CustomInvoiceDetail", urlRefresh_Model);

                    case "Print":
                        return GenratePdfFile(_CustomInvoice_Model);
                    case "BacktoList":
                        SI_ListModel _SI_ListModel = new SI_ListModel();
                        _SI_ListModel.WF_status = _CustomInvoice_Model.WF_status1;
                        TempData["ListFilterData"] = _CustomInvoice_Model.ListFilterData1;
                        return RedirectToAction("CustomInvoiceList", "CustomInvoice", _SI_ListModel);

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
        public ActionResult SaveSIDetail(CustomInvoice_Model _CustomInvoice_Model)
        {
            string SaveMessage = "";
            string PageName = _CustomInvoice_Model.Title.Replace(" ", "");
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
                if (Session["Userid"] != null)
                {
                    UserID = Session["Userid"].ToString();
                }

                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblVouGLDetail = new DataTable();
         
                 DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable dtheader = new DataTable();

                DtblHDetail = ToDtblHDetail(_CustomInvoice_Model);
                DtblItemDetail = ToDtblItemDetail(_CustomInvoice_Model.SI_ItemDetail);
                DtblVouGLDetail = ToDtblVouGlDetail(_CustomInvoice_Model.VouGlDetails);
                DtblTaxDetail = ToDtblTaxDetail(_CustomInvoice_Model.ItemTaxdetails);
                DtblOCDetail = ToDtblOCDetail(_CustomInvoice_Model.ItemOCdetails);


                DataTable dtAttachment = new DataTable();
                var _Cust_InvoiceModelattch = TempData["ModelDataattch"] as Cust_InvoiceModelattch;
                //TempData["ModelDataattch"] = null;
                if (_CustomInvoice_Model.attatchmentdetail != null)
                {
                    if (_Cust_InvoiceModelattch != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_Cust_InvoiceModelattch.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _Cust_InvoiceModelattch.AttachMentDetailItmStp as DataTable;
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
                        if (_CustomInvoice_Model.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _CustomInvoice_Model.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_CustomInvoice_Model.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_CustomInvoice_Model.inv_no))
                            {
                                dtrowAttachment1["id"] = _CustomInvoice_Model.inv_no;
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
                    // if (Session["TransType"].ToString() == "Update")
                    if (_CustomInvoice_Model.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string InvoiceCode = string.Empty;
                            if (!string.IsNullOrEmpty(_CustomInvoice_Model.inv_no))
                            {
                                InvoiceCode = _CustomInvoice_Model.inv_no;
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
                
                string Nurr = _CustomInvoice_Model.NarrationOnC + " " + $"_Inv_no _Inv_dt {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}" ;
                SaveMessage = _CustomInvoice_ISERVICE.InsertCI_Details(DtblHDetail, DtblItemDetail, DtblVouGLDetail, DtblTaxDetail
                    , DtblOCDetail, DtblAttchDetail, Nurr);
                if (SaveMessage == "DocModify")
                {
                    _CustomInvoice_Model.Message = "DocModify";
                    _CustomInvoice_Model.BtnName = "Refresh";
                    _CustomInvoice_Model.Command = "Refresh";
                    TempData["ModelData"] = _CustomInvoice_Model;
                    return RedirectToAction("SalesReturnDetail");
                }
                else
                {
                    string[] Data = SaveMessage.Split(',');

                    string SINo = Data[0];
                    if (SINo == "Data_Not_Found")
                    {
                        //var a = SaveMessage.Split(',');
                        var msg = SINo.Replace("_", " ") + " " + SaveMessage.Split(',')[1]+" in "+PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _CustomInvoice_Model.Message = SINo.Split(',')[0].Replace("_", "");
                        return RedirectToAction("CustomInvoiceDetail");
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
                        var _Cust_InvoiceModelattche = TempData["ModelDataattch"] as Cust_InvoiceModelattch;
                        TempData["ModelDataattch"] = null;
                        if (_Cust_InvoiceModelattche != null)
                        {
                            if (_Cust_InvoiceModelattche.Guid != null)
                            {
                                Guid = _Cust_InvoiceModelattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, SI_No, _CustomInvoice_Model.TransType, DtblAttchDetail);

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
                        //TempData["SI_No"] = _CustomInvoice_Model.inv_no;
                        //TempData["SI_Date"] = _CustomInvoice_Model.inv_dt;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "Refresh";
                        try
                        {
                            //string fileName = "PC_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "ExportInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_CustomInvoice_Model.inv_no, _CustomInvoice_Model.inv_dt, fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _CustomInvoice_Model.inv_no, "C", UserID, "0", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _CustomInvoice_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _CustomInvoice_Model.Message = _CustomInvoice_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        //_CustomInvoice_Model.Message = "Cancelled";
                        _CustomInvoice_Model.Command = "Update";
                        _CustomInvoice_Model.SI_Number = _CustomInvoice_Model.inv_no;
                        _CustomInvoice_Model.SI_Date = _CustomInvoice_Model.inv_dt;
                        _CustomInvoice_Model.TransType = "Update";
                        _CustomInvoice_Model.BtnName = "Refresh";
                        _CustomInvoice_Model.AppStatus = "D";
                        UrlModel urlModel = new UrlModel();
                        urlModel.Command = "Update";
                        urlModel.SI_Number = _CustomInvoice_Model.inv_no;
                        urlModel.SI_Date = _CustomInvoice_Model.inv_dt;
                        urlModel.TransType = "Update";
                        urlModel.BtnName = "Refresh";
                        urlModel.AppStatus = "D";
                        TempData["ModelData"] = _CustomInvoice_Model;
                        return RedirectToAction("CustomInvoiceDetail", urlModel);
                    }

                    if (Message == "Update" || Message == "Save")
                    {
                        //Session["Message"] = "Save";
                        //Session["Command"] = "Update";
                        //Session["SI_No"] = SINo;
                        //Session["SI_Date"] = SIDate;
                        //Session["TransType"] = "Update";                   
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnSave";
                        _CustomInvoice_Model.Message = "Save";
                        _CustomInvoice_Model.Command = "Update";
                        _CustomInvoice_Model.SI_Number = SINo;
                        _CustomInvoice_Model.SI_Date = SIDate;
                        _CustomInvoice_Model.TransType = "Update";
                        _CustomInvoice_Model.BtnName = "BtnSave";
                        _CustomInvoice_Model.AppStatus = "D";
                        TempData["ModelData"] = _CustomInvoice_Model;
                    }
                    return RedirectToAction("CustomInvoiceDetail");
                }
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_CustomInvoice_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        //{
                        //    Guid = Session["Guid"].ToString();
                        //}
                        if (_CustomInvoice_Model.Guid != null)
                        {
                            Guid = _CustomInvoice_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }

        private ActionResult SIDelete(CustomInvoice_Model _CustomInvoice_Model, string command)
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
                string doc_no = _CustomInvoice_Model.inv_no;
                string Message = _CustomInvoice_ISERVICE.Delete_CI_Detail(_CustomInvoice_Model, CompID, BrchID);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = _CustomInvoice_Model.Title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + br_id, PageName, doc_no1, Server);
                }

                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["SI_No"] = "";
                //_CustomInvoice_Model = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";

                return RedirectToAction("CustomInvoiceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //  return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private DataTable ToDtblHDetail(CustomInvoice_Model _CustomInvoice_Model)
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
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("inv_no", typeof(string));
                dtheader.Columns.Add("inv_dt", typeof(string));
                dtheader.Columns.Add("sh_no", typeof(string));
                dtheader.Columns.Add("sh_dt", typeof(string));
                dtheader.Columns.Add("cust_id", typeof(int));
                dtheader.Columns.Add("curr_id", typeof(int));
                dtheader.Columns.Add("conv_rate", typeof(double));
                dtheader.Columns.Add("user_id", typeof(int));
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("val", typeof(double));
                dtheader.Columns.Add("tax_amt", typeof(double));
                dtheader.Columns.Add("inv_amt", typeof(double));
                dtheader.Columns.Add("bill_add_id", typeof(string));
                dtheader.Columns.Add("ship_add_id", typeof(string));
                //Added for Other detail by Suraj on 20-10-2023
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
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("inv_amt_spec", typeof(string));
                dtheader.Columns.Add("ass_val_bs", typeof(string));
                dtheader.Columns.Add("ass_val_spec", typeof(string));
                dtheader.Columns.Add("custom_inv_no", typeof(string));
                dtheader.Columns.Add("inv_head", typeof(string));
                dtheader.Columns.Add("buyer_ord_no_dt", typeof(string));
                dtheader.Columns.Add("non_taxable", typeof(string));
                dtheader.Columns.Add("exp_addr", typeof(string));
                dtheader.Columns.Add("consig_name", typeof(string));
                dtheader.Columns.Add("custom_inv_dt", typeof(string));
                dtheader.Columns.Add("round_off_spec", typeof(string));
                

                DataRow dtrowHeader = dtheader.NewRow();
                if (_CustomInvoice_Model.inv_no != null)
                {
                    dtrowHeader["TransType"] = "Update";
                }
                else
                {
                    dtrowHeader["TransType"] = "Save";
                }
                dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                string cancelflag = _CustomInvoice_Model.CancelFlag.ToString();
                if (cancelflag == "False")
                {
                    dtrowHeader["Cancelled"] = "N";
                }
                else
                {
                    dtrowHeader["Cancelled"] = "Y";
                }
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["inv_no"] = _CustomInvoice_Model.inv_no;
                dtrowHeader["inv_dt"] = _CustomInvoice_Model.inv_dt;
                dtrowHeader["sh_no"] = _CustomInvoice_Model.ship_no;
                dtrowHeader["sh_dt"] = _CustomInvoice_Model.ship_dt;
                dtrowHeader["cust_id"] = _CustomInvoice_Model.cust_id;
                dtrowHeader["curr_id"] = _CustomInvoice_Model.curr_id;
                dtrowHeader["conv_rate"] = _CustomInvoice_Model.conv_rate;
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                dtrowHeader["inv_status"] = "D";
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["val"] = _CustomInvoice_Model.GrVal;
                dtrowHeader["tax_amt"] = IsNull(_CustomInvoice_Model.TaxAmt, "0");
                dtrowHeader["inv_amt"] = _CustomInvoice_Model.NetValBs;
                dtrowHeader["bill_add_id"] = _CustomInvoice_Model.SI_Bill_Add_Id;
                dtrowHeader["ship_add_id"] = _CustomInvoice_Model.SI_Shipp_Add_Id;

                //Added for Other detail by Suraj on 20-10-2023
                dtrowHeader["pre_carr_by"] = _CustomInvoice_Model.pre_carr_by;
                dtrowHeader["pi_rcpt_carr"] = _CustomInvoice_Model.pi_rcpt_carr;
                dtrowHeader["ves_fli_no"] = _CustomInvoice_Model.ves_fli_no;
                dtrowHeader["loading_port"] = _CustomInvoice_Model.loading_port;
                dtrowHeader["discharge_port"] = _CustomInvoice_Model.discharge_port;
                dtrowHeader["fin_disti"] = _CustomInvoice_Model.fin_disti;
                dtrowHeader["container_no"] = _CustomInvoice_Model.container_no;
                dtrowHeader["other_ref"] = _CustomInvoice_Model.other_ref;
                dtrowHeader["term_del_pay"] = _CustomInvoice_Model.term_del_pay;
                dtrowHeader["des_good"] = _CustomInvoice_Model.des_good;
                dtrowHeader["prof_detail"] = _CustomInvoice_Model.prof_detail;
                dtrowHeader["declar"] = _CustomInvoice_Model.declar;
                dtrowHeader["cntry_origin"] = _CustomInvoice_Model.CountryOfOriginOfGoods;
                dtrowHeader["cntry_fin_dest"] = _CustomInvoice_Model.CountryOfFinalDestination;
                dtrowHeader["ext_ref"] = _CustomInvoice_Model.ExportersReference;
                dtrowHeader["buyer_consig"] = _CustomInvoice_Model.BuyerIfOtherThenConsignee;
                dtrowHeader["trade_term"] = _CustomInvoice_Model.trade_term;
                dtrowHeader["consig_addr"] = _CustomInvoice_Model.ConsigneeAddress;
                dtrowHeader["oc_amt"] =IsNull(_CustomInvoice_Model.OcAmt,"0");
                dtrowHeader["inv_amt_spec"] =IsNull(_CustomInvoice_Model.NetValSpec, "0");
                dtrowHeader["ass_val_bs"] =IsNull(_CustomInvoice_Model.AssVal, "0");
                dtrowHeader["ass_val_spec"] =IsNull(_CustomInvoice_Model.AssValSpec, "0");
                dtrowHeader["custom_inv_no"] = _CustomInvoice_Model.custom_inv_no;
                dtrowHeader["inv_head"] = _CustomInvoice_Model.InvoiceHeading;
                dtrowHeader["buyer_ord_no_dt"] = _CustomInvoice_Model.BuyersOrderNumberAndDate;
                string NonTaxable = _CustomInvoice_Model.nontaxable.ToString();
                if (NonTaxable == "False")
                {
                    dtrowHeader["non_taxable"] = "N";
                }
                else
                {
                    dtrowHeader["non_taxable"] = "Y";
                }
                dtrowHeader["exp_addr"] = _CustomInvoice_Model.ExporterAddress;
                dtrowHeader["consig_name"] = _CustomInvoice_Model.ConsigneeName;
                dtrowHeader["custom_inv_dt"] = _CustomInvoice_Model.CustomInvDate;
                dtrowHeader["round_off_spec"] = IsNull(_CustomInvoice_Model.RoundOffSpec,"0");
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


                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("ship_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gr_val_sp", typeof(string));
                dtItem.Columns.Add("item_gr_val_bs", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_inv_amt_bs", typeof(string));
                dtItem.Columns.Add("item_oc_amt_bs", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("item_ass_rate", typeof(string));
                dtItem.Columns.Add("item_ass_val_bs", typeof(string));
                dtItem.Columns.Add("item_ass_val_spec", typeof(string));
                dtItem.Columns.Add("item_inv_amt_spec", typeof(string));

                JArray jObject = JArray.Parse(SIItemDetail);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["uom_id"].ToString();
                    dtrowLines["ship_qty"] = jObject[i]["ship_qty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    dtrowLines["item_gr_val_sp"] = jObject[i]["item_gr_val_sp"].ToString();
                    dtrowLines["item_gr_val_bs"] = jObject[i]["item_gr_val_bs"].ToString();
                    if (jObject[i]["item_tax_amt"].ToString() == "")
                    {
                        dtrowLines["item_tax_amt"] = "0";
                    }
                    else
                    {
                        dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                    }
                    dtrowLines["item_inv_amt_bs"] = jObject[i]["item_inv_amt_bs"].ToString();
                    dtrowLines["item_oc_amt_bs"] = jObject[i]["item_oc_amt_bs"].ToString();
                    dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                    dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtrowLines["item_ass_rate"] = jObject[i]["item_ass_rate"].ToString();
                    dtrowLines["item_ass_val_bs"] = jObject[i]["item_ass_val_bs"].ToString();
                    dtrowLines["item_ass_val_spec"] = jObject[i]["item_ass_val_spec"].ToString();
                    dtrowLines["item_inv_amt_spec"] = jObject[i]["item_inv_val_spec"].ToString();
            
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

                dtItem.Columns.Add("oc_id", typeof(int));
                dtItem.Columns.Add("oc_val", typeof(double));
                if (OCDetails != null)
                {
                    JArray jObject = JArray.Parse(OCDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["oc_id"] = jObject[i]["oc_id"].ToString();
                        dtrowLines["oc_val"] = jObject[i]["oc_val"].ToString();
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
        private DataTable ToDtblVouGlDetail(string VouGlDetails)
        {
            try
            {
                DataTable DtblVouGlDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("comp_id", typeof(int));
                dtItem.Columns.Add("id", typeof(double));
                dtItem.Columns.Add("type", typeof(int));
                dtItem.Columns.Add("doctype", typeof(string));
                dtItem.Columns.Add("Value", typeof(int));
                dtItem.Columns.Add("DrAmt", typeof(string));
                dtItem.Columns.Add("CrAmt", typeof(string));
                dtItem.Columns.Add("TransType", typeof(string));
                dtItem.Columns.Add("Gltype", typeof(string));

                JArray jObject = JArray.Parse(VouGlDetails);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["comp_id"] = jObject[i]["comp_id"].ToString();
                    dtrowLines["id"] = jObject[i]["id"].ToString();
                    dtrowLines["type"] = 'I';
                    dtrowLines["doctype"] = jObject[i]["doctype"].ToString();
                    dtrowLines["Value"] = jObject[i]["Value"].ToString();
                    dtrowLines["DrAmt"] = jObject[i]["DrAmt"].ToString();
                    dtrowLines["CrAmt"] = jObject[i]["CrAmt"].ToString();
                    dtrowLines["TransType"] = jObject[i]["TransType"].ToString();
                    dtrowLines["Gltype"] = jObject[i]["Gltype"].ToString();

                    dtItem.Rows.Add(dtrowLines);
                }
                DtblVouGlDetail = dtItem;
                ViewData["VouDetails"] = dtVoudetail(jObject);
                return DtblVouGlDetail;
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
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_alias", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("ship_qty", typeof(double));
            dtItem.Columns.Add("item_rate", typeof(double));
            dtItem.Columns.Add("item_gr_val_sp", typeof(double));
            dtItem.Columns.Add("item_gr_val_bs", typeof(double));
            dtItem.Columns.Add("item_tax_amt", typeof(double));
            dtItem.Columns.Add("item_inv_amt_bs", typeof(double));

            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("item_ass_rate", typeof(string));
            dtItem.Columns.Add("item_ass_val_bs", typeof(string));
            dtItem.Columns.Add("item_ass_val_sp", typeof(string));
            dtItem.Columns.Add("item_inv_amt_sp", typeof(string));



            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();
                dtrowLines["uom_id"] = jObject[i]["uom_id"].ToString();
                dtrowLines["uom_alias"] = jObject[i]["uom_name"].ToString();
                dtrowLines["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowLines["ship_qty"] = jObject[i]["ship_qty"].ToString();
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                dtrowLines["item_gr_val_sp"] = jObject[i]["item_gr_val_sp"].ToString();
                dtrowLines["item_gr_val_bs"] = jObject[i]["item_gr_val_bs"].ToString();
                if (jObject[i]["item_tax_amt"].ToString() == "")
                {
                    dtrowLines["item_tax_amt"] = "0";
                }
                else
                {
                    dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                }
                dtrowLines["item_inv_amt_bs"] = jObject[i]["item_inv_amt_bs"].ToString();

                dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt_bs"].ToString();
                dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                dtrowLines["item_ass_rate"] = jObject[i]["item_ass_rate"].ToString();
                dtrowLines["item_ass_val_bs"] = jObject[i]["item_ass_val_bs"].ToString();
                dtrowLines["item_ass_val_sp"] = jObject[i]["item_ass_val_spec"].ToString();
                dtrowLines["item_inv_amt_sp"] = jObject[i]["item_inv_val_spec"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtTaxdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

           
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
        public DataTable dtVoudetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("comp_id", typeof(string));
            dtItem.Columns.Add("acc_id", typeof(string));
            dtItem.Columns.Add("acc_name", typeof(string));
            dtItem.Columns.Add("type", typeof(string));
            dtItem.Columns.Add("doctype", typeof(string));
            dtItem.Columns.Add("Value", typeof(string));
            dtItem.Columns.Add("dr_amt_sp", typeof(string));
            dtItem.Columns.Add("cr_amt_sp", typeof(string));
            dtItem.Columns.Add("TransType", typeof(string));
            dtItem.Columns.Add("Gltype", typeof(string));

           for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowVoudetails = dtItem.NewRow();
                dtrowVoudetails["comp_id"] = jObject[i]["comp_id"].ToString();
                dtrowVoudetails["acc_id"] = jObject[i]["id"].ToString();
                dtrowVoudetails["acc_name"] = jObject[i]["acc_name"].ToString();
                dtrowVoudetails["type"] = jObject[i]["type"].ToString();
                dtrowVoudetails["doctype"] = jObject[i]["doctype"].ToString();
                dtrowVoudetails["Value"] = jObject[i]["Value"].ToString();
                dtrowVoudetails["dr_amt_sp"] = jObject[i]["DrAmt"].ToString();
                dtrowVoudetails["cr_amt_sp"] = jObject[i]["CrAmt"].ToString();
                dtrowVoudetails["TransType"] = jObject[i]["TransType"].ToString();
                dtrowVoudetails["Gltype"] = jObject[i]["Gltype"].ToString();
                dtItem.Rows.Add(dtrowVoudetails);
            }
            return dtItem;
        }
       
        private DataTable GetRoleList()
        {
            try
            {

                string UserID = string.Empty;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                //        DocumentMenuId = "105103145127";
                //    }
                //}
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
        public ActionResult GetCustomInvoiceList(string docid, string status)
        {
            // Session["MenuDocumentId"] = docid;
            SI_ListModel _SI_ListModel = new SI_ListModel();
            //Session["WF_status"] = status;
            _SI_ListModel.WF_status = status;
            return RedirectToAction("CustomInvoiceList", _SI_ListModel);
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

                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                dt = _CustomInvoice_ISERVICE.Get_CI_DetailList(Comp_ID, Br_ID, _SI_ListModel.CustID, _SI_ListModel.SI_FromDate, _SI_ListModel.SI_ToDate, _SI_ListModel.Status, UserID, DocumentMenuId, wfstatus);
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
                        _SIList.custom_inv_no = dr["custom_inv_no"].ToString();
                        _SIList.InvDate = dr["InvDt"].ToString();
                        _SIList.ship_no = dr["sh_no"].ToString();
                        _SIList.Ship_dt = dr["sh_date"].ToString();
                        _SIList.CustName = dr["cust_name"].ToString();
                        _SIList.Currency = dr["curr"].ToString();
                        _SIList.InvoiceValue = dr["val"].ToString();
                        _SIList.Stauts = dr["Status"].ToString();
                        _SIList.CreateDate = dr["CreateDate"].ToString();
                        _SIList.ApproveDate = dr["ApproveDate"].ToString();
                        _SIList.ModifyDate = dr["ModifyDate"].ToString();
                        _SIList.create_by = dr["create_by"].ToString();
                        _SIList.app_by = dr["app_by"].ToString();
                        _SIList.mod_by = dr["mod_by"].ToString();
                        _SIList.ass_val = dr["ass_val"].ToString();
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
        public ActionResult SearchSI_Detail(string CustId, string Fromdate, string Todate, string Status)
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
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                //        DocumentMenuId = "105103145127";
                //    }
                //}
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                dt = _CustomInvoice_ISERVICE.Get_CI_DetailList(Comp_ID, Br_ID, CustId, Fromdate, Todate, Status, null, null, "");
                //Session["LSISearch"] = "LSI_Search";
                _SI_ListModel.LSISearch = "LSI_Search";
                if (dt.Tables[0].Rows.Count > 0)
                {


                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        SalesInvoiceList _SIList = new SalesInvoiceList();
                        _SIList.InvoiceNo = dr["inv_no"].ToString();
                        _SIList.InvoiceDate = dr["InvDate"].ToString();
                        _SIList.InvDate = dr["InvDt"].ToString();
                        _SIList.ship_no = dr["sh_no"].ToString();
                        _SIList.Ship_dt = dr["sh_date"].ToString();
                        _SIList.CustName = dr["cust_name"].ToString();
                        _SIList.Currency = dr["curr"].ToString();
                        _SIList.InvoiceValue = dr["val"].ToString();
                        _SIList.Stauts = dr["Status"].ToString();
                        _SIList.CreateDate = dr["CreateDate"].ToString();
                        _SIList.ApproveDate = dr["ApproveDate"].ToString();
                        _SIList.ModifyDate = dr["ModifyDate"].ToString();
                        _SIList.create_by = dr["create_by"].ToString();
                        _SIList.app_by = dr["app_by"].ToString();
                        _SIList.mod_by = dr["mod_by"].ToString();
                        _SIList.custom_inv_no = dr["custom_inv_no"].ToString();
                        _SIList.custom_inv_dt = dr["custom_inv_dt"].ToString();
                        _SIList.ass_val = dr["ass_val"].ToString();
                        _SalesInvoiceList.Add(_SIList);

                    }
                }
                _SI_ListModel.SIList = _SalesInvoiceList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialCustomInvoiceList.cshtml", _SI_ListModel);
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

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        public ActionResult GetAutoCompleteCustomerList(CustomInvoice_Model _DSIModel)
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
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                //        DocumentMenuId = "105103145127";
                //    }
                //}

                CustomerType = "E";


                CustList = _CustomInvoice_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustomerType);

                List<CustomerName> _SuppList = new List<CustomerName>();
                foreach (var data in CustList)
                {
                    CustomerName _SuppDetail = new CustomerName();
                    _SuppDetail.cust_id = data.Key;
                    _SuppDetail.cust_name = data.Value;
                    _SuppList.Add(_SuppDetail);
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
        public ActionResult GetCustList(SI_ListModel _SI_ListModel, CustomInvoice_Model _CustomInvoice_Model, string type)
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
                    if (string.IsNullOrEmpty(_CustomInvoice_Model.CustName))
                    {
                        CustomerName = "0";
                    }
                    else
                    {
                        CustomerName = _CustomInvoice_Model.CustName;
                    }
                }

                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                CustType = "E";
                //    }
                //}                    

                CustList = _CustomInvoice_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType);
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
                    _CustomInvoice_Model.CustomerNameList = _CustList;
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
        //[HttpPost]
        // public ActionResult GetSIOCTypeList(CustomInvoice_Model _CustomInvoice_Model)
        // {
        //     try
        //     {
        //         List<OcCalciOtherCharge> _TaxCalciOCName = new List<OcCalciOtherCharge>();
        //         _CustomInvoice_Model = new CustomInvoice_Model();
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
        //         DataTable dt = _CustomInvoice_ISERVICE.GetOCList(Comp_ID, Br_ID);

        //         foreach (DataRow dr in dt.Rows)
        //         {
        //             OcCalciOtherCharge _OCName = new OcCalciOtherCharge();
        //             _OCName.oc_id = dr["oc_id"].ToString();
        //             _OCName.oc_name = dr["oc_name"].ToString();
        //             _TaxCalciOCName.Add(_OCName);

        //         }
        //         _CustomInvoice_Model.OcCalciOtherChargeList = _TaxCalciOCName;
        //         return PartialView("~/Areas/ApplicationLayer/Views/Shared/_OtherChargeNew.cshtml", _CustomInvoice_Model);
        //     }
        //     catch (Exception ex)
        //     {
        //         string path = Server.MapPath("~");
        //         Errorlog.LogError(path, ex);
        //         return Json("ErrorPage");
        //     }
        // }
        [HttpPost]
        public JsonResult GetTaxRecivable()
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
                DataSet GlDt = _CustomInvoice_ISERVICE.GetTaxRecivableAcc(Comp_ID, BranchID);
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
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                //        DocumentMenuId = "105103145127";
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
                DataSet result = _CustomInvoice_ISERVICE.GetShipmentList(Cust_id, Comp_ID, Br_ID);

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
        public JsonResult Getcurr_details(string ship_no, string ship_date)
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
                DataTable result = _CustomInvoice_ISERVICE.Getcurr_details(Comp_ID, Br_ID, ship_no, ship_date);

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
        public JsonResult GetShipmentDetails(string ShipmentNo, string ShipmentDate)
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
                DataSet result = _CustomInvoice_ISERVICE.GetShipmentDetail(ShipmentNo, ShipmentDate, Comp_ID, Br_ID);

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

        //public string CheckSaleReturnAgainstSaleInvoice(string DocNo, string DocDate)
        //{
        //    string str = "";
        //    try
        //    {
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
        //        DataSet Deatils = _CustomInvoice_ISERVICE.CheckSaleReturnAgainstSaleInvoice(Comp_ID, Br_ID, DocNo, DocDate);
        //        if (Deatils.Tables[0].Rows.Count > 0)
        //        {
        //            str = "Used";
        //        }
        //        if (Deatils.Tables[1].Rows.Count > 0)
        //        {
        //            str = "PaymentCreated";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        throw ex;
        //    }
        //    return str;
        //}
        public ActionResult getTaxDetailItemWise(string ItemID, string SelectedItemdetail, string ItemName, string AssAmount)
        {
            try
            {
                CustomInvoice_Model _CustomInvoice_Model = new CustomInvoice_Model();
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
                DataTable dt = _CustomInvoice_ISERVICE.GetTaxTypeList(Comp_ID, Br_ID);
                foreach (DataRow dr in dt.Rows)
                {
                    TaxCalciTaxName _TaxName = new TaxCalciTaxName();
                    _TaxName.tax_id = dr["tax_id"].ToString();
                    _TaxName.tax_name = dr["tax_name"].ToString();
                    _TaxCalciTaxName.Add(_TaxName);

                }
                _TaxCalciTaxName.Insert(0, new TaxCalciTaxName() { tax_id = "0", tax_name = "---Select---" });
                _CustomInvoice_Model.TaxCalciTaxNameList = _TaxCalciTaxName;
                _CustomInvoice_Model.TaxCalci_ItemName = ItemName;
                _CustomInvoice_Model.TaxCalci_AssessableValue = AssAmount;
                return PartialView("~/Areas/Common/Views/_TaxCalculationNew.cshtml", _CustomInvoice_Model);

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
            CustomInvoice_Model _CustomInvoice_Model = new CustomInvoice_Model();
            UrlModel URLModel = new UrlModel();//send Data by url  when forward 
            var a = TrancType.Split(',');
            _CustomInvoice_Model.SI_Number = a[0].Trim();
            _CustomInvoice_Model.SI_Date = a[1].Trim();
            _CustomInvoice_Model.DocumentMenuId = a[2].Trim();
            var WF_status1 = a[3].Trim();
            if (WF_status1 != null && WF_status1 != "")
            {
                _CustomInvoice_Model.WF_status1 = WF_status1;
                URLModel.WF_status1 = _CustomInvoice_Model.WF_status1;
            }
            _CustomInvoice_Model.TransType = "Update";
            _CustomInvoice_Model.BtnName = "BtnToDetailPage";
            _CustomInvoice_Model.Message = Mailerror;
            TempData["ModelData"] = _CustomInvoice_Model;
            TempData["ListFilterData"] = ListFilterData1;
            URLModel.SI_Number = _CustomInvoice_Model.SI_Number;
            URLModel.SI_Date = _CustomInvoice_Model.SI_Date;
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.DocumentMenuId = _CustomInvoice_Model.DocumentMenuId;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("CustomInvoiceDetail", URLModel);
        }
        //public ActionResult GetSIList(string docid, string status)
        //{
        //    Session["WF_Docid"] = docid;
        //    Session["WF_status"] = status;
        //    return RedirectToAction("CustomInvoiceList");
        //}
        public ActionResult SIListApprove(CustomInvoice_Model _CustomInvoice_Model, string ListFilterData1, string WF_status1)
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
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105103145127")
                //    {
                MenuID = DocumentMenuId;
                //    }
                //}
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string SI_No = _CustomInvoice_Model.inv_no;
                string SI_Date = _CustomInvoice_Model.inv_dt;
                string A_Status = _CustomInvoice_Model.A_Status;
                string A_Level = _CustomInvoice_Model.A_Level;
                string A_Remarks = _CustomInvoice_Model.A_Remarks;
                string Narration = _CustomInvoice_Model.Narration;

                string Message = _CustomInvoice_ISERVICE.Approve_CI(CompID, BrchID, SI_No, SI_Date, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks, Narration);
                string ApMessage = Message.Split(',')[1].Trim();
                string SINo = Message.Split(',')[0].Trim();
                string VouNo = Message.Split(',')[3].Trim();
                string VouDate = Message.Split(',')[4].Trim();
                string VouType = Message.Split(',')[5].Trim();
                if (ApMessage == "A")
                {
                    try
                    {
                        //string fileName = "CI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "ExportInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(SI_No, SI_Date, fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, SI_No, "AP", UserID, "0", filePath);
                        //Session["Message"] = "Approved";
                        //_CustomInvoice_Model.Message = "Approved";
                    }
                    catch (Exception exMail)
                    {
                        _CustomInvoice_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _CustomInvoice_Model.Message = _CustomInvoice_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                UrlModel urlModel = new UrlModel();
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                _CustomInvoice_Model.TransType = "Update";
                _CustomInvoice_Model.Command = "Approve";
                _CustomInvoice_Model.SI_Number = SINo;
                _CustomInvoice_Model.SI_Date = _CustomInvoice_Model.inv_dt;
                _CustomInvoice_Model.BtnName = "BtnEdit";
                _CustomInvoice_Model.AppStatus = "D";
                _CustomInvoice_Model.GLVoucherNo = VouNo;
                _CustomInvoice_Model.GLVoucherDt = VouDate;
                _CustomInvoice_Model.GLVoucherType = VouType;
                if (WF_status1 != null && WF_status1 != "")
                {
                    _CustomInvoice_Model.WF_status1 = WF_status1;
                    urlModel.WF_status1 = WF_status1;
                }
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";

                urlModel.TransType = "Update";
                urlModel.Command = "Update";
                urlModel.SI_Number = _CustomInvoice_Model.SI_Number;
                urlModel.SI_Date = _CustomInvoice_Model.SI_Date;
                urlModel.BtnName = "BtnEdit";
                urlModel.AppStatus = "D";

                TempData["ModelData"] = _CustomInvoice_Model;
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("CustomInvoiceDetail", urlModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1)
        {
            CustomInvoice_Model _CustomInvoice_Model = new CustomInvoice_Model();

            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _CustomInvoice_Model.inv_no = jObjectBatch[i]["SINo"].ToString();
                    _CustomInvoice_Model.inv_dt = jObjectBatch[i]["SIDate"].ToString();
                    _CustomInvoice_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _CustomInvoice_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _CustomInvoice_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                    _CustomInvoice_Model.Narration = jObjectBatch[i]["Narration"].ToString();
                }
            }
            if (WF_status1 != null && WF_status1 != "")
            {
                SIListApprove(_CustomInvoice_Model, ListFilterData1, WF_status1);
            }
            else
            {
                SIListApprove(_CustomInvoice_Model, ListFilterData1, "");
            }
            //SIListApprove(_CustomInvoice_Model, ListFilterData1, WF_status1);
            TempData["ListFilterData"] = ListFilterData1;
            TempData["ModelData"] = _CustomInvoice_Model;
            UrlModel urlModel = new UrlModel();
            urlModel.TransType = "Update";
            urlModel.Command = "Update";
            urlModel.SI_Number = _CustomInvoice_Model.SI_Number;
            urlModel.SI_Date = _CustomInvoice_Model.SI_Date;
            if (WF_status1 != null && WF_status1 != "")
            {
                urlModel.WF_status1 = WF_status1;
            }
            urlModel.SI_Date = _CustomInvoice_Model.SI_Date;
            urlModel.BtnName = "BtnEdit";
            urlModel.AppStatus = "D";
            return RedirectToAction("CustomInvoiceDetail", urlModel);
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
        public FileResult GenratePdfFile(CustomInvoice_Model _model)
        {
            try
            {
                var data = GetPdfData(_model.inv_no, _model.inv_dt);
                if (data != null)
                    return File(data, "application/pdf", /*ViewBag.Title.Replace(" ", "").*/"ExportInvoice" + ".pdf");
                else
                    return File("ErrorPage", "application/pdf");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return null;
                throw ex;
            }

        }
        public byte[] GetPdfData(string invNo, string invDt)
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
                string ReportType = "custom";
                DataSet Details = _CustomInvoice_ISERVICE.GetCustomInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt, ReportType);
                ViewBag.PageName = "CI";
                ViewBag.Title = "Custom Invoice";
                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                string htmlcontent = "";
                if (ReportType == "common")
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/CustomInvoice/CustomInvoicePrint.cshtml"));
                }
                else
                {
                    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Reports/CustomReports/AlaskaExports/AE_CostumInvoicePrint.cshtml"));
                }
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (ReportType == "common")
                    {
                        pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                    }
                    else
                    {
                        pdfDoc = new Document(PageSize.A4, 20f, 20f, 70f, 90f);
                    }
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = HeaderFooterPagination(bytes, Details, ReportType);

                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
               // return null;
            }
            finally
            {

            }
        }
        private Byte[] HeaderFooterPagination(Byte[] bytes, DataSet Details, string ReportType)
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
            //string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string logo = Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string draftImage = "";
            if (docstatus == "D" || docstatus == "F")
            {
                draftImage = Server.MapPath("~/Content/Images/draft.png");
            }
            else if (docstatus == "C")
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
                                if (docstatus == "D" || docstatus == "F" ||  docstatus == "C")
                                {
                                    content.AddImage(draftimg);
                                }
                                try
                                {
                                    var image = Image.GetInstance(logo);
                                    image.SetAbsolutePosition(31, 794);
                                    image.ScaleAbsolute(68f, 15f);
                                    content.AddImage(image);
                                }
                                catch { }
                                content.Rectangle(34.5, 28, 526, 60);

                                //-----------------Adding Declaration--------------
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
                                //-----------------Adding Declaration--------------


                                //content.Rectangle(450, 25, 120, 35);
                                string strdate = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
                                Phrase pdate = new Phrase(String.Format(strdate, i, PageCount), font);
                                Phrase ptitle = new Phrase(String.Format("Export Invoice", i, PageCount), fonttitle);
                                Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                Phrase p4 = new Phrase(String.Format("Declaration :", i, PageCount), fontb);
                                //Phrase p7 = new Phrase(String.Format("Signature & date", i, PageCount), fontb);
                                Phrase p8 = new Phrase(String.Format("For " + comp_nm, i, PageCount), fontb);
                                Phrase p7 = new Phrase(String.Format("Authorised Signatory", i, PageCount), fontb);
                                //Phrase p1 = new Phrase(String.Format("We declare that this invoice show the actual prices of the goods", i, PageCount), font1);
                                //Phrase p2 = new Phrase(String.Format("described and that all particulars are true and currect.", i, PageCount), font1);
                                //Phrase p3 = new Phrase(String.Format("'we intend to claim rewards under Merchandise Exports From India Scheme (MEIS)'", i, PageCount), font1);
                                /*------------------Header ---------------------------*/
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, pdate, 560, 794, 0);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ptitle, 300, 785, 0);

                                /*------------------Header end---------------------------*/

                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p4, 40, 75, 0);
                                //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p1, 40, 65, 0);
                                //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p2, 40, 55, 0);
                                //ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_LEFT, p3, 40, 45, 0);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p7, 555, 45, 0);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p8, 555, 70, 0);
                            }
                        }

                    }
                    bytes = ms.ToArray();
                }
            }

            return bytes;
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
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Cust_InvoiceModelattch _Cust_InvoiceModelattch = new Cust_InvoiceModelattch();
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
                //Session["Guid"] = SI_No;
                _Cust_InvoiceModelattch.Guid = DocNo;
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
                    _Cust_InvoiceModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _Cust_InvoiceModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _Cust_InvoiceModelattch;
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
                DataSet GlDt = _CustomInvoice_ISERVICE.GetAllGLDetails(DtblGLDetail);
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
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
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


        public FileResult ExportItemsToExcel(string invNo, string invDate)
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
            DataTable DtItems = _CustomInvoice_ISERVICE.GetCustomItemsToExportExcel(CompID, BrchID, invNo, invDate);
            var commonController = new CommonController(_Common_IServices);
            return commonController.ExportDatatableToExcel("CustomInvoice_Items", DtItems);
        }
    }
}