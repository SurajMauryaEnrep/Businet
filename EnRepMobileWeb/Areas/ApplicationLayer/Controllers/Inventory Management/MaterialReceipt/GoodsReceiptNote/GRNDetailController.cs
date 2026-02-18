
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.GoodsReceiptNote;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.GoodsReceiptNote;
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

//namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialReceipt.GoodsReceiptNote
namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers
{
    public class GRNDetailController : Controller
    {
        DateTime FromDate;
        string title, branchID, GRN_no, language, UserID = string.Empty;
        List<GoodsReceiptNoteList> _GoodsReceiptNoteList;
        string DocumentMenuId = "105102115101";
        string CompID, BrchID = string.Empty;
        //DataSet  MenuDs;

        Logging.ErrorLog Errorlog = new Logging.ErrorLog();

        GoodsReceiptNote_ISERVICE _GoodsReceiptNote_ISERVICE;
        GoodsReceiptNoteList_ISERVICE _GoodsReceiptNoteList_ISERVICE;
        Common_IServices _Common_IServices;

        public GRNDetailController(Common_IServices _Common_IServices, GoodsReceiptNote_ISERVICE _GoodsReceiptNote_ISERVICE, GoodsReceiptNoteList_ISERVICE _GoodsReceiptNoteList_ISERVICE)
        {
            this._GoodsReceiptNote_ISERVICE = _GoodsReceiptNote_ISERVICE;
            this._GoodsReceiptNoteList_ISERVICE = _GoodsReceiptNoteList_ISERVICE;
            this._Common_IServices = _Common_IServices;
        }
        public ActionResult GRNList(GRN_ListModel _GRN_ListModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                if (DocumentMenuId != null)
                {
                    _GRN_ListModel.wfdocid = DocumentMenuId;
                }
                else
                {
                    _GRN_ListModel.wfdocid = "0";
                }
                
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, branchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                //GetAutoCompleteSearchSupplier(_GRN_ListModel);
              


                List<Status> statusLists = new List<Status>();
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _GRN_ListModel.StatusList = listOfStatus;

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                /*Commented By NItesh 22072025 Statr date */

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //string endDate = dtnow.ToString("yyyy-MM-dd");
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    _GRN_ListModel.SuppID = a[0].Trim();
                    _GRN_ListModel.SupplierID = a[0].Trim();
                    _GRN_ListModel.GRN_FromDate = a[1].Trim();
                    _GRN_ListModel.GRN_ToDate = a[2].Trim();
                    _GRN_ListModel.Status = a[3].Trim();
                    if (_GRN_ListModel.Status == "0")
                    {
                        _GRN_ListModel.Status = null;
                    }
                    _GRN_ListModel.FromDate = _GRN_ListModel.GRN_FromDate;
                    _GRN_ListModel.ListFilterData = TempData["ListFilterData"].ToString();
                }
                else
                {
                    _GRN_ListModel.FromDate = startDate;// FromDate.ToString("yyyy-MM-dd");
                    _GRN_ListModel.GRN_FromDate = startDate;// FromDate.ToString("yyyy-MM-dd");
                    _GRN_ListModel.GRN_ToDate = CurrentDate;// FromDate.ToString("yyyy-MM-dd");
                }
                GetAllData(_GRN_ListModel);/**Added By Nitesh 03-04-2024 Get AllData In One Procedure**/
                //_GRN_ListModel.GRNList = GetGRNDetailList(_GRN_ListModel);
                CommonPageDetails();
                // ViewBag.MenuPageName = getDocumentName();
                _GRN_ListModel.Title = title;
                //Session["GRNSearch"] = "0";
                _GRN_ListModel.GRNSearch = "0";
                // ViewBag.VBRoleList = GetRoleList();
               
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/GoodsReceiptNote/GoodsReceiptNoteList.cshtml", _GRN_ListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void GetAllData(GRN_ListModel _GRN_ListModel)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
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
                if (Session["UserID"] != null)
                {
                    UserID = Session["UserID"].ToString();
                }

                if (string.IsNullOrEmpty(_GRN_ListModel.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _GRN_ListModel.SuppName;
                }
               
                DataSet  SuppList1 = _GoodsReceiptNote_ISERVICE.GetAllData(Comp_ID, SupplierName, Br_ID,UserID, _GRN_ListModel.SuppID, _GRN_ListModel.GRN_FromDate, _GRN_ListModel.GRN_ToDate, _GRN_ListModel.Status, _GRN_ListModel.wfdocid, _GRN_ListModel.WF_status);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (DataRow data in SuppList1.Tables[0].Rows)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data["supp_id"].ToString();
                    _SuppDetail.supp_name = data["supp_name"].ToString();
                    _SuppList.Add(_SuppDetail);
                }
                _SuppList.Insert(0,new SupplierName() { supp_id="0",supp_name="All"});
                _GRN_ListModel.SupplierNameList = _SuppList;
                if (_GRN_ListModel.SupplierID != null && _GRN_ListModel.SupplierID != "")
                {
                    _GRN_ListModel.SuppID = _GRN_ListModel.SupplierID;
                }
                SetAllDataInListTabel(_GRN_ListModel, SuppList1);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        private void SetAllDataInListTabel(GRN_ListModel _GRN_ListModel,DataSet dt)
        {
            try
            {
                List<GoodsReceiptNoteList>  _GoodsReceiptNoteList = new List<GoodsReceiptNoteList>();
                if (dt.Tables[1].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[1].Rows)
                    {
                        GoodsReceiptNoteList _GRNList = new GoodsReceiptNoteList();
                        _GRNList.GRNNo = dr["GRNNo"].ToString();
                        _GRNList.GRNDate = dr["GRNDate"].ToString();
                        _GRNList.MrDate = dr["MrDate"].ToString();
                        _GRNList.DeliveryNoteNo = dr["DeliveryNoteNo"].ToString();
                        _GRNList.DeliveryNoteDate = dr["DeliveryNoteDate"].ToString();
                        _GRNList.SuppName = dr["supp_name"].ToString();
                        _GRNList.Stauts = dr["Status"].ToString();
                        _GRNList.CreateDate = dr["CreateDate"].ToString();
                        _GRNList.ApproveDate = dr["ApproveDate"].ToString();
                        _GRNList.ModifyDate = dr["modDate"].ToString();
                        _GRNList.create_by = dr["create_by"].ToString();
                        _GRNList.mod_by = dr["mod_by"].ToString();
                        _GRNList.app_by = dr["app_by"].ToString();
                        _GRNList.bill_no = dr["bill_no"].ToString();
                        _GRNList.bill_dt = dr["bill_dt"].ToString();
                        _GoodsReceiptNoteList.Add(_GRNList);
                    }

                }
                _GRN_ListModel.GRNList = _GoodsReceiptNoteList;
                if (dt.Tables[2].Rows.Count > 0)
                {
                    //FromDate = Convert.ToDateTime(dt.Tables[1].Rows[0]["finstrdate"]);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }
        public ActionResult GetGRNList(string docid, string status)
        {
            GRN_ListModel _GRN_ListModel = new GRN_ListModel();
            _GRN_ListModel.WF_status = status;
            //Session["WF_Docid"] = docid;
            //Session["WF_status"] = status;
            return RedirectToAction("GRNList", _GRN_ListModel);
        }
        public ActionResult AddNewGoodsReceiptNote()
        { /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("GRNList");
            }
           /*End to chk Financial year exist or not*/
            GoodsReceiptNoteModel GoodsReceiptNote = new GoodsReceiptNoteModel();
            GoodsReceiptNote.AppStatus = "D";
            GoodsReceiptNote.BtnName = "BtnAddNew";
            GoodsReceiptNote.TransType = "Save";
            GoodsReceiptNote.Command = "New";
            TempData["ModelData"] = GoodsReceiptNote;
            //Session["Message"] = null;
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnAddNew";
            //Session["TransType"] = "Save";
            //Session["Command"] = "New";
            TempData["ListFilterData"] = null;
            return RedirectToAction("GRNDetail", "GRNDetail");
        }
        public ActionResult EditGRN(string grn_no, string grn_dt, string ListFilterData, string WF_status)
        { /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
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


            GoodsReceiptNoteModel GoodsReceiptNote = new GoodsReceiptNoteModel();
            URLModelDetails URLModel = new URLModelDetails();
            GoodsReceiptNote.Message = "New";
            GoodsReceiptNote.Command = "Add";
            GoodsReceiptNote.grn_no = grn_no;
            GoodsReceiptNote.grn_dt = grn_dt;
            GoodsReceiptNote.TransType = "Update";
            GoodsReceiptNote.AppStatus = "D";
            GoodsReceiptNote.BtnName = "BtnToDetailPage";
            GoodsReceiptNote.WF_status1 = WF_status;
            TempData["ModelData"] = GoodsReceiptNote;
            TempData["ListFilterData"] = ListFilterData;
            URLModel.grn_no = grn_no;
            URLModel.grn_dt = grn_dt;
            URLModel.TransType = "Update";
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnToDetailPage";
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["GRN_No"] = grn_no;
            //Session["GRN_Date"] = grn_dt;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("GRNDetail", URLModel);
        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string TrancType, string Mailerror)
        {
            //Session["Message"] = "";
            GoodsReceiptNoteModel GoodsReceiptNote = new GoodsReceiptNoteModel();
            var a = TrancType.Split(',');
            GoodsReceiptNote.grn_no = a[0].Trim();
            GoodsReceiptNote.grn_dt = a[1].Trim();
            GoodsReceiptNote.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            GoodsReceiptNote.BtnName = "BtnToDetailPage";
            GoodsReceiptNote.Message = Mailerror;
            URLModelDetails URLModel = new URLModelDetails();
            URLModel.grn_no = a[0].Trim();
            URLModel.grn_dt = a[1].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = GoodsReceiptNote;
            TempData["WF_status1"] = WF_status1;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("GRNDetail", URLModel);
        }
        public ActionResult GRNDetail(URLModelDetails URLModel)
        {
            ViewBag.DocumentMenuId = DocumentMenuId;
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
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.grn_dt) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                //string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));

                //ViewBag.ValDigit = ValDigit;
                //ViewBag.QtyDigit = QtyDigit;
                //ViewBag.RateDigit = RateDigit;

                

                var GoodsReceiptNote = TempData["ModelData"] as GoodsReceiptNoteModel;
                if (GoodsReceiptNote != null)
                {
                    SetDecimals(GoodsReceiptNote);
                    var other = new CommonController(_Common_IServices);

                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    GetAutoCompleteSupplierName(GoodsReceiptNote);

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.dn_no = "---Select---";
                    _DocumentNumber.dn_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);

                    GoodsReceiptNote.DeliveryNoteDateList = _DocumentNumberList;
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
                    GoodsReceiptNote.WarehouseList = requirementAreaLists;


                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        GoodsReceiptNote.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        GoodsReceiptNote.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (GoodsReceiptNote.Message != "Used")
                    {
                        GoodsReceiptNote.bill_dt = null;
                        GoodsReceiptNote.DeliveryNoteDate = null;
                    }


                    if (GoodsReceiptNote.Message == "Used")
                    {
                        CommonPageDetails();
                        // ViewBag.MenuPageName = getDocumentName();
                        GoodsReceiptNote.Title = title;
                        GoodsReceiptNote.DocumentStatus = "";
                        _DocumentNumberList.Add(new DocumentNumber { dn_no = GoodsReceiptNote.DeliveryNoteNumber, dn_dt = GoodsReceiptNote.DeliveryNoteNumber });
                        GoodsReceiptNote.DeliveryNoteDateList = _DocumentNumberList;
                        DataSet ds = new DataSet();
                        ds = GoodsReceiptNote.DtSet;
                        ViewBag.ItemDetails = ds.Tables[0];
                        GoodsReceiptNote.BtnName = "Refresh";
                        GoodsReceiptNote.BatchDetail = DataTableToJSONWithStringBuilder(ds.Tables[1]);
                        GoodsReceiptNote.SerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        // ViewBag.VBRoleList = GetRoleList();
                      
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/GoodsReceiptNote/GoodsReceiptNoteDetail.cshtml", GoodsReceiptNote);
                    }
                    else if (GoodsReceiptNote.TransType == "Update" || GoodsReceiptNote.TransType == "Edit")
                    {
                        CommonPageDetails();
                        GoodsReceiptNote.Title = title;
                        string GRN_Date = GoodsReceiptNote.grn_dt;
                        string GRN_No = GoodsReceiptNote.grn_no;
                        string Type = "GRN";
                        //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        DataSet ds = _GoodsReceiptNote_ISERVICE.Edit_GRNDetail(CompID, BrchID, GRN_No, GRN_Date, UserID, DocumentMenuId, Type);
                        ViewBag.AttechmentDetails = ds.Tables[7];
                        ViewBag.SubItemDetails = ds.Tables[8];
                        ViewBag.ItemTaxDetails = ds.Tables[10];
                        ViewBag.OtherChargeDetails = ds.Tables[11];
                        ViewBag.OCTaxDetails = ds.Tables[12];

                        ViewBag.HsnListData = ds.Tables[13];

                        GoodsReceiptNote.grn_no = ds.Tables[0].Rows[0]["mr_no"].ToString();
                        GoodsReceiptNote.grn_dt = ds.Tables[0].Rows[0]["MrDate"].ToString();
                        GoodsReceiptNote.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        GoodsReceiptNote.bill_dt = ds.Tables[0].Rows[0]["BillDate"].ToString();
                        GoodsReceiptNote.supp_id = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        GoodsReceiptNote.DeliveryNoteNumber = ds.Tables[0].Rows[0]["doc_no"].ToString();
                        GoodsReceiptNote.DeliveryNoteDate = ds.Tables[0].Rows[0]["DocDate"].ToString();
                        GoodsReceiptNote.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        GoodsReceiptNote.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        GoodsReceiptNote.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        GoodsReceiptNote.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        GoodsReceiptNote.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        GoodsReceiptNote.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        GoodsReceiptNote.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        GoodsReceiptNote.Status = ds.Tables[0].Rows[0]["status_name"].ToString();
                        GoodsReceiptNote.wh_id = ds.Tables[1].Rows[0]["wh_id"].ToString();
                        GoodsReceiptNote.GrossValue = ds.Tables[0].Rows[0]["gr_val"].ToString();
                        GoodsReceiptNote.TaxAmountNonRecoverable = ds.Tables[0].Rows[0]["tax_amt_nrecov"].ToString();
                        GoodsReceiptNote.TaxAmountRecoverable = ds.Tables[0].Rows[0]["tax_amt_recov"].ToString();
                        GoodsReceiptNote.OtherCharges = ds.Tables[0].Rows[0]["oc_amt"].ToString();
                        GoodsReceiptNote.NetMRValue = ds.Tables[0].Rows[0]["net_val"].ToString();
                        GoodsReceiptNote.NetLandedValue = ds.Tables[0].Rows[0]["landed_val"].ToString();
                        GoodsReceiptNote.ConvRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        GoodsReceiptNote.CurrId = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        GoodsReceiptNote.BsCurrId = ds.Tables[0].Rows[0]["curr_id"].ToString();//Added by Suraj Maurya on 06-03-2025 for getting base currency
                        GoodsReceiptNote.GRNumber = ds.Tables[0].Rows[0]["gr_no"].ToString();
                        GoodsReceiptNote.OrderType = ds.Tables[0].Rows[0]["order_type"].ToString();
                        SetDecimals(GoodsReceiptNote);
                        ViewBag.CostingDetails = ds.Tables[9];
                        if (ds.Tables[9].Rows.Count > 0)
                        {
                            GoodsReceiptNote.CurrId = ds.Tables[9].Rows[0]["curr_id"].ToString();
                            GoodsReceiptNote.CurrName = ds.Tables[9].Rows[0]["curr_name"].ToString();
                            GoodsReceiptNote.ConvRate = ds.Tables[9].Rows[0]["conv_rate"].ToString();
                            //GoodsReceiptNote.OrderType = ds.Tables[9].Rows[0]["order_type"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["gr_date"].ToString() == "")
                        {

                        }
                        else
                        {
                            GoodsReceiptNote.GRDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["gr_date"].ToString());
                        }
                        if (ds.Tables[0].Rows[0]["freight_amt"].ToString() == "")
                        {
                            GoodsReceiptNote.FreightAmount = Convert.ToDecimal(0).ToString(GoodsReceiptNote.RateDigit);
                        }
                        else
                        {
                            GoodsReceiptNote.FreightAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["freight_amt"]).ToString(GoodsReceiptNote.RateDigit);
                        }
                        GoodsReceiptNote.TransporterName = ds.Tables[0].Rows[0]["trans_name"].ToString();
                        GoodsReceiptNote.VehicleNumber = ds.Tables[0].Rows[0]["veh_no"].ToString();
                        if (ds.Tables[0].Rows[0]["veh_load"].ToString() == "")
                        {
                            GoodsReceiptNote.veh_load = Convert.ToDecimal(0).ToString(GoodsReceiptNote.RateDigit);
                        }
                        else
                        {
                            GoodsReceiptNote.veh_load = Convert.ToDecimal(ds.Tables[0].Rows[0]["veh_load"]).ToString(GoodsReceiptNote.RateDigit);
                        }
                        GoodsReceiptNote.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                        GoodsReceiptNote.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                        //if (ds.Tables[8].Rows.Count > 0)
                        //{
                        //    GoodsReceiptNote.VouType = ds.Tables[8].Rows[0]["vou_type"].ToString();
                        //    GoodsReceiptNote.VouNo = ds.Tables[8].Rows[0]["vou_no"].ToString();
                        //    GoodsReceiptNote.VouDt = ds.Tables[8].Rows[0]["vou_dt"].ToString();
                        //}

                        GoodsReceiptNote.BatchDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        GoodsReceiptNote.SerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        _DocumentNumber.dn_no = GoodsReceiptNote.DeliveryNoteNumber;
                        _DocumentNumber.dn_dt = GoodsReceiptNote.DeliveryNoteNumber;
                        _DocumentNumberList.Add(_DocumentNumber);
                        GoodsReceiptNote.DeliveryNoteDateList = _DocumentNumberList;

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        GoodsReceiptNote.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        GoodsReceiptNote.DocumentStatus = doc_status;

                        if (GoodsReceiptNote.Status == "Cancelled")
                        {
                            GoodsReceiptNote.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            GoodsReceiptNote.BtnName = "Refresh";
                            GoodsReceiptNote.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                        }
                        else
                        {
                            GoodsReceiptNote.CancelFlag = false;
                        }
                        GoodsReceiptNote.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        GoodsReceiptNote.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && GoodsReceiptNote.Command != "Edit")
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
                                    GoodsReceiptNote.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
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
                                        GoodsReceiptNote.BtnName = "BtnToDetailPage";
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
                                        GoodsReceiptNote.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    GoodsReceiptNote.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
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
                                        GoodsReceiptNote.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    GoodsReceiptNote.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
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
                                    GoodsReceiptNote.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A"|| doc_status == "PC")
                            {
                                //string CostingDetailAccss = "N";
                                //if(ViewBag.VBRoleList != null)
                                //{
                                //    foreach (System.Data.DataRow Row in ViewBag.VBRoleList.Rows)
                                //    {
                                //        if (Row["feature_id"].ToString() == "105102115101011")
                                //        {
                                //            CostingDetailAccss = "Y";
                                //        }
                                //    } 
                                //}
                                if (create_id == UserID || approval_id == UserID /*|| CostingDetailAccss=="Y"*/)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    GoodsReceiptNote.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    GoodsReceiptNote.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        // ViewBag.MenuPageName = getDocumentName();
                        
                        ViewBag.ItemDetails = ds.Tables[1];

                        //ViewBag.VoucherDetails = ds.Tables[8];
                        //ViewBag.hdGLDetails = ds.Tables[9];
                        //ViewBag.VoucherTotal = ds.Tables[10];

                        //  ViewBag.VBRoleList = GetRoleList();
                       
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/GoodsReceiptNote/GoodsReceiptNoteDetail.cshtml", GoodsReceiptNote);
                    }
                    else
                    {
                        // ViewBag.MenuPageName = getDocumentName();
                        CommonPageDetails();
                        GoodsReceiptNote.Title = title;
                        //Session["DocumentStatus"] = "";
                        GoodsReceiptNote.DocumentStatus = "";
                        //  ViewBag.VBRoleList = GetRoleList();
                        
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/GoodsReceiptNote/GoodsReceiptNoteDetail.cshtml", GoodsReceiptNote);
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
                    GoodsReceiptNoteModel GoodsReceiptNote1 = new GoodsReceiptNoteModel();
                    SetDecimals(GoodsReceiptNote1);
                    if (URLModel.grn_no != null || URLModel.grn_dt != null)
                    {
                        GoodsReceiptNote1.grn_no = URLModel.grn_no;
                        GoodsReceiptNote1.grn_dt = URLModel.grn_dt;
                    }
                    if (URLModel.TransType != null)
                    {
                        GoodsReceiptNote1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        GoodsReceiptNote1.TransType = "New";
                    }
                    if (URLModel.BtnName != null)
                    {
                        GoodsReceiptNote1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        GoodsReceiptNote1.BtnName = "BtnRefresh";
                    }
                    if (URLModel.Command != null)
                    {
                        GoodsReceiptNote1.Command = URLModel.Command;
                    }
                    else
                    {
                        GoodsReceiptNote1.Command = "Refresh";
                    }
                    var other = new CommonController(_Common_IServices);

                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    GetAutoCompleteSupplierName(GoodsReceiptNote1);

                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.dn_no = "---Select---";
                    _DocumentNumber.dn_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);

                    GoodsReceiptNote1.DeliveryNoteDateList = _DocumentNumberList;
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
                    GoodsReceiptNote1.WarehouseList = requirementAreaLists;

                    //GoodsReceiptNote1.ItemDetailsList = null;
                    GoodsReceiptNote1.bill_dt = null;
                    //GoodsReceiptNote1.grn_dt = DateTime.Now;
                    GoodsReceiptNote1.DeliveryNoteDate = null;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        GoodsReceiptNote1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        GoodsReceiptNote1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (GoodsReceiptNote1.TransType == "Update" || GoodsReceiptNote1.TransType == "Edit")
                    {
                        CommonPageDetails();
                        GoodsReceiptNote1.Title = title;
                        //string GRN_Date = Session["GRN_Date"].ToString();
                        //string GRN_No = Session["GRN_No"].ToString();
                        string GRN_Date = GoodsReceiptNote1.grn_dt;
                        string GRN_No = GoodsReceiptNote1.grn_no;
                        string Type = "GRN";
                        //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                        DataSet ds = _GoodsReceiptNote_ISERVICE.Edit_GRNDetail(CompID, BrchID, GRN_No, GRN_Date, UserID, DocumentMenuId, Type);
                        ViewBag.AttechmentDetails = ds.Tables[7];
                        ViewBag.SubItemDetails = ds.Tables[8];
                        ViewBag.ItemTaxDetails = ds.Tables[10];
                        ViewBag.OtherChargeDetails = ds.Tables[11];
                        ViewBag.OCTaxDetails = ds.Tables[12];

                        ViewBag.HsnListData = ds.Tables[13];

                        GoodsReceiptNote1.grn_no = ds.Tables[0].Rows[0]["mr_no"].ToString();
                        GoodsReceiptNote1.grn_dt = ds.Tables[0].Rows[0]["MrDate"].ToString();
                        GoodsReceiptNote1.bill_no = ds.Tables[0].Rows[0]["bill_no"].ToString();
                        GoodsReceiptNote1.bill_dt = ds.Tables[0].Rows[0]["BillDate"].ToString();
                        GoodsReceiptNote1.supp_id = ds.Tables[0].Rows[0]["supp_id"].ToString();
                        GoodsReceiptNote1.DeliveryNoteNumber = ds.Tables[0].Rows[0]["doc_no"].ToString();
                        GoodsReceiptNote1.DeliveryNoteDate = ds.Tables[0].Rows[0]["DocDate"].ToString();
                        GoodsReceiptNote1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        GoodsReceiptNote1.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        GoodsReceiptNote1.Createdon = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        GoodsReceiptNote1.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        GoodsReceiptNote1.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        GoodsReceiptNote1.AmendedBy = ds.Tables[0].Rows[0]["ModifyName"].ToString();
                        GoodsReceiptNote1.AmendedOn = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                        GoodsReceiptNote1.Status = ds.Tables[0].Rows[0]["status_name"].ToString();
                        GoodsReceiptNote1.wh_id = ds.Tables[1].Rows[0]["wh_id"].ToString();
                        GoodsReceiptNote1.GrossValue = ds.Tables[0].Rows[0]["gr_val"].ToString();
                        GoodsReceiptNote1.TaxAmountNonRecoverable = ds.Tables[0].Rows[0]["tax_amt_nrecov"].ToString();
                        GoodsReceiptNote1.TaxAmountRecoverable = ds.Tables[0].Rows[0]["tax_amt_recov"].ToString();
                        GoodsReceiptNote1.OtherCharges = ds.Tables[0].Rows[0]["oc_amt"].ToString();
                        GoodsReceiptNote1.NetMRValue = ds.Tables[0].Rows[0]["net_val"].ToString();
                        GoodsReceiptNote1.NetLandedValue = ds.Tables[0].Rows[0]["landed_val"].ToString();
                        GoodsReceiptNote1.ConvRate = ds.Tables[0].Rows[0]["conv_rate"].ToString();
                        GoodsReceiptNote1.CurrId = ds.Tables[0].Rows[0]["curr_id"].ToString();
                        GoodsReceiptNote1.BsCurrId = ds.Tables[0].Rows[0]["curr_id"].ToString();//Added by Suraj Maurya on 06-03-2025 for getting base currency
                        GoodsReceiptNote1.EWBNNumber = ds.Tables[0].Rows[0]["ewb_no"].ToString();
                        GoodsReceiptNote1.EInvoive = ds.Tables[0].Rows[0]["einv_no"].ToString();
                        GoodsReceiptNote1.GRNumber = ds.Tables[0].Rows[0]["gr_no"].ToString();
                        GoodsReceiptNote1.OrderType = ds.Tables[0].Rows[0]["order_type"].ToString();
                        SetDecimals(GoodsReceiptNote1);
                        ViewBag.CostingDetails = ds.Tables[9];
                        if (ds.Tables[9].Rows.Count > 0)
                        {
                            GoodsReceiptNote1.CurrId = ds.Tables[9].Rows[0]["curr_id"].ToString();
                            GoodsReceiptNote1.CurrName = ds.Tables[9].Rows[0]["curr_name"].ToString();
                            GoodsReceiptNote1.ConvRate = ds.Tables[9].Rows[0]["conv_rate"].ToString();
                            //GoodsReceiptNote1.OrderType = ds.Tables[9].Rows[0]["order_type"].ToString();
                        }
                        if (ds.Tables[0].Rows[0]["gr_date"].ToString() == "")
                        {

                        }
                        else
                        {
                            GoodsReceiptNote1.GRDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["gr_date"].ToString());
                        }
                        if (ds.Tables[0].Rows[0]["freight_amt"].ToString() == "")
                        {
                            GoodsReceiptNote1.FreightAmount = Convert.ToDecimal(0).ToString(GoodsReceiptNote1.RateDigit);
                        }
                        else
                        {
                            GoodsReceiptNote1.FreightAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["freight_amt"]).ToString(GoodsReceiptNote1.RateDigit);
                        }
                        GoodsReceiptNote1.TransporterName = ds.Tables[0].Rows[0]["trans_name"].ToString();
                        GoodsReceiptNote1.VehicleNumber = ds.Tables[0].Rows[0]["veh_no"].ToString();
                        if (ds.Tables[0].Rows[0]["veh_load"].ToString() == "")
                        {
                            GoodsReceiptNote1.veh_load = Convert.ToDecimal(0).ToString(GoodsReceiptNote1.RateDigit);
                        }
                        else
                        {
                            GoodsReceiptNote1.veh_load = Convert.ToDecimal(ds.Tables[0].Rows[0]["veh_load"]).ToString(GoodsReceiptNote1.RateDigit);
                        }

                        //if (ds.Tables[8].Rows.Count > 0)
                        //{
                        //    GoodsReceiptNote1.VouType = ds.Tables[8].Rows[0]["vou_type"].ToString();
                        //    GoodsReceiptNote1.VouNo = ds.Tables[8].Rows[0]["vou_no"].ToString();
                        //    GoodsReceiptNote1.VouDt = ds.Tables[8].Rows[0]["vou_dt"].ToString();
                        //}

                        GoodsReceiptNote1.BatchDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        GoodsReceiptNote1.SerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        _DocumentNumber.dn_no = GoodsReceiptNote1.DeliveryNoteNumber;
                        _DocumentNumber.dn_dt = GoodsReceiptNote1.DeliveryNoteNumber;
                        _DocumentNumberList.Add(_DocumentNumber);
                        GoodsReceiptNote1.DeliveryNoteDateList = _DocumentNumberList;

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        GoodsReceiptNote1.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        GoodsReceiptNote1.DocumentStatus = doc_status;

                        if (GoodsReceiptNote1.Status == "Cancelled")
                        {
                            GoodsReceiptNote1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            GoodsReceiptNote1.BtnName = "Refresh";
                            GoodsReceiptNote1.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                        }
                        else
                        {
                            GoodsReceiptNote1.CancelFlag = false;
                        }
                        GoodsReceiptNote1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        GoodsReceiptNote1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && GoodsReceiptNote1.Command != "Edit")
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
                                    GoodsReceiptNote1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
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
                                        GoodsReceiptNote1.BtnName = "BtnToDetailPage";
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
                                        GoodsReceiptNote1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    GoodsReceiptNote1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
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
                                        GoodsReceiptNote1.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    GoodsReceiptNote1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
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
                                    GoodsReceiptNote1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A" || doc_status == "PC")
                            {
                                //string CostingDetailAccss = "N";
                                //if (ViewBag.VBRoleList != null)
                                //{
                                //    foreach (System.Data.DataRow Row in ViewBag.VBRoleList.Rows)
                                //    {
                                //        if (Row["feature_id"].ToString() == "105102115101011")
                                //        {
                                //            CostingDetailAccss = "Y";
                                //        }
                                //    }
                                //}
                                //if (create_id == UserID || approval_id == UserID || CostingDetailAccss == "Y")
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    GoodsReceiptNote1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    GoodsReceiptNote1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                      
                        ViewBag.ItemDetails = ds.Tables[1];

                        //ViewBag.VoucherDetails = ds.Tables[8];
                        //ViewBag.hdGLDetails = ds.Tables[9];
                        //ViewBag.VoucherTotal = ds.Tables[10];

                        //   ViewBag.VBRoleList = GetRoleList();
                       
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/GoodsReceiptNote/GoodsReceiptNoteDetail.cshtml", GoodsReceiptNote1);
                    }
                    else
                    {
                        CommonPageDetails();
                        // ViewBag.MenuPageName = getDocumentName();
                        GoodsReceiptNote1.Title = title;
                        //Session["DocumentStatus"] = "";
                        GoodsReceiptNote1.DocumentStatus = "";
                        // ViewBag.VBRoleList = GetRoleList();
                        
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/GoodsReceiptNote/GoodsReceiptNoteDetail.cshtml", GoodsReceiptNote1);
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
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
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
                //foreach (DataRow Row in ds.Tables[1].Rows)
                //{
                //    //if (Row["param_id"].ToString() == "101")
                //    if (Row["param_desc"].ToString().Trim() == "GST Applicable")
                //    {
                //        ViewBag.GstApplicable = Row["param_stat"].ToString();
                //    }
                //    if (Row["param_id"].ToString() == "111")
                //    {
                //        ViewBag.UpdateItemPriceOnGrn = Row["param_stat"].ToString();
                //    }
                //}
                ViewBag.GstApplicable = ds.Tables[7].Rows.Count > 0 ? ds.Tables[7].Rows[0]["param_stat"].ToString() : "";
                ViewBag.UpdateItemPriceOnGrn = ds.Tables[10].Rows.Count > 0 ? ds.Tables[10].Rows[0]["param_stat"].ToString() : "";
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
        public ActionResult GoodsReceiptNoteSave(GoodsReceiptNoteModel GoodsReceiptNote, string command)
        {
            try
            {/*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (GoodsReceiptNote.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        GoodsReceiptNoteModel GoodsReceiptNoteAddNew = new GoodsReceiptNoteModel();
                        GoodsReceiptNoteAddNew.AppStatus = "D";
                        GoodsReceiptNoteAddNew.BtnName = "BtnAddNew";
                        GoodsReceiptNoteAddNew.TransType = "Save";
                        GoodsReceiptNoteAddNew.Command = "New";
                        TempData["ModelData"] = GoodsReceiptNoteAddNew;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                              if (!string.IsNullOrEmpty(GoodsReceiptNote.grn_no))
                                return RedirectToAction("EditGRN", new { grn_no = GoodsReceiptNote.grn_no, grn_dt = GoodsReceiptNote.grn_dt, ListFilterData = GoodsReceiptNote.ListFilterData1, WF_status = GoodsReceiptNote.WFStatus });
                            else
                                GoodsReceiptNoteAddNew.Command = "Refresh";
                            GoodsReceiptNoteAddNew.TransType = "Refresh";
                            GoodsReceiptNoteAddNew.BtnName = "Refresh";
                            GoodsReceiptNoteAddNew.DocumentStatus = null;
                            TempData["ModelData"] = GoodsReceiptNoteAddNew;
                            return RedirectToAction("GRNDetail", "GRNDetail", GoodsReceiptNoteAddNew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("GRNDetail", "GRNDetail");

                    case "Edit":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditGRN", new { grn_no = GoodsReceiptNote.grn_no, grn_dt = GoodsReceiptNote.grn_dt, ListFilterData = GoodsReceiptNote.ListFilterData1, WF_status = GoodsReceiptNote.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string GRNDt = GoodsReceiptNote.grn_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, GRNDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditGRN", new { grn_no = GoodsReceiptNote.grn_no, grn_dt = GoodsReceiptNote.grn_dt, ListFilterData = GoodsReceiptNote.ListFilterData1, WF_status = GoodsReceiptNote.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        URLModelDetails URLModel = new URLModelDetails();
                        if (CheckInvoiceAgainstGRN(GoodsReceiptNote.grn_no, GoodsReceiptNote.grn_dt) == "InvoiceCreated")
                        {
                            GoodsReceiptNote.Message = "InvoiceCreated";
                            GoodsReceiptNote.TransType = "Update";
                            GoodsReceiptNote.Command = "Add";
                            //GoodsReceiptNote.DocumentStatus = GoodsReceiptNote.Status;
                            GoodsReceiptNote.BtnName = "BtnToDetailPage";
                            //TempData["ListFilterData"] = GoodsReceiptNote.ListFilterData1;
                            //TempData["ModelData"] = GoodsReceiptNote;
                        }
                        else
                        {
                            GoodsReceiptNote.TransType = "Update";
                            GoodsReceiptNote.Command = command;
                            GoodsReceiptNote.BtnName = "BtnEdit";
                            GoodsReceiptNote.Message = null;
                                                    
                            URLModel.grn_no = GoodsReceiptNote.grn_no;
                            URLModel.grn_dt = GoodsReceiptNote.grn_dt;
                            URLModel.TransType = "Update";
                            URLModel.BtnName = "BtnEdit";
                            URLModel.Command = command;
                           
                        }
                        TempData["ListFilterData"] = GoodsReceiptNote.ListFilterData1;
                        TempData["ModelData"] = GoodsReceiptNote;
                        return RedirectToAction("GRNDetail", URLModel);

                    case "Delete":
                        GoodsReceiptNoteModel GoodsReceiptNoteDelete = new GoodsReceiptNoteModel();
                        string MRType = "GRN";
                        DeleteGRNDetails(GoodsReceiptNote, MRType);
                        GoodsReceiptNoteDelete.Message = "Deleted";
                        GoodsReceiptNoteDelete.Command = "Refresh";
                        GoodsReceiptNoteDelete.TransType = "Refresh";
                        GoodsReceiptNoteDelete.AppStatus = "D";
                        GoodsReceiptNoteDelete.BtnName = "BtnDelete";
                        TempData["ModelData"] = GoodsReceiptNoteDelete;
                        TempData["ListFilterData"] = GoodsReceiptNote.ListFilterData1;
                        return RedirectToAction("GRNDetail");

                    case "Save":
                        GoodsReceiptNote.Command = command;
                        if (GoodsReceiptNote.TransType == null)
                        {
                            GoodsReceiptNote.TransType = command;
                        }
                        if (GoodsReceiptNote.doc_status == "PC")
                        {
                            SaveGRNCostingDetail(GoodsReceiptNote);
                        }
                        else
                        {
                            SaveGoodsRecipteNote(GoodsReceiptNote, GoodsReceiptNote.Title);
                        }                    
                        if (GoodsReceiptNote.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        if (GoodsReceiptNote.Message == "DocModify")
                        {
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            ViewBag.DocumentStatus = "D";
                           
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);

                            List<SupplierName> suppLists = new List<SupplierName>();
                            suppLists.Add(new SupplierName { supp_id = GoodsReceiptNote.supp_id, supp_name = GoodsReceiptNote.SuppName });
                            GoodsReceiptNote.SupplierNameList = suppLists;

                            List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                            DocumentNumber _DocumentNumber = new DocumentNumber();
                            _DocumentNumber.dn_no = GoodsReceiptNote.DeliveryNoteNumber;
                            _DocumentNumber.dn_dt = "0";
                            _DocumentNumberList.Add(_DocumentNumber);

                            GoodsReceiptNote.DeliveryNoteDateList = _DocumentNumberList;
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
                            requirementAreaLists.Insert(0, new Warehouse() { wh_id = "0", wh_name = GoodsReceiptNote.GRNwhname });
                            GoodsReceiptNote.WarehouseList = requirementAreaLists;
                            GoodsReceiptNote.grn_dt = DateTime.Now.ToString();
                            GoodsReceiptNote.bill_no = GoodsReceiptNote.bill_no;
                            GoodsReceiptNote.bill_dt = GoodsReceiptNote.bill_dt;
                            GoodsReceiptNote.SuppName = GoodsReceiptNote.SuppName;
                            GoodsReceiptNote.DeliveryNoteNumber = GoodsReceiptNote.DeliveryNoteNumber;
                            GoodsReceiptNote.DeliveryNoteDate = GoodsReceiptNote.DeliveryNoteDate;
                            var dttble = ViewBag.ItemDetails as DataTable;
                            GoodsReceiptNote.wh_id = dttble.Rows[0]["wh_id"].ToString();
                            ViewBag.ItemDetails = ViewData["ItemDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"];
                            ViewBag.ItemStockBatchWise= ViewData["ItemBatchDetails"];
                            GoodsReceiptNote.BtnName = "Refresh";
                            GoodsReceiptNote.Command = "Refresh";
                            GoodsReceiptNote.DocumentStatus = "D";

                            //string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"]));
                            //string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"]));
                            //string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"]));
                            //ViewBag.ValDigit = ValDigit;
                            //ViewBag.QtyDigit = QtyDigit;
                            //ViewBag.RateDigit = RateDigit;
                            SetDecimals(GoodsReceiptNote);

                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/GoodsReceiptNote/GoodsReceiptNoteDetail.cshtml", GoodsReceiptNote);
                        }
                        else
                        {
                            URLModelDetails URLModelSave = new URLModelDetails();
                            URLModelSave.BtnName = GoodsReceiptNote.BtnName;
                            URLModelSave.TransType = GoodsReceiptNote.TransType;
                            URLModelSave.Command = GoodsReceiptNote.Command;
                            URLModelSave.grn_no = GoodsReceiptNote.grn_no;
                            URLModelSave.grn_dt = GoodsReceiptNote.grn_dt;
                            //Session["grn_no"] = Session["grn_no"].ToString();
                            TempData["ModelData"] = GoodsReceiptNote;
                            TempData["ListFilterData"] = GoodsReceiptNote.ListFilterData1;
                            return RedirectToAction("GRNDetail", URLModelSave);
                        }
                    case "Forward":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditGRN", new { grn_no = GoodsReceiptNote.grn_no, grn_dt = GoodsReceiptNote.grn_dt, ListFilterData = GoodsReceiptNote.ListFilterData1, WF_status = GoodsReceiptNote.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string GRNDt1 = GoodsReceiptNote.grn_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, GRNDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditGRN", new { grn_no = GoodsReceiptNote.grn_no, grn_dt = GoodsReceiptNote.grn_dt, ListFilterData = GoodsReceiptNote.ListFilterData1, WF_status = GoodsReceiptNote.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 09-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditGRN", new { grn_no = GoodsReceiptNote.grn_no, grn_dt = GoodsReceiptNote.grn_dt, ListFilterData = GoodsReceiptNote.ListFilterData1, WF_status = GoodsReceiptNote.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string GRNDt2 = GoodsReceiptNote.grn_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, GRNDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditGRN", new { grn_no = GoodsReceiptNote.grn_no, grn_dt = GoodsReceiptNote.grn_dt, ListFilterData = GoodsReceiptNote.ListFilterData1, WF_status = GoodsReceiptNote.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        GoodsReceiptNote.Command = command;
                        string MrType = "GRN";
                        Approve_GoodNoteReceipt(GoodsReceiptNote.grn_no, GoodsReceiptNote.grn_dt, MrType, "", "", "", "", "");
                        GoodsReceiptNote.Message = "Approved";
                        GoodsReceiptNote.TransType = "Update";
                        GoodsReceiptNote.Command = "Approve";
                        GoodsReceiptNote.grn_no = GoodsReceiptNote.grn_no;
                        GoodsReceiptNote.grn_dt = GoodsReceiptNote.grn_dt;
                        GoodsReceiptNote.AppStatus = "D";
                        GoodsReceiptNote.BtnName = "BtnEdit";
                        URLModelDetails URLModelApprove = new URLModelDetails();
                        URLModelApprove.BtnName = GoodsReceiptNote.BtnName;
                        URLModelApprove.TransType = GoodsReceiptNote.TransType;
                        URLModelApprove.Command = GoodsReceiptNote.Command;
                        URLModelApprove.grn_no = GoodsReceiptNote.grn_no;
                        URLModelApprove.grn_dt = GoodsReceiptNote.grn_dt;
                        TempData["ModelData"] = GoodsReceiptNote;
                        TempData["ListFilterData"] = GoodsReceiptNote.ListFilterData1;
                        return RedirectToAction("GRNDetail", URLModelApprove);

                    case "Refresh":
                        GoodsReceiptNoteModel GoodsReceiptNoteRefresh = new GoodsReceiptNoteModel();
                        GoodsReceiptNoteRefresh.BtnName = "Refresh";
                        GoodsReceiptNoteRefresh.Command = command;
                        GoodsReceiptNoteRefresh.TransType = "Save";
                        //GoodsReceiptNoteRefresh.Message = null;
                        TempData["ModelData"] = GoodsReceiptNoteRefresh;
                        //Session["DocumentStatus"] = null;
                        TempData["ListFilterData"] = GoodsReceiptNote.ListFilterData1;
                        TempData["ListFilterData"] = GoodsReceiptNote.ListFilterData1;
                        return RedirectToAction("GRNDetail");

                    case "Print":
                        return GenratePdfFile(GoodsReceiptNote);
                    case "BacktoList":
                        GRN_ListModel _GRN_ListModel = new GRN_ListModel();
                        _GRN_ListModel.WF_status= GoodsReceiptNote.WF_status1;
                        TempData["ListFilterData"] = GoodsReceiptNote.ListFilterData1;
                        return RedirectToAction("GRNList", "GRNDetail", _GRN_ListModel);

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
        public string CheckInvoiceAgainstGRN(string DocNo, string DocDate)
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
                DataSet Deatils = _GoodsReceiptNote_ISERVICE.CheckInvoiceAgainstGRN(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "InvoiceCreated";
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
        public ActionResult SaveGoodsRecipteNote(GoodsReceiptNoteModel GoodsReceiptNote, string title)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
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
                var commonContr = new CommonController();
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable DtblItemBatchDetail = new DataTable();
                DataTable DtblItemSerialDetail = new DataTable();
                DataTable DtblAttchDetail = new DataTable();
                DataTable DtblSubItemDetail = new DataTable();
                //DataTable Voudetail = new DataTable();

                DtblHDetail = HeaderDetail(GoodsReceiptNote);
                
                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("rec_qty", typeof(string));
                dtItem.Columns.Add("wh_id", typeof(int));
                dtItem.Columns.Add("reject_wh_id", typeof(int));
                dtItem.Columns.Add("reject_qty", typeof(string));
                dtItem.Columns.Add("rework_wh_id", typeof(int));
                dtItem.Columns.Add("rework_qty", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("item_gross_val", typeof(string));
                dtItem.Columns.Add("item_tax_amt_recov", typeof(string));
                dtItem.Columns.Add("item_tax_amt_nrecov", typeof(string));
                dtItem.Columns.Add("item_oc_amt", typeof(string));
                dtItem.Columns.Add("item_net_val", typeof(string));
                dtItem.Columns.Add("item_landed_rate", typeof(string));
                dtItem.Columns.Add("item_landed_val", typeof(string));
                dtItem.Columns.Add("it_remarks", typeof(string));
                dtItem.Columns.Add("short_qty", typeof(string));
                dtItem.Columns.Add("sample_qty", typeof(string));
                dtItem.Columns.Add("reason_rej", typeof(string));
                dtItem.Columns.Add("reason_rwk", typeof(string));

                JArray jObject = JArray.Parse(GoodsReceiptNote.GRNItemdetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                    dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"].ToString());
                    dtrowLines["rec_qty"] = jObject[i]["rec_qty"].ToString();
                    dtrowLines["wh_id"] = Convert.ToInt32(jObject[i]["wh_id"].ToString());
                    if (jObject[i]["reject_wh_id"].ToString() == "null")
                    {
                        dtrowLines["reject_wh_id"] = "0";
                    }
                    else
                    {
                        //var asd = jObject[i]["reject_wh_id"].ToString();
                        dtrowLines["reject_wh_id"] = Convert.ToInt32(jObject[i]["reject_wh_id"].ToString());
                    }
                    //dtrowLines["reject_wh_id"] =Convert.ToInt32(jObject[i]["reject_wh_id"].ToString());
                    dtrowLines["reject_qty"] = jObject[i]["reject_qty"].ToString();
                    if (jObject[i]["rework_wh_id"].ToString() == "null")
                    {
                        dtrowLines["rework_wh_id"] = "0";
                    }
                    else
                    {
                        dtrowLines["rework_wh_id"] = Convert.ToInt32(jObject[i]["rework_wh_id"].ToString());
                    }
                    //dtrowLines["rework_wh_id"] = Convert.ToInt32(jObject[i]["rework_wh_id"].ToString());
                    dtrowLines["rework_qty"] = jObject[i]["rework_qty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                    dtrowLines["item_gross_val"] = jObject[i]["item_gross_val"].ToString();
                    dtrowLines["item_tax_amt_recov"] = jObject[i]["item_tax_amt_recov"].ToString();
                    dtrowLines["item_tax_amt_nrecov"] = jObject[i]["item_tax_amt_nrecov"].ToString();
                    dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                    dtrowLines["item_net_val"] = jObject[i]["item_net_val"].ToString();
                    dtrowLines["item_landed_rate"] = jObject[i]["item_landed_rate"].ToString();
                    dtrowLines["item_landed_val"] = jObject[i]["item_landed_val"].ToString();
                    dtrowLines["it_remarks"] = jObject[i]["it_remarks"].ToString();
                    dtrowLines["short_qty"] = jObject[i]["short_qty"].ToString();
                    dtrowLines["sample_qty"] = jObject[i]["sample_qty"].ToString();
                    dtrowLines["reason_rej"] = jObject[i]["reason_rej"].ToString();
                    dtrowLines["reason_rwk"] = jObject[i]["reason_rwk"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDetail = dtItem;
                ViewData["ItemDetails"] = dtitemdetail(jObject);
                
                DataTable Batch_detail = new DataTable();
                Batch_detail.Columns.Add("item_id", typeof(string));
                Batch_detail.Columns.Add("batch_no", typeof(string));
                Batch_detail.Columns.Add("batch_qty", typeof(string));
                Batch_detail.Columns.Add("reject_batch_qty", typeof(string));
                Batch_detail.Columns.Add("rework_batch_qty", typeof(string));
                Batch_detail.Columns.Add("exp_dt", typeof(string));
                Batch_detail.Columns.Add("bt_sale", typeof(string));
                Batch_detail.Columns.Add("mfg_name", typeof(string));
                Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                Batch_detail.Columns.Add("mfg_date", typeof(string));
                if (GoodsReceiptNote.BatchDetail != null)
                {
                    JArray jObjectBatch = JArray.Parse(GoodsReceiptNote.BatchDetail);
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
                        dtrowBatchDetailsLines["bt_sale"] = jObjectBatch[i]["saleable"].ToString();
                        dtrowBatchDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectBatch[i]["MfgName"].ToString(),null);
                        dtrowBatchDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectBatch[i]["Mrp"].ToString(),null);
                        dtrowBatchDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectBatch[i]["MfgDate"].ToString(),null);
                        Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                    }
                    ViewData["ItemBatchDetails"] = dtbatchdetail(jObjectBatch);
                   
                }
                DtblItemBatchDetail = Batch_detail;
                

                DataTable Serial_detail = new DataTable();
                Serial_detail.Columns.Add("item_id", typeof(string));
                Serial_detail.Columns.Add("serial_no", typeof(string));
                Serial_detail.Columns.Add("QtyType", typeof(string));
                Serial_detail.Columns.Add("mfg_name", typeof(string));
                Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                Serial_detail.Columns.Add("mfg_date", typeof(string));

                if (GoodsReceiptNote.SerialDetail != null)
                {
                    JArray jObjectSerial = JArray.Parse(GoodsReceiptNote.SerialDetail);
                    for (int i = 0; i < jObjectSerial.Count; i++)
                    {
                        DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                        dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["item_id"].ToString();
                        dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["serial_no"].ToString();
                        dtrowSerialDetailsLines["QtyType"] = jObjectSerial[i]["QtyType"].ToString();
                        dtrowSerialDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectSerial[i]["MfgName"].ToString(),null);
                        dtrowSerialDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectSerial[i]["Mrp"].ToString(),null);
                        dtrowSerialDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectSerial[i]["MfgDate"].ToString(),null);
                        Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                    }
                    ViewData["ItemSerialDetails"] = dtserialdetail(jObjectSerial);
                }
                DtblItemSerialDetail = Serial_detail;

                DataTable dtAttachment = new DataTable();
                var _GoodsReceiptNoteModelAttch = TempData["ModelDataattch"] as GoodsReceiptNoteModelAttch;
                TempData["ModelDataattch"] = null;
                if (GoodsReceiptNote.attatchmentdetail != null)
                {
                    if (_GoodsReceiptNoteModelAttch != null)
                    {
                        if (_GoodsReceiptNoteModelAttch.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = _GoodsReceiptNoteModelAttch.AttachMentDetailItmStp as DataTable;
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
                        //if (Session["AttachMentDetailItmStp"] != null)
                        if (GoodsReceiptNote.AttachMentDetailItmStp != null)
                        {
                            dtAttachment = GoodsReceiptNote.AttachMentDetailItmStp as DataTable;
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
                    JArray jObject1 = JArray.Parse(GoodsReceiptNote.attatchmentdetail);
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
                            if (!string.IsNullOrEmpty(GoodsReceiptNote.grn_no))
                            {
                                dtrowAttachment1["id"] = GoodsReceiptNote.grn_no;
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
                    if (GoodsReceiptNote.TransType == "Update")
                    {

                        string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                        if (Directory.Exists(AttachmentFilePath))
                        {
                            string ItmCode = string.Empty;
                            if (!string.IsNullOrEmpty(GoodsReceiptNote.grn_no))
                            {
                                ItmCode = GoodsReceiptNote.grn_no;
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


                //DataTable Voudt = new DataTable();
                //Voudt.Columns.Add("comp_id", typeof(int));
                //Voudt.Columns.Add("accid", typeof(string));
                //Voudt.Columns.Add("type", typeof(string));
                //Voudt.Columns.Add("doctype", typeof(string));
                //Voudt.Columns.Add("value", typeof(string));
                //Voudt.Columns.Add("dramt", typeof(string));
                //Voudt.Columns.Add("cramt", typeof(string));
                //Voudt.Columns.Add("Transtype", typeof(string));


                //JArray AObject = JArray.Parse(GoodsReceiptNote.Voudetails);
                //for (int i = 0; i < AObject.Count; i++)
                //{

                //    DataRow dtrowVoudetails = Voudt.NewRow();
                //    dtrowVoudetails["comp_id"] = Session["CompId"].ToString();
                //    dtrowVoudetails["accid"] = AObject[i]["accid"].ToString();
                //    dtrowVoudetails["type"] = AObject[i]["type"].ToString();
                //    dtrowVoudetails["doctype"] = AObject[i]["doctype"].ToString();
                //    dtrowVoudetails["value"] = AObject[i]["Value"].ToString();
                //    dtrowVoudetails["dramt"] = AObject[i]["DrAmt"].ToString();
                //    dtrowVoudetails["cramt"] = AObject[i]["CrAmt"].ToString();
                //    dtrowVoudetails["TransType"] = AObject[i]["TransType"].ToString();
                //    Voudt.Rows.Add(dtrowVoudetails);
                //}
                //Voudetail = Voudt;
                DtblSubItemDetail = ToDtblSubItem(GoodsReceiptNote.SubItemDetailsDt);

                SaveMessage = _GoodsReceiptNote_ISERVICE.InsertGRN_Details(DtblHDetail, DtblItemDetail, DtblItemBatchDetail
                    , DtblItemSerialDetail, DtblAttchDetail, DtblSubItemDetail);
                if (SaveMessage == "DocModify")
                {
                    GoodsReceiptNote.Message = "DocModify";
                    GoodsReceiptNote.BtnName = "Refresh";
                    GoodsReceiptNote.Command = "Refresh";
                    TempData["ModelData"] = GoodsReceiptNote;
                    return RedirectToAction("GRNDetail");
                }
                else
                {
                    string[] FDetail = SaveMessage.Split(',');
                    string Inv_no = FDetail[0].ToString();
                    if (Inv_no == "StNtAvl")
                    {
                        GoodsReceiptNote.StockItemWiseMessage = string.Join(",", FDetail.Skip(1));
                        GoodsReceiptNote.Message = "StockNotAvailable";
                    }
                    else
                    {
                        string Message = FDetail[0].ToString();
                        string grn_no = FDetail[2].ToString();
                        string grn_DATE = FDetail[1].ToString();
                        string Cansal = FDetail[3].ToString();

                        if (Message == "DataNotFound")
                        {
                            var msg = "Data Not found" + " " + grn_DATE + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            GoodsReceiptNote.Message = Message;
                            return RedirectToAction("GRNDetail");
                        }

                        if (Message == "Used")
                        {
                            DtblItemDetail.Columns.Add("item_name", typeof(string));
                            DtblItemDetail.Columns.Add("uom_alias", typeof(string));
                            DtblItemDetail.Columns.Add("rej_wh_id", typeof(string));
                            DtblItemDetail.Columns.Add("lot_id", typeof(string));
                            DtblItemDetail.Columns.Add("i_batch", typeof(string));
                            DtblItemDetail.Columns.Add("i_serial", typeof(string));
                            for (var i = 0; i < DtblItemDetail.Rows.Count; i++)
                            {
                                foreach (JObject item in jObject.Children())
                                {
                                    if (item.GetValue("item_id").ToString() == DtblItemDetail.Rows[i]["item_id"].ToString())
                                    {
                                        DtblItemDetail.Rows[i]["item_name"] = item.GetValue("item_name");
                                        DtblItemDetail.Rows[i]["uom_alias"] = item.GetValue("uom_alias");
                                        DtblItemDetail.Rows[i]["rej_wh_id"] = DtblItemDetail.Rows[i]["reject_wh_id"].ToString();
                                        DtblItemDetail.Rows[i]["i_batch"] = item.GetValue("i_batch");
                                        DtblItemDetail.Rows[i]["i_serial"] = item.GetValue("i_serial");
                                    }
                                }
                            }
                            DataSet dtset = new DataSet();
                            dtset.Tables.AddRange(new DataTable[] { DtblItemDetail, DtblItemBatchDetail, DtblItemSerialDetail });
                            GoodsReceiptNote.DtSet = dtset;
                            GoodsReceiptNote.Message = "Used";
                            string Guid = "";
                            if (_GoodsReceiptNoteModelAttch != null)
                            {
                                if (_GoodsReceiptNoteModelAttch.Guid != null)
                                {
                                    Guid = _GoodsReceiptNoteModelAttch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, grn_no, GoodsReceiptNote.TransType, dtAttachment);

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
                            //                string grn_no1 = grn_no.Replace("/", "");
                            //                string img_nm = CompID + BrchID + grn_no1 + "_" + Path.GetFileName(DrItmNm).ToString();
                            //                string doc_path = Path.Combine(Server.MapPath("~/Attachment/" + PageName + "/"), img_nm);
                            //                string DocumentPath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //                if (!Directory.Exists(DocumentPath))
                            //                {
                            //                    DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                            //                }

                            //                System.IO.File.Delete(file);
                            //                //System.IO.File.Move(file, doc_path);
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        else
                        {
                            if (Message == "StNtAvl")
                            {
                                GoodsReceiptNote.Message = "StockNotAvailable";
                            }
                            if (Message == "Save")
                            {
                                string Guid = "";
                                if (_GoodsReceiptNoteModelAttch != null)
                                {
                                    if (_GoodsReceiptNoteModelAttch.Guid != null)
                                    {
                                        Guid = _GoodsReceiptNoteModelAttch.Guid;
                                    }
                                }
                                string guid = Guid;
                                var comCont = new CommonController(_Common_IServices);
                                comCont.ResetImageLocation(CompID, BrchID, guid, PageName, grn_no, GoodsReceiptNote.TransType, dtAttachment);

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
                                //                string grn_no1 = grn_no.Replace("/", "");
                                //                string img_nm = CompID + BrchID + grn_no1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                                    //string fileName = "GRN_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                    string fileName = "GoodsRecieptNote_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                    var filePath = SavePdfDocToSendOnEmailAlert(grn_no, grn_DATE, fileName, DocumentMenuId, "C");
                                    _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, grn_no, "C", UserID, "", filePath);
                                }
                                catch (Exception exMail)
                                {
                                    GoodsReceiptNote.Message = "ErrorInMail";
                                    string path = Server.MapPath("~");
                                    Errorlog.LogError(path, exMail);
                                }
                                //GoodsReceiptNote.Message = "Cancelled";
                                GoodsReceiptNote.Message = GoodsReceiptNote.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                                GoodsReceiptNote.Command = "Update";
                                GoodsReceiptNote.grn_no = grn_no;
                                GoodsReceiptNote.grn_dt = grn_DATE;
                                GoodsReceiptNote.TransType = "Update";
                                GoodsReceiptNote.AppStatus = "D";
                                GoodsReceiptNote.BtnName = "Refresh";

                            }
                            else
                            {
                                if (Message == "Update" || Message == "Save")
                                {
                                    GoodsReceiptNote.Message = "Save";
                                    GoodsReceiptNote.Command = "Update";
                                    GoodsReceiptNote.grn_no = grn_no;
                                    GoodsReceiptNote.grn_dt = grn_DATE;
                                    GoodsReceiptNote.TransType = "Update";
                                    GoodsReceiptNote.AppStatus = "D";
                                    GoodsReceiptNote.BtnName = "BtnSave";
                                    GoodsReceiptNote.AttachMentDetailItmStp = null;
                                    GoodsReceiptNote.Guid = null;
                                }
                                else if (Message == "InvoiceCreated")
                                {
                                    GoodsReceiptNote.Message = "InvoiceCreated";
                                }

                            }
                        }
                    }                    
                    return RedirectToAction("GRNDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (GoodsReceiptNote.TransType == "Save")
                    {
                        string Guid = "";
                        if (GoodsReceiptNote.Guid != null)
                        {   
                            Guid = GoodsReceiptNote.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public ActionResult SaveGRNCostingDetail(GoodsReceiptNoteModel GoodsReceiptNote)
        {
            try
            {
                string SaveMessage = "";
                //getDocumentName(); /* To set Title*/
                //string PageName = title.Replace(" ", "");
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
                DataTable DtblHDetail = HeaderDetail(GoodsReceiptNote);
                DataTable DtblItemDetail = CostingDetailItmList(GoodsReceiptNote.CostingDetailItmDt);
                if (DtblItemDetail.Rows.Count > 0 || GoodsReceiptNote.CancelFlag)
                {
                    DataTable DtblTaxDetail = ToDtblTaxDetail(GoodsReceiptNote.CostingDetailItmTaxDt, "Tax");
                    DataTable DtblOCTaxDetail = ToDtblTaxDetail(GoodsReceiptNote.CostingDetailItmOCTaxDt, "OC");
                    DataTable DtblOcDetail = ToDtblOCDetail(GoodsReceiptNote.CostingDetailOcDt);


                    SaveMessage = _GoodsReceiptNote_ISERVICE.InsertGRNCosting_Details(DtblHDetail, DtblItemDetail, DtblTaxDetail
                        , DtblOcDetail, DtblOCTaxDetail);
                    string Status = SaveMessage.Split(',')[3];
                    if (Status.Trim() == "A")
                    {
                        try
                        {
                           // string fileName = "GRN_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "GoodsRecieptNote_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(GoodsReceiptNote.grn_no, GoodsReceiptNote.grn_dt, fileName, DocumentMenuId,"");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, GoodsReceiptNote.grn_no, Status.Trim(), UserID, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            GoodsReceiptNote.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        GoodsReceiptNote.Message = "CostingComplited";
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
                return RedirectToAction("GRNDetail");
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public DataTable HeaderDetail(GoodsReceiptNoteModel GoodsReceiptNote)
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

            DataTable DtblHDetail = new DataTable();
            DataTable dtheader = new DataTable();
            dtheader.Columns.Add("TransType", typeof(string));
            dtheader.Columns.Add("MenuID", typeof(string));
            dtheader.Columns.Add("Cancelled", typeof(string));
            dtheader.Columns.Add("comp_id", typeof(string));
            dtheader.Columns.Add("br_id", typeof(string));
            dtheader.Columns.Add("mr_type", typeof(string));
            dtheader.Columns.Add("mr_no", typeof(string));
            dtheader.Columns.Add("mr_dt", typeof(string));
            dtheader.Columns.Add("doc_no", typeof(string));
            dtheader.Columns.Add("doc_dt", typeof(string));
            dtheader.Columns.Add("supp_id", typeof(string));
            dtheader.Columns.Add("user_id", typeof(string));
            dtheader.Columns.Add("mr_status", typeof(string));
            dtheader.Columns.Add("mac_id", typeof(string));
            dtheader.Columns.Add("gr_val", typeof(string));
            dtheader.Columns.Add("tax_amt_nrecov", typeof(string));
            dtheader.Columns.Add("tax_amt_recov", typeof(string));
            dtheader.Columns.Add("oc_amt", typeof(string));
            dtheader.Columns.Add("net_val", typeof(string));
            dtheader.Columns.Add("landed_val", typeof(string));
            dtheader.Columns.Add("gr_no", typeof(string));
            if (GoodsReceiptNote.GRDate != null)
            {
                dtheader.Columns.Add("gr_date", typeof(DateTime));
            }
            else
            {
                dtheader.Columns.Add("gr_date", typeof(string));
            }
            dtheader.Columns.Add("freight_amt", typeof(string));
            dtheader.Columns.Add("trans_name", typeof(string));
            dtheader.Columns.Add("veh_no", typeof(string));
            dtheader.Columns.Add("veh_load", typeof(string));

            dtheader.Columns.Add("ewb_no", typeof(string));
            dtheader.Columns.Add("einv_no", typeof(string));
            dtheader.Columns.Add("curr_id", typeof(string));
            dtheader.Columns.Add("conv_rate", typeof(string));
            dtheader.Columns.Add("order_type", typeof(string));
            dtheader.Columns.Add("bill_no", typeof(string));
            dtheader.Columns.Add("bill_date", typeof(string));
            dtheader.Columns.Add("cancel_remarks", typeof(string));

            DataRow dtrowHeader = dtheader.NewRow();
            //dtrowHeader["TransType"]= Session["TransType"].ToString();
            dtrowHeader["TransType"] = GoodsReceiptNote.TransType;
            dtrowHeader["MenuID"] = DocumentMenuId;
            string cancelflag = GoodsReceiptNote.CancelFlag.ToString();
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
            dtrowHeader["mr_type"] = "GRN";
            dtrowHeader["mr_no"] = GoodsReceiptNote.grn_no;
            dtrowHeader["mr_dt"] = GoodsReceiptNote.grn_dt;
            dtrowHeader["doc_no"] = GoodsReceiptNote.DeliveryNoteNumber;
            dtrowHeader["doc_dt"] = GoodsReceiptNote.DeliveryNoteDate;
            dtrowHeader["supp_id"] = GoodsReceiptNote.supp_id;
            dtrowHeader["user_id"] = Session["UserId"].ToString();
            //dtrowHeader["mr_status"] = Session["AppStatus"].ToString();
            dtrowHeader["mr_status"] = IsNull(GoodsReceiptNote.AppStatus, "D");
            string mac = Session["UserMacaddress"].ToString();
            string system = Session["UserSystemName"].ToString();
            string ip = Session["UserIP"].ToString();
            string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
            dtrowHeader["mac_id"] = mac_id;
            dtrowHeader["gr_val"] = GoodsReceiptNote.GrossValue == null ? "0" : GoodsReceiptNote.GrossValue;
            dtrowHeader["tax_amt_nrecov"] = GoodsReceiptNote.TaxAmountNonRecoverable;
            dtrowHeader["tax_amt_recov"] = GoodsReceiptNote.TaxAmountRecoverable;
            dtrowHeader["oc_amt"] = GoodsReceiptNote.OtherCharges;
            dtrowHeader["net_val"] = GoodsReceiptNote.NetMRValue == null ? "0" : GoodsReceiptNote.NetMRValue;
            dtrowHeader["landed_val"] = GoodsReceiptNote.NetLandedValue;
            dtrowHeader["gr_no"] = GoodsReceiptNote.GRNumber;
            dtrowHeader["gr_date"] = GoodsReceiptNote.GRDate;
            dtrowHeader["freight_amt"] = GoodsReceiptNote.FreightAmount;
            dtrowHeader["trans_name"] = GoodsReceiptNote.TransporterName;
            dtrowHeader["veh_no"] = GoodsReceiptNote.VehicleNumber;
            dtrowHeader["veh_load"] = GoodsReceiptNote.veh_load;
            dtrowHeader["ewb_no"] = GoodsReceiptNote.EWBNNumber;
            dtrowHeader["einv_no"] = GoodsReceiptNote.EInvoive;
            dtrowHeader["curr_id"] = GoodsReceiptNote.CurrId;
            dtrowHeader["conv_rate"] = GoodsReceiptNote.ConvRate;
            dtrowHeader["order_type"] = GoodsReceiptNote.OrderType;
            dtrowHeader["bill_no"] = GoodsReceiptNote.bill_no;
            dtrowHeader["bill_date"] = GoodsReceiptNote.bill_dt;
            dtrowHeader["cancel_remarks"] = GoodsReceiptNote.CancelledRemarks;

            dtheader.Rows.Add(dtrowHeader);
            DtblHDetail = dtheader;
            return DtblHDetail;
        }
        public DataTable CostingDetailItmList(string GRNItemdetails)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("grn_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("item_gross_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt", typeof(string));
            dtItem.Columns.Add("item_tax_amt_recov", typeof(string));
            dtItem.Columns.Add("item_tax_amt_nrecov", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val", typeof(string));
            dtItem.Columns.Add("item_net_val_base", typeof(string));
            dtItem.Columns.Add("item_landed_rate", typeof(string));
            dtItem.Columns.Add("item_landed_val", typeof(string));
            dtItem.Columns.Add("tax_expted", typeof(string));
            dtItem.Columns.Add("manual_gst", typeof(string));
            dtItem.Columns.Add("hsn_code", typeof(string));
            dtItem.Columns.Add("item_gross_val_bs", typeof(string));
            if (GRNItemdetails != null)
            {
                JArray jObject = JArray.Parse(GRNItemdetails);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                    dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["UOMID"].ToString());
                    dtrowLines["grn_qty"] = jObject[i]["GrnQty"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["ItmRate"].ToString();
                    dtrowLines["item_gross_val"] = jObject[i]["GrossVal"].ToString();
                    dtrowLines["item_tax_amt"] = jObject[i]["TaxAmt"].ToString();
                    dtrowLines["item_tax_amt_recov"] = jObject[i]["TaxRecovAmt"].ToString();
                    dtrowLines["item_tax_amt_nrecov"] = jObject[i]["TaxNonRecovAmt"].ToString();
                    dtrowLines["item_oc_amt"] = jObject[i]["OCAmt"].ToString();
                    dtrowLines["item_net_val"] = jObject[i]["NetValSpec"].ToString();
                    dtrowLines["item_net_val_base"] = jObject[i]["NetValBase"].ToString();
                    dtrowLines["item_landed_rate"] = jObject[i]["LandedCostPerPc"].ToString();
                    dtrowLines["item_landed_val"] = jObject[i]["LandedCostValue"].ToString();
                    dtrowLines["tax_expted"] = jObject[i]["TaxExempted"].ToString();
                    dtrowLines["manual_gst"] = jObject[i]["ManualGST"].ToString();
                    dtrowLines["hsn_code"] = jObject[i]["hsn_code"].ToString();
                    dtrowLines["item_gross_val_bs"] = jObject[i]["GrossValBase"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }

            }

            return dtItem;
        }
        private DataTable ToDtblTaxDetail(string TaxDetails,string taxOn)
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
                dtItem.Columns.Add("tax_recov", typeof(string));

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
                        dtrowLines["tax_recov"] = taxOn == "OC" ? "" : jObject[i]["TaxRecov"].ToString();
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
        public DataTable dtitemdetail(JArray jObject)
        {

            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_alias", typeof(string));
            dtItem.Columns.Add("rec_qty", typeof(string));
            dtItem.Columns.Add("wh_id", typeof(int));
            dtItem.Columns.Add("rej_wh_id", typeof(int));
            dtItem.Columns.Add("reject_qty", typeof(string));
            dtItem.Columns.Add("rework_wh_id", typeof(int));
            dtItem.Columns.Add("rework_qty", typeof(string));
            dtItem.Columns.Add("item_rate", typeof(string));
            dtItem.Columns.Add("lot_id", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));
            dtItem.Columns.Add("item_gross_val", typeof(string));
            dtItem.Columns.Add("item_tax_amt_recov", typeof(string));
            dtItem.Columns.Add("item_tax_amt_nrecov", typeof(string));
            dtItem.Columns.Add("item_oc_amt", typeof(string));
            dtItem.Columns.Add("item_net_val", typeof(string));
            dtItem.Columns.Add("item_landed_rate", typeof(string));
            dtItem.Columns.Add("item_landed_val", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));



            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["item_id"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = Convert.ToInt32(jObject[i]["uom_id"].ToString());
                dtrowLines["uom_alias"] = jObject[i]["uom_alias"].ToString();
                dtrowLines["rec_qty"] = jObject[i]["rec_qty"].ToString();
                dtrowLines["wh_id"] = Convert.ToInt32(jObject[i]["wh_id"].ToString());
                if (jObject[i]["reject_wh_id"].ToString() == "null")
                {
                    dtrowLines["rej_wh_id"] = "0";
                }
                else
                {
                    dtrowLines["rej_wh_id"] = Convert.ToInt32(jObject[i]["reject_wh_id"].ToString());
                }
                
                dtrowLines["reject_qty"] = jObject[i]["reject_qty"].ToString();
                if (jObject[i]["rework_wh_id"].ToString() == "null")
                {
                    dtrowLines["rework_wh_id"] = "0";
                }
                else
                {
                    dtrowLines["rework_wh_id"] = Convert.ToInt32(jObject[i]["rework_wh_id"].ToString());
                }
                dtrowLines["rework_qty"] = jObject[i]["rework_qty"].ToString();
                dtrowLines["item_rate"] = jObject[i]["item_rate"].ToString();
                //if(jObject[i]["lot_id"].ToString() == "null"|| jObject[i]["lot_id"].ToString() == "")
                    
                //{
                //    dtrowLines["lot_id"] = "0";
                //}
                //else
                //{
                    dtrowLines["lot_id"] = jObject[i]["lot_id"].ToString();
                //}
                
                dtrowLines["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowLines["i_serial"] = jObject[i]["i_serial"].ToString();
                dtrowLines["item_gross_val"] = jObject[i]["item_gross_val"].ToString();
                dtrowLines["item_tax_amt_recov"] = jObject[i]["item_tax_amt_recov"].ToString();
                dtrowLines["item_tax_amt_nrecov"] = jObject[i]["item_tax_amt_nrecov"].ToString();
                dtrowLines["item_oc_amt"] = jObject[i]["item_oc_amt"].ToString();
                dtrowLines["item_net_val"] = jObject[i]["item_net_val"].ToString();
                dtrowLines["item_landed_rate"] = jObject[i]["item_landed_rate"].ToString();
                dtrowLines["item_landed_val"] = jObject[i]["item_landed_val"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["it_remarks"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }
        public DataTable dtbatchdetail(JArray jObjectBatch)
        {
            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("batch_qty", typeof(string));
            Batch_detail.Columns.Add("reject_batch_qty", typeof(string));
            Batch_detail.Columns.Add("rework_batch_qty", typeof(string));
            Batch_detail.Columns.Add("exp_dt", typeof(string));
            Batch_detail.Columns.Add("bt_sale", typeof(string));

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
                dtrowBatchDetailsLines["bt_sale"] = jObjectBatch[i]["saleable"].ToString();
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
                dtSubItem.Columns.Add("short_qty", typeof(string));
                dtSubItem.Columns.Add("sample_qty", typeof(string));

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
                        dtrowItemdetails["short_qty"] = jObject2[i]["ShortQty"].ToString();
                        dtrowItemdetails["sample_qty"] = jObject2[i]["SampleQty"].ToString();
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
            dtSubItem.Columns.Add("sub_item_name", typeof(string));
            dtSubItem.Columns.Add("AccptQty", typeof(string));
            dtSubItem.Columns.Add("RejQty", typeof(string));
            dtSubItem.Columns.Add("RewrkQty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                if (jObject2[i]["sub_item_name"].ToString() != null)
                {
                    dtrowItemdetails["sub_item_name"] = jObject2[i]["sub_item_name"].ToString();
                }
                else
                {
                    dtrowItemdetails["sub_item_name"] = "";
                }
                dtrowItemdetails["AccptQty"] = jObject2[i]["AccQty"].ToString();
                dtrowItemdetails["RejQty"] = jObject2[i]["RejQty"].ToString();
                dtrowItemdetails["RewrkQty"] = jObject2[i]["RewQty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        public ActionResult GetOpeningMaterialRecieptList(string docid, string status)
        {
            //Session["WF_status"] = status;
            GRN_ListModel _GRN_ListModel = new GRN_ListModel();
            _GRN_ListModel.WF_status = status;
            return RedirectToAction("GRNDetail", _GRN_ListModel);
        }
        public ActionResult Approve_GoodNoteReceipt(string grn_no, string grn_dt, string MrType, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_status1)
        {
            //string Comp_ID = string.Empty;
            //string UserID = string.Empty;
            //string BranchID = string.Empty;
            //string MenuDocId = string.Empty;
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
            //    MenuDocId = DocumentMenuId;
            //}
            try
            {
                GoodsReceiptNoteModel GoodsReceiptNote = new GoodsReceiptNoteModel();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                MrType = "GRN";
                string Message = _GoodsReceiptNote_ISERVICE.Approve_GRN(grn_no, grn_dt, MrType, DocumentMenuId, BrchID, CompID, UserID, mac_id, A_Status, A_Level, A_Remarks);
                string ApMessage = Message.Split(',')[0].Trim();
                string grn_no1 = Message.Split(',')[1].Trim();
                string grn_dt1 = Message.Split(',')[2].Trim();
                if (ApMessage == "A" || ApMessage == "PC")
                {
                    string status = "AP";
                    if (ApMessage == "PC")
                        status = ApMessage;

                    try
                    {
                       // string fileName = "GRN_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "GoodsRecieptNote_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(grn_no, grn_dt, fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, grn_no, "AP", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        GoodsReceiptNote.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    //Session["Message"] = "Approved";
                    //GoodsReceiptNote.Message = "Approved";
                    GoodsReceiptNote.Message = GoodsReceiptNote.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                GoodsReceiptNote.TransType = "Update";
                GoodsReceiptNote.Command = "Approve";
                GoodsReceiptNote.grn_no = grn_no1;
                GoodsReceiptNote.grn_dt = grn_dt1;
                GoodsReceiptNote.AppStatus = "D";
                GoodsReceiptNote.BtnName = "BtnEdit";
                TempData["WF_status1"] = WF_status1;
                TempData["ModelData"] = GoodsReceiptNote;
                URLModelDetails URLModel = new URLModelDetails();
                URLModel.grn_no = grn_no1;
                URLModel.grn_dt = grn_dt1;
                URLModel.BtnName = "BtnEdit";
                URLModel.TransType = "Update";
                URLModel.Command = "Approve";
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["GRN_No"] = grn_no1;
                //Session["GRN_Date"] = grn_dt1;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("GRNDetail", URLModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult DeleteGRNDetails(GoodsReceiptNoteModel GoodsReceiptNote, string MRType)
        {
            //JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/

            try
            {
                //string Comp_ID = string.Empty;
                //string UserID = string.Empty;
                //string BranchID = string.Empty;
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
                string GRN_Delete = _GoodsReceiptNote_ISERVICE.Delete_GRN_Detail(GoodsReceiptNote, MRType, CompID, BrchID);
                /*--------------------------For Attatchment Start--------------------------*/
                if (!string.IsNullOrEmpty(GoodsReceiptNote.grn_no))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string GRNNo1 = GoodsReceiptNote.grn_no.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, GRNNo1, Server);
                }
                /*--------------------------For Attatchment End--------------------------*/
                //Validate = Json(GRN_Delete);
                GoodsReceiptNote.Message = "Deleted";
                GoodsReceiptNote.Command = "Refresh";
                GoodsReceiptNote.TransType = "Refresh";
                GoodsReceiptNote.AppStatus = "D";
                GoodsReceiptNote.BtnName = "BtnDelete";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["GRN_No"] = GoodsReceiptNote.grn_no;
                //Session["GRN_Date"] = GoodsReceiptNote.grn_dt;
                //GoodsReceiptNote = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("GRNDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return Validate;
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
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet result = _GoodsReceiptNote_ISERVICE.GetDeliveryNoteList(Supp_id, CompID, Br_ID);
                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "";
                Drow[2] = "";
                Drow[3] = "";
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
                DataSet result = _GoodsReceiptNote_ISERVICE.GetDeliveryNoteDetail(DnNo, Comp_ID, Br_ID);
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
        public JsonResult GetGRNDetails(string GNNo, string GNDate)
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
                DataSet result = _GoodsReceiptNote_ISERVICE.GetGRNDetails(GNNo, GNDate, Comp_ID, Br_ID);
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
                DataSet result = _GoodsReceiptNote_ISERVICE.GetWarehouseList(Comp_ID, Br_ID);
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
                DataSet result = _GoodsReceiptNote_ISERVICE.GetWarehouseList(Comp_ID, Br_ID);
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
        public ActionResult GetAutoCompleteSupplierName(GoodsReceiptNoteModel GoodsReceiptNote)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(GoodsReceiptNote.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = GoodsReceiptNote.SuppName;
                }
                SuppList = _GoodsReceiptNote_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in SuppList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                GoodsReceiptNote.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetAutoCompleteSearchSupplier(GRN_ListModel _GRN_ListModele)
        {
            string SupplierName = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
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
                if (string.IsNullOrEmpty(_GRN_ListModele.SuppName))
                {
                    SupplierName = "0";
                }
                else
                {
                    SupplierName = _GRN_ListModele.SuppName;
                }
                SuppList = _GoodsReceiptNote_ISERVICE.GetSupplierList(Comp_ID, SupplierName, Br_ID);

                List<SupplierName> _SuppList = new List<SupplierName>();
                foreach (var data in SuppList)
                {
                    SupplierName _SuppDetail = new SupplierName();
                    _SuppDetail.supp_id = data.Key;
                    _SuppDetail.supp_name = data.Value;
                    _SuppList.Add(_SuppDetail);
                }
                _GRN_ListModele.SupplierNameList = _SuppList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        private List<GoodsReceiptNoteList> GetGRNDetailList(GRN_ListModel _GRN_ListModele)
        {
            try
            {
                _GoodsReceiptNoteList = new List<GoodsReceiptNoteList>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //Session["DocumentStatus"] = "D";
                _GRN_ListModele.DocumentStatus = "D";
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                DataSet dt = new DataSet();
                dt = _GoodsReceiptNoteList_ISERVICE.GetGRNDetailList(CompID, BrchID, UserID, _GRN_ListModele.SuppID, _GRN_ListModele.GRN_FromDate, _GRN_ListModele.GRN_ToDate, _GRN_ListModele.Status, _GRN_ListModele.wfdocid, _GRN_ListModele.WF_status);

                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        GoodsReceiptNoteList _GRNList = new GoodsReceiptNoteList();
                        _GRNList.GRNNo = dr["GRNNo"].ToString();
                        _GRNList.GRNDate = dr["GRNDate"].ToString();
                        _GRNList.MrDate = dr["MrDate"].ToString();
                        _GRNList.DeliveryNoteNo = dr["DeliveryNoteNo"].ToString();
                        _GRNList.DeliveryNoteDate = dr["DeliveryNoteDate"].ToString();
                        _GRNList.SuppName = dr["supp_name"].ToString();
                        _GRNList.Stauts = dr["Status"].ToString();
                        _GRNList.CreateDate = dr["CreateDate"].ToString();
                        _GRNList.ApproveDate = dr["ApproveDate"].ToString();
                        _GRNList.ModifyDate = dr["modDate"].ToString();
                        _GRNList.create_by = dr["create_by"].ToString();
                        _GRNList.mod_by = dr["mod_by"].ToString();
                        _GRNList.app_by = dr["app_by"].ToString();
                        _GRNList.bill_no = dr["bill_no"].ToString();
                        _GRNList.bill_dt = dr["bill_dt"].ToString();
                        _GoodsReceiptNoteList.Add(_GRNList);
                    }

                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    //FromDate = Convert.ToDateTime(dt.Tables[1].Rows[0]["finstrdate"]);
                }

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
            return _GoodsReceiptNoteList;
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
        private DataTable GetRoleList()
        {
            try
            {
                string userid = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, userid, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult SearchGRNDetail(string SuppId, string Fromdate, string Todate, string Status)
        {
            try
            {
                //Session.Remove("WF_Docid");
                //Session.Remove("WF_status");
                _GoodsReceiptNoteList = new List<GoodsReceiptNoteList>();
                GRN_ListModel _GRN_ListModel = new GRN_ListModel();
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

                DataSet dt = new DataSet();
                dt = _GoodsReceiptNoteList_ISERVICE.GetGRNDetailList(CompID, BrchID, UserID, SuppId, Fromdate, Todate, Status, "0", "");

                //Session["GRNSearch"] = "GRN_Search";
                _GRN_ListModel.GRNSearch = "GRN_Search";
                if (dt.Tables[0].Rows.Count > 0)
                {


                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        GoodsReceiptNoteList _GRNList = new GoodsReceiptNoteList();
                        _GRNList.GRNNo = dr["GRNNo"].ToString();
                        _GRNList.GRNDate = dr["GRNDate"].ToString();
                        _GRNList.MrDate = dr["MrDate"].ToString();
                        _GRNList.DeliveryNoteNo = dr["DeliveryNoteNo"].ToString();
                        _GRNList.DeliveryNoteDate = dr["DeliveryNoteDate"].ToString();
                        _GRNList.SuppName = dr["supp_name"].ToString();
                        _GRNList.Stauts = dr["Status"].ToString();
                        _GRNList.CreateDate = dr["CreateDate"].ToString();
                        _GRNList.ApproveDate = dr["ApproveDate"].ToString();
                        _GRNList.ModifyDate = dr["modDate"].ToString();
                        _GRNList.create_by = dr["create_by"].ToString();
                        _GRNList.mod_by = dr["mod_by"].ToString();
                        _GRNList.app_by = dr["app_by"].ToString();
                        _GRNList.bill_no = dr["bill_no"].ToString();
                        _GRNList.bill_dt = dr["bill_dt"].ToString();
                        _GoodsReceiptNoteList.Add(_GRNList);
                    }
                }
                _GRN_ListModel.GRNList = _GoodsReceiptNoteList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialGoodsReceiptNoteList.cshtml", _GRN_ListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                GoodsReceiptNoteModelAttch _GoodsReceiptNoteModelAttch = new GoodsReceiptNoteModelAttch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string DeliveryNoteNo = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["GRN_No"] != null)
                //{
                //    DeliveryNoteNo = Session["GRN_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = DocNo;
                _GoodsReceiptNoteModelAttch.Guid = DocNo;
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
                    _GoodsReceiptNoteModelAttch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _GoodsReceiptNoteModelAttch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _GoodsReceiptNoteModelAttch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }

        ///*-----------------GL Voucher Posting Start-----------------------*/
        //public DataTable ToDataTable<T>(IList<T> data)
        //{
        //    try
        //    {
        //        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        //        object[] values = new object[props.Count];
        //        using (DataTable table = new DataTable())
        //        {
        //            long _pCt = props.Count;
        //            for (int i = 0; i < _pCt; ++i)
        //            {
        //                PropertyDescriptor prop = props[i];
        //                table.Columns.Add(prop.Name, prop.PropertyType);
        //            }
        //            foreach (T item in data)
        //            {
        //                long _vCt = values.Length;
        //                for (int i = 0; i < _vCt; ++i)
        //                {
        //                    values[i] = props[i].GetValue(item);
        //                }
        //                table.Rows.Add(values);
        //            }
        //            return table;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
        //[HttpPost]
        //public JsonResult GetGLDetails(List<GL_Detail> GLDetail)
        //{
        //    JsonResult Validate = Json("Please fill all mandatory field");/*Validate Message*/
        //    try
        //    {

        //        DataTable DtblGLDetail = new DataTable();

        //        if (GLDetail != null)
        //        {
        //            DtblGLDetail = ToDataTable(GLDetail);
        //        }
        //        DataSet GlDt = _GoodsReceiptNote_ISERVICE.GetAllGLDetails(DtblGLDetail);
        //        Validate = Json(GlDt);
        //        JsonResult DataRows = null;
        //        DataRows = Json(JsonConvert.SerializeObject(GlDt), JsonRequestBehavior.AllowGet);

        //        return DataRows;
        //    }

        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return Json("ErrorPage");
        //    }
        //    return Validate;
        //} 
        ///*-----------------GL Voucher Posting End-----------------------*/
        ///

        /*--------------------------For PDF Print Start--------------------------*/

        [HttpPost]
        public FileResult GenratePdfFile(GoodsReceiptNoteModel _model)
        {
            return File(PdfFiledata(_model.grn_no, _model.grn_dt), "application/pdf", "GoodsRecieptNote.pdf");
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
                DataSet Details = _GoodsReceiptNote_ISERVICE.GetGRNDeatilsForPrint(CompID, BrchID, grnNo, Convert.ToDateTime(grnDate).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                //ViewBag.InvoiceTo= "Invoice to:";
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.Title = "Goods Receipt Note";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["mr_status"].ToString().Trim();
                ViewBag.Website = Details.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/GoodsReceiptNote/GoodsReceiptNotePrint.cshtml"));
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
                        var data = PdfFiledata(Doc_no, Doc_dt);
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
                dt.Columns.Add("item_id", typeof(string));
                dt.Columns.Add("sub_item_id", typeof(string));
                dt.Columns.Add("sub_item_name", typeof(string));
                dt.Columns.Add("AccptQty", typeof(string));
                dt.Columns.Add("RejQty", typeof(string));
                dt.Columns.Add("RewrkQty", typeof(string));
                dt.Columns.Add("ShortQty", typeof(string));
                dt.Columns.Add("SampleQty", typeof(string));

                //if (Status == "D" || Status == "F" || Status == "")
                //{
           
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    foreach (JObject item in arr.Children())//
                    {
                        DataRow dRow = dt.NewRow();
                        dRow["item_id"] = item.GetValue("item_id").ToString();
                        dRow["sub_item_id"] = item.GetValue("sub_item_id").ToString();
                        dRow["sub_item_name"] = item.GetValue("sub_item_name").ToString();
                        dRow["AccptQty"] = item.GetValue("Accqty").ToString();
                        dRow["RejQty"] = item.GetValue("Rejqty").ToString();
                        dRow["RewrkQty"] = item.GetValue("Rewqty").ToString();
                        dRow["ShortQty"] = item.GetValue("Shortqty").ToString();
                        dRow["SampleQty"] = item.GetValue("Sampleqty").ToString();
                        dt.Rows.Add(dRow);
                    }
                //}
                //else
                //{
                //    //dt = _LSODetail_ISERVICE.SO_GetSubItemDetails(CompID, BrchID, Item_id, QtNo, Doc_no, Doc_dt, Flag).Tables[0];
                //}
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    _subitemPageName = "GRN",
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
        private void SetDecimals(GoodsReceiptNoteModel _model)//Created by Suraj on 24-10-2024
        {

            _model.ValDigit = ToFixDecimal(Convert.ToInt32((_model.OrderType == "I" ? Session["ExpImpValDigit"] : Session["ValDigit"]).ToString()));
            _model.QtyDigit = ToFixDecimal(Convert.ToInt32((_model.OrderType == "I" ? Session["ExpImpQtyDigit"] : Session["QtyDigit"]).ToString()));
            _model.RateDigit = ToFixDecimal(Convert.ToInt32((_model.OrderType == "I" ? Session["ExpImpRateDigit"] : Session["RateDigit"]).ToString()));
            _model.ExchDigit = ToFixDecimal(Convert.ToInt32(Session["ExchDigit"].ToString()));

            ViewBag.ValDigit = _model.ValDigit;
            ViewBag.QtyDigit = _model.QtyDigit;
            ViewBag.RateDigit = _model.RateDigit;
            ViewBag.ExchDigit = _model.ExchDigit;
        }

    }
}