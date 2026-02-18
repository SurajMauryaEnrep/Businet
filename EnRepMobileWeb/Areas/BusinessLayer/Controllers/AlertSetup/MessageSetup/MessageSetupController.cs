using EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.MessageSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.MessageSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.AlertSetup.MessageSetup
{
    public class MessageSetupController : Controller
    {
        string compId, userId, branchId, language, title = String.Empty;
        string DocumentMenuId = "103145001";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AlertSetup_IService _alertSetup_IService;
        public MessageSetupController(Common_IServices _Common_IServices, AlertSetup_IService alertSetup_IService)
        {
            this._Common_IServices = _Common_IServices;
            _alertSetup_IService = alertSetup_IService;
        }
        // GET: BusinessLayer/MessageSetup
        public ActionResult MessageSetup(AlertSetupModel asModel)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();
                if (Session["Language"] != null)
                    language = Session["Language"].ToString();

                CommonPageDetails();
                //Getting document list

                //asModel.DocList = GetDocuments(compId, branchId, language);
                //List<DocumentListModel> dlList = new List<DocumentListModel>();
                //asModel.DocList.Insert(0, new DocumentListModel() { docId = "0", docName = "All" });


                //Getting document events(doc status)
              

                string docId = "", events = "";
                if (asModel.docId != null && !string.IsNullOrEmpty(asModel.docId))
                    docId = asModel.docId;
                if (asModel.events != null && !string.IsNullOrEmpty(asModel.events))
                    docId = asModel.events;
                asModel.transactionType = "DEFAULT";

              

            //   DataTable dtAlertMsgList = _alertSetup_IService.GetAlertMsg(compId, branchId, "ALL", docId, events, language);

                Getdata(asModel, docId, "All", "ALL", events);
                //ViewBag.AlertMsgList = dtAlertMsgList;
                asModel.btnName = "BtnAddNew";
                asModel.command = "New";
                asModel.transactionType = "Save";
                asModel.title = title;
                //ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/BusinessLayer/Views/AlertSetup/MessageSetup/MessageSetupList.cshtml", asModel);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        private void Getdata(AlertSetupModel asModel,string docId, string ddlType, string alertType,string events)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
         
            DataSet GEtAlldata = _alertSetup_IService.GetAllData(compId, branchId, language, docId, ddlType, alertType, events);
            List<DocumentListModel> dlList = new List<DocumentListModel>();
            foreach (DataRow item in GEtAlldata.Tables[0].Rows)
            {
                DocumentListModel dlm = new DocumentListModel
                {
                    docId = item["doc_id"].ToString(),
                    docName = item["doc_Name"].ToString()
                };
                dlList.Add(dlm);              
            }
            dlList.Insert(0, new DocumentListModel() { docId = "0", docName = "All" });
            asModel.DocList = dlList;

            List<DocEventListModel> dlList1 = new List<DocEventListModel>();
            foreach (DataRow item in GEtAlldata.Tables[1].Rows)
            {
                DocEventListModel dlm = new DocEventListModel
                {
                    statusCode = item["status_code"].ToString().Trim(),
                    statusName = item["status_name"].ToString().Trim()
                };
                dlList1.Add(dlm);
            }

            if (ddlType == "All")
            {
                dlList1.Insert(0, new DocEventListModel() { statusCode = "0", statusName = "All" });
            asModel.DocEventList = dlList1;
            }
            else
            {
                dlList1.Insert(0, new DocEventListModel() { statusCode = "0", statusName = "--Select--" });
            }
            DataTable dtAlertMsgList = GEtAlldata.Tables[2];
            ViewBag.AlertMsgList = dtAlertMsgList;
            // asModel.DocEventList = GetDocumentEvents(asModel.docId, "All", "", compId, branchId);
        }
        public List<DocumentListModel> GetDocuments(string compId, string branchId, string language)
        {
            DataTable dtDocList = _alertSetup_IService.GetDocumentList(compId, branchId, language,"Internal");// Added by Nidhi on 26-04-2025
            List<DocumentListModel> dlList = new List<DocumentListModel>();
            foreach (DataRow item in dtDocList.Rows)
            {
                DocumentListModel dlm = new DocumentListModel
                {
                    docId = item["doc_id"].ToString(),
                    docName = item["doc_Name"].ToString()
                };
                dlList.Add(dlm);
            }
            return dlList;
        }
        //public List<DocEventListModel> GetDocumentAllEvents(string docId)
        //{
        //    DataTable dtDocList = _Common_IServices.GetStatusList(docId).Tables[0];
        //    List<DocEventListModel> dlList = new List<DocEventListModel>();
        //    foreach (DataRow item in dtDocList.Rows)
        //    {
        //        DocEventListModel dlm = new DocEventListModel
        //        {
        //            statusCode = item["status_code"].ToString(),
        //            statusName = item["status_name"].ToString()
        //        };
        //        dlList.Add(dlm);
        //    }
        //    if (dtDocList.Rows.Count < 1)
        //    {
        //        dlList.Insert(0, new DocEventListModel() { statusCode = "0", statusName = "ALL" });
        //    }
        //    return dlList;
        //}
        public List<DocEventListModel> GetDocumentEvents(string docId, string ddlType, string alertType, string compId, string brId)
        {
            DataTable dtDocList = _alertSetup_IService.GetDocumentEvents(docId, ddlType, alertType, compId, brId);
            List<DocEventListModel> dlList = new List<DocEventListModel>();
            foreach (DataRow item in dtDocList.Rows)
            {
                DocEventListModel dlm = new DocEventListModel
                {
                    statusCode = item["status_code"].ToString().Trim(),
                    statusName = item["status_name"].ToString().Trim()
                };
                dlList.Add(dlm);
            }

            if (ddlType == "All")
            {
                dlList.Insert(0, new DocEventListModel() { statusCode = "0", statusName = "All" });
            }
            else
            {
                dlList.Insert(0, new DocEventListModel() { statusCode = "0", statusName = "--Select--" });
            }
            return dlList;
        }
        public ActionResult SearchMessageSetupDetail(string docId, string events, string alertType)
        {
            try
            {
                AlertSetupModel asModel = new AlertSetupModel();
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();
                if (Session["Language"] != null)
                    language = Session["Language"].ToString();
                CommonPageDetails();
                //Getting document list
                asModel.DocList = GetDocuments(compId, branchId, language);
                List<DocumentListModel> dlList = new List<DocumentListModel>();
                asModel.DocList.Insert(0, new DocumentListModel() { docId = "0", docName = "All" });
                //Getting document events(doc status)
                asModel.DocEventList = GetDocumentEvents(asModel.docId, "All", alertType, compId, branchId);

                if (docId == "0" || string.IsNullOrEmpty(docId))
                    docId = "";
                if (events == "0" || string.IsNullOrEmpty(events))
                    events = "";

                DataTable dtAlertMsgList = _alertSetup_IService.GetAlertMsg(compId, branchId,alertType, docId, events, language);
                ViewBag.AlertMsgList = dtAlertMsgList;
                asModel.docId = docId;
                asModel.events = events;
                
                asModel.SearchStatus = "Search";
                //ViewBag.MenuPageName = getDocumentName();
                asModel.title = title;
                return View("~/Areas/BusinessLayer/Views/Shared/PartialMessageSetupList.cshtml", asModel);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult BindDocumentEvents(string documentId, string ddlType, string alertType, string compId, string brId)
        {
            try
            {
                //if (String.IsNullOrEmpty(compId) || compId == "0")
                //        compId = Session["CompId"].ToString();
                //if (brId == "0" || String.IsNullOrEmpty(brId.Trim()))
                //    branchId = Session["BranchId"].ToString();
                JsonResult result = null;
                DataTable dt = _alertSetup_IService.GetDocumentEvents(documentId, ddlType, alertType, compId, brId);
                result = Json(JsonConvert.SerializeObject(dt));
                return result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public JsonResult GetAlertFields(string documentId, string brId, string alertType)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                JsonResult result = null;
                DataTable dt = _alertSetup_IService.GetDocumentFieldName(compId, documentId);
                if (alertType != "mail")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["docFieldName"].ToString().Trim() == "Line Break")
                            dr.Delete();
                    }
                }
                dt.AcceptChanges();
                result = Json(JsonConvert.SerializeObject(dt));
                return result;
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return null;
            }
        }
        public ActionResult AddMessageSetupDetail()
        {
            AlertSetupModel asModel = new AlertSetupModel();

            asModel.message = "New";
            asModel.command = "New";
            asModel.appStatus = "D";
            asModel.transactionType = "Save";
            asModel.btnName = "BtnAddNew";
            TempData["AsModelData"] = asModel;
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("AddUpdMessageSetupDetail", "MessageSetup");
        }
        public ActionResult AddUpdMessageSetupDetail(AlertSetupModel asModel, string transType, string btnName, string command, string docId, string events, string alertType)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();
                if (Session["Language"] != null)
                    language = Session["Language"].ToString();

                AlertSetupModel tdModel = TempData["AsModelData"] as AlertSetupModel;
                if (tdModel == null)
                    tdModel = asModel;


                //Binding document list
                List<DocumentListModel> objDocList = GetDocuments(compId, branchId, language);
                objDocList.Insert(0, new DocumentListModel() { docId = "0", docName = "---Select---" });
                tdModel.DocList = objDocList;
                //Getting document events(doc status)
                string action = "Insert";
                if (transType == "Update")
                    action = "All";
                tdModel.DocEventList = GetDocumentEvents(asModel.docId, action, "", compId, branchId);
                if (!string.IsNullOrEmpty(transType))
                    tdModel.transactionType = transType;
                if (transType == "Update")
                {

                    if (alertType == "mail" || alertType == "email")
                        alertType = "mail";
                    else
                        alertType = "dashboard";
                    DataTable dt = _alertSetup_IService.GetAlertMsg(compId, branchId, alertType, docId, events, language);
                    if (dt.Rows.Count > 0)
                    {
                        tdModel.msg = dt.Rows[0]["msg"].ToString();
                        tdModel.events = dt.Rows[0]["event"].ToString().Trim();
                        tdModel.docId = dt.Rows[0]["doc_id"].ToString().Trim();
                        tdModel.CreateDate = dt.Rows[0]["create_dt"].ToString().Trim();
                        tdModel.createBy = dt.Rows[0]["user_nm"].ToString().Trim();
                        tdModel.amendedon = dt.Rows[0]["mod_dt"].ToString().Trim();
                        tdModel.amendedBy = dt.Rows[0]["mod_nm"].ToString().Trim();
                        tdModel.alertType = alertType;
                        tdModel.msgBody = dt.Rows[0]["mailBody"].ToString().Trim();
                        tdModel.msgFooter = dt.Rows[0]["mailfooter"].ToString().Trim();
                        tdModel.msgSubject = dt.Rows[0]["mailsubject"].ToString().Trim();
                        tdModel.msgHeader = dt.Rows[0]["mailheader"].ToString().Trim();
                        tdModel.msgAttachment = dt.Rows[0]["mailattachment"].ToString().Trim();
                    }
                }
                CommonPageDetails();
                //ViewBag.MenuPageName = getDocumentName();
                List<DocEventListModel> dlList = new List<DocEventListModel>();
                //Page refresh event 
                if (string.IsNullOrEmpty(tdModel.btnName))
                {
                    tdModel.btnName = "BtnAddNew";

                    dlList.Insert(0, new DocEventListModel() { statusCode = "0", statusName = "--Select--" });
                    tdModel.DocEventList = dlList;
                }
                if (string.IsNullOrEmpty(tdModel.btnName))
                {
                    tdModel.btnName = "BtnAddNew";
                    dlList.Insert(0, new DocEventListModel() { statusCode = "0", statusName = "--Select--" });
                    tdModel.DocEventList = dlList;
                }
                if (tdModel.btnName == "Refresh")
                {
                    dlList.Insert(0, new DocEventListModel() { statusCode = "0", statusName = "--Select--" });
                    tdModel.DocEventList = dlList;
                }
                tdModel.deleteCommand = "";
                tdModel.title = title;
                return View("~/Areas/BusinessLayer/Views/AlertSetup/MessageSetup/MessageSetup.cshtml", tdModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private string getDocumentName()
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["Language"] != null)
                    language = Session["Language"].ToString();

                string DocumentName = _Common_IServices.GetPageNameByDocumentMenuId(compId, DocumentMenuId, language);
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
        private void CommonPageDetails()
        {
            try
            {
                if (Session["CompId"] != null)
                {
                    compId = Session["CompId"].ToString();
                }
                if (Session["BranchId"] != null)
                {
                    branchId = Session["BranchId"].ToString();
                }
                if (Session["userid"] != null)
                {
                    userId = Session["userid"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
                DataSet ds = _Common_IServices.GetCommonPageDetails(compId, branchId, userId, DocumentMenuId, language);
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
        public ActionResult DeleteAlertMsg(AlertSetupModel deleteModel)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
            int result = _alertSetup_IService.DeleteAlertSetup(compId, branchId, deleteModel.docId, deleteModel.events, deleteModel.alertType);
            return RedirectToAction("AddUpdMessageSetupDetail");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitChangesOnAlertSetup(AlertSetupModel asModel, string command)
        {
            if (asModel.deleteCommand == "Delete")
                command = "Delete";
            switch (command)
            {
                case "AddNew":
                    AlertSetupModel asModelAdd = new AlertSetupModel();
                    asModelAdd.message = null;
                    asModelAdd.appStatus = "D";
                    asModelAdd.btnName = "BtnAddNew";
                    asModelAdd.transactionType = "Save";
                    asModelAdd.command = "New";
                    TempData["AsModelData"] = asModelAdd;
                    return RedirectToAction("AddUpdMessageSetupDetail", "MessageSetup");

                case "Edit":

                    asModel.message = null;
                    asModel.appStatus = "D";
                    asModel.btnName = "BtnEdit";
                    asModel.hdnSavebtn = "";
                    asModel.transactionType = "Update";
                    asModel.command = command;
                    string transType = "Update";
                    string btnName = "BtnEdit";
                    return RedirectToAction("AddUpdMessageSetupDetail", new { transType = transType, btnName = btnName, command = command, docId = asModel.docId, events = asModel.events, alertType = asModel.alertType });

                case "Delete":
                    AlertSetupModel asModelDelete = new AlertSetupModel();
                    asModelDelete.message = "Delete";
                    asModelDelete.appStatus = "DL";
                    asModelDelete.btnName = "Refresh";
                    asModelDelete.transactionType = "Refresh";
                    asModelDelete.command = command;
                    TempData["AsModelData"] = asModelDelete;
                    DeleteAlertMsg(asModel);
                    return RedirectToAction("AddUpdMessageSetupDetail");

                case "Save":

                    asModel.command = command;
                    if (string.IsNullOrEmpty(asModel.transactionType))
                        asModel.transactionType = command;
                    SaveAlertSetupDetails(asModel);
                    TempData["AsModelData"] = asModel;
                    transType = asModel.transactionType;
                    btnName = asModel.btnName;
                    string alertType = asModel.alertType;
                    return RedirectToAction("AddUpdMessageSetupDetail", new { transType = transType, btnName = btnName, command = command, docId = asModel.docId, events = asModel.events, alertType = alertType });

                case "Refresh":
                    AlertSetupModel asModelRefresh = new AlertSetupModel();
                    asModelRefresh.message = null;
                    asModelRefresh.btnName = "Refresh";
                    asModelRefresh.transactionType = "Save";
                    asModelRefresh.command = command;
                    TempData["AsModelData"] = asModelRefresh;
                    return RedirectToAction("AddUpdMessageSetupDetail");
                case "BacktoList":
                    return RedirectToAction("MessageSetup", "MessageSetup");
                default:
                    return new EmptyResult();
            }
        }
        [NonAction]
        public ActionResult SaveAlertSetupDetails(AlertSetupModel asModel)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();
                if (Session["Language"] != null)
                    language = Session["Language"].ToString();
                asModel.events = asModel.events.Trim();
                int cmpId = 0;
                asModel.crtOrModId = userId;
                int.TryParse(compId, out cmpId);
                asModel.compId = cmpId;
                int brId = 0;
                int.TryParse(branchId, out brId);
                asModel.brId = brId;
                asModel.action = asModel.transactionType;
                int result = 0;
                //Save Mail Msg Setup Detail
                //if (asModel.alertType.Contains("mail"))
                //{
                //    result = _alertSetup_IService.AddUpdateEmailAlertSetup(asModel);
                //}
                ////Save Dashboard Msg Setup Detail
                //else
                //{
                result = _alertSetup_IService.AddUpdateAlertSetup(asModel);
                //}
                string msg = "success";
                if (result > 0)
                    msg = "Success";
                if (msg == "Success")
                {
                    asModel.message = "Save";
                    asModel.command = "Update";
                    asModel.transactionType = "Update";
                    asModel.btnName = "BtnSave";
                }
                return RedirectToAction("AddUpdMessageSetupDetail");
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public ActionResult EditAlertSetup(string docId, string events, string alertType, string asData)
        {
            AlertSetupModel asModel = new AlertSetupModel();
            asModel.command = "Add";
            asModel.message = "New";
            asModel.transactionType = "Update";
            asModel.appStatus = "D";
            asModel.btnName = "BtnToDetailPage";
            asModel.alertType = alertType;
            TempData["AsData"] = asData;
            var transType = "Update";
            var btnName = "BtnToDetailPage";
            var command = "Add";
            return (RedirectToAction("AddUpdMessageSetupDetail", new { transType = transType, btnName = btnName, command = command, docId = docId, events = events, alertType = alertType }));
        }
    }

}







































































