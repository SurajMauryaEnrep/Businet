using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ProductionOrder;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ProductionOrder;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using System.IO;
using System.Web;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Linq;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.ProductionOrder
{
    public class ProductionOrderController : Controller
    {
        string DocumentMenuId = "105105125", title;
        string CompID,BranchId, UserID,language = String.Empty;
        string br_id = "";

        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        private readonly Common_IServices _Common_IServices;
        private readonly ProductionOrder_ISERVICES _ProductionOrder_ISERVICES;
        DataSet dtSet;
        public ProductionOrderController(Common_IServices _Common_IServices, ProductionOrder_ISERVICES _ProductionOrder_ISERVICES)
        {
            this._ProductionOrder_ISERVICES = _ProductionOrder_ISERVICES;
            this._Common_IServices = _Common_IServices;
        }
        // GET: ApplicationLayer/ProductionOrder
        ProductionOrderModel _ProductionOrderModel = new ProductionOrderModel();
        public ActionResult ProductionOrder(string WF_Status)//List Page Load
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                ProductionOrderModel _ProductionOrderModel = new ProductionOrderModel();
                _ProductionOrderModel.WF_Status = WF_Status;
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _ProductionOrderModel.Title = title;
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                _ProductionOrderModel.txtFromdate = startDate;
                _ProductionOrderModel.txtToDate = CurrentDate;


                dtSet = PrdOrdGetAllData(_ProductionOrderModel);
                /*commented by Hina on 06-04-2024 to combine all list Procedure  in single Procedure*/
                //ViewBag.GetJCList = GetJCList(_ProductionOrderModel);
                //if (dtSet.Tables[0].Rows.Count > 0)
                //{
                    DateTime dtime = DateTime.Now;
                    _ProductionOrderModel.txtToDate = dtime.ToString("yyyy-MM-dd");
                    ViewBag.GetJCList = dtSet;
                //}
                /*commented by Hina on 06-04-2024 to combine all list Procedure  in single Procedure*/
                //BindShopFloorList(_ProductionOrderModel);
                List<ShopFloor> _ShopFloor = new List<ShopFloor>();
                ShopFloor _Shfllist = new ShopFloor();
                _Shfllist.shfl_id = "0";
                _Shfllist.shfl_name = "---Select---";
                _ShopFloor.Add(_Shfllist);
                foreach (DataRow dr in dtSet.Tables[2].Rows)
                {
                    ShopFloor _Statuslist1 = new ShopFloor();
                    _Statuslist1.shfl_id = dr["shfl_id"].ToString();
                    _Statuslist1.shfl_name = dr["shfl_name"].ToString();
                    _ShopFloor.Add(_Statuslist1);
                }
                _ProductionOrderModel.ShopFloorList = _ShopFloor;

                List<OperationName> _op = new List<OperationName>();
                OperationName _opObj = new OperationName();
                _opObj.op_id = "0";
                _opObj.op_name = "---Select---";
                _op.Add(_opObj);
                foreach (DataRow dr in dtSet.Tables[3].Rows)
                {
                    OperationName _oplist = new OperationName();
                    _oplist.op_id = dr["op_id"].ToString();
                    _oplist.op_name = dr["op_name"].ToString();
                    _op.Add(_oplist);
                }
                _ProductionOrderModel.OperationNameList = _op;

                GetshiftList(_ProductionOrderModel);/*Add by Hina Sharma on 07-03-2025 for filter*/

                List<WorkstationName> ws = new List<WorkstationName>();/*Add by Hina Sharma on 07-03-2025 for filter*/
                WorkstationName wsObj = new WorkstationName();
                wsObj.ws_id = "0";
                wsObj.ws_name = "---Select---";
                ws.Add(wsObj);
                foreach (DataRow dr in dtSet.Tables[4].Rows)
                {
                    WorkstationName _wslist = new WorkstationName();
                    _wslist.ws_id = dr["ws_id"].ToString();
                    _wslist.ws_name = dr["ws_name"].ToString();
                    ws.Add(_wslist);
                }
                _ProductionOrderModel.WorkstationNameList = ws;


                
                //Commented by Suraj on 30-08-2024
                //if (dtSet.Tables[3].Rows.Count > 0)
                //{
                //    List<OperationName> _op = new List<OperationName>();
                //    if (dtSet.Tables[3].Rows.Count > 0)
                //    {
                //        OperationName _opObj = new OperationName();
                //        _opObj.op_id = "0";
                //        _opObj.op_name = "---Select---";
                //        _op.Add(_opObj);
                //        foreach (DataRow dr in dtSet.Tables[3].Rows)
                //        {
                //            OperationName _oplist = new OperationName();
                //            _oplist.op_id = dr["op_id"].ToString();
                //            _oplist.op_name = dr["op_name"].ToString();
                //            _op.Add(_oplist);
                //        }
                //    }
                //    else
                //    {
                //        OperationName _opObj = new OperationName();
                //        _opObj.op_id = "0";
                //        _opObj.op_name = "---Select---";
                //        _op.Add(_opObj);
                //    }
                //    _ProductionOrderModel.OperationNameList = _op;
                //}
                /*commented by Hina on 06-04-2024 to combine all list Procedure  in single Procedure*/
                //GetStatus(_ProductionOrderModel);
                List<statusLists> statusLists = new List<statusLists>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    statusLists list = new statusLists();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _ProductionOrderModel.statusLists = statusLists;

                _ProductionOrderModel.product_id = "0";
                /*commented by Hina on 06-04-2024 to combine all list Procedure  in single Procedure*/
                //ViewBag.VBRoleList = GetRoleList();
                //DataSet dsOp = _ProductionOrder_ISERVICES.BindOperationNameInListPage(Convert.ToInt32(CompID));
                //if (dsOp.Tables.Count > 0)
                //{
                //    List<OperationName> _op = new List<OperationName>();
                //    if (dsOp.Tables.Count > 0)
                //    {
                //        OperationName _opObj = new OperationName();
                //        _opObj.op_id = "0";
                //        _opObj.op_name = "---Select---";
                //        _op.Add(_opObj);
                //        foreach (DataRow data in dsOp.Tables[0].Rows)
                //        {
                //            OperationName _oplist = new OperationName();
                //            _oplist.op_id = data["op_id"].ToString();
                //            _oplist.op_name = data["op_name"].ToString();
                //            _op.Add(_oplist);
                //        }
                //    }
                //    else
                //    {
                //        OperationName _opObj = new OperationName();
                //        _opObj.op_id = "0";
                //        _opObj.op_name = "---Select---";
                //        _op.Add(_opObj);
                //    }
                //    _ProductionOrderModel.OperationNameList = _op;
                //}

                var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, br_id, DocumentMenuId);

                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                //string endDate = dtnow.ToString("yyyy-MM-dd");
              
                if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                {
                    var PRData = TempData["ListFilterData"].ToString();
                    var a = PRData.Split(',');
                    var ddl_shfl_id = a[0].Trim();
                    var ddl_Product_id = a[1].Trim();
                    var ddl_op_id = a[2].Trim();
                    var txtFromdate = a[3].Trim();
                    var txtToDate = a[4].Trim();
                    var Status = a[5].Trim();
                    var command = a[6].Trim();
                    var ddl_shift_id = a[7].Trim();
                    var ddl_workst_id = a[8].Trim();
                    if (Status == "0")
                    {
                        Status = null;
                        _ProductionOrderModel.txtFromdate = txtFromdate;
                        _ProductionOrderModel.ListFilterData = TempData["ListFilterData"].ToString();
                    }

                    ViewBag.ListData = TempData["ListFilterData"].ToString();
                    //ViewBag.GetWSDetails = GetJCSearch(ddl_shfl_id, ddl_Product_id, ddl_op_id, txtFromdate, txtToDate, Status, command);
                    DataSet dt = _ProductionOrder_ISERVICES.GetJCSearch(CompID, br_id, ddl_shfl_id, ddl_Product_id, ddl_op_id, ddl_shift_id, ddl_workst_id, txtFromdate, txtToDate, Status);
                    ViewBag.GetJCList = dt;
                    _ProductionOrderModel.ddl_ShopfloorName = ddl_shfl_id;
                    _ProductionOrderModel.product_id = ddl_Product_id;
                    _ProductionOrderModel.ddl_OperationName = ddl_op_id;
                    _ProductionOrderModel.ddl_shift = ddl_shift_id;
                    _ProductionOrderModel.ddl_WorkstationName = ddl_workst_id;
                    _ProductionOrderModel.txtFromdate = txtFromdate;
                    _ProductionOrderModel.txtToDate = txtToDate;
                    _ProductionOrderModel.Status = Status;
                }
                else
                {
                    _ProductionOrderModel.txtFromdate = startDate;
                    _ProductionOrderModel.txtToDate = CurrentDate;
                }
                ViewBag.DocumentMenuId = DocumentMenuId;
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionOrder/ProductionOrderList.cshtml", _ProductionOrderModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet PrdOrdGetAllData(ProductionOrderModel _ProductionOrderModel)
        {
            string Comp_ID = string.Empty;
            if (Session["CompId"] != null)
            {
                Comp_ID = Session["CompId"].ToString();
            }
            //if (Session["CompId"] != null)
            //{
            string br_id = Session["BranchId"].ToString();
            string UserID = Session["UserID"].ToString();
            string wfstatus = "";
            //if (Session["WF_status"] != null)
            //{
            //    wfstatus = Session["WF_status"].ToString();
            //}
            if (_ProductionOrderModel.WF_Status != null)
            {
                wfstatus = _ProductionOrderModel.WF_Status;
            }
            else
            {
                wfstatus = "";
            }
            DataSet ds = _ProductionOrder_ISERVICES.GetAllData(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id), UserID, wfstatus, DocumentMenuId
                , _ProductionOrderModel.txtFromdate, _ProductionOrderModel.txtToDate);
            return ds;
        }
        //private DataTable GetRoleList()
        //{
        //    try
        //    {
        //        string UserID = string.Empty;
        //        string CompID = string.Empty;
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            UserID = Session["userid"].ToString();
        //        }
        //        DataTable RoleList = _Common_IServices.GetRole_List(CompID, UserID, DocumentMenuId);

        //        return RoleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
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
                    BranchId = Session["BranchId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(CompID, BranchId, UserID, DocumentMenuId, language);
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
        public void GetshiftList(ProductionOrderModel _ProductionOrderModel)
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
                List<shift> sh = new List<shift>();
                shift shObj = new shift();
                shObj.id = "0";
                shObj.name = "---Select---";
                sh.Add(shObj);
                shift shObj1 = new shift();
                shObj1.id = "1";
                shObj1.name = "Shift-1";
                sh.Add(shObj1);
                shift shObj2 = new shift();
                shObj2.id = "2";
                shObj2.name = "Shift-2";
                sh.Add(shObj2);
                shift shObj3 = new shift();
                shObj3.id = "3";
                shObj3.name = "Shift-3";
                sh.Add(shObj3);
                _ProductionOrderModel.shiftList = sh;

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        //public void GetStatus(ProductionOrderModel _ProductionOrderModel)
        //{
        //    try
        //    {
        //        List<statusLists> statusLists = new List<statusLists>();
        //        var other = new CommonController(_Common_IServices);
        //        var statusListsC = other.GetStatusList1(DocumentMenuId);
        //        var listOfStatus = statusListsC.ConvertAll(x => new statusLists { status_id = x.status_id, status_name = x.status_name });
        //        _ProductionOrderModel.statusLists = listOfStatus;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //    }
        //}
        [HttpPost]
        public ActionResult ProductionOrder(ProductionOrderModel _ProductionOrderModel, string command)//List Page Load
        {
            try
            {
                //ProductionOrderModel _ProductionOrderModel = new ProductionOrderModel();
                //Session.Remove("WF_status");
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _ProductionOrderModel.Title = title;
                string CompID = Session["CompId"].ToString();
                string br_id = Session["BranchId"].ToString();
                string ddl_shfl_id = _ProductionOrderModel.ddl_ShopfloorName;
                string ddl_Product_id = _ProductionOrderModel.product_id;
                string ddl_op_id = _ProductionOrderModel.ddl_OperationName;
                string ddl_shift_id = _ProductionOrderModel.ddl_shift;
                string ddl_workst_id = _ProductionOrderModel.ddl_WorkstationName;
                string txtFromdate = _ProductionOrderModel.txtFromdate;
                string txtToDate = _ProductionOrderModel.txtToDate;
                string Status = _ProductionOrderModel.Status;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                // Session["WF_status"] ="";
                _ProductionOrderModel.WF_Status = null;
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                /*commented by Hina on 06-04-2024 to combine all list Procedure  in single Procedure*/
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, br_id, DocumentMenuId);
                ViewBag.DocumentMenuId = DocumentMenuId;
                DataSet dt = _ProductionOrder_ISERVICES.GetJCSearch(CompID, br_id, ddl_shfl_id, ddl_Product_id, ddl_op_id, ddl_shift_id, ddl_workst_id, txtFromdate, txtToDate, Status);
                ViewBag.GetJCList = dt;
                List<ShopFloor> _ShopFloor = new List<ShopFloor>();
                ShopFloor _Shfllist = new ShopFloor();
                _Shfllist.shfl_id = "0";
                _Shfllist.shfl_name = "---Select---";
                _ShopFloor.Add(_Shfllist);
                foreach (DataRow dr in dt.Tables[1].Rows)
                {
                    ShopFloor _Statuslist1 = new ShopFloor();
                    _Statuslist1.shfl_id = dr["shfl_id"].ToString();
                    _Statuslist1.shfl_name = dr["shfl_name"].ToString();
                    _ShopFloor.Add(_Statuslist1);
                }
                _ProductionOrderModel.ShopFloorList = _ShopFloor;
                List<OperationName> _op = new List<OperationName>();
                OperationName _opObj = new OperationName();
                _opObj.op_id = "0";
                _opObj.op_name = "---Select---";
                _op.Add(_opObj);
                foreach (DataRow dr in dt.Tables[2].Rows)
                {
                    OperationName _oplist = new OperationName();
                    _oplist.op_id = dr["op_id"].ToString();
                    _oplist.op_name = dr["op_name"].ToString();
                    _op.Add(_oplist);
                }
                _ProductionOrderModel.OperationNameList = _op;
                GetshiftList(_ProductionOrderModel);/*Add by Hina Sharma on 07-03-2025 for filter*/

                List<WorkstationName> ws = new List<WorkstationName>();/*Add by Hina Sharma on 07-03-2025 for filter*/
                WorkstationName wsObj = new WorkstationName();
                wsObj.ws_id = "0";
                wsObj.ws_name = "---Select---";
                ws.Add(wsObj);
                foreach (DataRow dr in dt.Tables[3].Rows)
                {
                    WorkstationName _wslist = new WorkstationName();
                    _wslist.ws_id = dr["ws_id"].ToString();
                    _wslist.ws_name = dr["ws_name"].ToString();
                    ws.Add(_wslist);
                }
                _ProductionOrderModel.WorkstationNameList = ws;

                /*commented by Hina on 06-04-2024 to combine all list Procedure  in single Procedure*/
                //BindShopFloorList(_ProductionOrderModel);
                if (command == "Search")
                {
                    var ListfilterData = ddl_shfl_id + ',' + ddl_Product_id + ',' + ddl_op_id + ',' + txtFromdate + ',' + txtToDate + ',' + Status + ',' + command + ',' + ddl_shift_id + ',' + ddl_workst_id;
                    ViewBag.ListData = ListfilterData;
                }
                /*commented by Hina on 06-04-2024 to combine all list Procedure  in single Procedure*/
                //DataSet dsOp = _ProductionOrder_ISERVICES.BindOperationNameInListPage(Convert.ToInt32(CompID));

                //if (dsOp.Tables.Count > 0)
                //{
                //    List<OperationName> _op = new List<OperationName>();
                //    if (dsOp.Tables.Count > 0)
                //    {
                //        OperationName _opObj = new OperationName();
                //        _opObj.op_id = "0";
                //        _opObj.op_name = "---Select---";
                //        _op.Add(_opObj);
                //        foreach (DataRow data in dsOp.Tables[0].Rows)
                //        {
                //            OperationName _oplist = new OperationName();
                //            _oplist.op_id = data["op_id"].ToString();
                //            _oplist.op_name = data["op_name"].ToString();
                //            _op.Add(_oplist);
                //        }
                //    }
                //    else
                //    {
                //        OperationName _opObj = new OperationName();
                //        _opObj.op_id = "0";
                //        _opObj.op_name = "---Select---";
                //        _op.Add(_opObj);
                //    }
                //    _ProductionOrderModel.OperationNameList = _op;
                //}
                //GetStatus(_ProductionOrderModel);
                List<statusLists> statusLists = new List<statusLists>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    statusLists list = new statusLists();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _ProductionOrderModel.statusLists = statusLists;
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionOrder/ProductionOrderList.cshtml", _ProductionOrderModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult dbClickEdit(string jc_no, string jc_dt, string ListFilterData, string WF_Status)
        {
            try
            {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                //var commCont = new CommonController(_Common_IServices);
                //if (commCont.CheckFinancialYear(CompID, br_id) == "Not Exist")
                //{
                //    TempData["Message1"] = "Financial Year not Exist";
                //}
                /*End to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                {
                    //Session["Message"] = "";
                    //Session["Command"] = "View";
                    //Session["TransType"] = "EditNew";
                    //Session["BtnName"] = "BtnToDetailPage";
                    //Session["TransType"] = "Update";
                    //Session["jc_no"] = jc_no;
                    //_ProductionOrderModel.jc_no = jc_no;
                    ////DateTime dt3 = DateTime.Now;
                    ////DateTime dt3 = DateTime.ParseExact(jc_dt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    //_ProductionOrderModel.jc_dt = jc_dt;
                    //Session["jc_dt"] = jc_dt;
                    //Session["SaveUpd"] = "0";
                    //Session["dbclick"] = "dbclick";
                    ProductionOrderModel dblclick = new ProductionOrderModel();
                    UrlModel _url_model = new UrlModel();
                    dblclick.PO_No = jc_no;
                    dblclick.PO_dt = jc_dt;
                    dblclick.TransType = "Update";
                    dblclick.BtnName = "BtnToDetailPage";
                    if (WF_Status != null && WF_Status != "")
                    {
                        _url_model.wf = WF_Status;
                        dblclick.WF_Status1 = WF_Status;
                    }
                    TempData["ModelData"] = dblclick;
                    //_url_model.Cmd = "Update";
                    _url_model.tp = "Update";
                    _url_model.bt = "BtnToDetailPage";
                    _url_model.PO_No = jc_no;
                    _url_model.PO_dt = jc_dt;
                    TempData["ListFilterData"] = ListFilterData;
                    return RedirectToAction("AddProductionOrderDetail", _url_model);
                }
                else
                {
                    RedirectToAction("Home", "Index");
                }
                return RedirectToAction("AddProductionOrderDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        private DataSet GetJCList(ProductionOrderModel _ProductionOrderModel)
        {
            DataSet ds = new DataSet();
            try
            {
                string Comp_ID = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    string UserID = Session["UserID"].ToString();
                    string wfstatus = "";
                    //if (Session["WF_status"] != null)
                    //{
                    //    wfstatus = Session["WF_status"].ToString();
                    //}
                    if (_ProductionOrderModel.WF_Status != null)
                    {
                        wfstatus = _ProductionOrderModel.WF_Status;
                    }
                    else
                    {
                        wfstatus = "";
                    }

                    ds = _ProductionOrder_ISERVICES.GetJCList(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id), UserID, wfstatus, DocumentMenuId);
                    if (ds.Tables.Count > 0)
                    {
                        //_ProductionOrderModel.txtFromdate = Convert.ToString(ds.Tables[1].Rows[0]["finstrdate"]);
                        DateTime dt3 = DateTime.Now;
                        _ProductionOrderModel.txtToDate = dt3.ToString("yyyy-MM-dd");
                    }
                    DataSet dsOp = _ProductionOrder_ISERVICES.BindOperationNameInListPage(Convert.ToInt32(Comp_ID));
                    if (dsOp.Tables.Count > 0)
                    {
                        List<OperationName> _op = new List<OperationName>();
                        if (dsOp.Tables.Count > 0)
                        {
                            OperationName _opObj = new OperationName();
                            _opObj.op_id = "0";
                            _opObj.op_name = "---Select---";
                            _op.Add(_opObj);
                            foreach (DataRow data in dsOp.Tables[0].Rows)
                            {
                                OperationName _oplist = new OperationName();
                                _oplist.op_id = data["op_id"].ToString();
                                _oplist.op_name = data["op_name"].ToString();
                                _op.Add(_oplist);
                            }
                        }
                        else
                        {
                            OperationName _opObj = new OperationName();
                            _opObj.op_id = "0";
                            _opObj.op_name = "---Select---";
                            _op.Add(_opObj);
                        }
                        _ProductionOrderModel.OperationNameList = _op;
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
            return ds;
        }
        public ActionResult AddProductionOrderDetail(UrlModel _urlModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                /*Add by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                //var commCont = new CommonController(_Common_IServices);
                //if (commCont.CheckFinancialYearAndPreviousYear(CompID, br_id, _urlModel.PO_dt) == "TransNotAllow")
                //{
                //    //TempData["Message2"] = "TransNotAllow";
                //    ViewBag.Message = "TransNotAllow";
                //}
                /*----------Attachment Section Start----------*/
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
                /*----------Attachment Section End----------*/
                var _Pro_Oder_Model = TempData["ModelData"] as ProductionOrderModel;
                if (_Pro_Oder_Model != null)
                {
                    CommonPageDetails();
                    // ProductionOrderModel _Pro_Oder_Model = new ProductionOrderModel();
                    _Pro_Oder_Model.jc_no = "";
                    _Pro_Oder_Model.jc_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                    BindShopFloorList(_Pro_Oder_Model);
                    BindDDLOnPageLoad(_Pro_Oder_Model);
                    //ViewBag.MenuPageName = getDocumentName();
                    _Pro_Oder_Model.Title = title;
                    

                    var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, br_id, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _Pro_Oder_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    //if (Session["dbclick"] != null && Session["TransType"] != null && Session["jc_no"] != null)
                    if (_Pro_Oder_Model.TransType == "Update"/* && Session["TransType"] != null && Session["jc_no"] != null*/)
                    {
                        //_Pro_Oder_Model.jc_no = Session["jc_no"].ToString();
                        //_Pro_Oder_Model.jc_no = _Pro_Oder_Model.PO_No;
                        //_Pro_Oder_Model.jc_dt = _Pro_Oder_Model.PO_dt;
                        var jc_dt = "";
                        var jc_no = _Pro_Oder_Model.PO_No;
                        if (_Pro_Oder_Model.PO_dt != null && _Pro_Oder_Model.PO_dt != "")
                        {
                            jc_dt = _Pro_Oder_Model.PO_dt;
                        }
                        else
                        {
                            jc_dt = _Pro_Oder_Model.jc_dt;
                        }
                        DataSet ds = new DataSet();
                        string UserID = Session["UserID"].ToString();
                        //ds = _ProductionOrder_ISERVICES.BinddbClick(Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BranchId"].ToString()), Session["jc_no"].ToString(), Session["jc_dt"].ToString(), UserID, DocumentMenuId);
                        ds = _ProductionOrder_ISERVICES.BinddbClick(Convert.ToInt32(CompID), Convert.ToInt32(br_id), jc_no, jc_dt, UserID, DocumentMenuId);
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                DataSet dsEdit = new DataSet();
                                string itemid = Convert.ToString(ds.Tables[0].Rows[0]["product_id"]);
                                //dsEdit = _ProductionOrder_ISERVICES.BindRevnoEdit(Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BranchId"].ToString()), itemid);
                                dsEdit = _ProductionOrder_ISERVICES.BindRevnoEdit(Convert.ToInt32(CompID), Convert.ToInt32(br_id), itemid);
                                //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                _Pro_Oder_Model.DocumentStatus = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                string jcstatus = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                //Session["jc_status"] = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                _Pro_Oder_Model.jc_status = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                if (jcstatus == "D")
                                {
                                    //Session["jc_status"] = jcstatus;
                                    _Pro_Oder_Model.jc_status = jcstatus;
                                }
                                else if (jcstatus == "C")
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                    _Pro_Oder_Model.CancelFlag = true;
                                }
                                else if (jcstatus == "FC")
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                    _Pro_Oder_Model.ForceClose = true;
                                }
                                else if (jcstatus == "A")
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                    _Pro_Oder_Model.CancelFlag = false;
                                }
                                else
                                {
                                    _Pro_Oder_Model.BtnName = "BtnRefresh";
                                }
                                _Pro_Oder_Model.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                                _Pro_Oder_Model.jc_no = Convert.ToString(ds.Tables[0].Rows[0]["jc_no"]);
                                _Pro_Oder_Model.jc_dt = Convert.ToString(ds.Tables[0].Rows[0]["jc_dt"]);
                                _Pro_Oder_Model.PrducOrderType = ds.Tables[0].Rows[0]["ord_type"].ToString();
                                _Pro_Oder_Model.SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();
                                _Pro_Oder_Model.product_id = Convert.ToString(ds.Tables[0].Rows[0]["product_id"]);
                                _Pro_Oder_Model.op_output_itemid = Convert.ToString(ds.Tables[0].Rows[0]["op_out_item_id"]);
                                _Pro_Oder_Model.op_output_item_name = Convert.ToString(ds.Tables[0].Rows[0]["op_output_item_name"]);
                                _Pro_Oder_Model.op_output_uom_id = Convert.ToString(ds.Tables[0].Rows[0]["op_out_uom_id"]);
                                _Pro_Oder_Model.op_output_uom_Name = Convert.ToString(ds.Tables[0].Rows[0]["op_output_uom_name"]);
                                _Pro_Oder_Model.product_name = Convert.ToString(ds.Tables[0].Rows[0]["item_name"]);
                                _Pro_Oder_Model.uom_id = Convert.ToString(ds.Tables[0].Rows[0]["uom_id"]);
                                _Pro_Oder_Model.uom_Name = Convert.ToString(ds.Tables[0].Rows[0]["uom_alias"]);
                                _Pro_Oder_Model.uom_id = Convert.ToString(ds.Tables[0].Rows[0]["uom_id"]);
                                _Pro_Oder_Model.uom_Name = Convert.ToString(ds.Tables[0].Rows[0]["uom_alias"]);
                                _Pro_Oder_Model.jc_qty = Convert.ToInt32(ds.Tables[0].Rows[0]["jc_qty"]);
                                _Pro_Oder_Model.batch_no = Convert.ToString(ds.Tables[0].Rows[0]["batch_no"]);
                                _Pro_Oder_Model.advicedt = Convert.ToString(ds.Tables[0].Rows[0]["adv_dt"]);
                                _Pro_Oder_Model.adviceno = Convert.ToString(ds.Tables[0].Rows[0]["adv_no"]);
                                // _Pro_Oder_Model.hdnadviceno = ds.Tables[0].Rows[0]["adv_no"].ToString();

                                List<AdviceNumber> _AdviceNumberList = new List<AdviceNumber>();
                                AdviceNumber _AdviceNumber = new AdviceNumber();
                                _AdviceNumber.advice_no = ds.Tables[0].Rows[0]["adv_no"].ToString();
                                _AdviceNumber.advice_dt = ds.Tables[0].Rows[0]["adv_no"].ToString();
                                _AdviceNumberList.Add(_AdviceNumber);
                                _Pro_Oder_Model.AdviceNumberList = _AdviceNumberList;

                                List<RevisionNumber> _rev = new List<RevisionNumber>();
                                if (dsEdit.Tables.Count > 0)
                                {
                                    foreach (DataRow data in dsEdit.Tables[0].Rows)
                                    {
                                        RevisionNumber _Statuslist = new RevisionNumber();
                                        _Statuslist.rev_no = data["rev_no"].ToString();
                                        _Statuslist.rev_text = data["rev_no"].ToString();
                                        _rev.Add(_Statuslist);
                                    }
                                }
                                _Pro_Oder_Model.RevisionNumberList = _rev;

                                List<OperationName> op = new List<OperationName>();
                                OperationName opObj = new OperationName();
                                opObj.op_id = ds.Tables[0].Rows[0]["op_id"].ToString();
                                opObj.op_name = ds.Tables[0].Rows[0]["op_name"].ToString();
                                op.Add(opObj);
                                _Pro_Oder_Model.OperationNameList = op;

                                //List<OperationName> _op = new List<OperationName>();
                                //if (dsEdit.Tables.Count > 0)
                                //{
                                //    foreach (DataRow data in dsEdit.Tables[1].Rows)
                                //    {
                                //        OperationName _oplist = new OperationName();
                                //        _oplist.op_id = data["op_id"].ToString();
                                //        _oplist.op_name = data["op_name"].ToString();
                                //        _op.Add(_oplist);
                                //    }
                                //}
                                //_Pro_Oder_Model.OperationNameList = _op;

                                //_Pro_Oder_Model.ddl_RevisionNumber = Convert.ToString(ds.Tables[0].Rows[0]["rev_no"]);
                                _Pro_Oder_Model.jc_qty = Convert.ToInt32(ds.Tables[0].Rows[0]["jc_qty"]);
                                _Pro_Oder_Model.ddl_OperationName = Convert.ToString(ds.Tables[0].Rows[0]["op_id"]);
                                _Pro_Oder_Model.OP_Name = Convert.ToString(ds.Tables[0].Rows[0]["op_name"]);
                                _Pro_Oder_Model.ddl_op_id = Convert.ToString(ds.Tables[0].Rows[0]["op_id"]);
                                _Pro_Oder_Model.ddl_shift = Convert.ToString(ds.Tables[0].Rows[0]["shift_id"]);
                                //_Pro_Oder_Model.ddl_shiftName = Convert.ToString(ds.Tables[0].Rows[0]["op_id"]);
                                if (_Pro_Oder_Model.PrducOrderType == "IH")
                                {
                                    _Pro_Oder_Model.ddl_ShopfloorName = Convert.ToString(ds.Tables[0].Rows[0]["shfl_id"]);
                                    _Pro_Oder_Model.ddl_WorkstationName = Convert.ToString(ds.Tables[0].Rows[0]["ws_id"]);
                                    _Pro_Oder_Model.ddl_WorkstationText = Convert.ToString(ds.Tables[0].Rows[0]["ws_name"]);
                                    _Pro_Oder_Model.supervisor_name = Convert.ToString(ds.Tables[0].Rows[0]["supervisor_name"]);
                                    _Pro_Oder_Model.jc_st_date = Convert.ToString(ds.Tables[0].Rows[0]["jc_st_date"]);
                                    _Pro_Oder_Model.jc_en_date = Convert.ToString(ds.Tables[0].Rows[0]["jc_en_date"]);
                                    _Pro_Oder_Model.ddl_shift = Convert.ToString(ds.Tables[0].Rows[0]["shift_id"]);
                                }
                                _Pro_Oder_Model.batch_no = Convert.ToString(ds.Tables[0].Rows[0]["batch_no"]);
                                _Pro_Oder_Model.CreatedBy = Convert.ToString(ds.Tables[0].Rows[0]["created_id"]);
                                _Pro_Oder_Model.CreatedOn = Convert.ToString(ds.Tables[0].Rows[0]["create_dt"]);
                                _Pro_Oder_Model.ApprovedBy = Convert.ToString(ds.Tables[0].Rows[0]["app_id"]);
                                _Pro_Oder_Model.ApprovedOn = Convert.ToString(ds.Tables[0].Rows[0]["app_dt"]);
                                _Pro_Oder_Model.AmmendedBy = Convert.ToString(ds.Tables[0].Rows[0]["mod_id"]);
                                _Pro_Oder_Model.AmmendedOn = Convert.ToString(ds.Tables[0].Rows[0]["mod_dt"]);
                                _Pro_Oder_Model.Status = Convert.ToString(ds.Tables[0].Rows[0]["status_name"]);
                                _Pro_Oder_Model.jc_status = Convert.ToString(ds.Tables[0].Rows[0]["jc_status"]).Trim();

                                if (ds.Tables[6].Rows.Count > 0)
                                {
                                    _Pro_Oder_Model.ReceiptNumber = ds.Tables[6].Rows[0]["cnf_no"].ToString();
                                    _Pro_Oder_Model.ReceiptDate = ds.Tables[6].Rows[0]["cnf_dt"].ToString();
                                    _Pro_Oder_Model.OrderedQuantity = ds.Tables[6].Rows[0]["jc_qty"].ToString();
                                    _Pro_Oder_Model.ActualStartDateAndTime = ds.Tables[6].Rows[0]["jc_st_date"].ToString();
                                    _Pro_Oder_Model.ActualEndDateAndTime = ds.Tables[6].Rows[0]["jc_en_date"].ToString();
                                    _Pro_Oder_Model.ProductionHours = ds.Tables[6].Rows[0]["hrs"].ToString();
                                }

                                List<JC_Item_details> ArrItem = new List<JC_Item_details>();

                                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                {
                                    JC_Item_details jid = new JC_Item_details();
                                    jid.item_id = Convert.ToString(ds.Tables[1].Rows[i]["item_id"]);
                                    jid.item_name = Convert.ToString(ds.Tables[1].Rows[i]["item_name"]);
                                    jid.uom_id = Convert.ToString(ds.Tables[1].Rows[i]["uom_id"]);
                                    jid.uom_name = Convert.ToString(ds.Tables[1].Rows[i]["uom_alias"]);
                                    jid.Item_type_id = Convert.ToString(ds.Tables[1].Rows[i]["item_type_id"]);
                                    jid.Item_type_name = Convert.ToString(ds.Tables[1].Rows[i]["Item_type"]);
                                    jid.req_qty = Convert.ToString(ds.Tables[1].Rows[i]["req_qty"]);
                                    jid.sub_item = Convert.ToString(ds.Tables[1].Rows[i]["sub_item"]);
                                    jid.avl_stock_shfl = Convert.ToString(ds.Tables[1].Rows[i]["avl_stock_shfl"]);
                                    jid.avl_stock_warehouse = Convert.ToString(ds.Tables[1].Rows[i]["avl_stock_warehouse"]);
                                    jid.seq_no = Convert.ToString(ds.Tables[1].Rows[i]["seq_no"]);
                                    jid.alt_fill = Convert.ToString(ds.Tables[1].Rows[i]["alt_fill"]);
                                    ArrItem.Add(jid);
                                }

                                ViewBag.OPItemList = ds.Tables[2];
                                ViewBag.QtyDigit = other.ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                                ViewBag.ConsumptionItmDetails = ds.Tables[7];

                                ViewBag.OutputItmDetails = ds.Tables[8];

                                ViewBag.AttechmentDetails = ds.Tables[9];

                                _Pro_Oder_Model.JC_Item_Details_List = ArrItem;

                                //------------------------------ Work Flow Part -----------------------------------//

                                string Statuscode = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                _Pro_Oder_Model.hdnautomrs = ds.Tables[0].Rows[0]["auto_mrs"].ToString().Trim();
                                if(_Pro_Oder_Model.hdnautomrs=="Y")
                                {
                                    _Pro_Oder_Model.AutoMRS = true;
                                }
                                else
                                {
                                    _Pro_Oder_Model.AutoMRS = false;
                                }
                                string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                                UserID = Session["UserID"].ToString();
                                string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                                _Pro_Oder_Model.create_id = create_id;
                                _Pro_Oder_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                                _Pro_Oder_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                                if (Statuscode != "D" && Statuscode != "F")
                                {
                                    ViewBag.AppLevel = ds.Tables[5];
                                }
                                //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                                if (ViewBag.AppLevel != null && _Pro_Oder_Model.Command != "Edit")
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
                                            if (nextLevel == "0")
                                            {
                                                //if (create_id == UserID)
                                                //{
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                                // }
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                            }
                                            else
                                            {
                                                ViewBag.Approve = "N";
                                                ViewBag.ForwardEnbl = "Y";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                            }

                                            //Session["BtnName"] = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            if (nextLevel == "0")
                                            {
                                                if (create_id == UserID)
                                                {
                                                    ViewBag.Approve = "Y";
                                                    ViewBag.ForwardEnbl = "N";
                                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                    if (TempData["Message1"] != null)
                                                    {
                                                        ViewBag.Message = TempData["Message1"];
                                                    }
                                                    /*End to chk Financial year exist or not*/
                                                }
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                            }
                                            else
                                            {
                                                ViewBag.Approve = "N";
                                                ViewBag.ForwardEnbl = "Y";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                            }

                                        }
                                        if (UserID == sent_to)
                                        {
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                        }


                                        if (nextLevel == "0")
                                        {
                                            if (sent_to == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                            }


                                        }
                                    }
                                    else if (Statuscode == "F")
                                    {
                                        if (UserID == sent_to)
                                        {
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                        }
                                        if (nextLevel == "0")
                                        {
                                            if (sent_to == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _Pro_Oder_Model.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    else if (Statuscode == "A")
                                    {
                                        if (create_id == UserID || approval_id == UserID)
                                        {
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _Pro_Oder_Model.BtnName = "BtnToDetailPage";

                                        }
                                        else
                                        {
                                            //Session["BtnName"] = "BtnRefresh";
                                            _Pro_Oder_Model.BtnName = "BtnRefresh";
                                        }
                                    }
                                    else
                                    {
                                        _Pro_Oder_Model.BtnName = "BtnRefresh";
                                    }
                                }
                                if (ViewBag.AppLevel.Rows.Count == 0)
                                {
                                    ViewBag.Approve = "Y";
                                }
                                //------------------------------ Work Flow Part End-----------------------------------//

                                //Session.Remove("dbclick");
                            }
                        }
                        ViewBag.SubItemDetails = ds.Tables[10];
                        ViewBag.ProductionScheduleDetails = ds.Tables[11];
                        if (ds.Tables[12].Rows.Count > 0)
                        {
                            _Pro_Oder_Model.MRSNumber = ds.Tables[12].Rows[0]["mrs_no"].ToString();
                            _Pro_Oder_Model.MRSDate = ds.Tables[12].Rows[0]["mrs_dt"].ToString();
                        }
                            
                        ViewBag.HdnMrsDeatilData = ds.Tables[12];
                    }
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionOrder/ProductionOrderDetail.cshtml", _Pro_Oder_Model);
                }
                else
                {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        br_id = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, br_id) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    ProductionOrderModel _ProductionOrderModel = new ProductionOrderModel();
                    if (_urlModel != null)
                    {
                        _ProductionOrderModel.BtnName = _urlModel.bt;
                        _ProductionOrderModel.PO_No = _urlModel.PO_No;
                        _ProductionOrderModel.PO_dt = _urlModel.PO_dt;
                        _ProductionOrderModel.Command = _urlModel.Cmd;
                        _ProductionOrderModel.TransType = _urlModel.tp;
                        _ProductionOrderModel.WF_Status1 = _urlModel.wf;
                        _ProductionOrderModel.DocumentStatus = _urlModel.DMS;//Document Status
                    }
                    CommonPageDetails();
                    _ProductionOrderModel.jc_no = "";
                    _ProductionOrderModel.jc_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                    BindShopFloorList(_ProductionOrderModel);
                    BindDDLOnPageLoad(_ProductionOrderModel);
                    //ViewBag.MenuPageName = getDocumentName();
                    _ProductionOrderModel.Title = title;
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }

                    if (Session["BranchId"] != null)
                    {
                        br_id = Session["BranchId"].ToString();
                    }

                    var other = new CommonController(_Common_IServices);
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, br_id, DocumentMenuId);
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ProductionOrderModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    //if (Session["dbclick"] != null && Session["TransType"] != null && Session["jc_no"] != null)
                    if (_ProductionOrderModel.TransType == "Update"/* && Session["TransType"] != null && Session["jc_no"] != null*/)
                    {
                        //_ProductionOrderModel.jc_no = Session["jc_no"].ToString();
                        //_ProductionOrderModel.jc_no = _ProductionOrderModel.PO_No;
                        //_ProductionOrderModel.jc_dt = _ProductionOrderModel.PO_dt;
                        var jc_dt = "";
                        var jc_no = _ProductionOrderModel.PO_No;
                        if (_ProductionOrderModel.PO_dt != null && _ProductionOrderModel.PO_dt != "")
                        {
                            jc_dt = _ProductionOrderModel.PO_dt;
                        }
                        else
                        {
                            jc_dt = _ProductionOrderModel.jc_dt;
                        }
                        DataSet ds = new DataSet();
                        string UserID = Session["UserID"].ToString();
                        //ds = _ProductionOrder_ISERVICES.BinddbClick(Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BranchId"].ToString()), Session["jc_no"].ToString(), Session["jc_dt"].ToString(), UserID, DocumentMenuId);
                        ds = _ProductionOrder_ISERVICES.BinddbClick(Convert.ToInt32(CompID), Convert.ToInt32(br_id), jc_no, jc_dt, UserID, DocumentMenuId);
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                DataSet dsEdit = new DataSet();
                                string itemid = Convert.ToString(ds.Tables[0].Rows[0]["product_id"]);
                                //dsEdit = _ProductionOrder_ISERVICES.BindRevnoEdit(Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BranchId"].ToString()), itemid);
                                dsEdit = _ProductionOrder_ISERVICES.BindRevnoEdit(Convert.ToInt32(CompID), Convert.ToInt32(br_id), itemid);
                                //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                _ProductionOrderModel.DocumentStatus = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                string jcstatus = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                //Session["jc_status"] = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                _ProductionOrderModel.jc_status = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                if (jcstatus == "D")
                                {
                                    //Session["jc_status"] = jcstatus;
                                    _ProductionOrderModel.jc_status = jcstatus;
                                }
                                else if (jcstatus == "C")
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                    _ProductionOrderModel.CancelFlag = true;
                                }
                                else if (jcstatus == "FC")
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                    _ProductionOrderModel.ForceClose = true;
                                }
                                else if (jcstatus == "A")
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                    _ProductionOrderModel.CancelFlag = false;
                                }
                                else
                                {
                                    _ProductionOrderModel.BtnName = "BtnRefresh";
                                }
                                _ProductionOrderModel.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                                _ProductionOrderModel.jc_no = Convert.ToString(ds.Tables[0].Rows[0]["jc_no"]);
                                _ProductionOrderModel.jc_dt = Convert.ToString(ds.Tables[0].Rows[0]["jc_dt"]);
                                _ProductionOrderModel.PrducOrderType = ds.Tables[0].Rows[0]["ord_type"].ToString();
                                _ProductionOrderModel.SourceType = ds.Tables[0].Rows[0]["src_type"].ToString();
                                _ProductionOrderModel.product_id = Convert.ToString(ds.Tables[0].Rows[0]["product_id"]);
                                _ProductionOrderModel.op_output_itemid = Convert.ToString(ds.Tables[0].Rows[0]["op_out_item_id"]);
                                _ProductionOrderModel.op_output_item_name = Convert.ToString(ds.Tables[0].Rows[0]["op_output_item_name"]);
                                _ProductionOrderModel.op_output_uom_id = Convert.ToString(ds.Tables[0].Rows[0]["op_out_uom_id"]);
                                _ProductionOrderModel.op_output_uom_Name = Convert.ToString(ds.Tables[0].Rows[0]["op_output_uom_name"]);
                                _ProductionOrderModel.product_name = Convert.ToString(ds.Tables[0].Rows[0]["item_name"]);
                                _ProductionOrderModel.uom_id = Convert.ToString(ds.Tables[0].Rows[0]["uom_id"]);
                                _ProductionOrderModel.uom_Name = Convert.ToString(ds.Tables[0].Rows[0]["uom_alias"]);
                                _ProductionOrderModel.uom_id = Convert.ToString(ds.Tables[0].Rows[0]["uom_id"]);
                                _ProductionOrderModel.uom_Name = Convert.ToString(ds.Tables[0].Rows[0]["uom_alias"]);
                                _ProductionOrderModel.jc_qty = Convert.ToInt32(ds.Tables[0].Rows[0]["jc_qty"]);
                                _ProductionOrderModel.batch_no = Convert.ToString(ds.Tables[0].Rows[0]["batch_no"]);
                                _ProductionOrderModel.advicedt = Convert.ToString(ds.Tables[0].Rows[0]["adv_dt"]);
                                _ProductionOrderModel.adviceno = Convert.ToString(ds.Tables[0].Rows[0]["adv_no"]);
                                //_ProductionOrderModel.hdnadviceno = ds.Tables[0].Rows[0]["adv_no"].ToString();
                                List<AdviceNumber> _AdviceNumberList = new List<AdviceNumber>();
                                AdviceNumber _AdviceNumber = new AdviceNumber();
                                _AdviceNumber.advice_no = ds.Tables[0].Rows[0]["adv_no"].ToString();
                                _AdviceNumber.advice_dt = ds.Tables[0].Rows[0]["adv_no"].ToString();
                                _AdviceNumberList.Add(_AdviceNumber);
                                _ProductionOrderModel.AdviceNumberList = _AdviceNumberList;

                                List<RevisionNumber> _rev = new List<RevisionNumber>();
                                if (dsEdit.Tables.Count > 0)
                                {
                                    foreach (DataRow data in dsEdit.Tables[0].Rows)
                                    {
                                        RevisionNumber _Statuslist = new RevisionNumber();
                                        _Statuslist.rev_no = data["rev_no"].ToString();
                                        _Statuslist.rev_text = data["rev_no"].ToString();
                                        _rev.Add(_Statuslist);
                                    }
                                }
                                _ProductionOrderModel.RevisionNumberList = _rev;

                                List<OperationName> op = new List<OperationName>();
                                OperationName opObj = new OperationName();
                                opObj.op_id = ds.Tables[0].Rows[0]["op_id"].ToString();
                                opObj.op_name = ds.Tables[0].Rows[0]["op_name"].ToString();
                                op.Add(opObj);
                                _ProductionOrderModel.OperationNameList = op;

                                //List<OperationName> _op = new List<OperationName>();
                                //if (dsEdit.Tables.Count > 0)
                                //{
                                //    foreach (DataRow data in dsEdit.Tables[1].Rows)
                                //    {
                                //        OperationName _oplist = new OperationName();
                                //        _oplist.op_id = data["op_id"].ToString();
                                //        _oplist.op_name = data["op_name"].ToString();
                                //        _op.Add(_oplist);
                                //    }
                                //}
                                //_ProductionOrderModel.OperationNameList = _op;

                                //_ProductionOrderModel.ddl_RevisionNumber = Convert.ToString(ds.Tables[0].Rows[0]["rev_no"]);
                                _ProductionOrderModel.jc_qty = Convert.ToInt32(ds.Tables[0].Rows[0]["jc_qty"]);
                                _ProductionOrderModel.ddl_OperationName = Convert.ToString(ds.Tables[0].Rows[0]["op_id"]);
                                _ProductionOrderModel.OP_Name = Convert.ToString(ds.Tables[0].Rows[0]["op_name"]);
                                _ProductionOrderModel.ddl_op_id = Convert.ToString(ds.Tables[0].Rows[0]["op_id"]);
                                if (_ProductionOrderModel.PrducOrderType == "IH")
                                {
                                    _ProductionOrderModel.ddl_ShopfloorName = Convert.ToString(ds.Tables[0].Rows[0]["shfl_id"]);
                                    _ProductionOrderModel.ddl_WorkstationName = Convert.ToString(ds.Tables[0].Rows[0]["ws_id"]);
                                    _ProductionOrderModel.ddl_WorkstationText = Convert.ToString(ds.Tables[0].Rows[0]["ws_name"]);
                                    _ProductionOrderModel.supervisor_name = Convert.ToString(ds.Tables[0].Rows[0]["supervisor_name"]);
                                    _ProductionOrderModel.jc_st_date = Convert.ToString(ds.Tables[0].Rows[0]["jc_st_date"]);
                                    _ProductionOrderModel.jc_en_date = Convert.ToString(ds.Tables[0].Rows[0]["jc_en_date"]);
                                    _ProductionOrderModel.ddl_shift = Convert.ToString(ds.Tables[0].Rows[0]["shift_id"]);
                                }
                                _ProductionOrderModel.batch_no = Convert.ToString(ds.Tables[0].Rows[0]["batch_no"]);
                                _ProductionOrderModel.CreatedBy = Convert.ToString(ds.Tables[0].Rows[0]["created_id"]);
                                _ProductionOrderModel.CreatedOn = Convert.ToString(ds.Tables[0].Rows[0]["create_dt"]);
                                _ProductionOrderModel.ApprovedBy = Convert.ToString(ds.Tables[0].Rows[0]["app_id"]);
                                _ProductionOrderModel.ApprovedOn = Convert.ToString(ds.Tables[0].Rows[0]["app_dt"]);
                                _ProductionOrderModel.AmmendedBy = Convert.ToString(ds.Tables[0].Rows[0]["mod_id"]);
                                _ProductionOrderModel.AmmendedOn = Convert.ToString(ds.Tables[0].Rows[0]["mod_dt"]);
                                _ProductionOrderModel.Status = Convert.ToString(ds.Tables[0].Rows[0]["status_name"]);
                                _ProductionOrderModel.jc_status = Convert.ToString(ds.Tables[0].Rows[0]["jc_status"]).Trim();

                                if (ds.Tables[6].Rows.Count > 0)
                                {
                                    _ProductionOrderModel.ReceiptNumber = ds.Tables[6].Rows[0]["cnf_no"].ToString();
                                    _ProductionOrderModel.ReceiptDate = ds.Tables[6].Rows[0]["cnf_dt"].ToString();
                                    _ProductionOrderModel.OrderedQuantity = ds.Tables[6].Rows[0]["jc_qty"].ToString();
                                    _ProductionOrderModel.ActualStartDateAndTime = ds.Tables[6].Rows[0]["jc_st_date"].ToString();
                                    _ProductionOrderModel.ActualEndDateAndTime = ds.Tables[6].Rows[0]["jc_en_date"].ToString();
                                    _ProductionOrderModel.ProductionHours = ds.Tables[6].Rows[0]["hrs"].ToString();
                                }

                                List<JC_Item_details> ArrItem = new List<JC_Item_details>();

                                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                {
                                    JC_Item_details jid = new JC_Item_details();
                                    jid.item_id = Convert.ToString(ds.Tables[1].Rows[i]["item_id"]);
                                    jid.item_name = Convert.ToString(ds.Tables[1].Rows[i]["item_name"]);
                                    jid.uom_id = Convert.ToString(ds.Tables[1].Rows[i]["uom_id"]);
                                    jid.uom_name = Convert.ToString(ds.Tables[1].Rows[i]["uom_alias"]);
                                    jid.Item_type_id = Convert.ToString(ds.Tables[1].Rows[i]["item_type_id"]);
                                    jid.Item_type_name = Convert.ToString(ds.Tables[1].Rows[i]["Item_type"]);
                                    jid.req_qty = Convert.ToString(ds.Tables[1].Rows[i]["req_qty"]);
                                    jid.sub_item = Convert.ToString(ds.Tables[1].Rows[i]["sub_item"]);
                                    jid.avl_stock_shfl = Convert.ToString(ds.Tables[1].Rows[i]["avl_stock_shfl"]);
                                    jid.avl_stock_warehouse = Convert.ToString(ds.Tables[1].Rows[i]["avl_stock_warehouse"]);
                                    jid.seq_no = Convert.ToString(ds.Tables[1].Rows[i]["seq_no"]);
                                    jid.alt_fill = Convert.ToString(ds.Tables[1].Rows[i]["alt_fill"]);
                                    ArrItem.Add(jid);
                                }

                                ViewBag.OPItemList = ds.Tables[2];
                                ViewBag.QtyDigit = other.ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                                ViewBag.ConsumptionItmDetails = ds.Tables[7];

                                ViewBag.OutputItmDetails = ds.Tables[8];

                                ViewBag.AttechmentDetails = ds.Tables[9];

                                _ProductionOrderModel.JC_Item_Details_List = ArrItem;

                                //------------------------------ Work Flow Part -----------------------------------//
                                _ProductionOrderModel.hdnautomrs = ds.Tables[0].Rows[0]["auto_mrs"].ToString().Trim();
                                if (_ProductionOrderModel.hdnautomrs == "Y")
                                {
                                    _ProductionOrderModel.AutoMRS = true;
                                }
                                else
                                {
                                    _ProductionOrderModel.AutoMRS = false;
                                }
                                string Statuscode = ds.Tables[0].Rows[0]["jc_status"].ToString().Trim();
                                string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                                UserID = Session["UserID"].ToString();
                                string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                                _ProductionOrderModel.create_id = create_id;
                                _ProductionOrderModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                                _ProductionOrderModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);

                                if (Statuscode != "D" && Statuscode != "F")
                                {
                                    ViewBag.AppLevel = ds.Tables[5];
                                }
                                //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                                if (ViewBag.AppLevel != null && _ProductionOrderModel.Command != "Edit")
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
                                            if (nextLevel == "0")
                                            {
                                                //if (create_id == UserID)
                                                //{
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                                // }
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                            }
                                            else
                                            {
                                                ViewBag.Approve = "N";
                                                ViewBag.ForwardEnbl = "Y";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                            }

                                            //Session["BtnName"] = "BtnToDetailPage";
                                        }
                                        else
                                        {
                                            if (nextLevel == "0")
                                            {
                                                if (create_id == UserID)
                                                {
                                                    ViewBag.Approve = "Y";
                                                    ViewBag.ForwardEnbl = "N";
                                                    /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                    if (TempData["Message1"] != null)
                                                    {
                                                        ViewBag.Message = TempData["Message1"];
                                                    }
                                                    /*End to chk Financial year exist or not*/
                                                }
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                            }
                                            else
                                            {
                                                ViewBag.Approve = "N";
                                                ViewBag.ForwardEnbl = "Y";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                            }

                                        }
                                        if (UserID == sent_to)
                                        {
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                        }


                                        if (nextLevel == "0")
                                        {
                                            if (sent_to == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                                //Session["BtnName"] = "BtnToDetailPage";
                                                _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                            }


                                        }
                                    }
                                    else if (Statuscode == "F")
                                    {
                                        if (UserID == sent_to)
                                        {
                                            ViewBag.ForwardEnbl = "Y";
                                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                        }
                                        if (nextLevel == "0")
                                        {
                                            if (sent_to == UserID)
                                            {
                                                ViewBag.Approve = "Y";
                                                ViewBag.ForwardEnbl = "N";
                                                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                                                if (TempData["Message1"] != null)
                                                {
                                                    ViewBag.Message = TempData["Message1"];
                                                }
                                                /*End to chk Financial year exist or not*/
                                            }
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                        }
                                    }
                                    else if (Statuscode == "A")
                                    {
                                        if (create_id == UserID || approval_id == UserID)
                                        {
                                            //Session["BtnName"] = "BtnToDetailPage";
                                            _ProductionOrderModel.BtnName = "BtnToDetailPage";

                                        }
                                        else
                                        {
                                            //Session["BtnName"] = "BtnRefresh";
                                            _ProductionOrderModel.BtnName = "BtnRefresh";
                                        }
                                    }
                                    else
                                    {
                                        _ProductionOrderModel.BtnName = "BtnRefresh";
                                    }
                                }
                                if (ViewBag.AppLevel.Rows.Count == 0)
                                {
                                    ViewBag.Approve = "Y";
                                }
                                //------------------------------ Work Flow Part End-----------------------------------//

                                //Session.Remove("dbclick");
                            }
                        }
                        ViewBag.SubItemDetails = ds.Tables[10];
                        ViewBag.ProductionScheduleDetails = ds.Tables[11];
                        if (ds.Tables[12].Rows.Count > 0)
                        {
                            _ProductionOrderModel.MRSNumber = ds.Tables[12].Rows[0]["mrs_no"].ToString();
                            _ProductionOrderModel.MRSDate = ds.Tables[12].Rows[0]["mrs_dt"].ToString();
                        }
                       
                        ViewBag.HdnMrsDeatilData = ds.Tables[12];
                    }
                    //ViewBag.VBRoleList = GetRoleList();
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionOrder/ProductionOrderDetail.cshtml", _ProductionOrderModel);
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AddNewProductionOrderDetail()//Detail Page Load
        {
            try
            {
                //_ProductionOrderModel.jc_no = "";
                //_ProductionOrderModel.jc_dt = System.DateTime.Now.ToString("dd-MM-yyyy");
                //_ProductionOrderModel.jc_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                //BindShopFloorList(_ProductionOrderModel);
                //BindDDLOnPageLoad(_ProductionOrderModel);
                //Session.Remove("TransType");
                //Session["Message"] = "New";
                //Session["Command"] = "Add";
                //Session["TransType"] = "Save";
                //Session["BtnName"] = "BtnAddNew";
                //Session["DocumentStatus"] = "";
                //ViewBag.MenuPageName = getDocumentName();
                //_ProductionOrderModel.Title = title;
                //ViewBag.VBRoleList = GetRoleList();
                //if (Session["CompId"] != null)
                //{
                //    CompID = Session["CompId"].ToString();
                //}
                //if (Session["BranchId"] != null)
                //{
                //    br_id = Session["BranchId"].ToString();
                //}
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, br_id, DocumentMenuId);
                //ViewBag.DocumentMenuId = DocumentMenuId;
                ProductionOrderModel AddNewModel = new ProductionOrderModel();
                AddNewModel.Command = "Add";
                AddNewModel.TransType = "Save";
                AddNewModel.BtnName = "BtnAddNew";
                TempData["ModelData"] = AddNewModel;
                UrlModel AddNew_Model = new UrlModel();
                AddNew_Model.bt = "BtnAddNew";
                AddNew_Model.Cmd = "Add";
                AddNew_Model.tp = "Save";
                TempData["ListFilterData"] = null;
                /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    br_id = Session["BranchId"].ToString();
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYear(CompID, br_id) == "Not Exist")
                {
                    TempData["Message"] = "Financial Year not Exist";
                    return RedirectToAction("ProductionOrder");
                }
                /*End to chk Financial year exist or not*/
                return RedirectToAction("AddProductionOrderDetail", AddNew_Model);
                // return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionOrder/ProductionOrderDetail.cshtml", _ProductionOrderModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult ProductionOrderDetail()
        {
            try
            {
                BindShopFloorList(_ProductionOrderModel);
                BindDDLOnPageLoad(_ProductionOrderModel);
                _ProductionOrderModel.jc_dt = System.DateTime.Now.ToString("dd-MM-yyyy");
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                _ProductionOrderModel.Title = title;
                //ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionOrder/ProductionOrderDetail.cshtml", _ProductionOrderModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
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
        private DataTable dtJCHeaderTbl()
        {
            DataTable dtJCHeader = new DataTable();
            dtJCHeader.Columns.Add("comp_id", typeof(int));
            dtJCHeader.Columns.Add("br_id", typeof(int));
            dtJCHeader.Columns.Add("jc_no", typeof(string));
            dtJCHeader.Columns.Add("jc_dt", typeof(string));
            dtJCHeader.Columns.Add("Ord_type", typeof(string));
            dtJCHeader.Columns.Add("src_type", typeof(string));
            dtJCHeader.Columns.Add("product_id", typeof(string));
            dtJCHeader.Columns.Add("uom_id", typeof(int));
            dtJCHeader.Columns.Add("op_output_itemid", typeof(string));
            dtJCHeader.Columns.Add("op_output_uom_id", typeof(int));
            dtJCHeader.Columns.Add("advice_no", typeof(string));
            dtJCHeader.Columns.Add("advice_dt", typeof(string));
            dtJCHeader.Columns.Add("jc_qty", typeof(string));
            dtJCHeader.Columns.Add("op_id", typeof(string));
            dtJCHeader.Columns.Add("shfl_id", typeof(string));
            dtJCHeader.Columns.Add("ws_id", typeof(string));
            dtJCHeader.Columns.Add("batch_no", typeof(string));
            dtJCHeader.Columns.Add("supervisor_name", typeof(string));
            dtJCHeader.Columns.Add("jc_st_date", typeof(string));
            dtJCHeader.Columns.Add("jc_en_date", typeof(string));
            dtJCHeader.Columns.Add("shift_id", typeof(string));
            dtJCHeader.Columns.Add("create_id", typeof(int));
            dtJCHeader.Columns.Add("jc_status", typeof(string));
            dtJCHeader.Columns.Add("mac_id", typeof(string));
            dtJCHeader.Columns.Add("TransType", typeof(string));
            dtJCHeader.Columns.Add("AutoMRS", typeof(string));

            return dtJCHeader;
        }
        private string CheckProductionCnfAgainstPrdctionOrdr(ProductionOrderModel _ProductionOrderModel)
        {
            try
            {
                string str = string.Empty;
                if (Session["BranchId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                //string date = Convert.ToDateTime(_ProductionOrderModel.jc_dt).ToString("yyyy-MM-dd");
                //DateTime dt3 = DateTime.ParseExact(_ProductionOrderModel.jc_dt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                str = _ProductionOrder_ISERVICES.CheckPCagainstPrOrdr(CompID, br_id, _ProductionOrderModel.jc_no, _ProductionOrderModel.jc_dt, _ProductionOrderModel.PrducOrderType).Tables[0].Rows[0]["result"].ToString();
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        
        [ValidateAntiForgeryToken]
        public ActionResult ProductionOrderSave(ProductionOrderModel _ProductionOrderModel, string command)
        {
            /*----- Attatchment Section start--------*/
            string SaveMessage = "";
            CommonPageDetails();
            /*getDocumentName()*/; /* To set Title*/
            string PageName = title.Replace(" ", "");
            /*----- Attatchment Section End--------*/

            try
            {/*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (Session["compid"] != null)
                {
                    CompID = Session["compid"].ToString();
                    br_id = Session["BranchId"].ToString();
                    _ProductionOrderModel.comp_id = Convert.ToInt32(Session["compid"].ToString());
                    _ProductionOrderModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                    _ProductionOrderModel.create_id = Session["UserId"].ToString();
                    Int32 create_id = Convert.ToInt32(IsNull(_ProductionOrderModel.create_id, "0"));
                    string SystemDetail = string.Empty;
                    SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                    _ProductionOrderModel.mac_id = SystemDetail;//Session["UserMacaddress"].ToString();
                    string mac_id = _ProductionOrderModel.mac_id;
                    string jc_no = _ProductionOrderModel.jc_no;

                    if (_ProductionOrderModel.DeleteCommand == "Delete")
                    {
                        command = "Delete";
                    }
                    DataTable dtJCHeader = new DataTable();
                    dtJCHeader = dtJCHeaderTbl();
                    DataRow drJCHeader = dtJCHeader.NewRow();

                    DataTable dtJCItemDetail = new DataTable();
                    dtJCItemDetail.Columns.Add("jc_no", typeof(string));
                    dtJCItemDetail.Columns.Add("jc_dt", typeof(DateTime));
                    dtJCItemDetail.Columns.Add("item_id", typeof(string));
                    dtJCItemDetail.Columns.Add("uom_id", typeof(int));
                    dtJCItemDetail.Columns.Add("Item_type_id", typeof(string));
                    dtJCItemDetail.Columns.Add("req_qty", typeof(float));
                    dtJCItemDetail.Columns.Add("seq_no", typeof(int));
                    dtJCItemDetail.Columns.Add("remarks", typeof(string));

                    /*------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));

                    /*------------------Sub Item end----------------------*/


                    switch (command)
                    {
                        case "Edit":
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                BranchId = Session["BranchId"].ToString();
                            //if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                            //{
                            //    TempData["Message"] = "Financial Year not Exist";
                            //    return RedirectToAction("dbClickEdit", new { jc_no = _ProductionOrderModel.jc_no, jc_dt = _ProductionOrderModel.jc_dt, ListFilterData = _ProductionOrderModel.ListFilterData1, WF_Status = _ProductionOrderModel.WFStatus });
                            //}
                            /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                            //string PrdOrdDt = _ProductionOrderModel.jc_dt;
                            //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BranchId, PrdOrdDt) == "TransNotAllow")
                            //{
                            //    TempData["Message1"] = "TransNotAllow";
                            //    return RedirectToAction("dbClickEdit", new { jc_no = _ProductionOrderModel.jc_no, jc_dt = _ProductionOrderModel.jc_dt, ListFilterData = _ProductionOrderModel.ListFilterData1, WF_Status = _ProductionOrderModel.WFStatus });
                            //}
                            /*End to chk Financial year exist or not*/
                            //Session["Message"] = "";
                            UrlModel EditModel = new UrlModel();
                            var msg = "";
                            if (_ProductionOrderModel.DocumentStatus != "D" && _ProductionOrderModel.DocumentStatus != "F")
                                msg = (CheckProductionCnfAgainstPrdctionOrdr(_ProductionOrderModel));
                            
                            if (msg == "UsedPRDCF")
                            {
                                //Session["Message"] = "UsedPRDCF";
                                _ProductionOrderModel.Message = "UsedPRDCF";
                                _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                _ProductionOrderModel.TransType = "Update";
                                _ProductionOrderModel.Command = "Refresh";
                                _ProductionOrderModel.PO_No = _ProductionOrderModel.jc_no;
                                _ProductionOrderModel.PO_dt = _ProductionOrderModel.jc_dt;
                                TempData["ModelData"] = _ProductionOrderModel;
                                //UrlModel EditModel = new UrlModel();
                                EditModel.Cmd = "Refresh";
                                EditModel.tp = "Update";
                                EditModel.bt = "BtnToDetailPage";
                                EditModel.PO_No = _ProductionOrderModel.jc_no;
                                EditModel.PO_dt = _ProductionOrderModel.jc_dt;
                            }

                            else if (msg == "UsedJO")
                            {
                                //Session["Message"] = "UsedJO";
                                _ProductionOrderModel.Message = "UsedJO";
                                _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                _ProductionOrderModel.TransType = "Update";
                                _ProductionOrderModel.Command = "Refresh";
                                _ProductionOrderModel.PO_No = _ProductionOrderModel.jc_no;
                                _ProductionOrderModel.PO_dt = _ProductionOrderModel.jc_dt;
                                TempData["ModelData"] = _ProductionOrderModel;

                                EditModel.Cmd = "Refresh";
                                EditModel.tp = "Update";
                                EditModel.bt = "BtnEdit";
                                EditModel.PO_No = _ProductionOrderModel.jc_no;
                                EditModel.PO_dt = _ProductionOrderModel.jc_dt;
                            }
                            else if (msg == "UsedMRS")
                            {
                                //Session["Message"] = "UsedJO";
                                _ProductionOrderModel.Message = "UsedMRS";
                                _ProductionOrderModel.BtnName = "BtnToDetailPage";
                                _ProductionOrderModel.TransType = "Update";
                                _ProductionOrderModel.Command = "Refresh";
                                _ProductionOrderModel.PO_No = _ProductionOrderModel.jc_no;
                                _ProductionOrderModel.PO_dt = _ProductionOrderModel.jc_dt;
                                TempData["ModelData"] = _ProductionOrderModel;

                                EditModel.Cmd = "Refresh";
                                EditModel.tp = "Update";
                                EditModel.bt = "BtnEdit";
                                EditModel.PO_No = _ProductionOrderModel.jc_no;
                                EditModel.PO_dt = _ProductionOrderModel.jc_dt;
                            }
                            else
                            {
                                //Session["Command"] = "Edit";
                                //Session["BtnName"] = "BtnAddNew";
                                //Session["TransType"] = "Update";
                                //Session["dbclick"] = "dbclick";
                                _ProductionOrderModel.Command = command;
                                _ProductionOrderModel.BtnName = "BtnEdit";
                                _ProductionOrderModel.TransType = "Update";
                                _ProductionOrderModel.PO_No = _ProductionOrderModel.jc_no;
                                _ProductionOrderModel.PO_dt = _ProductionOrderModel.jc_dt;
                                TempData["ModelData"] = _ProductionOrderModel;
                                //UrlModel EditModel = new UrlModel();
                                EditModel.Cmd = command;
                                EditModel.tp = "Update";
                                EditModel.bt = "BtnEdit";
                                EditModel.PO_No = _ProductionOrderModel.jc_no;
                                EditModel.PO_dt = _ProductionOrderModel.jc_dt;
                            }
                            TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                            return RedirectToAction("AddProductionOrderDetail", EditModel);
                        case "Print":
                            return GenratePdfFile(_ProductionOrderModel);
                        case "AddNew":

                            //Session["Message"] = "";
                            //Session.Remove("TransType");
                            //Session["Command"] = "Add";
                            //Session["TransType"] = "Save";
                            //Session["BtnName"] = "BtnAddNew";
                            //Session["bom_status"] = "bom_status";
                            //Session.Remove("dbclick");
                            //Session.Remove("SaveUpd");
                            //Session["DocumentStatus"] = "";
                            //drJCHeader["jc_dt"] = _ProductionOrderModel.jc_dt;
                            ProductionOrderModel adddnew = new ProductionOrderModel();
                            adddnew.Command = "Add";
                            adddnew.TransType = "Save";
                            adddnew.BtnName = "BtnAddNew";
                            TempData["ModelData"] = adddnew;
                            UrlModel NewModel = new UrlModel();
                            NewModel.Cmd = "Add";
                            NewModel.tp = "Save";
                            NewModel.bt = "BtnAddNew";
                            TempData["ListFilterData"] = null;
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                BranchId = Session["BranchId"].ToString();
                            if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                            {
                                TempData["Message"] = "Financial Year not Exist";
                                if (!string.IsNullOrEmpty(_ProductionOrderModel.jc_no))
                                    return RedirectToAction("dbClickEdit", new { jc_no = _ProductionOrderModel.jc_no, jc_dt = _ProductionOrderModel.jc_dt, ListFilterData = _ProductionOrderModel.ListFilterData1, WF_Status = _ProductionOrderModel.WFStatus });
                                else
                                    adddnew.Command = "Refresh";
                                adddnew.TransType = "Refresh";
                                adddnew.BtnName = "Refresh";
                                adddnew.DocumentStatus = null;
                                TempData["ModelData"] = adddnew;
                                return RedirectToAction("AddProductionOrderDetail", adddnew);
                            }
                           /*End to chk Financial year exist or not*/
                            return RedirectToAction("AddProductionOrderDetail", NewModel);

                        case "Approve":
                            /*start Add by Hina on 19-02-2024 to chk Financial year exist or not*/
                            if (Session["CompId"] != null)
                                CompID = Session["CompId"].ToString();
                            if (Session["BranchId"] != null)
                                BranchId = Session["BranchId"].ToString();
                            //if (commCont.CheckFinancialYear(CompID, BranchId) == "Not Exist")
                            //{
                            //    TempData["Message"] = "Financial Year not Exist";
                            //    return RedirectToAction("dbClickEdit", new { jc_no = _ProductionOrderModel.jc_no, jc_dt = _ProductionOrderModel.jc_dt, ListFilterData = _ProductionOrderModel.ListFilterData1, WF_Status = _ProductionOrderModel.WFStatus });
                            //}
                            /*Above Commented and modify by Hina sharma on 09-05-2025 to check Existing with previous year transaction*/
                            //string PrdOrdDt1 = _ProductionOrderModel.jc_dt;
                            //if (commCont.CheckFinancialYearAndPreviousYear(CompID, BranchId, PrdOrdDt1) == "TransNotAllow")
                            //{
                            //    TempData["Message1"] = "TransNotAllow";
                            //    return RedirectToAction("dbClickEdit", new { jc_no = _ProductionOrderModel.jc_no, jc_dt = _ProductionOrderModel.jc_dt, ListFilterData = _ProductionOrderModel.ListFilterData1, WF_Status = _ProductionOrderModel.WFStatus });
                            //}
                            /*End to chk Financial year exist or not*/
                            //Session["Command"] = command;
                            //Session["TransType"] = command;
                            Approve_ProductionOrderDetails(_ProductionOrderModel, command, "");
                            TempData["ModelData"] = _ProductionOrderModel;
                            UrlModel ApproveModel = new UrlModel();
                            ApproveModel.tp = "Update";
                            ApproveModel.PO_No = _ProductionOrderModel.PO_No;
                            ApproveModel.PO_dt = _ProductionOrderModel.PO_dt;
                            ApproveModel.bt = "BtnSave";
                            ApproveModel.Cmd = "Approve";
                            //var Trtype2 = Session["TransType"].ToString();
                            //_ProductionOrderModel.TransType = Trtype2;
                            //drJCHeader["comp_id"] = _ProductionOrderModel.comp_id;
                            //drJCHeader["br_id"] = _ProductionOrderModel.br_id;
                            //drJCHeader["jc_no"] = _ProductionOrderModel.jc_no;
                            //string dt2 = _ProductionOrderModel.jc_dt;

                            //DateTime dt3 = DateTime.ParseExact(dt2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //_ProductionOrderModel.jc_dt = dt3.ToString("yyyy-MM-dd");
                            //drJCHeader["jc_dt"] = _ProductionOrderModel.jc_dt;
                            ////drJCHeader["src_type"] = _ProductionOrderModel.ddl_SourceType;
                            //drJCHeader["product_id"] = _ProductionOrderModel.product_id;
                            //drJCHeader["uom_id"] = _ProductionOrderModel.uom_id;
                            //drJCHeader["advice_no"] = _ProductionOrderModel.hdnadviceno;
                            //drJCHeader["advice_dt"] = _ProductionOrderModel.advicedt;
                            //drJCHeader["jc_qty"] = _ProductionOrderModel.jc_qty;
                            ////drJCHeader["rev_no"] = _ProductionOrderModel.ddl_RevisionNumber;
                            //drJCHeader["op_id"] = _ProductionOrderModel.ddl_OperationName;
                            //drJCHeader["shfl_id"] = _ProductionOrderModel.ddl_ShopfloorName;
                            //drJCHeader["ws_id"] = _ProductionOrderModel.ddl_WorkstationName;
                            //drJCHeader["batch_no"] = _ProductionOrderModel.batch_no;
                            //drJCHeader["supervisor_name"] = _ProductionOrderModel.supervisor_name;
                            //drJCHeader["jc_st_date"] = _ProductionOrderModel.jc_st_date.Replace("T", " ");
                            //drJCHeader["jc_en_date"] = _ProductionOrderModel.jc_en_date.Replace("T", " ");
                            //drJCHeader["shift_id"] = _ProductionOrderModel.ddl_shift;
                            //drJCHeader["create_id"] = _ProductionOrderModel.create_id;
                            //_ProductionOrderModel.jc_status = "A";
                            //drJCHeader["jc_status"] = _ProductionOrderModel.jc_status;
                            //drJCHeader["mac_id"] = _ProductionOrderModel.mac_id;
                            //drJCHeader["TransType"] = _ProductionOrderModel.TransType;
                            //dtJCHeader.Rows.Add(drJCHeader);

                            ///*---------Attachments Section Start----------------*/
                            //DataTable JCSaveAttachments2 = new DataTable();

                            //JCSaveAttachments2.Columns.Add("id", typeof(string));
                            //JCSaveAttachments2.Columns.Add("file_name", typeof(string));
                            //JCSaveAttachments2.Columns.Add("file_path", typeof(string));
                            //JCSaveAttachments2.Columns.Add("file_def", typeof(char));
                            //JCSaveAttachments2.Columns.Add("comp_id", typeof(Int32));

                            //DataRow JCSavedrAttachments = JCSaveAttachments2.NewRow();
                            ///*---------Attachments Section End----------------*/

                            //SaveMessage = _ProductionOrder_ISERVICES.insertJCDetail(dtJCHeader, dtJCItemDetail, JCSaveAttachments2, "","","");
                            //string[] splitmsg = SaveMessage.Split(',');
                            //if (splitmsg[0].ToString().Trim() == "Approve")
                            //{
                            //    Session["Message"] = "Approved";
                            //    Session["Command"] = "EditNew";
                            //    Session["TransType"] = "Approve";
                            //    Session["BtnName"] = "BtnSave";
                            //    ViewBag.Message = Session["Message"].ToString();
                            //    Session["SaveUpd"] = "AfterSaveUpdate";
                            //    Session["br_id"] = Session["BranchId"].ToString();
                            //    _ProductionOrderModel.jc_no = splitmsg[1].ToString().Trim();
                            //    Session["jc_no"] = splitmsg[1].ToString().Trim();
                            //    Session["jc_dt"] = splitmsg[2].ToString().Trim();
                            //    Session["dbclick"] = "dbclick";
                            //}
                            //else if (splitmsg[0].ToString().Trim() == "Duplicate")
                            //{
                            //    Session["Message"] = "Duplicate";
                            //    ViewBag.Message = Session["Message"].ToString();
                            //}
                            TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                            return RedirectToAction("AddProductionOrderDetail", ApproveModel);

                        case "Delete":
                            //Session["Command"] = command;
                            //Session["TransType"] = "Delete";

                            //var Trtype1 = Session["TransType"].ToString();
                            //var Trtype = Session["TransType"].ToString();
                            //_ProductionOrderModel.TransType = Trtype;
                            _ProductionOrderModel.TransType = "Delete";
                            drJCHeader["comp_id"] = _ProductionOrderModel.comp_id;
                            drJCHeader["br_id"] = _ProductionOrderModel.br_id;
                            drJCHeader["jc_no"] = _ProductionOrderModel.jc_no;
                            //string dt6 = _ProductionOrderModel.jc_dt;

                            //DateTime dt7 = DateTime.ParseExact(dt6, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //_ProductionOrderModel.jc_dt = dt7.ToString("yyyy-MM-dd");
                            drJCHeader["jc_dt"] = _ProductionOrderModel.jc_dt;
                            drJCHeader["Ord_type"] = _ProductionOrderModel.PrducOrderType;
                            drJCHeader["src_type"] = _ProductionOrderModel.SourceType;
                            drJCHeader["product_id"] = _ProductionOrderModel.product_id;
                            drJCHeader["uom_id"] = _ProductionOrderModel.uom_id;
                            drJCHeader["op_output_itemid"] = _ProductionOrderModel.op_output_itemid;
                            drJCHeader["op_output_uom_id"] = _ProductionOrderModel.op_output_uom_id;
                            drJCHeader["advice_no"] = _ProductionOrderModel.hdnadviceno;
                            drJCHeader["advice_dt"] = _ProductionOrderModel.advicedt;
                            drJCHeader["jc_qty"] = _ProductionOrderModel.jc_qty;
                            //drJCHeader["rev_no"] = _ProductionOrderModel.ddl_RevisionNumber;
                            drJCHeader["op_id"] = _ProductionOrderModel.ddl_OperationName;
                            if (_ProductionOrderModel.PrducOrderType == "SC")
                            {
                                drJCHeader["shfl_id"] = "0";
                                drJCHeader["ws_id"] = "0";
                                drJCHeader["batch_no"] = _ProductionOrderModel.batch_no;
                                drJCHeader["supervisor_name"] = "";
                                drJCHeader["jc_st_date"] = "1900-01-01 00:00:00.000";
                                drJCHeader["jc_en_date"] = "1900-01-01 00:00:00.000";
                                drJCHeader["shift_id"] = "0";
                            }
                            else
                            {
                                drJCHeader["shfl_id"] = _ProductionOrderModel.ddl_ShopfloorName;
                                drJCHeader["ws_id"] = _ProductionOrderModel.ddl_WorkstationName;
                                drJCHeader["batch_no"] = _ProductionOrderModel.batch_no;
                                drJCHeader["supervisor_name"] = _ProductionOrderModel.supervisor_name;
                                drJCHeader["jc_st_date"] = _ProductionOrderModel.jc_st_date == null ? null : _ProductionOrderModel.jc_st_date.Replace("T", " ");
                                drJCHeader["jc_en_date"] = _ProductionOrderModel.jc_en_date == null ? null : _ProductionOrderModel.jc_en_date.Replace("T", " ");
                                drJCHeader["shift_id"] = _ProductionOrderModel.ddl_shift;
                            }

                            drJCHeader["create_id"] = _ProductionOrderModel.create_id;
                            _ProductionOrderModel.jc_status = "";
                            drJCHeader["jc_status"] = _ProductionOrderModel.jc_status;
                            drJCHeader["mac_id"] = _ProductionOrderModel.mac_id;
                            drJCHeader["TransType"] = _ProductionOrderModel.TransType;
                            if (_ProductionOrderModel.AutoMRS == true)
                            {
                                drJCHeader["AutoMRS"] = "Y";
                            }
                            else
                            {
                                drJCHeader["AutoMRS"] = "N";
                            }
                            dtJCHeader.Rows.Add(drJCHeader);

                            /*---------Attachments Section Start----------------*/
                            DataTable JCSaveAttachments1 = new DataTable();

                            JCSaveAttachments1.Columns.Add("id", typeof(string));
                            JCSaveAttachments1.Columns.Add("file_name", typeof(string));
                            JCSaveAttachments1.Columns.Add("file_path", typeof(string));
                            JCSaveAttachments1.Columns.Add("file_def", typeof(char));
                            JCSaveAttachments1.Columns.Add("comp_id", typeof(Int32));

                            DataRow drAttachments = JCSaveAttachments1.NewRow();
                            /*---------Attachments Section End----------------*/
                            string compny_id = Convert.ToInt32(_ProductionOrderModel.comp_id).ToString();
                            string jcprdOrdNo = _ProductionOrderModel.jc_no;
                            string PrdOrdJCNum = jcprdOrdNo.Replace("/", "");
                            string Brid = Session["BranchId"].ToString();

                            DataTable ProductionSch = DtTableProdSchedule(_ProductionOrderModel.HdnProductionSchDetail);
                            SaveMessage = _ProductionOrder_ISERVICES.insertJCDetail(dtJCHeader, dtJCItemDetail, JCSaveAttachments1, dtSubItem, "", "", "", ProductionSch);
                            
                            string[] splitmsg = SaveMessage.Split(',');
                            //string ProdOrdCode = splitmsg[1].ToString().Trim();
                            //string PrdOrd_Number = ProdOrdCode.Replace("/", "");
                            string PrdOrd_Message = splitmsg[0].ToString().Trim();
                            /*---------Attachments Section Start----------------*/
                            if (!string.IsNullOrEmpty(PrdOrdJCNum))
                            {
                                //getDocumentName(); /* To set Title*/
                                PageName = title.Replace(" ", "");
                                var other = new CommonController(_Common_IServices);
                                other.DeleteTempFile(compny_id + Brid, PageName, PrdOrdJCNum, Server);
                            }
                            /*---------Attachments Section End----------------*/


                            if (PrdOrd_Message == "Delete")
                            {
                                //Session["Message"] = "Deleted";
                                //Session["Command"] = "Refresh";
                                //Session["TransType"] = "Refresh";
                                //Session["BtnName"] = "BtnDelete";
                                //Session["dbclick"] = null;
                                //ViewBag.Message = Session["Message"].ToString();
                                ProductionOrderModel DeleteModel = new ProductionOrderModel();
                                DeleteModel.Message = "Deleted";
                                DeleteModel.Command = "Refresh";
                                DeleteModel.TransType = "Refresh";
                                DeleteModel.BtnName = "BtnRefresh";
                                TempData["ModelData"] = DeleteModel;
                                UrlModel Delete_Model = new UrlModel();
                                Delete_Model.Cmd = DeleteModel.Command;
                                Delete_Model.tp = "Refresh";
                                Delete_Model.bt = "BtnRefresh";
                                TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                                // _ProductionOrderModel = null;

                                return RedirectToAction("AddProductionOrderDetail", Delete_Model);
                            }
                            else
                            {
                                TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                                return RedirectToAction("AddProductionOrderDetail");
                            }
                        case "Save":
                            //Session["Command"] = command;
                            //Trtype = Session["TransType"].ToString();
                            if (_ProductionOrderModel.jc_no != null)
                            {
                                _ProductionOrderModel.TransType = "Update";
                            }
                            else
                            {
                                _ProductionOrderModel.TransType = "Save";

                            }
                            // _ProductionOrderModel.TransType = Trtype;
                            var Trtype = _ProductionOrderModel.TransType;
                            drJCHeader["comp_id"] = _ProductionOrderModel.comp_id;

                            drJCHeader["br_id"] = _ProductionOrderModel.br_id;
                            string br_id = _ProductionOrderModel.br_id.ToString();
                            string jc_no1 = _ProductionOrderModel.jc_no;
                            if (string.IsNullOrEmpty(jc_no1))
                            {
                                drJCHeader["jc_no"] = "";
                            }
                            else
                            {
                                drJCHeader["jc_no"] = jc_no1;
                            }

                            drJCHeader["jc_dt"] = _ProductionOrderModel.jc_dt;
                            drJCHeader["Ord_type"] = _ProductionOrderModel.PrducOrderType;
                            drJCHeader["src_type"] = _ProductionOrderModel.SourceType;
                            drJCHeader["product_id"] = _ProductionOrderModel.product_id;
                            drJCHeader["uom_id"] = _ProductionOrderModel.uom_id;
                            drJCHeader["op_output_itemid"] = _ProductionOrderModel.op_output_itemid;
                            drJCHeader["op_output_uom_id"] = _ProductionOrderModel.op_output_uom_id;
                            drJCHeader["advice_no"] = _ProductionOrderModel.hdnadviceno;
                            drJCHeader["advice_dt"] = _ProductionOrderModel.advicedt;
                            drJCHeader["jc_qty"] = _ProductionOrderModel.jc_qty;
                            //drJCHeader["rev_no"] = _ProductionOrderModel.ddl_RevisionNumber;
                            drJCHeader["op_id"] = _ProductionOrderModel.ddl_OperationName;
                            if (_ProductionOrderModel.PrducOrderType == "SC")
                            {
                                drJCHeader["shfl_id"] = "0";
                                drJCHeader["ws_id"] = "0";
                                drJCHeader["batch_no"] = _ProductionOrderModel.batch_no;
                                drJCHeader["supervisor_name"] = "";
                                drJCHeader["jc_st_date"] = "1900-01-01 00:00:00.000";
                                drJCHeader["jc_en_date"] = "1900-01-01 00:00:00.000";
                                drJCHeader["shift_id"] = "0";
                            }
                            else
                            {
                                drJCHeader["shfl_id"] = _ProductionOrderModel.ddl_ShopfloorName;
                                drJCHeader["ws_id"] = _ProductionOrderModel.ddl_WorkstationName;
                                drJCHeader["batch_no"] = _ProductionOrderModel.batch_no;
                                drJCHeader["supervisor_name"] = _ProductionOrderModel.supervisor_name;
                                drJCHeader["jc_st_date"] = _ProductionOrderModel.jc_st_date == null ? null : _ProductionOrderModel.jc_st_date.Replace("T", " ");
                                drJCHeader["jc_en_date"] = _ProductionOrderModel.jc_en_date == null ? null : _ProductionOrderModel.jc_en_date.Replace("T", " ");

                                drJCHeader["shift_id"] = _ProductionOrderModel.ddl_shift;
                            }
                            drJCHeader["create_id"] = _ProductionOrderModel.create_id;
                            if (Trtype == "Save")
                            {
                                _ProductionOrderModel.jc_status = "D";
                                //Session["jc_status"] = "D";
                            }
                            else if (Trtype == "Update")
                            {
                                if (_ProductionOrderModel.ForceClose == true)
                                {
                                    _ProductionOrderModel.jc_status = "FC";
                                }
                                else if (_ProductionOrderModel.CancelFlag == true)
                                {
                                    _ProductionOrderModel.jc_status = "C";
                                }
                                else if (_ProductionOrderModel.CancelFlag == false)
                                {
                                    //_ProductionOrderModel.jc_status = "A";
                                    _ProductionOrderModel.jc_status = "D";
                                }
                            }
                            //if (Session["jc_status"].ToString() == "D")
                            //if (_ProductionOrderModel.jc_status == "D")
                            //{
                            //    _ProductionOrderModel.jc_status = "D";
                            //}
                            drJCHeader["jc_status"] = _ProductionOrderModel.jc_status;
                            drJCHeader["mac_id"] = _ProductionOrderModel.mac_id;
                            drJCHeader["TransType"] = _ProductionOrderModel.TransType;
                              if (_ProductionOrderModel.AutoMRS == true)
                            {
                                drJCHeader["AutoMRS"] = "Y";
                            }
                            else
                            {
                                drJCHeader["AutoMRS"] = "N";
                            }
                              
                            dtJCHeader.Rows.Add(drJCHeader);

                            JArray jObject = JArray.Parse(_ProductionOrderModel.JCItemdetails);
                            for (int i = 0; i < jObject.Count; i++)
                            {
                                DataRow drAddItemDetail = dtJCItemDetail.NewRow();
                                drAddItemDetail["jc_no"] = jObject[i]["jc_no"];
                                //string dt = jObject[i]["jc_dt"].ToString();
                                //string Date = DateTime.ParseExact(dt, "dd-MM-yyyy", null).ToString("yyyy-MM-dd");
                                //DateTime dt1 = Convert.ToDateTime(Date);
                                drAddItemDetail["jc_dt"] = jObject[i]["jc_dt"].ToString();
                                drAddItemDetail["item_id"] = jObject[i]["item_id"];
                                drAddItemDetail["uom_id"] = jObject[i]["uom_id"];
                                drAddItemDetail["Item_type_id"] = jObject[i]["Item_type_id"];
                                drAddItemDetail["req_qty"] = jObject[i]["req_qty"];
                                drAddItemDetail["seq_no"] = jObject[i]["seq_no"];
                                drAddItemDetail["remarks"] = jObject[i]["remarks"];
                                dtJCItemDetail.Rows.Add(drAddItemDetail);
                            }
                            DataTable dt1 = dtitemdetail(jObject);
                            DataView dv = new DataView(dt1);
                            DataTable dtInput = new DataTable();
                            DataTable dtOutput = new DataTable();
                            dv.RowFilter = "Item_type_id ='OF' or Item_type_id = 'OW' ";
                            dtOutput = dv.ToTable();
                            //ViewData["InItemDetails"] = dtitemdetail(jObject);
                            //_ProductionOrderModel.JC_Item_Details_List = dtitemdetail(jObject);
                            ViewData["OutItemDetails"] = dtOutput;
                            dv.RowFilter = "Item_type_id ='IR' ";
                            dtInput= dv.ToTable();
                            List<JC_Item_details> ArrItem = new List<JC_Item_details>();

                            for (int i = 0; i < dtInput.Rows.Count; i++)
                            {
                                JC_Item_details jid = new JC_Item_details();
                                jid.item_id = Convert.ToString(dtInput.Rows[i]["item_id"]);
                                jid.item_name = Convert.ToString(dtInput.Rows[i]["item_name"]);
                                jid.uom_id = Convert.ToString(dtInput.Rows[i]["uom_id"]);
                                jid.uom_name = Convert.ToString(dtInput.Rows[i]["uom_name"]);
                                jid.Item_type_id = Convert.ToString(dtInput.Rows[i]["Item_type_id"]);
                                jid.Item_type_name = Convert.ToString(dtInput.Rows[i]["Item_type_name"]);
                                jid.req_qty = Convert.ToString(dtInput.Rows[i]["req_qty"]);
                                jid.avl_stock_shfl = Convert.ToString(dtInput.Rows[i]["avl_stock_shfl"]);
                                jid.avl_stock_warehouse = Convert.ToString(dtInput.Rows[i]["avl_stock_warehouse"]);
                                jid.seq_no = Convert.ToString(dtInput.Rows[i]["seq_no"]);
                                //jid.alt_fill = Convert.ToString(dtInput.Rows[i]["alt_fill"]);
                                ArrItem.Add(jid);
                            }
                            //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];
                            _ProductionOrderModel.JC_Item_Details_List = ArrItem;
                           
                            /*-----------------Attachment Section Start------------------------*/
                            DataTable PrdOrdrAttachments = new DataTable();
                            DataTable PrdOrdrdtAttachment = new DataTable();
                            var attachData = TempData["IMGDATA"] as AttachMentModel;
                            TempData["IMGDATA"] = null;
                            if (_ProductionOrderModel.attatchmentdetail != null)
                            {
                                if (attachData != null)
                                {
                                    //if (Session["AttachMentDetailItmStp"] != null)
                                    //{
                                    //    PrdOrdrdtAttachment = Session["AttachMentDetailItmStp"] as DataTable;
                                    //}
                                    if (attachData.AttachMentDetailItmStp != null)
                                    {
                                        PrdOrdrdtAttachment = attachData.AttachMentDetailItmStp as DataTable;
                                    }
                                    else
                                    {
                                        PrdOrdrdtAttachment.Columns.Add("id", typeof(string));
                                        PrdOrdrdtAttachment.Columns.Add("file_name", typeof(string));
                                        PrdOrdrdtAttachment.Columns.Add("file_path", typeof(string));
                                        PrdOrdrdtAttachment.Columns.Add("file_def", typeof(char));
                                        PrdOrdrdtAttachment.Columns.Add("comp_id", typeof(Int32));

                                    }
                                }
                                else
                                {
                                    if (_ProductionOrderModel.AttachMentDetailItmStp != null)
                                    {
                                        PrdOrdrdtAttachment = _ProductionOrderModel.AttachMentDetailItmStp as DataTable;
                                    }
                                    else
                                    {
                                        PrdOrdrdtAttachment.Columns.Add("id", typeof(string));
                                        PrdOrdrdtAttachment.Columns.Add("file_name", typeof(string));
                                        PrdOrdrdtAttachment.Columns.Add("file_path", typeof(string));
                                        PrdOrdrdtAttachment.Columns.Add("file_def", typeof(char));
                                        PrdOrdrdtAttachment.Columns.Add("comp_id", typeof(Int32));

                                    }
                                }
                                JArray jObject1 = JArray.Parse(_ProductionOrderModel.attatchmentdetail);
                                for (int i = 0; i < jObject1.Count; i++)
                                {
                                    string flag = "Y";
                                    foreach (DataRow dr in PrdOrdrdtAttachment.Rows)
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

                                        DataRow dtrowAttachment1 = PrdOrdrdtAttachment.NewRow();
                                        if (!string.IsNullOrEmpty((_ProductionOrderModel.jc_no).ToString()))
                                        {
                                            dtrowAttachment1["id"] = _ProductionOrderModel.jc_no;
                                        }
                                        else
                                        {
                                            dtrowAttachment1["id"] = "0";
                                        }
                                        dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                        dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                        dtrowAttachment1["file_def"] = "Y";
                                        dtrowAttachment1["comp_id"] = Session["CompId"].ToString();
                                        PrdOrdrdtAttachment.Rows.Add(dtrowAttachment1);
                                    }
                                }
                                //if (Session["TransType"].ToString() == "Update")
                                if (_ProductionOrderModel.TransType == "Update")
                                {
                                    //int brnch_id = Convert.ToInt32(Session["BranchId"].ToString());
                                    string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                                    if (Directory.Exists(AttachmentFilePath))
                                    {
                                        string PrdOrder_CODE = string.Empty;
                                        if (!string.IsNullOrEmpty((_ProductionOrderModel.jc_no).ToString()))
                                        {
                                            PrdOrder_CODE = (_ProductionOrderModel.jc_no).ToString();

                                        }
                                        else
                                        {
                                            PrdOrder_CODE = "0";
                                        }
                                        string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + br_id + PrdOrder_CODE.Replace("/", "") + "*");

                                        foreach (var fielpath in filePaths)
                                        {
                                            string flag = "Y";
                                            foreach (DataRow dr in PrdOrdrdtAttachment.Rows)
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
                                PrdOrdrAttachments = PrdOrdrdtAttachment;
                            }
                            /*------------------Sub Item ----------------------*/

                            if (_ProductionOrderModel.SubItemDetailsDt != null)
                            {
                                JArray jObject2 = JArray.Parse(_ProductionOrderModel.SubItemDetailsDt);
                                for (int i = 0; i < jObject2.Count; i++)
                                {
                                    DataRow dtrowItemdetails = dtSubItem.NewRow();
                                    dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                                    dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                                    dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                                    dtSubItem.Rows.Add(dtrowItemdetails);
                                }
                                ViewData["SubItemDetails"] = dtsubitemdetail(jObject2);
                            }

                            /*------------------Sub Item end----------------------*/

                            /*-----------------Attachment Section End------------------------*/
                            //string br_id = Session["BranchId"].ToString();
                            //string br_id = drJCHeader["br_id"].ToString();
                             ProductionSch = DtTableProdSchedule(_ProductionOrderModel.HdnProductionSchDetail);
                            SaveMessage = _ProductionOrder_ISERVICES.insertJCDetail(dtJCHeader, dtJCItemDetail, PrdOrdrAttachments, dtSubItem, "", "", "", ProductionSch);
                            if (SaveMessage == "DocModify")
                            {
                                _ProductionOrderModel.Message = "DocModify";
                                _ProductionOrderModel.BtnName = "BtnRefresh";
                                _ProductionOrderModel.Command = "Refresh";
                                TempData["ModelData"] = _ProductionOrderModel;
                                ViewBag.ProductionScheduleDetails = DtTableProdSchedule1(_ProductionOrderModel.HdnProductionSchDetail);
                                _ProductionOrderModel.product_name = _ProductionOrderModel.product_name;
                                _ProductionOrderModel.uom_Name = _ProductionOrderModel.UOMName;
                                List<AdviceNumber> _AdviceNumberList = new List<AdviceNumber>();
                                AdviceNumber _AdviceNumber = new AdviceNumber();
                                _AdviceNumber.advice_no = _ProductionOrderModel.hdnadviceno;
                                _AdviceNumber.advice_dt = "0";
                                _AdviceNumberList.Add(_AdviceNumber);
                                _ProductionOrderModel.AdviceNumberList = _AdviceNumberList;

                                List<OperationName> op = new List<OperationName>();
                                OperationName opObj = new OperationName();
                                opObj.op_id = _ProductionOrderModel.ddl_op_id;
                                opObj.op_name = _ProductionOrderModel.OP_Name;
                                op.Add(opObj);
                                _ProductionOrderModel.OperationNameList = op;

                                List<WorkstationName> ws = new List<WorkstationName>();
                                WorkstationName wsObj = new WorkstationName();
                                wsObj.ws_id = _ProductionOrderModel.ddl_WorkstationName;
                                wsObj.ws_name =_ProductionOrderModel.ddl_WorkstationText;
                                ws.Add(wsObj);
                                _ProductionOrderModel.WorkstationNameList = ws;

                                List<shift> sh = new List<shift>();
                                shift shObj = new shift();
                                shObj.id = _ProductionOrderModel.ddl_shift;
                                shObj.name = _ProductionOrderModel.ddl_shiftName;
                                sh.Add(shObj);
                                _ProductionOrderModel.shiftList = sh;

                                ViewBag.OPItemList = ViewData["OutItemDetails"];

                                _ProductionOrderModel.hdnadviceno = _ProductionOrderModel.hdnadviceno;
                                _ProductionOrderModel.OP_Name = _ProductionOrderModel.OP_Name;
                                _ProductionOrderModel.op_output_uom_Name = _ProductionOrderModel.op_output_UomName;
                                _ProductionOrderModel.ddl_WorkstationText = _ProductionOrderModel.ddl_WorkstationText;
                               // _ProductionOrderModel.sh
                               //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                                _ProductionOrderModel.BtnName = "BtnRefresh";
                                _ProductionOrderModel.Command = "Refresh";
                                _ProductionOrderModel.DocumentStatus = "D";

                                _ProductionOrderModel.jc_dt = System.DateTime.Now.ToString("yyyy-MM-dd");
                                BindShopFloorList(_ProductionOrderModel);
                                
                                //ViewBag.MenuPageName = getDocumentName();
                                _ProductionOrderModel.Title = title;
                                

                                var other = new CommonController(_Common_IServices);
                                ViewBag.AppLevel = other.GetApprovalLevel(CompID, br_id, DocumentMenuId);
                                ViewBag.DocumentMenuId = DocumentMenuId;
                                //ViewBag.VBRoleList = GetRoleList();
                                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionOrder/ProductionOrderDetail.cshtml", _ProductionOrderModel);


                            }
                            else
                            {

                                splitmsg = SaveMessage.Split(',');
                                string ProdOrdCode = splitmsg[1].ToString().Trim();
                                string PrdOrd_Number = ProdOrdCode.Replace("/", "");
                                string PrdOrdMessage = splitmsg[0].ToString().Trim();
                                if (PrdOrdMessage == "Data_Not_Found")
                                {
                                    //var a = SaveMessage.Split(',');
                                    var msgs = PrdOrdMessage.Replace("_", " ") + " " + ProdOrdCode + " in " + PageName;//ProdOrdCode is use for table type
                                    string path = Server.MapPath("~");
                                    Errorlog.LogError_customsg(path, msgs, "", "");
                                    _ProductionOrderModel.Message = PrdOrdMessage.Split(',')[0].Replace("_", "");
                                    return View("~/Views/Shared/Error.cshtml");
                                }
                                /*-----------------Attachment Section Start------------------------*/
                                if (PrdOrdMessage == "Save")

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
                                    comCont.ResetImageLocation(CompID, br_id, guid, PageName, PrdOrd_Number, _ProductionOrderModel.TransType, PrdOrdrAttachments);
                                    //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                                    //string[] filePaths = Directory.GetFiles(sourcePath, CompID + br_id + Guid + "_" + "*");
                                    //foreach (string file in filePaths)
                                    //{
                                    //    string[] items = file.Split('\\');
                                    //    string ItemName = items[items.Length - 1];
                                    //    ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                                    //    foreach (DataRow dr in PrdOrdrAttachments.Rows)
                                    //    {
                                    //        string DrItmNm = dr["file_name"].ToString();
                                    //        if (ItemName == DrItmNm)
                                    //        {
                                    //            string img_nm = CompID + br_id + PrdOrd_Number + "_" + Path.GetFileName(DrItmNm).ToString();
                                    //            string doc_path = Path.Combine(Server.MapPath("~/Attachment/" + PageName + "/"), img_nm);
                                    //            string DocumentPath = Server.MapPath("~/Attachment/" + PageName + "/");
                                    //            if (!Directory.Exists(DocumentPath))
                                    //            {
                                    //                DirectoryInfo di = Directory.CreateDirectory(DocumentPath);
                                    //            }

                                    //            System.IO.File.Move(file, doc_path);
                                    //        }
                                    //    }
                                    //}


                                }
                                /*-----------------Attachment Section End------------------------*/

                                if (PrdOrdMessage == "Update" || PrdOrdMessage == "Save")
                                {
                                    _ProductionOrderModel.Message = "Save";
                                    _ProductionOrderModel.PO_No = ProdOrdCode;
                                    _ProductionOrderModel.PO_dt = splitmsg[2].ToString().Trim();
                                    _ProductionOrderModel.TransType = "Update";
                                    _ProductionOrderModel.BtnName = "BtnSave";
                                    _ProductionOrderModel.SaveUpd = "AfterSaveUpdate";
                                    _ProductionOrderModel.DocumentStatus = "c";
                                    TempData["ModelData"] = _ProductionOrderModel;
                                    UrlModel SaveModel = new UrlModel();
                                    SaveModel.bt = _ProductionOrderModel.BtnName;
                                    SaveModel.PO_No = _ProductionOrderModel.PO_No;
                                    SaveModel.PO_dt = _ProductionOrderModel.PO_dt;
                                    SaveModel.tp = _ProductionOrderModel.TransType;
                                    SaveModel.Cmd = _ProductionOrderModel.Command;
                                    TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                                    return RedirectToAction("AddProductionOrderDetail", SaveModel);
                                }
                                if (PrdOrdMessage == "Cancelled"|| PrdOrdMessage == "ForceClosed")
                                {
                                    try
                                    {
                                       // string fileName = "PP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                        string fileName = "ProductionOrder_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                        var filePath = SavePdfDocToSendOnEmailAlert(_ProductionOrderModel.jc_no, _ProductionOrderModel.jc_dt, fileName, DocumentMenuId,"C");
                                        _Common_IServices.SendAlertEmail(Session["CompId"].ToString(), Session["BranchId"].ToString(), DocumentMenuId, _ProductionOrderModel.jc_no, "C", Session["UserID"].ToString(), "", filePath);
                                    }
                                    catch (Exception exMail)
                                    {
                                        _ProductionOrderModel.Message = "ErrorInMail";
                                        string path = Server.MapPath("~");
                                        Errorlog.LogError(path, exMail);
                                    }
                                    _ProductionOrderModel.Message = PrdOrdMessage == "ErrorInMail" ? "Cancelled_ErrorInMail" : PrdOrdMessage == "Cancelled" ? "Cancelled" : "Save";
                                    //_ProductionOrderModel.Message = PrdOrdMessage == "Cancelled"? "Cancelled" : "Save";
                                    _ProductionOrderModel.PO_No = ProdOrdCode;
                                    _ProductionOrderModel.PO_dt = splitmsg[2].ToString().Trim();
                                    _ProductionOrderModel.TransType = "Update";
                                    _ProductionOrderModel.BtnName = "BtnSave";
                                    _ProductionOrderModel.SaveUpd = "AfterSaveUpdate";
                                    _ProductionOrderModel.DocumentStatus = PrdOrdMessage == "Cancelled"?"C":"FC";
                                    _ProductionOrderModel.Command = "Refresh";
                                    TempData["ModelData"] = _ProductionOrderModel;
                                    UrlModel CancelModel = new UrlModel();
                                    CancelModel.bt = _ProductionOrderModel.BtnName;
                                    CancelModel.PO_No = _ProductionOrderModel.PO_No;
                                    CancelModel.PO_dt = _ProductionOrderModel.PO_dt;
                                    CancelModel.tp = _ProductionOrderModel.TransType;
                                    CancelModel.Cmd = _ProductionOrderModel.Command;
                                    TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                                    return RedirectToAction("AddProductionOrderDetail", CancelModel);
                                }
                                //else if (PrdOrdMessage == "ForceClosed")
                                //{
                                //    string fileName = "PP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                                //    var filePath = SavePdfDocToSendOnEmailAlert(_ProductionOrderModel.jc_no, _ProductionOrderModel.jc_dt, fileName);
                                //    _Common_IServices.SendAlertEmail(Session["CompId"].ToString(), Session["BranchId"].ToString(), DocumentMenuId, _ProductionOrderModel.jc_no, "C", Session["UserID"].ToString(), "", filePath);

                                //    _ProductionOrderModel.Message = "Cancelled";
                                //    _ProductionOrderModel.PO_No = ProdOrdCode;
                                //    _ProductionOrderModel.PO_dt = splitmsg[2].ToString().Trim();
                                //    _ProductionOrderModel.TransType = "Update";
                                //    _ProductionOrderModel.BtnName = "BtnSave";
                                //    _ProductionOrderModel.SaveUpd = "AfterSaveUpdate";
                                //    _ProductionOrderModel.DocumentStatus = "c";
                                //    _ProductionOrderModel.Command = "Refresh";
                                //    TempData["ModelData"] = _ProductionOrderModel;
                                //    UrlModel CancelModel = new UrlModel();
                                //    CancelModel.bt = _ProductionOrderModel.BtnName;
                                //    CancelModel.PO_No = _ProductionOrderModel.PO_No;
                                //    CancelModel.PO_dt = _ProductionOrderModel.PO_dt;
                                //    CancelModel.tp = _ProductionOrderModel.TransType;
                                //    CancelModel.Cmd = _ProductionOrderModel.Command;
                                //    TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                                //    return RedirectToAction("AddProductionOrderDetail", CancelModel);
                                //}
                                else if (PrdOrdMessage == "Duplicate")
                                {
                                    _ProductionOrderModel.Message = "Duplicate";
                                    _ProductionOrderModel.PO_No = ProdOrdCode;
                                    _ProductionOrderModel.PO_dt = splitmsg[2].ToString().Trim();
                                    _ProductionOrderModel.TransType = "Update";
                                    _ProductionOrderModel.BtnName = "BtnSave";
                                    _ProductionOrderModel.Command = "Refresh";
                                    TempData["ModelData"] = _ProductionOrderModel;
                                    UrlModel Dupli_Model = new UrlModel();
                                    Dupli_Model.bt = _ProductionOrderModel.BtnName;
                                    Dupli_Model.PO_No = _ProductionOrderModel.PO_No;
                                    Dupli_Model.PO_dt = _ProductionOrderModel.PO_dt;
                                    Dupli_Model.tp = _ProductionOrderModel.TransType;
                                    Dupli_Model.Cmd = _ProductionOrderModel.Command;
                                    return RedirectToAction("AddProductionOrderDetail", Dupli_Model);
                                }
                                return RedirectToAction("AddProductionOrderDetail");
                            }
                        case "Refresh":
                            ProductionOrderModel RefreshModel = new ProductionOrderModel();
                            RefreshModel.Command = command;
                            RefreshModel.BtnName = "BtnRefresh";
                            RefreshModel.TransType = "Save";
                            TempData["ModelData"] = RefreshModel;
                            UrlModel refesh_Model = new UrlModel();
                            refesh_Model.tp = "Save";
                            refesh_Model.bt = "BtnRefresh";
                            refesh_Model.Cmd = command;
                            TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                            _ProductionOrderModel = null;

                            return RedirectToAction("AddProductionOrderDetail", refesh_Model);
                        case "BacktoList":
                            var WF_Status = _ProductionOrderModel.WF_Status1;
                            TempData["ListFilterData"] = _ProductionOrderModel.ListFilterData1;
                            _ProductionOrderModel = null;

                            return RedirectToAction("ProductionOrder", "ProductionOrder", new { WF_Status });
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                /*---------------Attachment Section start-------------------*/
                string compn_id = Convert.ToInt32(_ProductionOrderModel.comp_id).ToString();
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (_ProductionOrderModel.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (_ProductionOrderModel.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = _ProductionOrderModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(compn_id + br_id, PageName, Guid, Server);
                    }
                }
                /*-----------------Attachment Section end------------------*/
                return View("~/Views/Shared/Error.cshtml");
            }
            return RedirectToAction("AddBillofMaterialDetail");
        }
        public DataTable dtitemdetail(JArray jObject)
        {
            DataTable dtJCItemDetail = new DataTable();

            dtJCItemDetail.Columns.Add("jc_no", typeof(string));
            dtJCItemDetail.Columns.Add("jc_dt", typeof(DateTime));
            dtJCItemDetail.Columns.Add("item_id", typeof(string));
            dtJCItemDetail.Columns.Add("item_name", typeof(string));
            dtJCItemDetail.Columns.Add("uom_id", typeof(int));
            dtJCItemDetail.Columns.Add("uom_name", typeof(string));
            dtJCItemDetail.Columns.Add("uom_alias", typeof(string));
            dtJCItemDetail.Columns.Add("Item_type_id", typeof(string)); 
            dtJCItemDetail.Columns.Add("Item_type_name", typeof(string));
            dtJCItemDetail.Columns.Add("Item_type", typeof(string));
            dtJCItemDetail.Columns.Add("req_qty", typeof(string));
            dtJCItemDetail.Columns.Add("pend_qty", typeof(string));
            dtJCItemDetail.Columns.Add("seq_no", typeof(int));
            dtJCItemDetail.Columns.Add("avl_stock_shfl", typeof(string));
            dtJCItemDetail.Columns.Add("avl_stock_warehouse", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow drAddItemDetail = dtJCItemDetail.NewRow();
                drAddItemDetail["jc_no"] = jObject[i]["jc_no"];
                //string dt = jObject[i]["jc_dt"].ToString();
                //string Date = DateTime.ParseExact(dt, "dd-MM-yyyy", null).ToString("yyyy-MM-dd");
                //DateTime dt1 = Convert.ToDateTime(Date);
                drAddItemDetail["jc_dt"] = jObject[i]["jc_dt"].ToString();
                drAddItemDetail["item_id"] = jObject[i]["item_id"];
                drAddItemDetail["item_name"] = jObject[i]["item_name"];
                drAddItemDetail["uom_id"] = jObject[i]["uom_id"];
                drAddItemDetail["Item_type_id"] = jObject[i]["Item_type_id"];
                if (jObject[i]["Item_type_id"].ToString()=="OF"|| jObject[i]["Item_type_id"].ToString()=="OW")
                {
                    drAddItemDetail["uom_alias"] = jObject[i]["uom_name"];
                    drAddItemDetail["Item_type"] = jObject[i]["Item_type_name"];
                    drAddItemDetail["pend_qty"] = jObject[i]["pend_qty"];
                }
                else
                {
                    drAddItemDetail["uom_name"] = jObject[i]["uom_name"];
                    drAddItemDetail["Item_type_name"] = jObject[i]["Item_type_name"];
                    drAddItemDetail["avl_stock_shfl"] = jObject[i]["avl_stock_shfl"];
                    drAddItemDetail["avl_stock_warehouse"] = jObject[i]["avl_stock_warehouse"];
                }
                drAddItemDetail["req_qty"] = jObject[i]["req_qty"];
                drAddItemDetail["seq_no"] = jObject[i]["seq_no"];
                
                dtJCItemDetail.Rows.Add(drAddItemDetail);
            }
            
            return dtJCItemDetail;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }

            return dtSubItem;
        }

        public DataTable DtTableProdSchedule(string ProdSchedule)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("prod_dt", typeof(string));
            dt.Columns.Add("prod_qty", typeof(string));
            dt.Columns.Add("remarks", typeof(string));
            /*------------------Sub Item ----------------------*/

            if (ProdSchedule != null)
            {
                JArray jObject2 = JArray.Parse(ProdSchedule);
                for (int i = 0; i < jObject2.Count; i++)
                {
                    DataRow dtrowItemdetails = dt.NewRow();
                    dtrowItemdetails["prod_dt"] = jObject2[i]["sch_date"].ToString();
                    dtrowItemdetails["prod_qty"] = jObject2[i]["sch_qty"].ToString();
                    dtrowItemdetails["remarks"] = jObject2[i]["remarks"].ToString();
                    dt.Rows.Add(dtrowItemdetails);
                }
                //ViewData["SubItemDetails"] = dtsubitemdetail(jObject2);
            }

            /*------------------Sub Item end----------------------*/
            return dt;
        }
        public DataTable DtTableProdSchedule1(string ProdSchedule)//For Showing unSaved data
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("prod_dt", typeof(string));
            dt.Columns.Add("hdn_prod_dt", typeof(string));
            dt.Columns.Add("prod_qty", typeof(string));
            dt.Columns.Add("remarks", typeof(string));
            /*------------------Sub Item ----------------------*/

            if (ProdSchedule != null)
            {
                JArray jObject2 = JArray.Parse(ProdSchedule);
                for (int i = 0; i < jObject2.Count; i++)
                {
                    DataRow dtrowItemdetails = dt.NewRow();
                    dtrowItemdetails["prod_dt"] = jObject2[i]["sch_date_toShow"].ToString();
                    dtrowItemdetails["hdn_prod_dt"] = jObject2[i]["sch_date"].ToString();
                    dtrowItemdetails["prod_qty"] = jObject2[i]["sch_qty"].ToString();
                    dtrowItemdetails["remarks"] = jObject2[i]["remarks"].ToString();
                    dt.Rows.Add(dtrowItemdetails);
                }
                //ViewData["SubItemDetails"] = dtsubitemdetail(jObject2);
            }

            /*------------------Sub Item end----------------------*/
            return dt;
        }
        [HttpPost]
        public ActionResult GetProductNameInDDL(ProductionOrderModel _ProductionOrderModel)
        {
            JsonResult DataRows = null;
            string SOItmName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_ProductionOrderModel.SO_ItemName))
                    {
                        SOItmName = "0";
                    }
                    else
                    {
                        SOItmName = _ProductionOrderModel.SO_ItemName;
                    }
                    DataSet ProductList = _ProductionOrder_ISERVICES.BindProductNameInDDL(Comp_ID, Br_ID, SOItmName);
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public ActionResult GetProductNameInDDLListPage(ProductionOrderModel _ProductionOrderModel)
        {
            JsonResult DataRows = null;
            string SOItmName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_ProductionOrderModel.SO_ItemName))
                    {
                        SOItmName = "0";
                    }
                    else
                    {
                        SOItmName = _ProductionOrderModel.SO_ItemName;
                    }

                    DataSet ProductList = _ProductionOrder_ISERVICES.BindProductNameInDDL(Comp_ID, Br_ID, SOItmName);
                    DataRow DRow = ProductList.Tables[0].NewRow();

                    DRow[0] = "0";
                    DRow[1] = "---Select---";
                    DRow[2] = "0";
                    ProductList.Tables[0].Rows.InsertAt(DRow, 0);
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/

                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public JsonResult GetProductionOrederItemUOM(string Itm_ID)
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
                DataSet result = _ProductionOrder_ISERVICES.GetSOItemUOMDL(Itm_ID, Comp_ID, Convert.ToInt32(Br_ID));

                DataRow Drow = result.Tables[1].NewRow();
                Drow[0] = "---Select---";
                Drow[1] = "0";
                result.Tables[1].Rows.InsertAt(Drow, 0);

                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                _ProductionOrderModel.product_id = Itm_ID;
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
        public JsonResult BindOPName_BaseOnRevNo(string Itm_ID, int rev_no, string ProductionOrderNumber, string Jc_Date)
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
                DateTime jcdt1 = DateTime.ParseExact(Jc_Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string jcdt2 = jcdt1.ToString("yyyy-MM-dd");
                DataSet result = _ProductionOrder_ISERVICES.BindOPNameBaseOnRevNo(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), Itm_ID, rev_no, ProductionOrderNumber, jcdt2);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                _ProductionOrderModel.product_id = Itm_ID;
                return DataRows;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public void BindShopFloorList(ProductionOrderModel _ProductionOrderModel)
        {
            DataTable dt = new DataTable();
            try
            {
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();
                    dt = _ProductionOrder_ISERVICES.GetShopFloorDetailsDAL(Convert.ToInt32(Comp_ID), Convert.ToInt32(br_id));
                    List<ShopFloor> _Status = new List<ShopFloor>();
                    ShopFloor _Statuslist1 = new ShopFloor();
                    _Statuslist1.shfl_id = "0";
                    _Statuslist1.shfl_name = "---Select---";
                    _Status.Add(_Statuslist1);
                    foreach (DataRow data in dt.Rows)
                    {
                        ShopFloor _Statuslist = new ShopFloor();
                        _Statuslist.shfl_id = data["shfl_id"].ToString();
                        _Statuslist.shfl_name = data["shfl_name"].ToString();
                        _Status.Add(_Statuslist);
                    }
                    _ProductionOrderModel.ShopFloorList = _Status;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        public void BindDDLOnPageLoad(ProductionOrderModel _ProductionOrderModel)
        {
            DataTable dt = new DataTable();
            try
            {
                if (Session["CompId"] != null)
                {
                    //List<SourceType> st = new List<SourceType>();
                    //SourceType stObj = new SourceType();
                    //stObj.id = "D";
                    //stObj.name = "Direct";
                    //st.Add(stObj);
                    //SourceType stObj1 = new SourceType();
                    //stObj1.id = "P";
                    //stObj1.name = "Production Plan";
                    //st.Add(stObj1);
                    ////_ProductionOrderModel.SourceTypeList = st;

                    //List<RevisionNumber> _Status = new List<RevisionNumber>();
                    //RevisionNumber rev = new RevisionNumber();
                    //rev.rev_no = "";
                    //rev.rev_text = "---Select---";
                    //_Status.Add(rev);
                    //_ProductionOrderModel.RevisionNumberList = _Status;

                    List<AdviceNumber> _AdviceNumberList = new List<AdviceNumber>();
                    AdviceNumber _AdviceNumber = new AdviceNumber();
                    _AdviceNumber.advice_no = "--- Select ---";
                    _AdviceNumber.advice_dt = "0";
                    _AdviceNumberList.Add(_AdviceNumber);
                    _ProductionOrderModel.AdviceNumberList = _AdviceNumberList;

                    List<OperationName> op = new List<OperationName>();
                    OperationName opObj = new OperationName();
                    opObj.op_id = "0";
                    opObj.op_name = "---Select---";
                    op.Add(opObj);
                    _ProductionOrderModel.OperationNameList = op;

                    List<WorkstationName> ws = new List<WorkstationName>();
                    WorkstationName wsObj = new WorkstationName();
                    wsObj.ws_id = "0";
                    wsObj.ws_name = "---Select---";
                    ws.Add(wsObj);
                    _ProductionOrderModel.WorkstationNameList = ws;
                    GetshiftList(_ProductionOrderModel);/*Add and commented by Hina Sharma on 07-03-2025 for filter*/
                    //List<shift> sh = new List<shift>();
                    //shift shObj = new shift();
                    //shObj.id = "0";
                    //shObj.name = "---Select---";
                    //sh.Add(shObj);
                    //shift shObj1 = new shift();
                    //shObj1.id = "1";
                    //shObj1.name = "Shift-1";
                    //sh.Add(shObj1);
                    //shift shObj2 = new shift();
                    //shObj2.id = "2";
                    //shObj2.name = "Shift-2";
                    //sh.Add(shObj2);
                    //shift shObj3 = new shift();
                    //shObj3.id = "3";
                    //shObj3.name = "Shift-3";
                    //sh.Add(shObj3);
                    //_ProductionOrderModel.shiftList = sh;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        [HttpPost]
        public ActionResult BindWorkStationList(int shfl_id)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    if (string.IsNullOrEmpty(_ProductionOrderModel.product_id))
                    {
                        product_id = "0";
                    }
                    else
                    {
                        product_id = _ProductionOrderModel.product_id;
                    }
                    DataSet ProductList = _ProductionOrder_ISERVICES.GetWorkStationDAL(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), shfl_id);
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/

                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        [HttpPost]
        public ActionResult Plus_AddAttribute(string advice_no, string advice_dt, int op_id, string shflid, string itemid, string ProdQty, string Product_id)
        {
            JsonResult DataRows = null;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        Br_ID = Session["BranchId"].ToString();
                    }
                    DataSet ProductList = _ProductionOrder_ISERVICES.Bind_Plus_AddAttribute(Convert.ToInt32(Comp_ID), Convert.ToInt32(Br_ID), advice_no, advice_dt, op_id, shflid, itemid, ProdQty, Product_id);
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }

        [HttpPost]
        public ActionResult GetItemDetailsList(string productid, string advice_no, string advice_dt, int op_id)
        {
            JsonResult DataRows = null;

            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                    if (Session["BranchId"] != null)
                    {
                        br_id = Session["BranchId"].ToString();
                    }
                    DataSet ProductList = _ProductionOrder_ISERVICES.Get_ItemDetailsList(Convert.ToInt32(CompID), Convert.ToInt32(br_id), productid, advice_no, advice_dt, op_id);
                    DataRows = Json(JsonConvert.SerializeObject(ProductList));/*Result convert into Json Format for javasript*/
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        //private string getDocumentName()
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["Language"] != null)
        //        {
        //            language = Session["Language"].ToString();
        //        }
        //        string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(CompID, DocumentMenuId, language);
        //        string[] Docpart = DocumentName.Split('>');
        //        int len = Docpart.Length;
        //        if (len > 1)
        //        {
        //            title = Docpart[len - 1].Trim();
        //        }
        //        return DocumentName;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
        public ActionResult ToRefreshByJS(string ListFilterData1, string ModelData,string Mailerror)
        {
            //Session["Message"] = "";
            ProductionOrderModel ToRefreshByJS = new ProductionOrderModel();
            UrlModel Model = new UrlModel();
            var a = ModelData.Split(',');
            ToRefreshByJS.PO_No = a[0].Trim();
            ToRefreshByJS.PO_dt = a[1].Trim();
            ToRefreshByJS.TransType = "Update";
            ToRefreshByJS.BtnName = "BtnToDetailPage";
            ToRefreshByJS.Message =  Mailerror;
            if (a[2].Trim() != null && a[2].Trim() != "")
            {
                ToRefreshByJS.WF_Status1 = a[2].Trim();
                Model.wf = a[2].Trim();
            }
            Model.bt = "BtnToDetailPage";
            Model.PO_No = ToRefreshByJS.PO_No;
            Model.PO_dt = ToRefreshByJS.PO_dt;
            Model.tp = "Update";
            TempData["ModelData"] = ToRefreshByJS;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("AddProductionOrderDetail", Model);
        }
        public ActionResult GetProductionOrderList(string docid, string status)
        {

            //Session["WF_status"] = status;
            var WF_Status = status;
            return RedirectToAction("ProductionOrder", new { WF_Status });
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
            try
            {
                ProductionOrderModel _ProductionOrderModel = new ProductionOrderModel();
                if (AppDtList != null)
                {
                    JArray jObjectBatch = JArray.Parse(AppDtList);
                    for (int i = 0; i < jObjectBatch.Count; i++)
                    {
                        _ProductionOrderModel.jc_no = jObjectBatch[i]["DocNo"].ToString();
                        _ProductionOrderModel.jc_dt = jObjectBatch[i]["DocDate"].ToString();
                        _ProductionOrderModel.PrducOrderType = jObjectBatch[i]["OrderType"].ToString();
                        _ProductionOrderModel.SourceType = jObjectBatch[i]["SrcType"].ToString();
                        _ProductionOrderModel.A_Status = jObjectBatch[i]["A_Status"].ToString();
                        _ProductionOrderModel.A_Level = jObjectBatch[i]["A_Level"].ToString();
                        _ProductionOrderModel.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();
                        _ProductionOrderModel.product_id = jObjectBatch[i]["product_id"].ToString();
                        _ProductionOrderModel.hdnadviceno = jObjectBatch[i]["Adv_No"].ToString();
                    }
                }
                if (_ProductionOrderModel.A_Status != "Approve")
                {
                    _ProductionOrderModel.A_Status = "Approve";
                }
                _ProductionOrderModel.jc_st_date = "2022-02-19T14:55";
                _ProductionOrderModel.jc_en_date = "2022-02-20T14:55";
                string command = "Approve";
                Approve_ProductionOrderDetails(_ProductionOrderModel, command, ListFilterData1);
                UrlModel ApproveModel = new UrlModel();
                if (WF_Status1 != null && WF_Status1 != "")
                {
                    _ProductionOrderModel.WF_Status1 = WF_Status1;
                    ApproveModel.wf = WF_Status1;
                }

                ApproveModel.tp = "Update";
                ApproveModel.PO_No = _ProductionOrderModel.PO_No;
                if (_ProductionOrderModel.PO_dt != null && _ProductionOrderModel.PO_dt != "")
                {
                    ApproveModel.PO_dt = _ProductionOrderModel.PO_dt;
                }
                else
                {
                    ApproveModel.PO_dt = _ProductionOrderModel.jc_dt;
                    _ProductionOrderModel.PO_dt = _ProductionOrderModel.jc_dt;
                }
                ApproveModel.bt = "BtnSave";
                ApproveModel.Cmd = "Approve";
                TempData["ModelData"] = _ProductionOrderModel;
                return RedirectToAction("AddProductionOrderDetail", ApproveModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        public ActionResult Approve_ProductionOrderDetails(ProductionOrderModel _ProductionOrderModel, string command, string ListFilterData1)
        {
            try
            {
                DataTable dtJCHeader = dtJCHeaderTbl();
                DataRow drJCHeader = dtJCHeader.NewRow();
                DataTable dtJCItemDetail = new DataTable();
                DataTable ProductionSch = new DataTable();
                dtJCItemDetail.Columns.Add("jc_no", typeof(string));
                dtJCItemDetail.Columns.Add("jc_dt", typeof(DateTime));
                dtJCItemDetail.Columns.Add("item_id", typeof(string));
                dtJCItemDetail.Columns.Add("uom_id", typeof(int));
                dtJCItemDetail.Columns.Add("Item_type_id", typeof(string));
                dtJCItemDetail.Columns.Add("req_qty", typeof(float));
                dtJCItemDetail.Columns.Add("seq_no", typeof(int));
                dtJCItemDetail.Columns.Add("remarks", typeof(string));

                //Session["Command"] = command;
                //Session["TransType"] = command;
                _ProductionOrderModel.TransType = command;
                drJCHeader["comp_id"] = Session["CompId"].ToString();
                drJCHeader["br_id"] = Session["BranchId"].ToString();

                drJCHeader["jc_no"] = _ProductionOrderModel.jc_no;
                //string dt2 = _ProductionOrderModel.jc_dt;

                //DateTime dt3 = DateTime.ParseExact(dt2, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //_ProductionOrderModel.jc_dt = dt3.ToString("yyyy-MM-dd");
                drJCHeader["jc_dt"] = _ProductionOrderModel.jc_dt;
                drJCHeader["Ord_type"] = _ProductionOrderModel.PrducOrderType;
                drJCHeader["src_type"] = _ProductionOrderModel.SourceType;
                drJCHeader["product_id"] = IsNull(_ProductionOrderModel.product_id, "0");
                drJCHeader["op_output_itemid"] = IsNull(_ProductionOrderModel.op_output_itemid, "0");
                drJCHeader["op_output_uom_id"] = IsNull(_ProductionOrderModel.op_output_uom_id, "0");
                drJCHeader["uom_id"] = IsNull(_ProductionOrderModel.uom_id, "0");
                drJCHeader["advice_no"] = IsNull(_ProductionOrderModel.hdnadviceno, "0");
                drJCHeader["advice_dt"] = IsNull(_ProductionOrderModel.advicedt, "0");
                drJCHeader["jc_qty"] = _ProductionOrderModel.jc_qty;
                //drJCHeader["rev_no"] = IsNull(_ProductionOrderModel.ddl_RevisionNumber, "0");
                drJCHeader["op_id"] = IsNull(_ProductionOrderModel.ddl_OperationName, "0");

                if (_ProductionOrderModel.PrducOrderType == "SC")
                {
                    drJCHeader["shfl_id"] = "0";
                    drJCHeader["ws_id"] = "0";
                    drJCHeader["batch_no"] = IsNull(_ProductionOrderModel.batch_no, "0");
                    drJCHeader["supervisor_name"] = "";
                    drJCHeader["jc_st_date"] = "1900-01-01 00:00:00.000";
                    drJCHeader["jc_en_date"] = "1900-01-01 00:00:00.000";
                    drJCHeader["shift_id"] = "0";
                }
                else
                {
                    drJCHeader["shfl_id"] = IsNull(_ProductionOrderModel.ddl_ShopfloorName, "0");
                    drJCHeader["ws_id"] = IsNull(_ProductionOrderModel.ddl_WorkstationName, "0");
                    drJCHeader["batch_no"] = IsNull(_ProductionOrderModel.batch_no, "0");
                    drJCHeader["supervisor_name"] = "";// _ProductionOrderModel.supervisor_name;
                    drJCHeader["jc_st_date"] = _ProductionOrderModel.jc_st_date == null ? null : _ProductionOrderModel.jc_st_date.Replace("T", " ");
                    drJCHeader["jc_en_date"] = _ProductionOrderModel.jc_en_date == null ? null : _ProductionOrderModel.jc_en_date.Replace("T", " ");
                    drJCHeader["shift_id"] = "0";// _ProductionOrderModel.ddl_shift;
                }
                drJCHeader["create_id"] = Session["UserID"].ToString();
                _ProductionOrderModel.jc_status = "A";
                drJCHeader["jc_status"] = _ProductionOrderModel.jc_status;
                string SystemDetail = string.Empty;
                SystemDetail = "MAC:" + Session["UserMacaddress"].ToString() + ",SystemName:" + Session["UserSystemName"].ToString() + ",IP:" + Session["UserIP"].ToString();
                drJCHeader["mac_id"] = SystemDetail;//_ProductionOrderModel.mac_id;
                drJCHeader["TransType"] = _ProductionOrderModel.TransType;
                if (_ProductionOrderModel.AutoMRS == true)
                {
                    drJCHeader["AutoMRS"] = "Y";
                }
                else
                {
                    drJCHeader["AutoMRS"] = "N";
                }
                dtJCHeader.Rows.Add(drJCHeader);

                /*---------Attachments Section Start----------------*/
                DataTable JCSaveAttachments3 = new DataTable();

                JCSaveAttachments3.Columns.Add("id", typeof(string));
                JCSaveAttachments3.Columns.Add("file_name", typeof(string));
                JCSaveAttachments3.Columns.Add("file_path", typeof(string));
                JCSaveAttachments3.Columns.Add("file_def", typeof(char));
                JCSaveAttachments3.Columns.Add("comp_id", typeof(Int32));

                DataRow Jcsave_drAttachments = JCSaveAttachments3.NewRow();
                /*---------Attachments Section End----------------*/


                string A_Status = _ProductionOrderModel.A_Status;
                string A_Level = _ProductionOrderModel.A_Level;
                string A_Remarks = _ProductionOrderModel.A_Remarks;

                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));

                ProductionSch = DtTableProdSchedule(_ProductionOrderModel.HdnProductionSchDetail);
                string Message = _ProductionOrder_ISERVICES.insertJCDetail(dtJCHeader, dtJCItemDetail, JCSaveAttachments3, dtSubItem, A_Status, A_Level, A_Remarks, ProductionSch);
                string[] splitmsg = Message.Split(',');
                if (splitmsg[0].ToString().Trim() == "Approve")
                {
                    try
                    {
                        //string fileName = "PP_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "ProductionOrder_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_ProductionOrderModel.jc_no, _ProductionOrderModel.jc_dt, fileName, DocumentMenuId,"AP");
                        _Common_IServices.SendAlertEmail(Session["CompId"].ToString(), Session["BranchId"].ToString(), DocumentMenuId, _ProductionOrderModel.jc_no, "AP", Session["UserID"].ToString(), "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _ProductionOrderModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _ProductionOrderModel.Message = _ProductionOrderModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                    //Session["Message"] = "Approved";
                    //Session["Command"] = "EditNew";
                    //Session["TransType"] = "Approve";
                    //Session["BtnName"] = "BtnSave";
                    //ViewBag.Message = Session["Message"].ToString();
                    //Session["SaveUpd"] = "AfterSaveUpdate";
                    //Session["br_id"] = Session["BranchId"].ToString();
                    //_ProductionOrderModel.jc_no = splitmsg[1].ToString().Trim();
                    //Session["jc_no"] = splitmsg[1].ToString().Trim();
                    //Session["jc_dt"] = splitmsg[2].ToString().Trim();
                    //Session["dbclick"] = "dbclick";
                    // UrlModel ApproveModel = new UrlModel();
                   // _ProductionOrderModel.Message = "Approved";
                    _ProductionOrderModel.PO_No = splitmsg[1].ToString().Trim();
                    _ProductionOrderModel.PO_dt = splitmsg[2].ToString().Trim(); ;
                    _ProductionOrderModel.TransType = "Update";
                    _ProductionOrderModel.BtnName = "BtnSave";
                    _ProductionOrderModel.Command = "Approve";

                }
                else if (splitmsg[0].ToString().Trim() == "Duplicate")
                {
                    //Session["Message"] = "Duplicate";
                    _ProductionOrderModel.Message = "Duplicate";
                    _ProductionOrderModel.PO_No = splitmsg[1].ToString().Trim();
                    _ProductionOrderModel.PO_dt = splitmsg[2].ToString().Trim(); ;
                    _ProductionOrderModel.TransType = "Update";
                    _ProductionOrderModel.BtnName = "BtnToDetailPage";
                    _ProductionOrderModel.Command = "Refresh";
                    // ViewBag.Message = Session["Message"].ToString();
                }
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("AddProductionOrderDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult GetAdviceDetail(string AdviceNo, string AdviceDate)
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
                    br_id = Session["BranchId"].ToString();
                }
                DataSet result = _ProductionOrder_ISERVICES.GetAdviceDetails(CompID, br_id, AdviceNo, AdviceDate);
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

        public ActionResult GetConfirmationDetail(string ItemID, string JCNumber, string JCDate)
        {
            try
            {
                JsonResult DataRows = null;
                _ProductionOrderModel = new ProductionOrderModel();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _ProductionOrder_ISERVICES.GetConfirmationDetail(CompID, BrchID, ItemID, JCNumber, JCDate);

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


        /*-----------------Attachment Section Start------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                AttachMentModel _attachmentModel = new AttachMentModel();
                //string TransType = "";
                //string jcCode = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["jc_no"] != null)
                //{
                //    jcCode = Session["jc_no"].ToString();
                //}
                if (TransType == "Save")
                {
                    //jcCode = gid.ToString();
                    DocNo = gid.ToString();
                }
                //jcCode = jcCode.Replace("/", "");
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = jcCode;
                _attachmentModel.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                    _ProductionOrderModel.br_id = Convert.ToInt32(Session["BranchId"].ToString());
                    br_id = _ProductionOrderModel.br_id.ToString();
                }
                //if (Session["BranchId"] != null)
                //{
                //    br_id = Session["BranchId"].ToString();
                //}
                CommonPageDetails();
                //getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                //DataTable dt = other.Upload(PageName, TransType, CompID+br_id, jcCode, Files, Server);
                DataTable dt = other.Upload(PageName, TransType, CompID + br_id, DocNo, Files, Server);
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

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, string Flag, string Status, string jc_no, string jc_dt,string OutputItemID)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();
                string ReqstFlag = Flag;
                if (Status == "D" || Status == "F" || Status == "")
                {
                    dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    if(Flag== "PrdOrdOutQty")
                    { 
                        if (Item_id == OutputItemID)
                            {
                                IsDisabled = "Y";
                            }
                    }

                    
                    Flag = "Quantity";
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
                else if(Flag== "POMRS_Issue" || Flag == "POMRS_Quantity")
                {
                    dt = _ProductionOrder_ISERVICES.JC_GetSubItemDetails(CompID, br_id, Item_id, jc_no, jc_dt, Flag).Tables[0];
                }
                else
                {
                    dt = _ProductionOrder_ISERVICES.JC_GetSubItemDetails(CompID, br_id, Item_id, jc_no, jc_dt, Flag).Tables[0];
                    Flag = "Quantity";
                }

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
                    dt_SubItemDetails = dt,
                    IsDisabled = IsDisabled,
                    _subitemPageName = "CNF",
                    decimalAllowed = ReqstFlag == "PrdOrdReqQty" ? "Y" : "N"
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
                throw Ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
        }

        /*--------Print---------*/

        public FileResult GenratePdfFile(ProductionOrderModel _Model)
        {
            var data = GetPdfData(_Model.jc_no, _Model.jc_dt);
            if (data != null)
                return File(data, "application/pdf", "ProductionOrder.pdf");
            else
                return File("ErrorPage", "application/pdf");
        }
        public byte[] GetPdfData(string jcNo, string jcDate)
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
                    br_id = Session["BranchId"].ToString();
                }
                DataSet Deatils = new DataSet();
                Deatils = _ProductionOrder_ISERVICES.GetProductionOrderPrintDeatils(CompID, br_id, jcNo, jcDate);
                ViewBag.PageName = "PDO";
                ViewBag.Title = "Production Order";
                ViewBag.Details = Deatils;
                //ViewBag.CompLogoDtl = Deatils.Tables[0];
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");


                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["status_code"].ToString().Trim();
                string GLVoucherHtml = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ProductionOrder/ProductionOrderPrint.cshtml"));
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
                br_id = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, br_id, docid, docstatus);
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
        public ActionResult GetAlternateItemDetails(string Product_Id,string Op_Id,string Item_Id,string shfl_id,string Disabled)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                DataSet ds = _ProductionOrder_ISERVICES.GetAlternateItemDetails(CompID, br_id, Product_Id, Op_Id, Item_Id, shfl_id,null);
                ViewBag.AlternateItemList = ds.Tables[0];
                ViewBag.Disabled = Disabled;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialSelectAlternateItem.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        public ActionResult GetSubItemWareshflAvlstockDetails(string Wh_id, string Item_id, string flag, string DocumentMenuId, 
            string UomId,string flag1,string Doc_no,string Doc_dt)
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
                if (DocumentMenuId == "105102140")
                {
                    Br_ID = br_id;
                }
                DataSet ds = _ProductionOrder_ISERVICES.GetSubItemWhAvlSHOPstockDetails(Comp_ID, Br_ID, Item_id, UomId, flag1, Doc_no, Doc_dt);
                ViewBag.SubitemAvlStockDetail = ds.Tables[0];
                ViewBag.DocumentMenuId = DocumentMenuId;
              
                if(flag == "WH") 
                {
                    ViewBag.Flag = "WH";
                }
                else
                {
                    ViewBag.Flag = "Shopfloor";
                }
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

    }
}