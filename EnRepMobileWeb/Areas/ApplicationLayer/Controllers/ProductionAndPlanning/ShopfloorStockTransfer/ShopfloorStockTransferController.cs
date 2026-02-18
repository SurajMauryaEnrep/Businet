using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ShopfloorStockTransfer;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ShopfloorStockTransfer;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.ShopfloorStockTransfer
{
    public class ShopfloorStockTransferController : Controller
    {
        string CompID, Br_ID, UserID, language = String.Empty;
        string DocumentMenuId = "105105140", title, FromDate;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ShopfloorStockTransfer_ISERVICES _ShopfloorStockTransfer_ISERVICES;
        public ShopfloorStockTransferController(Common_IServices _Common_IServices, ShopfloorStockTransfer_ISERVICES _ShopfloorStockTransfer_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._ShopfloorStockTransfer_ISERVICES = _ShopfloorStockTransfer_ISERVICES;

        }
        // GET: ApplicationLayer/ShopfloorStockTransfer
        public ActionResult ShopfloorStockTransfer(string WF_Status)
        {
            ShopfloorStockTransfer_Model stockTransfer_Model = new ShopfloorStockTransfer_Model();
            stockTransfer_Model.WF_Status = WF_Status;
            ViewBag.MenuPageName = getDocumentName();
            stockTransfer_Model.Title = title;
            ViewBag.VBRoleList = GetRoleList();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;

            //string endDate = dtnow.ToString("yyyy-MM-dd");
            var other = new CommonController(_Common_IServices);
            ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
            ViewBag.DocumentMenuId = DocumentMenuId;
            GetStatusList(stockTransfer_Model);
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                var TransferType= a[0].Trim();
                var Materialtype = a[1].Trim();
                var Fromdate = a[2].Trim();
                var Todate = a[3].Trim();
                var Status = a[4].Trim();
                DataTable dt = _ShopfloorStockTransfer_ISERVICES.GetSHFLTransferListFiltered(TransferType, Materialtype, Fromdate, Todate, Status, CompID, Br_ID, DocumentMenuId);
                ViewBag.SHFLTransferList = dt;
                stockTransfer_Model.ListFilterData = TempData["ListFilterData"].ToString();
                stockTransfer_Model.TransferType = TransferType;
                stockTransfer_Model.MaterialType = Materialtype;
                stockTransfer_Model.FromDate = Fromdate;
                stockTransfer_Model.ToDate = Todate;
                stockTransfer_Model.status = Status;
            }
            else
            {
                stockTransfer_Model.FromDate = startDate;
                stockTransfer_Model.ToDate = CurrentDate;
                DataTable Dt = GetSHFLTransferDetails(stockTransfer_Model);
                
                ViewBag.SHFLTransferList = Dt;
            }
            //Session["SSTSearch"] = "0";
            stockTransfer_Model.SSTSearch = "0";
            //Session["BtnNameSST"] = "BtnAddNew";
            stockTransfer_Model.BtnName = "BtnAddNew";
            CommonPageDetails();
            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorStockTransfer/ShopfloorStockTransferList.cshtml", stockTransfer_Model);
        }
        public ActionResult ShopfloorStockTransfer_Edit(string Trf_Number, string Trf_Date, string ListFilterData,string WF_Status)
        {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            //Session["shfltrfMessage"] = "New";
            //Session["Command"] = "Update";
            //Session["trf_no"] = Trf_Number;
            //Session["trf_dt"] = Trf_Date;
            //Session["TransType"] = "Update";
            ////Session["AppStatus"] = 'D';
            //Session["BtnNameSST"] = "BtnEdit";
            ShopfloorStockTransfer_Model dblclick = new ShopfloorStockTransfer_Model();
            UrlModel dbl_Click = new UrlModel();
            dblclick.SHPF_No = Trf_Number;
            dblclick.SHPF_Dt = Trf_Date;
            dblclick.TransType = "Update";
            dblclick.BtnName = "BtnEdit";
            dblclick.Command = "Refresh";
            if (WF_Status != null && WF_Status != "")
            {
                dbl_Click.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            //_url.Cmd = "Update";
            dbl_Click.tp = "Update";
            dbl_Click.bt = "BtnEdit";
            dbl_Click.SHPF_No = Trf_Number;
            dbl_Click.SHPF_Dt = Trf_Date;
            dbl_Click.Cmd = "Refresh";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("ShopfloorStockTransferDetail", dbl_Click);
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, Br_ID, UserID, DocumentMenuId, language);
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
        private DataTable GetSHFLTransferDetails(ShopfloorStockTransfer_Model List_Model)
        {
            //ShopfloorStockTransfer_Model List_Model = new ShopfloorStockTransfer_Model();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                Br_ID = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            
            string wfstatus = "";
            //if (Session["WF_status"] != null)
            //{
            //    wfstatus = Session["WF_status"].ToString();
            //}
            if(List_Model.WF_Status != null)
            {
                wfstatus = List_Model.WF_Status;
            }
            else
            {
                wfstatus = "";
            }

            DataSet dt = _ShopfloorStockTransfer_ISERVICES.GetSHFLTransferList(CompID, Br_ID, UserID, wfstatus, DocumentMenuId, List_Model.FromDate, List_Model.ToDate);
            if (dt.Tables[1].Rows.Count > 0)
            {
               // FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
            }
            return dt.Tables[0];
        }
        public ActionResult GetSHFLTransferListFiltered(string TransferType, string Materialtype, string Fromdate, string Todate, string Status)
        {
            try
            {
                ShopfloorStockTransfer_Model Serch_model = new ShopfloorStockTransfer_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                //Session["WF_status"] = null;
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                DataTable dt = _ShopfloorStockTransfer_ISERVICES.GetSHFLTransferListFiltered(TransferType,Materialtype, Fromdate, Todate, Status, CompID, Br_ID, DocumentMenuId);
                //Session["SSTSearch"] = "SSTSearch";
                Serch_model.SSTSearch = "SSTSearch";
                ViewBag.SHFLTransferList = dt;
                CommonPageDetails();
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_ShopfloorStockTransferList.cshtml", Serch_model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }

        }
        public void GetStatusList(ShopfloorStockTransfer_Model stockTransfer_Model)
        {
            try
            {
                //var DocumentMenuId = "105102120";
                List<LStatus> statusLists = new List<LStatus>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new LStatus { status_id = x.status_id, status_name = x.status_name });
                stockTransfer_Model.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        private DataTable GetRoleList()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult AddShopfloorStockTransferDetail()
        {
            //Session["shfltrfMessage"] = "New";
            //Session["Command"] = "Add";
            ////Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnNameSST"] = "BtnAddNew";
            ShopfloorStockTransfer_Model AddNewModel = new ShopfloorStockTransfer_Model();
            AddNewModel.Command = "New";
            AddNewModel.TransType = "Save";
            AddNewModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = AddNewModel;
            UrlModel AddNew_Model = new UrlModel();
            AddNew_Model.bt = "BtnAddNew";
            AddNew_Model.Cmd = "New";
            AddNew_Model.tp = "Save";
            ViewBag.MenuPageName = getDocumentName();
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                Br_ID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ShopfloorStockTransfer");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("ShopfloorStockTransferDetail", "ShopfloorStockTransfer", AddNew_Model);
        }
        public ActionResult ShopfloorStockTransferDetail(UrlModel _urlModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                /*Add by Hina sharma on 12-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, Br_ID, _urlModel.SHPF_Dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var st_Model= TempData["ModelData"] as ShopfloorStockTransfer_Model;
                if(st_Model != null)
                {
                    ViewBag.MenuPageName = getDocumentName();
                    st_Model.Title = title;
                    ViewBag.VBRoleList = GetRoleList();
                    st_Model.trf_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                   
                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        st_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (st_Model.TransType == "Update")
                    {
                        //string trf_no = Session["trf_no"].ToString();
                        //string trf_dt = Session["trf_dt"].ToString();
                        string trf_no = st_Model.SHPF_No;
                        string trf_dt = st_Model.SHPF_Dt;
                        DataSet ds = _ShopfloorStockTransfer_ISERVICES.GetShopfloorStockTransferDetailByNo(CompID, Br_ID, trf_no, trf_dt, UserID, DocumentMenuId);

                        st_Model.trf_no = ds.Tables[0].Rows[0]["trf_no"].ToString();
                        st_Model.trf_dt = ds.Tables[0].Rows[0]["trf_dt"].ToString();
                        st_Model.TransferType = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        st_Model.MaterialType = ds.Tables[0].Rows[0]["material_type"].ToString();
                        st_Model.shopfloorSource_id = ds.Tables[0].Rows[0]["src"].ToString();
                        st_Model.Destnation_id = ds.Tables[0].Rows[0]["dstn"].ToString();
                        st_Model.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        st_Model.create_by = ds.Tables[0].Rows[0]["create_name"].ToString();
                        st_Model.create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        st_Model.mod_by = ds.Tables[0].Rows[0]["mod_name"].ToString();
                        st_Model.mod_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        st_Model.app_by = ds.Tables[0].Rows[0]["app_name"].ToString();
                        st_Model.app_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        st_Model.StatusCode = ds.Tables[0].Rows[0]["trf_status"].ToString();
                        st_Model.status = ds.Tables[0].Rows[0]["status_name"].ToString();

                        st_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        st_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        string StatusCode = ds.Tables[0].Rows[0]["trf_status"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        st_Model.StatusCode = StatusCode;
                        st_Model.create_id = create_id;
                        //if (StatusCode == "C")
                        //{
                        //    st_Model.CancelFlag = true;
                        //    Session["BtnNameSST"] = "Refresh";
                        //}
                        //else
                        //{
                        //    st_Model.CancelFlag = false;
                        //}
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && st_Model.Command != "Edit")
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

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnNameSST"] = "Refresh";
                                    st_Model.BtnName = "Refresh";
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
                                        //Session["BtnNameSST"] = "BtnEdit";
                                        st_Model.BtnName = "BtnEdit";
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
                                        //Session["BtnNameSST"] = "BtnEdit";
                                        st_Model.BtnName = "BtnEdit";
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
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnNameSST"] = "BtnEdit";
                                    st_Model.BtnName = "BtnEdit";
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
                                        //Session["BtnNameSST"] = "BtnEdit";
                                        st_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnNameSST"] = "BtnEdit";
                                    st_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnNameSST"] = "BtnEdit";
                                    st_Model.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnNameSST"] = "BtnEdit";
                                    st_Model.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    //Session["BtnNameSST"] = "Refresh";
                                    st_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.SubItemDetails = ds.Tables[7];
                        ViewBag.ItemStockBatchWise = ds.Tables[2];
                        ViewBag.ItemStockSerialWise = ds.Tables[3];
                        ViewBag.DocumentCode = StatusCode;
                        //ViewBag.Approve = "Y";
                        //Session["DocumentStatus"] = StatusCode;
                        st_Model.DocumentStatus = StatusCode;
                        st_Model.Cmd_batch = st_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorStockTransfer/ShopfloorStockTransferDetail.cshtml", st_Model);
                    }
                    else
                    {
                        ViewBag.Approve = "Y";
                        ViewBag.DocumentCode = "0";
                        //Session["DocumentStatus"] = "New";
                        st_Model.DocumentStatus = "New";
                        st_Model.Cmd_batch = st_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorStockTransfer/ShopfloorStockTransferDetail.cshtml", st_Model);
                    }
                }
                else
                {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        Br_ID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    ShopfloorStockTransfer_Model stockTransfer_Model = new ShopfloorStockTransfer_Model();
                    if (_urlModel != null)
                    {
                        stockTransfer_Model.BtnName = _urlModel.bt;
                        stockTransfer_Model.SHPF_No = _urlModel.SHPF_No;
                        stockTransfer_Model.SHPF_Dt = _urlModel.SHPF_Dt;
                        stockTransfer_Model.Command = _urlModel.Cmd;
                        stockTransfer_Model.TransType = _urlModel.tp;
                        stockTransfer_Model.WF_Status1 = _urlModel.wf;
                        stockTransfer_Model.DocumentStatus = _urlModel.DMS;
                    }
                    ViewBag.MenuPageName = getDocumentName();
                    stockTransfer_Model.Title = title;
                    ViewBag.VBRoleList = GetRoleList();
                    stockTransfer_Model.trf_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                    var other = new CommonController(_Common_IServices);
                    ViewBag.AppLevel = other.GetApprovalLevel(CompID, Br_ID, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        stockTransfer_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (stockTransfer_Model.TransType == "Update")
                    {
                        //string trf_no = Session["trf_no"].ToString();
                        //string trf_dt = Session["trf_dt"].ToString();
                        string trf_no = stockTransfer_Model.SHPF_No;
                        string trf_dt = stockTransfer_Model.SHPF_Dt;
                        DataSet ds = _ShopfloorStockTransfer_ISERVICES.GetShopfloorStockTransferDetailByNo(CompID, Br_ID, trf_no, trf_dt, UserID, DocumentMenuId);

                        stockTransfer_Model.trf_no = ds.Tables[0].Rows[0]["trf_no"].ToString();
                        stockTransfer_Model.trf_dt = ds.Tables[0].Rows[0]["trf_dt"].ToString();
                        stockTransfer_Model.TransferType = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        stockTransfer_Model.MaterialType = ds.Tables[0].Rows[0]["material_type"].ToString();
                        stockTransfer_Model.shopfloorSource_id = ds.Tables[0].Rows[0]["src"].ToString();
                        stockTransfer_Model.Destnation_id = ds.Tables[0].Rows[0]["dstn"].ToString();
                        stockTransfer_Model.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                        stockTransfer_Model.create_by = ds.Tables[0].Rows[0]["create_name"].ToString();
                        stockTransfer_Model.create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        stockTransfer_Model.mod_by = ds.Tables[0].Rows[0]["mod_name"].ToString();
                        stockTransfer_Model.mod_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        stockTransfer_Model.app_by = ds.Tables[0].Rows[0]["app_name"].ToString();
                        stockTransfer_Model.app_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        stockTransfer_Model.StatusCode = ds.Tables[0].Rows[0]["trf_status"].ToString();
                        stockTransfer_Model.status = ds.Tables[0].Rows[0]["status_name"].ToString();

                        stockTransfer_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        stockTransfer_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        string StatusCode = ds.Tables[0].Rows[0]["trf_status"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        stockTransfer_Model.StatusCode = StatusCode;
                        stockTransfer_Model.create_id = create_id;
                        //if (StatusCode == "C")
                        //{
                        //    stockTransfer_Model.CancelFlag = true;
                        //    Session["BtnNameSST"] = "Refresh";
                        //}
                        //else
                        //{
                        //    stockTransfer_Model.CancelFlag = false;
                        //}
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && stockTransfer_Model.Command != "Edit")
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

                            if (StatusCode == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnNameSST"] = "Refresh";
                                    stockTransfer_Model.BtnName = "Refresh";
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
                                        //Session["BtnNameSST"] = "BtnEdit";
                                        stockTransfer_Model.BtnName = "BtnEdit";
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
                                        //Session["BtnNameSST"] = "BtnEdit";
                                        stockTransfer_Model.BtnName = "BtnEdit";
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
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnNameSST"] = "BtnEdit";
                                    stockTransfer_Model.BtnName = "BtnEdit";
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
                                        //Session["BtnNameSST"] = "BtnEdit";
                                        stockTransfer_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnNameSST"] = "BtnEdit";
                                    stockTransfer_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnNameSST"] = "BtnEdit";
                                    stockTransfer_Model.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnNameSST"] = "BtnEdit";
                                    stockTransfer_Model.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    //Session["BtnNameSST"] = "Refresh";
                                    stockTransfer_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.SubItemDetails = ds.Tables[7];
                        ViewBag.ItemStockBatchWise = ds.Tables[2];
                        ViewBag.ItemStockSerialWise = ds.Tables[3];
                        ViewBag.DocumentCode = StatusCode;
                        //ViewBag.Approve = "Y";
                        //Session["DocumentStatus"] = StatusCode;
                        stockTransfer_Model.DocumentStatus = StatusCode;
                        stockTransfer_Model.Cmd_batch = stockTransfer_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorStockTransfer/ShopfloorStockTransferDetail.cshtml", stockTransfer_Model);
                    }
                    else
                    {
                        ViewBag.Approve = "Y";
                        ViewBag.DocumentCode = "0";
                        //Session["DocumentStatus"] = "New";
                        stockTransfer_Model.DocumentStatus = "New";
                        stockTransfer_Model.Cmd_batch = stockTransfer_Model.Command;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorStockTransfer/ShopfloorStockTransferDetail.cshtml", stockTransfer_Model);
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
        public ActionResult ShopfloorStockTransferSave(ShopfloorStockTransfer_Model stockTransfer_Model,string command)
        {
            try
            {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (stockTransfer_Model.DeleteCommand == "Delete")
                    if (true)
                    {
                        command = "Delete";
                    }
                switch (command)
                {
                    case "AddNew":
                        //Session["shfltrfMessage"] = null;
                        ////Session["AppStatus"] = 'D';
                        //Session["BtnNameSST"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        ShopfloorStockTransfer_Model adddnew = new ShopfloorStockTransfer_Model();
                        adddnew.Command = "New";
                        adddnew.TransType = "Save";
                        adddnew.BtnName = "BtnAddNew";
                        TempData["ModelData"] = adddnew;
                        UrlModel NewModel = new UrlModel();
                        NewModel.Cmd = "New";
                        NewModel.tp = "Save";
                        NewModel.bt = "BtnAddNew";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(stockTransfer_Model.trf_no))
                                return RedirectToAction("ShopfloorStockTransfer_Edit", new { Trf_Number = stockTransfer_Model.trf_no, Trf_Date = stockTransfer_Model.trf_dt, ListFilterData = stockTransfer_Model.ListFilterData1, WF_Status = stockTransfer_Model.WFStatus });
                            else
                                adddnew.Command = "Refresh";
                            adddnew.TransType = "Refresh";
                            adddnew.BtnName = "Refresh";
                            adddnew.DocumentStatus = null;
                            TempData["ModelData"] = adddnew;
                            return RedirectToAction("ShopfloorStockTransferDetail", adddnew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ShopfloorStockTransferDetail", NewModel);

                    case "Edit":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("ShopfloorStockTransfer_Edit", new { Trf_Number = stockTransfer_Model.trf_no, Trf_Date = stockTransfer_Model.trf_dt, ListFilterData = stockTransfer_Model.ListFilterData1, WF_Status = stockTransfer_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 12-05-2025 to check Existing with previous year transaction*/
                        string CnfDt = stockTransfer_Model.trf_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, Br_ID, CnfDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("ShopfloorStockTransfer_Edit", new { Trf_Number = stockTransfer_Model.trf_no, Trf_Date = stockTransfer_Model.trf_dt, ListFilterData = stockTransfer_Model.ListFilterData1, WF_Status = stockTransfer_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["shfltrfMessage"] = null;

                        //Session["trf_no"] = stockTransfer_Model.trf_no;
                        //Session["trf_dt"] = stockTransfer_Model.trf_dt;
                        stockTransfer_Model.Command = command;
                        stockTransfer_Model.BtnName = "BtnEdit";
                        stockTransfer_Model.TransType = "Update";
                        stockTransfer_Model.SHPF_No = stockTransfer_Model.trf_no;
                        stockTransfer_Model.SHPF_Dt = stockTransfer_Model.trf_dt;
                        TempData["ModelData"] = stockTransfer_Model;
                        UrlModel EditModel = new UrlModel();
                        EditModel.Cmd = command;
                        EditModel.tp = "Update";
                        EditModel.bt = "BtnEdit";
                        EditModel.SHPF_No = stockTransfer_Model.trf_no;
                        EditModel.SHPF_Dt = stockTransfer_Model.trf_dt;
                        TempData["ListFilterData"] = stockTransfer_Model.ListFilterData1;
                        return RedirectToAction("ShopfloorStockTransferDetail", EditModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnNameSST"] = "Refresh";
                        ShopfloorStockTransfer_Delete(stockTransfer_Model, command);
                        ShopfloorStockTransfer_Model DeleteModel = new ShopfloorStockTransfer_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnRefresh";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnRefresh";
                        TempData["ListFilterData"] = stockTransfer_Model.ListFilterData1;
                        return RedirectToAction("ShopfloorStockTransferDetail", Delete_Model);

                    case "Save":
                        //Session["Command"] = command;
                        stockTransfer_Model.Command = command;
                        ShopfloorStockTransfer_SaveUpdate(stockTransfer_Model);
                        if (stockTransfer_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = stockTransfer_Model;
                        UrlModel SaveModel = new UrlModel();
                        SaveModel.bt = stockTransfer_Model.BtnName;
                        SaveModel.SHPF_No = stockTransfer_Model.SHPF_No;
                        SaveModel.SHPF_Dt = stockTransfer_Model.SHPF_Dt;
                        SaveModel.tp = stockTransfer_Model.TransType;
                        SaveModel.Cmd = stockTransfer_Model.Command;
                        TempData["ListFilterData"] = stockTransfer_Model.ListFilterData1;
                        return RedirectToAction("ShopfloorStockTransferDetail", SaveModel);
                    case "Update":
                        // Session["Command"] = command;
                        stockTransfer_Model.Command = command;
                        ShopfloorStockTransfer_SaveUpdate(stockTransfer_Model);
                        if (stockTransfer_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["ModelData"] = stockTransfer_Model;
                        UrlModel Save_Model = new UrlModel();
                        Save_Model.bt = stockTransfer_Model.BtnName;
                        Save_Model.SHPF_No = stockTransfer_Model.SHPF_No;
                        Save_Model.SHPF_Dt = stockTransfer_Model.SHPF_Dt;
                        Save_Model.tp = stockTransfer_Model.TransType;
                        Save_Model.Cmd = stockTransfer_Model.Command;
                        TempData["ListFilterData"] = stockTransfer_Model.ListFilterData1;
                        return RedirectToAction("ShopfloorStockTransferDetail", Save_Model);
                    case "Approve":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("ShopfloorStockTransfer_Edit", new { Trf_Number = stockTransfer_Model.trf_no, Trf_Date = stockTransfer_Model.trf_dt, ListFilterData = stockTransfer_Model.ListFilterData1, WF_Status = stockTransfer_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 12-05-2025 to check Existing with previous year transaction*/
                        string CnfDt1 = stockTransfer_Model.trf_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, Br_ID, CnfDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("ShopfloorStockTransfer_Edit", new { Trf_Number = stockTransfer_Model.trf_no, Trf_Date = stockTransfer_Model.trf_dt, ListFilterData = stockTransfer_Model.ListFilterData1, WF_Status = stockTransfer_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        stockTransfer_Model.Command = command;
                        ShopfloorStockTransfer_Approve(stockTransfer_Model,"");
                        TempData["ModelData"] = stockTransfer_Model;
                        UrlModel Approve = new UrlModel();
                        Approve.tp = "Update";
                        Approve.SHPF_No = stockTransfer_Model.SHPF_No;
                        Approve.SHPF_Dt = stockTransfer_Model.SHPF_Dt;
                        Approve.bt = "BtnEdit";
                        TempData["ListFilterData"] = stockTransfer_Model.ListFilterData1;
                        return RedirectToAction("ShopfloorStockTransferDetail", Approve);

                    case "Forward":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            Br_ID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, Br_ID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("ShopfloorStockTransfer_Edit", new { Trf_Number = stockTransfer_Model.trf_no, Trf_Date = stockTransfer_Model.trf_dt, ListFilterData = stockTransfer_Model.ListFilterData1, WF_Status = stockTransfer_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 12-05-2025 to check Existing with previous year transaction*/
                        string CnfDt2 = stockTransfer_Model.trf_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, Br_ID, CnfDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("ShopfloorStockTransfer_Edit", new { Trf_Number = stockTransfer_Model.trf_no, Trf_Date = stockTransfer_Model.trf_dt, ListFilterData = stockTransfer_Model.ListFilterData1, WF_Status = stockTransfer_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Refresh":
                        //Session["BtnNameSST"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["shfltrfMessage"] = null;
                        //Session["DocumentStatus"] = null;
                        ShopfloorStockTransfer_Model RefreshModel = new ShopfloorStockTransfer_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh_Model = new UrlModel();
                        refesh_Model.tp = "Save";
                        refesh_Model.bt = "BtnRefresh";
                        refesh_Model.Cmd = command;
                        TempData["ListFilterData"] = stockTransfer_Model.ListFilterData1;
                        return RedirectToAction("ShopfloorStockTransferDetail", refesh_Model);

                    case "Print":
                        return GenratePdfFile(stockTransfer_Model);
                    case "BacktoList":
                        //Session.Remove("shfltrfMessage");
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnNameSST");
                        //Session.Remove("DocumentStatus");
                        var WF_Status = stockTransfer_Model.WF_Status1;
                        TempData["ListFilterData"] = stockTransfer_Model.ListFilterData1;
                        return RedirectToAction("ShopfloorStockTransfer",new{ WF_Status});

                    default:
                        return RedirectToAction("ShopfloorStockTransferDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult ToRefreshByJS(string ListFilterData1,string ModelData,string Mailerror)
        {
            // Session["Message"] = "";
            ShopfloorStockTransfer_Model ToRefreshByJS = new ShopfloorStockTransfer_Model();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.SHPF_No = a[0].Trim();
            ToRefreshByJS.SHPF_Dt = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnEdit";
            ToRefreshByJS.Message =  Mailerror;
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wf = a[2].Trim();
            }
            Model.bt = "BtnEdit";
            Model.SHPF_No = ToRefreshByJS.SHPF_No;
            Model.SHPF_Dt = ToRefreshByJS.SHPF_Dt;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ShopfloorStockTransferDetail", Model);
        }

        public ActionResult GetShopfloorStockTransferList(string docid, string status)
        {

            //Session["WF_status"] = status;
            var WF_Status = status;
            return RedirectToAction("ShopfloorStockTransfer",new { WF_Status });
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1,string WF_Status1)
        {

            ShopfloorStockTransfer_Model stockTransfer_Model = new ShopfloorStockTransfer_Model();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    stockTransfer_Model.trf_no = jObjectBatch[i]["trf_no"].ToString();
                    stockTransfer_Model.trf_dt = jObjectBatch[i]["trf_dt"].ToString();
                    stockTransfer_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    stockTransfer_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    stockTransfer_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if(stockTransfer_Model.A_Status != "Approve")
            {
                stockTransfer_Model.A_Status = "Approve";
            }
            ShopfloorStockTransfer_Approve(stockTransfer_Model, ListFilterData1);
            UrlModel ApproveModel = new UrlModel();
            if (WF_Status1 != null && WF_Status1 != "")
            {
                ApproveModel.wf = WF_Status1;
                stockTransfer_Model.WF_Status1 = WF_Status1;
            }
            TempData["ModelData"] = stockTransfer_Model;
            ApproveModel.tp = "Update";
            ApproveModel.SHPF_No = stockTransfer_Model.SHPF_No;
            ApproveModel.SHPF_Dt = stockTransfer_Model.SHPF_Dt;
            ApproveModel.bt = "BtnEdit";
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ShopfloorStockTransferDetail", ApproveModel);
        }

        public ActionResult ShopfloorStockTransfer_Approve(ShopfloorStockTransfer_Model stockTransfer_Model,string ListFilterData1)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string trf_no = stockTransfer_Model.trf_no;
                string trf_dt = stockTransfer_Model.trf_dt;
                string A_Status = stockTransfer_Model.A_Status;
                string A_Level = stockTransfer_Model.A_Level;
                string A_Remarks = stockTransfer_Model.A_Remarks;
                string Message = _ShopfloorStockTransfer_ISERVICES.Approve_ShopfloorStockTransfer(CompID, Br_ID, DocumentMenuId, trf_no, trf_dt, UserID, mac_id, A_Status, A_Level, A_Remarks);
                string[] FDetail = Message.Split(',');
                string data = FDetail[0].ToString();
                if(data== "StockNotAvail")
                {
                    stockTransfer_Model.StockItemWiseMessage = string.Join(",", FDetail.Skip(1));
                    stockTransfer_Model.Message = "StockNotAvail";
                }
                else
                {
                    if (Message == "Approved")
                    {
                        try
                        {
                            //string fileName = "SST_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "ShopfloorStockTransfer_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(trf_no, trf_dt, fileName, DocumentMenuId, "AP");
                            _Common_IServices.SendAlertEmail(CompID, Br_ID, DocumentMenuId, trf_no, "AP", UserID, "0", filePath);
                        }
                        catch (Exception exMail)
                        {
                            stockTransfer_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        stockTransfer_Model.Message = stockTransfer_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";

                        //Session["shfltrfMessage"] = "Approved";
                        //stockTransfer_Model.Message = "Approved";
                    }
                }
                
                //if (Message == "StockNotAvail")
                //{
                //    // Session["shfltrfMessage"] = "StockNotAvail";
                //    stockTransfer_Model.Message = "StockNotAvail";
                //}
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["trf_no"] = trf_no;
                //Session["trf_dt"] = trf_dt;
                ////Session["AppStatus"] = 'D';
                //Session["BtnNameSST"] = "BtnEdit";
                stockTransfer_Model.SHPF_No = trf_no;
                stockTransfer_Model.SHPF_Dt = trf_dt;
                stockTransfer_Model.TransType = "Update";
                stockTransfer_Model.BtnName = "BtnEdit";
                stockTransfer_Model.Command = "Approve";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("ShopfloorStockTransferDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        public ActionResult ShopfloorStockTransfer_SaveUpdate(ShopfloorStockTransfer_Model stockTransfer_Model)
        {
            try
            {
                var commonContr = new CommonController();
                //if (stockTransfer_Model.CancelFlag == false)
                //{
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                
                DataTable ShopfloorStockTransferHeader = new DataTable();
                DataTable ShopfloorStockTransferDetails = new DataTable();
             
                DataTable ItemBatchDetails = new DataTable();
                DataTable ItemSerialDetails = new DataTable();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("MenuDocumentId", typeof(string));
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("trf_no", typeof(string));
                dtheader.Columns.Add("trf_dt", typeof(string));
                dtheader.Columns.Add("src", typeof(int));
                dtheader.Columns.Add("dstn", typeof(int));
                dtheader.Columns.Add("trf_type", typeof(string));
                dtheader.Columns.Add("material_type", typeof(string));
                dtheader.Columns.Add("remarks", typeof(string));//------------After this remaining to set 
                dtheader.Columns.Add("user_id", typeof(int));
                // dtheader.Columns.Add("user_dt", typeof(string));
                dtheader.Columns.Add("trf_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
               

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                //dtrowHeader["TransType"] = Session["Command"].ToString();
                dtrowHeader["TransType"] = stockTransfer_Model.Command;
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["trf_no"] = stockTransfer_Model.trf_no;
                dtrowHeader["trf_dt"] = stockTransfer_Model.trf_dt;
                dtrowHeader["src"] = stockTransfer_Model.shopfloorSource_id;
                dtrowHeader["dstn"] = stockTransfer_Model.Destnation_id;
                dtrowHeader["trf_type"] = stockTransfer_Model.TransferType;
                dtrowHeader["material_type"] = stockTransfer_Model.MaterialType;
                dtrowHeader["remarks"] = stockTransfer_Model.remarks;
                dtrowHeader["user_id"] = UserID;
                // dtrowHeader["user_dt"] = stockTransfer_Model.trf_dt;
                dtrowHeader["trf_status"] = IsNull(stockTransfer_Model.StatusCode,"D") ;
                string SystemDetail = string.Empty;
                SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                dtrowHeader["mac_id"] = SystemDetail;
            

                dtheader.Rows.Add(dtrowHeader);
                ShopfloorStockTransferHeader = dtheader;

                DataTable dtItem = new DataTable();
                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("trf_qty", typeof(string));
                dtItem.Columns.Add("it_remarks", typeof(string));

                JArray jObject = JArray.Parse(stockTransfer_Model.ItemDetail);
                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();
                    dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                    dtrowLines["trf_qty"] = jObject[i]["TransferQuantity"].ToString();
                    dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                ShopfloorStockTransferDetails = dtItem;

                DataTable Batch_detail = new DataTable();
                Batch_detail.Columns.Add("item_id", typeof(string));
                Batch_detail.Columns.Add("uom_id", typeof(string));
                Batch_detail.Columns.Add("batch_no", typeof(string));
                Batch_detail.Columns.Add("expiry_date", typeof(string));
                Batch_detail.Columns.Add("batch_qty", typeof(string));
                Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
                Batch_detail.Columns.Add("lot_no", typeof(string));
                Batch_detail.Columns.Add("mfg_name", typeof(string));
                Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                Batch_detail.Columns.Add("mfg_date", typeof(string));
                if (stockTransfer_Model.ItemBatchWiseDetail != null)
                {
                    JArray jObjectBatch = JArray.Parse(stockTransfer_Model.ItemBatchWiseDetail);
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
                        dtrowBatchDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectBatch[i]["mfg_name"].ToString(), null);
                        dtrowBatchDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectBatch[i]["mfg_mrp"].ToString(), null);
                        dtrowBatchDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectBatch[i]["mfg_date"].ToString(), null);

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
                Serial_detail.Columns.Add("mfg_name", typeof(string));
                Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                Serial_detail.Columns.Add("mfg_date", typeof(string));

                if (stockTransfer_Model.ItemSerialWiseDetail != null)
                {
                    JArray jObjectSerial = JArray.Parse(stockTransfer_Model.ItemSerialWiseDetail);
                    for (int i = 0; i < jObjectSerial.Count; i++)
                    {
                        DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                        dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                        dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                        dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                        dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                        dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                        dtrowSerialDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectSerial[i]["mfg_name"].ToString(), null);
                        dtrowSerialDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectSerial[i]["mfg_mrp"].ToString(), null);
                        dtrowSerialDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectSerial[i]["mfg_date"].ToString(), null);
                        Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                    }
                }
                ItemSerialDetails = Serial_detail;

                /*------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));
                if (stockTransfer_Model.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(stockTransfer_Model.SubItemDetailsDt);
                    for (int i = 0; i < jObject2.Count; i++)
                    {
                        DataRow dtrowItemdetails = dtSubItem.NewRow();
                        dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                        dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                        dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                        dtSubItem.Rows.Add(dtrowItemdetails);
                    }
                }

                /*------------------Sub Item end----------------------*/

                string SaveMessage =  _ShopfloorStockTransfer_ISERVICES.InsertUpdate_ShopfloorStockTransfer(ShopfloorStockTransferHeader, ShopfloorStockTransferDetails, ItemBatchDetails, ItemSerialDetails, dtSubItem);

                string[] FData = SaveMessage.Split(',');

                string Message = FData[0].ToString();
                string trf_number = FData[1].ToString();
                if (Message == "Data_Not_Found")
                {
                    //var a = SaveMessage.Split(',');
                    ViewBag.documentname = getDocumentName();
                    stockTransfer_Model.Title = title;
                    var msgs = Message.Replace("_", " ") + " " + trf_number+" in "+ stockTransfer_Model.Title;//ProdOrdCode is use for table type
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msgs, "", "");
                    stockTransfer_Model.Message = Message.Split(',')[0].Replace("_", "");
                    return RedirectToAction("ShopfloorStockTransferDetail");
                }

                if (Message == "StockNotAvail")
                {
                    //Session["shfltrfMessage"] = "StockNotAvail";

                    //Session["BtnNameSST"] = "BtnRefresh";
                    //Session["Command"] = "Refresh";
                    //Session["TransType"] = "Save";
                    //Session["DocumentStatus"] = null;
                    //Session["trf_no"] = trf_number;
                    //Session["trf_dt"] = stockTransfer_Model.trf_dt;
                    stockTransfer_Model.Message = "StockNotAvail";
                    stockTransfer_Model.SHPF_No = trf_number;
                    stockTransfer_Model.SHPF_Dt = stockTransfer_Model.trf_dt;
                    stockTransfer_Model.TransType = "Update";
                    stockTransfer_Model.BtnName = "BtnRefresh";
                    stockTransfer_Model.Command = "Refresh";
                }
                if (Message == "Update" || Message == "Save")
                {
                    //Session["trf_no"] = trf_number;
                    //Session["trf_dt"] = stockTransfer_Model.trf_dt;

                    //Session["shfltrfMessage"] = "Save";
                    //Session["Command"] = "Update";
                    //Session["TransType"] = "Update";
                    ////Session["AppStatus"] = 'D';
                    //Session["BtnNameSST"] = "BtnEdit";
                    stockTransfer_Model.Message = "Save";
                    stockTransfer_Model.SHPF_No = trf_number;
                    stockTransfer_Model.SHPF_Dt = stockTransfer_Model.trf_dt;
                    stockTransfer_Model.TransType = "Update";
                    stockTransfer_Model.BtnName = "BtnEdit";
                }
                return RedirectToAction("ShopfloorStockTransferDetail");
                
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
        }
        private ActionResult ShopfloorStockTransfer_Delete(ShopfloorStockTransfer_Model stockTransfer_Model, string command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                DataSet Message = _ShopfloorStockTransfer_ISERVICES.Delete_ShopfloorStockTransfer(CompID, Br_ID, stockTransfer_Model.trf_no, stockTransfer_Model.trf_dt);
               // Session["shfltrfMessage"] = "Deleted";
               // Session["Command"] = "Refresh";
               // Session["trf_dt"] = null;
               // Session["TransType"] = command;
               //// Session["AppStatus"] = 'D';
               // Session["BtnNameSST"] = "BtnDelete";
                return RedirectToAction("ShopfloorStockTransferDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
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

        [HttpPost]
        public JsonResult GetSourceAndDestinationList(string TransferType,string MaterialType)
        {
            try
            {
                JsonResult DataRows = null;
          
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataSet dt = _ShopfloorStockTransfer_ISERVICES.GetSourceAndDestinationList(CompID, Br_ID,TransferType, MaterialType);
                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        public ActionResult getItemStockBatchWise(string ItemId, string ShflID,string MaterialType, string trf_no, string trf_dt, string Doc_Status, string SelectedItemdetail, string typ, string Cmd)
        {
            try
            {
                DataSet ds = new DataSet();
                if (ItemId != "")
                {
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if(Doc_Status==null|| Doc_Status == "D"|| Doc_Status == "")
                    {
                        ds = _ShopfloorStockTransfer_ISERVICES.getItemStockBatchWise(ItemId, ShflID, CompID, Br_ID, MaterialType);
                    }
                    else
                    {
                        ds = _ShopfloorStockTransfer_ISERVICES.getItemStockBatchWiseAfterSavedDetail(ItemId,trf_no, trf_dt, CompID, Br_ID);
                    } 
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
                                ds.Tables[0].Rows[i]["mfg_name"] = item.GetValue("mfg_name");
                                ds.Tables[0].Rows[i]["mfg_mrp"] = item.GetValue("mfg_mrp");
                                ds.Tables[0].Rows[i]["mfg_date"] = item.GetValue("mfg_date");
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockBatchWise = ds.Tables[0];
                }
               
                    ViewBag.DocumentCode = Doc_Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.Command = Cmd;
                ViewBag.TransType = typ;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemstockSerialWise(string ItemId, string ShflID, string trf_no, string trf_dt,string Doc_Status, string SelectedItemSerial,string typ,string Cmd)
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
                    Br_ID = Session["BranchId"].ToString();
                }
                if (Doc_Status == null || Doc_Status == "D" || Doc_Status == "")
                {
                    ds = _ShopfloorStockTransfer_ISERVICES.getItemstockSerialWise(CompID, Br_ID, ItemId, ShflID);
                }
                else
                {
                    ds = _ShopfloorStockTransfer_ISERVICES.getItemstockSerialWiseAfterSavedDetail(CompID, Br_ID, trf_no, trf_dt);
                }
                  
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
                                ds.Tables[0].Rows[i]["mfg_name"] = item.GetValue("mfg_name");
                                ds.Tables[0].Rows[i]["mfg_mrp"] = item.GetValue("mfg_mrp");
                                ds.Tables[0].Rows[i]["mfg_date"] = item.GetValue("mfg_date");
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockSerialWise = ds.Tables[0];

                ViewBag.DocumentCode = Doc_Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.Command = Cmd;
                ViewBag.TransType = typ;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise_New.cshtml");
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
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
    , string Flag, string Status, string Doc_no,string src_shfl, string Doc_dt,string MaterialType)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();
                if (Flag == "Quantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        dt = _Common_IServices.GetSubItemShflAvlstockDetails(CompID, Br_ID, src_shfl, Item_id, MaterialType).Tables[0];
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
                        dt = _ShopfloorStockTransfer_ISERVICES.TRF_GetSubItemDetails(CompID, Br_ID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    }
                    ViewBag.ShowStock = "Y";
                }
                else if (Flag == "AvlQty")
                {
                    dt = _ShopfloorStockTransfer_ISERVICES.TRF_GetSubItemDetails(CompID, Br_ID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    ShowStock = "Y",
                    _subitemPageName = "ShflStkTrf",
                    decimalAllowed = "Y"
                };
                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag;
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /*--------------------------Add By Hina on 06-08-2024 For PDF Print Start--------------------------*/

        [HttpPost]
        public FileResult GenratePdfFile(ShopfloorStockTransfer_Model _model)
        {
            return File(GetPdfData(_model.trf_no, _model.trf_dt), "application/pdf", "ShopfloorStockTransfer.pdf");
        }
        public byte[] GetPdfData(string TRFNo, string TRFDate)
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
                    Br_ID = Session["BranchId"].ToString();
                }

                DataSet Details = _ShopfloorStockTransfer_ISERVICES.GetShflStkTrfDeatilsForPrint(CompID, Br_ID, TRFNo, Convert.ToDateTime(TRFDate).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Title = "ShopFloor Stock Transfer";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["trf_status"].ToString().Trim();
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ShopfloorStockTransfer/ShopfloorStockTransferPrint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                    //reader = new StringReader(DelSchedule);
                    //pdfDoc.NewPage();
                    //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
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
                Br_ID = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, Br_ID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, Doc_dt);
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
    }
}
