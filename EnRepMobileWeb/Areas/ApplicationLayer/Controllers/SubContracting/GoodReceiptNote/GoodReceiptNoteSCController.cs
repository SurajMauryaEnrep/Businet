using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.SubContracting.GoodReceiptNote;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.SubContracting.GoodReceiptNote;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.SubContracting.GoodReceiptNote
{
    public class GoodReceiptNoteSCController : Controller
    {
        string CompID, BrchID, UserID, language = String.Empty;
        string DocumentMenuId = "105108120", title;
        List<GoodsReceiptNoteSCList> _GoodsReceiptNoteSCList;
      
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        GoodReceiptNoteSC_IServices _GoodReceiptNoteSC_IServices;
        CommonController cmn = new CommonController();
        public GoodReceiptNoteSCController(Common_IServices _Common_IServices, GoodReceiptNoteSC_IServices _GoodReceiptNoteSC_IServices)
        {
            this._Common_IServices = _Common_IServices;
            this._GoodReceiptNoteSC_IServices = _GoodReceiptNoteSC_IServices;
        }
        // GET: ApplicationLayer/GoodReceiptNoteSC
        public ActionResult GoodReceiptNoteSC(string WF_Status)
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
                GRNListModel _GRNListModel = new GRNListModel();
                if (WF_Status != null && WF_Status != "")
                {
                    _GRNListModel.WF_Status = WF_Status;
                }
                DateTime dtnow = DateTime.Now;
                string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");



                GetAutoCompleteSearchSuppList(_GRNListModel);
                CommonPageDetails();
                _GRNListModel.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _GRNListModel.StatusList = statusLists;
                if (TempData["ListFilterData"] != null)
                {
                    if (TempData["ListFilterData"].ToString() != "")
                    {
                        var PRData = TempData["ListFilterData"].ToString();
                        var a = PRData.Split(',');
                        _GRNListModel.SuppID = a[0].Trim();
                        _GRNListModel.FromDate = a[1].Trim();
                        _GRNListModel.ToDate = a[2].Trim();
                        _GRNListModel.Status = a[3].Trim();
                        if (_GRNListModel.Status == "0")
                        {
                            _GRNListModel.Status = null;
                        }
                        _GRNListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    }
                }
                else
                {
                    _GRNListModel.FromDate = startDate;
                }
                _GRNListModel.GRNSCList = getGRNSCList(_GRNListModel);


                _GRNListModel.Title = title;
                //Session["GRNSCSearch"] = "0";
                _GRNListModel.GRNSCSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/SubContracting/GoodReceiptNote/GoodReceiptNoteListSC.cshtml", _GRNListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AddGoodReceiptNoteSCDetail()
        {
            GoodReceiptNoteSCModel _GoodReceiptNoteSCModel = new GoodReceiptNoteSCModel();
            _GoodReceiptNoteSCModel.Message = "New";
            _GoodReceiptNoteSCModel.Command = "Add";
            _GoodReceiptNoteSCModel.AppStatus = "D";
            ViewBag.DocumentStatus = _GoodReceiptNoteSCModel.AppStatus;
            _GoodReceiptNoteSCModel.TransType = "Save";
            _GoodReceiptNoteSCModel.BtnName = "BtnAddNew";
            _GoodReceiptNoteSCModel.Show = "show";
            TempData["ModelData"] = _GoodReceiptNoteSCModel;
            
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
                return RedirectToAction("GoodReceiptNoteSC");
            }
            /*End to chk Financial year exist or not*/
            CommonPageDetails();
            return RedirectToAction("GoodReceiptNoteSCDetail", "GoodReceiptNoteSC");
        }
        public ActionResult GoodReceiptNoteSCDetail(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel1, string GRNCodeURL, string GRNDate, string TransType, string BtnName, string command,string WF_Status1)
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
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                CommonPageDetails();
                /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, GRNDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _GoodReceiptNoteSCModel = TempData["ModelData"] as GoodReceiptNoteSCModel;
                if (_GoodReceiptNoteSCModel != null)
                {                    
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _GoodReceiptNoteSCModel.Title = title;
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                   if(WF_Status1 != null &&WF_Status1 != "")
                    {
                        _GoodReceiptNoteSCModel.WF_Status1 = WF_Status1;
                    }
                    _GoodReceiptNoteSCModel.Title = title;
                    _GoodReceiptNoteSCModel.ValDigit = ValDigit;
                    _GoodReceiptNoteSCModel.QtyDigit = QtyDigit;
                    _GoodReceiptNoteSCModel.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;

                    List<SupplierName> suppLists = new List<SupplierName>();
                    suppLists.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _GoodReceiptNoteSCModel.SupplierNameList = suppLists;

                    List<DeliveryNoteNo> jobOrd_NoLists = new List<DeliveryNoteNo>();
                    jobOrd_NoLists.Add(new DeliveryNoteNo { DNnoId = "0", DNnoVal = "---Select---" });
                    _GoodReceiptNoteSCModel.DeliveryNoteNoList = jobOrd_NoLists;

                    DataTable dt = new DataTable();
                    List<Warehouse> WHLists = new List<Warehouse>();
                    dt = GetWarehouseList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Warehouse WarehouseList = new Warehouse();
                        WarehouseList.wh_id = dr["wh_id"].ToString();
                        WarehouseList.wh_id_scrap = dr["wh_id"].ToString();
                        WarehouseList.wh_name = dr["wh_name"].ToString();
                        WHLists.Add(WarehouseList);
                    }
                    WHLists.Insert(0, new Warehouse() { wh_id = "0", wh_id_scrap="0", wh_name = "---Select---" });
                    _GoodReceiptNoteSCModel.WarehouseList = WHLists;

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _GoodReceiptNoteSCModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    if (_GoodReceiptNoteSCModel.TransType == "Update" || _GoodReceiptNoteSCModel.Command == "Edit")
                    {
                        string GRNSC_NO = _GoodReceiptNoteSCModel.GRNNumber;
                        string GRNSC_Date = _GoodReceiptNoteSCModel.GRNDate;
                        DataSet ds = _GoodReceiptNoteSC_IServices.GetGRNDetailEditUpdate(CompID, BrchID, GRNSC_NO, GRNSC_Date, UserID, DocumentMenuId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            _GoodReceiptNoteSCModel.JobOrdTyp = ds.Tables[13].Rows[0]["JobOrdTyp"].ToString();
                            if (_GoodReceiptNoteSCModel.JobOrdTyp == "D")
                            {
                                _GoodReceiptNoteSCModel.JobOrdTyp = "Direct";
                            }
                            _GoodReceiptNoteSCModel.GRNNumber = ds.Tables[0].Rows[0]["mr_no"].ToString();
                            _GoodReceiptNoteSCModel.DocNoAttach = ds.Tables[0].Rows[0]["mr_no"].ToString();
                            _GoodReceiptNoteSCModel.GRNDate = ds.Tables[0].Rows[0]["MrDate"].ToString();
                            _GoodReceiptNoteSCModel.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _GoodReceiptNoteSCModel.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                             suppLists.Add(new SupplierName { supp_id = _GoodReceiptNoteSCModel.SuppID, supp_name = _GoodReceiptNoteSCModel.SuppName });
                            _GoodReceiptNoteSCModel.SupplierNameList = suppLists;
                            _GoodReceiptNoteSCModel.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _GoodReceiptNoteSCModel.Bill_Dt = ds.Tables[0].Rows[0]["BillDate"].ToString();
                            _GoodReceiptNoteSCModel.DeliveryNoteNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            _GoodReceiptNoteSCModel.DeliveryNote_Number = ds.Tables[0].Rows[0]["src_doc_no"].ToString();

                            jobOrd_NoLists.Add(new DeliveryNoteNo { DNnoId = _GoodReceiptNoteSCModel.DeliveryNoteNumber, DNnoVal = _GoodReceiptNoteSCModel.DeliveryNoteNumber });
                            _GoodReceiptNoteSCModel.DeliveryNoteNoList = jobOrd_NoLists;
                            if (ds.Tables[0].Rows[0]["DocDate"] != null && ds.Tables[0].Rows[0]["DocDate"].ToString() != "")
                            {
                                _GoodReceiptNoteSCModel.DeliveryNoteDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DocDate"]).ToString("yyyy-MM-dd");
                            }
                            _GoodReceiptNoteSCModel.FinishProduct = ds.Tables[0].Rows[0]["FItem"].ToString();
                            _GoodReceiptNoteSCModel.FinishProductId = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _GoodReceiptNoteSCModel.FinishUom = ds.Tables[0].Rows[0]["FUOM"].ToString();
                            _GoodReceiptNoteSCModel.FinishUomId = ds.Tables[0].Rows[0]["fg_uom_id"].ToString();

                            _GoodReceiptNoteSCModel.wh_id = ds.Tables[1].Rows[0]["wh_id"].ToString();
                            _GoodReceiptNoteSCModel.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _GoodReceiptNoteSCModel.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _GoodReceiptNoteSCModel.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _GoodReceiptNoteSCModel.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _GoodReceiptNoteSCModel.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _GoodReceiptNoteSCModel.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _GoodReceiptNoteSCModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _GoodReceiptNoteSCModel.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                            _GoodReceiptNoteSCModel.GRNStatus = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                            _GoodReceiptNoteSCModel.GrossValue = ds.Tables[0].Rows[0]["tot_serv_chrg"].ToString();
                            _GoodReceiptNoteSCModel.TaxAmountRecoverable = ds.Tables[0].Rows[0]["tax_amt_recov"].ToString();
                            _GoodReceiptNoteSCModel.TaxAmountNonRecoverable = ds.Tables[0].Rows[0]["tax_amt_nrecov"].ToString();
                            _GoodReceiptNoteSCModel.OtherCharges = ds.Tables[0].Rows[0]["oc_amt"].ToString();
                            _GoodReceiptNoteSCModel.NetMRValue = ds.Tables[0].Rows[0]["net_serv_chrg"].ToString();
                            _GoodReceiptNoteSCModel.ConsumptionValue = ds.Tables[0].Rows[0]["cons_val"].ToString();
                            _GoodReceiptNoteSCModel.NetLandedValue = ds.Tables[0].Rows[0]["landed_val"].ToString();
                            _GoodReceiptNoteSCModel.ewb_no = ds.Tables[0].Rows[0]["ewb_no"].ToString();

                            _GoodReceiptNoteSCModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _GoodReceiptNoteSCModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);//Cancelled

                            ViewBag.ItemDetails = ds.Tables[1];
                            _GoodReceiptNoteSCModel.BatchDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                            _GoodReceiptNoteSCModel.SerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                            ViewBag.AttechmentDetails = ds.Tables[7];
                            ViewBag.ConsumeItemDetails = ds.Tables[8];
                            ViewBag.ItemStockBatchWise = ds.Tables[9];
                            ViewBag.ScrapItemDetails = ds.Tables[10];
                            ViewBag.SubItemDetails = ds.Tables[12];
                            //if(_GoodReceiptNoteSCModel.GRNStatus=="PC" && _GoodReceiptNoteSCModel.Command == "Edit")
                            //{
                            //    ViewBag.CostingDetails = ds.Tables[14];
                            //    _GoodReceiptNoteSCModel.ConvRate = ds.Tables[14].Rows[0]["conv_rate"].ToString().Trim();
                            //    _GoodReceiptNoteSCModel.CurrId = ds.Tables[14].Rows[0]["curr_id"].ToString().Trim();
                            //}
                            if (ds.Tables[14].Rows.Count > 0)
                            {
                                ViewBag.CostingDetails = ds.Tables[14];
                                _GoodReceiptNoteSCModel.ConvRate = ds.Tables[14].Rows[0]["conv_rate"].ToString().Trim();
                                _GoodReceiptNoteSCModel.CurrId = ds.Tables[14].Rows[0]["curr_id"].ToString().Trim();
                            }
                            if (_GoodReceiptNoteSCModel.GRNStatus != "PC")
                            {
                                _GoodReceiptNoteSCModel.Show = "show";
                            }
                            ViewBag.ItemTaxDetails = ds.Tables[15];
                            ViewBag.OtherChargeDetails = ds.Tables[16];
                            ViewBag.OCTaxDetails = ds.Tables[17];
                            _GoodReceiptNoteSCModel.ScrapItemBatchWiseDetail = DataTableToJSONWithStringBuilder(ds.Tables[11]);

                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _GoodReceiptNoteSCModel.Cancelled = true;
                        }
                        else
                        {
                            _GoodReceiptNoteSCModel.Cancelled = false;
                        }

                        _GoodReceiptNoteSCModel.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }

                        if (ViewBag.AppLevel != null && _GoodReceiptNoteSCModel.Command != "Edit")
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

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _GoodReceiptNoteSCModel.BtnName = "Refresh";
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
                                        _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
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
                                        _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";

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
                                    _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
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
                                        _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
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
                                    _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
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
                                    _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _GoodReceiptNoteSCModel.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                    }
                    else
                    {
                        _GoodReceiptNoteSCModel.Show = "show";
                    }

                    if (_GoodReceiptNoteSCModel.BtnName != null)
                    {
                        _GoodReceiptNoteSCModel.BtnName = _GoodReceiptNoteSCModel.BtnName;
                    }
                    _GoodReceiptNoteSCModel.TransType = _GoodReceiptNoteSCModel.TransType;
                    ViewBag.TransType = _GoodReceiptNoteSCModel.TransType;


                    if (_GoodReceiptNoteSCModel.DocumentStatus == null)
                    {
                        _GoodReceiptNoteSCModel.DocumentStatus = "D";
                        ViewBag.DocumentCode = "D";
                        ViewBag.Command = _GoodReceiptNoteSCModel.Command;
                    }
                    else
                    {
                        _GoodReceiptNoteSCModel.DocumentStatus = _GoodReceiptNoteSCModel.DocumentStatus;
                        ViewBag.DocumentCode = _GoodReceiptNoteSCModel.DocumentStatus;
                        ViewBag.Command = _GoodReceiptNoteSCModel.Command;
                    }
                    ViewBag.DocumentStatus = _GoodReceiptNoteSCModel.DocumentStatus;
                    ViewBag.DocumentCode = _GoodReceiptNoteSCModel.DocumentStatus;
                   

                    return View("~/Areas/ApplicationLayer/Views/SubContracting/GoodReceiptNote/GoodReceiptNoteDetailSC.cshtml", _GoodReceiptNoteSCModel);
                }
               
                else
                {/*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/              
                    ViewBag.DocumentMenuId = DocumentMenuId;

                    string ValDigit1 = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit1 = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit1 = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _GoodReceiptNoteSCModel1.ValDigit = ValDigit1;
                    _GoodReceiptNoteSCModel1.QtyDigit = QtyDigit1;
                    _GoodReceiptNoteSCModel1.RateDigit = RateDigit1;
                    ViewBag.ValDigit = ValDigit1;
                    ViewBag.QtyDigit = QtyDigit1;
                    ViewBag.RateDigit = RateDigit1;
                    ViewBag.DocumentStatus = "D";
                    if (WF_Status1 != null && WF_Status1 != "")
                    {
                        _GoodReceiptNoteSCModel1.WF_Status1 = WF_Status1;
                    }
                    List<SupplierName> suppLists1 = new List<SupplierName>();
                    suppLists1.Add(new SupplierName { supp_id = "0", supp_name = "---Select---" });
                    _GoodReceiptNoteSCModel1.SupplierNameList = suppLists1;

                    List<DeliveryNoteNo> jobOrd_NoLists1 = new List<DeliveryNoteNo>();
                    jobOrd_NoLists1.Add(new DeliveryNoteNo { DNnoId = "0", DNnoVal = "---Select---" });
                    _GoodReceiptNoteSCModel1.DeliveryNoteNoList = jobOrd_NoLists1;

                    DataTable dt = new DataTable();
                    List<Warehouse> WHLists1 = new List<Warehouse>();
                    dt = GetWarehouseList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Warehouse WarehouseList = new Warehouse();
                        WarehouseList.wh_id = dr["wh_id"].ToString();
                        WarehouseList.wh_id_scrap = dr["wh_id"].ToString();
                        WarehouseList.wh_name = dr["wh_name"].ToString();
                        WHLists1.Add(WarehouseList);
                    }
                    WHLists1.Insert(0, new Warehouse() { wh_id = "0", wh_id_scrap="0", wh_name = "---Select---" });
                    _GoodReceiptNoteSCModel1.WarehouseList = WHLists1;

                    if (_GoodReceiptNoteSCModel1.TransType == "Update" || _GoodReceiptNoteSCModel1.Command == "Edit")
                    {
                       
                        string GRNSC_NO = GRNCodeURL;
                        string GRNSC_Date = GRNDate;
                        DataSet ds = _GoodReceiptNoteSC_IServices.GetGRNDetailEditUpdate(CompID, BrchID, GRNSC_NO, GRNSC_Date, UserID, DocumentMenuId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            _GoodReceiptNoteSCModel1.JobOrdTyp = ds.Tables[13].Rows[0]["JobOrdTyp"].ToString();
                            if (_GoodReceiptNoteSCModel1.JobOrdTyp == "D")
                            {
                                _GoodReceiptNoteSCModel1.JobOrdTyp = "Direct";
                            }
                            _GoodReceiptNoteSCModel1.GRNNumber = ds.Tables[0].Rows[0]["mr_no"].ToString();
                            _GoodReceiptNoteSCModel1.DocNoAttach = ds.Tables[0].Rows[0]["mr_no"].ToString();
                            _GoodReceiptNoteSCModel1.GRNDate = ds.Tables[0].Rows[0]["MrDate"].ToString();
                            _GoodReceiptNoteSCModel1.SuppName = ds.Tables[0].Rows[0]["supp_name"].ToString();
                            _GoodReceiptNoteSCModel1.SuppID = ds.Tables[0].Rows[0]["supp_id"].ToString();
                            suppLists1.Add(new SupplierName { supp_id = _GoodReceiptNoteSCModel1.SuppID, supp_name = _GoodReceiptNoteSCModel1.SuppName });
                            _GoodReceiptNoteSCModel1.SupplierNameList = suppLists1;
                            _GoodReceiptNoteSCModel1.Bill_No = ds.Tables[0].Rows[0]["bill_no"].ToString();
                            _GoodReceiptNoteSCModel1.Bill_Dt = ds.Tables[0].Rows[0]["BillDate"].ToString();
                            _GoodReceiptNoteSCModel1.DeliveryNoteNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            _GoodReceiptNoteSCModel1.DeliveryNote_Number = ds.Tables[0].Rows[0]["src_doc_no"].ToString();

                            jobOrd_NoLists1.Add(new DeliveryNoteNo { DNnoId = _GoodReceiptNoteSCModel1.DeliveryNoteNumber, DNnoVal = _GoodReceiptNoteSCModel1.DeliveryNoteNumber });
                            _GoodReceiptNoteSCModel1.DeliveryNoteNoList = jobOrd_NoLists1;
                            if (ds.Tables[0].Rows[0]["DocDate"] != null && ds.Tables[0].Rows[0]["DocDate"].ToString() != "")
                            {
                                _GoodReceiptNoteSCModel1.DeliveryNoteDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DocDate"]).ToString("yyyy-MM-dd");
                            }
                            _GoodReceiptNoteSCModel1.FinishProduct = ds.Tables[0].Rows[0]["FItem"].ToString();
                            _GoodReceiptNoteSCModel1.FinishProductId = ds.Tables[0].Rows[0]["fg_product_id"].ToString();
                            _GoodReceiptNoteSCModel1.FinishUom = ds.Tables[0].Rows[0]["FUOM"].ToString();
                            _GoodReceiptNoteSCModel1.FinishUomId = ds.Tables[0].Rows[0]["fg_uom_id"].ToString();

                            _GoodReceiptNoteSCModel1.wh_id = ds.Tables[1].Rows[0]["wh_id"].ToString();
                            _GoodReceiptNoteSCModel1.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                            _GoodReceiptNoteSCModel1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                            _GoodReceiptNoteSCModel1.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _GoodReceiptNoteSCModel1.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                            _GoodReceiptNoteSCModel1.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _GoodReceiptNoteSCModel1.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                            _GoodReceiptNoteSCModel1.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _GoodReceiptNoteSCModel1.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();
                            _GoodReceiptNoteSCModel1.GRNStatus = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                            _GoodReceiptNoteSCModel1.GrossValue = ds.Tables[0].Rows[0]["tot_serv_chrg"].ToString();
                            _GoodReceiptNoteSCModel1.TaxAmountRecoverable = ds.Tables[0].Rows[0]["tax_amt_recov"].ToString();
                            _GoodReceiptNoteSCModel1.TaxAmountNonRecoverable = ds.Tables[0].Rows[0]["tax_amt_nrecov"].ToString();
                            _GoodReceiptNoteSCModel1.OtherCharges = ds.Tables[0].Rows[0]["oc_amt"].ToString();
                            _GoodReceiptNoteSCModel1.NetMRValue = ds.Tables[0].Rows[0]["net_serv_chrg"].ToString();
                            _GoodReceiptNoteSCModel1.ConsumptionValue = ds.Tables[0].Rows[0]["cons_val"].ToString();
                            _GoodReceiptNoteSCModel1.NetLandedValue = ds.Tables[0].Rows[0]["landed_val"].ToString();
                            _GoodReceiptNoteSCModel1.ewb_no = ds.Tables[0].Rows[0]["ewb_no"].ToString();

                            _GoodReceiptNoteSCModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                            _GoodReceiptNoteSCModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);//Cancelled

                            ViewBag.ItemDetails = ds.Tables[1];
                            _GoodReceiptNoteSCModel1.BatchDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                            _GoodReceiptNoteSCModel1.SerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                            ViewBag.AttechmentDetails = ds.Tables[7];
                            ViewBag.ConsumeItemDetails = ds.Tables[8];
                            ViewBag.ItemStockBatchWise = ds.Tables[9];
                            ViewBag.ScrapItemDetails = ds.Tables[10];
                            ViewBag.SubItemDetails = ds.Tables[12];
                            //if (_GoodReceiptNoteSCModel1.GRNStatus == "PC" && _GoodReceiptNoteSCModel1.Command == "Edit")
                            //{
                            //    _GoodReceiptNoteSCModel1.ConvRate = ds.Tables[14].Rows[0]["conv_rate"].ToString().Trim();
                            //    _GoodReceiptNoteSCModel1.CurrId = ds.Tables[14].Rows[0]["curr_id"].ToString().Trim();
                            //    ViewBag.CostingDetails = ds.Tables[14];
                            //}
                            if (ds.Tables[14].Rows.Count > 0)
                            {
                                ViewBag.CostingDetails = ds.Tables[14];
                                _GoodReceiptNoteSCModel1.ConvRate = ds.Tables[14].Rows[0]["conv_rate"].ToString().Trim();
                                _GoodReceiptNoteSCModel1.CurrId = ds.Tables[14].Rows[0]["curr_id"].ToString().Trim();
                            }
                            if(_GoodReceiptNoteSCModel1.GRNStatus != "PC")
                            {
                                _GoodReceiptNoteSCModel1.Show = "show";
                            }
                            ViewBag.ItemTaxDetails = ds.Tables[15];
                            ViewBag.OtherChargeDetails = ds.Tables[16];
                            ViewBag.OCTaxDetails = ds.Tables[17];
                            _GoodReceiptNoteSCModel1.ScrapItemBatchWiseDetail = DataTableToJSONWithStringBuilder(ds.Tables[11]);

                            ViewBag.QtyDigit = QtyDigit1;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _GoodReceiptNoteSCModel1.Cancelled = true;
                        }
                        else
                        {
                            _GoodReceiptNoteSCModel1.Cancelled = false;
                        }

                        _GoodReceiptNoteSCModel1.DocumentStatus = Statuscode;
                        if (ViewBag.AppLevel != null && _GoodReceiptNoteSCModel1.Command != "Edit")
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

                            if (Statuscode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    _GoodReceiptNoteSCModel1.BtnName = "Refresh";
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
                                        _GoodReceiptNoteSCModel1.BtnName = "BtnToDetailPage";
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
                                        _GoodReceiptNoteSCModel1.BtnName = "BtnToDetailPage";

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
                                    _GoodReceiptNoteSCModel1.BtnName = "BtnToDetailPage";
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
                                        _GoodReceiptNoteSCModel1.BtnName = "BtnToDetailPage";
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
                                    _GoodReceiptNoteSCModel1.BtnName = "BtnToDetailPage";
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
                                    _GoodReceiptNoteSCModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _GoodReceiptNoteSCModel1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    _GoodReceiptNoteSCModel1.BtnName = "Refresh";
                                }
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }
                    else
                    {
                        _GoodReceiptNoteSCModel1.Show = "show";
                    }
                    var GRNCode = "";
                    if (GRNCodeURL != null)
                    {
                        GRNCode = GRNCodeURL;
                        _GoodReceiptNoteSCModel1.GRNNumber = GRNCodeURL;
                    }
                    else
                    {
                        GRNCode = _GoodReceiptNoteSCModel1.GRNNumber;
                    }
                    if (TransType != null)
                    {
                        _GoodReceiptNoteSCModel1.TransType = TransType;
                        ViewBag.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _GoodReceiptNoteSCModel1.Command = command;
                        ViewBag.Command = command;
                    }
                    if(TransType=="Save" && command=="Save")
                    {
                        _GoodReceiptNoteSCModel1.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _GoodReceiptNoteSCModel1.DocumentStatus;

                    }
                    if (_GoodReceiptNoteSCModel1.BtnName == null && _GoodReceiptNoteSCModel1.Command == null)
                    {
                        _GoodReceiptNoteSCModel1.BtnName = "AddNew";
                        _GoodReceiptNoteSCModel1.Command = "Add";
                        _GoodReceiptNoteSCModel1.AppStatus = "D";
                        ViewBag.DocumentStatus = _GoodReceiptNoteSCModel1.AppStatus;
                        _GoodReceiptNoteSCModel1.DocumentStatus = "D";
                        _GoodReceiptNoteSCModel1.TransType = "Save";
                        _GoodReceiptNoteSCModel1.BtnName = "BtnAddNew";

                    }

                    if (_GoodReceiptNoteSCModel1.BtnName != null)
                    {
                        _GoodReceiptNoteSCModel1.BtnName = _GoodReceiptNoteSCModel1.BtnName;
                    }
                    _GoodReceiptNoteSCModel1.TransType = _GoodReceiptNoteSCModel1.TransType;
                    ViewBag.TransType = _GoodReceiptNoteSCModel1.TransType;
                    if (_GoodReceiptNoteSCModel1.DocumentStatus != null)
                    {
                        _GoodReceiptNoteSCModel1.DocumentStatus = _GoodReceiptNoteSCModel1.DocumentStatus;
                        ViewBag.DocumentStatus = _GoodReceiptNoteSCModel1.DocumentStatus;
                        ViewBag.DocumentCode = _GoodReceiptNoteSCModel1.DocumentStatus;
                    }
                    _GoodReceiptNoteSCModel1.Title = title;
                    return View("~/Areas/ApplicationLayer/Views/SubContracting/GoodReceiptNote/GoodReceiptNoteDetailSC.cshtml", _GoodReceiptNoteSCModel1);

                }
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GRNSCBtnCommand(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel, string command)
        {
            try
            {
                /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_GoodReceiptNoteSCModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        GoodReceiptNoteSCModel _GoodReceiptNoteSCModelAdd = new GoodReceiptNoteSCModel();
                        _GoodReceiptNoteSCModelAdd.Message = "New";
                        _GoodReceiptNoteSCModelAdd.Command = "Add";
                        _GoodReceiptNoteSCModelAdd.AppStatus = "D";
                        _GoodReceiptNoteSCModelAdd.DocumentStatus = "D";
                        _GoodReceiptNoteSCModelAdd.Show = "show";
                        ViewBag.DocumentStatus = _GoodReceiptNoteSCModel.DocumentStatus;
                        _GoodReceiptNoteSCModelAdd.TransType = "Save";
                        _GoodReceiptNoteSCModelAdd.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _GoodReceiptNoteSCModelAdd;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_GoodReceiptNoteSCModel.GRNNumber))
                                return RedirectToAction("DoubleClickFromList", new { DocNo = _GoodReceiptNoteSCModel.GRNNumber, DocDate = _GoodReceiptNoteSCModel.GRNDate, ListFilterData = _GoodReceiptNoteSCModel.ListFilterData1, WF_status = _GoodReceiptNoteSCModel.WFStatus });
                            else
                                _GoodReceiptNoteSCModelAdd.Command = "Refresh";
                            _GoodReceiptNoteSCModelAdd.TransType = "Refresh";
                            _GoodReceiptNoteSCModelAdd.BtnName = "Refresh";
                            _GoodReceiptNoteSCModelAdd.DocumentStatus = null;
                            TempData["ModelData"] = _GoodReceiptNoteSCModelAdd;
                            return RedirectToAction("GoodReceiptNoteSCDetail", "GoodReceiptNoteSC");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("GoodReceiptNoteSCDetail", "GoodReceiptNoteSC");

                    case "Edit":
                        /*start Add by Hina on 15-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _GoodReceiptNoteSCModel.GRNNumber, DocDate = _GoodReceiptNoteSCModel.GRNDate, ListFilterData = _GoodReceiptNoteSCModel.ListFilterData1, WF_status = _GoodReceiptNoteSCModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string grnDt = _GoodReceiptNoteSCModel.GRNDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, grnDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _GoodReceiptNoteSCModel.GRNNumber, DocDate = _GoodReceiptNoteSCModel.GRNDate, ListFilterData = _GoodReceiptNoteSCModel.ListFilterData1, WF_status = _GoodReceiptNoteSCModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        var TransType = "";
                        var BtnName = "";
                        var GRNCodeURL = "";
                        var GRNDate = "";
                        if (CheckGRNAgainstInvoice(_GoodReceiptNoteSCModel) == "InvProcessed")
                        {
                            _GoodReceiptNoteSCModel.Message = "InvProcessed";
                            _GoodReceiptNoteSCModel.TransType = "Update";
                            _GoodReceiptNoteSCModel.Command = "Add";
                            _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
                            TransType = "Update";
                            BtnName = "BtnToDetailPage";
                            GRNCodeURL = _GoodReceiptNoteSCModel.GRNNumber;
                            GRNDate = _GoodReceiptNoteSCModel.GRNDate;
                            command = _GoodReceiptNoteSCModel.Command;
                        }
                        else
                        {
                            _GoodReceiptNoteSCModel.TransType = "Update";
                            _GoodReceiptNoteSCModel.Command = command;
                            _GoodReceiptNoteSCModel.BtnName = "BtnEdit";
                            _GoodReceiptNoteSCModel.Message = "New";
                            _GoodReceiptNoteSCModel.AppStatus = "D";
                            _GoodReceiptNoteSCModel.DocumentStatus = "D";
                            ViewBag.DocumentStatus = _GoodReceiptNoteSCModel.AppStatus;
                            TransType = "Update";
                            BtnName = "BtnEdit";
                            GRNCodeURL = _GoodReceiptNoteSCModel.GRNNumber;
                            GRNDate = _GoodReceiptNoteSCModel.GRNDate;
                            command = _GoodReceiptNoteSCModel.Command;
                        }                        
                        TempData["ModelData"] = _GoodReceiptNoteSCModel;
                        TempData["ListFilterData"] = _GoodReceiptNoteSCModel.ListFilterData1;
                        return (RedirectToAction("GoodReceiptNoteSCDetail", new { GRNCodeURL = GRNCodeURL, GRNDate, TransType, BtnName, command }));
                    case "Delete":
                        _GoodReceiptNoteSCModel.Command = command;
                        _GoodReceiptNoteSCModel.BtnName = "Refresh";
                        DeleteGRNSCDetails(_GoodReceiptNoteSCModel, command);
                        TempData["ListFilterData"] = _GoodReceiptNoteSCModel.ListFilterData1;
                        return RedirectToAction("GoodReceiptNoteSCDetail");

                    case "Save":
                        _GoodReceiptNoteSCModel.Command = command;
                        if (_GoodReceiptNoteSCModel.GRNStatus == "PC")
                        {
                            SaveGRNCostingDetail(_GoodReceiptNoteSCModel);
                        }
                        else
                        {
                            SaveGRNSCDetail(_GoodReceiptNoteSCModel);
                        }                        
                        if (_GoodReceiptNoteSCModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (_GoodReceiptNoteSCModel.Message == "DocModify")
                        {
                            DocumentMenuId = _GoodReceiptNoteSCModel.DocumentMenuId;
                            CommonPageDetails();
                            ViewBag.DocumentMenuId = _GoodReceiptNoteSCModel.DocumentMenuId;
                            ViewBag.DocumentStatus = "D";

                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = _GoodReceiptNoteSCModel.SuppID, supp_name = _GoodReceiptNoteSCModel.SuppName });
                            _GoodReceiptNoteSCModel.SupplierNameList = suppLists;

                            List<DeliveryNoteNo> jobOrd_NoLists = new List<DeliveryNoteNo>();
                            jobOrd_NoLists.Add(new DeliveryNoteNo { DNnoId = _GoodReceiptNoteSCModel.DeliveryNote_Number, DNnoVal = _GoodReceiptNoteSCModel.DeliveryNote_Number });
                            _GoodReceiptNoteSCModel.DeliveryNoteNoList = jobOrd_NoLists;

                            DataTable dt = new DataTable();
                            List<Warehouse> WHLists = new List<Warehouse>();
                            dt = GetWarehouseList();
                            foreach (DataRow dr in dt.Rows)
                            {
                                Warehouse WarehouseList = new Warehouse();
                                WarehouseList.wh_id = dr["wh_id"].ToString();
                                WarehouseList.wh_id_scrap = dr["wh_id"].ToString();
                                WarehouseList.wh_name = dr["wh_name"].ToString();
                                WHLists.Add(WarehouseList);
                            }
                            WHLists.Insert(0, new Warehouse() { wh_id = "0", wh_name = "---Select---" });
                            _GoodReceiptNoteSCModel.WarehouseList = WHLists;

                            _GoodReceiptNoteSCModel.wh_id = _GoodReceiptNoteSCModel.wh_id;
                            _GoodReceiptNoteSCModel.DeliveryNoteNumber = _GoodReceiptNoteSCModel.DeliveryNote_Number;
                            _GoodReceiptNoteSCModel.DeliveryNoteDate = _GoodReceiptNoteSCModel.DeliveryNoteDate;
                            _GoodReceiptNoteSCModel.Bill_No = _GoodReceiptNoteSCModel.Bill_No;
                            _GoodReceiptNoteSCModel.Bill_Dt = _GoodReceiptNoteSCModel.Bill_Dt;
                            _GoodReceiptNoteSCModel.CompId = _GoodReceiptNoteSCModel.wh_id;
                            _GoodReceiptNoteSCModel.BrchID = _GoodReceiptNoteSCModel.wh_id;

                            ViewBag.ItemDetails = ViewData["ItemDetails"];

                            var dttble = ViewBag.ItemDetails as DataTable;
                            _GoodReceiptNoteSCModel.wh_id = dttble.Rows[0]["wh_id"].ToString();
                            ViewBag.ConsumeItemDetails = ViewData["ConsumeItemDetails"];
                            ViewBag.ScrapItemDetails = ViewData["ScrapItemDetails"];
                            ViewBag.ItemStockBatchWise = ViewData["ConsumeBatchDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            _GoodReceiptNoteSCModel.BtnName = "Refresh";
                            _GoodReceiptNoteSCModel.Command = "Refresh";
                            _GoodReceiptNoteSCModel.DocumentStatus = "D";

                            string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            ViewBag.ValDigit = ValDigit;
                            ViewBag.QtyDigit = QtyDigit;
                            ViewBag.RateDigit = RateDigit;
                            _GoodReceiptNoteSCModel.ValDigit = ValDigit;
                            _GoodReceiptNoteSCModel.QtyDigit = QtyDigit;
                            _GoodReceiptNoteSCModel.RateDigit = RateDigit;
                            return View("~/Areas/ApplicationLayer/Views/SubContracting/GoodReceiptNote/GoodReceiptNoteDetailSC.cshtml", _GoodReceiptNoteSCModel);
                        }
                        else
                        {
                            GRNCodeURL = _GoodReceiptNoteSCModel.GRNNumber;
                            GRNDate = _GoodReceiptNoteSCModel.GRNDate;
                            TransType = _GoodReceiptNoteSCModel.TransType;
                            BtnName = _GoodReceiptNoteSCModel.BtnName;
                            TempData["ModelData"] = _GoodReceiptNoteSCModel;
                            TempData["ListFilterData"] = _GoodReceiptNoteSCModel.ListFilterData1;
                            return (RedirectToAction("GoodReceiptNoteSCDetail", new { GRNCodeURL = GRNCodeURL, GRNDate, TransType, BtnName, command }));
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
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _GoodReceiptNoteSCModel.GRNNumber, DocDate = _GoodReceiptNoteSCModel.GRNDate, ListFilterData = _GoodReceiptNoteSCModel.ListFilterData1, WF_status = _GoodReceiptNoteSCModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string grnDt1 = _GoodReceiptNoteSCModel.GRNDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, grnDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _GoodReceiptNoteSCModel.GRNNumber, DocDate = _GoodReceiptNoteSCModel.GRNDate, ListFilterData = _GoodReceiptNoteSCModel.ListFilterData1, WF_status = _GoodReceiptNoteSCModel.WFStatus });
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
                        //    return RedirectToAction("DoubleClickFromList", new { DocNo = _GoodReceiptNoteSCModel.GRNNumber, DocDate = _GoodReceiptNoteSCModel.GRNDate, ListFilterData = _GoodReceiptNoteSCModel.ListFilterData1, WF_status = _GoodReceiptNoteSCModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string grnDt2 = _GoodReceiptNoteSCModel.GRNDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, grnDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DoubleClickFromList", new { DocNo = _GoodReceiptNoteSCModel.GRNNumber, DocDate = _GoodReceiptNoteSCModel.GRNDate, ListFilterData = _GoodReceiptNoteSCModel.ListFilterData1, WF_status = _GoodReceiptNoteSCModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _GoodReceiptNoteSCModel.Command = command;
                        GRNApprove(_GoodReceiptNoteSCModel, "");

                        GRNCodeURL = _GoodReceiptNoteSCModel.GRNNumber;
                        GRNDate = _GoodReceiptNoteSCModel.GRNDate;
                        TransType = _GoodReceiptNoteSCModel.TransType;
                        BtnName = _GoodReceiptNoteSCModel.BtnName;
                        TempData["ModelData"] = _GoodReceiptNoteSCModel;
                        TempData["ListFilterData"] = _GoodReceiptNoteSCModel.ListFilterData1;
                        return (RedirectToAction("GoodReceiptNoteSCDetail", new { GRNCodeURL = GRNCodeURL, GRNDate, TransType, BtnName, command }));



                    case "Refresh":
                        GoodReceiptNoteSCModel _GoodReceiptNoteSCModelRefresh = new GoodReceiptNoteSCModel();
                        _GoodReceiptNoteSCModel.Message = null;
                        _GoodReceiptNoteSCModelRefresh.Command = command;
                        _GoodReceiptNoteSCModelRefresh.TransType = "Refresh";
                        _GoodReceiptNoteSCModelRefresh.BtnName = "Refresh";
                        _GoodReceiptNoteSCModelRefresh.DocumentStatus = null;
                        _GoodReceiptNoteSCModelRefresh.Show = "show";
                        TempData["ModelData"] = _GoodReceiptNoteSCModelRefresh;
                        TempData["ListFilterData"] = _GoodReceiptNoteSCModel.ListFilterData1;
                        return RedirectToAction("GoodReceiptNoteSCDetail");

                    case "Print":
                    return GenratePdfFile(_GoodReceiptNoteSCModel);
                    case "BacktoList":
                        TempData["ListFilterData"] = _GoodReceiptNoteSCModel.ListFilterData1;
                        return RedirectToAction("GoodReceiptNoteSC", "GoodReceiptNoteSC");

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
        public ActionResult SaveGRNSCDetail(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel)
        {
            string SaveMessage = "";
            /*getDocumentName();*/ /* To set Title*/
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_GoodReceiptNoteSCModel.Cancelled == false)
                {
                    _GoodReceiptNoteSCModel.DocumentMenuId = _GoodReceiptNoteSCModel.DocumentMenuId;


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
                    DataTable DispatchQtyItemDetails = new DataTable();
                    DataTable DtblAttchDetail = new DataTable();
                    DataTable DtblItemReturnDetail = new DataTable();
                    DataTable DtblConsumeItemDetail = new DataTable();
                    DataTable DtblScrapItemDetail = new DataTable();
                    DataTable DtblSubItemDetail = new DataTable();
                    //DataTable DtblConsScrapbySubItemDetail = new DataTable();
                    

                    DataTable dtheader = new DataTable();

                    DtblHDetail = ToDtblHDetail(_GoodReceiptNoteSCModel);
                    DtblItemDetail = ToDtblItemDetail(_GoodReceiptNoteSCModel.GRNSCItemdetails);
                    DtblConsumeItemDetail = ToDtblConsumeItemDetail(_GoodReceiptNoteSCModel.GRNSCConsumeItemdetails);
                    DtblScrapItemDetail = ToDtblScrapItemDetail(_GoodReceiptNoteSCModel.GRNSCScrapItemdetails);
                    DtblSubItemDetail = ToDtblSubItem(_GoodReceiptNoteSCModel.SubItemDetailsDt);
                    //DtblConsScrapbySubItemDetail = ToDtblConsSubItem(_GoodReceiptNoteSCModel.ConsScrapbySubItemDetailsDt);
                    
                    DataTable DtblItemBatchDetail = new DataTable();
                    DataTable DtblItemSerialDetail = new DataTable();

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    Batch_detail.Columns.Add("batch_qty", typeof(float));
                    Batch_detail.Columns.Add("reject_batch_qty", typeof(string));
                    Batch_detail.Columns.Add("rework_batch_qty", typeof(string));
                    Batch_detail.Columns.Add("exp_dt", typeof(string));
                    if (_GoodReceiptNoteSCModel.BatchDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_GoodReceiptNoteSCModel.BatchDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["item_id"].ToString();
                            dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["batch_no"].ToString();
                            dtrowBatchDetailsLines["batch_qty"] = jObjectBatch[i]["batch_qty"].ToString();
                            dtrowBatchDetailsLines["reject_batch_qty"] = jObjectBatch[i]["reject_batch_qty"].ToString();
                            dtrowBatchDetailsLines["rework_batch_qty"] = jObjectBatch[i]["rework_batch_qty"].ToString();
                            if (jObjectBatch[i]["exp_dt"].ToString() == "" || jObjectBatch[i]["exp_dt"].ToString() == null)
                            {
                                dtrowBatchDetailsLines["exp_dt"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["exp_dt"].ToString();
                            }
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                        ViewData["BatchDetails"] = dtbatchdetail(jObjectBatch);
                    }
                    DtblItemBatchDetail = Batch_detail;

                    DataTable Serial_detail = new DataTable();
                    Serial_detail.Columns.Add("item_id", typeof(string));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("QtyType", typeof(string));

                    if (_GoodReceiptNoteSCModel.SerialDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_GoodReceiptNoteSCModel.SerialDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["item_id"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["serial_no"].ToString();
                            dtrowSerialDetailsLines["QtyType"] = jObjectSerial[i]["QtyType"].ToString();
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                        ViewData["SerialDetails"] = dtserialdetail(jObjectSerial);
                    }
                    DtblItemSerialDetail = Serial_detail;

                    DataTable DtblConsumeItemBatchDetails = new DataTable();
                    DataTable ConsumeBatch_detail = new DataTable();
                    
                    ConsumeBatch_detail.Columns.Add("item_id", typeof(string));
                    ConsumeBatch_detail.Columns.Add("DispSrcDocNo", typeof(string));
                    ConsumeBatch_detail.Columns.Add("DispSrcDocDt", typeof(string));
                    ConsumeBatch_detail.Columns.Add("wh_id", typeof(string));
                    ConsumeBatch_detail.Columns.Add("lot_no", typeof(string));
                    ConsumeBatch_detail.Columns.Add("batch_no", typeof(string));
                    ConsumeBatch_detail.Columns.Add("expiry_date", typeof(DateTime));
                    ConsumeBatch_detail.Columns.Add("reserve_qty", typeof(float));
                    ConsumeBatch_detail.Columns.Add("consume_qty", typeof(float));
                    
                    if (_GoodReceiptNoteSCModel.ConsumeItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_GoodReceiptNoteSCModel.ConsumeItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowCnsumBatchDetailsLines = ConsumeBatch_detail.NewRow();
                            //dtrowCnsumBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                            //dtrowCnsumBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                            dtrowCnsumBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                            dtrowCnsumBatchDetailsLines["DispSrcDocNo"] = jObjectBatch[i]["SrcDocNo"].ToString();
                            dtrowCnsumBatchDetailsLines["DispSrcDocDt"] = jObjectBatch[i]["SrcDocDt"].ToString();
                            dtrowCnsumBatchDetailsLines["wh_id"] = jObjectBatch[i]["WarehouseID"].ToString();
                            dtrowCnsumBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                            dtrowCnsumBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                            if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null || jObjectBatch[i]["ExpiryDate"].ToString() == "undefined")
                            {
                                dtrowCnsumBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowCnsumBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                            }

                            dtrowCnsumBatchDetailsLines["reserve_qty"] = jObjectBatch[i]["ReserveQty"].ToString();
                            dtrowCnsumBatchDetailsLines["consume_qty"] = jObjectBatch[i]["Consumed_Qty"].ToString();
                            
                            ConsumeBatch_detail.Rows.Add(dtrowCnsumBatchDetailsLines);
                        }
                        ViewData["ConsumeBatchDetails"] = dtconsumebatchdetail(jObjectBatch);
                    }
                    DtblConsumeItemBatchDetails = ConsumeBatch_detail;

                    DataTable DtblScrapItemBatchDetail = new DataTable();
                    
                    DataTable ScrapBatch_detail = new DataTable();
                    ScrapBatch_detail.Columns.Add("item_id", typeof(string));
                    ScrapBatch_detail.Columns.Add("batch_no", typeof(string));
                    ScrapBatch_detail.Columns.Add("batch_qty", typeof(float));
                    ScrapBatch_detail.Columns.Add("exp_dt", typeof(string));
                    if (_GoodReceiptNoteSCModel.ScrapItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_GoodReceiptNoteSCModel.ScrapItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowscrapBatchDetails = ScrapBatch_detail.NewRow();
                            dtrowscrapBatchDetails["item_id"] = jObjectBatch[i]["item_id"].ToString();
                            dtrowscrapBatchDetails["batch_no"] = jObjectBatch[i]["batch_no"].ToString();
                            dtrowscrapBatchDetails["batch_qty"] = jObjectBatch[i]["batch_qty"].ToString();
                            if (jObjectBatch[i]["exp_dt"].ToString() == "" || jObjectBatch[i]["exp_dt"].ToString() == null)
                            {
                                dtrowscrapBatchDetails["exp_dt"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowscrapBatchDetails["exp_dt"] = jObjectBatch[i]["exp_dt"].ToString();
                            }
                            ScrapBatch_detail.Rows.Add(dtrowscrapBatchDetails);
                        }
                        ViewData["ScrapBatchDetails"] = dtscrapbatchdetail(jObjectBatch);
                    }
                    DtblScrapItemBatchDetail = ScrapBatch_detail;


                    var _JobOrderDetailsattch = TempData["ModelDataattch"] as GRNDetailsattch;
                    TempData["ModelDataattch"] = null;
                    DataTable dtAttachment = new DataTable();
                    if (_GoodReceiptNoteSCModel.attatchmentdetail != null)
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
                            if (_GoodReceiptNoteSCModel.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _GoodReceiptNoteSCModel.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(_GoodReceiptNoteSCModel.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(_GoodReceiptNoteSCModel.GRNNumber))
                                {
                                    dtrowAttachment1["id"] = _GoodReceiptNoteSCModel.GRNNumber;
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

                        if (_GoodReceiptNoteSCModel.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\LogsFile\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_GoodReceiptNoteSCModel.GRNNumber))
                                {
                                    ItmCode = _GoodReceiptNoteSCModel.GRNNumber;
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

                    SaveMessage = _GoodReceiptNoteSC_IServices.InsertGRNSC_Details(DtblHDetail, DtblItemDetail, DtblItemBatchDetail ,DtblItemSerialDetail, DtblAttchDetail, DtblConsumeItemDetail, DtblConsumeItemBatchDetails, DtblScrapItemDetail, DtblScrapItemBatchDetail, DtblSubItemDetail);
                    if (SaveMessage == "DocModify")
                    {
                        _GoodReceiptNoteSCModel.Message = "DocModify";
                        _GoodReceiptNoteSCModel.BtnName = "Refresh";
                        _GoodReceiptNoteSCModel.Command = "Refresh";
                        _GoodReceiptNoteSCModel.DocumentMenuId = DocumentMenuId;
                        ViewBag.Message = "DocModify";
                        TempData["ModelData"] = _GoodReceiptNoteSCModel;
                        return RedirectToAction("GoodReceiptNoteSCDetail");
                    }
                    else
                    {
                        string[] Data = SaveMessage.Split(',');

                        string DNNo = Data[1];
                        string DN_No = DNNo.Replace("/", "");
                        string Message = Data[0];
                        if (Message == "Data_Not_Found")
                        {
                            var a = DN_No.Split('-');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _GoodReceiptNoteSCModel.Message = Message.Replace("_", "");
                            return RedirectToAction("GoodReceiptNoteSCDetail");
                        }
                        string DNDate = Data[2];
                        string Message1 = Data[4];
                        string StatusCode = Data[3];
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
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, DN_No, _GoodReceiptNoteSCModel.TransType, DtblAttchDetail);

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
                            //                string DN_No1 = DN_No.Replace("/", "");
                            //                string img_nm = CompID + BrchID + DN_No1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                            _GoodReceiptNoteSCModel.Message = "Save";

                            _GoodReceiptNoteSCModel.GRNNumber = DNNo;
                            _GoodReceiptNoteSCModel.GRNDate = DNDate;
                            _GoodReceiptNoteSCModel.TransType = "Update";
                            _GoodReceiptNoteSCModel.Command = "Update";
                            _GoodReceiptNoteSCModel.AppStatus = "D";
                            _GoodReceiptNoteSCModel.DocumentStatus = "D";

                            _GoodReceiptNoteSCModel.BtnName = "BtnSave";
                            TempData["ModelData"] = _GoodReceiptNoteSCModel;
                            return RedirectToAction("GoodReceiptNoteSCDetail");

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
                    _GoodReceiptNoteSCModel.CreatedBy = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    string SaveMessage1 = _GoodReceiptNoteSC_IServices.GRNCancel(_GoodReceiptNoteSCModel, CompID, br_id, mac_id);
                    var Message1 = SaveMessage1.Split('-');
                    var Message = Message1[1];
                    if (Message == "StNtAvl")
                    {
                        _GoodReceiptNoteSCModel.Message = "StockNotAvailable";
                        _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
                        _GoodReceiptNoteSCModel.Command = "Approve";
                        _GoodReceiptNoteSCModel.TransType = "Update";
                        _GoodReceiptNoteSCModel.AppStatus = "D";
                    }
                    else
                    {
                        _GoodReceiptNoteSCModel.Message = "Cancelled";
                        _GoodReceiptNoteSCModel.Command = "Update";
                        _GoodReceiptNoteSCModel.GRNNumber = _GoodReceiptNoteSCModel.GRNNumber;
                        _GoodReceiptNoteSCModel.GRNDate = _GoodReceiptNoteSCModel.GRNDate;
                        _GoodReceiptNoteSCModel.TransType = "Update";
                        _GoodReceiptNoteSCModel.AppStatus = "D";
                        _GoodReceiptNoteSCModel.BtnName = "Refresh";
                        return RedirectToAction("GoodReceiptNoteSCDetail");
                    }
                    
                }
                return RedirectToAction("GoodReceiptNoteSCDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_GoodReceiptNoteSCModel.TransType == "Save")
                    {
                        string Guid = "";
                        if (_GoodReceiptNoteSCModel.Guid != null)
                        {
                            Guid = _GoodReceiptNoteSCModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public ActionResult SaveGRNCostingDetail(GoodReceiptNoteSCModel GoodsReceiptNote)
        {
            try
            {
                string SaveMessage = "";
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
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtbConlItemDetail = new DataTable();
                DtblHDetail = ToDtblHDetail(GoodsReceiptNote);
                DtblItemDetail = CostingDetailItmList(GoodsReceiptNote.CostingDetailItmDt);
                DtbConlItemDetail = CostingDetailConItmConList(GoodsReceiptNote.CostingDetailConItmDt);
                if (DtblItemDetail.Rows.Count > 0 || GoodsReceiptNote.Cancelled)
                {
                    DataTable DtblTaxDetail = ToDtblTaxDetail(GoodsReceiptNote.CostingDetailItmTaxDt, "Tax");
                    DataTable DtblOCTaxDetail = ToDtblTaxDetail(GoodsReceiptNote.CostingDetailItmOCTaxDt, "OC");
                    DataTable DtblOcDetail = ToDtblOCDetail(GoodsReceiptNote.CostingDetailOcDt);

                    SaveMessage = _GoodReceiptNoteSC_IServices.InsertGRNCosting_Details(DtblHDetail, DtblItemDetail, DtbConlItemDetail, DtblTaxDetail, DtblOcDetail, DtblOCTaxDetail);
                    string GRNSC_No = SaveMessage.Split(',')[0].Trim();
                    string StkCheck = SaveMessage.Split(',')[1].Trim();
                    string Status = SaveMessage.Split(',')[4];
                    if (StkCheck == "StkNotAvl")
                    {
                        GoodsReceiptNote.Message = "StockNotAvailable";
                        GoodsReceiptNote.TransType = "Update";
                        GoodsReceiptNote.Command = "Save";
                        GoodsReceiptNote.AppStatus = "D";
                        GoodsReceiptNote.BtnName = "BtnToDetailPage";
                    }                  
                    else if (Status.Trim() == "A")
                    {
                        GoodsReceiptNote.Message = "CostingComplited";
                        //string fileName = "PO_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        //var filePath = SavePdfDocToSendOnEmailAlert(GoodsReceiptNote.GRNNumber, GoodsReceiptNote.GRNDate, fileName);
                        //_Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, GoodsReceiptNote.GRNNumber, Status.Trim(), UserID, "", filePath);
                    }
                    else if (Status.Trim() == "C")
                    {
                        GoodsReceiptNote.Message = "Cancelled";
                    }
                    GoodsReceiptNote.Command = "Update";
                    GoodsReceiptNote.BtnName = "BtnToDetailPage";
                    GoodsReceiptNote.TransType = "Update";
                }
                else
                {
                    GoodsReceiptNote.Message = "CostingDetailNotFound";
                    GoodsReceiptNote.Command = "Update";
                    GoodsReceiptNote.BtnName = "BtnToDetailPage";
                    GoodsReceiptNote.TransType = "Update";
                }
                return RedirectToAction("GoodReceiptNoteSCDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public DataTable CostingDetailItmList(string GRNItemdetails)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("grn_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_gross_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_net_val", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            if (GRNItemdetails != null)
            {
                JArray jObject = JArray.Parse(GRNItemdetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                    dtrowLines["grn_qty"] = jObject[i]["GrnQty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["ItmRate"].ToString();
                    dtrowLines["item_gross_val"] = jObject[i]["GrossVal"].ToString();
                    dtrowLines["item_tax_amt"] = jObject[i]["TaxAmt"].ToString();
                    dtrowLines["item_net_val"] = jObject[i]["NetValSpec"].ToString();
                    dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
            }
            return dtItem;
        }
        public DataTable CostingDetailConItmConList(string GRNConItemdetails)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(string));
            dtItem.Columns.Add("con_qty", typeof(string));
            dtItem.Columns.Add("tot_cost", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            if (GRNConItemdetails != null)
            {
                JArray jObject = JArray.Parse(GRNConItemdetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["uom_id"].ToString();
                    dtrowLines["con_qty"] = jObject[i]["ConQty"].ToString();
                    dtrowLines["tot_cost"] = jObject[i]["TotCost"].ToString();
                    dtrowLines["hsn_code"] = "40021990";
                    //dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
            }
            return dtItem;
        }
        private DataTable ToDtblTaxDetail(string TaxDetails, string taxOn)
        {
            try
            {
                DataTable DtblItemTaxDetail = new DataTable();
                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("tax_id", typeof(int));
                dtItem.Columns.Add("tax_rate", typeof(string));
                dtItem.Columns.Add("tax_val", typeof(string));
                dtItem.Columns.Add("tax_level", typeof(int));
                dtItem.Columns.Add("tax_apply_on", typeof(string));
                dtItem.Columns.Add("recov", typeof(string));

                if (TaxDetails != null)
                {
                    JArray jObject = JArray.Parse(TaxDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["tax_id"] = jObject[i]["TaxID"].ToString();
                        dtrowLines["tax_rate"] = jObject[i]["TaxRate"].ToString();
                        dtrowLines["tax_val"] = jObject[i]["TaxValue"].ToString();
                        dtrowLines["tax_level"] = jObject[i]["TaxLevel"].ToString();
                        dtrowLines["tax_apply_on"] = jObject[i]["TaxApplyOn"].ToString();
                        dtrowLines["recov"] = taxOn == "OC" ? "" : jObject[i]["TaxRecov"].ToString();
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
        private DataTable ToDtblOCDetail(string OCDetails)
        {
            try
            {

                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("oc_id", typeof(int));
                dtItem.Columns.Add("oc_val", typeof(string));
                dtItem.Columns.Add("tax_amt", typeof(string));
                dtItem.Columns.Add("total_amt", typeof(string));
                dtItem.Columns.Add("curr_id", typeof(string));
                dtItem.Columns.Add("conv_rate", typeof(string));
                dtItem.Columns.Add("supp_id", typeof(string));
                dtItem.Columns.Add("supp_type", typeof(string));//Added by Suraj on 11-04-2024
                dtItem.Columns.Add("bill_no", typeof(string));
                dtItem.Columns.Add("bill_date", typeof(string));
                if (OCDetails != null)
                {
                    JArray jObject = JArray.Parse(OCDetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["oc_id"] = jObject[i]["OC_ID"].ToString();
                        dtrowLines["oc_val"] = jObject[i]["OCValue"].ToString();
                        dtrowLines["tax_amt"] = jObject[i]["OC_TaxAmt"].ToString();
                        dtrowLines["total_amt"] = jObject[i]["OC_TotlAmt"].ToString();
                        dtrowLines["curr_id"] = jObject[i]["OC_Curr_Id"].ToString();
                        dtrowLines["conv_rate"] = jObject[i]["OC_Conv"].ToString();
                        dtrowLines["supp_id"] = jObject[i]["OC_SuppId"].ToString();
                        dtrowLines["supp_type"] = jObject[i]["OC_SuppType"].ToString();
                        dtrowLines["bill_no"] = jObject[i]["OC_BillNo"].ToString();
                        dtrowLines["bill_date"] = jObject[i]["OC_BillDate"].ToString();
                        dtItem.Rows.Add(dtrowLines);
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
        private DataTable ToDtblHDetail(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel)
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
                dtheader.Columns.Add("GRN_No", typeof(string));
                dtheader.Columns.Add("GRN_Date", typeof(string));
                dtheader.Columns.Add("SuppID", typeof(int));
                dtheader.Columns.Add("DNDocNo", typeof(string));
                dtheader.Columns.Add("DNDocDate", typeof(string));
                dtheader.Columns.Add("FItemID", typeof(string));
                dtheader.Columns.Add("FUomId", typeof(int));
                dtheader.Columns.Add("CompID", typeof(string));
                dtheader.Columns.Add("BranchID", typeof(string));
                dtheader.Columns.Add("UserID", typeof(int));
                dtheader.Columns.Add("GRNStatus", typeof(string));
                dtheader.Columns.Add("SystemDetail", typeof(string));
                dtheader.Columns.Add("gr_val", typeof(string));
                dtheader.Columns.Add("tax_amt_nrecov", typeof(string));
                dtheader.Columns.Add("tax_amt_recov", typeof(string));
                dtheader.Columns.Add("oc_amt", typeof(string));
                dtheader.Columns.Add("net_val", typeof(string));
                dtheader.Columns.Add("landed_val", typeof(string));
                dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("tot_serv_chrg", typeof(string));
                dtheader.Columns.Add("net_serv_chrg", typeof(string));
                dtheader.Columns.Add("cons_val", typeof(string));
                dtheader.Columns.Add("ewb_no", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _GoodReceiptNoteSCModel.TransType;
                dtrowHeader["GRN_No"] = _GoodReceiptNoteSCModel.GRNNumber;
                dtrowHeader["GRN_Date"] = _GoodReceiptNoteSCModel.GRNDate;
                dtrowHeader["SuppID"] = _GoodReceiptNoteSCModel.SuppID;
                dtrowHeader["DNDocNo"] = _GoodReceiptNoteSCModel.DeliveryNote_Number;
                dtrowHeader["DNDocDate"] = _GoodReceiptNoteSCModel.DeliveryNoteDate;
                dtrowHeader["FItemID"] = _GoodReceiptNoteSCModel.FinishProductId;
                dtrowHeader["FUomId"] = _GoodReceiptNoteSCModel.FinishUomId;
                dtrowHeader["CompID"] = CompID;
                dtrowHeader["BranchID"] = BrchID;
                dtrowHeader["UserID"] = UserID;
                dtrowHeader["GRNStatus"] = IsNull(_GoodReceiptNoteSCModel.GRNStatus, "D");

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["SystemDetail"] = mac_id;
                dtrowHeader["gr_val"] = "0";
                dtrowHeader["tax_amt_nrecov"] = IsNull(_GoodReceiptNoteSCModel.TaxAmountNonRecoverable, "0");
                dtrowHeader["tax_amt_recov"] = IsNull(_GoodReceiptNoteSCModel.TaxAmountRecoverable, "0");
                dtrowHeader["oc_amt"] = IsNull(_GoodReceiptNoteSCModel.OtherCharges, "0");
                dtrowHeader["net_val"] = "0";
                dtrowHeader["landed_val"] = IsNull(_GoodReceiptNoteSCModel.NetLandedValue, "0");
                string cancelflag = _GoodReceiptNoteSCModel.Cancelled.ToString();
                if (cancelflag == "False")
                {
                    dtrowHeader["Cancelled"] = "N";
                }
                else
                {
                    dtrowHeader["Cancelled"] = "Y";
                }
                dtrowHeader["tot_serv_chrg"] = IsNull(_GoodReceiptNoteSCModel.GrossValue, "0");
                dtrowHeader["net_serv_chrg"] = IsNull(_GoodReceiptNoteSCModel.NetMRValue, "0");
                dtrowHeader["cons_val"] = IsNull(_GoodReceiptNoteSCModel.ConsumptionValue, "0");
                dtrowHeader["ewb_no"] = _GoodReceiptNoteSCModel.ewb_no;
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
        private DataTable ToDtblItemDetail(string grnItemDetail)
        {
            try
            {
                DataTable DtblItemDetail = new DataTable();
                DataTable dtItem = new DataTable();
                //dtItem.Columns.Add("MDSrcDocNo", typeof(string));
                //dtItem.Columns.Add("MDSrcDocDate", typeof(string));
                dtItem.Columns.Add("ItemID", typeof(string));
                dtItem.Columns.Add("UOMID", typeof(string));
                dtItem.Columns.Add("AcceptQty", typeof(float));
                dtItem.Columns.Add("RejectQty", typeof(float));
                dtItem.Columns.Add("ReworkQty", typeof(float));
                dtItem.Columns.Add("WhId", typeof(int));
                dtItem.Columns.Add("Reject_WhId", typeof(int));
                dtItem.Columns.Add("Rework_WhId", typeof(int));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gross_val", typeof(string));
                dtItem.Columns.Add("item_tax_amt_recov", typeof(string));
                dtItem.Columns.Add("item_tax_amt_nrecov", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val", typeof(string));
                dtItem.Columns.Add("item_landed_rate", typeof(string));
                dtItem.Columns.Add("item_landed_val", typeof(string));
                dtItem.Columns.Add("Remarks", typeof(string));


                if (grnItemDetail != null)
                {
                    JArray jObject = JArray.Parse(grnItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal  Accept_qty, Reject_qty, Rework_qty;
                        
                        if (jObject[i]["AccptQty"].ToString() == "")
                            Accept_qty = 0;
                        else
                            Accept_qty = Convert.ToDecimal(jObject[i]["AccptQty"].ToString());

                        if (jObject[i]["RejQty"].ToString() == "")
                            Reject_qty = 0;
                        else
                            Reject_qty = Convert.ToDecimal(jObject[i]["RejQty"].ToString());

                        if (jObject[i]["RewrkQty"].ToString() == "")
                            Rework_qty = 0;
                        else
                            Rework_qty = Convert.ToDecimal(jObject[i]["RewrkQty"].ToString());

                        DataRow dtrowLines = dtItem.NewRow();

                        
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                        dtrowLines["AcceptQty"] = Accept_qty; 
                         dtrowLines["RejectQty"] = Reject_qty;
                        dtrowLines["ReworkQty"] = Rework_qty;
                        // dtrowLines["WhId"] = jObject[i]["WhID"].ToString();
                        //dtrowLines["Reject_WhId"] = jObject[i]["RejWhID"].ToString();
                        //dtrowLines["Rework_WhId"] = jObject[i]["RewrkWhID"].ToString();
                        if (jObject[i]["WhID"].ToString() == "null")
                        {
                            dtrowLines["WhId"] = "0";
                        }
                        else
                        {
                            //var asd = jObject[i]["reject_wh_id"].ToString();
                            dtrowLines["WhId"] = Convert.ToInt32(jObject[i]["WhID"].ToString());
                        }
                        if (jObject[i]["RejWhID"].ToString() == "null")
                        {
                            dtrowLines["Reject_WhId"] = "0";
                        }
                        else
                        {
                            //var asd = jObject[i]["reject_wh_id"].ToString();
                            dtrowLines["Reject_WhId"] = Convert.ToInt32(jObject[i]["RejWhID"].ToString());
                        }
                        if (jObject[i]["RewrkWhID"].ToString() == "null")
                        {
                            dtrowLines["Rework_WhId"] = "0";
                        }
                        else
                        {
                            //var asd = jObject[i]["reject_wh_id"].ToString();
                            dtrowLines["Rework_WhId"] = Convert.ToInt32(jObject[i]["RewrkWhID"].ToString());
                        }
                        dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                        dtrowLines["item_gross_val"] = jObject[i]["item_gross_val"].ToString();
                        dtrowLines["item_tax_amt_recov"] = jObject[i]["item_tax_amt_recov"].ToString();
                        dtrowLines["item_tax_amt_nrecov"] = jObject[i]["item_tax_amt_nrecov"].ToString();
                        dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                        dtrowLines["item_net_val"] = jObject[i]["item_net_val"].ToString();
                        dtrowLines["item_landed_rate"] = jObject[i]["item_landed_rate"].ToString();
                        dtrowLines["item_landed_val"] = jObject[i]["item_landed_val"].ToString();
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();

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
        private DataTable ToDtblConsumeItemDetail(string ConsumegrnItemDetail)
        {
            try
            {
                DataTable DtblConsumeItemDetail = new DataTable();
                DataTable dtItemconsume = new DataTable();
                
                dtItemconsume.Columns.Add("ItemID", typeof(string));
                dtItemconsume.Columns.Add("UOMID", typeof(string));
                dtItemconsume.Columns.Add("IssueQty", typeof(float));
                dtItemconsume.Columns.Add("ConsumeQty", typeof(float));
                dtItemconsume.Columns.Add("Remarks", typeof(string));


                if (ConsumegrnItemDetail != null)
                {
                    JArray jObject = JArray.Parse(ConsumegrnItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal Issue_qty, Consume_qty;

                        if (jObject[i]["IssuedQty"].ToString() == "")
                            Issue_qty = 0;
                        else
                            Issue_qty = Convert.ToDecimal(jObject[i]["IssuedQty"].ToString());

                        if (jObject[i]["ConsumedQty"].ToString() == "")
                            Consume_qty = 0;
                        else
                            Consume_qty = Convert.ToDecimal(jObject[i]["ConsumedQty"].ToString());



                        DataRow dtrowsLines = dtItemconsume.NewRow();


                        dtrowsLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowsLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        dtrowsLines["IssueQty"] = Issue_qty;
                        dtrowsLines["ConsumeQty"] = Consume_qty;
                        dtrowsLines["Remarks"] = jObject[i]["Remarks"].ToString();
                        dtItemconsume.Rows.Add(dtrowsLines);
                    }
                    ViewData["ConsumeItemDetails"] = dtconsumeitemdetail(jObject);
                }

                DtblConsumeItemDetail = dtItemconsume;
                return DtblConsumeItemDetail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private DataTable ToDtblScrapItemDetail(string ScrapItemDetail)
        {
            try
            {
                DataTable DtblScrapItemDetail = new DataTable();
                DataTable dtscrapItem = new DataTable();

                dtscrapItem.Columns.Add("ItemID", typeof(string));
                dtscrapItem.Columns.Add("UOMID", typeof(string));
                dtscrapItem.Columns.Add("ItemRate", typeof(string));
                dtscrapItem.Columns.Add("ReceivedQty", typeof(float));
                dtscrapItem.Columns.Add("MaterialTyp", typeof(string));
                dtscrapItem.Columns.Add("WhId", typeof(int));
                dtscrapItem.Columns.Add("Remarks", typeof(string));


                if (ScrapItemDetail != null)
                {
                    JArray jObject = JArray.Parse(ScrapItemDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        decimal Recev_qty;

                        if (jObject[i]["ReceivQty"].ToString() == "")
                            Recev_qty = 0;
                        else
                            Recev_qty = Convert.ToDecimal(jObject[i]["ReceivQty"].ToString());

                        DataRow dtrowLines = dtscrapItem.NewRow();
                        dtrowLines["ItemID"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["UOMID"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["ItemRate"] = jObject[i]["ItemRateScrap"].ToString();
                        dtrowLines["ReceivedQty"] = Recev_qty;
                        dtrowLines["MaterialTyp"] = jObject[i]["MtrTyp"].ToString();
                        if (jObject[i]["WhID"].ToString() == "null")
                        {
                            dtrowLines["WhId"] = "0";
                        }
                        else
                        {
                            dtrowLines["WhId"] = Convert.ToInt32(jObject[i]["WhID"].ToString());
                        }
                        dtrowLines["Remarks"] = jObject[i]["Remarks"].ToString();

                        dtscrapItem.Rows.Add(dtrowLines);
                    }
                    ViewData["ScrapItemDetails"] = dtscrapitemdetail(jObject);
                }

                DtblScrapItemDetail = dtscrapItem;
                return DtblScrapItemDetail;
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
                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("acc_qty", typeof(string));
                dtSubItem.Columns.Add("rej_qty", typeof(string));
                dtSubItem.Columns.Add("rew_qty", typeof(string));
                dtSubItem.Columns.Add("issue_qty", typeof(string));
                dtSubItem.Columns.Add("cons_qty", typeof(string));
                dtSubItem.Columns.Add("recv_qty", typeof(string));
                dtSubItem.Columns.Add("Type", typeof(string));

                if (!string.IsNullOrEmpty(SubitemDetails))
                {
                    JArray jObject2 = JArray.Parse(SubitemDetails);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["acc_qty"] = jObject2[i]["AccQty"].ToString();
                        dtrowItemdetails["rej_qty"] = jObject2[i]["RejQty"].ToString();
                        dtrowItemdetails["rew_qty"] = jObject2[i]["RewQty"].ToString();
                        dtrowItemdetails["issue_qty"] = jObject2[i]["Issue_Qty"].ToString();
                        dtrowItemdetails["cons_qty"] = jObject2[i]["Consumed_Qty"].ToString();
                        dtrowItemdetails["recv_qty"] = jObject2[i]["Qty"].ToString();
                        dtrowItemdetails["Type"] = jObject2[i]["SubItmTyp"].ToString();
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
        public DataTable dtitemdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(string));
            dtItem.Columns.Add("uom_alias", typeof(string));
            dtItem.Columns.Add("rec_qty", typeof(float));
            dtItem.Columns.Add("reject_qty", typeof(float));
            dtItem.Columns.Add("rework_qty", typeof(float));
            dtItem.Columns.Add("wh_id", typeof(int));
            dtItem.Columns.Add("rej_wh_id", typeof(int));
            dtItem.Columns.Add("rework_wh_id", typeof(int));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("lot_id", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                decimal Accept_qty, Reject_qty, Rework_qty;

                if (jObject[i]["AccptQty"].ToString() == "")
                    Accept_qty = 0;
                else
                    Accept_qty = Convert.ToDecimal(jObject[i]["AccptQty"].ToString());

                if (jObject[i]["RejQty"].ToString() == "")
                    Reject_qty = 0;
                else
                    Reject_qty = Convert.ToDecimal(jObject[i]["RejQty"].ToString());

                if (jObject[i]["RewrkQty"].ToString() == "")
                    Rework_qty = 0;
                else
                    Rework_qty = Convert.ToDecimal(jObject[i]["RewrkQty"].ToString());

                DataRow dtrowLines = dtItem.NewRow();


                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["uom_alias"] = jObject[i]["UomName"].ToString();
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                dtrowLines["rec_qty"] = Accept_qty;
                dtrowLines["reject_qty"] = Reject_qty;
                dtrowLines["rework_qty"] = Rework_qty;
                // dtrowLines["WhId"] = jObject[i]["WhID"].ToString();
                //dtrowLines["Reject_WhId"] = jObject[i]["RejWhID"].ToString();
                //dtrowLines["Rework_WhId"] = jObject[i]["RewrkWhID"].ToString();
                if (jObject[i]["WhID"].ToString() == "null")
                {
                    dtrowLines["wh_id"] = "0";
                }
                else
                {
                    //var asd = jObject[i]["reject_wh_id"].ToString();
                    dtrowLines["wh_id"] = Convert.ToInt32(jObject[i]["WhID"].ToString());
                }
                if (jObject[i]["RejWhID"].ToString() == "null")
                {
                    dtrowLines["rej_wh_id"] = "0";
                }
                else
                {
                    //var asd = jObject[i]["reject_wh_id"].ToString();
                    dtrowLines["rej_wh_id"] = Convert.ToInt32(jObject[i]["RejWhID"].ToString());
                }
                if (jObject[i]["RewrkWhID"].ToString() == "null")
                {
                    dtrowLines["rework_wh_id"] = "0";
                }
                else
                {
                    //var asd = jObject[i]["reject_wh_id"].ToString();
                    dtrowLines["rework_wh_id"] = Convert.ToInt32(jObject[i]["RewrkWhID"].ToString());
                }
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();

                dtrowLines["it_remarks"] = jObject[i]["Remarks"].ToString();
                dtrowLines["lot_id"] = "0";
                dtrowLines["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowLines["i_serial"] = jObject[i]["i_serial"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtconsumeitemdetail(JArray jObject)
        {
            DataTable dtItemconsume = new DataTable();

            dtItemconsume.Columns.Add("item_id", typeof(string));
            dtItemconsume.Columns.Add("item_name", typeof(string));
            dtItemconsume.Columns.Add("sub_item", typeof(string));
            dtItemconsume.Columns.Add("uom_id", typeof(string));
            dtItemconsume.Columns.Add("UomName", typeof(string));
            dtItemconsume.Columns.Add("issue_qty", typeof(float));
            dtItemconsume.Columns.Add("cons_qty", typeof(float));
            dtItemconsume.Columns.Add("i_batch", typeof(string));
            dtItemconsume.Columns.Add("it_remarks", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                decimal Issue_qty, Consume_qty;

                if (jObject[i]["IssuedQty"].ToString() == "")
                    Issue_qty = 0;
                else
                    Issue_qty = Convert.ToDecimal(jObject[i]["IssuedQty"].ToString());

                if (jObject[i]["ConsumedQty"].ToString() == "")
                    Consume_qty = 0;
                else
                    Consume_qty = Convert.ToDecimal(jObject[i]["ConsumedQty"].ToString());



                DataRow dtrowsLines = dtItemconsume.NewRow();


                dtrowsLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowsLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowsLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowsLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowsLines["UomName"] = jObject[i]["UomName"].ToString();
                dtrowsLines["issue_qty"] = Issue_qty;
                dtrowsLines["cons_qty"] = Consume_qty;
                dtrowsLines["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowsLines["it_remarks"] = jObject[i]["Remarks"].ToString();
                dtItemconsume.Rows.Add(dtrowsLines);
            }
            return dtItemconsume;
        }
        public DataTable dtscrapitemdetail(JArray jObject)
        {
            DataTable dtscrapItem = new DataTable();

            dtscrapItem.Columns.Add("item_id", typeof(string));
            dtscrapItem.Columns.Add("item_name", typeof(string));
            dtscrapItem.Columns.Add("sub_item", typeof(string));
            dtscrapItem.Columns.Add("uom", typeof(string));
            dtscrapItem.Columns.Add("UomName", typeof(string));
            dtscrapItem.Columns.Add("item_rate", typeof(string));
            dtscrapItem.Columns.Add("rec_qty", typeof(float));
            dtscrapItem.Columns.Add("mtr_type", typeof(string));
            dtscrapItem.Columns.Add("mtr_typeName", typeof(string));
            dtscrapItem.Columns.Add("wh_id_Scrap", typeof(int));
            dtscrapItem.Columns.Add("lot_no", typeof(string));
            dtscrapItem.Columns.Add("i_batch", typeof(string));
            dtscrapItem.Columns.Add("remarks", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                decimal Recev_qty;

                if (jObject[i]["ReceivQty"].ToString() == "")
                    Recev_qty = 0;
                else
                    Recev_qty = Convert.ToDecimal(jObject[i]["ReceivQty"].ToString());

                DataRow dtrowLines = dtscrapItem.NewRow();
                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom"] = jObject[i]["UOMID"].ToString();
                dtrowLines["UomName"] = jObject[i]["UomName"].ToString();
                dtrowLines["item_rate"] = jObject[i]["ItemRateScrap"].ToString();
                dtrowLines["rec_qty"] = Recev_qty;
                dtrowLines["mtr_type"] = jObject[i]["MtrTyp"].ToString();
                dtrowLines["mtr_typeName"] = jObject[i]["MtrTypName"].ToString();
                if (jObject[i]["WhID"].ToString() == "null")
                {
                    dtrowLines["wh_id_Scrap"] = "0";
                }
                else
                {
                    dtrowLines["wh_id_Scrap"] = Convert.ToInt32(jObject[i]["WhID"].ToString());
                }
                dtrowLines["lot_no"] = jObject[i]["lot_no"].ToString();
                dtrowLines["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowLines["remarks"] = jObject[i]["Remarks"].ToString();

                dtscrapItem.Rows.Add(dtrowLines);
            }
            return dtscrapItem;
        }
        public DataTable dtbatchdetail(JArray jObjectBatch)
        {
            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("batch_qty", typeof(float));
            Batch_detail.Columns.Add("reject_batch_qty", typeof(string));
            Batch_detail.Columns.Add("rework_batch_qty", typeof(string));
            Batch_detail.Columns.Add("exp_dt", typeof(string));

            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["item_id"].ToString();
                dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["batch_no"].ToString();
                dtrowBatchDetailsLines["batch_qty"] = jObjectBatch[i]["batch_qty"].ToString();
                dtrowBatchDetailsLines["reject_batch_qty"] = jObjectBatch[i]["reject_batch_qty"].ToString();
                dtrowBatchDetailsLines["rework_batch_qty"] = jObjectBatch[i]["rework_batch_qty"].ToString();
                if (jObjectBatch[i]["exp_dt"].ToString() == "" || jObjectBatch[i]["exp_dt"].ToString() == null)
                {
                    dtrowBatchDetailsLines["exp_dt"] = "01-Jan-1900";
                }
                else
                {
                    dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["exp_dt"].ToString();
                }
                Batch_detail.Rows.Add(dtrowBatchDetailsLines);
            }
            return Batch_detail;
        }
        public DataTable dtserialdetail(JArray jObjectSerial)
        {
            DataTable Serial_detail = new DataTable();
            Serial_detail.Columns.Add("item_id", typeof(string));
            Serial_detail.Columns.Add("serial_no", typeof(string));
            Serial_detail.Columns.Add("QtyType", typeof(string));

            for (int i = 0; i < jObjectSerial.Count; i++)
            {
                DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["item_id"].ToString();
                dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["serial_no"].ToString();
                dtrowSerialDetailsLines["QtyType"] = jObjectSerial[i]["QtyType"].ToString();
                Serial_detail.Rows.Add(dtrowSerialDetailsLines);
            }
            return Serial_detail;
        }
        public DataTable dtconsumebatchdetail(JArray jObjectBatch)
        {
            DataTable ConsumeBatch_detail = new DataTable();

            ConsumeBatch_detail.Columns.Add("item_id", typeof(string));
            ConsumeBatch_detail.Columns.Add("src_doc_no", typeof(string));
            ConsumeBatch_detail.Columns.Add("srcdate", typeof(string));
            ConsumeBatch_detail.Columns.Add("src_doc_dt", typeof(string));
            ConsumeBatch_detail.Columns.Add("wh_id", typeof(string));
            ConsumeBatch_detail.Columns.Add("wh_name", typeof(string));
            ConsumeBatch_detail.Columns.Add("lot_id", typeof(string));
            ConsumeBatch_detail.Columns.Add("batch_no", typeof(string));
            ConsumeBatch_detail.Columns.Add("expiry_date", typeof(DateTime));
            ConsumeBatch_detail.Columns.Add("exp_dt", typeof(DateTime));
            ConsumeBatch_detail.Columns.Add("res_qty", typeof(float));
            ConsumeBatch_detail.Columns.Add("cons_qty", typeof(float));

            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowCnsumBatchDetailsLines = ConsumeBatch_detail.NewRow();
                //dtrowCnsumBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                //dtrowCnsumBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                dtrowCnsumBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                dtrowCnsumBatchDetailsLines["src_doc_no"] = jObjectBatch[i]["SrcDocNo"].ToString();
                dtrowCnsumBatchDetailsLines["srcdate"] = jObjectBatch[i]["SrcDocDt"].ToString();
                dtrowCnsumBatchDetailsLines["src_doc_dt"] = jObjectBatch[i]["SrcDocDt"].ToString();
                dtrowCnsumBatchDetailsLines["wh_id"] = jObjectBatch[i]["WarehouseID"].ToString();
                dtrowCnsumBatchDetailsLines["wh_name"] = jObjectBatch[i]["Warehouse"].ToString();
                dtrowCnsumBatchDetailsLines["lot_id"] = jObjectBatch[i]["LotNo"].ToString();
                dtrowCnsumBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null || jObjectBatch[i]["ExpiryDate"].ToString() == "undefined")
                {
                    dtrowCnsumBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                }
                else
                {
                    dtrowCnsumBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                }
                dtrowCnsumBatchDetailsLines["exp_dt"] = jObjectBatch[i]["ExpiryDate"].ToString();
                dtrowCnsumBatchDetailsLines["res_qty"] = jObjectBatch[i]["ReserveQty"].ToString();
                dtrowCnsumBatchDetailsLines["cons_qty"] = jObjectBatch[i]["Consumed_Qty"].ToString();



                ConsumeBatch_detail.Rows.Add(dtrowCnsumBatchDetailsLines);
            }
            return ConsumeBatch_detail;
        }
        public DataTable dtscrapbatchdetail(JArray jObjectBatch)
        {
            DataTable ScrapBatch_detail = new DataTable();
            ScrapBatch_detail.Columns.Add("item_id", typeof(string));
            ScrapBatch_detail.Columns.Add("batch_no", typeof(string));
            ScrapBatch_detail.Columns.Add("batch_qty", typeof(float));
            ScrapBatch_detail.Columns.Add("exp_dt", typeof(string));

            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowscrapBatchDetails = ScrapBatch_detail.NewRow();
                dtrowscrapBatchDetails["item_id"] = jObjectBatch[i]["item_id"].ToString();
                dtrowscrapBatchDetails["batch_no"] = jObjectBatch[i]["batch_no"].ToString();
                dtrowscrapBatchDetails["batch_qty"] = jObjectBatch[i]["batch_qty"].ToString();
                if (jObjectBatch[i]["exp_dt"].ToString() == "" || jObjectBatch[i]["exp_dt"].ToString() == null)
                {
                    dtrowscrapBatchDetails["exp_dt"] = "01-Jan-1900";
                }
                else
                {
                    dtrowscrapBatchDetails["exp_dt"] = jObjectBatch[i]["exp_dt"].ToString();
                }
                ScrapBatch_detail.Rows.Add(dtrowscrapBatchDetails);
            }
            return ScrapBatch_detail;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_name", typeof(string));
            dtSubItem.Columns.Add("AccptQty", typeof(string));
            dtSubItem.Columns.Add("RejQty", typeof(string));
            dtSubItem.Columns.Add("RewrkQty", typeof(string));
            dtSubItem.Columns.Add("Issue_Qty", typeof(string));
            dtSubItem.Columns.Add("Consumed_Qty", typeof(string));
            dtSubItem.Columns.Add("Qty", typeof(string));
            dtSubItem.Columns.Add("SubItmTyp", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["sub_item_name"] = jObject2[i]["sub_item_name"].ToString();
                dtrowItemdetails["AccptQty"] = jObject2[i]["AccQty"].ToString();
                dtrowItemdetails["RejQty"] = jObject2[i]["RejQty"].ToString();
                dtrowItemdetails["RewrkQty"] = jObject2[i]["RewQty"].ToString();
                dtrowItemdetails["Issue_Qty"] = jObject2[i]["Issue_Qty"].ToString();
                dtrowItemdetails["Consumed_Qty"] = jObject2[i]["Consumed_Qty"].ToString();
                dtrowItemdetails["Qty"] = jObject2[i]["Qty"].ToString();
                dtrowItemdetails["SubItmTyp"] = jObject2[i]["SubItmTyp"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        public string CheckGRNAgainstInvoice(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel)
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
                string DocNo = _GoodReceiptNoteSCModel.GRNNumber;
                string DocDate = _GoodReceiptNoteSCModel.GRNDate;
                DataSet Deatils = _GoodReceiptNoteSC_IServices.CheckGRNAgainstInvoic(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "InvProcessed";
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
        private List<GoodsReceiptNoteSCList> getGRNSCList(GRNListModel _GRNListModel)
        {
            _GoodsReceiptNoteSCList = new List<GoodsReceiptNoteSCList>();
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
                if (_GRNListModel.WF_Status != null)
                {
                    wfstatus = _GRNListModel.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }

                DataSet DSet = _GoodReceiptNoteSC_IServices.GetGRNSCListandSrchDetail(CompID, BrchID, _GRNListModel, UserID, wfstatus, DocumentMenuId);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        GoodsReceiptNoteSCList _GRNList = new GoodsReceiptNoteSCList();
                        _GRNList.GRNNo = dr["GRNSCNo"].ToString();
                        _GRNList.GRNDate = dr["GRNDate"].ToString();
                        _GRNList.GRN_Date = dr["GRN_Dt"].ToString();
                        _GRNList.DeliveryNoteNo = dr["DeliveryNoteNo"].ToString();
                        _GRNList.DeliveryNoteDate = dr["DeliveryNoteDate"].ToString();
                        _GRNList.SuppName = dr["supp_name"].ToString();
                        _GRNList.Stauts = dr["Status"].ToString();
                        _GRNList.CreateDate = dr["CreateDate"].ToString();
                        _GRNList.ApproveDate = dr["ApproveDate"].ToString();
                        _GRNList.ModifyDate = dr["modDate"].ToString();
                        _GRNList.Create_By = dr["create_by"].ToString();
                        _GRNList.Mod_By = dr["mod_by"].ToString();
                        _GRNList.App_By = dr["app_by"].ToString();

                        _GoodsReceiptNoteSCList.Add(_GRNList);
                    }
                }

                //Session["FinStDt"] = DSet.Tables[1].Rows[0]["finstrdate"];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;

            }
            return _GoodsReceiptNoteSCList;
        }

        [HttpPost]
        public ActionResult SearchGRNSCListDetail(string SuppId, string Fromdate, string Todate, string Status)
        {
            GRNListModel _GRNListModel = new GRNListModel();
            try
            {
                //Session.Remove("WF_Docid");
                // Session.Remove("WF_status");
                _GRNListModel.WF_Status = null;
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
                _GoodsReceiptNoteSCList = new List<GoodsReceiptNoteSCList>();
                _GRNListModel.SuppID = SuppId;

                _GRNListModel.FromDate = Fromdate;
                _GRNListModel.ToDate = Todate;
                _GRNListModel.Status = Status;
                DataSet DSet = _GoodReceiptNoteSC_IServices.GetGRNSCListandSrchDetail(CompID, BrchID, _GRNListModel, "", "", "");
                //Session["GRNSCSearch"] = "GRNSC_Search";
                _GRNListModel.GRNSCSearch= "GRNSC_Search";
                foreach (DataRow dr in DSet.Tables[0].Rows)
                {
                    GoodsReceiptNoteSCList _GRNList = new GoodsReceiptNoteSCList();
                    _GRNList.GRNNo = dr["GRNSCNo"].ToString();
                    _GRNList.GRNDate = dr["GRNDate"].ToString();
                    _GRNList.GRN_Date = dr["GRN_Dt"].ToString();
                    _GRNList.DeliveryNoteNo = dr["DeliveryNoteNo"].ToString();
                    _GRNList.DeliveryNoteDate = dr["DeliveryNoteDate"].ToString();
                    _GRNList.SuppName = dr["supp_name"].ToString();
                    _GRNList.Stauts = dr["Status"].ToString();
                    _GRNList.CreateDate = dr["CreateDate"].ToString();
                    _GRNList.ApproveDate = dr["ApproveDate"].ToString();
                    _GRNList.ModifyDate = dr["modDate"].ToString();
                    _GRNList.Create_By = dr["create_by"].ToString();
                    _GRNList.Mod_By = dr["mod_by"].ToString();
                    _GRNList.App_By = dr["app_by"].ToString();
                    _GoodsReceiptNoteSCList.Add(_GRNList);
                }
                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                _GRNListModel.GRNSCList = _GoodsReceiptNoteSCList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialGoodReceiptNoteSCList.cshtml", _GRNListModel);
        }
        public ActionResult DoubleClickFromList(string DocNo, string DocDate, GoodReceiptNoteSCModel _GoodReceiptNoteSCModel, string ListFilterData,string WF_Status)
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
            _GoodReceiptNoteSCModel.Message = "New";
            _GoodReceiptNoteSCModel.Command = "Update";
            _GoodReceiptNoteSCModel.TransType = "Update";
            _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
            _GoodReceiptNoteSCModel.GRNNumber = DocNo;
            _GoodReceiptNoteSCModel.GRNDate = DocDate;
            if(WF_Status != null && WF_Status!= "")
            {
                _GoodReceiptNoteSCModel.WF_Status1 = WF_Status;
               
            }
            var WF_Status1 = _GoodReceiptNoteSCModel.WF_Status1;
            var GRNCodeURL = DocNo;
            var GRNDate = DocDate;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var command = "Add";

            TempData["ModelData"] = _GoodReceiptNoteSCModel;
            TempData["ListFilterData"] = ListFilterData;

            return (RedirectToAction("GoodReceiptNoteSCDetail", "GoodReceiptNoteSC", new { GRNCodeURL = GRNCodeURL, GRNDate, TransType, BtnName, command, WF_Status1 }));


        }
        public ActionResult DeleteGRNSCDetails(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel, string command)
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
                string GRNNo = _GoodReceiptNoteSCModel.GRNNumber;
                string GRNDelete = _GoodReceiptNoteSC_IServices.GRN_DeleteDetail(_GoodReceiptNoteSCModel, CompID, BrchID);

                if (!string.IsNullOrEmpty(GRNNo))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string DNNo1 = GRNNo.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, DNNo1, Server);
                }
                _GoodReceiptNoteSCModel = new GoodReceiptNoteSCModel();
                _GoodReceiptNoteSCModel.Message = "Deleted";
                _GoodReceiptNoteSCModel.Command = "Refresh";
                _GoodReceiptNoteSCModel.TransType = "Refresh";
                _GoodReceiptNoteSCModel.BtnName = "BtnDelete";
                TempData["ModelData"] = _GoodReceiptNoteSCModel;
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
        public ActionResult GRNApprove(GoodReceiptNoteSCModel _GoodReceiptNoteSCModel, string ListFilterData1)
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
                string GRN_No = _GoodReceiptNoteSCModel.GRNNumber;
                string GRN_Date = _GoodReceiptNoteSCModel.GRNDate;
                string A_Status = _GoodReceiptNoteSCModel.A_Status;
                string A_Level = _GoodReceiptNoteSCModel.A_Level;
                string A_Remarks = _GoodReceiptNoteSCModel.A_Remarks;

                string Message = _GoodReceiptNoteSC_IServices.GRNApproveDetails(CompID, BrchID, GRN_No, GRN_Date, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks);
                string GRNSC_No = Message.Split(',')[0].Trim();
                string StkCheck = Message.Split(',')[1].Trim();
                //if(StkCheck== "StkNotAvl")
                //{
                    //_GoodReceiptNoteSCModel.Message = "StockNotAvailable";
                    //_GoodReceiptNoteSCModel.TransType = "Update";
                    //_GoodReceiptNoteSCModel.Command = "Save";
                    //_GoodReceiptNoteSCModel.AppStatus = "D";
                    //_GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
                //}
                //else
                //{
                    string ApMessage = Message.Split(',')[2].Trim();
                    if (ApMessage == "A" || ApMessage == "PC")
                    {
                        _GoodReceiptNoteSCModel.Message = "Approved";
                    }
                    _GoodReceiptNoteSCModel.TransType = "Update";
                    _GoodReceiptNoteSCModel.Command = "Approve";
                    _GoodReceiptNoteSCModel.AppStatus = "D";
                    _GoodReceiptNoteSCModel.BtnName = "BtnEdit";

                //}

                var GRNCodeURL = GRNSC_No;
                var GRNDate = _GoodReceiptNoteSCModel.GRNDate;
                var TransType = _GoodReceiptNoteSCModel.TransType;
                var BtnName = _GoodReceiptNoteSCModel.BtnName;
                var command = _GoodReceiptNoteSCModel.Command;
                TempData["ModelData"] = _GoodReceiptNoteSCModel;
                TempData["ListFilterData"] = ListFilterData1;
                return (RedirectToAction("GoodReceiptNoteSCDetail", new{ GRNCodeURL = GRNCodeURL, GRNDate, TransType, BtnName, command }));


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1,string WF_Status)
        {
            GoodReceiptNoteSCModel _GoodReceiptNoteSCModel = new GoodReceiptNoteSCModel();

            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _GoodReceiptNoteSCModel.GRNNumber = jObjectBatch[i]["GRNNo"].ToString();
                    _GoodReceiptNoteSCModel.GRNDate = jObjectBatch[i]["GRNDate"].ToString();

                    _GoodReceiptNoteSCModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _GoodReceiptNoteSCModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _GoodReceiptNoteSCModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();

                }
            }
            if (_GoodReceiptNoteSCModel.A_Status != "Approve")
            {
                _GoodReceiptNoteSCModel.A_Status = "Approve";
            }
            GRNApprove(_GoodReceiptNoteSCModel, ListFilterData1);
            if(WF_Status != null && WF_Status !="")
            {
                _GoodReceiptNoteSCModel.WF_Status1 = WF_Status;
            }
            var WF_Status1 = _GoodReceiptNoteSCModel.WF_Status1;
            var GRNCodeURL = _GoodReceiptNoteSCModel.GRNNumber;
            var GRNDate = _GoodReceiptNoteSCModel.GRNDate;
            var TransType = _GoodReceiptNoteSCModel.TransType;
            var BtnName = _GoodReceiptNoteSCModel.BtnName;
            var command = _GoodReceiptNoteSCModel.Command;
            TempData["ModelData"] = _GoodReceiptNoteSCModel;
            return (RedirectToAction("GoodReceiptNoteSCDetail", new { GRNCodeURL = GRNCodeURL, GRNDate, TransType, BtnName, command, WF_Status1 }));


        }
        public ActionResult ToRefreshByJS(string FrwdDtList, string ListFilterData1,string WF_Status)
        {
            GoodReceiptNoteSCModel _GoodReceiptNoteSCModel = new GoodReceiptNoteSCModel();


            if (FrwdDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(FrwdDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _GoodReceiptNoteSCModel.GRNNumber = jObjectBatch[i]["GRNNo"].ToString();
                    _GoodReceiptNoteSCModel.GRNDate = jObjectBatch[i]["GRNDate"].ToString();
                    _GoodReceiptNoteSCModel.TransType = "Update";
                    _GoodReceiptNoteSCModel.BtnName = "BtnToDetailPage";
                    if (WF_Status != null && WF_Status != "")
                    {
                        _GoodReceiptNoteSCModel.WF_Status1 = WF_Status;
                    }
                  
                    TempData["ModelData"] = _GoodReceiptNoteSCModel;
                }
            }
            var WF_Status1 = _GoodReceiptNoteSCModel.WF_Status1;
            var GRNCodeURL = _GoodReceiptNoteSCModel.GRNNumber;
            var GRNDate = _GoodReceiptNoteSCModel.GRNDate;
            var TransType = _GoodReceiptNoteSCModel.TransType;
            var BtnName = _GoodReceiptNoteSCModel.BtnName;
            var command = "Refresh";
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("GoodReceiptNoteSCDetail", new { GRNCodeURL = GRNCodeURL, GRNDate, TransType, BtnName, command, WF_Status1 }));

        }
        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                GRNDetailsattch _GRNDetail = new GRNDetailsattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Guid gid = new Guid();
                gid = Guid.NewGuid();


                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _GRNDetail.Guid = DocNo;
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
                    _GRNDetail.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _GRNDetail.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _GRNDetail;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        /*-----------------------------Attachment End------------------------------------------*/
        /*-----------ConsumedBatchWise dEtail Start--------------------*/
        public ActionResult getOrderResrvItemStockBatchWise(string ItemId, string CompId, string BranchId
           , string SelectedItemdetail, string DMenuId, string Command, string TransType, string GRN_Status,string DN_NO,string HdnMessage)
        {
            try
            {
                DataSet ds = new DataSet();
                if (ItemId != "")
                {
                    if (Session["CompId"] != null)
                    {
                        CompId = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BranchId = Session["BranchId"].ToString();
                    }
                    ds = _GoodReceiptNoteSC_IServices.getOrderResrvItemStockBatchWise(ItemId, CompId, BranchId, DN_NO);
                }

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())//
                        {
                            if (item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim()
                                && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim()
                                && item.GetValue("BatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim()
                                && item.GetValue("SrcDocNo").ToString().Trim() == ds.Tables[0].Rows[i]["src_doc_no"].ToString().Trim()
                                //&& item.GetValue("SrcDocDate").ToString().Trim() == ds.Tables[0].Rows[i]["srcdate"].ToString().Trim()
                                && item.GetValue("WarehouseID").ToString().Trim() == ds.Tables[0].Rows[i]["wh_id"].ToString().Trim()
                                )
                            {
                                
                                ds.Tables[0].Rows[i]["cons_qty"] = item.GetValue("Consumed_Qty");
                            }
                            
                            //ds.Tables[0].Rows[i]["cons_qty"] = item.GetValue("Consumed_Qty");
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
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = GRN_Status;
                ViewBag.Message = HdnMessage;

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialConsumeMaterialBatchDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getOrderResrvItemStockBatchWiseOnDblClk(string GRNNo, string GRNDate, string ItemID
            , string DMenuId, string Command, string TransType, string DN_NO)
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
                ds = _GoodReceiptNoteSC_IServices.getOrderResrvItemStockBatchWiseOnDblClk(CompID, br_id, GRNNo, GRNDate, ItemID, DN_NO);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];

                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
               

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialConsumeMaterialBatchDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }


        /*-----------ConsumedBatchWise dEtail End--------------------*/
        public ActionResult GetGRNDashbordList(string docid, string status)
        {

            //  Session["WF_status"] = status;
            var WF_Status = status;
            return RedirectToAction("GoodReceiptNoteSC",new { WF_Status });
        }

        public ActionResult GetAutoCompleteSearchSuppList(GRNListModel _GRNListModel)
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
                if (string.IsNullOrEmpty(_GRNListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _GRNListModel.SuppName;
                }
                CustList = _GoodReceiptNoteSC_IServices.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in CustList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _GRNListModel.SupplierNameList = _SuppList;
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
        public JsonResult GetDeliveryNoteList(string Supp_id)
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
                DataSet result = _GoodReceiptNoteSC_IServices.GetDeliveryNoteList(Supp_id, Comp_ID, Br_ID);

                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";

                //myDataTable.Rows.InsertAt(newRow, 0);

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
        public JsonResult GetDeliveryNoteDetail(string DnNo)
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
                DataSet result = _GoodReceiptNoteSC_IServices.GetDeliveryNoteDetail(DnNo, Comp_ID, Br_ID);
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
        public JsonResult GetGRNSCDetails(string DNNo, string DNDate)
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
                DataSet result = _GoodReceiptNoteSC_IServices.GetGRNSCDetails(DNNo, DNDate, Comp_ID, Br_ID);
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
                DataSet result = _GoodReceiptNoteSC_IServices.GetWarehouseList(Comp_ID, Br_ID);
                dt = result.Tables[0];
                //DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                //return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
                //return Json("ErrorPage");
            }
            return dt;
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
                DataSet result = _GoodReceiptNoteSC_IServices.GetWarehouseList(Comp_ID, Br_ID);
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
        /*---------------------------------Sub-Item Start-------------------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
     , string Flag, string Status,string DNNo, string Doc_no, string Doc_dt)
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
                dt.Columns.Add("item_id", typeof(string));
                dt.Columns.Add("sub_item_id", typeof(string));
                dt.Columns.Add("sub_item_name", typeof(string));
                dt.Columns.Add("AccptQty", typeof(string));
                dt.Columns.Add("RejQty", typeof(string));
                dt.Columns.Add("RewrkQty", typeof(string));
                dt.Columns.Add("Issue_Qty", typeof(string));
                dt.Columns.Add("Consumed_Qty", typeof(string));
                dt.Columns.Add("Qty", typeof(string));
                dt.Columns.Add("ItmTyp", typeof(string));

                //if (Status == "D" || Status == "F" || Status == "")
                //{

                JArray arr = JArray.Parse(SubItemListwithPageData);
                int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");

                foreach (JObject item in arr.Children())//
                {
                    DataRow dRow = dt.NewRow();
                    
                    dRow["item_id"] = item.GetValue("item_id").ToString();
                    dRow["sub_item_id"] = item.GetValue("sub_item_id").ToString();
                    dRow["sub_item_name"] = item.GetValue("sub_item_name").ToString();
                    dRow["AccptQty"] = cmn.ConvertToDecimal(item.GetValue("Accqty").ToString(), DecDgt);
                    dRow["RejQty"] = cmn.ConvertToDecimal(item.GetValue("Rejqty").ToString(), DecDgt);
                    dRow["RewrkQty"] = cmn.ConvertToDecimal(item.GetValue("Rewqty").ToString(), DecDgt);
                    dRow["Issue_Qty"] = cmn.ConvertToDecimal(item.GetValue("Issue_Qty").ToString(), DecDgt);
                    dRow["Consumed_Qty"] = cmn.ConvertToDecimal(item.GetValue("Consumed_Qty").ToString(), DecDgt);
                    dRow["Qty"] = cmn.ConvertToDecimal(item.GetValue("Qty").ToString(), DecDgt);
                    dRow["ItmTyp"] = item.GetValue("SubItmTyp").ToString();
                    dt.Rows.Add(dRow);
                }
                //}
                //else
                //{
                //dt = _GoodReceiptNoteSC_IServices.GetSubItemDetailsFromDNSC(CompID, BrchID, Item_id, DNNo, Doc_no, Doc_dt, Flag,Status).Tables[0];
                if(Flag== "AccQuantity" || Flag == "RejQuantity" || Flag == "RewQuantity")
                {
                  Flag = "DNSC_QCAcptRejRwkQty";
                }
               else if (Flag == "GRNSCRecvQty")
                {
                    Flag = "Quantity";
                }
               else if (Flag == "GRNSCIssueQty") 
                {
                    Flag = "GRNSCIssueQty";
                }
                else
                {
                    Flag = "GRNSCConsumeQty";
                }

                //}
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    //_subitemPageName = "MTO",
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
        /*--------------------------For PDF Print Start--------------------------*/

        [HttpPost]
        public FileResult GenratePdfFile(GoodReceiptNoteSCModel _model)
        {
            return File(PdfFiledata(_model.GRNNumber, _model.GRNDate), "application/pdf", "GoodsRecieptNote.pdf");
        }
        public byte[] PdfFiledata(string grnNo, string grnDate)
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
                DataSet Details = _GoodReceiptNoteSC_IServices.GetGRNDeatilsForPrint(CompID, BrchID, grnNo, Convert.ToDateTime(grnDate).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                //ViewBag.InvoiceTo = "Invoice to:";
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.Title = "Goods Receipt Note";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["mr_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 07-04-2025*/
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/SubContracting/GoodReceiptNote/GoodsReceiptNoteSCPrint.cshtml"));

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
        /*--------------------------For PDF Print End--------------------------*/
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