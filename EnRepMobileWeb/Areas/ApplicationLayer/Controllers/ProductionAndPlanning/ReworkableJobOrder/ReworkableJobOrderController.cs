using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.ReworkableJobOrder;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.ReworkableJobOrder;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.ReworkableJobOrder
{
    public class ReworkableJobOrderController : Controller
    {
        string CompID, BrchID, UserID, language, title = String.Empty;
        string DocumentMenuId = "105105127";
        List<RJOList> _RJOList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        CommonController cmn = new CommonController();
        ReworkableJobOrder_ISERVICES _ReworkableJobOrder_ISERVICES;
        DataTable dt;
        string str;
        public ReworkableJobOrderController(Common_IServices _Common_IServices, ReworkableJobOrder_ISERVICES _ReworkableJobOrder_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._ReworkableJobOrder_ISERVICES = _ReworkableJobOrder_ISERVICES;
        }
        // GET: ApplicationLayer/ReworkableJobOrder
        public ActionResult ReworkableJobOrder(string WF_Status)
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
                RJOListModel _RJOListModel = new RJOListModel();
                if (WF_Status != null && WF_Status != "")
                {
                    _RJOListModel.WF_Status = WF_Status;

                }
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;

                //List<ItemNameList> _itmListL = new List<ItemNameList>();
                List<ItemNameList> _itmListL = new List<ItemNameList>() {
                        new ItemNameList {ID="0",Name="---Select---"}
                    };
                //dt = BindItemsList();

                //foreach (DataRow dr in dt.Rows)
                //{
                //    ItemNameList _itmL = new ItemNameList();
                //    _itmL.ID = dr["item_id"].ToString();
                //    _itmL.Name = dr["item_name"].ToString();
                //    _itmListL.Add(_itmL);

                //}
                _RJOListModel.ItemNameLlist = _itmListL;

                CommonPageDetails();
                _RJOListModel.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _RJOListModel.StatusList = statusLists;
                if (TempData["ListFilterData"] != null)
                {
                    if (TempData["ListFilterData"].ToString() != "")
                    {
                        var PRData = TempData["ListFilterData"].ToString();
                        var a = PRData.Split(',');
                        _RJOListModel.ItemID = a[0].Trim();
                        _RJOListModel.FromDate = a[1].Trim();
                        _RJOListModel.ToDate = a[2].Trim();
                        _RJOListModel.Status = a[3].Trim();
                        if (_RJOListModel.Status == "0")
                        {
                            _RJOListModel.Status = null;
                        }
                        _RJOListModel.ListFilterData = TempData["ListFilterData"].ToString();
                        _RJOListModel.ItemName= a[0].Trim();
                    }
                }
                else
                {
                    _RJOListModel.FromDate = startDate;
                    _RJOListModel.ToDate = CurrentDate;
                }
                _RJOListModel.ReworkJobOrdrList = getRewrkJobOrderList(_RJOListModel);


                _RJOListModel.Title = title;
                //Session["DNSCSearch"] = "0";
                _RJOListModel.RJOSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ReworkableJobOrder/ReworkableJobOrderList.cshtml", _RJOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            


              }
        public ActionResult AddReworkableJobOrderDetail()
        {
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            ReworkableJobOrder_Model _ReworkableJobOrder_Model = new ReworkableJobOrder_Model();
            _ReworkableJobOrder_Model.Message = "New";
            _ReworkableJobOrder_Model.Command = "Add";
            _ReworkableJobOrder_Model.AppStatus = "D";
            ViewBag.DocumentStatus = _ReworkableJobOrder_Model.AppStatus;
            _ReworkableJobOrder_Model.TransType = "Save";
            _ReworkableJobOrder_Model.BtnName = "BtnAddNew";
            TempData["ModelData"] = _ReworkableJobOrder_Model;
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("ReworkableJobOrder");
            }
            /*End to chk Financial year exist or not*/
            CommonPageDetails();
            return RedirectToAction("ReworkableJobOrderDetail", "ReworkableJobOrder");
        }
        public ActionResult ReworkableJobOrderDetail(ReworkableJobOrder_Model _ReworkableJobOrder_Model1, string RwrkJOCodeURL, string RwrkJoDate, string TransType, string BtnName, string command)
        {
            try
            {
                ViewBag.DocID = DocumentMenuId;
                CommonPageDetails();
                /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, RwrkJoDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
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
                DataSet Ds_AllDdl = _ReworkableJobOrder_ISERVICES.AllDDLBind_OnPageLOad(CompID, BrchID, "0");

                var _ReworkableJobOrder_Model = TempData["ModelData"] as ReworkableJobOrder_Model;
                if (_ReworkableJobOrder_Model != null)
                {

                    
                   
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _ReworkableJobOrder_Model.Title = title;
                    _ReworkableJobOrder_Model.ValDigit = ValDigit;
                    _ReworkableJobOrder_Model.QtyDigit = QtyDigit;
                    _ReworkableJobOrder_Model.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if(_ReworkableJobOrder_Model.ReworkJO_No==null)
                    {
                        str = GetNewBatchNo();
                        _ReworkableJobOrder_Model.Newbatch_No = str;
                    }
                    
                    List<ItemName> _itmList = new List<ItemName>() {
                        new ItemName {ID="0",Name="---Select---"}
                    };
                    //dt = BindItemsList();
                   
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ItemName _itm = new ItemName();
                    //    _itm.ID = dr["item_id"].ToString();
                    //    _itm.Name = dr["item_name"].ToString();
                    //    _itmList.Add(_itm);

                    //}
                  _ReworkableJobOrder_Model.ItemNamelist = _itmList;

                    //List<wh_namelist> _whList = new List<wh_namelist>();
                    dt = Ds_AllDdl.Tables[1];// BindWarehouseList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    wh_namelist _wh = new wh_namelist();
                    //    _wh.WareH_id = dr["wh_id"].ToString();
                    //    _wh.wareH_name = dr["wh_name"].ToString();
                    //    _whList.Add(_wh);

                    //}
                    //_whList.Insert(0, new wh_namelist() { WareH_id = "0", wareH_name = "---Select---" });
                    _ReworkableJobOrder_Model.wh_Namelist = GetWarehouseList(dt);// _whList;

                    //List<Shopfloorlist> _shflrList = new List<Shopfloorlist>();
                    dt = Ds_AllDdl.Tables[2];// BindShopfloorList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    Shopfloorlist _shflr = new Shopfloorlist();
                    //    _shflr.shflr_id = dr["shfl_id"].ToString();
                    //    _shflr.shflr_name = dr["shfl_name"].ToString();
                    //    _shflrList.Add(_shflr);

                    //}
                    //_shflrList.Insert(0, new Shopfloorlist() { shflr_id = "0", shflr_name = "---Select---" });
                    _ReworkableJobOrder_Model.ShopfloorNamelist = GetShopfloorList(dt);// _shflrList;

                    List<WorkstationName> _wrklist = new List<WorkstationName>();
                    _wrklist.Add(new WorkstationName { ws_id = "0", ws_name = "---Select---" });
                    _ReworkableJobOrder_Model.WorkstationNameList = _wrklist;

                    List<ItemName1> _ItemName1 = new List<ItemName1>();
                    _ItemName1.Add(new ItemName1 { Item_Id = "0", Item_Name = "---Select---" });
                    _ReworkableJobOrder_Model.ItemNameList1 = _ItemName1;

                    //List<shift> sh = new List<shift>();/*Commented by Suraj Maurya on 25-07-2025*/
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
                    _ReworkableJobOrder_Model.shiftList = GetShiftList();// sh;

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _ReworkableJobOrder_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    if (_ReworkableJobOrder_Model.TransType == "Update" || _ReworkableJobOrder_Model.Command == "Edit")

                    {
                        string JobCard_NO = _ReworkableJobOrder_Model.ReworkJO_No;
                        string JobCard_Date = _ReworkableJobOrder_Model.ReworkJO_Date;
                        string ItemId = _ReworkableJobOrder_Model.Item_Id;
                        DataSet ds = _ReworkableJobOrder_ISERVICES.GetRewrkJODetailEditUpdate(CompID, BrchID, JobCard_NO, JobCard_Date,UserID, DocumentMenuId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {   
                            _ReworkableJobOrder_Model.ReworkJO_No = ds.Tables[0].Rows[0]["job_no"].ToString();
                            _ReworkableJobOrder_Model.DocNoAttach = ds.Tables[0].Rows[0]["job_no"].ToString();
                            _ReworkableJobOrder_Model.ReworkJO_Date = ds.Tables[0].Rows[0]["job_date"].ToString();
                            _ReworkableJobOrder_Model.Item_Name = ds.Tables[0].Rows[0]["item_name"].ToString();
                            _ReworkableJobOrder_Model.Item_Id = ds.Tables[0].Rows[0]["item_id"].ToString();
                            _ReworkableJobOrder_Model.ItemNamelist = new List<ItemName>() { new ItemName { ID = _ReworkableJobOrder_Model.Item_Id, Name = _ReworkableJobOrder_Model.Item_Name } };
                            _ReworkableJobOrder_Model.ItemName = ds.Tables[0].Rows[0]["item_id"].ToString();
                            _ReworkableJobOrder_Model.uom_id = ds.Tables[0].Rows[0]["uom"].ToString();
                            _ReworkableJobOrder_Model.uom_name = ds.Tables[0].Rows[0]["UomName"].ToString();
                            _ReworkableJobOrder_Model.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                            _ReworkableJobOrder_Model.Warehouse = ds.Tables[0].Rows[0]["wh_id"].ToString();
                            _ReworkableJobOrder_Model.WarehouseID = ds.Tables[0].Rows[0]["wh_id"].ToString();
                           // _ReworkableJobOrder_Model.WarehouseName = ds.Tables[0].Rows[0]["wh_name"].ToString();
                            //_whList.Add(new wh_namelist { WareH_id = _ReworkableJobOrder_Model.WarehouseID, wareH_name = _ReworkableJobOrder_Model.WarehouseName });
                            //_ReworkableJobOrder_Model.wh_Namelist = _whList;

                            _ReworkableJobOrder_Model.Available_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["AvlstockQty"]).ToString(QtyDigit);
                            _ReworkableJobOrder_Model.Rework_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["rwk_qty"]).ToString(QtyDigit);
                            _ReworkableJobOrder_Model.Shopfloor = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                            _ReworkableJobOrder_Model.Shopfloor_Id = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                            //_ReworkableJobOrder_Model.Shopfloor_Name = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                            _ReworkableJobOrder_Model.ddl_Workstation = ds.Tables[0].Rows[0]["ws_id"].ToString();
                            _ReworkableJobOrder_Model.WorkstationID = ds.Tables[0].Rows[0]["ws_id"].ToString();
                            _ReworkableJobOrder_Model.WorkstationName = ds.Tables[0].Rows[0]["ws_name"].ToString();
                            _wrklist.Add(new WorkstationName { ws_id = _ReworkableJobOrder_Model.ddl_Workstation, ws_name = _ReworkableJobOrder_Model.WorkstationName });
                            _ReworkableJobOrder_Model.WorkstationNameList = _wrklist;

                            _ReworkableJobOrder_Model.supervisor_name = ds.Tables[0].Rows[0]["supervisor_name"].ToString();
                            _ReworkableJobOrder_Model.ddl_shift = ds.Tables[0].Rows[0]["shift_id"].ToString();
                            _ReworkableJobOrder_Model.Newbatch_No = ds.Tables[0].Rows[0]["batch_no"].ToString();
                            _ReworkableJobOrder_Model.NewExpiryDate = ds.Tables[0].Rows[0]["exp_date"].ToString();
                            _ReworkableJobOrder_Model.Created_by = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            _ReworkableJobOrder_Model.Created_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _ReworkableJobOrder_Model.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                           
                            _ReworkableJobOrder_Model.Amended_by = ds.Tables[0].Rows[0]["AmendedBy"].ToString();
                            _ReworkableJobOrder_Model.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _ReworkableJobOrder_Model.Approved_by = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                            _ReworkableJobOrder_Model.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _ReworkableJobOrder_Model.StatusName = ds.Tables[0].Rows[0]["RJOStatus"].ToString();
                            _ReworkableJobOrder_Model.Status_Code = ds.Tables[0].Rows[0]["job_status"].ToString().Trim();
                            _ReworkableJobOrder_Model.src_type_WarehouseName = ds.Tables[0].Rows[0]["src_type"].ToString().Trim();
                            _ReworkableJobOrder_Model.NewExpiryDate_Flag = ds.Tables[0].Rows[0]["i_exp"].ToString().Trim();

                            _ReworkableJobOrder_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            _ReworkableJobOrder_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);//Cancelled
                            

                            ViewBag.ReqMaterialDetailsList = ds.Tables[1];
                            ViewBag.ItemReworkQtyDetail = ds.Tables[2];
                            ViewBag.AttechmentDetails = ds.Tables[7];
                            if (_ReworkableJobOrder_Model.Status_Code == "QP"||_ReworkableJobOrder_Model.Status_Code == "QC")
                            {
                                ViewBag.ConsumeMaterialDetails = ds.Tables[8];
                                ViewBag.ItemStockBatchWise = ds.Tables[9];
                                ViewBag.ItemStockSerialWise = ds.Tables[10];
                                ViewBag.MaterialOutputDetails = ds.Tables[11];
                            }
                            ViewBag.SubItemDetails = ds.Tables[12];
                            //if (_ReworkableJobOrder_Model.Status_Code == "QC")
                            //{
                            //    ViewBag.MaterialOutputDetails = ds.Tables[11];
                            //}
                            ViewBag.QtyDigit = QtyDigit;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["job_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _ReworkableJobOrder_Model.Cancelled = true;
                        }
                        else
                        {
                            _ReworkableJobOrder_Model.Cancelled = false;
                        }
                        if (Statuscode == "QP"||Statuscode == "QC")
                        {
                            _ReworkableJobOrder_Model.JobCompletion = true;
                        }
                        else
                        {
                            _ReworkableJobOrder_Model.JobCompletion = false;
                        }
                        _ReworkableJobOrder_Model.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[3];
                        }
                        if (ViewBag.AppLevel != null && _ReworkableJobOrder_Model.Command != "Edit")
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
                                    _ReworkableJobOrder_Model.BtnName = "Refresh";
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
                                        _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
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
                                        _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";

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
                                    _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
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
                                        _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
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
                                    _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
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
                                    _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A"|| Statuscode == "MRS"|| Statuscode == "PI" || Statuscode == "I" || Statuscode == "PFC")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
                                    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/

                                }
                                else
                                {
                                    _ReworkableJobOrder_Model.BtnName = "Refresh";
                                }
                            }
                            if (Statuscode == "QP"|| Statuscode == "QC")
                            {
                                _ReworkableJobOrder_Model.BtnName = "Refresh";
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }
                    if (_ReworkableJobOrder_Model.BtnName != null)
                    {
                        _ReworkableJobOrder_Model.BtnName = _ReworkableJobOrder_Model.BtnName;
                    }
                    _ReworkableJobOrder_Model.TransType = _ReworkableJobOrder_Model.TransType;
                    _ReworkableJobOrder_Model.Command = _ReworkableJobOrder_Model.Command;
                    _ReworkableJobOrder_Model.TranstypAttach = _ReworkableJobOrder_Model.TransType;
                    if (_ReworkableJobOrder_Model.DocumentStatus == null)
                    {
                        _ReworkableJobOrder_Model.DocumentStatus = "D";
                    }
                    else
                    {
                        _ReworkableJobOrder_Model.DocumentStatus = _ReworkableJobOrder_Model.DocumentStatus;
                    }
                    ViewBag.DocumentStatus = _ReworkableJobOrder_Model.DocumentStatus;
                    //ViewBag.FinstDt = Session["FinStDt"].ToString();

                   
                    

                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ReworkableJobOrder/ReworkableJobOrderDetail.cshtml", _ReworkableJobOrder_Model);
                }
                else
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
                    CommonPageDetails();
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    string ValDigit1 = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit1 = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit1 = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _ReworkableJobOrder_Model1.ValDigit = ValDigit1;
                    _ReworkableJobOrder_Model1.QtyDigit = QtyDigit1;
                    _ReworkableJobOrder_Model1.RateDigit = RateDigit1;
                    ViewBag.ValDigit = ValDigit1;
                    ViewBag.QtyDigit = QtyDigit1;
                    ViewBag.RateDigit = RateDigit1;
                    ViewBag.DocumentStatus = "D";
                    if (_ReworkableJobOrder_Model1.ReworkJO_No == null)
                    {
                        str = GetNewBatchNo();
                        _ReworkableJobOrder_Model1.Newbatch_No = str;
                    }
                   // List<ItemName> _itmList1 = new List<ItemName>();
                   // dt = BindItemsList();
                   // foreach (DataRow dr in dt.Rows)
                   // {
                   //     ItemName _itm = new ItemName();
                   //     _itm.ID = dr["item_id"].ToString();
                   //     _itm.Name = dr["item_name"].ToString();
                   //     _itmList1.Add(_itm);

                   // }
                   //_ReworkableJobOrder_Model1.ItemNamelist = _itmList1;
                   _ReworkableJobOrder_Model1.ItemNamelist = new List<ItemName>{ new ItemName { ID = "0", Name = "---Select---" } } ;

                    //List<wh_namelist> _whList1 = new List<wh_namelist>();
                    dt = Ds_AllDdl.Tables[1];//BindWarehouseList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    wh_namelist _wh = new wh_namelist();
                    //    _wh.WareH_id = dr["wh_id"].ToString();
                    //    _wh.wareH_name = dr["wh_name"].ToString();
                    //    _whList1.Add(_wh);

                    //}
                    //_whList1.Insert(0, new wh_namelist() { WareH_id = "0", wareH_name = "---Select---" });
                    _ReworkableJobOrder_Model1.wh_Namelist = GetWarehouseList(dt); //_whList1;

                    List<ItemName1> _ItemName1 = new List<ItemName1>();
                    _ItemName1.Add(new ItemName1 { Item_Id = "0", Item_Name = "---Select---" });
                    _ReworkableJobOrder_Model1.ItemNameList1 = _ItemName1;

                    List<Shopfloorlist> _shflrList1 = new List<Shopfloorlist>();
                    dt = Ds_AllDdl.Tables[2]; //BindShopfloorList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Shopfloorlist _shflr = new Shopfloorlist();
                        _shflr.shflr_id = dr["shfl_id"].ToString();
                        _shflr.shflr_name = dr["shfl_name"].ToString();
                        _shflrList1.Add(_shflr);

                    }
                    _shflrList1.Insert(0, new Shopfloorlist() { shflr_id = "0", shflr_name = "---Select---" });
                    _ReworkableJobOrder_Model1.ShopfloorNamelist = _shflrList1;

                    List<WorkstationName> _wrklist1 = new List<WorkstationName>();
                    _wrklist1.Add(new WorkstationName { ws_id = "0", ws_name = "---Select---" });
                    _ReworkableJobOrder_Model1.WorkstationNameList = _wrklist1;

                    //List<shift> sh1 = new List<shift>();/*Commented by Suraj Maurya on 25-07-2025*/
                    //shift shObj = new shift();
                    //shObj.id = "0";
                    //shObj.name = "---Select---";
                    //sh1.Add(shObj);
                    //shift shObj1 = new shift();
                    //shObj1.id = "1";
                    //shObj1.name = "Shift-1";
                    //sh1.Add(shObj1);
                    //shift shObj2 = new shift();
                    //shObj2.id = "2";
                    //shObj2.name = "Shift-2";
                    //sh1.Add(shObj2);
                    //shift shObj3 = new shift();
                    //shObj3.id = "3";
                    //shObj3.name = "Shift-3";
                    //sh1.Add(shObj3);
                    _ReworkableJobOrder_Model1.shiftList = GetShiftList();//sh1;

                    if (TempData["ListFilterData"] != null)
                    {
                        _ReworkableJobOrder_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    if (_ReworkableJobOrder_Model1.TransType == "Update" || _ReworkableJobOrder_Model1.Command == "Edit")

                    {
                        string JobCard_NO = RwrkJOCodeURL;
                        string JobCard_Date = RwrkJoDate;
                        DataSet ds = _ReworkableJobOrder_ISERVICES.GetRewrkJODetailEditUpdate(CompID, BrchID, JobCard_NO, JobCard_Date, UserID, DocumentMenuId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            
                            _ReworkableJobOrder_Model1.ReworkJO_No = ds.Tables[0].Rows[0]["job_no"].ToString();
                            _ReworkableJobOrder_Model1.DocNoAttach = ds.Tables[0].Rows[0]["job_no"].ToString();
                            _ReworkableJobOrder_Model1.ReworkJO_Date = ds.Tables[0].Rows[0]["job_date"].ToString();
                            _ReworkableJobOrder_Model1.Item_Name = ds.Tables[0].Rows[0]["item_name"].ToString();
                            _ReworkableJobOrder_Model1.Item_Id = ds.Tables[0].Rows[0]["item_id"].ToString();
                            _ReworkableJobOrder_Model1.ItemNamelist = new List<ItemName>() { new ItemName { ID = _ReworkableJobOrder_Model1.Item_Id, Name = _ReworkableJobOrder_Model1.Item_Name } };
                            _ReworkableJobOrder_Model1.ItemName = ds.Tables[0].Rows[0]["item_id"].ToString();
                            _ReworkableJobOrder_Model1.uom_id = ds.Tables[0].Rows[0]["uom"].ToString();
                            _ReworkableJobOrder_Model1.uom_name = ds.Tables[0].Rows[0]["UomName"].ToString();
                            _ReworkableJobOrder_Model1.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                            _ReworkableJobOrder_Model1.Warehouse = ds.Tables[0].Rows[0]["wh_id"].ToString();
                            _ReworkableJobOrder_Model1.WarehouseID = ds.Tables[0].Rows[0]["wh_id"].ToString();
                          //  _ReworkableJobOrder_Model1.WarehouseName = ds.Tables[0].Rows[0]["wh_name"].ToString();
                            //_whList1.Add(new wh_namelist { WareH_id = _ReworkableJobOrder_Model1.WarehouseID, wareH_name = _ReworkableJobOrder_Model1.WarehouseName });
                            //_ReworkableJobOrder_Model1.wh_Namelist = _whList1;

                            _ReworkableJobOrder_Model1.Available_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["AvlstockQty"]).ToString(QtyDigit1);
                            _ReworkableJobOrder_Model1.Rework_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["rwk_qty"]).ToString(QtyDigit1);
                            _ReworkableJobOrder_Model1.Shopfloor = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                            _ReworkableJobOrder_Model1.Shopfloor_Id = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                            //_ReworkableJobOrder_Model1.Shopfloor_Name = ds.Tables[0].Rows[0]["scjob_no"].ToString();
                            _ReworkableJobOrder_Model1.ddl_Workstation = ds.Tables[0].Rows[0]["ws_id"].ToString();
                            _ReworkableJobOrder_Model1.WorkstationID = ds.Tables[0].Rows[0]["ws_id"].ToString();
                            _ReworkableJobOrder_Model1.WorkstationName = ds.Tables[0].Rows[0]["ws_name"].ToString();
                            _wrklist1.Add(new WorkstationName { ws_id = _ReworkableJobOrder_Model1.WorkstationID, ws_name = _ReworkableJobOrder_Model1.WorkstationName });
                            _ReworkableJobOrder_Model1.WorkstationNameList = _wrklist1;

                            _ReworkableJobOrder_Model1.supervisor_name = ds.Tables[0].Rows[0]["supervisor_name"].ToString();
                            _ReworkableJobOrder_Model1.ddl_shift = ds.Tables[0].Rows[0]["shift_id"].ToString();
                            _ReworkableJobOrder_Model1.Newbatch_No = ds.Tables[0].Rows[0]["batch_no"].ToString();
                            _ReworkableJobOrder_Model1.NewExpiryDate = ds.Tables[0].Rows[0]["exp_date"].ToString();
                            //_ReworkableJobOrder_Model1.ddl_shiftName = ds.Tables[0].Rows[0]["scjob_no"].ToString();


                            _ReworkableJobOrder_Model1.Created_by = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            _ReworkableJobOrder_Model1.Created_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                            _ReworkableJobOrder_Model1.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();

                            _ReworkableJobOrder_Model1.Amended_by = ds.Tables[0].Rows[0]["AmendedBy"].ToString();
                            _ReworkableJobOrder_Model1.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                            _ReworkableJobOrder_Model1.Approved_by = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                            _ReworkableJobOrder_Model1.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                            _ReworkableJobOrder_Model1.StatusName = ds.Tables[0].Rows[0]["RJOStatus"].ToString();
                            _ReworkableJobOrder_Model1.Status_Code = ds.Tables[0].Rows[0]["job_status"].ToString().Trim();
                            _ReworkableJobOrder_Model1.src_type_WarehouseName = ds.Tables[0].Rows[0]["src_type"].ToString().Trim();
                            _ReworkableJobOrder_Model1.NewExpiryDate_Flag = ds.Tables[0].Rows[0]["i_exp"].ToString().Trim();
                            _ReworkableJobOrder_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                            _ReworkableJobOrder_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);//Cancelled

                            ViewBag.ReqMaterialDetailsList = ds.Tables[1];
                            ViewBag.ItemReworkQtyDetail = ds.Tables[2];
                            ViewBag.AttechmentDetails = ds.Tables[7];
                            if (_ReworkableJobOrder_Model1.Status_Code == "QP" || _ReworkableJobOrder_Model1.Status_Code == "QC")
                            {
                                ViewBag.ConsumeMaterialDetails = ds.Tables[8];
                                ViewBag.ItemStockBatchWise = ds.Tables[9];
                                ViewBag.ItemStockSerialWise = ds.Tables[10];
                                ViewBag.MaterialOutputDetails = ds.Tables[11];
                            }
                            ViewBag.SubItemDetails = ds.Tables[12];
                            //if (_ReworkableJobOrder_Model1.Status_Code == "QC")
                            //{
                            //    ViewBag.MaterialOutputDetails = ds.Tables[11];
                            //}
                            ViewBag.QtyDigit = QtyDigit1;
                        }
                        var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                        var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string Statuscode = ds.Tables[0].Rows[0]["job_status"].ToString().Trim();
                        if (Statuscode == "C")
                        {
                            _ReworkableJobOrder_Model1.Cancelled = true;
                        }
                        else
                        {
                            _ReworkableJobOrder_Model1.Cancelled = false;
                        }
                        if (Statuscode == "QP"||Statuscode == "QC")
                        {
                            _ReworkableJobOrder_Model1.JobCompletion = true;
                        }
                        else
                        {
                            _ReworkableJobOrder_Model1.JobCompletion = false;
                        }
                        _ReworkableJobOrder_Model1.DocumentStatus = Statuscode;
                        if (Statuscode != "D" && Statuscode != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[5];
                        }
                        if (ViewBag.AppLevel != null && _ReworkableJobOrder_Model1.Command != "Edit")
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
                                    _ReworkableJobOrder_Model1.BtnName = "Refresh";
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
                                        _ReworkableJobOrder_Model1.BtnName = "BtnToDetailPage";
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
                                        _ReworkableJobOrder_Model1.BtnName = "BtnToDetailPage";

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
                                    _ReworkableJobOrder_Model1.BtnName = "BtnToDetailPage";
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
                                        _ReworkableJobOrder_Model1.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (Statuscode == "F")
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
                                    _ReworkableJobOrder_Model1.BtnName = "BtnToDetailPage";
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
                                    _ReworkableJobOrder_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (Statuscode == "A"|| Statuscode == "MRS"|| Statuscode == "PI"|| Statuscode == "I" || Statuscode == "PFC")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _ReworkableJobOrder_Model1.BtnName = "BtnToDetailPage";
                                    /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                }
                                else
                                {
                                    _ReworkableJobOrder_Model1.BtnName = "Refresh";
                                }
                            }
                            if (Statuscode == "QP" || Statuscode == "QC")
                            {
                                _ReworkableJobOrder_Model1.BtnName = "Refresh";
                            }
                        }

                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }

                    }


                    var RwrkJOCode = "";
                    if (RwrkJOCodeURL != null)
                    {
                        RwrkJOCode = RwrkJOCodeURL;
                        _ReworkableJobOrder_Model1.ReworkJO_No = RwrkJOCodeURL;
                    }
                    else
                    {
                        RwrkJOCode = _ReworkableJobOrder_Model1.ReworkJO_No;
                    }
                    if (TransType != null)
                    {
                        _ReworkableJobOrder_Model1.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _ReworkableJobOrder_Model1.Command = command;
                    }

                    if (_ReworkableJobOrder_Model1.BtnName == null && _ReworkableJobOrder_Model1.Command == null)
                    {
                        _ReworkableJobOrder_Model1.BtnName = "AddNew";
                        _ReworkableJobOrder_Model1.Command = "Add";
                        _ReworkableJobOrder_Model1.AppStatus = "D";
                        ViewBag.DocumentStatus = _ReworkableJobOrder_Model1.AppStatus;
                        _ReworkableJobOrder_Model1.TransType = "Save";
                        _ReworkableJobOrder_Model1.BtnName = "BtnAddNew";

                    }

                    if (_ReworkableJobOrder_Model1.BtnName != null)
                    {
                        _ReworkableJobOrder_Model1.BtnName = _ReworkableJobOrder_Model1.BtnName;
                    }
                    if (_ReworkableJobOrder_Model1.TransType == null || _ReworkableJobOrder_Model1.BtnName == null)
                    {
                        _ReworkableJobOrder_Model1.TransType = "Update";
                        _ReworkableJobOrder_Model1.BtnName = "BtnEdit";

                    }
                    _ReworkableJobOrder_Model1.TransType = _ReworkableJobOrder_Model1.TransType;
                    _ReworkableJobOrder_Model1.TranstypAttach = _ReworkableJobOrder_Model1.TransType;
                    if (_ReworkableJobOrder_Model1.DocumentStatus != null)
                    {
                        _ReworkableJobOrder_Model1.DocumentStatus = _ReworkableJobOrder_Model1.DocumentStatus;
                        ViewBag.DocumentStatus = _ReworkableJobOrder_Model1.DocumentStatus;
                    }

                    _ReworkableJobOrder_Model1.Title = title;
                    
                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/ReworkableJobOrder/ReworkableJobOrderDetail.cshtml", _ReworkableJobOrder_Model1);


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
        public ActionResult ReworkableJobOrderBtnCommand(ReworkableJobOrder_Model _ReworkableJobOrder_Model, string command)
        {
            try
            {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_ReworkableJobOrder_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        //_ReworkableJobOrder_Model = new ReworkableJobOrder_Model();
                        //_ReworkableJobOrder_Model.Message = "New";
                        //_ReworkableJobOrder_Model.Command = "Add";
                        //_ReworkableJobOrder_Model.AppStatus = "D";
                        //_ReworkableJobOrder_Model.DocumentStatus = "D";
                        //ViewBag.DocumentStatus = _ReworkableJobOrder_Model.DocumentStatus;
                        //_ReworkableJobOrder_Model.TransType = "Save";
                        //_ReworkableJobOrder_Model.BtnName = "BtnAddNew";
                        //TempData["ModelData"] = _ReworkableJobOrder_Model;
                        ReworkableJobOrder_Model _ReworkableJobOrder_ModelAdd = new ReworkableJobOrder_Model();
                        _ReworkableJobOrder_ModelAdd.Message = "New";
                        _ReworkableJobOrder_ModelAdd.Command = "Add";
                        _ReworkableJobOrder_ModelAdd.AppStatus = "D";
                        _ReworkableJobOrder_ModelAdd.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _ReworkableJobOrder_ModelAdd.DocumentStatus;
                        _ReworkableJobOrder_ModelAdd.TransType = "Save";
                        _ReworkableJobOrder_ModelAdd.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _ReworkableJobOrder_ModelAdd;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_ReworkableJobOrder_Model.ReworkJO_No))
                                return RedirectToAction("RJODoubleClickFromList", new { DocNo = _ReworkableJobOrder_Model.ReworkJO_No, DocDate = _ReworkableJobOrder_Model.ReworkJO_Date, ListFilterData = _ReworkableJobOrder_Model.ListFilterData1, WF_Status = _ReworkableJobOrder_Model.WFStatus });
                            else
                                _ReworkableJobOrder_ModelAdd.Command = "Refresh";
                            _ReworkableJobOrder_ModelAdd.TransType = "Refresh";
                            _ReworkableJobOrder_ModelAdd.BtnName = "Refresh";
                            _ReworkableJobOrder_ModelAdd.DocumentStatus = null;
                            TempData["ModelData"] = _ReworkableJobOrder_ModelAdd;
                            return RedirectToAction("ReworkableJobOrderDetail", "ReworkableJobOrder", _ReworkableJobOrder_ModelAdd);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("ReworkableJobOrderDetail", "ReworkableJobOrder");
                   
                      case "Edit":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("RJODoubleClickFromList", new { DocNo = _ReworkableJobOrder_Model.ReworkJO_No, DocDate = _ReworkableJobOrder_Model.ReworkJO_Date, ListFilterData = _ReworkableJobOrder_Model.ListFilterData1, WF_Status = _ReworkableJobOrder_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string RwkDt = _ReworkableJobOrder_Model.ReworkJO_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, RwkDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("RJODoubleClickFromList", new { DocNo = _ReworkableJobOrder_Model.ReworkJO_No, DocDate = _ReworkableJobOrder_Model.ReworkJO_Date, ListFilterData = _ReworkableJobOrder_Model.ListFilterData1, WF_Status = _ReworkableJobOrder_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        if (_ReworkableJobOrder_Model.Status_Code == "MRS")
                        {
                            if (CheckRewrkJobOrderAgainstMRS(_ReworkableJobOrder_Model) == "Used")
                            {
                                _ReworkableJobOrder_Model.Message = "Used";
                                _ReworkableJobOrder_Model.TransType = "Update";
                                _ReworkableJobOrder_Model.Command = "Add";
                                _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
                                TempData["ModelData"] = _ReworkableJobOrder_Model;
                            }

                            else
                            {
                                _ReworkableJobOrder_Model.TransType = "Update";
                                _ReworkableJobOrder_Model.Command = command;
                                _ReworkableJobOrder_Model.BtnName = "BtnEdit";
                                _ReworkableJobOrder_Model.Message = "New";
                                _ReworkableJobOrder_Model.AppStatus = "D";
                                _ReworkableJobOrder_Model.DocumentStatus = "D";
                                ViewBag.DocumentStatus = _ReworkableJobOrder_Model.AppStatus;
                                TempData["ModelData"] = _ReworkableJobOrder_Model;
                            }
                        }
                        else
                        {
                            _ReworkableJobOrder_Model.TransType = "Update";
                            _ReworkableJobOrder_Model.Command = command;
                            _ReworkableJobOrder_Model.BtnName = "BtnEdit";
                            _ReworkableJobOrder_Model.Message = "New";
                            _ReworkableJobOrder_Model.AppStatus = "D";
                            _ReworkableJobOrder_Model.DocumentStatus = "D";
                            ViewBag.DocumentStatus = _ReworkableJobOrder_Model.AppStatus;
                        }
                        //_ReworkableJobOrder_Model.TransType = "Update";
                        //_ReworkableJobOrder_Model.Command = command;
                        //_ReworkableJobOrder_Model.BtnName = "BtnEdit";
                        //_ReworkableJobOrder_Model.Message = "New";
                        //_ReworkableJobOrder_Model.AppStatus = "D";
                        //_ReworkableJobOrder_Model.DocumentStatus = "D";
                        //ViewBag.DocumentStatus = _ReworkableJobOrder_Model.AppStatus;

                        var TransType = "Update";
                        var BtnName = "BtnEdit";
                        var RwrkJOCodeURL = _ReworkableJobOrder_Model.ReworkJO_No;
                        var RwrkJoDate = _ReworkableJobOrder_Model.ReworkJO_Date;
                        command = _ReworkableJobOrder_Model.Command;
                        TempData["ModelData"] = _ReworkableJobOrder_Model;
                        TempData["ListFilterData"] = _ReworkableJobOrder_Model.ListFilterData1;
                        return (RedirectToAction("ReworkableJobOrderDetail", new { RwrkJOCodeURL = RwrkJOCodeURL, RwrkJoDate, TransType, BtnName, command }));

                    case "Delete":
                        _ReworkableJobOrder_Model.Command = command;
                        _ReworkableJobOrder_Model.BtnName = "Refresh";
                        DeleteRJODetails(_ReworkableJobOrder_Model, command);
                        TempData["ListFilterData"] = _ReworkableJobOrder_Model.ListFilterData1;
                        return RedirectToAction("ReworkableJobOrderDetail");

                    case "Save":
                        _ReworkableJobOrder_Model.Command = command;

                        SaveRewrkJODetail(_ReworkableJobOrder_Model);
                        if (_ReworkableJobOrder_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        RwrkJOCodeURL = _ReworkableJobOrder_Model.ReworkJO_No;
                        RwrkJoDate = _ReworkableJobOrder_Model.ReworkJO_Date;
                        TransType = _ReworkableJobOrder_Model.TransType;
                        BtnName = _ReworkableJobOrder_Model.BtnName;
                        TempData["ModelData"] = _ReworkableJobOrder_Model;
                        TempData["ListFilterData"] = _ReworkableJobOrder_Model.ListFilterData1;
                        return (RedirectToAction("ReworkableJobOrderDetail", new { RwrkJOCodeURL = RwrkJOCodeURL, RwrkJoDate, TransType, BtnName, command }));
                    case "Forward":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("RJODoubleClickFromList", new { DocNo = _ReworkableJobOrder_Model.ReworkJO_No, DocDate = _ReworkableJobOrder_Model.ReworkJO_Date, ListFilterData = _ReworkableJobOrder_Model.ListFilterData1, WF_Status = _ReworkableJobOrder_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string RwkDt1 = _ReworkableJobOrder_Model.ReworkJO_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, RwkDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("RJODoubleClickFromList", new { DocNo = _ReworkableJobOrder_Model.ReworkJO_No, DocDate = _ReworkableJobOrder_Model.ReworkJO_Date, ListFilterData = _ReworkableJobOrder_Model.ListFilterData1, WF_Status = _ReworkableJobOrder_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("RJODoubleClickFromList", new { DocNo = _ReworkableJobOrder_Model.ReworkJO_No, DocDate = _ReworkableJobOrder_Model.ReworkJO_Date, ListFilterData = _ReworkableJobOrder_Model.ListFilterData1, WF_Status = _ReworkableJobOrder_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string RwkDt2 = _ReworkableJobOrder_Model.ReworkJO_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, RwkDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("RJODoubleClickFromList", new { DocNo = _ReworkableJobOrder_Model.ReworkJO_No, DocDate = _ReworkableJobOrder_Model.ReworkJO_Date, ListFilterData = _ReworkableJobOrder_Model.ListFilterData1, WF_Status = _ReworkableJobOrder_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _ReworkableJobOrder_Model.Command = command;
                        RJOApprove(_ReworkableJobOrder_Model, "");

                        RwrkJOCodeURL = _ReworkableJobOrder_Model.ReworkJO_No;
                        RwrkJoDate = _ReworkableJobOrder_Model.ReworkJO_Date;
                        TransType = _ReworkableJobOrder_Model.TransType;
                        BtnName = _ReworkableJobOrder_Model.BtnName;
                        TempData["ModelData"] = _ReworkableJobOrder_Model;
                        TempData["ListFilterData"] = _ReworkableJobOrder_Model.ListFilterData1;
                        return (RedirectToAction("ReworkableJobOrderDetail", new { RwrkJOCodeURL = RwrkJOCodeURL, RwrkJoDate, TransType, BtnName, command }));

                    case "Refresh":
                        ReworkableJobOrder_Model _ReworkableJobOrder_ModelRefresh = new ReworkableJobOrder_Model();
                       _ReworkableJobOrder_Model.Message = null;
                        _ReworkableJobOrder_ModelRefresh.Command = command;
                        _ReworkableJobOrder_ModelRefresh.TransType = "Refresh";
                        _ReworkableJobOrder_ModelRefresh.BtnName = "Refresh";
                        _ReworkableJobOrder_ModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = _ReworkableJobOrder_ModelRefresh;
                        TempData["ListFilterData"] = _ReworkableJobOrder_Model.ListFilterData1;
                        return RedirectToAction("ReworkableJobOrderDetail");

                    case "Print":
                    //return GenratePdfFile(_ReworkableJobOrder_Model);
                    case "BacktoList":
                        TempData["ListFilterData"] = _ReworkableJobOrder_Model.ListFilterData1;
                        return RedirectToAction("ReworkableJobOrder", "ReworkableJobOrder");

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
        public ActionResult SaveRewrkJODetail(ReworkableJobOrder_Model _ReworkableJobOrder_Model)
        {
            string SaveMessage = "";
            /*getDocumentName();*/ /* To set Title*/
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_ReworkableJobOrder_Model.Cancelled == false)
                {
                    _ReworkableJobOrder_Model.DocumentMenuId = _ReworkableJobOrder_Model.DocumentMenuId;


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
                    DataTable DtblReqMatrlDetail = new DataTable();
                    DataTable ReworkQtyItemDetails = new DataTable();
                    DataTable DtblAttchDetail = new DataTable();
                    DataTable DtblConsumeMatrlDetail = new DataTable();
                    DataTable CMItemBatchDetails = new DataTable();
                    DataTable CMItemSerialDetails = new DataTable();
                    DataTable dtheader = new DataTable();

                    DtblHDetail = ToDtblHDetail(_ReworkableJobOrder_Model);
                    DtblReqMatrlDetail = ToDtblRequiredMaterialDetail(_ReworkableJobOrder_Model.MaterialRequireddetails);
                    DtblConsumeMatrlDetail = ToDtblConsumeMatrlDetail(_ReworkableJobOrder_Model.ConsumeMaterialdetails);

                    DataTable RewrkQty_detail = new DataTable();
                    RewrkQty_detail.Columns.Add("item_id", typeof(string));
                    //RewrkQty_detail.Columns.Add("uom_id", typeof(int));
                    //RewrkQty_detail.Columns.Add("wh_id", typeof(int));
                    RewrkQty_detail.Columns.Add("lot_no", typeof(string));
                    RewrkQty_detail.Columns.Add("batch_no", typeof(string));
                    RewrkQty_detail.Columns.Add("serial_no", typeof(string));
                    RewrkQty_detail.Columns.Add("expiry_date", typeof(string));
                    //RewrkQty_detail.Columns.Add("avl_stk_qty", typeof(string));
                    RewrkQty_detail.Columns.Add("rework_qty", typeof(string));
                    if (_ReworkableJobOrder_Model.ItemReworkQtyDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_ReworkableJobOrder_Model.ItemReworkQtyDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowDetailsLines = RewrkQty_detail.NewRow();
                            dtrowDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                            //dtrowDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();
                            //dtrowDetailsLines["wh_id"] = jObjectBatch[i]["WhId"].ToString();
                            dtrowDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                            dtrowDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                            dtrowDetailsLines["serial_no"] = jObjectBatch[i]["SerialNo"].ToString();
                            if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null)
                            {
                                dtrowDetailsLines["expiry_date"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                            }
                            //dtrowDetailsLines["avl_stk_qty"] = jObjectBatch[i]["AvlStockQty"].ToString();
                            dtrowDetailsLines["rework_qty"] = jObjectBatch[i]["ReworkQty"].ToString();
                            RewrkQty_detail.Rows.Add(dtrowDetailsLines);
                        }
                    }
                    ReworkQtyItemDetails = RewrkQty_detail;

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("Comp_Id", typeof(int));
                    Batch_detail.Columns.Add("Br_Id", typeof(int));
                    Batch_detail.Columns.Add("matrlTyp_Id", typeof(string));
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    //Batch_detail.Columns.Add("uom_id", typeof(int));
                    //Batch_detail.Columns.Add("avl_batch_qty", typeof(float));
                    Batch_detail.Columns.Add("expiry_date", typeof(string));
                    Batch_detail.Columns.Add("consume_qty", typeof(float));
                    if (_ReworkableJobOrder_Model.CMItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_ReworkableJobOrder_Model.CMItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["Comp_Id"] = CompID; 
                            dtrowBatchDetailsLines["Br_Id"] = BrchID;
                           dtrowBatchDetailsLines["matrlTyp_Id"] = jObjectBatch[i]["MaterialtypID"].ToString();
                            dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                            //dtrowBatchDetailsLines["uom_id"] = jObjectBatch[i]["UOMId"].ToString();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                            dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                            //dtrowBatchDetailsLines["avl_batch_qty"] = jObjectBatch[i]["BatchAvlStock"].ToString();
                            if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null)
                            {
                                dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                            }

                            dtrowBatchDetailsLines["consume_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                    }
                    CMItemBatchDetails = Batch_detail;

                    DataTable Serial_detail = new DataTable();
                    
                    Serial_detail.Columns.Add("Comp_Id", typeof(int));
                    Serial_detail.Columns.Add("Br_Id", typeof(int));
                    Serial_detail.Columns.Add("matrlTyp_Id", typeof(string));
                    Serial_detail.Columns.Add("item_id", typeof(string));
                    //Serial_detail.Columns.Add("uom_id", typeof(int));
                    Serial_detail.Columns.Add("lot_no", typeof(string));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("consume_qty", typeof(float));

                    if (_ReworkableJobOrder_Model.CMItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_ReworkableJobOrder_Model.CMItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["Comp_Id"] = CompID; 
                            dtrowSerialDetailsLines["Br_Id"] = BrchID;
                            dtrowSerialDetailsLines["matrlTyp_Id"] = jObjectSerial[i]["MaterialtypID"].ToString();
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                            //dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            dtrowSerialDetailsLines["consume_qty"] = jObjectSerial[i]["ConsumedQty"].ToString();
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                    }
                    CMItemSerialDetails = Serial_detail;
                    var  hdnJobCmplted = "";
                    if (_ReworkableJobOrder_Model.JobCompletion == true)
                    {
                         hdnJobCmplted = _ReworkableJobOrder_Model.hdnJobCompletion;
                    }
                    else
                    {
                         hdnJobCmplted = "";
                    }

                        var _JobOrderDetailsattch = TempData["ModelDataattch"] as RJODetailsattch;
                    TempData["ModelDataattch"] = null;
                    DataTable dtAttachment = new DataTable();
                    if (_ReworkableJobOrder_Model.attatchmentdetail != null)
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
                            if (_ReworkableJobOrder_Model.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _ReworkableJobOrder_Model.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(_ReworkableJobOrder_Model.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(_ReworkableJobOrder_Model.ReworkJO_No))
                                {
                                    dtrowAttachment1["id"] = _ReworkableJobOrder_Model.ReworkJO_No;
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

                        if (_ReworkableJobOrder_Model.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_ReworkableJobOrder_Model.ReworkJO_No))
                                {
                                    ItmCode = _ReworkableJobOrder_Model.ReworkJO_No;
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

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    dtSubItem.Columns.Add("Type", typeof(string));
                    dtSubItem.Columns.Add("Req_qty", typeof(string));
                    if (_ReworkableJobOrder_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_ReworkableJobOrder_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["Type"] = jObject2[i]["subItemTyp"].ToString();
                            dtrowItemdetails["Req_qty"] = jObject2[i]["req_qty"].ToString();
                            //dtrowItemdetails["avl_stock"] = jObject2[i]["avl_qty"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                    }

                    /*------------------Sub Item end----------------------*/

                    SaveMessage = _ReworkableJobOrder_ISERVICES.InsertReworkJO_Details(DtblHDetail, DtblReqMatrlDetail, ReworkQtyItemDetails, DtblAttchDetail, DtblConsumeMatrlDetail, CMItemBatchDetails, CMItemSerialDetails, hdnJobCmplted, dtSubItem);

                    string[] Data = SaveMessage.Split(',');

                    string RewrkNo = Data[1];
                    string Rewrk_No = RewrkNo.Replace("/", "");
                    string Message = Data[0];
                    if (Message == "Data_Not_Found")
                    {
                        //var a = SaveMessage.Split(',');
                        var msgs = Message.Replace("_", " ") + " " + Rewrk_No + " in " + PageName;//ProdOrdCode is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msgs, "", "");
                        _ReworkableJobOrder_Model.Message = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("ReworkableJobOrderDetail");
                    }
                    string RewrkDate = Data[2];
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
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, RewrkNo, _ReworkableJobOrder_Model.TransType, dtAttachment);

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
                        //                string RewrkNo1 = RewrkNo.Replace("/", "");
                        //                string img_nm = CompID + BrchID + RewrkNo1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                        _ReworkableJobOrder_Model.Message = "Save";

                        _ReworkableJobOrder_Model.ReworkJO_No = RewrkNo;
                        _ReworkableJobOrder_Model.ReworkJO_Date = RewrkDate;
                        _ReworkableJobOrder_Model.TransType = "Update";
                        _ReworkableJobOrder_Model.Command = "Update";
                        if(_ReworkableJobOrder_Model.JobCompletion == true)
                        {
                            if(StatusCode== "StockNotAvailable")
                            {
                                _ReworkableJobOrder_Model.Message = "StockNotAvail";
                                _ReworkableJobOrder_Model.BtnName = "BtnRefresh";
                            }
                            else
                            {
                                _ReworkableJobOrder_Model.AppStatus = "JC";
                                _ReworkableJobOrder_Model.DocumentStatus = "JC";
                                _ReworkableJobOrder_Model.BtnName = "BtnSave";
                            }
                           
                        }
                        else
                        {
                            _ReworkableJobOrder_Model.AppStatus = "D";
                            _ReworkableJobOrder_Model.DocumentStatus = "D";
                            _ReworkableJobOrder_Model.BtnName = "BtnSave";
                        }
                        //_ReworkableJobOrder_Model.BtnName = "BtnSave";
                        TempData["ModelData"] = _ReworkableJobOrder_Model;
                        return RedirectToAction("ReworkableJobOrderDetail");

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
                    _ReworkableJobOrder_Model.Created_by = UserID;
                    string br_id = Session["BranchId"].ToString();
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet SaveMessage1 = _ReworkableJobOrder_ISERVICES.RewrkJOCancel(_ReworkableJobOrder_Model, CompID, br_id, mac_id);
                    var Result = SaveMessage1.Tables[0].Rows[0]["result"].ToString();
                    if (Result == "Used")
                    {
                        _ReworkableJobOrder_Model.Message = "Used";
                        _ReworkableJobOrder_Model.TransType = "Update";
                        _ReworkableJobOrder_Model.Command = "Add";
                        _ReworkableJobOrder_Model.BtnName = "Refresh";
                        TempData["ModelData"] = _ReworkableJobOrder_Model;
                    }
                    else
                    {
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _ReworkableJobOrder_Model.ReworkJO_No, "C", UserID, "0");
                        _ReworkableJobOrder_Model.Message = "Cancelled";
                        _ReworkableJobOrder_Model.Command = "Update";
                        _ReworkableJobOrder_Model.ReworkJO_No = _ReworkableJobOrder_Model.ReworkJO_No;
                        _ReworkableJobOrder_Model.ReworkJO_Date = _ReworkableJobOrder_Model.ReworkJO_Date;
                        _ReworkableJobOrder_Model.TransType = "Update";
                        _ReworkableJobOrder_Model.AppStatus = "D";
                        _ReworkableJobOrder_Model.BtnName = "Refresh";
                    }
                    
                    TempData["ModelData"] = _ReworkableJobOrder_Model;
                    return RedirectToAction("ReworkableJobOrderDetail");
                }
                return RedirectToAction("ReworkableJobOrderDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_ReworkableJobOrder_Model.TransType == "Save")
                    {
                        string Guid = "";
                        if (_ReworkableJobOrder_Model.Guid != null)
                        {
                            Guid = _ReworkableJobOrder_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private DataTable ToDtblHDetail(ReworkableJobOrder_Model _ReworkableJobOrder_Model)
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
                //dtheader.Columns.Add("Cancelled", typeof(string));
                dtheader.Columns.Add("RJO_No", typeof(string));
                dtheader.Columns.Add("RJO_Date", typeof(string));
                dtheader.Columns.Add("Item_ID", typeof(string));
                dtheader.Columns.Add("Uom_Id", typeof(string));
                dtheader.Columns.Add("Wh_Id", typeof(string));
                //dtheader.Columns.Add("AvailableQty", typeof(string));
                dtheader.Columns.Add("ReworkQty", typeof(string));
                dtheader.Columns.Add("Shfl_Id", typeof(string));
                dtheader.Columns.Add("WrkStation_Id", typeof(string));
                dtheader.Columns.Add("Supervisor_Name", typeof(string));
                dtheader.Columns.Add("Shift_Id", typeof(string));
                dtheader.Columns.Add("NewBatchNo", typeof(string));
                dtheader.Columns.Add("NewExpdate", typeof(string));

                dtheader.Columns.Add("CompID", typeof(string));
                dtheader.Columns.Add("BranchID", typeof(string));
                dtheader.Columns.Add("UserID", typeof(int));
                dtheader.Columns.Add("RJO_Status", typeof(string));
                dtheader.Columns.Add("SystemDetail", typeof(string));
                dtheader.Columns.Add("src_type", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _ReworkableJobOrder_Model.TransType;
                dtrowHeader["RJO_No"] = _ReworkableJobOrder_Model.ReworkJO_No;
                dtrowHeader["RJO_Date"] = _ReworkableJobOrder_Model.ReworkJO_Date;
                dtrowHeader["Item_ID"] = _ReworkableJobOrder_Model.Item_Id;
                dtrowHeader["Uom_Id"] = _ReworkableJobOrder_Model.uom_id;
                dtrowHeader["Wh_Id"] = _ReworkableJobOrder_Model.WarehouseID;
                //dtrowHeader["AvailableQty"] = _ReworkableJobOrder_Model.Available_Qty;
                dtrowHeader["ReworkQty"] = _ReworkableJobOrder_Model.Rework_Qty;
                dtrowHeader["Shfl_Id"] = _ReworkableJobOrder_Model.Shopfloor_Id;
                dtrowHeader["WrkStation_Id"] = _ReworkableJobOrder_Model.WorkstationID;
                dtrowHeader["Supervisor_Name"] = _ReworkableJobOrder_Model.supervisor_name;
                dtrowHeader["Shift_Id"] = _ReworkableJobOrder_Model.ddl_shift;
                dtrowHeader["NewBatchNo"] = _ReworkableJobOrder_Model.Newbatch_No;
                dtrowHeader["NewExpdate"] = _ReworkableJobOrder_Model.NewExpiryDate;

                dtrowHeader["CompID"] = CompID;
                dtrowHeader["BranchID"] = BrchID;
                dtrowHeader["UserID"] = UserID;
                dtrowHeader["RJO_Status"] = IsNull(_ReworkableJobOrder_Model.Status_Code, "D");

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["SystemDetail"] = mac_id;
                dtrowHeader["src_type"] = _ReworkableJobOrder_Model.src_type_WarehouseName;
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
        private DataTable ToDtblRequiredMaterialDetail(string ReqMterlDetail)
        {
            try
            {
                DataTable DtblReqMtr_Detail = new DataTable();
                DataTable dtItem = new DataTable();
               
                dtItem.Columns.Add("RMTypeID", typeof(string));
                dtItem.Columns.Add("RM_ItemID", typeof(string));
                dtItem.Columns.Add("RM_UOMID", typeof(string));
                dtItem.Columns.Add("RM_ReqQty", typeof(string));
                


                if (ReqMterlDetail != null)
                {
                    JArray jObject = JArray.Parse(ReqMterlDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["RMTypeID"] = jObject[i]["RMItemTyp"].ToString();
                        dtrowLines["RM_ItemID"] = jObject[i]["RMItemID"].ToString();
                        dtrowLines["RM_UOMID"] = jObject[i]["RMUOMID"].ToString();
                        dtrowLines["RM_ReqQty"] = jObject[i]["RMReq_Qty"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                }

                DtblReqMtr_Detail = dtItem;
                return DtblReqMtr_Detail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private DataTable ToDtblConsumeMatrlDetail(string ConsMterlDetail)
        {
            try
            {
                DataTable DtblConsMtr_Detail = new DataTable();
                DataTable dtcons = new DataTable();
                dtcons.Columns.Add("CM_CompId", typeof(int));
                dtcons.Columns.Add("CM_BrId", typeof(int));
                dtcons.Columns.Add("CM_RJO", typeof(string));
                dtcons.Columns.Add("CM_RJODate", typeof(string));
                dtcons.Columns.Add("CMTypeID", typeof(string));
                dtcons.Columns.Add("CM_ItemID", typeof(string));
                dtcons.Columns.Add("CM_UOMID", typeof(string));
                dtcons.Columns.Add("CM_ReqQty", typeof(string));
                dtcons.Columns.Add("CM_ConsumeQty", typeof(string));



                if (ConsMterlDetail != null)
                {
                    JArray jObject = JArray.Parse(ConsMterlDetail);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtcons.NewRow();
                        dtrowLines["CM_CompId"] = CompID;
                        dtrowLines["CM_BrId"] = BrchID;
                        dtrowLines["CM_RJO"] = jObject[i]["CMRJONo"].ToString();
                        dtrowLines["CM_RJODate"] = jObject[i]["CMRJODate"].ToString();
                        dtrowLines["CMTypeID"] = jObject[i]["CMItemTyp"].ToString();
                        dtrowLines["CM_ItemID"] = jObject[i]["CMItemID"].ToString();
                        dtrowLines["CM_UOMID"] = jObject[i]["CMUOMID"].ToString();
                        dtrowLines["CM_ReqQty"] = jObject[i]["CMReq_Qty"].ToString();
                        dtrowLines["CM_ConsumeQty"] = jObject[i]["CMCons_Qty"].ToString();
                        dtcons.Rows.Add(dtrowLines);
                    }
                }

                DtblConsMtr_Detail = dtcons;
                return DtblConsMtr_Detail;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }


        }
        private List<RJOList> getRewrkJobOrderList(RJOListModel _RJOListModel)
        {
            _RJOList = new List<RJOList>();
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
               
                if (_RJOListModel.WF_Status != null)
                {
                    wfstatus = _RJOListModel.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }

                DataSet DSet = _ReworkableJobOrder_ISERVICES.GetRJOListandSrchDetail(CompID, BrchID, _RJOListModel, UserID, wfstatus, DocumentMenuId);

                if (DSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DSet.Tables[0].Rows)
                    {
                        RJOList _RJO_List = new RJOList();
                       _RJO_List.RJONumber = dr["RwkJobNo"].ToString();
                        _RJO_List.RJODate = dr["RwkJobDate"].ToString();
                        _RJO_List.RJO_Dt = dr["RwkJobDt"].ToString();
                        _RJO_List.ItemName = dr["ItemName"].ToString();
                        _RJO_List.ItemID = dr["item_id"].ToString();
                        _RJO_List.Uom = dr["UomName"].ToString();
                        _RJO_List.RewrkQty = dr["rwk_qty"].ToString();
                        _RJO_List.RJO_Status = dr["RwkJobStatus"].ToString();
                        _RJO_List.CreatedBy = dr["create_by"].ToString();
                        _RJO_List.CreatedON = dr["CreateDate"].ToString();
                        _RJO_List.ApprovedBy = dr["app_by"].ToString();
                        _RJO_List.ApprovedOn = dr["ApproveDate"].ToString();
                        _RJO_List.AmendedBy = dr["mod_by"].ToString();
                        _RJO_List.AmendedOn = dr["AmendedDate"].ToString();
                        _RJO_List.FinStDt = DSet.Tables[2].Rows[0]["findate"].ToString();

                        _RJOList.Add(_RJO_List);
                    }
                }

                //Session["FinStDt"] = DSet.Tables[2].Rows[0]["findate"];

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;

            }
            return _RJOList;
        }

        [HttpPost]
        public ActionResult SearchRewrkJOListDetail(string ItemId, string Fromdate, string Todate, string Status)
        {
            RJOListModel _RJOListModel = new RJOListModel();
            try
            {
                _RJOListModel.WF_Status = null;
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
                _RJOList = new List<RJOList>();
                _RJOListModel.ItemID = ItemId;

                _RJOListModel.FromDate = Fromdate;
                _RJOListModel.ToDate = Todate;
                _RJOListModel.Status = Status;
                DataSet DSet = _ReworkableJobOrder_ISERVICES.GetRJOListandSrchDetail(CompID, BrchID, _RJOListModel, "", "", "");
               
                _RJOListModel.RJOSearch = "RJO_Search";
                foreach (DataRow dr in DSet.Tables[0].Rows)
                {
                    RJOList _RJO_List = new RJOList();
                    _RJO_List.RJONumber = dr["RwkJobNo"].ToString();
                    _RJO_List.RJODate = dr["RwkJobDate"].ToString();
                    _RJO_List.RJO_Dt = dr["RwkJobDt"].ToString();
                    _RJO_List.ItemName = dr["ItemName"].ToString();
                    _RJO_List.ItemID = dr["item_id"].ToString();
                    _RJO_List.Uom = dr["UomName"].ToString();
                    _RJO_List.RewrkQty = dr["rwk_qty"].ToString();
                    _RJO_List.RJO_Status = dr["RwkJobStatus"].ToString();
                    _RJO_List.CreatedBy = dr["create_by"].ToString();
                    _RJO_List.CreatedON = dr["CreateDate"].ToString();
                    _RJO_List.ApprovedBy = dr["app_by"].ToString();
                    _RJO_List.ApprovedOn = dr["ApproveDate"].ToString();
                    _RJO_List.AmendedBy = dr["mod_by"].ToString();
                    _RJO_List.AmendedOn = dr["AmendedDate"].ToString();
                    
                    _RJOList.Add(_RJO_List);
                }

                _RJOListModel.ReworkJobOrdrList = _RJOList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialReworkableJobOrder.cshtml", _RJOListModel);
        }
        public ActionResult RJODoubleClickFromList(string DocNo, string DocDate, string ListFilterData, string WF_Status,string ItemIdList, ReworkableJobOrder_Model _ReworkableJobOrder_Model)
        {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
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
            _ReworkableJobOrder_Model.Message = "New";
            _ReworkableJobOrder_Model.Command = "Update";
            _ReworkableJobOrder_Model.TransType = "Update";
            _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
            _ReworkableJobOrder_Model.ReworkJO_No = DocNo;
            _ReworkableJobOrder_Model.ReworkJO_Date = DocDate;
            _ReworkableJobOrder_Model.Item_Id = ItemIdList;
            if (WF_Status != null && WF_Status != "")
            {
                _ReworkableJobOrder_Model.WF_Status1 = WF_Status;
            }
            var WF_Status1 = _ReworkableJobOrder_Model.WF_Status1;
            var RwrkJOCodeURL = DocNo;
            var RwrkJoDate = DocDate;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var command = "Add";

            TempData["ModelData"] = _ReworkableJobOrder_Model;
            TempData["ListFilterData"] = ListFilterData;

            return (RedirectToAction("ReworkableJobOrderDetail", "ReworkableJobOrder", new { RwrkJOCodeURL = RwrkJOCodeURL, RwrkJoDate, TransType, BtnName, command, WF_Status1 }));


        }
        public ActionResult DeleteRJODetails(ReworkableJobOrder_Model _ReworkableJobOrder_Model, string command)
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
                string RJONo = _ReworkableJobOrder_Model.ReworkJO_No;
                string RJODelete = _ReworkableJobOrder_ISERVICES.RewrkJO_DeleteDetail(_ReworkableJobOrder_Model, CompID, BrchID);

                if (!string.IsNullOrEmpty(RJONo))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string RJONo1 = RJONo.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, RJONo1, Server);
                }
                _ReworkableJobOrder_Model = new ReworkableJobOrder_Model();
                _ReworkableJobOrder_Model.Message = "Deleted";
                _ReworkableJobOrder_Model.Command = "Refresh";
                _ReworkableJobOrder_Model.TransType = "Refresh";
                _ReworkableJobOrder_Model.BtnName = "BtnDelete";
                TempData["ModelData"] = _ReworkableJobOrder_Model;
                return RedirectToAction("ReworkableJobOrderDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult RJOApprove(ReworkableJobOrder_Model _ReworkableJobOrder_Model, string ListFilterData1)
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
                string RJO_No = _ReworkableJobOrder_Model.ReworkJO_No;
                string RJO_Date = _ReworkableJobOrder_Model.ReworkJO_Date;
                string A_Status = _ReworkableJobOrder_Model.A_Status;
                string A_Level = _ReworkableJobOrder_Model.A_Level;
                string A_Remarks = _ReworkableJobOrder_Model.A_Remarks;

                string Message = _ReworkableJobOrder_ISERVICES.RewrkJOApproveDetails(CompID, BrchID, RJO_No, RJO_Date, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks);
                string ApMessage = Message.Split(',')[2].Trim();
                string ReworkJO_No = Message.Split(',')[0].Trim();

                if (ApMessage == "MRS" || ApMessage == "PFC")
                {
                    _ReworkableJobOrder_Model.Message = "Approved";
                }
                _Common_IServices.SendAlertEmail(CompID, BrchID, MenuID, RJO_No, "AP", UserID, "0");
                _ReworkableJobOrder_Model.TransType = "Update";
                _ReworkableJobOrder_Model.Command = "Approve";
                _ReworkableJobOrder_Model.AppStatus = "D";
                _ReworkableJobOrder_Model.BtnName = "BtnEdit";

                var RwrkJOCodeURL = ReworkJO_No;
                var RwrkJoDate = _ReworkableJobOrder_Model.ReworkJO_Date;
                var TransType = _ReworkableJobOrder_Model.TransType;
                var BtnName = _ReworkableJobOrder_Model.BtnName;
                var command = _ReworkableJobOrder_Model.Command;
                TempData["ModelData"] = _ReworkableJobOrder_Model;
                TempData["ListFilterData"] = ListFilterData1;
                return (RedirectToAction("ReworkableJobOrderDetail", new { RwrkJOCodeURL = RwrkJOCodeURL, RwrkJoDate, TransType, BtnName, command }));


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult ApproveDocByWorkFlow(string AppDtList, string ListFilterData1, string WF_Status)
        {
            ReworkableJobOrder_Model _ReworkableJobOrder_Model = new ReworkableJobOrder_Model();

            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _ReworkableJobOrder_Model.ReworkJO_No = jObjectBatch[i]["RJONo"].ToString();
                    _ReworkableJobOrder_Model.ReworkJO_Date = jObjectBatch[i]["RJODate"].ToString();

                    _ReworkableJobOrder_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _ReworkableJobOrder_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _ReworkableJobOrder_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();

                }
            }
            if (_ReworkableJobOrder_Model.A_Status != "Approve")
            {
                _ReworkableJobOrder_Model.A_Status = "Approve";
            }
            RJOApprove(_ReworkableJobOrder_Model, ListFilterData1);
            if (WF_Status != null && WF_Status != "")
            {
                _ReworkableJobOrder_Model.WF_Status1 = WF_Status;
            }
            var WF_Status1 = _ReworkableJobOrder_Model.WF_Status1;
            var RwrkJOCodeURL = _ReworkableJobOrder_Model.ReworkJO_No;
            var RwrkJoDate = _ReworkableJobOrder_Model.ReworkJO_Date;
            var TransType = _ReworkableJobOrder_Model.TransType;
            var BtnName = _ReworkableJobOrder_Model.BtnName;
            var command = _ReworkableJobOrder_Model.Command;
            return (RedirectToAction("ReworkableJobOrderDetail", new { RwrkJOCodeURL = RwrkJOCodeURL, RwrkJoDate, TransType, BtnName, command, WF_Status1 }));


        }
        public ActionResult ToRefreshByJS(string FrwdDtList, string ListFilterData1, string WF_Status)
        {
            ReworkableJobOrder_Model _ReworkableJobOrder_Model = new ReworkableJobOrder_Model();

            if (FrwdDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(FrwdDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _ReworkableJobOrder_Model.ReworkJO_No = jObjectBatch[i]["RJONo"].ToString();
                    _ReworkableJobOrder_Model.ReworkJO_Date = jObjectBatch[i]["RJODate"].ToString();
                    _ReworkableJobOrder_Model.TransType = "Update";
                    _ReworkableJobOrder_Model.BtnName = "BtnToDetailPage";
                    if (WF_Status != null && WF_Status != "")
                    {
                        _ReworkableJobOrder_Model.WF_Status1 = WF_Status;
                    }
                    TempData["ModelData"] = _ReworkableJobOrder_Model;
                }
            }
            var WF_Status1 = _ReworkableJobOrder_Model.WF_Status1;
            var RwrkJOCodeURL = _ReworkableJobOrder_Model.ReworkJO_No;
            var RwrkJoDate = _ReworkableJobOrder_Model.ReworkJO_Date;
            var TransType = _ReworkableJobOrder_Model.TransType;
            var BtnName = _ReworkableJobOrder_Model.BtnName;
            var command = "Refresh";
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("ReworkableJobOrderDetail", new { RwrkJOCodeURL = RwrkJOCodeURL, RwrkJoDate, TransType, BtnName, command, WF_Status1 }));

        }
        public ActionResult GetReworkJobOrderDashbordList(string docid, string status)
        {
            var WF_Status = status;
            return RedirectToAction("ReworkableJobOrder", new { WF_Status });
        }
        private string CheckRewrkJobOrderAgainstMRS(ReworkableJobOrder_Model _ReworkableJobOrder_Model)
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
                    BrchID = Session["BranchId"].ToString();
                }

                str = _ReworkableJobOrder_ISERVICES.ChkRJOagainstMRS(CompID, BrchID, _ReworkableJobOrder_Model.ReworkJO_No, _ReworkableJobOrder_Model.ReworkJO_Date).Tables[0].Rows[0]["result"].ToString();
                return str;
            }
            catch(Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
            
        }

        [NonAction]
        private DataTable BindItemsList()
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
                
                DataSet dt = _ReworkableJobOrder_ISERVICES.AllDDLBind_OnPageLOad(CompID, BrchID, "0");
                return dt.Tables[0];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }
        private string GetNewBatchNo()
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
                    BrchID = Session["BranchId"].ToString();
                }

                str = _ReworkableJobOrder_ISERVICES.GetNewBatchNo(CompID, BrchID).Tables[0].Rows[0]["result"].ToString();
                return str;
            }
            catch(Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
         
        }
        //[NonAction]
        //private DataTable BindWarehouseList()
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

        //        DataSet dt = _ReworkableJobOrder_ISERVICES.AllDDLBind_OnPageLOad(CompID, BrchID, "0");
        //        return dt.Tables[1];
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }

        //}

       public ActionResult getItemReworkQuantityDetail(string ItemID, string WHID, string Status, 
           string SelectedItemdetail, string hdnCommand, string hdnTranstyp, ReworkableJobOrder_Model _ReworkableJobOrder_Model,string src_type)
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
                DataSet ds = new DataSet();
                ds = _ReworkableJobOrder_ISERVICES.GetReworkQtyDetails(CompID, BrchID, ItemID, WHID, src_type);

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("BatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim())
                            {
                                ds.Tables[0].Rows[i]["rework_qty"] = item.GetValue("ReworkQty");
                            }
                        }
                    }
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemReworkQtyDetail = ds.Tables[0];
                }
                _ReworkableJobOrder_Model.TransType = hdnTranstyp;
                _ReworkableJobOrder_Model.Command = hdnCommand;
                _ReworkableJobOrder_Model.DocumentStatus = Status;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialReworkQuantityDEtail.cshtml", _ReworkableJobOrder_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getItemReworkQuantityDetailAfterInsert(string RJO_No, string RJO_Date, string Status, string ItemId, string WHID, string TransType, string Command, ReworkableJobOrder_Model _ReworkableJobOrder_Model)
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
                DataSet ds = new DataSet();

                ds = _ReworkableJobOrder_ISERVICES.getItemReworkQtyAfterInsert(Comp_ID, Br_ID, RJO_No, RJO_Date, ItemId, WHID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemReworkQtyDetail = ds.Tables[0];
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentCode = Status;
                //ViewBag.TransType = TransType;
                //ViewBag.Command = Command;
                _ReworkableJobOrder_Model.TransType = TransType;
                _ReworkableJobOrder_Model.Command = Command;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialReworkQuantityDEtail.cshtml", _ReworkableJobOrder_Model);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        //[NonAction]
        //private DataTable BindShopfloorList()
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

        //        DataSet dt = _ReworkableJobOrder_ISERVICES.AllDDLBind_OnPageLOad(CompID, BrchID, "0");
        //        return dt.Tables[2];
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }

        //}

        [HttpPost]
        public ActionResult BindWorkStationList(ReworkableJobOrder_Model _ReworkableJobOrder_Model, int shfl_id)
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
                    if (string.IsNullOrEmpty(_ReworkableJobOrder_Model.Item_Id))
                    {
                        product_id = "0";
                    }
                    else
                    {
                        product_id = _ReworkableJobOrder_Model.Item_Name;
                    }
                    DataSet ProductList = _ReworkableJobOrder_ISERVICES.GetWorkStationDAL(Comp_ID, Br_ID, shfl_id);
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

        public JsonResult BindRewrkWHAvalStk(string ItemID, string WarehouseID,string src_type,string accodian_type)
        {
            JsonResult DataRows = null;
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
                DataSet Deatils = _ReworkableJobOrder_ISERVICES.GetRewrkWHAvalStk(CompID, BrchID, ItemID, WarehouseID, src_type, accodian_type);
                DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
     
        public JsonResult BindMaterialName(string ddl_MaterialTyp)
        {
            JsonResult DataRows = null;
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
                DataSet Deatils = _ReworkableJobOrder_ISERVICES.GetMaterialNameByMtrlTyp(CompID, BrchID, ddl_MaterialTyp);
                DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
                //return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
        }
        public ActionResult GetItemList(ReworkableJobOrder_Model queryParameters,string ddl_MaterialTyp,string ddl_HedrItemId)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            string ItemName = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            try
            {
                if (string.IsNullOrEmpty(queryParameters.ItemName1))
                {
                    ItemName = "0";
                }
                else
                {
                    ItemName = queryParameters.ItemName1;
                }
               DataSet ItemList1 = _ReworkableJobOrder_ISERVICES.ItemList(ItemName, CompID, BrchID, ddl_MaterialTyp, ddl_HedrItemId);

                List<ItemName1> _ItemList1 = new List<ItemName1>();
                //foreach (var data in ItemList)
                //{
                //    ItemName1 _ItemDetail = new ItemName1();
                //    _ItemDetail.Item_Id = data.Key;
                //    _ItemDetail.Item_Name = data.Value;
                //    _ItemList1.Add(_ItemDetail);
                //}
                for (int i = 0; i < ItemList1.Tables[0].Rows.Count; i++)
                {
                    string itemId = ItemList1.Tables[0].Rows[i]["Item_id"].ToString();
                    string itemName = ItemList1.Tables[0].Rows[i]["Item_name"].ToString();
                    string Uom = ItemList1.Tables[0].Rows[i]["uom_alias"].ToString();
                    ItemList.Add(itemId + "_" + Uom, itemName);
                }
               // queryParameters.ItemNameList1 = ItemList;

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
            return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetItemUOM(string MaterialID)
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
                DataSet result = _ReworkableJobOrder_ISERVICES.GetItemUOM( Comp_ID, Br_ID, MaterialID);
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
        public JsonResult GetDetailsOfRequiredMaterialTbl(string RJO_No, string RJO_Date,string ShopfloorId)
        {
            JsonResult DataRows = null;
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
                DataSet Deatils = _ReworkableJobOrder_ISERVICES.GetDetailsOfRequiredMaterialTbl(Comp_ID, Br_ID, RJO_No, RJO_Date, ShopfloorId);

                DataRows = Json(JsonConvert.SerializeObject(Deatils));/*Result convert into Json Format for javasript*/
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            return DataRows;
        }
        public ActionResult getCMItemStockBatchWiseBYShpfloor(string ItemId, string ShpfloorId, string Status,  string SelectedItemdetail, string docid, string TransType, string Command)
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
                    BrchID = Session["BranchId"].ToString();
                }
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
               
                ds = _ReworkableJobOrder_ISERVICES.getCMItemStockBatchSerialWise(ItemId, ShpfloorId, CompID, BrchID);

                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("BatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim())
                            {
                                ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("ConsumeQty");
                            }
                        }
                    }
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                }
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getCMItemStockBatchWiseAfterInsert(string RJO_No, string RJO_Date, string Status,string MtrlTypId, string ItemId, string docid, string TransType, string Command)
        {
            try
            {
                DataSet ds = new DataSet();
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string Type = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                
                ds = _ReworkableJobOrder_ISERVICES.getCMStockBatchWiseAfterInsert(Comp_ID, Br_ID, RJO_No, RJO_Date, MtrlTypId, ItemId);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentCode = Status;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;

                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        public ActionResult getItemstockSerialWiseByShpFloor(string ItemId, string Status, string ShpfloorId, string SelectedItemSerial, string docid, string TransType, string Command)
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
                    BrchID = Session["BranchId"].ToString();
                }
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
                //ds = _Common_IServices.getItemstockSerialWise(ItemId, ShpfloorId, CompID, BrchID);
                ds = _ReworkableJobOrder_ISERVICES.getCMItemStockBatchSerialWise(ItemId, ShpfloorId, CompID, BrchID);
                if (SelectedItemSerial != null && SelectedItemSerial != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemSerial);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LOTId").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("SerialNO").ToString().Trim() == ds.Tables[0].Rows[i]["serial_no"].ToString().Trim())
                            {
                                ds.Tables[0].Rows[i]["SerailSelected"] = "Y";
                            }
                        }
                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult getCMItemStockSerialWiseAfterInsert(string RJO_No, string RJO_Date, string Status, string MtrlTypId, string ItemId, string docid, string TransType, string Command)
        {
            try
            {
                DataSet ds = new DataSet();
                string Comp_ID = string.Empty;
                string Br_ID = string.Empty;
                string PL_Type = string.Empty;

                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }
                
               
                ds = _ReworkableJobOrder_ISERVICES.getCMStockSerialWiseAfterInsert(Comp_ID, Br_ID, RJO_No, RJO_Date, MtrlTypId, ItemId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentCode = Status;
                ViewBag.TransType = TransType;
                ViewBag.Command = Command;
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        /*--------------------------For Attatchment Start--------------------------*/
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                RJODetailsattch _RJODetail = new RJODetailsattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                Guid gid = new Guid();
                gid = Guid.NewGuid();


                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _RJODetail.Guid = DocNo;
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
                    _RJODetail.AttachMentDetailItmStp = dt;
                }
                else
                {
                    _RJODetail.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _RJODetail;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        /*--------------------------For Attatchment End--------------------------*/

        /*---------Sub Item Section Start----------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
            , string Flag, string Status, string Doc_no, string Doc_dt, string Wh_id,string Shfl_Id,string src_type,string matrial_type)
            {
            try
            {
                
                var flag2 = string.Empty;
                var Flag1 = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();
                if (Flag == "RJO_RewrkQuantity"|| Flag == "RM_RequireQty")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        if (Flag == "RJO_RewrkQuantity")
                        {
                            if (src_type == "W")
                            {
                                flag2 = "Rwkwh";
                                dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, Wh_id, Item_id, null/*UomId*/, flag2).Tables[0];
                                dt.Columns.Add("Qty", typeof(string));
                            }
                            else
                            {
                                flag2 = "Rwkshfl";
                             
                                dt = _ReworkableJobOrder_ISERVICES.GetSubItemWhAvlstockDetails(CompID, BrchID, Wh_id, Item_id, null/*UomId*/, flag2, "RJO_RewrkQuantity").Tables[0];
                                dt.Columns.Add("Qty", typeof(string));
                            }
                        }
                        else
                        {
                            var Shfl_Id1 = "";
                            if (matrial_type != "SR")
                            {
                                if (src_type == "W")
                                {
                                    Shfl_Id1 = Shfl_Id;
                                }
                                else
                                {
                                    Shfl_Id1 = Wh_id;
                                }
                                    flag2 = "Rwkshfl";
                                dt = _ReworkableJobOrder_ISERVICES.GetSubItemWhAvlstockDetails(CompID, BrchID, Shfl_Id1, Item_id, null/*UomId*/, flag2, "RM_RequireQty").Tables[0];
                                dt.Columns.Add("Qty", typeof(string));
                                //  Flag = "RJO_RewrkQuantity";
                            }
                            else
                            {
                                dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                               // Flag1 = "RM_RequireQty_rwk";
                                Flag1 = "Service_RM_RequireQty";
                            }
                        }
                        
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                      
                        int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                           foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = _ReworkableJobOrder_ISERVICES.RJO_GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, Shfl_Id).Tables[0];
                    }
                }
                if(Flag == "CM_ConsumeQty")
                {
                    if (Status == "I" || Status=="PFC")
                    {
                        dt = _ReworkableJobOrder_ISERVICES.GetCMSubItemShflAvlstockDetails(CompID, BrchID, Doc_no, Doc_dt, Shfl_Id, Item_id, "br").Tables[0];
                        dt.Columns.Add("Qty", typeof(string));
                        JArray arr = JArray.Parse(SubItemListwithPageData);

                        int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["material_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);

                                }
                            }

                        }
                    }
                    else
                    {
                        dt = _ReworkableJobOrder_ISERVICES.RJO_GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, Shfl_Id).Tables[0];
                        IsDisabled = "Y";
                    }

                }
                if (Flag == "RJO_QCAccptQty" || Flag == "RJO_QCRejQty" || Flag == "RJO_QCRwkQty")
                {
                    if (Status == "QC" || Status == "QP")
                    {
                        dt = _ReworkableJobOrder_ISERVICES.QCAcptRejRewkQty_GetSubItemDetails(CompID, BrchID, Item_id,Doc_no, Doc_dt, Flag).Tables[0];
                        
                    }
                    
                }
                //if(Flag == "RM_RequireQty")
                //{
                //    Flag1 = "RM_RequireQty_rwk";
                //}
               
                ViewBag.DocumentMenuId = DocumentMenuId;
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {

                    Flag = Flag,
                    Flag1 = Flag1,
                    //_subitemPageName = "MTO",
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

        /*---------Sub Item Section End----------*/
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
        public List<shift> GetShiftList()
        {
            return new List<shift>
            {
                new shift { id = "0", name = "---Select---" },
                new shift { id = "1", name = "Shift-1" },
                new shift { id = "2", name = "Shift-2" },
                new shift { id = "3", name = "Shift-3" }
            };
        }
        private List<wh_namelist> GetWarehouseList(DataTable warehouseTable)
        {
            var warehouseList = new List<wh_namelist>();

            foreach (DataRow row in warehouseTable.Rows)
            {
                warehouseList.Add(new wh_namelist
                {
                    WareH_id = row["wh_id"].ToString(),
                    wareH_name = row["wh_name"].ToString()
                });
            }

            // Insert the default "---Select---" item at the top
            warehouseList.Insert(0, new wh_namelist
            {
                WareH_id = "0",
                wareH_name = "---Select---"
            });

            return warehouseList;
        }
        private List<Shopfloorlist> GetShopfloorList(DataTable shopfloorTable)
        {
            var shopfloorList = new List<Shopfloorlist>();

            foreach (DataRow row in shopfloorTable.Rows)
            {
                shopfloorList.Add(new Shopfloorlist
                {
                    shflr_id = row["shfl_id"].ToString(),
                    shflr_name = row["shfl_name"].ToString()
                });
            }

            // Add default "---Select---" option
            shopfloorList.Insert(0, new Shopfloorlist
            {
                shflr_id = "0",
                shflr_name = "---Select---"
            });

            return shopfloorList;
        }


    }

}