using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionConfirmation;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using System.IO;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.ProductionConfirmation
{
    public class ProductionConfirmationController : Controller
    {
        string CompID, BrID, UserID, language, FromDate = String.Empty;
        string DocumentMenuId = "105105130", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        ProductionConfirmation_IService _ProductionConfirmation_IService;
        ProductionConfirmation_Model _ProductionConfirmation_Model = new ProductionConfirmation_Model();
        DataSet dtSet;
        public ProductionConfirmationController(Common_IServices _Common_IServices, ProductionConfirmation_IService _ProductionConfirmation_IService)
        {
            this._Common_IServices = _Common_IServices;
            this._ProductionConfirmation_IService = _ProductionConfirmation_IService;
        }
        // GET: ApplicationLayer/ProductionConfirmation
        public ActionResult ProductionConfirmation(string WF_Status)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                ProductionConfirmation_Model _ProductionConfirmation_Model = new ProductionConfirmation_Model();
                _ProductionConfirmation_Model.WF_Status = WF_Status;
                SearchItem search = new SearchItem();
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //string endDate = dtnow.ToString("yyyy-MM-dd");
                /*Code Start Commented And Modify By Hina Sharma on 07-03-2025 to hit single proc*/
                //GetStatusList(_ProductionConfirmation_Model);
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrID, DocumentMenuId);
                CommonPageDetails();
                List<LStatus> statusLists = new List<LStatus>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    LStatus list = new LStatus();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _ProductionConfirmation_Model.StatusList = statusLists;
                /*Code End Commented And Modify By Hina Sharma on 07-03-2025 to hit single proc*/
                ViewBag.DocumentMenuId = DocumentMenuId;

                /*commented and Modify by Hina on 07-03-2025 to combine all list Procedure  in single Procedure*/

                //List<ProductName> _ProductName = new List<ProductName>();
                //DataTable dt1 = GetProductList(search, "A", null, null);
                //foreach (DataRow dr in dt1.Rows)
                //{
                //    ProductName ddlProductName = new ProductName();
                //    ddlProductName.ID = dr["item_id"].ToString();
                //    ddlProductName.Name = dr["item_name"].ToString();
                //    _ProductName.Add(ddlProductName);
                //}
                ////_ProductName.Insert(0, new ProductName() { ID = 0, Name = "---Select---" });
                //_ProductionConfirmation_Model.ProductNameList = _ProductName;
                _ProductionConfirmation_Model.FromDate = startDate;
              
                if(_ProductionConfirmation_Model.txtFromdate == null)
                {
                    _ProductionConfirmation_Model.txtFromdate = startDate;
                    _ProductionConfirmation_Model.txtToDate = CurrentDate;
                }
                dtSet = PrdCnfGetAllData(_ProductionConfirmation_Model, search, "A", null, null);
                List<ProductName> _ProductName = new List<ProductName>();
                //ProductName _itmlist = new ProductName();
                //_itmlist.ID = "0";
                //_itmlist.Name = "---Select---";
                //_ProductName.Add(_itmlist);
                foreach (DataRow dr in dtSet.Tables[2].Rows)
                {
                    ProductName ddlProductName = new ProductName();
                    ddlProductName.ID = dr["item_id"].ToString();
                    ddlProductName.Name = dr["item_name"].ToString();
                    _ProductName.Add(ddlProductName);
                }
                // _ProductName.Insert(0, new ProductName() { ID = "0",Name = "---Select---" });
                _ProductionConfirmation_Model.ProductNameList = _ProductName;

                List<ShopFloor> _ShopFloor = new List<ShopFloor>();/*Add by Hina Sharma on 07-03-2025 for filter*/
                ShopFloor _Shfllist = new ShopFloor();
                _Shfllist.shfl_id = "0";
                _Shfllist.shfl_name = "---Select---";
                _ShopFloor.Add(_Shfllist);
                foreach (DataRow dr in dtSet.Tables[3].Rows)
                {
                    ShopFloor _Statuslist1 = new ShopFloor();
                    _Statuslist1.shfl_id = dr["shfl_id"].ToString();
                    _Statuslist1.shfl_name = dr["shfl_name"].ToString();
                    _ShopFloor.Add(_Statuslist1);
                }
                _ProductionConfirmation_Model.ShopFloorList = _ShopFloor;

                List<OperationName> _op = new List<OperationName>();/*Add by Hina Sharma on 07-03-2025 for filter*/
                OperationName _opObj = new OperationName();
                _opObj.op_id = "0";
                _opObj.op_name = "---Select---";
                _op.Add(_opObj);
                foreach (DataRow dr in dtSet.Tables[4].Rows)
                {
                    OperationName _oplist = new OperationName();
                    _oplist.op_id = dr["op_id"].ToString();
                    _oplist.op_name = dr["op_name"].ToString();
                    _op.Add(_oplist);
                }
                _ProductionConfirmation_Model.OperationNameList = _op;

                GetshiftList(_ProductionConfirmation_Model);/*Add by Hina Sharma on 07-03-2025 for filter*/

                List<WorkstationName> ws = new List<WorkstationName>();/*Add by Hina Sharma on 07-03-2025 for filter*/
                WorkstationName wsObj = new WorkstationName();
                wsObj.ws_id = "0";
                wsObj.ws_name = "---Select---";
                ws.Add(wsObj);
                foreach (DataRow dr in dtSet.Tables[5].Rows)
                {
                    WorkstationName _wslist = new WorkstationName();
                    _wslist.ws_id = dr["ws_id"].ToString();
                    _wslist.ws_name = dr["ws_name"].ToString();
                    ws.Add(_wslist);
                }
                _ProductionConfirmation_Model.WorkstationNameList = ws;

                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var ProductID = a[0].Trim();
                    var Fromdate = a[1].Trim();
                    var Todate = a[2].Trim();
                    var Status = a[3].Trim();
                    var OpId = a[4].Trim();
                    var ShflId = a[5].Trim();
                    var WSId = a[6].Trim();
                    var ShftId = a[7].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                    }
                    _ProductionConfirmation_Model.FromDate = Fromdate;
                    _ProductionConfirmation_Model.ListFilterData = TempData["ListFilterData"].ToString();
                    DataTable dt = _ProductionConfirmation_IService.GetProductionConfirmationFilter(ProductID, Fromdate, Todate, Status, CompID, BrID, DocumentMenuId,
                        OpId, ShflId, WSId, ShftId);
                    //Session["CNFSearch"] = "CNF_Search";
                    _ProductionConfirmation_Model.CNFSearch = "CNF_Search";
                    ViewBag.ProductionCnfList = dt;
                    _ProductionConfirmation_Model.ddl_ProductName = ProductID;
                    _ProductionConfirmation_Model.ddl_OperationName = OpId;
                    _ProductionConfirmation_Model.ddl_ShopfloorName = ShflId;
                    _ProductionConfirmation_Model.ddl_WorkstationName = WSId;
                    _ProductionConfirmation_Model.shift = ShftId;
                    _ProductionConfirmation_Model.txtFromdate = Fromdate;
                    _ProductionConfirmation_Model.txtToDate = Todate;
                    _ProductionConfirmation_Model.ListStatus = Status;

                }
                else
                {
                    /*Commented and modify by Hina sharma on 07-03-2025 to combine in single procedure*/
                    //DataTable DtProductionCnfList = new DataTable();
                    //DtProductionCnfList = GetProductionCnfDetails(_ProductionConfirmation_Model);
                    _ProductionConfirmation_Model.FromDate = startDate;
                    //ViewBag.ProductionCnfList = DtProductionCnfList;
                    ViewBag.ProductionCnfList = dtSet.Tables[0];

                }
                //ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.MenuPageName = getDocumentName();
                _ProductionConfirmation_Model.Title = title;
                //Session["CNFSearch"] = "0";
                _ProductionConfirmation_Model.CNFSearch = null;
                ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionConfirmation/ProductionConfirmationList.cshtml", _ProductionConfirmation_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet PrdCnfGetAllData(ProductionConfirmation_Model _PCModel, SearchItem search, string SrcType, string fy, string period)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }

            string br_id = Session["BranchId"].ToString();
            string UserID = Session["UserID"].ToString();
            string wfstatus = "";

            if (_PCModel.WF_Status != null)
            {
                wfstatus = _PCModel.WF_Status;
            }
            else
            {
                wfstatus = "";
            }
            DataSet ds = _ProductionConfirmation_IService.GetAllData(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id), UserID, wfstatus, DocumentMenuId, SrcType, fy, period, search.SearchName,
                _PCModel.txtFromdate, _PCModel.txtToDate);
            return ds;
        }
        public ActionResult AddProductionConfirmationDetail()
        {
            //Session.Remove("TransType");

            //Session["Message"] = "New";
            //Session["Command"] = "New";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "";
            ProductionConfirmation_Model AddNewModel = new ProductionConfirmation_Model();
            AddNewModel.Command = "New";
            AddNewModel.TransType = "Save";
            AddNewModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = AddNewModel;
            UrlModel AddNew_Model = new UrlModel();
            AddNew_Model.bt = "BtnAddNew";
            AddNew_Model.Cmd = "New";
            AddNew_Model.tp = "Save";
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ProductionConfirmation");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("ProductionConfirmationDetail", "ProductionConfirmation", AddNew_Model);

        }
        private DataTable GetProductList(SearchItem search, string SrcType, string fy, string period)
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
                    BrID = Session["BranchId"].ToString();
                }
                DataTable dt = _ProductionConfirmation_IService.Bind_ProductList1(CompID, BrID, SrcType, fy, period, search.SearchName);
                //DataTable dt = _ProductionAdvice_IService.GetRequirmentreaList(CompID, BrchID, flag);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult ProductionConfirmationDetail(UrlModel _urlModel)
        {
            try
            {
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                /*Add by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrID, _urlModel.PAC_dt) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var other = new CommonController(_Common_IServices);
                ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrID, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                var _Pro_conf_Model = TempData["ModelData"] as ProductionConfirmation_Model;
                if (_Pro_conf_Model != null)
                {
                    CommonPageDetails();
                    // ProductionConfirmation_Model _ProductionConfirmation_Model = new ProductionConfirmation_Model();
                    List<OrderNumber> _OrderNumberList = new List<OrderNumber>();
                    OrderNumber _OrderNumber = new OrderNumber();
                    _OrderNumber.porder_no = "--- Select ---";
                    _OrderNumber.porder_dt = "0";
                    _OrderNumberList.Add(_OrderNumber);
                    _Pro_conf_Model.OrderNumberList = _OrderNumberList;

                    List<WorkstationName> ws = new List<WorkstationName>();/*Add by hina on 20-09-2024 to add for dropdown*/
                    WorkstationName wsObj = new WorkstationName();
                    wsObj.ws_id = "0";
                    wsObj.ws_name = "---Select---";
                    ws.Add(wsObj);
                    _Pro_conf_Model.WorkstationNameList = ws;

                    _Pro_conf_Model.confirmation_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                    //ViewBag.VBRoleList = GetRoleList();
                    _Pro_conf_Model.product_id = "0";
                    _Pro_conf_Model.product_name = "---Select---";

                    GetshiftList(_Pro_conf_Model);

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _Pro_conf_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_Pro_conf_Model.TransType == "Update")
                    {
                        //string cnf_no = Session["PC_Number"].ToString();
                        //string cnf_dt = Session["PC_Date"].ToString();

                        string cnf_no = _Pro_conf_Model.PC_Number;
                        string cnf_dt = _Pro_conf_Model.PC_Date;
                        DataSet ds = _ProductionConfirmation_IService.GetProductionConfirmationDetailByNo(CompID, BrID, cnf_no, cnf_dt, UserID, DocumentMenuId);

                        _Pro_conf_Model.Sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                        _Pro_conf_Model.confirmation_no = ds.Tables[0].Rows[0]["cnf_no"].ToString();
                        _Pro_conf_Model.confirmation_dt = ds.Tables[0].Rows[0]["cnf_dt"].ToString();
                        _Pro_conf_Model.product_id = ds.Tables[0].Rows[0]["product_id"].ToString();
                        _Pro_conf_Model.product_id = ds.Tables[0].Rows[0]["product_id"].ToString();
                        _Pro_conf_Model.product_name = ds.Tables[0].Rows[0]["item_name"].ToString();

                        List<OrderNumber> _OrderNoList = new List<OrderNumber>();
                        OrderNumber _OrderNo = new OrderNumber();
                        _OrderNo.porder_no = ds.Tables[0].Rows[0]["jc_no"].ToString();
                        _OrderNo.porder_dt = ds.Tables[0].Rows[0]["jc_dt"].ToString();
                        _OrderNoList.Add(_OrderNo);
                        _Pro_conf_Model.OrderNumberList = _OrderNoList;

                        List<WorkstationName> ws1 = new List<WorkstationName>();/*Add by hina on 20-09-2024 to add for dropdown*/
                        WorkstationName wsObj1 = new WorkstationName();
                        wsObj1.ws_id = ds.Tables[0].Rows[0]["ws_id"].ToString();
                        wsObj1.ws_name = ds.Tables[0].Rows[0]["ws_name"].ToString();
                        ws1.Add(wsObj1);
                        _Pro_conf_Model.WorkstationNameList = ws1;

                        _Pro_conf_Model.uom_id = ds.Tables[0].Rows[0]["uom_id"].ToString();
                        _Pro_conf_Model.uom_name = ds.Tables[0].Rows[0]["uom_alias"].ToString();
                        _Pro_conf_Model.order_no = ds.Tables[0].Rows[0]["jc_no"].ToString();
                        _Pro_conf_Model.hdn_order_no = ds.Tables[0].Rows[0]["jc_no"].ToString();
                        _Pro_conf_Model.order_dt = ds.Tables[0].Rows[0]["jcdt"].ToString();
                        _Pro_conf_Model.hdn_order_dt = ds.Tables[0].Rows[0]["jc_dt"].ToString();
                        _Pro_conf_Model.order_qty = ds.Tables[0].Rows[0]["jc_qty"].ToString();
                        _Pro_conf_Model.pending_qty = ds.Tables[0].Rows[0]["pending_qty"].ToString();
                        _Pro_conf_Model.operation_id = ds.Tables[0].Rows[0]["op_id"].ToString();
                        _Pro_conf_Model.operation = ds.Tables[0].Rows[0]["op_name"].ToString();
                        _Pro_conf_Model.shopfloor_id = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                        _Pro_conf_Model.shopfloor = ds.Tables[0].Rows[0]["shfl_name"].ToString();
                        //_Pro_conf_Model.workstation_id = ds.Tables[0].Rows[0]["ws_id"].ToString();/*commented by Hina on 20-09-2024 to add for dropdown*/
                        //_Pro_conf_Model.workstation = ds.Tables[0].Rows[0]["ws_name"].ToString();
                        _Pro_conf_Model.ddl_WorkstationName = Convert.ToString(ds.Tables[0].Rows[0]["ws_id"]);
                        _Pro_conf_Model.ddl_WorkstationText = Convert.ToString(ds.Tables[0].Rows[0]["ws_name"]);
                        _Pro_conf_Model.supervisorName = ds.Tables[0].Rows[0]["supervisor_name"].ToString();
                        _Pro_conf_Model.jobstartdate = ds.Tables[0].Rows[0]["jc_st_date"].ToString();
                        _Pro_conf_Model.jobenddate = ds.Tables[0].Rows[0]["jc_en_date"].ToString();
                        _Pro_conf_Model.hours = ds.Tables[0].Rows[0]["hrs"].ToString();
                        _Pro_conf_Model.shift_id = ds.Tables[0].Rows[0]["shift_id"].ToString();
                        _Pro_conf_Model.ddl_shift = ds.Tables[0].Rows[0]["shift_name"].ToString();
                        _Pro_conf_Model.shift = ds.Tables[0].Rows[0]["shift_id"].ToString();

                        _Pro_conf_Model.Remks = ds.Tables[0].Rows[0]["cnf_remarks"].ToString();
                        _Pro_conf_Model.create_by = ds.Tables[0].Rows[0]["createname"].ToString();
                        _Pro_conf_Model.create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _Pro_conf_Model.mod_by = ds.Tables[0].Rows[0]["modname"].ToString();
                        _Pro_conf_Model.mod_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _Pro_conf_Model.app_by = ds.Tables[0].Rows[0]["appname"].ToString();
                        _Pro_conf_Model.app_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _Pro_conf_Model.status = ds.Tables[0].Rows[0]["status_name"].ToString();
                        _Pro_conf_Model.op_output_item_name = ds.Tables[0].Rows[0]["op_out_item_name"].ToString();
                        _Pro_conf_Model.op_output_itemid = ds.Tables[0].Rows[0]["op_out_item_id"].ToString();
                        _Pro_conf_Model.op_output_UomName = ds.Tables[0].Rows[0]["op_out_uom_name"].ToString();
                        _Pro_conf_Model.op_output_uom_id = ds.Tables[0].Rows[0]["op_out_uom_id"].ToString();
                        _Pro_conf_Model.op_output_uom_Name = ds.Tables[0].Rows[0]["op_out_uom_name"].ToString();


                        _Pro_conf_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        _Pro_conf_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);

                        string StatusCode = ds.Tables[0].Rows[0]["cnf_status"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                        ViewBag.SubItemDetails = ds.Tables[9];
                        _Pro_conf_Model.StatusCode = StatusCode;
                        _Pro_conf_Model.create_id = create_id;
                        if (StatusCode == "C")
                        {
                            _Pro_conf_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _Pro_conf_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _Pro_conf_Model.CancelFlag = false;
                        }
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[7];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _Pro_conf_Model.Command != "Edit")
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
                                    //Session["BtnName"] = "Refresh";
                                    _Pro_conf_Model.BtnName = "Refresh";
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
                                        //Session["BtnName"] = "BtnEdit";
                                        _Pro_conf_Model.BtnName = "BtnEdit";
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
                                        //Session["BtnName"] = "BtnEdit";
                                        _Pro_conf_Model.BtnName = "BtnEdit";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnEdit";
                                    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _Pro_conf_Model.BtnName = "BtnEdit";
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
                                        //Session["BtnName"] = "BtnEdit";
                                        _Pro_conf_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnName"] = "BtnEdit";
                                    _Pro_conf_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnName"] = "BtnEdit";
                                    _Pro_conf_Model.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnEdit";
                                    _Pro_conf_Model.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _Pro_conf_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.ConsumptionItemDetails = ds.Tables[1];
                        ViewBag.OutputItemDetails = ds.Tables[2];
                        ViewBag.ItemStockBatchWise = ds.Tables[3];
                        ViewBag.ItemStockSerialWise = ds.Tables[4];
                        ViewBag.AttechmentDetails = ds.Tables[8];
                        ViewBag.DocumentCode = StatusCode;
                        //Session["DocumentStatus"] = StatusCode;
                        _Pro_conf_Model.DocumentStatus = StatusCode;
                        // ViewBag.MenuPageName = getDocumentName();
                        _Pro_conf_Model.Title = title;
                        ViewBag.Command = _Pro_conf_Model.Command;
                        _Pro_conf_Model.Docid = DocumentMenuId;
                        _Pro_conf_Model.batch_Command = _Pro_conf_Model.Command;
                        ViewBag.TransType = _Pro_conf_Model.TransType;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionConfirmation/ProductionConfirmationDetail.cshtml", _Pro_conf_Model);
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        _Pro_conf_Model.Title = title;
                        ViewBag.DocumentCode = "0";
                        //Session["DocumentStatus"] = "New";
                        _Pro_conf_Model.DocumentStatus = "New";
                        _Pro_conf_Model.Docid = DocumentMenuId;
                        ViewBag.Command = _Pro_conf_Model.Command;
                        _Pro_conf_Model.batch_Command = _Pro_conf_Model.Command;
                        ViewBag.TransType = _Pro_conf_Model.TransType;
                        ViewBag.DocumentStatus = _Pro_conf_Model.DocumentStatus;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionConfirmation/ProductionConfirmationDetail.cshtml", _Pro_conf_Model);
                    }
                }
                else
                {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        BrID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    ProductionConfirmation_Model _ProductionConfirmation_Model = new ProductionConfirmation_Model();
                    if (_urlModel != null)
                    {
                        _ProductionConfirmation_Model.BtnName = _urlModel.bt;
                        _ProductionConfirmation_Model.PC_Number = _urlModel.PAC_No;
                        _ProductionConfirmation_Model.PC_Date = _urlModel.PAC_dt;
                        _ProductionConfirmation_Model.Command = _urlModel.Cmd;
                        _ProductionConfirmation_Model.TransType = _urlModel.tp;
                        _ProductionConfirmation_Model.WF_Status1 = _urlModel.wf;
                        _ProductionConfirmation_Model.DocumentStatus = _urlModel.DMS;
                    }
                    List<OrderNumber> _OrderNumberList = new List<OrderNumber>();
                    OrderNumber _OrderNumber = new OrderNumber();
                    _OrderNumber.porder_no = "--- Select ---";
                    _OrderNumber.porder_dt = "0";
                    _OrderNumberList.Add(_OrderNumber);

                    List<WorkstationName> ws = new List<WorkstationName>();/*Add by hina on 20-09-2024 to add for dropdown*/
                    WorkstationName wsObj = new WorkstationName();
                    wsObj.ws_id = "0";
                    wsObj.ws_name = "---Select---";
                    ws.Add(wsObj);
                    _ProductionConfirmation_Model.WorkstationNameList = ws;

                    GetshiftList(_ProductionConfirmation_Model);
                    _ProductionConfirmation_Model.OrderNumberList = _OrderNumberList;
                    _ProductionConfirmation_Model.confirmation_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                    _ProductionConfirmation_Model.product_id = "0";
                    _ProductionConfirmation_Model.product_name = "---Select---";
                    CommonPageDetails();

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ProductionConfirmation_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update")
                    if (_ProductionConfirmation_Model.TransType == "Update")
                    {
                        //string cnf_no = Session["PC_Number"].ToString();
                        //string cnf_dt = Session["PC_Date"].ToString();

                        string cnf_no = _ProductionConfirmation_Model.PC_Number;
                        string cnf_dt = _ProductionConfirmation_Model.PC_Date;
                        DataSet ds = _ProductionConfirmation_IService.GetProductionConfirmationDetailByNo(CompID, BrID, cnf_no, cnf_dt, UserID, DocumentMenuId);
                        _ProductionConfirmation_Model.Sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                        _ProductionConfirmation_Model.confirmation_no = ds.Tables[0].Rows[0]["cnf_no"].ToString();
                        _ProductionConfirmation_Model.confirmation_dt = ds.Tables[0].Rows[0]["cnf_dt"].ToString();
                        _ProductionConfirmation_Model.product_id = ds.Tables[0].Rows[0]["product_id"].ToString();
                        _ProductionConfirmation_Model.product_id = ds.Tables[0].Rows[0]["product_id"].ToString();
                        _ProductionConfirmation_Model.product_name = ds.Tables[0].Rows[0]["item_name"].ToString();
                        GetshiftList(_ProductionConfirmation_Model);
                        List<OrderNumber> _OrderNoList = new List<OrderNumber>();
                        OrderNumber _OrderNo = new OrderNumber();
                        _OrderNo.porder_no = ds.Tables[0].Rows[0]["jc_no"].ToString();
                        _OrderNo.porder_dt = ds.Tables[0].Rows[0]["jc_dt"].ToString();
                        _OrderNoList.Add(_OrderNo);
                        _ProductionConfirmation_Model.OrderNumberList = _OrderNoList;

                        List<WorkstationName> ws1 = new List<WorkstationName>();/*Add by hina on 20-09-2024 to add for dropdown*/
                        WorkstationName wsObj1 = new WorkstationName();
                        wsObj1.ws_id = ds.Tables[0].Rows[0]["ws_id"].ToString();
                        wsObj1.ws_name = ds.Tables[0].Rows[0]["ws_name"].ToString();
                        ws1.Add(wsObj1);
                        _ProductionConfirmation_Model.WorkstationNameList = ws1;

                        _ProductionConfirmation_Model.uom_id = ds.Tables[0].Rows[0]["uom_id"].ToString();
                        _ProductionConfirmation_Model.uom_name = ds.Tables[0].Rows[0]["uom_alias"].ToString();
                        _ProductionConfirmation_Model.order_no = ds.Tables[0].Rows[0]["jc_no"].ToString();
                        _ProductionConfirmation_Model.hdn_order_no = ds.Tables[0].Rows[0]["jc_no"].ToString();
                        _ProductionConfirmation_Model.order_dt = ds.Tables[0].Rows[0]["jcdt"].ToString();
                        _ProductionConfirmation_Model.hdn_order_dt = ds.Tables[0].Rows[0]["jc_dt"].ToString();
                        _ProductionConfirmation_Model.order_qty = ds.Tables[0].Rows[0]["jc_qty"].ToString();
                        _ProductionConfirmation_Model.pending_qty = ds.Tables[0].Rows[0]["pending_qty"].ToString();
                        _ProductionConfirmation_Model.operation_id = ds.Tables[0].Rows[0]["op_id"].ToString();
                        _ProductionConfirmation_Model.operation = ds.Tables[0].Rows[0]["op_name"].ToString();
                        _ProductionConfirmation_Model.shopfloor_id = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                        _ProductionConfirmation_Model.shopfloor = ds.Tables[0].Rows[0]["shfl_name"].ToString();
                        //_ProductionConfirmation_Model.workstation_id = ds.Tables[0].Rows[0]["ws_id"].ToString();/*Add by hina on 20-09-2024 to add for dropdown*/
                        //_ProductionConfirmation_Model.workstation = ds.Tables[0].Rows[0]["ws_name"].ToString();
                        _ProductionConfirmation_Model.ddl_WorkstationName = Convert.ToString(ds.Tables[0].Rows[0]["ws_id"]);
                        _ProductionConfirmation_Model.ddl_WorkstationText = Convert.ToString(ds.Tables[0].Rows[0]["ws_name"]);
                        _ProductionConfirmation_Model.supervisorName = ds.Tables[0].Rows[0]["supervisor_name"].ToString();
                        _ProductionConfirmation_Model.jobstartdate = ds.Tables[0].Rows[0]["jc_st_date"].ToString();
                        _ProductionConfirmation_Model.jobenddate = ds.Tables[0].Rows[0]["jc_en_date"].ToString();
                        _ProductionConfirmation_Model.hours = ds.Tables[0].Rows[0]["hrs"].ToString();
                        _ProductionConfirmation_Model.shift_id = ds.Tables[0].Rows[0]["shift_id"].ToString();
                        _ProductionConfirmation_Model.ddl_shift = ds.Tables[0].Rows[0]["shift_name"].ToString();
                        _ProductionConfirmation_Model.shift = ds.Tables[0].Rows[0]["shift_id"].ToString();
                        _ProductionConfirmation_Model.Remks = ds.Tables[0].Rows[0]["cnf_remarks"].ToString();
                        _ProductionConfirmation_Model.create_by = ds.Tables[0].Rows[0]["createname"].ToString();
                        _ProductionConfirmation_Model.create_on = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _ProductionConfirmation_Model.mod_by = ds.Tables[0].Rows[0]["modname"].ToString();
                        _ProductionConfirmation_Model.mod_on = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _ProductionConfirmation_Model.app_by = ds.Tables[0].Rows[0]["appname"].ToString();
                        _ProductionConfirmation_Model.app_on = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _ProductionConfirmation_Model.status = ds.Tables[0].Rows[0]["status_name"].ToString();

                        _ProductionConfirmation_Model.op_output_item_name = ds.Tables[0].Rows[0]["op_out_item_name"].ToString();
                        _ProductionConfirmation_Model.op_output_itemid = ds.Tables[0].Rows[0]["op_out_item_id"].ToString();
                        _ProductionConfirmation_Model.op_output_UomName = ds.Tables[0].Rows[0]["op_out_uom_name"].ToString();
                        _ProductionConfirmation_Model.op_output_uom_id = ds.Tables[0].Rows[0]["op_out_uom_id"].ToString();
                        _ProductionConfirmation_Model.op_output_uom_Name = ds.Tables[0].Rows[0]["op_out_uom_name"].ToString();

                        _ProductionConfirmation_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[7]);
                        _ProductionConfirmation_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);

                        string StatusCode = ds.Tables[0].Rows[0]["cnf_status"].ToString().Trim();
                        string create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["app_id"].ToString();
                        ViewBag.SubItemDetails = ds.Tables[9];
                        _ProductionConfirmation_Model.StatusCode = StatusCode;
                        _ProductionConfirmation_Model.create_id = create_id;
                        if (StatusCode == "C")
                        {
                            _ProductionConfirmation_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _ProductionConfirmation_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            _ProductionConfirmation_Model.CancelFlag = false;
                        }
                        if (StatusCode != "D" && StatusCode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[7];
                        }

                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _ProductionConfirmation_Model.Command != "Edit")
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
                                    //Session["BtnName"] = "Refresh";
                                    _ProductionConfirmation_Model.BtnName = "Refresh";
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
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionConfirmation_Model.BtnName = "BtnEdit";
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
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionConfirmation_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionConfirmation_Model.BtnName = "BtnEdit";
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
                                        //Session["BtnName"] = "BtnEdit";
                                        _ProductionConfirmation_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionConfirmation_Model.BtnName = "BtnEdit";
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
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionConfirmation_Model.BtnName = "BtnEdit";
                                }
                            }
                            if (StatusCode == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnEdit";
                                    _ProductionConfirmation_Model.BtnName = "BtnEdit";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    _ProductionConfirmation_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        ViewBag.ConsumptionItemDetails = ds.Tables[1];
                        ViewBag.OutputItemDetails = ds.Tables[2];
                        ViewBag.ItemStockBatchWise = ds.Tables[3];
                        ViewBag.ItemStockSerialWise = ds.Tables[4];
                        ViewBag.AttechmentDetails = ds.Tables[8];
                        ViewBag.DocumentCode = StatusCode;
                        //Session["DocumentStatus"] = StatusCode;
                        //_ProductionConfirmation_Model.DocumentStatus = StatusCode;
                        //ViewBag.MenuPageName = getDocumentName();
                        _ProductionConfirmation_Model.Title = title;
                        _ProductionConfirmation_Model.Docid = DocumentMenuId;
                        ViewBag.Command = _ProductionConfirmation_Model.Command;
                        ViewBag.TransType = _ProductionConfirmation_Model.TransType;
                        _ProductionConfirmation_Model.batch_Command = _ProductionConfirmation_Model.Command;
                        ViewBag.DocumentStatus = _ProductionConfirmation_Model.DocumentStatus;
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionConfirmation/ProductionConfirmationDetail.cshtml", _ProductionConfirmation_Model);
                    }
                    else
                    {
                        ViewBag.DocumentCode = "0";
                        //ViewBag.MenuPageName = getDocumentName();
                        _ProductionConfirmation_Model.Title = title;
                        _ProductionConfirmation_Model.Docid = DocumentMenuId;
                        ViewBag.Command = _ProductionConfirmation_Model.Command;
                        ViewBag.TransType = _ProductionConfirmation_Model.TransType;
                        _ProductionConfirmation_Model.batch_Command = _ProductionConfirmation_Model.Command;
                        //ViewBag.VBRoleList = GetRoleList();
                        //Session["DocumentStatus"] = "New";
                        _ProductionConfirmation_Model.DocumentStatus = "New";
                        ViewBag.DocumentStatus = _ProductionConfirmation_Model.DocumentStatus;
                        return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionConfirmation/ProductionConfirmationDetail.cshtml", _ProductionConfirmation_Model);
                    }
                }


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public void GetshiftList(ProductionConfirmation_Model _Pro_conf_Model)
        {
            try
            {
                string BrchID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                List<Shiftlist> _shiftlists = new List<Shiftlist>();
                Shiftlist shiftlist = new Shiftlist();
                shiftlist.ID = 0;
                shiftlist.Name = "---Select---";
                _shiftlists.Add(shiftlist);
                Shiftlist shObj1 = new Shiftlist();
                shObj1.ID = 1;
                shObj1.Name = "Shift-1";
                _shiftlists.Add(shObj1);
                Shiftlist shObj2 = new Shiftlist();
                shObj2.ID = 2;
                shObj2.Name = "Shift-2";
                _shiftlists.Add(shObj2);
                Shiftlist shObj3 = new Shiftlist();
                shObj3.ID = 3;
                shObj3.Name = "Shift-3";
                _shiftlists.Add(shObj3);
                _Pro_conf_Model.ShiftdropdownlistP = _shiftlists;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }

        private void CommonPageDetails()
        {
            try
            {
                var BrchID = "";
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
        public void GetStatusList(ProductionConfirmation_Model _ProductionConfirmation_Model)
        {
            try
            {
                List<LStatus> statusLists = new List<LStatus>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new LStatus { status_id = x.status_id, status_name = x.status_name });
                _ProductionConfirmation_Model.StatusList = listOfStatus;
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
        [HttpPost]
        public JsonResult GetOrderLists(string Product_id)
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
                DataSet result = _ProductionConfirmation_IService.GetOrderList(Product_id, Comp_ID, Br_ID);

                DataRow Drow = result.Tables[0].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";
                Drow[2] = "0";

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
        public JsonResult GetOrderDetail(string OrderNo, string OrderDate, string Flag, string Shflid)
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
                    BrID = Session["BranchId"].ToString();
                }
                DataSet result = _ProductionConfirmation_IService.GetOrderDetails(CompID, BrID, OrderNo, OrderDate, Flag, Shflid);
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
        public ActionResult getItemStockBatchWise(string ItemId, string ShflId, string DocStatus, string SelectedItemdetail
            , string Transtype, string cmd, string uom_id)
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
                    BrID = Session["BranchId"].ToString();
                }
                if (ItemId != "")
                {
                    ds = _ProductionConfirmation_IService.getItemStockBatchWise(CompID, BrID, ItemId, ShflId, uom_id);
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
        public ActionResult getItemStockBatchWiseAfterStockUpadte(string PC_No, string PC_Date, string ItemID, string Transtype, string cmd)
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
                    BrID = Session["BranchId"].ToString();
                }
                ds = _ProductionConfirmation_IService.getItemStockBatchWiseAfterStockUpdate(CompID, BrID, PC_No, PC_Date, ItemID);
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
        public ActionResult getItemstockSerialWise(string ItemId, string ShflID, string DocStatus, string SelectedItemSerial, string Transtype, string cmd)
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
                    BrID = Session["BranchId"].ToString();
                }
                ds = _ProductionConfirmation_IService.getItemstockSerialWise(CompID, BrID, ItemId, ShflID);

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
        public ActionResult getItemstockSerialWiseAfterStockUpadte(string PCNo, string PCDate, string ItemID, string Transtype, string cmd)
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
                    BrID = Session["BranchId"].ToString();
                }
                ds = _ProductionConfirmation_IService.getItemstockSerialWiseAfterStockUpdate(CompID, BrID, PCNo, PCDate, ItemID);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductionConfirmationSave(ProductionConfirmation_Model _ProductionConfirmation_Model, string command)
        {
            try
            {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ProductionConfirmation_Model.DeleteCommand == "Delete")
                    if (true)
                    {
                        command = "Delete";
                    }
                switch (command)
                {
                    case "AddNew":
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        ProductionConfirmation_Model adddnew = new ProductionConfirmation_Model();
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
                            BrID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ProductionConfirmation_Model.confirmation_no))
                                return RedirectToAction("ProductionConfirmation_Edit", new { Cnf_Number = _ProductionConfirmation_Model.confirmation_no, Cnf_Date = _ProductionConfirmation_Model.confirmation_dt, ListFilterData = _ProductionConfirmation_Model.ListFilterData1, WF_Status = _ProductionConfirmation_Model.WFStatus });
                            else
                                adddnew.Command = "Refresh";
                            adddnew.TransType = "Refresh";
                            adddnew.BtnName = "BtnRefresh";
                            adddnew.DocumentStatus = null;
                            TempData["ModelData"] = adddnew;
                            return RedirectToAction("ProductionConfirmationDetail", adddnew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ProductionConfirmationDetail", NewModel);

                    case "Edit":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("ProductionConfirmation_Edit", new { Cnf_Number = _ProductionConfirmation_Model.confirmation_no, Cnf_Date = _ProductionConfirmation_Model.confirmation_dt, ListFilterData = _ProductionConfirmation_Model.ListFilterData1, WF_Status = _ProductionConfirmation_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string CnfDt = _ProductionConfirmation_Model.confirmation_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrID, CnfDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("ProductionConfirmation_Edit", new { Cnf_Number = _ProductionConfirmation_Model.confirmation_no, Cnf_Date = _ProductionConfirmation_Model.confirmation_dt, ListFilterData = _ProductionConfirmation_Model.ListFilterData1, WF_Status = _ProductionConfirmation_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["Message"] = null;

                        //Session["PC_Number"] = _ProductionConfirmation_Model.confirmation_no;
                        //Session["PC_Date"] = _ProductionConfirmation_Model.confirmation_dt;
                        UrlModel EditModel = new UrlModel();
                        if (CheckQCAgainstCnf(_ProductionConfirmation_Model) == "Used")
                        {
                            _ProductionConfirmation_Model.Message = "Used";
                            EditModel.Cmd = "Refresh";
                            _ProductionConfirmation_Model.Command = "Refresh";
                            _ProductionConfirmation_Model.BtnName = "BtnEdit";
                            _ProductionConfirmation_Model.TransType = "Update";
                            _ProductionConfirmation_Model.PC_Number = _ProductionConfirmation_Model.confirmation_no;
                            _ProductionConfirmation_Model.PC_Date = _ProductionConfirmation_Model.confirmation_dt;

                        }
                        else
                        {
                            EditModel.Cmd = command;
                            _ProductionConfirmation_Model.Command = command;
                            _ProductionConfirmation_Model.BtnName = "BtnEdit";
                            _ProductionConfirmation_Model.TransType = "Update";
                            _ProductionConfirmation_Model.PC_Number = _ProductionConfirmation_Model.confirmation_no;
                            _ProductionConfirmation_Model.PC_Date = _ProductionConfirmation_Model.confirmation_dt;

                        }
                        TempData["ModelData"] = _ProductionConfirmation_Model;

                        EditModel.tp = "Update";
                        EditModel.bt = "BtnEdit";

                        EditModel.PAC_No = _ProductionConfirmation_Model.PC_Number;
                        EditModel.PAC_dt = _ProductionConfirmation_Model.PC_Date;
                        TempData["ListFilterData"] = _ProductionConfirmation_Model.ListFilterData1;
                        return RedirectToAction("ProductionConfirmationDetail", EditModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "Refresh";
                        ////PC_No = _ProductionConfirmation_Model.confirmation_no;
                        ////PC_Date = _ProductionConfirmation_Model.confirmation_dt;
                        ProductionConfirmationDelete(_ProductionConfirmation_Model, command);
                        ProductionConfirmation_Model DeleteModel = new ProductionConfirmation_Model();
                        DeleteModel.Message = "Deleted";
                        DeleteModel.Command = "Refresh";
                        DeleteModel.TransType = "Refresh";
                        DeleteModel.BtnName = "BtnRefresh";
                        TempData["ModelData"] = DeleteModel;
                        UrlModel Delete_Model = new UrlModel();
                        Delete_Model.Cmd = DeleteModel.Command;
                        Delete_Model.tp = "Refresh";
                        Delete_Model.bt = "BtnRefresh";
                        TempData["ListFilterData"] = _ProductionConfirmation_Model.ListFilterData1;
                        return RedirectToAction("ProductionConfirmationDetail", Delete_Model);

                    case "Save":
                        // Session["Command"] = command;                      
                        ProductionConfirmation_SaveUpdate(_ProductionConfirmation_Model);
                        if (_ProductionConfirmation_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_ProductionConfirmation_Model.Message == "DocModified")
                        {
                            if (Session["CompId"] != null)
                            {
                                CompID = Session["CompId"].ToString();
                            }
                            if (Session["BranchId"] != null)
                            {
                                BrID = Session["BranchId"].ToString();
                            }
                            if (Session["userid"] != null)
                            {
                                UserID = Session["userid"].ToString();
                            }
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrID, DocumentMenuId);
                            ViewBag.DocumentMenuId = DocumentMenuId;
                            CommonPageDetails();
                            // ProductionConfirmation_Model _ProductionConfirmation_Model = new ProductionConfirmation_Model();
                            List<OrderNumber> _OrderNumberList = new List<OrderNumber>();
                            OrderNumber _OrderNumber = new OrderNumber();
                            _OrderNumber.porder_no = _ProductionConfirmation_Model.hdn_order_no;
                            _OrderNumber.porder_dt = _ProductionConfirmation_Model.order_dt;
                            _OrderNumberList.Add(_OrderNumber);
                            _ProductionConfirmation_Model.OrderNumberList = _OrderNumberList;
                            _ProductionConfirmation_Model.confirmation_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                            _ProductionConfirmation_Model.product_id = _ProductionConfirmation_Model.product_id;
                            _ProductionConfirmation_Model.product_name = _ProductionConfirmation_Model.productname;
                            ViewBag.DocumentCode = _ProductionConfirmation_Model.status;
                            _ProductionConfirmation_Model.Title = title;
                            _ProductionConfirmation_Model.Docid = DocumentMenuId;
                            ViewBag.Command = _ProductionConfirmation_Model.Command;
                            ViewBag.TransType = _ProductionConfirmation_Model.TransType;
                            _ProductionConfirmation_Model.errorcmd = null;
                            _ProductionConfirmation_Model.DocumentStatus = "D";
                            ViewBag.DocumentStatus = _ProductionConfirmation_Model.DocumentStatus;
                            _ProductionConfirmation_Model.errorcmd = _ProductionConfirmation_Model.Message;
                            _ProductionConfirmation_Model.Remks = _ProductionConfirmation_Model.Remks;

                            ViewBag.ConsumptionItemDetails = ViewData["Consum_itemtable"];
                            ViewBag.OutputItemDetails = ViewData["output_itemtable"];
                            ViewBag.ItemStockBatchWise = ViewData["BatchWiseDetail"];
                            ViewBag.ItemStockSerialWise = ViewData["SerialWiseDetail"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetailsDt"];
                            return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionConfirmation/ProductionConfirmationDetail.cshtml", _ProductionConfirmation_Model);
                        }
                        TempData["ModelData"] = _ProductionConfirmation_Model;
                        UrlModel SaveModel = new UrlModel();
                        SaveModel.bt = _ProductionConfirmation_Model.BtnName;
                        SaveModel.PAC_No = _ProductionConfirmation_Model.PC_Number;
                        SaveModel.PAC_dt = _ProductionConfirmation_Model.PC_Date;
                        SaveModel.tp = _ProductionConfirmation_Model.TransType;
                        SaveModel.Cmd = _ProductionConfirmation_Model.Command;
                        //if (Session["PC_Number"] != null && Session["PC_Date"] != null)
                        //{
                        //    Session["PC_Number"] = Session["PC_Number"].ToString();
                        //    Session["PC_Date"] = Session["PC_Date"].ToString();
                        TempData["ListFilterData"] = _ProductionConfirmation_Model.ListFilterData1;
                        return RedirectToAction("ProductionConfirmationDetail", SaveModel);
                    //}
                    //else
                    //{
                    //    return View("~/Views/Shared/Error.cshtml");
                    //}

                    case "Approve":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("ProductionConfirmation_Edit", new { Cnf_Number = _ProductionConfirmation_Model.confirmation_no, Cnf_Date = _ProductionConfirmation_Model.confirmation_dt, ListFilterData = _ProductionConfirmation_Model.ListFilterData1, WF_Status = _ProductionConfirmation_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string CnfDt1 = _ProductionConfirmation_Model.confirmation_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrID, CnfDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("ProductionConfirmation_Edit", new { Cnf_Number = _ProductionConfirmation_Model.confirmation_no, Cnf_Date = _ProductionConfirmation_Model.confirmation_dt, ListFilterData = _ProductionConfirmation_Model.ListFilterData1, WF_Status = _ProductionConfirmation_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        // Session["Command"] = command;
                        //PC_No = _ProductionConfirmation_Model.confirmation_no;
                        //PC_Date = _ProductionConfirmation_Model.confirmation_dt;
                        ProductionConfirmation_Approve(_ProductionConfirmation_Model, "", "");
                        TempData["ModelData"] = _ProductionConfirmation_Model;
                        UrlModel Approve = new UrlModel();
                        Approve.tp = "Update";
                        Approve.PAC_No = _ProductionConfirmation_Model.PC_Number;
                        Approve.PAC_dt = _ProductionConfirmation_Model.PC_Date;
                        Approve.bt = "BtnEdit";
                        TempData["ListFilterData"] = _ProductionConfirmation_Model.ListFilterData1;
                        return RedirectToAction("ProductionConfirmationDetail", Approve);

                    case "Forward":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("ProductionConfirmation_Edit", new { Cnf_Number = _ProductionConfirmation_Model.confirmation_no, Cnf_Date = _ProductionConfirmation_Model.confirmation_dt, ListFilterData = _ProductionConfirmation_Model.ListFilterData1, WF_Status = _ProductionConfirmation_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                        string CnfDt2 = _ProductionConfirmation_Model.confirmation_dt;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrID, CnfDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("ProductionConfirmation_Edit", new { Cnf_Number = _ProductionConfirmation_Model.confirmation_no, Cnf_Date = _ProductionConfirmation_Model.confirmation_dt, ListFilterData = _ProductionConfirmation_Model.ListFilterData1, WF_Status = _ProductionConfirmation_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Refresh":
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        ProductionConfirmation_Model RefreshModel = new ProductionConfirmation_Model();
                        RefreshModel.Command = command;
                        RefreshModel.BtnName = "BtnRefresh";
                        RefreshModel.TransType = "Save";
                        TempData["ModelData"] = RefreshModel;
                        UrlModel refesh_Model = new UrlModel();
                        refesh_Model.tp = "Save";
                        refesh_Model.bt = "BtnRefresh";
                        refesh_Model.Cmd = command;
                        TempData["ListFilterData"] = _ProductionConfirmation_Model.ListFilterData1;
                        return RedirectToAction("ProductionConfirmationDetail", refesh_Model);

                    case "Print":
                        return GenratePdfFile(_ProductionConfirmation_Model);
                    case "BacktoList":
                        //Session.Remove("Message");
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        var WF_Status = _ProductionConfirmation_Model.WF_Status1;
                        TempData["ListFilterData"] = _ProductionConfirmation_Model.ListFilterData1;
                        return RedirectToAction("ProductionConfirmation", new { WF_Status });

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
        private string CheckQCAgainstCnf(ProductionConfirmation_Model PC_model)
        {
            try
            {
                string str = "";
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string br_id = Session["BranchId"].ToString();
                DataSet message = _ProductionConfirmation_IService.CheckQCAgainstCnf(CompID, br_id, PC_model.confirmation_no, PC_model.confirmation_dt);
                if (message.Tables.Count > 0)
                {
                    if (message.Tables[0].Rows.Count > 0)
                    {
                        str = "Used";
                    }
                }
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private ActionResult ProductionConfirmationDelete(ProductionConfirmation_Model _ProductionConfirmation_Model, string command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                string cnfNo = _ProductionConfirmation_Model.confirmation_no;
                string cnfNumber = cnfNo.Replace("/", "");

                DataSet Message = _ProductionConfirmation_IService.Delete_ProductionConfirmation(CompID, BrID, _ProductionConfirmation_Model.confirmation_no, _ProductionConfirmation_Model.confirmation_dt);

                /*---------Attachments Section Start----------------*/
                if (!string.IsNullOrEmpty(cnfNumber))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    other.DeleteTempFile(CompID + BrID, PageName, cnfNumber, Server);
                }
                /*---------Attachments Section End----------------*/


                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["PC_Number"] = _ProductionConfirmation_Model.confirmation_no;
                //Session["PC_Date"] = _ProductionConfirmation_Model.confirmation_dt;
                //Session["TransType"] = command;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("ProductionConfirmationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ProductionConfirmation_SaveUpdate(ProductionConfirmation_Model _ProductionConfirmation_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");

            try
            {

                if (_ProductionConfirmation_Model.CancelFlag == false)
                {
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        BrID = Session["BranchId"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        UserID = Session["userid"].ToString();
                    }
                    //if (Session["Document_Menu_Id"] != null)
                    //{
                    //    DocumentMenuId = Session["Document_Menu_Id"].ToString();
                    //}
                    DataTable ProdctionConfirmationHeader = new DataTable();
                    DataTable ConsumptionItemDetails = new DataTable();
                    DataTable OutputItemDetails = new DataTable();
                    DataTable ItemBatchDetails = new DataTable();
                    DataTable ItemSerialDetails = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("cnf_no", typeof(string));
                    dtheader.Columns.Add("cnf_dt", typeof(string));
                    dtheader.Columns.Add("product_id", typeof(string));
                    dtheader.Columns.Add("uom_id", typeof(string));
                    dtheader.Columns.Add("jc_no", typeof(string));
                    dtheader.Columns.Add("jc_dt", typeof(string));
                    dtheader.Columns.Add("jc_qty", typeof(string));
                    //dtheader.Columns.Add("pend_qty", typeof(string));
                    dtheader.Columns.Add("op_id", typeof(string));
                    dtheader.Columns.Add("shfl_id", typeof(string));
                    dtheader.Columns.Add("ws_id", typeof(string));
                    dtheader.Columns.Add("supervisor_name", typeof(string));
                    dtheader.Columns.Add("jc_st_date", typeof(string));
                    dtheader.Columns.Add("jc_en_date", typeof(string));
                    dtheader.Columns.Add("hrs", typeof(string));
                    dtheader.Columns.Add("shift_id", typeof(string));
                    dtheader.Columns.Add("cnf_remarks", typeof(string));
                    dtheader.Columns.Add("userid", typeof(string));
                    dtheader.Columns.Add("cnf_status", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));
                    dtheader.Columns.Add("op_out_item_id", typeof(string));
                    dtheader.Columns.Add("op_out_uom_id", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    //dtrowHeader["TransType"] = Session["TransType"].ToString();
                    if (_ProductionConfirmation_Model.confirmation_no != null)
                    {
                        dtrowHeader["TransType"] = "Update";
                    }
                    else
                    {
                        dtrowHeader["TransType"] = "Save";
                    }
                    dtrowHeader["comp_id"] = Session["CompId"].ToString();
                    //dtrowHeader["br_id"] = Session["BranchId"].ToString();
                    dtrowHeader["br_id"] = BrID;
                    dtrowHeader["cnf_no"] = _ProductionConfirmation_Model.confirmation_no;
                    dtrowHeader["cnf_dt"] = _ProductionConfirmation_Model.confirmation_dt;
                    dtrowHeader["product_id"] = _ProductionConfirmation_Model.product_id;
                    dtrowHeader["uom_id"] = _ProductionConfirmation_Model.uom_id;
                    dtrowHeader["jc_no"] = _ProductionConfirmation_Model.hdn_order_no;
                    dtrowHeader["jc_dt"] = _ProductionConfirmation_Model.order_dt;
                    dtrowHeader["jc_qty"] = _ProductionConfirmation_Model.order_qty;
                    // dtrowHeader["pend_qty"] = _ProductionConfirmation_Model.pending_qty;
                    dtrowHeader["op_id"] = _ProductionConfirmation_Model.operation_id;
                    dtrowHeader["shfl_id"] = _ProductionConfirmation_Model.shopfloor_id;
                    //dtrowHeader["ws_id"] = _ProductionConfirmation_Model.workstation;/*commented and add by Hina on 20-09-2024 to add for dropdown*/
                    dtrowHeader["ws_id"] = _ProductionConfirmation_Model.ddl_WorkstationName;
                    dtrowHeader["supervisor_name"] = _ProductionConfirmation_Model.supervisorName;
                    dtrowHeader["jc_st_date"] = _ProductionConfirmation_Model.jobstartdate.Replace("T", " ");
                    dtrowHeader["jc_en_date"] = _ProductionConfirmation_Model.jobenddate.Replace("T", " ");
                    dtrowHeader["hrs"] = _ProductionConfirmation_Model.hours;
                    dtrowHeader["shift_id"] = _ProductionConfirmation_Model.shift_id;
                    dtrowHeader["cnf_remarks"] = _ProductionConfirmation_Model.Remks;
                    dtrowHeader["userid"] = UserID;
                    dtrowHeader["cnf_status"] = _ProductionConfirmation_Model.StatusCode;
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    dtrowHeader["mac_id"] = SystemDetail;
                    dtrowHeader["op_out_item_id"] = _ProductionConfirmation_Model.op_output_itemid;
                    dtrowHeader["op_out_uom_id"] = _ProductionConfirmation_Model.op_output_uom_id;

                    dtheader.Rows.Add(dtrowHeader);
                    ProdctionConfirmationHeader = dtheader;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("cons_qty", typeof(string));
                    dtItem.Columns.Add("remarks", typeof(string));

                    JArray jObject = JArray.Parse(_ProductionConfirmation_Model.ConsumptionItemDetail);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                        dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                        dtrowLines["cons_qty"] = jObject[i]["ConsumedQuantity"].ToString();
                        dtrowLines["remarks"] = jObject[i]["remarks"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ConsumptionItemDetails = dtItem;

                    DataTable dtOutputItem = new DataTable();
                    dtOutputItem.Columns.Add("product_id", typeof(string));
                    dtOutputItem.Columns.Add("uom_id", typeof(int));
                    dtOutputItem.Columns.Add("prod_qty", typeof(string));
                    dtOutputItem.Columns.Add("lot_no", typeof(string));
                    dtOutputItem.Columns.Add("batch_no", typeof(string));
                    dtOutputItem.Columns.Add("ex_date", typeof(string));
                    dtOutputItem.Columns.Add("remarks", typeof(string));

                    JArray jOutputObject = JArray.Parse(_ProductionConfirmation_Model.OutputItemDetail);
                    for (int i = 0; i < jOutputObject.Count; i++)
                    {
                        DataRow dtOutputrowLines = dtOutputItem.NewRow();
                        dtOutputrowLines["product_id"] = jOutputObject[i]["ProductId"].ToString();
                        dtOutputrowLines["uom_id"] = jOutputObject[i]["UOMId"].ToString();
                        dtOutputrowLines["prod_qty"] = jOutputObject[i]["ProducedQuantity"].ToString();
                        dtOutputrowLines["lot_no"] = jOutputObject[i]["lot"].ToString();
                        dtOutputrowLines["batch_no"] = jOutputObject[i]["BatchNo"].ToString();
                        dtOutputrowLines["ex_date"] = jOutputObject[i]["ExDate"].ToString();
                        dtOutputrowLines["remarks"] = jOutputObject[i]["remarks"].ToString();
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
                    if (_ProductionConfirmation_Model.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_ProductionConfirmation_Model.ItemBatchWiseDetail);
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

                    if (_ProductionConfirmation_Model.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_ProductionConfirmation_Model.ItemSerialWiseDetail);
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

                    /*-----------------Attachment Section Start------------------------*/
                    DataTable CnfAttachments = new DataTable();
                    DataTable CnfdtAttachment = new DataTable();
                    var attachData = TempData["IMGDATA"] as Pro_Model;
                    TempData["IMGDATA"] = null;
                    if (_ProductionConfirmation_Model.attatchmentdetail != null)
                    {
                        if (attachData != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)
                            //{
                            //    CnfdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                            //}
                            if (attachData.AttachMentDetailItmStp != null)
                            {
                                CnfdtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                CnfdtAttachment.Columns.Add("id", typeof(string));
                                CnfdtAttachment.Columns.Add("file_name", typeof(string));
                                CnfdtAttachment.Columns.Add("file_path", typeof(string));
                                CnfdtAttachment.Columns.Add("file_def", typeof(char));
                                CnfdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        else
                        {
                            if (_ProductionConfirmation_Model.AttachMentDetailItmStp != null)
                            {
                                CnfdtAttachment = _ProductionConfirmation_Model.AttachMentDetailItmStp as DataTable;
                            }
                            else
                            {
                                CnfdtAttachment.Columns.Add("id", typeof(string));
                                CnfdtAttachment.Columns.Add("file_name", typeof(string));
                                CnfdtAttachment.Columns.Add("file_path", typeof(string));
                                CnfdtAttachment.Columns.Add("file_def", typeof(char));
                                CnfdtAttachment.Columns.Add("comp_id", typeof(Int32));

                            }
                        }
                        JArray jObject1 = JArray.Parse(_ProductionConfirmation_Model.attatchmentdetail);
                        for (int i = 0; i < jObject1.Count; i++)
                        {
                            string flag = "Y";
                            foreach (DataRow dr in CnfdtAttachment.Rows)
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

                                DataRow dtrowAttachment1 = CnfdtAttachment.NewRow();
                                if (!string.IsNullOrEmpty((_ProductionConfirmation_Model.confirmation_no).ToString()))
                                {
                                    dtrowAttachment1["id"] = _ProductionConfirmation_Model.confirmation_no;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                CnfdtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        //if (Session["TransType"].ToString() == "Update")
                        if (_ProductionConfirmation_Model.TransType == "Update")
                        {
                            //string brnch_id = dtrowHeader["br_id"].ToString();
                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName;
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string CNF_CODE = string.Empty;
                                if (!string.IsNullOrEmpty((_ProductionConfirmation_Model.confirmation_no).ToString()))
                                {
                                    CNF_CODE = (_ProductionConfirmation_Model.confirmation_no).ToString();

                                }
                                else
                                {
                                    CNF_CODE = "0";
                                }
                                string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + BrID + CNF_CODE.Replace("/", "") + "*");

                                foreach (var fielpath in filePaths)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in CnfdtAttachment.Rows)
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
                        CnfAttachments = CnfdtAttachment;
                    }
                    /*-----------------Attachment Section End------------------------*/


                    //string br_id = Session["BranchId"].ToString();
                    /*------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));

                    if (_ProductionConfirmation_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_ProductionConfirmation_Model.SubItemDetailsDt);
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

                    SaveMessage = _ProductionConfirmation_IService.InsertUpdate_ProductionConfirmation(ProdctionConfirmationHeader, ConsumptionItemDetails, OutputItemDetails, ItemBatchDetails, ItemSerialDetails, CnfAttachments, dtSubItem);

                    string[] FData = SaveMessage.Split(',');

                    string Message = FData[0].ToString();
                    string PCnfNumber = FData[1].ToString();
                    string Pcnf_Number = PCnfNumber.Replace("/", "");
                    if (Message == "Data_Not_Found")
                    {
                        //var a = SaveMessage.Split(',');
                        var msgs = Message.Replace("_", " ") + " " + PCnfNumber + " in " + PageName;//ProdOrdCode is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msgs, "", "");
                        _ProductionConfirmation_Model.Message = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("ProductionConfirmationDetail");
                    }
                    if (Message == "DocModified")
                    {
                        /****** This Massage Created By Nitesh 14-09-2023 
                         used in New Data Insert by use src_doc_no or Item_Name  When add data in table and other User Cancel or Force
                        this  (src_doc_no or Item Name) Document in refrence Page When Data is Not Insert and Give massage NotModified
                        and retun to case(Save) and Show Data User insert but Not Save **********/
                        _ProductionConfirmation_Model.Message = Message;
                        _ProductionConfirmation_Model.BtnName = "BtnRefresh";
                        _ProductionConfirmation_Model.Command = "Refresh";
                        _ProductionConfirmation_Model.batchCommand = "Refresh";
                        _ProductionConfirmation_Model.TransType_Modified = "Refresh";
                        _ProductionConfirmation_Model.TransType = "Refresh";
                        ViewData["Consum_itemtable"] = DocModified_ConsumptionItemTable(_ProductionConfirmation_Model);
                        ViewData["output_itemtable"] = DocModified_OutputItemDetails(_ProductionConfirmation_Model);
                        ViewData["BatchWiseDetail"] = Doc_ModifiedItemBatchWiseDetail(_ProductionConfirmation_Model);
                        ViewData["SerialWiseDetail"] = DocmodifiedItemSerialWiseDetail(_ProductionConfirmation_Model);
                        ViewData["SubItemDetailsDt"] = DocModifiedSubItemDetailsDt(_ProductionConfirmation_Model);
                        return RedirectToAction("ProductionConfirmationDetail");
                    }
                    /*-----------------Attachment Section Start------------------------*/
                    if (Message == "Save")

                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (attachData != null)
                        {
                            if (attachData.Guid != null)
                            {
                                Guid = attachData.Guid;
                            }
                        }
                        string guid = Guid;
                        var comCont = new CommonController(_Common_IServices);
                        comCont.ResetImageLocation(CompID, BrID, guid, PageName, PCnfNumber, _ProductionConfirmation_Model.TransType, CnfAttachments);
                        //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                        //if (Directory.Exists(sourcePath))
                        //{
                        //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrID + Guid + "_" + "*");
                        //    foreach (string file in filePaths)
                        //    {
                        //        string[] items = file.Split('\\');
                        //        string ItemName = items[items.Length - 1];
                        //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                        //        foreach (DataRow dr in CnfAttachments.Rows)
                        //        {
                        //            string DrItmNm = dr["file_name"].ToString();
                        //            if (ItemName == DrItmNm)
                        //            {
                        //                string img_nm = CompID + BrID + Pcnf_Number + "_" + Path.GetFileName(DrItmNm).ToString();
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


                    if (Message == "StockNotAvail")
                    {
                        //Session["Message"] = "StockNotAvail";

                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = "Refresh";
                        //Session["TransType"] = "Save";
                        //Session["DocumentStatus"] = null;
                        //Session["PC_Number"] = PCnfNumber;
                        //Session["PC_Date"] = _ProductionConfirmation_Model.confirmation_dt;
                        _ProductionConfirmation_Model.Message = "StockNotAvail";
                        _ProductionConfirmation_Model.PC_Number = PCnfNumber;
                        _ProductionConfirmation_Model.PC_Date = _ProductionConfirmation_Model.confirmation_dt;
                        _ProductionConfirmation_Model.TransType = "Update";
                        _ProductionConfirmation_Model.BtnName = "BtnRefresh";
                        _ProductionConfirmation_Model.Command = "Refresh";
                    }
                    if (Message == "Update" || Message == "Save")
                    {
                        //Session["PC_Number"] = PCnfNumber;
                        //Session["PC_Date"] = _ProductionConfirmation_Model.confirmation_dt;

                        //Session["Message"] = "Save";
                        //Session["Command"] = "Update";
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnEdit";
                        _ProductionConfirmation_Model.Message = "Save";
                        _ProductionConfirmation_Model.PC_Number = PCnfNumber;
                        _ProductionConfirmation_Model.PC_Date = _ProductionConfirmation_Model.confirmation_dt;
                        _ProductionConfirmation_Model.TransType = "Update";
                        _ProductionConfirmation_Model.BtnName = "BtnEdit";
                    }
                    return RedirectToAction("ProductionConfirmationDetail");
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
                    if (Session["Document_Menu_Id"] != null)
                    {
                        DocumentMenuId = Session["Document_Menu_Id"].ToString();
                    }

                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet message = _ProductionConfirmation_IService.Cancel_ProductionConfirmation(CompID, br_id, _ProductionConfirmation_Model.confirmation_no, _ProductionConfirmation_Model.confirmation_dt, UserID, DocumentMenuId, mac_id);

                    // _ProductionConfirmation_Model.Message = message.Tables[0].Rows[0]["result"].ToString();
                    _ProductionConfirmation_Model.PC_Number = _ProductionConfirmation_Model.confirmation_no;
                    _ProductionConfirmation_Model.PC_Date = _ProductionConfirmation_Model.confirmation_dt;
                    _ProductionConfirmation_Model.TransType = "Update";
                    _ProductionConfirmation_Model.BtnName = "Refresh";
                    try
                    {
                        //string fileName = "PC_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "ProductionConfirmation_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_ProductionConfirmation_Model.confirmation_no, _ProductionConfirmation_Model.confirmation_dt, fileName, DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _ProductionConfirmation_Model.confirmation_no, "C", UserID, "0", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _ProductionConfirmation_Model.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _ProductionConfirmation_Model.Message = _ProductionConfirmation_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    //    Session["PC_Number"] = _ProductionConfirmation_Model.confirmation_no;
                    //Session["PC_Date"] = _ProductionConfirmation_Model.confirmation_dt;

                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    return RedirectToAction("ProductionConfirmationDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ProductionConfirmation_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ProductionConfirmation_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ProductionConfirmation_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrID, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                throw ex;
                // return View("~/Views/Shared/Error.cshtml");
            }
        }
        public DataTable DocModifiedSubItemDetailsDt(ProductionConfirmation_Model _ProductionConfirmation_Model)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("Qty", typeof(string));

            if (_ProductionConfirmation_Model.SubItemDetailsDt != null)
            {
                JArray jObject2 = JArray.Parse(_ProductionConfirmation_Model.SubItemDetailsDt);
                for (int i = 0; i < jObject2.Count; i++)
                {
                    DataRow dtrowItemdetails = dtSubItem.NewRow();
                    dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                    dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                    dtrowItemdetails["Qty"] = jObject2[i]["qty"].ToString();
                    dtSubItem.Rows.Add(dtrowItemdetails);
                }
            }
            return dtSubItem;
        }
        public DataTable DocmodifiedItemSerialWiseDetail(ProductionConfirmation_Model _ProductionConfirmation_Model)
        {
            DataTable Serial_detail = new DataTable();
            Serial_detail.Columns.Add("item_id", typeof(string));
            Serial_detail.Columns.Add("uom_id", typeof(int));
            Serial_detail.Columns.Add("serial_no", typeof(string));
            Serial_detail.Columns.Add("serial_qty", typeof(string));
            Serial_detail.Columns.Add("lot_no", typeof(string));

            if (_ProductionConfirmation_Model.ItemSerialWiseDetail != null)
            {
                JArray jObjectSerial = JArray.Parse(_ProductionConfirmation_Model.ItemSerialWiseDetail);
                for (int i = 0; i < jObjectSerial.Count; i++)
                {
                    DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                    dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                    dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                    dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                    dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                    dtrowSerialDetailsLines["lot_id"] = jObjectSerial[i]["LOTId"].ToString();
                    Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                }
            }
            return Serial_detail;
        }

        public DataTable Doc_ModifiedItemBatchWiseDetail(ProductionConfirmation_Model _ProductionConfirmation_Model)
        {
            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("uom_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("expiry_date", typeof(string));
            Batch_detail.Columns.Add("exp_dt", typeof(string));
            Batch_detail.Columns.Add("batch_qty", typeof(string));
            Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
            Batch_detail.Columns.Add("lot_no", typeof(string));
            Batch_detail.Columns.Add("lot_id", typeof(string));
            Batch_detail.Columns.Add("issue_qty", typeof(string));
            if (_ProductionConfirmation_Model.ItemBatchWiseDetail != null)
            {
                JArray jObjectBatch = JArray.Parse(_ProductionConfirmation_Model.ItemBatchWiseDetail);
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
                    dtrowBatchDetailsLines["lot_id"] = jObjectBatch[i]["LotNo"].ToString();
                    dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                    dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["ExpiryDate"].ToString();

                    Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                }
            }
            return Batch_detail;
        }
        public DataTable DocModified_ConsumptionItemTable(ProductionConfirmation_Model _ProductionConfirmation_Model)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(string));
            dtItem.Columns.Add("uom_alias", typeof(string));
            dtItem.Columns.Add("avl_stock_shfl", typeof(string));
            dtItem.Columns.Add("foronereqqty", typeof(string));
            dtItem.Columns.Add("cons_qty", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));

            JArray jObject = JArray.Parse(_ProductionConfirmation_Model.ConsumptionItemDetail);
            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowLines["item_name"] = jObject[i]["item_name"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                dtrowLines["uom_alias"] = jObject[i]["uom_alias"].ToString();
                dtrowLines["avl_stock_shfl"] = jObject[i]["avl_stock_shfl"].ToString();
                dtrowLines["foronereqqty"] = jObject[i]["foronereqqty"].ToString();
                dtrowLines["cons_qty"] = jObject[i]["ConsumedQuantity"].ToString();
                dtrowLines["i_batch"] = jObject[i]["hdi_cbatch"].ToString();
                dtrowLines["i_serial"] = jObject[i]["hdi_cserial"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }
            //ConsumptionItemDetails = dtItem;
            return dtItem;
        }

        public DataTable DocModified_OutputItemDetails(ProductionConfirmation_Model _ProductionConfirmation_Model)
        {

            DataTable dtOutputItem = new DataTable();
            dtOutputItem.Columns.Add("product_id", typeof(string));
            dtOutputItem.Columns.Add("item_name", typeof(string));
            dtOutputItem.Columns.Add("uom_id", typeof(string));
            dtOutputItem.Columns.Add("uom_alias", typeof(string));
            dtOutputItem.Columns.Add("prod_qty", typeof(string));
            dtOutputItem.Columns.Add("lot_no", typeof(string));
            dtOutputItem.Columns.Add("batch_no", typeof(string));
            dtOutputItem.Columns.Add("exp_dt", typeof(string));
            dtOutputItem.Columns.Add("item_type", typeof(string));
            dtOutputItem.Columns.Add("sub_item", typeof(string));
            dtOutputItem.Columns.Add("i_exp", typeof(string));
            dtOutputItem.Columns.Add("accept_qty", typeof(string));
            dtOutputItem.Columns.Add("reject_qty", typeof(string));
            dtOutputItem.Columns.Add("rework_qty", typeof(string));

            JArray jOutputObject = JArray.Parse(_ProductionConfirmation_Model.OutputItemDetail);
            for (int i = 0; i < jOutputObject.Count; i++)
            {
                DataRow dtOutputrowLines = dtOutputItem.NewRow();
                dtOutputrowLines["product_id"] = jOutputObject[i]["ProductId"].ToString();
                dtOutputrowLines["item_name"] = jOutputObject[i]["item_name"].ToString();
                dtOutputrowLines["uom_id"] = jOutputObject[i]["UOMId"].ToString();
                dtOutputrowLines["uom_alias"] = jOutputObject[i]["uom_alias"].ToString();
                dtOutputrowLines["prod_qty"] = jOutputObject[i]["ProducedQuantity"].ToString();
                dtOutputrowLines["lot_no"] = jOutputObject[i]["lot"].ToString();
                dtOutputrowLines["batch_no"] = jOutputObject[i]["BatchNo"].ToString();
                dtOutputrowLines["exp_dt"] = jOutputObject[i]["ExDate"].ToString();
                dtOutputrowLines["exp_dt"] = jOutputObject[i]["ExDate"].ToString();
                dtOutputrowLines["item_type"] = jOutputObject[i]["Item_type"].ToString();
                dtOutputrowLines["sub_item"] = jOutputObject[i]["sub_item"].ToString();
                dtOutputrowLines["i_exp"] = jOutputObject[i]["i_exp"].ToString();
                dtOutputrowLines["accept_qty"] = jOutputObject[i]["OP_QCAcceptQty"].ToString();
                dtOutputrowLines["reject_qty"] = jOutputObject[i]["OP_QCRejectQty"].ToString();
                dtOutputrowLines["rework_qty"] = jOutputObject[i]["OP_QCReworkQty"].ToString();
                dtOutputItem.Rows.Add(dtOutputrowLines);
            }
            return dtOutputItem;
        }

        public ActionResult ProductionConfirmation_Approve(ProductionConfirmation_Model _ProductionConfirmation_Model, string ListFilterData1, string WF_Status1)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Cnf_No = _ProductionConfirmation_Model.confirmation_no;
                string Cnf_Date = _ProductionConfirmation_Model.confirmation_dt;
                string A_Status = _ProductionConfirmation_Model.A_Status;
                string A_Level = _ProductionConfirmation_Model.A_Level;
                string A_Remarks = _ProductionConfirmation_Model.A_Remarks;
                string Message = _ProductionConfirmation_IService.Approve_ProductionConfirmation(CompID, BrID, DocumentMenuId, Cnf_No, Cnf_Date, UserID, mac_id, A_Status, A_Level, A_Remarks);
                string[] FDetail = Message.Split(',');
                string data = FDetail[0].ToString();
                if(data== "StockNotAvail")
                {
                    _ProductionConfirmation_Model.StockItemWiseMessage = string.Join(",", FDetail.Skip(1));
                    _ProductionConfirmation_Model.Message = "StockNotAvail";
                }
                else
                {
                    if (Message == "Approved")
                    {
                        try
                        {
                            //string fileName = "PC_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = "ProductionConfirmation_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(Cnf_No, Cnf_Date, fileName, DocumentMenuId, "AP");
                            _Common_IServices.SendAlertEmail(CompID, BrID, DocumentMenuId, Cnf_No, "AP", UserID, "0", filePath);
                        }
                        catch (Exception exMail)
                        {
                            _ProductionConfirmation_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        //Session["Message"] = "Approved";
                        // _ProductionConfirmation_Model.Message = "Approved";
                        _ProductionConfirmation_Model.Message = _ProductionConfirmation_Model.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    }
                }
                
                //if (Message == "StockNotAvail")
                //{
                //    //Session["Message"] = "StockNotAvail";
                //    _ProductionConfirmation_Model.Message = "StockNotAvail";
                //}
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["PC_Number"] = Cnf_No;
                //Session["PC_Date"] = Cnf_Date;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                UrlModel ApproveModel = new UrlModel();
                _ProductionConfirmation_Model.PC_Number = Cnf_No;
                _ProductionConfirmation_Model.PC_Date = Cnf_Date;
                _ProductionConfirmation_Model.TransType = "Update";
                _ProductionConfirmation_Model.BtnName = "BtnEdit";
                _ProductionConfirmation_Model.Command = "Approve";
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _ProductionConfirmation_Model.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }
                TempData["ModelData"] = _ProductionConfirmation_Model;

                ApproveModel.tp = "Update";
                ApproveModel.PAC_No = _ProductionConfirmation_Model.PC_Number;
                ApproveModel.PAC_dt = _ProductionConfirmation_Model.PC_Date;
                ApproveModel.bt = "BtnEdit";
                ApproveModel.Cmd = "Approve";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("ProductionConfirmationDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        private DataTable GetProductionCnfDetails(ProductionConfirmation_Model _ProductionConfirmation_Model)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
            }
            string wfstatus = "";
            //if (Session["WF_status"] != null)
            //{
            //    wfstatus = Session["WF_status"].ToString();
            //}
            if (_ProductionConfirmation_Model.WF_Status != null)
            {
                wfstatus = _ProductionConfirmation_Model.WF_Status;
            }
            else
            {
                wfstatus = "";
            }

            DataSet dt = _ProductionConfirmation_IService.GetProductionConfirmationList(CompID, BrID, UserID, wfstatus, DocumentMenuId);
            if (dt.Tables[1].Rows.Count > 0)
            {
                //FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
            }

            //DataTable Cnf_DT = _ProductionConfirmation_IService.GetProductionConfirmationDetailByNo(CompID, BrID);
            return dt.Tables[0];
        }
        public ActionResult ProductionConfirmation_Edit(string Cnf_Number, string Cnf_Date, string ListFilterData, string WF_Status)
        {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            //Session["Message"] = "New";
            //Session["Command"] = "Update";
            //Session["PC_Number"] = Cnf_Number;
            //Session["PC_Date"] = Cnf_Date;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnEdit";
            ProductionConfirmation_Model dblclick = new ProductionConfirmation_Model();
            UrlModel _url = new UrlModel();
            dblclick.PC_Number = Cnf_Number;
            dblclick.PC_Date = Cnf_Date;
            dblclick.TransType = "Update";
            dblclick.BtnName = "BtnEdit";
            dblclick.Command = "Refresh";
            if (WF_Status != null && WF_Status != "")
            {
                _url.wf = WF_Status;
                dblclick.WF_Status1 = WF_Status;
            }
            TempData["ModelData"] = dblclick;
            //_url.Cmd = "Update";
            _url.tp = "Update";
            _url.bt = "BtnEdit";
            _url.PAC_No = Cnf_Number;
            _url.PAC_dt = Cnf_Date;
            _url.Cmd = "Refresh";
            TempData["ListFilterData"] = ListFilterData;

            return RedirectToAction("ProductionConfirmationDetail", _url);
        }
        public ActionResult ProductionConfirmation_Search(string ProductID, string Fromdate, string Todate, string Status, string OPID, string ShflID, string WSID, string ShftID)
        {
            try
            {
                ProductionConfirmation_Model _ProductionConfirmation_Model = new ProductionConfirmation_Model();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                // Session["WF_status"] = null;
                _ProductionConfirmation_Model.WF_Status = null;
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }

                DataTable dt = _ProductionConfirmation_IService.GetProductionConfirmationFilter(ProductID, Fromdate, Todate, Status, CompID, BrID, DocumentMenuId, OPID, ShflID, WSID, ShftID);
                //Session["CNFSearch"] = "CNF_Search";
                _ProductionConfirmation_Model.CNFSearch = "CNF_Search";
                ViewBag.ProductionCnfList = dt;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/_ProductionConfirmationList.cshtml", _ProductionConfirmation_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData,string Mailerror)
        {
            //Session["Message"] = "";
            ProductionConfirmation_Model ToRefreshByJS = new ProductionConfirmation_Model();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.PC_Number = a[0].Trim();
            ToRefreshByJS.PC_Date = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnEdit";
            ToRefreshByJS.Message = Mailerror;
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wf = a[2].Trim();
            }
            Model.bt = "BtnEdit";
            Model.PAC_No = ToRefreshByJS.PC_Number;
            Model.PAC_dt = ToRefreshByJS.PC_Date;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("ProductionConfirmationDetail", Model);
        }

        public ActionResult GetProductionConfirmationList(string docid, string status)
        {

            //  Session["WF_status"] = status;
            var WF_status = status;
            return RedirectToAction("ProductionConfirmation", new { WF_status });
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
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status1)
        {

            ProductionConfirmation_Model _ProductionConfirmation_Model = new ProductionConfirmation_Model();
            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _ProductionConfirmation_Model.confirmation_no = jObjectBatch[i]["cnf_no"].ToString();
                    _ProductionConfirmation_Model.confirmation_dt = jObjectBatch[i]["cnf_dt"].ToString();
                    _ProductionConfirmation_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _ProductionConfirmation_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _ProductionConfirmation_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                }
            }
            if (_ProductionConfirmation_Model.A_Status != "Approve")
            {
                _ProductionConfirmation_Model.A_Status = "Approve";
            }
            ProductionConfirmation_Approve(_ProductionConfirmation_Model, ListFilterData1, WF_Status1);
            UrlModel ApproveModel = new UrlModel();
            if (WF_Status1 != null && WF_Status1 != "")
            {
                ApproveModel.wf = WF_Status1;
            }
            TempData["ModelData"] = _ProductionConfirmation_Model;
            ApproveModel.tp = "Update";
            ApproveModel.PAC_No = _ProductionConfirmation_Model.PC_Number;
            ApproveModel.PAC_dt = _ProductionConfirmation_Model.PC_Date;
            ApproveModel.bt = "BtnEdit";
            return RedirectToAction("ProductionConfirmationDetail", ApproveModel);
        }


        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Pro_Model _attachmentModel = new Pro_Model();
                //string TransType = "";
                //string CnfCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["PC_Number"] != null)
                //{
                //    CnfCode = Session["PC_Number"].ToString();
                //}
                if (TransType == "Save")
                {
                    //CnfCode = gid.ToString();
                    DocNo = gid.ToString();
                }
                //CnfCode = CnfCode.Replace("/", "");
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = CnfCode;
                _attachmentModel.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                // getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _attachmentModel.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _attachmentModel.AttachMentDetailItmStp = null;
                }
                TempData["IMGDATA"] = _attachmentModel;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        /*-----------------Attachment Section End------------------------*/


        public ActionResult GetSubItemDetails(string Item_id, string Uom_id, string SubItemListwithPageData, string IsDisabled, string Flag, string Shfl_id, string Status, string Doc_no, string Doc_dt, string Prod_no, string Prod_Dt)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();
                if (Flag == "Quantity" || Flag == "PC_ConsumeQuantity")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        if (Flag == "Quantity")
                        {
                            //dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                            dt = _ProductionConfirmation_IService.GetConsumeSubItemDetails(CompID, null, Item_id, null, Prod_no, Prod_Dt, null).Tables[0];
                        }
                        else
                        {
                            //if (Flag != "PC_ConsumeQuantity")
                            //{
                            dt = _ProductionConfirmation_IService.GetConsumeSubItemShflAvlstockDetails(CompID, BrID, Item_id, Uom_id, Shfl_id).Tables[0];
                            //}
                            //else
                            //{
                            //dt = _ProductionConfirmation_IService.GetConsumeSubItemDetails(CompID, BrID, Item_id, Shfl_id, Prod_no, Prod_Dt, Uom_id).Tables[0];
                            //}
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
                        dt = _ProductionConfirmation_IService.PC_GetSubItemDetails(CompID, BrID, Item_id, Uom_id, Doc_no, Doc_dt, Flag, Shfl_id).Tables[0];
                    }
                }
                else
                {
                    dt = _ProductionConfirmation_IService.PC_GetSubItemDetails(CompID, BrID, Item_id, Uom_id, Doc_no, Doc_dt, Flag, Shfl_id).Tables[0];
                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    decimalAllowed = Flag == "PC_ConsumeQuantity" ? "Y" : "",
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    _subitemPageName = "CNF"
                };
                //ViewBag._subitemPageName = "CNF";
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
        /*--------Print---------*/

        public FileResult GenratePdfFile(ProductionConfirmation_Model _Model)
        {
            var data = GetPdfData(_Model.confirmation_no, _Model.confirmation_dt);
            if (data != null)
                return File(data, "application/pdf", "ProductionConfirmation.pdf");
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
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataSet Deatils = new DataSet();
                Deatils = _ProductionConfirmation_IService.GetProductionConfirmationPrintDeatils(CompID, BrID, confirmationNo, confirmationDate);

                ViewBag.PageName = "PC";
                ViewBag.Title = "Production Confirmation";
                ViewBag.Details = Deatils;
                //ViewBag.CompLogoDtl = Deatils.Tables[0];
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionConfirmation/ProductionConfirmationPrint.cshtml"));
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
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrID = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, BrID, docid, docstatus);
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
        [HttpPost]
        public ActionResult GetQcDetailItemWise(string cnf_no, string cnf_dt, string ItemId, string UOMId)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrID = Session["BranchId"].ToString();
                }
                DataSet ds = _ProductionConfirmation_IService.GetQcDetail(CompID, BrID, ItemId, UOMId, cnf_no, cnf_dt);
                ViewBag.QcDeatl = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialQCDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

    }

}