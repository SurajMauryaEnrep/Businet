using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue.MaterialTransferIssue;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialIssue.MaterialTransferIssue;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialIssue;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.TransporterSetup;
using EnRepMobileWeb.Areas.BusinessLayer.Controllers.TransporterSetup;
using EnRepMobileWeb.MODELS.BusinessLayer.TransporterSetup;
using System.Configuration;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialIssue.MaterialTransferIssue
{
    public class MaterialTransferIssueController : Controller
    {
        string DocumentMenuId = "105102130103";
        string FromDate, title;
        List<MTIList> _MTIList;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        CommonController cmn = new CommonController();
        string CompID, BrchID, UserID, BranchName, ship_no, userid, language = String.Empty;
        MaterialTransferIssue_ISERVICES _MTI_ISERVICES;
        MaterialIssue_IServices _MaterialIssue_IServices;
        MTIList_ISERVICES _MTIList_ISERVICES;
        DataTable dt;
        DataSet dtSet;
        MaterialTransferIssueModel _MTIModel;
        private readonly TransporterSetup_ISERVICES _trptIService;
        public MaterialTransferIssueController(Common_IServices _Common_IServices, MaterialTransferIssue_ISERVICES _MTI_ISERVICES, 
            MTIList_ISERVICES _MTIList_ISERVICES, MaterialIssue_IServices _MaterialIssue_IServices, TransporterSetup_ISERVICES trptIService)
        {
            this._MaterialIssue_IServices = _MaterialIssue_IServices;
            this._Common_IServices = _Common_IServices;
            this._MTI_ISERVICES = _MTI_ISERVICES;
            this._MTIList_ISERVICES = _MTIList_ISERVICES;
            this._trptIService = trptIService;
        }
        // GET: ApplicationLayer/MaterialTransferIssue
       
        public ActionResult MaterialTransferIssue()
        {
            MaterialTransferIssueListModel _MTIListModel = new MaterialTransferIssueListModel();

            CommonPageDetails();
            //List<Status> statusLists = new List<Status>();
            //var other = new CommonController(_Common_IServices);
            //var statusListsC = other.GetStatusList1(DocumentMenuId);
            //var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
            //_MTIListModel.StatusList = listOfStatus;

            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            _MTIListModel.StatusList = statusLists;
            /*commented by Hina on 22-03-2024 to combine all list Procedure  in single Procedure*/
            //List<ToBranchList> _ToBranchList = new List<ToBranchList>();
            //dt = GetToBranchList();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    ToBranchList _ToBranch = new ToBranchList();
            //    _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
            //    _ToBranch.br_val = dr["comp_nm"].ToString();
            //    _ToBranchList.Add(_ToBranch);
            //}
            //_MTIListModel.ToBranchList = _ToBranchList;

            //List<ToWharehouseList> _ToWharehouseList = new List<ToWharehouseList>();

            //BrchID = Session["BranchId"].ToString();
            //dt = GetToWHList(BrchID);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    ToWharehouseList _ToWharehouse = new ToWharehouseList();
            //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
            //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
            //    _ToWharehouseList.Add(_ToWharehouse);
            //}
            //_MTIListModel.ToWharehouseList = _ToWharehouseList;
            var flag = "ListPage";
            //var PageName = "MTReceipt";

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;
            dtSet = MTI_GetAllDDLListAndListPageData(flag, startDate, CurrentDate);

            List<ToBranchList> _ToBranchList = new List<ToBranchList>();

            foreach (DataRow dr in dtSet.Tables[1].Rows)
            {
                ToBranchList _ToBranch = new ToBranchList();
                _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                _ToBranch.br_val = dr["comp_nm"].ToString();
                _ToBranchList.Add(_ToBranch);
            }
            _ToBranchList.Insert(0, new ToBranchList() { br_id = 0, br_val = "---Select---" });/*Add by Hina on 13-09-2024*/
            _MTIListModel.ToBranchList = _ToBranchList;

            List<ToWharehouseList> _ToWharehouseList = new List<ToWharehouseList>();
            foreach (DataRow dr in dtSet.Tables[0].Rows)
            {
                ToWharehouseList _ToWharehouse = new ToWharehouseList();
                _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                _ToWharehouse.wh_val = dr["wh_name"].ToString();
                _ToWharehouseList.Add(_ToWharehouse);
            }
            _ToWharehouseList.Insert(0, new ToWharehouseList() { wh_id = 0, wh_val = "---Select---" });/*Add by Hina on 13-09-2024*/
            _MTIListModel.ToWharehouseList = _ToWharehouseList;


            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            //string endDate = dtnow.ToString("yyyy-MM-dd");

            if (TempData["ListFilterData"] != null)
            {
                var PRData = TempData["ListFilterData"].ToString();
                var a = PRData.Split(',');
                _MTIListModel.to_wh =Convert.ToInt32(a[0].Trim());
                _MTIListModel.to_br =Convert.ToInt32(a[1].Trim());
                _MTIListModel.MTO_FromDate = a[2].Trim();
                _MTIListModel.MTO_ToDate = a[3].Trim();
                _MTIListModel.TRF_Type= a[4].Trim();
                _MTIListModel.MTIStatus = a[5].Trim();
                if (_MTIListModel.MTIStatus == "0")
                {
                    _MTIListModel.MTIStatus = null;
                }
                _MTIListModel.FromDate = _MTIListModel.MTO_FromDate;
                _MTIListModel.ListFilterData = TempData["ListFilterData"].ToString();
                /*Uncommented By Shubham Maurya on 30-03-2024 for after search data*/
                _MTIListModel.BindMTIList = GetMTIDetailList(_MTIListModel);
            }
            else
            {
                /*commented by Hina on 22-03-2024 to combine all list Procedure  in single Procedure*/
                //_MTIListModel.BindMTIList = GetMTIDetailList(_MTIListModel);
                _MTIList = new List<MTIList>();
                if (dtSet.Tables[2].Rows.Count > 0)
                {

                    foreach (DataRow dr in dtSet.Tables[2].Rows)
                    {
                        MTIList _TempMTIList = new MTIList();
                        _TempMTIList.MTINo = dr["issue_no"].ToString();
                        _TempMTIList.MTIDate = dr["issue_dt"].ToString();
                        _TempMTIList.issue_date = dr["issue_date"].ToString().Trim();
                        _TempMTIList.TRFType = dr["TRFType"].ToString();
                        _TempMTIList.trfType = dr["trf_type"].ToString();
                        _TempMTIList.ReqNo = dr["req_no"].ToString();
                        _TempMTIList.ReqDate = dr["req_dt"].ToString();
                        _TempMTIList.FromWH = dr["fromwh"].ToString();
                        _TempMTIList.ToBranch = dr["tobr"].ToString();
                        _TempMTIList.ToWH = dr["towh"].ToString();
                        _TempMTIList.MTIList_Stauts = dr["Status"].ToString();
                        _TempMTIList.CreateDate = dr["CreateDate"].ToString();
                        _TempMTIList.create_by = dr["create_by"].ToString();
                        _TempMTIList.mod_by = dr["mod_by"].ToString();
                        _MTIList.Add(_TempMTIList);
                    }
                }
                _MTIListModel.BindMTIList = _MTIList;
                _MTIListModel.FromDate = startDate;
            }
           
            //ViewBag.MenuPageName = getDocumentName();
            _MTIListModel.Title = title;
            //Session["MTISearch"] = "0";
            _MTIListModel.MTISearch = "0";
            //ViewBag.VBRoleList = GetRoleList();
            ViewBag.DocumentMenuId = DocumentMenuId;
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/MaterialTransferIssue/MaterialTransferIssueList.cshtml", _MTIListModel);
        }
        //private DataTable GetRoleList()
        //{
        //    try
        //    {
        //        if (Session["CompId"] != null)
        //        {
        //            CompID = Session["CompId"].ToString();
        //        }
        //        if (Session["userid"] != null)
        //        {
        //            userid = Session["userid"].ToString();
        //        }
        //        DataTable RoleList = _Common_IServices.GetRole_List(CompID, userid, DocumentMenuId);

        //        return RoleList;
        //    }
        //    catch (Exception ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, ex);
        //        return null;
        //    }
        //}
        public ActionResult AddMaterialTransferIssueDetail()
        {
            MaterialTransferIssueModel _MTIModel = new MaterialTransferIssueModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            _MTIModel.Message = "New";
            _MTIModel.Command = "Add";
            _MTIModel.AppStatus = "D";
            _MTIModel.TransType = "Save";
            _MTIModel.BtnName = "BtnAddNew";
            TempData["ModelData"] = _MTIModel;
            URLDetailModel URLModel = new URLDetailModel();         
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnAddNew";
            URLModel.TransType = "Save";
            // URLModel.AppStatus = "D";
            //ViewBag.MenuPageName = getDocumentName();
            CommonPageDetails();
            TempData["ListFilterData"] = null;
            /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("MaterialTransferIssue");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("MaterialTransferIssueDetail", "MaterialTransferIssue", URLModel);           
        }
        public List<TransListModel> GetTransporterList()
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            var transController = new TransporterSetupController(_Common_IServices, _trptIService);
            var transList = transController.GetTransporterList(CompID);
            transList.RemoveAt(0);
            transList.Insert(0, new TransListModel { TransId = "0", TransName = "---Select--" });
            return transList;
        }
        public ActionResult MaterialTransferIssueDetail(URLDetailModel URLModel)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["BranchName"] != null)
            {
                BranchName = Session["BranchName"].ToString();
            }
            /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, URLModel.MTI_Date) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
                var Modeldata = TempData["ModelData"] as MaterialTransferIssueModel;
                if (Modeldata != null)
                {
                    CommonPageDetails();
                    Modeldata.MaterialIssueDate = DateTime.Now;                 
                    Modeldata.from_br = BranchName;
                    /*commented by Hina on 22-03-2024 to combine all list Procedure  in single Procedure*/
                    //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    //dt = GetFromWHList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    FromWharehouse _warehouse = new FromWharehouse();
                    //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _warehouse.wh_val = dr["wh_name"].ToString();
                    //    _warehouseList.Add(_warehouse);
                    //}
                    //Modeldata.FromWharehouseList = _warehouseList;

                    //List<ToBranch> _ToBranchList = new List<ToBranch>();
                    //dt = GetToBranchList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ToBranch _ToBranch = new ToBranch();
                    //    _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    //    _ToBranch.br_val = dr["comp_nm"].ToString();
                    //    _ToBranchList.Add(_ToBranch);
                    //}
                    //Modeldata.ToBranchList = _ToBranchList;
                   
                    dtSet = MTI_GetAllDDLListAndListPageData("","","");
                    List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    foreach (DataRow dr in dtSet.Tables[0].Rows)
                    {
                        FromWharehouse _warehouse = new FromWharehouse();
                        _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _warehouse.wh_val = dr["wh_name"].ToString();
                        _warehouseList.Add(_warehouse);
                    }
                    _warehouseList.Insert(0, new FromWharehouse() { wh_id = 0, wh_val = "---Select---" });
                    Modeldata.FromWharehouseList = _warehouseList;

                    List<ToBranch> _ToBranchList = new List<ToBranch>();
                    _ToBranchList.Insert(0, new ToBranch() { br_id = 0, br_val = "---Select---" });
                    foreach (DataRow dr in dtSet.Tables[1].Rows)
                    {
                        ToBranch _ToBranch = new ToBranch();
                        _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                        _ToBranch.br_val = dr["comp_nm"].ToString();
                        _ToBranchList.Add(_ToBranch);
                    }
                    Modeldata.ToBranchList = _ToBranchList;

                    //List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                    //dt = GetToWHList(BrchID);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ToWharehouse _ToWharehouse = new ToWharehouse();
                    //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                    //    _ToWharehouseList.Add(_ToWharehouse);
                    //}
                    //Modeldata.ToWharehouseList = _ToWharehouseList;
                    List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                    _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
                    Modeldata.ToWharehouseList = _ToWharehouseList;
                    Modeldata.from_brid = BrchID;

                    Modeldata.TransList = GetTransporterList();

                    List<Req_NO> _Req_NOList = new List<Req_NO>();
                    Req_NO _Req_NO = new Req_NO();
                    _Req_NO.RequirementDate = "0";
                    _Req_NO.RequirementNo = "---Select---";
                    _Req_NOList.Add(_Req_NO);
                    Modeldata.Req_NO_List = _Req_NOList;

                    Modeldata.CompId = CompID;
                    Modeldata.BrchID = BrchID;
                    Modeldata.Title = title;
                    Modeldata.Req_Date = null;
                    Modeldata.MTI_Type = Modeldata.MTI_Type;
                    Modeldata.trf_type = Modeldata.MTI_Type;
                    Modeldata.hdtrf_type = Modeldata.MTI_Type;
                    if (TempData["ListFilterData"] != null)
                    {
                        Modeldata.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (Modeldata.TransType == "Update" || Modeldata.TransType == "Edit")
                    {
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }

                        BrchID = Session["BranchId"].ToString();
                        //string MTI_no = Session["MTI_Number"].ToString();
                        //string trf_type = Session["MTI_Type"].ToString();
                        //string MTI_date = Session["MTI_Date"].ToString();
                        string MTI_no = Modeldata.MTI_Number;
                        string MTI_date = Modeldata.MTI_Date;
                        DataSet ds = _MTI_ISERVICES.GetMTODetailByNo(CompID, BrchID, MTI_no, MTI_date);

                        List<Req_NO> _Req_NOList1 = new List<Req_NO>();
                        Req_NO _Req_NO1 = new Req_NO();
                        _Req_NO1.RequirementDate = ds.Tables[0].Rows[0]["req_dt"].ToString();
                        _Req_NO1.RequirementNo = ds.Tables[0].Rows[0]["req_no"].ToString();
                        _Req_NOList1.Add(_Req_NO1);
                        Modeldata.Req_NO_List = _Req_NOList1;

                        Modeldata.MaterialIssueNo = ds.Tables[0].Rows[0]["issue_no"].ToString();
                        Modeldata.MaterialIssueDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["issue_dt"].ToString());
                        Modeldata.MTI_Type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        Modeldata.trf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        Modeldata.from_br = ds.Tables[0].Rows[0]["frombr"].ToString();
                        Modeldata.from_brid = ds.Tables[0].Rows[0]["src_br"].ToString();
                        Modeldata.from_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["src_wh"].ToString());
                        Modeldata.to_br = Convert.ToInt32(ds.Tables[0].Rows[0]["dstn_br"].ToString());
                        Modeldata.to_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["dstn_wh"].ToString());
                        Modeldata.hdto_WhName = ds.Tables[0].Rows[0]["dstn_whname"].ToString();
                        Modeldata.issue_rem = ds.Tables[0].Rows[0]["issue_rem"].ToString();
                        Modeldata.Req_No = ds.Tables[0].Rows[0]["req_no"].ToString();
                        Modeldata.Req_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["req_dt"].ToString());
                        Modeldata.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        Modeldata.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        Modeldata.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        Modeldata.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        Modeldata.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        Modeldata.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        Modeldata.trf_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        Modeldata.StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                        Modeldata.GR_No = ds.Tables[0].Rows[0]["gr_no"].ToString();
                        Modeldata.GR_Dt = ds.Tables[0].Rows[0]["gr_date"].ToString();
                        Modeldata.HdnGRDate = ds.Tables[0].Rows[0]["gr_date"].ToString();
                        Modeldata.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_id"].ToString();
                        Modeldata.Veh_Number = ds.Tables[0].Rows[0]["veh_number"].ToString();
                        Modeldata.Driver_Name = ds.Tables[0].Rows[0]["driver_name"].ToString();
                        Modeldata.Mob_No = ds.Tables[0].Rows[0]["mob_no"].ToString();
                        Modeldata.Tot_Tonnage = ds.Tables[0].Rows[0]["tot_tonnage"].ToString();
                        Modeldata.No_Of_Packages = ds.Tables[0].Rows[0]["no_of_pkgs"].ToString();

                        List<ToWharehouse> _ToWharehouseList1 = new List<ToWharehouse>();
                        _ToWharehouseList1.Insert(0, new ToWharehouse() { wh_id = Modeldata.to_wh, wh_val = Modeldata.hdto_WhName });
                        Modeldata.ToWharehouseList = _ToWharehouseList1;

                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            Modeldata.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            Modeldata.BtnName = "Refresh"; ;
                        }
                        else
                        {
                            Modeldata.CancelFlag = false;
                        }
                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                        Modeldata.DocumentStatus = ds.Tables[0].Rows[0]["status_code"].ToString();
                        //ViewBag.MenuPageName = getDocumentName();
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.ItemStockBatchWise = ds.Tables[2];
                        ViewBag.ItemStockSerialWise = ds.Tables[3];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        Modeldata.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        Modeldata.DocumentMenuId = DocumentMenuId;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        Modeldata.CMN_Command = Modeldata.Command;
                      
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/MaterialTransferIssue/MaterialTransferIssueDetail.cshtml", Modeldata);
                    }
                    else
                    {

                        //ViewBag.MenuPageName = getDocumentName();
                        //Session["DocumentStatus"] = "D";
                        Modeldata.DocumentStatus = "D";
                        Modeldata.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        Modeldata.DocumentMenuId = DocumentMenuId;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        Modeldata.CMN_Command = Modeldata.Command;                   
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/MaterialTransferIssue/MaterialTransferIssueDetail.cshtml", Modeldata);
                    }
                }
                else
                {
                    MaterialTransferIssueModel _MTIModel = new MaterialTransferIssueModel();
                    if (URLModel.MTI_Number != null || URLModel.MTI_Date != null)
                    {
                        _MTIModel.MTI_Number = URLModel.MTI_Number;
                        _MTIModel.MTI_Date = URLModel.MTI_Date;
                    }
                    if (URLModel.TransType != null)
                    {
                        _MTIModel.TransType = URLModel.TransType;
                    }
                    else
                    {
                        _MTIModel.TransType = "New";
                    }
                    if (URLModel.BtnName != null)
                    {
                        _MTIModel.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        _MTIModel.BtnName = "BtnRefresh";
                    }
                    if (URLModel.Command != null)
                    {
                        _MTIModel.Command = URLModel.Command;
                    }
                    else
                    {
                        _MTIModel.Command = "Refresh";
                    }                       
                    _MTIModel.MaterialIssueDate = DateTime.Now;                 
                    _MTIModel.from_br = BranchName;
                    CommonPageDetails();
                    /*commented by Hina on 22-03-2024 to combine all list Procedure  in single Procedure*/
                    //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    //dt = GetFromWHList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    FromWharehouse _warehouse = new FromWharehouse();
                    //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _warehouse.wh_val = dr["wh_name"].ToString();
                    //    _warehouseList.Add(_warehouse);
                    //}
                    //_MTIModel.FromWharehouseList = _warehouseList;

                    //List<ToBranch> _ToBranchList = new List<ToBranch>();
                    //dt = GetToBranchList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ToBranch _ToBranch = new ToBranch();
                    //    _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    //    _ToBranch.br_val = dr["comp_nm"].ToString();
                    //    _ToBranchList.Add(_ToBranch);
                    //}
                    //_MTIModel.ToBranchList = _ToBranchList;

                    dtSet = MTI_GetAllDDLListAndListPageData("","","");
                    List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    foreach (DataRow dr in dtSet.Tables[0].Rows)
                    {
                        FromWharehouse _warehouse = new FromWharehouse();
                        _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                        _warehouse.wh_val = dr["wh_name"].ToString();
                        _warehouseList.Add(_warehouse);
                    }
                    _warehouseList.Insert(0, new FromWharehouse() { wh_id = 0, wh_val = "---Select---" });
                    _MTIModel.FromWharehouseList = _warehouseList;

                    List<ToBranch> _ToBranchList = new List<ToBranch>();
                    _ToBranchList.Insert(0, new ToBranch() { br_id = 0, br_val = "---Select---" });
                    foreach (DataRow dr in dtSet.Tables[1].Rows)
                    {
                        ToBranch _ToBranch = new ToBranch();
                        _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                        _ToBranch.br_val = dr["comp_nm"].ToString();
                        _ToBranchList.Add(_ToBranch);
                    }
                    _MTIModel.ToBranchList = _ToBranchList;

                    //List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                    //dt = GetToWHList(BrchID);
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    ToWharehouse _ToWharehouse = new ToWharehouse();
                    //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                    //    _ToWharehouseList.Add(_ToWharehouse);
                    //}
                    //_MTIModel.ToWharehouseList = _ToWharehouseList;
                    List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                    _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
                    _MTIModel.ToWharehouseList = _ToWharehouseList;
                    _MTIModel.from_brid = BrchID;

                    _MTIModel.TransList = GetTransporterList();

                    List<Req_NO> _Req_NOList = new List<Req_NO>();
                    Req_NO _Req_NO = new Req_NO();
                    _Req_NO.RequirementDate = "0";
                    _Req_NO.RequirementNo = "---Select---";
                    _Req_NOList.Add(_Req_NO);
                    _MTIModel.Req_NO_List = _Req_NOList;

                    _MTIModel.CompId = CompID;
                    _MTIModel.BrchID = BrchID;
                    _MTIModel.Title = title;
                    _MTIModel.Req_Date = null;
                    if (TempData["ListFilterData"] != null)
                    {
                        _MTIModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }

                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_MTIModel.TransType == "Update" || _MTIModel.TransType == "Edit")
                    {
                        if (Session["CompId"] != null)
                        {
                            CompID = Session["CompId"].ToString();
                        }

                        BrchID = Session["BranchId"].ToString();
                        //string MTI_no = Session["MTI_Number"].ToString();
                        //string trf_type = Session["MTI_Type"].ToString();
                        //string MTI_date = Session["MTI_Date"].ToString();
                        string MTI_no = _MTIModel.MTI_Number;
                        string MTI_date = _MTIModel.MTI_Date;
                        DataSet ds = _MTI_ISERVICES.GetMTODetailByNo(CompID, BrchID, MTI_no, MTI_date);

                        List<Req_NO> _Req_NOList1 = new List<Req_NO>();
                        Req_NO _Req_NO1 = new Req_NO();
                        _Req_NO1.RequirementDate = ds.Tables[0].Rows[0]["req_dt"].ToString();
                        _Req_NO1.RequirementNo = ds.Tables[0].Rows[0]["req_no"].ToString();
                        _Req_NOList1.Add(_Req_NO1);
                        _MTIModel.Req_NO_List = _Req_NOList1;

                        _MTIModel.MaterialIssueNo = ds.Tables[0].Rows[0]["issue_no"].ToString();
                        _MTIModel.MaterialIssueDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["issue_dt"].ToString());
                        _MTIModel.MTI_Type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        _MTIModel.trf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        _MTIModel.hdtrf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        _MTIModel.from_br = ds.Tables[0].Rows[0]["frombr"].ToString();
                        _MTIModel.from_brid = ds.Tables[0].Rows[0]["src_br"].ToString();
                        _MTIModel.from_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["src_wh"].ToString());
                        _MTIModel.to_br = Convert.ToInt32(ds.Tables[0].Rows[0]["dstn_br"].ToString());
                        _MTIModel.to_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["dstn_wh"].ToString());
                        _MTIModel.hdto_WhName = ds.Tables[0].Rows[0]["dstn_whname"].ToString();
                        _MTIModel.issue_rem = ds.Tables[0].Rows[0]["issue_rem"].ToString();
                        _MTIModel.Req_No = ds.Tables[0].Rows[0]["req_no"].ToString();
                        _MTIModel.Req_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["req_dt"].ToString());
                        _MTIModel.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _MTIModel.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _MTIModel.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _MTIModel.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _MTIModel.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _MTIModel.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _MTIModel.trf_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _MTIModel.StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();

                        _MTIModel.GR_No = ds.Tables[0].Rows[0]["gr_no"].ToString();
                        _MTIModel.GR_Dt = ds.Tables[0].Rows[0]["gr_date"].ToString();
                        _MTIModel.HdnGRDate = ds.Tables[0].Rows[0]["gr_date"].ToString();
                        _MTIModel.Transpt_NameID = ds.Tables[0].Rows[0]["trpt_id"].ToString();
                        _MTIModel.Veh_Number = ds.Tables[0].Rows[0]["veh_number"].ToString();
                        _MTIModel.Driver_Name = ds.Tables[0].Rows[0]["driver_name"].ToString();
                        _MTIModel.Mob_No = ds.Tables[0].Rows[0]["mob_no"].ToString();
                        _MTIModel.Tot_Tonnage = ds.Tables[0].Rows[0]["tot_tonnage"].ToString();
                        _MTIModel.No_Of_Packages = ds.Tables[0].Rows[0]["no_of_pkgs"].ToString();

                        List<ToWharehouse> _ToWharehouseList1 = new List<ToWharehouse>();
                        _ToWharehouseList1.Insert(0, new ToWharehouse() { wh_id = _MTIModel.to_wh, wh_val = _MTIModel.hdto_WhName });
                        _MTIModel.ToWharehouseList = _ToWharehouseList1;

                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            _MTIModel.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            _MTIModel.BtnName = "Refresh"; ;
                        }
                        else
                        {
                            _MTIModel.CancelFlag = false;
                        }
                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString();
                        _MTIModel.DocumentStatus = ds.Tables[0].Rows[0]["status_code"].ToString();
                        //ViewBag.MenuPageName = getDocumentName();
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.ItemStockBatchWise = ds.Tables[2];
                        ViewBag.ItemStockSerialWise = ds.Tables[3];
                        ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _MTIModel.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        _MTIModel.DocumentMenuId = DocumentMenuId;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        _MTIModel.CMN_Command = _MTIModel.Command;
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/MaterialTransferIssue/MaterialTransferIssueDetail.cshtml", _MTIModel);
                    }
                    else
                    {
                       // ViewBag.MenuPageName = getDocumentName();
                        //Session["DocumentStatus"] = "D";
                        _MTIModel.DocumentStatus = "D";
                        _MTIModel.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        _MTIModel.DocumentMenuId = DocumentMenuId;
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        _MTIModel.CMN_Command = _MTIModel.Command;
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/MaterialTransferIssue/MaterialTransferIssueDetail.cshtml", _MTIModel);
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult EditMTI(string MTINO,string MTI_Date, string MTI_Type,string ListFilterData)
        {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
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
            //Session["Message"] = "New";
            //Session["Command"] = "Add";          
            //Session["MTI_Number"] = MTINO.Trim(); 
            //Session["MTI_Type"] = MTI_Type.Trim(); 
            //Session["MTI_Date"] = MTI_Date.Trim();
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            MaterialTransferIssueModel _MTIModel = new MaterialTransferIssueModel();
            _MTIModel.Message = "New";
            _MTIModel.Command = "Add";
            _MTIModel.MTI_Number = MTINO.Trim();
            _MTIModel.MTI_Type = MTI_Type.Trim();
            _MTIModel.hdtrf_type = MTI_Type.Trim();
            _MTIModel.trf_type = MTI_Type.Trim();
            _MTIModel.MTI_Date = MTI_Date.Trim();
            _MTIModel.TransType = "Update";
            _MTIModel.AppStatus = "D";
            _MTIModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = _MTIModel;
            URLDetailModel URLModel = new URLDetailModel();
            URLModel.MTI_Type = _MTIModel.MTI_Type;
           
            URLModel.MTI_Number = MTINO.Trim();
            URLModel.MTI_Date = MTI_Date.Trim();
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnToDetailPage";
            URLModel.TransType = "Update";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("MaterialTransferIssueDetail", "MaterialTransferIssue", URLModel);
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
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //        return null;
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MaterialTransferIssueSave(MaterialTransferIssueModel _MTIModel, string command)
        {
            try
            {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (_MTIModel.DeleteCommand == "Delete")
                    if (true)
                    {
                        command = "Delete";
                    }
                switch (command)
                {
                    case "AddNew":
                        MaterialTransferIssueModel _MTIModelAddNew = new MaterialTransferIssueModel();
                        _MTIModelAddNew.AppStatus = "D";
                        _MTIModelAddNew.BtnName = "BtnAddNew";
                        _MTIModelAddNew.TransType = "Save";
                        _MTIModelAddNew.Command = "New";
                        //_MTIModelAddNew.DocumentMenuId = DocumentMenuId;
                        TempData["ModelData"] = _MTIModelAddNew;
                        URLDetailModel URLModel = new URLDetailModel();                 
                        URLModel.Command = "Add";
                        URLModel.BtnName = "BtnAddNew";
                        URLModel.TransType = "Save";
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                            if (!string.IsNullOrEmpty(_MTIModel.MaterialIssueNo))
                                return RedirectToAction("EditMTI", new { MTINO = _MTIModel.MaterialIssueNo, MTI_Date = _MTIModel.MaterialIssueDate, MTI_Type = _MTIModel.hdtrf_type, ListFilterData = _MTIModel.ListFilterData1 });
                            else
                                _MTIModelAddNew.Command = "Refresh";
                                _MTIModelAddNew.TransType = "Refresh";
                                _MTIModelAddNew.BtnName = "Refresh";
                                _MTIModelAddNew.DocumentStatus = null;
                                TempData["ModelData"] = _MTIModelAddNew;
                                return RedirectToAction("MaterialTransferIssueDetail");
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("MaterialTransferIssueDetail", URLModel);

                    case "Edit":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditMTI", new { MTINO = _MTIModel.MaterialIssueNo, MTI_Date = _MTIModel.MaterialIssueDate, MTI_Type = _MTIModel.hdtrf_type, ListFilterData = _MTIModel.ListFilterData1 });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string issueDt = _MTIModel.MaterialIssueDate.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, issueDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditMTI", new { MTINO = _MTIModel.MaterialIssueNo, MTI_Date = _MTIModel.MaterialIssueDate, MTI_Type = _MTIModel.hdtrf_type, ListFilterData = _MTIModel.ListFilterData1 });
                        }
                        /*End to chk Financial year exist or not*/
                        string checkstatus = CheckTransferRecieve(_MTIModel.MaterialIssueNo, _MTIModel.MaterialIssueDate.ToString("yyyy-MM-dd"));
                        if (checkstatus == "Used")
                        {
                            _MTIModel.TransType = "Update";
                            _MTIModel.Command = "Add";
                            _MTIModel.BtnName = "BtnToDetailPage";
                            _MTIModel.Message = checkstatus;
                            _MTIModel.MTI_Number = _MTIModel.MaterialIssueNo;
                            _MTIModel.MTI_Date = _MTIModel.MaterialIssueDate.ToString("yyyy-MM-dd");
                            TempData["ModelData"] = _MTIModel;
                            return RedirectToAction("MaterialTransferIssueDetail");
                        }
                        else
                        {
                            _MTIModel.TransType = "Update";
                            _MTIModel.Command = command;
                            _MTIModel.BtnName = "BtnEdit";
                            _MTIModel.MTI_Number = _MTIModel.MaterialIssueNo;
                            _MTIModel.MTI_Date = _MTIModel.MaterialIssueDate.ToString("yyyy-MM-dd");
                            TempData["ModelData"] = _MTIModel;
                            TempData["ListFilterData"] = _MTIModel.ListFilterData1;

                            URLDetailModel URLModelEdit = new URLDetailModel();
                            URLModelEdit.TransType = "Update";
                            URLModelEdit.Command = command;
                            URLModelEdit.BtnName = "BtnEdit";
                            URLModelEdit.MTI_Number = _MTIModel.MaterialIssueNo;
                            URLModelEdit.MTI_Date = _MTIModel.MaterialIssueDate.ToString("yyyy-MM-dd");
                            return RedirectToAction("MaterialTransferIssueDetail", URLModelEdit);
                        }
                       //CheckTransferRecieve(_MTIModel.MaterialIssueNo, _MTIModel.MaterialIssueDate.ToString("yyyy-MM-dd"));
                                                         //Session["MTI_Type"] = _MTIModel.hdtrf_type;                      
                        

                    case "Save":
                        //Session["Command"] = command;
                        _MTIModel.Command = command;
                        SaveMaterialTransferIssue(_MTIModel);
                        if (_MTIModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_MTIModel.Message == "DocModify"|| _MTIModel.Message == "StockNotAvail")
                        {
                            CommonPageDetails();
                            _MTIModel.MaterialIssueDate = DateTime.Now;
                            _MTIModel.from_br = BranchName;
                            /*commented by Hina on 22-03-2024 to combine all list Procedure  in single Procedure*/
                            //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                            //dt = GetFromWHList();
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    FromWharehouse _warehouse = new FromWharehouse();
                            //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                            //    _warehouse.wh_val = dr["wh_name"].ToString();
                            //    _warehouseList.Add(_warehouse);
                            //}
                            //_MTIModel.FromWharehouseList = _warehouseList;
                            //var forBranch = _MTIModel.hdfrom_whName;
                            //List<ToBranch> _ToBranchList = new List<ToBranch>();
                            //dt = GetToBranchList();
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    ToBranch _ToBranch = new ToBranch();
                            //    _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                            //    _ToBranch.br_val = dr["comp_nm"].ToString();
                            //    _ToBranchList.Add(_ToBranch);
                            //}
                            //ToBranch _oWarehouse = new ToBranch();
                            //_oWarehouse.br_id = 0;
                            //_oWarehouse.br_val = forBranch;
                            //_ToBranchList.Insert(0, _oWarehouse);
                            //_MTIModel.ToBranchList = _ToBranchList;

                            //_MTIModel.from_br = forBranch;

                            dtSet = MTI_GetAllDDLListAndListPageData("","","");
                            List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                            foreach (DataRow dr in dtSet.Tables[0].Rows)
                            {
                                FromWharehouse _warehouse = new FromWharehouse();
                                _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                _warehouse.wh_val = dr["wh_name"].ToString();
                                _warehouseList.Add(_warehouse);
                            }
                            _warehouseList.Insert(0, new FromWharehouse() { wh_id = 0, wh_val = "---Select---" });
                            _MTIModel.FromWharehouseList = _warehouseList;

                            var forBranch = _MTIModel.hdfrom_whName;
                            List<ToBranch> _ToBranchList = new List<ToBranch>();
                            foreach (DataRow dr in dtSet.Tables[1].Rows)
                            {
                                ToBranch _ToBranch = new ToBranch();
                                _ToBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                                _ToBranch.br_val = dr["comp_nm"].ToString();
                                _ToBranchList.Add(_ToBranch);
                            }
                            ToBranch _oWarehouse = new ToBranch();
                            _oWarehouse.br_id = 0;
                            _oWarehouse.br_val = forBranch;
                            _ToBranchList.Insert(0, _oWarehouse);
                            _MTIModel.ToBranchList = _ToBranchList;

                            _MTIModel.from_br = forBranch;

                            List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                            dt = GetToWHList(BrchID);
                            foreach (DataRow dr in dt.Rows)
                            {
                                ToWharehouse _ToWharehouse = new ToWharehouse();
                                _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                _ToWharehouse.wh_val = dr["wh_name"].ToString();
                                _ToWharehouseList.Add(_ToWharehouse);
                            }
                            _MTIModel.ToWharehouseList = _ToWharehouseList;
                            _MTIModel.from_brid = BrchID;

                            var prNumber = _MTIModel.hdReq_Number;

                            _MTIModel.TransList = GetTransporterList();

                            List<Req_NO> _Req_NOList = new List<Req_NO>();
                            Req_NO _Req_NO = new Req_NO();
                            _Req_NO.RequirementDate = "0";
                            _Req_NO.RequirementNo = prNumber;
                            _Req_NOList.Add(_Req_NO);
                            _MTIModel.Req_NO_List = _Req_NOList;

                            _MTIModel.trf_type = _MTIModel.hdtrf_type;
                            //_MTIModel.from_br = Session["BranchId"].ToString();
                            _MTIModel.from_wh = _MTIModel.hdfrom_whid;
                            _MTIModel.to_br = _MTIModel.hdto_brid;
                            _MTIModel.to_wh = _MTIModel.hdto_whid;

                            ViewBag.ItemDetails = ViewData["ItemMTIDetails"];
                            ViewBag.ItemStockBatchWise = ViewData["BatchDetails"];
                            ViewBag.ItemStockSerialWise = ViewData["SerialDetail"];
                            ViewBag.SubItemDetails = ViewData["SubItem"];

                            _MTIModel.Title = title;
                            _MTIModel.Req_Date = null;
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/MaterialTransferIssue/MaterialTransferIssueDetail.cshtml", _MTIModel);
                        }
                        //if (Session["MTI_Number"] != null && Session["MTI_Date"] != null)
                        if (_MTIModel.MTI_Number != null && _MTIModel.MTI_Date != null)
                        {
                            //Session["MTI_Number"] = Session["MTI_Number"].ToString();
                            //Session["MTI_Date"] = Session["MTI_Date"].ToString();
                            TempData["ModelData"] = _MTIModel;
                            URLDetailModel URLModelSave = new URLDetailModel();
                            URLModelSave.MTI_Type = _MTIModel.MTI_Type;
                            URLModelSave.MTI_Number = _MTIModel.MTI_Number;
                            URLModelSave.MTI_Date = _MTIModel.MTI_Date;                      
                            URLModelSave.Command = _MTIModel.Command;
                            URLModelSave.TransType = _MTIModel.TransType ;
                            URLModelSave.BtnName = _MTIModel.BtnName;
                            TempData["ListFilterData"] = _MTIModel.ListFilterData1;
                            return RedirectToAction("MaterialTransferIssueDetail", URLModelSave);
                        }
                        //else if (Session["MTI_Number"] == null && Session["MTI_Date"] != null)
                        else if (_MTIModel.MTI_Number == null && _MTIModel.MTI_Date != null)
                        {

                            TempData["ListFilterData"] = _MTIModel.ListFilterData1;
                            return RedirectToAction("MaterialTransferIssueDetail");
                        }
                        else
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                    case "Refresh":
                        MaterialTransferIssueModel _MTIModelRefesh = new MaterialTransferIssueModel();
                        _MTIModelRefesh.BtnName = "BtnRefresh";
                        _MTIModelRefesh.Command = command;
                        _MTIModelRefesh.TransType = "Save";
                        _MTIModelRefesh.Message = null;
                        _MTIModelRefesh.DocumentStatus = null;
                        TempData["ModelData"] = _MTIModelRefesh;
                        TempData["ListFilterData"] = _MTIModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferIssueDetail");

                    case "Print":
                        //return new EmptyResult();
                        return GenratePdfFile(_MTIModel);
                    case "BacktoList":
                        TempData["ListFilterData"] = _MTIModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferIssue", "MaterialTransferIssue");

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
        public FileResult GenratePdfFile(MaterialTransferIssueModel _MTIModel)
        {
            if (_MTIModel.hdtrf_type == "B")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PrintFormat", typeof(string));
                dt.Columns.Add("ShowProdDesc", typeof(string));
                dt.Columns.Add("ShowCustSpecProdDesc", typeof(string));
                dt.Columns.Add("ShowProdTechDesc", typeof(string));
                dt.Columns.Add("ShowSubItem", typeof(string));
                dt.Columns.Add("CustAliasName", typeof(string));
                DataRow dtr = dt.NewRow();
                //dtr["PrintFormat"] = _model.PrintFormat;
                dtr["ShowProdDesc"] = _MTIModel.ShowProdDesc;
                dtr["ShowCustSpecProdDesc"] = _MTIModel.ShowCustSpecProdDesc;
                dtr["ShowProdTechDesc"] = _MTIModel.ShowProdTechDesc;
                dtr["ShowSubItem"] = _MTIModel.ShowSubItem;
                dtr["CustAliasName"] = _MTIModel.CustomerAliasName;
                dt.Rows.Add(dtr);
                ViewBag.PrintOption = dt;
          }
            ViewBag.DocumentMenuId = _MTIModel.DocumentMenuId;
            return File(GetPdfData(_MTIModel.MaterialIssueNo, _MTIModel.MaterialIssueDate), "application/pdf", "MaterialTransferIssue.pdf");
        }
        public byte[] GetPdfData(string Doc_No, DateTime Doc_dt)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter writer = null;
            string CompID = string.Empty;
            string BrchID = string.Empty;

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
                DataSet Details = _MTI_ISERVICES.GetMaterialTransferIssuePrint(CompID, BrchID, Doc_No, Convert.ToDateTime(Doc_dt).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                //string path1 = Server.MapPath("~") + "..\\Attachment\\";
                //string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                //ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");
                string serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                string localIp = ConfigurationManager.AppSettings["LocalServerip"].ToString();
                if (Request.Url.Host == localIp)
                    serverUrl = ConfigurationManager.AppSettings["LocalServerURL"].ToString();
                else
                    serverUrl = ConfigurationManager.AppSettings["LiveServerURL"].ToString();
                ViewBag.FLogoPath = serverUrl + Details.Tables[0].Rows[0]["logo"].ToString();
                ViewBag.DigiSign = serverUrl + Details.Tables[1].Rows[0]["digi_sign"].ToString();
                string trnsfer_type = Details.Tables[0].Rows[0]["src_type"].ToString();
                ViewBag.Title = "Material Transfer Issue";
                ViewBag.trf_type = trnsfer_type;
                ViewBag.DocumentMenuId = DocumentMenuId;
                string htmlcontent = string.Empty;
                //if (trnsfer_type =="B")
                //{
                //    htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/MaterialTransferIssue/MaterialTransferIssuePrint1.cshtml"));
                //}
                //else
                //{
                     htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialIssue/MaterialTransferIssue/MaterialTransferIssuePrint.cshtml"));
                //}
                
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
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
            return null;
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
        private void CommonPageDetails()
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
            //ViewBag.PackSerialization = ds.Tables[6].Rows.Count > 0 ? ds.Tables[6].Rows[0]["param_stat"].ToString() : "";
            string[] Docpart = DocumentName.Split('>');
            int len = Docpart.Length;
            if (len > 1)
            {
                title = Docpart[len - 1].Trim();
            }
            ViewBag.MenuPageName = DocumentName;
        }
        public ActionResult SaveMaterialTransferIssue(MaterialTransferIssueModel _MTIModel)
        {
            try
            {
                if (_MTIModel.CancelFlag == false)
                {
                    var commonContr = new CommonController();
                    if (Session["compid"] != null)
                    {
                        CompID = Session["compid"].ToString();
                    }
                    if (Session["userid"] != null)
                    {
                        userid = Session["userid"].ToString();
                    }
                    DataTable MaterialTransferIssuetHeader = new DataTable();
                    DataTable MaterialTransferIssueItemDetails = new DataTable();
                    //DataTable MaterialIssueAttachments = new DataTable();
                    DataTable ItemBatchDetails = new DataTable();
                    DataTable ItemSerialDetails = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("trf_type", typeof(string));
                    dtheader.Columns.Add("issue_no", typeof(string));
                    dtheader.Columns.Add("issue_dt", typeof(DateTime));
                    dtheader.Columns.Add("src_br", typeof(int));
                    dtheader.Columns.Add("src_wh", typeof(int));
                    dtheader.Columns.Add("dstn_br", typeof(int));
                    dtheader.Columns.Add("dstn_wh", typeof(int));
                    dtheader.Columns.Add("req_no", typeof(string));
                    dtheader.Columns.Add("req_dt", typeof(DateTime));
                    dtheader.Columns.Add("issue_rem", typeof(string));
                    dtheader.Columns.Add("create_id", typeof(string));
                    dtheader.Columns.Add("issue_status", typeof(string));
                    dtheader.Columns.Add("UserMacaddress", typeof(string));
                    dtheader.Columns.Add("UserSystemName", typeof(string));
                    dtheader.Columns.Add("UserIP", typeof(string));

                    dtheader.Columns.Add("GR_Number", typeof(string));
                    dtheader.Columns.Add("GR_Dt", typeof(string));                   
                    dtheader.Columns.Add("Trans_Name", typeof(string));
                    dtheader.Columns.Add("Veh_Number", typeof(string));
                    dtheader.Columns.Add("Driver_Name", typeof(string));
                    dtheader.Columns.Add("Mob_No", typeof(string));
                    dtheader.Columns.Add("Tot_Tonnage", typeof(string));
                    dtheader.Columns.Add("No_Of_Pkgs", typeof(string));
                 



                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    //dtrowHeader["TransType"] = Session["TransType"].ToString();
                    if (_MTIModel.MaterialIssueNo != null)
                    {
                        dtrowHeader["TransType"] = "Update";
                    }
                    else
                    {
                        dtrowHeader["TransType"] = "Save";
                    }
                    dtrowHeader["comp_id"] = Session["CompId"].ToString();
                    dtrowHeader["br_id"] = Session["BranchId"].ToString();
                    dtrowHeader["trf_type"] = _MTIModel.hdtrf_type;
                    dtrowHeader["issue_no"] = _MTIModel.MaterialIssueNo;
                    dtrowHeader["issue_dt"] = DateTime.Now;
                    dtrowHeader["src_br"] = Session["BranchId"].ToString();
                    dtrowHeader["src_wh"] = _MTIModel.hdfrom_whid;
                    dtrowHeader["dstn_wh"] = _MTIModel.hdto_whid;
                    if (_MTIModel.hdtrf_type == "W")
                    {
                        dtrowHeader["dstn_br"] = Session["BranchId"].ToString();
                    }
                    else
                    {
                        dtrowHeader["dstn_br"] = _MTIModel.hdto_brid;
                    }
                    
                    dtrowHeader["req_no"] = _MTIModel.Req_No;
                    dtrowHeader["req_dt"] = Convert.ToDateTime(_MTIModel.Req_Date);                   
                    dtrowHeader["issue_rem"] = _MTIModel.issue_rem;
                    dtrowHeader["create_id"] = Session["UserId"].ToString();
                    //dtrowHeader["issue_status"] = Session["AppStatus"].ToString();
                    dtrowHeader["issue_status"] = "D";
                    dtrowHeader["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrowHeader["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrowHeader["UserIP"] = Session["UserIP"].ToString();

                    dtrowHeader["GR_Number"] = _MTIModel.GR_No;
                    dtrowHeader["GR_Dt"] = _MTIModel.HdnGRDate;                   
                    dtrowHeader["Trans_Name"] = _MTIModel.Transpt_NameID;                
                    dtrowHeader["Veh_Number"] = _MTIModel.Veh_Number;
                    dtrowHeader["Driver_Name"] = _MTIModel.Driver_Name;
                    dtrowHeader["Mob_No"] = _MTIModel.Mob_No;
                    dtrowHeader["Tot_Tonnage"] = _MTIModel.Tot_Tonnage;
                    dtrowHeader["No_Of_Pkgs"] = _MTIModel.No_Of_Packages;

                    dtheader.Rows.Add(dtrowHeader);
                    MaterialTransferIssuetHeader = dtheader;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(int));
                    dtItem.Columns.Add("br_id", typeof(int));
                    dtItem.Columns.Add("trf_type", typeof(string));
                    dtItem.Columns.Add("issue_no", typeof(string));
                    dtItem.Columns.Add("issue_dt", typeof(DateTime));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("req_qty", typeof(string));
                    dtItem.Columns.Add("pend_qty", typeof(string));
                    dtItem.Columns.Add("wh_id", typeof(string));
                    dtItem.Columns.Add("avl_stock", typeof(string));
                    dtItem.Columns.Add("issue_qty", typeof(string));                  
                    dtItem.Columns.Add("it_remarks", typeof(string));
                    dtItem.Columns.Add("price", typeof(string));
                    dtItem.Columns.Add("value", typeof(string));

                    JArray jObject = JArray.Parse(_MTIModel.MaterialIssueItemDetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["comp_id"] = Session["CompId"].ToString();
                        dtrowLines["br_id"] = Session["BranchId"].ToString();
                        dtrowLines["trf_type"] = _MTIModel.hdtrf_type;
                        dtrowLines["issue_no"] = _MTIModel.MaterialIssueNo;
                        //dtrowLines["issue_dt"] = DateTime.Now;
                        dtrowLines["issue_dt"] = _MTIModel.MaterialIssueDate;
                        dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                        dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                        dtrowLines["req_qty"] = jObject[i]["mrs_qty"].ToString();
                        dtrowLines["pend_qty"] = jObject[i]["pend_qty"].ToString();
                        dtrowLines["wh_id"] = jObject[i]["WareHouseId"].ToString();
                        dtrowLines["avl_stock"] = jObject[i]["avl_stock"].ToString();
                        dtrowLines["issue_qty"] = jObject[i]["issue_qty"].ToString();                       
                        dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                        dtrowLines["price"] = jObject[i]["itemprice"].ToString();
                        dtrowLines["value"] = jObject[i]["value"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["ItemMTIDetails"] = dtitemdetail(jObject, _MTIModel);
                    MaterialTransferIssueItemDetails = dtItem;

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("comp_id", typeof(int));
                    Batch_detail.Columns.Add("br_id", typeof(int));
                    Batch_detail.Columns.Add("trf_type", typeof(string));
                    Batch_detail.Columns.Add("issue_no", typeof(string));
                    Batch_detail.Columns.Add("issue_dt", typeof(DateTime));
                    Batch_detail.Columns.Add("item_id", typeof(string));
                    Batch_detail.Columns.Add("uom_id", typeof(string));
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
                    Batch_detail.Columns.Add("expiry_date", typeof(DateTime));
                    Batch_detail.Columns.Add("issue_qty", typeof(string));
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    Batch_detail.Columns.Add("mfg_name", typeof(string));
                    Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                    Batch_detail.Columns.Add("mfg_date", typeof(string));
                    if (_MTIModel.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_MTIModel.ItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                            dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                            dtrowBatchDetailsLines["trf_type"] = _MTIModel.hdtrf_type;
                            dtrowBatchDetailsLines["issue_no"] = _MTIModel.MaterialIssueNo;
                            dtrowBatchDetailsLines["issue_dt"] = DateTime.Now;
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
                            dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();


                            dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectBatch[i]["mfg_name"].ToString(), null);
                            dtrowBatchDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectBatch[i]["mfg_mrp"].ToString(), null);
                            dtrowBatchDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectBatch[i]["mfg_date"].ToString(), null);
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                        ViewData["BatchDetails"] = dtBatchDetails(jObjectBatch, _MTIModel);
                    }
                    ItemBatchDetails = Batch_detail;

                    DataTable Serial_detail = new DataTable();
                    Serial_detail.Columns.Add("comp_id", typeof(int));
                    Serial_detail.Columns.Add("br_id", typeof(int));
                    Serial_detail.Columns.Add("trf_type", typeof(string));
                    Serial_detail.Columns.Add("issue_no", typeof(string));
                    Serial_detail.Columns.Add("issue_dt", typeof(DateTime));
                    Serial_detail.Columns.Add("item_id", typeof(string));
                    Serial_detail.Columns.Add("uom_id", typeof(int));
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("serial_qty", typeof(string));
                    Serial_detail.Columns.Add("issue_qty", typeof(string));
                    Serial_detail.Columns.Add("lot_no", typeof(string));
                    Serial_detail.Columns.Add("mfg_name", typeof(string));
                    Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                    Serial_detail.Columns.Add("mfg_date", typeof(string));

                    if (_MTIModel.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_MTIModel.ItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                            dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();

                            dtrowSerialDetailsLines["trf_type"] = _MTIModel.hdtrf_type;
                            dtrowSerialDetailsLines["issue_no"] = _MTIModel.MaterialIssueNo;
                            dtrowSerialDetailsLines["issue_dt"] = DateTime.Now;
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                            dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                            dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                            dtrowSerialDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectSerial[i]["mfg_name"].ToString(), null);
                            dtrowSerialDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectSerial[i]["mfg_mrp"].ToString(), null);
                            dtrowSerialDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectSerial[i]["mfg_date"].ToString(), null);
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                        ViewData["SerialDetail"] = dtSerialDetails(jObjectSerial, _MTIModel);
                    }
                    ItemSerialDetails = Serial_detail;

                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    dtSubItem.Columns.Add("req_qty", typeof(string));
                    dtSubItem.Columns.Add("pend_qty", typeof(string));
                    dtSubItem.Columns.Add("avl_stock", typeof(string));
                    if (_MTIModel.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(_MTIModel.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtrowItemdetails["req_qty"] = jObject2[i]["req_qty"].ToString();
                            dtrowItemdetails["pend_qty"] = jObject2[i]["pend_qty"].ToString();
                            dtrowItemdetails["avl_stock"] = jObject2[i]["avl_qty"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubItem"] = dtSubItemDetails(jObject2);
                    }

                    /*------------------Sub Item end----------------------*/

                    string SaveMessage = _MTI_ISERVICES.InsertUpdateMaterialTransferIssue(MaterialTransferIssuetHeader
                        , MaterialTransferIssueItemDetails, ItemBatchDetails, ItemSerialDetails, dtSubItem);
                    if (SaveMessage == "DocModify")
                    {
                        _MTIModel.Message = "DocModify";
                        _MTIModel.BtnName = "BtnRefresh";
                        _MTIModel.Command = "Refresh";
                        TempData["ModelData"] = _MTIModel;
                        return RedirectToAction("MaterialTransferIssueDetail");
                    }
                    else
                    {
                        string[] FDate = SaveMessage.Split(',');

                        string Message = FDate[0].ToString();
                        string MTI_Number = FDate[1].ToString();
                        string MTI_Date = FDate[2].ToString();
                        string MTI_Type = FDate[3].ToString();
                        if (Message == "Data_Not_Found")
                        {
                            var msg = Message.Replace("_", " ") + " " + MTI_Number;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _MTIModel.Message = Message.Replace("_", "");
                            return RedirectToAction("MaterialTransferIssueDetail");
                        }
                        if (Message == "StockNotAvail")
                        {
                            _MTIModel.StockItemWiseMsg1 = string.Join(",", FDate.Skip(4));
                            _MTIModel.Message = "StockNotAvail";
                            _MTIModel.BtnName = "Refresh";
                            _MTIModel.Command = "Refresh";
                            _MTIModel.TransType = "Save";
                            _MTIModel.DocumentStatus = null;
                            _MTIModel.MTI_Date = MTI_Date;
                            TempData["ModelData"] = _MTIModel;
                        }

                        if (Message == "Update" || Message == "Save")
                        {
                            _Common_IServices.SendAlertEmail(Session["CompId"].ToString(), Session["BranchId"].ToString(), DocumentMenuId, MTI_Number, "AP", userid, "0");
                            //Session["MTI_Type"] = MTI_Type;
                            //Session["MTI_Number"] = MTI_Number;
                            //Session["MTI_Date"] = MTI_Date;

                            //Session["Message"] = "Save";
                            //Session["Command"] = "Update";
                            //Session["TransType"] = "Update";
                            //Session["AppStatus"] = 'D';
                            //Session["BtnName"] = "BtnEdit";
                            _MTIModel.MTI_Type = MTI_Type;
                            _MTIModel.MTI_Number = MTI_Number;
                            _MTIModel.MTI_Date = MTI_Date;
                            _MTIModel.Message = Message;
                            _MTIModel.Command = "Update";
                            _MTIModel.TransType = "Update";
                            _MTIModel.AppStatus = "D";
                            _MTIModel.BtnName = "BtnEdit";
                            TempData["ModelData"] = _MTIModel;
                        }
                        ViewBag.DocumentMenuId = DocumentMenuId;
                        return RedirectToAction("MaterialTransferIssueDetail");
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
                        userid = Session["userid"].ToString();
                    }

                    string br_id = Session["BranchId"].ToString();
                    _MTIModel.CreatedBy = userid;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    //string MTI_date = Session["MTI_Date"].ToString();
                    string MTI_date = _MTIModel.MaterialIssueDate.ToString();
                    //string MTI_Type =  Session["MTI_Type"].ToString();
                    string MTI_Type = _MTIModel.hdtrf_type;
                    DataSet message = _MTI_ISERVICES.MaterialTransferIssueCancel(_MTIModel, CompID, br_id, mac_id, MTI_date, MTI_Type);

                    //Session["MTI_Number"] = _MTIModel.MaterialIssueNo;
                    //Session["MTI_Date"] = _MTIModel.MaterialIssueDate.ToString("yyyy-MM-dd");
                    //Session["MTI_Type"] = _MTIModel.trf_type;

                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MTIModel.MaterialIssueNo, "C", userid, "0");
                    _MTIModel.MTI_Number = _MTIModel.MaterialIssueNo;
                    _MTIModel.MTI_Date = _MTIModel.MaterialIssueDate.ToString("yyyy-MM-dd");
                    _MTIModel.MTI_Type = MTI_Type;
                    _MTIModel.Message = "Cancelled";
                    _MTIModel.Command = "Update";
                    _MTIModel.TransType = "Update";
                    _MTIModel.AppStatus = "D";
                    _MTIModel.BtnName = "Refresh";
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    TempData["ModelData"] = _MTIModel;
                    return RedirectToAction("MaterialTransferIssueDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public DataTable dtitemdetail(JArray jObject, MaterialTransferIssueModel _MTIModel)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("comp_id", typeof(int));
            dtItem.Columns.Add("br_id", typeof(int));
            dtItem.Columns.Add("trf_type", typeof(string));
            dtItem.Columns.Add("issue_no", typeof(string));
            dtItem.Columns.Add("issue_dt", typeof(DateTime));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_Name", typeof(string));
            dtItem.Columns.Add("base_uom_id", typeof(int));
            dtItem.Columns.Add("uom_Name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("req_qty", typeof(string));
            dtItem.Columns.Add("pend_qty", typeof(string));
            dtItem.Columns.Add("wh_id", typeof(string));
            dtItem.Columns.Add("wh_name", typeof(string));
            dtItem.Columns.Add("avl_stock", typeof(string));
            dtItem.Columns.Add("issue_qty", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));
            dtItem.Columns.Add("price", typeof(string));
            dtItem.Columns.Add("value", typeof(string));
            dtItem.Columns.Add("item_val", typeof(string));

           


            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["comp_id"] = Session["CompId"].ToString();
                dtrowLines["br_id"] = Session["BranchId"].ToString();
                dtrowLines["trf_type"] = _MTIModel.hdtrf_type;
                dtrowLines["issue_no"] = _MTIModel.MaterialIssueNo;
                dtrowLines["issue_dt"] = _MTIModel.MaterialIssueDate;
                dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowLines["item_Name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["base_uom_id"] = jObject[i]["UOMId"].ToString();
                dtrowLines["uom_Name"] = jObject[i]["UOM"].ToString();
                dtrowLines["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowLines["req_qty"] = jObject[i]["mrs_qty"].ToString();
                dtrowLines["pend_qty"] = jObject[i]["pend_qty"].ToString();
                dtrowLines["wh_id"] = jObject[i]["WareHouseId"].ToString();
                dtrowLines["wh_name"] = jObject[i]["wh_name"].ToString();
                dtrowLines["avl_stock"] = jObject[i]["avl_stock"].ToString();
                dtrowLines["issue_qty"] = jObject[i]["issue_qty"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["remarks"].ToString();
                dtrowLines["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowLines["i_serial"] = jObject[i]["i_serial"].ToString();
                dtrowLines["price"] = jObject[i]["itemprice"].ToString();
                dtrowLines["value"] = jObject[i]["value"].ToString();
                dtrowLines["item_val"] = jObject[i]["value"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }
            return dtItem;
        }
        public DataTable dtBatchDetails(JArray jObjectBatch, MaterialTransferIssueModel _MTIModel)
        {
            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("comp_id", typeof(int));
            Batch_detail.Columns.Add("br_id", typeof(int));
            Batch_detail.Columns.Add("trf_type", typeof(string));
            Batch_detail.Columns.Add("issue_no", typeof(string));
            Batch_detail.Columns.Add("issue_dt", typeof(DateTime));
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("uom_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("avl_batch_qty", typeof(string));
            Batch_detail.Columns.Add("expiry_date", typeof(DateTime));
            Batch_detail.Columns.Add("exp_dt", typeof(string));
            Batch_detail.Columns.Add("issue_qty", typeof(string));
            Batch_detail.Columns.Add("lot_id", typeof(string));

            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                dtrowBatchDetailsLines["trf_type"] = _MTIModel.hdtrf_type;
                dtrowBatchDetailsLines["issue_no"] = _MTIModel.MaterialIssueNo;
                dtrowBatchDetailsLines["issue_dt"] = DateTime.Now;
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
                dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["ExpiryDate"].ToString();
                dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                //dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["LotNo"].ToString();
                dtrowBatchDetailsLines["lot_id"] = jObjectBatch[i]["LotNo"].ToString();
                dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                Batch_detail.Rows.Add(dtrowBatchDetailsLines);
            }
            return Batch_detail;
        }
        public DataTable dtSerialDetails(JArray jObjectSerial, MaterialTransferIssueModel _MTIModel)
        {
            DataTable Serial_detail = new DataTable();
            Serial_detail.Columns.Add("comp_id", typeof(int));
            Serial_detail.Columns.Add("br_id", typeof(int));
            Serial_detail.Columns.Add("trf_type", typeof(string));
            Serial_detail.Columns.Add("issue_no", typeof(string));
            Serial_detail.Columns.Add("issue_dt", typeof(DateTime));
            Serial_detail.Columns.Add("item_id", typeof(string));
            Serial_detail.Columns.Add("uom_id", typeof(int));
            Serial_detail.Columns.Add("serial_no", typeof(string));
            Serial_detail.Columns.Add("serial_qty", typeof(string));
            Serial_detail.Columns.Add("issue_qty", typeof(string));
            Serial_detail.Columns.Add("lot_no", typeof(string));

            for (int i = 0; i < jObjectSerial.Count; i++)
            {
                DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();

                dtrowSerialDetailsLines["trf_type"] = _MTIModel.hdtrf_type;
                dtrowSerialDetailsLines["issue_no"] = _MTIModel.MaterialIssueNo;
                dtrowSerialDetailsLines["issue_dt"] = DateTime.Now;
                dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemId"].ToString();
                dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNO"].ToString();
                dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["LOTId"].ToString();
                Serial_detail.Rows.Add(dtrowSerialDetailsLines);
            }
            return Serial_detail;
        }
        public DataTable dtSubItemDetails(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));
            dtSubItem.Columns.Add("req_qty", typeof(string));
            dtSubItem.Columns.Add("pend_qty", typeof(string));
            dtSubItem.Columns.Add("avl_stock", typeof(string));

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtrowItemdetails["req_qty"] = jObject2[i]["req_qty"].ToString();
                dtrowItemdetails["pend_qty"] = jObject2[i]["pend_qty"].ToString();
                dtrowItemdetails["avl_stock"] = jObject2[i]["avl_qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        //[NonAction]
        //private DataTable GetFromWHList()
        //{
        //    string CompID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    string BranchId = Session["BranchId"].ToString();

        //    DataTable dt = _MTI_ISERVICES.GetWhList(CompID, BranchId);
        //    return dt;
        //}
        //[NonAction]
        //private DataTable GetToBranchList()
        //{
        //    string CompID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    DataTable dt = _MTI_ISERVICES.GetToBranchList(CompID);
        //    return dt;
        //}
        [NonAction]
        private DataSet MTI_GetAllDDLListAndListPageData(string flag, string startDate, string CurrentDate)
        {
            string CompID = string.Empty;
            string BranchId = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                 BranchId = Session["BranchId"].ToString();
            }
            DataSet ds = _MTI_ISERVICES.MTI_GetAllDDLListAndListPageData(CompID, BranchId, flag, startDate, CurrentDate);
            return ds;
        }

        [HttpPost]
        private DataTable GetToWHList(string Tobranch)
        {
            List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>(); ;
            _MTIModel = new MaterialTransferIssueModel();
            string CompID = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            //string BranchId = Session["BranchId"].ToString();

            DataTable dt = _MTI_ISERVICES.GetToWhList(CompID, Tobranch);
            foreach (DataRow dr in dt.Rows)
            {
                ToWharehouse _ToWharehouse = new ToWharehouse();
                _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                _ToWharehouse.wh_val = dr["wh_name"].ToString();
                _ToWharehouseList.Add(_ToWharehouse);

            }

            _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
            _MTIModel.ToWharehouseList = _ToWharehouseList;

            return dt;
        }

        [HttpPost]
        public JsonResult GetToWHList1(string Tobranch)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                DataTable dt = _MTI_ISERVICES.GetToWhList(CompID, Tobranch);
                DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);

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
        public ActionResult GetMaterialTranserReqList(MaterialTransferIssueModel _MTIModel)
        {
            try
            {
                string MTRNo, SourceWH, TransferType, ToWH, ToBR, SourceBR = string.Empty;
                DataSet MTRListDs = new DataSet();

                List<Req_NO> _Req_NOList = new List<Req_NO>();

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                MTRNo = _MTIModel.FilterMTRNo;
                SourceWH = _MTIModel.FilterSourceWH;
                SourceBR = Session["BranchId"].ToString(); 
                TransferType = _MTIModel.FilterTransferType;
                ToWH= _MTIModel.FilterToWH;
                if (_MTIModel.FilterTransferType == "W")
                {
                    ToBR= Session["BranchId"].ToString();
                }
                else
                {
                    ToBR = _MTIModel.FilterToBR;
                }                
                string BrchID = Session["BranchId"].ToString();
                MTRListDs = _MTI_ISERVICES.getMTRNOList(CompID, BrchID, MTRNo, SourceBR, SourceWH, TransferType, ToWH, ToBR);
                if (MTRListDs.Tables[0].Rows.Count > 0)
                {
                    DataRow Drow = MTRListDs.Tables[0].NewRow();
                    Drow[0] = "---Select---";
                    Drow[1] = "0";

                    MTRListDs.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in MTRListDs.Tables[0].Rows)
                    {
                        Req_NO _Req_NO = new Req_NO(); ;
                        _Req_NO.RequirementDate = dr["trf_dt"].ToString();
                        _Req_NO.RequirementNo = dr["trf_no"].ToString();
                        _Req_NOList.Add(_Req_NO);
                    }

                }

                _MTIModel.Req_NO_List = _Req_NOList;
                return Json(_Req_NOList.Select(c => new { Name = c.RequirementNo, ID = c.RequirementDate }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetMTRDetailByNumber(string BrchID, string TRFType, string TRFNo, string TRFDate)
        {
            string SessionBrchID = string.Empty;
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    SessionBrchID = Session["BranchId"].ToString();
                }
                DataSet ds = _MTI_ISERVICES.GetMaterialTransferItemDetail(CompID, BrchID, TRFDate, TRFNo, TRFType, SessionBrchID);
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
        //public void GetStatusList(MaterialTransferIssueListModel _MTIListModel)
        //{
        //    try
        //    {
        //        string MenuDocId = string.Empty;
        //        if (Session["MenuDocumentId"] != null)
        //        {
        //            MenuDocId = DocumentMenuId;
        //        }
        //        DataSet result = _MTIList_ISERVICES.GetStatusList(MenuDocId);
        //        DataTable DTdata = new DataTable();
        //        DTdata = result.Tables[0];

        //        List<Status> _Status = new List<Status>();
        //        if (DTdata.Rows.Count > 0)
        //        {
        //            foreach (DataRow data in DTdata.Rows)
        //            {
        //                Status _Statuslist = new Status();
        //                _Statuslist.status_id = data["status_code"].ToString();
        //                _Statuslist.status_name = data["status_name"].ToString();
        //                _Status.Add(_Statuslist);
        //            }
        //        }
        //        _MTIListModel.StatusList = _Status;
        //    }
        //    catch (Exception Ex)
        //    {
        //        string path = Server.MapPath("~");
        //        Errorlog.LogError(path, Ex);
        //    }
        //}
        private List<MTIList> GetMTIDetailList(MaterialTransferIssueListModel _MTIListModel)
        {
            _MTIList = new List<MTIList>();

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            dt = _MTIList_ISERVICES.GetMTIDetailList(CompID, BrchID, _MTIListModel.to_wh, _MTIListModel.to_br, _MTIListModel.MTO_FromDate, _MTIListModel.MTO_ToDate, _MTIListModel.TRF_Type, _MTIListModel.MTIStatus);
            if (dt.Rows.Count > 0)
            {
                //FromDate = dt.Rows[0]["finstrdate"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    MTIList _TempMTIList = new MTIList();
                    _TempMTIList.MTINo = dr["issue_no"].ToString();
                    _TempMTIList.MTIDate = dr["issue_dt"].ToString();
                    _TempMTIList.issue_date = dr["issue_date"].ToString().Trim();
                    _TempMTIList.TRFType = dr["TRFType"].ToString();
                    _TempMTIList.trfType = dr["trf_type"].ToString();
                    _TempMTIList.ReqNo = dr["req_no"].ToString();
                    _TempMTIList.ReqDate = dr["req_dt"].ToString();
                    _TempMTIList.FromWH = dr["fromwh"].ToString();                   
                    _TempMTIList.ToBranch = dr["tobr"].ToString();
                    _TempMTIList.ToWH = dr["towh"].ToString();
                    _TempMTIList.MTIList_Stauts = dr["Status"].ToString();
                    _TempMTIList.CreateDate = dr["CreateDate"].ToString();
                    _TempMTIList.create_by = dr["create_by"].ToString();
                    _TempMTIList.mod_by = dr["mod_by"].ToString();
                    _MTIList.Add(_TempMTIList);
                }
            }
            return _MTIList;
        }
        [HttpPost]
        public ActionResult SearchMTIDetail(int toWh, int tobranch, string Fromdate, string Todate, string TransferType, string Status)
        {
            _MTIList = new List<MTIList>();
            MaterialTransferIssueListModel _MTIListModel = new MaterialTransferIssueListModel();
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }

            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            dt = _MTIList_ISERVICES.GetMTIDetailList(CompID, BrchID, toWh, tobranch, Fromdate, Todate, TransferType, Status);
            if (dt.Rows.Count > 0)
            {
                //Session["MTISearch"] = "MTI_Search";
                _MTIListModel.MTISearch = "MTI_Search";
                //FromDate = dt.Rows[0]["finstrdate"].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    MTIList _TempMTIList = new MTIList();
                    _TempMTIList.MTINo = dr["issue_no"].ToString();
                    _TempMTIList.MTIDate = dr["issue_dt"].ToString();
                    _TempMTIList.issue_date = dr["issue_date"].ToString().Trim();
                    _TempMTIList.TRFType = dr["TRFType"].ToString();
                    _TempMTIList.trfType = dr["trf_type"].ToString();
                    _TempMTIList.ReqNo = dr["req_no"].ToString();
                    _TempMTIList.ReqDate = dr["req_dt"].ToString();
                    _TempMTIList.FromWH = dr["fromwh"].ToString();
                    _TempMTIList.ToBranch = dr["tobr"].ToString();
                    _TempMTIList.ToWH = dr["towh"].ToString();
                    _TempMTIList.MTIList_Stauts = dr["Status"].ToString();
                    _TempMTIList.CreateDate = dr["CreateDate"].ToString();
                    _TempMTIList.create_by = dr["create_by"].ToString();
                    _TempMTIList.mod_by = dr["mod_by"].ToString();
                    _MTIList.Add(_TempMTIList);
                }
            }
            _MTIListModel.BindMTIList = _MTIList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMTIList.cshtml", _MTIListModel);
        }
        public ActionResult getItemStockBatchWise(string ItemId, string WarehouseId, string CompId, string BranchId, string SelectedItemdetail,string CMD,string typ)
        {
            try   
            {
                DataSet ds = new DataSet();
                if (ItemId != "")
                    ds = _MaterialIssue_IServices.getItemStockBatchWise(ItemId,null/*UomId*/, WarehouseId, CompId, BranchId);
                if (SelectedItemdetail != null && SelectedItemdetail != "")
                {
                    JArray jObjectBatch = JArray.Parse(SelectedItemdetail);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        foreach (JObject item in jObjectBatch.Children())
                        {
                            if (item.GetValue("LotNo").ToString() == ds.Tables[0].Rows[i]["lot_id"].ToString() && item.GetValue("BatchNo").ToString() == ds.Tables[0].Rows[i]["batch_no"].ToString())
                            {
                                ds.Tables[0].Rows[i]["issue_qty"] = item.GetValue("IssueQty");
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.ItemStockBatchWise = ds.Tables[0];

                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.Command = CMD;
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
        public ActionResult getItemstockSerialWise(string ItemId, string WarehouseId, string CompId, string BranchId, string SelectedItemSerial, string CMD, string typ)
        {
            try
            {
                DataSet ds = new DataSet();
                if (ItemId != "")
                    ds = _MaterialIssue_IServices.getItemstockSerialWise(ItemId, WarehouseId, CompId, BranchId);

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
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.Command = CMD;
                ViewBag.TransType = typ;
                //return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialMaterialIssueItemStockSerialWise.cshtml");
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockSerialWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult getItemstockSerialWiseAfterStockUpadte(string IssueType, string IssueNo, string IssueDate, string ItemID, string CMD, string typ)
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
                ds = _MTI_ISERVICES.getItemstockSerialWiseAfterStockUpdate(CompID, br_id, IssueType, IssueNo, IssueDate, ItemID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ItemStockSerialWise = ds.Tables[0];
                }
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.Command = CMD;
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
  
        public ActionResult getItemStockBatchWiseAfterStockUpadte(string IssueType, string IssueNo, string IssueDate, string ItemID, string CMD, string typ)
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
                ds = _MTI_ISERVICES.getItemStockBatchWiseAfterStockUpdate(CompID, br_id, IssueType, IssueNo, IssueDate, ItemID);
                ViewBag.DocID = DocumentMenuId;
                ViewBag.DocumentMenuId = DocumentMenuId;
                ViewBag.Command = CMD;
                ViewBag.TransType = typ;
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                return PartialView("~/Areas/Common/Views/Comn_PartialItemStockBatchWise_New.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }

        private string CheckTransferRecieve(string txtMaterialIssueNo,string txtMaterialIssueDate)
        {
            try
            {
                //JsonResult DataRows = null;
               // DataTable ds = new DataTable();
                string CompID = string.Empty;
                string br_id = string.Empty;
                string result = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    br_id = Session["BranchId"].ToString();
                }
                dt = _MTI_ISERVICES.CheckTransferRecieve(CompID, br_id, txtMaterialIssueNo, txtMaterialIssueDate).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    result = "Used";
                }
                else
                {
                    result = null;
                }
               // DataRows = Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                throw Ex;
            }
        }

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
   , string Flag, string Status, string Doc_no, string Doc_dt,string whId)
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
                //if (Flag == "IssueQty")
                //{
                    if (Status == "D" || Status == "F" || Status == "")
                    {
                        //dt = _Common_IServices.GetubItemDetails(CompID, Item_id).Tables[0];
                        dt = _Common_IServices.GetSubItemWhAvlstockDetails(CompID,BrchID,whId, Item_id, null/*UomId*/, "wh").Tables[0];
                        dt.Columns.Add("Qty", typeof(string));
                        dt.Columns.Add("req_qty",typeof(string));
                        dt.Columns.Add("pend_qty",typeof(string));
                        //dt.Columns.Add("avl_stock", typeof(string));
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        DataTable NewDt = new DataTable();
                    int DecDgt = Convert.ToInt32(Session["QtyDigit"] != null ? Session["QtyDigit"] : "0");
                    string flag = "N";
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                        flag = "N";
                        foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = cmn.ConvertToDecimal(item.GetValue("qty").ToString(), DecDgt);
                                    dt.Rows[i]["req_qty"] = cmn.ConvertToDecimal(item.GetValue("req_qty").ToString(), DecDgt);
                                    dt.Rows[i]["pend_qty"] = cmn.ConvertToDecimal(item.GetValue("pend_qty").ToString(), DecDgt);
                                    //dt.Rows[i]["avl_stock"] = item.GetValue("avl_qty");
                                flag = "Y";
                            }
                            }
                        if(flag == "N")
                        {
                            dt.Rows[i].Delete();
                            //dt.Rows[i].AcceptChanges();
                        }
                        
                        }
                    for (var i = dt.Rows.Count-1; i >= 0; i--)
                    {
                        dt.Rows[i].AcceptChanges();
                    }
                    }
                    else
                    {
                        dt = _MTI_ISERVICES.MTI_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    }
                //}
                //else if (Flag == "Issued")
                //{
                //    dt = _MTI_ISERVICES.MTI_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                //}
                //else if (Flag == "Received")
                //{
                //    dt = _MTI_ISERVICES.MTI_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                //}

                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag == "IssueQty" ? Flag : "MTI",
                    dt_SubItemDetails = dt,
                    _subitemPageName="MTI",
                    IsDisabled = IsDisabled,
                    decimalAllowed="Y"

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

        public ActionResult MTI_getItemstockWareHouselWise(string ItemId, string UomId = null)
        {
            try
            {
                string CompID = "", BrchID = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet ds;
                ds = _MTI_ISERVICES.MTI_getItemstockWarehouseWise(ItemId, UomId, CompID, BrchID);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockWareHouselWise = ds.Tables[0];
                return PartialView("~/Areas/Common/Views/PartialItemStockWareHouseWise.cshtml");
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