using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.Assembly_Kit;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.Assembly_Kit;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.MODELS.Common;
using System.Web;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Linq;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.AssemblyKit
{
    public class AssemblyKitController : Controller
    {
        string CompID, language, BranchId, UserID = String.Empty;
        string DocumentMenuId = "105102160", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AssemblyKit_ISERVICES _AssKit_ISERVICES;
        public AssemblyKitController(Common_IServices _Common_IServices, AssemblyKit_ISERVICES _AssKit_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._AssKit_ISERVICES = _AssKit_ISERVICES;
        }
        // GET: ApplicationLayer/AssemblyKit
        public ActionResult AssemblyKit(string WF_status)
        {
            AssemblyKit_ListModel listModel = new AssemblyKit_ListModel();
            GetCompDeatil();
            CommonPageDetails();
            listModel.WF_Status = WF_status;

            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;

            GetItemList(listModel);
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var PRData = TempData["ListFilterData"].ToString();
                var a = PRData.Split(',');             
               
                var Fromdate = a[0].Trim();
                var Todate = a[1].Trim();
                var Status = a[2].Trim();
                var AssemblyProduct = a[3].Trim();
                if (Status == "0")
                {
                    Status = null;
                }


                listModel.FromDate = Fromdate;
                listModel.ListFilterData = TempData["ListFilterData"].ToString();
              
                DataTable dt = _AssKit_ISERVICES.SearchDataFilter(AssemblyProduct, Fromdate, Todate, Status, CompID, BranchId, DocumentMenuId,UserID,"");
                ViewBag.AssemblyKitList = dt;

                listModel.AssemblyProduct = AssemblyProduct;
                listModel.FromDate = Fromdate;
                listModel.ToDate = Todate;
                listModel.Status = Status;              
            }
            else
            {
                DataTable GpassList = new DataTable();
                listModel.FromDate = startDate;
                listModel.ToDate = CurrentDate;
                GpassList = GetListDeatil(listModel);
               
                ViewBag.AssemblyKitList = GpassList;
            }
            listModel.Title = title;
            listModel.DocumentMenuId = DocumentMenuId;
            ViewBag.DocumentMenuId = DocumentMenuId;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/AssemblyKit/AssemblyKitList.cshtml", listModel);
        }

      
        public ActionResult DashBordtoList(string docid, string status)
        {
            var WF_status = status;
            return RedirectToAction("AssemblyKit", new { WF_status });
        }

        private DataTable GetListDeatil(AssemblyKit_ListModel _listModel)
        {
            GetCompDeatil();
            string wfstatus = "";

            if (_listModel.WF_Status != null)
            {
                wfstatus = _listModel.WF_Status;
            }
            else
            {
                wfstatus = "";
            }

            DataTable dt = _AssKit_ISERVICES.SearchDataFilter("0", _listModel.FromDate, _listModel.ToDate, "", CompID, BranchId, DocumentMenuId, UserID, wfstatus);
            return dt;
        }
        public void GetCompDeatil()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BranchId = Session["BranchId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["UserId"] != null)
            {
                UserID = Session["UserId"].ToString();
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
        }
        private void CommonPageDetails()
        {
            try
            {

                GetCompDeatil();
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BranchId, UserID, DocumentMenuId, language);
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
        public ActionResult GetItemList(AssemblyKit_ListModel queryParameters)
        {
            GetCompDeatil();
            DataSet itemList1 = new DataSet();
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.AssemblyProduct))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.AssemblyProduct;
                }
                itemList1 = _AssKit_ISERVICES.BindItemList(ItemName, CompID, BranchId);

                List<ItemName_List> _ItemList = new List<ItemName_List>();
                foreach (DataRow data in itemList1.Tables[0].Rows)
                {
                    ItemName_List _ItemDetail = new ItemName_List();
                    _ItemDetail.Item_ID = data["Item_id"].ToString();
                    _ItemDetail.Item_Name = data["Item_name"].ToString();
                    _ItemList.Add(_ItemDetail);
                }
                queryParameters.ItemNameList = _ItemList;

                List<Status> list2 = new List<Status>();
                foreach (var dr in ViewBag.StatusList.Rows)
                {
                    Status Status = new Status();
                    Status.status_id = dr["status_code"].ToString();
                    Status.status_name = dr["status_name"].ToString();
                    list2.Add(Status);
                }

                queryParameters.StatusList = list2;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(JsonConvert.SerializeObject(itemList1.Tables[0]), JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddAssemblyKitDetail()
        {

            GetCompDeatil();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
            {
                AssemblyKit_ListModel ListModel = new AssemblyKit_ListModel();
                ListModel.Message = "FY not Exist";
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("AssemblyKit", ListModel);
            }
            else
            {
                AssemblyKit_DetailModel AddNew_Model = new AssemblyKit_DetailModel();
                AddNew_Model.Message = null;
                AddNew_Model.TransType = "Save";
                AddNew_Model.Command = "AddNew";
                AddNew_Model.BtnName = "BtnAddNew";
                TempData["ModelData"] = AddNew_Model;
                UrlModel NewModel = new UrlModel();
                NewModel.Cmd = "AddNew";
                NewModel.trnstyp = "Save";
                NewModel.btn = "BtnAddNew";
                return RedirectToAction("AssemblyKitDetail", NewModel);
            }
        }
        public ActionResult AssemblyKitDetail(UrlModel _urlModel)
        {
            try
            {
                GetCompDeatil();
                CommonPageDetails();
                /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BranchId, _urlModel.Doc_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var DetailModel = TempData["ModelData"] as AssemblyKit_DetailModel;
                if (DetailModel != null)
                {
                    SetData(DetailModel);
                    DetailModel.batch_Command = DetailModel.Command;
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/AssemblyKit/AssemblyKitDetail.cshtml", DetailModel);
                }
                else
                {
                    AssemblyKit_DetailModel detailModel1 = new AssemblyKit_DetailModel();
                    SetUrlData(_urlModel, detailModel1);
                    SetData(detailModel1);
                    detailModel1.batch_Command = detailModel1.Command;
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/AssemblyKit/AssemblyKitDetail.cshtml", detailModel1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private void SetData(AssemblyKit_DetailModel DetailModel)
        {
            try
            {
                GetItemList_Deatil(DetailModel);
                if (DetailModel.DocumentDate == null)
                {
                    DetailModel.DocumentDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                }
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    DetailModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                }
                if (DetailModel.TransType == "Update")
                {
                    SetAllDataInView(DetailModel);
                }
                else
                {
                    DetailModel.batch_Command = DetailModel.Command;
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
        public void SetAllDataInView(AssemblyKit_DetailModel _DeatilModel)
        {
            try
            {
              
                GetCompDeatil();
                string Doc_no = _DeatilModel.DocumentNumber;
                string Doc_dt = _DeatilModel.DocumentDate;
                DataSet ds = new DataSet();
                ds = _AssKit_ISERVICES.GetDeatilData(CompID, BranchId, Doc_no, Doc_dt, UserID, DocumentMenuId);
                ViewBag.AttechmentDetails = ds.Tables[3];
                ViewBag.ItemDetailData = ds.Tables[1];
                ViewBag.SubItemDetails = ds.Tables[2];
                ViewBag.ItemStockBatchWise = ds.Tables[7];
                ViewBag.ItemStockSerialWise = ds.Tables[8];
                _DeatilModel.DocumentNumber = ds.Tables[0].Rows[0]["doc_no"].ToString();
                _DeatilModel.DocumentDate = ds.Tables[0].Rows[0]["Doc_dt"].ToString();
                _DeatilModel.AssemblyProduct = ds.Tables[0].Rows[0]["item_name"].ToString();
                GetItemList_Deatil(_DeatilModel);
                _DeatilModel.hdnAssemblyProductID = ds.Tables[0].Rows[0]["prod_id"].ToString();
                _DeatilModel.WHIDHeader = ds.Tables[0].Rows[0]["wh_id"].ToString();
                _DeatilModel.Warehouse = ds.Tables[0].Rows[0]["wh_id"].ToString();
                _DeatilModel.remarks = ds.Tables[0].Rows[0]["remark"].ToString();
                _DeatilModel.AssemblyQuantity = ds.Tables[0].Rows[0]["ass_qty"].ToString();
                _DeatilModel.AssemblyUOMID = ds.Tables[0].Rows[0]["uom"].ToString();
                _DeatilModel.AssemblyUOM = ds.Tables[0].Rows[0]["uom_alias"].ToString();
                _DeatilModel.Created_by = ds.Tables[0].Rows[0]["createname"].ToString();
                _DeatilModel.Created_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                _DeatilModel.Approved_by = ds.Tables[0].Rows[0]["appname"].ToString();
                _DeatilModel.Approved_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                _DeatilModel.Amended_by = ds.Tables[0].Rows[0]["modname"].ToString();
                _DeatilModel.Amended_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                _DeatilModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                _DeatilModel.Status_Code = ds.Tables[0].Rows[0]["status"].ToString().Trim();
                _DeatilModel.StatusName = ds.Tables[0].Rows[0]["status_name"].ToString();

                string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                _DeatilModel.StatusCode = doc_status;
                _DeatilModel.doc_status = doc_status;
                _DeatilModel.DocumentStatus = doc_status;
                ViewBag.DocumentCode = doc_status;
                if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                {
                    _DeatilModel.CancelFlag = true;

                    _DeatilModel.BtnName = "Refresh";
                }
                else
                {
                    _DeatilModel.CancelFlag = false;
                }
                _DeatilModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                _DeatilModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);

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
                        if (create_id != UserID)
                        {
                            _DeatilModel.BtnName = "Refresh";
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
                                /*End to chk Financial year exist or not*/
                                _DeatilModel.BtnName = "BtnEdit";
                            }

                        }
                        if (UserID == sent_to)
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
                                _DeatilModel.BtnName = "BtnEdit";
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
                            _DeatilModel.BtnName = "BtnEdit";
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
                            _DeatilModel.BtnName = "BtnEdit";
                        }
                    }
                    if (doc_status == "A")
                    {
                        if (create_id == UserID || approval_id == UserID)
                        {
                            _DeatilModel.BtnName = "BtnEdit";

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
                _DeatilModel.batch_Command = _DeatilModel.Command;
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
        private void SetUrlData(UrlModel _urlModel, AssemblyKit_DetailModel _Model)
        {
            try
            {
                if (_urlModel.trnstyp != null)
                {
                    _Model.TransType = _urlModel.trnstyp;
                }
                else
                {
                    _Model.TransType = "Refresh";
                }
                if (_urlModel.btn != null)
                {
                    _Model.BtnName = _urlModel.btn;
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
                if (_urlModel.Doc_no != null && _urlModel.Doc_dt != "")
                {
                    _Model.DocumentNumber = _urlModel.Doc_no;
                    _Model.DocumentDate = _urlModel.Doc_dt;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public ActionResult GetItemList_Deatil(AssemblyKit_DetailModel queryParameters)    
        {
            GetCompDeatil();
            DataSet itemList1 = new DataSet();
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.AssemblyProduct))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.AssemblyProduct;
                }
                itemList1 = _AssKit_ISERVICES.BindItemList(ItemName, CompID, BranchId);

                List<ItemName_List1> _ItemList = new List<ItemName_List1>();
                List<Warehouse> _WarehouseList = new List<Warehouse>();
                foreach (DataRow data in itemList1.Tables[0].Rows)
                {
                    ItemName_List1 _ItemDetail = new ItemName_List1();
                    _ItemDetail.Item_ID = data["Item_id"].ToString();
                    _ItemDetail.Item_Name = data["Item_name"].ToString();
                    _ItemList.Add(_ItemDetail);
                }
                queryParameters.ItemNameList = _ItemList;

                //DataSet result = _Common_IServices.GetWarehouseList(Comp_ID, Br_ID);
                foreach (DataRow dr in itemList1.Tables[1].Rows)
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
                queryParameters.WarehouseList = _WarehouseList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(JsonConvert.SerializeObject(itemList1.Tables[0]), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetWaherHouseList()
        {
            GetCompDeatil();
            DataSet itemList1 = new DataSet();
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {

                List<Warehouse> _WarehouseList = new List<Warehouse>();

                itemList1 = _AssKit_ISERVICES.BindItemList(ItemName, CompID, BranchId);

                foreach (DataRow dr in itemList1.Tables[1].Rows)
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
                //queryParameters.WarehouseList = _WarehouseList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(JsonConvert.SerializeObject(itemList1.Tables[1]), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemStockBatchWise(string ItemId, string Wh_ID, string DocStatus, string SelectedItemdetail
  , string Transtype, string cmd, string uom_id)
        {
            try
            {
                DataSet ds = new DataSet();
                GetCompDeatil();
                if (ItemId != "")
                {
                    ds = _AssKit_ISERVICES.GetItemStockBatchWise(CompID, BranchId, ItemId, Wh_ID, uom_id);
                }

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString() == ds.Tables[0].Rows[i]["item_id"].ToString() && item.GetValue("LotNo").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("BatchNo").ToString() == ds.Tables[0].Rows[i]["batch_no"].ToString())
                            {
                                ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty");
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockBatchWise = ds.Tables[0];
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentCode = DocStatus;
                ViewBag.Transtype = Transtype;
                ViewBag.Command = cmd;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult getItemstockSerialWise(string ItemId, string WhID, string DocStatus, string SelectedItemSerial, string Transtype, string cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                GetCompDeatil();
                ds = _AssKit_ISERVICES.getItemstockSerialWise(CompID, BranchId, ItemId, WhID);

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
                ViewBag.Command = cmd;
                ViewBag.DocumentCode = DocStatus;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemstockSerialWiseAfterStockUpadte(string Doc_no, string Doc_dt, string ItemID, string Transtype, string cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                GetCompDeatil();
                ds = _AssKit_ISERVICES.getItemstockSerialWiseAfterStockUpdate(CompID, BranchId, Doc_no, Doc_dt, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                ViewBag.Transtype = Transtype;
                ViewBag.Command = cmd;
                ViewBag.DocID = DocumentMenuId;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetSubItemDetails(string Item_id, string Uom_id, string WhID, string SubItemListwithPageData, string IsDisabled
         , string Flag, string Status, string Doc_no, string Doc_dt)
        {
            GetCompDeatil();
            DataTable dt = new DataTable();
            if (Status == "D" || Status == "F" || Status == "")
            {
                Flag = "AssemblyKitINput";
                dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BranchId, WhID, Item_id, null/*UomId*/, "wh").Tables[0];
                dt.Columns.Add("Qty", typeof(string));

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
                Flag = "AssemblyKitINput";
                   dt = _AssKit_ISERVICES.GetDeatilSubitm(CompID, BranchId, Item_id, Uom_id, WhID, Doc_no, Doc_dt, Flag).Tables[0];

            }
            SubItemPopupDt subitmModel = new SubItemPopupDt
            {
                Flag = Flag,
                dt_SubItemDetails = dt,
                IsDisabled = IsDisabled,
            };
            return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAssemblyKit(AssemblyKit_DetailModel SaveDeatilModel, string command)
        {
            try
            {
                GetCompDeatil();
                var commCont = new CommonController(_Common_IServices);

                //if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                //{
                //    UrlModel CheckFinYear = new UrlModel();
                //    TempData["Message"] = "Financial Year not Exist";
                //    if (!string.IsNullOrEmpty(SaveDeatilModel.DocumentNumber))

                //        return RedirectToAction("DblClick", new { Doc_no = SaveDeatilModel.DocumentNumber, Doc_dt = SaveDeatilModel.DocumentDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                //    else
                //        CheckFinYear.Cmd = "Refresh";
                //    CheckFinYear.trnstyp = "Refresh";
                //    CheckFinYear.btn = "BtnRefresh";
                //    CheckFinYear.DMS = null;
                //    return RedirectToAction("AssemblyKitDetail", CheckFinYear);
                //}
                //else
                //{
                    if (SaveDeatilModel.DeleteCommand == "Delete")
                        if (true)
                        {
                            command = "Delete";
                        }
                    switch (command)
                    {
                        case "AddNew":
                            AssemblyKit_DetailModel AddNewModel = new AssemblyKit_DetailModel();
                            AddNewModel.Command = "AddNew";
                            AddNewModel.TransType = "Save";
                            AddNewModel.BtnName = "BtnAddNew";
                            TempData["ModelData"] = AddNewModel;
                            UrlModel NewModel = new UrlModel();
                            NewModel.Cmd = "AddNew";
                            NewModel.trnstyp = "Save";
                            NewModel.btn = "BtnAddNew";
                            TempData["ListFilterData"] = null;
                        if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                        {
                            UrlModel CheckFinYear = new UrlModel();
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(SaveDeatilModel.DocumentNumber))

                                return RedirectToAction("DblClick", new { Doc_no = SaveDeatilModel.DocumentNumber, Doc_dt = SaveDeatilModel.DocumentDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                            else
                                CheckFinYear.Cmd = "Refresh";
                            CheckFinYear.trnstyp = "Refresh";
                            CheckFinYear.btn = "BtnRefresh";
                            CheckFinYear.DMS = null;
                            return RedirectToAction("AssemblyKitDetail", CheckFinYear);
                        }
                        return RedirectToAction("AssemblyKitDetail", NewModel);
                        case "Save":
                            SaveUpdateData(SaveDeatilModel);
                            if (SaveDeatilModel.Message == "DataNotFound")
                            {
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            TempData["ModelData"] = SaveDeatilModel;
                            UrlModel SaveModel = new UrlModel();
                            SaveModel.btn = SaveDeatilModel.BtnName;
                            SaveModel.Doc_no = SaveDeatilModel.DocumentNumber;
                            SaveModel.Doc_dt = SaveDeatilModel.DocumentDate;
                            SaveModel.trnstyp = SaveDeatilModel.TransType;
                            SaveModel.Cmd = SaveDeatilModel.Command;
                            TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                            return RedirectToAction("AssemblyKitDetail", SaveModel);
                        case "Edit":
                            /*start Add by Hina on 14-05-2025 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                BranchId = Session["BranchId"].ToString();
                            /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                            string kitdt = SaveDeatilModel.DocumentDate;
                            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BranchId, kitdt) == "TransNotAllow")
                            {
                                TempData["Message1"] = "TransNotAllow";
                                return RedirectToAction("DblClick", new { Doc_no = SaveDeatilModel.DocumentNumber, Doc_dt = SaveDeatilModel.DocumentDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                            }
                            /*End to chk Financial year exist or not*/
                            UrlModel EditModel = new UrlModel();
                            EditModel.Cmd = command;
                            SaveDeatilModel.Command = command;
                            SaveDeatilModel.BtnName = "BtnEdit";
                            SaveDeatilModel.TransType = "Update";
                            SaveDeatilModel.DocumentNumber = SaveDeatilModel.DocumentNumber;
                            SaveDeatilModel.DocumentDate = SaveDeatilModel.DocumentDate;
                            TempData["ModelData"] = SaveDeatilModel;
                            EditModel.trnstyp = "Update";
                            EditModel.btn = "BtnEdit";
                            EditModel.trnstyp = SaveDeatilModel.TransType;
                            EditModel.Doc_no = SaveDeatilModel.DocumentNumber;
                            EditModel.Doc_dt = SaveDeatilModel.DocumentDate;
                            TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                            return RedirectToAction("AssemblyKitDetail", EditModel);
                        case "Delete":
                            Delete(SaveDeatilModel, command);
                            AssemblyKit_DetailModel DeleteModel = new AssemblyKit_DetailModel();
                            DeleteModel.Message = "Deleted";
                            DeleteModel.Command = "Refresh";
                            DeleteModel.TransType = "Refresh";
                            DeleteModel.BtnName = "BtnRefresh";
                            TempData["ModelData"] = DeleteModel;
                            UrlModel Delete_Model = new UrlModel();
                            Delete_Model.Cmd = DeleteModel.Command;
                            Delete_Model.trnstyp = "Refresh";
                            Delete_Model.btn = "BtnRefresh";
                            TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                            return RedirectToAction("AssemblyKitDetail", Delete_Model);
                        case "Refresh":
                            AssemblyKit_DetailModel RefreshModel = new AssemblyKit_DetailModel();
                            RefreshModel.Command = command;
                            RefreshModel.BtnName = "BtnRefresh";
                            RefreshModel.TransType = "Save";
                            TempData["ModelData"] = RefreshModel;
                            UrlModel Refresh_Model = new UrlModel();
                            Refresh_Model.trnstyp = "Save";
                            Refresh_Model.btn = "BtnRefresh";
                            Refresh_Model.Cmd = command;
                            TempData["ListFilterData"] = RefreshModel.ListFilterData1;
                            return RedirectToAction("AssemblyKitDetail", Refresh_Model);
                        case "Approve":
                            /*start Add by Hina on 14-05-2025 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                BranchId = Session["BranchId"].ToString();
                            /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                            string kitdt1 = SaveDeatilModel.DocumentDate;
                            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BranchId, kitdt1) == "TransNotAllow")
                            {
                                TempData["Message1"] = "TransNotAllow";
                                return RedirectToAction("DblClick", new { Doc_no = SaveDeatilModel.DocumentNumber, Doc_dt = SaveDeatilModel.DocumentDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                            }
                            /*End to chk Financial year exist or not*/
                            ApproveData(SaveDeatilModel, "", "");
                            TempData["ModelData"] = SaveDeatilModel;
                            UrlModel Approve = new UrlModel();
                            Approve.trnstyp = "Update";
                            Approve.Doc_no = SaveDeatilModel.DocumentNumber;
                            Approve.Doc_dt = SaveDeatilModel.DocumentDate;
                            Approve.btn = "BtnEdit";
                            TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                            return RedirectToAction("AssemblyKitDetail", Approve);

                        case "Print":
                        return GenratePdfFile(SaveDeatilModel);

                    case "BacktoList":
                            var WF_status = SaveDeatilModel.WF_Status1;
                            TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                            return RedirectToAction("AssemblyKit", new { WF_status });
                        default:
                            return new EmptyResult();
                    }
                //}

              
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public FileResult GenratePdfFile(AssemblyKit_DetailModel _Model)
        {
            return File(GetPdfData(_Model.DocumentNumber, _Model.DocumentDate), "application/pdf", "AssemblyKit.pdf");
        }
        public byte[] GetPdfData(string DocumentNumber, string DocumentDate)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
               
                GetCompDeatil();
                DataSet Deatils = _AssKit_ISERVICES.GetPurchaseAKDeatilsPDF(CompID, BranchId, DocumentNumber, DocumentDate);
                ViewBag.PageName = "AK";
                ViewBag.Title = "Assembly Kit";
                ViewBag.Details = Deatils;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status"].ToString().Trim();
                ViewBag.Website = Deatils.Tables[0].Rows[0]["comp_website"].ToString();
           
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/AssemblyKit/AssemblyKitprint.cshtml"));
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status1)
        {

            AssemblyKit_DetailModel _model = new AssemblyKit_DetailModel();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _model.DocumentNumber = jObjectBatch[i]["DocNo"].ToString();
                    _model.DocumentDate = jObjectBatch[i]["DocDate"].ToString();
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
                ApproveModel.wrkf = WF_Status1;
            }
            TempData["ModelData"] = _model;
            ApproveModel.trnstyp = "Update";
            ApproveModel.Doc_no = _model.DocumentNumber;
            ApproveModel.Doc_dt = _model.DocumentDate;
            ApproveModel.btn = "BtnEdit";
            return RedirectToAction("AssemblyKitDetail", ApproveModel);
        }
        public ActionResult ApproveData(AssemblyKit_DetailModel _model, string ListFilterData1, string WF_Status1)
        {
            try
            {
                GetCompDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Doc_no = _model.DocumentNumber;
                string Doc_dt = _model.DocumentDate;
                string A_Status = _model.A_Status;
                string A_Level = _model.A_Level;
                string A_Remarks = _model.A_Remarks;
                string Message = _AssKit_ISERVICES.Approve_Detail(CompID, BranchId, DocumentMenuId, Doc_no, Doc_dt, UserID, mac_id, A_Status, A_Level, A_Remarks);
                string[] FDetail = Message.Split(',');
                string data = FDetail[0].ToString();
                if(data== "StockNotAvail")
                {
                    _model.StockItemWiseMessage = string.Join(",", FDetail.Skip(1));
                    _model.Message = "StockNotAvail";
                }
                else
                {
                    if (Message == "Approved")
                    {
                        //string fileName = "PC_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        ////var filePath = SavePdfDocToSendOnEmailAlert(Cnf_No, Cnf_Date, fileName);
                        //_Common_IServices.SendAlertEmail(CompID, brnchID, DocumentMenuId, Cnf_No, "AP", UserID, "0", filePath);

                        _model.Message = "Approved";
                    }
                }
               
                //if (Message == "StockNotAvail")
                //{
                //    //Session["Message"] = "StockNotAvail";
                //    _model.Message = "StockNotAvail";
                //}

                UrlModel ApproveModel = new UrlModel();
                _model.DocumentNumber = Doc_no;
                _model.DocumentDate = Doc_dt;
                _model.TransType = "Update";
                _model.BtnName = "BtnEdit";
                _model.Command = "Approve";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _model.WF_Status1 = WF_Status1;
                    ApproveModel.wrkf = WF_Status1;
                }
                TempData["ModelData"] = _model;

                ApproveModel.trnstyp = "Update";
                ApproveModel.Doc_no = _model.DocumentNumber;
                ApproveModel.Doc_dt = _model.DocumentDate;
                ApproveModel.btn = "BtnEdit";
                ApproveModel.Cmd = "Approve";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("AssemblyKitDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult SaveUpdateData(AssemblyKit_DetailModel _model)
        {
            string SaveMessage = "";
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_model.CancelFlag == false)
                {
                    GetCompDeatil();
                    DataTable FGRHeader = new DataTable();
                    DataTable ItemDetails = new DataTable();
                    DataTable Attachments = new DataTable();
                    DataTable ItemBatchDetails = new DataTable();
                    DataTable ItemSerialDetails = new DataTable();
                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(string));
                    dtheader.Columns.Add("doc_no", typeof(string));
                    dtheader.Columns.Add("doc_dt", typeof(string));
                    dtheader.Columns.Add("prod_id", typeof(string));
                    dtheader.Columns.Add("uom", typeof(string));
                    dtheader.Columns.Add("ass_qty", typeof(string));
                    dtheader.Columns.Add("remark", typeof(string));
                    dtheader.Columns.Add("status", typeof(string));
                    dtheader.Columns.Add("create_id", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));
                    dtheader.Columns.Add("wh_id", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;

                    if (_model.DocumentNumber != null)
                    {
                        dtrowHeader["TransType"] = "Update";
                    }
                    else
                    {
                        dtrowHeader["TransType"] = "Save";
                    }
                    dtrowHeader["comp_id"] = CompID;

                    dtrowHeader["br_id"] = BranchId;
                    dtrowHeader["doc_no"] = _model.DocumentNumber;
                    dtrowHeader["doc_dt"] = _model.DocumentDate;
                    dtrowHeader["prod_id"] = _model.hdnAssemblyProductID;
                    dtrowHeader["uom"] = _model.AssemblyUOMID;
                    dtrowHeader["ass_qty"] = _model.AssemblyQuantity;
                    dtrowHeader["remark"] = _model.remarks;
                    dtrowHeader["status"] = _model.DocumentStatus;
                    dtrowHeader["create_id"] = UserID;
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" +
                        Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    dtrowHeader["mac_id"] = SystemDetail;
                    dtrowHeader["wh_id"] = _model.WHIDHeader;
                    dtheader.Rows.Add(dtrowHeader);
                    FGRHeader = dtheader;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("wh_id", typeof(int));
                    dtItem.Columns.Add("qty", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));

                    JArray jObject = JArray.Parse(_model.ItemDetail);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                        dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                        dtrowLines["wh_id"] = jObject[i]["WareHouse"].ToString();
                        dtrowLines["qty"] = jObject[i]["InputQty"].ToString();
                        dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ItemDetails = dtItem;
                    DataTable InputdtSubItem = new DataTable();
                    InputdtSubItem.Columns.Add("item_id", typeof(string));
                    InputdtSubItem.Columns.Add("sub_item_id", typeof(string));
                    InputdtSubItem.Columns.Add("qty", typeof(string));

                    if (_model.SubItemDeatilINput != null)
                    {
                        JArray jObject2 = JArray.Parse(_model.SubItemDeatilINput);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = InputdtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            InputdtSubItem.Rows.Add(dtrowItemdetails);
                        }
                    }
                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("uom_id", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    Batch_detail.Columns.Add("expiry_date", typeof(string));
                    Batch_detail.Columns.Add("batch_qty", typeof(string));
                    Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    if (_model.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_model.ItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
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
                            dtrowBatchDetailsLines["batch_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();

                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                    }
                    ItemBatchDetails = Batch_detail;

                    DataTable Serial_detail = new DataTable();
                    Serial_detail.Columns.Add("item_id", typeof(string));
                    Serial_detail.Columns.Add("uom_id", typeof(int));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("serial_qty", typeof(string));
                    Serial_detail.Columns.Add("lot_no", typeof(string));

                    if (_model.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_model.ItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                            dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                    }
                    ItemSerialDetails = Serial_detail;

                    DataTable dtAttachment = new DataTable();
                    var _AssKitattch = TempData["ModelDataattch"] as attchmentModel;
                    TempData["ModelDataattch"] = null;
                    if (_model.attatchmentdetail != null)
                    {
                        if (_AssKitattch != null)
                        {
                            if (_AssKitattch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _AssKitattch.AttachMentDetailItmStp as DataTable;
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
                                if (!string.IsNullOrEmpty(_model.DocumentNumber))
                                {
                                    dtrowAttachment1["id"] = _model.DocumentNumber;
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
                                if (!string.IsNullOrEmpty(_model.DocumentNumber))
                                {
                                    ItmCode = _model.DocumentNumber;
                                }
                                else
                                {
                                    ItmCode = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BranchId + ItmCode.Replace("/", "") + "*");
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
                    Attachments = dtAttachment;
                    SaveMessage = _AssKit_ISERVICES.InsertUpdate(FGRHeader, ItemDetails, ItemBatchDetails, ItemSerialDetails, InputdtSubItem, Attachments);
                    string[] Data = SaveMessage.Split(',');

                    string doc_no = Data[1];
                    string Message = Data[0];
                    string doc_dt = Data[2];
                    if (Message == "DataNotFound")
                    {

                        var a = doc_no.Split(',');
                        var msg = "Data Not found" + " " + a[0].Trim() + " in " + PageName;
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msg, "", "");
                        _model.Message = Message;
                        return RedirectToAction("GatePassInwardDetail");
                    }
                    if (Message == "Save")
                    {
                        string Guid = "";
                        if (_AssKitattch != null)
                        {
                            if (_AssKitattch.Guid != null)
                            {
                                Guid = _AssKitattch.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BranchId, guid, PageName, doc_no, _model.TransType, Attachments);
                    }
                    if (Message == "Update" || Message == "Save")
                    {
                        _model.Message = "Save";
                        _model.Command = "Update";
                        _model.DocumentNumber = doc_no;
                        _model.DocumentDate = doc_dt;
                        _model.TransType = "Update";
                        _model.BtnName = "BtnEdit";
                        _model.AttachMentDetailItmStp = null;
                        _model.Guid = null;
                    }
                    return RedirectToAction("AssemblyKitDetail", _model);
                }
                else
                {
                    GetCompDeatil();
                    string br_id = BranchId;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet message = _AssKit_ISERVICES.Cancel_Document(CompID, br_id, _model.DocumentNumber, _model.DocumentDate, UserID, DocumentMenuId, mac_id);

                    _model.Message = message.Tables[0].Rows[0]["result"].ToString();
                    _model.DocumentNumber = _model.DocumentNumber;
                    _model.DocumentDate = _model.DocumentDate;
                    _model.TransType = "Update";
                    _model.BtnName = "Refresh";

                    string fileName = "FGR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    // var filePath = SavePdfDocToSendOnEmailAlert(_model.RecieptDate, _model.RecieptDate, fileName);
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _model.DocumentNumber, "C", UserID, "0" /*filePath*/);

                    return RedirectToAction("AssemblyKitDetail");
                }
            }
         
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

                }
        private ActionResult Delete(AssemblyKit_DetailModel _model, string command)
        {
            try
            {
                GetCompDeatil();
                string doc_no = _model.DocumentNumber;
                DataSet Message = _AssKit_ISERVICES.DeleteData(CompID, BranchId, _model.DocumentNumber, _model.DocumentDate);
                if (!string.IsNullOrEmpty(doc_no))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + BranchId, PageName, doc_no1, Server);
                }
                return RedirectToAction("AssemblyKitDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult getItemStockBatchWiseAfterStockUpadte(string Doc_no, string Doc_dt, string ItemID, string Transtype, string cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                GetCompDeatil();
                ds = _AssKit_ISERVICES.getItemStockBatchWiseAfterStockUpdate(CompID, BranchId, Doc_no, Doc_dt, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                ViewBag.Transtype = Transtype;
                ViewBag.Command = cmd;
                ViewBag.DocID = DocumentMenuId;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult DblClick(string Doc_no, string Doc_dt, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            GetCompDeatil();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            AssemblyKit_DetailModel dblclick = new AssemblyKit_DetailModel();
            UrlModel _url = new UrlModel();
            dblclick.DocumentNumber = Doc_no;
            dblclick.DocumentDate = Doc_dt;
            dblclick.TransType = "Update";
            dblclick.BtnName = "BtnEdit";
            dblclick.Command = "Refresh";
            if (WF_Status != null && WF_Status != "")
            {
                _url.wrkf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            _url.trnstyp= "Update";
            _url.btn = "BtnEdit";
            _url.Doc_no = Doc_no;
            _url.Doc_dt = Doc_dt;
            _url.Cmd = "Refresh";
            TempData["ListFilterData"] = ListFilterData;

            return RedirectToAction("AssemblyKitDetail", _url);
        }

        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                GetCompDeatil();
                CommonPageDetails();
                attchmentModel _AssKitattch = new attchmentModel();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                Guid gid = new Guid();
                gid = Guid.NewGuid();

                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");

                _AssKitattch.Guid = DocNo;
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BranchId, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    _AssKitattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _AssKitattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _AssKitattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }

        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData)
        {
            //Session["Message"] = "";
            AssemblyKit_DetailModel ToRefreshByJS = new AssemblyKit_DetailModel();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.DocumentNumber = a[0].Trim();
            ToRefreshByJS.DocumentDate = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnEdit";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wrkf = a[2].Trim();
            }
            Model.btn = "BtnEdit";
            Model.Doc_no = ToRefreshByJS.DocumentNumber;
            Model.Doc_dt = ToRefreshByJS.DocumentDate;
            Model.trnstyp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("AssemblyKitDetail", Model);
        }

        [HttpPost]
        public ActionResult List_Search( string Fromdate, string Todate, string Status, string Item_id)
        {
            try
            {
                GetCompDeatil();
                AssemblyKit_ListModel SearchModel = new AssemblyKit_ListModel();
                SearchModel.WF_Status = null;
                DataTable dt = _AssKit_ISERVICES.SearchDataFilter(Item_id, Fromdate, Todate, Status, CompID, BranchId, DocumentMenuId, UserID, "");
                ViewBag.AssemblyKitList = dt;
                SearchModel.Search = "Search";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialAssemblyKitList.cshtml", SearchModel);
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