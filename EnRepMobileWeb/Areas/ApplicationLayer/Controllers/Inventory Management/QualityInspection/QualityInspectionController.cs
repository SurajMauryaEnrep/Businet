using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.QualityInspection;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.QualityInspection;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Configuration;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management
{

    public class QualityInspectionController : Controller
    {
        string FromDate, title;
        //string DocumentMenuId = "105102120";
        string DocumentMenuId = string.Empty;
        List<QCInspectionList> _QCInspectionList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string comp_id, BrchID, language, userid = String.Empty;
        QualityInspectionModel _QualityInspectionModel;

        string CompID = string.Empty;
        QualityInspection_ISERVICES _QualityInspection_ISERVICES;
        QualityInspectionList_ISERVICES _QualityInspectionList_ISERVICES;
        Common_IServices _Common_IServices;
        DataTable dt;
        DataSet ds;
        // GET: ApplicationLayer/QualityInspection

        public QualityInspectionController(Common_IServices _Common_IServices, QualityInspection_ISERVICES _QualityInspection_ISERVICES, QualityInspectionList_ISERVICES _QualityInspectionList_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._QualityInspection_ISERVICES = _QualityInspection_ISERVICES;
            this._QualityInspectionList_ISERVICES = _QualityInspectionList_ISERVICES;
        }
        public ActionResult QualityInspectionListInv(QCInspectionList_Model _QCInspectionList_Model)
        {
            try
            {
                //QCInspectionList_Model _QCInspectionList_Model = new QCInspectionList_Model();
                DocumentMenuId = "105102120";
                GetStatusList(_QCInspectionList_Model);
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");              

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                _QCInspectionList_Model.DocumentMenuId = DocumentMenuId;
                _QCInspectionList_Model.QC_Type = "0";
                _QCInspectionList_Model.QISearch = "0";
                _QCInspectionList_Model.Title = title;
                CommonPageDetails();
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _QCInspectionList_Model.WF_status = TempData["WF_status"].ToString();
                    if (_QCInspectionList_Model.WF_status != null)
                    {
                        wfstatus = _QCInspectionList_Model.WF_status;
                        _QCInspectionList_Model.QC_Type = "0";
                    }
                    else
                    {
                        wfstatus = "0";
                    }
                }
                else
                {
                    if (_QCInspectionList_Model.WF_status != null)
                    {
                        wfstatus = _QCInspectionList_Model.WF_status;
                        _QCInspectionList_Model.QC_Type = "0";
                    }
                    else
                    {
                        wfstatus = "0";
                    }
                }
                string Item_id = "0";
                string Item_Name = "0";
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _QCInspectionList_Model.QC_Type = a[0].Trim();
                    _QCInspectionList_Model.QC_FromDate = a[1].Trim();
                    _QCInspectionList_Model.QC_ToDate = a[2].Trim();
                    _QCInspectionList_Model.Status = a[3].Trim();
                    _QCInspectionList_Model.ddl_ItemName = a[4].Trim();
                     Item_id = a[4].Trim();
                    Item_Name = a[5].Trim();
                    if (_QCInspectionList_Model.Status == "0")
                    {
                        _QCInspectionList_Model.Status = null;
                    }
                   
                    _QCInspectionList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                    _QCInspectionList_Model.QCList = GetQCDetailList(_QCInspectionList_Model, wfstatus, Item_id);
                    List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                    ItemName_List _ItemList = new ItemName_List();
                    _ItemList.Item_ID = Item_id;
                    _ItemList.Item_Name = Item_Name;
                    _ItemList1.Add(_ItemList);
                    _QCInspectionList_Model.ItemNameList = _ItemList1;
                }
                else
                {
                    List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                    ItemName_List _ItemList = new ItemName_List();
                    _ItemList.Item_ID = "0";
                    _ItemList.Item_Name = "---Select---";
                    _ItemList1.Add(_ItemList);
                    _QCInspectionList_Model.ItemNameList = _ItemList1;
                }
                if (_QCInspectionList_Model.QC_FromDate!=null)
                {
                    _QCInspectionList_Model.FromDate = _QCInspectionList_Model.QC_FromDate;
                }
                else
                {
                    _QCInspectionList_Model.FromDate = startDate;
                    _QCInspectionList_Model.QC_FromDate = startDate;
                    _QCInspectionList_Model.QC_ToDate = CurrentDate;
                   // _QCInspectionList_Model.QC_FromDate = startDate;
                }
                _QCInspectionList_Model.QCList = GetQCDetailList(_QCInspectionList_Model, wfstatus, Item_id);
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                _QCInspectionList_Model.MenuDocumentId = DocumentMenuId;
                _QCInspectionList_Model.Title = title;
               
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionList.cshtml", _QCInspectionList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult QualityInspectionListPrd(QCInspectionList_Model _QCInspectionList_Model)
        {
            try
            {
                //QCInspectionList_Model _QCInspectionList_Model = new QCInspectionList_Model();
                DocumentMenuId = "105105135";
                GetStatusList(_QCInspectionList_Model);
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;


                _QCInspectionList_Model.DocumentMenuId = DocumentMenuId;
               _QCInspectionList_Model.QC_Type = "0";
               _QCInspectionList_Model.QISearch = "0";
                
                CommonPageDetails();
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _QCInspectionList_Model.WF_status = TempData["WF_status"].ToString();
                    if (_QCInspectionList_Model.WF_status != null)
                    {
                        wfstatus = _QCInspectionList_Model.WF_status;
                        _QCInspectionList_Model.QC_Type = "0";
                    }
                    else
                    {
                        wfstatus = "0";
                    }
                }
                else
                {
                    if (_QCInspectionList_Model.WF_status != null)
                    {
                        wfstatus = _QCInspectionList_Model.WF_status;
                        _QCInspectionList_Model.QC_Type = "0";
                    }
                    else
                    {
                        wfstatus = "0";
                    }
                }
                string Item_id = "0";
                string Item_Name = "0";
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _QCInspectionList_Model.QC_Type = a[0].Trim();
                    _QCInspectionList_Model.QC_FromDate = a[1].Trim();
                    _QCInspectionList_Model.QC_ToDate = a[2].Trim();
                    _QCInspectionList_Model.Status = a[3].Trim();
                    _QCInspectionList_Model.ddl_ItemName = a[4].Trim();
                    Item_id = a[4].Trim();
                    Item_Name = a[5].Trim();
                    if (_QCInspectionList_Model.Status == "0")
                    {
                        _QCInspectionList_Model.Status = null;
                    }
                    List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                    ItemName_List _ItemList = new ItemName_List();
                    _ItemList.Item_ID = Item_id;
                    _ItemList.Item_Name = Item_Name;
                    _ItemList1.Add(_ItemList);
                    _QCInspectionList_Model.ItemNameList = _ItemList1;
                    _QCInspectionList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                }
                else
                {
                    List<ItemName_List> _ItemList1 = new List<ItemName_List>();
                    ItemName_List _ItemList = new ItemName_List();
                    _ItemList.Item_ID = "0";
                    _ItemList.Item_Name = "---Select---";
                    _ItemList1.Add(_ItemList);
                    _QCInspectionList_Model.ItemNameList = _ItemList1;
                }
                if (_QCInspectionList_Model.QC_FromDate != null)
                {
                    _QCInspectionList_Model.FromDate = _QCInspectionList_Model.QC_FromDate;
                }
                else
                {
                    _QCInspectionList_Model.FromDate = startDate;
                    _QCInspectionList_Model.QC_FromDate = startDate;
                    _QCInspectionList_Model.QC_ToDate = CurrentDate;
                }
                _QCInspectionList_Model.QCList = GetQCDetailList(_QCInspectionList_Model, wfstatus, Item_id);
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                _QCInspectionList_Model.Title = title;
                _QCInspectionList_Model.MenuDocumentId = DocumentMenuId;
               
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionList.cshtml", _QCInspectionList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        [HttpPost]
        public JsonResult GetRejectionReason(string SearchName)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = Session["CompId"]?.ToString() ?? string.Empty;
                string Br_ID = Session["BranchId"]?.ToString() ?? string.Empty;

                // Fetch data from service (assuming DataTable result)
                DataTable result = _QualityInspection_ISERVICES.GetRejectionReason(Comp_ID, Br_ID, SearchName);

                //// Convert DataTable to List<object> for Select2
                //var rejectionReasons = result.AsEnumerable().Select(row => new
                //{
                //    id = row["rej_id"],         // Ensure this column exists in DataTable
                //    text = row["rej_reason"]    // Ensure this column exists in DataTable
                //}).ToList();

                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);

                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json(new { error = "An error occurred while fetching data." });
            }
        }
        public ActionResult QualityInspectionListSC(QCInspectionList_Model _QCInspectionList_Model)
        {
            try
            {
                //QCInspectionList_Model _QCInspectionList_Model = new QCInspectionList_Model();
                DocumentMenuId = "105108115";
                GetStatusList(_QCInspectionList_Model);
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");               

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                _QCInspectionList_Model.DocumentMenuId = DocumentMenuId;
                _QCInspectionList_Model.QC_Type = "0";
                _QCInspectionList_Model.QISearch = "0";
               
                CommonPageDetails();
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    _QCInspectionList_Model.WF_status = TempData["WF_status"].ToString();
                    if (_QCInspectionList_Model.WF_status != null)
                    {
                        wfstatus = _QCInspectionList_Model.WF_status;
                        _QCInspectionList_Model.QC_Type = "0";
                    }
                    else
                    {
                        wfstatus = "0";
                    }
                }
                else
                {
                    if (_QCInspectionList_Model.WF_status != null)
                    {
                        wfstatus = _QCInspectionList_Model.WF_status;
                        _QCInspectionList_Model.QC_Type = "0";
                    }
                    else
                    {
                        wfstatus = "0";
                    }
                }
                string Item_id = "0";
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var ListFilterData = TempData["ListFilterData"].ToString();
                    var a = ListFilterData.Split(',');
                    _QCInspectionList_Model.QC_Type = a[0].Trim();
                    _QCInspectionList_Model.QC_FromDate = a[1].Trim();
                    _QCInspectionList_Model.QC_ToDate = a[2].Trim();
                    _QCInspectionList_Model.Status = a[3].Trim();
                    if (_QCInspectionList_Model.Status == "0")
                    {
                        _QCInspectionList_Model.Status = null;
                    }
                    _QCInspectionList_Model.ListFilterData = TempData["ListFilterData"].ToString();
                }
                if (_QCInspectionList_Model.QC_FromDate != null)
                {
                    _QCInspectionList_Model.FromDate = _QCInspectionList_Model.QC_FromDate;
                }
                else
                {
                    _QCInspectionList_Model.FromDate = startDate;
                    _QCInspectionList_Model.QC_FromDate = startDate;
                    _QCInspectionList_Model.QC_ToDate = CurrentDate;
                }
                _QCInspectionList_Model.QCList = GetQCDetailList(_QCInspectionList_Model, wfstatus, Item_id);
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;

                _QCInspectionList_Model.Title = title;
                _QCInspectionList_Model.MenuDocumentId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionList.cshtml", _QCInspectionList_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult QualityInspectionDetail(URLDetailModel URLModel)
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
                userid = Session["userid"].ToString();
            }
            /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.DocDate) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                var _QualityInspectionModel = TempData["ModelData"] as QualityInspectionModel;
                if (_QualityInspectionModel != null)
                {
                    if (URLModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = URLModel.DocumentMenuId;
                    }
                    else
                    {
                        DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                    }
                    if (URLModel.QC_Type != null)
                    {
                        _QualityInspectionModel.qc_type = URLModel.QC_Type;
                    }
                    _QualityInspectionModel.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.doc_no = "---Select---";
                    _DocumentNumber.doc_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);

                    _QualityInspectionModel.DocumentNumberList = _DocumentNumberList;
                    _QualityInspectionModel.ItemDetailsList = null;
                    _QualityInspectionModel.ItemDetailsQCList = null;
                    _QualityInspectionModel.qc_dt = DateTime.Now;
                    _QualityInspectionModel.src_doc_date = null;
                    var other = new CommonController(_Common_IServices);

                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _QualityInspectionModel.MenuDocumentId = DocumentMenuId;
                    /*Commented and make all Procs in one Procedure by Hina on 21-08-2024*/
                    //List<RejectWarehouse> _RejectWarehouseList = new List<RejectWarehouse>();
                    //dt = GetRejectWHList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    RejectWarehouse _RejectWarehouse = new RejectWarehouse();
                    //    _RejectWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _RejectWarehouse.wh_val = dr["wh_val"].ToString();
                    //    _RejectWarehouseList.Add(_RejectWarehouse);
                    //}
                    //_RejectWarehouseList.Insert(0, new RejectWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    //_QualityInspectionModel.RejectWarehouseList = _RejectWarehouseList;
                    //dt = GetSourceAndAcceptWHList();
                    //List<SourceWarehouse> sourceWarehousesList = new List<SourceWarehouse>();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    SourceWarehouse _SourceWarehouse = new SourceWarehouse();
                    //    _SourceWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _SourceWarehouse.wh_val = dr["wh_val"].ToString();
                    //    sourceWarehousesList.Add(_SourceWarehouse);
                    //}
                    //sourceWarehousesList.Insert(0, new SourceWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    //_QualityInspectionModel.SourceWarehouseList = sourceWarehousesList;

                    //List<AcceptedWarehouse> AcceptedWarehousesList = new List<AcceptedWarehouse>();

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    AcceptedWarehouse _AcceptedWarehouse = new AcceptedWarehouse();
                    //    _AcceptedWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _AcceptedWarehouse.wh_val = dr["wh_val"].ToString();
                    //    AcceptedWarehousesList.Add(_AcceptedWarehouse);
                    //}
                    //AcceptedWarehousesList.Insert(0, new AcceptedWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    //_QualityInspectionModel.AcceptedWarehouseList = AcceptedWarehousesList;

                    //List<ReworkWarehouse> _ReworkWarehouseList = new List<ReworkWarehouse>();
                    //dt = GetReworkWHList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ReworkWarehouse _ReworkWarehouse = new ReworkWarehouse();
                    //    _ReworkWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _ReworkWarehouse.wh_val = dr["wh_val"].ToString();
                    //    _ReworkWarehouseList.Add(_ReworkWarehouse);
                    //}
                    //_ReworkWarehouseList.Insert(0, new ReworkWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    //_QualityInspectionModel.ReworkWarehouseList = _ReworkWarehouseList;

                    ds = GetSourceAndAcceptRejctRewrkWHAndShopfloorList();

                    List<SupplierName> supplierName = new List<SupplierName>();
                    if (_QualityInspectionModel.qc_type == "PUR" || _QualityInspectionModel.qc_type == "RQC")
                    {

                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            SupplierName _SupplierName = new SupplierName();
                            _SupplierName.supp_id = dr["supp_id"].ToString();
                            _SupplierName.supp_name = dr["supp_name"].ToString();
                            supplierName.Add(_SupplierName);
                        }
                    }
                    supplierName.Insert(0, new SupplierName() { supp_id = "0", supp_name = "---Select---" });
                    _QualityInspectionModel.SupplierNameList = supplierName;

                    List<SourceWarehouse> sourceWarehousesList = new List<SourceWarehouse>();
                    foreach (DataRow dr in ds.Tables[0].Rows) 
                        {
                        SourceWarehouse _SourceWarehouse = new SourceWarehouse();
                        _SourceWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _SourceWarehouse.wh_val = dr["wh_val"].ToString();
                        sourceWarehousesList.Add(_SourceWarehouse);
                    }
                    sourceWarehousesList.Insert(0, new SourceWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    _QualityInspectionModel.SourceWarehouseList = sourceWarehousesList;

                    List<AcceptedWarehouse> AcceptedWarehousesList = new List<AcceptedWarehouse>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AcceptedWarehouse _AcceptedWarehouse = new AcceptedWarehouse();
                        _AcceptedWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _AcceptedWarehouse.wh_val = dr["wh_val"].ToString();
                        AcceptedWarehousesList.Add(_AcceptedWarehouse);
                    }
                    AcceptedWarehousesList.Insert(0, new AcceptedWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    _QualityInspectionModel.AcceptedWarehouseList = AcceptedWarehousesList;

                    List<RejectWarehouse> _RejectWarehouseList = new List<RejectWarehouse>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        RejectWarehouse _RejectWarehouse = new RejectWarehouse();
                        _RejectWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _RejectWarehouse.wh_val = dr["wh_val"].ToString();
                        _RejectWarehouseList.Add(_RejectWarehouse);
                    }
                    _RejectWarehouseList.Insert(0, new RejectWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    _QualityInspectionModel.RejectWarehouseList = _RejectWarehouseList;

                    List<ReworkWarehouse> _ReworkWarehouseList = new List<ReworkWarehouse>();
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ReworkWarehouse _ReworkWarehouse = new ReworkWarehouse();
                        _ReworkWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _ReworkWarehouse.wh_val = dr["wh_val"].ToString();
                        _ReworkWarehouseList.Add(_ReworkWarehouse);
                    }
                    _ReworkWarehouseList.Insert(0, new ReworkWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    _QualityInspectionModel.ReworkWarehouseList = _ReworkWarehouseList;

                    List<SourceShopfloor> _ShopfloorList = new List<SourceShopfloor>();/*Add By Hina on 21-08-2024 for if Random QC has to be by Shopfloor*/
                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        SourceShopfloor _shopfloor = new SourceShopfloor();
                        _shopfloor.shfl_id = Convert.ToInt32(dr["shfl_id"]);
                        _shopfloor.shfl_val = dr["shfl_val"].ToString();
                        _ShopfloorList.Add(_shopfloor);
                    }
                    _ShopfloorList.Insert(0, new SourceShopfloor() { shfl_id = 0, shfl_val = "---Select---" });
                    _QualityInspectionModel.SourceShopfloorList = _ShopfloorList;

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _QualityInspectionModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _QualityInspectionModel.WF_status1 = TempData["WF_status1"].ToString();
                    }               
                    if (_QualityInspectionModel.TransType != null)
                    {
                        //if (Session["TransType"].ToString() == "Update")
                        if (_QualityInspectionModel.TransType == "Update")
                        {
                            string qc_no = _QualityInspectionModel.qc_no;
                            string QcType = _QualityInspectionModel.qc_type;
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            _QualityInspectionModel.MenuDocumentId = DocumentMenuId;
                            DataSet ds = _QualityInspection_ISERVICES.GetQcInspectionDetail(CompID, qc_no, BrchID, userid, DocumentMenuId);
                            ViewBag.AttechmentDetails = ds.Tables[7];
                            ViewBag.SubItemDetails = ds.Tables[8];
                            _QualityInspectionModel.qc_no = ds.Tables[0].Rows[0]["qc_no"].ToString();
                            _QualityInspectionModel.qc_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["qc_dt"].ToString());
                            _QualityInspectionModel.qc_type = ds.Tables[0].Rows[0]["qc_type"].ToString();
                            _QualityInspectionModel.batch_no = ds.Tables[0].Rows[0]["batch_no"].ToString();
                            _QualityInspectionModel.qc_remarks = ds.Tables[0].Rows[0]["qc_remarks"].ToString();
                            _QualityInspectionModel.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _QualityInspectionModel.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            _QualityInspectionModel.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                            _QualityInspectionModel.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                            _QualityInspectionModel.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                            _QualityInspectionModel.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            _QualityInspectionModel.qc_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                            _QualityInspectionModel.qc_statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                            //_QualityInspectionModel.SourceWH = Convert.ToInt32(ds.Tables[0].Rows[0]["source_wh_id"].ToString());
                            _QualityInspectionModel.AcceptedWH = Convert.ToInt32(ds.Tables[0].Rows[0]["accept_wh_id"].ToString());
                            _QualityInspectionModel.RejectWH = Convert.ToInt32(ds.Tables[0].Rows[0]["reject_wh_id"].ToString());
                            _QualityInspectionModel.ReworkWH = Convert.ToInt32(ds.Tables[0].Rows[0]["rework_wh_id"].ToString());
                            _QualityInspectionModel.Location_type = ds.Tables[0].Rows[0]["loc"].ToString();/*Add by Hina on 23-08-2024 to random qc for shopfloor*/
                            if (_QualityInspectionModel.Location_type == "SF")
                            {
                                _QualityInspectionModel.SourceSF = Convert.ToInt32(ds.Tables[0].Rows[0]["source_wh_id"].ToString());
                            }
                            else
                            {
                                _QualityInspectionModel.SourceWH = Convert.ToInt32(ds.Tables[0].Rows[0]["source_wh_id"].ToString());
                            }
                            _QualityInspectionModel.QCLotBatchdetails = DataTableToJSONWithStringBuilder(ds.Tables[6]);

                            if (_QualityInspectionModel.qc_statuscode == "C")
                            {
                                _QualityInspectionModel.CancelFlag = true;
                                _QualityInspectionModel.BtnName = "Refresh";
                            }
                            else
                            {
                                _QualityInspectionModel.CancelFlag = false;
                            }
                            _QualityInspectionModel.src_doc_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]);
                            _QualityInspectionModel.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            DocumentNumber objDocumentNumber = new DocumentNumber();
                            objDocumentNumber.doc_no = _QualityInspectionModel.src_doc_no;
                            objDocumentNumber.doc_dt = _QualityInspectionModel.src_doc_no;
                            if (!_QualityInspectionModel.DocumentNumberList.Contains(objDocumentNumber))
                            {
                                _QualityInspectionModel.DocumentNumberList.Add(objDocumentNumber);
                            }
                            //Session["DocumentStatus"] = _QualityInspectionModel.qc_statuscode;
                            _QualityInspectionModel.DocumentStatus = _QualityInspectionModel.qc_statuscode;
                            //ViewBag.MenuPageName = getDocumentName();
                            _QualityInspectionModel.Title = title;
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.ItemDetailsQc = ds.Tables[2];
                            ViewBag.DocumentCode = _QualityInspectionModel.qc_statuscode;
                            _QualityInspectionModel.QCItemParamdetails = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                            //ViewBag.VBRoleList = GetRoleList();

                            string Statuscode = _QualityInspectionModel.qc_statuscode;
                            string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                            string UserID = Session["UserID"].ToString();
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();

                            _QualityInspectionModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            _QualityInspectionModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                            _QualityInspectionModel.create_id = Convert.ToInt32(create_id);
                            if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[5];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _QualityInspectionModel.Command != "Edit")
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
                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _QualityInspectionModel.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _QualityInspectionModel.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _QualityInspectionModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _QualityInspectionModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _QualityInspectionModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _QualityInspectionModel.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _QualityInspectionModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _QualityInspectionModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _QualityInspectionModel.BtnName = "BtnRefresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            BindQCType(_QualityInspectionModel);
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionDetail.cshtml", _QualityInspectionModel);
                        }
                        else
                        {
                            //ViewBag.MenuPageName = getDocumentName();
                            //ViewBag.VBRoleList = GetRoleList();
                            _QualityInspectionModel.Title = title;
                            //Session["DocumentStatus"] = "New";
                            _QualityInspectionModel.DocumentStatus = "New";
                            BindQCType(_QualityInspectionModel);
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionDetail.cshtml", _QualityInspectionModel);
                        }
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        //ViewBag.VBRoleList = GetRoleList();
                        _QualityInspectionModel.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _QualityInspectionModel.DocumentStatus = "New";
                        BindQCType(_QualityInspectionModel);
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionDetail.cshtml", _QualityInspectionModel);
                    }
                }
                else
                {/*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
                    QualityInspectionModel _QualityInspectionModel1 = new QualityInspectionModel();
                    if (URLModel.DocumentMenuId != null)
                    {
                        DocumentMenuId = URLModel.DocumentMenuId;
                    }
                    else
                    {
                        DocumentMenuId = _QualityInspectionModel1.DocumentMenuId;
                    }
                    if (URLModel.QC_Type != null)
                    {
                        _QualityInspectionModel1.qc_type = URLModel.QC_Type;
                    }
                    _QualityInspectionModel1.DocumentMenuId = DocumentMenuId;
                    CommonPageDetails();
                    if (URLModel.DocNo != null)
                    {
                        _QualityInspectionModel1.qc_no = URLModel.DocNo;
                    }
                    if (URLModel.TransType != null)
                    {
                        _QualityInspectionModel1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _QualityInspectionModel1.TransType = "Save";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _QualityInspectionModel1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _QualityInspectionModel1.BtnName = "Refresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _QualityInspectionModel1.Command = URLModel.Command;
                    }
                    else
                    {
                        _QualityInspectionModel1.Command = "Refresh";
                    }
                    if (URLModel.QC_Type != null)
                    {
                        _QualityInspectionModel1.qc_type = URLModel.QC_Type;
                    }
                    if (URLModel.WF_status1 != null)
                    {
                        _QualityInspectionModel1.WF_status1 = URLModel.WF_status1;
                    }
                    List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                    DocumentNumber _DocumentNumber = new DocumentNumber();
                    _DocumentNumber.doc_no = "---Select---";
                    _DocumentNumber.doc_dt = "0";
                    _DocumentNumberList.Add(_DocumentNumber);

                    _QualityInspectionModel1.DocumentNumberList = _DocumentNumberList;
                    _QualityInspectionModel1.ItemDetailsList = null;
                    _QualityInspectionModel1.ItemDetailsQCList = null;
                    _QualityInspectionModel1.qc_dt = DateTime.Now;
                    _QualityInspectionModel1.src_doc_date = null;
                    var other = new CommonController(_Common_IServices);

                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    _QualityInspectionModel1.MenuDocumentId = DocumentMenuId;

                    /*Commented and Modify  by Hina on 21-08-2024 to for all procs in 1 Procedure*/
                    //List<RejectWarehouse> _RejectWarehouseList = new List<RejectWarehouse>();
                    //dt = GetRejectWHList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    RejectWarehouse _RejectWarehouse = new RejectWarehouse();
                    //    _RejectWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _RejectWarehouse.wh_val = dr["wh_val"].ToString();
                    //    _RejectWarehouseList.Add(_RejectWarehouse);
                    //}
                    //_RejectWarehouseList.Insert(0, new RejectWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    //_QualityInspectionModel1.RejectWarehouseList = _RejectWarehouseList;

                    //dt = GetSourceAndAcceptWHList();
                    //List<SourceWarehouse> sourceWarehousesList = new List<SourceWarehouse>();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    SourceWarehouse _SourceWarehouse = new SourceWarehouse();
                    //    _SourceWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _SourceWarehouse.wh_val = dr["wh_val"].ToString();
                    //    sourceWarehousesList.Add(_SourceWarehouse);
                    //}
                    //sourceWarehousesList.Insert(0, new SourceWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    //_QualityInspectionModel1.SourceWarehouseList = sourceWarehousesList;

                    //List<AcceptedWarehouse> AcceptedWarehousesList = new List<AcceptedWarehouse>();

                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    AcceptedWarehouse _AcceptedWarehouse = new AcceptedWarehouse();
                    //    _AcceptedWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _AcceptedWarehouse.wh_val = dr["wh_val"].ToString();
                    //    AcceptedWarehousesList.Add(_AcceptedWarehouse);
                    //}
                    //AcceptedWarehousesList.Insert(0, new AcceptedWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    //_QualityInspectionModel1.AcceptedWarehouseList = AcceptedWarehousesList;

                    //List<ReworkWarehouse> _ReworkWarehouseList = new List<ReworkWarehouse>();
                    //dt = GetReworkWHList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ReworkWarehouse _ReworkWarehouse = new ReworkWarehouse();
                    //    _ReworkWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _ReworkWarehouse.wh_val = dr["wh_val"].ToString();
                    //    _ReworkWarehouseList.Add(_ReworkWarehouse);
                    //}
                    //_ReworkWarehouseList.Insert(0, new ReworkWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    //_QualityInspectionModel1.ReworkWarehouseList = _ReworkWarehouseList;

                    ds = GetSourceAndAcceptRejctRewrkWHAndShopfloorList();

                    List<SupplierName> supplierName = new List<SupplierName>();
                    if (_QualityInspectionModel1.qc_type == "PUR" || _QualityInspectionModel1.qc_type == "RQC")
                    {

                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            SupplierName _SupplierName = new SupplierName();
                            _SupplierName.supp_id = dr["supp_id"].ToString();
                            _SupplierName.supp_name = dr["supp_name"].ToString();
                            supplierName.Add(_SupplierName);
                        }
                    }

                    supplierName.Insert(0, new SupplierName() { supp_id = "0", supp_name = "---Select---" });
                    _QualityInspectionModel1.SupplierNameList = supplierName;


                    List<SourceWarehouse> sourceWarehousesList = new List<SourceWarehouse>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SourceWarehouse _SourceWarehouse = new SourceWarehouse();
                        _SourceWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _SourceWarehouse.wh_val = dr["wh_val"].ToString();
                        sourceWarehousesList.Add(_SourceWarehouse);
                    }
                    sourceWarehousesList.Insert(0, new SourceWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    _QualityInspectionModel1.SourceWarehouseList = sourceWarehousesList;

                    List<AcceptedWarehouse> AcceptedWarehousesList = new List<AcceptedWarehouse>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AcceptedWarehouse _AcceptedWarehouse = new AcceptedWarehouse();
                        _AcceptedWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _AcceptedWarehouse.wh_val = dr["wh_val"].ToString();
                        AcceptedWarehousesList.Add(_AcceptedWarehouse);
                    }
                    AcceptedWarehousesList.Insert(0, new AcceptedWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    _QualityInspectionModel1.AcceptedWarehouseList = AcceptedWarehousesList;

                    List<RejectWarehouse> _RejectWarehouseList = new List<RejectWarehouse>();
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        RejectWarehouse _RejectWarehouse = new RejectWarehouse();
                        _RejectWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _RejectWarehouse.wh_val = dr["wh_val"].ToString();
                        _RejectWarehouseList.Add(_RejectWarehouse);
                    }
                    _RejectWarehouseList.Insert(0, new RejectWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    _QualityInspectionModel1.RejectWarehouseList = _RejectWarehouseList;

                    List<ReworkWarehouse> _ReworkWarehouseList = new List<ReworkWarehouse>();
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        ReworkWarehouse _ReworkWarehouse = new ReworkWarehouse();
                        _ReworkWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _ReworkWarehouse.wh_val = dr["wh_val"].ToString();
                        _ReworkWarehouseList.Add(_ReworkWarehouse);
                    }
                    _ReworkWarehouseList.Insert(0, new ReworkWarehouse() { wh_id = 0, wh_val = "---Select---" });
                    _QualityInspectionModel1.ReworkWarehouseList = _ReworkWarehouseList;

                    List<SourceShopfloor> _ShopfloorList = new List<SourceShopfloor>();/*Add By Hina on 21-08-2024 for if Random QC has to be by Shopfloor*/
                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        SourceShopfloor _shopfloor = new SourceShopfloor();
                        _shopfloor.shfl_id = Convert.ToInt32(dr["shfl_id"]);
                        _shopfloor.shfl_val = dr["shfl_val"].ToString();
                        _ShopfloorList.Add(_shopfloor);
                    }
                    _ShopfloorList.Insert(0, new SourceShopfloor() { shfl_id = 0, shfl_val = "---Select---" });
                    _QualityInspectionModel1.SourceShopfloorList = _ShopfloorList;


                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _QualityInspectionModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        _QualityInspectionModel1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (_QualityInspectionModel1.TransType != null)
                    {
                        //if (Session["TransType"].ToString() == "Update")
                        if (_QualityInspectionModel1.TransType == "Update")
                        {
                            string qc_no = _QualityInspectionModel1.qc_no;
                            string QcType = _QualityInspectionModel1.qc_type;
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            _QualityInspectionModel1.MenuDocumentId = DocumentMenuId;
                            DataSet ds = _QualityInspection_ISERVICES.GetQcInspectionDetail(CompID, qc_no, BrchID, userid, DocumentMenuId);
                            ViewBag.AttechmentDetails = ds.Tables[7];
                            ViewBag.SubItemDetails = ds.Tables[8];
                            _QualityInspectionModel1.qc_no = ds.Tables[0].Rows[0]["qc_no"].ToString();
                            _QualityInspectionModel1.qc_dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["qc_dt"].ToString());
                            _QualityInspectionModel1.qc_type = ds.Tables[0].Rows[0]["qc_type"].ToString();
                            _QualityInspectionModel1.batch_no = ds.Tables[0].Rows[0]["batch_no"].ToString();
                            _QualityInspectionModel1.qc_remarks = ds.Tables[0].Rows[0]["qc_remarks"].ToString();
                            _QualityInspectionModel1.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                            _QualityInspectionModel1.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                            _QualityInspectionModel1.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                            _QualityInspectionModel1.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                            _QualityInspectionModel1.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                            _QualityInspectionModel1.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                            _QualityInspectionModel1.qc_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                            _QualityInspectionModel1.qc_statuscode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                            _QualityInspectionModel1.SourceWH = Convert.ToInt32(ds.Tables[0].Rows[0]["source_wh_id"].ToString());
                            _QualityInspectionModel1.AcceptedWH = Convert.ToInt32(ds.Tables[0].Rows[0]["accept_wh_id"].ToString());
                            _QualityInspectionModel1.RejectWH = Convert.ToInt32(ds.Tables[0].Rows[0]["reject_wh_id"].ToString());
                            _QualityInspectionModel1.ReworkWH = Convert.ToInt32(ds.Tables[0].Rows[0]["rework_wh_id"].ToString());
                            _QualityInspectionModel1.Location_type = ds.Tables[0].Rows[0]["loc"].ToString();
                            if (_QualityInspectionModel1.Location_type == "SF")
                            {
                                _QualityInspectionModel1.SourceSF = Convert.ToInt32(ds.Tables[0].Rows[0]["source_wh_id"].ToString());
                            }
                            else
                            {
                                _QualityInspectionModel1.SourceWH = Convert.ToInt32(ds.Tables[0].Rows[0]["source_wh_id"].ToString());
                            }

                            _QualityInspectionModel1.QCLotBatchdetails = DataTableToJSONWithStringBuilder(ds.Tables[6]);

                            if (_QualityInspectionModel1.qc_statuscode == "C")
                            {
                                _QualityInspectionModel1.CancelFlag = true;
                                _QualityInspectionModel1.BtnName = "Refresh";
                            }
                            else
                            {
                                _QualityInspectionModel1.CancelFlag = false;
                            }
                            _QualityInspectionModel1.src_doc_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["src_doc_date"]);
                            _QualityInspectionModel1.src_doc_no = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                            DocumentNumber objDocumentNumber = new DocumentNumber();
                            objDocumentNumber.doc_no = _QualityInspectionModel1.src_doc_no;
                            objDocumentNumber.doc_dt = _QualityInspectionModel1.src_doc_no;
                            if (!_QualityInspectionModel1.DocumentNumberList.Contains(objDocumentNumber))
                            {
                                _QualityInspectionModel1.DocumentNumberList.Add(objDocumentNumber);
                            }
                            //Session["DocumentStatus"] = _QualityInspectionModel1.qc_statuscode;
                            _QualityInspectionModel1.DocumentStatus = _QualityInspectionModel1.qc_statuscode;
                            //ViewBag.MenuPageName = getDocumentName();
                            _QualityInspectionModel1.Title = title;
                            ViewBag.ItemDetails = ds.Tables[1];
                            ViewBag.ItemDetailsQc = ds.Tables[2];
                            ViewBag.DocumentCode = _QualityInspectionModel1.qc_statuscode;
                            _QualityInspectionModel1.QCItemParamdetails = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                            //ViewBag.VBRoleList = GetRoleList();

                            string Statuscode = _QualityInspectionModel1.qc_statuscode;
                            string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                            string UserID = Session["UserID"].ToString();
                            string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();

                            _QualityInspectionModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            _QualityInspectionModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                            _QualityInspectionModel1.create_id = Convert.ToInt32(create_id);
                            if (Statuscode != "D" && Statuscode != "F")
                            {
                                ViewBag.AppLevel = ds.Tables[5];
                            }
                            //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                            if (ViewBag.AppLevel != null && _QualityInspectionModel1.Command != "Edit")
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
                                if (Statuscode == "D")
                                {
                                    if (create_id != UserID)
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _QualityInspectionModel1.BtnName = "BtnRefresh";
                                    }
                                    else
                                    {
                                        if (nextLevel == "0")
                                        {
                                            if (create_id == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _QualityInspectionModel1.BtnName = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            ViewBag.Approve = "N";
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _QualityInspectionModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _QualityInspectionModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _QualityInspectionModel1.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                }
                                if (Statuscode == "F")
                                {
                                    if (UserID == sent_to)
                                    {
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _QualityInspectionModel1.BtnName = "BtnToDetailPage";
                                    }
                                    if (nextLevel == "0")
                                    {
                                        if (sent_to == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _QualityInspectionModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (Statuscode == "A")
                                {
                                    if (create_id == UserID || approval_id == UserID)
                                    {
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _QualityInspectionModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        //Session["BtnName"] = "BtnRefresh";
                                        _QualityInspectionModel1.BtnName = "BtnRefresh";
                                    }
                                }
                            }
                            if (ViewBag.AppLevel.Rows.Count == 0)
                            {
                                ViewBag.Approve = "Y";
                            }
                            BindQCType(_QualityInspectionModel1);
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionDetail.cshtml", _QualityInspectionModel1);
                        }
                        else
                        {

                            //ViewBag.MenuPageName = getDocumentName();
                            //ViewBag.VBRoleList = GetRoleList();
                            _QualityInspectionModel1.Title = title;
                            //Session["DocumentStatus"] = "New";
                            _QualityInspectionModel1.DocumentStatus = "New";
                            BindQCType(_QualityInspectionModel1);

                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionDetail.cshtml", _QualityInspectionModel1);
                        }
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        //ViewBag.VBRoleList = GetRoleList();
                        _QualityInspectionModel1.Title = title;
                        //Session["DocumentStatus"] = "New";
                        _QualityInspectionModel1.DocumentStatus = "New";
                        BindQCType(_QualityInspectionModel1);
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionDetail.cshtml", _QualityInspectionModel1);
                    }
                }      
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void BindQCType(QualityInspectionModel _model)
        {
            List<QCTypeList> _QCtypelist = new List<QCTypeList>();
            if (DocumentMenuId == "105102120") //For Inventory
            {
                _QCtypelist.Add(new QCTypeList { QCType_id = "PUR", QCType_val = "Purchase" });
               // _QCtypelist.Add(new QCTypeList { QCType_id = "RQC", QCType_val = "Random QC" });
                _QCtypelist.Add(new QCTypeList { QCType_id = "RQC", QCType_val = "Ad-Hoc QC" });
                _QCtypelist.Add(new QCTypeList { QCType_id = "SMR", QCType_val = "Sample Receipt" });
                _QCtypelist.Add(new QCTypeList { QCType_id = "FGR", QCType_val = "Finished Goods Receipt" });
            }
            else if (DocumentMenuId == "105105135") //For Production
            {
                _QCtypelist.Add(new QCTypeList { QCType_id = "PRD", QCType_val = "Production" });
                _QCtypelist.Add(new QCTypeList { QCType_id = "RWK", QCType_val = "Rework" });
                _QCtypelist.Add(new QCTypeList { QCType_id = "PJO", QCType_val = "Packaging" });
              //  _QCtypelist.Add(new QCTypeList { QCType_id = "FGR", QCType_val = "Finished Goods Receipt" });
            }
            else //For SubContract
            {
                _QCtypelist.Add(new QCTypeList { QCType_id = "SCQ", QCType_val = "Sub-Contracting" });
            }
            _model._QCTypeList = _QCtypelist;
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
                    userid = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BrchID, userid, DocumentMenuId, language);
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
        public ActionResult AddNewQualityInspection(string DocumentMenuId,string QC_Type)
        {
            QualityInspectionModel _QualityInspectionModel = new QualityInspectionModel();
            _QualityInspectionModel.Message = "New";
            _QualityInspectionModel.Command = "Add";
            _QualityInspectionModel.AppStatus = "D";
            _QualityInspectionModel.TransType = "Save";
            _QualityInspectionModel.BtnName = "BtnAddNew";
            _QualityInspectionModel.DocumentMenuId = DocumentMenuId;
            TempData["ModelData"] = _QualityInspectionModel;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.DocumentMenuId= DocumentMenuId;
            if(DocumentMenuId== "105102120") 
                URLModel.QC_Type= "PUR";
            else
                URLModel.QC_Type = QC_Type;
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnAddNew";
            URLModel.TransType = "Save";
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                if(DocumentMenuId== "105102120")
                {return RedirectToAction("QualityInspectionListInv");}
                else if (DocumentMenuId == "105105135")
                { return RedirectToAction("QualityInspectionListPrd"); }
                else if (DocumentMenuId == "105108115")
                { return RedirectToAction("QualityInspectionListSC"); }
             }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("QualityInspectionDetail", "QualityInspection", URLModel);
        }
        public ActionResult EditQualityInspection(string QCId,string QCDt, string QCType, string ListFilterData, string DocumentMenuId, string WF_status)
        {/*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
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
            QualityInspectionModel _QualityInspectionModel = new QualityInspectionModel();
            _QualityInspectionModel.Message = "New";
            _QualityInspectionModel.Command = "Add";
            _QualityInspectionModel.qc_no = QCId;
            _QualityInspectionModel.qc_dt = Convert.ToDateTime(QCDt);
            _QualityInspectionModel.qc_type = QCType;
           _QualityInspectionModel.TransType = "Update";
            _QualityInspectionModel.AppStatus = "D";
            _QualityInspectionModel.DocumentMenuId = DocumentMenuId;
            _QualityInspectionModel.BtnName = "BtnToDetailPage";
            _QualityInspectionModel.WF_status1 = WF_status;
            TempData["WF_status1"] = WF_status; 
            TempData["ModelData"] = _QualityInspectionModel;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.DocNo = QCId;
            URLModel.DocDate = QCDt;
            URLModel.QC_Type = QCType;
            TempData["WF_status1"] = WF_status;
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.Command = "Add";
            URLModel.DocumentMenuId = DocumentMenuId;
            URLModel.WF_status1 = WF_status;
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["QualityInspection"] = QCId;
            //Session["QCType"] = QCType;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("QualityInspectionDetail", "QualityInspection", URLModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveQualityInspection(QualityInspectionModel _QualityInspectionModel, string qc_no, string command, HttpPostedFileBase[] QCFiles)
        {
            try
            {/*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                //Session["MenuDocumentId"] = DocumentMenuId;
                //Session["QCType"] = _QualityInspectionModel.qc_type;
                if (_QualityInspectionModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNewQCInsp":
                        QualityInspectionModel _QualityInspectionModelAddNew = new QualityInspectionModel();
                        _QualityInspectionModelAddNew.AppStatus = "D";
                        _QualityInspectionModelAddNew.BtnName = "BtnAddNew";
                        _QualityInspectionModelAddNew.TransType = "Save";
                        _QualityInspectionModelAddNew.Command = "New";
                        _QualityInspectionModelAddNew.DocumentMenuId = DocumentMenuId;
                        TempData["ModelData"] = _QualityInspectionModelAddNew;
                        URLDetailModel URLModel = new URLDetailModel();
                        URLModel.DocumentMenuId = DocumentMenuId;
                        URLModel.QC_Type = _QualityInspectionModel.qc_type;
                        URLModel.Command = "New";
                        URLModel.BtnName = "BtnAddNew";
                        URLModel.TransType = "Save";
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_QualityInspectionModel.qc_no))
                                return RedirectToAction("EditQualityInspection", new { QCId = _QualityInspectionModel.qc_no, QCDt = _QualityInspectionModel.qc_dt, QCType = _QualityInspectionModel.qc_type, ListFilterData = _QualityInspectionModel.ListFilterData1, DocumentMenuId=_QualityInspectionModel.DocumentMenuId, WF_status = _QualityInspectionModel.WFStatus });
                            else
                                _QualityInspectionModelAddNew.Command = "Refresh";
                            _QualityInspectionModelAddNew.TransType = "Refresh";
                            _QualityInspectionModelAddNew.BtnName = "Refresh";
                            _QualityInspectionModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = _QualityInspectionModelAddNew;
                            return RedirectToAction("QualityInspectionDetail", "QualityInspection", _QualityInspectionModelAddNew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("QualityInspectionDetail", "QualityInspection", URLModel);

                    case "Edit":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditQualityInspection", new { QCId = _QualityInspectionModel.qc_no, QCDt = _QualityInspectionModel.qc_dt, QCType = _QualityInspectionModel.qc_type, ListFilterData = _QualityInspectionModel.ListFilterData1, DocumentMenuId = _QualityInspectionModel.DocumentMenuId, WF_status = _QualityInspectionModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string QcDt = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, QcDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditQualityInspection", new { QCId = _QualityInspectionModel.qc_no, QCDt = _QualityInspectionModel.qc_dt, QCType = _QualityInspectionModel.qc_type, ListFilterData = _QualityInspectionModel.ListFilterData1, DocumentMenuId = _QualityInspectionModel.DocumentMenuId, WF_status = _QualityInspectionModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        URLDetailModel URLModelEdit = new URLDetailModel();
                        if(_QualityInspectionModel.src_doc_no != null && _QualityInspectionModel.src_doc_date !=null)/*add by Hina on 28-08-2024 to random qc for shopfloor*/
                        {
                            if (CheckGRNAgainstQC(_QualityInspectionModel.src_doc_no, Convert.ToDateTime(_QualityInspectionModel.src_doc_date.ToString()).ToString("yyyy-MM-dd")) == "Used")
                            {
                                _QualityInspectionModel.Message = "Used";
                                _QualityInspectionModel.BtnName = "BtnToDetailPage";
                                _QualityInspectionModel.Command = "Refresh";
                                URLModelEdit.TransType = _QualityInspectionModel.TransType;
                                URLModelEdit.Command = "Refresh";// _QualityInspectionModel.Command;
                                URLModelEdit.BtnName = "BtnToDetailPage";
                                URLModelEdit.DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                                URLModelEdit.QC_Type = _QualityInspectionModel.qc_type;
                                URLModelEdit.DocNo = _QualityInspectionModel.qc_no;
                                URLModelEdit.DocDate = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                                TempData["ModelData"] = _QualityInspectionModel;
                                //Session["Message"] = "Used";
                            }
                            else
                            {
                                _QualityInspectionModel.TransType = "Update";
                                _QualityInspectionModel.Command = command;
                                _QualityInspectionModel.BtnName = "BtnEdit";
                                _QualityInspectionModel.Message = null;

                                TempData["ModelData"] = _QualityInspectionModel;
                                URLModelEdit.TransType = _QualityInspectionModel.TransType;
                                URLModelEdit.Command = _QualityInspectionModel.Command;
                                URLModelEdit.BtnName = _QualityInspectionModel.BtnName;
                                URLModelEdit.DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                                URLModelEdit.QC_Type = _QualityInspectionModel.qc_type;
                                URLModelEdit.DocNo = _QualityInspectionModel.qc_no;
                                URLModelEdit.DocDate = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                                
                            }
                        }
                        else
                        {
                            _QualityInspectionModel.TransType = "Update";
                            _QualityInspectionModel.Command = command;
                            _QualityInspectionModel.BtnName = "BtnEdit";
                            _QualityInspectionModel.Message = null;

                            TempData["ModelData"] = _QualityInspectionModel;
                            URLModelEdit.TransType = _QualityInspectionModel.TransType;
                            URLModelEdit.Command = _QualityInspectionModel.Command;
                            URLModelEdit.BtnName = _QualityInspectionModel.BtnName;
                            URLModelEdit.DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                            URLModelEdit.QC_Type = _QualityInspectionModel.qc_type;
                            URLModelEdit.DocNo = _QualityInspectionModel.qc_no;
                            URLModelEdit.DocDate = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                            
                        }
                        TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                        return RedirectToAction("QualityInspectionDetail", URLModelEdit);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        //qc_no = _QualityInspectionModel.qc_no;
                        QualityInspectionModel _QualityInspectionModelDelete = new QualityInspectionModel();
                        URLDetailModel URLModelDelete = new URLDetailModel();
                        QCInspectionDelete(_QualityInspectionModel, command);
                        _QualityInspectionModelDelete.Message = "Deleted";
                        _QualityInspectionModelDelete.Command = "Refresh";
                        _QualityInspectionModelDelete.TransType = command;
                        _QualityInspectionModelDelete.AppStatus = "D";
                        _QualityInspectionModelDelete.BtnName = "BtnDelete";

                        URLModelDelete.DocumentMenuId = DocumentMenuId;
                        URLModelDelete.QC_Type = _QualityInspectionModelDelete.qc_type;
                        URLModelDelete.Command = "Refresh";
                        URLModelDelete.BtnName = "BtnDelete";
                        URLModelDelete.TransType = command;
                        TempData["ModelData"] = _QualityInspectionModelDelete;
                        TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                        return RedirectToAction("QualityInspectionDetail", URLModelDelete);

                    case "Save":
                        //Session["Command"] = command;
                        _QualityInspectionModel.Command = command;
                        if (ModelState.IsValid)
                        {
                            SaveQualityInspectionDetail(_QualityInspectionModel, QCFiles);
                            if (_QualityInspectionModel.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            if (_QualityInspectionModel.Message == "DocModify")
                            {
                                ViewBag.DocumentMenuId = DocumentMenuId;
                                ViewBag.DocumentStatus = "D";
                                ViewBag.DocumentCode = "D";

                                _QualityInspectionModel.DocumentMenuId = DocumentMenuId;
                                CommonPageDetails();
                                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                                DocumentNumber _DocumentNumber = new DocumentNumber();
                                _DocumentNumber.doc_no = _QualityInspectionModel.src_doc_no;
                                _DocumentNumber.doc_dt = "0";
                                _DocumentNumberList.Add(_DocumentNumber);

                                _QualityInspectionModel.DocumentNumberList = _DocumentNumberList;
                               
                                _QualityInspectionModel.qc_dt = DateTime.Now;
                               
                               
                                ViewBag.DocumentMenuId = DocumentMenuId;
                                _QualityInspectionModel.MenuDocumentId = DocumentMenuId;

                                /*Commented and modify by Hina on 21-08-2024 for all procs in 1 Procedure */
                                //List<RejectWarehouse> _RejectWarehouseList = new List<RejectWarehouse>();
                                //dt = GetRejectWHList();
                                //foreach (DataRow dr in dt.Rows)
                                //{
                                //    RejectWarehouse _RejectWarehouse = new RejectWarehouse();
                                //    _RejectWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                //    _RejectWarehouse.wh_val = dr["wh_val"].ToString();
                                //    _RejectWarehouseList.Add(_RejectWarehouse);
                                //}
                                //_RejectWarehouseList.Insert(0, new RejectWarehouse() { wh_id = 0, wh_val = "---Select---" });
                                //_QualityInspectionModel.RejectWarehouseList = _RejectWarehouseList;
                                //dt = GetSourceAndAcceptWHList();
                                //List<SourceWarehouse> sourceWarehousesList = new List<SourceWarehouse>();
                                //foreach (DataRow dr in dt.Rows)
                                //{
                                //    SourceWarehouse _SourceWarehouse = new SourceWarehouse();
                                //    _SourceWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                //    _SourceWarehouse.wh_val = dr["wh_val"].ToString();
                                //    sourceWarehousesList.Add(_SourceWarehouse);
                                //}
                                //sourceWarehousesList.Insert(0, new SourceWarehouse() { wh_id = 0, wh_val = "---Select---" });
                                //_QualityInspectionModel.SourceWarehouseList = sourceWarehousesList;

                                //List<AcceptedWarehouse> AcceptedWarehousesList = new List<AcceptedWarehouse>();

                                //foreach (DataRow dr in dt.Rows)
                                //{
                                //    AcceptedWarehouse _AcceptedWarehouse = new AcceptedWarehouse();
                                //    _AcceptedWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                //    _AcceptedWarehouse.wh_val = dr["wh_val"].ToString();
                                //    AcceptedWarehousesList.Add(_AcceptedWarehouse);
                                //}
                                //AcceptedWarehousesList.Insert(0, new AcceptedWarehouse() { wh_id = 0, wh_val = "---Select---" });
                                //_QualityInspectionModel.AcceptedWarehouseList = AcceptedWarehousesList;

                                //List<ReworkWarehouse> _ReworkWarehouseList = new List<ReworkWarehouse>();
                                //dt = GetReworkWHList();
                                //foreach (DataRow dr in dt.Rows)
                                //{
                                //    ReworkWarehouse _ReworkWarehouse = new ReworkWarehouse();
                                //    _ReworkWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                //    _ReworkWarehouse.wh_val = dr["wh_val"].ToString();
                                //    _ReworkWarehouseList.Add(_ReworkWarehouse);
                                //}
                                //_ReworkWarehouseList.Insert(0, new ReworkWarehouse() { wh_id = 0, wh_val = "---Select---" });
                                //_QualityInspectionModel.ReworkWarehouseList = _ReworkWarehouseList;

                                ds = GetSourceAndAcceptRejctRewrkWHAndShopfloorList();

                                List<SupplierName> supplierName = new List<SupplierName>();
                                if (_QualityInspectionModel.qc_type == "PUR" || _QualityInspectionModel.qc_type == "RQC")
                                {

                                    foreach (DataRow dr in ds.Tables[4].Rows)
                                    {
                                        SupplierName _SupplierName = new SupplierName();
                                        _SupplierName.supp_id = dr["supp_id"].ToString();
                                        _SupplierName.supp_name = dr["supp_name"].ToString();
                                        supplierName.Add(_SupplierName);
                                    }
                                }

                                supplierName.Insert(0, new SupplierName() { supp_id = "0", supp_name = "---Select---" });
                                _QualityInspectionModel.SupplierNameList = supplierName;


                                List<SourceWarehouse> sourceWarehousesList = new List<SourceWarehouse>();
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    SourceWarehouse _SourceWarehouse = new SourceWarehouse();
                                    _SourceWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                    _SourceWarehouse.wh_val = dr["wh_val"].ToString();
                                    sourceWarehousesList.Add(_SourceWarehouse);
                                }
                                sourceWarehousesList.Insert(0, new SourceWarehouse() { wh_id = 0, wh_val = "---Select---" });
                                _QualityInspectionModel.SourceWarehouseList = sourceWarehousesList;

                                List<AcceptedWarehouse> AcceptedWarehousesList = new List<AcceptedWarehouse>();

                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    AcceptedWarehouse _AcceptedWarehouse = new AcceptedWarehouse();
                                    _AcceptedWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                    _AcceptedWarehouse.wh_val = dr["wh_val"].ToString();
                                    AcceptedWarehousesList.Add(_AcceptedWarehouse);
                                }
                                AcceptedWarehousesList.Insert(0, new AcceptedWarehouse() { wh_id = 0, wh_val = "---Select---" });
                                _QualityInspectionModel.AcceptedWarehouseList = AcceptedWarehousesList;

                                List<RejectWarehouse> _RejectWarehouseList = new List<RejectWarehouse>();
                                foreach (DataRow dr in ds.Tables[1].Rows)
                                {
                                    RejectWarehouse _RejectWarehouse = new RejectWarehouse();
                                    _RejectWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                    _RejectWarehouse.wh_val = dr["wh_val"].ToString();
                                    _RejectWarehouseList.Add(_RejectWarehouse);
                                }
                                _RejectWarehouseList.Insert(0, new RejectWarehouse() { wh_id = 0, wh_val = "---Select---" });
                                _QualityInspectionModel.RejectWarehouseList = _RejectWarehouseList;

                                List<ReworkWarehouse> _ReworkWarehouseList = new List<ReworkWarehouse>();
                                foreach (DataRow dr in ds.Tables[2].Rows)
                                {
                                    ReworkWarehouse _ReworkWarehouse = new ReworkWarehouse();
                                    _ReworkWarehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                    _ReworkWarehouse.wh_val = dr["wh_val"].ToString();
                                    _ReworkWarehouseList.Add(_ReworkWarehouse);
                                }
                                _ReworkWarehouseList.Insert(0, new ReworkWarehouse() { wh_id = 0, wh_val = "---Select---" });
                                _QualityInspectionModel.ReworkWarehouseList = _ReworkWarehouseList;

                                List<SourceShopfloor> _ShopfloorList = new List<SourceShopfloor>();/*Add By Hina on 21-08-2024 for if Random QC has to be by Shopfloor*/
                                foreach (DataRow dr in ds.Tables[3].Rows)
                                {
                                    SourceShopfloor _shopfloor = new SourceShopfloor();
                                    _shopfloor.shfl_id = Convert.ToInt32(dr["shfl_id"]);
                                    _shopfloor.shfl_val = dr["shfl_val"].ToString();
                                    _ShopfloorList.Add(_shopfloor);
                                }
                                _ShopfloorList.Insert(0, new SourceShopfloor() { shfl_id = 0, shfl_val = "---Select---" });
                                _QualityInspectionModel.SourceShopfloorList = _ShopfloorList;


                                _QualityInspectionModel.src_doc_no = _QualityInspectionModel.src_doc_no;
                                _QualityInspectionModel.src_doc_date = _QualityInspectionModel.src_doc_date;
                                ViewBag.ItemDetails = ViewData["ItemDetails"];
                                ViewBag.ItemDetailsQc = ViewData["ParamItemDetails"];
                                ViewBag.ItemStockBatchWise = ViewData["LotBatchDetails"];
                                ViewBag.SubItemDetails = ViewData["SubitemDetails"];

                                //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                                _QualityInspectionModel.BtnName = "Refresh";
                                _QualityInspectionModel.Command = "Refresh";
                                _QualityInspectionModel.DocumentStatus = "D";

                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionDetail.cshtml", _QualityInspectionModel);
                            }
                            else if (_QualityInspectionModel.Message == "Used")
                            {
                                TempData["ModelData"] = _QualityInspectionModel;
                                URLDetailModel URLModelSave = new URLDetailModel();
                                URLModelSave.TransType = _QualityInspectionModel.TransType;
                                URLModelSave.Command = _QualityInspectionModel.Command;
                                URLModelSave.BtnName = _QualityInspectionModel.BtnName;
                                URLModelSave.DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                                URLModelSave.QC_Type = _QualityInspectionModel.qc_type;
                                URLModelSave.DocNo = _QualityInspectionModel.qc_no;
                                URLModelSave.DocDate = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                                //Session["QualityInspection"] = Session["QualityInspection"].ToString();
                                TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                                return RedirectToAction("QualityInspectionDetail", URLModelSave);
                            }
                            else
                            {
                                TempData["ModelData"] = _QualityInspectionModel;
                                URLDetailModel URLModelSave = new URLDetailModel();
                                URLModelSave.TransType = _QualityInspectionModel.TransType;
                                URLModelSave.Command = _QualityInspectionModel.Command;
                                URLModelSave.BtnName = _QualityInspectionModel.BtnName;
                                URLModelSave.DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                                URLModelSave.QC_Type = _QualityInspectionModel.qc_type;
                                URLModelSave.DocNo = _QualityInspectionModel.qc_no; 
                                URLModelSave.DocDate = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                                //Session["QualityInspection"] = Session["QualityInspection"].ToString();
                                TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                                return RedirectToAction("QualityInspectionDetail", URLModelSave);
                            }
                        }
                        else
                        {
                            _QualityInspectionModel.ItemDetailsList = null;
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionList.cshtml", _QualityInspectionModel);
                        }

                    case "Forward":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditQualityInspection", new { QCId = _QualityInspectionModel.qc_no, QCDt = _QualityInspectionModel.qc_dt, QCType = _QualityInspectionModel.qc_type, ListFilterData = _QualityInspectionModel.ListFilterData1, DocumentMenuId = _QualityInspectionModel.DocumentMenuId, WF_status = _QualityInspectionModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string QcDt1 = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, QcDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditQualityInspection", new { QCId = _QualityInspectionModel.qc_no, QCDt = _QualityInspectionModel.qc_dt, QCType = _QualityInspectionModel.qc_type, ListFilterData = _QualityInspectionModel.ListFilterData1, DocumentMenuId = _QualityInspectionModel.DocumentMenuId, WF_status = _QualityInspectionModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 21-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditQualityInspection", new { QCId = _QualityInspectionModel.qc_no, QCDt = _QualityInspectionModel.qc_dt, QCType = _QualityInspectionModel.qc_type, ListFilterData = _QualityInspectionModel.ListFilterData1, DocumentMenuId = _QualityInspectionModel.DocumentMenuId, WF_status = _QualityInspectionModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string QcDt2 = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, QcDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditQualityInspection", new { QCId = _QualityInspectionModel.qc_no, QCDt = _QualityInspectionModel.qc_dt, QCType = _QualityInspectionModel.qc_type, ListFilterData = _QualityInspectionModel.ListFilterData1, DocumentMenuId = _QualityInspectionModel.DocumentMenuId, WF_status = _QualityInspectionModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _QualityInspectionModel.Command = command;
                        //qc_no = _QualityInspectionModel.qc_no;
                        //Session["QualityInspection"] = qc_no;
                        QCInspectionApprove(_QualityInspectionModel, command,"","","","","");
                        TempData["ModelData"] = _QualityInspectionModel;
                        URLDetailModel URLModelApprove = new URLDetailModel();
                        URLModelApprove.TransType = _QualityInspectionModel.TransType;
                        URLModelApprove.Command = _QualityInspectionModel.Command;
                        URLModelApprove.BtnName = _QualityInspectionModel.BtnName;
                        URLModelApprove.DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                        URLModelApprove.QC_Type = _QualityInspectionModel.qc_type;
                        URLModelApprove.DocNo = _QualityInspectionModel.qc_no;
                        URLModelApprove.DocDate = _QualityInspectionModel.qc_dt.ToString("yyyy-MM-dd");
                        TempData["WF_status1"] = _QualityInspectionModel.WF_status1;
                        TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                        return RedirectToAction("QualityInspectionDetail", URLModelApprove);

                    case "Refresh":
                        QualityInspectionModel _QualityInspectionModelRefresh = new QualityInspectionModel();
                        URLDetailModel URLModelRefresh = new URLDetailModel();
                        _QualityInspectionModelRefresh.BtnName = "Refresh";
                        _QualityInspectionModelRefresh.Command = command;
                        _QualityInspectionModelRefresh.TransType = "Save";
                        _QualityInspectionModelRefresh.DocumentMenuId = DocumentMenuId;
                        _QualityInspectionModelRefresh.qc_type = _QualityInspectionModel.qc_type;
                        TempData["ModelData"] = _QualityInspectionModelRefresh;
                        //URLModelRefresh.TransType = _QualityInspectionModel.TransType;
                        URLModelRefresh.TransType = "Add";
                        URLModelRefresh.Command = _QualityInspectionModel.Command;
                        URLModelRefresh.BtnName = _QualityInspectionModel.BtnName;
                        URLModelRefresh.DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                        URLModelRefresh.QC_Type = _QualityInspectionModel.qc_type;
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                        return RedirectToAction("QualityInspectionDetail", URLModelRefresh);

                    case "Print":
                        return GenratePdfFile(_QualityInspectionModel);
                    case "BacktoList":
                        //Session.Remove("Message");// = null;
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        if (_QualityInspectionModel.DocumentMenuId == "105102120")
                        {
                            TempData["WF_status"] = _QualityInspectionModel.WF_status1;
                            TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                            return RedirectToAction("QualityInspectionListInv", "QualityInspection");
                        }
                        else
                        if (_QualityInspectionModel.DocumentMenuId == "105105135")
                        {
                            TempData["WF_status"] = _QualityInspectionModel.WF_status1;
                            TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                            return RedirectToAction("QualityInspectionListPrd", "QualityInspection");
                        }
                        else
                        if (_QualityInspectionModel.DocumentMenuId == "105108115")
                        {
                            TempData["WF_status"] = _QualityInspectionModel.WF_status1;
                            TempData["ListFilterData"] = _QualityInspectionModel.ListFilterData1;
                            return RedirectToAction("QualityInspectionListSC", "QualityInspection");
                        }
                        return RedirectToAction("QualityInspectionDetail");
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
        public ActionResult SaveQualityInspectionDetail(QualityInspectionModel _QualityInspectionModel, HttpPostedFileBase[] QCFiles)
        {
            string SaveMessage = "";
            //getDocumentName(); /* To set Title*/
            string PageName = _QualityInspectionModel.Title.Replace(" ", "");

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }

            try
            {
                if (_QualityInspectionModel.CancelFlag == false)
                {
                    DataTable QualityInspectionHeader = new DataTable();
                    DataTable QualityInspectionItemDetail = new DataTable();
                    DataTable QualityInspectionItemParamDetail = new DataTable();
                    DataTable Attachments = new DataTable();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("MenuDocumentId", typeof(string));
                    dt.Columns.Add("TransType", typeof(string));
                    dt.Columns.Add("comp_id", typeof(int));
                    dt.Columns.Add("br_id", typeof(int));
                    dt.Columns.Add("qc_no", typeof(string));
                    dt.Columns.Add("qc_dt", typeof(string));
                    dt.Columns.Add("qc_type", typeof(string));
                    dt.Columns.Add("src_doc_no", typeof(string));
                    dt.Columns.Add("src_doc_date", typeof(string));
                    dt.Columns.Add("batch_no", typeof(string));
                    dt.Columns.Add("qc_remarks", typeof(string));
                    dt.Columns.Add("create_id", typeof(string));
                    dt.Columns.Add("mod_id", typeof(string));
                    dt.Columns.Add("qc_status", typeof(string));
                    dt.Columns.Add("UserName", typeof(string));
                    dt.Columns.Add("UserMacaddress", typeof(string));
                    dt.Columns.Add("UserSystemName", typeof(string));
                    dt.Columns.Add("UserIP", typeof(string));
                    dt.Columns.Add("SourceWH", typeof(int));
                    dt.Columns.Add("AcceptedWH", typeof(int));
                    dt.Columns.Add("RejectWH", typeof(int));
                    dt.Columns.Add("ReworkWH", typeof(int));
                    dt.Columns.Add("Location", typeof(string));/*add by Hina on 23-08-2024 to random qc for shopfloor*/

                    DataRow dtrow = dt.NewRow();
                    //dtrow["MenuDocumentId"] = _QualityInspectionModel.DocumentMenuId;
                    dtrow["MenuDocumentId"] = DocumentMenuId;
                    dtrow["TransType"] = _QualityInspectionModel.TransType;
                    dtrow["comp_id"] = Session["CompId"].ToString();
                    dtrow["br_id"] = Session["BranchId"].ToString();
                    dtrow["qc_no"] = _QualityInspectionModel.qc_no;
                    dtrow["qc_dt"] = _QualityInspectionModel.qc_dt;
                    dtrow["qc_type"] = _QualityInspectionModel.qc_type;
                    if(_QualityInspectionModel.qc_type=="RQC")
                    { 
                        if(_QualityInspectionModel.Location_type=="SF")
                        {
                            dtrow["src_doc_no"] = 0;
                        }
                        else
                        {
                            dtrow["src_doc_no"] = _QualityInspectionModel.src_doc_no;
                        }

                    }
                    else
                    {
                        dtrow["src_doc_no"] = _QualityInspectionModel.src_doc_no;
                    }
                    //dtrow["src_doc_no"] = _QualityInspectionModel.src_doc_no;
                    if (_QualityInspectionModel.src_doc_date == null)
                    {
                        dtrow["src_doc_date"] = DateTime.Now;
                    }
                    else
                    {
                        dtrow["src_doc_date"] = _QualityInspectionModel.src_doc_date;
                    }
                    dtrow["batch_no"] = _QualityInspectionModel.batch_no;
                    dtrow["qc_remarks"] = _QualityInspectionModel.qc_remarks;
                    dtrow["create_id"] = Session["UserId"].ToString();
                    dtrow["mod_id"] = Session["UserId"].ToString();
                    //dtrow["qc_status"] = Session["AppStatus"].ToString();
                    dtrow["qc_status"] = "D";
                    dtrow["UserName"] = Session["UserName"].ToString();
                    dtrow["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrow["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrow["UserIP"] = Session["UserIP"].ToString();
                   
                    dtrow["AcceptedWH"] = _QualityInspectionModel.AcceptedWH;
                    dtrow["RejectWH"] = _QualityInspectionModel.RejectWH;
                    dtrow["ReworkWH"] = _QualityInspectionModel.ReworkWH;
                    dtrow["Location"] = _QualityInspectionModel.Location_type;/*add by Hina on 23-08-2024 to random qc for shopfloor*/
                    if(_QualityInspectionModel.Location_type=="SF")
                    {
                        dtrow["SourceWH"] = _QualityInspectionModel.SourceSF;
                    }
                    else
                    {
                        dtrow["SourceWH"] = _QualityInspectionModel.SourceWH;
                    }

                    dt.Rows.Add(dtrow);

                    QualityInspectionHeader = dt;


                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(Int32));
                    dtItem.Columns.Add("br_id", typeof(Int32));
                    dtItem.Columns.Add("qc_no", typeof(string));
                    dtItem.Columns.Add("qc_dt", typeof(string));
                    dtItem.Columns.Add("qc_type", typeof(string));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("recd_qty", typeof(string));
                    dtItem.Columns.Add("accept_qty", typeof(string));
                    dtItem.Columns.Add("reject_qty", typeof(string));
                    dtItem.Columns.Add("reason_rej", typeof(string));
                    dtItem.Columns.Add("rework_qty", typeof(string));
                    dtItem.Columns.Add("reason_rwk", typeof(string));
                    dtItem.Columns.Add("short_qty", typeof(string));
                    dtItem.Columns.Add("sample_qty", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));
                    dtItem.Columns.Add("prod_qty", typeof(string));
                    dtItem.Columns.Add("pend_qty", typeof(string));
                    dtItem.Columns.Add("reason_rej_id", typeof(string));
                    dtItem.Columns.Add("reason_acpt", typeof(string));
                    //DataRow dtrowItemdetails = dtItem.NewRow();

                    JArray jObject = JArray.Parse(_QualityInspectionModel.QCItemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtItem.NewRow();
                        decimal Recd_qty, Accept_qty, Reject_qty, Rework_qty,Short_qty,Sample_qty;

                        if (jObject[i]["RecivedQty"].ToString() == "")
                            Recd_qty = 0;
                        else
                            Recd_qty = Convert.ToDecimal(jObject[i]["RecivedQty"].ToString());

                        if (jObject[i]["AcceptQty"].ToString() == "")
                            Accept_qty = 0;
                        else
                            Accept_qty = Convert.ToDecimal(jObject[i]["AcceptQty"].ToString());

                        if (jObject[i]["RejectQty"].ToString() == "")
                            Reject_qty = 0;
                        else
                            Reject_qty = Convert.ToDecimal(jObject[i]["RejectQty"].ToString());

                        if (jObject[i]["ReworkableQty"].ToString() == "")
                            Rework_qty = 0;
                        else
                            Rework_qty = Convert.ToDecimal(jObject[i]["ReworkableQty"].ToString());

                        if (jObject[i]["ShortQty"].ToString() == "")
                            Short_qty = 0;
                        else
                            Short_qty = Convert.ToDecimal(jObject[i]["ShortQty"].ToString());

                        if (jObject[i]["SampleQty"].ToString() == "")
                            Sample_qty = 0;
                        else
                            Sample_qty = Convert.ToDecimal(jObject[i]["SampleQty"].ToString());


                        dtrowItemdetails["comp_id"] = Session["CompId"].ToString();
                        dtrowItemdetails["br_id"] = Session["BranchId"].ToString();
                        dtrowItemdetails["qc_no"] = _QualityInspectionModel.qc_no;
                        dtrowItemdetails["qc_dt"] = _QualityInspectionModel.qc_dt;
                        dtrowItemdetails["qc_type"] = _QualityInspectionModel.qc_type;
                        dtrowItemdetails["item_id"] = jObject[i]["ItemID"].ToString();
                        //string str = Convert.ToInt32(jObject[i]["UOMId"]).ToString();
                        dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMID"]);
                        dtrowItemdetails["recd_qty"] = Recd_qty;
                        dtrowItemdetails["accept_qty"] = Accept_qty;
                        dtrowItemdetails["reject_qty"] = Reject_qty;
                        dtrowItemdetails["reason_rej"] = jObject[i]["ReasonRej"].ToString();
                        dtrowItemdetails["rework_qty"] = Rework_qty;
                        dtrowItemdetails["reason_rwk"] = jObject[i]["ReasonRwk"].ToString();
                        dtrowItemdetails["short_qty"] = Short_qty;
                        dtrowItemdetails["sample_qty"] = Sample_qty;
                        dtrowItemdetails["it_remarks"] = jObject[i]["Remarks"].ToString();
                        if (_QualityInspectionModel.qc_type == "PRD")
                        {
                            dtrowItemdetails["prod_qty"] = jObject[i]["ProdQty"].ToString();
                            dtrowItemdetails["pend_qty"] = jObject[i]["PendQty"].ToString();
                        }
                        else
                        {
                            dtrowItemdetails["prod_qty"] = "0";
                            dtrowItemdetails["pend_qty"] = "0";
                        }
                        dtrowItemdetails["reason_rej_id"] = jObject[i]["ReasonRej_ID"].ToString();
                        dtrowItemdetails["reason_acpt"] = jObject[i]["ReasonAccept"].ToString();
                        dtItem.Rows.Add(dtrowItemdetails);
                    }
                    QualityInspectionItemDetail = dtItem;
                    ViewData["ItemDetails"] = dtitemdetail(jObject);

                    DataTable dtparam = new DataTable();

                    dtparam.Columns.Add("comp_id", typeof(int));
                    dtparam.Columns.Add("br_id", typeof(int));
                    dtparam.Columns.Add("qc_no", typeof(string));
                    dtparam.Columns.Add("qc_dt", typeof(string));
                    dtparam.Columns.Add("qc_type", typeof(string));
                    dtparam.Columns.Add("item_id", typeof(string));
                    dtparam.Columns.Add("uom_id", typeof(int));
                    dtparam.Columns.Add("sam_size", typeof(string));
                    dtparam.Columns.Add("param_Id", typeof(int));
                    dtparam.Columns.Add("param_uom_Id", typeof(int));/*Added by Suraj on 16-08-2023*/
                    dtparam.Columns.Add("upper_val", typeof(string));
                    dtparam.Columns.Add("lower_val", typeof(string));
                    dtparam.Columns.Add("param_result", typeof(string));
                    dtparam.Columns.Add("param_action", typeof(string));
                    dtparam.Columns.Add("param_remarks", typeof(string));
                    dtparam.Columns.Add("sr_no", typeof(string));


                    JArray JPObject = JArray.Parse(_QualityInspectionModel.QCItemParamdetails);
                    for (int i = 0; i < JPObject.Count; i++)
                    {
                        DataRow dtparamrow = dtparam.NewRow();

                        dtparamrow["comp_id"] = Session["CompId"].ToString();
                        dtparamrow["br_id"] = Session["BranchId"].ToString();
                        dtparamrow["qc_no"] = _QualityInspectionModel.qc_no;
                        dtparamrow["qc_dt"] = _QualityInspectionModel.qc_dt;
                        dtparamrow["qc_type"] = _QualityInspectionModel.qc_type;
                        dtparamrow["item_id"] = JPObject[i]["ItemID"].ToString();
                        if (JPObject[i]["ParameterType"].ToString() == "O")
                        {
                            dtparamrow["uom_id"] = 0;
                        }
                        else if (JPObject[i]["ParameterType"].ToString() == "L")
                        {
                            dtparamrow["uom_id"] = 0;
                        }
                        else
                        {
                            dtparamrow["uom_id"] = Convert.ToInt32(JPObject[i]["UOMID"]);
                        }
                        dtparamrow["sam_size"] = JPObject[i]["SamSize"].ToString();
                        dtparamrow["param_Id"] = JPObject[i]["ParameterName"].ToString();
                        dtparamrow["param_uom_Id"] = JPObject[i]["param_uom_Id"].ToString();
                        dtparamrow["upper_val"] = JPObject[i]["UpperRange"].ToString();
                        dtparamrow["lower_val"] = JPObject[i]["LowerRange"].ToString();
                        if (JPObject[i]["ParameterType"].ToString() == "N")
                        {
                            dtparamrow["param_result"] = JPObject[i]["Result"].ToString();
                        }
                        else if (JPObject[i]["ParameterType"].ToString() == "O")
                        {
                            dtparamrow["param_result"] = JPObject[i]["Result"].ToString();
                        }
                        else
                        {
                            dtparamrow["param_result"] = JPObject[i]["ToggleResult"].ToString();
                        }
                        dtparamrow["param_action"] = JPObject[i]["Action"].ToString();
                        dtparamrow["param_remarks"] = JPObject[i]["Remarks"].ToString();
                        dtparamrow["sr_no"] = JPObject[i]["SRNumber"].ToString();

                        dtparam.Rows.Add(dtparamrow);
                    }
                    ViewData["ParamItemDetails"] = dtparamitemdetail(JPObject);
                    QualityInspectionItemParamDetail = dtparam;
                    DataTable dtAttachment = new DataTable();
                    var _QualityInspectionModelAttch = TempData["ModelDataattch"] as QualityInspectionModelAttch;
                    TempData["ModelDataattch"] = null;
                    if (_QualityInspectionModel.attatchmentdetail != null)
                    {
                        if (_QualityInspectionModelAttch != null)
                        {
                            if (_QualityInspectionModelAttch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _QualityInspectionModelAttch.AttachMentDetailItmStp as DataTable;
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
                            if (_QualityInspectionModel.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _QualityInspectionModel.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(_QualityInspectionModel.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(_QualityInspectionModel.qc_no))
                                {
                                    dtrowAttachment1["id"] = _QualityInspectionModel.qc_no;
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
                        if (_QualityInspectionModel.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_QualityInspectionModel.qc_no))
                                {
                                    ItmCode = _QualityInspectionModel.qc_no;
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
                        Attachments = dtAttachment;
                    }
                    DataTable QCLotBatchDetails = GetQCLotBatchDetailsTable(_QualityInspectionModel);
                    /*------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("recd_qty", typeof(string));
                    dtSubItem.Columns.Add("accept_qty", typeof(string));
                    dtSubItem.Columns.Add("reject_qty", typeof(string));
                    dtSubItem.Columns.Add("rework_qty", typeof(string));
                    dtSubItem.Columns.Add("short_qty", typeof(string));
                    dtSubItem.Columns.Add("sample_qty", typeof(string));
                    dtSubItem.Columns.Add("prod_qty", typeof(string));
                    dtSubItem.Columns.Add("pend_qty", typeof(string));

                        if (_QualityInspectionModel.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_QualityInspectionModel.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["recd_qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["accept_qty"] = jObject2[i]["accept_qty"].ToString();
                            dtrowItemdetails["reject_qty"] = jObject2[i]["reject_qty"].ToString();
                            dtrowItemdetails["rework_qty"] = jObject2[i]["rework_qty"].ToString();
                            dtrowItemdetails["short_qty"] = jObject2[i]["short_qty"].ToString();
                            dtrowItemdetails["sample_qty"] = jObject2[i]["sample_qty"].ToString();
                            if (_QualityInspectionModel.qc_type == "PRD")
                            {
                                dtrowItemdetails["prod_qty"] = jObject2[i]["ProdQty"].ToString();
                                dtrowItemdetails["pend_qty"] = jObject2[i]["PendQty"].ToString();
                            }
                            else
                            {
                                dtrowItemdetails["prod_qty"] = "0";
                                dtrowItemdetails["pend_qty"] = "0";
                            }
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubitemDetails"] = dtsubitemdetail(jObject2);
                    }
                    /*------------------Sub Item end----------------------*/
                    SaveMessage = _QualityInspection_ISERVICES.InsertQualityInspectionDetail(QualityInspectionHeader, QualityInspectionItemDetail, QualityInspectionItemParamDetail, Attachments, QCLotBatchDetails, dtSubItem);
                    if (SaveMessage == "DocModify")
                    {
                        _QualityInspectionModel.Message = "DocModify";
                        _QualityInspectionModel.BtnName = "Refresh";
                        _QualityInspectionModel.Command = "Refresh";
                        _QualityInspectionModel.DocumentMenuId = _QualityInspectionModel.DocumentMenuId;
                        TempData["ModelData"] = _QualityInspectionModel;
                        return RedirectToAction("DeliveryNoteDetail");
                    }
                    else
                    {
                        string QualityInspectionNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        string Message = SaveMessage.Substring(0, SaveMessage.IndexOf("-"));
                        if (Message == "DataNotFound")
                        {
                            var a = QualityInspectionNo.Split('-');
                            var tblName = a[0];
                            var msg = "Data Not found" + " " + tblName + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _QualityInspectionModel.Message = Message;
                            return RedirectToAction("QualityInspectionDetail");
                        }

                        if (Message == "Save")
                        {
                            string Guid = "";
                            if (_QualityInspectionModelAttch != null)
                            {
                                if (_QualityInspectionModelAttch.Guid != null)
                                {
                                    Guid = _QualityInspectionModelAttch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, QualityInspectionNo, _QualityInspectionModel.TransType, Attachments);

                            //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //if (Directory.Exists(sourcePath))
                            //{
                            //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
                            //    foreach (string file in filePaths)
                            //    {
                            //        string[] items = file.Split('\\');
                            //        string ItemName = items[items.Length - 1];
                            //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                            //        foreach (DataRow dr in Attachments.Rows)
                            //        {
                            //            string DrItmNm = dr["file_name"].ToString();
                            //            if (ItemName == DrItmNm)
                            //            {
                            //                string QualityInspectionNo1 = QualityInspectionNo.Replace("/", "");
                            //                string img_nm = CompID + BrchID + QualityInspectionNo1 + "_" + Path.GetFileName(DrItmNm).ToString();
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

                        if (Message == "Update" || Message == "Save")
                        {
                            //Session["Message"] = "Save";
                            _QualityInspectionModel.Message = "Save";
                        }
                        _QualityInspectionModel.Command = "Update";
                        _QualityInspectionModel.qc_no = QualityInspectionNo;
                        _QualityInspectionModel.TransType = "Update";
                        _QualityInspectionModel.AppStatus = "D";
                        _QualityInspectionModel.BtnName = "BtnSave";
                        _QualityInspectionModel.AttachMentDetailItmStp = null;
                        _QualityInspectionModel.Guid = null;
                        //Session["Command"] = "Update";
                        //Session["QualityInspection"] = QualityInspectionNo;
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnSave";
                        //Session["AttachMentDetailItmStp"] = null;
                        //Session["Guid"] = null;
                        return RedirectToAction("QualityInspectionDetail");
                    }
                }
                else
                {
                    if (CheckGRNAgainstQC(_QualityInspectionModel.src_doc_no, Convert.ToDateTime(_QualityInspectionModel.src_doc_date.ToString()).ToString("yyyy-MM-dd")) != "Used")
                    {
                        string br_id = Session["BranchId"].ToString();
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        var loc_type = _QualityInspectionModel.Location_type;
                        SaveMessage = _QualityInspection_ISERVICES.QCInspectionCancel(_QualityInspectionModel, CompID, userid, br_id, mac_id);

                        string QualityInspectionNo = SaveMessage.Substring(0, SaveMessage.IndexOf('-'));
                        string message = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);

                        if (message == "StockNotAvailable")
                        {
                            //Session["Message"] = "StockNotAvailableForCancel";
                            _QualityInspectionModel.Message = "StockNotAvailableForCancel";
                        }
                        else if (message == "SR Cancelled")
                        {
                            //Session["Message"] = "SR Cancelled";
                            _QualityInspectionModel.Message = "SR Cancelled";
                        }
                        else
                        {
                            try
                            {
                                // string fileName = "QI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                string fileName = "QualityInspection_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                var filePath = SavePdfDocToSendOnEmailAlert(_QualityInspectionModel.qc_no, _QualityInspectionModel.qc_dt, _QualityInspectionModel.qc_type, fileName, DocumentMenuId, "C");
                                _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _QualityInspectionModel.qc_no, "C", userid, "", filePath);
                            }
                            catch (Exception exMail)
                            {
                                _QualityInspectionModel.Message = "ErrorInMail";
                                string path = Server.MapPath("~");
                                Errorlog.LogError(path, exMail);
                            }
                            //Session["Message"] = "Cancelled";
                            //_QualityInspectionModel.Message = "Cancelled";
                            _QualityInspectionModel.Message = _QualityInspectionModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        }
                        // _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _QualityInspectionModel.qc_no, "C", userid, "");
                        _QualityInspectionModel.Command = "Update";
                        _QualityInspectionModel.qc_no = QualityInspectionNo;
                        _QualityInspectionModel.TransType = "Update";
                        _QualityInspectionModel.AppStatus = "D";
                        _QualityInspectionModel.BtnName = "Refresh";
                    }
                    else
                    {
                        _QualityInspectionModel.Message = "Used";
                        _QualityInspectionModel.BtnName = "BtnToDetailPage";
                    }
                    return RedirectToAction("QualityInspectionDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_QualityInspectionModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_QualityInspectionModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _QualityInspectionModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public DataTable dtitemdetail(JArray jObject)
        {

            DataTable dtItem = new DataTable();
            
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string)); 
            dtItem.Columns.Add("recd_qty", typeof(string));
            dtItem.Columns.Add("accept_qty", typeof(string));
            dtItem.Columns.Add("reject_qty", typeof(string));
            dtItem.Columns.Add("rework_qty", typeof(string));
            dtItem.Columns.Add("short_qty", typeof(string));
            dtItem.Columns.Add("sample_qty", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowItemdetails = dtItem.NewRow();
                decimal Recd_qty, Accept_qty, Reject_qty, Rework_qty,Short_qty,Sample_qty;

                if (jObject[i]["RecivedQty"].ToString() == "")
                    Recd_qty = 0;
                else
                    Recd_qty = Convert.ToDecimal(jObject[i]["RecivedQty"].ToString());

                if (jObject[i]["AcceptQty"].ToString() == "")
                    Accept_qty = 0;
                else
                    Accept_qty = Convert.ToDecimal(jObject[i]["AcceptQty"].ToString());

                if (jObject[i]["RejectQty"].ToString() == "")
                    Reject_qty = 0;
                else
                    Reject_qty = Convert.ToDecimal(jObject[i]["RejectQty"].ToString());

                if (jObject[i]["ReworkableQty"].ToString() == "")
                    Rework_qty = 0;
                else
                    Rework_qty = Convert.ToDecimal(jObject[i]["ReworkableQty"].ToString());

                if (jObject[i]["ShortQty"].ToString() == "")
                    Short_qty = 0;
                else
                    Short_qty = Convert.ToDecimal(jObject[i]["ShortQty"].ToString());

                if (jObject[i]["SampleQty"].ToString() == "")
                    Sample_qty = 0;
                else
                    Sample_qty = Convert.ToDecimal(jObject[i]["SampleQty"].ToString());

                dtrowItemdetails["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowItemdetails["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowItemdetails["uom_id"] = Convert.ToInt32(jObject[i]["UOMID"]);
                dtrowItemdetails["uom_name"] = jObject[i]["UOMName"].ToString();
                dtrowItemdetails["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowItemdetails["recd_qty"] = Recd_qty;
                dtrowItemdetails["accept_qty"] = Accept_qty;
                dtrowItemdetails["reject_qty"] = Reject_qty;
                dtrowItemdetails["rework_qty"] = Rework_qty;
                dtrowItemdetails["short_qty"] = Short_qty;
                dtrowItemdetails["sample_qty"] = Sample_qty;
                dtrowItemdetails["it_remarks"] = jObject[i]["Remarks"].ToString();
                dtItem.Rows.Add(dtrowItemdetails);
            }




            return dtItem;
        }
        public DataTable dtparamitemdetail(JArray JPObject)
        {

            DataTable dtparam = new DataTable();

            
            dtparam.Columns.Add("item_id", typeof(string));
            dtparam.Columns.Add("item_name", typeof(string));
            dtparam.Columns.Add("uom_id", typeof(int));
            dtparam.Columns.Add("uom_name", typeof(string));
            dtparam.Columns.Add("uom_alias", typeof(string));
            dtparam.Columns.Add("sam_size", typeof(string));
            dtparam.Columns.Add("param_Id", typeof(int));
            dtparam.Columns.Add("param_name", typeof(string));
            dtparam.Columns.Add("paramtype", typeof(string));
            dtparam.Columns.Add("param_type", typeof(string));
            dtparam.Columns.Add("param_uom_Id", typeof(int));/*Added by Suraj on 16-08-2023*/
            dtparam.Columns.Add("upper_val", typeof(string));
            dtparam.Columns.Add("lower_val", typeof(string));
            dtparam.Columns.Add("Result", typeof(string));
            dtparam.Columns.Add("param_action", typeof(string));
            dtparam.Columns.Add("param_remarks", typeof(string));
            dtparam.Columns.Add("sr_no", typeof(string));

            for (int i = 0; i < JPObject.Count; i++)
            {
                DataRow dtparamrow = dtparam.NewRow();

                dtparamrow["item_id"] = JPObject[i]["ItemID"].ToString();
                dtparamrow["item_name"] = JPObject[i]["ItemName"].ToString();
                //dtparamrow["uom_id"] = Convert.ToInt32(JPObject[i]["UOMID"]);
                if (JPObject[i]["ParameterType"].ToString() == "O")
                {
                    dtparamrow["uom_id"] = 0;
                }
                else if (JPObject[i]["ParameterType"].ToString() == "L")
                {
                    dtparamrow["uom_id"] = 0;
                }
                else
                {
                    dtparamrow["uom_id"] = Convert.ToInt32(JPObject[i]["UOMID"]);
                }
                dtparamrow["uom_name"] = JPObject[i]["UOMName"].ToString();
                dtparamrow["sam_size"] = JPObject[i]["SamSize"].ToString();
                dtparamrow["param_Id"] = JPObject[i]["ParameterName"].ToString();
                dtparamrow["param_name"] = JPObject[i]["Parameter_Name"].ToString();
                dtparamrow["param_uom_Id"] = JPObject[i]["param_uom_Id"].ToString();
                dtparamrow["paramtype"] = JPObject[i]["ParameterType"].ToString();
                dtparamrow["param_type"] = JPObject[i]["ParameterType"].ToString();
                dtparamrow["upper_val"] = JPObject[i]["UpperRange"].ToString();
                dtparamrow["lower_val"] = JPObject[i]["LowerRange"].ToString();
                if (JPObject[i]["ParameterType"].ToString() == "O")
                {
                    dtparamrow["uom_id"] = 0;
                }
                else if (JPObject[i]["ParameterType"].ToString() == "L")
                {
                    dtparamrow["uom_id"] = 0;
                }
                else
                {
                    dtparamrow["uom_id"] = Convert.ToInt32(JPObject[i]["UOMID"]);
                }
                dtparamrow["param_action"] = JPObject[i]["Action"].ToString();
                dtparamrow["param_remarks"] = JPObject[i]["Remarks"].ToString();
                dtparamrow["sr_no"] = JPObject[i]["SRNumber"].ToString();

                dtparam.Rows.Add(dtparamrow);
            }



            return dtparam;
        }
        public DataTable dtlotbatchdetail(JArray JPObject)
        {

            DataTable dtparam = new DataTable();

            
            dtparam.Columns.Add("item_id", typeof(string));
            dtparam.Columns.Add("uom_id", typeof(int));
            dtparam.Columns.Add("qc_qty", typeof(string));
            dtparam.Columns.Add("lot_id", typeof(string));
            dtparam.Columns.Add("batch_no", typeof(string));
            dtparam.Columns.Add("exp_dt", typeof(string));
            dtparam.Columns.Add("expiry_date", typeof(string));
            dtparam.Columns.Add("accept_qty", typeof(string));
            dtparam.Columns.Add("reject_qty", typeof(string));
            dtparam.Columns.Add("rework_qty", typeof(string));
            dtparam.Columns.Add("short_qty", typeof(string));
            dtparam.Columns.Add("sample_qty", typeof(string));
            dtparam.Columns.Add("serial_no", typeof(string));

            for (int i = 0; i < JPObject.Count; i++)
            {
                DataRow dtparamrow = dtparam.NewRow();

                
                dtparamrow["item_id"] = JPObject[i]["ItmCode"].ToString();
                dtparamrow["uom_id"] = Convert.ToInt32(JPObject[i]["ItemUOMID"]);
                dtparamrow["qc_qty"] = JPObject[i]["QCQuantity"].ToString();
                dtparamrow["lot_id"] = JPObject[i]["LotNo"].ToString();
                dtparamrow["batch_no"] = JPObject[i]["ItemBatchNo"].ToString().Trim();
                if (JPObject[i]["ItemExpiryDate"].ToString() == "" || JPObject[i]["ItemExpiryDate"].ToString() == null)
                {
                    dtparamrow["exp_dt"] = "1900-01-01";
                }
                else
                {
                    dtparamrow["exp_dt"] = JPObject[i]["ItemExpiryDate"].ToString();
                }
                dtparamrow["expiry_date"] = JPObject[i]["ItemExpiryDate"].ToString();
                dtparamrow["accept_qty"] = JPObject[i]["LBAccQty"].ToString();
                dtparamrow["reject_qty"] = JPObject[i]["LBRejQty"].ToString();
                dtparamrow["rework_qty"] = JPObject[i]["LBRewQty"].ToString();
                dtparamrow["short_qty"] = JPObject[i]["LBShortQty"].ToString();
                dtparamrow["sample_qty"] = JPObject[i]["LBSampleQty"].ToString();
                dtparamrow["serial_no"] = JPObject[i]["ItemSerialNo"].ToString().Trim();

                dtparam.Rows.Add(dtparamrow);
            }

         return dtparam;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {

            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("prod_qty", typeof(string));
            dtSubItem.Columns.Add("accept_qty", typeof(string));
            dtSubItem.Columns.Add("reject_qty", typeof(string));
            dtSubItem.Columns.Add("rework_qty", typeof(string));
            dtSubItem.Columns.Add("short_qty", typeof(string));
            dtSubItem.Columns.Add("sample_qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["prod_qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["accept_qty"] = jObject2[i]["accept_qty"].ToString();
                dtrowItemdetails["reject_qty"] = jObject2[i]["reject_qty"].ToString();
                dtrowItemdetails["rework_qty"] = jObject2[i]["rework_qty"].ToString();
                dtrowItemdetails["short_qty"] = jObject2[i]["short_qty"].ToString();
                dtrowItemdetails["sample_qty"] = jObject2[i]["sample_qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                QualityInspectionModelAttch _QualityInspectionModelAttch = new QualityInspectionModelAttch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string QualityInspection = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["QualityInspection"] != null)
                //{
                //    QualityInspection = Session["QualityInspection"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _QualityInspectionModelAttch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _QualityInspectionModelAttch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _QualityInspectionModelAttch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _QualityInspectionModelAttch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        public DataTable GetQCLotBatchDetailsTable(QualityInspectionModel _QualityInspectionModel)
        {
            try
            {
                var commonCont = new CommonController(_Common_IServices);
                DataTable dtparam = new DataTable();

                dtparam.Columns.Add("comp_id", typeof(int));
                dtparam.Columns.Add("br_id", typeof(int));
                dtparam.Columns.Add("qc_no", typeof(string));
                dtparam.Columns.Add("qc_dt", typeof(string));
                dtparam.Columns.Add("item_id", typeof(string));
                dtparam.Columns.Add("uom_id", typeof(int));
                dtparam.Columns.Add("qc_qty", typeof(string));
                dtparam.Columns.Add("lot_id", typeof(string));
                dtparam.Columns.Add("batch_no", typeof(string));
                dtparam.Columns.Add("exp_dt", typeof(string));
                dtparam.Columns.Add("AccQty", typeof(string));
                dtparam.Columns.Add("RejQty", typeof(string));
                dtparam.Columns.Add("RewQty", typeof(string));
                dtparam.Columns.Add("ShortQty", typeof(string));
                dtparam.Columns.Add("SampleQty", typeof(string));
                dtparam.Columns.Add("serial_no", typeof(string));
                dtparam.Columns.Add("mfg_name", typeof(string));
                dtparam.Columns.Add("mfg_mrp", typeof(string));
                dtparam.Columns.Add("mfg_date", typeof(string));

                if (_QualityInspectionModel.QCLotBatchdetails != null)
                {
                    JArray JPObject = JArray.Parse(_QualityInspectionModel.QCLotBatchdetails);
                    for (int i = 0; i < JPObject.Count; i++)
                    {
                        DataRow dtparamrow = dtparam.NewRow();

                        dtparamrow["comp_id"] = Session["CompId"].ToString();
                        dtparamrow["br_id"] = Session["BranchId"].ToString();
                        dtparamrow["qc_no"] = _QualityInspectionModel.qc_no;
                        dtparamrow["qc_dt"] = _QualityInspectionModel.qc_dt;
                        dtparamrow["item_id"] = JPObject[i]["ItmCode"].ToString();
                        dtparamrow["uom_id"] = Convert.ToInt32(JPObject[i]["ItemUOMID"]);
                        dtparamrow["qc_qty"] = JPObject[i]["QCQuantity"].ToString();
                        dtparamrow["lot_id"] = JPObject[i]["LotNo"].ToString();
                        dtparamrow["batch_no"] = JPObject[i]["ItemBatchNo"].ToString().Trim();
                        if (JPObject[i]["ItemExpiryDate"].ToString() == "" || JPObject[i]["ItemExpiryDate"].ToString() == null)
                        {
                            dtparamrow["exp_dt"] = "1900-01-01";
                        }
                        else
                        {
                            dtparamrow["exp_dt"] = JPObject[i]["ItemExpiryDate"].ToString();
                        }
                        
                        dtparamrow["AccQty"] = JPObject[i]["LBAccQty"].ToString();
                        dtparamrow["RejQty"] = JPObject[i]["LBRejQty"].ToString();
                        dtparamrow["RewQty"] = JPObject[i]["LBRewQty"].ToString();
                        dtparamrow["ShortQty"] = JPObject[i]["LBShortQty"].ToString();
                        dtparamrow["SampleQty"] = JPObject[i]["LBSampleQty"].ToString();
                        dtparamrow["serial_no"] = JPObject[i]["ItemSerialNo"].ToString().Trim();
                        dtparamrow["mfg_name"] = commonCont.IsBlank(JPObject[i]["mfg_name"].ToString(), null);
                        dtparamrow["mfg_mrp"] = commonCont.IsBlank(JPObject[i]["mfg_mrp"].ToString(), null);
                        dtparamrow["mfg_date"] = commonCont.IsBlank(JPObject[i]["mfg_date"].ToString(), null);

                        dtparam.Rows.Add(dtparamrow);
                    }
                    ViewData["LotBatchDetails"] = dtlotbatchdetail(JPObject);
                }

                return dtparam;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetQCSourceDocumentNoList(QualityInspectionModel _QualityInspectionModel,string suppid)
        {
            try
            {
                string DocumentNumber = string.Empty;
                DataSet DocumentNumberList = new DataSet();
                string Spp_ID = string.Empty;
                string Src_type = string.Empty;
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                Src_type = _QualityInspectionModel.Src_type;
                DocumentNumber = _QualityInspectionModel.DocumentNo;
                var itemId = "";
                if (Src_type == "PRD")
                {
                    itemId = _QualityInspectionModel.itemId;
                }
             //   var supp_id = _QualityInspectionModel.suppid;
                string BrchID = Session["BranchId"].ToString();
                DocumentNumberList = _QualityInspection_ISERVICES.getQCInsSourceDocumentNo(CompID, BrchID, DocumentNumber, Src_type, itemId, suppid);
                if (DocumentNumberList.Tables.Count > 0)
                {
                    if (Src_type == "PUR")
                    {
                        DataRow Drow = DocumentNumberList.Tables[0].NewRow();
                        Drow[0] = "---Select---"; //+','+"---Select---";
                        Drow[1] = "---Select---";
                        Drow[2] = "---Select---";
                       
                        DocumentNumberList.Tables[0].Rows.InsertAt(Drow, 0);

                        foreach (DataRow dr in DocumentNumberList.Tables[0].Rows)
                        {
                            DocumentNumber _DocumentNumber = new DocumentNumber();
                            _DocumentNumber.doc_no = dr["dn_no"].ToString();
                            _DocumentNumber.doc_dt = dr["dn_dt"].ToString();
                            _DocumentNumber.supp_name = dr["supp_name"].ToString();
                           
                            _DocumentNumberList.Add(_DocumentNumber);
                        }

                        return Json(_DocumentNumberList.Select(c => new {
                            Name = c.doc_no,
                            ID = c.doc_dt, // or use a structured model instead
                            Supp_Name = c.supp_name
                        }).ToList(), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                         DataRow Drow = DocumentNumberList.Tables[0].NewRow();
                        Drow[0] = "---Select---";
                        Drow[1] = "0";
                        DocumentNumberList.Tables[0].Rows.InsertAt(Drow, 0);

                        foreach (DataRow dr in DocumentNumberList.Tables[0].Rows)
                        {
                            DocumentNumber _DocumentNumber = new DocumentNumber();
                            _DocumentNumber.doc_no = dr["dn_no"].ToString();
                            _DocumentNumber.doc_dt = dr["dn_dt"].ToString();
                            _DocumentNumberList.Add(_DocumentNumber);
                        }

                        return Json(_DocumentNumberList.Select(c => new { Name = c.doc_no, ID = c.doc_dt, }).ToList(), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(_DocumentNumberList.Select(c => new { Name = c.doc_no, ID = c.doc_dt, }).ToList(), JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public ActionResult GetItemDetailBySourceDocumentNo(string SourDocumentNo, string Src_Type)
        {
            try
            {
                JsonResult DataRows = null;
                _QualityInspectionModel = new QualityInspectionModel();
                List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _QualityInspection_ISERVICES.GetItemDetailBySourceDocumentNo(CompID, BrchID, SourDocumentNo, Src_Type);

                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult GetAllItemQCParamDetail(string ItemID,string qc_no,string qc_dt,string TransType,string Command,string DocumentStatus)
        {
            try
            {
                JsonResult DataRows = null;
                _QualityInspectionModel = new QualityInspectionModel();
                if (TransType != null)
                {
                    _QualityInspectionModel.TransType = TransType;
                }
                if (Command != null)
                {
                    _QualityInspectionModel.Command = Command;
                }
                if (DocumentStatus != null)
                {
                    _QualityInspectionModel.DocumentStatus = DocumentStatus;
                }
                List<ItemDetailsQc> _ItemDetailsList = new List<ItemDetailsQc>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds = _QualityInspection_ISERVICES.GetItemQCParamDetail(CompID, BrchID, ItemID,qc_no,qc_dt, DocumentStatus);
                //ViewBag.ItemDetailsQc = ds.Tables[0];
                //return PartialView("~/Areas/ApplicationLayer/Views/Shared/_QCParameterEvalutionDetail.cshtml", _QualityInspectionModel);
                DataRows = Json(JsonConvert.SerializeObject(ds));
                return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public ActionResult GetItemQCParamDetail(string ItemID,string qc_no,string qc_dt,string TransType,string Command,string DocumentStatus)
        {
            try
            {
                //JsonResult DataRows = null;
                _QualityInspectionModel = new QualityInspectionModel();
                if (TransType != null)
                {
                    _QualityInspectionModel.TransType = TransType;
                }
                if (Command != null)
                {
                    _QualityInspectionModel.Command = Command;
                }
                if (DocumentStatus != null)
                {
                    _QualityInspectionModel.DocumentStatus = DocumentStatus;
                }
                List<ItemDetailsQc> _ItemDetailsList = new List<ItemDetailsQc>();
                List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds = _QualityInspection_ISERVICES.GetItemQCParamDetail(CompID, BrchID, ItemID,qc_no,qc_dt, DocumentStatus);
                ViewBag.ItemDetailsQc = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_QCParameterEvalutionDetail.cshtml", _QualityInspectionModel);
                //DataRows = Json(JsonConvert.SerializeObject(ds));
                //return DataRows;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        public void GetStatusList(QCInspectionList_Model _QCInspectionList_Model)
        {
            try
            {
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105102120")
                //    {
                //        DocumentMenuId = "105102120";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105105135")
                //    {
                //        DocumentMenuId = "105105135";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105108115")
                //    {
                //        DocumentMenuId = "105108115";
                //    }
                //}
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _QCInspectionList_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private List<QCInspectionList> GetQCDetailList(QCInspectionList_Model _QCInspectionList_Model,string wfstatus,string Item_id)
        {
            try
            {
                _QCInspectionList = new List<QCInspectionList>();

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
                    userid = Session["userid"].ToString();
                }
                DataSet dt = new DataSet();
                dt = _QualityInspectionList_ISERVICES.GetQCDetailList(CompID, BrchID, _QCInspectionList_Model.QC_FromDate
                    , _QCInspectionList_Model.QC_ToDate, _QCInspectionList_Model.QC_Type, _QCInspectionList_Model.Status
                    , userid, wfstatus, DocumentMenuId, Item_id);
                if (dt.Tables[1].Rows.Count > 0)
                {
                    FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
                }
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        QCInspectionList _QCList = new QCInspectionList();
                        _QCList.QCNo = dr["QCNo"].ToString();
                        _QCList.QCDate = dr["QCDate"].ToString();
                        _QCList.QC_date = dr["qc_dt"].ToString();
                        _QCList.SourceType = dr["SourceType"].ToString();
                        _QCList.QCType = dr["qc_type"].ToString();
                        _QCList.Location = dr["Location"].ToString();/*Add by Hina on 31-08-2024 to random qc for shopfloor*/
                        _QCList.SourceDocNo = dr["SourceDocNo"].ToString();
                        _QCList.SourceDocDate = dr["SourceDocDate"].ToString();
                        _QCList.Stauts = dr["Status"].ToString();
                        _QCList.CreateDate = dr["CreateDate"].ToString();
                        _QCList.ApproveDate = dr["ApproveDate"].ToString();
                        _QCList.ModifyDate = dr["ModifyDate"].ToString();
                        _QCList.create_by = dr["create_by"].ToString();
                        _QCList.app_by = dr["app_by"].ToString();
                        _QCList.mod_by = dr["mod_by"].ToString();
                        _QCInspectionList.Add(_QCList);
                    }
                }
                return _QCInspectionList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult SearchQCDetail(string Fromdate, string Todate, string QC_Type, string Status,string DocumentMenuId, string ItemID)
        {
            try
            {
                _QCInspectionList = new List<QCInspectionList>();
                QCInspectionList_Model _QC_ListModel = new QCInspectionList_Model();
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
                    userid = Session["userid"].ToString();
                }
                //Session["WF_status"] = null;
                _QC_ListModel.WF_status = null;
                DataSet dt = new DataSet();
                dt = _QualityInspectionList_ISERVICES.GetQCDetailList(CompID, BrchID, Fromdate, Todate, QC_Type, Status, userid, "0", DocumentMenuId, ItemID);
                _QC_ListModel.QISearch = "QI_Search";
                if (dt.Tables[0].Rows.Count > 0)
                {
                    
                    // FromDate = dt.Tables[0].Rows[0]["finstrdate"].ToString();
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        QCInspectionList _QCList = new QCInspectionList();
                        _QCList.QCNo = dr["QCNo"].ToString();
                        _QCList.QCDate = dr["QCDate"].ToString();
                        _QCList.QC_date = dr["qc_dt"].ToString();
                        _QCList.SourceType = dr["SourceType"].ToString();
                        _QCList.QCType = dr["qc_type"].ToString();
                        _QCList.Location = dr["Location"].ToString();/*Add by Hina on 31-08-2024 to random qc for shopfloor*/
                        _QCList.SourceDocNo = dr["SourceDocNo"].ToString();
                        _QCList.SourceDocDate = dr["SourceDocDate"].ToString();
                        _QCList.Stauts = dr["Status"].ToString();
                        _QCList.CreateDate = dr["CreateDate"].ToString();
                        _QCList.ApproveDate = dr["ApproveDate"].ToString();
                        _QCList.ModifyDate = dr["ModifyDate"].ToString();
                        _QCList.create_by = dr["create_by"].ToString();
                        _QCList.app_by = dr["app_by"].ToString();
                        _QCList.mod_by = dr["mod_by"].ToString();
                        _QCInspectionList.Add(_QCList);

                    }
                }
                _QC_ListModel.QCList = _QCInspectionList;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartalQCInspectionList.cshtml", _QC_ListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private ActionResult QCInspectionDelete(QualityInspectionModel _QualityInspectionModel, string command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();

                }
                string br_id = Session["BranchId"].ToString();
                string qc_no = _QualityInspectionModel.qc_no;

                DataSet Message = _QualityInspection_ISERVICES.QCInspectionDelete(_QualityInspectionModel, comp_id, br_id, DocumentMenuId, qc_no);
                if (!string.IsNullOrEmpty(qc_no))
                {
                    //getDocumentName(); /* To set Title*/
                    string PageName = _QualityInspectionModel.Title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string qc_no1 = qc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + br_id, PageName, qc_no1, Server);
                }
                _QualityInspectionModel.Message = "Deleted";
                _QualityInspectionModel.Command = "Refresh";
                _QualityInspectionModel.TransType = command;
                _QualityInspectionModel.AppStatus = "D";
                _QualityInspectionModel.BtnName = "BtnDelete";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["QualityInspection"] = _QualityInspectionModel.qc_no;
                //Session["TransType"] = command;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("QualityInspectionDetail", "QualityInspection");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //  return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private ActionResult QCInspectionApprove(QualityInspectionModel _QualityInspectionModel, string command, string ListFilterData1, string WF_status1, string docid, string qc_type,string Loc_type)
        {
            try
            {

                if (_QualityInspectionModel.MenuDocumentId != null)
                {
                    DocumentMenuId = _QualityInspectionModel.MenuDocumentId;
                }
                else
                {
                    DocumentMenuId = docid;
                }                            
               
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                string br_id = Session["BranchId"].ToString();
                _QualityInspectionModel.CreatedBy = Session["UserId"].ToString();
                var Loctype = "";
                string qctyp = _QualityInspectionModel.qc_type;/*Add by Hina on 31-08-2024 to random qc for shopfloor*/
                if (qctyp == "RQC" && _QualityInspectionModel.WF_status1 == null)
                {
                     Loctype = _QualityInspectionModel.Location_type;
                }
                else
                {
                    Loctype = Loc_type;

                }
                
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                
                DataSet Message = _QualityInspection_ISERVICES.QCInspectionApprove(_QualityInspectionModel, comp_id, br_id, mac_id, DocumentMenuId, Loctype);/*add Loc_type by Hina on 29-08-2024 to random qc for shopfloor*/
                if (Message.Tables[0].Rows.Count > 0)
                {
                    if(_QualityInspectionModel.qc_type=="RQC" && Loctype == "WH")
                    {
                        string[] FDetail = Message.Tables[0].Rows[0]["Result"].ToString().Split(',');
                        string data = FDetail[0].ToString();
                        if (data == "StockNotAvailable")
                        {
                            _QualityInspectionModel.StockItemWiseMessage = string.Join(",", FDetail.Skip(1));
                            _QualityInspectionModel.Message = "StockNotAvailableForApprove";
                        }
                        else
                        {
                            try
                            {
                                //string fileName = "QI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                string fileName = "QualityInspection_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                var filePath = SavePdfDocToSendOnEmailAlert(_QualityInspectionModel.qc_no, _QualityInspectionModel.qc_dt, _QualityInspectionModel.qc_type, fileName, DocumentMenuId, "AP");
                                _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _QualityInspectionModel.qc_no, "AP", userid, "", filePath);
                            }
                            catch (Exception exMail)
                            {
                                _QualityInspectionModel.Message = "ErrorInMail";
                                string path = Server.MapPath("~");
                                Errorlog.LogError(path, exMail);
                            }
                            _QualityInspectionModel.Message = _QualityInspectionModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                        }
                    }
                    else if (_QualityInspectionModel.qc_type == "RQC" && Loctype == "SF")
                    {
                        string[] FDetail = Message.Tables[0].Rows[0]["Result"].ToString().Split(',');
                        string data = FDetail[0].ToString();
                        if (data == "StockNotAvailable")
                        {
                            _QualityInspectionModel.StockItemWiseMessage = string.Join(",", FDetail.Skip(1));
                            _QualityInspectionModel.Message = "StockNotAvailableForApprove";
                        }
                        else
                        {
                            try
                            {
                                //string fileName = "QI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                string fileName = "QualityInspection_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                var filePath = SavePdfDocToSendOnEmailAlert(_QualityInspectionModel.qc_no, _QualityInspectionModel.qc_dt, _QualityInspectionModel.qc_type, fileName, DocumentMenuId, "AP");
                                _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _QualityInspectionModel.qc_no, "AP", userid, "", filePath);
                            }
                            catch (Exception exMail)
                            {
                                _QualityInspectionModel.Message = "ErrorInMail";
                                string path = Server.MapPath("~");
                                Errorlog.LogError(path, exMail);
                            }
                            _QualityInspectionModel.Message = _QualityInspectionModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                        }
                    }
                    else if (Message.Tables[0].Rows[0]["Result"].ToString() == "StockNotAvailable")
                    {
                        _QualityInspectionModel.Message = "StockNotAvailableForApprove";
                    }
                    else
                    {
                        //Added by Nidhi on 09-07-2025
                        try
                        {
                            //string fileName = "QI_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "QualityInspection_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(_QualityInspectionModel.qc_no, _QualityInspectionModel.qc_dt, _QualityInspectionModel.qc_type, fileName, DocumentMenuId, "AP");
                            _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _QualityInspectionModel.qc_no, "AP", userid, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _QualityInspectionModel.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        _QualityInspectionModel.Message = _QualityInspectionModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                        // _Common_IServices.SendAlertEmail(comp_id, br_id, DocumentMenuId, _QualityInspectionModel.qc_no, "AP", userid, "");
                        //_QualityInspectionModel.Message = "Approved";
                    }
                }
                _QualityInspectionModel.TransType = "Update";
                _QualityInspectionModel.Command = command;
                //_QualityInspectionModel.QualityInspection = _QualityInspectionModel.qc_no;
                _QualityInspectionModel.AppStatus = "D";
                _QualityInspectionModel.BtnName = "BtnApprove";
                _QualityInspectionModel.WF_status1 = WF_status1;
                TempData["WF_status1"] = WF_status1;
                _QualityInspectionModel.DocumentMenuId = DocumentMenuId;
                _QualityInspectionModel.qc_type = qc_type;
                TempData["ModelData"] = _QualityInspectionModel;
                URLDetailModel URLModelApprove = new URLDetailModel();
                URLModelApprove.DocumentMenuId = DocumentMenuId;
                URLModelApprove.QC_Type = _QualityInspectionModel.qc_type;
                URLModelApprove.Command = "New";
                URLModelApprove.BtnName = _QualityInspectionModel.BtnName;
                URLModelApprove.TransType = "Update";
               TempData["WF_status1"] = _QualityInspectionModel.WF_status1;
                //TempData["ModelData"] = _QualityInspectionModel;
                //Session["TransType"] = "Update";
                //Session["Command"] = command;
                //Session["QualityInspection"] = _QualityInspectionModel.qc_no;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnApprove";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("QualityInspectionDetail", "QualityInspection", URLModelApprove);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }

        //[HttpPost]
        //public JsonResult CheckGRNAgainstQC(string DocNo, string DocDate)
        //{
        //    JsonResult DataRows = null;
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
        //        DataSet Deatils = _QualityInspection_ISERVICES.CheckGRNAgainstQC(Comp_ID, Br_ID, DocNo, DocDate);
        //        DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return Json("ErrorPage");
        //    }
        //    return DataRows;
        //}
        public string CheckGRNAgainstQC(string DocNo, string DocDate)
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
                DataSet Deatils = _QualityInspection_ISERVICES.CheckGRNAgainstQC(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0|| Deatils.Tables[1].Rows.Count > 0)
                {
                    str = "Used";
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
        [NonAction]
        private DataTable GetRejectWHList()
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataTable dt = _QualityInspection_ISERVICES.GetRejectWHList(CompID, BrchID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        //[NonAction]
        //private DataTable GetSourceAndAcceptWHList()
        //{
        //    try
        //    {
        //        string CompID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["BranchId"] != null)
        //        {
        //            BrchID = Session["BranchId"].ToString();
        //        }

        //        DataTable dt = _QualityInspection_ISERVICES.GetSourceAndAcceptWHList(CompID, BrchID);
        //        return dt;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
        [NonAction]
        private DataSet GetSourceAndAcceptRejctRewrkWHAndShopfloorList()
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataSet ds = _QualityInspection_ISERVICES.GetSourceAndAcceptWHList(CompID, BrchID);
                return ds;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetReworkWHList()
        {
            try
            {
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }

                DataTable dt = _QualityInspection_ISERVICES.GetReworkWHList(CompID, BrchID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }

        public ActionResult getItemStockBatchWise(string ItemId, string Status, string WarehouseId, string SelectedItemdetail,string TransType,string Command,string DocumentStatus,string DocumentMenuId,string Src_Type)
        {
            try
            {
                QualityInspectionModel _QualityInspectionModel = new QualityInspectionModel();
                if (TransType != null)
                {
                    _QualityInspectionModel.TransType = TransType;
                }
                if (Command != null)
                {
                    _QualityInspectionModel.Command = Command;
                }
                if (DocumentStatus != null)
                {
                    _QualityInspectionModel.DocumentStatus = DocumentStatus;
                }
                if (DocumentMenuId != null)
                {
                    _QualityInspectionModel.DocumentMenuId = DocumentMenuId;
                }
                var QtyDigit = "0.";
                var VD = Convert.ToInt32(Session["QtyDigit"].ToString());
                for (var i = 0; i < VD; i++)
                {
                    QtyDigit += "0";
                }
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //if (Session["MenuDocumentId"] != null)
                //{
                //    if (Session["MenuDocumentId"].ToString() == "105102120")
                //    {
                //        DocumentMenuId = "105102120";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105105135")
                //    {
                //        DocumentMenuId = "105105135";
                //    }
                //    if (Session["MenuDocumentId"].ToString() == "105108115")
                //    {
                //        DocumentMenuId = "105108115";
                //    }
                //}
                if (Status != "A" && Status != "C")
                    ds = _Common_IServices.getItemStockBatchWise(ItemId, WarehouseId, CompID, BrchID);

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {

                    if (Status != "A" && Status != "C")
                    {
                        ds.Tables[0].Columns.Add("AccQty", typeof(string));
                        ds.Tables[0].Columns.Add("RejQty", typeof(string));
                        ds.Tables[0].Columns.Add("RewQty", typeof(string));
                        ds.Tables[0].Columns.Add("ShortQty", typeof(string));
                        ds.Tables[0].Columns.Add("SampleQty", typeof(string));
                    }
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    DataTable dt = new DataTable();
                    if (Status == "A" || Status == "C")
                    {


                        //QualityInspectionModel _QualityInspectionModel = new QualityInspectionModel();
                        string qc_no = jObjectBatch[0]["qc_no"].ToString();
                        string qc_dt = jObjectBatch[0]["qc_dt"].ToString();
                        string item_id = jObjectBatch[0]["ItmCode"].ToString();
                        _QualityInspectionModel.qc_no = qc_no;
                        _QualityInspectionModel.qc_dt = Convert.ToDateTime(qc_dt);
                        _QualityInspectionModel.item_id = ItemId;
                        _QualityInspectionModel.SourceWH = Convert.ToInt32(WarehouseId);
                        ds = _QualityInspection_ISERVICES.AfterApproveItemStockDetailBatchLotWise(_QualityInspectionModel, CompID, BrchID);
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            string checknull = "Y";
                            foreach (JObject item in jObjectBatch.Children())
                            {
                                //if (item.GetValue("ItmCode").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("ItemBatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim())
                                if (item.GetValue("ItmCode").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("ItemBatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim() && item.GetValue("ItemSerialNo").ToString().Trim() == ds.Tables[0].Rows[i]["serial_no"].ToString().Trim())
                                {
                                    if (item.GetValue("LBAccQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBAccQty")) != 0)
                                        ds.Tables[0].Rows[i]["AccQty"] = Convert.ToDecimal(item.GetValue("LBAccQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["AccQty"] = "";
                                    if (item.GetValue("LBRejQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBRejQty")) != 0)
                                        ds.Tables[0].Rows[i]["RejQty"] = Convert.ToDecimal(item.GetValue("LBRejQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["RejQty"] = "";
                                    if (item.GetValue("LBRewQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBRewQty")) != 0)
                                        ds.Tables[0].Rows[i]["RewQty"] = Convert.ToDecimal(item.GetValue("LBRewQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["RewQty"] = "";
                                    if (item.GetValue("LBShortQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBShortQty")) != 0)
                                        ds.Tables[0].Rows[i]["ShortQty"] = Convert.ToDecimal(item.GetValue("LBShortQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["ShortQty"] = "";
                                    if (item.GetValue("LBSampleQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBSampleQty")) != 0)
                                        ds.Tables[0].Rows[i]["SampleQty"] = Convert.ToDecimal(item.GetValue("LBSampleQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["SampleQty"] = "";
                                    checknull = "N";
                                }
                            }
                            if (checknull == "Y")
                            {
                                ds.Tables[0].Rows[i]["AccQty"] = "";
                                ds.Tables[0].Rows[i]["RejQty"] = "";
                                ds.Tables[0].Rows[i]["RewQty"] = "";
                                ds.Tables[0].Rows[i]["ShortQty"] = "";
                                ds.Tables[0].Rows[i]["SampleQty"] = "";
                            }
                        }

                        foreach (JObject itms in jObjectBatch.Children())
                        {
                            //DataRow rows = ds.Tables[0].AsEnumerable().SingleOrDefault(r => IsNull(r.Field<string>("item_id"), "") == itms.GetValue("ItmCode").ToString().Trim() && IsNull(r.Field<string>("lot_id"), "") == itms.GetValue("LotNo").ToString().Trim() && IsNull(r.Field<string>("batch_no"), "") == itms.GetValue("ItemBatchNo").ToString().Trim());
                            DataRow rows = ds.Tables[0].AsEnumerable().SingleOrDefault(r => IsNull(r.Field<string>("item_id"), "").Trim() == itms.GetValue("ItmCode").ToString().Trim() && IsNull(r.Field<string>("lot_id"), "").Trim() == itms.GetValue("LotNo").ToString().Trim() && IsNull(r.Field<string>("batch_no"), "").Trim() == itms.GetValue("ItemBatchNo").ToString().Trim() && IsNull(r.Field<string>("serial_no"), "").Trim() == itms.GetValue("ItemSerialNo").ToString().Trim());
                            if (rows == null && ItemId == itms.GetValue("ItmCode").ToString().Trim())
                            {
                                DataRow dr = ds.Tables[0].NewRow();
                                dr["item_id"] = itms.GetValue("ItmCode").ToString().Trim();
                                dr["lot_id"] = itms.GetValue("LotNo").ToString().Trim();
                                dr["batch_no"] = itms.GetValue("ItemBatchNo").ToString().Trim();
                                dr["serial_no"] = itms.GetValue("ItemSerialNo").ToString().Trim();
                                dr["avl_batch_qty"] = "0";
                                dr["expiry_date"] = Convert.ToDateTime(itms.GetValue("ItemExpiryDate").ToString().Trim()).ToString("dd-MM-yyyy");
                                dr["exp_date"] = Convert.ToDateTime(itms.GetValue("ItemExpiryDate").ToString().Trim()).ToString("dd-MM-yyyy");
                                dr["exp_dt"] = Convert.ToDateTime(itms.GetValue("ItemExpiryDate").ToString().Trim()).ToString("dd-MM-yyyy");
                                dr["issue_qty"] = "";
                                dr["AccQty"] = itms.GetValue("LBAccQty").ToString().Trim();
                                dr["RejQty"] = itms.GetValue("LBRejQty").ToString().Trim();
                                dr["RewQty"] = itms.GetValue("LBRewQty").ToString().Trim();
                                dr["ShortQty"] = itms.GetValue("LBShortQty").ToString().Trim();
                                dr["SampleQty"] = itms.GetValue("LBSampleQty").ToString().Trim();
                                ds.Tables[0].Rows.Add(dr);
                            }

                        }


                    }

                }
                else
                {
                    ds.Tables[0].Columns.Add("AccQty", typeof(string), "");
                    ds.Tables[0].Columns.Add("RejQty", typeof(string), "");
                    ds.Tables[0].Columns.Add("RewQty", typeof(string), "");
                    ds.Tables[0].Columns.Add("ShortQty", typeof(string), "");
                    ds.Tables[0].Columns.Add("SampleQty", typeof(string), "");
                }
                //if (Status == "A" || Status == "C")
                //{
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        var num = Convert.ToDecimal(IsNull(dr["AccQty"].ToString(), "0")) + Convert.ToDecimal(IsNull(dr["RejQty"].ToString(), "0")) + Convert.ToDecimal(IsNull(dr["RewQty"].ToString(), "0"));
                //        if ((num) == 0)
                //            dr.Delete();
                //    }
                //    ds.Tables[0].AcceptChanges();
                //}

                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "lot_id,batch_no asc";
                DataTable dttbl = dv.ToTable();
                _QualityInspectionModel.qc_type = Src_Type;
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = dttbl;//ds.Tables[0];
                

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAdhocQCBatchLotWiseDetail.cshtml", _QualityInspectionModel);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getItemStockBatchWiseOfShopFloor(string ItemId, string Status, string ShflID, string SelectedItemdetail, string TransType, string Command, string DocumentStatus, string DocumentMenuId, string UOMId,string Src_Type)
        {
            try
            {
                QualityInspectionModel _QualityInspectionModel = new QualityInspectionModel();
                if (TransType != null)
                {
                    _QualityInspectionModel.TransType = TransType;
                }
                if (Command != null)
                {
                    _QualityInspectionModel.Command = Command;
                }
                if (DocumentStatus != null)
                {
                    _QualityInspectionModel.DocumentStatus = DocumentStatus;
                }
                if (DocumentMenuId != null)
                {
                    _QualityInspectionModel.DocumentMenuId = DocumentMenuId;
                }
                var QtyDigit = "0.";
                var VD = Convert.ToInt32(Session["QtyDigit"].ToString());
                for (var i = 0; i < VD; i++)
                {
                    QtyDigit += "0";
                }
                DataSet ds = new DataSet();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                
                if (Status != "A" && Status != "C")
                {
                    ds = _QualityInspection_ISERVICES.getItemStockBatchWiseofShopFloor(CompID, BrchID, ItemId, ShflID, UOMId);
                    
                }
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {

                    if (Status != "A" && Status != "C")
                    {
                        ds.Tables[0].Columns.Add("AccQty", typeof(string));
                        ds.Tables[0].Columns.Add("RejQty", typeof(string));
                        ds.Tables[0].Columns.Add("RewQty", typeof(string));
                        ds.Tables[0].Columns.Add("ShortQty", typeof(string));
                        ds.Tables[0].Columns.Add("SampleQty", typeof(string));
                    }
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    DataTable dt = new DataTable();
                    if (Status == "A" || Status == "C")
                    {


                        //QualityInspectionModel _QualityInspectionModel = new QualityInspectionModel();
                        string qc_no = jObjectBatch[0]["qc_no"].ToString();
                        string qc_dt = jObjectBatch[0]["qc_dt"].ToString();
                        string item_id = jObjectBatch[0]["ItmCode"].ToString();
                        _QualityInspectionModel.qc_no = qc_no;
                        _QualityInspectionModel.qc_dt = Convert.ToDateTime(qc_dt);
                        _QualityInspectionModel.item_id = ItemId;
                        _QualityInspectionModel.SourceSF = Convert.ToInt32(ShflID);
                        ds = _QualityInspection_ISERVICES.AfterApproveItemStockDetailBatchLotWiseforShopfloor(_QualityInspectionModel, CompID, BrchID);
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            string checknull = "Y";
                            foreach (JObject item in jObjectBatch.Children())
                            {
                                //if (item.GetValue("ItmCode").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("ItemBatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim())
                                if (item.GetValue("ItmCode").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("ItemBatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim() && item.GetValue("ItemSerialNo").ToString().Trim() == ds.Tables[0].Rows[i]["serial_no"].ToString().Trim())
                                {
                                    if (item.GetValue("LBAccQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBAccQty")) != 0)
                                        ds.Tables[0].Rows[i]["AccQty"] = Convert.ToDecimal(item.GetValue("LBAccQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["AccQty"] = "";
                                    if (item.GetValue("LBRejQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBRejQty")) != 0)
                                        ds.Tables[0].Rows[i]["RejQty"] = Convert.ToDecimal(item.GetValue("LBRejQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["RejQty"] = "";
                                    if (item.GetValue("LBRewQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBRewQty")) != 0)
                                        ds.Tables[0].Rows[i]["RewQty"] = Convert.ToDecimal(item.GetValue("LBRewQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["RewQty"] = "";
                                    if (item.GetValue("LBShortQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBShortQty")) != 0)
                                        ds.Tables[0].Rows[i]["ShortQty"] = Convert.ToDecimal(item.GetValue("LBShortQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["ShortQty"] = "";
                                    if (item.GetValue("LBSampleQty").ToString() != "" && Convert.ToDecimal(item.GetValue("LBSampleQty")) != 0)
                                        ds.Tables[0].Rows[i]["SampleQty"] = Convert.ToDecimal(item.GetValue("LBSampleQty")).ToString(QtyDigit);
                                    else
                                        ds.Tables[0].Rows[i]["SampleQty"] = "";
                                    checknull = "N";
                                }
                            }
                            if (checknull == "Y")
                            {
                                ds.Tables[0].Rows[i]["AccQty"] = "";
                                ds.Tables[0].Rows[i]["RejQty"] = "";
                                ds.Tables[0].Rows[i]["RewQty"] = "";
                                ds.Tables[0].Rows[i]["ShortQty"] = "";
                                ds.Tables[0].Rows[i]["SampleQty"] = "";
                            }
                        }

                        foreach (JObject itms in jObjectBatch.Children())
                        {
                            DataRow rows = ds.Tables[0].AsEnumerable().SingleOrDefault(r => IsNull(r.Field<string>("item_id"), "") == itms.GetValue("ItmCode").ToString().Trim() && IsNull(r.Field<string>("lot_id"), "") == itms.GetValue("LotNo").ToString().Trim() && IsNull(r.Field<string>("batch_no"), "") == itms.GetValue("ItemBatchNo").ToString().Trim() && IsNull(r.Field<string>("serial_no"), "") == itms.GetValue("ItemSerialNo").ToString().Trim());
                            if (rows == null && ItemId == itms.GetValue("ItmCode").ToString().Trim())
                            {
                                DataRow dr = ds.Tables[0].NewRow();
                                dr["item_id"] = itms.GetValue("ItmCode").ToString().Trim();
                                dr["lot_id"] = itms.GetValue("LotNo").ToString().Trim();
                                dr["batch_no"] = itms.GetValue("ItemBatchNo").ToString().Trim();
                                dr["serial_no"] = itms.GetValue("ItemSerialNo").ToString().Trim();
                                dr["avl_batch_qty"] = "0";
                                dr["expiry_date"] = Convert.ToDateTime(itms.GetValue("ItemExpiryDate").ToString().Trim()).ToString("dd-MM-yyyy");
                                dr["exp_date"] = Convert.ToDateTime(itms.GetValue("ItemExpiryDate").ToString().Trim()).ToString("dd-MM-yyyy");
                                dr["exp_dt"] = Convert.ToDateTime(itms.GetValue("ItemExpiryDate").ToString().Trim()).ToString("dd-MM-yyyy");
                                dr["issue_qty"] = "";
                                dr["AccQty"] = itms.GetValue("LBAccQty").ToString().Trim();
                                dr["RejQty"] = itms.GetValue("LBRejQty").ToString().Trim();
                                dr["RewQty"] = itms.GetValue("LBRewQty").ToString().Trim();
                                dr["ShortQty"] = itms.GetValue("LBShortQty").ToString().Trim();
                                dr["SampleQty"] = itms.GetValue("LBSampleQty").ToString().Trim();
                                ds.Tables[0].Rows.Add(dr);
                            }

                        }


                    }

                }
                else
                {
                    ds.Tables[0].Columns.Add("AccQty", typeof(string), "");
                    ds.Tables[0].Columns.Add("RejQty", typeof(string), "");
                    ds.Tables[0].Columns.Add("RewQty", typeof(string), "");
                    ds.Tables[0].Columns.Add("ShortQty", typeof(string), "");
                    ds.Tables[0].Columns.Add("SampleQty", typeof(string), "");
                }
                //if (Status == "A" || Status == "C")
                //{
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        var num = Convert.ToDecimal(IsNull(dr["AccQty"].ToString(), "0")) + Convert.ToDecimal(IsNull(dr["RejQty"].ToString(), "0")) + Convert.ToDecimal(IsNull(dr["RewQty"].ToString(), "0"));
                //        if ((num) == 0)
                //            dr.Delete();
                //    }
                //    ds.Tables[0].AcceptChanges();
                //}

                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "lot_id,batch_no asc";
                DataTable dttbl = dv.ToTable();
                _QualityInspectionModel.qc_type = Src_Type;
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = dttbl;//ds.Tables[0];
               

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAdhocQCBatchLotWiseDetail.cshtml", _QualityInspectionModel);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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
        private string ToDecimal(string strNumber,int  QtyDigit)
        {
            var DecDigit = "0.";
            //var QtyDigit = Convert.ToInt32(Session["QtyDigit"].ToString());
            for (var i = 0; i < QtyDigit; i++)
                DecDigit += "0";

            strNumber = string.IsNullOrEmpty(strNumber) == true ? "0" : strNumber;
            return Convert.ToDecimal(strNumber).ToString(DecDigit);

        }

        /*--------------------------WorkFlow Methods Start-----------------------------*/

        public ActionResult ToRefreshByJS(string ListFilterData1,string TrancType)
        {
            //Session["Message"] = "";
            //var WF_status1 = "";
            QualityInspectionModel _QualityInspectionModel = new QualityInspectionModel();
            var a = TrancType.Split(',');
            _QualityInspectionModel.qc_no = a[0].Trim();
            _QualityInspectionModel.qc_type = a[1].Trim();
            _QualityInspectionModel.DocumentMenuId = a[2].Trim();
            _QualityInspectionModel.TransType = "Update";
            //if (_QualityInspectionModel.qc_type=="RWK")
            //{
            //     WF_status1 = a[5].Trim();
            //}
            //else
            //{
            //    WF_status1 = a[3].Trim();

            //}
            var WF_status1 = a[3].Trim();
            _QualityInspectionModel.WF_status1 = WF_status1;
            _QualityInspectionModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _QualityInspectionModel;
            TempData["WF_status1"] = WF_status1; ;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.DocNo = a[0].Trim();
            URLModel.QC_Type = a[01].Trim();
            URLModel.TransType = "Update";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.WF_status1 = WF_status1;
            URLModel.DocumentMenuId = a[2].Trim();
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("QualityInspectionDetail", URLModel);
        }

        public ActionResult GetQualityInspectionList(string docid, string status)
        {
            QCInspectionList_Model _QCInspectionList_Model = new QCInspectionList_Model();
            if(docid== "105102120")
            {
                //Session["WF_status"] = status;
                _QCInspectionList_Model.WF_status = status;
                return RedirectToAction("QualityInspectionListInv", _QCInspectionList_Model);
            }else
            if(docid == "105105135")
            {
                _QCInspectionList_Model.WF_status = status;
                return RedirectToAction("QualityInspectionListPrd", _QCInspectionList_Model);
            }else
            if(docid == "105108115")
            {
                _QCInspectionList_Model.WF_status = status;
                return RedirectToAction("QualityInspectionListSC", _QCInspectionList_Model);
            }
            //Session["WF_status"] = status;
            //Session["MenuDocumentId"] = docid;
            return RedirectToAction("QualityInspectionList");
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_status1, string docid, string qc_type,string Loc_type)/*add Loc_type by Hina on 29-08-2024 to random qc for shopfloor*/
        {

            QualityInspectionModel QI_Model = new QualityInspectionModel();
            URLDetailModel URLModel = new URLDetailModel();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    QI_Model.qc_type = jObjectBatch[i]["qc_type"].ToString();
                    QI_Model.qc_no = jObjectBatch[i]["qc_no"].ToString();
                    QI_Model.qc_dt = Convert.ToDateTime(jObjectBatch[i]["qc_dt"]);
                    QI_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    QI_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    QI_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                    QI_Model.MenuDocumentId = jObjectBatch[i]["DocMenuId"].ToString();
                    QI_Model.Location_type = jObjectBatch[i]["Loc_type"].ToString();/*add Loc_type by Hina on 29-08-2024 to random qc for shopfloor*/
                }
            }
            if (QI_Model.A_Status != "Approve")
            {
                QI_Model.A_Status = "Approve";
            }
            string command = "";
            QCInspectionApprove(QI_Model, command, ListFilterData1, WF_status1, docid, qc_type, Loc_type);/*add Loc_type by Hina on 29-08-2024 to random qc for shopfloor*/

            URLModel.DocNo = QI_Model.qc_no;
            URLModel.QC_Type = QI_Model.qc_type;
            URLModel.TransType = QI_Model.TransType;
            URLModel.BtnName = QI_Model.BtnName;
            URLModel.Command = QI_Model.Command;
            URLModel.DocumentMenuId = docid;
            URLModel.WF_status1 = WF_status1;
            TempData["WF_status1"] = WF_status1;
            TempData["ModelData"] = QI_Model;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("QualityInspectionDetail", URLModel);
        }


        /*--------------------------WorkFlow Methods End-----------------------------*/
        public string SetDocMenuID(string Src_Type)
        {
            if (Src_Type == "PUR" || Src_Type == "RQC" || Src_Type == "SMR" || Src_Type=="FGR")
            {
                Session["MenuDocumentId"] = "105102120";
                DocumentMenuId = "105102120";
            }
            else if (Src_Type == "PRD"|| Src_Type == "RWK" || Src_Type == "PJO")
            {
                Session["MenuDocumentId"] = "105105135";
                DocumentMenuId = "105105135";
            }
            else if (Src_Type == "SCQ")
            {
                Session["MenuDocumentId"] = "105108115";
                DocumentMenuId = "105108115";
            }
            var other = new CommonController(_Common_IServices);
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            dt = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
            string level = "0";
            if (dt.Rows.Count > 0)
            {
                level = dt.Rows[dt.Rows.Count - 1]["level"].ToString();
            }


            return DocumentMenuId + "," + level;
        }

        [HttpPost]
        public JsonResult GetBatchNo(string DocumentNumber) 
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
                DataSet result = _QualityInspection_ISERVICES.GetBatchNo(Comp_ID, Br_ID, DocumentNumber);
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
        public ActionResult GetSubItemWhAvlstockDetails(string Wh_id, string Item_id, string flag)
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
                DataSet ds = _Common_IServices.GetSubItemWhAvlstockDetails(Comp_ID, Br_ID, Wh_id, Item_id,null/*UomId*/, flag);
                DataView data = ds.Tables[0].DefaultView;
                data.RowFilter = "[avl_stk] IS NOT NULL AND [avl_stk] <> '' AND CONVERT([avl_stk], 'System.Decimal') > 0.0";
                DataTable dt = data.ToTable();
                ViewBag.SubitemAvlStockDetail = dt;// ds.Tables[0];
                if (flag == "whres")
                {
                    ViewBag.Flag = "ReservStock";
                }
                else
                {
                    ViewBag.Flag = "WH";
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetSubItemShflAvlstockDetails(string Shfl_id, string Item_id, string flag)/*Add by Hina on 23-08-2024 to random qc for shopfloor*/
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
                DataSet ds = _Common_IServices.GetSubItemShflAvlstockDetails(Comp_ID, Br_ID, Shfl_id, Item_id, flag);
                DataView data = ds.Tables[0].DefaultView;
                data.RowFilter = "[avl_stk] IS NOT NULL AND [avl_stk] <> '' AND CONVERT([avl_stk], 'System.Decimal') > 0.0";
                DataTable dt = data.ToTable();
                ViewBag.SubitemAvlStockDetail = dt;// ds.Tables[0];
                ViewBag.Flag = "Shfl";
                
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, string Flag
            , string Status, string Doc_no, string Doc_dt,string src_wh_id,string qc_type,string location_type)
        {
            try
            {
                var QtyDigit = 0; 

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();
                //if (Flag == "ProduceQty")
                //{
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                    if (qc_type == "PUR")
                    {
                        QtyDigit = Convert.ToInt32(Session["QtyDigit"].ToString());
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "PUR").Tables[0];

                        //dt.Columns.Add("accept_qty", typeof(string));
                        //dt.Columns.Add("prod_qty", typeof(string));
                        //dt.Columns.Add("reject_qty", typeof(string));
                        //dt.Columns.Add("rework_qty", typeof(string));
                    }
                    else if (qc_type == "RQC")
                    {
                        if(location_type=="WH")/*Add by Hina on 23-08-2024 to random qc for shopfloor*/
                        {
                            QtyDigit = Convert.ToInt32(Session["QtyDigit"].ToString());
                            dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, src_wh_id, Item_id, null/*UomId*/, "wh").Tables[0];
                            DataView data = dt.DefaultView;
                            data.RowFilter = "[avl_stk] IS NOT NULL AND [avl_stk] <> '' AND CONVERT([avl_stk], 'System.Decimal') > 0.0";
                            dt = data.ToTable();
                            dt.Columns.Add("accept_qty", typeof(string));
                            dt.Columns.Add("prod_qty", typeof(string));
                            dt.Columns.Add("reject_qty", typeof(string));
                            dt.Columns.Add("rework_qty", typeof(string));
                            dt.Columns.Add("short_qty", typeof(string));
                            dt.Columns.Add("sample_qty", typeof(string));
                        }
                        else
                        {
                            QtyDigit = Convert.ToInt32(Session["QtyDigit"].ToString());
                            dt = _Common_IServices.GetSubItemShflAvlstockDetails(CompID, BrchID, src_wh_id, Item_id,"shfl").Tables[0];
                            DataView data = dt.DefaultView;
                            data.RowFilter = "[avl_stk] IS NOT NULL AND [avl_stk] <> '' AND CONVERT([avl_stk], 'System.Decimal') > 0.0";
                            dt = data.ToTable();
                            dt.Columns.Add("accept_qty", typeof(string));
                            dt.Columns.Add("prod_qty", typeof(string));
                            dt.Columns.Add("reject_qty", typeof(string));
                            dt.Columns.Add("rework_qty", typeof(string));
                            dt.Columns.Add("short_qty", typeof(string));
                            dt.Columns.Add("sample_qty", typeof(string));
                        }
                        
                    }
                    else if(qc_type == "RWK")
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "RWK").Tables[0];
                        ViewBag.QCtype = "RWK";
                    }
                    else if (qc_type == "PJO")
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "PJO").Tables[0];
                        ViewBag.QCtype = "PJO";
                    }
                    else if (qc_type == "FGR")
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "FGR").Tables[0];
                        ViewBag.QCtype = "PJO";
                    }
                    else if (qc_type == "SMR")
                    {
                        QtyDigit = Convert.ToInt32(Session["QtyDigit"].ToString());
                        if (Flag == "ProduceQty"||(Flag == "QCAccQty" && Status == "") || (Flag == "QCRejQty" && Status == "") || (Flag == "QCRewQty" && Status == ""))
                        {
                            dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "SMR").Tables[0];
                        }
                        else
                        {
                            dt = _QualityInspection_ISERVICES.QCRWK_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, "SMR").Tables[0];

                        }
                        
                    }
                    else if(qc_type == "SCQ")
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "SCQ").Tables[0];
                        
                    }
                    else
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "PRD_QC").Tables[0];
                    }
                     
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                if (qc_type == "PRD")
                                {
                                    dt.Rows[i]["pend_qty"] = ToDecimal(IsNull(item.GetValue("PendQty").ToString(), "0"), QtyDigit);
                                    dt.Rows[i]["qc_qty"] = ToDecimal(IsNull(item.GetValue("qty").ToString(), "0"), QtyDigit);
                                }                                  
                                
                                /**----------------------------------***/
                                if(Flag== "PRDProduceQty"|| Flag == "PRDProdQty"|| Flag == "PRDPendQty")
                                {
                                    dt.Rows[i]["prod_qty"] = ToDecimal(IsNull(item.GetValue("ProdQty").ToString(), "0"), QtyDigit);
                                }
                                else
                                {
                                    dt.Rows[i]["prod_qty"] = ToDecimal(IsNull(item.GetValue("qty").ToString(), "0"), QtyDigit);
                                }
                                dt.Rows[i]["accept_qty"] = ToDecimal(IsNull(item.GetValue("Accqty").ToString(), "0"), QtyDigit);
                                dt.Rows[i]["reject_qty"] = ToDecimal(IsNull(item.GetValue("Rejqty").ToString(), "0"), QtyDigit);
                                dt.Rows[i]["rework_qty"] = ToDecimal(IsNull(item.GetValue("Rewqty").ToString(), "0"), QtyDigit);
                                if (qc_type == "PUR" || qc_type == "RQC")
                                {
                                    dt.Rows[i]["short_qty"] = ToDecimal(IsNull(item.GetValue("Shortqty").ToString(), "0"), QtyDigit);
                                    dt.Rows[i]["sample_qty"] = ToDecimal(IsNull(item.GetValue("Sampleqty").ToString(), "0"), QtyDigit);
                                }
                            }
                            }
                        }
                    }
                    else
                    {
                    if (qc_type == "PUR")
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "ApprvPUR").Tables[0];
                    }
                    else if (qc_type == "RQC")
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "RQC").Tables[0];
                    }
                    else if (qc_type == "SCQ")
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetailsAftrApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, "SCQ").Tables[0];
                    }
                    else if (qc_type == "RWK")
                    {
                        dt = _QualityInspection_ISERVICES.QCRWK_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                        //Commented by Hina on 25-10-2023 using by multiple flag instead of Flag == "ProduceQty"
                        //if (Flag == "ProduceQty")
                        //{
                        //    dt = _QualityInspection_ISERVICES.QCRWK_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                        //}
                        //else
                        //{
                        //    dt = _QualityInspection_ISERVICES.QCRWK_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, "RWK").Tables[0];
                        //}
                    }
                    else if (qc_type == "PJO")
                    {
                        dt = _QualityInspection_ISERVICES.QCRWK_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, "PJO").Tables[0];
                      
                    }
                    //else if (qc_type == "FGR")
                    //{
                    //    dt = _QualityInspection_ISERVICES.QCRWK_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, "PJO").Tables[0];

                    //}
                    else if (qc_type == "SMR")
                    {
                        if (Flag == "ProduceQty")
                        {
                            dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "SMR").Tables[0];
                        }
                        else
                        {
                            dt = _QualityInspection_ISERVICES.QCRWK_GetSubItemDetailsAfterApprov(CompID, BrchID, Item_id, Doc_no, Doc_dt, "SMR").Tables[0];
                            //Flag = "ProduceQty";
                        }
                    }
                    else if(qc_type== "PRD")
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "PRDApproved").Tables[0];
                    }
                    else
                    {
                        dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Status).Tables[0];
                    }
                    
                }
                //}
                //else
                //{
                //    dt = _QualityInspection_ISERVICES.QC_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                //}
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    _subitemPageName = qc_type == "RQC" ? "RQC" : "QC",
                    decimalAllowed = (qc_type == "RQC"|| qc_type == "PUR" ||  qc_type == "FGR") ? "Y" : "N",
                    ShowStock = (Flag == "ProduceQty" && qc_type == "RQC"||qc_type == "RWK" || qc_type == "PJO") ? "Y" : "N",
                    //QCType= qc_type == "RWK" ? "RWK" : ""

                };
                ViewBag.SubItemDetails = dt;
                ViewBag.IsDisabled = IsDisabled;
                ViewBag.Flag = Flag;
                ViewBag.QcType = qc_type;
                ViewBag._subitemPageName = "QC";
                
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        public ActionResult GetItemList(BindItemList bindItem, string PageName,string SrcWh_Id,string LocationTyp,string suppid)/*Add LocationTyp by Hina on 21-08-2024 for random qc by shopfloor*/
        {
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (Session["CompId"] != null)
                {
                    comp_id = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                DataSet SOItmList = _QualityInspection_ISERVICES.GetItmListDL(comp_id, BrchID, bindItem.SearchName, PageName, SrcWh_Id,LocationTyp, suppid);
                //if (SOItmList.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < SOItmList.Tables[0].Rows.Count; i++)
                //    {
                //        string itemId = SOItmList.Tables[0].Rows[i]["Item_id"].ToString();
                //        string itemName = SOItmList.Tables[0].Rows[i]["Item_name"].ToString();
                //        string Uom = SOItmList.Tables[0].Rows[i]["uom_name"].ToString();
                //        ItemList.Add(itemId + "_" + Uom, itemName);
                //    }
                //}
                JsonResult DataRows = Json(JsonConvert.SerializeObject(SOItmList), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            //return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public FileResult GenratePdfFile(QualityInspectionModel _model)
        {
            return File(GetPdfData(_model), "application/pdf", ViewBag.Title.Replace(" ", "") + ".pdf");
        }
        public byte[] GetPdfData(QualityInspectionModel _model)
        {
            StringReader reader = null;
            StringReader reader2 = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                DataSet dsData = _QualityInspection_ISERVICES.GetQualityInspectionPrintDetails(CompID, BrchID, _model.qc_no, _model.qc_dt.ToString("yyyy-MM-dd"), _model.qc_type);
                ViewBag.PdfData = dsData;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + dsData.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Title = "Quality Inspection";
                ViewBag.DocID = _model.DocumentMenuId;
                ViewBag.SrcTyp = _model.qc_type;
                ViewBag.Page = "1";//for render page 1 data
                string htmlContent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionPrint.cshtml"));
                ViewBag.Page = "2";//for render page 2 data
                string htmlContent2 = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/QualityInspection/QualityInspectionPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlContent);
                    reader2 = new StringReader(htmlContent2);
                    pdfDoc = new Document(PageSize.A4.Rotate(), 20f, 20f, 20f, 30f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);

                    pdfDoc.NewPage();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader2);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    bytes = HeaderFooterPagination(bytes, dsData);
                    return bytes.ToArray();
                }
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return null;
            }
        }
        private Byte[] HeaderFooterPagination(Byte[] bytes, DataSet Details)
        {
            var docstatus = Details.Tables[0].Rows[0]["qc_status"].ToString().Trim();
            var comp_nm = Details.Tables[0].Rows[0]["comp_nm"].ToString().Trim();

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font font = new Font(bf, 9, Font.NORMAL);
            Font font1 = new Font(bf, 8, Font.NORMAL);
            Font fontb = new Font(bf, 9, Font.NORMAL);
            fontb.SetStyle("bold");
            Font fonttitle = new Font(bf, 13, Font.BOLD);
            fonttitle.SetStyle("underline");
            //string logo = ConfigurationManager.AppSettings["LocalServerURL"].ToString() + Details.Tables[0].Rows[0]["logo"].ToString().Replace("Attachment", "");
            string draftImage = Server.MapPath("~/Content/Images/draft.png");

            using (var reader1 = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader1, ms))
                    {
                        var draftimg = Image.GetInstance(draftImage);
                        draftimg.SetAbsolutePosition(90, 90);
                        draftimg.ScaleAbsolute(580f, 580f);
                        int PageCount = reader1.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            var content = stamper.GetUnderContent(i);
                            if (docstatus == "D" || docstatus == "F")
                            {
                                content.AddImage(draftimg);
                            }
                            try
                            {
                                //var image = Image.GetInstance(logo);
                                //image.SetAbsolutePosition(30, 540);
                                //image.ScaleAbsolute(90f, 25f);
                                //if (i == 1)
                                //    content.AddImage(image);
                            }
                            catch { }
                            Phrase p = new Phrase(String.Format("Page {0} of {1}", i, PageCount), font);
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 820, 10, 0);
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
        //Added by Nidhi on 09-07-2025
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, DateTime Doc_dt, string qc_type,string fileName, string docid, string docstatus)
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
                QualityInspectionModel _model = new QualityInspectionModel();
                _model.qc_no = Doc_no;
                _model.qc_dt = Doc_dt;
                _model.qc_type = qc_type;
                DataTable dt = new DataTable();
                var commonCont = new CommonController(_Common_IServices);
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(_model);
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
    }
}