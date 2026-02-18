using EnRepMobileWeb.Areas.BusinessLayer.Controllers.AlertSetup.MessageSetup;
using EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.DocumentSetup;
using EnRepMobileWeb.MODELS.BusinessLayer.AlertSetup.MessageSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.DocumentSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.BusinessLayer_ISERVICES.AlertSetup.MessageSetup;
using EnRepMobileWeb.SERVICES.ISERVICES.Common_IServices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace EnRepMobileWeb.Areas.BusinessLayer.Controllers.AlertSetup.DocumentSetup
{
    public class DocumentSetupController : Controller
    {
        string compId, userId, branchId, language, title = String.Empty;
        string DocumentMenuId = "103145005";
        Logging.ErrorLog Errorlog = new Logging.ErrorLog();
        Common_IServices _Common_IServices;
        AlertSetup_IService _asIservice;
        private readonly DocumentSetup_IService _docSetupService;
        public DocumentSetupController(Common_IServices _Common_IServices, AlertSetup_IService asIservice, DocumentSetup_IService docSetupService)
        {
            this._Common_IServices = _Common_IServices;
            _asIservice = asIservice;
            _docSetupService = docSetupService;
        }
        // GET: BusinessLayer/DocumentSetup
        public ActionResult DocumentSetup(DocumentSetupModel documentSetupModel)
        {
            //getDocumentName();
            CommonPageDetails();
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
            var alertController = new MessageSetupController(_Common_IServices, _asIservice);

            //Getting document list
            documentSetupModel.DocList = alertController.GetDocuments(compId, branchId, language);
            List<DocumentListModel> dlList = new List<DocumentListModel>();

            //documentSetupModel.EmailDocList = BindDocumentList("mail");
            //documentSetupModel.SmsDocList = BindDocumentList("sms");
            //documentSetupModel.WhatsappDocList = BindDocumentList("whatsapp");
            //documentSetupModel.DashboardDocList = BindDocumentList("dashboard");
            // documentSetupModel.DocList.Insert(0, new DocumentListModel() { docId = "0", docName = "--Select--" });

            //Getting document events(doc status)

            GetAllData(documentSetupModel);

            documentSetupModel.DocEventList = alertController.GetDocumentEvents("0", "All","", compId, branchId);

            //ViewBag.maildata = _docSetupService.GetAlertDocData(compId, branchId, "mail", "0", "0", "0");
            //ViewBag.smsdata = _docSetupService.GetAlertDocData(compId, branchId, "sms", "0", "0", "0");
            //ViewBag.whatsappdata = _docSetupService.GetAlertDocData(compId, branchId, "whatsapp", "0", "0", "0");
            //ViewBag.dashboarddata = _docSetupService.GetAlertDocData(compId, branchId, "dashboard", "0", "0", "0");
            //ViewBag.MenuPageName = getDocumentName();
            if (TempData["TempModelData"] != null)
            {
                DocumentSetupModel dsModel = TempData["TempModelData"] as DocumentSetupModel;
                documentSetupModel.msg = dsModel.msg;
                documentSetupModel.activeTab = dsModel.activeTab;
            }
            else
            {
                documentSetupModel.activeTab = "1";
            }            
            TempData["TempModelData"] = null;
            documentSetupModel.title = title;
            return View("~/Areas/BusinessLayer/Views/AlertSetup/DocumentSetup/DocumentSetup.cshtml", documentSetupModel);
        }
        private void GetAllData(DocumentSetupModel documentSetupModel)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
            DataSet table = _docSetupService.GetAlldata(compId, branchId, language);
            
            BindDropDown(documentSetupModel,"Email", table.Tables[1]);
            BindDropDown(documentSetupModel, "MainData", table.Tables[0]);

            ViewBag.maildata = table.Tables[2];
            ViewBag.smsdata = table.Tables[3];
            ViewBag.whatsappdata = table.Tables[4];
            ViewBag.dashboarddata = table.Tables[5];

        }
        private void BindDropDown(DocumentSetupModel documentSetupModel, string Flag,DataTable table)
        {
            List<DocumentListModel> doclist = new List<DocumentListModel>();
            if (table.Rows.Count > 0)
            {
                DataTable dt = table ;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DocumentListModel objDoc = new DocumentListModel
                    {
                        docId = dt.Rows[i]["DocId"].ToString(),
                        docName = dt.Rows[i]["DocName"].ToString()
                    };
                    doclist.Add(objDoc);
                }


            }

            doclist.Insert(0, new DocumentListModel() { docId = "0", docName = "--Select--" });
            if (Flag== "Email")
            {
                documentSetupModel.EmailDocList = doclist;
            }
            else
            {
                documentSetupModel.SmsDocList = doclist;
                documentSetupModel.WhatsappDocList = doclist;
                documentSetupModel.DashboardDocList = doclist;
            }
           
           
        }
        public List<DocumentListModel> BindDocumentList(string alertType)
        {
            List<DocumentListModel> doclist = new List<DocumentListModel>();
            doclist.Insert(0, new DocumentListModel() { docId = "0", docName = "--Select--" });
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
            DataSet ds = _docSetupService.GetDocumentList(compId, branchId, alertType, language);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DocumentListModel objDoc = new DocumentListModel
                    {
                        docId = dt.Rows[i]["DocId"].ToString(),
                        docName = dt.Rows[i]["DocName"].ToString()
                    };
                    doclist.Add(objDoc);
                }
            }
            return doclist;
        }
        public JsonResult BindDocumentEvents(string documentId, string ddlType)
        {
            string compId = "0", brId = "0";
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                brId = Session["BranchId"].ToString();
            DataSet ds = _docSetupService.GetDocumentEvents(compId, brId, ddlType, documentId);
            DataTable dt = new DataTable();
            if(ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
           return Json(JsonConvert.SerializeObject(dt));
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
        public JsonResult BindUserList(string docId, string userType, string alertType, string events)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                if (Session["userid"] != null)
                    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();

                JsonResult result = null;
                DataTable dt = _docSetupService.GetUsersList(compId, branchId, docId, userType, events, alertType);
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
        public int SaveDetails(DocumentSetupModel documentSetupModel)
        {
            try
            {
                if (Session["CompId"] != null)
                    compId = Session["CompId"].ToString();
                //if (Session["userid"] != null)
                //    userId = Session["userid"].ToString();
                if (Session["BranchId"] != null)
                    branchId = Session["BranchId"].ToString();
                int result = _docSetupService.SaveDocSetupDetails(compId, branchId, documentSetupModel.alertType, documentSetupModel.docId, documentSetupModel.eventId, documentSetupModel.Doctype, documentSetupModel.User);
                return result;
            }
            catch (Exception exc)
            {
                string path = Server.MapPath("~");
                Errorlog.LogError(path, exc);
                return 0;
            }
        }
        public ActionResult SubmitFormData(DocumentSetupModel documentSetupModel, string Command)
        {
            if (documentSetupModel.DeleteCommand == "Delete")
                Command = "Delete";
            switch (Command)
            {
                case "SaveMailData":
                    documentSetupModel.alertType = "mail";
                    documentSetupModel.docId = documentSetupModel.mailCommonData.docId;
                    documentSetupModel.eventId = documentSetupModel.mailCommonData.eventId;
                    documentSetupModel.Doctype = documentSetupModel.mailCommonData.Doctype;
                    documentSetupModel.User = documentSetupModel.mailCommonData.User;
                    int result = SaveDetails(documentSetupModel);
                    if (result > 0)
                        //TempData["msg"] = "success";
                        documentSetupModel.msg = "success";
                    documentSetupModel.activeTab = "1";
                    TempData["TempModelData"] = documentSetupModel;
                    break;
                case "SaveSmsData":
                    documentSetupModel.alertType = "sms";
                    documentSetupModel.docId = documentSetupModel.smsCommonData.docId;
                    documentSetupModel.eventId = documentSetupModel.smsCommonData.eventId;
                    documentSetupModel.Doctype = documentSetupModel.smsCommonData.Doctype;
                    documentSetupModel.User = documentSetupModel.smsCommonData.User;
                    result = SaveDetails(documentSetupModel);
                    if (result > 0)
                        documentSetupModel.msg = "success";
                    documentSetupModel.activeTab = "2";
                    TempData["TempModelData"] = documentSetupModel;
                    break;
                case "SaveWhatsappData":
                    documentSetupModel.alertType = "whatsapp";
                    documentSetupModel.docId = documentSetupModel.whatsappCommonData.docId;
                    documentSetupModel.eventId = documentSetupModel.whatsappCommonData.eventId;
                    documentSetupModel.Doctype = documentSetupModel.whatsappCommonData.Doctype;
                    documentSetupModel.User = documentSetupModel.whatsappCommonData.User;
                    result = SaveDetails(documentSetupModel);
                    if (result > 0)
                        //TempData["msg"] = "success";
                        documentSetupModel.msg = "success";
                    documentSetupModel.activeTab = "3";
                    TempData["TempModelData"] = documentSetupModel;
                    break;
                case "SaveDashboardData":
                    documentSetupModel.alertType = "dashboard";
                    documentSetupModel.docId = documentSetupModel.dashboardCommonData.docId;
                    documentSetupModel.eventId = documentSetupModel.dashboardCommonData.eventId;
                    documentSetupModel.Doctype = documentSetupModel.dashboardCommonData.Doctype;
                    documentSetupModel.User = documentSetupModel.dashboardCommonData.User;
                    result = SaveDetails(documentSetupModel);
                    if (result > 0)
                        //TempData["msg"] = "Deleted";
                        documentSetupModel.msg = "success";
                    documentSetupModel.activeTab = "4";
                    TempData["TempModelData"] = documentSetupModel;
                    break;
                case "Delete":
                    result = DeleteDocSetupData(documentSetupModel.rowId);
                    if (result > 0)
                        documentSetupModel.msg = "Deleted";
                    TempData["TempModelData"] = documentSetupModel;
                    break;
            }
            return RedirectToAction("DocumentSetup");
        }
        public int DeleteDocSetupData(string rowId)
        {
            try
            {
                return _docSetupService.DeleteAlertDocSetup(rowId);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult BindRcptType(string document, string events, string alertType)
        {
            if (Session["CompId"] != null)
                compId = Session["CompId"].ToString();
            if (Session["userid"] != null)
                userId = Session["userid"].ToString();
            if (Session["BranchId"] != null)
                branchId = Session["BranchId"].ToString();
            if (Session["Language"] != null)
                language = Session["Language"].ToString();
            List<UserType> rcptTypeList = new List<UserType>();
            UserType objUT = new UserType
            {
                value = 0,
                text = "---Select---"
            };
            rcptTypeList.Add(objUT);
            DataTable dttbl = _docSetupService.CheckIfDocumentTypeAlreadySet(compId, branchId, document, events, alertType);
            if (dttbl.Rows.Count < 1)
            {
                objUT = new UserType
                {
                    value = 1,
                    text = "Document User"
                };
                rcptTypeList.Add(objUT);
            }
            objUT = new UserType
            {
                value = 2,
                text = "Others"
            };
            rcptTypeList.Add(objUT);
            return Json(JsonConvert.SerializeObject(rcptTypeList));
        }
    }

}