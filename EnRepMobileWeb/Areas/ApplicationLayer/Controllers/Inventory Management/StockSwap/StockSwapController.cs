
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.Stock_Swap;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.Stock_Swap;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.StockSwap
{
    public class StockSwapController : Controller
    {
        string CompID, BrchID, userid, language = String.Empty;
        string DocumentMenuId = "105102165", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        StockSwap_ISERVICES _StockSwap_ISERVICES;
        StockSwap_Model _StockSwap_Model;
        CommonController cmn = new CommonController();
        public StockSwapController(StockSwap_ISERVICES _StockSwap_ISERVICES, Common_IServices _Common_IServices)
        {
            this._StockSwap_ISERVICES = _StockSwap_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/StockSwap
        public ActionResult StockSwap(string wfStatus)
        {
            StockSwap_Model model = new StockSwap_Model();
            CommonPageDetails();
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
            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            model.StatusList = statusLists;

            DataTable dt = new DataTable();
            List<ProductNameList> ProductNameLists = new List<ProductNameList>();
            dt = GetProductNameLists("0");
            foreach (DataRow dr in dt.Rows)
            {
                ProductNameList PnameList = new ProductNameList();
                PnameList.ProductId = dr["Item_id"].ToString();
                PnameList.ProductName = dr["Item_name"].ToString();
                ProductNameLists.Add(PnameList);
            }
            ProductNameLists.Insert(0, new ProductNameList() { ProductId = "0", ProductName = "---Select---" });
            model._ProductNameList = ProductNameLists;

            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //string ToDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;

            model.ToDate = CurrentDate;
            model.FromDate = startDate;
            if (wfStatus != null)
            {
                ViewBag.ListFilterData1 = "0,0,0," + wfStatus+','+ model.FromDate+','+ model.FromDate;
            }
            if (TempData["UrlData"] != null)
            {
                if (TempData["UrlData"].ToString() != "")
                {
                    UrlData urlData = TempData["UrlData"] as UrlData;
                    if (urlData.ListFilterData1 != null)
                    {
                        var arr = urlData.ListFilterData1.Split(',');
                        model.ProductName = arr[0];
                        model.ProductName1 = arr[1];
                        model.Status = arr[2];
                        model.FromDate = arr[4];
                        model.ToDate = arr[5];
                        if (wfStatus == null)
                        {
                            wfStatus = arr[3];
                        }
                        if (arr[0] == "")
                            model.ProductName = null;
                        if (arr[1] == "")         
                            model.ProductName1 = null;
                        if (arr[2] == "")
                            model.Status = null;
                        ViewBag.ListFilterData1 = model.ProductName + "," + model.ProductName1 + "," + model.Status + "," + wfStatus +","+ model.FromDate + "," + model.ToDate;
                    }
                }
            }
            if(wfStatus==null || wfStatus == "")
            {
                wfStatus = "ab";
            }
            model.ListStatus = model.Status;
            DataSet ds = _StockSwap_ISERVICES.GetSwapStockList(CompID, BrchID,model.ProductName, model.ProductName1, model.Status, DocumentMenuId, wfStatus, userid, model.FromDate, model.ToDate);
            ViewBag.SwapStockList = ds.Tables[0];
            model.Title = title;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockSwap/StockSwapList.cshtml", model);
        }
        public ActionResult AddStockSwapDetail(string swp_no,string swp_dt,string ListFilterData1)
        {
            UrlData urlData = new UrlData();
            string BtnName = swp_no == null ? "BtnAddNew" : "BtnToDetailPage";
            string TransType = swp_dt == null ? "Save" : "Update";
            
            SetUrlData(urlData, "Add", TransType, BtnName, null, swp_no, swp_dt, ListFilterData1);
            /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {

                if (swp_no == null && swp_dt == null)
                {
                    TempData["Message1"] = "Financial Year not Exist";
                    return RedirectToAction("StockSwap", urlData);
                }
                else
                {
                    //TempData["Message1"] = "Financial Year not Exist";
                    return RedirectToAction("StockSwapDetail", urlData);
                }

            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("StockSwapDetail", "StockSwap", urlData);
        }
        public ActionResult StockSwapDetail(UrlData urlData)
        {
            try
            {
                StockSwap_Model model = new StockSwap_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    model.CompId= Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                    model.BrchID= Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                /*Add by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, urlData.DocDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                CommonPageDetails();
                if (TempData["UrlData"] != null)
                {
                    urlData = TempData["UrlData"] as UrlData;
                    model.Message = TempData["Message"] != null ? TempData["Message"].ToString() : null;
                }
                model.btncommand = urlData.Command;
                model.TransType = urlData.TransType;
                model.BtnName = urlData.BtnName;
                model.SwapNumber = urlData.DocNo;
                model.SwapDate = urlData.DocDate;
                model.ListFilterData1 = urlData.ListFilterData1;
                DataSet ds = _StockSwap_ISERVICES.Edit_SwapStockDetail(CompID, BrchID, model.SwapNumber, model.SwapDate, userid, DocumentMenuId);
                ViewBag.ItemStockBatchWise = ds.Tables[1];
                ViewBag.ItemStockSerialWise = ds.Tables[2];
                ViewBag.SubItemDetails= ds.Tables[3];
                DataTable dt = new DataTable();
                List<ProductNameList> ProductNameLists = new List<ProductNameList>();
                if (model.SwapNumber != null && model.SwapNumber.ToString() != "")
                {
                    dt = GetProductNameLists("0");
                    foreach (DataRow dr in dt.Rows)
                    {
                        ProductNameList PnameList = new ProductNameList();
                        PnameList.ProductId = dr["Item_id"].ToString();
                        PnameList.ProductName = dr["Item_name"].ToString();
                        ProductNameLists.Add(PnameList);
                    }
                }
                ProductNameLists.Insert(0, new ProductNameList() { ProductId = "0", ProductName = "---Select---" });
                model._ProductNameList = ProductNameLists;

                DataTable dt1 = new DataTable();
                List<WarehouseList> WarehouseLists = new List<WarehouseList>();
                dt1 = GetWarehouseNameLists("0");
                foreach (DataRow dr in dt1.Rows)
                {
                    WarehouseList PnameList = new WarehouseList();
                    PnameList.WarehouseId = dr["wh_id"].ToString();
                    PnameList.WarehouseName = dr["wh_name"].ToString();
                    WarehouseLists.Add(PnameList);
                }
                WarehouseLists.Insert(0, new WarehouseList() { WarehouseId = "0", WarehouseName = "---Select---" });
                model._WarehouseList = WarehouseLists;

                List<WarehouseListDest> WarehouseListdest = new List<WarehouseListDest>();

                WarehouseListdest.Insert(0, new WarehouseListDest() { WarehouseId = "0", WarehouseName = "---Select---" });
                model._WarehouseListDest = WarehouseListdest;
                StockSwapDetailBindData(model, ds);
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;       
                model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/StockSwap/StockSwapDetail.cshtml", model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        private void StockSwapDetailBindData(StockSwap_Model _StockSwap_Model,DataSet ds)
        {
            try
            {
                if (Session["userid"] != null)
                {
                    userid = Session["userid"].ToString();
                }
                var QtyDigit = "0.";
                var VD = Convert.ToInt32(Session["QtyDigit"].ToString());
                for (var i = 0; i < VD; i++)
                {
                    QtyDigit += "0";
                }
                if (ds.Tables.Count > 1)
                {
                    _StockSwap_Model.SwapNumber = ds.Tables[0].Rows[0]["swp_no"].ToString();
                    _StockSwap_Model.SwapDate = ds.Tables[0].Rows[0]["swp_dt"].ToString();
                    _StockSwap_Model.ProductName = ds.Tables[0].Rows[0]["src_prod_id"].ToString();
                    _StockSwap_Model.Src_Uom = ds.Tables[0].Rows[0]["srcUomName"].ToString();
                    _StockSwap_Model.Src_UomID = ds.Tables[0].Rows[0]["src_uom"].ToString();
                    _StockSwap_Model.Warehouse = Convert.ToInt32(ds.Tables[0].Rows[0]["src_wh_id"].ToString());
                    _StockSwap_Model.SwapQuantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["swp_qty"]).ToString(QtyDigit); 
                    _StockSwap_Model.DestSwapQuantity = Convert.ToInt32(ds.Tables[0].Rows[0]["swp_qty"]).ToString(QtyDigit);
                    //_StockSwap_Model.src_avl_stk = ds.Tables[0].Rows[0]["src_wh_avl_stk_bs"].ToString();
                    _StockSwap_Model.AvailableQuantitySrc = ds.Tables[0].Rows[0]["src_wh_avl_stk_bs"].ToString();
                    _StockSwap_Model.i_batch = ds.Tables[0].Rows[0]["i_batch"].ToString();
                    _StockSwap_Model.i_serial = ds.Tables[0].Rows[0]["i_serial"].ToString();
                    _StockSwap_Model.Status = ds.Tables[0].Rows[0]["SwapStkStatus"].ToString();
                    _StockSwap_Model.StatusCode = ds.Tables[0].Rows[0]["swp_status"].ToString().Trim();
                    _StockSwap_Model.ProductName1 = ds.Tables[0].Rows[0]["dest_prod_id"].ToString();
                    _StockSwap_Model.Dest_UomID = ds.Tables[0].Rows[0]["dest_uom"].ToString();
                    _StockSwap_Model.Dest_Uom = ds.Tables[0].Rows[0]["DestUomName"].ToString();
                    _StockSwap_Model.Warehouse1 = Convert.ToInt32(ds.Tables[0].Rows[0]["dest_wh_id"].ToString());
                    //_StockSwap_Model.Dest_avl_stk = ds.Tables[0].Rows[0]["dest_wh_avl_stk_bs"].ToString();
                    _StockSwap_Model.AvailableQuantityDest = ds.Tables[0].Rows[0]["dest_wh_avl_stk_bs"].ToString();
                    _StockSwap_Model.CreatedBy = ds.Tables[0].Rows[0]["createdby"].ToString();
                    _StockSwap_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                    string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                    _StockSwap_Model.ApprovedBy = ds.Tables[0].Rows[0]["Approvedby"].ToString();
                    _StockSwap_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                    _StockSwap_Model.AmendedBy = ds.Tables[0].Rows[0]["Modby"].ToString();
                    _StockSwap_Model.AmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                    _StockSwap_Model.SrcSubItm = ds.Tables[0].Rows[0]["SrcSubItm"].ToString();
                    _StockSwap_Model.DestSubItm = ds.Tables[0].Rows[0]["DestSubItm"].ToString();
                    _StockSwap_Model.swp_type =  ds.Tables[0].Rows[0]["swp_type"].ToString().Trim();
                    _StockSwap_Model.DestSwapQuantity =  ds.Tables[0].Rows[0]["dest_swp_qty"].ToString().Trim();
                    _StockSwap_Model.CreatedId = create_id;
                    string Statuscode = _StockSwap_Model.StatusCode;

                    List<WarehouseListDest> WarehouseLists = new List<WarehouseListDest>();
                    foreach (DataRow dr in ds.Tables[7].Rows)
                    {
                        WarehouseListDest PnameList = new WarehouseListDest();
                        PnameList.WarehouseId = dr["wh_id"].ToString();
                        PnameList.WarehouseName = dr["wh_name"].ToString();
                        WarehouseLists.Add(PnameList);
                    }
                    WarehouseLists.Insert(0, new WarehouseListDest() { WarehouseId = "0", WarehouseName = "---Select---" });
                    _StockSwap_Model._WarehouseListDest = WarehouseLists;

                    string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                    _StockSwap_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                    _StockSwap_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                    if (ViewBag.AppLevel != null && _StockSwap_Model.btncommand != "Edit")
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
                            if (create_id != userid)
                            {
                                _StockSwap_Model.BtnName = "Refresh";
                            }
                            else
                            {
                                if (nextLevel == "0")
                                {
                                    if (create_id == userid)
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
                                    _StockSwap_Model.BtnName = "BtnToDetailPage";
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
                                    _StockSwap_Model.BtnName = "BtnToDetailPage";
                                }

                            }
                            if (userid == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                _StockSwap_Model.BtnName = "BtnToDetailPage";
                            }


                            if (nextLevel == "0")
                            {
                                if (sent_to == userid)
                                {
                                    ViewBag.Approve = "Y";
                                    ViewBag.ForwardEnbl = "N";
                                    /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _StockSwap_Model.BtnName = "BtnToDetailPage";
                                }


                            }
                        }
                        if (Statuscode == "F")
                        {
                            if (userid == sent_to)
                            {
                                ViewBag.ForwardEnbl = "Y";
                                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                                if (TempData["Message1"] != null)
                                {
                                    ViewBag.Message = TempData["Message1"];
                                }
                                /*End to chk Financial year exist or not*/
                                _StockSwap_Model.BtnName = "BtnToDetailPage";
                            }
                            if (nextLevel == "0")
                            {
                                if (sent_to == userid)
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
                                _StockSwap_Model.BtnName = "BtnToDetailPage";
                            }
                        }
                        if (Statuscode == "A")
                        {
                            if (create_id == userid || approval_id == userid)
                            {
                                _StockSwap_Model.BtnName = "BtnToDetailPage";

                            }
                            else
                            {
                                _StockSwap_Model.BtnName = "Refresh";
                            }
                        }
                    }
                    if (ViewBag.AppLevel.Rows.Count == 0)
                    {
                        ViewBag.Approve = "Y";
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return null;
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
        public ActionResult SwapStockListSearch(string Src_prod_id, string Dest_prod_id, string txtFromdate, string txtTodate, string status, string wfStatus)
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
                    userid = Session["userid"].ToString();
                }
                if (Src_prod_id == "0")
                {
                    Src_prod_id = null;
                }
                if (status == "0")
                {
                    status = null;
                }
                if (Dest_prod_id == "0")
                {
                    Dest_prod_id = null;
                }
                if (wfStatus == null || wfStatus == "")
                {
                    wfStatus = "ab";
                }
                DataSet ds = new DataSet();
                ds = _StockSwap_ISERVICES.GetSwapStockList(CompID, BrchID, Src_prod_id, Dest_prod_id, status, DocumentMenuId, wfStatus, userid, txtFromdate, txtTodate);
                ViewBag.SwapStockList = ds.Tables[0];
                ViewBag.SwpSearch = "SwpSearch";
                ViewBag.ListFilterData1 = Src_prod_id + "," + Dest_prod_id + "," + status + "," + wfStatus + "," + txtFromdate + "," + txtTodate;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialStockSwapList.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private void SetUrlData(UrlData urlData, string Command, string TransType, string BtnName, string Message = null, string DocNo=null,string DocDate=null, string ListFilterData1 = null)
        {
            urlData.Command = Command;
            urlData.TransType = TransType;
            urlData.BtnName = BtnName;
            urlData.DocNo = DocNo;
            urlData.DocDate = DocDate;
            urlData.ListFilterData1 = ListFilterData1;
            TempData["UrlData"] = urlData;
            TempData["Message"] = Message;
        }
        public ActionResult StockSwapDetailSave(StockSwap_Model _StockSwap_Model, string command)
        {
            try
            {
                /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                UrlData urlData = new UrlData();
                if (_StockSwap_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message1"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_StockSwap_Model.SwapNumber))
                                return RedirectToAction("AddStockSwapDetail", new { swp_no = _StockSwap_Model.SwapNumber, swp_dt = _StockSwap_Model.SwapDate, ListFilterData1 = _StockSwap_Model.ListFilterData1 });
                            else
                             SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _StockSwap_Model.ListFilterData1);
                            return RedirectToAction("StockSwapDetail", urlData);
                           
                        }
                        /*End to chk Financial year exist or not*/
                       SetUrlData(urlData, "Add", "Save", "BtnAddNew",null, null, null, _StockSwap_Model.ListFilterData1);
                        return RedirectToAction("StockSwapDetail", urlData);
                    case "Edit":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message1"] = "Financial Year not Exist";
                        //    return RedirectToAction("AddStockSwapDetail", new { swp_no = _StockSwap_Model.SwapNumber, swp_dt = _StockSwap_Model.SwapDate, ListFilterData1 = _StockSwap_Model.ListFilterData1 });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string stkdt = _StockSwap_Model.SwapDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, stkdt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("AddStockSwapDetail", new { swp_no = _StockSwap_Model.SwapNumber, swp_dt = _StockSwap_Model.SwapDate, ListFilterData1 = _StockSwap_Model.ListFilterData1 });
                        }
                        /*End to chk Financial year exist or not*/
                        SetUrlData(urlData, "Edit", "Update", "BtnEdit",null, _StockSwap_Model.SwapNumber, _StockSwap_Model.SwapDate, _StockSwap_Model.ListFilterData1);
                        return RedirectToAction("StockSwapDetail", urlData);
                    case "Save":
                        SaveUpdateMaterialIssue(_StockSwap_Model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _StockSwap_Model.Message, _StockSwap_Model.SwapNumber, _StockSwap_Model.SwapDate, _StockSwap_Model.ListFilterData1);
                        return RedirectToAction("StockSwapDetail", urlData);
                    case "Approve":
                        /*start Add by Hina on 14-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message1"] = "Financial Year not Exist";
                        //    return RedirectToAction("AddStockSwapDetail", new { swp_no = _StockSwap_Model.SwapNumber, swp_dt = _StockSwap_Model.SwapDate, ListFilterData1 = _StockSwap_Model.ListFilterData1 });
                        //}
                        /*Above Commented and modify by Hina sharma on 14-05-2025 to check Existing with previous year transaction*/
                        string stkdt1 = _StockSwap_Model.SwapDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, stkdt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("AddStockSwapDetail", new { swp_no = _StockSwap_Model.SwapNumber, swp_dt = _StockSwap_Model.SwapDate, ListFilterData1 = _StockSwap_Model.ListFilterData1 });
                        }
                        /*End to chk Financial year exist or not*/
                        var swapType = _StockSwap_Model.SwapType;
                        ApproveSwapStockDetails(_StockSwap_Model.SwapNumber, _StockSwap_Model.SwapDate, _StockSwap_Model.A_Status, _StockSwap_Model.A_Level, _StockSwap_Model.A_Remarks,"","", swapType, _StockSwap_Model);
                        SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _StockSwap_Model.Message, _StockSwap_Model.SwapNumber, _StockSwap_Model.SwapDate, _StockSwap_Model.ListFilterData1);
                        return RedirectToAction("StockSwapDetail", urlData);
                    case "Refresh":
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", null, null, null, _StockSwap_Model.ListFilterData1);
                        return RedirectToAction("StockSwapDetail", urlData);
                    case "Delete":
                        DeleteSwapStockDetail(_StockSwap_Model);
                        SetUrlData(urlData, "Refresh", "Refresh", "Refresh", _StockSwap_Model.Message, null, null, _StockSwap_Model.ListFilterData1);
                        return RedirectToAction("StockSwapDetail", urlData);
                    case "BacktoList":
                        SetUrlData(urlData, "", "", "", null, null, null, _StockSwap_Model.ListFilterData1);
                        return RedirectToAction("StockSwap");
                    default:
                        SetUrlData(urlData, "Add", "Save", "BtnAddNew");
                        return RedirectToAction("StockSwapDetail", urlData);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult SaveUpdateMaterialIssue(StockSwap_Model _StockSwap_Model)
        {
            try
            {
                if (Session["compid"] != null)                
                    CompID = Session["compid"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                if (Session["userid"] != null)
                    userid = Session["userid"].ToString();
                DataTable ItemBatchDetails = new DataTable();
                DataTable ItemSerialDetails = new DataTable();
                DataTable dtSubItem = new DataTable();

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                if (_StockSwap_Model.swp_type == null)
                {
                    if (_StockSwap_Model.SwapType != null)
                    {
                        _StockSwap_Model.swp_type = _StockSwap_Model.SwapType;
                    }
                }
                DataTable Batch_detail = new DataTable();
                Batch_detail.Columns.Add("lot_no", typeof(string));
                Batch_detail.Columns.Add("batch_no", typeof(string));
                Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
                Batch_detail.Columns.Add("expiry_date", typeof(DateTime));
                Batch_detail.Columns.Add("swp_qty", typeof(string));
                if (_StockSwap_Model.swp_type == "I" || _StockSwap_Model.swp_type == "U")
                {
                    if (_StockSwap_Model.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_StockSwap_Model.ItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
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
                            dtrowBatchDetailsLines["swp_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                    }
                    //ItemBatchDetails = Batch_detail;
                }
                DataTable Serial_detail = new DataTable();
                Serial_detail.Columns.Add("lot_no", typeof(string));
                Serial_detail.Columns.Add("serial_no", typeof(string));
                Serial_detail.Columns.Add("serial_qty", typeof(string));
                Serial_detail.Columns.Add("swp_qty", typeof(string));
                if (_StockSwap_Model.swp_type == "I" || _StockSwap_Model.swp_type == "U")
                {
                    if (_StockSwap_Model.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_StockSwap_Model.ItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["swp_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                    }
                    //ItemSerialDetails = Serial_detail;
                }
                /*----------------------Sub Item ----------------------*/
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("avl_stock", typeof(string));
                dtSubItem.Columns.Add("swp_qty", typeof(string));
                dtSubItem.Columns.Add("SwapType", typeof(string));

                if (_StockSwap_Model.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(_StockSwap_Model.SubItemDetailsDt);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["avl_stock"] = jObject2[i]["avl_qty"].ToString();
                        dtrowItemdetails["swp_qty"] = jObject2[i]["qty"].ToString();
                        dtrowItemdetails["SwapType"] = jObject2[i]["SwapType"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                }
                if (_StockSwap_Model.AvailableQuantityDest == null)
                {
                    _StockSwap_Model.AvailableQuantityDest = "0";
                }
                string SaveMessage = _StockSwap_ISERVICES.InsertUpdateMaterialIssue(_StockSwap_Model, CompID, BrchID, userid, mac_id, DocumentMenuId, Batch_detail, Serial_detail, dtSubItem);

                string[] FDate = SaveMessage.Split(',');
                string Message = FDate[5].ToString();
                string SwapNumber = FDate[0].ToString();
                string SwapDate = FDate[6].ToString();
                if (Message == "Update" || Message == "Save")
                {
                    _Common_IServices.SendAlertEmail(Session["CompId"].ToString(), Session["BranchId"].ToString(), DocumentMenuId, SwapNumber, "AP", userid, "0");
                    _StockSwap_Model.SwapNumber = SwapNumber;
                    _StockSwap_Model.SwapDate = SwapDate;
                    _StockSwap_Model.Message = "Save";
                    _StockSwap_Model.btncommand = "Update";
                    _StockSwap_Model.TransType = "Update";
                    _StockSwap_Model.BtnName = "BtnEdit";
                }
                return RedirectToAction("StockSwapDetail");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult ApproveSwapStockDetails(string swp_no,string swp_dt, string A_Status, string A_Level, string A_Remarks, string ListFilterData1, string WF_status1,string SwapType, StockSwap_Model _model)
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
                    userid = Session["userid"].ToString();
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;

                string Result = _StockSwap_ISERVICES.ApproveSwapStockDetails(CompID, BrchID, swp_no, swp_dt, A_Status
                    , A_Level, A_Remarks, userid, mac_id, DocumentMenuId, SwapType);
                //if(Result == "stockNotAvailable")
                //{
                //    _model.Message = "stockNotAvailable";
                //}
                //else
                //{
                    _model.Message = Result.Split(',')[0] == "A" ? "Approved" : Result.Split(',')[3];
                //}
                
                SetUrlData(urlData, "Add", "Update", "BtnToDetailPage", _model.Message, swp_no, swp_dt, ListFilterData1);
                return RedirectToAction("StockSwapDetail", urlData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
                //throw ex;
            }
        }
        public ActionResult GetSubItemWhAvlstockDetail(string Item_id, string flag,string swp_no, string swp_dt)
        {
            //JsonResult DataRows = null;
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
                DataSet ds = _StockSwap_ISERVICES.GetSubItemWhAvlstockDetails(Comp_ID, Br_ID, Item_id, flag, swp_no, swp_dt);
                ViewBag.SubitemAvlStockDetail = ds.Tables[0];              
                ViewBag.Flag = "WH";
                return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                //DataRows = Json(JsonConvert.SerializeObject(OCTaxTmplt));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            //return DataRows;
        }
        private void DeleteSwapStockDetail(StockSwap_Model model)
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
                string Result = _StockSwap_ISERVICES.DeleteSwapStockDetails(CompID, BrchID, model.SwapNumber, model.SwapDate);
                model.Message = Result.Split(',')[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
, string Flag, string Status, string Doc_no, string Doc_dt, string wh_id,string Type)
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
                    dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, wh_id, Item_id, null/*UomId*/, "wh").Tables[0];
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
                            if (Type == "SrcSwapQty")
                            {
                                if (item.GetValue("src_Item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);
                                    //dt.Rows[i]["avl_stk"] = cmn.ConvertToDecimal(item.GetValue("avl_stock").ToString(), DecDgt);
                                    flag = "Y";
                                }
                            }
                            else
                            {
                                if (item.GetValue("dest_Item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);
                                    //dt.Rows[i]["avl_stk"] = cmn.ConvertToDecimal(item.GetValue("avl_stock").ToString(), DecDgt);
                                    flag = "Y";
                                }
                            }
                        }
                    }
                    for (var i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        dt.Rows[i].AcceptChanges();
                    }
                }

                else
                {
                    dt = _StockSwap_ISERVICES.GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag,Type).Tables[0];
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag == "SwapQty" ? Flag : "MTI",
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    Flag1 = Type,
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
        [NonAction]
        private DataTable GetProductNameLists( string itemId)
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

                DataTable dt = _StockSwap_ISERVICES.GetProductNameLists(CompID, BrchID, itemId);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        [NonAction]
        private DataTable GetWarehouseNameLists(string Wh_id)
        {
            try
            {
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
                DataTable dt = _StockSwap_ISERVICES.GetWarehouseNameLists(CompID, BrchID, Wh_id);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
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
        public ActionResult getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId
           , string SelectedItemdetail, string DMenuId, string Command, string TransType,string status)
        {
            try
            {
                DataSet ds = new DataSet();
                if (ItemId != "")
                {
                    ds = _StockSwap_ISERVICES.getItemStockBatchWise(ItemId, WarehouseId, CompId, BranchId);
                }
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())//
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
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = status;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemStockBatchWiseAfterStockUpadte(string SwapNumber, string SwapDate, string ItemID
            , string DMenuId, string Command, string TransType,string status)
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
                ds = _StockSwap_ISERVICES.getItemStockBatchWiseAfterStockUpdate(CompID, br_id, SwapNumber, SwapDate, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = status;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId
            , string SelectedItemSerial, string DMenuId, string Command, string TransType,string status)
        {
            try
            {
                DataSet ds = new DataSet();

                if (ItemId != "")
                    ds = _StockSwap_ISERVICES.getItemstockSerialWise(ItemId, WarehouseId, CompId, BranchId);
                if (SelectedItemSerial != null && SelectedItemSerial != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemSerial);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString() == ds.Tables[0].Rows[i]["item_id"].ToString() && item.GetValue("LOTId").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("SerialNO").ToString() == ds.Tables[0].Rows[i]["serial_no"].ToString())
                            {
                                ds.Tables[0].Rows[i]["SerailSelected"] = "Y";
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockSerialWise = ds.Tables[0];
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = status;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemstockSerialWiseAfterStockUpadte(string SwapNumber, string SwapDate, string ItemID
           , string DMenuId, string Command, string TransType,string status)
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
                ds = _StockSwap_ISERVICES.getItemstockSerialWiseAfterStockUpdate(CompID, br_id, SwapNumber, SwapDate, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                DocumentMenuId = DMenuId;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                ViewBag.DocumentCode = status;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
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
        public ActionResult GetDestProductListBind(BindItemList bindItem,string Itemid)
        {
            try
            {
                Dictionary<string, string> ItemList = new Dictionary<string, string>();
              
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ItemTableData = _StockSwap_ISERVICES.GetDataitemList(CompID, BrchID, bindItem.SearchName, Itemid);

                if (ItemTableData.Tables[0].Rows.Count > 0)
                {
                    //ItemList.Add("0" + "_" + "H1", "Heading");
                    for (int i = 0; i < ItemTableData.Tables[0].Rows.Count; i++)
                    {
                        string itemId = ItemTableData.Tables[0].Rows[i]["Item_id"].ToString();
                        string itemName = ItemTableData.Tables[0].Rows[i]["Item_name"].ToString();
                        string Uom = ItemTableData.Tables[0].Rows[i]["uom_name"].ToString();
                        ItemList.Add(itemId + "_" + Uom, itemName);
                    }
                }
                return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
           
        }
    }
}
