using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.DirectPurchaseInvoice;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.Resources;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.DirectPurchaseInvoice;
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
using System.Web;
using System.Web.Mvc;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.DirectPurchaseInvoice
{
    public class DirectPurchaseInvoiceController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105101154", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        DirectPurchaseInvoice_ISERVICE _DirectPurchaseInvoice_ISERVICE;
        Common_IServices _Common_IServices;
        public DirectPurchaseInvoiceController(Common_IServices _Common_IServices, DirectPurchaseInvoice_ISERVICE _DirectPurchaseInvoice_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._DirectPurchaseInvoice_ISERVICE = _DirectPurchaseInvoice_ISERVICE;
        }
        // GET: ApplicationLayer/DirectPurchaseInvoice
        //public ActionResult DirectPurchaseInvoice(DPIListModel _DPIListModel)
        public ActionResult DirectPurchaseInvoice(string wfStatus)
        {
            try
            {
                CommonPageDetails();
                DPIListModel _DPIListModel = new DPIListModel();
                if (wfStatus != null)
                {
                    _DPIListModel.wfstatus= wfStatus;
                    _DPIListModel.ListFilterData = "0,0,0,0"+"," + wfStatus;
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string todate = range.ToDate;
                if (_DPIListModel.DPI_FromDate == null)
                {
                    _DPIListModel.FromDate = startDate;
                    _DPIListModel.DPI_FromDate = startDate;
                    _DPIListModel.DPI_ToDate = todate;
                }
                else
                {
                    _DPIListModel.FromDate = _DPIListModel.DPI_FromDate;
                }
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _DPIListModel.StatusList = statusLists;
                if (TempData["UrlData"] != null)
                {
                    if (TempData["UrlData"].ToString() != "")
                    {
                        UrlData urlData = TempData["UrlData"] as UrlData;
                        if (urlData.ListFilterData1 != null)
                        {
                            var arr = urlData.ListFilterData1.Split(',');
                            _DPIListModel.SuppID = arr[0];
                            _DPIListModel.FromDate = arr[1];
                            _DPIListModel.ToDate = arr[2];
                            _DPIListModel.Status = arr[3];
                            if (wfStatus == null)
                            {
                                wfStatus = arr[3];
                            }
                            _DPIListModel.ListFilterData = _DPIListModel.SuppID + "," + _DPIListModel.FromDate + "," + _DPIListModel.ToDate + "," + _DPIListModel.Status + "," + wfStatus;
                        }

                    }
                }
                GetAllData(_DPIListModel);
                _DPIListModel.Title = title;
                return View("~/Areas/ApplicationLayer/Views/Procurement/DirectPurchaseInvoice/DirectPurchaseInvoiceList.cshtml", _DPIListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }           
        }
        private void GetAllData(DPIListModel _DPIListModel)
        {
            try
            {
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string SupplierName = string.Empty;
                string User_ID = string.Empty;
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
                if (string.IsNullOrEmpty(_DPIListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _DPIListModel.SuppName;
                }
                DataSet CustList = _DirectPurchaseInvoice_ISERVICE.GetAllData(Comp_ID, SupplierName, Br_ID, User_ID
                    , _DPIListModel.SuppID, _DPIListModel.DPI_FromDate, _DPIListModel.DPI_ToDate, _DPIListModel.Status, _DPIListModel.wfdocid, _DPIListModel.wfstatus);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (DataRow data in CustList.Tables[0].Rows)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data["supp_id"].ToString();
                    _SuppDetail.supp_name = data["supp_name"].ToString();
                    _SuppList.Add(_SuppDetail);
                }
                _SuppList.Insert(0, new SupplierName() { supp_id = "0", supp_name = "All" });
                _DPIListModel.SupplierNameList = _SuppList;
                ViewBag.ListDetail = CustList.Tables[1];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }            
        }
        public ActionResult AddDirectPurchaseInvoiceDetail(string DocNo, string DocDate, string ListFilterData)
        {
            try
            {
                UrlData urlData = new UrlData();
                /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if(DocNo==null)
                {
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {

                        TempData["Message"] = "Financial Year not Exist";
                        SetUrlData(urlData, "", "", "", "Financial Year not Exist", DocNo, DocDate, ListFilterData, "");
                        return RedirectToAction("DirectPurchaseInvoice", "DirectPurchaseInvoice", urlData);
                    }
                }
                
                /*End to chk Financial year exist or not*/
                string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = DocNo == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, DocDate, ListFilterData, "");
                return RedirectToAction("DirectPurchaseInvoiceDetail", "DirectPurchaseInvoice", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }          
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string Inv_no=null, string Inv_dt=null, string ListFilterData1 = null, string StockItemWiseMessage=null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Inv_no = Inv_no;
                urlData.Inv_dt = Inv_dt;
                urlData.ListFilterData1 = ListFilterData1;
                urlData.StockItemWiseMessage = StockItemWiseMessage;
                TempData["UrlData"] = urlData;
                TempData["Message"] = Message;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }            
        }
        public ActionResult DirectPurchaseInvoiceDetail(UrlData urlData)
        {
            try
            {
                CommonPageDetails();
                DirectPurchaseInvoice_Model model = new DirectPurchaseInvoice_Model();
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
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.Inv_no = urlData.Inv_no;
                model.Inv_dt = urlData.Inv_dt;
                model.ListFilterData1 = urlData.ListFilterData1;
                model.StockItemWiseMessage = urlData.StockItemWiseMessage;
                model.UserID = UserID;

                List<SupplierName> suppLists = new List<SupplierName>();
                suppLists.Add(new SupplierName { supp_id = model.DPI_SuppID, supp_name = model.DPI_SuppID });
                model.SupplierNameList = suppLists;

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

                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;

                if (model.Inv_no != null && model.Inv_dt != null)
                {
                    DataSet ds = GetDPIDetailEdit(model.Inv_no, model.Inv_dt, DocumentMenuId);
                    ViewBag.AttechmentDetails = ds.Tables[8];
                    ViewBag.GLAccount = ds.Tables[7];
                    ViewBag.GLTOtal = ds.Tables[9];
                    ViewBag.ItemTaxDetails = ds.Tables[2];
                    ViewBag.OtherChargeDetails = ds.Tables[3];
                    ViewBag.TotalDetails = ds.Tables[0];
                    ViewBag.ItemTaxDetailsList = ds.Tables[10];
                    ViewBag.OCTaxDetails = ds.Tables[11];
                    ViewBag.SubItemDetails = ds.Tables[13];
                    ViewBag.ItemTDSDetails = ds.Tables[14];
                    ViewBag.ItemOC_TDSDetails = ds.Tables[15];
                    ViewBag.ItemStockBatchWise = ds.Tables[16];
                    ViewBag.ItemStockSerialWise = ds.Tables[17];
                    model.Inv_no = ds.Tables[0].Rows[0]["inv_no"].ToString();
                    model.Inv_dt = ds.Tables[0].Rows[0]["InvDt"].ToString();
                    model.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                    model.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                    model.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                    model.conv_rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(QtyDigit);
                    model.DPI_SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                    model.supp_acc_id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                    model.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                    model.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                    model.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                    model.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                    model.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                    model.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                    model.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                    model.Status_name = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                    model.PriceBasis = ds.Tables[0].Rows[0]["price_basis"].ToString();
                    model.FreightType = ds.Tables[0].Rows[0]["freight_type"].ToString();
                    model.ModeOfTransport = ds.Tables[0].Rows[0]["mode_trans"].ToString();
                    model.Destination = ds.Tables[0].Rows[0]["dest"].ToString();
                    model.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                    model.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                    model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();
                    model.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                    model.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                    model.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                    model.GrossValueInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val_bs"]).ToString(ValDigit);
                    model.TaxAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(ValDigit);
                    model.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt"]).ToString(ValDigit);
                    model.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val"]).ToString(ValDigit);
                    model.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val"]).ToString(ValDigit);
                    model.NetLandedValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_landed_val"]).ToString(ValDigit);

                    model.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                    model.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                    model.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                    model.DocSuppOtherCharges = ds.Tables[0].Rows[0]["doc_supp_oc_amt"].ToString();
                    model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                    model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();

                    var state_code = ds.Tables[0].Rows[0]["state_code"];
                    var br_state_code = ds.Tables[0].Rows[0]["br_state_code"];
                    if (state_code.ToString() == br_state_code.ToString())
                    {
                        model.Hd_GstType = "Both";
                    }
                    else
                    {
                        model.Hd_GstType = "IGST";
                    }
                    string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                    ViewBag.Approve_id = approval_id;
                    string create_id = ds.Tables[0].Rows[0]["creator_Id"].ToString();
                    string doc_status = ds.Tables[0].Rows[0]["inv_status1"].ToString().Trim();
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
                    string RCMApplicable = ds.Tables[0].Rows[0]["rcm_app"].ToString().Trim();
                    if (RCMApplicable == "Y")
                    {
                        model.RCMApplicable = true;
                    }
                    else
                    {
                        model.RCMApplicable = false;
                    }
                    model.doc_status = doc_status;
                    if (ds.Tables[7].Rows.Count > 0)
                    {
                        if (doc_status == "A" || doc_status == "C")
                        {
                            model.GLVoucherType = ds.Tables[7].Rows[0]["vou_type"].ToString();
                        }
                        model.GLVoucherNo = ds.Tables[7].Rows[0]["vou_no"].ToString();
                        model.GLVoucherDt = ds.Tables[7].Rows[0]["vou_dt"].ToString();
                        ViewBag.GLVoucherNo = model.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
                    }
                    model.DocumentStatus = doc_status;
                    if (doc_status == "C")
                    {
                        model.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
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
                GetAutoCompleteSearchSuppList(model);
                model.Title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/Procurement/DirectPurchaseInvoice/DirectPurchaseInvoiceDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public DataSet GetDPIDetailEdit(string Inv_no, string Inv_dt, string DocumentMenuId)
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
                DataSet result = _DirectPurchaseInvoice_ISERVICE.GetDPIDetailDAL(CompID, BrchID, Inv_no, Inv_dt, UserID, DocumentMenuId);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActionDPIDeatils(DirectPurchaseInvoice_Model _model, string Command)
        {
            try
            {/*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                UrlData urlData = new UrlData();
                if (_model.DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                switch (Command)
                {
                    case "AddNew":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_model.Inv_no))
                            {
                                SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", "Financial Year not Exist", _model.Inv_no, _model.Inv_dt, _model.ListFilterData1, "");
                                return RedirectToAction("DirectPurchaseInvoiceDetail", _model);
                            }
                            //return RedirectToAction("AddDirectPurchaseInvoiceDetail", new { DocNo = _model.SPO_No, DocDate = _model.SPO_Date, ListFilterData = _model.ListFilterData1, WF_status = _Model.WFStatus });
                            else
                            {
                                //_model.Command = "Refresh";
                                //_model.TransType = "Refresh";
                                //_model.BtnName = "BtnRefresh";
                                //_model.DocumentStatus = null;
                                //TempData["ModelData"] = _model;
                                SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, null, _model.ListFilterData1, "");
                                return RedirectToAction("DirectPurchaseInvoiceDetail", _model);

                            }
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, null, _model.ListFilterData1, "");
                        return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.SPO_No, DocDate = _Model.SPO_Date, ListFilterData = _Model.ListFilterData1, WF_status = _Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string invdt = _model.Inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1,"");
                            return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                        }
                        /*End to chk Financial year exist or not*/
                        if (_model.doc_status == "A")
                        {
                            string checkforCancle = CheckDPIForCancellationinReturn(_model.Inv_no, _model.Inv_dt);
                            if (checkforCancle != "")
                            {
                                SetUrlData(urlData, "Refresh", "Update", "BtnToDetailPage", checkforCancle, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1,"");
                            }
                            else
                            {
                                SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1,"");
                            }
                        }
                        else
                        {
                            SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1,"");
                        }
                        return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                    case "Save":
                        SaveDPIDetails(_model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1,_model.StockItemWiseMessage);
                        return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                    case "Approve":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        string Invdt1 = _model.Inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, Invdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", _model.Message, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1, "");
                            return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                        }
                        ApproveDPIDetails(_model, _model.Inv_no, _model.Inv_dt, "", "", "", _model.Nurration, "", "", "", _model.BP_Nurration, _model.DN_Nurration);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Inv_no, _model.Inv_dt, _model.ListFilterData1, "");
                        return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _model.ListFilterData1, "");
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _model.ListFilterData1, "");
                        return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                    case "Delete":
                        DeleteDPIDetail(_model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, null, _model.ListFilterData1, "");
                        return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                    case "BacktoList":
                        SetUrlData(urlData, "", "", "", null, null, null, _model.ListFilterData1, "");
                        return RedirectToAction("DirectPurchaseInvoice");
                    case "Print":
                        return GenratePdfFile(_model);
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult SaveDPIDetails(DirectPurchaseInvoice_Model _model)
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
                DataTable BatchItemTableData = new DataTable();
                DataTable SerialItemTableData = new DataTable();
                DataTable DtblTaxDetail = new DataTable();
                DataTable DtblOCTaxDetail = new DataTable();
                DataTable DtblIOCDetail = new DataTable();
                DataTable DtblTdsDetail = new DataTable();
                DataTable DtblOcTdsDetail = new DataTable();
                DataTable DtblVouDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable CostCenterDetails = new DataTable();

                DtblHDetail = ToDtblHeaderDetail(_model);
                DtblItemDetail = ToDtblItemDetail(_model.ItemDetails);
                DtblSubItem = ToDtblSubItem(_model.SubItemDetails);
                BatchItemTableData = ToDtblbatchDetail(_model.BatchItemDeatilData);
                SerialItemTableData = ToDtblserialDetail(_model.SerialItemDeatilData);
                DtblTaxDetail = ToDtblTaxDetail(_model.TaxDetail, "Y");
                DtblOCTaxDetail = ToDtblTaxDetail(_model.OC_TaxDetail, "N");
                DtblIOCDetail = ToDtblOcDetail(_model.OCDetail);
                DtblTdsDetail = ToDtblTdsDetail(_model.tds_details, "");
                DtblOcTdsDetail = ToDtblTdsDetail(_model.oc_tds_details, "OC");
                DtblVouDetail = ToDtblvouDetail(_model.vouDetail);
                DtblAttchDetail = ToDtblAttachmentDetail(_model);
                CostCenterDetails = ToDtblccDetail(_model.CC_DetailList);

                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as DirectPurchaseInvoiceattch;
                TempData["ModelDataattch"] = null;
                string Nurr = "";
                if (_model.CancelFlag)
                {
                    Nurr = _model.Nurration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }
                string tds_amt = _model.TDS_Amount == null ? "0" : _model.TDS_Amount;
                SaveMessage = _DirectPurchaseInvoice_ISERVICE.InsertDPI_Details(DtblHDetail, DtblItemDetail, DtblSubItem, BatchItemTableData, SerialItemTableData, DtblTaxDetail
                    , DtblOCTaxDetail, DtblIOCDetail, DtblTdsDetail, DtblOcTdsDetail, DtblVouDetail, DtblAttchDetail, CostCenterDetails, Nurr, tds_amt);
                if (SaveMessage == "DocModify")
                {
                    _model.Message = "DocModify";
                    _model.BtnName = "Refresh";
                    _model.Command = "Refresh";
                    return RedirectToAction("DirectPurchaseInvoiceDetail");
                }
                else if(SaveMessage== "Cancelled")
                {
                    try
                    {
                        //string fileName = "DPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = _model.Inv_no.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_model.Inv_no, _model.Inv_dt, fileName, DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, "105101154", _model.Inv_no, "Cancel", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _model.Message = _model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    //_model.Message = "Cancelled";
                    _model.Command = "Update";
                    _model.AppStatus = "D";
                    _model.BtnName = "Refresh";
                    _model.TransType = "Update";
                    return RedirectToAction("DirectPurchaseInvoiceDetail");
                }
                else
                {
                    string[] FDetail = SaveMessage.Split(',');
                    string Inv_no = FDetail[0].ToString();
                    if(Inv_no== "StNtAvl")
                    {
                        _model.StockItemWiseMessage= string.Join(",", FDetail.Skip(1));
                        _model.Message = "StNtAvl";
                        _model.Command = "Update";
                        _model.AppStatus = "D";
                        _model.BtnName = "Refresh";
                        _model.TransType = "Update";
                    }
                    else
                    {
                        string Message = FDetail[5].ToString();
                        string Inv_DATE = FDetail[6].ToString();
                        string Cansal = FDetail[1].ToString();
                        if (Message == "DataNotFound")
                        {
                            var msg = "Data Not found" + " " + Inv_DATE + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _model.Message = Message;
                            return RedirectToAction("DirectPurchaseInvoiceDetail");
                        }
                        if (Message == "Save")
                        {
                            string Guid = "";
                            if (_DirectPurchaseInvoiceattch != null)
                            {
                                if (_DirectPurchaseInvoiceattch.Guid != null)
                                {
                                    Guid = _DirectPurchaseInvoiceattch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, Inv_no, _model.TransType, DtblAttchDetail);
                        }
                        if (Cansal == "C" && Message == "Update")
                        {
                            _model.Message = "Cancelled";
                            _model.Command = "Update";
                            _model.Inv_no = Inv_no;
                            _model.Inv_dt = Inv_DATE;
                            _model.AppStatus = "D";
                            _model.BtnName = "Refresh";
                            _model.TransType = "Update";
                        }
                        else if (Message == "StNtAvl")
                        {
                            _model.Message = "StNtAvl";
                            _model.Command = "Update";
                            _model.Inv_no = Inv_no;
                            _model.Inv_dt = Inv_DATE;
                            _model.AppStatus = "D";
                            _model.BtnName = "Refresh";
                            _model.TransType = "Update";
                        }
                        else
                        {
                            if (Message == "Update" || Message == "Save")
                            {
                                _model.Message = "Save";
                                _model.Command = "Update";
                                _model.Inv_no = Inv_no;
                                _model.Inv_dt = Inv_DATE;
                                _model.AppStatus = "D";
                                _model.BtnName = "BtnSave";
                                _model.TransType = "Update";
                            }
                        }
                    }             
                    return RedirectToAction("DirectPurchaseInvoiceDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void DeleteDPIDetail(DirectPurchaseInvoice_Model _model)
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
                string Result = _DirectPurchaseInvoice_ISERVICE.DeleteDPIDetails(CompID, BrchID, _model.Inv_no, _model.Inv_dt);
                _model.Message = Result.Split(',')[0];

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ApproveDPIDetails(DirectPurchaseInvoice_Model _model,string Inv_No, string Inv_Date
            , string A_Status, string A_Level, string A_Remarks, string VoucherNarr, string FilterData
            , string docid, string WF_Status1, string Bp_Nurr, string Dn_Nurration)
        {
            try
            {
                UrlData urlData = new UrlData();
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
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Dn_Nurr = _model.DN_Nurration == null ? Dn_Nurration : _model.DN_Nurration;
                string Result = _DirectPurchaseInvoice_ISERVICE.ApproveDPIDetail(Inv_No, Inv_Date, docid, BrchID
                    , CompID, UserID, mac_id, A_Status, A_Level, A_Remarks, VoucherNarr, Bp_Nurr, Dn_Nurr);
                //ViewBag.PrintFormat = PrintFormatDataTable(_Model);
                try
                {
                    //string fileName = "DPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = Inv_No.Replace("/", "") + "_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(Inv_No, Inv_Date, fileName, DocumentMenuId,"AP");
                    _Common_IServices.SendAlertEmail(CompID, BrchID, "105101154", Inv_No, "AP", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                if(Result.Split(',')[1] == "A")
                {
                    _model.Message = _model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }              
                //_model.Message = Result.Split(',')[1] == "A" ? "Approved" : "Error";
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[0], Result.Split(',')[7], FilterData,"");
                return RedirectToAction("DirectPurchaseInvoiceDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
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
                        var data = GetPdfData("105101154", Doc_no, Doc_dt);
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
        private DataTable ToDtblHeaderDetail(DirectPurchaseInvoice_Model _model)
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
                dtheader.Columns.Add("gr_val_bs", typeof(string));
                dtheader.Columns.Add("rcm_app", typeof(string));
                dtheader.Columns.Add("net_landed_val", typeof(string));
                dtheader.Columns.Add("cancel_remarks", typeof(string));
                DataRow dtrowHeader = dtheader.NewRow();
                if (_model.Inv_no != null)
                {
                    _model.TransType = "Update";
                    dtrowHeader["TransType"] = _model.TransType;
                }
                else
                {
                    _model.TransType = "Save";
                    dtrowHeader["TransType"] = _model.TransType;
                }
                dtrowHeader["MenuID"] = DocumentMenuId;
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
                dtrowHeader["inv_no"] = _model.Inv_no;
                dtrowHeader["inv_dt"] = _model.Inv_dt;
                dtrowHeader["supp_id"] = _model.DPI_SuppID;
                dtrowHeader["bill_no"] = _model.bill_no;
                dtrowHeader["bill_dt"] = _model.bill_date;
                dtrowHeader["curr_id"] = _model.curr_id;
                dtrowHeader["conv_rate"] = _model.conv_rate;
                dtrowHeader["user_id"] = Session["UserId"].ToString();
                if (_model.AppStatus == null)
                {
                    _model.AppStatus = "D";
                    dtrowHeader["inv_status"] = _model.AppStatus;
                }
                else
                {
                    dtrowHeader["inv_status"] = _model.AppStatus;
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["gr_val"] = _model.GrossValue;
                dtrowHeader["tax_amt_nrecov"] = _model.TaxAmount;
                dtrowHeader["oc_amt"] = _model.OtherCharges;
                dtrowHeader["net_val"] = _model.NetAmountInBase;
                dtrowHeader["net_val_bs"] = _model.NetAmountInBase;
                dtrowHeader["Narration"] = _model.Narration;
                dtrowHeader["price_basis"] = _model.PriceBasis;
                dtrowHeader["freight_type"] = _model.FreightType;
                dtrowHeader["mode_trans"] = _model.ModeOfTransport;
                dtrowHeader["dest"] = _model.Destination;
                dtrowHeader["bill_add_id"] = _model.bill_add_id == null ? "0" : _model.bill_add_id;
                dtrowHeader["remarks"] = _model.remarks;
                dtrowHeader["ewb_no"] = _model.EWBNNumber;
                dtrowHeader["einv_no"] = _model.EInvoive;
                dtrowHeader["gr_val_bs"] = _model.GrossValue;
                string RCMApplicable = _model.RCMApplicable.ToString();
                if (RCMApplicable == "False")
                {
                    dtrowHeader["rcm_app"] = "N";
                }
                else
                {
                    dtrowHeader["rcm_app"] = "Y";
                }
                dtrowHeader["net_landed_val"] ="0";
                dtrowHeader["cancel_remarks"] = _model.CancelledRemarks;
                dtheader.Rows.Add(dtrowHeader);

                dtheaderdeatil = dtheader;
                return dtheaderdeatil;
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
                DataTable dtItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("item_type", typeof(string));
                dtItem.Columns.Add("inv_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_disc_perc", typeof(string));
                dtItem.Columns.Add("item_disc_amt", typeof(string));
                dtItem.Columns.Add("item_disc_val", typeof(string));
                dtItem.Columns.Add("item_gr_val", typeof(string));
                dtItem.Columns.Add("tax_amt", typeof(string));
                dtItem.Columns.Add("tax_amt_recov", typeof(string));
                dtItem.Columns.Add("tax_amt_nrecov", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val_spec", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("wh_id", typeof(int));
                dtItem.Columns.Add("lot_id", typeof(string));
                dtItem.Columns.Add("remarks", typeof(string));
                dtItem.Columns.Add("gl_vou_no", typeof(string));
                dtItem.Columns.Add("gl_vou_dt", typeof(string));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("claim_itc", typeof(string));
                dtItem.Columns.Add("item_gr_val_bs", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(string));
                dtItem.Columns.Add("net_landed_val", typeof(string));
                dtItem.Columns.Add("landed_cost_per_pc", typeof(string));
                dtItem.Columns.Add("sr_no", typeof(int));
                dtItem.Columns.Add("mrp", typeof(string));
                dtItem.Columns.Add("pack_size", typeof(string));

                if (ItemDetails != null)
                {
                    JArray jObject = JArray.Parse(ItemDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                        dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"].ToString());
                        dtrowLines["inv_qty"] = jObject[i]["inv_qty"].ToString();
                        dtrowLines["item_type"] = jObject[i]["ItemType"].ToString();
                        dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                        dtrowLines["item_disc_perc"] = jObject[i]["item_disc_perc"].ToString();
                        dtrowLines["item_disc_amt"] = jObject[i]["item_disc_amt"].ToString();
                        dtrowLines["item_disc_val"] = jObject[i]["item_disc_val"].ToString();
                        dtrowLines["item_gr_val"] = jObject[i]["item_gr_val"].ToString();
                        dtrowLines["tax_amt"] = jObject[i]["tax_amt"].ToString();
                        dtrowLines["tax_amt_recov"] = jObject[i]["tax_amt_recov"].ToString();
                        dtrowLines["tax_amt_nrecov"] = jObject[i]["tax_amt_nrecov"].ToString();
                        dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                        dtrowLines["item_net_val_spec"] = jObject[i]["item_net_val_spec"].ToString();
                        dtrowLines["item_net_val_bs"] = jObject[i]["item_net_val_bs"].ToString();
                        dtrowLines["wh_id"] = jObject[i]["wh_id"].ToString();
                        dtrowLines["lot_id"] = jObject[i]["LotNumber"].ToString();
                        dtrowLines["remarks"] = jObject[i]["remarks"].ToString();
                        dtrowLines["gl_vou_no"] = jObject[i]["gl_vou_no"].ToString();
                        dtrowLines["gl_vou_dt"] = jObject[i]["gl_vou_dt"].ToString();
                        dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                        dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                        dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                        dtrowLines["claim_itc"] = jObject[i]["ClaimITC"].ToString();
                        dtrowLines["item_gr_val_bs"] = jObject[i]["item_gr_val_bs"].ToString();
                        dtrowLines["item_acc_id"] = jObject[i]["item_acc_id"].ToString();
                        dtrowLines["net_landed_val"] = jObject[i]["LandedCostValue"].ToString();
                        dtrowLines["landed_cost_per_pc"] = jObject[i]["LandedCostPerPc"].ToString();
                        dtrowLines["sr_no"] = jObject[i]["srno"].ToString();
                        dtrowLines["mrp"] = jObject[i]["mrp"].ToString();
                        dtrowLines["pack_size"] = jObject[i]["PackSize"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                }               
                dtItemDetail = dtItem;
                return dtItemDetail;
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
                        dtrowTaxDetailsLines["tax_recov"] = recov == "Y" ? jObjectTax[i]["tax_recov"].ToString() : "";
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
        private DataTable ToDtblOcDetail(string OCDetail)
        {
            try
            {
                DataTable OCdetail = new DataTable();
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
                OCdetail = OC_detail;
                return OCdetail;
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
                CC_Details.Columns.Add("vou_sr_no", typeof(string));
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
                    ViewData["CCdetail"] = dtCCdetail(JAObj);
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
        private DataTable ToDtblAttachmentDetail(DirectPurchaseInvoice_Model _model)
        {
            try
            {
                string PageName = _model.Title.Replace(" ", "");
                DataTable dtAttachment = new DataTable();
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as DirectPurchaseInvoiceattch;
                //TempData["ModelDataattch"] = null;
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
                    ViewData["VouDetail"] = dtVoudetail(jObjectVOU);
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
        private DataTable ToDtblSubItem(string SubitemDetails)
        {
            try
            {
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("inv_qty", typeof(string));

                if (!string.IsNullOrEmpty(SubitemDetails))
                {
                    JArray jObject2 = JArray.Parse(SubitemDetails);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["inv_qty"] = jObject2[i]["Qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                    ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
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
                var commonContr = new CommonController();
                DataTable Batchitemtable = new DataTable();
                Batchitemtable.Columns.Add("item_id", typeof(string));
                Batchitemtable.Columns.Add("batch_no", typeof(string));
                Batchitemtable.Columns.Add("batch_qty", typeof(string));
                Batchitemtable.Columns.Add("exp_dt", typeof(string));
                Batchitemtable.Columns.Add("mfg_name", typeof(string));
                Batchitemtable.Columns.Add("mfg_mrp", typeof(string));
                Batchitemtable.Columns.Add("mfg_date", typeof(string));

                if (BatchItemDeatilData != null)
                {
                    JArray obj = JArray.Parse(BatchItemDeatilData);
                    for (int i = 0; i < obj.Count; i++)
                    {
                        DataRow Batchitemrow = Batchitemtable.NewRow();
                        Batchitemrow["item_id"] = obj[i]["itemid"].ToString();
                        Batchitemrow["batch_no"] = obj[i]["BatchNo"].ToString();
                        Batchitemrow["batch_qty"] = obj[i]["BatchQty"].ToString();

                        if (obj[i]["BatchExDate"].ToString() == "" || obj[i]["BatchExDate"].ToString() == null)
                        {
                            Batchitemrow["exp_dt"] = "01-Jan-1900";
                            //Batchitemrow["exp_dt"] = "";
                        }
                        else
                        {
                            Batchitemrow["exp_dt"] = obj[i]["BatchExDate"].ToString();
                        }
                        Batchitemrow["mfg_name"] = commonContr.IsBlank(obj[i]["MfgName"].ToString(),null);
                        Batchitemrow["mfg_mrp"] = commonContr.IsBlank(obj[i]["MfgMrp"].ToString(),null);
                        Batchitemrow["mfg_date"] = commonContr.IsBlank(obj[i]["MfgDate"].ToString(),null);
                        Batchitemtable.Rows.Add(Batchitemrow);
                    }
                }                
                return Batchitemtable;
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
                var commonContr = new CommonController();
                DataTable Serialitemtable = new DataTable();
                Serialitemtable.Columns.Add("item_id", typeof(string));
                Serialitemtable.Columns.Add("serial_no", typeof(string));
                Serialitemtable.Columns.Add("mfg_name", typeof(string));
                Serialitemtable.Columns.Add("mfg_mrp", typeof(string));
                Serialitemtable.Columns.Add("mfg_date", typeof(string));

                if (SerialItemDeatilData != null)
                {
                    JArray obj1 = JArray.Parse(SerialItemDeatilData);
                    for (int i = 0; i < obj1.Count; i++)
                    {
                        DataRow Serialitemrow = Serialitemtable.NewRow();
                        Serialitemrow["item_id"] = obj1[i]["itemid"].ToString();
                        Serialitemrow["serial_no"] = obj1[i]["SerialNo"].ToString();
                        Serialitemrow["mfg_name"] = commonContr.IsBlank(obj1[i]["MfgName"].ToString(),null);
                        Serialitemrow["mfg_mrp"] = commonContr.IsBlank(obj1[i]["MfgMrp"].ToString(),null);
                        Serialitemrow["mfg_date"] = commonContr.IsBlank(obj1[i]["MfgDate"].ToString(),null);
                        Serialitemtable.Rows.Add(Serialitemrow);
                    }
                }                
                return Serialitemtable;
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
            dtSubItem.Columns.Add("inv_qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["inv_qty"] = jObject2[i]["Qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
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
        public ActionResult GetAutoCompleteSearchSuppList(DirectPurchaseInvoice_Model model)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string SuppType = string.Empty;
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
                if (string.IsNullOrEmpty(model.DPI_SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = model.DPI_SuppName;
                }
                SuppType = "D";
                CustList = _DirectPurchaseInvoice_ISERVICE.GetSupplierList(CompID, SupplierName, BrchID, SuppType);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                model.SupplierNameList = _SuppList;
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
        public string ToFixDecimal(int number)
        {
            string str = "0.";
            for (int i = 0; i < number; i++)
            {
                str += "0";
            }
            return str;
        }
        /*---------------------------------Sub-Item Start-------------------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
     , string Flag, string Status, string Doc_no, string Doc_dt, string src_type, string UOMId)
        {
            try
            {
                Flag = "Quantity";
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
                //if (Flag == "QuantitySpec")
                if (Flag == "Quantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        //if (src_type == "D" && Flag == "QuantitySpec")
                        if (src_type == "D" && Flag == "Quantity")
                        {
                            dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, "0", Item_id, UOMId, "br").Tables[0];
                            dt.Columns.Add("Qty", typeof(string));
                        }                     
                        //else
                        //{
                        //    dt = _DPODetail_ISERVICE.GetQTandPRSubItemwithWhAvlstock(CompID, BrchID, Item_id, SRCDoc_no, SRCDoc_date, Flag, src_type).Tables[0];
                        //}
                        //dt.Columns.Add("Qty", typeof(string));
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
                        dt = _DirectPurchaseInvoice_ISERVICE.DPI_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt).Tables[0];                    
                    }
                    Flag = "Quantity";
                }              
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    ShowStock = "N",//(Status == "D" || Status == "F" || Status == "") ? "Y" : "N",
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
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
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
        /***---------------------------For PDF Start-----------------------------------------***/
        public FileResult GenratePdfFile(DirectPurchaseInvoice_Model _model)
        {
            return File(GetPdfData(_model.DocumentMenuId, _model.Inv_no, _model.Inv_dt), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
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
                DataSet Details = _DirectPurchaseInvoice_ISERVICE.GetDirectPurchaseInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt);
                ViewBag.PageName = "PI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                ViewBag.Title = "Direct Purchase Invoice";
                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/DirectPurchaseInvoice/DirectPurchaseInvoicePrint.cshtml"));

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
        public ActionResult DPIListSearch(string SuppId, string Fromdate, string Todate,string Status, string wfStatus)
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
                DataSet ds = new DataSet();
                //ds = _TDSPosting_ISERVICES.GetTdsPostngList(CompID, BrchID, InCase(Year, "0", null), InCase(month, "0", null), InCase(status, "0", null), DocumentMenuId, null, UserID);
                ds = _DirectPurchaseInvoice_ISERVICE.GetAllData(CompID,"", BrchID, UserID
                    , SuppId, Fromdate, Todate, Status, "105101154", wfStatus);
                ViewBag.ListDetail = ds.Tables[1];
                ViewBag.ListSearch = "ListSearch";
                ViewBag.ListFilterData1 = SuppId + "," + Fromdate + "," + Todate + ","+ Status+ "," + wfStatus;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialDirectPurchaseInvoiceList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                DirectPurchaseInvoiceattch _DirectPurchaseInvoiceattch = new DirectPurchaseInvoiceattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                Guid gid = new Guid();
                gid = Guid.NewGuid();
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _DirectPurchaseInvoiceattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                string PageName = title.Replace(" ", "");
                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _DirectPurchaseInvoiceattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _DirectPurchaseInvoiceattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _DirectPurchaseInvoiceattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public DataTable GetWarehouseList()
        {
            DataTable dt = new DataTable();
            try
            {
                //JsonResult DataRows = null;
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
                DataSet result = _DirectPurchaseInvoice_ISERVICE.GetWarehouseList(Comp_ID, Br_ID);
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
        public string CheckDPIForCancellationinReturn(string DocNo, string DocDate)
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
                DataSet Deatils = _DirectPurchaseInvoice_ISERVICE.CheckDPIDetail(Comp_ID, Br_ID, DocNo, DocDate);
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
        //Added by Nidhi on 02-09-2025
        public string SavePdfDocToSendOnEmailAlert_Ext(string docid, string Doc_no, string Doc_dt,string fileName)
        {
            DataTable dt = new DataTable();
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
                        string fileName = "DPI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
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
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            var WF_status1 = "";
            //Session["Message"] = "";
            DirectPurchaseInvoice_Model _Model = new DirectPurchaseInvoice_Model();
            var a = TrancType.Split(',');
            _Model.Inv_no = a[0].Trim();
            _Model.Inv_dt = a[1].Trim();
            _Model.TransType = a[2].Trim();
            if (a[3].Trim() != "" && a[3].Trim() != null)
            {
                WF_status1 = a[3].Trim();
                _Model.WF_Status1 = WF_status1;
            }
            var docId = a[4].Trim();
            _Model.Message = Mailerror;
            _Model.BtnName = "BtnToDetailPage";
            _Model.DocumentMenuId = docId;
            TempData["ModelData"] = _Model;
            TempData["WF_status1"] = WF_status1; ;
            TempData["ListFilterData"] = ListFilterData1;
            UrlData URLModel = new UrlData();
            URLModel.Inv_no = a[0].Trim();
            URLModel.Inv_dt = a[1].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.ListFilterData1 = ListFilterData1;
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnToDetailPage";
            TempData["UrlData"] = URLModel;
            TempData["Message"] = _Model.Message;
            return RedirectToAction("DirectPurchaseInvoiceDetail", URLModel);
        }
    }
}






















