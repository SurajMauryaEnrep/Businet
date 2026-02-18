using System;
using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Inventory_Management.MaterialReceipt.OpeningMaterialReceipt;
using EnRepMobileWeb.MODELS.ApplicationLayer.InventoryManagement.MaterialReceipt.OpeningMaterialReceipt;
using System.Data;
using EnRepMobileWeb.MODELS.Common;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.IO;
using System.Web;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Inventory_Management.MaterialReceipt.OpeningMaterialReceipt
{
    public class OpeningMaterialReceiptController : Controller
    {
        string DocumentMenuId = "105102115120";
        string CompID, BrchID, FromDate, language, title, userid = String.Empty;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        OpeningMaterialReceipt_ISERVICE _OPR_ISERVICE;
        public OpeningMaterialReceiptController(Common_IServices _Common_IServices, OpeningMaterialReceipt_ISERVICE _OPR_ISERVICE)
        {
            this._Common_IServices = _Common_IServices;
            this._OPR_ISERVICE = _OPR_ISERVICE;
        }
        public ActionResult OpeningMaterialReceipt(OpeningMaterial_ListModel _OpeningMaterial_ListModel)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            CommonPageDetails();
            string wfstatus = string.Empty;
            GetOpeningDate(_OpeningMaterial_ListModel);

            _OpeningMaterial_ListModel.FromDate = FromDate;
            GetStatusList(_OpeningMaterial_ListModel);
            if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
            {
                _OpeningMaterial_ListModel.WF_status = TempData["WF_status"].ToString();
                if (_OpeningMaterial_ListModel.WF_status != null)
                {
                    wfstatus = _OpeningMaterial_ListModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
            }
            else
            {
                if (_OpeningMaterial_ListModel.WF_status != null)
                {
                    wfstatus = _OpeningMaterial_ListModel.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
            }
            if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
            {
                var PRData = TempData["ListFilterData"].ToString();
                var a = PRData.Split(',');
                _OpeningMaterial_ListModel.FromDate = a[0].Trim();
                wfstatus = "Search";
            }
                _OpeningMaterial_ListModel.OPR_List = GetOP_DetailList(_OpeningMaterial_ListModel, wfstatus);

            ViewBag.DocumentMenuId = DocumentMenuId;
            _OpeningMaterial_ListModel.Title = title;
            _OpeningMaterial_ListModel.OMRSearch = "0";

            return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/OpeningMaterialReceipt/OpeningMaterialReceiptList.cshtml", _OpeningMaterial_ListModel);
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
        public void GetOpeningDate(OpeningMaterial_ListModel _OpeningMaterial_ListModel)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    string comp_id = Session["CompId"].ToString();
                    string br_id = Session["BranchId"].ToString();

                    DataSet ds = _OPR_ISERVICE.GetOpeningDate(Convert.ToInt32(comp_id), Convert.ToInt32(br_id));
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        FromDate = ds.Tables[1].Rows[0]["finstrdate"].ToString();
                        //Session["OpFindate"] = FromDate;
                        ViewBag.OpFindate = FromDate;
                    }
                    List<OPYear> fyList = new List<OPYear>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        OPYear fyObj1 = new OPYear();
                        foreach (DataRow data in ds.Tables[0].Rows)
                        {
                            OPYear fyObj = new OPYear();
                            fyObj.id = data["id"].ToString();
                            fyObj.name = data["name"].ToString();
                            fyList.Add(fyObj);
                        }
                    }
                    _OpeningMaterial_ListModel.fycount = ds.Tables[0].Rows.Count;
                    _OpeningMaterial_ListModel.op_yearList = fyList;
                    //Session["OpFindate"] = FromDate;
                    ViewBag.OpFindate = FromDate;
                    _OpeningMaterial_ListModel.FromDate = FromDate;
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
            }
        }
        public void GetStatusList(OpeningMaterial_ListModel _OpeningMaterial_ListModel)
        {
            try
            {
                List<Status> statusLists = new List<Status>();
                var other = new CommonController(_Common_IServices);
                var statusListsC = other.GetStatusList1(DocumentMenuId);
                var listOfStatus = statusListsC.ConvertAll(x => new Status { status_id = x.status_id, status_name = x.status_name });
                _OpeningMaterial_ListModel.StatusList = listOfStatus;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
            }
        }
        //private string getDocumentName()
        //{
        //    try
        //    {
        //        string CompID = string.Empty;
        //        string language = string.Empty;
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
        private List<OpeningMaterialList> GetOP_DetailList(OpeningMaterial_ListModel _OpeningMaterial_ListModel, string wfstatus)
        {
            try
            {
                List<OpeningMaterialList> _OpeningMaterialList = new List<OpeningMaterialList>();
                DataSet dt = new DataSet();
                string UserID = "";
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
                dt = _OPR_ISERVICE.GetOPRDetailList(CompID, BrchID, _OpeningMaterial_ListModel.FromDate, wfstatus, UserID, DocumentMenuId);
                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        OpeningMaterialList _OPList = new OpeningMaterialList();
                        _OPList.OPRDate = dr["OPDate"].ToString();
                        _OPList.OPR_DT = dr["OP_DT"].ToString();
                        _OPList.wh_Name = dr["wh_name"].ToString();
                        _OPList.wh_id = dr["wh_id"].ToString();
                        _OPList.id = dr["id"].ToString();
                        _OPList.Stauts = dr["Status"].ToString();
                        _OPList.op_val = dr["op_val"].ToString();
                        _OPList.CreateDate = dr["CreateDate"].ToString();
                        _OPList.ApproveDate = dr["ApproveDate"].ToString();
                        _OPList.create_by = dr["create_by"].ToString();
                        _OPList.app_by = dr["app_by"].ToString();
                        _OpeningMaterialList.Add(_OPList);
                    }
                }
                return _OpeningMaterialList;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult AddNewOpeningMaterialReceipt()
        {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                return RedirectToAction("OpeningMaterialReceipt");
            }
            /*End to chk Financial year exist or not*/
            OpeningMaterialReceiptModel openingMaterialReceiptModel = new OpeningMaterialReceiptModel();
            openingMaterialReceiptModel.Message = "New";
            openingMaterialReceiptModel.Command = "Add";
            openingMaterialReceiptModel.AppStatus = "D";
            openingMaterialReceiptModel.TransType = "Save";
            openingMaterialReceiptModel.BtnName = "BtnAddNew";
            openingMaterialReceiptModel.OpFinSTdate = ViewBag.OpFindate;
            TempData["ModelData"] = openingMaterialReceiptModel;

            return RedirectToAction("OpeningMaterialReceiptDetail", "OpeningMaterialReceipt");
        }
        public ActionResult OpeningMaterialReceiptDetail(URLModelDetails URLModel)
        {
            string UserID = string.Empty;
            ViewBag.DocumentMenuId = DocumentMenuId;
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["Language"] != null)
            {
                language = Session["Language"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            try
            {
                var openingMaterialReceiptModel = TempData["ModelData"] as OpeningMaterialReceiptModel;
                if (openingMaterialReceiptModel != null)
                {
                    DataSet OPds = _OPR_ISERVICE.GetOpeningDate(Convert.ToInt32(CompID), Convert.ToInt32(BrchID));
                    OpeningMaterial_ListModel _OpeningMaterial_ListModel = new OpeningMaterial_ListModel();
                    DataTable dt = new DataTable();
                    List<Warehouse> wh_Lists = new List<Warehouse>();
                    dt = GetWarehouseList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Warehouse WarehouseList = new Warehouse();
                        WarehouseList.wh_id = dr["wh_id"].ToString();
                        WarehouseList.wh_name = dr["wh_name"].ToString();
                        wh_Lists.Add(WarehouseList);
                    }
                    wh_Lists.Insert(0, new Warehouse() { wh_id = "0", wh_name = "---Select---" });
                    openingMaterialReceiptModel.WarehouseList = wh_Lists;
                    openingMaterialReceiptModel.OpFinSTdate = ViewBag.OpFindate;
                    CommonPageDetails();
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        openingMaterialReceiptModel.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                   
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        openingMaterialReceiptModel.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (openingMaterialReceiptModel.TransType == "Update" || openingMaterialReceiptModel.TransType == "Edit")
                    {
                        string Comp_ID = string.Empty;
                        string BranchID = string.Empty;
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = Session["CompId"].ToString();
                        }
                        if (Session["BranchId"] != null)
                        {
                            BranchID = Session["BranchId"].ToString();
                        }
                        if (Session["UserId"] != null)
                        {
                            UserID = Session["UserId"].ToString();
                        }
                        string OPR_Date = openingMaterialReceiptModel.OPR_Date;
                        string wh_id = openingMaterialReceiptModel.wh_id;
                        string whid = openingMaterialReceiptModel.wh_id;
                        DataSet ds = _OPR_ISERVICE.Edit_OpeningDetail(Comp_ID, BranchID, OPR_Date, wh_id, UserID, DocumentMenuId, openingMaterialReceiptModel.opstk_rno);
                        openingMaterialReceiptModel.OpeningValue = ds.Tables[0].Rows[0]["opening_val"].ToString();

                        openingMaterialReceiptModel.wh_Name = ds.Tables[0].Rows[0]["wh_name"].ToString();
                        openingMaterialReceiptModel.Opening_dt = ds.Tables[0].Rows[0]["opDate"].ToString();
                        openingMaterialReceiptModel.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        openingMaterialReceiptModel.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        openingMaterialReceiptModel.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        openingMaterialReceiptModel.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        openingMaterialReceiptModel.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        openingMaterialReceiptModel.status_name = ds.Tables[0].Rows[0]["status_name"].ToString();
                        openingMaterialReceiptModel.op_status = ds.Tables[0].Rows[0]["op_status"].ToString();
                        openingMaterialReceiptModel.opstk_rno = ds.Tables[0].Rows[0]["row_id"].ToString();
                        openingMaterialReceiptModel.BatchDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        openingMaterialReceiptModel.SerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        int fycount;
                        fycount = Convert.ToInt32(ds.Tables[7].Rows[0]["fy_count"].ToString());
                        openingMaterialReceiptModel.fy_count = fycount;

                        Warehouse WarehouseList = new Warehouse();
                        WarehouseList.wh_id = openingMaterialReceiptModel.wh_id;

                        WarehouseList.wh_name = openingMaterialReceiptModel.wh_Name;
                        wh_Lists.Add(WarehouseList);
                        openingMaterialReceiptModel.WarehouseList = wh_Lists;

                        openingMaterialReceiptModel.wh_id = ds.Tables[0].Rows[0]["wh_id"].ToString().Trim();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        openingMaterialReceiptModel.doc_status = doc_status;
                        openingMaterialReceiptModel.DocumentStatus = doc_status;

                        openingMaterialReceiptModel.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        openingMaterialReceiptModel.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }

                        if (ViewBag.AppLevel != null && openingMaterialReceiptModel.Command != "Edit")
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
                                    openingMaterialReceiptModel.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                        }
                                        openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                    }
                                    openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    openingMaterialReceiptModel.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        if (fycount > 1)
                        {
                            openingMaterialReceiptModel.BtnName = "BtnDisable";
                        }
                        openingMaterialReceiptModel.Title = title;
                        GetOpeningDate(_OpeningMaterial_ListModel);
                        openingMaterialReceiptModel.Opening_dt = _OpeningMaterial_ListModel.FromDate;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.SubItemDetails = ds.Tables[8];
                        ViewBag.fycount = OPds.Tables[0].Rows.Count;
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/OpeningMaterialReceipt/OpeningMaterialReceiptDetail.cshtml", openingMaterialReceiptModel);
                    }
                    else
                    {
                        //ViewBag.MenuPageName = getDocumentName();
                        openingMaterialReceiptModel.Title = title;
                        GetOpeningDate(_OpeningMaterial_ListModel);
                        //Session["DocumentStatus"] = "D";
                        openingMaterialReceiptModel.DocumentStatus = "D";
                        openingMaterialReceiptModel.Opening_dt = _OpeningMaterial_ListModel.FromDate;
                        //ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/OpeningMaterialReceipt/OpeningMaterialReceiptDetail.cshtml", openingMaterialReceiptModel);

                    }
                }
                else
                {
                    OpeningMaterialReceiptModel openingMaterialReceiptModel1 = new OpeningMaterialReceiptModel();
                    OpeningMaterial_ListModel _OpeningMaterial_ListModel = new OpeningMaterial_ListModel();
                    if (URLModel.OPR_Date != null && URLModel.wh_id != null && URLModel.id != null)
                    {
                        openingMaterialReceiptModel1.OPR_Date = URLModel.OPR_Date;
                        openingMaterialReceiptModel1.wh_id = URLModel.wh_id;
                        openingMaterialReceiptModel1.opstk_rno = URLModel.id;
                    }

                    if (URLModel.TransType != null)
                    {
                        openingMaterialReceiptModel1.TransType = URLModel.TransType;
                    }
                    else
                    {
                        openingMaterialReceiptModel1.TransType = "Save";
                    }
                    if (URLModel.BtnName != null)
                    {
                        openingMaterialReceiptModel1.BtnName = URLModel.BtnName;
                    }
                    else
                    {
                        openingMaterialReceiptModel1.BtnName = "Refresh";
                    }
                    if (URLModel.Command != null)
                    {
                        openingMaterialReceiptModel1.Command = URLModel.Command;
                    }
                    else
                    {
                        openingMaterialReceiptModel1.Command = "Refresh";
                    }
                    DataTable dt = new DataTable();
                    List<Warehouse> wh_Lists = new List<Warehouse>();
                    dt = GetWarehouseList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Warehouse WarehouseList = new Warehouse();
                        WarehouseList.wh_id = dr["wh_id"].ToString();
                        WarehouseList.wh_name = dr["wh_name"].ToString();
                        wh_Lists.Add(WarehouseList);
                    }
                    wh_Lists.Insert(0, new Warehouse() { wh_id = "0", wh_name = "---Select---" });
                    openingMaterialReceiptModel1.WarehouseList = wh_Lists;
                    openingMaterialReceiptModel1.OpFinSTdate = ViewBag.OpFindate;
                    CommonPageDetails();
                    if (TempData["ListFilterData"] != null && TempData["ListFilterData"].ToString() != "")
                    {
                        openingMaterialReceiptModel1.ListFilterData1 = TempData["ListFilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        openingMaterialReceiptModel1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    if (openingMaterialReceiptModel1.TransType == "Update" || openingMaterialReceiptModel1.TransType == "Edit")
                    {
                        string Comp_ID = string.Empty;
                        string BranchID = string.Empty;
                        if (Session["CompId"] != null)
                        {
                            Comp_ID = Session["CompId"].ToString();
                        }
                        if (Session["BranchId"] != null)
                        {
                            BranchID = Session["BranchId"].ToString();
                        }
                        if (Session["UserId"] != null)
                        {
                            UserID = Session["UserId"].ToString();
                        }
                        string OPR_Date = openingMaterialReceiptModel1.OPR_Date;
                        string wh_id = openingMaterialReceiptModel1.wh_id;
                        DataSet ds = _OPR_ISERVICE.Edit_OpeningDetail(Comp_ID, BranchID, OPR_Date, wh_id, UserID, DocumentMenuId, openingMaterialReceiptModel1.opstk_rno);
                        openingMaterialReceiptModel1.OpeningValue = ds.Tables[0].Rows[0]["opening_val"].ToString();

                        openingMaterialReceiptModel1.wh_Name = ds.Tables[0].Rows[0]["wh_name"].ToString();
                        openingMaterialReceiptModel1.Opening_dt = ds.Tables[0].Rows[0]["opDate"].ToString();
                        openingMaterialReceiptModel1.Create_id = ds.Tables[0].Rows[0]["create_id"].ToString();
                        openingMaterialReceiptModel1.CreatedBy = ds.Tables[0].Rows[0]["CreateName"].ToString();
                        openingMaterialReceiptModel1.CreatedOn = ds.Tables[0].Rows[0]["CreateDate"].ToString();
                        openingMaterialReceiptModel1.ApprovedBy = ds.Tables[0].Rows[0]["ApproveName"].ToString();
                        openingMaterialReceiptModel1.ApprovedOn = ds.Tables[0].Rows[0]["ApproveDate"].ToString();
                        openingMaterialReceiptModel1.status_name = ds.Tables[0].Rows[0]["status_name"].ToString();
                        openingMaterialReceiptModel1.op_status = ds.Tables[0].Rows[0]["op_status"].ToString();
                        openingMaterialReceiptModel1.opstk_rno = ds.Tables[0].Rows[0]["row_id"].ToString();
                        openingMaterialReceiptModel1.BatchDetail = DataTableToJSONWithStringBuilder(ds.Tables[2]);
                        openingMaterialReceiptModel1.SerialDetail = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        int fycount;
                        fycount = Convert.ToInt32(ds.Tables[7].Rows[0]["fy_count"].ToString());
                        openingMaterialReceiptModel1.fy_count = fycount;

                        Warehouse WarehouseList = new Warehouse();
                        WarehouseList.wh_id = openingMaterialReceiptModel1.wh_id;

                        WarehouseList.wh_name = openingMaterialReceiptModel1.wh_Name;
                        wh_Lists.Add(WarehouseList);
                        openingMaterialReceiptModel1.WarehouseList = wh_Lists;

                        openingMaterialReceiptModel1.wh_id = ds.Tables[0].Rows[0]["wh_id"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        openingMaterialReceiptModel1.doc_status = doc_status;
                        openingMaterialReceiptModel1.DocumentStatus = doc_status;

                        openingMaterialReceiptModel1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[6]);
                        openingMaterialReceiptModel1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[5]);
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[6];
                        }

                        if (ViewBag.AppLevel != null && openingMaterialReceiptModel1.Command != "Edit")
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
                                    openingMaterialReceiptModel1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                        }
                                        openingMaterialReceiptModel1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        openingMaterialReceiptModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    openingMaterialReceiptModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        openingMaterialReceiptModel1.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    openingMaterialReceiptModel1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                    }
                                    openingMaterialReceiptModel1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    openingMaterialReceiptModel1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    openingMaterialReceiptModel1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        if (fycount > 1)
                        {
                            openingMaterialReceiptModel1.BtnName = "BtnDisable";
                        }
                        openingMaterialReceiptModel1.Title = title;
                        GetOpeningDate(_OpeningMaterial_ListModel);
                        openingMaterialReceiptModel1.Opening_dt = _OpeningMaterial_ListModel.FromDate;
                        ViewBag.ItemDetails = ds.Tables[1];
                        ViewBag.SubItemDetails = ds.Tables[8];
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/OpeningMaterialReceipt/OpeningMaterialReceiptDetail.cshtml", openingMaterialReceiptModel1);
                    }
                    else
                    {
                        openingMaterialReceiptModel1.Title = title;
                        GetOpeningDate(_OpeningMaterial_ListModel);
                        openingMaterialReceiptModel1.DocumentStatus = "D";
                        openingMaterialReceiptModel1.Opening_dt = _OpeningMaterial_ListModel.FromDate;
                        return View("~/Areas/ApplicationLayer/Views/InventoryManagement/MaterialReceipt/OpeningMaterialReceipt/OpeningMaterialReceiptDetail.cshtml", openingMaterialReceiptModel1);
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
        public ActionResult ToRefreshByJS(string TrancType)
        {
            URLModelDetails URLModel = new URLModelDetails();
            OpeningMaterialReceiptModel openingMaterialReceiptModel = new OpeningMaterialReceiptModel();
            var a = TrancType.Split(',');
            openingMaterialReceiptModel.OPR_Date = a[0].Trim();
            openingMaterialReceiptModel.wh_id = a[1].Trim();
            openingMaterialReceiptModel.TransType = a[2].Trim();
            openingMaterialReceiptModel.opstk_rno = a[4].Trim();
            var WF_status1 = a[3].Trim();
            openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
            URLModel.OPR_Date = a[0].Trim();
            URLModel.wh_id = a[1].Trim();
            URLModel.TransType = a[2].Trim();
            URLModel.id = a[4].Trim();
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = openingMaterialReceiptModel;
            TempData["WF_status1"] = WF_status1;
            return RedirectToAction("OpeningMaterialReceiptDetail", URLModel);
        }
        public ActionResult EditOMR(string id, string wh_id, string Opening_dt, string WF_status, string ListFilterData)
        {/*start Add by Hina on 13-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, BrchID) == "Not Exist")
            {
                TempData["Message1"] = "Financial Year not Exist";
            }
            /*End to chk Financial year exist or not*/
            URLModelDetails URLModel = new URLModelDetails();
            OpeningMaterialReceiptModel openingMaterialReceiptModel = new OpeningMaterialReceiptModel();
            openingMaterialReceiptModel.Message = "New";
            openingMaterialReceiptModel.Command = "Add";
            openingMaterialReceiptModel.wh_id = wh_id;
            openingMaterialReceiptModel.opstk_rno = id;
            openingMaterialReceiptModel.OPR_Date = Opening_dt;
            openingMaterialReceiptModel.TransType = "Update";
            openingMaterialReceiptModel.AppStatus = "D";
            openingMaterialReceiptModel.BtnName = "BtnToDetailPage";
            openingMaterialReceiptModel.WF_status1 = WF_status;
            TempData["ModelData"] = openingMaterialReceiptModel;
            URLModel.OPR_Date = Opening_dt;
            URLModel.wh_id = wh_id;
            URLModel.id = id;
            URLModel.TransType = "Update";
            URLModel.Command = "Add";
            URLModel.BtnName = "BtnToDetailPage";
            TempData["ListFilterData"] = ListFilterData;
            return RedirectToAction("OpeningMaterialReceiptDetail", URLModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpeningMaterialReceiptSave(OpeningMaterialReceiptModel openingMaterialReceiptModel, string command)
        {
            try
            {
                if (openingMaterialReceiptModel.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        OpeningMaterialReceiptModel openingMaterialReceiptModelAddNew = new OpeningMaterialReceiptModel();
                        openingMaterialReceiptModelAddNew.AppStatus = "D";
                        openingMaterialReceiptModelAddNew.BtnName = "BtnAddNew";
                        openingMaterialReceiptModelAddNew.TransType = "Save";
                        openingMaterialReceiptModelAddNew.Command = "New";
                        TempData["ModelData"] = openingMaterialReceiptModelAddNew;
                        TempData["ListFilterData"] = null;
                        return RedirectToAction("OpeningMaterialReceiptDetail", "OpeningMaterialReceipt");

                    case "Edit":
                        openingMaterialReceiptModel.TransType = "Update";
                        openingMaterialReceiptModel.Command = command;
                        openingMaterialReceiptModel.BtnName = "BtnEdit";
                        openingMaterialReceiptModel.Message = null;
                        openingMaterialReceiptModel.OPR_Date = openingMaterialReceiptModel.Opening_dt;
                        TempData["ModelData"] = openingMaterialReceiptModel;
                        URLModelDetails URLModel = new URLModelDetails();
                        URLModel.OPR_Date = openingMaterialReceiptModel.OPR_Date;
                        URLModel.wh_id = openingMaterialReceiptModel.wh_id;
                        URLModel.id = openingMaterialReceiptModel.opstk_rno;
                        URLModel.TransType = "Update";
                        URLModel.BtnName = "BtnEdit";
                        URLModel.Command = command;
                        TempData["ListFilterData"] = openingMaterialReceiptModel.ListFilterData1;
                        return RedirectToAction("OpeningMaterialReceiptDetail", URLModel);

                    case "Delete":
                        OpeningMaterialReceiptModel openingMaterialReceiptModelDelete = new OpeningMaterialReceiptModel();
                        OMRDelete(openingMaterialReceiptModel);
                        openingMaterialReceiptModelDelete.Message = "Deleted";
                        openingMaterialReceiptModelDelete.Command = "Refresh";
                        openingMaterialReceiptModelDelete.TransType = "Refresh";
                        openingMaterialReceiptModelDelete.AppStatus = "DL";
                        openingMaterialReceiptModelDelete.BtnName = "BtnDelete";
                        TempData["ModelData"] = openingMaterialReceiptModelDelete;
                        TempData["ListFilterData"] = openingMaterialReceiptModel.ListFilterData1;
                        return RedirectToAction("OpeningMaterialReceiptDetail");

                    case "Save":
                        if (openingMaterialReceiptModel.TransType == null)
                        {
                            openingMaterialReceiptModel.TransType = command;
                        }
                        SaveOpBlDetail(openingMaterialReceiptModel);
                        if (openingMaterialReceiptModel.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        URLModelDetails URLModelSave = new URLModelDetails();
                        URLModelSave.OPR_Date = openingMaterialReceiptModel.OPR_Date;
                        URLModelSave.wh_id = openingMaterialReceiptModel.wh_id;
                        URLModelSave.id = openingMaterialReceiptModel.opstk_rno;
                        URLModelSave.Command = openingMaterialReceiptModel.Command;
                        URLModelSave.TransType = openingMaterialReceiptModel.TransType;
                        URLModelSave.BtnName = openingMaterialReceiptModel.BtnName;
                        TempData["ModelData"] = openingMaterialReceiptModel;
                        TempData["ListFilterData"] = openingMaterialReceiptModel.ListFilterData1;
                        return RedirectToAction("OpeningMaterialReceiptDetail", URLModelSave);

                    case "Forward":
                        return new EmptyResult();

                    case "Approve":
                        URLModelDetails URLModelApprove = new URLModelDetails();
                        Approve_OpeningMaterialReceiptDetails(openingMaterialReceiptModel.opstk_rno, openingMaterialReceiptModel.Opening_dt, openingMaterialReceiptModel.wh_id, "", "", "", "");
                        openingMaterialReceiptModel.OPR_Date = openingMaterialReceiptModel.Opening_dt;
                        URLModelApprove.OPR_Date = openingMaterialReceiptModel.OPR_Date;
                        URLModelApprove.wh_id = openingMaterialReceiptModel.wh_id;
                        URLModelApprove.id = openingMaterialReceiptModel.opstk_rno;
                        URLModelApprove.Command = openingMaterialReceiptModel.Command;
                        URLModelApprove.TransType = openingMaterialReceiptModel.TransType;
                        URLModelApprove.BtnName = openingMaterialReceiptModel.BtnName;
                        openingMaterialReceiptModel.Message = "Approved";
                        TempData["ModelData"] = openingMaterialReceiptModel;
                        TempData["ListFilterData"] = openingMaterialReceiptModel.ListFilterData1;
                        return RedirectToAction("OpeningMaterialReceiptDetail", URLModelApprove);

                    case "Refresh":
                        OpeningMaterialReceiptModel openingMaterialReceiptModelRefresh = new OpeningMaterialReceiptModel();
                        openingMaterialReceiptModelRefresh.BtnName = "Refresh";
                        openingMaterialReceiptModelRefresh.Command = command;
                        openingMaterialReceiptModelRefresh.TransType = "Save";
                        TempData["ModelData"] = openingMaterialReceiptModelRefresh;
                        TempData["ListFilterData"] = openingMaterialReceiptModel.ListFilterData1;
                        return RedirectToAction("OpeningMaterialReceiptDetail");

                    case "Print":
                        return new EmptyResult();

                    case "BacktoList":
                        TempData["WF_status"] = openingMaterialReceiptModel.WF_status1;
                        TempData["ListFilterData"] = openingMaterialReceiptModel.ListFilterData1;
                        return RedirectToAction("OpeningMaterialReceipt", "OpeningMaterialReceipt");

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
        [NonAction]
        public ActionResult SaveOpBlDetail(OpeningMaterialReceiptModel openingMaterialReceiptModel)
        {
            try
            {
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
                string op_rno = string.Empty;
                if (!string.IsNullOrEmpty(openingMaterialReceiptModel.opstk_rno))
                {
                    op_rno = openingMaterialReceiptModel.opstk_rno;
                }
                else
                {
                    op_rno = "0";
                }
                DataTable DtblHDetail = new DataTable();
                DataTable DtblItemDetail = new DataTable();
                DataTable ItemBatchDetails = new DataTable();
                DataTable ItemSerialDetails = new DataTable();

                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("TransType", typeof(string));
                dtheader.Columns.Add("MenuID", typeof(string));
                dtheader.Columns.Add("wh_id", typeof(string));
                dtheader.Columns.Add("comp_id", typeof(int));
                dtheader.Columns.Add("br_id", typeof(int));
                dtheader.Columns.Add("op_dt", typeof(DateTime));

                dtheader.Columns.Add("user_id", typeof(string));
                dtheader.Columns.Add("op_status", typeof(string));
                dtheader.Columns.Add("mac_id", typeof(string));
                dtheader.Columns.Add("op_val", typeof(string));

                DataRow dtrowHeader = dtheader.NewRow();
                dtrowHeader["TransType"] = openingMaterialReceiptModel.TransType;
                dtrowHeader["MenuID"] = DocumentMenuId;
                dtrowHeader["wh_id"] = openingMaterialReceiptModel.wh_id;
                dtrowHeader["comp_id"] = Session["CompId"].ToString();
                dtrowHeader["br_id"] = Session["BranchId"].ToString();
                dtrowHeader["op_dt"] = openingMaterialReceiptModel.Opening_dt;

                dtrowHeader["user_id"] = Session["UserId"].ToString();
                dtrowHeader["op_status"] = "D";
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                dtrowHeader["mac_id"] = mac_id;
                dtrowHeader["op_val"] = openingMaterialReceiptModel.OpeningValue;

                dtheader.Rows.Add(dtrowHeader);
                DtblHDetail = dtheader;

                DataTable dtItem = new DataTable();

                dtItem.Columns.Add("item_id", typeof(string));
                dtItem.Columns.Add("uom_id", typeof(int));
                dtItem.Columns.Add("op_qty", typeof(string));
                dtItem.Columns.Add("lot_id", typeof(string));
                dtItem.Columns.Add("item_rate", typeof(string));
                dtItem.Columns.Add("op_val", typeof(string));

                JArray jObject = JArray.Parse(openingMaterialReceiptModel.OPR_ItemDetail);

                for (int i = 0; i < jObject.Count; i++)
                {
                    DataRow dtrowLines = dtItem.NewRow();

                    dtrowLines["item_id"] = jObject[i]["ItemId"].ToString();
                    dtrowLines["uom_id"] = jObject[i]["UOMId"].ToString();
                    dtrowLines["op_qty"] = jObject[i]["OPQty"].ToString();
                    dtrowLines["lot_id"] = jObject[i]["LotNumber"].ToString();
                    dtrowLines["item_rate"] = jObject[i]["ItemCost"].ToString();
                    dtrowLines["op_val"] = jObject[i]["OpValue"].ToString();
                    dtItem.Rows.Add(dtrowLines);
                }
                DtblItemDetail = dtItem;

                DataTable Batch_detail = new DataTable();
                Batch_detail.Columns.Add("item_id", typeof(string));
                Batch_detail.Columns.Add("batch_no", typeof(string));
                Batch_detail.Columns.Add("batch_qty", typeof(float));
                Batch_detail.Columns.Add("exp_dt", typeof(string));
                Batch_detail.Columns.Add("mfg_name", typeof(string));//Added Mfg Details by Suraj Maurya on 25-11-2025
                Batch_detail.Columns.Add("mfg_mrp", typeof(string));
                Batch_detail.Columns.Add("mfg_date", typeof(string));
                if (openingMaterialReceiptModel.ItemBatchWiseDetail != null)
                {
                    JArray jObjectBatch = JArray.Parse(openingMaterialReceiptModel.ItemBatchWiseDetail);
                    for (int i = 0; i < jObjectBatch.Count; i++)
                    {
                        DataRow dtrowBatchDetailsLines = Batch_detail.NewRow();
                        dtrowBatchDetailsLines["item_id"] = jObjectBatch[i]["item_id"].ToString();
                        dtrowBatchDetailsLines["batch_no"] = jObjectBatch[i]["batch_no"].ToString();
                        dtrowBatchDetailsLines["batch_qty"] = jObjectBatch[i]["batch_qty"].ToString();
                        if (jObjectBatch[i]["exp_dt"].ToString() == "" || jObjectBatch[i]["exp_dt"].ToString() == null)
                        {
                            dtrowBatchDetailsLines["exp_dt"] = "01-Jan-1900";
                        }
                        else
                        {
                            dtrowBatchDetailsLines["exp_dt"] = jObjectBatch[i]["exp_dt"].ToString();
                        }
                        dtrowBatchDetailsLines["mfg_name"] = IsBlank(jObjectBatch[i]["MfgName"].ToString(),null);
                        dtrowBatchDetailsLines["mfg_mrp"] = IsBlank(jObjectBatch[i]["MfgMrp"].ToString(),null);
                        dtrowBatchDetailsLines["mfg_date"] = IsBlank(jObjectBatch[i]["MfgDate"].ToString(),null);
                        Batch_detail.Rows.Add(dtrowBatchDetailsLines);
                    }
                }
                ItemBatchDetails = Batch_detail;


                DataTable Serial_detail = new DataTable();
                Serial_detail.Columns.Add("item_id", typeof(string));
                Serial_detail.Columns.Add("serial_no", typeof(string));
                Serial_detail.Columns.Add("mfg_name", typeof(string));//Added Mfg Details by Suraj Maurya on 25-11-2025
                Serial_detail.Columns.Add("mfg_mrp", typeof(string));
                Serial_detail.Columns.Add("mfg_date", typeof(string));

                if (openingMaterialReceiptModel.ItemSerialWiseDetail != null)
                {
                    JArray jObjectSerial = JArray.Parse(openingMaterialReceiptModel.ItemSerialWiseDetail);
                    for (int i = 0; i < jObjectSerial.Count; i++)
                    {
                        DataRow dtrowSerialDetailsLines = Serial_detail.NewRow();
                        dtrowSerialDetailsLines["item_id"] = jObjectSerial[i]["item_id"].ToString();
                        dtrowSerialDetailsLines["serial_no"] = jObjectSerial[i]["serial_no"].ToString();
                        dtrowSerialDetailsLines["mfg_name"] = IsBlank(jObjectSerial[i]["MfgName"].ToString(),null);
                        dtrowSerialDetailsLines["mfg_mrp"] = IsBlank(jObjectSerial[i]["MfgMrp"].ToString(),null);
                        dtrowSerialDetailsLines["mfg_date"] = IsBlank(jObjectSerial[i]["MfgDate"].ToString(),null);
                        Serial_detail.Rows.Add(dtrowSerialDetailsLines);
                    }
                }
                ItemSerialDetails = Serial_detail;

                /*----------------------Sub Item ----------------------*/
                DataTable dtSubItem = new DataTable();
                dtSubItem.Columns.Add("item_id", typeof(string));
                dtSubItem.Columns.Add("sub_item_id", typeof(string));
                dtSubItem.Columns.Add("qty", typeof(string));
                if (openingMaterialReceiptModel.SubItemDetailsDt != null)
                {
                    JArray jObject2 = JArray.Parse(openingMaterialReceiptModel.SubItemDetailsDt);
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

                string SaveMessage = _OPR_ISERVICE.InsertOP_Details(DtblHDetail, DtblItemDetail, ItemBatchDetails, ItemSerialDetails, dtSubItem, op_rno);
                string[] FDetail = SaveMessage.Split(',');
                string Message = FDetail[0].ToString();
                string op_dt = FDetail[1].ToString();
                string wh_no = FDetail[2].ToString();
                string r_no = FDetail[7].ToString();
                if (Message == "Data_Not_Found")
                {
                    var msg = Message.Replace("_", " ") + " " + op_dt + " in " + wh_no;
                    string path = Server.MapPath("~");
                    Errorlog.LogError_customsg(path, msg, "", "");
                    openingMaterialReceiptModel.Message = Message.Replace("_", "");
                    return RedirectToAction("OpeningMaterialReceiptDetail");
                }

                if (Message == "Update" || Message == "Save")
                {
                    openingMaterialReceiptModel.Message = "Save";
                    openingMaterialReceiptModel.Command = "Update";
                    openingMaterialReceiptModel.wh_id = wh_no;
                    openingMaterialReceiptModel.OPR_Date = op_dt;
                    openingMaterialReceiptModel.opstk_rno = r_no;
                    openingMaterialReceiptModel.TransType = "Update";
                    openingMaterialReceiptModel.AppStatus = "D";
                    openingMaterialReceiptModel.BtnName = "BtnSave";
                }
                return RedirectToAction("OpeningMaterialReceiptDetail");
            }

            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        private ActionResult OMRDelete(OpeningMaterialReceiptModel openingMaterialReceiptModel)
        {
            if (Session["compid"] != null)
            {
                CompID = Session["compid"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                BrchID = Session["BranchId"].ToString();
            }
            try
            {
                string OPR_Delete = _OPR_ISERVICE.Delete_OPR_Detail(openingMaterialReceiptModel, CompID, BrchID);

                openingMaterialReceiptModel.Message = "Deleted";
                openingMaterialReceiptModel.Command = "Refresh";
                openingMaterialReceiptModel.wh_id = "";
                openingMaterialReceiptModel.opstk_rno = "";
                openingMaterialReceiptModel.TransType = "Refresh";
                openingMaterialReceiptModel.AppStatus = "DL";
                openingMaterialReceiptModel.BtnName = "BtnDelete";

                return RedirectToAction("OpeningMaterialReceiptDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult GetOpeningMaterialRecieptList(string docid, string status)
        {
            OpeningMaterial_ListModel _OpeningMaterial_ListModel = new OpeningMaterial_ListModel();
            //Session["WF_status"] = status;
            _OpeningMaterial_ListModel.WF_status = status;
            return RedirectToAction("OpeningMaterialReceipt", _OpeningMaterial_ListModel);
        }
        public ActionResult Approve_OpeningMaterialReceiptDetails(string id, string OPR_Date, string wh_id, string A_Status, string A_Level, string A_Remarks, string WF_status1)
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
                if (Session["MenuDocumentId"] != null)
                {
                    MenuDocId = DocumentMenuId;
                }
                OpeningMaterialReceiptModel openingMaterialReceiptModel = new OpeningMaterialReceiptModel();
                URLModelDetails URLModelApprove = new URLModelDetails();
                openingMaterialReceiptModel.wh_id = wh_id;
                openingMaterialReceiptModel.Opening_dt = OPR_Date;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = _OPR_ISERVICE.Approve_OpeningMaterialReceipt(id, wh_id, OPR_Date, BranchID, Comp_ID, UserID, mac_id, A_Status, A_Level, A_Remarks, DocumentMenuId);
                string ApMessage = Message.Split(',')[0].Trim();
                string wh_id1 = Message.Split(',')[1].Trim();
                string OPR_Date1 = Message.Split(',')[2].Trim();
                string op_id = Message.Split(',')[3].Trim();
                if (ApMessage == "A")
                {
                    openingMaterialReceiptModel.Message = "Approved";
                }
                openingMaterialReceiptModel.TransType = "Update";
                openingMaterialReceiptModel.Command = "Approve";
                openingMaterialReceiptModel.wh_id = wh_id1;
                openingMaterialReceiptModel.opstk_rno = op_id;
                openingMaterialReceiptModel.AppStatus = "D";
                openingMaterialReceiptModel.BtnName = "BtnEdit";
                openingMaterialReceiptModel.OPR_Date = OPR_Date1;
                openingMaterialReceiptModel.WF_status1 = WF_status1;
                URLModelApprove.OPR_Date = OPR_Date1;
                URLModelApprove.wh_id = wh_id1;
                URLModelApprove.id = op_id;
                URLModelApprove.Command = "Approve";
                URLModelApprove.TransType = "Update";
                URLModelApprove.BtnName = "BtnEdit";
                TempData["ModelData"] = openingMaterialReceiptModel;
                return RedirectToAction("OpeningMaterialReceiptDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public DataTable GetWarehouseList()
        {
            DataTable dt = new DataTable();
            try
            {
                //JsonResult DataRows = null;
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
                DataSet result = _OPR_ISERVICE.GetWarehouseList(Comp_ID, Br_ID);
                dt = result.Tables[0];

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
            return dt;

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
        public ActionResult GetOPR_ItemList(BindItemList bindItem, string wh_id)
        {
            //JsonResult DataRows = null;
            string ItmName = string.Empty;
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
            Dictionary<string, string> ItemList = new Dictionary<string, string>();
            //string wh_id;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    Br_ID = Session["BranchId"].ToString();
                }

                ItmName = "0";
                if (string.IsNullOrEmpty(bindItem.SearchName))
                {
                    bindItem.SearchName = "";
                }
                //wh_id = _OPRModel.warehouse_id;

                DataSet ItmList = _OPR_ISERVICE.GetOpeningRcptItmList(Comp_ID, Br_ID, bindItem.SearchName, wh_id);
                DataRow Drow = ItmList.Tables[0].NewRow();
                Drow[0] = "0";
                Drow[1] = "---Select---";
                Drow[2] = "0";
                ItmList.Tables[0].Rows.InsertAt(Drow, 0);
                if (ItmList.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ItmList.Tables[0].Rows.Count; i++)
                    {
                        string itemId = ItmList.Tables[0].Rows[i]["Item_id"].ToString();
                        string itemName = ItmList.Tables[0].Rows[i]["Item_name"].ToString();
                        string Uom = ItmList.Tables[0].Rows[i]["uom_name"].ToString();
                        ItemList.Add(itemId + "_" + Uom, itemName);
                    }
                }
                return Json(ItemList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            //return DataRows;
        }
        [HttpPost]
        public JsonResult GetItemUOM(string Itm_ID)
        {
            try
            {
                JsonResult DataRows = null;
                string Comp_ID = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                DataSet result = _OPR_ISERVICE.GetItemUOMDAL(Itm_ID, Comp_ID);
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
        [HttpPost]
        public JsonResult GetOpeningQuantityDetails(string Op_dt, string Wh_id, string Item_ID, string Flag)
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
                    BrchID = Session["BranchId"].ToString();
                }
                DataSet result = _OPR_ISERVICE.GetOpeningQtyDetauls(CompID, BrchID, Op_dt, Wh_id, Item_ID, Flag);
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
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled, string Flag, string Status, string wh_id, string OP_dt)
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
                    dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    DataTable NewDt = new DataTable();
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
                    dt = _OPR_ISERVICE.OMR_GetSubItemDetails(CompID, BrchID, Item_id, wh_id, OP_dt, Flag).Tables[0];
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
        public ActionResult DownloadFile()
        {
            try
            {
                string compId = "0";
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["BranchId"] != null)
                    BrchID = Session["BranchId"].ToString();
                DataTable OMRDetail = new DataTable();
                OMRDetail.Columns.Add("Item Name*", typeof(string));
                OMRDetail.Columns.Add("Opening Quantity*", typeof(string));
                OMRDetail.Columns.Add("Item Cost*", typeof(string));

                DataSet ds = _OPR_ISERVICE.GetMasterDataForExcelFormat(compId, BrchID);
                CommonController obj = new CommonController();
                string filePath = obj.CreateExcelFile("ImportOpeningStockTemplate", Server);
                UpdateExcel(filePath, ds, OMRDetail);
                string fileName = Path.GetFileName(filePath);
                return File(filePath, "application/octet-stream", fileName);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        private void UpdateExcel(string filePath, DataSet dataSet, DataTable OMRDetail)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    // Define a list of DataTables and corresponding sheet names
                    var predefinedSheets = new (string sheetName, DataTable table)[]
                    {
                ("ItemDetail", OMRDetail)};
                    // Add predefined sheets
                    foreach (var sheet in predefinedSheets)
                    {
                        // Apply Text format to all cells in the worksheet
                        var worksheet = package.Workbook.Worksheets.Add(sheet.sheetName);
                        worksheet.Cells.LoadFromDataTable(sheet.table, true);
                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                        var headerRow = worksheet.Cells[1, 1, 1, sheet.table.Columns.Count];
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Font.Name = "Calibri";
                        headerRow.Style.Font.Size = 12;
                        headerRow.Style.Font.Italic = true;
                    }
                    foreach (DataTable table in dataSet.Tables)
                    {
                        if (table.TableName == "Table")
                        {
                            table.TableName = "BatchDetail";
                        }
                        if (table.TableName == "BatchDetail")
                        {
                            table.Columns.Add("Batch Number(max 25 characters)", typeof(string));
                            table.Columns.Add("Quantity", typeof(string));
                            table.Columns.Add("Manufacturer Name", typeof(string));
                            table.Columns.Add("MRP", typeof(string));
                            table.Columns.Add("Manufactured Date", typeof(string));
                            table.Columns.Add("Expiry Date", typeof(string));
                        }
                        if (table.TableName == "Table1")
                        {
                            table.TableName = "SerialDetail";
                        }
                        if (table.TableName == "SerialDetail")
                        {
                            table.Columns.Add("Serial Number(max 25 characters)", typeof(string));
                            table.Columns.Add("Manufacturer Name", typeof(string));
                            table.Columns.Add("MRP", typeof(string));
                            table.Columns.Add("Manufactured Date", typeof(string));
                        }
                        if (table.TableName == "Table2")
                        {
                            table.TableName = "SubItemDetail";
                        }
                        if (table.TableName == "SubItemDetail")
                        {
                            if (!table.Columns.Contains("Quantity"))
                            {
                                table.Columns.Add("Quantity", typeof(string));
                            }
                        }
                        if (table.TableName == "Table3")
                        {
                            table.TableName = "ItemName";
                        }
                        var worksheet = package.Workbook.Worksheets[table.TableName] ?? package.Workbook.Worksheets.Add(table.TableName);
                        worksheet.Cells.Clear();
                        worksheet.Cells.LoadFromDataTable(table, true);
                        if (worksheet.Dimension != null)
                        {
                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                        }
                        for (int col = 1; col <= table.Columns.Count; col++)
                        {
                            if (col == 1)
                            {
                                worksheet.Column(col).Width = 40;  // Set width of 40 for the first column
                            }
                            else
                            {
                                worksheet.Column(col).Width = 20;  // Set width of 20 for the other columns
                            }
                        }
                        var headerRow = worksheet.Cells[1, 1, 1, table.Columns.Count];
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Font.Name = "Arial";
                        headerRow.Style.Font.Size = 11;
                        headerRow.Style.Font.Italic = true;
                        headerRow.Style.WrapText = true;
                    }
                    var ItemNameSheet = package.Workbook.Worksheets["ItemName"];
                    if (ItemNameSheet != null)
                    {
                        int ItemNaemRowCount = ItemNameSheet.Dimension.End.Row;
                        var itemnameList = new List<string>();
                        for (int row = 2; row <= ItemNaemRowCount; row++) // Skip header row
                        {
                            var itemnameValue = ItemNameSheet.Cells[row, 1].Text;
                            if (!string.IsNullOrEmpty(itemnameValue))
                            {
                                itemnameList.Add(itemnameValue);
                            }
                        }
                        if (itemnameList.Count > 0)
                        {
                            var worksheet = package.Workbook.Worksheets["ItemDetail"];
                            var accgrpRange = ItemNameSheet.Cells[2, 1, itemnameList.Count + 1, 1]; // Define range starting from row 2
                            string ItemRangeName = "ItemName"; // Name the range
                            package.Workbook.Names.Add(ItemRangeName, accgrpRange);

                            // Apply data validation to the target range in the desired worksheet
                            var startRow = 2;
                            var endRow = 1048576; // Excel's max row count
                            var range = worksheet.Cells[startRow, 1, endRow, 1];
                            var validation = range.DataValidation.AddListDataValidation();
                            validation.ShowErrorMessage = true;
                            validation.ErrorTitle = "Invalid Item Name";
                            validation.Error = "Please select a valid Item Name from the list.";

                            // Use the named range in the data validation formula
                            validation.Formula.ExcelFormula = $"={ItemRangeName}";
                        }
                    }
                    ProtectSheet(package.Workbook, "ItemName");
                    package.Workbook.Worksheets.FirstOrDefault(ws => ws.View.TabSelected = false);
                    package.Save();
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
            }
        }
        private void ProtectSheet(ExcelWorkbook workbook, string sheetName)
        {
            var worksheet = workbook.Worksheets[sheetName];
            if (worksheet != null)
            {
                // Protect the worksheet with a password (optional)
                worksheet.Protection.SetPassword("enrep");
            }
        }
        public ActionResult ValidateExcelFile(string uploadStatus, string Warehouse)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            conString = "Invalid File";
                            break;
                    }
                    if (conString == "Invalid File")
                        return Json("Invalid File. Please upload a valid file", JsonRequestBehavior.AllowGet);
                    DataSet ds = new DataSet();
                    DataTable ItemDetail = new DataTable();
                    DataTable BatchDetail = new DataTable();
                    DataTable SerialDetail = new DataTable();
                    DataTable SubItemDetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();

                                //string ItemDetailQuery = "SELECT DISTINCT * From [ItemDetail$] ; ";
                                string ItemDetailQuery = "SELECT DISTINCT * FROM [ItemDetail$] WHERE [Item Name*] <> '' AND [Item Name*] IS NOT NULL;";
                                string BatchDetailQuery = "SELECT DISTINCT * From [BatchDetail$] ; ";
                                string SerialdetailQuery = "SELECT DISTINCT * From [SerialDetail$]; ";
                                string SubItemQuery = "SELECT DISTINCT * From [SubItemDetail$]; ";

                                connExcel.Open();
                                cmdExcel.CommandText = ItemDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(ItemDetail);

                                cmdExcel.CommandText = BatchDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(BatchDetail);

                                cmdExcel.CommandText = SerialdetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SerialDetail);
                                connExcel.Close();

                                cmdExcel.CommandText = SubItemQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SubItemDetail);
                                connExcel.Close();

                                int dtItemscount = 0;
                                int dtItemslength = 0;
                                if (ItemDetail.Rows.Count == 1)
                                {
                                    foreach (DataRow dtItem in ItemDetail.Rows)
                                    {
                                        dtItemslength = dtItem.ItemArray.Length;
                                        for (int i = 0; i < dtItem.ItemArray.Length; i++)
                                        {
                                            if (dtItem.IsNull(i) || string.IsNullOrWhiteSpace(dtItem[i].ToString()))
                                            {
                                                dtItemscount++;
                                            }
                                        }
                                    }
                                    if (dtItemscount >= dtItemslength)
                                    {
                                        return Json("Excel file is empty. Please fill data in excel file and try again");
                                    }
                                }
                                ds.Tables.Add(ItemDetail);
                                ds.Tables.Add(BatchDetail);
                                ds.Tables.Add(SerialDetail);
                                ds.Tables.Add(SubItemDetail);
                                DataSet dts = VerifyData(ds, uploadStatus, Warehouse);
                                if (dts == null)
                                    return Json("Excel file is empty. Please fill data in excel file and try again");
                                ViewBag.ImportItemsPreview = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialImportOpeningMaterialReceiptDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataSet VerifyData(DataSet dscustomer, string uploadStatus, string Warehouse)
        {
            string compId = "";
            string BrchID = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            DataSet dts = PrepareDataset(dscustomer);
            if (dscustomer.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dscustomer.Tables[0].Rows[0].ToString()))
            {
                DataSet result = _OPR_ISERVICE.GetVerifiedDataOfExcel(Warehouse, BrchID, dts.Tables[0], dts.Tables[1], dts.Tables[2], dts.Tables[3], compId);
                if (uploadStatus.Trim() == "0")
                    return result;

                var filteredRows = result.Tables[0].AsEnumerable().Where(x => x.Field<string>("UploadStatus").ToUpper() == uploadStatus.ToUpper()).ToList();
                DataTable newDataTable = filteredRows.Any() ? filteredRows.CopyToDataTable() : result.Tables[0].Clone();
                result.Tables[0].Clear();

                for (int i = 0; i < newDataTable.Rows.Count; i++)
                {
                    result.Tables[0].ImportRow(newDataTable.Rows[i]);
                }
                result.Tables[0].AcceptChanges();
                return result;
            }
            else
            {
                return null;
            }
        }
        public DataSet PrepareDataset(DataSet dsopenigmaterial)
        {
            CommonController cmn = new CommonController();
            double op_val = 0.0;
            DataTable ItemDetail = new DataTable();
            DataTable BatchDetail = new DataTable();
            DataTable SerialDetail = new DataTable();
            DataTable SubItemDetail = new DataTable();
            DataTable opval = new DataTable();
            ItemDetail.Columns.Add("item_name", typeof(string));
            ItemDetail.Columns.Add("op_qty", typeof(string));
            ItemDetail.Columns.Add("item_rate", typeof(string));

            for (int i = 0; i < dsopenigmaterial.Tables[0].Rows.Count; i++)
            {
                DataTable dtitemdetail = dsopenigmaterial.Tables[0];
                DataRow dtr = ItemDetail.NewRow();
                dtr["item_name"] = dtitemdetail.Rows[i][0].ToString().Trim();
                string qtyString = dtitemdetail.Rows[i][1].ToString().Trim();
                string rateString = dtitemdetail.Rows[i][2].ToString().Trim();
                float qty = 0;
                float.TryParse(qtyString, out qty);
                float rate = 0;
                float.TryParse(rateString, out rate);
                op_val += qty * rate;  
                
                decimal qtyDecimal = 0.0m;
                if (string.IsNullOrEmpty(qtyString))
                {
                    if (dtitemdetail.Rows[i][1] == DBNull.Value || string.IsNullOrEmpty(qtyString))
                    {
                        qtyDecimal = 0.0m;
                    }
                    else
                    {
                        if (decimal.TryParse(qtyString, out qtyDecimal))
                        {
                        }
                        else
                        {
                            qtyDecimal = 0.0m;
                        }
                    }
                    dtr["op_qty"] = qtyDecimal.ToString("0000.00");
                }
                else
                {
                    float itmrate = 0;
                    float.TryParse(dtitemdetail.Rows[i][1].ToString().Trim(), out itmrate);
                    dtr["op_qty"] = itmrate.ToString("F3");
                }
                if (string.IsNullOrEmpty(rateString))
                {
                    if (dtitemdetail.Rows[i][2] == DBNull.Value || string.IsNullOrEmpty(rateString))
                    {
                        qtyDecimal = 0.0m;
                    }
                    else
                    {
                        if (decimal.TryParse(rateString, out qtyDecimal))
                        {
                        }
                        else
                        {
                            qtyDecimal = 0.0m;
                        }
                    }
                    dtr["item_rate"] = qtyDecimal.ToString("0000.00");
                }
                else
                {
                    float itmrate = 0;
                    float.TryParse(dtitemdetail.Rows[i][2].ToString().Trim(), out itmrate);
                    dtr["item_rate"] = itmrate.ToString("F3");
                }
                ItemDetail.Rows.Add(dtr);
            }
            //------------------------Batch Detail---------------------
            BatchDetail.Columns.Add("item_name", typeof(string));
            BatchDetail.Columns.Add("batch_no", typeof(string));
            BatchDetail.Columns.Add("batch_qty", typeof(string));
            BatchDetail.Columns.Add("Mft_name", typeof(string));
            BatchDetail.Columns.Add("MRP", typeof(string));
            BatchDetail.Columns.Add("Mft_dt", typeof(string));
            BatchDetail.Columns.Add("exp_dt", typeof(string));
            for (int i = 0; i < dsopenigmaterial.Tables[1].Rows.Count; i++)
            {
                DataTable dtbatchdetail = dsopenigmaterial.Tables[1];
                DataRow dtr = BatchDetail.NewRow();
                dtr["item_name"] = dtbatchdetail.Rows[i][0].ToString().Trim();
                dtr["batch_no"] = dtbatchdetail.Rows[i][1].ToString().Trim();
                //string qtyString = dtbatchdetail.Rows[i][2].ToString().Trim();
                //decimal qtyDecimal = 0.0m;
                //if (string.IsNullOrEmpty(qtyString))
                //{
                //    if (dtbatchdetail.Rows[i][2] == DBNull.Value || string.IsNullOrEmpty(qtyString))
                //    {
                //        qtyDecimal = 0.0m;
                //    }
                //    else
                //    {
                //        if (decimal.TryParse(qtyString, out qtyDecimal))
                //        {
                //        }
                //        else
                //        {
                //            qtyDecimal = 0.0m;
                //        }
                //    }
                //    dtr["batch_qty"] = qtyDecimal.ToString("0000.00");
                //}
                //else
                //{
                //    float batQty = 0;
                //    float.TryParse(dtbatchdetail.Rows[i][2].ToString().Trim(), out batQty);
                //    dtr["batch_qty"] = batQty.ToString("F3");
                //}
                dtr["batch_qty"] = FormatNumber(dtbatchdetail.Rows[i][2]);
                dtr["Mft_name"] = dtbatchdetail.Rows[i][3].ToString().Trim();
                dtr["MRP"] = FormatNumber(dtbatchdetail.Rows[i][4]);
                dtr["Mft_dt"] = FormatDate(dtbatchdetail.Rows[i][5]);
                dtr["exp_dt"] = FormatDate(dtbatchdetail.Rows[i][6]);
                //string inputDate = dtbatchdetail.Rows[i][6].ToString().Trim();
                //string format1 = @"^\d{2}-\d{2}-\d{4}$";
                //string format2 = @"^\d{4}-\d{2}-\d{2}$";
                //if (!Regex.IsMatch(inputDate, format1) && !Regex.IsMatch(inputDate, format2))
                //{
                //    dtr["exp_dt"] = dtbatchdetail.Rows[i][6].ToString().Trim();
                //}
                //else
                //{
                //    DateTime parsedDate;
                //    if (DateTime.TryParseExact(inputDate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                //    {
                //        dtr["exp_dt"] = parsedDate.ToString("yyyy-MM-dd");
                //    }
                //    else
                //    {
                //        dtr["exp_dt"] = dtbatchdetail.Rows[i][6].ToString().Trim();
                //    }
                //}
                BatchDetail.Rows.Add(dtr);
            }
            //------------------------End-------------------------------
            //------------------------Serial Detail---------------------
            SerialDetail.Columns.Add("item_name", typeof(string));
            SerialDetail.Columns.Add("serial_no", typeof(string));
            SerialDetail.Columns.Add("Mft_name", typeof(string));
            SerialDetail.Columns.Add("MRP", typeof(string));
            SerialDetail.Columns.Add("Mft_dt", typeof(string));
            for (int i = 0; i < dsopenigmaterial.Tables[2].Rows.Count; i++)
            {
                DataTable dtserialdetail = dsopenigmaterial.Tables[2];
                DataRow dtr = SerialDetail.NewRow();
                dtr["item_name"] = dtserialdetail.Rows[i][0].ToString().Trim();
                dtr["serial_no"] = dtserialdetail.Rows[i][1].ToString().Trim();
                dtr["Mft_name"] = dtserialdetail.Rows[i][2].ToString().Trim();
                dtr["MRP"] = FormatNumber(dtserialdetail.Rows[i][3]);
                dtr["Mft_dt"] = FormatDate(dtserialdetail.Rows[i][4]);
                SerialDetail.Rows.Add(dtr);
            }
            //------------------------End-------------------------------
            //------------------------SubItem Detail---------------------
            SubItemDetail.Columns.Add("item_name", typeof(string));
            SubItemDetail.Columns.Add("sub_item_name", typeof(string));
            SubItemDetail.Columns.Add("qty", typeof(string));
            for (int i = 0; i < dsopenigmaterial.Tables[3].Rows.Count; i++)
            {
                DataTable dtsubitemdetail = dsopenigmaterial.Tables[3];
                DataRow dtr = SubItemDetail.NewRow();
                dtr["item_name"] = dtsubitemdetail.Rows[i][0].ToString().Trim();
                dtr["sub_item_name"] = dtsubitemdetail.Rows[i][1].ToString().Trim();
                string qtyString = dtsubitemdetail.Rows[i][2].ToString().Trim();
                decimal qtyDecimal = 0.0m;
                if (string.IsNullOrEmpty(qtyString))
                {
                    if (dtsubitemdetail.Rows[i][2] == DBNull.Value || string.IsNullOrEmpty(qtyString))
                    {
                        qtyDecimal = 0.0m;
                    }
                    else
                    {
                        if (decimal.TryParse(qtyString, out qtyDecimal))
                        {
                        }
                        else
                        {
                            qtyDecimal = 0.0m;
                        }
                    }
                    dtr["qty"] = qtyDecimal.ToString("0000.00");
                }
                else
                {
                    float itmrate = 0;
                    float.TryParse(dtsubitemdetail.Rows[i][2].ToString().Trim(), out itmrate);
                    dtr["qty"] = itmrate.ToString("F3");
                }
                SubItemDetail.Rows.Add(dtr);
            }
            opval.Columns.Add("op_val", typeof(float));
            opval.Rows.Add(Math.Round(op_val, 2));
            //------------------------End-------------------------------
            DataSet dts = new DataSet();
            dts.Tables.Add(ItemDetail);
            dts.Tables.Add(BatchDetail);
            dts.Tables.Add(SerialDetail);
            dts.Tables.Add(SubItemDetail);
            dts.Tables.Add(opval);
            return dts;
        }
        public ActionResult ShowValidationError(string ItemName, string Warehouse)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable ItemDetail = new DataTable();
                    DataTable BatchDetail = new DataTable();
                    DataTable SerialDetail = new DataTable();
                    DataTable SubItemDetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string ItemDetailQuery = "";
                                if (string.IsNullOrWhiteSpace(ItemName))
                                {
                                    ItemDetailQuery = "SELECT DISTINCT * FROM [ItemDetail$] WHERE [Item Name*] IS NULL OR LTRIM(RTRIM([Item Name])) = '';";
                                }
                                else
                                {
                                    ItemDetailQuery = "SELECT DISTINCT * FROM [ItemDetail$] WHERE [Item Name*] = '" + ItemName + "';";
                                }
                                string BatchDetailQuery = "SELECT DISTINCT * From [BatchDetail$] where [Item Name] = '" + ItemName + "' ; ";
                                string SerialdetailQuery = "SELECT DISTINCT * From [SerialDetail$] where [Item Name] = '" + ItemName + "' ; ";
                                string SubItemQuery = "SELECT DISTINCT * From [SubItemDetail$] where [Item Name] = '" + ItemName + "' ; ";


                                connExcel.Open();
                                cmdExcel.CommandText = ItemDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(ItemDetail);

                                cmdExcel.CommandText = BatchDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(BatchDetail);

                                cmdExcel.CommandText = SerialdetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SerialDetail);
                                connExcel.Close();

                                cmdExcel.CommandText = SubItemQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SubItemDetail);
                                connExcel.Close();

                                ds.Tables.Add(ItemDetail);
                                ds.Tables.Add(BatchDetail);
                                ds.Tables.Add(SerialDetail);
                                ds.Tables.Add(SubItemDetail);
                                DataTable dts = VerifySingleData(ds, Warehouse);
                                ViewBag.ErrorDetails = dts;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/Common/Views/Cmn_PartialExportErrorDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private DataTable VerifySingleData(DataSet ds, string Warehouse)
        {
            string compId = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            DataSet dts = PrepareDataset(ds);
            DataTable result = _OPR_ISERVICE.ShowExcelErrorDetail(Warehouse, BrchID, dts.Tables[0], dts.Tables[1], dts.Tables[2], dts.Tables[3], compId);
            return result;
        }
        public ActionResult BindBatchDetail(string ItemName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();
                CommonController cmn = new CommonController();
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": // Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": // Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            throw new Exception("Invalid file type");
                    }

                    DataTable dtbatchdetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string batchQuery = "SELECT DISTINCT * From [BatchDetail$] where [Item Name] = '" + ItemName + "' ; ";
                                cmdExcel.CommandText = batchQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtbatchdetail);

                                DataTable BatchDetail = new DataTable();
                                BatchDetail.Columns.Add("item_name", typeof(string));
                                BatchDetail.Columns.Add("batch_no", typeof(string));
                                BatchDetail.Columns.Add("batch_qty", typeof(string));
                                BatchDetail.Columns.Add("Mft_name", typeof(string));
                                BatchDetail.Columns.Add("MRP", typeof(string));
                                BatchDetail.Columns.Add("Mft_dt", typeof(string));
                                BatchDetail.Columns.Add("exp_dt", typeof(string));
                                for (int i = 0; i < dtbatchdetail.Rows.Count; i++)
                                {
                                    DataTable dtbranchdetail = dtbatchdetail;
                                    DataRow dtr = BatchDetail.NewRow();
                                    dtr["item_name"] = dtbatchdetail.Rows[i][0].ToString().Trim();
                                    dtr["batch_no"] = dtbatchdetail.Rows[i][1].ToString().Trim();
                                    dtr["batch_qty"] = FormatNumber(dtbatchdetail.Rows[i][2]);
                                    dtr["Mft_name"] = dtbatchdetail.Rows[i][3].ToString().Trim();
                                    dtr["MRP"] = FormatNumber(dtbatchdetail.Rows[i][4]);
                                    dtr["Mft_dt"] = dtbatchdetail.Rows[i][5].ToString().Trim();
                                    dtr["exp_dt"] = dtbatchdetail.Rows[i][6].ToString().Trim();
                                    BatchDetail.Rows.Add(dtr);
                                }
                                ViewBag.BatchDetail = BatchDetail;
                                ViewBag.Name = "BatchDetail";
                            }
                        }
                    }
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBatchSerialSubItemDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult BindSerialDetail(string ItemName)
        {
            try
            {
                CommonController cmn = new CommonController();
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();

                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": // Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": // Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            throw new Exception("Invalid file type");
                    }

                    DataTable dtserialdetail = new DataTable();
                    string oppening_qty = "";
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string serialQuery = "SELECT DISTINCT * From [SerialDetail$] where [Item Name] = '" + ItemName + "' ; ";

                                cmdExcel.CommandText = serialQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtserialdetail);

                                DataTable SerialDetail = new DataTable();
                                SerialDetail.Columns.Add("item_name", typeof(string));
                                SerialDetail.Columns.Add("serial_no", typeof(string));
                                SerialDetail.Columns.Add("Mft_name", typeof(string));
                                SerialDetail.Columns.Add("MRP", typeof(string));
                                SerialDetail.Columns.Add("Mft_dt", typeof(string));
                                for (int i = 0; i < dtserialdetail.Rows.Count; i++)
                                {
                                    DataTable dtbranchdetail = dtserialdetail;
                                    DataRow dtr = SerialDetail.NewRow();
                                    dtr["item_name"] = dtserialdetail.Rows[i][0].ToString().Trim();
                                    dtr["serial_no"] = dtserialdetail.Rows[i][1].ToString().Trim();
                                    dtr["Mft_name"] = dtserialdetail.Rows[i][2].ToString().Trim();
                                    dtr["MRP"] = FormatNumber(dtserialdetail.Rows[i][3]);
                                    dtr["Mft_dt"] = dtserialdetail.Rows[i][4].ToString().Trim();
                                    SerialDetail.Rows.Add(dtr);
                                }
                                ViewBag.SerialDetail = SerialDetail;
                                ViewBag.Name = "SerialDetail";
                                ViewBag.Opening_Quantity = oppening_qty;
                            }
                        }
                    }
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBatchSerialSubItemDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult BindSubItemDetail(string ItemName)
        {
            try
            {
                if (Session["CompId"] != null)
                    CompID = Session["CompId"].ToString();

                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": // Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": // Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                        default:
                            throw new Exception("Invalid file type");
                    }

                    DataTable dtsubitemdetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string serialQuery = "SELECT DISTINCT * FROM [SubItemDetail$] WHERE [Item Name] = '" + ItemName + "';";


                                cmdExcel.CommandText = serialQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dtsubitemdetail);

                                DataTable SubItemDetail = new DataTable();
                                SubItemDetail.Columns.Add("item_name", typeof(string));
                                SubItemDetail.Columns.Add("sub_item_name", typeof(string));
                                SubItemDetail.Columns.Add("qty", typeof(string));
                                for (int i = 0; i < dtsubitemdetail.Rows.Count; i++)
                                {
                                    DataTable dtbranchdetail = dtsubitemdetail;
                                    DataRow dtr = SubItemDetail.NewRow();
                                    dtr["item_name"] = dtsubitemdetail.Rows[i][0].ToString().Trim();
                                    dtr["sub_item_name"] = dtsubitemdetail.Rows[i][1].ToString().Trim();
                                    string qtyString = dtsubitemdetail.Rows[i][2].ToString().Trim();
                                    decimal qtyDecimal = 0.0m;
                                    if (string.IsNullOrEmpty(qtyString))
                                    {
                                        if (dtsubitemdetail.Rows[i][2] == DBNull.Value || string.IsNullOrEmpty(qtyString))
                                        {
                                            qtyDecimal = 0.0m;
                                        }
                                        else
                                        {
                                            if (decimal.TryParse(qtyString, out qtyDecimal))
                                            {
                                            }
                                            else
                                            {
                                                qtyDecimal = 0.0m;
                                            }
                                        }
                                        dtr["qty"] = qtyDecimal.ToString("0000.00");
                                    }
                                    else
                                    {
                                        float subQty = 0;
                                        float.TryParse(dtsubitemdetail.Rows[i][2].ToString().Trim(), out subQty);
                                        dtr["qty"] = subQty.ToString("F3");
                                    }
                                    SubItemDetail.Rows.Add(dtr);
                                }
                                ViewBag.SubItemDetail = SubItemDetail;
                                ViewBag.Name = "SubItemDetail";
                            }
                        }
                    }
                }
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialBatchSerialSubItemDetail.cshtml");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        public JsonResult ImportOMRDetailFromExcel(string Warehouse, string op_dt)
        {
            try
            {
                string filePath = string.Empty;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase postedFile = Request.Files[0];
                    string path = Server.MapPath("~");
                    string currentDir = Environment.CurrentDirectory;
                    DirectoryInfo directory = new DirectoryInfo(currentDir);

                    string FolderPath = path + ("..\\ImportExcelFiles\\");
                    bool exists = System.IO.Directory.Exists(FolderPath);
                    if (!exists)
                    {
                        Directory.CreateDirectory(FolderPath);
                    }
                    filePath = FolderPath + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                            break;
                    }
                    DataSet ds = new DataSet();
                    DataTable ItemDetail = new DataTable();
                    DataTable BatchDetail = new DataTable();
                    DataTable SerialDetail = new DataTable();
                    DataTable SubItemDetail = new DataTable();
                    conString = string.Format(conString, filePath);

                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;

                                //Get the name of First Sheet.
                                connExcel.Open();
                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                connExcel.Close();
                                string ItemDetailQuery = "SELECT DISTINCT * From [ItemDetail$] ; ";
                                string BatchDetailQuery = "SELECT DISTINCT * From [BatchDetail$] ; ";
                                string SerialdetailQuery = "SELECT DISTINCT * From [SerialDetail$]; ";
                                string SubItemQuery = "SELECT DISTINCT * From [SubItemDetail$]; ";

                                connExcel.Open();
                                cmdExcel.CommandText = ItemDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(ItemDetail);

                                cmdExcel.CommandText = BatchDetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(BatchDetail);

                                cmdExcel.CommandText = SerialdetailQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SerialDetail);
                                connExcel.Close();

                                cmdExcel.CommandText = SubItemQuery;
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(SubItemDetail);
                                connExcel.Close();


                                ds.Tables.Add(ItemDetail);
                                ds.Tables.Add(BatchDetail);
                                ds.Tables.Add(SerialDetail);
                                ds.Tables.Add(SubItemDetail);
                                string msg = SaveOMRFromExcel(ds, Warehouse, op_dt);
                                return Json(msg, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
                else
                    return Json("No file selected", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("cannot insert duplicate"))
                    return Json("something went wrong", JsonRequestBehavior.AllowGet);
                else
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        private string SaveOMRFromExcel(DataSet dsOMR, string Warehouse, string op_dt)
        {
            string compId = "";
            string UserID = "";
            string BrchID = "";
            if (Session["compid"] != null)
                compId = Session["compid"].ToString();
            if (Session["BranchId"] != null)
                BrchID = Session["BranchId"].ToString();
            if (Session["userid"] != null)
                UserID = Session["userid"].ToString();
            DataSet dts = PrepareDataset(dsOMR);
            DataTable table = dts.Tables[4];
            float op_val1 = 0;
            if (table.Rows.Count > 0)
            {
                string valueString = table.Rows[0][0].ToString().Trim();
                float.TryParse(valueString, out op_val1);
            }
            string result = _OPR_ISERVICE.BulkImportOMRDetail(compId, UserID, BrchID, Warehouse, op_dt, op_val1, dts.Tables[0], dts.Tables[1], dts.Tables[2], dts.Tables[3]);
            return result;
        }

        private object IsBlank(string input, object output)//Added by Suraj Maurya on 27-11-2025
        {
            return input == "" ? output : input;
        }
        public ActionResult SearchOpeningDetail(string financial_year)
        {
            try
            {
                OpeningMaterial_ListModel _OpeningMaterial_ListModel = new OpeningMaterial_ListModel();

                List<OpeningMaterialList> _OpeningMaterialList = new List<OpeningMaterialList>();
                DataSet dt = new DataSet();
                string UserID = "";
                string Wf_status = "Search";
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
               
                dt = _OPR_ISERVICE.GetOPRDetailList(CompID, BrchID, financial_year, Wf_status, UserID, DocumentMenuId);
                if (dt.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        OpeningMaterialList _OPList = new OpeningMaterialList();
                        _OPList.OPRDate = dr["OPDate"].ToString();
                        _OPList.OPR_DT = dr["OP_DT"].ToString();
                        _OPList.wh_Name = dr["wh_name"].ToString();
                        _OPList.wh_id = dr["wh_id"].ToString();
                        _OPList.id = dr["id"].ToString();
                        _OPList.Stauts = dr["Status"].ToString();
                        _OPList.CreateDate = dr["CreateDate"].ToString();
                        _OPList.ApproveDate = dr["ApproveDate"].ToString();
                        _OPList.create_by = dr["create_by"].ToString();
                        _OPList.app_by = dr["app_by"].ToString();
                        _OpeningMaterialList.Add(_OPList);
                    }
                    _OpeningMaterial_ListModel.OPR_List = _OpeningMaterialList;
                }
                else
                {
                    _OpeningMaterial_ListModel.OPR_List = null;
                }
                _OpeningMaterial_ListModel.OMRSearch = "OMRSearch";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialOpeningMaterialList.cshtml", _OpeningMaterial_ListModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        //Added By Nidhi on 08-12-2025
        private string FormatDate(object value)
        {
            if (value == null) return "";
            string input = value.ToString().Trim();
            if (string.IsNullOrEmpty(input)) return "";
            DateTime date;
            string[] formats = { "dd-MM-yyyy", "yyyy-MM-dd" };

            if (DateTime.TryParseExact(input, formats,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out date))
            {
                return date.ToString("yyyy-MM-dd");
            }
            return input;
        }
        public string FormatNumber(object value)
        {
            if (value == null || value == DBNull.Value)
                return "0000.00";
            var text = value.ToString().Trim();
            if (string.IsNullOrEmpty(text))
                return "0000.00";
            decimal number;
            if (decimal.TryParse(text, out number))
                return number.ToString("F3");
            return "0000.00";
        }

    }

}