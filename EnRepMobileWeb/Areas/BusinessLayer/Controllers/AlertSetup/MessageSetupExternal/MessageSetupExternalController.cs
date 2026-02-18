using EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.MessageSetupExternal;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.MessageSetupExternal;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.AlertSetup.MessageSetupExternal
{
    public class MessageSetupExternalController : Controller
    {
        string compId, userId, branchId, language, title = String.Empty;
        string DocumentMenuId = "103145003";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        MessageSetupExternal_IService _MessageSetupExternal_IServices;
        public MessageSetupExternalController(Common_IServices _Common_IServices, MessageSetupExternal_IService messageSetupExternal_IServices)
        {
            this._Common_IServices = _Common_IServices;
            this._MessageSetupExternal_IServices = messageSetupExternal_IServices;
        }
        // GET: BusinessLayer/MessageSetupExternal
        public ActionResult MessageSetupExternal(AlertMessageSetupExternal asModel)
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

                ViewBag.MenuPageName = getDocumentName();
                CommonPageDetails();
                string docId = "", events = "";
                if (asModel.docId != null && !string.IsNullOrEmpty(asModel.docId))
                    docId = asModel.docId;
                if (asModel.events != null && !string.IsNullOrEmpty(asModel.events))
                    docId = asModel.events;
                asModel.transactionType = "DEFAULT";
                Getdata(asModel, docId, "All", "ALL", events);
 
                asModel.btnName = "BtnAddNew";
                asModel.command = "New";
                asModel.transactionType = "Save";
                asModel.title = title;
                return View("~/Areas/BusinessLayer/Views/AlertSetup/MessageSetupExternal/MessageSetupExternalList.cshtml", asModel);
            }
            catch (Exception ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, ex);
                return Json("ErrorPage");
            }
            
        }
        public ActionResult AddMessageSetupDetail()
        {
            AlertMessageSetupExternal asModel = new AlertMessageSetupExternal();
            asModel.message = "New";
            asModel.command = "New";
            asModel.appStatus = "D";
            asModel.transactionType = "Save";
            asModel.btnName = "BtnAddNew";
            TempData["AsModelData"] = asModel;
            ViewBag.MenuPageName = getDocumentName();
            return RedirectToAction("AddMessageSetupExternalDetail", "MessageSetupExternal");
        }
        public ActionResult AddMessageSetupExternalDetail(AlertMessageSetupExternal asModel, string transType, string btnName, string command, string docId, string events, string alertType)
        {
            try
            {
                Session["Message"] = "New";
                Session["Command"] = "Add";
                Session["AppStatus"] = 'D';
                Session["TransType"] = "Save";
                Session["BtnName"] = "BtnAddNew";
                ViewBag.MenuPageName = getDocumentName();

                AlertMessageSetupExternal tdModel = TempData["AsModelData"] as AlertMessageSetupExternal;
                if (tdModel == null)
                    tdModel = asModel;

                List<DocumentListModel> objDocList = GetDocuments(compId, branchId, language);
                objDocList.Insert(0, new DocumentListModel() { docId = "0", docName = "---Select---" });
                tdModel.DocList = objDocList;

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
                    DataTable dt = _MessageSetupExternal_IServices.GetAlertMsg(compId, branchId, docId, events, language);
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
                return View("~/Areas/BusinessLayer/Views/AlertSetup/MessageSetupExternal/MessageSetupExternalDetail.cshtml", tdModel);
            }
            catch (Exception Ex)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, Ex);
                return null;
            }

        }
        public ActionResult SubmitChangesOnAlertSetup(AlertMessageSetupExternal asModel, string command)
        {
            if (asModel.deleteCommand == "Delete")
                command = "Delete";
            switch (command)
            {
                case "AddNew":
                    AlertMessageSetupExternal asModelAdd = new AlertMessageSetupExternal();
                    asModelAdd.message = null;
                    asModelAdd.appStatus = "D";
                    asModelAdd.btnName = "BtnAddNew";
                    asModelAdd.transactionType = "Save";
                    asModelAdd.command = "New";
                    TempData["AsModelData"] = asModelAdd;
                    return RedirectToAction("AddMessageSetupExternalDetail", "MessageSetupExternal");

                case "Edit":

                    asModel.message = null;
                    asModel.appStatus = "D";
                    asModel.btnName = "BtnEdit";
                    asModel.hdnSavebtn = "";
                    asModel.transactionType = "Update";
                    asModel.command = command;
                    string transType = "Update";
                    string btnName = "BtnEdit";
                    return RedirectToAction("AddMessageSetupExternalDetail", new { transType = transType, btnName = btnName, command = command, docId = asModel.docId, events = asModel.events, alertType = asModel.alertType });

                case "Delete":
                    AlertMessageSetupExternal asModelDelete = new AlertMessageSetupExternal();
                    asModelDelete.message = "Delete";
                    asModelDelete.appStatus = "DL";
                    asModelDelete.btnName = "Refresh";
                    asModelDelete.transactionType = "Refresh";
                    asModelDelete.command = command;
                    TempData["AsModelData"] = asModelDelete;
                    DeleteAlertMsg(asModel);
                    return RedirectToAction("AddMessageSetupExternalDetail");

                case "Save":

                    asModel.command = command;
                    if (string.IsNullOrEmpty(asModel.transactionType))
                        asModel.transactionType = command;
                    SaveAlertSetupDetails(asModel);
                    TempData["AsModelData"] = asModel;
                    transType = asModel.transactionType;
                    btnName = asModel.btnName;
                    string alertType = asModel.alertType;
                    return RedirectToAction("AddMessageSetupExternalDetail", new { transType = transType, btnName = btnName, command = command, docId = asModel.docId, events = asModel.events, alertType = alertType });

                case "Refresh":
                    AlertMessageSetupExternal asModelRefresh = new AlertMessageSetupExternal();
                    asModelRefresh.message = null;
                    asModelRefresh.btnName = "Refresh";
                    asModelRefresh.transactionType = "Save";
                    asModelRefresh.command = command;
                    TempData["AsModelData"] = asModelRefresh;
                    return RedirectToAction("AddMessageSetupExternalDetail");
                case "BacktoList":
                    return RedirectToAction("MessageSetupExternal", "MessageSetupExternal");
                default:
                    return new EmptyResult();
            }
        }
        public ActionResult SaveAlertSetupDetails(AlertMessageSetupExternal asModel)
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
                if (asModel.docId == "105104135115")
                {
                    asModel.events = "A";
                }
                else
                {
                    asModel.events = asModel.events.Trim();
                }
                int cmpId = 0;
                asModel.crtOrModId = userId;
                int.TryParse(compId, out cmpId);
                asModel.compId = cmpId;
                int brId = 0;
                int.TryParse(branchId, out brId);
                asModel.brId = brId;
                asModel.action = asModel.transactionType;
                int result = 0;
                result = _MessageSetupExternal_IServices.AddUpdateAlertSetup(asModel);
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
                return RedirectToAction("AddMessageSetupExternalDetail");
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<DocumentListModel> GetDocuments(string compId, string branchId, string language)
        {
            DataTable dtDocList = _MessageSetupExternal_IServices.GetDocumentList(compId, branchId, language, "External");
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
        public ActionResult MessageSetupExternalDetail()
        {
            try
            {
                ViewBag.MenuPageName = getDocumentName();
                return View("~/Areas/BusinessLayer/Views/AlertSetup/MessageSetupExternal/MessageSetupExternalDetail.cshtml");
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
                {
                    compId = Session["CompId"].ToString();
                }
                if (Session["Language"] != null)
                {
                    language = Session["Language"].ToString();
                }
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
        public List<DocEventListModel> GetDocumentEvents(string docId, string ddlType, string alertType, string compId, string brId)
        {
            DataTable dtDocList = _MessageSetupExternal_IServices.GetDocumentEvents(docId, ddlType, alertType, compId, brId);
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
        public JsonResult GetAlertFields(string documentId, string brId, string alertType)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                JsonResult result = null;
                DataTable dt = _MessageSetupExternal_IServices.GetDocumentFieldName(compId, documentId);
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
        private void Getdata(AlertMessageSetupExternal asModel, string docId, string ddlType, string alertType, string events)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();

            DataSet GEtAlldata = _MessageSetupExternal_IServices.GetAllData(compId, branchId, language, docId, events);
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
        public ActionResult EditAlertSetup(string docId, string events, string alertType, string asData)
        {
            AlertMessageSetupExternal asModel = new AlertMessageSetupExternal();
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
            return (RedirectToAction("AddMessageSetupExternalDetail", new { transType = transType, btnName = btnName, command = command, docId = docId, events = events, alertType = alertType }));
        }
        public JsonResult BindDocumentEvents(string documentId, string ddlType, string alertType, string compId, string brId)
        {
            try
            {
                JsonResult result = null;
                DataTable dt = _MessageSetupExternal_IServices.GetDocumentEvents(documentId, ddlType, alertType, compId, brId);
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
        public ActionResult DeleteAlertMsg(AlertMessageSetupExternal deleteModel)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
            int result = _MessageSetupExternal_IServices.DeleteAlertSetup(compId, branchId, deleteModel.docId, deleteModel.events, deleteModel.alertType);
            return RedirectToAction("AddMessageSetupExternalDetail");
        }
        public ActionResult SearchMessageSetupDetail(string docId, string events, string alertType)
        {
            try
            {
                AlertMessageSetupExternal asModel = new AlertMessageSetupExternal();
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

                DataTable dtAlertMsgList = _MessageSetupExternal_IServices.GetAlertMsg(compId, branchId, docId, events, language);
                ViewBag.AlertMsgList = dtAlertMsgList;
                asModel.docId = docId;
                asModel.events = events;

                asModel.SearchStatus = "Search";
                //ViewBag.MenuPageName = getDocumentName();
                asModel.title = title;
                return View("~/Areas/BusinessLayer/Views/Shared/PartialMessageSetupExternalList.cshtml", asModel);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}