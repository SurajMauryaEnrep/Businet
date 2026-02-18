using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EnRepMobileWeb.SERVICES.SERVICES;
using EnRepMobileWeb.MODELS.DASHBOARD;
using Newtonsoft.Json;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.MaterialTransferReceipt;
using Newtonsoft.Json.Linq;
using System.IO;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.MaterialTransferReceipt;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using System.Text;
using EnRepMobileWeb.MODELS.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialReceipt.TransferMaterialReceipt
{
    public class MaterialTransferReceiptController : Controller
    {
        
        string DocumentMenuId = "105102115110", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        string CompID, BrchID, UserID, BranchName, userid, language = String.Empty,mr_no;
        string FromDate;
        List<TMRList> _TMRList;
        Common_IServices _Common_IServices;
        MaterialTransferReceiptModel _MTRModel;
        DataTable dt;
        DataSet dtSet;
        MaterialTransferReceipt_ISERVICES _MTR_ISERVICES;
        TMRList_ISERVICES _TMRList_ISERVICES;
        public MaterialTransferReceiptController(Common_IServices _Common_IServices, MaterialTransferReceipt_ISERVICES _MTR_ISERVICES, TMRList_ISERVICES _TMRList_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this._MTR_ISERVICES = _MTR_ISERVICES;
            this._TMRList_ISERVICES = _TMRList_ISERVICES;
        }
        // GET: ApplicationLayer/MaterialTransferReceipt
        public ActionResult MaterialTransferReceipt(TMR_ListModel TMRDashBord)
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
                BrchID = Session["BranchId"].ToString();
            }
            CommonPageDetails();
            TMR_ListModel _TMRListModel = new TMR_ListModel();
            if (TMRDashBord.WF_status != null)
            {
                _TMRListModel.WF_status = TMRDashBord.WF_status;
            }
            else
            {
                
            }
            ViewBag.DocumentMenuId = DocumentMenuId;
            List<Status> statusLists = new List<Status>();
            foreach (DataRow dr in ViewBag.StatusList.Rows)
            {
                Status list = new Status();
                list.status_id = dr["status_code"].ToString();
                list.status_name = dr["status_name"].ToString();
                statusLists.Add(list);
            }
            _TMRListModel.StatusList = statusLists;

            /*commented by Hina on 18-03-2024 to combine all list Procedure  in single Procedure*/
            // List<FromWharehouseList> _warehouseListpage = new List<FromWharehouseList>();
            //DataTable  dt2 = GetFromWHList();
            // foreach (DataRow dr in dt2.Rows)
            // {
            //     FromWharehouseList _warehouselist = new FromWharehouseList();
            //     _warehouselist.wh_id = Convert.ToInt32(dr["wh_id"]);
            //     _warehouselist.wh_val = dr["wh_name"].ToString();
            //     _warehouseListpage.Add(_warehouselist);
            // }
            // _TMRListModel.FromWharehouseListPage = _warehouseListpage;
            // DateTime dtnow = DateTime.Now;
            // string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
            // List<FromBranchList> _FromBranchList = new List<FromBranchList>();
            //DataTable  dt1 = GetFromBranchList();
            // foreach (DataRow dr in dt1.Rows)
            // {
            //     FromBranchList _FromBranch = new FromBranchList();
            //     _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
            //     _FromBranch.br_val = dr["comp_nm"].ToString();
            //     _FromBranchList.Add(_FromBranch);
            // }
            // _TMRListModel.FromBranchListPage = _FromBranchList;
            var flag = "ListPage";
            var PageName = "MTReceipt";
            dtSet = GetAllDDLListAndListPageData(flag, PageName, UserID,_TMRListModel.WF_status, DocumentMenuId);
            List<FromWharehouseList> _warehouseListpage = new List<FromWharehouseList>();
            foreach (DataRow dr in dtSet.Tables[0].Rows)
            {
                FromWharehouseList _warehouselist = new FromWharehouseList();
                _warehouselist.wh_id = Convert.ToInt32(dr["wh_id"]);
                _warehouselist.wh_val = dr["wh_name"].ToString();
                _warehouseListpage.Add(_warehouselist);
            }
            _warehouseListpage.Insert(0, new FromWharehouseList() { wh_id = 0, wh_val = "---Select---" });/*Add by Hina on 13-09-2024*/
            _TMRListModel.FromWharehouseListPage = _warehouseListpage;
            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;

            List<FromBranchList> _FromBranchList = new List<FromBranchList>();
            foreach (DataRow dr in dtSet.Tables[1].Rows)
            {
                FromBranchList _FromBranch = new FromBranchList();
                _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                _FromBranch.br_val = dr["comp_nm"].ToString();
                _FromBranchList.Add(_FromBranch);
            }
            _FromBranchList.Insert(0, new FromBranchList() { br_id = 0, br_val = "---Select---" });/*Add by Hina on 13-09-2024*/
            _TMRListModel.FromBranchListPage = _FromBranchList;

            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() !="")
            {
                var ListFilterData = TempData["ListFilterData"].ToString();
                var a = ListFilterData.Split(',');
                _TMRListModel.from_br = (a[0].Trim());
                _TMRListModel.from_wh =(a[1].Trim());
                _TMRListModel.TMR_FromDate = a[2].Trim();
                _TMRListModel.TMR_ToDate = a[3].Trim();
                _TMRListModel.Status = a[4].Trim();
                if (_TMRListModel.Status == "0")
                {
                    _TMRListModel.Status = null;
                }

                //_TMRListModel.TMRList = GetTMRDetailList(_TMRListModel);
                _TMRListModel.ListFilterData = TempData["ListFilterData"].ToString();
                
            }
            else
            {
                /*commented by Hina on 18-03-2024 to combine all list Procedure  in single Procedure*/
                //_TMRListModel.TMRList = GetTMRDetailList(_TMRListModel);
                //_TMRList = new List<TMRList>();
                //if (dtSet.Tables[2].Rows.Count > 0)
                //{

                //    foreach (DataRow dr in dtSet.Tables[2].Rows)
                //    {
                //        TMRList _TempTMRList = new TMRList();
                //        _TempTMRList.MRNo = dr["mr_no"].ToString();
                //        _TempTMRList.MRDate = dr["mr_dt"].ToString();
                //        _TempTMRList.MR_Dt = dr["mr_date"].ToString().Trim();
                //        _TempTMRList.TrfType = dr["TRFType"].ToString();
                //        _TempTMRList.Trf_Type = dr["trf_type"].ToString();
                //        _TempTMRList.SourceBranch = dr["frombr"].ToString();
                //        _TempTMRList.SourceWH = dr["fromwh"].ToString();
                //        _TempTMRList.Stauts = dr["Status"].ToString();
                //        _TempTMRList.CreateDate = dr["CreateDate"].ToString();
                //        _TempTMRList.ApproveDate = dr["ApproveDate"].ToString();
                //        _TempTMRList.ModifyDate = dr["ModifyDate"].ToString();
                //        _TempTMRList.create_by = dr["create_by"].ToString();
                //        _TempTMRList.mod_by = dr["mod_by"].ToString();
                //        _TempTMRList.app_by = dr["app_by"].ToString();

                //        _TMRList.Add(_TempTMRList);
                //    }
                //}
                //_TMRListModel.TMRList = _TMRList;
            }
           
            if (_TMRListModel.TMR_FromDate != null)
            {
                _TMRListModel.FromDate = _TMRListModel.TMR_FromDate;
            }
            else
            {
                _TMRListModel.FromDate = startDate;
                _TMRListModel.TMR_FromDate = startDate;
                _TMRListModel.TMR_ToDate = CurrentDate;
            }
            _TMRListModel.TMRList = GetTMRDetailList(_TMRListModel);
            ViewBag.MenuPageName = getDocumentName();
            _TMRListModel.Title = title;
            //Session["MTRSearch"] = "0";
            _TMRListModel.MTRSearch = "0";
            ViewBag.VBRoleList = GetRoleList();
            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/MaterialTransferReceipt/MaterialTransferReceiptList.cshtml", _TMRListModel);
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
                    userid = Session["userid"].ToString();
                }
                DataTable RoleList = _Common_IServices.GetRole_List(CompID, userid, DocumentMenuId);

                return RoleList;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult AddMaterialTransferReceiptDetail()
        {
            MaterialTransferReceiptModel _MTRModel = new MaterialTransferReceiptModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["AppStatus"] = 'D';
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            _MTRModel.Message = "New";
            _MTRModel.Command = "Add";
            _MTRModel.AppStatus = "D";
             _MTRModel.TransType = "Save";
            _MTRModel.BtnName = "BtnAddNew";
            TempData["ModelData"]= _MTRModel;
            ViewBag.MenuPageName = getDocumentName();
            /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("MaterialTransferReceipt");
            }
            /*End to chk Financial year exist or not*/
            return RedirectToAction("MaterialTransferReceiptDetail", "MaterialTransferReceipt");
        }
        public ActionResult MaterialTransferReceiptDetail(URLModelDetails _UrlModel)
        {
            ViewBag.DocumentMenuId = DocumentMenuId;
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
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            /*Add by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, _UrlModel.TMR_Date) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            try
            {
               var  _MTRModelData=  TempData["ModelData"] as MaterialTransferReceiptModel;
                if (_MTRModelData!=null)
                {
                    if (_MTRModelData.WF_status1 != null)
                    {
                        _MTRModelData.WF_status1 = _MTRModelData.WF_status1;
                    }
                    CommonPageDetails();
                    //MaterialTransferReceiptModel _MTRModelData = new MaterialTransferReceiptModel();
                    //var other = new CommonController(_Common_IServices);

                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    _MTRModelData.to_br = BranchName;
                    _MTRModelData.MaterialReceiptDate = DateTime.Now;
                    /*commented by Hina on 18-03-2024 to combine all list Procedure  in single Procedure*/
                    //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    //dt = GetFromWHList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    FromWharehouse _warehouse = new FromWharehouse();
                    //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _warehouse.wh_val = dr["wh_name"].ToString();
                    //    _warehouseList.Add(_warehouse);
                    //}
                    //_MTRModelData.FromWharehouseList = _warehouseList;

                    //List<FromBranch> _FromBranchList = new List<FromBranch>();
                    //dt = GetFromBranchList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    FromBranch _FromBranch = new FromBranch();
                    //    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    //    _FromBranch.br_val = dr["comp_nm"].ToString();
                    //    _FromBranchList.Add(_FromBranch);
                    //}
                    //_MTRModelData.FromBranchList = _FromBranchList;

                    dtSet = GetAllDDLListAndListPageData("","","","","");
                    /******************Bind FromWharehouse Dropdown*************************/
                    //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    //foreach (DataRow dr in dtSet.Tables[0].Rows)
                    //{
                    //    FromWharehouse _warehouse = new FromWharehouse();
                    //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _warehouse.wh_val = dr["wh_name"].ToString();
                    //    _warehouseList.Add(_warehouse);
                    //}
                    //_MTRModelData.FromWharehouseList = _warehouseList;
                    /******************Bind FromBranch Dropdown*************************/
                    //List<FromBranch> _FromBranchList = new List<FromBranch>();
                    //foreach (DataRow dr in dtSet.Tables[1].Rows)
                    //{
                    //    FromBranch _FromBranch = new FromBranch();
                    //    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    //    _FromBranch.br_val = dr["comp_nm"].ToString();
                    //    _FromBranchList.Add(_FromBranch);
                    //}
                    //_MTRModelData.FromBranchList = _FromBranchList;

                    //List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                    ////dt = GetToWHList1(BrchID);
                    //foreach (DataRow dr in dtSet.Tables[0].Rows)
                    //{
                    //    ToWharehouse _ToWharehouse = new ToWharehouse();
                    //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                    //    _ToWharehouseList.Add(_ToWharehouse);
                    //}
                    //_MTRModelData.ToWharehouseList = _ToWharehouseList;

                    List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    _warehouseList.Insert(0, new FromWharehouse() { wh_id = 0, wh_val = "---Select---" });
                    _MTRModelData.FromWharehouseList = _warehouseList;

                    List<FromBranch> _FromBranchList = new List<FromBranch>();
                    _FromBranchList.Insert(0, new FromBranch() { br_id = 0, br_val = "---Select---" });
                    foreach (DataRow dr in dtSet.Tables[1].Rows)
                    {
                        FromBranch _FromBranch = new FromBranch();
                        _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                        _FromBranch.br_val = dr["comp_nm"].ToString();
                        _FromBranchList.Add(_FromBranch);
                    }
                    _MTRModelData.FromBranchList = _FromBranchList;
                    List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                    _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
                    //foreach (DataRow dr in dtSet.Tables[0].Rows)
                    //{
                    //    ToWharehouse _ToWharehouse = new ToWharehouse();
                    //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                    //    _ToWharehouseList.Add(_ToWharehouse);
                    //}
                    _MTRModelData.ToWharehouseList = _ToWharehouseList;
                    _MTRModelData.hdto_brid = BrchID;

                    List<Req_NO> _Req_NOList = new List<Req_NO>();
                    Req_NO _Req_NO = new Req_NO();
                    _Req_NO.RequirementDate = "0";
                    _Req_NO.RequirementNo = "---Select---";
                    _Req_NOList.Add(_Req_NO);
                    _MTRModelData.Req_NO_List = _Req_NOList;

                    List<MI_NO> _MI_NOList = new List<MI_NO>();
                    MI_NO _MI_NO = new MI_NO();
                    _MI_NO.MIDate = "0";
                    _MI_NO.MINo = "---Select---";
                    _MI_NOList.Add(_MI_NO);
                    _MTRModelData.MI_NO_List = _MI_NOList;
                    _MTRModelData.CompId = CompID;
                    _MTRModelData.BrchID = BrchID;
                    _MTRModelData.hdtrf_type = _MTRModelData.TMR_Type;
                    _MTRModelData.Title = title;
                    _MTRModelData.Req_Date = null;
                    _MTRModelData.Issue_Date = null;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _MTRModelData.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (_MTRModelData.TransType == "Update" || _MTRModelData.TransType == "Edit")
                    {
                        string TMR_no = _MTRModelData.TMR_Number;
                        string TMR_date = _MTRModelData.TMR_Date;
                        DataSet ds = _MTR_ISERVICES.GetTMRDetailByNo(CompID, BrchID, TMR_no, TMR_date, UserID, DocumentMenuId);
                        ViewBag.AttechmentDetails = ds.Tables[7];
                        List<Req_NO> _Req_NOList1 = new List<Req_NO>();
                        Req_NO _Req_NO1 = new Req_NO();
                        _Req_NO1.RequirementDate = ds.Tables[0].Rows[0]["trf_dt"].ToString();
                        _Req_NO1.RequirementNo = ds.Tables[0].Rows[0]["trf_no"].ToString();
                        _Req_NOList1.Add(_Req_NO1);
                        _MTRModelData.Req_NO_List = _Req_NOList1;

                        List<MI_NO> _MI_NOList1 = new List<MI_NO>();
                        MI_NO _MI_NO1 = new MI_NO();
                        _MI_NO1.MIDate = ds.Tables[0].Rows[0]["issue_dt"].ToString();
                        _MI_NO1.MINo = ds.Tables[0].Rows[0]["issue_no"].ToString();
                        _MI_NOList1.Add(_MI_NO1);
                        _MTRModelData.MI_NO_List = _MI_NOList1;

                        _MTRModelData.MaterialReceiptNo = ds.Tables[0].Rows[0]["mr_no"].ToString();
                        _MTRModelData.MaterialReceiptDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["mr_dt"].ToString());
                        _MTRModelData.trf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        _MTRModelData.hdtrf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        _MTRModelData.from_br = ds.Tables[0].Rows[0]["src_br"].ToString();
                        _MTRModelData.from_brid = ds.Tables[0].Rows[0]["src_br"].ToString();
                        _MTRModelData.from_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["src_wh"].ToString());
                        _MTRModelData.to_br = ds.Tables[0].Rows[0]["tobr"].ToString();
                        _MTRModelData.to_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["dstn_wh"].ToString());
                        _MTRModelData.Req_No = ds.Tables[0].Rows[0]["trf_no"].ToString();
                        _MTRModelData.Req_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["trf_dt"].ToString());
                        _MTRModelData.Issue_No = ds.Tables[0].Rows[0]["issue_no"].ToString();
                        _MTRModelData.Issue_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["issue_dt"].ToString());
                        _MTRModelData.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _MTRModelData.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _MTRModelData.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _MTRModelData.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _MTRModelData.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _MTRModelData.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _MTRModelData.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _MTRModelData.Createid = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _MTRModelData.StatusCode = doc_status;
                        List<FromWharehouse> _warehouseList1 = new List<FromWharehouse>();

                        foreach (DataRow dr in ds.Tables[8].Rows)
                        {
                            FromWharehouse _warehouse = new FromWharehouse();
                            _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                            _warehouse.wh_val = dr["wh_name"].ToString();
                            _warehouseList1.Add(_warehouse);
                        }
                        _MTRModelData.FromWharehouseList = _warehouseList1;

                        List<ToWharehouse> _ToWharehouseList1 = new List<ToWharehouse>();
                        foreach (DataRow dr in ds.Tables[9].Rows)
                        {
                            ToWharehouse _ToWharehouse = new ToWharehouse();
                            _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                            _ToWharehouse.wh_val = dr["wh_name"].ToString();
                            _ToWharehouseList1.Add(_ToWharehouse);
                        }
                        _MTRModelData.ToWharehouseList = _ToWharehouseList1;
                        if (_MTRModelData.StatusCode == "C")
                        {
                            _MTRModelData.CancelFlag = true;
                            _MTRModelData.BtnName = "BtnRefresh";
                        }
                        else
                        {
                            _MTRModelData.CancelFlag = false;
                        }

                        _MTRModelData.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _MTRModelData.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _MTRModelData.Command != "Edit")
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
                                    _MTRModelData.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        _MTRModelData.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _MTRModelData.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _MTRModelData.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        _MTRModelData.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    _MTRModelData.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    _MTRModelData.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    _MTRModelData.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    _MTRModelData.BtnName = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        _MTRModelData.DocumentStatus = doc_status; ;
                        //ViewBag.MenuPageName = getDocumentName();
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.ItemStockBatchWise = ds.Tables[2];
                        ViewBag.ItemStockSerialWise = ds.Tables[3];
                        ViewBag.DocumentCode = doc_status;
                        _MTRModelData.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/MaterialTransferReceipt/MaterialTransferReceiptDetail.cshtml", _MTRModelData);
                    }
                    else
                    {

                        //Session["DocumentStatus"] = "New";
                        _MTRModelData.DocumentStatus = "New";
                        //ViewBag.MenuPageName = getDocumentName();
                        _MTRModelData.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/MaterialTransferReceipt/MaterialTransferReceiptDetail.cshtml", _MTRModelData);
                    }
                }
                else
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
                    MaterialTransferReceiptModel _MTRModel = new MaterialTransferReceiptModel();
                    if (_UrlModel.TMR_Number != null || _UrlModel.TMR_Date != null)
                    {
                        _MTRModel.TMR_Number = _UrlModel.TMR_Number;
                        
                        _MTRModel.TMR_Date = _UrlModel.TMR_Date;
                    }
                    if (_UrlModel.TransType != null)
                    {
                        _MTRModel.TransType = _UrlModel.TransType;
                    }
                    else
                    {
                        _MTRModel.TransType = "New";
                    }
                    if (_UrlModel.BtnName != null)
                    {
                        _MTRModel.BtnName = _UrlModel.BtnName;
                    }
                    else
                    {
                        _MTRModel.BtnName = "BtnRefresh";
                    }
                    if (_UrlModel.Command != null)
                    {
                        _MTRModel.Command = _UrlModel.Command;
                    }
                    else
                    {
                        _MTRModel.Command = "Refresh";
                    }
                    if(_UrlModel.WF_status1 != null)
                    {
                        _MTRModel.WF_status1 = _UrlModel.WF_status1;
                    }
                    CommonPageDetails();
                    //var other = new CommonController(_Common_IServices);

                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, BrchID, DocumentMenuId);
                    _MTRModel.to_br = BranchName;
                    _MTRModel.MaterialReceiptDate = DateTime.Now;
                    /*commented by Hina on 18-03-2024 to combine all list Procedure  in single Procedure*/
                    //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    //dt = GetFromWHList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    FromWharehouse _warehouse = new FromWharehouse();
                    //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _warehouse.wh_val = dr["wh_name"].ToString();
                    //    _warehouseList.Add(_warehouse);
                    //}
                    //_MTRModel.FromWharehouseList = _warehouseList;

                    //List<FromBranch> _FromBranchList = new List<FromBranch>();
                    //dt = GetFromBranchList();
                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    FromBranch _FromBranch = new FromBranch();
                    //    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    //    _FromBranch.br_val = dr["comp_nm"].ToString();
                    //    _FromBranchList.Add(_FromBranch);
                    //}
                    //_MTRModel.FromBranchList = _FromBranchList;
                    dtSet = GetAllDDLListAndListPageData("","","","","");
                    /******************Bind FromWharehouse Dropdown*************************/
                    //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    //foreach (DataRow dr in dtSet.Tables[0].Rows)
                    //{
                    //    FromWharehouse _warehouse = new FromWharehouse();
                    //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _warehouse.wh_val = dr["wh_name"].ToString();
                    //    _warehouseList.Add(_warehouse);
                    //}
                    //_MTRModel.FromWharehouseList = _warehouseList;
                    /******************Bind FromBranch Dropdown*************************/
                    //List<FromBranch> _FromBranchList = new List<FromBranch>();
                    //foreach (DataRow dr in dtSet.Tables[1].Rows)
                    //{
                    //    FromBranch _FromBranch = new FromBranch();
                    //    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                    //    _FromBranch.br_val = dr["comp_nm"].ToString();
                    //    _FromBranchList.Add(_FromBranch);
                    //}
                    //_MTRModel.FromBranchList = _FromBranchList;

                    //List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                    ////dt = GetToWHList1(BrchID);
                    //foreach (DataRow dr in dtSet.Tables[0].Rows)
                    //{
                    //    ToWharehouse _ToWharehouse = new ToWharehouse();
                    //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                    //    _ToWharehouseList.Add(_ToWharehouse);
                    //}
                    //_MTRModel.ToWharehouseList = _ToWharehouseList;
                    List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                    _warehouseList.Insert(0, new FromWharehouse() { wh_id = 0, wh_val = "---Select---" });
                    _MTRModel.FromWharehouseList = _warehouseList;
                    List<FromBranch> _FromBranchList = new List<FromBranch>();

                    _FromBranchList.Insert(0, new FromBranch() { br_id = 0, br_val = "---Select---" });
                    foreach (DataRow dr in dtSet.Tables[1].Rows)
                    {
                        FromBranch _FromBranch = new FromBranch();
                        _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                        _FromBranch.br_val = dr["comp_nm"].ToString();
                        _FromBranchList.Add(_FromBranch);
                    }

                    _MTRModel.FromBranchList = _FromBranchList;

                    List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                    _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
                    //foreach (DataRow dr in dtSet.Tables[0].Rows)
                    //{
                    //    ToWharehouse _ToWharehouse = new ToWharehouse();
                    //    _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                    //    _ToWharehouse.wh_val = dr["wh_name"].ToString();
                    //    _ToWharehouseList.Add(_ToWharehouse);
                    //}
                    _MTRModel.ToWharehouseList = _ToWharehouseList;
                    _MTRModel.hdto_brid = BrchID;

                    List<Req_NO> _Req_NOList = new List<Req_NO>();
                    Req_NO _Req_NO = new Req_NO();
                    _Req_NO.RequirementDate = "0";
                    _Req_NO.RequirementNo = "---Select---";
                    _Req_NOList.Add(_Req_NO);
                    _MTRModel.Req_NO_List = _Req_NOList;

                    List<MI_NO> _MI_NOList = new List<MI_NO>();
                    MI_NO _MI_NO = new MI_NO();
                    _MI_NO.MIDate = "0";
                    _MI_NO.MINo = "---Select---";
                    _MI_NOList.Add(_MI_NO);
                    _MTRModel.MI_NO_List = _MI_NOList;
                    _MTRModel.CompId = CompID;
                    _MTRModel.BrchID = BrchID;
                    _MTRModel.Title = title;
                    _MTRModel.Req_Date = null;
                    _MTRModel.Issue_Date = null;
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        _MTRModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (_MTRModel.TransType == "Update" || _MTRModel.TransType == "Edit")
                    {
                        string TMR_no = _MTRModel.TMR_Number;
                        string TMR_date = _MTRModel.TMR_Date;
                        DataSet ds = _MTR_ISERVICES.GetTMRDetailByNo(CompID, BrchID, TMR_no, TMR_date, UserID, DocumentMenuId);
                        ViewBag.AttechmentDetails = ds.Tables[7];
                        List<Req_NO> _Req_NOList1 = new List<Req_NO>();
                        Req_NO _Req_NO1 = new Req_NO();
                        _Req_NO1.RequirementDate = ds.Tables[0].Rows[0]["trf_dt"].ToString();
                        _Req_NO1.RequirementNo = ds.Tables[0].Rows[0]["trf_no"].ToString();
                        _Req_NOList1.Add(_Req_NO1);
                        _MTRModel.Req_NO_List = _Req_NOList1;

                        List<MI_NO> _MI_NOList1 = new List<MI_NO>();
                        MI_NO _MI_NO1 = new MI_NO();
                        _MI_NO1.MIDate = ds.Tables[0].Rows[0]["issue_dt"].ToString();
                        _MI_NO1.MINo = ds.Tables[0].Rows[0]["issue_no"].ToString();
                        _MI_NOList1.Add(_MI_NO1);
                        _MTRModel.MI_NO_List = _MI_NOList1;

                        _MTRModel.MaterialReceiptNo = ds.Tables[0].Rows[0]["mr_no"].ToString();
                        _MTRModel.MaterialReceiptDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["mr_dt"].ToString());
                        _MTRModel.trf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        _MTRModel.hdtrf_type = ds.Tables[0].Rows[0]["trf_type"].ToString();
                        _MTRModel.from_br = ds.Tables[0].Rows[0]["src_br"].ToString();
                        _MTRModel.from_brid = ds.Tables[0].Rows[0]["src_br"].ToString();
                        _MTRModel.from_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["src_wh"].ToString());
                        _MTRModel.to_br = ds.Tables[0].Rows[0]["tobr"].ToString();
                        _MTRModel.to_wh = Convert.ToInt32(ds.Tables[0].Rows[0]["dstn_wh"].ToString());
                        _MTRModel.Req_No = ds.Tables[0].Rows[0]["trf_no"].ToString();
                        _MTRModel.Req_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["trf_dt"].ToString());
                        _MTRModel.Issue_No = ds.Tables[0].Rows[0]["issue_no"].ToString();
                        _MTRModel.Issue_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["issue_dt"].ToString());
                        _MTRModel.CreatedBy = ds.Tables[0].Rows[0]["create_id"].ToString();
                        _MTRModel.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        _MTRModel.ApprovedBy = ds.Tables[0].Rows[0]["app_id"].ToString();
                        _MTRModel.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        _MTRModel.AmmendedBy = ds.Tables[0].Rows[0]["mod_id"].ToString();
                        _MTRModel.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        _MTRModel.Status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        _MTRModel.Createid = ds.Tables[0].Rows[0]["creator_id"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        _MTRModel.StatusCode = doc_status;

                        List<FromWharehouse> _warehouseList1 = new List<FromWharehouse>();

                        foreach (DataRow dr in ds.Tables[8].Rows)
                        {
                            FromWharehouse _warehouse = new FromWharehouse();
                            _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                            _warehouse.wh_val = dr["wh_name"].ToString();
                            _warehouseList1.Add(_warehouse);
                        }
                        _MTRModel.FromWharehouseList = _warehouseList1;

                        List<ToWharehouse> _ToWharehouseList1 = new List<ToWharehouse>();             
                        foreach (DataRow dr in ds.Tables[9].Rows)
                        {
                            ToWharehouse _ToWharehouse = new ToWharehouse();
                            _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                            _ToWharehouse.wh_val = dr["wh_name"].ToString();
                            _ToWharehouseList1.Add(_ToWharehouse);
                        }
                        _MTRModel.ToWharehouseList = _ToWharehouseList1;

                        //_MTRModel.StatusCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        if (_MTRModel.StatusCode == "C")
                        {
                            _MTRModel.CancelFlag = true;
                            //Session["BtnName"] = "BtnRefresh";
                            _MTRModel.BtnName = "BtnRefresh";
                        }
                        else
                        {
                            _MTRModel.CancelFlag = false;
                        }

                        _MTRModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        _MTRModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }
                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && _MTRModel.Command != "Edit")
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
                                    //Session["BtnName"] = "BtnRefresh";
                                    _MTRModel.BtnName = "BtnRefresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _MTRModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _MTRModel.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _MTRModel.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        _MTRModel.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _MTRModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _MTRModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    _MTRModel.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "BtnRefresh";
                                    _MTRModel.BtnName = "BtnRefresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        //Session["DocumentStatus"] = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        //Session["DocumentStatus"] = doc_status;
                        _MTRModel.DocumentStatus = doc_status; ;
                        //ViewBag.MenuPageName = getDocumentName();
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.ItemStockBatchWise = ds.Tables[2];
                        ViewBag.ItemStockSerialWise = ds.Tables[3];
                        //ViewBag.DocumentCode = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        ViewBag.DocumentCode = doc_status;
                        _MTRModel.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/MaterialTransferReceipt/MaterialTransferReceiptDetail.cshtml", _MTRModel);
                    }
                    else
                    {

                        //Session["DocumentStatus"] = "New";
                        _MTRModel.DocumentStatus = "New";
                        //ViewBag.MenuPageName = getDocumentName();
                        _MTRModel.Title = title;
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/MaterialTransferReceipt/MaterialTransferReceiptDetail.cshtml", _MTRModel);
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
        public ActionResult EditTMR(string TMRNO, string TMR_Date, string TMR_Type,string ListFilterData, string WF_status)
        { /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
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
            MaterialTransferReceiptModel DoubleClick = new MaterialTransferReceiptModel();
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["TMR_Number"] = TMRNO.Trim();
            //Session["TMR_Type"] = TMR_Type.Trim();
            //Session["TMR_Date"] = TMR_Date.Trim();
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            DoubleClick.Message = "New";
            DoubleClick.Command = "Add";
            DoubleClick.TMR_Number = TMRNO.Trim();
            DoubleClick.TMR_Type = TMR_Type.Trim();
            DoubleClick.hdtrf_type = TMR_Type.Trim();
            DoubleClick.TMR_Date= TMR_Date.Trim();
            DoubleClick.TransType = "Update";
            DoubleClick.AppStatus = "D";
            DoubleClick.BtnName = "BtnToDetailPage";
            DoubleClick.WF_status1 = WF_status;
            TempData["ModelData"] = DoubleClick;
            URLModelDetails _UrlModel = new URLModelDetails();
            //_UrlModel.Message = "New";
            _UrlModel.Command = "Add";
            _UrlModel.TMR_Number = TMRNO.Trim();
            
            _UrlModel.TMR_Date = TMR_Date.Trim();
            _UrlModel.TransType = "Update";
            //_UrlModel.AppStatus = "D";
            _UrlModel.BtnName = "BtnToDetailPage";
            _UrlModel.WF_status1 = WF_status;
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("MaterialTransferReceiptDetail", "MaterialTransferReceipt", _UrlModel);
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
        [ValidateAntiForgeryToken]
        public ActionResult MaterialTransferReceiptSave(MaterialTransferReceiptModel _MTRModel, string command)
        {
            try
            {/*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/

                if (_MTRModel.DeleteCommand == "Delete")
                    if (true)
                    {
                        command = "Delete";
                    }
                switch (command)
                {
                    case "AddNew":
                        MaterialTransferReceiptModel _MTRModeladdnew = new MaterialTransferReceiptModel();
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";                      
                        _MTRModeladdnew.Command = "Add";                       
                        _MTRModeladdnew.TransType = "Save";
                        _MTRModeladdnew.AppStatus = "D";
                        _MTRModeladdnew.BtnName = "BtnAddNew";
                        TempData["ModelData"] = _MTRModeladdnew;
                        TempData["ListFilterData"] = null;
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                             if (!string.IsNullOrEmpty(_MTRModel.MaterialReceiptNo))
                                return RedirectToAction("EditTMR", new { TMRNO = _MTRModel.MaterialReceiptNo, TMR_Date = _MTRModel.MaterialReceiptDate, TMR_Type = _MTRModel.hdtrf_type, ListFilterData = _MTRModel.ListFilterData1, WF_status = _MTRModel.WFStatus });
                            else
                                _MTRModeladdnew.Command = "Refresh";
                            _MTRModeladdnew.TransType = "Refresh";
                            _MTRModeladdnew.BtnName = "BtnRefresh";
                            _MTRModeladdnew.DocumentStatus = null;
                            TempData["ModelData"] = _MTRModeladdnew;
                            return RedirectToAction("MaterialTransferReceiptDetail", _MTRModeladdnew);
                        }
                        /*End to chk Financial year exist or not*/
                        return RedirectToAction("MaterialTransferReceiptDetail");

                    case "Edit":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditTMR", new { TMRNO = _MTRModel.MaterialReceiptNo, TMR_Date = _MTRModel.MaterialReceiptDate, TMR_Type = _MTRModel.hdtrf_type, ListFilterData = _MTRModel.ListFilterData1, WF_status = _MTRModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string rcptDt = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, rcptDt) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditTMR", new { TMRNO = _MTRModel.MaterialReceiptNo, TMR_Date = _MTRModel.MaterialReceiptDate, TMR_Type = _MTRModel.hdtrf_type, ListFilterData = _MTRModel.ListFilterData1, WF_status = _MTRModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["TransType"] = "Update";
                        //Session["Command"] = command;
                        //Session["Message"] = null;
                        //Session["BtnName"] = "BtnEdit";
                        //Session["TMR_Number"] = _MTRModel.MaterialReceiptNo;
                        //Session["TMR_Date"] = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        _MTRModel.Message = null;
                        _MTRModel.Command = command;
                        _MTRModel.TransType = "Update";
                        _MTRModel.AppStatus = "D";
                        _MTRModel.TMR_Number = _MTRModel.MaterialReceiptNo;
                        _MTRModel.TMR_Date = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        _MTRModel.BtnName = "BtnEdit";
                        TempData["ModelData"] = _MTRModel;
                        URLModelDetails _UrlModel = new URLModelDetails();
                        _UrlModel.TransType = "Update";
                        _UrlModel.Command = command;
                        _UrlModel.TMR_Number = _MTRModel.MaterialReceiptNo;
                        _UrlModel.TMR_Date = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        _UrlModel.BtnName = "BtnEdit";
                        TempData["ListFilterData"] = _MTRModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferReceiptDetail", _UrlModel);

                    case "Delete":
                        //Session["Command"] = command;
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["TMR_Number"] = _MTRModel.MaterialReceiptNo;
                        //Session["TMR_Date"] = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");

                        _MTRModel.Command = command;
                        _MTRModel.TMR_Number = _MTRModel.MaterialReceiptNo;
                        _MTRModel.TMR_Number = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        MTRDelete(_MTRModel, command);
                        MaterialTransferReceiptModel _MTRModelDelete = new MaterialTransferReceiptModel();
                       
                        _MTRModelDelete.BtnName = "BtnDelete";
                        _MTRModelDelete.Message = "Deleted";
                        _MTRModelDelete.AppStatus = "DL";
                        _MTRModelDelete.TransType = "Refresh";
                        _MTRModelDelete.Command = "Refresh";
                        TempData["ModelData"] = _MTRModelDelete;
                        TempData["ListFilterData"] = _MTRModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferReceiptDetail");


                    case "Save":
                        //Session["Command"] = command;
                        _MTRModel.Command= command;
                        SaveMaterialTransferReceipt(_MTRModel);
                        if (_MTRModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (_MTRModel.Message == "DocModify")
                        {
                            CommonPageDetails();
                            _MTRModel.to_br = BranchName;
                            _MTRModel.MaterialReceiptDate = DateTime.Now;
                            /*commented by Hina on 18-03-2024 to combine all list Procedure  in single Procedure*/
                            //List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                            //dt = GetFromWHList();
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    FromWharehouse _warehouse = new FromWharehouse();
                            //    _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                            //    _warehouse.wh_val = dr["wh_name"].ToString();
                            //    _warehouseList.Add(_warehouse);
                            //}
                            //_MTRModel.FromWharehouseList = _warehouseList;

                            //List<FromBranch> _FromBranchList = new List<FromBranch>();
                            //dt = GetFromBranchList();
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    FromBranch _FromBranch = new FromBranch();
                            //    _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                            //    _FromBranch.br_val = dr["comp_nm"].ToString();
                            //    _FromBranchList.Add(_FromBranch);
                            //}
                            //_MTRModel.FromBranchList = _FromBranchList;
                            dtSet = GetAllDDLListAndListPageData("","","","","");
                            /******************Bind FromWharehouse Dropdown*************************/
                            List<FromWharehouse> _warehouseList = new List<FromWharehouse>();
                            foreach (DataRow dr in dtSet.Tables[0].Rows)
                            {
                                FromWharehouse _warehouse = new FromWharehouse();
                                _warehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                _warehouse.wh_val = dr["wh_name"].ToString();
                                _warehouseList.Add(_warehouse);
                            }
                            _MTRModel.FromWharehouseList = _warehouseList;
                            /******************Bind FromBranch Dropdown*************************/
                            List<FromBranch> _FromBranchList = new List<FromBranch>();
                            foreach (DataRow dr in dtSet.Tables[1].Rows)
                            {
                                FromBranch _FromBranch = new FromBranch();
                                _FromBranch.br_id = Convert.ToInt32(dr["Comp_Id"]);
                                _FromBranch.br_val = dr["comp_nm"].ToString();
                                _FromBranchList.Add(_FromBranch);
                            }
                            _MTRModel.FromBranchList = _FromBranchList;

                            List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>();
                            //dt = GetToWHList1(BrchID);
                            foreach (DataRow dr in dtSet.Tables[0].Rows)
                            {
                                ToWharehouse _ToWharehouse = new ToWharehouse();
                                _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
                                _ToWharehouse.wh_val = dr["wh_name"].ToString();
                                _ToWharehouseList.Add(_ToWharehouse);
                            }
                            _MTRModel.ToWharehouseList = _ToWharehouseList;
                            _MTRModel.hdto_brid = BrchID;

                            var reqNumber = _MTRModel.hdReq_No;
                            var reqDate = _MTRModel.hdReq_Dt;
                            List<Req_NO> _Req_NOList = new List<Req_NO>();
                            Req_NO _Req_NO = new Req_NO();
                            _Req_NO.RequirementDate = reqDate;
                            _Req_NO.RequirementNo = reqNumber;
                            _Req_NOList.Add(_Req_NO);
                            _MTRModel.Req_NO_List = _Req_NOList;

                            var issuNumber = _MTRModel.hdIssue_No;
                            var issuDt = _MTRModel.hdIssue_dt;
                            List<MI_NO> _MI_NOList = new List<MI_NO>();
                            MI_NO _MI_NO = new MI_NO();
                            _MI_NO.MIDate = issuDt;
                            _MI_NO.MINo = issuNumber;
                            _MI_NOList.Add(_MI_NO);
                            _MTRModel.MI_NO_List = _MI_NOList;
                            _MTRModel.CompId = CompID;
                            _MTRModel.BrchID = BrchID;
                            _MTRModel.Title = title;
                            _MTRModel.Issue_Date = Convert.ToDateTime(issuDt);
                            _MTRModel.Req_Date = Convert.ToDateTime(reqDate);
                            _MTRModel.trf_type = _MTRModel.hdtrf_type;
                            _MTRModel.from_br = _MTRModel.from_brid;
                            _MTRModel.from_wh = _MTRModel.hdfrom_whid;
                            var destBranch = _MTRModel.hdto_bridName;
                            _MTRModel.to_br = destBranch;

                            ViewBag.ItemDetails = ViewData["ItemMTIDetails"];
                            ViewBag.ItemStockBatchWise = ViewData["BatchDetails"];
                            ViewBag.ItemStockSerialWise = ViewData["SerialDetail"];
                            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/MaterialTransferReceipt/MaterialTransferReceiptDetail.cshtml", _MTRModel);
                        }
                        if (_MTRModel.TMR_Number != null)
                        {
                            //Session["TMR_Number"] = Session["TMR_Number"].ToString();
                            //Session["TMR_Date"] = Session["TMR_Date"].ToString();
                            TempData["ModelData"] = _MTRModel;
                            URLModelDetails URLModelSave = new URLModelDetails();
                            URLModelSave.BtnName = _MTRModel.BtnName;
                            URLModelSave.TransType = _MTRModel.TransType;
                            URLModelSave.Command = _MTRModel.Command;
                            URLModelSave.TMR_Number = _MTRModel.TMR_Number;
                            URLModelSave.TMR_Date = _MTRModel.TMR_Date;
                            TempData["ListFilterData"] = _MTRModel.ListFilterData1;
                            return RedirectToAction("MaterialTransferReceiptDetail", URLModelSave);
                        }
                        else
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }

                    case "Refresh":
                        MaterialTransferReceiptModel _MTRModelRefresh = new MaterialTransferReceiptModel();
                        _MTRModelRefresh.BtnName= "BtnRefresh";
                        _MTRModelRefresh.Command = command;
                        _MTRModelRefresh.TransType = "Save";
                        _MTRModelRefresh.Message = null;
                        _MTRModelRefresh.DocumentStatus = null;
                        TempData["ModelData"] = _MTRModelRefresh;
                        //Session["BtnName"] = "BtnRefresh";
                        //Session["Command"] = command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        
                        TempData["ListFilterData"] = _MTRModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferReceiptDetail");

                    case "Approve":
                        /*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            BrchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditTMR", new { TMRNO = _MTRModel.MaterialReceiptNo, TMR_Date = _MTRModel.MaterialReceiptDate, TMR_Type = _MTRModel.hdtrf_type, ListFilterData = _MTRModel.ListFilterData1, WF_status = _MTRModel.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 13-05-2025 to check Existing with previous year transaction*/
                        string rcptDt1 = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, BrchID, rcptDt1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditTMR", new { TMRNO = _MTRModel.MaterialReceiptNo, TMR_Date = _MTRModel.MaterialReceiptDate, TMR_Type = _MTRModel.hdtrf_type, ListFilterData = _MTRModel.ListFilterData1, WF_status = _MTRModel.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = command;
                        _MTRModel.Command = command;
                        _MTRModel.TMR_Number = _MTRModel.MaterialReceiptNo;
                        _MTRModel.TMR_Date = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        //Session["TMR_Number"] = _MTRModel.MaterialReceiptNo;
                        //Session["TMR_Date"] = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        Approve_TMR(_MTRModel.MaterialReceiptNo, _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd"), "","", "","","", "", _MTRModel.from_brid);
                        _MTRModel.TMR_Number = _MTRModel.MaterialReceiptNo;
                        _MTRModel.TMR_Date = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                        _MTRModel.Message = "Approved";
                        _MTRModel.Command = "Approve";
                        _MTRModel.TransType = "Update";
                        _MTRModel.AppStatus = "D";
                        _MTRModel.BtnName = "BtnEdit";
                        _MTRModel.WF_status1 = TempData["WF_Status1"].ToString();
                        TempData["ModelData"] = _MTRModel;
                        URLModelDetails _approve_TMRModel = new URLModelDetails();
                        _approve_TMRModel.TMR_Number = _MTRModel.TMR_Number;
                        _approve_TMRModel.WF_status1 = _MTRModel.WF_status1;
                        _approve_TMRModel.TMR_Date = _MTRModel.TMR_Date;
                        _approve_TMRModel.TransType = "Update";
                        _approve_TMRModel.BtnName = "BtnToDetailPage";
                       
                        TempData["ListFilterData"] = _MTRModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferReceiptDetail", _approve_TMRModel);

                    case "Print":
                        return GenratePdfFile(_MTRModel);
                    case "BacktoList":
                        //Session.Remove("Message");
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        TMR_ListModel TMRDashBord = new TMR_ListModel();
                        TMRDashBord.WF_status = _MTRModel.WF_status1;
                        TempData["ListFilterData"] = _MTRModel.ListFilterData1;
                        return RedirectToAction("MaterialTransferReceipt", "MaterialTransferReceipt", TMRDashBord);

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
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                MTRCModelAttch _MTRCModelAttch = new MTRCModelAttch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
               // string MaterialReceiptNo = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["TMR_Number"] != null)
                //{
                //    MaterialReceiptNo = Session["TMR_Number"].ToString();
                //}
                //if (TransType == "Save")
                //{
                //    MaterialReceiptNo = gid.ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = MaterialReceiptNo;
                _MTRCModelAttch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
               // getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + BrchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _MTRCModelAttch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _MTRCModelAttch.AttachMentDetailItmStp = null;
                }
                TempData["AttchData"] = _MTRCModelAttch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }

        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
        }
        public ActionResult SaveMaterialTransferReceipt(MaterialTransferReceiptModel _MTRModel)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            if (Session["compid"] != null)
            {
                CompID = Session["compid"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {
                TempData["ListFilterData"] = _MTRModel.ListFilterData1;
                if (_MTRModel.CancelFlag == false)
                {
                    var commonContr = new CommonController();
                    DataTable MaterialTransferReceiptHeader = new DataTable();
                    DataTable MaterialTransferReceiptItemDetails = new DataTable();
                    DataTable ItemBatchDetails = new DataTable();
                    DataTable ItemSerialDetails = new DataTable();
                    DataTable Attachments = new DataTable();

                    DataTable dtheader = new DataTable();
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("trf_type", typeof(string));
                    dtheader.Columns.Add("mr_no", typeof(string));
                    dtheader.Columns.Add("mr_dt", typeof(string));
                    dtheader.Columns.Add("src_br", typeof(int));
                    dtheader.Columns.Add("src_wh", typeof(int));
                    dtheader.Columns.Add("dstn_br", typeof(int));
                    dtheader.Columns.Add("dstn_wh", typeof(int));
                    dtheader.Columns.Add("trf_no", typeof(string));
                    dtheader.Columns.Add("trf_dt", typeof(string));
                    dtheader.Columns.Add("issue_no", typeof(string));
                    dtheader.Columns.Add("issue_dt", typeof(string));                   
                    dtheader.Columns.Add("create_id", typeof(string));                   
                    dtheader.Columns.Add("mr_status", typeof(string));
                    dtheader.Columns.Add("UserMacaddress", typeof(string));
                    dtheader.Columns.Add("UserSystemName", typeof(string));
                    dtheader.Columns.Add("UserIP", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    //dtrowHeader["TransType"] = Session["TransType"].ToString();
                    if (_MTRModel.MaterialReceiptNo != null)
                    {
                        _MTRModel.TransType = "Update";
                    }
                    else
                    {
                        _MTRModel.TransType = "Save";
                    }
                    dtrowHeader["TransType"] = _MTRModel.TransType;
                    dtrowHeader["comp_id"] = Session["CompId"].ToString();
                    dtrowHeader["br_id"] = Session["BranchId"].ToString();
                    dtrowHeader["trf_type"] = _MTRModel.hdtrf_type;
                    dtrowHeader["mr_no"] = _MTRModel.MaterialReceiptNo;
                    //dtrowHeader["mr_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                    dtrowHeader["mr_dt"] = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                    dtrowHeader["issue_no"] = _MTRModel.Issue_No;
                    dtrowHeader["issue_dt"] = _MTRModel.Issue_Date;
                    dtrowHeader["src_br"] = _MTRModel.from_brid;
                    dtrowHeader["src_wh"] = _MTRModel.hdfrom_whid;
                    dtrowHeader["dstn_wh"] = _MTRModel.to_wh;                   
                    dtrowHeader["dstn_br"] = Session["BranchId"].ToString();                  
                    dtrowHeader["trf_no"] = _MTRModel.Req_No;
                    dtrowHeader["trf_dt"] = _MTRModel.Req_Date;
                    dtrowHeader["create_id"] = Session["UserId"].ToString();
                    //dtrowHeader["mr_status"] = Session["AppStatus"].ToString();
                    dtrowHeader["mr_status"] = IsNull(_MTRModel.AppStatus, "D");
                    dtrowHeader["UserMacaddress"] = Session["UserMacaddress"].ToString();
                    dtrowHeader["UserSystemName"] = Session["UserSystemName"].ToString();
                    dtrowHeader["UserIP"] = Session["UserIP"].ToString();
                    dtheader.Rows.Add(dtrowHeader);
                    MaterialTransferReceiptHeader = dtheader;

                    DataTable dtItem = new DataTable();
                    dtItem.Columns.Add("comp_id", typeof(int));
                    dtItem.Columns.Add("br_id", typeof(int));
                    dtItem.Columns.Add("trf_type", typeof(string));
                    dtItem.Columns.Add("mr_no", typeof(string));
                    dtItem.Columns.Add("mr_dt", typeof(string));
                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("req_qty", typeof(string));
                    dtItem.Columns.Add("pend_qty", typeof(string));
                    dtItem.Columns.Add("rec_qty", typeof(string));
                    dtItem.Columns.Add("wh_id", typeof(string));
                    dtItem.Columns.Add("lot_id", typeof(string));
                    //dtItem.Columns.Add("item_landed_rate", typeof(string));
                    //dtItem.Columns.Add("item_landed_val", typeof(string));

                    JArray jObject = JArray.Parse(_MTRModel.Itemdetails);
                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();
                        dtrowLines["comp_id"] = Session["CompId"].ToString();
                        dtrowLines["br_id"] = Session["BranchId"].ToString();
                        dtrowLines["trf_type"] = _MTRModel.hdtrf_type;
                        dtrowLines["mr_no"] = _MTRModel.MaterialReceiptNo;
                        dtrowLines["mr_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                        dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                        dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                        dtrowLines["req_qty"] = jObject[i]["mrs_qty"].ToString();
                        dtrowLines["pend_qty"] = jObject[i]["pend_qty"].ToString();
                        dtrowLines["rec_qty"] = jObject[i]["rec_qty"].ToString();
                        dtrowLines["wh_id"] = jObject[i]["WareHouseId"].ToString();
                        dtrowLines["lot_id"] = "0";

                        dtItem.Rows.Add(dtrowLines);
                    }
                    ViewData["ItemMTIDetails"] = dtitemdetail(jObject, _MTRModel);
                    MaterialTransferReceiptItemDetails = dtItem;

                    DataTable Batch_detail = new DataTable();
                    Batch_detail.Columns.Add("comp_id", typeof(int));
                    Batch_detail.Columns.Add("br_id", typeof(int));
                    Batch_detail.Columns.Add("trf_type", typeof(string));
                    Batch_detail.Columns.Add("mr_no", typeof(string));
                    Batch_detail.Columns.Add("mr_dt", typeof(string));
                    Batch_detail.Columns.Add("item_id", typeof(string));                  
                    Batch_detail.Columns.Add("batch_no", typeof(string));
                    Batch_detail.Columns.Add("batch_qty", typeof(string));
                    Batch_detail.Columns.Add("expiry_date", typeof(string));                  
                    Batch_detail.Columns.Add("lot_no", typeof(string));
                    Batch_detail.Columns.Add("mfg_name", typeof(string));
                    Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                    Batch_detail.Columns.Add("mfg_date", typeof(string));
                    if (_MTRModel.ItemBatchWiseDetail != null)
                    {
                        JArray jObjectBatch = JArray.Parse(_MTRModel.ItemBatchWiseDetail);
                        for (int i = 0; i < jObjectBatch.Count; i++)
                        {
                            DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                            dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                            dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                            dtrowBatchDetailsLines["trf_type"] = _MTRModel.hdtrf_type;
                            dtrowBatchDetailsLines["mr_no"] = _MTRModel.MaterialReceiptNo;
                            dtrowBatchDetailsLines["mr_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                            dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["item_id"].ToString();                           
                            dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["batch_no"].ToString();
                            dtrowBatchDetailsLines["batch_qty"] = jObjectBatch[i]["batch_qty"].ToString();
                            if (jObjectBatch[i]["exp_date"].ToString() == "" || jObjectBatch[i]["exp_date"].ToString() == null)
                            {
                                dtrowBatchDetailsLines["expiry_date"] = "01-Jan-1900";
                            }
                            else
                            {
                                dtrowBatchDetailsLines["expiry_date"] = jObjectBatch[i]["exp_date"].ToString();
                            }
                            //dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                            dtrowBatchDetailsLines["lot_no"] = jObjectBatch[i]["lot_id"].ToString();
                            dtrowBatchDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectBatch[i]["mfg_name"].ToString(), null);
                            dtrowBatchDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectBatch[i]["mfg_mrp"].ToString(), null);
                            dtrowBatchDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectBatch[i]["mfg_date"].ToString(), null);
                            Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                        }
                        ViewData["BatchDetails"] = dtBatchDetails(jObjectBatch, _MTRModel);
                    }
                    ItemBatchDetails = Batch_detail;

                    DataTable Serial_detail = new DataTable();
                    Serial_detail.Columns.Add("comp_id", typeof(int));
                    Serial_detail.Columns.Add("br_id", typeof(int));
                    Serial_detail.Columns.Add("trf_type", typeof(string));
                    Serial_detail.Columns.Add("mr_no", typeof(string));
                    Serial_detail.Columns.Add("mr_dt", typeof(string));
                    Serial_detail.Columns.Add("item_id", typeof(string));       
                    Serial_detail.Columns.Add("serial_no", typeof(string));
                    Serial_detail.Columns.Add("serial_qty", typeof(string));                    
                    Serial_detail.Columns.Add("lot_no", typeof(string));
                    Serial_detail.Columns.Add("mfg_name", typeof(string));
                    Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                    Serial_detail.Columns.Add("mfg_date", typeof(string));

                    if (_MTRModel.ItemSerialWiseDetail != null)
                    {
                        JArray jObjectSerial = JArray.Parse(_MTRModel.ItemSerialWiseDetail);
                        for (int i = 0; i < jObjectSerial.Count; i++)
                        {
                            DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                            dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                            dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();
                            dtrowSerialDetailsLines["trf_type"] = _MTRModel.hdtrf_type;
                            dtrowSerialDetailsLines["mr_no"] = _MTRModel.MaterialReceiptNo;
                            dtrowSerialDetailsLines["mr_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                            dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemID"].ToString();
                            //dtrowSerialDetailsLines["uom_id"] = jObjectSerial[i]["UOMId"].ToString();
                            dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNo"].ToString();
                            //dtrowSerialDetailsLines["serial_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            //dtrowSerialDetailsLines["issue_qty"] = jObjectSerial[i]["IssuedQuantity"].ToString();
                            dtrowSerialDetailsLines["lot_no"] = jObjectSerial[i]["serial_lot_id"].ToString();
                            dtrowSerialDetailsLines["mfg_name"] = commonContr.IsBlank(jObjectSerial[i]["mfg_name"].ToString(), null);
                            dtrowSerialDetailsLines["mfg_mrp"] = commonContr.IsBlank(jObjectSerial[i]["mfg_mrp"].ToString(), null);
                            dtrowSerialDetailsLines["mfg_date"] = commonContr.IsBlank(jObjectSerial[i]["mfg_date"].ToString(), null);
                            Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                        }
                        ViewData["SerialDetail"] = dtSerialDetails(jObjectSerial, _MTRModel);
                    }
                    ItemSerialDetails = Serial_detail;

                    DataTable dtAttachment = new DataTable();
                    var _MTRCModelAttch = TempData["AttchData"] as MTRCModelAttch;
                    TempData["AttchData"] = null;
                    if (_MTRModel.attatchmentdetail != null)
                    {
                        if (_MTRCModelAttch != null)
                        {
                            if (_MTRCModelAttch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _MTRCModelAttch.AttachMentDetailItmStp as DataTable;
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
                            if (_MTRModel.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _MTRModel.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(_MTRModel.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(_MTRModel.MaterialReceiptNo))
                                {
                                    dtrowAttachment1["id"] = _MTRModel.MaterialReceiptNo;
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
                        if (_MTRModel.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(_MTRModel.MaterialReceiptNo))
                                {
                                    ItmCode = _MTRModel.MaterialReceiptNo;
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
                        Attachments = dtAttachment;
                    }
                    SaveMessage = _MTR_ISERVICES.InsertUpdateMaterialTransferReceipt(MaterialTransferReceiptHeader, MaterialTransferReceiptItemDetails, ItemBatchDetails, ItemSerialDetails, Attachments);
                    if (SaveMessage == "DocModify")
                    {
                        _MTRModel.Message = "DocModify";
                        _MTRModel.BtnName = "BtnRefresh";
                        _MTRModel.Command = "Refresh";
                        TempData["ModelData"] = _MTRModel;
                        return RedirectToAction("MaterialTransferReceiptDetail");
                    }
                    else
                    {
                        string[] FDate = SaveMessage.Split(',');

                        string Message = FDate[0].ToString();
                        string TMR_Number = FDate[1].ToString();
                        string TMR_Date = FDate[2].ToString();
                        string TMR_Type = FDate[3].ToString();

                        if (Message == "Data_Not_Found")
                        {
                            var a = TMR_Number.Split(',');
                            var msg = Message.Replace("_", " ") + " " + a[0].Trim() + " in " + PageName;
                            //var msg = "Data Not Found" +" "+ a[0].Trim();
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            _MTRModel.Message = Message.Replace("_", "");
                            return RedirectToAction("MaterialTransferReceiptDetail");
                        }
                        if (Message == "Save")
                        {
                            string Guid = "";
                            if (_MTRCModelAttch != null)
                            {
                                if (_MTRCModelAttch.Guid != null)
                                {
                                    Guid = _MTRCModelAttch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, BrchID, guid, PageName, TMR_Number, _MTRModel.TransType, Attachments);
                            //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //if (Directory.Exists(sourcePath))
                            //{
                            //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + BrchID + Guid + "_" + "*");
                            //    foreach (string file in filePaths)
                            //    {
                            //        string[] items = file.Split('\\');
                            //        string ItemName = items[items.Length - 1];
                            //        ItemName = ItemName.Substring(ItemName.IndexOf('_') + 1);
                            //        foreach (DataRow dr in Attachments.Rows)
                            //        {
                            //            string DrItmNm = dr["file_name"].ToString();
                            //            if (ItemName == DrItmNm)
                            //            {
                            //                string TMR_Number1 = TMR_Number.Replace("/", "");
                            //                string img_nm = CompID + BrchID + TMR_Number1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                        if (Message == "Update" || Message == "Save")
                        {
                            _MTRModel.Message = "Save";
                            _MTRModel.Command = "Update";
                            _MTRModel.TMR_Number = TMR_Number;
                            _MTRModel.TMR_Date = TMR_Date;
                            _MTRModel.TMR_Type = TMR_Type;
                            _MTRModel.TransType = "Update";
                            _MTRModel.AppStatus = "D";
                            _MTRModel.BtnName = "BtnSave";
                            _MTRModel.AttachMentDetailItmStp = null;
                            _MTRModel.Guid = null;
                            TempData["ModelData"] = _MTRModel;
                        }
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
                    _MTRModel.CreatedBy = userid;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    string TMR_Date = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                    string TMR_Type = _MTRModel.trf_type;

                    DataSet message = _MTR_ISERVICES.MaterialTransferReceiptCancel(_MTRModel, CompID, br_id, mac_id, TMR_Date);
                    try
                    {
                        //string fileName = "MTR__" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = "MaterialTransferReceipt_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(_MTRModel.MaterialReceiptNo, _MTRModel.MaterialReceiptDate.ToString(), fileName, DocumentMenuId,"C");
                        _Common_IServices.SendAlertEmail(CompID, br_id, DocumentMenuId, _MTRModel.MaterialReceiptNo, "C", userid, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _MTRModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _MTRModel.TMR_Number = _MTRModel.MaterialReceiptNo;
                    _MTRModel.TMR_Date = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
                    _MTRModel.TMR_Type = _MTRModel.trf_type;
                    //_MTRModel.Message = "Cancelled";
                    _MTRModel.Message = _MTRModel.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                    _MTRModel.Command = "Update";                  
                    _MTRModel.TransType = "Update";
                    _MTRModel.AppStatus = "D";
                    _MTRModel.BtnName = "Refresh";
                    TempData["ModelData"] = _MTRModel;                 
                    return RedirectToAction("MaterialTransferIssueDetail");
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    if(_MTRModel.TransType == "Save")
                    {
                        string Guid = "";
                        if(_MTRModel.Guid != null) 
                        {
                            Guid = _MTRModel.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID+ BrchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public DataTable dtitemdetail(JArray jObject, MaterialTransferReceiptModel _MTRModel)
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("comp_id", typeof(int));
            dtItem.Columns.Add("br_id", typeof(int));
            dtItem.Columns.Add("trf_type", typeof(string));
            dtItem.Columns.Add("mr_no", typeof(string));
            dtItem.Columns.Add("mr_dt", typeof(string));
            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("req_qty", typeof(string));
            dtItem.Columns.Add("pend_qty", typeof(string));
            dtItem.Columns.Add("rec_qty", typeof(string));
            dtItem.Columns.Add("wh_id", typeof(string));
            dtItem.Columns.Add("lot_id", typeof(string));
            dtItem.Columns.Add("i_batch", typeof(string));
            dtItem.Columns.Add("i_serial", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("item_Name", typeof(string));
            dtItem.Columns.Add("base_uom_id", typeof(int));
            dtItem.Columns.Add("uom_Name", typeof(string));
            dtItem.Columns.Add("wh_name", typeof(string));

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();
                dtrowLines["comp_id"] = Session["CompId"].ToString();
                dtrowLines["br_id"] = Session["BranchId"].ToString();
                dtrowLines["trf_type"] = _MTRModel.hdtrf_type;
                dtrowLines["mr_no"] = _MTRModel.MaterialReceiptNo;
                dtrowLines["mr_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                dtrowLines["base_uom_id"] = jObject[i]["UOMId"].ToString();
                dtrowLines["req_qty"] = jObject[i]["mrs_qty"].ToString();
                dtrowLines["pend_qty"] = jObject[i]["pend_qty"].ToString();
                dtrowLines["rec_qty"] = jObject[i]["rec_qty"].ToString();
                dtrowLines["wh_id"] = jObject[i]["WareHouseId"].ToString();
                dtrowLines["lot_id"] = "0";
                dtrowLines["i_batch"] = jObject[i]["i_batch"].ToString();
                dtrowLines["i_serial"] = jObject[i]["i_serial"].ToString();
                dtrowLines["sub_item"] = jObject[i]["sub_item"].ToString();
                dtrowLines["item_Name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["uom_Name"] = jObject[i]["UOM"].ToString();
                dtrowLines["wh_name"] = jObject[i]["WHName"].ToString();

                dtItem.Rows.Add(dtrowLines);
            }
            return dtItem;
        }
        public DataTable dtBatchDetails(JArray jObjectBatch, MaterialTransferReceiptModel _MTRModel)
        {
            DataTable Batch_detail = new DataTable();
            Batch_detail.Columns.Add("comp_id", typeof(int));
            Batch_detail.Columns.Add("br_id", typeof(int));
            Batch_detail.Columns.Add("trf_type", typeof(string));
            Batch_detail.Columns.Add("mr_no", typeof(string));
            Batch_detail.Columns.Add("mr_dt", typeof(string));
            Batch_detail.Columns.Add("item_id", typeof(string));
            Batch_detail.Columns.Add("batch_no", typeof(string));
            Batch_detail.Columns.Add("batch_qty", typeof(string));
            Batch_detail.Columns.Add("IssueQty", typeof(string));
            Batch_detail.Columns.Add("exp_dt", typeof(string));
            Batch_detail.Columns.Add("exp_date", typeof(string));
            Batch_detail.Columns.Add("lot_no", typeof(string));

            for (int i = 0; i < jObjectBatch.Count; i++)
            {
                DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                dtrowBatchDetailsLines["comp_id"] = Session["CompId"].ToString();
                dtrowBatchDetailsLines["br_id"] = Session["BranchId"].ToString();
                dtrowBatchDetailsLines["trf_type"] = _MTRModel.hdtrf_type;
                dtrowBatchDetailsLines["mr_no"] = _MTRModel.MaterialReceiptNo;
                dtrowBatchDetailsLines["mr_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["item_id"].ToString();
                dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["batch_no"].ToString();
                dtrowBatchDetailsLines["batch_qty"] = jObjectBatch[i]["batch_qty"].ToString();
                dtrowBatchDetailsLines["IssueQty"] = jObjectBatch[i]["batch_qty"].ToString();
                dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["exp_date"].ToString();
                if (jObjectBatch[i]["exp_date"].ToString() == "" || jObjectBatch[i]["exp_date"].ToString() == null)
                {
                    dtrowBatchDetailsLines["exp_date"] = "01-Jan-1900";
                }
                else
                {
                    dtrowBatchDetailsLines["exp_date"] = jObjectBatch[i]["exp_date"].ToString();
                }                
                //dtrowBatchDetailsLines["issue_qty"] = jObjectBatch[i]["IssueQty"].ToString();
                dtrowBatchDetailsLines["lot_no"] = "0";
                Batch_detail.Rows.Add(dtrowBatchDetailsLines);
            }
            return Batch_detail;
        }
        public DataTable dtSerialDetails(JArray jObjectSerial, MaterialTransferReceiptModel _MTRModel)
        {
            DataTable Serial_detail = new DataTable();
            Serial_detail.Columns.Add("comp_id", typeof(int));
            Serial_detail.Columns.Add("br_id", typeof(int));
            Serial_detail.Columns.Add("trf_type", typeof(string));
            Serial_detail.Columns.Add("mr_no", typeof(string));
            Serial_detail.Columns.Add("mr_dt", typeof(string));
            Serial_detail.Columns.Add("item_id", typeof(string));
            Serial_detail.Columns.Add("serial_no", typeof(string));
            Serial_detail.Columns.Add("serial_qty", typeof(string));
            Serial_detail.Columns.Add("lot_no", typeof(string));

            for (int i = 0; i < jObjectSerial.Count; i++)
            {
                DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                dtrowSerialDetailsLines["comp_id"] = Session["CompId"].ToString();
                dtrowSerialDetailsLines["br_id"] = Session["BranchId"].ToString();
                dtrowSerialDetailsLines["trf_type"] = _MTRModel.hdtrf_type;
                dtrowSerialDetailsLines["mr_no"] = _MTRModel.MaterialReceiptNo;
                dtrowSerialDetailsLines["mr_dt"] = DateTime.Now.ToString("yyyy-MM-dd");
                dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["ItemID"].ToString();
                dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["SerialNo"].ToString();
                dtrowSerialDetailsLines["lot_no"] = "0";
                Serial_detail.Rows.Add(dtrowSerialDetailsLines);
            }
            return Serial_detail;
        }
        //[HttpPost]
        public ActionResult Approve_TMR(string MTR_no, string MTR_date, string A_Status, string A_Level, string A_Remarks,string ListFilterData1,string WF_Status1, string docid,string SrcBranch)
        {
            try
            {
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string BranchID = string.Empty;
                string MenuDocId = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["UserId"] != null)
                {
                    UserID = Session["UserId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    BranchID = Session["BranchId"].ToString();
                }
                if (docid != null)
                {
                    DocumentMenuId = docid;
                }
                MaterialTransferReceiptModel _MTRModel = new MaterialTransferReceiptModel();
                _MTRModel.CreatedBy = Session["UserId"].ToString();
                _MTRModel.MaterialReceiptNo = MTR_no;
                _MTRModel.MaterialReceiptDate = Convert.ToDateTime(MTR_date);
                _MTRModel.from_brid = SrcBranch;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string GrnDetail = _MTR_ISERVICES.Approve_TMR(_MTRModel,BranchID, Comp_ID, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId);
                try
                {
                    //string fileName = "MTR__" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    string fileName = "MaterialTransferReceipt_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                    var filePath = SavePdfDocToSendOnEmailAlert(_MTRModel.MaterialReceiptNo, _MTRModel.MaterialReceiptDate.ToString(), fileName, DocumentMenuId,"A");
                    _Common_IServices.SendAlertEmail(Comp_ID, BranchID, DocumentMenuId, _MTRModel.MaterialReceiptNo, "A", UserID, "", filePath);
                }
                catch (Exception exMail)
                {
                    _MTRModel.Message = "ErrorInMail";
                    string path = Server.MapPath("~");
                    Errorlog.LogError(path, exMail);
                }
                _MTRModel.TMR_Number = _MTRModel.MaterialReceiptNo;
                _MTRModel.TMR_Date = _MTRModel.MaterialReceiptDate.ToString("yyyy-MM-dd");
               // _MTRModel.Message = "Approved";
                 _MTRModel.Command = "Approve";
                _MTRModel.Message = _MTRModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                _MTRModel.TransType = "Update";
                _MTRModel.AppStatus = "D";
                _MTRModel.BtnName = "BtnEdit";
                _MTRModel.WF_status1 = WF_Status1;
                TempData["WF_Status1"] = WF_Status1;
                TempData["ModelData"] = _MTRModel;
                URLModelDetails _approve_TMRModel = new URLModelDetails();
                _approve_TMRModel.TMR_Number = _MTRModel.TMR_Number;
                _approve_TMRModel.WF_status1 = _MTRModel.WF_status1;
                _approve_TMRModel.TMR_Date = _MTRModel.TMR_Date;
                _approve_TMRModel.TransType = "Update";
                _approve_TMRModel.BtnName = "BtnToDetailPage";
                TempData["ListFilterData"] = ListFilterData1;
                return RedirectToAction("MaterialTransferReceiptDetail", "MaterialTransferReceipt", _approve_TMRModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }

        private ActionResult MTRDelete(MaterialTransferReceiptModel _MTRModel, string command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string br_id = Session["BranchId"].ToString();
                string MaterialReceiptNo = _MTRModel.MaterialReceiptNo;

                DataSet Message = _MTR_ISERVICES.MTRDelete(_MTRModel, CompID, br_id, MaterialReceiptNo);
                if (!string.IsNullOrEmpty(MaterialReceiptNo))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string DeliveryNoteNo1 = MaterialReceiptNo.Replace("/", "");
                    other.DeleteTempFile(CompID + br_id, PageName, DeliveryNoteNo1, Server);
                }
                _MTRModel.BtnName = "BtnDelete";
                _MTRModel.Message = "Deleted";
                _MTRModel.AppStatus = "DL";
                _MTRModel.TransType = "Refresh";
                _MTRModel.Command = "Refresh";
             
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["TRFNo"] = "";
                //_MTRModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";

                return RedirectToAction("MaterialTransferReceiptDetail", "MaterialTransferReceipt");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

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

        //    DataTable dt = _MTR_ISERVICES.GetWhList(CompID, BranchId);
        //    return dt;
        //}
        //[NonAction]
        //private DataTable GetFromBranchList()
        //{
        //    string CompID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
            
        //    DataTable dt = _MTR_ISERVICES.GetToBranchList(CompID);
        //    return dt;
        //}
        [NonAction]
        private DataSet GetAllDDLListAndListPageData(string flag, string PageName, string UserID, string WF_status, string DocumentMenuId)
        {
            string CompID = string.Empty;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            string BranchId = Session["BranchId"].ToString();
            DataSet ds = _MTR_ISERVICES.GetAllDDLListAndListPageData(CompID, BranchId, flag, PageName, UserID, WF_status, DocumentMenuId);
            return ds;
        }

        [HttpPost]
        public ActionResult GetToWHList(string Tobranch)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                DataTable dt = _MTR_ISERVICES.GetToWhList(CompID, Tobranch);

                //DataSet result = _ItemDetail_ISERVICES.GetAttributeValueDAL(Comp_ID, AttributeID);
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
        [HttpPost]
        public JsonResult GetToWHList1(string SrcBrId/*, string FromWhID*/)
        {
            try
            {
                JsonResult DataRows = null;
                string CompID = string.Empty;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                DataTable dt = _MTR_ISERVICES.GetToWhList(CompID, SrcBrId/*, FromWhID*/);

                //DataSet result = _ItemDetail_ISERVICES.GetAttributeValueDAL(Comp_ID, AttributeID);
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
        //private DataTable GetToWHList1(string Tobranch)
        //{
        //    List<ToWharehouse> _ToWharehouseList = new List<ToWharehouse>(); ;
        //    _MTRModel = new MaterialTransferReceiptModel();
        //    string CompID = string.Empty;
        //    if (Session["CompId"] != null)
        //    {
        //        CompID = Session["CompId"].ToString();
        //    }
        //    //string BranchId = Session["BranchId"].ToString();

        //    DataTable dt = _MTR_ISERVICES.GetToWhList(CompID, Tobranch);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        ToWharehouse _ToWharehouse = new ToWharehouse();
        //        _ToWharehouse.wh_id = Convert.ToInt32(dr["wh_id"]);
        //        _ToWharehouse.wh_val = dr["wh_name"].ToString();
        //        _ToWharehouseList.Add(_ToWharehouse);

        //    }

        //    _ToWharehouseList.Insert(0, new ToWharehouse() { wh_id = 0, wh_val = "---Select---" });
        //    _MTRModel.ToWharehouseList = _ToWharehouseList;

        //    return dt;
        //}
        public ActionResult GetMaterialTranserReqList(MaterialTransferReceiptModel _MTRModel)
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
                MTRNo = _MTRModel.FilterMTRNo;
                MTRNo = _MTRModel.TMR_Number;
                SourceWH = _MTRModel.FilterSourceWH;
                ToBR = Session["BranchId"].ToString();
                TransferType = _MTRModel.FilterTransferType;               
                ToWH = _MTRModel.FilterToWH;
                if (_MTRModel.FilterTransferType == "W")
                {
                    SourceBR = Session["BranchId"].ToString();
                }
                else
                {
                    SourceBR = _MTRModel.FilterFromBR;
                }
                string BrchID = Session["BranchId"].ToString();
                MTRListDs = _MTR_ISERVICES.getMTRNOList(CompID, BrchID, MTRNo, SourceBR, SourceWH, TransferType, ToWH, ToBR);
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

                _MTRModel.Req_NO_List = _Req_NOList;
                return Json(_Req_NOList.Select(c => new { Name = c.RequirementNo, ID = c.RequirementDate }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
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
        public ActionResult GetMaterialIssueList(MaterialTransferReceiptModel _MTRModel,string MTR_No, string TMR_Date)
        {
            try
            {
               
                DataSet MIListDs = new DataSet();
                List<MI_NO> _MI_NOList = new List<MI_NO>();
                string  SourceWH,SourceBR = string.Empty;

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                //MTRNo = _MTRModel.FilterMTRNo;               
                SourceWH = _MTRModel.FilterSourceWH;
                string BrchID = Session["BranchId"].ToString();
                if (_MTRModel.FilterTransferType == "W")
                {
                    SourceBR = Session["BranchId"].ToString();
                }
                else
                {
                    SourceBR = _MTRModel.FilterFromBR;
                }
                MIListDs = _MTR_ISERVICES.getMINOList(CompID, BrchID, SourceBR, SourceWH, MTR_No, TMR_Date);
                if (MIListDs.Tables[0].Rows.Count > 0)
                {
                    DataRow Drow = MIListDs.Tables[0].NewRow();
                    Drow[0] = "---Select---";
                    Drow[1] = "0";

                    MIListDs.Tables[0].Rows.InsertAt(Drow, 0);

                    foreach (DataRow dr in MIListDs.Tables[0].Rows)
                    {
                        MI_NO _MI_NO = new MI_NO(); ;
                        _MI_NO.MIDate = dr["issue_dt"].ToString();
                        _MI_NO.MINo = dr["issue_no"].ToString();
                        _MI_NOList.Add(_MI_NO);
                    }

                }

                _MTRModel.MI_NO_List = _MI_NOList;
                return Json(_MI_NOList.Select(c => new { Name = c.MINo, ID = c.MIDate }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult GetMTRDetailByNumber(string TRFType, string TRFNo, string TRFDate, string MINo, string MIDate,string frombranch)
        {
           
            try
            {
                JsonResult DataRows = null;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = _MTR_ISERVICES.GetMaterialTransferItemDetail(CompID, BrchID, TRFDate, TRFNo, TRFType,MINo,MIDate, frombranch);
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

        public ActionResult getItemStockBatchWiseAfterStockUpadte(string IssueType, string IssueNo, string IssueDate, string ItemID,string frombranch)
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
                ds = _MTR_ISERVICES.getItemStockBatchWiseAfterStockUpdate(CompID, br_id, IssueType, IssueNo, IssueDate, ItemID, frombranch);
                if (ds.Tables[0].Rows.Count > 0)
                    ViewBag.ItemStockBatchWise = ds.Tables[0];
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/MaterialTranferReceiptPartialBatchDetail.cshtml");
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        

        private List<TMRList> GetTMRDetailList(TMR_ListModel _TMRListModel)
        {
            _TMRList = new List<TMRList>();

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
                UserID = Session["userid"].ToString();
            }
            string wfstatus = "";
            //if (Session["WF_status"] != null)
            //{
            //    wfstatus = Session["WF_status"].ToString();
            //}
            if(_TMRListModel.WF_status != null)
            {
                wfstatus = _TMRListModel.WF_status;
            }
            else
            {
                wfstatus = "";
            }
            DataSet dt = new DataSet();
            dt = _TMRList_ISERVICES.GetTMRDetailList(CompID, BrchID, _TMRListModel.from_br, _TMRListModel.from_wh, _TMRListModel.TMR_FromDate, _TMRListModel.TMR_ToDate, _TMRListModel.Status, UserID, wfstatus, DocumentMenuId);
            if (dt.Tables[1].Rows.Count > 0)
            {
                FromDate = dt.Tables[1].Rows[0]["finstrdate"].ToString();
            }
            if (dt.Tables[0].Rows.Count > 0)
            {
                
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    TMRList _TempTMRList = new TMRList();
                    _TempTMRList.MRNo = dr["mr_no"].ToString();
                    _TempTMRList.MRDate = dr["mr_dt"].ToString();
                    _TempTMRList.MR_Dt = dr["mr_date"].ToString().Trim();
                    _TempTMRList.TrfType = dr["TRFType"].ToString();
                    _TempTMRList.Trf_Type = dr["trf_type"].ToString();
                    _TempTMRList.SourceBranch = dr["frombr"].ToString();                   
                    _TempTMRList.SourceWH = dr["fromwh"].ToString();
                    _TempTMRList.Stauts = dr["Status"].ToString();
                    _TempTMRList.CreateDate = dr["CreateDate"].ToString();
                    _TempTMRList.ApproveDate = dr["ApproveDate"].ToString();
                    _TempTMRList.ModifyDate = dr["ModifyDate"].ToString();
                    _TempTMRList.create_by = dr["create_by"].ToString();
                    _TempTMRList.mod_by = dr["mod_by"].ToString();
                    _TempTMRList.app_by = dr["app_by"].ToString();

                    _TMRList.Add(_TempTMRList);
                }
            }
            return _TMRList;
        }
        [HttpPost]
        public ActionResult SearchTMRDetail(string from_br, string from_wh, string fromdate, string Todate,string Status)
        {
            //Session.Remove("WF_status");
            _TMRList = new List<TMRList>();
            TMR_ListModel _TMRListModel = new TMR_ListModel();
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
                UserID = Session["userid"].ToString();
            }
            //Session["WF_status"] = null;
            _TMRListModel.WF_status = null;
            DataSet dt = new DataSet();
            dt = _TMRList_ISERVICES.GetTMRDetailList(CompID, BrchID, from_br, from_wh, fromdate, Todate, Status, UserID,"", DocumentMenuId);
            //Session["MTRSearch"] = "MTR_Search";
            _TMRListModel.MTRSearch = "MTR_Search";
            if (dt.Tables[0].Rows.Count > 0)
            {
                
               
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    TMRList _TempTMRList = new TMRList();
                    _TempTMRList.MRNo = dr["mr_no"].ToString();
                    _TempTMRList.MRDate = dr["mr_dt"].ToString();
                    _TempTMRList.MR_Dt = dr["mr_date"].ToString().Trim();
                    _TempTMRList.TrfType = dr["TRFType"].ToString();
                    _TempTMRList.Trf_Type = dr["trf_type"].ToString();
                    _TempTMRList.SourceBranch = dr["frombr"].ToString();
                    _TempTMRList.SourceWH = dr["fromwh"].ToString();
                    _TempTMRList.Stauts = dr["Status"].ToString();
                    _TempTMRList.CreateDate = dr["CreateDate"].ToString();
                    _TempTMRList.ApproveDate = dr["ApproveDate"].ToString();
                    _TempTMRList.ModifyDate = dr["ModifyDate"].ToString();
                    _TempTMRList.create_by = dr["create_by"].ToString();
                    _TempTMRList.mod_by = dr["mod_by"].ToString();
                    _TempTMRList.app_by = dr["app_by"].ToString();
                    _TMRList.Add(_TempTMRList);
                }
            }
            _TMRListModel.TMRList = _TMRList;
            return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialTMRList.cshtml", _TMRListModel);
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

        public ActionResult ToRefreshByJS(string ListFilterData1,string transtype,string Mailerror)
        {
            MaterialTransferReceiptModel _MTRCModel = new MaterialTransferReceiptModel();
            //Session["Message"] = "";
          
            var a = transtype.Split(',');
            _MTRCModel.docid = a[0].Trim();
            _MTRCModel.TMR_Number = a[1].Trim();
            _MTRCModel.TMR_Date =a[2].Trim();
            _MTRCModel.WF_status1 = a[3].Trim();
            _MTRCModel.TransType = "Update";         
            _MTRCModel.BtnName = "BtnToDetailPage";
            _MTRCModel.Message = Mailerror;
            _MTRCModel.Message = null;
            TempData["ModelData"] = _MTRCModel;
            URLModelDetails _UrlModel = new URLModelDetails();
            _UrlModel.TMR_Number = _MTRCModel.TMR_Number;
            //_UrlModel.docid =  _MTRCModel.docid;
            _UrlModel.WF_status1 = _MTRCModel.WF_status1;
            _UrlModel.TMR_Date = _MTRCModel.TMR_Date;
            _UrlModel.TransType = "Update";
            _UrlModel.BtnName = "BtnToDetailPage";
            //_UrlModel.Message = null;
            TempData["ListFilterData"] = ListFilterData1;
            return RedirectToAction("MaterialTransferReceiptDetail", _UrlModel);
        }

        public ActionResult GetMTRDetailDashbrd(string docid, string status)
        {
            TMR_ListModel TMRDashBord = new TMR_ListModel();
            //Session["WF_status"] = status;
            TMRDashBord.WF_status = status;        
            return RedirectToAction("MaterialTransferReceipt", TMRDashBord);
        }

        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, string Flag, string Status, string Doc_no, string Doc_dt,string frombranch)
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
                if (Status == "")
                {
                    dt = _MTR_ISERVICES.MTRcpt_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, "IssueDtl", frombranch).Tables[0];
                    //dt.Columns.Add("item_id", typeof(string));
                    //dt.Columns.Add("sub_item_id", typeof(string));
                    //dt.Columns.Add("Qty", typeof(string));
                    //dt.Columns.Add("req_qty", typeof(string));
                    //dt.Columns.Add("pend_qty", typeof(string));
                    //JArray arr = JArray.Parse(SubItemListwithPageData);
                    //DataTable NewDt = new DataTable();
                    ////for (var i = 0; i < dt.Rows.Count; i++)
                    ////{
                    //    foreach (JObject item in arr.Children())//
                    //    {
                    //    DataRow dtrow = dt.NewRow();
                    //    //if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                    //    //{
                    //            dtrow["item_id"] = item.GetValue("item_id");
                    //            dtrow["sub_item_id"] = item.GetValue("sub_item_id");
                    //            dtrow["Qty"] = item.GetValue("qty");
                    //            dtrow["req_qty"] = item.GetValue("req_qty");
                    //            dtrow["pend_qty"] = item.GetValue("pend_qty");
                    //    //}
                    //    dt.Rows.Add(dtrow);
                    //    }
                    ////}
                }
                else
                {
                    dt = _MTR_ISERVICES.MTRcpt_GetSubItemDetails(CompID, BrchID, Item_id, Doc_no, Doc_dt, Flag, frombranch).Tables[0];
                }             
                SubItemPopupDt subitmModel = new SubItemPopupDt
                {
                    Flag = Flag,
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
        /*--------------------------Add By Hina on 06-08-2024 For PDF Print Start--------------------------*/

        [HttpPost]
        public FileResult GenratePdfFile(MaterialTransferReceiptModel  _model)
        {
            return File(GetPdfData(_model.MaterialReceiptNo, _model.MaterialReceiptDate), "application/pdf", "MaterialTransferReceipt.pdf");
        }
        public byte[] GetPdfData(string MTRNo, DateTime MTRDate)
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
                    BrchID = Session["BranchId"].ToString();
                }

                DataSet Details = _MTR_ISERVICES.GetMTRDeatilsForPrint(CompID, BrchID, MTRNo, Convert.ToDateTime(MTRDate).ToString("yyyy-MM-dd"));
                ViewBag.Details = Details;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Details.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Title = "Material Transfer Receipt";
                ViewBag.DocStatus = Details.Tables[0].Rows[0]["mr_status"].ToString().Trim();
                 string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/MaterialTransferReceipt/MaterialTransferReceiptPrint.cshtml"));
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
        //Added by Nidhi on 23-06-2025 
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {

            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {
                DataTable dt = new DataTable();
                var commonCont = new CommonController(_Common_IServices);
                string mailattch = commonCont.CheckMailAttch(CompID, BrchID, docid, docstatus);
                if (!string.IsNullOrEmpty(mailattch))
                {
                    if (mailattch.Trim() == "Yes")
                    {
                        var data = GetPdfData(Doc_no, DateTime.Parse(Doc_dt));
                        return commonCont.SaveAlertDocument(data, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return "ErrorPage";
            }
            return null;
        }
    }

}