using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.ScrapSaleInvoice;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.ScrapSaleInvoice;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using ZXing;
using EnRepMobileWeb.Resources;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TransporterSetup;
using EnRepMobileWeb.Areas.BusinessLayer.Controllers.TransporterSetup;
using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;


namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.ScrapSaleInvoice
{
    public class ScrapSaleInvoiceController : Controller
    {
        List<ScrapSaleInvoiceList> _SSIList;
        string CompID, language, title, BrchID, UserID, create_id = string.Empty, crm="Y";
        string DocumentMenuId = "105103148";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ScrapSI_ISERVICE _ScrapSI_ISERVICE;
        private readonly TransporterSetup_ISERVICES _trptIService;
        CommonController cmn = new CommonController();
        

        public ScrapSaleInvoiceController(Common_IServices _Common_IServices, ScrapSI_ISERVICE _ScrapSI_ISERVICE,
           TransporterSetup_ISERVICES trptIService)
        {
            this._Common_IServices = _Common_IServices;
            this._ScrapSI_ISERVICE = _ScrapSI_ISERVICE;
            this._trptIService = trptIService;
        }
        // GET: ApplicationLayer/ScrapSaleInvoice
        public ActionResult ScrapSaleInvoice(ScrapSIListModel _SSIListModel)
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
                if (DocumentMenuId != null)
                {
                    _SSIListModel.wfdocid = DocumentMenuId;
                }
                else
                {
                    _SSIListModel.wfdocid = "0";
                }

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                #region Commented By Nitesh 08-04-2024 For All Data in one procedure 

                //  GetAutoCompleteSearchCustList(_SSIListModel);
                #endregion
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    if (PRData != null && PRData != "")
                    {
                        var a = PRData.Split(',');
                        _SSIListModel.CustID = a[0].Trim();
                        _SSIListModel.SSI_FromDate = a[1].Trim();
                        _SSIListModel.SSI_ToDate = a[2].Trim();
                        _SSIListModel.Status = a[3].Trim();
                        _SSIListModel.SQ_SalePerson = a[4].Trim();
                        if (_SSIListModel.Status == "0")
                        {
                            _SSIListModel.Status = null;

                        }
                        _SSIListModel.FromDate = _SSIListModel.SSI_FromDate;
                        _SSIListModel.ListFilterData = TempData["ListFilterData"].ToString();
                        #region Commented By Nitesh 08-04-2024 For All Data in one procedure 
                        //  _SSIListModel.SSIList = GetScrapSIList(_SSIListModel);
                        #endregion
                    }
                    else
                    {
                        #region Commented By Nitesh 08-04-2024 For All Data in one procedure 
                        //  _SSIListModel.SSIList = GetScrapSIList(_SSIListModel);
                        #endregion
                        _SSIListModel.FromDate = startDate;
                        _SSIListModel.SSI_FromDate = startDate;
                        _SSIListModel.SSI_ToDate = CurrentDate;
                    }
                }
                else
                {
                    List<CustomerName> _CustList = new List<CustomerName>();

                    _CustList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "---Select---" });
                    _SSIListModel.CustomerNameList = _CustList;
                    #region Commented By Nitesh 08-04-2024 For All Data in one procedure 
                    // _SSIListModel.SSIList = GetScrapSIList(_SSIListModel);
                    #endregion
                    _SSIListModel.FromDate = startDate;
                    _SSIListModel.SSI_FromDate = startDate;
                    _SSIListModel.SSI_ToDate = CurrentDate;
                }

                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _SSIListModel.StatusList = statusLists;
                GetAllData(_SSIListModel);
                _SSIListModel.Title = title;
                _SSIListModel.SSISearch = "0";
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceList.cshtml", _SSIListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(ScrapSIListModel _SSIListModel)
        {
            string CustomerName = string.Empty;
            string wfStatus = string.Empty;
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }

                if (string.IsNullOrEmpty(_SSIListModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _SSIListModel.CustName;
                }
                CustType = "D";
                if (_SSIListModel.WF_status != null)
                {
                    wfStatus = _SSIListModel.WF_status;
                }
                else
                {
                    wfStatus = null;
                }
                DataSet GetAllData = _ScrapSI_ISERVICE.GetAllData(Comp_ID, CustomerName, Br_ID, CustType
                    , UserID, _SSIListModel.CustID, _SSIListModel.SSI_FromDate, _SSIListModel.SSI_ToDate, _SSIListModel.Status, _SSIListModel.wfdocid, wfStatus, _SSIListModel.SQ_SalePerson);
                List<CustomerName> _CustmrList = new List<CustomerName>();
                foreach (DataRow data in GetAllData.Tables[0].Rows)
                {
                    CustomerName _SuppDetail = new CustomerName();
                    _SuppDetail.Cust_id = data["cust_id"].ToString();
                    _SuppDetail.Cust_name = data["cust_name"].ToString();
                    _CustmrList.Add(_SuppDetail);
                }
                _CustmrList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "All" });
                _SSIListModel.CustomerNameList = _CustmrList;

                List<SalePersonList> _SlPrsnList = new List<SalePersonList>();
                foreach (DataRow data in GetAllData.Tables[4].Rows)
                {
                    SalePersonList _SlPrsnDetail = new SalePersonList();
                    _SlPrsnDetail.salep_id = data["sls_pers_id"].ToString();
                    _SlPrsnDetail.salep_name = data["sls_pers_name"].ToString();
                    _SlPrsnList.Add(_SlPrsnDetail);
                }
                _SlPrsnList.Insert(0, new SalePersonList() { salep_id = "0", salep_name = "---Select---" });
                _SSIListModel.SalePersonList = _SlPrsnList;

                SetAllData(GetAllData, _SSIListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void SetAllData(DataSet DSet, ScrapSIListModel _SSIListModel)
        {
            List<ScrapSaleInvoiceList> _SSIList = new List<ScrapSaleInvoiceList>();
            if (DSet.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in DSet.Tables[1].Rows)
                {
                    ScrapSaleInvoiceList _ServicePIList = new ScrapSaleInvoiceList();
                    _ServicePIList.InvNo = dr["inv_no"].ToString();
                    _ServicePIList.InvDate = dr["inv_dt"].ToString();
                    _ServicePIList.SalesPerson = dr["SalesPerson"].ToString();
                    _ServicePIList.InvDt = dr["InvDt"].ToString();
                    _ServicePIList.InvValue = dr["net_val_bs"].ToString();
                    _ServicePIList.CustName = dr["cust_name"].ToString();
                    _ServicePIList.InvStauts = dr["InvStauts"].ToString();
                    _ServicePIList.CreateDate = dr["CreateDate"].ToString();
                    _ServicePIList.ApproveDate = dr["ApproveDate"].ToString();
                    _ServicePIList.ModifyDate = dr["ModifyDate"].ToString();
                    _ServicePIList.create_by = dr["create_by"].ToString();
                    _ServicePIList.app_by = dr["app_by"].ToString();
                    _ServicePIList.mod_by = dr["mod_by"].ToString();
                    _ServicePIList.cust_ref_no = dr["cust_ref_no"].ToString();
                    _SSIList.Add(_ServicePIList);
                }
            }
            _SSIListModel.SSIList = _SSIList;
            ViewBag.FinStDt = DSet.Tables[3].Rows[0]["findate"];
        }
        public ActionResult AddScrapSaleInvoiceDetail()
        {
            ScrapSIModel _ScrapSIModel = new ScrapSIModel();
            _ScrapSIModel.Message = "New";
            _ScrapSIModel.Command = "Add";
            _ScrapSIModel.AppStatus = "D";
            _ScrapSIModel.TransType = "Save";
            _ScrapSIModel.BtnName = "BtnAddNew";
            _ScrapSIModel.DocumentStatus = "D";
            TempData["ModelData"] = _ScrapSIModel;
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ScrapSaleInvoice");
            }
            /*End to chk Financial year exist or not*/
            ViewBag.DocumentStatus = "D";
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("ScrapSaleInvoiceDetail", "ScrapSaleInvoice");
        }
        public ActionResult ScrapSaleInvoiceDetail(URLModelDetails URLModel)
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
            /*Add by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.Sinv_dt) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                var _ScrapSIModel = TempData["ModelData"] as ScrapSIModel;
                if (_ScrapSIModel != null)
                {
                    //ScrapSIModel _ScrapSIModel = new ScrapSIModel();
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _ScrapSIModel.GstApplicable = ViewBag.GstApplicable;
                    //Session["SSISearch"] = null;
                    _ScrapSIModel.SSISearch = null;
                    _ScrapSIModel.UserID = UserID;

                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.RateDigit = RateDigit;
                    ViewBag.QtyDigit = QtyDigit;

                    List<CustomerName> _CustList = new List<CustomerName>();

                    _CustList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "---Select---" });
                    _ScrapSIModel.CustomerNameList = _CustList;
                    GetSalesPersonList(_ScrapSIModel);
                    _ScrapSIModel.Title = title;

                    _ScrapSIModel.TransList = GetTransporterList();

                    DataTable dtbscurr = _Common_IServices.GetBaseCurrency(CompID).Tables[0];
                    if (dtbscurr.Rows.Count > 0)
                    {
                        _ScrapSIModel.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                        ViewBag.bs_curr_id = dtbscurr.Rows[0]["bs_curr_id"].ToString();
                    }
                    //DataTable dt = GetTransporterList();
                    //List<TransListModel> _TransName = new List<TransListModel>();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    TransListModel _transdetail = new TransListModel();
                    //    _transdetail.TransId = dr["acc_id"].ToString();
                    //    _transdetail.TransName = dr["acc_name"].ToString();
                    //    _TransName.Add(_transdetail);
                    //}
                    //_TransName.Insert(0, new TransListModel() { TransId = "0", TransName = "---Select---" });
                    //_ScrapSIModel.TransList = _TransName;
                    if (_ScrapSIModel.Sinv_no == "" || _ScrapSIModel.Sinv_no == null)
                    {
                        DataSet dt1 = new DataSet();
                        BindDispatchDetail(_ScrapSIModel, "", dt1);
                    }

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ScrapSIModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _ScrapSIModel.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["Inv_No"] != null && Session["Inv_Date"] != null)
                    if (_ScrapSIModel.Sinv_no != null && _ScrapSIModel.Sinv_dt != null)
                    {
                        string Doc_no = _ScrapSIModel.Sinv_no;
                        string Doc_date = _ScrapSIModel.Sinv_dt;
                        //string Doc_no = Session["Inv_No"].ToString();
                        //string Doc_date = Session["Inv_Date"].ToString();

                        DataSet ds = GetScrapSaleInvoiceEdit(Doc_no, Doc_date);


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
                            ViewBag.ItemTCSDetails = ds.Tables[13];
                            ViewBag.SubItemDetails = ds.Tables[14];
                            ViewBag.ItemStockBatchWise = ds.Tables[15];
                            ViewBag.ItemOC_TDSDetails = ds.Tables[16];
                            ViewBag.ItemStockSerialWise = ds.Tables[17];
                            ViewBag.PaymentScheduleData = ds.Tables[22];
                            _ScrapSIModel.Sinv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _ScrapSIModel.Sinv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                            _ScrapSIModel.tcs_amt = ds.Tables[0].Rows[0]["tcs_amt"].ToString();

                            _ScrapSIModel.CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();
                            _ScrapSIModel.CustID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                            _CustList.Add(new CustomerName { Cust_id = _ScrapSIModel.CustID, Cust_name = _ScrapSIModel.CustName });
                            _ScrapSIModel.CustomerNameList = _CustList;

                            _ScrapSIModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _ScrapSIModel.Create_by = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _ScrapSIModel.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _ScrapSIModel.Approved_by = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _ScrapSIModel.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _ScrapSIModel.Amended_by = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _ScrapSIModel.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _ScrapSIModel.StatusName = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                            _ScrapSIModel.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            _ScrapSIModel.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                            _ScrapSIModel.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _ScrapSIModel.Address = ds.Tables[0].Rows[0]["CustAddress"].ToString();
                            _ScrapSIModel.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _ScrapSIModel.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _ScrapSIModel.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _ScrapSIModel.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _ScrapSIModel.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            _ScrapSIModel.bs_curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            
                            _ScrapSIModel.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _ScrapSIModel.Currency = ds.Tables[0].Rows[0]["curr_Name"].ToString();
                            _ScrapSIModel.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                            _ScrapSIModel.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                            _ScrapSIModel.SSIStatus = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            _ScrapSIModel.GR_No = ds.Tables[0].Rows[0]["gr_no"].ToString();
                            _ScrapSIModel.GR_Dt = ds.Tables[0].Rows[0]["gr_date"].ToString();
                            //_ScrapSIModel.GR_Dt = ds.Tables[0].Rows[0]["gr_dt"].ToString();
                            _ScrapSIModel.HdnGRDate = ds.Tables[0].Rows[0]["gr_date"].ToString();
                            _ScrapSIModel.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_name"].ToString();
                            _ScrapSIModel.Veh_Number = ds.Tables[0].Rows[0]["veh_number"].ToString();
                            _ScrapSIModel.Driver_Name = ds.Tables[0].Rows[0]["driver_name"].ToString();
                            _ScrapSIModel.Mob_No = ds.Tables[0].Rows[0]["mob_no"].ToString();
                            _ScrapSIModel.Tot_Tonnage = ds.Tables[0].Rows[0]["tot_tonnage"].ToString();
                            _ScrapSIModel.No_Of_Packages = ds.Tables[0].Rows[0]["no_of_pkgs"].ToString();
                            
                            _ScrapSIModel.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            _ScrapSIModel.CustRefNo = ds.Tables[0].Rows[0]["cust_ref_no"].ToString();
                            _ScrapSIModel.CustRefDt = ds.Tables[0].Rows[0]["cust_ref_dt"].ToString();
                            _ScrapSIModel.PlaceOfSupply = ds.Tables[0].Rows[0]["placeofsupply"].ToString();
                            // _ScrapSIModel.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            //_ScrapSIModel.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                            _ScrapSIModel.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                            _ScrapSIModel.slprsn_id = ds.Tables[0].Rows[0]["sls_per"].ToString();
                            _ScrapSIModel.Ship_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                            _ScrapSIModel.ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                            _ScrapSIModel.ShipTo = ds.Tables[0].Rows[0]["ship_to"].ToString();

                            BindDispatchDetail(_ScrapSIModel, "update", ds);

                            _ScrapSIModel.Address1 = ds.Tables[0].Rows[0]["disp_Addr1"].ToString();
                            _ScrapSIModel.Address2 = ds.Tables[0].Rows[0]["disp_Addr2"].ToString();
                            _ScrapSIModel.Country = ds.Tables[0].Rows[0]["disp_country"].ToString();
                            _ScrapSIModel.State = ds.Tables[0].Rows[0]["disp_state"].ToString();

                            _ScrapSIModel.District = ds.Tables[0].Rows[0]["disp_district"].ToString();
                            _ScrapSIModel.City = ds.Tables[0].Rows[0]["disp_city"].ToString();
                            _ScrapSIModel.Pin = ds.Tables[0].Rows[0]["disp_pin"].ToString();

                            if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            {
                                _ScrapSIModel.Hd_GstType = "Both";
                            }
                            else
                            {
                                _ScrapSIModel.Hd_GstType = "IGST";
                            }
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            ViewBag.Approve_id = approval_id;
                            string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                            string doc_status = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                            _ScrapSIModel.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                            if (roundoff_status == "Y")
                            {
                                _ScrapSIModel.RoundOffFlag = true;
                            }
                            else
                            {
                                _ScrapSIModel.RoundOffFlag = false;
                            }
                            _ScrapSIModel.DocumentStatus = doc_status;
                            _ScrapSIModel.ShipFromAddress = ds.Tables[0].Rows[0]["ship_from_addr"].ToString();
                            _ScrapSIModel.ddlPayment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                            _ScrapSIModel.ddlDelivery_term = ds.Tables[0].Rows[0]["deli_term"].ToString();
                            _ScrapSIModel.Declaration_1 = ds.Tables[0].Rows[0]["declar_1"].ToString();
                            _ScrapSIModel.Declaration_2 = ds.Tables[0].Rows[0]["declar_2"].ToString();
                            _ScrapSIModel.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();
                            _ScrapSIModel.pvt_mark = ds.Tables[0].Rows[0]["pvt_mark"].ToString();
                            _ScrapSIModel.Corporate_Address = ds.Tables[0].Rows[0]["corp_off_addr"].ToString();
                            string nontaxable = ds.Tables[0].Rows[0]["non_taxable"].ToString();
                            if (nontaxable == "Y")
                            {
                                _ScrapSIModel.nontaxable = true;
                            }
                            else
                            {
                                _ScrapSIModel.nontaxable = false;
                            }
                            ViewBag.DocumentCode = doc_status;
                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                if (doc_status == "A" || doc_status == "C")
                                {
                                    _ScrapSIModel.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                                }
                                _ScrapSIModel.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                                _ScrapSIModel.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                                ViewBag.GLVoucherNo = _ScrapSIModel.GLVoucherNo;/*add by Hina Sharma on 11-08-2025*/
                            }
                            ViewBag.DocumentStatus = doc_status;
                            //Session["DocumentStatus"] = doc_status;
                            _ScrapSIModel.DocumentStatus = doc_status;
                            if (doc_status == "C")
                            {
                                _ScrapSIModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                _ScrapSIModel.Cancelled = true;
                                //Session["BtnName"] = "Refresh";
                                _ScrapSIModel.BtnName = "Refresh";
                            }
                            else
                            {
                                _ScrapSIModel.Cancelled = false;
                            }
                            _ScrapSIModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _ScrapSIModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);


                            if (doc_status != "D" && doc_status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[6];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _ScrapSIModel.Command != "Edit")
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
                                        _ScrapSIModel.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ScrapSIModel.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ScrapSIModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ScrapSIModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ScrapSIModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (doc_status == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ScrapSIModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ScrapSIModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (doc_status == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ScrapSIModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "Refresh";
                                        _ScrapSIModel.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            //ViewBag.MenuPageName = getDocumentName();
                            getWarehouse(_ScrapSIModel);
                            _ScrapSIModel.Title = title;
                            _ScrapSIModel.DocumentMenuId = DocumentMenuId;

                            ViewBag.ItemDetails = ds.Tables[1];
                            //ViewBag.VBRoleList = GetRoleList();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            _ScrapSIModel._mdlCommand = _ScrapSIModel.Command;
                            _ScrapSIModel.hdnTransType = _ScrapSIModel.TransType;
                        }
                    }
                    else
                    {
                        _ScrapSIModel._mdlCommand = _ScrapSIModel.Command;
                        _ScrapSIModel.hdnTransType = _ScrapSIModel.TransType;
                        _ScrapSIModel.SourceType = "D";
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceDetail.cshtml", _ScrapSIModel);
                }
                else
                {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //  }
                    /*End to chk Financial year exist or not*/
                    ScrapSIModel _ScrapSIModel1 = new ScrapSIModel();
                    _ScrapSIModel1.UserID = UserID;
                    CommonPageDetails();
                    ViewBag.DocID = DocumentMenuId;
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _ScrapSIModel1.GstApplicable = ViewBag.GstApplicable;
                    _ScrapSIModel1.TransList = GetTransporterList();
                    //Session["SSISearch"] = null;
                    _ScrapSIModel1.SSISearch = null;
                    if (URLModel.Sinv_no != null || URLModel.Sinv_dt != null)
                    {
                        _ScrapSIModel1.Sinv_no = URLModel.Sinv_no;
                        _ScrapSIModel1.Sinv_dt = URLModel.Sinv_dt;
                    }
                    if (URLModel.TransType != null)
                    {
                        _ScrapSIModel1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _ScrapSIModel1.TransType = "New";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _ScrapSIModel1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _ScrapSIModel1.BtnName = "BtnRefresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _ScrapSIModel1.Command = URLModel.Command;
                    }
                    else
                    {
                        _ScrapSIModel1.Command = "Refresh";
                    }
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.RateDigit = RateDigit;
                    ViewBag.QtyDigit = QtyDigit;

                    List<CustomerName> _CustList = new List<CustomerName>();

                    _CustList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "---Select---" });
                    _ScrapSIModel1.CustomerNameList = _CustList;
                    GetSalesPersonList(_ScrapSIModel1);
                    if (_ScrapSIModel1.Sinv_no == "" || _ScrapSIModel1.Sinv_no == null)
                    {
                        DataSet dt1 = new DataSet();
                        BindDispatchDetail(_ScrapSIModel1, "", dt1);
                    }
                    _ScrapSIModel1.Title = title;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ScrapSIModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                    {
                        _ScrapSIModel1.WF_status1 = TempData["WF_status"].ToString();
                    }
                    //if (Session["Inv_No"] != null && Session["Inv_Date"] != null)
                    if (_ScrapSIModel1.Sinv_no != null && _ScrapSIModel1.Sinv_dt != null)
                    {
                        string Doc_no = _ScrapSIModel1.Sinv_no;
                        string Doc_date = _ScrapSIModel1.Sinv_dt;
                        //string Doc_no = Session["Inv_No"].ToString();
                        //string Doc_date = Session["Inv_Date"].ToString();

                        DataSet ds = GetScrapSaleInvoiceEdit(Doc_no, Doc_date);


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
                            ViewBag.ItemTCSDetails = ds.Tables[13];
                            ViewBag.SubItemDetails = ds.Tables[14];
                            ViewBag.ItemStockBatchWise = ds.Tables[15];
                            ViewBag.ItemOC_TDSDetails = ds.Tables[16];
                            ViewBag.ItemStockSerialWise = ds.Tables[17];
                            ViewBag.PaymentScheduleData = ds.Tables[22];
                            _ScrapSIModel1.Sinv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                            _ScrapSIModel1.Sinv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                            _ScrapSIModel1.tcs_amt = ds.Tables[0].Rows[0]["tcs_amt"].ToString();

                            _ScrapSIModel1.CustName = ds.Tables[0].Rows[0]["cust_name"].ToString();
                            _ScrapSIModel1.CustID = ds.Tables[0].Rows[0]["cust_id"].ToString();
                            _CustList.Add(new CustomerName { Cust_id = _ScrapSIModel1.CustID, Cust_name = _ScrapSIModel1.CustName });
                            _ScrapSIModel1.CustomerNameList = _CustList;

                            _ScrapSIModel1.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _ScrapSIModel1.Create_by = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _ScrapSIModel1.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _ScrapSIModel1.Approved_by = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _ScrapSIModel1.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _ScrapSIModel1.Amended_by = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _ScrapSIModel1.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _ScrapSIModel1.StatusName = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                            _ScrapSIModel1.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            _ScrapSIModel1.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                            _ScrapSIModel1.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            _ScrapSIModel1.Address = ds.Tables[0].Rows[0]["CustAddress"].ToString();
                            _ScrapSIModel1.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            _ScrapSIModel1.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            _ScrapSIModel1.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            _ScrapSIModel1.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            _ScrapSIModel1.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            _ScrapSIModel1.bs_curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                           _ScrapSIModel1.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            _ScrapSIModel1.Currency = ds.Tables[0].Rows[0]["curr_Name"].ToString();
                            _ScrapSIModel1.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                            _ScrapSIModel1.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                            _ScrapSIModel1.SSIStatus = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            _ScrapSIModel1.GR_No = ds.Tables[0].Rows[0]["gr_no"].ToString();
                            _ScrapSIModel1.GR_Dt = ds.Tables[0].Rows[0]["gr_date"].ToString();
                            //_ScrapSIModel1.GR_Dt = ds.Tables[0].Rows[0]["gr_dt"].ToString();
                            _ScrapSIModel1.HdnGRDate = ds.Tables[0].Rows[0]["gr_date"].ToString();
                            _ScrapSIModel1.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_name"].ToString();
                            _ScrapSIModel1.Veh_Number = ds.Tables[0].Rows[0]["veh_number"].ToString();
                            _ScrapSIModel1.Driver_Name = ds.Tables[0].Rows[0]["driver_name"].ToString();
                            _ScrapSIModel1.Mob_No = ds.Tables[0].Rows[0]["mob_no"].ToString();
                            _ScrapSIModel1.Tot_Tonnage = ds.Tables[0].Rows[0]["tot_tonnage"].ToString();
                            _ScrapSIModel1.No_Of_Packages = ds.Tables[0].Rows[0]["no_of_pkgs"].ToString();

                            _ScrapSIModel1.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            _ScrapSIModel1.CustRefNo = ds.Tables[0].Rows[0]["cust_ref_no"].ToString();
                            _ScrapSIModel1.CustRefDt = ds.Tables[0].Rows[0]["cust_ref_dt"].ToString();
                            _ScrapSIModel1.PlaceOfSupply = ds.Tables[0].Rows[0]["placeofsupply"].ToString();
                            // _ScrapSIModel1.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            //_ScrapSIModel1.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();//Commented by Suraj Maurya on 10-02-2025
                            _ScrapSIModel1.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                            _ScrapSIModel1.slprsn_id = ds.Tables[0].Rows[0]["sls_per"].ToString();
                            _ScrapSIModel1.Ship_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                            _ScrapSIModel1.ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                            _ScrapSIModel1.ShipTo = ds.Tables[0].Rows[0]["ship_to"].ToString();


                            BindDispatchDetail(_ScrapSIModel1, "update", ds);

                            _ScrapSIModel1.Address1 = ds.Tables[0].Rows[0]["disp_Addr1"].ToString();
                            _ScrapSIModel1.Address2 = ds.Tables[0].Rows[0]["disp_Addr2"].ToString();
                            _ScrapSIModel1.Country = ds.Tables[0].Rows[0]["disp_country"].ToString();
                            _ScrapSIModel1.State = ds.Tables[0].Rows[0]["disp_state"].ToString();

                            _ScrapSIModel1.District = ds.Tables[0].Rows[0]["disp_district"].ToString();
                            _ScrapSIModel1.City = ds.Tables[0].Rows[0]["disp_city"].ToString();
                            _ScrapSIModel1.Pin = ds.Tables[0].Rows[0]["disp_pin"].ToString();

                            if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            {
                                _ScrapSIModel1.Hd_GstType = "Both";
                            }
                            else
                            {
                                _ScrapSIModel1.Hd_GstType = "IGST";
                            }
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            ViewBag.Approve_id = approval_id;
                            string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                            string doc_status = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                            _ScrapSIModel1.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                            if (roundoff_status == "Y")
                            {
                                _ScrapSIModel1.RoundOffFlag = true;
                            }
                            else
                            {
                                _ScrapSIModel1.RoundOffFlag = false;
                            }
                            _ScrapSIModel1.ShipFromAddress = ds.Tables[0].Rows[0]["ship_from_addr"].ToString();
                            _ScrapSIModel1.ddlPayment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                            _ScrapSIModel1.ddlDelivery_term = ds.Tables[0].Rows[0]["deli_term"].ToString();
                            _ScrapSIModel1.Declaration_1 = ds.Tables[0].Rows[0]["declar_1"].ToString();
                            _ScrapSIModel1.Declaration_2 = ds.Tables[0].Rows[0]["declar_2"].ToString();
                            _ScrapSIModel1.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();
                            _ScrapSIModel1.Corporate_Address = ds.Tables[0].Rows[0]["corp_off_addr"].ToString();
                            _ScrapSIModel1.pvt_mark = ds.Tables[0].Rows[0]["pvt_mark"].ToString();
                            string nontaxable = ds.Tables[0].Rows[0]["non_taxable"].ToString();
                            if (nontaxable == "Y")
                            {
                                _ScrapSIModel1.nontaxable = true;
                            }
                            else
                            {
                                _ScrapSIModel1.nontaxable = false;
                            }
                            _ScrapSIModel1.DocumentStatus = doc_status;
                            ViewBag.DocumentCode = doc_status;
                            if (ds.Tables[7].Rows.Count > 0)
                            {
                                if (doc_status == "A" || doc_status == "C")
                                {
                                    _ScrapSIModel1.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                                }
                                _ScrapSIModel1.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                                _ScrapSIModel1.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                                ViewBag.GLVoucherNo = _ScrapSIModel1.GLVoucherNo;/*add by Hina Sharma on 11-08-2025*/
                            }
                            ViewBag.DocumentStatus = doc_status;
                            //Session["DocumentStatus"] = doc_status;
                            _ScrapSIModel1.DocumentStatus = doc_status;
                            if (doc_status == "C")
                            {
                                _ScrapSIModel1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                                _ScrapSIModel1.Cancelled = true;
                                //Session["BtnName"] = "Refresh";
                                _ScrapSIModel1.BtnName = "Refresh";
                            }
                            else
                            {
                                _ScrapSIModel1.Cancelled = false;
                            }
                            _ScrapSIModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _ScrapSIModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);


                            if (doc_status != "D" && doc_status != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[6];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _ScrapSIModel1.Command != "Edit")
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
                                        _ScrapSIModel1.BtnName = "Refresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ScrapSIModel1.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ScrapSIModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ScrapSIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ScrapSIModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (doc_status == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ScrapSIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ScrapSIModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (doc_status == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _ScrapSIModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "Refresh";
                                        _ScrapSIModel1.BtnName = "Refresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            getWarehouse(_ScrapSIModel1);
                            //ViewBag.MenuPageName = getDocumentName();
                            _ScrapSIModel1.Title = title;
                            _ScrapSIModel1.DocumentMenuId = DocumentMenuId;

                            ViewBag.ItemDetails = ds.Tables[1];
                            //ViewBag.VBRoleList = GetRoleList();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            _ScrapSIModel1._mdlCommand = _ScrapSIModel1.Command;
                            _ScrapSIModel1.hdnTransType = _ScrapSIModel1.TransType;
                        }
                    }
                    else
                    {
                        _ScrapSIModel1._mdlCommand = _ScrapSIModel1.Command;
                        _ScrapSIModel1.hdnTransType = _ScrapSIModel1.TransType;
                        _ScrapSIModel1.SourceType = "D";
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceDetail.cshtml", _ScrapSIModel1);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void BindDispatchDetail(ScrapSIModel _ScrapSIModel, string Flag, DataSet dt)
        {
            if (Flag == "update")
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
                _ScrapSIModel.countryList = _CmnCountryList;


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
                _ScrapSIModel.StateList = _CmnStateList;


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
                _ScrapSIModel.DistrictList = _CmnDistrictList;




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
                _ScrapSIModel.cityLists = _CmnCityList;
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
                getdata = _ScrapSI_ISERVICE.BindDispatchDetail(CompID, BrchID);

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
                _ScrapSIModel.countryList = _CmnCountryList;


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
                _ScrapSIModel.StateList = _CmnStateList;


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
                _ScrapSIModel.DistrictList = _CmnDistrictList;




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
                _ScrapSIModel.cityLists = _CmnCityList;


                if (_ScrapSIModel.Sinv_no == "" || _ScrapSIModel.Sinv_no == null)
                {
                    if (getdata.Tables[0].Rows.Count > 0)
                    {
                        _ScrapSIModel.Address1 = getdata.Tables[0].Rows[0]["comp_add"].ToString();
                        _ScrapSIModel.Country = getdata.Tables[0].Rows[0]["country_id"].ToString();
                        _ScrapSIModel.State = getdata.Tables[0].Rows[0]["state_id"].ToString();
                        _ScrapSIModel.District = getdata.Tables[0].Rows[0]["district_id"].ToString();
                        _ScrapSIModel.City = getdata.Tables[0].Rows[0]["city_id"].ToString();
                        _ScrapSIModel.Pin = getdata.Tables[0].Rows[0]["pin"].ToString();
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
                DataTable dt = _ScrapSI_ISERVICE.GetDistrictOnStateDDL(ddlStateID);
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
                DataTable dt = _ScrapSI_ISERVICE.GetCityOnDistrictDDL(ddlDistrictID);
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
        [NonAction]
        private void getWarehouse(ScrapSIModel _ScrapSIModel)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                List<Warehouse> _WarehouseList = new List<Warehouse>();
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet result = _Common_IServices.GetWarehouseList(Comp_ID, Br_ID);
                foreach (DataRow dr in result.Tables[0].Rows)
                {
                    Warehouse _Warehouse = new Warehouse();
                    _Warehouse.wh_id = dr["wh_id"].ToString();
                    _Warehouse.wh_name = dr["wh_name"].ToString();
                    _WarehouseList.Add(_Warehouse);
                }
                Warehouse _oWarehouse = new Warehouse();
                _oWarehouse.wh_id = "0";
                _oWarehouse.wh_name = "---Select---";
                _WarehouseList.Insert(0, _oWarehouse);
                _ScrapSIModel.WarehouseList = _WarehouseList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);

            }
        }
        public ActionResult GetInvoiceList(string docid, string status)
        {
            ScrapSIListModel _SSIListModel = new ScrapSIListModel();
            _SSIListModel.WF_status = status;
            //Session["WF_Docid"] = docid;
            return RedirectToAction("ScrapSaleInvoice", "ScrapSaleInvoice", _SSIListModel);
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

        public ActionResult GetAutoCompleteSearchCustList(ScrapSIListModel _ScrapSIListModel)
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
                if (string.IsNullOrEmpty(_ScrapSIListModel.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _ScrapSIListModel.CustName;
                }
                CustType = "D";

                CustList = _ScrapSI_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType);
                List<CustomerName> _CustmrList = new List<CustomerName>();
                foreach (var data in CustList)
                {
                    CustomerName _SuppDetail = new CustomerName();
                    _SuppDetail.Cust_id = data.Key;
                    _SuppDetail.Cust_name = data.Value;
                    _CustmrList.Add(_SuppDetail);
                }
                _ScrapSIListModel.CustomerNameList = _CustmrList;
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
        public JsonResult GetCustAddrDetail(string Cust_id,string DocumentMenuId)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                DataSet result = _ScrapSI_ISERVICE.GetCustAddrDetailDL(Cust_id, Comp_ID, br_id, DocumentMenuId);
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

        //[NonAction]
        //public DataTable GetTransporterList()
        //{
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
        //        DataTable dt = _ScrapSI_ISERVICE.GetTransporterList(Comp_ID,Br_ID);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        // return Json("ErrorPage");
        //        throw ex;
        //    }
        //}
        public List<TransListModel> GetTransporterList()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            var transController = new TransporterSetupController(_Common_IServices, _trptIService);
            var transList = transController.GetTransporterList(CompID);
            transList.RemoveAt(0);
            transList.Insert(0, new TransListModel { TransId = "0", TransName = "---Select--" });
            return transList;
        }
        public ActionResult DoubleClickOnList(string DocNo, string DocDate, string ListFilterData, string WF_status)
        {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
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
            URLModelDetails URLModel = new URLModelDetails();
            ScrapSIModel _ScrapSIModel = new ScrapSIModel();
            _ScrapSIModel.Message = "New";
            _ScrapSIModel.Command = "Update";
            _ScrapSIModel.TransType = "Update";
            _ScrapSIModel.BtnName = "BtnToDetailPage";
            _ScrapSIModel.Sinv_no = DocNo;
            _ScrapSIModel.Sinv_dt = DocDate;
            TempData["WF_status"] = WF_status;
            TempData["ListFilterData"] = ListFilterData;
            TempData["ModelData"] = _ScrapSIModel;
            URLModel.Sinv_no = DocNo;
            URLModel.Sinv_dt = DocDate;
            URLModel.TransType = "Update";
            URLModel.Command = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            return RedirectToAction("ScrapSaleInvoiceDetail", "ScrapSaleInvoice", URLModel);
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string MailError)
        {

            ScrapSIModel _ScrapSIModel = new ScrapSIModel();
            var a = TrancType.Split(',');
            _ScrapSIModel.Sinv_no = a[0].Trim();
            _ScrapSIModel.Sinv_dt = a[1].Trim();
            _ScrapSIModel.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            _ScrapSIModel.BtnName = "BtnToDetailPage";
            _ScrapSIModel.Message = MailError;
            URLModelDetails URLModel = new URLModelDetails();
            URLModel.Sinv_no = a[0].Trim();
            URLModel.Sinv_dt = a[01].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _ScrapSIModel;
            TempData["WF_status"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ScrapSaleInvoiceDetail", "ScrapSaleInvoice", URLModel);
        }
        [HttpPost]
        public JsonResult GetScrapVerifcationList(string Supp_id)
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
                DataSet result = _ScrapSI_ISERVICE.GetScrapVerifcationList(Supp_id, Comp_ID, Br_ID);

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
        public JsonResult GetScrapVerifcationDetails(string VerNo, string VerDate)
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
                DataSet result = _ScrapSI_ISERVICE.GetScrapVerifcationDetail(VerNo, VerDate, Comp_ID, Br_ID);

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
        public ActionResult ActionSSIDeatils(ScrapSIModel _ScrapSIModel, string command)
        {
            try
            {/*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ScrapSIModel.Delete == "Delete")
                {
                    command = "Delete";
                }
                if (_ScrapSIModel.Delete == "Edit")
                {
                    command = "Edit";
                }
                if (command == null)
                {
                    if (_ScrapSIModel.EditCommand == "UpdateTransPortDetail")
                    {
                        command = "UpdateTransPortDetail";
                    }
                }
                switch (command)
                {
                    case "AddNew":
                        ScrapSIModel _ScrapSIModelAddNew = new ScrapSIModel();
                        _ScrapSIModelAddNew.AppStatus = "D";
                        _ScrapSIModelAddNew.BtnName = "BtnAddNew";
                        _ScrapSIModelAddNew.TransType = "Save";
                        _ScrapSIModelAddNew.Command = "New";
                        ViewBag.DocumentStatus = "D";
                        TempData["ModelData"] = _ScrapSIModelAddNew;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ScrapSIModel.Sinv_no))
                                return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                            else
                                _ScrapSIModelAddNew.Command = "Refresh";
                                _ScrapSIModelAddNew.TransType = "Refresh";
                                _ScrapSIModelAddNew.BtnName = "Refresh";
                                _ScrapSIModelAddNew.DocumentStatus = null;
                                TempData["ModelData"] = _ScrapSIModelAddNew;
                                return RedirectToAction("ScrapSaleInvoiceDetail", "ScrapSaleInvoice");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ScrapSaleInvoiceDetail", "ScrapSaleInvoice");
                    case "UpdateTransPortDetail":
                        /*start Add by Hina on 16-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SSInvDate4 = _ScrapSIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SSInvDate4) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        URLModelDetails URLeditModel2 = new URLModelDetails();
                        _ScrapSIModel.TransType = "Update";
                        _ScrapSIModel.Command = command;
                        _ScrapSIModel.BtnName = "BtnEdit";
                        _ScrapSIModel.Sinv_no = _ScrapSIModel.Sinv_no;
                        _ScrapSIModel.Sinv_dt = _ScrapSIModel.Sinv_dt;
                        _ScrapSIModel.DocumentMenuId = _ScrapSIModel.DocumentMenuId;
                       
                        //_ShipmentDetail_MODEL.CustType = _ShipmentDetail_MODEL.CustType;
                        TempData["ModelData"] = _ScrapSIModel;
                        URLeditModel2.TransType = _ScrapSIModel.TransType;
                        URLeditModel2.Command = command;
                        URLeditModel2.BtnName = "BtnEdit";
                        URLeditModel2.Sinv_no = _ScrapSIModel.Sinv_no;
                        URLeditModel2.Sinv_dt = _ScrapSIModel.Sinv_dt;
                        TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                        return RedirectToAction("ScrapSaleInvoiceDetail", URLeditModel2);
                    case "Edit":
                        _ScrapSIModel.EditCommand = null;
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        URLModelDetails URLModelEdit = new URLModelDetails();
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SSInvDate = _ScrapSIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SSInvDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/

                        if (_ScrapSIModel.SSIStatus == "A")
                        {
                            string checkforCancle = CheckSSIForCancellationinVoucher(_ScrapSIModel.Sinv_no, _ScrapSIModel.Sinv_dt);
                            if (checkforCancle != "")
                            {
                                _ScrapSIModel.Delete = null;
                                //Session["Message"] = checkforCancle;
                                _ScrapSIModel.Message = checkforCancle;
                                _ScrapSIModel.BtnName = "BtnToDetailPage";
                                _ScrapSIModel.Command = "Refresh";
                                URLModelEdit.Command = "Refresh";
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.Sinv_no = _ScrapSIModel.Sinv_no;
                                URLModelEdit.Sinv_dt = _ScrapSIModel.Sinv_dt;
                                TempData["ModelData"] = _ScrapSIModel;
                                TempData["FilterData"] = _ScrapSIModel.ListFilterData1;
                            }
                            else
                            {
                                
                                _ScrapSIModel.TransType = "Update";
                                _ScrapSIModel.Command = command;
                                _ScrapSIModel.BtnName = "BtnEdit";
                                TempData["ModelData"] = _ScrapSIModel;
                                URLModelEdit.TransType = "Update";
                                URLModelEdit.Command = command;
                                URLModelEdit.BtnName = "BtnEdit";
                                URLModelEdit.Sinv_no = _ScrapSIModel.Sinv_no;
                                URLModelEdit.Sinv_dt = _ScrapSIModel.Sinv_dt;
                                TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                            }
                        }
                        else
                        {
                            _ScrapSIModel.TransType = "Update";
                            _ScrapSIModel.Command = command;
                            _ScrapSIModel.BtnName = "BtnEdit";
                            TempData["ModelData"] = _ScrapSIModel;
                            URLModelEdit.TransType = "Update";
                            URLModelEdit.Command = command;
                            URLModelEdit.BtnName = "BtnEdit";
                            URLModelEdit.Sinv_no = _ScrapSIModel.Sinv_no;
                            URLModelEdit.Sinv_dt = _ScrapSIModel.Sinv_dt;
                            TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                        }
                        return RedirectToAction("ScrapSaleInvoiceDetail", URLModelEdit);
                    case "Delete":
                        ScrapSIModel _ScrapSIModelDelete = new ScrapSIModel();
                        _ScrapSIModel.Command = command;
                        _ScrapSIModel.BtnName = "Refresh";
                        //Inv_No = _ScrapSIModel.Sinv_no;
                        ScrapPIDelete(_ScrapSIModel, command, _ScrapSIModel.Title);
                        _ScrapSIModelDelete.Message = "Deleted";
                        _ScrapSIModelDelete.Command = "Refresh";
                        _ScrapSIModelDelete.TransType = "Refresh";
                        _ScrapSIModelDelete.AppStatus = "DL";
                        _ScrapSIModelDelete.BtnName = "BtnDelete";
                        _ScrapSIModelDelete.SourceType = "D";
                        TempData["ModelData"] = _ScrapSIModelDelete;
                        TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                        return RedirectToAction("ScrapSaleInvoiceDetail");

                    case "Save":
                        //Session["Command"] = command;
                        _ScrapSIModel.Command = command;
                        if (_ScrapSIModel.TransType == null)
                        {
                            _ScrapSIModel.TransType = command;
                        }
                        if (_ScrapSIModel.EditCommand == "UpdateTransPortDetail")
                        {
                            _ScrapSIModel.TransType = "UpdateTransPort";
                        }
                        string checkforCancleOnSave = CheckSSIForCancellationinVoucher(_ScrapSIModel.Sinv_no, _ScrapSIModel.Sinv_dt);
                        if (checkforCancleOnSave != "")
                        {
                            _ScrapSIModel.Message = checkforCancleOnSave;
                            _ScrapSIModel.BtnName = "BtnToDetailPage";
                            TempData["ModelData"] = _ScrapSIModel;
                            TempData["FilterData"] = _ScrapSIModel.ListFilterData1;
                        }
                        else
                        {
                            SaveScrapSaleInvoice(_ScrapSIModel);
                        }
                        
                        if (_ScrapSIModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_ScrapSIModel.Message == "DocModify")
                        {
                            DocumentMenuId = _ScrapSIModel.DocumentMenuId;
                            CommonPageDetails();
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";
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

                            List<CustomerName> suppLists = new List<CustomerName>();
                            suppLists.Add(new CustomerName { Cust_id = _ScrapSIModel.CustID, Cust_name = _ScrapSIModel.CustName });
                            _ScrapSIModel.CustomerNameList = suppLists;

                            List<SourceDoc> _DocumentNumberList = new List<SourceDoc>();
                            SourceDoc _DocumentNumber = new SourceDoc();
                            _DocumentNumber.doc_no = _ScrapSIModel.SrcDocNo;
                            _DocumentNumber.doc_dt = "0";
                            _DocumentNumberList.Add(_DocumentNumber);
                            _ScrapSIModel.SourceDocList = _DocumentNumberList;

                            List<CurrancyList> currancyLists = new List<CurrancyList>();
                            currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                            _ScrapSIModel.currancyLists = currancyLists;
                            _ScrapSIModel.Title = title;

                            DataSet dt1 = new DataSet();
                            BindDispatchDetail(_ScrapSIModel, "", dt1);
                            _ScrapSIModel.Sinv_dt = DateTime.Now.ToString();
                            _ScrapSIModel.bill_no = _ScrapSIModel.bill_no;
                            _ScrapSIModel.bill_date = _ScrapSIModel.bill_date;
                            _ScrapSIModel.CustName = _ScrapSIModel.CustName;
                            _ScrapSIModel.Address = _ScrapSIModel.Address;
                            _ScrapSIModel.SrcDocNo = _ScrapSIModel.SrcDocNo;
                            _ScrapSIModel.SrcDocDate = _ScrapSIModel.SrcDocDate;

                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.OtherChargeDetails = ViewData["OCDetails"];
                            ViewBag.ItemTaxDetails = ViewData["TaxDetails"];
                            ViewBag.ItemTaxDetailsList = ViewData["SRSITaxDetails"];
                            ViewBag.OCTaxDetails = ViewData["OCTaxDetails"];
                            ViewBag.ItemOC_TDSDetails = ViewData["OCTDSDetails"];/*Add by Hina on 22-07-2024 for tds on 3rd party OC */
                            ViewBag.GLAccount = ViewData["VouDetail"];
                            ViewBag.ItemTDSDetails = ViewData["TDSDetails"];
                            ViewBag.CostCenterData = ViewData["CCdetail"];                          
                            ViewBag.PaymentScheduleData = ViewData["PaymentSchedule"];
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            _ScrapSIModel.BtnName = "Refresh";
                            _ScrapSIModel.Command = "Refresh";
                            _ScrapSIModel.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            //ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceDetail.cshtml", _ScrapSIModel);

                        }
                        else
                        {
                            TempData["ModelData"] = _ScrapSIModel;
                            URLModelDetails URLModel = new URLModelDetails();
                            URLModel.BtnName = "BtnSave";
                            URLModel.Command = command;
                            URLModel.TransType = "Update";
                            URLModel.Sinv_no = _ScrapSIModel.Sinv_no;
                            URLModel.Sinv_dt = _ScrapSIModel.Sinv_dt;
                            TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                            return RedirectToAction("ScrapSaleInvoiceDetail", URLModel);
                        }
                    case "Forward":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SSInvDate1 = _ScrapSIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SSInvDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string SSInvDate2 = _ScrapSIModel.Sinv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SSInvDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _ScrapSIModel.Sinv_no, DocDate = _ScrapSIModel.Sinv_dt, ListFilterData = _ScrapSIModel.ListFilterData1, WF_status = _ScrapSIModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _ScrapSIModel.Command = command;
                        ScrapSaleInvoiceApprove(_ScrapSIModel, _ScrapSIModel.PV_Narration, _ScrapSIModel.BP_Narration, _ScrapSIModel.DN_Narration);
                        URLModelDetails URLModelApprove = new URLModelDetails();
                        URLModelApprove.BtnName = _ScrapSIModel.BtnName;
                        URLModelApprove.Command = command;
                        URLModelApprove.TransType = _ScrapSIModel.TransType;
                        URLModelApprove.Sinv_no = _ScrapSIModel.Sinv_no;
                        URLModelApprove.Sinv_dt = _ScrapSIModel.Sinv_dt;
                        TempData["ModelData"] = _ScrapSIModel;
                        TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                        return RedirectToAction("ScrapSaleInvoiceDetail", URLModelApprove);

                    case "Refresh":
                        ScrapSIModel _ScrapSIModelRefresh = new ScrapSIModel();
                        _ScrapSIModelRefresh.BtnName = "Refresh";
                        _ScrapSIModelRefresh.Command = command;
                        _ScrapSIModelRefresh.TransType = "Save";
                        _ScrapSIModelRefresh.DocumentStatus = "D";
                        ViewBag.DocumentStatus = "D";
                        TempData["ModelData"] = _ScrapSIModelRefresh;
                        TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                        return RedirectToAction("ScrapSaleInvoiceDetail");

                    case "Print":
                        return GenratePdfFile(_ScrapSIModel);
                    case "BacktoList":

                        //ScrapSIListModel _SSIListModel = new ScrapSIListModel();
                       // _ScrapSIModel.WFStatus = _ScrapSIModel.WF_status1;
                        //TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                        var WF_Status = _ScrapSIModel.WF_status1;
                        if (_ScrapSIModel.ListFilterData1 == "undefined")
                        {
                            TempData["ListFilterData"] = null;
                        }
                        else
                        {
                            TempData["ListFilterData"] = _ScrapSIModel.ListFilterData1;
                        }

                        return RedirectToAction("ScrapSaleInvoice", "ScrapSaleInvoice", new { WF_Status });

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
        public ActionResult SaveScrapSaleInvoice(ScrapSIModel _ScrapSIModel)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = _ScrapSIModel.Title.Replace(" ", "");

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
                DataTable DTPaymentSchedule = new DataTable();
                DataTable DtblTdsDetail = new DataTable();
                DataTable SubitemINVQty = new DataTable();
                DataTable ItemBatchDetails = new DataTable();
                DataTable ItemSerialDetails = new DataTable();
                DataTable DtblOCTdsDetail = new DataTable();
                DataTable DtblTcsDetail = new DataTable();//for TCS Added by Suraj Maurya on 10-02-2025

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
                dtheader.Columns.Add("cust_id", typeof(string));
                dtheader.Columns.Add("bill_add_id", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));
                dtheader.Columns.Add("cust_ref_no", typeof(string));/*Add by Hina shrama on 04-03-2025 */
                dtheader.Columns.Add("cust_ref_dt", typeof(string));/*Add by Hina shrama on 05-03-2025 */
                dtheader.Columns.Add("placeofsupply", typeof(string));/*Add by Hina shrama on 10-10-2024 */
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("UserID", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("tax_amt", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("GR_Number", typeof(string));
                dtheader.Columns.Add("GR_Dt", typeof(string));
                //dtheader.Columns.Add("GR_Date", typeof(string));
                //dtheader.Columns.Add("Trans_id", typeof(string));
                dtheader.Columns.Add("Trans_Name", typeof(string));
                dtheader.Columns.Add("Veh_Number", typeof(string));
                dtheader.Columns.Add("Driver_Name", typeof(string));
                dtheader.Columns.Add("Mob_No", typeof(string));
                dtheader.Columns.Add("Tot_Tonnage", typeof(string));
                dtheader.Columns.Add("No_Of_Pkgs", typeof(string));
                dtheader.Columns.Add("Narration", typeof(string));
                dtheader.Columns.Add("tcs_amt", typeof(string));
                dtheader.Columns.Add("ship_add_id", typeof(string));
                dtheader.Columns.Add("ship_from_address", typeof(string));
                dtheader.Columns.Add("disp_Addr1", typeof(string));
                dtheader.Columns.Add("disp_Addr2", typeof(string));
                dtheader.Columns.Add("disp_country", typeof(int));
                dtheader.Columns.Add("disp_state", typeof(int));
                dtheader.Columns.Add("disp_district", typeof(int));
                dtheader.Columns.Add("disp_city", typeof(int));
                dtheader.Columns.Add("disp_pin", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                //dtrowHeader["TransType"] = Session["TransType"].ToString();
                dtrowHeader["TransType"] = _ScrapSIModel.TransType;
                dtrowHeader["MenuID"] = DocumentMenuId;
                string cancelflag = _ScrapSIModel.Cancelled.ToString();
                if (cancelflag == "False")
                {
                    dtrowHeader["Cancelled"] = "N";
                }
                else
                {
                    dtrowHeader["Cancelled"] = "Y";
                }
                string roundoffflag = _ScrapSIModel.RoundOffFlag.ToString();
                if (roundoffflag == "False")
                {
                    dtrowHeader["roundoff"] = "N";
                }
                else
                {
                    dtrowHeader["roundoff"] = "Y";
                }
                dtrowHeader["pm_flag"] = _ScrapSIModel.pmflagval;
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();

                if (_ScrapSIModel.Sinv_no == null)
                {
                    dtrowHeader["inv_no"] = null;
                }
                else
                {
                    dtrowHeader["inv_no"] = _ScrapSIModel.Sinv_no;
                }

                dtrowHeader["inv_dt"] = _ScrapSIModel.Sinv_dt;
                dtrowHeader["cust_id"] = _ScrapSIModel.CustID;
                dtrowHeader["bill_add_id"] = _ScrapSIModel.bill_add_id == null ? "0" : _ScrapSIModel.bill_add_id;
                dtrowHeader["remarks"] = _ScrapSIModel.Remarks; 
                dtrowHeader["cust_ref_no"] = _ScrapSIModel.CustRefNo;/*Add by Hina shrama on 04-03-2025 */
                dtrowHeader["cust_ref_dt"] = _ScrapSIModel.CustRefDt;/*Add by Hina shrama on 05-03-2025 */
                dtrowHeader["placeofsupply"] = _ScrapSIModel.PlaceOfSupply;/*Add by Hina shrama on 10-10-2024 */
                dtrowHeader["inv_status"] = IsNull(_ScrapSIModel.AppStatus, "D");
                dtrowHeader["UserID"] = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["gr_val"] = _ScrapSIModel.GrossValue;
                dtrowHeader["tax_amt"] = _ScrapSIModel.TaxAmount;
                dtrowHeader["oc_amt"] = _ScrapSIModel.OtherCharges;
                dtrowHeader["net_val_bs"] = _ScrapSIModel.NetAmountInBase;
                dtrowHeader["gr_val"] = _ScrapSIModel.GrossValue;
                dtrowHeader["tax_amt"] = _ScrapSIModel.TaxAmount;
                dtrowHeader["oc_amt"] = _ScrapSIModel.OtherCharges;
                dtrowHeader["net_val_bs"] = _ScrapSIModel.NetAmountInBase;
                dtrowHeader["GR_Number"] = _ScrapSIModel.GR_No;
                dtrowHeader["GR_Dt"] = _ScrapSIModel.HdnGRDate;
                //dtrowHeader["GR_Dt"] = _ScrapSIModel.GR_Dt;
                //dtrowHeader["GR_Date"] = _ScrapSIModel.HdnGRDate;
                dtrowHeader["Trans_Name"] = _ScrapSIModel.Transpt_NameID;
                //dtrowHeader["Trans_Name"] = _ScrapSIModel.Transpt_Name;
                dtrowHeader["Veh_Number"] = _ScrapSIModel.Veh_Number;
                dtrowHeader["Driver_Name"] = _ScrapSIModel.Driver_Name;
                dtrowHeader["Mob_No"] = _ScrapSIModel.Mob_No;
                dtrowHeader["Tot_Tonnage"] = _ScrapSIModel.Tot_Tonnage;
                dtrowHeader["No_Of_Pkgs"] = _ScrapSIModel.No_Of_Packages;
                dtrowHeader["Narration"] = _ScrapSIModel.Narration;
                dtrowHeader["tcs_amt"] = _ScrapSIModel.tcs_amt;
                dtrowHeader["ship_add_id"] = _ScrapSIModel.Ship_Add_Id;
                dtrowHeader["ship_from_address"] = _ScrapSIModel.ShipFromAddress;
                if ( cancelflag == "False")
                {
                    dtrowHeader["disp_Addr1"] = _ScrapSIModel.Address1;
                    dtrowHeader["disp_Addr2"] = _ScrapSIModel.Address2;
                    dtrowHeader["disp_country"] = Convert.ToInt32(_ScrapSIModel.Country);
                    dtrowHeader["disp_state"] = Convert.ToInt32(_ScrapSIModel.State);
                    dtrowHeader["disp_district"] = Convert.ToInt32(_ScrapSIModel.District);
                    dtrowHeader["disp_city"] = Convert.ToInt32(_ScrapSIModel.City);
                    dtrowHeader["disp_pin"] = _ScrapSIModel.Pin;
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
                dtrowHeader["cancel_remarks"] = _ScrapSIModel.CancelledRemarks;
                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;

                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(string));
                dtItem.Columns.Add("HsnNo", typeof(string));
                dtItem.Columns.Add("inv_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));/*Add TaxExempted,ManualGST by Hina sharma on 13-01-2025*/
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("wh_id", typeof(string));
                dtItem.Columns.Add("avl_qty", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));
                dtItem.Columns.Add("it_remarks", typeof(string));/*add by Hina sharma on 04-03-2025*/
                dtItem.Columns.Add("DiscInPer", typeof(string));/*add by Hina sharma on 04-03-2025*/
                dtItem.Columns.Add("DiscVal", typeof(string));/*add by Hina sharma on 04-03-2025*/
                dtItem.Columns.Add("sr_no", typeof(int));
                dtItem.Columns.Add("FOC", typeof(string));
                dtItem.Columns.Add("PackSize", typeof(string));

                JArray jObject = JArray.Parse(_ScrapSIModel.Itemdetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                    //dtrowLines["HsnNo"] = Convert.ToInt32(jObject[i]["Hsn_no"].ToString());
                    dtrowLines["HsnNo"] = (jObject[i]["Hsn_no"].ToString());
                    dtrowLines["inv_qty"] = jObject[i]["inv_qty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                    dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();/*Add TaxExempted,ManualGST by Hina sharma on 13-01-2025*/
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtrowLines["item_tax_amt"] = jObject[i]["item_tax_amt"].ToString();
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                    dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                    dtrowLines["wh_id"] = jObject[i]["wh_id"].ToString();
                    dtrowLines["avl_qty"] = jObject[i]["avl_qty"].ToString();
                    dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();
                    dtrowLines["it_remarks"] = jObject[i]["itemRemarks"].ToString();/*add by Hina sharma on 04-03-2025*/
                    dtrowLines["DiscInPer"] = jObject[i]["DiscInPer"].ToString();/*add by Hina sharma on 04-03-2025*/
                    dtrowLines["DiscVal"] = jObject[i]["DiscVal"].ToString();/*add by Hina sharma on 04-03-2025*/
                    dtrowLines["sr_no"] = jObject[i]["sr_no"].ToString();
                    dtrowLines["FOC"] = jObject[i]["FOC"].ToString();
                    dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();
                    dtItem.Rows.Add(dtrowLines); 
                }
                DtblItemDetail = dtItem;
                ViewData["ItemDetails"] = dtitemdetail(jObject);

                DtblTaxDetail = ToDtblTaxDetail(_ScrapSIModel.ItemTaxdetails);
                ViewData["TaxDetails"] = ViewData["SRSITaxDetails"];
                DtblOCTaxDetail = ToDtblTaxDetail(_ScrapSIModel.OC_TaxDetail);
                ViewData["OCTaxDetails"] = ViewData["SRSITaxDetails"];
                //DtblTdsDetail = ToDtblTdsDetail(_ScrapSIModel.tds_details);

                DataTable OC_detail = new DataTable();
                OC_detail.Columns.Add("oc_id", typeof(string));
                OC_detail.Columns.Add("oc_val", typeof(string));
                OC_detail.Columns.Add("tax_amt", typeof(string));
                OC_detail.Columns.Add("total_amt", typeof(string));
                /*Code Added by Hina on 22-07-2024 to chnge for 3rd party OC and tds on 3rd party OC*/
                OC_detail.Columns.Add("curr_id", typeof(string));
                OC_detail.Columns.Add("conv_rate", typeof(string));
                OC_detail.Columns.Add("supp_id", typeof(string));
                OC_detail.Columns.Add("supp_type", typeof(string));
                OC_detail.Columns.Add("bill_no", typeof(string));
                OC_detail.Columns.Add("bill_date", typeof(string));
                OC_detail.Columns.Add("tds_amt", typeof(string));
                OC_detail.Columns.Add("roundoff", typeof(string));//Added by Suraj Maurya on 11-12-2024
                OC_detail.Columns.Add("pm_flag", typeof(string));//Added by Suraj Maurya on 11-12-2024
                if (_ScrapSIModel.ItemOCdetails != null)
                {
                    JArray jObjectOC = JArray.Parse(_ScrapSIModel.ItemOCdetails);
                    for (int i = 0; i < jObjectOC.Count; i++)
                    {
                        DataRow dtrowOCDetailsLines = OC_detail.NewRow();
                        dtrowOCDetailsLines["oc_id"] = jObjectOC[i]["oc_id"].ToString();
                        dtrowOCDetailsLines["oc_val"] = jObjectOC[i]["oc_val"].ToString();
                        dtrowOCDetailsLines["tax_amt"] = jObjectOC[i]["tax_amt"].ToString();
                        dtrowOCDetailsLines["total_amt"] = jObjectOC[i]["total_amt"].ToString();
                        /*Code Added by Hina on 22-07-2024 to chnge for 3rd party OC and tds on 3rd party OC*/
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
                CC_Details.Columns.Add("vou_sr_no", typeof(string));/*Add by Hina on 22-07-2024 to add for modify in GL 3rd party OC */
                CC_Details.Columns.Add("gl_sr_no", typeof(string));
                CC_Details.Columns.Add("acc_id", typeof(string));
                CC_Details.Columns.Add("cc_id", typeof(int));
                CC_Details.Columns.Add("cc_val_id", typeof(int));
                CC_Details.Columns.Add("cc_amt", typeof(string));

                JArray JAObj = JArray.Parse(_ScrapSIModel.CC_DetailList);
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


                /**----------------Cost Center Section--------------------*/
                DataTable PaymentSchedule = new DataTable();

             
                PaymentSchedule.Columns.Add("sr_no", typeof(int));
                PaymentSchedule.Columns.Add("paym_mst", typeof(string));
                PaymentSchedule.Columns.Add("precentage", typeof(double));
                PaymentSchedule.Columns.Add("amt", typeof(double));

                if(_ScrapSIModel.HdnPaymentSchedule != null)
                {
                    JArray JPS = JArray.Parse(_ScrapSIModel.HdnPaymentSchedule);

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
                var _ScrapSIModelattch = TempData["ModelDataattch"] as ScrapSIModelattch;
                TempData["ModelDataattch"] = null;
                if (_ScrapSIModel.attatchmentdetail != null)
                {
                    if (_ScrapSIModelattch != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_ScrapSIModelattch.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _ScrapSIModelattch.AttachMentDetailItmStp as DataTable;
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
                        if (_ScrapSIModel.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _ScrapSIModel.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(_ScrapSIModel.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_ScrapSIModel.Sinv_no))
                            {
                                dtrowAttachment1["id"] = _ScrapSIModel.Sinv_no;
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
                    if (_ScrapSIModel.TransType == "Update")
                    {
                        string AttachmentFilePath = Server.MapPath("~/Attachment/" + PageName + "/");
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_ScrapSIModel.Sinv_no))
                            {
                                ItmCode = _ScrapSIModel.Sinv_no;
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
                                    if (drImgPath == fielpath)
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
                /*Code commented and Added by Hina on 22-07-2024 to chnge for 3rd party OC and tds on 3rd party OC*/
                //vou_Details.Columns.Add("comp_id", typeof(string));
                //vou_Details.Columns.Add("id", typeof(string));
                //vou_Details.Columns.Add("type", typeof(string));
                //vou_Details.Columns.Add("doctype", typeof(string));
                //vou_Details.Columns.Add("Value", typeof(string));
                //vou_Details.Columns.Add("DrAmt", typeof(string));
                //vou_Details.Columns.Add("CrAmt", typeof(string));
                //vou_Details.Columns.Add("TransType", typeof(string));
                //vou_Details.Columns.Add("Gltype", typeof(string));
                //vou_Details.Columns.Add("gl_sr_no", typeof(string));

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
                if (_ScrapSIModel.vouDetail != null)
                {

                    JArray jObjectVOU = JArray.Parse(_ScrapSIModel.vouDetail);
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

                /*----------For TDS on Third party Other charge -----------------------------*/
                DtblOCTdsDetail = ToDtblOCTdsDetail(_ScrapSIModel.oc_tds_details);
                DtblTcsDetail = ToDtblTcsDetail(_ScrapSIModel.tcs_details);

                DataTable subitmInv_qty = new DataTable();

                subitmInv_qty.Columns.Add("item_id", typeof(string));
                subitmInv_qty.Columns.Add("sub_item_id", typeof(string));
                subitmInv_qty.Columns.Add("avl_qty", typeof(string));
                subitmInv_qty.Columns.Add("inv_qty", typeof(string));
                JArray JAObj1 = JArray.Parse(_ScrapSIModel.SubItemDetailsDt);
                for (int i = 0; i < JAObj1.Count; i++)
                {
                    DataRow dtrowLines1 = subitmInv_qty.NewRow();

                    dtrowLines1["item_id"] = JAObj1[i]["item_id"].ToString();
                    dtrowLines1["sub_item_id"] = JAObj1[i]["sub_item_id"].ToString();
                    dtrowLines1["avl_qty"] = JAObj1[i]["avl_stk"].ToString();
                    dtrowLines1["inv_qty"] = JAObj1[i]["qty"].ToString();


                    subitmInv_qty.Rows.Add(dtrowLines1);
                }
                SubitemINVQty = subitmInv_qty;


                DataTable Batch_detail = new DataTable();
                Batch_detail.Columns.Add("comp_id", typeof(int));
                Batch_detail.Columns.Add("br_id", typeof(int));
                Batch_detail.Columns.Add("item_id", typeof(string));
                Batch_detail.Columns.Add("uom_id", typeof(string));
                Batch_detail.Columns.Add("batch_no", typeof(string));
                Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
                Batch_detail.Columns.Add("expiry_date", typeof(DateTime));
                Batch_detail.Columns.Add("inv_qty", typeof(string));
                Batch_detail.Columns.Add("lot_no", typeof(string));
                Batch_detail.Columns.Add("mfg_name", typeof(string));
                Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                Batch_detail.Columns.Add("mfg_date", typeof(string));
                if (_ScrapSIModel.ItemBatchWiseDetail != null)
                {
                    JArray jObjectBatch = JArray.Parse(_ScrapSIModel.ItemBatchWiseDetail);
                    for (int i = 0; i < jObjectBatch.Count; i++)
                    {
                        DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                        dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                        dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();                                          
                        dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                        dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();
                        dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                        dtrowBatchDetailsLines["avl_batch_qty"] = jObjectBatch[i]["avl_batch_qty"].ToString();
                        if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null || jObjectBatch[i]["ExpiryDate"].ToString() == "undefined")
                        {
                            dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                        }
                        else
                        {
                            dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                        }
                        dtrowBatchDetailsLines["inv_qty"] = jObjectBatch[i]["inv_qty"].ToString();
                        dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                        dtrowBatchDetailsLines["mfg_name"] = IsBlank(jObjectBatch[i]["mfg_name"].ToString(),null);
                        dtrowBatchDetailsLines["mfg_mrp"] = IsBlank(jObjectBatch[i]["mfg_mrp"].ToString(),null);
                        dtrowBatchDetailsLines["mfg_date"] = IsBlank(jObjectBatch[i]["mfg_date"].ToString(),null);
                        Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                    }
                  //  ViewData["BatchDetails"] = dtBatchDetails(jObjectBatch, _ScrapSIModel);
                }
                ItemBatchDetails = Batch_detail;

                DataTable Serial_detail = new DataTable();
                Serial_detail.Columns.Add("item_id", typeof(string));
                Serial_detail.Columns.Add("uom_id", typeof(int));
                Serial_detail.Columns.Add("serial_no", typeof(string));
                Serial_detail.Columns.Add("serial_qty", typeof(string));
                Serial_detail.Columns.Add("lot_no", typeof(string));
                Serial_detail.Columns.Add("inv_qty", typeof(string));
                Serial_detail.Columns.Add("mfg_name", typeof(string));
                Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                Serial_detail.Columns.Add("mfg_date", typeof(string));

                if (_ScrapSIModel.ItemSerialWiseDetail != null)
                {
                    JArray jObjectSerial = JArray.Parse(_ScrapSIModel.ItemSerialWiseDetail);
                    for (int i = 0; i < jObjectSerial.Count; i++)
                    {
                        DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                        dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                        dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                        dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                        dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                        dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                        dtrowSerialDetailsLines["inv_qty"] = jObjectSerial[i]["invqty"].ToString();
                        dtrowSerialDetailsLines["mfg_name"] = IsBlank(jObjectSerial[i]["mfg_name"].ToString(),null);
                        dtrowSerialDetailsLines["mfg_mrp"] = IsBlank(jObjectSerial[i]["mfg_mrp"].ToString(),null);
                        dtrowSerialDetailsLines["mfg_date"] = IsBlank(jObjectSerial[i]["mfg_date"].ToString(),null);
                        Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                    }
                }
                ItemSerialDetails = Serial_detail;


                string Narr = "";
                string CN_Narr = "";
                if (_ScrapSIModel.Cancelled!=false)
                {
                    Narr = _ScrapSIModel.PV_Narration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                    CN_Narr = _ScrapSIModel.CN_Narration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }
                var nontax = "N";
                string nontaxable = _ScrapSIModel.nontaxable.ToString();
                if (nontaxable == "False")
                {
                    nontax = "N";
                }
                else
                {
                    nontax = "Y";
                }
                SaveMessage = _ScrapSI_ISERVICE.InsertscrpSSI_Details(DtblHDetail, DtblItemDetail, DtblTaxDetail
                    , DtblOCTaxDetail, DtblIOCDetail, DtblAttchDetail, DtblVouDetail, CRCostCenterDetails, SubitemINVQty
                    , ItemBatchDetails, DtblOCTdsDetail, Narr,CN_Narr, _ScrapSIModel.slprsn_id, DtblTcsDetail, ItemSerialDetails, _ScrapSIModel.ddlPayment_term,
                    _ScrapSIModel.ddlDelivery_term, _ScrapSIModel.Declaration_1, _ScrapSIModel.Declaration_2,
                    _ScrapSIModel.Invoice_Heading, nontax, _ScrapSIModel.ShipTo, DTPaymentSchedule, _ScrapSIModel.Corporate_Address, _ScrapSIModel.pvt_mark);
                if (SaveMessage == "DocModify")
                {
                    _ScrapSIModel.Message = "DocModify";
                    _ScrapSIModel.BtnName = "Refresh";
                    _ScrapSIModel.Command = "Refresh";
                    _ScrapSIModel.DocumentMenuId = DocumentMenuId;
                    TempData["ModelData"] = _ScrapSIModel;
                    return RedirectToAction("ScrapSaleInvoiceDetail");
                }
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
                        _ScrapSIModel.Message = Message;
                        return RedirectToAction("ScrapSaleInvoiceDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_ScrapSIModelattch != null)
                        {
                            if (_ScrapSIModelattch.Guid != null)
                            {
                                Guid = _ScrapSIModelattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, _ScrapSIModel.TransType, DtblAttchDetail);
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
                        try
                        {
                            ViewBag.PrintFormat = PrintFormatDataTable(_ScrapSIModel,null);
                            //string fileName = "DSI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "TaxInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(Inv_no, Inv_DATE, fileName, null, _ScrapSIModel.GstApplicable, "105103148","C");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, "105103148", Inv_no, "Cancel", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _ScrapSIModel.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        

                        _ScrapSIModel.Message = _ScrapSIModel.Message == "ErrorInMail"? "Cancelled_ErrorInMail" : "Cancelled";
                        _ScrapSIModel.Command = "Update";
                        _ScrapSIModel.Sinv_no = Inv_no;
                        _ScrapSIModel.Sinv_dt = Inv_DATE;
                        _ScrapSIModel.TransType = "Update";
                        _ScrapSIModel.AppStatus = "D";
                        _ScrapSIModel.BtnName = "Refresh";
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
                        if (Message == "Update" || Message == "Save" || Message == "UpdateTransPort")
                        {
                            _ScrapSIModel.Message = "Save";
                            _ScrapSIModel.Command = "Update";
                            _ScrapSIModel.Sinv_no = Inv_no;
                            _ScrapSIModel.Sinv_dt = Inv_DATE;
                            _ScrapSIModel.TransType = "Update";
                            _ScrapSIModel.AppStatus = "D";
                            _ScrapSIModel.BtnName = "BtnSave";
                            _ScrapSIModel.AttachMentDetailItmStp = null;
                            _ScrapSIModel.Guid = null;
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
                    return RedirectToAction("ScrapSaleInvoiceDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ScrapSIModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ScrapSIModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ScrapSIModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
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
                DataSet GlDt = _ScrapSI_ISERVICE.GetAllGLDetails(DtblGLDetail);
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
        private DataTable ToDtblTaxDetail(string TaxDetails)
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
                        Tax_detail.Rows.Add(dtrowTaxDetailsLines);
                    }
                    ViewData["SRSITaxDetails"] = dtTaxdetail(jObjectTax);

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
        private DataTable ToDtblOCTdsDetail(string octdsDetails)/*Add by Hina on 22-07-2024 for tds on 3rd party OC */
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

        //private DataTable ToDtblTdsDetail(string tdsDetails)
        //{
        //    try
        //    {
        //        DataTable DtblItemtdsDetail = new DataTable();
        //        DataTable tds_detail = new DataTable();
        //        tds_detail.Columns.Add("tds_id", typeof(string));
        //        tds_detail.Columns.Add("tds_rate", typeof(string));
        //        tds_detail.Columns.Add("tds_val", typeof(string));
        //        tds_detail.Columns.Add("tds_level", typeof(string));
        //        tds_detail.Columns.Add("tds_apply_on", typeof(string));


        //        if (tdsDetails != null)
        //        {
        //            JArray jObjecttds = JArray.Parse(tdsDetails);
        //            for (int i = 0; i < jObjecttds.Count; i++)
        //            {
        //                DataRow dtrowtdsDetailsLines = tds_detail.NewRow();
        //                dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
        //                string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
        //                tds_rate = tds_rate.Replace("%", "");
        //                dtrowtdsDetailsLines["tds_rate"] = tds_rate;
        //                dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
        //                dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
        //                dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();

        //                tds_detail.Rows.Add(dtrowtdsDetailsLines);
        //            }
        //            ViewData["TDSDetails"] = dtTDSdetail(jObjecttds);
        //        }
        //        DtblItemtdsDetail = tds_detail;
        //        return DtblItemtdsDetail;
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


            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("inv_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_gr_val", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));/*Add TaxExempted,ManualGST by Hina sharma on 02-01-2025*/
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val_bs", typeof(string));
            dtItem.Columns.Add("hsn_no", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));/*add by Hina sharma on 04-03-2025*/
            dtItem.Columns.Add("DiscInPer", typeof(string));/*add by Hina sharma on 04-03-2025*/
            dtItem.Columns.Add("DiscVal", typeof(string));/*add by Hina sharma on 04-03-2025*/
            dtItem.Columns.Add("sr_no", typeof(int));/*add by Hina sharma on 04-03-2025*/
            dtItem.Columns.Add("FOC", typeof(string));/*add by Hina sharma on 04-03-2025*/
            dtItem.Columns.Add("PackSize", typeof(string));/*add by Hina sharma on 04-03-2025*/



            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();

                dtrowLines["inv_qty"] = jObject[i]["inv_qty"].ToString();
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();/*Add TaxExempted,ManualGST by Hina sharma on 02-01-2025*/
                dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
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
                dtrowLines["it_remarks"] = jObject[i]["itemRemarks"].ToString();/*add by Hina sharma on 04-03-2025*/
                dtrowLines["DiscInPer"] = jObject[i]["DiscInPer"].ToString();/*add by Hina sharma on 04-03-2025*/
                dtrowLines["DiscVal"] = jObject[i]["DiscVal"].ToString();/*add by Hina sharma on 04-03-2025*/
                dtrowLines["sr_no"] = jObject[i]["sr_no"].ToString();
                dtrowLines["FOC"] = jObject[i]["FOC"].ToString();
                dtrowLines["PackSize"] = jObject[i]["PackSize"].ToString();/*add by Hina sharma on 04-03-2025*/

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

        //public DataTable dtTDSdetail(JArray jObjecttds)
        //{
        //    DataTable DtblItemtdsDetail = new DataTable();
        //    DataTable tds_detail = new DataTable();
        //    tds_detail.Columns.Add("tds_id", typeof(string));
        //    tds_detail.Columns.Add("tds_rate", typeof(string));
        //    tds_detail.Columns.Add("tds_val", typeof(string));
        //    tds_detail.Columns.Add("tds_level", typeof(string));
        //    tds_detail.Columns.Add("tds_apply_on", typeof(string));
        //    tds_detail.Columns.Add("tds_name", typeof(string));
        //    tds_detail.Columns.Add("tds_apply_Name", typeof(string));

        //    for (int i = 0; i < jObjecttds.Count; i++)
        //    {
        //        DataRow dtrowtdsDetailsLines = tds_detail.NewRow();
        //        dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["Tds_id"].ToString();
        //        string tds_rate = jObjecttds[i]["Tds_rate"].ToString();
        //        tds_rate = tds_rate.Replace("%", "");
        //        dtrowtdsDetailsLines["tds_rate"] = tds_rate;
        //        dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["Tds_level"].ToString();
        //        dtrowtdsDetailsLines["tds_val"] = jObjecttds[i]["Tds_val"].ToString();
        //        dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["Tds_apply_on"].ToString();
        //        dtrowtdsDetailsLines["tds_name"] = jObjecttds[i]["Tds_name"].ToString();
        //        dtrowtdsDetailsLines["tds_apply_Name"] = jObjecttds[i]["Tds_applyOnName"].ToString();
        //        tds_detail.Rows.Add(dtrowtdsDetailsLines);
        //    }

        //    return tds_detail;
        //}
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
            CC_Details.Columns.Add("vou_sr_no", typeof(string));/*add by Hina on 22-07-2024 for 3rd party OC*/
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
        public DataTable dtVoudetail(JArray jObjectVOU)
        {
            DataTable vou_Details = new DataTable();/*Modify by Hina on 22-07-2024 for 3rd party OC and TDS on 3rd party OC*/
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

        public DataTable dtOCTDSdetail(JArray jObjecttdsoc)/*Add by Hina on 22-07-2024 for tds on 3rd party OC */
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
        private ActionResult ScrapPIDelete(ScrapSIModel _SSIModel, string command, string title)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();

                string BPNo = _SSIModel.Sinv_no;
                string BankPayNumber = BPNo.Replace("/", "");

                string Message = _ScrapSI_ISERVICE.ScrapSIDelete(_SSIModel, CompID, br_id, DocumentMenuId);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(BankPayNumber))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + br_id, PageName, BankPayNumber, Server);
                }
                /*---------Attachments Section End----------------*/

                _SSIModel.Message = "Deleted";
                _SSIModel.Command = "Refresh";
                _SSIModel.Sinv_no = null;
                _SSIModel.Sinv_dt = null;
                _SSIModel.TransType = "Refresh";
                _SSIModel.AppStatus = "DL";
                _SSIModel.BtnName = "BtnDelete";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["Inv_No"] = null;
                //Session["Inv_Date"] = null;
                //_SSIModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("ScrapSaleInvoiceDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ScrapSaleInvoiceApprove(ScrapSIModel _SSIModel, string PV_VoucherNarr, string BP_VoucherNarr, string DN_VoucherNarr)
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
                var DocNo = _SSIModel.Sinv_no;
                var DocDate = _SSIModel.Sinv_dt;
                var A_Status = _SSIModel.A_Status;
                var A_Level = _SSIModel.A_Level;
                var A_Remarks = _SSIModel.A_Remarks;
                var DN_Nurr_Tcs = _SSIModel.DN_Narration_Tcs;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string SaleVouMsg = _SSIModel.SaleVouMsg;

                string Message = _ScrapSI_ISERVICE.ApproveSSI(DocNo, DocDate, MenuDocId, BranchID, Comp_ID, UserID, mac_id
                    , A_Status, A_Level, A_Remarks, SaleVouMsg,  PV_VoucherNarr,  BP_VoucherNarr,  DN_VoucherNarr, DN_Nurr_Tcs);
                try
                {
                    ViewBag.PrintFormat = PrintFormatDataTable(_SSIModel,null);
                   // string fileName = "DSI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "TaxInvoice_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(DocNo, DocDate, fileName, null, _SSIModel.GstApplicable, MenuDocId,"AP");
                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, MenuDocId, DocNo, "AP", UserID, "", filePath);
                }
                catch(Exception exMail)
                {
                    _SSIModel.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                

                string[] FDetail = Message.Split(',');
                string INV_NO = string.Empty;
                string INV_DT = string.Empty;
                string ApMessage = string.Empty;
                if (FDetail[0].ToString()== "StockNotAvail")
                {
                    INV_NO = FDetail[1].ToString();
                    INV_DT = FDetail[2].ToString();
                    _SSIModel.StockItemWiseMessage = string.Join(",", FDetail.Skip(3));
                    _SSIModel.Message = "stockNotAvailable";
                    _SSIModel.Command = "Update";
                   
                    _SSIModel.BtnName = "BtnToDetailPage";
                }
                else
                {
                     INV_NO = FDetail[0].ToString();
                     INV_DT = FDetail[7].ToString();
                     ApMessage = FDetail[6].ToString().Trim();
                }
                if (ApMessage == "A")
                {
                    //Session["Message"] = "Approved";
                    _SSIModel.Message = _SSIModel.Message == "ErrorInMail"? "Approved_ErrorInMail" : "Approved";
                    _SSIModel.BtnName = "BtnEdit";
                    _SSIModel.Command = "Approve";
                }
                _SSIModel.TransType = "Update";              
                _SSIModel.Sinv_no = INV_NO;
                _SSIModel.Sinv_dt = INV_DT;
                _SSIModel.AppStatus = "D";
               

                // Session["TransType"] = "Update";
                // Session["Command"] = "Approve";
                // Session["Inv_No"] = INV_NO;
                //Session["Inv_Date"] = INV_DT;
                // Session["AppStatus"] = 'D';
                // Session["BtnName"] = "BtnEdit";
                //TempData["ListFilterData"] = FilterData;             
                return RedirectToAction("ScrapSaleInvoiceDetail", _SSIModel);
                //DataRows = Json(GrnDetail);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                // return Json("ErrorPage");
            }
            //return DataRows;
        }
        private DataTable PrintFormatDataTable(ScrapSIModel _Model,string PrintFormat)
        {
            DataTable dt = new DataTable();
            //var commonCont = new CommonController(_Common_IServices);
            
            dt.Columns.Add("PrintFormat", typeof(string));
            dt.Columns.Add("ShowProdDesc", typeof(string));
            dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
            dt.Columns.Add("ShowProdTechDesc", typeof(string));
            dt.Columns.Add("ShowSubItem", typeof(string));
            dt.Columns.Add("CustAliasName", typeof(string));
            dt.Columns.Add("ShowTotalQty", typeof(string));
            dt.Columns.Add("ShowWithoutSybbol", typeof(string));
            dt.Columns.Add("showDeclare1", typeof(string));
            dt.Columns.Add("showDeclare2", typeof(string));
            dt.Columns.Add("showInvHeading", typeof(string));
            dt.Columns.Add("NumberOfCopy", typeof(string));
            dt.Columns.Add("PrintShipFromAddress", typeof(string));
            dt.Columns.Add("PrintCorpAddr", typeof(string));
            dt.Columns.Add("PrintRemarks", typeof(string));
            if (_Model != null)
            {
                DataRow dtr = dt.NewRow();
                dtr["PrintFormat"] = _Model.PrintFormat;
                dtr["ShowProdDesc"] = _Model.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = _Model.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = _Model.ShowProdTechDesc;
                dtr["ShowSubItem"] = _Model.ShowSubItem;
                dtr["CustAliasName"] = _Model.CustomerAliasName;
                dtr["ShowTotalQty"] = _Model.ShowTotalQty;
                dtr["ShowWithoutSybbol"] = _Model.ShowWithoutSybbol;
                dtr["showDeclare1"] = _Model.showDeclare1;
                dtr["showDeclare2"] = _Model.showDeclare2;
                dtr["showInvHeading"] = _Model.showInvHeading;
                dtr["NumberOfCopy"] = _Model.NumberofCopy;
                dtr["PrintShipFromAddress"] = _Model.PrintShipFromAddress;
                dtr["PrintCorpAddr"] = _Model.PrintCorpAddr;
                dtr["PrintRemarks"] = _Model.PrintRemarks;
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
                    dtr["ShowTotalQty"] = jObject[0]["ShowTotalQty"];
                    dtr["ShowWithoutSybbol"] = jObject[0]["ShowWithoutSybbol"];
                    dtr["showDeclare1"] = jObject[0]["showDeclare1"];
                    dtr["showDeclare2"] = jObject[0]["showDeclare2"];
                    dtr["showInvHeading"] = jObject[0]["showInvHeading"];
                    dtr["NumberOfCopy"] = jObject[0]["NumberOfCopy"];
                    dtr["PrintShipFromAddress"] = jObject[0]["PrintShipFromAddress"];
                    dtr["PrintCorpAddr"] = jObject[0]["PrintCorpAddr"];
                    dtr["PrintRemarks"] = jObject[0]["PrintRemarks"];
                    dt.Rows.Add(dtr);
                }
            }
            //Commented by Suraj on 27-05-2025 
            //Cmn_PrintOptions cmn_PrintOptions = new Cmn_PrintOptions//Added by Suraj on 08-10-2024
            //{
            //    PrintFormat = _Model.PrintFormat,
            //    ShowProdDesc = _Model.ShowProdDesc,
            //    ShowCustSpecProdDesc = _Model.ShowCustSpecProdDesc,
            //    ShowProdTechDesc = _Model.ShowProdTechDesc,
            //    ShowSubItem = _Model.ShowSubItem,
            //    CustomerAliasName = _Model.CustomerAliasName,
            //    ShowTotalQty = _Model.ShowTotalQty,
            //};
            //dt = commonCont.PrintOptionsDt(cmn_PrintOptions);
            return dt;
        }
        public string SavePdfDocToSendOnEmailAlert(string poNo, string poDate, string fileName, string PrintFormat,string GstApplicable,string docid, string docstatus)
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
                        //dt = commonCont.PrintOptionsDt(PrintFormat); //Added by Suraj on 08-10-2024
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
                        if (GstApplicable == "Y")
                        {
                            var data = GetPdfDataOfGstInv(dt, "105103148", poNo, poDate, 1);
                            return commonCont.SaveAlertDocument(data, fileName);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(mailattch))
                    {
                        if (mailattch.Trim() == "Yes")
                        {
                            var data = GetPdfData("105103148", poNo, poDate, 1);
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

        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1, string PV_VoucherNarr, string BP_VoucherNarr, string DN_VoucherNarr)
        {
            //JArray jObjectBatch = JArray.Parse(list);
            ScrapSIModel _SSIModel = new ScrapSIModel();
            URLModelDetails URLModel = new URLModelDetails();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _SSIModel.Sinv_no = jObjectBatch[i]["DocNo"].ToString();
                    _SSIModel.Sinv_dt = jObjectBatch[i]["DocDate"].ToString();
                    _SSIModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _SSIModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _SSIModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                    _SSIModel.SaleVouMsg = jObjectBatch[i]["SaleVouMsg"].ToString();
                    _SSIModel.DN_Narration_Tcs = jObjectBatch[i]["DN_Nurr_tcs"].ToString();
                    _SSIModel.GstApplicable = jObjectBatch[i]["GstApplicable"].ToString();/* Added by Suraj Maurya on 26-05-2025 */
                }
            }
            if (_SSIModel.A_Status != "Approve" || _SSIModel.A_Status == "" || _SSIModel.A_Status == null)
            {
                _SSIModel.A_Status = "Approve";
            }
            _SSIModel.ListFilterData1 = ListFilterData1;
            
            ScrapSaleInvoiceApprove(_SSIModel, PV_VoucherNarr, BP_VoucherNarr, DN_VoucherNarr);/*Add by Hina on 22-07-2024 to add for third party OC, tds on third party OC*/
            TempData["ModelData"] = _SSIModel;
            TempData["WF_status"] = WF_status1;
            URLModel.Sinv_no = _SSIModel.Sinv_no;
            URLModel.Sinv_dt = _SSIModel.Sinv_dt;
            URLModel.TransType = _SSIModel.TransType;
            URLModel.BtnName = _SSIModel.BtnName;
            URLModel.Command = _SSIModel.Command;
            return RedirectToAction("ScrapSaleInvoiceDetail", URLModel);
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
        //    return RedirectToAction("ScrapSaleInvoiceDetail");
        //}

        private List<ScrapSaleInvoiceList> GetScrapSIList(ScrapSIListModel _SSIListModel)
        {
            _SSIList = new List<ScrapSaleInvoiceList>();
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
                if (_SSIListModel.WF_status != null)
                {
                    wfStatus = _SSIListModel.WF_status;
                
                }
                else
                {
                    wfStatus = null;
                }

                DataSet DSet = _ScrapSI_ISERVICE.GetSSIList(CompID, BrchID, UserID, _SSIListModel.CustID, _SSIListModel.SSI_FromDate, _SSIListModel.SSI_ToDate, _SSIListModel.Status, _SSIListModel.wfdocid, wfStatus, _SSIListModel.SQ_SalePerson);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ScrapSaleInvoiceList _ScrapPIList = new ScrapSaleInvoiceList();
                        _ScrapPIList.InvNo = dr["inv_no"].ToString();
                        _ScrapPIList.InvDate = dr["inv_dt"].ToString();
                        _ScrapPIList.SalesPerson = dr["SalesPerson"].ToString();
                        _ScrapPIList.InvDt = dr["InvDt"].ToString();
                        _ScrapPIList.InvValue = dr["net_val_bs"].ToString();
                        _ScrapPIList.CustName = dr["cust_name"].ToString();
                        _ScrapPIList.InvStauts = dr["InvStauts"].ToString();
                        _ScrapPIList.CreateDate = dr["CreateDate"].ToString();
                        _ScrapPIList.ApproveDate = dr["ApproveDate"].ToString();
                        _ScrapPIList.ModifyDate = dr["ModifyDate"].ToString();
                        _ScrapPIList.create_by = dr["create_by"].ToString();
                        _ScrapPIList.app_by = dr["app_by"].ToString();
                        _ScrapPIList.mod_by = dr["mod_by"].ToString();
                        _ScrapPIList.cust_ref_no = dr["cust_ref_no"].ToString();
                        _SSIList.Add(_ScrapPIList);
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
            return _SSIList;
        }

        public ActionResult SearchScrapSaleInvoiceList(string CustId, string Fromdate, string Todate, string Status,string sales_person)
        {
            ScrapSIListModel _SSIListModel = new ScrapSIListModel();
            try
            {
                
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
                _SSIList = new List<ScrapSaleInvoiceList>();
                DataSet DSet = _ScrapSI_ISERVICE.GetSSIList(CompID, BrchID, UserID, CustId, Fromdate, Todate, Status, DocumentMenuId, _SSIListModel.WF_status, sales_person);
                //Session["SSISearch"] = "SPI_Search";
                _SSIListModel.SSISearch = "SSI_Search";

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        ScrapSaleInvoiceList _ScrapPIList = new ScrapSaleInvoiceList();
                        _ScrapPIList.InvNo = dr["inv_no"].ToString();
                        _ScrapPIList.InvDate = dr["inv_dt"].ToString();
                        _ScrapPIList.SalesPerson = dr["SalesPerson"].ToString();
                        _ScrapPIList.InvDt = dr["InvDt"].ToString();
                        _ScrapPIList.InvValue = dr["net_val_bs"].ToString();
                        _ScrapPIList.CustName = dr["cust_name"].ToString();
                        _ScrapPIList.InvStauts = dr["InvStauts"].ToString();
                        _ScrapPIList.CreateDate = dr["CreateDate"].ToString();
                        _ScrapPIList.ApproveDate = dr["ApproveDate"].ToString();
                        _ScrapPIList.ModifyDate = dr["ModifyDate"].ToString();
                        _ScrapPIList.create_by = dr["create_by"].ToString();
                        _ScrapPIList.app_by = dr["app_by"].ToString();
                        _ScrapPIList.mod_by = dr["mod_by"].ToString();
                        _ScrapPIList.cust_ref_no = dr["cust_ref_no"].ToString();
                        _SSIList.Add(_ScrapPIList);
                    }
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                ViewBag.FinStDt = DSet.Tables[2].Rows[0]["findate"];
                _SSIListModel.SSIList = _SSIList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialScrapSaleInvoiceList.cshtml", _SSIListModel);
        }
        public DataSet GetScrapSaleInvoiceEdit(string Inv_No, string Inv_Date)
        {
            try
            {
                //JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string Br_ID = string.Empty;
                string Voutype = "SV";
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
                DataSet result = _ScrapSI_ISERVICE.GetScrapSalesInvoiceDetail(Comp_ID, Br_ID, Voutype, Inv_No, Inv_Date, UserID, DocumentMenuId);
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
                ScrapSIModelattch _ScrapSIModelattch = new ScrapSIModelattch();
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
                _ScrapSIModelattch.Guid = DocNo;
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
                    _ScrapSIModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _ScrapSIModelattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _ScrapSIModelattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public string CheckSSIForCancellationinVoucher(string DocNo, string DocDate)
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
                /*Some changes in this proc due to third party tds on OC for auto generated DN by Hina on 08-07-2024  */
                DataSet Deatils = _ScrapSI_ISERVICE.CheckSSIDetail(Comp_ID, Br_ID, DocNo, DocDate);

                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = "InvoicecannotModifiedPaymentCreated";
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

        public JsonResult getsubitem(string ItemId)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = "", BrchID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds;
                ds = _ScrapSI_ISERVICE.getsubitem(ItemId, CompID, BrchID);
                //if (ds.Tables[0].Rows.Count > 0)
                    // var abc = ds.Tables[0].Rows[0]["sub_item"].ToString();
                    DataRows = Json(JsonConvert.SerializeObject(ds));/*Result convert into Json Format for javasript*/
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }

        [HttpPost]
        public JsonResult GetWarehouseList1()
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
                DataSet result = _ScrapSI_ISERVICE.GetWarehouseList(Comp_ID, Br_ID);
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
        public JsonResult GetSubitemdata(string Item_id, string UomId, string wh_id)
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
                DataTable dt = new DataTable();




                dt = _ScrapSI_ISERVICE.Scrap_GetSubItemDetails_INV_QTY(Comp_ID, Br_ID, Item_id, wh_id, UomId);


                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
 , string Flag, string Status, string Doc_no, string Doc_dt, string wh_id, string UomId)
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
                if (Status == "D" || Status == "F" || Status == "")
                {

                    dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, wh_id, Item_id, UomId, "wh").Tables[0];
                   // dt = _ScrapSI_ISERVICE.Scrap_GetSubItemDetails_INV_QTY(CompID, BrchID, Item_id, wh_id, UomId);

                    dt.Columns.Add("Qty", typeof(string));
                   
                   
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    DataTable NewDt = new DataTable();
                    string flag = "N";
                    int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        flag = "N";
                        foreach (JObject item in arr.Children())//
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);
                               
                               
                            }
                        }
                    }
                   
                }
                else
                {
                    // dt = _MaterialIssue_IServices.MI_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    dt = _ScrapSI_ISERVICE.Scrap_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "Scrap");
                }
                if (Flag == "AvlScrap")
                {
                    SubItemPopupDt subitmModel1 = new SubItemPopupDt
                    {
                        Flag = "AvlScrap",

                        dt_SubItemDetails = dt,
                        IsDisabled = IsDisabled,
                        decimalAllowed = "Y"

                    };
                    return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel1);
                }
                else
                {
                    SubItemPopupDt subitmModel = new SubItemPopupDt
                    {
                        Flag = "Scrap",

                        dt_SubItemDetails = dt,
                        IsDisabled = IsDisabled,
                        decimalAllowed = "Y"

                    };
                    return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
                }
                //SubItemPopupDt subitmModel = new SubItemPopupDt
                //{
                //    Flag  = "Scrap",

                //    dt_SubItemDetails = dt,           
                //    IsDisabled = IsDisabled,
                //    decimalAllowed = "Y"

                //};

                //return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId, string doc_status
           , string SelectedItemdetail, string DMenuId, string Command, string TransType, string MRSNo, string HdnitmRJOFlag, string UomId = null)
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
                DataSet ds = new DataSet();
              
                    if (ItemId != "")
                    {
                        ds = _ScrapSI_ISERVICE.getItemStockBatchWise(ItemId, UomId, WarehouseId, CompID, BrchID);
                    }
              
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {

                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    foreach (JObject item in jObjectBatch.Children())//
                    {
                        string ItmMatched = "N";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (item.GetValue("ItemId").ToString() == ds.Tables[0].Rows[i]["item_id"].ToString() && item.GetValue("LotNo").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("BatchNo").ToString() == ds.Tables[0].Rows[i]["batch_no"].ToString())
                            {
                                ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("inv_qty");
                                ItmMatched = "Y";
                            }
                        }
                        if(ItmMatched == "N"&& item.GetValue("wh_id").ToString() == WarehouseId && item.GetValue("ItemId").ToString() == ItemId)
                        {
                            DataRow dr = ds.Tables[0].NewRow();
                            dr["item_id"] = item.GetValue("ItemId").ToString();
                            dr["lot_id"] = item.GetValue("LotNo").ToString();
                            dr["batch_no"] = item.GetValue("BatchNo").ToString();
                            dr["avl_batch_qty"] = item.GetValue("avl_batch_qty").ToString();
                            dr["expiry_date"] = item.GetValue("ExpiryDate").ToString();
                            dr["exp_date"] = item.GetValue("ExpiryDate").ToString();
                            dr["exp_dt"] = item.GetValue("ExpiryDate").ToString();
                            dr["issue_qty"] = item.GetValue("inv_qty").ToString();
                            dr["mfg_name"] = item.GetValue("mfg_name").ToString();
                            dr["mfg_mrp"] = item.GetValue("mfg_mrp").ToString();
                            dr["mfg_date"] = item.GetValue("mfg_date").ToString();
                            ds.Tables[0].Rows.Add(dr);
                        }
                        
                    }
                    
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockBatchWise = ds.Tables[0];
                }           
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = doc_status;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult getItemStockBatchWiseAfterStockUpadte( string Doc_No, string Doc_dt, string ItemID, string doc_status
    , string DMenuId, string Command, string TransType, string UomId = null,string WarehouseId=null)
        {
            try
            {
                DataSet ds = new DataSet();
                string CompID = string.Empty;
                string br_id = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                ds = _ScrapSI_ISERVICE.getItemStockBatchWiseAfterStockUpdate(CompID, br_id, Doc_No, Doc_dt, ItemID, UomId);
                if (ds.Tables[0].Rows.Count > 0)
                ViewBag.ItemStockBatchWise = ds.Tables[0];
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = doc_status;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult getItemstockSerialWise(string ItemId, string WhID, string Scrap_Status, string SelectedItemSerial, string Transtype, string Command)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                ds = _ScrapSI_ISERVICE.getItemstockSerialWise(CompID, BrchID, ItemId, WhID);

                if (SelectedItemSerial != null && SelectedItemSerial != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemSerial);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("LOTId").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("SerialNO").ToString() == ds.Tables[0].Rows[i]["serial_no"].ToString())
                            {
                                ds.Tables[0].Rows[i]["SerailSelected"] = "Y";
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockSerialWise = ds.Tables[0];


                ViewBag.DocID = DocumentMenuId;
                ViewBag.Transtype = Transtype;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = Scrap_Status;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemstockSerialWiseAfterStockUpadte(string Docno, string Docdt, string ItemID, string Transtype, string Command)
        {
            try
            {
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                ds = _ScrapSI_ISERVICE.getItemstockSerialWiseAfterStockUpdate(CompID, BrchID, Docno, Docdt, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                ViewBag.Transtype = Transtype;
                ViewBag.Command = Command;
                ViewBag.DocID = DocumentMenuId;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        /*--------------------------For PDF Print Start code by Hina on 24-07-2024--------------------------*/
        public FileResult GenratePdfFile(ScrapSIModel _model)
        {
            //PrintOptionsList ProntOption = new PrintOptionsList();
            //if (_model.HdnPrintOptons == "Y")
            //{
            //    ProntOption.PrtOpt_catlog_number = _model.PrtOpt_catlog_number == "Y" ? true : false;
            //    ProntOption.PrtOpt_item_code = _model.PrtOpt_item_code == "Y" ? true : false;
            //    ProntOption.PrtOpt_item_desc = _model.PrtOpt_item_desc == "Y" ? true : false;
            //}
            //else
            //{
            //    ProntOption.PrtOpt_catlog_number = true;
            //    ProntOption.PrtOpt_item_code = true;
            //    ProntOption.PrtOpt_item_desc = true;
            //}
            DataTable dt = new DataTable();
            dt = PrintFormatDataTable(_model,null);
            ViewBag.PrintOption = dt;
            //return File(GetPdfData(_model.DocumentMenuId, _model.inv_no, _model.inv_dt, ProntOption), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
            if (_model.GstApplicable == "Y")
                return File(GetPdfDataOfGstInv(dt, _model.DocumentMenuId, _model.Sinv_no, _model.Sinv_dt, _model.NumberofCopy), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");
            else
                return File(GetPdfData(_model.DocumentMenuId, _model.Sinv_no, _model.Sinv_dt, _model.NumberofCopy), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");
        }
        public byte[] GetPdfData(string docId, string invNo, string invDt, int NumberofCopy/*, PrintOptionsList ProntOption*/)
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
                if (NumberofCopy == 0)/* Added by Suraj Maurya on 26-05-2025 to added default copy */
                {
                    NumberofCopy = 1;
                }
                DataSet Details = _ScrapSI_ISERVICE.GetScrapSaleInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt);
                ViewBag.PageName = "SI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
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
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";

                //ViewBag.Title = "Sales Invoice";
                //htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoicePrint.cshtml"));


                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                     
                    pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                        
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
                        ViewBag.Title = "Sales Invoice";
                        htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoicePrint.cshtml"));
                        reader = new StringReader(htmlcontent);

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                        pdfDoc.NewPage();
                    }
                    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = HeaderFooterPagination(bytes, Details);
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return null;
                throw ex;
            }
            finally
            {

            }
        }
        public byte[] GetPdfDataOfGstInv(DataTable dt, string docId, string invNo, string invDt, int NumberofCopy/*, PrintOptionsList ProntOption*/)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                string Br_ID = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    Br_ID = Session["BranchId"].ToString();

                if (NumberofCopy == 0)/* Added by Suraj Maurya on 26-05-2025 to added default copy */
                {
                    NumberofCopy = 1;
                }
                DataSet Details = _ScrapSI_ISERVICE.GetScrapSlsInvGstDtlForPrint(CompID, Br_ID, invNo, invDt);
                ViewBag.PageName = "SI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();

                ViewBag.Details = Details;
                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.TotalshipQty = Details.Tables[13].Rows[0]["TotalshipQty"].ToString().Trim();
                //ViewBag.ProntOption = ProntOption;
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
                //ViewBag.Title = "Tax Invoice";
                //if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                //{
                //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceWithGSTPrintF2.cshtml"));
                //}
                //else
                //{
                //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceWithGSTPrint.cshtml"));
                //}
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F3")
                    {
                        pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 60f);
                    }
                    else
                    {
                        pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 60f);
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
                        ViewBag.Title = "Tax Invoice";
                        if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F2")
                        {
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceWithGSTPrintF2.cshtml"));
                        }
                        else if (dt.Rows[0]["PrintFormat"].ToString().ToUpper() == "F3")
                        {
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceWithGSTPrintF3.cshtml"));
                        }
                        else
                        {
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/ScrapSaleInvoice/ScrapSaleInvoiceWithGSTPrint.cshtml"));
                        }
                        reader = new StringReader(htmlcontent);

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                        pdfDoc.NewPage();
                    }
                    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = GSTHeaderFooterPagination(bytes, Details, NumberofCopy);
                    return bytes.ToArray();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
                //return null;
            }
            finally
            {

            }
        }
        private Byte[] HeaderFooterPagination(Byte[] bytes, DataSet Details)
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
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(20, 220);
                                draftimg.ScaleAbsolute(550f, 600f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (docstatus == "D" || docstatus == "F"  || docstatus == "C")
                                    {
                                        content.AddImage(draftimg);
                                    }
                                    Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 570, 15, 0);
                                }
                     }
                    bytes = ms.ToArray();
                }
            }

            return bytes;
        }
        private Byte[] GSTHeaderFooterPagination(Byte[] bytes, DataSet Details, int NumberofCopy)
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
            Font font2 = new Font(bf, 6, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 15, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());
            string State_Name = Details.Tables[7].Rows[0]["state_name"].ToString();
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
                        var draftimg = Image.GetInstance(draftImage);
                        draftimg.SetAbsolutePosition(20, 40);
                        draftimg.ScaleAbsolute(650f, 600f);

                        var qrCode = Image.GetInstance(QR);
                        qrCode.SetAbsolutePosition(475, 710);//Position Change by Suraj Maurya on 31-03-2025 to 475,720
                        qrCode.ScaleAbsolute(100f, 95f);

                        int PageCount = reader1.NumberOfPages;
                        var PageCount1 = reader1.NumberOfPages / NumberofCopy;

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
                            //    if (i == 1)
                            //        content.AddImage(qrCode);
                            //}
                            //Phrase ptitle = new Phrase(String.Format("Tax Invoice", i, PageCount), fonttitle);
                            //int pageNumber = PageCount1;

                            if (PageCount1 > count)//add by shubham maurya on 17-04-2025 
                                    count = count + 1;
                            else
                                count = 1;

                            if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["inv_qr_code"].ToString()))
                            {
                                if (count == 1)
                                    content.AddImage(qrCode);
                            }
                            Phrase p = new Phrase(String.Format("Page {0} of {1}", count, PageCount1), font);
                            //if (i == PageCount)
                            if (count == PageCount1)
                            {
                                //try
                                //{
                                //    //var bnetlogo = Image.GetInstance(bnetImage);
                                //    //bnetlogo.SetAbsolutePosition(322, 30);//businet footer logo//
                                //    //bnetlogo.ScaleAbsolute(70f, 16f);
                                //    //content.AddImage(bnetlogo);
                                //    var bnetlogo = Image.GetInstance(bnetImage);
                                //    bnetlogo.SetAbsolutePosition(322, 38);
                                //    bnetlogo.ScaleAbsolute(40f, 10f);
                                //    content.AddImage(bnetlogo);

                                //}
                                //catch { }
                                ////Phrase ftr = new Phrase(String.Format("This Document is generated by ", i, PageCount), font);
                                ////ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, ftr, 320, 30, 0);
                                ////Phrase ftr1 = new Phrase(String.Format("SUBJECT TO " + StateName + " JURISDICTION ", i, PageCount), font);
                                ////ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ftr1, 300, 50, 0);
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
        //public ActionResult GetSelectPrintTypePopup(string PrintOptions, string DocMenuId)
        //{
        //    try
        //    {
        //        ViewBag.PrintCommand = "Y";
        //        ViewBag.PrintOptions = PrintOptions;
        //        ViewBag.DocMenuId = DocMenuId;
        //        return PartialView("~/Areas/Common/Views/Cmn_SelectPrintType.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }

        //}

        /*--------------------------For PDF Print End--------------------------*/
        public ActionResult GetSalesPersonList(ScrapSIModel _ScrapSIModel)
        {
            string SalesPersonName = string.Empty;
            Dictionary<string, string> SPersonList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            string UserName = string.Empty;
            string rpt_id = string.Empty;
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
                if (string.IsNullOrEmpty(_ScrapSIModel.SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = _ScrapSIModel.SalePerson;
                }
                //SPersonList = _ScrapSI_ISERVICE.GetSalesPersonList(Comp_ID, SalesPersonName, Br_ID);
                SPersonList = _ScrapSI_ISERVICE.GetSalesPersonList(Comp_ID, SalesPersonName, Br_ID, UserID, _ScrapSIModel.Sinv_no, _ScrapSIModel.Sinv_dt);
                List<SalesPersonName> _SlPrsnList = new List<SalesPersonName>();
                if ((rpt_id == "0" || _ScrapSIModel.TransType == "Update" || UserID == "1001")&& crm=="Y")
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

                _ScrapSIModel.SalesPersonNameList = _SlPrsnList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(SPersonList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        //Added by Nidhi on 27-08-2025
        public string SavePdfDocToSendOnEmailAlert_Ext(ScrapSIModel model, string Doc_no, string Doc_dt, string docid, string fileName, string PrintFormat, string GstApplicable)
        {
            DataTable dt = new DataTable();
            var commonCont = new CommonController(_Common_IServices);
            if(string.IsNullOrEmpty(PrintFormat))
            {
                dt = PrintFormatDataTable(model, PrintFormat);
            }
            else
            {
                dt = PrintFormatDataTable(null, PrintFormat);
            }
            ViewBag.PrintOption = dt;

            ViewBag.PrintOption = dt;
            if (GstApplicable == "Y")
            {
                var data = GetPdfDataOfGstInv(dt, "105103148", Doc_no, Doc_dt, 1);
                return commonCont.SaveAlertDocument_MailExt(data, fileName);
            }
            else
            {
                var data = GetPdfData("105103148", Doc_no, Doc_dt, 1);
                return commonCont.SaveAlertDocument_MailExt(data, fileName);
            }
        }
        public ActionResult SendEmailAlert(ScrapSIModel _model, string mail_id, string status, string docid, string SrcType, string Doc_no, string Doc_dt, string statusAM, string filepath,string GstApplicable)
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
                            string fileName = "DSI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
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
        private object IsBlank(string input, object output)//Added by Suraj Maurya on 27-11-2025
        {
            return input == "" ? output : input;
        }
    }

}
