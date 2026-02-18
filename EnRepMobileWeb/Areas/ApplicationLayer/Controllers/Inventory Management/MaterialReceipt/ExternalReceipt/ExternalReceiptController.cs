using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.ExternalReceipt;
using System;
using System.Data;
using System.Web.Mvc;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Collections.Generic;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.ExternalReceipt;
using EnRepMobileWeb.MODELS.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialReceipt.ExternalReceipt
{
    public class ExternalReceiptController : Controller
    {
        string CompID, brnchID, UserID, language = String.Empty;
        string DocumentMenuId = "105102115130", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ExternalReceipt_ISERVICES _ExternalReceipt_ISERVICES;
        public ExternalReceiptController(Common_IServices _Common_IServices, ExternalReceipt_ISERVICES _ExternalReceipt_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._ExternalReceipt_ISERVICES = _ExternalReceipt_ISERVICES;
        }
        // GET: ApplicationLayer/ExternalReceipt

        public ActionResult DashBordtoList(string docid, string status)
        {
            var WF_status = status;
            return RedirectToAction("ExternalReceipt", new { WF_status });
        }

        public ActionResult ExternalReceipt(string WF_status)
        {
            try
            {
                ExternalReceiptModel ListModel = new ExternalReceiptModel();
                CompDeatil();
                CommonPageDetails();
                BindAllDropDownList(ListModel);
                ListModel.WF_Status = WF_status;

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                DataTable ListData = new DataTable();
                ExternalReceiptModel Gpass_model = new ExternalReceiptModel();
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var Entity_type = a[0].Trim();
                    var Entity_name = a[1].Trim();
                    var Fromdate = a[2].Trim();
                    var Todate = a[3].Trim();
                    var Status = a[4].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    ListModel.EntityName = Entity_name;
                    Gpass_model.entity_type = Entity_type;
                    GetSupp_CustList(Gpass_model);
                    ListModel.EntityNameList1 = Gpass_model.EntityNameList1;
                    ListModel.EntityName = a[1].Trim();
                    ListModel.FromDate = Fromdate;
                    ListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    GetSupp_CustList(Gpass_model);
                    DataTable dt = _ExternalReceipt_ISERVICES.SearchDataFilter( Entity_type, Entity_name, Fromdate, Todate, Status, CompID, brnchID, DocumentMenuId);
                    ViewBag.ExternalReciptList = dt;
                    ListModel.EntityType = Entity_type;
                    ListModel.EntityName = Entity_name;
                    ListModel.FromDate = Fromdate;
                    ListModel.ToDate = Todate;
                    ListModel.Status = Status;

                }
                else
                {
                    ListModel.FromDate = startDate;
                    ListModel.ToDate = CurrentDate;
                    ListData = GetListData(ListModel);
                  
                    ViewBag.ExternalReciptList = ListData;
                }
                
                ListModel.Title = title;
               
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/ExternalReceipt/ExternalReceiptList.cshtml", ListModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
          
        }
        public ActionResult GetSupp_CustList(ExternalReceiptModel ListModel)
        {
            try
            {
                CompDeatil();
                string EntityName = string.Empty;
                string EntityType = string.Empty;
                List<EntityNameList1> _EntityName = new List<EntityNameList1>();
                if (string.IsNullOrEmpty(ListModel.EntityName))
                {
                    EntityName = "0";
                }
                else
                {
                    EntityName = ListModel.EntityName.ToString();
                }
                if (string.IsNullOrEmpty(ListModel.entity_type))
                {
                    EntityType = "0";
                }
                else
                {
                    EntityType = ListModel.entity_type.ToString();
                }
                DataSet SuppCustList = _ExternalReceipt_ISERVICES.getSuppCustList(CompID, brnchID, EntityName, EntityType);
                if (EntityType == "0")
                {
                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList1 _EntityNameList = new EntityNameList1();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                else
                {
                    DataRow Drow = SuppCustList.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    SuppCustList.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList1 _EntityNameList = new EntityNameList1();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                ListModel.EntityNameList1 = _EntityName;
                return Json(_EntityName.Select(c => new { Name = c.entity_name, ID = c.entity_id }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public void BindAllDropDownList(ExternalReceiptModel ListModel)
        {
            List<EntityNameList1> _EntityName = new List<EntityNameList1>();
            EntityNameList1 _EntityNameList = new EntityNameList1();
            _EntityNameList.entity_name = "---Select---";
            _EntityNameList.entity_id = "0";
            _EntityName.Add(_EntityNameList);
            ListModel.EntityNameList1 = _EntityName;
            List<StatusList> list2 = new List<StatusList>();
            foreach (var dr in ViewBag.StatusList.Rows)
            {
                StatusList Status = new StatusList();
                Status.StatusID = dr["status_code"].ToString();
                Status.Status_Name = dr["status_name"].ToString();
                list2.Add(Status);
            }

            ListModel.Status_list = list2;

        }
        [HttpPost]
        public ActionResult DataSearch_Search( string Entity_type, string Entity_id, string Fromdate, string Todate, string Status)
        {
            try
            {
                CompDeatil();
                ExternalReceiptModel SearchModel = new ExternalReceiptModel();
                SearchModel.WF_Status = null;
                DataTable dt = _ExternalReceipt_ISERVICES.SearchDataFilter(Entity_type, Entity_id, Fromdate, Todate, Status, CompID, brnchID, DocumentMenuId);
                SearchModel.Search = "Search";
                ViewBag.ExternalReciptList = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialExternalReceiptList.cshtml", SearchModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        private DataTable GetListData(ExternalReceiptModel _listModel)
        {
            CompDeatil();
            string wfstatus = "";

            if (_listModel.WF_Status != null)
            {
                wfstatus = _listModel.WF_Status;
            }
            else
            {
                wfstatus = "";
            }

            DataSet dt = _ExternalReceipt_ISERVICES.GetAllDropDownList(CompID, brnchID, UserID, wfstatus, DocumentMenuId, _listModel.FromDate, _listModel.ToDate);
            return dt.Tables[0];
        }
        public ActionResult AddExternalReceiptDetail()
        {
            try
            {
                CompDeatil();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                {
                    TempData["Message"] = "Financial Year not Exist";
                    return RedirectToAction("ExternalReceipt");
                }
                else
                {
                    ExternalReceiptDeatilModel AddNew_Model = new ExternalReceiptDeatilModel();
                    AddNew_Model.Massage = null;
                    AddNew_Model.TransType = "Save";
                    AddNew_Model.Command = "AddNew";
                    AddNew_Model.BtnName = "BtnAddNew";
                    TempData["ModelData"] = AddNew_Model;
                    UrlModel NewModel = new UrlModel();
                    NewModel.Cmd = "AddNew";
                    NewModel.tp = "Save";
                    NewModel.bt = "BtnAddNew";
                    return RedirectToAction("ExternalReceiptDetail", "ExternalReceipt", NewModel);
                }
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            
        }
        public ActionResult ExternalReceiptDetail(UrlModel _urlModel)
        {
            try
            {
                CompDeatil();
                CommonPageDetails();
                /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, brnchID, _urlModel.recpt_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var DetailModel = TempData["ModelData"] as ExternalReceiptDeatilModel;
                if (DetailModel != null)
                {
                    if (DetailModel.ReceiptDate == null)
                    {
                        DetailModel.ReceiptDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    BindAllDropdownList_Deatil(DetailModel);
                    DetailModel.Title = title;
                    DetailModel.Qty_pari_Command = DetailModel.Command;
                    //DetailModel.BatchCommand = DetailModel.BatchCommand;
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/ExternalReceipt/ExternalReceiptDetail.cshtml", DetailModel);
                }
                else
                {
                    ExternalReceiptDeatilModel _model = new ExternalReceiptDeatilModel();
                    if (_model.ReceiptDate == null)
                    {
                        _model.ReceiptDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    SetUrlData(_urlModel, _model);
                    BindAllDropdownList_Deatil(_model);
                  
                  
                    _model.Title = title;
                    _model.Qty_pari_Command = _model.Command;
                    //  _model.BatchCommand = _model.BatchCommand;
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/ExternalReceipt/ExternalReceiptDetail.cshtml", _model);
                }
                   
              
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public void BindAllDropdownList_Deatil(ExternalReceiptDeatilModel DetailModel)
        {
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                DetailModel.ListFilterData1 = TempData["ListFilterData"].ToString();
            }
            List<SrcDocNoList> srcDocNoLists = new List<SrcDocNoList>();
            srcDocNoLists.Add(new SrcDocNoList { SrcDocnoId = "0", SrcDocnoVal = "---Select---" });
            DetailModel.docNoLists = srcDocNoLists;
            List<EntityNameList> _EntityName = new List<EntityNameList>();
            EntityNameList _EntityNameList = new EntityNameList();
            _EntityNameList.entity_name = "---Select---";
            _EntityNameList.entity_id = "0";
            _EntityName.Add(_EntityNameList);
            DetailModel.EntityNameList = _EntityName;
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
            DetailModel.WarehouseList = requirementAreaLists;

            if (DetailModel.TransType == "Update")
            {
                SetAllDataInView(DetailModel);
            }
        }
       
        public void SetAllDataInView(ExternalReceiptDeatilModel DetailModel)
        {
            try
            {
                CompDeatil();
                  string recpt_no  = DetailModel.ReceiptNumber;
                string recpt_dt = DetailModel.ReceiptDate;
                DataSet ds = new DataSet();
                ds = _ExternalReceipt_ISERVICES.GetDeatilData(CompID, brnchID, recpt_no, recpt_dt, UserID, DocumentMenuId);
                ViewBag.AttechmentDetails = ds.Tables[5];
                ViewBag.ItemDetailData = ds.Tables[1];
                ViewBag.ItemStockBatchWise = ds.Tables[3];
                ViewBag.ItemStockSerialWise = ds.Tables[4];
                ViewBag.SubItemDetails = ds.Tables[2];
                DetailModel.ReceiptNumber = ds.Tables[0].Rows[0]["recpt_no"].ToString();
                DetailModel.ReceiptDate = ds.Tables[0].Rows[0]["gpass_dt"].ToString();
                DetailModel.EntityType = ds.Tables[0].Rows[0]["entity_type"].ToString().Trim();
                DetailModel.EntityTypeID = ds.Tables[0].Rows[0]["entity_type"].ToString().Trim();
                DetailModel.EntityID = ds.Tables[0].Rows[0]["entity_id"].ToString();
                DetailModel.EntityName = ds.Tables[0].Rows[0]["Entityname"].ToString();
                DetailModel.CheckedBy = ds.Tables[0].Rows[0]["check_by"].ToString();
                DetailModel.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                DetailModel.Created_by = ds.Tables[0].Rows[0]["createname"].ToString();
                DetailModel.Created_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                DetailModel.Approved_by = ds.Tables[0].Rows[0]["appname"].ToString();
                DetailModel.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                DetailModel.Amended_by = ds.Tables[0].Rows[0]["modname"].ToString();
                DetailModel.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                DetailModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                DetailModel.Status_Code = ds.Tables[0].Rows[0]["recpt_status"].ToString().Trim();
                DetailModel.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();

                string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                DetailModel.StatusCode = doc_status;
                DetailModel.doc_status = doc_status;
                DetailModel.DocumentStatus = doc_status;
                ViewBag.DocumentCode = doc_status;
                List<EntityNameList> _EntityName = new List<EntityNameList>();
                EntityNameList _EntityNameList = new EntityNameList();
                _EntityNameList.entity_name = DetailModel.EntityName;
                _EntityNameList.entity_id = DetailModel.EntityID;
                _EntityName.Add(_EntityNameList);
                DetailModel.EntityNameList = _EntityName;

                DetailModel.SourceType = ds.Tables[0].Rows[0]["src_type"].ToString().Trim();
                if (DetailModel.SourceType =="A")
                {
                    DetailModel.docNoLists = null;
                    DetailModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                    DetailModel.DocumentDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                    List<SrcDocNoList> srcDocNoLists1 = new List<SrcDocNoList>();
                    srcDocNoLists1.Add(new SrcDocNoList { SrcDocnoId = DetailModel.SrcDocNo, SrcDocnoVal = DetailModel.SrcDocNo });
                    DetailModel.docNoLists = srcDocNoLists1;
                    DetailModel.SrcDocNo = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                    //List<SrcDocNoList> _SrcDocNo = new List<SrcDocNoList>();
                    //SrcDocNoList _SrcDocNoList = new SrcDocNoList();
                    //_SrcDocNoList.SrcDocnoId = DetailModel.SrcDocNo;
                    //_SrcDocNoList.SrcDocnoVal = DetailModel.SrcDocNo;
                    //_SrcDocNo.Add(_SrcDocNoList);
                    //DetailModel.docNoLists = _SrcDocNo;

                    ViewBag.ItemOrderQtyDetail = ds.Tables[9];
                }
              


                if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                {
                    DetailModel.CancelledRemarks = ds.Tables[0].Rows[0]["cancel_remarks"].ToString();
                    DetailModel.CancelFlag = true;

                    DetailModel.BtnName = "Refresh";
                }
                else
                {
                    DetailModel.CancelFlag = false;
                }
                DetailModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[8]);
                DetailModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);

                if (doc_status != "D" && doc_status != "F")
                {
                    ViewBag.AppLevel = ds.Tables[8];
                }
                if (ViewBag.AppLevel != null && DetailModel.Command != "Edit")
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

                    if (doc_status == "D")
                    {
                        if (create_id != UserID)
                        {
                            DetailModel.BtnName = "Refresh";
                        }
                        else
                        {
                            if (nextLevel == "0")
                            {
                                if (create_id == UserID)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                }
                                DetailModel.BtnName = "BtnEdit";
                            }
                            else
                            {
                                ViewBag.Approve = "N";
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                DetailModel.BtnName = "BtnEdit";
                            }

                        }
                        if (UserID == sent_to)
                        {
                            ViewBag.ForwardEnbl = "Y";
                            /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                            if (TempData["Message1"] != null)
                            {
                                ViewBag.Message = TempData["Message1"];
                            }
                            DetailModel.BtnName = "BtnEdit";
                        }


                        if (nextLevel == "0")
                        {
                            if (sent_to == UserID)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                                /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                DetailModel.BtnName = "BtnEdit";
                            }


                        }
                    }
                    if (doc_status == "F")
                    {
                        if (UserID == sent_to)
                        {
                            ViewBag.ForwardEnbl = "Y";
                            /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                            if (TempData["Message1"] != null)
                            {
                                ViewBag.Message = TempData["Message1"];
                            }
                            /*End to chk Financial year exist or not*/
                            DetailModel.BtnName = "BtnEdit";
                        }
                        if (nextLevel == "0")
                        {
                            if (sent_to == UserID)
                            {
                                ViewBag.Approve = "Y";
                                ViewBag.ForwardEnbl = "N";
                                /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                            }
                            DetailModel.BtnName = "BtnEdit";
                        }
                    }
                    if (doc_status == "A")
                    {
                        if (create_id == UserID || approval_id == UserID)
                        {
                            DetailModel.BtnName = "BtnEdit";

                        }
                        else
                        {
                            DetailModel.BtnName = "Refresh";
                        }
                    }
                }
                if (ViewBag.AppLevel.Rows.Count == 0)
                {
                    ViewBag.Approve = "Y";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public ActionResult GetSupp_CustListDeatil(ExternalReceiptDeatilModel DetailModel)
        {
            try
            {
                CompDeatil();
                string EntityName = string.Empty;
                string EntityType = string.Empty;
                List<EntityNameList> _EntityName = new List<EntityNameList>();
                if (string.IsNullOrEmpty(DetailModel.EntityName))
                {
                    EntityName = "0";
                }
                else
                {
                    EntityName = DetailModel.EntityName.ToString();
                }
                if (string.IsNullOrEmpty(DetailModel.entity_type))
                {
                    EntityType = "0";
                }
                else
                {
                    EntityType = DetailModel.entity_type.ToString();
                }
                DataSet SuppCustList = _ExternalReceipt_ISERVICES.getSuppCustList(CompID, brnchID, EntityName, EntityType);
                if (EntityType == "0")
                {
                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)
                    {
                        EntityNameList _EntityNameList = new EntityNameList();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                else
                {
                    DataRow Drow = SuppCustList.Tables[0].NewRow();
                    Drow[0] = "0";
                    Drow[1] = "---Select---";
                    SuppCustList.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in SuppCustList.Tables[0].Rows)

                    {
                        EntityNameList _EntityNameList = new EntityNameList();
                        _EntityNameList.entity_name = dr["val"].ToString();
                        _EntityNameList.entity_id = dr["id"].ToString();
                        _EntityName.Add(_EntityNameList);
                    }
                }
                DetailModel.EntityNameList = _EntityName;
                return Json(_EntityName.Select(c => new { Name = c.entity_name, ID = c.entity_id }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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
                DataSet result = _ExternalReceipt_ISERVICES.GetWarehouseList(Comp_ID, Br_ID);
                dt = result.Tables[0];
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
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
                DataSet result = _ExternalReceipt_ISERVICES.GetWarehouseList(Comp_ID, Br_ID);
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
        private void SetUrlData(UrlModel _urlModel, ExternalReceiptDeatilModel _Model)
        {
         
            if (_urlModel.tp != null)
            {
                _Model.TransType = _urlModel.tp;
            }
            else
            {
                _Model.TransType = "Refresh";
            }
            if (_urlModel.bt != null)
            {
                _Model.BtnName = _urlModel.bt;
            }
            else
            {
                _Model.BtnName = "BtnRefresh";
            }
            if (_urlModel.Cmd != null)
            {
                _Model.Command = _urlModel.Cmd;
            }
            else
            {
                _Model.Command = "Save";
            }
            if (_urlModel.DMS != null)
            {
                _Model.DocumentStatus = _urlModel.DMS;
            }
            else
            {
                _Model.DocumentStatus = "";
            }
            if (_urlModel.recp_no != null && _urlModel.recpt_dt != "")
            {
                _Model.ReceiptNumber = _urlModel.recp_no;
                _Model.ReceiptDate = _urlModel.recpt_dt;
            }

        }
        private void CommonPageDetails()
        {
            try
            {

                CompDeatil();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, brnchID, UserID, DocumentMenuId, language);
                ViewBag.AppLevel = ds.Tables[0];
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
        public void CompDeatil()
        {

            if (Session["CompId"].ToString() != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"].ToString() != null)
            {
                brnchID = Session["BranchId"].ToString();
            }
            if (Session["UserId"].ToString() != null)
            {
                UserID = Session["UserId"].ToString();
            }
            if (Session["Language"].ToString() != null)
            {
                language = Session["Language"].ToString();
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
        }
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled,string Status,string recpt_no,string recpt_dt)
        {
            try
            {
                CompDeatil();
                DataTable dt = new DataTable();
                if (Status == "D" || Status == "F" || Status == "" || Status == null)
                {

                    dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        foreach (JObject item in arr.Children())//
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["Qty"] = item.GetValue("qty");
                            }
                        }
                    }
                }
                else
                {
                    dt = _ExternalReceipt_ISERVICES.GetSubItemDetails(CompID,brnchID,recpt_no,recpt_dt, Item_id).Tables[0];
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = "ExternalReceipt",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y",
                };
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
          
        }

        public ActionResult AgainstissueGetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, 
            string Status, string recpt_no, string recpt_dt, string srDocno, string srDocdt, string  flag)
        {
            try
            {
                CompDeatil();
                DataTable dt = new DataTable();
                if (Status == "D" || Status == "F" || Status == "" || Status == null)
                {
                  


                    dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    dt.Columns.Add("IssuedQty", typeof(string));
                    dt.Columns.Add("pend_qty", typeof(string));
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        bool isMatched = false;

                        foreach (JObject item in arr.Children())
                        {
                            if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() &&
                                item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                            {
                                dt.Rows[i]["IssuedQty"] = item.GetValue("IssueQty");
                                dt.Rows[i]["pend_qty"] = item.GetValue("PendingQty");
                                dt.Rows[i]["Qty"] = item.GetValue("qty");

                                isMatched = true;
                                break;   // 👍 Stop loop after match
                            }
                        }

                        if (!isMatched)
                        {
                            dt.Rows.RemoveAt(i); // 👍 Safe because reverse loop
                        }
                    }

                }
                else
                {
                    dt = _ExternalReceipt_ISERVICES.GetSubItemDetails(CompID, brnchID, recpt_no, recpt_dt, Item_id).Tables[0];
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = flag,                  
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    decimalAllowed = "Y",
                };
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }

        public ActionResult SaveAllDeatilExternalReceipt(ExternalReceiptDeatilModel DetailSaveModel, string command)
        {
            try
            {
                CompDeatil();
                var commCont = new CommonController(_Common_IServices);
                if (DetailSaveModel.DeleteCommand == "Delete")
                {
                    if (true)
                    {
                        command = "Delete";
                    }
                }
                switch (command)
                {
                    case "AddNew":
                        ExternalReceiptDeatilModel AddNewModel = new ExternalReceiptDeatilModel();
                        AddNewModel.Command = "AddNew";
                        AddNewModel.TransType = "Save";
                        AddNewModel.BtnName = "BtnAddNew";
                        TempData["ModelData"] = AddNewModel;
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "AddNew";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";

                        if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(DetailSaveModel.ReceiptNumber))
                            {
                                return RedirectToAction("DblClick", new { recpt_no = DetailSaveModel.ReceiptNumber, recpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                            }
                            else
                            {
                                NewModel.Cmd = "Refresh";
                                NewModel.tp = "Refresh";
                                NewModel.bt = "BtnRefresh";
                                NewModel.DMS = null;
                                TempData["ModelData"] = NewModel;
                                return RedirectToAction("ExternalReceiptDetail", NewModel);
                            }
                        }
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("ExternalReceiptDetail", NewModel);
                    case "Edit":
                        //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DblClick", new { rcpt_no = DetailSaveModel.ReceiptNumber, rcpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string rcptDt = DetailSaveModel.ReceiptDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, brnchID, rcptDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DblClick", new { recpt_no = DetailSaveModel.ReceiptNumber, recpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        }
                        UrlModel EditModel = new UrlModel();
                        EditModel.Cmd = command;
                        DetailSaveModel.Command = command;
                        DetailSaveModel.BtnName = "BtnEdit";
                        DetailSaveModel.TransType = "Update";
                        DetailSaveModel.ReceiptNumber = DetailSaveModel.ReceiptNumber;
                        DetailSaveModel.ReceiptDate = DetailSaveModel.ReceiptDate;
                        TempData["ModelData"] = DetailSaveModel;
                        EditModel.tp = "Update";
                        EditModel.bt = "BtnEdit";
                        EditModel.recp_no = DetailSaveModel.ReceiptNumber;
                        EditModel.recpt_dt = DetailSaveModel.ReceiptDate;
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("ExternalReceiptDetail", EditModel);
                    case "Save":
                        //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DblClick", new { recpt_no = DetailSaveModel.ReceiptNumber, recpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        //}
                        SaveUpdate(DetailSaveModel);
                        if (DetailSaveModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = DetailSaveModel;
                        UrlModel SaveModel = new UrlModel();
                        SaveModel.bt = DetailSaveModel.BtnName;
                        SaveModel.recp_no = DetailSaveModel.ReceiptNumber;
                        SaveModel.recpt_dt = DetailSaveModel.ReceiptDate;
                        SaveModel.tp = DetailSaveModel.TransType;
                        SaveModel.Cmd = DetailSaveModel.Command;
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("ExternalReceiptDetail", SaveModel);
                    case "Delete":
                        if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            return RedirectToAction("DblClick", new { recpt_no = DetailSaveModel.ReceiptNumber, recpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        }
                        Delete(DetailSaveModel, command);
                        ExternalReceiptDeatilModel DeleteModel = new ExternalReceiptDeatilModel();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnRefresh";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnRefresh";
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("ExternalReceiptDetail", Delete_Model);
                    case "Approve":
                        //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DblClick", new { rcpt_no = DetailSaveModel.ReceiptNumber, rcpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string rcptDt1 = DetailSaveModel.ReceiptDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, brnchID, rcptDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DblClick", new { recpt_no = DetailSaveModel.ReceiptNumber, recpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        }
                        ApproveData(DetailSaveModel, "", "");
                        TempData["ModelData"] = DetailSaveModel;
                        UrlModel Approve = new UrlModel();
                        Approve.tp = "Update";
                        Approve.recp_no = DetailSaveModel.ReceiptNumber;
                        Approve.recpt_dt = DetailSaveModel.ReceiptDate;
                        Approve.bt = "BtnEdit";
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("ExternalReceiptDetail", Approve);
                    case "Refresh":
                        ExternalReceiptDeatilModel RefreshModel = new ExternalReceiptDeatilModel();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel Refresh_Model = new UrlModel();
                        Refresh_Model.tp = "Save";
                        Refresh_Model.bt = "BtnRefresh";
                        Refresh_Model.Cmd = command;
                        TempData["ListFilterData"] = RefreshModel.ListFilterData1;
                        return RedirectToAction("ExternalReceiptDetail", Refresh_Model);
                    case "Forward":
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("DblClick", new { rcpt_no = DetailSaveModel.ReceiptNumber, rcpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string rcptDt2 = DetailSaveModel.ReceiptDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, brnchID, rcptDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("DblClick", new { recpt_no = DetailSaveModel.ReceiptNumber, recpt_dt = DetailSaveModel.ReceiptDate, ListFilterData = DetailSaveModel.ListFilterData1, WF_Status = DetailSaveModel.WFStatus });
                        }
                        return new EmptyResult();
                    case "Print":
                       // return new EmptyResult();
                        return GenratePdfFile(DetailSaveModel);

                    case "BacktoList":
                        var WF_status = DetailSaveModel.WF_Status1;
                        TempData["ListFilterData"] = DetailSaveModel.ListFilterData1;
                        return RedirectToAction("ExternalReceipt", new { WF_status });

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
        public FileResult GenratePdfFile(ExternalReceiptDeatilModel PrintData)
        {
            return File(GetPdfData(PrintData.ReceiptNumber, PrintData.ReceiptDate, PrintData.Status_Code), "application/pdf", "ExternalReceipt.pdf");
        }
        public byte[] GetPdfData(string Doc_No, string Doc_dt, string StatusCode)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;

            try
            {
                ExternalReceiptDeatilModel PrintData = new ExternalReceiptDeatilModel();
                CompDeatil();
                DataSet Details = _ExternalReceipt_ISERVICES.GetDataForPrint(CompID, brnchID, Doc_No, Convert.ToDateTime(Doc_dt).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                ViewBag.DocStatus = StatusCode.Trim();
                ViewBag.Title = "External Receipt Receipt";
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                ViewBag.DocumentMenuId = DocumentMenuId;
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/ExternalReceipt/ExternalReceiptPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 20f, 100f);
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
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, p, 580, 80, 0);
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
                throw ex;
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
        public ActionResult ApproveData(ExternalReceiptDeatilModel _model, string ListFilterData1, string WF_Status1)
        {
            try
            {
                CompDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string recp_no = _model.ReceiptNumber;
                string recp_dt = _model.ReceiptDate;
                string A_Status = _model.A_Status;
                string A_Level = _model.A_Level;
                string A_Remarks = _model.A_Remarks;
                string Message = _ExternalReceipt_ISERVICES.Approve_details(CompID, brnchID, DocumentMenuId, recp_no, recp_dt, UserID, mac_id, A_Status, A_Level, A_Remarks);

                if (Message == "Approved")
                {
                    //string fileName = "PC_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    ////var filePath = SavePdfDocToSendOnEmailAlert(Cnf_No, Cnf_Date, fileName);
                    //_Common_IServices.SendAlertEmail(CompID, brnchID, DocumentMenuId, Cnf_No, "AP", UserID, "0", filePath);

                    _model.Message = "Approved";
                }
                UrlModel ApproveModel = new UrlModel();
                _model.ReceiptNumber = recp_no;
                _model.ReceiptDate = recp_dt;
                _model.TransType = "Update";
                _model.BtnName = "BtnEdit";
                _model.Command = "Approve";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _model.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _model;

                ApproveModel.tp = "Update";
                ApproveModel.recp_no = _model.ReceiptNumber;
                ApproveModel.recpt_dt = _model.ReceiptDate;
                ApproveModel.bt = "BtnEdit";
                ApproveModel.Cmd = "Approve";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("ExternalReceiptDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private ActionResult Delete(ExternalReceiptDeatilModel _model, string command)
        {
            try
            {
                CompDeatil();
                string doc_no = _model.ReceiptNumber;
                DataSet Message = _ExternalReceipt_ISERVICES.DeleteData(CompID, brnchID, _model.ReceiptNumber, _model.ReceiptDate);
                if (!string.IsNullOrEmpty(doc_no))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + brnchID, PageName, doc_no1, Server);
                }
                return RedirectToAction("ExternalReceiptDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult SaveUpdate(ExternalReceiptDeatilModel _model)
        {
            string SaveMessage = "";
            CommonPageDetails();
            string PageName = title.Replace(" ", "");
            try
            {
                if (_model.CancelFlag == false)
                {
                    var commonContr = new CommonController();
                    DataTable HeaderData = new DataTable();
                    DataTable headerRow = new DataTable();
                    DataTable DataItemTable = new DataTable();
                    DataTable DataSubItemTable = new DataTable();
                    DataTable BatchItemTableData = new DataTable();
                    DataTable LotBatchSerialItemTableData = new DataTable();
                    DataTable SerialItemTableData = new DataTable();
                    DataTable Attachments = new DataTable();

                    headerRow.Columns.Add("TransType", typeof(string));
                    headerRow.Columns.Add("DocumentMenuId", typeof(string));
                    headerRow.Columns.Add("comp_id", typeof(string));
                    headerRow.Columns.Add("br_id", typeof(string));
                    headerRow.Columns.Add("recpt_no", typeof(string));
                    headerRow.Columns.Add("recpt_dt", typeof(string));
                    headerRow.Columns.Add("entity_type", typeof(string));
                    headerRow.Columns.Add("entity_id", typeof(string));
                    headerRow.Columns.Add("check_by", typeof(string));
                    headerRow.Columns.Add("recpt_status", typeof(string));
                    headerRow.Columns.Add("remarks", typeof(string));
                    headerRow.Columns.Add("create_id", typeof(string));
                    headerRow.Columns.Add("mac_id", typeof(string));
                    headerRow.Columns.Add("src_type", typeof(string));
                    headerRow.Columns.Add("src_doc_no", typeof(string));
                    headerRow.Columns.Add("src_doc_dt", typeof(string));

                    DataRow SetData_Header = headerRow.NewRow();
                    if (_model.ReceiptNumber != null)
                    {
                        SetData_Header["TransType"] = "Update";
                    }
                    else
                    {
                        SetData_Header["TransType"] = "Save";
                    }
                    SetData_Header["DocumentMenuId"] = DocumentMenuId;
                    SetData_Header["comp_id"] = CompID;
                    SetData_Header["br_id"] = brnchID;
                    SetData_Header["recpt_no"] = _model.ReceiptNumber;
                    SetData_Header["recpt_dt"] = _model.ReceiptDate;
                    SetData_Header["entity_type"] = _model.EntityTypeID;
                    SetData_Header["entity_id"] = _model.EntityID;
                    SetData_Header["check_by"] = _model.CheckedBy;
                    SetData_Header["recpt_status"] = "D";
                    SetData_Header["remarks"] = _model.remarks;
                    SetData_Header["create_id"] = UserID;

                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    SetData_Header["mac_id"] = SystemDetail;
                    SetData_Header["src_type"] = _model.SourceType;
                    SetData_Header["src_doc_no"] = _model.SrcDocNo;
                    SetData_Header["src_doc_dt"] = _model.DocumentDate;
                    headerRow.Rows.Add(SetData_Header);
                    HeaderData = headerRow;

                    DataTable itemtable = new DataTable();
                    itemtable.Columns.Add("item_id", typeof(string));
                    itemtable.Columns.Add("uom_id", typeof(string));
                    itemtable.Columns.Add("rec_qty", typeof(string));
                    itemtable.Columns.Add("price", typeof(string));
                    itemtable.Columns.Add("wh_id", typeof(string));
                    itemtable.Columns.Add("remarks", typeof(string));
                    itemtable.Columns.Add("issued_qty", typeof(string));
                    itemtable.Columns.Add("pending_qty", typeof(string));

                 
                    JArray oject = JArray.Parse(_model.ItemDeatilData);
                    for (int i = 0; i < oject.Count; i++)
                    {
                        DataRow itemrow = itemtable.NewRow();
                        itemrow["item_id"] = oject[i]["itemid"].ToString();
                        itemrow["uom_id"] = oject[i]["UOMID"].ToString();
                        itemrow["rec_qty"] = oject[i]["ReceivedQuantity"].ToString();
                        itemrow["price"] = oject[i]["CostPrice"].ToString();
                        itemrow["wh_id"] = oject[i]["wh_id"].ToString();
                        itemrow["issued_qty"] = oject[i]["issued_qty"].ToString();
                        itemrow["pending_qty"] = oject[i]["pending_qty"].ToString();
                        itemrow["remarks"] = oject[i]["ItemRemarks"].ToString();
                        itemtable.Rows.Add(itemrow);
                    }
                    DataItemTable = itemtable;

                    DataTable subitemtable = new DataTable();
                    subitemtable.Columns.Add("item_id", typeof(string));
                    subitemtable.Columns.Add("sub_item_id", typeof(string));
                    subitemtable.Columns.Add("rec_qty", typeof(string));
                    subitemtable.Columns.Add("issued_qty", typeof(string));
                    subitemtable.Columns.Add("pending_qty", typeof(string));

                  
                    JArray object1 = JArray.Parse(_model.SubItemDetailsDt);
                    for (int i = 0; i < object1.Count; i++)
                    {
                        DataRow subitemrow = subitemtable.NewRow();
                        subitemrow["item_id"] = object1[i]["item_id"].ToString();
                        subitemrow["sub_item_id"] = object1[i]["sub_item_id"].ToString();
                        subitemrow["rec_qty"] = object1[i]["qty"].ToString();
                        subitemrow["issued_qty"] = object1[i]["issued_qty"].ToString();
                        subitemrow["pending_qty"] = object1[i]["pending_qty"].ToString();

                        subitemtable.Rows.Add(subitemrow);
                    }
                    DataSubItemTable = subitemtable;


                    //if(_model.SourceType=="A")
                    //{
                        DataTable lotBatchserialitemtable = new DataTable();
                        lotBatchserialitemtable.Columns.Add("item_id", typeof(string));
                        lotBatchserialitemtable.Columns.Add("lot_no", typeof(string));
                        lotBatchserialitemtable.Columns.Add("batchnoserialno", typeof(string));
                    lotBatchserialitemtable.Columns.Add("exp_dt", typeof(string));
                    lotBatchserialitemtable.Columns.Add("issued_qty", typeof(string));
                    lotBatchserialitemtable.Columns.Add("pending_qty", typeof(string));
                    lotBatchserialitemtable.Columns.Add("recpt_qty", typeof(string));
                    lotBatchserialitemtable.Columns.Add("mfg_name", typeof(string));
                    lotBatchserialitemtable.Columns.Add("mfg_mrp", typeof(string));
                    lotBatchserialitemtable.Columns.Add("mfg_date", typeof(string));



                    if (_model.hdnSaveDataBatchSerialLotDeatil != null)
                    {
                        JArray objlot = JArray.Parse(_model.hdnSaveDataBatchSerialLotDeatil);
                        for (int i = 0; i < objlot.Count; i++)
                        {
                            DataRow lotBatchserialitemrow = lotBatchserialitemtable.NewRow();
                            lotBatchserialitemrow["item_id"] = objlot[i]["itemid"].ToString();
                            lotBatchserialitemrow["lot_no"] = objlot[i]["Lot_id"].ToString();
                            lotBatchserialitemrow["batchnoserialno"] = objlot[i]["BatchSerial"].ToString();
                            if (objlot[i]["BatchExDate"].ToString() == "" || objlot[i]["BatchExDate"].ToString() == null)
                            {
                                lotBatchserialitemrow["exp_dt"] = "01-Jan-1900";
                            }
                            else
                            {
                                lotBatchserialitemrow["exp_dt"] = objlot[i]["BatchExDate"].ToString();
                            }
                            lotBatchserialitemrow["issued_qty"] = objlot[i]["issued_qty"].ToString();
                            lotBatchserialitemrow["pending_qty"] = objlot[i]["pending_qty"].ToString();
                            lotBatchserialitemrow["recpt_qty"] = objlot[i]["rec_qty"].ToString();
                            lotBatchserialitemrow["mfg_name"] = commonContr.IsBlank(objlot[i]["mfg_name"].ToString(),null);
                            lotBatchserialitemrow["mfg_mrp"] = commonContr.IsBlank(objlot[i]["mfg_mrp"].ToString(),null);
                            lotBatchserialitemrow["mfg_date"] = commonContr.IsBlank(objlot[i]["mfg_date"].ToString(),null);



                            lotBatchserialitemtable.Rows.Add(lotBatchserialitemrow);
                        }
                    }
                        LotBatchSerialItemTableData = lotBatchserialitemtable;

                    //}
                    //else
                    //{
                        DataTable Batchitemtable = new DataTable();
                        Batchitemtable.Columns.Add("item_id", typeof(string));
                        Batchitemtable.Columns.Add("batch_no", typeof(string));
                        Batchitemtable.Columns.Add("recpt_qty", typeof(string));
                        Batchitemtable.Columns.Add("exp_dt", typeof(string));
                    Batchitemtable.Columns.Add("mfg_name", typeof(string));
                    Batchitemtable.Columns.Add("mfg_mrp", typeof(string));
                    Batchitemtable.Columns.Add("mfg_date", typeof(string));

                    if (_model.BatchItemDeatilData != null)
                    {
                        JArray obj = JArray.Parse(_model.BatchItemDeatilData);
                        for (int i = 0; i < obj.Count; i++)
                        {
                            DataRow Batchitemrow = Batchitemtable.NewRow();
                            Batchitemrow["item_id"] = obj[i]["itemid"].ToString();
                            Batchitemrow["batch_no"] = obj[i]["BatchNo"].ToString();
                            Batchitemrow["recpt_qty"] = obj[i]["BatchQty"].ToString();

                            if (obj[i]["BatchExDate"].ToString() == "" || obj[i]["BatchExDate"].ToString() == null)
                            {
                                Batchitemrow["exp_dt"] = "01-Jan-1900";
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
                      
                        BatchItemTableData = Batchitemtable;
                  
                        DataTable Serialitemtable = new DataTable();
                        Serialitemtable.Columns.Add("item_id", typeof(string));
                        Serialitemtable.Columns.Add("serial_no", typeof(string));
                        Serialitemtable.Columns.Add("mfg_name", typeof(string));
                        Serialitemtable.Columns.Add("mfg_mrp", typeof(string));
                        Serialitemtable.Columns.Add("mfg_date", typeof(string));

                    if (_model.SerialItemDeatilData != null)
                    {
                        JArray obj1 = JArray.Parse(_model.SerialItemDeatilData);
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
                   
                        SerialItemTableData = Serialitemtable;
                   // }

                  


                    DataTable dtAttachment = new DataTable();
                    var _GatePassattch = TempData["ModelDataattch"] as GatePassattchment;
                    TempData["ModelDataattch"] = null;
                    if (_model.attatchmentdetail != null)
                    {
                        if (_GatePassattch != null)
                        {
                            if (_GatePassattch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _GatePassattch.AttachMentDetailItmStp as DataTable;
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
                                if (!string.IsNullOrEmpty(_model.ReceiptNumber))
                                {
                                    dtrowAttachment1["id"] = _model.ReceiptNumber;
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
                        if (_model.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_model.ReceiptNumber))
                                {
                                    ItmCode = _model.ReceiptNumber;
                                }
                                else
                                {
                                    ItmCode = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + brnchID + ItmCode.Replace("/", "") + "*");
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
                        Attachments = dtAttachment;
                    }

                    SaveMessage = _ExternalReceipt_ISERVICES.InsertUpdateData(HeaderData, DataItemTable, Attachments, DataSubItemTable, BatchItemTableData, SerialItemTableData, LotBatchSerialItemTableData);
                    string[] Data = SaveMessage.Split(',');

                    string recpt_no = Data[1];
                    string Message = Data[0];
                    string recpt_dt = Data[2];
                    if (Message == "DataNotFound")
                    {

                        var a = recpt_no.Split(',');
                        var msg = "Data Not found" + " " + a[0].Trim() + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _model.Message = Message;
                        return RedirectToAction("ExternalReceiptDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_GatePassattch != null)
                        {
                            if (_GatePassattch.Guid != null)
                            {
                                Guid = _GatePassattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, brnchID, guid, PageName, recpt_no, _model.TransType, Attachments);
                    }
                    if (Message == "Update" || Message == "Save")
                    {
                        _model.Message = "Save";
                        _model.Command = "Update";
                        _model.ReceiptNumber = recpt_no;
                        _model.ReceiptDate = recpt_dt;
                        _model.TransType = "Update";
                        _model.BtnName = "BtnEdit";
                        _model.AttachMentDetailItmStp = null;
                        _model.Guid = null;
                    }
                    return RedirectToAction("ExternalReceiptDetail");
                }
                else
                {
                    CompDeatil();
                    string br_id = brnchID;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet message = _ExternalReceipt_ISERVICES.CancelData(CompID, br_id, _model.ReceiptNumber, _model.ReceiptDate, UserID, DocumentMenuId, mac_id, _model.CancelledRemarks);

                    _model.Message = message.Tables[0].Rows[0]["result"].ToString();
                    _model.ReceiptNumber = _model.ReceiptNumber;
                    _model.ReceiptDate = _model.ReceiptDate;
                    _model.TransType = "Update";
                    _model.BtnName = "Refresh";

                    string fileName = "FGR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    // var filePath = SavePdfDocToSendOnEmailAlert(_model.RecieptDate, _model.RecieptDate, fileName);
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _model.ReceiptNumber, "C", UserID, "0" /*filePath*/);

                    return RedirectToAction("ExternalReceiptDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {

                    if (_model.TransType == "Save")
                    {
                        string Guid = "";

                        if (_model.Guid != null)
                        {

                            Guid = _model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + brnchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public ActionResult DblClick(string recpt_no, string recpt_dt, string ListFilterData, string WF_Status)
        {
            try
            {
                CompDeatil();
                //var commCont = new CommonController(_Common_IServices);
                //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                //{
                //    TempData["Message1"] = "Financial Year not Exist";
                //}
                ExternalReceiptDeatilModel dblclick = new ExternalReceiptDeatilModel();
                UrlModel _url = new UrlModel();
                dblclick.ReceiptNumber = recpt_no;
                dblclick.ReceiptDate = recpt_dt;
                dblclick.TransType = "Update";
                dblclick.BtnName = "BtnEdit";
                dblclick.Command = "Refresh";
                if (WF_Status != null && WF_Status != "")
                {
                    _url.wf = WF_Status;
                    dblclick.WF_Status1 = WF_Status;
                }
                TempData["ModelData"] = dblclick;
                _url.tp = "Update";
                _url.bt = "BtnEdit";
                _url.recp_no = recpt_no;
                _url.recpt_dt = recpt_dt;
                _url.Cmd = "Refresh";
                TempData["ListFilterData"] = ListFilterData;
                return RedirectToAction("ExternalReceiptDetail", _url);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                CompDeatil();
                CommonPageDetails();
                GatePassattchment _GatePassattch = new GatePassattchment();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                Guid gid = new Guid();
                gid = Guid.NewGuid();

                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");

                _GatePassattch.Guid = DocNo;
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + brnchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _GatePassattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _GatePassattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _GatePassattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
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
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData)
        {
            ExternalReceiptDeatilModel ToRefreshByJS = new ExternalReceiptDeatilModel();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.ReceiptNumber = a[0].Trim();
            ToRefreshByJS.ReceiptDate = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnEdit";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wf = a[2].Trim();
            }
            Model.bt = "BtnEdit";
            Model.recp_no = ToRefreshByJS.ReceiptNumber;
            Model.recpt_dt = ToRefreshByJS.ReceiptDate;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ExternalReceiptDetail", Model);
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status1)
        {

            ExternalReceiptDeatilModel _model = new ExternalReceiptDeatilModel();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _model.ReceiptNumber = jObjectBatch[i]["DocNo"].ToString();
                    _model.ReceiptDate = jObjectBatch[i]["DocDate"].ToString();
                    _model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_model.A_Status != "Approve")
            {
                _model.A_Status = "Approve";
            }
            ApproveData(_model, ListFilterData1, WF_Status1);
            UrlModel ApproveModel = new UrlModel();
            if (WF_Status1 != null && WF_Status1 != "")
            {
                ApproveModel.wf = WF_Status1;
            }
            TempData["ModelData"] = _model;
            ApproveModel.tp = "Update";
            ApproveModel.recp_no = _model.ReceiptNumber;
            ApproveModel.recpt_dt = _model.ReceiptDate;
            ApproveModel.bt = "BtnEdit";
            return RedirectToAction("ExternalReceiptDetail", ApproveModel);
        }

        public JsonResult GetSourceDocList(string Itm_ID, string SuppID, string entity_type, string sr_number)
        {
            JsonResult DataRows = null;
            try
            {
                CompDeatil();
                DataSet Deatils = _ExternalReceipt_ISERVICES.GetSourceDocList(CompID, brnchID, SuppID, entity_type);

                DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
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
        public ActionResult BindItemTable(string entity_type, string entity_Name, string Doc_no, string Doc_dt, string hdn_Status, string rcpt_no, string ddlGatePassDate)
        {
            try
            {
                JsonResult DataRows = null;
                CompDeatil();
                DataSet Deatils = _ExternalReceipt_ISERVICES.GetItemDeatilData(CompID, brnchID, entity_Name, entity_type, Doc_no, Doc_dt, rcpt_no);
                DataRows = Json(JsonConvert.SerializeObject(Deatils));
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
        public ActionResult getItemOrderQuantityDetail(string ItemID, string Status, string SelectedItemdetail, string TransType, string Command, string docid, string flag)
        {
            try
            {

                DataTable DTableOrderQty = new DataTable();
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    DataTable dtorderqty = new DataTable();
                    dtorderqty.Columns.Add("ItemID", typeof(string));
                    dtorderqty.Columns.Add("UomID", typeof(string));
                    dtorderqty.Columns.Add("LotID", typeof(string));
                    dtorderqty.Columns.Add("BatchSerial", typeof(string));
                    dtorderqty.Columns.Add("Exp_date", typeof(string));
                    dtorderqty.Columns.Add("Expdate", typeof(string));
                    //dtorderqty.Columns.Add("UomID", typeof(string));
                    dtorderqty.Columns.Add("IssueQty", typeof(string));
                    dtorderqty.Columns.Add("PendingQty", typeof(string));
                    dtorderqty.Columns.Add("RecQty", typeof(string));
                    dtorderqty.Columns.Add("mfg_name", typeof(string));
                    dtorderqty.Columns.Add("mfg_mrp", typeof(string));
                    dtorderqty.Columns.Add("mfg_date", typeof(string));
                    dtorderqty.Columns.Add("mfg_date_hdn", typeof(string));

                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);

                    foreach (JObject item in jObjectBatch.Children())
                    {
                        if (item.GetValue("ItemID").ToString() == ItemID.ToString())
                        {
                            DataRow dtorderqtyrow = dtorderqty.NewRow();
                            dtorderqtyrow["ItemID"] = item.GetValue("ItemID").ToString();
                            dtorderqtyrow["UomID"] = item.GetValue("UomID").ToString();
                            dtorderqtyrow["LotID"] = item.GetValue("LotID").ToString();
                            dtorderqtyrow["BatchSerial"] = item.GetValue("BatchSerial").ToString();
                            dtorderqtyrow["Exp_date"] = item.GetValue("Exp_date").ToString();
                            dtorderqtyrow["Expdate"] = item.GetValue("Expdate").ToString();
                            //dtorderqtyrow["UomID"] = item.GetValue("UomID").ToString();
                            dtorderqtyrow["IssueQty"] = item.GetValue("IssueQty").ToString(); 
                            dtorderqtyrow["PendingQty"] = item.GetValue("PendingQty").ToString();
                            dtorderqtyrow["RecQty"] = item.GetValue("RecQty").ToString();
                            dtorderqtyrow["mfg_name"] = item.GetValue("MfgName").ToString();
                            dtorderqtyrow["mfg_mrp"] = item.GetValue("MfgMrp").ToString();
                            dtorderqtyrow["mfg_date"] = item.GetValue("MfgDate").ToString();
                            dtorderqtyrow["mfg_date_hdn"] = item.GetValue("MfgDateHdn").ToString();

                            dtorderqty.Rows.Add(dtorderqtyrow);
                        }
                    }
                    DTableOrderQty = dtorderqty;
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = docid;
                ViewBag.Command = Command;
                ViewBag.TransType = TransType;
                ViewBag.flag = flag;
                if (DTableOrderQty.Rows.Count > 0)
                {
                    ViewBag.ItemOrderQtyDetail = DTableOrderQty;
                }
          
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_PartialExternelRecieptRecQtyDetail.cshtml");

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }

}