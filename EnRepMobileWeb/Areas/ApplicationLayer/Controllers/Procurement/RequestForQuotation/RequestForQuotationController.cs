using EnRepMobileWeb.Areas.Common.Controllers.Common;
using EnRepMobileWeb.MODELS.ApplicationLayer.Procurement.RequestForQuotation;
using EnRepMobileWeb.MODELS.Common;
using EnRepMobileWeb.SERVICES.ISERVICES.Application_Layer.Procurement.RequestForQuotation;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.ApplicationLayer.Controllers.Procurement.RequestForQuotation
{
    public class RequestForQuotationController : Controller
    {
        string CompID, branchID, userid, UserID, language = String.Empty, rfq_no;
        string DocumentMenuId = "105101115", title;
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        RequestForQuotation_ISERVICES requestForQuotation_ISERVICES;
        public RequestForQuotationController(Common_IServices _Common_IServices, RequestForQuotation_ISERVICES requestForQuotation_ISERVICES)
        {
            this._Common_IServices = _Common_IServices;
            this.requestForQuotation_ISERVICES = requestForQuotation_ISERVICES;
        }
        // GET: ApplicationLayer/RequestForQuotation
        public ActionResult RequestForQuotation(RequestForQuotation_Model RFQ_Model)
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
            var other = new CommonController(_Common_IServices);
           // ViewBag.AppLevel = other.GetApprovalLevel(CompID, branchID, DocumentMenuId);
            //RequestForQuotation_Model RFQ_Model = new RequestForQuotation_Model();
            //ViewBag.MenuPageName = getDocumentName();
            ViewBag.DocumentMenuId = DocumentMenuId;

            List<StatusLists> statusLists = new List<StatusLists>();
            if (ViewBag.StatusList.Rows.Count > 0)
            {
                foreach (DataRow data in ViewBag.StatusList.Rows)
                {
                    StatusLists _Statuslist = new StatusLists();
                    _Statuslist.status_id = data["status_code"].ToString();
                    _Statuslist.status_name = data["status_name"].ToString();
                    statusLists.Add(_Statuslist);
                }
            }

            RFQ_Model.statusLists = statusLists;
            //List<StatusLists> statusLists = new List<StatusLists>();
            //var statusListsC = other.GetStatusList1(DocumentMenuId);
            //var listOfStatus = statusListsC.ConvertAll(x => new StatusLists { status_id = x.status_id, status_name = x.status_name });
            //RFQ_Model.statusLists = listOfStatus;

            RFQ_Model.Title = title;
            //RFQ_Model.Message = "New";
            RFQ_Model.TransType = "Save";
            RFQ_Model.BtnName = "BtnAddNew";
            RFQ_Model.Command = "Add";
            RFQ_Model.DocumentStatus = "D";
            ViewBag.DocumentStatus = "D";
            //Session["Message"] = "New";
            //Session["TransType"] = "Save";
            //Session["BtnName"] = "BtnAddNew";
            //Session["Command"] = "Add";
            //Session["DocumentStatus"] = "D";
            string wfstatus = "";
            if (TempData["WF_status"] != null && TempData["WF_status"].ToString() != "")
            {
                RFQ_Model.WF_status = TempData["WF_status"].ToString();
                //if (Session["WF_status"] != null)
                if (RFQ_Model.WF_status != null)
                {
                    wfstatus = RFQ_Model.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
            }
            else
            {
                //if (Session["WF_status"] != null)
                if (RFQ_Model.WF_status != null)
                {
                    wfstatus = RFQ_Model.WF_status;
                }
                else
                {
                    wfstatus = "";
                }
            }
            //DateTime dtnow = DateTime.Now;
            //string startDate = new DateTime(dtnow.Year, dtnow.Month, 1).ToString("yyyy-MM-dd");

            var range = CommonController.Comman_GetFutureDateRange();
            string startDate = range.FromDate;
            string CurrentDate = range.ToDate;
            if (TempData["FilterData"] != null && TempData["FilterData"].ToString() != "")
            {
                var PRData = TempData["FilterData"].ToString();
                if (PRData != null && PRData != "")
                {
                    var a = PRData.Split(',');
                    //var req_area =
                    RFQ_Model.RFQ_FromDate = a[0].Trim();
                    RFQ_Model.RFQ_ToDate = a[1].Trim();
                    RFQ_Model.RFQ_status = a[2].Trim();
                    if (RFQ_Model.RFQ_status == "0")
                    {
                        RFQ_Model.RFQ_status = null;
                    }
                    RFQ_Model.PRData = TempData["FilterData"].ToString();
                }
                else
                {
                    RFQ_Model.RFQ_FromDate = startDate;
                    RFQ_Model.RFQ_ToDate = CurrentDate;
                }
            }
            else
            {
                RFQ_Model.RFQ_FromDate = startDate;
                RFQ_Model.RFQ_ToDate = CurrentDate;
            }
            DataSet dt1 = requestForQuotation_ISERVICES.GetRFQDetailList(rfq_no,CompID, branchID, RFQ_Model.RFQ_FromDate, RFQ_Model.RFQ_ToDate, RFQ_Model.RFQ_status, UserID, wfstatus, DocumentMenuId);
            ViewBag.RFQDetailsList = dt1.Tables[0];
            ViewBag.AttechmentDetails = dt1.Tables[2];
            //RFQ_Model.RFQ_FromDate = startDate;// dt1.Tables[1].Rows[0]["finstrdate"].ToString();
            RFQ_Model.RFQSearch = "0";
            //Session["RFQSearch"] = "0";
          //  ViewBag.VBRoleList = GetRoleList();
            return View("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationList.cshtml", RFQ_Model);
        }
        private void CommonPageDetails()
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
        public ActionResult SearchRFQDetail(string Fromdate, string Todate, string Status)
        {
            try
            {
                RequestForQuotation_Model RFQ_Model = new RequestForQuotation_Model();
                Session["WF_status"] = "";
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }

                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    UserID = Session["userid"].ToString();
                }
                DataTable dt1 = requestForQuotation_ISERVICES.GetRFQDetailList(rfq_no, CompID, branchID, Fromdate, Todate, Status, UserID,"", DocumentMenuId).Tables[0];
                ViewBag.RFQDetailsList = dt1;
                RFQ_Model.RFQSearch = "RFQ_Search";
                return PartialView("~/Areas/ApplicationLayer/Views/Shared/PartialRequestForQuotation.cshtml", RFQ_Model);
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
            RequestForQuotation_Model RFQ_Model = new RequestForQuotation_Model();
            /*start Add by Hina on 06-02-2024 to chk Financial year exist or not*/
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                branchID = Session["BranchId"].ToString();
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
            {
                TempData["Message"] = "Financial Year not Exist";
                RFQ_Model.Message = "Financial Year not Exist";
                return RedirectToAction("RequestForQuotation", RFQ_Model);
            }
            /*End to chk Financial year exist or not*/
            RFQ_Model.TransType = "Save";
            RFQ_Model.Command = "New";
            RFQ_Model.Message = "New";
            RFQ_Model.BtnName = "BtnAddNew";
            RFQ_Model.DocumentStatus = "D";
            ViewBag.DocumentStatus= "D";
            TempData["ModelData"] = RFQ_Model;
            TempData["FilterData"] = null;

            //Session["TransType"] = "Save";
            //Session["Command"] = "New";
            //Session["Message"] = "New";
            //Session["BtnName"] = "BtnAddNew";
            //Session["DocumentStatus"] = "D";         
            return RedirectToAction("AddRequestForQuotationDetail");
        }
        public ActionResult EditRFQ(string RFQId,string RFQDate, string PRData, string WF_status)
        {
            RequestForQuotation_Model RFQ_Model = new RequestForQuotation_Model();
            /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
           
            if (Session["CompId"] != null)
                CompID = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                branchID = Session["BranchId"].ToString();
            //var commCont = new CommonController(_Common_IServices);
            //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
            //{
            //    TempData["Message1"] = "Financial Year not Exist";
            //}
            /*End to chk Financial year exist or not*/
            RFQ_Model.Message = "New";
            RFQ_Model.Command = "Add";
            RFQ_Model.RFQ_Number = RFQId;
            RFQ_Model.RFQ_date = RFQDate;
            RFQ_Model.TransType = "Update";
            RFQ_Model.AppStatus = "D";
            RFQ_Model.BtnName = "BtnToDetailPage";
            RFQ_Model.WF_status1 = WF_status;
            TempData["ModelData"] = RFQ_Model;
            TempData["FilterData"] = PRData;
            var RFQ_Number = RFQId;
            var RFQ_Date = RFQDate;
            var TransType = "Update";
            var BtnName = "BtnToDetailPage";
            var Command = "Add";
            //Session["Message"] = "New";
            //Session["Command"] = "Add";
            //Session["RFQ_no"] = RFQId;
            //Session["TransType"] = "Update";
            //Session["AppStatus"] = 'D';
            //Session["BtnName"] = "BtnToDetailPage";
            return ( RedirectToAction("AddRequestForQuotationDetail", new { RFQ_Number = RFQ_Number, RFQ_Date= RFQ_Date, TransType, BtnName, Command }));
        }
        public ActionResult GetRFQListDashboard(string docid, string status)
        {
            RequestForQuotation_Model RFQ_Model = new RequestForQuotation_Model();
            //Session["WF_status"] = status;
            RFQ_Model.WF_status = status;
            return RedirectToAction("RequestForQuotation", RFQ_Model);
        }
        public ActionResult AddRequestForQuotationDetail(RequestForQuotation_Model RFQ_Model1,string RFQ_Number,string RFQ_Date, string TransType,string BtnName,string Command)
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
            /*Add by Hina sharma on 06-05-2025 to check Existing with previous year transaction*/
            var commCont = new CommonController(_Common_IServices);
            if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, RFQ_Model1.RFQ_date) == "TransNotAllow")
            {
                //TempData["Message2"] = "TransNotAllow";
                ViewBag.Message = "TransNotAllow";
            }
            CommonPageDetails();
            try
            {
                var RFQ_Model = TempData["ModelData"] as RequestForQuotation_Model;
                if (RFQ_Model != null)
                {
                    // var other = new CommonController(_Common_IServices); /**Commented By Nitesh**/
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    //ViewBag.AppLevel = other.GetApprovalLevel(CompID, branchID, DocumentMenuId); /**Commented By Nitesh**/
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    //Session["AppStatus"] = 'D';
                    RFQ_Model.AppStatus = "D";
                   // ViewBag.MenuPageName = getDocumentName();
                    RFQ_Model.Title = title;
                    RFQ_Model.DocumentStatus = "D";
                    ViewBag.DocumentStatus = "D";
                    DataSet ds = GetSuppList();

                    List<supplist> _suppListD = new List<supplist>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        supplist supplist = new supplist();
                        supplist.Supp_id = dr["supp_id"].ToString();
                        supplist.Supp_Name = dr["supp_name"].ToString();
                        _suppListD.Add(supplist);
                    }
                    _suppListD.Insert(0, new supplist() { Supp_id = "0", Supp_Name = "---Select---" });
                    RFQ_Model._supplist = _suppListD;
                    List<PRReqList> pRReqs = new List<PRReqList>();
                    PRReqList pRReqList = new PRReqList();
                    pRReqList.PrDt = "";
                    pRReqList.PrNo = "---Select---";
                    pRReqs.Add(pRReqList);
                    RFQ_Model.pRReqLists = pRReqs;
                    if (TempData["FilterData"] != null && TempData["FilterData"].ToString() != "")
                    {
                        RFQ_Model.PRData1 = TempData["FilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        RFQ_Model.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (RFQ_Model.TransType == "Update" || RFQ_Model.TransType == "Edit")
                    {
                        //if (Session["CompId"] != null)
                        //{
                        //    CompID = Session["CompId"].ToString();
                        //}
                        //branchID = Session["BranchId"].ToString();
                        //string rfq_no = Session["RFQ_no"].ToString();
                        string rfq_no = RFQ_Model.RFQ_Number;
                        DataSet updateDT = requestForQuotation_ISERVICES.getdetailsRFQ(CompID, branchID, rfq_no, UserID, DocumentMenuId);
                        ViewBag.AttechmentDetails = updateDT.Tables[8];
                        ViewBag.SubItemDetails = updateDT.Tables[9];
                        RFQ_Model.RFQ_no = updateDT.Tables[0].Rows[0]["rfq_no"].ToString();
                        RFQ_Model.RFQ_date = updateDT.Tables[0].Rows[0]["rfq_dt"].ToString();
                        RFQ_Model.SourceType = updateDT.Tables[0].Rows[0]["rfq_type"].ToString();
                        RFQ_Model.Valid_upto = updateDT.Tables[0].Rows[0]["valid_dt"].ToString();
                        RFQ_Model.Remarks = updateDT.Tables[0].Rows[0]["rfq_rem"].ToString();
                        RFQ_Model.CreatedBy = updateDT.Tables[0].Rows[0]["create_nm"].ToString();
                        RFQ_Model.CreatedOn = updateDT.Tables[0].Rows[0]["create_dt"].ToString();
                        RFQ_Model.ApprovedBy = updateDT.Tables[0].Rows[0]["app_nm"].ToString();
                        RFQ_Model.ApprovedOn = updateDT.Tables[0].Rows[0]["app_dt"].ToString();
                        RFQ_Model.AmmendedBy = updateDT.Tables[0].Rows[0]["mod_nm"].ToString();
                        RFQ_Model.AmmendedOn = updateDT.Tables[0].Rows[0]["mod_dt"].ToString();
                        pRReqList.PrDt = updateDT.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        pRReqList.PrNo = updateDT.Tables[0].Rows[0]["src_doc_no"].ToString();
                        pRReqs.Add(pRReqList);
                        RFQ_Model.pRReqLists = pRReqs;
                        RFQ_Model.reqNo = updateDT.Tables[0].Rows[0]["src_doc_no"].ToString();
                        RFQ_Model.src_doc_no = updateDT.Tables[0].Rows[0]["src_doc_no"].ToString();
                        RFQ_Model.PR_date = updateDT.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        RFQ_Model.src_doc_dt = updateDT.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        RFQ_Model.Create_id = updateDT.Tables[0].Rows[0]["creator_id"].ToString();
                        RFQ_Model.RFQ_status = updateDT.Tables[0].Rows[0]["app_status"].ToString();


                        string approval_id = updateDT.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = updateDT.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = updateDT.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        RFQ_Model.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        RFQ_Model.DocumentStatus = doc_status;
                        ViewBag.DocumentStatus = doc_status;

                        if (RFQ_Model.doc_status == "C")
                        {
                            RFQ_Model.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            RFQ_Model.BtnName = "Refresh";
                        }
                        else
                        {
                            RFQ_Model.CancelFlag = false;
                        }

                        RFQ_Model.WFBarStatus = DataTableToJSONWithStringBuilder(updateDT.Tables[7]);
                        RFQ_Model.WFStatus = DataTableToJSONWithStringBuilder(updateDT.Tables[6]);


                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (doc_status != "D" && doc_status != "F")
                        {
                            ViewBag.AppLevel = updateDT.Tables[7];
                        }

                        if (ViewBag.AppLevel != null && RFQ_Model.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (updateDT.Tables[5].Rows.Count > 0)
                            {
                                sent_to = updateDT.Tables[5].Rows[0]["sent_to"].ToString();
                            }

                            if (updateDT.Tables[6].Rows.Count > 0)
                            {
                                nextLevel = updateDT.Tables[6].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    RFQ_Model.BtnName = "Refresh";
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
                                        RFQ_Model.BtnName = "BtnToDetailPage";
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
                                        RFQ_Model.BtnName = "BtnToDetailPage";
                                    }

                                }
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    RFQ_Model.BtnName = "BtnToDetailPage";
                                }


                                if (nextLevel == "0")
                                {
                                    if (sent_to == UserID)
                                    {
                                        ViewBag.Approve = "Y";
                                        ViewBag.ForwardEnbl = "N";
                                        //Session["BtnName"] = "BtnToDetailPage";
                                        RFQ_Model.BtnName = "BtnToDetailPage";
                                    }


                                }
                            }
                            if (doc_status == "F")
                            {
                                if (UserID == sent_to)
                                {
                                    ViewBag.ForwardEnbl = "Y";
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    RFQ_Model.BtnName = "BtnToDetailPage";
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
                                    RFQ_Model.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    RFQ_Model.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    RFQ_Model.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                       // ViewBag.MenuPageName = getDocumentName();
                        RFQ_Model.Title = title;
                        ViewBag.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                        ViewBag.ItemDetails = updateDT.Tables[1];
                        ViewBag.Suppdetail_Save = updateDT.Tables[2];
                        ViewBag.ItemDelSchdetails = updateDT.Tables[3];
                        ViewBag.ItemTermsdetails = updateDT.Tables[4];
                      //  ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationDetail.cshtml", RFQ_Model);
                    }
                    //if (Session["BtnName"].ToString() == "Refresh")
                    if (RFQ_Model.BtnName == "Refresh")
                    {
                        ViewBag.Approve = "Y";
                        ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationDetail.cshtml", RFQ_Model);

                    }
                    else
                    {
                        RFQ_Model.Command = "Add";
                        RFQ_Model.TransType = "Save";
                        RFQ_Model.AppStatus = "D";
                        RFQ_Model.BtnName = "BtnAddNew";
                        RFQ_Model.ProspectFromRFQ = "Y";
                        RFQ_Model.ProspectFromQuot = "N";
                        //Session["Command"] = "Add";
                        //Session["TransType"] = "Save";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["ProspectFromRFQ"] = "Y";
                        //Session["ProspectFromQuot"] = "N";
                    //    ViewBag.VBRoleList = GetRoleList();
                        return View("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationDetail.cshtml", RFQ_Model);
                    }
                }
                else
                {/*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/

                    if (Session["CompId"] != null)
                        CompID = Session["CompId"].ToString();
                    if (Session["BranchId"] != null)
                        branchID = Session["BranchId"].ToString();
                    //var commCont = new CommonController(_Common_IServices);
                    //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                    //{
                    //    TempData["Message1"] = "Financial Year not Exist";
                    //}
                    /*End to chk Financial year exist or not*/
                    //RequestForQuotation_Model RFQ_Model = new RequestForQuotation_Model();
                    //   var other = new CommonController(_Common_IServices); /**Commented By Nitesh 30-03-2024**/
                    //Session["AttachMentDetailItmStp"] = null;
                    //Session["Guid"] = null;
                    // ViewBag.AppLevel = other.GetApprovalLevel(CompID, branchID, DocumentMenuId); /**Commented By Nitesh 30-03-2024**/
                    ViewBag.DocumentMenuId = DocumentMenuId;
                    //Session["AppStatus"] = 'D';
                    RFQ_Model1.AppStatus = "D";
                  //  ViewBag.MenuPageName = getDocumentName();
                    RFQ_Model1.Title = title;
                    RFQ_Model1.DocumentStatus = "D";
                    ViewBag.DocumentStatus = "D";
                    DataSet ds = GetSuppList();

                    List<supplist> _suppListD = new List<supplist>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        supplist supplist = new supplist();
                        supplist.Supp_id = dr["supp_id"].ToString();
                        supplist.Supp_Name = dr["supp_name"].ToString();
                        _suppListD.Add(supplist);
                    }
                    _suppListD.Insert(0, new supplist() { Supp_id = "0", Supp_Name = "---Select---" });
                    RFQ_Model1._supplist = _suppListD;
                    List<PRReqList> pRReqs = new List<PRReqList>();
                    PRReqList pRReqList = new PRReqList();
                    pRReqList.PrDt = "";
                    pRReqList.PrNo = "---Select---";
                    pRReqs.Add(pRReqList);
                    RFQ_Model1.pRReqLists = pRReqs; 
                    if (TempData["FilterData"] != null && TempData["FilterData"].ToString() != "")
                    {
                        RFQ_Model1.PRData1 = TempData["FilterData"].ToString();
                    }
                    if (TempData["WF_status1"] != null && TempData["WF_status1"].ToString() != "")
                    {
                        RFQ_Model1.WF_status1 = TempData["WF_status1"].ToString();
                    }
                    //if (Session["TransType"].ToString() == "Update" || Session["TransType"].ToString() == "Edit")
                    if (RFQ_Model1.TransType == "Update" || RFQ_Model1.TransType == "Edit")
                    {
                        //if (Session["CompId"] != null)
                        //{
                        //    CompID = Session["CompId"].ToString();
                        //}
                        //branchID = Session["BranchId"].ToString();
                        //string rfq_no = Session["RFQ_no"].ToString();
                        string rfq_no = RFQ_Model1.RFQ_Number;
                        DataSet updateDT = requestForQuotation_ISERVICES.getdetailsRFQ(CompID, branchID, rfq_no, UserID, DocumentMenuId);
                        ViewBag.AttechmentDetails = updateDT.Tables[8];
                        ViewBag.SubItemDetails = updateDT.Tables[9];
                        RFQ_Model1.RFQ_no = updateDT.Tables[0].Rows[0]["rfq_no"].ToString();
                        RFQ_Model1.RFQ_date = updateDT.Tables[0].Rows[0]["rfq_dt"].ToString();
                        RFQ_Model1.SourceType = updateDT.Tables[0].Rows[0]["rfq_type"].ToString();
                        RFQ_Model1.Valid_upto = updateDT.Tables[0].Rows[0]["valid_dt"].ToString();
                        RFQ_Model1.Remarks = updateDT.Tables[0].Rows[0]["rfq_rem"].ToString();
                        RFQ_Model1.CreatedBy = updateDT.Tables[0].Rows[0]["create_nm"].ToString();
                        RFQ_Model1.CreatedOn = updateDT.Tables[0].Rows[0]["create_dt"].ToString();
                        RFQ_Model1.ApprovedBy = updateDT.Tables[0].Rows[0]["app_nm"].ToString();
                        RFQ_Model1.ApprovedOn = updateDT.Tables[0].Rows[0]["app_dt"].ToString();
                        RFQ_Model1.AmmendedBy = updateDT.Tables[0].Rows[0]["mod_nm"].ToString();
                        RFQ_Model1.AmmendedOn = updateDT.Tables[0].Rows[0]["mod_dt"].ToString();
                        pRReqList.PrDt = updateDT.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        pRReqList.PrNo = updateDT.Tables[0].Rows[0]["src_doc_no"].ToString();
                        pRReqs.Add(pRReqList);
                        RFQ_Model1.pRReqLists = pRReqs;
                        RFQ_Model1.reqNo = updateDT.Tables[0].Rows[0]["src_doc_no"].ToString();
                        RFQ_Model1.src_doc_no = updateDT.Tables[0].Rows[0]["src_doc_no"].ToString();
                        RFQ_Model1.PR_date = updateDT.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        RFQ_Model1.src_doc_dt = updateDT.Tables[0].Rows[0]["src_doc_dt"].ToString();
                        RFQ_Model1.Create_id = updateDT.Tables[0].Rows[0]["creator_id"].ToString();
                        RFQ_Model1.RFQ_status = updateDT.Tables[0].Rows[0]["app_status"].ToString();


                        string approval_id = updateDT.Tables[0].Rows[0]["approval_id"].ToString();
                        string create_id = updateDT.Tables[0].Rows[0]["creator_id"].ToString();
                        string doc_status = updateDT.Tables[0].Rows[0]["status_code"].ToString().Trim();
                        RFQ_Model1.doc_status = doc_status;
                        //Session["DocumentStatus"] = doc_status;
                        RFQ_Model1.DocumentStatus = doc_status;
                        ViewBag.DocumentStatus = doc_status;

                        if (RFQ_Model1.doc_status == "C")
                        {
                            RFQ_Model1.CancelFlag = true;
                            //Session["BtnName"] = "Refresh";
                            RFQ_Model1.BtnName = "Refresh";
                        }
                        else
                        {
                            RFQ_Model1.CancelFlag = false;
                        }

                        RFQ_Model1.WFBarStatus = DataTableToJSONWithStringBuilder(updateDT.Tables[7]);
                        RFQ_Model1.WFStatus = DataTableToJSONWithStringBuilder(updateDT.Tables[6]);


                        //if (ViewBag.AppLevel != null && Session["Command"].ToString() != "Edit")
                        if (ViewBag.AppLevel != null && RFQ_Model1.Command != "Edit")
                        {

                            var sent_to = "";
                            var nextLevel = "";
                            if (updateDT.Tables[5].Rows.Count > 0)
                            {
                                sent_to = updateDT.Tables[5].Rows[0]["sent_to"].ToString();
                            }

                            if (updateDT.Tables[6].Rows.Count > 0)
                            {
                                nextLevel = updateDT.Tables[6].Rows[0]["nextlevel"].ToString().Trim();
                            }

                            if (doc_status == "D")
                            {
                                if (create_id != UserID)
                                {
                                    //Session["BtnName"] = "Refresh";
                                    RFQ_Model1.BtnName = "Refresh";
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
                                        RFQ_Model1.BtnName = "BtnToDetailPage";
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
                                        RFQ_Model1.BtnName = "BtnToDetailPage";
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
                                    RFQ_Model1.BtnName = "BtnToDetailPage";
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
                                        RFQ_Model1.BtnName = "BtnToDetailPage";
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
                                    RFQ_Model1.BtnName = "BtnToDetailPage";
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
                                    RFQ_Model1.BtnName = "BtnToDetailPage";
                                }
                            }
                            if (doc_status == "A")
                            {
                                if (create_id == UserID || approval_id == UserID)
                                {
                                    //Session["BtnName"] = "BtnToDetailPage";
                                    RFQ_Model1.BtnName = "BtnToDetailPage";

                                }
                                else
                                {
                                    //Session["BtnName"] = "Refresh";
                                    RFQ_Model1.BtnName = "Refresh";
                                }
                            }
                        }
                        if (ViewBag.AppLevel.Rows.Count == 0)
                        {
                            ViewBag.Approve = "Y";
                        }
                        //  ViewBag.MenuPageName = getDocumentName();  /**Commented By Nitesh 30-03-2024**/
                        RFQ_Model1.Title = title;
                        ViewBag.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                        ViewBag.ItemDetails = updateDT.Tables[1];
                        ViewBag.Suppdetail_Save = updateDT.Tables[2];
                        ViewBag.ItemDelSchdetails = updateDT.Tables[3];
                        ViewBag.ItemTermsdetails = updateDT.Tables[4];
                        // ViewBag.VBRoleList = GetRoleList();  /**Commented By Nitesh 30-03-2024**/
                        return View("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationDetail.cshtml", RFQ_Model1);
                    }
                    //if (Session["BtnName"].ToString() == "Refresh")
                    if (RFQ_Model1.BtnName == "Refresh")
                    {
                        ViewBag.Approve = "Y";
                        //     ViewBag.VBRoleList = GetRoleList();  /**Commented By Nitesh 30-03-2024**/
                        return View("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationDetail.cshtml", RFQ_Model1);

                    }
                    else
                    {
                        RFQ_Model1.Command = "Add";
                        RFQ_Model1.TransType = "Save";
                        RFQ_Model1.AppStatus = "D";
                        RFQ_Model1.BtnName = "BtnAddNew";
                        RFQ_Model1.ProspectFromRFQ = "Y";
                        RFQ_Model1.ProspectFromQuot = "N";
                        //  ViewBag.VBRoleList = GetRoleList();  /**Commented By Nitesh 30-03-2024**/

                        //Session["Command"] = "Add";
                        //Session["TransType"] = "Save";
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["ProspectFromRFQ"] = "Y";
                        //Session["ProspectFromQuot"] = "N";                        
                        return View("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationDetail.cshtml", RFQ_Model1);
                    }
                }               
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
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
        [HttpPost]
        public DataSet GetSuppList()
        {
            try
            {

                string Comp_ID = string.Empty;
                string SearchName = string.Empty;
                string SuppPros_type = string.Empty;
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                DataSet result = requestForQuotation_ISERVICES.GetSuppList(Comp_ID, branchID, SearchName, SuppPros_type);
                return result;
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }
        }
        public ActionResult GetAutoCompleteSearchSuppList(RequestForQuotation_Model RFQ_Model)
        {
            
            string SearchName = string.Empty;
            string SuppPros_type = string.Empty;
            Dictionary<string, string> SuppList = new Dictionary<string, string>();
            string Comp_ID = string.Empty;
            string Br_ID = string.Empty;
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
                if (string.IsNullOrEmpty(RFQ_Model.SearchName))
                {
                    SearchName = "";
                }
                else
                {
                    SearchName = RFQ_Model.SearchName;
                }
               
                SuppPros_type = RFQ_Model.SuppPros_type;
                DataSet searchresult = requestForQuotation_ISERVICES.GetSuppList(Comp_ID, Br_ID, SearchName, SuppPros_type);
                if (searchresult.Tables[0].Rows.Count > 0)
                {
                    SuppList.Add("0", "---Select---");
                    for (int i = 0; i < searchresult.Tables[0].Rows.Count; i++)
                    {

                        SuppList.Add(searchresult.Tables[0].Rows[i]["supp_id"].ToString(), searchresult.Tables[0].Rows[i]["supp_name"].ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return Json(SuppList.Select(c => new { Name = c.Value, ID = c.Key }).ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSuppDetail(string Supp_id, string SuppPros_type)
        {
            try
            {
                 JsonResult DataRows = null;
                string Comp_ID = string.Empty;               
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
           
                DataSet result = requestForQuotation_ISERVICES.GetSuppDetails(Comp_ID, branchID,Supp_id, SuppPros_type);

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
        [ValidateAntiForgeryToken]
        public ActionResult RequestForQuotationDetailSave(RequestForQuotation_Model RFQ_Model, string Command)
        {
            try
            {   /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                var commCont = new CommonController(_Common_IServices);
                /*End to chk Financial year exist or not*/
                if (RFQ_Model.hdnAction == "Delete")
                {
                    Command = RFQ_Model.hdnAction;
                }
                switch (Command)
                {
                    case "AddNew":
                        RequestForQuotation_Model RFQ_ModelAddNew = new RequestForQuotation_Model();
                        TempData["FilterData"] = null;
                        RFQ_ModelAddNew.Message = null;
                        RFQ_ModelAddNew.AppStatus = "D";
                        RFQ_ModelAddNew.BtnName = "BtnAddNew";
                        RFQ_ModelAddNew.TransType = "Save";
                        RFQ_ModelAddNew.Command = "New";
                        TempData["ModelData"] = RFQ_ModelAddNew;
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        {
                            TempData["Message"] = "Financial Year not Exist";
                             if (!string.IsNullOrEmpty(RFQ_Model.RFQ_no))
                                return RedirectToAction("EditRFQ", new { RFQId = RFQ_Model.RFQ_no, RFQDate= RFQ_Model.RFQ_date, PRData = RFQ_Model.PRData1, WF_status = RFQ_Model.WFStatus });
                            else
                                RFQ_ModelAddNew.Command = "Refresh";
                            RFQ_ModelAddNew.TransType = "Refresh";
                            RFQ_ModelAddNew.BtnName = "Refresh";
                            RFQ_ModelAddNew.DocumentStatus = null;
                            TempData["ModelData"] = RFQ_ModelAddNew;
                            return RedirectToAction("AddRequestForQuotationDetail", RFQ_ModelAddNew);

                        }
                        /*End to chk Financial year exist or not*/

                        //Session["Message"] = null;
                        //Session["AppStatus"] = 'D';
                        //Session["BtnName"] = "BtnAddNew";
                        //Session["TransType"] = "Save";
                        //Session["Command"] = "New";
                        return RedirectToAction("AddRequestForQuotationDetail");

                    case "Edit":
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditRFQ", new { RFQId = RFQ_Model.RFQ_no, PRData = RFQ_Model.PRData1, WF_status = RFQ_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        string RFQdate = RFQ_Model.RFQ_date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, RFQdate) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditRFQ", new { RFQId = RFQ_Model.RFQ_no, RFQDate = RFQ_Model.RFQ_date, PRData = RFQ_Model.PRData1, WF_status = RFQ_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        var RFQ_Number = "";
                        var RFQ_Date = "";
                        var TransType = "";
                        var BtnName = "";
                        if (CheckPQAgainstRFQ(RFQ_Model.RFQ_no, RFQ_Model.RFQ_date) == "Used")
                        {
                            //Session["Message"] = "Used";
                            RFQ_Model.Message = "Used";
                            RFQ_Model.RFQ_Number = RFQ_Model.RFQ_no;
                            RFQ_Model.RFQ_date = RFQ_Model.RFQ_date;
                            RFQ_Model.BtnName = "BtnSave";
                            TempData["FilterData"] = RFQ_Model.PRData1;
                            TempData["ModelData"] = RFQ_Model;
                        }
                        else
                        {
                            TempData["FilterData"] = RFQ_Model.PRData1;
                            RFQ_Model.TransType = "Update";
                            RFQ_Model.Command = Command;
                            RFQ_Model.BtnName = "BtnEdit";
                            RFQ_Model.Message = null;
                            RFQ_Model.RFQ_Number = RFQ_Model.RFQ_no;
                            RFQ_Model.RFQ_date = RFQ_Model.RFQ_date;
                            TempData["ModelData"] = RFQ_Model;
                            RFQ_Number = RFQ_Model.RFQ_no;
                            RFQ_Date = RFQ_Model.RFQ_date;
                            TransType = "Update";
                            BtnName= "BtnEdit";
                            //Session["TransType"] = "Update";
                            //Session["Command"] = Command;
                            //Session["BtnName"] = "BtnEdit";
                            //Session["Message"] = null;
                            //Session["RFQ_no"] = RFQ_Model.RFQ_no;
                        }
                        
                        return( RedirectToAction("AddRequestForQuotationDetail", new { RFQ_Number = RFQ_Number, RFQ_Date= RFQ_Date, TransType, BtnName, Command }));

                    case "Delete":
                        RequestForQuotation_Model RFQ_Modeldelete = new RequestForQuotation_Model();
                        //Session["Command"] = Command;
                        //Session["BtnName"] = "Refresh";
                        RFQ_Model.Command = Command;
                        //RFQ_Modeldelete.BtnName = "Refresh";
                        RFQDelete(RFQ_Model, Command);
                        RFQ_Modeldelete.Message = "Deleted";
                        RFQ_Modeldelete.Command = "Refresh";
                        RFQ_Modeldelete.RFQ_Number = "";
                        //RFQ_Modeldelete.RFQ_date = "";
                        RFQ_Modeldelete.TransType = "Refresh";
                        RFQ_Modeldelete.AppStatus = "DL";
                        RFQ_Modeldelete.BtnName = "Refresh";
                        TempData["ModelData"] = RFQ_Modeldelete;
                        TempData["FilterData"] = RFQ_Model.PRData1;
                        return RedirectToAction("AddRequestForQuotationDetail");

                    case "Save":
                        //Session["Command"] = Command;
                        RFQ_Model.Command = Command;
                        if (RFQ_Model.TransType == null)
                        {
                            RFQ_Model.TransType = Command;
                        }
                        SaveRFQDetail(RFQ_Model);
                        if (RFQ_Model.Message == "DataNotFound")
                        {
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        else if (RFQ_Model.Message == "DocModify")
                        {
                            DataSet ds = GetSuppList();

                            List<supplist> _suppListD = new List<supplist>();
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                supplist supplist = new supplist();
                                supplist.Supp_id = dr["supp_id"].ToString();
                                supplist.Supp_Name = dr["supp_name"].ToString();
                                _suppListD.Add(supplist);
                            }
                            _suppListD.Insert(0, new supplist() { Supp_id = "0", Supp_Name = "---Select---" });
                            RFQ_Model._supplist = _suppListD;
                            
                            List<PRReqList> pRReqs = new List<PRReqList>();
                            PRReqList pRReqList = new PRReqList();
                            pRReqList.PrDt = "";
                            pRReqList.PrNo = RFQ_Model.src_doc_no; 
                            pRReqs.Add(pRReqList);
                            RFQ_Model.pRReqLists = pRReqs;

                            
                            var other = new CommonController(_Common_IServices);
                            ViewBag.AppLevel = other.GetApprovalLevel(CompID, branchID, DocumentMenuId);
                            RFQ_Model.reqNo = RFQ_Model.src_doc_no;
                            RFQ_Model.PR_date = RFQ_Model.src_doc_dt;
                            ViewBag.ItemDetails = ViewData["ItemRFQDetails"];
                            ViewBag.Suppdetail_Save = ViewData["SuppRFQDetails"];
                            ViewBag.ItemDelSchdetails = ViewData["DelvScheRFQDetails"];  
                            ViewBag.ItemTermsdetails = ViewData["TrmAndConRFQDetails"];
                            ViewBag.SubItemDetails = ViewData["SubItemDetail"]; 
                            //ViewBag.AttechmentDetails = ViewData["AttachmentDetails"];
                            RFQ_Model.BtnName = "Refresh";
                            RFQ_Model.Command = "Refresh"; 
                            ViewBag.VBRoleList = GetRoleList();
                            return View("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationDetail.cshtml", RFQ_Model);
                        }
                        else
                        {
                            RFQ_Number = RFQ_Model.RFQ_Number;
                            RFQ_Date = RFQ_Model.RFQ_date;
                            TransType = RFQ_Model.TransType;
                            BtnName = RFQ_Model.BtnName;
                            TempData["ModelData"] = RFQ_Model;
                            TempData["FilterData"] = RFQ_Model.PRData1;
                            return (RedirectToAction("AddRequestForQuotationDetail", new { RFQ_Number = RFQ_Number, RFQ_Date= RFQ_Date, TransType, BtnName, Command }));
                        }
                    case "Forward":
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditRFQ", new { RFQId = RFQ_Model.RFQ_no, PRData = RFQ_Model.PRData1, WF_status = RFQ_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        string RFQdate1 = RFQ_Model.RFQ_date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, RFQdate1) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditRFQ", new { RFQId = RFQ_Model.RFQ_no, RFQDate = RFQ_Model.RFQ_date, PRData = RFQ_Model.PRData1, WF_status = RFQ_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        return new EmptyResult();
                    case "Approve":
                        /*start Add by Hina on 07-02-2024 to chk Financial year exist or not*/
                        if (Session["CompId"] != null)
                            CompID = Session["CompId"].ToString();
                        if (Session["BranchId"] != null)
                            branchID = Session["BranchId"].ToString();
                        //if (commCont.CheckFinancialYear(CompID, branchID) == "Not Exist")
                        //{
                        //    TempData["Message"] = "Financial Year not Exist";
                        //    return RedirectToAction("EditRFQ", new { RFQId = RFQ_Model.RFQ_no, PRData = RFQ_Model.PRData1, WF_status = RFQ_Model.WFStatus });
                        //}
                        /*Above Commented and modify by Hina sharma on 05-05-2025 to check Existing with previous year transaction*/
                        string RFQdate2 = RFQ_Model.RFQ_date;
                        if (commCont.CheckFinancialYearAndPreviousYear(CompID, branchID, RFQdate2) == "TransNotAllow")
                        {
                            TempData["Message1"] = "TransNotAllow";
                            return RedirectToAction("EditRFQ", new { RFQId = RFQ_Model.RFQ_no, RFQDate = RFQ_Model.RFQ_date, PRData = RFQ_Model.PRData1, WF_status = RFQ_Model.WFStatus });
                        }
                        /*End to chk Financial year exist or not*/
                        //Session["Command"] = Command;
                        RFQ_Model.Command = Command;
                        string RFQ_no = RFQ_Model.RFQ_no;
                        string RFQ_date=RFQ_Model.RFQ_date;
                        string src_doc_no=RFQ_Model.src_doc_no;
                        string src_doc_dt = RFQ_Model.src_doc_dt;
                        string SrcType = RFQ_Model.SourceType;
                        //Session["RFQ_no"] = RFQ_no;
                        RFQApprove(RFQ_Model, RFQ_no, RFQ_date, src_doc_no, src_doc_dt, SrcType, "","","","","");
                        RFQ_Number = RFQ_Model.RFQ_Number;
                        RFQ_Date = RFQ_Model.RFQ_date;
                        TransType = RFQ_Model.TransType;
                        BtnName = RFQ_Model.BtnName;
                        TempData["ModelData"] = RFQ_Model;
                        TempData["FilterData"] = RFQ_Model.PRData1;
                        return( RedirectToAction("AddRequestForQuotationDetail", new { RFQ_Number = RFQ_Number, RFQ_Date= RFQ_Date, TransType, BtnName, Command }));

                    case "Refresh":
                        RequestForQuotation_Model RFQ_ModelRefresh = new RequestForQuotation_Model();
                        //Session["BtnName"] = "Refresh";
                        //Session["Command"] = Command;
                        //Session["TransType"] = "Save";
                        //Session["Message"] = null;
                        //Session["DocumentStatus"] = null;
                        RFQ_ModelRefresh.BtnName = "Refresh";
                        RFQ_ModelRefresh.Command = Command;
                        RFQ_ModelRefresh.TransType = "Save";
                        RFQ_ModelRefresh.Message = null;
                        RFQ_ModelRefresh.DocumentStatus = null;
                        ViewBag.DocumentStatus = null;
                        TempData["ModelData"] = RFQ_ModelRefresh;
                        TempData["FilterData"] = RFQ_Model.PRData1;
                        return RedirectToAction("AddRequestForQuotationDetail");

                    case "Print":
                        return GenratePdfFile(RFQ_Model);
                        //return PartialView("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationPrint.cshtml");
                    case "BacktoList":
                        //Session.Remove("RFQ_no");
                        //Session.Remove("Message");
                        //Session.Remove("TransType");
                        //Session.Remove("Command");
                        //Session.Remove("BtnName");
                        //Session.Remove("DocumentStatus");
                        TempData["WF_status"] = RFQ_Model.WF_status1;
                        TempData["FilterData"] = RFQ_Model.PRData1;
                        return RedirectToAction("RequestForQuotation");

                    default:
                        return new EmptyResult();

                }

               // ViewBag.MenuPageName = getDocumentName();
              //  return RedirectToAction("AddRequestForQuotationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return View("~/Views/Shared/Error.cshtml");
            }

        }
        /*-------------------------------For PDF Print Start-----------------------------*/
       
        public FileResult GenratePdfFile(RequestForQuotation_Model RFQ_Model)
        {

            try
            {
                DataSet Deatils = new DataSet();
                if (RFQ_Model.RFQ_no != "" && RFQ_Model.RFQ_date != "")
                {
                    if (Session["CompId"] != null)
                    {
                        CompID = Session["CompId"].ToString();
                    }
                    if (Session["BranchId"] != null)
                    {
                        branchID = Session["BranchId"].ToString();
                    }
                    Deatils = requestForQuotation_ISERVICES.GetRequestForQuotationDeatils(CompID, branchID, RFQ_Model.RFQ_no, RFQ_Model.RFQ_date);
                }
                var count = 0;
                string htmlcontent = "";
                //string DelSchedule = "";
                StringReader reader = null;
                Document pdfDoc = null;
                PdfWriter writer = null;
                var outputStream = new MemoryStream();
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");
                if (Deatils.Tables[1].Rows.Count > 1)
                {
                    using (var zip = new ZipFile())
                    {
                        foreach (DataRow dr in Deatils.Tables[1].Rows)
                        {
                            using (MemoryStream stream = new System.IO.MemoryStream())
                            {
                                pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                                writer = PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                htmlcontent = HTMLString(Deatils, count, "Detail");
                                //DelSchedule = HTMLString(Deatils, count, "DelSch");/*Commented by Hina sharma on 24-10-2024 to show delivery schedule on same page as PO*/
                                count = count + 1;
                                reader = new StringReader(htmlcontent);
                                int pageN = writer.PageNumber;
                                String text = "Page " + 1;

                                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);

                                Paragraph para = new Paragraph(text, new Font(Font.FontFamily.HELVETICA, 10));
                                para.Alignment = Element.ALIGN_CENTER;
                                //pdfDoc.Add(para);

                                //pdfDoc.NewPage();/*Commented by Hina sharma on 24-10-2024 to show delivery schedule on same page as PO*/

                                //reader = new StringReader(DelSchedule);/*Commented by Hina sharma on 24-10-2024 to show delivery schedule on same page as PO*/
                                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);
                                var stremlen = stream.Length;

                                text = "Page " + 2;
                                Paragraph para1 = new Paragraph(text, new Font(Font.FontFamily.HELVETICA, 10));
                                para1.Alignment = Element.ALIGN_CENTER;

                                pdfDoc.Close();
                                pdfDoc.NewPage();
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
                                zip.AddEntry(PageName + dr["supp_name"] + ".pdf", bytes.ToArray());
                            }

                        }
                        var a = zip.Count;
                        var b = zip[0];

                        zip.Save(outputStream);
                    }
                    outputStream.Position = 0;
                    return File(outputStream, "application/zip", PageName + ".zip");
                }
                else
                {
                    if (Deatils.Tables[1].Rows.Count > 0)
                    {
                        htmlcontent = HTMLString(Deatils, count, "Detail");
                        //DelSchedule = HTMLString(Deatils, count, "DelSch");/*Commented by Hina sharma on 24-10-2024 to show delivery schedule on same page as PO*/
                        return GenratePdfFiles(htmlcontent/*, DelSchedule*/);
                    }
                    else
                    {
                        return File("ErrorPage", "application/pdf");
                    }
                }

            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
            finally
            {
            }
        }
        public FileResult GenratePdfFiles(string htmlcontent/*, string DelSchedule*/)
        {
            try
            {
                StringReader reader = null;
                Document pdfDoc = null;
                PdfWriter writer = null;
                //PdfCopy pdfCopyProvider = null;
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");

                    pdfDoc = new Document(PageSize.A4, 0f, 0f, 10f, 20f);
                    pdfDoc.AddTitle(title);

                    writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    reader = new StringReader(htmlcontent);
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, reader);

                    //pdfDoc.NewPage();
                    //reader = new StringReader(DelSchedule);/*Commented by Hina sharma on 24-10-2024 to show delivery schedule on same page as PO*/
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
                    return File(bytes.ToArray(), "application/pdf", PageName + ".pdf");

                }
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return File("ErrorPage", "application/pdf");
            }
            finally
            {
            }
        }
        public string HTMLString(DataSet Deatils, int count, string Flag)
        {
            string HtmlText = "";
            if (Deatils != null)
            {
                ViewBag.Details = Deatils;
                ViewBag.SuppNo = count;
                ViewBag.Section = Flag;
                string path1 = Server.MapPath("~") + "..\\Attachment\\";
                string LogoPath = path1 + Deatils.Tables[0].Rows[0]["logo"].ToString().Replace("/", "\\'");
                ViewBag.FLogoPath = LogoPath.Replace("~\\", "").Replace("\\\\", "\\").Replace("'", "");

                ViewBag.Title = "Request for Quotation";
                ViewBag.DocStatus = Deatils.Tables[0].Rows[0]["rfq_status"].ToString().Trim();
                HtmlText = ConvertPartialViewToString(PartialView("~/Areas/ApplicationLayer/Views/Procurement/RequestForQuotation/RequestForQuotationPrint.cshtml"));
            }
            return HtmlText;
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

        public ActionResult ToRefreshByJS(string ListFilterData, string TrancType)
        {
            RequestForQuotation_Model RFQ_Model = new RequestForQuotation_Model();
            //Session["Message"] = "";
            var a = TrancType.Split(',');
            RFQ_Model.RFQ_Number = a[0].Trim();
            RFQ_Model.TransType = a[1].Trim();
            var WF_status1 = a[2].Trim();
            RFQ_Model.BtnName = "BtnToDetailPage";
            var RFQ_Number = RFQ_Model.RFQ_Number;
            var RFQ_Date = RFQ_Model.RFQ_date;
            var TransType = RFQ_Model.TransType;
            var BtnName = RFQ_Model.BtnName;
            var Command = RFQ_Model.Command;
            TempData["ModelData"] = RFQ_Model;
            TempData["FilterData"] = ListFilterData;
            TempData["WF_status1"] = WF_status1;
            return( RedirectToAction("AddRequestForQuotationDetail", new { RFQ_Number = RFQ_Number, RFQ_Date= RFQ_Date, TransType, BtnName, Command }));
        }
        public string CheckPQAgainstRFQ(string DocNo, string DocDate)
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
                DataSet Deatils = requestForQuotation_ISERVICES.CheckPQAgainstRFQ(Comp_ID, Br_ID, DocNo, DocDate);
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
        private ActionResult RFQDelete(RequestForQuotation_Model _RFQModel, string Command)
        {
            try
            {

                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                string br_id = Session["BranchId"].ToString();

                string RFQ_NO = _RFQModel.RFQ_no;

                string Message = requestForQuotation_ISERVICES.RFQDelete(_RFQModel, CompID, br_id, RFQ_NO);

                if (!string.IsNullOrEmpty(RFQ_NO))
                {
                    getDocumentName(); /* To set Title*/
                    string PageName = title.Replace(" ", "");
                    var other = new CommonController(_Common_IServices);
                    string RFQ_NO1 = RFQ_NO.Replace("/", "");
                    other.DeleteTempFile(CompID + br_id, PageName, RFQ_NO1, Server);
                }
                _RFQModel.Message = "Deleted";
                _RFQModel.Command = "Refresh";
                _RFQModel.RFQ_Number = "";
                _RFQModel.TransType = "Refresh";
                _RFQModel.AppStatus = "DL";
                _RFQModel.BtnName = "Refresh";
                //Session["Message"] = "Deleted";
                //Session["Command"] = "Refresh";
                //Session["PR_No"] = "";
                //Session["TransType"] = "Refresh";
                //Session["AppStatus"] = "DL";
                //Session["BtnName"] = "Refresh";
                return RedirectToAction("AddRequestForQuotationDetail");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }

        }

        public ActionResult RFQApprove(RequestForQuotation_Model _RFQ_Model,string RFQ_no, string RFQ_date, string src_doc_no,string src_doc_dt,
            string SrcType, string A_Status, string A_Level, string A_Remarks,string ListFilterData,string WF_status1)
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
                //RequestForQuotation_Model _RFQ_Model = new RequestForQuotation_Model();
                _RFQ_Model.CreatedBy = Session["UserId"].ToString();
                _RFQ_Model.RFQ_no = RFQ_no;
                _RFQ_Model.RFQ_date = RFQ_date;
                _RFQ_Model.src_doc_no = src_doc_no;
                _RFQ_Model.src_doc_dt = src_doc_dt;
                _RFQ_Model.SourceType = SrcType;
                string mac = Session["UserMacaddress"].ToString();
                string system = Session["UserSystemName"].ToString();
                string ip = Session["UserIP"].ToString();
                string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                string Message = requestForQuotation_ISERVICES.RFQApprove(_RFQ_Model, CompID, BranchID, A_Status, A_Level, A_Remarks, mac_id, DocumentMenuId);
                string ApMessage = Message.Split(',')[0].Trim();
                //string RFQ_no = Message.Split(',')[1].Trim();
                if (ApMessage == "A")
                {
                    //Session["Message"] = "Approved";
                    _RFQ_Model.Message = "Approved";
                }
                _RFQ_Model.TransType = "Update";
                _RFQ_Model.Command = "Approve";
                _RFQ_Model.RFQ_Number = _RFQ_Model.RFQ_no;
                //_RFQ_Model.Message = "Approved";
                _RFQ_Model.AppStatus = "D";
                _RFQ_Model.BtnName = "BtnEdit";
                TempData["ModelData"] = _RFQ_Model;
                TempData["FilterData"] = ListFilterData;
                TempData["WF_status1"] = WF_status1;
                var RFQ_Number= _RFQ_Model.RFQ_no;
                var RFQ_Date = _RFQ_Model.RFQ_date;
                var TransType = "Update";
                var BtnName = "BtnEdit";
                var Command= "Approve";
                //Session["TransType"] = "Update";
                //Session["Command"] = "Approve";
                //Session["RFQ_no"] = _RFQ_Model.RFQ_no;
                ////Session["Message"] = "Approved";
                //Session["AppStatus"] = 'D';
                //Session["BtnName"] = "BtnEdit";
                return ( RedirectToAction("AddRequestForQuotationDetail", new { RFQ_Number = RFQ_Number, RFQ_Date= RFQ_Date, TransType, BtnName, Command }));
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                //return View("~/Views/Shared/Error.cshtml");
                throw ex;
            }
        }
        [NonAction]
        public ActionResult SaveRFQDetail(RequestForQuotation_Model RFQ_Model)
        {
            string SaveMessage = "";
            getDocumentName(); /* To set Title*/
            string PageName = title.Replace(" ", "");
            if (Session["compid"] != null)
            {
                CompID = Session["compid"].ToString();
            }
            if (Session["BranchId"] != null)
            {
                branchID = Session["BranchId"].ToString();
            }
            if (Session["userid"] != null)
            {
                userid = Session["userid"].ToString();
            }
            try
            {
              
                if (RFQ_Model.CancelFlag == false)
                {
                    DataTable RFQHeader = new DataTable();
                    DataTable RFQItemDetails = new DataTable();
                    DataTable RFQSuppDetails = new DataTable();
                    DataTable RFQDeleveryShedDetail = new DataTable();
                    DataTable RFQTermAndConDetail = new DataTable();
                    DataTable Attachments = new DataTable();

                    DataTable dtheader = new DataTable();

                    dtheader.Columns.Add("TransType", typeof(string));
                    dtheader.Columns.Add("MenuDocumentId", typeof(string));
                    dtheader.Columns.Add("comp_id", typeof(int));
                    dtheader.Columns.Add("br_id", typeof(int));
                    dtheader.Columns.Add("rfq_no", typeof(string));
                    dtheader.Columns.Add("rfq_dt", typeof(string));
                    dtheader.Columns.Add("rfq_type", typeof(string));
                    dtheader.Columns.Add("src_doc_no", typeof(string));
                    dtheader.Columns.Add("src_doc_dt", typeof(string));
                    dtheader.Columns.Add("rfq_rem", typeof(string));
                    dtheader.Columns.Add("valid_dt", typeof(string));
                    dtheader.Columns.Add("user_id", typeof(int));
                    dtheader.Columns.Add("rfq_status", typeof(string));
                    dtheader.Columns.Add("mac_id", typeof(string));

                    DataRow dtrowHeader = dtheader.NewRow();
                    dtrowHeader["TransType"] = RFQ_Model.TransType;
                    dtrowHeader["MenuDocumentId"] = DocumentMenuId;
                    dtrowHeader["comp_id"] = CompID;
                    dtrowHeader["br_id"] = branchID;
                    dtrowHeader["rfq_no"] = RFQ_Model.RFQ_no;
                    dtrowHeader["rfq_dt"] = RFQ_Model.RFQ_date;
                    dtrowHeader["rfq_type"] = RFQ_Model.SourceType;
                    dtrowHeader["src_doc_no"] = RFQ_Model.src_doc_no;
                    dtrowHeader["src_doc_dt"] =  RFQ_Model.src_doc_dt;//DBNull.Value;// "2021-08-31";//
                    dtrowHeader["rfq_rem"] = RFQ_Model.Remarks;
                    dtrowHeader["valid_dt"] = RFQ_Model.Valid_upto;
                    dtrowHeader["user_id"] = userid;
                    dtrowHeader["rfq_status"] = "D";
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    dtrowHeader["mac_id"] = mac_id;
                    //Session["date"] = RFQ_Model.Valid_upto;

                    dtheader.Rows.Add(dtrowHeader);
                    RFQHeader = dtheader;
                    

                    DataTable dtItem = new DataTable();

                    dtItem.Columns.Add("item_id", typeof(string));
                    dtItem.Columns.Add("uom_id", typeof(int));
                    dtItem.Columns.Add("rfq_qty", typeof(string));
                    dtItem.Columns.Add("it_remarks", typeof(string));

                    JArray jObject = JArray.Parse(RFQ_Model.Itemdetails);

                    for (int i = 0; i < jObject.Count; i++)
                    {
                        DataRow dtrowLines = dtItem.NewRow();

                        dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                        dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                        dtrowLines["rfq_qty"] = jObject[i]["RequQty"].ToString();
                        dtrowLines["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                        dtItem.Rows.Add(dtrowLines);
                    }
                    RFQItemDetails = dtItem;
                    ViewData["ItemRFQDetails"] = dtitemdetail(jObject);


                    DataTable dtSupp = new DataTable();

                    dtSupp.Columns.Add("supp_id", typeof(int));
                    dtSupp.Columns.Add("cont_email", typeof(string));
                    dtSupp.Columns.Add("cont_num", typeof(string));
                    dtSupp.Columns.Add("SuppType", typeof(string));

                    JArray jObjectS = JArray.Parse(RFQ_Model.Suppdetail_Save);

                    for (int i = 0; i < jObjectS.Count; i++)
                    {
                        DataRow dtrowLines = dtSupp.NewRow();

                        dtrowLines["supp_id"] = Convert.ToInt32(jObjectS[i]["SuppID"].ToString());
                        dtrowLines["cont_email"] = jObjectS[i]["SuppEmail"].ToString();
                        dtrowLines["cont_num"] = jObjectS[i]["SuppContact"].ToString();
                        dtrowLines["SuppType"] = jObjectS[i]["SuppType"].ToString();
                        
                        dtSupp.Rows.Add(dtrowLines);
                    }
                    RFQSuppDetails = dtSupp;
                    ViewData["SuppRFQDetails"] = dtsuppdetail(jObjectS); 


                    DataTable dtDelShed = new DataTable();
                        dtDelShed.Columns.Add("item_id", typeof(string));
                        dtDelShed.Columns.Add("sch_date", typeof(string));
                        dtDelShed.Columns.Add("delv_qty", typeof(float));
                    if (RFQ_Model.DelShedDetailList != null && RFQ_Model.DelShedDetailList != "[]")
                    {
                        JArray jObjectD = JArray.Parse(RFQ_Model.DelShedDetailList);

                        for (int i = 0; i < jObjectD.Count; i++)
                        {
                            DataRow dtrowLines = dtDelShed.NewRow();

                            dtrowLines["item_id"] = jObjectD[i]["DelItemID"].ToString();
                            dtrowLines["sch_date"] = jObjectD[i]["DelDate"].ToString();
                            dtrowLines["delv_qty"] = jObjectD[i]["DelQty"].ToString();
                            dtDelShed.Rows.Add(dtrowLines);
                        }
                        ViewData["DelvScheRFQDetails"] = dtdeldetail(jObjectD);
                        ViewBag.QtyDigit = ToFixDecimal(Convert.ToInt32(Session["QtyDigit"].ToString()));
                    }
                    RFQDeleveryShedDetail = dtDelShed;
                    

                    DataTable dtTNC = new DataTable();
                        dtTNC.Columns.Add("term_desc", typeof(string));
                        dtTNC.Columns.Add("sno", typeof(int));
                    if (RFQ_Model.TermAndConDetailsList != null && RFQ_Model.TermAndConDetailsList != "[]")
                    {
                        JArray jObjectT = JArray.Parse(RFQ_Model.TermAndConDetailsList);
                        int sno = 1;
                        for (int i = 0; i < jObjectT.Count; i++)
                        {
                            DataRow dtrowLines = dtTNC.NewRow();
                            dtrowLines["term_desc"] = jObjectT[i]["TermAndCondition"].ToString();
                            dtrowLines["sno"] = sno;
                            dtTNC.Rows.Add(dtrowLines);
                            sno += 1;
                        }
                        ViewData["TrmAndConRFQDetails"] = dttermAndCondetail(jObjectT);
                    }
                    RFQTermAndConDetail = dtTNC;
                    /*----------------------Sub Item ----------------------*/
                    DataTable dtSubItem = new DataTable();
                    dtSubItem.Columns.Add("item_id", typeof(string));
                    dtSubItem.Columns.Add("sub_item_id", typeof(string));
                    dtSubItem.Columns.Add("qty", typeof(string));
                    if (RFQ_Model.SubItemDetailsDt != null)
                    {
                        JArray jObject2 = JArray.Parse(RFQ_Model.SubItemDetailsDt);
                        for (int i = 0; i < jObject2.Count; i++)
                        {
                            DataRow dtrowItemdetails = dtSubItem.NewRow();
                            dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                            dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                            dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                            dtSubItem.Rows.Add(dtrowItemdetails);
                        }
                        ViewData["SubItemDetail"] = dtsubitemdetail(jObject2);
                    }

                    /*------------------Sub Item end----------------------*/
                    ViewData["TrmAndConRFQDetails"] = RFQTermAndConDetail;

                    DataTable dtAttachment = new DataTable();
                    var _RequestForQuotationattch = TempData["ModelDataattch"] as RequestForQuotationattch;
                    TempData["ModelDataattch"] = null;
                    if (RFQ_Model.attatchmentdetail != null)
                    {
                        if (_RequestForQuotationattch != null)
                        {
                            //if (Session["AttachMentDetailItmStp"] != null)
                            if (_RequestForQuotationattch.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = _RequestForQuotationattch.AttachMentDetailItmStp as DataTable;
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
                            if (RFQ_Model.AttachMentDetailItmStp != null)
                            {
                                dtAttachment = RFQ_Model.AttachMentDetailItmStp as DataTable;
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
                        JArray jObject1 = JArray.Parse(RFQ_Model.attatchmentdetail);
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
                                if (!string.IsNullOrEmpty(RFQ_Model.RFQ_no))
                                {
                                    dtrowAttachment1["id"] = RFQ_Model.RFQ_no;
                                }
                                else
                                {
                                    dtrowAttachment1["id"] = "0";
                                }
                                dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
                                dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
                                dtrowAttachment1["file_def"] = "Y";
                                dtrowAttachment1["comp_id"] = CompID;
                                dtAttachment.Rows.Add(dtrowAttachment1);
                            }
                        }
                        
                        //if (Session["TransType"].ToString() == "Update")
                        if (RFQ_Model.TransType == "Update")
                        {

                            string AttachmentFilePath = System.Web.HttpContext.Current.Server.MapPath("~") + "..\\Attachment\\" + PageName + "/";
                            if (Directory.Exists(AttachmentFilePath))
                            {
                                string ItmCode = string.Empty;
                                if (!string.IsNullOrEmpty(RFQ_Model.RFQ_no))
                                {
                                    ItmCode = RFQ_Model.RFQ_no;
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
                        //ViewData["AttachmentDetails"] = Attachments;
                    }


                    SaveMessage = requestForQuotation_ISERVICES.InsertUpdateRFQ(RFQHeader, RFQItemDetails,RFQSuppDetails, RFQDeleveryShedDetail, RFQTermAndConDetail, dtSubItem, Attachments);
                    if (SaveMessage == "DocModify")
                    {
                        RFQ_Model.Message = "DocModify";
                        RFQ_Model.BtnName = "Refresh";
                        RFQ_Model.Command = "Refresh";
                        TempData["ModelData"] = RFQ_Model;
                        return RedirectToAction("AddRequestForQuotationDetail");
                    }
                    else
                    {
                        string[] Data = SaveMessage.Split('-');
                        string RFQNo = Data[1];
                        string Message = Data[0];
                        if (Message == "DataNotFound")
                        {
                            var msg = "Data Not found" +" "+ RFQNo+" in "+PageName;
                            string path = Server.MapPath("~");
                            Errorlog.LogError_customsg(path, msg, "", "");
                            RFQ_Model.Message = Message;
                            return RedirectToAction("AddRequestForQuotationDetail");
                        }

                        if (Message == "Save")
                        {
                            string Guid = "";
                            if (_RequestForQuotationattch != null)
                            {
                                if (_RequestForQuotationattch.Guid != null)
                                {
                                    Guid = _RequestForQuotationattch.Guid;
                                }
                            }
                            string guid = Guid;
                            var comCont = new CommonController(_Common_IServices);
                            comCont.ResetImageLocation(CompID, branchID, guid, PageName, RFQNo, RFQ_Model.TransType, Attachments);

                            //string sourcePath = Server.MapPath("~/Attachment/" + PageName + "/");
                            //if (Directory.Exists(sourcePath))
                            //{
                            //    string[] filePaths = Directory.GetFiles(sourcePath, CompID + branchID + Guid + "_" + "*");
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
                            //                string RFQNo1 = RFQNo.Replace("/", "");
                            //                string img_nm = CompID + branchID + RFQNo1 + "_" + Path.GetFileName(DrItmNm).ToString();
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
                            RFQ_Model.Message = "Save";
                            RFQ_Model.Command = "Update";
                            RFQ_Model.RFQ_Number = RFQNo;
                            RFQ_Model.TransType = "Update";
                            RFQ_Model.AppStatus = "D";
                            RFQ_Model.BtnName = "BtnSave";
                            RFQ_Model.AttachMentDetailItmStp = null;
                            RFQ_Model.Guid = null;
                            //Session["Message"] = "Save";
                            //Session["Command"] = "Update";
                            //Session["RFQ_no"] = RFQNo;
                            //Session["TransType"] = "Update";
                            //Session["AppStatus"] = 'D';
                            //Session["BtnName"] = "BtnSave";
                            //Session["AttachMentDetailItmStp"] = null;
                            //Session["Guid"] = null;
                        }
                        return RedirectToAction("AddRequestForQuotationDetail");
                    }
                    

                }
                else
                {
                    string br_id = Session["BranchId"].ToString();
                    RFQ_Model.CreatedBy = userid;
                    string mac = Session["UserMacaddress"].ToString();
                    string system = Session["UserSystemName"].ToString();
                    string ip = Session["UserIP"].ToString();
                    string mac_id = "MAC:" + mac + ",SystemName:" + system + ",IP:" + ip;
                    SaveMessage = requestForQuotation_ISERVICES.RFQCancel(RFQ_Model, CompID, br_id, mac_id);
                    string MRSNo = SaveMessage.Substring(SaveMessage.IndexOf('-') + 1);
                   RFQ_Model.Message = "Cancelled";
                   RFQ_Model.Command = "Update";
                   RFQ_Model.TransType = "Update";
                   RFQ_Model.AppStatus = "D";
                   RFQ_Model.BtnName = "Refresh";
                    RFQ_Model.RFQ_Number = RFQ_Model.RFQ_no;
                    RFQ_Model.RFQ_date = RFQ_Model.RFQ_date;
                    //Session["Message"] = "Cancelled";
                    //Session["Command"] = "Update";
                    //Session["TransType"] = "Update";
                    //Session["AppStatus"] = 'D';
                    //Session["BtnName"] = "Refresh";
                    return RedirectToAction("AddRequestForQuotationDetail");
                }


            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                if (string.IsNullOrEmpty(SaveMessage))
                {
                    //if (Session["TransType"].ToString() == "Save")
                    if (RFQ_Model.TransType == "Save")
                    {
                        string Guid = "";
                        //if (Session["Guid"] != null)
                        if (RFQ_Model.Guid != null)
                        {
                            //Guid = Session["Guid"].ToString();
                            Guid = RFQ_Model.Guid;
                        }
                        var other = new CommonController(_Common_IServices);
                        other.DeleteTempFile(CompID+ branchID, PageName, Guid, Server);
                    }
                }
                throw ex;
            }
        }
        public DataTable dtitemdetail(JArray jObject)
        {
            DataTable dtItem = new DataTable();

            dtItem.Columns.Add("item_id", typeof(string));
            dtItem.Columns.Add("item_name", typeof(string));
            dtItem.Columns.Add("sub_item", typeof(string));
            dtItem.Columns.Add("uom_id", typeof(int));
            dtItem.Columns.Add("uom_name", typeof(string));
            dtItem.Columns.Add("rfq_qty", typeof(string));
            dtItem.Columns.Add("it_remarks", typeof(string));

            //JArray jObject = JArray.Parse(RFQ_Model.Itemdetails);

            for (int i = 0; i < jObject.Count; i++)
            {
                DataRow dtrowLines = dtItem.NewRow();

                dtrowLines["item_id"] = jObject[i]["ItemID"].ToString();
                dtrowLines["item_name"] = jObject[i]["ItemName"].ToString();
                dtrowLines["sub_item"] = jObject[i]["subitem"].ToString();
                dtrowLines["uom_id"] = jObject[i]["UOMID"].ToString();
                dtrowLines["uom_name"] = jObject[i]["UOM"].ToString();
                dtrowLines["rfq_qty"] = jObject[i]["RequQty"].ToString();
                dtrowLines["it_remarks"] = jObject[i]["ItemRemarks"].ToString();
                dtItem.Rows.Add(dtrowLines);
            }

            return dtItem;
        }

        public DataTable dtsuppdetail(JArray jObjectS)
        {
            DataTable dtSupp = new DataTable();

            dtSupp.Columns.Add("supp_id", typeof(int));
            dtSupp.Columns.Add("supp_name", typeof(string));
            dtSupp.Columns.Add("BillingAddress", typeof(string));
            dtSupp.Columns.Add("cont_email", typeof(string));
            dtSupp.Columns.Add("cont_num", typeof(string));
            dtSupp.Columns.Add("supp_type", typeof(string));

            //JArray jObject = JArray.Parse(RFQ_Model.Itemdetails);

            for (int i = 0; i < jObjectS.Count; i++)
            {
                DataRow dtrowLines = dtSupp.NewRow();

                dtrowLines["supp_id"] = Convert.ToInt32(jObjectS[i]["SuppID"].ToString());
                dtrowLines["supp_name"] = jObjectS[i]["supp_name"].ToString();
                dtrowLines["BillingAddress"] = jObjectS[i]["Address"].ToString();
                dtrowLines["cont_email"] = jObjectS[i]["SuppEmail"].ToString();
                dtrowLines["cont_num"] = jObjectS[i]["SuppContact"].ToString();
                dtrowLines["supp_type"] = jObjectS[i]["SuppType"].ToString();
                dtSupp.Rows.Add(dtrowLines);
            }

            return dtSupp;
        }
        public DataTable dtdeldetail(JArray jObjectD)
        {
            DataTable dtDelShed = new DataTable();
            
            dtDelShed.Columns.Add("item_id", typeof(string));
            dtDelShed.Columns.Add("item_name", typeof(string));
            dtDelShed.Columns.Add("sch_date", typeof(string));
            dtDelShed.Columns.Add("delv_qty", typeof(float));
           
            for (int i = 0; i < jObjectD.Count; i++)
                {
                    DataRow dtrowLines = dtDelShed.NewRow();

                    dtrowLines["item_id"] = jObjectD[i]["DelItemID"].ToString();
                    dtrowLines["item_name"] = jObjectD[i]["ItemName"].ToString();
                    dtrowLines["sch_date"] = jObjectD[i]["DelDate"].ToString();
                    dtrowLines["delv_qty"] = jObjectD[i]["DelQty"].ToString();
                    dtDelShed.Rows.Add(dtrowLines);
                }
            return dtDelShed;
        }
        public DataTable dttermAndCondetail(JArray jObjectT)
        {
            DataTable dtTNC = new DataTable();
            dtTNC.Columns.Add("term_desc", typeof(string));
            dtTNC.Columns.Add("sno", typeof(int));
            int sno = 1;
            for (int i = 0; i < jObjectT.Count; i++)
            {
                DataRow dtrowLines = dtTNC.NewRow();
                dtrowLines["term_desc"] = jObjectT[i]["TermAndCondition"].ToString();
                dtrowLines["sno"] = sno;
                dtTNC.Rows.Add(dtrowLines);
                sno += 1;
            }
            return dtTNC;
        }
        public DataTable dtsubitemdetail(JArray jObject2)
        {
            DataTable dtSubItem = new DataTable();
            dtSubItem.Columns.Add("item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_id", typeof(string));
            dtSubItem.Columns.Add("sub_item_name", typeof(string));
            dtSubItem.Columns.Add("qty", typeof(string));
           

            for (int i = 0; i < jObject2.Count; i++)
            {
                DataRow dtrowItemdetails = dtSubItem.NewRow();
                dtrowItemdetails["item_id"] = jObject2[i]["item_id"].ToString();
                dtrowItemdetails["sub_item_id"] = jObject2[i]["sub_item_id"].ToString();
                if (jObject2[i]["sub_item_name"] != null)
                {
                    dtrowItemdetails["sub_item_name"] = jObject2[i]["sub_item_name"].ToString();
                }
                else
                {
                    dtrowItemdetails["sub_item_name"] = "";
                }
                dtrowItemdetails["qty"] = jObject2[i]["qty"].ToString();
                dtSubItem.Rows.Add(dtrowItemdetails);
            }
            return dtSubItem;
        }
        /*--------------------------For SubItem Start--------------------------*/
        public ActionResult GetSubItemDetails(string Item_id, string SubItemListwithPageData, string IsDisabled
        , string Flag, string Status, string QtNo, string Doc_no, string Doc_dt)
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
                    if (Status == "D" || Status == "F" || Status == "")
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
                        dt = requestForQuotation_ISERVICES.GetSubItemDetailsAfterApprove(CompID, branchID, Item_id, Doc_no, Doc_dt, Flag).Tables[0];
                    }
                }
                else
                {
                    dt.Columns.Add("item_id", typeof(string));
                    dt.Columns.Add("sub_item_id", typeof(string));
                    dt.Columns.Add("sub_item_name", typeof(string));
                    dt.Columns.Add("Qty", typeof(string));

                    JArray arr = JArray.Parse(SubItemListwithPageData);
                    foreach (JObject item in arr.Children())//
                    {
                        DataRow dRow = dt.NewRow();
                        dRow["item_id"] = item.GetValue("item_id").ToString();
                        dRow["sub_item_id"] = item.GetValue("sub_item_id").ToString();
                        dRow["sub_item_name"] = item.GetValue("sub_item_name").ToString();
                        dRow["Qty"] = Convert.ToDecimal(IsNull(item.GetValue("qty").ToString(), "0")).ToString(ToFixDecimal(QtyDigit));
                        
                        dt.Rows.Add(dRow);
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

                //ViewBag.SubItemDetails = dt;
                //ViewBag.IsDisabled = IsDisabled;
                //ViewBag.Flag = Flag == "Quantity" ? Flag : "MTO";
                return View("~/Areas/Common/Views/Cmn_PartialSubItemDetail.cshtml", subitmModel);
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

        //public DataTable dtattachdetail(JArray jObject1)
        //{
        //    DataTable dtAttachment = new DataTable();
        //    dtAttachment.Columns.Add("id", typeof(string));
        //    dtAttachment.Columns.Add("file_name", typeof(string));
        //    dtAttachment.Columns.Add("file_path", typeof(string));
        //    dtAttachment.Columns.Add("file_def", typeof(char));
        //    dtAttachment.Columns.Add("comp_id", typeof(Int32));

        //    for (int i = 0; i < jObject1.Count; i++)
        //    {
        //        string flag = "Y";
        //        foreach (DataRow dr in dtAttachment.Rows)
        //        {
        //            string drImg = dr["file_name"].ToString();
        //            string ObjImg = jObject1[i]["file_name"].ToString();
        //            if (drImg == ObjImg)
        //            {
        //                flag = "N";
        //            }
        //        }
        //        if (flag == "Y")
        //        {

        //            DataRow dtrowAttachment1 = dtAttachment.NewRow();

        //            dtrowAttachment1["id"] = "0";
        //            dtrowAttachment1["file_path"] = jObject1[i]["file_path"].ToString();
        //            dtrowAttachment1["file_name"] = jObject1[i]["file_name"].ToString();
        //            dtrowAttachment1["file_def"] = "Y";
        //            dtrowAttachment1["comp_id"] = CompID;
        //            dtAttachment.Rows.Add(dtrowAttachment1);
        //        }
        //    }
        //    return dtAttachment;
        //}
        public JsonResult Upload(string title, string DocNo, string TransType)
        {

            try
            {
                RequestForQuotationattch _RequestForQuotationattch = new RequestForQuotationattch();
                var other = new CommonController(_Common_IServices);
                HttpFileCollectionBase Files = Request.Files;

                //string TransType = "";
                //string RFQNo = "";
                Guid gid = new Guid();
                gid = Guid.NewGuid();
                //if (Session["TransType"] != null)
                //if (TransType != null)
                //{
                //    TransType = Session["TransType"].ToString();
                //}
                //if (Session["rfq_no"] != null)
                //{
                //    RFQNo = Session["rfq_no"].ToString();
                //}
                if (TransType == "Save")
                {
                    DocNo = gid.ToString();
                }
                DocNo = DocNo.Replace("/", "");
                _RequestForQuotationattch.Guid = DocNo;
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchID = Session["BranchId"].ToString();
                }
                //string br_id = Session["BranchId"].ToString();
                getDocumentName(); /* To set Title*/
                string PageName = title.Replace(" ", "");

                DataTable dt = other.Upload(PageName, TransType, CompID + branchID, DocNo, Files, Server);
                if (dt.Rows.Count > 0)
                {
                    //Session["AttachMentDetailItmStp"] = dt;
                    _RequestForQuotationattch.AttachMentDetailItmStp = dt;
                }
                else
                {
                    //Session["AttachMentDetailItmStp"] = null;
                    _RequestForQuotationattch.AttachMentDetailItmStp = null;
                }
                TempData["ModelDataattch"] = _RequestForQuotationattch;
                return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("Error");
            }
        }
        public ActionResult getDetailBySourceDocumentNo(string SourDocumentNo)
        {
            try
            {
                JsonResult DataRows = null;
                //RequestForQuotation_Model RFQ_Model;
               // List<ItemDetails> _ItemDetailsList = new List<ItemDetails>();
               //  List<DocumentNumber> _DocumentNumberList = new List<DocumentNumber>();
                if (Session["CompId"] != null)
                {
                    CompID = Session["CompId"].ToString();

                }
                string BrchID = Session["BranchId"].ToString();
                DataSet ds = requestForQuotation_ISERVICES.getDetailBySourceDocumentNo(CompID, BrchID, SourDocumentNo);

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
        [HttpPost]
        public ActionResult getListPRNumber(RequestForQuotation_Model RFQ_Model)
        {
            JsonResult DataRows = null;
            string DocumentNumber = string.Empty;

            string Comp_ID = string.Empty;
            try
            {
                if (Session["CompId"] != null)
                {
                    Comp_ID = Session["CompId"].ToString();
                }
                string BrchID = string.Empty;
                if (Session["BranchId"] != null)
                {
                    BrchID = Session["BranchId"].ToString();
                }
                if (string.IsNullOrEmpty(RFQ_Model.reqNo))
                {
                    DocumentNumber = "0";
                }
                else
                {
                    DocumentNumber = RFQ_Model.reqNo;
                }
                DataSet PRNOList = requestForQuotation_ISERVICES.getPRDocumentNo(Comp_ID, BrchID, DocumentNumber);
                DataRows = Json(JsonConvert.SerializeObject(PRNOList));/*Result convert into Json Format for javasript*/
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return View("~/Views/Shared/Error.cshtml");
            }
            return DataRows;
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


        public ActionResult GetRFQTrackingDetail(string RFQ_no, string RFQ_dt)
        {
            try
            {
                //JsonResult DataRows = null;
                //string Comp_ID = string.Empty;
                //string BranchID = string.Empty;
                //if (Session["CompId"] != null)
                //{
                //    Comp_ID = Session["CompId"].ToString();
                //}
                //if (Session["BranchId"] != null)
                //{
                //    BranchID = Session["BranchId"].ToString();
                //}
                //DataSet result = requestForQuotation_ISERVICES.GetRFQTrackingDetail(Comp_ID, BranchID, RFQ_no, RFQ_dt);


                //DataRows = Json(JsonConvert.SerializeObject(result), JsonRequestBehavior.AllowGet);
                //ViewBag.POTrackingList = result.Tables[0];
                ViewBag.po_no = RFQ_no;
                ViewBag.PO_date = RFQ_dt;
                ////ViewBag.suppName = SuppName;
                //ViewBag.DocumentMenuId = DocumentMenuId;

                return View("~/Areas/ApplicationLayer/Views/Shared/_RequestForQuotation.cshtml");

            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return Json("ErrorPage");
            }
        }


        public ActionResult LoadData(string RFQ_no, string RFQ_dt)
        {
            try
            {
                var draw = Request.Form["draw"];
                var start = Convert.ToInt32(Request.Form["start"]);
                var length = Convert.ToInt32(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];
                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn1 = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                string Comp_ID = Session["CompId"]?.ToString() ?? string.Empty;
                string BranchID = Session["BranchId"]?.ToString() ?? string.Empty;





                // Fetch dataset from service
                DataSet result = requestForQuotation_ISERVICES.GetRFQTrackingDetail(Comp_ID, BranchID, RFQ_no, RFQ_dt);
                var dt = result.Tables[0];

                if (dt == null || dt.Rows.Count == 0)
                {
                    return Json(new { draw, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() }, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(sortColumn1) && !string.IsNullOrEmpty(sortColumnDir))
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = $"{sortColumn1} {sortColumnDir}";
                    dt = dv.ToTable();
                }
                var rows = dt.AsEnumerable();

                // Filtering
                if (!string.IsNullOrEmpty(searchValue))
                {
                    rows = rows.Where(row =>
                        (row["RFQ_Count_Per_Supplier"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["serialno"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["supp_name"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["rfq_no"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["rfq_dt"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["item_name"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["qt_no"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["qt_dt"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["item_rate"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["po_no"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["po_dt"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["ord_qty_base"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["po_item_price"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["item_net_val_bs"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["item_oc_amt"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["item_gr_val"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["item_tax_amt"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (row["uom_alias"]?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                    );
                }

                var recordsFiltered = rows.Count();
                var recordsTotal = dt.Rows.Count;

              



                // 1️⃣ Limit data only for the current page
                var pagedRows = rows.Skip(start).Take(length).ToList();

                // 2️⃣ Group only within the current visible page
                var pageGroups = pagedRows
                    .GroupBy(r => r["item_id"]?.ToString() ?? "")
                    .Select(g => new
                    {
                        ItemId = g.Key,
                        RowCount = g.Count(),
                        FirstIndex = pagedRows.IndexOf(g.First())
                    })
                    .ToList();

                // 3️⃣ Prepare final structured output
                int visibleSrNo = start; 
                // for continuous numbering
                int lastSrNo = 0;        // 🆕 to capture the last visible Sr.No.

                var data = pagedRows.Select((row, index) =>
                {
                   
                    string itemId = row["item_id"]?.ToString() ?? "";
                //    string serialno = row["serialno"]?.ToString() ?? "";
                    var group = pageGroups.FirstOrDefault(g => g.ItemId == itemId);

                    int.TryParse(row["serialno"]?.ToString(), out int serialNo);

                    string sr_no = "";
                    string itemName = "";
                    string uomAlias = "";
                    string rfqQty = "";
                    int rowSpan = 0;

                    if (group != null && group.FirstIndex == index) // show only for first row in visible page group
                    {

                        visibleSrNo++;
                        sr_no = visibleSrNo.ToString();
                        rowSpan = group.RowCount;

                        itemName = row["item_name"]?.ToString() ?? "";
                        uomAlias = row["uom_alias"]?.ToString() ?? "";
                        rfqQty = row["rfq_qty"]?.ToString() ?? "";

                       
                    }
                  

                    return new
                    {
                        
                    srno = sr_no,
                        RowSpan = rowSpan,
                        RFQ_Count_Per_Supplier = row["RFQ_Count_Per_Supplier"]?.ToString() ?? "",
                        supp_name = row["supp_name"]?.ToString() ?? "",
                        qt_no = row["qt_no"]?.ToString() ?? "",
                        qt_dt = row["qt_dt"]?.ToString() ?? "",
                        item_rate = row["item_rate"]?.ToString() ?? "",
                        po_no = row["po_no"]?.ToString() ?? "",
                        po_dt = row["po_dt"]?.ToString() ?? "",
                        ord_qty_base = row["ord_qty_base"]?.ToString() ?? "",
                        po_item_price = row["po_item_price"]?.ToString() ?? "",
                        item_net_val_bs = row["item_net_val_bs"]?.ToString() ?? "",
                        item_tax_amt = row["item_tax_amt"]?.ToString() ?? "",
                        item_oc_amt = row["item_oc_amt"]?.ToString() ?? "",                     
                        item_gr_val = row["item_gr_val"]?.ToString() ?? "",
                        item_name = itemName,
                        uom_alias = uomAlias,
                        rfq_qty = rfqQty,

                    };
                }).ToList();

                // 🆕 After the loop, `lastSrNo` holds the last serial number shown on this page
                Console.WriteLine("Last Sr.No. on this page: " + lastSrNo);






                return Json(new
                {
                    draw,
                    recordsTotal,
                    recordsFiltered,
                    data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json(new { draw = 0, recordsTotal = 0, recordsFiltered = 0, data = new List<object>() }, JsonRequestBehavior.AllowGet);
            }
        }


        public FileResult ExcelDownload(string searchValue, string RFQNumber, string RFQDate, int start = 0, 
            int length = 0, string sortColumnName = "", string sortDirection = "asc")
        {

            try
            {
                string Comp_ID = Session["CompId"]?.ToString() ?? string.Empty;
                string BranchID = Session["BranchId"]?.ToString() ?? string.Empty;

                // ✅ DataTables pagination & sorting info
                var draw = Request.Form["draw"];
              

                var sortColumnIndex = Request.Form["order[0][column]"];
              

                // ✅ Fetch data from service
                DataSet result = requestForQuotation_ISERVICES.GetRFQTrackingDetail(Comp_ID, BranchID, RFQNumber, RFQDate);
                var dt = result.Tables[0];
                var rows = dt.AsEnumerable();

                // ✅ Filtering
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    rows = rows.Where(r => r.ItemArray.Any(c =>
                        (c?.ToString() ?? "").IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0));
                }

                // ✅ Sorting (if DataTables sends column name)
                if (!string.IsNullOrEmpty(sortColumnName) && dt.Columns.Contains(sortColumnName))
                {
                    rows = sortDirection == "asc"
                        ? rows.OrderBy(r => r[sortColumnName])
                        : rows.OrderByDescending(r => r[sortColumnName]);
                }

                // ✅ Pagination (client-side like DataTables)
                var pagedRows = rows.Skip(start).Take(length).ToList();

                // ✅ Transform data for Excel
                var data = pagedRows.Select((row, index) => new
                {
                    srno = (start + index + 1).ToString(),
                    item_name = row["item_name"]?.ToString() ?? "",
                    uom_alias = row["uom_alias"]?.ToString() ?? "",
                    rfq_qty = row["rfq_qty"]?.ToString() ?? "",
                    supp_name = row["supp_name"]?.ToString() ?? "",
                    qt_no = row["qt_no"]?.ToString() ?? "",
                    qt_dt = row["qt_dt"]?.ToString() ?? "",
                    item_rate = row["item_rate"]?.ToString() ?? "",
                    po_no = row["po_no"]?.ToString() ?? "",
                    po_dt = row["po_dt"]?.ToString() ?? "",
                    ord_qty_base = row["ord_qty_base"]?.ToString() ?? "",
                    po_item_price = row["po_item_price"]?.ToString() ?? "",
                    item_net_val_bs = row["item_net_val_bs"]?.ToString() ?? "",
                    item_tax_amt = row["item_tax_amt"]?.ToString() ?? "",
                    item_oc_amt = row["item_oc_amt"]?.ToString() ?? "",
                    item_gr_val = row["item_gr_val"]?.ToString() ?? ""
                   
                }).ToList();

                // ✅ Convert to DataTable
                DataTable exportTable = ToDataTable(data);

                // ✅ Rename columns for Excel headers
                if (exportTable.Columns.Contains("srno")) exportTable.Columns["srno"].ColumnName = "Sr. No";
                if (exportTable.Columns.Contains("item_name")) exportTable.Columns["item_name"].ColumnName = "Item Name";
                if (exportTable.Columns.Contains("uom_alias")) exportTable.Columns["uom_alias"].ColumnName = "UOM";
                if (exportTable.Columns.Contains("rfq_qty")) exportTable.Columns["rfq_qty"].ColumnName = "RFQ Quantity";
                if (exportTable.Columns.Contains("supp_name")) exportTable.Columns["supp_name"].ColumnName = "Supplier Name";
                if (exportTable.Columns.Contains("qt_no")) exportTable.Columns["qt_no"].ColumnName = "Quotation Number";
                if (exportTable.Columns.Contains("qt_dt")) exportTable.Columns["qt_dt"].ColumnName = "Quotation Date";
                if (exportTable.Columns.Contains("item_rate")) exportTable.Columns["item_rate"].ColumnName = "Price Before Tax";
                if (exportTable.Columns.Contains("po_no")) exportTable.Columns["po_no"].ColumnName = "Order Number";
                if (exportTable.Columns.Contains("po_dt")) exportTable.Columns["po_dt"].ColumnName = "Order Date";
                if (exportTable.Columns.Contains("ord_qty_base")) exportTable.Columns["ord_qty_base"].ColumnName = "Order Quantity";
                if (exportTable.Columns.Contains("po_item_price")) exportTable.Columns["po_item_price"].ColumnName = "Order Price";
                if (exportTable.Columns.Contains("item_net_val_bs")) exportTable.Columns["item_net_val_bs"].ColumnName = "Value";
                if (exportTable.Columns.Contains("item_oc_amt")) exportTable.Columns["item_oc_amt"].ColumnName = "Other Charges";
                if (exportTable.Columns.Contains("item_gr_val")) exportTable.Columns["item_gr_val"].ColumnName = "Order Value";
                if (exportTable.Columns.Contains("item_tax_amt")) exportTable.Columns["item_tax_amt"].ColumnName = "Tax Amount";

                // ✅ Export to Excel
                var commonController = new CommonController(_Common_IServices);
                return commonController.ExportDatatableToExcel("RFQ Tracking Details", exportTable);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }

        /// <summary>
        /// Converts a list of objects into a DataTable (used for Excel export)
        /// </summary>
        private static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in props)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in props)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table;
        }


      



    }

}