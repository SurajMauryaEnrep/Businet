using System;
using System.Web.Mvc;
using System.Data;
using System.Collections.Generic;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.SupplementaryPurchaseInvoiceIService;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.SupplementaryPurchaseInvoice;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using EnRepMobileWeb.Resources;
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.SupplementaryPurchaseInvoice
{
    public class SupplementaryPurchaseInvoiceController : Controller
    {
        string CompID, language = String.Empty, DocumentMenuId = "105101147", title, User_ID, BrchID = string.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        SupplementaryPurchaseInvoice_ISERVICE _SupplementaryPurchaseInvoiceIService;
        SupplementaryPurchaseInvoiceList_ISERVICE _SupplementaryPurchaseInvoiceListIService;

        public SupplementaryPurchaseInvoiceController(Common_IServices _Common_IServices, SupplementaryPurchaseInvoice_ISERVICE _SupplementaryPurchaseInvoice_ISERVICE, SupplementaryPurchaseInvoiceList_ISERVICE _SupplementaryPurchaseInvoiceList_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._SupplementaryPurchaseInvoiceIService = _SupplementaryPurchaseInvoice_ISERVICE;
            this._SupplementaryPurchaseInvoiceListIService = _SupplementaryPurchaseInvoiceList_ISERVICE;
        }

        public ActionResult GetSuppPurchaseInvoiceDashbrd(string wfStatus)
        {
            SuppPI_ListModel _ListModel = new SuppPI_ListModel(); DocumentMenuId = "105101147";
            TempData["DocumentID"] = DocumentMenuId;
            TempData["WF_Status"] = wfStatus;
            TempData["docid"] = DocumentMenuId;
            TempData["ModelData"] = _ListModel;
            _ListModel.WFStatus = wfStatus;
            return RedirectToAction("SupplementaryPurchaseInvoice", _ListModel);

        }
        public ActionResult SupplementaryPurchaseInvoice(string wfStatus, SuppPI_ListModel _SuppPI_ListModel)
        {
            try
            {
                DocumentMenuId = "105101147";
                _SuppPI_ListModel.DocumentMenuId = DocumentMenuId;
                _SuppPI_ListModel.SuppPI_wfdocid = DocumentMenuId;
                
                string SupplierName = string.Empty;
                GetCompDeatil();
                CommonPageDetails();

                if (TempData["WF_Status"] != null && TempData["WF_Status"].ToString() != "")
                {
                    if (TempData["WF_Status"] != null)
                    {
                        _SuppPI_ListModel.WF_Status = TempData["WF_Status"].ToString();
                        _SuppPI_ListModel.SuppPI_wfstatus = _SuppPI_ListModel.WF_Status;
                        _SuppPI_ListModel.ListFilterData = "0,0,0,0" + "," + TempData["WF_Status"].ToString();
                    }
                    else
                    {
                        _SuppPI_ListModel.SuppPI_wfstatus = "";
                    }
                }
                else
                {
                    if (wfStatus != null)
                    {
                        _SuppPI_ListModel.SuppPI_wfstatus = wfStatus;
                    }
                    else
                    {
                        _SuppPI_ListModel.SuppPI_wfstatus = "";
                    }
                }

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string todate = range.ToDate;
                GetSuppList(_SuppPI_ListModel);
                ViewBag.DocumentMenuId = DocumentMenuId;
                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _SuppPI_ListModel.StatusList = statusLists;
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    //var PRData = TempData["FilterData"].ToString();
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    _SuppPI_ListModel.SuppPI_SuppID = a[0].Trim();
                    _SuppPI_ListModel.SuppPI_FromDate = a[1].Trim();
                    _SuppPI_ListModel.SuppPI_ToDate = a[2].Trim();
                    _SuppPI_ListModel.Status = a[3].Trim();
                    if (_SuppPI_ListModel.Status == "0")
                    {
                        _SuppPI_ListModel.Status = null;
                    }
                    _SuppPI_ListModel.FilterData = TempData["ListFilterData"].ToString();
                }
                if (_SuppPI_ListModel.SuppPI_FromDate == null)
                {
                    _SuppPI_ListModel.SuppPI_FromDate = startDate;
                    _SuppPI_ListModel.SuppPI_ToDate = todate;
                }
                _SuppPI_ListModel.FromDate = _SuppPI_ListModel.SuppPI_FromDate;
                _SuppPI_ListModel.ToDate = Convert.ToDateTime(_SuppPI_ListModel.SuppPI_ToDate);
                // _SuppPI_ListModel.PIList = GetGRNDetailList(_SuppPI_ListModel);
                DataSet dtTable = GetGRNDetailList(_SuppPI_ListModel);
                ViewBag.ListDetail = dtTable.Tables[0];
                if (dtTable.Tables[1].Rows.Count > 0)
                {
                    //   FromDate = dtTable.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                //_SuppPI_ListModel.Status = _SuppPI_ListModel.Status;
                _SuppPI_ListModel.WF_Status = wfStatus;
                _SuppPI_ListModel.Title = title;
                ViewBag.title = title;
                return View("~/Areas/ApplicationLayer/Views/Procurement/SupplementaryPurchaseInvoice/SupplementaryPurchaseInvoiceList.cshtml", _SuppPI_ListModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult SupplementaryPurchaseInvoiceDetail(SuppPI_ListModel _PI_ListModel, string Inv_no, string Inv_dt, string TransType, string BtnName, string Command, string menuDocumentID, string WF_Status1,string Mailerror)
        {
            try
            {
                if (Session["UserID"] != null)
                {
                    User_ID = Session["UserID"].ToString();
                }
                CommonPageDetails();
                SuppPI_ListModel SPI_Model = new SuppPI_ListModel();
                SPI_Model.Message = Mailerror;

                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);

                if (TempData["wf_status1"] != null && TempData["wf_status1"].ToString() != "")
                {
                    SPI_Model.WF_Status = TempData["wf_status1"].ToString();
                }
                else
                {
                    SPI_Model.WF_Status = WF_Status1;
                }
                if (TempData["FilterData"] != null && TempData["FilterData"].ToString() != "")
                {
                    SPI_Model.FilterData1 = TempData["FilterData"].ToString();
                }
                SPI_Model.Command = Command;
                SPI_Model.TransType = TransType;
                SPI_Model.BtnName = BtnName;
                SPI_Model.Inv_Nos = Inv_no;
                SPI_Model.SuppPI_inv_dt = Inv_dt;
                SPI_Model.DocumentMenuId = DocumentMenuId;
                SPI_Model.UserID = User_ID;
                //SPI_Model.ListFilterData = Filter;

                List<SupplierName> suppLists = new List<SupplierName>();
                suppLists.Add(new SupplierName { supp_id = SPI_Model.SuppPI_SuppID, supp_name = SPI_Model.SuppPI_SuppID });
                SPI_Model.SupplierNameList = suppLists;

                List<CurrancyList> currancyLists = new List<CurrancyList>();
                currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                SPI_Model.currancyLists = currancyLists;

                List<PurchaseInvoice> PurchaseInvLists = new List<PurchaseInvoice>();
                PurchaseInvLists.Add(new PurchaseInvoice { InvNo = "---Select---", Invdt = "0" });
                SPI_Model.PurchaseInvoiceList = PurchaseInvLists;

                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;
                GetSuppList(SPI_Model);
                if (SPI_Model.Inv_Nos != null && SPI_Model.SuppPI_inv_dt != null)
                {
                    SetData(SPI_Model);
                }
                GetAutoCompleteSearchSuppList(SPI_Model);
                SPI_Model.Title = title;
                ViewBag.title = title;
                SPI_Model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                //SPI_Model.Message = _SuppPI_ListModel.Message;
                return View("~/Areas/ApplicationLayer/Views/Procurement/SupplementaryPurchaseInvoice/SupplementaryPurchaseInvoiceDetail.cshtml", SPI_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(SuppPI_ListModel _SuppPI_ListModel)
        {
            try
            {
                string SupplierName = string.Empty;
                GetCompDeatil();
                if (string.IsNullOrEmpty(_SuppPI_ListModel.SuppPI_SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _SuppPI_ListModel.SuppPI_SuppName;
                }
                DataSet CustList = _SupplementaryPurchaseInvoiceIService.GetAllData(CompID, SupplierName, BrchID, User_ID
                    , _SuppPI_ListModel.SuppPI_SuppID, _SuppPI_ListModel.SuppPI_FromDate, _SuppPI_ListModel.SuppPI_ToDate, _SuppPI_ListModel.Status, _SuppPI_ListModel.SuppPI_wfdocid, _SuppPI_ListModel.SuppPI_wfstatus);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (DataRow data in CustList.Tables[0].Rows)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data["supp_id"].ToString();
                    _SuppDetail.supp_name = data["supp_name"].ToString();
                    _SuppList.Add(_SuppDetail);
                }
                _SuppList.Insert(0, new SupplierName() { supp_id = "0", supp_name = "All" });
                _SuppPI_ListModel.SupplierNameList = _SuppList;
                ViewBag.ListDetail = CustList.Tables[1];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
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
        private void SetDecimals(SuppPI_ListModel _model)
        {
            _model.ValDigit = ToFixDecimal(Convert.ToInt32((_model.DocumentMenuId == "105101140125" ? Session["ExpImpValDigit"] : Session["ValDigit"]).ToString()));
            _model.QtyDigit = ToFixDecimal(Convert.ToInt32((_model.DocumentMenuId == "105101140125" ? Session["ExpImpQtyDigit"] : Session["QtyDigit"]).ToString()));
            _model.RateDigit = ToFixDecimal(Convert.ToInt32((_model.DocumentMenuId == "105101140125" ? Session["ExpImpRateDigit"] : Session["RateDigit"]).ToString()));
            _model.ExchDigit = ToFixDecimal(Convert.ToInt32(Session["ExchDigit"].ToString()));
            ViewBag.ValDigit = _model.ValDigit;
            ViewBag.QtyDigit = _model.QtyDigit;
            ViewBag.RateDigit = _model.RateDigit;
            ViewBag.ExchDigit = _model.ExchDigit;
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
        public ActionResult SuppPIListSearch(string SuppId, string Fromdate, string Todate, string Status, string wfStatus)
        {
            try
            {
                GetCompDeatil();
                DataSet ds = _SupplementaryPurchaseInvoiceIService.GetAllData(CompID, "", BrchID, User_ID, SuppId, Fromdate, Todate, Status, DocumentMenuId, wfStatus);
                ViewBag.ListDetail = ds.Tables[1];
                ViewBag.ListSearch = "ListSearch";
                ViewBag.ListFilterData1 = SuppId + "," + Fromdate + "," + Todate + "," + Status + "," + wfStatus;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSupplementaryPurchaseInvoiceList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult DblClick(string DocNo, string DocDate, string ListFilterData, string WF_Status)
        {
            GetCompDeatil();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            SuppPI_ListModel dblclick = new SuppPI_ListModel();
            UrlData _url = new UrlData();
            dblclick.Inv_Nos = DocNo;
            dblclick.Inv_Dt1 = DocDate;
            dblclick.TransType = "Update";
            dblclick.BtnName = "BtnToDetailPage";
            dblclick.Command = "Refresh";
            SuppPI_ListModel _Model = new SuppPI_ListModel();
            TempData["ListFilterData"] = ListFilterData;
            _Model.Message = "New";
            _Model.Command = "Update";
            _Model.TransType = "Update";
            _Model.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = dblclick;
            _url.TransType = "Update";
            _url.BtnName = "BtnToDetailPage";
            _url.Inv_no = DocNo;
            _url.Inv_dt = DocDate;
            _url.Command = "Refresh";
            _url.ListFilterData = ListFilterData;
            _url.WF_Status = WF_Status;
            return RedirectToAction("SupplementaryPurchaseInvoiceDetailEdit", _url);
        }
        public ActionResult SupplementaryPurchaseInvoiceDetailEdit(UrlData urlData)
        {
            try
            {
                CommonPageDetails(); GetCompDeatil(); getDocumentName();
                SuppPI_ListModel model = new SuppPI_ListModel();

                /*Add by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.Inv_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }

                /*End Financial year check*/
                model.Command = urlData.Command;
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.Inv_Nos = urlData.Inv_no;
                model.SuppPI_inv_dt = urlData.Inv_dt;
                model.ListFilterData = urlData.ListFilterData;
                model.Message = urlData.Message;
                model.UserID = User_ID;

                List<SupplierName> SupplierNameLists = new List<SupplierName>();
                SupplierNameLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                model.SupplierNameList = SupplierNameLists;

                List<CurrancyList> currancyLists = new List<CurrancyList>();
                currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                model.currancyLists = currancyLists;

                List<PurchaseInvoice> PurchaseInvLists = new List<PurchaseInvoice>();
                PurchaseInvLists.Add(new PurchaseInvoice { InvNo = model.Purchaseinv_no, Invdt = model.Inv_Dt1 });
                model.PurchaseInvoiceList = PurchaseInvLists;

                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;

                if (model.Inv_Nos != null && model.SuppPI_inv_dt != null)
                {
                    DataSet ds = GetSuppPI_Edit(model.Inv_Nos, model.SuppPI_inv_dt, DocumentMenuId);
                    ViewBag.ItemDetails = ds.Tables[2];
                    ViewBag.SubItemDetails = ds.Tables[3];
                    ViewBag.AttechmentDetails = ds.Tables[10];
                    ViewBag.GLAccount = ds.Tables[9];
                    ViewBag.GLTOtal = ds.Tables[7];
                    ViewBag.ItemTaxDetails = ds.Tables[12];
                    ViewBag.OtherChargeDetails = ds.Tables[8];
                    ViewBag.TotalDetails = ds.Tables[0];
                    ViewBag.ItemTaxDetailsList = ds.Tables[16];
                    ViewBag.OCTaxDetails = ds.Tables[7];

                    ViewBag.ItemTDSDetails = ds.Tables[13];
                    ViewBag.ItemOC_TDSDetails = ds.Tables[14];
                    ViewBag.ItemTDSDetailsPrev = ds.Tables[17];
                    //ViewBag.ItemStockBatchWise = ds.Tables[16];
                    //ViewBag.ItemStockSerialWise = ds.Tables[17];


                    //model.dt_SubItemDetails=ds.Tables[3];
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

                    model.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                    model.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                    model.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                    model.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();

                    //--Purchase Invoice Details START
                   
                    model.Inv_Nos = ds.Tables[0].Rows[0]["spinv_no"].ToString();
                    model.SuppPI_inv_dt = ds.Tables[0].Rows[0]["spinv_dt"].ToString();
                    //--Purchase Invoice Details END
                    model.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                    model.GrossValueInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val_bs"]).ToString(ValDigit);
                    model.TaxAmountRecoverable = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_recov"]).ToString(ValDigit);
                    model.TaxAmountNonRecoverable = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(ValDigit);
                    model.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_oc_amt"]).ToString(ValDigit);
                    model.DocSuppOtherChargesTP = Convert.ToDecimal(ds.Tables[0].Rows[0]["oc_amt_TP"]).ToString(ValDigit);
                    model.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                    model.TDS_Amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tds_amt"]).ToString(ValDigit);

                    model.GrossValuePrev = ds.Tables[1].Rows[0]["gr_val"].ToString();
                    model.GrossValueInBasePrev = ds.Tables[1].Rows[0]["gr_val"].ToString();
                    model.TaxAmountRecoverablePrev = ds.Tables[1].Rows[0]["RecovAmt"].ToString();
                    model.TaxAmountNonRecoverablePrev = ds.Tables[1].Rows[0]["NRecovAmt"].ToString();
                    model.OtherChargesPrev = ds.Tables[1].Rows[0]["oc_amt_with_tp"].ToString();
                    model.NetAmountInBasePrev = ds.Tables[1].Rows[0]["net_val_bs"].ToString();
                    model.TDS_AmountPrev = ds.Tables[1].Rows[0]["tds_amt"].ToString();
                    //Purchase Invoice Details END

                    //--Difference Details START
                    model.TxtDiff_TotalAmountInSpec = ds.Tables[0].Rows[0]["tot_val_diff"].ToString();
                    model.TxtDiff_TotalAmountInBase = ds.Tables[0].Rows[0]["tot_val_bs_diff"].ToString();
                    model.TxtDiff_TotalTaxAmountRec = ds.Tables[0].Rows[0]["tot_tax_amt_recov_diff"].ToString();
                    model.TxtDiff_TotalTaxAmountNonRec = ds.Tables[0].Rows[0]["tot_tax_amt_nrecov_diff"].ToString();
                    model.TxtDiff_TotalOCAmount = ds.Tables[0].Rows[0]["tot_oc_amt_diff"].ToString();
                    model.TxtDiff_Amount = ds.Tables[0].Rows[0]["tot_net_amt_diff"].ToString();
                    model.TxtDiff_TotalTDSAmount = ds.Tables[0].Rows[0]["tds_amt_diff"].ToString();
                    //--Difference Details END

                    model.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                    model.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                    model.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                    model.DocSuppOtherCharges = ds.Tables[0].Rows[0]["oc_amt_self"].ToString();
                    model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                    model.Inv_Nos = ds.Tables[0].Rows[0]["spinv_no"].ToString();
                    model.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                    model.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                    model.conv_rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString(QtyDigit);

                    string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                    model.SuppCurrency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                    currancyLists.Add(new CurrancyList { curr_id = model.SuppCurrency, curr_name = Curr_name });
                    model.currancyLists = currancyLists;

                    model.SuppPI_SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                    model.SuppPI_SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();

                    SupplierNameLists.Add(new SupplierName { supp_id = model.SuppPI_SuppID, supp_name = model.SuppPI_SuppName });
                    model.SupplierNameList = SupplierNameLists;
                    model.Supp_Acc_Id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                    model.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                    GetPurchaseInvoices("D", model.SuppPI_SuppID);
                    GetAutoCompleteSearchPurchaseInvoiceList(model);
                    model.Purchaseinv_no = ds.Tables[1].Rows[0]["InvDt"].ToString();
                    model.GRNNumber = ds.Tables[1].Rows[0]["GRNNo"].ToString();
                    model.GRNDate = ds.Tables[1].Rows[0]["GRNdt"].ToString();
                    model.PurchaseInv_bill_no = ds.Tables[1].Rows[0]["BillNo"].ToString();
                    model.PurchaseInv_bill_dt = ds.Tables[1].Rows[0]["Billdt"].ToString();
                    model.DocSuppOtherChargesPrev = ds.Tables[1].Rows[0]["oc_amt"].ToString();
                    model.DocSuppOtherChargesPrevTP = ds.Tables[1].Rows[0]["oc_amt_tp"].ToString();

                    model.Purchaseinv_no = ds.Tables[0].Rows[0]["Purchaseinv_no"].ToString();
                    model.Inv_Dt1 = ds.Tables[0].Rows[0]["Purchaseinv_no"].ToString();
                    PurchaseInvLists.Add(new PurchaseInvoice { InvNo = model.Purchaseinv_no, Invdt = model.Inv_Dt1 });
                    model.PurchaseInvoiceList = PurchaseInvLists;

                    model.Inv_Dt1 = ds.Tables[1].Rows[0]["Invdt"].ToString(); //ds.Tables[1].Rows[0]["Invdt"].ToString();
                    model.bill_add_id = ds.Tables[0].Rows[0]["bill_add_id"].ToString();
                    model.Ship_Gst_number = ds.Tables[0].Rows[0]["supp_gst_no"].ToString();
                    model.Ship_StateCode = ds.Tables[0].Rows[0]["state_code"].ToString();

                    model.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                    model.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                    model.GrossValueInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val_bs"]).ToString(ValDigit);
                    model.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_oc_amt"]).ToString(ValDigit);
                    model.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                    model.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                    model.NetAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);

                    model.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                    model.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                    model.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();

                    model.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                    //model.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();

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
                    if (ds.Tables[9].Rows.Count > 0)
                    {
                        if (doc_status == "A" || doc_status == "C")
                        {
                            model.GLVoucherType = ds.Tables[9].Rows[0]["vou_type"].ToString();
                        }
                        model.GLVoucherNo = ds.Tables[9].Rows[0]["vou_no"].ToString();
                        model.GLVoucherDt = ds.Tables[9].Rows[0]["vou_dt"].ToString();
                        ViewBag.GLVoucherNo = model.GLVoucherNo;/*add by Hina Sharma on 14-08-2025*/
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
                            if (create_id != User_ID)
                            {
                                model.BtnName = "Refresh";
                            }
                            else
                            {
                                if (nextLevel == "0")
                                {
                                    if (create_id == User_ID)
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
                            if (User_ID == sent_to)
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
                                if (sent_to == User_ID)
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
                            if (User_ID == sent_to)
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
                                if (sent_to == User_ID)
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
                            if (create_id == User_ID || approval_id == User_ID)
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
                    ViewBag.CostCenterData = ds.Tables[15];
                }
                model.Title = title;
                model.DocumentMenuId = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                model.WF_Status = urlData.WF_Status;
                return View("~/Areas/ApplicationLayer/Views/Procurement/SupplementaryPurchaseInvoice/SupplementaryPurchaseInvoiceDetail.cshtml", model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public DataSet GetSuppPI_Edit(string Inv_no, string Inv_dt, string DocumentMenuId)
        {
            try
            {
                GetCompDeatil();
                DataSet result = _SupplementaryPurchaseInvoiceIService.GetDPIDetailDAL(CompID, BrchID, Inv_no, Inv_dt, User_ID, DocumentMenuId);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void SetData(SuppPI_ListModel DetailModel)
        {
            try
            {
                // GetItemList_Deatil(DetailModel);
                if (DetailModel.Inv_Dt1 == null)
                {
                    DetailModel.Inv_Dt1 = System.DateTime.Now.ToString("yyyy-MM-dd");
                }
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    DetailModel.ListFilterData = TempData["ListFilterData"].ToString();
                }
                if (DetailModel.TransType == "Update")
                {
                    SetAllDataInView(DetailModel);
                }
                else
                {
                    DetailModel.BatchCommand = DetailModel.Command;
                    ViewBag.DocumentCode = "0";
                    DetailModel.DocumentStatus = "New";

                }
                ViewBag.DocumentMenuId = DocumentMenuId;

                DetailModel.Title = title;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult GetItemList_Deatil(SuppPI_ListModel queryParameters)
        {
            GetCompDeatil();
            DataSet itemList1 = new DataSet();
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(JsonConvert.SerializeObject(itemList1.Tables[0]), JsonRequestBehavior.AllowGet);
        }
        public void GetCompDeatil()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["UserID"] != null)
            {
                User_ID = Session["UserID"].ToString();
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
        }
        public void SetAllDataInView(SuppPI_ListModel _DeatilModel)
        {
            try
            {
                GetCompDeatil();
                string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                ViewBag.ValDigit = ValDigit;
                ViewBag.QtyDigit = QtyDigit;
                ViewBag.RateDigit = RateDigit;

                List<SupplierName> SupplierNameLists = new List<SupplierName>();
                SupplierNameLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                _DeatilModel.SupplierNameList = SupplierNameLists;

                List<CurrancyList> currancyLists = new List<CurrancyList>();
                currancyLists.Add(new CurrancyList { curr_id = "0", curr_name = "---Select---" });
                _DeatilModel.currancyLists = currancyLists;

                List<PurchaseInvoice> PurchaseInvLists = new List<PurchaseInvoice>();
                //PurchaseInvLists.Add(new PurchaseInvoice { InvNo = "0", Invdt = "---Select---" });
                // PurchaseInvLists.Add(new PurchaseInvoice { InvNo = _DeatilModel.Purchaseinv_no, InvNo = _DeatilModel.Inv_Dt1 });
                _DeatilModel.PurchaseInvoiceList = PurchaseInvLists;

                string Doc_no = _DeatilModel.Inv_Nos;
                string Doc_dt = _DeatilModel.SuppPI_inv_dt;
                DataSet ds = new DataSet();
                if (_DeatilModel.Inv_Nos != null && _DeatilModel.SuppPI_inv_dt != null)
                {
                    ds = _SupplementaryPurchaseInvoiceIService.GetDPIDetailDAL(CompID, BrchID, Doc_no, Doc_dt, User_ID, DocumentMenuId);

                    ViewBag.ItemDetails = ds.Tables[2];
                    ViewBag.SubItemDetails = ds.Tables[3];
                    ViewBag.AttechmentDetails = ds.Tables[10];
                    ViewBag.GLAccount = ds.Tables[9];
                    //ViewBag.GLTOtal = ds.Tables[9];
                    ViewBag.ItemTaxDetails = ds.Tables[12];
                    ViewBag.OtherChargeDetails = ds.Tables[8];
                    ViewBag.TotalDetails = ds.Tables[0];
                    ViewBag.ItemTaxDetailsList = ds.Tables[16];
                    ViewBag.OCTaxDetails = ds.Tables[7];
                    ViewBag.ItemTDSDetails = ds.Tables[13];
                    ViewBag.ItemTDSDetailsPrev = ds.Tables[17];
                    ViewBag.ItemOC_TDSDetails = ds.Tables[14];

                    //---------------------------Header Start ------------------------------
                    _DeatilModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                    _DeatilModel.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                    _DeatilModel.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                    _DeatilModel.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                    _DeatilModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                    _DeatilModel.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                    _DeatilModel.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                    _DeatilModel.Status_name = ds.Tables[0].Rows[0]["InvoiceStatus"].ToString();
                    //---------------------------Header END ------------------------------

                    //---------------------------Invoice Detail Start ------------------------------
                    string Curr_name = ds.Tables[0].Rows[0]["curr_name"].ToString();
                    _DeatilModel.SuppCurrency = ds.Tables[0].Rows[0]["curr_id"].ToString();
                    currancyLists.Add(new CurrancyList { curr_id = _DeatilModel.SuppCurrency, curr_name = Curr_name });
                    _DeatilModel.currancyLists = currancyLists;

                    _DeatilModel.SuppPI_SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                    _DeatilModel.SuppPI_SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();

                    SupplierNameLists.Add(new SupplierName { supp_id = _DeatilModel.SuppPI_SuppID, supp_name = _DeatilModel.SuppPI_SuppName });
                    _DeatilModel.SupplierNameList = SupplierNameLists;
                    _DeatilModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();

                    //GetPurchaseInvoices("D", _DeatilModel.SuppPI_SuppID);
                    //GetAutoCompleteSearchPurchaseInvoiceList(_DeatilModel);

                    _DeatilModel.ExRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                    _DeatilModel.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                    _DeatilModel.bill_date = ds.Tables[0].Rows[0]["BillDate"].ToString();
                    _DeatilModel.TDS_Amount = ds.Tables[0].Rows[0]["tds_amt"].ToString();
                    _DeatilModel.conv_rate = Convert.ToDecimal(ds.Tables[0].Rows[0]["conv_rate"]).ToString();
                    _DeatilModel.SuppPI_SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                    _DeatilModel.Supp_Acc_Id = ds.Tables[0].Rows[0]["supp_acc_id"].ToString();
                    _DeatilModel.SuppTdsApplicable = ds.Tables[0].Rows[0]["tds_posting"].ToString();
                    _DeatilModel.Hd_GstCat = ds.Tables[0].Rows[0]["gst_cat"].ToString();
                    _DeatilModel.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                    _DeatilModel.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                    _DeatilModel.DocSuppOtherCharges_self = ds.Tables[0].Rows[0]["oc_amt_self"].ToString();
                    _DeatilModel.curr_id = ds.Tables[0].Rows[0]["curr_id"].ToString();
                    _DeatilModel.conv_rate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                    //---------------------------Invoice Detail Start ------------------------------
                    _DeatilModel.Address = ds.Tables[0].Rows[0]["SuppAddress"].ToString();
                    _DeatilModel.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();

                    //---------------------------Purchase Invoice Detail Start ------------------------------
                    _DeatilModel.Purchaseinv_no = ds.Tables[1].Rows[0]["inv_no"].ToString();
                    _DeatilModel.Inv_Dt1 = ds.Tables[1].Rows[0]["Invdt"].ToString();
                    PurchaseInvLists.Add(new PurchaseInvoice { InvNo = _DeatilModel.Purchaseinv_no, Invdt = _DeatilModel.Inv_Dt1 });
                    _DeatilModel.PurchaseInvoiceList = PurchaseInvLists;

                    _DeatilModel.PurchaseInv_bill_no = ds.Tables[1].Rows[0]["BillNo"].ToString();
                    _DeatilModel.PurchaseInv_bill_dt = ds.Tables[1].Rows[0]["Billdt"].ToString();
                    _DeatilModel.GRNNumber = ds.Tables[1].Rows[0]["GRNNo"].ToString();
                    _DeatilModel.GRNDate = ds.Tables[1].Rows[0]["GRNdt"].ToString();
                    //---------------------------Purchase Invoice Detail End ------------------------------

                    //--Difference Details START
                    _DeatilModel.GrossValue = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val"]).ToString(ValDigit);
                    _DeatilModel.GrossValueInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["gr_val_bs"]).ToString(ValDigit);
                    _DeatilModel.TaxAmountRecoverable = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_recov"]).ToString(ValDigit);
                    _DeatilModel.TaxAmountNonRecoverable = Convert.ToDecimal(ds.Tables[0].Rows[0]["tax_amt_nrecov"]).ToString(ValDigit);
                    _DeatilModel.OtherCharges = Convert.ToDecimal(ds.Tables[0].Rows[0]["tot_oc_amt"]).ToString(ValDigit);
                    _DeatilModel.NetAmountInBase = Convert.ToDecimal(ds.Tables[0].Rows[0]["net_val_bs"]).ToString(ValDigit);
                    _DeatilModel.TDS_Amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["tds_amt"]).ToString(ValDigit);

                    _DeatilModel.GrossValuePrev = ds.Tables[1].Rows[0]["gr_val"].ToString();
                    _DeatilModel.GrossValueInBasePrev = ds.Tables[1].Rows[0]["gr_val"].ToString();
                    _DeatilModel.TaxAmountRecoverablePrev = ds.Tables[1].Rows[0]["RecovAmt"].ToString();
                    _DeatilModel.TaxAmountNonRecoverablePrev = ds.Tables[1].Rows[0]["NRecovAmt"].ToString();
                    _DeatilModel.OtherChargesPrev = ds.Tables[1].Rows[0]["oc_amt_with_tp"].ToString();
                    _DeatilModel.NetAmountInBasePrev = ds.Tables[1].Rows[0]["net_val_bs"].ToString();
                    _DeatilModel.TDS_AmountPrev = ds.Tables[1].Rows[0]["tds_amt"].ToString();

                    _DeatilModel.TxtDiff_TotalAmountInSpec = ds.Tables[0].Rows[0]["tot_val_diff"].ToString();
                    _DeatilModel.TxtDiff_TotalAmountInBase = ds.Tables[0].Rows[0]["tot_val_bs_diff"].ToString();
                    _DeatilModel.TxtDiff_TotalTaxAmountRec = ds.Tables[0].Rows[0]["tot_tax_amt_recov_diff"].ToString();
                    _DeatilModel.TxtDiff_TotalTaxAmountNonRec = ds.Tables[0].Rows[0]["tot_tax_amt_nrecov_diff"].ToString();
                    _DeatilModel.TxtDiff_TotalOCAmount = ds.Tables[0].Rows[0]["tot_oc_amt_diff"].ToString();
                    _DeatilModel.TxtDiff_Amount = ds.Tables[0].Rows[0]["tot_net_amt_diff"].ToString();
                    _DeatilModel.TxtDiff_TotalTDSAmount = ds.Tables[0].Rows[0]["tds_amt_diff"].ToString();
                    //--Difference Details END

                    //--Purchase Invoice Details START
                    _DeatilModel.Inv_Nos = ds.Tables[0].Rows[0]["spinv_no"].ToString();
                    _DeatilModel.SuppPI_inv_dt = ds.Tables[0].Rows[0]["spinv_dt"].ToString();
                    //Purchase Invoice Details END

                    //------------------Other Details START------------------------------------
                    _DeatilModel.PriceBasis = ds.Tables[0].Rows[0]["price_basis"].ToString();
                    _DeatilModel.FreightType = ds.Tables[0].Rows[0]["freight_type"].ToString();
                    _DeatilModel.ModeOfTransport = ds.Tables[0].Rows[0]["mode_trans"].ToString();
                    _DeatilModel.Destination = ds.Tables[0].Rows[0]["dest"].ToString();
                    //------------------Other Details END------------------------------------

                    string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                    ViewBag.Approve_id = approval_id;
                    string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                    string doc_status = ds.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                    _DeatilModel.Status = doc_status;
                    _DeatilModel.doc_status = doc_status;
                    _DeatilModel.DocumentStatus = doc_status;
                    ViewBag.DocumentCode = doc_status;

                    if (ds.Tables[0].Rows[0]["inv_status"].ToString().Trim() == "C")
                    {
                        _DeatilModel.CancelFlag = true;
                        _DeatilModel.BtnName = "Refresh";
                    }
                    else
                    {
                        _DeatilModel.CancelFlag = false;
                    }
                    _DeatilModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                    _DeatilModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);

                    if (doc_status != "D" && doc_status != "F")
                    {
                        ViewBag.AppLevel = ds.Tables[6];
                    }
                    if (ViewBag.AppLevel != null && _DeatilModel.Command != "Edit")
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
                                _DeatilModel.BtnName = "Refresh";
                            }
                            else
                            {
                                if (nextLevel == "0")
                                {
                                    if (create_id == User_ID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                    }
                                    _DeatilModel.BtnName = "BtnEdit";
                                }
                                else
                                {
                                    ViewBag.Approve = "N";
                                    ViewBag.ForwardEnbl = "Y";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    _DeatilModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (User_ID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";

                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                _DeatilModel.BtnName = "BtnEdit";
                            }
                            if (nextLevel == "0")
                            {
                                if (sent_to == User_ID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    _DeatilModel.BtnName = "BtnEdit";
                                }
                            }
                        }
                        if (doc_status == "F")
                        {
                            if (User_ID == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                _DeatilModel.BtnName = "BtnEdit";
                            }
                        }
                        if (doc_status == "A")
                        {
                            if (create_id == User_ID || approval_id == User_ID)
                            {
                                _DeatilModel.BtnName = "BtnToDetailPage"; //"BtnEdit";
                            }
                            else
                            {
                                _DeatilModel.BtnName = "Refresh";
                            }
                        }
                    }
                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }
                }
                _DeatilModel.BatchCommand = _DeatilModel.Command;
                _DeatilModel.WFStatus = _DeatilModel.WFStatus;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult ToRefreshByJS(string FilterData, string TrancType, string Mailerror)
        {
            SuppPI_ListModel _PI_ListModel = new SuppPI_ListModel();
            //Session["Message"] = "";
            _PI_ListModel.Message = "";
            var a = TrancType.Split(',');
            _PI_ListModel.DocumentMenuId = a[0].Trim();
            _PI_ListModel.Inv_Nos = a[1].Trim();
            _PI_ListModel.SuppPI_inv_dt = a[2].Trim();
            _PI_ListModel.TransType = "Update";
            var WF_status1 = a[3].Trim();
            _PI_ListModel.WF_Status = WF_status1;
            _PI_ListModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _PI_ListModel;
            TempData["WF_status"] = WF_status1;
            TempData["FilterData"] = FilterData;
            var Inv_no = _PI_ListModel.Inv_Nos;
            var Inv_dt = _PI_ListModel.SuppPI_inv_dt;
            var TransType = _PI_ListModel.TransType;
            var BtnName = _PI_ListModel.BtnName;
            var Command = _PI_ListModel.Command;
            var menuDocumentID = _PI_ListModel.DocumentMenuId;
            return RedirectToAction("SupplementaryPurchaseInvoiceDetail", new { Inv_no = Inv_no, Inv_dt, TransType, BtnName, Command, menuDocumentID, WF_status1, Mailerror });
        }
        private void CommonPageDetails()
        {
            try
            {
                GetCompDeatil();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, User_ID, DocumentMenuId, language);
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
        [HttpPost]
        public ActionResult GetSuppList(SuppPI_ListModel _SuppPI_ListModel)
        {
            string SupplierName = string.Empty, SuppType = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            try
            {
                GetCompDeatil();
                if (string.IsNullOrEmpty(_SuppPI_ListModel.SuppPI_SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _SuppPI_ListModel.SuppPI_SuppName;
                }
                SuppType = "D"; //Default Value
                CustList = _SupplementaryPurchaseInvoiceIService.GetSupplierList(CompID, SupplierName, BrchID, SuppType);
                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _SuppPI_ListModel.SupplierNameList = _SuppList;
                return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json(new { success = false, message = "An error occurred on the server. Please try again later." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSuppList_OrderType(string SuppType)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            // string SuppType = string.Empty;
            SuppPI_ListModel _SuppPI_ListModel = new SuppPI_ListModel();
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
                // Call your service to get supplier list
                CustList = _SupplementaryPurchaseInvoiceIService.GetSupplierList(Comp_ID, SupplierName, Br_ID, SuppType);
                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _SuppPI_ListModel.SupplierNameList = _SuppList;
                // Return success response as JSON
                return Json(CustList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the error
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // Return a JSON error response
                return Json(new { success = false, message = "An error occurred on the server. Please try again later." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetPurchaseInvoices(string InvoiceType, string Supp_id)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet Details = _SupplementaryPurchaseInvoiceIService.GetPurchaseInvoicesList(CompID, Supp_id, BrchID, InvoiceType);
                DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
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
        public JsonResult GetPurchaseInvoicesDetails(string InvoiceType, string InvNo)
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
                DataSet Details = _SupplementaryPurchaseInvoiceIService.GetPurchaseInvoicesDetailsList(Comp_ID, InvNo, Br_ID, InvoiceType);
                DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
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
        public JsonResult GetPurchaseInvoicesItemDetails(string InvoiceType, string InvNo)
        {
            JsonResult DataRows = null;
            try
            {
                GetCompDeatil();
                DataSet Details = _SupplementaryPurchaseInvoiceIService.GetPurchaseInvoicesItemDetailsList(CompID, InvNo, BrchID, InvoiceType);
                DataRows = Json(JsonConvert.SerializeObject(Details));/*Result convert into Json Format for javasript*/
                //ViewBag.ItemOC_TDSDetails = Details.Tables[6];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }

        public ActionResult GetAutoCompleteSearchPurchaseInvoiceList(SuppPI_ListModel _SuppPI_ListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> PInvtList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;

            string Supp_id = null;
            Supp_id = _SuppPI_ListModel.SuppPI_SuppID;
            // string SuppType = string.Empty;
            SuppPI_ListModel _SuppPI_ListModels = new SuppPI_ListModel();
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
                // Call your service to get supplier list
                PInvtList = _SupplementaryPurchaseInvoiceIService.GetPurchaseInvoicesList1(Comp_ID, Supp_id, Br_ID, "D");
                List<PurchaseInvoice> _PInvList = new List<PurchaseInvoice>();
                foreach (var data in PInvtList)
                {
                    PurchaseInvoice _PInvDetail = new PurchaseInvoice();
                    _PInvDetail.InvNo = data.Key;
                    _PInvDetail.Invdt = data.Value;
                    _PInvList.Add(_PInvDetail);
                }
                _SuppPI_ListModel.PurchaseInvoiceList = _PInvList;
                // Return success response as JSON
                return Json(PInvtList.Select(c => new { Name = c.Key, ID = c.Value }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the error
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // Return a JSON error response
                return Json(new { success = false, message = "An error occurred on the server. Please try again later." }, JsonRequestBehavior.AllowGet);
            }
        }

        //rivate List<SuppPurchaseInvoiceList> GetGRNDetailList(SuppPI_ListModel _SuppPI_ListModel)
        private DataSet GetGRNDetailList(SuppPI_ListModel _SuppPI_ListModel)
        {
            DataSet dt = new DataSet();
            try
            {
                List<SuppPurchaseInvoiceList> _SuppPurchaseInvoiceList = new List<SuppPurchaseInvoiceList>();
                GetCompDeatil();
                dt = _SupplementaryPurchaseInvoiceListIService.GetSPI_DetailList(CompID, BrchID, User_ID, _SuppPI_ListModel.SuppPI_SuppID, _SuppPI_ListModel.SuppPI_FromDate, _SuppPI_ListModel.SuppPI_ToDate, _SuppPI_ListModel.Status, _SuppPI_ListModel.SuppPI_wfdocid, _SuppPI_ListModel.SuppPI_wfstatus);
                //if (dt.Tables[1].Rows.Count > 0)
                //{
                //    //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                //}
                //if (dt.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Tables[0].Rows)
                //    {
                //        SuppPurchaseInvoiceList _SuppPIList = new SuppPurchaseInvoiceList();
                //        _SuppPIList.InvoiceNo = dr["spinv_no"].ToString();
                //        _SuppPIList.InvoiceDate = dr["SpInvDate"].ToString();
                //        _SuppPIList.InvDate = dr["InvDt"].ToString();
                //        _SuppPIList.Mr_No = dr["Pinv_no"].ToString();
                //        _SuppPIList.Mr_Date = dr["Pinv_dt"].ToString();
                //        _SuppPIList.InvoiceType = dr["InvType"].ToString();
                //        _SuppPIList.SuppName = dr["supp_name"].ToString();
                //        _SuppPIList.SuppCurrency = dr["curr"].ToString();
                //        _SuppPIList.InvoiceValue = dr["net_val"].ToString();
                //        _SuppPIList.Stauts = dr["Status"].ToString();
                //        _SuppPIList.CreateDate = dr["CreateDate"].ToString();
                //        _SuppPIList.ApproveDate = dr["ApproveDate"].ToString();
                //        _SuppPIList.ModifyDate = dr["ModifyDate"].ToString();
                //        _SuppPIList.create_by = dr["create_by"].ToString();
                //        _SuppPIList.app_by = dr["app_by"].ToString();
                //        _SuppPIList.mod_by = dr["mod_by"].ToString();
                //        _SuppPIList.BillNumber = dr["bill_no"].ToString();
                //        _SuppPIList.BillDate = dr["bill_date"].ToString();
                //        _SuppPurchaseInvoiceList.Add(_SuppPIList);
                //    }
                //}
                SuppPI_ListModel _SuppPIList = new SuppPI_ListModel();
                _SuppPIList.WF_Status = _SuppPI_ListModel.SuppPI_wfstatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
            return dt;
        }
        public ActionResult GetAutoCompleteSearchSuppList(SuppPI_ListModel _SuppPI_ListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> CustList = new Dictionary<string, string>();
            string SuppType = string.Empty;
            try
            {
                GetCompDeatil();
                if (string.IsNullOrEmpty(_SuppPI_ListModel.SuppPI_SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _SuppPI_ListModel.SuppPI_SuppName;
                }
                //if (_SuppPI_ListModel.DocumentMenuId != null && _SuppPI_ListModel.DocumentMenuId != "")
                //{
                //    if (_SuppPI_ListModel.DocumentMenuId == "105101147")
                //    {
                //        SuppType = "D";
                //    }
                //}
                SuppType = "D";
                CustList = _SupplementaryPurchaseInvoiceIService.GetSupplierList(CompID, SupplierName, BrchID, SuppType);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _SuppPI_ListModel.SupplierNameList = _SuppList;
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
        private string getDocumentName()
        {
            try
            {
                GetCompDeatil();
                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
                string[] Docpart = DocumentName.Split('>');
                int len = Docpart.Length;
                if (len > 1)
                {
                    title = Docpart[len - 1].Trim();
                }
                ViewBag.title = title.ToString();
                return DocumentName;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string Inv_no = null, string Inv_dt = null, string ListFilterData = null)
        {
            try
            {
                urlData.Command = Command;
                urlData.TransType = TransType;
                ViewBag.TransType = TransType;
                urlData.BtnName = BtnName;
                urlData.Inv_no = Inv_no;
                urlData.Inv_dt = Inv_dt;
                urlData.ListFilterData = ListFilterData;
                TempData["UrlData"] = urlData;
                TempData["Message"] = Message;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        public string CheckDPIForCancellationinReturn(string DocNo, string DocDate)
        {
            string Result = "";
            try
            {
                GetCompDeatil();
                DataSet Deatils = _SupplementaryPurchaseInvoiceIService.CheckSuppPIDetail(CompID, BrchID, DocNo, DocDate);
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

        private DataTable ToDtblbatchDetail(string BatchItemDeatilData)
        {
            try
            {
                DataTable Batchitemtable = new DataTable();
                Batchitemtable.Columns.Add("item_id", typeof(string));
                Batchitemtable.Columns.Add("batch_no", typeof(string));
                Batchitemtable.Columns.Add("batch_qty", typeof(string));
                Batchitemtable.Columns.Add("exp_dt", typeof(string));

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
                DataTable Serialitemtable = new DataTable();
                Serialitemtable.Columns.Add("item_id", typeof(string));
                Serialitemtable.Columns.Add("serial_no", typeof(string));

                if (SerialItemDeatilData != null)
                {
                    JArray obj1 = JArray.Parse(SerialItemDeatilData);
                    for (int i = 0; i < obj1.Count; i++)
                    {
                        DataRow Serialitemrow = Serialitemtable.NewRow();
                        Serialitemrow["item_id"] = obj1[i]["itemid"].ToString();
                        Serialitemrow["serial_no"] = obj1[i]["SerialNo"].ToString();
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

        public ActionResult SaveSuppPIDetails(SuppPI_ListModel _model)
        {
            try
            {
                string SaveMessage = "";
                string PageName = _model.Title.Replace(" ", "");
                GetCompDeatil();

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
                DtblSubItem = ToDtblSubItem(_model.dt_SubItemDetails);
                DtblTaxDetail = ToDtblTaxDetail(_model.TaxDetail, "Y");
                //DtblOCTaxDetail = ToDtblOcDetail(_model.OCDetail);
                DtblOCTaxDetail = ToDtblTaxOCDetail(_model.OC_TaxDetail, "Y");
                DtblIOCDetail = ToDtblOcDetail(_model.OCDetail);
                DtblTdsDetail = ToDtblTdsDetail(_model.Tds_details, "");
                DtblOcTdsDetail = ToDtblTdsDetail(_model.Oc_tds_details, "OC");
                DtblVouDetail = ToDtblvouDetail(_model.vouDetail);
                DtblAttchDetail = ToDtblAttachmentDetail(_model);
                CostCenterDetails = ToDtblccDetail(_model.CC_DetailList);

                var _SuppPurchaseInvoiceattch = TempData["ModelDataattch"] as SuppPurchaseInvoiceattch;
                TempData["ModelDataattch"] = null;

                string Nurr = "";
                if (_model.CancelFlag)
                {
                    Nurr = _model.Nurration + $" {Resource.Cancelled} {Resource.On} {DateTime.Now.ToString("dd-MM-yyyy hh:mm")}.";
                }
                string tds_amt = _model.TDS_Amount == null ? "0" : _model.TDS_Amount;
                SaveMessage = _SupplementaryPurchaseInvoiceIService.InsertDPI_Details(DtblHDetail, DtblItemDetail, DtblSubItem, DtblTaxDetail, DtblOCTaxDetail, DtblIOCDetail, DtblTdsDetail, DtblOcTdsDetail, DtblVouDetail, DtblAttchDetail, CostCenterDetails, Nurr, tds_amt);
                if (SaveMessage == "DocModify")
                {
                    _model.Message = "DocModify";
                    _model.BtnName = "Refresh";
                    _model.Command = "Refresh";
                    return RedirectToAction("SupplementaryPurchaseInvoiceDetail");
                }
                else if (SaveMessage == "Cancelled")
                {
                    try
                    {
                        //string fileName = "SI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        //var filePath = SavePdfDocToSendOnEmailAlert(DocumentMenuId, Inv_No, Inv_Date, fileName);
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _model.Inv_No, "Cancel", User_ID, "");
                    }
                    catch (Exception exMail)
                    {
                        _model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _model.Message = "Cancelled";
                    _model.Command = "Update";
                    _model.AppStatus = "D";
                    _model.BtnName = "Refresh";
                    _model.TransType = "Update";
                    return RedirectToAction("SupplementaryPurchaseInvoiceDetail");
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
                        return RedirectToAction("SupplementaryPurchaseInvoiceDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_SuppPurchaseInvoiceattch != null)
                        {
                            if (_SuppPurchaseInvoiceattch.Guid != null)
                            {
                                Guid = _SuppPurchaseInvoiceattch.Guid;
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
                        _model.Inv_Nos = Inv_no;
                        _model.SuppPI_inv_dt = Inv_DATE;
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
                            _model.Inv_Nos = Inv_no;
                            _model.SuppPI_inv_dt = Inv_DATE;
                            _model.AppStatus = "D";
                            _model.BtnName = "BtnSave";
                            _model.TransType = "Update";
                        }
                    }
                    return RedirectToAction("SupplementaryPurchaseInvoiceDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void DeleteSuppPIDetail(SuppPI_ListModel _model)
        {
            try
            {
                GetCompDeatil();
                string Result = _SupplementaryPurchaseInvoiceIService.DeleteDetails(CompID, BrchID, _model.Inv_Nos, _model.SuppPI_inv_dt);
                _model.Message = Result.Split(',')[0];
                _model.Message = "Deleted";
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }

        public ActionResult ApproveSuppPIDetails(SuppPI_ListModel _model, string Inv_No, string Inv_Date, string A_Status, string A_Level, string A_Remarks, string VoucherNarr, string FilterData, string docid, string WF_Status1, string Bp_Nurr, string Dn_Nurration, string Cn_Nurration)
        {
            try
            {
                UrlData urlData = new UrlData();
                GetCompDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Dn_Nurr = _model.DN_Nurration == null ? Dn_Nurration : _model.DN_Nurration;
                string Cn_Nurr = _model.CN_Nurration == null ? Cn_Nurration : _model.CN_Nurration;
                string Result = _SupplementaryPurchaseInvoiceIService.ApproveDPIDetail(Inv_No, Inv_Date, docid, BrchID
                    , CompID, User_ID, mac_id, A_Status, A_Level, A_Remarks, VoucherNarr, Bp_Nurr, Dn_Nurr, Cn_Nurr);
                try
                {
                    //string fileName = "SI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    //var filePath = SavePdfDocToSendOnEmailAlert(DocumentMenuId, Inv_No, Inv_Date, fileName);
                    _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, Inv_No, "AP", User_ID, "");
                }
                catch (Exception exMail)
                {
                    _model.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                _model.Message = Result.Split(',')[1] == "A" ? "Approved" : "Error";
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, Result.Split(',')[0], Result.Split(',')[7], FilterData);
                urlData.Message = _model.Message;
                return RedirectToAction("SupplementaryPurchaseInvoiceDetailEdit", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public FileResult GenratePdfFile(SuppPI_ListModel _model)
        {
            return File(GetPdfData(_model.DocumentMenuId, _model.Inv_Nos, _model.SuppPI_inv_dt), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
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
                DataSet Details = _SupplementaryPurchaseInvoiceIService.GetDirectPurchaseInvoiceDeatilsForPrint(CompID, BrchID, invNo, invDt);
                ViewBag.PageName = "PI";
                //string invType = Details.Tables[0].Rows[0]["inv_type"].ToString().Trim();
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.InvoiceTo = "";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["inv_status"].ToString().Trim();
                //ViewBag.ProntOption = ProntOption;
                string htmlcontent = "";
                ViewBag.Title = "Direct Purchase Invoice";
                htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/SupplementaryPurchaseInvoice/DirectPurchaseInvoicePrint.cshtml"));

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

        public ActionResult AddSupplementaryPurchaseInvoiceDetail(string DocNo, string DocDate, string ListFilterData)
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
                if (DocNo == null)
                {
                    if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    {

                        TempData["Message"] = "Financial Year not Exist";
                        SetUrlData(urlData, "", "", "", "Financial Year not Exist", DocNo, DocDate, ListFilterData);
                        return RedirectToAction("SupplementaryPurchaseInvoice", "SupplementaryPurchaseInvoice", urlData);
                    }
                }

                /*End to chk Financial year exist or not*/
                string BtnName = DocNo == null ? "BtnAddNew" : "BtnToDetailPage";
                string TransType = DocNo == null ? "Save" : "Update";
                SetUrlData(urlData, "Add", TransType, BtnName, null, DocNo, DocDate, ListFilterData);
                return RedirectToAction("SupplementaryPurchaseInvoiceDetail", "SupplementaryPurchaseInvoice", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
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
                DataSet Deatils = _SupplementaryPurchaseInvoiceIService.CheckPIDetail(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count == 0)
                {
                    Result = "InvoiceCannotbeModified";
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
        [ValidateAntiForgeryToken]
        public ActionResult ActionSuppPIDetails(SuppPI_ListModel _model, string Command)
        {
            try
            {
                /*start Add by Hina on 12-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                UrlData urlData = new UrlData();

                if (_model.SuppPI_DeleteCommand == "Delete")
                {
                    Command = "Delete";
                }
                GetCompDeatil();
                switch (Command)
                {
                    case "AddNew":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_model.Inv_Nos))
                            {
                                SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", "Financial Year not Exist", _model.Inv_Nos, _model.SuppPI_inv_dt, _model.ListFilterData);
                                _model.Message = "Financial Year not Exist";
                                return RedirectToAction("SupplementaryPurchaseInvoiceDetail", urlData);
                            }
                            //return RedirectToAction("AddDirectPurchaseInvoiceDetail", new { DocNo = _model.SPO_No, DocDate = _model.SPO_Date, ListFilterData = _model.ListFilterData1, WF_status = _Model.WFStatus });
                            else
                            {
                                //_model.Command = "Refresh";
                                //_model.TransType = "Refresh";
                                //_model.BtnName = "BtnRefresh";
                                //_model.DocumentStatus = null;
                                //TempData["ModelData"] = _model;
                                SetUrlData(urlData, "Refresh", "Refresh", "Refresh", "Financial Year not Exist", null, null, _model.ListFilterData);
                                _model.Message = "Financial Year not Exist";
                                return RedirectToAction("SupplementaryPurchaseInvoiceDetail", urlData);

                            }
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew", null, null, null, _model.ListFilterData);

                        return RedirectToAction("SupplementaryPurchaseInvoiceDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickOnList", new { DocNo = _Model.SPO_No, DocDate = _Model.SPO_Date, ListFilterData = _Model.ListFilterData1, WF_status = _Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 07-05-2025 to check Existing with previous year transaction*/
                        string invdt = _model.SuppPI_inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, invdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", null, _model.Inv_Nos, _model.SuppPI_inv_dt, _model.ListFilterData);
                            return RedirectToAction("SupplementaryPurchaseInvoiceDetailEdit", urlData);
                        }
                        /*End to chk Financial year exist or not*/
                        if (_model.doc_status == "A")
                        {
                            string checkforCancel = CheckDPIForCancellationinReturn(_model.Inv_Nos, _model.SuppPI_inv_dt);
                            if (checkforCancel != "")
                            {
                                SetUrlData(urlData, "Refresh", "Update", "BtnToDetailPage", checkforCancel, _model.Inv_Nos, _model.SuppPI_inv_dt, _model.ListFilterData);
                                urlData.Message = checkforCancel;
                            }
                            else
                            {
                                SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _model.Inv_Nos, _model.SuppPI_inv_dt, _model.ListFilterData);
                            }
                        }
                        else
                        {
                            SetUrlData(urlData, "Edit", "Update", "BtnEdit", null, _model.Inv_Nos, _model.SuppPI_inv_dt, _model.ListFilterData);
                        }
                        return RedirectToAction("SupplementaryPurchaseInvoiceDetailEdit", urlData);

                    case "Save":
                        string checkforCancle_onSave = CheckPIForCancellationinReturn(_model.Purchaseinv_no, _model.Inv_Dt1);
                        if (checkforCancle_onSave != "") 
                        {
                            _model.Message = "InvoiceCannotbeModified";
                            _model.BtnName = "BtnToDetailPage";
                            TempData["ModelData"] = _model;
                            TempData["FilterData"] = _model.FilterData1;
                        }
                        else
                        {
                            SaveSuppPIDetails(_model);
                        }
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Inv_Nos, _model.SuppPI_inv_dt, _model.ListFilterData);
                        urlData.Message = _model.Message;
                        return RedirectToAction("SupplementaryPurchaseInvoiceDetailEdit", urlData);

                    case "Approve":
                        /*start Add by Hina on 07-05-2025 to chk Financial year exist or not*/
                        string Invdt1 = _model.SuppPI_inv_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, Invdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            SetUrlData(urlData, "Update", "Update", "BtnToDetailPage", _model.Message, _model.Inv_Nos, _model.SuppPI_inv_dt, _model.ListFilterData);
                            return RedirectToAction("SupplementaryPurchaseInvoiceDetailEdit", urlData);
                        }
                        /*End to chk Financial year exist or not*/

                        ApproveSuppPIDetails(_model, _model.Inv_Nos, _model.SuppPI_inv_dt, "", "", "", _model.Nurration, "", "", "", _model.BP_Nurration, _model.DN_Nurration, _model.CN_Nurration);
                        urlData.Message = _model.Message;
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, _model.Inv_Nos, _model.SuppPI_inv_dt, _model.ListFilterData);
                        return RedirectToAction("SupplementaryPurchaseInvoiceDetailEdit", urlData);

                    case "Refresh":
                        _model.BtnName = "";
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _model.ListFilterData);
                        _model.BtnName = "Refresh";
                        return RedirectToAction("SupplementaryPurchaseInvoiceDetail", urlData);
                    case "Delete":
                        DeleteSuppPIDetail(_model);
                        urlData.Message = _model.Message;
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _model.Message, null, null, _model.ListFilterData);
                        return RedirectToAction("SupplementaryPurchaseInvoiceDetailEdit", "SupplementaryPurchaseInvoice", urlData);
                    case "BacktoList":
                        SetUrlData(urlData, "", "", "", null, null, null, _model.ListFilterData);
                        TempData["WF_Status"] = _model.WF_Status;
                        TempData["FilterData"] = _model.FilterData1;
                        SuppPI_ListModel _modela = new SuppPI_ListModel();
                        _modela.WF_Status = _model.WF_Status;
                        return RedirectToAction("SupplementaryPurchaseInvoice", _modela);
                    case "Print":
                        //  return GenratePdfFile(_model);
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("SupplementaryPurchaseInvoiceDetail", urlData);
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("SupplementaryPurchaseInvoiceDetail", urlData);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult VarienceDetails(string GRNNo, string GRNDate, string ItmCode, string DocumentMenuId, string SubItem)
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
                DataTable Deatils = _SupplementaryPurchaseInvoiceIService.GetVarienceDetails(Comp_ID, Br_ID, GRNNo, GRNDate, ItmCode).Tables[0];
                ViewBag.VarienceDetail = Deatils;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.PopSubItem = SubItem;
                return PartialView("~/Areas/Common/Views/_VarianceDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        /*---------------------------------Sub-Item Start-------------------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, string Flag, string Status, string Doc_no, string Doc_dt)
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
                    dt = _SupplementaryPurchaseInvoiceIService.PI_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
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
        public ActionResult GetTDSDetail(string NetAmt, string TDS_data, string doc_id, string Disable, string tax_type)
        {
            try
            {
                string CompID = string.Empty;
                string Br_ID = string.Empty;
                int ValDigit = 0;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["ValDigit"] != null)
                {
                    ValDigit = Convert.ToInt32(Session["ValDigit"]);
                }
                CommonTDS_Model model = new CommonTDS_Model();
                List<TDS_Tmplt_List> tmplt_list = new List<TDS_Tmplt_List>();
                List<TDS_Name_List> tds_list = new List<TDS_Name_List>();
                ViewBag.AssValue = NetAmt;
                Br_ID = Session["BranchId"].ToString();
                DataSet ds = _Common_IServices.Cmn_GetTDSDetail(CompID, Br_ID, doc_id);
                ViewBag.TDS_Data = ToDtblTdsDetail(TDS_data);
                tmplt_list.Add(new TDS_Tmplt_List
                {
                    tmplt_id = "0",
                    tmplt_name = "---Select---",
                });
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmplt_list.Add(new TDS_Tmplt_List
                    {
                        tmplt_id = dr["tmplt_id"].ToString(),
                        tmplt_name = dr["tmplt_name"].ToString(),
                    });
                }
                model.tds_tmplt_list = tmplt_list;
                tds_list.Add(new TDS_Name_List
                {
                    tds_id = "0",
                    tds_name = "---Select---",
                    tds_acc_id = "0"
                });
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    tds_list.Add(new TDS_Name_List
                    {
                        tds_id = dr["tax_id"].ToString(),
                        tds_name = dr["tax_name"].ToString(),
                        tds_acc_id = dr["tax_acc_id"].ToString()
                    });
                }
                model.tds_name_list = tds_list;
                model.Disable = Disable;
                ViewBag.TDS_Details = ds.Tables[0];
                ViewBag.tax_type = tax_type;
                return PartialView("~/Areas/Common/Views/Cmn_TDSCalculation.cshtml", model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private DataTable ToDtblTdsDetail(string tdsDetails)
        {
            try
            {
                DataTable DtblItemtdsDetail = new DataTable();
                DataTable tds_detail = new DataTable();
                tds_detail.Columns.Add("tds_name", typeof(string));
                tds_detail.Columns.Add("tds_id", typeof(string));
                tds_detail.Columns.Add("tds_rate", typeof(string));
                tds_detail.Columns.Add("tds_amt", typeof(string));
                tds_detail.Columns.Add("tds_level", typeof(string));
                tds_detail.Columns.Add("tds_apply_on", typeof(string));
                tds_detail.Columns.Add("tds_apply_on_id", typeof(string));
                tds_detail.Columns.Add("tds_acc_id", typeof(string));
                tds_detail.Columns.Add("tds_base_amt", typeof(string));
                if (tdsDetails != null)
                {
                    JArray jObjecttds = JArray.Parse(tdsDetails);
                    for (int i = 0; i < jObjecttds.Count; i++)
                    {
                        DataRow dtrowtdsDetailsLines = tds_detail.NewRow();
                        dtrowtdsDetailsLines["tds_name"] = jObjecttds[i]["tds_name"].ToString();
                        dtrowtdsDetailsLines["tds_id"] = jObjecttds[i]["tds_id"].ToString();
                        string tds_rate = jObjecttds[i]["tds_rate"].ToString();
                        tds_rate = tds_rate.Replace("%", "");
                        dtrowtdsDetailsLines["tds_rate"] = tds_rate;
                        dtrowtdsDetailsLines["tds_level"] = jObjecttds[i]["tds_level"].ToString();
                        dtrowtdsDetailsLines["tds_amt"] = jObjecttds[i]["tds_amt"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on"] = jObjecttds[i]["tds_apply_on"].ToString();
                        dtrowtdsDetailsLines["tds_apply_on_id"] = jObjecttds[i]["tds_apply_on_id"].ToString();
                        dtrowtdsDetailsLines["tds_acc_id"] = jObjecttds[i]["tds_acc_id"].ToString();
                        dtrowtdsDetailsLines["tds_base_amt"] = jObjecttds[i]["tds_base_amt"].ToString();
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
        private DataTable ToDtblHeaderDetail(SuppPI_ListModel _model)
        {
            try
            {
                DataTable dtheaderdeatil = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("inv_type", typeof(string));
                dtheader.Columns.Add("spinv_no", typeof(string));
                dtheader.Columns.Add("spinv_dt", typeof(DateTime));
                dtheader.Columns.Add("supp_id", typeof(int));
                dtheader.Columns.Add("address", typeof(int));
                dtheader.Columns.Add("bill_no", typeof(string));
                dtheader.Columns.Add("bill_dt", typeof(DateTime));
                dtheader.Columns.Add("curr_id", typeof(int));
                dtheader.Columns.Add("conv_rate", typeof(double));
                dtheader.Columns.Add("inv_no", typeof(string));
                dtheader.Columns.Add("inv_dt", typeof(DateTime));
                dtheader.Columns.Add("inv_status", typeof(string));
                dtheader.Columns.Add("net_val_bs", typeof(double));
                dtheader.Columns.Add("tot_net_amt_diff", typeof(double));
                dtheader.Columns.Add("price_basis", typeof(string));
                dtheader.Columns.Add("freight_type", typeof(string));
                dtheader.Columns.Add("mode_trans", typeof(string));
                dtheader.Columns.Add("dest", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));
                dtheader.Columns.Add("gst_inv_no", typeof(string)); // Nullable
                dtheader.Columns.Add("irn_no", typeof(string)); // Nullable
                dtheader.Columns.Add("gst_status", typeof(string)); // Nullable
                dtheader.Columns.Add("ack_no", typeof(string)); // Nullable
                dtheader.Columns.Add("ewb_no", typeof(string)); // Nullable
                dtheader.Columns.Add("e_inv_status", typeof(string)); // Nullable
                dtheader.Columns.Add("ewb_status", typeof(string)); // Nullable
                dtheader.Columns.Add("einv_no", typeof(string)); // Nullable
                dtheader.Columns.Add("roundoff", typeof(string)); // Nullable
                dtheader.Columns.Add("pm_flag", typeof(string)); // Nullable
                dtheader.Columns.Add("tds_amt", typeof(double));
                dtheader.Columns.Add("rcm_app", typeof(string)); // Nullable
                dtheader.Columns.Add("create_id", typeof(int));
                dtheader.Columns.Add("create_dt", typeof(DateTime));
                dtheader.Columns.Add("mac_id", typeof(string)); // Nullable
                dtheader.Columns.Add("tot_val", typeof(double));
                dtheader.Columns.Add("tot_val_diff", typeof(double));
                dtheader.Columns.Add("tot_val_bs", typeof(double));
                dtheader.Columns.Add("tot_val_bs_diff", typeof(double));
                dtheader.Columns.Add("tot_tax_amt_recov", typeof(double));
                dtheader.Columns.Add("tot_tax_amt_recov_diff", typeof(double));
                dtheader.Columns.Add("tot_tax_amt_nrecov", typeof(double));
                dtheader.Columns.Add("tot_tax_amt_nrecov_diff", typeof(double));
                dtheader.Columns.Add("tds_amt_diff", typeof(double));
                dtheader.Columns.Add("tot_net_amt", typeof(double)); // Nullable
                dtheader.Columns.Add("oc_amt_self", typeof(double));
                dtheader.Columns.Add("oc_amt_self_diff", typeof(double));
                dtheader.Columns.Add("oc_amt_with_tp", typeof(double)); // Nullable
                dtheader.Columns.Add("oc_amt_tp_diff", typeof(double)); // Nullable
                dtheader.Columns.Add("tot_oc_amt", typeof(double)); // Nullable
                dtheader.Columns.Add("tot_oc_amt_diff", typeof(double)); // it does not include tp amounts


                // **Row Initialization**
                DataRow dtrowHeader = dtheader.NewRow();

                // Fill the DataRow with appropriate values (same as before)
                _model.TransType = _model.Inv_Nos != null ? "Update" : "Save";
                dtrowHeader["TransType"] = _model.TransType;
                dtrowHeader["MenuID"] = DocumentMenuId;
                dtrowHeader["Cancelled"] = _model.CancelFlag ? "Y" : "N";
                dtrowHeader["roundoff"] = _model.RoundOffFlag ? "Y" : "N";
                dtrowHeader["pm_flag"] = string.IsNullOrEmpty(_model.pmflagval) ? " " : _model.pmflagval;
                dtrowHeader["inv_type"] = "D";// _model.OrderType;
                dtrowHeader["comp_id"] = Convert.ToInt32(Session["CompId"]);
                dtrowHeader["br_id"] = Convert.ToInt32(Session["BranchId"]);
                dtrowHeader["inv_no"] = _model.Purchaseinv_no;
                dtrowHeader["inv_dt"] = _model.Inv_Dt1;
                dtrowHeader["spinv_no"] = _model.Inv_Nos;
                dtrowHeader["spinv_dt"] = _model.SuppPI_inv_dt;
                dtrowHeader["supp_id"] = _model.SuppPI_SuppID;
                dtrowHeader["bill_no"] = _model.bill_no;
                dtrowHeader["bill_dt"] = _model.bill_date;
                dtrowHeader["curr_id"] = _model.curr_id;
                dtrowHeader["conv_rate"] = _model.conv_rate;
                dtrowHeader["address"] = _model.bill_add_id;
                dtrowHeader["create_id"] = Convert.ToInt32(Session["UserId"]);
                dtrowHeader["create_dt"] = _model.Inv_Dt1;
                dtrowHeader["inv_status"] = string.IsNullOrEmpty(_model.AppStatus) ? "D" : _model.AppStatus;

                // **Populate Missing Values**
                dtrowHeader["gst_inv_no"] = "";  // Example: update with real value
                dtrowHeader["irn_no"] = "";  // Example: update with real value
                dtrowHeader["gst_status"] = "";  // Example: update with real value
                dtrowHeader["ack_no"] = "";  // Example: update with real value
                dtrowHeader["ewb_no"] = _model.EWBNNumber;  // Example: update with real value
                dtrowHeader["e_inv_status"] = ""; // Example: update with real value
                dtrowHeader["ewb_status"] = "";  // Example: update with real value
                dtrowHeader["einv_no"] = _model.EInvoive;   // Example: update with real value

                // Continue with the rest of the fields
                dtrowHeader["net_val_bs"] = _model.NetAmountInBase;
                dtrowHeader["remarks"] = _model.remarks;
                dtrowHeader["price_basis"] = _model.PriceBasis;
                dtrowHeader["freight_type"] = _model.FreightType;
                dtrowHeader["mode_trans"] = _model.ModeOfTransport;
                dtrowHeader["dest"] = _model.Destination;
                dtrowHeader["rcm_app"] = _model.RCMApplicable ? "Y" : "N";
                dtrowHeader["tds_amt"] = _model.TDS_Amount;

                dtrowHeader["tot_val"] = _model.GrossValue;
                dtrowHeader["tot_val_diff"] = _model.TxtDiff_TotalAmountInBase;
                dtrowHeader["tot_val_bs"] = _model.GrossValueInBase;
                dtrowHeader["tot_val_bs_diff"] = _model.TxtDiff_TotalAmountInBase;
                dtrowHeader["tot_tax_amt_recov"] = _model.TaxAmountRecoverable;
                dtrowHeader["tot_tax_amt_recov_diff"] = _model.TxtDiff_TotalTaxAmountRec;
                dtrowHeader["tot_tax_amt_nrecov"] = _model.TaxAmountNonRecoverable;
                dtrowHeader["tot_tax_amt_nrecov_diff"] = _model.TxtDiff_TotalTaxAmountNonRec;
                dtrowHeader["oc_amt_self"] = _model.DocSuppOtherCharges;
                dtrowHeader["oc_amt_self_diff"] = _model.TxtDiff_TotalOCTPAmount ?? "0";
                dtrowHeader["oc_amt_with_tp"] = string.IsNullOrEmpty(_model.DocSuppOtherChargesTP) ? (object)DBNull.Value : Convert.ToDouble(_model.DocSuppOtherChargesTP);
                dtrowHeader["oc_amt_tp_diff"] = _model.OtherCharges;
                dtrowHeader["tds_amt_diff"] = _model.TxtDiff_TotalTDSAmount;
                dtrowHeader["tot_net_amt"] = _model.NetAmountInBase;
                dtrowHeader["tot_net_amt_diff"] = _model.TxtDiff_Amount;
                dtrowHeader["tot_oc_amt"] = _model.OtherCharges;
                dtrowHeader["tot_oc_amt_diff"] = _model.TxtDiff_TotalOCAmount;

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
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
        //private DataTable ToDtblItemDetail(SuppPI_ListModel _model)
        private DataTable ToDtblItemDetail(string ItemDetails)
        {
            try
            {
                DataTable dtItem = new DataTable();
                // Columns matching SPINV_ItemDetails table type
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("inv_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("gl_vou_no", typeof(string));
                dtItem.Columns.Add("gl_vou_dt", typeof(DateTime));
                dtItem.Columns.Add("tax_expted", typeof(string));
                dtItem.Columns.Add("hsn_code", typeof(string));
                dtItem.Columns.Add("manual_gst", typeof(string));
                dtItem.Columns.Add("value_bs", typeof(string));
                dtItem.Columns.Add("claim_itc", typeof(string));
                dtItem.Columns.Add("item_acc_id", typeof(int));
                dtItem.Columns.Add("item_gr_val_bs", typeof(string));
                dtItem.Columns.Add("item_gr_val_bs_diff", typeof(string));
                dtItem.Columns.Add("item_gr_val_sp", typeof(string));
                dtItem.Columns.Add("item_gr_val_sp_diff", typeof(string));
                dtItem.Columns.Add("item_tax_amt", typeof(string));
                dtItem.Columns.Add("item_tax_amt_diff", typeof(string));
                dtItem.Columns.Add("item_tax_recov", typeof(string));
                dtItem.Columns.Add("item_tax_recov_diff", typeof(string));
                dtItem.Columns.Add("item_tax_nrecov", typeof(string));
                dtItem.Columns.Add("item_tax_nrecov_diff", typeof(string));
                dtItem.Columns.Add("item_oc_amt_self", typeof(string));
                dtItem.Columns.Add("item_oc_amt_self_diff", typeof(string));
                dtItem.Columns.Add("item_oc_amt_tp", typeof(string));
                dtItem.Columns.Add("item_net_val_sp", typeof(string));
                dtItem.Columns.Add("item_net_val_sp_diff", typeof(string));
                dtItem.Columns.Add("item_net_val_bs", typeof(string));
                dtItem.Columns.Add("item_net_val_bs_diff", typeof(string));
                dtItem.Columns.Add("item_landed_cost", typeof(string));
                dtItem.Columns.Add("item_landed_cost_diff", typeof(string));
                dtItem.Columns.Add("item_oc_amt_tp_diff", typeof(string));
                dtItem.Columns.Add("item_oc_amt_tp_prv", typeof(string));
                if (ItemDetails != null)
                {
                    JArray jObject = JArray.Parse(ItemDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        string itemId = jObject[i]["item_id"]?.ToString();
                        string Item = null;

                        if (!string.IsNullOrEmpty(itemId) && (itemId.EndsWith("P") || itemId.EndsWith("R")))
                        {
                            Item = itemId.Substring(0, itemId.Length - 1);
                        }
                        if (itemId.EndsWith("R"))
                        {
                            dtrowLines["item_id"] = Item;// jObject[i]["item_id"]?.ToString();
                            dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"] ?? "0");
                            dtrowLines["inv_qty"] = Convert.ToDouble(jObject[i]["inv_qty"] ?? "0");
                            dtrowLines["item_rate"] = jObject[i]["item_rate"] ?? "0";// Convert.ToDouble(jObject[i]["item_rate"] ?? "0");
                            dtrowLines["gl_vou_no"] = jObject[i]["gl_vou_no"]?.ToString();
                            dtrowLines["gl_vou_dt"] = string.IsNullOrEmpty(jObject[i]["gl_vou_dt"]?.ToString()) ? (object)DBNull.Value : Convert.ToDateTime(jObject[i]["gl_vou_dt"]);
                            dtrowLines["tax_expted"] = jObject[i]["TaxExempted"]?.ToString();
                            dtrowLines["hsn_code"] = jObject[i]["hsn_code"]?.ToString();
                            dtrowLines["manual_gst"] = jObject[i]["ManualGST"]?.ToString();
                            dtrowLines["value_bs"] = Convert.ToDouble(jObject[i]["value_bs"] ?? "0");
                            dtrowLines["claim_itc"] = jObject[i]["ClaimITC"]?.ToString();
                            dtrowLines["item_acc_id"] = string.IsNullOrEmpty(jObject[i]["item_acc_id"]?.ToString()) ? (object)DBNull.Value : Convert.ToInt32(jObject[i]["item_acc_id"]);
                            dtrowLines["item_gr_val_bs"] = Convert.ToDouble(jObject[i]["item_gr_val_bs"] ?? "0");
                            dtrowLines["item_gr_val_bs_diff"] = Convert.ToDouble(jObject[i]["item_gr_val_bs_diff"] ?? "0");
                            dtrowLines["item_gr_val_sp"] = Convert.ToDouble(jObject[i]["item_gr_val_sp"] ?? "0");
                            dtrowLines["item_gr_val_sp_diff"] = Convert.ToDouble(jObject[i]["item_gr_val_sp_diff"] ?? "0");
                            dtrowLines["item_tax_amt"] = Convert.ToDouble(jObject[i]["item_tax_amt"] ?? "0");
                            dtrowLines["item_tax_amt_diff"] = Convert.ToDouble(jObject[i]["item_tax_amt_diff"] ?? "0");
                            dtrowLines["item_tax_recov"] = Convert.ToDouble(jObject[i]["item_tax_recov"] ?? "0");
                            dtrowLines["item_tax_recov_diff"] = Convert.ToDouble(jObject[i]["item_tax_recov_diff"] ?? "0");
                            dtrowLines["item_tax_nrecov"] = Convert.ToDouble(jObject[i]["item_tax_nrecov"] ?? "0");
                            dtrowLines["item_tax_nrecov_diff"] = Convert.ToDouble(jObject[i]["item_tax_nrecov_diff"] ?? "0");
                            dtrowLines["item_oc_amt_self"] = Convert.ToDouble(jObject[i]["item_oc_amt_self"] ?? "0");
                            dtrowLines["item_oc_amt_self_diff"] = Convert.ToDouble(jObject[i]["item_oc_amt_self_diff"] ?? "0");
                            dtrowLines["item_oc_amt_tp"] = Convert.ToDouble(jObject[i]["item_oc_amt_tp"] ?? "0");
                            dtrowLines["item_oc_amt_tp_diff"] = Convert.ToDouble(jObject[i]["item_oc_amt_tp_diff"] ?? "0");
                            dtrowLines["item_net_val_sp"] = Convert.ToDouble(jObject[i]["item_net_val_sp"] ?? "0");
                            dtrowLines["item_net_val_sp_diff"] = Convert.ToDouble(jObject[i]["item_net_val_sp_diff"] ?? "0");
                            dtrowLines["item_net_val_bs"] = Convert.ToDouble(jObject[i]["item_net_val_bs"] ?? "0");
                            dtrowLines["item_net_val_bs_diff"] = Convert.ToDouble(jObject[i]["item_net_val_bs_diff"] ?? "0");
                            dtrowLines["item_landed_cost"] = Convert.ToDouble(jObject[i]["item_landed_cost"] ?? "0");
                            dtrowLines["item_landed_cost_diff"] = Convert.ToDouble(jObject[i]["item_landed_cost_diff"] ?? "0");
                            dtrowLines["item_oc_amt_tp_prv"] = Convert.ToDouble(jObject[i]["item_oc_amt_tp_prv"] ?? "0");

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
                throw;
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
                Tax_detail.Columns.Add("oc_id", typeof(string));
                Tax_detail.Columns.Add("supp_id", typeof(string));

                HashSet<string> uniqueTaxKeys = new HashSet<string>();

                if (TaxDetails != null)
                {
                    JArray jObjectTax = JArray.Parse(TaxDetails);
                    for (int i = 0; i < jObjectTax.Count; i++)
                    {
                        string itemId = jObjectTax[i]["item_id"]?.ToString();
                        if (itemId.EndsWith("R"))
                        {
                            if (!string.IsNullOrEmpty(itemId) && (itemId.EndsWith("P") || itemId.EndsWith("R")))
                            {
                                itemId = itemId.Substring(0, itemId.Length - 1);
                            }

                            string tax_id = jObjectTax[i]["tax_id"]?.ToString();
                            string tax_rate = jObjectTax[i]["tax_rate"]?.ToString().Replace("%", "");
                            string tax_val = jObjectTax[i]["tax_val"]?.ToString();
                            string tax_level = jObjectTax[i]["tax_level"]?.ToString();
                            string tax_apply_on = jObjectTax[i]["tax_apply_on"]?.ToString();
                            string tax_recov = recov == "Y" ? jObjectTax[i]["tax_recov"]?.ToString() : "";
                            //string tax_ocid = jObjectTax[i]["item_id"].ToString();

                            // Generate unique key
                            string key = string.Join("|", itemId, tax_id, tax_rate, tax_val, tax_level, tax_apply_on, tax_recov);

                            // Check uniqueness
                            if (!uniqueTaxKeys.Contains(key))
                            {
                                uniqueTaxKeys.Add(key);

                                DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();
                                dtrowTaxDetailsLines["item_id"] = itemId;
                                dtrowTaxDetailsLines["tax_id"] = tax_id;
                                dtrowTaxDetailsLines["tax_rate"] = tax_rate;
                                dtrowTaxDetailsLines["tax_val"] = tax_val;
                                dtrowTaxDetailsLines["tax_level"] = tax_level;
                                dtrowTaxDetailsLines["tax_apply_on"] = tax_apply_on;
                                dtrowTaxDetailsLines["tax_recov"] = tax_recov;
                                dtrowTaxDetailsLines["supp_id"] = "";
                                dtrowTaxDetailsLines["oc_id"] = "";
                                Tax_detail.Rows.Add(dtrowTaxDetailsLines);
                            }
                        }
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
                throw;
            }
        }
        private DataTable ToDtblTaxOCDetail(string TaxDetails, string recov)
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
                Tax_detail.Columns.Add("oc_id", typeof(string));
                Tax_detail.Columns.Add("supp_id", typeof(string));

                HashSet<string> uniqueTaxKeys = new HashSet<string>();

                if (TaxDetails != null)
                {
                    JArray jObjectTax = JArray.Parse(TaxDetails);
                    for (int i = 0; i < jObjectTax.Count; i++)
                    {
                        string ocfor = jObjectTax[i]["ocfor"]?.ToString();
                        if (ocfor.EndsWith("R"))
                        {
                            string itemId = jObjectTax[i]["item_id"]?.ToString();
                            string tax_id = jObjectTax[i]["tax_id"]?.ToString();
                            string tax_rate = jObjectTax[i]["tax_rate"]?.ToString().Replace("%", "");
                            string tax_val = jObjectTax[i]["tax_val"]?.ToString();
                            string tax_level = jObjectTax[i]["tax_level"]?.ToString();
                            string tax_apply_on = jObjectTax[i]["tax_apply_on"]?.ToString();
                            string tax_recov = recov == "Y" ? jObjectTax[i]["tax_recov"]?.ToString() : "";
                            //string tax_ocid = jObjectTax[i]["item_id"].ToString();

                            // Generate unique key
                            string key = string.Join("|", itemId, tax_id, tax_rate, tax_val, tax_level, tax_apply_on, tax_recov);

                            // Check uniqueness
                            if (!uniqueTaxKeys.Contains(key))
                            {
                                uniqueTaxKeys.Add(key);

                                DataRow dtrowTaxDetailsLines = Tax_detail.NewRow();
                                dtrowTaxDetailsLines["item_id"] = itemId;
                                dtrowTaxDetailsLines["tax_id"] = tax_id;
                                dtrowTaxDetailsLines["tax_rate"] = tax_rate;
                                dtrowTaxDetailsLines["tax_val"] = tax_val;
                                dtrowTaxDetailsLines["tax_level"] = tax_level;
                                dtrowTaxDetailsLines["tax_apply_on"] = tax_apply_on;
                                dtrowTaxDetailsLines["tax_recov"] = tax_recov;
                                dtrowTaxDetailsLines["supp_id"] = "";
                                dtrowTaxDetailsLines["oc_id"] = "";
                                Tax_detail.Rows.Add(dtrowTaxDetailsLines);
                            }
                        }
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
                throw;
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
                        //if (i % 2 == 0)
                        //{
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
                        //else
                        //{
                        //    dtrowtdsDetailsLines["oc_id"] = "";
                        //    dtrowtdsDetailsLines["supp_id"] = "";
                        //}
                        dtrowtdsDetailsLines["tds_base_amt"] = jObjecttds[i]["Tds_totalAmnt"].ToString();
                        dtrowtdsDetailsLines["tds_ass_apply_on"] = jObjecttds[i]["Tds_AssValApplyOn"].ToString();
                        tds_detail.Rows.Add(dtrowtdsDetailsLines);
                        //}
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
                        var OcFor = jObjectOC[i]["OcFor"].ToString();
                        if (OcFor == "R")
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
        private DataTable ToDtblAttachmentDetail(SuppPI_ListModel _model)
        {
            try
            {
                string PageName = _model.Title.Replace(" ", "");
                DataTable dtAttachment = new DataTable();
                var _DirectPurchaseInvoiceattch = TempData["ModelDataattch"] as SuppPurchaseInvoiceattch;
                
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
                            if (!string.IsNullOrEmpty(_model.Inv_Nos))
                            {
                                dtrowAttachment1["id"] = _model.Inv_Nos;
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
                            if (!string.IsNullOrEmpty(_model.Inv_Nos))
                            {
                                ItmCode = _model.Inv_Nos;
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
                        dtrowItemdetails["inv_qty"] = jObject2[i]["qty"].ToString();
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
                dtrowItemdetails["inv_qty"] = jObject2[i]["qty"].ToString();
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
                string itemId = jObjectTax[i]["item_id"]?.ToString();
                if (!string.IsNullOrEmpty(itemId) && (itemId.EndsWith("P") || itemId.EndsWith("R")))
                {
                    itemId = itemId.Substring(0, itemId.Length - 1);
                }
                dtrowTaxDetailsLines["item_id"] = itemId;// jObject[i]["item_id"]?.ToString();
                                                         // dtrowTaxDetailsLines["item_id"] = jObjectTax[i]["item_id"].ToString();
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
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                SuppPurchaseInvoiceattch _DirectPurchaseInvoiceattch = new SuppPurchaseInvoiceattch();
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
        public string SavePdfDocToSendOnEmailAlert(string docid, string Inv_no, string Inv_Date, string fileName)
        {
            try
            {
                DataTable dt = new DataTable();
                var commonCont = new CommonController(_Common_IServices);
                var data = GetPdfData(docid, Inv_no, Inv_Date);
                return commonCont.SaveAlertDocument(data, fileName);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
        }
    }
}