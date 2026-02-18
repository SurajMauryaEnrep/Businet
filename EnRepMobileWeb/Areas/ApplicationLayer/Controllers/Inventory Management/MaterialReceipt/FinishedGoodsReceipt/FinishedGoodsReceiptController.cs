
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.FinishedGoodsReceipt;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.FinishedGoodsReceipt;
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
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialReceipt.FinishedGoodsReceipt
{
    public class FinishedGoodsReceiptController : Controller
    {
        string CompID, brnchID, UserID, language = String.Empty;
        string DocumentMenuId = "105102115105", title;
       
        Common_IServices _Common_IServices;
        FinishedGoodsReceipt_ISERVICE _FinishedGoodsReceipt_ISERVICE;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        public FinishedGoodsReceiptController(Common_IServices common_IServices, FinishedGoodsReceipt_ISERVICE finishedGoodsReceipt_ISERVICE)
        {
            this._Common_IServices = common_IServices;
            this._FinishedGoodsReceipt_ISERVICE = finishedGoodsReceipt_ISERVICE;
        }
        // GET: ApplicationLayer/FinishedGoodsReceipt

        public ActionResult DashBordtoListFinishGoodsReceipt(string docid, string status)
        {
            var WF_status = status;
            return RedirectToAction("FinishedGoodsReceipt", new { WF_status });
        }
        public ActionResult FinishedGoodsReceipt(string WF_status)
        {
            try
            {
                FinishedGoodsReceiptModel FGRModel = new FinishedGoodsReceiptModel();
                FGRModel.WF_Status = WF_status;
                ComDeatil();
                CommonPageDetails();
                BindAllDropDownList(FGRModel);
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var shopfloreid = a[0].Trim();
                    var opertionid = a[1].Trim();
                    var Fromdate = a[2].Trim();
                    var Todate = a[3].Trim();
                    var Status = a[4].Trim();
                    var src_type = a[5].Trim();
                    var itemID = a[6].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    FGRModel.FromDate = Fromdate;
                    FGRModel.ListFilterData = TempData["ListFilterData"].ToString();
                    DataTable dt = _FinishedGoodsReceipt_ISERVICE.SearchDataFilter(shopfloreid, opertionid, Fromdate, Todate, Status, CompID, brnchID, DocumentMenuId, src_type, itemID);
                    FGRModel.FGRSearch = "FGR_Search";
                    ViewBag.FinishGoodReciptList = dt;
                    FGRModel.Shopfloor = shopfloreid;
                    FGRModel.Operation = opertionid;
                    FGRModel.FromDate = Fromdate;
                    FGRModel.ToDate = Todate;
                    FGRModel.Status = Status;
                    FGRModel.Source_type = src_type;
                    FGRModel.Product_id = itemID;
                    FGRModel.ProductName = itemID;

                }
                else
                {
                    FGRModel.FromDate = startDate;
                    FGRModel.ToDate = CurrentDate;
                    DataTable FGRList = new DataTable();
                    FGRList = GetFGRDetails(FGRModel);
                  
                    ViewBag.FinishGoodReciptList = FGRList;
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                FGRModel.Title = title;

                return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/FinishedGoodsReceipt/FinishedGoodsReceiptList.cshtml", FGRModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
          
        }
        [HttpPost]
        public ActionResult FinishGoodsReciept_Search(string shopfloreid,string opertionid, string Fromdate, string Todate, string Status,string Source_type, string Item_id)
        {
            try
            {
                ComDeatil();
                FinishedGoodsReceiptModel SearchModel = new FinishedGoodsReceiptModel();       
                SearchModel.WF_Status = null;
                DataTable dt = _FinishedGoodsReceipt_ISERVICE.SearchDataFilter(shopfloreid, opertionid, Fromdate, Todate, Status, CompID, brnchID, DocumentMenuId, Source_type, Item_id);
                SearchModel.FGRSearch = "FRG_Search";
                ViewBag.FinishGoodReciptList = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialFinishedGoodsReceiptList.cshtml", SearchModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult FRGDblClick(string rcpt_no, string rcpt_dt, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            ComDeatil();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            FinishedGoodsReceiptModelDetail dblclick = new FinishedGoodsReceiptModelDetail();
            UrlModel _url = new UrlModel();
            dblclick.RecieptNumber = rcpt_no;
            dblclick.RecieptDate = rcpt_dt;
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
            _url.FGR_no = rcpt_no;
            _url.FGR_dt = rcpt_dt;
            _url.Cmd = "Refresh";
            TempData["ListFilterData"] = ListFilterData;

            return RedirectToAction("FinishedGoodsReceiptDetail", _url);
        }
        private DataTable GetFGRDetails(FinishedGoodsReceiptModel FGRModelList)
        {
            ComDeatil();
            string wfstatus = "";
           
            if (FGRModelList.WF_Status != null)
            {
                wfstatus = FGRModelList.WF_Status;
            }
            else
            {
                wfstatus = "";
            }

            DataSet dt = _FinishedGoodsReceipt_ISERVICE.GetFGRList(CompID, brnchID, UserID, wfstatus, DocumentMenuId, FGRModelList.FromDate, FGRModelList.ToDate);
        
            return dt.Tables[0];
        }
        public void ComDeatil()
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
        public void BindAllDropDownList(FinishedGoodsReceiptModel FGRModel)
        {
            try {
                DataSet dt = _FinishedGoodsReceipt_ISERVICE.GetAllDropDownList(CompID, brnchID);

                List<ShopfloorListDropDown> list = new List<ShopfloorListDropDown>();
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    ShopfloorListDropDown shp = new ShopfloorListDropDown();
                    shp.shop_id = dr["shfl_id"].ToString();
                    shp.shop_name = dr["shfl_name"].ToString();
                    list.Add(shp);
                }


                List<OperationListDropDown> list1 = new List<OperationListDropDown>();
                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    OperationListDropDown operation = new OperationListDropDown();
                    operation.op_id = dr["op_id"].ToString();
                    operation.op_name = dr["op_name"].ToString();
                    list1.Add(operation);
                }
                list.Insert(0, new ShopfloorListDropDown() { shop_id = "0", shop_name = "All" });
                list1.Insert(0, new OperationListDropDown() { op_id = "0", op_name = "All" });
                FGRModel.ShopfloorList = list;
                FGRModel.OperationList = list1;

                List<StatusList> list2 = new List<StatusList>();
                foreach (var dr in ViewBag.StatusList.Rows)
                {
                    StatusList Status = new StatusList();
                    Status.StatusID = dr["status_code"].ToString();
                    Status.Status_Name = dr["status_name"].ToString();
                    list2.Add(Status);
                }
                list2.Insert(0, new StatusList() { StatusID = "0", Status_Name = "All" });
                FGRModel.Status_list = list2;
              
                GetItemList(FGRModel);
              
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
           

        }
        public void BindAllDropDownDeatil( FinishedGoodsReceiptModelDetail FGR)
        {
            try {
                DataSet dt = _FinishedGoodsReceipt_ISERVICE.GetAllDropDownList(CompID, brnchID);

                List<ShopfloorListDropDown> list = new List<ShopfloorListDropDown>();
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    ShopfloorListDropDown shp = new ShopfloorListDropDown();
                    shp.shop_id = dr["shfl_id"].ToString();
                    shp.shop_name = dr["shfl_name"].ToString();
                    list.Add(shp);
                }


                List<OperationListDropDown> list1 = new List<OperationListDropDown>();
                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    OperationListDropDown operation = new OperationListDropDown();
                    operation.op_id = dr["op_id"].ToString();
                    operation.op_name = dr["op_name"].ToString();
                    list1.Add(operation);
                }
                list.Insert(0, new ShopfloorListDropDown() { shop_id = "0", shop_name = "---Select---" });
                list1.Insert(0, new OperationListDropDown() { op_id = "0", op_name = "---Select---" });
                FGR.ShopfloorList = list;
                FGR.OperationList = list1;

                List<ProductName> _productname = new List<ProductName>();
                _productname.Insert(0, new ProductName() { Item_id = "0", Item_name = "---Select---" });
                FGR.productNameList = _productname;
                GetItemListinDeatil(FGR);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
          

        }
        private void CommonPageDetails()
        {
            try
            {

                ComDeatil();
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
        public ActionResult AddFinishedGoodsReceiptDetail()
        {
            try {
                ComDeatil();
                FinishedGoodsReceiptModelDetail FGR = new FinishedGoodsReceiptModelDetail();
                FGR.Massage = null;
                FGR.TransType = "Save";
                FGR.Command = "AddNew";
                FGR.BtnName = "BtnAddNew";
                TempData["ModelData"] = FGR;
                UrlModel NewModel = new UrlModel();
                NewModel.Cmd = "AddNew";
                NewModel.tp = "Save";
                NewModel.bt = "BtnAddNew";
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                {
                    TempData["Message"] = "Financial Year not Exist";
                    return RedirectToAction("FinishedGoodsReceipt");
                }
                return RedirectToAction("FinishedGoodsReceiptDetail", "FinishedGoodsReceipt", NewModel);
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
         
        }
        public ActionResult FinishedGoodsReceiptDetail(UrlModel _urlModel)
        {
            try
            {             
                ComDeatil();
                CommonPageDetails();
                /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, brnchID, _urlModel.FGR_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var FGRDetailModel = TempData["ModelData"] as FinishedGoodsReceiptModelDetail;
                if(FGRDetailModel != null)
                {
                    if (FGRDetailModel.RecieptDate==null)
                    {
                        FGRDetailModel.RecieptDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    GetDeatilData(FGRDetailModel);
                    if (FGRDetailModel.TransType == "Update")                     
                    {
                        SetAllDataInView(FGRDetailModel);
                    }
                    else
                    {                      
                        ViewBag.DocumentCode = "0";                   
                        FGRDetailModel.DocumentStatus = "New";
                        FGRDetailModel.Docid = DocumentMenuId;
                        ViewBag.Command = FGRDetailModel.Command;
                        FGRDetailModel.batch_Command = FGRDetailModel.Command;
                        ViewBag.TransType = FGRDetailModel.TransType;
                        ViewBag.DocumentStatus = FGRDetailModel.DocumentStatus;
                    }                   
                        FGRDetailModel.Title = title;
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/FinishedGoodsReceipt/FinishedGoodsReceiptDetail.cshtml", FGRDetailModel);
                }
                else
                {
                    FinishedGoodsReceiptModelDetail Model = new FinishedGoodsReceiptModelDetail();
                    Model.RecieptDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                    SetUrlData(_urlModel, Model);
                    GetDeatilData(Model);
                    if (Model.TransType == "Update")
                    {
                        SetAllDataInView(Model);
                    }
                    else
                    {


                        ViewBag.DocumentCode = "0";
                        Model.DocumentStatus = "New";
                        Model.Docid = DocumentMenuId;
                        ViewBag.Command = Model.Command;
                        Model.batch_Command = Model.Command;
                        ViewBag.TransType = Model.TransType;
                        ViewBag.DocumentStatus = Model.DocumentStatus;

                    }
                    Model.Title = title;
                    return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/FinishedGoodsReceipt/FinishedGoodsReceiptDetail.cshtml", Model);
                }
                     
              
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public void SetAllDataInView(FinishedGoodsReceiptModelDetail _FGRModel)
        {
            try {
                ComDeatil();
                string rcpt_no = _FGRModel.RecieptNumber;
                string rcpt_dt = _FGRModel.RecieptDate;
                DataSet ds = _FinishedGoodsReceipt_ISERVICE.GetFGRDeatilData(CompID, brnchID, rcpt_no, rcpt_dt, UserID, DocumentMenuId);

                _FGRModel.RecieptNumber = ds.Tables[0].Rows[0]["rcpt_no"].ToString();
                _FGRModel.RecieptDate = ds.Tables[0].Rows[0]["rcpt_dt"].ToString();
                _FGRModel.operation_id = ds.Tables[0].Rows[0]["op_id"].ToString();
                _FGRModel.Operation = ds.Tables[0].Rows[0]["op_id"].ToString();
                _FGRModel.shopfloor_id = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                _FGRModel.Shopfloor = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                _FGRModel.SuppervisorName = ds.Tables[0].Rows[0]["supervisor_name"].ToString();
                _FGRModel.Remarks = ds.Tables[0].Rows[0]["rcpt_remarks"].ToString();
                _FGRModel.create_by = ds.Tables[0].Rows[0]["createname"].ToString();
                _FGRModel.create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                _FGRModel.mod_by = ds.Tables[0].Rows[0]["modname"].ToString();
                _FGRModel.mod_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                _FGRModel.app_by = ds.Tables[0].Rows[0]["appname"].ToString();
                _FGRModel.app_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                _FGRModel.Status = ds.Tables[0].Rows[0]["status_name"].ToString();

                _FGRModel.Source_type = ds.Tables[0].Rows[0]["src_type"].ToString();
                _FGRModel.Source_typeID = ds.Tables[0].Rows[0]["src_type"].ToString();
                _FGRModel.ProductName = ds.Tables[0].Rows[0]["product_Name"].ToString();
                _FGRModel.Product_id = ds.Tables[0].Rows[0]["prod_id"].ToString();               
                _FGRModel.UomName = ds.Tables[0].Rows[0]["uom_alias"].ToString();
                _FGRModel.Uom_id = ds.Tables[0].Rows[0]["uom_id"].ToString();


                _FGRModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                _FGRModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                string StatusCode = ds.Tables[0].Rows[0]["rcpt_status"].ToString().Trim();
                string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                ViewBag.SubItemDetails = ds.Tables[9];
                _FGRModel.StatusCode = StatusCode;
                _FGRModel.create_id = create_id;
                ViewBag.ConsumptionItemDetails = ds.Tables[1];
                ViewBag.OutputItemDetails = ds.Tables[2];
                ViewBag.ItemStockBatchWise = ds.Tables[3];
                ViewBag.ItemStockSerialWise = ds.Tables[4];
                ViewBag.SubItemDetails = ds.Tables[8];
                ViewBag.Cons_SubItemDetails = ds.Tables[9];
                ViewBag.DocumentCode = StatusCode;
                _FGRModel.DocumentStatus = StatusCode;
                _FGRModel.Title = title;
                ViewBag.Command = _FGRModel.Command;
                _FGRModel.Docid = DocumentMenuId;
                _FGRModel.batch_Command = _FGRModel.Command;
                ViewBag.TransType = _FGRModel.TransType;

                if (StatusCode == "C")
                {
                    _FGRModel.CancelFlag = true;
                    _FGRModel.BtnName = "Refresh";
                }
                else
                {
                    _FGRModel.CancelFlag = false;
                }
                if (StatusCode != "D" && StatusCode != "F")
                {
                    ViewBag.AppLevel = ds.Tables[7];
                }

                if (ViewBag.AppLevel != null && _FGRModel.Command != "Edit")
                {

                    var sent_to = "";
                    var nextLevel = "";
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        sent_to = ds.Tables[5].Rows[0]["sent_to"].ToString();
                    }

                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        nextLevel = ds.Tables[6].Rows[0]["nextlevel"].ToString().Trim();
                    }

                    if (StatusCode == "D")
                    {
                        if (create_id != UserID)
                        {
                            _FGRModel.BtnName = "Refresh";
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
                                _FGRModel.BtnName = "BtnEdit";
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
                                _FGRModel.BtnName = "BtnEdit";
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
                            _FGRModel.BtnName = "BtnEdit";
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
                                _FGRModel.BtnName = "BtnEdit";
                            }


                        }
                    }
                    if (StatusCode == "F")
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
                            _FGRModel.BtnName = "BtnEdit";
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
                            _FGRModel.BtnName = "BtnEdit";
                        }
                    }
                    if (StatusCode == "A")
                    {
                        if (create_id == UserID || approval_id == UserID)
                        {
                            _FGRModel.BtnName = "BtnEdit";

                        }
                        else
                        {
                            _FGRModel.BtnName = "Refresh";
                        }
                    }
                }
                if (ViewBag.AppLevel.Rows.Count == 0)
                {
                    if(_FGRModel.Command=="Edit")
                    {
                        ViewBag.Approve = "N";
                    }
                    else
                    {
                        ViewBag.Approve = "Y";
                    }
                   
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            
         
        }
        private void SetUrlData(UrlModel _urlModel, FinishedGoodsReceiptModelDetail Model)
        {
            if (_urlModel.tp != null)
            {
                Model.TransType = _urlModel.tp;
            }
            else
            {
                Model.TransType = "Refresh";
            }
            if (_urlModel.bt != null)
            {
                Model.BtnName = _urlModel.bt;
            }
            else
            {
                Model.BtnName = "BtnRefresh";
            }
            if (_urlModel.Cmd != null)
            {
                Model.Command = _urlModel.Cmd;
            }
            else
            {
                Model.Command = "Save";
            } 
            if (_urlModel.DMS != null)
            {
                Model.DocumentStatus = _urlModel.DMS;
            }
            else
            {
                Model.DocumentStatus = "";
            }
            if (_urlModel.FGR_no != null && _urlModel.FGR_no != "")
            {
                Model.RecieptNumber = _urlModel.FGR_no;
                Model.RecieptDate = _urlModel.FGR_dt;
            }

        }
        public void GetDeatilData(FinishedGoodsReceiptModelDetail FGRDetailModel)
        {                     
            BindAllDropDownDeatil( FGRDetailModel);
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                FGRDetailModel.ListFilterData1 = TempData["ListFilterData"].ToString();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFinishedGoodReceipt(FinishedGoodsReceiptModelDetail SaveDeatilModel, string command)
        {
            try
            {
                ComDeatil();
                var commCont = new CommonController(_Common_IServices);
                if (SaveDeatilModel.DeleteCommand == "Delete")
                    if (true)
                    {
                        command = "Delete";
                    }
                switch (command)
                {
                    case "AddNew":
                        FinishedGoodsReceiptModelDetail AddNewModel = new FinishedGoodsReceiptModelDetail();
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
                            if (!string.IsNullOrEmpty(SaveDeatilModel.RecieptNumber))
                
                                return RedirectToAction("FRGDblClick", new { rcpt_no = SaveDeatilModel.RecieptNumber, rcpt_dt = SaveDeatilModel.RecieptDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                            else
                                NewModel.Cmd = "Refresh";
                            NewModel.tp = "Refresh";
                            NewModel.bt = "BtnRefresh";
                            NewModel.DMS = null;
                            TempData["ModelData"] = NewModel;
                            return RedirectToAction("FinishedGoodsReceiptDetail", NewModel);
                        }
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("FinishedGoodsReceiptDetail", NewModel);
                    case "Save":
                        FGR_SaveUpdate(SaveDeatilModel);
                        if (SaveDeatilModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = SaveDeatilModel;
                        UrlModel SaveModel = new UrlModel();
                        SaveModel.bt = SaveDeatilModel.BtnName;
                        SaveModel.FGR_no = SaveDeatilModel.RecieptNumber;
                        SaveModel.FGR_dt = SaveDeatilModel.RecieptDate;
                        SaveModel.tp = SaveDeatilModel.TransType;
                        SaveModel.Cmd = SaveDeatilModel.Command;
                        TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                        return RedirectToAction("FinishedGoodsReceiptDetail", SaveModel);
                    case "Edit":
                        //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("FRGDblClick", new { rcpt_no = SaveDeatilModel.RecieptNumber, rcpt_dt = SaveDeatilModel.RecieptDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string FGRDt = SaveDeatilModel.RecieptDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, brnchID, FGRDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("FRGDblClick", new { rcpt_no = SaveDeatilModel.RecieptNumber, rcpt_dt = SaveDeatilModel.RecieptDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                        }
                        UrlModel EditModel = new UrlModel();
                        EditModel.Cmd = command;
                        SaveDeatilModel.Command = command;
                        SaveDeatilModel.BtnName = "BtnEdit";
                        SaveDeatilModel.TransType = "Update";
                        SaveDeatilModel.RecieptNumber = SaveDeatilModel.RecieptNumber;
                        SaveDeatilModel.RecieptDate = SaveDeatilModel.RecieptDate;
                        TempData["ModelData"] = SaveDeatilModel;
                        EditModel.tp = "Update";
                        EditModel.bt = "BtnEdit";
                        EditModel.FGR_no = SaveDeatilModel.RecieptNumber;
                        EditModel.FGR_dt = SaveDeatilModel.RecieptDate;
                        TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                        return RedirectToAction("FinishedGoodsReceiptDetail", EditModel);
                    case "Delete":
                        FinishGoodsReciptDelete(SaveDeatilModel, command);
                        FinishedGoodsReceiptModelDetail DeleteModel = new FinishedGoodsReceiptModelDetail();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnRefresh";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnRefresh";
                        TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                        return RedirectToAction("FinishedGoodsReceiptDetail", Delete_Model);
                    case "Approve":
                        //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("FRGDblClick", new { rcpt_no = SaveDeatilModel.RecieptNumber, rcpt_dt = SaveDeatilModel.RecieptDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string FGRDt1 = SaveDeatilModel.RecieptDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, brnchID, FGRDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("FRGDblClick", new { rcpt_no = SaveDeatilModel.RecieptNumber, rcpt_dt = SaveDeatilModel.RecieptDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                        }
                        FinishedGoodsReceipt_Approve(SaveDeatilModel, "", "");
                        TempData["ModelData"] = SaveDeatilModel;
                        UrlModel Approve = new UrlModel();
                        Approve.tp = "Update";
                        Approve.FGR_no = SaveDeatilModel.RecieptNumber;
                        Approve.FGR_dt = SaveDeatilModel.RecieptDate;
                        Approve.bt = "BtnEdit";
                        TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                        return RedirectToAction("FinishedGoodsReceiptDetail", Approve);
                    case "Forward":
                        TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                        //if (commCont.CheckFinancialYear(CompID, brnchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("FRGDblClick", new { rcpt_no = SaveDeatilModel.RecieptNumber, rcpt_dt = SaveDeatilModel.RecieptDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string FGRDt2 = SaveDeatilModel.RecieptDate;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, brnchID, FGRDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("FRGDblClick", new { rcpt_no = SaveDeatilModel.RecieptNumber, rcpt_dt = SaveDeatilModel.RecieptDate, ListFilterData = SaveDeatilModel.ListFilterData1, WF_Status = SaveDeatilModel.WFStatus });
                        }

                        return new EmptyResult();
                    case "Refresh":
                        FinishedGoodsReceiptModelDetail RefreshModel = new FinishedGoodsReceiptModelDetail();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel Refresh_Model = new UrlModel();
                        Refresh_Model.tp = "Save";
                        Refresh_Model.bt = "BtnRefresh";
                        Refresh_Model.Cmd = command;
                        TempData["ListFilterData"] = RefreshModel.ListFilterData1;
                        return RedirectToAction("FinishedGoodsReceiptDetail", Refresh_Model);
                    case "Print":
                        //return new EmptyResult();
                        return GenratePdfFile(SaveDeatilModel);
                    case "BacktoList":
                        var WF_status = SaveDeatilModel.WF_Status1;
                        TempData["ListFilterData"] = SaveDeatilModel.ListFilterData1;
                        return RedirectToAction("FinishedGoodsReceipt",new { WF_status });

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
        public FileResult GenratePdfFile(FinishedGoodsReceiptModelDetail _Model)
        {
            var data = GetPdfData(_Model.RecieptNumber, _Model.RecieptDate);
            if (data != null)
                return File(data, "application/pdf", "FinishedGoodsReceiptPrint.pdf");
            else
                return File("ErrorPage", "application/pdf");

        }
        public byte[] GetPdfData(string confirmationNo, string confirmationDate)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            try
            {
                ComDeatil();
                DataSet Deatils = new DataSet();
                Deatils = _FinishedGoodsReceipt_ISERVICE.GetProductionConfirmationPrintDeatils(CompID, brnchID, confirmationNo, confirmationDate);

                ViewBag.PageName = "FGR";
                ViewBag.Title = "Finished Goods Receipt";
                ViewBag.Details = Deatils;
                ViewBag.CompLogoDtl = Deatils.Tables[0];
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                //ViewBag.DocStatus = "D";

                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/FinishedGoodsReceipt/FinishedGoodsReceiptPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(GLVoucherHtml);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    pdfDoc.Close();
                    Byte[] bytes = stream.ToArray();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLDITALIC, BaseColor.BLACK);
                    string draftImage = Server.MapPath("~/Content/Images/draft.png");
                    using (var reader1 = new PdfReader(bytes))
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (var stamper = new PdfStamper(reader1, ms))
                            {
                                var draftimg = Image.GetInstance(draftImage);
                                draftimg.SetAbsolutePosition(0, 160);
                                draftimg.ScaleAbsolute(580f, 580f);

                                int PageCount = reader1.NumberOfPages;
                                for (int i = 1; i <= PageCount; i++)
                                {
                                    var content = stamper.GetUnderContent(i);
                                    if (ViewBag.DocStatus == "D" || ViewBag.DocStatus == "F")
                                    {
                                        content.AddImage(draftimg);
                                    }
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

        /*--------Print End--------*/
        public string SavePdfDocToSendOnEmailAlert(string confirmationNo, string confirmationDate, string fileName)
        {
            var data = GetPdfData(confirmationNo, confirmationDate);
            var commonCont = new CommonController(_Common_IServices);
            return commonCont.SaveAlertDocument(data, fileName);
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status1)
        {

            FinishedGoodsReceiptModelDetail _model = new FinishedGoodsReceiptModelDetail();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _model.RecieptNumber = jObjectBatch[i]["DocNo"].ToString();
                    _model.RecieptDate = jObjectBatch[i]["DocDate"].ToString();
                    _model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_model.A_Status != "Approve")
            {
                _model.A_Status = "Approve";
            }
            FinishedGoodsReceipt_Approve(_model, ListFilterData1, WF_Status1);
            UrlModel ApproveModel = new UrlModel();
            if (WF_Status1 != null && WF_Status1 != "")
            {
                ApproveModel.wf = WF_Status1;
            }
            TempData["ModelData"] = _model;
            ApproveModel.tp = "Update";
            ApproveModel.FGR_no = _model.RecieptNumber;
            ApproveModel.FGR_dt = _model.RecieptDate;
            ApproveModel.bt = "BtnEdit";
            return RedirectToAction("FinishedGoodsReceiptDetail", ApproveModel);
        }
        public ActionResult FinishedGoodsReceipt_Approve(FinishedGoodsReceiptModelDetail _model, string ListFilterData1, string WF_Status1)
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
            try
            {
                ComDeatil();
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string rcpt_no = _model.RecieptNumber;
                string rcpt_dt = _model.RecieptDate;
                string A_Status = _model.A_Status;
                string A_Level = _model.A_Level;
                string A_Remarks = _model.A_Remarks;
                string Message = _FinishedGoodsReceipt_ISERVICE.Approve_FinishedGoodsReceipt(CompID, brnchID, DocumentMenuId, rcpt_no, rcpt_dt, UserID, mac_id, A_Status, A_Level, A_Remarks);
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
                        //try
                        //{
                        //    string fileName = "FGR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        //    var filePath = SavePdfDocToSendOnEmailAlert(_model.RecieptNumber, _model.RecieptDate, fileName);
                        //    _Common_IServices.SendAlertEmail(CompID, brnchID, DocumentMenuId, _model.RecieptNumber, "A", UserID, "", filePath);
                        //}
                        //catch (Exception exMail)
                        //{
                        //    _model.Message = "ErrorInMail";
                        //    string path = Server.MapPath("~");
                        //    Errorlog.LogError(path, exMail);
                        //}
                        _model.Message = "Approved";
                        //_model.Message = _model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    }
                }
             
                //if (Message == "StockNotAvail")
                //{
                //    //Session["Message"] = "StockNotAvail";
                //    _model.Message = "StockNotAvail";
                //}
               
                UrlModel ApproveModel = new UrlModel();
                _model.RecieptNumber = rcpt_no;
                _model.RecieptDate = rcpt_dt;
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
                ApproveModel.FGR_no = _model.RecieptNumber;
                ApproveModel.FGR_dt = _model.RecieptDate;
                ApproveModel.bt = "BtnEdit";
                ApproveModel.Cmd = "Approve";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("FinishedGoodsReceiptDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData)
        {
            //Session["Message"] = "";
            FinishedGoodsReceiptModelDetail ToRefreshByJS = new FinishedGoodsReceiptModelDetail();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.RecieptNumber = a[0].Trim();
            ToRefreshByJS.RecieptDate = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnEdit";
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wf = a[2].Trim();
            }
            Model.bt = "BtnEdit";
            Model.FGR_no = ToRefreshByJS.RecieptNumber;
            Model.FGR_dt = ToRefreshByJS.RecieptDate;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("FinishedGoodsReceiptDetail", Model);
        }
        private ActionResult FinishGoodsReciptDelete(FinishedGoodsReceiptModelDetail _model, string command)
        {
            try
            {
                ComDeatil();
                DataSet Message = _FinishedGoodsReceipt_ISERVICE.Delete_FinishGoodsRecipt(CompID, brnchID, _model.RecieptNumber, _model.RecieptDate);
                return RedirectToAction("FinishedGoodsReceiptDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);              
                throw ex;
            }
        }
        public ActionResult FGR_SaveUpdate(FinishedGoodsReceiptModelDetail _model)
        {
            if (Session["UserId"].ToString() != null)
            {
                UserID = Session["UserId"].ToString();
            }
            string SaveMessage = "";
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_model.CancelFlag == false)
                {
                    ComDeatil();
                    DataTable FGRHeader = new DataTable();
                    DataTable InputFGRItemDetails = new DataTable();
                    DataTable OutputItemDetails = new DataTable();
                    DataTable ItemBatchDetails = new DataTable();
                    DataTable ItemSerialDetails = new DataTable();
                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(string));
                    dtheader.Columns.Add("rcpt_no", typeof(string));
                    dtheader.Columns.Add("rcpt_dt", typeof(string));
                    dtheader.Columns.Add("op_id", typeof(string));
                    dtheader.Columns.Add("shfl_id", typeof(string));
                    dtheader.Columns.Add("supervisor_name", typeof(string));
                    dtheader.Columns.Add("rcpt_remarks", typeof(string));
                    dtheader.Columns.Add("create_id", typeof(string));
                    dtheader.Columns.Add("rcpt_status", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));
                    dtheader.Columns.Add("src_type", typeof(string));
                    dtheader.Columns.Add("prod_id", typeof(string));
                    dtheader.Columns.Add("uom_id", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;

                    if (_model.RecieptNumber != null)
                    {
                        dtrowHeader["TransType"] = "Update";
                    }
                    else
                    {
                        dtrowHeader["TransType"] = "Save";
                    }
                    dtrowHeader["comp_id"] = CompID;

                    dtrowHeader["br_id"] = brnchID;
                    dtrowHeader["rcpt_no"] = _model.RecieptNumber;
                    dtrowHeader["rcpt_dt"] = _model.RecieptDate;
                    dtrowHeader["op_id"] = _model.operation_id;
                    dtrowHeader["shfl_id"] = _model.shopfloor_id;
                    dtrowHeader["supervisor_name"] = _model.SuppervisorName;
                    dtrowHeader["rcpt_remarks"] = _model.Remarks;
                    dtrowHeader["create_id"] = UserID;
                    dtrowHeader["rcpt_status"] = _model.StatusCode;
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    dtrowHeader["mac_id"] = SystemDetail;
                    dtrowHeader["src_type"] = _model.Source_typeID;
                    dtrowHeader["prod_id"] = _model.Product_id;
                    dtrowHeader["uom_id"] = _model.Uom_id;
                    dtheader.Rows.Add(dtrowHeader);
                    FGRHeader = dtheader;


                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("cons_qty", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));
                    dtItem.Columns.Add("item_type", typeof(string));

                    JArray jObject = JArray.Parse(_model.InputItemDetail);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                        dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                        dtrowLines["cons_qty"] = jObject[i]["ConsumedQuantity"].ToString();
                        dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                        dtrowLines["item_type"] = jObject[i]["item_type"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    InputFGRItemDetails = dtItem;

                    DataTable dtOutputItem = new DataTable();
                    dtOutputItem.Columns.Add("item_id", typeof(string));
                    dtOutputItem.Columns.Add("uom_id", typeof(int));
                    dtOutputItem.Columns.Add("out_qty", typeof(string));
                    dtOutputItem.Columns.Add("cost_price", typeof(string));
                    dtOutputItem.Columns.Add("lot_no", typeof(string));
                    dtOutputItem.Columns.Add("batch_no", typeof(string));
                    dtOutputItem.Columns.Add("exp_dt", typeof(string));
                    dtOutputItem.Columns.Add("remarks", typeof(string));
                    dtOutputItem.Columns.Add("item_type", typeof(string));
                    dtOutputItem.Columns.Add("QC_req", typeof(string));

                    JArray jOutputObject = JArray.Parse(_model.OutputItemDetail);
                    for (int i = 0; i < jOutputObject.Count; i++)
                    {
                        DataRow dtOutputrowLines = dtOutputItem.NewRow();
                        dtOutputrowLines["item_id"] = jOutputObject[i]["Item_id"].ToString();
                        dtOutputrowLines["uom_id"] = jOutputObject[i]["UOMId"].ToString();
                        dtOutputrowLines["out_qty"] = jOutputObject[i]["OutputQuantity"].ToString();
                        dtOutputrowLines["cost_price"] = jOutputObject[i]["CostPrice"].ToString();
                        dtOutputrowLines["lot_no"] = jOutputObject[i]["lot"].ToString();
                        dtOutputrowLines["batch_no"] = jOutputObject[i]["BatchNo"].ToString();
                        dtOutputrowLines["exp_dt"] = jOutputObject[i]["ExDate"].ToString();
                        dtOutputrowLines["remarks"] = jOutputObject[i]["remarks"].ToString();
                        dtOutputrowLines["item_type"] = jOutputObject[i]["item_type"].ToString();
                        dtOutputrowLines["QC_req"] = jOutputObject[i]["QC_req"].ToString();
                        dtOutputItem.Rows.Add(dtOutputrowLines);
                    }
                    OutputItemDetails = dtOutputItem;
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
                    /*------------------Sub Item ----------------------*/
                    DataTable OutputdtSubItem = new DataTable();
                    OutputdtSubItem.Columns.Add("item_id", typeof(string));
                    OutputdtSubItem.Columns.Add("sub_item_id", typeof(string));
                    OutputdtSubItem.Columns.Add("qty", typeof(string));

                    if (_model.OutPutSubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_model.OutPutSubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = OutputdtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            OutputdtSubItem.Rows.Add(dtrowItemdetails);
                        }
                    }
                    /*------------------Sub Item end----------------------*/
                    /*------------------Sub Item ----------------------*/
                    DataTable InputdtSubItem = new DataTable();
                    InputdtSubItem.Columns.Add("item_id", typeof(string));
                    InputdtSubItem.Columns.Add("sub_item_id", typeof(string));
                    InputdtSubItem.Columns.Add("qty", typeof(string));

                    if (_model.InputSubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_model.InputSubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = InputdtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            InputdtSubItem.Rows.Add(dtrowItemdetails);
                        }
                    }

                    /*------------------Sub Item end----------------------*/
                    SaveMessage = _FinishedGoodsReceipt_ISERVICE.InsertUpdate_FinishGoodRecipt(FGRHeader, InputFGRItemDetails, OutputItemDetails, ItemBatchDetails, ItemSerialDetails, OutputdtSubItem, InputdtSubItem);
                    string[] FData = SaveMessage.Split(',');

                    string Message = FData[0].ToString();
                    string recpt_Number = FData[1].ToString();
                    // string Pcnf_Number = PCnfNumber.Replace("/", "");
                    if (Message == "Data_Not_Found")
                    {
                        //var a = SaveMessage.Split(',');
                        var msgs = Message.Replace("_", " ") + " " + recpt_Number + " in " + PageName;//ProdOrdCode is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msgs, "", "");
                        _model.Message = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("FinishedGoodsReceiptDetail");
                    }
                    if (Message == "Update" || Message == "Save")
                    {

                        _model.Message = "Save";
                        _model.RecieptNumber = recpt_Number;
                        _model.RecieptDate = _model.RecieptDate;
                        _model.TransType = "Update";
                        _model.BtnName = "BtnEdit";
                    }
                    return RedirectToAction("FinishedGoodsReceiptDetail");
                }
                else
                {

                    ComDeatil();
                    string br_id = brnchID;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet message = _FinishedGoodsReceipt_ISERVICE.Cancel_FinishGoodsReceipt(CompID, br_id, _model.RecieptNumber, _model.RecieptDate, UserID, DocumentMenuId, mac_id);

                    //try
                    //{
                    //    string file_Name = "FGR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    //    var filePath = SavePdfDocToSendOnEmailAlert(_model.RecieptNumber, _model.RecieptDate, file_Name);
                    //    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _model.RecieptNumber, "C", UserID, "", filePath);
                    //}
                    //catch (Exception exMail)
                    //{
                    //    _model.Message = "ErrorInMail";
                    //    string path = Server.MapPath("~");
                    //    Errorlog.LogError(path, exMail);
                    //}
                    //_model.Message = _model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";

                    _model.Message = message.Tables[0].Rows[0]["result"].ToString();
                    _model.RecieptNumber = _model.RecieptNumber;
                    _model.RecieptDate = _model.RecieptDate;
                    _model.TransType = "Update";
                    _model.BtnName = "Refresh";

                    string fileName = "FGR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                   // var filePath = SavePdfDocToSendOnEmailAlert(_model.RecieptDate, _model.RecieptDate, fileName);
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _model.RecieptNumber, "C", UserID, "0" /*filePath*/);
                  
                    return RedirectToAction("FinishedGoodsReceiptDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        public ActionResult GetSubItemDetails(string Item_id, string Uom_id, string Shfl_id, string SubItemListwithPageData, string IsDisabled
            , string Flag, string Status, string rcpt_no, string rcpt_dt)
        {
            ComDeatil();
            DataTable dt = new DataTable();
            if (Flag == "Input" || Flag == "OutPut")
            {
                if (Status == "D" || Status == "F" || Status == "")
                {
                    if (Flag == "OutPut")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    }
                    else
                    {
                        dt = _FinishedGoodsReceipt_ISERVICE.GetConsumeSubItemShflAvlstockDetails(CompID, brnchID, Item_id, Uom_id, Shfl_id).Tables[0];
                    }

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
                   
                        dt = _FinishedGoodsReceipt_ISERVICE.GetOutputDeatilSubitm(CompID, brnchID, Item_id, Uom_id, Shfl_id, rcpt_no, rcpt_dt, Flag).Tables[0];
                   
                }
            }
            else
            {
                dt = _FinishedGoodsReceipt_ISERVICE.GetOutputDeatilSubitm(CompID, brnchID, Item_id, Uom_id, Shfl_id, rcpt_no, rcpt_dt, "QC").Tables[0];
            }
            SubItemPopupDt subitmModel = new SubItemPopupDt
            {
                Flag = Flag,
                dt_SubItemDetails = dt,
                IsDisabled = IsDisabled,
            };
            return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
        }
        public ActionResult getItemStockBatchWise(string ItemId, string ShflId, string DocStatus, string SelectedItemdetail
          , string Transtype, string cmd, string uom_id)
        {
            try
            {
                DataSet ds = new DataSet();
                ComDeatil();
                if (ItemId != "")
                {
                    ds = _FinishedGoodsReceipt_ISERVICE.getItemStockBatchWise(CompID, brnchID, ItemId, ShflId, uom_id);
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
        public ActionResult getItemStockBatchWiseAfterStockUpadte(string FGR_NO, string FGR_dt, string ItemID, string Transtype, string cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                ComDeatil();
                ds = _FinishedGoodsReceipt_ISERVICE.getItemStockBatchWiseAfterStockUpdate(CompID, brnchID, FGR_NO, FGR_dt, ItemID);
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
        public ActionResult GetAvlStock(string itemid, string shopflore)
        {
            try
            {
                JsonResult DataRows = null;
                DataSet ds = new DataSet();
                ComDeatil();
                ds = _FinishedGoodsReceipt_ISERVICE.GetAvlStock(CompID, brnchID, itemid, shopflore);
                DataRows = Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);

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
        public ActionResult getItemstockSerialWise(string ItemId, string ShflID, string DocStatus, string SelectedItemSerial, string Transtype, string cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                ComDeatil();
                ds = _FinishedGoodsReceipt_ISERVICE.getItemstockSerialWise(CompID, brnchID, ItemId, ShflID);

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
        public ActionResult getItemstockSerialWiseAfterStockUpadte(string rcpt_no, string rcpt_dt, string ItemID, string Transtype, string cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                ComDeatil();
                ds = _FinishedGoodsReceipt_ISERVICE.getItemstockSerialWiseAfterStockUpdate(CompID, brnchID, rcpt_no, rcpt_dt, ItemID);
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

        public ActionResult GetItemList(FinishedGoodsReceiptModel queryParameters)
        {
            ComDeatil();
            DataSet itemList1 = new DataSet();
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.ProductName))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.ProductName;
                }
                itemList1 = _FinishedGoodsReceipt_ISERVICE.ItemList(ItemName, CompID, brnchID);

                List<ProductName> _ItemList = new List<ProductName>();
                foreach (DataRow data in itemList1.Tables[0].Rows)
                {
                    ProductName _ItemDetail = new ProductName();
                    _ItemDetail.Item_id = data["Item_id"].ToString();
                    _ItemDetail.Item_name = data["Item_name"].ToString();
                    _ItemList.Add(_ItemDetail);
                }
                queryParameters.productNameList = _ItemList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(JsonConvert.SerializeObject(itemList1), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemListinDeatil(FinishedGoodsReceiptModelDetail queryParameters)
        {
            ComDeatil();
            DataSet ItemName1 = new DataSet();
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.ProductName))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.ProductName;
                }
                ItemName1 = _FinishedGoodsReceipt_ISERVICE.ItemList(ItemName, CompID, brnchID);

                //List<ProductName> _ItemList = new List<ProductName>();
                //foreach (var data in ItemList)
                //{
                //    ProductName _ItemDetail = new ProductName();
                //    _ItemDetail.Item_id = data.Key;
                //    _ItemDetail.Item_name = data.Value;
                //    _ItemList.Add(_ItemDetail);
                //}
                //queryParameters.productNameList = _ItemList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(JsonConvert.SerializeObject(ItemName1), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOperationData(string itemID)
        {
            DataSet Data = new DataSet();
            ComDeatil();
            Data = _FinishedGoodsReceipt_ISERVICE.OperationData(itemID, CompID, brnchID);
            return Json(JsonConvert.SerializeObject(Data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBomitemData(string itemID,string operation_ID,string shop_floor, string flag)
        {
            DataSet Data = new DataSet();
            ComDeatil();
            Data = _FinishedGoodsReceipt_ISERVICE.GetBomDeatilData(itemID, operation_ID, shop_floor, CompID, brnchID, flag);
            return Json(JsonConvert.SerializeObject(Data), JsonRequestBehavior.AllowGet);
        }
    }
}