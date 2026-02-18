using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.PurchaseRequisition;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.PurchaseRequisition;
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
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.PurchaseRequisition
{
    public class PurchaseRequisitionController : Controller
    {
        string CompID, userid, branchID, UserID, language = String.Empty;
        string DocumentMenuId = "105101110", title, pr_no;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        PurchaseRequisition_ISERVICES purchaseRequisition_ISERVICES;
        public PurchaseRequisitionController(Common_IServices _Common_IServices, PurchaseRequisition_ISERVICES purchaseRequisition_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this.purchaseRequisition_ISERVICES = purchaseRequisition_ISERVICES;
        }
        // GET: ApplicationLayer/PurchaseRequisition
        public ActionResult PurchaseRequisition(PurchaseRequisition_Model PRLIST_Model)
        {
            try
            {
                //Session["AttachMentDetailItmStp"] = null;
                //Session["Guid"] = null;
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
                    branchID = Session["BranchId"].ToString();
                }
                CommonPageDetails();
                #region Commented By Nitesh 30-03-2024 for All Data Is Come CommonPageDeatil
                #endregion
                //var other = new CommonController(_Common_IServices);
                //ViewBag.AppLevel = other.GetApprovalLevel(CompID, branchID, DocumentMenuId);
                //var statusListsC = other.GetStatusList1(DocumentMenuId);
                //var listOfStatus = statusListsC.ConvertAll(x => new StatusList { status_id = x.status_id, status_name = x.status_name });
                //PurchaseRequisition_Model PRLIST_Model = new PurchaseRequisition_Model();
                ViewBag.DocumentMenuId = DocumentMenuId;
                PRLIST_Model.TransType = "Save";
                PRLIST_Model.Command = "New";
                PRLIST_Model.BtnName = "BtnAddNew";
                PRLIST_Model.DocumentStatus = "D";
                string wfstatus = "";
                if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
                {
                    PRLIST_Model.WF_status = TempData["WF_status"].ToString();
                    //if (Session["WF_status"] != null)
                    if (PRLIST_Model.WF_status != null)
                    {
                        //wfstatus = Session["WF_status"].ToString();
                        wfstatus = PRLIST_Model.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
                else
                {
                    //if (Session["WF_status"] != null)
                    if (PRLIST_Model.WF_status != null)
                    {
                        //wfstatus = Session["WF_status"].ToString();
                        wfstatus = PRLIST_Model.WF_status;
                    }
                    else
                    {
                        wfstatus = "";
                    }
                }
               
                DateTime dtnow = DateTime.Now;
                //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");
                var range = CommonController.Comman_GetFutureDateRange();
                string startDate = range.FromDate;
                string EndDate = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day).ToString("yyyy-MM-dd");
                if (TempData["PRData"] != null && TempData["PRData"].ToString() != "")
                {
                    var PRData = TempData["PRData"].ToString();
                    if (PRData != null && PRData != "")
                    {
                        var a = PRData.Split(',');
                        var req_area = a[0].Trim();
                        PRLIST_Model.req_area = Convert.ToInt32(req_area);
                        PRLIST_Model.PR_FromDate = a[1].Trim();
                        PRLIST_Model.PR_status = a[3].Trim();
                        if (PRLIST_Model.PR_status == "0")
                        {
                            PRLIST_Model.PR_status = null;
                        }
                        PRLIST_Model.PR_ToDate = a[2].Trim();
                        PRLIST_Model.PRData = TempData["PRData"].ToString();
                    }
                    else
                    {
                        PRLIST_Model.PR_FromDate = startDate;
                    }
                }
                else
                {
                    PRLIST_Model.PR_FromDate = startDate;
                }
                GetAllData(PRLIST_Model);

              //  ViewBag.MenuPageName = getDocumentName();
                PRLIST_Model.Title = title;
                //Session["PRSearch"] = "0";
                PRLIST_Model.PRSearch = "0";
               // ViewBag.VBRoleList = GetRoleList();
                return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseRequisition/PurchaseRequisitionList.cshtml", PRLIST_Model);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
        }
        private void CommonPageDetails()
        {
            try
            {
                string BrchID =string.Empty;
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
        private void GetAllData(PurchaseRequisition_Model PRLIST_Model)/**Added By Nitesh 30-03-2024 For All data in One procedure**/
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
            
            DataSet Data = purchaseRequisition_ISERVICES.GetAllData(CompID, BrchID, pr_no, PRLIST_Model.req_area, PRLIST_Model.PR_FromDate, PRLIST_Model.PR_ToDate, PRLIST_Model.PR_status, UserID, PRLIST_Model.WF_status, DocumentMenuId);
            //DataTable dt = new DataTable();
            List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
          //  dt = GetRequirmentreaList();
            foreach (DataRow dr in Data.Tables[0].Rows)
            {
                RequirementAreaList _RAList = new RequirementAreaList();
                _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                _RAList.req_val = dr["setup_val"].ToString();
                requirementAreaLists.Add(_RAList);
            }
            requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "All" });
            PRLIST_Model._requirementAreaLists = requirementAreaLists;

            
            List<StatusList> statusLists = new List<StatusList>();
            if (ViewBag.StatusList.Rows.Count > 0)
            {
                foreach (DataRow data in ViewBag.StatusList.Rows)
                {
                    StatusList _Statuslist = new StatusList();
                    _Statuslist.status_id = data["status_code"].ToString();
                    _Statuslist.status_name = data["status_name"].ToString();
                    statusLists.Add(_Statuslist);
                }
            }

            PRLIST_Model.statusLists = statusLists;

            //DataSet dt1 = purchaseRequisition_ISERVICES.GetPRDetailList(pr_no, CompID, branchID, PRLIST_Model.req_area, PRLIST_Model.PR_FromDate, PRLIST_Model.PR_ToDate, PRLIST_Model.PR_status, UserID, PRLIST_Model.WF_status, DocumentMenuId);
            ViewBag.PRDetailsList = Data.Tables[1];
            ViewBag.AttechmentDetails = Data.Tables[3];
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
        [HttpPost]
        public ActionResult SearchPRDetail(int req_area, string Fromdate, string Todate, string Status)
        {
            try
            {
                PurchaseRequisition_Model PRLIST_Model = new PurchaseRequisition_Model();
                //Session["WF_status"] = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }

                DataTable dt1 = purchaseRequisition_ISERVICES.GetPRDetailList(pr_no, CompID, branchID, req_area, Fromdate, Todate, Status, UserID, "", DocumentMenuId).Tables[0];

                ViewBag.PRDetailsList = dt1;
                //Session["PRSearch"] = "PR_Search";
                PRLIST_Model.PRSearch = "PR_Search";

                //string[] Filter_Details = { req_area.ToString() + ',' + Fromdate + ',' + Todate + ',' + Status };
                //Session["Filter_Details"] =  string.Concat(Filter_Details);

                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialPurchaseRequisitionList.cshtml", PRLIST_Model);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult AddBtnFromList()
        {
            PurchaseRequisition_Model PRLIST_Model = new PurchaseRequisition_Model();
            /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                branchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                PRLIST_Model.Message = "Financial Year not Exist";
                return RedirectToAction("PurchaseRequisition", PRLIST_Model);
            }
            /*End to chk Financial year exist or not*/
            PRLIST_Model.TransType = "Save";
            PRLIST_Model.Command = "New";
            PRLIST_Model.Message = "New";
            PRLIST_Model.BtnName = "BtnAddNew";
            PRLIST_Model.DocumentStatus = "D";
            //Session["Filter_Details"] = null;
            TempData["ModelData"] = PRLIST_Model;
            TempData["PRData"] = null;
            //Session["TransType"] = "Save";
            //Session["Command"] = "New";
            //Session["Message"] = "New";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";
            //Session["Filter_Details"] = null;         
            return RedirectToAction("AddPurchaseRequisitionDetail");
        }
        public ActionResult GetPRTrackingDetail(string PR_No, string PR_Date)
        {
            try
            {
                JsonResult DataRows = null;
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
                DataSet result = purchaseRequisition_ISERVICES.GetPOTrackingDetail(Comp_ID, BranchID, PR_No, PR_Date);
                DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                ViewBag.PRTrackingList = result.Tables[0];

                return View("~/Areas/ApplicationLayer/Views/Shared/_PurchaseRequisitionTracking.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }
        public ActionResult GetPRListDashboard(string docid, string status)
        {
            PurchaseRequisition_Model PRLIST_Model = new PurchaseRequisition_Model();
            //Session["WF_status"] = status;
            PRLIST_Model.WF_status = status;
            return RedirectToAction("PurchaseRequisition", PRLIST_Model);
        }
        public ActionResult AddPurchaseRequisitionDetail(PurchaseRequisition_Model purchaseRequisition_Model1, string PR_Number, string PR_Date, string TransType, string BtnName, string Command)
        {
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
                branchID = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                UserID = Session["userid"].ToString();
            }
            /*Add by Hina sharma on 30-04-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PR_Date) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            CommonPageDetails();
            try
            {
                var purchaseRequisition_Model = TempData["ModelData"] as PurchaseRequisition_Model;
                if (purchaseRequisition_Model != null)
                {
                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    purchaseRequisition_Model.QtyDigit = QtyDigit;

                    DataTable dt = new DataTable();
                    List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                    dt = GetRequirmentreaList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        RequirementAreaList _RAList = new RequirementAreaList();
                        _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                        _RAList.req_val = dr["setup_val"].ToString();
                        requirementAreaLists.Add(_RAList);
                    }
                    requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "---Select---" });
                    purchaseRequisition_Model._requirementAreaLists = requirementAreaLists;
                    purchaseRequisition_Model.SourceType = "D";
                  
                  //  ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, branchID, DocumentMenuId).Tables[0];
                    if (TempData["PRData"] != null && TempData["PRData"].ToString() != "")
                    {
                        purchaseRequisition_Model.PRData1 = TempData["PRData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        purchaseRequisition_Model.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (purchaseRequisition_Model.TransType == "Update" || purchaseRequisition_Model.TransType == "Edit")
                    {
                        //string pr_no = Session["PR_No"].ToString();
                        //string pr_dt = Session["PR_Date"].ToString();
                        string pr_no = purchaseRequisition_Model.PR_Number;
                        string pr_dt = purchaseRequisition_Model.PR_Date;
                        DataSet ds = purchaseRequisition_ISERVICES.getdetailsPR(CompID, branchID, pr_no, pr_dt, UserID, DocumentMenuId);
                       
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        ViewBag.UOMList = ds.Tables[7];
                        purchaseRequisition_Model.PR_No = ds.Tables[0].Rows[0]["pr_no"].ToString();
                        purchaseRequisition_Model.Req_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["pr_dt"].ToString());
                        purchaseRequisition_Model.SourceType = ds.Tables[0].Rows[0]["pr_type"].ToString();
                        purchaseRequisition_Model.req_area = Convert.ToInt32(ds.Tables[0].Rows[0]["req_area"].ToString());
                        purchaseRequisition_Model.Req_By = ds.Tables[0].Rows[0]["req_by"].ToString();
                        purchaseRequisition_Model.Remarks = ds.Tables[0].Rows[0]["pr_rem"].ToString();
                        purchaseRequisition_Model.CreatedBy = ds.Tables[0].Rows[0]["create_nm"].ToString();
                        purchaseRequisition_Model.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        purchaseRequisition_Model.ApprovedBy = ds.Tables[0].Rows[0]["app_nm"].ToString();
                        purchaseRequisition_Model.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        purchaseRequisition_Model.AmmendedBy = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                        purchaseRequisition_Model.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        purchaseRequisition_Model.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        purchaseRequisition_Model.PR_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        purchaseRequisition_Model.SourceDocumentNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        purchaseRequisition_Model.SourceDocumentDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();

                        purchaseRequisition_Model.ForAmmendendBtn = ds.Tables[9].Rows[0]["flag"].ToString().Trim();
                        purchaseRequisition_Model.Amendment = ds.Tables[8].Rows[0]["Amendment"].ToString();

                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        purchaseRequisition_Model.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        purchaseRequisition_Model.DocumentStatus = doc_status;

                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            purchaseRequisition_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            purchaseRequisition_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            purchaseRequisition_Model.CancelFlag = false;
                        }
                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "FC")
                        {
                            purchaseRequisition_Model.ForceClose = true;
                            //Session["BtnName"] = "Refresh";
                            purchaseRequisition_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            purchaseRequisition_Model.ForceClose = false;
                        }
                        purchaseRequisition_Model.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        purchaseRequisition_Model.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);

                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = ds.Tables[4];
                        }

                        if (ViewBag.AppLevel != null && purchaseRequisition_Model.Command != "Edit")
                        {
                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }
                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    purchaseRequisition_Model.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];

                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        purchaseRequisition_Model.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        purchaseRequisition_Model.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    purchaseRequisition_Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        purchaseRequisition_Model.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    purchaseRequisition_Model.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    purchaseRequisition_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    purchaseRequisition_Model.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    purchaseRequisition_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        //    ViewBag.MenuPageName = getDocumentName();
                        if (purchaseRequisition_Model.Amend == "Amend")
                        {
                            purchaseRequisition_Model.OrdStatus = "D";
                            purchaseRequisition_Model.DocumentStatus = "D";
                            ViewBag.DocumentStatus = "D";
                        }
                        purchaseRequisition_Model.UserID = UserID;
                        purchaseRequisition_Model.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        //   ViewBag.VBRoleList = GetRoleList();
                        if (purchaseRequisition_Model.Amendment != "Amendment" && purchaseRequisition_Model.Amendment != "" && purchaseRequisition_Model.Amendment != null)
                        {
                            purchaseRequisition_Model.BtnName = "BtnRefresh";
                            purchaseRequisition_Model.wfDisableAmnd = "wfDisableAmnd";
                        }
                        return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseRequisition/PurchaseRequisition.cshtml", purchaseRequisition_Model);
                    }
                    else
                    {
                      //  ViewBag.MenuPageName = getDocumentName();
                        purchaseRequisition_Model.Title = title;
                        //Session["DocumentStatus"] = "D";
                        purchaseRequisition_Model.DocumentStatus = "D";
                       // ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseRequisition/PurchaseRequisition.cshtml", purchaseRequisition_Model);
                    }
                }
                else
                {/*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                    PurchaseRequisition_Model PRLIST_Model = new PurchaseRequisition_Model();
                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        branchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //    //return RedirectToAction("PurchaseRequisition", PRLIST_Model);
                    //}
                    ///*Above Commented and modify by Hina sharma on 30-04-2025 to check Existing with previous year transaction*/
                    //string PRDate3 = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                    //if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PRDate3) == "TransNotAllow")
                    //{
                    //    TempData["Message1"] = "TransNotAllow";
                    //    return RedirectToAction("EditPR", new { DocNo = purchaseRequisition_Model.PR_No, DocDate = purchaseRequisition_Model.PR_Date, ListFilterData = purchaseRequisition_Model.PRData1, DocumentMenuId = purchaseRequisition_Model.DocumentMenuId, WF_status = purchaseRequisition_Model.WFStatus });
                    //}
                    /*End to chk Financial year exist or not*/
                    //PurchaseRequisition_Model purchaseRequisition_Model1 = new PurchaseRequisition_Model();
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    purchaseRequisition_Model1.PR_Number = PR_Number;
                    purchaseRequisition_Model1.PR_Date = PR_Date;
                    purchaseRequisition_Model1.TransType = TransType;
                    purchaseRequisition_Model1.BtnName = BtnName;
                    purchaseRequisition_Model1.Command = Command;
                    DataTable dt = new DataTable();
                    List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                    dt = GetRequirmentreaList();
                    foreach (DataRow dr in dt.Rows)
                    {
                        RequirementAreaList _RAList = new RequirementAreaList();
                        _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                        _RAList.req_val = dr["setup_val"].ToString();
                        requirementAreaLists.Add(_RAList);
                    }
                    requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "---Select---" });
                    purchaseRequisition_Model1._requirementAreaLists = requirementAreaLists;
                    purchaseRequisition_Model1.SourceType = "D";

                    string QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    purchaseRequisition_Model1.QtyDigit = QtyDigit;/*Add by hina on 23-09-2024 to show aval qty*/
                    //  ViewBag.AppLevel = _Common_IServices.GetApprovalLevel(CompID, branchID, DocumentMenuId).Tables[0];
                    if (TempData["PRData"] != null && TempData["PRData"].ToString() != "")
                    {
                        purchaseRequisition_Model1.PRData1 = TempData["PRData"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (purchaseRequisition_Model1.TransType == "Update" || purchaseRequisition_Model1.TransType == "Edit")
                    {
                        //string pr_no = Session["PR_No"].ToString();
                        string pr_no = purchaseRequisition_Model1.PR_Number;
                        string pr_dt = purchaseRequisition_Model1.PR_Date;
                        DataSet ds = purchaseRequisition_ISERVICES.getdetailsPR(CompID, branchID, pr_no, pr_dt, UserID, DocumentMenuId);
                        ViewBag.UOMList = ds.Tables[7];
                        ViewBag.AttechmentDetails = ds.Tables[5];
                        ViewBag.SubItemDetails = ds.Tables[6];
                        purchaseRequisition_Model1.PR_No = ds.Tables[0].Rows[0]["pr_no"].ToString();
                        purchaseRequisition_Model1.Req_date = Convert.ToDateTime(ds.Tables[0].Rows[0]["pr_dt"].ToString());
                        purchaseRequisition_Model1.SourceType = ds.Tables[0].Rows[0]["pr_type"].ToString();
                        purchaseRequisition_Model1.req_area = Convert.ToInt32(ds.Tables[0].Rows[0]["req_area"].ToString());
                        purchaseRequisition_Model1.Req_By = ds.Tables[0].Rows[0]["req_by"].ToString();
                        purchaseRequisition_Model1.Remarks = ds.Tables[0].Rows[0]["pr_rem"].ToString();
                        purchaseRequisition_Model1.CreatedBy = ds.Tables[0].Rows[0]["create_nm"].ToString();
                        purchaseRequisition_Model1.CreatedOn = ds.Tables[0].Rows[0]["create_dt"].ToString();
                        purchaseRequisition_Model1.ApprovedBy = ds.Tables[0].Rows[0]["app_nm"].ToString();
                        purchaseRequisition_Model1.ApprovedOn = ds.Tables[0].Rows[0]["app_dt"].ToString();
                        purchaseRequisition_Model1.AmmendedBy = ds.Tables[0].Rows[0]["mod_nm"].ToString();
                        purchaseRequisition_Model1.AmmendedOn = ds.Tables[0].Rows[0]["mod_dt"].ToString();
                        purchaseRequisition_Model1.Create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        purchaseRequisition_Model1.PR_status = ds.Tables[0].Rows[0]["app_status"].ToString();
                        purchaseRequisition_Model1.ForAmmendendBtn = ds.Tables[9].Rows[0]["flag"].ToString().Trim();
                        purchaseRequisition_Model1.Amendment = ds.Tables[8].Rows[0]["Amendment"].ToString();
                        purchaseRequisition_Model1.SourceDocumentNumber = ds.Tables[0].Rows[0]["src_doc_no"].ToString();
                        purchaseRequisition_Model1.SourceDocumentDate = ds.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        string approval_id = ds.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = ds.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = ds.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        purchaseRequisition_Model1.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        purchaseRequisition_Model1.DocumentStatus = doc_status;

                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "C")
                        {
                            purchaseRequisition_Model1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            purchaseRequisition_Model1.BtnName = "Refresh";
                        }
                        else
                        {
                            purchaseRequisition_Model1.CancelFlag = false;
                        }
                        if (ds.Tables[0].Rows[0]["status_code"].ToString().Trim() == "FC")
                        {
                            purchaseRequisition_Model1.ForceClose = true;
                            //Session["BtnName"] = "Refresh";
                            purchaseRequisition_Model1.BtnName = "Refresh";
                        }
                        else
                        {
                            purchaseRequisition_Model1.ForceClose = false;
                        }
                        purchaseRequisition_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(ds.Tables[4]);
                        purchaseRequisition_Model1.WFStatus = DataTableToJSONWithStringBuilder(ds.Tables[3]);
                        if (ViewBag.AppLevel != null && purchaseRequisition_Model1.Command != "Edit")
                        {
                            var sent_to = "";
                            var nextLevel = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                sent_to = ds.Tables[2].Rows[0]["sent_to"].ToString();
                            }
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                nextLevel = ds.Tables[3].Rows[0]["nextlevel"].ToString().Trim();
                            }
                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    purchaseRequisition_Model1.BtnName = "Refresh";
                                }
                                else
                                {
                                    if (nextLevel == "0")
                                    {
                                        if (create_id == UserID)
                                        {
                                            ViewBag.Approve = "Y";
                                            ViewBag.ForwardEnbl = "N";
                                            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                            if (TempData["Message1"] != null)
                                            {
                                                ViewBag.Message = TempData["Message1"];
                                            }
                                            /*End to chk Financial year exist or not*/
                                        }
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        purchaseRequisition_Model1.BtnName = "BtnToDetailPage";
                                    }
                                    else
                                    {
                                        ViewBag.Approve = "N";
                                        ViewBag.ForwardEnbl = "Y";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        purchaseRequisition_Model1.BtnName = "BtnToDetailPage";
                                    }
                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    purchaseRequisition_Model1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        purchaseRequisition_Model1.BtnName = "BtnToDetailPage";
                                    }
                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                    if (TempData["Message1"] != null)
                                    {
                                        ViewBag.Message = TempData["Message1"];
                                    }
                                    /*End to chk Financial year exist or not*/
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    purchaseRequisition_Model1.BtnName = "BtnToDetailPage";
                                }
                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                                        if (TempData["Message1"] != null)
                                        {
                                            ViewBag.Message = TempData["Message1"];
                                        }
                                        /*End to chk Financial year exist or not*/
                                    }
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    purchaseRequisition_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    purchaseRequisition_Model1.BtnName = "BtnToDetailPage";
                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    purchaseRequisition_Model1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        if (purchaseRequisition_Model1.Amend == "Amend")
                        {
                            purchaseRequisition_Model1.OrdStatus = "D";
                            purchaseRequisition_Model1.DocumentStatus = "D";
                            ViewBag.DocumentStatus = "D";
                        }
                        //  ViewBag.MenuPageName = getDocumentName();
                        purchaseRequisition_Model1.UserID = UserID;
                        purchaseRequisition_Model1.Title = title;
                        ViewBag.ItemDetails = ds.Tables[1];
                        //  ViewBag.VBRoleList = GetRoleList();
                        if (purchaseRequisition_Model1.Amendment != "Amendment" && purchaseRequisition_Model1.Amendment != "" && purchaseRequisition_Model1.Amendment != null)
                        {
                            purchaseRequisition_Model1.BtnName = "BtnRefresh";
                            purchaseRequisition_Model1.wfDisableAmnd = "wfDisableAmnd";
                        }
                        return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseRequisition/PurchaseRequisition.cshtml", purchaseRequisition_Model1);
                    }
                    else
                    {
                       // ViewBag.MenuPageName = getDocumentName();
                        purchaseRequisition_Model1.Title = title;
                        if (purchaseRequisition_Model1.BtnName == null)
                        {
                            purchaseRequisition_Model1.BtnName = "BtnAddNew";
                        }
                        //Session["DocumentStatus"] = "D";
                        purchaseRequisition_Model1.DocumentStatus = "D";
                      //  ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseRequisition/PurchaseRequisition.cshtml", purchaseRequisition_Model1);
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
        public string CheckRFQAgainstPR(string DocNo, string DocDate)
        {
            string str = "";
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
                DataSet Deatils = purchaseRequisition_ISERVICES.CheckRFQAgainstPR(Comp_ID, Br_ID, DocNo, DocDate);
                if (Deatils.Tables[0].Rows.Count > 0)
                {
                    str = "Used";
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                throw ex;
            }
            return str;
        }
        public ActionResult EditPR(string PRId, string PRDate, string PRData, string WF_status)
        {
            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
            PurchaseRequisition_Model PRLIST_Model = new PurchaseRequisition_Model();
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                branchID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
            //{   
            //    TempData["Message1"] = "Financial Year not Exist";
            //    //return RedirectToAction("PurchaseRequisition", PRLIST_Model);
            //}
            //if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PRDate) == "TransNotAllow")
            //{
            //    TempData["Message2"] = "TransNotAllow";
            //    ViewBag.Message = "TransNotAllow";
            //}

            /*End to chk Financial year exist or not*/
            PRLIST_Model.PR_Number = PRId;
            PRLIST_Model.Message = "New";
            PRLIST_Model.Command = "Add";
            PRLIST_Model.PR_Date = PRDate;
            PRLIST_Model.TransType = "Update";
            PRLIST_Model.AppStatus = "D";
            PRLIST_Model.BtnName = "BtnToDetailPage";
            PRLIST_Model.WF_status1 = WF_status;
            TempData["PRData"] = PRData;
            TempData["ModelData"] = PRLIST_Model;
            var PR_Number = PRId;
            var PR_Date = PRDate;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var Command = "Add";
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["PR_No"] = PRId;
            //Session["PR_Date"] = PRDate;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            return (RedirectToAction("AddPurchaseRequisitionDetail", new { PR_Number = PR_Number, PR_Date, TransType, BtnName, Command }));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PurchaseRiquisitionSave(PurchaseRequisition_Model purchaseRequisition_Model, string PR_No, string command)
        {
            try
            {
                var commCont = new CommonController(_Common_IServices);
                if (purchaseRequisition_Model.DeleteCommand == "Delete")
                {
                    command = "Delete";
                }
                switch (command)
                {
                    case "AddNew":
                        PurchaseRequisition_Model purchaseRequisition_ModelAddNew = new PurchaseRequisition_Model();
                        TempData["PRData"] = null;
                        purchaseRequisition_ModelAddNew.Message = null;
                        purchaseRequisition_ModelAddNew.AppStatus = "D";
                        purchaseRequisition_ModelAddNew.BtnName = "BtnAddNew";
                        purchaseRequisition_ModelAddNew.TransType = "Save";
                        purchaseRequisition_ModelAddNew.Command = "New";
                        TempData["ModelData"] = purchaseRequisition_ModelAddNew;
                      
                        /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                              if (!string.IsNullOrEmpty(purchaseRequisition_Model.PR_No))
                                return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                            else
                                purchaseRequisition_ModelAddNew.Command = "Refresh";
                            purchaseRequisition_ModelAddNew.TransType = "Refresh";
                            purchaseRequisition_ModelAddNew.BtnName = "Refresh";
                            purchaseRequisition_ModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = purchaseRequisition_ModelAddNew;
                            return RedirectToAction("AddPurchaseRequisitionDetail", "PurchaseRequisition", purchaseRequisition_ModelAddNew);
                        }
                        /*End to chk Financial year exist or not*/
                       
                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        return RedirectToAction("AddPurchaseRequisitionDetail", "PurchaseRequisition");

                    case "Edit":
                        /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();

                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        //}
                        /*Commented and modify by Hina sharma on 30-04-2025 to check Existing with previous year transaction*/
                        string PRDate = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                        string FResult = commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PRDate);
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PRDate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });

                        }
                        /*End to chk Financial year exist or not*/
                        var PR_Number = ""; 
                        var PR_Date = "";
                        var TransType = "";
                        var BtnName = "";
                        if (CheckRFQAgainstPR(purchaseRequisition_Model.PR_No, purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd")) == "Used")
                        {
                            //Session["Message"] = "Used";
                            purchaseRequisition_Model.Message = "Used";
                            purchaseRequisition_Model.PR_Number = purchaseRequisition_Model.PR_No;
                            purchaseRequisition_Model.TransType = "Update";
                            purchaseRequisition_Model.Command = "Refresh";
                            purchaseRequisition_Model.BtnName = "BtnToDetailPage";
                            TempData["PRData"] = purchaseRequisition_Model.PRData1;
                            TempData["ModelData"] = purchaseRequisition_Model;
                            PR_Number = purchaseRequisition_Model.PR_No;
                            PR_Date = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                            TransType = "Update";
                            BtnName = "BtnToDetailPage";
                            command = "Refresh";
                        }
                        else
                        {
                            purchaseRequisition_Model.PR_Number = purchaseRequisition_Model.PR_No;
                            purchaseRequisition_Model.TransType = "Update";
                            purchaseRequisition_Model.Command = command;
                            purchaseRequisition_Model.BtnName = "BtnEdit";
                            purchaseRequisition_Model.Message = null;
                            TempData["PRData"] = purchaseRequisition_Model.PRData1;
                            TempData["ModelData"] = purchaseRequisition_Model;
                            PR_Number = purchaseRequisition_Model.PR_No;
                            PR_Date = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                            TransType = "Update";
                            BtnName = "BtnEdit";
                            command = purchaseRequisition_Model.Command;
                        }
                        return (RedirectToAction("AddPurchaseRequisitionDetail", new { PR_Number = PR_Number, PR_Date, TransType, BtnName, command }));

                    case "Delete":
                        PurchaseRequisition_Model purchaseRequisition_ModelDelete = new PurchaseRequisition_Model();
                        purchaseRequisition_ModelDelete.Command = command;
                        purchaseRequisition_ModelDelete.BtnName = "Refresh";
                        PRDelete(purchaseRequisition_Model, command);
                        purchaseRequisition_ModelDelete.Message = "Deleted";
                        purchaseRequisition_ModelDelete.Command = "Refresh";
                        purchaseRequisition_ModelDelete.PR_No = "";
                        purchaseRequisition_ModelDelete.TransType = "Refresh";
                        purchaseRequisition_ModelDelete.AppStatus = "DL";
                        purchaseRequisition_ModelDelete.BtnName = "BtnDelete";
                        TempData["ModelData"] = purchaseRequisition_ModelDelete;
                        TempData["PRData"] = purchaseRequisition_Model.PRData1;
                        return RedirectToAction("AddPurchaseRequisitionDetail");
                    case "Save":
                        purchaseRequisition_Model.Command = command;
                        if (purchaseRequisition_Model.TransType == null)
                        {
                            purchaseRequisition_Model.TransType = command;
                        }
                        if (purchaseRequisition_Model.Amend != null && purchaseRequisition_Model.Amend != "" && purchaseRequisition_Model.Amendment == null)
                        {
                            purchaseRequisition_Model.TransType = "Amendment";

                        }
                        SavePRDetail(purchaseRequisition_Model);
                        if (purchaseRequisition_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        TempData["PRData"] = purchaseRequisition_Model.PRData1;
                        TempData["ModelData"] = purchaseRequisition_Model;
                        PR_Number = purchaseRequisition_Model.PR_Number;
                        PR_Date = purchaseRequisition_Model.PR_Date;
                        TransType = purchaseRequisition_Model.TransType;
                        BtnName = purchaseRequisition_Model.BtnName;
                        return (RedirectToAction("AddPurchaseRequisitionDetail", new { PR_Number = PR_Number, PR_Date, TransType, BtnName, command }));

                    case "Forward":
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 30-04-2025 to check Existing with previous year transaction*/
                        string PRDate1 = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PRDate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        }
                        return new EmptyResult();

                    case "Approve":
                        /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 30-04-2025 to check Existing with previous year transaction*/
                        string PRDate2 = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PRDate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPR", new { PRId = purchaseRequisition_Model.PR_No, PRDate = purchaseRequisition_Model.Req_date, PRData = purchaseRequisition_Model.PRData1, WF_status = purchaseRequisition_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        purchaseRequisition_Model.Command = command;
                        PR_No = purchaseRequisition_Model.PR_No;
                        purchaseRequisition_Model.PR_No = PR_No;
                        PRListApprove(purchaseRequisition_Model, PR_No, purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd"), purchaseRequisition_Model.SourceType, "", "", "", "", "");
                        PR_Number = purchaseRequisition_Model.PR_Number;
                        PR_Date = purchaseRequisition_Model.PR_Date;
                        TransType = purchaseRequisition_Model.TransType;
                        BtnName = purchaseRequisition_Model.BtnName;
                        TempData["ModelData"] = purchaseRequisition_Model;
                        TempData["PRData"] = purchaseRequisition_Model.PRData1;
                        return (RedirectToAction("AddPurchaseRequisitionDetail", new { PR_Number = PR_Number, PR_Date, TransType, BtnName, command }));
                    case "Refresh":
                        PurchaseRequisition_Model purchaseRequisition_ModelRefresh = new PurchaseRequisition_Model();
                        purchaseRequisition_ModelRefresh.BtnName = "Refresh";
                        purchaseRequisition_ModelRefresh.Command = command;
                        purchaseRequisition_ModelRefresh.TransType = "Save";
                        purchaseRequisition_ModelRefresh.Message = null;
                        TempData["ModelData"] = purchaseRequisition_ModelRefresh;
                        purchaseRequisition_Model.DocumentStatus = null;
                        return RedirectToAction("AddPurchaseRequisitionDetail");

                    case "Print":
                        return GenratePdfFile(purchaseRequisition_Model);
                    case "BacktoList":
                        TempData["PRData"] = purchaseRequisition_Model.PRData1;
                        TempData["WF_status"] = purchaseRequisition_Model.WF_status1;
                        return RedirectToAction("PurchaseRequisition", "PurchaseRequisition");

                    case "Amendment":
                        /*start Add by Hina on 08-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    purchaseRequisition_Model.PR_Date = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditPR", new { DocNo = purchaseRequisition_Model.PR_No, DocDate = purchaseRequisition_Model.PR_Date, ListFilterData = purchaseRequisition_Model.PRData1, DocumentMenuId = purchaseRequisition_Model.DocumentMenuId, WF_status = purchaseRequisition_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 30-04-2025 to check Existing with previous year transaction*/
                        string PRDate3 = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, PRDate3) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditPR", new { DocNo = purchaseRequisition_Model.PR_No, DocDate = purchaseRequisition_Model.PR_Date, ListFilterData = purchaseRequisition_Model.PRData1, DocumentMenuId = purchaseRequisition_Model.DocumentMenuId, WF_status = purchaseRequisition_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //  URLDetailModel URLModelAmendment = new URLDetailModel();
                        purchaseRequisition_Model.Command = "Edit";
                        purchaseRequisition_Model.TransType = "Update";
                        purchaseRequisition_Model.BtnName = "BtnEdit";
                        purchaseRequisition_Model.Amend = "Amend";
                        purchaseRequisition_Model.PR_Number = purchaseRequisition_Model.PR_No;
                        purchaseRequisition_Model.PR_Date = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                     
                        TempData["PRData"] = purchaseRequisition_Model.PRData1;
                        TempData["ModelData"] = purchaseRequisition_Model;
                     
                        PR_Number = purchaseRequisition_Model.PR_No;
                        PR_Date = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                        TransType = "Update";
                        BtnName = "BtnEdit";
                        command = purchaseRequisition_Model.Command;
                        return RedirectToAction("AddPurchaseRequisitionDetail", new { PR_Number = PR_Number, PR_Date, TransType, BtnName, command });

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
        public ActionResult SavePRDetail(PurchaseRequisition_Model purchaseRequisition_Model)
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
                branchID = Session["BranchId"].ToString();
            }
            try
            {

                if (purchaseRequisition_Model.ForceClose != false)
                {



                    purchaseRequisition_Model.CreatedBy = userid;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    SaveMessage = purchaseRequisition_ISERVICES.PRForceClose(purchaseRequisition_Model, CompID, branchID, mac_id);
                    string MRSNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                    _Common_IServices.SendAlertEmail(CompID, branchID, DocumentMenuId, purchaseRequisition_Model.PR_No, "FC", userid, "");
                    purchaseRequisition_Model.Message = "Save";
                    purchaseRequisition_Model.Command = "Update";
                    purchaseRequisition_Model.TransType = "Update";
                    purchaseRequisition_Model.AppStatus = "D";
                    purchaseRequisition_Model.BtnName = "Refresh";
                    TempData["PR_No"] = purchaseRequisition_Model.PR_No;
                    TempData["PRDate"] = purchaseRequisition_Model.Req_date;
                    //Session["Message"] = "Save";
                    //Session["Command"] = "Update";
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    return RedirectToAction("AddPurchaseRequisitionDetail");
                }
                else
                {
                    if (purchaseRequisition_Model.CancelFlag == false)
                    {
                        DataTable PRHeader = new DataTable();
                        DataTable PRItemDetails = new DataTable();
                        DataTable Attachments = new DataTable();
                        DataTable dtheader = new DataTable();

                        dtheader.Columns.Add("TransType", typeof(string));
                        dtheader.Columns.Add("MenuDocumentId", typeof(string));
                        dtheader.Columns.Add("comp_id", typeof(int));
                        dtheader.Columns.Add("br_id", typeof(int));
                        dtheader.Columns.Add("user_id", typeof(int));
                        dtheader.Columns.Add("pr_dt", typeof(DateTime));
                        dtheader.Columns.Add("pr_no", typeof(string));
                        dtheader.Columns.Add("pr_type", typeof(string));
                        dtheader.Columns.Add("req_area", typeof(int));
                        dtheader.Columns.Add("req_by", typeof(string));
                        dtheader.Columns.Add("pr_rem", typeof(string));
                        dtheader.Columns.Add("pr_status", typeof(string));
                        dtheader.Columns.Add("mac_id", typeof(string));

                        DataRow dtrowHeader = dtheader.NewRow();
                        //dtrowHeader["TransType"] = Session["TransType"].ToString();
                        dtrowHeader["TransType"] = purchaseRequisition_Model.TransType;
                        dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                        dtrowHeader["comp_id"] = Session["CompId"].ToString();
                        dtrowHeader["br_id"] = Session["BranchId"].ToString();
                        dtrowHeader["user_id"] = Session["UserId"].ToString();
                        dtrowHeader["pr_dt"] = purchaseRequisition_Model.Req_date;
                        dtrowHeader["pr_no"] = purchaseRequisition_Model.PR_No;
                        dtrowHeader["pr_type"] = purchaseRequisition_Model.SourceType;
                        dtrowHeader["req_area"] = purchaseRequisition_Model.req_area;
                        dtrowHeader["req_by"] = purchaseRequisition_Model.Req_By;
                        dtrowHeader["pr_rem"] = purchaseRequisition_Model.Remarks;
                        dtrowHeader["pr_status"] = "D";
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        dtrowHeader["mac_id"] = mac_id;


                        dtheader.Rows.Add(dtrowHeader);
                        PRHeader = dtheader;


                        DataTable dtItem = new DataTable();

                        dtItem.Columns.Add("item_id", typeof(string));
                        dtItem.Columns.Add("uom_id", typeof(int));
                        dtItem.Columns.Add("pr_qty", typeof(string));
                        dtItem.Columns.Add("po_qty", typeof(string));
                        dtItem.Columns.Add("it_remarks", typeof(string));
                        dtItem.Columns.Add("min_stk_lvl", typeof(string));

                        JArray jObject = JArray.Parse(purchaseRequisition_Model.Itemdetails);

                        for (int i = 0; i < jObject.Count; i++)
                        {
                            DataRow dtrowLines = dtItem.NewRow();

                            dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                            dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                            dtrowLines["pr_qty"] = jObject[i]["RequQty"].ToString();
                            dtrowLines["po_qty"] = jObject[i]["OrederQty"].ToString();
                            dtrowLines["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                            dtrowLines["min_stk_lvl"] = jObject[i]["min_stk_lvl"].ToString();
                            dtItem.Rows.Add(dtrowLines);
                        }
                        PRItemDetails = dtItem;
                        /*----------------------Sub Item ----------------------*/
                        DataTable dtSubItem = new DataTable();
                        dtSubItem.Columns.Add("item_id", typeof(string));
                        dtSubItem.Columns.Add("sub_item_id", typeof(string));
                        dtSubItem.Columns.Add("qty", typeof(string));
                        if (purchaseRequisition_Model.SubItemDetailsDt != null)
                        {
                            JArray jObject2 = JArray.Parse(purchaseRequisition_Model.SubItemDetailsDt);
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
                        DataTable dtAttachment = new DataTable();
                        var _PurchaseRequisitionattch = TempData["ModelDataattch"] as PurchaseRequisitionattch;
                        TempData["ModelDataattch"] = null;
                        if (purchaseRequisition_Model.attatchmentdetail != null)
                        {
                            if (_PurchaseRequisitionattch != null)
                            {
                                if (_PurchaseRequisitionattch.AttachMentDetailItmStp != null)
                                {
                                    dtAttachment = _PurchaseRequisitionattch.AttachMentDetailItmStp as DataTable;
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
                                if (purchaseRequisition_Model.AttachMentDetailItmStp != null)
                                {
                                    dtAttachment = purchaseRequisition_Model.AttachMentDetailItmStp as DataTable;
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
                            JArray jObject1 = JArray.Parse(purchaseRequisition_Model.attatchmentdetail);
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
                                    if (!string.IsNullOrEmpty(purchaseRequisition_Model.PR_No))
                                    {
                                        dtrowAttachment1["id"] = purchaseRequisition_Model.PR_No;
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
                            //if (Session["TransType"].ToString() == "Update")
                            if (purchaseRequisition_Model.TransType == "Update")
                            {

                                string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                                if (Directory.Exists(AttachmentFilePath))
                                {
                                    string ItmCode = string.Empty;
                                    if (!string.IsNullOrEmpty(purchaseRequisition_Model.PR_No))
                                    {
                                        ItmCode = purchaseRequisition_Model.PR_No;
                                    }
                                    else
                                    {
                                        ItmCode = "0";
                                    }
                                    string[] filePaths = Directory.GetFiles(AttachmentFilePath, CompID + branchID + ItmCode.Replace("/", "") + "*");
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
                        SaveMessage = purchaseRequisition_ISERVICES.InsertUpdatePR(PRHeader, PRItemDetails, dtSubItem, Attachments);
                        string[] Data = SaveMessage.Split(',');

                        string PRNo = Data[1];
                        string Message = Data[0];
                        string PRDate = Data[2];
                        if (Message == "DataNotFound")
                        {

                            var a = PRNo.Split(',');
                            var msg = "Data Not found" + " " + a[0].Trim() + " in " + PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            purchaseRequisition_Model.Message = Message;
                            return RedirectToAction("AddPurchaseRequisitionDetail");
                        }
                        if (Message == "Save")
                        {
                            string Guid = "";
                            //if (Session["Guid"] != null)
                            if (_PurchaseRequisitionattch != null)
                            {
                                if (_PurchaseRequisitionattch.Guid != null)
                                {
                                    //Guid = Session["Guid"].ToString();
                                    Guid = _PurchaseRequisitionattch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, branchID, guid, PageName, PRNo, purchaseRequisition_Model.TransType, Attachments);

                            //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //if (Directory.Exists(sourcePath))
                            //{
                            //    string[] filePaths = Directory.GetFiles(sourcePath, CompID+ branchID + Guid + "_" + "*");
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
                            //                string PRNo1 = PRNo.Replace("/", "");
                            //                string img_nm = CompID + branchID + PRNo1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                            purchaseRequisition_Model.Message = "Save";
                            purchaseRequisition_Model.Command = "Update";
                            purchaseRequisition_Model.PR_No = PRNo;
                            purchaseRequisition_Model.PR_Date = PRDate;
                            purchaseRequisition_Model.PR_Number = PRNo;
                            purchaseRequisition_Model.TransType = "Update";
                            purchaseRequisition_Model.AppStatus = "D";
                            purchaseRequisition_Model.BtnName = "BtnSave";
                            //Session["AttachMentDetailItmStp"] = null;
                            //Session["Guid"] = null;
                            purchaseRequisition_Model.AttachMentDetailItmStp = null;
                            purchaseRequisition_Model.Guid = null;
                            //Session["Message"] = "Save";
                            //Session["Command"] = "Update";
                            //Session["PR_No"] = PRNo;
                            //Session["PR_Date"] = PRDate;
                            //Session["TransType"] = "Update";
                            //Session["AppStatus"] = 'D';
                            //Session["BtnName"] = "BtnSave";
                            //Session["AttachMentDetailItmStp"] = null;
                            //Session["Guid"] = null;
                        }
                        return RedirectToAction("AddPurchaseRequisitionDetail");
                    }
                    else
                    {

                        purchaseRequisition_Model.CreatedBy = userid;
                        string mac = Session["UserMacaddress"].ToString();
                        string system = Session["UserSystemName"].ToString();
                        string ip = Session["UserIP"].ToString();
                        string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                        SaveMessage = purchaseRequisition_ISERVICES.PRCancel(purchaseRequisition_Model, CompID, branchID, mac_id);
                        string MRSNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                        try
                        {
                            //string fileName = "PR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            string fileName = purchaseRequisition_Model.PR_No.Replace("/", "") +"_"+ System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                            var filePath = SavePdfDocToSendOnEmailAlert(purchaseRequisition_Model.PR_No, purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd"), fileName, DocumentMenuId,"C");
                            _Common_IServices.SendAlertEmail(CompID, branchID, DocumentMenuId, purchaseRequisition_Model.PR_No, "C", userid, "", filePath);
                        }
                        catch (Exception exMail)
                        {
                            purchaseRequisition_Model.Message = "ErrorInMail";
                            string path = Server.MapPath("~");
                            Errorlog.LogError(path, exMail);
                        }
                        purchaseRequisition_Model.Message = purchaseRequisition_Model.Message == "ErrorInMail" ? "Cancelled_ErrorInMail" : "Cancelled";
                        //purchaseRequisition_Model.Message = "Cancelled";
                        purchaseRequisition_Model.Command = "Update";
                        purchaseRequisition_Model.TransType = "Update";
                        purchaseRequisition_Model.AppStatus = "D";
                        purchaseRequisition_Model.BtnName = "Refresh";
                        purchaseRequisition_Model.PR_Number = purchaseRequisition_Model.PR_No;
                        purchaseRequisition_Model.PR_Date = purchaseRequisition_Model.Req_date.ToString("yyyy-MM-dd");
                        TempData["PR_No"] = purchaseRequisition_Model.PR_No;
                        TempData["PRDate"] = purchaseRequisition_Model.Req_date;
                        //Session["Message"] = "Cancelled";
                        //Session["Command"] = "Update";
                        //Session["TransType"] = "Update";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "Refresh";
                        return RedirectToAction("AddPurchaseRequisitionDetail");
                    }
                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (purchaseRequisition_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (purchaseRequisition_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = purchaseRequisition_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID + branchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public JsonResult Upload(string title, string DocNo, string TransType)
        {
            try
            {
                PurchaseRequisitionattch _PurchaseRequisitionattch = new PurchaseRequisitionattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;
                //string TransType = "";
                //string PRNo = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["PR_No"] != null)
                //{
                //    PRNo = Session["PR_No"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                //Session["Guid"] = PRNo;
                _PurchaseRequisitionattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _PurchaseRequisitionattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _PurchaseRequisitionattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _PurchaseRequisitionattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        private ActionResult PRDelete(PurchaseRequisition_Model _PRModel, string command)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                string br_id = Session["BranchId"].ToString();
                string doc_no = _PRModel.PR_No;
                string Message = purchaseRequisition_ISERVICES.PRDelete(_PRModel, CompID, branchID, DocumentMenuId);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string doc_no1 = doc_no.Replace("/", "");
                    other.DeleteTempFile(CompID + br_id, PageName, doc_no1, Server);
                }
                _PRModel.Message = "Deleted";
                _PRModel.Command = "Refresh";
                _PRModel.PR_No = "";
                _PRModel.TransType = "Refresh";
                _PRModel.AppStatus = "DL";
                _PRModel.BtnName = "BtnDelete";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["PR_No"] = "";
                //_PRModel = null;
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "BtnDelete";
                return RedirectToAction("AddPurchaseRequisitionDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public ActionResult PRListApprove(PurchaseRequisition_Model _PRModel, string PR_No, string PR_Date, string SrcType, string A_Status, string A_Level, string A_Remarks, string PRData, string WF_status1)
        {
            try

            {
                string Comp_ID = string.Empty;
                string UserID = string.Empty;
                string BranchID = string.Empty;
                string MenuDocId = string.Empty;
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
                    BranchID = Session["BranchId"].ToString();
                }
                if (Session["MenuDocumentId"] != null)
                {
                    MenuDocId = DocumentMenuId;
                }
                //PurchaseRequisition_Model _PRModel = new PurchaseRequisition_Model();
                _PRModel.CreatedBy = Session["UserId"].ToString();
                _PRModel.PR_No = PR_No;
                Session["PRDate"] = PR_Date;
                _PRModel.SourceType = SrcType;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = purchaseRequisition_ISERVICES.PRListApprove(_PRModel, CompID, BranchID, PR_Date, SrcType, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId);
                string ApMessage = Message.Split(',')[0].Trim();
                string PRNo = Message.Split(',')[1].Trim();
                string PRDate = Message.Split(',')[2].Trim();
                if (ApMessage == "A")
                {
                    try
                    {
                        //string fileName = "PR_" + System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        string fileName = PRNo.Replace("/", "") +"_"+ System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".pdf";
                        var filePath = SavePdfDocToSendOnEmailAlert(PRNo, PRDate, fileName, DocumentMenuId, "AP");
                        _Common_IServices.SendAlertEmail(CompID, BranchID, DocumentMenuId, _PRModel.PR_No, "AP", UserID, "", filePath);
                    }
                    catch (Exception exMail)
                    {
                        _PRModel.Message = "ErrorInMail";
                        string path = Server.MapPath("~");
                        Errorlog.LogError(path, exMail);
                    }
                    _PRModel.Message = _PRModel.Message == "ErrorInMail" ? "Approved_ErrorInMail" : "Approved";
                }
                _PRModel.TransType = "Update";
                _PRModel.Command = "Approve";
                _PRModel.PR_Number = _PRModel.PR_No;
                _PRModel.PR_Date = PRDate;
                _PRModel.AppStatus = "D";
                _PRModel.BtnName = "BtnEdit";
                TempData["ModelData"] = _PRModel;
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["PRNo"] = _PRModel.PR_No;
                //Session["PRDate"] = PRDate;
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                TempData["PRData"] = PRData;
                TempData["WF_status1"] = WF_status1;
                return RedirectToAction("AddPurchaseRequisitionDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                // return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        public ActionResult PurchaseRequisitionDetail()
        {
            try
            {
                ViewBag.DocumentMenuId = DocumentMenuId;
                PurchaseRequisition_Model purchaseRequisition_Model = new PurchaseRequisition_Model();
                DataTable dt = new DataTable();
                List<RequirementAreaList> requirementAreaLists = new List<RequirementAreaList>();
                dt = GetRequirmentreaList();
                foreach (DataRow dr in dt.Rows)
                {
                    RequirementAreaList _RAList = new RequirementAreaList();
                    _RAList.req_id = Convert.ToInt32(dr["setup_id"]);
                    _RAList.req_val = dr["setup_val"].ToString();
                    requirementAreaLists.Add(_RAList);
                }
                requirementAreaLists.Insert(0, new RequirementAreaList() { req_id = 0, req_val = "All" });
                purchaseRequisition_Model._requirementAreaLists = requirementAreaLists;

                //Session["TransType"] = "Save";
                purchaseRequisition_Model.TransType = "Save";
                ViewBag.MenuPageName = getDocumentName();
                purchaseRequisition_Model.Title = title;
                return View("~/Areas/ApplicationLayer/Views/Procurement/PurchaseRequisition/PurchaseRequisition.cshtml", purchaseRequisition_Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FileResult GenratePdfFile(PurchaseRequisition_Model _Model)
        {
            return File(GetPdfData(_Model.PR_No, _Model.Req_date.ToString("yyyy-MM-dd")), "application/pdf", "PurchaseRequisition.pdf");
        }
        public byte[] GetPdfData(string PR_No, string PR_Date)
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
                    branchID = Session["BranchId"].ToString();
                }
                DataSet Deatils = purchaseRequisition_ISERVICES.GetPurchaseRequisitionDeatils(CompID, branchID, PR_No, PR_Date);
                ViewBag.PageName = "PR";
                ViewBag.Title = "Purchase Requisition";
                ViewBag.Details = Deatils;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                //ViewBag.InvoiceTo = "Invoice to:";
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["pr_status"].ToString().Trim();
                ViewBag.Website = Deatils.Tables[0].Rows[0]["comp_website"].ToString();/*Add by hina on 04-04-2025*/
                string htmlcontent = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/PurchaseRequisition/PurchaseRequisitionprint.cshtml"));
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    reader = new StringReader(htmlcontent);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 20f);
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

        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
   , string Flag, string Status, string Doc_no, string Doc_dt ,string Amendbtn)
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                DataTable dt = new DataTable();
                int QtyDigit = Convert.ToInt32(Session["QtyDigit"]);
                if (Flag == "Quantity")
                {
                    if (Status == "D" || Status == "F" || Status == "" || Amendbtn == "Amend")
                    {
                        dt = _Common_IServices.GetSubItemDetails(CompID, Item_id).Tables[0];
                        JArray arr = JArray.Parse(SubItemListwithPageData);
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            foreach (JObject item in arr.Children())//
                            {
                                if (item.GetValue("item_id").ToString() == dt.Rows[i]["item_id"].ToString() && item.GetValue("sub_item_id").ToString() == dt.Rows[i]["sub_item_id"].ToString())
                                {
                                    dt.Rows[i]["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                                }
                            }
                        }
                    }
                    else
                    {
                        dt = purchaseRequisition_ISERVICES.PR_GetSubItemDetails(CompID, branchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    }
                }
                else if (Flag == "AvlStock")
                {
                    dt = purchaseRequisition_ISERVICES.PR_GetSubItemDetails(CompID, branchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    ViewBag.SubitemAvlStockDetail = dt;
                    Flag = "WH";
                    ViewBag.Flag = "WH";
                }
                else
                {
                    dt = purchaseRequisition_ISERVICES.PR_GetSubItemDetails(CompID, branchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];

                }

                if (Flag == "WH")
                {
                    ViewBag.DocumentMenuId = DocumentMenuId;                   
                        ViewBag.Flag = "WH";                   
                    return PartialView("~/Areas/Common/Views/Cmn_PartialSubItemStkDetail.cshtml");
                }
                else
                {
                    SubItemPopupDt subitmModel = new SubItemPopupDt
                    {

                        Flag = Flag == "Quantity" ? Flag : "PR",
                        _subitemPageName = "PR",
                        dt_SubItemDetails = dt,
                        IsDisabled = IsDisabled,
                        decimalAllowed = "Y"

                    };

                    //ViewBag.SubItemDetails = dt;
                    //ViewBag.IsDisabled = IsDisabled;
                    //ViewBag.Flag = Flag == "Quantity" ? Flag : "MTO";
                    return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        private string IsNull(string Str, string Str2)
        {
            if (string.IsNullOrEmpty(Str))
                Str = Str2;
            return Str;
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
        public ActionResult ToRefreshByJS(string PRData, string TrancType, string Mailerror)
        {
            //Session["Message"] = "";
            PurchaseRequisition_Model PRLIST_Model = new PurchaseRequisition_Model();
            var a = TrancType.Split(',');
            PRLIST_Model.PR_Number = a[0].Trim();
            PRLIST_Model.PR_Date = a[1].Trim();
            PRLIST_Model.TransType = a[2].Trim();
            var WF_status1 = a[3].Trim();
            PRLIST_Model.Message = Mailerror;
            PRLIST_Model.BtnName = "BtnToDetailPage";
            TempData["ModelData"] = PRLIST_Model;
            TempData["PRData"] = PRData;
            TempData["WF_status1"] = WF_status1;
            return RedirectToAction("AddPurchaseRequisitionDetail");
        }
        [NonAction]
        private DataTable GetRequirmentreaList()
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
                DataTable dt = purchaseRequisition_ISERVICES.GetRequirmentreaList(CompID, BrchID);
                return dt;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
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
        /*------------- Added by Nidhi 20-05-2025 11:44 ----------*/
        public string SavePdfDocToSendOnEmailAlert(string Doc_no, string Doc_dt, string fileName, string docid, string docstatus)
        {
            if (Session["CompId"] != null)
            {
                CompID = Session["CompId"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                branchID = Session["BranchId"].ToString();
            }
            var commonCont = new CommonController(_Common_IServices);
            try
            {
                string mailattch = commonCont.CheckMailAttch(CompID, branchID, docid, docstatus);
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
        /*-------------- END  20-05-2025 11:44 ---------------*/

        [HttpPost]
        public ActionResult GetDataReorderLevel()
        {
            try
            {
                string CompID = Session["CompId"]?.ToString() ?? "";
                string BrchID = Session["BranchId"]?.ToString() ?? "";

                // Service call returns DataSet
                DataSet ds = purchaseRequisition_ISERVICES.GetDataReorderLevel(CompID, BrchID);

                // Direct DataTable return as JSON
                if (ds != null && ds.Tables.Count > 0)
                {
                   //  Json(ds.Tables[0], JsonRequestBehavior.AllowGet);
                    return Json(JsonConvert.SerializeObject(ds.Tables[0]));
                }

                // Agar koi data nahi hai
                return Json(ds, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

    }
}