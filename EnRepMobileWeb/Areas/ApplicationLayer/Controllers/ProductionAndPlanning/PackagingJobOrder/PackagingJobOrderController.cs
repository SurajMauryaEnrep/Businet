using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.ProductionAndPlanning.PackagingJobOrder;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.ProductionAndPlanning.PackagingJobOrder;
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

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.ProductionAndPlanning.PackagingJobOrder
{
    public class PackagingJobOrderController : Controller
    {
        string CompID, BrchID, UserID, language, title = String.Empty;
        string DocumentMenuId = "105105128";
        List<PJOList> _PJOList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        CommonController cmn = new CommonController();
        PackagingJobOrder_ISERVICES _PackagingJobOrder_ISERVICES;
        DataTable dt;
        string str;
        public PackagingJobOrderController(Common_IServices _Common_IServices, PackagingJobOrder_ISERVICES _PackagingJobOrder_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._PackagingJobOrder_ISERVICES = _PackagingJobOrder_ISERVICES;
        }
        // GET: ApplicationLayer/PackagingJobOrder
        public ActionResult PackagingJobOrder(string WF_Status)
        {
            try
            {
                ViewBag.DocID = DocumentMenuId;

              
                PJOListModel _PJOListModel = new PJOListModel();
                if (WF_Status != null && WF_Status != "")
                {
                    _PJOListModel.WF_Status = WF_Status;

                }
                //DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string CurrentDate = range.ToDate;
                CommonPageDetails();
                _PJOListModel.Title = title;
                ViewBag.DocumentMenuId = DocumentMenuId;

              
                if (TempData["ListFilterData"] != null)
                {
                    if (TempData["ListFilterData"].ToString() != "")
                    {
                        var PRData = TempData["ListFilterData"].ToString();
                        var a = PRData.Split(',');
                        _PJOListModel.ItemID = a[0].Trim();
                        _PJOListModel.FromDate = a[1].Trim();
                        _PJOListModel.ToDate = a[2].Trim();
                        _PJOListModel.Status = a[3].Trim();
                        if (_PJOListModel.Status == "0")
                        {
                            _PJOListModel.Status = null;
                        }
                        _PJOListModel.ListFilterData = TempData["ListFilterData"].ToString();
                    }
                }
                else
                {
                    _PJOListModel.FromDate = startDate;
                    _PJOListModel.ToDate = CurrentDate;
                }
               
                GetAllData(_PJOListModel);

                _PJOListModel.Title = title;
                _PJOListModel.PJOSearch = "0";
                return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/PackagingJobOrder/PackagingJobOrderList.cshtml", _PJOListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public void CompDataWithID()
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
        }
        private void GetAllData(PJOListModel _PJOListModel)
        {

            try
            {
                CompDataWithID();
                var docid = DocumentMenuId;
                string wfstatus = "";

                if (_PJOListModel.WF_Status != null)
                {
                    wfstatus = _PJOListModel.WF_Status;
                }
                else
                {
                    wfstatus = "";
                }
                DataSet GetAllData = _PackagingJobOrder_ISERVICES.GetAllData(CompID, BrchID, _PJOListModel, UserID, wfstatus, DocumentMenuId);
                List<ItemNameList> _itmListL = new List<ItemNameList>();
                //  dt = BindItemsList();
                //foreach (DataRow dr in GetAllData.Tables[0].Rows) /* Commented By Suraj Maurya on 28-07-2025 */
                //{
                //    ItemNameList _itmL = new ItemNameList();
                //    _itmL.ID = dr["item_id"].ToString();
                //    _itmL.Name = dr["item_name"].ToString();
                //    _itmListL.Add(_itmL);

                //}
                _itmListL.Insert(0,new ItemNameList() { ID="0",Name="All"});
                _PJOListModel.ItemNameLlist = _itmListL;

                List<Status> statusLists = new List<Status>();
                foreach (DataRow dr in ViewBag.StatusList.Rows)
                {
                    Status list = new Status();
                    list.status_id = dr["status_code"].ToString();
                    list.status_name = dr["status_name"].ToString();
                    statusLists.Add(list);
                }
                _PJOListModel.StatusList = statusLists;

                SetAllDatailListTable(_PJOListModel, GetAllData);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private void SetAllDatailListTable(PJOListModel _PJOListModel,DataSet DSet)
        {
            try
            {
                DataTable TableData = new DataTable();
               if( _PJOListModel.PJOSearch == "PJO_Search")
                {
                    TableData =DSet.Tables[0];
                }
                else
                {
                    TableData = DSet.Tables[1];
                }
                List<PJOList> _PJOList = new List<PJOList>();
                if (TableData.Rows.Count > 0)
                {
                    foreach (DataRow dr in TableData.Rows)
                    {
                        PJOList _PJO_List = new PJOList();
                        _PJO_List.PJONumber = dr["pckJobNo"].ToString();
                        _PJO_List.PJODate = dr["pckJobDate"].ToString();
                        _PJO_List.PJO_Dt = dr["pckJobDt"].ToString();
                        _PJO_List.ItemName = dr["ItemName"].ToString();
                        _PJO_List.Uom = dr["UomName"].ToString();
                        _PJO_List.PkgQty = dr["pkg_qty"].ToString();
                        _PJO_List.PJO_Status = dr["pkgJobStatus"].ToString();
                        _PJO_List.CreatedON = dr["CreateDate"].ToString();
                        _PJO_List.ApprovedBy = dr["app_by"].ToString();
                        _PJO_List.ApprovedOn = dr["ApproveDate"].ToString();
                        _PJO_List.CreatedBy = dr["create_by"].ToString();
                        _PJO_List.AmendedBy = dr["mod_by"].ToString();
                        _PJO_List.AmendedOn = dr["AmendedDate"].ToString();
                        _PJO_List.ItemID = dr["item_id"].ToString();
                        if (_PJOListModel.PJOSearch != "PJO_Search")
                        {
                            _PJO_List.FinStDt = DSet.Tables[3].Rows[0]["findate"].ToString();
                        }

                        _PJOList.Add(_PJO_List);
                    }
                }
                _PJOListModel.packingJobOrdrList = _PJOList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
          
        }
        [HttpPost]
        public ActionResult SearchPackingJOListDetail(string ItemId, string Fromdate, string Todate, string Status)
        {
            PJOListModel _PJOListModel = new PJOListModel();
            try
            {
                _PJOListModel.WF_Status = null;
                string User_ID = string.Empty;
                CompDataWithID();
                var docid = DocumentMenuId;
                List<PJOList> _PJOList = new List<PJOList>();
                _PJOListModel.ItemID = ItemId;

                _PJOListModel.FromDate = Fromdate;
                _PJOListModel.ToDate = Todate;
                _PJOListModel.Status = Status;
                DataSet DSet = _PackagingJobOrder_ISERVICES.GetRJOListandSrchDetail(CompID, BrchID, _PJOListModel, "", "", "");

                _PJOListModel.PJOSearch = "PJO_Search";
                SetAllDatailListTable(_PJOListModel, DSet);
                
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                //return Json("ErrorPage");
                return View("~/Views/Shared/Error.cshtml");
            }
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPackagingJobOrder.cshtml", _PJOListModel);
        }
        public ActionResult AddPackagingJobOrderDetail()
        {            
            PackagingJobOrder_Model _PackagingJobOrder_Model = new PackagingJobOrder_Model();
            _PackagingJobOrder_Model.Message = "New";
            _PackagingJobOrder_Model.Command = "Add";
            _PackagingJobOrder_Model.AppStatus = "D";
            ViewBag.DocumentStatus = _PackagingJobOrder_Model.AppStatus;
            _PackagingJobOrder_Model.TransType = "Save";
            _PackagingJobOrder_Model.BtnName = "BtnAddNew";
            TempData["ModelData"] = _PackagingJobOrder_Model;
            TempData["ListFilterData"] = null;
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("PackagingJobOrder");
            }         
            CommonPageDetails();
            return RedirectToAction("PackagingJobOrderDetail", "PackagingJobOrder");
        }
        public ActionResult PackagingJobOrderDetail(PackagingJobOrder_Model _PackagingJobOrder_Model1, string PackingJOCodeURL, string PackingJoDate, string TransType, string BtnName, string command)
        {
            try
            {
                ViewBag.DocID = DocumentMenuId;
                CommonPageDetails();
                /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                var commCont = new CommonController(_Common_IServices);
                if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PackingJoDate) == "TransNotAllow")
                {
                    //TempData["Message2"] = "TransNotAllow";
                    ViewBag.Message = "TransNotAllow";
                }
                var _PackagingJobOrder_Model = TempData["ModelData"] as PackagingJobOrder_Model;
                if (_PackagingJobOrder_Model != null)
                {

                    
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    string ValDigit = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _PackagingJobOrder_Model.Title = title;
                    _PackagingJobOrder_Model.ValDigit = ValDigit;
                    _PackagingJobOrder_Model.QtyDigit = QtyDigit;
                    _PackagingJobOrder_Model.RateDigit = RateDigit;
                    ViewBag.ValDigit = ValDigit;
                    ViewBag.QtyDigit = QtyDigit;
                    ViewBag.RateDigit = RateDigit;
                    if (_PackagingJobOrder_Model.PJO_No == null)
                    {
                        str = GetNewBatchNo();
                        _PackagingJobOrder_Model.Newbatch_No = str;
                    }

                    GetAllDropDownDetail(_PackagingJobOrder_Model);

                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _PackagingJobOrder_Model.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    if (_PackagingJobOrder_Model.TransType == "Update" || _PackagingJobOrder_Model.Command == "Edit")
                    {
                        AllDetaildata(_PackagingJobOrder_Model);
                    }
                    if (_PackagingJobOrder_Model.BtnName != null)
                    {
                        _PackagingJobOrder_Model.BtnName = _PackagingJobOrder_Model.BtnName;
                    }
                    _PackagingJobOrder_Model.TransType = _PackagingJobOrder_Model.TransType;
                    _PackagingJobOrder_Model.Command = _PackagingJobOrder_Model.Command;
                    _PackagingJobOrder_Model.TranstypAttach = _PackagingJobOrder_Model.TransType;
                    if (_PackagingJobOrder_Model.DocumentStatus == null)
                    {
                        _PackagingJobOrder_Model.DocumentStatus = "D";
                    }
                    else
                    {
                        _PackagingJobOrder_Model.DocumentStatus = _PackagingJobOrder_Model.DocumentStatus;
                    }
                    ViewBag.DocumentStatus = _PackagingJobOrder_Model.DocumentStatus;

                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/PackagingJobOrder/PackagingJobOrderDetail.cshtml", _PackagingJobOrder_Model);
                }
                else
                {
                   
                    CommonPageDetails();
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    string ValDigit1 = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                    string QtyDigit1 = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    string RateDigit1 = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));
                    _PackagingJobOrder_Model1.ValDigit = ValDigit1;
                    _PackagingJobOrder_Model1.QtyDigit = QtyDigit1;
                    _PackagingJobOrder_Model1.RateDigit = RateDigit1;
                    ViewBag.ValDigit = ValDigit1;
                    ViewBag.QtyDigit = QtyDigit1;
                    ViewBag.RateDigit = RateDigit1;
                    ViewBag.DocumentStatus = "D";
                    if (_PackagingJobOrder_Model1.PJO_No == null)
                    {
                        str = GetNewBatchNo();
                        _PackagingJobOrder_Model1.Newbatch_No = str;
                    }
                    GetAllDropDownDetail(_PackagingJobOrder_Model1);
                    if (TempData["ListFilterData"] != null)
                    {
                        _PackagingJobOrder_Model1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    if (_PackagingJobOrder_Model1.TransType == "Update" || _PackagingJobOrder_Model1.Command == "Edit")
                    {
                        _PackagingJobOrder_Model1.PJO_No = PackingJOCodeURL;
                        _PackagingJobOrder_Model1.PJO_Date = PackingJoDate;
                        AllDetaildata(_PackagingJobOrder_Model1);
                    }
                    var RwrkJOCode = "";
                    if (PackingJOCodeURL != null)
                    {
                        RwrkJOCode = PackingJOCodeURL;
                        _PackagingJobOrder_Model1.PJO_No = PackingJOCodeURL;
                    }
                    else
                    {
                        RwrkJOCode = _PackagingJobOrder_Model1.PJO_No;
                    }
                    if (TransType != null)
                    {
                        _PackagingJobOrder_Model1.TransType = TransType;
                    }
                    if (command != null)
                    {
                        _PackagingJobOrder_Model1.Command = command;
                    }

                    if (_PackagingJobOrder_Model1.BtnName == null && _PackagingJobOrder_Model1.Command == null)
                    {
                        _PackagingJobOrder_Model1.BtnName = "AddNew";
                        _PackagingJobOrder_Model1.Command = "Add";
                        _PackagingJobOrder_Model1.AppStatus = "D";
                        ViewBag.DocumentStatus = _PackagingJobOrder_Model1.AppStatus;
                        _PackagingJobOrder_Model1.TransType = "Save";
                        _PackagingJobOrder_Model1.BtnName = "BtnAddNew";

                    }

                    if (_PackagingJobOrder_Model1.BtnName != null)
                    {
                        _PackagingJobOrder_Model1.BtnName = _PackagingJobOrder_Model1.BtnName;
                    }
                    if (_PackagingJobOrder_Model1.TransType == null || _PackagingJobOrder_Model1.BtnName == null)
                    {
                        _PackagingJobOrder_Model1.TransType = "Update";
                        _PackagingJobOrder_Model1.BtnName = "BtnEdit";

                    }
                    _PackagingJobOrder_Model1.TransType = _PackagingJobOrder_Model1.TransType;
                    _PackagingJobOrder_Model1.TranstypAttach = _PackagingJobOrder_Model1.TransType;
                    if (_PackagingJobOrder_Model1.DocumentStatus != null)
                    {
                        _PackagingJobOrder_Model1.DocumentStatus = _PackagingJobOrder_Model1.DocumentStatus;
                        ViewBag.DocumentStatus = _PackagingJobOrder_Model1.DocumentStatus;
                    }

                    _PackagingJobOrder_Model1.Title = title;

                    return View("~/Areas/ApplicationLayer/Views/ProductionAndPlanning/PackagingJobOrder/PackagingJobOrderDetail.cshtml", _PackagingJobOrder_Model1);
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        private void GetAllDropDownDetail(PackagingJobOrder_Model _PackagingJobOrder_Model1)
        {
            CompDataWithID();          
            DataSet TableData = _PackagingJobOrder_ISERVICES.GetAllDataDeatil(CompID, BrchID, "0");
            /**Bind DropDownList Item Name**/
            List<ItemName> _itmList1 = new List<ItemName>() { 
                new ItemName { ID="0", Name="---Select---" }
            };
            //foreach (DataRow dr in TableData.Tables[0].Rows) /* Commented By Suraj Maurya on 28-07-2025 */
            //{
            //    ItemName _itm = new ItemName();
            //    _itm.ID = dr["item_id"].ToString();
            //    _itm.Name = dr["item_name"].ToString();
            //    _itmList1.Add(_itm);
            //}
            _PackagingJobOrder_Model1.ItemNamelist = _itmList1;

            /**Bind DropDownList Warehouse **/
            List<wh_namelist> _whList1 = new List<wh_namelist>();
            foreach (DataRow dr in TableData.Tables[1].Rows)
            {
                wh_namelist _wh = new wh_namelist();
                _wh.WareH_id = dr["wh_id"].ToString();
                _wh.wareH_name = dr["wh_name"].ToString();
                _whList1.Add(_wh);
            }
            _whList1.Insert(0, new wh_namelist() { WareH_id = "0", wareH_name = "---Select---" });
            _PackagingJobOrder_Model1.wh_Namelist = _whList1;

            /**Bind DropDownList Shopfloorlist **/
            List<Shopfloorlist> _shflrList1 = new List<Shopfloorlist>();
            foreach (DataRow dr in TableData.Tables[2].Rows)
            {
                Shopfloorlist _shflr = new Shopfloorlist();
                _shflr.shflr_id = dr["shfl_id"].ToString();
                _shflr.shflr_name = dr["shfl_name"].ToString();
                _shflrList1.Add(_shflr);

            }
            _shflrList1.Insert(0, new Shopfloorlist() { shflr_id = "0", shflr_name = "---Select---" });
            _PackagingJobOrder_Model1.ShopfloorNamelist = _shflrList1;

            /**Bind WorkStation Data**/
            List<WorkstationName> _wrklist1 = new List<WorkstationName>();
            _wrklist1.Add(new WorkstationName { ws_id = "0", ws_name = "---Select---" });
            _PackagingJobOrder_Model1.WorkstationNameList = _wrklist1;

            List<ItemName1> _ItemName1 = new List<ItemName1>();
            _ItemName1.Add(new ItemName1 { Item_Id = "0", Item_Name = "---Select---" });
            _PackagingJobOrder_Model1.ItemNameList1 = _ItemName1;

            /**Bind Shift Data**/
            List<shift> sh1 = new List<shift>();
            shift shObj = new shift();
            shObj.id = "0";
            shObj.name = "---Select---";
            sh1.Add(shObj);
            shift shObj1 = new shift();
            shObj1.id = "1";
            shObj1.name = "Shift-1";
            sh1.Add(shObj1);
            shift shObj2 = new shift();
            shObj2.id = "2";
            shObj2.name = "Shift-2";
            sh1.Add(shObj2);
            shift shObj3 = new shift();
            shObj3.id = "3";
            shObj3.name = "Shift-3";
            sh1.Add(shObj3);
            _PackagingJobOrder_Model1.shiftList = sh1;

        }
        private void AllDetaildata(PackagingJobOrder_Model _PackagingJobOrder_Model)
        {
            try
            {
                CompDataWithID();
                string ValDigit1 = ToFixDecimal(Convert.ToInt32(Session["ValDigit"].ToString()));
                string QtyDigit1 = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                string RateDigit1 = ToFixDecimal(Convert.ToInt32(Session["RateDigit"].ToString()));


                string JobCard_NO = _PackagingJobOrder_Model.PJO_No;
                string JobCard_Date = _PackagingJobOrder_Model.PJO_Date;
                string ItemId = _PackagingJobOrder_Model.Item_Id;
                DataSet ds = _PackagingJobOrder_ISERVICES.GetRewrkJODetailEditUpdate(CompID, BrchID, JobCard_NO, JobCard_Date, UserID, DocumentMenuId);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    _PackagingJobOrder_Model.PJO_No = ds.Tables[0].Rows[0]["job_no"].ToString();
                    _PackagingJobOrder_Model.DocNoAttach = ds.Tables[0].Rows[0]["job_no"].ToString();
                    _PackagingJobOrder_Model.PJO_Date = ds.Tables[0].Rows[0]["job_date"].ToString();

                    List<ItemName> _itmList1 = new List<ItemName>() {
                new ItemName { ID=ds.Tables[0].Rows[0]["item_id"].ToString(), Name=ds.Tables[0].Rows[0]["item_name"].ToString() }
            };

                    _PackagingJobOrder_Model.ItemNamelist = _itmList1;

                    _PackagingJobOrder_Model.Item_Name = ds.Tables[0].Rows[0]["item_name"].ToString();
                    _PackagingJobOrder_Model.Item_Id = ds.Tables[0].Rows[0]["item_id"].ToString();
                    _PackagingJobOrder_Model.ItemName = ds.Tables[0].Rows[0]["item_id"].ToString();
                    _PackagingJobOrder_Model.uom_id = ds.Tables[0].Rows[0]["uom"].ToString();
                    _PackagingJobOrder_Model.uom_name = ds.Tables[0].Rows[0]["UomName"].ToString();
                    _PackagingJobOrder_Model.sub_item = ds.Tables[0].Rows[0]["sub_item"].ToString();
                    _PackagingJobOrder_Model.Warehouse = ds.Tables[0].Rows[0]["wh_id"].ToString();
                    _PackagingJobOrder_Model.WarehouseID = ds.Tables[0].Rows[0]["wh_id"].ToString();
                    // _PackagingJobOrder_Model.WarehouseName = ds.Tables[0].Rows[0]["wh_name"].ToString();

                    //List<wh_namelist> _whList1 = new List<wh_namelist>();
                    //_whList1.Add(new wh_namelist { WareH_id = _PackagingJobOrder_Model.WarehouseID, wareH_name = _PackagingJobOrder_Model.WarehouseName });
                    //_PackagingJobOrder_Model.wh_Namelist = _whList1;

                    _PackagingJobOrder_Model.Available_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["AvlstockQty"]).ToString(QtyDigit1);
                    _PackagingJobOrder_Model.pkg_Qty = Convert.ToDecimal(ds.Tables[0].Rows[0]["pkg_qty"]).ToString(QtyDigit1);
                    _PackagingJobOrder_Model.Shopfloor = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                    _PackagingJobOrder_Model.Shopfloor_Id = ds.Tables[0].Rows[0]["shfl_id"].ToString();
                    _PackagingJobOrder_Model.ddl_Workstation = ds.Tables[0].Rows[0]["ws_id"].ToString();
                    _PackagingJobOrder_Model.WorkstationID = ds.Tables[0].Rows[0]["ws_id"].ToString();
                    _PackagingJobOrder_Model.WorkstationName = ds.Tables[0].Rows[0]["ws_name"].ToString();

                    List<WorkstationName> _wrklist = new List<WorkstationName>();
                    _wrklist.Add(new WorkstationName { ws_id = _PackagingJobOrder_Model.ddl_Workstation, ws_name = _PackagingJobOrder_Model.WorkstationName });
                    _PackagingJobOrder_Model.WorkstationNameList = _wrklist;

                    _PackagingJobOrder_Model.supervisor_name = ds.Tables[0].Rows[0]["supervisor_name"].ToString();
                    _PackagingJobOrder_Model.ddl_shift = ds.Tables[0].Rows[0]["shift_id"].ToString();
                    _PackagingJobOrder_Model.Newbatch_No = ds.Tables[0].Rows[0]["batch_no"].ToString();
                    _PackagingJobOrder_Model.NewExpiryDate = ds.Tables[0].Rows[0]["exp_date"].ToString();
                    _PackagingJobOrder_Model.Created_by = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    _PackagingJobOrder_Model.Created_on = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                    _PackagingJobOrder_Model.Create_id = ds.Tables[0].Rows[0]["createid"].ToString();

                    _PackagingJobOrder_Model.Amended_by = ds.Tables[0].Rows[0]["AmendedBy"].ToString();
                    _PackagingJobOrder_Model.Amended_on = ds.Tables[0].Rows[0]["ModifyDate"].ToString();
                    _PackagingJobOrder_Model.Approved_by = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                    _PackagingJobOrder_Model.Approved_on = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                    _PackagingJobOrder_Model.StatusName = ds.Tables[0].Rows[0]["RJOStatus"].ToString();
                    _PackagingJobOrder_Model.Status_Code = ds.Tables[0].Rows[0]["job_status"].ToString().Trim();
                    _PackagingJobOrder_Model.src_type = ds.Tables[0].Rows[0]["src_type"].ToString().Trim();

                    _PackagingJobOrder_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                    _PackagingJobOrder_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);


                    ViewBag.ReqMaterialDetailsList = ds.Tables[1];
                    ViewBag.ItemReworkQtyDetail = ds.Tables[2];
                    ViewBag.AttechmentDetails = ds.Tables[7];
                    if (_PackagingJobOrder_Model.Status_Code == "QP" || _PackagingJobOrder_Model.Status_Code == "QC")
                    {
                        ViewBag.ConsumeMaterialDetails = ds.Tables[8];
                        ViewBag.ItemStockBatchWise = ds.Tables[9];
                        ViewBag.ItemStockSerialWise = ds.Tables[10];
                        ViewBag.MaterialOutputDetails = ds.Tables[11];
                    }
                    ViewBag.SubItemDetails = ds.Tables[12];
                    ViewBag.QtyDigit = QtyDigit1;
                }
                var create_id = ds.Tables[0].Rows[0]["createid"].ToString();
                var approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                string Statuscode = ds.Tables[0].Rows[0]["job_status"].ToString().Trim();
                if (Statuscode == "C")
                {
                    _PackagingJobOrder_Model.Cancelled = true;
                }
                else
                {
                    _PackagingJobOrder_Model.Cancelled = false;
                }
                if (Statuscode == "QP" || Statuscode == "QC")
                {
                    _PackagingJobOrder_Model.JobCompletion = true;
                }
                else
                {
                    _PackagingJobOrder_Model.JobCompletion = false;
                }
                _PackagingJobOrder_Model.DocumentStatus = Statuscode;
                if (Statuscode != "D" && Statuscode != "F")
                {
                    ViewBag.AppLevel = ds.Tables[3];
                }
                if (ViewBag.AppLevel != null && _PackagingJobOrder_Model.Command != "Edit")
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
                            _PackagingJobOrder_Model.BtnName = "Refresh";
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
                                _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
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
                                _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";

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
                            _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
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
                                _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
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
                            _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
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
                            _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
                        }
                    }
                    if (Statuscode == "A" || Statuscode == "MRS" || Statuscode == "PI" || Statuscode == "I" || Statuscode == "PFC")
                    {
                        if (create_id == UserID || approval_id == UserID)
                        {
                            _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
                            /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                            if (TempData["Message1"] != null)
                            {
                                ViewBag.Message = TempData["Message1"];
                            }
                            /*End to chk Financial year exist or not*/

                        }
                        else
                        {
                            _PackagingJobOrder_Model.BtnName = "Refresh";
                        }
                    }
                    if (Statuscode == "QP" || Statuscode == "QC")
                    {
                        _PackagingJobOrder_Model.BtnName = "Refresh";
                    }
                }

                if (ViewBag.AppLevel.Rows.Count == 0)
                {
                    ViewBag.Approve = "Y";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PackagingJobOrderBtnCommand(PackagingJobOrder_Model _PackagingJobOrder_Model, string command)
        {
            try
            {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_PackagingJobOrder_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                CompDataWithID();
                switch (command)
                {
                    case "AddNew":
                       
                        PackagingJobOrder_Model _PackagingJobOrder_ModelAdd = new PackagingJobOrder_Model();
                        _PackagingJobOrder_ModelAdd.Message = "New";
                        _PackagingJobOrder_ModelAdd.Command = "Add";
                        _PackagingJobOrder_ModelAdd.AppStatus = "D";
                        _PackagingJobOrder_ModelAdd.DocumentStatus = "D";
                        ViewBag.DocumentStatus = _PackagingJobOrder_ModelAdd.DocumentStatus;
                        _PackagingJobOrder_ModelAdd.TransType = "Save";
                        _PackagingJobOrder_ModelAdd.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _PackagingJobOrder_ModelAdd;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
                      
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_PackagingJobOrder_Model.PJO_No))
                                return RedirectToAction("PJODoubleClickFromList", new { DocNo = _PackagingJobOrder_Model.PJO_No, DocDate = _PackagingJobOrder_Model.PJO_Date, ListFilterData = _PackagingJobOrder_Model.ListFilterData1, WF_Status = _PackagingJobOrder_Model.WFStatus });
                            else
                                _PackagingJobOrder_ModelAdd.Command = "Refresh";
                            _PackagingJobOrder_ModelAdd.TransType = "Refresh";
                            _PackagingJobOrder_ModelAdd.BtnName = "Refresh";
                            _PackagingJobOrder_ModelAdd.DocumentStatus = null;
                            TempData["ModelData"] = _PackagingJobOrder_ModelAdd;
                            return RedirectToAction("PackagingJobOrderDetail", "PackagingJobOrder", _PackagingJobOrder_ModelAdd);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("PackagingJobOrderDetail", "PackagingJobOrder");

                    case "Edit":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("PJODoubleClickFromList", new { DocNo = _PackagingJobOrder_Model.PJO_No, DocDate = _PackagingJobOrder_Model.PJO_Date, ListFilterData = _PackagingJobOrder_Model.ListFilterData1, WF_Status = _PackagingJobOrder_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string PkgDt = _PackagingJobOrder_Model.PJO_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PkgDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("PJODoubleClickFromList", new { DocNo = _PackagingJobOrder_Model.PJO_No, DocDate = _PackagingJobOrder_Model.PJO_Date, ListFilterData = _PackagingJobOrder_Model.ListFilterData1, WF_Status = _PackagingJobOrder_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        if (_PackagingJobOrder_Model.Status_Code == "MRS")
                        {
                            if (CheckPackingJobOrderAgainstMRS(_PackagingJobOrder_Model) == "Used")
                            {
                                _PackagingJobOrder_Model.Message = "Used";
                                _PackagingJobOrder_Model.TransType = "Update";
                                _PackagingJobOrder_Model.Command = "Add";
                                _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
                                TempData["ModelData"] = _PackagingJobOrder_Model;
                            }

                            else
                            {
                                _PackagingJobOrder_Model.TransType = "Update";
                                _PackagingJobOrder_Model.Command = command;
                                _PackagingJobOrder_Model.BtnName = "BtnEdit";
                                _PackagingJobOrder_Model.Message = "New";
                                _PackagingJobOrder_Model.AppStatus = "D";
                                _PackagingJobOrder_Model.DocumentStatus = "D";
                                ViewBag.DocumentStatus = _PackagingJobOrder_Model.AppStatus;
                                TempData["ModelData"] = _PackagingJobOrder_Model;
                            }
                        }
                        else
                        {
                            _PackagingJobOrder_Model.TransType = "Update";
                            _PackagingJobOrder_Model.Command = command;
                            _PackagingJobOrder_Model.BtnName = "BtnEdit";
                            _PackagingJobOrder_Model.Message = "New";
                            _PackagingJobOrder_Model.AppStatus = "D";
                            _PackagingJobOrder_Model.DocumentStatus = "D";
                            ViewBag.DocumentStatus = _PackagingJobOrder_Model.AppStatus;
                        }
                        
                        var TransType = "Update";
                        var BtnName = "BtnEdit";
                        var PackingJOCodeURL = _PackagingJobOrder_Model.PJO_No;
                        var PackingJoDate = _PackagingJobOrder_Model.PJO_Date;
                        command = _PackagingJobOrder_Model.Command;
                        TempData["ModelData"] = _PackagingJobOrder_Model;
                        TempData["ListFilterData"] = _PackagingJobOrder_Model.ListFilterData1;
                        return (RedirectToAction("PackagingJobOrderDetail", new { PackingJOCodeURL = PackingJOCodeURL, PackingJoDate, TransType, BtnName, command }));

                    case "Delete":
                        _PackagingJobOrder_Model.Command = command;
                        _PackagingJobOrder_Model.BtnName = "Refresh";
                        DeletePJODetails(_PackagingJobOrder_Model, command);
                        TempData["ListFilterData"] = _PackagingJobOrder_Model.ListFilterData1;
                        return RedirectToAction("PackagingJobOrderDetail");

                    case "Save":
                        _PackagingJobOrder_Model.Command = command;

                        SavePkgJODetail(_PackagingJobOrder_Model);
                        if (_PackagingJobOrder_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        PackingJOCodeURL = _PackagingJobOrder_Model.PJO_No;
                        PackingJoDate = _PackagingJobOrder_Model.PJO_Date;
                        TransType = _PackagingJobOrder_Model.TransType;
                        BtnName = _PackagingJobOrder_Model.BtnName;
                        TempData["ModelData"] = _PackagingJobOrder_Model;
                        TempData["ListFilterData"] = _PackagingJobOrder_Model.ListFilterData1;
                        return (RedirectToAction("PackagingJobOrderDetail", new { PackingJOCodeURL = PackingJOCodeURL, PackingJoDate, TransType, BtnName, command }));
                    case "Forward":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("PJODoubleClickFromList", new { DocNo = _PackagingJobOrder_Model.PJO_No, DocDate = _PackagingJobOrder_Model.PJO_Date, ListFilterData = _PackagingJobOrder_Model.ListFilterData1, WF_Status = _PackagingJobOrder_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string PkgDt1 = _PackagingJobOrder_Model.PJO_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PkgDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("PJODoubleClickFromList", new { DocNo = _PackagingJobOrder_Model.PJO_No, DocDate = _PackagingJobOrder_Model.PJO_Date, ListFilterData = _PackagingJobOrder_Model.ListFilterData1, WF_Status = _PackagingJobOrder_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/

                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("PJODoubleClickFromList", new { DocNo = _PackagingJobOrder_Model.PJO_No, DocDate = _PackagingJobOrder_Model.PJO_Date, ListFilterData = _PackagingJobOrder_Model.ListFilterData1, WF_Status = _PackagingJobOrder_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string PkgDt2 = _PackagingJobOrder_Model.PJO_Date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, PkgDt2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("PJODoubleClickFromList", new { DocNo = _PackagingJobOrder_Model.PJO_No, DocDate = _PackagingJobOrder_Model.PJO_Date, ListFilterData = _PackagingJobOrder_Model.ListFilterData1, WF_Status = _PackagingJobOrder_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        _PackagingJobOrder_Model.Command = command;
                        PJOApprove(_PackagingJobOrder_Model, "");

                        PackingJOCodeURL = _PackagingJobOrder_Model.PJO_No;
                        PackingJoDate = _PackagingJobOrder_Model.PJO_Date;
                        TransType = _PackagingJobOrder_Model.TransType;
                        BtnName = _PackagingJobOrder_Model.BtnName;
                        TempData["ModelData"] = _PackagingJobOrder_Model;
                        TempData["ListFilterData"] = _PackagingJobOrder_Model.ListFilterData1;
                        return (RedirectToAction("PackagingJobOrderDetail", new { PackingJOCodeURL = PackingJOCodeURL, PackingJoDate, TransType, BtnName, command }));

                    case "Refresh":
                        PackagingJobOrder_Model _PackagingJobOrder_ModelRefresh = new PackagingJobOrder_Model();
                        _PackagingJobOrder_Model.Message = null;
                        _PackagingJobOrder_ModelRefresh.Command = command;
                        _PackagingJobOrder_ModelRefresh.TransType = "Refresh";
                        _PackagingJobOrder_ModelRefresh.BtnName = "Refresh";
                        _PackagingJobOrder_ModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = _PackagingJobOrder_ModelRefresh;
                        TempData["ListFilterData"] = _PackagingJobOrder_Model.ListFilterData1;
                        return RedirectToAction("PackagingJobOrderDetail");

                    case "Print":
                    //return GenratePdfFile(_PackagingJobOrder_Model);
                    case "BacktoList":
                        TempData["ListFilterData"] = _PackagingJobOrder_Model.ListFilterData1;
                        return RedirectToAction("PackagingJobOrder", "PackagingJobOrder");

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
        public ActionResult SavePkgJODetail(PackagingJobOrder_Model _PackagingJobOrder_Model)
        {
            string SaveMessage = "";
         
            CommonPageDetails();
            string PageName = title.Replace(" ", "");

            try
            {
                if (_PackagingJobOrder_Model.Cancelled == false)
                {
                    CompDataWithID();
                    _PackagingJobOrder_Model.DocumentMenuId = _PackagingJobOrder_Model.DocumentMenuId;
                 
                    DataTable DtblHDetail = new DataTable();
                    DataTable DtblReqMatrlDetail = new DataTable();
                    DataTable PackingQtyItemDetails = new DataTable();
                    DataTable DtblAttchDetail = new DataTable();
                    DataTable DtblConsumeMatrlDetail = new DataTable();
                    DataTable CMItemBatchDetails = new DataTable();
                    DataTable CMItemSerialDetails = new DataTable();
                    DataTable dtheader = new DataTable();

                    DtblHDetail = ToDtblHDetail(_PackagingJobOrder_Model);
                    DtblReqMatrlDetail = ToDtblRequiredMaterialDetail(_PackagingJobOrder_Model.MaterialRequireddetails);
                    DtblConsumeMatrlDetail = ToDtblConsumeMatrlDetail(_PackagingJobOrder_Model.ConsumeMaterialdetails);

                    DataTable PackingQty_detail = new DataTable();
                    PackingQty_detail.Columns.Add("item_id", typeof(string));
                   
                    PackingQty_detail.Columns.Add("lot_no", typeof(string));
                    PackingQty_detail.Columns.Add("batch_no", typeof(string));
                    PackingQty_detail.Columns.Add("serial_no", typeof(string));
                    PackingQty_detail.Columns.Add("expiry_date", typeof(string));
                   
                    PackingQty_detail.Columns.Add("pkg_Qty", typeof(string));
                    if (_PackagingJobOrder_Model.ItemReworkQtyDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_PackagingJobOrder_Model.ItemReworkQtyDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowDetailsLines = PackingQty_detail.NewRow();
                            dtrowDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                           
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
                         
                            dtrowDetailsLines["pkg_Qty"] = jObjectBatch[i]["ReworkQty"].ToString();
                            PackingQty_detail.Rows.Add(dtrowDetailsLines);
                        }
                    }
                    PackingQtyItemDetails = PackingQty_detail;

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("Comp_Id", typeof(int));
                    Batch_detail.Columns.Add("Br_Id", typeof(int));
                    Batch_detail.Columns.Add("matrlTyp_Id", typeof(string));
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                  
                    Batch_detail.Columns.Add("expiry_date", typeof(string));
                    Batch_detail.Columns.Add("consume_qty", typeof(float));
                    if (_PackagingJobOrder_Model.CMItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_PackagingJobOrder_Model.CMItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["Comp_Id"] = CompID;
                            dtrowBatchDetailsLines["Br_Id"] = BrchID;
                          //  dtrowBatchDetailsLines["matrlTyp_Id"] = jObjectBatch[i]["MaterialtypID"].ToString();
                            dtrowBatchDetailsLines["matrlTyp_Id"] = jObjectBatch[i]["MatrlTypeID"].ToString();
                            dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["ItemId"].ToString();
                            
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                            dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["BatchNo"].ToString();
                           
                            if (jObjectBatch[i]["ExpiryDate"].ToString() == "" || jObjectBatch[i]["ExpiryDate"].ToString() == null)
                            {
                                dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["ExpiryDate"].ToString();
                            }

                            dtrowBatchDetailsLines["consume_qty"] = jObjectBatch[i]["ConsumeQty"].ToString();
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                    }
                    CMItemBatchDetails = Batch_detail;

                    DataTable Serial_detail = new DataTable();

                    Serial_detail.Columns.Add("Comp_Id", typeof(int));
                    Serial_detail.Columns.Add("Br_Id", typeof(int));
                    Serial_detail.Columns.Add("matrlTyp_Id", typeof(string));
                    Serial_detail.Columns.Add("item_id", typeof(string));
              
                    Serial_detail.Columns.Add("lot_no", typeof(string));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("consume_qty", typeof(float));

                    if (_PackagingJobOrder_Model.CMItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_PackagingJobOrder_Model.CMItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["Comp_Id"] = CompID;
                            dtrowSerialDetailsLines["Br_Id"] = BrchID;
                            dtrowSerialDetailsLines["matrlTyp_Id"] = jObjectSerial[i]["MaterialtypID"].ToString();
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                           
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            dtrowSerialDetailsLines["consume_qty"] = jObjectSerial[i]["ConsumedQty"].ToString();
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                    }
                    CMItemSerialDetails = Serial_detail;
                    var hdnJobCmplted = "";
                    if (_PackagingJobOrder_Model.JobCompletion == true)
                    {
                        hdnJobCmplted = _PackagingJobOrder_Model.hdnJobCompletion;
                    }
                    else
                    {
                        hdnJobCmplted = "";
                    }

                    var _JobOrderDetailsattch = TempData["ModelDataattch"] as PJODetailsattch;
                    TempData["ModelDataattch"] = null;
                    DataTable dtAttachment = new DataTable();
                    if (_PackagingJobOrder_Model.attatchmentdetail != null)
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
                            if (_PackagingJobOrder_Model.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _PackagingJobOrder_Model.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(_PackagingJobOrder_Model.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(_PackagingJobOrder_Model.PJO_No))
                                {
                                    dtrowAttachment1["id"] = _PackagingJobOrder_Model.PJO_No;
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

                        if (_PackagingJobOrder_Model.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_PackagingJobOrder_Model.PJO_No))
                                {
                                    ItmCode = _PackagingJobOrder_Model.PJO_No;
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
                        DtblAttchDetail = dtAttachment;
                    }

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    dtSubItem.Columns.Add("Type", typeof(string));
                    dtSubItem.Columns.Add("Req_qty", typeof(string));
                    if (_PackagingJobOrder_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_PackagingJobOrder_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["Type"] = jObject2[i]["subItemTyp"].ToString();
                            dtrowItemdetails["Req_qty"] = jObject2[i]["req_qty"].ToString();
                           
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                    }

                    /*------------------Sub Item end----------------------*/

                    SaveMessage = _PackagingJobOrder_ISERVICES.InsertPackingJO_Details(DtblHDetail, DtblReqMatrlDetail, PackingQtyItemDetails, DtblAttchDetail, DtblConsumeMatrlDetail, CMItemBatchDetails, CMItemSerialDetails, hdnJobCmplted, dtSubItem);

                    string[] Data = SaveMessage.Split(',');

                    string RewrkNo = Data[1];
                    string Rewrk_No = RewrkNo.Replace("/", "");
                    string Message = Data[0];
                    if (Message == "Data_Not_Found")
                    {
                        
                        var msgs = Message.Replace("_", " ") + " " + Rewrk_No + " in " + PageName;//ProdOrdCode is use for table type
                        string path = Server.MapPath("~");
                        Errorlog.LogError_customsg(path, msgs, "", "");
                        _PackagingJobOrder_Model.Message = Message.Split(',')[0].Replace("_", "");
                        return RedirectToAction("PackagingJobOrderDetail");
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
                        comCont.ResetImageLocation(CompID, BrchID, guid, PageName, RewrkNo, _PackagingJobOrder_Model.TransType, dtAttachment);

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
                        _PackagingJobOrder_Model.Message = "Save";

                        _PackagingJobOrder_Model.PJO_No = RewrkNo;
                        _PackagingJobOrder_Model.PJO_Date = RewrkDate;
                        _PackagingJobOrder_Model.TransType = "Update";
                        _PackagingJobOrder_Model.Command = "Update";
                        if (_PackagingJobOrder_Model.JobCompletion == true)
                        {
                            if (StatusCode == "StockNotAvailable")
                            {
                                _PackagingJobOrder_Model.Message = "StockNotAvail";
                                _PackagingJobOrder_Model.BtnName = "BtnRefresh";
                            }
                            else
                            {
                                _PackagingJobOrder_Model.AppStatus = "JC";
                                _PackagingJobOrder_Model.DocumentStatus = "JC";
                                _PackagingJobOrder_Model.BtnName = "BtnSave";
                            }

                        }
                        else
                        {
                            _PackagingJobOrder_Model.AppStatus = "D";
                            _PackagingJobOrder_Model.DocumentStatus = "D";
                            _PackagingJobOrder_Model.BtnName = "BtnSave";
                        }
                      
                        TempData["ModelData"] = _PackagingJobOrder_Model;
                        return RedirectToAction("PackagingJobOrderDetail");

                    }

                }
                else
                {
                    CompDataWithID();
                    _PackagingJobOrder_Model.Created_by = UserID;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    DataSet SaveMessage1 = _PackagingJobOrder_ISERVICES.packingJOCancel(_PackagingJobOrder_Model, CompID, BrchID, mac_id);
                    var Result = SaveMessage1.Tables[0].Rows[0]["result"].ToString();
                    if (Result == "Used")
                    {
                        _PackagingJobOrder_Model.Message = "Used";
                        _PackagingJobOrder_Model.TransType = "Update";
                        _PackagingJobOrder_Model.Command = "Add";
                        _PackagingJobOrder_Model.BtnName = "Refresh";
                        TempData["ModelData"] = _PackagingJobOrder_Model;
                    }
                    else
                    {
                        _Common_IServices.SendAlertEmail(CompID, BrchID, DocumentMenuId, _PackagingJobOrder_Model.PJO_No, "C", UserID, "0");
                        _PackagingJobOrder_Model.Message = "Cancelled";
                        _PackagingJobOrder_Model.Command = "Update";
                        _PackagingJobOrder_Model.PJO_No = _PackagingJobOrder_Model.PJO_No;
                        _PackagingJobOrder_Model.PJO_Date = _PackagingJobOrder_Model.PJO_Date;
                        _PackagingJobOrder_Model.TransType = "Update";
                        _PackagingJobOrder_Model.AppStatus = "D";
                        _PackagingJobOrder_Model.BtnName = "Refresh";
                    }

                    TempData["ModelData"] = _PackagingJobOrder_Model;
                    return RedirectToAction("PackagingJobOrderDetail");
                }
                return RedirectToAction("PackagingJobOrderDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if (_PackagingJobOrder_Model.TransType == "Save")
                    {
                        string Guid = "";
                        if (_PackagingJobOrder_Model.Guid != null)
                        {
                            Guid = _PackagingJobOrder_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        private DataTable ToDtblHDetail(PackagingJobOrder_Model _PackagingJobOrder_Model)
        {
            try
            {
                CompDataWithID();
                DataTable DtblHDetail = new DataTable();
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
            
                dtheader.Columns.Add("PJO_No", typeof(string));
                dtheader.Columns.Add("PJO_Date", typeof(string));
                dtheader.Columns.Add("Item_ID", typeof(string));
                dtheader.Columns.Add("Uom_Id", typeof(string));
                dtheader.Columns.Add("Wh_Id", typeof(string));
              
                dtheader.Columns.Add("pkgQty", typeof(string));
                dtheader.Columns.Add("Shfl_Id", typeof(string));
                dtheader.Columns.Add("WrkStation_Id", typeof(string));
                dtheader.Columns.Add("Supervisor_Name", typeof(string));
                dtheader.Columns.Add("Shift_Id", typeof(string));
                dtheader.Columns.Add("NewBatchNo", typeof(string));
                dtheader.Columns.Add("NewExpdate", typeof(string));

                dtheader.Columns.Add("CompID", typeof(string));
                dtheader.Columns.Add("BranchID", typeof(string));
                dtheader.Columns.Add("UserID", typeof(int));
                dtheader.Columns.Add("PJO_Status", typeof(string));
                dtheader.Columns.Add("SystemDetail", typeof(string));
                dtheader.Columns.Add("src_type", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = _PackagingJobOrder_Model.TransType;
                dtrowHeader["PJO_No"] = _PackagingJobOrder_Model.PJO_No;
                dtrowHeader["PJO_Date"] = _PackagingJobOrder_Model.PJO_Date;
                dtrowHeader["Item_ID"] = _PackagingJobOrder_Model.Item_Id;
                dtrowHeader["Uom_Id"] = _PackagingJobOrder_Model.uom_id;
                dtrowHeader["Wh_Id"] = _PackagingJobOrder_Model.WarehouseID;
          
                dtrowHeader["pkgQty"] = _PackagingJobOrder_Model.pkg_Qty;
                dtrowHeader["Shfl_Id"] = _PackagingJobOrder_Model.Shopfloor_Id;
                dtrowHeader["WrkStation_Id"] = _PackagingJobOrder_Model.WorkstationID;
                dtrowHeader["Supervisor_Name"] = _PackagingJobOrder_Model.supervisor_name;
                dtrowHeader["Shift_Id"] = _PackagingJobOrder_Model.ddl_shift;
                dtrowHeader["NewBatchNo"] = _PackagingJobOrder_Model.Newbatch_No;
                dtrowHeader["NewExpdate"] = _PackagingJobOrder_Model.NewExpiryDate;
                dtrowHeader["src_type"] = _PackagingJobOrder_Model.src_type;

                dtrowHeader["CompID"] = CompID;
                dtrowHeader["BranchID"] = BrchID;
                dtrowHeader["UserID"] = UserID;
                dtrowHeader["PJO_Status"] = IsNull(_PackagingJobOrder_Model.Status_Code, "D");

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["SystemDetail"] = mac_id;
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
     

       
        public ActionResult PJODoubleClickFromList(string DocNo, string DocDate, string ListFilterData, string WF_Status, string ItemIdList, PackagingJobOrder_Model _PackagingJobOrder_Model)
        {/*start Add by Hina on 20-02-2024 to chk Financial year exist or not*/
            CompDataWithID();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            _PackagingJobOrder_Model.Message = "New";
            _PackagingJobOrder_Model.Command = "Update";
            _PackagingJobOrder_Model.TransType = "Update";
            _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
            _PackagingJobOrder_Model.PJO_No = DocNo;
            _PackagingJobOrder_Model.PJO_Date = DocDate;
            _PackagingJobOrder_Model.Item_Id = ItemIdList;
            if (WF_Status != null && WF_Status != "")
            {
                _PackagingJobOrder_Model.WF_Status1 = WF_Status;
            }
            var WF_Status1 = _PackagingJobOrder_Model.WF_Status1;
            var PackingJOCodeURL = DocNo;
            var PackingJoDate = DocDate;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var command = "Add";

            TempData["ModelData"] = _PackagingJobOrder_Model;
            TempData["ListFilterData"] = ListFilterData;

            return (RedirectToAction("PackagingJobOrderDetail", "PackagingJobOrder", new { PackingJOCodeURL = PackingJOCodeURL, PackingJoDate, TransType, BtnName, command, WF_Status1 }));


        }
        public ActionResult DeletePJODetails(PackagingJobOrder_Model _PackagingJobOrder_Model, string command)
        {

            try
            {
                CompDataWithID();
                string br_id = Session["BranchId"].ToString();
                string RJONo = _PackagingJobOrder_Model.PJO_No;
                string RJODelete = _PackagingJobOrder_ISERVICES.PackingJO_DeleteDetail(_PackagingJobOrder_Model, CompID, BrchID);

                if (!string.IsNullOrEmpty(RJONo))
                {
                    CommonPageDetails(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string RJONo1 = RJONo.Replace("/", "");
                    other.DeleteTempFile(CompID + BrchID, PageName, RJONo1, Server);
                }
                _PackagingJobOrder_Model = new PackagingJobOrder_Model();
                _PackagingJobOrder_Model.Message = "Deleted";
                _PackagingJobOrder_Model.Command = "Refresh";
                _PackagingJobOrder_Model.TransType = "Refresh";
                _PackagingJobOrder_Model.BtnName = "BtnDelete";
                TempData["ModelData"] = _PackagingJobOrder_Model;
                return RedirectToAction("PackagingJobOrderDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }
        public ActionResult PJOApprove(PackagingJobOrder_Model _PackagingJobOrder_Model, string ListFilterData1)
        {
            try

            {
                CompDataWithID();
                string MenuID = DocumentMenuId;

                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string PJO_No = _PackagingJobOrder_Model.PJO_No;
                string PJO_Date = _PackagingJobOrder_Model.PJO_Date;
                string A_Status = _PackagingJobOrder_Model.A_Status;
                string A_Level = _PackagingJobOrder_Model.A_Level;
                string A_Remarks = _PackagingJobOrder_Model.A_Remarks;

                string Message = _PackagingJobOrder_ISERVICES.PackingJOApproveDetails(CompID, BrchID, PJO_No, PJO_Date, UserID, MenuID, mac_id, A_Status, A_Level, A_Remarks);
                string ApMessage = Message.Split(',')[2].Trim();
                string PJONo = Message.Split(',')[0].Trim();

                if (ApMessage == "MRS" ||  ApMessage=="A" || ApMessage == "PFC")
                {
                    _PackagingJobOrder_Model.Message = "Approved";
                }
                _Common_IServices.SendAlertEmail(CompID, BrchID, MenuID, PJONo, "AP", UserID, "0");
                _PackagingJobOrder_Model.TransType = "Update";
                _PackagingJobOrder_Model.Command = "Approve";
                _PackagingJobOrder_Model.AppStatus = "D";
                _PackagingJobOrder_Model.BtnName = "BtnEdit";
                var PackingJOCodeURL = PJO_No;
                var PackingJoDate = _PackagingJobOrder_Model.PJO_Date;
                var TransType = _PackagingJobOrder_Model.TransType;
                var BtnName = _PackagingJobOrder_Model.BtnName;
                var command = _PackagingJobOrder_Model.Command;
                TempData["ModelData"] = _PackagingJobOrder_Model;
                TempData["ListFilterData"] = ListFilterData1;
                return (RedirectToAction("PackagingJobOrderDetail", new { PackingJOCodeURL = PackingJOCodeURL, PackingJoDate, TransType, BtnName, command }));


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
            PackagingJobOrder_Model _PackagingJobOrder_Model = new PackagingJobOrder_Model();

            if (AppDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(AppDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _PackagingJobOrder_Model.PJO_No = jObjectBatch[i]["RJONo"].ToString();
                    _PackagingJobOrder_Model.PJO_Date = jObjectBatch[i]["RJODate"].ToString();

                    _PackagingJobOrder_Model.A_Status = jObjectBatch[i]["A_Status"].ToString();
                    _PackagingJobOrder_Model.A_Level = jObjectBatch[i]["A_Level"].ToString();
                    _PackagingJobOrder_Model.A_Remarks = jObjectBatch[i]["A_Remarks"].ToString();

                }
            }
            if (_PackagingJobOrder_Model.A_Status != "Approve")
            {
                _PackagingJobOrder_Model.A_Status = "Approve";
            }
            PJOApprove(_PackagingJobOrder_Model, ListFilterData1);
            if (WF_Status != null && WF_Status != "")
            {
                _PackagingJobOrder_Model.WF_Status1 = WF_Status;
            }
            var WF_Status1 = _PackagingJobOrder_Model.WF_Status1;
            var PackingJOCodeURL = _PackagingJobOrder_Model.PJO_No;
            var PackingJoDate = _PackagingJobOrder_Model.PJO_Date;
            var TransType = _PackagingJobOrder_Model.TransType;
            var BtnName = _PackagingJobOrder_Model.BtnName;
            var command = _PackagingJobOrder_Model.Command;
            return (RedirectToAction("PackagingJobOrderDetail", new { PackingJOCodeURL = PackingJOCodeURL, PackingJoDate, TransType, BtnName, command, WF_Status1 }));


        }
        public ActionResult ToRefreshByJS(string FrwdDtList, string ListFilterData1, string WF_Status)
        {
            PackagingJobOrder_Model _PackagingJobOrder_Model = new PackagingJobOrder_Model();

            if (FrwdDtList != null)
            {
                JArray jObjectBatch = JArray.Parse(FrwdDtList);
                for (int i = 0; i < jObjectBatch.Count; i++)
                {
                    _PackagingJobOrder_Model.PJO_No = jObjectBatch[i]["RJONo"].ToString();
                    _PackagingJobOrder_Model.PJO_Date = jObjectBatch[i]["RJODate"].ToString();
                    _PackagingJobOrder_Model.TransType = "Update";
                    _PackagingJobOrder_Model.BtnName = "BtnToDetailPage";
                    if (WF_Status != null && WF_Status != "")
                    {
                        _PackagingJobOrder_Model.WF_Status1 = WF_Status;
                    }
                    TempData["ModelData"] = _PackagingJobOrder_Model;
                }
            }
            var WF_Status1 = _PackagingJobOrder_Model.WF_Status1;
            var PackingJOCodeURL = _PackagingJobOrder_Model.PJO_No;
            var PackingJoDate = _PackagingJobOrder_Model.PJO_Date;
            var TransType = _PackagingJobOrder_Model.TransType;
            var BtnName = _PackagingJobOrder_Model.BtnName;
            var command = "Refresh";
            TempData["ListFilterData"] = ListFilterData1;
            return (RedirectToAction("PackagingJobOrderDetail", new { PackingJOCodeURL = PackingJOCodeURL, PackingJoDate, TransType, BtnName, command, WF_Status1 }));

        }
        public ActionResult GetPackingJobOrderDashbordList(string docid, string status)
        {
            var WF_Status = status;
            return RedirectToAction("PackagingJobOrder", new { WF_Status });
        }
        private string CheckPackingJobOrderAgainstMRS(PackagingJobOrder_Model _PackagingJobOrder_Model)
        {
            try
            {
                string str = string.Empty;
                CompDataWithID();
                str = _PackagingJobOrder_ISERVICES.ChkPJOagainstMRS(CompID, BrchID, _PackagingJobOrder_Model.PJO_No, _PackagingJobOrder_Model.PJO_Date).Tables[0].Rows[0]["result"].ToString();
                return str;
            }
            catch (Exception Ex)
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
                CompDataWithID();
                DataSet dt = _PackagingJobOrder_ISERVICES.AllDDLBind_OnPageLOad(CompID, BrchID, "0");
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
                CompDataWithID();
                str = _PackagingJobOrder_ISERVICES.GetNewBatchNo(CompID, BrchID).Tables[0].Rows[0]["result"].ToString();
                return str;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }

        }
        [NonAction]
        private DataTable BindWarehouseList()
        {
            try
            {
                CompDataWithID();
                DataSet dt = _PackagingJobOrder_ISERVICES.AllDDLBind_OnPageLOad(CompID, BrchID, "0");
                return dt.Tables[1];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }

        public ActionResult getItemPkgQuantityDetail(string ItemID, string WHID, string Status, string SelectedItemdetail, string hdnCommand, string hdnTranstyp, PackagingJobOrder_Model _PackagingJobOrder_Model,string src_type)
        {
            try
            {
  
                CompDataWithID();
                DataSet ds = new DataSet();
                ds = _PackagingJobOrder_ISERVICES.GetReworkQtyDetails(CompID, BrchID, ItemID, WHID, src_type);
                if(ds.Tables.Count!=0)
                {
                    if (SelectedItemdetail != null && SelectedItemdetail != "")
                    {
                        JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            foreach (JObject item in jObjectBatch.Children())
                            {
                                if (item.GetValue("ItemId").ToString().Trim() == ds.Tables[0].Rows[i]["item_id"].ToString().Trim() && item.GetValue("LotNo").ToString().Trim() == ds.Tables[0].Rows[i]["lot_id"].ToString().Trim() && item.GetValue("BatchNo").ToString().Trim() == ds.Tables[0].Rows[i]["batch_no"].ToString().Trim())
                                {
                                    ds.Tables[0].Rows[i]["PkgQty"] = item.GetValue("ReworkQty");
                                }
                            }
                        }
                    }
                }
               
                ViewBag.DocumentCode = Status;
                ViewBag.DocID = DocumentMenuId;
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.ItemPkgQtyDetail = ds.Tables[0];
                    }
                }
                else
                {
                    ViewBag.ItemPkgQtyDetail = null;
                }
                _PackagingJobOrder_Model.TransType = hdnTranstyp;
                _PackagingJobOrder_Model.Command = hdnCommand;
                _PackagingJobOrder_Model.DocumentStatus = Status;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPkgJobOrderQuantityDetail.cshtml", _PackagingJobOrder_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public ActionResult getItemPkgQuantityDetailAfterInsert(string PJO_No, string PJO_Date, string Status, string ItemId, string WHID, string TransType, string Command, string src_type)
        {
            try
            {
                PackagingJobOrder_Model _PackagingJobOrder_Model = new PackagingJobOrder_Model();
                string shfl_id = string.Empty;
                CompDataWithID();
                DataSet ds = new DataSet();
                if (src_type == "W")
                {
                    ds = _PackagingJobOrder_ISERVICES.getItemReworkQtyAfterInsert(CompID, BrchID, PJO_No, PJO_Date, ItemId, WHID);
                }
                else
                {
                    shfl_id = WHID;
                    ds = _PackagingJobOrder_ISERVICES.getItempkgQtyAfterInsert(CompID, BrchID, PJO_No, PJO_Date, ItemId, shfl_id);
                }
                if (ds.Tables.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.ItemPkgQtyDetail = ds.Tables[0];
                    }
                }
                else
                {
                    ViewBag.ItemPkgQtyDetail = null;
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentCode = Status;
                //ViewBag.TransType = TransType;
                //ViewBag.Command = Command;
                _PackagingJobOrder_Model.TransType = TransType;
                _PackagingJobOrder_Model.Command = Command;
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPkgJobOrderQuantityDetail.cshtml", _PackagingJobOrder_Model);

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [NonAction]
        private DataTable BindShopfloorList()
        {
            try
            {
                CompDataWithID();
                   DataSet dt = _PackagingJobOrder_ISERVICES.AllDDLBind_OnPageLOad(CompID, BrchID, "0");
                return dt.Tables[2];
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }

        }

        [HttpPost]
        public ActionResult BindWorkStationList(PackagingJobOrder_Model _PackagingJobOrder_Model, int shfl_id)
        {
            JsonResult DataRows = null;
            string product_id = string.Empty;        
            try
            {
                if (Session["CompId"] != null)
                {
                    CompDataWithID();
                    if (string.IsNullOrEmpty(_PackagingJobOrder_Model.Item_Id))
                    {
                        product_id = "0";
                    }
                    else
                    {
                        product_id = _PackagingJobOrder_Model.Item_Name;
                    }
                    DataSet ProductList = _PackagingJobOrder_ISERVICES.GetWorkStationDAL(CompID, BrchID, shfl_id);
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

        public JsonResult BindPkgWHAvalStk(string ItemID, string WarehouseID,string src_type)
        {
            JsonResult DataRows = null;
            try
            {
                CompDataWithID();
                DataSet Deatils = _PackagingJobOrder_ISERVICES.GetRewrkWHAvalStk(CompID, BrchID, ItemID, WarehouseID, src_type);
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
                CompDataWithID();
                DataSet Deatils = _PackagingJobOrder_ISERVICES.GetMaterialNameByMtrlTyp(CompID, BrchID, ddl_MaterialTyp);
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

        public ActionResult GetItemList(PackagingJobOrder_Model queryParameters, string ddl_MaterialTyp, string ddl_HedrItemId)
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
                DataSet ItemList1 = _PackagingJobOrder_ISERVICES.ItemList(ItemName, CompID, BrchID, ddl_MaterialTyp, ddl_HedrItemId);

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
                CompDataWithID();
                DataSet result = _PackagingJobOrder_ISERVICES.GetItemUOM(CompID, BrchID, MaterialID);
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
        public JsonResult GetDetailsOfRequiredMaterialTbl(string RJO_No, string RJO_Date, string ShopfloorId)
        {
            JsonResult DataRows = null;
            try
            {
                CompDataWithID();
                DataSet Deatils = _PackagingJobOrder_ISERVICES.GetDetailsOfRequiredMaterialTbl(CompID, BrchID, RJO_No, RJO_Date, ShopfloorId);

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
        public ActionResult getCMItemStockBatchWiseBYShpfloor(string ItemId, string ShpfloorId, string Status, string SelectedItemdetail, string docid, string TransType, string Command)
        {
            try
            {
                DataSet ds = new DataSet();
                CompDataWithID();
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }

                ds = _PackagingJobOrder_ISERVICES.getCMItemStockBatchSerialWise(ItemId, ShpfloorId, CompID, BrchID);

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
        public ActionResult getCMItemStockBatchWiseAfterInsert(string RJO_No, string RJO_Date, string Status, string MtrlTypId, string ItemId, string docid, string TransType, string Command)
        {
            try
            {
                DataSet ds = new DataSet();              
                string Type = string.Empty;

                CompDataWithID();

                ds = _PackagingJobOrder_ISERVICES.getCMStockBatchWiseAfterInsert(CompID, BrchID, RJO_No, RJO_Date, MtrlTypId, ItemId);

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
                CompDataWithID();
                if (docid != null && docid != "")
                {
                    DocumentMenuId = docid;
                }
                //ds = _Common_IServices.getItemstockSerialWise(ItemId, ShpfloorId, CompID, BrchID);
                ds = _PackagingJobOrder_ISERVICES.getCMItemStockBatchSerialWise(ItemId, ShpfloorId, CompID, BrchID);
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
                string PL_Type = string.Empty;
                CompDataWithID();
                ds = _PackagingJobOrder_ISERVICES.getCMStockSerialWiseAfterInsert(CompID, BrchID, RJO_No, RJO_Date, MtrlTypId, ItemId);
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
                PJODetailsattch _RJODetail = new PJODetailsattch();
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
                CompDataWithID();
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
            , string Flag, string Status, string Doc_no, string Doc_dt, string Wh_id, string Shfl_Id,string src_type,string matrial_type)
        {
            try
            {
                var flag1 = string.Empty;
                CompDataWithID();
                DataTable dt = new DataTable();
                if (Flag == "RJO_RewrkQuantity" || Flag == "RM_RequireQty")
                {
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        if (Flag == "RJO_RewrkQuantity")
                        {
                            if(src_type=="W")
                            {
                                flag1 = "wh";
                                dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID, BrchID, Wh_id, Item_id, null/*UomId*/, flag1).Tables[0];
                            }
                            if (src_type == "S")
                            {
                                flag1 = "shfl";
                                dt = _PackagingJobOrder_ISERVICES.GetSubItemWhAvlstockDetails(CompID, BrchID, Wh_id, Item_id, null/*UomId*/, flag1).Tables[0];
                            }
                            
                            dt.Columns.Add("Qty", typeof(string));
                        }
                        else
                        {
                            if (matrial_type != "SR")
                            {
                                Wh_id = Shfl_Id;
                                flag1 = "shfl";
                                dt = _PackagingJobOrder_ISERVICES.GetSubItemWhAvlstockDetails(CompID, BrchID, Wh_id, Item_id, null/*UomId*/, flag1).Tables[0];
                                dt.Columns.Add("Qty", typeof(string));
                              //  Flag = "RJO_RewrkQuantity";
                            }
                            else
                            {
                                 dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                                Flag = "Service_RM_RequireQty";
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
                        dt = _PackagingJobOrder_ISERVICES.RJO_GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, Shfl_Id).Tables[0];
                    }
                }
                if (Flag == "CM_ConsumeQty")
                {
                    if (Status == "I" || Status == "PFC")
                    {
                        dt = _PackagingJobOrder_ISERVICES.GetCMSubItemShflAvlstockDetails(CompID, BrchID, Doc_no, Doc_dt, Shfl_Id, Item_id, "br").Tables[0];
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
                        dt = _PackagingJobOrder_ISERVICES.RJO_GetSubItemDetailsAfterApprove(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, Shfl_Id).Tables[0];
                        IsDisabled = "Y";
                    }

                }
                if (Flag == "RJO_QCAccptQty" || Flag == "RJO_QCRejQty" || Flag == "RJO_QCRwkQty")
                {
                    if (Status == "QC" || Status == "QP")
                    {
                        dt = _PackagingJobOrder_ISERVICES.QCAcptRejRewkQty_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];

                    }

                }
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
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

                CompDataWithID();              
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
    }

}