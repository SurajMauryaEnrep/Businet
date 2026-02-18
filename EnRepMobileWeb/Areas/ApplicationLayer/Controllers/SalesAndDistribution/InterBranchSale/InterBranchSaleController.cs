using EnRepMobileWeb.Areas.BusinessLayer.Controllers.TransporterSetup;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SalesAndDistribution.InterBranchSale;
using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.Resources;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SalesAndDistribution.InterBranchSale;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TransporterSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SalesAndDistribution.InterBranchSale
{
    public class InterBranchSaleController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105103149", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly TransporterSetup_ISERVICES _trptIService;
        Common_IServices _Common_IServices;
        InterBranchSale_ISERVICE _IBS_ISERVICE;
        CommonController cmn = new CommonController();
        public InterBranchSaleController(Common_IServices _Common_IServices, InterBranchSale_ISERVICE _IBS_ISERVICE, TransporterSetup_ISERVICES trptIService)
        {
            this._Common_IServices = _Common_IServices;
            this._IBS_ISERVICE = _IBS_ISERVICE;
            this._trptIService = trptIService;
        }
        // GET: ApplicationLayer/InterBranchSale
        public ActionResult InterBranchSale(string wfStatus)
        {
            CommonPageDetails();
            IBSListModel _IBSListModel = new IBSListModel();
            if (wfStatus != null)
            {
                _IBSListModel.wfstatus = wfStatus;
                _IBSListModel.ListFilterData = "0,0,0,0" + "," + wfStatus;
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;
            if (_IBSListModel.SSI_FromDate == null)
            {
                _IBSListModel.FromDate = startDate;
                _IBSListModel.SSI_FromDate = startDate;
                _IBSListModel.ToDate = CurrentDate;
                _IBSListModel.SSI_ToDate = CurrentDate;
            }
            else
            {
                _IBSListModel.FromDate = _IBSListModel.SSI_FromDate;
                
            }
            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            _IBSListModel.StatusList = statusLists;
            if (TempData["UrlData"] != null)
            {
                if (TempData["UrlData"].ToString() != "" && TempData["UrlData"].ToString() != null)
                {
                    UrlData urlData = TempData["UrlData"] as UrlData;
                    if (urlData.ListFilterData1 != null&& urlData.ListFilterData1 != "undefined")
                    {
                        var arr = urlData.ListFilterData1.Split(',');
                        _IBSListModel.CustID = arr[0];
                        _IBSListModel.FromDate = arr[1];
                        _IBSListModel.ToDate = arr[2];
                        _IBSListModel.Status = arr[3];
                        _IBSListModel.wfstatus = arr[4];
                        if (wfStatus == null)
                        {
                            wfStatus = arr[3];
                        }
                        _IBSListModel.ListFilterData = _IBSListModel.CustID + "," + _IBSListModel.FromDate + "," + _IBSListModel.ToDate + "," + _IBSListModel.Status + "," + wfStatus;
                    }

                }
            }
            GetAllData(_IBSListModel);
            _IBSListModel.Title = title;
            return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/InterBranchSale/InterBranchSaleList.cshtml", _IBSListModel);
        }
        private void GetAllData(IBSListModel _IBSListModel)
        {
            try
            {
                string SupplierName = string.Empty;
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
                if (string.IsNullOrEmpty(_IBSListModel.CustName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _IBSListModel.CustName;
                }
                DataSet CustList = _IBS_ISERVICE.GetAllData(CompID, SupplierName, BrchID, UserID
                    , _IBSListModel.CustID, _IBSListModel.FromDate, _IBSListModel.SSI_ToDate, _IBSListModel.Status, "105103149", _IBSListModel.wfstatus);

                List<CustomerName> _CustList = new List<CustomerName>();
                foreach (DataRow data in CustList.Tables[0].Rows)
                {
                    CustomerName _CustDetail = new CustomerName();
                    _CustDetail.Cust_id = data["cust_id"].ToString();
                    _CustDetail.Cust_name = data["cust_name"].ToString();
                    _CustList.Add(_CustDetail);
                }
                _CustList.Insert(0, new CustomerName() { Cust_id = "0", Cust_name = "All" });
                _IBSListModel.CustomerNameList = _CustList;
                ViewBag.ListDetail = CustList.Tables[1];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        
        //public ActionResult AddInterBranchSaleDetail()
        //{
        //    Session["Message"] = "New";
        //    Session["Command"] = "Add";
        //    Session["AppStatus"] = 'D';
        //    Session["TransType"] = "Save";
        //    Session["BtnName"] = "BtnAddNew";
        //    return RedirectToAction("InterBranchSaleDetail", "InterBranchSale");
        //}
        public ActionResult AddIBSDetail(string DocNo, string DocDate, string ListFilterData)
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
                        return RedirectToAction("InterBranchSale", "InterBranchSale", urlData);
                    }
                }

                /*End to chk Financial year exist or not*/
                string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = DocNo == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, DocDate, ListFilterData);
                return RedirectToAction("InterBranchSaleDetail", "InterBranchSale", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult InterBranchSaleDetail(UrlData urlData)
        {
            try
            {
                CommonPageDetails();
                InterBranchSale_Model model = new InterBranchSale_Model();
                model.GstApplicable= ViewBag.GstApplicable;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                    UserID = Session["userid"].ToString();
                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.Inv_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    model.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                }
                model.Command = urlData.Command;
                model.EditCommand = urlData.Command;
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.Inv_no = urlData.Inv_no;
                model.Inv_dt = urlData.Inv_dt;
                model.ListFilterData1 = urlData.ListFilterData1;

                List<CustomerName> suppLists = new List<CustomerName>();
                suppLists.Add(new CustomerName { Cust_id = model.cust_id, Cust_name = model.cust_id });
                model.CustomerNameList = suppLists;

                DataTable dt = new DataTable();
                List<Warehouse> requirementAreaLists = new List<Warehouse>();
                dt = GetWarehouseList();
                foreach (DataRow dr in dt.Rows)
                {
                    Warehouse WarehouseList = new Warehouse();
                    WarehouseList.wh_id = dr["wh_id"].ToString();
                    WarehouseList.wh_name = dr["wh_name"].ToString();
                    requirementAreaLists.Add(WarehouseList);
                }
                requirementAreaLists.Insert(0, new Warehouse() { wh_id = "0", wh_name = "---Select---" });
                model.WarehouseList = requirementAreaLists;

                model.TransList = GetTransporterList();

                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;

                if (model.Inv_no != null && model.Inv_dt != null)
                {
                    DataSet ds = GetIBSEdit(model.Inv_no, model.Inv_dt);
                    ViewBag.AttechmentDetails = ds.Tables[8];
                    ViewBag.GLAccount = ds.Tables[7];
                    ViewBag.GLTOtal = ds.Tables[9];
                    ViewBag.ItemTaxDetails = ds.Tables[2];
                    ViewBag.OtherChargeDetails = ds.Tables[3];
                    ViewBag.TotalDetails = ds.Tables[0];
                    ViewBag.ItemTaxDetailsList = ds.Tables[10];
                    ViewBag.OCTaxDetails = ds.Tables[11];
                    ViewBag.SubItemDetails = ds.Tables[14];
                    ViewBag.ItemTDSDetails = ds.Tables[13];
                    ViewBag.ItemOC_TDSDetails = ds.Tables[16];
                    ViewBag.ItemStockBatchWise = ds.Tables[15];
                    ViewBag.ItemStockSerialWise = ds.Tables[17];
                    model.Inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                    model.Inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                    model.conv_rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(QtyDigit);
                    model.cust_id = ds.Tables[0].Rows[0]["cust_id"].ToString();
                    model.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                    model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                    model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                            model.Create_by = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            model.Create_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            model.Approved_by = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            model.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            model.Amended_by = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            model.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            model.StatusName = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                            model.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                            model.Ship_Gst_number = ds.Tables[0].Rows[0]["cust_gst_no"].ToString();
                            model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                            model.Address = ds.Tables[0].Rows[0]["CustAddress"].ToString();
                            model.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                            model.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt"]).ToString(ValDigit);
                            model.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                            model.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                            model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            model.bs_curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                            
                            model.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                            model.Currency = ds.Tables[0].Rows[0]["curr_Name"].ToString();
                            model.cust_acc_id = ds.Tables[0].Rows[0]["cust_acc_id"].ToString();
                            model.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                            model.SSIStatus = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            model.GR_No = ds.Tables[0].Rows[0]["gr_no"].ToString();
                            model.GR_Dt = ds.Tables[0].Rows[0]["gr_date"].ToString();
                            //model.GR_Dt = ds.Tables[0].Rows[0]["gr_dt"].ToString();
                            model.HdnGRDate = ds.Tables[0].Rows[0]["gr_date"].ToString();
                            model.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_name"].ToString();
                            model.Veh_Number = ds.Tables[0].Rows[0]["veh_number"].ToString();
                            model.Driver_Name = ds.Tables[0].Rows[0]["driver_name"].ToString();
                            model.Mob_No = ds.Tables[0].Rows[0]["mob_no"].ToString();
                            model.Tot_Tonnage = ds.Tables[0].Rows[0]["tot_tonnage"].ToString();
                            model.No_Of_Packages = ds.Tables[0].Rows[0]["no_of_pkgs"].ToString();
                            
                            model.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                            model.CustRefNo = ds.Tables[0].Rows[0]["cust_ref_no"].ToString();
                            model.CustRefDt = ds.Tables[0].Rows[0]["cust_ref_dt"].ToString();
                            model.PlaceOfSupply = ds.Tables[0].Rows[0]["placeofsupply"].ToString();
                            // model.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                            //model.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                            model.IRNNumber = ds.Tables[0].Rows[0]["gst_irn_no"].ToString();
                            //model.slprsn_id = ds.Tables[0].Rows[0]["sls_per"].ToString();
                            model.Ship_Add_Id = ds.Tables[0].Rows[0]["ship_add_id"].ToString();
                            model.ShippingAddress = ds.Tables[0].Rows[0]["ship_address"].ToString();
                            if (ds.Tables[0].Rows[0]["state_code"] == ds.Tables[0].Rows[0]["br_state_code"])
                            {
                                model.Hd_GstType = "Both";
                            }
                            else
                            {
                                model.Hd_GstType = "IGST";
                            }
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                            string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                            string doc_status = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                            string roundoff_status = ds.Tables[0].Rows[0]["roundoff"].ToString().Trim();
                            model.pmflagval = ds.Tables[0].Rows[0]["pm_flag"].ToString().Trim();
                            if (roundoff_status == "Y")
                            {
                                model.RoundOffFlag = true;
                            }
                            else
                            {
                                model.RoundOffFlag = false;
                            }
                            model.DocumentStatus = doc_status;
                            model.ShipFromAddress = ds.Tables[0].Rows[0]["ship_from_addr"].ToString();
                            model.ddlPayment_term = ds.Tables[0].Rows[0]["pay_term"].ToString();
                            model.ddlDelivery_term = ds.Tables[0].Rows[0]["deli_term"].ToString();
                            model.Declaration_1 = ds.Tables[0].Rows[0]["declar_1"].ToString();
                            model.Declaration_2 = ds.Tables[0].Rows[0]["declar_2"].ToString();
                            model.Invoice_Heading = ds.Tables[0].Rows[0]["inv_heading"].ToString();
                            string nontaxable = ds.Tables[0].Rows[0]["non_taxable"].ToString();
                            if (nontaxable == "Y")
                            {
                                model.nontaxable = true;
                            }
                            else
                            {
                                model.nontaxable = false;
                            }
                            ViewBag.DocumentCode = doc_status;
                    if (ds.Tables[7].Rows.Count > 0)
                    {
                        if (doc_status == "A" || doc_status == "C")
                        {
                            model.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                        }
                        model.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                        model.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                        ViewBag.GLVoucherNo = model.GLVoucherNo;/*add by Hina sharma on 19-08-2025*/
                    }
                    model.DocumentStatus = doc_status;
                    if (doc_status == "C")
                    {
                        model.CancelFlag = true;
                        model.BtnName = "Refresh";
                    }
                    else
                    {
                        model.CancelFlag = false;
                    }
                    model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                    model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                    if (doc_status != "D" && doc_status != "F")
                    {
                        ViewBag.AppLevel = ds.Tables[6];
                    }
                    if (ViewBag.AppLevel != null && model.Command != "Edit")
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
                                model.BtnName = "Refresh";
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
                                    model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    ViewBag.Approve = "N";
                                    ViewBag.ForwardEnbl = "Y";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (UserID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                model.BtnName = "BtnToDetailPage";
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
                                    model.BtnName = "BtnToDetailPage";
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
                                model.BtnName = "BtnToDetailPage";
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
                                model.BtnName = "BtnToDetailPage";
                            }
                        }
                        if (doc_status == "A")
                        {
                            if (create_id == UserID || approval_id == UserID)
                            {
                                model.BtnName = "BtnToDetailPage";
                            }
                            else
                            {
                                model.BtnName = "Refresh";
                            }
                        }
                    }
                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }
                    ViewBag.CostCenterData = ds.Tables[12];
                    ViewBag.ItemDetails = ds.Tables[1];
                }
                GetAutoCompleteSearchCustList(model);
                //GetSalesPersonList(model);
                model.Title = title;
                model._ModelCommand = model.Command;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/InterBranchSale/InterBranchSaleDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string Inv_no = null, string Inv_dt = null, string ListFilterData1 = null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Inv_no = Inv_no;
                urlData.Inv_dt = Inv_dt;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionIBSDeatils(InterBranchSale_Model _model, string Command)
        {
            try
            {
                var commCont = new CommonController(_Common_IServices);
                UrlData urlData = new UrlData();
                if (_model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                if (_model.DeleteCommand == "Edit")
                {
                    Command = "Edit";
                }
                if (Command == null)
                {
                    if (_model.EditCommand == "UpdateTransPortDetail")
                    {
                        Command = "UpdateTransPortDetail";
                    }
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
                            if (!string.IsNullOrEmpty(_model.Inv_no))
                            {
                                SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", "Financial Year not Exist", _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                                return RedirectToAction("InterBranchSaleDetail", _model);
                            }
                            else
                            {
                                SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, null, _model.ListFilterData1);
                                return RedirectToAction("InterBranchSaleDetail", _model);
                            }
                        }
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, null, _model.ListFilterData1);
                        return RedirectToAction("InterBranchSaleDetail", urlData);
                    case "Edit":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        string invdt = _model.Inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                            return RedirectToAction("InterBranchSaleDetail", urlData);
                        }
                        if (_model.SSIStatus == "A")
                        {
                            string checkforCancle = CheckDependencyIBS(_model.Inv_no, _model.Inv_dt);
                            if (checkforCancle != "")
                            {
                                SetUrlData(urlData, "Refresh", "Update", "BtnToDetailPage", checkforCancle, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                            }
                            else
                            {
                                SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                            }
                        }
                        else
                        {
                            SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                        }
                        return RedirectToAction("InterBranchSaleDetail", urlData);
                    case "UpdateTransPortDetail":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        string SSInvDate4 = _model.Inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, SSInvDate4) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickOnList", new { DocNo = _model.Inv_no, DocDate = _model.Inv_dt, ListFilterData = _model.ListFilterData1, WF_status = _model.WFStatus });
                        }
                        
                        SetUrlData(urlData, "UpdateTransPortDetail", "Update", "BtnEdit", null, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                        return RedirectToAction("InterBranchSaleDetail", urlData);
                    case "Save":
                        if (_model.EditCommand == "UpdateTransPortDetail")
                        {
                            _model.TransType = "UpdateTransPort";
                        }
                        SaveIBSDetails(_model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                        return RedirectToAction("InterBranchSaleDetail", urlData);
                    case "Approve":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        string Invdt1 = _model.Inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, Invdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", _model.Message, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                            return RedirectToAction("InterBranchSaleDetail", urlData);
                        }
                        //ApproveDPIDetails(_model, _model.Inv_no, _model.Inv_dt, "", "", "", _model.Nurration, "", "", "", _model.BP_Nurration, _model.DN_Nurration);
                        ApproveIBSDetails(_model, _model.PV_Narration, _model.BP_Narration, _model.DN_Narration);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1);
                        return RedirectToAction("InterBranchSaleDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _model.ListFilterData1);
                        return RedirectToAction("InterBranchSaleDetail", urlData);
                    case "Delete":
                        DeleteIBSDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, null, _model.ListFilterData1);
                        return RedirectToAction("InterBranchSaleDetail", urlData);
                    case "BacktoList":
                        SetUrlData(urlData, "", "", "", null, null, null, _model.ListFilterData1);
                        return RedirectToAction("InterBranchSale");
                    case "Print":
                        return GenratePdfFile(_model);
                        //return RedirectToAction("InterBranchSale");
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("InterBranchSaleDetail", urlData);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult SaveIBSDetails(InterBranchSale_Model _model)
        {
            try
            {
                string SaveMessage = "";
                string PageName = _model.Title.Replace(" ", "");

                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                    UserID = Session["userid"].ToString();

                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblSubItem = new DataTable();
                DataTable ItemBatchDetails = new DataTable();
                DataTable ItemSerialDetails = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblIOCDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable DtblVouDetail = new DataTable();
                DataTable CostCenterDetails = new DataTable();
                DataTable DtblTdsDetail = new DataTable();
                DataTable DtblOCTdsDetail = new DataTable();

                DtblHDetail = ToDtblHeaderDetail(_model);
                DtblItemDetail = ToDtblItemDetail(_model.Itemdetails);
                DtblSubItem = ToDtblSubItem(_model.SubItemDetailsDt);
                ItemBatchDetails = ToDtblbatchDetail(_model.ItemBatchWiseDetail);
                ItemSerialDetails = ToDtblserialDetail(_model.ItemSerialWiseDetail);
                DtblTaxDetail = ToDtblTaxDetail(_model.ItemTaxdetails);
                DtblOCTaxDetail = ToDtblTaxDetail(_model.OC_TaxDetail);
                DtblIOCDetail = ToDtblOcDetail(_model.ItemOCdetails);
                CostCenterDetails = ToDtblccDetail(_model.CC_DetailList);
                DtblAttchDetail = ToDtblAttachmentDetail(_model);
                DtblVouDetail = ToDtblvouDetail(_model.vouDetail);
                DtblOCTdsDetail = ToDtblOCTdsDetail(_model.oc_tds_details);
                var _IBSAttch = TempData["ModelDataattch"] as IBSAttch;
                TempData["ModelDataattch"] = null;
                string Narr = "";
                string CN_Narr = "";
                if (_model.CancelFlag != false)
                {
                    Narr = _model.PV_Narration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                    CN_Narr = _model.CN_Narration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }
                SaveMessage = _IBS_ISERVICE.InsertIBSDetails(DtblHDetail, DtblItemDetail, DtblTaxDetail
                  , DtblOCTaxDetail, DtblIOCDetail, DtblAttchDetail, DtblVouDetail, CostCenterDetails, DtblSubItem
                  , ItemBatchDetails, DtblOCTdsDetail, Narr, CN_Narr, ItemSerialDetails);
                if (SaveMessage == "DocModify")
                {
                    _model.Message = "DocModify";
                    _model.BtnName = "Refresh";
                    _model.Command = "Refresh";
                    _model.DocumentMenuId = DocumentMenuId;
                    TempData["ModelData"] = _model;
                    return RedirectToAction("InterBranchSaleDetail");
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
                        _model.Message = Message;
                        return RedirectToAction("InterBranchSaleDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_IBSAttch != null)
                        {
                            if (_IBSAttch.Guid != null)
                            {
                                Guid = _IBSAttch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, _model.TransType, DtblAttchDetail);
                        //string Guid = "";
                        //if (_IBSAttch != null)
                        //{
                        //    if (_IBSAttch.Guid != null)
                        //    {
                        //        Guid = _IBSAttch.Guid;
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
                        try
                        {
                            ViewBag.PrintFormat = PrintFormatDataTable(_model,null);
                            string fileName = "DSI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(Inv_no, Inv_DATE, fileName, null, _model.GstApplicable);
                            _Common_IServices.SendAlertEmail(CompID, BrchID, "105103148", Inv_no, "Cancel", UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }


                        _model.Message = _model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        _model.Command = "Update";
                        _model.Inv_no = Inv_no;
                        _model.Inv_dt = Inv_DATE;
                        _model.TransType = "Update";
                        _model.AppStatus = "D";
                        _model.BtnName = "Refresh";
                    }
                    else
                    {
                        if (Message == "Update" || Message == "Save" || Message == "UpdateTransPort")
                        {
                            _model.Message = "Save";
                            _model.Command = "Update";
                            _model.Inv_no = Inv_no;
                            _model.Inv_dt = Inv_DATE;
                            _model.TransType = "Update";
                            _model.AppStatus = "D";
                            _model.BtnName = "BtnSave";
                            _model.AttachMentDetailItmStp = null;
                            _model.Guid = null;
                        }
                    }
                    return RedirectToAction("InterBranchSaleDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private DataTable ToDtblHeaderDetail(InterBranchSale_Model _model)
        {
            try
            {
                DataTable dtheaderdeatil = new DataTable();
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
                dtheader.Columns.Add("cust_ref_no", typeof(string));
                dtheader.Columns.Add("cust_ref_dt", typeof(string));
                dtheader.Columns.Add("placeofsupply", typeof(string));
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("UserID", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("tax_amt", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(string));
                dtheader.Columns.Add("GR_Number", typeof(string));
                dtheader.Columns.Add("GR_Dt", typeof(string));
                dtheader.Columns.Add("Trans_Name", typeof(string));
                dtheader.Columns.Add("Veh_Number", typeof(string));
                dtheader.Columns.Add("Driver_Name", typeof(string));
                dtheader.Columns.Add("Mob_No", typeof(string));
                dtheader.Columns.Add("Tot_Tonnage", typeof(string));
                dtheader.Columns.Add("No_Of_Pkgs", typeof(string));
                dtheader.Columns.Add("Narration", typeof(string));
                dtheader.Columns.Add("ship_add_id", typeof(string));
                dtheader.Columns.Add("ship_from_addr", typeof(string));
                dtheader.Columns.Add("sls_per", typeof(string));
                dtheader.Columns.Add("pay_term", typeof(string));
                dtheader.Columns.Add("deli_term", typeof(string));
                dtheader.Columns.Add("declar_1", typeof(string));
                dtheader.Columns.Add("declar_2", typeof(string));
                dtheader.Columns.Add("inv_heading", typeof(string));
                dtheader.Columns.Add("non_taxable", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _model.TransType;
                dtrowHeader["MenuID"] = "105103149";
                string cancelflag = _model.CancelFlag.ToString();
                if (cancelflag == "False")
                    dtrowHeader["Cancelled"] = "N";
                else
                    dtrowHeader["Cancelled"] = "Y";
                string roundoffflag = _model.RoundOffFlag.ToString();
                if (roundoffflag == "False")
                    dtrowHeader["roundoff"] = "N";
                else
                    dtrowHeader["roundoff"] = "Y";
                dtrowHeader["pm_flag"] = _model.pmflagval;
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                if (_model.Inv_no == null)
                    dtrowHeader["inv_no"] = null;
                else
                    dtrowHeader["inv_no"] = _model.Inv_no;              
                dtrowHeader["inv_dt"] = _model.Inv_dt;
                dtrowHeader["cust_id"] = _model.cust_id;
                dtrowHeader["bill_add_id"] = _model.bill_add_id == null ? "0" : _model.bill_add_id;
                dtrowHeader["remarks"] = _model.Remarks;
                dtrowHeader["cust_ref_no"] = _model.CustRefNo;/*Add by Hina shrama on 04-03-2025 */
                dtrowHeader["cust_ref_dt"] = _model.CustRefDt;/*Add by Hina shrama on 05-03-2025 */
                dtrowHeader["placeofsupply"] = _model.PlaceOfSupply;/*Add by Hina shrama on 10-10-2024 */
                dtrowHeader["inv_status"] = IsNull(_model.AppStatus, "D");
                dtrowHeader["UserID"] = Session["UserId"].ToString();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["gr_val"] = _model.GrossValue;
                dtrowHeader["tax_amt"] = _model.TaxAmount;
                dtrowHeader["oc_amt"] = _model.OtherCharges;
                dtrowHeader["net_val_bs"] = _model.NetAmountInBase;
                dtrowHeader["GR_Number"] = _model.GR_No;
                dtrowHeader["GR_Dt"] = _model.HdnGRDate;
                dtrowHeader["Trans_Name"] = _model.Transpt_NameID;
                dtrowHeader["Veh_Number"] = _model.Veh_Number;
                dtrowHeader["Driver_Name"] = _model.Driver_Name;
                dtrowHeader["Mob_No"] = _model.Mob_No;
                dtrowHeader["Tot_Tonnage"] = _model.Tot_Tonnage;
                dtrowHeader["No_Of_Pkgs"] = _model.No_Of_Packages;
                dtrowHeader["Narration"] = _model.Narration;
                dtrowHeader["ship_add_id"] = _model.Ship_Add_Id;
                dtrowHeader["ship_from_addr"] = _model.ShipFromAddress;
                dtrowHeader["sls_per"] = "0";
                dtrowHeader["pay_term"] = _model.ddlPayment_term;
                dtrowHeader["deli_term"] = _model.ddlDelivery_term;
                dtrowHeader["declar_1"] = _model.Declaration_1;
                dtrowHeader["declar_2"] = _model.Declaration_2;
                dtrowHeader["inv_heading"] = _model.Invoice_Heading;
                string nontaxable = _model.nontaxable.ToString();
                if (nontaxable == "False")
                    dtrowHeader["non_taxable"] = "N";
                else
                    dtrowHeader["non_taxable"] = "Y";
                dtheader.Rows.Add(dtrowHeader);
                return dtheader;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblItemDetail(string ItemDetails)
        {
            try
            {
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(string));
                dtItem.Columns.Add("HsnNo", typeof(string));
                dtItem.Columns.Add("inv_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("wh_id", typeof(string));
                dtItem.Columns.Add("avl_qty", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));
                dtItem.Columns.Add("it_remarks", typeof(string));
                dtItem.Columns.Add("DiscInPer", typeof(string));
                dtItem.Columns.Add("DiscVal", typeof(string));
                dtItem.Columns.Add("sr_no", typeof(int));
                dtItem.Columns.Add("pack_size", typeof(string));

                if (ItemDetails != null)
                {
                    JArray jObject = JArray.Parse(ItemDetails);
                    {
                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();
                            dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                            dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
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
                            dtrowLines["pack_size"] = jObject[i]["pack_size"].ToString();
                            dtItem.Rows.Add(dtrowLines);
                        }
                    }
                }  
                return dtItem;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblSubItem(string SubitemDetails)
        {
            try
            {
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("avl_qty", typeof(string));
                dtSubItem.Columns.Add("inv_qty", typeof(string));

                if (!string.IsNullOrEmpty(SubitemDetails))
                {
                    JArray jObject2 = JArray.Parse(SubitemDetails);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["avl_qty"] = jObject2[i]["avl_stk"].ToString();
                        dtrowItemdetails["inv_qty"] = jObject2[i]["qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                }
                return dtSubItem;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblbatchDetail(string BatchItemDeatilData)
        {
            try
            {
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
                if (BatchItemDeatilData != null)
                {
                    JArray jObjectBatch = JArray.Parse(BatchItemDeatilData);
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
                }
                return Batch_detail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblserialDetail(string SerialItemDeatilData)
        {
            try
            {
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

                if (SerialItemDeatilData != null)
                {
                    JArray jObjectSerial = JArray.Parse(SerialItemDeatilData);
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
                return Serial_detail;
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
                }
                return Tax_detail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblOcDetail(string OCDetail)
        {
            try
            {
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
                if (OCDetail != null)
                {
                    JArray jObjectOC = JArray.Parse(OCDetail);
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
                }
                return OC_detail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblccDetail(string CC_DetailList)
        {
            try
            {
                DataTable CC_Details = new DataTable();
                CC_Details.Columns.Add("vou_sr_no", typeof(string));/*Add by Hina on 22-07-2024 to add for modify in GL 3rd party OC */
                CC_Details.Columns.Add("gl_sr_no", typeof(string));
                CC_Details.Columns.Add("acc_id", typeof(string));
                CC_Details.Columns.Add("cc_id", typeof(int));
                CC_Details.Columns.Add("cc_val_id", typeof(int));
                CC_Details.Columns.Add("cc_amt", typeof(string));
                if (CC_DetailList != null)
                {
                    JArray JAObj = JArray.Parse(CC_DetailList);
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
                }              
                return CC_Details;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblvouDetail(string vouDetail)
        {
            try
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
                }
                return vou_Details;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private DataTable ToDtblAttachmentDetail(InterBranchSale_Model _model)
        {
            try
            {
                string PageName = _model.Title.Replace(" ", "");
                DataTable dtAttachment = new DataTable();
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as IBSAttch;
                
                if (_model.attatchmentdetail != null)
                {
                    if (_DirectPurchaseInvoiceattch != null)
                    {
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (_DirectPurchaseInvoiceattch.AttachMentDetailItmStp != null)
                        {
                            //dtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            dtAttachment = _DirectPurchaseInvoiceattch.AttachMentDetailItmStp as DataTable;
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
                        if (_model.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _model.AttachMentDetailItmStp as DataTable;
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

                    JArray jObject1 = JArray.Parse(_model.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(_model.Inv_no))
                            {
                                dtrowAttachment1["id"] = _model.Inv_no;
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
                    if (_model.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(_model.Inv_no))
                            {
                                ItmCode = _model.Inv_no;
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
                    //DtblAttchDetail = dtAttachment;           
                }
                return dtAttachment;
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
        private void DeleteIBSDetail(InterBranchSale_Model _model)
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
                string Result = _IBS_ISERVICE.DeleteIBSDetails(CompID, BrchID, _model.Inv_no, _model.Inv_dt);
                _model.Message = Result.Split(',')[0];

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ApproveIBSDetails(InterBranchSale_Model _model, string PV_VoucherNarr, string BP_VoucherNarr, string DN_VoucherNarr)
        {
            try
            {
                string MenuDocId = string.Empty;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["UserId"] != null)
                    UserID = Session["UserId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID  = Session["BranchId"].ToString();
                if (DocumentMenuId != null)
                    MenuDocId = DocumentMenuId;
                var DocNo = _model.Inv_no;
                var DocDate = _model.Inv_dt;
                var A_Status = _model.A_Status;
                var A_Level = _model.A_Level;
                var A_Remarks = _model.A_Remarks;
                var DN_Nurr_Tcs = _model.DN_Narration_Tcs;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string SaleVouMsg = _model.SaleVouMsg;

                string Message = _IBS_ISERVICE.ApproveIBSDetail(DocNo, DocDate, MenuDocId, BrchID, CompID, UserID, mac_id
                    , A_Status, A_Level, A_Remarks, SaleVouMsg, PV_VoucherNarr, BP_VoucherNarr, DN_VoucherNarr, DN_Nurr_Tcs);
                try
                {
                    ViewBag.PrintFormat = PrintFormatDataTable(_model,null);
                    string fileName = "DSI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(DocNo, DocDate, fileName, null, _model.GstApplicable);
                    _Common_IServices.SendAlertEmail(CompID, BrchID, MenuDocId, DocNo, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                string[] FDetail = Message.Split(',');
                string INV_NO = string.Empty;
                string INV_DT = string.Empty;
                string ApMessage = string.Empty;
                if (FDetail[0].ToString() == "StockNotAvail")
                {
                    INV_NO = FDetail[1].ToString();
                    INV_DT = FDetail[2].ToString();
                    _model.Message = "stockNotAvailable";
                    _model.Command = "Update";

                    _model.BtnName = "BtnToDetailPage";
                }
                else
                {
                    INV_NO = FDetail[0].ToString();
                    INV_DT = FDetail[7].ToString();
                    ApMessage = FDetail[6].ToString().Trim();
                }
                if (ApMessage == "A")
                {
                    _model.Message = _model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    _model.BtnName = "BtnEdit";
                    _model.Command = "Approve";
                }
                _model.TransType = "Update";
                _model.Inv_no = INV_NO;
                _model.Inv_dt = INV_DT;
                _model.AppStatus = "D";            
                return RedirectToAction("InterBranchSaleDetail", _model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1, string PV_VoucherNarr, string BP_VoucherNarr, string DN_VoucherNarr)
        {
            //JArray jObjectBatch = JArray.Parse(list);
            InterBranchSale_Model _SSIModel = new InterBranchSale_Model();
            UrlData urlData = new UrlData();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _SSIModel.Inv_no = jObjectBatch[i]["DocNo"].ToString();
                    _SSIModel.Inv_dt = jObjectBatch[i]["DocDate"].ToString();
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

            ApproveIBSDetails(_SSIModel, PV_VoucherNarr, BP_VoucherNarr, DN_VoucherNarr);/*Add by Hina on 22-07-2024 to add for third party OC, tds on third party OC*/
            TempData["ModelData"] = _SSIModel;
            TempData["WF_status"] = WF_status1;
            SetUrlData(urlData, _SSIModel.Command, _SSIModel.TransType, _SSIModel.BtnName, _SSIModel.Message, _SSIModel.Inv_no, _SSIModel.Inv_dt, _SSIModel.ListFilterData1);
            return RedirectToAction("InterBranchSaleDetail");
        }
        /*--------------------------For PDF Print Start code by Hina on 24-07-2024--------------------------*/
        public FileResult GenratePdfFile(InterBranchSale_Model _model)
        {
            DataTable dt = new DataTable();
            dt = PrintFormatDataTable(_model,null);
            ViewBag.PrintOption = dt;
            if (_model.GstApplicable == "Y")
                return File(GetPdfDataOfGstInv(dt, _model.DocumentMenuId, _model.Inv_no, _model.Inv_dt, _model.NumberofCopy), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");
            else
                return File(GetPdfData(_model.DocumentMenuId, _model.Inv_no, _model.Inv_dt, _model.NumberofCopy), "application/pdf", ViewBag.Title.Replace(" ", "") + _model.PrintFormat + ".pdf");
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
                DataSet Details = _IBS_ISERVICE.GetIBSDeatilsForPrint(CompID, BrchID, invNo, invDt);
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
                        ViewBag.Title = "Inter Branch Sale";
                        htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/InterBranchSale/InterBranchSalePrint.cshtml"));
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
                DataSet Details = _IBS_ISERVICE.GetIBSGstDtlForPrint(CompID, Br_ID, invNo, invDt);
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
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 60f);
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
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/InterBranchSale/InterBranchSaleWithGSTPrintF2.cshtml"));
                        }
                        else
                        {
                            htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SalesAndDistributionViews/InterBranchSale/InterBranchSaleWithGSTPrintF1.cshtml"));
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
            string draftImage = Server.MapPath("~/Content/Images/draft.png");

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
                            if (docstatus == "D" || docstatus == "F")
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
            string QR = GenerateQRCode(Details.Tables[0].Rows[0]["inv_qr_code"].ToString());
            string State_Name = Details.Tables[7].Rows[0]["state_name"].ToString();
            String StateName = (State_Name).ToUpper();
            ViewBag.QrCode = QR;
            string draftImage = Server.MapPath("~/Content/Images/draft.png");
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
                            if (docstatus == "D" || docstatus == "F")
                            {
                                content.AddImage(draftimg);
                            }
                            if (PageCount1 > count)
                                count = count + 1;
                            else
                                count = 1;

                            if (!string.IsNullOrEmpty(Details.Tables[0].Rows[0]["inv_qr_code"].ToString()))
                            {
                                if (count == 1)
                                    content.AddImage(qrCode);
                            }
                            Phrase p = new Phrase(String.Format("Page {0} of {1}", count, PageCount1), font);
                            if (count == PageCount1)
                            {
                                Phrase ftr1 = new Phrase(String.Format("SUBJECT TO " + StateName + " JURISDICTION ", i, PageCount), font2);
                                ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, ftr1, 298, 40, 0);
                            }
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_CENTER, p, 560, 15, 0);
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
        private DataTable PrintFormatDataTable(InterBranchSale_Model _Model, string PrintFormat)
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
            dt.Columns.Add("ShowPackSize", typeof(string));

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
                dtr["ShowPackSize"] = _Model.ShowPackSize;
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
                    dtr["ShowPackSize"] = jObject[0]["ShowPackSize"];
                    dt.Rows.Add(dtr);
                }
            }
            return dt;
        }
        public string SavePdfDocToSendOnEmailAlert(string poNo, string poDate, string fileName, string PrintFormat, string GstApplicable)
        {
            DataTable dt = new DataTable();
            var commonCont = new CommonController(_Common_IServices);
            if (ViewBag.PrintFormat == null)
            {
                if (PrintFormat != null)
                {
                    dt = PrintFormatDataTable(null, PrintFormat);
                    //dt = commonCont.PrintOptionsDt(PrintFormat); //Added by Suraj on 08-10-2024
                }
            }
            else
            {
                dt = ViewBag.PrintFormat;
            }

            ViewBag.PrintOption = dt;
            if (GstApplicable == "Y")
            {
                var data = GetPdfDataOfGstInv(dt, "105103149", poNo, poDate, 1);
                return commonCont.SaveAlertDocument(data, fileName);
            }
            else
            {
                var data = GetPdfData("105103149", poNo, poDate, 1);
                return commonCont.SaveAlertDocument(data, fileName);
            }
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
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
        public ActionResult GetAutoCompleteSearchCustList(InterBranchSale_Model _model)
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
                if (string.IsNullOrEmpty(_model.CustName))
                {
                    CustomerName = "0";
                }
                else
                {
                    CustomerName = _model.CustName;
                }
                CustType = "D";

                CustList = _IBS_ISERVICE.GetCustomerList(Comp_ID, CustomerName, Br_ID, CustType, "105103149");
                List<CustomerName> _CustmrList = new List<CustomerName>();
                foreach (var data in CustList)
                {
                    CustomerName _SuppDetail = new CustomerName();
                    _SuppDetail.Cust_id = data.Key;
                    _SuppDetail.Cust_name = data.Value;
                    _CustmrList.Add(_SuppDetail);
                }
                _model.CustomerNameList = _CustmrList;
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
        public JsonResult GetCustAddrDetail(string Cust_id, string DocumentMenuId)
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
                DataSet result = _IBS_ISERVICE.GetCustAddrDetailDL(Cust_id, Comp_ID, br_id, DocumentMenuId);
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
        public DataTable GetWarehouseList()
        {
            DataTable dt = new DataTable();
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
                DataSet result = _IBS_ISERVICE.GetWarehouseList(CompID, BrchID);
                dt = result.Tables[0];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
            return dt;
        }
        public DataSet GetIBSEdit(string Inv_No, string Inv_Date)
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
                DataSet result = _IBS_ISERVICE.GetIBSDetail(Comp_ID, Br_ID, Voutype, Inv_No, Inv_Date, UserID, DocumentMenuId);

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
        public ActionResult GetSalesPersonList(InterBranchSale_Model _model)
        {
            string SalesPersonName = string.Empty;
            Dictionary<string, string> SPersonList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(_model.SalePerson))
                {
                    SalesPersonName = "0";
                }
                else
                {
                    SalesPersonName = _model.SalePerson;
                }
                SPersonList = _IBS_ISERVICE.GetSalesPersonList(CompID, SalesPersonName, BrchID, "1001");
                List<SalesPersonName> _SlPrsnList = new List<SalesPersonName>();
                foreach (var data in SPersonList)
                {
                    SalesPersonName _SlPrsnDetail = new SalesPersonName();
                    _SlPrsnDetail.slprsn_id = data.Key;
                    _SlPrsnDetail.slprsn_name = data.Value;
                    _SlPrsnList.Add(_SlPrsnDetail);
                }

                _model.SalesPersonNameList = _SlPrsnList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(SPersonList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
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
                    dt = _IBS_ISERVICE.Scrap_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "Scrap");
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = "Scrap",

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
                    ds = _IBS_ISERVICE.getItemStockBatchWise(ItemId, UomId, WarehouseId, CompID, BrchID);
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
                        if (ItmMatched == "N" && item.GetValue("wh_id").ToString() == WarehouseId && item.GetValue("ItemId").ToString() == ItemId)
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

        public ActionResult getItemStockBatchWiseAfterStockUpadte(string Doc_No, string Doc_dt, string ItemID, string doc_status
    , string DMenuId, string Command, string TransType, string UomId = null, string WarehouseId = null)
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
                ds = _IBS_ISERVICE.getItemStockBatchWiseAfterStockUpdate(CompID, br_id, Doc_No, Doc_dt, ItemID, UomId);
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
                ds = _IBS_ISERVICE.getItemstockSerialWise(CompID, BrchID, ItemId, WhID);

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
                ds = _IBS_ISERVICE.getItemstockSerialWiseAfterStockUpdate(CompID, BrchID, Docno, Docdt, ItemID);
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
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                IBSAttch _ScrapSIModelattch = new IBSAttch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                string branchID = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _ScrapSIModelattch.Guid = DocNo;
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    branchID = Session["BranchId"].ToString();
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _ScrapSIModelattch.AttachMentDetailItmStp = dt;
                }
                else
                {
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
        public ActionResult SearchIBSList(string CustId, string Fromdate, string Todate, string Status)
        {
            //ScrapSIListModel _SSIListModel = new ScrapSIListModel();
            IBSListModel _IBSListModel = new IBSListModel();
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
                //_SSIList = new List<ScrapSaleInvoiceList>();
                DataSet DSet = _IBS_ISERVICE.GetAllData(CompID,"0", BrchID, UserID, CustId, Fromdate, Todate, Status, DocumentMenuId, _IBSListModel.WF_status);
                _IBSListModel.SSISearch = "SSI_Search";
                ViewBag.ListSearch = "ListSearch";
                ViewBag.ListDetail = DSet.Tables[1];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialInterBranchSaleList.cshtml", _IBSListModel);
        }

        [HttpPost]
        public string CheckDependencyIBS(string InvNo, string Inv_dt)
        {
            string Result = "";
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                BrchID = Session["BranchId"].ToString();
               
                DataSet Deatils = _IBS_ISERVICE.checkDependencyIBP(CompID, BrchID, InvNo, Inv_dt);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    Result = "PurchaseInvoicehasbeengeneratedinrespectivebranchInvoicecannotbemodified";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;//Json("ErrorPage");
            }
            return Result;
        }
        private object IsBlank(string input, object output)//Added by Suraj Maurya on 27-11-2025
        {
            return input == "" ? output : input;
        }
    }
}